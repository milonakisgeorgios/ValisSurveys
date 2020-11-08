using System;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class details_email : CollectorsPage
    {
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
        protected Int32 TotalRecipientsEmailed
        {
            get
            {
                return SurveyManager.GetEmailedRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalRecipientsResponded
        {
            get
            {
                return SurveyManager.GetRespondedRecipientsCount(this.CollectorId);
            }
        }


        protected Int32 TotalMessages
        {
            get
            {
                return SurveyManager.GetMessagesCount(this.CollectorId);
            }
        }
        protected Int32 TotalDraftMessages
        {
            get
            {
                return SurveyManager.GetDraftMessagesCount(this.CollectorId);
            }
        }
        protected Int32 TotalScheduledMessages
        {
            get
            {
                return SurveyManager.GetScheduledMessagesCount(this.CollectorId);
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {

            }
        }


    }
}