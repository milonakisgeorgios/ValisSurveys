using System;
using System.Threading;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    /// <summary>
    /// ΔΕΝ ΕΙΝΑΙ ΑΠΑΡΑΙΤΗΤΟ ΝΑ ΚΛΗΘΕΙ ΜΕ MessageID!!!
    /// </summary>
    public partial class editMessage_recipients : CollectorsPage
    {
        #region Grab MessageId from Url
        public Int32? MessageId
        {
            get
            {
                Object _obj = this.ViewState["MessageId"];
                if (_obj == null) return null;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["MessageId"] = value;
            }
        }

        protected VLMessage SelectedMessage
        {
            get
            {
                if (this.Context.Items["SelectedMessage"] == null)
                {
                    if (this.MessageId.HasValue)
                    {
                        this.Context.Items["SelectedMessage"] = SurveyManager.GetMessageById(this.MessageId.Value);
                    }
                    else
                    {
                        return null;
                    }
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
                    if (Request.Params["messageId"] != null)
                    {
                        this.MessageId = Int32.Parse(Request.Params["messageId"]);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
        #endregion

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            if (this.IsPostBack == false)
            {
                this.ddlSearchField.Items.Clear();
                this.ddlSearchField.Items.Add(new ListItem("EmailAddress", "0"));
                this.ddlSearchField.Items.Add(new ListItem("FirstName", "1"));
                this.ddlSearchField.Items.Add(new ListItem("LastName", "2"));

                this.ddlCriteria.Items.Clear();
                this.ddlCriteria.Items.Add(new ListItem("Equals", "0"));
                this.ddlCriteria.Items.Add(new ListItem("StartsWith", "1"));
                this.ddlCriteria.Items.Add(new ListItem("EndsWith", "2"));
                this.ddlCriteria.Items.Add(new ListItem("Contains", "3"));

                this.rdbtnNewAndUnsent.Checked = true;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.IsPostBack == false)
            {
                if (this.SelectedMessage != null)
                {
                    if (this.SelectedMessage.DeliveryMethod == DeliveryMethod.All)
                        this.rdbtnAll.Checked = true;
                    else if (this.SelectedMessage.DeliveryMethod == DeliveryMethod.AllResponded)
                        this.rdbtnAllResponded.Checked = true;
                    else if (this.SelectedMessage.DeliveryMethod == DeliveryMethod.Custom) 
                    { 
                        this.rdbtnCustom.Checked = true;
                        this.ddlCriteria.SelectedValue = ((byte)this.SelectedMessage.CustomSearchField.Value).ToString();
                        this.ddlSearchField.SelectedValue = ((byte)this.SelectedMessage.CustomSearchField.Value).ToString();
                        this.txtKeyword.Text = this.SelectedMessage.CustomKeyword;
                    }
                    else if (this.SelectedMessage.DeliveryMethod == DeliveryMethod.NewAndUnsent)
                        this.rdbtnNewAndUnsent.Checked = true;
                    else if (this.SelectedMessage.DeliveryMethod == DeliveryMethod.NotResponded)
                        this.rdbtnNotResponded.Checked = true;
                }
            }
        }


        /// <summary>
        /// Το σύνολο των recipients για τον επιλεγμένο Collector
        /// </summary>
        protected Int32 TotalRecipients
        {
            get
            {
                return SurveyManager.GetRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalNotEmailedRecipients
        {
            get
            {
                return SurveyManager.GetNotEmailedRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalNotRespondedRecipients
        {
            get
            {
                return SurveyManager.GetNotRespondedRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalRespondedRecipients
        {
            get
            {
                return SurveyManager.GetRespondedRecipientsCount(this.CollectorId);
            }
        }


        protected void saveAndContinue_Click(object sender, EventArgs e)
        {
            if (this.SelectedMessage != null)
            {
                updateExistingMessage();
            }
            else
            {
                createNewMessage();
            }
        }

        void createNewMessage()
        {
            VLMessage message = null;
            try
            {
                //Θα δημιουργήσουμε ένα καινούργιο message

                if (this.rdbtnNewAndUnsent.Checked)
                {
                    message = SurveyManager.CreateMessage(this.CollectorId);
                    message.DeliveryMethod = Valis.Core.DeliveryMethod.NewAndUnsent;
                }
                else if (this.rdbtnNotResponded.Checked)
                {
                    message = SurveyManager.CreateMessage(this.CollectorId);
                    message.DeliveryMethod = Valis.Core.DeliveryMethod.NotResponded;
                }
                else if (this.rdbtnAllResponded.Checked)
                {
                    message = SurveyManager.CreateMessage(this.CollectorId);
                    message.DeliveryMethod = Valis.Core.DeliveryMethod.AllResponded;
                }
                else if (this.rdbtnAll.Checked)
                {
                    message = SurveyManager.CreateMessage(this.CollectorId);
                    message.DeliveryMethod = Valis.Core.DeliveryMethod.All; ;
                }
                else if (this.rdbtnCustom.Checked)
                {
                    if (string.IsNullOrWhiteSpace(this.txtKeyword.Text))
                        throw new VLException("Custom Criteria are invalid!");
                    if (string.IsNullOrWhiteSpace(ddlSearchField.SelectedValue))
                        throw new VLException("Custom Criteria are invalid!");
                    if (string.IsNullOrWhiteSpace(ddlCriteria.SelectedValue))
                        throw new VLException("Custom Criteria are invalid!");

                    message = SurveyManager.CreateMessage(this.CollectorId);
                    message.DeliveryMethod = Valis.Core.DeliveryMethod.Custom;
                    message.CustomSearchField = (RecipientSearchField)Enum.Parse(typeof(RecipientSearchField), this.ddlSearchField.SelectedValue);
                    message.CustomOperator = (ComparisonOperator)Enum.Parse(typeof(ComparisonOperator), this.ddlCriteria.SelectedValue);
                    message.CustomKeyword = txtKeyword.Text;
                }

                message.IsDeliveryMethodOK = true;
                message = SurveyManager.UpdateMessage(message);

                Response.Redirect(string.Format("~/clay/mysurveys/collectors/editMessage_content.aspx?surveyId={0}&collectorId={1}&messageId={2}&textslanguage={3}", this.Surveyid, this.CollectorId, message.MessageId, this.TextsLanguage), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch(ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
                if (message != null)
                {
                    SurveyManager.DeleteMessage(message);
                }
            }
        }

        void updateExistingMessage()
        {
            try
            {
                if (this.rdbtnNewAndUnsent.Checked)
                {
                    this.SelectedMessage.DeliveryMethod = Valis.Core.DeliveryMethod.NewAndUnsent;
                }
                else if (this.rdbtnNotResponded.Checked)
                {
                    this.SelectedMessage.DeliveryMethod = Valis.Core.DeliveryMethod.NotResponded;
                }
                else if (this.rdbtnAllResponded.Checked)
                {
                    this.SelectedMessage.DeliveryMethod = Valis.Core.DeliveryMethod.AllResponded;
                }
                else if (this.rdbtnAll.Checked)
                {
                    this.SelectedMessage.DeliveryMethod = Valis.Core.DeliveryMethod.All; ;
                }
                else if (this.rdbtnCustom.Checked)
                {
                    if (string.IsNullOrWhiteSpace(this.txtKeyword.Text))
                        throw new VLException("Custom Criteria are invalid!");
                    if (string.IsNullOrWhiteSpace(ddlSearchField.SelectedValue))
                        throw new VLException("Custom Criteria are invalid!");
                    if (string.IsNullOrWhiteSpace(ddlCriteria.SelectedValue))
                        throw new VLException("Custom Criteria are invalid!");

                    this.SelectedMessage.DeliveryMethod = Valis.Core.DeliveryMethod.Custom;
                    this.SelectedMessage.CustomSearchField = (RecipientSearchField)Enum.Parse(typeof(RecipientSearchField), this.ddlSearchField.SelectedValue);
                    this.SelectedMessage.CustomOperator = (ComparisonOperator)Enum.Parse(typeof(ComparisonOperator), this.ddlCriteria.SelectedValue);
                    this.SelectedMessage.CustomKeyword = txtKeyword.Text;
                }

                this.SelectedMessage.IsDeliveryMethodOK = true;
                var updatedMessage = SurveyManager.UpdateMessage(this.SelectedMessage);

                Response.Redirect(string.Format("~/clay/mysurveys/collectors/message_preview.aspx?surveyId={0}&collectorId={1}&messageId={2}&textslanguage={3}", this.Surveyid, this.CollectorId, updatedMessage.MessageId, this.TextsLanguage), false);
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