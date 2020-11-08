using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Surveys
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var surveyId = TryParseInt32(context, "surveyId");
                var textsLanguage = TryParseInt16(context, "textsLanguage");

                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);

                surveyManager.DeleteSurvey(surveyId);


                //empty json object
                context.Response.Write("{}");
            }
            catch
            {
                throw;
            }
        }
    }
}