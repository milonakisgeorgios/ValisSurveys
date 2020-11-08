using log4net;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Analysis
{
    /// <summary>
    /// All Summary Data exports organize survey results by question summaries. 
    /// The PDF export includes the exact charts, tables, and open-ended responses 
    /// you see in the Question Summaries tab.
    /// </summary>
    public class ExportSummaryPDF : IHttpHandler
    {
        const int ChunkSize = 10240;
        const int TransmitFileUpperLimit = 262144/*256 Kb*/;
        const int CompressionLowerLimit = 262144/*256 Kb*/;

        static ILog Logger = LogManager.GetLogger(typeof(ExportSummaryPDF));
        static readonly ValisSystem m_valisSystem = new ValisSystem();
        static bool checkPhantomJs = false;


        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                /*validate accesstoken:*/
                var scId = TryParseString(context, "ScId", true);
                var accessTokenId = Int32.Parse(scId);
                var accessToken = m_valisSystem.ValidateAccessToken(accessTokenId);

                /*grab call parameters:*/
                var surveyId = TryParseInt32(context, "surveyId");
                var viewId = TryParseGuid(context, "viewId");
                var textsLanguage = TryParseInt16(context, "textsLanguage", false, BuiltinLanguages.PrimaryLanguage);

                /*load requested view from system:*/
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                var view = surveyManager.GetViewById(viewId);
                if (view == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id,"View", viewId));
                }

                if (checkPhantomJs == false)
                {
                    /*check to see if does exist phantomjs.exe*/
                    var _fileName = Path.Combine(ValisSystem.Core.PhantomJs.Path, "phantomjs.exe");
                    FileInfo fileInfo = new FileInfo(_fileName);
                    if (fileInfo.Exists == false)
                    {
                        throw new VLException(string.Format("phantomjs.exe does not exist! (path = {0})", _fileName));
                    }
                    /*check to see if does exist rasterize.js*/
                    _fileName = Path.Combine(ValisSystem.Core.PhantomJs.Path, "rasterize.js");
                    fileInfo = new FileInfo(_fileName);
                    if (fileInfo.Exists == false)
                    {
                        throw new VLException(string.Format("rasterize.js does not exist! (path = {0})", _fileName));
                    }

                    checkPhantomJs = true;
                }
                

                /*check for the existance of the file:*/
                if (view.PdfReportIsValid)
                {
                    FileInfo fileInfo = new FileInfo(GetFilePath(view));
                    if (fileInfo.Exists == false)
                    {
                        view.PdfReportIsValid = false;
                    }
                }
                /*prepare pdf report if it is not ready:*/
                if (!view.PdfReportIsValid)
                {
                    if (string.IsNullOrWhiteSpace(view.PdfReportName) == false && string.IsNullOrWhiteSpace(view.PdfReportPath) == false)
                    {
                        /*erase previous report*/
                        try
                        {
                                FileInfo fileInfo = new FileInfo(GetFilePath(view));
                                if (fileInfo.Exists)
                                {
                                    fileInfo.Delete();
                                }

                        }
                        catch (Exception ex)
                        {
                            Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                        }
                        finally
                        {
                            view.PdfReportPath = null;
                            view.PdfReportName = null;
                        }
                    }

                    view.PdfReportCreationDt = DateTime.Now;
                    var pdfReportPath = Path.Combine(Path.Combine(view.Client.ToString(CultureInfo.InvariantCulture), view.Survey.ToString(CultureInfo.InvariantCulture)), "reports");
                    var pdfReportName = "report" + view.PdfReportCreationDt.Value.ToString("yyyyMMddHHmmss") + ".pdf";

                    view.PdfReportPath = pdfReportPath;
                    view.PdfReportName = pdfReportName;
                    var filename = GetFilePath(view);

                    var arguments = string.Format("rasterize.js \"http://{0}/clay/mysurveys/analysis/report.aspx?surveyid={1}\" \"{2}\" A4", ValisSystem.Core.ReportEngine.Host, surveyId, filename);
                    ExecutePhantomJs(arguments);

                    view.PdfReportIsValid = true;
                    view = surveyManager.UpdateView(view);
                }


                /*send pdf report:*/
                SendFile(context.Request, context.Response, view);
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

        public SecureString convertToSecureString(string strPassword)
        {
            var secureStr = new SecureString();
            if (strPassword.Length > 0)
            {
                foreach (var c in strPassword.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }
        void ExecutePhantomJs(string arguments)
        {

            try
            {
                ProcessStartInfo ProcessInfo;
                Process Process;

                ProcessInfo = new ProcessStartInfo();
                ProcessInfo.FileName = Path.Combine(ValisSystem.Core.PhantomJs.Path, "phantomjs.exe");
                ProcessInfo.Arguments = arguments;
                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;
                ProcessInfo.WorkingDirectory = ValisSystem.Core.PhantomJs.Path;

                Process = Process.Start(ProcessInfo);
                Process.WaitForExit();
            }
            catch(Exception ex)
            {
                var message = string.Format("ExecutePhantomJs() crashed!  ProcessInfo.FileName = {0}", Path.Combine(ValisSystem.Core.PhantomJs.Path, "phantomjs.exe"));
                Logger.Warn(message);
                message = string.Format("ExecutePhantomJs() crashed!  ProcessInfo.Arguments = {0}", arguments);
                Logger.Warn(message);
                message = string.Format("ExecutePhantomJs() crashed!  {0}", ex.Message);
                Logger.Error(message, ex);


                throw new VLException("ExecutePhantomJs() crashed");
            }
        }

        #region HandlerSupport
        void SendException(HttpContext context, Exception ex, HttpStatusCode status)
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
        string TryParseString(HttpContext context, string attributeName, bool required = true, string defValue = default(string))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            string result = defValue;
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }


            return context.Request.Params[attributeName];
        }
        Int32 TryParseInt32(HttpContext context, string attributeName, bool required = true, Int32 defValue = default(Int32))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int32 result = defValue;
            if (!Int32.TryParse(context.Request.Params[attributeName], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }
        Int16 TryParseInt16(HttpContext context, string attributeName, bool required = true, Int16 defValue = default(Int16))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int16 result = defValue;
            if (!Int16.TryParse(context.Request.Params[attributeName], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }
        protected Guid TryParseGuid(HttpContext context, string attributeName, bool required = true, Guid defValue = default(Guid))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Guid result = defValue;
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
        #endregion


        string GetFilePath(VLView view)
        {
            string rootDirectory = ValisSystem.Core.FileInventory.Path;

            return Path.Combine(Path.Combine(rootDirectory, view.PdfReportPath), view.PdfReportName);
        }

        void SendFile(HttpRequest request, HttpResponse response, VLView view)
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
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(view.PdfReportName.Replace(" ", "_"), System.Text.Encoding.UTF8));
                else
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + view.PdfReportName.Replace(" ", "_"));

                response.ContentType = "application/pdf";


                SendFileFromFS(request, response, view);
            }
            catch (Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
        }
        void SendFileFromFS(HttpRequest request, HttpResponse response, VLView view)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            bool isGzipOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("gzip"));
            bool isDeflateOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("deflate"));


            Stream outputStream = null;
            bool disposeStream = false;
            try
            {
                string filepath = GetFilePath(view);
                string filename = view.PdfReportName;

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

    }
}