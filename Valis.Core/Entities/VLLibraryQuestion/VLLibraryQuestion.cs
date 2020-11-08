using System;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Valis.Core
{
    /// <summary>
    /// Αντιπροσωπεύει μία ερώτηση που ανήκει σε μία βιβλιοθήκη ερωτήσεων
    /// </summary>
    [Serializable]
    [DataContract, DataObject]
    public sealed class VLLibraryQuestion : VLObject
    {
        #region class fields
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int32 m_questionId;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        Int16 m_category;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        QuestionType m_questionType;
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


        #region class public properties
        /// <summary>
        /// 
        /// </summary>
        public Int32 QuestionId
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
        public Int16 Category
        {
            get { return m_category; }
            set { m_category = value; }
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


        #region class constructors
        /// <summary>
        /// 
        /// </summary>
        internal VLLibraryQuestion()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        internal VLLibraryQuestion(DbDataReader reader)
            : base(reader)
        {
                this.QuestionId = reader.GetInt32(0);
				this.Category = reader.GetInt16(1);
                this.QuestionType = (QuestionType)reader.GetByte(2);
				this.IsRequired = reader.GetBoolean(3);
                if (!reader.IsDBNull(4)) this.RequiredBehavior = (RequiredMode)reader.GetByte(4);
				if (!reader.IsDBNull(5)) this.RequiredMinLimit = reader.GetInt16(5);
				if (!reader.IsDBNull(6)) this.RequiredMaxLimit = reader.GetInt16(6);
				this.AttributeFlags = reader.GetInt32(7);
                this.ValidationBehavior = (ValidationMode)reader.GetByte(8);
				if (!reader.IsDBNull(9)) this.ValidationField1 = reader.GetString(9);
				if (!reader.IsDBNull(10)) this.ValidationField2 = reader.GetString(10);
				if (!reader.IsDBNull(11)) this.ValidationField3 = reader.GetString(11);
				if (!reader.IsDBNull(12)) this.RegularExpression = reader.GetString(12);
                if (!reader.IsDBNull(13)) this.RandomBehavior = (RandomizationMode)reader.GetByte(13);
                if (!reader.IsDBNull(14)) this.OtherFieldType = (OtherFieldType)reader.GetByte(14);
				if (!reader.IsDBNull(15)) this.OtherFieldRows = reader.GetByte(15);
				if (!reader.IsDBNull(16)) this.OtherFieldChars = reader.GetByte(16);
                this.m_optionsSequence = reader.GetByte(17);
                this.m_columnsSequence = reader.GetByte(18);
				if (!reader.IsDBNull(19)) this.RangeStart = reader.GetInt32(19);
				if (!reader.IsDBNull(20)) this.RangeEnd = reader.GetInt32(20);
                this.m_textsLanguage = reader.GetInt16(21);
				this.QuestionText = reader.GetString(22);
				if (!reader.IsDBNull(23)) this.Description = reader.GetString(23);
				if (!reader.IsDBNull(24)) this.HelpText = reader.GetString(24);
				if (!reader.IsDBNull(25)) this.FrontLabelText = reader.GetString(25);
				if (!reader.IsDBNull(26)) this.AfterLabelText = reader.GetString(26);
				if (!reader.IsDBNull(27)) this.InsideText = reader.GetString(27);
				if (!reader.IsDBNull(28)) this.RequiredMessage = reader.GetString(28);
				if (!reader.IsDBNull(29)) this.ValidationMessage = reader.GetString(29);
				if (!reader.IsDBNull(30)) this.OtherFieldLabel = reader.GetString(30);

        }
        #endregion


        #region GetHashCode & Equals overrides
        /// <summary>
        /// Serves as a hash function for a particular type. GetHashCode is suitable for use in hashing algorithms and data structures like a hash table
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.QuestionId.GetHashCode() ^
                this.Category.GetHashCode() ^
                this.QuestionType.GetHashCode() ^
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


            var other = (VLLibraryQuestion)obj;

            //reference types
            if (!Object.Equals(ValidationField1, other.ValidationField1)) return false;
            if (!Object.Equals(ValidationField2, other.ValidationField2)) return false;
            if (!Object.Equals(ValidationField3, other.ValidationField3)) return false;
            if (!Object.Equals(RegularExpression, other.RegularExpression)) return false;
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
            if (!QuestionId.Equals(other.QuestionId)) return false;
            if (!m_category.Equals(other.m_category)) return false;
            if (!QuestionType.Equals(other.QuestionType)) return false;
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

            return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator ==(VLLibraryQuestion o1, VLLibraryQuestion o2)
        {
            return Object.Equals(o1, o2);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="o1"></param>
        /// <param name="o2"></param>
        /// <returns></returns>
        public static Boolean operator !=(VLLibraryQuestion o1, VLLibraryQuestion o2)
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
