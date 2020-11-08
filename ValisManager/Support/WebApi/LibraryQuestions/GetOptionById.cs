using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class GetOptionById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var questionId = TryParseInt32(context, "QuestionId");
            var optionId = TryParseByte(context, "optionId");
            var language = TryParseInt16(context, "language");

            VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

            var option = libraryManager.GetLibraryQuestionOptionById(questionId, optionId, language);
            if (option == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryOption", optionId));
            }


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