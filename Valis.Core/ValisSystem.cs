using System;
using System.Configuration;
using System.Globalization;
using Valis.Core.Configuration;
using Valis.Core.Dal;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ValisSystem
    {
        /// <summary>
        /// 
        /// </summary>
        public static string SectionName
        {
            get
            {
                return "valisSystem";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal readonly static ValisSection Settings = (ValisSection)ConfigurationManager.GetSection(ValisSystem.SectionName);

        /// <summary>
        /// 
        /// </summary>
        public static VsCoreConfiguration Core { get { if (Settings != null) return Settings.Core; else return null; } }
        /// <summary>
        /// 
        /// </summary>
        public static VsDaemonConfiguration Daemon { get { if (Settings != null) return Settings.Daemon; else return null; } }
        /// <summary>
        /// 
        /// </summary>
        public static VsManagerConfiguration Manager { get { if (Settings != null) return Settings.Manager; else return null; } }
        /// <summary>
        /// 
        /// </summary>
        public static VsServerConfiguration Server { get { if (Settings != null) return Settings.Server; else return null; } }
        /// <summary>
        /// 
        /// </summary>
        public static VsReporterConfiguration Reporter { get { if (Settings != null) return Settings.Reporter; else return null; } }



        #region support stuff
        SystemDaoBase m_SystemDaoBase;
        SystemDaoBase SystemDao
        {
            get
            {
                if (m_SystemDaoBase == null)
                {
                    m_SystemDaoBase = SystemDaoBase.GetInstance(SectionName, Core.Database);
                }
                return m_SystemDaoBase;
            }
        }


        #endregion

        /// <summary>
        /// 
        /// </summary>
        public ValisSystem()
        {
            if(Settings == null)
            {
                throw new ApplicationException("Invalid default configuration section (valisSystem)");
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="pswdToken"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public VLAccessToken LogOnUser(string logOnToken, string pswdToken, string ipaddress = null)
        {
            if (Utility.ValidateParameter(ref logOnToken, true, true, true, 72) == false)
                return null;
            if (Utility.ValidateParameter(ref pswdToken, true, true, false, 128) == false)
                return null;
            if (Utility.ValidateParameter(ref ipaddress, false, false, true, 50) == false)
                return null;


            var userPassword = GetPasswordWithFormat(logOnToken, pswdToken, true, true);
            if (userPassword == null)
                return null;


            DateTime dtNow = Utility.RoundToSeconds(DateTime.UtcNow);
            var accessToken = SystemDao.OpenAccessToken(userPassword.Principal, userPassword.PrincipalType, ipaddress, dtNow);
            if (accessToken == null)
                return null;

            accessToken.ValidateInstance();

            return accessToken;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        /// <param name="ipaddress"></param>
        /// <returns></returns>
        public VLAccessToken ValidateAccessToken(Int32 accessTokenId, string ipaddress = null)
        {
            if (Utility.ValidateParameter(ref ipaddress, false, false, true, 50) == false)
                return null;

            DateTime dtNow = Utility.RoundToSeconds(DateTime.UtcNow);
            var accessToken = SystemDao.ValidateAccessToken(accessTokenId, dtNow);
            if (accessToken != null)
            {

                accessToken.ValidateInstance();

            }
            return accessToken;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        public void LogOffUser(VLAccessToken accessToken)
        {
            if (accessToken == null) throw new ArgumentNullException("accessToken");

            LogOffUser(accessToken.AccessTokenId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        public void LogOffUser(Int32 accessTokenId)
        {
            SystemDao.CloseAccessToken(accessTokenId, Utility.RoundToSeconds(DateTime.UtcNow));
        }


        /// <summary>
        /// Αντίγραφο της μεθόδου υπάρχει και στην class VLSystemManager
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="password"></param>
        /// <param name="updateLastActivityDate">Εάν θα ανανεωθεί το πεδίο LastActivityDate στον πίνακα Principals</param>
        /// <param name="failIfNotApproved"></param>
        /// <returns></returns>
        UserPassword GetPasswordWithFormat(string logOnToken, string password, bool updateLastActivityDate = true, bool failIfNotApproved = true)
        {
            DateTime dtNow = Utility.RoundToSeconds(DateTime.UtcNow);
            var userPassword = SystemDao.GetPasswordWithFormat(logOnToken, updateLastActivityDate, dtNow);
            if (userPassword == null)
                return null;


            if (userPassword.IsLockedOut)
                return null;
            if (!userPassword.IsActive)
                return null;
            if (!userPassword.IsApproved && failIfNotApproved)
                return null;


            string encodedPasswd = Utility.EncodePassword(password, userPassword.PswdFormat, userPassword.PswdSalt);

            bool isPasswordCorrect = userPassword.PswdFromDB.Equals(encodedPasswd);


            if (isPasswordCorrect && userPassword.FailedPasswordAttemptCount == 0 && userPassword.FailedPasswordAnswerAttemptCount == 0)
                return userPassword;

            SystemDao.UpdateUserInfo(
                        logOnToken,
                        isPasswordCorrect,
                        updateLastActivityDate,
                        userPassword.MaxInvalidPasswordAttempts,
                        userPassword.PasswordAttemptWindow,
                        dtNow,
                        isPasswordCorrect ? dtNow : userPassword.LastLoginDate,
                        isPasswordCorrect ? dtNow : userPassword.LastActivityDate);

            if (isPasswordCorrect)
                return userPassword;

            return null;
        }





        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, '{1}'", Settings.SectionInformation.Name, Core.Database.ConnectionString);
        }
    }
}
