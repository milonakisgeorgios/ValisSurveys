using System;
using System.ComponentModel;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract, DataObject]
    public sealed class VLFile : VLObject
    {
        [Flags]
        internal enum FileAttributes : int
        {
            None = 0,
            AttributeXxx1   = 1,          // 1 << 0
            IsPhysicalFile  = 2,          // 1 << 1
            IsEncrypted     = 4,          // 1 << 2
            IsHashed        = 8,          // 1 << 3
            IsCompressed    = 16,         // 1 << 4
            IsImage         = 32,         // 1 << 5
        }

        #region class fields
        Int32 m_client;
        Guid m_fileId;
        Int32 m_survey;
        String m_originalFileName;
        String m_managedFileName;
        String m_extension;
        Int32 m_size;
        VLFileStatus m_status;
        Int32 m_attributeFlags;
        String m_inventoryPath;
        Int16? m_width;
        Int16? m_height;
        #endregion

        #region EntityState
        EntityState _currentEntityState = EntityState.Added;

        /// <summary>
        ///	Indicates state of object
        /// </summary>
        /// <remarks>0=Unchanged, 1=Added, 2=Changed</remarks>
        [BrowsableAttribute(false), XmlIgnoreAttribute()]
        public override EntityState EntityState
        {
            get { return _currentEntityState; }
            internal set { _currentEntityState = value; }
        }
        #endregion
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Client
        {
            get { return this.m_client; }
            internal set
            {
                if (this.m_client == value)
                    return;

                this.m_client = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Guid FileId
        {
            get { return this.m_fileId; }
            internal set
            {
                if (this.m_fileId == value)
                    return;

                this.m_fileId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Survey
        {
            get { return this.m_survey; }
            internal set
            {
                if (this.m_survey == value)
                    return;

                this.m_survey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Το όνομα του αρχείου έτσι όπως κατέθηκε στον Server
        /// </summary>
        public System.String OriginalFileName
        {
            get { return this.m_originalFileName; }
            internal set
            {
                if (this.m_originalFileName == value)
                    return;

                this.m_originalFileName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Το όνομα του αρχείου έτσι όπως του αποδόθηκε απο το σύστημα. Με αυτό το όνομα αποθηκεύεται στο filesystem.
        /// </summary>
        public System.String ManagedFileName
        {
            get { return this.m_managedFileName; }
            internal set
            {
                if (this.m_managedFileName == value)
                    return;

                this.m_managedFileName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Extension
        {
            get { return this.m_extension; }
            internal set
            {
                if (this.m_extension == value)
                    return;

                this.m_extension = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Size
        {
            get { return this.m_size; }
            internal set
            {
                if (this.m_size == value)
                    return;

                this.m_size = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public VLFileStatus Status
        {
            get { return this.m_status; }
            internal set
            {
                if (this.m_status == value)
                    return;

                this.m_status = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal System.Int32 AttributeFlags
        {
            get { return this.m_attributeFlags; }
            set
            {
                if (this.m_attributeFlags == value)
                    return;

                this.m_attributeFlags = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Είναι αποθηκευμένο σε filesystem ή σε binary μορφή στην βάση  (FileContent)?
        /// </summary>
        public System.Boolean IsPhysicalFile
        {
            get { return (this.m_attributeFlags & ((int)FileAttributes.IsPhysicalFile)) == ((int)FileAttributes.IsPhysicalFile); }
            internal set
            {
                if (this.IsPhysicalFile == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)FileAttributes.IsPhysicalFile;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)FileAttributes.IsPhysicalFile;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsEncrypted
        {
            get { return (this.m_attributeFlags & ((int)FileAttributes.IsEncrypted)) == ((int)FileAttributes.IsEncrypted); }
            internal set
            {
                if (this.IsEncrypted == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)FileAttributes.IsEncrypted;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)FileAttributes.IsEncrypted;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsHashed
        {
            get { return (this.m_attributeFlags & ((int)FileAttributes.IsHashed)) == ((int)FileAttributes.IsHashed); }
            internal set
            {
                if (this.IsHashed == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)FileAttributes.IsHashed;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)FileAttributes.IsHashed;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsCompressed
        {
            get { return (this.m_attributeFlags & ((int)FileAttributes.IsCompressed)) == ((int)FileAttributes.IsCompressed); }
            internal set
            {
                if (this.IsCompressed == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)FileAttributes.IsCompressed;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)FileAttributes.IsCompressed;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsImage
        {
            get { return (this.m_attributeFlags & ((int)FileAttributes.IsImage)) == ((int)FileAttributes.IsImage); }
            internal set
            {
                if (this.IsImage == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)FileAttributes.IsImage;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)FileAttributes.IsImage;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String InventoryPath
        {
            get { return this.m_inventoryPath; }
            internal set
            {
                if (this.m_inventoryPath == value)
                    return;

                this.m_inventoryPath = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int16? Width
        {
            get { return this.m_width; }
            set
            {
                if (this.m_width == value)
                    return;

                this.m_width = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16? Height
        {
            get { return this.m_height; }
            set
            {
                if (this.m_height == value)
                    return;

                this.m_height = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLFile()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLFile(DbDataReader reader)
        {
            this.Client = reader.GetInt32(0);
            this.FileId = reader.GetGuid(1);
            this.Survey = reader.GetInt32(2);
            this.OriginalFileName = reader.GetString(3);
            this.ManagedFileName = reader.GetString(4);
            this.Extension = reader.GetString(5);
            this.Size = reader.GetInt32(6);
            this.Status = (VLFileStatus)reader.GetByte(7);
            this.AttributeFlags = reader.GetInt32(8);
            if (!reader.IsDBNull(9)) this.InventoryPath = reader.GetString(9);
            if (!reader.IsDBNull(10)) this.Width = reader.GetInt16(10);
            if (!reader.IsDBNull(11)) this.Height = reader.GetInt16(11);

            /*Κάνουμε εμείς initialize τα πεδία της base class (LrObject), για να μην την αφήσουμε 
             * να αναζητήσει το ordinal με την ονομασία της κολώνας*/
            this.CreateDT = reader.GetDateTime(12);
            this.CreateByPrincipal = reader.GetInt32(13);
            this.LastUpdateDT = this.CreateDT;
            this.LastUpdateByPrincipal = this.CreateByPrincipal;


            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Client.GetHashCode() ^
                this.FileId.GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.OriginalFileName.GetHashCode() ^
                this.ManagedFileName.GetHashCode() ^
                this.Extension.GetHashCode() ^
                this.Size.GetHashCode() ^
                this.Status.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.InventoryPath.GetHashCode() ^
                ((this.Width == null) ? string.Empty : this.Width.ToString()).GetHashCode() ^
                ((this.Height == null) ? string.Empty : this.Height.ToString()).GetHashCode();
        }
        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLFile)obj;

            //reference types
            if (!Object.Equals(OriginalFileName, other.OriginalFileName)) return false;
            if (!Object.Equals(ManagedFileName, other.ManagedFileName)) return false;
            if (!Object.Equals(Extension, other.Extension)) return false;
            if (!Object.Equals(InventoryPath, other.InventoryPath)) return false;
            //value types
            if (!Client.Equals(other.Client)) return false;
            if (!FileId.Equals(other.FileId)) return false;
            if (!Survey.Equals(other.Survey)) return false;
            if (!Size.Equals(other.Size)) return false;
            if (!Status.Equals(other.Status)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!Width.Equals(other.Width)) return false;
            if (!Height.Equals(other.Height)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLFile o1, VLFile o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLFile o1, VLFile o2)
        {
            return !(o1 == o2);
        }

        #endregion






        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_originalFileName, true, true, false, 512, "OriginalFileName");
            Utility.CheckParameter(ref m_managedFileName, true, true, false, 512, "ManagedFileName");
            Utility.CheckParameter(ref m_extension, true, true, false, 24, "Extension");
            Utility.CheckParameter(ref m_inventoryPath, true, true, false, 512, "InventoryPath");
        }
    }
}
