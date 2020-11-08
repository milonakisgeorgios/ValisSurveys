using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Collectors
{
    public class GetChargedCollectors : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var clientId = TryParseInt32(context, "clientId", true);
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");

            var manager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = manager.GetChargedCollectorsPaged(clientId, pageIndex, pageSize, ref totalRecords);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;

            var rows = items.Select(c => new
            {
                c.SurveyId,
                c.SurveyTitle,
                c.CollectorId,
                c.CollectorTitle,
                c.CollectorType,
                c.Status,
                c.Responses,
                c.CreditType,
                c.CreationDT,
                c.CreatedBy,
                FirstChargeDt = c.FirstChargeDt.HasValue ? c.FirstChargeDt.Value.ToShortDateString() : string.Empty,
                LastChargeDt = c.LastChargeDt.HasValue ? c.LastChargeDt.Value.ToShortDateString() : string.Empty
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