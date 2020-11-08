using log4net;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Valis.Core
{
    public sealed class VLLibraryManager : VLManagerBase
    {
        static ILog Logger = LogManager.GetLogger(typeof(VLLibraryManager));

        
        #region support stuff
        private VLLibraryManager(IAccessToken accessToken) : base(accessToken) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static VLLibraryManager GetAnInstance(IAccessToken accessToken)
        {
            var instance = new VLLibraryManager(accessToken);
            return instance;
        }
        #endregion


        #region VLLibraryQuestionCategory
        public Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategories(string whereClause = null, string orderByClause = "order by Name", short textsLanguage = -2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestionCategories(this.AccessTokenId, whereClause, orderByClause, textsLanguage);
        }
        public Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategories(int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = "order by Name", short textsLanguage = -2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestionCategories(this.AccessTokenId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        public VLLibraryQuestionCategory GetLibraryQuestionCategoryById(Int16 categoryId, short textsLanguage = -2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestionCategoryById(this.AccessTokenId, categoryId, textsLanguage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        public VLLibraryQuestionCategory CreateLibraryQuestionCategory(string name, short textsLanguage = -2)
        {
            Utility.CheckParameter(ref name, true, true, false, 128, "Name");

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ManageBuidingBlocks);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            /*
             * Ελέγχουμε εάν υπάρχει ήδη στην βάση αυτό το name, σε οποιαδήποτε γλώσσα:
             */
            var existingItem = LibrariesDal.GetLibraryQuestionCategoryByName(this.AccessTokenId, name, textsLanguage);
            if (existingItem != null)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", name));
            }

            var category = new VLLibraryQuestionCategory();
            category.Name = name;
            return LibrariesDal.CreateLibraryQuestionCategory(this.AccessTokenId, category, textsLanguage);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public VLLibraryQuestionCategory UpdateLibraryQuestionCategory(VLLibraryQuestionCategory category)
        {
            if (category == null) throw new ArgumentNullException("category");
            category.ValidateInstance();

            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ManageBuidingBlocks);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            /*Το name κάθε κατηγορίας πρέπει να είναι μοναδικό*/
            var existingItem = LibrariesDal.GetLibraryQuestionCategoryByName(this.AccessTokenId, category.Name, category.TextsLanguage);
            if (existingItem != null && existingItem.CategoryId != category.CategoryId)
            {
                throw new VLException(SR.GetString(SR.Value_is_already_in_use, "Name", category.Name));
            }
            if (existingItem == null)
            {
                existingItem = LibrariesDal.GetLibraryQuestionCategoryById(AccessTokenId, category.CategoryId, category.TextsLanguage);
            }
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryQuestionCategory", category.CategoryId));

            existingItem.AttributeFlags = category.AttributeFlags;
            existingItem.Name = category.Name;

            return LibrariesDal.UpdateLibraryQuestionCategory(this.AccessTokenId, existingItem);
        }

        public void DeleteLibraryQuestionCategory(VLLibraryQuestionCategory category)
        {
            if (category == null) throw new ArgumentNullException("category");
            DeleteLibraryQuestionCategory(category.CategoryId);
        }
        public void DeleteLibraryQuestionCategory(Int16 categoryId)
        {
            var existingItem = LibrariesDal.GetLibraryQuestionCategoryById(this.AccessTokenId, categoryId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryQuestionCategory", categoryId));

            //Can we delete the LibraryQuestinCategory?
            if (existingItem.IsBuiltIn)
            {
                throw new VLException(SR.GetString(SR.You_cannot_delete_builtin, "LibraryQuestionCategory"));
            }

            var _total = LibrariesDal.GetLibraryQuestionsCount(this.AccessTokenId, categoryId);
            if( _total == 1 )
            {
                throw new VLException(string.Format("You cannot delete library category '{0}'. There is one question depending on it!", existingItem.Name));
            }
            else if(_total > 1)
            {
                throw new VLException(string.Format("You cannot delete library category '{0}'. There are questions depending on it!", existingItem.Name));
            }


            LibrariesDal.DeleteLibraryQuestionCategory(this.AccessTokenId, existingItem.CategoryId);
        }
        #endregion

        #region VLLibraryQuestion
        public Collection<VLLibraryQuestion> GetLibraryQuestions(Int16 categoryId, string whereClause = null, string orderByClause = null, short textsLanguage = /*DefaultLanguage*/-2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestions(this.AccessTokenId, categoryId, whereClause, orderByClause);
        }
        public Collection<VLLibraryQuestion> GetLibraryQuestions(Int16 categoryId, int pageIndex, int pageSize, ref int totalRows, string whereClause = null, string orderByClause = null, short textsLanguage = /*DefaultLanguage*/-2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestions(this.AccessTokenId, categoryId, pageIndex, pageSize, ref totalRows, whereClause, orderByClause, textsLanguage);
        }
        public VLLibraryQuestion GetLibraryQuestionById(Int32 questionId, short textsLanguage = /*DefaultLanguage*/-2)
        {
            #region SecurityLayer
            #endregion

            return LibrariesDal.GetLibraryQuestionById(this.AccessTokenId, questionId, textsLanguage);
        }
        public VLLibraryQuestion CreateLibraryQuestion(Int16 categoryId, QuestionType type, string questionText, short textsLanguage = /*DefaultLanguage*/-2)
        {
            VLLibraryQuestion question = new VLLibraryQuestion();
            question.Category = categoryId;
            question.QuestionType = type;
            question.QuestionText = questionText;

            return CreateLibraryQuestion(question, textsLanguage);
        }
        internal VLLibraryQuestion CreateLibraryQuestion(VLLibraryQuestion question, short textsLanguage = /*DefaultLanguage*/-2)
        {
            if (question == null) throw new ArgumentNullException("question");
            question.ValidateInstance();


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ManageBuidingBlocks);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            /*Υπάρχει η κατηγορία?*/
            var category = LibrariesDal.GetLibraryQuestionCategoryById(this.AccessTokenId, question.Category);
            if (category == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryQuestionCategory", question.Category));
            }

            return LibrariesDal.CreateLibraryQuestion(this.AccessTokenId, question);
        }
        public VLLibraryQuestion UpdateLibraryQuestion(VLLibraryQuestion question)
        {
            #region input parameters validation
            if (question == null) throw new ArgumentNullException("question");
            question.ValidateInstance();
            #endregion


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ManageBuidingBlocks);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion


            //Υπάρχει η ερώτησή στο σύστημα:
            var existingItem = LibrariesDal.GetLibraryQuestionById(AccessTokenId, question.QuestionId, question.TextsLanguage);
            if (existingItem == null)
            {
                throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Question", question.QuestionId));
            }
            //Δεν μπορούμε να αλλάξουμε τον τύπο της ερώτησης στο update:
            if (existingItem.QuestionType != question.QuestionType)
            {
                throw new VLException(string.Format("You cannot change the questionType of an existing question from {0} to {1}!", existingItem.QuestionType.ToString(), question.QuestionType.ToString()));
            }

            /*Ελέγχουμε την εγκυρότητα της αλλαγμένης ερώτησης, που πάμε να αποθηκεύσουμε:*/
            _Validatequestion(question);





            //existingItem.Category = question.Category;
            //existingItem.QuestionType = question.QuestionType;
            existingItem.IsRequired = question.IsRequired;
            existingItem.RequiredBehavior = question.RequiredBehavior;
            existingItem.RequiredMinLimit = question.RequiredMinLimit;
            existingItem.RequiredMaxLimit = question.RequiredMaxLimit;

            //AttributeFlags
            existingItem.OptionalInputBox = question.OptionalInputBox;
            existingItem.RandomizeOptionsSequence = question.RandomizeOptionsSequence;
            existingItem.DoNotRandomizeLastOption = question.DoNotRandomizeLastOption;
            existingItem.RandomizeColumnSequence = question.RandomizeColumnSequence;
            existingItem.OneResponsePerColumn = question.OneResponsePerColumn;
            existingItem.AddResetLink = question.AddResetLink;
            existingItem.UseDateTimeControls = question.UseDateTimeControls;

            existingItem.ValidationBehavior = question.ValidationBehavior;
            existingItem.ValidationField1 = question.ValidationField1;
            existingItem.ValidationField2 = question.ValidationField2;
            existingItem.ValidationField3 = question.ValidationField3;
            existingItem.RegularExpression = question.RegularExpression;
            existingItem.RandomBehavior = question.RandomBehavior;
            existingItem.OtherFieldType = question.OtherFieldType;
            existingItem.OtherFieldRows = question.OtherFieldRows;
            existingItem.OtherFieldChars = question.OtherFieldChars;
            //OptionsSequence
            //ColumnsSequence
            existingItem.RangeStart = question.RangeStart;
            existingItem.RangeEnd = question.RangeEnd;
            //TextsLanguage
            existingItem.QuestionText = question.QuestionText;
            existingItem.Description = question.Description;
            existingItem.HelpText = question.HelpText;
            existingItem.FrontLabelText = question.FrontLabelText;
            existingItem.AfterLabelText = question.AfterLabelText;
            existingItem.InsideText = question.InsideText;
            existingItem.RequiredMessage = question.RequiredMessage;
            existingItem.ValidationMessage = question.ValidationMessage;
            existingItem.OtherFieldLabel = question.OtherFieldLabel;



            return LibrariesDal.UpdateLibraryQuestion(AccessTokenId, existingItem);
        }
        public void DeleteLibraryQuestion(Int32 questionId)
        {
            var existingItem = LibrariesDal.GetLibraryQuestionById(this.AccessTokenId, questionId);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "LibraryQuestion", questionId));


            #region SecurityLayer
            if (this.PrincipalType == Core.PrincipalType.SystemUser)
            {
                CheckPermissions(VLPermissions.ManageSystem, VLPermissions.Developer, VLPermissions.SystemService, VLPermissions.ManageBuidingBlocks);
            }
            else
            {
                throw new VLAccessDeniedException();
            }
            #endregion

            //Δεν μπορούμε να διαγράψουμε την συγκεκριμένη ερώτηση εάν έχει ήδη χρησιμοποιηθεί:
            if(SurveysDal.GetQuestionsForLibraryQuestionCount(this.AccessTokenId, existingItem.QuestionId) > 0)
            {
                throw new VLException("You cannot delete this LibraryQuestion! It is been used by Survey Questions!");
            }





            LibrariesDal.DeleteLibraryQuestion(this.AccessTokenId, questionId);
        }

        private void _Validatequestion(VLLibraryQuestion question)
        {

            if (question.IsRequired == false)
            {
                question.RequiredBehavior = null;
                question.RequiredMinLimit = null;
                question.RequiredMaxLimit = null;
                question.RequiredMessage = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(question.RequiredMessage))
                {
                    throw new VLException("RequiredMessage cannot be null or an empty string!");
                }
            }
            if (question.OptionalInputBox == false)
            {
                question.OtherFieldType = null;
                question.OtherFieldRows = null;
                question.OtherFieldChars = null;
                question.OtherFieldLabel = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(question.OtherFieldLabel))
                {
                    throw new VLException("You must give a label for the OtherField!");
                }
                if (question.OtherFieldType != OtherFieldType.SingleLine && question.OtherFieldType != OtherFieldType.MultipleLines)
                {
                    throw new VLException("You must give a type for the OtherField!");
                }

            }
            if (question.ValidationBehavior == ValidationMode.DoNotValidate)
            {
                question.ValidationField1 = null;
                question.ValidationField2 = null;
                question.ValidationField3 = null;
                question.RegularExpression = null;
                question.ValidationMessage = null;
            }
            else
            {

                if (question.ValidationBehavior == ValidationMode.TextOfSpecificLength || question.ValidationBehavior == ValidationMode.WholeNumber)
                {
                    Int32 _between = 0, _and = 0;

                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        throw new VLException("ValidationMessage cannot be null or an empty string!");
                    }

                    //Πρέπει το ValidationField1 & ValidationField2 να είναι ακέραιοι αριθμοί και ValidationField1 <= ValidationField2:                    
                    if (string.IsNullOrWhiteSpace(question.ValidationField1))
                    {
                        throw new VLException("Validation setup error. First range value is wrong!");
                    }
                    if (string.IsNullOrWhiteSpace(question.ValidationField2))
                    {
                        throw new VLException("Validation setup error. Second range value is wrong!");
                    }
                    try
                    {
                        _between = Convert.ToInt32(question.ValidationField1, CultureInfo.InvariantCulture);
                        _and = Convert.ToInt32(question.ValidationField2, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new VLException("Validation setup error. Range values have wrong types!");
                    }

                    if (_between > _and)
                    {
                        throw new VLException("Validation setup error. Range values are wrong!");
                    }
                }
                else if (question.ValidationBehavior == ValidationMode.DecimalNumber)
                {
                    Double _between = 0, _and = 0;

                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        throw new VLException("ValidationMessage cannot be null or an empty string!");
                    }

                    //Πρέπει το ValidationField1 & ValidationField2 να είναι ακέραιοι αριθμοί και ValidationField1 <= ValidationField2:
                    if (string.IsNullOrWhiteSpace(question.ValidationField1))
                    {
                        throw new VLException("Validation setup error. First range value is wrong!");
                    }
                    if (string.IsNullOrWhiteSpace(question.ValidationField2))
                    {
                        throw new VLException("Validation setup error. Second range value is wrong!");
                    }
                    try
                    {
                        _between = Convert.ToDouble(question.ValidationField1, CultureInfo.InvariantCulture);
                        _and = Convert.ToDouble(question.ValidationField2, CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        throw new VLException("Validation setup error. Range values have wrong types!");
                    }

                    if (_between > _and)
                    {
                        throw new VLException("Validation setup error. Range values are wrong!");
                    }
                }
                else if (question.ValidationBehavior == ValidationMode.Date1)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "The specified date is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.Date2)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "The specified date is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.Email)
                {
                    //Είναι απαραίτητο να υπάρχει ValidationMessage:
                    if (string.IsNullOrWhiteSpace(question.ValidationMessage))
                    {
                        question.ValidationMessage = "Email address is invalid";
                    }

                }
                else if (question.ValidationBehavior == ValidationMode.RegularExpression)
                {
                    //ΔΕΝ Είναι απαραίτητο να υπάρχει ValidationMessage:

                }

            }
        }
        #endregion


        #region VLLibraryOption
        public Collection<VLLibraryOption> GetLibraryQuestionOptions(VLLibraryQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");
            return GetLibraryQuestionOptions(question.QuestionId, question.TextsLanguage);
        }
        public Collection<VLLibraryOption> GetLibraryQuestionOptions(Int32 question, short textsLanguage = /*DefaultLanguage*/-2)
        {
            return LibrariesDal.GetLibraryOptions(this.AccessTokenId, question, textsLanguage);
        }
        public VLLibraryOption GetLibraryQuestionOptionById(Int32 question, Byte option, short textsLanguage = /*DefaultLanguage*/-2)
        {
            return LibrariesDal.GetLibraryOptionById(AccessTokenId, question, option, textsLanguage);
        }

        public void DeleteLibraryQuestionOption(VLLibraryOption option)
        {
            if (option == null) throw new ArgumentNullException("option");
            DeleteLibraryQuestionOption(option.Question, option.OptionId);
        }

        public void DeleteLibraryQuestionOption(Int32 question, byte option)
        {
            LibrariesDal.DeleteLibraryOption(this.AccessTokenId, question, option);
        }


        public VLLibraryOption CreateLibraryQuestionOption(VLLibraryQuestion question, string optionText, QuestionOptionType type = QuestionOptionType.Default)
        {
            if (question == null) throw new ArgumentNullException("question");
            return CreateLibraryQuestionOption(question.QuestionId, optionText, type, question.TextsLanguage);
        }
        public VLLibraryOption CreateLibraryQuestionOption(Int32 question, string optionText, QuestionOptionType type = QuestionOptionType.Default, short textsLanguage = /*DefaultLanguage*/-2)
        {
            VLLibraryOption option = new VLLibraryOption();
            option.Question = question;
            option.OptionText = optionText;
            option.OptionType = type;
            return LibrariesDal.CreateLibraryOption(this.AccessTokenId, option, textsLanguage);
        }
        public VLLibraryOption UpdateLibraryQuestionOption(VLLibraryOption option)
        {
            if (option == null) throw new ArgumentNullException("option");
            option.ValidateInstance();

            //διαβάζουμε το option απο την βάση μας:
            var existingItem = LibrariesDal.GetLibraryOptionById(AccessTokenId, option.Question, option.OptionId, option.TextsLanguage);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Option", option.OptionId));


            existingItem.OptionText = option.OptionText;

            return LibrariesDal.UpdateLibraryOption(AccessTokenId, existingItem);
        }
        #endregion


        #region VLLibraryColumn
        public Collection<VLLibraryColumn> GetLibraryQuestionColumns(VLLibraryQuestion question)
        {
            if (question == null) throw new ArgumentNullException("question");
            return GetLibraryQuestionColumns(question.QuestionId, question.TextsLanguage);
        }
        public Collection<VLLibraryColumn> GetLibraryQuestionColumns(Int32 question, short textsLanguage = /*DefaultLanguage*/-2)
        {
            return LibrariesDal.GetLibraryColumns(this.AccessTokenId, question, textsLanguage);
        }
        public VLLibraryColumn GetLibraryQuestionColumnById(Int32 question, byte columnId, short textsLanguage = /*DefaultLanguage*/-2)
        {
            return LibrariesDal.GetLibraryColumnById(AccessTokenId, question, columnId, textsLanguage);
        }

        public void DeleteLibraryQuestionColumn(VLLibraryColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");
            DeleteLibraryQuestionColumn(column.Question, column.ColumnId);
        }

        public void DeleteLibraryQuestionColumn(Int32 question, byte columnId)
        {
            LibrariesDal.DeleteLibraryColumn(this.AccessTokenId, question, columnId);
        }


        public VLLibraryColumn CreateLibraryQuestionColumn(VLLibraryQuestion question, string columnText)
        {
            if (question == null) throw new ArgumentNullException("question");
            return CreateLibraryQuestionColumn(question.QuestionId, columnText, question.TextsLanguage);
        }
        public VLLibraryColumn CreateLibraryQuestionColumn(Int32 question, string columnText, short textsLanguage = /*DefaultLanguage*/-2)
        {
            VLLibraryColumn column = new VLLibraryColumn();
            column.Question = question;
            column.ColumnText = columnText;

            return LibrariesDal.CreateLibraryColumn(this.AccessTokenId, column, textsLanguage);
        }
        public VLLibraryColumn UpdateLibraryQuestionColumn(VLLibraryColumn column)
        {
            if (column == null) throw new ArgumentNullException("column");
            column.ValidateInstance();

            //διαβάζουμε το option απο την βάση μας:
            var existingItem = LibrariesDal.GetLibraryColumnById(AccessTokenId, column.Question, column.ColumnId, column.TextsLanguage);
            if (existingItem == null) throw new VLException(SR.GetString(SR.There_is_no_item_with_id, "Column", column.ColumnId));


            existingItem.ColumnText = column.ColumnText;

            return LibrariesDal.UpdateLibraryColumn(AccessTokenId, existingItem);
        }

        #endregion

    }
}
