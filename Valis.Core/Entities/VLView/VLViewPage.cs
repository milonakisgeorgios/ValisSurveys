using System;
using System.Data.Common;
using System.Diagnostics;

namespace Valis.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VLViewPage
    {
        [Flags]
        internal enum ViewPageAttributes : int
        {
            None = 0,
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Guid m_viewId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_page;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_showResponses;
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
        public Int32 Survey
        {
            get { return m_survey; }
            internal set { m_survey = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16 Page
        {
            get { return m_page; }
            internal set { m_page = value; }
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
        #endregion

        #region class constructors
        public VLViewPage()
        {
            m_viewId = default(Guid);
            m_survey = default(Int32);
            m_page = default(Int16);
            m_showResponses = default(Boolean);
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLViewPage(DbDataReader reader)
        {
            this.ViewId = reader.GetGuid(0);
            this.Survey = reader.GetInt32(1);
            this.Page = reader.GetInt16(2);
            this.ShowResponses = reader.GetBoolean(3);
            this.AttributeFlags = reader.GetInt32(4);
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ViewId.GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.Page.GetHashCode() ^
                this.ShowResponses.GetHashCode() ^
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


            var other = (VLViewPage)obj;

            //reference types
            //value types
            if (!m_viewId.Equals(other.m_viewId)) return false;
            if (!m_survey.Equals(other.m_survey)) return false;
            if (!m_page.Equals(other.m_page)) return false;
            if (!m_showResponses.Equals(other.m_showResponses)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;

            return true;
        }
        public static Boolean operator ==(VLViewPage o1, VLViewPage o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLViewPage o1, VLViewPage o2)
        {
            return !(o1 == o2);
        }

        #endregion
    }
}
