using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// Μέσα σε αυτή την κλάση έχουμε όλα τα απαραίτητα στοιχεία (με ένα διάβασμα)
    /// για να γεμίσουμε με data, το dashboard του system user
    /// </summary>
    public class VLSystemDashboard
    {
        /// <summary>
        /// Συνολικός αριθμός πελατών στο σύστημα
        /// </summary>
        public Int32 TotalClients { get; set; }
        /// <summary>
        /// Συνολικός αριθμός logins πελατών στο σύστημα
        /// </summary>
        public Int32 TotalClientLogins { get; set; }
        /// <summary>
        /// Συνολικός αριθμός logins system-users στο σύστημα
        /// </summary>
        public Int32 TotalSystemLogins { get; set; }
        /// <summary>
        /// Συνολικός αριθμός ερωτηματολογίων
        /// </summary>
        public Int32 TotalSurveys { get; set; }
        /// <summary>
        /// Συνολικός αριθμός ερωτηματολογίων που διαθέτουν έστω και ένα 'ανοιχτό/ενεργό' collector
        /// </summary>
        public Int32 TotalActiveSurveys { get; set; }
        /// <summary>
        /// Συνολικός αριθμός collectors στο σύστημα
        /// </summary>
        public Int32 TotalCollectors { get; set; }
        /// <summary>
        /// Συνολικός αριθμός ανοιχτών/ενεργών collectors στο σύστημα
        /// </summary>
        public Int32 TotalActiveCollectors { get; set; }
        /// <summary>
        /// Συνολικό πλήθος απαντήσεων που έχουν μαζέψει όλοι οι ανοιχτοί/ενεργοί collectors του συστήματος
        /// </summary>
        public Int32 TotalResponses { get; set; }
        /// <summary>
        /// Συνολικό πλήθος μηνυμάτων που έχουν δημιουργηθεί στους ανοιχτούς/ενεργούς collectors του συστήματος
        /// </summary>
        public Int32 TotalMessages { get; set; }
        /// <summary>
        /// Συνολικό πλήθος αποδεκτών (emails) που απευθύνονται τα TotalMessages.
        /// </summary>
        public Int32 TotalRecipients { get; set; }
        /// <summary>
        /// Συνολικό πλήθος αποδεκτών (emails) που έχουν απευθύνονται και έχουν αποσταλεί για τα TotalMessages.
        /// </summary>
        public Int32 TotalSentRecipients { get; set; }
        /// <summary>
        /// Συνολικά πλήθος των clicks που έχουν μαζέψει οι ανοιχτοί/ενεργοί collectors.
        /// </summary>
        public Int32 TotalClicks { get; set; }
        /// <summary>
        /// Συνολικά πλήθος των clicks που έχουν μαζέψει οι ανοιχτοί/ενεργοί collectors και έχουν δώσει ένα ολοκληρωμένο Response
        /// </summary>
        public Int32 TotalClicksWithResponse { get; set; }

        /// <summary>
        /// Χρονοσειρά με το πλήθος των responses/μερα για τις τελευταίες ημέρες
        /// <para>Ολα τα στοιχεία των χρονοσειρών αφορούν ανοιχτούς/ενεργούς collectors</para>
        /// </summary>
        public Collection<Tuple<String, Int32>> LastResponses { get; set; }
        /// <summary>
        /// Χρονοσειρά με το πλήθος των messages/μερα για τις τελευταίες ημέρες και τις επόμενες 10 ημέρες
        /// <para>Ολα τα στοιχεία των χρονοσειρών αφορούν ανοιχτούς/ενεργούς collectors</para>
        /// </summary>
        public Collection<Tuple<String, Int32>> LastMessages { get; set; }
        /// <summary>
        /// Χρονοσειρά με το πλήθος των messagerecipients/μερα για τις τελευταίες ημέρες και τις επόμενες 10 ημέρες
        /// <para>Ολα τα στοιχεία των χρονοσειρών αφορούν ανοιχτούς/ενεργούς collectors</para>
        /// </summary>
        public Collection<Tuple<String, Int32>> LastMessageRecipients { get; set; }
        /// <summary>
        /// Χρονοσειρά με το πλήθος των clicks/μερα για τις τελευταίες ημέρες
        /// <para>Ολα τα στοιχεία των χρονοσειρών αφορούν ανοιχτούς/ενεργούς collectors</para>
        /// </summary>
        public Collection<Tuple<String, Int32>> LastClicks { get; set; }
        /// <summary>
        /// Χρονοσειρά με το πλήθος των (clicks που συμπλήρωσαν το ερωτηματολόγιο)/μερα για τις τελευταίες ημέρες
        /// <para>Ολα τα στοιχεία των χρονοσειρών αφορούν ανοιχτούς/ενεργούς collectors</para>
        /// </summary>
        public Collection<Tuple<String, Int32>> LastClicksWithResponse { get; set; }

        /// <summary>
        /// Χρονοσειρά με το πλήθος των (client logins)/μερα για τις τελευταίες ημέρες
        /// </summary>
        public Collection<Tuple<String, Int32>> LastLogins { get; set; }

        /// <summary>
        /// Χρονοσειρά με το πλήθος των LogRecords/μερα για τις τελευταίες ημέρες
        /// </summary>
        public Collection<Tuple<String, Int32>> LastLogRecords { get; set; }




        public Int32 LastResponsesMax { get; set; }
        public Int32 LastResponsesTickInterval { get; set; }

        public Int32 LastMessagesMax { get; set; }
        public Int32 LastMessagesTickInterval { get; set; }

        public Int32 LastMessageRecipientsMax { get; set; }
        public Int32 LastMessageRecipientsTickInterval { get; set; }

        public Int32 LastClicksMax { get; set; }
        public Int32 LastClicksTickInterval { get; set; }

        public Int32 LastClicksWithResponseMax { get; set; }
        public Int32 LastClicksWithResponseTickInterval { get; set; }

        public Int32 LastLoginsMax { get; set; }
        public Int32 LastLoginsTickInterval { get; set; }

        public Int32 LastLogRecordsMax { get; set; }
        public Int32 LastLogRecordsTickInterval { get; set; }

        internal VLSystemDashboard(DbDataReader reader)
        {
            this.LastResponses = new Collection<Tuple<string, int>>();
            this.LastMessages = new Collection<Tuple<string, int>>();
            this.LastMessageRecipients = new Collection<Tuple<string, int>>();
            this.LastClicks = new Collection<Tuple<string, int>>();
            this.LastClicksWithResponse = new Collection<Tuple<string, int>>();
            this.LastLogins = new Collection<Tuple<string, int>>();
            this.LastLogRecords = new Collection<Tuple<string, int>>();


            this.TotalClients = reader.GetInt32(0);
            this.TotalClientLogins = reader.GetInt32(1);
            this.TotalSystemLogins = reader.GetInt32(2);
            this.TotalSurveys = reader.GetInt32(3);
            this.TotalActiveSurveys = reader.GetInt32(4);
            this.TotalCollectors = reader.GetInt32(5);
            this.TotalActiveCollectors = reader.GetInt32(6);

            this.TotalResponses = reader.GetInt32(7);
            this.TotalMessages = reader.GetInt32(8);
            this.TotalRecipients = reader.GetInt32(9);
            this.TotalSentRecipients = reader.GetInt32(10);
            this.TotalClicks = reader.GetInt32(11);
            this.TotalClicksWithResponse = reader.GetInt32(12);
        }


        internal void CalculateRanges()
        {
            foreach (var tuple in this.LastResponses)
            {
                if (tuple.Item2 > this.LastResponsesMax)
                    this.LastResponsesMax = tuple.Item2;
            }
            var temp = this.LastResponsesMax / 10;
            if ((temp * 10) <= this.LastResponsesMax)
                this.LastResponsesMax = ((temp + 1) * 10);
            this.LastResponsesTickInterval = this.LastResponsesMax / 10;


            foreach (var tuple in this.LastMessages)
            {
                if (tuple.Item2 > this.LastMessagesMax)
                    this.LastMessagesMax = tuple.Item2;
            }
            temp = this.LastMessagesMax / 10;
            if ((temp * 10) <= this.LastMessagesMax)
                this.LastMessagesMax = ((temp + 1) * 10);
            this.LastMessagesTickInterval = this.LastMessagesMax / 10;


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


            foreach (var tuple in this.LastClicksWithResponse)
            {
                if (tuple.Item2 > this.LastClicksWithResponseMax)
                    this.LastClicksWithResponseMax = tuple.Item2;
            }
            temp = this.LastClicksWithResponseMax / 10;
            if ((temp * 10) <= this.LastClicksWithResponseMax)
                this.LastClicksWithResponseMax = ((temp + 1) * 10);
            this.LastClicksWithResponseTickInterval = this.LastClicksWithResponseMax / 10;


            foreach (var tuple in this.LastLogins)
            {
                if (tuple.Item2 > this.LastLoginsMax)
                    this.LastLoginsMax = tuple.Item2;
            }
            temp = this.LastLoginsMax / 10;
            if ((temp * 10) <= this.LastLoginsMax)
                this.LastLoginsMax = ((temp + 1) * 10);
            this.LastLoginsTickInterval = this.LastLoginsMax / 10;


            foreach (var tuple in this.LastLogRecords)
            {
                if (tuple.Item2 > this.LastLogRecordsMax)
                    this.LastLogRecordsMax = tuple.Item2;
            }
            temp = this.LastLogRecordsMax / 10;
            if ((temp * 10) <= this.LastLogRecordsMax)
                this.LastLogRecordsMax = ((temp + 1) * 10);
            this.LastLogRecordsTickInterval = this.LastLogRecordsMax / 10;
        }
    }
}
