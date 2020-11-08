using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLViewCollector
    {
        [Flags]
        internal enum ViewCollectorAttributes : int
        {
            None = 0,
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collector;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CollectorType m_collectorType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_collectorName;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_includeResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
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
        public Int32 Collector
        {
            get { return m_collector; }
            internal set { m_collector = value; }
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
        public CollectorType CollectorType
        {
            get { return m_collectorType; }
            internal set { m_collectorType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String CollectorName
        {
            get { return m_collectorName; }
            internal set { m_collectorName = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IncludeResponses
        {
            get { return m_includeResponses; }
            set { m_includeResponses = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set { m_attributeFlags = value; }
        }
        #endregion

        #region class constructors
        public VLViewCollector()
        {
            m_viewId = default(Guid);
            m_collector = default(Int32);
            m_survey = default(Int32);
            m_collectorType = default(Byte);
            m_collectorName = default(string);
            m_includeResponses = default(Boolean);
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLViewCollector(DbDataReader reader)
        {
            this.ViewId = reader.GetGuid(0);
            this.Collector = reader.GetInt32(1);
            this.Survey = reader.GetInt32(2);
            this.CollectorType = (CollectorType)reader.GetByte(3);
            this.CollectorName = reader.GetString(4);
            this.IncludeResponses = reader.GetBoolean(5);
            this.AttributeFlags = reader.GetInt32(6);

        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ViewId.GetHashCode() ^
                this.Collector.GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.CollectorType.GetHashCode() ^
                this.CollectorName.GetHashCode() ^
                this.IncludeResponses.GetHashCode() ^
                this.AttributeFlags.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;


            var other = (VLViewCollector)obj;

            //reference types
            if (!Object.Equals(m_collectorName, other.m_collectorName)) return false;
            //value types
            if (!m_viewId.Equals(other.m_viewId)) return false;
            if (!m_collector.Equals(other.m_collector)) return false;
            if (!m_survey.Equals(other.m_survey)) return false;
            if (!m_collectorType.Equals(other.m_collectorType)) return false;
            if (!m_includeResponses.Equals(other.m_includeResponses)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLViewCollector o1, VLViewCollector o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLViewCollector o1, VLViewCollector o2)
        {
            return !(o1 == o2);
        }

        #endregion
    }
}
