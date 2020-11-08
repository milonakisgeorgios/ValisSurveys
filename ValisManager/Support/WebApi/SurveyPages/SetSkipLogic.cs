using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class SetSkipLogic : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var pageId = TryParseInt16(context, "pageId");
            var textsLanguage = TryParseInt16(context, "textsLanguage", required: false, defValue: 0);
            Int16? skipToPage = TryParseInt16(context, "skipToPage", false, null);


            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);

            VLSurveyPage surveyPage = null;

            if(skipToPage.HasValue)
                surveyPage = surveyManager.SetPageSkipLogic(surveyId, pageId, skipToPage.Value, textsLanguage);
            else
                surveyPage = surveyManager.UnSetPageSkipLogic(surveyId, pageId, textsLanguage);


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