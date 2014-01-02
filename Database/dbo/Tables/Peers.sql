CREATE TABLE [dbo].[Peers](
	[Channel] [varchar](32) NOT NULL,
	[IPAddress] [varchar](16) NOT NULL,
	[Lease] [datetime] NOT NULL,
	[Version] [varchar](16) NOT NULL,
	[Guid] [uniqueidentifier] NULL,
	[FirstContact] [datetime] NOT NULL
) ON [PRIMARY]