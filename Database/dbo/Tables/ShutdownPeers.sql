CREATE TABLE [dbo].[ShutdownPeers](
	[GUID] [uniqueidentifier] NULL,
	[Channel] [varchar](255) NOT NULL,
	[IPAddress] [varchar](50) NOT NULL,
	[Version] [varchar](255) NOT NULL,
	[FirstContact] [datetime] NOT NULL,
	[LastContact] [datetime] NOT NULL,
	[UnRegister] [bit] NOT NULL
) ON [PRIMARY]