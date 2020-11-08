using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class UpdateOption : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var optionId = TryParseByte(context, "optionId");
            var optionText = TryParseString(context, "optionText");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var option = libraryManager.GetLibraryQuestionOptionById(questionId, optionId, language);
            if (option == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryOption", optionId));
            }

            option.OptionText = optionText;
            option = libraryManager.UpdateLibraryQuestionOption(option);


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