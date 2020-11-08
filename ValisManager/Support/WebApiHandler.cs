using log4net;
using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using Valis.Core;

namespace ValisManager.Support
{
    /// <summary>
    /// 
    /// </summary>
    public class WebApiHandler : IHttpHandler, IRequiresSessionState
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(WebApiHandler));

        static ValisSystem m_valisSystem = new ValisSystem();


        private static System.Random m_random = new Random();
        public static int NextRefId()
        {
            lock (m_random) return m_random.Next(10000000, 1000000000);
        } 


        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "application/json";
                context.Response.Charset = Encoding.UTF8.WebName;
                context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //context.Response.Cache.SetExpires( );
                //context.Response.Cache.SetLastModified( ); 
                context.Response.StatusCode = (int)HttpStatusCode.OK;


                if (context.Request.HttpMethod.Equals("GET"))
                {
                    var scId = TryParseString(context, "ScId", true);
                    var accessTokenId = Int32.Parse(scId);
                    var accessToken = m_valisSystem.ValidateAccessToken(accessTokenId);
                    ProcessGetRequestWrapped(accessToken, context);
                }
                else if (context.Request.HttpMethod.Equals("POST"))
                {
                    var scId = TryParseString(context, "ScId", true);
                    var accessTokenId = Int32.Parse(scId);
                    var accessToken = m_valisSystem.ValidateAccessToken(accessTokenId);
                    ProcessPostRequestWrapped(accessToken, context);
                }
                else
                {
                    #region HTTP Error 405 Method not allowed
                    SendException(context, null, HttpStatusCode.MethodNotAllowed);
                    #endregion
                }


                context.Response.Flush();
                context.ApplicationInstance.CompleteRequest();
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

        protected virtual void ProcessGetRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            throw new NotImplementedException();
        }
        protected virtual void ProcessPostRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            throw new NotImplementedException();
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


            VLException vlex = ex as VLException;
            Int32 refId = NextRefId();
            if(vlex != null)
            {
                refId = vlex.ReferenceId;
                Logger.Error(string.Format("RefId={0}, RawUrl={1}", refId, context.Request.RawUrl), ex);
            }
            else
            {
                Logger.Error(string.Format("RefId={0}, RawUrl={1}", refId, context.Request.RawUrl), ex);
            }

            var sb = new StringBuilder();
            sb.AppendFormat("<div class=\"requestWrapper\">Request: {0}</div>", context.Request.FilePath);
            if (ex != null)
            {
                sb.Append("<div class=\"errorContainer\">");
                sb.AppendFormat("<span class=\"errorText\">Reference number = {0} (required for support).</span><br /><br />", refId);
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



        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Double TryParseDouble(HttpContext context, string attributeName, bool required = true, Double defValue = default(Double))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Double result = defValue;
            if (!Double.TryParse(context.Request.Params[attributeName], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Double? TryParseDouble(HttpContext context, string attributeName, bool required, Double? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Double result = defValue.HasValue ? defValue.Value : default(Double);
            if (!Double.TryParse(context.Request.Params[attributeName], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Decimal TryParseDecimal(HttpContext context, string attributeName, bool required = true, Decimal defValue = default(Decimal))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Decimal result = defValue;
            if (!Decimal.TryParse(context.Request.Params[attributeName], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Decimal? TryParseDecimal(HttpContext context, string attributeName, bool required, Decimal? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Decimal result = defValue.HasValue ? defValue.Value : default(Decimal);
            if (!Decimal.TryParse(context.Request.Params[attributeName], System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Int64 TryParseInt64(HttpContext context, string attributeName, bool required = true, Int64 defValue = default(Int64))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int64 result = defValue;
            if (!Int64.TryParse(context.Request.Params[attributeName], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        protected Int64? TryParseInt64(HttpContext context, string attributeName, bool required, Int64? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int64 result = defValue.HasValue ? defValue.Value : default(Int64);
            if (!Int64.TryParse(context.Request.Params[attributeName], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Int32 TryParseInt32(HttpContext context, string attributeName, bool required = true, Int32 defValue = default(Int32))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Int32? TryParseInt32(HttpContext context, string attributeName, bool required, Int32? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int32 result = defValue.HasValue ? defValue.Value : default(Int32);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Int16 TryParseInt16(HttpContext context, string attributeName, bool required = true, Int16 defValue = default(Int16))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Int16? TryParseInt16(HttpContext context, string attributeName, bool required, Int16? defValue)
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Int16 result = defValue.HasValue ? defValue.Value : default(Int16);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Byte TryParseByte(HttpContext context, string attributeName, bool required = true, Byte defValue = default(Byte))
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            Byte result = defValue;
            if (!Byte.TryParse(context.Request.Params[attributeName], NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Boolean TryParseBoolean(HttpContext context, string attributeName, bool required = true, bool defValue = default(bool))
        {
            var _value = context.Request.Params[attributeName];

            if (string.IsNullOrEmpty(_value))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            if (String.Equals(_value, "on", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "off", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "0", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "True", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "False", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            _value = _value.Trim();
            if (String.Equals(_value, "on", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "off", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "1", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "0", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "True", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "False", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            if (String.Equals(_value, "yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            if (String.Equals(_value, "no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }


            if (required == true)
            {
                throw new ArgumentNullException(attributeName);
            }
            return defValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected string TryParseString(HttpContext context, string attributeName, bool required = true, string defValue = default(string))
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <returns></returns>
        protected Guid? TryParseGuid(HttpContext context, string attributeName, bool required, Guid? defValue)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="attributeName"></param>
        /// <param name="required"></param>
        /// <param name="defValue"></param>
        /// <param name="format">"dd-MMM-yyyy": 31-Jul-2012 js(d-M-yy), "dd/MM/yyyy":27/07/2012 js(dd/mm/yy)</param>
        /// <returns></returns>
        protected DateTime? TryParseDateTime(HttpContext context, string attributeName, bool required = true, DateTime? defValue = null, string format = "dd-MMM-yyyy")
        {
            if (string.IsNullOrEmpty(context.Request.Params[attributeName]))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            DateTime result = defValue.HasValue ? defValue.Value : default(DateTime);
            if (!Utilities.TryParse(context.Request.Params[attributeName], format, out result))
            {
                if (required == true)
                {
                    throw new ArgumentNullException(attributeName);
                }
                return defValue;
            }

            return result;
        }



        protected string GetMimeType(string filename)
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