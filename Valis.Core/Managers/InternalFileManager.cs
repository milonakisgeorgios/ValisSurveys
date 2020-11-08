using System;
using System.Diagnostics;
using System.IO;
using Valis.Core.Dal;

namespace Valis.Core.Managers
{
    /// <summary>
    /// Αυτή η class προσφέρει συγκεκριμένες εσωτερικές υπηρεσίες, σχετικά με τα αρχεία και το streaming αυτών
    /// <para>Η ύπαρξή της απλοποιεί τα πράγματα στην χρήση των αρχείων απο HTML κώδικα</para>
    /// </summary>
    internal class InternalFileManager
    {
        #region support
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
        #endregion



        /// <summary>
        /// Gets the full path of the specified managed file.
        /// <para>GetFilePath returns the entire path for the file (including the filename and the extension).</para>
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetFilePath(VLFile file)
        {
            return FilesDaoBase.GetFilePath(ValisSystem.Core.FileInventory.Path, file);
        }


        /// <summary>
        /// Επιστρέφει το αρχείο με το συγκεκριμένο Id
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public VLFile GetFileById(Guid fileId)
        {
            return FilesDal.GetFileByIdInternal(fileId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public System.Byte[] GetFileStream(Guid fileId)
        {
            VLFile file = FilesDal.GetFileByIdInternal(fileId);
            return GetFileStream(file);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public System.Byte[] GetFileStream(VLFile file)
        {
            if (file == null) throw new ArgumentNullException("file");

            //Αναλόγως που βρίσκεται το binary περιεχόμενο του αρχείου μας, το διαβάζουμε μέσα στον πίνακα _binaryContent:
            System.Byte[] _binaryContent = null;
            if (file.IsPhysicalFile)
            {
                #region read file content from filesystem
                string filePath = this.GetFilePath(file);

                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    using (FileStream fs = fileInfo.OpenRead())
                    {
                        //το _binaryContent το θέτουμε στο μέγεθος του φυσικού αρχείου που θα διαβάσουμε
                        _binaryContent = new byte[fileInfo.Length];
                        //Διαβάζουμε μονομιάς το binary περιεχόμενο
                        int totalNumberOfBytes = fs.Read(_binaryContent, 0, _binaryContent.Length);
                    }
                }
                else
                {
                    throw new VLException(string.Format("Physical file {0} with path: '{1}' and fileId={2} does not exists!", file.OriginalFileName, filePath, file.FileId));
                }
                #endregion
            }
            else
            {
                #region read file content from database
                _binaryContent = FilesDal.GetFileStreamInternal(file.FileId);
                #endregion
            }

            //θα πρέπει το κατεγεγραμμένο μέγεθος του αρχείου στην βάση (file.FileBytes) να είναι ίδιο
            //με αυτό που διαβάσαμε απο το φυσικό αρχείο
            if (_binaryContent.Length != file.Size)
            {
                throw new VLException(string.Format("The readed physical size of {0} file is {1} bytes which is different from the recorded value of {2} bytes!", file.OriginalFileName, _binaryContent.Length, file.Size));
            }


            return _binaryContent;
        }
    }
}
