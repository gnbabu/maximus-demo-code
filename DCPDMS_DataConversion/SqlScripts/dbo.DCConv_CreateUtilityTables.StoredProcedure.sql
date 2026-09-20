/****** Object:  StoredProcedure [dbo].[DCConv_CreateUtilityTables]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_CreateUtilityTables]','P') is not null
DROP PROCEDURE [dbo].DCConv_CreateUtilityTables
GO
/****** Object:  StoredProcedure [dbo].[DCConv_CreateUtilityTables]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 6/2/2016
-- Description:	Creates the tables used by conversion
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_CreateUtilityTables] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- create the errors table if it doesn't exist already
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'DCConv_Errors'))
	BEGIN
		CREATE TABLE [dbo].[DCConv_Errors](
			[Error_ID] [int] IDENTITY(1,1) NOT NULL,
			[RunID] [varchar](20) NULL,
			[TableContext] [varchar](50) NULL,
			[ColumnContext] [varchar](50) NULL,
			[ErrorLine] [int] NULL,
			[ErrorNumber] [int] NULL,
			[ErrorMessage] [varchar](4000) NULL,
			[ErrorProcedure] [varchar](128) NULL,
			[Timestamp] [datetime] NULL,
		 CONSTRAINT [PK_DCConv_Errors] PRIMARY KEY CLUSTERED 
		(
			[Error_ID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
		
		ALTER TABLE [dbo].[DCConv_Errors] ADD  CONSTRAINT [DF_DCConv_Errors_Timestamp]  DEFAULT (getdate()) FOR [Timestamp]
	END

	-- create the registration cross reference table if it doesn't exist already
	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'DCConv_KeyCrossReferences'))
	BEGIN
		CREATE TABLE [dbo].[DCConv_KeyCrossReferences](
			[SysID] [int] NULL,
			[RegistrationID] [int] NULL
		) ON [PRIMARY]
	END

	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'DCConv_ConversionErrors'))
	BEGIN
		CREATE TABLE [dbo].[DCConv_ConversionErrors](
			[ConversionErrorID] [int] IDENTITY(1,1) NOT NULL,
			[DataType] [varchar](50) NULL,
			[SysID] [int] NULL,
			[ProviderID] [varchar](10) NULL,
			[ErrorCode] [int] NULL,
			[ErrorMessage] [varchar](1000) NULL,
		 CONSTRAINT [PK_DCConv_ConversionErrors] PRIMARY KEY CLUSTERED 
		(
			[ConversionErrorID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END

	IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'DCConv_Counts'))
	BEGIN
		CREATE TABLE [dbo].[DCConv_Counts](
			[ConversionCountID] [int] IDENTITY(1,1) NOT NULL,
			[ConversionRunID] [varchar](50) NULL,
			[DataTypeDescription] [varchar](50) NULL,
			[NumberOfRecordsConverted] [int] NULL,
			[NumberOfFailedRecords] [int] NULL,
			[TotalRecordsProcessed] [int] NULL,
		 CONSTRAINT [PK_DCConv_Counts] PRIMARY KEY CLUSTERED 
		(
			[ConversionCountID] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
		) ON [PRIMARY]
	END
	
	IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'DCConv_DataElementsToValidate'))
		DROP TABLE [dbo].[DCConv_DataElementsToValidate]

	CREATE TABLE [dbo].[DCConv_DataElementsToValidate](
		[SOURCE_TABLE] [varchar](50) NULL,
		[ELEMENT_TO_VALIDATE] [varchar](50) NULL
	) ON [PRIMARY]

	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Enrollments', N'P-STAT-EFF-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Enrollments', N'P-ENROL-STAT-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Enrollments', N'P-STAT-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-REC-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-INDIV-GRP-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-APPL-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAltIDs', N'P-ALT-ID-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAltIDs', N'P-ALT-ID-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAltIDs', N'P-ALT-ID-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Enrollments', N'P-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-NAM-ORG-IND')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-DOB-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderTaxIds', N'P-TAX-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAddresses', N'G-QUAD-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAddresses', N'G-WARD-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderLicense', N'P-LIC-CERT-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderLicense', N'P-LIC-EFF-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderLicense', N'LICENSE_END_DATE')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderCLIADetails', N'P-CLIA-CERT-EFF-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderCLIADetails', N'P-CERT-EXPIR-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderMedicare', N'P-MCARE-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderMedicare', N'P-MCARE-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAddresses', N'P-ST-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderQuestions', N'P-QSTNR-NPP-IND')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderQuestions', N'P-QSTNR-NPR-IND')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAddresses', N'P-ADR-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-DEA-EFF-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-DEA-EXP-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderMedicaid', N'P-PREV-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderMedicaid', N'P-PREV-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_Providers', N'P-OWNER-TY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderCatOfService', N'P-COS-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderCatOfService', N'P-COS-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderCatOfService', N'P-COS-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderTaxonomy', N'P-TAXONOMY-CD')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderTaxonomy', N'P-TAXON-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderTaxonomy', N'P-TAXON-END-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAffiliates', N'P-AFFL-BEG-DT')
	INSERT [dbo].[DCConv_DataElementsToValidate] ([SOURCE_TABLE], [ELEMENT_TO_VALIDATE]) VALUES (N'SRC_ProviderAffiliates', N'P-AFFL-END-DT')
END


GO
