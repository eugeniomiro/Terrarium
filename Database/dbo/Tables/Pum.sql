CREATE TABLE [dbo].[Pum](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[Alias] [varchar](255) NOT NULL,
	[TeamName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Pum] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]