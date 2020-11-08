using System;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ContactImportResult
    {
        /// <summary>
        /// Πόσες γραμμές μπήκαν με επιτυχία?
        /// </summary>
        public Int32 SuccesfullImports { get; set; }

        /// <summary>
        /// Πόσες γραμμές δεν ήταν δυνατόν να εισαχθούν (δεν μετράνε τα invalidEmails ή τα SameEmails)
        /// </summary>
        public Int32 FailedImports { get; set; }
        /// <summary>
        /// Πόσα emails ήταν κακοσχηματισμένο (με λάθος format)?
        /// </summary>
        public Int32 InvalidEmails { get; set; }


        /// <summary>
        /// Πόσα ίδια emails εντοπίστηκαν κατα το import?
        /// </summary>
        public Int32 SameEmails { get; set; }

        /// <summary>
        /// Πόσα OptedOut emails εντοπίστηκαν κατα το import?
        /// </summary>
        public Int32 OptedOutEmails { get; set; }

        /// <summary>
        /// Πόσα Bounced emails εντοπίστηκαν κατα το import?
        /// </summary>
        public Int32 BouncedEmails { get; set; }
    }
}
