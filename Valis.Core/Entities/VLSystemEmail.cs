using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    /// <summary>
    /// Αντιπροσωπεύει ένα email που δημιουργείται απο το ίδιο το σύστημα.
    /// <para>Πολλές λειτουργίες του συστήματος, απαιτούν την αποστολή emails είτε για alerting ή για επιβεβαίωση διαφόρων στοιχείων.
    /// Αυτά τα email (και γενικότερα το κύκλωμα των SystemEmails), δεν έχουν καμμία σχέση με τα emails που δημιουργούν οι Collectors
    /// τύπου email (αυτά τα emails, απασχολούν άλλο κύκλωμα και χρεώνονται).</para>
    /// <para>Τα SystemEmails, αμέσως μετά την δημιουργία τους, το σύστημα προσπαθεί να τα στείλει άμεσα!</para>
    /// <para>Ενα SystemEmail είναι επεξεργασμένο έτοιμο για 'τυφλή' αποστολή!</para>
    /// </summary>
    public sealed class VLSystemEmail
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_emailId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_accessToken;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_moduleName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_enterDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_fromAddress;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_fromDisplayName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_toAddress;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_subject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_body;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EmailStatus m_status;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_sendDT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_error;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 EmailId
        {
            get { return m_emailId; }
            internal set { m_emailId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AccessToken
        {
            get { return m_accessToken; }
            internal set { m_accessToken = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String ModuleName
        {
            get { return m_moduleName; }
            internal set { m_moduleName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EnterDt
        {
            get { return m_enterDt; }
            internal set { m_enterDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int16 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String FromAddress
        {
            get { return m_fromAddress; }
            internal set { m_fromAddress = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String FromDisplayName
        {
            get { return m_fromDisplayName; }
            internal set { m_fromDisplayName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String ToAddress
        {
            get { return m_toAddress; }
            internal set { m_toAddress = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Subject
        {
            get { return m_subject; }
            internal set { m_subject = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Body
        {
            get { return m_body; }
            internal set { m_body = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public EmailStatus Status
        {
            get { return m_status; }
            internal set { m_status = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? SendDT
        {
            get { return m_sendDT; }
            internal set { m_sendDT = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Error
        {
            get { return m_error; }
            internal set { m_error = value; }
        }
        #endregion


        #region class constructors
        public VLSystemEmail()
        {
            m_emailId = default(Int32);
            m_accessToken = default(Int32);
            m_moduleName = default(string);
            m_enterDt = default(DateTime);
            m_attributeFlags = default(Int16);
            m_fromAddress = default(string);
            m_fromDisplayName = default(string);
            m_toAddress = default(string);
            m_subject = default(string);
            m_body = default(string);
            m_status = default(EmailStatus);
            m_sendDT = null;
            m_error = default(string);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSystemEmail(DbDataReader reader)
        {
            this.EmailId = reader.GetInt32(0);
            this.AccessToken = reader.GetInt32(1);
            this.ModuleName = reader.GetString(2);
            this.EnterDt = reader.GetDateTime(3);
            this.AttributeFlags = reader.GetInt16(4);
            this.FromAddress = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.FromDisplayName = reader.GetString(6);
            this.ToAddress = reader.GetString(7);
            if (!reader.IsDBNull(8)) this.Subject = reader.GetString(8);
            if (!reader.IsDBNull(9)) this.Body = reader.GetString(9);
            this.Status = (EmailStatus)reader.GetByte(10);
            if (!reader.IsDBNull(11)) this.SendDT = reader.GetDateTime(11);
            if (!reader.IsDBNull(12)) this.Error = reader.GetString(12);

        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.EmailId.GetHashCode() ^
                this.AccessToken.GetHashCode() ^
                this.ModuleName.GetHashCode() ^
                this.EnterDt.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.FromAddress.GetHashCode() ^
                ((this.FromDisplayName == null) ? string.Empty : this.FromDisplayName.ToString()).GetHashCode() ^
                this.ToAddress.GetHashCode() ^
                ((this.Subject == null) ? string.Empty : this.Subject.ToString()).GetHashCode() ^
                ((this.Body == null) ? string.Empty : this.Body.ToString()).GetHashCode() ^
                this.Status.GetHashCode() ^
                ((this.SendDT == null) ? string.Empty : this.SendDT.ToString()).GetHashCode() ^
                ((this.Error == null) ? string.Empty : this.Error.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLSystemEmail)obj;

            //reference types
            if (!Object.Equals(m_moduleName, other.m_moduleName)) return false;
            if (!Object.Equals(m_fromAddress, other.m_fromAddress)) return false;
            if (!Object.Equals(m_fromDisplayName, other.m_fromDisplayName)) return false;
            if (!Object.Equals(m_toAddress, other.m_toAddress)) return false;
            if (!Object.Equals(m_subject, other.m_subject)) return false;
            if (!Object.Equals(m_body, other.m_body)) return false;
            if (!Object.Equals(m_error, other.m_error)) return false;
            //value types
            if (!m_emailId.Equals(other.m_emailId)) return false;
            if (!m_accessToken.Equals(other.m_accessToken)) return false;
            if (!m_enterDt.Equals(other.m_enterDt)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_status.Equals(other.m_status)) return false;
            if (!m_sendDT.Equals(other.m_sendDT)) return false;

            return true;
        }
        public static Boolean operator ==(VLSystemEmail o1, VLSystemEmail o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLSystemEmail o1, VLSystemEmail o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            ValidateModuleName(ref m_moduleName);
            ValidateFromAddress(ref m_fromAddress);
            ValidateFromDisplayName(ref m_fromDisplayName);
            ValidateToAddress(ref m_toAddress);
            ValidateSubject(ref m_subject);

            Utility.CheckParameter(ref m_body, false, false, false, -1, "Body");
            Utility.CheckParameter(ref m_error, false, false, false, -1, "Error");
        }

        internal static void ValidateModuleName(ref string moduleName)
        {
            Utility.CheckParameter(ref moduleName, true, true, true, 128, "ModuleName");
        }
        internal static void ValidateFromAddress(ref string fromAddress)
        {
            Utility.CheckParameter(ref fromAddress, true, true, true, 512, "FromAddress");
        }
        internal static void ValidateFromDisplayName(ref string fromDisplayName)
        {
            Utility.CheckParameter(ref fromDisplayName, false, false, false, 512, "FromDisplayName");
        }
        internal static void ValidateToAddress(ref string toAddress)
        {
            Utility.CheckParameter(ref toAddress, true, true, true, 512, "ToAddress");
        }
        internal static void ValidateSubject(ref string subject)
        {
            Utility.CheckParameter(ref subject, false, false, false, 1024, "Subject");
        }
    }
}
