using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class CreateOption : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var optionText = TryParseString(context, "optionText");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var option = libraryManager.CreateLibraryQuestionOption(questionId, optionText, textsLanguage: language);

            var item = new
            {
                option.Question,
                option.OptionId,
                option.OptionText,
                option.OptionType,
                option.OptionValue,
                option.TextsLanguage,
                option.DisplayOrder,
                option.AttributeFlags
            };

            var response = JsonConvert.SerializeObject(item, Formatting.None);
            context.Response.Write(response);
        }
    }
}