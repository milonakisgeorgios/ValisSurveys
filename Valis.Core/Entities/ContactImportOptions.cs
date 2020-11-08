using System;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ContactImportOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HasHeaderRecord { get; set; }
        /// <summary>
        /// Ο χαρακτήρας με τον οποίο χωρόζονται τα πεδία μέσα σε μία γραμμή του αρχείου
        /// </summary>
        public string DelimiterCharacter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool TrimFields { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ContinueOnError { get; set; }




        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το Email
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα).
        /// </summary>
        public Byte EmailOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το FirstName
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte FirstNameOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το LastName
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte LastNameOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το Title
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte TitleOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το Organization
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte OrganizationOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το Department
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte DepartmentOrdinal { get; set; }

        /// <summary>
        /// Η αυξουσα σειρά στην οποία βρίσκεται το πεδίο που φέρει το Comment
        /// Η αρίθμηση ξεκινά απο τον αριθμό 1 (ένα). τιμή 0 (μηδέν) σημαίνει ότι δεν θα διαβαστεί αυτό το πεδίο απο το αρχείο.
        /// </summary>
        public Byte CommentOrdinal { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public ContactImportOptions()
        {
            this.HasHeaderRecord = true;
            this.DelimiterCharacter = ",";
            this.TrimFields = true;
            this.ContinueOnError = true;
            this.EmailOrdinal = 1;
        }


        public static readonly ContactImportOptions Default = new ContactImportOptions { HasHeaderRecord = false, TrimFields = true, ContinueOnError = true, DelimiterCharacter = ",", EmailOrdinal = 1, FirstNameOrdinal = 2, LastNameOrdinal = 3, TitleOrdinal = 4 };

        internal static readonly ContactImportOptions UnitTest = new ContactImportOptions { HasHeaderRecord = true, ContinueOnError = true, EmailOrdinal = 2, FirstNameOrdinal = 3, LastNameOrdinal = 4 };

    }
}
