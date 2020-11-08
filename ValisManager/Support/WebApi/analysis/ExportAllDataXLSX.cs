using log4net;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Analysis
{
    /// <summary>
    /// 'All Responses Data' exports organize survey results by respondent. The XLS 
    /// export includes a spreadsheet where each row contains the answers from a given respondent, 
    /// allowing you to do your own analysis in Excel.
    /// </summary>
    public class ExportAllDataXLSX : IHttpHandler
    {
        const int ChunkSize = 10240;
        const int TransmitFileUpperLimit = 262144/*256 Kb*/;
        const int CompressionLowerLimit = 262144/*256 Kb*/;

        static ILog Logger = LogManager.GetLogger(typeof(ExportSummaryPDF));
        static readonly ValisSystem m_valisSystem = new ValisSystem();


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

                /*load requested survey from system:*/
                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                var survey = surveyManager.ExportAllResponsesAsXls(surveyId);
                if (survey == null)
                {
                    throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Survey", surveyId));
                }

                if (survey.AllResponsesXlsxExportIsValid)
                {
                    /*send xlsl report:*/
                    SendFile(context.Request, context.Response, survey);
                }
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


        string GetFilePath(VLSurvey survey)
        {
            string rootDirectory = ValisSystem.Core.FileInventory.Path;

            return Path.Combine(Path.Combine(rootDirectory, survey.AllResponsesXlsxExportPath), survey.AllResponsesXlsxExportName);
        }


        void SendFile(HttpRequest request, HttpResponse response, VLSurvey survey)
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
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(survey.AllResponsesXlsxExportName.Replace(" ", "_"), System.Text.Encoding.UTF8));
                else
                    response.AppendHeader("Content-Disposition", "attachment; filename=" + survey.AllResponsesXlsxExportName.Replace(" ", "_"));

                response.ContentType = "application/vnd.ms-excel";


                SendFileFromFS(request, response, survey);
            }
            catch (Exception ex)
            {
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
                throw;
            }
        }
        void SendFileFromFS(HttpRequest request, HttpResponse response, VLSurvey survey)
        {
            string acceptEncoding = request.Headers["Accept-Encoding"];
            bool isGzipOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("gzip"));
            bool isDeflateOutput = (!string.IsNullOrWhiteSpace(acceptEncoding) && acceptEncoding.Contains("deflate"));


            Stream outputStream = null;
            bool disposeStream = false;
            try
            {
                string filepath = GetFilePath(survey);
                string filename = survey.AllResponsesXlsxExportName;

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