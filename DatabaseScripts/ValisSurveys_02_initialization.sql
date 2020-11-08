set nocount on
GO
USE [ValisSurveys]
GO

delete from [dbo].[ViewSummary]
delete from [dbo].[ViewCollectors]
delete from [dbo].[ViewFilterDetails]
delete from [dbo].[ViewFilters]
delete from [dbo].[ViewPages]
delete from [dbo].[ViewQuestions]
delete from [dbo].[Views]

delete from dbo.ResponseDetails
delete from dbo.Responses
delete from dbo.RuntimeSessions

delete from [dbo].[FileInventoryStreams]
delete from [dbo].FileInventory


delete from [dbo].[SurveyColumns_Texts]
delete from [dbo].[SurveyColumns] 
delete from [dbo].[SurveyOptions_Texts]
delete from [dbo].[SurveyOptions] 

delete from [dbo].[SurveyQuestions_Texts]
delete from [dbo].[SurveyQuestions] 

delete from [dbo].[SurveyPages_Texts]
delete from [dbo].[SurveyPages] 

delete from [dbo].[MessageRecipients]
delete from [dbo].[Recipients]
delete from [dbo].[Messages]
delete from [dbo].[CollectorPayments]
delete from [dbo].[SurveyCollectors_Texts]
delete from [dbo].[SurveyCollectors]

delete from [dbo].[Surveys_Texts] 
delete from [dbo].[Surveys] 
delete from [dbo].[SurveyFolders]
delete from [dbo].[SurveyThemes]

delete from [dbo].[SystemEmails]
delete from [dbo].[Audits]
delete from dbo.AccessTokens
delete from dbo.Credentials
delete from dbo.SystemUsers
delete from [dbo].[ClientContacts]
delete from [dbo].[ClientLists]
delete from dbo.ClientUsers
delete from [dbo].[Payments]
delete from [dbo].[KnownEmails]
delete from dbo.Clients
delete from [dbo].[ClientProfiles]
delete from dbo.Roles
delete from dbo.QuestionTypes_Texts
delete from dbo.QuestionTypes

delete from [dbo].[LibraryColumns_Texts]
delete from [dbo].[LibraryColumns]
delete from [dbo].[LibraryOptions_Texts]
delete from [dbo].[LibraryOptions]
delete from [dbo].[LibraryQuestions_Texts]
delete from [dbo].[LibraryQuestions]
delete from [dbo].[LibraryQuestionCategories_Texts]
delete from [dbo].[LibraryQuestionCategories]

delete from [dbo].[EmailTemplates]
delete from dbo.Languages
delete from dbo.PrincipalTypes
delete from [dbo].[Countries]
delete from [dbo].[CollectorTypes]
delete from [dbo].[CollectorStatuses]

delete from [dbo].[SystemParameters]
delete from [dbo].[UniqueSequences]
delete from [dbo].[UserAgents]



DBCC CHECKIDENT (AccessTokens, reseed, 1)
DBCC CHECKIDENT (Audits, reseed, 1)
DBCC CHECKIDENT (CollectorPayments, reseed, 1)
DBCC CHECKIDENT (ClientContacts, reseed, 1)
DBCC CHECKIDENT (ClientLists, reseed, 1)
DBCC CHECKIDENT (Payments, reseed, 1)
DBCC CHECKIDENT (ClientProfiles, reseed, 1)
DBCC CHECKIDENT (Clients, reseed, 1)
DBCC CHECKIDENT (Countries, reseed, 1)
DBCC CHECKIDENT (Credentials, reseed, 1)
DBCC CHECKIDENT (EmailTemplates, reseed, 1)
DBCC CHECKIDENT (KnownEmails, reseed, 1)

DBCC CHECKIDENT (LibraryQuestionCategories, reseed, 1)
DBCC CHECKIDENT (LibraryQuestions, reseed, 1)

