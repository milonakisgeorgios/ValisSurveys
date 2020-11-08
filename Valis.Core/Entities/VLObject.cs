using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract, Serializable]
    public abstract class VLObject
    {
        [DataMember(Name = "Tag", IsRequired = false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Object m_Tag;
        [DataMember(Name = "InternalTag", IsRequired = false), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private System.Object m_InternalTag;
        [DataMember(Name = "CreationDT", IsRequired = true), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_CreationDT;
        [DataMember(Name = "CreatedBy", IsRequired = true), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 m_CreatedBy;
        [DataMember(Name = "LastUpdateDT", IsRequired = true), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime m_LastUpdateDT;
        [DataMember(Name = "LastUpdatedBy", IsRequired = true), DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Int32 m_LastUpdatedBy;



        /// <summary>
        /// User-defined data associated with the object.
        /// <para>Δεν αντιστοιχεί σε πεδίο στην βάση, και δεν διαχειρίζεται απο το σύστημα!</para>
        /// </summary>
        [System.ComponentModel.Bindable(false)]
        [LocalizableAttribute(false)]
        [DescriptionAttribute("User-defined data associated with the object")]
        public System.Object Tag
        {
            [DebuggerStepThrough]
            get { return m_Tag; }
            set { m_Tag = value; }
        }
        /// <summary>
        /// Runtime-defined data associated with the object.
        /// <para>Δεν αντιστοιχεί σε πεδίο στην βάση!</para>
        /// </summary>
        [System.ComponentModel.Bindable(false)]
        [LocalizableAttribute(false)]
        [DescriptionAttribute("Runtime-defined data associated with the object")]
        internal System.Object InternalTag
        {
            [DebuggerStepThrough]
            get { return m_InternalTag; }
            set { m_InternalTag = value; }
        }

        /// <summary>
        /// Ημερομηνία δημιουργίας αυτής της οντότητας στο σύστημα
        /// <para>Coordinated Universal Time (UTC)</para>
        /// </summary>
        public DateTime CreateDT
        {
            [DebuggerStepThrough]
            get { return m_CreationDT; }
            internal set { m_CreationDT = value; }
        }
        /// <summary>
        /// Ποιο principal είναι υπεύθυνο για την δημιουργία του
        /// </summary>
        public Int32 CreateByPrincipal
        {
            [DebuggerStepThrough]
            get { return m_CreatedBy; }
            internal set { m_CreatedBy = value; }
        }
        /// <summary>
        /// Ημερομηνία τελευταίας αλλαγής των στοιχείων αυτής της οντότητας
        /// <para>Coordinated Universal Time (UTC)</para>
        /// </summary>
        public DateTime LastUpdateDT
        {
            [DebuggerStepThrough]
            get { return m_LastUpdateDT; }
            internal set { m_LastUpdateDT = value; }
        }
        /// <summary>
        /// Ποιο principal είναι υπεύθυνο για την τελευταία αλλαγή
        /// </summary>
        public Int32 LastUpdateByPrincipal
        {
            [DebuggerStepThrough]
            get { return m_LastUpdatedBy; }
            internal set { m_LastUpdatedBy = value; }
        }


        /// <summary>
        /// Χρησιμεύει στο tracking των αλλαγών για την δημιουργία των change logs.
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        protected VLObject()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected VLObject(DbDataReader reader)
        {
            InitializeFromReader(reader);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        protected void InitializeFromReader(DbDataReader reader)
        {
            int _ordinal = reader.GetOrdinal("CreationDT");
            this.m_CreationDT = reader.GetDateTime(_ordinal);

            _ordinal = reader.GetOrdinal("CreatedBy");
            this.m_CreatedBy = reader.GetInt32(_ordinal);

            _ordinal = reader.GetOrdinal("LastUpdateDT");
            this.m_LastUpdateDT = reader.GetDateTime(_ordinal);

            _ordinal = reader.GetOrdinal("LastUpdatedBy");
            this.m_LastUpdatedBy = reader.GetInt32(_ordinal);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected VLObject(VLObject source)
        {
            if (source == null) throw new ArgumentNullException("source");
            m_Tag = source.m_Tag;
            m_InternalTag = source.m_InternalTag;
            m_CreationDT = source.m_CreationDT;
            m_CreatedBy = source.m_CreatedBy;
            m_LastUpdateDT = source.m_LastUpdateDT;
            m_LastUpdatedBy = source.m_LastUpdatedBy;
        }


        /// <summary>
        ///		Indicates state of object
        /// </summary>
        /// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public abstract EntityState EntityState { get; internal set; }

        /// <summary>
        /// True if object has been MarkedToDelete/>. ReadOnly.
        /// </summary>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public virtual bool IsDeleted
        {
            get { return this.EntityState == EntityState.Deleted; }
        }

        /// <summary>
        /// Indicates if the object has been modified from its original state.
        /// </summary>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public virtual bool IsDirty
        {
            get { return this.EntityState != EntityState.Unchanged; }
        }

        /// <summary>
        /// Indicates if the object is new.
        /// </summary>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public virtual bool IsNew
        {
            get { return this.EntityState == EntityState.Added; }
            internal set { this.EntityState = EntityState.Added; }
        }

        ///<summary>
        ///   Marks entity to be deleted.
        ///</summary>
        internal virtual void MarkToDelete()
        {

            if (this.EntityState != EntityState.Added)
                this.EntityState = EntityState.Deleted;
        }

        ///<summary>
        ///   Remove the "isDeleted" mark from the entity.
        ///</summary>
        internal virtual void RemoveDeleteMark()
        {
            if (this.EntityState != EntityState.Added)
            {
                this.EntityState = EntityState.Changed;
            }
        }
    }
}
