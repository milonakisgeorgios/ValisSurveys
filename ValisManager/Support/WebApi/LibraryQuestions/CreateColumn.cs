using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class CreateColumn : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var columnText = TryParseString(context, "columnText");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var column = libraryManager.CreateLibraryQuestionColumn(questionId, columnText, textsLanguage: language);

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