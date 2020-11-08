using System;

namespace ValisManager.clay.controls
{
    public partial class balanceStatistics : ManagerUserControl
    {

        public Int32 SurveyClientId { get; set; }


        protected Int32 EmailsCreditsBalance { get; set; }
        protected Int32 EmailsCreditsReserved { get; set; }
        protected Int32 EmailsCreditsTotal { get; set; }

        protected Int32 ResponsesCreditsBalance { get; set; }
        protected Int32 ResponsesCreditsReserved { get; set; }
        protected Int32 ResponsesCreditsTotal { get; set; }

        protected Int32 ClicksCreditsBalance { get; set; }
        protected Int32 ClicksCreditsReserved { get; set; }
        protected Int32 ClicksCreditsTotal { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            var balances = SystemManager.GetBalances(SurveyClientId);
            foreach (var item in balances)
            {
                if (item.CreditType == Valis.Core.CreditType.EmailType)
                {
                    this.EmailsCreditsBalance = item.Balance;
                    this.EmailsCreditsReserved = item.Reserved;
                    this.EmailsCreditsTotal = item.Balance - item.Reserved;
                }
                else if (item.CreditType == Valis.Core.CreditType.ResponseType)
                {
                    this.ResponsesCreditsBalance = item.Balance;
                    this.ResponsesCreditsReserved = item.Reserved;
                    this.ResponsesCreditsTotal = item.Balance - item.Reserved;
                }
                else if (item.CreditType == Valis.Core.CreditType.ClickType)
                {
                    this.ClicksCreditsBalance = item.Balance;
                    this.ClicksCreditsReserved = item.Reserved;
                    this.ClicksCreditsTotal = item.Balance - item.Reserved;
                }
            }
        }
    }
}