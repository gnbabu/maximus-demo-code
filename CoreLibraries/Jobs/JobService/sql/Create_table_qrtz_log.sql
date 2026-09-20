
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[QRTZ_BLOB_TRIGGERS]') AND OBJECTPROPERTY(id, N'ISUSERTABLE') = 1)
Begin
	CREATE TABLE [dbo].[QRTZ_LOG] (
	[Id] [int] IDENTITY (1, 1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar] (255) NOT NULL,
	[Level] [varchar] (50) NOT NULL,
	[Logger] [varchar] (255) NOT NULL,
	[Message] [varchar] (4000) NOT NULL,
	[Exception] [varchar] (2000) NULL
	)
END

