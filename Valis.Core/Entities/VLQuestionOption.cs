using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    public sealed class VLQuestionOption
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_question;
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
        String m_CustomId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SkipToBehavior m_skipTo;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_skipToPage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_skipToQuestion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_skipToWebUrl;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string m_optionText;
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

        /// <summary>
        /// Ενας κωδικός (τον οποίο τον δίνει ο Πελάτης) και συνοδεύει το export του option
        /// </summary>
        public String CustomId
        {
            get { return m_CustomId; }
            set { m_CustomId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public SkipToBehavior SkipTo
        {
            get { return m_skipTo; }
            set { m_skipTo = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? SkipToPage
        {
            get { return m_skipToPage; }
            set { m_skipToPage = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? SkipToQuestion
        {
            get { return m_skipToQuestion; }
            set { m_skipToQuestion = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String SkipToWebUrl
        {
            get { return m_skipToWebUrl; }
            set { m_skipToWebUrl = value; }
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


        /// <summary>
        /// Utility method. Resets SkipTo fields.
        /// </summary>
        public void ResetSkipping()
        {
            this.SkipTo = SkipToBehavior.None;
            this.SkipToPage = null;
            this.SkipToQuestion = null;
            this.SkipToWebUrl = null;
        }

        private string m_htmlOptionId;
        public string HtmlOptionId
        {
            get
            {
                if (string.IsNullOrEmpty(m_htmlOptionId))
                {
                    m_htmlOptionId = string.Format("QstnID_{0}_OptID_{1}_", this.Question, this.OptionId);
                }
                return m_htmlOptionId;
            }
        }


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLQuestionOption()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLQuestionOption(DbDataReader reader)
        {
            int ordinal = reader.GetOrdinal("Survey");
            this.Survey = reader.GetInt32(ordinal);
            ordinal = reader.GetOrdinal("Question");
            this.Question = reader.GetInt16(ordinal);
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
            ordinal = reader.GetOrdinal("CustomId");
            if (!reader.IsDBNull(ordinal)) this.CustomId = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("SkipTo");
            this.SkipTo = (SkipToBehavior)reader.GetByte(ordinal);
            ordinal = reader.GetOrdinal("SkipToPage");
            if (!reader.IsDBNull(ordinal)) this.SkipToPage = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("SkipToQuestion");
            if (!reader.IsDBNull(ordinal)) this.SkipToQuestion = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("SkipToWebUrl");
            if (!reader.IsDBNull(ordinal)) this.SkipToWebUrl = reader.GetString(ordinal);

            ordinal = reader.GetOrdinal("TextsLanguage");
            this.m_textsLanguage = reader.GetInt16(ordinal);
            ordinal = reader.GetOrdinal("OptionText");
            this.OptionText = reader.GetString(ordinal);

        }

        internal VLQuestionOption(VLQuestionOption source)
        {
            this.m_survey = default(Int32);
            this.m_question = default(Int16);
            this.m_optionId = default(byte);
            this.m_optionType = source.m_optionType;
            this.m_displayOrder = default(Int16);
            this.m_optionValue = source.m_optionValue;
            this.m_attributeFlags = default(Int32);
            this.m_CustomId = default(string);
            this.m_textsLanguage = source.m_textsLanguage;
            this.m_optionText = source.m_optionText;
            this.m_skipTo = SkipToBehavior.None;
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
                this.OptionId.GetHashCode() ^
                this.OptionType.ToString().GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                this.OptionValue.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.CustomId == null) ? string.Empty : this.CustomId.ToString()).GetHashCode() ^
                this.SkipTo.GetHashCode() ^
                ((this.SkipToPage == null) ? string.Empty : this.SkipToPage.ToString()).GetHashCode() ^
                ((this.SkipToQuestion == null) ? string.Empty : this.SkipToQuestion.ToString()).GetHashCode() ^
                ((this.SkipToWebUrl == null) ? string.Empty : this.SkipToWebUrl.ToString()).GetHashCode() ^
                this.OptionText.GetHashCode();
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


            var other = (VLQuestionOption)obj;

            //reference types
            if (!Object.Equals(OptionText, other.OptionText)) return false;
            if (!Object.Equals(CustomId, other.CustomId)) return false;
            if (!Object.Equals(SkipToWebUrl, other.SkipToWebUrl)) return false;
            //value types
            if (!Survey.Equals(other.Survey)) return false;
            if (!Question.Equals(other.Question)) return false;
            if (!OptionId.Equals(other.OptionId)) return false;
            if (!OptionType.Equals(other.OptionType)) return false;
            if (!DisplayOrder.Equals(other.DisplayOrder)) return false;
            if (!OptionValue.Equals(other.OptionValue)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!SkipTo.Equals(other.SkipTo)) return false;
            if (!SkipToPage.Equals(other.SkipToPage)) return false;
            if (!SkipToQuestion.Equals(other.SkipToQuestion)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLQuestionOption o1, VLQuestionOption o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLQuestionOption o1, VLQuestionOption o2)
        {
            return !(o1 == o2);
        }

        #endregion





        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_CustomId, false, false, false, 64, "CustomId");
            Utility.CheckParameter(ref m_skipToWebUrl, false, false, false, 512, "SkipToWebUrl");
            Utility.CheckParameter(ref m_optionText, true, true, false, 128, "OptionText");
        }


        public override string ToString()
        {
            return string.Format("{0}:{1} -> {2}", this.OptionId, BuiltinLanguages.GetLanguageById(this.TextsLanguage).Name, this.OptionText);
        }
    }
}
