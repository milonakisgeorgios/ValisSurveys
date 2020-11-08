using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Valis.Core.Dal;

namespace Valis.Core.SqlServer
{
    internal class FilesDao : FilesDaoBase
    {
        #region LrFile
        protected override Collection<VLFile> GetFilesImpl(Int32 accessToken, Int32 callerClientId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select [Client],[FileId],[Survey],[OriginalFileName],[ManagedFileName],[Extension],[Size],[Status],[AttributeFlags],[InventoryPath], [Width], [Height],[CreationDT],[CreatedBy] from (select * from dbo.FileInventory where Client = @Client) as a {0} {1}", whereClause, orderByClause));
                command.CommandType = CommandType.Text;
                AddParameter(command, "@Client", callerClientId, DbType.Int32);

                return ExecuteAndGetFiles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFilesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesImpl"), ex);
            }
        }
        protected override int GetFilesCountImpl(Int32 accessToken, Int32 callerClientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select count(*) from (select * from dbo.FileInventory where Client = @Client) as a {0}", whereClause));
                command.CommandType = CommandType.Text;
                AddParameter(command, "@Client", callerClientId, DbType.Int32);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFilesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesCountImpl"), ex);
            }
        }
        protected override Collection<VLFile> GetFilesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_files_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);

                var collection = ExecuteAndGetFiles(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFilesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFilesPagedImpl"), ex);
            }
        }
        protected override VLFile GetFileByIdImpl(Int32 accessToken, Guid fileId)
        {
            try
            {
                DbCommand command = CreateCommand("select [Client],[FileId],[Survey],[OriginalFileName],[ManagedFileName],[Extension],[Size],[Status],[AttributeFlags],[InventoryPath], [Width], [Height],[CreationDT],[CreatedBy] from dbo.FileInventory where FileId = @FileId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@FileId", fileId, DbType.Guid);

                return ExecuteAndGetFile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFileByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileByIdImpl"), ex);
            }
        }
        protected override VLFile GetFileByIdInternalImpl(Guid fileId)
        {
            try
            {
                DbCommand command = CreateCommand("select [Client],[FileId],[Survey],[OriginalFileName],[ManagedFileName],[Extension],[Size],[Status],[AttributeFlags],[InventoryPath], [Width], [Height],[CreationDT],[CreatedBy] from dbo.FileInventory where FileId = @FileId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@FileId", fileId, DbType.Guid);

                return ExecuteAndGetFile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFileByIdInternalImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileByIdInternalImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileByIdInternalImpl"), ex);
            }
        }
        protected override void DeleteFileImpl(Int32 accessToken, Guid fileId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_files_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@fileId", fileId, DbType.Guid);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "DeleteFileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteFileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteFileImpl"), ex);
            }
        }
        protected override VLFile CreateFileImpl(Int32 accessToken, VLFile file, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_files_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", file.Client, DbType.Int32);
                AddParameter(command, "@fileId", file.FileId, DbType.Guid);
                AddParameter(command, "@survey", file.Survey, DbType.Int32);
                AddParameter(command, "@originalFileName", file.OriginalFileName, DbType.String);
                AddParameter(command, "@managedFileName", file.ManagedFileName, DbType.String);
                AddParameter(command, "@extension", file.Extension, DbType.String);
                AddParameter(command, "@size", file.Size, DbType.Int32);
                AddParameter(command, "@status", file.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", file.AttributeFlags, DbType.Int32);
                AddParameter(command, "@inventoryPath", file.InventoryPath, DbType.String);
                AddParameter(command, "@width", file.Width, DbType.Int16);
                AddParameter(command, "@height", file.Height, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetFile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "CreateFileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "CreateFileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "CreateFileImpl"), ex);
            }
        }
        protected override VLFile UpdateFileImpl(Int32 accessToken, VLFile file, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_files_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", file.Client, DbType.Int32);
                AddParameter(command, "@fileId", file.FileId, DbType.Guid);
                AddParameter(command, "@survey", file.Survey, DbType.Int32);
                AddParameter(command, "@originalFileName", file.OriginalFileName, DbType.String);
                AddParameter(command, "@managedFileName", file.ManagedFileName, DbType.String);
                AddParameter(command, "@extension", file.Extension, DbType.String);
                AddParameter(command, "@size", file.Size, DbType.Int32);
                AddParameter(command, "@status", file.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", file.AttributeFlags, DbType.Int32);
                AddParameter(command, "@inventoryPath", file.InventoryPath, DbType.String);
                AddParameter(command, "@width", file.Width, DbType.Int16);
                AddParameter(command, "@height", file.Height, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetFile(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "UpdateFileImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "UpdateFileImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "UpdateFileImpl"), ex);
            }
        }

        protected override void SetFileStreamImpl(Int32 accessToken, Guid fileId, byte[] byteStream)
        {
            try
            {
                DbCommand command = CreateCommand("if not exists(select FileId from dbo.FileInventoryStreams where FileId = @FileId) insert into dbo.FileInventoryStreams (FileId, ByteStream) values(@FileId, @ByteStream) else update dbo.FileInventoryStreams set ByteStream = @ByteStream where FileId = @FileId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@FileId", fileId, DbType.Guid);
                AddParameter(command, "@ByteStream", byteStream, DbType.Binary);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "SetFileStreamImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "SetFileStreamImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "SetFileStreamImpl"), ex);
            }
        }
        protected override byte[] GetFileStreamImpl(Int32 accessToken, Guid fileId)
        {
            DbCommand command = CreateCommand("select ByteStream from dbo.FileInventoryStreams where FileId = @FileId");
            command.CommandType = CommandType.Text;
            AddParameter(command, "@FileId", fileId, DbType.Guid);

            try
            {
                command.Connection.Open();
                return (byte[])command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFileStreamImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileStreamImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileStreamImpl"), ex);
            }
            finally
            {
                command.Connection.Close();
            }
        }
        protected override byte[] GetFileStreamInternalImpl(Guid fileId)
        {
            DbCommand command = CreateCommand("select ByteStream from dbo.FileInventoryStreams where FileId = @FileId");
            command.CommandType = CommandType.Text;
            AddParameter(command, "@FileId", fileId, DbType.Guid);

            try
            {
                command.Connection.Open();
                return (byte[])command.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "GetFileStreamInternalImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileStreamInternalImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "GetFileStreamInternalImpl"), ex);
            }
            finally
            {
                command.Connection.Close();
            }
        }
        protected override void DeleteFileStreamImpl(Int32 accessToken, Guid fileId)
        {
            try
            {
                DbCommand command = CreateCommand("delete from dbo.FileInventoryStreams where FileId = @FileId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@FileId", fileId, DbType.Guid);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "DeleteFileStreamImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteFileStreamImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteFileStreamImpl"), ex);
            }
        }

        protected override Collection<VLFile> DeleteAllFilesInSurveyImpl(int accessToken, Int32 clientId, Int32 surveyId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_files_DeleteAllForSurvey");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientId, DbType.Int32);
                AddParameter(command, "@survey", surveyId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetFiles(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_FilesDao, "DeleteAllFilesInSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteAllFilesInSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_FilesDao, "DeleteAllFilesInSurveyImpl"), ex);
            }
        }
        #endregion

    }
}
