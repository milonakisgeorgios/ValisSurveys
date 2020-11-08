using System;
using System.Globalization;
using System.Threading;
using Valis.Core;

namespace ValisManager.manager.customers.payments
{
    public partial class create : ManagerPage
    {
        protected VLClient SelectedClient
        {
            get
            {
                return this.ViewState["SelectedClient"] as VLClient;
            }
            set
            {
                this.ViewState["SelectedClient"] = value;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα ClientId
                    string _value = this.Request.Params["ClientId"];
                    if (!string.IsNullOrEmpty(_value))
                    {
                        var clientId = Int32.Parse(_value);
                        this.SelectedClient = SystemManager.GetClientById(clientId);
                    }
                    if (this.SelectedClient == null)
                        throw new VLException("ClientId is invalid!");


                }
            }
            catch (Exception ex)
            {
                this.Response.Redirect("../list.aspx");
            }
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                CreditType resourceType = (CreditType)Enum.Parse(typeof(CreditType), this.frmResourceType.SelectedValue);
                Int32 quantity = Int32.Parse(this.Quantity.Text);
                var paymentDate = DateTime.ParseExact(this.PaymentDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                
                var payment = SystemManager.AddPayment(this.SelectedClient.ClientId, resourceType, quantity, paymentDate, PaymentType.Default);

                if (string.IsNullOrWhiteSpace(this.CustomCode1.Text))
                    payment.CustomCode1 = null;
                else
                    payment.CustomCode1 = this.CustomCode1.Text; 

                if(payment.IsDirty)
                {
                    payment = SystemManager.UpdatePayment(payment);
                }

                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", payment.Client)), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }



    }
}