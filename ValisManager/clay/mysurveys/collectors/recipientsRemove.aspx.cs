using System;
using System.Threading;
using System.Web.UI;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class recipientsRemove : CollectorsPage
    {

        protected string GetRemoveButtonHandler
        {
            get
            {
                PostBackOptions myPostBackOptions = new PostBackOptions(this.btnRemoveRecipients);
                myPostBackOptions.AutoPostBack = false;
                myPostBackOptions.RequiresJavaScriptProtocol = false;
                myPostBackOptions.PerformValidation = false;
                myPostBackOptions.ClientSubmit = true;

                return Page.ClientScript.GetPostBackEventReference(myPostBackOptions);
            }
        }


        protected void btnRemoveRecipients_Click(object sender, EventArgs e)
        {
            try
            {
                if(this.rdbtnUnsent.Checked)
                {
                    //Remove All Unsent/New Recipients
                    RemoveAllUnsent();
                }
                else if (this.rdbtnOptOut.Checked)
                {
                    //Remove All Opted-Out Recipients
                    RemoveAllOptedOut();
                }
                else if (this.rdbtnBounced.Checked)
                {
                    //Remove All Bounced Email Recipients
                    RemoveAllBounced();
                }
                else if (this.rdbtnDomain.Checked)
                {
                    //Remove All Contacts by Domain Name
                    RemoveAllByDomain();
                }

                Response.Redirect(string.Format("recipients.aspx?surveyid={0}&collectorId={1}&textslanguage={2} ", this.Surveyid, this.CollectorId, this.TextsLanguage), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch(Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        void RemoveAllUnsent()
        {
            var deletedRows = SurveyManager.RemoveAllUnsentRecipients(this.CollectorId);
        }
        void RemoveAllOptedOut()
        {
            var deletedRows = SurveyManager.RemoveAllOptedOutRecipients(this.CollectorId);
        }
        void RemoveAllBounced()
        {
            var deletedRows = SurveyManager.RemoveAllBouncedRecipients(this.CollectorId);
        }
        void RemoveAllByDomain()
        {
            var deletedRows = SurveyManager.RemoveByDomainRecipients(this.CollectorId, this.txtDomainName.Text);
        }


    }
}