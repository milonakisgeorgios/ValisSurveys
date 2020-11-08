using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLViewFilter
    {
        [Flags]
        internal enum ViewFilterAttributes : short
        {
            None                = 0,

            ORingDetails        = 16,           // 1 << 4
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_filterId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_applyOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isRule;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionType? m_questionType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        LogicalOperator? m_logicalOperator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isActive;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Collection<VLFilterDetail> m_filterDetails;
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
		public Int32 Survey
		{
			get { return m_survey; }
			internal set { m_survey = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public String Name
		{
			get { return m_name; }
			set { m_name = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public Int16 ApplyOrder
		{
			get { return m_applyOrder; }
			internal set { m_applyOrder = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public Boolean IsRule
		{
			get { return m_isRule; }
			internal set { m_isRule = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public Int16? Question
		{
			get { return m_question; }
			internal set { m_question = value; }
		}
        /// <summary>
        /// 
        /// </summary>
        public QuestionType? QuestionType
        {
            get { return this.m_questionType; }
            internal set { m_questionType = value; }
        }
		/// <summary>
		/// 
		/// </summary>
		public LogicalOperator? LogicalOperator
		{
			get { return m_logicalOperator; }
			internal set { m_logicalOperator = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		public Boolean IsActive
		{
			get { return m_isActive; }
			set { m_isActive = value; }
		}
		/// <summary>
		/// 
		/// </summary>
		internal Int16 AttributeFlags
		{
			get { return m_attributeFlags; }
			set { m_attributeFlags = value; }
		}


        /// <summary>
        /// Μας λέει η σύνθεση των details γίνει με ORing αυτών
        /// </summary>
        internal System.Boolean ORingDetails
        {
            get { return (this.m_attributeFlags & ((Int16)ViewFilterAttributes.ORingDetails)) == ((Int16)ViewFilterAttributes.ORingDetails); }
            set
            {
                if (this.ORingDetails == value)
                    return;

                if (value)
                    this.m_attributeFlags = (short)(this.m_attributeFlags | (short)ViewFilterAttributes.ORingDetails);
                else
                    this.m_attributeFlags = (short)(this.m_attributeFlags ^ (short)ViewFilterAttributes.ORingDetails);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Collection<VLFilterDetail> FilterDetails
        {
            get { return m_filterDetails; }
            set { m_filterDetails = value; }
        }
        #endregion
        
        #region class constructors
        public VLViewFilter()
        {
			m_viewId = default(Guid);
			m_filterId = default(Int32);
			m_survey = default(Int32);
			m_name = default(string);
			m_applyOrder = default(Int16);
			m_isRule = default(Boolean);
			m_question = null;
            m_questionType = null;
			m_logicalOperator = null;
			m_isActive = default(Boolean);
			m_attributeFlags = default(Int16);
            m_filterDetails = new Collection<VLFilterDetail>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLViewFilter(DbDataReader reader) 
        {
            this.ViewId = reader.GetGuid(0);
            this.FilterId = reader.GetInt32(1);
            this.Survey = reader.GetInt32(2);
            if (!reader.IsDBNull(3)) this.Name = reader.GetString(3);
            this.ApplyOrder = reader.GetInt16(4);
            this.IsRule = reader.GetBoolean(5);
            if (!reader.IsDBNull(6)) this.Question = reader.GetInt16(6);
            if (!reader.IsDBNull(7)) this.QuestionType = (QuestionType)reader.GetByte(7);
            if (!reader.IsDBNull(8)) this.LogicalOperator = (LogicalOperator)reader.GetByte(8);
            this.IsActive = reader.GetBoolean(9);
            this.AttributeFlags = reader.GetInt16(10);
        }
        #endregion
        
        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
			return this.ViewId.GetHashCode() ^
				this.FilterId.GetHashCode() ^
				this.Survey.GetHashCode() ^
				((this.Name == null) ? string.Empty : this.Name.ToString()).GetHashCode() ^
				this.ApplyOrder.GetHashCode() ^
				this.IsRule.GetHashCode() ^
                ((this.Question == null) ? string.Empty : this.Question.ToString()).GetHashCode() ^
                ((this.QuestionType == null) ? string.Empty : this.QuestionType.ToString()).GetHashCode() ^
				((this.LogicalOperator == null) ? string.Empty : this.LogicalOperator.ToString()).GetHashCode() ^
				this.IsActive.GetHashCode() ^
				this.AttributeFlags.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
			if (Object.ReferenceEquals(this,obj)) 
				return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLViewFilter)obj;

            //reference types
			if (!Object.Equals(m_name, other.m_name)) return false;
            //value types
			if (!m_viewId.Equals(other.m_viewId)) return false;
			if (!m_filterId.Equals(other.m_filterId)) return false;
			if (!m_survey.Equals(other.m_survey)) return false;
			if (!m_applyOrder.Equals(other.m_applyOrder)) return false;
			if (!m_isRule.Equals(other.m_isRule)) return false;
            if (!m_question.Equals(other.m_question)) return false;
            if (!m_questionType.Equals(other.m_questionType)) return false;
			if (!m_logicalOperator.Equals(other.m_logicalOperator)) return false;
			if (!m_isActive.Equals(other.m_isActive)) return false;
			if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            //Πρέπει να συγκρίνουμε τα FilterDetails:
            if (m_filterDetails.Count != other.m_filterDetails.Count) return false;
            for (int index = 0; index < m_filterDetails.Count; index++)
            {
                if (m_filterDetails[index] != other.m_filterDetails[index])
                    return false;
            }


                return true; 
        }
        public static Boolean operator ==(VLViewFilter o1, VLViewFilter o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLViewFilter o1, VLViewFilter o2)
        {
            return !(o1 == o2);
        }
        
        #endregion

        
        /// <summary>
        /// 
        /// </summary>
        public string ViewFilterSql
        {
            get
            {
                if (!this.IsRule)
                {
                    return MakeLogicalOperatorSql();
                }
                else
                {
                    return MakeRuleSql();
                }
            }
        }
        string MakeLogicalOperatorSql()
        {
            if (this.LogicalOperator.Value == Core.LogicalOperator.And)
                return "and";
            else if (this.LogicalOperator.Value == Core.LogicalOperator.Or)
                return "or";
            else if (this.LogicalOperator.Value == Core.LogicalOperator.LeftParenthesis)
                return "(";
            else if (this.LogicalOperator.Value == Core.LogicalOperator.RightParenthesis)
                return ")";
            return string.Empty;
        }
        string MakeRuleSql()
        {
            switch(this.QuestionType.Value)
            {
                case Core.QuestionType.SingleLine:
                    return Make_UserInput_Sql();
                case Core.QuestionType.MultipleLine:
                    return Make_UserInput_Sql();
                case Core.QuestionType.Integer:
                    return Make_UserInput_Sql();
                case Core.QuestionType.Decimal:
                    return Make_UserInput_Sql();
                case Core.QuestionType.Date:
                    return Make_UserInput_Sql();
                case Core.QuestionType.Time:
                    return string.Empty;
                case Core.QuestionType.DateTime:
                    return string.Empty;
                case Core.QuestionType.OneFromMany:
                    return Make_PredefinedOptions_Sql();
                case Core.QuestionType.ManyFromMany:
                    return Make_PredefinedOptions_Sql();
                case Core.QuestionType.DropDown:
                    return Make_PredefinedOptions_Sql();
                case Core.QuestionType.Slider:
                    return string.Empty;
                case Core.QuestionType.MatrixOnePerRow:
                    return Make_MatrixOnePerRow_SQL();
                case Core.QuestionType.MatrixManyPerRow:
                    return Make_MatrixManyPerRow_SQL();
                case Core.QuestionType.MatrixManyPerRowCustom:
                    return string.Empty;
                case Core.QuestionType.Composite:
                    return string.Empty;
            }
            return string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Make_UserInput_Sql()
        {
            StringBuilder sb = new StringBuilder();
            Int32 detailscounter = 0;

            sb.AppendFormat("[Question]={0} and (", this.Question);
            foreach (var detail in m_filterDetails)
            {
                if (detailscounter++ > 0)
                {
                    sb.AppendFormat(") and (", this.Question);
                }
                else
                {
                    sb.Append("(");
                }

                if(this.QuestionType.Value == Core.QuestionType.Integer)
                {
                    #region
                    sb.Append("cast(UserInput as int) ");
                    if (detail.Operator == ComparisonOperator.Between)
                        sb.AppendFormat("between {0} and {1}", detail.UserInput1, detail.UserInput2);
                    else if (detail.Operator == ComparisonOperator.Equals)
                        sb.AppendFormat("= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Greater)
                        sb.AppendFormat("> {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.GreaterOrEqual)
                        sb.AppendFormat(">= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Less)
                        sb.AppendFormat("< {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.LessOrEqual)
                        sb.AppendFormat("<= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.NotEqual)
                        sb.AppendFormat("!= {0}", detail.UserInput1);
                    #endregion
                }
                else if (this.QuestionType.Value == Core.QuestionType.Decimal)
                {
                    #region
                    sb.Append("cast(UserInput as float(53)) ");
                    if (detail.Operator == ComparisonOperator.Between)
                        sb.AppendFormat("between {0} and {1}", detail.UserInput1, detail.UserInput2);
                    else if (detail.Operator == ComparisonOperator.Equals)
                        sb.AppendFormat("= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Greater)
                        sb.AppendFormat("> {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.GreaterOrEqual)
                        sb.AppendFormat(">= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Less)
                        sb.AppendFormat("< {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.LessOrEqual)
                        sb.AppendFormat("<= {0}", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.NotEqual)
                        sb.AppendFormat("!= {0}", detail.UserInput1);
                    #endregion
                }
                else if (this.QuestionType.Value == Core.QuestionType.Date)
                {
                    #region
                    sb.Append("convert(date, UserInput, 101) ");
                    if (detail.Operator == ComparisonOperator.Between)
                        sb.AppendFormat("between convert(date, '{0}', 101) and convert(date, '{1}', 101)", detail.UserInput1, detail.UserInput2);
                    else if (detail.Operator == ComparisonOperator.Equals)
                        sb.AppendFormat("= convert(date, '{0}', 101)", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Greater)
                        sb.AppendFormat("> convert(date, '{0}', 101)", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.GreaterOrEqual)
                        sb.AppendFormat(">= convert(date, '{0}', 101)", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Less)
                        sb.AppendFormat("< convert(date, '{0}', 101)", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.LessOrEqual)
                        sb.AppendFormat("<= convert(date, '{0}', 101)", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.NotEqual)
                        sb.AppendFormat("!= convert(date, '{0}', 101)", detail.UserInput1);
                    #endregion
                }
                else
                {
                    #region
                    sb.Append("UserInput ");
                    if (detail.Operator == ComparisonOperator.Between)
                        sb.AppendFormat("between '{0}' and '{1}'", detail.UserInput1, detail.UserInput2);
                    else if (detail.Operator == ComparisonOperator.Equals)
                        sb.AppendFormat("= '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Greater)
                        sb.AppendFormat("> '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.GreaterOrEqual)
                        sb.AppendFormat(">= '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Less)
                        sb.AppendFormat("< '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.LessOrEqual)
                        sb.AppendFormat("<= '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.NotEqual)
                        sb.AppendFormat("!= '{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.StartsWith)
                        sb.AppendFormat("like '{0}%'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.EndsWith)
                        sb.AppendFormat("like '%{0}'", detail.UserInput1);
                    else if (detail.Operator == ComparisonOperator.Like)
                        sb.AppendFormat("like '%{0}%'", detail.UserInput1);
                    #endregion
                }
            }
            sb.Append("))");

            return sb.ToString();
        }

        /// <summary>
        /// QuestionType.OneFromMany
        /// QuestionType.DropDown
        /// QuestionType.ManyFromMany
        /// </summary>
        /// <returns></returns>
        string Make_PredefinedOptions_Sql()
        {
            StringBuilder sb = new StringBuilder();
            Int32 detailscounter = 0;

            sb.AppendFormat("[Question]={0} and (", this.Question);
            foreach (var detail in m_filterDetails)
            {
                if (detailscounter++ > 0)
                {
                    if (this.QuestionType.Value == Core.QuestionType.OneFromMany || this.QuestionType.Value == Core.QuestionType.DropDown)
                        sb.AppendFormat(") or (", this.Question);
                    else
                        sb.AppendFormat(") and (", this.Question);
                }
                else
                {
                    sb.Append("(");
                }


                if (detail.Operator == ComparisonOperator.IsChecked)
                {
                    if (detail.SelectedOption.HasValue)
                        sb.AppendFormat("SelectedOption={0}", detail.SelectedOption.Value);
                    if (detail.SelectedOption.HasValue && detail.SelectedColumn.HasValue)
                        sb.AppendFormat(" and SelectedColumn={0}", detail.SelectedColumn.Value);
                    if (!detail.SelectedOption.HasValue && detail.SelectedColumn.HasValue)
                        sb.AppendFormat("SelectedColumn={0}", detail.SelectedColumn.Value);
                }
                else if (detail.Operator == ComparisonOperator.IsNotChecked)
                {
                    if (detail.SelectedOption.HasValue)
                        sb.AppendFormat("SelectedOption!={0}", detail.SelectedOption.Value);
                    if (detail.SelectedOption.HasValue && detail.SelectedColumn.HasValue)
                        sb.AppendFormat(" and SelectedColumn!={0}", detail.SelectedColumn.Value);
                    if (!detail.SelectedOption.HasValue && detail.SelectedColumn.HasValue)
                        sb.AppendFormat("SelectedColumn!={0}", detail.SelectedColumn.Value);
                }
            }
            sb.Append("))");

            return sb.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string Make_MatrixOnePerRow_SQL()
        {
            StringBuilder sb = new StringBuilder();

            /*Θέλουμε να μαζέψουμε όλα τα διακριτά options:*/
            Collection<byte> options = new Collection<byte>();
            foreach (var detail in m_filterDetails)
            {
                if (!options.Contains(detail.SelectedOption.Value))
                    options.Add(detail.SelectedOption.Value);
            }


            Int32 optionscounter = 0;
            sb.AppendFormat("Question={0} and (", this.Question);
            foreach (var selectedOption in options)
            {
                if (optionscounter++ > 0)
                {
                    sb.AppendFormat(") and (SelectedOption={0} and (", selectedOption);
                }
                else
                {
                    sb.AppendFormat("(SelectedOption={0} and (", selectedOption);
                }


                Int32 detailscounter = 0;
                foreach(var fdetail in m_filterDetails.Where(x=>x.SelectedOption == selectedOption))
                {
                    if (detailscounter++ > 0)
                    {
                        sb.Append(" or ");
                    }

                    if (fdetail.SelectedColumn.HasValue)
                        sb.AppendFormat("SelectedColumn={0}", fdetail.SelectedColumn.Value);
                    else
                        sb.Append("1=1");
                }
                sb.Append(")");

            }
            sb.Append("))");

            return sb.ToString();
        }
        string Make_MatrixManyPerRow_SQL()
        {
            StringBuilder sb = new StringBuilder();


            return sb.ToString();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!this.IsRule)
            {
                return this.LogicalOperator.ToString();
            }
            else
            {
                return this.ViewFilterSql;
            }
        }
    }
}
