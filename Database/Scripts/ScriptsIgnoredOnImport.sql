
USE [master]
GO

/****** Object:  Database [Terrarium]    Script Date: 05/14/2008 08:28:03 ******/
CREATE DATABASE [Terrarium] ON  PRIMARY 
( NAME = N'Terrarium_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Terrarium_Data.MDF' , SIZE = 120448KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'Terrarium_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Terrarium_Log.LDF' , SIZE = 470144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

EXEC dbo.sp_dbcmptlevel @dbname=N'Terrarium', @new_cmptlevel=110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Terrarium].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO

ALTER DATABASE [Terrarium] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [Terrarium] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

ALTER DATABASE [Terrarium] SET  READ_WRITE
GO

ALTER DATABASE [Terrarium] SET  MULTI_USER
GO

USE [Terrarium]
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'TerrariumUser')
EXEC sys.sp_executesql N'CREATE SCHEMA [TerrariumUser] AUTHORIZATION [dbo]'
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ----------------------------------------------------------
--
-- Create Tables
--
-- ----------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Watson]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Watson.
--CREATE TABLE [dbo].[Watson](
--	[id] [int] IDENTITY(1,1) NOT NULL,
--	[LogType] [varchar](50) NULL,
--	[MachineName] [varchar](255) NULL,
--	[OSVersion] [varchar](50) NULL,
--	[GameVersion] [varchar](50) NULL,
--	[CLRVersion] [varchar](50) NULL,
--	[ErrorLog] [text] NULL,
--	[UserEmail] [varchar](255) NULL,
--	[UserComment] [text] NULL,
--	[DateSubmitted] [datetime] NULL
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsageSummary]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.UsageSummary.
--CREATE TABLE [dbo].[UsageSummary](
--	[Peers] [int] NOT NULL,
--	[SummaryDateTime] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usage]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Usage.
--CREATE TABLE [dbo].[Usage](
--	[Alias] [varchar](255) NOT NULL,
--	[Domain] [varchar](255) NOT NULL,
--	[TickTime] [datetime] NOT NULL,
--	[UsageMinutes] [int] NOT NULL,
--	[IPAddress] [varchar](25) NOT NULL,
--	[GameVersion] [varchar](255) NOT NULL,
--	[PeerChannel] [varchar](255) NOT NULL,
--	[PeerCount] [int] NOT NULL,
--	[AnimalCount] [int] NOT NULL,
--	[MaxAnimalCount] [int] NOT NULL,
--	[WorldHeight] [int] NOT NULL,
--	[WorldWidth] [int] NOT NULL,
--	[MachineName] [varchar](255) NOT NULL,
--	[OSVersion] [varchar](255) NOT NULL,
--	[ProcessorCount] [int] NOT NULL,
--	[ClrVersion] [varchar](255) NOT NULL,
--	[WorkingSet] [int] NOT NULL,
--	[MaxWorkingSet] [int] NOT NULL,
--	[MinWorkingSet] [int] NOT NULL,
--	[ProcessorTime] [int] NOT NULL,
--	[ProcessStartTime] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PumTeam]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.PumTeam.
--CREATE TABLE [dbo].[PumTeam](
--	[PumId] [int] NOT NULL,
--	[Alias] [varchar](255) NOT NULL,
--	[ManagerAlias] [varchar](255) NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Pum]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Pum.
--CREATE TABLE [dbo].[Pum](
--	[Id] [int] IDENTITY(1,1) NOT NULL,
--	[Name] [varchar](255) NOT NULL,
--	[Alias] [varchar](255) NOT NULL,
--	[TeamName] [varchar](255) NOT NULL,
-- CONSTRAINT [PK_Pum] PRIMARY KEY CLUSTERED 
--(
--	[Id] ASC
--)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DailyPopulation]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.DailyPopulation.
--CREATE TABLE [dbo].[DailyPopulation](
--	[SampleDateTime] [datetime] NOT NULL,
--	[SpeciesName] [varchar](255) NOT NULL,
--	[Population] [int] NOT NULL,
--	[BirthCount] [int] NOT NULL,
--	[StarvedCount] [int] NOT NULL,
--	[KilledCount] [int] NOT NULL,
--	[ErrorCount] [int] NOT NULL,
--	[TimeoutCount] [int] NOT NULL,
--	[SickCount] [int] NOT NULL,
--	[OldAgeCount] [int] NOT NULL,
--	[SecurityViolationCount] [int] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Downloads]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Downloads.
--CREATE TABLE [dbo].[Downloads](
--	[Filename] [varchar](255) NOT NULL,
--	[DownloadCount] [int] NOT NULL,
--	[LastDownloadDate] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[History]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.History.
--CREATE TABLE [dbo].[History](
--	[GUID] [uniqueidentifier] NOT NULL,
--	[TickNumber] [int] NOT NULL,
--	[SpeciesName] [varchar](255) NOT NULL,
--	[ContactTime] [datetime] NOT NULL,
--	[ClientTime] [datetime] NOT NULL,
--	[CorrectTime] [tinyint] NOT NULL,
--	[Population] [int] NOT NULL,
--	[BirthCount] [int] NOT NULL,
--	[TeleportedToCount] [int] NOT NULL,
--	[StarvedCount] [int] NOT NULL,
--	[KilledCount] [int] NOT NULL,
--	[TeleportedFromCount] [int] NOT NULL,
--	[ErrorCount] [int] NOT NULL,
--	[TimeoutCount] [int] NOT NULL,
--	[SickCount] [int] NOT NULL,
--	[OldAgeCount] [int] NOT NULL,
--	[SecurityViolationCount] [int] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[NodeLastContact]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.NodeLastContact.
--CREATE TABLE [dbo].[NodeLastContact](
--	[GUID] [uniqueidentifier] NOT NULL,
--	[LastTick] [int] NOT NULL,
--	[LastContact] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Peers]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Peers.
--CREATE TABLE [dbo].[Peers](
--	[Channel] [varchar](32) NOT NULL,
--	[IPAddress] [varchar](16) NOT NULL,
--	[Lease] [datetime] NOT NULL,
--	[Version] [varchar](16) NOT NULL,
--	[Guid] [uniqueidentifier] NULL,
--	[FirstContact] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RandomTips]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.RandomTips.
--CREATE TABLE [dbo].[RandomTips](
--	[id] [int] IDENTITY(1,1) NOT NULL,
--	[tip] [varchar](512) NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShutdownPeers]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.ShutdownPeers.
--CREATE TABLE [dbo].[ShutdownPeers](
--	[Guid] [uniqueidentifier] NULL,
--	[Channel] [varchar](255) NOT NULL,
--	[IPAddress] [varchar](50) NOT NULL,
--	[Version] [varchar](255) NOT NULL,
--	[FirstContact] [datetime] NOT NULL,
--	[LastContact] [datetime] NOT NULL,
--	[UnRegister] [bit] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Species]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.Species.
--CREATE TABLE [dbo].[Species](
--	[Name] [varchar](255) NOT NULL,
--	[Type] [varchar](50) NOT NULL,
--	[Author] [varchar](255) NOT NULL,
--	[AuthorEmail] [varchar](255) NOT NULL,
--	[DateAdded] [datetime] NOT NULL,
--	[AssemblyFullName] [text] NOT NULL,
--	[Extinct] [tinyint] NOT NULL,
--	[LastReintroduction] [datetime] NULL,
--	[ReintroductionNode] [uniqueidentifier] NULL,
--	[Version] [varchar](255) NOT NULL,
--	[BlackListed] [bit] NOT NULL,
--	[ExtinctDate] [datetime] NULL
--) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TimedOutNodes]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.TimedOutNodes.
--CREATE TABLE [dbo].[TimedOutNodes](
--	[GUID] [uniqueidentifier] NOT NULL,
--	[TimeoutDate] [datetime] NOT NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRegister]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.UserRegister.
--CREATE TABLE [dbo].[UserRegister](
--	[IPAddress] [varchar](50) NOT NULL,
--	[Email] [varchar](255) NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[VersionedSettings]') AND type in (N'U'))
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.VersionedSettings.
--CREATE TABLE [dbo].[VersionedSettings](
--	[Version] [varchar](255) NOT NULL,
--	[Disabled] [int] NOT NULL,
--	[Message] [varchar](255) NULL
--) ON [PRIMARY]

END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ----------------------------------------------------------
--
-- Create Default Data
--
-- ----------------------------------------------------------
INSERT INTO VersionedSettings (Version, Disabled, Message) 
VALUES						  ('Default',0,'')
GO

INSERT INTO RandomTips 
VALUES ('You can use Alt-Enter to enter a true Full-Screen view.')
GO

-- ----------------------------------------------------------
--
-- Create Stored Procedures
--
-- ----------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Web_GetTips]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.Web_GetTips    Script Date: 1/4/2005 6:12:18 PM ******/
create procedure [dbo].[Web_GetTips]
as

select id, tip from tips



' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumInsertWatson]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumInsertWatson    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumInsertWatson    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumInsertWatson]
    @LogType varchar(50),
    @MachineName varchar(255),
    @OSVersion varchar(50),
    @GameVersion varchar(50),
    @CLRVersion varchar(50),
    @ErrorLog text,
    @UserComment text,
    @UserEmail varchar(255)
AS
    INSERT INTO
        Watson(
            [LogType],
            [MachineName],
            [OSVersion],
            [GameVersion],
            [CLRVersion],
            [ErrorLog],
            [UserComment],
            [UserEmail]
        )
        VALUES(
            @LogType,
            @MachineName,
            @OSVersion,
            @GameVersion,
            @CLRVersion,
            @ErrorLog,
            @UserComment,
            @UserEmail
        )

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumReportUsageSummary]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE procedure [dbo].[TerrariumReportUsageSummary]

