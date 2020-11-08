using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class GetDeleteOptions : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var pageId = TryParseInt16(context, "pageId");
            var textsLanguage = TryParseInt16(context, "textsLanguage", required: false, defValue: 0);

            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var options = surveyManager.GetDeleteSurveyPageOptions(surveyId, pageId);


            var response = JsonConvert.SerializeObject(options, Formatting.None);
            context.Response.Write(response);
        }
    }
}