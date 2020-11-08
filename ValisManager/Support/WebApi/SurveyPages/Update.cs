using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class Update : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "SurveyId");
            var pageId = TryParseInt16(context, "PageId");
            var textsLanguage = TryParseInt16(context, "TextsLanguage");

            //
            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);

            var surveyPage = surveyManager.GetSurveyPageById(surveyId, pageId, textsLanguage);
            if (surveyPage == null)
            {
                throw new VLException(string.Format("There is no SurveyPage with id='{0},{1}'.", surveyId, pageId));
            }

            surveyPage.ShowTitle = TryParseString(context, "ShowTitle", required: true);
            surveyPage.Description = TryParseString(context, "Description", required: false);

            surveyPage = surveyManager.UpdateSurveyPage(surveyPage);


            var _item = new
            {
                surveyPage.Survey,
                surveyPage.PageId,
                surveyPage.DisplayOrder,
                surveyPage.HasSkipLogic,
                surveyPage.CustomId,
                surveyPage.SkipTo,
                surveyPage.SkipToPage,
                surveyPage.SkipToWebUrl,
                surveyPage.TextsLanguage,
                surveyPage.ShowTitle,
                surveyPage.Description,
                CreateDT = surveyPage.CreateDT.ToShortDateString(),
                LastUpdateDT = surveyPage.LastUpdateDT.ToShortDateString(),
            };

            var response = JsonConvert.SerializeObject(_item, Formatting.None);
            context.Response.Write(response);
        }
    }
}