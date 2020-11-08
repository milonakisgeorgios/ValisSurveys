using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    [DataContract, DataObject]
    [Serializable]
    public sealed class VLContact : VLObject
    {

        [Flags]
        internal enum ContactAttributes : int
        {
            None = 0,
            AttributeXxx1 = 1,              // 1 << 0
            IsOptedOut = 2,                 // 1 << 1
            IsBouncedEmail = 4,             // 1 << 2
            AttributeXxx4 = 8,              // 1 << 3
            HasImportMark = 512             // 1 << 9
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_clientId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_listId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_contactId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_organization;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_title;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_department;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_firstName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_lastName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_email;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_comment;
        #endregion


        #region EntityState
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ClientId
        {
            get { return this.m_clientId; }
            internal set
            {
                if (this.m_clientId == value)
                    return;

                this.m_clientId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ListId
        {
            get { return this.m_listId; }
            internal set
            {
                if (this.m_listId == value)
                    return;

                this.m_listId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ContactId
        {
            get { return this.m_contactId; }
            internal set
            {
                if (this.m_contactId == value)
                    return;

                this.m_contactId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Organization
        {
            get { return this.m_organization; }
            set
            {
                if (this.m_organization == value)
                    return;

                this.m_organization = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Title
        {
            get { return this.m_title; }
            set
            {
                if (this.m_title == value)
                    return;

                this.m_title = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Department
        {
            get { return this.m_department; }
            set
            {
                if (this.m_department == value)
                    return;

                this.m_department = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String FirstName
        {
            get { return this.m_firstName; }
            set
            {
                if (this.m_firstName == value)
                    return;

                this.m_firstName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String LastName
        {
            get { return this.m_lastName; }
            set
            {
                if (this.m_lastName == value)
                    return;

                this.m_lastName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Email
        {
            get { return this.m_email; }
            set
            {
                if (this.m_email == value)
                    return;

                this.m_email = value;
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
        /// The contact have declined to receive further mailings
        /// </summary>
        public System.Boolean IsOptedOut
        {
            get { return (this.m_attributeFlags & ((int)ContactAttributes.IsOptedOut)) == ((int)ContactAttributes.IsOptedOut); }
            internal set
            {
                if (this.IsOptedOut == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ContactAttributes.IsOptedOut;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ContactAttributes.IsOptedOut;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// A delivery error has occured, and the messages cannot be delivered to the contact's email.
        /// </summary>
        public System.Boolean IsBouncedEmail
        {
            get { return (this.m_attributeFlags & ((int)ContactAttributes.IsBouncedEmail)) == ((int)ContactAttributes.IsBouncedEmail); }
            internal set
            {
                if (this.IsBouncedEmail == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ContactAttributes.IsBouncedEmail;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ContactAttributes.IsBouncedEmail;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Χρησιμοποιείται κατα την διαδικασία του ImportContactsFromCSV, έτσι ώστε να σημαδευτούν τα νέα Contacts, για να μπορέσει να τα βρεί
        /// η SystemDal.ImportContactsFinalizeImpl, και να τερματίσει την διαδικασία του ImportContactsFromCSV!
        /// </summary>
        internal System.Boolean HasImportMark
        {
            get { return (this.m_attributeFlags & ((int)ContactAttributes.HasImportMark)) == ((int)ContactAttributes.HasImportMark); }
            set
            {
                if (this.HasImportMark == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ContactAttributes.HasImportMark;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ContactAttributes.HasImportMark;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.String Comment
        {
            get { return this.m_comment; }
            set
            {
                if (this.m_comment == value)
                    return;

                this.m_comment = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLContact()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLContact(DbDataReader reader)
            : base(reader)
        {
            this.ClientId = reader.GetInt32(0);
            this.ListId = reader.GetInt32(1);
            this.ContactId = reader.GetInt32(2);
            if (!reader.IsDBNull(3)) this.Organization = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.Title = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.Department = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.FirstName = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.LastName = reader.GetString(7);
            this.Email = reader.GetString(8);
            this.AttributeFlags = reader.GetInt32(9);
            if (!reader.IsDBNull(10)) this.Comment = reader.GetString(10);
        }
        internal void InitializeInstance(Int32 clientId, Int32 listId, string email)
        {
            m_clientId = clientId;
            m_listId = listId;
            m_contactId = default(Int32);
            m_organization = default(string);
            m_title = default(string);
            m_department = default(string);
            m_firstName = default(string);
            m_lastName = default(string);
            m_email = email;
            m_attributeFlags = default(Int32);
            m_comment = default(string);
        }
        #endregion


        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ClientId.GetHashCode() ^
                this.ListId.GetHashCode() ^
                this.ContactId.GetHashCode() ^
                ((this.Organization == null) ? string.Empty : this.Organization.ToString()).GetHashCode() ^
                ((this.Title == null) ? string.Empty : this.Title.ToString()).GetHashCode() ^
                ((this.Department == null) ? string.Empty : this.Department.ToString()).GetHashCode() ^
                ((this.FirstName == null) ? string.Empty : this.FirstName.ToString()).GetHashCode() ^
                ((this.LastName == null) ? string.Empty : this.LastName.ToString()).GetHashCode() ^
                this.Email.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode();
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


            var other = (VLContact)obj;

            //reference types
            if (!Object.Equals(Organization, other.Organization)) return false;
            if (!Object.Equals(Title, other.Title)) return false;
            if (!Object.Equals(Department, other.Department)) return false;
            if (!Object.Equals(FirstName, other.FirstName)) return false;
            if (!Object.Equals(LastName, other.LastName)) return false;
            if (!Object.Equals(Email, other.Email)) return false;
            if (!Object.Equals(Comment, other.Comment)) return false;
            //value types
            if (!ClientId.Equals(other.ClientId)) return false;
            if (!ListId.Equals(other.ListId)) return false;
            if (!ContactId.Equals(other.ContactId)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLContact o1, VLContact o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLContact o1, VLContact o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_organization, false, false, false, 256, "Organization");
            Utility.CheckParameter(ref m_title, false, false, false, 256, "Title");
            Utility.CheckParameter(ref m_department, false, false, false, 256, "Department");
            Utility.CheckParameter(ref m_firstName, false, false, false, 128, "FirstName");
            Utility.CheckParameter(ref m_lastName, false, false, false, 128, "LastName");
            ValidateEmail(ref m_email);
            Utility.CheckParameter(ref m_comment, false, false, false, -1, "Comment");
        }


        internal static void ValidateEmail(ref string email)
        {
            Utility.CheckParameter(ref email, true, true, false, 256, "Email");
        }


        public override string ToString()
        {
            return this.Email;
        }
    }
}
