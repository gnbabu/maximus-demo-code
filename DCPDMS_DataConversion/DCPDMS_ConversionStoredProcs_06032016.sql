USE [DC_PDMS_RBM01]
GO

/****** Object:  StoredProcedure [dbo].[DCConv_ClearRegistrationData]    Script Date: 6/3/2016 11:50:08 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/10/2016
-- Description:	Deletes the content of the registration tables
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ClearRegistrationData] 
(
	@pin_conv_run_id varchar(20)
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @table varchar(50);
	SET @table = '';
	BEGIN TRY
		BEGIN TRANSACTION;

		SET @table = 'REG_TAXONOMY';
		DELETE FROM [dbo].REG_TAXONOMY;
	
		SET @table = 'REG_SPECIALTY';
		DELETE FROM [dbo].REG_SPECIALTY;
		
		SET @table = 'REG_LICENSE';
		DELETE FROM [dbo].REG_LICENSE;
		
		SET @table = 'REG_CLIA';
		DELETE FROM [dbo].REG_CLIA;
		
		SET @table = 'REG_MEDICARE';
		DELETE FROM [dbo].REG_MEDICARE;
		
		SET @table = 'REG_SERVICE_LOCATION';
		DELETE FROM [dbo].REG_SERVICE_LOCATION;
		
		SET @table = 'REG_ADDITIONAL_ADDRESSES';
		DELETE FROM [dbo].REG_ADDITIONAL_ADDRESSES;
		
		SET @table = 'REG_DEA';
		DELETE FROM [dbo].REG_DEA;
		
		SET @table = 'REG_MEDICAID';
		DELETE FROM [dbo].REG_MEDICAID;
		
		SET @table = 'REG_OWNER';
		DELETE FROM [dbo].REG_OWNER;
	
		SET @table = 'REG_PROVIDER';
		DELETE FROM [dbo].REG_PROVIDER;

		SET @table = 'REGISTRATION';
		DELETE FROM [dbo].REGISTRATION

		SET @table = 'DCConv_KeyCrossReferences';
		DELETE FROM [dbo].DCConv_KeyCrossReferences;

		
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorLine int;
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorProcedure varchar(128);

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorProcedure = ERROR_PROCEDURE();

		-- add the error to the log
		INSERT INTO [dbo].[DCConv_Errors]([RunID],[TableContext],[ColumnContext],[ErrorLine],[ErrorNumber],[ErrorMessage],[ErrorProcedure])
		VALUES
        (@pin_conv_run_id,@table,'', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_ConvertDCDataToPDMS]    Script Date: 6/3/2016 11:50:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 6/2/2016
-- Description:	Populates the PDMS REG tables with data from DC
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertDCDataToPDMS] 
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- create the utility tables
	EXEC DCConv_CreateUtilityTables

	-- drop and re-create synonyms
	EXEC DCConv_DropDCStagingTableSynonyms;
	EXEC DCConv_CreateDCStagingTableSynonyms;

	-- clear out any old data
	EXEC DCConv_ClearRegistrationData @pin_conv_run_id;

	-- create the taxonomy and specialty reference value tables
	EXEC DCConv_ConvertReferenceValues @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	-- convert the groups
	EXEC DCConv_ConvertGroups @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	-- convert individuals
	EXEC DCConv_ConvertIndividuals @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	-- convert affiliations
	--EXEC DCConv_ConvertAffiliations @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_ConvertGroups]    Script Date: 6/3/2016 11:50:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/10/2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertGroups] 
(
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
	
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @currentTable varchar(50);
	DECLARE @MaxRegistrationID int;

	BEGIN TRY
--		BEGIN TRANSACTION

		SELECT @MaxRegistrationID = ISNULL(MAX(RegistrationID), 0) FROM DCConv_KeyCrossReferences;
		-- add the mapping for the group ids
		-- choose all providers with affiliates as group members
		INSERT INTO DCConv_KeyCrossReferences
		SELECT DISTINCT en.[P-SYS-ID], ROW_NUMBER() OVER (ORDER BY en.[P-SYS-ID]) + @MaxRegistrationID, 1
		FROM SRC_Enrollments en
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		WHERE prov.[P-REC-TY-CD] = 'P' AND 
		(SELECT COUNT(*) FROM SRC_ProviderAffiliates aff 
		WHERE aff.[P-GROUP-SYS-ID] = en.[P-SYS-ID] AND 
		aff.[P-MEMBER-SYS-ID] IS NOT NULL) > 0 ORDER BY en.[P-SYS-ID];

		

		SELECT * FROM DCConv_KeyCrossReferences g1 WHERE (SELECT COUNT(*) FROM DCConv_KeyCrossReferences g2 WHERE g1.SysID=g2.SysID) > 0 ORDER BY RegistrationId;

		-- convert the registration data
		SET @currentTable = 'REGISTRATION';

		SET IDENTITY_INSERT dbo.[REGISTRATION] ON;

		INSERT INTO [dbo].[REGISTRATION] ([REG_ID], 
			[REQUESTED_EFFECTIVE_DATE],
			[CHANGE_EFFECTIVE_DATE],
			[REGISTRATION_STATUS_TYPE_ID],
			[LAST_MODIFIED_DATE_TIME],
			[LAST_MODIFIED_USER],
			[REG_PROGRAM_STATUS_TYPE_ID],
			[DIDD_REFERRAL_ID],
			[SUBMIT_DATE_TIME],
			[DIDD_COMMISSIONER_DATE_TIME],
			[PECOS_VERIFIED],
			[IS_PAPER_APPLICATION],
			[REG_CREATE_DATE_TIME])
		SELECT DISTINCT map.RegistrationId AS REG_ID, 
		(SELECT MIN(dbo.fn_ConvertDCDateToPDMS([P-STAT-EFF-DT])) FROM SRC_Enrollments WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-SYS-ID] = map.SysId) AS [REQUESTED_EFFECTIVE_DATE],
		(SELECT MIN(dbo.fn_ConvertDCDateToPDMS([P-STAT-EFF-DT])) FROM SRC_Enrollments WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-SYS-ID] = map.SysId) AS [CHANGE_EFFECTIVE_DATE],
		1 AS [REGISTRATION_STATUS_TYPE_ID],
		@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
		dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
		6 AS [REG_PROGRAM_STATUS_TYPE_ID],
		NULL AS [DIDD_REFERRAL_ID],  -- to be filled in once the DIDD data are gathered
		dbo.fn_ConvertDCDateToPDMS(prov.[P-APPL-DT]) AS [SUBMIT_DATE_TIME],
		NULL AS [DIDD_COMMISSIONER_DATE_TIME], -- to be filled in once the DIDD data are gathered
		0 AS [PECOS_VERIFIED],
		NULL AS [IS_PAPER_APPLICATION],
		@pin_conv_run_time AS [REG_CREATE_DATE_TIME]
		FROM DCConv_KeyCrossReferences map 
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		WHERE map.IsGroup = 1;

		SET IDENTITY_INSERT dbo.[REGISTRATION] OFF;

		---- convert the providers
		SET @currentTable = 'REG_PROVIDER';
		INSERT INTO [dbo].[REG_PROVIDER]
				   ([REG_ID]
				   ,[NAME]
				   ,[PARTY_ID]
				   ,[DBA]
				   ,[NPI]
				   ,[TAX_ID]
				   ,[ENTITY_TYPE_ID]
				   ,[PROVIDER_TYPE_ID]
				   ,[CONTACT_NAME]
				   ,[CONTACT_ADDRESS1]
				   ,[CONTACT_ADDRESS2]
				   ,[CONTACT_CITY]
				   ,[CONTACT_STATE]
				   ,[CONTACT_ZIP]
				   ,[CONTACT_EXT_ZIP]
				   ,[CONTACT_PHONE_NUMBER]
				   ,[CONTACT_FAX_NUMBER]
				   ,[CONTACT_EMAIL_ADDRESS]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER]
				   ,[ENROLLMENT_STATUS_CODE]
				   ,[TERM_DATE]
				   ,[FIRST_NAME]
				   ,[LAST_NAME]
				   ,[MIDDLE_INITIAL]
				   ,[BIRTH_DATE]
				   ,[DEATH_DATE]
				   ,[END_DATE]
				   ,[TERM_REASON_ID]
				   ,[TYPE_OF_PRACTICE_ID]
				   ,[TAX_ID_TYPE_ID]
				   ,[NPI_START_DATE]
				   ,[NPI_END_DATE])
			SELECT DISTINCT map.RegistrationId AS [REG_ID],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-NAM]))) = 0 THEN RTRIM(LTRIM(prov.[P-DBA-NAM])) ELSE RTRIM(LTRIM(prov.[P-NAM])) END AS [NAME],
				   NULL AS [PARTY_ID],
				   prov.[P-DBA-NAM] AS [DBA],
				   prov.[P-NPI-NUM] AS [NPI],
				   primary_adr.[P-TAX-KEY-ID] AS [TAX_ID],
				   dbo.fn_ConvertProviderEntityTypeToPDMS(prov.[P-SYS-ID],@pin_conv_data_export_date) 
				   AS [ENTITY_TYPE_ID],  -- this is also know as the provider category
				   dbo.fn_ConvertProviderTypeToPDMS((SELECT TOP 1 enr.[P-TY-CD] FROM SRC_Enrollments enr WHERE enr.[P-SYS-ID]=map.[SysId] AND enr.[P-ENROL-STAT-TY-CD] = '00' AND enr.[P-STAT-END-DT] > @pin_conv_data_export_date ORDER BY enr.[P-STAT-EFF-DT] DESC)) AS [PROVIDER_TYPE_ID], -- use the most recent active enrollment
				   primary_adr.[P-CONTCT-NAM] AS [CONTACT_NAME],
				   primary_adr.[P-LINE1-AD] AS [CONTACT_ADDRESS1],
				   primary_adr.[P-LINE2-AD] AS [CONTACT_ADDRESS2],
				   primary_adr.[P-CITY-NAM] AS [CONTACT_CITY],
				   primary_adr.[P-ST-CD] AS [CONTACT_STATE],
				   primary_adr.[P-ZIP5-CD] AS [CONTACT_ZIP],
				   primary_adr.[P-ZIP4-CD] AS [CONTACT_EXT_ZIP],
				   primary_adr.[P-CONTCT-PHON-NUM] AS [CONTACT_PHONE_NUMBER], 
			       primary_adr.[P-CONTCT-FAX-NUM]  AS [CONTACT_FAX_NUMBER],
				   primary_adr.[P-CONTCT-EMAIL-AD-TEXT]  AS [CONTACT_EMAIL_ADDRESS],
				   1  AS [MODIFIED_STATUS_TYPE_ID],
				   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				   dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
				   dbo.fn_ConvertEnrollmentStatusToPDMS((SELECT TOP 1 enr.[P-ENROL-STAT-TY-CD] FROM SRC_Enrollments enr WHERE enr.[P-STAT-END-DT] > @pin_conv_data_export_date  AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-EFF-DT]) DESC)) AS ENROLLMENT_STATUS_CODE, -- use the most recent enrollment status that is current
				   NULL AS TERM_DATE, -- will have to change later
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-FST-NAM]))) = 0 THEN prov.[P-DBA-FST-NAM] ELSE prov.[P-FST-NAM] END AS [FIRST_NAME],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-LAST-NAM]))) = 0 THEN prov.[P-DBA-LAST-NAM] ELSE prov.[P-LAST-NAM] END AS [LAST_NAME],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-MI-NAM]))) = 0 THEN prov.[P-DBA-MI-NAM] ELSE prov.[P-MI-NAM] END AS [MIDDLE_INITIAL],
				   dbo.fn_ConvertDCDateToPDMS(prov.[P-DOB-DT]) AS [BIRTH_DATE],
				   (SELECT TOP 1 dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) FROM SRC_Enrollments enr WHERE [P-ENROL-STAT-TY-CD] = '41' AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) DESC) AS [DEATH_DATE],  -- use the most recent date of death in case there's more than one
				   NULL AS [END_DATE],  -- *** Must ask Jon what this should be!!
				   dbo.fn_ConvertTerminationReasonToPDMS((SELECT TOP 1 enr.[P-ENROL-STAT-TY-CD] FROM SRC_Enrollments enr WHERE [P-ENROL-STAT-TY-CD] <> '00' AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) DESC)) AS [TERM_REASON_ID],
				   NULL AS [TYPE_OF_PRACTICE_ID], -- ** Must ask Jon what this should be
				   NULL AS [TAX_ID_TYPE_ID], -- not sure what to do with this since the tax id table talks about being for provider in data elements, but the table description says it's for the owner.
				   (SELECT MIN(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-BEG-DT])) FROM SRC_ProviderAltIDs alt_id WHERE alt_id.[P-SYS-ID] = map.SysID AND alt_id.[P-ALT-ID-TY-CD] = 'XX' AND alt_id.[P-ALT-ID] = prov.[P-NPI-NUM] AND dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT]) > @pin_conv_data_export_date)  AS [NPI_START_DATE],
				   (SELECT MAX(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT])) FROM SRC_ProviderAltIDs alt_id WHERE alt_id.[P-SYS-ID] = map.SysID AND alt_id.[P-ALT-ID-TY-CD] = 'XX' AND alt_id.[P-ALT-ID] = prov.[P-NPI-NUM] AND dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT]) > @pin_conv_data_export_date) AS [NPI_END_DATE]
			FROM DCConv_KeyCrossReferences map
			INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
			INNER JOIN SRC_ProviderAddresses primary_adr ON primary_adr.[P-SYS-ID] = map.SysID AND primary_adr.[P-ADR-TY-CD] = 'L'
			WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_TAXONOMY';
		INSERT INTO [dbo].[REG_TAXONOMY]
				   ([REG_ID]
				   ,[PRIMARY_FLAG]
				   ,[TAXONOMY_TYPE_ID]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[START_DATE]
				   ,[END_DATE]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
				1 AS [PRIMARY_FLAG],
				dbo.fn_ConvertProviderTaxonomyCodeToPDMS(tax.[P-TAXONOMY-CD]) AS [TAXONOMY_TYPE_ID],
				1 AS [MODIFIED_STATUS_TYPE_ID],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-BEG-DT]) AS [START_DATE],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-END-DT]) AS [END_DATE],
				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderTaxonomy tax ON tax.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_SPECIALTY';
		INSERT INTO [dbo].[REG_SPECIALTY]
           ([REG_ID]
           ,[PRIMARY_FLAG]
           ,[SPECIALTY_TYPE_ID]
           ,[SPECIALTY_BOARD_CERTIFIED]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[START_DATE]
           ,[END_DATE]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[REG_TAXONOMY_ID])
		SELECT map.RegistrationId AS [REG_ID],
			1 AS [PRIMARY_FLAG],
			dbo.fn_ConvertDCSpecialtyTypeToPDMS(specl.[P-SPECL-CD]),
			'N' AS [SPECIALTY_BOARD_CERTIFIED],
			1 AS [MODIFIED_STATUS_TYPE_ID],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-BEG-DT]) AS [START_DATE],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-END-DT]) AS  [END_DATE],
			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
			NULL AS [REG_TAXONOMY_ID] -- Must talk to Diwakar about this!!
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysId]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_LICENSE';
		INSERT INTO [dbo].[REG_LICENSE]
           ([REG_ID]
           ,[LICENSE_TYPE_ID]
           ,[LICENSE_NUMBER]
           ,[LICENSE_STATE]
           ,[LICENSE_EFF_DATE]
           ,[LICENSE_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
           dbo.fn_ConvertLicenseTypeToPDMS(lic.[P-LIC-CERT-CD]) AS [LICENSE_TYPE_ID],
           lic.[P-LIC-CERT-NUM] AS [LICENSE_NUMBER],
           lic.[P-ST-CD] AS [LICENSE_STATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) AS [LICENSE_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EXPIR-DT]) AS [LICENSE_END_DATE],
           1 AS [MODIFIED_STATUS_TYPE_ID],
 		   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderLicense lic ON lic.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_CLIA';
		INSERT INTO [dbo].[REG_CLIA]
           ([REG_ID]
           ,[CLIA_NUMBER]
           ,[CLIA_STATE]
           ,[CLIA_EFF_DATE]
           ,[CLIA_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationID AS [REG_ID],
				clia.[P-CLIA-NUM] AS [CLIA_NUMBER],
				'' AS [CLIA_STATE], -- this is not available in the DC data
                dbo.fn_ConvertDCDateToPDMS(cliad.[P-CLIA-CERT-EFF-DT]) AS [CLIA_EFF_DATE],
                dbo.fn_ConvertDCDateToPDMS(cliad.[P-CERT-EXPIR-DT]) AS [CLIA_END_DATE],
				1 AS [MODIFIED_STATUS_TYPE_ID],
 				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderCLIA clia ON clia.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderCLIADetails cliad ON clia.[P-CLIA-NUM] = cliad.[P-CLIA-NUM]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_MEDICARE';
		INSERT INTO [dbo].[REG_MEDICARE]
           ([REG_ID]
           ,[MEDICARE_NUMBER]
           ,[MEDICARE_EFF_DATE]
           ,[MEDICARE_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[NPI]
           ,[ENROLLMENT_STATUS_TYPE_ID])
		SELECT map.RegistrationId AS [REG_ID],
           mcare.[P-MCARE-NUM] AS [MEDICARE_NUMBER],
           dbo.fn_ConvertDCDateToPDMS(mcare.[P-MCARE-BEG-DT]) AS [MEDICARE_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(mcare.[P-MCARE-END-DT]) AS [MEDICARE_END_DATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           NULL AS [NPI],
           1 AS [ENROLLMENT_STATUS_TYPE_ID]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicare mcare ON mcare.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 1;

		-- copy service locations
		SET @currentTable = 'REG_SERVICE_LOCATION';
		INSERT INTO [dbo].[REG_SERVICE_LOCATION]
			   ([REG_ID]
			   ,[PRACTICE_NAME]
			   ,[MEDICAID_ID]
			   ,[SERVICING_ADDRESS1]
			   ,[SERVICING_ADDRESS2]
			   ,[SERVICING_CITY]
			   ,[SERVICING_COUNTY]
			   ,[SERVICING_STATE]
			   ,[SERVICING_ZIP]
			   ,[SERVICING_EXT_ZIP]
			   ,[SERVICING_COUNTRY]
			   ,[SERVICING_ADDRESS_NAME]
			   ,[SERVICING_PHONE_NUMBER]
			   ,[SERVICING_PHONE_EXT]
			   ,[SERVICING_FAX_NUMBER]
			   ,[SERVICING_EMAIL_ADDRESS]
			   ,[MAILTO_ADDRESS1]
			   ,[MAILTO_ADDRESS2]
			   ,[MAILTO_CITY]
			   ,[MAILTO_COUNTY]
			   ,[MAILTO_STATE]
			   ,[MAILTO_ZIP]
			   ,[MAILTO_EXT_ZIP]
			   ,[MAILTO_COUNTRY]
			   ,[MAILTO_ADDRESS_NAME]
			   ,[MAILTO_PHONE_NUMBER]
			   ,[MAILTO_PHONE_EXT]
			   ,[MAILTO_FAX_NUMBER]
			   ,[MAILTO_EMAIL_ADDRESS]
			   ,[PAYTO_ADDRESS1]
			   ,[PAYTO_ADDRESS2]
			   ,[PAYTO_CITY]
			   ,[PAYTO_COUNTY]
			   ,[PAYTO_STATE]
			   ,[PAYTO_ZIP]
			   ,[PAYTO_EXT_ZIP]
			   ,[PAYTO_COUNTRY]
			   ,[PAYTO_ADDRESS_NAME]
			   ,[PAYTO_PHONE_NUMBER]
			   ,[PAYTO_PHONE_EXT]
			   ,[PAYTO_FAX_NUMBER]
			   ,[PAYTO_EMAIL_ADDRESS]
			   ,[BILLING_CONTACT_FIRST_NAME]
			   ,[BILLING_CONTACT_LAST_NAME]
			   ,[CHECK_PAYABLE_TO_NAME]
			   ,[MODIFIED_STATUS_TYPE_ID]
			   ,[LAST_MODIFIED_DATE_TIME]
			   ,[LAST_MODIFIED_USER]
			   ,[CLAIM_NOOF_ALLOWANCES]
			   ,[FISCAL_YEAR_END])
		 SELECT map.RegistrationId AS [REG_ID],
			    '' AS PRACTICE_NAME,
			   prov.[P-ID] AS [MEDICAID_ID],
			   serv_adr.[P-LINE1-AD] AS [SERVICING_ADDRESS1],
			   serv_adr.[P-LINE2-AD] AS [SERVICING_ADDRESS2],
			   serv_adr.[P-CITY-NAM] AS [SERVICING_CITY], 
			   serv_adr.[P-CNTY-CD] AS [SERVICING_COUNTY],
			   serv_adr.[P-ST-CD] AS [SERVICING_STATE],
			   serv_adr.[P-ZIP5-CD] AS [SERVICING_ZIP],
			   serv_adr.[P-ZIP4-CD] AS [SERVICING_EXT_ZIP],
			   'US' AS [SERVICING_COUNTRY],
			   NULL AS [SERVICING_ADDRESS_NAME],
			   serv_adr.[P-PHON-NUM] AS [SERVICING_PHONE_NUMBER],
			   '' AS [SERVICING_PHONE_EXT],
			   serv_adr.[P-FAX-NUM] AS [SERVICING_FAX_NUMBER], 
			   serv_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [SERVICING_EMAIL_ADDRESS],
			   mail_adr.[P-LINE1-AD] AS [MAILTO_ADDRESS1],
			   mail_adr.[P-LINE2-AD] AS [MAILTO_ADDRESS2],
			   mail_adr.[P-CITY-NAM] AS [MAILTO_CITY], 
			   mail_adr.[P-CNTY-CD] AS [MAILTO_COUNTY],
			   mail_adr.[P-ST-CD] AS [MAILTO_STATE],
			   mail_adr.[P-ZIP5-CD] AS [MAILTO_ZIP],
			   mail_adr.[P-ZIP4-CD] AS [MAILTO_EXT_ZIP],
			   'US' AS [MAILTO_COUNTRY],
			   NULL AS [MAILTO_ADDRESS_NAME],
			   mail_adr.[P-PHON-NUM] AS [MAILTO_PHONE_NUMBER],
			   '' AS [MAILTO_PHONE_EXT],
			   mail_adr.[P-FAX-NUM] AS [MAILTO_FAX_NUMBER], 
			   mail_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [MAILTO_EMAIL_ADDRESS],
			   pay_adr.[P-LINE1-AD] AS [PAYTO_ADDRESS1],
			   pay_adr.[P-LINE2-AD] AS [PAYTO_ADDRESS2],
			   pay_adr.[P-CITY-NAM] AS [PAYTO_CITY], 
			   pay_adr.[P-CNTY-CD] AS [PAYTO_COUNTY],
			   pay_adr.[P-ST-CD] AS [PAYTO_STATE],
			   pay_adr.[P-ZIP5-CD] AS [PAYTO_ZIP],
			   pay_adr.[P-ZIP4-CD] AS [PAYTO_EXT_ZIP],
			   'US' AS [PAYTOTO_COUNTRY],
			   NULL AS [PAYTOTO_ADDRESS_NAME],
			   pay_adr.[P-PHON-NUM] AS [PAYTOTO_PHONE_NUMBER],
			   '' AS [PAYTOTO_PHONE_EXT],
			   pay_adr.[P-FAX-NUM] AS [PAYTOTO_FAX_NUMBER], 
			   pay_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [PAYTOTO_EMAIL_ADDRESS],
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 1) AS [BILLING_CONTACT_FIRST_NAME],
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 3) AS [BILLING_CONTACT_LAST_NAME],
			   NULL AS [CHECK_PAYABLE_TO_NAME],
				1 AS [MODIFIED_STATUS_TYPE_ID],
 				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
				NULL AS [CLAIM_NOOF_ALLOWANCES],
			   dbo.fn_GetProviderFiscalYearEnd(map.[SysID], prov.[P-MCARE-FY-MO-NUM],prov.[P-MCAID-FY-MO-NUM],prov.[P-FACI-FY-MO-NUM]) AS [FISCAL_YEAR_END]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		LEFT OUTER JOIN SRC_ProviderAddresses serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] AND serv_adr.[P-ADR-TY-CD] = 'L'
		LEFT OUTER JOIN SRC_ProviderAddresses mail_adr ON mail_adr.[P-SYS-ID] = map.[SysID] AND mail_adr.[P-ADR-TY-CD] = 'M'
		LEFT OUTER JOIN SRC_ProviderAddresses pay_adr ON pay_adr.[P-SYS-ID] = map.[SysID] AND pay_adr.[P-ADR-TY-CD] = 'B'
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_ADDITIONAL_ADDRESSES';
		INSERT INTO [dbo].[REG_ADDITIONAL_ADDRESSES]
           ([REG_ID]
           ,[ADDRESS_DESC]
           ,[ADDRESS1]
           ,[ADDRESS2]
           ,[CITY]
           ,[STATE]
           ,[ZIP]
           ,[EXT_ZIP]
           ,[COUNTY]
           ,[PHONE]
           ,[PHONE_EXT]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
           adr_type.[Short] AS [ADDRESS_DESC],
           adr.[P-LINE1-AD] AS [ADDRESS1],
           adr.[P-LINE2-AD] AS [ADDRESS2],
           adr.[P-CITY-NAM] AS [CITY],
           adr.[P-ST-CD] AS [STATE],
           adr.[P-ZIP5-CD] AS [ZIP],
           adr.[P-ZIP4-CD] AS [EXT_ZIP],
           adr.[P-CNTY-CD] AS [COUNTY],
           adr.[P-PHON-NUM] AS [PHONE],
           '' AS [PHONE_EXT],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderAddresses adr ON adr.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_LkUpAddressType adr_type ON adr.[P-ADR-TY-CD] = adr_type.[P-ADR-TY-CD]
		WHERE map.IsGroup = 1 AND adr.[P-ADR-TY-CD] NOT IN ('B', 'L', 'M');

		SET @currentTable = 'REG_DEA';
		INSERT INTO [dbo].[REG_DEA]
				   ([REG_ID]
				   ,[DEA_ID]
				   ,[DEA_NUMBER]
				   ,[DEA_EFF_DATE]
				   ,[DEA_END_DATE]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER])
		SELECT map.[RegistrationId] AS [REG_ID],
			NULL AS [DEA_ID],
            prov.[P-DEA-NUM] AS [DEA_NUMBER],
            dbo.fn_ConvertDCDateToPDMS(prov.[P-DEA-EFF-DT]) AS [DEA_EFF_DATE],
			dbo.fn_ConvertDCDateToPDMS(prov.[P-DEA-EXP-DT]) AS [DEA_END_DATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysId]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_MEDICAID';
		INSERT INTO [dbo].[REG_MEDICAID]
           ([REG_ID]
           ,[MEDICAID_NUMBER]
           ,[MEDICAID_EFF_DATE]
           ,[MEDICAID_END_DATE]
           ,[MEDICAID_STATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[NPI]
           ,[ENROLLMENT_STATUS_TYPE_ID])
		SELECT 
           map.RegistrationId AS [REG_ID],
			mcaid.[P-PREV-MCAID-ID] AS [MEDICAID_NUMBER],
           dbo.fn_ConvertDCDateToPDMS(mcaid.[P-PREV-BEG-DT]) AS [MEDICAID_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(mcaid.[P-PREV-END-DT]) AS [MEDICAID_END_DATE],
           NULL AS [MEDICAID_STATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           NULL AS [NPI], -- check to see if this is in the UI
           dbo.fn_ConvertEnrollmentStatusToPDMS(en.[P-ENROL-STAT-TY-CD]) AS [ENROLLMENT_STATUS_TYPE_ID]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicaid mcaid ON mcaid.[P-SYS-ID] = map.[SysId]
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 1;

		SET @currentTable = 'REG_OWNER';
		INSERT INTO [dbo].[REG_OWNER]
           ([REG_ID]
           ,[NAME]
           ,[DOB]
           ,[TAX_ID]
           ,[PERCENTAGE_OF_OWNERSHIP]
           ,[TITLE]
           ,[ADDRESS1]
           ,[ADDRESS2]
           ,[CITY]
           ,[STATE]
           ,[ZIP]
           ,[EXT_ZIP]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[REG_OWNER_TYPE_ID]
           ,[OWNER_ID])
		SELECT map.RegistrationId AS [REG_ID],
			dbo.fn_BuildName(own.[P-OWNER-FST-NAM],own.[P-OWNER-MI-NAM], own.[P-OWNER-LAST-NAM], own.[P-OWNER-SFX-NAM], 1) AS [NAME],
            NULL AS [DOB],
            own.[P-OWNER-TAX-ID] AS [TAX_ID],
            CAST(CAST(own.[P-OWNER-PCT] AS decimal(5,2)) AS int) AS [PERCENTAGE_OF_OWNERSHIP],
            own.[P-OWNER-TITL-NAM] AS [TITLE],
            own.[P-OWNER-LINE1-AD] AS [ADDRESS1],
            own.[P-OWNER-LINE2-AD] AS [ADDRESS2],
            own.[P-OWNER-CITY-NAM] AS [CITY],
            own.[P-OWNER-ST-CD] AS [STATE],
            own.[P-OWNER-ZIP5-CD] AS [ZIP],
            own.[P-OWNER-ZIP4-CD] AS [EXT_ZIP],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
            dbo.fn_ConvertProviderOwnerTypeToPDMS(prov.[P-OWNER-TY-CD]) AS REG_OWNER_TYPE_ID,
            NULL AS OWNER_ID
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderOwner own ON own.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 1;

		--COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH

	--	ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorLine int;
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorProcedure varchar(128);

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorProcedure = ERROR_PROCEDURE();

		-- add the error to the log
		INSERT INTO [dbo].[DCConv_Errors]([RunID],[TableContext],[ColumnContext],[ErrorLine],[ErrorNumber],[ErrorMessage],[ErrorProcedure])
		VALUES
        (@pin_conv_run_id,@currentTable,'', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);

		IF OBJECT_ID('tempdb..DCConv_KeyCrossReferences') IS NOT NULL
			DROP TABLE DCConv_KeyCrossReferences;
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_ConvertIndividuals]    Script Date: 6/3/2016 11:50:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/10/2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertIndividuals] 
(
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
	
)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @currentTable varchar(50);
	DECLARE @MaxRegistrationID int;

	BEGIN TRY
--		BEGIN TRANSACTION
		SELECT @MaxRegistrationID = ISNULL(MAX(RegistrationID), 0) FROM DCConv_KeyCrossReferences;
		-- add the mapping for the group ids
		-- choose all providers with affiliates as group members
		INSERT INTO DCConv_KeyCrossReferences
		SELECT DISTINCT en.[P-SYS-ID], (ROW_NUMBER() OVER (ORDER BY en.[P-SYS-ID])) + @MaxRegistrationID, 0
		FROM SRC_Enrollments en
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		WHERE prov.[P-REC-TY-CD] = 'P' AND 
		(SELECT COUNT(*) FROM SRC_ProviderAffiliates aff 
		WHERE aff.[P-GROUP-SYS-ID] = en.[P-SYS-ID] AND 
		aff.[P-MEMBER-SYS-ID] IS NOT NULL) = 0 ORDER BY en.[P-SYS-ID];

		SELECT * FROM DCConv_KeyCrossReferences g1 WHERE (SELECT COUNT(*) FROM DCConv_KeyCrossReferences g2 WHERE g1.SysID=g2.SysID) > 0 ORDER BY RegistrationId;

		-- convert the registration data
		SET @currentTable = 'REGISTRATION';

		SET IDENTITY_INSERT dbo.[REGISTRATION] ON;

		INSERT INTO [dbo].[REGISTRATION] ([REG_ID], 
			[REQUESTED_EFFECTIVE_DATE],
			[CHANGE_EFFECTIVE_DATE],
			[REGISTRATION_STATUS_TYPE_ID],
			[LAST_MODIFIED_DATE_TIME],
			[LAST_MODIFIED_USER],
			[REG_PROGRAM_STATUS_TYPE_ID],
			[DIDD_REFERRAL_ID],
			[SUBMIT_DATE_TIME],
			[DIDD_COMMISSIONER_DATE_TIME],
			[PECOS_VERIFIED],
			[IS_PAPER_APPLICATION],
			[REG_CREATE_DATE_TIME])
		SELECT DISTINCT map.RegistrationId AS REG_ID, 
		(SELECT MIN(dbo.fn_ConvertDCDateToPDMS([P-STAT-EFF-DT])) FROM SRC_Enrollments WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-SYS-ID] = map.SysId) AS [REQUESTED_EFFECTIVE_DATE],
		(SELECT MIN(dbo.fn_ConvertDCDateToPDMS([P-STAT-EFF-DT])) FROM SRC_Enrollments WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-SYS-ID] = map.SysId) AS [CHANGE_EFFECTIVE_DATE],
		1 AS [REGISTRATION_STATUS_TYPE_ID],
		@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
		dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
		6 AS [REG_PROGRAM_STATUS_TYPE_ID],
		NULL AS [DIDD_REFERRAL_ID],  -- to be filled in once the DIDD data are gathered
		dbo.fn_ConvertDCDateToPDMS(prov.[P-APPL-DT]) AS [SUBMIT_DATE_TIME],
		NULL AS [DIDD_COMMISSIONER_DATE_TIME], -- to be filled in once the DIDD data are gathered
		0 AS [PECOS_VERIFIED],
		NULL AS [IS_PAPER_APPLICATION],
		@pin_conv_run_time AS [REG_CREATE_DATE_TIME]
		FROM DCConv_KeyCrossReferences map 
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		WHERE map.IsGroup = 0;

		SET IDENTITY_INSERT dbo.[REGISTRATION] OFF;

		PRINT 'REG_PROVIDER';
		---- convert the providers
		SET @currentTable = 'REG_PROVIDER';
		INSERT INTO [dbo].[REG_PROVIDER]
				   ([REG_ID]
				   ,[NAME]
				   ,[PARTY_ID]
				   ,[DBA]
				   ,[NPI]
				   ,[TAX_ID]
				   ,[ENTITY_TYPE_ID]
				   ,[PROVIDER_TYPE_ID]
				   ,[CONTACT_NAME]
				   ,[CONTACT_ADDRESS1]
				   ,[CONTACT_ADDRESS2]
				   ,[CONTACT_CITY]
				   ,[CONTACT_STATE]
				   ,[CONTACT_ZIP]
				   ,[CONTACT_EXT_ZIP]
				   ,[CONTACT_PHONE_NUMBER]
				   ,[CONTACT_FAX_NUMBER]
				   ,[CONTACT_EMAIL_ADDRESS]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER]
				   ,[ENROLLMENT_STATUS_CODE]
				   ,[TERM_DATE]
				   ,[FIRST_NAME]
				  ,[LAST_NAME]
				   ,[MIDDLE_INITIAL]
				   ,[BIRTH_DATE]
				   ,[DEATH_DATE]
				   ,[END_DATE]
				   ,[TERM_REASON_ID]
				   ,[TYPE_OF_PRACTICE_ID]
				   ,[TAX_ID_TYPE_ID]
				   ,[NPI_START_DATE]
				   ,[NPI_END_DATE])
			SELECT DISTINCT map.RegistrationId AS [REG_ID],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-NAM]))) = 0 THEN RTRIM(LTRIM(prov.[P-DBA-NAM])) ELSE RTRIM(LTRIM(prov.[P-NAM])) END AS [NAME],
				   NULL AS [PARTY_ID],
				   prov.[P-DBA-NAM] AS [DBA],
				   prov.[P-NPI-NUM] AS [NPI],
				   primary_adr.[P-TAX-KEY-ID] AS [TAX_ID],
				   dbo.fn_ConvertProviderEntityTypeToPDMS(prov.[P-SYS-ID],@pin_conv_data_export_date) 
				   AS [ENTITY_TYPE_ID],  -- this is also know as the provider category
				   dbo.fn_ConvertProviderTypeToPDMS((SELECT TOP 1 enr.[P-TY-CD] FROM SRC_Enrollments enr WHERE enr.[P-SYS-ID]=map.[SysId] AND enr.[P-ENROL-STAT-TY-CD] = '00' AND enr.[P-STAT-END-DT] > @pin_conv_data_export_date ORDER BY enr.[P-STAT-EFF-DT] DESC)) AS [PROVIDER_TYPE_ID], -- use the most recent active enrollment
				   primary_adr.[P-CONTCT-NAM] AS [CONTACT_NAME],
				   primary_adr.[P-LINE1-AD] AS [CONTACT_ADDRESS1],
				   primary_adr.[P-LINE2-AD] AS [CONTACT_ADDRESS2],
				   primary_adr.[P-CITY-NAM] AS [CONTACT_CITY],
				   primary_adr.[P-ST-CD] AS [CONTACT_STATE],
				   primary_adr.[P-ZIP5-CD] AS [CONTACT_ZIP],
				   primary_adr.[P-ZIP4-CD] AS [CONTACT_EXT_ZIP],
				   primary_adr.[P-CONTCT-PHON-NUM] AS [CONTACT_PHONE_NUMBER], 
			       primary_adr.[P-CONTCT-FAX-NUM]  AS [CONTACT_FAX_NUMBER],
				   CASE WHEN LEN(RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT]))) > 80 THEN RTRIM(LTRIM(SUBSTRING(primary_adr.[P-CONTCT-EMAIL-AD-TEXT], 1, 80))) ELSE  RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT])) END  AS [CONTACT_EMAIL_ADDRESS],
				   1  AS [MODIFIED_STATUS_TYPE_ID],
				   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				   dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
				   dbo.fn_ConvertEnrollmentStatusToPDMS((SELECT TOP 1 enr.[P-ENROL-STAT-TY-CD] FROM SRC_Enrollments enr WHERE enr.[P-STAT-END-DT] > @pin_conv_data_export_date  AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-EFF-DT]) DESC)) AS ENROLLMENT_STATUS_CODE, -- use the most recent enrollment status that is current
				   NULL AS TERM_DATE, -- will have to change later
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-FST-NAM]))) = 0 THEN prov.[P-DBA-FST-NAM] ELSE prov.[P-FST-NAM] END AS [FIRST_NAME],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-LAST-NAM]))) = 0 THEN prov.[P-DBA-LAST-NAM] ELSE prov.[P-LAST-NAM] END AS [LAST_NAME],
				   CASE WHEN LEN(RTRIM(LTRIM(prov.[P-MI-NAM]))) = 0 THEN prov.[P-DBA-MI-NAM] ELSE prov.[P-MI-NAM] END AS [MIDDLE_INITIAL],
				   dbo.fn_ConvertDCDateToPDMS(prov.[P-DOB-DT]) AS [BIRTH_DATE],
				   (SELECT TOP 1 dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) FROM SRC_Enrollments enr WHERE [P-ENROL-STAT-TY-CD] = '41' AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) DESC) AS [DEATH_DATE],  -- use the most recent date of death in case there's more than one
				   NULL AS [END_DATE],  -- *** Must ask Jon what this should be!!
				   dbo.fn_ConvertTerminationReasonToPDMS((SELECT TOP 1 enr.[P-ENROL-STAT-TY-CD] FROM SRC_Enrollments enr WHERE [P-ENROL-STAT-TY-CD] <> '00' AND enr.[P-SYS-ID] = map.SysID ORDER BY dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) DESC)) AS [TERM_REASON_ID],
				   NULL AS [TYPE_OF_PRACTICE_ID], -- ** Must ask Jon what this should be
				   NULL AS [TAX_ID_TYPE_ID], -- not sure what to do with this since the tax id table talks about being for provider in data elements, but the table description says it's for the owner.
				   (SELECT MIN(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-BEG-DT])) FROM SRC_ProviderAltIDs alt_id WHERE alt_id.[P-SYS-ID] = map.SysID AND alt_id.[P-ALT-ID-TY-CD] = 'XX' AND alt_id.[P-ALT-ID] = prov.[P-NPI-NUM] AND dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT]) > @pin_conv_data_export_date)  AS [NPI_START_DATE],
				   (SELECT MAX(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT])) FROM SRC_ProviderAltIDs alt_id WHERE alt_id.[P-SYS-ID] = map.SysID AND alt_id.[P-ALT-ID-TY-CD] = 'XX' AND alt_id.[P-ALT-ID] = prov.[P-NPI-NUM] AND dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT]) > @pin_conv_data_export_date) AS [NPI_END_DATE]
			FROM DCConv_KeyCrossReferences map
			INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
			INNER JOIN SRC_ProviderAddresses primary_adr ON primary_adr.[P-SYS-ID] = map.SysID AND primary_adr.[P-ADR-TY-CD] = 'L' AND primary_adr.[G-AUD-DT] = (SELECT MAX([G-AUD-DT]) FROM SRC_ProviderAddresses WHERE [P-SYS-ID] = map.SysID AND [P-ADR-TY-CD] = 'L')
			WHERE map.IsGroup = 0;

		PRINT 'REG_TAXONOMY';
		SET @currentTable = 'REG_TAXONOMY';
		INSERT INTO [dbo].[REG_TAXONOMY]
				   ([REG_ID]
				   ,[PRIMARY_FLAG]
				   ,[TAXONOMY_TYPE_ID]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[START_DATE]
				   ,[END_DATE]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
				1 AS [PRIMARY_FLAG],
				dbo.fn_ConvertProviderTaxonomyCodeToPDMS(tax.[P-TAXONOMY-CD]) AS [TAXONOMY_TYPE_ID],
				1 AS [MODIFIED_STATUS_TYPE_ID],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-BEG-DT]) AS [START_DATE],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-END-DT]) AS [END_DATE],
				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderTaxonomy tax ON tax.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0;

		PRINT 'REG_SPECIALTY';
		SET @currentTable = 'REG_SPECIALTY';
		INSERT INTO [dbo].[REG_SPECIALTY]
           ([REG_ID]
           ,[PRIMARY_FLAG]
           ,[SPECIALTY_TYPE_ID]
           ,[SPECIALTY_BOARD_CERTIFIED]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[START_DATE]
           ,[END_DATE]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[REG_TAXONOMY_ID])
		SELECT map.RegistrationId AS [REG_ID],
			1 AS [PRIMARY_FLAG],
			dbo.fn_ConvertDCSpecialtyTypeToPDMS(specl.[P-SPECL-CD]),
			'N' AS [SPECIALTY_BOARD_CERTIFIED],
			1 AS [MODIFIED_STATUS_TYPE_ID],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-BEG-DT]) AS [START_DATE],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-END-DT]) AS  [END_DATE],
			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
			NULL AS [REG_TAXONOMY_ID] -- Must talk to Diwakar about this!!
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysId]
		WHERE map.IsGroup = 0;

		PRINT 'REG_LICENSE';
		SET @currentTable = 'REG_LICENSE';
		INSERT INTO [dbo].[REG_LICENSE]
           ([REG_ID]
           ,[LICENSE_TYPE_ID]
           ,[LICENSE_NUMBER]
           ,[LICENSE_STATE]
           ,[LICENSE_EFF_DATE]
           ,[LICENSE_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
           dbo.fn_ConvertLicenseTypeToPDMS(lic.[P-LIC-CERT-CD]) AS [LICENSE_TYPE_ID],
           lic.[P-LIC-CERT-NUM] AS [LICENSE_NUMBER],
           lic.[P-ST-CD] AS [LICENSE_STATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) AS [LICENSE_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EXPIR-DT]) AS [LICENSE_END_DATE],
           1 AS [MODIFIED_STATUS_TYPE_ID],
 		   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderLicense lic ON lic.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0;

		SET @currentTable = 'REG_CLIA';
		INSERT INTO [dbo].[REG_CLIA]
           ([REG_ID]
           ,[CLIA_NUMBER]
           ,[CLIA_STATE]
           ,[CLIA_EFF_DATE]
           ,[CLIA_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationID AS [REG_ID],
				clia.[P-CLIA-NUM] AS [CLIA_NUMBER],
				'' AS [CLIA_STATE], -- this is not available in the DC data
                dbo.fn_ConvertDCDateToPDMS(cliad.[P-CLIA-CERT-EFF-DT]) AS [CLIA_EFF_DATE],
                dbo.fn_ConvertDCDateToPDMS(cliad.[P-CERT-EXPIR-DT]) AS [CLIA_END_DATE],
				1 AS [MODIFIED_STATUS_TYPE_ID],
 				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderCLIA clia ON clia.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderCLIADetails cliad ON clia.[P-CLIA-NUM] = cliad.[P-CLIA-NUM]
		WHERE map.IsGroup = 0;

		SET @currentTable = 'REG_MEDICARE';
		INSERT INTO [dbo].[REG_MEDICARE]
           ([REG_ID]
           ,[MEDICARE_NUMBER]
           ,[MEDICARE_EFF_DATE]
           ,[MEDICARE_END_DATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[NPI]
           ,[ENROLLMENT_STATUS_TYPE_ID])
		SELECT map.RegistrationId AS [REG_ID],
           mcare.[P-MCARE-NUM] AS [MEDICARE_NUMBER],
           dbo.fn_ConvertDCDateToPDMS(mcare.[P-MCARE-BEG-DT]) AS [MEDICARE_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(mcare.[P-MCARE-END-DT]) AS [MEDICARE_END_DATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           NULL AS [NPI],
           1 AS [ENROLLMENT_STATUS_TYPE_ID]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicare mcare ON mcare.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0;

		-- copy service locations
		SET @currentTable = 'REG_SERVICE_LOCATION';
		INSERT INTO [dbo].[REG_SERVICE_LOCATION]
			   ([REG_ID]
			   ,[PRACTICE_NAME]
			   ,[MEDICAID_ID]
			   ,[SERVICING_ADDRESS1]
			   ,[SERVICING_ADDRESS2]
			   ,[SERVICING_CITY]
			   ,[SERVICING_COUNTY]
			   ,[SERVICING_STATE]
			   ,[SERVICING_ZIP]
			   ,[SERVICING_EXT_ZIP]
			   ,[SERVICING_COUNTRY]
			   ,[SERVICING_ADDRESS_NAME]
			   ,[SERVICING_PHONE_NUMBER]
			   ,[SERVICING_PHONE_EXT]
			   ,[SERVICING_FAX_NUMBER]
			   ,[SERVICING_EMAIL_ADDRESS]
			   ,[MAILTO_ADDRESS1]
			   ,[MAILTO_ADDRESS2]
			   ,[MAILTO_CITY]
			   ,[MAILTO_COUNTY]
			   ,[MAILTO_STATE]
			   ,[MAILTO_ZIP]
			   ,[MAILTO_EXT_ZIP]
			   ,[MAILTO_COUNTRY]
			   ,[MAILTO_ADDRESS_NAME]
			   ,[MAILTO_PHONE_NUMBER]
			   ,[MAILTO_PHONE_EXT]
			   ,[MAILTO_FAX_NUMBER]
			   ,[MAILTO_EMAIL_ADDRESS]
			   ,[PAYTO_ADDRESS1]
			   ,[PAYTO_ADDRESS2]
			   ,[PAYTO_CITY]
			   ,[PAYTO_COUNTY]
			   ,[PAYTO_STATE]
			   ,[PAYTO_ZIP]
			   ,[PAYTO_EXT_ZIP]
			   ,[PAYTO_COUNTRY]
			   ,[PAYTO_ADDRESS_NAME]
			   ,[PAYTO_PHONE_NUMBER]
			   ,[PAYTO_PHONE_EXT]
			   ,[PAYTO_FAX_NUMBER]
			   ,[PAYTO_EMAIL_ADDRESS]
			   ,[BILLING_CONTACT_FIRST_NAME]
			   ,[BILLING_CONTACT_LAST_NAME]
			   ,[CHECK_PAYABLE_TO_NAME]
			   ,[MODIFIED_STATUS_TYPE_ID]
			   ,[LAST_MODIFIED_DATE_TIME]
			   ,[LAST_MODIFIED_USER]
			   ,[CLAIM_NOOF_ALLOWANCES]
			   ,[FISCAL_YEAR_END])
		 SELECT map.RegistrationId AS [REG_ID],
			    '' AS PRACTICE_NAME,
			   prov.[P-ID] AS [MEDICAID_ID],
			   serv_adr.[P-LINE1-AD] AS [SERVICING_ADDRESS1],
			   serv_adr.[P-LINE2-AD] AS [SERVICING_ADDRESS2],
			   serv_adr.[P-CITY-NAM] AS [SERVICING_CITY], 
			   serv_adr.[P-CNTY-CD] AS [SERVICING_COUNTY],
			   serv_adr.[P-ST-CD] AS [SERVICING_STATE],
			   serv_adr.[P-ZIP5-CD] AS [SERVICING_ZIP],
			   serv_adr.[P-ZIP4-CD] AS [SERVICING_EXT_ZIP],
			   'US' AS [SERVICING_COUNTRY],
			   NULL AS [SERVICING_ADDRESS_NAME],
			   serv_adr.[P-PHON-NUM] AS [SERVICING_PHONE_NUMBER],
			   '' AS [SERVICING_PHONE_EXT],
			   serv_adr.[P-FAX-NUM] AS [SERVICING_FAX_NUMBER], 
			   serv_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [SERVICING_EMAIL_ADDRESS],
			   mail_adr.[P-LINE1-AD] AS [MAILTO_ADDRESS1],
			   mail_adr.[P-LINE2-AD] AS [MAILTO_ADDRESS2],
			   mail_adr.[P-CITY-NAM] AS [MAILTO_CITY], 
			   mail_adr.[P-CNTY-CD] AS [MAILTO_COUNTY],
			   mail_adr.[P-ST-CD] AS [MAILTO_STATE],
			   mail_adr.[P-ZIP5-CD] AS [MAILTO_ZIP],
			   mail_adr.[P-ZIP4-CD] AS [MAILTO_EXT_ZIP],
			   'US' AS [MAILTO_COUNTRY],
			   NULL AS [MAILTO_ADDRESS_NAME],
			   mail_adr.[P-PHON-NUM] AS [MAILTO_PHONE_NUMBER],
			   '' AS [MAILTO_PHONE_EXT],
			   mail_adr.[P-FAX-NUM] AS [MAILTO_FAX_NUMBER], 
			   mail_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [MAILTO_EMAIL_ADDRESS],
			   pay_adr.[P-LINE1-AD] AS [PAYTO_ADDRESS1],
			   pay_adr.[P-LINE2-AD] AS [PAYTO_ADDRESS2],
			   pay_adr.[P-CITY-NAM] AS [PAYTO_CITY], 
			   pay_adr.[P-CNTY-CD] AS [PAYTO_COUNTY],
			   pay_adr.[P-ST-CD] AS [PAYTO_STATE],
			   pay_adr.[P-ZIP5-CD] AS [PAYTO_ZIP],
			   pay_adr.[P-ZIP4-CD] AS [PAYTO_EXT_ZIP],
			   'US' AS [PAYTOTO_COUNTRY],
			   NULL AS [PAYTOTO_ADDRESS_NAME],
			   pay_adr.[P-PHON-NUM] AS [PAYTOTO_PHONE_NUMBER],
			   '' AS [PAYTOTO_PHONE_EXT],
			   pay_adr.[P-FAX-NUM] AS [PAYTOTO_FAX_NUMBER], 
			   pay_adr.[P-CONTCT-EMAIL-AD-TEXT] AS [PAYTOTO_EMAIL_ADDRESS],
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 1) AS [BILLING_CONTACT_FIRST_NAME],
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 3) AS [BILLING_CONTACT_LAST_NAME],
			   NULL AS [CHECK_PAYABLE_TO_NAME],
				1 AS [MODIFIED_STATUS_TYPE_ID],
 				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
				NULL AS [CLAIM_NOOF_ALLOWANCES],
			   dbo.fn_GetProviderFiscalYearEnd(map.[SysID], prov.[P-MCARE-FY-MO-NUM],prov.[P-MCAID-FY-MO-NUM],prov.[P-FACI-FY-MO-NUM]) AS [FISCAL_YEAR_END]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		LEFT OUTER JOIN SRC_ProviderAddresses serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] AND serv_adr.[P-ADR-TY-CD] = 'L'
		LEFT OUTER JOIN SRC_ProviderAddresses mail_adr ON mail_adr.[P-SYS-ID] = map.[SysID] AND mail_adr.[P-ADR-TY-CD] = 'M'
		LEFT OUTER JOIN SRC_ProviderAddresses pay_adr ON pay_adr.[P-SYS-ID] = map.[SysID] AND pay_adr.[P-ADR-TY-CD] = 'B'
		WHERE map.IsGroup = 0;

		SET @currentTable = 'REG_ADDITIONAL_ADDRESSES';
		INSERT INTO [dbo].[REG_ADDITIONAL_ADDRESSES]
           ([REG_ID]
           ,[ADDRESS_DESC]
           ,[ADDRESS1]
           ,[ADDRESS2]
           ,[CITY]
           ,[STATE]
           ,[ZIP]
           ,[EXT_ZIP]
           ,[COUNTY]
           ,[PHONE]
           ,[PHONE_EXT]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT map.RegistrationId AS [REG_ID],
           adr_type.[Short] AS [ADDRESS_DESC],
           adr.[P-LINE1-AD] AS [ADDRESS1],
           adr.[P-LINE2-AD] AS [ADDRESS2],
           adr.[P-CITY-NAM] AS [CITY],
           adr.[P-ST-CD] AS [STATE],
           adr.[P-ZIP5-CD] AS [ZIP],
           adr.[P-ZIP4-CD] AS [EXT_ZIP],
           adr.[P-CNTY-CD] AS [COUNTY],
           adr.[P-PHON-NUM] AS [PHONE],
           '' AS [PHONE_EXT],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderAddresses adr ON adr.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_LkUpAddressType adr_type ON adr.[P-ADR-TY-CD] = adr_type.[P-ADR-TY-CD]
		WHERE map.IsGroup = 0 AND adr.[P-ADR-TY-CD] NOT IN ('B', 'L', 'M');

		SET @currentTable = 'REG_DEA';
		INSERT INTO [dbo].[REG_DEA]
				   ([REG_ID]
				   ,[DEA_ID]
				   ,[DEA_NUMBER]
				   ,[DEA_EFF_DATE]
				   ,[DEA_END_DATE]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER])
		SELECT map.[RegistrationId] AS [REG_ID],
			NULL AS [DEA_ID],
            prov.[P-DEA-NUM] AS [DEA_NUMBER],
            dbo.fn_ConvertDCDateToPDMS(prov.[P-DEA-EFF-DT]) AS [DEA_EFF_DATE],
			dbo.fn_ConvertDCDateToPDMS(prov.[P-DEA-EXP-DT]) AS [DEA_END_DATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysId]
		WHERE map.IsGroup = 0;

		SET @currentTable = 'REG_MEDICAID';
		INSERT INTO [dbo].[REG_MEDICAID]
           ([REG_ID]
           ,[MEDICAID_NUMBER]
           ,[MEDICAID_EFF_DATE]
           ,[MEDICAID_END_DATE]
           ,[MEDICAID_STATE]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[NPI]
           ,[ENROLLMENT_STATUS_TYPE_ID])
		SELECT 
           map.RegistrationId AS [REG_ID],
			mcaid.[P-PREV-MCAID-ID] AS [MEDICAID_NUMBER],
           dbo.fn_ConvertDCDateToPDMS(mcaid.[P-PREV-BEG-DT]) AS [MEDICAID_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(mcaid.[P-PREV-END-DT]) AS [MEDICAID_END_DATE],
           NULL AS [MEDICAID_STATE],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           NULL AS [NPI], -- check to see if this is in the UI
           dbo.fn_ConvertEnrollmentStatusToPDMS(en.[P-ENROL-STAT-TY-CD]) AS [ENROLLMENT_STATUS_TYPE_ID]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicaid mcaid ON mcaid.[P-SYS-ID] = map.[SysId]
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0;

		SET @currentTable = 'REG_OWNER';
		INSERT INTO [dbo].[REG_OWNER]
           ([REG_ID]
           ,[NAME]
           ,[DOB]
           ,[TAX_ID]
           ,[PERCENTAGE_OF_OWNERSHIP]
           ,[TITLE]
           ,[ADDRESS1]
           ,[ADDRESS2]
           ,[CITY]
           ,[STATE]
           ,[ZIP]
           ,[EXT_ZIP]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[REG_OWNER_TYPE_ID]
           ,[OWNER_ID])
		SELECT map.RegistrationId AS [REG_ID],
			dbo.fn_BuildName(own.[P-OWNER-FST-NAM],own.[P-OWNER-MI-NAM], own.[P-OWNER-LAST-NAM], own.[P-OWNER-SFX-NAM], 1) AS [NAME],
            NULL AS [DOB],
            own.[P-OWNER-TAX-ID] AS [TAX_ID],
            CAST(CAST(own.[P-OWNER-PCT] AS decimal(5,2)) AS int) AS [PERCENTAGE_OF_OWNERSHIP],
            own.[P-OWNER-TITL-NAM] AS [TITLE],
            own.[P-OWNER-LINE1-AD] AS [ADDRESS1],
            own.[P-OWNER-LINE2-AD] AS [ADDRESS2],
            own.[P-OWNER-CITY-NAM] AS [CITY],
            own.[P-OWNER-ST-CD] AS [STATE],
            own.[P-OWNER-ZIP5-CD] AS [ZIP],
            own.[P-OWNER-ZIP4-CD] AS [EXT_ZIP],
			1 AS [MODIFIED_STATUS_TYPE_ID],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
            dbo.fn_ConvertProviderOwnerTypeToPDMS(prov.[P-OWNER-TY-CD]) AS REG_OWNER_TYPE_ID,
            NULL AS OWNER_ID
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderOwner own ON own.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0;


		--COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH

	--	ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);
		DECLARE @ErrorLine int;
		DECLARE @ErrorSeverity INT;
		DECLARE @ErrorState INT;
		DECLARE @ErrorNumber int;
		DECLARE @ErrorProcedure varchar(128);

		SELECT 
			@ErrorMessage = ERROR_MESSAGE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorProcedure = ERROR_PROCEDURE();

		-- add the error to the log
		INSERT INTO [dbo].[DCConv_Errors]([RunID],[TableContext],[ColumnContext],[ErrorLine],[ErrorNumber],[ErrorMessage],[ErrorProcedure])
		VALUES
        (@pin_conv_run_id,@currentTable,'', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);

		IF OBJECT_ID('tempdb..DCConv_KeyCrossReferences') IS NOT NULL
			DROP TABLE DCConv_KeyCrossReferences;
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_ConvertReferenceValues]    Script Date: 6/3/2016 11:50:12 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Richard Mays
-- Create date: 5/18/2016
-- Description:	Converts and/or moves the DC reference values to PDMS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertReferenceValues]
	-- Add the parameters for the stored procedure here
	@pin_run_id VARCHAR(10),
	@pin_run_reference_time DATETIME,
	@pin_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"
	-- populate the provider type reference values
	DELETE
	FROM dbo.PROVIDER_TYPE;

	DBCC CHECKIDENT (
			'dbo.PROVIDER_TYPE'
			,RESEED
			,0
			);

	-- replace the PDMS reference values with those from DC
	INSERT INTO [dbo].[PROVIDER_TYPE] (
		[PROVIDER_TYPE_ABBREVIATION]
		,[PROVIDER_TYPE_NAME]
		,[LAST_MODIFIED_DATE_TIME]
		,[LAST_MODIFIED_USER]
		,[IS_USED_IN_MMIS]
		,[PROVIDER_CATEGORY_TYPE_ID]
		,[MMIS_PROVIDER_TYPE_ID]
		,[REQUIRE_NPI]
		,[PROVIDER_RISK_LEVEL_ID]
		)
	SELECT pty.[Short] AS [PROVIDER_TYPE_ABBREVIATION]
		,pty.[Long] AS [PROVIDER_TYPE_NAME]
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,'Y' AS [IS_USED_IN_MMIS]
		,dbo.fn_GetProviderCategoryFromDCProviderType(pty.[P-TY-CD]) AS [PROVIDER_CATEGORY_TYPE_ID]
		,pty.[P-TY-CD] AS [MMIS_PROVIDER_TYPE_ID]
		,0 AS [REQUIRE_NPI]
		,0 AS [PROVIDER_RISK_LEVEL_ID]
	FROM SRC_LkUpProviderType [pty];

	-- populate the specialty type reference values
	DELETE
	FROM dbo.SPECIALTY_TYPE;

	DBCC CHECKIDENT (
			'dbo.SPECIALTY_TYPE'
			,RESEED
			,0
			);

	INSERT INTO [dbo].[SPECIALTY_TYPE] (
		[SPECIALTY_TYPE_NAME]
		,[LAST_MODIFIED_DATE_TIME]
		,[LAST_MODIFIED_USER]
		,[MMIS_SPECIALTY_TYPE_ID]
		)
	SELECT spec.[Long] AS [SPECIALTY_TYPE_NAME]
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,spec.[P-SPECL-CD] AS MMIS_SPECIALTY_TYPE_ID
	FROM SRC_LkUpSpecialtyType spec;

	CREATE TABLE #specialtyCodeMap (
		PDMS_ProviderTypeID INT
		,MMIS_ProviderTypeCode VARCHAR(3)
		,PDMS_SpecialtyTypeID INT
		,MMIS_SpecialtyTypeCode VARCHAR(3)
		)

	DELETE
	FROM dbo.TAXONOMY_TYPE;

	DBCC CHECKIDENT (
			'dbo.TAXONOMY_TYPE'
			,RESEED
			,0
			);

	INSERT INTO #specialtyCodeMap
	SELECT DISTINCT NULL AS PDMS_ProviderTypeID
		,en.[P-TY-CD] AS MMIS_ProviderTypeCode
		,NULL AS PDMS_SpecialtyTypeID
		,spec.[P-SPECL-CD] AS MMIS_SpecialtyTypeCode
	FROM SRC_Enrollments en
	LEFT OUTER JOIN SRC_LkUpProviderType pt ON pt.[P-TY-CD] = en.[P-TY-CD]
	LEFT OUTER JOIN SRC_ProviderSpecialty spec ON en.[P-SYS-ID] = spec.[P-SYS-ID]
	LEFT OUTER JOIN SRC_LkUpSpecialtyType sc ON sc.[P-SPECL-CD] = spec.[P-SPECL-CD]
	WHERE LEN(RTRIM(LTRIM(spec.[P-SPECL-CD]))) > 0 --AND
		--[P-ENROL-STAT-TY-CD] = '00' AND [P-STAT-END-DT] IS NOT NULL AND
		--CASE WHEN ISDATE([P-STAT-END-DT]) = 1 THEN CAST([P-STAT-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015' AND
		--CASE WHEN ISDATE(spec.[P-SPECL-END-DT]) = 1 THEN spec.[P-SPECL-END-DT] ELSE '1/1/1753' END > '12/31/2015'
	ORDER BY en.[P-TY-CD]
		,spec.[P-SPECL-CD];

	UPDATE #specialtyCodeMap
	SET PDMS_ProviderTypeID = pt.PROVIDER_TYPE_ID
		,PDMS_SpecialtyTypeID = sp.SPECIALTY_TYPE_ID
	FROM #specialtyCodeMap map
	INNER JOIN [dbo].[PROVIDER_TYPE] pt ON pt.[MMIS_PROVIDER_TYPE_ID] = map.[MMIS_ProviderTypeCode]
	INNER JOIN [dbo].[SPECIALTY_TYPE] sp ON sp.[MMIS_SPECIALTY_TYPE_ID] = map.[MMIS_SpecialtyTypeCode]

	INSERT INTO [dbo].[TAXONOMY_TYPE] (
		[SPECIALTY_TYPE_ID]
		,[PROVIDER_TYPE_ID]
		,[TAXONOMY_CODE]
		,[TAXONOMY_NAME]
		,[EXPIRATION_DATE]
		,[LAST_MODIFIED_DATE_TIME]
		,[LAST_MODIFIED_USER]
		,[MMIS_SPECIALTY_TYPE_ID]
		,[NPI_REQUIRED]
		)
	SELECT map.PDMS_SpecialtyTypeID AS [SPECIALTY_TYPE_ID]
		,map.PDMS_ProviderTypeID AS [PROVIDER_TYPE_ID]
		,'' AS [TAXONOMY_CODE]
		,'' AS [TAXONOMY_NAME]
		,'12/31/9999' AS [EXPIRATION_DATE]
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,map.MMIS_SpecialtyTypeCode AS [MMIS_SPECIALTY_TYPE_ID]
		,0 AS [NPI_REQUIRED]
	FROM #specialtyCodeMap map
	WHERE map.PDMS_ProviderTypeID IS NOT NULL;

	DROP TABLE #specialtyCodeMap;

	CREATE TABLE #taxonomyTypeMap (
		PDMS_ProviderTypeID INT
		,MMIS_ProviderTypeCode VARCHAR(3)
		,MMIS_TaxonomyCode VARCHAR(80)
		,MMIS_TaxonomyAbbrev VARCHAR(25)
		,MMIS_TaxonomyLongDesc VARCHAR(35)
		)

	INSERT INTO #taxonomyTypeMap
	SELECT DISTINCT NULL AS PDMS_ProviderTypeID
		,en.[P-TY-CD] AS MMIS_ProviderTypeCode
		,tax.[P-TAXONOMY-CD] AS MMIS_TaxonomyCode
		,taxHd.[P-TAXON-CLS-DESC] AS MMIS_TaxonomyAbbrev
		,taxHd.[P-TAXON-LONG-DESC] AS MMIS_TaxonomyLongDesc
	FROM SRC_Enrollments en
	INNER JOIN SRC_ProviderTaxonomy tax ON en.[P-SYS-ID] = tax.[P-SYS-ID]
	INNER JOIN SRC_ProviderTaxonomyHeader taxHd ON taxHd.[P-TAXONOMY-CD] = tax.[P-TAXONOMY-CD]
	WHERE LEN(RTRIM(LTRIM(tax.[P-TAXONOMY-CD]))) > 0;

	UPDATE #taxonomyTypeMap
	SET PDMS_ProviderTypeID = pt.PROVIDER_TYPE_ID
	FROM #taxonomyTypeMap map
	INNER JOIN [dbo].[PROVIDER_TYPE] pt ON pt.[MMIS_PROVIDER_TYPE_ID] = map.[MMIS_ProviderTypeCode];

	INSERT INTO [dbo].[TAXONOMY_TYPE] (
		[SPECIALTY_TYPE_ID]
		,[PROVIDER_TYPE_ID]
		,[TAXONOMY_CODE]
		,[TAXONOMY_NAME]
		,[EXPIRATION_DATE]
		,[LAST_MODIFIED_DATE_TIME]
		,[LAST_MODIFIED_USER]
		,[MMIS_SPECIALTY_TYPE_ID]
		,[NPI_REQUIRED]
		)
	SELECT 0 AS [SPECIALTY_TYPE_ID]
		,map.PDMS_ProviderTypeID AS [PROVIDER_TYPE_ID]
		,map.MMIS_TaxonomyCode AS [TAXONOMY_CODE]
		,map.MMIS_TaxonomyLongDesc AS [TAXONOMY_NAME]
		,'12/31/9999' AS EXPIRATION_DATE
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,'' AS [MMIS_SPECIALTY_TYPE_ID]
		,1 AS [NPI_REQUIRED]
	FROM #taxonomyTypeMap map
	WHERE map.PDMS_ProviderTypeID IS NOT NULL;

	DROP TABLE #taxonomyTypeMap;
		--	EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
END
GO

/****** Object:  StoredProcedure [dbo].[DCConv_CreateDCStagingTableSynonyms]    Script Date: 6/3/2016 11:50:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Creates the synonyms for referring to the DC data staging tables.
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_CreateDCStagingTableSynonyms] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- create synonyms for each DC data source table
	CREATE SYNONYM SRC_Providers FOR  [DC_PDMS_CONVSRC01].[dbo].[DT_PROVDRTB_STG];
	CREATE SYNONYM SRC_Enrollments FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PENROLTB_STG];
	CREATE SYNONYM SRC_ProviderAddresses FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PADDRSTB_STG];
	CREATE SYNONYM SRC_ProviderAltIDs FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PALTIDTB_STG];
	CREATE SYNONYM SRC_ProviderTaxonomy FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PTAXONTB_STG];
	CREATE SYNONYM SRC_ProviderSpecialty FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PSPECLTB_STG];
	CREATE SYNONYM SRC_ProviderLicense FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PLICNSTB_STG];
	CREATE SYNONYM SRC_ProviderCLIA FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PCLIAPTB_STG];
	CREATE SYNONYM SRC_ProviderCLIADetails FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PCLIACTB_STG];
	CREATE SYNONYM SRC_ProviderMedicare FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PMCARETB_STG];
	CREATE SYNONYM SRC_ProviderMedicaid FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PREVMCTB_STG];
	CREATE SYNONYM SRC_ProviderOwner FOR [DC_PDMS_CONVSRC01].[dbo].[DT_POWNINTB_STG];
	CREATE SYNONYM SRC_ProviderTaxonomyHeader FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PTXNHDTB_STG];
	CREATE SYNONYM SRC_ProviderAffiliates FOR [DC_PDMS_CONVSRC01].[dbo].[DT_PAFFILTB_STG];
	
	CREATE SYNONYM SRC_LkUpAddressType FOR [DC_PDMS_CONVSRC01].[dbo].[LKUP_PROVIDER_ADDRESS_TYPE];
	CREATE SYNONYM SRC_LkUpProviderType FOR [DC_PDMS_CONVSRC01].[dbo].[LKUP_PROVIDER_TYPE_CODE];
	CREATE SYNONYM SRC_LkUpSpecialtyType FOR [DC_PDMS_CONVSRC01].[dbo].[LKUP_PROVIDER_SPECIALTY_CODE];
	
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_CreateUtilityTables]    Script Date: 6/3/2016 11:50:14 AM ******/
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
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_DropDCStagingTableSynonyms]    Script Date: 6/3/2016 11:50:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- DROP date: 5/12/2016
-- Description:	Drop the DC staging table synonyms
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_DropDCStagingTableSynonyms] 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- DROP synonyms for each DC data source table
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_Providers'))
		DROP SYNONYM SRC_Providers;

	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_Enrollments'))
		DROP SYNONYM SRC_Enrollments;

	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderAddresses'))
		DROP SYNONYM SRC_ProviderAddresses;

    IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderAltIDs'))
		DROP SYNONYM SRC_ProviderAltIDs;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderTaxonomy'))
		DROP SYNONYM SRC_ProviderTaxonomy;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderSpecialty'))
		DROP SYNONYM SRC_ProviderSpecialty;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderLicense'))
		DROP SYNONYM SRC_ProviderLicense;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderCLIA'))
		DROP SYNONYM SRC_ProviderCLIA;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderCLIADetails'))
		DROP SYNONYM SRC_ProviderCLIADetails;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderMedicare'))
		DROP SYNONYM SRC_ProviderMedicare;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderMedicaid'))
		DROP SYNONYM SRC_ProviderMedicaid;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderOwner'))
		DROP SYNONYM SRC_ProviderOwner;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderTaxonomyHeader'))
		DROP SYNONYM SRC_ProviderTaxonomyHeader;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderAffiliates'))
		DROP SYNONYM SRC_ProviderAffiliates;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpAddressType'))
		DROP SYNONYM SRC_LkUpAddressType;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpProviderType'))
		DROP SYNONYM SRC_LkUpProviderType;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpSpecialtyType'))
		DROP SYNONYM SRC_LkUpSpecialtyType;
