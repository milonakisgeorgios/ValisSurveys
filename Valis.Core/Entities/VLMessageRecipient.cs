using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    public sealed class VLMessageRecipient
    {
        [Flags]
        internal enum MessageRecipientAttributes : int
        {
            None = 0,
            AttributeXxx1 = 1,            // 1 << 0
            AttributeXxx2 = 2,            // 1 << 1
            AttributeXxx3 = 4,            // 1 << 2
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_message;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64 m_recipient;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_errorCount;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_sendDT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        MessageRecipientStatus m_status;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_collectorPayment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isCharged;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_error;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 Message
        {
            get { return m_message; }
            internal set { m_message = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int64 Recipient
        {
            get { return m_recipient; }
            internal set { m_recipient = value; }
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
        public Int16 ErrorCount
        {
            get { return m_errorCount; }
            internal set { m_errorCount = value; }
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
        public MessageRecipientStatus Status
        {
            get { return m_status; }
            internal set { m_status = value; }
        }
        /// <summary>
        /// Εάν ο Πελάτης χρεώνεται, και εάν η πληρωμή που επέλεξε είναι τύπου 'email', εδώ είναι το collectorPayment
        /// στο οποίο έγινε η χρέωαη για αυτή την αποστολή email.
        /// </summary>
        public Int32? CollectorPayment
        {
            get { return m_collectorPayment; }
            internal set { m_collectorPayment = value; }
        }
        /// <summary>
        /// Εάν ο Πελάτης χρεώνεται, και εάν η πληρωμή που επέλεξε είναι τύπου 'email', εδώ είναι το σύνολο των
        /// μονάων που χρεώθηκε αυτή η αποστολή email.
        /// </summary>
        public Boolean IsCharged
        {
            get { return m_isCharged; }
            internal set { m_isCharged = value; }
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
        internal VLMessageRecipient()
        {
            m_message = default(Int32);
            m_recipient = default(Int64);
            m_attributeFlags = default(Int16);
            m_errorCount = default(Int16);
            m_sendDT = null;
            m_status = default(Byte);
            m_collectorPayment = null;
            m_isCharged = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLMessageRecipient(DbDataReader reader)
        {
            this.Message = reader.GetInt32(0);
            this.Recipient = reader.GetInt64(1);
            this.AttributeFlags = reader.GetInt16(2);
            this.ErrorCount = reader.GetInt16(3);
            if (!reader.IsDBNull(4)) this.SendDT = reader.GetDateTime(4);
            this.Status = (MessageRecipientStatus)reader.GetByte(5);
            if (!reader.IsDBNull(6)) this.CollectorPayment = reader.GetInt32(6);
            this.IsCharged = reader.GetBoolean(7);
            if (!reader.IsDBNull(8)) this.Error = reader.GetString(8);
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Message.GetHashCode() ^
                this.Recipient.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.ErrorCount.GetHashCode() ^
                ((this.SendDT == null) ? string.Empty : this.SendDT.ToString()).GetHashCode() ^
                this.Status.GetHashCode() ^
                ((this.CollectorPayment == null) ? string.Empty : this.CollectorPayment.ToString()).GetHashCode() ^
                this.IsCharged.GetHashCode() ^
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


            var other = (VLMessageRecipient)obj;

            //reference types
            if (!Object.Equals(Error, other.Error)) return false;
            //value types
            if (!m_message.Equals(other.m_message)) return false;
            if (!m_recipient.Equals(other.m_recipient)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_errorCount.Equals(other.m_errorCount)) return false;
            if (!m_sendDT.Equals(other.m_sendDT)) return false;
            if (!m_status.Equals(other.m_status)) return false;
            if (!m_collectorPayment.Equals(other.m_collectorPayment)) return false;
            if (!m_isCharged.Equals(other.m_isCharged)) return false;

            return true;
        }
        public static Boolean operator ==(VLMessageRecipient o1, VLMessageRecipient o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLMessageRecipient o1, VLMessageRecipient o2)
        {
            return !(o1 == o2);
        }

        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_error, false, false, false, -1, "Error");
        }
        #endregion
    }
}
