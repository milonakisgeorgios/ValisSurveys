using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    public sealed class VLQuestionColumn
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_columnId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_displayOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_CustomId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_columnText;
        #endregion


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 Survey
        {
            get { return m_survey; }
            internal set { m_survey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 Question
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
        /// Ενας κωδικός (τον οποίο τον δίνει ο Πελάτης) και συνοδεύει το export του column
        /// </summary>
        public String CustomId
        {
            get { return m_CustomId; }
            set { m_CustomId = value; }
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


        private string m_htmlPartialColumnId;
        public string HtmlPartialColumnId
        {
            get
            {
                if (string.IsNullOrEmpty(m_htmlPartialColumnId))
                {
                    m_htmlPartialColumnId = string.Format("ClmnID_{0}_", this.ColumnId);
                }
                return m_htmlPartialColumnId;
            }
        }


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLQuestionColumn()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLQuestionColumn(DbDataReader reader)
        {
            int ordinal = reader.GetOrdinal("Survey");
            this.Survey = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("Question");
            this.Question = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("ColumnId");
            this.ColumnId = reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("DisplayOrder");
            this.DisplayOrder = reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("AttributeFlags");
            this.AttributeFlags = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("CustomId");
            if (!reader.IsDBNull(ordinal)) this.CustomId = reader.GetString(ordinal);
            ordinal = reader.GetOrdinal("TextsLanguage");
            this.m_textsLanguage = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("ColumnText");
            this.ColumnText = reader.GetString(ordinal);

        }
        internal VLQuestionColumn(VLQuestionColumn source)
        {
            this.m_survey = default(Int32);
            this.m_question = default(Int16);
            this.m_columnId = default(Byte);
            this.m_displayOrder = default(Byte);
            this.m_attributeFlags = default(Int32);
            this.m_CustomId = default(String);
            this.m_textsLanguage = source.m_textsLanguage;
            this.m_columnText = source.m_columnText;
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Survey.GetHashCode() ^
                this.Question.GetHashCode() ^
                this.ColumnId.GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.ColumnText.GetHashCode();
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


            var other = (VLQuestionColumn)obj;

            //reference types
            if (!Object.Equals(ColumnText, other.ColumnText)) return false;
            //value types
            if (!Survey.Equals(other.Survey)) return false;
            if (!Question.Equals(other.Question)) return false;
            if (!ColumnId.Equals(other.ColumnId)) return false;
            if (!DisplayOrder.Equals(other.DisplayOrder)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLQuestionColumn o1, VLQuestionColumn o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLQuestionColumn o1, VLQuestionColumn o2)
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


        public override string ToString()
        {
            return string.Format("{0}:{1} -> {2}", this.ColumnId, BuiltinLanguages.GetLanguageById(this.TextsLanguage).Name, this.ColumnText);
        }
    }
}
