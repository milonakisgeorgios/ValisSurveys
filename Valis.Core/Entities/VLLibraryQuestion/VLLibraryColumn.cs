using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{

    [Serializable]
    public sealed class VLLibraryColumn
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_columnId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_displayOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_columnText;
        #endregion


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 Question
        {
            get { return m_question; }
            internal set { m_question = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte ColumnId
        {
            get { return m_columnId; }
            internal set { m_columnId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte DisplayOrder
        {
            get { return m_displayOrder; }
            set { m_displayOrder = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ColumnText
        {
            get { return m_columnText; }
            internal set
            {
                this.m_columnText = value;
            }
        }
        #endregion

        
        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLLibraryColumn()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLibraryColumn(DbDataReader reader)
        {
            int ordinal = reader.GetOrdinal("Question");
            this.Question = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("ColumnId");
            this.ColumnId = reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("DisplayOrder");
            this.DisplayOrder = reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("AttributeFlags");
            this.AttributeFlags = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("TextsLanguage");
            this.m_textsLanguage = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("ColumnText");
            this.ColumnText = reader.GetString(ordinal);

        }
        internal VLLibraryColumn(VLLibraryColumn source)
        {
            this.m_question = default(Int32);
            this.m_columnId = default(Byte);
            this.m_displayOrder = default(Byte);
            this.m_attributeFlags = default(Int32);
            this.m_textsLanguage = source.m_textsLanguage;
            this.m_columnText = source.m_columnText;
        }
        #endregion


        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Question.GetHashCode() ^
                this.ColumnId.GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.ColumnText.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLLibraryColumn)obj;

            //reference types
            //value types
            if (!m_question.Equals(other.m_question)) return false;
            if (!m_columnId.Equals(other.m_columnId)) return false;
            if (!m_displayOrder.Equals(other.m_displayOrder)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLLibraryColumn o1, VLLibraryColumn o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLLibraryColumn o1, VLLibraryColumn o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_columnText, true, true, false, 128, "ColumnText");
        }
    }
}
