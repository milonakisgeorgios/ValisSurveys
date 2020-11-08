using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{

    [Serializable]
    public sealed class VLLibraryOption
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_optionId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionOptionType m_optionType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_displayOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_optionValue;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_optionText;
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
        public Byte OptionId
        {
            get { return m_optionId; }
            internal set { m_optionId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public QuestionOptionType OptionType
        {
            get { return m_optionType; }
            set { m_optionType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 DisplayOrder
        {
            get { return m_displayOrder; }
            set { m_displayOrder = value; }
        }
        /// <summary>
        /// Αυτό το πεδίο αντιπροσωπεύει τον βαθμό που πέρνει ένας χρήστης όταν επιλέξει σαν απάντηση το
        /// συγκεκριμένο option
        /// </summary>
        public Int16 OptionValue
        {
            get { return m_optionValue; }
            set { m_optionValue = value; }
        }
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set
            {
                this.m_attributeFlags = value;
            }
        }

        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        public string OptionText
        {
            get { return m_optionText; }
            internal set
            {
                this.m_optionText = value;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLLibraryOption()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLibraryOption(DbDataReader reader)
        {
            int ordinal = reader.GetOrdinal("Question");
            this.Question = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("OptionId");
            this.OptionId = reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("OptionType");
            if (!reader.IsDBNull(ordinal)) this.OptionType = (QuestionOptionType)reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("DisplayOrder");
            this.DisplayOrder = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("OptionValue");
            this.OptionValue = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("AttributeFlags");
            this.AttributeFlags = reader.GetInt32(ordinal);

            ordinal = reader.GetOrdinal("TextsLanguage");
            this.m_textsLanguage = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("OptionText");
            this.OptionText = reader.GetString(ordinal);

        }

        internal VLLibraryOption(VLLibraryOption source)
        {
            this.m_question = default(Int32);
            this.m_optionId = default(byte);
            this.m_optionType = source.m_optionType;
            this.m_displayOrder = default(Int16);
            this.m_optionValue = source.m_optionValue;
            this.m_attributeFlags = default(Int32);
            this.m_textsLanguage = source.m_textsLanguage;
            this.m_optionText = source.m_optionText;
        }
        #endregion


        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Question.GetHashCode() ^
                this.OptionId.GetHashCode() ^
                ((this.OptionType == null) ? string.Empty : this.OptionType.ToString()).GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                this.OptionValue.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.OptionText.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLLibraryOption)obj;

            //reference types
            //value types
            if (!m_question.Equals(other.m_question)) return false;
            if (!m_optionId.Equals(other.m_optionId)) return false;
            if (!m_optionType.Equals(other.m_optionType)) return false;
            if (!m_displayOrder.Equals(other.m_displayOrder)) return false;
            if (!m_optionValue.Equals(other.m_optionValue)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLLibraryOption o1, VLLibraryOption o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLLibraryOption o1, VLLibraryOption o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_optionText, true, true, false, 128, "OptionText");
        }
    }
}
