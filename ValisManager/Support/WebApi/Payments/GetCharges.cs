using Newtonsoft.Json;
using System.Linq;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Payments
{
    /// <summary>
    /// Επιστρέφει όλα το VLCharge απο ανήκουν σε έναν συγκεκριμένο Payment
    /// </summary>
    public class GetCharges : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var paymentId = TryParseInt32(context, "paymentId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);

            var items = systemManager.GetCharges(paymentId);

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
                LastChargeDt = c.LastChargeDt.HasValue ? c.LastChargeDt.Value.ToShortDateString() : string.Empty
            }).ToArray();

            var data = new
            {
                total = 1,     //total pages for the query
                page = 1,       //current page of the query
                records = items.Count, //total number of records for the query
                rows
            };



            var response = JsonConvert.SerializeObject(data, Formatting.None);
            context.Response.Write(response);
        }
    }
}