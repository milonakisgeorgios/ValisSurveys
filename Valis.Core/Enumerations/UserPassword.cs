using System;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class UserPassword
    {
        public Int32 Principal { get; set; }
        public PrincipalType PrincipalType { get; set; }
        public bool IsBuiltin { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public string PswdFromDB { get; set; }
        public string PswdSalt { get; set; }
        public VLPasswordFormat PswdFormat { get; set; }
        public string IPAddress { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public int MaxInvalidPasswordAttempts { get; set; }
        public int PasswordAttemptWindow { get; set; }

        /// <summary>
        /// Τα permissions που διαθέτει την στιγμή του login ο συγκεκριμένος χρήστης
        /// </summary>
        public VLPermissions Permissions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal UserPassword()
        {
            LastLoginDate = DateTime.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal UserPassword(DbDataReader reader)
            : this()
        {
            this.Principal = reader.GetInt32(0);
            this.PrincipalType = (PrincipalType)reader.GetByte(1);
            if (!reader.IsDBNull(2)) this.IsBuiltin = reader.GetBoolean(2);
            if (!reader.IsDBNull(3)) this.IsActive = reader.GetBoolean(3);
            this.IsApproved = reader.GetBoolean(4);
            this.IsLockedOut = reader.GetBoolean(5);
            if (!reader.IsDBNull(6)) this.PswdFromDB = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.PswdSalt = reader.GetString(7);
            this.PswdFormat = (VLPasswordFormat)reader.GetInt32(8);
            if (!reader.IsDBNull(9)) this.IPAddress = reader.GetString(9);
            this.FailedPasswordAttemptCount = reader.GetInt32(10);
            this.FailedPasswordAnswerAttemptCount = reader.GetInt32(11);
            if (!reader.IsDBNull(12)) this.LastLoginDate = reader.GetDateTime(12);
            if (!reader.IsDBNull(13)) this.LastActivityDate = reader.GetDateTime(13);
            if (!reader.IsDBNull(14)) this.MaxInvalidPasswordAttempts = reader.GetInt32(14);
            if (!reader.IsDBNull(15)) this.PasswordAttemptWindow = reader.GetInt32(15);
            if (!reader.IsDBNull(16)) this.Permissions = (VLPermissions)reader.GetInt64(16);
        }
    }
}
