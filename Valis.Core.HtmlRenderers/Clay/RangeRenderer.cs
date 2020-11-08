using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    /// <summary>
    /// 
    /// </summary>
    public class RangeRenderer : QuestionRenderer
    {
        internal RangeRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }

        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "range");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "rangeline");
                Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    #region
                    //FrontLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "frontLabel");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    Writer.RenderEndTag();


                    for (int range = question.RangeStart.Value; range <= question.RangeEnd.Value; range++)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mark");
                        Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        {
                            Writer.Write(range.ToString(CultureInfo.InvariantCulture));
                        }
                        Writer.RenderEndTag();
                    }

                    //AfterLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "afterLabel");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    Writer.RenderEndTag();
                    #endregion
                }
                Writer.RenderEndTag();


                Int32? selectedRange = null;
                if (this.RuntimeSession != null)
                {
                    var _selectedRange = this.RuntimeSession[question.HtmlQuestionId] as string;
                    if (!string.IsNullOrWhiteSpace(_selectedRange))
                    {
                        Int32 temp = -1;
                        if (Int32.TryParse(_selectedRange, out temp))
                        {
                            selectedRange = temp;
                        }
                    }
                }

                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "rangeline");
                Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                {
                    #region
                    //FrontLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "frontLabel");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    HttpUtility.HtmlEncode(question.FrontLabelText, Writer);
                    Writer.RenderEndTag();

                    for (int range = question.RangeStart.Value; range <= question.RangeEnd.Value; range++)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Class, "mark");
                        Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        {
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                            Writer.AddAttribute(HtmlTextWriterAttribute.Value, range.ToString(CultureInfo.InvariantCulture));
                            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
                            if (selectedRange == range)
                            {
                                Writer.AddAttribute(HtmlTextWriterAttribute.Checked, "Checked");
                            }
                            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                            Writer.RenderEndTag();
                        }
                        Writer.RenderEndTag();
                    }

                    //AfterLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "afterLabel");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Span);
                    HttpUtility.HtmlEncode(question.AfterLabelText, Writer);
                    Writer.RenderEndTag();
                    #endregion
                }
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
        }
        public void RenderQuestion2(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {

            Writer.AddAttribute(HtmlTextWriterAttribute.Width, "99%");
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "rangeTable");
            Writer.RenderBeginTag(HtmlTextWriterTag.Table);
            {
                //<thead>

                //<tbody>
                Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                {
                    //FrontLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    HttpUtility.HtmlEncode(question.FrontLabelText, Writer);
                    Writer.RenderEndTag();

                    for (int range = question.RangeStart.Value; range <= question.RangeEnd.Value; range++)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Width, "0%");
                        Writer.AddAttribute(HtmlTextWriterAttribute.Align, "center");
                        Writer.RenderBeginTag(HtmlTextWriterTag.Td);


                        Writer.AddAttribute(HtmlTextWriterAttribute.Width, "38px");
                        Writer.RenderBeginTag(HtmlTextWriterTag.Div);
                        {
                            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                            Writer.RenderEndTag();
                        }
                        Writer.RenderEndTag();

                        Writer.RenderEndTag();
                    }

                    //AfterLabelText
                    Writer.AddAttribute(HtmlTextWriterAttribute.Width, "50%");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    HttpUtility.HtmlEncode(question.AfterLabelText, Writer);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
            }

            Writer.RenderEndTag();
        }


    }
}
