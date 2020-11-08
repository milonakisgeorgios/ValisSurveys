using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.filebrowser
{
    public class FetchFile : WebApiHandler
    {
        const int ChunkSize = 10240;
        const int TransmitFileUpperLimit = 262144/*256 Kb*/;
        const int CompressionLowerLimit = 262144/*256 Kb*/;


        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            Guid? fileId = TryParseGuid(context, "fileid", required: false, defValue: null);

            if (fileId.HasValue == false)
            {
                return;
            }

            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);

            //τραβάμε τα στοιχεία του αρχείου απο το σύστημα
            var file = surveyManager.GetFileById(fileId.Value);
            if (file == null)
            {
                return;
            }

            SendFile(context.Request, context.Response, surveyManager, file);
        }

        void SendFile(HttpRequest request, HttpResponse response, VLSurveyManager surveyManager, VLFile file)
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
                SendFileFromCMS(request, response, surveyManager, file);
            }
            else
            {
                SendFileFromFS(request, response, surveyManager, file);
            }
        }


        void SendFileFromCMS(HttpRequest request, HttpResponse response, VLSurveyManager surveyManager, VLFile file)
        {
            Stream contentStream = null;
            try
            {
                var binaryContent = surveyManager.GetFileStream(file);

                contentStream = new MemoryStream(binaryContent, 0, binaryContent.Length, false, false);

                StreamContent(request, response, binaryContent.Length, contentStream);
            }
            catch
            {

            }
            finally
            {
                if (contentStream != null) ((IDisposable)contentStream).Dispose();
            }
        }
        void SendFileFromFS(HttpRequest request, HttpResponse response, VLSurveyManager surveyManager, VLFile file)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            bool isGzipOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("gzip"));
            bool isDeflateOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("deflate"));


            Stream outputStream = null;
            bool disposeStream = false;
            try
            {
                string filepath = surveyManager.GetFilePath(file);
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
            catch
            {

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
            catch
            {
                throw;
            }
            finally
            {
                if (outputStream != null && disposeStream) ((IDisposable)outputStream).Dispose();
            }
        }

    }
}