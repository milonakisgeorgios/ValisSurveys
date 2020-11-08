using System;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class recipients : CollectorsPage
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
        /// Το σύνολο των recipients στους οποίους έχει σταλεί email
        /// </summary>
        protected Int32 TotalEmailedRecipients
        {
            get
            {
                return SurveyManager.GetEmailedRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// Το σύνολο των recipients στους οποίους δεν έχει σταλεί email
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

        protected Int32 TotalPartiallyRespondeRecipients
        {
            get
            {
                return SurveyManager.GetPartiallyRespondeRecipientsCount(this.CollectorId);
            }
        }

        protected Int32 TotalFullRespondeRecipients
        {
            get
            {
                return SurveyManager.GetFullRespondeRecipientsCount(this.CollectorId);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalBouncedRecipients
        {
            get
            {
                return SurveyManager.GetBouncedRecipientsCount(this.CollectorId);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected Int32 TotalOptedOutRecipients
        {
            get
            {
                return SurveyManager.GetOptedOutRecipientsCount(this.CollectorId);
            }
        }


    }
}