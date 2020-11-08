
namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public enum MessageRecipientStatus : byte
    {
        /// <summary>
        /// Μόλις πρωτοδημιουργήθηκε
        /// </summary>
        New = 0,
        /// <summary>
        /// Σε αναμονή για αποστολή
        /// </summary>
        Pending = 1,
        /// <summary>
        /// Στάλθηκε
        /// </summary>
        Sent = 2,
        /// <summary>
        /// Αποτυχία κατα την αποστολή
        /// </summary>
        Failed = 3,
        /// <summary>
        /// Αποτυχία κατα την αποστολή. Δεν υπήρχε διαθέσιμο ποσό για να αφαιρεθεί απο τις Πληρωμές του Πελάτη
        /// </summary>
        NoCredit = 4,
        /// <summary>
        /// Αποτυχία κατα την αποστολή. Ο πελάτης έχει ξεπεράσει κάποιο ανώτατη τιμής λειτουργίας
        /// </summary>
        NoQuota =5,
        /// <summary>
        /// Αποτυχία κατα την αποστολή
        /// <para>O Recipient έχει κάνει OptedOut, μετά την προετοιμασία του μηνύματος</para>
        /// </summary>
        OptedOut = 6,
        /// <summary>
        /// Αποτυχία κατα την αποστολή
        /// <para>O Recipient έχει γίνει Bounced, μετά την προετοιμασία του μηνύματος</para>
        /// </summary>
        Bounced = 7
    }
}
