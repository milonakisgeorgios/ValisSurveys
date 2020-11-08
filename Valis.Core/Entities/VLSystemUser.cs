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
    [Serializable]
    [DataContract, DataObject]
    public sealed class VLSystemUser : VLObject
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_userId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_defaultLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_firstName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_lastName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_email;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_timeZoneId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isActive;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isBuiltIn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_role;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_notes;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastActivityDate;
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
        public System.Int32 UserId
        {
            get { return this.m_userId; }
            internal set
            {
                if (this.m_userId == value)
                    return;

                this.m_userId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16? DefaultLanguage
        {
            get { return this.m_defaultLanguage; }
            set
            {
                if (this.m_defaultLanguage == value)
                    return;

                this.m_defaultLanguage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String FirstName
        {
            get { return this.m_firstName; }
            set
            {
                if (this.m_firstName == value)
                    return;

                this.m_firstName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String LastName
        {
            get { return this.m_lastName; }
            set
            {
                if (this.m_lastName == value)
                    return;

                this.m_lastName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Email
        {
            get { return this.m_email; }
            set
            {
                if (this.m_email == value)
                    return;

                this.m_email = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String TimeZoneId
        {
            get { return this.m_timeZoneId; }
            set
            {
                if (this.m_timeZoneId == value)
                    return;

                this.m_timeZoneId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsActive
        {
            get { return this.m_isActive; }
            internal set
            {
                if (this.m_isActive == value)
                    return;

                this.m_isActive = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
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
        public System.Int16 Role
        {
            get { return this.m_role; }
            internal set
            {
                if (this.m_role == value)
                    return;

                this.m_role = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Notes
        {
            get { return this.m_notes; }
            set
            {
                if (this.m_notes == value)
                    return;

                this.m_notes = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastActivityDate
        {
            get { return this.m_lastActivityDate; }
            internal set
            {
                if (this.m_lastActivityDate == value)
                    return;

                this.m_lastActivityDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLSystemUser()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSystemUser(DbDataReader reader)
            : base(reader)
        {

            this.UserId = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) this.DefaultLanguage = reader.GetInt16(1);
            if (!reader.IsDBNull(2)) this.FirstName = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.LastName = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.Email = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.TimeZoneId = reader.GetString(5);
            this.IsActive = reader.GetBoolean(6);
            this.IsBuiltIn = reader.GetBoolean(7);
            this.AttributeFlags = reader.GetInt32(8);
            this.Role = reader.GetInt16(9);
            if (!reader.IsDBNull(10)) this.Notes = reader.GetString(10);
            if (!reader.IsDBNull(11)) this.LastActivityDate = reader.GetDateTime(11);


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
            return this.UserId.GetHashCode() ^
                ((this.DefaultLanguage == null) ? string.Empty : this.DefaultLanguage.ToString()).GetHashCode() ^
                ((this.FirstName == null) ? string.Empty : this.FirstName.ToString()).GetHashCode() ^
                ((this.LastName == null) ? string.Empty : this.LastName.ToString()).GetHashCode() ^
                ((this.Email == null) ? string.Empty : this.Email.ToString()).GetHashCode() ^
                ((this.TimeZoneId == null) ? string.Empty : this.TimeZoneId.ToString()).GetHashCode() ^
                this.IsActive.GetHashCode() ^
                this.IsBuiltIn.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.Role.GetHashCode() ^
                ((this.Notes == null) ? string.Empty : this.Notes.ToString()).GetHashCode() ^
                ((this.LastActivityDate == null) ? string.Empty : this.LastActivityDate.ToString()).GetHashCode();
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


            var other = (VLSystemUser)obj;

            //reference types
            if (!Object.Equals(FirstName, other.FirstName)) return false;
            if (!Object.Equals(LastName, other.LastName)) return false;
            if (!Object.Equals(Email, other.Email)) return false;
            if (!Object.Equals(TimeZoneId, other.TimeZoneId)) return false;
            if (!Object.Equals(Notes, other.Notes)) return false;
            //value types
            if (!UserId.Equals(other.UserId)) return false;
            if (!DefaultLanguage.Equals(other.DefaultLanguage)) return false;
            if (!IsActive.Equals(other.IsActive)) return false;
            if (!IsBuiltIn.Equals(other.IsBuiltIn)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!Role.Equals(other.Role)) return false;
            if (!LastActivityDate.Equals(other.LastActivityDate)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLSystemUser o1, VLSystemUser o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLSystemUser o1, VLSystemUser o2)
        {
            return !(o1 == o2);
        }

        #endregion






        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_firstName, true, true, true, 64, "FirstName");
            Utility.CheckParameter(ref m_lastName, true, true, true, 64, "LastName");
            Utility.CheckParameter(ref m_email, false, false, true, 256, "Email");
            Utility.CheckParameter(ref m_notes, false, false, false, -1, "Notes");
        }

    }
}
