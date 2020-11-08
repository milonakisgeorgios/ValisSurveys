using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    public class VLResponse
    {
        [Flags]
        internal enum ResponseAttributes : int
        {
            None = 0,

            IsDisqualified      = 4,            //1 << 2
            IsViewResponse      = 16,          // 1 << 4
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64 m_responseId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ResponseType m_responseType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64? m_recipient;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_recipientIP;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_userAgent;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_openDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_closeDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_mustBeCharged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CreditType m_creditType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_collectorPayment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isCharged;

        Collection<VLResponseDetail> m_details;
        #endregion


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int64 ResponseId
        {
            get { return m_responseId; }
            internal set { m_responseId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Survey
        {
            get { return m_survey; }
            internal set { m_survey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? Collector
        {
            get { return m_collector; }
            set { m_collector = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ResponseType ResponseType
        {
            get { return m_responseType; }
            set { m_responseType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int64? Recipient
        {
            get { return m_recipient; }
            set { m_recipient = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String RecipientIP
        {
            get { return m_recipientIP; }
            set { m_recipientIP = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? UserAgent
        {
            get { return m_userAgent; }
            set { m_userAgent = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OpenDate
        {
            get { return m_openDate; }
            set { m_openDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CloseDate
        {
            get { return m_closeDate; }
            set { m_closeDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }

        /// <summary>
        /// Μας λέει εάν το συγκεκριμένο response προέρχεται απο έναν Disqualified recipient.
        /// </summary>
        public System.Boolean IsDisqualified
        {
            get { return (this.m_attributeFlags & ((int)ResponseAttributes.IsDisqualified)) == ((int)ResponseAttributes.IsDisqualified); }
            internal set
            {
                if (this.IsDisqualified == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ResponseAttributes.IsDisqualified;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ResponseAttributes.IsDisqualified;
            }
        }

        /// <summary>
        /// Μας λέει εάν αυτό το Response διαβάστηκα απο το σύστημα, μέσω της ομάδας μεθόδων 'GetViewResponses'.
        /// <para>Στην πράξη αυτό σημαίνει ότι το collection ΡesponseDetails φέρει εγγραφές.</para>
        /// </summary>
        public System.Boolean IsViewResponse
        {
            get { return (this.m_attributeFlags & ((int)ResponseAttributes.IsViewResponse)) == ((int)ResponseAttributes.IsViewResponse); }
            internal set
            {
                if (this.IsViewResponse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ResponseAttributes.IsViewResponse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ResponseAttributes.IsViewResponse;
            }
        }


        /// <summary>
        /// Χρησιμοποιείται για να σημαδέψει ένα Response ότι απαιτεί πληρωμή, για να γίνει ορατό απο τον Πελάτη.
        /// <para>Χρησιμοποιείται απο το κύκλωμα των πληρωμών</para>
        /// </summary>
        internal Boolean MustBeCharged
        {
            get { return m_mustBeCharged; }
            set { m_mustBeCharged = value; }
        }
        /// <summary>
        /// Το είδος του resource (none, email, response, click) που χρεώθηκε για αυτή την απάντηση.
        /// <para>Εχει πάντα σωστή τιμή, η οποία αντιγράφεται απο τον Collector απο τον οποίο προέκυψε αυτό το response</para>
        /// <para>Χρησιμοποιείται απο το κύκλωμα των πληρωμών</para>
        /// </summary>
        public CreditType CreditType
        {
            get { return m_creditType; }
            internal set { m_creditType = value; }
        }
        /// <summary>
        /// Απο ποιό collectorPayment έγινε η χρέωση
        /// <para>Χρησιμοποιείται απο το κύκλωμα των πληρωμών</para>
        /// </summary>
        public Int32? CollectorPayment
        {
            get { return m_collectorPayment; }
            internal set { m_collectorPayment = value; }
        }
        /// <summary>
        /// Μας λέει εάν ο Πελάτης έχει χρεωθεί για αυτήν την απάντηση, οπότε είναι ορατή για αυτόν.
        /// <para></para>
        /// <para>Χρησιμοποιείται απο το κύκλωμα των πληρωμών</para>
        /// </summary>
        public Boolean IsCharged
        {
            get { return m_isCharged; }
            internal set { m_isCharged = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Collection<VLResponseDetail> ResponseDetails
        {
            get { return m_details; }
            set { m_details = value; }
        }
        #endregion


        #region class constructors
        public VLResponse()
        {
            m_responseId = default(Int64);
            m_survey = default(Int32);
            m_collector = null;
            m_responseType = default(Byte);
            m_recipient = null;
            m_recipientIP = default(string);
            m_userAgent = null;
            m_openDate = default(DateTime);
            m_closeDate = null;
            m_attributeFlags = default(Int32);
            m_mustBeCharged = default(Boolean);
            m_collectorPayment = null;
            m_isCharged = default(Boolean);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLResponse(DbDataReader reader)
        {
            this.ResponseId = reader.GetInt64(0);
            this.Survey = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.Collector = reader.GetInt32(2);
            this.ResponseType = (ResponseType)reader.GetByte(3);
            if (!reader.IsDBNull(4)) this.Recipient = reader.GetInt64(4);
            if (!reader.IsDBNull(5)) this.RecipientIP = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.UserAgent = reader.GetInt32(6);
            this.OpenDate = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) this.CloseDate = reader.GetDateTime(8);
            this.AttributeFlags = reader.GetInt32(9);
            this.MustBeCharged = reader.GetBoolean(10);
            this.CreditType = (CreditType)reader.GetByte(11);
            if (!reader.IsDBNull(12)) this.CollectorPayment = reader.GetInt32(12);
            this.IsCharged = reader.GetBoolean(13);
        }

        #endregion


        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ResponseId.GetHashCode() ^
                this.Survey.GetHashCode() ^
                ((this.Collector == null) ? string.Empty : this.Collector.ToString()).GetHashCode() ^
                this.ResponseType.GetHashCode() ^
                ((this.Recipient == null) ? string.Empty : this.Recipient.ToString()).GetHashCode() ^
                ((this.RecipientIP == null) ? string.Empty : this.RecipientIP.ToString()).GetHashCode() ^
                ((this.UserAgent == null) ? string.Empty : this.UserAgent.ToString()).GetHashCode() ^
                this.OpenDate.GetHashCode() ^
                ((this.CloseDate == null) ? string.Empty : this.CloseDate.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.MustBeCharged.GetHashCode() ^
                this.CreditType.GetHashCode() ^
                ((this.CollectorPayment == null) ? string.Empty : this.CollectorPayment.ToString()).GetHashCode() ^
                this.IsCharged.GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLResponse)obj;

            //reference types
            if (!Object.Equals(RecipientIP, other.RecipientIP)) return false;
            //value types
            if (!m_responseId.Equals(other.m_responseId)) return false;
            if (!m_survey.Equals(other.m_survey)) return false;
            if (!m_collector.Equals(other.m_collector)) return false;
            if (!m_responseType.Equals(other.m_responseType)) return false;
            if (!m_recipient.Equals(other.m_recipient)) return false;
            if (!m_userAgent.Equals(other.m_userAgent)) return false;
            if (!m_openDate.Equals(other.m_openDate)) return false;
            if (!m_closeDate.Equals(other.m_closeDate)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_mustBeCharged.Equals(other.m_mustBeCharged)) return false;
            if (!m_creditType.Equals(other.m_creditType)) return false;
            if (!m_collectorPayment.Equals(other.m_collectorPayment)) return false;
            if (!m_isCharged.Equals(other.m_isCharged)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLResponse o1, VLResponse o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLResponse o1, VLResponse o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_recipientIP, false, false, false, 64, "RecipientIP");
        }
    }
}
