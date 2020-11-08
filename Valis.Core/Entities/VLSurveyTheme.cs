using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    [DataContract, DataObject]
    public sealed class VLSurveyTheme : VLObject
    {

        [Flags]
        internal enum SurveyThemeAttributes : int
        {
            None = 0,
            IsBuiltIn   = 1,           // 1 << 0
            IsVisible   = 2,           // 1 << 1
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_themeId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_clientId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_rtHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_rtCSS;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_dtHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_dtCSS;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        #endregion


        #region EntityState
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
        bool _deserializing = false;


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 ThemeId
        {
            get { return m_themeId; }
            internal set
            {
                if (this.m_themeId == value)
                    return;

                this.m_themeId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? ClientId
        {
            get { return m_clientId; }
            internal set
            {
                if (this.m_clientId == value)
                    return;

                this.m_clientId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public String Name
        {
            get { return m_name; }
            internal set
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
        public System.String RtHtml
        {
            get { return this.m_rtHtml; }
            set
            {
                if (this.m_rtHtml == value)
                    return;

                this.m_rtHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String RtCSS
        {
            get { return this.m_rtCSS; }
            set
            {
                if (this.m_rtCSS == value)
                    return;

                this.m_rtCSS = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String DtHtml
        {
            get { return this.m_dtHtml; }
            set
            {
                if (this.m_dtHtml == value)
                    return;

                this.m_dtHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String DtCSS
        {
            get { return this.m_dtCSS; }
            set
            {
                if (this.m_dtCSS == value)
                    return;

                this.m_dtCSS = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 AttributeFlags
        {
            get { return m_attributeFlags; }
            set
            {
                if (this.m_attributeFlags == value)
                    return;

                this.m_attributeFlags = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }


        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)SurveyThemeAttributes.IsBuiltIn)) == ((int)SurveyThemeAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyThemeAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyThemeAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        #endregion


        #region class constructors
        public VLSurveyTheme()
        {
            m_themeId = default(Int32);
            m_clientId = null;
            m_name = default(string);
            m_rtHtml = default(string);
            m_rtCSS = default(string);
            m_dtHtml = default(string);
            m_dtCSS = default(string);
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSurveyTheme(DbDataReader reader)
            : base(reader)
        {
            this.ThemeId = reader.GetInt32(0);
            if (!reader.IsDBNull(1)) this.ClientId = reader.GetInt32(1);
            this.Name = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.RtHtml = reader.GetString(3);
            if (!reader.IsDBNull(4)) this.RtCSS = reader.GetString(4);
            if (!reader.IsDBNull(5)) this.DtHtml = reader.GetString(5);
            if (!reader.IsDBNull(6)) this.DtCSS = reader.GetString(6);
            this.AttributeFlags = reader.GetInt32(7);

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        public override int GetHashCode()
        {
            return this.ThemeId.GetHashCode() ^
                ((this.ClientId == null) ? string.Empty : this.ClientId.ToString()).GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.RtHtml == null) ? string.Empty : this.RtHtml.ToString()).GetHashCode() ^
                ((this.RtCSS == null) ? string.Empty : this.RtCSS.ToString()).GetHashCode() ^
                ((this.DtHtml == null) ? string.Empty : this.DtHtml.ToString()).GetHashCode() ^
                ((this.DtCSS == null) ? string.Empty : this.DtCSS.ToString()).GetHashCode() ^
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


            var other = (VLSurveyTheme)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            if (!Object.Equals(RtHtml, other.RtHtml)) return false;
            if (!Object.Equals(RtCSS, other.RtCSS)) return false;
            if (!Object.Equals(DtHtml, other.DtHtml)) return false;
            if (!Object.Equals(DtCSS, other.DtCSS)) return false;
            //value types
            if (!ThemeId.Equals(other.ThemeId)) return false;
            if (!ClientId.Equals(other.ClientId)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;

            return true; 
        }
        public static Boolean operator ==(VLSurveyTheme o1, VLSurveyTheme o2)
        {
            return Object.Equals(o1, o2);
        }
        public static Boolean operator !=(VLSurveyTheme o1, VLSurveyTheme o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_name, true, true, false, 128, "Name");
            Utility.CheckParameter(ref m_rtHtml, false, false, false, -1, "RtHtml");
            Utility.CheckParameter(ref m_rtCSS, false, false, false, -1, "RtCSS");
            Utility.CheckParameter(ref m_dtHtml, false, false, false, -1, "DtHtml");
            Utility.CheckParameter(ref m_dtCSS, false, false, false, -1, "DtCSS");
        }
    }
}
