using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    [DataContract, DataObject]
    [Serializable]
    public sealed class VLRole : VLObject
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_roleId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_description;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        VLPermissions m_permissions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isBuiltIn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isClientRole;
        #endregion


        #region EntityState
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
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 RoleId
        {
            get { return this.m_roleId; }
            internal set
            {
                if (this.m_roleId == value)
                    return;

                this.m_roleId = value;
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
            internal set
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
        public System.String Description
        {
            get { return this.m_description; }
            internal set
            {
                if (this.m_description == value)
                    return;

                this.m_description = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VLPermissions Permissions
        {
            get { return this.m_permissions; }
            internal set
            {
                if (this.m_permissions == value)
                    return;

                this.m_permissions = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// εάν ένας ρόλος είναι Builtin, τότε δεν μπορούμε να τον αλλάξουμ αλλά ο΄θτε και να τον διαγράψουμε
        /// </summary>
        public System.Boolean IsBuiltIn
        {
            get { return this.m_isBuiltIn; }
            internal set
            {
                if (this.m_isBuiltIn == value)
                    return;

                this.m_isBuiltIn = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει ότι αυτός ο ρόλος προορίζεται για χρήση απο accounts ποτ ανήκουν σε Πελάτες.
        /// </summary>
        public Boolean IsClientRole
        {
            get { return m_isClientRole; }
            internal set
            {
                if (this.m_isClientRole == value)
                    return;

                this.m_isClientRole = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLRole()
        {
            m_roleId = default(Int16);
            m_name = default(string);
            m_description = default(string);
            m_permissions = default(Int64);
            m_isBuiltIn = default(Boolean);
            m_isClientRole = default(Boolean);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLRole(DbDataReader reader)
            : base(reader)
        {
            this.RoleId = reader.GetInt16(0);
            this.Name = reader.GetString(1);
            if (!reader.IsDBNull(2)) this.Description = reader.GetString(2);
            this.Permissions = (VLPermissions)reader.GetInt64(3);
            this.IsBuiltIn = reader.GetBoolean(4);
            this.IsClientRole = reader.GetBoolean(5);

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.RoleId.GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.Description == null) ? string.Empty : this.Description.ToString()).GetHashCode() ^
                this.Permissions.GetHashCode() ^
                this.IsBuiltIn.GetHashCode() ^
                this.IsClientRole.GetHashCode();
        }
        /// <summary>
        /// 
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


            var other = (VLRole)obj;

            //reference types
            if (!Object.Equals(m_name, other.m_name)) return false;
            if (!Object.Equals(m_description, other.m_description)) return false;
            //value types
            if (!m_roleId.Equals(other.m_roleId)) return false;
            if (!m_permissions.Equals(other.m_permissions)) return false;
            if (!m_isBuiltIn.Equals(other.m_isBuiltIn)) return false;
            if (!m_isClientRole.Equals(other.m_isClientRole)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLRole o1, VLRole o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLRole o1, VLRole o2)
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
            Utility.CheckParameter(ref m_description, false, false, false, 1024, "Description");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        internal static void ValidateName(ref string name)
        {
            Utility.CheckParameter(ref name, true, true, false, 64, "Name");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} ({1})", this.Name, (VLPermissions)this.Permissions);
        }

    }
}
