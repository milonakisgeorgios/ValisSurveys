using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class OptionalInputBoxSupport : QuestionRenderer
    {

        internal OptionalInputBoxSupport(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }


        protected void RenderOptionInputBox(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            var content = string.Empty;
            if (this.RuntimeSession != null)
            {
                content = this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput"] as string;
            }


            if (question.OtherFieldType == OtherFieldType.MultipleLines)
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Cols, "50");
                Writer.AddAttribute(HtmlTextWriterAttribute.Rows, "4");
                Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput");
                Writer.RenderBeginTag(HtmlTextWriterTag.Textarea);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    Writer.Write(HttpUtility.HtmlEncode(content));
                }
                Writer.RenderEndTag();
            }
            else
            {
                if (question.ValidationBehavior == ValidationMode.WholeNumber)
                {
                    RenderWholeNumber(survey, page, question, content);
                }
                else if (question.ValidationBehavior == ValidationMode.DecimalNumber)
                {
                    RenderDecimal(survey, page, question, content);
                }
                else if (question.ValidationBehavior == ValidationMode.Date1)
                {
                    RenderDate1(survey, page, question, content);
                }
                else if (question.ValidationBehavior == ValidationMode.Date2)
                {
                    RenderDate2(survey, page, question, content);
                }
                else
                {
                    RenderDefault(survey, page, question, content);
                }
            }
        }

        void RenderWholeNumber(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question, string content)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Integer");
            if (question.ValidationBehavior == ValidationMode.WholeNumber)
            {
                if (string.IsNullOrWhiteSpace(question.ValidationField2) == false)
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, (question.ValidationField2.Length + 1).ToString());
            }
            if (!string.IsNullOrWhiteSpace(content))
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(content));
            }
            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput");
            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
            Writer.RenderEndTag();
        }
        void RenderDecimal(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question, string content)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Decimal");
            if (!string.IsNullOrWhiteSpace(content))
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(content));
            }
            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput");
            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
            Writer.RenderEndTag();
        }
        void RenderDate1(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question, string content)
        {
            Writer.RenderBeginTag(HtmlTextWriterTag.Table);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                #region Date format
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("(MM/DD/YYYY)");
                Writer.RenderEndTag();
                #endregion

                #region MONTH
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Month");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                #region DAY
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Day");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                #region YEAR
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Year");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "4");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "4");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();

            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();
        }
        void RenderDate2(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question, string content)
        {
            Writer.RenderBeginTag(HtmlTextWriterTag.Table);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                #region Date format
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("(DD/MM/YYYY)");
                Writer.RenderEndTag();
                #endregion

                #region DAY
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Day");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_DAY");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                #region MONTH
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Month");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_MONTH");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                #region YEAR
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Year");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "4");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "4");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput" + "_YEAR");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                #endregion

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();
        }
        void RenderDefault(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question, string content)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Generic");
            if (question.ValidationBehavior == ValidationMode.TextOfSpecificLength)
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, question.ValidationField2);
            }
            if (!string.IsNullOrWhiteSpace(content))
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Value, HttpUtility.HtmlEncode(content));
            }
            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "OptionalInputBox_userinput");
            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
            Writer.RenderEndTag();
        }

    }
}
