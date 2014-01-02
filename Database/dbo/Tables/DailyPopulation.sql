CREATE TABLE [dbo].[DailyPopulation](
	[SampleDateTime] [datetime] NOT NULL,
	[SpeciesName] [varchar](255) NOT NULL,
	[Population] [int] NOT NULL,
	[BirthCount] [int] NOT NULL,
	[StarvedCount] [int] NOT NULL,
	[KilledCount] [int] NOT NULL,
	[ErrorCount] [int] NOT NULL,
	[TimeoutCount] [int] NOT NULL,
	[SickCount] [int] NOT NULL,
	[OldAgeCount] [int] NOT NULL,
	[SecurityViolationCount] [int] NOT NULL
) ON [PRIMARY]