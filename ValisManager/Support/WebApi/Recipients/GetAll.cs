using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Recipients
{
    public class GetAll : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var collectorId = TryParseInt32(context, "collectorId", true);
                var pageIndex = TryParseInt32(context, "page", false, 1);
                var pageSize = TryParseInt32(context, "rows", false, 10);
                var sortIndex = TryParseString(context, "sidx", false, "Name");
                var sortOrder = TryParseString(context, "sord", false, "asc");

                var manager = VLSurveyManager.GetAnInstance(accessToken);
                var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

                int totalRecords = 0;
                var items = manager.GetRecipients(collectorId, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

                int totalpages = totalRecords / pageSize;
                if (totalpages * pageSize < totalRecords)
                    totalpages++;


                var rows = items.Select(c => new
                {
                    c.RecipientId,
                    c.Collector,
                    c.RecipientKey,
                    c.Email,
                    c.FirstName,
                    c.LastName,
                    c.Title,
                    c.Status,
                    c.IsSentEmail,
                    c.IsOptedOut,
                    c.IsBouncedEmail,
                    c.HasPartiallyResponded,
                    c.HasResponded,
                    c.HasManuallyAdded
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
            catch
            {
                throw;
            }

        }
    }
}