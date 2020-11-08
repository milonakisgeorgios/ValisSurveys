using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLFilterDetail
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_filterId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_detailId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ComparisonOperator m_operator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_selectedOption;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_selectedColumn;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_userInput1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_userInput2;
        #endregion

        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Guid ViewId
        {
            get { return m_viewId; }
            internal set { m_viewId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 FilterId
        {
            get { return m_filterId; }
            internal set { m_filterId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 DetailId
        {
            get { return m_detailId; }
            internal set { m_detailId = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ComparisonOperator Operator
        {
            get { return m_operator; }
            set { m_operator = value; }
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
        public String UserInput1
        {
            get { return m_userInput1; }
            set { m_userInput1 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String UserInput2
        {
            get { return m_userInput2; }
            set { m_userInput2 = value; }
        }
        #endregion


        #region class constructors
        [DebuggerStepThrough]
        public VLFilterDetail()
        {
            m_viewId = default(Guid);
            m_filterId = default(Int32);
            m_detailId = default(Int32);
            m_operator = default(Byte);
            m_selectedOption = null;
            m_selectedColumn = null;
            m_userInput1 = default(string);
            m_userInput2 = default(string);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLFilterDetail(DbDataReader reader)
        {
            this.ViewId = reader.GetGuid(0);
            this.FilterId = reader.GetInt32(1);
            this.DetailId = reader.GetInt32(2);
            this.Operator = (ComparisonOperator)reader.GetByte(3);
            if (!reader.IsDBNull(4)) this.SelectedOption = reader.GetByte(4);
            if (!reader.IsDBNull(5)) this.SelectedColumn = reader.GetByte(5);
            if (!reader.IsDBNull(6)) this.UserInput1 = reader.GetString(6);
            if (!reader.IsDBNull(7)) this.UserInput2 = reader.GetString(7);

        }
        #endregion


        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ViewId.GetHashCode() ^
                this.FilterId.GetHashCode() ^
                this.DetailId.GetHashCode() ^
                this.Operator.GetHashCode() ^
                ((this.SelectedOption == null) ? string.Empty : this.SelectedOption.ToString()).GetHashCode() ^
                ((this.SelectedColumn == null) ? string.Empty : this.SelectedColumn.ToString()).GetHashCode() ^
                ((this.UserInput1 == null) ? string.Empty : this.UserInput1.ToString()).GetHashCode() ^
                ((this.UserInput2 == null) ? string.Empty : this.UserInput2.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLFilterDetail)obj;

            //reference types
            if (!Object.Equals(m_userInput1, other.m_userInput1)) return false;
            if (!Object.Equals(m_userInput2, other.m_userInput2)) return false;
            //value types
            if (!m_viewId.Equals(other.m_viewId)) return false;
            if (!m_filterId.Equals(other.m_filterId)) return false;
            if (!m_detailId.Equals(other.m_detailId)) return false;
            if (!m_operator.Equals(other.m_operator)) return false;
            if (!m_selectedOption.Equals(other.m_selectedOption)) return false;
            if (!m_selectedColumn.Equals(other.m_selectedColumn)) return false;

            return true;
        }
        public static Boolean operator ==(VLFilterDetail o1, VLFilterDetail o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLFilterDetail o1, VLFilterDetail o2)
        {
            return !(o1 == o2);
        }

        #endregion
    }
}
