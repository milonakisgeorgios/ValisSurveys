using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Profiles
{
    public class GetProfiles : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetClientProfiles(pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            var rows = items.Select(c => new
            {
                c.ProfileId,
                c.Name,
                //c.Comment,
                c.MaxNumberOfUsers,
                c.MaxNumberOfSurveys,
                c.MaxNumberOfLists,
                c.MaxNumberOfRecipientsPerList,
                c.MaxNumberOfRecipientsPerMessage,
                c.MaxNumberOfCollectorsPerSurvey,
                c.MaxNumberOfEmailsPerDay,
                c.MaxNumberOfEmailsPerWeek,
                c.MaxNumberOfEmailsPerMonth,
                c.MaxNumberOfEmails,
                c.IsBuiltIn,
                c.UseCredits,
                c.CanTranslateSurveys,
                c.CanUseSurveyTemplates,
                c.CanUseQuestionTemplates,
                c.CanCreateWebLinkCollectors,
                c.CanCreateEmailCollectors,
                c.CanCreateWebsiteCollectors,
                c.CanUseSkipLogic,
                c.CanExportData,
                c.CanExportReport,
                c.CanUseWebAPI,
                c.CreateDT
            }).ToArray();
            var data = new
            {
                total = totalpages,     //total pages for the query
                page = pageIndex,       //current page of the query
                records = totalRecords, //total number of records for the query
                rows
            };


            var response = JsonConvert.SerializeObject(data, Formatting.None);
            context.Response.Write(response);

        }
    }
}