using System;
using System.Globalization;

namespace Valis.Core
{
    internal static class SR
    {
        #region
        internal static string GetString(string strString)
        {
            return strString;
        }
        internal static string GetString(string strString, int param1)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1);
        }
        internal static string GetString(string strString, string param1)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1);
        }
        internal static string GetString(string strString, Guid param1)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1);
        }
        internal static string GetString(string strString, string param1, string param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, Guid param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, Int32 param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, Int64 param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        internal static string GetString(string strString, string param1, string param2, string param3)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2, param3);
        }
        internal static string GetString(string strString, Guid param1, short param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        internal static string GetString(string strString, Int32 param1, short param2)
        {
            return string.Format(CultureInfo.InvariantCulture, strString, param1, param2);
        }
        #endregion


        internal const string Password_is_too_small = "The password must be at minumum {0} characters long!";
        internal const string Password_MinRequiredNonAlphanumericCharacters = "The password must contain at least {0} non alphanumeric characters!";
        internal const string Password_is_too_simple = "The password is too simple!";
        internal const string Password_is_invalid = "The password is invalid!";
        internal const string PasswordAnswer_is_invalid = "The passwordAnswer is invalid!";
        internal const string PasswordQuestion_is_invalid = "The passwordQuestion is invalid!";


        internal const string PageIndex_must_be_greatter_than_zero = "The '{0}' must be greater than zero (0).";
        internal const string PageSize_must_be_atleast_one = "The '{0}' must be one (1) or greatter.";
        internal const string StartRowIndex_must_be_greatter_than_zero = "The '{0}' must be greater than zero (0).";

        internal const string Parameter_can_not_be_empty = "The parameter '{0}' must not be empty.";
        internal const string Parameter_too_long = "The parameter '{0}' is too long: it must not exceed {1} chars in length.";
        internal const string Parameter_can_not_contain_comma = "The parameter '{0}' must not contain commas.";
        internal const string Parameter_array_empty = "The array parameter '{0}' should not be empty.";
        internal const string Parameter_duplicate_array_element = "The array '{0}' should not contain duplicate values.";

        internal const string Argument_is_invalid = "Argument '{0}' is invalid!";
        internal const string Argument_is_null_or_empty = "Argument '{0}' is null or empty string!";



        internal const string Invalid_accessToken_while_calling_SystemDao = "Invalid accessToken while calling SystemDao::{0}";
        internal const string Exception_occured_at_SystemDao = "Exception occured at SystemDao::{0}";
        internal const string Invalid_accessToken_while_calling_SurveyDao = "Invalid accessToken while calling SurveyDao::{0}";
        internal const string Exception_occured_at_SurveyDao = "Exception occured at SurveyDao::{0}";
        internal const string Invalid_accessToken_while_calling_FilesDao = "Invalid accessToken while calling FilesDao::{0}";
        internal const string Exception_occured_at_FilesDao = "Exception occured at FilesDao::{0}";
        internal const string Invalid_accessToken_while_calling_ViewModelDao = "Invalid accessToken while calling ViewModelDao::{0}";
        internal const string Exception_occured_at_ViewModelDao = "Exception occured at ViewModelDao::{0}";
        internal const string Invalid_accessToken_while_calling_SupportDao = "Invalid accessToken while calling SupportDao::{0}";
        internal const string Exception_occured_at_SupportDao = "Exception occured at SupportDao::{0}";
        internal const string Invalid_accessToken_while_calling_LibrariesDao = "Invalid accessToken while calling LibrariesDao::{0}";
        internal const string Exception_occured_at_LibrariesDao = "Exception occured at LibrariesDao::{0}";


        /// <summary>
        /// There is no {0} with id = '{1}'!
        /// </summary>
        internal const string There_is_no_item_with_id = "There is no {0} with id = '{1}'!";
        /// <summary>
        /// There is no {0} with name = '{1}'!
        /// </summary>
        internal const string There_is_no_item_with_name = "There is no {0} with name = '{1}'!";
        /// <summary>
        /// There is no {0} with key = '{1}'!
        /// </summary>
        internal const string There_is_no_item_with_key = "There is no {0} with key = '{1}'!";
        /// <summary>
        /// {0} '{1}' is already in use!
        /// </summary>
        internal const string Value_is_already_in_use = "{0} '{1}' is already in use!";
        /// <summary>
        /// You cannot set the '{0}' permission
        /// </summary>
        internal const string You_cannot_set_the_XXX_permission = "You cannot set the '{0}' permission!";
        /// <summary>
        /// You cannot delete a builtin '{0}'
        /// </summary>
        internal const string You_cannot_delete_builtin = "You cannot delete a builtin '{0}'";
        /// <summary>
        /// You cannot delete the builtin {0} '{1}'
        /// </summary>
        internal const string You_cannot_delete_the_builtin_entity = "You cannot delete the builtin {0} '{1}'";
        /// <summary>
        /// You cannot update the builtin {0} '{1}'
        /// </summary>
        internal const string You_cannot_update_the_builtin_entity = "You cannot update the builtin {0} '{1}'";
        /// <summary>
        /// You cannot update a builtin '{0}'
        /// </summary>
        internal const string You_cannot_update_builtin = "You cannot update a builtin '{0}'";
        /// <summary>
        /// 
        /// </summary>
        internal const string The_Name_is_in_use = "The name '{0}' is in use by another '{1}'";


        internal const string Roles_more_than_one = "There are more than one Roles with the same {0} '{1}'!";
        internal const string Surveys_more_than_one = "There are more than one Surveys with the same {0} '{1}'!";
        internal const string Themes_more_than_one = "There are more than one Themes with the same {0} '{1}'!";
        internal const string Clients_more_than_one = "There are more than one Clients with the same {0} '{1}'!";
        internal const string ClientProfiles_more_than_one = "There are more than one Profile with the same {0} '{1}'!";
        internal const string ClientLists_more_than_one = "There are more than one Client Lists with the same {0} '{1}'!";
        internal const string ListContacts_more_than_one = "There are more than one Contacts with the same {0} '{1}'!";
        internal const string Collectors_more_than_one = "There are more than one Collector with the same {0} '{1}'!";
        internal const string Messages_more_than_one = "There are more than one Messages with the same {0} '{1}'!";
        internal const string Recipients_more_than_one = "There are more than one Recipients with the same {0} '{1}'!";
        internal const string LibraryQuestionCategories_more_than_one = "There are more than one LibraryQuestionCategories with the same {0} '{1}'!";
        internal const string VerifiedEmails_more_than_one = "There are more than one VerifiedEmails with the same {0} '{1}'!";
        internal const string EmailTemplates_more_than_one = "There are more than one emailTemplates with the same {0} '{1}'!";

        internal static string GetString(string p1, string p2, VLLibraryQuestion libraryQuestion)
        {
            throw new NotImplementedException();
        }
    }
}