as

declare @Peers int

select @Peers = (select count(*) from Peers)

insert	UsageSummary
       (Peers,
    SummaryDateTime)
values (@Peers,
    GETDATE())

    
' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumReportUsage]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE  procedure [dbo].[TerrariumReportUsage]
    @Alias varchar(255),
    @Domain varchar(255),
    @IPAddress varchar(25),
    @GameVersion varchar(255),
    @PeerChannel varchar(255),
    @PeerCount int,
    @AnimalCount int,
    @MaxAnimalCount int,
    @WorldWidth int,
    @WorldHeight int,
    @MachineName varchar(255),
    @OSVersion varchar(255),
    @ProcessorCount int,
    @ClrVersion varchar(255),
    @WorkingSet int,
    @MaxWorkingSet int,
    @MinWorkingSet int,
    @ProcessorTime int,
    @ProcessStartTime datetime

as 

declare @LastTickTime datetime
declare @CurrentTickTime datetime

select @CurrentTickTime = GETDATE()

select @LastTickTime = (select TOP 1 TickTime From Usage where Alias = @Alias order by TickTime desc)
if (@LastTickTime is null)
    select @LastTickTime = ''01/01/1901''

if (DATEDIFF(minute,@LastTickTime, @CurrentTickTime) >= 60)
    begin
    
        insert	Usage
        values (@Alias,
            @Domain,
            @CurrentTickTime,
            60,
            @IPAddress,
            @GameVersion,
            @PeerChannel,
            @PeerCount,
            @AnimalCount,
            @MaxAnimalCount,
            @WorldHeight,
            @WorldWidth,
            @MachineName,
            @OSVersion,
            @ProcessorCount,
            @ClrVersion,
            @WorkingSet,
            @MaxWorkingSet,
            @MinWorkingSet,
            @ProcessorTime,
            @ProcessStartTime)
    end
