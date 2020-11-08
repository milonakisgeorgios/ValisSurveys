using log4net;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;
using Valis.Core;

namespace ValisManager.clay.fileman.asp_net
{
    /// <summary>
    /// Summary description for connector
    /// <para>http://www.roxyfileman.com/api</para>
    /// </summary>
    public class connector : IHttpHandler, IRequiresSessionState
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(connector));
        #region support stuff
        static ValisSystem m_valisSystem = new ValisSystem();
        HttpContext m_context = null;

        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        HttpRequest Request
        {
            get
            {
                return m_context.Request;
            }
        }
        HttpResponse Response
        {
            get
            {
                return m_context.Response;
            }
        }

        HttpServerUtility Server
        {
            get
            {
                return m_context.Server;
            }
        }
        #endregion

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
        Int32? TryParseInt32(HttpContext context, string attributeName, bool required, Int32? defValue)
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





        public void ProcessRequest(HttpContext context)
        {
            m_context = context;

            try
            {
                var command = Request.QueryString["a"];
                var type = Request.QueryString["type"];
                var _scId = TryParseString(context, "scId", true);
                var surveyId = TryParseInt32(context, "survey", false, null);

                var accessTokenId = Int32.Parse(_scId);
                var accessToken = m_valisSystem.ValidateAccessToken(accessTokenId);

                Logger.InfoFormat("command={0}, type={1}, scId={2}, surveyId={3}", command, type, _scId, surveyId);
                ProcessRequestImplementation(accessToken, surveyId, command);
            }
            catch(Exception ex)
            {
                Logger.Error(context.Request.RawUrl, ex);
            }
        }

        void ProcessRequestImplementation(VLAccessToken accesToken, Int32? surveyId, string command)
        {
            var surveyManager = VLSurveyManager.GetAnInstance(accesToken);
            var survey = surveyManager.GetSurveyById(surveyId.Value);

            switch (command)
                {
                    case "CREATEDIR":
                        break;
                    case "DELETEDIR":
                        break;
                    case "MOVEDIR":
                        break;
                    case "COPYDIR":
                        break;
                    case "RENAMEDIR":
                        break;
                    case "FILESLIST":
                        {
                            StringBuilder writer = new StringBuilder();
                            GetFilesList(surveyManager, survey, writer);
                            SendResponse(this.Response, writer.ToString());
                        }
                        break;
                    case "UPLOAD":
                        {
                            StringBuilder writer = new StringBuilder();
                            UploadFile(surveyManager, survey, writer);
                            SendResponse(this.Response, writer.ToString());
                        }
                        break;
                    case "DOWNLOAD":
                        break;
                    case "DOWNLOADDIR":
                        break;
                    case "DELETEFILE":
                        {
                            Guid? fileId = TryParseGuid(m_context, "fileid", required: false, defValue: null);

                            if (fileId.HasValue == false)
                            {
                                return;
                            }

                            StringBuilder writer = new StringBuilder();
                            DeleteFile(surveyManager, survey, fileId.Value, writer);
                            SendResponse(this.Response, writer.ToString());
                        }
                        break;
                    case "MOVEFILE":
                        break;
                    case "COPYFILE":
                        break;
                    case "RENAMEFILE":
                        break;
                    case "GENERATETHUMB":
                        break;
                    case "DIRLIST":
                    default://DIRLIST
                        {
                            StringBuilder writer = new StringBuilder();
                            GetDirList(surveyManager, survey, writer);
                            SendResponse(this.Response, writer.ToString());
                        }
                        break;
                }
        }



        #region CommandHandlers
        void GetDirList(VLSurveyManager surveyManager, VLSurvey survey, StringBuilder sb)
        {
            var files = surveyManager.GetFiles(survey);

            sb.Append("[");

            //[
            //{"p":"/fileman/Uploads","f":"6","d":"5"},
            //{"p":"/fileman/Uploads/cccccccccc","f":"0","d":"0"},
            //{"p":"/fileman/Uploads/Documents","f":"2","d":"1"},
            //{"p":"/fileman/Uploads/Documents/Wide","f":"5","d":"0"},
            //{"p":"/fileman/Uploads/FLV","f":"0","d":"0"},
            //{"p":"/fileman/Uploads/Images","f":"5","d":"3"},{"p":"/fileman/Uploads/Images/Horizontal","f":"7","d":"0"},{"p":"/fileman/Uploads/Images/Vertical","f":"8","d":"0"},{"p":"/fileman/Uploads/Images/Wide","f":"5","d":"0"},{"p":"/fileman/Uploads/Manuals","f":"0","d":"2"},{"p":"/fileman/Uploads/Manuals/Mysql","f":"0","d":"0"},{"p":"/fileman/Uploads/Manuals/PHP","f":"0","d":"0"}
            //]
            sb.Append("{");
            //sb.AppendFormat("\"p\":\"{0}\",\"f\":\"{1}\",\"d\":\"0\"", survey.ShowTitle, files.Count);
            sb.AppendFormat("\"p\":\"survey\",\"f\":\"{0}\",\"d\":\"0\"", files.Count);
            sb.Append("}");

            sb.Append("]");
        }
        void GetFilesList(VLSurveyManager surveyManager, VLSurvey survey, StringBuilder sb)
        {
            var files = surveyManager.GetFiles(survey);


            sb.Append("[");

            //{"p":"/fileman/Uploads/404.jpg","s":"23073","t":"1394898074","w":"564","h":"434"},
            //{"p":"/fileman/Uploads/06316_148.jpg","s":"99318","t":"1394898075","w":"400","h":"216"},
            bool _comma = false;
            foreach(var file in files)
            {
                var url = string.Format("{0}services/api/filebrowser/FetchFileEx?fileid={1}", Request.ApplicationPath, file.FileId);

                if(_comma)
                {
                    sb.Append(",");
                }
                sb.Append("{");
                sb.AppendFormat("\"fileid\":\"{0}\", ", file.FileId);   //p - path of the file - absolute to the document root
                sb.AppendFormat("\"p\":\"{0}\", ",  Server.HtmlEncode(file.ManagedFileName));   //p - path of the file - absolute to the document root
                sb.AppendFormat("\"s\":\"{0}\", ", file.Size);   //s - file size in bytes
                sb.AppendFormat("\"t\":\"{0}\", ", Utility.DatetimeToUnixTime(file.CreateDT));   //st - modification time of the file - unix timestamp
                sb.AppendFormat("\"url\":\"{0}\", ", url);   //
                if(file.IsImage)
                {
                    sb.AppendFormat("\"w\":\"{0}\", ", file.Width.HasValue ? file.Width.Value : 0);   //w - if the file is image, the width of the image
                    sb.AppendFormat("\"h\":\"{0}\" ", file.Height.HasValue ? file.Height.Value : 0);   //h - if the file is image, the height of the image
                }
                sb.Append("}");
                _comma = true;
            }

            sb.Append("]");
        }

        void UploadFile(VLSurveyManager surveyManager, VLSurvey survey, StringBuilder sb)
        {
            try
            {
                HttpPostedFile oFile = Request.Files[0];
                if (oFile == null)
                    throw new VLException("Κανένα αρχείο δεν έγινε upload!");

                int fileLength = oFile.ContentLength;
                if (fileLength <= 0)
                    throw new VLException("To αρχείο είχε μηδενικό μέγεθος!");


                string postedFileName = Path.GetFileName(oFile.FileName);
                //Πρέπει να μεταφέρουμε το αρχείο στον EcmsFileManager.
                //Για να το κάνουμε αυτό το μεταφέρουμε στην μνήμη
                System.Byte[] mem = new byte[fileLength];
                Stream postedStream = oFile.InputStream;
                postedStream.Read(mem, 0, fileLength);


                VLFile file = surveyManager.AssignFile(survey.SurveyId, mem, postedFileName);
                sb.Append("{\"res\":\"ok\", \"msg\":\"\"}");
            }
            catch(Exception ex)
            {
                sb.AppendFormat("{{\"res\":\"error\", \"msg\":\"{0}\"}}", ex.Message);
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
            }

        }

        void DeleteFile(VLSurveyManager surveyManager, VLSurvey survey, Guid fileId, StringBuilder sb)
        {
            try
            {
                surveyManager.Removefile(fileId);


                sb.Append("{\"res\":\"ok\", \"msg\":\"\"}");
            }
            catch(Exception ex)
            {
                sb.AppendFormat("{{\"res\":\"error\", \"msg\":\"{0}\"}}", ex.Message);
                Logger.Error(HttpContext.Current.Request.RawUrl, ex);
            }
        }
        #endregion

        void SendResponse(HttpResponse response, string json)
        {
            response.ClearHeaders();
            response.Clear();

            response.ContentType = "application/json";
            response.Charset = Encoding.UTF8.WebName;
            response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.CacheControl = "no-cache";

            response.Write(json);
        }

    }
}