using System;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class charges : CollectorsPage
    {
        Int32? m_totalCollectorPayments = null;
        protected Int32 TotalCollectorPayments
        {
            get
            {
                if(m_totalCollectorPayments.HasValue == false)
                {
                    m_totalCollectorPayments = SystemManager.GetCollectorPayments(this.SelectedCollector.CollectorId).Count;
                }
                return m_totalCollectorPayments.Value;
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