else
    begin
        return 50000
    end

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumTopAnimals]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumTopAnimals    Script Date: 1/4/2005 6:12:18 PM ******/
CREATE PROCEDURE [dbo].[TerrariumTopAnimals]

    @Count		int,
    @Version	varchar(25),
    @SpeciesType	varchar(50)

AS

SET ROWCOUNT @Count

SELECT	Species.Name As Name, 
    Species.Author As AuthorName, 
    DailyPopulation.Population Population 
FROM	SPECIES INNER JOIN DailyPopulation ON (Species.Name = DailyPopulation.SpeciesName)
WHERE 	DailyPopulation.SampleDateTime = (SELECT Max(SampleDateTime) FROM DailyPopulation)
    AND Species.Version = @Version 
    AND Species.Type = @SpeciesType 
    AND DailyPopulation.SecurityViolationCount = 0 
    ORDER BY DailyPopulation.Population DESC

SET ROWCOUNT 0

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabLatestSpeciesData]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabLatestSpeciesData    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabLatestSpeciesData    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabLatestSpeciesData] (
    @SpeciesName varchar(50)
)
AS
    SELECT
        *
    FROM
        DailyPopulation
    WHERE
        SpeciesName = @SpeciesName AND
        SampleDateTime = (
            SELECT
                Max(SampleDateTime)
            FROM
                DailyPopulation
            WHERE
                SpeciesName = @SpeciesName
        )

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabSpeciesDataInDateRange]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesDataInDateRange    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesDataInDateRange    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabSpeciesDataInDateRange] (
    @SpeciesName varchar(50),
    @BeginDate datetime,
    @EndDate datetime
)
AS
    SELECT
        *
    FROM
        DailyPopulation
    WHERE
        SpeciesName = @SpeciesName AND
        SampleDateTime BETWEEN @BeginDate AND @EndDate

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabSpeciesInfo]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesInfo    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabSpeciesInfo    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE   PROCEDURE [dbo].[TerrariumGrabSpeciesInfo]
AS
    SELECT
        Species.Name As SpeciesName,
        Species.Author As AuthorName,
        Species.AuthorEmail As AuthorEmail,
        Species.Version As Version,
        DailyPopulation.Population As Population,
        Species.Type As Type
    FROM
        Species INNER JOIN
        DailyPopulation ON (Species.Name = DailyPopulation.SpeciesName)
    WHERE
        DailyPopulation.SampleDateTime = (
            SELECT
                Max(SampleDateTime)
            FROM
                DailyPopulation
            WHERE
                DailyPopulation.SpeciesName = Species.Name
        ) AND
        BlackListed=0

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumAggregate]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumAggregate    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumAggregate    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE          PROCEDURE [dbo].[TerrariumAggregate] (
    @Expiration_Error int out,
    @Rollup_Error int out,
    @Timeout_Add_Error int out,
    @Timeout_Delete_Error int out,
    @Extinction_Error int out
)
AS
-- Timing Code
    -- Declarations
    DECLARE @Last datetime;
    DECLARE @Now datetime;
    DECLARE @Timeout datetime;
    -- Initialization
    SELECT @Last=Max(SampleDateTime) FROM DailyPopulation
    SET @Now = GETUTCDATE();
    SET @Timeout = DATEADD(hh, -48, @Now);
BEGIN TRANSACTION
-- Expire Peer Leases
    INSERT INTO
        ShutdownPeers (
            Guid,
            Channel,
            IPAddress,
            FirstContact,
            LastContact,
            Version,
            UnRegister
        )
    SELECT
        Guid,
        Channel,
        IPAddress,
        FirstContact,
        GETUTCDATE(),
        Version,
        0
    FROM
        Peers
    WHERE
        Lease < @Now
    DELETE FROM
        Peers
    WHERE
        Lease < @Now
    SET @Expiration_Error = @@ERROR;
IF @Expiration_Error = 0
    COMMIT TRAN
