using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Αντιπροσωπεύει ένα γνωστό email address στο σύστημά μας.
    /// <para>Περιέχει επιβεβαιωμένα emails, opted-out emails και bounced emails</para>
    /// </summary>
    [DataContract, DataObject]
    [Serializable]
    public class VLKnownEmail
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_emailId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_emailAddress;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_localPart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_domainPart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_registerDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isDomainOK;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isVerified;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isOptedOut;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isBounced;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_verifiedDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_optedOutDt;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32? Client
        {
            get { return m_client; }
            internal set { m_client = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 EmailId
        {
            get { return m_emailId; }
            internal set { m_emailId = value; }
        }
        /// <summary>
        /// Αυτή είναι η Email Address της εγγραφής μας.
        /// <para>Αυτό το πεδίο δεν μπορεί να αλλάξει μετά την δημιουργία της εγγραφής.</para>
        /// </summary>
        public String EmailAddress
        {
            get { return m_emailAddress; }
            internal set { m_emailAddress = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String LocalPart
        {
            get { return m_localPart; }
            internal set { m_localPart = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String DomainPart
        {
            get { return m_domainPart; }
            internal set { m_domainPart = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RegisterDt
        {
            get { return m_registerDt; }
            internal set { m_registerDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsDomainOK
        {
            get { return m_isDomainOK; }
            internal set { m_isDomainOK = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsVerified
        {
            get { return m_isVerified; }
            internal set { m_isVerified = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsOptedOut
        {
            get { return m_isOptedOut; }
            internal set { m_isOptedOut = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsBounced
        {
            get { return m_isBounced; }
            internal set { m_isBounced = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? VerifiedDt
        {
            get { return m_verifiedDt; }
            internal set { m_verifiedDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? OptedOutDt
        {
            get { return m_optedOutDt; }
            internal set { m_optedOutDt = value; }
        }
        #endregion

        #region class constructors
        public VLKnownEmail()
        {
            m_client = null;
            m_emailId = default(Int32);
            m_emailAddress = default(string);
            m_localPart = default(string);
            m_domainPart = default(string);
            m_registerDt = default(DateTime);
            m_isDomainOK = default(Boolean);
            m_isVerified = default(Boolean);
            m_isOptedOut = default(Boolean);
            m_isBounced = default(Boolean);
            m_verifiedDt = null;
            m_optedOutDt = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLKnownEmail(DbDataReader reader)
        {
            if (!reader.IsDBNull(0)) this.Client = reader.GetInt32(0);
            this.EmailId = reader.GetInt32(1);
            this.EmailAddress = reader.GetString(2);
            this.LocalPart = reader.GetString(3);
            this.DomainPart = reader.GetString(4);
            this.RegisterDt = reader.GetDateTime(5);
            this.IsDomainOK = reader.GetBoolean(6);
            this.IsVerified = reader.GetBoolean(7);
            this.IsOptedOut = reader.GetBoolean(8);
            this.IsBounced = reader.GetBoolean(9);
            if (!reader.IsDBNull(10)) this.VerifiedDt = reader.GetDateTime(10);
            if (!reader.IsDBNull(11)) this.OptedOutDt = reader.GetDateTime(11);
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return ((this.Client == null) ? string.Empty : this.Client.ToString()).GetHashCode() ^
                this.EmailId.GetHashCode() ^
                this.EmailAddress.GetHashCode() ^
                this.LocalPart.GetHashCode() ^
                this.DomainPart.GetHashCode() ^
                this.RegisterDt.GetHashCode() ^
                this.IsDomainOK.GetHashCode() ^
                this.IsVerified.GetHashCode() ^
                this.IsOptedOut.GetHashCode() ^
                this.IsBounced.GetHashCode() ^
                ((this.VerifiedDt == null) ? string.Empty : this.VerifiedDt.ToString()).GetHashCode() ^
                ((this.OptedOutDt == null) ? string.Empty : this.OptedOutDt.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLKnownEmail)obj;

            //reference types
            if (!Object.Equals(m_emailAddress, other.m_emailAddress)) return false;
            if (!Object.Equals(m_localPart, other.m_localPart)) return false;
            if (!Object.Equals(m_domainPart, other.m_domainPart)) return false;
            //value types
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_emailId.Equals(other.m_emailId)) return false;
            if (!m_registerDt.Equals(other.m_registerDt)) return false;
            if (!m_isDomainOK.Equals(other.m_isDomainOK)) return false;
            if (!m_isVerified.Equals(other.m_isVerified)) return false;
            if (!m_isOptedOut.Equals(other.m_isOptedOut)) return false;
            if (!m_isBounced.Equals(other.m_isBounced)) return false;
            if (!m_verifiedDt.Equals(other.m_verifiedDt)) return false;
            if (!m_optedOutDt.Equals(other.m_optedOutDt)) return false;

            return true;
        }
        public static Boolean operator ==(VLKnownEmail o1, VLKnownEmail o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLKnownEmail o1, VLKnownEmail o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_emailAddress, true, true, true, 384, "EmailAddress");
            Utility.CheckParameter(ref m_localPart, true, true, true, 128, "LocalPart");
            Utility.CheckParameter(ref m_domainPart, true, true, true, 256, "DomainPart");
        }
    }
}
