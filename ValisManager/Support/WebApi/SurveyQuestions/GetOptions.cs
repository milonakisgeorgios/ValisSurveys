using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class GetOptions : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var questionId = TryParseInt16(context, "questionId");
            var textsLanguage = TryParseInt16(context, "textsLanguage");

            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var options = surveyManager.GetQuestionOptions(surveyId, questionId, textsLanguage);


            var response = JsonConvert.SerializeObject(options, Formatting.None);
            context.Response.Write(response);
        }
    }
}