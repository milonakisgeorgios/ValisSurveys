using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class MultipleLineRenderer : QuestionRenderer
    {
        internal MultipleLineRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }

        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "soption");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);


            Writer.AddAttribute(HtmlTextWriterAttribute.Cols, "50");
            Writer.AddAttribute(HtmlTextWriterAttribute.Rows, "4");
            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
            Writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
            if(this.RuntimeSession != null)
            {
                Writer.Write(HttpUtility.HtmlEncode(this.RuntimeSession[question.HtmlQuestionId]));
            }
            Writer.RenderEndTag();

            Writer.RenderEndTag();
        }
    }
}
