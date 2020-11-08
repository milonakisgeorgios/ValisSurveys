using Newtonsoft.Json;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.Payments
{
    public class GetPaymentInfoForCollectorPayment  : WebApiHandler
    {
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            var collectorPaymentId = TryParseInt32(context, "collectorPaymentId", true);

            var systemManager = VLSystemManager.GetAnInstance(accessToken);
            var collectorPayment = systemManager.GetCollectorPaymentById(collectorPaymentId);
            if (collectorPayment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "CollectorPayment", collectorPaymentId));
            }

            var payment = systemManager.GetPaymentById(collectorPayment.Payment);
            if (payment == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "payment", collectorPayment.Payment));
            }


            var item = new
            {
                payment.PaymentId,
                payment.Quantity,
                payment.QuantityUsed,
                payment.PaymentType,
                payment.CreditType,
                Description = string.Format("{0} {1} {2}", payment.Quantity, payment.CreditType, payment.PaymentDate.ToShortDateString()),
                collectorPayment.QuantityReserved
            };


            var response = JsonConvert.SerializeObject(item, Formatting.None);
            context.Response.Write(response);
        }
    }
}