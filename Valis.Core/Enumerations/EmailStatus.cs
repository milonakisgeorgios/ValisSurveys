
namespace Valis.Core
{
    /// <summary>
    /// απαριθμέι τα statuses στα οποίο μπορεί να βρίσκεται ενα
    /// email που πρέπει να σταλεί σε κάποιον παραλήπτη απο το σύστημά μας.
    /// </summary>
    public enum EmailStatus : byte
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
        /// 
        /// </summary>
        Executing = 2,
        /// <summary>
        /// Στάλθηκε
        /// </summary>
        Sent = 3,
        /// <summary>
        /// Αποτυχία κατα την αποστολή
        /// </summary>
        Failed = 4,
    }
}
