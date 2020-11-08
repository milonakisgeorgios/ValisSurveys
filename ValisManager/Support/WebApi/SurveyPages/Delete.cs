using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class Delete : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var pageId = TryParseInt16(context, "pageId");
            var questionsDeleteBehavior = TryParseByte(context, "questionsDeleteBehavior", required:false, defValue: 0);


            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            surveyManager.DeleteSurveyPage(surveyId, pageId, (DeleteQuestionsBehavior)questionsDeleteBehavior);

            context.Response.Write("{}");
        }
    }
}