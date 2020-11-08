using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyPages
{
    public class GetCandidateSkipToPages : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var surveyId = TryParseInt32(context, "surveyId");
            var pageId = TryParseInt16(context, "pageId");
            var textsLanguage = TryParseInt16(context, "textsLanguage", required: false, defValue: 0);

            var surveyManager = VLSurveyManager.GetAnInstance(accessToken);
            var pages = surveyManager.GetCandidateSkipPagesForPage(surveyId, pageId, addVirtualPages:true,  textsLanguage:textsLanguage);

            var rows = pages.Select(c => new { 
                c.Survey,
                c.PageId,
                c.DisplayOrder,
                c.HasSkipLogic,
                c.CustomId,
                c.SkipTo,
                c.SkipToPage,
                c.SkipToWebUrl,
                c.ShowTitle,
                /*
                 * Στις κανονικές σελίδες εμφανίζουμε το πρόθεμα Page: <pageid>
                 * Στις virtual σελίδες εμφναίζουμε μόνο το ShowTitle
                 */
                OptionTitle = c.PageId >= 0 ? (HttpUtility.HtmlEncode(string.Format("Page {0}: {1}", c.DisplayOrder, (c.ShowTitle != null ? c.ShowTitle : "<untitled page>")))) : ((c.ShowTitle != null ? c.ShowTitle : "<untitled page>"))
            }).ToArray();

            var response = JsonConvert.SerializeObject(rows, Formatting.None);
            context.Response.Write(response);
        }
    }
}