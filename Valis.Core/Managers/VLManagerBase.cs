using System;
using System.Diagnostics;
using Valis.Core.Dal;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VLManagerBase : IAccessToken
    {
        #region inner state
        TimeZoneInfo m_tzi = null;
        #endregion


        /// <summary>
        /// This is the AccessTokenId of the user who owns this particular Manager.
        /// </summary>
        public Int32 AccessTokenId { get; internal set; }
        /// <summary>
        /// This is the UserId of the user whow owns this instance (Manager).
        /// </summary>
        public Int32 Principal { get; internal set; }
        /// <summary>
        /// The type (SystemUser, ClientUser) of the user who owns this instance (Manager).
        /// </summary>
        public PrincipalType PrincipalType { get; internal set; }
        /// <summary>
        /// The Client to which this account belongs (if this account is of type ClientUser)
        /// </summary>
        public Int32? ClientId { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public Int16 DefaultLanguage { get; internal set; }
        /// <summary>
        /// These are the permissions of the user who owns this instance (Manager)
        /// </summary>
        public VLPermissions Permissions { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string LogOnToken { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string Email { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string FirstName { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string LastName { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsBuiltIn { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string TimeZoneId { get; internal set; }
        /// <summary>
        /// The Client΄s profile, to which this account belongs (if this account is of type ClientUser)
        /// </summary>
        public Int32? Profile { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public Boolean? UseCredits { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        public string ClientName { get; internal set; }
        /// <summary>
        /// 
        /// </summary>
        internal string FileInventoryPath { get; set; }

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
        /// When a manager is being used by unit test code, this flag is true.
        /// </summary>
        internal Boolean IsUnderUnitTesting { get; set; }



        VLSystemManager m_System;
        internal VLSystemManager TheSystem
        {
            get
            {
                if (m_System == null)
                {
                    m_System = VLSystemManager.GetAnInstance(this);
                }
                return m_System;
            }
            set
            {
                m_System = value;
            }
        }


        #region Dals
        SystemDaoBase m_SystemDal = null;
        internal SystemDaoBase SystemDal
        {
            [DebuggerStepThrough]
            get
            {
                if (m_SystemDal == null)
                {
                    m_SystemDal = SystemDaoBase.GetInstance(ValisSystem.SectionName, ValisSystem.Core.Database);
                }
                return m_SystemDal;
            }
        }

        SurveysDaoBase m_SurveysDal = null;
        internal SurveysDaoBase SurveysDal
        {
            [DebuggerStepThrough]
            get
            {
                if (m_SurveysDal == null)
                {
                    m_SurveysDal = SurveysDaoBase.GetInstance(ValisSystem.SectionName, ValisSystem.Core.Database);
                }
                return m_SurveysDal;
            }
        }

        FilesDaoBase m_FilesDal = null;
        internal FilesDaoBase FilesDal
        {
            [DebuggerStepThrough]
            get
            {
                if (m_FilesDal == null)
                {
                    m_FilesDal = FilesDaoBase.GetInstance(ValisSystem.SectionName, ValisSystem.Core.Database);
                }
                return m_FilesDal;
            }
        }


        ViewModelDaoBase m_ViewModelDal = null;
        internal ViewModelDaoBase ViewModelDal
        {
            [DebuggerStepThrough]
            get
            {
                if (m_ViewModelDal == null)
                {
                    m_ViewModelDal = ViewModelDaoBase.GetInstance(ValisSystem.SectionName, ValisSystem.Core.Database);
                }
                return m_ViewModelDal;
            }
        }


        LibrariesDaoBase m_LibrariesDal = null;
        internal LibrariesDaoBase LibrariesDal
        {
            [DebuggerStepThrough]
            get
            {
                if (m_LibrariesDal == null)
                {
                    m_LibrariesDal = LibrariesDaoBase.GetInstance(ValisSystem.SectionName, ValisSystem.Core.Database);
                }
                return m_LibrariesDal;
            }
        }
        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        protected VLManagerBase(IAccessToken accessToken)
        {
            if (accessToken == null) throw new ArgumentNullException("accessToken");

            this.AccessTokenId = accessToken.AccessTokenId;
            this.Principal = accessToken.Principal;
            this.PrincipalType = accessToken.PrincipalType;
            this.ClientId = accessToken.ClientId;
            this.DefaultLanguage = accessToken.DefaultLanguage;
            this.Permissions = accessToken.Permissions;
            this.LogOnToken = accessToken.LogOnToken;
            this.Email = accessToken.Email;
            this.FirstName = accessToken.FirstName;
            this.LastName = accessToken.LastName;
            this.IsBuiltIn = accessToken.IsBuiltIn;
            this.TimeZoneId = accessToken.TimeZoneId;
            this.Profile = accessToken.Profile;
            this.UseCredits = accessToken.UseCredits;
            this.ClientName = accessToken.ClientName;

            this.FileInventoryPath = ValisSystem.Core.FileInventory.Path;
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
            if (value.Kind != DateTimeKind.Unspecified)
            {
                value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
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
            if (value.Kind != DateTimeKind.Unspecified)
            {
                value = new DateTime(value.Year, value.Month, value.Day, value.Hour, value.Minute, value.Second);
            }
            return TimeZoneInfo.ConvertTimeToUtc(value, m_tzi);
        }



        /// <summary>
        /// Enumerates the array of the required set of permissions, and checks whether the owner of this particular
        /// manager instance, has any of them. If yes it will return true, otherwise it will return false.
        /// </summary>
        /// <param name="requiredPermissions"></param>
        /// <returns></returns>
        protected bool ValidatePermissions(params VLPermissions[] requiredPermissions)
        {
            if (requiredPermissions == null) throw new ArgumentNullException("requiredPermissions");

            foreach (var perms in requiredPermissions)
            {
                if (HasPermissions(perms))
                {
                    return true; //Βρέθηκε τουλάχιστον ένα σύνολο permissions, επιστρέφουμε
                }
            }
            return false;
        }

        /// <summary>
        /// Enumerates the array of the required set of permissions, and checks whether the owner of this particular
        /// manager instance, has any of them. If yes it will return true, otherwise it will throw a VLAccessDeniedException.
        /// </summary>
        /// <param name="requiredPermissions"></param>
        protected void CheckPermissions(params VLPermissions[] requiredPermissions)
        {
            if (requiredPermissions == null) throw new ArgumentNullException("requiredPermissions");
            foreach (var perms in requiredPermissions)
            {
                if (HasPermissions(perms))
                {
                    return; //Βρέθηκε τουλάχιστον ένα σύνολο permissions, επιστρέφουμε
                }
            }
            throw new VLAccessDeniedException(requiredPermissions);
        }

        /// <summary>
        /// Specifies whether the owner of this particular manager instance. has the required permissions.
        /// The method returns true if the user has the permissions, otherwise it will return false.
        /// </summary>
        /// <param name="requiredPermissions"></param>
        /// <returns></returns>
        public bool HasPermissions(VLPermissions requiredPermissions)
        {
            return ((requiredPermissions & this.Permissions) == requiredPermissions);
        }



        /// <summary>
        /// Gets the full path of the specified managed file.
        /// <para>GetFilePath returns the entire path for the file (including the filename and the extension).</para>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFilePath(VLFile file)
        {
            return FilesDaoBase.GetFilePath(FileInventoryPath, file);
        }


        public static bool SupportOptions(QuestionType qtype)
        {
            switch (qtype)
            {
                case QuestionType.SingleLine:
                    return false;
                case QuestionType.MultipleLine:
                    return false;
                case QuestionType.Integer:
                    return false;
                case QuestionType.Decimal:
                    return false;
                case QuestionType.Date:
                    return false;
                case QuestionType.Time:
                    return false;
                case QuestionType.DateTime:
                    return false;
                case QuestionType.OneFromMany:
                    return true;
                case QuestionType.ManyFromMany:
                    return true;
                case QuestionType.DropDown:
                    return true;
                case QuestionType.DescriptiveText:
                    return false;
                case QuestionType.Slider:
                    return false;
                case QuestionType.MatrixOnePerRow:
                    return true;
                case QuestionType.MatrixManyPerRow:
                    return true;
                case QuestionType.MatrixManyPerRowCustom:
                    return true;
                case QuestionType.Composite:
                    return false;
                default:
                    return false;
            }
        }

        public static bool SupportColumns(QuestionType qtype)
        {
            switch (qtype)
            {
                case QuestionType.SingleLine:
                    return false;
                case QuestionType.MultipleLine:
                    return false;
                case QuestionType.Integer:
                    return false;
                case QuestionType.Decimal:
                    return false;
                case QuestionType.Date:
                    return false;
                case QuestionType.Time:
                    return false;
                case QuestionType.DateTime:
                    return false;
                case QuestionType.OneFromMany:
                    return false;
                case QuestionType.ManyFromMany:
                    return false;
                case QuestionType.DropDown:
                    return false;
                case QuestionType.DescriptiveText:
                    return false;
                case QuestionType.Slider:
                    return false;
                case QuestionType.MatrixOnePerRow:
                    return true;
                case QuestionType.MatrixManyPerRow:
                    return true;
                case QuestionType.MatrixManyPerRowCustom:
                    return true;
                case QuestionType.Composite:
                    return false;
                default:
                    return false;
            }
        }

    }
}
