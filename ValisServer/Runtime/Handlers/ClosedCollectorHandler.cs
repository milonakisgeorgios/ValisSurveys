using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace ValisServer.Runtime
{
    /// <summary>
    /// Αυτός είναι ο Handler που χειρίζεται requests για κλειστούς collectors!
    /// <para>Οταν κληθεί η ClosedCollectorHandler, έχουμε στα σίγουρα βρεί το Survey + surveyTheme, καθώς και τον Collector!</para>
    /// </summary>
    public class ClosedCollectorHandler : SurveyHttpHandler
    {


        #region ProcessRequestImplementation
        protected override void ProcessRequestImplementation()
        {
            ShowCollectorIsClosedPage();
        }

        #endregion


        void ShowCollectorIsClosedPage()
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
                htmlWriter.Write("<table style=\"width:100%; margin-top: 120px;\"><tbody><tr><td align=\"center\">This survey is currently closed. Please contact the author of this survey for further assistance.</td></tr></tbody></table>");
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