USE [ValisSupport]
GO
/****** Object:  Table [dbo].[WatchdogAO]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchdogAO]') AND type in (N'U'))
DROP TABLE [dbo].[WatchdogAO]
GO
/****** Object:  Table [dbo].[LogRecords]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogRecords]') AND type in (N'U'))
DROP TABLE [dbo].[LogRecords]
GO
/****** Object:  Table [dbo].[ApplicationServiceAO3]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO3]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationServiceAO3]
GO
/****** Object:  Table [dbo].[ApplicationServiceAO2]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO2]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationServiceAO2]
GO
/****** Object:  Table [dbo].[ApplicationServiceAO1]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO1]') AND type in (N'U'))
DROP TABLE [dbo].[ApplicationServiceAO1]
GO
/****** Object:  StoredProcedure [dbo].[valis_WatchdogAO_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_WatchdogAO_Heartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[valis_WatchdogAO_Heartbeat]
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO3_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO3_Heartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[valis_ApplicationServiceAO3_Heartbeat]
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO2_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO2_Heartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[valis_ApplicationServiceAO2_Heartbeat]
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO1_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO1_Heartbeat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[valis_ApplicationServiceAO1_Heartbeat]
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO1_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO1_Heartbeat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
	Project Name: ''ValisSurveys''
	Code Generated at: ''29/11/2014 7:01:51 πμ''
	Author: ''milonakisgeorgios'
	---------------------------------
*/
CREATE PROCEDURE [dbo].[valis_ApplicationServiceAO1_Heartbeat]
@currentTimeUtc datetime2(3)
as
set nocount on


	insert into [dbo].[ApplicationServiceAO1] ([HeartbeatDt]) values(@currentTimeUtc)


	delete from [dbo].[ApplicationServiceAO1]
	where [RowId] not in (select top 2000 RowId from [dbo].[ApplicationServiceAO1] order by RowId)
' 
END
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO2_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO2_Heartbeat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
	Project Name: ''ValisSurveys''
	Code Generated at: ''29/11/2014 7:05:43 πμ''
	Author: ''milonakisgeorgios'
	---------------------------------
*/
CREATE PROCEDURE [dbo].[valis_ApplicationServiceAO2_Heartbeat]
@currentTimeUtc datetime2(3)
as
set nocount on


	insert into [dbo].[ApplicationServiceAO2] ([HeartbeatDt]) values(@currentTimeUtc)


	delete from [dbo].[ApplicationServiceAO2]
	where [RowId] not in (select top 2000 RowId from [dbo].[ApplicationServiceAO2] order by RowId)
' 
END
GO
/****** Object:  StoredProcedure [dbo].[valis_ApplicationServiceAO3_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_ApplicationServiceAO3_Heartbeat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
	Project Name: ''ValisSurveys''
	Code Generated at: ''3/1/2015 11:03:13 πμ''
	Author: ''milonakisgeorgios'
	---------------------------------
*/
CREATE PROCEDURE [dbo].[valis_ApplicationServiceAO3_Heartbeat]
@currentTimeUtc datetime2(3)
as
set nocount on


	insert into [dbo].[ApplicationServiceAO3] ([HeartbeatDt]) values(@currentTimeUtc)


	delete from [dbo].[ApplicationServiceAO3]
	where [RowId] not in (select top 2000 RowId from [dbo].[ApplicationServiceAO3] order by RowId)
' 
END
GO
/****** Object:  StoredProcedure [dbo].[valis_WatchdogAO_Heartbeat]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[valis_WatchdogAO_Heartbeat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'/*
	Project Name: ''ValisSurveys''
	Code Generated at: ''29/11/2014 7:06:31 πμ''
	Author: ''milonakisgeorgios'
	---------------------------------
*/
CREATE PROCEDURE [dbo].[valis_WatchdogAO_Heartbeat]
@currentTimeUtc datetime2(3)
as
set nocount on


	insert into [dbo].[WatchdogAO] ([HeartbeatDt]) values(@currentTimeUtc)


	delete from [dbo].[WatchdogAO]
	where [RowId] not in (select top 2000 RowId from [dbo].[WatchdogAO] order by RowId)
' 
END
GO
/****** Object:  Table [dbo].[ApplicationServiceAO1]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO1]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ApplicationServiceAO1](
	[RowId] [bigint] IDENTITY(1,1) NOT NULL,
	[HeartbeatDt] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationServiceAO1] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ApplicationServiceAO2]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO2]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ApplicationServiceAO2](
	[RowId] [bigint] IDENTITY(1,1) NOT NULL,
	[HeartbeatDt] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationServiceAO2] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ApplicationServiceAO3]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ApplicationServiceAO3]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ApplicationServiceAO3](
	[RowId] [bigint] IDENTITY(1,1) NOT NULL,
	[HeartbeatDt] [datetime] NOT NULL,
 CONSTRAINT [PK_ApplicationServiceAO3] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[LogRecords]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogRecords]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LogRecords](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [nvarchar](255) NOT NULL,
	[Level] [nvarchar](50) NOT NULL,
	[Logger] [nvarchar](255) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Exception] [nvarchar](max) NULL,
 CONSTRAINT [PK_LogRecords] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[WatchdogAO]    Script Date: 3/1/2015 6:15:26 μμ ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WatchdogAO]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WatchdogAO](
	[RowId] [bigint] IDENTITY(1,1) NOT NULL,
	[HeartbeatDt] [datetime] NOT NULL,
 CONSTRAINT [PK_WatchdogAO] PRIMARY KEY CLUSTERED 
(
	[RowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
