using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using Valis.Core.Dal;
using Valis.Core.ViewModel;

namespace Valis.Core.SqlServer
{
    internal class SurveysDao : SurveysDaoBase
    {
        #region VLRuntimeSession
        internal override Collection<VLRuntimeSession> GetRuntimeSessionsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("select [SessionId],[Survey],[RequestType],[Collector],[ResponseType],[RecipientKey],[RecipientIP],[UserAgent],[StartDt],[LastDt],[AttributeFlags],[SerializedData],[PagesStack], [CollectorPayment],[IsCharged] from [dbo].[RuntimeSessions] " + whereClause + orderByClause);
                command.CommandType = CommandType.Text;

                return ExecuteAndGetRuntimeSessions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
            }
        }
        internal override VLRuntimeSession GetRuntimeSessionByIdImpl(Int32 accessToken, Guid sessionId)
        {
            try
            {
                DbCommand command = CreateCommand("select [SessionId],[Survey],[RequestType],[Collector],[ResponseType],[RecipientKey],[RecipientIP],[UserAgent],[StartDt],[LastDt],[AttributeFlags],[SerializedData],[PagesStack], [CollectorPayment], [IsCharged] from [dbo].[RuntimeSessions] where [SessionId]=@SessionId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@SessionId", sessionId, DbType.Guid);

                return ExecuteAndGetRuntimeSession(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRuntimeSessionByIdImpl"), ex);
            }
        }
        internal override VLRuntimeSession CreateRuntimeSessionImpl(Int32 accessToken, VLRuntimeSession session, string userAgent, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_runtimesessions_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@sessionId", session.SessionId, DbType.Guid);
                AddParameter(command, "@survey", session.Survey, DbType.Int32);
                AddParameter(command, "@requestType", session.RequestType, DbType.Byte);
                AddParameter(command, "@collector", session.Collector, DbType.Int32);
                AddParameter(command, "@responseType", session.ResponseType, DbType.Byte);
                AddParameter(command, "@recipientKey", session.RecipientKey, DbType.AnsiString);
                AddParameter(command, "@recipientIP", session.RecipientIP, DbType.AnsiString);
                AddParameter(command, "@startDt", session.StartDt, DbType.DateTime2);
                AddParameter(command, "@lastDt", session.LastDt, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", session.AttributeFlags, DbType.Int16);
                AddParameter(command, "@serializedData", session.SerializedData, DbType.Binary);
                AddParameter(command, "@pagesStack", session.PagesStack, DbType.Binary);
                AddParameter(command, "@collectorPayment", session.CollectorPayment, DbType.Int32);
                AddParameter(command, "@isCharged", session.IsCharged, DbType.Boolean);
                AddParameter(command, "@agentString", userAgent, DbType.AnsiString);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetRuntimeSession(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateSessionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSessionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSessionImpl"), ex);
            }
        }
        internal override void UpdateRuntimeSessionImpl(Int32 accessToken, VLRuntimeSession session)
        {
            try
            {
                DbCommand command = CreateCommand("update [dbo].[RuntimeSessions] set Collector = @collector, RecipientKey = @recipientKey,RecipientIP = @recipientIP,UserAgent = @userAgent,LastDt = @lastDt,AttributeFlags = @attributeFlags,SerializedData = @serializedData, PagesStack = @pagesStack, CollectorPayment = @collectorPayment, IsCharged = @isCharged where [SessionId]=@SessionId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@collector", session.Collector, DbType.Int32);
                AddParameter(command, "@recipientKey", session.RecipientKey, DbType.AnsiString);
                AddParameter(command, "@recipientIP", session.RecipientIP, DbType.AnsiString);
                AddParameter(command, "@userAgent", session.UserAgent, DbType.Int32);
                AddParameter(command, "@lastDt", session.LastDt, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", session.AttributeFlags, DbType.Int16);
                AddParameter(command, "@serializedData", session.SerializedData, DbType.Binary);
                AddParameter(command, "@pagesStack", session.PagesStack, DbType.Binary);
                AddParameter(command, "@collectorPayment", session.CollectorPayment, DbType.Int32);
                AddParameter(command, "@isCharged", session.IsCharged, DbType.Boolean);
                AddParameter(command, "@sessionId", session.SessionId, DbType.Guid);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateSessionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSessionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSessionImpl"), ex);
            }
        }
        internal override void DeleteRuntimeSessionImpl(Int32 accessToken, Guid sessionId)
        {
            try
            {
                DbCommand command = CreateCommand("delete from [dbo].[RuntimeSessions] where [SessionId]=@SessionId");
                command.CommandType = CommandType.Text;
                AddParameter(command, "@SessionId", sessionId, DbType.Guid);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteSessionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSessionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSessionImpl"), ex);
            }
        }
        internal override void DeleteAllRuntimeSessionsImpl(Int32 accessToken)
        {
            try
            {
                DbCommand command = CreateCommand("delete from [dbo].[RuntimeSessions]");
                command.CommandType = CommandType.Text;


                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteAllSessionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllSessionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllSessionsImpl"), ex);
            }
        }
        internal override VLRuntimeSession ChargePaymentForClickImpl(Int32 accessToken, Guid sessionId, Int32 collectorId, Int32 surveyId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Charge_For_Click");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@sessionId", sessionId, DbType.Guid);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetRuntimeSession(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "ChargePaymentForResponseImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForResponseImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForResponseImpl"), ex);
            }
        }
        #endregion

        #region VLSurvey
        internal override Collection<VLSurvey> GetSurveysImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveys(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysImpl"), ex);
            }
        }
        internal override int GetSurveysCountImpl(Int32 accessToken, string whereClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysCountImpl"), ex);
            }
        }
        internal override Collection<VLSurvey> GetSurveysPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetSurveys(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysPagedImpl"), ex);
            }
        }

        internal override Collection<VLSurvey> GetSurveysForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetAll_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveys(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysForClientImpl"), ex);
            }
        }
        internal override int GetSurveysCountForClientImpl(Int32 accessToken, Int32 clientId, string whereClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetTotalRows_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysCountForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysCountForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysCountForClientImpl"), ex);
            }
        }
        internal override Collection<VLSurvey> GetSurveysPagedForClientImpl(Int32 accessToken, Int32 clientId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetPage_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetSurveys(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysPagedForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysPagedForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysPagedForClientImpl"), ex);
            }
        }



        internal override Collection<VLSurvey> GetSurveyVariantsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetVariants");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);

                return ExecuteAndGetSurveys(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyVariantsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyVariantsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyVariantsImpl"), ex);
            }
        }
        
        internal override VLSurvey GetSurveyByIdImpl(Int32 accessToken, Int32 surveyId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurvey(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveysByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveysByIdImpl"), ex);
            }
        }
        internal override void DeleteSurveyImpl(Int32 accessToken, Int32 surveyId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyImpl"), ex);
            }
        }
        internal override void DestroySurveyImpl(Int32 accessToken, Int32 surveyId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_Destroy");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DestroySurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DestroySurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DestroySurveyImpl"), ex);
            }
        }
        internal override VLSurvey CreateSurveyImpl(Int32 accessToken, VLSurvey survey, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", survey.Client, DbType.Int32);
                AddParameter(command, "@surveyId", survey.SurveyId, DbType.Int32);
                AddParameter(command, "@folder", survey.Folder, DbType.Int16);
                AddParameter(command, "@publicId", survey.PublicId, DbType.String);
                AddParameter(command, "@title", survey.Title, DbType.String);
                AddParameter(command, "@theme", survey.Theme, DbType.Int32);
                AddParameter(command, "@logo", survey.Logo, DbType.Int32);
                AddParameter(command, "@attributeFlags", survey.AttributeFlags, DbType.Int32);
                AddParameter(command, "@primaryLanguage", survey.PrimaryLanguage, DbType.Int16);
                AddParameter(command, "@supportedLanguagesIds", survey.SupportedLanguagesIds, DbType.AnsiString);
                AddParameter(command, "@questionNumberingType", survey.QuestionNumberingType, DbType.Byte);
                AddParameter(command, "@progressBarPosition", survey.ProgressBarPosition, DbType.Byte);
                AddParameter(command, "@requiredHighlightType", survey.RequiredHighlightType, DbType.Byte);
                AddParameter(command, "@customId", survey.CustomId, DbType.String);
                AddParameter(command, "@sourceSurvey", survey.SourceSurvey, DbType.Int32);
                AddParameter(command, "@templateSurvey", survey.TemplateSurvey, DbType.Int32);
                AddParameter(command, "@onCompletionMode", survey.OnCompletionMode, DbType.Byte);
                AddParameter(command, "@onDisqualificationMode", survey.OnDisqualificationMode, DbType.Byte);
                AddParameter(command, "@export1Name", survey.AllResponsesXlsxExportName, DbType.String);
                AddParameter(command, "@export1Path", survey.AllResponsesXlsxExportPath, DbType.String);
                AddParameter(command, "@export1CreationDt", survey.AllResponsesXlsxExportCreationDt, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@showTitle", survey.ShowTitle, DbType.String);
                AddParameter(command, "@headerHtml", survey.HeaderHtml, DbType.String);
                AddParameter(command, "@welcomeHtml", survey.WelcomeHtml, DbType.String);
                AddParameter(command, "@goodbyeHtml", survey.GoodbyeHtml, DbType.String);
                AddParameter(command, "@footerHtml", survey.FooterHtml, DbType.String);
                AddParameter(command, "@disqualificationHtml", survey.DisqualificationHtml, DbType.String);
                AddParameter(command, "@disqualificationUrl", survey.DisqualificationUrl, DbType.String);
                AddParameter(command, "@onCompletionUrl", survey.OnCompletionUrl, DbType.String);
                AddParameter(command, "@startButton", survey.StartButton, DbType.String);
                AddParameter(command, "@previousButton", survey.PreviousButton, DbType.String);
                AddParameter(command, "@nextButton", survey.NextButton, DbType.String);
                AddParameter(command, "@doneButton", survey.DoneButton, DbType.String);

                return ExecuteAndGetSurvey(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyImpl"), ex);
            }
        }
        internal override VLSurvey UpdateSurveyImpl(Int32 accessToken, VLSurvey survey, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", survey.Client, DbType.Int32);
                AddParameter(command, "@surveyId", survey.SurveyId, DbType.Int32);
                AddParameter(command, "@folder", survey.Folder, DbType.Int16);
                AddParameter(command, "@publicId", survey.PublicId, DbType.String);
                AddParameter(command, "@title", survey.Title, DbType.String);
                AddParameter(command, "@theme", survey.Theme, DbType.Int32);
                AddParameter(command, "@logo", survey.Logo, DbType.Int32);
                AddParameter(command, "@attributeFlags", survey.AttributeFlags, DbType.Int32);
                AddParameter(command, "@primaryLanguage", survey.PrimaryLanguage, DbType.Int16);
                AddParameter(command, "@supportedLanguagesIds", survey.SupportedLanguagesIds, DbType.AnsiString);
                AddParameter(command, "@questionNumberingType", survey.QuestionNumberingType, DbType.Byte);
                AddParameter(command, "@progressBarPosition", survey.ProgressBarPosition, DbType.Byte);
                AddParameter(command, "@requiredHighlightType", survey.RequiredHighlightType, DbType.Byte);
                AddParameter(command, "@customId", survey.CustomId, DbType.String);
                AddParameter(command, "@sourceSurvey", survey.SourceSurvey, DbType.Int32);
                AddParameter(command, "@templateSurvey", survey.TemplateSurvey, DbType.Int32);
                AddParameter(command, "@onCompletionMode", survey.OnCompletionMode, DbType.Byte);
                AddParameter(command, "@onDisqualificationMode", survey.OnDisqualificationMode, DbType.Byte);
                AddParameter(command, "@export1Name", survey.AllResponsesXlsxExportName, DbType.String);
                AddParameter(command, "@export1Path", survey.AllResponsesXlsxExportPath, DbType.String);
                AddParameter(command, "@export1CreationDt", survey.AllResponsesXlsxExportCreationDt, DbType.DateTime2);
                AddParameter(command, "@lastUpdateDT", survey.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", survey.TextsLanguage, DbType.Int16);
                AddParameter(command, "@showTitle", survey.ShowTitle, DbType.String);
                AddParameter(command, "@headerHtml", survey.HeaderHtml, DbType.String);
                AddParameter(command, "@welcomeHtml", survey.WelcomeHtml, DbType.String);
                AddParameter(command, "@goodbyeHtml", survey.GoodbyeHtml, DbType.String);
                AddParameter(command, "@footerHtml", survey.FooterHtml, DbType.String);
                AddParameter(command, "@disqualificationHtml", survey.DisqualificationHtml, DbType.String);
                AddParameter(command, "@disqualificationUrl", survey.DisqualificationUrl, DbType.String);
                AddParameter(command, "@onCompletionUrl", survey.OnCompletionUrl, DbType.String);
                AddParameter(command, "@startButton", survey.StartButton, DbType.String);
                AddParameter(command, "@previousButton", survey.PreviousButton, DbType.String);
                AddParameter(command, "@nextButton", survey.NextButton, DbType.String);
                AddParameter(command, "@doneButton", survey.DoneButton, DbType.String);

                return ExecuteAndGetSurvey(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyImpll"), ex);
            }
        }
        
        internal override VLSurvey AddSurveyLanguageImpl(Int32 accessToken, Int32 surveyId, Int16 sourceLanguage, Int16 targetLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_AddLanguage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@sourceLanguage", sourceLanguage, DbType.Int16);
                AddParameter(command, "@targetLanguage", targetLanguage, DbType.Int16);

                return ExecuteAndGetSurvey(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "AddSurveyLanguageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "AddSurveyLanguageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "AddSurveyLanguageImpl"), ex);
            }
        }
        internal override VLSurvey RemoveSurveyLanguageImpl(Int32 accessToken, Int32 surveyId, Int16 languageToBeDeleted)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveys_RemoveLanguage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@languageToBeDeleted", languageToBeDeleted, DbType.Int16);

                return ExecuteAndGetSurvey(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "RemoveSurveyLanguageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveSurveyLanguageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveSurveyLanguageImpl"), ex);
            }
        }
        #endregion

        #region VLSurveyTheme
        internal override Collection<VLSurveyTheme> GetSurveyThemesImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetSurveyThemes(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyThemesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesImpl"), ex);
            }
        }
        internal override int GetSurveyThemesCountImpl(Int32 accessToken, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyThemesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesCountImpl"), ex);
            }
        }
        internal override Collection<VLSurveyTheme> GetSurveyThemesPagedImpl(Int32 accessToken, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetSurveyThemes(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyThemesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesPagedImpl"), ex);
            }
        }
        internal override VLSurveyTheme GetSurveyThemeByIdImpl(Int32 accessToken, Int32 themeId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@themeId", themeId, DbType.Int32);


                return ExecuteAndGetSurveyTheme(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyThemesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyThemesByIdImpl"), ex);
            }
        }
        internal override void DeleteSurveyThemeImpl(Int32 accessToken, Int32 themeId,DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@themeId", themeId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteSurveyThemeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyThemeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyThemeImpl"), ex);
            }
        }
        internal override VLSurveyTheme CreateSurveyThemeImpl(Int32 accessToken, VLSurveyTheme theme, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@themeId", theme.ThemeId, DbType.Int32);
                AddParameter(command, "@clientId", theme.ClientId, DbType.Int32);
                AddParameter(command, "@name", theme.Name, DbType.String);
                AddParameter(command, "@rtHtml", theme.RtHtml, DbType.String);
                AddParameter(command, "@rtCSS", theme.RtCSS, DbType.String);
                AddParameter(command, "@dtHtml", theme.DtHtml, DbType.String);
                AddParameter(command, "@dtCSS", theme.DtCSS, DbType.String);
                AddParameter(command, "@attributeFlags", theme.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSurveyTheme(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateSurveyThemeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyThemeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyThemeImpl"), ex);
            }
        }
        internal override VLSurveyTheme UpdateSurveyThemeImpl(Int32 accessToken, VLSurveyTheme theme, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveythemes_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@themeId", theme.ThemeId, DbType.Int32);
                AddParameter(command, "@clientId", theme.ClientId, DbType.Int32);
                AddParameter(command, "@name", theme.Name, DbType.String);
                AddParameter(command, "@rtHtml", theme.RtHtml, DbType.String);
                AddParameter(command, "@rtCSS", theme.RtCSS, DbType.String);
                AddParameter(command, "@dtHtml", theme.DtHtml, DbType.String);
                AddParameter(command, "@dtCSS", theme.DtCSS, DbType.String);
                AddParameter(command, "@attributeFlags", theme.AttributeFlags, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", theme.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetSurveyTheme(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateSurveyThemeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyThemeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyThemeImpll"), ex);
            }
        }
        #endregion

        #region VLSurveyPage
        internal override Collection<VLSurveyPage> GetSurveyPagesImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyPages(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyPagesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesImpl"), ex);
            }
        }
        internal override int GetSurveyPagesCountImpl(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyPagesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesCountImpl"), ex);
            }
        }
        internal override Collection<VLSurveyPage> GetSurveyPagesPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetSurveyPages(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyPagesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesPagedImpl"), ex);
            }
        }
        internal override VLSurveyPage GetSurveyPageByIdImpl(Int32 accessToken, Int32 surveyId, Int16 pageId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@pageId", pageId, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyPage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyPagesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyPagesByIdImpl"), ex);
            }
        }
        internal override void DeleteSurveyPageImpl(Int32 accessToken, Int32 surveyId, Int16 pageId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@pageId", pageId, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteSurveyPageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyPageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyPageImpl"), ex);
            }
        }

        internal override VLSurveyPage CreateSurveyPageImpl(Int32 accessToken, VLSurveyPage surveyPage, short textsLanguage, InsertPosition position, Int16? referingPageId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyPage.Survey, DbType.Int32);
                AddParameter(command, "@displayOrder", surveyPage.DisplayOrder, DbType.Int16);
                AddParameter(command, "@previousPage", surveyPage.PreviousPage, DbType.Int32);
                AddParameter(command, "@nextPage", surveyPage.NextPage, DbType.Int32);
                AddParameter(command, "@attributeFlags", surveyPage.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", surveyPage.CustomId, DbType.String);
                AddParameter(command, "@skipTo", surveyPage.SkipTo, DbType.Byte);
                AddParameter(command, "@skipToPage", surveyPage.SkipToPage, DbType.Int16);
                AddParameter(command, "@skipToWebUrl", surveyPage.SkipToWebUrl, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@showTitle", surveyPage.ShowTitle, DbType.String);
                AddParameter(command, "@description", surveyPage.Description, DbType.String);
                AddParameter(command, "@position", (Int16)position, DbType.Int16);
                AddParameter(command, "@referingPageId", referingPageId, DbType.Int16);

                return ExecuteAndGetSurveyPage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateSurveyPageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyPageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyPageImpl"), ex);
            }
        }

        internal override VLSurveyPage UpdateSurveyPageImpl(Int32 accessToken, VLSurveyPage surveyPage, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveypages_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyPage.Survey, DbType.Int32);
                AddParameter(command, "@pageId", surveyPage.PageId, DbType.Int16);
                AddParameter(command, "@displayOrder", surveyPage.DisplayOrder, DbType.Int16);
                AddParameter(command, "@previousPage", surveyPage.PreviousPage, DbType.Int32);
                AddParameter(command, "@nextPage", surveyPage.NextPage, DbType.Int32);
                AddParameter(command, "@attributeFlags", surveyPage.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", surveyPage.CustomId, DbType.String);
                AddParameter(command, "@skipTo", surveyPage.SkipTo, DbType.Byte);
                AddParameter(command, "@skipToPage", surveyPage.SkipToPage, DbType.Int16);
                AddParameter(command, "@skipToWebUrl", surveyPage.SkipToWebUrl, DbType.String);
                AddParameter(command, "@lastUpdateDT", surveyPage.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", surveyPage.TextsLanguage, DbType.Int16);
                AddParameter(command, "@showTitle", surveyPage.ShowTitle, DbType.String);
                AddParameter(command, "@description", surveyPage.Description, DbType.String);

                return ExecuteAndGetSurveyPage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateSurveyPageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyPageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyPageImpll"), ex);
            }
        }
        #endregion

        #region VLSurveyQuestion
        internal override Collection<VLSurveyQuestion> GetQuestionsForSurveyImpl(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyQuestionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsImpl"), ex);
            }
        }
        internal override Collection<VLSurveyQuestion> GetQuestionsForSurveyImpl(Int32 accessToken, Int32 survey, Int16 page, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetAll_ForPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@page", page, DbType.Int16);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyQuestionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsImpl"), ex);
            }
        }

        internal override Collection<VLSurveyQuestion> GetChildQuestionsImpl(Int32 accessToken, Int32 survey, Int16 masterQuestion, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetChild_ForMaster");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@masterQuestion", masterQuestion, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetChildQuestionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetChildQuestionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetChildQuestionsImpl"), ex);
            }
        }
        internal override int GetQuestionsForSurveyCountImpl(Int32 accessToken, Int32 survey)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
            }
        }
        internal override int GetQuestionsForSurveyCountImpl(Int32 accessToken, Int32 survey, Int16 page)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetTotalRows_ForPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@page", page, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsCountImpl"), ex);
            }
        }
        internal override int GetQuestionsForLibraryQuestionCountImpl(Int32 accessToken, Int32 libraryQuestion)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetTotalRows_ForLibraryQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@libraryQuestion", libraryQuestion, DbType.Int32);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionsForLibraryQuestionCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionsForLibraryQuestionCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionsForLibraryQuestionCountImpl"), ex);
            }
        }
        internal override VLSurveyQuestion GetQuestionByIdImpl(Int32 accessToken, Int32 survey, Int16 questionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@questionId", questionId, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSurveyQuestionsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSurveyQuestionsByIdImpl"), ex);
            }
        }
        internal override void DeleteQuestionImpl(Int32 accessToken, Int32 survey, Int16 questionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@questionId", questionId, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteSurveyQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteSurveyQuestionImpl"), ex);
            }
        }

        internal override VLSurveyQuestion CreateSurveyQuestionImpl(Int32 accessToken, VLSurveyQuestion question, short textsLanguage, InsertPosition position, Int16? referingQuestionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", question.Survey, DbType.Int32);
                AddParameter(command, "@page", question.Page, DbType.Int16);
                AddParameter(command, "@masterQuestion", question.MasterQuestion, DbType.Int16);
                AddParameter(command, "@displayOrder", question.DisplayOrder, DbType.Int16);
                AddParameter(command, "@questionType", question.QuestionType, DbType.Byte);
                AddParameter(command, "@customType", question.CustomType, DbType.Byte);
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
                AddParameter(command, "@rangeStart", question.RangeStart, DbType.Int32);
                AddParameter(command, "@rangeEnd", question.RangeEnd, DbType.Int32);
                AddParameter(command, "@libraryQuestion", question.LibraryQuestion, DbType.Int32);
                AddParameter(command, "@customId", question.CustomId, DbType.String);
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
                AddParameter(command, "@position", (Int16)position, DbType.Int16);
                AddParameter(command, "@referingQuestionId", referingQuestionId, DbType.Int16);

                return ExecuteAndGetSurveyQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateSurveyQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateSurveyQuestionImpl"), ex);
            }
        }
        internal override VLSurveyQuestion UpdateSurveyQuestionImpl(Int32 accessToken, VLSurveyQuestion question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", question.Survey, DbType.Int32);
                AddParameter(command, "@questionId", question.QuestionId, DbType.Int16);
                AddParameter(command, "@page", question.Page, DbType.Int16);
                //AddParameter(command, "@masterQuestion", question.MasterQuestion, DbType.Int16);
                //AddParameter(command, "@displayOrder", question.DisplayOrder, DbType.Int16);
                //AddParameter(command, "@questionType", question.QuestionType, DbType.Byte);
                AddParameter(command, "@customType", question.CustomType, DbType.Byte);
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
                AddParameter(command, "@rangeStart", question.RangeStart, DbType.Int32);
                AddParameter(command, "@rangeEnd", question.RangeEnd, DbType.Int32);
                AddParameter(command, "@libraryQuestion", question.LibraryQuestion, DbType.Int32);
                AddParameter(command, "@customId", question.CustomId, DbType.String);
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

                return ExecuteAndGetSurveyQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateSurveyQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateSurveyQuestionImpll"), ex);
            }
        }

        internal override VLSurveyQuestion AddLibraryQuestionImpl(int accessToken, int surveyId, short pageId, int libraryQuestionId, short textsLanguage, InsertPosition position, short? referingQuestionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_AddLibraryQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyId, DbType.Int32);
                AddParameter(command, "@page", pageId, DbType.Int16);
                AddParameter(command, "@libraryQuestion", libraryQuestionId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@position", (Int16)position, DbType.Int16);
                AddParameter(command, "@referingQuestionId", referingQuestionId, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetSurveyQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "AddLibraryQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "AddLibraryQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "AddLibraryQuestionImpl"), ex);
            }
        }

        internal override Collection<VLSurveyQuestionEx> GetQuestionExsForSurveyImpl(Int32 accessToken, Int32 survey, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyquestions_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetSurveyQuestionExs(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionExsForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionExsForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionExsForSurveyImpl"), ex);
            }
        }
        #endregion

        #region VLQuestionOption
        /// <summary>
        /// Επιστρέφει όλα τα Options (ανεξαρτήτως ερώτησης) που ανήκουν στο συγκεκριμένο ερωτηματολόγιο
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        internal override Collection<VLQuestionOption> GetQuestionOptionsExImpl(Int32 accessToken, Int32 survey, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetQuestionOptions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionOptionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsImpl"), ex);
            }
        }
        /// <summary>
        /// Επιστρέφει τα options της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        internal override Collection<VLQuestionOption> GetQuestionOptionsImpl(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetQuestionOptions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionOptionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsImpl"), ex);
            }
        }
        internal override int GetQuestionOptionsCountImpl(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionOptionsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsCountImpl"), ex);
            }
        }
        internal override VLQuestionOption GetQuestionOptionByIdImpl(Int32 accessToken, Int32 survey, Int16 question, Byte optionId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@optionId", optionId, DbType.Byte);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetQuestionOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionOptionsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionOptionsByIdImpl"), ex);
            }
        }
        internal override void DeleteQuestionOptionImpl(Int32 accessToken, Int32 survey, Int16 question, Byte optionId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@optionId", optionId, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteQuestionOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteQuestionOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteQuestionOptionImpl"), ex);
            }
        }
        internal override void DeleteAllQuestionOptionsImpl(Int32 accessToken, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_DeleteAllForQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteAllQuestionOptionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllQuestionOptionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllQuestionOptionsImpl"), ex);
            }
        }
        internal override VLQuestionOption CreateQuestionOptionImpl(Int32 accessToken, VLQuestionOption option, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", option.Survey, DbType.Int32);
                AddParameter(command, "@question", option.Question, DbType.Int16);
                AddParameter(command, "@optionType", option.OptionType, DbType.Byte);
                AddParameter(command, "@displayOrder", option.DisplayOrder, DbType.Int16);
                AddParameter(command, "@optionValue", option.OptionValue, DbType.Int16);
                AddParameter(command, "@attributeFlags", option.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", option.CustomId, DbType.String);
                AddParameter(command, "@skipTo", option.SkipTo, DbType.Byte);
                AddParameter(command, "@skipToPage", option.SkipToPage, DbType.Int16);
                AddParameter(command, "@skipToQuestion", option.SkipToQuestion, DbType.Int16);
                AddParameter(command, "@skipToWebUrl", option.SkipToWebUrl, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@optionText", option.OptionText, DbType.String);

                return ExecuteAndGetQuestionOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateQuestionOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateQuestionOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateQuestionOptionImpl"), ex);
            }
        }
        internal override VLQuestionOption UpdateQuestionOptionImpl(Int32 accessToken, VLQuestionOption option, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveyoptions_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", option.Survey, DbType.Int32);
                AddParameter(command, "@question", option.Question, DbType.Int16);
                AddParameter(command, "@optionId", option.OptionId, DbType.Byte);
                AddParameter(command, "@optionType", option.OptionType, DbType.Byte);
                AddParameter(command, "@displayOrder", option.DisplayOrder, DbType.Int16);
                AddParameter(command, "@optionValue", option.OptionValue, DbType.Int16);
                AddParameter(command, "@attributeFlags", option.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", option.CustomId, DbType.String);
                AddParameter(command, "@skipTo", option.SkipTo, DbType.Byte);
                AddParameter(command, "@skipToPage", option.SkipToPage, DbType.Int16);
                AddParameter(command, "@skipToQuestion", option.SkipToQuestion, DbType.Int16);
                AddParameter(command, "@skipToWebUrl", option.SkipToWebUrl, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", option.TextsLanguage, DbType.Int16);
                AddParameter(command, "@optionText", option.OptionText, DbType.String);

                return ExecuteAndGetQuestionOption(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateQuestionOptionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateQuestionOptionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateQuestionOptionImpll"), ex);
            }
        }
        
        #endregion

        #region VLQuestionColumn
        /// <summary>
        /// Επιστρέφει τα columns της συγκεκριμένης ερώτησης, πάντα ταξονομημένα ως προς το DisplayOrder τους.
        /// <para>Η ταξινόμηση αυτή είναι πολύ σημαντική! (Χρησιμοποιείται για την διαδικασία της μετάφρασης)</para>
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="question"></param>
        /// <param name="textsLanguage"></param>
        /// <returns></returns>
        internal override Collection<VLQuestionColumn> GetQuestionColumnsImpl(Int32 accessToken, Int32 survey, Int16 question, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetQuestionColumns(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionColumnsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsImpl"), ex);
            }
        }
        internal override int GetQuestionColumnsCountImpl(Int32 accessToken, Int32 survey, Int16 question)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionColumnsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsCountImpl"), ex);
            }
        }
        internal override VLQuestionColumn GetQuestionColumnByIdImpl(Int32 accessToken, Int32 survey, Int16 question, Byte columnId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@columnId", columnId, DbType.Byte);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetQuestionColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetQuestionColumnsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetQuestionColumnsByIdImpl"), ex);
            }
        }
        internal override void DeleteQuestionColumnImpl(Int32 accessToken, Int32 survey, Int16 question, Byte columnId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@columnId", columnId, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteQuestionColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteQuestionColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteQuestionColumnImpl"), ex);
            }
        }

        internal override void DeleteAllQuestionColumnsImpl(Int32 accessToken, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_DeleteAllForQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteAllQuestionColumnsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllQuestionColumnsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllQuestionColumnsImpl"), ex);
            }
        }
        internal override VLQuestionColumn CreateQuestionColumnImpl(Int32 accessToken, VLQuestionColumn column, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", column.Survey, DbType.Int32);
                AddParameter(command, "@question", column.Question, DbType.Int16);
                AddParameter(command, "@displayOrder", column.DisplayOrder, DbType.Byte);
                AddParameter(command, "@attributeFlags", column.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", column.CustomId, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@columnText", column.ColumnText, DbType.String);

                return ExecuteAndGetQuestionColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateQuestionColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateQuestionColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateQuestionColumnImpl"), ex);
            }
        }
        internal override VLQuestionColumn UpdateQuestionColumnImpl(Int32 accessToken, VLQuestionColumn column, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycolumns_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", column.Survey, DbType.Int32);
                AddParameter(command, "@question", column.Question, DbType.Int16);
                AddParameter(command, "@columnId", column.ColumnId, DbType.Byte);
                AddParameter(command, "@displayOrder", column.DisplayOrder, DbType.Byte);
                AddParameter(command, "@attributeFlags", column.AttributeFlags, DbType.Int32);
                AddParameter(command, "@customId", column.CustomId, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", column.TextsLanguage, DbType.Int16);
                AddParameter(command, "@columnText", column.ColumnText, DbType.String);

                return ExecuteAndGetQuestionColumn(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateQuestionColumnImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateQuestionColumnImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateQuestionColumnImpll"), ex);
            }
        }
        
        #endregion

        #region VLCollector
        internal override Collection<VLCollector> GetCollectorsImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetCollectors(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsImpl"), ex);
            }
        }
        internal override Collection<VLCollector> GetCollectorsImpl(Int32 accessToken, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetCollectors(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsImpl"), ex);
            }
        }
        internal override int GetCollectorsCountImpl(Int32 accessToken, Int32 surveyId, string whereClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsCountImpl"), ex);
            }
        }
        internal override int GetCollectorsCountForClientImpl(Int32 accessToken, Int32 clientId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetTotalRows_ForClient");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", clientId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsCountForClientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsCountForClientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsCountForClientImpl"), ex);
            }
        }
        
        internal override Collection<VLCollector> GetCollectorsPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", surveyId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                var collection = ExecuteAndGetCollectors(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsPagedImpl"), ex);
            }
        }
        internal override Collection<VLCollector> GetCollectorVariantsImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetVariants");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);

                return ExecuteAndGetCollectors(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorVariantsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorVariantsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorVariantsImpl"), ex);
            }
        }
        internal override VLCollector GetCollectorByIdImpl(Int32 accessToken, Int32 collectorId, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);

                return ExecuteAndGetCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorsByIdImpl"), ex);
            }
        }
        internal override void DeleteCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteCollectorImpl"), ex);
            }
        }
        internal override VLCollector CreateCollectorImpl(Int32 accessToken, VLCollector collector, DateTime currentTimeUtc, short textsLanguage)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", collector.Survey, DbType.Int32);
                AddParameter(command, "@collectorType", collector.CollectorType, DbType.Byte);
                AddParameter(command, "@name", collector.Name, DbType.String);
                AddParameter(command, "@attributeFlags", collector.AttributeFlags, DbType.Int32);
                AddParameter(command, "@status", collector.Status, DbType.Byte);
                AddParameter(command, "@responses", collector.Responses, DbType.Int32);
                AddParameter(command, "@webLink", collector.WebLink, DbType.String);
                AddParameter(command, "@editResponseMode", collector.EditResponseMode, DbType.Byte);
                AddParameter(command, "@onCompletionMode", collector.OnCompletionMode, DbType.Byte);
                AddParameter(command, "@stopCollectorDT", collector.StopCollectorDT, DbType.DateTime2);
                AddParameter(command, "@maxResponses", collector.MaxResponses, DbType.Int32);
                AddParameter(command, "@password", collector.Password, DbType.String);
                AddParameter(command, "@webSurveyType", collector.WebSurveyType, DbType.Byte);
                AddParameter(command, "@webWidth", collector.WebWidth, DbType.Int16);
                AddParameter(command, "@webHeight", collector.WebHeight, DbType.Int16);
                AddParameter(command, "@webPopupEvery", collector.WebPopupEvery, DbType.Int32);
                AddParameter(command, "@webFontColor", collector.WebFontColor, DbType.AnsiString);
                AddParameter(command, "@webBackgroundColor", collector.WebBackgroundColor, DbType.AnsiString);
                AddParameter(command, "@creditType", collector.CreditType, DbType.Byte);
                AddParameter(command, "@firstChargeDt", collector.FirstChargeDt, DbType.DateTime2);
                AddParameter(command, "@lastChargeDt", collector.LastChargeDt, DbType.DateTime2);
                AddParameter(command, "@profile", collector.Profile, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", textsLanguage, DbType.Int16);
                AddParameter(command, "@onCompletionURL", collector.OnCompletionURL, DbType.AnsiString);
                AddParameter(command, "@disqualificationPageURL", collector.DisqualificationPageURL, DbType.String);
                AddParameter(command, "@passwordLabel", collector.PasswordLabel, DbType.String);
                AddParameter(command, "@submitButtonLabel", collector.SubmitButtonLabel, DbType.String);
                AddParameter(command, "@passwordRequiredMessage", collector.PasswordRequiredMessage, DbType.String);
                AddParameter(command, "@passwordFailedMessage", collector.PasswordFailedMessage, DbType.String);
                AddParameter(command, "@webInvitation", collector.WebInvitation, DbType.String);
                AddParameter(command, "@webNeverButton", collector.WebNeverButton, DbType.String);
                AddParameter(command, "@webLaterButton", collector.WebLaterButton, DbType.String);
                AddParameter(command, "@webNowButton", collector.WebNowButton, DbType.String);
                AddParameter(command, "@closedMessage", collector.ClosedMessage, DbType.String);

                return ExecuteAndGetCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorImpl"), ex);
            }
        }
        internal override VLCollector UpdateCollectorImpl(Int32 accessToken, VLCollector collector, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_surveycollectors_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collector.CollectorId, DbType.Int32);
                AddParameter(command, "@survey", collector.Survey, DbType.Int32);
                AddParameter(command, "@collectorType", collector.CollectorType, DbType.Byte);
                AddParameter(command, "@name", collector.Name, DbType.String);
                AddParameter(command, "@attributeFlags", collector.AttributeFlags, DbType.Int32);
                AddParameter(command, "@status", collector.Status, DbType.Byte);
                AddParameter(command, "@responses", collector.Responses, DbType.Int32);
                AddParameter(command, "@webLink", collector.WebLink, DbType.String);
                AddParameter(command, "@editResponseMode", collector.EditResponseMode, DbType.Byte);
                AddParameter(command, "@onCompletionMode", collector.OnCompletionMode, DbType.Byte);
                AddParameter(command, "@stopCollectorDT", collector.StopCollectorDT, DbType.DateTime2);
                AddParameter(command, "@maxResponses", collector.MaxResponses, DbType.Int32);
                AddParameter(command, "@password", collector.Password, DbType.String);
                AddParameter(command, "@webSurveyType", collector.WebSurveyType, DbType.Byte);
                AddParameter(command, "@webWidth", collector.WebWidth, DbType.Int16);
                AddParameter(command, "@webHeight", collector.WebHeight, DbType.Int16);
                AddParameter(command, "@webPopupEvery", collector.WebPopupEvery, DbType.Int32);
                AddParameter(command, "@webFontColor", collector.WebFontColor, DbType.AnsiString);
                AddParameter(command, "@webBackgroundColor", collector.WebBackgroundColor, DbType.AnsiString);
                AddParameter(command, "@creditType", collector.CreditType, DbType.Byte);
                AddParameter(command, "@firstChargeDt", collector.FirstChargeDt, DbType.DateTime2);
                AddParameter(command, "@lastChargeDt", collector.LastChargeDt, DbType.DateTime2);
                AddParameter(command, "@profile", collector.Profile, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", collector.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);
                AddParameter(command, "@textsLanguage", collector.TextsLanguage, DbType.Int16);
                AddParameter(command, "@onCompletionURL", collector.OnCompletionURL, DbType.AnsiString);
                AddParameter(command, "@disqualificationPageURL", collector.DisqualificationPageURL, DbType.String);
                AddParameter(command, "@passwordLabel", collector.PasswordLabel, DbType.String);
                AddParameter(command, "@submitButtonLabel", collector.SubmitButtonLabel, DbType.String);
                AddParameter(command, "@passwordRequiredMessage", collector.PasswordRequiredMessage, DbType.String);
                AddParameter(command, "@passwordFailedMessage", collector.PasswordFailedMessage, DbType.String);
                AddParameter(command, "@webInvitation", collector.WebInvitation, DbType.String);
                AddParameter(command, "@webNeverButton", collector.WebNeverButton, DbType.String);
                AddParameter(command, "@webLaterButton", collector.WebLaterButton, DbType.String);
                AddParameter(command, "@webNowButton", collector.WebNowButton, DbType.String);
                AddParameter(command, "@closedMessage", collector.ClosedMessage, DbType.String);

                return ExecuteAndGetCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorImpll"), ex);
            }
        }



        internal override Collection<VLCollectorPeek> GetCollectorPeeksImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select s.SurveyId, c.CollectorId, c.Name as CollectorName, s.Title as SurveyName, (select count(*) from [dbo].[Recipients] where [Collector]=c.CollectorId) as TotalRecipients, c.CollectorType from [dbo].[SurveyCollectors] as c inner join [dbo].[Surveys] as s on c.Survey = s.SurveyId {0} {1}", whereClause, orderByClause));
                command.CommandType = CommandType.Text;


                return ExecuteAndGetCollectorPeeks(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorPeeksImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorPeeksImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorPeeksImpl"), ex);
            }
        }
        #endregion

        #region VLMessage
        internal override Collection<VLMessage> GetMessagesExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);

                return ExecuteAndGetMessages(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessagesExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessagesExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessagesExImpl"), ex);
            }
        }
        internal override Collection<VLMessage> GetMessagesImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);

                return ExecuteAndGetMessages(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorMessagesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesImpl"), ex);
            }
        }
        internal override int GetMessagesCountImpl(Int32 accessToken, Int32 collectorId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorMessagesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesCountImpl"), ex);
            }
        }
        internal override Collection<VLMessage> GetMessagesPagedImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);

                var collection = ExecuteAndGetMessages(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorMessagesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesPagedImpl"), ex);
            }
        }
        internal override VLMessage GetMessageByIdImpl(Int32 accessToken, Int32 messageId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@messageId", messageId, DbType.Int32);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetCollectorMessagesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetCollectorMessagesByIdImpl"), ex);
            }
        }
        internal override void DeleteMessageImpl(Int32 accessToken, Int32 messageId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@messageId", messageId, DbType.Int32);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteCollectorMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteCollectorMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteCollectorMessageImpl"), ex);
            }
        }
        internal override VLMessage CreateMessageImpl(Int32 accessToken, VLMessage message, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", message.Collector, DbType.Int32);
                AddParameter(command, "@sender", message.Sender, DbType.String);
                AddParameter(command, "@subject", message.Subject, DbType.String);
                AddParameter(command, "@body", message.Body, DbType.String);
                AddParameter(command, "@status", message.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", message.AttributeFlags, DbType.Int32);
                AddParameter(command, "@scheduledAt", message.ScheduledAt, DbType.DateTime2);
                AddParameter(command, "@sentCounter", message.SentCounter, DbType.Int32);
                AddParameter(command, "@failedCounter", message.FailedCounter, DbType.Int32);
                AddParameter(command, "@skipCounter", message.SkipCounter, DbType.Int32);
                AddParameter(command, "@deliveryMethod", message.DeliveryMethod, DbType.Byte);
                AddParameter(command, "@customSearchField", message.CustomSearchField, DbType.Byte);
                AddParameter(command, "@customOperator", message.CustomOperator, DbType.Byte);
                AddParameter(command, "@customKeyword", message.CustomKeyword, DbType.String);
                AddParameter(command, "@pendingAt", message.PendingAt, DbType.DateTime2);
                AddParameter(command, "@preparingAt", message.PreparingAt, DbType.DateTime2);
                AddParameter(command, "@preparedAt", message.PreparedAt, DbType.DateTime2);
                AddParameter(command, "@executingAt", message.ExecutingAt, DbType.DateTime2);
                AddParameter(command, "@terminatedAt", message.TerminatedAt, DbType.DateTime2);
                AddParameter(command, "@error", message.Error, DbType.String);
                AddParameter(command, "@senderVerificationCode", message.SenderVerificationCode, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateCollectorMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorMessageImpl"), ex);
            }
        }
        internal override VLMessage UpdateMessageImpl(Int32 accessToken, VLMessage message, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", message.Collector, DbType.Int32);
                AddParameter(command, "@messageId", message.MessageId, DbType.Int32);
                AddParameter(command, "@sender", message.Sender, DbType.String);
                AddParameter(command, "@subject", message.Subject, DbType.String);
                AddParameter(command, "@body", message.Body, DbType.String);
                AddParameter(command, "@status", message.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", message.AttributeFlags, DbType.Int32);
                AddParameter(command, "@scheduledAt", message.ScheduledAt, DbType.DateTime2);
                AddParameter(command, "@sentCounter", message.SentCounter, DbType.Int32);
                AddParameter(command, "@failedCounter", message.FailedCounter, DbType.Int32);
                AddParameter(command, "@skipCounter", message.SkipCounter, DbType.Int32);
                AddParameter(command, "@deliveryMethod", message.DeliveryMethod, DbType.Byte);
                AddParameter(command, "@customSearchField", message.CustomSearchField, DbType.Byte);
                AddParameter(command, "@customOperator", message.CustomOperator, DbType.Byte);
                AddParameter(command, "@customKeyword", message.CustomKeyword, DbType.String);
                AddParameter(command, "@pendingAt", message.PendingAt, DbType.DateTime2);
                AddParameter(command, "@preparingAt", message.PreparingAt, DbType.DateTime2);
                AddParameter(command, "@preparedAt", message.PreparedAt, DbType.DateTime2);
                AddParameter(command, "@executingAt", message.ExecutingAt, DbType.DateTime2);
                AddParameter(command, "@terminatedAt", message.TerminatedAt, DbType.DateTime2);
                AddParameter(command, "@error", message.Error, DbType.String);
                AddParameter(command, "@senderVerificationCode", message.SenderVerificationCode, DbType.String);
                AddParameter(command, "@lastUpdateDT", message.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateCollectorMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorMessageImpll"), ex);
            }
        }
        internal override VLMessage UnScheduleMessageImpl(Int32 accessToken, VLMessage message, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_UnSchedule");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", message.Collector, DbType.Int32);
                AddParameter(command, "@messageId", message.MessageId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UnScheduleMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UnScheduleMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UnScheduleMessageImpl"), ex);
            }
        }

        internal override VLMessage GetNextPendingMessageImpl(Int32 accessToken, DateTime scheduleDt, int minuteOffset, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetNextPending");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@scheduleDt", scheduleDt, DbType.DateTime2);
                AddParameter(command, "@minuteOffset", minuteOffset, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime2);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetNextPendingMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetNextPendingMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetNextPendingMessageImpl"), ex);
            }
        }

        internal override VLMessage GetNextPreparedMessageImpl(Int32 accessToken, DateTime scheduleDt, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messages_GetNextPrepared");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@scheduleDt", scheduleDt, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime2);

                return ExecuteAndGetMessage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetNextPreparedMessageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetNextPreparedMessageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetNextPreparedMessageImpl"), ex);
            }
        }
        #endregion

        #region VLRecipient
        internal override Collection<VLRecipient> GetRecipientsImpl(Int32 accessToken, Int32 collectorId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetRecipients(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRecipientsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsImpl"), ex);
            }
        }
        internal override Collection<VLRecipient> GetRecipientsExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetRecipients(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRecipientsExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsExImpl"), ex);
            }
        }
        internal override int GetRecipientsCountImpl(Int32 accessToken, Int32 collectorId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRecipientsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsCountImpl"), ex);
            }
        }
        internal override Collection<VLRecipient> GetRecipientsPagedImpl(Int32 accessToken, Int32 collectorId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetRecipients(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRecipientsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientsPagedImpl"), ex);
            }
        }
        internal override VLRecipient GetRecipientByIdImpl(Int32 accessToken, Int64 recipientId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@recipientId", recipientId, DbType.Int64);


                return ExecuteAndGetrRecipient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetRecipientByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetRecipientByIdImpl"), ex);
            }
        }
        internal override void DeleteRecipientImpl(Int32 accessToken, Int64 recipientId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@recipientId", recipientId, DbType.Int64);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteRecipientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteRecipientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteRecipientImpl"), ex);
            }
        }
        internal override int RemoveAllUnsentRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Remove_AllUnsent");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "RemoveAllRecipientsFromCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllRecipientsFromCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllRecipientsFromCollectorImpl"), ex);
            }
        }
        internal override int RemoveAllOptedOutRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Remove_AllOptedOut");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "RemoveAllOptedOutRecipientsFromCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllOptedOutRecipientsFromCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllOptedOutRecipientsFromCollectorImpl"), ex);
            }
        }
        internal override int RemoveAllBouncedRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Remove_AllBounced");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "RemoveAllBouncedRecipientsFromCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllBouncedRecipientsFromCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveAllBouncedRecipientsFromCollectorImpl"), ex);
            }
        }
        internal override int RemoveByDomainRecipientsFromCollectorImpl(Int32 accessToken, Int32 collectorId, string domainName, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Remove_AllByDomain");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@domainName", domainName, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return Convert.ToInt32(ExecuteScalar(command));
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "RemoveByDomainRecipientsFromCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveByDomainRecipientsFromCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "RemoveByDomainRecipientsFromCollectorImpl"), ex);
            }
        }
        
        
        internal override VLRecipient CreateRecipientImpl(Int32 accessToken, VLRecipient recipient, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collector", recipient.Collector, DbType.Int32);
                AddParameter(command, "@recipientKey", recipient.RecipientKey, DbType.String);
                AddParameter(command, "@email", recipient.Email, DbType.String);
                AddParameter(command, "@firstName", recipient.FirstName, DbType.String);
                AddParameter(command, "@lastName", recipient.LastName, DbType.String);
                AddParameter(command, "@title", recipient.Title, DbType.String);
                AddParameter(command, "@customValue", recipient.CustomValue, DbType.String);
                AddParameter(command, "@status", recipient.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", recipient.AttributeFlags, DbType.Int32);
                AddParameter(command, "@personalPassword", recipient.PersonalPassword, DbType.String);
                AddParameter(command, "@activationDate", recipient.ActivationDate, DbType.DateTime2);
                AddParameter(command, "@validFromDate", recipient.ValidFromDate, DbType.DateTime2);
                AddParameter(command, "@validToDate", recipient.ValidToDate, DbType.DateTime2);
                AddParameter(command, "@expireAfter", recipient.ExpireAfter, DbType.Int16);
                AddParameter(command, "@expirationDate", recipient.ExpirationDate, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetrRecipient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateCollectorRecipientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorRecipientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateCollectorRecipientImpl"), ex);
            }
        }
        internal override VLRecipient UpdateRecipientImpl(Int32 accessToken, VLRecipient recipient, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@recipientId", recipient.RecipientId, DbType.Int64);
                AddParameter(command, "@recipientKey", recipient.RecipientKey, DbType.String);
                AddParameter(command, "@email", recipient.Email, DbType.String);
                AddParameter(command, "@firstName", recipient.FirstName, DbType.String);
                AddParameter(command, "@lastName", recipient.LastName, DbType.String);
                AddParameter(command, "@title", recipient.Title, DbType.String);
                AddParameter(command, "@customValue", recipient.CustomValue, DbType.String);
                AddParameter(command, "@status", recipient.Status, DbType.Byte);
                AddParameter(command, "@attributeFlags", recipient.AttributeFlags, DbType.Int32);
                AddParameter(command, "@personalPassword", recipient.PersonalPassword, DbType.String);
                AddParameter(command, "@activationDate", recipient.ActivationDate, DbType.DateTime2);
                AddParameter(command, "@validFromDate", recipient.ValidFromDate, DbType.DateTime2);
                AddParameter(command, "@validToDate", recipient.ValidToDate, DbType.DateTime2);
                AddParameter(command, "@expireAfter", recipient.ExpireAfter, DbType.Int16);
                AddParameter(command, "@expirationDate", recipient.ExpirationDate, DbType.DateTime2);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetrRecipient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateCollectorRecipientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorRecipientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateCollectorRecipientImpll"), ex);
            }
        }

        internal override void ImportRecipientsImpl(Int32 callerPrincipalId, VLRecipient[] recipient, int length, DateTime currentTimeUtc, ref Int32 successImports, ref Int32 failedImports)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Import");
                AddParameter(command, "@callerPrincipalId", callerPrincipalId, DbType.Int32);
                AddParameter(command, "@collector", recipient[0].Collector, DbType.Int32);
                AddParameter(command, "@totalRecords", length, DbType.Int32);

                Int32 totalRecords = length;
                for (int index = 0; index < length; index++)
                {
                    var suffix = (index+1).ToString();

                    AddParameter(command, "@key" + suffix, recipient[index].RecipientKey, DbType.String);
                    AddParameter(command, "@email" + suffix, recipient[index].Email, DbType.String);
                    AddParameter(command, "@firstName" + suffix, recipient[index].FirstName, DbType.String);
                    AddParameter(command, "@lastName" + suffix, recipient[index].LastName, DbType.String);
                    AddParameter(command, "@title" + suffix, recipient[index].Title, DbType.String);
                    AddParameter(command, "@customValue" + suffix, recipient[index].CustomValue, DbType.String);
                    AddParameter(command, "@status" + suffix, recipient[index].Status, DbType.Byte);
                    AddParameter(command, "@attributeFlags" + suffix, recipient[index].AttributeFlags, DbType.Int32);
                }
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                successImports = Convert.ToInt32(ExecuteScalar(command));
                failedImports = totalRecords - successImports;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "ImportRecipientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ImportRecipientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ImportRecipientImpl"), ex);
            }
        }

        internal override void ImportRecipientsFinalizeImpl(Int32 callerPrincipalId, Int32 collectorId, DateTime currentTimeUtc, ref Int32 optedOutRecipients, ref Int32 bouncedRecipients, ref Int32 totalRecipients)
        {
            try
            {
                DbCommand command = CreateCommand("valis_recipients_Import_Finalize");
                AddParameter(command, "@callerPrincipalId", callerPrincipalId, DbType.Int32);
                AddParameter(command, "@collector", collectorId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                try
                {
                    command.Connection.Open();
                    using (DbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows == false)
                            return;
                        reader.Read();

                        optedOutRecipients = reader.GetInt32(0);
                        bouncedRecipients = reader.GetInt32(1);
                        totalRecipients = reader.GetInt32(2);
                    }
                }
                finally
                {
                    command.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "ImportRecipientFinalizeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ImportRecipientFinalizeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ImportRecipientFinalizeImpl"), ex);
            }
        }
        
        
        #endregion

        #region VLMessageRecipient


        internal override Collection<VLMessageRecipient> GetMessageRecipientsImpl(Int32 accessToken, Int32 message, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@messageId", message, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetMessageRecipients(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessageRecipientsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsImpl"), ex);
            }
        }
        internal override int GetMessageRecipientsCountImpl(Int32 accessToken, Int32 message, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@messageId", message, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessageRecipientsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsCountImpl"), ex);
            }
        }
        internal override Collection<VLMessageRecipient> GetMessageRecipientsPagedImpl(Int32 accessToken, Int32 message, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@messageId", message, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetMessageRecipients(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessageRecipientsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsPagedImpl"), ex);
            }
        }
        internal override Collection<VLMessageRecipient> GetMessageRecipientsExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetMessageRecipients(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessageRecipientsExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsExImpl"), ex);
            }
        }
        internal override VLMessageRecipient GetMessageRecipientByIdImpl(Int32 accessToken, Int32 message, Int64 recipient)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@message", message, DbType.Int32);
                AddParameter(command, "@recipient", recipient, DbType.Int64);


                return ExecuteAndGetMessageRecipient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetMessageRecipientsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetMessageRecipientsByIdImpl"), ex);
            }
        }
        internal override VLMessageRecipient UpdateMessageRecipientImpl(Int32 accessToken, VLMessageRecipient messageRecipient, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@message", messageRecipient.Message, DbType.Int32);
                AddParameter(command, "@recipient", messageRecipient.Recipient, DbType.Int64);
                AddParameter(command, "@attributeFlags", messageRecipient.AttributeFlags, DbType.Int16);
                AddParameter(command, "@errorCount", messageRecipient.ErrorCount, DbType.Int16);
                AddParameter(command, "@sendDT", messageRecipient.SendDT, DbType.DateTime2);
                AddParameter(command, "@status", messageRecipient.Status, DbType.Byte);
                AddParameter(command, "@collectorPayment", messageRecipient.CollectorPayment, DbType.Int32);
                AddParameter(command, "@isCharged", messageRecipient.IsCharged, DbType.Boolean);
                AddParameter(command, "@error", messageRecipient.Error, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetMessageRecipient(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateMessageRecipientImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateMessageRecipientImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateMessageRecipientImpll"), ex);
            }
        }
        



        internal override Int32 PrepareMessageRecipientsImpl(Int32 accessToken, Int32 collectorId, Int32 messageId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_prepare_for_message");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@messageId", messageId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "PrepareMessageRecipientsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "PrepareMessageRecipientsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "PrepareMessageRecipientsImpl"), ex);
            }
        }

        internal override void UnPrepareMessageRecipientsImpl(Int32 accessToken, Int32 collectorId, Int32 messageId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_messagerecipients_unprepare_for_message");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@messageId", messageId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UnPrepareMessageRecipientsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UnPrepareMessageRecipientsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UnPrepareMessageRecipientsImpl"), ex);
            }
        }
        
        #endregion

        #region VLResponse

        internal override Collection<VLResponse> GetPaidResponsesImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_paid_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetResponses(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetPaidResponsesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesImpl"), ex);
            }
        }
        internal override int GetPaidResponsesCountImpl(Int32 accessToken, Int32 survey, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_paid_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetPaidResponsesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesCountImpl"), ex);
            }
        }
        internal override Collection<VLResponse> GetPaidResponsesPagedImpl(Int32 accessToken, Int32 survey, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_paid_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetResponses(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetPaidResponsesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponsesPagedImpl"), ex);
            }
        }


        internal override Collection<VLResponse> GetResponsesImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetResponses(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponsesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesImpl"), ex);
            }
        }
        internal override Collection<VLResponse> GetResponsesExImpl(Int32 accessToken, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetResponses(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponsesExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesExImpl"), ex);
            }
        }
        internal override int GetResponsesCountImpl(Int32 accessToken, Int32 survey, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponsesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesCountImpl"), ex);
            }
        }
        internal override Collection<VLResponse> GetResponsesPagedImpl(Int32 accessToken, Int32 survey, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", accessToken, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetResponses(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponsesPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponsesPagedImpl"), ex);
            }
        }
        internal override VLResponse GetResponseByIdImpl(Int32 accessToken, Int64 responseId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@responseId", responseId, DbType.Int64);


                return ExecuteAndGetResponse(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponseByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseByIdImpl"), ex);
            }
        }
        /// <summary>
        /// Διαγράφει το συγκεκριμένο Response απο το σύστημα και ταυτόχρονα κάνει update τα αντίστοιχα πεδία στον collector και στο Survey!
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="responseId"></param>
        /// <param name="currentTimeUtc"></param>
        internal override void DeleteResponseImpl(Int32 accessToken, Int64 responseId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@responseId", responseId, DbType.Int64);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteResponseImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseImpl"), ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="survey"></param>
        /// <param name="currentTimeUtc"></param>
        internal override void DeleteAllResponsesForSurveyImpl(Int32 accessToken, Int32 survey, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_DeleteAll_ForSurvey");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteAllResponsesForSurveyImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllResponsesForSurveyImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllResponsesForSurveyImpl"), ex);
            }
        }
        internal override VLCollector DeleteAllResponsesForCollectorImpl(Int32 accessToken, Int32 survey, Int32 collector, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_DeleteAll_ForCollector");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@collector", collector, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteAllResponsesForCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllResponsesForCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteAllResponsesForCollectorImpl"), ex);
            }
        }
        /// <summary>
        /// Δημιουργεί ένα νέο response στο σύστημα και παράλληλα ενημερώνει τα αντίστοιχα πεδία στον Collector και στο Survey!
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="response"></param>
        /// <param name="currentTimeUtc"></param>
        /// <returns></returns>
        internal override VLResponse CreateResponseImpl(Int32 accessToken, VLResponse response, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", response.Survey, DbType.Int32);
                AddParameter(command, "@collector", response.Collector, DbType.Int32);
                AddParameter(command, "@responseType", response.ResponseType, DbType.Byte);
                AddParameter(command, "@recipient", response.Recipient, DbType.Int64);
                AddParameter(command, "@recipientIP", response.RecipientIP, DbType.AnsiString);
                AddParameter(command, "@userAgent", response.UserAgent, DbType.Int32);
                AddParameter(command, "@openDate", response.OpenDate, DbType.DateTime2);
                AddParameter(command, "@closeDate", response.CloseDate, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", response.AttributeFlags, DbType.Int32);
                AddParameter(command, "@mustBeCharged", response.MustBeCharged, DbType.Boolean);
                AddParameter(command, "@creditType", response.CreditType, DbType.Byte);
                AddParameter(command, "@collectorPayment", response.CollectorPayment, DbType.Int32);
                AddParameter(command, "@isCharged", response.IsCharged, DbType.Boolean);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetResponse(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateResponseImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateResponseImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateResponseImpl"), ex);
            }
        }
        internal override VLResponse UpdateResponseImpl(Int32 accessToken, VLResponse answer, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@responseId", answer.ResponseId, DbType.Int64);
                AddParameter(command, "@survey", answer.Survey, DbType.Int32);
                AddParameter(command, "@collector", answer.Collector, DbType.Int32);
                AddParameter(command, "@responseType", answer.ResponseType, DbType.Byte);
                AddParameter(command, "@recipient", answer.Recipient, DbType.Int64);
                AddParameter(command, "@recipientIP", answer.RecipientIP, DbType.AnsiString);
                AddParameter(command, "@userAgent", answer.UserAgent, DbType.Int32);
                AddParameter(command, "@openDate", answer.OpenDate, DbType.DateTime2);
                AddParameter(command, "@closeDate", answer.CloseDate, DbType.DateTime2);
                AddParameter(command, "@attributeFlags", answer.AttributeFlags, DbType.Int32);
                AddParameter(command, "@mustBeCharged", answer.MustBeCharged, DbType.Boolean);
                AddParameter(command, "@creditType", answer.CreditType, DbType.Byte);
                AddParameter(command, "@collectorPayment", answer.CollectorPayment, DbType.Int32);
                AddParameter(command, "@isCharged", answer.IsCharged, DbType.Boolean);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetResponse(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateResponseImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateResponseImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateResponseImpl"), ex);
            }
        }

        internal override VLResponse ChargePaymentForResponseImpl(Int32 accessToken, Int64 responseId, Int32 collectorId, Int32 surveyId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_payments_Charge_For_Response");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@responseId", responseId, DbType.Int64);
                AddParameter(command, "@collectorId", collectorId, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetResponse(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SystemDao, "ChargePaymentForResponseImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForResponseImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SystemDao, "ChargePaymentForResponseImpl"), ex);
            }
        }


        internal override Collection<VLResponseEx> GetPaidResponseExsImpl(Int32 accessToken, Int32 survey, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responses_paid_GetAllEx");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetResponseExs(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetPaidResponseExsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponseExsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetPaidResponseExsImpl"), ex);
            }
        }
        #endregion


        #region VLResponseDetail
        internal override Collection<VLResponseDetail> GetResponseDetailsImpl(Int32 accessToken, Int64 response)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", response, DbType.Int64);


                return ExecuteAndGetResponseDetails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponseDetailsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailsImpl"), ex);
            }
        }
        internal override Collection<VLResponseDetail> GetResponseDetailsImpl(Int32 accessToken, Int64 response, Int16 question)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_GetAll_forQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", response, DbType.Int64);
                AddParameter(command, "@question", question, DbType.Int16);


                return ExecuteAndGetResponseDetails(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponseDetailsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailsImpl"), ex);
            }
        }
        internal override VLResponseDetail GetResponseDetailByIdImpl(Int32 accessToken, Int64 response, Int16 question, byte selectedOption, byte selectedColumn)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", response, DbType.Int64);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@selectedOption", selectedOption, DbType.Byte);
                AddParameter(command, "@selectedColumn", selectedColumn, DbType.Byte);


                return ExecuteAndGetResponseDetail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetResponseDetailByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetResponseDetailByIdImpl"), ex);
            }
        }
        internal override void DeleteResponseDetailImpl(Int32 accessToken, Int64 response, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", response, DbType.Int64);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteResponseDetailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseDetailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseDetailImpl"), ex);
            }
        }
        internal override void DeleteResponseDetailImpl(Int32 accessToken, Int64 response, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_Delete_forQuestion");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", response, DbType.Int64);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteResponseDetailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseDetailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteResponseDetailImpl"), ex);
            }
        }
        internal override VLResponseDetail CreateResponseDetailImpl(Int32 accessToken, VLResponseDetail answer, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", answer.Response, DbType.Int64);
                AddParameter(command, "@question", answer.Question, DbType.Int16);
                AddParameter(command, "@selectedOption", answer.SelectedOption, DbType.Byte);
                AddParameter(command, "@selectedColumn", answer.SelectedColumn, DbType.Byte);
                AddParameter(command, "@userInput", answer.UserInput, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetResponseDetail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateResponseDetailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateResponseDetailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateResponseDetailImpl"), ex);
            }
        }
        internal override VLResponseDetail UpdateResponseDetailImpl(Int32 accessToken, VLResponseDetail answer, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_responsedetails_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@response", answer.Response, DbType.Int64);
                AddParameter(command, "@question", answer.Question, DbType.Int16);
                AddParameter(command, "@selectedOption", answer.SelectedOption, DbType.Byte);
                AddParameter(command, "@selectedColumn", answer.SelectedColumn, DbType.Byte);
                AddParameter(command, "@userInput", answer.UserInput, DbType.String);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetResponseDetail(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateResponseDetailImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateResponseDetailImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateResponseDetailImpl"), ex);
            }
        }
        #endregion

        #region VLView
        internal override Collection<VLView> GetViewsImpl(Int32 accessToken, Int32 surveyId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);


                return ExecuteAndGetViews(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsImpl"), ex);
            }
        }
        internal override int GetViewsCountImpl(Int32 accessToken, Int32 surveyId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsCountImpl"), ex);
            }
        }
        internal override Collection<VLView> GetViewsPagedImpl(Int32 accessToken, Int32 surveyId, int startRowIndex, int maximumRows, ref int totalRows, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);
                AddParameter(command, "@whereClause", whereClause == null ? string.Empty : whereClause, DbType.String);
                AddParameter(command, "@orderBy", orderByClause == null ? string.Empty : orderByClause, DbType.String);
                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetViews(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewsPagedImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsPagedImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsPagedImpl"), ex);
            }
        }
        internal override VLView GetViewByIdImpl(Int32 accessToken, Guid viewId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);


                return ExecuteAndGetView(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewsByIdImpl"), ex);
            }
        }
        internal override void DeleteViewImpl(Int32 accessToken, Guid viewId, DateTime _lastUpdateDT, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@lastUpdateDT", _lastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteViewImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewImpl"), ex);
            }
        }
        internal override VLView CreateViewImpl(Int32 accessToken, VLView view, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", view.Client, DbType.Int32);
                AddParameter(command, "@userId", view.UserId, DbType.Int32);
                AddParameter(command, "@survey", view.Survey, DbType.Int32);
                AddParameter(command, "@viewId", view.ViewId, DbType.Guid);
                AddParameter(command, "@name", view.Name, DbType.String);
                AddParameter(command, "@isDefaultView", view.IsDefaultView, DbType.Boolean);
                AddParameter(command, "@attributeFlags", view.AttributeFlags, DbType.Int32);
                AddParameter(command, "@timePeriodStart", view.TimePeriodStart, DbType.DateTime2);
                AddParameter(command, "@timePeriodEnd", view.TimePeriodEnd, DbType.DateTime2);
                AddParameter(command, "@totalResponseTime", view.TotalResponseTime, DbType.Int32);
                AddParameter(command, "@totalResponseTimeUnit", view.TotalResponseTimeUnit, DbType.Byte);
                AddParameter(command, "@totalResponseTimeOperator", view.TotalResponseTimeOperator, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetView(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateViewImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateViewImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateViewImpl"), ex);
            }
        }
        internal override VLView UpdateViewImpl(Int32 accessToken, VLView view, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_views_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@client", view.Client, DbType.Int32);
                AddParameter(command, "@userId", view.UserId, DbType.Int32);
                AddParameter(command, "@survey", view.Survey, DbType.Int32);
                AddParameter(command, "@viewId", view.ViewId, DbType.Guid);
                AddParameter(command, "@name", view.Name, DbType.String);
                AddParameter(command, "@isDefaultView", view.IsDefaultView, DbType.Boolean);
                AddParameter(command, "@attributeFlags", view.AttributeFlags, DbType.Int32);
                AddParameter(command, "@timePeriodStart", view.TimePeriodStart, DbType.DateTime2);
                AddParameter(command, "@timePeriodEnd", view.TimePeriodEnd, DbType.DateTime2);
                AddParameter(command, "@totalResponseTime", view.TotalResponseTime, DbType.Int32);
                AddParameter(command, "@totalResponseTimeUnit", view.TotalResponseTimeUnit, DbType.Byte);
                AddParameter(command, "@totalResponseTimeOperator", view.TotalResponseTimeOperator, DbType.Byte);
                AddParameter(command, "@filtersVersion", view.FiltersVersion, DbType.Int32);
                AddParameter(command, "@summaryDesignVersion", view.SummaryDesignVersion, DbType.Int32);
                AddParameter(command, "@summaryRecordedResponses", view.SummaryRecordedResponses, DbType.Int32);
                AddParameter(command, "@summaryFiltersVersion", view.SummaryFiltersVersion, DbType.Int32);
                AddParameter(command, "@sumaryGenerationDt", view.SumaryGenerationDt, DbType.DateTime2);
                AddParameter(command, "@summaryVisibleResponses", view.SummaryVisibleResponses, DbType.Int32);
                AddParameter(command, "@summaryFilteredResponses", view.SummaryFilteredResponses, DbType.Int32);
                AddParameter(command, "@pdfReportIsValid", view.PdfReportIsValid, DbType.Boolean);
                AddParameter(command, "@pdfReportName", view.PdfReportName, DbType.String);
                AddParameter(command, "@pdfReportPath", view.PdfReportPath, DbType.String);
                AddParameter(command, "@pdfReportSize", view.PdfReportSize, DbType.Int32);
                AddParameter(command, "@pdfReportCreationDt", view.PdfReportCreationDt, DbType.DateTime2);
                AddParameter(command, "@lastUpdateDT", view.LastUpdateDT, DbType.DateTime);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetView(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewImpll"), ex);
            }
        }
        #endregion

        #region VLViewPage
        internal override Collection<VLViewPage> GetViewPagesImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select ViewId, Survey, Page, ShowResponses, AttributeFlags from (select * from dbo.ViewPages where ViewId=@viewId) as a {0} {1}", whereClause, orderByClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;

                return ExecuteAndGetViewPages(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewPagesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesImpl"), ex);
            }
        }
        internal override int GetViewPagesCountImpl(Int32 accessToken, Guid viewId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select count(*) from (select * from dbo.ViewPages where ViewId=@viewId) as a {0}", whereClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewPagesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesCountImpl"), ex);
            }
        }
        internal override VLViewPage GetViewPageByIdImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 page)
        {
            try
            {
                DbCommand command = CreateCommand("select ViewId, Survey, Page, ShowResponses, AttributeFlags from dbo.ViewPages where ViewId = @viewId and Survey = @survey and Page = @page");
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@page", page, DbType.Int16);
                command.CommandType = CommandType.Text;


                return ExecuteAndGetViewPage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewPagesByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewPagesByIdImpl"), ex);
            }
        }
        internal override VLViewPage UpdateViewPageImpl(Int32 accessToken, VLViewPage obj, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewpages_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", obj.ViewId, DbType.Guid);
                AddParameter(command, "@survey", obj.Survey, DbType.Int32);
                AddParameter(command, "@page", obj.Page, DbType.Int16);
                AddParameter(command, "@showResponses", obj.ShowResponses, DbType.Boolean);
                AddParameter(command, "@attributeFlags", obj.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetViewPage(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewPageImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewPageImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewPageImpll"), ex);
            }
        }
        #endregion

        #region VLViewQuestion
        internal override Collection<VLViewQuestion> GetViewQuestionsImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select ViewId, Survey, Question, ShowResponses, AttributeFlags, ChartType, LabelType, AxisScale, ScaleMaxPercentage, ScaleMaxAbsolute, SummaryTotalAnswered, SummaryTotalSkipped from (select * from dbo.ViewQuestions where ViewId=@viewId) as a {0} {1}", whereClause, orderByClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;


                return ExecuteAndGetViewQuestions(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewQuestionsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsImpl"), ex);
            }
        }
        internal override int GetViewQuestionsCountImpl(Int32 accessToken, Guid viewId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select count(*) from (select * from dbo.ViewQuestions where ViewId=@viewId) as a {0}", whereClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewQuestionsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsCountImpl"), ex);
            }
        }
        internal override VLViewQuestion GetViewQuestionByIdImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question)
        {
            try
            {
                DbCommand command = CreateCommand("select ViewId, Survey, Question, ShowResponses, AttributeFlags, ChartType, LabelType, AxisScale, ScaleMaxPercentage, ScaleMaxAbsolute, SummaryTotalAnswered, SummaryTotalSkipped from dbo.ViewQuestions where ViewId = @viewId and Survey = @survey and Question = @question");
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                command.CommandType = CommandType.Text;

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewQuestionsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewQuestionsByIdImpl"), ex);
            }
        }
        internal override VLViewQuestion UpdateViewQuestionImpl(Int32 accessToken, VLViewQuestion obj, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", obj.ViewId, DbType.Guid);
                AddParameter(command, "@survey", obj.Survey, DbType.Int32);
                AddParameter(command, "@question", obj.Question, DbType.Int16);
                AddParameter(command, "@showResponses", obj.ShowResponses, DbType.Boolean);
                AddParameter(command, "@attributeFlags", obj.AttributeFlags, DbType.Int32);
                AddParameter(command, "@chartType", obj.ChartType, DbType.Byte);
                AddParameter(command, "@labelType", obj.LabelType, DbType.Byte);
                AddParameter(command, "@axisScale", obj.AxisScale, DbType.Byte);
                AddParameter(command, "@scaleMaxPercentage", obj.ScaleMaxPercentage, DbType.Decimal);
                AddParameter(command, "@scaleMaxAbsolute", obj.ScaleMaxAbsolute, DbType.Decimal);
                AddParameter(command, "@summaryTotalAnswered", obj.SummaryTotalAnswered, DbType.Int32);
                AddParameter(command, "@summaryTotalSkipped", obj.SummaryTotalSkipped, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewQuestionImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewQuestionImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewQuestionImpll"), ex);
            }
        }

        internal override VLViewQuestion SetChartTypeImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, ChartType chartType, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_SetChartType");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@chartType", chartType, DbType.Byte);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "SetChartTypeImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "SetChartTypeImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "SetChartTypeImpl"), ex);
            }
        }
        internal override VLViewQuestion SwitchAxisScaleImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_SwitchAxisScale");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "SwitchAxisScaleImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "SwitchAxisScaleImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "SwitchAxisScaleImpl"), ex);
            }
        }
        internal override VLViewQuestion ToggleChartVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_ToggleChartVisibility");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "ToggleChartVisibilityImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleChartVisibilityImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleChartVisibilityImpl"), ex);
            }
        }
        internal override VLViewQuestion ToggleDataTableVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_ToggleDataTableVisibility");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "ToggleDataTableVisibilityImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleDataTableVisibilityImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleDataTableVisibilityImpl"), ex);
            }
        }
        internal override VLViewQuestion ToggleZeroResponseOptionsVisibilityImpl(Int32 accessToken, Guid viewId, Int32 survey, Int16 question, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewquestions_ToggleZeroResponseOptionsVisibility");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@survey", survey, DbType.Int32);
                AddParameter(command, "@question", question, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                return ExecuteAndGetViewQuestion(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "ToggleZeroResponseOptionsVisibilityImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleZeroResponseOptionsVisibilityImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "ToggleZeroResponseOptionsVisibilityImpl"), ex);
            }
        }
        #endregion

        #region VLViewCollector
        internal override Collection<VLViewCollector> GetViewCollectorsImpl(Int32 accessToken, Guid viewId, string whereClause, string orderByClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select ViewId, Collector, Survey, CollectorType, CollectorName, IncludeResponses, AttributeFlags from (select * from dbo.ViewCollectors where ViewId=@viewId) as a {0} {1}", whereClause, orderByClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;


                return ExecuteAndGetViewCollectors(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewCollectorsImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsImpl"), ex);
            }
        }
        internal override int GetViewCollectorsCountImpl(Int32 accessToken, Guid viewId, string whereClause)
        {
            try
            {
                DbCommand command = CreateCommand(string.Format("select count(*) from (select * from dbo.ViewCollectors where ViewId=@viewId) as a {0}", whereClause));
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewCollectorsCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsCountImpl"), ex);
            }
        }
        internal override VLViewCollector GetViewCollectorByIdImpl(Int32 accessToken, Guid viewId, Int32 collector)
        {
            try
            {
                DbCommand command = CreateCommand("select ViewId, Collector, Survey, CollectorType, CollectorName, IncludeResponses, AttributeFlags from dbo.ViewCollectors where ViewId = @viewId and Collector = @collector");
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@collector", collector, DbType.Int32);
                command.CommandType = CommandType.Text;

                return ExecuteAndGetViewCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewCollectorsByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewCollectorsByIdImpl"), ex);
            }
        }
        internal override VLViewCollector UpdateViewCollectorImpl(Int32 accessToken, VLViewCollector obj, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewcollectors_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", obj.ViewId, DbType.Guid);
                AddParameter(command, "@collector", obj.Collector, DbType.Int32);
                AddParameter(command, "@survey", obj.Survey, DbType.Int32);
                AddParameter(command, "@collectorType", obj.CollectorType, DbType.Byte);
                AddParameter(command, "@collectorName", obj.CollectorName, DbType.String);
                AddParameter(command, "@includeResponses", obj.IncludeResponses, DbType.Boolean);
                AddParameter(command, "@attributeFlags", obj.AttributeFlags, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetViewCollector(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewCollectorImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewCollectorImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewCollectorImpll"), ex);
            }
        }
        #endregion

        #region VLViewFilter
        internal override Collection<VLViewFilter> GetViewFiltersImpl(Int32 accessToken, Guid viewId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);


                return ExecuteAndGetViewFilters(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewFiltersImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewFiltersImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetFiltersImpl"), ex);
            }
        }
        internal override int GetViewFiltersCountImpl(Int32 accessToken, Guid viewId)
        {
            try
            {
                DbCommand command = CreateCommand("select count(*) from dbo.ViewFilters where ViewId=@viewId");
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                command.CommandType = CommandType.Text;

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewFiltersCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewFiltersCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewFiltersCountImpl"), ex);
            }
        }
        internal override VLViewFilter GetViewFilterByIdImpl(Int32 accessToken, Int32 filterId)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_GetById");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@filterId", filterId, DbType.Int32);


                return ExecuteAndGetViewFilter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewFilterByIdImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewFilterByIdImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewFilterByIdImpl"), ex);
            }
        }
        internal override void DeleteViewFilterImpl(Int32 accessToken, Guid viewId, Int32 filterId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_Delete");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@filterId", filterId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteViewFilterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewFilterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewFilterImpl"), ex);
            }
        }
        internal override void DeleteViewFiltersForViewImpl(Int32 accessToken, Guid viewId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_DeleteAllForView");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);

                ExecuteNonReader(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "DeleteViewFilterAllForViewImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewFilterAllForViewImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "DeleteViewFilterAllForViewImpl"), ex);
            }
        }
        internal override VLViewFilter CreateViewFilterImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_Create");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", filter.ViewId, DbType.Guid);
                AddParameter(command, "@survey", filter.Survey, DbType.Int32);
                AddParameter(command, "@name", filter.Name, DbType.String);
                AddParameter(command, "@applyOrder", filter.ApplyOrder, DbType.Int16);
                AddParameter(command, "@isRule", filter.IsRule, DbType.Boolean);
                AddParameter(command, "@question", filter.Question, DbType.Int16);
                AddParameter(command, "@questionType", filter.QuestionType, DbType.Byte);
                AddParameter(command, "@logicalOperator", filter.LogicalOperator, DbType.Byte);
                AddParameter(command, "@isActive", filter.IsActive, DbType.Boolean);
                AddParameter(command, "@attributeFlags", filter.AttributeFlags, DbType.Int16);
                if(filter.FilterDetails.Count > 0)
                {
                    for (int i = 0; i < filter.FilterDetails.Count; i++)
                    {
                        AddParameter(command, "@opr" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].Operator, DbType.Byte);
                        AddParameter(command, "@opt" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].SelectedOption, DbType.Byte);
                        AddParameter(command, "@col" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].SelectedColumn, DbType.Byte);
                        AddParameter(command, "@usA" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].UserInput1, DbType.String);
                        AddParameter(command, "@usB" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].UserInput2, DbType.String);
                    }
                }
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetViewFilter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "CreateViewFilterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateViewFilterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "CreateViewFilterImpl"), ex);
            }
        }
        internal override VLViewFilter UpdateViewFilterImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_Update");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", filter.ViewId, DbType.Guid);
                AddParameter(command, "@filterId", filter.FilterId, DbType.Int32);
                AddParameter(command, "@name", filter.Name, DbType.String);
                AddParameter(command, "@applyOrder", filter.ApplyOrder, DbType.Int16);
                AddParameter(command, "@isActive", filter.IsActive, DbType.Boolean);
                AddParameter(command, "@attributeFlags", filter.AttributeFlags, DbType.Int16);
                if (filter.FilterDetails.Count > 0)
                {
                    for (int i = 0; i < filter.FilterDetails.Count; i++)
                    {
                        AddParameter(command, "@opr" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].Operator, DbType.Byte);
                        AddParameter(command, "@opt" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].SelectedOption, DbType.Byte);
                        AddParameter(command, "@col" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].SelectedColumn, DbType.Byte);
                        AddParameter(command, "@usA" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].UserInput1, DbType.String);
                        AddParameter(command, "@usB" + (i).ToString(CultureInfo.InvariantCulture), filter.FilterDetails[i].UserInput2, DbType.String);
                    }
                }
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetViewFilter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewFilterImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewFilterImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewFilterImpll"), ex);
            }
        }

        internal override VLViewFilter UpdateViewFilterQuickImpl(Int32 accessToken, VLViewFilter filter, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewfilters_Update_Quick");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@viewId", filter.ViewId, DbType.Guid);
                AddParameter(command, "@filterId", filter.FilterId, DbType.Int32);
                AddParameter(command, "@name", filter.Name, DbType.String);
                AddParameter(command, "@applyOrder", filter.ApplyOrder, DbType.Int16);
                AddParameter(command, "@isActive", filter.IsActive, DbType.Boolean);
                AddParameter(command, "@attributeFlags", filter.AttributeFlags, DbType.Int16);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);



                return ExecuteAndGetViewFilter(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "UpdateViewFilterQuickImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewFilterQuickImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "UpdateViewFilterQuickImpl"), ex);
            }
        }
        #endregion

        #region VLResponses from Views
        internal override Collection<VLResponse> GetViewResponsesImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewresponses_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);

                if (qfilters != null)
                {
                    for (int i = 0; i < qfilters.Count; i++)
                    {
                        AddParameter(command, "@qfilter" + (i + 1).ToString(CultureInfo.InvariantCulture), qfilters[i], DbType.String);
                    }
                }

                return ExecuteAndGetViewResponses(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewResponsesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesImpl"), ex);
            }
        }

        internal override Collection<VLResponse> GetViewResponsesPagesImpl(Int32 accessToken, Int32 surveyId, Guid viewId, int startRowIndex, int maximumRows, ref int totalRows, Collection<string> qfilters)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewresponses_GetPage");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);
                AddParameter(command, "@startRowIndex", startRowIndex, DbType.Int32);
                AddParameter(command, "@maximumRows", maximumRows, DbType.Int32);

                if (qfilters != null)
                {
                    for (int i = 0; i < qfilters.Count; i++)
                    {
                        AddParameter(command, "@qfilter" + (i + 1).ToString(CultureInfo.InvariantCulture), qfilters[i], DbType.String);
                    }
                }

                AddParameter(command, "@totalRows", totalRows, DbType.Int32, ParameterDirection.Output);


                var collection = ExecuteAndGetViewResponses(command);
                totalRows = Convert.ToInt32(command.Parameters["@totalRows"].Value);
                return collection;
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewResponsesPagesImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesPagesImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesPagesImpl"), ex);
            }
        }

        internal override int GetViewResponsesCountImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewresponses_GetTotalRows");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);

                if (qfilters != null)
                {
                    for (int i = 0; i < qfilters.Count; i++)
                    {
                        AddParameter(command, "@qfilter" + (i + 1).ToString(CultureInfo.InvariantCulture), qfilters[i], DbType.String);
                    }
                }

                return (Int32)ExecuteScalar(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewResponsesCountImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesCountImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewResponsesCountImpl"), ex);
            }
        }

        internal override VLSummaryEx GetViewSummaryExImpl(Int32 accessToken, Int32 surveyId, Guid viewId, Collection<string> qfilters)
        {
            try
            {
                DbCommand command = CreateCommand("valis_viewsummary_GetAll");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@surveyId", surveyId, DbType.Int32);
                AddParameter(command, "@viewId", viewId, DbType.Guid);

                if (qfilters != null)
                {
                    for (int i = 0; i < qfilters.Count; i++)
                    {
                        AddParameter(command, "@qfilter" + (i + 1).ToString(CultureInfo.InvariantCulture), qfilters[i], DbType.String);
                    }
                }

                return ExecuteAndGetViewSummaryEx(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetViewSummaryExImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewSummaryExImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetViewSummaryExImpl"), ex);
            }
        }
        #endregion


        internal override VLClientDashboard GetClientDashboardImpl(Int32 accessToken, Int32 clientId, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_statistics_GetClientDashboard");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@clientId", clientId, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetClientDashboard(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetClientDashboardImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetClientDashboardImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetClientDashboardImpl"), ex);
            }
        }
        internal override VLSystemDashboard GetSystemDashboardImpl(Int32 accessToken, DateTime currentTimeUtc)
        {
            try
            {
                DbCommand command = CreateCommand("valis_statistics_GetSystemDashboard");
                AddParameter(command, "@accessToken", accessToken, DbType.Int32);
                AddParameter(command, "@currentTimeUtc", currentTimeUtc, DbType.DateTime);


                return ExecuteAndGetSystemDashboard(command);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.State == 10)
                {
                    throw new VLInvalidAccessTokenException(SR.GetString(SR.Invalid_accessToken_while_calling_SurveyDao, "GetSystemDashboardImpl"), ex);
                }
                else
                {
                    throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSystemDashboardImpl"), ex);
                }
            }
            catch (Exception ex)
            {
                throw new VLDataException(SR.GetString(SR.Exception_occured_at_SurveyDao, "GetSystemDashboardImpl"), ex);
            }
        }
    }
}
