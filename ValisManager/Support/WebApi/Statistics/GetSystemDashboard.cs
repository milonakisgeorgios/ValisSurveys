using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Statistics
{
    public class GetSystemDashboard : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var dashboard = surveyManager.GetSystemDashboard();


            var response = JsonConvert.SerializeObject(dashboard, Formatting.None);
            context.Response.Write(response);
        }
    }
}