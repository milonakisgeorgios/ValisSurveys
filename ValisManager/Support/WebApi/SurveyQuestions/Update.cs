using Newtonsoft.Json;
using System;
using System.Web;
using Valis.Core;

namespace ValisManager.Support.WebApi.SurveyQuestions
{
    public class Update : WebApiHandler
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
                var questionId = TryParseInt16(context, "questionId");
                var textsLanguage = TryParseInt16(context, "textsLanguage");
                var questionType = (QuestionType)Enum.Parse(typeof(QuestionType), TryParseString(context, "questionType", true));

                VLSurveyManager surveyManager = VLSurveyManager.GetAnInstance(accessToken);
                VLSurveyQuestion question = surveyManager.GetQuestionById(surveyId, questionId, textsLanguage);
                if (question == null)
                {
                    throw new VLException(string.Format("There is no SurveyQuestion with id='{0},{1}'.", surveyId, questionId));
                }
                if(question.QuestionType != questionType)
                {
                    throw new VLException(string.Format("You cannot update the questionType from {0} to {1}!", question.QuestionType.ToString(), questionType.ToString()));
                }
                question.QuestionText = TryParseString(context, "questionText");
                question.Description = TryParseString(context, "description", false);
                question.IsRequired = TryParseBoolean(context, "isRequired");
                question.RequiredMessage = TryParseString(context, "requiredMessage", false);



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
                             * Τώρα θα ασχοληθούμε με το update των options για αυτή την ερώτηση.
                             * Θα διαγράψουμε όλα τα παλιά options, και θα τα αντικαταστήσουμε με τα νέα
                             * που μας ήρθαν
                             */
                            var questionChoices = TryParseString(context, "QuestionChoices");
                            var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (_choices.Length == 0)
                            {
                                throw new VLException("You must type at least one option for this type of question!");
                            }
                            var _options = surveyManager.GetQuestionOptions(surveyId, questionId, textsLanguage);
                            Int32 index = 0;
                            for(; index < _options.Count; index++)
                            {
                                if (index < _choices.Length)
                                {
                                    if (String.CompareOrdinal(_options[index].OptionText, _choices[index]) != 0)
                                    {
                                        _options[index].OptionText = _choices[index];
                                        surveyManager.UpdateQuestionOption(_options[index]);
                                    }
                                }
                                else
                                {
                                    surveyManager.DeleteQuestionOption(_options[index]);
                                }
                            }
                            for(; index < _choices.Length; index++)
                            {
                                surveyManager.CreateQuestionOption(surveyId, questionId, _choices[index], textsLanguage: textsLanguage);
                            }
                            

                        }
                        break;
                    case QuestionType.DropDown:
                        {
                            question.RandomizeOptionsSequence = TryParseBoolean(context, "Randomize");

                            /*
                             * Τώρα θα ασχοληθούμε με το update των options για αυτή την ερώτηση.
                             * Θα διαγράψουμε όλα τα παλιά options, και θα τα αντικαταστήσουμε με τα νέα
                             * που μας ήρθαν
                             */
                            var questionChoices = TryParseString(context, "QuestionChoices");
                            var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (_choices.Length == 0)
                            {
                                throw new VLException("You must type at least one option for this type of question!");
                            }
                            var _options = surveyManager.GetQuestionOptions(surveyId, questionId, textsLanguage);
                            Int32 index = 0;
                            for (; index < _options.Count; index++)
                            {
                                if (index < _choices.Length)
                                {
                                    if (String.CompareOrdinal(_options[index].OptionText, _choices[index]) != 0)
                                    {
                                        _options[index].OptionText = _choices[index];
                                        surveyManager.UpdateQuestionOption(_options[index]);
                                    }
                                }
                                else
                                {
                                    surveyManager.DeleteQuestionOption(_options[index]);
                                }
                            }
                            for (; index < _choices.Length; index++)
                            {
                                surveyManager.CreateQuestionOption(surveyId, questionId, _choices[index], textsLanguage: textsLanguage);
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
                             * Τώρα θα ασχοληθούμε με το update των options για αυτή την ερώτηση.
                             * Θα διαγράψουμε όλα τα παλιά options, και θα τα αντικαταστήσουμε με τα νέα
                             * που μας ήρθαν
                             */
                            var questionChoices = TryParseString(context, "QuestionChoices");
                            var _choices = questionChoices.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (_choices.Length == 0)
                            {
                                throw new VLException("You must type at least one option for this type of question!");
                            }
                            var _options = surveyManager.GetQuestionOptions(surveyId, questionId, textsLanguage);
                            Int32 index = 0;
                            for (; index < _options.Count; index++)
                            {
                                if (index < _choices.Length)
                                {
                                    if (String.CompareOrdinal(_options[index].OptionText, _choices[index]) != 0)
                                    {
                                        _options[index].OptionText = _choices[index];
                                        surveyManager.UpdateQuestionOption(_options[index]);
                                    }
                                }
                                else
                                {
                                    surveyManager.DeleteQuestionOption(_options[index]);
                                }
                            }
                            for (; index < _choices.Length; index++)
                            {
                                surveyManager.CreateQuestionOption(surveyId, questionId, _choices[index], textsLanguage: textsLanguage);
                            }


                            /*
                             * Τώρα θα ασχοληθούμε με το update των κολωνών για αυτή την ερώτηση
                             */
                            var questionColumns = TryParseString(context, "QuestionColumns");
                            var _columns = questionColumns.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                            if (_columns.Length == 0)
                            {
                                throw new VLException("You must type at least one column for this type of question!");
                            }
                            var _headers = surveyManager.GetQuestionColumns(surveyId, questionId, textsLanguage);
                            index = 0;
                            for (; index < _headers.Count; index++)
                            {
                                if (index < _columns.Length)
                                {
                                    if (String.CompareOrdinal(_headers[index].ColumnText, _columns[index]) != 0)
                                    {
                                        _headers[index].ColumnText = _columns[index];
                                        surveyManager.UpdateQuestionColumn(_headers[index]);
                                    }
                                }
                                else
                                {
                                    surveyManager.DeleteQuestionColumn(_headers[index]);
                                }
                            }
                            for (; index < _columns.Length; index++)
                            {
                                surveyManager.CreateQuestionColumn(surveyId, questionId, _columns[index], textsLanguage: textsLanguage);
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