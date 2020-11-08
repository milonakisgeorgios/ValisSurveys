using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Αντιπροσωπεύει μία ερώτηση που ανήκει σε ένα survey
    /// </summary>
    [DataContract, DataObject]
    public class VLSurveyQuestion : VLObject
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_survey;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_questionId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_page;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_masterQuestion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_displayOrder;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionType m_questionType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionType? m_customType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Boolean m_isRequired;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RequiredMode? m_requiredBehavior;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_requiredMinLimit;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16? m_requiredMaxLimit;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_attributeFlags;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        ValidationMode m_validationBehavior;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_validationField1;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_validationField2;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_validationField3;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_regularExpression;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        RandomizationMode? m_randomBehavior;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        OtherFieldType? m_otherFieldType;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_otherFieldRows;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte? m_otherFieldChars;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_optionsSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Byte m_columnsSequence;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_rangeStart;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_rangeEnd;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32? m_libraryQuestion;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_CustomId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_textsLanguage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_questionText;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_description;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_helpText;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_frontLabelText;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_afterLabelText;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_insideText;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_requiredMessage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_validationMessage;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        String m_otherFieldLabel;
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

        private string m_htmlQuestionId;
        public string HtmlQuestionId
        {
            get
            {
                if(string.IsNullOrEmpty(m_htmlQuestionId))
                {
                    m_htmlQuestionId = string.Format("QstnID_{0}_", this.QuestionId);
                }
                return m_htmlQuestionId;
            }
        }


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
        public System.Int16 QuestionId
        {
            get { return this.m_questionId; }
            internal set
            {
                if (this.m_questionId == value)
                    return;

                this.m_questionId = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16 Page
        {
            get { return this.m_page; }
            internal set
            {
                if (this.m_page == value)
                    return;

                this.m_page = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int16? MasterQuestion
        {
            get { return this.m_masterQuestion; }
            set
            {
                if (this.m_masterQuestion == value)
                    return;

                this.m_masterQuestion = value;
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
            internal set
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
        public QuestionType QuestionType
        {
            get { return this.m_questionType; }
            internal set
            {
                if (this.m_questionType == value)
                    return;

                this.m_questionType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public QuestionType? CustomType
        {
            get { return this.m_customType; }
            internal set
            {
                if (this.m_customType == value)
                    return;

                this.m_customType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Boolean IsRequired
        {
            get { return this.m_isRequired; }
            set
            {
                if (this.m_isRequired == value)
                    return;

                this.m_isRequired = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RequiredMode? RequiredBehavior
        {
            get { return this.m_requiredBehavior; }
            set
            {
                if (this.m_requiredBehavior == value)
                    return;

                this.m_requiredBehavior = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? RequiredMinLimit
        {
            get { return this.m_requiredMinLimit; }
            set
            {
                if (this.m_requiredMinLimit == value)
                    return;

                this.m_requiredMinLimit = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Int16? RequiredMaxLimit
        {
            get { return this.m_requiredMaxLimit; }
            set
            {
                if (this.m_requiredMaxLimit == value)
                    return;

                this.m_requiredMaxLimit = value;
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
        /// Add "Other" or a comment field (Optional) 
        /// <para>Allow respondents to add a comment to clarify their answer</para>
        /// </summary>
        public System.Boolean OptionalInputBox
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.OptionalInputBox)) == ((int)QuestionAttributes.OptionalInputBox); }
            internal set
            {
                if (this.OptionalInputBox == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.OptionalInputBox;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.OptionalInputBox;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Sort, randomize or flip rows (Optional) 
        /// <para>Specify a fixed or varying order of your answer choices.</para>
        /// </summary>
        public System.Boolean RandomizeOptionsSequence
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.RandomizeOptionsSequence)) == ((int)QuestionAttributes.RandomizeOptionsSequence); }
            internal set
            {
                if (this.RandomizeOptionsSequence == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.RandomizeOptionsSequence;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.RandomizeOptionsSequence;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Do not randomize the last option 
        /// <para>Prevent the last option, such as "None of the above" from being randomized</para>
        /// </summary>
        public System.Boolean DoNotRandomizeLastOption
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.DoNotRandomizeLastOption)) == ((int)QuestionAttributes.DoNotRandomizeLastOption); }
            internal set
            {
                if (this.DoNotRandomizeLastOption == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.DoNotRandomizeLastOption;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.DoNotRandomizeLastOption;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean RandomizeColumnSequence
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.RandomizeColumnSequence)) == ((int)QuestionAttributes.RandomizeColumnSequence); }
            internal set
            {
                if (this.RandomizeColumnSequence == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.RandomizeColumnSequence;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.RandomizeColumnSequence;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Allow only one response per column (Optional) 
        /// <para>This is also called "Forced Ranking"</para>
        /// </summary>
        public System.Boolean OneResponsePerColumn
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.OneResponsePerColumn)) == ((int)QuestionAttributes.OneResponsePerColumn); }
            internal set
            {
                if (this.OneResponsePerColumn == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.OneResponsePerColumn;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.OneResponsePerColumn;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Boolean UseDateTimeControls
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.UseDateTimeControls)) == ((int)QuestionAttributes.UseDateTimeControls); }
            internal set
            {
                if (this.UseDateTimeControls == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.UseDateTimeControls;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.UseDateTimeControls;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Boolean AddResetLink
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.AddResetLink)) == ((int)QuestionAttributes.AddResetLink); }
            internal set
            {
                if (this.AddResetLink == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.AddResetLink;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.AddResetLink;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Μας λέει εάν η συγκεκριμένη ερώτηση έχει σεταρισμένο Skip Logic
        /// </summary>
        public System.Boolean HasSkipLogic
        {
            get { return (this.m_attributeFlags & ((int)QuestionAttributes.HasSkipLogic)) == ((int)QuestionAttributes.HasSkipLogic); }
            internal set
            {
                if (this.HasSkipLogic == value)
                    return;

                if (value)
                    this.m_attributeFlags = this.m_attributeFlags | (int)QuestionAttributes.HasSkipLogic;
                else
                    this.m_attributeFlags = this.m_attributeFlags ^ (int)QuestionAttributes.HasSkipLogic;

                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ValidationMode ValidationBehavior
        {
            get { return this.m_validationBehavior; }
            set
            {
                if (this.m_validationBehavior == value)
                    return;

                this.m_validationBehavior = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ValidationField1
        {
            get { return this.m_validationField1; }
            set
            {
                if (this.m_validationField1 == value)
                    return;

                this.m_validationField1 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ValidationField2
        {
            get { return this.m_validationField2; }
            set
            {
                if (this.m_validationField2 == value)
                    return;

                this.m_validationField2 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ValidationField3
        {
            get { return this.m_validationField3; }
            set
            {
                if (this.m_validationField3 == value)
                    return;

                this.m_validationField3 = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String RegularExpression
        {
            get { return this.m_regularExpression; }
            set
            {
                if (this.m_regularExpression == value)
                    return;

                this.m_regularExpression = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public RandomizationMode? RandomBehavior
        {
            get { return this.m_randomBehavior; }
            set
            {
                if (this.m_randomBehavior == value)
                    return;

                this.m_randomBehavior = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// <para>0 -> Single line of text</para>
        /// <para>1 -> Many lines of text</para>
        /// </summary>
        public OtherFieldType? OtherFieldType
        {
            get { return this.m_otherFieldType; }
            set
            {
                if (this.m_otherFieldType == value)
                    return;

                this.m_otherFieldType = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Byte? OtherFieldRows
        {
            get { return this.m_otherFieldRows; }
            set
            {
                if (this.m_otherFieldRows == value)
                    return;

                this.m_otherFieldRows = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Byte? OtherFieldChars
        {
            get { return this.m_otherFieldChars; }
            set
            {
                if (this.m_otherFieldChars == value)
                    return;

                this.m_otherFieldChars = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Byte OptionsSequence
        {
            get
            {
                return this.m_optionsSequence;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32 ColumnsSequence
        {
            get
            {
                return this.m_columnsSequence;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? RangeStart
        {
            get { return this.m_rangeStart; }
            set
            {
                if (this.m_rangeStart == value)
                    return;

                this.m_rangeStart = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? RangeEnd
        {
            get { return this.m_rangeEnd; }
            set
            {
                if (this.m_rangeEnd == value)
                    return;

                this.m_rangeEnd = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// Ενας κωδικός (τον οποίο τον δίνει ο Πελάτης) και συνοδεύει το export του question
        /// </summary>
        public Int32? LibraryQuestion
        {
            get { return m_libraryQuestion; }
            set
            {
                if (this.m_libraryQuestion == value)
                    return;

                this.m_libraryQuestion = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
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
        /// The language of thetranslatable fields..
        /// </summary>
        public System.Int16 TextsLanguage
        {
            get { return this.m_textsLanguage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String QuestionText
        {
            get { return this.m_questionText; }
            set
            {
                if (this.m_questionText == value)
                    return;

                this.m_questionText = value;
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
        /// <summary>
        /// 
        /// </summary>
        public System.String HelpText
        {
            get { return this.m_helpText; }
            set
            {
                if (this.m_helpText == value)
                    return;

                this.m_helpText = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String FrontLabelText
        {
            get { return this.m_frontLabelText; }
            set
            {
                if (this.m_frontLabelText == value)
                    return;

                this.m_frontLabelText = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String AfterLabelText
        {
            get { return this.m_afterLabelText; }
            set
            {
                if (this.m_afterLabelText == value)
                    return;

                this.m_afterLabelText = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String InsideText
        {
            get { return this.m_insideText; }
            set
            {
                if (this.m_insideText == value)
                    return;

                this.m_insideText = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String RequiredMessage
        {
            get { return this.m_requiredMessage; }
            set
            {
                if (this.m_requiredMessage == value)
                    return;

                this.m_requiredMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String ValidationMessage
        {
            get { return this.m_validationMessage; }
            set
            {
                if (this.m_validationMessage == value)
                    return;

                this.m_validationMessage = value;
                if (!this._deserializing && this.EntityState == EntityState.Unchanged)
                    this.EntityState = EntityState.Changed;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public System.String OtherFieldLabel
        {
            get { return this.m_otherFieldLabel; }
            set
            {
                if (this.m_otherFieldLabel == value)
                    return;

                this.m_otherFieldLabel = value;
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
        internal VLSurveyQuestion()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLSurveyQuestion(DbDataReader reader)
            : base(reader)
        {
            this.Survey = reader.GetInt32(0);
            this.QuestionId = reader.GetInt16(1);
            this.Page = reader.GetInt16(2);
            if (!reader.IsDBNull(3)) this.MasterQuestion = reader.GetInt16(3);
            this.DisplayOrder = reader.GetInt16(4);
            this.QuestionType = (QuestionType)reader.GetByte(5);
            if (!reader.IsDBNull(6)) this.CustomType = (QuestionType)reader.GetByte(6);
            this.IsRequired = reader.GetBoolean(7);
            if (!reader.IsDBNull(8)) this.RequiredBehavior = (RequiredMode)reader.GetByte(8);
            if (!reader.IsDBNull(9)) this.RequiredMinLimit = reader.GetInt16(9);
            if (!reader.IsDBNull(10)) this.RequiredMaxLimit = reader.GetInt16(10);
            this.AttributeFlags = reader.GetInt32(11);
            this.ValidationBehavior = (ValidationMode)reader.GetByte(12);
            if (!reader.IsDBNull(13)) this.ValidationField1 = reader.GetString(13);
            if (!reader.IsDBNull(14)) this.ValidationField2 = reader.GetString(14);
            if (!reader.IsDBNull(15)) this.ValidationField3 = reader.GetString(15);
            if (!reader.IsDBNull(16)) this.RegularExpression = reader.GetString(16);
            if (!reader.IsDBNull(17)) this.RandomBehavior = (RandomizationMode)reader.GetByte(17);
            if (!reader.IsDBNull(18)) this.OtherFieldType = (OtherFieldType)reader.GetByte(18);
            if (!reader.IsDBNull(19)) this.OtherFieldRows = reader.GetByte(19);
            if (!reader.IsDBNull(20)) this.OtherFieldChars = reader.GetByte(20);
            this.m_optionsSequence = reader.GetByte(21);
            this.m_columnsSequence = reader.GetByte(22);
            if (!reader.IsDBNull(23)) this.RangeStart = reader.GetInt32(23);
            if (!reader.IsDBNull(24)) this.RangeEnd = reader.GetInt32(24);
            if (!reader.IsDBNull(25)) this.LibraryQuestion = reader.GetInt32(25);
            if (!reader.IsDBNull(26)) this.CustomId = reader.GetString(26);

            this.m_textsLanguage = reader.GetInt16(27);
            this.QuestionText = reader.GetString(28);
            if (!reader.IsDBNull(29)) this.Description = reader.GetString(29);
            if (!reader.IsDBNull(30)) this.HelpText = reader.GetString(30);
            if (!reader.IsDBNull(31)) this.FrontLabelText = reader.GetString(31);
            if (!reader.IsDBNull(32)) this.AfterLabelText = reader.GetString(32);
            if (!reader.IsDBNull(33)) this.InsideText = reader.GetString(33);
            if (!reader.IsDBNull(34)) this.RequiredMessage = reader.GetString(34);
            if (!reader.IsDBNull(35)) this.ValidationMessage = reader.GetString(35);
            if (!reader.IsDBNull(36)) this.OtherFieldLabel = reader.GetString(36);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        internal VLSurveyQuestion(VLSurveyQuestion source)
        {
            this.m_survey = default(Int32);
            this.m_questionId = default(Int16);
            this.m_page = default(Int16);
            this.m_masterQuestion = source.m_masterQuestion;
            this.m_displayOrder = default(Int16);
            this.m_questionType = source.m_questionType;
            this.m_customType = source.m_customType;
            this.m_isRequired = source.m_isRequired;

            this.m_requiredBehavior = source.m_requiredBehavior;
            this.m_requiredMinLimit = source.m_requiredMinLimit;
            this.m_requiredMaxLimit = source.m_requiredMaxLimit;
            this.m_attributeFlags = default(Int32);

            this.OptionalInputBox = source.OptionalInputBox;
            this.RandomizeOptionsSequence = source.RandomizeOptionsSequence;
            this.DoNotRandomizeLastOption = source.DoNotRandomizeLastOption;
            this.RandomizeColumnSequence = source.RandomizeColumnSequence;
            this.OneResponsePerColumn = source.OneResponsePerColumn;
            this.UseDateTimeControls = source.UseDateTimeControls;
            this.AddResetLink = source.AddResetLink;
            this.HasSkipLogic = source.HasSkipLogic;

            this.m_validationBehavior = source.m_validationBehavior;
            this.m_validationField1 = source.m_validationField1;
            this.m_validationField2 = source.m_validationField2;
            this.m_validationField3 = source.m_validationField3;
            this.m_regularExpression = source.m_regularExpression;
            this.m_randomBehavior = source.m_randomBehavior;
            this.m_otherFieldType = source.m_otherFieldType;
            this.m_otherFieldRows = source.m_otherFieldRows;
            this.m_otherFieldChars = source.m_otherFieldChars;
            this.m_optionsSequence = default(byte);
            this.m_columnsSequence = default(byte);
            this.m_rangeStart = source.m_rangeStart;
            this.m_rangeEnd = source.m_rangeEnd;
            this.m_libraryQuestion = null;
            this.m_CustomId = default(string);

            this.m_textsLanguage = source.m_textsLanguage;
            this.m_questionText = source.m_questionText;
            this.m_description = source.m_description;
            this.m_helpText = source.m_helpText;
            this.m_frontLabelText = source.m_frontLabelText;
            this.m_afterLabelText = source.m_afterLabelText;
            this.m_insideText = source.m_insideText;
            this.m_requiredMessage = source.m_requiredMessage;
            this.m_validationMessage = source.m_validationMessage;
            this.m_otherFieldLabel = source.m_otherFieldLabel;

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
                this.QuestionId.GetHashCode() ^
                this.Page.GetHashCode() ^
                ((this.MasterQuestion == null) ? string.Empty : this.MasterQuestion.ToString()).GetHashCode() ^
                this.DisplayOrder.GetHashCode() ^
                this.QuestionType.GetHashCode() ^
                ((this.CustomType == null) ? string.Empty : this.CustomType.ToString()).GetHashCode() ^
                this.IsRequired.GetHashCode() ^
                ((this.RequiredBehavior == null) ? string.Empty : this.RequiredBehavior.ToString()).GetHashCode() ^
                ((this.RequiredMinLimit == null) ? string.Empty : this.RequiredMinLimit.ToString()).GetHashCode() ^
                ((this.RequiredMaxLimit == null) ? string.Empty : this.RequiredMaxLimit.ToString()).GetHashCode() ^
                this.AttributeFlags.GetHashCode() ^
                this.ValidationBehavior.GetHashCode() ^
                ((this.ValidationField1 == null) ? string.Empty : this.ValidationField1.ToString()).GetHashCode() ^
                ((this.ValidationField2 == null) ? string.Empty : this.ValidationField2.ToString()).GetHashCode() ^
                ((this.ValidationField3 == null) ? string.Empty : this.ValidationField3.ToString()).GetHashCode() ^
                ((this.RegularExpression == null) ? string.Empty : this.RegularExpression.ToString()).GetHashCode() ^
                ((this.RandomBehavior == null) ? string.Empty : this.RandomBehavior.ToString()).GetHashCode() ^
                ((this.OtherFieldType == null) ? string.Empty : this.OtherFieldType.ToString()).GetHashCode() ^
                ((this.OtherFieldRows == null) ? string.Empty : this.OtherFieldRows.ToString()).GetHashCode() ^
                ((this.OtherFieldChars == null) ? string.Empty : this.OtherFieldChars.ToString()).GetHashCode() ^
                this.OptionsSequence.GetHashCode() ^
                this.ColumnsSequence.GetHashCode() ^
                ((this.RangeStart == null) ? string.Empty : this.RangeStart.ToString()).GetHashCode() ^
                ((this.RangeEnd == null) ? string.Empty : this.RangeEnd.ToString()).GetHashCode() ^
                ((this.LibraryQuestion == null) ? string.Empty : this.LibraryQuestion.ToString()).GetHashCode() ^
                ((this.CustomId == null) ? string.Empty : this.CustomId.ToString()).GetHashCode() ^
                this.QuestionText.GetHashCode() ^
                ((this.Description == null) ? string.Empty : this.Description.ToString()).GetHashCode() ^
                ((this.HelpText == null) ? string.Empty : this.HelpText.ToString()).GetHashCode() ^
                ((this.FrontLabelText == null) ? string.Empty : this.FrontLabelText.ToString()).GetHashCode() ^
                ((this.AfterLabelText == null) ? string.Empty : this.AfterLabelText.ToString()).GetHashCode() ^
                ((this.InsideText == null) ? string.Empty : this.InsideText.ToString()).GetHashCode() ^
                ((this.RequiredMessage == null) ? string.Empty : this.RequiredMessage.ToString()).GetHashCode() ^
                ((this.ValidationMessage == null) ? string.Empty : this.ValidationMessage.ToString()).GetHashCode() ^
                ((this.OtherFieldLabel == null) ? string.Empty : this.OtherFieldLabel.ToString()).GetHashCode();
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


            var other = (VLSurveyQuestion)obj;

            //reference types
            if (!Object.Equals(ValidationField1, other.ValidationField1)) return false;
            if (!Object.Equals(ValidationField2, other.ValidationField2)) return false;
            if (!Object.Equals(ValidationField3, other.ValidationField3)) return false;
            if (!Object.Equals(RegularExpression, other.RegularExpression)) return false;
            if (!Object.Equals(CustomId, other.CustomId)) return false;
            if (!Object.Equals(QuestionText, other.QuestionText)) return false;
            if (!Object.Equals(Description, other.Description)) return false;
            if (!Object.Equals(HelpText, other.HelpText)) return false;
            if (!Object.Equals(FrontLabelText, other.FrontLabelText)) return false;
            if (!Object.Equals(AfterLabelText, other.AfterLabelText)) return false;
            if (!Object.Equals(InsideText, other.InsideText)) return false;
            if (!Object.Equals(RequiredMessage, other.RequiredMessage)) return false;
            if (!Object.Equals(ValidationMessage, other.ValidationMessage)) return false;
            if (!Object.Equals(OtherFieldLabel, other.OtherFieldLabel)) return false;
            //value types
            if (!Survey.Equals(other.Survey)) return false;
            if (!QuestionId.Equals(other.QuestionId)) return false;
            if (!Page.Equals(other.Page)) return false;
            if (!MasterQuestion.Equals(other.MasterQuestion)) return false;
            if (!DisplayOrder.Equals(other.DisplayOrder)) return false;
            if (!QuestionType.Equals(other.QuestionType)) return false;
            if (!CustomType.Equals(other.CustomType)) return false;
            if (!IsRequired.Equals(other.IsRequired)) return false;
            if (!RequiredBehavior.Equals(other.RequiredBehavior)) return false;
            if (!RequiredMinLimit.Equals(other.RequiredMinLimit)) return false;
            if (!RequiredMaxLimit.Equals(other.RequiredMaxLimit)) return false;
            if (!AttributeFlags.Equals(other.AttributeFlags)) return false;
            if (!ValidationBehavior.Equals(other.ValidationBehavior)) return false;
            if (!RandomBehavior.Equals(other.RandomBehavior)) return false;
            if (!OtherFieldType.Equals(other.OtherFieldType)) return false;
            if (!OtherFieldRows.Equals(other.OtherFieldRows)) return false;
            if (!OtherFieldChars.Equals(other.OtherFieldChars)) return false;
            if (!OptionsSequence.Equals(other.OptionsSequence)) return false;
            if (!ColumnsSequence.Equals(other.ColumnsSequence)) return false;
            if (!RangeStart.Equals(other.RangeStart)) return false;
            if (!RangeEnd.Equals(other.RangeEnd)) return false;
            if (!LibraryQuestion.Equals(other.LibraryQuestion)) return false;

            return true; 
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLSurveyQuestion o1, VLSurveyQuestion o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLSurveyQuestion o1, VLSurveyQuestion o2)
        {
            return !(o1 == o2);
        }

        #endregion



        /// <summary>
        /// 
        /// </summary>
        internal void ValidateInstance()
        {
            Utility.CheckParameter(ref m_validationField1, false, false, false, 50, "ValidationField1");
            Utility.CheckParameter(ref m_validationField2, false, false, false, 50, "ValidationField2");
            Utility.CheckParameter(ref m_validationField3, false, false, false, 50, "ValidationField3");
            Utility.CheckParameter(ref m_regularExpression, false, false, false, 512, "RegularExpression");
            Utility.CheckParameter(ref m_CustomId, false, false, false, 64, "CustomId");
            if (m_questionType != Core.QuestionType.DescriptiveText)
            {
                Utility.CheckParameter(ref m_questionText, true, true, false, 1024, "QuestionText");
            }
            else
            {
                Utility.CheckParameter(ref m_questionText, true, true, false, -1, "QuestionText");
            }
            Utility.CheckParameter(ref m_description, false, false, false, -1, "Description");
            Utility.CheckParameter(ref m_helpText, false, false, false, -1, "HelpText");
            Utility.CheckParameter(ref m_frontLabelText, false, false, false, 128, "FrontLabelText");
            Utility.CheckParameter(ref m_afterLabelText, false, false, false, 128, "AfterLabelText");
            Utility.CheckParameter(ref m_insideText, false, false, false, 50, "InsideText");
            Utility.CheckParameter(ref m_requiredMessage, false, false, false, 512, "RequiredMessage");
            Utility.CheckParameter(ref m_validationMessage, false, false, false, 512, "ValidationMessage");
            Utility.CheckParameter(ref m_otherFieldLabel, false, false, false, 128, "OtherFieldLabel");
        }


        public override string ToString()
        {
            return string.Format("{0}:{1} -> {2}", this.QuestionId, BuiltinLanguages.GetLanguageById(this.TextsLanguage).Name, this.QuestionText);
        }
    }
}
