using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    [Serializable]
    public sealed class VLCollectorPayment
    {

        [Flags]
        internal enum CollectorPaymentAttributes : int
        {
            None = 0,
            IsActive = 1,               // 1 << 0
            IsUsed = 2,             // 1 << 1
            AttributeYyy = 4,            // 1 << 2
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collectorPaymentId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_payment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_useOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_quantityLimit;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_quantityReserved;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_quantityUsed;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_firstChargeDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastChargeDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 CollectorPaymentId
        {
            get { return m_collectorPaymentId; }
            internal set { m_collectorPaymentId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Collector
        {
            get { return m_collector; }
            internal set { m_collector = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Payment
        {
            get { return m_payment; }
            internal set { m_payment = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 UseOrder
        {
            get { return m_useOrder; }
            internal set { m_useOrder = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? QuantityLimit
        {
            get { return m_quantityLimit; }
            set { m_quantityLimit = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 QuantityReserved
        {
            get { return m_quantityReserved; }
            internal set { m_quantityReserved = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 QuantityUsed
        {
            get { return m_quantityUsed; }
            internal set { m_quantityUsed = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FirstChargeDt
        {
            get { return m_firstChargeDt; }
            internal set { m_firstChargeDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastChargeDt
        {
            get { return m_lastChargeDt; }
            internal set { m_lastChargeDt = value; }
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
        /// Μας λέει εάν αυτό το CollectorPayment, μπορεί να χρησιμοιηθεί για την λειτουργία του Collector
        /// </summary>
        public System.Boolean IsActive
        {
            get { return (this.m_attributeFlags & ((int)CollectorPaymentAttributes.IsActive)) == ((int)CollectorPaymentAttributes.IsActive); }
            internal set
            {
                if (this.IsActive == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorPaymentAttributes.IsActive;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorPaymentAttributes.IsActive;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Μας λέει εάν αυτό το CollectorPayment, χρησιμοποιήθηκε για την λειτοργία του Collector
        /// </summary>
        public System.Boolean IsUsed
        {
            get { return (this.m_attributeFlags & ((int)CollectorPaymentAttributes.IsUsed)) == ((int)CollectorPaymentAttributes.IsUsed); }
            internal set
            {
                if (this.IsUsed == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorPaymentAttributes.IsUsed;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorPaymentAttributes.IsUsed;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region EntityState
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EntityState _currentEntityState = EntityState.Added;

        /// <summary>
        ///	Indicates state of object
        /// </summary>
        /// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public EntityState EntityState
        {
            get { return _currentEntityState; }
            internal set { _currentEntityState = value; }
        }
        #endregion
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _deserializing = false;


        #region class constructors
        public VLCollectorPayment()
        {
            m_collectorPaymentId = default(Int32);
            m_collector = default(Int32);
            m_payment = default(Int32);
            m_useOrder = default(Int16);
            m_quantityLimit = null;
            m_quantityUsed = default(Int32);
            m_firstChargeDt = null;
            m_lastChargeDt = null;
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLCollectorPayment(DbDataReader reader)
        {
            this.CollectorPaymentId = reader.GetInt32(0);
            this.Collector = reader.GetInt32(1);
            this.Payment = reader.GetInt32(2);
            this.UseOrder = reader.GetInt16(3);
            if (!reader.IsDBNull(4)) this.QuantityLimit = reader.GetInt32(4);
            this.QuantityReserved = reader.GetInt32(5);
            this.QuantityUsed = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.FirstChargeDt = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) this.LastChargeDt = reader.GetDateTime(8);
            this.AttributeFlags = reader.GetInt32(9);


            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.CollectorPaymentId.GetHashCode() ^
                this.Collector.GetHashCode() ^
                this.Payment.GetHashCode() ^
                this.UseOrder.GetHashCode() ^
                ((this.QuantityLimit == null) ? string.Empty : this.QuantityLimit.ToString()).GetHashCode() ^
                this.QuantityReserved.GetHashCode() ^
                this.QuantityUsed.GetHashCode() ^
                ((this.FirstChargeDt == null) ? string.Empty : this.FirstChargeDt.ToString()).GetHashCode() ^
                ((this.LastChargeDt == null) ? string.Empty : this.LastChargeDt.ToString()).GetHashCode() ^
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


            var other = (VLCollectorPayment)obj;

            //reference types
            //value types
            if (!m_collectorPaymentId.Equals(other.m_collectorPaymentId)) return false;
            if (!m_collector.Equals(other.m_collector)) return false;
            if (!m_payment.Equals(other.m_payment)) return false;
            if (!m_useOrder.Equals(other.m_useOrder)) return false;
            if (!m_quantityLimit.Equals(other.m_quantityLimit)) return false;
            if (!m_quantityReserved.Equals(other.m_quantityReserved)) return false;
            if (!m_quantityUsed.Equals(other.m_quantityUsed)) return false;
            if (!m_firstChargeDt.Equals(other.m_firstChargeDt)) return false;
            if (!m_lastChargeDt.Equals(other.m_lastChargeDt)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLCollectorPayment o1, VLCollectorPayment o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLCollectorPayment o1, VLCollectorPayment o2)
        {
            return !(o1 == o2);
        }

        #endregion
    }
}
