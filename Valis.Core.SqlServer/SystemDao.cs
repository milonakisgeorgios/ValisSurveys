using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Valis.Core.Dal;
using Valis.Core.ViewModel;

namespace Valis.Core.SqlServer
{
    internal class SystemDao : SystemDaoBase
    {


        #region Login/Logout/Validation apparatus...
        protected override UserPassword GetPasswordWithFormatImpl(string logOnToken, bool updateLastActivityDate, DateTime currentTimeUtc)
        {
            DbCommand command = CreateCommand("dbo.security_GetPswdWithFormat");
            AddParameter(command, "@logOnToken", logOnToken, DbType.String);
            AddParameter(command, "@UpdateLastActivityDate", updateLastActivityDate, DbType.Boolean);
            AddParameter(command, "@CurrentTimeUtc", currentTimeUtc, DbType.DateTime);

            UserPassword userPswd = null;
            try
            {
                command.Connection.Open();

                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read())
                    {
                        userPswd = new UserPassword(reader);
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }

            return userPswd;
        }
        protected override void UpdateUserInfoImpl(string logOnToken, bool isPasswordCorrect, bool updateLastLoginActivityDate, int maxInvalidPasswordAttempts, int passwordAttemptWindow, DateTime currentTimeUtc, DateTime? lastLoginDate, DateTime? lastActivityDate)
        {
            DbCommand command = CreateCommand("dbo.security_UpdateUserInfo");
            AddParameter(command, "@LogOnToken", logOnToken, DbType.String);
            AddParameter(command, "@IsPasswordCorrect", isPasswordCorrect, DbType.Boolean);
            AddParameter(command, "@UpdateLastLoginActivityDate", updateLastLoginActivityDate, DbType.Boolean);
            AddParameter(command, "@MaxInvalidPasswordAttempts", maxInvalidPasswordAttempts, DbType.Int32);
            AddParameter(command, "@PasswordAttemptWindow", passwordAttemptWindow, DbType.Int32);
            AddParameter(command, "@CurrentTimeUtc", currentTimeUtc, DbType.DateTime);
            AddParameter(command, "@LastLoginDate", lastLoginDate, DbType.DateTime);
            AddParameter(command, "@LastActivityDate", lastActivityDate, DbType.DateTime);
            try
            {
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }
        }
        protected override VLAccessToken OpenAccessTokenImpl(Int32 principal, PrincipalType principalType, string ipAddress, DateTime currentTimeUtc)
        {
            DbCommand command = CreateCommand("dbo.security_AccessTokenOpen");
            AddParameter(command, "@principal", principal, DbType.Int32);
            AddParameter(command, "@principalType", principalType, DbType.Byte);
            AddParameter(command, "@ipAddress", ipAddress, DbType.String);
            AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
            //var p = AddParameter(command, "@ReturnValue", 0, DbType.Int32, ParameterDirection.ReturnValue);

            VLAccessToken accessToken = null;
            try
            {
                command.Connection.Open();

                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read() == false)
                        return null;

                    accessToken = new VLAccessToken(reader);
                }
                //int status = ((p.Value != null) ? ((int)p.Value) : -1);
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }

