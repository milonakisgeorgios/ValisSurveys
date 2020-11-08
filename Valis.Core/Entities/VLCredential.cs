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
    public sealed class VLCredential : VLObject
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_credentialId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_principal;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        PrincipalType m_principalType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_logOnToken;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pswdToken;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        VLPasswordFormat m_pswdFormat;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pswdSalt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pswdQuestion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pswdAnswer;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isApproved;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isLockedOut;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastLoginDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastPasswordChangedDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastLockoutDate;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_failedPasswordAttemptCount;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_failedPasswordAttemptWindowStart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_failedPasswordAnswerAttemptCount;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        public System.Int32 CredentialId
        {
            get { return this.m_credentialId; }
            internal set
            {
                if (this.m_credentialId == value)
                    return;

                this.m_credentialId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Principal
        {
            get { return this.m_principal; }
            set
            {
                if (this.m_principal == value)
                    return;

                this.m_principal = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public PrincipalType PrincipalType
        {
            get { return this.m_principalType; }
            set
            {
                if (this.m_principalType == value)
                    return;

                this.m_principalType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String LogOnToken
        {
            get { return this.m_logOnToken; }
            set
            {
                if (this.m_logOnToken == value)
                    return;

                this.m_logOnToken = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PswdToken
        {
            get { return this.m_pswdToken; }
            set
            {
                if (this.m_pswdToken == value)
                    return;

                this.m_pswdToken = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VLPasswordFormat PswdFormat
        {
            get { return this.m_pswdFormat; }
            set
            {
                if (this.m_pswdFormat == value)
                    return;

                this.m_pswdFormat = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PswdSalt
        {
            get { return this.m_pswdSalt; }
            set
            {
                if (this.m_pswdSalt == value)
                    return;

                this.m_pswdSalt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PswdQuestion
        {
            get { return this.m_pswdQuestion; }
            set
            {
                if (this.m_pswdQuestion == value)
                    return;

                this.m_pswdQuestion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PswdAnswer
        {
            get { return this.m_pswdAnswer; }
            set
            {
                if (this.m_pswdAnswer == value)
                    return;

                this.m_pswdAnswer = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsApproved
        {
            get { return this.m_isApproved; }
            set
            {
                if (this.m_isApproved == value)
                    return;

                this.m_isApproved = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsLockedOut
        {
            get { return this.m_isLockedOut; }
            set
            {
                if (this.m_isLockedOut == value)
                    return;

                this.m_isLockedOut = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastLoginDate
        {
            get { return this.m_lastLoginDate; }
            set
            {
                if (this.m_lastLoginDate == value)
                    return;

                this.m_lastLoginDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastPasswordChangedDate
        {
            get { return this.m_lastPasswordChangedDate; }
            set
            {
                if (this.m_lastPasswordChangedDate == value)
                    return;

                this.m_lastPasswordChangedDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastLockoutDate
        {
            get { return this.m_lastLockoutDate; }
            set
            {
                if (this.m_lastLockoutDate == value)
                    return;

                this.m_lastLockoutDate = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 FailedPasswordAttemptCount
        {
            get { return this.m_failedPasswordAttemptCount; }
            set
            {
                if (this.m_failedPasswordAttemptCount == value)
                    return;

                this.m_failedPasswordAttemptCount = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? FailedPasswordAttemptWindowStart
        {
            get { return this.m_failedPasswordAttemptWindowStart; }
            set
            {
                if (this.m_failedPasswordAttemptWindowStart == value)
                    return;

                this.m_failedPasswordAttemptWindowStart = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 FailedPasswordAnswerAttemptCount
        {
            get { return this.m_failedPasswordAnswerAttemptCount; }
            set
            {
                if (this.m_failedPasswordAnswerAttemptCount == value)
                    return;

                this.m_failedPasswordAnswerAttemptCount = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? FailedPasswordAnswerAttemptWindowStart
        {
            get { return this.m_failedPasswordAnswerAttemptWindowStart; }
            set
            {
                if (this.m_failedPasswordAnswerAttemptWindowStart == value)
                    return;

                this.m_failedPasswordAnswerAttemptWindowStart = value;
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

        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLCredential()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLCredential(DbDataReader reader)
            : base(reader)
        {
            this.CredentialId = reader.GetInt32(0);
            this.Principal = reader.GetInt32(1);
            this.PrincipalType = (PrincipalType)reader.GetByte(2);
            this.LogOnToken = reader.GetString(3);
            this.PswdToken = reader.GetString(4);
            this.PswdFormat = (VLPasswordFormat)reader.GetInt32(5);
            if (!reader.IsDBNull(6)) this.PswdSalt = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.PswdQuestion = reader.GetString(7);
            if (!reader.IsDBNull(8)) this.PswdAnswer = reader.GetString(8);
            this.IsApproved = reader.GetBoolean(9);
            this.IsLockedOut = reader.GetBoolean(10);
            if (!reader.IsDBNull(11)) this.LastLoginDate = reader.GetDateTime(11);
            if (!reader.IsDBNull(12)) this.LastPasswordChangedDate = reader.GetDateTime(12);
            if (!reader.IsDBNull(13)) this.LastLockoutDate = reader.GetDateTime(13);
            this.FailedPasswordAttemptCount = reader.GetInt32(14);
            if (!reader.IsDBNull(15)) this.FailedPasswordAttemptWindowStart = reader.GetDateTime(15);
            this.FailedPasswordAnswerAttemptCount = reader.GetInt32(16);
            if (!reader.IsDBNull(17)) this.FailedPasswordAnswerAttemptWindowStart = reader.GetDateTime(17);
            if (!reader.IsDBNull(18)) this.Comment = reader.GetString(18);


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
            return this.CredentialId.GetHashCode() ^
                this.Principal.GetHashCode() ^
                this.PrincipalType.GetHashCode() ^
                this.LogOnToken.GetHashCode() ^
                this.PswdToken.GetHashCode() ^
                this.PswdFormat.GetHashCode() ^
                ((this.PswdSalt == null) ? string.Empty : this.PswdSalt.ToString()).GetHashCode() ^
                ((this.PswdQuestion == null) ? string.Empty : this.PswdQuestion.ToString()).GetHashCode() ^
                ((this.PswdAnswer == null) ? string.Empty : this.PswdAnswer.ToString()).GetHashCode() ^
                this.IsApproved.GetHashCode() ^
                this.IsLockedOut.GetHashCode() ^
                ((this.LastLoginDate == null) ? string.Empty : this.LastLoginDate.ToString()).GetHashCode() ^
                ((this.LastPasswordChangedDate == null) ? string.Empty : this.LastPasswordChangedDate.ToString()).GetHashCode() ^
                ((this.LastLockoutDate == null) ? string.Empty : this.LastLockoutDate.ToString()).GetHashCode() ^
                this.FailedPasswordAttemptCount.GetHashCode() ^
                ((this.FailedPasswordAttemptWindowStart == null) ? string.Empty : this.FailedPasswordAttemptWindowStart.ToString()).GetHashCode() ^
                this.FailedPasswordAnswerAttemptCount.GetHashCode() ^
                ((this.FailedPasswordAnswerAttemptWindowStart == null) ? string.Empty : this.FailedPasswordAnswerAttemptWindowStart.ToString()).GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode();
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


            var other = (VLCredential)obj;

            //reference types
            if (!Object.Equals(LogOnToken, other.LogOnToken)) return false;
            if (!Object.Equals(PswdToken, other.PswdToken)) return false;
            if (!Object.Equals(PswdSalt, other.PswdSalt)) return false;
            if (!Object.Equals(PswdQuestion, other.PswdQuestion)) return false;
            if (!Object.Equals(PswdAnswer, other.PswdAnswer)) return false;
            if (!Object.Equals(Comment, other.Comment)) return false;
            //value types
            if (!CredentialId.Equals(other.CredentialId)) return false;
            if (!Principal.Equals(other.Principal)) return false;
            if (!PrincipalType.Equals(other.PrincipalType)) return false;
            if (!PswdFormat.Equals(other.PswdFormat)) return false;
            if (!IsApproved.Equals(other.IsApproved)) return false;
            if (!IsLockedOut.Equals(other.IsLockedOut)) return false;
            if (!LastLoginDate.Equals(other.LastLoginDate)) return false;
            if (!LastPasswordChangedDate.Equals(other.LastPasswordChangedDate)) return false;
            if (!LastLockoutDate.Equals(other.LastLockoutDate)) return false;
            if (!FailedPasswordAttemptCount.Equals(other.FailedPasswordAttemptCount)) return false;
            if (!FailedPasswordAttemptWindowStart.Equals(other.FailedPasswordAttemptWindowStart)) return false;
            if (!FailedPasswordAnswerAttemptCount.Equals(other.FailedPasswordAnswerAttemptCount)) return false;
            if (!FailedPasswordAnswerAttemptWindowStart.Equals(other.FailedPasswordAnswerAttemptWindowStart)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLCredential o1, VLCredential o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLCredential o1, VLCredential o2)
        {
            return !(o1 == o2);
        }

        #endregion


        #region DataContract serialization
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [OnSerializing()]
        private void OnBeginSerializing(StreamingContext context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [OnSerialized()]
        private void OnEndSerializing(StreamingContext context)
        {
        }
        /// <summary>
        /// Called before deserializing the type.
        /// </summary>
        [OnDeserializingAttribute]
        private void OnBeginDeserializing(StreamingContext context)
        {
            this._deserializing = true;
        }
        /// <summary>
        /// Called after deserializing the type.
        /// </summary>
        [OnDeserializedAttribute]
        private void OnEndDeserializing(StreamingContext context)
        {
            this._deserializing = false;
        }
        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            ValidateLogOnToken(ref m_logOnToken);
            ValidatePswdToken(ref m_pswdToken);
            if(this.PswdFormat == VLPasswordFormat.Clear)
            {
                Utility.CheckParameter(ref m_pswdSalt, false, false, false, 256, "PswdSalt");
            }
            else
            {
                Utility.CheckParameter(ref m_pswdSalt, true, true, false, 256, "PswdSalt");
            }
            Utility.CheckParameter(ref m_pswdQuestion, false, false, false, 256, "PswdQuestion");
            Utility.CheckParameter(ref m_pswdAnswer, false, false, false, 128, "PswdAnswer");
            Utility.CheckParameter(ref m_comment, false, false, false, 2048, "Comment");
        }

        internal static void ValidateLogOnToken(ref string logOnToken)
        {
            Utility.CheckParameter(ref logOnToken, true, true, false, 72, "LogOnToken");
        }
        internal static void ValidatePswdToken(ref string pswdToken)
        {
            Utility.CheckParameter(ref pswdToken, true, true, false, 256, "PswdToken");
        }

    }
}
