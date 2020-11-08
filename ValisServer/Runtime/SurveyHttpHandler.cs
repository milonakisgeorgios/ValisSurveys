using log4net;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Valis.Core;

namespace ValisServer.Runtime
{
    /// <summary>
    /// This is the base class for all surveys handlers
    /// </summary>
    public abstract class SurveyHttpHandler : IHttpHandler
    {
        /// <summary>
        /// This is the log4Net logger!
        /// </summary>
        protected ILog Logger;


        public bool IsReusable { get { return true; } }

        /// <summary>
        /// This is our SurveyManager
        /// </summary>
        protected VLSurveyManager SurveyManager { get; set; }
        /// <summary>
        /// This is out SystemManager
        /// </summary>
        protected VLSystemManager SystemManager { get; set; }


        /// <summary>
        /// 
        /// </summary>
        protected RuntimeRequestType RequestType { get; set; }
        /// <summary>
        /// The survey's language to be rendered
        /// </summary>
        protected VLLanguage Language { get; set; }
        /// <summary>
        /// The selected survey, which we want to display
        /// </summary>
        protected VLSurvey Survey { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected VLSurveyTheme SurveyTheme { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected VLCollector Collector { get; set; }
        /// <summary>
        /// Αυτός είναι ο Recipient για τον οποίο τρέχει το survey.
        /// <para>Μπορεί να είναι πραγματικός recipient ή ένας virtual Recipient!</para>
        /// <para>Στην περίπτωση manual καταχώρησης απο webLink τότε έχει τιμή null!</para>
        /// </summary>
        protected VLRecipient Recipient { get; set; }
        /// <summary>
        /// The current runtime session
        /// </summary>
        protected VLRuntimeSession RuntimeSession { get; set; }



        /// <summary>
        /// Gets the System.Web.HttpRequest object for the current HTTP request.
        /// </summary>
        protected HttpRequest Request { get; set; }
        /// <summary>
        /// Gets the System.Web.HttpResponse object for the current HTTP response.
        /// </summary>
        protected HttpResponse Response { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected HttpContext Context { get; set; }


        public static string GetLanguageThumbnail(short languageId)
        {
            switch (languageId)
            {
                case 33: return "/Content/flags/en.png";//English
                case 38: return "/Content/flags/fr.png";//French
                case 42: return "/Content/flags/de.png";//German
                case 43: return "/Content/flags/el.png";//Greek
                case 60: return "/Content/flags/it.png";//Italian
                case 99: return "/Content/flags/ro.png";//Romanian
                case 101: return "/Content/flags/ru.png";//Russian
                case 117: return "/Content/flags/es.pngf";//Spanish
            }
            return string.Empty;
        }
        /// <summary>
        /// 
        /// </summary>
        public SurveyHttpHandler()
        {
            this.Logger = LogManager.GetLogger(this.GetType());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                this.Request = context.Request;
                this.Response = context.Response;
                this.Context = context;

                this.SurveyManager = ((ValisHttpApplication)context.ApplicationInstance).SurveyManager;
                this.SystemManager = ((ValisHttpApplication)context.ApplicationInstance).SystemManager;


                this.RequestType = (RuntimeRequestType)Context.Items["RequestType"];
                this.Language = (VLLanguage)context.Items["language"];
                this.Survey = (VLSurvey)context.Items["survey"];
                this.Collector = (VLCollector)context.Items["collector"];
                this.Recipient = (VLRecipient)context.Items["recipient"];
                this.RuntimeSession = (VLRuntimeSession)context.Items["runtimeSession"];
                this.SurveyTheme = SurveyManager.GetSurveyThemeById(this.Survey.Theme);


                #region Debug Info
                {
                    if(this.RuntimeSession != null)
                    {
                        foreach (var key in this.RuntimeSession.Keys)
                        {
                            var message = string.Format("BEFORE: RuntimeSession[{0}] = {1}", key, this.RuntimeSession[key]);
                            System.Diagnostics.Debug.WriteLine(message);
                        }
                    }
                }
                #endregion


                ProcessRequestImplementation();


                #region Debug Info
                {
                    if (this.RuntimeSession != null)
                    {
                        foreach (var key in this.RuntimeSession.Keys)
                        {
                            var message = string.Format("AFTER: RuntimeSession[{0}] = {1}", key, this.RuntimeSession[key]);
                            System.Diagnostics.Debug.WriteLine(message);
                        }
                    }
                }
                #endregion
            }
            catch(Exception ex)
            {
                Logger.Error(context.Request.RawUrl, ex);
                ShowCannotExecutePage(9999, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void ProcessRequestImplementation();


        /// <summary>
        /// Κάνει render το περιεχόμενο του head της δυναμικής σελίδας του Html
        /// <para>Θέτει τον τίτλο της σελίδας, κάνει include την jquery, jquery-ui και τα CSS του SurveyTheme</para>
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderHtmlHead(HtmlTextWriter writer)
        {
            //html title
            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            HttpUtility.HtmlEncode(this.Survey.Title, writer);
            writer.RenderEndTag();
            //Includes
            writer.WriteLine("<script src=\"/Scripts/jquery-1.11.1.min.js\" type=\"text/javascript\"></script>");
            writer.WriteLine("<script src=\"/Scripts/jquery-ui-1.10.4.custom.min.js\" type=\"text/javascript\"></script>");
            writer.WriteLine("<script src=\"/Scripts/survey.js\" type=\"text/javascript\"></script>");

            writer.WriteLine("<link href=\"/content/ui-lightness/jquery-ui-1.10.4.custom.min.css\" rel=\"stylesheet\" />");
            //Stylesheet
            writer.WriteLine("<style type=\"text/css\">");
            writer.WriteLine(this.SurveyTheme.RtCSS);
            writer.WriteLine("</style>");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected void RenderLanguageSelector(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "languageSelector");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "languageSelector");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                foreach(var item in this.Survey.SupportedLanguages)
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, string.Format("languageSelectorImg_{0}", item.TwoLetterISOCode));
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, GetLanguageThumbnail(item.LanguageId));
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, string.Format("OnLanguageSelect('{0}','{1}','{2}','{3}')", item.LanguageId, item.TwoLetterISOCode, this.Language.LanguageId, this.Language.TwoLetterISOCode));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);
                    writer.RenderEndTag();
                }
            }
            writer.RenderEndTag();
        }


        /// <summary>
        /// 
        /// </summary>
        protected void ShowCannotExecutePage(Int32 errorCode, string errorMessage)
        {
            var responseHtml = new StringBuilder(this.SurveyTheme.RtHtml);


            var _htmlBuffer = new StringBuilder();
            var htmlWriter = new HtmlTextWriter(new StringWriter(_htmlBuffer));
            {
                RenderHtmlHead(htmlWriter);
                responseHtml.Replace("#@HTMLHEAD", _htmlBuffer.ToString());
            }
            //HEADER
            {
                if (Survey.ShowSurveyTitle && !string.IsNullOrWhiteSpace(this.Survey.HeaderHtml))
                {
                    responseHtml.Replace("#@SURVEYHEADER", this.Survey.HeaderHtml);
                }
                else
                {
                    responseHtml.Replace("#@SURVEYHEADER", string.Empty);
                }

                responseHtml.Replace("#@SURVEYLANGUAGESELECTOR", string.Empty);
            }
            //SURVEYBODY
            {
                _htmlBuffer.Clear();
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "surveyBody");
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                htmlWriter.Write("<table style=\"width:100%; margin-top: 120px;\"><tbody>");
                htmlWriter.Write("<tr><td align=\"center\">This survey cannot be executed. Please contact the author of this survey for further assistance.</td></tr>");
                htmlWriter.Write(string.Format("<tr><td align=\"center\">ErrorCode = {0}</td></tr>", errorCode));
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    htmlWriter.Write(string.Format("<tr><td align=\"center\">ErrorMessage = {0}</td></tr>", HttpUtility.HtmlEncode(errorMessage)));
                }
                htmlWriter.Write("</tbody></table>");
                htmlWriter.RenderEndTag();
                responseHtml.Replace("#@SURVEYBODY", _htmlBuffer.ToString());
            }
            //NAVIGATION
            {
                responseHtml.Replace("#@SURVEYNAVIGATION", string.Empty);
            }
            //FOOTER
            {
                if (!string.IsNullOrWhiteSpace(this.Survey.FooterHtml))
                {
                    responseHtml.Replace("#@SURVEYFOOTER", this.Survey.FooterHtml);
                }
                else
                {
                    responseHtml.Replace("#@SURVEYFOOTER", string.Empty);
                }
            }
            //SURVEY_BOTTOM_PROGRESSBAR
            responseHtml.Replace("#@SURVEY_BOTTOM_PROGRESSBAR", string.Empty);


            Response.ContentType = "text/html";
            Response.Write(responseHtml.ToString());
        }

    }
}