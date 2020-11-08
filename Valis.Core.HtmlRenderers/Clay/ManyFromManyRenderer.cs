using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class ManyFromManyRenderer : OptionalInputBoxSupport
    {
        internal ManyFromManyRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }

        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            var options = SurveyManager.GetQuestionOptions(question);

            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "manyfrommany");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                #region

                //we render the options:
                foreach (var option in options)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mOption");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        //<label></label>
                        Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                        {
                            //<input type="checkbox" />
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Value, "1");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Name, option.HtmlOptionId);
                            if (this.RuntimeSession != null && this.RuntimeSession[option.HtmlOptionId] != null)
                            {
                                Writer.AddAttribute(HtmlTextWriterAttribute.Checked, "Checked");
                            }
                            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                            Writer.RenderEndTag();
                        }
                        HttpUtility.HtmlEncode(option.OptionText, Writer);
                        Writer.RenderEndTag();
                    }
                    Writer.RenderEndTag();
                }

                //Do we need to add an OptionalInputBox?
                if (question.OptionalInputBox)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mOption OptionalInputBox");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        //<label></label>
                        Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                        {
                            //<input type="checkbox" />
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Value, "1");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_");
                            if (this.RuntimeSession != null && this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_"] != null)
                            {
                                Writer.AddAttribute(HtmlTextWriterAttribute.Checked, "Checked");
                            }
                            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                            Writer.RenderEndTag();
                        }
                        HttpUtility.HtmlEncode(question.OtherFieldLabel, Writer);
                        Writer.RenderEndTag();

                        //Other Field:
                        RenderOptionInputBox(survey, page, question);
                    }
                    Writer.RenderEndTag();
                }
                #endregion
            }

            Writer.RenderEndTag();
        }
    }
}
