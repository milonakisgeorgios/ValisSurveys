using System;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLAccessToken : IAccessToken
    {
        #region inner state
        TimeZoneInfo m_tzi = null;
        #endregion

        public Int32 AccessTokenId { get; internal set; }
        public Int32 Principal { get; internal set; }
        public PrincipalType PrincipalType { get; internal set; }
        public Int32? ClientId { get; internal set; }
        public Int16 DefaultLanguage { get; internal set; }
        public VLPermissions Permissions { get; internal set; }
        public string LogOnToken { get; internal set; }
        public string Email { get; internal set; }
        public string FirstName { get; internal set; }
        public string LastName { get; internal set; }
        public Boolean IsBuiltIn { get; internal set; }
        public string TimeZoneId { get; internal set; }
        public Int32? Profile { get; internal set; }
        public Boolean? UseCredits { get; internal set; }
        public string ClientName { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLAccessToken(DbDataReader reader)
        {
            this.AccessTokenId = reader.GetInt32(0);
            this.Principal = reader.GetInt32(1);
            this.PrincipalType = (PrincipalType)reader.GetByte(2);
            if (!reader.IsDBNull(3)) this.ClientId = reader.GetInt32(3);
            this.DefaultLanguage = reader.GetInt16(4);
            this.Permissions = (VLPermissions)reader.GetInt64(5);
            this.LogOnToken = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.Email = reader.GetString(7);
            if (!reader.IsDBNull(8)) this.FirstName = reader.GetString(8);
            if (!reader.IsDBNull(9)) this.LastName = reader.GetString(9);
            this.IsBuiltIn = reader.GetBoolean(10);
            if (!reader.IsDBNull(11)) this.TimeZoneId = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.Profile = reader.GetInt32(12);
            if (!reader.IsDBNull(13)) this.UseCredits = reader.GetBoolean(13);
            if (!reader.IsDBNull(14)) this.ClientName = reader.GetString(14);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        internal VLAccessToken(VLAccessToken source)
        {
            this.AccessTokenId = source.AccessTokenId;
            this.Principal = source.Principal;
            this.PrincipalType = source.PrincipalType;
            this.ClientId = source.ClientId;
            this.DefaultLanguage = source.DefaultLanguage;
            this.Permissions = source.Permissions;
            this.LogOnToken = source.LogOnToken;
            this.Email = source.Email;
            this.FirstName = source.FirstName;
            this.LastName = source.LastName;
            this.IsBuiltIn = source.IsBuiltIn;
            this.TimeZoneId = source.TimeZoneId;
            this.Profile = source.Profile;
            this.UseCredits = source.UseCredits;
            this.ClientName = source.ClientName;
        }


        /// <summary>
        /// Gets a boolean value specifying whether the current user is a system administrator (sysadmin).
        /// </summary>
        public bool IsSysAdmin
        {
            get
            {
                if ((this.Permissions & VLPermissions.ManageSystem) == VLPermissions.ManageSystem)
                {
                    return true;
                }
                return false;
            }
        }



        /// <summary>
        /// Μετατρέπει μία UTC ώρα και ημερομηνία σε τοπική (για τον χρήστη μας) ώρα και ημερομηνία
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime ConvertTimeFromUtc(DateTime value)
        {
            if (m_tzi == null)
            {
                m_tzi = TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneId);
            }
            return TimeZoneInfo.ConvertTimeFromUtc(value, m_tzi);
        }
        /// <summary>
        /// Μετατρέπει μία τοπική (για τον χρήστη μας) ώρα και ημερομηνία σε UTC
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DateTime ConvertTimeToUtc(DateTime value)
        {
            if (m_tzi == null)
            {
                m_tzi = TimeZoneInfo.FindSystemTimeZoneById(this.TimeZoneId);
            }
            return TimeZoneInfo.ConvertTimeToUtc(value, m_tzi);
        }

        
        /// <summary>
        /// Ελέγχουμε εάν το VLAccessToken είναι σωστά αρχικοποιημένο
        /// </summary>
        internal void ValidateInstance()
        {

            if (this.Permissions == VLPermissions.None)
            {
                throw new VLException("Invalid VLAccessToken. Permissions are invalid!");
            }

        }

        /// <summary>
        /// Specifies whether this particular access token, has the required permissions.
        /// </summary>
        /// <param name="requiredPermissions"></param>
        /// <returns></returns>
        public bool HasPermissions(VLPermissions requiredPermissions)
        {
            return ((requiredPermissions & this.Permissions) == requiredPermissions);
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

            var other = (VLAccessToken)obj;


            //reference types
            if (!Object.Equals(this.LogOnToken, other.LogOnToken)) return false;
            if (!Object.Equals(this.Email, other.Email)) return false;
            if (!Object.Equals(this.FirstName, other.FirstName)) return false;
            if (!Object.Equals(this.LastName, other.LastName)) return false;
            //Value type
            if (!this.AccessTokenId.Equals(other.AccessTokenId)) return false;
            if (!this.Principal.Equals(other.Principal)) return false;
            if (!this.PrincipalType.Equals(other.PrincipalType)) return false;
            if (!this.Permissions.Equals(other.Permissions)) return false;
            if (!this.IsBuiltIn.Equals(other.IsBuiltIn)) return false;

            return true;
        }
    }
}
