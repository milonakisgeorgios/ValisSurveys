using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class DropDownRenderer : QuestionRenderer
    {
        internal DropDownRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }
        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            var options = SurveyManager.GetQuestionOptions(question);

            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "DropDown");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mOption");
                Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    Writer.RenderBeginTag(HtmlTextWriterTag.Label);
                    HttpUtility.HtmlEncode(question.QuestionText, Writer);
                    Writer.RenderEndTag();

                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
                    Writer.RenderBeginTag(HtmlTextWriterTag.Select);

                    Writer.AddAttribute(HtmlTextWriterAttribute.Value, string.Empty);
                    Writer.RenderBeginTag(HtmlTextWriterTag.Option);
                    HttpUtility.HtmlEncode("[please select]", Writer);
                    Writer.RenderEndTag();

                    string _selectedOptionId = null;
                    if (this.RuntimeSession != null)
                    {
                        _selectedOptionId = this.RuntimeSession[question.HtmlQuestionId] as string;
                    }
                    Int32 selectedOptionId = string.IsNullOrEmpty(_selectedOptionId) == false ? Int32.Parse(_selectedOptionId) : 0;
                    foreach (var option in options)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, option.OptionId.ToString(CultureInfo.InvariantCulture));
                        if (option.OptionId == selectedOptionId)
                        {
                            Writer.AddAttribute(HtmlTextWriterAttribute.Selected, "Selected");
                        }
                        Writer.RenderBeginTag(HtmlTextWriterTag.Option);
                        HttpUtility.HtmlEncode(option.OptionText, Writer);
                        Writer.RenderEndTag();
                    }

                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
        }
    }
}
