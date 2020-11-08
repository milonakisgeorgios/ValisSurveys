/*
erikweber
erikweber@123

RebeccaBrink
RebeccaBrink@fl12

*/



SET IDENTITY_INSERT [dbo].[Clients] ON 
GO
INSERT [dbo].[Clients] ([ClientId], [Code], [Name], [Profession], [Country], [TimeZoneId], [Prefecture], [Town], [Address], [Zip], [Telephone1], [Telephone2], [WebSite], [AttributeFlags], [Profile], [Comment], [FolderSequence], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, N'Flovela', N'Flovela LTD', N'', 4, N'GTB Standard Time', N'', N'', N'', N'', N'', N'', N'', 0, 7, N'', 0, CAST(N'2020-11-01T20:40:13.0000000' AS DateTime2), 4, CAST(N'2020-11-01T23:23:32.0000000' AS DateTime2), 2)
GO
INSERT [dbo].[Clients] ([ClientId], [Code], [Name], [Profession], [Country], [TimeZoneId], [Prefecture], [Town], [Address], [Zip], [Telephone1], [Telephone2], [WebSite], [AttributeFlags], [Profile], [Comment], [FolderSequence], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (3, N'Soticon', N'Softicon', N'', 4, N'GTB Standard Time', N'', N'', N'', N'', N'', N'', N'', 0, 7, N'', 0, CAST(N'2020-11-01T22:50:27.0000000' AS DateTime2), 4, CAST(N'2020-11-01T23:23:21.0000000' AS DateTime2), 2)
GO
SET IDENTITY_INSERT [dbo].[Clients] OFF


GO
INSERT [dbo].[ClientUsers] ([Client], [UserId], [DefaultLanguage], [Title], [Department], [FirstName], [LastName], [Country], [TimeZoneId], [Prefecture], [Town], [Address], [Zip], [Telephone1], [Telephone2], [Email], [IsActive], [IsBuiltIn], [AttributeFlags], [Role], [Comment], [LastActivityDate], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 1001, 43, N'', N'', N'Erik', N'Weber', 4, NULL, N'', N'', N'', N'', N'', N'', N'eriker@gmail.com', 1, 0, 0, 15, N'', CAST(N'2020-11-07T09:51:55.0000000' AS DateTime2), CAST(N'2020-11-01T20:42:34.0000000' AS DateTime2), 4, CAST(N'2020-11-01T20:42:35.0000000' AS DateTime2), 4)
GO
INSERT [dbo].[ClientUsers] ([Client], [UserId], [DefaultLanguage], [Title], [Department], [FirstName], [LastName], [Country], [TimeZoneId], [Prefecture], [Town], [Address], [Zip], [Telephone1], [Telephone2], [Email], [IsActive], [IsBuiltIn], [AttributeFlags], [Role], [Comment], [LastActivityDate], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 1002, 43, N'', N'', N'Rebecca', N'Brink', 4, NULL, N'', N'', N'', N'', N'', N'', N'rebenk@gmail.com', 1, 0, 0, 20, N'', NULL, CAST(N'2020-11-01T20:43:38.0000000' AS DateTime2), 4, CAST(N'2020-11-01T20:43:38.0000000' AS DateTime2), 4)
GO
INSERT [dbo].[ClientUsers] ([Client], [UserId], [DefaultLanguage], [Title], [Department], [FirstName], [LastName], [Country], [TimeZoneId], [Prefecture], [Town], [Address], [Zip], [Telephone1], [Telephone2], [Email], [IsActive], [IsBuiltIn], [AttributeFlags], [Role], [Comment], [LastActivityDate], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (3, 1003, 43, N'', N'', N'George', N'Mylonakis', 4, NULL, N'', N'', N'', N'', N'', N'', N'milonakis@hotmail.com', 0, 0, 0, 15, N'', CAST(N'2020-11-01T22:51:53.0000000' AS DateTime2), CAST(N'2020-11-01T22:51:09.0000000' AS DateTime2), 4, CAST(N'2020-11-01T22:51:09.0000000' AS DateTime2), 4)
GO


SET IDENTITY_INSERT [dbo].[Credentials] ON 
GO
INSERT [dbo].[Credentials] ([CredentialId], [Principal], [PrincipalType], [LogOnToken], [PswdToken], [PswdFormat], [PswdSalt], [PswdQuestion], [PswdAnswer], [IsApproved], [IsLockedOut], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (10, 1001, 1, N'erikweber', N'KIba3FenN0ur3JypAV0aGRuvtro=', 1, N'csd2ZMpEg20Rz/s0mTS7Iw==', NULL, NULL, 1, 0, CAST(N'2020-11-07T09:51:37.0000000' AS DateTime2), NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-01T20:42:34.0000000' AS DateTime2), 4, CAST(N'2020-11-01T20:42:34.0000000' AS DateTime2), 4)
GO
INSERT [dbo].[Credentials] ([CredentialId], [Principal], [PrincipalType], [LogOnToken], [PswdToken], [PswdFormat], [PswdSalt], [PswdQuestion], [PswdAnswer], [IsApproved], [IsLockedOut], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (11, 1002, 1, N'RebeccaBrink', N'+1rsEWNl31bsRVFfCdPKrKjrg7M=', 1, N'D9/KQFU6nW+ZK+JvcpiBBg==', NULL, NULL, 1, 0, NULL, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-01T20:43:38.0000000' AS DateTime2), 4, CAST(N'2020-11-01T20:43:38.0000000' AS DateTime2), 4)
GO
INSERT [dbo].[Credentials] ([CredentialId], [Principal], [PrincipalType], [LogOnToken], [PswdToken], [PswdFormat], [PswdSalt], [PswdQuestion], [PswdAnswer], [IsApproved], [IsLockedOut], [LastLoginDate], [LastPasswordChangedDate], [LastLockoutDate], [FailedPasswordAttemptCount], [FailedPasswordAttemptWindowStart], [FailedPasswordAnswerAttemptCount], [FailedPasswordAnswerAttemptWindowStart], [Comment], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (12, 1003, 1, N'testuser', N'WP/hao/UyCbiU/m4ga/2O4s8k08=', 1, N'XefCnhllq2/V2FOJutX+Jg==', NULL, NULL, 1, 0, NULL, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-01T22:51:09.0000000' AS DateTime2), 4, CAST(N'2020-11-01T22:51:09.0000000' AS DateTime2), 4)
GO
SET IDENTITY_INSERT [dbo].[Credentials] OFF
GO


GO
SET IDENTITY_INSERT [dbo].[Surveys] ON 
GO
INSERT [dbo].[Surveys] ([Client], [SurveyId], [Folder], [PublicId], [Title], [Theme], [Logo], [AttributeFlags], [PageSequence], [QuestionSequence], [TicketSequence], [PrimaryLanguage], [SupportedLanguagesIds], [QuestionNumberingType], [ProgressBarPosition], [RequiredHighlightType], [DesignVersion], [RecordedResponses], [CustomId], [SourceSurvey], [TemplateSurvey], [OnCompletionMode], [OnDisqualificationMode], [Export1Name], [Export1Path], [Export1CreationDt], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 2, NULL, N'193CCBCE4A6341D29912E5C49AA906FB', N'Customer Satisfaction Survey', 1, NULL, 980, 6, 10, 0, 0, N'0,', 1, 0, 1, 57, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, CAST(N'2020-11-07T14:37:14.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:56:34.0000000' AS DateTime2), 1001)
GO
SET IDENTITY_INSERT [dbo].[Surveys] OFF
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 1, 1, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:37:14.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:50:19.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 2, 2, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:38:47.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:50:28.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 3, 3, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:39:51.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:50:36.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 4, 4, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:41:13.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:50:43.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 5, 5, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:42:21.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:50:53.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyPages] ([Survey], [PageId], [DisplayOrder], [PreviousPage], [NextPage], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToWebUrl], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 6, 6, NULL, NULL, 0, NULL, 0, NULL, NULL, CAST(N'2020-11-07T14:45:33.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:51:07.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 1, 1, NULL, 1, 22, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, 10, NULL, NULL, CAST(N'2020-11-07T14:37:44.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:37:44.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 2, 1, NULL, 2, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:38:33.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:38:33.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 3, 2, NULL, 1, 11, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 10, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:39:39.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:39:39.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 4, 3, NULL, 1, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:40:30.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:40:30.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 5, 3, NULL, 2, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:41:06.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:41:06.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 6, 4, NULL, 1, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:41:41.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:41:41.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 7, 4, NULL, 2, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:42:15.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:42:15.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 8, 5, NULL, 1, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 6, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:42:59.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:42:59.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 9, 5, NULL, 2, 10, NULL, 1, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 5, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:43:29.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:43:29.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyQuestions] ([Survey], [QuestionId], [Page], [MasterQuestion], [DisplayOrder], [QuestionType], [CustomType], [IsRequired], [RequiredBehavior], [RequiredMinLimit], [RequiredMaxLimit], [AttributeFlags], [ValidationBehavior], [ValidationField1], [ValidationField2], [ValidationField3], [RegularExpression], [RandomBehavior], [OtherFieldType], [OtherFieldRows], [OtherFieldChars], [OptionsSequence], [ColumnsSequence], [RangeStart], [RangeEnd], [LibraryQuestion], [CustomId], [CreationDT], [CreatedBy], [LastUpdateDT], [LastUpdatedBy]) VALUES (2, 10, 6, NULL, 1, 2, NULL, 0, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, NULL, NULL, NULL, NULL, CAST(N'2020-11-07T14:45:47.0000000' AS DateTime2), 1001, CAST(N'2020-11-07T14:45:47.0000000' AS DateTime2), 1001)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 2, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 2, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 2, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 2, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 2, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 6, 0, 6, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 7, 0, 7, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 8, 0, 8, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 9, 0, 9, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 3, 10, 0, 10, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 4, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 4, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 4, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 4, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 4, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 5, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 5, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 5, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 5, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 5, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 6, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 6, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 6, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 6, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 6, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 7, 6, 0, 6, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 8, 6, 0, 6, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 9, 1, 0, 1, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 9, 2, 0, 2, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 9, 3, 0, 3, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 9, 4, 0, 4, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions] ([Survey], [Question], [OptionId], [OptionType], [DisplayOrder], [OptionValue], [AttributeFlags], [CustomId], [SkipTo], [SkipToPage], [SkipToQuestion], [SkipToWebUrl]) VALUES (2, 9, 5, 0, 5, 0, 0, NULL, 0, NULL, NULL, NULL)
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 2, 1, 0, N'Very satisfied')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 2, 2, 0, N'Somewhat satisfied')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 2, 3, 0, N'Neither satisfied nor dissatisfied')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 2, 4, 0, N'Somewhat dissatisfied')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 2, 5, 0, N'Very dissatisfied')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 1, 0, N'Reliable')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 2, 0, N'High quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 3, 0, N'Useful')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 4, 0, N'Unique')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 5, 0, N'Good value for money')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 6, 0, N'Overpriced')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 7, 0, N'Impractical')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 8, 0, N'Ineffective')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 9, 0, N'Poor quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 3, 10, 0, N'Unreliable')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 4, 1, 0, N'Extremely well')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 4, 2, 0, N'Very well')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 4, 3, 0, N'Somewhat well')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 4, 4, 0, N'Not so well')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 4, 5, 0, N'Not at all well')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 5, 1, 0, N'Very high quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 5, 2, 0, N'High quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 5, 3, 0, N'Neither high nor low quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 5, 4, 0, N'Low quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 5, 5, 0, N'Very low quality')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 6, 1, 0, N'Excellent')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 6, 2, 0, N'Above average')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 6, 3, 0, N'Average')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 6, 4, 0, N'Below average')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 6, 5, 0, N'Poor')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 1, 0, N'Extremely responsive')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 2, 0, N'Very responsive')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 3, 0, N'Somewhat responsive')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 4, 0, N'Not so responsive')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 5, 0, N'Not at all responsive')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 7, 6, 0, N'Not applicable')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 1, 0, N'This is my first purchase')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 2, 0, N'Less than six months')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 3, 0, N'Six months to a year')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 4, 0, N'1 - 2 years')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 5, 0, N'3 or more years')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 8, 6, 0, N'I haven''t made a purchase yet')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 9, 1, 0, N'Extremely likely')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 9, 2, 0, N'Very likely')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 9, 3, 0, N'Somewhat likely')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 9, 4, 0, N'Not so likely')
GO
INSERT [dbo].[SurveyOptions_Texts] ([Survey], [Question], [OptionId], [Language], [OptionText]) VALUES (2, 9, 5, 0, N'Not at all likely')
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 1, 0, N'First Card', NULL)
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 2, 0, N'Second Card', NULL)
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 3, 0, N'Third Card', NULL)
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 4, 0, N'Fourth Card', NULL)
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 5, 0, N'Fifth Card', NULL)
GO
INSERT [dbo].[SurveyPages_Texts] ([Survey], [PageId], [Language], [ShowTitle], [Description]) VALUES (2, 6, 0, N'The Last Page!', NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 1, 0, N'How likely is it that you would recommend this company to a friend or colleague', NULL, NULL, N'NOT AT ALL LIKELY', N'EXTREMELY LIKELY', NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 2, 0, N'Overall, how satisfied or dissatisfied are you with our company?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 3, 0, N'Which of the following words would you use to describe our products? Select all that apply.', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 4, 0, N'How well do our products meet your needs?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 5, 0, N'How would you rate the quality of the product?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 6, 0, N'How would you rate the value for money of the product?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 7, 0, N'How responsive have we been to your questions or concerns about our products?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 8, 0, N'How long have you been a customer of our company?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 9, 0, N'How likely are you to purchase any of our products again?', NULL, NULL, NULL, NULL, NULL, N'This question requires an answer.', NULL, NULL)
GO
INSERT [dbo].[SurveyQuestions_Texts] ([Survey], [QuestionId], [Language], [QuestionText], [Description], [HelpText], [FrontLabelText], [AfterLabelText], [InsideText], [RequiredMessage], [ValidationMessage], [OtherFieldLabel]) VALUES (2, 10, 0, N'Do you have any other comments, questions, or concerns?', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[Surveys_Texts] ([SurveyId], [Language], [ShowTitle], [HeaderHtml], [WelcomeHtml], [GoodbyeHtml], [FooterHtml], [DisqualificationHtml], [DisqualificationUrl], [OnCompletionUrl], [StartButton], [PreviousButton], [NextButton], [DoneButton]) VALUES (2, 0, NULL, N'<span style="font-size:16px"><strong>This is the ''customer satisfaction survey'', a demo survey</strong></span>', N'<div style="text-align: center;"><br />
<span style="font-family:tahoma,geneva,sans-serif"><span style="font-size:20px"><strong>WELCOME</strong>!<br />
You are going to fullfil the Customer Survey</span></span><br />
<br />
<span style="color:rgb(0, 0, 0); font-family:open sans,arial,sans-serif; font-size:14px">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Ut et risus in massa commodo aliquet nec in justo. Nullam quis risus sit amet nisl tempus ultrices tristique vitae urna. Sed placerat metus at eleifend elementum. Sed imperdiet elementum leo, in fermentum quam commodo eu. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Nunc dictum ullamcorper felis, sit amet accumsan ligula lacinia at. Donec tincidunt nulla vitae libero semper consequat. Nam auctor dapibus orci sed elementum.<br />
<br />
<strong><em><span style="font-size:20px">Press<br />
START!</span></em></strong></span></div>', N'', N'<span style="font-size:10px">Serviced to You by Your trusteed Compny 2020!</span>', N'', N'', N'', N'Start', N'Previous', N'Next', N'Submit')
GO
