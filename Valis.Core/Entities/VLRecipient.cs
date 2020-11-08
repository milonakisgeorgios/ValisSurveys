using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    public sealed class VLRecipient : VLObject
    {
        [Flags]
        internal enum RecipientAttributes : int
        {
            None                    = 0,
            IsSentEmail             = 1,            // 1 << 0
            IsOptedOut              = 2,            // 1 << 1
            IsBouncedEmail          = 4,            // 1 << 2
            HasResponded            = 8,            // 1 << 3
            HasPartiallyResponded   = 16,           // 1 << 4
            IsWebLinkRecipient      = 32,           // 1 << 5
            HasManuallyAdded        = 64,           // 1 << 6
            HasImportMark           = 512           // 1 << 9
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64 m_recipientId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_recipientKey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_email;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_firstName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_lastName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_title;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_customValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RecipientStatus m_status;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_personalPassword;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_activationDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_validFromDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_validToDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_expireAfter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_expirationDate;
        #endregion


        #region EntityState
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EntityState _currentEntityState = EntityState.Added;

        /// <summary>
        ///	Indicates state of object
        /// </summary>
        /// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public override EntityState EntityState
        {
            get { return _currentEntityState; }
            internal set { _currentEntityState = value; }
        }
        #endregion
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _deserializing = false;



        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Int64 RecipientId
        {
            get { return this.m_recipientId; }
            internal set
            {
                if (this.m_recipientId == value)
                    return;

                this.m_recipientId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Collector
        {
            get { return this.m_collector; }
            internal set
            {
                if (this.m_collector == value)
                    return;

                this.m_collector = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Every recipients has a unique key of (possibly) variable length not less than 32 characters.
        /// <para>This RecipientKey is used for the construction of various web links related to this Recipient.</para>
        /// </summary>
        public System.String RecipientKey
        {
            get { return this.m_recipientKey; }
            internal set
            {
                if (this.m_recipientKey == value)
                    return;

                this.m_recipientKey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Email
        {
            get { return this.m_email; }
            set
            {
                if (this.m_email == value)
                    return;

                this.m_email = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String FirstName
        {
            get { return this.m_firstName; }
            set
            {
                if (this.m_firstName == value)
                    return;

                this.m_firstName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String LastName
        {
            get { return this.m_lastName; }
            set
            {
                if (this.m_lastName == value)
                    return;

                this.m_lastName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Title
        {
            get { return this.m_title; }
            set
            {
                if (this.m_title == value)
                    return;

                this.m_title = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String CustomValue
        {
            get { return this.m_customValue; }
            set
            {
                if (this.m_customValue == value)
                    return;

                this.m_customValue = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RecipientStatus Status
        {
            get { return this.m_status; }
            set
            {
                if (this.m_status == value)
                    return;

                this.m_status = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal System.Int32 AttributeFlags
        {
            get { return this.m_attributeFlags; }
            set
            {
                if (this.m_attributeFlags == value)
                    return;

                this.m_attributeFlags = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Does the system have sent any email to the Recipient?
        /// </summary>
        public System.Boolean IsSentEmail
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.IsSentEmail)) == ((int)RecipientAttributes.IsSentEmail); }
            internal set
            {
                if (this.IsSentEmail == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.IsSentEmail;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.IsSentEmail;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// The recipient have declined to receive further mailings
        /// </summary>
        public System.Boolean IsOptedOut
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.IsOptedOut)) == ((int)RecipientAttributes.IsOptedOut); }
            internal set
            {
                if (this.IsOptedOut == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.IsOptedOut;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.IsOptedOut;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// A delivery error has occured, and the messages cannot be delivered to the recipient's email.
        /// </summary>
        public System.Boolean IsBouncedEmail
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.IsBouncedEmail)) == ((int)RecipientAttributes.IsBouncedEmail); }
            internal set
            {
                if (this.IsBouncedEmail == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.IsBouncedEmail;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.IsBouncedEmail;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Εάν ο recipient απάντησε στο ερωτηματολόγιο πετυχημένα (έφτασε και πάτησε και το submit)
        /// </summary>
        public System.Boolean HasResponded
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.HasResponded)) == ((int)RecipientAttributes.HasResponded); }
            internal set
            {
                if (this.HasResponded == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.HasResponded;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.HasResponded;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Εάν ο recipient έχει ξεκινήσει το ερωτηματολόγιο αλλά δεν έχει τελειώσε ακόμα (δεν έχει πατήσει ακόμα το submit κουμπί)
        /// </summary>
        public System.Boolean HasPartiallyResponded
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.HasPartiallyResponded)) == ((int)RecipientAttributes.HasPartiallyResponded); }
            internal set
            {
                if (this.HasPartiallyResponded == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.HasPartiallyResponded;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.HasPartiallyResponded;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Μας λέει ότι αυτός ο Recipient δημιουργήθηκε απο το σύστημα εικονικά για να αντιστοιχηθεί με έναν WebLink (αγνωστο/ανώνυμο) recipient.
        /// </summary>
        public System.Boolean IsWebLinkRecipient
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.IsWebLinkRecipient)) == ((int)RecipientAttributes.IsWebLinkRecipient); }
            internal set
            {
                if (this.IsWebLinkRecipient == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.IsWebLinkRecipient;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.IsWebLinkRecipient;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }


        /// <summary>
        /// Μας λέει ότι το response για αυτόν τον recipient μπήκε με manual τρόπο.
        /// </summary>
        public System.Boolean HasManuallyAdded
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.HasManuallyAdded)) == ((int)RecipientAttributes.HasManuallyAdded); }
            internal set
            {
                if (this.HasManuallyAdded == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.HasManuallyAdded;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.HasManuallyAdded;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Χρησιμοποιείται κατα την διαδικασία του ImportRecipientsFromCSV, έτσι ώστε να σημαδευτούν τα νέα Recipients, για να μπορέσει να τα βρεί
        /// η SurveysDal.ImportRecipientsFinalize, και να τερματίσει την διαδικασία του ImportRecipientsFromCSV!
        /// </summary>
        internal System.Boolean HasImportMark
        {
            get { return (this.m_attributeFlags & ((int)RecipientAttributes.HasImportMark)) == ((int)RecipientAttributes.HasImportMark); }
            set
            {
                if (this.HasImportMark == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)RecipientAttributes.HasImportMark;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)RecipientAttributes.HasImportMark;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PersonalPassword
        {
            get { return this.m_personalPassword; }
            set
            {
                if (this.m_personalPassword == value)
                    return;

                this.m_personalPassword = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Ημερα και ώρα κατα την οποία είδε την welcome page ή την πρώτη page του survey.
        /// <para>H ενεργοποίηση γίνεται αφού φορτώσει η σελίδα του survey με επιτυχία στον browser του 
        /// recipient, απο τον ίδιο τον browser, μετά απο 5 δευτερόλεπτα!!</para>
        /// </summary>
        public System.DateTime? ActivationDate
        {
            get { return this.m_activationDate; }
            set
            {
                if (this.m_activationDate == value)
                    return;

                this.m_activationDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? ValidFromDate
        {
            get { return this.m_validFromDate; }
            set
            {
                if (this.m_validFromDate == value)
                    return;

                this.m_validFromDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? ValidToDate
        {
            get { return this.m_validToDate; }
            set
            {
                if (this.m_validToDate == value)
                    return;

                this.m_validToDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 ExpireAfter
        {
            get { return this.m_expireAfter; }
            set
            {
                if (this.m_expireAfter == value)
                    return;

                this.m_expireAfter = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? ExpirationDate
        {
            get { return this.m_expirationDate; }
            set
            {
                if (this.m_expirationDate == value)
                    return;

                this.m_expirationDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion
        

        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLRecipient()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLRecipient(DbDataReader reader)
        {
            this.RecipientId = reader.GetInt64(0);
            this.Collector = reader.GetInt32(1);
            this.RecipientKey = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.Email = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.FirstName = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.LastName = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.Title = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.CustomValue = reader.GetString(7);
            this.Status = (RecipientStatus)reader.GetByte(8);
            this.AttributeFlags = reader.GetInt32(9);
            if (!reader.IsDBNull(10)) this.PersonalPassword = reader.GetString(10);
            if (!reader.IsDBNull(11)) this.ActivationDate = reader.GetDateTime(11);
            if (!reader.IsDBNull(12)) this.ValidFromDate = reader.GetDateTime(12);
            if (!reader.IsDBNull(13)) this.ValidToDate = reader.GetDateTime(13);
            this.ExpireAfter = reader.GetInt16(14);
            if (!reader.IsDBNull(15)) this.ExpirationDate = reader.GetDateTime(15);

            this.LastUpdateDT = this.CreateDT = reader.GetDateTime(16);
            this.LastUpdateByPrincipal = this.CreateByPrincipal = reader.GetInt32(17);

            this.EntityState = EntityState.Unchanged;
        }
        /// <summary>
        /// 
        /// </summary>
        internal void InitializeInstance(Int32 collector, string email, string recipientKey)
        {
            m_recipientId = default(Int64);
            m_collector = collector;
            m_recipientKey = recipientKey;
            m_email = email;
            m_firstName = default(string);
            m_lastName = default(string);
            m_title = default(string);
            m_customValue = default(string);
            m_status = default(RecipientStatus);
            m_attributeFlags = default(Int32);
            m_personalPassword = default(string);
            m_activationDate = null;
            m_validFromDate = null;
            m_validToDate = null;
            m_expireAfter = default(Int16);
            m_expirationDate = null;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.RecipientId.GetHashCode() ^
                this.Collector.GetHashCode() ^
                this.RecipientKey.GetHashCode() ^
                this.Email.GetHashCode() ^
                ((this.FirstName == null) ? string.Empty : this.FirstName.ToString()).GetHashCode() ^
                ((this.LastName == null) ? string.Empty : this.LastName.ToString()).GetHashCode() ^
                ((this.Title == null) ? string.Empty : this.Title.ToString()).GetHashCode() ^
                ((this.CustomValue == null) ? string.Empty : this.CustomValue.ToString()).GetHashCode() ^
                this.Status.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.PersonalPassword == null) ? string.Empty : this.PersonalPassword.ToString()).GetHashCode() ^
                ((this.ActivationDate == null) ? string.Empty : this.ActivationDate.ToString()).GetHashCode() ^
                ((this.ValidFromDate == null) ? string.Empty : this.ValidFromDate.ToString()).GetHashCode() ^
                ((this.ValidToDate == null) ? string.Empty : this.ValidToDate.ToString()).GetHashCode() ^
                this.ExpireAfter.GetHashCode() ^
                ((this.ExpirationDate == null) ? string.Empty : this.ExpirationDate.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLRecipient)obj;

            //reference types
            if (!Object.Equals(m_recipientKey, other.m_recipientKey)) return false;
            if (!Object.Equals(m_email, other.m_email)) return false;
            if (!Object.Equals(m_firstName, other.m_firstName)) return false;
            if (!Object.Equals(m_lastName, other.m_lastName)) return false;
            if (!Object.Equals(m_title, other.m_title)) return false;
            if (!Object.Equals(m_customValue, other.m_customValue)) return false;
            if (!Object.Equals(m_personalPassword, other.m_personalPassword)) return false;
            //value types
            if (!m_recipientId.Equals(other.m_recipientId)) return false;
            if (!m_collector.Equals(other.m_collector)) return false;
            if (!m_status.Equals(other.m_status)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_activationDate.Equals(other.m_activationDate)) return false;
            if (!m_validFromDate.Equals(other.m_validFromDate)) return false;
            if (!m_validToDate.Equals(other.m_validToDate)) return false;
            if (!m_expireAfter.Equals(other.m_expireAfter)) return false;
            if (!m_expirationDate.Equals(other.m_expirationDate)) return false;

            return true;
        }
        public static Boolean operator ==(VLRecipient o1, VLRecipient o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLRecipient o1, VLRecipient o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_recipientKey, false, true, false, 128, "RecipientKey");

            if(this.IsWebLinkRecipient == false)
                Utility.CheckParameter(ref m_email, true, true, false, 256, "Email");
            else
                Utility.CheckParameter(ref m_email, false, true, false, 256, "Email");

            Utility.CheckParameter(ref m_firstName, false, false, false, 128, "FirstName");
            Utility.CheckParameter(ref m_lastName, false, false, false, 128, "LastName");
            Utility.CheckParameter(ref m_title, false, false, false, 256, "Title");
            Utility.CheckParameter(ref m_customValue, false, false, false, 128, "CustomValue");
            Utility.CheckParameter(ref m_personalPassword, false, false, false, 128, "PersonalPassword");
        }


        internal static void ValidateEmail(ref string email)
        {
            Utility.CheckParameter(ref email, true, true, false, 256, "Email");
        }


        public override string ToString()
        {
            return this.Email;
        }
    }
}
