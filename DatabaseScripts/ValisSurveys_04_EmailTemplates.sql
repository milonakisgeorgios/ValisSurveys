set nocount on
GO
USE [ValisSurveys]
GO

delete from [dbo].[EmailTemplates]

DBCC CHECKIDENT (EmailTemplates, reseed, 1)



SET IDENTITY_INSERT dbo.EmailTemplates ON
insert into [dbo].[EmailTemplates] ([TemplateId],[Name],[Sender],[Subject],[Body],[AttributeFlags],[CreationDT],[CreatedBy],[LastUpdateDT],[LastUpdatedBy]) values(1,N'SurveyInvitation',null,N'',N'',0,GETUTCDATE(), 0, GETUTCDATE(), 0);
insert into [dbo].[EmailTemplates] ([TemplateId],[Name],[Sender],[Subject],[Body],[AttributeFlags],[CreationDT],[CreatedBy],[LastUpdateDT],[LastUpdatedBy]) values(2,N'VerifySender',null,N'',N'',0,GETUTCDATE(), 0, GETUTCDATE(), 0);
SET IDENTITY_INSERT dbo.EmailTemplates OFF

update [dbo].[EmailTemplates] set 
	[Sender]=N'@useremail',
	[Subject]=N'Take My Survey',
	[Body]=N'
We are conducting a survey, and your response would be appreciated.

Here is a link to the survey:
[SurveyLink]

This link is uniquely tied to this survey and your email address. Please do not forward this message.

Thanks for your participation!

Please note: If you do not wish to receive further emails from us, please click the link below, and you will be automatically removed from our mailing list.
[RemoveLink]
'
where
	[TemplateId]=1


	
update [dbo].[EmailTemplates] set 
	[Sender]=N'@NoreplyEmail',
	[Subject]=N'@BrandName: Verify Your Reply-to Address',
	[Body]=N'
Hello,

To use @Sender as the reply-to address for an email collector in your @useremail @BrandName account, please click this link:
@VerifyReplyToLink

If you did not request this verification, you can ignore this message. If you''re worried that someone is trying to misuse your email address, please contact us at @SupportEmail for assistance.

Thanks,
The @SurveyTeam

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~






This e-mail was sent to @Sender and contains information directly related to your account with us. This is a one-time email and you do not need to unsubscribe.  
Please do not reply to this email. If you want to contact us, please contact our Customer Support. To update your communication preference, please log in. 

@EmailSignature
'
where
	[TemplateId]=2

