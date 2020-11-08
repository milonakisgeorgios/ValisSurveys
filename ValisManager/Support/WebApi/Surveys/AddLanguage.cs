using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Surveys
{
    public class AddLanguage : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var sourceLanguage = TryParseInt16(context, "sourceLanguage");
            var targetLanguage = TryParseInt16(context, "targetLanguage");

            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var translatedSurvey = surveyManager.AddSurveyLanguage(surveyId, sourceLanguage, targetLanguage);

            if (translatedSurvey != null)
            {
                var _survey = new
                {
                    translatedSurvey.Client,
                    translatedSurvey.SurveyId,
                    translatedSurvey.TextsLanguage,
                    translatedSurvey.Title,
                    translatedSurvey.ShowTitle
                };

                var response = JsonConvert.SerializeObject(_survey, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException("AddSurveyLanguage failure!");
            }
        }
    }
}