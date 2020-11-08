using System;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum VLPermissions : long
    {
        /// <summary>
        /// 
        /// </summary>
        None                                = 0L,

/********PERMISSIONS FOR SYSTEM-USERS**************************************************************************************************/
/********PERMISSIONS FOR SYSTEM-USERS**************************************************************************************************/



        /// <summary>
        /// This permission allows the manipulation of the system. It is assigned only to
        /// system admins, and is not intended to be used by any client code.
        /// </summary>
        ManageSystem                        = 1L,                   // 1 << 0
        /// <summary>
        /// This permission allows the use of special debigging tools in order to help the developers
        /// of the system.
        /// </summary>
        Developer                           = 2L,                   // 1 << 1
        /// <summary>
        /// Αυτό το δικαίωμα το πέρνουν accounts που αφορούν services που υποστηρίζουν την υπηρεσία μας
        /// </summary>
        SystemService                       = 4L,                   // 1 << 2
        
        /// <summary>
        /// Μπορεί να δεί entities σχετικές με το security (Roles, SystemUsers, Credentials, Logins, ....)
        /// </summary>
        EnumerateSecurity                   = 32L,                  // 1 << 5
        /// <summary>
        /// Δίνει το δικαίωμα της δημιουργίας entities σχετικά με το security (Roles, SystemUsers, Credentials, ...)
        /// </summary>
        ManageSecurity                      = 64L,                  // 1 << 6

        /// <summary>
        /// Μπορεί να βλέπει τον πίνακα παραμέτρων του συστήματος
        /// </summary>
        EnumerateSystemParameters           = 128L,                 // 1 << 7
        /// <summary>
        /// Μπορεί να δημιουργεί ,διαγράφει και να κάνει update εγγραφές απο τις παραμέτρους του συστήματος
        /// </summary>
        ManageSystemParameters              = 256L,                 // 1 << 8

        /// <summary>
        /// Μπορεί να βλέπει Survey Templates, Predefined Questions, .....
        /// </summary>
        EnumerateBuildingBlocks             = 512L,                 // 1 << 9
        /// <summary>
        /// Μπορεί να διαχειρίζεται Survey Templates, Predefined Questions, .....
        /// </summary>
        ManageBuidingBlocks                 = 1024L,                // 1 << 10

        /// <summary>
        /// Μπορεί να βλέπει τα Survey Themes
        /// </summary>
        EnumerateThemes                     = 2048L,                // 1 << 11
        /// <summary>
        /// Μπορεί να διαχειρίζεται τα Survey Themes
        /// </summary>
        ManageThemes                        = 4096L,                // 1 << 12

        /// <summary>
        /// Μπορεί να απαριθμεί τους Survey Renderes του συστήματος
        /// </summary>
        EnumerateRenders                    = 8192L,                // 1 << 13
        /// <summary>
        /// Μπορεί να διαχειρίζεται τους Survey Renderes του συστήματος
        /// </summary>
        ManageRenders                       = 16384L,               // 1 << 14

        /// <summary>
        /// Μπορεί να απαριθμεί τους Πελάτες της υπηρεσίας
        /// </summary>
        EnumerateClients                    = 32768L,               // 1 << 15
        /// <summary>
        /// Μπορεί να διαχειρίζεται τους Πελάτες του συστήματος
        /// </summary>
        ManageClients                        = 65536L,               // 1 << 16


        /// <summary>
        /// Μπορεί να απαριθμεί τις Πληρωμές των πελατών της υπηρεσία
        /// </summary>
        EnumeratePayments                   = 131072L,               // 1 << 17
        /// <summary>
        /// Μπορεί να διαχειρίζεται τις Πληρωμές των Πελατών του συστήματος
        /// </summary>
        ManagePayments                      = 262144L,               // 1 << 18



/********PERMISSIONS FOR CLIENT-USERS**************************************************************************************************/
/********PERMISSIONS FOR CLIENT-USERS**************************************************************************************************/

        /// <summary>
        /// Δίνει στον πελάτη/χρήστη της υπηρεσίας πλήρη έλεγχο μέσα στο δικό του sandbox
        /// </summary>
        ClientFullControl                   = 4194304L,             // 1 << 22
        /// <summary>
        /// Δίνει στον πεάτη/χρήστη της υπηρεσίας απεριόριστους πόρους
        /// </summary>
        ClientUnlimitedQuota                = 8388608L,             // 1 << 23

        /// <summary>
        /// This permission allows the enumeration of a client's users
        /// </summary>
        ClientEnumerateUsers                = 16777216L,            // 1 << 24
        /// <summary>
        /// This permission allows the manipulations of a client's users
        /// </summary>
        ClientManageUsers                   = 33554432L,            // 1 << 25

        /// <summary>
        /// This permissions allows the enumeration of a client's lists (address book)
        /// </summary>
        ClientEnumerateLists                = 67108864L,            // 1 << 26
        /// <summary>
        /// This permission allows the manipulation of a client's lists (address book)
        /// </summary>
        ClientManageLists                   = 134217728L,           // 1 << 27
        /// <summary>
        /// This permission allows a client's user to import a list of contacts (address book)
        /// </summary>
        ClientImportLists                   = 268435456L,           // 1 << 28


        /// <summary>
        /// 
        /// </summary>
        ClientEnumerateSurveys              = 536870912L,           // 1 << 29
        /// <summary>
        /// 
        /// </summary>
        ClientPreviewSurveys                = 1073741824L,          // 1 << 30
        /// <summary>
        /// 
        /// </summary>
        ClientCreateSurveys                 = 2147483648L,          // 1 << 31
        /// <summary>
        /// 
        /// </summary>
        ClientEditSurveys                   = 4294967296L,          // 1 << 32
        /// <summary>
        /// 
        /// </summary>
        ClientDeleteSurveys                 = 8589934592L,          // 1 << 33
        /// <summary>
        /// 
        /// </summary>
        ClientRunSurveys                    = 17179869184L,         // 1 << 34


        /// <summary>
        /// 
        /// </summary>
        ClientEnumerateCollectors           = 34359738368L,         // 1 << 35
        /// <summary>
        /// 
        /// </summary>
        ClientManageCollectors              = 68719476736L,         // 1 << 36

        /// <summary>
        /// 
        /// </summary>
        ClientEnumerateAnswers              = 137438953472L,        // 1 << 37
        /// <summary>
        /// 
        /// </summary>
        ClientManageAnswers                 = 274877906944L,         // 1 << 38

        /// <summary>
        /// 
        /// </summary>
        ClientEnumeratePayments             = 549755813888L,         // 1 << 39
        /// <summary>
        /// 
        /// </summary>
        ClientManagePayments                = 1099511627776L         // 1 << 40


    }
}
