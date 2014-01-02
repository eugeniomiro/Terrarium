
USE [master]
GO

/****** Object:  Database [Terrarium]    Script Date: 05/14/2008 08:28:03 ******/
CREATE DATABASE [Terrarium] ON  PRIMARY 
( NAME = N'Terrarium_Data', SIZE = 120448KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 LOG ON 
( NAME = N'Terrarium_Log', SIZE = 470144KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

ALTER DATABASE [Terrarium] SET ANSI_NULL_DEFAULT OFF
GO

ALTER DATABASE [Terrarium] SET DATE_CORRELATION_OPTIMIZATION OFF
GO

USE [Terrarium]
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