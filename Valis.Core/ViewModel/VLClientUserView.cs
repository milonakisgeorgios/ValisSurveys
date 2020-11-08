using System;
using System.ComponentModel;
using System.Data.Common;
using System.Xml.Serialization;

namespace Valis.Core.ViewModel
{
    public sealed class VLClientUserView : VLObject
    {
        #region class fields
        Int32 m_client;
        Int32 m_userId;
        Int16? m_defaultLanguageId;
        String m_defaultLanguageName;
        String m_title;
        String m_department;
        String m_firstName;
        String m_lastName;
        Int32? m_countryId;
        String m_countryName;
        String m_timeZoneId;
        String m_prefecture;
        String m_town;
        String m_address;
        String m_zip;
        String m_telephone1;
        String m_telephone2;
        String m_email;
        Boolean m_isActive;
        Boolean m_isBuiltIn;
        Int32 m_attributeFlags;
        Int16 m_roleId;
        String m_roleName;
        DateTime? m_lastActivityDate;
        Int32 m_credentialId;
        String m_logOnToken;
        Int32 m_pswdFormat;
        Boolean m_isApproved;
        Boolean m_isLockedOut;
        DateTime? m_lastLoginDate;
        DateTime? m_lastPasswordChangedDate;
        DateTime? m_lastLockoutDate;
        Int32 m_failedPasswordAttemptCount;
        DateTime? m_failedPasswordAttemptWindowStart;
        Int32 m_failedPasswordAnswerAttemptCount;
        DateTime? m_failedPasswordAnswerAttemptWindowStart;
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
        public Int32 Client
        {
            get { return m_client; }
            internal set { m_client = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 UserId
        {
            get { return m_userId; }
            internal set { m_userId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? DefaultLanguageId
        {
            get { return m_defaultLanguageId; }
            internal set { m_defaultLanguageId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String DefaultLanguageName
        {
            get { return m_defaultLanguageName; }
            internal set { m_defaultLanguageName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Title
        {
            get { return m_title; }
            internal set { m_title = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Department
        {
            get { return m_department; }
            internal set { m_department = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String FirstName
        {
            get { return m_firstName; }
            internal set { m_firstName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String LastName
        {
            get { return m_lastName; }
            internal set { m_lastName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? CountryId
        {
            get { return m_countryId; }
            internal set { m_countryId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String CountryName
        {
            get { return m_countryName; }
            internal set { m_countryName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String TimeZoneId
        {
            get { return m_timeZoneId; }
            internal set { m_timeZoneId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Prefecture
        {
            get { return m_prefecture; }
            internal set { m_prefecture = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Town
        {
            get { return m_town; }
            internal set { m_town = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Address
        {
            get { return m_address; }
            internal set { m_address = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Zip
        {
            get { return m_zip; }
            internal set { m_zip = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Telephone1
        {
            get { return m_telephone1; }
            internal set { m_telephone1 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Telephone2
        {
            get { return m_telephone2; }
            internal set { m_telephone2 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Email
        {
            get { return m_email; }
            internal set { m_email = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsActive
        {
            get { return m_isActive; }
            internal set { m_isActive = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsBuiltIn
        {
            get { return m_isBuiltIn; }
            internal set { m_isBuiltIn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            internal set { m_attributeFlags = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 RoleId
        {
            get { return m_roleId; }
            internal set { m_roleId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String RoleName
        {
            get { return m_roleName; }
            internal set { m_roleName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastActivityDate
        {
            get { return m_lastActivityDate; }
            internal set { m_lastActivityDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 CredentialId
        {
            get { return m_credentialId; }
            internal set { m_credentialId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String LogOnToken
        {
            get { return m_logOnToken; }
            internal set { m_logOnToken = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 PswdFormat
        {
            get { return m_pswdFormat; }
            internal set { m_pswdFormat = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsApproved
        {
            get { return m_isApproved; }
            internal set { m_isApproved = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsLockedOut
        {
            get { return m_isLockedOut; }
            internal set { m_isLockedOut = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLoginDate
        {
            get { return m_lastLoginDate; }
            internal set { m_lastLoginDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastPasswordChangedDate
        {
            get { return m_lastPasswordChangedDate; }
            internal set { m_lastPasswordChangedDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastLockoutDate
        {
            get { return m_lastLockoutDate; }
            internal set { m_lastLockoutDate = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 FailedPasswordAttemptCount
        {
            get { return m_failedPasswordAttemptCount; }
            internal set { m_failedPasswordAttemptCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FailedPasswordAttemptWindowStart
        {
            get { return m_failedPasswordAttemptWindowStart; }
            internal set { m_failedPasswordAttemptWindowStart = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 FailedPasswordAnswerAttemptCount
        {
            get { return m_failedPasswordAnswerAttemptCount; }
            internal set { m_failedPasswordAnswerAttemptCount = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? FailedPasswordAnswerAttemptWindowStart
        {
            get { return m_failedPasswordAnswerAttemptWindowStart; }
            internal set { m_failedPasswordAnswerAttemptWindowStart = value; }
        }
        #endregion

        #region class constructors
        public VLClientUserView()
        {
            m_client = default(Int32);
            m_userId = default(Int32);
            m_defaultLanguageId = null;
            m_defaultLanguageName = default(string);
            m_title = default(string);
            m_department = default(string);
            m_firstName = default(string);
            m_lastName = default(string);
            m_countryId = null;
            m_countryName = default(string);
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
            m_roleId = default(Int16);
            m_roleName = default(string);
            m_lastActivityDate = null;
            m_credentialId = default(Int32);
            m_logOnToken = default(string);
            m_pswdFormat = default(Int32);
            m_isApproved = default(Boolean);
            m_isLockedOut = default(Boolean);
            m_lastLoginDate = null;
            m_lastPasswordChangedDate = null;
            m_lastLockoutDate = null;
            m_failedPasswordAttemptCount = default(Int32);
            m_failedPasswordAttemptWindowStart = null;
            m_failedPasswordAnswerAttemptCount = default(Int32);
            m_failedPasswordAnswerAttemptWindowStart = null;
        }

        internal VLClientUserView(DbDataReader reader)
            : base(reader)
        {
            this.Client = reader.GetInt32(0);
            this.UserId = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.DefaultLanguageId = reader.GetInt16(2);
            this.DefaultLanguageName = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.Title = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.Department = reader.GetString(5);
            this.FirstName = reader.GetString(6);
            this.LastName = reader.GetString(7);
            if (!reader.IsDBNull(8)) this.CountryId = reader.GetInt32(8);
            if (!reader.IsDBNull(9)) this.CountryName = reader.GetString(9);


            if (!reader.IsDBNull(10)) this.TimeZoneId = reader.GetString(10);

            if (!reader.IsDBNull(11)) this.Prefecture = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.Town = reader.GetString(12);
            if (!reader.IsDBNull(13)) this.Address = reader.GetString(13);
            if (!reader.IsDBNull(14)) this.Zip = reader.GetString(14);
            if (!reader.IsDBNull(15)) this.Telephone1 = reader.GetString(15);
            if (!reader.IsDBNull(16)) this.Telephone2 = reader.GetString(16);
            if (!reader.IsDBNull(17)) this.Email = reader.GetString(17);
            this.IsActive = reader.GetBoolean(18);
            this.IsBuiltIn = reader.GetBoolean(19);
            this.AttributeFlags = reader.GetInt32(20);
            this.RoleId = reader.GetInt16(21);
            this.RoleName = reader.GetString(22);
            if (!reader.IsDBNull(23)) this.LastActivityDate = reader.GetDateTime(23);

            //CreationDT
            //CreatedBy
            //LastUpdateDT
            //LastUpdatedBy

            this.CredentialId = reader.GetInt32(28);
            this.LogOnToken = reader.GetString(29);
            this.PswdFormat = reader.GetInt32(30);
            this.IsApproved = reader.GetBoolean(31);
            this.IsLockedOut = reader.GetBoolean(32);
            if (!reader.IsDBNull(33)) this.LastLoginDate = reader.GetDateTime(33);
            if (!reader.IsDBNull(34)) this.LastPasswordChangedDate = reader.GetDateTime(34);
            if (!reader.IsDBNull(35)) this.LastLockoutDate = reader.GetDateTime(35);
            this.FailedPasswordAttemptCount = reader.GetInt32(36);
            if (!reader.IsDBNull(37)) this.FailedPasswordAttemptWindowStart = reader.GetDateTime(37);
            this.FailedPasswordAnswerAttemptCount = reader.GetInt32(38);
            if (!reader.IsDBNull(39)) this.FailedPasswordAnswerAttemptWindowStart = reader.GetDateTime(39);
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Client.GetHashCode() ^
                this.UserId.GetHashCode() ^
                ((this.DefaultLanguageId == null) ? string.Empty : this.DefaultLanguageId.ToString()).GetHashCode() ^
                this.DefaultLanguageName.GetHashCode() ^
                ((this.Title == null) ? string.Empty : this.Title.ToString()).GetHashCode() ^
                ((this.Department == null) ? string.Empty : this.Department.ToString()).GetHashCode() ^
                this.FirstName.GetHashCode() ^
                this.LastName.GetHashCode() ^
                ((this.CountryId == null) ? string.Empty : this.CountryId.ToString()).GetHashCode() ^
                ((this.CountryName == null) ? string.Empty : this.CountryName.ToString()).GetHashCode() ^
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
                this.RoleId.GetHashCode() ^
                this.RoleName.GetHashCode() ^
                ((this.LastActivityDate == null) ? string.Empty : this.LastActivityDate.ToString()).GetHashCode() ^
                this.CredentialId.GetHashCode() ^
                this.LogOnToken.GetHashCode() ^
                this.PswdFormat.GetHashCode() ^
                this.IsApproved.GetHashCode() ^
                this.IsLockedOut.GetHashCode() ^
                ((this.LastLoginDate == null) ? string.Empty : this.LastLoginDate.ToString()).GetHashCode() ^
                ((this.LastPasswordChangedDate == null) ? string.Empty : this.LastPasswordChangedDate.ToString()).GetHashCode() ^
                ((this.LastLockoutDate == null) ? string.Empty : this.LastLockoutDate.ToString()).GetHashCode() ^
                this.FailedPasswordAttemptCount.GetHashCode() ^
                ((this.FailedPasswordAttemptWindowStart == null) ? string.Empty : this.FailedPasswordAttemptWindowStart.ToString()).GetHashCode() ^
                this.FailedPasswordAnswerAttemptCount.GetHashCode() ^
                ((this.FailedPasswordAnswerAttemptWindowStart == null) ? string.Empty : this.FailedPasswordAnswerAttemptWindowStart.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLClientUserView)obj;

            //reference types
            if (!Object.Equals(m_defaultLanguageName, other.m_defaultLanguageName)) return false;
            if (!Object.Equals(m_title, other.m_title)) return false;
            if (!Object.Equals(m_department, other.m_department)) return false;
            if (!Object.Equals(m_firstName, other.m_firstName)) return false;
            if (!Object.Equals(m_lastName, other.m_lastName)) return false;
            if (!Object.Equals(m_countryName, other.m_countryName)) return false;
            if (!Object.Equals(m_prefecture, other.m_prefecture)) return false;
            if (!Object.Equals(m_town, other.m_town)) return false;
            if (!Object.Equals(m_address, other.m_address)) return false;
            if (!Object.Equals(m_zip, other.m_zip)) return false;
            if (!Object.Equals(m_telephone1, other.m_telephone1)) return false;
            if (!Object.Equals(m_telephone2, other.m_telephone2)) return false;
            if (!Object.Equals(m_email, other.m_email)) return false;
            if (!Object.Equals(m_roleName, other.m_roleName)) return false;
            if (!Object.Equals(m_logOnToken, other.m_logOnToken)) return false;
            //value types
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_userId.Equals(other.m_userId)) return false;
            if (!m_defaultLanguageId.Equals(other.m_defaultLanguageId)) return false;
            if (!m_countryId.Equals(other.m_countryId)) return false;
            if (!m_isActive.Equals(other.m_isActive)) return false;
            if (!m_isBuiltIn.Equals(other.m_isBuiltIn)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_roleId.Equals(other.m_roleId)) return false;
            if (!m_lastActivityDate.Equals(other.m_lastActivityDate)) return false;
            if (!m_credentialId.Equals(other.m_credentialId)) return false;
            if (!m_pswdFormat.Equals(other.m_pswdFormat)) return false;
            if (!m_isApproved.Equals(other.m_isApproved)) return false;
            if (!m_isLockedOut.Equals(other.m_isLockedOut)) return false;
            if (!m_lastLoginDate.Equals(other.m_lastLoginDate)) return false;
            if (!m_lastPasswordChangedDate.Equals(other.m_lastPasswordChangedDate)) return false;
            if (!m_lastLockoutDate.Equals(other.m_lastLockoutDate)) return false;
            if (!m_failedPasswordAttemptCount.Equals(other.m_failedPasswordAttemptCount)) return false;
            if (!m_failedPasswordAttemptWindowStart.Equals(other.m_failedPasswordAttemptWindowStart)) return false;
            if (!m_failedPasswordAnswerAttemptCount.Equals(other.m_failedPasswordAnswerAttemptCount)) return false;
            if (!m_failedPasswordAnswerAttemptWindowStart.Equals(other.m_failedPasswordAnswerAttemptWindowStart)) return false;

            return true;
        }
        public static Boolean operator ==(VLClientUserView o1, VLClientUserView o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLClientUserView o1, VLClientUserView o2)
        {
            return !(o1 == o2);
        }

        #endregion
    }
}
