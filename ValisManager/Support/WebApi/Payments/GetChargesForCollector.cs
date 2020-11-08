using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;
using Valis.Core.ViewModel;

namespace ValisManager.Support.WebApi.Payments
{
    public class GetChargesForCollector : WebApiHandler
    {
        string GetPaymentQuantity(VLCharge item)
        {
            if (item.CreditType == CreditType.EmailType)
            {
                if (item.PaymentQuantity == 1)
                {
                    return "1 email";
                }
                else
                {
                    return string.Format("{0} emails", item.PaymentQuantity);
                }
            }
            else if (item.CreditType == CreditType.ResponseType)
            {
                if (item.PaymentQuantity == 1)
                {
                    return "1 response";
                }
                else
                {
                    return string.Format("{0} responses", item.PaymentQuantity);
                }
            }
            else if (item.CreditType == CreditType.ClickType)
            {
                if (item.PaymentQuantity == 1)
                {
                    return "1 click";
                }
                else
                {
                    return string.Format("{0} clicks", item.PaymentQuantity);
                }
            }

            return string.Empty;
        }
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId", true);
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "Name");
            var sortOrder = TryParseString(context, "sord", false, "asc");
            var orderByClause = string.Format("order by {0} {1}", sortIndex, sortOrder);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            int totalRecords = 0;
            var items = systemManager.GetChargesForCollector(collectorId, pageIndex, pageSize, ref totalRecords, string.Empty, orderByClause);

            int totalpages = totalRecords / pageSize;
            if (totalpages * pageSize < totalRecords)
                totalpages++;


            var rows = items.Select(c => new
            {
                c.CollectorPaymentId,

                c.SurveyId,

                c.CollectorId,
                c.Responses,
                c.CreditType,

                c.PaymentId,
                c.UseOrder,
                c.QuantityReserved,
                c.QuantityUsed,
                FirstChargeDt = c.FirstChargeDt.HasValue ? c.FirstChargeDt.Value.ToShortDateString() : string.Empty,
                LastChargeDt = c.LastChargeDt.HasValue ? c.LastChargeDt.Value.ToShortDateString() : string.Empty,
                c.IsActive,
                c.IsUsed,

                PaymentDate = c.PaymentDate.ToShortDateString(),
                c.PaymentIsActive,
                PaymentQuantity = GetPaymentQuantity(c),
                c.PaymentQuantityUsed
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