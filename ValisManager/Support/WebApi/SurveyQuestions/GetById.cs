using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var questionId = TryParseInt16(context, "questionId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");


            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            VLSurveyQuestion question = surveyManager.GetQuestionById(surveyId, questionId, textsLanguage);

            if (question != null)
            {
                var _question = new
                {
                    question.Survey,
                    question.QuestionId,
                    question.Page,
                    question.MasterQuestion,
                    question.DisplayOrder,
                    question.QuestionType,
                    question.CustomType,
                    question.IsRequired,
                    question.AttributeFlags,
                    question.ValidationBehavior,
                    question.RegularExpression,
                    question.HasSkipLogic,
                    question.TextsLanguage,

                    question.QuestionText,
                    question.Description,
                    question.HelpText,
                    question.FrontLabelText,
                    question.AfterLabelText,
                    question.InsideText,
                    question.RequiredMessage,
                    question.ValidationMessage
                };


                var response = JsonConvert.SerializeObject(_question, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Question with id='{0},{1}'.", surveyId, questionId));
            }

        }
    }
}