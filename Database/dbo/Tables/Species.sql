CREATE TABLE [dbo].[Species](
	[Name] [varchar](255) NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Author] [varchar](255) NOT NULL,
	[AuthorEmail] [varchar](255) NOT NULL,
	[DateAdded] [datetime] NOT NULL,
	[AssemblyFullName] [text] NOT NULL,
	[Extinct] [tinyint] NOT NULL,
	[LastReintroduction] [datetime] NULL,
	[ReintroductionNode] [uniqueidentifier] NULL,
	[Version] [varchar](255) NOT NULL,
	[BlackListed] [bit] NOT NULL,
	[ExtinctDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]