ELSE
    ROLLBACK TRAN
-- TRANSACTION 1
-- Reporting and Timeouts
BEGIN TRANSACTION
    -- Timeouts
        -- Add Timed Out Nodes
        INSERT INTO
            TimedOutNodes (
                GUID,
                TimeOutDate
            )
        SELECT
            GUID,
            @Timeout As Expr1
        FROM
            NodeLastContact
        WHERE
            LastContact < @Timeout
        SET @Timeout_Add_Error = @@ERROR;
        -- Removed Timed Out Nodes from Current Nodes
        DELETE FROM
            NodeLastContact
        WHERE
            GUID IN (
                SELECT
                    TimedOutNodes.GUID
                FROM
                    NodeLastContact INNER JOIN
                    TimedOutNodes ON
                        (NodeLastContact.GUID = TimedOutNodes.GUID)
            )
        SET @Timeout_Delete_Error = @@ERROR;
    IF @Timeout_Delete_Error = 0
        COMMIT TRAN
    ELSE
        ROLLBACK TRAN
-- TRANSACTION 2
BEGIN TRANSACTION
    -- Mark Extinct Animals
        CREATE TABLE #ExtinctSpecies (Name varchar(250) collate SQL_Latin1_General_CP1_CI_AS)
        IF @@ERROR = 0
        BEGIN
                INSERT INTO
                        #ExtinctSpecies
                SELECT
                    Name
                FROM
                    Species
                        WHERE
                            Species.Name NOT IN (
                                SELECT
                                    SpeciesName
                                FROM
                                    DailyPopulation
                                WHERE
                                    SampleDateTime=(SELECT Max(SampleDateTime) FROM DailyPopulation)
                            ) AND
                            Species.Name NOT IN (
                                SELECT
                                    Name
                                FROM
                                    Species INNER JOIN
                                    NodeLastContact ON
                                        (Species.ReintroductionNode = NodeLastContact.GUID)
                                WHERE
                                    NodeLastContact.LastContact < Species.LastReintroduction
                            ) AND
                            Species.Name NOT IN (
                                SELECT DISTINCT
                                        SpeciesName
                                FROM
                                        History
                            ) AND
                        Species.DateAdded < @Last AND
                        Extinct = 0;
                IF @@ERROR = 0
                BEGIN
                        UPDATE
                                Species
                        SET
                                Extinct = 1,
                                Species.ExtinctDate = @Now
                        WHERE
                                Name IN (
                                        SELECT Name From #ExtinctSpecies
                                );
                        IF @@ERROR = 0
                        BEGIN
                            INSERT INTO
                                        DailyPopulation (
                                            SampleDateTime,
                                            SpeciesName,
                                            Population,
                                            BirthCount,
                                            StarvedCount,
                                            KilledCount,
                                            ErrorCount,
                                            SecurityViolationCount,
                                            TimeoutCount,
                                            SickCount,
                                            OldAgeCount
                                        )
                                SELECT
                                        @Now As Expr1,
                                        Name, 0, 0, 0, 0, 0, 0, 0, 0, 0
                                FROM
                                        #ExtinctSpecies    
                        END
                END
        END
        SET @Extinction_Error = @@ERROR;
    IF @Extinction_Error = 0
        COMMIT TRAN
    ELSE
        ROLLBACK TRAN
        -- Mark Non Extinct Animals
        UPDATE
                Species
        SET
                Species.Extinct = 0
        WHERE
                Species.Name IN (SELECT DISTINCT SpeciesName From History)
-- TRANSACTION 3
    -- Rollup
    INSERT INTO
        DailyPopulation (
            SampleDateTime,
            SpeciesName,
            Population,
            BirthCount,
            StarvedCount,
            KilledCount,
            ErrorCount,
            SecurityViolationCount,
            TimeoutCount,
            SickCount,
            OldAgeCount
        )
    SELECT
        @Now As Expr1,
        History.SpeciesName,
        Sum(History.Population) As SumOfPopulation,
        Sum(History.BirthCount) As SumOfBirthCount,
        Sum(History.StarvedCount) As SumOfStarvedCount,
        Sum(History.KilledCount) As SumOfKilledCount,
        Sum(History.ErrorCount) As SumOfErrors,
        Sum(History.SecurityViolationCount) As SumOfSecurityViolations,
        Sum(History.TimeoutCount) As SumOfTimeouts,
        Sum(History.SickCount) As SumOfSickCount,
        Sum(History.OldAgeCount) As SumOfOldAgeCount
    FROM
        NodeLastContact INNER JOIN
        History ON
            (NodeLastContact.LastTick = History.TickNumber) AND
            (NodeLastContact.GUID = History.GUID) INNER JOIN
        Species ON
            (Species.Name = History.SpeciesName)
    WHERE
        Species.Extinct = 0
    GROUP BY
        History.SpeciesName
    SET @Rollup_Error = @@ERROR;
        DELETE
                History
        FROM
                NodeLastContact INNER JOIN
                History ON (
                        NodeLastContact.LastTick > History.TickNumber AND
                        NodeLastContact.GUID = History.GUID
                )

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumCountDownload]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumCountDownload    Script Date: 1/4/2005 6:12:18 PM ******/
CREATE procedure [dbo].[TerrariumCountDownload]

