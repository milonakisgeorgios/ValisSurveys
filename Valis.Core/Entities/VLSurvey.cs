using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    [DataContract, DataObject]
    public sealed class VLSurvey : VLObject
    {

        [Flags]
        internal enum SurveyAttributes : int
        {
            None                            = 0,
            IsBuiltIn                       = 1,           // 1 << 0
            ShowLanguageSelector            = 2,           // 1 << 1
            ShowWelcomePage                 = 4,           // 1 << 2
            ShowGoodbyePage                 = 8,           // 1 << 3
            ShowCustomFooter                = 16,          // 1 << 4
            UsePageNumbering                = 32,          // 1 << 5 
            UseQuestionNumbering            = 64,          // 1 << 6
            UseProgressBar                  = 128,         // 1 << 7
            ShowSurveyTitle                 = 256,         // 1 << 8
            ShowPageTitles                  = 512,         // 1 << 9
            SaveFilesInDatabase             = 1024,        // 1 << 10
            HasCollectors                   = 2048,        // 1 << 11
            HasResponses                    = 4096,        // 1 << 12
            IsFromCopy                      = 8192,        // 1 << 13
            IsFromTemplate                  = 16384,        // 1 << 14
            AllResponsesXlsxExportIsValid   = 32768,        // 1 << 15
        }

        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_client;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_surveyId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_folder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_publicId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_title;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_theme;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_logo;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_pageSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_questionSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_ticketSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_primaryLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_supportedLanguagesIds;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionNumberingType m_questionNumberingType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ProgressBarPosition m_progressBarPosition;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RequiredHighlightType m_requiredHighlightType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_designVersion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_recordedResponses;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_CustomId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_sourceSurvey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_templateSurvey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        EndSurveyMode m_onCompletionMode;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DisqualificationMode m_onDisqualificationMode;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_export1Name;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_export1Path;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        DateTime? m_export1CreationDt;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_totalPages;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        VLSupportedLanguagesColection m_supportedLanguages = null;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_showTitle;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_headerHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_welcomehtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_goodbyeHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_footerHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_disqualificationHtml;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_disqualificationUrl;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_onCompletionUrl;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_startButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_previousButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_nextButton;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_doneButton;
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
        /// The client who owns this survey
        /// </summary>
        public Int32 Client
        {
            get { return m_client; }
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
        /// This is the Survey's ID
        /// </summary>
        public Int32 SurveyId
        {
            get { return m_surveyId; }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? Folder
        {
            get { return m_folder; }
            internal set
            {
                if (this.m_folder == value)
                    return;

                this.m_folder = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// This is the Survey's PublicID
        /// <para>Πρέπει να είναι μοναδικό σε ολόκληρο το σύστημα. 
        /// Κατα την δημιουργία ενός νέου Survey, το σύστημα αυτόματα δημιουργεί ένα νέο GUID για το PublicId</para>
        /// </summary>
        public String PublicId
        {
            get { return m_publicId; }
            internal set
            {
                if (this.m_publicId == value)
                    return;

                this.m_publicId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// This is the Title of the survey
        /// </summary>
        public String Title
        {
            get { return m_title; }
            set
            {
                if (this.m_title == value)
                    return;

                this.m_title = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32 Theme
        {
            get { return m_theme; }
            set
            {
                if (this.m_theme == value)
                    return;

                this.m_theme = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int32? Logo
        {
            get { return m_logo; }
            internal set
            {
                if (this.m_logo == value)
                    return;

                this.m_logo = value;
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
        /// <summary>
        /// 
        /// </summary>
        internal Int16 PageSequence
        {
            get { return m_pageSequence; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int16 QuestionSequence
        {
            get { return m_questionSequence; }
        }
        /// <summary>
        /// 
        /// </summary>
        internal Int32 TicketSequence
        {
            get { return m_ticketSequence; }
        }
        /// <summary>
        /// This is the Language in which first created the survey.
        /// <para>If the survey does not support languages, then the PrimaryLanguage has the value of the 'Invariant Language'.</para>
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
        /// This is the internal backing field for the supported languages 
        /// </summary>
        internal String SupportedLanguagesIds
        {
            get { return m_supportedLanguagesIds; }
            set
            {
                if (this.m_supportedLanguagesIds == value)
                    return;

                this.m_supportedLanguagesIds = value;
                this.m_supportedLanguages = null;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// This is an enumeration of the Languages which the survey supports
        /// </summary>
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
        /// 
        /// </summary>
        public QuestionNumberingType QuestionNumberingType
        {
            get { return m_questionNumberingType; }
            set
            {
                if (this.m_questionNumberingType == value)
                    return;

                this.m_questionNumberingType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ProgressBarPosition ProgressBarPosition
        {
            get { return m_progressBarPosition; }
            set
            {
                if (this.m_progressBarPosition == value)
                    return;

                this.m_progressBarPosition = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RequiredHighlightType RequiredHighlightType
        {
            get { return m_requiredHighlightType; }
            set
            {
                if (this.m_requiredHighlightType == value)
                    return;

                this.m_requiredHighlightType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Δηλώνει την τρέχουσα έκδοση του survey, και αφορά μόνο τις ερωτήσεις και τα options αυτών.
        /// <para>Το σύστημα κάνει tracking τις αλλαγές των ερωτήσεων/options του survey αυτόματα. Μας ενδιαφέρουν μόνο αυτές οι αλλαγές,
        /// απο αυτές (ερωτήσεις/options) εξαρτώνται και οι απαντήσεις μας.</para>
        /// </summary>
        public Int32 DesignVersion
        {
            get { return m_designVersion; }
            set
            {
                if (this.m_designVersion == value)
                    return;

                this.m_designVersion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Το σύνολο των Responses που έχουν καταχωρηθεί στο σύστημα για το συγκεκριμένο Survey μέσω οποιουδήποτε collector.
        /// <para>O μετρητής αυτός μετράει όλα τα responses είτε έχουν πληρωθεί είτε όχι.</para>
        /// </summary>
        public Int32 RecordedResponses
        {
            get { return m_recordedResponses; }
            set
            {
                if (this.m_recordedResponses == value)
                    return;

                this.m_recordedResponses = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Ενας κωδικός (τον οποίο τον δίνει ο Πελάτης) και συνοδεύει το export του survey
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
        /// Αυτό το είναι το SurveyId του ερωτηματολογίου απο το οποίο έγινε αντιγραφή το παρών
        /// </summary>
        public System.Int32? SourceSurvey
        {
            get { return this.m_sourceSurvey; }
            set
            {
                if (this.m_sourceSurvey == value)
                    return;

                this.m_sourceSurvey = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Αυτό είναι το TemplateId του προτυπου ερωτηματολογίου (template) με βάση το οποίο δημιουργε
        /// </summary>
        public System.Int32? TemplateSurvey
        {
            get { return this.m_templateSurvey; }
            set
            {
                if (this.m_templateSurvey == value)
                    return;

                this.m_templateSurvey = value;
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
        /// 
        /// </summary>
        public DisqualificationMode OnDisqualificationMode
        {
            get { return this.m_onDisqualificationMode; }
            set
            {
                if (this.m_onDisqualificationMode == value)
                    return;

                this.m_onDisqualificationMode = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public String AllResponsesXlsxExportName
        {
            get { return m_export1Name; }
            set { m_export1Name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public String AllResponsesXlsxExportPath
        {
            get { return m_export1Path; }
            set { m_export1Path = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? AllResponsesXlsxExportCreationDt
        {
            get { return m_export1CreationDt; }
            set { m_export1CreationDt = value; }
        }

        /// <summary>
        /// Το σύνολο των σελίδων του συγκεκριμένου survey, την ώρα που διαβάστηκε απο το σύστημα
        /// </summary>
        public System.Int32 TotalPages
        {
            get { return this.m_totalPages; }
        }

        public System.Boolean IsBuiltIn
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.IsBuiltIn)) == ((int)SurveyAttributes.IsBuiltIn); }
            internal set
            {
                if (this.IsBuiltIn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.IsBuiltIn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.IsBuiltIn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowLanguageSelector
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowLanguageSelector)) == ((int)SurveyAttributes.ShowLanguageSelector); }
            internal set
            {
                if (this.ShowLanguageSelector == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowLanguageSelector;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowLanguageSelector;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowWelcomePage
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowWelcomePage)) == ((int)SurveyAttributes.ShowWelcomePage); }
            internal set
            {
                if (this.ShowWelcomePage == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowWelcomePage;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowWelcomePage;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowGoodbyePage
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowGoodbyePage)) == ((int)SurveyAttributes.ShowGoodbyePage); }
            internal set
            {
                if (this.ShowGoodbyePage == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowGoodbyePage;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowGoodbyePage;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowCustomFooter
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowCustomFooter)) == ((int)SurveyAttributes.ShowCustomFooter); }
            internal set
            {
                if (this.ShowCustomFooter == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowCustomFooter;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowCustomFooter;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean UsePageNumbering
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.UsePageNumbering)) == ((int)SurveyAttributes.UsePageNumbering); }
            internal set
            {
                if (this.UsePageNumbering == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.UsePageNumbering;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.UsePageNumbering;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean UseQuestionNumbering
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.UseQuestionNumbering)) == ((int)SurveyAttributes.UseQuestionNumbering); }
            internal set
            {
                if (this.UseQuestionNumbering == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.UseQuestionNumbering;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.UseQuestionNumbering;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean UseProgressBar
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.UseProgressBar)) == ((int)SurveyAttributes.UseProgressBar); }
            internal set
            {
                if (this.UseProgressBar == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.UseProgressBar;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.UseProgressBar;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowSurveyTitle
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowSurveyTitle)) == ((int)SurveyAttributes.ShowSurveyTitle); }
            internal set
            {
                if (this.ShowSurveyTitle == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowSurveyTitle;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowSurveyTitle;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean ShowPageTitles
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.ShowPageTitles)) == ((int)SurveyAttributes.ShowPageTitles); }
            internal set
            {
                if (this.ShowPageTitles == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.ShowPageTitles;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.ShowPageTitles;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        public System.Boolean SaveFilesInDatabase
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.SaveFilesInDatabase)) == ((int)SurveyAttributes.SaveFilesInDatabase); }
            internal set
            {
                if (this.SaveFilesInDatabase == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.SaveFilesInDatabase;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.SaveFilesInDatabase;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// Μας λέει εάν το Survey διαθέτει collectors
        /// </summary>
        public System.Boolean HasCollectors
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.HasCollectors)) == ((int)SurveyAttributes.HasCollectors); }
            internal set
            {
                if (this.HasCollectors == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.HasCollectors;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.HasCollectors;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει εάν το survey διαθέτει Responses
        /// </summary>
        public System.Boolean HasResponses
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.HasResponses)) == ((int)SurveyAttributes.HasResponses); }
            internal set
            {
                if (this.HasResponses == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.HasResponses;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.HasResponses;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει εάν το παρών ερωτηματολόγιο είναι αντίγραφο κάποιου άλλου προυπάρχοντως ερωτηματολογίου
        /// </summary>
        public System.Boolean IsFromCopy
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.IsFromCopy)) == ((int)SurveyAttributes.IsFromCopy); }
            internal set
            {
                if (this.IsFromCopy == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.IsFromCopy;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.IsFromCopy;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει εάν το παρών ερωτηματολόγιο δημιουργήθηκε με βάση κάποιο template
        /// </summary>
        public System.Boolean IsFromTemplate
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.IsFromTemplate)) == ((int)SurveyAttributes.IsFromTemplate); }
            internal set
            {
                if (this.IsFromTemplate == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.IsFromTemplate;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.IsFromTemplate;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean AllResponsesXlsxExportIsValid
        {
            get { return (this.m_attributeFlags & ((int)SurveyAttributes.AllResponsesXlsxExportIsValid)) == ((int)SurveyAttributes.AllResponsesXlsxExportIsValid); }
            internal set
            {
                if (this.AllResponsesXlsxExportIsValid == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)SurveyAttributes.AllResponsesXlsxExportIsValid;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)SurveyAttributes.AllResponsesXlsxExportIsValid;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// The language of the translatable fields..
        /// </summary>
        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        /// <summary>
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String ShowTitle
        {
            get { return m_showTitle; }
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
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String HeaderHtml
        {
            get { return m_headerHtml; }
            set
            {
                if (this.m_headerHtml == value)
                    return;

                this.m_headerHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String WelcomeHtml
        {
            get { return m_welcomehtml; }
            set
            {
                if (this.m_welcomehtml == value)
                    return;

                this.m_welcomehtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String GoodbyeHtml
        {
            get { return m_goodbyeHtml; }
            set
            {
                if (this.m_goodbyeHtml == value)
                    return;

                this.m_goodbyeHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String FooterHtml
        {
            get { return m_footerHtml; }
            set
            {
                if (this.m_footerHtml == value)
                    return;

                this.m_footerHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String DisqualificationHtml
        {
            get { return m_disqualificationHtml; }
            set
            {
                if (this.m_disqualificationHtml == value)
                    return;

                this.m_disqualificationHtml = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String DisqualificationUrl
        {
            get { return m_disqualificationUrl; }
            set
            {
                if (this.m_disqualificationUrl == value)
                    return;

                this.m_disqualificationUrl = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        public System.String OnCompletionUrl
        {
            get { return m_onCompletionUrl; }
            set
            {
                if (this.m_onCompletionUrl == value)
                    return;

                this.m_onCompletionUrl = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summar y>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String StartButton
        {
            get { return m_startButton; }
            set
            {
                if (this.m_startButton == value)
                    return;

                this.m_startButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String PreviousButton
        {
            get { return m_previousButton; }
            set
            {
                if (this.m_previousButton == value)
                    return;

                this.m_previousButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String NextButton
        {
            get { return m_nextButton; }
            set
            {
                if (this.m_nextButton == value)
                    return;

                this.m_nextButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// <para>Translatable Property</para>
        /// </summary>
        public System.String DoneButton
        {
            get { return m_doneButton; }
            set
            {
                if (this.m_doneButton == value)
                    return;

                this.m_doneButton = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        #endregion

        /// <summary>
        /// Μας λέει εάν υποστηρίζει την συγκεκριμένη γλώσσα
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public bool SupportsLanguage(Int16 languageId)
        {
            foreach(var lang in SupportedLanguages)
            {
                if (lang.LanguageId == languageId)
                    return true;
            }

            return false;
        }

        #region class constructors
        public VLSurvey()
        {
            m_client = default(Int32);
            m_surveyId = default(Int32);
            m_folder = null;
            m_publicId = Guid.NewGuid().ToString("N").ToUpperInvariant();
            m_title = default(string);
            m_theme = BuiltinThemes.Default.ThemeId;
            m_logo = null;
            m_attributeFlags = default(Int32);
            m_pageSequence = default(Int16);
            m_questionSequence = default(Int16);
            m_ticketSequence = default(Int32);
            m_primaryLanguage = default(Int16);
            m_supportedLanguagesIds = default(string);
            m_questionNumberingType = default(QuestionNumberingType);
            m_progressBarPosition = default(ProgressBarPosition);
            m_requiredHighlightType = default(RequiredHighlightType);
            m_designVersion = default(Int32);
            m_recordedResponses = default(Int32);
            m_CustomId = default(string);

            m_textsLanguage = default(Int16);
            m_showTitle = default(string);
            m_headerHtml = default(string);
            m_welcomehtml = default(string);
            m_goodbyeHtml = default(string);
            m_footerHtml = default(string);
            m_disqualificationHtml = default(string);
            m_disqualificationUrl = default(string);
            m_onCompletionUrl = default(string);
            m_startButton = "Start";
            m_previousButton = "Previous";
            m_nextButton = "Next";
            m_doneButton = "Submit";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSurvey(DbDataReader reader)
            : base(reader)
        {
            this.m_client = reader.GetInt32(0);
            this.m_surveyId = reader.GetInt32(1);
            if (!reader.IsDBNull(2)) this.Folder = reader.GetInt16(2);
            this.PublicId = reader.GetString(3);
            this.Title = reader.GetString(4);
            this.Theme = reader.GetInt32(5);
            if (!reader.IsDBNull(6)) this.Logo = reader.GetInt32(6);
            this.AttributeFlags = reader.GetInt32(7);
            this.m_pageSequence = reader.GetInt16(8);
            this.m_questionSequence = reader.GetInt16(9);
            this.m_ticketSequence = reader.GetInt32(10);
            this.PrimaryLanguage = reader.GetInt16(11);
            if (!reader.IsDBNull(12)) this.SupportedLanguagesIds = reader.GetString(12);

            this.m_questionNumberingType = (QuestionNumberingType)reader.GetByte(13);
            this.m_progressBarPosition = (ProgressBarPosition)reader.GetByte(14);
            this.m_requiredHighlightType = (RequiredHighlightType)reader.GetByte(15);
            this.m_designVersion = reader.GetInt32(16);
            this.m_recordedResponses = reader.GetInt32(17);
            if (!reader.IsDBNull(18)) this.m_CustomId = reader.GetString(18);
            if (!reader.IsDBNull(19)) this.SourceSurvey = reader.GetInt32(19);
            if (!reader.IsDBNull(20)) this.TemplateSurvey = reader.GetInt32(20);

            this.m_onCompletionMode = (EndSurveyMode)reader.GetByte(21);
            this.m_onDisqualificationMode = (DisqualificationMode)reader.GetByte(22);

            if (!reader.IsDBNull(23)) this.m_export1Name = reader.GetString(23);
            if (!reader.IsDBNull(24)) this.m_export1Path = reader.GetString(24);
            if (!reader.IsDBNull(25)) this.m_export1CreationDt = reader.GetDateTime(25);

            this.m_totalPages = reader.GetInt32(26);

            this.m_textsLanguage = reader.GetInt16(27);
            if (!reader.IsDBNull(28)) this.ShowTitle = reader.GetString(28);
            if (!reader.IsDBNull(29)) this.HeaderHtml = reader.GetString(29);
            if (!reader.IsDBNull(30)) this.WelcomeHtml = reader.GetString(30);
            if (!reader.IsDBNull(31)) this.GoodbyeHtml = reader.GetString(31);
            if (!reader.IsDBNull(32)) this.FooterHtml = reader.GetString(32);

            if (!reader.IsDBNull(33)) this.DisqualificationHtml = reader.GetString(33);
            if (!reader.IsDBNull(34)) this.DisqualificationUrl = reader.GetString(34);
            if (!reader.IsDBNull(35)) this.OnCompletionUrl = reader.GetString(35);

            if (!reader.IsDBNull(36)) this.StartButton = reader.GetString(36);
            if (!reader.IsDBNull(37)) this.PreviousButton = reader.GetString(37);
            if (!reader.IsDBNull(38)) this.NextButton = reader.GetString(38);
            if (!reader.IsDBNull(39)) this.DoneButton = reader.GetString(39);


            this.EntityState = EntityState.Unchanged;
        }
        /// <summary>
        /// Αντιγράφει το source survey, αλλά δίνει νέο PublicId, μηδενίζει τα pageSequence, questionSequence και ticketSequence.
        /// <para>To νεοδημιουργημένο survey, υποστηρίζει μόνο μία γλώσσα, την TextsLanguage του source survey.</para>
        /// </summary>
        /// <param name="source"></param>
        internal VLSurvey(VLSurvey source)
        {
            this.m_client = source.m_client;
            this.m_surveyId = default(Int32);
            this.m_folder = default(Int16?);
            this.m_publicId = Guid.NewGuid().ToString("N").ToUpperInvariant();
            this.m_title = source.m_title;
            this.m_theme = source.m_theme;
            this.m_logo = source.m_logo;
            this.m_attributeFlags = default(Int32);
            this.m_pageSequence = default(Int16);
            this.m_questionSequence = default(Int16);
            this.m_ticketSequence = default(Int32);
            this.m_primaryLanguage = source.TextsLanguage;
            this.m_supportedLanguagesIds = string.Format("{0},", source.TextsLanguage);

            this.m_questionNumberingType = source.m_questionNumberingType;
            this.m_progressBarPosition = source.m_progressBarPosition;
            this.m_requiredHighlightType = source.m_requiredHighlightType;
            this.m_designVersion = default(Int32);
            this.m_recordedResponses = default(Int32);
            this.m_CustomId = default(string);

            this.IsBuiltIn = false;
            this.ShowLanguageSelector = source.ShowLanguageSelector;
            this.ShowWelcomePage = source.ShowWelcomePage;
            this.ShowGoodbyePage = source.ShowGoodbyePage;
            this.ShowCustomFooter = source.ShowCustomFooter;
            this.UsePageNumbering = source.UsePageNumbering;
            this.UseQuestionNumbering = source.UseQuestionNumbering;
            this.UseProgressBar = source.UseProgressBar;
            this.ShowSurveyTitle = source.ShowSurveyTitle;
            this.ShowPageTitles = source.ShowPageTitles;
            this.SaveFilesInDatabase = false;
            this.HasCollectors = false;
            this.HasResponses = false;

            this.m_textsLanguage = source.TextsLanguage;
            this.m_showTitle = source.m_showTitle;
            this.m_headerHtml = source.m_headerHtml;
            this.m_welcomehtml = source.m_welcomehtml;
            this.m_goodbyeHtml = source.m_goodbyeHtml;
            this.m_footerHtml = source.m_footerHtml;
            this.m_disqualificationHtml = source.m_disqualificationHtml;
            this.m_disqualificationUrl = source.m_disqualificationUrl;
            this.m_onCompletionUrl = source.m_onCompletionUrl;

            this.m_startButton = source.m_startButton;
            this.m_previousButton = source.m_previousButton;
            this.m_nextButton = source.m_nextButton;
            this.m_doneButton = source.m_doneButton;
        }
        #endregion


        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {

            return this.Client.GetHashCode() ^
                this.SurveyId.GetHashCode() ^
                ((this.Folder == null) ? string.Empty : this.Folder.ToString()).GetHashCode() ^
                this.PublicId.GetHashCode() ^
                this.Title.GetHashCode() ^
                this.Theme.GetHashCode() ^
                ((this.Logo == null) ? string.Empty : this.Logo.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.PageSequence.GetHashCode() ^
                this.QuestionSequence.GetHashCode() ^
                this.TicketSequence.GetHashCode() ^
                this.PrimaryLanguage.GetHashCode() ^
                this.SupportedLanguagesIds.GetHashCode() ^
                this.QuestionNumberingType.GetHashCode() ^
                this.ProgressBarPosition.GetHashCode() ^
                this.RequiredHighlightType.GetHashCode() ^
                this.DesignVersion.GetHashCode() ^
                this.RecordedResponses.GetHashCode() ^
                ((this.CustomId == null) ? string.Empty : this.CustomId.ToString()).GetHashCode() ^
                ((this.SourceSurvey == null) ? string.Empty : this.SourceSurvey.ToString()).GetHashCode() ^
                ((this.TemplateSurvey == null) ? string.Empty : this.TemplateSurvey.ToString()).GetHashCode() ^
                this.OnCompletionMode.GetHashCode() ^
                this.OnDisqualificationMode.GetHashCode() ^
                ((this.m_export1Name == null) ? string.Empty : this.m_export1Name.ToString()).GetHashCode() ^
                ((this.m_export1Path == null) ? string.Empty : this.m_export1Path.ToString()).GetHashCode() ^
                ((this.m_export1CreationDt == null) ? string.Empty : this.m_export1CreationDt.ToString()).GetHashCode() ^
                this.TextsLanguage.GetHashCode() ^
                this.ShowTitle.GetHashCode() ^
                ((this.HeaderHtml == null) ? string.Empty : this.HeaderHtml.ToString()).GetHashCode() ^
                ((this.WelcomeHtml == null) ? string.Empty : this.WelcomeHtml.ToString()).GetHashCode() ^
                ((this.GoodbyeHtml == null) ? string.Empty : this.GoodbyeHtml.ToString()).GetHashCode() ^
                ((this.FooterHtml == null) ? string.Empty : this.FooterHtml.ToString()).GetHashCode() ^
                ((this.DisqualificationHtml == null) ? string.Empty : this.DisqualificationHtml.ToString()).GetHashCode() ^
                ((this.DisqualificationUrl == null) ? string.Empty : this.DisqualificationUrl.ToString()).GetHashCode() ^
                ((this.OnCompletionUrl == null) ? string.Empty : this.OnCompletionUrl.ToString()).GetHashCode() ^
                ((this.StartButton == null) ? string.Empty : this.StartButton.ToString()).GetHashCode() ^
                ((this.PreviousButton == null) ? string.Empty : this.PreviousButton.ToString()).GetHashCode() ^
                ((this.NextButton == null) ? string.Empty : this.NextButton.ToString()).GetHashCode() ^
                ((this.DoneButton == null) ? string.Empty : this.DoneButton.ToString()).GetHashCode();
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


            var other = (VLSurvey)obj;

            //reference types
            if (!Object.Equals(m_publicId, other.m_publicId)) return false;
            if (!Object.Equals(m_title, other.m_title)) return false;
            if (!Object.Equals(m_supportedLanguagesIds, other.m_supportedLanguagesIds)) return false;
            if (!Object.Equals(m_CustomId, other.m_CustomId)) return false;
            if (!Object.Equals(m_export1Name, other.m_export1Name)) return false;
            if (!Object.Equals(m_export1Path, other.m_export1Path)) return false;
            if (!Object.Equals(ShowTitle, other.ShowTitle)) return false;
            if (!Object.Equals(HeaderHtml, other.HeaderHtml)) return false;
            if (!Object.Equals(WelcomeHtml, other.WelcomeHtml)) return false;
            if (!Object.Equals(GoodbyeHtml, other.GoodbyeHtml)) return false;
            if (!Object.Equals(FooterHtml, other.FooterHtml)) return false;
            if (!Object.Equals(DisqualificationHtml, other.DisqualificationHtml)) return false;
            if (!Object.Equals(DisqualificationUrl, other.DisqualificationUrl)) return false;
            if (!Object.Equals(OnCompletionUrl, other.OnCompletionUrl)) return false;
            if (!Object.Equals(StartButton, other.StartButton)) return false;
            if (!Object.Equals(PreviousButton, other.PreviousButton)) return false;
            if (!Object.Equals(NextButton, other.NextButton)) return false;
            if (!Object.Equals(DoneButton, other.DoneButton)) return false;
            //value types
            if (!m_client.Equals(other.m_client)) return false;
            if (!m_surveyId.Equals(other.m_surveyId)) return false;
            if (!m_folder.Equals(other.m_folder)) return false;
            if (!m_theme.Equals(other.m_theme)) return false;
            if (!m_logo.Equals(other.m_logo)) return false;
            if (!m_attributeFlags.Equals(other.m_attributeFlags)) return false;
            if (!m_pageSequence.Equals(other.m_pageSequence)) return false;
            if (!m_questionSequence.Equals(other.m_questionSequence)) return false;
            if (!m_ticketSequence.Equals(other.m_ticketSequence)) return false;
            if (!m_primaryLanguage.Equals(other.m_primaryLanguage)) return false;
            if (!m_questionNumberingType.Equals(other.m_questionNumberingType)) return false;
            if (!m_progressBarPosition.Equals(other.m_progressBarPosition)) return false;
            if (!m_requiredHighlightType.Equals(other.m_requiredHighlightType)) return false;
            if (!m_designVersion.Equals(other.m_designVersion)) return false;
            if (!m_recordedResponses.Equals(other.m_recordedResponses)) return false;
            if (!m_sourceSurvey.Equals(other.m_sourceSurvey)) return false;
            if (!m_templateSurvey.Equals(other.m_templateSurvey)) return false;
            if (!m_onCompletionMode.Equals(other.m_onCompletionMode)) return false;
            if (!m_onDisqualificationMode.Equals(other.m_onDisqualificationMode)) return false;
            if (!m_export1CreationDt.Equals(other.m_export1CreationDt)) return false;

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLSurvey o1, VLSurvey o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLSurvey o1, VLSurvey o2)
        {
            return !(o1 == o2);
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_publicId, true, true, false, 64, "PublicId");
            Utility.CheckParameter(ref m_title, true, true, false, 256, "Title");
            Utility.CheckParameter(ref m_showTitle, false, false, false, 256, "ShowTitle");
            Utility.CheckParameter(ref m_headerHtml, false, false, false, -1, "HeaderHtml");
            Utility.CheckParameter(ref m_welcomehtml, false, false, false, -1, "WelcomeHtml");
            Utility.CheckParameter(ref m_goodbyeHtml, false, false, false, -1, "GoodbyeHtml");
            Utility.CheckParameter(ref m_footerHtml, false, false, false, -1, "FooterHtml");
            Utility.CheckParameter(ref m_disqualificationHtml, false, false, false, -1, "DisqualificationHtml");
            Utility.CheckParameter(ref m_disqualificationUrl, false, false, false, 1024, "DisqualificationUrl");
            Utility.CheckParameter(ref m_onCompletionUrl, false, false, false, 1024, "OnCompletionUrl");
            Utility.CheckParameter(ref m_startButton, true, true, false, 50, "StartButton");
            Utility.CheckParameter(ref m_previousButton, true, true, false, 50, "PreviousButton");
            Utility.CheckParameter(ref m_nextButton, true, true, false, 50, "NextButton");
            Utility.CheckParameter(ref m_doneButton, true, true, false, 50, "DoneButton");
        }

        public override string ToString()
        {
            return string.Format("{0}:{1} -> {2}", this.SurveyId, BuiltinLanguages.GetLanguageById(this.TextsLanguage).Name, this.Title);
        }
    }
}
