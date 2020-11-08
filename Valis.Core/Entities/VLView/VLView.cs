using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLView : VLObject
    {

        [Flags]
        internal enum ViewAttributes : int
        {
            None                                = 0,

            PartialShowInUse                    = 16,           // 1 << 4
            EnablePartialShow                   = 32,           // 1 << 5

            FilteringByCollectorInUse           = 64,           // 1 << 6
            EnableFilteringByCollector          = 128,          // 1 << 7

            FilteringByTimePeriodInUse          = 256,          // 1 << 8
            EnableFilteringByTimePeriod         = 512,          // 1 << 9

            FilteringByResponseTimeInUse        = 1024,         // 1 << 10
            EnableFilteringByResponseTime       = 2048,         // 1 << 11

            FilteringByQuestionInUse            = 4096,         // 1 << 12
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_userId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isDefaultView;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_timePeriodStart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_timePeriodEnd;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_totalResponseTime;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ResponseTimeUnit? m_totalResponseTimeUnit;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ResponseTimeOperator? m_totalResponseTimeOperator;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_NumberOfQuestionFilters;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_filtersVersion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryDesignVersion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryRecordedResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryFiltersVersion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_sumaryGenerationDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryVisibleResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_summaryFilteredResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_pdfReportIsValid;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pdfReportName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_pdfReportPath;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_pdfReportSize;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_pdfReportCreationDt;
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
        public System.Int32 Client
        {
            get { return this.m_client; }
            internal set
            {
                if (this.m_client == value)
                    return;

                this.m_client = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? UserId
        {
            get { return this.m_userId; }
            internal set
            {
                if (this.m_userId == value)
                    return;

                this.m_userId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Survey
        {
            get { return this.m_survey; }
            internal set
            {
                if (this.m_survey == value)
                    return;

                this.m_survey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Guid ViewId
        {
            get { return this.m_viewId; }
            internal set
            {
                if (this.m_viewId == value)
                    return;

                this.m_viewId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
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
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean IsDefaultView
        {
            get { return this.m_isDefaultView; }
            set
            {
                if (this.m_isDefaultView == value)
                    return;

                this.m_isDefaultView = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal System.Int32 AttributeFlags
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
        /// Μας λέει εάν ο χρήστης έχει επιλέξει το PartialShow filtering
        /// </summary>
        public System.Boolean PartialShowInUse
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.PartialShowInUse)) == ((int)ViewAttributes.PartialShowInUse); }
            internal set
            {
                if (this.PartialShowInUse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.PartialShowInUse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.PartialShowInUse;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean EnablePartialShow
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.EnablePartialShow)) == ((int)ViewAttributes.EnablePartialShow); }
            internal set
            {
                if (this.EnablePartialShow == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.EnablePartialShow;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.EnablePartialShow;
            }
        }

        /// <summary>
        /// Μας λέει εάν ο χρήστης έχει επιλέξει το Collector filtering
        /// </summary>
        public System.Boolean FilteringByCollectorInUse
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.FilteringByCollectorInUse)) == ((int)ViewAttributes.FilteringByCollectorInUse); }
            internal set
            {
                if (this.FilteringByCollectorInUse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.FilteringByCollectorInUse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.FilteringByCollectorInUse;
            }
        }
        /// <summary>
        /// Μας λέει εάν ο χρήστης έχει ενεργό ή όχι το Collector filtering
        /// </summary>
        public System.Boolean EnableFilteringByCollector
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.EnableFilteringByCollector)) == ((int)ViewAttributes.EnableFilteringByCollector); }
            internal set
            {
                if (this.EnableFilteringByCollector == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.EnableFilteringByCollector;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.EnableFilteringByCollector;
            }
        }

        /// <summary>
        /// Μας λέει εάν ο χρήστης έχει επιλέξει το TimePeriod filtering
        /// </summary>
        public System.Boolean FilteringByTimePeriodInUse
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.FilteringByTimePeriodInUse)) == ((int)ViewAttributes.FilteringByTimePeriodInUse); }
            internal set
            {
                if (this.FilteringByTimePeriodInUse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.FilteringByTimePeriodInUse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.FilteringByTimePeriodInUse;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean EnableFilteringByTimePeriod
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.EnableFilteringByTimePeriod)) == ((int)ViewAttributes.EnableFilteringByTimePeriod); }
            internal set
            {
                if (this.EnableFilteringByTimePeriod == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.EnableFilteringByTimePeriod;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.EnableFilteringByTimePeriod;
            }
        }

        /// <summary>
        /// Μας λέει εάν ο χρήστης έχει επιλέξει το Total ResponseTime filtering
        /// </summary>
        public System.Boolean FilteringByResponseTimeInUse
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.FilteringByResponseTimeInUse)) == ((int)ViewAttributes.FilteringByResponseTimeInUse); }
            internal set
            {
                if (this.FilteringByResponseTimeInUse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.FilteringByResponseTimeInUse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.FilteringByResponseTimeInUse;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean EnableFilteringByResponseTime
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.EnableFilteringByResponseTime)) == ((int)ViewAttributes.EnableFilteringByResponseTime); }
            internal set
            {
                if (this.EnableFilteringByResponseTime == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.EnableFilteringByResponseTime;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.EnableFilteringByResponseTime;
            }
        }


        /// <summary>
        /// Μας λέει εάν υπάρχει έστω και ένα ενεργό (IsActive) ViewFilter
        /// </summary>
        public System.Boolean FilteringByQuestionInUse
        {
            get { return (this.m_attributeFlags & ((int)ViewAttributes.FilteringByQuestionInUse)) == ((int)ViewAttributes.FilteringByQuestionInUse); }
            internal set
            {
                if (this.FilteringByQuestionInUse == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ViewAttributes.FilteringByQuestionInUse;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ViewAttributes.FilteringByQuestionInUse;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? TimePeriodStart
        {
            get { return this.m_timePeriodStart; }
            set
            {
                if (this.m_timePeriodStart == value)
                    return;

                this.m_timePeriodStart = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? TimePeriodEnd
        {
            get { return this.m_timePeriodEnd; }
            set
            {
                if (this.m_timePeriodEnd == value)
                    return;

                this.m_timePeriodEnd = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? TotalResponseTime
        {
            get { return this.m_totalResponseTime; }
            set
            {
                if (this.m_totalResponseTime == value)
                    return;

                this.m_totalResponseTime = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ResponseTimeUnit? TotalResponseTimeUnit
        {
            get { return this.m_totalResponseTimeUnit; }
            set
            {
                if (this.m_totalResponseTimeUnit == value)
                    return;

                this.m_totalResponseTimeUnit = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ResponseTimeOperator? TotalResponseTimeOperator
        {
            get { return this.m_totalResponseTimeOperator; }
            set
            {
                if (this.m_totalResponseTimeOperator == value)
                    return;

                this.m_totalResponseTimeOperator = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Το πλήθος των ViewFilters που διαθέτει αυτό το View
        /// <para>Συντηρείται αυτόματα απο το σύστημα</para>
        /// </summary>
        public System.Int16 NumberOfQuestionFilters
        {
            get { return this.m_NumberOfQuestionFilters; }
            internal set
            {
                if (this.m_NumberOfQuestionFilters == value)
                    return;

                this.m_NumberOfQuestionFilters = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Η τρέχουσα έκδοση των φίλτρων για αυτό το View.
        /// <para>Κάθε φορά που αλλάζει στα filters αυτό το View, το σύστημα αλλάζει το FiltersVersion, έτσι ώστε να είναι εμφανής η αλλαγή που έγινε στα φίλτρα</para>
        /// </summary>
        public Int32 FiltersVersion
        {
            get { return m_filtersVersion; }
            internal set
            {
                if (this.m_filtersVersion == value)
                    return;

                this.m_filtersVersion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Οταν δημιουργήθηκε το summary για αυτό το view, αυτή ήταν η τιμή του Survey.DesignVersion
        /// </summary>
        public System.Int32? SummaryDesignVersion
        {
            get { return m_summaryDesignVersion; }
            set
            {
                if (m_summaryDesignVersion == value)
                    return;

                m_summaryDesignVersion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Οταν δημιουργήθηκε το summary για αυτό το view, αυτή ήταν η τιμή του Survey.RecordedResponses
        /// <para>Το σύνολο των Responses που έχουν καταχωρηθεί στο σύστημα για το συγκεκριμένο Survey μέσω οποιουδήποτε collector.</para>
        /// <para>O μετρητής αυτός μετράει όλα τα responses είτε έχουν πληρωθεί είτε όχι.</para>
        /// </summary>
        public System.Int32? SummaryRecordedResponses
        {
            get { return m_summaryRecordedResponses; }
            set
            {
                if (m_summaryRecordedResponses == value)
                    return;

                m_summaryRecordedResponses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Οταν δημιουργήθηκε το summary για αυτό το view, αυτή ήταν η τιμή του View.FiltersVersion
        /// </summary>
        public System.Int32? SummaryFiltersVersion
        {
            get { return m_summaryFiltersVersion; }
            set
            {
                if (m_summaryFiltersVersion == value)
                    return;

                m_summaryFiltersVersion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Η ημερομηνία δημιουργίας του αποθηκευμένου sumary, για το τρέχον summary
        /// </summary>
        public System.DateTime? SumaryGenerationDt
        {
            get { return m_sumaryGenerationDt; }
            set
            {
                if (m_sumaryGenerationDt == value)
                    return;

                m_sumaryGenerationDt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Τα VisibleResponses είναι το ΠΟΣΑ απο τα RecordedResponses είναι ορατά απο τον Πελάτη μας.
        /// <para>Ορατά είναι όσα responses έχουν πληρωθεί ή δεν χρεώνονται</para>
        /// <para>Στην δημιουργία του summary, συμμετέχουν πάντα μόνο τα VisibleResponses.</para>
        /// </summary>
        public System.Int32? SummaryVisibleResponses
        {
            get { return m_summaryVisibleResponses; }
            set
            {
                if (m_summaryVisibleResponses == value)
                    return;

                m_summaryVisibleResponses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// To πλήθος των responses που παρέμειναν μετά την εφαρμογή των φίλτρων επάνω στο πλήθος των VisibleResponses
        /// <para>Αυτά μετρήθηκαν και δημιουργήθηκε το summary</para>
        /// </summary>
        public System.Int32? SummaryFilteredResponses
        {
            get { return m_summaryFilteredResponses; }
            set
            {
                if (m_summaryFilteredResponses == value)
                    return;

                m_summaryFilteredResponses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean PdfReportIsValid
        {
            get { return m_pdfReportIsValid; }
            set
            {
                if (m_pdfReportIsValid == value)
                    return;

                m_pdfReportIsValid = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PdfReportName
        {
            get { return m_pdfReportName; }
            set
            {
                if (m_pdfReportName == value)
                    return;

                m_pdfReportName = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PdfReportPath
        {
            get { return m_pdfReportPath; }
            set
            {
                if (m_pdfReportPath == value)
                    return;

                m_pdfReportPath = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? PdfReportSize
        {
            get { return m_pdfReportSize; }
            set
            {
                if (m_pdfReportSize == value)
                    return;

                m_pdfReportSize = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? PdfReportCreationDt
        {
            get { return m_pdfReportCreationDt; }
            set
            {
                if (m_pdfReportCreationDt == value)
                    return;

                m_pdfReportCreationDt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion
        
        #region class constructors
        public VLView()
        {
            m_client = default(Int32);
            m_userId = null;
            m_survey = default(Int32);
            m_viewId = Guid.NewGuid();
            m_name = default(string);
            m_isDefaultView = default(Boolean);
            m_attributeFlags = default(Int32);
            m_timePeriodStart = null;
            m_timePeriodEnd = null;
            m_totalResponseTime = null;
            m_totalResponseTimeUnit = null;
            m_totalResponseTimeOperator = null;
            m_filtersVersion = default(Int32);
            m_summaryDesignVersion = null;
            m_summaryRecordedResponses = null;
            m_summaryFiltersVersion = null;
            m_sumaryGenerationDt = null;
            m_summaryVisibleResponses = null;
            m_summaryFilteredResponses = null;

            m_pdfReportIsValid = false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLView(DbDataReader reader)
            : base(reader)
        {
            this.Client = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) this.UserId = reader.GetInt32(1);
            this.Survey = reader.GetInt32(2);
            this.ViewId = reader.GetGuid(3);
            if (!reader.IsDBNull(4)) this.Name = reader.GetString(4);
            this.IsDefaultView = reader.GetBoolean(5);
            this.AttributeFlags = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.TimePeriodStart = reader.GetDateTime(7);
            if (!reader.IsDBNull(8)) this.TimePeriodEnd = reader.GetDateTime(8);
            if (!reader.IsDBNull(9)) this.TotalResponseTime = reader.GetInt32(9);
            if (!reader.IsDBNull(10)) this.TotalResponseTimeUnit = (ResponseTimeUnit)reader.GetByte(10);
            if (!reader.IsDBNull(11)) this.TotalResponseTimeOperator = (ResponseTimeOperator)reader.GetByte(11);
            if (!reader.IsDBNull(12)) this.NumberOfQuestionFilters = reader.GetInt16(12);
            this.FiltersVersion = reader.GetInt32(13);
            if (!reader.IsDBNull(14)) this.SummaryDesignVersion = reader.GetInt32(14);
            if (!reader.IsDBNull(15)) this.SummaryRecordedResponses = reader.GetInt32(15);
            if (!reader.IsDBNull(16)) this.SummaryFiltersVersion = reader.GetInt32(16);
            if (!reader.IsDBNull(17)) this.SumaryGenerationDt = reader.GetDateTime(17);
            if (!reader.IsDBNull(18)) this.SummaryVisibleResponses = reader.GetInt32(18);
            if (!reader.IsDBNull(19)) this.SummaryFilteredResponses = reader.GetInt32(19);

            this.PdfReportIsValid = reader.GetBoolean(20);
            if (!reader.IsDBNull(21)) this.PdfReportName = reader.GetString(21);
            if (!reader.IsDBNull(22)) this.PdfReportPath = reader.GetString(22);
            if (!reader.IsDBNull(23)) this.PdfReportSize = reader.GetInt32(23);
            if (!reader.IsDBNull(24)) this.PdfReportCreationDt = reader.GetDateTime(24);


            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.Client.GetHashCode() ^
                ((this.UserId == null) ? string.Empty : this.UserId.ToString()).GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.ViewId.GetHashCode() ^
                ((this.Name == null) ? string.Empty : this.Name.ToString()).GetHashCode() ^
                this.IsDefaultView.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.TimePeriodStart == null) ? string.Empty : this.TimePeriodStart.ToString()).GetHashCode() ^
                ((this.TimePeriodEnd == null) ? string.Empty : this.TimePeriodEnd.ToString()).GetHashCode() ^
                ((this.TotalResponseTime == null) ? string.Empty : this.TotalResponseTime.ToString()).GetHashCode() ^
                ((this.TotalResponseTimeUnit == null) ? string.Empty : this.TotalResponseTimeUnit.ToString()).GetHashCode() ^
                ((this.TotalResponseTimeOperator == null) ? string.Empty : this.TotalResponseTimeOperator.ToString()).GetHashCode() ^
                this.NumberOfQuestionFilters.GetHashCode() ^
                this.FiltersVersion.GetHashCode() ^
                ((this.SummaryDesignVersion == null) ? string.Empty : this.SummaryDesignVersion.ToString()).GetHashCode() ^
                ((this.SummaryRecordedResponses == null) ? string.Empty : this.SummaryRecordedResponses.ToString()).GetHashCode() ^
                ((this.SummaryFiltersVersion == null) ? string.Empty : this.SummaryFiltersVersion.ToString()).GetHashCode() ^
                ((this.SumaryGenerationDt == null) ? string.Empty : this.SumaryGenerationDt.ToString()).GetHashCode() ^
                ((this.SummaryVisibleResponses == null) ? string.Empty : this.SummaryVisibleResponses.ToString()).GetHashCode() ^
                ((this.SummaryFilteredResponses == null) ? string.Empty : this.SummaryFilteredResponses.ToString()).GetHashCode() ^
                this.PdfReportIsValid.GetHashCode() ^
                ((this.PdfReportName == null) ? string.Empty : this.PdfReportName.ToString()).GetHashCode() ^
                ((this.PdfReportPath == null) ? string.Empty : this.PdfReportPath.ToString()).GetHashCode() ^
                ((this.PdfReportSize == null) ? string.Empty : this.PdfReportSize.ToString()).GetHashCode() ^
                ((this.PdfReportCreationDt == null) ? string.Empty : this.PdfReportCreationDt.ToString()).GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLView)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            if (!Object.Equals(PdfReportName, other.PdfReportName)) return false;
            if (!Object.Equals(PdfReportPath, other.PdfReportPath)) return false;
            //value types
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_userId.Equals(other.m_userId)) return false;
            if (!m_survey.Equals(other.m_survey)) return false;
            if (!m_viewId.Equals(other.m_viewId)) return false;
            if (!m_isDefaultView.Equals(other.m_isDefaultView)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_timePeriodStart.Equals(other.m_timePeriodStart)) return false;
            if (!m_timePeriodEnd.Equals(other.m_timePeriodEnd)) return false;
            if (!m_totalResponseTime.Equals(other.m_totalResponseTime)) return false;
            if (!m_totalResponseTimeUnit.Equals(other.m_totalResponseTimeUnit)) return false;
            if (!m_totalResponseTimeOperator.Equals(other.m_totalResponseTimeOperator)) return false;
            if (!m_NumberOfQuestionFilters.Equals(other.m_NumberOfQuestionFilters)) return false;
            if (!m_filtersVersion.Equals(other.m_filtersVersion)) return false;
            if (!m_summaryDesignVersion.Equals(other.m_summaryDesignVersion)) return false;
            if (!m_summaryRecordedResponses.Equals(other.m_summaryRecordedResponses)) return false;
            if (!m_summaryFiltersVersion.Equals(other.m_summaryFiltersVersion)) return false;
            if (!m_sumaryGenerationDt.Equals(other.m_sumaryGenerationDt)) return false;
            if (!m_summaryVisibleResponses.Equals(other.m_summaryVisibleResponses)) return false;
            if (!SummaryFilteredResponses.Equals(other.SummaryFilteredResponses)) return false;
            if (!PdfReportIsValid.Equals(other.PdfReportIsValid)) return false;
            if (!PdfReportSize.Equals(other.PdfReportSize)) return false;
            if (!PdfReportCreationDt.Equals(other.PdfReportCreationDt)) return false;

            return true;
        }
        public static Boolean operator ==(VLView o1, VLView o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLView o1, VLView o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_name, false, false, false, 256, "Name");
            Utility.CheckParameter(ref m_pdfReportName, false, false, false, 512, "PdfReportName");
            Utility.CheckParameter(ref m_pdfReportPath, false, false, false, 512, "PdfReportPath");
        }
    }
}
