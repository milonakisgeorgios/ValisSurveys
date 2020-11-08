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
    internal abstract class FilesDaoBase : DataAccess
    {
        #region Instance Factory
        /// <summary>
        /// 
        /// </summary>
        protected FilesDaoBase() : base() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configSectionName"></param>
        /// <param name="dbElement"></param>
        /// <returns></returns>
        public static FilesDaoBase GetInstance(string configSectionName, DatabaseConfigurationElement dbElement)
        {
            if (string.IsNullOrWhiteSpace(configSectionName)) throw new ArgumentException("configSectionName is invalid");
            if (dbElement == null) throw new ArgumentNullException("dbElement");

            string key = configSectionName + "FilesDaoBase";
            var m_instance = GetDalInstance(key);
            if (m_instance == null)
            {
                var ProviderAssembly = Assembly.Load(dbElement.AssemblyName);

                m_instance = (FilesDaoBase)Activator.CreateInstance(ProviderAssembly.GetType(ProviderAssembly.GetName().Name + ".FilesDao"));
                m_instance.ConnectionString = dbElement.ConnectionString;
                m_instance.ProviderFactory = dbElement.ProviderFactory;
                m_instance.AssemblyName = dbElement.AssemblyName;
                m_instance.ConfigSectionName = configSectionName;

                SetDalInstance(key, m_instance);
            }
            return (FilesDaoBase)m_instance;
        }
        #endregion


        internal static string GetFilePath(string rootDirectory, VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (!file.IsPhysicalFile)
                throw new VLException("The file is not a physical file!");
            if (string.IsNullOrWhiteSpace(rootDirectory))
                throw new VLException("The rootDirectory is invalid!");

            return Path.Combine(Path.Combine(rootDirectory, file.InventoryPath), file.ManagedFileName);
        }


        public VLFile SaveFile(Int32 accessToken, VLSurvey survey, byte[] buffer, string fileName, string rootDirectory, Guid? fileId = null, string managedFileName = null, bool deleteFileIfExists = false)
        {
            if (fileId == null || fileId == default(Guid))
            {
                fileId = Guid.NewGuid();
            }
            if (string.IsNullOrWhiteSpace(managedFileName))
            {
                managedFileName = fileName;
            }

            try
            {
                #region implementation
                VLFile file = new VLFile();
                file.Client = survey.Client;
                file.FileId = fileId.Value;
                file.Survey = survey.SurveyId;
                file.OriginalFileName = fileName;
                file.ManagedFileName = managedFileName;
                file.Extension = Path.GetExtension(fileName);
                file.Size = buffer.Length;
                file.Status = VLFileStatus.Temporal;

                try
                {
                    MemoryStream stream = new MemoryStream(buffer);
                    Image img = Image.FromStream(stream);
                    file.IsImage = true;
                    file.Width = Convert.ToInt16(img.Width);
                    file.Height = Convert.ToInt16(img.Height);
                }
                catch
                {
                    file.IsImage = false;
                }


                if (survey.SaveFilesInDatabase)
                {
                    file.IsPhysicalFile = false;
                    #region implementation
                    file = CreateFileImpl(accessToken, file, Utility.UtcNow());
                    SetFileStreamImpl(accessToken, file.FileId, buffer);
                    #endregion
                }
                else
                {
                    file.IsPhysicalFile = true;
                    #region implementation
                    string fileDirPath = Path.Combine(rootDirectory, survey.Client.ToString(CultureInfo.InvariantCulture), survey.SurveyId.ToString(CultureInfo.InvariantCulture));

                    //Εάν δεν υπάρχει το fileDirPath το δημιουργούμε
                    if (!Directory.Exists(fileDirPath))
                    {
                        Directory.CreateDirectory(fileDirPath);
                    }
                    file.InventoryPath = Path.Combine(survey.Client.ToString(CultureInfo.InvariantCulture), survey.SurveyId.ToString(CultureInfo.InvariantCulture)) + "\\";

                    //Εάν υπάρχει ήδη φυσικό αρχείο με το ίδιο όνομα το διαγράφουμε?
                    if (deleteFileIfExists)
                    {
                        FileInfo fileInfo = new FileInfo(GetFilePath(rootDirectory, file));
                        if (fileInfo.Exists)
                        {
                            fileInfo.Delete();
                        }
                    }

                    //Πρέπει να σιγουρευτούμε ότι το ManagedFileName είναι μοναδικό μέσα στο fileDirPath
                    Int32 loopCounter = 1;
                    FileInfo fInfo = new FileInfo(GetFilePath(rootDirectory, file));
                    while (fInfo.Exists)
                    {
                        file.ManagedFileName = Path.GetFileNameWithoutExtension(managedFileName) + "_" + loopCounter.ToString(CultureInfo.InvariantCulture) + file.Extension;
                        loopCounter++;
                        fInfo = new FileInfo(GetFilePath(rootDirectory, file));
                    }


                    /*
                     *  Ο τρόπος που ανοίγουμε την FileStream για γράψιμο, διαγράφει τυχόν προυπάρχον αρχείο με το ίδιο όνομα
                     */
                    using (FileStream fstream = new FileStream(GetFilePath(rootDirectory, file), FileMode.Create))
                    {
                        using (BinaryWriter bw = new BinaryWriter(fstream))
                        {
                            bw.Write(buffer);
                        }
                    }

                    file = CreateFileImpl(accessToken, file, Utility.UtcNow());
                    #endregion
                }
                #endregion
                return file;
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::SaveFile()", ex);
            }
        }


        public void DeletePhysicalFile(VLFile fileToBeDeleted, string rootDirectory)
        {
            if (fileToBeDeleted.IsPhysicalFile == false)
                return;

            try
            {
                FileInfo finfo = new FileInfo(GetFilePath(rootDirectory, fileToBeDeleted));
                //Εάν το αρχείο υπάρχει θα προσπαθήσουμε να το διαγράψουμε. Εάν δεν υπάρχει, προς το παρόν αδιαφορούμε!
                if (finfo.Exists)
                {
                    finfo.Delete();
                }
            }
            catch (Exception ex)
            {
                throw new VLException(string.Format(CultureInfo.InvariantCulture, "Failure to delete '{0}'. (FileId='{1}')", GetFilePath(rootDirectory, fileToBeDeleted), fileToBeDeleted.FileId), ex);
            }
        }


        #region VLFile

        public Collection<VLFile> GetFiles(Int32 accessToken, Int32 callerClientId, string whereClause = null, string orderByClause = null)
        {
            try
            {
                return GetFilesImpl(accessToken, callerClientId, whereClause, orderByClause);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFiles().", ex);
            }
        }

        public int GetFilesCount(Int32 accessToken, Int32 callerClientId, string whereClause = null)
        {
            try
            {
                return GetFilesCountImpl(accessToken, callerClientId, whereClause);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFilesCount().", ex);
            }
        }

        public Collection<VLFile> GetFiles(Int32 accessToken, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null)
        {
            Utility.CheckPagingParameters(ref pageIndex, ref pageSize);
            int startRowIndex = ((pageIndex - 1) * pageSize) + 1;

            try
            {
                return GetFilesPagedImpl(accessToken, startRowIndex, pageSize, ref totalRows, whereClause, orderByClause);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFiles().", ex);
            }
        }

        public VLFile GetFileById(Int32 accessToken, Guid fileId)
        {
            try
            {
                return GetFileByIdImpl(accessToken, fileId);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFileById().", ex);
            }
        }

        internal VLFile GetFileByIdInternal(Guid fileId)
        {
            try
            {
                return GetFileByIdInternalImpl(fileId);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFileById().", ex);
            }
        }

        public VLFile CreateFile(Int32 accessToken, VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            try
            {
                return CreateFileImpl(accessToken, file, Utility.UtcNow());
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::CreateFile().", ex);
            }
        }

        public VLFile UpdateFile(Int32 accessToken, VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");
            try
            {
                return UpdateFileImpl(accessToken, file, Utility.UtcNow());
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::UpdateFile().", ex);
            }
        }

        public void DeleteFile(Int32 accessToken, Guid fileId)
        {
            try
            {
                DeleteFileImpl(accessToken, fileId, Utility.UtcNow());
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::DeleteFile().", ex);
            }
        }

        public Collection<VLFile> DeleteAllFilesInSurvey(Int32 accessToken, Int32 clientId, Int32 surveyId)
        {
            try
            {
                return DeleteAllFilesInSurveyImpl(accessToken, clientId, surveyId, Utility.UtcNow());
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::DeleteAllFilesInSurvey().", ex);
            }
        }

        public void SetFileStream(Int32 accessToken, Guid fileId, byte[] byteStream)
        {
            try
            {
                SetFileStreamImpl(accessToken, fileId, byteStream);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::SetFileStream().", ex);
            }
        }

        public byte[] GetFileStream(Int32 accessToken, Guid fileId)
        {
            try
            {
                return GetFileStreamImpl(accessToken, fileId);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFileStream().", ex);
            }
        }

        internal byte[] GetFileStreamInternal(Guid fileId)
        {
            try
            {
                return GetFileStreamInternalImpl(fileId);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::GetFileStreamInternal().", ex);
            }
        }

        public void DeleteFileStream(Int32 accessToken, Guid fileId)
        {
            try
            {
                DeleteFileStreamImpl(accessToken, fileId);
            }
            catch (Exception ex)
            {
                throw new VLDataException("Exception occured at FilesDaoBase::DeleteFileStream().", ex);
            }
        }


        protected abstract Collection<VLFile> GetFilesImpl(Int32 accessToken, Int32 callerClientId, string whereClause, string orderByClause);
        protected abstract int GetFilesCountImpl(Int32 accessToken, Int32 callerClientId, string whereClause);
        protected abstract Collection<VLFile> GetFilesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause);
        protected abstract VLFile GetFileByIdImpl(Int32 accessToken, Guid fileId);
        protected abstract VLFile GetFileByIdInternalImpl(Guid fileId);
        protected abstract void DeleteFileImpl(Int32 accessToken, Guid fileId, DateTime currentTimeUtc);
        protected abstract VLFile UpdateFileImpl(Int32 accessToken, VLFile file, DateTime currentTimeUtc);
        protected abstract VLFile CreateFileImpl(Int32 accessToken, VLFile file, DateTime currentTimeUtc);
        protected abstract void SetFileStreamImpl(Int32 accessToken, Guid fileId, byte[] byteStream);
        protected abstract byte[] GetFileStreamImpl(Int32 accessToken, Guid fileId);
        protected abstract byte[] GetFileStreamInternalImpl(Guid fileId);
        protected abstract void DeleteFileStreamImpl(Int32 accessToken, Guid fileId);
        protected abstract Collection<VLFile> DeleteAllFilesInSurveyImpl(Int32 accessToken, Int32 clientId, Int32 surveyId, DateTime currentTimeUtc);
        internal static Collection<VLFile> ExecuteAndGetFiles(DbCommand cmd)
        {
            var collection = new Collection<VLFile>();
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
                    while (reader.Read())
                    {
                        var _object = new VLFile(reader);
                        collection.Add(_object);
                    }
                }
            }
            finally
            {
                if (_closeTheConnection)
                {
                    cmd.Connection.Close();
                }
            }
            return collection;
        }
        protected VLFile ExecuteAndGetFile(DbCommand cmd)
        {
            VLFile _retObject = null;
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
                    if (reader.HasRows == false)
                        return null;
                    reader.Read();

                    _retObject = new VLFile(reader);
                }
            }
            finally
            {
                if (_closeTheConnection)
                {
                    cmd.Connection.Close();
                }
            }

            return _retObject;
        }
        #endregion



    }
}
