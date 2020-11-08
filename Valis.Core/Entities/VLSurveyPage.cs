using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Valis.Core
{
    public sealed class VLSurveyPage : VLObject
    {

        [Flags]
        internal enum SurveyPageAttributes : int
        {
            None = 0,
            XXxxxx = 1,                 // 1 << 0
            HasSkipLogic = 512,           //1 << 9
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_pageId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_displayOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_previousPage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_nextPage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_CustomId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        SkipToBehavior m_skipTo;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_skipToPage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_skipToWebUrl;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_showTitle;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_description;
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
        public System.Int16 PageId
        {
            get { return this.m_pageId; }
            internal set
            {
                if (this.m_pageId == value)
                    return;

                this.m_pageId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 DisplayOrder
        {
            get { return this.m_displayOrder; }
            set
            {
                if (this.m_displayOrder == value)
                    return;

                this.m_displayOrder = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? PreviousPage
        {
            get { return this.m_previousPage; }
            set
            {
                if (this.m_previousPage == value)
                    return;

                this.m_previousPage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? NextPage
        {
            get { return this.m_nextPage; }
            set
            {
                if (this.m_nextPage == value)
                    return;

                this.m_nextPage = value;
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
        /// Μας λέει εάν η συγκεκριμένη σελίδα έχει ενεργοποιημένο Logic.
        /// </summary>
        public System.Boolean HasSkipLogic
        {
            get { return (this.m_attributeFlags & ((int)SurveyPageAttributes.HasSkipLogic)) == ((int)SurveyPageAttributes.HasSkipLogic); }
            internal set
            {
                if (this.HasSkipLogic == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyPageAttributes.HasSkipLogic;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyPageAttributes.HasSkipLogic;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Ενας κωδικός (τον οποίο τον δίνει ο Πελάτης) και συνοδεύει το export του surveypage
        /// </summary>
        public String CustomId
        {
            get { return m_CustomId; }
            set
            {
                if (this.m_CustomId == value)
                    return;

                this.m_CustomId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
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
        public String SkipToWebUrl
        {
            get { return m_skipToWebUrl; }
            set { m_skipToWebUrl = value; }
        }

        /// <summary>
        /// The language of thetranslatable fields..
        /// </summary>
        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ShowTitle
        {
            get { return this.m_showTitle; }
            set
            {
                if (this.m_showTitle == value)
                    return;

                this.m_showTitle = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Description
        {
            get { return this.m_description; }
            set
            {
                if (this.m_description == value)
                    return;

                this.m_description = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion

        #region class internal properties
        internal Boolean HasValidationErrors { get; set; }
        internal Boolean HasMissingValues { get; set; }
        #endregion

        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        public VLSurveyPage()
        {
            m_survey = default(Int32);
            m_pageId = default(Int16);
            m_displayOrder = default(Int16);
            m_previousPage = null;
            m_nextPage = null;
            m_attributeFlags = default(Int32);
            m_CustomId = default(string);
            m_skipTo = SkipToBehavior.None;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSurveyPage(DbDataReader reader)
            : base(reader)
        {
            this.Survey = reader.GetInt32(0);
            this.PageId = reader.GetInt16(1);
            this.DisplayOrder = reader.GetInt16(2);
            if (!reader.IsDBNull(3)) this.PreviousPage = reader.GetInt32(3);
            if (!reader.IsDBNull(4)) this.NextPage = reader.GetInt32(4);
            this.AttributeFlags = reader.GetInt32(5);
            if (!reader.IsDBNull(6)) this.CustomId = reader.GetString(6);
            this.SkipTo = (SkipToBehavior)reader.GetByte(7);
            if (!reader.IsDBNull(8)) this.SkipToPage = reader.GetInt16(8);
            if (!reader.IsDBNull(9)) this.SkipToWebUrl = reader.GetString(9);
            this.m_textsLanguage = reader.GetInt16(10);
            if (!reader.IsDBNull(11)) this.ShowTitle = reader.GetString(11);
            if (!reader.IsDBNull(12)) this.Description = reader.GetString(12);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        public VLSurveyPage(VLSurveyPage source)
        {
            this.m_survey = default(Int32);
            this.m_pageId = default(Int16);
            this.m_displayOrder = default(Int16);

            this.m_previousPage = source.m_previousPage;
            this.m_nextPage = source.m_nextPage;
            this.m_attributeFlags = default(Int32);
            this.m_CustomId = default(string);
            this.m_textsLanguage = source.m_textsLanguage;
            this.m_showTitle = source.m_showTitle;
            this.m_description = source.m_description;
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
                this.PageId.GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                ((this.PreviousPage == null) ? string.Empty : this.PreviousPage.ToString()).GetHashCode() ^
                ((this.NextPage == null) ? string.Empty : this.NextPage.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                ((this.CustomId == null) ? string.Empty : this.CustomId.ToString()).GetHashCode() ^
                this.SkipTo.GetHashCode() ^
                ((this.SkipToPage == null) ? string.Empty : this.SkipToPage.ToString()).GetHashCode() ^
                ((this.SkipToWebUrl == null) ? string.Empty : this.SkipToWebUrl.ToString()).GetHashCode() ^
                ((this.ShowTitle == null) ? string.Empty : this.ShowTitle.ToString()).GetHashCode() ^
                ((this.Description == null) ? string.Empty : this.Description.ToString()).GetHashCode();
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


            var other = (VLSurveyPage)obj;

            //reference types
            if (!Object.Equals(CustomId, other.CustomId)) return false;
            if (!Object.Equals(SkipToWebUrl, other.SkipToWebUrl)) return false;
            if (!Object.Equals(ShowTitle, other.ShowTitle)) return false;
            if (!Object.Equals(Description, other.Description)) return false;
            //value types
            if (!Survey.Equals(other.Survey)) return false;
            if (!PageId.Equals(other.PageId)) return false;
            if (!DisplayOrder.Equals(other.DisplayOrder)) return false;
            if (!PreviousPage.Equals(other.PreviousPage)) return false;
            if (!NextPage.Equals(other.NextPage)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!SkipTo.Equals(other.SkipTo)) return false;
            if (!SkipToPage.Equals(other.SkipToPage)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLSurveyPage o1, VLSurveyPage o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLSurveyPage o1, VLSurveyPage o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_showTitle, false, false, false, 128, "ShowTitle");
            Utility.CheckParameter(ref m_description, false, false, false, 2048, "Description");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}:{1} -> {2}", this.PageId, BuiltinLanguages.GetLanguageById(this.TextsLanguage).Name, this.ShowTitle);
        }
    }
}
