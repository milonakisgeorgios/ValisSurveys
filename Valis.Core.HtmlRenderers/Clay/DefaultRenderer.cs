using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class DefaultRenderer : QuestionRenderer
    {
        internal DefaultRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }


        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            Writer.Write("NOT - IMPLEMENTED - YET!");
            Writer.RenderEndTag();
        }
    }
}
