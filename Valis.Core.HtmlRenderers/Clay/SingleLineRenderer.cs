using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class SingleLineRenderer : QuestionRenderer
    {
        internal SingleLineRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }



        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            if (question.ValidationBehavior == ValidationMode.WholeNumber)
            {
                IntegerRenderer _renderer = new IntegerRenderer(this.SurveyManager, this.Writer, this.RuntimeSession);
                _renderer.RenderQuestion(survey, page, question);
            }
            else if (question.ValidationBehavior == ValidationMode.DecimalNumber)
            {
                DecimalRenderer _renderer = new DecimalRenderer(this.SurveyManager, this.Writer, this.RuntimeSession);
                _renderer.RenderQuestion(survey, page, question);
            }
            else if (question.ValidationBehavior == ValidationMode.Date1 || question.ValidationBehavior == ValidationMode.Date2)
            {
                DateRenderer _renderer = new DateRenderer(this.SurveyManager, this.Writer, this.RuntimeSession);
                _renderer.RenderQuestion(survey, page, question);
            }
            else
            {
                RenderDefault(survey, page, question);
            }
        }
        public void RenderDefault(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "soption");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Generic");
                Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                if (question.ValidationBehavior == ValidationMode.TextOfSpecificLength)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, question.ValidationField2);
                }
                if (this.RuntimeSession != null)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId] as string);
                }
                Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
                Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                Writer.RenderEndTag();

            }
            Writer.RenderEndTag();
        }



    }
}
