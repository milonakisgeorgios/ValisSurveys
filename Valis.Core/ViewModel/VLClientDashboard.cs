using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// Μέσα σε αυτή την κλάση έχουμε όλα τα απαραίτητα στοιχεία (με ένα διάβασμα)
    /// για να γεμίσουμε με data, το dashboard του πελάτη
    /// </summary>
    public class VLClientDashboard
    {

        public Int32 TotalSurveys { get; set; }
        public Int32 TotalActiveSurveys { get; set; }
        public Int32 TotalCollectors { get; set; }
        public Int32 TotalActiveCollectors { get; set; }
        public Int32 TotalResponses { get; set; }
        public Int32 TotalMessages { get; set; }
        public Int32 TotalRecipients { get; set; }
        public Int32 TotalSentRecipients { get; set; }
        public Int32 TotalClicks { get; set; }


        public Collection<Tuple<String, Int32>> LastResponses { get; set; }
        public Collection<Tuple<String, Int32>> LastMessages { get; set; }
        public Collection<Tuple<String, Int32>> LastMessageRecipients { get; set; }

        public Collection<Tuple<String, Int32>> LastClicks { get; set; }
        public Collection<Tuple<String, Int32>> LastClicksWithResponse { get; set; }

        public Int32 LastResponsesMax { get; set; }
        public Int32 LastResponsesTickInterval { get; set; }

        public Int32 LastMessageRecipientsMax { get; set; }
        public Int32 LastMessageRecipientsTickInterval { get; set; }


        public Int32 LastClicksMax { get; set; }
        public Int32 LastClicksTickInterval { get; set; }

        internal VLClientDashboard(DbDataReader reader)
        {
            this.LastResponses = new Collection<Tuple<string, int>>();
            this.LastMessages = new Collection<Tuple<string, int>>();
            this.LastMessageRecipients = new Collection<Tuple<string, int>>();
            this.LastClicks = new Collection<Tuple<string, int>>();
            this.LastClicksWithResponse = new Collection<Tuple<string, int>>();


            this.TotalSurveys = reader.GetInt32(0);
            this.TotalActiveSurveys = reader.GetInt32(1);
            this.TotalCollectors = reader.GetInt32(2);
            this.TotalActiveCollectors = reader.GetInt32(3);

            this.TotalResponses = reader.GetInt32(4);
            this.TotalMessages = reader.GetInt32(5);
            this.TotalRecipients = reader.GetInt32(6);
            this.TotalSentRecipients = reader.GetInt32(7);
            this.TotalClicks = reader.GetInt32(8);
        }

        internal void CalculateRanges()
        {

            foreach(var tuple in this.LastResponses)
            {
                if (tuple.Item2 > this.LastResponsesMax)
                    this.LastResponsesMax = tuple.Item2;
            }
            var temp = this.LastResponsesMax / 10;
            if ((temp * 10) <= this.LastResponsesMax)
                this.LastResponsesMax = ((temp + 1) * 10);
            this.LastResponsesTickInterval = this.LastResponsesMax / 10;


            foreach (var tuple in this.LastMessageRecipients)
            {
                if (tuple.Item2 > this.LastMessageRecipientsMax)
                    this.LastMessageRecipientsMax = tuple.Item2;
            }
            temp = this.LastMessageRecipientsMax / 10;
            if ((temp * 10) <= this.LastMessageRecipientsMax)
                this.LastMessageRecipientsMax = ((temp + 1) * 10);
            this.LastMessageRecipientsTickInterval = this.LastMessageRecipientsMax / 10;


            foreach (var tuple in this.LastClicks)
            {
                if (tuple.Item2 > this.LastClicksMax)
                    this.LastClicksMax = tuple.Item2;
            }
            temp = this.LastClicksMax / 10;
            if ((temp * 10) <= this.LastClicksMax)
                this.LastClicksMax = ((temp + 1) * 10);
            this.LastClicksTickInterval = this.LastClicksMax / 10;
        }
    }
}
