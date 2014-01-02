CREATE TABLE [dbo].[History](
	[GUID] [uniqueidentifier] NOT NULL,
	[TickNumber] [int] NOT NULL,
	[SpeciesName] [varchar](255) NOT NULL,
	[ContactTime] [datetime] NOT NULL,
	[ClientTime] [datetime] NOT NULL,
	[CorrectTime] [tinyint] NOT NULL,
	[Population] [int] NOT NULL,
	[BirthCount] [int] NOT NULL,
	[TeleportedToCount] [int] NOT NULL,
	[StarvedCount] [int] NOT NULL,
	[KilledCount] [int] NOT NULL,
	[TeleportedFromCount] [int] NOT NULL,
	[ErrorCount] [int] NOT NULL,
	[TimeoutCount] [int] NOT NULL,
	[SickCount] [int] NOT NULL,
	[OldAgeCount] [int] NOT NULL,
	[SecurityViolationCount] [int] NOT NULL
) ON [PRIMARY]