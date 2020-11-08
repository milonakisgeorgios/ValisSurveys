using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class CompositeRenderer : QuestionRenderer
    {
        internal CompositeRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }


        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "composite");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                //Τραβάμε τις child ερωτήσεις:
                var childQuestions = SurveyManager.GetChildQuestions(question.Survey, question.QuestionId, question.TextsLanguage);

                foreach (var childQuestion in childQuestions)
                {
                    var _renderer = HtmlRenderers.GetQuestionRenderer(this.SurveyManager, this.Writer, this.RuntimeSession, childQuestion.QuestionType);
                    _renderer.RenderQuestion(survey, page, childQuestion);
                }
            }
            Writer.RenderEndTag();
        }
    }
}
