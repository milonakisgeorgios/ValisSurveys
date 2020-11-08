using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using Valis.Core.Configuration;

namespace Valis.Core.Dal
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class LibrariesDaoBase : DataAccess
    {
        #region Instance Factory
        /// <summary>
        /// 
        /// </summary>
        protected LibrariesDaoBase() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="dbElement"></param>
        /// <returns></returns>
        public static LibrariesDaoBase GetInstance(string configSectionName, DatabaseConfigurationElement dbElement)
        {
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentException("configSectionName is invalid");
            if (dbElement == null) throw new ArgumentNullException("dbElement");

            string key = configSectionName + "LibrariesDaoBase";
            var m_instance = GetDalInstance(key);
            if (m_instance == null)
            {
                var ProviderAssembly = Assembly.Load(dbElement.AssemblyName);

                m_instance = (LibrariesDaoBase)Activator.CreateInstance(ProviderAssembly.GetType(ProviderAssembly.GetName().Name + ".LibrariesDao"));
                m_instance.ConnectionString = dbElement.ConnectionString;
                m_instance.ProviderFactory = dbElement.ProviderFactory;
                m_instance.AssemblyName = dbElement.AssemblyName;
                m_instance.ConfigSectionName = configSectionName;

                SetDalInstance(key, m_instance);
            }
            return (LibrariesDaoBase)m_instance;
        }
        #endregion


        #region VLLibraryQuestionCategory
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategories(Int32 accessToken, string whereClause = null, string orderByClause = null, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryQuestionCategoriesImpl(accessToken, whereClause, orderByClause, textsLanguage);
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
        public int GetLibraryQuestionCategoriesCount(Int32 accessToken, string whereClause = null, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryQuestionCategoriesCountImpl(accessToken, whereClause, textsLanguage);
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
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategories(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null, short textsLanguage = 0)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetLibraryQuestionCategoriesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
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
        /// <param name="categoryId"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryQuestionCategory GetLibraryQuestionCategoryById(Int32 accessToken, Int16 categoryId, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryQuestionCategoryByIdImpl(accessToken, categoryId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public VLLibraryQuestionCategory GetLibraryQuestionCategoryByName(Int32 accessToken, string name, short textsLanguage)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(SR.GetString(SR.Argument_is_null_or_empty, "name"));

            try
            {
                var whereClause = string.Format(CultureInfo.InvariantCulture, "where upper(Name)=N'{0}'", name.Replace("'", "''").ToUpperInvariant());
                var items = GetLibraryQuestionCategoriesImpl(accessToken, whereClause, null, textsLanguage);
                if (items.Count == 0)
                    return null;
                if (items.Count == 1)
                    return items[0];

                throw new VLException(SR.GetString(SR.LibraryQuestionCategories_more_than_one, "Name", name));
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
        /// <param name="categoryId"></param>
        /// <param name="_lastUpdateDT"></param>
        public void DeleteLibraryQuestionCategory(Int32 accessToken, Int16 categoryId)
        {
            try
            {
                DeleteLibraryQuestionCategoryImpl(accessToken, categoryId, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public VLLibraryQuestionCategory UpdateLibraryQuestionCategory(Int32 accessToken, VLLibraryQuestionCategory obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return UpdateLibraryQuestionCategoryImpl(accessToken, obj, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryQuestionCategory CreateLibraryQuestionCategory(Int32 accessToken, VLLibraryQuestionCategory obj, short textsLanguage = 0)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return CreateLibraryQuestionCategoryImpl(accessToken, obj, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }


        internal abstract Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategoriesImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetLibraryQuestionCategoriesCountImpl(Int32 accessToken, string whereClause, short textsLanguage);
        internal abstract Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategoriesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);
        internal abstract VLLibraryQuestionCategory GetLibraryQuestionCategoryByIdImpl(Int32 accessToken, Int16 categoryId, short textsLanguage);
        internal abstract void DeleteLibraryQuestionCategoryImpl(Int32 accessToken, Int16 categoryId, DateTime currentTimeUtc);
        internal abstract VLLibraryQuestionCategory UpdateLibraryQuestionCategoryImpl(Int32 accessToken, VLLibraryQuestionCategory category, DateTime currentTimeUtc);
        internal abstract VLLibraryQuestionCategory CreateLibraryQuestionCategoryImpl(Int32 accessToken, VLLibraryQuestionCategory category, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLLibraryQuestionCategory> ExecuteAndGetLibraryQuestionCategories(DbCommand cmd)
        {
            var collection = new Collection<VLLibraryQuestionCategory>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLLibraryQuestionCategory(reader);
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
        protected VLLibraryQuestionCategory ExecuteAndGetLibraryQuestionCategory(DbCommand cmd)
        {
            VLLibraryQuestionCategory _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLLibraryQuestionCategory(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLLibraryQuestion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryQuestion> GetLibraryQuestions(Int32 accessToken, Int16 category, string whereClause = null, string orderByClause = null, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryQuestionsImpl(accessToken, category, whereClause, orderByClause, textsLanguage);
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
        public int GetLibraryQuestionsCount(Int32 accessToken, Int16 category, string whereClause = null)
        {
            try
            {
                return GetLibraryQuestionsCountImpl(accessToken, category, whereClause);
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
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryQuestion> GetLibraryQuestions(Int32 accessToken, Int16 category, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null, short textsLanguage = 0)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetLibraryQuestionsPagedImpl(accessToken, category, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public VLLibraryQuestion GetLibraryQuestionById(Int32 accessToken, Int32 questionId, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryQuestionByIdImpl(accessToken, questionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteLibraryQuestion(Int32 accessToken, Int32 questionId)
        {
            try
            {
                DeleteLibraryQuestionImpl(accessToken, questionId, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public VLLibraryQuestion UpdateLibraryQuestion(Int32 accessToken, VLLibraryQuestion obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return UpdateLibraryQuestionImpl(accessToken, obj, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryQuestion CreateLibraryQuestion(Int32 accessToken, VLLibraryQuestion obj, short textsLanguage = 0)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            try
            {
                return CreateLibraryQuestionImpl(accessToken, obj, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLLibraryQuestion> GetLibraryQuestionsImpl(Int32 accessToken, Int16 category, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetLibraryQuestionsCountImpl(Int32 accessToken, Int16 category, string whereClause);
        internal abstract Collection<VLLibraryQuestion> GetLibraryQuestionsPagedImpl(Int32 accessToken, Int16 category, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);


        internal abstract Collection<VLLibraryQuestion> GetLibraryQuestionsExImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage);
        internal abstract int GetLibraryQuestionsCountExImpl(Int32 accessToken, string whereClause);
        internal abstract Collection<VLLibraryQuestion> GetLibraryQuestionsPagedExImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage);
        
        
        internal abstract VLLibraryQuestion GetLibraryQuestionByIdImpl(Int32 accessToken, Int32 questionId, short textsLanguage);
        internal abstract void DeleteLibraryQuestionImpl(Int32 accessToken, Int32 questionId, DateTime currentTimeUtc);
        internal abstract VLLibraryQuestion UpdateLibraryQuestionImpl(Int32 accessToken, VLLibraryQuestion obj, DateTime currentTimeUtc);
        internal abstract VLLibraryQuestion CreateLibraryQuestionImpl(Int32 accessToken, VLLibraryQuestion obj, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLLibraryQuestion> ExecuteAndGetLibraryQuestions(DbCommand cmd)
        {
            var collection = new Collection<VLLibraryQuestion>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLLibraryQuestion(reader);
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
        protected VLLibraryQuestion ExecuteAndGetLibraryQuestion(DbCommand cmd)
        {
            VLLibraryQuestion _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLLibraryQuestion(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLLibraryOption
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryOption> GetLibraryOptions(Int32 accessToken, Int32 questionId, short textsLanguage)
        {
            try
            {
                return GetLibraryOptionsImpl(accessToken, questionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public VLLibraryOption GetLibraryOptionById(Int32 accessToken, Int32 question, Byte optionId, short textsLanguage)
        {
            try
            {
                return GetLibraryOptionByIdImpl(accessToken, question, optionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteLibraryOption(Int32 accessToken, Int32 question, Byte optionId)
        {
            try
            {
                DeleteLibraryOptionImpl(accessToken, question, optionId, Utility.UtcNow());
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
        /// <param name="option"></param>
        /// <returns></returns>
        public VLLibraryOption UpdateLibraryOption(Int32 accessToken, VLLibraryOption option)
        {
            if (option == null) throw new ArgumentNullException("option");

            try
            {
                return UpdateLibraryOptionImpl(accessToken, option, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryOption CreateLibraryOption(Int32 accessToken, VLLibraryOption option, short textsLanguage)
        {
            if (option == null) throw new ArgumentNullException("option");

            try
            {
                return CreateLibraryOptionImpl(accessToken, option, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLLibraryOption> GetLibraryOptionsImpl(Int32 accessToken, Int32 questionId, short textsLanguage);
        internal abstract VLLibraryOption GetLibraryOptionByIdImpl(Int32 accessToken, Int32 question, Byte optionId, short textsLanguage);
        internal abstract void DeleteLibraryOptionImpl(Int32 accessToken, Int32 question, Byte optionId, DateTime currentTimeUtc);
        internal abstract VLLibraryOption UpdateLibraryOptionImpl(Int32 accessToken, VLLibraryOption option, DateTime currentTimeUtc);
        internal abstract VLLibraryOption CreateLibraryOptionImpl(Int32 accessToken, VLLibraryOption option, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLLibraryOption> ExecuteAndGetLibraryOptions(DbCommand cmd)
        {
            var collection = new Collection<VLLibraryOption>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLLibraryOption(reader);
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
        protected VLLibraryOption ExecuteAndGetLibraryOption(DbCommand cmd)
        {
            VLLibraryOption _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLLibraryOption(reader);
                }
            }
            finally
            {
                cmd.Connection.Close();
            }

            return _retObject;
        }
        #endregion


        #region VLLibraryColumn
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="whereClause"></param>
        /// <param name="orderByClause"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public Collection<VLLibraryColumn> GetLibraryColumns(Int32 accessToken, Int32 questionId, short textsLanguage)
        {
            try
            {
                return GetLibraryColumnsImpl(accessToken, questionId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }
        public VLLibraryColumn GetLibraryColumnById(Int32 accessToken, Int32 question, Byte columnId, short textsLanguage = 0)
        {
            try
            {
                return GetLibraryColumnByIdImpl(accessToken, question, columnId, textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        public void DeleteLibraryColumn(Int32 accessToken, Int32 question, Byte columnId)
        {
            try
            {
                DeleteLibraryColumnImpl(accessToken, question, columnId, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public VLLibraryColumn UpdateLibraryColumn(Int32 accessToken, VLLibraryColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");

            try
            {
                return UpdateLibraryColumnImpl(accessToken, column, Utility.UtcNow());
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
        /// <param name="obj"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryColumn CreateLibraryColumn(Int32 accessToken, VLLibraryColumn column, short textsLanguage = 0)
        {
            if (column == null) throw new ArgumentNullException("column");

            try
            {
                return CreateLibraryColumnImpl(accessToken, column, Utility.UtcNow(), textsLanguage);
            }
            catch
            {
                throw;
            }
        }

        internal abstract Collection<VLLibraryColumn> GetLibraryColumnsImpl(Int32 accessToken, Int32 questionId, short textsLanguage);
        internal abstract VLLibraryColumn GetLibraryColumnByIdImpl(Int32 accessToken, Int32 question, Byte columnId, short textsLanguage);
        internal abstract void DeleteLibraryColumnImpl(Int32 accessToken, Int32 question, Byte columnId, DateTime currentTimeUtc);
        internal abstract VLLibraryColumn UpdateLibraryColumnImpl(Int32 accessToken, VLLibraryColumn column, DateTime currentTimeUtc);
        internal abstract VLLibraryColumn CreateLibraryColumnImpl(Int32 accessToken, VLLibraryColumn column, DateTime currentTimeUtc, short textsLanguage);
        protected Collection<VLLibraryColumn> ExecuteAndGetLibraryColumns(DbCommand cmd)
        {
            var collection = new Collection<VLLibraryColumn>();

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var _object = new VLLibraryColumn(reader);
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
        protected VLLibraryColumn ExecuteAndGetLibraryColumn(DbCommand cmd)
        {
            VLLibraryColumn _retObject = null;

            try
            {
                cmd.Connection.Open();
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLLibraryColumn(reader);
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