DBCC CHECKIDENT (Messages, reseed, 1)
DBCC CHECKIDENT (Recipients, reseed, 1)
DBCC CHECKIDENT (Responses, reseed, 1)
DBCC CHECKIDENT (Roles, reseed, 1)
DBCC CHECKIDENT (SurveyCollectors, reseed, 1)
DBCC CHECKIDENT (Surveys, reseed, 1)
DBCC CHECKIDENT (SurveyThemes, reseed, 1)
DBCC CHECKIDENT (SystemEmails, reseed, 1)
DBCC CHECKIDENT (UserAgents, reseed, 1)
DBCC CHECKIDENT (ViewFilterDetails, reseed, 1)
DBCC CHECKIDENT (ViewFilters, reseed, 1)
DBCC CHECKIDENT (ViewSummary, reseed, 1)



insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001001', 'EnablePasswordRetrieval',				'0',5 /*BooleanType*/, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001002', 'EnablePasswordReset',					'0',5 /*BooleanType*/, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001003', 'RequiresQuestionAndAnswer',			'0',5 /*BooleanType*/, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001004', 'RequiresUniqueEmail',					'1',5 /*BooleanType*/, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001005', 'MaxInvalidPasswordAttempts',			'5',1 /*Int32Type  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001006', 'PasswordAttemptWindow',				'10',1 /*Int32Type  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001007', 'MinRequiredPasswordLength',			'4',1 /*Int32Type  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001008', 'MinRequiredNonalphanumericCharacters','0',1 /*Int32Type  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001010', 'DefaultTimeZoneId'					,'GTB Standard Time',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);

insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001040', '@BrandName'				,'ValisSurveys',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001041', '@FromDisplayName'			,'ValisSurveys',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001042', '@SystemPublicName'		,'ValisSurveys v2015.0.0107.0',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001043', '@NoreplyEmail'			,'noreply@valissurveys.com',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001044', '@SupportEmail'			,'support@valissurveys.com',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001045', '@SurveyTeam'				,'Valis Survey Team',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into dbo.SystemParameters ( ParameterId, ParameterKey, ParameterValue, ParameterType, AttributeFlags, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values ('64617461-7661-6c75-6500-000000001046', '@EmailSignature'			,'@2014 ValisSurveys. Athens, Greece. All rights reserved.',0 /*StringType  */, 1, GETUTCDATE(), 0, GETUTCDATE(), 0);

/*Countries*/

SET IDENTITY_INSERT dbo.Countries ON
insert into [dbo].[Countries] ([CountryId], [Name]) values(0,N'Undefined')
insert into [dbo].[Countries] ([CountryId], [Name]) values(1,N'Bulgaria')
insert into [dbo].[Countries] ([CountryId], [Name]) values(2,N'Cyprus')
insert into [dbo].[Countries] ([CountryId], [Name]) values(3,N'France')
insert into [dbo].[Countries] ([CountryId], [Name]) values(4,N'Germany')
insert into [dbo].[Countries] ([CountryId], [Name]) values(5,N'Greece')
SET IDENTITY_INSERT dbo.Countries OFF

/*Languages*/
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(0, 'Invariant Language', 127, '', 'iv', 'ivl');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(2, 'Albanian', 28, 'sq', 'sq', 'sqi');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(19, 'Bulgarian', 2, 'bg', 'bg', 'bul');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(33, 'English', 9, 'en', 'en', 'eng');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(38, 'French', 12, 'fr', 'fr', 'fra');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(42, 'German', 7, 'de', 'de', 'deu');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(43, 'Greek', 8, 'el', 'el', 'ell');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(50, 'Hungarian', 14, 'hu', 'hu', 'hun');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(95, 'Polish', 21, 'pl', 'pl', 'pol');
insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(101, 'Russian', 25, 'ru', 'ru', 'rus');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(129, 'Turkish', 31, 'tr', 'tr', 'tur');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(130, 'Turkmen', 66, 'tk', 'tk', 'tuk');
--insert into Languages (LanguageId,EnglishName,LCID,Name,TwoLetterISOCode,ThreeLetterISOCode) values(131, 'Ukrainian', 34, 'uk', 'uk', 'ukr');



