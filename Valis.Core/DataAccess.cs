using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;

namespace Valis.Core
{
    /// <summary>
    /// Supports Database Operations and Cache Operations
    /// </summary>
    internal abstract class DataAccess
    {
        #region instance pooling infrastructure
        /*
         * Ολοι οι απόγονοι της DataAccess class (δηλαδή οι Dal classes) δεν έχουν state, και θέλουμε να λειτουργούν σαν singletons 
         * για performance reasons.
         * Το μοναδικό πρόβλημα είναι ότι έπρεπε να υπάρχει ένα instance της κάθε class για κάθε διαφορετικό connection string.
         * Οι παρακάτω δύο μέθοδοι (GetDalInstance και SetDalInstance) σε συνεργασία με τον static factory (GetInstance) της κάθε
         * Dal class κάνουν αυτό ακριβώς το πράγμα: Διατηρούν ένα reusable instance της κάθε class ανα διαφορετικό set ρυθμίσεων.
         */

        /// <summary>
        /// 
        /// </summary>
        static Dictionary<string, DataAccess> s_dalInstancesCache = new Dictionary<string, DataAccess>();
        /// <summary>
        /// Αναζητά απο την cashe ένα DataAccess object για το συγκεκριμένο κλειδί.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected static DataAccess GetDalInstance(string key)
        {
            if (s_dalInstancesCache.ContainsKey(key))
                return s_dalInstancesCache[key];
            else
                return null;
        }
        /// <summary>
        /// Εισάγει στην cache το συγκεκριμένο DataAccess object με το συγκεκριμένο κλειδί.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="instance"></param>
        protected static void SetDalInstance(string key, DataAccess instance)
        {
            s_dalInstancesCache.Add(key, instance);
        }
        #endregion


        /// <summary>
        /// ConnectionString
        /// </summary>
        internal string ConnectionString { get; set; }
        /// <summary>
        /// ProviderFactory
        /// </summary>
        internal string ProviderFactory { get; set; }
        /// <summary>
        /// AssemblyName
        /// </summary>
        internal string AssemblyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        internal string ConfigSectionName { get; set; }


        DbProviderFactory m_factory;
        DbProviderFactory Factory
        {
            get
            {
                if (m_factory == null)
                {
                    m_factory = DbProviderFactories.GetFactory(ProviderFactory);
                }
                return m_factory;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal DataAccess()
        {

        }

        /// <summary>
        /// Δημιουργεί ένα νέο DbConnection
        /// </summary>
        /// <returns></returns>
        protected DbConnection CreateConnection()
        {
            DbConnection conn = Factory.CreateConnection();
            conn.ConnectionString = ConnectionString;

            return conn;
        }
        /// <summary>
        /// Δημιουργεί ένα νέο DbCommand
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        protected DbCommand CreateCommand(DbConnection connection = null, DbTransaction transaction = null, CommandType commandType = CommandType.StoredProcedure)
        {
            DbConnection conn = connection == null ? CreateConnection() : connection;
            DbCommand cmd = conn.CreateCommand();
            cmd.CommandType = commandType;
            cmd.Transaction = transaction;
            return cmd;
        }
        /// <summary>
        /// Δημιουργεί ένα νέο DbCommand για μία store procedure
        /// </summary>
        /// <param name="storeProcedureName"></param>
        /// <returns></returns>
        protected DbCommand CreateCommand(string storeProcedureName)
        {
            DbCommand cmd = CreateCommand();
            cmd.CommandText = storeProcedureName;
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        protected DbParameter AddParameter(DbCommand cmd, string parameterName, object parameterValue, DbType parameterType)
        {
            DbParameter p1 = cmd.CreateParameter();
            p1.ParameterName = parameterName;
            if (parameterValue == null)//if ((parameterValue.HasValue == false))
                p1.Value = DBNull.Value;
            else
                p1.Value = parameterValue;
            p1.DbType = parameterType;
            cmd.Parameters.Add(p1);
            return p1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterDirection"></param>
        /// <returns></returns>
        protected DbParameter AddParameter(DbCommand cmd, string parameterName, object parameterValue, DbType parameterType, ParameterDirection parameterDirection)
        {
            DbParameter p1 = cmd.CreateParameter();
            p1.ParameterName = parameterName;
            if (parameterValue == null)//if ((parameterValue.HasValue == false))
                p1.Value = DBNull.Value;
            else
                p1.Value = parameterValue;
            p1.DbType = parameterType;
            p1.Direction = parameterDirection;
            cmd.Parameters.Add(p1);
            return p1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <param name="parameterType"></param>
        /// <param name="parameterDirection"></param>
        /// <param name="parameterSize"></param>
        /// <returns></returns>
        protected DbParameter AddParameter(DbCommand cmd, string parameterName, object parameterValue, DbType parameterType, ParameterDirection parameterDirection, int parameterSize)
        {
            DbParameter p1 = cmd.CreateParameter();
            p1.ParameterName = parameterName;
            if (parameterValue == null)//if ((parameterValue.HasValue == false))
                p1.Value = DBNull.Value;
            else
                p1.Value = parameterValue;
            p1.DbType = parameterType;
            p1.Direction = parameterDirection;
            p1.Size = parameterSize;
            cmd.Parameters.Add(p1);
            return p1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected DataTable ExecuteReader(DbCommand cmd)
        {
            DataTable dtable = null;
            bool _closeTheConnection = false;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    _closeTheConnection = true;
                }
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    dtable = new DataTable();
                    dtable.Load(reader);
                }
            }
            catch
            {
                if (dtable != null)
                {
                    dtable.Dispose();
                }
                throw;
            }
            finally
            {
                if (_closeTheConnection)
                {
                    cmd.Connection.Close();
                }
            }

            return dtable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nativeSqlStm"></param>
        /// <returns></returns>
        protected int ExecuteNativeSql(string nativeSqlStm)
        {
            if (nativeSqlStm == null) throw new ArgumentNullException("nativeSqlStm");

            DbCommand command = CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = nativeSqlStm;
            return ExecuteNonReader(command);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected int ExecuteNonReader(DbCommand cmd)
        {
            int returnValue = 0;
            bool _closeTheConnection = false;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    _closeTheConnection = true;
                }
                returnValue = cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_closeTheConnection)
                {
                    cmd.Connection.Close();
                }
            }

            return returnValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected Object ExecuteScalar(DbCommand cmd)
        {
            Object returnValue = null;
            bool _closeTheConnection = false;

            try
            {
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                    _closeTheConnection = true;
                }
                returnValue = cmd.ExecuteScalar();
            }
            finally
            {
                if (_closeTheConnection)
                {
                    cmd.Connection.Close();
                }
            }

            return returnValue;
        }





        /// <summary>
        /// Validates that the accessToken is a positive integer.
        /// </summary>
        /// <param name="accessToken"></param>
        protected void _ValidateAccessToken(Int32 accessToken)
        {
            if (accessToken <= 0) throw new VLInvalidAccessTokenException();
        }
    }
}
