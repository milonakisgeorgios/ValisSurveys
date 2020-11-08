using System;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class MatrixOnePerRowRenderer : QuestionRenderer
    {
        internal MatrixOnePerRowRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }

        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Collection<VLQuestionOption> options = SurveyManager.GetQuestionOptions(question);
            Collection<VLQuestionColumn> columns = SurveyManager.GetQuestionColumns(question);

            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "matrixoneperrow");
            Writer.RenderBeginTag(HtmlTextWriterTag.Table);

            //<thead>
            Writer.RenderBeginTag(HtmlTextWriterTag.Thead);
            Writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            Writer.RenderBeginTag(HtmlTextWriterTag.Td);
            Writer.RenderEndTag();
            foreach (VLQuestionColumn column in columns)
            {
                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "ColumnText");
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                HttpUtility.HtmlEncode(column.ColumnText, Writer);
                Writer.RenderEndTag();
            }
            Writer.RenderEndTag();
            Writer.RenderEndTag();

            //<tbody>
            foreach (VLQuestionOption option in options)
            {
                Writer.RenderBeginTag(HtmlTextWriterTag.Tr);

                Writer.AddAttribute(HtmlTextWriterAttribute.Class, "OptionText");
                Writer.RenderBeginTag(HtmlTextWriterTag.Td);
                HttpUtility.HtmlEncode(option.OptionText, Writer);
                Writer.RenderEndTag();

                {
                    string _selectedColumnId = null;
                    if (this.RuntimeSession != null)
                    {
                        _selectedColumnId = this.RuntimeSession[option.HtmlOptionId] as string;
                    }                    
                    Int32 selectedColumnId = _selectedColumnId != null ? Int32.Parse(_selectedColumnId) : 0;
                    foreach (var column in columns)
                    {
                        Writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        Writer.AddAttribute(HtmlTextWriterAttribute.Type, "radio");
                        Writer.AddAttribute(HtmlTextWriterAttribute.Name, option.HtmlOptionId);//ID
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, column.ColumnId.ToString());
                        if (selectedColumnId == column.ColumnId)
                        {
                            Writer.AddAttribute(HtmlTextWriterAttribute.Checked, "Checked");
                        }
                        Writer.RenderBeginTag(HtmlTextWriterTag.Input);

                        Writer.RenderEndTag();

                        Writer.RenderEndTag();
                    }
                }

                Writer.RenderEndTag();
            }

            Writer.RenderEndTag();
        }
    }
}