            return accessToken;
        }
        protected override void CloseAccessTokenImpl(Int32 accessTokenId, DateTime currentTimeUtc)
        {
            DbCommand command = CreateCommand("dbo.security_AccessTokenClose");
            AddParameter(command, "@accessTokenId", accessTokenId, DbType.Int32);
            AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

            ExecuteNonReader(command);
        }
        protected override VLAccessToken ValidateAccessTokenImpl(Int32 accessTokenId, DateTime currentTimeUtc)
        {
            DbCommand command = CreateCommand("dbo.security_AccessTokenValidate");
            AddParameter(command, "@accessTokenId", accessTokenId, DbType.Int32);
            AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

            VLAccessToken accessToken = null;
            try
            {
                command.Connection.Open();

                using (DbDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (reader.Read() == false)
                        return null;

                    accessToken = new VLAccessToken(reader);
                }
                //int status = ((p.Value != null) ? ((int)p.Value) : -1);
            }
            catch
            {
                throw;
            }
            finally
            {
                command.Connection.Close();
            }

            return accessToken;
        }
        #endregion



        internal override Collection<VLSystemParameter> GetSystemParametersImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetSystemParameters(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemParametersImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersImpl"), ex);
            }
        }
        internal override int GetSystemParametersCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemParametersCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersCountImpl"), ex);
            }
        }
        internal override Collection<VLSystemParameter> GetSystemParametersPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetSystemParameters(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemParametersPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersPagedImpl"), ex);
            }
        }
        internal override VLSystemParameter GetSystemParameterByIdImpl(Int32 accessToken, Guid parameterId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@parameterId", parameterId, DbType.Guid);


                return ExecuteAndGetSystemParameter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemParametersByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemParametersByIdImpl"), ex);
            }
        }
        internal override void DeleteSystemParameterImpl(Int32 accessToken, Guid parameterId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@parameterId", parameterId, DbType.Guid);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteSystemParameterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemParameterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemParameterImpl"), ex);
            }
        }
        internal override VLSystemParameter CreateSystemParameterImpl(Int32 accessToken, VLSystemParameter parameter, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@parameterId", parameter.ParameterId, DbType.Guid);
                AddParameter(command, "@parameterKey", parameter.ParameterKey, DbType.String);
                AddParameter(command, "@parameterValue", parameter.ParameterValue, DbType.String);
                AddParameter(command, "@parameterType", parameter.ParameterType, DbType.Byte);
                AddParameter(command, "@attributeFlags", parameter.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemParameter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateSystemParameterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemParameterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemParameterImpl"), ex);
            }
        }
        internal override VLSystemParameter UpdateSystemParameterImpl(Int32 accessToken, VLSystemParameter parameter, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemparameters_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@parameterId", parameter.ParameterId, DbType.Guid);
                AddParameter(command, "@parameterKey", parameter.ParameterKey, DbType.String);
                AddParameter(command, "@parameterValue", parameter.ParameterValue, DbType.String);
                AddParameter(command, "@parameterType", parameter.ParameterType, DbType.Byte);
                AddParameter(command, "@attributeFlags", parameter.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", parameter.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemParameter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateSystemParameterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemParameterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemParameterImpll"), ex);
            }
        }


        #region VLRole
        internal override Collection<VLRole> GetRolesImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select RoleId,Name,Description,Permissions,IsBuiltIn, IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy from (select * from Roles where RoleId > 9) as a {0} {1}", whereClause, orderByClause));
                command.CommandType = CommandType.Text;


                return ExecuteAndGetRoles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetRolesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesImpl"), ex);
            }
        }
        internal override Collection<VLRole> GetRolesExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select RoleId,Name,Description,Permissions,IsBuiltIn, IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy from Roles {0} {1}", whereClause, orderByClause));
                command.CommandType = CommandType.Text;


                return ExecuteAndGetRoles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetRolesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesImpl"), ex);
            }
        }
        internal override int GetRolesCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select count(*) from (select * from Roles where RoleId > 9) as a {0}", whereClause));
                command.CommandType = CommandType.Text;

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetRolesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesCountImpl"), ex);
            }
        }
        internal override Collection<VLRole> GetRolesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_roles_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetRoles(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetRolesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesPagedImpl"), ex);
            }
        }
        internal override VLRole GetRoleByIdImpl(Int32 accessToken, Int16 roleId)
        {
            try
            {
                DbCommand command = CreateCommand("select RoleId,Name,Description,Permissions,IsBuiltIn, IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy from Roles where RoleId = @RoleId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@roleId", roleId, DbType.Int16);


                return ExecuteAndGetRole(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetRolesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetRolesByIdImpl"), ex);
            }
        }
        internal override void DeleteRoleImpl(Int32 accessToken, Int16 roleId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_roles_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@roleId", roleId, DbType.Int16);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteRoleImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteRoleImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteRoleImpl"), ex);
            }
        }
        internal override VLRole CreateRoleImpl(Int32 accessToken, VLRole role, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_roles_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@roleId", role.RoleId, DbType.Int16);
                AddParameter(command, "@name", role.Name, DbType.String);
                AddParameter(command, "@description", role.Description, DbType.String);
                AddParameter(command, "@permissions", role.Permissions, DbType.Int64);
                AddParameter(command, "@isClientRole", role.IsClientRole, DbType.Boolean);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetRole(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateRoleImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateRoleImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateRoleImpl"), ex);
            }
        }
        internal override VLRole UpdateRoleImpl(Int32 accessToken, VLRole role, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_roles_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@roleId", role.RoleId, DbType.Int16);
                AddParameter(command, "@name", role.Name, DbType.String);
                AddParameter(command, "@description", role.Description, DbType.String);
                AddParameter(command, "@permissions", role.Permissions, DbType.Int64);
                AddParameter(command, "@IsClientRole", role.IsClientRole, DbType.Boolean);
                AddParameter(command, "@lastUpdateDT", role.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetRole(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateRoleImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateRoleImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateRoleImpll"), ex);
            }
        }
        #endregion

        #region VLCountry
        internal override Collection<VLCountry> GetCountriesImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCountries(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCountriesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesImpl"), ex);
            }
        }
        internal override int GetCountriesCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCountriesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesCountImpl"), ex);
            }
        }
        internal override Collection<VLCountry> GetCountriesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetCountries(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCountriesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountriesPagedImpl"), ex);
            }
        }
        internal override VLCountry GetCountryByIdImpl(Int32 accessToken, Int32 countryId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@countryId", countryId, DbType.Int32);


                return ExecuteAndGetCountry(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCountryByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountryByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCountryByIdImpl"), ex);
            }
        }
        internal override void DeleteCountryImpl(Int32 accessToken, Int32 countryId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@countryId", countryId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteCountryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCountryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCountryImpl"), ex);
            }
        }
        internal override VLCountry UpdateCountryImpl(Int32 accessToken, VLCountry country, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@countryId", country.CountryId, DbType.Int32);
                AddParameter(command, "@name", country.Name, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetCountry(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateCountryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCountryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCountryImpl"), ex);
            }
        }
        internal override VLCountry CreateCountryImpl(Int32 accessToken, VLCountry country, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_countries_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@name", country.Name, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetCountry(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateCountryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCountryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCountryImpl"), ex);
            }
        }
        #endregion

        #region VLEmailTemplate
        internal override Collection<VLEmailTemplate> GetEmailTemplatesImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetEmailTemplates(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetEmailTemplatesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesImpl"), ex);
            }
        }
        internal override int GetEmailTemplatesCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetEmailTemplatesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesCountImpl"), ex);
            }
        }
        internal override Collection<VLEmailTemplate> GetEmailTemplatesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetEmailTemplates(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetEmailTemplatesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesPagedImpl"), ex);
            }
        }
        internal override VLEmailTemplate GetEmailTemplateByIdImpl(Int32 accessToken, Int16 templateId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@templateId", templateId, DbType.Int16);


                return ExecuteAndGetEmailTemplate(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetEmailTemplatesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetEmailTemplatesByIdImpl"), ex);
            }
        }
        internal override void DeleteEmailTemplateImpl(Int32 accessToken, Int16 templateId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@templateId", templateId, DbType.Int16);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteEmailTemplateImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteEmailTemplateImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteEmailTemplateImpl"), ex);
            }
        }
        internal override VLEmailTemplate CreateEmailTemplateImpl(Int32 accessToken, VLEmailTemplate template, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@templateId", template.TemplateId, DbType.Int16);
                AddParameter(command, "@name", template.Name, DbType.String);
                AddParameter(command, "@sender", template.Sender, DbType.String);
                AddParameter(command, "@subject", template.Subject, DbType.String);
                AddParameter(command, "@body", template.Body, DbType.String);
                AddParameter(command, "@attributeFlags", template.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetEmailTemplate(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateEmailTemplateImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateEmailTemplateImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateEmailTemplateImpl"), ex);
            }
        }
        internal override VLEmailTemplate UpdateEmailTemplateImpl(Int32 accessToken, VLEmailTemplate template, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_emailtemplates_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@templateId", template.TemplateId, DbType.Int16);
                AddParameter(command, "@name", template.Name, DbType.String);
                AddParameter(command, "@sender", template.Sender, DbType.String);
                AddParameter(command, "@subject", template.Subject, DbType.String);
                AddParameter(command, "@body", template.Body, DbType.String);
                AddParameter(command, "@attributeFlags", template.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", template.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetEmailTemplate(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateEmailTemplateImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateEmailTemplateImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateEmailTemplateImpll"), ex);
            }
        }
        
        #endregion

        #region VLSystemEmail

        internal override Collection<VLSystemEmail> GetSystemEmailsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetSystemEmails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemEmailsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsImpl"), ex);
            }
        }
        internal override Collection<VLSystemEmail> GetPendingSystemEmailsImpl(int accessToken, int maxRows, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_GetPending");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@maxRows", maxRows, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetSystemEmails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPendingSystemEmailsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPendingSystemEmailsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPendingSystemEmailsImpl"), ex);
            }
        }
        internal override int GetSystemEmailsCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemEmailsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsCountImpl"), ex);
            }
        }
        internal override Collection<VLSystemEmail> GetSystemEmailsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetSystemEmails(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemEmailsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsPagedImpl"), ex);
            }
        }
        internal override VLSystemEmail GetSystemEmailByIdImpl(Int32 accessToken, Int32 emailId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailId", emailId, DbType.Int32);


                return ExecuteAndGetSystemEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemEmailsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemEmailsByIdImpl"), ex);
            }
        }
        internal override void DeleteSystemEmailImpl(Int32 accessToken, Int32 emailId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailId", emailId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteSystemEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemEmailImpl"), ex);
            }
        }
        internal override VLSystemEmail CreateSystemEmailImpl(Int32 accessToken, VLSystemEmail email, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailAccessToken", email.AccessToken, DbType.Int32);
                AddParameter(command, "@moduleName", email.ModuleName, DbType.String);
                AddParameter(command, "@enterDt", email.EnterDt, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", email.AttributeFlags, DbType.Int16);
                AddParameter(command, "@fromAddress", email.FromAddress, DbType.String);
                AddParameter(command, "@fromDisplayName", email.FromDisplayName, DbType.String);
                AddParameter(command, "@toAddress", email.ToAddress, DbType.String);
                AddParameter(command, "@subject", email.Subject, DbType.String);
                AddParameter(command, "@body", email.Body, DbType.String);
                AddParameter(command, "@status", email.Status, DbType.Byte);
                AddParameter(command, "@sendDT", email.SendDT, DbType.DateTime2);
                AddParameter(command, "@error", email.Error, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateSystemEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemEmailImpl"), ex);
            }
        }
        internal override VLSystemEmail UpdateSystemEmailImpl(Int32 accessToken, VLSystemEmail email, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systememails_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailId", email.EmailId, DbType.Int32);
                AddParameter(command, "@attributeFlags", email.AttributeFlags, DbType.Int16);
                AddParameter(command, "@status", email.Status, DbType.Byte);
                AddParameter(command, "@sendDT", email.SendDT, DbType.DateTime2);
                AddParameter(command, "@error", email.Error, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateSystemEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemEmailImpll"), ex);
            }
        }
        
        #endregion

        #region VLSystemUser
        internal override Collection<VLSystemUser> GetSystemUsersExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetSystemUsers(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemUsersExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersExImpl"), ex);
            }
        }
        internal override Collection<VLSystemUser> GetSystemUsersImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetSystemUsers(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemUsersImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersImpl"), ex);
            }
        }
        internal override int GetSystemUsersCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemUsersCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersCountImpl"), ex);
            }
        }
        internal override Collection<VLSystemUser> GetSystemUsersPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetSystemUsers(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemUsersPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersPagedImpl"), ex);
            }
        }
        internal override VLSystemUser GetSystemUserByIdImpl(Int32 accessToken, Int32 userId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@userId", userId, DbType.Int32);


                return ExecuteAndGetSystemUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetSystemUsersByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetSystemUsersByIdImpl"), ex);
            }
        }
        internal override void DeleteSystemUserImpl(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@userId", userId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteSystemUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteSystemUserImpl"), ex);
            }
        }
        internal override VLSystemUser CreateSystemUserImpl(Int32 accessToken, VLSystemUser systemUser, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@defaultLanguage", systemUser.DefaultLanguage, DbType.Int16);
                AddParameter(command, "@firstName", systemUser.FirstName, DbType.String);
                AddParameter(command, "@lastName", systemUser.LastName, DbType.String);
                AddParameter(command, "@email", systemUser.Email, DbType.String);
                AddParameter(command, "@timeZoneId", systemUser.TimeZoneId, DbType.String);
                AddParameter(command, "@isActive", systemUser.IsActive, DbType.Boolean);
                AddParameter(command, "@isBuiltIn", systemUser.IsBuiltIn, DbType.Boolean);
                AddParameter(command, "@attributeFlags", systemUser.AttributeFlags, DbType.Int32);
                AddParameter(command, "@role", systemUser.Role, DbType.Int16);
                AddParameter(command, "@notes", systemUser.Notes, DbType.String);
                AddParameter(command, "@lastActivityDate", systemUser.LastActivityDate, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateSystemUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateSystemUserImpl"), ex);
            }
        }
        internal override VLSystemUser UpdateSystemUserImpl(Int32 accessToken, VLSystemUser systemUser, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@userId", systemUser.UserId, DbType.Int32);
                AddParameter(command, "@defaultLanguage", systemUser.DefaultLanguage, DbType.Int16);
                AddParameter(command, "@firstName", systemUser.FirstName, DbType.String);
                AddParameter(command, "@lastName", systemUser.LastName, DbType.String);
                AddParameter(command, "@email", systemUser.Email, DbType.String);
                AddParameter(command, "@timeZoneId", systemUser.TimeZoneId, DbType.String);
                AddParameter(command, "@isActive", systemUser.IsActive, DbType.Boolean);
                AddParameter(command, "@isBuiltIn", systemUser.IsBuiltIn, DbType.Boolean);
                AddParameter(command, "@attributeFlags", systemUser.AttributeFlags, DbType.Int32);
                AddParameter(command, "@role", systemUser.Role, DbType.Int16);
                AddParameter(command, "@notes", systemUser.Notes, DbType.String);
                AddParameter(command, "@lastActivityDate", systemUser.LastActivityDate, DbType.DateTime2);
                AddParameter(command, "@lastUpdateDT", systemUser.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSystemUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateSystemUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateSystemUserImpll"), ex);
            }
        }
        #endregion

        #region VLCredential
        internal override Collection<VLCredential> GetCredentialsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_credentials_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCredentials(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCredentialsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCredentialsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCredentialsImpl"), ex);
            }
        }

        internal override VLCredential GetCredentialByIdImpl(Int32 accessToken, Int32 credentialId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_credentials_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@credentialId", credentialId, DbType.Int32);


                return ExecuteAndGetCredential(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCredentialsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCredentialsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCredentialsByIdImpl"), ex);
            }
        }
        internal override void DeleteCredentialImpl(Int32 accessToken, Int32 credentialId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_credentials_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@credentialId", credentialId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteCredentialImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCredentialImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCredentialImpl"), ex);
            }
        }
        internal override VLCredential CreateCredentialImpl(Int32 accessToken, VLCredential credentials, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_credentials_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@credentialId", credentials.CredentialId, DbType.Int32);
                AddParameter(command, "@principal", credentials.Principal, DbType.Int32);
                AddParameter(command, "@principalType", credentials.PrincipalType, DbType.Byte);
                AddParameter(command, "@logOnToken", credentials.LogOnToken, DbType.String);
                AddParameter(command, "@pswdToken", credentials.PswdToken, DbType.String);
                AddParameter(command, "@pswdFormat", credentials.PswdFormat, DbType.Int32);
                AddParameter(command, "@pswdSalt", credentials.PswdSalt, DbType.String);
                AddParameter(command, "@pswdQuestion", credentials.PswdQuestion, DbType.String);
                AddParameter(command, "@pswdAnswer", credentials.PswdAnswer, DbType.String);
                AddParameter(command, "@isApproved", credentials.IsApproved, DbType.Boolean);
                AddParameter(command, "@isLockedOut", credentials.IsLockedOut, DbType.Boolean);
                AddParameter(command, "@lastLoginDate", credentials.LastLoginDate, DbType.DateTime2);
                AddParameter(command, "@lastPasswordChangedDate", credentials.LastPasswordChangedDate, DbType.DateTime2);
                AddParameter(command, "@lastLockoutDate", credentials.LastLockoutDate, DbType.DateTime2);
                AddParameter(command, "@failedPasswordAttemptCount", credentials.FailedPasswordAttemptCount, DbType.Int32);
                AddParameter(command, "@failedPasswordAttemptWindowStart", credentials.FailedPasswordAttemptWindowStart, DbType.DateTime2);
                AddParameter(command, "@failedPasswordAnswerAttemptCount", credentials.FailedPasswordAnswerAttemptCount, DbType.Int32);
                AddParameter(command, "@failedPasswordAnswerAttemptWindowStart", credentials.FailedPasswordAnswerAttemptWindowStart, DbType.DateTime2);
                AddParameter(command, "@comment", credentials.Comment, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetCredential(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateCredentialImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCredentialImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCredentialImpl"), ex);
            }
        }
        internal override VLCredential UpdateCredentialImpl(Int32 accessToken, VLCredential credentials, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_credentials_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@credentialId", credentials.CredentialId, DbType.Int32);
                AddParameter(command, "@principal", credentials.Principal, DbType.Int32);
                AddParameter(command, "@principalType", credentials.PrincipalType, DbType.Byte);
                AddParameter(command, "@logOnToken", credentials.LogOnToken, DbType.String);
                AddParameter(command, "@pswdToken", credentials.PswdToken, DbType.String);
                AddParameter(command, "@pswdFormat", credentials.PswdFormat, DbType.Int32);
                AddParameter(command, "@pswdSalt", credentials.PswdSalt, DbType.String);
                AddParameter(command, "@pswdQuestion", credentials.PswdQuestion, DbType.String);
                AddParameter(command, "@pswdAnswer", credentials.PswdAnswer, DbType.String);
                AddParameter(command, "@isApproved", credentials.IsApproved, DbType.Boolean);
                AddParameter(command, "@isLockedOut", credentials.IsLockedOut, DbType.Boolean);
                AddParameter(command, "@lastLoginDate", credentials.LastLoginDate, DbType.DateTime2);
                AddParameter(command, "@lastPasswordChangedDate", credentials.LastPasswordChangedDate, DbType.DateTime2);
                AddParameter(command, "@lastLockoutDate", credentials.LastLockoutDate, DbType.DateTime2);
                AddParameter(command, "@failedPasswordAttemptCount", credentials.FailedPasswordAttemptCount, DbType.Int32);
                AddParameter(command, "@failedPasswordAttemptWindowStart", credentials.FailedPasswordAttemptWindowStart, DbType.DateTime2);
                AddParameter(command, "@failedPasswordAnswerAttemptCount", credentials.FailedPasswordAnswerAttemptCount, DbType.Int32);
                AddParameter(command, "@failedPasswordAnswerAttemptWindowStart", credentials.FailedPasswordAnswerAttemptWindowStart, DbType.DateTime2);
                AddParameter(command, "@comment", credentials.Comment, DbType.String);
                AddParameter(command, "@lastUpdateDT", credentials.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetCredential(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateCredentialImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCredentialImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCredentialImpll"), ex);
            }
        }
        #endregion

        #region VLClient
        internal override Collection<VLClient> GetClientsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClients(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsImpl"), ex);
            }
        }
        internal override int GetClientsCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsCountImpl"), ex);
            }
        }
        internal override Collection<VLClient> GetClientsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClients(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsPagedImpl"), ex);
            }
        }
        internal override Collection<VLClientEx> GetClientExsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetPageEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClientExs(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientExsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientExsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientExsPagedImpl"), ex);
            }
        }
        internal override VLClient GetClientByIdImpl(Int32 accessToken, Int32 clientId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);


                return ExecuteAndGetClient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientsByIdImpl"), ex);
            }
        }

        internal override VLClient GetClientForCollectorImpl(Int32 accessToken, Int32 collectorId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);


                return ExecuteAndGetClient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientforCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientforCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientforCollectorImpl"), ex);
            }
        }
        internal override VLClient GetClientForSurveyImpl(Int32 accessToken, Int32 surveyId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_GetForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);


                return ExecuteAndGetClient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientForSurveyImpl"), ex);
            }
        }
        internal override void DeleteClientImpl(Int32 accessToken, Int32 clientId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientImpl"), ex);
            }
        }
        internal override VLClient CreateClientImpl(Int32 accessToken, VLClient client, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", client.ClientId, DbType.Int32);
                AddParameter(command, "@code", client.Code, DbType.String);
                AddParameter(command, "@name", client.Name, DbType.String);
                AddParameter(command, "@profession", client.Profession, DbType.String);
                AddParameter(command, "@country", client.Country, DbType.Int32);
                AddParameter(command, "@timeZoneId", client.TimeZoneId, DbType.String);
                AddParameter(command, "@prefecture", client.Prefecture, DbType.String);
                AddParameter(command, "@town", client.Town, DbType.String);
                AddParameter(command, "@address", client.Address, DbType.String);
                AddParameter(command, "@zip", client.Zip, DbType.String);
                AddParameter(command, "@telephone1", client.Telephone1, DbType.String);
                AddParameter(command, "@telephone2", client.Telephone2, DbType.String);
                AddParameter(command, "@webSite", client.WebSite, DbType.String);
                AddParameter(command, "@attributeFlags", client.AttributeFlags, DbType.Int32);
                AddParameter(command, "@profile", client.Profile, DbType.Byte);
                AddParameter(command, "@comment", client.Comment, DbType.String);
                AddParameter(command, "@folderSequence", client.FolderSequence, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientImpl"), ex);
            }
        }
        internal override VLClient UpdateClientImpl(Int32 accessToken, VLClient client, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clients_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", client.ClientId, DbType.Int32);
                AddParameter(command, "@code", client.Code, DbType.String);
                AddParameter(command, "@name", client.Name, DbType.String);
                AddParameter(command, "@profession", client.Profession, DbType.String);
                AddParameter(command, "@country", client.Country, DbType.Int32);
                AddParameter(command, "@timeZoneId", client.TimeZoneId, DbType.String);
                AddParameter(command, "@prefecture", client.Prefecture, DbType.String);
                AddParameter(command, "@town", client.Town, DbType.String);
                AddParameter(command, "@address", client.Address, DbType.String);
                AddParameter(command, "@zip", client.Zip, DbType.String);
                AddParameter(command, "@telephone1", client.Telephone1, DbType.String);
                AddParameter(command, "@telephone2", client.Telephone2, DbType.String);
                AddParameter(command, "@webSite", client.WebSite, DbType.String);
                AddParameter(command, "@attributeFlags", client.AttributeFlags, DbType.Int32);
                AddParameter(command, "@profile", client.Profile, DbType.Byte);
                AddParameter(command, "@comment", client.Comment, DbType.String);
                AddParameter(command, "@folderSequence", client.FolderSequence, DbType.Int16);
                AddParameter(command, "@lastUpdateDT", client.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientImpll"), ex);
            }
        }
        #endregion

        #region VLClientProfile
        internal override Collection<VLClientProfile> GetClientProfilesExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClientProfiles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientProfilesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesImpl"), ex);
            }
        }
        internal override Collection<VLClientProfile> GetClientProfilesImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClientProfiles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientProfilesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesImpl"), ex);
            }
        }
        internal override int GetClientProfilesCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientProfilesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesCountImpl"), ex);
            }
        }
        internal override Collection<VLClientProfile> GetClientProfilesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClientProfiles(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientProfilesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesPagedImpl"), ex);
            }
        }
        internal override VLClientProfile GetClientProfileByIdImpl(Int32 accessToken, Int32 profileId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@profileId", profileId, DbType.Int32);


                return ExecuteAndGetClientProfile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientProfilesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientProfilesByIdImpl"), ex);
            }
        }
        internal override void DeleteClientProfileImpl(Int32 accessToken, Int32 profileId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@profileId", profileId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteClientProfileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientProfileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientProfileImpl"), ex);
            }
        }
        internal override VLClientProfile CreateClientProfileImpl(Int32 accessToken, VLClientProfile profile, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@profileId", profile.ProfileId, DbType.Int32);
                AddParameter(command, "@name", profile.Name, DbType.String);
                AddParameter(command, "@comment", profile.Comment, DbType.String);
                AddParameter(command, "@maxNumberOfUsers", profile.MaxNumberOfUsers, DbType.Int32);
                AddParameter(command, "@maxNumberOfSurveys", profile.MaxNumberOfSurveys, DbType.Int32);
                AddParameter(command, "@maxNumberOfLists", profile.MaxNumberOfLists, DbType.Int32);
                AddParameter(command, "@maxNumberOfRecipientsPerList", profile.MaxNumberOfRecipientsPerList, DbType.Int32);
                AddParameter(command, "@maxNumberOfRecipientsPerMessage", profile.MaxNumberOfRecipientsPerMessage, DbType.Int32);
                AddParameter(command, "@maxNumberOfCollectorsPerSurvey", profile.MaxNumberOfCollectorsPerSurvey, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerDay", profile.MaxNumberOfEmailsPerDay, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerWeek", profile.MaxNumberOfEmailsPerWeek, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerMonth", profile.MaxNumberOfEmailsPerMonth, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmails", profile.MaxNumberOfEmails, DbType.Int32);
                AddParameter(command, "@attributeFlags", profile.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientProfile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateClientProfileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientProfileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientProfileImpl"), ex);
            }
        }
        internal override VLClientProfile UpdateClientProfileImpl(Int32 accessToken, VLClientProfile profile, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientprofiles_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@profileId", profile.ProfileId, DbType.Int32);
                AddParameter(command, "@name", profile.Name, DbType.String);
                AddParameter(command, "@comment", profile.Comment, DbType.String);
                AddParameter(command, "@maxNumberOfUsers", profile.MaxNumberOfUsers, DbType.Int32);
                AddParameter(command, "@maxNumberOfSurveys", profile.MaxNumberOfSurveys, DbType.Int32);
                AddParameter(command, "@maxNumberOfLists", profile.MaxNumberOfLists, DbType.Int32);
                AddParameter(command, "@maxNumberOfRecipientsPerList", profile.MaxNumberOfRecipientsPerList, DbType.Int32);
                AddParameter(command, "@maxNumberOfRecipientsPerMessage", profile.MaxNumberOfRecipientsPerMessage, DbType.Int32);
                AddParameter(command, "@maxNumberOfCollectorsPerSurvey", profile.MaxNumberOfCollectorsPerSurvey, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerDay", profile.MaxNumberOfEmailsPerDay, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerWeek", profile.MaxNumberOfEmailsPerWeek, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmailsPerMonth", profile.MaxNumberOfEmailsPerMonth, DbType.Int32);
                AddParameter(command, "@maxNumberOfEmails", profile.MaxNumberOfEmails, DbType.Int32);
                AddParameter(command, "@attributeFlags", profile.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", profile.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientProfile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateClientProfileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientProfileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientProfileImpll"), ex);
            }
        }
        
        #endregion

        #region VLClientUser
        internal override Collection<VLClientUser> GetClientUsersImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClientUsers(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientUsersImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersImpl"), ex);
            }
        }
        internal override Collection<VLClientUser> GetClientUsersImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClientUsers(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientUsersImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersImpl"), ex);
            }
        }
        internal override int GetClientUsersCountImpl(Int32 accessToken, Int32 clientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientUsersCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersCountImpl"), ex);
            }
        }
        internal override Collection<VLClientUser> GetClientUsersPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClientUsers(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientUsersPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersPagedImpl"), ex);
            }
        }
        internal override VLClientUser GetClientUserByIdImpl(Int32 accessToken, Int32 userId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@userId", userId, DbType.Int32);


                return ExecuteAndGetClientUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientUsersByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientUsersByIdImpl"), ex);
            }
        }
        internal override void DeleteClientUserImpl(Int32 accessToken, Int32 userId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@userId", userId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteClientUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientUserImpl"), ex);
            }
        }
        internal override VLClientUser CreateClientUserImpl(Int32 accessToken, VLClientUser clientUser, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientUser.Client, DbType.Int32);
                AddParameter(command, "@defaultLanguage", clientUser.DefaultLanguage, DbType.Int16);
                AddParameter(command, "@title", clientUser.Title, DbType.String);
                AddParameter(command, "@department", clientUser.Department, DbType.String);
                AddParameter(command, "@firstName", clientUser.FirstName, DbType.String);
                AddParameter(command, "@lastName", clientUser.LastName, DbType.String);
                AddParameter(command, "@country", clientUser.Country, DbType.Int32);
                AddParameter(command, "@timeZoneId", clientUser.TimeZoneId, DbType.String);
                AddParameter(command, "@prefecture", clientUser.Prefecture, DbType.String);
                AddParameter(command, "@town", clientUser.Town, DbType.String);
                AddParameter(command, "@address", clientUser.Address, DbType.String);
                AddParameter(command, "@zip", clientUser.Zip, DbType.String);
                AddParameter(command, "@telephone1", clientUser.Telephone1, DbType.String);
                AddParameter(command, "@telephone2", clientUser.Telephone2, DbType.String);
                AddParameter(command, "@email", clientUser.Email, DbType.String);
                AddParameter(command, "@isActive", clientUser.IsActive, DbType.Boolean);
                AddParameter(command, "@isBuiltIn", clientUser.IsBuiltIn, DbType.Boolean);
                AddParameter(command, "@attributeFlags", clientUser.AttributeFlags, DbType.Int32);
                AddParameter(command, "@role", clientUser.Role, DbType.Int16);
                AddParameter(command, "@comment", clientUser.Comment, DbType.String);
                AddParameter(command, "@lastActivityDate", clientUser.LastActivityDate, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateClientUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientUserImpl"), ex);
            }
        }
        internal override VLClientUser UpdateClientUserImpl(Int32 accessToken, VLClientUser clientUser, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientUser.Client, DbType.Int32);
                AddParameter(command, "@userId", clientUser.UserId, DbType.Int32);
                AddParameter(command, "@defaultLanguage", clientUser.DefaultLanguage, DbType.Int16);
                AddParameter(command, "@title", clientUser.Title, DbType.String);
                AddParameter(command, "@department", clientUser.Department, DbType.String);
                AddParameter(command, "@firstName", clientUser.FirstName, DbType.String);
                AddParameter(command, "@lastName", clientUser.LastName, DbType.String);
                AddParameter(command, "@country", clientUser.Country, DbType.Int32);
                AddParameter(command, "@timeZoneId", clientUser.TimeZoneId, DbType.String);
                AddParameter(command, "@prefecture", clientUser.Prefecture, DbType.String);
                AddParameter(command, "@town", clientUser.Town, DbType.String);
                AddParameter(command, "@address", clientUser.Address, DbType.String);
                AddParameter(command, "@zip", clientUser.Zip, DbType.String);
                AddParameter(command, "@telephone1", clientUser.Telephone1, DbType.String);
                AddParameter(command, "@telephone2", clientUser.Telephone2, DbType.String);
                AddParameter(command, "@email", clientUser.Email, DbType.String);
                AddParameter(command, "@isActive", clientUser.IsActive, DbType.Boolean);
                AddParameter(command, "@isBuiltIn", clientUser.IsBuiltIn, DbType.Boolean);
                AddParameter(command, "@attributeFlags", clientUser.AttributeFlags, DbType.Int32);
                AddParameter(command, "@role", clientUser.Role, DbType.Int16);
                AddParameter(command, "@comment", clientUser.Comment, DbType.String);
                AddParameter(command, "@lastActivityDate", clientUser.LastActivityDate, DbType.DateTime2);
                AddParameter(command, "@lastUpdateDT", clientUser.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientUser(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateClientUserImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientUserImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientUserImpll"), ex);
            }
        }
        #endregion

        #region VLKnownEmail
        internal override Collection<VLKnownEmail> GetKnownEmailsExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetKnownEmails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailsExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsExImpl"), ex);
            }
        }
        internal override Collection<VLKnownEmail> GetKnownEmailsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetKnownEmails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsImpl"), ex);
            }
        }
        internal override int GetKnownEmailsCountImpl(Int32 accessToken, Int32 clientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsCountImpl"), ex);
            }
        }
        internal override Collection<VLKnownEmail> GetKnownEmailsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetKnownEmails(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailsPagedImpl"), ex);
            }
        }
        internal override Collection<VLKnownEmailEx> GetKnownEmailExsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetPageEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetKnownEmailExs(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailExsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailExsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailExsPagedImpl"), ex);
            }
        }
        internal override VLKnownEmail GetKnownEmailByIdImpl(Int32 accessToken, Int32 emailId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailId", emailId, DbType.Int32);


                return ExecuteAndGetVerifiedEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetKnownEmailByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetKnownEmailByIdImpl"), ex);
            }
        }
        internal override void DeleteKnownEmailImpl(Int32 accessToken, Int32 emailId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@emailId", emailId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteKnownEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteKnownEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteKnownEmailImpl"), ex);
            }
        }
        internal override VLKnownEmail CreateKnownEmailImpl(Int32 accessToken, VLKnownEmail email, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", email.Client, DbType.Int32);
                AddParameter(command, "@emailId", email.EmailId, DbType.Int32);
                AddParameter(command, "@emailAddress", email.EmailAddress, DbType.String);
                AddParameter(command, "@localPart", email.LocalPart, DbType.String);
                AddParameter(command, "@domainPart", email.DomainPart, DbType.String);
                AddParameter(command, "@registerDt", email.RegisterDt, DbType.DateTime2);
                AddParameter(command, "@isDomainOK", email.IsDomainOK, DbType.Boolean);
                AddParameter(command, "@isVerified", email.IsVerified, DbType.Boolean);
                AddParameter(command, "@isOptedOut", email.IsOptedOut, DbType.Boolean);
                AddParameter(command, "@isBounced", email.IsBounced, DbType.Boolean);
                AddParameter(command, "@verifiedDt", email.VerifiedDt, DbType.DateTime2);
                AddParameter(command, "@optedOutDt", email.OptedOutDt, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetVerifiedEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateKnownEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateKnownEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateKnownEmailImpl"), ex);
            }
        }
        internal override VLKnownEmail UpdateKnownEmailImpl(Int32 accessToken, VLKnownEmail email, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_knownemails_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", email.Client, DbType.Int32);
                AddParameter(command, "@emailId", email.EmailId, DbType.Int32);
                AddParameter(command, "@emailAddress", email.EmailAddress, DbType.String);
                AddParameter(command, "@localPart", email.LocalPart, DbType.String);
                AddParameter(command, "@domainPart", email.DomainPart, DbType.String);
                AddParameter(command, "@registerDt", email.RegisterDt, DbType.DateTime2);
                AddParameter(command, "@isDomainOK", email.IsDomainOK, DbType.Boolean);
                AddParameter(command, "@isVerified", email.IsVerified, DbType.Boolean);
                AddParameter(command, "@isOptedOut", email.IsOptedOut, DbType.Boolean);
                AddParameter(command, "@isBounced", email.IsBounced, DbType.Boolean);
                AddParameter(command, "@verifiedDt", email.VerifiedDt, DbType.DateTime2);
                AddParameter(command, "@optedOutDt", email.OptedOutDt, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetVerifiedEmail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateKnownEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateKnownEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateKnownEmailImpl"), ex);
            }
        }
        #endregion

        #region VLPayment
        internal override Collection<VLBalance> GetBalancesImpl(Int32 accessToken, Int32 clientId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_GetBalances");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);


                return ExecuteAndGetBalances(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetBalancesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetBalancesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetBalancesImpl"), ex);
            }
        }
        internal override Collection<VLPayment> GetPaymentsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetPayments(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsImpl"), ex);
            }
        }        
        internal override int GetPaymentsCountImpl(Int32 accessToken, Int32 clientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsCountImpl"), ex);
            }
        }
        internal override Collection<VLPayment> GetPaymentsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetPayments(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsPagedImpl"), ex);
            }
        }
        internal override Collection<VLPaymentView1> GetPaymentsView1PagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_View1_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetPaymentsView1(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentsView1PagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsView1PagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsView1PagedImpl"), ex);
            }
        }
        internal override VLPayment GetPaymentByIdImpl(Int32 accessToken, Int32 paymentId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", paymentId, DbType.Int32);


                return ExecuteAndGetPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentByIdImpl"), ex);
            }
        }
        internal override void DeletePaymentImpl(Int32 accessToken, Int32 paymentId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", paymentId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeletePaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeletePaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeletePaymentImpl"), ex);
            }
        }
        internal override VLPayment CreatePaymentImpl(Int32 accessToken, VLPayment payment, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", payment.PaymentId, DbType.Int32);
                AddParameter(command, "@client", payment.Client, DbType.Int32);
                AddParameter(command, "@comment", payment.Comment, DbType.String);
                AddParameter(command, "@paymentType", payment.PaymentType, DbType.Byte);
                AddParameter(command, "@paymentDate", payment.PaymentDate, DbType.DateTime2);
                AddParameter(command, "@customCode1", payment.CustomCode1, DbType.String);
                AddParameter(command, "@customCode2", payment.CustomCode2, DbType.String);
                AddParameter(command, "@isActive", payment.IsActive, DbType.Boolean);
                AddParameter(command, "@isTimeLimited", payment.IsTimeLimited, DbType.Boolean);
                AddParameter(command, "@validFromDt", payment.ValidFromDt, DbType.DateTime2);
                AddParameter(command, "@validToDt", payment.ValidToDt, DbType.DateTime2);
                AddParameter(command, "@creditType", payment.CreditType, DbType.Byte);
                AddParameter(command, "@quantity", payment.Quantity, DbType.Int32);
                AddParameter(command, "@quantityUsed", payment.QuantityUsed, DbType.Int32);
                AddParameter(command, "@attributeFlags", payment.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreatePaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreatePaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreatePaymentImpl"), ex);
            }
        }
        internal override VLPayment UpdatePaymentImpl(Int32 accessToken, VLPayment payment, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", payment.PaymentId, DbType.Int32);
                AddParameter(command, "@client", payment.Client, DbType.Int32);
                AddParameter(command, "@comment", payment.Comment, DbType.String);
                AddParameter(command, "@paymentType", payment.PaymentType, DbType.Byte);
                AddParameter(command, "@paymentDate", payment.PaymentDate, DbType.DateTime2);
                AddParameter(command, "@customCode1", payment.CustomCode1, DbType.String);
                AddParameter(command, "@customCode2", payment.CustomCode2, DbType.String);
                AddParameter(command, "@isActive", payment.IsActive, DbType.Boolean);
                AddParameter(command, "@isTimeLimited", payment.IsTimeLimited, DbType.Boolean);
                AddParameter(command, "@validFromDt", payment.ValidFromDt, DbType.DateTime2);
                AddParameter(command, "@validToDt", payment.ValidToDt, DbType.DateTime2);
                AddParameter(command, "@creditType", payment.CreditType, DbType.Byte);
                AddParameter(command, "@quantity", payment.Quantity, DbType.Int32);
                AddParameter(command, "@quantityUsed", payment.QuantityUsed, DbType.Int32);
                AddParameter(command, "@attributeFlags", payment.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", payment.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdatePaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdatePaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdatePaymentImpl"), ex);
            }
        }
        
        #endregion

        #region VLCollectorPayment

        internal override Collection<VLCollectorPayment> GetCollectorPaymentsForPaymentImpl(Int32 accessToken, Int32 paymentId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetAll_ForPayment");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", paymentId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCollectorPayments(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsForPaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForPaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForPaymentImpl"), ex);
            }
        }
        internal override Collection<VLCollectorPayment> GetCollectorPaymentsForSurveyImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetAll_ForSurvey");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCollectorPayments(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForSurveyImpl"), ex);
            }
        }
        internal override int GetCollectorPaymentsCountForSurveyImpl(Int32 accessToken, Int32 surveyId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetTotalRows_ForSurvey");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsCountForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsCountForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsCountForSurveyImpl"), ex);
            }
        }
        internal override Collection<VLCollectorPayment> GetCollectorPaymentsPagedForSurveyImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetPage_ForSurvey");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetCollectorPayments(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsPagedForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsPagedForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsPagedForSurveyImpl"), ex);
            }
        }
        

        internal override Collection<VLCollectorPayment> GetCollectorPaymentsForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetAll_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCollectorPayments(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsForCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsForCollectorImpl"), ex);
            }
        }
        internal override int GetCollectorPaymentsCountForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetTotalRows_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsCountForCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsCountForCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsCountForCollectorImpl"), ex);
            }
        }
        internal override Collection<VLCollectorPayment> GetCollectorPaymentsPagedForCollectorImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetPage_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetCollectorPayments(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsPagedForCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsPagedForCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsPagedForCollectorImpl"), ex);
            }
        }
        
        
        internal override VLCollectorPayment GetCollectorPaymentByIdImpl(Int32 accessToken, Int32 collectorPaymentId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorPaymentId", collectorPaymentId, DbType.Int32);


                return ExecuteAndGetCollectorPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentsByIdImpl"), ex);
            }
        }
        internal override VLCollectorPayment GetCollectorPaymentByCollectorAndPaymentImpl(Int32 accessToken, Int32 collectorId, Int32 paymentId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_GetByCollectorAndPayment");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@payment", paymentId, DbType.Int32);


                return ExecuteAndGetCollectorPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetCollectorPaymentByCollectorAndPaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentByCollectorAndPaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetCollectorPaymentByCollectorAndPaymentImpl"), ex);
            }
        }


        internal override void DeleteCollectorPaymentImpl(Int32 accessToken, Int32 collectorPaymentId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorPaymentId", collectorPaymentId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteCollectorPaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCollectorPaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteCollectorPaymentImpl"), ex);
            }
        }
        internal override VLCollectorPayment CreateCollectorPaymentImpl(Int32 accessToken, VLCollectorPayment collectorPayment, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorPaymentId", collectorPayment.CollectorPaymentId, DbType.Int32);
                AddParameter(command, "@collector", collectorPayment.Collector, DbType.Int32);
                AddParameter(command, "@payment", collectorPayment.Payment, DbType.Int32);
                AddParameter(command, "@useOrder", collectorPayment.UseOrder, DbType.Int16);
                AddParameter(command, "@quantityLimit", collectorPayment.QuantityLimit, DbType.Int32);
                AddParameter(command, "@quantityReserved", collectorPayment.QuantityReserved, DbType.Int32);
                AddParameter(command, "@quantityUsed", collectorPayment.QuantityUsed, DbType.Int32);
                AddParameter(command, "@firstChargeDt", collectorPayment.FirstChargeDt, DbType.DateTime2);
                AddParameter(command, "@lastChargeDt", collectorPayment.LastChargeDt, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", collectorPayment.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetCollectorPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateCollectorPaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCollectorPaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateCollectorPaymentImpl"), ex);
            }
        }
        internal override VLCollectorPayment UpdateCollectorPaymentImpl(Int32 accessToken, VLCollectorPayment collectorPayment, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_collectorpayments_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorPaymentId", collectorPayment.CollectorPaymentId, DbType.Int32);
                AddParameter(command, "@collector", collectorPayment.Collector, DbType.Int32);
                AddParameter(command, "@payment", collectorPayment.Payment, DbType.Int32);
                AddParameter(command, "@useOrder", collectorPayment.UseOrder, DbType.Int16);
                AddParameter(command, "@quantityLimit", collectorPayment.QuantityLimit, DbType.Int32);
                AddParameter(command, "@quantityReserved", collectorPayment.QuantityReserved, DbType.Int32);
                AddParameter(command, "@quantityUsed", collectorPayment.QuantityUsed, DbType.Int32);
                AddParameter(command, "@firstChargeDt", collectorPayment.FirstChargeDt, DbType.DateTime2);
                AddParameter(command, "@lastChargeDt", collectorPayment.LastChargeDt, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", collectorPayment.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetCollectorPayment(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateCollectorPaymentImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCollectorPaymentImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateCollectorPaymentImpll"), ex);
            }
        }
        
        #endregion


        #region VlCharge
        internal override Collection<VLChargedCollector> GetChargedCollectorsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payment_chargedCollectors_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetChargedCollectors(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetChargedCollectorsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetChargedCollectorsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetChargedCollectorsImpl"), ex);
            }
        }
        internal override Collection<VLChargedCollector> GetChargedCollectorsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payment_chargedCollectors_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetChargedCollectors(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetChargedCollectorsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetChargedCollectorsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetChargedCollectorsPagedImpl"), ex);
            }
        }
        internal override Collection<VLCharge> GetChargesImpl(Int32 accessToken, Int32 paymentId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_paymentCharges_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@paymentId", paymentId, DbType.Int32);

                return ExecuteAndGetCharges(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentChargesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesImpl"), ex);
            }
        }
        
        internal override Collection<VLCharge> GetChargesForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_paymentCharges_GetAll_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCharges(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentChargesForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesForClientImpl"), ex);
            }
        }
        internal override Collection<VLCharge> GetChargesPagedForClientImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_paymentCharges_GetPage_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetCharges(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentChargesPagedForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesPagedForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsPagedImpl"), ex);
            }
        }

        internal override Collection<VLCharge> GetChargesForCollectorImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_paymentCharges_GetAll_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetCharges(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentChargesForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesForClientImpl"), ex);
            }
        }
        internal override Collection<VLCharge> GetChargesPagedForCollectorImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_paymentCharges_GetPage_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetCharges(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetPaymentChargesPagedForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentChargesPagedForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetPaymentsPagedImpl"), ex);
            }
        }

        #endregion


        internal override bool ChargePaymentForEmailImpl(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Charge_For_Email");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@CollectorPaymentId", collectorPaymentId, DbType.Int32);
                AddParameter(command, "@CollectorId", collectorId, DbType.Int32);
                AddParameter(command, "@MessageId", messageId, DbType.Int32);
                AddParameter(command, "@Recipient", recipientId, DbType.Int64);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                Int32 retValue = (Int32)ExecuteScalar(command);
                return retValue == 1;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "ChargePaymentForEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForEmailImpl"), ex);
            }
        }

        internal override bool UnchargePaymentForEmailImpl(Int32 accessToken, Int32 collectorPaymentId, Int32 collectorId, Int32 messageId, Int64 recipientId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Uncharge_For_Email");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@CollectorPaymentId", collectorPaymentId, DbType.Int32);
                AddParameter(command, "@CollectorId", collectorId, DbType.Int32);
                AddParameter(command, "@MessageId", messageId, DbType.Int32);
                AddParameter(command, "@Recipient", recipientId, DbType.Int64);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                Int32 retValue = (Int32)ExecuteScalar(command);
                return retValue == 1;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UnchargePaymentForEmailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UnchargePaymentForEmailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UnchargePaymentForEmailImpl"), ex);
            }
        }


        #region VLClientList

        internal override Collection<VLClientList> GetClientListsImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetClientLists(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientListsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsImpl"), ex);
            }
        }
        internal override int GetClientListsCountImpl(Int32 accessToken, Int32 clientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientListsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsCountImpl"), ex);
            }
        }
        internal override Collection<VLClientList> GetClientListsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClientLists(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientListsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsPagedImpl"), ex);
            }
        }
        internal override VLClientList GetClientListByIdImpl(Int32 accessToken, Int32 listId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);


                return ExecuteAndGetClientList(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetClientListsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetClientListsByIdImpl"), ex);
            }
        }
        internal override void DeleteClientListImpl(Int32 accessToken, Int32 listId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteClientListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteClientListImpl"), ex);
            }
        }
        internal override VLClientList CreateClientListImpl(Int32 accessToken, VLClientList list, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", list.Client, DbType.Int32);
                AddParameter(command, "@listId", list.ListId, DbType.Int32);
                AddParameter(command, "@name", list.Name, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientList(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateClientListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateClientListImpl"), ex);
            }
        }
        internal override VLClientList UpdateClientListImpl(Int32 accessToken, VLClientList list, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientlists_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", list.Client, DbType.Int32);
                AddParameter(command, "@listId", list.ListId, DbType.Int32);
                AddParameter(command, "@name", list.Name, DbType.String);
                AddParameter(command, "@lastUpdateDT", list.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetClientList(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateClientListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateClientListImpll"), ex);
            }
        }
        
        #endregion


        #region VLContacts

        internal override Collection<VLContact> GetContactsImpl(Int32 accessToken, Int32 listId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetContacts(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetContactsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsImpl"), ex);
            }
        }
        internal override Collection<VLContact> GetContactsExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetContacts(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetContactsExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsExImpl"), ex);
            }
        }
        internal override int GetContactsCountImpl(Int32 accessToken, Int32 listId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetContactsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsCountImpl"), ex);
            }
        }
        internal override Collection<VLContact> GetContactsPagedImpl(Int32 accessToken, Int32 listId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetContacts(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetContactsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsPagedImpl"), ex);
            }
        }
        internal override VLContact GetContactByIdImpl(Int32 accessToken, Int32 contactId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@contactId", contactId, DbType.Int32);


                return ExecuteAndGetContact(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetContactsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetContactsByIdImpl"), ex);
            }
        }
        internal override void DeleteContactImpl(Int32 accessToken, Int32 contactId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@contactId", contactId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "DeleteContactImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteContactImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "DeleteContactImpl"), ex);
            }
        }
        internal override int RemoveAllContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Remove_All");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "RemoveAllContactsFromListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllContactsFromListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllContactsFromListImpl"), ex);
            }
        }
        internal override int RemoveAllOptedOutContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Remove_AllOptedOut");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
            }
        }
        internal override int RemoveAllBouncedContactsFromListImpl(Int32 accessToken, Int32 listId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Remove_AllBounced");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveAllOptedOutContactsFromListImpl"), ex);
            }
        }
        internal override int RemoveByDomainContactsFromListImpl(Int32 accessToken, Int32 listId, string domainName, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Remove_AllByDomain");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@domainName", domainName, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "RemoveByDomainContactsFromListImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveByDomainContactsFromListImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "RemoveByDomainContactsFromListImpl"), ex);
            }
        }


        internal override VLContact CreateContactImpl(Int32 accessToken, VLContact contact, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", contact.ClientId, DbType.Int32);
                AddParameter(command, "@listId", contact.ListId, DbType.Int32);
                AddParameter(command, "@contactId", contact.ContactId, DbType.Int32);
                AddParameter(command, "@organization", contact.Organization, DbType.String);
                AddParameter(command, "@title", contact.Title, DbType.String);
                AddParameter(command, "@department", contact.Department, DbType.String);
                AddParameter(command, "@firstName", contact.FirstName, DbType.String);
                AddParameter(command, "@lastName", contact.LastName, DbType.String);
                AddParameter(command, "@email", contact.Email, DbType.String);
                AddParameter(command, "@attributeFlags", contact.AttributeFlags, DbType.Int32);
                AddParameter(command, "@comment", contact.Comment, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetContact(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "CreateContactImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateContactImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "CreateContactImpl"), ex);
            }
        }

        internal override void ImportContactImpl(Int32 callerPrincipalId, VLContact[] contacts, int length, DateTime currentTimeUtc, ref Int32 successImports, ref Int32 failedImports)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Import");
                AddParameter(command, "@callerPrincipalId", callerPrincipalId, DbType.Int32);
                AddParameter(command, "@clientId", contacts[0].ClientId, DbType.Int32);
                AddParameter(command, "@listId", contacts[0].ListId, DbType.Int32);
                AddParameter(command, "@totalRecords", length, DbType.Int32);

                Int32 totalRecords = length;
                for (int index = 0; index < length; index++)
                {
                    var suffix = (index + 1).ToString();

                    AddParameter(command, "@organization" + suffix, contacts[index].Organization, DbType.String);
                    AddParameter(command, "@title" + suffix, contacts[index].Title, DbType.String);
                    AddParameter(command, "@department" + suffix, contacts[index].Department, DbType.String);
                    AddParameter(command, "@firstName" + suffix, contacts[index].FirstName, DbType.String);
                    AddParameter(command, "@lastName" + suffix, contacts[index].LastName, DbType.String);
                    AddParameter(command, "@email" + suffix, contacts[index].Email, DbType.String);
                    AddParameter(command, "@attributeFlags" + suffix, contacts[index].AttributeFlags, DbType.Int32);
                }                
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                successImports = Convert.ToInt32(ExecuteScalar(command));
                failedImports = totalRecords - successImports;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "ImportContactImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ImportContactImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ImportContactImpl"), ex);
            }
        }

        internal override void ImportContactsFinalizeImpl(Int32 callerPrincipalId, Int32 clientId, Int32 listId, DateTime currentTimeUtc, ref Int32 optedOutContacts, ref Int32 bouncedContacts, ref Int32 totalContacts)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Import_Finalize");
                AddParameter(command, "@callerPrincipalId", callerPrincipalId, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@listId", listId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                try
                {
                    command.Connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                            return;
                        reader.Read();

                        optedOutContacts = reader.GetInt32(0);
                        bouncedContacts = reader.GetInt32(1);
                        totalContacts = reader.GetInt32(2);
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "ImportContactsFinalizeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ImportContactsFinalizeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ImportContactsFinalizeImpl"), ex);
            }
        }
        
        internal override VLContact UpdateContactImpl(Int32 accessToken, VLContact contact, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientcontacts_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", contact.ClientId, DbType.Int32);
                AddParameter(command, "@listId", contact.ListId, DbType.Int32);
                AddParameter(command, "@contactId", contact.ContactId, DbType.Int32);
                AddParameter(command, "@organization", contact.Organization, DbType.String);
                AddParameter(command, "@title", contact.Title, DbType.String);
                AddParameter(command, "@department", contact.Department, DbType.String);
                AddParameter(command, "@firstName", contact.FirstName, DbType.String);
                AddParameter(command, "@lastName", contact.LastName, DbType.String);
                AddParameter(command, "@email", contact.Email, DbType.String);
                AddParameter(command, "@attributeFlags", contact.AttributeFlags, DbType.Int32);
                AddParameter(command, "@comment", contact.Comment, DbType.String);
                AddParameter(command, "@lastUpdateDT", contact.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetContact(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateContactImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateContactImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateContactImpll"), ex);
            }
        }
        internal override void UpdateContactCounterImpl(Int32 callerPrincipalId, Int32 listId)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("update [dbo].[ClientLists] set [TotalContacts] = (select count(*) from [dbo].[ClientContacts] where [ListId]={0}) where [ListId]={0}", listId));
                command.CommandType = CommandType.Text;

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "UpdateContactCounterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateContactCounterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "UpdateContactCounterImpl"), ex);
            }
        }

        #endregion

        #region VLLogin
        internal override Collection<VLLogin> GetLoginsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_logins_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetLogins(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetLoginsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetLoginsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetLoginsPagedImpl"), ex);
            }
        }

        internal override VLLogin GetLoginByIdImpl(Int32 accessToken, Int32 loginId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_logins_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@loginId", loginId, DbType.Int32);


                return ExecuteAndGetLogin(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "GetLoginByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetLoginByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "GetLoginByIdImpl"), ex);
            }
        }

        #endregion
    }
}
