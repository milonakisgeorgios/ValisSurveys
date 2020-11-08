using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Valis.Core.Dal;
using Valis.Core.ViewModel;

namespace Valis.Core.SqlServer
{
    internal class ViewModelDao : ViewModelDaoBase
    {

        internal override Collection<VLSystemUserView> GetSystemUserViewsPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_systemusers_view_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetSystemUserViews(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_ViewModelDao, "GetSystemUserViewsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_ViewModelDao, "GetSystemUserViewsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_ViewModelDao, "GetSystemUserViewsPagedImpl"), ex);
            }
        }

        internal override Collection<VLClientUserView> GetClientUserViewsPagedImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_clientusers_view_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetClientUserViews(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_ViewModelDao, "GetClientUserViewsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_ViewModelDao, "GetClientUserViewsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_ViewModelDao, "GetClientUserViewsPagedImpl"), ex);
            }
        }
    }
}