@Filename varchar(255)

as

declare @DownloadCount	int;

select @DownloadCount = (SELECT DownloadCount FROM Downloads WHERE Filename = @Filename)

select @DownloadCount = @DownloadCount + 1

IF EXISTS( SELECT Filename FROM Downloads WHERE Filename = @Filename)
    BEGIN
        UPDATE 	Downloads
        SET	DownloadCount = @DownloadCount,
            LastDownloadDate = GETDATE()
        WHERE	Filename = @Filename
    END
ELSE
    BEGIN
        INSERT 	Downloads
        VALUES (@FileName,
            1,
            GETDATE())
    END

SELECT DownloadCount = @DownloadCount


' 
END
GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumInsertHistory]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumInsertHistory    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumInsertHistory    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumInsertHistory]
    @Guid uniqueidentifier,
    @TickNumber int,
    @SpeciesName varchar(255),
    @ContactTime datetime,
    @ClientTime datetime,
    @CorrectTime tinyint,
    @Population int,
    @BirthCount int,
    @TeleportedToCount int,
    @StarvedCount int,
    @KilledCount int,
    @TeleportedFromCount int,
    @ErrorCount int,
    @TimeoutCount int,
    @SickCount int,
    @OldAgeCount int,
    @SecurityViolationCount int,
    @BlackListed int out
AS
    SELECT
        @BlackListed=BlackListed
    FROM
        Species
    WHERE
        Name=@SpeciesName
    INSERT INTO
        History(
            GUID,
            TickNumber,
            SpeciesName,
            ContactTime,
            ClientTime,
            CorrectTime,
            Population,
            BirthCount,
            TeleportedToCount,
            StarvedCount,
            KilledCount,
            TeleportedFromCount,
            ErrorCount,
            TimeoutCount,
            SickCount,
            OldAgeCount,
            SecurityViolationCount
        )
        VALUES(
            @Guid,
            @TickNumber,
            @SpeciesName,
            @ContactTime,
            @ClientTime,
            @CorrectTime,
            @Population,
            @BirthCount,
            @TeleportedToCount,
            @StarvedCount,
            @KilledCount,
            @TeleportedFromCount,
            @ErrorCount,
            @TimeoutCount,
            @SickCount,
            @OldAgeCount,
            @SecurityViolationCount
        )

' 
END
GO

SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumTimeoutReport]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumTimeoutReport    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumTimeoutReport    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumTimeoutReport]
    @Guid uniqueidentifier,
    @LastContact datetime,
    @LastTick int,
    @ReturnCode int output
AS
    Declare @MaxTick int
    Select @ReturnCode=0
    SELECT
        @MaxTick=Max(LastTick)
    FROM
        NodeLastContact
    WHERE
        Guid=@Guid
    -- Check to see if we have a record in the NodeLastContact
    -- Table
    IF @MaxTick IS NOT NULL
        BEGIN
            -- Now check to see if that record is newer
            -- than the current record from the client
            IF @MaxTick >= @LastTick
                BEGIN
                    Select @ReturnCode = 2
                END
            ELSE
                BEGIN
                    UPDATE
                        NodeLastContact
                    SET
                        LastTick = @LastTick,
                        LastContact = @LastContact
                    WHERE
                        Guid=@Guid
                END
        END
    ELSE
        BEGIN
            DECLARE @TimeoutCount datetime
            SELECT
                @TimeoutCount=Max(TimeoutDate)
            FROM
                TimedOutNodes
            WHERE
                Guid=@Guid
            IF @TimeoutCount IS NULL
                BEGIN
                    INSERT INTO
                        NodeLastContact(
                            LastTick,
                            LastContact,
                            Guid
                        )
                        VALUES(
                            @LastTick,
                            @LastContact,
                            @Guid
                        )
                END
            ELSE
                BEGIN
                    Select @ReturnCode=1
                END
        END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumDeletePeer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumDeletePeer    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumDeletePeer    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumDeletePeer]
    @Channel varchar(255),
    @IPAddress varchar(50),
    @Guid uniqueidentifier
AS
    INSERT INTO
        ShutdownPeers (
            Guid,
            Channel,
            IPAddress,
            FirstContact,
            LastContact,
            Version,
            UnRegister
        )
    SELECT
        Guid,
        Channel,
        IPAddress,
        FirstContact,
        GETUTCDATE(),
        Version,
        1
    FROM
        Peers
    WHERE
        Channel = @Channel AND
        IPAddress = @IPAddress AND
        Guid = @Guid
    DELETE FROM
        Peers
    WHERE
        Channel = @Channel AND
        IPAddress = @IPAddress AND
        Guid = @Guid

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabAllPeers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabAllPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllPeers]
    @Version varchar(255),
    @Channel varchar(255)
AS
    SELECT
        *
    FROM
        Peers
    WHERE
        Version=@Version AND
        Channel=@Channel

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumRegisterPeer]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumRegisterPeer    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumRegisterPeer    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE     PROCEDURE [dbo].[TerrariumRegisterPeer]
    @Version varchar(255),
    @FullVersion varchar(255),
    @Channel varchar(255),
    @IPAddress varchar(50),
    @Guid uniqueidentifier,
    @Disabled_Error bit output
