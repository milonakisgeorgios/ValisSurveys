using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    /// <summary>
    /// 
    /// </summary>
    public class DecimalRenderer : QuestionRenderer
    {
        internal DecimalRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }


        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "soption");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                //Writer.AddAttribute(HtmlTextWriterAttribute.For, question.HtmlQuestionId);
                //Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                //HttpUtility.HtmlEncode(question.QuestionText, Writer);
                //Writer.RenderEndTag();

                Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Decimal");
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
