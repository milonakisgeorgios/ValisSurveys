using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class OneFromManyRenderer : OptionalInputBoxSupport
    {
        internal OneFromManyRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }




        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            var options = SurveyManager.GetQuestionOptions(question);

            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "onefrommany");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                #region
                string _selectedOptionId = null;
                Int32 selectedOptionId = 0;
                if (this.RuntimeSession != null)
                {
                    _selectedOptionId = this.RuntimeSession[question.HtmlQuestionId] as string;
                    if (!string.IsNullOrWhiteSpace(_selectedOptionId))
                    {
                        if (!Int32.TryParse(_selectedOptionId, out selectedOptionId))
                        {
                            selectedOptionId = 0;
                        }
                    }
                }

                //we render the options:
                foreach (var option in options)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mOption");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        //<label></label>
                        Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                        {
                            //<input type="radio" />
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Value, option.OptionId.ToString(CultureInfo.InvariantCulture));
                            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
                            if (option.OptionId == selectedOptionId)
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
                if(question.OptionalInputBox)
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mOption OptionalInputBox");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    {
                        //<label></label>
                        Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                        {
                            //<input type="radio" />
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Value, question.HtmlQuestionId + "OptionalInputBox_");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
                            if (question.HtmlQuestionId + "OptionalInputBox_" == _selectedOptionId)
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
