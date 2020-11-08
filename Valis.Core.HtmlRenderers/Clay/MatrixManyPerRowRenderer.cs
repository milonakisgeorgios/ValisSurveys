using System.Collections.ObjectModel;
using System.Web;
using System.Web.UI;

namespace Valis.Core.Html.Clay
{
    public class MatrixManyPerRowRenderer : QuestionRenderer
    {
        internal MatrixManyPerRowRenderer(VLSurveyManager surveyManager, HtmlTextWriter writer, VLRuntimeSession runtimeSession)
            : base(surveyManager, writer, runtimeSession)
        {

        }


        public override void RenderQuestion(VLSurvey survey, VLSurveyPage page, VLSurveyQuestion question)
        {
            Collection<VLQuestionOption> options = SurveyManager.GetQuestionOptions(question);
            Collection<VLQuestionColumn> columns = SurveyManager.GetQuestionColumns(question);

            Writer.AddAttribute(HtmlTextWriterAttribute.Class, "matrixmanyperrow");
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
                    foreach (var column in columns)
                    {
                        Writer.RenderBeginTag(HtmlTextWriterTag.Td);

                        Writer.AddAttribute(HtmlTextWriterAttribute.Type, "checkbox");
                        Writer.AddAttribute(HtmlTextWriterAttribute.Value, "1");
                        Writer.AddAttribute(HtmlTextWriterAttribute.Name, option.HtmlOptionId + column.HtmlPartialColumnId);//ID
                        if (this.RuntimeSession != null && this.RuntimeSession[option.HtmlOptionId + column.HtmlPartialColumnId] != null)
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
