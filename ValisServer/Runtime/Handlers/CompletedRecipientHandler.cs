using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace ValisServer.Runtime
{
    /// <summary>
    /// Αυτός είναι ο handler που χειρίζεται requests απο Recipients που έχουν τρέξει ήδη το survey!
    /// </summary>
    public class CompletedRecipientHandler : SurveyHttpHandler
    {

        protected override void ProcessRequestImplementation()
        {
            ShowgoodByePage();
        }

        void ShowgoodByePage()
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
            //#@SURVEY_TOP_PROGRESSBAR
            responseHtml.Replace("#@SURVEY_TOP_PROGRESSBAR", string.Empty);
            //FORM ATTRIBUTES
            responseHtml.Replace("#@FORM_METHOD", "post");
            responseHtml.Replace("#@FORM_ACTION", string.Empty);
            //SURVEYBODY
            {
                _htmlBuffer.Clear();
                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "surveyBody");
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                RenderGoodbyePage(htmlWriter);
                htmlWriter.RenderEndTag();
                responseHtml.Replace("#@SURVEYBODY", _htmlBuffer.ToString());
            }
            //#@SURVEY_BOTTOM_PROGRESSBAR
            responseHtml.Replace("#@SURVEY_BOTTOM_PROGRESSBAR", string.Empty);
            //NAVIGATION
            {
                _htmlBuffer.Clear();

                htmlWriter.AddAttribute(HtmlTextWriterAttribute.Class, "surveyNavigation");
                htmlWriter.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Name, "NextButton");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Id, "NextButton");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode("Close"));
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Type, "button");
                    htmlWriter.AddAttribute(HtmlTextWriterAttribute.Onclick, "OnclientClose()");
                    htmlWriter.RenderBeginTag(HtmlTextWriterTag.Input);
                    htmlWriter.RenderEndTag();
                    htmlWriter.WriteLine();
                }
                htmlWriter.RenderEndTag();
                responseHtml.Replace("#@SURVEYNAVIGATION", _htmlBuffer.ToString());
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


            Response.ContentType = "text/html";
            Response.Write(responseHtml.ToString());
        }

        void RenderGoodbyePage(HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(this.Survey.GoodbyeHtml))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "surveyGoodbye");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write("<div style=\"margin: 80px auto 24px auto;font-size: 2em; font-weight: bold;\">Thanks for completing this survey.</div>");

                writer.RenderEndTag();
            }
            else
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "surveyGoodbye");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write(this.Survey.GoodbyeHtml);

                writer.RenderEndTag();
            }
        }
    }
}