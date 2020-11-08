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
    [Serializable]
    public sealed class VLClientProfile : VLObject
    {
        [Flags]
        internal enum ClientProfileAttributes : int
        {
            None                        = 0,        //
            IsBuiltIn                   = 1,        // 1 << 0
            UseCredits = 2,        // 1 << 1
            CanTranslateSurveys         = 4,        // 1 << 2
            CanUseSurveyTemplates       = 8,        // 1 << 3
            CanUseQuestionTemplates     = 16,       // 1 << 4
            CanCreateWebLinkCollectors  = 32,       // 1 << 5
            CanCreateEmailCollectors    = 64,       // 1 << 6
            CanCreateWebsiteCollectors  = 128,      // 1 << 7
            CanUseSkipLogic             = 256,      // 1 << 8
            CanExportData               = 512,      // 1 << 9
            CanExportReport             = 1024,     // 1 << 10
            CanUseWebAPI                = 2048,     // 1 << 11

        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_profileId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_comment;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfUsers;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfSurveys;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfLists;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfRecipientsPerList;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfRecipientsPerMessage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfCollectorsPerSurvey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfEmailsPerDay;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfEmailsPerWeek;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfEmailsPerMonth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxNumberOfEmails;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
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
        public System.Int32 ProfileId
        {
            get { return this.m_profileId; }
            internal set
            {
                if (this.m_profileId == value)
                    return;

                this.m_profileId = value;
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
        public System.String Comment
        {
            get { return this.m_comment; }
            set
            {
                if (this.m_comment == value)
                    return;

                this.m_comment = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfUsers
        {
            get { return this.m_maxNumberOfUsers; }
            set
            {
                if (this.m_maxNumberOfUsers == value)
                    return;

                this.m_maxNumberOfUsers = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfSurveys
        {
            get { return this.m_maxNumberOfSurveys; }
            set
            {
                if (this.m_maxNumberOfSurveys == value)
                    return;

                this.m_maxNumberOfSurveys = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfLists
        {
            get { return this.m_maxNumberOfLists; }
            set
            {
                if (this.m_maxNumberOfLists == value)
                    return;

                this.m_maxNumberOfLists = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfRecipientsPerList
        {
            get { return this.m_maxNumberOfRecipientsPerList; }
            set
            {
                if (this.m_maxNumberOfRecipientsPerList == value)
                    return;

                this.m_maxNumberOfRecipientsPerList = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfRecipientsPerMessage
        {
            get { return this.m_maxNumberOfRecipientsPerMessage; }
            set
            {
                if (this.m_maxNumberOfRecipientsPerMessage == value)
                    return;

                this.m_maxNumberOfRecipientsPerMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfCollectorsPerSurvey
        {
            get { return this.m_maxNumberOfCollectorsPerSurvey; }
            set
            {
                if (this.m_maxNumberOfCollectorsPerSurvey == value)
                    return;

                this.m_maxNumberOfCollectorsPerSurvey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfEmailsPerDay
        {
            get { return this.m_maxNumberOfEmailsPerDay; }
            set
            {
                if (this.m_maxNumberOfEmailsPerDay == value)
                    return;

                this.m_maxNumberOfEmailsPerDay = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfEmailsPerWeek
        {
            get { return this.m_maxNumberOfEmailsPerWeek; }
            set
            {
                if (this.m_maxNumberOfEmailsPerWeek == value)
                    return;

                this.m_maxNumberOfEmailsPerWeek = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfEmailsPerMonth
        {
            get { return this.m_maxNumberOfEmailsPerMonth; }
            set
            {
                if (this.m_maxNumberOfEmailsPerMonth == value)
                    return;

                this.m_maxNumberOfEmailsPerMonth = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxNumberOfEmails
        {
            get { return this.m_maxNumberOfEmails; }
            set
            {
                if (this.m_maxNumberOfEmails == value)
                    return;

                this.m_maxNumberOfEmails = value;
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

        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.IsBuiltIn)) == ((int)ClientProfileAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Οταν για έναν πελάτη, το UseCredits είναι true, τότε για να χρησιμοποιήσει το σύστημα και να κανει publish κάποιο
        /// ερωτηματολόγιο, θα πρέπει να διαθέτει 'resources' που προκύπτουν μέσω των πληρωμών του (Payments).
        /// </summary>
        public System.Boolean UseCredits
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.UseCredits)) == ((int)ClientProfileAttributes.UseCredits); }
            internal set
            {
                if (this.UseCredits == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.UseCredits;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.UseCredits;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanTranslateSurveys
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanTranslateSurveys)) == ((int)ClientProfileAttributes.CanTranslateSurveys); }
            internal set
            {
                if (this.CanTranslateSurveys == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanTranslateSurveys;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanTranslateSurveys;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanUseSurveyTemplates
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanUseSurveyTemplates)) == ((int)ClientProfileAttributes.CanUseSurveyTemplates); }
            internal set
            {
                if (this.CanUseSurveyTemplates == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanUseSurveyTemplates;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanUseSurveyTemplates;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanUseQuestionTemplates
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanUseQuestionTemplates)) == ((int)ClientProfileAttributes.CanUseQuestionTemplates); }
            internal set
            {
                if (this.CanUseQuestionTemplates == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanUseQuestionTemplates;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanUseQuestionTemplates;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanCreateWebLinkCollectors
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanCreateWebLinkCollectors)) == ((int)ClientProfileAttributes.CanCreateWebLinkCollectors); }
            internal set
            {
                if (this.CanCreateWebLinkCollectors == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanCreateWebLinkCollectors;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanCreateWebLinkCollectors;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanCreateEmailCollectors
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanCreateEmailCollectors)) == ((int)ClientProfileAttributes.CanCreateEmailCollectors); }
            internal set
            {
                if (this.CanCreateEmailCollectors == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanCreateEmailCollectors;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanCreateEmailCollectors;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanCreateWebsiteCollectors
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanCreateWebsiteCollectors)) == ((int)ClientProfileAttributes.CanCreateWebsiteCollectors); }
            internal set
            {
                if (this.CanCreateWebsiteCollectors == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanCreateWebsiteCollectors;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanCreateWebsiteCollectors;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanUseSkipLogic
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanUseSkipLogic)) == ((int)ClientProfileAttributes.CanUseSkipLogic); }
            internal set
            {
                if (this.CanUseSkipLogic == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanUseSkipLogic;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanUseSkipLogic;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanExportData
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanExportData)) == ((int)ClientProfileAttributes.CanExportData); }
            internal set
            {
                if (this.CanExportData == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanExportData;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanExportData;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanExportReport
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanExportReport)) == ((int)ClientProfileAttributes.CanExportReport); }
            internal set
            {
                if (this.CanExportReport == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanExportReport;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanExportReport;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean CanUseWebAPI
        {
            get { return (this.m_attributeFlags & ((int)ClientProfileAttributes.CanUseWebAPI)) == ((int)ClientProfileAttributes.CanUseWebAPI); }
            internal set
            {
                if (this.CanUseWebAPI == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)ClientProfileAttributes.CanUseWebAPI;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)ClientProfileAttributes.CanUseWebAPI;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLClientProfile()
        {
            m_profileId = default(Int32);
            m_name = default(string);
            m_comment = default(string);
            m_maxNumberOfUsers = null;
            m_maxNumberOfSurveys = null;
            m_maxNumberOfLists = null;
            m_maxNumberOfRecipientsPerList = null;
            m_maxNumberOfRecipientsPerMessage = null;
            m_maxNumberOfCollectorsPerSurvey = null;
            m_maxNumberOfEmailsPerDay = null;
            m_maxNumberOfEmailsPerWeek = null;
            m_maxNumberOfEmailsPerMonth = null;
            m_maxNumberOfEmails = null;
            m_attributeFlags = default(Int32);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLClientProfile(DbDataReader reader)
            : base(reader)
        {
            this.ProfileId = reader.GetInt32(0);
            this.Name = reader.GetString(1);
            if (!reader.IsDBNull(2)) this.Comment = reader.GetString(2);
            if (!reader.IsDBNull(3)) this.MaxNumberOfUsers = reader.GetInt32(3);
            if (!reader.IsDBNull(4)) this.MaxNumberOfSurveys = reader.GetInt32(4);
            if (!reader.IsDBNull(5)) this.MaxNumberOfLists = reader.GetInt32(5);
            if (!reader.IsDBNull(6)) this.MaxNumberOfRecipientsPerList = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.MaxNumberOfRecipientsPerMessage = reader.GetInt32(7);
            if (!reader.IsDBNull(8)) this.MaxNumberOfCollectorsPerSurvey = reader.GetInt32(8);
            if (!reader.IsDBNull(9)) this.MaxNumberOfEmailsPerDay = reader.GetInt32(9);
            if (!reader.IsDBNull(10)) this.MaxNumberOfEmailsPerWeek = reader.GetInt32(10);
            if (!reader.IsDBNull(11)) this.MaxNumberOfEmailsPerMonth = reader.GetInt32(11);
            if (!reader.IsDBNull(12)) this.MaxNumberOfEmails = reader.GetInt32(12);
            this.AttributeFlags = reader.GetInt32(13);

        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.ProfileId.GetHashCode() ^
                this.Name.GetHashCode() ^
                ((this.Comment == null) ? string.Empty : this.Comment.ToString()).GetHashCode() ^
                ((this.MaxNumberOfUsers == null) ? string.Empty : this.MaxNumberOfUsers.ToString()).GetHashCode() ^
                ((this.MaxNumberOfSurveys == null) ? string.Empty : this.MaxNumberOfSurveys.ToString()).GetHashCode() ^
                ((this.MaxNumberOfLists == null) ? string.Empty : this.MaxNumberOfLists.ToString()).GetHashCode() ^
                ((this.MaxNumberOfRecipientsPerList == null) ? string.Empty : this.MaxNumberOfRecipientsPerList.ToString()).GetHashCode() ^
                ((this.MaxNumberOfRecipientsPerMessage == null) ? string.Empty : this.MaxNumberOfRecipientsPerMessage.ToString()).GetHashCode() ^
                ((this.MaxNumberOfCollectorsPerSurvey == null) ? string.Empty : this.MaxNumberOfCollectorsPerSurvey.ToString()).GetHashCode() ^
                ((this.MaxNumberOfEmailsPerDay == null) ? string.Empty : this.MaxNumberOfEmailsPerDay.ToString()).GetHashCode() ^
                ((this.MaxNumberOfEmailsPerWeek == null) ? string.Empty : this.MaxNumberOfEmailsPerWeek.ToString()).GetHashCode() ^
                ((this.MaxNumberOfEmailsPerMonth == null) ? string.Empty : this.MaxNumberOfEmailsPerMonth.ToString()).GetHashCode() ^
                ((this.MaxNumberOfEmails == null) ? string.Empty : this.MaxNumberOfEmails.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode();
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


            var other = (VLClientProfile)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            if (!Object.Equals(Comment, other.Comment)) return false;
            //value types
            if (!ProfileId.Equals(other.ProfileId)) return false;
            if (!MaxNumberOfUsers.Equals(other.MaxNumberOfUsers)) return false;
            if (!MaxNumberOfSurveys.Equals(other.MaxNumberOfSurveys)) return false;
            if (!MaxNumberOfLists.Equals(other.MaxNumberOfLists)) return false;
            if (!MaxNumberOfRecipientsPerList.Equals(other.MaxNumberOfRecipientsPerList)) return false;
            if (!MaxNumberOfRecipientsPerMessage.Equals(other.MaxNumberOfRecipientsPerMessage)) return false;
            if (!MaxNumberOfCollectorsPerSurvey.Equals(other.MaxNumberOfCollectorsPerSurvey)) return false;
            if (!MaxNumberOfEmailsPerDay.Equals(other.MaxNumberOfEmailsPerDay)) return false;
            if (!MaxNumberOfEmailsPerWeek.Equals(other.MaxNumberOfEmailsPerWeek)) return false;
            if (!MaxNumberOfEmailsPerMonth.Equals(other.MaxNumberOfEmailsPerMonth)) return false;
            if (!MaxNumberOfEmails.Equals(other.MaxNumberOfEmails)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLClientProfile o1, VLClientProfile o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLClientProfile o1, VLClientProfile o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            ValidateName(ref m_name);
            Utility.CheckParameter(ref m_comment, false, false, false, -1, "Comment");


            if (m_maxNumberOfUsers.HasValue && m_maxNumberOfUsers <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfUsers property!", m_maxNumberOfUsers));
            }
            if (m_maxNumberOfSurveys.HasValue && m_maxNumberOfSurveys <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfSurveys property!", m_maxNumberOfSurveys));
            }
            if (m_maxNumberOfLists.HasValue && m_maxNumberOfLists <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfLists property!", m_maxNumberOfLists));
            }
            if (m_maxNumberOfRecipientsPerList.HasValue && m_maxNumberOfRecipientsPerList <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfRecipientsPerList property!", m_maxNumberOfRecipientsPerList));
            }
            if (m_maxNumberOfRecipientsPerMessage.HasValue && m_maxNumberOfRecipientsPerMessage <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfRecipientsPerMessage property!", m_maxNumberOfRecipientsPerMessage));
            }
            if (m_maxNumberOfCollectorsPerSurvey.HasValue && m_maxNumberOfCollectorsPerSurvey <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfCollectorsPerSurvey property!", m_maxNumberOfCollectorsPerSurvey));
            }
            if (m_maxNumberOfEmailsPerDay.HasValue && m_maxNumberOfEmailsPerDay <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfEmailsPerDay property!", m_maxNumberOfEmailsPerDay));
            }
            if (m_maxNumberOfEmailsPerWeek.HasValue && m_maxNumberOfEmailsPerWeek <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfEmailsPerWeek property!", m_maxNumberOfEmailsPerWeek));
            }
            if (m_maxNumberOfEmailsPerMonth.HasValue && m_maxNumberOfEmailsPerMonth <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfEmailsPerMonth property!", m_maxNumberOfEmailsPerMonth));
            }
            if (m_maxNumberOfEmails.HasValue && m_maxNumberOfEmails <= 0)
            {
                throw new VLException(string.Format("Value '{0}' is invalid for MaxNumberOfEmails property!", m_maxNumberOfEmails));
            }
        }
        internal static void ValidateName(ref string name)
        {
            Utility.CheckParameter(ref name, true, true, false, 128, "Name");
        }
        
    }
}
