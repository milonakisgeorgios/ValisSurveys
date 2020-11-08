using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    [DataContract, DataObject]
    public sealed class VLCollector : VLObject
    {
        [Flags]
        internal enum CollectorAttributes : int
        {
            None                                = 0,
            AllowMultipleResponsesPerComputer   = 1,           // 1 << 0
            Attribute01                         = 2,           // 1 << 1
            DisplayInstantResults               = 4,           // 1 << 2
            DisplayNumberOfResponses            = 8,           // 1 << 3
            Attribute012                        = 16,          // 1 << 4
            AllowResponseInspection             = 32,          // 1 << 5
            UseSSL                              = 64,          // 1 << 6
            SaveIPAddressOrEmail                = 128,         // 1 << 7
            EnableStopCollectorDT               = 256,         // 1 << 8
            EnableMaxResponseCount              = 512,         // 1 << 9
            EnablePasswordProtection            = 1024,        // 1 << 10
            EnablePersonalPasswordProtection    = 2048,         // 1 << 11
            EnableIPBlocking                    = 4096,         // 1 << 12

            WebUseFrame                         = 8192,         // 1 << 13
            WebShowBorder                       = 16384,        // 1 << 14
            WebHideSurvey                       = 32768,        // 1 << 15
            WebContinuePopupUntilCompletion     = 65536,        // 1 << 16

            HasSentEmails                       = 131073,        // 1 << 17
            ClosedByUser                        = 262144,        // 1 << 18
            UseCredits = 524288        // 1 << 19
        }


        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_collectorId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CollectorType m_collectorType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CollectorStatus m_status;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_responses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webLink;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EditResponseModes m_editResponseMode;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EndSurveyMode m_onCompletionMode;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_stopCollectorDT;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_maxResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_password;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        WebSiteSurveyType? m_webSurveyType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_webWidth;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_webHeight;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_webPopupEvery;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webFontColor;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webBackgroundColor;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        CreditType? m_creditType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_firstChargeDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_lastChargeDt;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_profile;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_primaryLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_supportedLanguagesIds;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        VLSupportedLanguagesColection m_supportedLanguages = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_onCompletionURL;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_disqualificationPageURL;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_passwordLabel;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_submitButtonLabel;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_passwordRequiredMessage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_passwordFailedMessage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webInvitation;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webNeverButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webLaterButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_webNowButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_closedMessage;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_totalMessages;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_draftMessages;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_runningMessages;
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
        public System.Int32 CollectorId
        {
            get { return this.m_collectorId; }
            internal set
            {
                if (this.m_collectorId == value)
                    return;

                this.m_collectorId = value;
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
        /// The type of the collector or the method of the collection. We can collect responses via different ways:
        /// ->by using a web link to send via email or post to your web site
        /// ->by email invitation
        /// ->by website
        /// ...
        /// </summary>
        public CollectorType CollectorType
        {
            get { return this.m_collectorType; }
            internal set
            {
                if (this.m_collectorType == value)
                    return;

                this.m_collectorType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// This is the name of the collector. The collectors which belong to the same survey must have unique names.
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
        /// Allow multiple responses per computer -- Recommended for kiosks or computer labs.
        /// </summary>
        public System.Boolean AllowMultipleResponsesPerComputer
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.AllowMultipleResponsesPerComputer)) == ((int)CollectorAttributes.AllowMultipleResponsesPerComputer); }
            internal set
            {
                if (this.AllowMultipleResponsesPerComputer == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.AllowMultipleResponsesPerComputer;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.AllowMultipleResponsesPerComputer;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        ///  Display results after a respondent completes the survey. 
        /// </summary>
        public System.Boolean DisplayInstantResults
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.DisplayInstantResults)) == ((int)CollectorAttributes.DisplayInstantResults); }
            internal set
            {
                if (this.DisplayInstantResults == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.DisplayInstantResults;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.DisplayInstantResults;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        ///  Display the number of responses for each question and answer option.
        /// </summary>
        public System.Boolean DisplayNumberOfResponses
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.DisplayNumberOfResponses)) == ((int)CollectorAttributes.DisplayNumberOfResponses); }
            internal set
            {
                if (this.DisplayNumberOfResponses == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.DisplayNumberOfResponses;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.DisplayNumberOfResponses;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean AllowResponseInspection
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.AllowResponseInspection)) == ((int)CollectorAttributes.AllowResponseInspection); }
            internal set
            {
                if (this.AllowResponseInspection == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.AllowResponseInspection;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.AllowResponseInspection;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean UseSSL
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.UseSSL)) == ((int)CollectorAttributes.UseSSL); }
            internal set
            {
                if (this.UseSSL == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.UseSSL;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.UseSSL;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean SaveIPAddressOrEmail
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.SaveIPAddressOrEmail)) == ((int)CollectorAttributes.SaveIPAddressOrEmail); }
            internal set
            {
                if (this.SaveIPAddressOrEmail == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.SaveIPAddressOrEmail;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.SaveIPAddressOrEmail;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Εάν ο Collector απενεργοποιηθεί μόνος του, απο μία συγκεκριμένη ημερομηνία και μετά (StopCollectorDT).
        /// <para>Πάει ζευγάρι με το πεδίο StopCollectorDT</para>
        /// </summary>
        public System.Boolean EnableStopCollectorDT
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.EnableStopCollectorDT)) == ((int)CollectorAttributes.EnableStopCollectorDT); }
            internal set
            {
                if (this.EnableStopCollectorDT == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.EnableStopCollectorDT;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.EnableStopCollectorDT;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean EnableMaxResponseCount
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.EnableMaxResponseCount)) == ((int)CollectorAttributes.EnableMaxResponseCount); }
            internal set
            {
                if (this.EnableMaxResponseCount == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.EnableMaxResponseCount;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.EnableMaxResponseCount;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean EnablePasswordProtection
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.EnablePasswordProtection)) == ((int)CollectorAttributes.EnablePasswordProtection); }
            internal set
            {
                if (this.EnablePasswordProtection == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.EnablePasswordProtection;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.EnablePasswordProtection;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean EnablePersonalPasswordProtection
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.EnablePersonalPasswordProtection)) == ((int)CollectorAttributes.EnablePersonalPasswordProtection); }
            internal set
            {
                if (this.EnablePersonalPasswordProtection == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.EnablePersonalPasswordProtection;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.EnablePersonalPasswordProtection;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean EnableIPBlocking
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.EnableIPBlocking)) == ((int)CollectorAttributes.EnableIPBlocking); }
            internal set
            {
                if (this.EnableIPBlocking == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.EnableIPBlocking;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.EnableIPBlocking;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean WebUseFrame
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.WebUseFrame)) == ((int)CollectorAttributes.WebUseFrame); }
            internal set
            {
                if (this.WebUseFrame == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.WebUseFrame;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.WebUseFrame;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean WebShowBorder
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.WebShowBorder)) == ((int)CollectorAttributes.WebShowBorder); }
            internal set
            {
                if (this.WebShowBorder == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.WebShowBorder;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.WebShowBorder;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean WebHideSurvey
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.WebHideSurvey)) == ((int)CollectorAttributes.WebHideSurvey); }
            internal set
            {
                if (this.WebHideSurvey == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.WebHideSurvey;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.WebHideSurvey;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean WebContinuePopupUntilCompletion
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.WebContinuePopupUntilCompletion)) == ((int)CollectorAttributes.WebContinuePopupUntilCompletion); }
            internal set
            {
                if (this.WebContinuePopupUntilCompletion == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.WebContinuePopupUntilCompletion;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.WebContinuePopupUntilCompletion;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει εάν απο αυτόν τον collector, έχει σταλεί έστω καi ένα email
        /// </summary>
        public System.Boolean HasSentEmails
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.HasSentEmails)) == ((int)CollectorAttributes.HasSentEmails); }
            internal set
            {
                if (this.HasSentEmails == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.HasSentEmails;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.HasSentEmails;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.Boolean ClosedByUser
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.ClosedByUser)) == ((int)CollectorAttributes.ClosedByUser); }
            internal set
            {
                if (this.ClosedByUser == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.ClosedByUser;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.ClosedByUser;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Αποθηκεύει την τιμή (ClientProfile.)UseCredits απο το profile του Client, την στιγμή της δημιουργίας του Collector.
        /// <para>Εάν το UseCredits είναι FALSE, τότε η λειτουργία του Collector ΔΕΝ ΧΡΕΩΝΕΤΑΙ!</para>
        /// <para>To σύστημα για να προβεί σε χρέωση για την λειτουργία ενός Collector, ελέγχει πάντα τα πεδία UseCredits και το CreditType!</para>
        /// <para>(Μπορεί να μήν είναι το τρέχων UseCredits του client!)</para>
        /// </summary>
        public System.Boolean UseCredits
        {
            get { return (this.m_attributeFlags & ((int)CollectorAttributes.UseCredits)) == ((int)CollectorAttributes.UseCredits); }
            internal set
            {
                if (this.UseCredits == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)CollectorAttributes.UseCredits;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)CollectorAttributes.UseCredits;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CollectorStatus Status
        {
            get { return this.m_status; }
            set
            {
                if (this.m_status == value)
                    return;

                this.m_status = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 Responses
        {
            get { return this.m_responses; }
            internal set
            {
                if (this.m_responses == value)
                    return;

                this.m_responses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebLink
        {
            get { return this.m_webLink; }
            set
            {
                if (this.m_webLink == value)
                    return;

                this.m_webLink = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public EditResponseModes EditResponseMode
        {
            get { return this.m_editResponseMode; }
            set
            {
                if (this.m_editResponseMode == value)
                    return;

                this.m_editResponseMode = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public EndSurveyMode OnCompletionMode
        {
            get { return this.m_onCompletionMode; }
            set
            {
                if (this.m_onCompletionMode == value)
                    return;

                this.m_onCompletionMode = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Ποια ημερομηνία θέλουμε ο Collector να σταματήσει να συλλέγει απαντήσεις
        /// </summary>
        public System.DateTime? StopCollectorDT
        {
            get { return this.m_stopCollectorDT; }
            set
            {
                if (this.m_stopCollectorDT == value)
                    return;

                this.m_stopCollectorDT = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MaxResponses
        {
            get { return this.m_maxResponses; }
            set
            {
                if (this.m_maxResponses == value)
                    return;

                this.m_maxResponses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String Password
        {
            get { return this.m_password; }
            set
            {
                if (this.m_password == value)
                    return;

                this.m_password = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public WebSiteSurveyType? WebSurveyType
        {
            get { return this.m_webSurveyType; }
            set
            {
                if (this.m_webSurveyType == value)
                    return;

                this.m_webSurveyType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 WebWidth
        {
            get { return this.m_webWidth; }
            set
            {
                if (this.m_webWidth == value)
                    return;

                this.m_webWidth = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 WebHeight
        {
            get { return this.m_webHeight; }
            set
            {
                if (this.m_webHeight == value)
                    return;

                this.m_webHeight = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 WebPopupEvery
        {
            get { return this.m_webPopupEvery; }
            set
            {
                if (this.m_webPopupEvery == value)
                    return;

                this.m_webPopupEvery = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebFontColor
        {
            get { return this.m_webFontColor; }
            set
            {
                if (this.m_webFontColor == value)
                    return;

                this.m_webFontColor = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebBackgroundColor
        {
            get { return this.m_webBackgroundColor; }
            set
            {
                if (this.m_webBackgroundColor == value)
                    return;

                this.m_webBackgroundColor = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Ο τύπος του resource που χρεώνεται για την λειτουργία αυτού του Collector!
        /// <para>Εάν το ResourceType είναι NULL, τότε η λειτουργία του Collector ΔΕΝ ΧΡΕΩΝΕΤΑΙ!</para>
        /// <para>To σύστημα για να προβεί σε χρέωση για την λειτουργία ενός Collector, ελέγχει πάντα τα πεδία UseCredits και το CreditType!</para>
        /// </summary>
        public CreditType? CreditType
        {
            get { return this.m_creditType; }
            internal set
            {
                if (this.m_creditType == value)
                    return;

                this.m_creditType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? FirstChargeDt
        {
            get { return this.m_firstChargeDt; }
            set
            {
                if (this.m_firstChargeDt == value)
                    return;

                this.m_firstChargeDt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? LastChargeDt
        {
            get { return this.m_lastChargeDt; }
            set
            {
                if (this.m_lastChargeDt == value)
                    return;

                this.m_lastChargeDt = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Αποθηκεύει το Profile του Client, την στιγμή της δημιουργίας του Collector.
        /// <para>(Μπορεί να μήν είναι το τρέχων Profile του client!)</para>
        /// </summary>
        public System.Int32? Profile
        {
            get { return this.m_profile; }
            set
            {
                if (this.m_profile == value)
                    return;

                this.m_profile = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        internal Int16 PrimaryLanguage
        {
            get { return m_primaryLanguage; }
            set
            {
                if (this.m_primaryLanguage == value)
                    return;

                this.m_primaryLanguage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        internal String SupportedLanguagesIds
        {
            get { return m_supportedLanguagesIds; }
            set
            {
                if (this.m_supportedLanguagesIds == value)
                    return;

                this.m_supportedLanguagesIds = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public VLSupportedLanguagesColection SupportedLanguages
        {
            get
            {
                if (this.m_supportedLanguages == null)
                {
                    this.m_supportedLanguages = new VLSupportedLanguagesColection(this.SupportedLanguagesIds);
                }
                return this.m_supportedLanguages;
            }
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
        public System.String OnCompletionURL
        {
            get { return this.m_onCompletionURL; }
            set
            {
                if (this.m_onCompletionURL == value)
                    return;

                this.m_onCompletionURL = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String DisqualificationPageURL
        {
            get { return this.m_disqualificationPageURL; }
            set
            {
                if (this.m_disqualificationPageURL == value)
                    return;

                this.m_disqualificationPageURL = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PasswordLabel
        {
            get { return this.m_passwordLabel; }
            set
            {
                if (this.m_passwordLabel == value)
                    return;

                this.m_passwordLabel = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String SubmitButtonLabel
        {
            get { return this.m_submitButtonLabel; }
            set
            {
                if (this.m_submitButtonLabel == value)
                    return;

                this.m_submitButtonLabel = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PasswordRequiredMessage
        {
            get { return this.m_passwordRequiredMessage; }
            set
            {
                if (this.m_passwordRequiredMessage == value)
                    return;

                this.m_passwordRequiredMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String PasswordFailedMessage
        {
            get { return this.m_passwordFailedMessage; }
            set
            {
                if (this.m_passwordFailedMessage == value)
                    return;

                this.m_passwordFailedMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebInvitation
        {
            get { return this.m_webInvitation; }
            set
            {
                if (this.m_webInvitation == value)
                    return;

                this.m_webInvitation = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebNeverButton
        {
            get { return this.m_webNeverButton; }
            set
            {
                if (this.m_webNeverButton == value)
                    return;

                this.m_webNeverButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebLaterButton
        {
            get { return this.m_webLaterButton; }
            set
            {
                if (this.m_webLaterButton == value)
                    return;

                this.m_webLaterButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String WebNowButton
        {
            get { return this.m_webNowButton; }
            set
            {
                if (this.m_webNowButton == value)
                    return;

                this.m_webNowButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ClosedMessage
        {
            get { return this.m_closedMessage; }
            set
            {
                if (this.m_closedMessage == value)
                    return;

                this.m_closedMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public Int32 TotalMessages
        {
            get
            {
                return m_totalMessages;
            }
            internal set
            {
                m_totalMessages = value;
            }
        }
        /// <summary>
        ///
        /// </summary>
        public Int32 DraftMessages
        {
            get
            {
                return m_draftMessages;
            }
            internal set
            {
                m_draftMessages = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 RunningMessages
        {
            get
            {
                return m_runningMessages;
            }
            internal set
            {
                m_runningMessages = value;
            }
        }
        #endregion


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLCollector()
        {
            this.m_webWidth = 500;
            this.m_webHeight = 350;
            this.m_webPopupEvery = 1;
            this.m_webFontColor = "000000";
            this.m_webBackgroundColor = "ffffff";
            this.m_webLaterButton = "Later";
            this.m_webNeverButton = "Never";
            this.m_webNowButton = "Now";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLCollector(DbDataReader reader)
            : base(reader)
        {
            this.CollectorId = reader.GetInt32(0);
            this.Survey = reader.GetInt32(1);
            this.CollectorType = (CollectorType)reader.GetByte(2);
            this.Name = reader.GetString(3);
            this.AttributeFlags = reader.GetInt32(4);
            this.Status = (CollectorStatus)reader.GetByte(5);
            this.Responses = reader.GetInt32(6);
            if (!reader.IsDBNull(7)) this.WebLink = reader.GetString(7);
            this.EditResponseMode = (EditResponseModes)reader.GetByte(8);
            this.OnCompletionMode = (EndSurveyMode)reader.GetByte(9);
            if (!reader.IsDBNull(10)) this.StopCollectorDT = reader.GetDateTime(10);
            if (!reader.IsDBNull(11)) this.MaxResponses = reader.GetInt32(11);
            if (!reader.IsDBNull(12)) this.Password = reader.GetString(12);
            if (!reader.IsDBNull(13)) this.WebSurveyType = (WebSiteSurveyType)reader.GetByte(13);
            this.WebWidth = reader.GetInt16(14);
            this.WebHeight = reader.GetInt16(15);
            this.WebPopupEvery = reader.GetInt32(16);
            this.WebFontColor = reader.GetString(17);
            this.WebBackgroundColor = reader.GetString(18);
            if (!reader.IsDBNull(19)) this.CreditType = (CreditType)reader.GetByte(19);
            if (!reader.IsDBNull(20)) this.FirstChargeDt = reader.GetDateTime(20);
            if (!reader.IsDBNull(21)) this.LastChargeDt = reader.GetDateTime(21);
            if (!reader.IsDBNull(22)) this.Profile = reader.GetInt32(22);

            this.m_textsLanguage = reader.GetInt16(23);
            if (!reader.IsDBNull(24)) this.OnCompletionURL = reader.GetString(24);
            if (!reader.IsDBNull(25)) this.DisqualificationPageURL = reader.GetString(25);
            if (!reader.IsDBNull(26)) this.PasswordLabel = reader.GetString(26);
            if (!reader.IsDBNull(27)) this.SubmitButtonLabel = reader.GetString(27);
            if (!reader.IsDBNull(28)) this.PasswordRequiredMessage = reader.GetString(28);
            if (!reader.IsDBNull(29)) this.PasswordFailedMessage = reader.GetString(29);
            if (!reader.IsDBNull(30)) this.WebInvitation = reader.GetString(30);
            this.WebNeverButton = reader.GetString(31);
            this.WebLaterButton = reader.GetString(32);
            this.WebNowButton = reader.GetString(33);
            if (!reader.IsDBNull(34)) this.ClosedMessage = reader.GetString(34);

            this.PrimaryLanguage = reader.GetInt16(35);
            if (!reader.IsDBNull(36)) this.SupportedLanguagesIds = reader.GetString(36);

            this.TotalMessages = reader.GetInt32(37);
            this.DraftMessages = reader.GetInt32(38);
            this.RunningMessages = reader.GetInt32(39);

            this.EntityState = EntityState.Unchanged;
        }
        #endregion

        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.CollectorId.GetHashCode() ^
                this.Survey.GetHashCode() ^
                this.CollectorType.GetHashCode() ^
                this.Name.GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.Status.GetHashCode() ^
                this.Responses.GetHashCode() ^
                ((this.WebLink == null) ? string.Empty : this.WebLink.ToString()).GetHashCode() ^
                this.EditResponseMode.GetHashCode() ^
                this.OnCompletionMode.GetHashCode() ^
                ((this.StopCollectorDT == null) ? string.Empty : this.StopCollectorDT.ToString()).GetHashCode() ^
                ((this.MaxResponses == null) ? string.Empty : this.MaxResponses.ToString()).GetHashCode() ^
                ((this.Password == null) ? string.Empty : this.Password.ToString()).GetHashCode() ^
                ((this.WebSurveyType == null) ? string.Empty : this.WebSurveyType.ToString()).GetHashCode() ^
                this.WebWidth.GetHashCode() ^
                this.WebHeight.GetHashCode() ^
                this.WebPopupEvery.GetHashCode() ^
                this.WebFontColor.GetHashCode() ^
                this.WebBackgroundColor.GetHashCode() ^
                ((this.CreditType == null) ? string.Empty : this.CreditType.ToString()).GetHashCode() ^
                ((this.FirstChargeDt == null) ? string.Empty : this.FirstChargeDt.ToString()).GetHashCode() ^
                ((this.LastChargeDt == null) ? string.Empty : this.LastChargeDt.ToString()).GetHashCode() ^
                ((this.Profile == null) ? string.Empty : this.Profile.ToString()).GetHashCode() ^
                ((this.OnCompletionURL == null) ? string.Empty : this.OnCompletionURL.ToString()).GetHashCode() ^
                ((this.DisqualificationPageURL == null) ? string.Empty : this.DisqualificationPageURL.ToString()).GetHashCode() ^
                ((this.PasswordLabel == null) ? string.Empty : this.PasswordLabel.ToString()).GetHashCode() ^
                ((this.SubmitButtonLabel == null) ? string.Empty : this.SubmitButtonLabel.ToString()).GetHashCode() ^
                ((this.PasswordRequiredMessage == null) ? string.Empty : this.PasswordRequiredMessage.ToString()).GetHashCode() ^
                ((this.PasswordFailedMessage == null) ? string.Empty : this.PasswordFailedMessage.ToString()).GetHashCode() ^
                ((this.WebInvitation == null) ? string.Empty : this.WebInvitation.ToString()).GetHashCode() ^
                this.WebNeverButton.GetHashCode() ^
                this.WebLaterButton.GetHashCode() ^
                this.WebNowButton.GetHashCode() ^
                ((this.ClosedMessage == null) ? string.Empty : this.ClosedMessage.ToString()).GetHashCode();
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


            var other = (VLCollector)obj;

            //reference types
            if (!Object.Equals(Name, other.Name)) return false;
            if (!Object.Equals(WebLink, other.WebLink)) return false;
            if (!Object.Equals(Password, other.Password)) return false;
            if (!Object.Equals(WebFontColor, other.WebFontColor)) return false;
            if (!Object.Equals(WebBackgroundColor, other.WebBackgroundColor)) return false;
            if (!Object.Equals(OnCompletionURL, other.OnCompletionURL)) return false;
            if (!Object.Equals(DisqualificationPageURL, other.DisqualificationPageURL)) return false;
            if (!Object.Equals(PasswordLabel, other.PasswordLabel)) return false;
            if (!Object.Equals(SubmitButtonLabel, other.SubmitButtonLabel)) return false;
            if (!Object.Equals(PasswordRequiredMessage, other.PasswordRequiredMessage)) return false;
            if (!Object.Equals(PasswordFailedMessage, other.PasswordFailedMessage)) return false;
            if (!Object.Equals(WebInvitation, other.WebInvitation)) return false;
            if (!Object.Equals(WebNeverButton, other.WebNeverButton)) return false;
            if (!Object.Equals(WebLaterButton, other.WebLaterButton)) return false;
            if (!Object.Equals(WebNowButton, other.WebNowButton)) return false;
            if (!Object.Equals(ClosedMessage, other.ClosedMessage)) return false;
            //value types
            if (!CollectorId.Equals(other.CollectorId)) return false;
            if (!Survey.Equals(other.Survey)) return false;
            if (!CollectorType.Equals(other.CollectorType)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!Status.Equals(other.Status)) return false;
            if (!Responses.Equals(other.Responses)) return false;
            if (!EditResponseMode.Equals(other.EditResponseMode)) return false;
            if (!OnCompletionMode.Equals(other.OnCompletionMode)) return false;
            if (!StopCollectorDT.Equals(other.StopCollectorDT)) return false;
            if (!MaxResponses.Equals(other.MaxResponses)) return false;
            if (!WebSurveyType.Equals(other.WebSurveyType)) return false;
            if (!WebWidth.Equals(other.WebWidth)) return false;
            if (!WebHeight.Equals(other.WebHeight)) return false;
            if (!WebPopupEvery.Equals(other.WebPopupEvery)) return false;
            if (!CreditType.Equals(other.CreditType)) return false;
            if (!FirstChargeDt.Equals(other.FirstChargeDt)) return false;
            if (!LastChargeDt.Equals(other.LastChargeDt)) return false;
            if (!Profile.Equals(other.Profile)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLCollector o1, VLCollector o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLCollector o1, VLCollector o2)
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
            Utility.CheckParameter(ref m_webLink, false, false, false, 128, "WebLink");
            Utility.CheckParameter(ref m_password, false, false, false, 128, "Password");
            Utility.CheckParameter(ref m_webFontColor, true, true, false, 6, "WebFontColor");
            Utility.CheckParameter(ref m_webBackgroundColor, true, true, false, 6, "WebBackgroundColor");
            Utility.CheckParameter(ref m_onCompletionURL, false, false, false, 256, "OnCompletionURL");
            Utility.CheckParameter(ref m_disqualificationPageURL, false, false, false, 256, "DisqualificationPageURL");
            Utility.CheckParameter(ref m_passwordLabel, false, false, false, 128, "PasswordLabel");
            Utility.CheckParameter(ref m_submitButtonLabel, false, false, false, 128, "SubmitButtonLabel");
            Utility.CheckParameter(ref m_passwordRequiredMessage, false, false, false, 1024, "PasswordRequiredMessage");
            Utility.CheckParameter(ref m_passwordFailedMessage, false, false, false, 1024, "PasswordFailedMessage");
            Utility.CheckParameter(ref m_webInvitation, false, false, false, 1024, "WebInvitation");
            Utility.CheckParameter(ref m_webNeverButton, true, true, false, 50, "WebNeverButton");
            Utility.CheckParameter(ref m_webLaterButton, true, true, false, 50, "WebLaterButton");
            Utility.CheckParameter(ref m_webNowButton, true, true, false, 50, "WebNowButton");
            Utility.CheckParameter(ref m_closedMessage, false, false, false, 1024, "ClosedMessage");
        }
    }
}
