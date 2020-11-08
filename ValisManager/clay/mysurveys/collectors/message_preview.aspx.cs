using System;
using System.Text;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    /// <summary>
    /// ΕΙΝΑΙ ΑΠΑΡΑΙΤΗΤΟ ΝΑ ΚΛΗΘΕΙ ΜΕ MessageID!!!
    /// </summary>
    public partial class message_preview : CollectorsPage
    {
        #region Grab MessageId from Url
        public Int32 MessageId
        {
            get
            {
                Object _obj = this.ViewState["MessageId"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["MessageId"] = value;
            }
        }
        public string PaymentValidationErrorMessage
        {
            get
            {
                Object _obj = this.ViewState["PaymentValidationErrorMessage"];
                if (_obj == null) return string.Empty;
                return (string)_obj;
            }
            set
            {
                this.ViewState["PaymentValidationErrorMessage"] = value;
            }
        }
        public bool IsPaymentValid
        {
            get
            {
                Object _obj = this.ViewState["IsPaymentValid"];
                if (_obj == null) return false;
                return (bool)_obj;
            }
            set
            {
                this.ViewState["IsPaymentValid"] = value;
            }
        }

        protected VLMessage SelectedMessage
        {
            get
            {
                if (this.Context.Items["SelectedMessage"] == null)
                {
                    this.Context.Items["SelectedMessage"] = SurveyManager.GetMessageById(this.MessageId);
                }
                return (VLMessage)this.Context.Items["SelectedMessage"];
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            try
            {
                if (this.IsPostBack == false)
                {
                    if (string.IsNullOrEmpty(Request.Params["messageId"]))
                        throw new ArgumentNullException("messageId");
                    this.MessageId = Int32.Parse(Request.Params["messageId"]);

                    string _PaymentValidationErrorMessage = string.Empty;
                    this.IsPaymentValid = SurveyManager.IsPaymentValidforScheduling(this.SelectedCollector, this.SelectedMessage, out _PaymentValidationErrorMessage);
                    this.PaymentValidationErrorMessage = _PaymentValidationErrorMessage;
                }

            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
        #endregion


        protected string GetDeliveryMethodHtml()
        {
            switch (this.SelectedMessage.DeliveryMethod)
            {
                case DeliveryMethod.All:
                    return "<span class=\"panelValue\">All</span>";
                case DeliveryMethod.AllResponded:
                    return "<span class=\"panelValue\">All Responded</span>";
                case DeliveryMethod.Custom:
                    return "<span class=\"panelValue\">Custom</span>";
                case DeliveryMethod.NewAndUnsent:
                    return "<span class=\"panelValue\">New/Unsent</span>";
                case DeliveryMethod.NotResponded:
                    return "<span class=\"panelValue\">Not Responded</span>";
            }
            return string.Empty;
        }
        protected string GetTotalRecipientsHtml()
        {
            if (this.SelectedMessage.Status == MessageStatus.Draft || this.SelectedMessage.Status == MessageStatus.Pending)
            {
                var totalRecipients = SurveyManager.CompileRecipientsCountForMessage(this.SelectedMessage);

                if (totalRecipients == 0)
                    return "<span class=\"panelValue\">No Recipients</span>";
                else if (totalRecipients == 1)
                    return "<span class=\"panelValue\">1 Recipient</span>";
                else
                    return string.Format("<span class=\"panelValue\">{0} Recipients</span>", totalRecipients);
            }
            else
            {
                return string.Format("The message mailed to {0} recipient(s).", this.SelectedMessage.SentCounter);
            }
        }


        protected string GetIntentionHtml()
        {
            if (this.SelectedMessage.Status == MessageStatus.Draft || this.SelectedMessage.Status == MessageStatus.Pending)
            {
                return "EMAIL WILL SEND";
            }
            else if (this.SelectedMessage.Status == Valis.Core.MessageStatus.Preparing || this.SelectedMessage.Status == Valis.Core.MessageStatus.Prepared || this.SelectedMessage.Status == Valis.Core.MessageStatus.Executing)
            {
                return "Currently Mailing";
            }
            else
            {
                return "EMAIL SENT";
            }
        }
        protected string GetScheduleHtml()
        {
            if (this.SelectedMessage.ScheduledAt.HasValue)
            {
                var scheduleAt = SurveyManager.ConvertTimeFromUtc(this.SelectedMessage.ScheduledAt.Value);
                return scheduleAt.ToString(Utilities.DateTime_Format_General);
            }
            else
            {
                return "Not yet scheduled";
            }
        }

        protected string GetMessageHtml()
        {
            try
            {
                StringBuilder sb = new StringBuilder("<table border=\"0\" cellspacing=\"0\" cellpadding=\"2\"><tbody>");

                sb.Append("<tr><td class=\"msgField\">To:</td><td class=\"msgValue\"><span id=\"litTo\">");
                sb.AppendFormat("somebody@domain.com");
                sb.Append("</span></td></tr>");

                sb.Append("<tr><td class=\"msgField\">From:</td><td class=\"msgValue\"><span id=\"litFrom\">");
                sb.AppendFormat(Server.HtmlEncode(this.SelectedMessage.Sender));
                sb.Append("</span></td></tr>");

                sb.Append("<tr><td class=\"msgField\">Subject:</td><td class=\"msgValue\"><span id=\"litSubject\">");
                sb.AppendFormat(Server.HtmlEncode(this.SelectedMessage.Subject));
                sb.Append("</span></td></tr>");

                sb.Append("<tr><td class=\"msgField\">Body:</td><td class=\"msgValue\"><span id=\"litBody\">");
                sb.AppendFormat(Server.HtmlEncode(this.SelectedMessage.Body).Replace("[SurveyLink]", "[<span class=\"highlight\">SurveyLink</span>]").Replace("[RemoveLink]", "[<span class=\"highlight\">RemoveLink</span>]"));
                sb.Append("</span></td></tr>");

                sb.Append("</tbody></table>");

                return sb.ToString();
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }

            return string.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

        }
    }
}