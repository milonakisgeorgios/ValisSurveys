using System;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccessToken
    {
        /// <summary>
        /// Αυτό είναι το id που περιγράφει την τρέχουσα συνεδρία ενός account με το σύστημά μας.
        /// <para>(Οι συνεδρίες καταγράφονται στον πίνακα AccessTokens)</para>
        /// </summary>
        Int32 AccessTokenId { get; }
        /// <summary>
        /// Αυτό είναι το UserId του χρήστη (account) που έχει δημιουργήσει την τρέχουσα συνεδρία. 
        /// <para>Υπάρχουν δύο ειδών χρηστών στο σύστημά μας που μπορούν να κάνουν login. Οι SystemUsers που στην ουσία δουλεύουν και υποστηρίζουν το σύστημα,
        /// και οι ClientUsers που έίναι οι πελάτες του συστήματος που κάνουν login για αν χρησιμοποιήσουν το σύστημα και τις παρεχόμενες υπηρεσίες του.</para>
        /// <para>Οι μέν SystemUsers αποθηκεύονται στον πίνακα SystemUsers, ενώ οι ClientUsers ανήκον πάντα σε ένα πελάτη και αποθηκεύονται στον πίνακα ClientUsers. Και οι δύο
        /// πίνακες μοιράζονται το ίδιο sequence για τη παραγωγή του Primary Key τους.</para>
        /// </summary>
        Int32 Principal { get; }
        /// <summary>
        /// Μας λέει εάν το συγκεκριμένο account της παρούσας συνεδρίας είναι ένα ClientUser ή ένα SystemUser.
        /// </summary>
        PrincipalType PrincipalType { get; }

        /// <summary>
        /// Το id του πελάτη εάν το account της συνεδρίας είναι ένας ClientUser
        /// </summary>
        Int32? ClientId { get; }
        /// <summary>
        /// Η προκαθορισμένη γλώσσα εργασίας για τον συγκεκριμένο χρήστη.
        /// </summary>
        Int16 DefaultLanguage { get; }
        /// <summary>
        /// Η μάσκα με τα permissions για το συγκεκριμένο account.
        /// <para>Διαβάζονται απο το ρόλο του SystemUser ή του ClientUser</para>
        /// </summary>
        VLPermissions Permissions { get; }

        string LogOnToken { get; }
        string Email { get; }
        string FirstName { get; }
        string LastName { get; }
        Boolean IsBuiltIn { get; }

        string TimeZoneId { get; }
        /// <summary>
        /// Εάν το account είναι τύπου ClientUser, τότε αυτό είναι το Profile του Client στον οποίο
        /// ανήκει το account.
        /// </summary>
        Int32? Profile { get; }
        /// <summary>
        /// Εάν το account είναι τύπου ClientUser, τότε αυτό μας λέει εάν ο client χρεώνεται credits για την λειτουργία των Collectors.
        /// <para>Στην ουσία είναι αντιγραφή του πεδίου ClientProfiles.UseCredits</para>
        /// </summary>
        Boolean? UseCredits { get; }
        /// <summary>
        /// 
        /// </summary>
        string ClientName { get; }


        DateTime ConvertTimeToUtc(DateTime value);
        DateTime ConvertTimeFromUtc(DateTime value);
    }
}
