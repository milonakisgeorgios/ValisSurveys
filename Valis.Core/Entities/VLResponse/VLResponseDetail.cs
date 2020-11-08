using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    public sealed class VLResponseDetail
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int64 m_response;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_selectedOption;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_selectedColumn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_userInput;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int64 Response
        {
            get { return m_response; }
            set { m_response = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 Question
        {
            get { return m_question; }
            set { m_question = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte? SelectedOption
        {
            get { return m_selectedOption; }
            set { m_selectedOption = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Byte? SelectedColumn
        {
            get { return m_selectedColumn; }
            set { m_selectedColumn = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String UserInput
        {
            get { return m_userInput; }
            set { m_userInput = value; }
        }
        #endregion

        #region class constructors
        public VLResponseDetail()
        {
            m_response = default(Int64);
            m_question = default(Int16);
            m_selectedOption = null;
            m_selectedColumn = null;
            m_userInput = null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLResponseDetail(DbDataReader reader)
        {
            this.Response = reader.GetInt64(0);
            this.Question = reader.GetInt16(1);
            if (!reader.IsDBNull(2)) this.SelectedOption = reader.GetByte(2);
            if (!reader.IsDBNull(3)) this.SelectedColumn = reader.GetByte(3);
            if (!reader.IsDBNull(4)) this.UserInput = reader.GetString(4);

        }
        #endregion


        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Response.GetHashCode() ^
                this.Question.GetHashCode() ^
                ((this.SelectedOption == null) ? string.Empty : this.SelectedOption.ToString()).GetHashCode() ^
                ((this.SelectedColumn == null) ? string.Empty : this.SelectedColumn.ToString()).GetHashCode() ^
                ((this.UserInput == null) ? string.Empty : this.UserInput.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLResponseDetail)obj;

            //reference types
            if (!Object.Equals(m_userInput, other.m_userInput)) return false;
            //value types
            if (!m_response.Equals(other.m_response)) return false;
            if (!m_question.Equals(other.m_question)) return false;
            if (!m_selectedOption.Equals(other.m_selectedOption)) return false;
            if (!m_selectedColumn.Equals(other.m_selectedColumn)) return false;

            return true;
        }
        public static Boolean operator ==(VLResponseDetail o1, VLResponseDetail o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLResponseDetail o1, VLResponseDetail o2)
        {
            return !(o1 == o2);
        }

        #endregion

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(this.UserInput))
            {

                return string.Format("SelectedOption = {0}, SelectedColumn = {1}", this.SelectedOption, this.SelectedColumn);
            }
            else
            {
                return string.Format("UserInput = '{0}'", this.UserInput);
            }
        }
    }
}
