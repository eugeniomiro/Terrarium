CREATE TABLE [dbo].[VersionedSettings](
	[Version] [varchar](255) NOT NULL,
	[Disabled] [int] NOT NULL,
	[Message] [varchar](255) NULL
) ON [PRIMARY]