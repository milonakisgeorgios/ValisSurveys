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
    public sealed class VLClientUser : VLObject
    {

        [Flags]
        internal enum ClientUserAttributes : int
        {
            None = 0,
            AttributeXxx = 1,          // 1 << 0
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_userId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_defaultLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_title;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_department;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_firstName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_lastName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_country;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_timeZoneId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_prefecture;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_town;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_address;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_zip;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_telephone1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_telephone2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_email;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isActive;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isBuiltIn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_role;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_comment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastActivityDate;
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
        public System.String Title
        {
            get { return this.m_title; }
            set
            {
                if (this.m_title == value)
                    return;

                this.m_title = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Department
        {
            get { return this.m_department; }
            set
            {
                if (this.m_department == value)
                    return;

                this.m_department = value;
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
        public System.Int32? Country
        {
            get { return this.m_country; }
            set
            {
                if (this.m_country == value)
                    return;

                this.m_country = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String TimeZoneId
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
        public System.String Prefecture
        {
            get { return this.m_prefecture; }
            set
            {
                if (this.m_prefecture == value)
                    return;

                this.m_prefecture = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Town
        {
            get { return this.m_town; }
            set
            {
                if (this.m_town == value)
                    return;

                this.m_town = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Address
        {
            get { return this.m_address; }
            set
            {
                if (this.m_address == value)
                    return;

                this.m_address = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Zip
        {
            get { return this.m_zip; }
            set
            {
                if (this.m_zip == value)
                    return;

                this.m_zip = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Telephone1
        {
            get { return this.m_telephone1; }
            set
            {
                if (this.m_telephone1 == value)
                    return;

                this.m_telephone1 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Telephone2
        {
            get { return this.m_telephone2; }
            set
            {
                if (this.m_telephone2 == value)
                    return;

                this.m_telephone2 = value;
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
        public System.String Comment
        {
            get { return this.m_comment; }
            set
            {
                if (this.m_comment == value)
                    return;

                this.m_comment = value;
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
        public VLClientUser()
        {
            m_client = default(Int32);
            m_userId = default(Int32);
            m_defaultLanguage = null;
            m_title = default(string);
            m_department = default(string);
            m_firstName = default(string);
            m_lastName = default(string);
            m_country = null;
            m_timeZoneId = default(string);
            m_prefecture = default(string);
            m_town = default(string);
            m_address = default(string);
            m_zip = default(string);
            m_telephone1 = default(string);
            m_telephone2 = default(string);
            m_email = default(string);
            m_isActive = default(Boolean);
            m_isBuiltIn = default(Boolean);
            m_attributeFlags = default(Int32);
            m_role = default(Int16);
            m_comment = default(string);
            m_lastActivityDate = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLClientUser(DbDataReader reader)
            : base(reader)
        {
            this.Client = reader.GetInt32(0);
            this.UserId = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.DefaultLanguage = reader.GetInt16(2);
            if (!reader.IsDBNull(3)) this.Title = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.Department = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.FirstName = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.LastName = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.Country = reader.GetInt32(7);
            if (!reader.IsDBNull(8)) this.TimeZoneId = reader.GetString(8);
            if (!reader.IsDBNull(9)) this.Prefecture = reader.GetString(9);
            if (!reader.IsDBNull(10)) this.Town = reader.GetString(10);
            if (!reader.IsDBNull(11)) this.Address = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.Zip = reader.GetString(12);
            if (!reader.IsDBNull(13)) this.Telephone1 = reader.GetString(13);
            if (!reader.IsDBNull(14)) this.Telephone2 = reader.GetString(14);
            if (!reader.IsDBNull(15)) this.Email = reader.GetString(15);
            this.IsActive = reader.GetBoolean(16);
            this.IsBuiltIn = reader.GetBoolean(17);
            this.AttributeFlags = reader.GetInt32(18);
            this.Role = reader.GetInt16(19);
            if (!reader.IsDBNull(20)) this.Comment = reader.GetString(20);
            if (!reader.IsDBNull(21)) this.LastActivityDate = reader.GetDateTime(21);


            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Client.GetHashCode() ^
                this.UserId.GetHashCode() ^
                ((this.DefaultLanguage == null) ? string.Empty : this.DefaultLanguage.ToString()).GetHashCode() ^
                ((this.Title == null) ? string.Empty : this.Title.ToString()).GetHashCode() ^
                ((this.Department == null) ? string.Empty : this.Department.ToString()).GetHashCode() ^
                ((this.FirstName == null) ? string.Empty : this.FirstName.ToString()).GetHashCode() ^
                ((this.LastName == null) ? string.Empty : this.LastName.ToString()).GetHashCode() ^
                ((this.Country == null) ? string.Empty : this.Country.ToString()).GetHashCode() ^
                ((this.TimeZoneId == null) ? string.Empty : this.TimeZoneId.ToString()).GetHashCode() ^
                ((this.Prefecture == null) ? string.Empty : this.Prefecture.ToString()).GetHashCode() ^
                ((this.Town == null) ? string.Empty : this.Town.ToString()).GetHashCode() ^
                ((this.Address == null) ? string.Empty : this.Address.ToString()).GetHashCode() ^
                ((this.Zip == null) ? string.Empty : this.Zip.ToString()).GetHashCode() ^
                ((this.Telephone1 == null) ? string.Empty : this.Telephone1.ToString()).GetHashCode() ^
                ((this.Telephone2 == null) ? string.Empty : this.Telephone2.ToString()).GetHashCode() ^
                ((this.Email == null) ? string.Empty : this.Email.ToString()).GetHashCode() ^
                this.IsActive.GetHashCode() ^
                this.IsBuiltIn.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.Role.GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode() ^
                ((this.LastActivityDate == null) ? string.Empty : this.LastActivityDate.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLClientUser)obj;

            //reference types
            if (!Object.Equals(m_title, other.m_title)) return false;
            if (!Object.Equals(m_department, other.m_department)) return false;
            if (!Object.Equals(m_firstName, other.m_firstName)) return false;
            if (!Object.Equals(m_lastName, other.m_lastName)) return false;
            if (!Object.Equals(m_timeZoneId, other.m_timeZoneId)) return false;
            if (!Object.Equals(m_prefecture, other.m_prefecture)) return false;
            if (!Object.Equals(m_town, other.m_town)) return false;
            if (!Object.Equals(m_address, other.m_address)) return false;
            if (!Object.Equals(m_zip, other.m_zip)) return false;
            if (!Object.Equals(m_telephone1, other.m_telephone1)) return false;
            if (!Object.Equals(m_telephone2, other.m_telephone2)) return false;
            if (!Object.Equals(m_email, other.m_email)) return false;
            if (!Object.Equals(m_comment, other.m_comment)) return false;
            //value types
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_userId.Equals(other.m_userId)) return false;
            if (!m_defaultLanguage.Equals(other.m_defaultLanguage)) return false;
            if (!m_country.Equals(other.m_country)) return false;
            if (!m_isActive.Equals(other.m_isActive)) return false;
            if (!m_isBuiltIn.Equals(other.m_isBuiltIn)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_role.Equals(other.m_role)) return false;
            if (!m_lastActivityDate.Equals(other.m_lastActivityDate)) return false;

            return true;
        }
        public static Boolean operator ==(VLClientUser o1, VLClientUser o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLClientUser o1, VLClientUser o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_title, false, false, false, 256, "Title");
            Utility.CheckParameter(ref m_department, false, false, false, 256, "Department");
            Utility.CheckParameter(ref m_firstName, true, true, false, 128, "FirstName");
            Utility.CheckParameter(ref m_lastName, true, true, false, 128, "LastName");
            Utility.CheckParameter(ref m_prefecture, false, false, false, 128, "Prefecture");
            Utility.CheckParameter(ref m_town, false, false, false, 128, "Town");
            Utility.CheckParameter(ref m_address, false, false, false, 512, "Address");
            Utility.CheckParameter(ref m_zip, false, false, false, 24, "Zip");
            Utility.CheckParameter(ref m_telephone1, false, false, false, 128, "Telephone1");
            Utility.CheckParameter(ref m_telephone2, false, false, false, 128, "Telephone2");
            Utility.CheckParameter(ref m_email, false, false, true, 256, "Email");
            Utility.CheckParameter(ref m_comment, false, false, false, -1, "Comment");
        }
    }
}
