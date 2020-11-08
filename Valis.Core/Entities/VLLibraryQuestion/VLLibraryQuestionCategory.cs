using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [DataContract, DataObject]
    public sealed class VLLibraryQuestionCategory : VLObject
    {
        
        [Flags]
        internal enum LibraryQuestionCategoryAttributes : int
        {
            None = 0,
            IsBuiltIn = 1,           // 1 << 0
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        System.Int16 m_categoryId = default(Int16);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        System.Int32 m_attributeFlags = default(Int32);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]

        System.Int16 m_textsLanguage = default(Int16);
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        System.String m_name = default(string);
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
        public System.Int16 CategoryId
        {
            get { return this.m_categoryId; }
            internal set
            {
                if (this.m_categoryId == value)
                    return;

                this.m_categoryId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 AttributeFlags
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
        /// The language of thetranslatable fields..
        /// </summary>
        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Name
        {
            get { return this.m_name; }
            set
            {
                if (this.m_name == value)
                    return;

                this.m_name = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)LibraryQuestionCategoryAttributes.IsBuiltIn)) == ((int)LibraryQuestionCategoryAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)LibraryQuestionCategoryAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)LibraryQuestionCategoryAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLLibraryQuestionCategory()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLibraryQuestionCategory(DbDataReader reader)
            : base(reader)
        {
            this.m_categoryId = reader.GetInt16(0);
            this.m_attributeFlags = reader.GetInt32(1);
            this.m_textsLanguage = reader.GetInt16(2);
            if (!reader.IsDBNull(3)) this.m_name = reader.GetString(3);

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
            return this.CategoryId.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.Name == null) ? string.Empty : this.Name.ToString()).GetHashCode();
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


            var other = (VLLibraryQuestionCategory)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            //value types
            if (!CategoryId.Equals(other.CategoryId)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLLibraryQuestionCategory o1, VLLibraryQuestionCategory o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLLibraryQuestionCategory o1, VLLibraryQuestionCategory o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// 

        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_name, true, true, false, 128, "Name");
        }



    }
}