END

GO

/****** Object:  StoredProcedure [dbo].[DCConv_Validate]    Script Date: 6/3/2016 11:50:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/26/2016
-- Description:	Validates the data
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Validate] 
	-- Add the parameters for the stored procedure here
	@pin_conv_run_id varchar(20), 
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Query VARCHAR(MAX), @Column VARCHAR(100), @Table VARCHAR(100)
	DECLARE @LkUp_Column varchar(100), @LkUp_Table varchar(100);

	-- Find the bad dates in the data
	DECLARE Col CURSOR FOR
	SELECT Table_Name, Column_Name
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE Column_Name LIKE '%-DT' AND Table_Name LIKE 'DT_%' AND 
	RTRIM(LTRIM(Column_Name)) <> 'G-AUD-DT'  
	ORDER BY TABLE_NAME, ORDINAL_POSITION

	OPEN Col
	FETCH NEXT FROM Col INTO @Table, @Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT @Table; PRINT @Column;
		IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE Table_Name = @Table AND Column_Name = 'P-SYS-ID'))
		BEGIN
			SET @Query = 'UPDATE [' + @Table + '] SET [ValidationErrorCode] = 1,';
			SET @Query = @Query + '[ValidationErrorData] = prov.[P-ID]  + ''||'' + ' + @Column + ' + ''|'' + tgt[' + @Column + '] '; 
			SET @Query = @Query + 'FROM	 [' + @Table + '] tgt ';
			SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID] = tgt.[P-SYS-ID]';
			SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
			SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
		END
		ELSE
		BEGIN
			IF (CHARINDEX('AFFIL',@Table) > 0) 
			BEGIN
				SET @Query = 'UPDATE [' + @Table + '] SET [ValidationErrorCode] = 1,';
				SET @Query = @Query + '[ValidationErrorData] = grp_prov.[P-ID]  + ''|'' + mem_prov.[P-ID]  + ''|''' + @Column + ' + ''|'' + tgt[' + @Column + '] '; 
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID] = tgt.[P-GROUP-SYS-ID]';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID] = tgt.[P-MEMBER-SYS-ID]';
				SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
			ELSE
			BEGIN
				SET @Query = 'UPDATE [' + @Table + '] SET [ValidationErrorCode] = 1,';
				SET @Query = @Query + '[ValidationErrorData] = ''||'' + ' + @Column + ' + ''|'' + tgt[' + @Column + '] '; 
				SET @Query = @Query + 'FROM	 [' + @Table + '] tgt ';
				SET @Query = @Query + ' WHERE (ISNUMERIC(LEFT(tgt.[' + @Column + '],4)) = 0 OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) < 1900 AND tgt.[' + @Column + '] <> ''1753-01-01'')';
				SET @Query = @Query + ' OR (CAST(LEFT(tgt.[' + @Column + '], 4) AS int) > 2100 AND tgt.[' + @Column + '] <> ''9999-12-31''))';
			END
		END
				 
		PRINT @Query;
		EXEC (@Query)

		FETCH NEXT FROM Col INTO @Table, @Column
	END
	CLOSE Col
	DEALLOCATE Col

	-- Find any bad look up values in the tables
	DECLARE LkUp_Col CURSOR FOR
	SELECT Table_Name, Column_Name
	FROM INFORMATION_SCHEMA.COLUMNS
	WHERE Column_Name LIKE 'P-%' AND Table_Name LIKE 'LKUP_%' AND Column_Name <> 'P-SYS-ID'
	ORDER BY TABLE_NAME, ORDINAL_POSITION

	OPEN LkUp_Col
	FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE Dt_Col CURSOR FOR
		SELECT Table_Name, Column_Name
		FROM INFORMATION_SCHEMA.COLUMNS
		WHERE Column_Name = @LkUp_Column AND Table_Name LIKE 'DT_%'
		ORDER BY TABLE_NAME, ORDINAL_POSITION

		OPEN Dt_Col
		FETCH NEXT FROM Dt_Col INTO @Table, @Column
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE Table_Name = @Table AND Column_Name = 'P-SYS-ID'))
			BEGIN
				SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, dt.[P-SYS-ID] AS Provider_Sys_ID, prov.[P-ID] AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
				SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] prov ON prov.[P-SYS-ID]=dt.[P-SYS-ID] '
				SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
				SET @Query = @Query + ' WHERE prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
			END
			ELSE
			BEGIN
				IF (CHARINDEX('AFFIL',@Table) > 0) 
				BEGIN
					SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, dt.[P-SYS-ID] AS Provider_Sys_ID, prov.[P-ID] AS Provider_ID, '''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
					SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] grp_prov ON grp_prov.[P-SYS-ID]=dt.[P-GROUP-SYS-ID] '
					SET @Query = @Query + ' INNER JOIN dbo.[DT_PROVDRTB_STG] mem_prov ON mem_prov.[P-SYS-ID]=dt.[P-MEMBER-SYS-ID] '
					SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
					SET @Query = @Query + ' WHERE grp_prov.[P-REC-TY-CD] = ''P'' AND mem_prov.[P-REC-TY-CD] = ''P'' AND  dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
				END
				ELSE
				BEGIN
					SET @Query = 'INSERT INTO #Results SELECT DISTINCT ''INVALID REF VALUE'' AS Validation_Type, ''No Sys ID'' AS Provider_Sys_ID, ''No Provider ID'' AS Provider_ID,'''' AS Group_Provider_ID, '''' AS Member_Provider_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, dt.[' + @Column + '] AS Bad_Value FROM [' + @Table + '] dt';
					SET @Query = @Query + ' LEFT OUTER JOIN [' + @LkUp_Table + '] lkup ON lkup.[' + @LkUp_Column + '] = dt.[' + @Column + '] ';
					SET @Query = @Query + ' WHERE dt.[' + @Column + '] IS NOT NULL AND LEN(RTRIM(LTRIM(dt.[' + @Column + ']))) > 0 AND lkup.[' + @Column + '] IS NULL';
				END
			END
			PRINT @Query;
			EXEC(@Query);

			FETCH NEXT FROM Dt_Col INTO @Table, @Column
		END
		CLOSE Dt_Col
		DEALLOCATE Dt_Col
		FETCH NEXT FROM LkUp_Col INTO @LkUp_Table, @LkUp_Column
	END
	CLOSE LkUp_Col
	DEALLOCATE LkUp_Col

	---- Find empty required fields in the data
	--DECLARE ColR CURSOR FOR
	--SELECT TableName, ColumnName
	--FROM dbo.[META_MMISRequiredFields]
	--ORDER BY TABLENAME

	--OPEN ColR
	--FETCH NEXT FROM ColR INTO @Table, @Column
	--WHILE @@FETCH_STATUS = 0
	--BEGIN
	--	SET @Query = 'INSERT INTO #Results SELECT ''V0003'' AS Validation_Type, [P-SYS-ID] AS Provider_Sys_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, [' + @Column + '] AS Bad_Value FROM [DT_' + @Table + '_STG]';
	--	SET @Query = @Query + ' WHERE LEN(RTRIM(LTRIM([' + @Column + ']))) = 0 '
		
		 
	--	PRINT @Query;
	--	BEGIN TRY
	--		EXEC (@Query)
	--	END TRY
	--	BEGIN CATCH
	--	SET @Query = 'INSERT INTO #Results SELECT ''V0003'' AS Validation_Type, ''None'' AS Provider_Sys_ID, ''' + @Table + ''' AS Table_Name, ''' + @Column + ''' AS Column_Name, [' + @Column + '] AS Bad_Value FROM [DT_' + @Table + '_STG]';
	--	SET @Query = @Query + ' WHERE LEN(RTRIM(LTRIM([' + @Column + ']))) = 0 '
	--	EXEC (@Query);
	--	END CATCH
	--	FETCH NEXT FROM ColR INTO @Table, @Column
	--END
	--CLOSE ColR
	--DEALLOCATE ColR


	SELECT * FROM #Results ORDER BY Validation_Type
END

GO

