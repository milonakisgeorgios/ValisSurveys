using System;
using System.Globalization;
using System.Threading;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.manager.customers.payments
{
    public partial class edit : ManagerPage
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


        protected VLPayment SelectedPayment
        {
            get
            {
                return this.ViewState["SelectedPayment"] as VLPayment;
            }
            set
            {
                this.ViewState["SelectedPayment"] = value;
            }
        }


        protected string GetDeleteButtonHandler
        {
            get
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this.deleteBtn);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = false;
                myPostBackOptions.PerformValidation = false;
                myPostBackOptions.ClientSubmit = true;

                return Page.ClientScript.GetPostBackEventReference(myPostBackOptions);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if(!this.IsPostBack)
                {
                    //Πρέπει στο url να υπάρχει ένα ClientId
                    string _value1 = this.Request.Params["ClientId"];
                    if (!string.IsNullOrEmpty(_value1))
                    {
                        var clientId = Int32.Parse(_value1);
                        this.SelectedClient = SystemManager.GetClientById(clientId);
                    }
                    if (this.SelectedClient == null)
                        throw new VLException("ClientId is invalid!");

                    //Πρέπει στο url να υπάρχει ένα PaymentId
                    string _value2 = this.Request.Params["PaymentId"];
                    if (!string.IsNullOrEmpty(_value2))
                    {
                        var paymentId = Int32.Parse(_value2);
                        this.SelectedPayment = SystemManager.GetPaymentById(paymentId);
                    }
                    if (this.SelectedPayment == null)
                        throw new VLException("PaymentId is invalid!");

                    SetValues(this.SelectedPayment);
                }
            }
            catch (Exception)
            {
                this.Response.Redirect(_UrlSuffix("../list.aspx"));
            }
        }

        void SetValues(VLPayment payment)
        {
            this.PaymentDate.Text = payment.PaymentDate.ToString("yyyy-MM-dd");
            this.Comment.Text = payment.Comment;
            this.CustomCode1.Text = payment.CustomCode1;
            this.CustomCode2.Text = payment.CustomCode2;
            this.frmResourceType.SelectedValue = ((Int32)payment.CreditType).ToString();
            this.Quantity.Text = payment.Quantity.ToString(CultureInfo.InvariantCulture);
            this.QuantityUsed.Text = payment.QuantityUsed.ToString(CultureInfo.InvariantCulture);

            if (payment.QuantityUsed > 0)
            {
                this.frmResourceType.Enabled = false;
            }
            else
            {
                var collectorPayments = SystemManager.GetCollectorPaymentsForPayment(this.SelectedPayment.PaymentId);
                if (collectorPayments.Count > 0)
                {
                    this.frmResourceType.Enabled = false;
                }
            }
        }
        void GetValues(VLPayment payment)
        {
            payment.CreditType = (CreditType)Enum.Parse(typeof(CreditType), this.frmResourceType.SelectedValue);
            payment.Quantity = Int32.Parse(this.Quantity.Text);
            payment.PaymentDate = DateTime.ParseExact(this.PaymentDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            if (string.IsNullOrWhiteSpace(this.CustomCode1.Text))
                payment.CustomCode1 = null;
            else
                payment.CustomCode1 = this.CustomCode1.Text; 
            if (string.IsNullOrWhiteSpace(this.CustomCode2.Text))
                payment.CustomCode2 = null;
            else
                payment.CustomCode2 = this.CustomCode2.Text;
            payment.Comment = this.Comment.Text;
        }

        protected void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                GetValues(SelectedPayment);

                SelectedPayment = SystemManager.UpdatePayment(SelectedPayment);

                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", SelectedPayment.Client)), false);
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

        protected void deleteBtn_Click(object sender, EventArgs e)
        {
            try
            {
                SystemManager.DeletePayment(this.SelectedPayment);

                this.Response.Redirect(_UrlSuffix(string.Format("../edit.aspx?ClientId={0}", SelectedPayment.Client)), false);
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