using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Αυτή η κλάση αναπαριστά ένα πρότυπο email, το οποίο χρησιμοποιείται απο το σύστημα
    /// σε διάφορα σημείά όταν θέλουμε να στείλουμε emails. Το σύστημα φορτώνει το κατάλληλο template,
    /// αντικαθιστά/επεξεργάζεται τα τυχόν placeholders, και δημιουργεί τo email που απαιτείται.
    /// </summary>
    public sealed class VLEmailTemplate : VLObject
    {
        [Flags]
        internal enum EmailTemplateAttributes : int
        {
            None = 0,
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_templateId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_sender;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_subject;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_body;
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _deserializing = false;

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int16 TemplateId
        {
            get { return m_templateId; }
            internal set { m_templateId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return m_name; }
            set { m_name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Subject
        {
            get { return m_subject; }
            set { m_subject = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Sender
        {
            get { return m_sender; }
            set { m_sender = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Body
        {
            get { return m_body; }
            set { m_body = value; }
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
        public VLEmailTemplate()
        {
            m_templateId = default(Int16);
            m_name = default(string);
            m_sender = default(string);
            m_subject = default(string);
            m_body = default(string);
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLEmailTemplate(DbDataReader reader)
            : base(reader)
        {
            this.TemplateId = reader.GetInt16(0);
            this.Name = reader.GetString(1);
            if (!reader.IsDBNull(2)) this.Sender = reader.GetString(2);
            this.Subject = reader.GetString(3);
            this.Body = reader.GetString(4);
            this.AttributeFlags = reader.GetInt32(5);

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.TemplateId.GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.Sender == null) ? string.Empty : this.Sender.ToString()).GetHashCode() ^
                this.Subject.GetHashCode() ^
                this.Body.GetHashCode() ^
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


            var other = (VLEmailTemplate)obj;

            //reference types
            if (!Object.Equals(m_name, other.m_name)) return false;
            if (!Object.Equals(m_sender, other.m_sender)) return false;
            if (!Object.Equals(m_subject, other.m_subject)) return false;
            if (!Object.Equals(m_body, other.m_body)) return false;
            //value types
            if (!m_templateId.Equals(other.m_templateId)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLEmailTemplate o1, VLEmailTemplate o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLEmailTemplate o1, VLEmailTemplate o2)
        {
            return !(o1 == o2);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            ValidateName(ref m_name);
            Utility.CheckParameter(ref m_subject, true, true, false, 50, "Subject");
            Utility.CheckParameter(ref m_body, true, true, false, -1, "Body");
        }
        internal static void ValidateName(ref string name)
        {
            Utility.CheckParameter(ref name, true, true, false, 50, "Name");
        }
    }
}
