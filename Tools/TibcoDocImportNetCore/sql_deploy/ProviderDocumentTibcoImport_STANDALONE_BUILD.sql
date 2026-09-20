
-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################  REPLICATE DOCUMENT TABLES NEEDED FOR STANDALONE SETUP   ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################


/****** Object:  Table [dbo].[DOCUMENT]    Script Date: 7/6/2021 1:54:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DOCUMENT]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DOCUMENT](
	[DOCUMENT_ID] [int] IDENTITY(1,1) NOT NULL,
	[NAME] [varchar](100) NULL,
	[DESCRIPTION] [varchar](500) NULL,
	[FILE_NAME] [varchar](max) NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[ONBASE_DOCUMENT_ID] [int] NULL,
	[IS_CONVERSION] [bit] NOT NULL,
 CONSTRAINT [PK_DOCUMENT] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DOCUMENT_ATTACHMENT_XREF]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DOCUMENT_ATTACHMENT_XREF]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DOCUMENT_ATTACHMENT_XREF](
	[DOCUMENT_ATTACHMENT_XREF_ID] [int] IDENTITY(1,1) NOT NULL,
	[DOCUMENT_TYPE_ID] [int] NOT NULL,
	[DOCUMENT_ID] [int] NOT NULL,
	[NOTES] [varchar](max) NULL,
	[ISDOWNLOADED] [bit] NULL,
	[DOCUMENT_PROCESSED_DATE] [datetime] NULL,
	[DOCUMENT_RECEIVED_DATE] [datetime] NULL,
	[CREATED_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[CREATED_BY_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[RetrieveReport_Type_ID] [int] NULL,
	[DOWNLOADED_DATE] [datetime] NULL,
	[SITRANSACTIONKEY] [varchar](50) NULL,
	[STG_MESSAGE_HEADER_ID] [int] NULL,
 CONSTRAINT [PK_Document_Attachment_Xref_id] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_ATTACHMENT_XREF_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DOCUMENT_INDEX]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DOCUMENT_INDEX]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DOCUMENT_INDEX](
	[DOCUMENT_INDEX_ID] [int] IDENTITY(1,1) NOT NULL,
	[DOCUMENT_ID] [int] NOT NULL,
	[INDEXID] [varchar](20) NULL,
	[CREATED_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[CREATED_BY_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[DOCUMENT_XREF_TYPE_ID] [int] NULL,
 CONSTRAINT [PK_DOCUMENT_INDEX_ID] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_INDEX_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DOCUMENT_TYPE]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DOCUMENT_TYPE]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DOCUMENT_TYPE](
	[DOCUMENT_TYPE_ID] [int] IDENTITY(1,1) NOT NULL,
	[DOCUMENT_TYPE_CODE] [varchar](10) NOT NULL,
	[DOCUMENT_TYPE_DESC] [varchar](256) NULL,
	[DOCUMENT_SERVICE_NAME] [varchar](256) NULL,
	[IS_VISIBLE] [varchar](1) NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DOCUMENT_TYPE] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[DOCUMENT_XREF_TYPE]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DOCUMENT_XREF_TYPE]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DOCUMENT_XREF_TYPE](
	[DOCUMENT_XREF_TYPE_ID] [int] IDENTITY(1,1) NOT NULL,
	[DOCUMENT_XREF_TYPE] [varchar](10) NOT NULL,
	[DOCUMENT_XREF_TYPE_DESC] [varchar](100) NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_DOCUMENT_XREF_TYPE_ID] PRIMARY KEY CLUSTERED 
(
	[DOCUMENT_XREF_TYPE_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[REG_CONVERTED_DOCUMENT_XREF]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REG_CONVERTED_DOCUMENT_XREF]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[REG_CONVERTED_DOCUMENT_XREF](
	[REG_ID] [int] NULL,
	[DOCUMENT_ID] [int] NULL,
	[DOC_TYPE] [nvarchar](128) NULL,
	[DTL_DOC_TYPE] [nvarchar](128) NULL,
	[SUB_DOC_TYPE] [nvarchar](128) NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[REG_DOCUMENT_XREF]    Script Date: 7/6/2021 1:54:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[REG_DOCUMENT_XREF]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[REG_DOCUMENT_XREF](
	[REG_ID] [int] NOT NULL,
	[REG_PAGE_TYPE_ID] [int] NOT NULL,
	[DOCUMENT_ID] [int] NOT NULL,
	[LAST_MODIFIED_DATE_TIME] [datetime] NOT NULL,
	[LAST_MODIFIED_USER] [uniqueidentifier] NOT NULL,
	[REG_PAGE_SECTION] [varchar](80) NULL,
	[SCREENING_ACTIVITY_ID] [int] NULL,
	[REG_SECTION_UPLOAD_CONTROL_ID] [int] NULL,
	[ROW_ID] [int] NULL,
	[WFTaskName] [varchar](100) NULL,
 CONSTRAINT [PK_REG_DOCUMENT_XREF] PRIMARY KEY CLUSTERED 
(
	[REG_ID] ASC,
	[REG_PAGE_TYPE_ID] ASC,
	[DOCUMENT_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_DOCUMENT_IS_CONVERSION]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[DOCUMENT] ADD  CONSTRAINT [DF_DOCUMENT_IS_CONVERSION]  DEFAULT ((0)) FOR [IS_CONVERSION]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_DOCUMENT_ATTACHMENT_XREF_LAST_MODIFIED_DATE_TIME]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[DOCUMENT_ATTACHMENT_XREF] ADD  CONSTRAINT [DF_DOCUMENT_ATTACHMENT_XREF_LAST_MODIFIED_DATE_TIME]  DEFAULT (getdate()) FOR [LAST_MODIFIED_DATE_TIME]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_DOCUMENT_INDEX_LAST_MODIFIED_DATE_TIME]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[DOCUMENT_INDEX] ADD  CONSTRAINT [DF_DOCUMENT_INDEX_LAST_MODIFIED_DATE_TIME]  DEFAULT (getdate()) FOR [LAST_MODIFIED_DATE_TIME]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_DOCUMENT_TYPE_LAST_MODIFIED_DATE_TIME]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[DOCUMENT_TYPE] ADD  CONSTRAINT [DF_DOCUMENT_TYPE_LAST_MODIFIED_DATE_TIME]  DEFAULT (getdate()) FOR [LAST_MODIFIED_DATE_TIME]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_DOCUMENT_XREF_TYPE_LAST_MODIFIED_DATE_TIME]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[DOCUMENT_XREF_TYPE] ADD  CONSTRAINT [DF_DOCUMENT_XREF_TYPE_LAST_MODIFIED_DATE_TIME]  DEFAULT (getdate()) FOR [LAST_MODIFIED_DATE_TIME]
END
GO

-- extended properties

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'DOCUMENT_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Document Id is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'DOCUMENT_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'NAME'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Name is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'NAME'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'DESCRIPTION'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Description is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'DESCRIPTION'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'FILE_NAME'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This File Name is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'FILE_NAME'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'LAST_MODIFIED_DATE_TIME'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Last Modified Date Time is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_DATE_TIME'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'LAST_MODIFIED_USER'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Last Modified User is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_USER'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'ONBASE_DOCUMENT_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Onbase Document Id is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'ONBASE_DOCUMENT_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', N'COLUMN',N'IS_CONVERSION'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This Is Conversion is for a document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT', @level2type=N'COLUMN',@level2name=N'IS_CONVERSION'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'DOCUMENT', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This table stores documents.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DOCUMENT'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'REG_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Reg Id for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'REG_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'DOCUMENT_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Document Id for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'DOCUMENT_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'DOC_TYPE'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Doc Type for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'DOC_TYPE'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'DTL_DOC_TYPE'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Dtl Doc Type for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'DTL_DOC_TYPE'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'SUB_DOC_TYPE'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Sub Doc Type for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'SUB_DOC_TYPE'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'LAST_MODIFIED_DATE_TIME'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Last Modified Date Time for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_DATE_TIME'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', N'COLUMN',N'LAST_MODIFIED_USER'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Last Modified User for a converted document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_USER'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_CONVERTED_DOCUMENT_XREF', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This table stores the relationship(s) between new and old documents for registrations.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_CONVERTED_DOCUMENT_XREF'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'REG_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Reg Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'REG_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'REG_PAGE_TYPE_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Reg Page Type Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'REG_PAGE_TYPE_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'DOCUMENT_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Document Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'DOCUMENT_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'LAST_MODIFIED_DATE_TIME'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Last Modified Date Time for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_DATE_TIME'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'LAST_MODIFIED_USER'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Last Modified User for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'LAST_MODIFIED_USER'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'REG_PAGE_SECTION'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Reg Page Section for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'REG_PAGE_SECTION'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'SCREENING_ACTIVITY_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Screening Activity Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'SCREENING_ACTIVITY_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'REG_SECTION_UPLOAD_CONTROL_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Reg Section Upload Control Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'REG_SECTION_UPLOAD_CONTROL_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'ROW_ID'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Row Id for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'ROW_ID'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', N'COLUMN',N'WFTaskName'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This is the provider''s Wftaskname for a registration document.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF', @level2type=N'COLUMN',@level2name=N'WFTaskName'
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'REG_DOCUMENT_XREF', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'This table stores the relationship(s) between documents and registrations.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'REG_DOCUMENT_XREF'
GO


-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                 SEED DOCUMENT TYPE TABLE                 ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################

-- cleanup in case previous failed
SET IDENTITY_INSERT [dbo].[DOCUMENT_TYPE] OFF
GO

SET IDENTITY_INSERT [dbo].[DOCUMENT_XREF_TYPE] OFF
GO



DELETE FROM [dbo].[DOCUMENT_TYPE];
GO

SET IDENTITY_INSERT [dbo].[DOCUMENT_TYPE] ON;
GO

DECLARE @AdminUser UNIQUEIDENTIFIER = '5D0689A8-D885-4211-B9FD-56757474AB4D',
		@DTM DATETIME;
SET @DTM = GETDATE();


INSERT INTO [dbo].[DOCUMENT_TYPE] ([DOCUMENT_TYPE_ID], [DOCUMENT_TYPE_CODE],[DOCUMENT_TYPE_DESC],[DOCUMENT_SERVICE_NAME],[IS_VISIBLE],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER])
VALUES (1, N'251', N'DECLARATION OF ELECTION OF HOSPICE BENEFIT DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(2, N'252', N'ATTENDING PHYSICIAN WRITTEN CERTIFICATION DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(3, N'253', N'REVOCATION OF HOSPICE BENEFIT DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(4, N'254', N'STATEMENT OF TERMINATION OF HOSPICE BENEFIT DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(5, N'255', N'SELECTION OF A DIFFERENT HOSPICE PROVIDER DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(6, N'256', N'IDG WRITTEN CERTIFICATION DOCUMENT TYPE', N'Hospice', N'Y', @DTM, @AdminUser)
,(7, N'014', N'Abortion Form 3197', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(8, N'014', N'Adjustment Form 6766', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(9, N'014', N'Adjustment Form 6767', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(10, N'014', N'Adjustment Form 6768', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(11, N'014', N'Behavioral Health', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(12, N'014', N'Certificate Of Medical Necessity (CMS)', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(13, N'014', N'Consultations', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(14, N'014', N'Consultations For Surgical Clearance', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(15, N'014', N'Diagnostic Testing', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(16, N'014', N'Discharge Summary', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(17, N'014', N'Extended Bed Hold Day(S) Prior Authorization (9402)', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(18, N'014', N'History And Physical', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(19, N'014', N'Hysterectomy Form 3199', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(20, N'014', N'Laboratory Tests', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(21, N'014', N'Medical Review Form 6653', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(22, N'014', N'Medication List', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(23, N'014', N'Operative Report', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(24, N'014', N'Other', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(25, N'014', N'Other Related Progress Notes', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(26, N'014', N'Photographs', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(27, N'014', N'Physician Progress Notes', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(28, N'014', N'Price List', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(29, N'014', N'Product Information', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(30, N'014', N'Progress Notes', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(31, N'014', N'Sterilization Form 3198', N'Prior Auth Document', N'Y', @DTM, @AdminUser)
,(32, N'014', N'ADMISSION SUMMARY', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(33, N'014', N'CERTIFICATION', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(34, N'014', N'COMPLETED REFERRAL FORM', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(35, N'014', N'DENTAL MODELS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(36, N'014', N'DIAGNOSTIC REPORT', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(37, N'014', N'DISCHARGE SUMMARY', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(38, N'014', N'EXPLANATION OF BENEFITS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(39, N'014', N'MODELS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(40, N'014', N'NURSING NOTES', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(41, N'014', N'OPERATIVE NOTE', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(42, N'014', N'PHYSICAL THERAPY CERTIFICATION', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(43, N'014', N'PHYSICAL THERAPY NOTES', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(44, N'014', N'PHYSICIAN ORDER PRESCRIPTION', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(45, N'014', N'PROSTHETICS OR ORTHOTIC CERTIFICATION', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(46, N'014', N'RADIOLOGY FILMS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(47, N'014', N'RADIOLOGY REPORTS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(48, N'014', N'REFERRAL FORM (OHIO 6653)', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(49, N'014', N'REPORT OF TESTS AND ANALYSIS REPORT', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(50, N'014', N'SUPPORT DATA FOR CLAIM', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(51, N'033', N'Hospital Cost Report (EXCEL)', N'Hospital Cost Report', N'Y', @DTM, @AdminUser)
,(52, N'034', N'Medicare Cost Report (EC)', N'Hospital Cost Report', N'Y', @DTM, @AdminUser)
,(53, N'035', N'Medicare Cost Report Print Image (PI)', N'Hospital Cost Report', N'Y', @DTM, @AdminUser)
,(54, N'036', N'Other', N'Hospital Cost Report', N'Y', @DTM, @AdminUser)
,(55, N'342', N'NF ICF-IID COST REPORTS', N'LTC Cost Report', N'Y', @DTM, @AdminUser)
,(56, N'345', N'Medicaid School Program Cost Report', N'MSP Cost Report', N'Y', @DTM, @AdminUser)
,(57, N'295', N'INTERIM', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(58, N'296', N'AMENDED INTERIM', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(59, N'297', N'REVISED INTERIM', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(60, N'298', N'FINAL', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(61, N'299', N'AMENDED FINAL', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(62, N'300', N'REVISED FINAL', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(63, N'301', N'CORRECTION ADVISORY', N'Hospital Cost Settlement', N'Y', @DTM, @AdminUser)
,(64, N'355', N'FINAL', N'MSP Cost Settlement', N'Y', @DTM, @AdminUser)
,(65, N'356', N'AMENDED FINAL', N'MSP Cost Settlement', N'Y', @DTM, @AdminUser)
,(66, N'357', N'SUPPLEMENTAL DOCUMENT', N'MSP Cost Settlement', N'Y', @DTM, @AdminUser)
,(67, N'346', N'LTC Rate Packages', N'LTC Rate Packages', N'Y', @DTM, @AdminUser)
,(68, N'051', N'MDS Reports', N'MDS Reports', N'Y', @DTM, @AdminUser)
,(69, N'050', N'RemittanceAdvice', N'RA', N'Y', @DTM, @AdminUser)
,(70, N'011', N'Provider (Generic)', N'Provider (Generic)', N'Y', @DTM, @AdminUser)
,(71, N'030', N'Financial - Hosp Cost Settlement', N'Financial - Hosp Cost Settlement', N'Y', @DTM, @AdminUser)
,(72, N'100', N'Addendum R', N'Addendum R', N'Y', @DTM, @AdminUser)
,(73, N'101', N'Government issued photo ID', N'Government issued photo ID', N'Y', @DTM, @AdminUser)
,(74, N'103', N'Anticipated Change of Ownership Explanation', N'Anticipated Change of Ownership Explanation', N'Y', @DTM, @AdminUser)
,(75, N'104', N'Application', N'Application', N'Y', @DTM, @AdminUser)
,(76, N'105', N'Application Fee Confirmation', N'Application Fee Confirmation', N'Y', @DTM, @AdminUser)
,(77, N'106', N'Authorized Agreement for State Medicaid Payment', N'Authorized Agreement for State Medicaid Payment', N'Y', @DTM, @AdminUser)
,(78, N'107', N'CHAPS or Joint Commission Accreditation', N'CHAPS or Joint Commission Accreditation', N'Y', @DTM, @AdminUser)
,(79, N'108', N'CLIA Certification', N'CLIA Certification', N'Y', @DTM, @AdminUser)
,(80, N'109', N'CMS 671', N'CMS 671', N'Y', @DTM, @AdminUser)
,(81, N'110', N'CPR Certification per individual', N'CPR Certification per individual', N'Y', @DTM, @AdminUser)
,(82, N'111', N'Change of Ownership Explanation', N'Change of Ownership Explanation', N'Y', @DTM, @AdminUser)
,(83, N'112', N'Confirmation from Consumer', N'Confirmation from Consumer', N'Y', @DTM, @AdminUser)
,(84, N'114', N'Copy of Joint Commission or CHAP certification', N'Copy of Joint Commission or CHAP certification', N'Y', @DTM, @AdminUser)
,(85, N'115', N'Copy of Liability Insurance', N'Copy of Liability Insurance', N'Y', @DTM, @AdminUser)
,(86, N'116', N'Copy of notice from the NPI Enumerator', N'Copy of notice from the NPI Enumerator', N'Y', @DTM, @AdminUser)
,(87, N'117', N'Criminal background check', N'Criminal background check', N'Y', @DTM, @AdminUser)
,(88, N'118', N'DMA - Form HLS 0038', N'DMA - Form HLS 0038', N'Y', @DTM, @AdminUser)
,(89, N'119', N'DODD Developmental Approval Letter', N'DODD Developmental Approval Letter', N'Y', @DTM, @AdminUser)
,(90, N'120', N'DODD Residential Facility license', N'DODD Residential Facility license', N'Y', @DTM, @AdminUser)
,(91, N'121', N'Date of ODMR/DD Development Approval Letter', N'Date of ODMR/DD Development Approval Letter', N'Y', @DTM, @AdminUser)
,(92, N'122', N'Designation of an 835 or 834-820 Trading Partner JFS 06306', N'Designation of an 835 or 834-820 Trading Partner JFS 06306', N'Y', @DTM, @AdminUser)
,(93, N'123', N'Documentation of Training', N'Documentation of Training', N'Y', @DTM, @AdminUser)
,(94, N'124', N'Drivers Record from Bureau of Motor Vehicles per individual', N'Drivers Record from Bureau of Motor Vehicles per individual', N'Y', @DTM, @AdminUser)
,(95, N'125', N'EMT Card per individual', N'EMT Card per individual', N'Y', @DTM, @AdminUser)
,(96, N'129', N'Final copies of all signed and fully executed docs for txn(s) culminating in the CHOP', N'Final copies of all signed and fully executed docs for txn(s) culminating in the CHOP', N'Y', @DTM, @AdminUser)
,(97, N'130', N'Fully executed CHOP docs (leases, purchase agreements) for Entering Operator', N'Fully executed CHOP docs (leases, purchase agreements) for Entering Operator', N'Y', @DTM, @AdminUser)
,(98, N'131', N'First-Aid Certification', N'First-Aid Certification', N'Y', @DTM, @AdminUser)
,(99, N'133', N'Form DMA', N'Form DMA', N'Y', @DTM, @AdminUser)
,(100, N'134', N'HMG-HV-TCM Application for ODH Provider Certification', N'HMG-HV-TCM Application for ODH Provider Certification', N'Y', @DTM, @AdminUser)
,(101, N'135', N'Home State Psychiatric License for Outstate Type 2', N'Home State Psychiatric License for Outstate Type 2', N'Y', @DTM, @AdminUser)
,(102, N'136', N'IRS Form W-9', N'IRS Form W-9', N'Y', @DTM, @AdminUser)
,(103, N'137', N'IRS Form W-9 From Entering Operator', N'IRS Form W-9 From Entering Operator', N'Y', @DTM, @AdminUser)
,(104, N'138', N'IRS Form W-9 From Exiting Operator', N'IRS Form W-9 From Exiting Operator', N'Y', @DTM, @AdminUser)
,(105, N'139', N'IRS Letter regarding EIN or S Corporation', N'IRS Letter regarding EIN or S Corporation', N'Y', @DTM, @AdminUser)
,(106, N'140', N'Initial Facility Letter', N'Initial Facility Letter', N'Y', @DTM, @AdminUser)
,(107, N'141', N'Initial Notification Letter', N'Initial Notification Letter', N'Y', @DTM, @AdminUser)
,(108, N'142', N'Initial Notification Letter From Entering Operator', N'Initial Notification Letter From Entering Operator', N'Y', @DTM, @AdminUser)
,(109, N'143', N'Initial Notification Letter From Exiting Operator', N'Initial Notification Letter From Exiting Operator', N'Y', @DTM, @AdminUser)
,(110, N'144', N'JFS 03620', N'JFS 03620', N'Y', @DTM, @AdminUser)
,(111, N'145', N'JFS 03620 From Exiting Operator', N'JFS 03620 From Exiting Operator', N'Y', @DTM, @AdminUser)
,(112, N'146', N'LTCF Provider Agreement (JFS 3623)', N'LTCF Provider Agreement (JFS 3623)', N'Y', @DTM, @AdminUser)
,(113, N'147', N'License', N'License', N'Y', @DTM, @AdminUser)
,(114, N'148', N'Medicaid Provider Agreement', N'Medicaid Provider Agreement', N'Y', @DTM, @AdminUser)
,(115, N'149', N'Medical Transportation Board Certificate of Licensure', N'Medical Transportation Board Certificate of Licensure', N'Y', @DTM, @AdminUser)
,(116, N'150', N'Medicare Approval Letter', N'Medicare Approval Letter', N'Y', @DTM, @AdminUser)
,(117, N'151', N'Medicare Participation', N'Medicare Participation', N'Y', @DTM, @AdminUser)
,(118, N'152', N'Medicare Provider Agreement', N'Medicare Provider Agreement', N'Y', @DTM, @AdminUser)
,(119, N'154', N'NPPES Validation Form From Entering Operator', N'NPPES Validation Form From Entering Operator', N'Y', @DTM, @AdminUser)
,(120, N'155', N'OBM 3456 - New Vendor Information Form', N'OBM 3456 - New Vendor Information Form', N'Y', @DTM, @AdminUser)
,(121, N'156', N'OBM 5657', N'OBM 5657', N'Y', @DTM, @AdminUser)
,(122, N'157', N'OBM 5657 From Entering Operator', N'OBM 5657 From Entering Operator', N'Y', @DTM, @AdminUser)
,(123, N'158', N'OBM 5657 From Exiting Operator', N'OBM 5657 From Exiting Operator', N'Y', @DTM, @AdminUser)
,(124, N'159', N'OBM 5678', N'OBM 5678', N'Y', @DTM, @AdminUser)
,(125, N'160', N'OBM 5678 From Entering Operator', N'OBM 5678 From Entering Operator', N'Y', @DTM, @AdminUser)
,(126, N'161', N'ODH Bed Registration for all Instate', N'ODH Bed Registration for all Instate', N'Y', @DTM, @AdminUser)
,(127, N'162', N'ODH HMG-HV Provider Certification Approval Letter', N'ODH HMG-HV Provider Certification Approval Letter', N'Y', @DTM, @AdminUser)
,(128, N'163', N'ODH Nursery Level Doc for Instate Type 01', N'ODH Nursery Level Doc for Instate Type 01', N'Y', @DTM, @AdminUser)
,(129, N'164', N'ODH Nursing Home License', N'ODH Nursing Home License', N'Y', @DTM, @AdminUser)
,(130, N'165', N'ODH issued Certificate of Need - CON', N'ODH issued Certificate of Need - CON', N'Y', @DTM, @AdminUser)
,(131, N'166', N'ODMH License for Instate Type 02', N'ODMH License for Instate Type 02', N'Y', @DTM, @AdminUser)
,(132, N'168', N'ORCB License', N'ORCB License', N'Y', @DTM, @AdminUser)
,(133, N'169', N'Provider Agreement (Behavioral) Outlier Services (03642)', N'Provider Agreement (Behavioral) Outlier Services (03642)', N'Y', @DTM, @AdminUser)
,(134, N'170', N'Provider Agreement Pediatric Outlier Services (03621)', N'Provider Agreement Pediatric Outlier Services (03621)', N'Y', @DTM, @AdminUser)
,(135, N'172', N'Statement of Acceptance and Intent From Entering Operator', N'Statement of Acceptance and Intent From Entering Operator', N'Y', @DTM, @AdminUser)
,(136, N'173', N'Proof of Vehicle Inspection', N'Proof of Vehicle Inspection', N'Y', @DTM, @AdminUser)
,(137, N'174', N'Provider Agreement Update Form', N'Provider Agreement Update Form', N'Y', @DTM, @AdminUser)
,(138, N'176', N'REVISED INTERIM', N'REVISED INTERIM', N'Y', @DTM, @AdminUser)
,(139, N'177', N'Social Security Card', N'Social Security Card', N'Y', @DTM, @AdminUser)
,(140, N'178', N'Statement of Acceptance and Intent', N'Statement of Acceptance and Intent', N'Y', @DTM, @AdminUser)
,(141, N'179', N'SUD Residential Treatment Facility Certification', N'SUD Residential Treatment Facility Certification', N'Y', @DTM, @AdminUser)
,(142, N'180', N'SURS - CLAIM DETAIL', N'SURS - CLAIM DETAIL', N'Y', @DTM, @AdminUser)
,(143, N'181', N'Verification of Bed Size Doc for Outstate Type 2', N'Verification of Bed Size Doc for Outstate Type 2', N'Y', @DTM, @AdminUser)
,(144, N'182', N'Verification of Consumer Listing', N'Verification of Consumer Listing', N'Y', @DTM, @AdminUser)
,(145, N'183', N'Verification of Nurse Aide Registry', N'Verification of Nurse Aide Registry', N'Y', @DTM, @AdminUser)
,(146, N'184', N'Voided Check or Bank Letter', N'Voided Check or Bank Letter', N'Y', @DTM, @AdminUser)
,(147, N'185', N'Voluntary Termination', N'Voluntary Termination', N'Y', @DTM, @AdminUser)
,(148, N'187', N'Voluntary Withdrawal', N'Voluntary Withdrawal', N'Y', @DTM, @AdminUser)
,(149, N'188', N'Written Authorization existing operator/owner and entering operator', N'Written Authorization existing operator/owner and entering operator', N'Y', @DTM, @AdminUser)
,(150, N'189', N'Welcome Letter', N'Welcome Letter', N'Y', @DTM, @AdminUser)
,(151, N'190', N'Written Authorization existing operator/owner and entering operator', N'Written Authorization existing operator/owner and entering operator', N'Y', @DTM, @AdminUser)
,(152, N'191', N'Other - document not listed', N'Other - document not listed', N'Y', @DTM, @AdminUser)
,(153, N'192', N'Combined - Use this selection if you submit multiple "Types of Document" in one file', N'Combined - Use this selection if you submit multiple "Types of Document" in one file', N'Y', @DTM, @AdminUser)
,(154, N'198', N'ORCB License Verification', N'ORCB License Verification', N'Y', @DTM, @AdminUser)
,(155, N'199', N'DEA Certificate', N'DEA Certificate', N'Y', @DTM, @AdminUser)
,(156, N'200', N'HCA Medication Authorization - ODM 02389', N'HCA Medication Authorization - ODM 02389', N'Y', @DTM, @AdminUser)
,(157, N'201', N'HCA Skilled Task Authorization - ODM 02390', N'HCA Skilled Task Authorization - ODM 02390', N'Y', @DTM, @AdminUser)
,(158, N'202', N'HCA Addendum M - JFS 02391', N'HCA Addendum M - JFS 02391', N'Y', @DTM, @AdminUser)
,(159, N'203', N'State Auditor Verification', N'State Auditor Verification', N'Y', @DTM, @AdminUser)
,(160, N'205', N'Notice of Operational Deficiency', N'Notice of Operational Deficiency', N'Y', @DTM, @AdminUser)
,(161, N'206', N'Plan of Correction', N'Plan of Correction', N'Y', @DTM, @AdminUser)
,(162, N'207', N'Proposed Adj. Order', N'Proposed Adj. Order', N'Y', @DTM, @AdminUser)
,(163, N'209', N'W-9 Form', N'W-9 Form', N'Y', @DTM, @AdminUser)
,(164, N'210', N'Welcome Letter', N'Welcome Letter', N'Y', @DTM, @AdminUser)
,(165, N'211', N'Documentation of Name Change', N'Documentation of Name Change', N'Y', @DTM, @AdminUser)
,(166, N'212', N'Elevated Screening  Letter', N'Elevated Screening  Letter', N'Y', @DTM, @AdminUser)
,(167, N'215', N'Ownership/Disclosure Documentation', N'Ownership/Disclosure Documentation', N'Y', @DTM, @AdminUser)
,(168, N'213', N'Notice of Suspension', N'Notice of Suspension', N'Y', @DTM, @AdminUser)
,(169, N'216', N'Application Addendums', N'Application Addendums', N'Y', @DTM, @AdminUser)
,(170, N'217', N'CMS 1539', N'CMS 1539', N'Y', @DTM, @AdminUser)
,(171, N'218', N'Medicaid State Terminations', N'Medicaid State Terminations', N'Y', @DTM, @AdminUser)
,(172, N'219', N'HRSA/Look-A-Like Letter', N'HRSA/Look-A-Like Letter', N'Y', @DTM, @AdminUser)
,(173, N'222', N'Electronic Visit Training Certificate', N'Electronic Visit Training Certificate', N'Y', @DTM, @AdminUser)
,(174, N'223', N'Medical Board Acupun Cert', N'Medical Board Acupun Cert', N'Y', @DTM, @AdminUser)
,(175, N'224', N'Chiro Board Acupun Cert', N'Chiro Board Acupun Cert', N'Y', @DTM, @AdminUser)
,(176, N'226', N'Acupuncture Certification', N'Acupuncture Certification', N'Y', @DTM, @AdminUser)
,(177, N'227', N'CPC Memorandum of Understanding', N'CPC Memorandum of Understanding', N'Y', @DTM, @AdminUser)
,(178, N'228', N'High School Diploma or GED', N'High School Diploma or GED', N'Y', @DTM, @AdminUser)
,(179, N'229', N'3 yr. Employer Experience Attestation', N'3 yr. Employer Experience Attestation', N'Y', @DTM, @AdminUser)
,(180, N'230', N'Individual Peer Support Program Certification', N'Individual Peer Support Program Certification', N'Y', @DTM, @AdminUser)
,(181, N'231', N'IPS-SE Employee Training Verification Statement', N'IPS-SE Employee Training Verification Statement', N'Y', @DTM, @AdminUser)
,(182, N'232', N'Documentation of Training/Certification', N'Documentation of Training/Certification', N'Y', @DTM, @AdminUser)
,(183, N'233', N'Proof of Employment', N'Proof of Employment', N'Y', @DTM, @AdminUser)
,(184, N'234', N'SURS - OVERPAYMENT LETTER', N'SURS - OVERPAYMENT LETTER', N'Y', @DTM, @AdminUser)
,(185, N'236', N'Documentation of Certification', N'Documentation of Certification', N'Y', @DTM, @AdminUser)
,(186, N'237', N'REVISED FINAL', N'REVISED FINAL', N'Y', @DTM, @AdminUser)
,(187, N'238', N'Bed Size Verification', N'Bed Size Verification', N'Y', @DTM, @AdminUser)
,(188, N'239', N'Copy of Accreditation', N'Copy of Accreditation', N'Y', @DTM, @AdminUser)
,(189, N'272', N'POTENTIALLY PREVENTABLE READMISSIONS', N'POTENTIALLY PREVENTABLE READMISSIONS', N'Y', @DTM, @AdminUser)
,(190, N'304', N'Readiness Review', N'Readiness Review', N'Y', @DTM, @AdminUser)
,(191, N'305', N'PRTF License/Certification', N'PRTF License/Certification', N'Y', @DTM, @AdminUser)
,(192, N'419', N'Update Form', N'Update Form', N'Y', @DTM, @AdminUser)
,(193, N'420', N'Trading Partner Agreement', N'Trading Partner Agreement', N'Y', @DTM, @AdminUser)
,(194, N'020', N'Letters', N'Letters', N'Y', @DTM, @AdminUser)
,(195, N'1000', N'8 hour Initial Certification Training', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(196, N'1001', N'Annual Training', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(197, N'1002', N'Assistive Technology Assessment Credential', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(198, N'1003', N'Assistive Technology Credentials', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(199, N'1004', N'BCI Background Check', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(200, N'1005', N'CEO Designee - BCI Background Check', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(201, N'1006', N'CEO Designee - Social Security Number', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(202, N'1007', N'CEO Designee - State of Ohio Identification', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(203, N'1008', N'CEO Designee FBI', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(204, N'1009', N'Competency-based Training Certificates', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(205, N'1010', N'CPR', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(206, N'1011', N'Director''s Approval Letter', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(207, N'1012', N'Documentation of Name Change', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(208, N'1013', N'Driver''s Abstract', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(209, N'1014', N'Driver''s License', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(210, N'1015', N'Electronic Visit Verification (EVV) Training Certificate', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(211, N'1016', N'FBI', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(212, N'1017', N'First Aid', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(213, N'1018', N'High School Diploma/GED', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(214, N'1019', N'Home Delivered Meals Certification', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(215, N'1020', N'License', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(216, N'1021', N'Licensure as a Dietician by the State of Ohio', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(217, N'1022', N'MUI Training', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(218, N'1023', N'Policies and procedures for Agencies', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(219, N'1024', N'Policies and procedures for Drivers/Medicaid provider Agreement', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(220, N'1025', N'Policies and procedures on Drivers', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(221, N'1026', N'Professional License', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(222, N'1027', N'Proof of auto insurance coverage', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(223, N'1028', N'Proof of Auto Insurance coverage/Medicaid provider Agreement', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(224, N'1029', N'Proof of Specialization', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(225, N'1030', N'Secretary of State Certificate', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(226, N'1031', N'Social Security Number', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(227, N'1032', N'Social Work Credentials', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(228, N'1033', N'Specialized Medical Equipment Experience', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(229, N'1034', N'State of Ohio Identification', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(230, N'1035', N'TIN/IRS documentation', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(231, N'1036', N'W-9', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(232, N'1037', N'Written Statement of Certification/License', N'PSMPartialProvider', N'Y', @DTM, @AdminUser)
,(233, N'014', N'PERIODONTAL CHARTS', N'Prior Auth Claim Report', N'Y', @DTM, @AdminUser)
,(234, N'1038', N'ACCRED', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(235, N'1039', N'APPROVELTR', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(236, N'1040', N'BWC', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(237, N'1041', N'CBC', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(238, N'1042', N'COMM INS', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(239, N'1043', N'CONS REQ', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(240, N'1044', N'CONTRACT', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(241, N'1045', N'D/P INS', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(242, N'1046', N'DMA FORM', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(243, N'1047', N'DR LICENSE', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(244, N'1048', N'ED/TRG', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(245, N'1049', N'FLOOR', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(246, N'1050', N'HCAS MED', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(247, N'1051', N'HCAS TASK', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(248, N'1052', N'HCBS DOCS', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(249, N'1053', N'HCBS TOOL', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(250, N'1054', N'INS CARD', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(251, N'1055', N'JFS06750', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(252, N'1056', N'JFS06751', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(253, N'1057', N'LTCGG', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(254, N'1058', N'MISC', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(255, N'1059', N'MPEF', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(256, N'1060', N'ND AGMT', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(257, N'1061', N'NPI INFO', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(258, N'1062', N'ODA APP', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(259, N'1063', N'ODA ATTES', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(260, N'1064', N'ODA1042', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(261, N'1065', N'ODH SURV', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(262, N'1066', N'PAYMENT', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(263, N'1067', N'PHOTO ID', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(264, N'1068', N'PROOF RES', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(265, N'1069', N'RCF', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(266, N'1070', N'RECOMAPRL', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(267, N'1071', N'REV CKLST', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(268, N'1072', N'SANCTION', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(269, N'1073', N'SERVICES', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(270, N'1074', N'SOS', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(271, N'1075', N'SS CARD', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(272, N'1076', N'STATUSCHG', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(273, N'1077', N'TO', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(274, N'1078', N'UNIT', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(275, N'1079', N'VOL TERM', N'PCWPartialProvider', N'Y', @DTM, @AdminUser)
,(276, N'1080', N'W-9', N'PCWPartialProvider', N'Y', @DTM, @AdminUser);


SET IDENTITY_INSERT [dbo].[DOCUMENT_TYPE] OFF
GO


-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################              SEED DOCUMENT TYPE XREF TABLE               ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################

DELETE FROM [dbo].[DOCUMENT_XREF_TYPE];
GO

SET IDENTITY_INSERT [dbo].[DOCUMENT_XREF_TYPE] ON;
GO


DECLARE @AdminUser UNIQUEIDENTIFIER = '5D0689A8-D885-4211-B9FD-56757474AB4D',
		@DTM DATETIME;
SELECT @DTM = GETDATE();

INSERT INTO [dbo].[DOCUMENT_XREF_TYPE]([DOCUMENT_XREF_TYPE_ID], [DOCUMENT_XREF_TYPE], [DOCUMENT_XREF_TYPE_DESC], [LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER])		
VALUES (1, N'ACN', N'For Adjustment Control Number', @DTM, @AdminUser)
,(2, N'ATN', N'For Provider', @DTM, @AdminUser)
,(3, N'CCS', N'CASE_CAT_SEQ', @DTM, @AdminUser)
,(4, N'CRR', N'TPL_CARRIER', @DTM, @AdminUser)
,(5, N'CTN', N'For Correspondence', @DTM, @AdminUser)
,(6, N'DID', N'DocID', @DTM, @AdminUser)
,(7, N'EMP', N'TPL_EMPLOYER', @DTM, @AdminUser)
,(8, N'FAR', N'For Financial Account Receivable', @DTM, @AdminUser)
,(9, N'FEX', N'For Financial Expenditure', @DTM, @AdminUser)
,(10, N'FHC', N'FOR FINANCIAL HOSP COST SETTLEMENT', @DTM, @AdminUser)
,(11, N'FLC', N'FOR FINANCIAL LTC COST SETTLEMENT', @DTM, @AdminUser)
,(12, N'HAD', N'FOR HOSPICE ATTACHMENT ID', @DTM, @AdminUser)
,(13, N'HID', N'FOR HOSPICE ENROLLMENT ID', @DTM, @AdminUser)
,(14, N'ICN', N'For Claims', @DTM, @AdminUser)
,(15, N'IDL', N'ID_LETTER', @DTM, @AdminUser)
,(16, N'IID', N'FOR INTERVENTION ID', @DTM, @AdminUser)
,(17, N'NPI', N'For Provider', @DTM, @AdminUser)
,(18, N'LGN', N'LETTER_GEN_DATE', @DTM, @AdminUser)
,(19, N'PID', N'For Provider', @DTM, @AdminUser)
,(20, N'PAN', N'For Prior Authorization', @DTM, @AdminUser)
,(21, N'PCN', N'FOR PROGRAMS', @DTM, @AdminUser)
,(22, N'PLH', N'TPL_POLICY HOLDER', @DTM, @AdminUser)
,(23, N'RID', N'For Recipient', @DTM, @AdminUser)
,(24, N'SLR', N'SAK_LETTER_REQUEST', @DTM, @AdminUser)
,(25, N'TID', N'Trading Partner ID', @DTM, @AdminUser)
,(26, N'LTC', N'LTC Cost Settlement Number', @DTM, @AdminUser)
,(27, N'HTN', N'Hospital Tracking Number', @DTM, @AdminUser)
,(28, N'SAK', N'GENERIC SAK ID', @DTM, @AdminUser)
,(29, N'ICF', N'NFICFIIDCostsReports SAK', @DTM, @AdminUser)
,(30, N'STN', N'STN	SURS TRACKING NUMBER', @DTM, @AdminUser)
,(31, N'NAT', N'NOT ACCESSED CSV REPORT', @DTM, @AdminUser)
,(32, N'RAN', N'Remittance Advice Number', @DTM, @AdminUser)
,(33, N'FPD', N'Financial Payment Deduction', @DTM, @AdminUser)

GO

SET IDENTITY_INSERT [dbo].[DOCUMENT_XREF_TYPE] OFF
GO



-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                      LOGGING TABLES                      ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################


-------------------------------------------------------------------------------------------------------------------
-- create LogBulkImport                                                DROP TABLE dbo.LogBulkImport
-------------------------------------------------------------------------------------------------------------------

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogBulkImport]') AND type in (N'U'))
BEGIN	
	CREATE TABLE [dbo].[LogBulkImport](
		[LogId] [int] IDENTITY(1,1) NOT NULL, 
		[JobId] [int] NOT NULL DEFAULT(0),  -- over all job that is being worked on (see LogBulkImportJobDef)
		[WorkerId] [int] NOT NULL DEFAULT(0),  -- if multiple workers are doing this, the id of the worker (usually determined by the number the directory it is running from - i.e. c:\jobs\worker001)
		[UnitOfWork] [varchar](128) NOT NULL DEFAULT(''),  -- uniquely identifies the job being done (mq message id, etc...) - no constraint so can retry failed attemps / check for duplicate runs	
		[RefId1] bigint NOT NULL DEFAULT(0),  -- ref id of master record created (see LogBulkImportJobDef)
		[RefId2] bigint NOT NULL DEFAULT(0),  -- ref id of secondary record created (see LogBulkImportJobDef)
		[RefId3] bigint NOT NULL DEFAULT(0),  -- ref id of tertiary record created (see LogBulkImportJobDef)
		[OtherId1] bigint NOT NULL DEFAULT(0),  -- ref id of master record created (see LogBulkImportJobDef)
		[OtherId2] bigint NOT NULL DEFAULT(0),  -- ref id of secondary record created (see LogBulkImportJobDef)
		[OtherId3] bigint NOT NULL DEFAULT(0),  -- ref id of tertiary record created (see LogBulkImportJobDef)
		[StartAt] [datetime2](3) NULL,  -- current work started at
		[Step1Complete] [datetime2](3) NULL,  -- for processes with multiple steps, when step 1 completed
		[Step1MS] [int] NOT NULL DEFAULT(0),  -- for processes with multiple steps, step 1 time to complete in milliseconds
		[Step2Complete] [datetime2](3) NULL,  -- for processes with multiple steps, when step 2 completed
		[Step2MS] [int] NOT NULL DEFAULT(0),  -- for processes with multiple steps, step 2 time to complete in milliseconds
		[Step3Complete] [datetime2](3) NULL,  -- for processes with multiple steps, when step 3 completed
		[Step3MS] [int] NOT NULL DEFAULT(0),  -- for processes with multiple steps, step 3 time to complete in milliseconds
		[TotalComplete] [datetime2](3) NULL,  -- for processes with multiple steps, when step 1 completed
		[TotalMS] [int] NOT NULL DEFAULT(0),  -- for processes with multiple steps, step 1 time to complete in milliseconds
		[Metric1] [int] NOT NULL DEFAULT(0),  -- any metric you would like to store expressed as int (file size, num rows, etc...) only set upon total complete
		[Metric2] [int] NOT NULL DEFAULT(0),  -- any metric you would like to store expressed as int (file size, num rows, etc...) only set upon total complete
		[Metric3] [int] NOT NULL DEFAULT(0),  -- any metric you would like to store expressed as int (file size, num rows, etc...) only set upon total complete
		[ErrOccurred] [tinyint] NOT NULL DEFAULT(0),  -- did an error occur
		[ErrMessage] [varchar](256) NOT NULL DEFAULT(''),  -- err message
		[StatusTimestamp] [datetime2](3) NULL,  -- Timestamp for Status Message
		[StatusType] [varchar](20) NOT NULL DEFAULT(''),  -- type of status message (startup, shutdown, etc...) free text
		[StatusMessage] [varchar](256) NOT NULL DEFAULT(''),  -- message for non-timed events
		[UniqueId] [uniqueidentifier] NULL,  -- if a unique identifier is needed (thread id, etc..)
	 CONSTRAINT [PK_LogBulkImport] PRIMARY KEY CLUSTERED 
	(
		[LogId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]

END

GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name='ix_LBI_JobWork' AND object_id = OBJECT_ID('dbo.LogBulkImport'))
BEGIN
	CREATE INDEX ix_LBI_JobWork ON dbo.LogBulkImport(JobId, WorkerId, UnitOfWork)
END
GO

-------------------------------------------------------------------------------------------------------------------
-- create LogBulkImportJobDef                                           DROP TABLE dbo.LogBulkImportJobDef  
-------------------------------------------------------------------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogBulkImportJobDef]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[LogBulkImportJobDef](
		[JobId] [int] NOT NULL DEFAULT(0),  -- job id to use in LogBulkImport  *** NOTE ALL OTHER FIELD ARE FOR EXPLANATION ONLY
		[JobDesc] [varchar](128) NOT NULL DEFAULT(''),  -- description for overall job
		[JobContact] [varchar](128) NOT NULL DEFAULT(''),  -- owner / who to contact if something goes wrong
		[UnitOfWorkDesc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in UnitOfEork
		[RefId1Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId1
		[RefId2Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId2
		[RefId3Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId3
		[OtherId1Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId1
		[OtherId2Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId2
		[OtherId3Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in RefId3
		[Metric1Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in Metric1
		[Metric2Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in Metric2
		[Metric3Desc] [varchar](128) NOT NULL DEFAULT(''),  -- what field is being stored in Metric3
		[NotesOnJob] [varchar](2000) NOT NULL DEFAULT(''),  -- notes on job
	 CONSTRAINT [PK_LogBulkImportJobDef] PRIMARY KEY CLUSTERED 
	(
		[JobId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
	) ON [PRIMARY]

END




-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                      LOGGING PROCS                       ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################



-------------------------------------------------------------------------------------------------------------------
-- create usp_LogBulkImportStatusMessage
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_LogBulkImportStatusMessage]'))
DROP PROCEDURE [dbo].[usp_LogBulkImportStatusMessage]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================================
-- Author:		Richard Varno
-- Date:		12/29/2020
-- Description:	Record non-timed status message (for startup / end events)
-- =================================================================================================================================================

CREATE PROC [dbo].[usp_LogBulkImportStatusMessage] (
	@JobId int
,	@WorkerId int
,	@StatusType varchar(20) = ''
,	@StatusMessage varchar(256) = ''
	)
AS

INSERT INTO [dbo].[LogBulkImport]
           ([JobId]
           ,[WorkerId]
           ,[StatusTimestamp]
		   ,[StatusType]
		   ,[StatusMessage])
     VALUES
           (@JobId
           ,@WorkerId
		   ,SYSDATETIME()
           ,@StatusType
		   ,@StatusMessage
		   )

GO



-------------------------------------------------------------------------------------------------------------------
-- create usp_LogBulkImportInit
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_LogBulkImportInit]'))
DROP PROCEDURE [dbo].[usp_LogBulkImportInit]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================================
-- Author:		Richard Varno
-- Date:		12/22/2020
-- Description:	Init Record for recording bulk import metrics returns New LogId to pass to usp_LogBulkImportCompleteStep
-- =================================================================================================================================================

CREATE PROC [dbo].[usp_LogBulkImportInit] (
	@JobId int
,	@WorkerId int
,	@UniqueId uniqueidentifier = NULL
	)
AS

INSERT INTO [dbo].[LogBulkImport]
           ([JobId]
           ,[WorkerId]
           ,[UniqueId]
		   ,[StartAt])
     VALUES
           (@JobId
           ,@WorkerId
           ,@UniqueId
		   ,SYSDATETIME())

		SELECT SCOPE_IDENTITY() AS LogId

GO


-------------------------------------------------------------------------------------------------------------------
-- create usp_LogBulkImportCompleteStep
-------------------------------------------------------------------------------------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_LogBulkImportCompleteStep]'))
DROP PROCEDURE [dbo].[usp_LogBulkImportCompleteStep]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =================================================================================================================================================
-- Author:		Richard Varno
-- Date:		12/22/2020
-- Description:	Complete Step (and total) Record for recording bulk import metrics
-- Only Pass Unit of Work on Step 1
-- Each Steps Metrics will be recorded for the step called  (so ref 1, metric 1 only gets set when calling step 1, etc...)
-- Anything over step 3 just marks complete
-- =================================================================================================================================================

CREATE PROC [dbo].[usp_LogBulkImportCompleteStep] (
	@LogId int
,	@Step tinyint
,	@IsComplete tinyint
,	@ErrOccurred tinyint = 0
,	@ErrMessage varchar(256) = ''
,	@UnitOfWork varchar(128) = ''  -- only call for step 1
,	@RefId bigint = 0
,	@Metric int = 0
,	@OtherId1 bigint = 0
,	@OtherId2 bigint = 0
,	@OtherId3 bigint = 0
	)
AS

	-- Check for errors first
	IF @ErrOccurred = 1 
		BEGIN
			UPDATE dbo.LogBulkImport
			SET ErrOccurred = 1
			, ErrMessage = CONCAT('STEP ', @Step,' Error: ', @ErrMessage)
			WHERE LogId = @LogId
		END

	IF @Step = 1 
		BEGIN
			UPDATE dbo.LogBulkImport
			SET Step1MS = DATEDIFF(millisecond, StartAt, SYSDATETIME())
			,	Step1Complete = SYSDATETIME()
			,	RefId1 = @RefId
			,	Metric1 = @Metric
			,	TotalMS = CASE WHEN @IsComplete = 1 THEN DATEDIFF(millisecond, StartAt, SYSDATETIME()) ELSE 0 END
			,	TotalComplete = CASE WHEN @IsComplete = 1 THEN SYSDATETIME() ELSE NULL END
			,	UnitOfWork = @UnitOfWork
			,	OtherId1 = CASE WHEN OtherId1 = 0 THEN @OtherId1 ELSE OtherId1 END
			,	OtherId2 = CASE WHEN OtherId2 = 0 THEN @OtherId2 ELSE OtherId2 END
			,	OtherId3 = CASE WHEN OtherId3 = 0 THEN @OtherId3 ELSE OtherId3 END
			WHERE LogId = @LogId
			RETURN 0
		END

	IF @Step = 2
		BEGIN
			UPDATE dbo.LogBulkImport
			SET Step2MS = DATEDIFF(millisecond, Step1Complete, SYSDATETIME())
			,	Step2Complete = SYSDATETIME()
			,	RefId2 = @RefId
			,	Metric2 = @Metric
			,	TotalMS = CASE WHEN @IsComplete = 1 THEN DATEDIFF(millisecond, StartAt, SYSDATETIME()) ELSE 0 END
			,	TotalComplete = CASE WHEN @IsComplete = 1 THEN SYSDATETIME() ELSE NULL END
			,	OtherId1 = CASE WHEN OtherId1 = 0 THEN @OtherId1 ELSE OtherId1 END
			,	OtherId2 = CASE WHEN OtherId2 = 0 THEN @OtherId2 ELSE OtherId2 END
			,	OtherId3 = CASE WHEN OtherId3 = 0 THEN @OtherId3 ELSE OtherId3 END
			WHERE LogId = @LogId
			RETURN 0
		END

	IF @Step = 3
		BEGIN
			UPDATE dbo.LogBulkImport
			SET Step3MS = DATEDIFF(millisecond, Step3Complete, SYSDATETIME())
			,	Step3Complete = SYSDATETIME()
			,	RefId3 = @RefId
			,	Metric3 = @Metric
			,	TotalMS = CASE WHEN @IsComplete = 1 THEN DATEDIFF(millisecond, StartAt, SYSDATETIME()) ELSE 0 END
			,	TotalComplete = CASE WHEN @IsComplete = 1 THEN SYSDATETIME() ELSE NULL END
			,	OtherId1 = CASE WHEN OtherId1 = 0 THEN @OtherId1 ELSE OtherId1 END
			,	OtherId2 = CASE WHEN OtherId2 = 0 THEN @OtherId2 ELSE OtherId2 END
			,	OtherId3 = CASE WHEN OtherId3 = 0 THEN @OtherId3 ELSE OtherId3 END
			WHERE LogId = @LogId
			RETURN 0
		END

	IF @Step > 3 OR @Step < 1
		BEGIN
			UPDATE dbo.LogBulkImport
			SET TotalMS = DATEDIFF(millisecond, StartAt, SYSDATETIME())
			,	TotalComplete = SYSDATETIME()
			,	OtherId1 = CASE WHEN OtherId1 = 0 THEN @OtherId1 ELSE OtherId1 END
			,	OtherId2 = CASE WHEN OtherId2 = 0 THEN @OtherId2 ELSE OtherId2 END
			,	OtherId3 = CASE WHEN OtherId3 = 0 THEN @OtherId3 ELSE OtherId3 END
			WHERE LogId = @LogId
			RETURN 0
		END

	

GO


-------------------------------------------------------------------------------------------------------------------
-- POPULATE JOB TABLE
-------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM [dbo].[LogBulkImportJobDef] WHERE JobId = 1) --'EDMS Initial Load'
DELETE FROM [dbo].[LogBulkImportJobDef] WHERE JobId = 1
GO

INSERT INTO [dbo].[LogBulkImportJobDef]
           ([JobId]
           ,[JobDesc]
           ,[JobContact]
		   ,[UnitOfWorkDesc]
           ,[RefId1Desc]
           ,[RefId2Desc]
           ,[RefId3Desc]
           ,[OtherId1Desc]
           ,[OtherId2Desc]
           ,[OtherId3Desc]
           ,[Metric1Desc]
           ,[Metric2Desc]
           ,[Metric3Desc]
           ,[NotesOnJob])
     VALUES
           (1
           ,'EDMS Initial Load'
           ,'Richard Varno richard.varno@maximus.com 615.479.1984'
		   ,'Tibco Message Id'
           ,'Document Id'
           ,''
           ,''
           ,'Tibco Document Id'
           ,'Tibco Document Sequence Id'
           ,''
           ,'Document File Size'
           ,''
           ,''
           ,'Read Messages from Gainwell Tibco MQ Server and Save to Document Table')

GO




-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                    APPLICATION PROCS                     ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################


-------------------------------------------------------------------------------------------------------------------
-- create usp_SaveProviderDocumentFromTibcoMQ 
-------------------------------------------------------------------------------------------------------------------


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SaveProviderDocumentFromTibcoMQ]'))
DROP PROCEDURE [dbo].usp_SaveProviderDocumentFromTibcoMQ
GO

/****** Object:  StoredProcedure [dbo].[usp_SaveProviderDocumentFromTibcoMQ]    Script Date: 12/30/2020 1:22:58 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =================================================================================================================================================
-- Author:		Richard Varno
-- Date:		09/01/2020
-- Description:	Save Provider Document called int TibcoDotNetCoreImport
-- =================================================================================================================================================

CREATE PROC [dbo].[usp_SaveProviderDocumentFromTibcoMQ] (
	@IDXML varchar(8000)
,	@DOC_TYPE_CODE varchar(10)
,	@FILE_NAME varchar(100)
	)
AS

-- EXEC dbo.usp_SaveProviderDocumentFromTibcoMQ '<root><entry><xkey>ATN</xkey><xval>509567</xval></entry></root>', '036', '6513253000345.jpeg'
/*
ID XML Will be in this format  
ATN = Application ID (REG_ID found in REG_APPLICATION)
PID = Medicare Provider # (REG_ID found in REG_SERVICE_LOCATION)
NPI = National Provider 

<ROOT>  
        <entry>
            <xkey>ATN</xkey>  
            <xvalue>123</xvalue> 
        </entry>
        <entry>
            <xkey>PID</xkey>  
            <xvalue>12345678</xvalue>
        </entry>
</ROOT>

*/
	-------------------------------------------------------------------------------
	-- RELEVANT TABLES
	-------------------------------------------------------------------------------
	-- SELECT TOP 1000 * FROM REG_MEDICAID -- WHERE MEDICAID_NUMBER IS NOT NULL
	-- SELECT TOP 100 * FROM dbo.REG_DOCUMENT_XREF ****
	-- SELECT TOP 100 * FROM dbo.DOCUMENT ****
	-- SELECT TOP 100 * FROM dbo.DOCUMENT_INDEX ****  ADD Type ID instead of XREF TYPE
	-- SELECT TOP 100 * FROM dbo.DOCUMENT_TYPE
	-- SELECT TOP 100 * FROM dbo.DOCUMENT_XREF_TYPE where document_xref_type_desc like '%provider%'  ****
	-- SELECT TOP 100 * FROM [dbo].[REG_APPLICATION] ORDER BY DATE_SIGNED DESC
	-- SELECT TOP 100 * FROM [dbo].[REG_SERVICE_LOCATION]
	-- SELECT TOP 100 * FROM [dbo].[REG_PROVIDER] WHERE NPI IS NOT NULL
	-- SELECT TOP 100 * FROM [dbo].REG_ALTERNATE_ID WHERE NPI IS NOT NULL
	-- SELECT TOP 100 * FROM dbo.DOCUMENT_ATTACHMENT_XREF 
 

	-- SELECT * FROM PROV_ID_TYPE
	 
	-- EXEC dbo.usp_SaveProviderDocumentFromTibcoMQ '<root><entry><xkey>ATN</xkey><xval>407777</xval></entry></root>', '036', '6513253000345.jpeg'

	DECLARE @REG_ID int 
		,	@DOC_ID INT
		,	@DOC_TYPE_ID INT
		,	@NPI varchar(20)
		,	@ATN varchar(20)
		,	@PID varchar(80)
		,	@APP_ID int
		,	@USER_ID uniqueidentifier
		,	@XDOC int

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

	BEGIN TRANSACTION

	BEGIN TRY

		SET @USER_ID = CONVERT(uniqueidentifier, '11111111-1111-1111-1111-111111111111')

		-------------------------------------------------------------------------------------------------
		-- 1) LOAD IDENTIFIERS TO TABLE AND FIND REG_ID 
		-------------------------------------------------------------------------------------------------
		DECLARE @ids TABLE (
			xkey VARCHAR(50) NULL,
			xval VARCHAR(50) NULL
		);

		EXEC sp_xml_preparedocument @XDOC OUTPUT, @IDXML;  
		INSERT INTO @ids(xkey, xval) 
		SELECT TRIM(xkey) as xkey, TRIM(xval) as xval 
		FROM OPENXML (@XDOC, '/root/entry',2) WITH (xkey  VARCHAR(10), xval VARCHAR(20));  

		/* possible values 
		ATN = Application ID (REG_ID found in REG_APPLICATION)
		PID = Medicare Provider # (REG_ID found in REG_SERVICE_LOCATION)
		NPI = National Provider 
		*/

		SET @REG_ID = 0;
		/*
		------------------------------------------------------------------------
		-- Try Lookup by PID
		------------------------------------------------------------------------
		SET @PID = ISNULL((SELECT MAX(xval) FROM @ids WHERE xkey = 'PID'), '#NOTFOUND#');
		IF (@PID <> '#NOTFOUND#')
			BEGIN			
				SET @REG_ID = ISNULL((SELECT MAX(REG_ID) FROM dbo.REG_SERVICE_LOCATION WHERE MEDICAID_ID = @PID), 0);
				IF @REG_ID > 0 GOTO ID_FOUND;
			END

		------------------------------------------------------------------------
		-- Try Lookup by NPI
		------------------------------------------------------------------------
		SET @NPI = ISNULL((SELECT MAX(xval) FROM @ids WHERE xkey = 'NPI'), '#NOTFOUND#');
		IF (@NPI <> '#NOTFOUND#')
			BEGIN
				SET @REG_ID = ISNULL((SELECT MAX(REG_ID) FROM dbo.REG_PROVIDER WHERE NPI = @NPI), 0);
				IF @REG_ID > 0 GOTO ID_FOUND;
				
				-- try alternate id  (SELECT * FROM PROV_ID_TYPE)[NPI = 7]
				SET @REG_ID = ISNULL((SELECT MAX(REG_ID) FROM dbo.REG_ALTERNATE_ID WHERE ALTERNATE_ID = @NPI AND ALTERNATE_ID_TYPE_ID = 7), 0);
				IF @REG_ID > 0 GOTO ID_FOUND;

			END
		
		------------------------------------------------------------------------
		-- Try Lookup by ATN
		------------------------------------------------------------------------
		SET @ATN = ISNULL((SELECT MAX(xval) FROM @ids WHERE xkey = 'ATN'), '#NOTFOUND#');
		IF (@ATN <> '#NOTFOUND#' )
			BEGIN
				IF ISNUMERIC(@ATN) = 1
					BEGIN
						SET @APP_ID = CONVERT(int, @ATN);
						SET @REG_ID = ISNULL((SELECT MAX(REG_ID) FROM dbo.reg_application WHERE APPLICATION_ID = @APP_ID), 0);
						IF @REG_ID > 0 GOTO ID_FOUND;
					END
			END
		*/
		-- if we get here no idea what to do, so abort  EDIT - STILL SAVE DOC EVEN IF REG_ID is Zero (will figure out later)
		--IF @REG_ID = 0 
		--	BEGIN
		--		ROLLBACK TRANSACTION
		--		-- RAISERROR ('Provider Not Found',2,1);  handle by return
		--		SELECT 'REG_ID_NOT_FOUND' AS RTN
		--		RETURN 0
		--	END
		
		ID_FOUND:

		-------------------------------------------------------------------------------------------------
		-- 2) CREATE DOCUMENT RECORD
		-------------------------------------------------------------------------------------------------
		
		-- select top 10 * from document

		INSERT INTO [dbo].[DOCUMENT]
		(	[NAME]
		,	[DESCRIPTION]
		,	[FILE_NAME]
		,	[LAST_MODIFIED_DATE_TIME]
		,	[LAST_MODIFIED_USER]
		,	[IS_CONVERSION])
		VALUES
		(	@FILE_NAME
		,	''
		,	@FILE_NAME
		,	GETDATE() 
		,	@USER_ID
		,	1);

		SET @DOC_ID = SCOPE_IDENTITY();

		-------------------------------------------------------------------------------------------------
		-- 3) CREATE REG_DOCUMENT_XREF RECORD
		-------------------------------------------------------------------------------------------------

		-- SELECT TOP 10 * FROM dbo.REG_DOCUMENT_XREF

		IF (@REG_ID > 0)  -- only if reg is found
			BEGIN
				INSERT INTO [dbo].[REG_DOCUMENT_XREF]
				(	[REG_ID]
				,	[REG_PAGE_TYPE_ID]
				,	[DOCUMENT_ID]
				,	[LAST_MODIFIED_DATE_TIME]
				,	[LAST_MODIFIED_USER])
				VALUES
				   (	@REG_ID
				   ,	0
				   ,	@DOC_ID
				   ,	GETDATE()
				   ,	@USER_ID);
			END

		-------------------------------------------------------------------------------------------------
		-- 3) CREATE ATTACHMENT_XREF
		-------------------------------------------------------------------------------------------------
		-- SELECT TOP 10 * FROM dbo.DOCUMENT_ATTACHMENT_XREF
		-- SELECT TOP 10 * FROM dbo.DOCUMENT_TYPE

		SET @DOC_TYPE_ID = ISNULL((SELECT MIN(DOCUMENT_TYPE_ID) FROM dbo.DOCUMENT_TYPE WHERE DOCUMENT_TYPE_CODE = @DOC_TYPE_CODE), 0);

		-- for not found types, use 0 so we can find / address later
		INSERT INTO [dbo].[DOCUMENT_ATTACHMENT_XREF]
           (	[DOCUMENT_TYPE_ID]
           ,	[DOCUMENT_ID]
           ,	[NOTES]
           ,	[DOCUMENT_RECEIVED_DATE]
           ,	[CREATED_MODIFIED_DATE_TIME]
           ,	[CREATED_BY_MODIFIED_USER]
           ,	[LAST_MODIFIED_DATE_TIME]
           ,	[LAST_MODIFIED_USER]
           ,	[RetrieveReport_Type_ID])
		VALUES
           (	@DOC_TYPE_ID
           ,	@DOC_ID
           ,	''
           ,	GETDATE()
           ,	GETDATE()
           ,	@USER_ID
           ,	GETDATE()
           ,	@USER_ID
           ,	0);


		-------------------------------------------------------------------------------------------------
		-- 4) CREATE DOCUMENT INDEX RECORDS  (from xml table)
		-------------------------------------------------------------------------------------------------
		-- SELECT TOP 10 * FROM dbo.DOCUMENT_INDEX
		-- SELECT TOP 10 * FROM dbo.DOCUMENT_XREF_TYPE where document_xref_type_desc like '%provider%'

		INSERT INTO [dbo].[DOCUMENT_INDEX]
		(	[DOCUMENT_ID]
		,	[INDEXID]
		,	[CREATED_MODIFIED_DATE_TIME]
		,	[CREATED_BY_MODIFIED_USER]
		,	[LAST_MODIFIED_DATE_TIME]
		,	[LAST_MODIFIED_USER]
		,	[DOCUMENT_XREF_TYPE_ID])
		SELECT @DOC_ID AS DOC_ID
		,	i.xval AS INDEXID
		,	GETDATE() AS CDT
		,	@USER_ID AS CUSER
		,	GETDATE() AS UDT
		,	@USER_ID AS UUSER
		,	t.DOCUMENT_XREF_TYPE_ID
		FROM @ids as i
		INNER JOIN dbo.DOCUMENT_XREF_TYPE as t ON t.DOCUMENT_XREF_TYPE = i.xkey;

		-------------------------------------------------------------------------------------------------
		-- 4) CREATE CONVERTED DOCUMENT XREF
		-------------------------------------------------------------------------------------------------
		INSERT INTO [dbo].[REG_CONVERTED_DOCUMENT_XREF]
		(	 [REG_ID]
			,[DOCUMENT_ID]
			,[DOC_TYPE]
			,[DTL_DOC_TYPE]
			,[SUB_DOC_TYPE]
			,[LAST_MODIFIED_DATE_TIME]
			,[LAST_MODIFIED_USER])
		SELECT  @REG_ID as REG_ID
			,	@DOC_ID as DOC_ID
			,	@DOC_TYPE_ID as DOC_TYPE
			,	t.DOCUMENT_SERVICE_NAME
			,	t.DOCUMENT_TYPE_DESC
			,	GETDATE() AS UDT
			,	@USER_ID AS CUSER
		FROM dbo.DOCUMENT_TYPE as t 
		WHERE t.DOCUMENT_TYPE_ID = @DOC_TYPE_ID

	COMMIT TRANSACTION

	SELECT CONCAT('DOC_ID:', @DOC_ID, ':REG_ID:', @REG_ID) AS RTN

