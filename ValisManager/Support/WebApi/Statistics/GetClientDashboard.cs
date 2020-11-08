using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Statistics
{
    public class GetClientDashboard : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var clientId = TryParseInt32(context, "clientId");


            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var dashboard = surveyManager.GetClientDashboard(clientId);


            var response = JsonConvert.SerializeObject(dashboard, Formatting.None);
            context.Response.Write(response);
        }
    }
}