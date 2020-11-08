using System.Web.UI;

namespace Valis.Core.Html
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class QuestionRenderer
    {
        protected HtmlTextWriter Writer { get; set; }
        protected VLRuntimeSession RuntimeSession { get; set; }
        protected VLSurveyManager SurveyManager { get; set; }


        protected QuestionRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
        {
            this.SurveyManager = surveyManager;
            this.Writer = writer;
            this.RuntimeSession = runtimeSession;
        }

        public abstract void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question);
    }
}
