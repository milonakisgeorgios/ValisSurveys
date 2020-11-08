using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.LibraryQuestions
{
    public class CreateQuestion : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var category = TryParseInt16(context, "category");
                var questionText = TryParseString(context, "questionText");
                var questionType = (QuestionType)Enum.Parse(typeof(QuestionType), TryParseString(context, "questionType", true));

                VLLibraryManager libraryManager = VLLibraryManager.GetAnInstance(accessToken);

                var question = libraryManager.CreateLibraryQuestion(category, questionType, questionText);

                var item = new
                {
                    question.QuestionId,
                    question.Category,
                    question.QuestionType,
                    question.IsRequired,
                    question.RequiredBehavior,
                    question.RequiredMinLimit,
                    question.RequiredMaxLimit,
                    question.OptionalInputBox,
                    question.RandomizeOptionsSequence,
                    question.RandomizeColumnSequence,
                    question.OneResponsePerColumn,
                    question.UseDateTimeControls,
                    question.AddResetLink,
                    question.ValidationBehavior,
                    question.RandomBehavior,
                    question.OtherFieldType,
                    question.TextsLanguage,
                    question.QuestionText,
                    question.CreateDT,
                    question.LastUpdateDT
                };

                var response = JsonConvert.SerializeObject(item, Formatting.None);
                context.Response.Write(response);
            }
            catch
            {
                throw;
            }
        }
    }
}