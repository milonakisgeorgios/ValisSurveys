using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class UpdateColumn : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var columnId = TryParseByte(context, "columnId");
            var columnText = TryParseString(context, "columnText");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var column = libraryManager.GetLibraryQuestionColumnById(questionId, columnId, language);
            if (column == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryColumn", columnId));
            }

            column.ColumnText = columnText;
            column = libraryManager.UpdateLibraryQuestionColumn(column);


            var item = new
            {
                column.Question,
                column.ColumnId,
                column.DisplayOrder,
                column.AttributeFlags,
                column.TextsLanguage,
                column.ColumnText
            };

            var response = JsonConvert.SerializeObject(item, Formatting.None);
            context.Response.Write(response);
        }
    }
}