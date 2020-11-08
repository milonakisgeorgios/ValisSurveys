using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core.ViewModel
{
    /// <summary>
    /// αποτελεί μία ενισχυμένη παραλλαγή του VLAccessToken
    /// </summary>
    public class VLLogin
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_accessTokenId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_principalType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_principal;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_clientId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_language;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64 m_permissions;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime m_enterDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_leaveDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_iPAddress;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_timeZoneId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_isOK;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_isCleared;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_clientName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_firstName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_lastName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_email;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_role;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_logOnToken;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 LoginId
        {
            get { return m_accessTokenId; }
            internal set { m_accessTokenId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte PrincipalType
        {
            get { return m_principalType; }
            internal set { m_principalType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Principal
        {
            get { return m_principal; }
            internal set { m_principal = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? ClientId
        {
            get { return m_clientId; }
            internal set { m_clientId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 Language
        {
            get { return m_language; }
            internal set { m_language = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int64 Permissions
        {
            get { return m_permissions; }
            internal set { m_permissions = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EnterDt
        {
            get { return m_enterDt; }
            internal set { m_enterDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? LeaveDt
        {
            get { return m_leaveDt; }
            internal set { m_leaveDt = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String IPAddress
        {
            get { return m_iPAddress; }
            internal set { m_iPAddress = value; }
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
        public Byte IsOK
        {
            get { return m_isOK; }
            internal set { m_isOK = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte IsCleared
        {
            get { return m_isCleared; }
            internal set { m_isCleared = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String ClientName
        {
            get { return m_clientName; }
            internal set { m_clientName = value; }
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
        public String Email
        {
            get { return m_email; }
            internal set { m_email = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? Role
        {
            get { return m_role; }
            internal set { m_role = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String LogOnToken
        {
            get { return m_logOnToken; }
            internal set { m_logOnToken = value; }
        }
        #endregion
        
        
        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLLogin()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLogin(DbDataReader reader) 
        {
            this.LoginId = reader.GetInt32(0);
			this.PrincipalType = reader.GetByte(1);
			this.Principal = reader.GetInt32(2);
			if (!reader.IsDBNull(3)) this.ClientId = reader.GetInt32(3);
			this.Language = reader.GetInt16(4);
			this.Permissions = reader.GetInt64(5);
			this.EnterDt = reader.GetDateTime(6);
			if (!reader.IsDBNull(7)) this.LeaveDt = reader.GetDateTime(7);
			if (!reader.IsDBNull(8)) this.IPAddress = reader.GetString(8);
			this.TimeZoneId = reader.GetString(9);
			this.IsOK = reader.GetByte(10);
			this.IsCleared = reader.GetByte(11);
			if (!reader.IsDBNull(12)) this.ClientName = reader.GetString(12);
            if (!reader.IsDBNull(13)) this.FirstName = reader.GetString(13);
            if (!reader.IsDBNull(14)) this.LastName = reader.GetString(14);
			if (!reader.IsDBNull(15)) this.Email = reader.GetString(15);
			if (!reader.IsDBNull(16)) this.Role = reader.GetInt16(16);
			this.LogOnToken = reader.GetString(17);
        }
        #endregion


    }
}
