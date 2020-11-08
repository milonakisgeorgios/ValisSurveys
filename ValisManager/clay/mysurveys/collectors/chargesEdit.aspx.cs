using System;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class chargesEdit : CollectorsPage
    {
        protected VLPayment SelectedPayment
        {
            get
            {
                return (VLPayment)ViewState["SelectedPayment"];
            }
            set
            {
                ViewState["SelectedPayment"] = value;
            }
        }
        protected VLCollectorPayment SelectedCollectorPayment
        {
            get
            {
                return (VLCollectorPayment)ViewState["SelectedCollectorPayment"];
            }
            set
            {
                ViewState["SelectedCollectorPayment"] = value;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα collectorPaymentId
                    string _value1 = this.Request.Params["collectorPaymentId"];
                    if (!string.IsNullOrEmpty(_value1))
                    {
                        var collectorPaymentId = Int32.Parse(_value1);
                        this.SelectedCollectorPayment = SystemManager.GetCollectorPaymentById(collectorPaymentId);
                        this.SelectedPayment = SystemManager.GetPaymentById(this.SelectedCollectorPayment.Payment);
                    }
                    if (this.SelectedCollectorPayment == null)
                        throw new VLException("collectorPaymentId is invalid!");

                    SetValues(this.SelectedCollectorPayment);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }


        void SetValues(VLCollectorPayment collectorPayment)
        {
            this.Payment.Text = string.Format("{0} {1}, at {2}", SelectedPayment.Quantity, SelectedPayment.CreditType, SelectedPayment.PaymentDate.ToShortDateString());
            this.QuantityLimit.Text = this.SelectedCollectorPayment.QuantityLimit.ToString();
            this.QuantityUsed.Text = this.SelectedCollectorPayment.QuantityUsed.ToString();
            if (this.SelectedCollectorPayment.FirstChargeDt.HasValue)
            {
                this.FirstChargeDt.Text = this.SelectedCollectorPayment.FirstChargeDt.ToString();
            } if (this.SelectedCollectorPayment.LastChargeDt.HasValue)
            {
                this.FirstChargeDt.Text = this.SelectedCollectorPayment.LastChargeDt.ToString();
            }
        }
        void GetValues(VLCollectorPayment collectorPayment)
        {
            if (string.IsNullOrWhiteSpace(this.QuantityLimit.Text))
            {
                collectorPayment.QuantityLimit = null;
            }
            else
            {
                collectorPayment.QuantityLimit = Int32.Parse(this.QuantityLimit.Text);
            }
        }

        protected void updateCollectorPayment_Click(object sender, EventArgs e)
        {
            try
            {
                GetValues(this.SelectedCollectorPayment);
                this.SelectedCollectorPayment = SystemManager.UpdateCollectorPayment(this.SelectedCollectorPayment);

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