AS
    DECLARE @Disabled bit;
    DECLARE @Lease datetime;
    SET @Lease = DATEADD(mi, 15, GETUTCDATE());
    SELECT
        @Disabled=Disabled
    FROM
        VersionedSettings
    WHERE
        Version=@FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                @Disabled=Disabled
            FROM
                VersionedSettings
            WHERE
                Version=@Version
            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT
                        @Disabled=Disabled
                    FROM    
                        VersionedSettings
                    WHERE
                        Version=''Default''
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @Version,
                            @Disabled
                        )
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
            ELSE
                BEGIN
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
        END
    IF @Disabled = 1
        BEGIN
            Set @Disabled_Error = 1;
        END
    ELSE
        BEGIN
            SELECT
                NULL
            FROM
                Peers
            WHERE
                Channel = @Channel AND
                IPAddress = @IPAddress
        
            IF @@ROWCOUNT = 0
                BEGIN
                    INSERT INTO
                        Peers(
                            Channel,
                            Version,
                            IPAddress,
                            Lease,
                            Guid,
                            FirstContact
                        )
                        VALUES(
                            @Channel,
                            @Version,
                            @IPAddress,
                            @Lease,
                            @Guid,
                            GETUTCDATE()
                        )
                END
            ELSE
                BEGIN
                    UPDATE
                        Peers
                    SET
                        Lease = @Lease,
                        Version = @Version
                    WHERE
                        Channel = @Channel AND
                        IPAddress = @IPAddress
                END
            Set @Disabled_Error = 0
        END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumRegisterPeerCountAndList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumRegisterPeerCountAndList    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumRegisterPeerCountAndList    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE        PROCEDURE [dbo].[TerrariumRegisterPeerCountAndList]
    @Version varchar(255),
    @FullVersion varchar(255),
    @Channel varchar(255),
    @IPAddress varchar(50),
    @Guid uniqueidentifier,
    @Disabled_Error bit output,
    @PeerCount int output
AS
    DECLARE @Disabled bit;
    DECLARE @Lease datetime;
    DECLARE @Total int
    DECLARE @BelowCount int
    DECLARE @AboveCount int
    SELECT @PeerCount=0;
    SET @Lease = DATEADD(mi, 15, GETUTCDATE());
    SELECT
        @Disabled=Disabled
    FROM
        VersionedSettings
    WHERE
        Version=@FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT
                @Disabled=Disabled
            FROM
                VersionedSettings
            WHERE
                Version=@Version
            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT
                        @Disabled=Disabled
                    FROM    
                        VersionedSettings
                    WHERE
                        Version=''Default''
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @Version,
                            @Disabled
                        )
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
            ELSE
                BEGIN
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
        END
    IF @Disabled = 1
        BEGIN
            Set @Disabled_Error = 1;
            SELECT TOP 0
                Lease,IPAddress
            FROM
                Peers
        END
    ELSE
        BEGIN
            DECLARE @Self int;
            SELECT
                @Self=count(*)
            FROM
                Peers
            WHERE
                Channel = @Channel AND
                IPAddress = @IPAddress
        
            IF @Self = 0
                BEGIN
                    BEGIN TRAN
                    INSERT INTO
                        Peers(
                            Channel,
                            Version,
                            IPAddress,
                            Lease,
                            Guid,
                            FirstContact
                        )
                        VALUES(
                            @Channel,
                            @Version,
                            @IPAddress,
                            @Lease,
                            @Guid,
                            GETUTCDATE()
                        )
                    COMMIT TRAN
                END
            ELSE
                BEGIN
                    BEGIN TRAN
                    UPDATE
                        Peers
                    SET
                        Lease = @Lease,
                        Version = @Version
                    WHERE
                        Channel = @Channel AND
                        IPAddress = @IPAddress
                    COMMIT TRAN
                END
            Set @Disabled_Error = 0
        SELECT
        @PeerCount=Count(*)
        FROM
            Peers
        WHERE
            Version=@Version AND
            Channel=@Channel
            SELECT @Total=count(*) FROM Peers WHERE Version=@Version AND Channel=@Channel
            IF @Total > 30
                BEGIN
                    SELECT @AboveCount=count(*)
                    FROM Peers
                    WHERE Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress
        
                    SELECT @BelowCount=count(*)
                    FROM Peers
                    WHERE Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress
        
                    IF @BelowCount < 10
                        BEGIN
                            SELECT IPAddress,Lease
                            FROM Peers
                            WHERE Version=@Version AND Channel=@Channel AND
                                (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                                 IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                                     IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<''255.255.255.255'' ORDER BY IPAddress DESC)
                                )
                        END
                    ELSE
                        BEGIN
                            IF @AboveCount < 10
                                BEGIN
                                    SELECT IPAddress,Lease
                                    FROM Peers
                                    WHERE Version=@Version AND Channel=@Channel AND
                                        (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                                         IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                         IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>''0.0.0.0'' ORDER BY IPAddress ASC)
                                        )
                                END
                            ELSE
                                BEGIN
                                    SELECT IPAddress,Lease
                                    FROM Peers
                                    WHERE Version=@Version AND Channel=@Channel AND
                                        (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                                         IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC)
                                        )
                                END
                        END
                END
            ELSE
                BEGIN
                    SELECT IPAddress, Lease FROM Peers WHERE Version=@Version AND Channel=@Channel
                END
        END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabNumPeers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabNumPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabNumPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumGrabNumPeers]
    @Version varchar(255),
    @Channel varchar(255)
