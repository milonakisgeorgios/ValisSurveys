using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public sealed class VLPayment : VLObject
    {
        [Flags]
        internal enum PaymentAttributes : int
        {
            None = 0,
            AttributeDdd = 1,            // 1 << 0
            AttributeXxx = 2,            // 1 << 1
            AttributeYyy = 4,            // 1 << 2
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_paymentId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_comment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        PaymentType m_paymentType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_paymentDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_customCode1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_customCode2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isActive;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isTimeLimited;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_validFromDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_validToDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CreditType m_creditType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_quantity;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_quantityUsed;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
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


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 PaymentId
        {
            get { return m_paymentId; }
            internal set { m_paymentId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Client
        {
            get { return m_client; }
            internal set { m_client = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Comment
        {
            get { return m_comment; }
            set { m_comment = value; }
        }
        /// <summary>
        /// Ο τύπος της πληρωμής
        /// <para>Πληροφοριακό πεδίο</para>
        /// </summary>
        public PaymentType PaymentType
        {
            get { return m_paymentType; }
            internal set { m_paymentType = value; }
        }
        /// <summary>
        /// Ημερομηνία που έγινε η πληρωμή
        /// <para>Πληροφοριακό πεδίο</para>
        /// </summary>
        public DateTime PaymentDate
        {
            get { return m_paymentDate; }
            internal set { m_paymentDate = value; }
        }
        /// <summary>
        /// Ενας κωδικός αναφορας για διασύνδεση με τρίτα συστήματα
        /// </summary>
        public String CustomCode1
        {
            get { return m_customCode1; }
            set { m_customCode1 = value; }
        }
        /// <summary>
        /// Ενας κωδικός αναφορας για διασύνδεση με τρίτα συστήματα
        /// </summary>
        public String CustomCode2
        {
            get { return m_customCode2; }
            set { m_customCode2 = value; }
        }
        /// <summary>
        /// Οταν η πληρωμή δεν διαθέτει άλλα credits, τότε το σύστημα την απενεργοποιεί (δεν είναι πιά διαθέσιμη).
        /// </summary>
        public Boolean IsActive
        {
            get { return m_isActive; }
            internal set { m_isActive = value; }
        }
        /// <summary>
        /// Μας πληροφορεί εάν αυτό το payment φέρει χρονικά όρια λήξης.
        /// <para>Δηλαδή το resource που αντιπροσωπεύει ένα χρονικό διάστημα μέσα στο οποίο μπορεί να καταναλωθεί</para>
        /// </summary>
        public Boolean IsTimeLimited
        {
            get { return m_isTimeLimited; }
            set { m_isTimeLimited = value; }
        }
        /// <summary>
        /// Για paymetnts με χρονικά όρια λήξης, μας λέει πότε ξεκινά αυτό το χρονικό διάστημα.
        /// </summary>
        public DateTime? ValidFromDt
        {
            get { return m_validFromDt; }
            set { m_validFromDt = value; }
        }
        /// <summary>
        /// Για paymetnts με χρονικά όρια λήξης, μας λέει πότε τερματίζει αυτό το χρονικό διάστημα.
        /// </summary>
        public DateTime? ValidToDt
        {
            get { return m_validToDt; }
            set { m_validToDt = value; }
        }
        /// <summary>
        /// Μας λέει τον τύπο του resource που αγόρασε αυτή η πληρωμή
        /// <para>'Ενας πελάτης μπορεί να αγοράσει 'emails', 'responses' ή 'clicks'.</para>
        /// </summary>
        public CreditType CreditType
        {
            get { return m_creditType; }
            set { m_creditType = value; }
        }
        /// <summary>
        /// Η ποσότητα του resource που αγόρασε ο Πελάτης με αυτή την πληρωμή.
        /// </summary>
        public Int32 Quantity
        {
            get { return m_quantity; }
            set { m_quantity = value; }
        }
        /// <summary>
        /// Η ποσότητα του resource που έχει χρησιμοποιηθεί απο τον Πελάτη.
        /// <para>Αυτό το πεδίο υπολογίζεται αυτόματα απο το σύστημα!</para>
        /// </summary>
        public Int32 QuantityUsed
        {
            get { return m_quantityUsed; }
            internal set { m_quantityUsed = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }
        #endregion

        #region class constructors
        public VLPayment()
        {
            m_paymentId = default(Int32);
            m_client = default(Int32);
            m_comment = default(string);
            m_paymentType = default(PaymentType);
            m_paymentDate = default(DateTime);
            m_customCode1 = default(string);
            m_customCode2 = default(string);
            m_isActive = default(Boolean);
            m_isTimeLimited = default(Boolean);
            m_validFromDt = null;
            m_validToDt = null;
            m_creditType = default(CreditType);
            m_quantity = default(Int32);
            m_quantityUsed = default(Int32);
            m_attributeFlags = default(Int32);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLPayment(DbDataReader reader)
            : base(reader)
        {
            this.PaymentId = reader.GetInt32(0);
            this.Client = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.Comment = reader.GetString(2);
            this.PaymentType = (PaymentType)reader.GetByte(3);
            this.PaymentDate = reader.GetDateTime(4);
            if (!reader.IsDBNull(5)) this.CustomCode1 = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.CustomCode2 = reader.GetString(6);
            this.IsActive = reader.GetBoolean(7);
            this.IsTimeLimited = reader.GetBoolean(8);
            if (!reader.IsDBNull(9)) this.ValidFromDt = reader.GetDateTime(9);
            if (!reader.IsDBNull(10)) this.ValidToDt = reader.GetDateTime(10);
            this.CreditType = (CreditType)reader.GetByte(11);
            this.Quantity = reader.GetInt32(12);
            this.QuantityUsed = reader.GetInt32(13);
            this.AttributeFlags = reader.GetInt32(14);


            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.PaymentId.GetHashCode() ^
                this.Client.GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode() ^
                this.PaymentType.GetHashCode() ^
                this.PaymentDate.GetHashCode() ^
                ((this.CustomCode1 == null) ? string.Empty : this.CustomCode1.ToString()).GetHashCode() ^
                ((this.CustomCode2 == null) ? string.Empty : this.CustomCode2.ToString()).GetHashCode() ^
                this.IsActive.GetHashCode() ^
                this.IsTimeLimited.GetHashCode() ^
                ((this.ValidFromDt == null) ? string.Empty : this.ValidFromDt.ToString()).GetHashCode() ^
                ((this.ValidToDt == null) ? string.Empty : this.ValidToDt.ToString()).GetHashCode() ^
                this.CreditType.GetHashCode() ^
                this.Quantity.GetHashCode() ^
                this.QuantityUsed.GetHashCode() ^
                this.AttributeFlags.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLPayment)obj;

            //reference types
            if (!Object.Equals(m_comment, other.m_comment)) return false;
            if (!Object.Equals(m_customCode1, other.m_customCode1)) return false;
            if (!Object.Equals(m_customCode2, other.m_customCode2)) return false;
            //value types
            if (!m_paymentId.Equals(other.m_paymentId)) return false;
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_paymentType.Equals(other.m_paymentType)) return false;
            if (!m_paymentDate.Equals(other.m_paymentDate)) return false;
            if (!m_isActive.Equals(other.m_isActive)) return false;
            if (!m_isTimeLimited.Equals(other.m_isTimeLimited)) return false;
            if (!m_validFromDt.Equals(other.m_validFromDt)) return false;
            if (!m_validToDt.Equals(other.m_validToDt)) return false;
            if (!m_creditType.Equals(other.m_creditType)) return false;
            if (!m_quantity.Equals(other.m_quantity)) return false;
            if (!m_quantityUsed.Equals(other.m_quantityUsed)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLPayment o1, VLPayment o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLPayment o1, VLPayment o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_comment, false, false, false, -1, "Comment");
            Utility.CheckParameter(ref m_customCode1, false, false, false, 128, "CustomCode1");
            Utility.CheckParameter(ref m_customCode2, false, false, false, 128, "Customcode2");
        }
    }
}
