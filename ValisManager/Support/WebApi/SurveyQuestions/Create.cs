using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class Create : WebApiHandler
    {
        protected override void ProcessPostRequestWrapped(VLAccessToken accessToken, HttpContext context)
        {
            ProcessGetRequestWrapped(accessToken, context);
        }
        protected override void ProcessGetRequestWrapped(Valis.Core.VLAccessToken accessToken, HttpContext context)
        {
            try
            {
                var surveyId = TryParseInt32(context, "surveyId");
                var pageId = TryParseInt16(context, "pageId");
                InsertPosition position = (InsertPosition)TryParseInt16(context, "position");
                Int16? referingQuestionId = TryParseInt16(context, "referingQuestionId", false, null);
                var textsLanguage = TryParseInt16(context, "textsLanguage");
                var questionText = TryParseString(context, "questionText");
                var description = TryParseString(context, "description", false);
                var questionType = (QuestionType)Enum.Parse(typeof(QuestionType), TryParseString(context, "questionType", true));
                var isRequired = TryParseBoolean(context, "isRequired");
                var requiredMessage = TryParseString(context, "requiredMessage", false);

                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                VLSurveyQuestion question = null;
                if(referingQuestionId.HasValue)
                {
                    if(position == InsertPosition.Before)
                    {
                        question = surveyManager.CreateQuestionBefore(surveyId, pageId, referingQuestionId.Value, questionType, questionText, textsLanguage);
                    } 
                    else if(position == InsertPosition.After)
                    {
                        question = surveyManager.CreateQuestionAfter(surveyId, pageId, referingQuestionId.Value, questionType, questionText, textsLanguage);
                    } 
                    else
                    {
                        question = surveyManager.CreateQuestion(surveyId, pageId, questionType, questionText, textsLanguage);
                    }
                }
                else
                {
                    question = surveyManager.CreateQuestion(surveyId, pageId, questionType, questionText, textsLanguage);
                }

                //Κάνουμε update γενικά πεδία
                question.Description = description;
                question.IsRequired = isRequired;
                question.RequiredMessage = requiredMessage;

                try
                {
                    switch (question.QuestionType)
                    {
                        #region grab and update specific properties
                        case QuestionType.SingleLine:
                            {
                                question.ValidationBehavior = (ValidationMode)TryParseByte(context, "ValidationBehavior");
                                question.ValidationField1 = TryParseString(context, "ValidationField1", false);
                                question.ValidationField2 = TryParseString(context, "ValidationField2", false);
                                question.ValidationMessage = TryParseString(context, "ValidationMessage", false);

                            }
                            break;
                        case QuestionType.MultipleLine:
                            {

                            }
                            break;
                        case QuestionType.Integer:
                            {
                                question.ValidationBehavior = (ValidationMode)TryParseByte(context, "ValidationBehavior");
                                question.ValidationField1 = TryParseString(context, "ValidationField1", false);
                                question.ValidationField2 = TryParseString(context, "ValidationField2", false);
                                question.ValidationMessage = TryParseString(context, "ValidationMessage", false);
                            }
                            break;
                        case QuestionType.Decimal:
                            {
                                question.ValidationBehavior = (ValidationMode)TryParseByte(context, "ValidationBehavior");
                                question.ValidationField1 = TryParseString(context, "ValidationField1", false);
                                question.ValidationField2 = TryParseString(context, "ValidationField2", false);
                                question.ValidationMessage = TryParseString(context, "ValidationMessage", false);
                            }
                            break;
                        case QuestionType.Date:
                            {
                                question.ValidationBehavior = (ValidationMode)TryParseByte(context, "ValidationBehavior");
                                question.UseDateTimeControls = TryParseBoolean(context, "UseDateTimeControls");
                            }
                            break;
                        case QuestionType.Time:
                            {

                            }
                            break;
                        case QuestionType.DateTime:
                            {

                            }
                            break;
                        case QuestionType.OneFromMany:
                        case QuestionType.ManyFromMany:
                            {
                                question.RandomizeOptionsSequence = TryParseBoolean(context, "Randomize");
                                question.OptionalInputBox = TryParseBoolean(context, "AddOtherField");
                                question.OtherFieldLabel = TryParseString(context, "OtherFieldLabel", false);
                                question.OtherFieldType = (OtherFieldType)TryParseByte(context, "OtherFieldType", false);

                                question.ValidationBehavior = (ValidationMode)TryParseByte(context, "ValidationBehavior");
                                question.ValidationField1 = TryParseString(context, "ValidationField1", false);
                                question.ValidationField2 = TryParseString(context, "ValidationField2", false);
                                question.ValidationMessage = TryParseString(context, "ValidationMessage", false);

                                /*
                                 * Τώρα θα φτιάξουμε τα options της ερώτησης!
                                 */
                                var questionChoices = TryParseString(context, "QuestionChoices");
                                var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                if(_choices.Length == 0)
                                {
                                    throw new VLException("You must type at least one option for this type of question!");
                                }
                                foreach(var optionText in _choices)
                                {
                                    surveyManager.CreateQuestionOption(question, optionText);
                                }
                            }
                            break;
                        case QuestionType.DropDown:
                            {
                                question.RandomizeOptionsSequence = TryParseBoolean(context, "Randomize");

                                /*
                                 * Τώρα θα φτιάξουμε τα options της ερώτησης!
                                 */
                                var questionChoices = TryParseString(context, "QuestionChoices");
                                var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                if (_choices.Length == 0)
                                {
                                    throw new VLException("You must type at least one option for this type of question!");
                                }
                                foreach (var optionText in _choices)
                                {
                                    surveyManager.CreateQuestionOption(question, optionText);
                                }
                            }
                            break;
                        case QuestionType.DescriptiveText:
                            {

                            }
                            break;
                        case QuestionType.Slider:
                            {

                            }
                            break;
                        case QuestionType.Range:
                            {
                                question.FrontLabelText = TryParseString(context, "FrontLabelText", false);
                                question.AfterLabelText = TryParseString(context, "AfterLabelText", false);
                                question.RangeStart = TryParseInt32(context, "RangeStart");
                                question.RangeEnd = TryParseInt32(context, "RangeEnd");

                            }
                            break;
                        case QuestionType.MatrixOnePerRow:
                        case QuestionType.MatrixManyPerRow:
                            {
                                question.RandomizeOptionsSequence = TryParseBoolean(context, "Randomize");


                                /*
                                 * Τώρα θα φτιάξουμε τα options της ερώτησης!
                                 */
                                var questionChoices = TryParseString(context, "QuestionChoices");
                                var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                if (_choices.Length == 0)
                                {
                                    throw new VLException("You must type at least one option for this type of question!");
                                }
                                foreach (var optionText in _choices)
                                {
                                    surveyManager.CreateQuestionOption(question, optionText);
                                }

                                /*
                                 * Τώρα θα φτιάξουμε τις κολώνες της ερώτησης!
                                 */
                                var questionColumns = TryParseString(context, "QuestionColumns");
                                var _columns = questionColumns.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                                if (_columns.Length == 0)
                                {
                                    throw new VLException("You must type at least one column for this type of question!");
                                }
                                foreach (var columnText in _columns)
                                {
                                    surveyManager.CreateQuestionColumn(question.Survey, question.QuestionId, columnText);
                                }
                            }
                            break;
                        case QuestionType.MatrixManyPerRowCustom:
                            {

                            }
                            break;
                        case QuestionType.Composite:
                            {

                            }
                            break;
                        #endregion
                    }

                    question = surveyManager.UpdateQuestion(question);                  
                }
                catch
                {
                    /*Εάν κάτι δεν πάει καλά, διαγράφουμε την ερώτηση που πήγαμε να δημιουργήσουμε*/
                    surveyManager.DeleteQuestion(question);
                    throw;
                }




                var _question = new
                {
                    question.Survey,
                    question.QuestionId,
                    question.Page,
                    question.MasterQuestion,
                    question.DisplayOrder,
                    question.QuestionType,
                    question.CustomType,
                    question.IsRequired,
                    question.AttributeFlags,
                    question.ValidationBehavior,
                    question.RegularExpression,
                    question.TextsLanguage,

                    question.QuestionText,
                    question.Description,
                    question.HelpText,
                    question.FrontLabelText,
                    question.AfterLabelText,
                    question.InsideText,
                    question.RequiredMessage,
                    question.ValidationMessage
                };


                var response = JsonConvert.SerializeObject(_question, Formatting.None);
                context.Response.Write(response);
            }
            catch
            {
                throw;
            }
        }
    }
}