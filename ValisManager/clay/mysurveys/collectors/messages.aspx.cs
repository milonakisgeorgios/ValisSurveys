using System;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class messages : CollectorsPage
    {
        Int32 m_TotalDraftMessages = -1;
        Int32 m_TotalNonDraftMessages = -1;



        protected Int32 TotalDraftMessages
        {
            get
            {
                return m_TotalDraftMessages;
            }
        }

        protected Int32 TotalNonDraftMessages
        {
            get
            {
                return m_TotalNonDraftMessages;
            }
        }

        protected bool ShowVerifySenderSuccessMessage
        {
            get
            {
                if (string.IsNullOrEmpty(Request.Params["senderverified"]))
                {
                    return false;
                }
                var senderverified = Request.Params["senderverified"];
                if(string.Equals(senderverified, "1"))
                {
                    return true;
                }
                return false;
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);


            m_TotalDraftMessages = SurveyManager.GetDraftMessagesCount(this.CollectorId);
            m_TotalNonDraftMessages = SurveyManager.GetNonDraftMessagesCount(this.CollectorId);
        }
    }
}