AS
    SELECT
        Count(*)
    FROM
        Peers
    WHERE
        Version=@Version AND
        Channel=@Channel

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabScaledPeers]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabScaledPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabScaledPeers    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabScaledPeers]
    @Version varchar(255),
    @Channel varchar(255),
    @IPAddress varchar(50)
AS
DECLARE @Total int
DECLARE @BelowCount int
DECLARE @AboveCount int
SELECT @Total=count(*) FROM Peers WHERE Version=@Version AND Channel=@Channel
IF @Total > 30
    BEGIN
        SELECT @AboveCount=count(*)
        FROM Peers
        WHERE Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress
        
        SELECT @BelowCount=count(*)
        FROM Peers
        WHERE Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress
        
        IF @BelowCount < 10
            BEGIN
                SELECT IPAddress,Lease
                FROM Peers
                WHERE Version=@Version AND Channel=@Channel AND
                    (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                     IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                         IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<''255.255.255.255'' ORDER BY IPAddress DESC)
                    )
            END
        ELSE
            BEGIN
                IF @AboveCount < 10
                    BEGIN
                        SELECT IPAddress,Lease
                        FROM Peers
                        WHERE Version=@Version AND Channel=@Channel AND
                            (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                             IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC) OR
                                                 IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>''0.0.0.0'' ORDER BY IPAddress ASC)
                            )
                    END
                ELSE
                    BEGIN
                        SELECT IPAddress,Lease
                        FROM Peers
                        WHERE Version=@Version AND Channel=@Channel AND
                            (IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress>@IPAddress ORDER BY IPAddress ASC) OR
                             IPAddress IN (select top 10 IPAddress from Peers Where Version=@Version AND Channel=@Channel AND IPAddress<@IPAddress ORDER BY IPAddress DESC)
                            )
                    END
            END
    END
ELSE
    BEGIN
        SELECT IPAddress, Lease FROM Peers WHERE Version=@Version AND Channel=@Channel
    END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumCheckSpeciesBlackList]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesBlackList    Script Date: 1/4/2005 6:12:18 PM ******/
create procedure [dbo].[TerrariumCheckSpeciesBlackList]

@Name	varchar(255)

AS

    SELECT	Blacklisted
    FROM	Species
    WHERE	Name = @Name


' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumCheckSpeciesExtinct]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesExtinct    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumCheckSpeciesExtinct    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumCheckSpeciesExtinct]
    @Name varchar(255)
AS
    SELECT
        Count(Name)
    FROM
        Species
    WHERE
        Name=@Name
            AND
        Extinct = 1
            AND
        BlackListed = 0

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabAllRecentSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabAllRecentSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllRecentSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllRecentSpecies]
    @Version varchar(255)
AS
    DECLARE @RecentDate datetime
    SELECT @RecentDate = DATEADD(dd, -5, getutcdate());
    
    SELECT
        Name,
        Type,
        Author,
        AuthorEmail,
        DateAdded,
        AssemblyFullName,
        Extinct,
        LastReintroduction,
        ReintroductionNode,
        Version,
        BlackListed
    FROM
        Species
    WHERE
        Version=@Version AND
        BlackListed=0 AND
        (
                (ExtinctDate > @RecentDate AND Extinct = 1) OR
                Extinct = 0
        )

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabAllSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabAllSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabAllSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabAllSpecies]
    @Version varchar(255)
AS
    SELECT
        Name,
        Type,
        Author,
        AuthorEmail,
        DateAdded,
        AssemblyFullName,
        Extinct,
        LastReintroduction,
        ReintroductionNode,
        Version,
        BlackListed
    FROM
        Species
    WHERE
        Version=@Version
            AND
        BlackListed=0

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabExtinctRecentSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctRecentSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctRecentSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabExtinctRecentSpecies]
    @Version varchar(255)
AS
    DECLARE @RecentDate datetime
    SELECT @RecentDate = DATEADD(dd, -5, getutcdate());
    SELECT
        Name,
        Type,
        Author,
        AuthorEmail,
        DateAdded,
        AssemblyFullName,
        Extinct,
        LastReintroduction,
        ReintroductionNode,
        Version,
        BlackListed
    FROM
        Species
    WHERE
        Version=@Version AND
        Extinct=1 AND
        BlackListed=0 AND
        ExtinctDate > @RecentDate

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumGrabExtinctSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabExtinctSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumGrabExtinctSpecies]
    @Version varchar(255)
AS
    SELECT
        Name,
        Type,
        Author,
        AuthorEmail,
        DateAdded,
        AssemblyFullName,
        Extinct,
        LastReintroduction,
        ReintroductionNode,
        Version,
        BlackListed
    FROM
        Species
    WHERE
        Version=@Version AND
        Extinct=1 AND
        BlackListed=0

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumInsertSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumInsertSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumInsertSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumInsertSpecies]
    @Name varchar(255),
    @Version varchar(255),
    @Type varchar(50),
    @Author varchar(255),
    @AuthorEmail varchar(255),
    @Extinct tinyint,
    @DateAdded datetime,
    @AssemblyFullName text,
    @BlackListed bit
