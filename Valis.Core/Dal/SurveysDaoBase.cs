using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Globalization;
using System.Reflection;
using Valis.Core.Configuration;
using Valis.Core.ViewModel;

namespace Valis.Core.Dal
{
    internal abstract class SurveysDaoBase : DataAccess
    {
        #region Instance Factory
        /// <summary>
        /// 
        /// </summary>
        protected SurveysDaoBase() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="dbElement"></param>
        /// <returns></returns>
        public static SurveysDaoBase GetInstance(string configSectionName, DatabaseConfigurationElement dbElement)
        {
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentException("configSectionName is invalid");
            if (dbElement == null) throw new ArgumentNullException("dbElement");

            string key = configSectionName + "SurveysDaoBase";
            var m_instance = GetDalInstance(key);
            if (m_instance == null)
            {
                var ProviderAssembly = Assembly.Load(dbElement.AssemblyName);

                m_instance = (SurveysDaoBase)Activator.CreateInstance(ProviderAssembly.GetType(ProviderAssembly.GetName().Name + ".SurveysDao"));
                m_instance.ConnectionString = dbElement.ConnectionString;
                m_instance.ProviderFactory = dbElement.ProviderFactory;
                m_instance.AssemblyName = dbElement.AssemblyName;
                m_instance.ConfigSectionName = configSectionName;

                SetDalInstance(key, m_instance);
            }
            return (SurveysDaoBase)m_instance;
        }
        #endregion



