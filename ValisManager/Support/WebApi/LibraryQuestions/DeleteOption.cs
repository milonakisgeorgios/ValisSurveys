using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class DeleteOption : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var optionId = TryParseByte(context, "optionId");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);
            libraryManager.DeleteLibraryQuestionOption(questionId, optionId);


            //empty json object
            context.Response.Write("{}");
        }
    }
}