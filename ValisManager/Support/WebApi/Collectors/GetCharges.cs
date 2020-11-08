using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Web;
using Valis.Core;
using Valis.Core.ViewModel;

namespace ValisManager.Support.WebApi.Collectors
{
    /// <summary>
    /// Επιστρέφει όλα το VLCharge απο ανήκουν σε έναν συγκεκριμένο Collector
    /// </summary>
    public class GetCharges : WebApiHandler
    {
        string GetPaymentTitle(VLCharge c)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("(#{0}, ", c.PaymentId);
            sb.AppendFormat("{0}) ", c.PaymentDate.ToShortDateString());
            sb.AppendFormat("<span style=\"font-size:1.2em; padding-left: 8px;\">{0} ", c.PaymentQuantity);

            switch (c.CreditType)
            {
                case CreditType.ClickType:
                    sb.Append(" clicks");
                    break;
                case CreditType.EmailType:
                    sb.Append(" emails");
                    break;
                case CreditType.ResponseType:
                    sb.Append(" responses");
                    break;
            }
            sb.Append("</span>");

            return sb.ToString();
        }

        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorId = TryParseInt32(context, "collectorId", true);
            var pageIndex = TryParseInt32(context, "page", false, 1);
            var pageSize = TryParseInt32(context, "rows", false, 10);
            var sortIndex = TryParseString(context, "sidx", false, "CollectorPaymentId");
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
                c.CollectorPaymentId,
                c.PaymentId,
                c.UseOrder,
                c.QuantityReserved,
                c.QuantityUsed,
                FirstChargeDt = c.FirstChargeDt.HasValue ? c.FirstChargeDt.Value.ToShortDateString() : string.Empty,
                LastChargeDt = c.LastChargeDt.HasValue ? c.LastChargeDt.Value.ToShortDateString() : string.Empty,
                c.PaymentDate,
                c.PaymentQuantity,
                c.PaymentQuantityUsed,
                PaymentTitle = GetPaymentTitle(c)
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