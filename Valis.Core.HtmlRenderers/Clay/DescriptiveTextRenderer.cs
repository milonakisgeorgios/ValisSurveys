using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class DescriptiveTextRenderer : QuestionRenderer
    {
        internal DescriptiveTextRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }

        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {

        }
    }
}
