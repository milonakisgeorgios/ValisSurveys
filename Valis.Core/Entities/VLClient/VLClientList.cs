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
    [DataContract, DataObject]
    [Serializable]
    public sealed class VLClientList : VLObject
    {
        [Flags]
        internal enum ClientListAttributes : int
        {
            None = 0,
            AttributeXxx1 = 1,            // 1 << 0
            AttributeXxx2 = 2,            // 1 << 1
            AttributeYyy3 = 4,            // 1 << 2
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_listId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_totalContacts;
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
        public System.Int32 Client
        {
            get { return this.m_client; }
            internal set
            {
                if (this.m_client == value)
                    return;

                this.m_client = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ListId
        {
            get { return this.m_listId; }
            internal set
            {
                if (this.m_listId == value)
                    return;

                this.m_listId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Name
        {
            get { return this.m_name; }
            set
            {
                if (this.m_name == value)
                    return;

                this.m_name = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 TotalContacts
        {
            get { return this.m_totalContacts; }
            internal set
            {
                if (this.m_totalContacts == value)
                    return;

                this.m_totalContacts = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion


        #region class constructors
        public VLClientList()
        {
            m_client = default(Int32);
            m_listId = default(Int32);
            m_name = default(string);
            m_totalContacts = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLClientList(DbDataReader reader)
            : base(reader)
        {
            this.Client = reader.GetInt32(0);
            this.ListId = reader.GetInt32(1);
            this.Name = reader.GetString(2);
            this.TotalContacts = reader.GetInt32(3);

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
            return this.Client.GetHashCode() ^
                this.ListId.GetHashCode() ^
                this.Name.GetHashCode() ^
                this.TotalContacts.GetHashCode();
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


            var other = (VLClientList)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            //value types
            if (!Client.Equals(other.Client)) return false;
            if (!ListId.Equals(other.ListId)) return false;
            if (!TotalContacts.Equals(other.TotalContacts)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLClientList o1, VLClientList o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLClientList o1, VLClientList o2)
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
        }
        internal static void ValidateName(ref string name)
        {
            Utility.CheckParameter(ref name, true, true, false, 512, "Name");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if(this.TotalContacts == 0)
            {
                return string.Format("{0} (empty)", this.Name);
            }
            else if(this.TotalContacts == 1)
            {
                return string.Format("{0} (1 contact)", this.Name);
            }
            else
            {
                return string.Format("{0} ({1} contacts)", this.Name, this.TotalContacts);
            }
        }
    }
}
