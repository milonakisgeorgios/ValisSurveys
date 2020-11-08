using log4net;
using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Valis.Core;
using Valis.Core.Managers;

namespace ValisServer.Support.filebrowser
{
    /// <summary>
    /// 
    /// </summary>
    public class FetchFileEx : IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// This is the log4Net logger!
        /// </summary>
        static ILog Logger = LogManager.GetLogger(typeof(FetchFileEx));

        const int ChunkSize = 10240;
        const int TransmitFileUpperLimit = 262144/*256 Kb*/;
        const int CompressionLowerLimit = 262144/*256 Kb*/;


        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            InternalFileManager manager = new InternalFileManager();

            try
            {
                Guid? fileId = TryParseGuid(context, "fileid", required: false, defValue: null);

                if (fileId.HasValue == false)
                {
                    return;
                }

                //τραβάμε τα στοιχεία του αρχείου απο το σύστημα
                var file = manager.GetFileById(fileId.Value);
                if (file == null)
                {
                    return;
                }

                SendFile(context.Request, context.Response, manager, file);
            }
            catch (ArgumentException ex)
            {
                #region HTTP Error 400 Bad Request
                SendException(context, ex, HttpStatusCode.BadRequest);
                #endregion
            }
            catch (ThreadAbortException)
            {
                //catching the ThreadAbortExceptions from HttpResponse.End() calls...
            }
            catch (Exception ex)
            {
                #region HTTP Error 500 Internal Server Error
                SendException(context, ex, HttpStatusCode.InternalServerError);
                #endregion
            }
        }


        static void SendException(HttpContext context, Exception ex, HttpStatusCode status)
        {
            context.Response.ClearContent();
            context.Response.ClearHeaders();
            context.Response.ContentType = "text/html";
            context.Response.Charset = Encoding.UTF8.WebName;
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.TrySkipIisCustomErrors = true;
            context.Response.StatusCode = (int)status;                  //xhr.status
            context.Response.StatusDescription = status.ToString();     //xhr.statusText

            Logger.Error(context.Request.RawUrl, ex);

            var sb = new StringBuilder();
            sb.AppendFormat("<div class=\"requestWrapper\">Request: {0}</div>", context.Request.FilePath);
            if (ex != null)
            {
                sb.Append("<div class=\"errorContainer\">");
                while (ex != null)
                {
                    sb.AppendFormat("<span class=\"errorText\">{0}</span>", context.Server.HtmlEncode(ex.Message));
                    ex = ex.InnerException;
                }
                sb.Append("</div>");
            }

            //context.Response.Write("<html><head></head><body>");
            context.Response.Write(sb.ToString());
            //context.Response.Write("</body></html>");

            context.Response.Flush();
            context.Response.End();//throws a ThreadAbortException
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        Guid? TryParseGuid(HttpContext context, string attributeName, bool required, Guid? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Guid result = defValue.HasValue ? defValue.Value : default(Guid);
            if (!Guid.TryParse(context.Request.Params[attributeName], out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }


        void SendFile(HttpRequest request, HttpResponse response, InternalFileManager fileManager, VLFile file)
        {
            try
            {
                response.ClearHeaders();
                response.Clear();
                response.Cookies.Clear();
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.Charset = System.Text.UTF8Encoding.UTF8.WebName;

                String userAgent = HttpContext.Current.Request.Headers.Get("User-Agent");
                if (userAgent.Contains("MSIE"))
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(file.OriginalFileName.Replace(" ", "_"), System.Text.Encoding.UTF8));
                else
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + file.OriginalFileName.Replace(" ", "_"));

                response.ContentType = GetMimeType(file.OriginalFileName);


                if (!file.IsPhysicalFile || file.IsCompressed || file.IsEncrypted)
                {
                    SendFileFromCMS(request, response, fileManager, file);
                }
                else
                {
                    SendFileFromFS(request, response, fileManager, file);
                }
            }
            catch(Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
        }


        void SendFileFromCMS(HttpRequest request, HttpResponse response, InternalFileManager fileManager, VLFile file)
        {
            Stream contentStream = null;
            try
            {
                var binaryContent = fileManager.GetFileStream(file);

                contentStream = new MemoryStream(binaryContent, 0, binaryContent.Length, false, false);

                StreamContent(request, response, binaryContent.Length, contentStream);
            }
            catch (Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
            finally
            {
                if (contentStream != null) ((IDisposable)contentStream).Dispose();
            }
        }
        void SendFileFromFS(HttpRequest request, HttpResponse response, InternalFileManager fileManager, VLFile file)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            bool isGzipOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("gzip"));
            bool isDeflateOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("deflate"));


            Stream outputStream = null;
            bool disposeStream = false;
            try
            {
                string filepath = fileManager.GetFilePath(file);
                string filename = file.ManagedFileName;

                FileInfo fileInfo = new FileInfo(filepath);
                if (fileInfo.Exists)
                {
                    int len = (int)fileInfo.Length;

                    if (len <= TransmitFileUpperLimit)
                    {
                        response.TransmitFile(filepath);
                    }
                    else
                    {
                        StreamContent(request, response, len, System.IO.File.OpenRead(filepath));
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
            finally
            {
                if (outputStream != null && disposeStream) ((IDisposable)outputStream).Dispose();
            }
        }

        void StreamContent(HttpRequest request, HttpResponse response, int length, Stream stream)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            bool isGzipOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("gzip"));
            bool isDeflateOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("deflate"));

            Stream outputStream = null;
            bool disposeStream = false;

            try
            {
                int bytes = 0;

                if (isGzipOutput && length > CompressionLowerLimit)
                {
                    response.AppendHeader("Content-Encoding", "gzip");
                    outputStream = new GZipStream(HttpContext.Current.Response.OutputStream, CompressionMode.Compress);
                    disposeStream = true;
                }
                else if (isDeflateOutput && length > CompressionLowerLimit)
                {
                    response.AppendHeader("Content-Encoding", "deflate");
                    outputStream = new DeflateStream(HttpContext.Current.Response.OutputStream, CompressionMode.Compress);
                    disposeStream = true;
                }
                else
                {
                    outputStream = response.OutputStream;
                }

                byte[] buffer = new byte[1024];
                while (length > 0 && (bytes = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputStream.Write(buffer, 0, bytes);
                    length -= bytes;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
            finally
            {
                if (outputStream != null && disposeStream) ((IDisposable)outputStream).Dispose();
            }
        }

        string GetMimeType(string filename)
        {
            string mime = "application/octetstream";
            string ext = System.IO.Path.GetExtension(filename).ToLowerInvariant();
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (rk != null && rk.GetValue("Content Type") != null)
                mime = rk.GetValue("Content Type").ToString();
            return mime;
        }

    }
}