END TRY

BEGIN CATCH
	DECLARE @ErrorMessage NVARCHAR(4000);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;

	SELECT @ErrorMessage = ERROR_MESSAGE()
		,@ErrorSeverity = ERROR_SEVERITY()
		,@ErrorState = ERROR_STATE();

	RAISERROR (
			@ErrorMessage
			,@ErrorSeverity
			,@ErrorState
			);

	ROLLBACK TRANSACTION
END CATCH;
GO


-------------------------------------------------------------------------------------------------------------------
-- create usp_SetOnBaseDocumentID 
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_SetOnBaseDocumentID]'))
DROP PROCEDURE [dbo].usp_SetOnBaseDocumentID
GO

/****** Object:  StoredProcedure [dbo].[usp_SetOnBaseDocumentID]    Script Date: 7/6/2021 3:09:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp_SetOnBaseDocumentID](
	@DOCUMENT_ID INT,
	@ONBASE_DOCUMENT_ID INT
) 
AS
---- For testing purpose only
--DECLARE @ONBASE_DOCUMENT_ID INT = 154
--DECLARE @DOCUMENT_ID INT = 423

BEGIN
	UPDATE DOCUMENT SET ONBASE_DOCUMENT_ID = @ONBASE_DOCUMENT_ID WHERE DOCUMENT_ID=@DOCUMENT_ID
END
GO


-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                     MONITORING PROCS                     ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################

-------------------------------------------------------------------------------------------------------------------
-- create usp_WorkerCurrentActive
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_WorkerCurrentActive]'))
DROP PROCEDURE [dbo].usp_WorkerCurrentActive
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].usp_WorkerCurrentActive

AS

BEGIN
	
	-- must be active in the last 10 minutes
	SELECT 
		WorkerId
	,	MAX(StatusTimestamp) as LastActive
	,	MAX(StatusMessage) as RecentMessage
	FROM dbo.LogBulkImport WHERE StatusTimestamp > DATEADD(minute, -10, getdate()) OR TotalComplete > DATEADD(minute, -10, getdate())
	GROUP BY WorkerId
	ORDER BY WorkerId

END
GO



-------------------------------------------------------------------------------------------------------------------
-- create usp_WorkersStatsPerMinuteByMostRecent
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_WorkersStatsPerMinuteByMostRecent]'))
DROP PROCEDURE [dbo].usp_WorkersStatsPerMinuteByMostRecent
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].usp_WorkersStatsPerMinuteByMostRecent

AS

BEGIN
	
	-----------------------------------------------------
	-- Stats per minute / # of workers by most recent
	-----------------------------------------------------
	SELECT WorkerCount, MaxWrk, MAX(DocsPerMin) as MaxDocs, AVG(DocsPerMin) as AvgDocs FROM (

	SELECT minGRP
		,	COUNT(*) as DocsPerMin
		,	COUNT(DISTINCT WorkerId) as WorkerCount
		,	MAX(WorkerId) as MaxWrk
		,	SUM(FileMB) as TotalMB
		,	AVG(FileMB) as AverageMB
		,	AVG(TotalMS) as AverageTotalMS
		,	AVG(Step1MS) as AverageStep1MS
		,	AVG(Step2MS) as AverageStep2MS
	FROM (
		SELECT 
			WorkerId
		,	TotalMS
		,	Step1MS
		,	Step2MS
		,	(Metric2 * 0.00000095367432) as FileMB
		,	DATEDIFF(minute, dateadd(dd, datediff(dd, 0, getdate())+0, 0), TotalComplete) as minGRP
		FROM dbo.LogBulkImport WHERE refid1 > 0
	) as x
	GROUP By minGRP

	) as y group by WorkerCount, MaxWrk -- ORDER BY mingrp DESC

END
GO


-------------------------------------------------------------------------------------------------------------------
-- create usp_WorkersStatsOverall
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_WorkersStatsOverall]'))
DROP PROCEDURE [dbo].usp_WorkersStatsOverall
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].usp_WorkersStatsOverall

AS

BEGIN

	-----------------------------------------------------
	-- Overall Stats per minute / # of workers
	-----------------------------------------------------
	SELECT WorkerCount, AVG(DocsPerMin) as AvgDocsPerMin, AVG(TotalMB) as AvgMBPerMin FROM (

	SELECT minGRP
		,	COUNT(*) as DocsPerMin
		,	COUNT(DISTINCT WorkerId) as WorkerCount
		,	SUM(FileMB) as TotalMB
		,	AVG(FileMB) as AverageMB
		,	AVG(TotalMS) as AverageTotalMS
	FROM (
		SELECT 
			WorkerId
		,	TotalMS
		,	(Metric2 * 0.00000095367432) as FileMB
		,	DATEDIFF(minute, dateadd(dd, datediff(dd, 0, getdate())+0, 0), TotalComplete) as minGRP
		FROM dbo.LogBulkImport WHERE refid1 > 0
	) as x
	GROUP By minGRP

	) as y GROUP BY WorkerCount ORDER BY WorkerCount DESC

END
GO




-------------------------------------------------------------------------------------------------------------------
-- create usp_WorkersStatsByWorker
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_WorkersStatsByWorker]'))
DROP PROCEDURE [dbo].usp_WorkersStatsByWorker
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].usp_WorkersStatsByWorker

AS

BEGIN

	-----------------------------------------------------
	-- Overall Stats per minute / by workers
	-----------------------------------------------------
	SELECT WorkerId, AVG(DocsPerMin) as AvgDocsPerMin, AVG(TotalMB) as AvgMBPerMin FROM (

	SELECT WorkerId
		,	COUNT(*) as DocsPerMin
		,	SUM(FileMB) as TotalMB
		,	AVG(FileMB) as AverageMB
		,	AVG(TotalMS) as AverageTotalMS
	FROM (
		SELECT 
			WorkerId
		,	TotalMS
		,	(Metric2 * 0.00000095367432) as FileMB
		FROM dbo.LogBulkImport WHERE refid1 > 0
	) as x
	GROUP By WorkerId

	) as y GROUP BY WorkerId ORDER BY WorkerId DESC

END
GO


-------------------------------------------------------------------------------------------------------------------
-- create usp_WorkerFullOverview
-------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_WorkerFullOverview]'))
DROP PROCEDURE [dbo].usp_WorkerFullOverview
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].usp_WorkerFullOverview

AS

BEGIN
	
	-- get all stats for each worker
	
	SELECT	w.WorkerId
		,	w.StatusMessage as LastStatusMessage
		,	CONCAT(DATEDIFF(MINUTE, m.LastStamp, GETDATE()), ' minutes ago') as LastActive
		,	CASE WHEN DATEDIFF(MINUTE, m.LastStamp, GETDATE()) > 5 THEN 'INACTIVE' ELSE 'ACTIVE' END as WorkerStatus
		,	s.TotalDocs
		,	s.TotalMB
		,	s.AverageMB
		,	s.AverageTotalMS
		,	s.DosPerMin
		,	w.ErrOccurred 
		,	w.ErrMessage
	FROM dbo.LogBulkImport as w
	INNER JOIN (SELECT WorkerId, MAX(LogId) as MaxLog, IIF(MAX(TotalComplete) > MAX(StatusTimestamp), MAX(TotalComplete), MAX(StatusTimestamp)) as LastStamp FROM dbo.LogBulkImport GROUP BY WorkerId) as m ON m.MaxLog = w.LogId
	LEFT JOIN (SELECT WorkerId
					,	COUNT(*) as TotalDocs
					,	SUM(FileMB) as TotalMB
					,	AVG(FileMB) as AverageMB
					,	AVG(TotalMS) as AverageTotalMS
					,	COUNT(*) / (SUM(TotalMS)/60000) as DosPerMin
				FROM (
					SELECT 
						WorkerId
					,	TotalMS
					,	(Metric2 * 0.00000095367432) as FileMB
					FROM dbo.LogBulkImport WHERE refid1 > 0
				) as x
				GROUP By WorkerId) as s ON s.WorkerId = w.WorkerId
	ORDER BY w.WorkerId

END
GO


-- ##############################################################################################################################
-- ##############################################################################################################################
-- ##################################                                                          ##################################
-- ##################################                  END MONITORING PROCS                    ##################################
-- ##################################                                                          ##################################
-- ##############################################################################################################################
-- ##############################################################################################################################

