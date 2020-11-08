using System;
using System.ComponentModel;
using System.Data.Common;
using System.Xml.Serialization;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLSystemUserView : VLObject
    {
        #region class fields
        Int32 m_userId;
        Int16 m_defaultLanguageId;
        String m_defaultLanguageName;
        String m_firstName;
        String m_lastName;
        String m_email;
        String m_timeZoneId;
        Boolean m_isActive;
        Boolean m_isBuiltIn;
        Int32 m_attributeFlags;
        Int16 m_roleId;
        String m_roleName;
        String m_notes;
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
        String m_comment;
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
        public Int32 UserId
        {
            get { return m_userId; }
            internal set { m_userId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 DefaultLanguageId
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
        public String FirstName
        {
            get { return m_firstName; }
            internal set { m_firstName = value; }
        }
        public String LastName
        {
            get { return m_lastName; }
            internal set { m_lastName = value; }
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
        public String TimeZoneId
        {
            get { return this.m_timeZoneId; }
            internal set { this.m_timeZoneId = value; }
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
        public String Notes
        {
            get { return m_notes; }
            internal set { m_notes = value; }
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
        /// <summary>
        /// 
        /// </summary>
        public String Comment
        {
            get { return m_comment; }
            internal set { m_comment = value; }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSystemUserView(DbDataReader reader)
            : base(reader)
        {
            this.UserId = reader.GetInt32(0);
            this.DefaultLanguageId = reader.GetInt16(1);
            this.DefaultLanguageName = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.FirstName = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.LastName = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.Email = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.TimeZoneId = reader.GetString(6);
            this.IsActive = reader.GetBoolean(7);
            this.IsBuiltIn = reader.GetBoolean(8);
            this.AttributeFlags = reader.GetInt32(9);
            this.RoleId = reader.GetInt16(10);
            this.RoleName = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.Notes = reader.GetString(12);
            if (!reader.IsDBNull(13)) this.LastActivityDate = reader.GetDateTime(13);

            //CreationDT
            //CreatedBy
            //LastUpdateDT
            //LastUpdatedBy

            this.CredentialId = reader.GetInt32(18);
            this.LogOnToken = reader.GetString(19);
            this.PswdFormat = reader.GetInt32(20);
            this.IsApproved = reader.GetBoolean(21);
            this.IsLockedOut = reader.GetBoolean(22);
            if (!reader.IsDBNull(23)) this.LastLoginDate = reader.GetDateTime(23);
            if (!reader.IsDBNull(24)) this.LastPasswordChangedDate = reader.GetDateTime(24);
            if (!reader.IsDBNull(25)) this.LastLockoutDate = reader.GetDateTime(25);
            this.FailedPasswordAttemptCount = reader.GetInt32(26);
            if (!reader.IsDBNull(27)) this.FailedPasswordAttemptWindowStart = reader.GetDateTime(27);
            this.FailedPasswordAnswerAttemptCount = reader.GetInt32(28);
            if (!reader.IsDBNull(29)) this.FailedPasswordAnswerAttemptWindowStart = reader.GetDateTime(29);
            if (!reader.IsDBNull(30)) this.Comment = reader.GetString(30);
        }
        #endregion

    }
}
