CREATE TABLE [dbo].[Watson](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[LogType] [varchar](50) NULL,
	[MachineName] [varchar](255) NULL,
	[OSVersion] [varchar](50) NULL,
	[GameVersion] [varchar](50) NULL,
	[CLRVersion] [varchar](50) NULL,
	[ErrorLog] [text] NULL,
	[UserEmail] [varchar](255) NULL,
	[UserComment] [text] NULL,
	[DateSubmitted] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]