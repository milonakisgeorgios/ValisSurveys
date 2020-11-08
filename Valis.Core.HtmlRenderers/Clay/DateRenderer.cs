using System;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    /// <summary>
    /// 
    /// </summary>
    public class DateRenderer : QuestionRenderer
    {
        internal DateRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }



        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "soption");
            Writer.RenderBeginTag(HtmlTextWriterTag.Div);
            {
                Writer.RenderBeginTag(HtmlTextWriterTag.Table);
                {
                    if (question.UseDateTimeControls)
                    {
                        RenderQuestionAsDatePicker(survey, page, question);
                    }
                    else
                    {
                        if (question.ValidationBehavior == ValidationMode.Date1)
                        {
                            RenderQuestionAsInputControls1(survey, page, question);
                        }
                        else if (question.ValidationBehavior == ValidationMode.Date2)
                        {
                            RenderQuestionAsInputControls2(survey, page, question);
                        }
                    }
                }
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
        }

        public void RenderQuestionAsDatePicker(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
            Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "10");
            if(question.ValidationBehavior == ValidationMode.Date1)
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date1");
            else if (question.ValidationBehavior == ValidationMode.Date2)
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date2");
            if (this.RuntimeSession != null)
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId] as string);
            }
            Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId);
            Writer.RenderBeginTag(HtmlTextWriterTag.Input);
            Writer.RenderEndTag();
        }

        public void RenderQuestionAsInputControls1(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            //MM/DD/YYYY
            Writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                Writer.Write("<td></td><th abbr=\"Month\" scope=\"col\">MM</th><td>&nbsp;</td><th abbr=\"Day\" scope=\"col\">DD</th><td>&nbsp;</td><th abbr=\"Year\" scope=\"col\">YYYY</th><th style=\"width: 80%;\"></th>");
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();

            Writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Month");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_MONTH"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_MONTH");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Day");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2"); 
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_DAY"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_DAY");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Year");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "4");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "4");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_YEAR"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_YEAR");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();
        }
        public void RenderQuestionAsInputControls2(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            //DD/MM/YYYY
            Writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                Writer.Write("<td></td><th abbr=\"Day\" scope=\"col\">DD</th><td>&nbsp;</td><th abbr=\"Month\" scope=\"col\">MM</th><td>&nbsp;</td><th abbr=\"Year\" scope=\"col\">YYYY</th><th style=\"width: 80%;\"></th>");
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();

            Writer.RenderBeginTag(HtmlTextWriterTag.Tbody);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            {
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Day");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_DAY"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_DAY");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Month");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "2");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "2");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_MONTH"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_MONTH");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.Write("/");
                Writer.RenderEndTag();

                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                {
                    Writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Class, "Date_Year");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, "4");
                    Writer.AddAttribute(HtmlTextWriterAttribute.Size, "4");
                    if (this.RuntimeSession != null)
                    {
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, this.RuntimeSession[question.HtmlQuestionId + "_YEAR"] as string);
                    }
                    Writer.AddAttribute(HtmlTextWriterAttribute.Name, question.HtmlQuestionId + "_YEAR");
                    Writer.RenderBeginTag(HtmlTextWriterTag.Input);
                    Writer.RenderEndTag();
                }
                Writer.RenderEndTag();
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();
        }

    }
}
