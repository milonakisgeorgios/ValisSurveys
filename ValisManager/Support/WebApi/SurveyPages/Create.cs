using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class Create : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "SurveyId");
            var textsLanguage = TryParseInt16(context, "TextsLanguage");
            var showTitle = TryParseString(context, "ShowTitle", required: true);
            var description = TryParseString(context, "Description", required: false);
            var position = TryParseInt16(context, "Position", required: true);
            var referingPage = TryParseInt16(context, "ReferingPage");
            
            //
            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            VLSurveyPage surveyPage = null;

            if (position == /*Add Page After*/ 0)
            {
                surveyPage = surveyManager.CreateSurveyPageAfter(surveyId, referingPage, showTitle, description, textsLanguage);
            }
            else if (position == /*Add Page Before*/ 1)
            {
                surveyPage = surveyManager.CreateSurveyPageBefore(surveyId, referingPage, showTitle, description, textsLanguage);
            }
            else
            {
                throw new VLException(string.Format("unknown position, {0}!", position));
            }


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