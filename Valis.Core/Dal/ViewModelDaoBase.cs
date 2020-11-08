using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Reflection;
using Valis.Core.Configuration;
using Valis.Core.ViewModel;

namespace Valis.Core.Dal
{
    internal abstract class ViewModelDaoBase : DataAccess
    {
        #region Instance Factory
        /// <summary>
        /// 
        /// </summary>
        protected ViewModelDaoBase() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="dbElement"></param>
        /// <returns></returns>
        public static ViewModelDaoBase GetInstance(string configSectionName, DatabaseConfigurationElement dbElement)
        {
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentException("configSectionName is invalid");
            if (dbElement == null) throw new ArgumentNullException("dbElement");

            string key = configSectionName + "ViewModelDaoBase";
            var m_instance = GetDalInstance(key);
            if (m_instance == null)
            {
                var ProviderAssembly = Assembly.Load(dbElement.AssemblyName);

                m_instance = (ViewModelDaoBase)Activator.CreateInstance(ProviderAssembly.GetType(ProviderAssembly.GetName().Name + ".ViewModelDao"));
                m_instance.ConnectionString = dbElement.ConnectionString;
                m_instance.ProviderFactory = dbElement.ProviderFactory;
                m_instance.AssemblyName = dbElement.AssemblyName;
                m_instance.ConfigSectionName = configSectionName;

                SetDalInstance(key, m_instance);
            }
            return (ViewModelDaoBase)m_instance;
        }
        #endregion


        #region VLSystemUserView
        public Collection<VLSystemUserView> GetSystemUserViews(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetSystemUserViewsPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLSystemUserView> GetSystemUserViewsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        protected Collection<VLSystemUserView> ExecuteAndGetSystemUserViews(DbCommand cmd)
        {
            var collection = new Collection<VLSystemUserView>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLSystemUserView(reader);
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
        protected VLSystemUserView ExecuteAndGetSystemUserView(DbCommand cmd)
        {
            VLSystemUserView _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLSystemUserView(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLClientUserView
        public Collection<VLClientUserView> GetClientUserViews(Int32 accessToken, Int32 clientId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetClientUserViewsPagedImpl(accessToken, clientId, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch
            {
                throw;
            }
        }
        
        internal abstract Collection<VLClientUserView> GetClientUserViewsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        protected Collection<VLClientUserView> ExecuteAndGetClientUserViews(DbCommand cmd)
        {
            var collection = new Collection<VLClientUserView>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLClientUserView(reader);
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
        protected VLClientUserView ExecuteAndGetClientUserView(DbCommand cmd)
        {
            VLClientUserView _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLClientUserView(reader);
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
