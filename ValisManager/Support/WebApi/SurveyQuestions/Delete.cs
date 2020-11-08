using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var surveyId = TryParseInt32(context, "surveyId");
                var questionId = TryParseInt16(context, "questionId");

                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);

                surveyManager.DeleteQuestion(surveyId, questionId);


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