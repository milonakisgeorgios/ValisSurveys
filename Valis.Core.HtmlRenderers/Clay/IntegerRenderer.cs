using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    /// <summary>
    /// 
    /// </summary>
    public class IntegerRenderer : QuestionRenderer
    {
        internal IntegerRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }



        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "soption");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Integer");
                if (question.ValidationBehavior == ValidationMode.WholeNumber)
                {
                    if (string.IsNullOrWhiteSpace(question.ValidationField2) == false)
                        Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, (question.ValidationField2.Length+1).ToString());
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
