using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Messages
{
    public class GetReadyMessages : WebApiHandler
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

                int totalRecords = 0;
                var items = manager.GetNonDraftMessages(collectorId, pageIndex, pageSize, ref totalRecords, string.Empty);

                int totalpages = totalRecords / pageSize;
                if (totalpages * pageSize < totalRecords)
                    totalpages++;





                var rows = items.Select(c => new
                {
                    c.Collector,
                    c.MessageId,
                    c.Sender,
                    c.Status,
                    c.IsDeliveryMethodOK,
                    c.IsSenderOK,
                    c.IsContentOK,
                    c.IsScheduleOK,
                    ScheduledAt = c.ScheduledAt.HasValue ? accessToken.ConvertTimeFromUtc(c.ScheduledAt.Value).ToString(Utilities.DateTime_Format_Human, CultureInfo.InvariantCulture) : null,
                    c.SentCounter,
                    c.FailedCounter,
                    c.SkipCounter,
                    c.DeliveryMethod,
                    c.Subject
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
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}