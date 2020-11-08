using System;
using System.Data.Common;
using System.Diagnostics;


namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLViewQuestion
    {


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_question;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_showResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ChartType? m_chartType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ChartLabelType? m_labelType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ChartAxisScale m_axisScale;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Decimal? m_scaleMaxPercentage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Decimal? m_scaleMaxAbsolute;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryTotalAnswered;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryTotalSkipped;
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
        public Boolean ShowResponses
        {
            get { return m_showResponses; }
            set { m_showResponses = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }



        public System.Boolean ShowChart
        {
            get { return (this.m_attributeFlags & ((int)ViewQuestionAttributes.ShowChart)) == ((int)ViewQuestionAttributes.ShowChart); }
            internal set
            {
                if (this.ShowChart == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewQuestionAttributes.ShowChart;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewQuestionAttributes.ShowChart;
            }
        }
        public System.Boolean ShowDataTable
        {
            get { return (this.m_attributeFlags & ((int)ViewQuestionAttributes.ShowDataTable)) == ((int)ViewQuestionAttributes.ShowDataTable); }
            internal set
            {
                if (this.ShowDataTable == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewQuestionAttributes.ShowDataTable;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewQuestionAttributes.ShowDataTable;
            }
        }
        public System.Boolean ShowDataInTheChart
        {
            get { return (this.m_attributeFlags & ((int)ViewQuestionAttributes.ShowDataInTheChart)) == ((int)ViewQuestionAttributes.ShowDataInTheChart); }
            internal set
            {
                if (this.ShowDataInTheChart == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewQuestionAttributes.ShowDataInTheChart;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewQuestionAttributes.ShowDataInTheChart;
            }
        }
        public System.Boolean HideZeroResponseOptions
        {
            get { return (this.m_attributeFlags & ((int)ViewQuestionAttributes.HideZeroResponseOptions)) == ((int)ViewQuestionAttributes.HideZeroResponseOptions); }
            internal set
            {
                if (this.HideZeroResponseOptions == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewQuestionAttributes.HideZeroResponseOptions;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewQuestionAttributes.HideZeroResponseOptions;
            }
        }
        public System.Boolean SwapRowsAndColumns
        {
            get { return (this.m_attributeFlags & ((int)ViewQuestionAttributes.SwapRowsAndColumns)) == ((int)ViewQuestionAttributes.SwapRowsAndColumns); }
            internal set
            {
                if (this.SwapRowsAndColumns == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewQuestionAttributes.SwapRowsAndColumns;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewQuestionAttributes.SwapRowsAndColumns;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public ChartType? ChartType
        {
            get { return m_chartType; }
            set { m_chartType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ChartLabelType? LabelType
        {
            get { return m_labelType; }
            set { m_labelType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ChartAxisScale AxisScale
        {
            get { return m_axisScale; }
            set { m_axisScale = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? ScaleMaxPercentage
        {
            get { return m_scaleMaxPercentage; }
            set { m_scaleMaxPercentage = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Decimal? ScaleMaxAbsolute
        {
            get { return m_scaleMaxAbsolute; }
            set { m_scaleMaxAbsolute = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? SummaryTotalAnswered
        {
            get { return m_summaryTotalAnswered; }
            set { m_summaryTotalAnswered = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? SummaryTotalSkipped
        {
            get { return m_summaryTotalSkipped; }
            set { m_summaryTotalSkipped = value; }
        }
        #endregion

        #region class constructors
        public VLViewQuestion()
        {
            this.ShowDataTable = true;
            this.ShowDataTable = true;
            this.AxisScale = ChartAxisScale.Percentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLViewQuestion(DbDataReader reader)
        {
            this.ViewId = reader.GetGuid(0);
            this.Survey = reader.GetInt32(1);
            this.Question = reader.GetInt16(2);
            this.ShowResponses = reader.GetBoolean(3);
            this.AttributeFlags = reader.GetInt32(4);
            if (!reader.IsDBNull(5)) this.ChartType = (ChartType)reader.GetByte(5);
            if (!reader.IsDBNull(6)) this.LabelType = (ChartLabelType)reader.GetByte(6);
            this.AxisScale = (ChartAxisScale)reader.GetByte(7);
            if (!reader.IsDBNull(8)) this.ScaleMaxPercentage = reader.GetDecimal(8);
            if (!reader.IsDBNull(9)) this.ScaleMaxAbsolute = reader.GetDecimal(9);
            if (!reader.IsDBNull(10)) this.SummaryTotalAnswered = reader.GetInt32(10);
            if (!reader.IsDBNull(11)) this.SummaryTotalSkipped = reader.GetInt32(11);
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ViewId.GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.Question.GetHashCode() ^
                this.ShowResponses.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.ChartType == null) ? string.Empty : this.ChartType.ToString()).GetHashCode() ^
                ((this.LabelType == null) ? string.Empty : this.LabelType.ToString()).GetHashCode() ^
                this.AxisScale.GetHashCode() ^
                ((this.ScaleMaxPercentage == null) ? string.Empty : this.ScaleMaxPercentage.ToString()).GetHashCode() ^
                ((this.ScaleMaxAbsolute == null) ? string.Empty : this.ScaleMaxAbsolute.ToString()).GetHashCode() ^
                ((this.SummaryTotalAnswered == null) ? string.Empty : this.SummaryTotalAnswered.ToString()).GetHashCode() ^
                ((this.SummaryTotalSkipped == null) ? string.Empty : this.SummaryTotalSkipped.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLViewQuestion)obj;

            //reference types
            //value types
            if (!m_viewId.Equals(other.m_viewId)) return false;
            if (!m_survey.Equals(other.m_survey)) return false;
            if (!m_question.Equals(other.m_question)) return false;
            if (!m_showResponses.Equals(other.m_showResponses)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_chartType.Equals(other.m_chartType)) return false;
            if (!m_labelType.Equals(other.m_labelType)) return false;
            if (!m_axisScale.Equals(other.m_axisScale)) return false;
            if (!m_scaleMaxPercentage.Equals(other.m_scaleMaxPercentage)) return false;
            if (!m_scaleMaxAbsolute.Equals(other.m_scaleMaxAbsolute)) return false;
            if (!m_summaryTotalAnswered.Equals(other.m_summaryTotalAnswered)) return false;
            if (!m_summaryTotalSkipped.Equals(other.m_summaryTotalSkipped)) return false;

            return true;
        }
        public static Boolean operator ==(VLViewQuestion o1, VLViewQuestion o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLViewQuestion o1, VLViewQuestion o2)
        {
            return !(o1 == o2);
        }

        #endregion

    }
}