SET IDENTITY_INSERT dbo.LibraryQuestionCategories ON
insert into [dbo].[LibraryQuestionCategories] ([CategoryId],[AttributeFlags],[CreationDT],[CreatedBy],[LastUpdateDT],[LastUpdatedBy]) values(1,1, GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 0/*Invariant*/, N'Common questions');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 19/*Bulgarian*/, N'Често задавани въпроси');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 33/*English*/, N'Common questions');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 38/*French*/, N'Questions fréquentes');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 42/*German*/, N'Häufig gestellte Fragen');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 43/*Greek*/, N'Συχνές ερωτήσεις');
insert into [dbo].[LibraryQuestionCategories_Texts] ([CategoryId],[Language],[Name]) values(1, 101/*Russian*/, N'Общие вопросы');
SET IDENTITY_INSERT dbo.LibraryQuestionCategories OFF



/*QuestionTypes*/
insert into dbo.QuestionTypes (TypeId, Name) values(1, N'SingleLine');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (1, 33, N'SingleLine', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (1, 43, N'ΜονήΓραμμήΚειμένου', null);
insert into dbo.QuestionTypes (TypeId, Name) values(2, N'MultipleLine');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (2, 33, N'MultipleLine', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (2, 43, N'Κείμενο', null);
insert into dbo.QuestionTypes (TypeId, Name) values(3, N'Integer');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (3, 33, N'Integer', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (3, 43, N'Ακέραιος', null);
insert into dbo.QuestionTypes (TypeId, Name) values(4, N'Decimal');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (4, 33, N'Decimal', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (4, 43, N'Δεκαδικός', null);
insert into dbo.QuestionTypes (TypeId, Name) values(5, N'Date');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values  (5, 33, N'Date', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (5, 43, N'Ημερομηνία', null);

insert into dbo.QuestionTypes (TypeId, Name) values(6, N'Time');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (6, 33, N'Time', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (6, 43, N'Ωρα', null);
insert into dbo.QuestionTypes (TypeId, Name) values(7, N'DateTime');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (7, 33, N'DateTime', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (7, 43, N'ΗμερομηνίαΩρα', null);
insert into dbo.QuestionTypes (TypeId, Name) values(10, N'OneFromMany');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (10, 33, N'OneFromMany', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (10, 43, N'OneFromMany', null);
insert into dbo.QuestionTypes (TypeId, Name) values(11, N'ManyFromMany');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (11, 33, N'ManyFromMany', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (11, 43, N'ManyFromMany', null);
insert into dbo.QuestionTypes (TypeId, Name) values(12, N'DropDown');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (12, 33, N'DropDown', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (12, 43, N'DropDown', null);
	
insert into dbo.QuestionTypes (TypeId, Name) values(16, N'DescriptiveText');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (16, 33, N'DescriptiveText', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (16, 43, N'DescriptiveText', null);

insert into dbo.QuestionTypes (TypeId, Name) values(21, N'Slider');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (21, 33, N'Slider', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (21, 43, N'Slider', null);
insert into dbo.QuestionTypes (TypeId, Name) values(22, N'Range');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (22, 33, N'Range', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (22, 43, N'Range', null);

insert into dbo.QuestionTypes (TypeId, Name) values(31, N'MatrixOnePerRow');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (31, 33, N'MatrixOnePerRow', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (31, 43, N'MatrixOnePerRow', null);
insert into dbo.QuestionTypes (TypeId, Name) values(32, N'MatrixManyPerRow');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (32, 33, N'MatrixManyPerRow', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (32, 43, N'MatrixManyPerRow', null);
insert into dbo.QuestionTypes (TypeId, Name) values(33, N'MatrixManyPerRowCustom');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (33, 33, N'MatrixManyPerRowCustom', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (33, 43, N'MatrixManyPerRowCustom', null);
insert into dbo.QuestionTypes (TypeId, Name) values(41, N'Composite');
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (41, 33, N'Composite', null);
	insert into dbo.QuestionTypes_Texts (TypeId,Language,ShowName,Description) values (41, 43, N'Συνθεση', null);




/*dbo.PrincipalTypes*/
insert into dbo.PrincipalTypes (TypeId, Name) values (0, 'SystemUser');
insert into dbo.PrincipalTypes (TypeId, Name) values (1, 'ClientUser');

/*[dbo].[CollectorTypes]*/
insert into [dbo].[CollectorTypes] ([TypeId],[Name]) values (0, 'WebLink');
insert into [dbo].[CollectorTypes] ([TypeId],[Name]) values (1, 'Email');
insert into [dbo].[CollectorTypes] ([TypeId],[Name]) values (2, 'Website');
insert into [dbo].[CollectorTypes] ([TypeId],[Name]) values (3, 'ShareOnFacebook');
/*[dbo].[CollectorStatuses]*/
insert into [dbo].[CollectorStatuses] ([StatusId],[Name]) values (0, 'New');
insert into [dbo].[CollectorStatuses] ([StatusId],[Name]) values (1, 'Open');
insert into [dbo].[CollectorStatuses] ([StatusId],[Name]) values (2, 'Close');
/*[dbo].[ClientProfiles]*/
SET IDENTITY_INSERT [dbo].[ClientProfiles] ON 
INSERT [dbo].[ClientProfiles] ([ProfileId], [Name], [Comment], [MaxNumberOfUsers], [MaxNumberOfSurveys], [MaxNumberOfLists], [MaxNumberOfRecipientsPerList], [MaxNumberOfRecipientsPerMessage], [MaxNumberOfCollectorsPerSurvey], [MaxNumberOfEmailsPerDay], [MaxNumberOfEmailsPerWeek], [MaxNumberOfEmailsPerMonth], [MaxNumberOfEmails], [AttributeFlags], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) 
VALUES (1, N'UTESTFree', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4093, GETUTCDATE(), 0, GETUTCDATE(), 0);
INSERT [dbo].[ClientProfiles] ([ProfileId], [Name], [Comment], [MaxNumberOfUsers], [MaxNumberOfSurveys], [MaxNumberOfLists], [MaxNumberOfRecipientsPerList], [MaxNumberOfRecipientsPerMessage], [MaxNumberOfCollectorsPerSurvey], [MaxNumberOfEmailsPerDay], [MaxNumberOfEmailsPerWeek], [MaxNumberOfEmailsPerMonth], [MaxNumberOfEmails], [AttributeFlags], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) 
VALUES (2, N'UTESTPaid', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 4095, GETUTCDATE(), 0, GETUTCDATE(), 0);

INSERT [dbo].[ClientProfiles] ([ProfileId], [Name], [Comment], [MaxNumberOfUsers], [MaxNumberOfSurveys], [MaxNumberOfLists], [MaxNumberOfRecipientsPerList], [MaxNumberOfRecipientsPerMessage], [MaxNumberOfCollectorsPerSurvey], [MaxNumberOfEmailsPerDay], [MaxNumberOfEmailsPerWeek], [MaxNumberOfEmailsPerMonth], [MaxNumberOfEmails], [AttributeFlags], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) 
VALUES (6, N'Demo', NULL, 3, 12, 2, 200, 200, 3, 200, 400, 1200, 5000, 633, GETUTCDATE(), 0, GETUTCDATE(), 0);
INSERT [dbo].[ClientProfiles] ([ProfileId], [Name], [Comment], [MaxNumberOfUsers], [MaxNumberOfSurveys], [MaxNumberOfLists], [MaxNumberOfRecipientsPerList], [MaxNumberOfRecipientsPerMessage], [MaxNumberOfCollectorsPerSurvey], [MaxNumberOfEmailsPerDay], [MaxNumberOfEmailsPerWeek], [MaxNumberOfEmailsPerMonth], [MaxNumberOfEmails], [AttributeFlags], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) 
VALUES (7, N'Default', NULL, 4, 40, 24, 1000, 1000, 24, 2500, 7500, 15000, 150000, 4095, GETUTCDATE(), 0, GETUTCDATE(), 0);

SET IDENTITY_INSERT [dbo].[ClientProfiles] OFF




SET IDENTITY_INSERT dbo.Roles ON
/*
	Τα πρώτα primary keys (0, 1, 2, ,3, 4, 5, 6, 7, 8, 9) είναι αόρατα απο την διαχείριση του συστήματος
*/

	/*None*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(0,		'None',				'No permissions at all',				0,						1,0, GETUTCDATE(), 0, GETUTCDATE(), 0);
	/*SystemAdmin*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(1,		'SystemAdmin',		'The role of a SystemAdmin',			4194303,		1,0, GETUTCDATE(), 0, GETUTCDATE(), 0);
	/*Developer*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(2,		'Developer',		'The role of a Developer',				4194303,		1,0, GETUTCDATE(), 0, GETUTCDATE(), 0);/*Developer*/
	/*ServiceRole*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(3,		'Service',			'The role of a Service',				64164,			1,0, GETUTCDATE(), 0, GETUTCDATE(), 0);
	
	/*Admin*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(10,		'Administrator',	'The role of an Administrator',			110305,		1,0, GETUTCDATE(), 0, GETUTCDATE(), 0);
	/*PowerClient*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(15,		'PowerClient',		'The role of a PowerClient',			'549751663232',		1,1, GETUTCDATE(), 0, GETUTCDATE(), 0);
	/*Client*/
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(20,		'Client',			'The role of a Client',					'549705525888',		1,1, GETUTCDATE(), 0, GETUTCDATE(), 0);
	/*DemoClient*/	
	insert into dbo.Roles (RoleId,Name,Description,Permissions,IsBuiltIn,IsClientRole,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(22,		'DemoClient',		'The role of a Demo-Client',			'173493190656',		1,1, GETUTCDATE(), 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT dbo.Roles OFF




/*installer*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(1, 0,	'Installer',		'Account',null,						0,1,	0,	0,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*SystemAdmin			(sysadmin/tolk!3n)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(2, 0,	'SystemAdmin',		'Account','sysadmin@dummy.com',		1,1,	0,	1,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*Developer				(developer/tolk!3n)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(3, 43, 'Developer',		'Account','developer@dummy.com',	1,1,	0,	2,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*Admin					(admin/tolk!3n)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(4, 43, 'Administrator',	'Account','admin@dummy.com',		1,1,	0,	10,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*ValisDaemon			(d@3m0nUs3r/v@l1$D@#M)NP@$$)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(5, 43, 'ValisDaemon',		'Service','valisdaemon@dummy.com',	1,1,	0,	3,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*ValisServer			(s3Rv3rUs3r/v@l1$D@#M)NP@$$)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(6, 43, 'ValisServer',		'Service','valisserver@dummy.com',	1,1,	0,	3,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*ValisReporter			(r2p0rt3rUs3r/v@l1$D@#M)NP@$$)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(7, 43, 'ValisReporter',	'Service','valisreporter@dummy.com',1,1,	0,	3,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*ValisManager			(m@n@g3rUs3r/v@l1$D@#M)NP@$$)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(8, 43, 'ValisManager',		'Service','valismanager@dummy.com',	1,1,	0,	3,		GETUTCDATE(), 0, GETUTCDATE(), 0);
/*ValisWathDog			(w@t3hd0gUs3r/v@l1$D@#M)NP@$$)*/
insert into dbo.SystemUsers (UserId,DefaultLanguage, FirstName,LastName,Email,IsActive,IsBuiltIn,AttributeFlags,Role,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values(9, 43, 'ValisWatchDg',		'Service','valiswatchdog@dummy.com',1,1,	0,	3,		GETUTCDATE(), 0, GETUTCDATE(), 0);


SET IDENTITY_INSERT dbo.Credentials ON
		/*sysadmin,		--> tolk!3n pswd='656M/ClNcGRUjxtEUQlpw46LXww=', salt='g1DnbizYE/M5jhfjsQRi2w=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(2, 2, 0, 'sysadmin',	'656M/ClNcGRUjxtEUQlpw46LXww=',1, 'g1DnbizYE/M5jhfjsQRi2w==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*developer,	--> tolk!3n pswd='JLNLNllthz1ndsk5gNjRbW+4KAA=', salt='4n2IX0ar6pi1qJF15ppNQw=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(3, 3, 0, 'developer',	'JLNLNllthz1ndsk5gNjRbW+4KAA=',1, '4n2IX0ar6pi1qJF15ppNQw==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*admin,		--> tolk!3n pswd='8VPkpntTPK7pa5o7bjmYG7n3bl4=', salt='4KPZV+8x3lELsJlv+jF+AQ=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(4, 4, 0, 'admin',		'8VPkpntTPK7pa5o7bjmYG7n3bl4=',1, '4KPZV+8x3lELsJlv+jF+AQ==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*d@3m0nUs3r,	--> v@l1$D@#M)NP@$$) pswd='Yhax+FwxfiWRELE3i1h38WXPl7Q=', salt='wn1Np+nsP3ZdYpk/2Tlp/g=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(5, 5, 0, 'd@3m0nUs3r',	'Yhax+FwxfiWRELE3i1h38WXPl7Q=',1, 'wn1Np+nsP3ZdYpk/2Tlp/g==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*s3Rv3rUs3r,	--> v@l1$D@#M)NP@$$) pswd='6KXEoffVARQ8JvR6ToVzeqPz2Ss=', salt='ybMxrR1Mi3eRINYQ/RGNNw=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(6, 6, 0, 's3Rv3rUs3r',	'6KXEoffVARQ8JvR6ToVzeqPz2Ss=',1, 'ybMxrR1Mi3eRINYQ/RGNNw==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*r2p0rt3rUs3r,	--> v@l1$D@#M)NP@$$) pswd='tc9SHqZhzeXeFgPoaPuAyjAUQ+c=', salt='BasIDRhCupzJPAjLwvs0Vg=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(7, 7, 0, 'r2p0rt3rUs3r', 'tc9SHqZhzeXeFgPoaPuAyjAUQ+c=',1, 'BasIDRhCupzJPAjLwvs0Vg==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*r2p0rt3rUs3r,	--> v@l1$D@#M)NP@$$) pswd='ytwUqatJ3K+k9aR50omdelH+2+8=', salt='Wdcv2bMdX6n0JN8BXOzd8Q=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(8, 8, 0, 'm@n@g3rUs3r', 'ytwUqatJ3K+k9aR50omdelH+2+8=',1, 'Wdcv2bMdX6n0JN8BXOzd8Q==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
		/*w@t3hd0gUs3r,	--> v@l1$D@#M)NP@$$) pswd='75vDJNPbiLP2M75xH+yGUWr+uJI=', salt='3qkjSXkUFG4PsrH4EI2vcw=='*/
		insert into dbo.Credentials (CredentialId, Principal, PrincipalType, LogOnToken,PswdToken,PswdFormat,PswdSalt, IsApproved, IsLockedOut, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, CreationDT, CreatedBy, LastUpdateDT, LastUpdatedBy) values(9, 9, 0, 'w@t3hd0gUs3r', '75vDJNPbiLP2M75xH+yGUWr+uJI=',1, '3qkjSXkUFG4PsrH4EI2vcw==', 1,0,GETUTCDATE(),GETUTCDATE(),null, 0,null,0,null,GETUTCDATE(), 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT dbo.Credentials OFF







insert into dbo.UniqueSequences(SequenceName, SequenceId) values ('Principals', 1000);	/*This is the Primary key for [dbo].[ClientUsers] and [dbo].[SystemUsers]*/






SET IDENTITY_INSERT dbo.SurveyThemes ON
insert into [dbo].[SurveyThemes] ([ThemeId],[ClientId], [Name],[RtHtml],[RtCSS],[DtHtml],[DtCSS],  AttributeFlags,CreationDT,CreatedBy,LastUpdateDT,LastUpdatedBy) values (1,NULL, 'Default', N'', N'', N'',N'', 0, GETUTCDATE(), 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT dbo.SurveyThemes OFF


update [dbo].[SurveyThemes] set [RtHtml]=N'
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width,initial-scale=1, maximum-scale=1.5" />
	<meta http-equiv="expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="pragma" content="no-cache" />
    #@HTMLHEAD
	<script>
		function OnLanguageSelect(languageId, twoLetterISOCode, prvLanguageId, prvTwoLetterISOCode)
		{	
			if(languageId == prvLanguageId)
			{
				document.forms[0].RefreshPage.value = "1";
				document.forms[0].submit();
			}
			else
			{
				var _href = window.location.href.replace("/"+prvTwoLetterISOCode+"/","/"+twoLetterISOCode+"/");
				document.forms[0].action = _href;
				document.forms[0].RefreshPage.value = "1";
				document.forms[0].submit();
			}
		}
		function OnclientClose()
		{
			setTimeout("window.close();", 100);
			//open(location, ''_self'').close();
		}
	</script>
</head>
<body>
    <div class="surveyHeader">
	#@SURVEYHEADER
	#@SURVEYLANGUAGESELECTOR
	</div>
    <form id="theForm" name="theForm" method="#@FORM_METHOD" action="#@FORM_ACTION">
        #@SURVEYBODY
	#@SURVEY_BOTTOM_PROGRESSBAR
        #@SURVEYNAVIGATION
    </form>
    #@SURVEYFOOTER
</body>
</html>
' where ThemeId = 1




update [dbo].[SurveyThemes] set [RtCSS]=N'
body {
    font-family: Arial, sans-serif;
    font-size: 16px;
}

* {
    margin: 0px; padding: 0px;
}


th abbr, abbr.noborder {
    border-bottom: 0 none;
}

/*#region surveyHeader
----------------------------------------------*/
.surveyHeader
{
    font-size: 1.7em;
    padding: 6px 0px 12px 6px;
    background-color: #002b60;
    color: #fff;
}
/*#endregion*/


div.languageSelector
{
    position: absolute;
    right: 8px; top: 8px;
}
div.languageSelector img
{
    margin: 4px;
    cursor: pointer;
}


/*#region surveyNavigation
----------------------------------------------*/
div.surveyNavigation
{
    margin: 24px 0px 12px 0px;
    height: 40px;
    text-align: center;
    background-color: #cce3ff;
}
div.surveyNavigation input[type=submit]
{
    margin-top: 6px;
    margin-left: 12px;
    font-size: 1em;
    height: 28px;
    border: 1px solid gray;
    cursor: pointer;
    min-width: 112px;
    border-radius: 3px 4px;
    background-color: #ededed;
}
div.surveyNavigation input#PreviousButton
{

}
div.surveyNavigation input#NextButton
{

}

div.surveyNavigation input[type=button]
{
    margin-top: 6px;
    margin-left: 12px;
    font-size: 1em;
    height: 28px;
    border: 1px solid gray;
    cursor: pointer;
    min-width: 112px;
    border-radius: 3px 4px;
    background-color: #ededed;
}
/*#endregion*/




/*#region surveyWelcome
----------------------------------------------*/
div.surveyWelcome
{
    margin: 12px 0px 12px 0px;
    padding: 6px;
}
/*#endregion*/


/*#region surveyGoodbye
----------------------------------------------*/
div.surveyGoodbye
{
    margin: 12px 0px 12px 0px;
    padding: 6px;
}
/*#endregion*/

/*#region surveyDisqualification
----------------------------------------------*/
div.surveyDisqualification
{
    margin: 12px 0px 12px 0px;
    padding: 6px;
}
/*#endregion*/

/*#region surveyEnd
----------------------------------------------*/
div.surveyEnd
{
    margin: 12px 0px 12px 0px;
    padding: 6px;
	text-align: center;
}
/*#endregion*/




/*#region pageHeader
----------------------------------------------*/
h2.pageHeader
{
    font-size: 1.3em;
    background-color: #cce3ff;
    padding: 4px 0px 8px 4px;
}
/*#endregion*/


/*#region progress bar
----------------------------------------------*/
.ProgBar {
    color: #000000;
    font: bold x-small Arial,sans-serif;
    margin: 15px 20%;
    width: 60%;
}
.ProgBar TABLE {
    background: none repeat scroll 0 0 #cccccc;
}
.ProgBar TD.ProgTxt {
    background: none repeat scroll 0 0 #ffffff;
    margin: 0 3px;
    padding: 3px 6px;
}
.ProgBar TD {
    border: 1px solid #cccccc;
    padding: 3px;
    vertical-align: middle;
}

.BarArea {
    background: none repeat scroll 0 0 #ac8f00;
    font-size: 1px;
    height: 10px;
}
/*#endregion*/


/*#region questions
----------------------------------------------*/
div.questions 
{

}
/*#endregion*/



div.questionBox 
{
    margin: 16px 0px 16px 0px;
}

.questionHeader
{
    font: bold medium/1.4 Arial,sans-serif;
    background-color: #dfedff;
	color:#000000;
    padding: 4px 0px 4px 12px;
}
p.questionDescription
{
	font-size: 11px;
	font-weight: normal;
	color: #5d5d5d;
}
.validationError {margin: 0px 0 4px 18px;font: bold small Arial,sans-serif;text-decoration:none;color:#CC0000;}
.requiredMarker { display: inline-block;float:left;text-align:center;font:bold 24px verdana;line-height:1.0;color:#000000;width:18px;}
.red { color:#CC0000;}
.requiredMarker abbr{padding: 2px 0px 0px;border-bottom: 0px;}
abbr.questionNumber {border-bottom: 0px;}



.questionControl 
{
    margin: 16px 0px 32px 26px;
    font-size: .9em;
    text-align: left;
}

div.singleline input[type=text]
{
	width: 260px;
	padding: 2px;
}

div.multipleline textarea
{
	resize: none;
}


/*#region MatrixOnePerRowRenderer
----------------------------------------------*/
table.matrixoneperrow
{
    font-size: .8em;
    font-weight: normal;
    text-align: center;

	border-collapse: collapse;
	background-color: #efefef;
}
table.matrixoneperrow td.ColumnText
{
    width: 120px;
}
table.matrixoneperrow td.OptionText
{
    width: 220px;
	text-align: left;
	padding-top: 4px;
}
table.matrixoneperrow tr
{
	line-height: 1.8em;
}
table.matrixoneperrow tbody tr:nth-child(odd) {
   background-color: #dedede;
}
/*#endregion*/


/*#region MatrixManyPerRowRenderer
----------------------------------------------*/
table.matrixmanyperrow
{
    font-size: .8em;
    font-weight: normal;
    text-align: center;
}
table.matrixmanyperrow td.ColumnText
{
    width: 120px;
}
table.matrixmanyperrow td.OptionText
{
    width: 220px;
	text-align: left;
	padding-top: 4px;
}
/*#endregion*/




/*#region ManyFromManyRenderer
----------------------------------------------*/
div.manyfrommany
{

}
div.manyfrommany .mOption
{
    margin: 8px;
    line-height: 1.2em;
}
div.manyfrommany .mOption input[type=checkbox]
{
    margin-right: 8px;
}
div.manyfrommany .OptionalInputBox input[type=text]
{
    margin-left: 6px;
}
/*#endregion*/



/*#region OneFromManyRenderer
----------------------------------------------*/
div.onefrommany
{

}
div.onefrommany .mOption
{
    margin: 8px;
    line-height: 1.2em;
}
div.onefrommany .mOption input[type=radio]
{
    margin-right: 8px;
}
div.onefrommany .OptionalInputBox input[type=text]
{
    margin-left: 6px;
}
/*#endregion*/



/*#region CompositeRenderer
----------------------------------------------*/
div.composite
{

}
div.composite .mOption
{
    margin: 8px;
    line-height: 1.2em;
}
div.composite label
{
    margin-right: 6px;
}
/*#endregion*/


/*#region range
----------------------------------------------*/

div.range 
{
margin: 8px;
}
div.rangeline
{
    
    line-height: 1.8em;
}
 div.rangeline span.frontLabel,  div.rangeline span.afterLabel
{
     width: 96px;
    display: inline-block;
}
div.rangeline span.afterLabel {
        text-align: left;
        padding-left: 8px;
}
div.rangeline span.frontLabel {
        text-align: right;
        padding-right: 12px;
}
div.range div.mark {
        width: 32px;
        display: inline-block;
        text-align: center;
}
/*#endregion*/

' where ThemeId = 1