AS
    INSERT INTO
        Species(
            [Name],
            [Version],
            [Type],
            [Author],
            [AuthorEmail],
            [Extinct],
            [DateAdded],
            [AssemblyFullName],
            [BlackListed]
        )
        VALUES(
            @Name,
            @Version,
            @Type,
            @Author,
            @AuthorEmail,
            @Extinct,
            @DateAdded,
            @AssemblyFullName,
            @BlackListed
        )

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumReintroduceSpecies]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumReintroduceSpecies    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumReintroduceSpecies    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumReintroduceSpecies]
    @Name varchar(255),
    @ReintroductionNode uniqueidentifier,
    @LastReintroduction datetime
AS
    UPDATE
        Species
    SET
        Extinct = 0,
        ReintroductionNode=@ReintroductionNode,
        LastReintroduction=@LastReintroduction
    WHERE
        Name = @Name;

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumRegisterUser]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumRegisterUser    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumRegisterUser    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE  PROCEDURE [dbo].[TerrariumRegisterUser]
    @Email varchar(255),
    @IPAddress varchar(50)
AS
    SELECT
        NULL
    FROM
        UserRegister
    WHERE
        IPAddress = @IPAddress
    IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO
                UserRegister(
                    IPAddress,
                    Email
                )
                VALUES(
                    @IPAddress,
                    @Email
                )
        END
    ELSE
        BEGIN
            UPDATE
                UserRegister
            SET
                Email = @Email
            WHERE
                IPAddress = @IPAddress
        END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumIsVersionDisabled]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumIsVersionDisabled    Script Date: 1/4/2005 6:12:18 PM ******/

create  procedure [dbo].[TerrariumIsVersionDisabled]

    @Version	varchar(255),
    @FullVersion	varchar(255)
as

        DECLARE @Disabled bit;
    DECLARE @Message varchar(255);

    SELECT
        @Disabled=Disabled,
        @Message=Message
    FROM
        VersionedSettings
    WHERE
        Version=@FullVersion
    IF @@ROWCOUNT = 0
        BEGIN
            SELECT
        @Disabled=Disabled,
        @Message=Message
            FROM
                VersionedSettings
            WHERE
                Version=@Version
            IF @@ROWCOUNT = 0
                BEGIN
                    SELECT
            @Disabled=Disabled,
            @Message=Message
                    FROM    
                        VersionedSettings
                    WHERE
                        Version=''Default''
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @Version,
                            @Disabled
                        )
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
            ELSE
                BEGIN
                    INSERT INTO
                        VersionedSettings (
                            Version,
                            Disabled
                        ) VALUES (
                            @FullVersion,
                            @Disabled
                        )
                END
        END	

SELECT Disabled = @Disabled, Message = @Message
    


' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumAdminDisableAllVersions]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumAdminDisableAllVersions    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumAdminDisableAllVersions    Script Date: 11/8/2001 8:16:22 PM ******/
CREATE PROCEDURE [dbo].[TerrariumAdminDisableAllVersions]
AS
    UPDATE
        VersionedSettings
    SET
        Disabled=1

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumAdminDisableVersion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumAdminDisableVersion    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumAdminDisableVersion    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumAdminDisableVersion]
    @Version varchar(255)
AS
    SELECT
        *
    FROM
        VersionedSettings
    WHERE
        Version=@Version
    IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO
                VersionedSettings (
                    Version,
                    Disabled
                ) VALUES (
                    @Version,
                    1
                )
        END
    ELSE
        BEGIN
            UPDATE
                VersionedSettings
            SET
                Disabled=1
            WHERE
                Version=@Version
        END

' 
END
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER OFF
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TerrariumAdminEnableVersion]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
/****** Object:  Stored Procedure dbo.TerrariumAdminEnableVersion    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumAdminEnableVersion    Script Date: 11/8/2001 8:16:23 PM ******/
CREATE PROCEDURE [dbo].[TerrariumAdminEnableVersion]
    @Version varchar(255)
AS
    SELECT
        *
    FROM
        VersionedSettings
    WHERE
        Version=@Version
    IF @@ROWCOUNT = 0
        BEGIN
            INSERT INTO
                VersionedSettings (
                    Version,
                    Disabled
                ) VALUES (
                    @Version,
                    0
                )
        END
    ELSE
        BEGIN
            UPDATE
                VersionedSettings
            SET
                Disabled=0
            WHERE
                Version=@Version
        END

' 
END
GO

/****** Object:  StoredProcedure [dbo].[TerrariumGrabTotalNumPeers]    Script Date: 10/05/2013 11:44:34 p.m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  Stored Procedure dbo.TerrariumGrabTotalNumPeers    Script Date: 1/4/2005 6:12:18 PM ******/
/****** Object:  Stored Procedure dbo.TerrariumGrabTotalNumPeers    Script Date: 11/8/2001 8:16:23 PM ******/
ALTER PROCEDURE [dbo].[TerrariumGrabTotalNumPeers]
AS
    SELECT
        Count(*)
    FROM
        Peers
GO
