using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class DeleteColumn :  WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var columnId = TryParseByte(context, "columnId");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);
            libraryManager.DeleteLibraryQuestionColumn(questionId, columnId);


            //empty json object
            context.Response.Write("{}");
        }
    }
}