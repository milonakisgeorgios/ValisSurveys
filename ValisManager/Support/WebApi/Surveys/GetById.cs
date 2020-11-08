using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Surveys
{
    public class GetById : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var textsLanguage = TryParseInt16(context, "textsLanguage", false, BuiltinLanguages.PrimaryLanguage);


            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var survey = surveyManager.GetSurveyById(surveyId, textsLanguage);

            if(survey!= null)
            {
                //Θέλω το πλήθος των ανοιχτών collector:
                var collectors = surveyManager.GetCollectorsCount(survey.SurveyId, "where Status=1");

                var _survey = new
                {
                    survey.Client,
                    survey.SurveyId,
                    survey.SupportedLanguagesIds,
                    survey.PrimaryLanguage,
                    survey.TextsLanguage,
                    survey.Title,
                    survey.ShowTitle,
                    survey.IsBuiltIn,
                    survey.HasCollectors,
                    survey.HasResponses,
                    OpenCollectors = collectors
                };

                var response = JsonConvert.SerializeObject(_survey, Formatting.None);
                context.Response.Write(response);
            }
            else
            {
                throw new VLException(string.Format("There is no Survey with id='{0}'.", surveyId));
            }
        }
    }
}