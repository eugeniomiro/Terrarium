CREATE TABLE [dbo].[NodeLastContact](
	[GUID] [uniqueidentifier] NOT NULL,
	[LastTick] [int] NOT NULL,
	[LastContact] [datetime] NOT NULL
) ON [PRIMARY]