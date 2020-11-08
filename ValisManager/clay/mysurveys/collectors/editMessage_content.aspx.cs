using System;
using System.Threading;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    /// <summary>
    /// ΕΙΝΑΙ ΑΠΑΡΑΙΤΗΤΟ ΝΑ ΚΛΗΘΕΙ ΜΕ MessageID!!!
    /// </summary>
    public partial class editMessage_content : CollectorsPage
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
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
        #endregion


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                this.txtReplyEmail.Text = this.SelectedMessage.Sender;
                this.txtSubject.Text = this.SelectedMessage.Subject;
                this.txtBody.Text = this.SelectedMessage.Body;
            }
        }

        protected void saveAndContinue_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedMessage.Sender = this.txtReplyEmail.Text;
                this.SelectedMessage.Subject = this.txtSubject.Text;
                this.SelectedMessage.Body = this.txtBody.Text;

                this.SelectedMessage.IsContentOK = true;
                var updatedMessage = SurveyManager.UpdateMessage(this.SelectedMessage);

                if(updatedMessage.IsSenderOK == false)
                {
                    this.ErrorMessage = "The message was saved, but there is an error below.";
                }
                else
                {
                    Response.Redirect(string.Format("~/clay/mysurveys/collectors/message_preview.aspx?surveyId={0}&collectorId={1}&messageId={2}&textslanguage={3}", this.Surveyid, this.CollectorId, updatedMessage.MessageId, this.TextsLanguage), false);
                    this.Context.ApplicationInstance.CompleteRequest();
                }
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