using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    public sealed class VLMessage : VLObject
    {
        [Flags]
        internal enum CollectorMessageAttributes : int
        {
            None                = 0,
            IsDeliveryMethodOK  = 1,            // 1 << 0
            IsSenderOK          = 2,            // 1 << 1
            IsContentOK         = 4,            // 1 << 2
            IsScheduleOK        = 8,            // 1 << 3
            SkipPaymentValidations = 16         // 1 << 4
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_messageId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_sender;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_subject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_body;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        MessageStatus m_status;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_scheduledAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_sentCounter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_failedCounter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_skipCounter;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DeliveryMethod m_deliveryMethod;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RecipientSearchField? m_customSearchField;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ComparisonOperator? m_customOperator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_customKeyword;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_pendingAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_preparingAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_preparedAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_executingAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_terminatedAt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_error;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_senderVerificationCode;
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
        /// 
        /// </summary>
        public System.Int32 MessageId
        {
            get { return this.m_messageId; }
            internal set
            {
                if (this.m_messageId == value)
                    return;

                this.m_messageId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Sender
        {
            get { return this.m_sender; }
            set
            {
                if (this.m_sender == value)
                    return;

                this.m_sender = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Subject
        {
            get { return this.m_subject; }
            set
            {
                if (this.m_subject == value)
                    return;

                this.m_subject = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>
        ///     Το σύστημα υποστηρίζει τα εξής δυναμικά πεδία:
        ///     [@SurveyLink]    
        ///     [@RemoveLink]
        ///     [@Title]
        ///     [@FirstName]
        ///     [@LastName]
        ///     [@CustomValue]
        /// </para>
        /// </summary>
        public System.String Body
        {
            get { return this.m_body; }
            set
            {
                if (this.m_body == value)
                    return;

                this.m_body = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public MessageStatus Status
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
        /// 
        /// </summary>
        public System.Boolean IsDeliveryMethodOK
        {
            get { return (this.m_attributeFlags & ((int)CollectorMessageAttributes.IsDeliveryMethodOK)) == ((int)CollectorMessageAttributes.IsDeliveryMethodOK); }
            internal set
            {
                if (this.IsDeliveryMethodOK == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorMessageAttributes.IsDeliveryMethodOK;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorMessageAttributes.IsDeliveryMethodOK;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsSenderOK
        {
            get { return (this.m_attributeFlags & ((int)CollectorMessageAttributes.IsSenderOK)) == ((int)CollectorMessageAttributes.IsSenderOK); }
            internal set
            {
                if (this.IsSenderOK == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorMessageAttributes.IsSenderOK;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorMessageAttributes.IsSenderOK;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsContentOK
        {
            get { return (this.m_attributeFlags & ((int)CollectorMessageAttributes.IsContentOK)) == ((int)CollectorMessageAttributes.IsContentOK); }
            internal set
            {
                if (this.IsContentOK == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorMessageAttributes.IsContentOK;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorMessageAttributes.IsContentOK;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsScheduleOK
        {
            get { return (this.m_attributeFlags & ((int)CollectorMessageAttributes.IsScheduleOK)) == ((int)CollectorMessageAttributes.IsScheduleOK); }
            internal set
            {
                if (this.IsScheduleOK == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorMessageAttributes.IsScheduleOK;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorMessageAttributes.IsScheduleOK;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Εάν αυτό το flag είναι true, τότε το συγκεκριμένο Message, προσπερνάει όλους τους ελέγχους για τον εάν υπάρχει payment, κατα τον χρονοπρογραμματισμό του, και κατα την
        /// αποστολή των emails στους παραλήπτες του μηνύματος.
        /// <para>Μόνο για testing purposes!</para>
        /// </summary>
        internal System.Boolean SkipPaymentValidations
        {
            get { return (this.m_attributeFlags & ((int)CollectorMessageAttributes.SkipPaymentValidations)) == ((int)CollectorMessageAttributes.SkipPaymentValidations); }
            set
            {
                if (this.SkipPaymentValidations == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorMessageAttributes.SkipPaymentValidations;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorMessageAttributes.SkipPaymentValidations;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// H ημερομηνία και ώρα έναρξης αποστολής του μηνύματος στους παραλάηπτες του.
        /// <para>Coordinated Universal Time (UTC)</para>
        /// </summary>
        public System.DateTime? ScheduledAt
        {
            get { return this.m_scheduledAt; }
            internal set
            {
                if (this.m_scheduledAt == value)
                    return;

                this.m_scheduledAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 SentCounter
        {
            get { return this.m_sentCounter; }
            internal set
            {
                if (this.m_sentCounter == value)
                    return;

                this.m_sentCounter = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 FailedCounter
        {
            get { return m_failedCounter; }
            internal set
            {
                if (this.m_failedCounter == value)
                    return;

                this.m_failedCounter = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 SkipCounter
        {
            get { return m_skipCounter; }
            internal set
            {
                if (this.m_skipCounter == value)
                    return;

                this.m_skipCounter = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public DeliveryMethod DeliveryMethod
        {
            get { return this.m_deliveryMethod; }
            set
            {
                if (this.m_deliveryMethod == value)
                    return;

                this.m_deliveryMethod = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RecipientSearchField? CustomSearchField
        {
            get { return this.m_customSearchField; }
            set
            {
                if (this.m_customSearchField == value)
                    return;

                this.m_customSearchField = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ComparisonOperator? CustomOperator
        {
            get { return this.m_customOperator; }
            set
            {
                if (this.m_customOperator == value)
                    return;

                this.m_customOperator = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String CustomKeyword
        {
            get { return this.m_customKeyword; }
            set
            {
                if (this.m_customKeyword == value)
                    return;

                this.m_customKeyword = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? PendingAt
        {
            get { return this.m_pendingAt; }
            internal set
            {
                if (this.m_pendingAt == value)
                    return;

                this.m_pendingAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? PreparingAt
        {
            get { return this.m_preparingAt; }
            internal set
            {
                if (this.m_preparingAt == value)
                    return;

                this.m_preparingAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? PreparedAt
        {
            get { return this.m_preparedAt; }
            internal set
            {
                if (this.m_preparedAt == value)
                    return;

                this.m_preparedAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? ExecutingAt
        {
            get { return this.m_executingAt; }
            internal set
            {
                if (this.m_executingAt == value)
                    return;

                this.m_executingAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? TerminatedAt
        {
            get { return this.m_terminatedAt; }
            internal set
            {
                if (this.m_terminatedAt == value)
                    return;

                this.m_terminatedAt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }


        public System.String Error
        {
            get { return this.m_error; }
            internal set
            {
                if (this.m_error == value)
                    return;

                this.m_error = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Αυτός είναι ο κωδικός που στέλνει το σύστημα για να πραγματοποιηθεί το verification 
        /// του sender email (εάν αυτό χρειάζεται)
        /// </summary>
        public System.String SenderVerificationCode
        {
            get { return this.m_senderVerificationCode; }
            internal set
            {
                if (this.m_senderVerificationCode == value)
                    return;

                this.m_senderVerificationCode = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLMessage()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLMessage(DbDataReader reader)
            : base(reader)
        {
            this.Collector = reader.GetInt32(0);
            this.MessageId = reader.GetInt32(1);
            this.Sender = reader.GetString(2);
            this.Subject = reader.GetString(3);
            this.Body = reader.GetString(4);
            this.Status = (MessageStatus)reader.GetByte(5);
            this.AttributeFlags = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.ScheduledAt = reader.GetDateTime(7);
            this.SentCounter = reader.GetInt32(8);
            this.FailedCounter = reader.GetInt32(9);
            this.SkipCounter = reader.GetInt32(10);
            this.DeliveryMethod = (DeliveryMethod)reader.GetByte(11);
            if (!reader.IsDBNull(12)) this.CustomSearchField = (RecipientSearchField)reader.GetByte(12);
            if (!reader.IsDBNull(13)) this.CustomOperator = (ComparisonOperator)reader.GetByte(13);
            if (!reader.IsDBNull(14)) this.CustomKeyword = reader.GetString(14);
            if (!reader.IsDBNull(15)) this.PendingAt = reader.GetDateTime(15);
            if (!reader.IsDBNull(16)) this.PreparingAt = reader.GetDateTime(16);
            if (!reader.IsDBNull(17)) this.PreparedAt = reader.GetDateTime(17);
            if (!reader.IsDBNull(18)) this.ExecutingAt = reader.GetDateTime(18);
            if (!reader.IsDBNull(19)) this.TerminatedAt = reader.GetDateTime(19);
            if (!reader.IsDBNull(20)) this.Error = reader.GetString(20);
            if (!reader.IsDBNull(21)) this.SenderVerificationCode = reader.GetString(21);

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Collector.GetHashCode() ^
                this.MessageId.GetHashCode() ^
                this.Sender.GetHashCode() ^
                this.Subject.GetHashCode() ^
                this.Body.GetHashCode() ^
                this.Status.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.ScheduledAt == null) ? string.Empty : this.ScheduledAt.ToString()).GetHashCode() ^
                this.SentCounter.GetHashCode() ^
                this.FailedCounter.GetHashCode() ^
                this.SkipCounter.GetHashCode() ^
                this.DeliveryMethod.GetHashCode() ^
                ((this.CustomSearchField == null) ? string.Empty : this.CustomSearchField.ToString()).GetHashCode() ^
                ((this.CustomOperator == null) ? string.Empty : this.CustomOperator.ToString()).GetHashCode() ^
                ((this.CustomKeyword == null) ? string.Empty : this.CustomKeyword.ToString()).GetHashCode() ^
                ((this.PendingAt == null) ? string.Empty : this.PendingAt.ToString()).GetHashCode() ^
                ((this.PreparingAt == null) ? string.Empty : this.PreparingAt.ToString()).GetHashCode() ^
                ((this.PreparedAt == null) ? string.Empty : this.PreparedAt.ToString()).GetHashCode() ^
                ((this.ExecutingAt == null) ? string.Empty : this.ExecutingAt.ToString()).GetHashCode() ^
                ((this.TerminatedAt == null) ? string.Empty : this.TerminatedAt.ToString()).GetHashCode() ^
                ((this.Error == null) ? string.Empty : this.Error.ToString()).GetHashCode() ^
                ((this.SenderVerificationCode == null) ? string.Empty : this.SenderVerificationCode.ToString()).GetHashCode();
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


            var other = (VLMessage)obj;

            //reference types
            if (!Object.Equals(Sender, other.Sender)) return false;
            if (!Object.Equals(Subject, other.Subject)) return false;
            if (!Object.Equals(Body, other.Body)) return false;
            if (!Object.Equals(CustomKeyword, other.CustomKeyword)) return false;
            if (!Object.Equals(Error, other.Error)) return false;
            if (!Object.Equals(SenderVerificationCode, other.SenderVerificationCode)) return false;
            //value types
            if (!Collector.Equals(other.Collector)) return false;
            if (!MessageId.Equals(other.MessageId)) return false;
            if (!Status.Equals(other.Status)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!ScheduledAt.Equals(other.ScheduledAt)) return false;
            if (!SentCounter.Equals(other.SentCounter)) return false;
            if (!FailedCounter.Equals(other.FailedCounter)) return false;
            if (!SkipCounter.Equals(other.SkipCounter)) return false;
            if (!DeliveryMethod.Equals(other.DeliveryMethod)) return false;
            if (!CustomSearchField.Equals(other.CustomSearchField)) return false;
            if (!CustomOperator.Equals(other.CustomOperator)) return false;
            if (!PendingAt.Equals(other.PendingAt)) return false;
            if (!PreparingAt.Equals(other.PreparingAt)) return false;
            if (!PreparedAt.Equals(other.PreparedAt)) return false;
            if (!ExecutingAt.Equals(other.ExecutingAt)) return false;
            if (!TerminatedAt.Equals(other.TerminatedAt)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLMessage o1, VLMessage o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLMessage o1, VLMessage o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_sender, true, true, false, 128, "Sender");
            Utility.CheckParameter(ref m_subject, true, true, false, 256, "Subject");
            Utility.CheckParameter(ref m_body, true, true, false, 2048, "Body");
            Utility.CheckParameter(ref m_customKeyword, false, false, false, 128, "CustomKeyword");
            Utility.CheckParameter(ref m_error, false, false, false, -1, "Error");
            Utility.CheckParameter(ref m_senderVerificationCode, false, false, false, 64, "SenderVerificationCode");
        }
    }
}
