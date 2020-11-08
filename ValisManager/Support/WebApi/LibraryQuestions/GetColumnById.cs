using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class GetColumnById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var columnId = TryParseByte(context, "columnId");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var column = libraryManager.GetLibraryQuestionColumnById(questionId, columnId, language);
            if (column == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryColumn", columnId));
            }


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