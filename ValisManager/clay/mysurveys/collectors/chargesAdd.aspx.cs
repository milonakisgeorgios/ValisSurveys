using System;
using System.Web.UI.WebControls;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class chargesAdd : CollectorsPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                this.Payment.Items.Clear();

                var payments = SystemManager.GetPayments(Globals.UserToken.ClientId.Value);
                foreach(var payment in payments)
                {
                    if(payment.CreditType != this.SelectedCollector.CreditType)
                    {
                        continue;
                    }
                    ListItem item = new ListItem(string.Format("{0} {1}, at {2}", payment.Quantity, payment.CreditType, payment.PaymentDate.ToShortDateString()), payment.PaymentId.ToString());
                    this.Payment.Items.Add(item);
                }
            }
        }

        protected void createCollectorPayment_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 paymentId = Int32.Parse(this.Payment.SelectedValue);
                Int32? quantityLimit = null;
                if(!string.IsNullOrWhiteSpace(this.QuantityLimit.Text))
                {
                    quantityLimit = Int32.Parse(this.QuantityLimit.Text);
                }

                var collectorPayment = SystemManager.AddPaymentToCollector(this.CollectorId, paymentId, quantityLimit);

                Response.Redirect(_UrlSuffix(string.Format("charges.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, this.CollectorId, this.TextsLanguage)), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}