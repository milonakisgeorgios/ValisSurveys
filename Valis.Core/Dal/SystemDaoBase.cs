using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net.Mail;
using System.Reflection;
using Valis.Core.Configuration;
using Valis.Core.ViewModel;

namespace Valis.Core.Dal
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class SystemDaoBase : DataAccess
    {
        #region Instance Factory
        /// <summary>
        /// 
        /// </summary>
        protected SystemDaoBase() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="dbElement"></param>
        /// <returns></returns>
        public static SystemDaoBase GetInstance(string configSectionName, DatabaseConfigurationElement dbElement)
        {
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentException("configSectionName is invalid");
            if (dbElement == null) throw new ArgumentNullException("dbElement");

            string key = configSectionName + "$SystemDaoBase";
            var m_instance = GetDalInstance(key);
            if (m_instance == null)
            {
                var ProviderAssembly = Assembly.Load(dbElement.AssemblyName);

                m_instance = (SystemDaoBase)Activator.CreateInstance(ProviderAssembly.GetType(ProviderAssembly.GetName().Name + ".SystemDao"));
                m_instance.ConnectionString = dbElement.ConnectionString;
                m_instance.ProviderFactory = dbElement.ProviderFactory;
                m_instance.AssemblyName = dbElement.AssemblyName;
                m_instance.ConfigSectionName = configSectionName;

                SetDalInstance(key, m_instance);
            }
            return (SystemDaoBase)m_instance;
        }
        #endregion


        #region Login/Logout/Validation apparatus...
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="updateLastActivityDate"></param>
        /// <param name="currentTimeUtc"></param>
        /// <returns></returns>
        internal UserPassword GetPasswordWithFormat(string logOnToken, bool updateLastActivityDate, DateTime currentTimeUtc)
        {
            try
            {
                return GetPasswordWithFormatImpl(logOnToken, updateLastActivityDate, currentTimeUtc);
            }
            catch (Exception ex)
            {
                throw new DataException("Exception occured at SystemDaoBase::GetPasswordWithFormat().", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logOnToken"></param>
        /// <param name="isPasswordCorrect"></param>
        /// <param name="updateLastLoginActivityDate"></param>
        /// <param name="maxInvalidPasswordAttempts"></param>
        /// <param name="passwordAttemptWindow"></param>
        /// <param name="currentTimeUtc"></param>
        /// <param name="lastLoginDate"></param>
        /// <param name="lastActivityDate"></param>
        internal void UpdateUserInfo(string logOnToken, bool isPasswordCorrect, bool updateLastLoginActivityDate, int maxInvalidPasswordAttempts, int passwordAttemptWindow, DateTime currentTimeUtc, DateTime? lastLoginDate, DateTime? lastActivityDate)
        {
            try
            {
                UpdateUserInfoImpl(logOnToken, isPasswordCorrect, updateLastLoginActivityDate, maxInvalidPasswordAttempts, passwordAttemptWindow, currentTimeUtc, lastLoginDate, lastActivityDate);
            }
            catch (Exception ex)
            {
                throw new DataException("Exception occured at SystemDaoBase::UpdateUserInfo().", ex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="principalType"></param>
        /// <param name="ipAddress"></param>
        /// <param name="currentTimeUtc"></param>
        /// <returns></returns>
        internal VLAccessToken OpenAccessToken(Int32 principal, PrincipalType principalType, string ipAddress, DateTime currentTimeUtc)
        {
            try
            {
                return OpenAccessTokenImpl(principal, principalType, ipAddress, currentTimeUtc);
            }
            catch (Exception ex)
            {
                throw new DataException("Exception occured at SystemDaoBase::OpenAccessToken().", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        /// <param name="currentTimeUtc"></param>
        internal void CloseAccessToken(Int32 accessTokenId, DateTime currentTimeUtc)
        {
            try
            {
                CloseAccessTokenImpl(accessTokenId, currentTimeUtc);
            }
            catch (Exception ex)
            {
                throw new DataException("Exception occured at SystemDaoBase::CloseAccessToken().", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessTokenId"></param>
        /// <param name="currentTimeUtc"></param>
        /// <returns></returns>
        internal VLAccessToken ValidateAccessToken(Int32 accessTokenId, DateTime currentTimeUtc)
        {
            try
            {
                return ValidateAccessTokenImpl(accessTokenId, currentTimeUtc);
            }
            catch (Exception ex)
            {
                throw new DataException("Exception occured at SystemDaoBase::ValidateAccessToken().", ex);
            }
        }


        protected abstract UserPassword GetPasswordWithFormatImpl(string logOnToken, bool updateLastActivityDate, DateTime currentTimeUtc);
        protected abstract void UpdateUserInfoImpl(string logOnToken, bool isPasswordCorrect, bool updateLastLoginActivityDate, int maxInvalidPasswordAttempts, int passwordAttemptWindow, DateTime currentTimeUtc, DateTime? lastLoginDate, DateTime? lastActivityDate);
        protected abstract VLAccessToken OpenAccessTokenImpl(Int32 principal, PrincipalType principalType, string ipAddress, DateTime currentTimeUtc);
        protected abstract void CloseAccessTokenImpl(Int32 accessTokenId, DateTime currentTimeUtc);
        protected abstract VLAccessToken ValidateAccessTokenImpl(Int32 accessTokenId, DateTime currentTimeUtc);
        #endregion


        #region VLSystemParameter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemParameter> GetSystemParameters(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetSystemParametersImpl(accessToken, whereClause, orderByClause);
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
        public int GetSystemParametersCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetSystemParametersCountImpl(accessToken, whereClause);
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
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemParameter> GetSystemParameters(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSystemParametersPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
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
        /// <param name="parameterId"></param>
        /// <returns></returns>
        public VLSystemParameter GetSystemParameterById(Int32 accessToken, Guid parameterId)
        {
            try
            {
                return GetSystemParameterByIdImpl(accessToken, parameterId);
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
        /// <param name="parameterKey"></param>
        /// <returns></returns>
        public VLSystemParameter GetSystemParameterByKey(int accessToken, string parameterKey)
        {
            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where ParameterKey = N'{0}'", parameterKey.Replace("'", "''").ToLowerInvariant());
                Collection<VLSystemParameter> items = GetSystemParametersImpl(accessToken, whereClause, null);

                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one SystemParameters with the same ParameterKey!");
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
        /// <param name="parameterId"></param>
        /// <param name="_lastUpdateDT"></param>
        public void DeleteSystemParameter(Int32 accessToken, Guid parameterId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteSystemParameterImpl(accessToken, parameterId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="parameter"></param>
        /// <returns></returns>
        public VLSystemParameter UpdateSystemParameter(Int32 accessToken, VLSystemParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            try
            {
                return UpdateSystemParameterImpl(accessToken, parameter, Utility.UtcNow());
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
        /// <param name="parameter"></param>
        /// <returns></returns>
        public VLSystemParameter CreateSystemParameter(Int32 accessToken, VLSystemParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException("parameter");

            try
            {
                return CreateSystemParameterImpl(accessToken, parameter, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSystemParameter> GetSystemParametersImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetSystemParametersCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLSystemParameter> GetSystemParametersPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLSystemParameter GetSystemParameterByIdImpl(Int32 accessToken, Guid parameterId);
        internal abstract void DeleteSystemParameterImpl(Int32 accessToken, Guid parameterId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLSystemParameter UpdateSystemParameterImpl(Int32 accessToken, VLSystemParameter parameter, DateTime currentTimeUtc);
        internal abstract VLSystemParameter CreateSystemParameterImpl(Int32 accessToken, VLSystemParameter parameter, DateTime currentTimeUtc);
        protected Collection<VLSystemParameter> ExecuteAndGetSystemParameters(DbCommand cmd)
        {
            var collection = new Collection<VLSystemParameter>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSystemParameter(reader);
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
        protected VLSystemParameter ExecuteAndGetSystemParameter(DbCommand cmd)
        {
            VLSystemParameter _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSystemParameter(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLRole
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLRole> GetRoles(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetRolesImpl(accessToken, whereClause, orderByClause);
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
        public int GetRolesCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetRolesCountImpl(accessToken, whereClause);
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
        public Collection<VLRole> GetRoles(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetRolesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους ρόλους (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public VLRole GetRoleById(Int32 accessToken, Int16 roleId)
        {
            try
            {
                return GetRoleByIdImpl(accessToken, roleId);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους ρόλους (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public VLRole GetRoleByName(Int32 accessToken, string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "name"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Name)=N'{0}'", name.Replace("'", "''").ToUpperInvariant());
                var items = GetRolesExImpl(accessToken, whereClause, null);//<-- ψάχνουμε και στα κρυμμένα roles!
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.Roles_more_than_one, "Name", name));
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
        /// <param name="roleId"></param>
        /// <param name="_lastUpdateDT"></param>
        public void DeleteRole(Int32 accessToken, Int16 roleId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteRoleImpl(accessToken, roleId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="role"></param>
        /// <returns></returns>
        public VLRole UpdateRole(Int32 accessToken, VLRole role)
        {
            if (role == null) throw new ArgumentNullException("role");

            try
            {
                return UpdateRoleImpl(accessToken, role, Utility.UtcNow());
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
        /// <param name="role"></param>
        /// <returns></returns>
        public VLRole CreateRole(Int32 accessToken, VLRole role)
        {
            if (role == null) throw new ArgumentNullException("role");

            try
            {
                return CreateRoleImpl(accessToken, role, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους ρόλους (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        internal abstract Collection<VLRole> GetRolesExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLRole> GetRolesImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetRolesCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLRole> GetRolesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLRole GetRoleByIdImpl(Int32 accessToken, Int16 roleId);
        internal abstract void DeleteRoleImpl(Int32 accessToken, Int16 roleId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLRole UpdateRoleImpl(Int32 accessToken, VLRole role, DateTime currentTimeUtc);
        internal abstract VLRole CreateRoleImpl(Int32 accessToken, VLRole role, DateTime currentTimeUtc);
        protected Collection<VLRole> ExecuteAndGetRoles(DbCommand cmd)
        {
            var collection = new Collection<VLRole>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLRole(reader);
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
        protected VLRole ExecuteAndGetRole(DbCommand cmd)
        {
            VLRole _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLRole(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLCountry
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCountry> GetCountries(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetCountriesImpl(accessToken, whereClause, orderByClause);
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
        public int GetCountriesCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetCountriesCountImpl(accessToken, whereClause);
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
        public Collection<VLCountry> GetCountries(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetCountriesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
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
        /// <param name="countryId"></param>
        /// <returns></returns>
        public VLCountry GetCountryById(Int32 accessToken, Int32 countryId)
        {
            try
            {
                return GetCountryByIdImpl(accessToken, countryId);
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
        /// <param name="countryId"></param>
        /// <param name="_lastUpdateDT"></param>
        public void DeleteCountry(Int32 accessToken, Int32 countryId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteCountryImpl(accessToken, countryId, Utility.UtcNow());
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
        /// <param name="country"></param>
        /// <returns></returns>
        public VLCountry UpdateCountry(Int32 accessToken, VLCountry country)
        {
            if (country == null) throw new ArgumentNullException("country");

            try
            {
                return UpdateCountryImpl(accessToken, country, Utility.UtcNow());
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
        /// <param name="country"></param>
        /// <returns></returns>
        public VLCountry CreateCountry(Int32 accessToken, VLCountry country)
        {
            if (country == null) throw new ArgumentNullException("country");

            try
            {
                return CreateCountryImpl(accessToken, country, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLCountry> GetCountriesImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetCountriesCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLCountry> GetCountriesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLCountry GetCountryByIdImpl(Int32 accessToken, Int32 countryId);
        internal abstract void DeleteCountryImpl(Int32 accessToken, Int32 countryId, DateTime currentTimeUtc);
        internal abstract VLCountry UpdateCountryImpl(Int32 accessToken, VLCountry country, DateTime currentTimeUtc);
        internal abstract VLCountry CreateCountryImpl(Int32 accessToken, VLCountry country, DateTime currentTimeUtc);
        protected Collection<VLCountry> ExecuteAndGetCountries(DbCommand cmd)
        {
            var collection = new Collection<VLCountry>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCountry(reader);
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
        protected VLCountry ExecuteAndGetCountry(DbCommand cmd)
        {
            VLCountry _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLCountry(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLEmailTemplate
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLEmailTemplate> GetEmailTemplates(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetEmailTemplatesImpl(accessToken, whereClause, orderByClause);
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
        public int GetEmailTemplatesCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetEmailTemplatesCountImpl(accessToken, whereClause);
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
        public Collection<VLEmailTemplate> GetEmailTemplates(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetEmailTemplatesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLEmailTemplate GetEmailTemplateById(Int32 accessToken, Int16 templateId)
        {
            try
            {
                return GetEmailTemplateByIdImpl(accessToken, templateId);
            }
            catch
            {
                throw;
            }
        }

        public VLEmailTemplate GetEmailTemplateByName(Int32 accessToken, string name)
        {
            VLEmailTemplate.ValidateName(ref name);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Name)=N'{0}'", name.Replace("'", "''").ToUpperInvariant());
                var items = GetEmailTemplatesImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.EmailTemplates_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }

        public void DeleteEmailTemplate(Int32 accessToken, Int16 templateId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteEmailTemplateImpl(accessToken, templateId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="template"></param>
        /// <returns></returns>
        public VLEmailTemplate UpdateEmailTemplate(Int32 accessToken, VLEmailTemplate template)
        {
            if (template == null) throw new ArgumentNullException("template");

            try
            {
                return UpdateEmailTemplateImpl(accessToken, template, Utility.UtcNow());
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
        /// <param name="template"></param>
        /// <returns></returns>
        public VLEmailTemplate CreateEmailTemplate(Int32 accessToken, VLEmailTemplate template)
        {
            if (template == null) throw new ArgumentNullException("template");

            try
            {
                return CreateEmailTemplateImpl(accessToken, template, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLEmailTemplate> GetEmailTemplatesImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetEmailTemplatesCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLEmailTemplate> GetEmailTemplatesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLEmailTemplate GetEmailTemplateByIdImpl(Int32 accessToken, Int16 templateId);
        internal abstract void DeleteEmailTemplateImpl(Int32 accessToken, Int16 templateId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLEmailTemplate UpdateEmailTemplateImpl(Int32 accessToken, VLEmailTemplate template, DateTime currentTimeUtc);
        internal abstract VLEmailTemplate CreateEmailTemplateImpl(Int32 accessToken, VLEmailTemplate template, DateTime currentTimeUtc);
        protected Collection<VLEmailTemplate> ExecuteAndGetEmailTemplates(DbCommand cmd)
        {
            var collection = new Collection<VLEmailTemplate>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLEmailTemplate(reader);
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
        protected VLEmailTemplate ExecuteAndGetEmailTemplate(DbCommand cmd)
        {
            VLEmailTemplate _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLEmailTemplate(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLSystemEmail

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemEmail> GetSystemEmails(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetSystemEmailsImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει pending SystemEmails, τα οποία περιμένουν να αποσταλλούν.
        /// <para>Οσα pending SystemEmails επιστραφούν, αλλάζουν στην βάση το Status τους σε Executing και το πεδίο SendDT γίνεται update με την τρέχουσα ημερομηνία και ώρα που έγινε η αλλαγή!</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="maxRows"></param>
        /// <returns></returns>
        public Collection<VLSystemEmail> GetPendingSystemEmails(Int32 accessToken, Int32 maxRows)
        {
            try
            {
                return GetPendingSystemEmailsImpl(accessToken, maxRows, Utility.UtcNow());
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
        public int GetSystemEmailsCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetSystemEmailsCountImpl(accessToken, whereClause);
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
        public Collection<VLSystemEmail> GetSystemEmails(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSystemEmailsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
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
        /// <param name="emailId"></param>
        /// <returns></returns>
        public VLSystemEmail GetSystemEmailById(Int32 accessToken, Int32 emailId)
        {
            try
            {
                return GetSystemEmailByIdImpl(accessToken, emailId);
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
        /// <param name="emailId"></param>
        public void DeleteSystemEmail(Int32 accessToken, Int32 emailId)
        {
            try
            {
                DeleteSystemEmailImpl(accessToken, emailId, Utility.UtcNow());
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
        /// <param name="email"></param>
        /// <returns></returns>
        public VLSystemEmail UpdateSystemEmail(Int32 accessToken, VLSystemEmail email)
        {
            if (email == null) throw new ArgumentNullException("email");

            try
            {
                return UpdateSystemEmailImpl(accessToken, email, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLSystemEmail CreateSystemEmail(Int32 accessToken, string moduleName, string fromAddress, string fromDisplayName, string toAddress, string subject, string body)
        {
            VLSystemEmail.ValidateModuleName(ref moduleName);
            VLSystemEmail.ValidateFromAddress(ref fromAddress);
            VLSystemEmail.ValidateFromDisplayName(ref fromDisplayName);
            VLSystemEmail.ValidateToAddress(ref toAddress);
            VLSystemEmail.ValidateSubject(ref subject);

            try
            {
                /*Ελέγχουμε το format του fromAddress:*/
                if (!Utility.EmailIsValid(fromAddress))
                {
                    throw new VLException("Invalid fromAddress format!");
                }
                MailAddress mailAddress1 = new MailAddress(fromAddress);
                var _localPart1 = mailAddress1.User;
                var _domainPart1 = mailAddress1.Host;
                if (!Utility.IsValidDomainName(_domainPart1))
                {
                    throw new VLException("Invalid fromAddress domain!");
                }

                /*Ελέγχουμε το format του toAddress:*/
                if (!Utility.EmailIsValid(toAddress))
                {
                    throw new VLException("Invalid toAddress format!");
                }
                MailAddress mailAddress2 = new MailAddress(toAddress);
                var _localPart2 = mailAddress2.User;
                var _domainPart2 = mailAddress2.Host;
                if (!Utility.IsValidDomainName(_domainPart2))
                {
                    throw new VLException("Invalid toAddress domain!");
                }

                VLSystemEmail email = new VLSystemEmail();
                email.AccessToken = accessToken;
                email.ModuleName = moduleName;
                email.EnterDt = Utility.UtcNow();
                email.FromAddress = fromAddress;
                email.FromDisplayName = fromDisplayName;
                email.ToAddress = toAddress;
                email.Subject = subject;
                email.Body = body;
                email.Status = EmailStatus.Pending;


                return CreateSystemEmailImpl(accessToken, email, Utility.UtcNow());
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
        /// <param name="email"></param>
        /// <returns></returns>
        public VLSystemEmail CreateSystemEmail(Int32 accessToken, VLSystemEmail email)
        {
            if (email == null) throw new ArgumentNullException("email");

            try
            {
                return CreateSystemEmailImpl(accessToken, email, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSystemEmail> GetSystemEmailsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLSystemEmail> GetPendingSystemEmailsImpl(int accessToken, int maxRows, DateTime currentTimeUtc);
        internal abstract int GetSystemEmailsCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLSystemEmail> GetSystemEmailsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLSystemEmail GetSystemEmailByIdImpl(Int32 accessToken, Int32 emailId);
        internal abstract void DeleteSystemEmailImpl(Int32 accessToken, Int32 emailId, DateTime currentTimeUtc);
        internal abstract VLSystemEmail UpdateSystemEmailImpl(Int32 accessToken, VLSystemEmail email, DateTime currentTimeUtc);
        internal abstract VLSystemEmail CreateSystemEmailImpl(Int32 accessToken, VLSystemEmail email, DateTime currentTimeUtc);
        protected Collection<VLSystemEmail> ExecuteAndGetSystemEmails(DbCommand cmd)
        {
            var collection = new Collection<VLSystemEmail>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSystemEmail(reader);
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
        protected VLSystemEmail ExecuteAndGetSystemEmail(DbCommand cmd)
        {
            VLSystemEmail _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSystemEmail(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLSystemUser
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLSystemUser> GetSystemUsers(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                return GetSystemUsersImpl(accessToken, whereClause, orderByClause);
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
        public int GetSystemUsersCount(Int32 accessToken, string whereClause = null)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                return GetSystemUsersCountImpl(accessToken, whereClause);
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
        public Collection<VLSystemUser> GetSystemUsers(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            _ValidateAccessToken(accessToken);
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSystemUsersPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους systemusers (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserById(Int32 accessToken, Int32 userId)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                return GetSystemUserByIdImpl(accessToken, userId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους systemusers (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserByEmail(Int32 accessToken, string email)
        {
            _ValidateAccessToken(accessToken);
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "email"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where lower(Email) = '{0}'", email.Replace("'", "''").ToLowerInvariant());
                var items = GetSystemUsersExImpl(accessToken, whereClause, string.Empty);
                if (items.Count == 0)
                    return null;

                /*we don't check the return count, we just grab one and return:*/
                return items[0];
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους systemusers (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="logOnToken"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserByLogOnToken(Int32 accessToken, string logOnToken)
        {
            VLCredential.ValidateLogOnToken(ref logOnToken);
            if (string.IsNullOrWhiteSpace(logOnToken)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "logOnToken"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where UserId in (select Principal from Credentials where PrincipalType = 0 and upper(LogOnToken) = '{0}')", logOnToken.Replace("'", "''").ToUpperInvariant());
                var items = GetSystemUsersExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one Users with the same LogOnToken!");
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλους τους systemusers (ακόμα και τους hidden)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="fromAccessTokenId"></param>
        /// <returns></returns>
        public VLSystemUser GetSystemUserFromAccessToken(Int32 accessToken, Int32 fromAccessTokenId)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where UserId in (select Principal from AccessTokens where AccessTokenId='{0}')", fromAccessTokenId);
                var items = GetSystemUsersExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one Principals with the same AccessTokenId!");
            }
            catch
            {
                throw;
            }
        }

        public void DeleteSystemUser(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                DeleteSystemUserImpl(accessToken, userId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLSystemUser UpdateSystemUser(Int32 accessToken, VLSystemUser systemUser)
        {
            _ValidateAccessToken(accessToken);
            if (systemUser == null) throw new ArgumentNullException("systemUser");

            try
            {
                return UpdateSystemUserImpl(accessToken, systemUser, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLSystemUser CreateSystemUser(Int32 accessToken, VLSystemUser systemUser)
        {
            _ValidateAccessToken(accessToken);
            if (systemUser == null) throw new ArgumentNullException("systemUser");

            try
            {
                return CreateSystemUserImpl(accessToken, systemUser, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLSystemUser> GetSystemUsersExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLSystemUser> GetSystemUsersImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetSystemUsersCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLSystemUser> GetSystemUsersPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLSystemUser GetSystemUserByIdImpl(Int32 accessToken, Int32 userId);
        internal abstract void DeleteSystemUserImpl(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLSystemUser UpdateSystemUserImpl(Int32 accessToken, VLSystemUser systemUser, DateTime currentTimeUtc);
        internal abstract VLSystemUser CreateSystemUserImpl(Int32 accessToken, VLSystemUser systemUser, DateTime currentTimeUtc);
        protected Collection<VLSystemUser> ExecuteAndGetSystemUsers(DbCommand cmd)
        {
            var collection = new Collection<VLSystemUser>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSystemUser(reader);
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
        protected VLSystemUser ExecuteAndGetSystemUser(DbCommand cmd)
        {
            VLSystemUser _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSystemUser(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLCredential
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLCredential> GetCredentials(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                return GetCredentialsImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public VLCredential GetCredentialById(Int32 accessToken, Int32 credentialId)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                return GetCredentialByIdImpl(accessToken, credentialId);
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
        /// <param name="principalType"></param>
        /// <param name="principalId"></param>
        /// <returns></returns>
        public VLCredential GetCredentialForPrincipal(Int32 accessToken, PrincipalType principalType, Int32 principalId)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where Principal = '{0}' and PrincipalType = '{1}'", principalId, (byte)principalType);
                var items = GetCredentialsImpl(accessToken, whereClause, string.Empty);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(string.Format(CultureInfo.InvariantCulture, "There are more than one Credentials for principal with id = '{0}'", principalId));
            }
            catch
            {
                throw;
            }
        }

        public VLCredential GetCredentialsForLogOnToken(Int32 accessToken, string logOnToken)
        {
            _ValidateAccessToken(accessToken);
            VLCredential.ValidateLogOnToken(ref logOnToken);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(LogOnToken) = '{0}'", logOnToken.Replace("'", "''").ToUpperInvariant());
                var items = GetCredentialsImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one Credentials with the same LogOnToken!");
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCredential(Int32 accessToken, Int32 credentialId, DateTime _lastUpdateDT)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                DeleteCredentialImpl(accessToken, credentialId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="credential"></param>
        /// <returns></returns>
        public VLCredential UpdateCredential(Int32 accessToken, VLCredential credential)
        {
            _ValidateAccessToken(accessToken);
            if (credential == null) throw new ArgumentNullException("credential");

            try
            {
                return UpdateCredentialImpl(accessToken, credential, Utility.UtcNow());
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
        /// <param name="credential"></param>
        /// <returns></returns>
        public VLCredential CreateCredential(Int32 accessToken, VLCredential credential)
        {
            _ValidateAccessToken(accessToken);
            if (credential == null) throw new ArgumentNullException("credential");

            try
            {
                return CreateCredentialImpl(accessToken, credential, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLCredential> GetCredentialsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract VLCredential GetCredentialByIdImpl(Int32 accessToken, Int32 credentialId);
        internal abstract void DeleteCredentialImpl(Int32 accessToken, Int32 credentialId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLCredential UpdateCredentialImpl(Int32 accessToken, VLCredential credential, DateTime currentTimeUtc);
        internal abstract VLCredential CreateCredentialImpl(Int32 accessToken, VLCredential credential, DateTime currentTimeUtc);
        protected Collection<VLCredential> ExecuteAndGetCredentials(DbCommand cmd)
        {
            var collection = new Collection<VLCredential>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCredential(reader);
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
        protected VLCredential ExecuteAndGetCredential(DbCommand cmd)
        {
            VLCredential _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLCredential(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion



        #region VLClient

        public Collection<VLClient> GetClients(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetClientsImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetClientsCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetClientsCountImpl(accessToken, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetClientsByProfileCount(Int32 accessToken, Int32 profileId)
        {
            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where Profile={0}", profileId);

                return GetClientsCountImpl(accessToken, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLClient> GetClients(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLClientEx> GetClientExs(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientExsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLClient GetClientById(Int32 accessToken, Int32 clientId)
        {
            try
            {
                return GetClientByIdImpl(accessToken, clientId);
            }
            catch
            {
                throw;
            }
        }
        public VLClient GetClientByName(Int32 accessToken, string name)
        {
            VLClient.ValidateName(ref name);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where lower(c.Name)=N'{0}'", name.Replace("'", "''").ToLowerInvariant());
                var items = GetClientsImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.Clients_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }

        public VLClient GetClientForCollector(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                return GetClientForCollectorImpl(accessToken, collectorId);
            }
            catch
            {
                throw;
            }
        }
        public VLClient GetClientForSurvey(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                return GetClientForSurveyImpl(accessToken, surveyId);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteClient(Int32 accessToken, Int32 clientId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteClientImpl(accessToken, clientId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLClient UpdateClient(Int32 accessToken, VLClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            try
            {
                return UpdateClientImpl(accessToken, client, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLClient CreateClient(Int32 accessToken, VLClient client)
        {
            if (client == null) throw new ArgumentNullException("client");

            try
            {
                return CreateClientImpl(accessToken, client, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLClient> GetClientsImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetClientsCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLClient> GetClientsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract Collection<VLClientEx> GetClientExsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLClient GetClientByIdImpl(Int32 accessToken, Int32 clientId);
        internal abstract VLClient GetClientForCollectorImpl(Int32 accessToken, Int32 collectorId);
        internal abstract VLClient GetClientForSurveyImpl(Int32 accessToken, Int32 surveyId);
        internal abstract void DeleteClientImpl(Int32 accessToken, Int32 clientId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLClient UpdateClientImpl(Int32 accessToken, VLClient client, DateTime currentTimeUtc);
        internal abstract VLClient CreateClientImpl(Int32 accessToken, VLClient client, DateTime currentTimeUtc);
        protected Collection<VLClient> ExecuteAndGetClients(DbCommand cmd)
        {
            var collection = new Collection<VLClient>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClient(reader);
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
        protected Collection<VLClientEx> ExecuteAndGetClientExs(DbCommand cmd)
        {
            var collection = new Collection<VLClientEx>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClientEx(reader);
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
        protected VLClient ExecuteAndGetClient(DbCommand cmd)
        {
            VLClient _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLClient(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLClientProfile
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLClientProfile> GetClientProfiles(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetClientProfilesImpl(accessToken, whereClause, orderByClause);
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
        public int GetClientProfilesCount(Int32 accessToken, string whereClause = null)
        {
            try
            {
                return GetClientProfilesCountImpl(accessToken, whereClause);
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
        public Collection<VLClientProfile> GetClientProfiles(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientProfilesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλα τα profiles (και τα hidden για το unit testing)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="profileId"></param>
        /// <returns></returns>
        public VLClientProfile GetClientProfileById(Int32 accessToken, Int32 profileId)
        {
            try
            {
                return GetClientProfileByIdImpl(accessToken, profileId);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// <para>Η μέθοδος αυτή βλέπει όλα τα profiles (και τα hidden για το unit testing)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public VLClientProfile GetClientProfileByName(Int32 accessToken, string name)
        {
            VLClientProfile.ValidateName(ref name);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where lower(Name)=N'{0}'", name.Replace("'", "''").ToLowerInvariant());
                var items = GetClientProfilesExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.ClientProfiles_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Επιστρέφει το profile του συγκεκριμένου ΠΕλάτη
        /// <para>Η μέθοδος αυτή βλέπει όλα τα profiles (και τα hidden για το unit testing)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public VLClientProfile GetClientProfileForClient(Int32 accessToken, Int32 clientId)
        {

            try
            {
                var whereClause = string.Format("where [ProfileId] in (select [Profile] from [dbo].[Clients] where [ClientId]={0})", clientId);
                var items = GetClientProfilesExImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;

                return items[0];
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
        /// <param name="profileId"></param>
        /// <param name="_lastUpdateDT"></param>
        public void DeleteClientProfile(Int32 accessToken, Int32 profileId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteClientProfileImpl(accessToken, profileId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="profile"></param>
        /// <returns></returns>
        public VLClientProfile UpdateClientProfile(Int32 accessToken, VLClientProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("obj");

            try
            {
                return UpdateClientProfileImpl(accessToken, profile, Utility.UtcNow());
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
        /// <param name="profile"></param>
        /// <returns></returns>
        public VLClientProfile CreateClientProfile(Int32 accessToken, VLClientProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile");

            try
            {
                return CreateClientProfileImpl(accessToken, profile, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLClientProfile> GetClientProfilesExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLClientProfile> GetClientProfilesImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetClientProfilesCountImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLClientProfile> GetClientProfilesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLClientProfile GetClientProfileByIdImpl(Int32 accessToken, Int32 profileId);
        internal abstract void DeleteClientProfileImpl(Int32 accessToken, Int32 profileId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLClientProfile UpdateClientProfileImpl(Int32 accessToken, VLClientProfile profile, DateTime currentTimeUtc);
        internal abstract VLClientProfile CreateClientProfileImpl(Int32 accessToken, VLClientProfile profile, DateTime currentTimeUtc);
        protected Collection<VLClientProfile> ExecuteAndGetClientProfiles(DbCommand cmd)
        {
            var collection = new Collection<VLClientProfile>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClientProfile(reader);
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
        protected VLClientProfile ExecuteAndGetClientProfile(DbCommand cmd)
        {
            VLClientProfile _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLClientProfile(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLClientUser
        public Collection<VLClientUser> GetClientUsers(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetClientUsersImpl(accessToken, clientId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public int GetClientUsersCount(Int32 accessToken, Int32 clientId, string whereClause = null)
        {
            try
            {
                return GetClientUsersCountImpl(accessToken, clientId, whereClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLClientUser> GetClientUsers(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientUsersPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser GetClientUserById(Int32 accessToken, Int32 userId)
        {
            try
            {
                return GetClientUserByIdImpl(accessToken, userId);
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser GetClientUserByEmail(Int32 accessToken, string email)
        {
            _ValidateAccessToken(accessToken);
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "email"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where lower(Email) = '{0}'", email.Replace("'", "''").ToLowerInvariant());
                var items = GetClientUsersImpl(accessToken, whereClause, string.Empty);
                if (items.Count == 0)
                    return null;

                /*we don't check the return count, we just grab one and return:*/
                return items[0];
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser GetClientUserByLogOnToken(Int32 accessToken, string logOnToken)
        {
            VLCredential.ValidateLogOnToken(ref logOnToken);
            if (string.IsNullOrWhiteSpace(logOnToken)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "logOnToken"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where UserId in (select Principal from Credentials where PrincipalType = 0 and upper(LogOnToken) = '{0}')", logOnToken.Replace("'", "''").ToUpperInvariant());
                var items = GetClientUsersImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one Users with the same LogOnToken!");
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser GetClientUserFromAccessToken(Int32 accessToken, Int32 fromAccessTokenId)
        {
            _ValidateAccessToken(accessToken);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where UserId in (select Principal from AccessTokens where AccessTokenId='{0}')", fromAccessTokenId);
                var items = GetClientUsersImpl(accessToken, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException("There are more than one Principals with the same AccessTokenId!");
            }
            catch
            {
                throw;
            }
        }
        public void DeleteClientUser(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteClientUserImpl(accessToken, userId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser UpdateClientUser(Int32 accessToken, VLClientUser clientUser)
        {
            if (clientUser == null) throw new ArgumentNullException("clientUser");

            try
            {
                return UpdateClientUserImpl(accessToken, clientUser, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        public VLClientUser CreateClientUser(Int32 accessToken, VLClientUser clientUser)
        {
            if (clientUser == null) throw new ArgumentNullException("clientUser");

            try
            {
                return CreateClientUserImpl(accessToken, clientUser, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLClientUser> GetClientUsersImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract Collection<VLClientUser> GetClientUsersImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetClientUsersCountImpl(Int32 accessToken, Int32 clientId, string whereClause);
        internal abstract Collection<VLClientUser> GetClientUsersPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLClientUser GetClientUserByIdImpl(Int32 accessToken, Int32 userId);
        internal abstract void DeleteClientUserImpl(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLClientUser UpdateClientUserImpl(Int32 accessToken, VLClientUser clientUser, DateTime currentTimeUtc);
        internal abstract VLClientUser CreateClientUserImpl(Int32 accessToken, VLClientUser clientUser, DateTime currentTimeUtc);
        protected Collection<VLClientUser> ExecuteAndGetClientUsers(DbCommand cmd)
        {
            var collection = new Collection<VLClientUser>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClientUser(reader);
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
        protected VLClientUser ExecuteAndGetClientUser(DbCommand cmd)
        {
            VLClientUser _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLClientUser(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLKnownEmail

        public Collection<VLKnownEmail> GetKnownEmails(Int32 accessToken, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetKnownEmailsExImpl(accessToken, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLKnownEmail> GetKnownEmails(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetKnownEmailsImpl(accessToken, clientId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetKnownEmailsCount(Int32 accessToken, Int32 clientId, string whereClause = null)
        {
            try
            {
                return GetKnownEmailsCountImpl(accessToken, clientId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLKnownEmail> GetKnownEmails(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetKnownEmailsPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLKnownEmailEx> GetKnownEmailExs(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetKnownEmailExsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLKnownEmail GetKnownEmailById(Int32 accessToken, Int32 emailId)
        {
            try
            {
                return GetKnownEmailByIdImpl(accessToken, emailId);
            }
            catch
            {
                throw;
            }
        }

        public VLKnownEmail GetKnownEmailByAddress(Int32 accessToken, Int32? clientId, string address)
        {
            if (string.IsNullOrWhiteSpace(address)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "address"));

            try
            {
                if (clientId.HasValue)
                {
                    var whereClause = string.Format(CultureInfo.InvariantCulture, "where EmailAddress=N'{0}'", address.Replace("'", "''").ToLowerInvariant());
                    var items = GetKnownEmailsImpl(accessToken, clientId.Value, whereClause, null);
                    if (items.Count == 0)
                        return null;
                    if (items.Count == 1)
                        return items[0];
                    throw new VLException(SR.GetString(SR.VerifiedEmails_more_than_one, "EmailAddress", address));
                }
                else
                {
                    var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(EmailAddress)=N'{0}'", address.Replace("'", "''").ToUpperInvariant());
                    var items = GetKnownEmailsExImpl(accessToken, whereClause, null);
                    if (items.Count == 0)
                        return null;
                    if (items.Count == 1)
                        return items[0];
                    throw new VLException(SR.GetString(SR.VerifiedEmails_more_than_one, "EmailAddress", address));
                }
            }
            catch
            {
                throw;
            }
        }

        public void DeleteKnownEmail(Int32 accessToken, Int32 emailId)
        {
            try
            {
                DeleteKnownEmailImpl(accessToken, emailId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLKnownEmail UpdateKnownEmail(Int32 accessToken, VLKnownEmail obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return UpdateKnownEmailImpl(accessToken, obj, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLKnownEmail CreateKnownEmail(Int32 accessToken, VLKnownEmail obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return CreateKnownEmailImpl(accessToken, obj, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLKnownEmail> GetKnownEmailsExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract Collection<VLKnownEmail> GetKnownEmailsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract int GetKnownEmailsCountImpl(Int32 accessToken, Int32 clientId, string whereClause);
        internal abstract Collection<VLKnownEmail> GetKnownEmailsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract Collection<VLKnownEmailEx> GetKnownEmailExsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLKnownEmail GetKnownEmailByIdImpl(Int32 accessToken, Int32 emailId);
        internal abstract void DeleteKnownEmailImpl(Int32 accessToken, Int32 emailId, DateTime currentTimeUtc);
        internal abstract VLKnownEmail UpdateKnownEmailImpl(Int32 accessToken, VLKnownEmail obj, DateTime currentTimeUtc);
        internal abstract VLKnownEmail CreateKnownEmailImpl(Int32 accessToken, VLKnownEmail obj, DateTime currentTimeUtc);
        protected Collection<VLKnownEmail> ExecuteAndGetKnownEmails(DbCommand cmd)
        {
            var collection = new Collection<VLKnownEmail>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLKnownEmail(reader);
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
        protected Collection<VLKnownEmailEx> ExecuteAndGetKnownEmailExs(DbCommand cmd)
        {
            var collection = new Collection<VLKnownEmailEx>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLKnownEmailEx(reader);
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
        protected VLKnownEmail ExecuteAndGetVerifiedEmail(DbCommand cmd)
        {
            VLKnownEmail _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLKnownEmail(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLPayment
        internal Collection<VLBalance> GetBalances(Int32 accessToken, Int32 clientId)
        {
            try
            {
                return GetBalancesImpl(accessToken, clientId);
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
        /// <param name="orderByClause"></param>
        /// <returns></returns>
        public Collection<VLPayment> GetPayments(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetPaymentsImpl(accessToken, clientId, whereClause, orderByClause);
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
        public int GetPaymentsCount(Int32 accessToken, Int32 clientId, string whereClause = null)
        {
            try
            {
                return GetPaymentsCountImpl(accessToken, clientId, whereClause);
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
        public Collection<VLPayment> GetPayments(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetPaymentsPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLPayment GetPaymentById(Int32 accessToken, Int32 paymentId)
        {
            try
            {
                return GetPaymentByIdImpl(accessToken, paymentId);
            }
            catch
            {
                throw;
            }
        }

        public void DeletePayment(Int32 accessToken, Int32 paymentId, DateTime _lastUpdateDT)
        {
            try
            {
                DeletePaymentImpl(accessToken, paymentId, _lastUpdateDT, Utility.UtcNow());
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
        /// <param name="payment"></param>
        /// <returns></returns>
        public VLPayment UpdatePayment(Int32 accessToken, VLPayment payment)
        {
            if (payment == null) throw new ArgumentNullException("payment");

            try
            {
                return UpdatePaymentImpl(accessToken, payment, Utility.UtcNow());
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
        /// <param name="payment"></param>
        /// <returns></returns>
        public VLPayment CreatePayment(Int32 accessToken, VLPayment payment)
        {
            if (payment == null) throw new ArgumentNullException("payment");

            try
            {
                return CreatePaymentImpl(accessToken, payment, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLBalance> GetBalancesImpl(Int32 accessToken, Int32 clientId);
        internal abstract Collection<VLPayment> GetPaymentsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract int GetPaymentsCountImpl(Int32 accessToken, Int32 clientId, string whereClause);
        internal abstract Collection<VLPayment> GetPaymentsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract Collection<VLPaymentView1> GetPaymentsView1PagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLPayment GetPaymentByIdImpl(Int32 accessToken, Int32 paymentId);
        internal abstract void DeletePaymentImpl(Int32 accessToken, Int32 paymentId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLPayment UpdatePaymentImpl(Int32 accessToken, VLPayment payment, DateTime currentTimeUtc);
        internal abstract VLPayment CreatePaymentImpl(Int32 accessToken, VLPayment payment, DateTime currentTimeUtc);
        
        protected Collection<VLPayment> ExecuteAndGetPayments(DbCommand cmd)
        {
            var collection = new Collection<VLPayment>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLPayment(reader);
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
        protected VLPayment ExecuteAndGetPayment(DbCommand cmd)
        {
            VLPayment _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLPayment(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }

        protected Collection<VLPaymentView1> ExecuteAndGetPaymentsView1(DbCommand cmd)
        {
            var collection = new Collection<VLPaymentView1>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLPaymentView1(reader);
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

        protected Collection<VLBalance> ExecuteAndGetBalances(DbCommand cmd)
        {
            var collection = new Collection<VLBalance>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLBalance(reader);
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
        #endregion

        #region VLCollectorPayment

        public Collection<VLCollectorPayment> GetCollectorPaymentsForPayment(Int32 accessToken, Int32 paymentId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetCollectorPaymentsForPaymentImpl(accessToken, paymentId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCollectorPayment> GetCollectorPaymentsForSurvey(Int32 accessToken, Int32 surveyId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetCollectorPaymentsForSurveyImpl(accessToken, surveyId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetCollectorPaymentsCountForSurvey(Int32 accessToken, Int32 surveyId, string whereClause = null)
        {
            try
            {
                return GetCollectorPaymentsCountForSurveyImpl(accessToken, accessToken, whereClause);
            }
            catch
            {
                throw;
            }
        }

        /// <returns></returns>
        public Collection<VLCollectorPayment> GetCollectorPaymentsForSurvey(Int32 accessToken, Int32 surveyId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetCollectorPaymentsPagedForSurveyImpl(accessToken, surveyId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }


        public Collection<VLCollectorPayment> GetCollectorPaymentsForCollector(Int32 accessToken, Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetCollectorPaymentsForCollectorImpl(accessToken, collectorId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }


        public int GetCollectorPaymentsCountForCollector(Int32 accessToken, Int32 collectorId, string whereClause = null)
        {
            try
            {
                return GetCollectorPaymentsCountForCollectorImpl(accessToken, accessToken, whereClause);
            }
            catch
            {
                throw;
            }
        }

        /// <returns></returns>
        public Collection<VLCollectorPayment> GetCollectorPaymentsForCollector(Int32 accessToken, Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetCollectorPaymentsPagedForCollectorImpl(accessToken, collectorId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLCollectorPayment GetCollectorPaymentById(Int32 accessToken, Int32 collectorPaymentId)
        {
            try
            {
                return GetCollectorPaymentByIdImpl(accessToken, collectorPaymentId);
            }
            catch
            {
                throw;
            }
        }
        public VLCollectorPayment GetCollectorPaymentByCollectorAndPayment(Int32 accessToken, Int32 collectorId, Int32 paymentId)
        {
            try
            {
                return GetCollectorPaymentByCollectorAndPaymentImpl(accessToken, collectorId, paymentId);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteCollectorPayment(Int32 accessToken, Int32 collectorPaymentId)
        {
            try
            {
                DeleteCollectorPaymentImpl(accessToken, collectorPaymentId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        public VLCollectorPayment UpdateCollectorPayment(Int32 accessToken, VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            try
            {
                return UpdateCollectorPaymentImpl(accessToken, collectorPayment, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        public VLCollectorPayment CreateCollectorPayment(Int32 accessToken, VLCollectorPayment collectorPayment)
        {
            if (collectorPayment == null) throw new ArgumentNullException("collectorPayment");

            try
            {
                return CreateCollectorPaymentImpl(accessToken, collectorPayment, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLCollectorPayment> GetCollectorPaymentsForPaymentImpl(Int32 accessToken, Int32 paymentId, string whereClause, string orderByClause);
        internal abstract Collection<VLCollectorPayment> GetCollectorPaymentsForSurveyImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause);
        internal abstract int GetCollectorPaymentsCountForSurveyImpl(Int32 accessToken, Int32 surveyId, string whereClause);
        internal abstract Collection<VLCollectorPayment> GetCollectorPaymentsPagedForSurveyImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);

        internal abstract Collection<VLCollectorPayment> GetCollectorPaymentsForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause);
        internal abstract int GetCollectorPaymentsCountForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause);
        internal abstract Collection<VLCollectorPayment> GetCollectorPaymentsPagedForCollectorImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        
        internal abstract VLCollectorPayment GetCollectorPaymentByIdImpl(Int32 accessToken, Int32 collectorPaymentId);
        internal abstract VLCollectorPayment GetCollectorPaymentByCollectorAndPaymentImpl(Int32 accessToken, Int32 collectorId, Int32 paymentId);
        internal abstract void DeleteCollectorPaymentImpl(Int32 accessToken, Int32 collectorPaymentId, DateTime currentTimeUtc);
        internal abstract VLCollectorPayment UpdateCollectorPaymentImpl(Int32 accessToken, VLCollectorPayment collectorPayment, DateTime currentTimeUtc);
        internal abstract VLCollectorPayment CreateCollectorPaymentImpl(Int32 accessToken, VLCollectorPayment collectorPayment, DateTime currentTimeUtc);

        protected Collection<VLCollectorPayment> ExecuteAndGetCollectorPayments(DbCommand cmd)
        {
            var collection = new Collection<VLCollectorPayment>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCollectorPayment(reader);
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
        protected VLCollectorPayment ExecuteAndGetCollectorPayment(DbCommand cmd)
        {
            VLCollectorPayment _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLCollectorPayment(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion

        #region VLCharge
        public Collection<VLChargedCollector> GetChargedCollectors(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetChargedCollectorsImpl(accessToken, clientId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLChargedCollector> GetChargedCollectorsPaged(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetChargedCollectorsPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }


        public Collection<VLCharge> GetCharges(Int32 accessToken, Int32 paymentId)
        {
            try
            {
                return GetChargesImpl(accessToken, paymentId);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCharge> GetChargesForClient(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetChargesForClientImpl(accessToken, clientId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCharge> GetChargesPagedForClient(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetChargesPagedForClientImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCharge> GetChargesForCollector(Int32 accessToken, Int32 collectorId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetChargesForCollectorImpl(accessToken, collectorId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        public Collection<VLCharge> GetChargesPagedForCollector(Int32 accessToken, Int32 collectorId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetChargesPagedForCollectorImpl(accessToken, collectorId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }


        internal bool ChargePaymentForEmail(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId)
        {
            try
            {
                return ChargePaymentForEmailImpl(accessToken, collectorPaymentId, collectorId, messageId, recipientId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal bool UnchargePaymentForEmail(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId)
        {
            try
            {
                return UnchargePaymentForEmailImpl(accessToken, collectorPaymentId, collectorId, messageId, recipientId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLChargedCollector> GetChargedCollectorsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract Collection<VLChargedCollector> GetChargedCollectorsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);

        internal abstract Collection<VLCharge> GetChargesImpl(Int32 accessToken, Int32 paymentId);
        internal abstract Collection<VLCharge> GetChargesForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract Collection<VLCharge> GetChargesPagedForClientImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract Collection<VLCharge> GetChargesForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause);
        internal abstract Collection<VLCharge> GetChargesPagedForCollectorImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract bool ChargePaymentForEmailImpl(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId, DateTime currentTimeUtc);
        internal abstract bool UnchargePaymentForEmailImpl(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId, DateTime currentTimeUtc);
        
        protected Collection<VLCharge> ExecuteAndGetCharges(DbCommand cmd)
        {
            var collection = new Collection<VLCharge>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLCharge(reader);
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

        protected Collection<VLChargedCollector> ExecuteAndGetChargedCollectors(DbCommand cmd)
        {
            var collection = new Collection<VLChargedCollector>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLChargedCollector(reader);
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
        #endregion



        #region VLClientList
        public Collection<VLClientList> GetClientLists(Int32 accessToken, Int32 clientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetClientListsImpl(accessToken, clientId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public int GetClientListsCount(Int32 accessToken, Int32 clientId, string whereClause = null)
        {
            try
            {
                return GetClientListsCountImpl(accessToken, clientId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLClientList> GetClientLists(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientListsPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLClientList GetClientListById(Int32 accessToken, Int32 listId)
        {
            try
            {
                return GetClientListByIdImpl(accessToken, listId);
            }
            catch
            {
                throw;
            }
        }


        public VLClientList GetClientListByName(Int32 accessToken, Int32 clientId, string name)
        {
            VLClientList.ValidateName(ref name);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where lower(Name)=N'{0}'", name.Replace("'", "''").ToLowerInvariant());
                var items = GetClientListsImpl(accessToken, clientId, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.ClientLists_more_than_one, "Name", name));
            }
            catch
            {
                throw;
            }
        }


        public void DeleteClientList(Int32 accessToken, Int32 listId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteClientListImpl(accessToken, listId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLClientList UpdateClientList(Int32 accessToken, VLClientList list)
        {
            if (list == null) throw new ArgumentNullException("list");

            try
            {
                return UpdateClientListImpl(accessToken, list, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLClientList CreateClientList(Int32 accessToken, VLClientList list)
        {
            if (list == null) throw new ArgumentNullException("list");

            try
            {
                return CreateClientListImpl(accessToken, list, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLClientList> GetClientListsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause);
        internal abstract int GetClientListsCountImpl(Int32 accessToken, Int32 clientId, string whereClause);
        internal abstract Collection<VLClientList> GetClientListsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLClientList GetClientListByIdImpl(Int32 accessToken, Int32 listId);
        internal abstract void DeleteClientListImpl(Int32 accessToken, Int32 listId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract VLClientList UpdateClientListImpl(Int32 accessToken, VLClientList list, DateTime currentTimeUtc);
        internal abstract VLClientList CreateClientListImpl(Int32 accessToken, VLClientList list, DateTime currentTimeUtc);
        protected Collection<VLClientList> ExecuteAndGetClientLists(DbCommand cmd)
        {
            var collection = new Collection<VLClientList>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClientList(reader);
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
        protected VLClientList ExecuteAndGetClientList(DbCommand cmd)
        {
            VLClientList _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLClientList(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLContacts
        public Collection<VLContact> GetContacts(Int32 accessToken, Int32 listId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetContactsImpl(accessToken, listId, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        
        public Collection<VLContact> GetContactsForClientByEmail(Int32 accessToken, Int32 clientId, string email)
        {
            VLContact.ValidateEmail(ref email);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where ClientId={0} and upper(Email)=N'{1}'", clientId, email.Replace("'", "''").ToUpperInvariant());
                
                return GetContactsExImpl(accessToken, whereClause, "order by ClientId, Email");
            }
            catch
            {
                throw;
            }
        }

        public int GetContactsCount(Int32 accessToken, Int32 listId, string whereClause = null)
        {
            try
            {
                return GetContactsCountImpl(accessToken, listId, whereClause);
            }
            catch
            {
                throw;
            }
        }

        public Collection<VLContact> GetContacts(Int32 accessToken, Int32 listId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetContactsPagedImpl(accessToken, listId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLContact GetContactById(Int32 accessToken, Int32 contactId)
        {
            try
            {
                return GetContactByIdImpl(accessToken, contactId);
            }
            catch
            {
                throw;
            }
        }
        public VLContact GetContactByEmail(Int32 accessToken, Int32 listId, string email)
        {
            VLContact.ValidateEmail(ref email);

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Email)=N'{0}'", email.Replace("'", "''").ToUpperInvariant());
                var items = GetContactsImpl(accessToken, listId, whereClause, null);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];
                throw new VLException(SR.GetString(SR.ListContacts_more_than_one, "Email", email));
            }
            catch
            {
                throw;
            }

        }

        public void DeleteContact(Int32 accessToken, Int32 contactId, DateTime _lastUpdateDT)
        {
            try
            {
                DeleteContactImpl(accessToken, contactId, _lastUpdateDT, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLContact UpdateContact(Int32 accessToken, VLContact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            try
            {
                return UpdateContactImpl(accessToken, contact, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        public VLContact CreateContact(Int32 accessToken, VLContact contact)
        {
            if (contact == null) throw new ArgumentNullException("contact");

            try
            {
                return CreateContactImpl(accessToken, contact, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }

        internal void ImportContact(Int32 callerPrincipalId, VLContact[] contacts, int length, ref Int32 successImports, ref Int32 failedImports)
        {

            try
            {
                ImportContactImpl(callerPrincipalId, contacts, length, Utility.UtcNow(), ref successImports, ref failedImports);
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Είναι τα τελευταίο πράγμα που καλείται μετά απο μία ή πολλαπλές κλήσεις της ImportContact
        /// <para>Ελέγχει για τυχόν OptedOut και Bounced Contacts (τα οποία και διαγράφει), και κάνει Update την
        /// κολώνα ClientLists.TotalContacts</para>
        /// </summary>
        /// <param name="callerPrincipalId"></param>
        /// <param name="clientId"></param>
        /// <param name="listId"></param>
        /// <param name="optedOutContacts"></param>
        /// <param name="bouncedContacts"></param>
        /// <param name="totalContacts"></param>
        internal void ImportContactsFinalize(Int32 callerPrincipalId, Int32 clientId, Int32 listId, ref Int32 optedOutContacts, ref Int32 bouncedContacts, ref Int32 totalContacts)
        {
            try
            {
                ImportContactsFinalizeImpl(callerPrincipalId, clientId, listId, Utility.UtcNow(), ref optedOutContacts, ref bouncedContacts, ref totalContacts);
            }
            catch
            {
                throw;
            }
        }

        internal void UpdateContactCounter(Int32 callerPrincipalId, Int32 listId)
        {
            try
            {
                UpdateContactCounterImpl(callerPrincipalId, listId);
            }
            catch
            {
                throw;
            }
        }

        internal int RemoveAllContactsFromList(Int32 accessToken, Int32 listId)
        {
            try
            {
                return RemoveAllContactsFromListImpl(accessToken, listId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveAllOptedOutContactsFromList(Int32 accessToken, Int32 listId)
        {
            try
            {
                return RemoveAllOptedOutContactsFromListImpl(accessToken, listId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveAllBouncedContactsFromList(Int32 accessToken, Int32 listId)
        {
            try
            {
                return RemoveAllBouncedContactsFromListImpl(accessToken, listId, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }
        internal int RemoveByDomainContactsFromList(Int32 accessToken, Int32 listId, string domainName)
        {
            Utility.CheckParameter(ref domainName, true, true, true, 128, "domainName");

            try
            {
                domainName = domainName.Replace("'", "''").ToUpperInvariant();
                return RemoveByDomainContactsFromListImpl(accessToken, listId, domainName, Utility.UtcNow());
            }
            catch
            {
                throw;
            }
        }



        internal abstract Collection<VLContact> GetContactsImpl(Int32 accessToken, Int32 listId, string whereClause, string orderByClause);
        internal abstract Collection<VLContact> GetContactsExImpl(Int32 accessToken, string whereClause, string orderByClause);
        internal abstract int GetContactsCountImpl(Int32 accessToken, Int32 listId, string whereClause);
        internal abstract Collection<VLContact> GetContactsPagedImpl(Int32 accessToken, Int32 listId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLContact GetContactByIdImpl(Int32 accessToken, Int32 contactId);
        internal abstract void DeleteContactImpl(Int32 accessToken, Int32 contactId, DateTime _lastUpdateDT, DateTime currentTimeUtc);
        internal abstract int RemoveAllContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc);
        internal abstract int RemoveAllOptedOutContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc);
        internal abstract int RemoveAllBouncedContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc);
        internal abstract int RemoveByDomainContactsFromListImpl(Int32 accessToken, Int32 listId, string domainName, DateTime currentTimeUtc);

        internal abstract VLContact UpdateContactImpl(Int32 accessToken, VLContact contact, DateTime currentTimeUtc);
        internal abstract VLContact CreateContactImpl(Int32 accessToken, VLContact contact, DateTime currentTimeUtc);
        internal abstract void ImportContactImpl(Int32 callerPrincipalId, VLContact[] contacts, int length, DateTime currentTimeUtc, ref Int32 successImports, ref Int32 failedImports);
        internal abstract void ImportContactsFinalizeImpl(Int32 callerPrincipalId, Int32 clientId, Int32 listId, DateTime currentTimeUtc, ref Int32 optedOutContacts, ref Int32 bouncedContacts, ref Int32 totalContacts);
        internal abstract void UpdateContactCounterImpl(Int32 callerPrincipalId, Int32 listId);
        protected Collection<VLContact> ExecuteAndGetContacts(DbCommand cmd)
        {
            var collection = new Collection<VLContact>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLContact(reader);
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
        protected VLContact ExecuteAndGetContact(DbCommand cmd)
        {
            VLContact _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLContact(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLLogins

        public Collection<VLLogin> GetLogins(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetLoginsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        public VLLogin GetLoginById(Int32 accessToken, Int32 loginId)
        {
            try
            {
                return GetLoginByIdImpl(accessToken, loginId);
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLLogin> GetLoginsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        internal abstract VLLogin GetLoginByIdImpl(Int32 accessToken, Int32 loginId);

        protected Collection<VLLogin> ExecuteAndGetLogins(DbCommand cmd)
        {
            var collection = new Collection<VLLogin>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLLogin(reader);
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
        protected VLLogin ExecuteAndGetLogin(DbCommand cmd)
        {
            VLLogin _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLLogin(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion
    }
}