        #region VLRuntimeSession
        public Collection<VLRuntimeSession> GetRuntimeSessions(Int32 accessToken, string whereClause = null, string orderByClause = "order by StartDt")
        {
            try
            {
                return GetRuntimeSessionsImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public VLRuntimeSession GetRuntimeSessionById(Int32 accessToken, Guid sessionId)
        {
            try
            {
                return GetRuntimeSessionByIdImpl(accessToken, sessionId);
            }
            catch
            {
                throw;
            }
        }
        public VLRuntimeSession GetRuntimeSessionByRecipientKeyAndCollector(Int32 accessToken, string recipientKey, Int32 collectorId)
        {
            if (string.IsNullOrWhiteSpace(recipientKey)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "recipientKey"));

            try
            {
                var whereClause = string.Format("where RecipientKey='{0}' and RequestType={1} and Collector = {2}", recipientKey, (byte)RuntimeRequestType.Collector_Email, collectorId);
                var items = GetRuntimeSessionsImpl(accessToken, whereClause, string.Empty);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(string.Format("There are multiple RuntimeSessions for the same RecipientKey '{0}', and Collector '{1}'!", recipientKey, collectorId));
            }
            catch
            {
                throw;
            }
        }
        public VLRuntimeSession GetRuntimeSessionByRecipientWebKeyAndCollector(Int32 accessToken, string recipientKey, Int32 collectorId)
        {
            if (string.IsNullOrWhiteSpace(recipientKey)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "recipientKey"));

            try
            {
                var whereClause = string.Format("where RecipientKey='{0}' and RequestType={1} and Collector = {2}", recipientKey, (byte)RuntimeRequestType.Collector_WebLink, collectorId);
                var items = GetRuntimeSessionsImpl(accessToken, whereClause, string.Empty);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(string.Format("There are multiple RuntimeSessions for the same RecipientKey '{0}', and Collector '{1}'!", recipientKey, collectorId));
            }
            catch
            {
                throw;
            }
        }
        public VLRuntimeSession CreateRuntimeSession(Int32 accessToken, VLRuntimeSession session, string userAgent)
        {
            try
            {
                return CreateRuntimeSessionImpl(accessToken, session, userAgent, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public void UpdateRuntimeSession(Int32 accessToken, VLRuntimeSession session)
        {
            try
            {
                UpdateRuntimeSessionImpl(accessToken, session);
            }
            catch
            {
                throw;
            }
        }
        public void DeleteRuntimeSession(Int32 accessToken, Guid sessionId)
        {
            try
            {
                DeleteRuntimeSessionImpl(accessToken, sessionId);
            }
            catch
            {
                throw;
            }
        }
        public void DeleteAllRuntimeSessions(Int32 accessToken)
        {
            try
            {
                DeleteAllRuntimeSessionsImpl(accessToken);
            }
            catch
            {
                throw;
            }
        }



        internal abstract Collection<VLRuntimeSession> GetRuntimeSessionsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract VLRuntimeSession GetRuntimeSessionByIdImpl(Int32 accessToken, Guid sessionId);
        internal abstract VLRuntimeSession CreateRuntimeSessionImpl(Int32 accessToken, VLRuntimeSession session, string userAgent, DateTime currentTimeUtc);
        internal abstract void UpdateRuntimeSessionImpl(Int32 accessToken, VLRuntimeSession session);
        internal abstract void DeleteRuntimeSessionImpl(Int32 accessToken, Guid sessionId);
        internal abstract void DeleteAllRuntimeSessionsImpl(Int32 accessToken);
        internal abstract VLRuntimeSession ChargePaymentForClickImpl(Int32 accessToken, Guid sessionId, Int32 collectorId, Int32 surveyId, DateTime currentTimeUtc);

        protected Collection<VLRuntimeSession> ExecuteAndGetRuntimeSessions(DbCommand cmd)
        {
            var collection = new Collection<VLRuntimeSession>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLRuntimeSession(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLRuntimeSession ExecuteAndGetRuntimeSession(DbCommand cmd)
        {
            VLRuntimeSession _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLRuntimeSession(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLSurvey
        /// <summary>
        /// Επιστρέφει όλα τα surveys, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveys(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetSurveysImpl(accessToken, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει τον συνολικό αριθμό των surveys, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public int GetSurveysCount(Int32 accessToken, string whereClause, short textsLanguage)
        {
            try
            {
                return GetSurveysCountImpl(accessToken, whereClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει όλα τα surveys ανα σελίδα, που είναι ορατά για τον χρήστη που κάνει την κλήση.
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveys(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSurveysPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Επιστρέφει όλα τα surveys, που είναι ορατά για τον συγκεκριμένο πελάτη.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveysForClient(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetSurveysForClientImpl(accessToken, clientId, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των surveys, που είναι ορατά για τον συγκεκριμένο πελάτη.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public int GetSurveysCountForClient(Int32 accessToken, Int32 clientId, string whereClause, short textsLanguage)
        {
            try
            {
                return GetSurveysCountForClientImpl(accessToken, clientId, whereClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει όλα τα surveys ανα σελίδα, που είναι ορατά για τον συγκεκριμένο πελάτη.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveysForClient(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSurveysPagedForClientImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Επιστρέφει το survey με το συγκεκριμένο id απο το σύστημα
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurvey GetSurveyById(Int32 accessToken, Int32 surveyId, short textsLanguage)
        {
            try
            {
                return GetSurveyByIdImpl(accessToken, surveyId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το survey με το συγκεκριμένο publicId απο το σύστημα
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="publicId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurvey GetSurveyByPublicId(Int32 accessToken, string publicId, short textsLanguage)
        {
            if (string.IsNullOrWhiteSpace(publicId)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "publicId"));
            
            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(PublicId)=N'{0}'", publicId.Replace("'", "''").ToUpperInvariant());
                var items = GetSurveysImpl(accessToken, whereClause, null, textsLanguage);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Surveys_more_than_one, "PublicId", publicId));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Εάν ο καλούντας είναι ένας SystemUser, τότε βλέπει όλα τα surveys του συστήματος.</para>
        /// <para>Εάν ο καλούντας είναι ένας clientUser, τότε βλέπει μόνο τα surveys του πελάτη στον οποίο ανήκει.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <returns></returns>
        public Collection<VLSurvey> GetSurveyVariantsById(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where SurveyId = {0}", surveyId);
                return GetSurveyVariantsImpl(accessToken, whereClause, null);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        public void DeleteSurvey(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                DeleteSurveyImpl(accessToken, surveyId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public VLSurvey UpdateSurvey(Int32 accessToken, VLSurvey survey)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            try
            {
                return UpdateSurveyImpl(accessToken, survey, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurvey CreateSurvey(Int32 accessToken, VLSurvey survey, short textsLanguage)
        {
            if (survey == null) throw new ArgumentNullException("survey");

            try
            {
                return CreateSurveyImpl(accessToken, survey, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public VLSurvey AddSurveyLanguage(Int32 accessToken, Int32 surveyId, Int16 sourceLanguage, Int16 targetLanguage)
        {
            try
            {
                return AddSurveyLanguageImpl(accessToken, surveyId, sourceLanguage, targetLanguage);
            }
            catch
            {
                throw;
            }
        }
        public VLSurvey RemoveSurveyLanguage(Int32 accessToken, Int32 surveyId, Int16 languageToBeDeleted)
        {
            try
            {
                return RemoveSurveyLanguageImpl(accessToken, surveyId, languageToBeDeleted);
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLSurvey> GetSurveysImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetSurveysCountImpl(Int32 accessToken, string whereClause, short textsLanguage);
        internal abstract Collection<VLSurvey> GetSurveysPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);

        internal abstract Collection<VLSurvey> GetSurveysForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetSurveysCountForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, short textsLanguage);
        internal abstract Collection<VLSurvey> GetSurveysPagedForClientImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);


        internal abstract Collection<VLSurvey> GetSurveyVariantsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract VLSurvey GetSurveyByIdImpl(Int32 accessToken, Int32 surveyId, short textsLanguage);
        internal abstract void DeleteSurveyImpl(Int32 accessToken, Int32 surveyId, DateTime currentTimeUtc);
        internal abstract void DestroySurveyImpl(Int32 accessToken, Int32 surveyId, DateTime currentTimeUtc);
        internal abstract VLSurvey UpdateSurveyImpl(Int32 accessToken, VLSurvey survey, DateTime currentTimeUtc);
        internal abstract VLSurvey CreateSurveyImpl(Int32 accessToken, VLSurvey survey, DateTime currentTimeUtc, short textsLanguage);
        internal abstract VLSurvey AddSurveyLanguageImpl(Int32 accessToken, Int32 surveyId, Int16 sourceLanguage, Int16 targetLanguage);
        internal abstract VLSurvey RemoveSurveyLanguageImpl(Int32 accessToken, Int32 surveyId, Int16 languageToBeDeleted);
        protected Collection<VLSurvey> ExecuteAndGetSurveys(DbCommand cmd)
        {
            var collection = new Collection<VLSurvey>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSurvey(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLSurvey ExecuteAndGetSurvey(DbCommand cmd)
        {
            VLSurvey _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSurvey(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLSurveyTheme


        public Collection<VLSurveyTheme> GetSurveyThemes(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetSurveyThemesImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetSurveyThemesCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetSurveyThemesCountImpl(accessToken, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLSurveyTheme> GetSurveyThemes(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSurveyThemesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLSurveyTheme GetSurveyThemeById(Int32 accessToken, Int32 themeId)
        {
            try
            {
                return GetSurveyThemeByIdImpl(accessToken, themeId);
            }
            catch
            {
                throw;
            }
        }


        public VLSurveyTheme GetSurveyThemeByName(Int32 accessToken, string name, Int32? clientId)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "name"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Name)=N'{0}'", name.Replace("'", "''").ToUpperInvariant());
                var items = GetSurveyThemesImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.Themes_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }
        public void DeleteSurveyTheme(Int32 accessToken, Int32 themeId)
        {
            try
            {
                DeleteSurveyThemeImpl(accessToken, themeId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLSurveyTheme UpdateSurveyTheme(Int32 accessToken, VLSurveyTheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");

            try
            {
                return UpdateSurveyThemeImpl(accessToken, theme, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLSurveyTheme CreateSurveyTheme(Int32 accessToken, VLSurveyTheme theme)
        {
            if (theme == null) throw new ArgumentNullException("theme");

            try
            {
                return CreateSurveyThemeImpl(accessToken, theme, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSurveyTheme> GetSurveyThemesImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetSurveyThemesCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLSurveyTheme> GetSurveyThemesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLSurveyTheme GetSurveyThemeByIdImpl(Int32 accessToken, Int32 themeId);
        internal abstract void DeleteSurveyThemeImpl(Int32 accessToken, Int32 themeId, DateTime currentTimeUtc);
        internal abstract VLSurveyTheme UpdateSurveyThemeImpl(Int32 accessToken, VLSurveyTheme theme, DateTime currentTimeUtc);
        internal abstract VLSurveyTheme CreateSurveyThemeImpl(Int32 accessToken, VLSurveyTheme theme, DateTime currentTimeUtc);
        protected Collection<VLSurveyTheme> ExecuteAndGetSurveyThemes(DbCommand cmd)
        {
            var collection = new Collection<VLSurveyTheme>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSurveyTheme(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLSurveyTheme ExecuteAndGetSurveyTheme(DbCommand cmd)
        {
            VLSurveyTheme _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSurveyTheme(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLSurveyPage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetSurveyPages(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage)
        {
            try
            {
                return GetSurveyPagesImpl(accessToken, surveyId, whereClause, "order by DisplayOrder", textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public int GetSurveyPagesCount(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage)
        {
            try
            {
                return GetSurveyPagesCountImpl(accessToken, surveyId, whereClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyPage> GetSurveyPages(Int32 accessToken, Int32 surveyId, int pageIndex, int pageSize, ref int totalRows, string whereClause, short textsLanguage)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSurveyPagesPagedImpl(accessToken, surveyId, startRowIndex, pageSize, ref totalRows, whereClause, "order by DisplayOrder", textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="pageId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyPage GetSurveyPageById(Int32 accessToken, Int32 surveyId, Int16 pageId, short textsLanguage)
        {
            try
            {
                return GetSurveyPageByIdImpl(accessToken, surveyId, pageId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyId"></param>
        /// <param name="pageId"></param>
        public void DeleteSurveyPage(Int32 accessToken, Int32 surveyId, Int16 pageId)
        {
            try
            {
                DeleteSurveyPageImpl(accessToken, surveyId, pageId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="surveyPage"></param>
        /// <returns></returns>
        public VLSurveyPage UpdateSurveyPage(Int32 accessToken, VLSurveyPage surveyPage)
        {
            if (surveyPage == null) throw new ArgumentNullException("surveyPage");

            try
            {
                return UpdateSurveyPageImpl(accessToken, surveyPage, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLSurveyPage CreateSurveyPage(Int32 accessToken, VLSurveyPage surveyPage, short textsLanguage, InsertPosition position, Int16? referingPageId)
        {
            if (surveyPage == null) throw new ArgumentNullException("surveyPage");

            try
            {
                return CreateSurveyPageImpl(accessToken, surveyPage, textsLanguage, position, referingPageId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSurveyPage> GetSurveyPagesImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetSurveyPagesCountImpl(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage);
        internal abstract Collection<VLSurveyPage> GetSurveyPagesPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);
        internal abstract VLSurveyPage GetSurveyPageByIdImpl(Int32 accessToken, Int32 surveyId, Int16 pageId, short textsLanguage);
        internal abstract void DeleteSurveyPageImpl(Int32 accessToken, Int32 survey, Int16 pageId, DateTime currentTimeUtc);
        internal abstract VLSurveyPage UpdateSurveyPageImpl(Int32 accessToken, VLSurveyPage surveyPage, DateTime currentTimeUtc);
        internal abstract VLSurveyPage CreateSurveyPageImpl(Int32 accessToken, VLSurveyPage surveyPage, short textsLanguage, InsertPosition position, Int16? referingPageId, DateTime currentTimeUtc);
        protected Collection<VLSurveyPage> ExecuteAndGetSurveyPages(DbCommand cmd)
        {
            var collection = new Collection<VLSurveyPage>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSurveyPage(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLSurveyPage ExecuteAndGetSurveyPage(DbCommand cmd)
        {
            VLSurveyPage _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSurveyPage(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLSurveyQuestion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForSurvey(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetQuestionsForSurveyImpl(accessToken, survey, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLSurveyQuestion> GetQuestionsForSurvey(Int32 accessToken, Int32 survey, Int16 page, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetQuestionsForSurveyImpl(accessToken, survey, page, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLSurveyQuestion> GetChildQuestions(Int32 accessToken, Int32 survey, Int16 masterQuestion, short textsLanguage)
        {
            try
            {
                return GetChildQuestionsImpl(accessToken, survey, masterQuestion, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <returns></returns>
        public int GetQuestionsForSurveyCount(Int32 accessToken, Int32 survey)
        {
            try
            {
                return GetQuestionsForSurveyCountImpl(accessToken, survey);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public int GetQuestionsForSurveyCount(Int32 accessToken, Int32 survey, Int16 page)
        {
            try
            {
                return GetQuestionsForSurveyCountImpl(accessToken, survey, page);
            }
            catch
            {
                throw;
            }
        }
        public int GetQuestionsForLibraryQuestionCount(Int32 accessToken, Int32 libraryQuestion)
        {
            try
            {
                return GetQuestionsForLibraryQuestionCountImpl(accessToken, libraryQuestion);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="questionId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyQuestion GetQuestionById(Int32 accessToken, Int32 survey, Int16 questionId, short textsLanguage)
        {
            try
            {
                return GetQuestionByIdImpl(accessToken, survey, questionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="questionId"></param>
        public void DeleteQuestion(Int32 accessToken, Int32 survey, Int16 questionId)
        {
            try
            {
                DeleteQuestionImpl(accessToken, survey, questionId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLSurveyQuestion UpdateSurveyQuestion(Int32 accessToken, VLSurveyQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");

            try
            {
                return UpdateSurveyQuestionImpl(accessToken, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLSurveyQuestion CreateSurveyQuestion(Int32 accessToken, VLSurveyQuestion question, short textsLanguage, InsertPosition position, Int16? referingQuestionId)
        {
            if (question == null) throw new ArgumentNullException("question");

            try
            {
                return CreateSurveyQuestionImpl(accessToken, question, textsLanguage, position, referingQuestionId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal VLSurveyQuestion AddLibraryQuestion(int accessToken, int surveyId, short pageId, int libraryQuestionId, short textsLanguage, InsertPosition position, short? referingQuestionId)
        {
            try
            {
                return AddLibraryQuestionImpl(accessToken, surveyId, pageId, libraryQuestionId, textsLanguage, position, referingQuestionId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSurveyQuestion> GetQuestionsForSurveyImpl(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage);
        internal abstract Collection<VLSurveyQuestion> GetQuestionsForSurveyImpl(Int32 accessToken, Int32 survey, Int16 page, string orderByClause, short textsLanguage);
        internal abstract Collection<VLSurveyQuestion> GetChildQuestionsImpl(Int32 accessToken, Int32 survey, Int16 masterQuestion, short textsLanguage);
        internal abstract int GetQuestionsForSurveyCountImpl(Int32 accessToken, Int32 survey);
        internal abstract int GetQuestionsForSurveyCountImpl(Int32 accessToken, Int32 survey, Int16 page);
        internal abstract int GetQuestionsForLibraryQuestionCountImpl(Int32 accessToken, Int32 libraryQuestion);
        internal abstract VLSurveyQuestion GetQuestionByIdImpl(Int32 accessToken, Int32 survey, Int16 questionId, short textsLanguage);
        internal abstract void DeleteQuestionImpl(Int32 accessToken, Int32 survey, Int16 questionId, DateTime currentTimeUtc);
        internal abstract VLSurveyQuestion UpdateSurveyQuestionImpl(Int32 accessToken, VLSurveyQuestion question, DateTime currentTimeUtc);
        internal abstract VLSurveyQuestion CreateSurveyQuestionImpl(Int32 accessToken, VLSurveyQuestion question, short textsLanguage, InsertPosition position, Int16? referingQuestionId, DateTime currentTimeUtc);
        internal abstract VLSurveyQuestion AddLibraryQuestionImpl(int accessToken, int surveyId, short pageId, int libraryQuestionId, short textsLanguage, InsertPosition position, short? referingQuestionId, DateTime currentTimeUtc);
        protected Collection<VLSurveyQuestion> ExecuteAndGetSurveyQuestions(DbCommand cmd)
        {
            var collection = new Collection<VLSurveyQuestion>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSurveyQuestion(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLSurveyQuestion ExecuteAndGetSurveyQuestion(DbCommand cmd)
        {
            VLSurveyQuestion _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSurveyQuestion(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }


        public Collection<VLSurveyQuestionEx> GetQuestionExsForSurvey(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetQuestionExsForSurveyImpl(accessToken, survey, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSurveyQuestionEx> GetQuestionExsForSurveyImpl(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage);
        protected Collection<VLSurveyQuestionEx> ExecuteAndGetSurveyQuestionExs(DbCommand cmd)
        {
            var questions = new Collection<VLSurveyQuestionEx>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε τις ερωτήσεις:*/
                    while (reader.Read())
                    {
                        var _object = new VLSurveyQuestionEx(reader);
                        questions.Add(_object);
                    }

                    /*Στο δεύτερο result set έχουμε τα Options:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var option = new VLQuestionOption(reader);

                            foreach(var q in questions)
                            {
                                if(option.Question == q.QuestionId)
                                {
                                    q.Options.Add(option);
                                    break;
                                }
                            }
                        }
                    }

                    /*Στο τρίτο result set έχουμε τα Columns:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var column = new VLQuestionColumn(reader);

                            foreach (var q in questions)
                            {
                                if(column.Question == q.QuestionId)
                                {
                                    q.Columns.Add(column);
                                    break;
                                }
                            }
                        }
                    }

                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return questions;
        }
        #endregion

        #region VLQuestionOption
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLQuestionOption> GetQuestionOptions(Int32 accessToken, Int32 survey, short textsLanguage)
        {
            try
            {
                return GetQuestionOptionsExImpl(accessToken, survey, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει τα options της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLQuestionOption> GetQuestionOptions(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage)
        {
            try
            {
                return GetQuestionOptionsImpl(accessToken, survey, question, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public int GetQuestionOptionsCount(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                return GetQuestionOptionsCountImpl(accessToken, survey, question);
            }
            catch
            {
                throw;
            }
        }

        public VLQuestionOption GetQuestionOptionById(Int32 accessToken, Int32 survey, Int16 question, Byte optionId, short textsLanguage)
        {
            try
            {
                return GetQuestionOptionByIdImpl(accessToken, survey, question, optionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Διαγράφει το συγκεκριμένο option απο το σύστημα.
        /// <para>Η διαγραφή αφορά όλες τις (τυχόν) μεταφράσεις του option.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="optionId"></param>
        public void DeleteQuestionOption(Int32 accessToken, Int32 survey, Int16 question, Byte optionId)
        {
            try
            {
                DeleteQuestionOptionImpl(accessToken, survey, question, optionId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Διαγράφει όλα τα options της συγκεκριμένης ερώτησης.
        /// <para>Η διαγραφή αφορά όλες τις (τυχόν) μεταφράσεις των options.</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        public void DeleteAllQuestionOptions(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                DeleteAllQuestionOptionsImpl(accessToken, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLQuestionOption UpdateQuestionOption(Int32 accessToken, VLQuestionOption option)
        {
            if (option == null) throw new ArgumentNullException("option");

            try
            {
                return UpdateQuestionOptionImpl(accessToken, option, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLQuestionOption CreateQuestionOption(Int32 accessToken, VLQuestionOption option, short textsLanguage)
        {
            if (option == null) throw new ArgumentNullException("option");

            try
            {
                return CreateQuestionOptionImpl(accessToken, option, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLQuestionOption> GetQuestionOptionsExImpl(Int32 accessToken, Int32 survey, short textsLanguage);
        internal abstract Collection<VLQuestionOption> GetQuestionOptionsImpl(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage);
        internal abstract int GetQuestionOptionsCountImpl(Int32 accessToken, Int32 survey, Int16 question);
        internal abstract VLQuestionOption GetQuestionOptionByIdImpl(Int32 accessToken, Int32 survey, Int16 question, Byte optionId, short textsLanguage);
        internal abstract void DeleteQuestionOptionImpl(Int32 accessToken, Int32 survey, Int16 question, Byte optionId, DateTime currentTimeUtc);
        internal abstract void DeleteAllQuestionOptionsImpl(Int32 accessToken, Int32 survey, Int16 question, DateTime currentTimeUtc);
        internal abstract VLQuestionOption UpdateQuestionOptionImpl(Int32 accessToken, VLQuestionOption option, DateTime currentTimeUtc);
        internal abstract VLQuestionOption CreateQuestionOptionImpl(Int32 accessToken, VLQuestionOption option, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLQuestionOption> ExecuteAndGetQuestionOptions(DbCommand cmd)
        {
            var collection = new Collection<VLQuestionOption>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLQuestionOption(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLQuestionOption ExecuteAndGetQuestionOption(DbCommand cmd)
        {
            VLQuestionOption _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLQuestionOption(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLQuestionColumn
        /// <summary>
        /// Επιστρέφει τα columns της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLQuestionColumn> GetQuestionColumns(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage)
        {
            try
            {
                return GetQuestionColumnsImpl(accessToken, survey, question, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public int GetQuestionColumnsCount(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                return GetQuestionColumnsCountImpl(accessToken, survey, question);
            }
            catch
            {
                throw;
            }
        }

        public VLQuestionColumn GetQuestionColumnById(Int32 accessToken, Int32 survey, Int16 question, Byte columnId, short textsLanguage)
        {
            try
            {
                return GetQuestionColumnByIdImpl(accessToken, survey, question, columnId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteQuestionColumn(Int32 accessToken, Int32 survey, Int16 question, Byte columnId)
        {
            try
            {
                DeleteQuestionColumnImpl(accessToken, survey, question, columnId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAllQuestionColumns(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                DeleteAllQuestionColumnsImpl(accessToken, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLQuestionColumn UpdateQuestionColumn(Int32 accessToken, VLQuestionColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");

            try
            {
                return UpdateQuestionColumnImpl(accessToken, column, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLQuestionColumn CreateQuestionColumn(Int32 accessToken, VLQuestionColumn column, short textsLanguage)
        {
            if (column == null) throw new ArgumentNullException("column");

            try
            {
                return CreateQuestionColumnImpl(accessToken, column, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLQuestionColumn> GetQuestionColumnsImpl(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage);
        internal abstract int GetQuestionColumnsCountImpl(Int32 accessToken, Int32 survey, Int16 question);
        internal abstract VLQuestionColumn GetQuestionColumnByIdImpl(Int32 accessToken, Int32 survey, Int16 question, Byte columnId, short textsLanguage);
        internal abstract void DeleteQuestionColumnImpl(Int32 accessToken, Int32 survey, Int16 question, Byte columnId, DateTime currentTimeUtc);
        internal abstract void DeleteAllQuestionColumnsImpl(Int32 accessToken, Int32 survey, Int16 question, DateTime currentTimeUtc);
        internal abstract VLQuestionColumn UpdateQuestionColumnImpl(Int32 accessToken, VLQuestionColumn column, DateTime currentTimeUtc);
        internal abstract VLQuestionColumn CreateQuestionColumnImpl(Int32 accessToken, VLQuestionColumn column, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLQuestionColumn> ExecuteAndGetQuestionColumns(DbCommand cmd)
        {
            var collection = new Collection<VLQuestionColumn>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLQuestionColumn(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLQuestionColumn ExecuteAndGetQuestionColumn(DbCommand cmd)
        {
            VLQuestionColumn _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLQuestionColumn(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLCollector
        public Collection<VLCollector> GetCollectors(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                return GetCollectorsImpl(accessToken, surveyId, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public int GetCollectorsCount(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage)
        {
            try
            {
                return GetCollectorsCountImpl(accessToken, surveyId, whereClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το σύνολο των collectors που διαθέτει ο συγκεκριμένος πελάτης, ανεξαρτήτως του σε ποιό survey ανήκουν
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        internal int GetCollectorsCountForClient(Int32 accessToken, Int32 clientId, string whereClause=null)
        {
            try
            {
                return GetCollectorsCountForClientImpl(accessToken, clientId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLCollector> GetCollectors(Int32 accessToken, Int32 surveyId, int pageIndex, int pageSize, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetCollectorsPagedImpl(accessToken, surveyId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public VLCollector GetCollectorById(Int32 accessToken, Int32 collectorId, short textsLanguage)
        {
            try
            {
                return GetCollectorByIdImpl(accessToken, collectorId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCollector> GetCollectorVariantsById(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where CollectorId = {0}", collectorId);
                return GetCollectorVariantsImpl(accessToken, whereClause, null);
            }
            catch
            {
                throw;
            }
        }

        public VLCollector GetCollectorByWebLink(Int32 accessToken, Int32 surveyId, string webLink, short textsLanguage)
        {
            if (string.IsNullOrWhiteSpace(webLink)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "webLink"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where WebLink=N'{0}'", webLink.Replace("'", "''"));
                var items = GetCollectorsImpl(accessToken, surveyId, whereClause, null, textsLanguage);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Collectors_more_than_one, "webLink", webLink));
            }
            catch
            {
                throw;
            }
        }
        public VLCollector GetCollectorByWebLink(Int32 accessToken, string webLink, short textsLanguage)
        {
            if (string.IsNullOrWhiteSpace(webLink)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "webLink"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where WebLink=N'{0}'", webLink.Replace("'", "''"));
                var items = GetCollectorsImpl(accessToken, whereClause, null, textsLanguage);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Collectors_more_than_one, "webLink", webLink));
            }
            catch
            {
                throw;
            }
        }

        public VLCollector GetCollectorByName(Int32 accessToken, Int32 surveyId, string name, short textsLanguage)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "name"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Name)=N'{0}'", name.Replace("'", "''").ToUpperInvariant());
                var items = GetCollectorsImpl(accessToken, surveyId, whereClause, null, textsLanguage);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Surveys_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCollector(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                DeleteCollectorImpl(accessToken, collectorId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLCollector UpdateCollector(Int32 accessToken, VLCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return UpdateCollectorImpl(accessToken, collector, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLCollector CreateCollector(Int32 accessToken, VLCollector collector, short textsLanguage)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return CreateCollectorImpl(accessToken, collector, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLCollector> GetCollectorsImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause, short textsLanguage);
        internal abstract Collection<VLCollector> GetCollectorsImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetCollectorsCountImpl(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage);
        internal abstract int GetCollectorsCountForClientImpl(Int32 accessToken, Int32 clientId, string whereClause);
        internal abstract Collection<VLCollector> GetCollectorsPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);
        internal abstract Collection<VLCollector> GetCollectorVariantsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract VLCollector GetCollectorByIdImpl(Int32 accessToken, Int32 collectorId, short textsLanguage);
        internal abstract void DeleteCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc);
        internal abstract VLCollector UpdateCollectorImpl(Int32 accessToken, VLCollector collector, DateTime currentTimeUtc);
        internal abstract VLCollector CreateCollectorImpl(Int32 accessToken, VLCollector collector, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLCollector> ExecuteAndGetCollectors(DbCommand cmd)
        {
            var collection = new Collection<VLCollector>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCollector(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLCollector ExecuteAndGetCollector(DbCommand cmd)
        {
            VLCollector _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLCollector(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }



        public Collection<VLCollectorPeek> GetCollectorPeeks(Int32 accessToken, Int32 clientId, CollectorType ctype)
        {
            try
            {
                string whereClause = string.Format("where Client = {0} and CollectorType = {1}", clientId, (Byte)ctype);

                return GetCollectorPeeksImpl(accessToken, whereClause, "order by SurveyName, CollectorName");
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLCollectorPeek> GetCollectorPeeksImpl(Int32 accessToken, string whereClause, string orderByClause);
        protected Collection<VLCollectorPeek> ExecuteAndGetCollectorPeeks(DbCommand cmd)
        {
            var collection = new Collection<VLCollectorPeek>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCollectorPeek(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLCollectorPeek ExecuteAndGetCollectorPeek(DbCommand cmd)
        {
            VLCollectorPeek _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLCollectorPeek(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLMessage

        public Collection<VLMessage> GetMessages(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause)
        {
            try
            {
                return GetMessagesImpl(accessToken, collectorId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetMessagesCount(Int32 accessToken, Int32 collectorId, string whereClause)
        {
            try
            {
                return GetMessagesCountImpl(accessToken, collectorId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLMessage> GetMessages(Int32 accessToken, Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause, string orderByClause)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetMessagesPagedImpl(accessToken, collectorId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLMessage GetMessageById(Int32 accessToken, Int32 messageId)
        {
            try
            {
                return GetMessageByIdImpl(accessToken, messageId);
            }
            catch
            {
                throw;
            }
        }

        public VLMessage GetMessageBySenderVerificationCode(Int32 accessToken, string code)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "SenderVerificationCode"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where SenderVerificationCode=N'{0}'", code.Replace("'", "''"));
                var items = GetMessagesExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Messages_more_than_one, "SenderVerificationCode", code));
            }
            catch
            {
                throw;
            }
        }
        public void DeleteMessage(Int32 accessToken, Int32 messageId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteMessageImpl(accessToken, messageId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLMessage UpdateMessage(Int32 accessToken, VLMessage collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return UpdateMessageImpl(accessToken, collector, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Ακυρώνει το χρονοπρογραμματισμό του Message, και το γυρνάει σε status Draft.
        /// <para>Για να επιτύχει αυτό πρέπει το SentCounter του message να είναι μηδενικό (0), και το message να βρίσκεται σε ένα απο τα
        /// παρακάτω status: Pending, PreparingError, ExecutingError, ExecutedWithErrors, Cancel</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLMessage UnScheduleMessage(Int32 accessToken, VLMessage collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return UnScheduleMessageImpl(accessToken, collector, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLMessage CreateMessage(Int32 accessToken, VLMessage collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return CreateMessageImpl(accessToken, collector, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLMessage GetNextPendingMessage(Int32 accessToken, DateTime scheduleDt, Int32 minuteOffset)
        {
            try
            {
                return GetNextPendingMessageImpl(accessToken, scheduleDt, minuteOffset, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLMessage GetNextPreparedMessage(Int32 accessToken, DateTime scheduleDt)
        {
            try
            {
                return GetNextPreparedMessageImpl(accessToken, scheduleDt, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLMessage> GetMessagesExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLMessage> GetMessagesImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause);
        internal abstract int GetMessagesCountImpl(Int32 accessToken, Int32 collectorId, string whereClause);
        internal abstract Collection<VLMessage> GetMessagesPagedImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLMessage GetMessageByIdImpl(Int32 accessToken, Int32 messageId);
        internal abstract void DeleteMessageImpl(Int32 accessToken, Int32 messageId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLMessage UpdateMessageImpl(Int32 accessToken, VLMessage collector, DateTime currentTimeUtc);
        internal abstract VLMessage UnScheduleMessageImpl(Int32 accessToken, VLMessage collector, DateTime currentTimeUtc);
        internal abstract VLMessage CreateMessageImpl(Int32 accessToken, VLMessage collector, DateTime currentTimeUtc);
        internal abstract VLMessage GetNextPendingMessageImpl(Int32 accessToken, DateTime scheduleDt, int minuteOffset, DateTime currentTimeUtc);
        internal abstract VLMessage GetNextPreparedMessageImpl(Int32 accessToken, DateTime scheduleDt, DateTime currentTimeUtc);

        protected Collection<VLMessage> ExecuteAndGetMessages(DbCommand cmd)
        {
            var collection = new Collection<VLMessage>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLMessage(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLMessage ExecuteAndGetMessage(DbCommand cmd)
        {
            VLMessage _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLMessage(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLRecipient

        public Collection<VLRecipient> GetRecipients(Int32 accessToken, Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetRecipientsImpl(accessToken, collectorId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        internal Collection<VLRecipient> GetRecipientsEx(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetRecipientsExImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLRecipient> GetRecipientsForSurveyByEmail(Int32 accessToken, Int32 surveyId, string email)
        {
            VLRecipient.ValidateEmail(ref email);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where Collector in (select CollectorId from [dbo].[SurveyCollectors] where Survey = {0}) and upper(Email)=N'{1}'", surveyId, email.Replace("'", "''").ToUpperInvariant());

                return GetRecipientsExImpl(accessToken, whereClause, "order by Collector, Email");
            }
            catch
            {
                throw;
            }
        }

        public int GetRecipientsCount(Int32 accessToken, Int32 collectorId, string whereClause = null)
        {
            try
            {
                return GetRecipientsCountImpl(accessToken,collectorId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLRecipient> GetRecipients(Int32 accessToken, Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetRecipientsPagedImpl(accessToken, collectorId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLRecipient GetRecipientById(Int32 accessToken, Int64 recipientId)
        {
            try
            {
                return GetRecipientByIdImpl(accessToken, recipientId);
            }
            catch
            {
                throw;
            }
        }

        public VLRecipient GetRecipientByEmail(Int32 accessToken, Int32 collectorId, string email)
        {
            VLRecipient.ValidateEmail(ref email);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Email)=N'{0}'", email.Replace("'", "''").ToUpperInvariant());
                var items = GetRecipientsImpl(accessToken, collectorId, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.Recipients_more_than_one, "Email", email));
            }
            catch
            {
                throw;
            }

        }


        public VLRecipient GetRecipientByKey(Int32 accessToken, Int32 collectorId, string recipientKey)
        {
            if (string.IsNullOrWhiteSpace(recipientKey)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "recipientKey"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where RecipientKey=N'{0}'", recipientKey.Replace("'", "''"));
                var items = GetRecipientsImpl(accessToken, collectorId, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Recipients_more_than_one, "RecipientKey", recipientKey));
            }
            catch
            {
                throw;
            }
        }
        public VLRecipient GetRecipientByKey(Int32 accessToken, string recipientKey)
        {
            if (string.IsNullOrWhiteSpace(recipientKey)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "recipientKey"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where RecipientKey=N'{0}'", recipientKey.Replace("'", "''"));
                var items = GetRecipientsExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.Recipients_more_than_one, "RecipientKey", recipientKey));
            }
            catch
            {
                throw;
            }
        }




        public void DeleteRecipient(Int32 accessToken, Int64 recipientId)
        {
            try
            {
                DeleteRecipientImpl(accessToken, recipientId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveAllUnsentRecipientsFromCollector(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                return RemoveAllUnsentRecipientsFromCollectorImpl(accessToken, collectorId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveAllOptedOutRecipientsFromCollector(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                return RemoveAllOptedOutRecipientsFromCollectorImpl(accessToken, collectorId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveAllBouncedRecipientsFromCollector(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                return RemoveAllBouncedRecipientsFromCollectorImpl(accessToken, collectorId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveByDomainRecipientsFromCollector(Int32 accessToken, Int32 collectorId, string domainName)
        {
            try
            {
                return RemoveByDomainRecipientsFromCollectorImpl(accessToken, collectorId, domainName, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }



        public VLRecipient UpdateRecipient(Int32 accessToken, VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");

            try
            {
                return UpdateRecipientImpl(accessToken, recipient, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLRecipient CreateRecipient(Int32 accessToken, VLRecipient recipient)
        {
            if (recipient == null) throw new ArgumentNullException("recipient");

            try
            {
                return CreateRecipientImpl(accessToken, recipient, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public void ImportRecipients(Int32 callerPrincipalId, VLRecipient[] recipients, int length, ref Int32 successImports, ref Int32 failedImports)
        {
            try
            {
                ImportRecipientsImpl(callerPrincipalId, recipients, length, Utility.UtcNow(), ref successImports, ref failedImports);
            }
            catch
            {
                throw;
            }
        }
        public void ImportRecipientsFinalize(Int32 callerPrincipalId, Int32 collectorId, ref Int32 optedOutRecipients, ref Int32 bouncedRecipients, ref Int32 totalRecipients)
        {
            try
            {
                ImportRecipientsFinalizeImpl(callerPrincipalId, collectorId, Utility.UtcNow(), ref optedOutRecipients, ref bouncedRecipients, ref totalRecipients);
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLRecipient> GetRecipientsImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause);
        internal abstract Collection<VLRecipient> GetRecipientsExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetRecipientsCountImpl(Int32 accessToken, Int32 collectorId, string whereClause);
        internal abstract Collection<VLRecipient> GetRecipientsPagedImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLRecipient GetRecipientByIdImpl(Int32 accessToken, Int64 recipientId);
        internal abstract void DeleteRecipientImpl(Int32 accessToken, Int64 recipientId, DateTime currentTimeUtc);
        internal abstract int RemoveAllUnsentRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc);
        internal abstract int RemoveAllOptedOutRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc);
        internal abstract int RemoveAllBouncedRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc);
        internal abstract int RemoveByDomainRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, string domainName, DateTime currentTimeUtc);
        

        internal abstract VLRecipient UpdateRecipientImpl(Int32 accessToken, VLRecipient recipient, DateTime currentTimeUtc);
        internal abstract VLRecipient CreateRecipientImpl(Int32 accessToken, VLRecipient recipient, DateTime currentTimeUtc);
        internal abstract void ImportRecipientsImpl(Int32 callerPrincipalId, VLRecipient[] recipient, int length, DateTime currentTimeUtc, ref Int32 successImports, ref Int32 failedImports);
        internal abstract void ImportRecipientsFinalizeImpl(Int32 callerPrincipalId, Int32 collectorId, DateTime currentTimeUtc, ref Int32 optedOutRecipients, ref Int32 bouncedRecipients, ref Int32 totalRecipients);
        
        protected Collection<VLRecipient> ExecuteAndGetRecipients(DbCommand cmd)
        {
            var collection = new Collection<VLRecipient>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLRecipient(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLRecipient ExecuteAndGetrRecipient(DbCommand cmd)
        {
            VLRecipient _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLRecipient(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLMessageRecipient

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLMessageRecipient> GetMessageRecipients(Int32 accessToken, Int32 messageId, string whereClause)
        {
            try
            {
                return GetMessageRecipientsImpl(accessToken, messageId, whereClause, "order by Message,Recipient ");
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLMessageRecipient> GetMessageRecipientsForRecipient(Int32 accessToken, Int64 recipientId)
        {
            try
            {
                string whereClause = string.Format("where Recipient='{0}'", recipientId);

                return GetMessageRecipientsExImpl(accessToken, whereClause, "order by Message,Recipient ");
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetMessageRecipientsCount(Int32 accessToken, Int32 messageId, string whereClause)
        {
            try
            {
                return GetMessageRecipientsCountImpl(accessToken, messageId, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLMessageRecipient> GetMessageRecipients(Int32 accessToken, Int32 messageId, int pageIndex, int pageSize, ref int totalRows, string whereClause)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetMessageRecipientsPagedImpl(accessToken, messageId, startRowIndex, pageSize, ref totalRows, whereClause, "order by Message,Recipient ");
            }
            catch
            {
                throw;
            }
        }
        

        
        public VLMessageRecipient GetMessageRecipientById(Int32 accessToken, Int32 messageId, Int64 recipientId)
        {
            try
            {
                return GetMessageRecipientByIdImpl(accessToken, messageId, recipientId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="messageRecipient"></param>
        /// <returns></returns>
        public VLMessageRecipient UpdateMessageRecipient(Int32 accessToken, VLMessageRecipient messageRecipient)
        {
            if (messageRecipient == null) throw new ArgumentNullException("messageRecipient");

            try
            {
                return UpdateMessageRecipientImpl(accessToken, messageRecipient, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLMessageRecipient> GetMessageRecipientsExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLMessageRecipient> GetMessageRecipientsImpl(Int32 accessToken, Int32 message, string whereClause, string orderByClause);
        internal abstract int GetMessageRecipientsCountImpl(Int32 accessToken, Int32 message, string whereClause);
        internal abstract Collection<VLMessageRecipient> GetMessageRecipientsPagedImpl(Int32 accessToken, Int32 message, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLMessageRecipient GetMessageRecipientByIdImpl(Int32 accessToken, Int32 message, Int64 recipient);
        internal abstract VLMessageRecipient UpdateMessageRecipientImpl(Int32 accessToken, VLMessageRecipient messageRecipient, DateTime currentTimeUtc);
        protected Collection<VLMessageRecipient> ExecuteAndGetMessageRecipients(DbCommand cmd)
        {
            var collection = new Collection<VLMessageRecipient>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLMessageRecipient(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLMessageRecipient ExecuteAndGetMessageRecipient(DbCommand cmd)
        {
            VLMessageRecipient _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLMessageRecipient(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }



        internal Int32 PrepareMessageRecipients(Int32 accessToken, Int32 collectorId, Int32 messageId)
        {
            try
            {
                return PrepareMessageRecipientsImpl(accessToken, collectorId, messageId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal void UnPrepareMessageRecipients(Int32 accessToken, Int32 collectorId, Int32 messageId)
        {
            try
            {
                UnPrepareMessageRecipientsImpl(accessToken, collectorId, messageId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Int32 PrepareMessageRecipientsImpl(Int32 accessToken, Int32 collectorId, Int32 messageId, DateTime currentTimeUtc);
        internal abstract void UnPrepareMessageRecipientsImpl(Int32 accessToken, Int32 collectorId, Int32 messageId, DateTime currentTimeUtc);
        #endregion

        #region VLResponse
        /// <summary>
        /// Επιστρέφει τα responses που μπορεί να δεί ένας Πελάτης, για συγκεκριμένο survey. Το τι μπορεί να δεί εξαρτάται απο το Profile του, και τις τυχών πληρωμές του.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetPaidResponses(Int32 accessToken, Int32 survey, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetPaidResponsesImpl(accessToken, survey, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το σύνολο των responses που μπορεί να δεί ένας Πελάτης, για συγκεκριμένο survey. Το τι μπορεί να δεί εξαρτάται απο το Profile του, και τις τυχών πληρωμές του.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public Int32 GetPaidResponsesCount(Int32 accessToken, Int32 survey, string whereClause = null)
        {
            try
            {
                return GetPaidResponsesCountImpl(accessToken, survey, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει τα responses που μπορεί να δεί ένας Πελάτης ανα σελίδα, για συγκεκριμένο survey. Το τι μπορεί να δεί εξαρτάται απο το Profile του, και τις τυχών πληρωμές του.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLResponse> GetPaidResponses(Int32 accessToken, Int32 survey, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetPaidResponsesPagedImpl(accessToken, survey, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Επιστρέφει όλα τα κατοχυρωμένα responses για το survey, ασχετως απο το εάν είναι ορατά στον Πελάτη
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLResponse> GetResponses(Int32 accessToken, Int32 survey, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetResponsesImpl(accessToken, survey, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το πλήθος των κατοχυρωμένων responses για το survey, ασχετως απο το εάν είναι ορατά στον Πελάτη
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        internal int GetResponsesCount(Int32 accessToken, Int32 survey, string whereClause = null)
        {
            try
            {
                return GetResponsesCountImpl(accessToken, survey, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει όλα τα κατοχυρωμένα responses για το survey ανα σελίδα, ασχετως απο το εάν είναι ορατά στον Πελάτη
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRows"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal Collection<VLResponse> GetResponses(Int32 accessToken, Int32 survey, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetResponsesPagedImpl(accessToken, survey, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLResponse GetResponseById(Int32 accessToken, Int64 responseId)
        {
            try
            {
                return GetResponseByIdImpl(accessToken, responseId);
            }
            catch
            {
                throw;
            }
        }

        public VLResponse GetResponseByRecipient(Int32 accessToken, Int64 recipientId)
        {
            try
            {
                var whereClause = string.Format("where Recipient={0}", recipientId);
                var items = GetResponsesExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;

                return items[0];
            }
            catch
            {
                throw;
            }
        }

        public void DeleteResponse(Int32 accessToken, Int64 answerId)
        {
            try
            {
                DeleteResponseImpl(accessToken, answerId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public void DeleteAllResponsesForSurvey(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                DeleteAllResponsesForSurveyImpl(accessToken, surveyId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLCollector DeleteAllResponsesForCollector(Int32 accessToken, Int32 surveyId, Int32 collectorId)
        {
            try
            {
                return DeleteAllResponsesForCollectorImpl(accessToken, surveyId, collectorId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLResponse UpdateResponse(Int32 accessToken, VLResponse response)
        {
            if (response == null) throw new ArgumentNullException("response");

            try
            {
                return UpdateResponseImpl(accessToken, response, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Δημιουργεί μία νέα εγγραφή στον πίνακα των Responses
        /// <para>Παράλληλα αυξάνει κατα μία (1) μονάδα το SurveyCollectors.Responses, το Surveys.RecordedResponses, 
        /// και κάνει true το Surveys.HasResponses</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public VLResponse CreateResponse(Int32 accessToken, VLResponse response)
        {
            if (response == null) throw new ArgumentNullException("CreateResponse");

            try
            {
                return CreateResponseImpl(accessToken, response, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLResponse> GetPaidResponsesImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause);
        internal abstract int GetPaidResponsesCountImpl(Int32 accessToken, Int32 survey, string whereClause);
        internal abstract Collection<VLResponse> GetPaidResponsesPagedImpl(Int32 accessToken, Int32 survey, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);

        internal abstract Collection<VLResponse> GetResponsesImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause);
        internal abstract Collection<VLResponse> GetResponsesExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetResponsesCountImpl(Int32 accessToken, Int32 survey, string whereClause);
        internal abstract Collection<VLResponse> GetResponsesPagedImpl(Int32 accessToken, Int32 survey, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLResponse GetResponseByIdImpl(Int32 accessToken, Int64 answerId);
        internal abstract void DeleteResponseImpl(Int32 accessToken, Int64 answerId, DateTime currentTimeUtc);
        internal abstract void DeleteAllResponsesForSurveyImpl(Int32 accessToken, Int32 survey, DateTime currentTimeUtc);
        internal abstract VLCollector DeleteAllResponsesForCollectorImpl(Int32 accessToken, Int32 survey, Int32 collector, DateTime currentTimeUtc);
        internal abstract VLResponse UpdateResponseImpl(Int32 accessToken, VLResponse answer, DateTime currentTimeUtc);
        internal abstract VLResponse CreateResponseImpl(Int32 accessToken, VLResponse answer, DateTime currentTimeUtc);
        internal abstract VLResponse ChargePaymentForResponseImpl(Int32 accessToken, Int64 responseId, Int32 collectorId, Int32 surveyId, DateTime currentTimeUtc);

        protected Collection<VLResponse> ExecuteAndGetResponses(DbCommand cmd)
        {
            var collection = new Collection<VLResponse>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLResponse(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLResponse ExecuteAndGetResponse(DbCommand cmd)
        {
            VLResponse _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLResponse(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }


        public Collection<VLResponseEx> GetPaidResponseExs(Int32 accessToken, Int32 survey, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetPaidResponseExsImpl(accessToken, survey, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        internal abstract Collection<VLResponseEx> GetPaidResponseExsImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause);
        protected Collection<VLResponseEx> ExecuteAndGetResponseExs(DbCommand cmd)
        {
            var responses = new Collection<VLResponseEx>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε τα responses:*/
                    while (reader.Read())
                    {
                        var _object = new VLResponseEx(reader);
                        responses.Add(_object);
                    }

                    /*Στο δεύτερο result set έχουμε τα Details:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var detail = new VLResponseDetail(reader);

                            foreach(var r in responses)
                            {
                                if(detail.Response == r.ResponseId)
                                {
                                    r.Details.Add(detail);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return responses;
        }
        #endregion



        #region VLResponseDetail
        public Collection<VLResponseDetail> GetResponseDetails(Int32 accessToken, Int64 response)
        {
            try
            {
                return GetResponseDetailsImpl(accessToken, response);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLResponseDetail> GetResponseDetails(Int32 accessToken, Int64 response, Int16 question)
        {
            try
            {
                return GetResponseDetailsImpl(accessToken, response, question);
            }
            catch
            {
                throw;
            }
        }
        public VLResponseDetail GetResponseDetailById(Int32 accessToken, Int64 response, Int16 question, byte selectedOption, byte selectedColumn)
        {
            try
            {
                return GetResponseDetailByIdImpl(accessToken, response, question, selectedOption, selectedColumn);
            }
            catch
            {
                throw;
            }
        }
        public void DeleteResponseDetail(Int32 accessToken, Int64 response)
        {
            try
            {
                DeleteResponseDetailImpl(accessToken, response, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public void DeleteResponseDetail(Int32 accessToken, Int64 response, Int16 question)
        {
            try
            {
                DeleteResponseDetailImpl(accessToken, response, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLResponseDetail UpdateResponseDetail(Int32 accessToken, VLResponseDetail detail)
        {
            if (detail == null) throw new ArgumentNullException("detail");

            try
            {
                return UpdateResponseDetailImpl(accessToken, detail, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLResponseDetail CreateResponseDetail(Int32 accessToken, VLResponseDetail detail)
        {
            if (detail == null) throw new ArgumentNullException("detail");

            try
            {
                return CreateResponseDetailImpl(accessToken, detail, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLResponseDetail> GetResponseDetailsImpl(Int32 accessToken, Int64 response);
        internal abstract Collection<VLResponseDetail> GetResponseDetailsImpl(Int32 accessToken, Int64 response, Int16 question);
        internal abstract VLResponseDetail GetResponseDetailByIdImpl(Int32 accessToken, Int64 response, Int16 question, byte selectedOption, byte selectedColumn);
        internal abstract void DeleteResponseDetailImpl(Int32 accessToken, Int64 response, DateTime currentTimeUtc);
        internal abstract void DeleteResponseDetailImpl(Int32 accessToken, Int64 response, Int16 question, DateTime currentTimeUtc);
        internal abstract VLResponseDetail UpdateResponseDetailImpl(Int32 accessToken, VLResponseDetail detail, DateTime currentTimeUtc);
        internal abstract VLResponseDetail CreateResponseDetailImpl(Int32 accessToken, VLResponseDetail detail, DateTime currentTimeUtc);
        protected Collection<VLResponseDetail> ExecuteAndGetResponseDetails(DbCommand cmd)
        {
            var collection = new Collection<VLResponseDetail>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLResponseDetail(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLResponseDetail ExecuteAndGetResponseDetail(DbCommand cmd)
        {
            VLResponseDetail _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLResponseDetail(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLViews
        public Collection<VLView> GetViews(Int32 accessToken, Int32 surveyId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetViewsImpl(accessToken, surveyId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetViewsCount(Int32 accessToken, Int32 surveyId, string whereClause = null)
        {
            try
            {
                return GetViewsCountImpl(accessToken, surveyId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLView> GetViews(Int32 accessToken, Int32 surveyId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetViewsPagedImpl(accessToken, surveyId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLView GetViewById(Int32 accessToken, Guid viewId)
        {
            try
            {
                return GetViewByIdImpl(accessToken, viewId);
            }
            catch
            {
                throw;
            }
        }

        public VLView GetDefaultView(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                var items = GetViewsImpl(accessToken, surveyId, "where IsDefaultView=1", null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(string.Format("There are more than one Default Views for survey {0}",surveyId));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteView(Int32 accessToken, Guid viewId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteViewImpl(accessToken, viewId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLView UpdateView(Int32 accessToken, VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            try
            {
                return UpdateViewImpl(accessToken, view, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLView CreateView(Int32 accessToken, VLView view)
        {
            if (view == null) throw new ArgumentNullException("view");

            try
            {
                return CreateViewImpl(accessToken, view, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLView> GetViewsImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause);
        internal abstract int GetViewsCountImpl(Int32 accessToken, Int32 surveyId, string whereClause);
        internal abstract Collection<VLView> GetViewsPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLView GetViewByIdImpl(Int32 accessToken, Guid viewId);
        internal abstract void DeleteViewImpl(Int32 accessToken, Guid viewId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLView UpdateViewImpl(Int32 accessToken, VLView view, DateTime currentTimeUtc);
        internal abstract VLView CreateViewImpl(Int32 accessToken, VLView view, DateTime currentTimeUtc);
        protected Collection<VLView> ExecuteAndGetViews(DbCommand cmd)
        {
            var collection = new Collection<VLView>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLView(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLView ExecuteAndGetView(DbCommand cmd)
        {
            VLView _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLView(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLViewPage
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLViewPage> GetViewPages(Int32 accessToken, Guid viewId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetViewPagesImpl(accessToken, viewId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetViewPagesCount(Int32 accessToken, Guid viewId, string whereClause = null)
        {
            try
            {
                return GetViewPagesCountImpl(accessToken, viewId, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public VLViewPage GetViewPageById(Int32 accessToken, Guid viewId, Int32 survey, Int16 page)
        {
            try
            {
                return GetViewPageByIdImpl(accessToken, viewId, survey, page);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewPage"></param>
        /// <returns></returns>
        public VLViewPage UpdateViewPage(Int32 accessToken, VLViewPage viewPage)
        {
            if (viewPage == null) throw new ArgumentNullException("viewPage");

            try
            {
                return UpdateViewPageImpl(accessToken, viewPage, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLViewPage> GetViewPagesImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause);
        internal abstract int GetViewPagesCountImpl(Int32 accessToken, Guid viewId, string whereClause);
        internal abstract VLViewPage GetViewPageByIdImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 page);
        internal abstract VLViewPage UpdateViewPageImpl(Int32 accessToken, VLViewPage viewPage, DateTime currentTimeUtc);
        protected Collection<VLViewPage> ExecuteAndGetViewPages(DbCommand cmd)
        {
            var collection = new Collection<VLViewPage>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLViewPage(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLViewPage ExecuteAndGetViewPage(DbCommand cmd)
        {
            VLViewPage _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLViewPage(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLViewQuestion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewQuestions(Int32 accessToken, Guid viewId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetViewQuestionsImpl(accessToken, viewId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetViewQuestionsCount(Int32 accessToken, Guid viewId, string whereClause = null)
        {
            try
            {
                return GetViewQuestionsCountImpl(accessToken, viewId, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public Collection<VLViewQuestion> GetViewPageQuestions(Int32 accessToken, Guid viewId, Int32 survey, Int16 page)
        {
            try
            {
                var whereClause = string.Format("where [ViewId]='{0}' and [Survey]={1} and [Question] in (select [QuestionId] from [dbo].[SurveyQuestions] where [Survey]={1} and [Page] = {2})", viewId, survey, page);
                return GetViewQuestionsImpl(accessToken, viewId, whereClause, null);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <returns></returns>
        public VLViewQuestion GetViewQuestionById(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                return GetViewQuestionByIdImpl(accessToken, viewId, survey, question);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewQuestion"></param>
        /// <returns></returns>
        public VLViewQuestion UpdateViewQuestion(Int32 accessToken, VLViewQuestion viewQuestion)
        {
            if (viewQuestion == null) throw new ArgumentNullException("viewQuestion");

            try
            {
                return UpdateViewQuestionImpl(accessToken, viewQuestion, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLViewQuestion SetChartType(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, ChartType chartType)
        {
            try
            {
                return SetChartTypeImpl(accessToken, viewId, survey, question, chartType, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLViewQuestion SwitchAxisScale(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                return SwitchAxisScaleImpl(accessToken, viewId, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLViewQuestion ToggleChartVisibility(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                return ToggleChartVisibilityImpl(accessToken, viewId, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLViewQuestion ToggleDataTableVisibility(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                return ToggleDataTableVisibilityImpl(accessToken, viewId, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLViewQuestion ToggleZeroResponseOptionsVisibility(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                return ToggleZeroResponseOptionsVisibilityImpl(accessToken, viewId, survey, question, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLViewQuestion> GetViewQuestionsImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause);
        internal abstract int GetViewQuestionsCountImpl(Int32 accessToken, Guid viewId, string whereClause);
        internal abstract VLViewQuestion GetViewQuestionByIdImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question);
        internal abstract VLViewQuestion UpdateViewQuestionImpl(Int32 accessToken, VLViewQuestion viewQuestion, DateTime currentTimeUtc);

        internal abstract VLViewQuestion SetChartTypeImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, ChartType chartType, DateTime currentTimeUtc);
        internal abstract VLViewQuestion SwitchAxisScaleImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc);
        internal abstract VLViewQuestion ToggleChartVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc);
        internal abstract VLViewQuestion ToggleDataTableVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc);
        internal abstract VLViewQuestion ToggleZeroResponseOptionsVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc);

        protected Collection<VLViewQuestion> ExecuteAndGetViewQuestions(DbCommand cmd)
        {
            var collection = new Collection<VLViewQuestion>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLViewQuestion(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLViewQuestion ExecuteAndGetViewQuestion(DbCommand cmd)
        {
            VLViewQuestion _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLViewQuestion(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLViewCollector
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLViewCollector> GetViewCollectors(Int32 accessToken, Guid viewId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetViewCollectorsImpl(accessToken, viewId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        public int GetViewCollectorsCount(Int32 accessToken, Guid viewId, string whereClause = null)
        {
            try
            {
                return GetViewCollectorsCountImpl(accessToken, viewId, whereClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLViewCollector GetViewCollectorById(Int32 accessToken, Guid viewId, Int32 collector)
        {
            try
            {
                return GetViewCollectorByIdImpl(accessToken, viewId, collector);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="collector"></param>
        /// <returns></returns>
        public VLViewCollector UpdateViewCollector(Int32 accessToken, VLViewCollector collector)
        {
            if (collector == null) throw new ArgumentNullException("collector");

            try
            {
                return UpdateViewCollectorImpl(accessToken, collector, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLViewCollector> GetViewCollectorsImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause);
        internal abstract int GetViewCollectorsCountImpl(Int32 accessToken, Guid viewId, string whereClause);
        internal abstract VLViewCollector GetViewCollectorByIdImpl(Int32 accessToken, Guid viewId, Int32 collector);
        internal abstract VLViewCollector UpdateViewCollectorImpl(Int32 accessToken, VLViewCollector collector, DateTime currentTimeUtc);
        protected Collection<VLViewCollector> ExecuteAndGetViewCollectors(DbCommand cmd)
        {
            var collection = new Collection<VLViewCollector>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLViewCollector(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLViewCollector ExecuteAndGetViewCollector(DbCommand cmd)
        {
            VLViewCollector _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLViewCollector(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLViewFilter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public Collection<VLViewFilter> GetViewFilters(Int32 accessToken, Guid viewId)
        {
            try
            {
                return GetViewFiltersImpl(accessToken, viewId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public int GetViewFiltersCount(Int32 accessToken, Guid viewId)
        {
            try
            {
                return GetViewFiltersCountImpl(accessToken, viewId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filterId"></param>
        /// <returns></returns>
        public VLViewFilter GetViewFilterById(Int32 accessToken, Int32 filterId)
        {
            try
            {
                return GetViewFilterByIdImpl(accessToken, filterId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filterId"></param>
        public void DeleteViewFilter(Int32 accessToken, Guid viewId, Int32 filterId)
        {
            try
            {
                DeleteViewFilterImpl(accessToken, viewId, filterId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="viewId"></param>
        public void DeleteViewFiltersForView(Int32 accessToken, Guid viewId)
        {
            try
            {
                DeleteViewFiltersForViewImpl(accessToken, viewId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public VLViewFilter UpdateViewFilter(Int32 accessToken, VLViewFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            try
            {
                return UpdateViewFilterImpl(accessToken, filter, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public VLViewFilter UpdateViewFilterQuick(Int32 accessToken, VLViewFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            try
            {
                return UpdateViewFilterQuickImpl(accessToken, filter, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public VLViewFilter CreateViewFilter(Int32 accessToken, VLViewFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");

            try
            {
                return CreateViewFilterImpl(accessToken, filter, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLViewFilter> GetViewFiltersImpl(Int32 accessToken, Guid viewId);
        internal abstract int GetViewFiltersCountImpl(Int32 accessToken, Guid viewId);
        internal abstract VLViewFilter GetViewFilterByIdImpl(Int32 accessToken, Int32 filterId);
        internal abstract void DeleteViewFilterImpl(Int32 accessToken, Guid viewId, Int32 filterId, DateTime currentTimeUtc);
        internal abstract void DeleteViewFiltersForViewImpl(Int32 accessToken, Guid viewId, DateTime currentTimeUtc);
        internal abstract VLViewFilter UpdateViewFilterImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc);
        internal abstract VLViewFilter UpdateViewFilterQuickImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc);
        internal abstract VLViewFilter CreateViewFilterImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc);
        protected Collection<VLViewFilter> ExecuteAndGetViewFilters(DbCommand cmd)
        {
            var collection = new Collection<VLViewFilter>();
            VLViewFilter selectedFilter = null;

            Func<Int32, VLViewFilter> FindFilter = delegate(Int32 filterId)
            {
                if (selectedFilter != null && selectedFilter.FilterId == filterId)
                    return selectedFilter;

                foreach (var filter in collection)
                {
                    if (filter.FilterId == filterId)
                    {
                        selectedFilter = filter;
                        return selectedFilter;
                    }
                }
                return null;
            };

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε τα filters:*/
                    while (reader.Read())
                    {
                        var _object = new VLViewFilter(reader);
                        _object.FilterDetails = new Collection<VLFilterDetail>();
                        collection.Add(_object);
                    }

                    /*Στο δέυτερο result set έχουμε τα filters details, ταξινομήμενα ανα filter:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var detail = new VLFilterDetail(reader);

                            var _filter = FindFilter(detail.FilterId);
                            if (_filter != null)
                            {
                                _filter.FilterDetails.Add(detail);
                            }
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }
        protected VLViewFilter ExecuteAndGetViewFilter(DbCommand cmd)
        {
            VLViewFilter _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;

                    /*Στο πρώτο result set έχουμε το φίλτρο:*/
                    if(reader.Read())
                    {
                        _retObject = new VLViewFilter(reader);
                        _retObject.FilterDetails = new Collection<VLFilterDetail>();
                    }

                    /*Στο δέυτερο result set έχουμε τα filters details:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var detail = new VLFilterDetail(reader);

                            _retObject.FilterDetails.Add(detail);
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLResponses from Views
        public Collection<VLResponse> GetViewReponses(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters = null)
        {
            try
            {
                return GetViewResponsesImpl(accessToken, surveyId, viewId, qfilters);
            }
            catch
            {
                throw;
            }
        }
        public int GetViewReponsesCount(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters = null)
        {
            try
            {
                return GetViewResponsesCountImpl(accessToken, surveyId, viewId, qfilters);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLResponse> GetViewReponses(Int32 accessToken, Int32 surveyId, Guid viewId, int pageIndex, int pageSize, ref int totalRows, Collection<string> qfilters = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetViewResponsesPagesImpl(accessToken, surveyId, viewId, startRowIndex, pageSize, ref totalRows, qfilters);
            }
            catch
            {
                throw;
            }
        }
        
        public VLSummaryEx GetViewSummaryEx(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters = null)
        {
            try
            {
                return GetViewSummaryExImpl(accessToken, surveyId, viewId, qfilters);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLResponse> GetViewResponsesImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters);
        internal abstract Collection<VLResponse> GetViewResponsesPagesImpl(Int32 accessToken, Int32 surveyId, Guid viewId, int startRowIndex, int maximumRows, ref int totalRows, Collection<string> qfilters);
        internal abstract int GetViewResponsesCountImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters);
        internal abstract VLSummaryEx GetViewSummaryExImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters);


        protected Collection<VLResponse> ExecuteAndGetViewResponses(DbCommand cmd)
        {
            var collection = new Collection<VLResponse>();
            VLResponse selectedResponse = null;

            Func<Int64, VLResponse> FindResponse = delegate(Int64 responseId)
            {
                if (selectedResponse != null && selectedResponse.ResponseId == responseId)
                    return selectedResponse;

                foreach (var response in collection)
                {
                    if (response.ResponseId == responseId)
                    {
                        selectedResponse = response;
                        return selectedResponse;
                    }
                }
                return null;
            };

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε τα responses:*/
                    while (reader.Read())
                    {
                        var _object = new VLResponse(reader);
                        _object.ResponseDetails = new Collection<VLResponseDetail>();
                        _object.IsViewResponse = true;
                        collection.Add(_object);
                    }

                    /*Στο δέυτερο result set έχουμε τα response details, ταξινομήμενα ανα response:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var detail = new VLResponseDetail(reader);

                            var _response = FindResponse(detail.Response);
                            if (_response != null)
                            {
                                _response.ResponseDetails.Add(detail);
                            }
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
            return collection;
        }


        protected VLSummaryEx ExecuteAndGetViewSummaryEx(DbCommand cmd)
        {
            VLSummaryEx _summary = null;
            Collection<VLSummaryEx.VLQuestionSummary> questions = new Collection<VLSummaryEx.VLQuestionSummary>();


            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε το VLSummary:*/
                    if (reader.Read())
                    {
                        _summary = new VLSummaryEx(reader);
                    }
                    else
                    {
                        throw new VLException("There is no VLSummaryEx result set!");
                    }

                    /*Στο δεύτερο result set έχουμε τα VLPageSumary:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var page = new VLSummaryEx.VLPageSumary(reader);
                            _summary.Pages.Add(page);
                        }
                    }
                    /*Στο τρίτο result set έχουμε τα VLQuestionSummary:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var question = new VLSummaryEx.VLQuestionSummary(reader);

                            foreach (var page in _summary.Pages)
                            {
                                if (page.Id == question.Page)
                                {
                                    page.Questions.Add(question);
                                    break;
                                }
                            }

                            questions.Add(question);
                        }
                    }
                    /*Στο τέταρτο result set έχουμε τα VLResponseOption:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            byte? _Option = null;
                            byte? _Column = null;
                            string _UserInput = null, _OptionText = null, _ColumnText = null;

                            var _RowId = reader.GetInt64(0);
                            var _Question = reader.GetInt16(1);
                            if (!reader.IsDBNull(2)) _Option = reader.GetByte(2);
                            if (!reader.IsDBNull(3)) _OptionText = reader.GetString(3);
                            if (!reader.IsDBNull(4)) _Column = reader.GetByte(4);
                            if (!reader.IsDBNull(5)) _ColumnText = reader.GetString(5);
                            if (!reader.IsDBNull(6)) _UserInput = reader.GetString(6);
                            var _TotalCount = reader.GetInt32(7);

                            foreach (var q in questions)
                            {
                                if (q.Id == _Question)
                                {
                                    double _Percentage = Math.Round((_TotalCount / (double)q.TotalAnswered) * 100, 2);
                                    VLSummaryEx.VLResponseOption foundedOption = null;

                                    if (_Option.HasValue)
                                    {
                                        foreach(var o in q.ResponseTotals)
                                        {
                                            if(o.Id == _Option.Value)
                                            {
                                                foundedOption = o;
                                                break;
                                            }
                                        }
                                    }
                                    if (foundedOption == null)
                                    {
                                        foundedOption = new VLSummaryEx.VLResponseOption();
                                        foundedOption.Id = _Option;
                                        //foundedOption.Text = _OptionText;
                                        foundedOption.Cols = new Collection<VLSummaryEx.VLResponseColumn>();
                                        q.ResponseTotals.Add(foundedOption);
                                    }

                                    if(_Column.HasValue)
                                    {
                                        VLSummaryEx.VLResponseColumn column = new VLSummaryEx.VLResponseColumn();
                                        column.Id = _Column.Value;
                                        //column.Text = _ColumnText;
                                        column.Ttl = _TotalCount;
                                        column.Pcnt = _Percentage;
                                        column.Input = _UserInput;

                                        foundedOption.Ttl += _TotalCount;
                                        foundedOption.Cols.Add(column);
                                    }
                                    else
                                    {
                                        foundedOption.Ttl = _TotalCount;
                                        foundedOption.Pcnt = _Percentage;
                                        foundedOption.Input = _UserInput;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }


            /*
             * Εδώ τώρα θα διορθώσουμε το ποσοστό στις κολώνες.
             * δεν το θέλουμε ως ποσοστό των συνολων των απαντήσεων για αυτή την ερώτηση,
             * αλλά ως ποσοστό των συνόλων των απαντήσεων για αυτό το Option στο οποίο ανήκουν!
             */
            foreach (var q in questions)
            {
                foreach (var r in q.ResponseTotals)
                {
                    foreach(var c in r.Cols)
                    {
                        c.Pcnt = Math.Round((c.Ttl / (double)r.Ttl) * 100, 2);
                    }
                }
            }

            return _summary;
        }
        #endregion


        #region Dashboards
        public VLClientDashboard GetClientDashboard(Int32 accessToken, Int32 clientId)
        {
            try
            {
                return GetClientDashboardImpl(accessToken, clientId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLSystemDashboard GetSystemDashboard(Int32 accessToken)
        {
            try
            {
                return GetSystemDashboardImpl(accessToken, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract VLClientDashboard GetClientDashboardImpl(Int32 accessToken, Int32 clientId, DateTime currentTimeUtc);
        internal abstract VLSystemDashboard GetSystemDashboardImpl(Int32 accessToken, DateTime currentTimeUtc);

        protected VLClientDashboard ExecuteAndGetClientDashboard(DbCommand cmd)
        {
            VLClientDashboard _retObject = null;
            bool _randomValues = false;
            var rnd = new System.Random();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε το VLClientDashboard:*/
                    if (reader.Read())
                    {
                        _retObject = new VLClientDashboard(reader);
                    }
                    else
                    {
                        throw new VLException("There is no VLClientDashboard result set!");
                    }

                    /*Στο δεύτερο result set έχουμε τα LastResponses:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastResponses.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 300));
                                _retObject.LastResponses.Add(tuple);
                            }
                        }
                    }
                    /*Στο τρίτο result set έχουμε τα LastMessages:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastMessages.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 234));
                                _retObject.LastMessages.Add(tuple);
                            }
                        }
                    }
                    /*Στο τρίτο result set έχουμε τα LastMessageRecipients:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastMessageRecipients.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 600));
                                _retObject.LastMessageRecipients.Add(tuple);
                            }
                        }
                    }
                    /*Στο τέταρτο result set έχουμε τα LastClicks:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastClicks.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 1200));
                                _retObject.LastClicks.Add(tuple);
                            }
                        }
                    }
                    /*Στο πέμπτο result set έχουμε τα LastClicksWithResponse:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastClicksWithResponse.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 100));
                                _retObject.LastClicksWithResponse.Add(tuple);
                            }
                        }
                    }

                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            _retObject.CalculateRanges();
            return _retObject;
        }

        protected VLSystemDashboard ExecuteAndGetSystemDashboard(DbCommand cmd)
        {
            VLSystemDashboard _retObject = null;
            bool _randomValues = false;
            var rnd = new System.Random();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    /*Στο πρώτο result set έχουμε το VLSystemDashboard:*/
                    if (reader.Read())
                    {
                        _retObject = new VLSystemDashboard(reader);
                    }
                    else
                    {
                        throw new VLException("There is no VLSystemDashboard result set!");
                    }


                    /*Στο δεύτερο result set έχουμε τα LastResponses:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastResponses.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 500));
                                _retObject.LastResponses.Add(tuple);
                            }
                        }
                    }
                    /*Στο τρίτο result set έχουμε τα LastMessages:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if(!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastMessages.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 50));
                                _retObject.LastMessages.Add(tuple);
                            }
                        }
                    }
                    /*Στο τρίτο result set έχουμε τα LastMessageRecipients:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastMessageRecipients.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 4000));
                                _retObject.LastMessageRecipients.Add(tuple);
                            }
                        }
                    }
                    /*Στο τέταρτο result set έχουμε τα LastClicks:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastClicks.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 5000));
                                _retObject.LastClicks.Add(tuple);
                            }
                        }
                    }
                    /*Στο πέμπτο result set έχουμε τα LastClicksWithResponse:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastClicksWithResponse.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 800));
                                _retObject.LastClicksWithResponse.Add(tuple);
                            }
                        }
                    }
                    /*Στο έκτο result set έχουμε τα LastLogins:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastLogins.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 300));
                                _retObject.LastLogins.Add(tuple);
                            }
                        }
                    }
                    /*Στο έβδομο result set έχουμε τα LastLogRecords:*/
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            if (!_randomValues)
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), reader.GetInt32(1));
                                _retObject.LastLogRecords.Add(tuple);
                            }
                            else
                            {
                                var tuple = new Tuple<string, Int32>(reader.GetDateTime(0).ToString("MM/dd"), rnd.Next(0, 20));
                                _retObject.LastLogRecords.Add(tuple);
                            }
                        }
                    }

                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            _retObject.CalculateRanges();
            return _retObject;
        }
        #endregion



    }
}
