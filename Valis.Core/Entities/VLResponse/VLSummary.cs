using System;
using System.Collections.ObjectModel;
using System.Data.Common;

namespace Valis.Core
{
    [Serializable]
    public class VLSummary
    {
        public class VLResponseTotal
        {
            public Int64 RowId { get; internal set; }
            public Int16 Question { get; internal set; }

            public byte Option { get; internal set; }
            public byte Column { get; internal set; }
            public string UserInput { get; internal set; }
            public int TotalCount { get; internal set; }
            public int Percentage { get; internal set; }

            internal VLResponseTotal(DbDataReader reader)
            {
                this.RowId = reader.GetInt64(0);
                this.Question = reader.GetInt16(1);
                if (!reader.IsDBNull(2)) this.Option = reader.GetByte(2);
                //if (!reader.IsDBNull(3)) _OptionText = reader.GetString(3);
                if (!reader.IsDBNull(4)) this.Column = reader.GetByte(4);
                //if (!reader.IsDBNull(5)) _ColumnText = reader.GetString(5);
                if (!reader.IsDBNull(6)) this.UserInput = reader.GetString(6);
                this.TotalCount = reader.GetInt32(7);
                this.Percentage = reader.GetInt32(8);
            }
        }

        public class VLQuestionSummary
        {
            public Int16 QuestionId { get; internal set; }
            public QuestionType QuestionType { get; internal set; }
            public String QuestionText { get; internal set; }
            public bool ShowResponses { get; internal set; }
            internal Int32 AttributeFlags { get; set; }
            public System.Int16 Page { get; internal set; }

            public Boolean ShowChart { get { return (this.AttributeFlags & ((int)ViewQuestionAttributes.ShowChart)) == ((int)ViewQuestionAttributes.ShowChart); } }
            public Boolean ShowDataTable { get { return (this.AttributeFlags & ((int)ViewQuestionAttributes.ShowDataTable)) == ((int)ViewQuestionAttributes.ShowDataTable); } }
            public Boolean ShowDataInTheChart { get { return (this.AttributeFlags & ((int)ViewQuestionAttributes.ShowDataInTheChart)) == ((int)ViewQuestionAttributes.ShowDataInTheChart); } }
            public Boolean HideZeroResponseOptions { get { return (this.AttributeFlags & ((int)ViewQuestionAttributes.HideZeroResponseOptions)) == ((int)ViewQuestionAttributes.HideZeroResponseOptions); } }
            public Boolean SwapRowsAndColumns { get { return (this.AttributeFlags & ((int)ViewQuestionAttributes.SwapRowsAndColumns)) == ((int)ViewQuestionAttributes.SwapRowsAndColumns); } }

            public ChartType? ChartType { get; set; }
            public ChartLabelType? LabelType { get; set; }
            public ChartAxisScale? AxisScale { get;set; }
            public Decimal? ScaleMaxPercentage { get; set; }
            public Decimal? ScaleMaxAbsolute { get; set; }

            public Int32 TotalAnswered { get; set; }
            public Int32 TotalSkipped { get; set; }

            public Collection<VLResponseTotal> ResponseTotals { get; set; }


            internal VLQuestionSummary(DbDataReader reader)
            {
                this.QuestionId = reader.GetInt16(0);
                this.QuestionType = (QuestionType)reader.GetByte(1);
                this.QuestionText = reader.GetString(2);
                this.ShowResponses = reader.GetBoolean(3);
                this.AttributeFlags = reader.GetInt32(4);
                this.Page = reader.GetInt16(5);
                if (!reader.IsDBNull(6)) this.ChartType = (ChartType)reader.GetByte(6);
                if (!reader.IsDBNull(7)) this.LabelType = (ChartLabelType)reader.GetByte(7);
                if (!reader.IsDBNull(8)) this.AxisScale = (ChartAxisScale)reader.GetByte(8);
                if (!reader.IsDBNull(9)) this.ScaleMaxPercentage = reader.GetDecimal(9);
                if (!reader.IsDBNull(10)) this.ScaleMaxAbsolute = reader.GetDecimal(10);
                if (!reader.IsDBNull(11)) this.TotalAnswered = reader.GetInt32(11);
                if (!reader.IsDBNull(12)) this.TotalSkipped = reader.GetInt32(12);

                this.ResponseTotals = new Collection<VLResponseTotal>();
            }
        }

        public class VLPageSumary
        {
            public System.Int16 PageId { get; internal set; }

            public System.String ShowTitle { get; internal set; }

            public bool ShowResponses { get; internal set; }

            public Int32 AttributeFlags { get; internal set; }

            public Collection<VLQuestionSummary> Questions { get; internal set; }


            internal VLPageSumary(DbDataReader reader)
            {
                this.PageId = reader.GetInt16(0);
                if (!reader.IsDBNull(1)) this.ShowTitle = reader.GetString(1);
                this.ShowResponses = reader.GetBoolean(2);
                this.AttributeFlags = reader.GetInt32(3);

                this.Questions = new Collection<VLQuestionSummary>();
            }
        }


        public Int32 Client { get; internal set; }

        public Int32 SurveyId { get; internal set; }

        public string SurveyTitle { get; internal set; }

        public Guid ViewId { get; internal set; }

        public Int16 TextsLanguage { get; internal set; }

        public Int32 DesignVersion { get; internal set; }

        /// <summary>
        /// Πόσα responses έχουν καταγραφεί στο σύστημα μας
        /// </summary>
        public Int32 RecordedResponses { get; internal set; }

        /// <summary>
        /// Πόσα responses (απο τα TotalResponses) βλέπει ο πελάτης μας
        /// </summary>
        public Int32 VisibleResponses { get; internal set; }

        /// <summary>
        /// Πόσα responses (απο τα VisibleResponses) παρέμειναν μετά την εφαρμογή των φίλτρων της όψης
        /// </summary>
        public Int32 FilteredResponses { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 FiltersVersion { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public Collection<VLPageSumary> Pages { get; internal set; }



        internal VLSummary(DbDataReader reader)
        {
            this.Client = reader.GetInt32(0);
            this.SurveyId = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.SurveyTitle = reader.GetString(2);
            this.ViewId = reader.GetGuid(3);
            this.TextsLanguage = reader.GetInt16(4);
            this.DesignVersion = reader.GetInt32(5);
            this.RecordedResponses = reader.GetInt32(6);
            this.VisibleResponses = reader.GetInt32(7);
            this.FilteredResponses = reader.GetInt32(8);
            this.FiltersVersion = reader.GetInt32(9);

            this.Pages = new Collection<VLPageSumary>();
        }
    }
}
