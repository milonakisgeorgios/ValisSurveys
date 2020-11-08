using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Valis.Core.Dal;

namespace Valis.Core.SqlServer
{
    internal class LibrariesDao : LibrariesDaoBase
    {

        #region VLLibraryQuestionCategory

        internal override Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategoriesImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryQuestionCategories(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionCategoriesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesImpl"), ex);
            }
        }
        internal override int GetLibraryQuestionCategoriesCountImpl(Int32 accessToken, string whereClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionCategoriesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesCountImpl"), ex);
            }
        }
        internal override Collection<VLLibraryQuestionCategory> GetLibraryQuestionCategoriesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);

                var collection = ExecuteAndGetLibraryQuestionCategories(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionCategoriesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesPagedImpl"), ex);
            }
        }
        internal override VLLibraryQuestionCategory GetLibraryQuestionCategoryByIdImpl(Int32 accessToken, Int16 categoryId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@categoryId", categoryId, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryQuestionCategory(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionCategoriesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionCategoriesByIdImpl"), ex);
            }
        }
        internal override void DeleteLibraryQuestionCategoryImpl(Int32 accessToken, Int16 categoryId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@categoryId", categoryId, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "DeleteLibraryQuestionCategoryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryQuestionCategoryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryQuestionCategoryImpl"), ex);
            }
        }
        internal override VLLibraryQuestionCategory CreateLibraryQuestionCategoryImpl(Int32 accessToken, VLLibraryQuestionCategory category, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@attributeFlags", category.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@name", category.Name, DbType.String);

                return ExecuteAndGetLibraryQuestionCategory(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "CreateLibraryQuestionCategoryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryQuestionCategoryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryQuestionCategoryImpl"), ex);
            }
        }
        internal override VLLibraryQuestionCategory UpdateLibraryQuestionCategoryImpl(Int32 accessToken, VLLibraryQuestionCategory category, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestioncategories_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@categoryId", category.CategoryId, DbType.Int16);
                AddParameter(command, "@attributeFlags", category.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", category.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", category.TextsLanguage, DbType.Int16);
                AddParameter(command, "@name", category.Name, DbType.String);

                return ExecuteAndGetLibraryQuestionCategory(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "UpdateLibraryQuestionCategoryImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryQuestionCategoryImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryQuestionCategoryImpll"), ex);
            }
        }
        
        #endregion


        #region VLLibraryQuestion
        internal override Collection<VLLibraryQuestion> GetLibraryQuestionsExImpl(Int32 accessToken,string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsExImpl"), ex);
            }
        }
        internal override int GetLibraryQuestionsCountExImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetTotalRowsEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsCountExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsCountExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsCountExImpl"), ex);
            }
        }
        internal override Collection<VLLibraryQuestion> GetLibraryQuestionsPagedExImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetPageEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetLibraryQuestions(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsPagedExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsPagedExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsPagedExImpl"), ex);
            }
        }

        internal override Collection<VLLibraryQuestion> GetLibraryQuestionsImpl(Int32 accessToken, Int16 category, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@category", category, DbType.Int16);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsImpl"), ex);
            }
        }
        internal override int GetLibraryQuestionsCountImpl(Int32 accessToken, Int16 category, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@category", category, DbType.Int16);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsCountImpl"), ex);
            }
        }
        internal override Collection<VLLibraryQuestion> GetLibraryQuestionsPagedImpl(Int32 accessToken, Int16 category, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@category", category, DbType.Int16);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetLibraryQuestions(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsPagedImpl"), ex);
            }
        }
        
        internal override VLLibraryQuestion GetLibraryQuestionByIdImpl(Int32 accessToken, Int32 questionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@questionId", questionId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryQuestionsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryQuestionsByIdImpl"), ex);
            }
        }
        internal override void DeleteLibraryQuestionImpl(Int32 accessToken, Int32 questionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@questionId", questionId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "DeleteLibraryQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryQuestionImpl"), ex);
            }
        }
        internal override VLLibraryQuestion CreateLibraryQuestionImpl(Int32 accessToken, VLLibraryQuestion question, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@category", question.Category, DbType.Int16);
                AddParameter(command, "@questionType", question.QuestionType, DbType.Byte);
                AddParameter(command, "@isRequired", question.IsRequired, DbType.Boolean);
                AddParameter(command, "@requiredBehavior", question.RequiredBehavior, DbType.Byte);
                AddParameter(command, "@requiredMinLimit", question.RequiredMinLimit, DbType.Int16);
                AddParameter(command, "@requiredMaxLimit", question.RequiredMaxLimit, DbType.Int16);
                AddParameter(command, "@attributeFlags", question.AttributeFlags, DbType.Int32);
                AddParameter(command, "@validationBehavior", question.ValidationBehavior, DbType.Byte);
                AddParameter(command, "@validationField1", question.ValidationField1, DbType.String);
                AddParameter(command, "@validationField2", question.ValidationField2, DbType.String);
                AddParameter(command, "@validationField3", question.ValidationField3, DbType.String);
                AddParameter(command, "@regularExpression", question.RegularExpression, DbType.String);
                AddParameter(command, "@randomBehavior", question.RandomBehavior, DbType.Byte);
                AddParameter(command, "@otherFieldType", question.OtherFieldType, DbType.Byte);
                AddParameter(command, "@otherFieldRows", question.OtherFieldRows, DbType.Byte);
                AddParameter(command, "@otherFieldChars", question.OtherFieldChars, DbType.Byte);
                AddParameter(command, "@optionsSequence", question.OptionsSequence, DbType.Byte);
                AddParameter(command, "@columnsSequence", question.ColumnsSequence, DbType.Byte);
                AddParameter(command, "@rangeStart", question.RangeStart, DbType.Int32);
                AddParameter(command, "@rangeEnd", question.RangeEnd, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@questionText", question.QuestionText, DbType.String);
                AddParameter(command, "@description", question.Description, DbType.String);
                AddParameter(command, "@helpText", question.HelpText, DbType.String);
                AddParameter(command, "@frontLabelText", question.FrontLabelText, DbType.String);
                AddParameter(command, "@afterLabelText", question.AfterLabelText, DbType.String);
                AddParameter(command, "@insideText", question.InsideText, DbType.String);
                AddParameter(command, "@requiredMessage", question.RequiredMessage, DbType.String);
                AddParameter(command, "@validationMessage", question.ValidationMessage, DbType.String);
                AddParameter(command, "@otherFieldLabel", question.OtherFieldLabel, DbType.String);

                return ExecuteAndGetLibraryQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "CreateLibraryQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryQuestionImpl"), ex);
            }
        }
        internal override VLLibraryQuestion UpdateLibraryQuestionImpl(Int32 accessToken, VLLibraryQuestion question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryquestions_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@questionId", question.QuestionId, DbType.Int32);
                AddParameter(command, "@category", question.Category, DbType.Int16);
                AddParameter(command, "@questionType", question.QuestionType, DbType.Byte);
                AddParameter(command, "@isRequired", question.IsRequired, DbType.Boolean);
                AddParameter(command, "@requiredBehavior", question.RequiredBehavior, DbType.Byte);
                AddParameter(command, "@requiredMinLimit", question.RequiredMinLimit, DbType.Int16);
                AddParameter(command, "@requiredMaxLimit", question.RequiredMaxLimit, DbType.Int16);
                AddParameter(command, "@attributeFlags", question.AttributeFlags, DbType.Int32);
                AddParameter(command, "@validationBehavior", question.ValidationBehavior, DbType.Byte);
                AddParameter(command, "@validationField1", question.ValidationField1, DbType.String);
                AddParameter(command, "@validationField2", question.ValidationField2, DbType.String);
                AddParameter(command, "@validationField3", question.ValidationField3, DbType.String);
                AddParameter(command, "@regularExpression", question.RegularExpression, DbType.String);
                AddParameter(command, "@randomBehavior", question.RandomBehavior, DbType.Byte);
                AddParameter(command, "@otherFieldType", question.OtherFieldType, DbType.Byte);
                AddParameter(command, "@otherFieldRows", question.OtherFieldRows, DbType.Byte);
                AddParameter(command, "@otherFieldChars", question.OtherFieldChars, DbType.Byte);
                AddParameter(command, "@optionsSequence", question.OptionsSequence, DbType.Byte);
                AddParameter(command, "@columnsSequence", question.ColumnsSequence, DbType.Byte);
                AddParameter(command, "@rangeStart", question.RangeStart, DbType.Int32);
                AddParameter(command, "@rangeEnd", question.RangeEnd, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", question.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", question.TextsLanguage, DbType.Int16);
                AddParameter(command, "@questionText", question.QuestionText, DbType.String);
                AddParameter(command, "@description", question.Description, DbType.String);
                AddParameter(command, "@helpText", question.HelpText, DbType.String);
                AddParameter(command, "@frontLabelText", question.FrontLabelText, DbType.String);
                AddParameter(command, "@afterLabelText", question.AfterLabelText, DbType.String);
                AddParameter(command, "@insideText", question.InsideText, DbType.String);
                AddParameter(command, "@requiredMessage", question.RequiredMessage, DbType.String);
                AddParameter(command, "@validationMessage", question.ValidationMessage, DbType.String);
                AddParameter(command, "@otherFieldLabel", question.OtherFieldLabel, DbType.String);

                return ExecuteAndGetLibraryQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "UpdateLibraryQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryQuestionImpll"), ex);
            }
        }
        #endregion





        internal override Collection<VLLibraryOption> GetLibraryOptionsImpl(Int32 accessToken, Int32 questionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryoptions_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", questionId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryOptions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryOptionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryOptionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryOptionsImpl"), ex);
            }
        }
        internal override VLLibraryOption GetLibraryOptionByIdImpl(Int32 accessToken, Int32 question, Byte optionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryoptions_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int32);
                AddParameter(command, "@optionId", optionId, DbType.Byte);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryOptionsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryOptionsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryOptionsByIdImpl"), ex);
            }
        }
        internal override void DeleteLibraryOptionImpl(Int32 accessToken, Int32 question, Byte optionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryoptions_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int32);
                AddParameter(command, "@optionId", optionId, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "DeleteLibraryOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryOptionImpl"), ex);
            }
        }
        internal override VLLibraryOption CreateLibraryOptionImpl(Int32 accessToken, VLLibraryOption option, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryoptions_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", option.Question, DbType.Int32);
                AddParameter(command, "@optionType", option.OptionType, DbType.Byte);
                AddParameter(command, "@displayOrder", option.DisplayOrder, DbType.Int16);
                AddParameter(command, "@optionValue", option.OptionValue, DbType.Int16);
                AddParameter(command, "@attributeFlags", option.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@optionText", option.OptionText, DbType.String);

                return ExecuteAndGetLibraryOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "CreateLibraryOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryOptionImpl"), ex);
            }
        }
        internal override VLLibraryOption UpdateLibraryOptionImpl(Int32 accessToken, VLLibraryOption option, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_libraryoptions_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", option.Question, DbType.Int32);
                AddParameter(command, "@optionId", option.OptionId, DbType.Byte);
                AddParameter(command, "@optionType", option.OptionType, DbType.Byte);
                AddParameter(command, "@displayOrder", option.DisplayOrder, DbType.Int16);
                AddParameter(command, "@optionValue", option.OptionValue, DbType.Int16);
                AddParameter(command, "@attributeFlags", option.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", option.TextsLanguage, DbType.Int16);
                AddParameter(command, "@optionText", option.OptionText, DbType.String);

                return ExecuteAndGetLibraryOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "UpdateLibraryOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryOptionImpll"), ex);
            }
        }




        internal override Collection<VLLibraryColumn> GetLibraryColumnsImpl(Int32 accessToken, Int32 questionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_librarycolumns_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", questionId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryColumns(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryColumnsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryColumnsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryColumnsImpl"), ex);
            }
        }
        internal override VLLibraryColumn GetLibraryColumnByIdImpl(Int32 accessToken, Int32 question, Byte columnId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_librarycolumns_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int32);
                AddParameter(command, "@columnId", columnId, DbType.Byte);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetLibraryColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "GetLibraryColumnsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryColumnsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "GetLibraryColumnsByIdImpl"), ex);
            }
        }
        internal override void DeleteLibraryColumnImpl(Int32 accessToken, Int32 question, Byte columnId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_librarycolumns_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int32);
                AddParameter(command, "@columnId", columnId, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "DeleteLibraryColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "DeleteLibraryColumnImpl"), ex);
            }
        }
        internal override VLLibraryColumn CreateLibraryColumnImpl(Int32 accessToken, VLLibraryColumn column, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_librarycolumns_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", column.Question, DbType.Int32);
                AddParameter(command, "@displayOrder", column.DisplayOrder, DbType.Byte);
                AddParameter(command, "@attributeFlags", column.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@columnText", column.ColumnText, DbType.String);

                return ExecuteAndGetLibraryColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "CreateLibraryColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "CreateLibraryColumnImpl"), ex);
            }
        }
        internal override VLLibraryColumn UpdateLibraryColumnImpl(Int32 accessToken, VLLibraryColumn column, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_librarycolumns_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@question", column.Question, DbType.Int32);
                AddParameter(command, "@columnId", column.ColumnId, DbType.Byte);
                AddParameter(command, "@displayOrder", column.DisplayOrder, DbType.Byte);
                AddParameter(command, "@attributeFlags", column.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", column.TextsLanguage, DbType.Int16);
                AddParameter(command, "@columnText", column.ColumnText, DbType.String);

                return ExecuteAndGetLibraryColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_LibrariesDao, "UpdateLibraryColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_LibrariesDao, "UpdateLibraryColumnImpll"), ex);
            }
        }
        
    }
}
