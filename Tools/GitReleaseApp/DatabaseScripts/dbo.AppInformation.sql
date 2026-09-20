CREATE TABLE [dbo].[AppInformation]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[Path] VARCHAR(10) NOT NULL,
	[StartDate] DATE NULL,
	[StartTime] VARCHAR(10) NULL,
	[EndDate] DATE NULL,
	[EndTime] VARCHAR(10) NULL,
	[UATBranchName] VARCHAR(20) NULL,
	[InitialTagName] VARCHAR(20) NULL,
	[FinalTagName] VARCHAR(20) NULL,
	[Token] VARCHAR(100) NULL,
	[RepoPath] VARCHAR(200) NULL,
	[DocumentationPath] VARCHAR(200) NULL,
	[NonPRODNotificationHook] VARCHAR(500) NULL,
	[ProdNotificationHook] VARCHAR(500) NULL,
	[NonProdNotificationStatus] bit NULL,
	[PRODNotificationStatus] bit NULL,
	[ConsoleOutput] VARCHAR(MAX) NULL, 
    [CreateDateTime] DATETIME NULL
)
