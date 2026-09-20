/****** Object:  StoredProcedure [dbo].[DCConv_ConvertIndividuals_Step1]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertIndividuals_Step1]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertIndividuals_Step1]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/10/2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertIndividuals_Step1] 
(
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime,
	@pin_active_only bit
	
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
		IF OBJECT_ID('tempdb..#lastEnroll') IS NOT NULL
		 DROP TABLE #lastEnroll;
		CREATE TABLE #lastEnroll ([P-SYS-ID] int, [P-STAT-EFF-DT] varchar(10));
		
		IF (@pin_active_only = 1)
		 INSERT INTO #lastEnroll
		 SELECT [P-SYS-ID], max([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		 FROM SRC_Enrollments en 
		 WHERE en.EnableConversion = 1 
		 AND en.[P-ENROL-STAT-TY-CD] <> '52'  -- not converting enrollemnt with status of 52 per Jon 7/22/2016
		 AND en.[P-ENROL-STAT-TY-CD] = '00'  -- active
		  AND CASE WHEN ISDATE(en.[P-STAT-END-DT]) = 1 
		 		   THEN CAST(en.[P-STAT-END-DT] AS datetime) 
	 			   ELSE '1/1/1753' END > @pin_conv_data_export_date
		 GROUP BY [P-SYS-ID]
		ELSE
		 INSERT INTO #lastEnroll
		 SELECT [P-SYS-ID], max([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		 FROM SRC_Enrollments en 
		 WHERE en.EnableConversion = 1 
		 AND en.[P-ENROL-STAT-TY-CD] <> '52'  -- not converting enrollemnt with status of 52 per Jon 7/22/2016
		 GROUP BY [P-SYS-ID]
		
		INSERT INTO DCConv_KeyCrossReferences
		SELECT DISTINCT en.[P-SYS-ID] as SysID, 
		 (ROW_NUMBER() OVER (ORDER BY en.[P-SYS-ID])) + @MaxRegistrationID as RegistrationID,
		 en.[P-STAT-EFF-DT] as EnrollmentStartDate, 
		 0 as IsGroup, 
		 NULL as EnrollmentEndDate		
		FROM #lastEnroll en
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		WHERE prov.[P-REC-TY-CD] = 'P' 
		AND prov.[P-INDIV-GRP-CD] = 'I' 
		AND prov.[EnableConversion] = 1		 
		ORDER BY en.[P-SYS-ID];
		

		--SELECT DISTINCT en.[P-SYS-ID], (ROW_NUMBER() OVER (ORDER BY en.[P-SYS-ID])) + @MaxRegistrationID,en.[P-STAT-EFF-DT], 0, NULL
		--FROM SRC_Enrollments en
		--INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		--WHERE prov.[P-REC-TY-CD] = 'P' AND --en.[P-ENROL-STAT-TY-CD] = '00' AND 
		--en.[P-STAT-END-DT] IS NOT NULL AND
		--CASE WHEN ISDATE(en.[P-STAT-END-DT]) = 1 THEN CAST(en.[P-STAT-END-DT] AS datetime) ELSE '1/1/1753' END > @pin_conv_data_export_date? AND
		--prov.[P-INDIV-GRP-CD] = 'I' AND
		--en.[P-ENROL-STAT-TY-CD] <> '52' AND  -- not converting enrollemnt with status of 52 per Jon 7/22/2016
		--en.[EnableConversion] = 1 AND prov.[EnableConversion] = 1
		-- ORDER BY en.[P-SYS-ID];
		--(SELECT COUNT(*) FROM SRC_ProviderAffiliates aff 
		--WHERE aff.[P-GROUP-SYS-ID] = en.[P-SYS-ID] AND 
		--aff.[P-MEMBER-SYS-ID] IS NOT NULL) = 0

		-- 

		--SELECT * FROM DCConv_KeyCrossReferences g1 WHERE (SELECT COUNT(*) FROM DCConv_KeyCrossReferences g2 WHERE g1.SysID=g2.SysID) > 0 ORDER BY RegistrationId;

		-- convert the registration data
		SET @currentTable = 'REGISTRATION';

		SET IDENTITY_INSERT dbo.[REGISTRATION] ON;

		WITH lastEnroll AS (
		 SELECT [P-SYS-ID], max([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		 FROM SRC_Enrollments 
		 WHERE [P-ENROL-STAT-TY-CD] <> '52'
		 AND [EnableConversion] = 1
		 GROUP BY [P-SYS-ID]
		)
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
		dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) AS [REQUESTED_EFFECTIVE_DATE],
		dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) AS [CHANGE_EFFECTIVE_DATE],
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
		INNER JOIN lastEnroll en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
		WHERE map.IsGroup = 0 AND 
		prov.[EnableConversion] = 1 		
		/* SELECT DISTINCT map.RegistrationId AS REG_ID, 
		dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) AS [REQUESTED_EFFECTIVE_DATE],
		dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) AS [CHANGE_EFFECTIVE_DATE],
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
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
		WHERE map.IsGroup = 0 AND 
		prov.[EnableConversion] = 1 AND 
		en.[EnableConversion] = 1 AND
		dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) = (SELECT MIN(dbo.fn_ConvertDCDateToPDMS([P-STAT-EFF-DT])) FROM SRC_Enrollments WHERE [P-ENROL-STAT-TY-CD] = '00' AND [P-SYS-ID] = map.SysId AND [EnableConversion] = 1);
 */
		SET IDENTITY_INSERT dbo.[REGISTRATION] OFF;


		PRINT 'REG_PROVIDER';
		---- convert the providers
		SET @currentTable = 'REG_PROVIDER';
		WITH deathDt as (
		 SELECT [P-SYS-ID], dbo.fn_ConvertDCDateToPDMS(MAX([P-STAT-END-DT])) as DEATH_DATE
		 FROM SRC_Enrollments enr 
		 WHERE [P-ENROL-STAT-TY-CD] = '41'
		 GROUP BY [P-SYS-ID]
		),
		enrollCode as (
		 SELECT enr.[P-SYS-ID], --dbo.fn_ConvertEnrollmentStatusToPDMS(enr.[P-ENROL-STAT-TY-CD]) as ENROLLMENT_STATUS_CODE
		  enr.[P-ENROL-STAT-TY-CD] as ENROLLMENT_STATUS_CODE,
		  dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) AS END_DATE
		 FROM SRC_Enrollments enr
		 JOIN (SELECT [P-SYS-ID], MAX([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
				FROM SRC_Enrollments
				WHERE EnableConversion = 1
				GROUP BY [P-SYS-ID]) sub
		 ON enr.[P-SYS-ID] = sub.[P-SYS-ID] and enr.[P-STAT-EFF-DT] = sub.[P-STAT-EFF-DT]
		),
		termRsn as (
		 SELECT enr.[P-SYS-ID], dbo.fn_ConvertTerminationReasonToPDMS(enr.[P-ENROL-STAT-TY-CD]) AS TERM_REASON_ID
		 FROM SRC_Enrollments enr
		 JOIN (SELECT [P-SYS-ID], MAX([P-STAT-END-DT]) as [P-STAT-END-DT]
				FROM SRC_Enrollments enr 
				WHERE [P-ENROL-STAT-TY-CD] <> '00'
				GROUP BY [P-SYS-ID]) SUB
		  ON enr.[P-SYS-ID] = sub.[P-SYS-ID] and enr.[P-STAT-END-DT] = sub.[P-STAT-END-DT]
		),
		npi as (
		 SELECT alt_id.[P-SYS-ID], alt_id.[P-ALT-ID],
		   MIN(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-BEG-DT])) as [NPI_START_DATE], 
		   MAX(dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT])) as [NPI_END_DATE]
		 FROM SRC_ProviderAltIDs alt_id 
		 WHERE alt_id.[P-ALT-ID-TY-CD] = 'XX' 
		 AND dbo.fn_ConvertDCDateToPDMS(alt_id.[P-ALT-ID-END-DT]) > @pin_conv_data_export_date
		 GROUP BY alt_id.[P-SYS-ID], alt_id.[P-ALT-ID]
		)
		INSERT INTO REG_PROVIDER
				   ([REG_ID]
				   ,[NAME]
				   ,[PARTY_ID]
				   ,[DBA]
				   ,[NPI]
				   ,[ENTITY_TYPE_ID]
				   ,[PROVIDER_TYPE_ID]				   
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER]
				   ,[ENROLLMENT_STATUS_CODE]
				   ,[TERM_DATE]
				   ,[FIRST_NAME]
				   ,[LAST_NAME]
				   ,[MIDDLE_INITIAL]
				   ,[TITLE]
				   ,[BIRTH_DATE]
				   ,[DEATH_DATE]
				   ,[END_DATE]
				   ,[TERM_REASON_ID]
				   ,[TYPE_OF_PRACTICE_ID]
				   ,[TAX_ID_TYPE_ID]
				   ,[NPI_START_DATE]
				   ,[NPI_END_DATE]
				   ,[APPLICATION_TYPE_ID])
		SELECT DISTINCT map.RegistrationId AS [REG_ID],
			CASE WHEN LEN(RTRIM(LTRIM(prov.[P-NAM]))) = 0 THEN RTRIM(LTRIM(prov.[P-DBA-NAM])) ELSE RTRIM(LTRIM(prov.[P-NAM])) END AS [NAME],
			NULL AS [PARTY_ID],
			prov.[P-DBA-NAM] AS [DBA],
			prov.[P-NPI-NUM] AS [NPI],
			dbo.fn_ConvertProviderEntityTypeToPDMS(prov.[P-SYS-ID],@pin_conv_data_export_date) AS [ENTITY_TYPE_ID],  -- this is also known as the provider category
			dbo.fn_ConvertProviderTypeToPDMS(prov.[P-SYS-ID], @pin_conv_data_export_date) AS [PROVIDER_TYPE_ID], -- use the most recent active enrollment
			1  AS [MODIFIED_STATUS_TYPE_ID],
			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
			ec.ENROLLMENT_STATUS_CODE,
			NULL AS TERM_DATE, -- will have to change later
			CASE WHEN LEN(RTRIM(LTRIM(prov.[P-FST-NAM]))) = 0 
				 THEN prov.[P-DBA-FST-NAM] 
				 ELSE prov.[P-FST-NAM] END AS [FIRST_NAME],
			CASE WHEN LEN(RTRIM(LTRIM(prov.[P-LAST-NAM]))) = 0 
			     THEN prov.[P-DBA-LAST-NAM] 
				 ELSE prov.[P-LAST-NAM] END AS [LAST_NAME],
			CASE WHEN LEN(RTRIM(LTRIM(prov.[P-MI-NAM]))) = 0 
				 THEN prov.[P-DBA-MI-NAM] 
				 ELSE prov.[P-MI-NAM] END AS [MIDDLE_INITIAL],
			prov.[P-SFX-NAM] as [TITLE],
			CASE dbo.fn_ConvertDCDateToPDMS(prov.[P-DOB-DT]) WHEN '01/01/1753' THEN NULL ELSE dbo.fn_ConvertDCDateToPDMS(prov.[P-DOB-DT]) END AS [BIRTH_DATE],
			dd.DEATH_DATE,
			--NULL AS [END_DATE],  
			 ec.END_DATE,    -- according to Jon
			tr.TERM_REASON_ID,
			NULL AS [TYPE_OF_PRACTICE_ID], -- ** Must ask Jon what this should be
 		    15 AS [TAX_ID_TYPE_ID], -- based on input from Diwakar
			npi.NPI_START_DATE,
			npi.NPI_END_DATE,
			dbo.fn_ConvertProviderApplicationTypeToPDMS(en.[P-TY-CD], en.[P-ENROL-STAT-TY-CD]) AS APPLICATION_TYPE_ID
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = map.SysID AND convert(varchar(10),map.EnrollmentStartDate,20) = en.[P-STAT-EFF-DT]
		LEFT OUTER JOIN deathDt dd ON prov.[P-SYS-ID] = dd.[P-SYS-ID]
		LEFT OUTER JOIN enrollCode ec ON prov.[P-SYS-ID] = ec.[P-SYS-ID]
		LEFT OUTER JOIN termRsn tr ON prov.[P-SYS-ID] = tr.[P-SYS-ID]
		LEFT OUTER JOIN npi ON prov.[P-SYS-ID] = npi.[P-SYS-ID] and prov.[P-NPI-NUM] = npi.[P-ALT-ID]
		WHERE map.IsGroup = 0 AND prov.[EnableConversion] = 1 and en.[EnableConversion] = 1;

		-- Provider risk level ID
		UPDATE REG_PROVIDER
		SET PROVIDER_RISK_LEVEL_ID = b.PROVIDER_RISK_LEVEL_ID
		FROM REG_PROVIDER a
		JOIN PROVIDER_TYPE b ON a.PROVIDER_TYPE_ID = b.PROVIDER_TYPE_ID
		JOIN DCConv_KeyCrossReferences map ON a.REG_ID = map.RegistrationID 
		WHERE map.IsGroup = 0;


		-- TAX_ID 
		WITH taxAgg as (
		 SELECT [P-SYS-ID], max([P-TAX-END-DT]) as [P-TAX-END-DT]
		 FROM SRC_ProviderTaxIds 
		 WHERE EnableConversion = 1
		 GROUP BY [P-SYS-ID]		 
		)
		UPDATE REG_PROVIDER
		SET [TAX_ID] = CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000')  
							THEN prov.[P-SSN-NUM] 
							WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000') 
							THEN tax.[P-FED-TAX-ID]
							WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') 
							THEN prov.[P-SSN-NUM]
					   END
			, [TAX_ID_TYPE_ID] = CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000')  
									  THEN 15 
									  WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000') 
									  THEN 16
									  WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') 
									  THEN 15
							     END
			--, [TAX_ID_EFF_DT] = tax.[P-TAX-BEG-DT]  -- Pending DB change
			--, [TAX_ID_END_DT] = tax.[P-TAX-END-DT]
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		JOIN taxAgg on map.SysID = taxAgg.[P-SYS-ID]
		JOIN SRC_ProviderTaxIds tax ON tax.[P-SYS-ID] = taxAgg.[P-SYS-ID] AND tax.[P-TAX-END-DT] = taxAgg.[P-TAX-END-DT]  		
		WHERE map.IsGroup = 0 AND 
		tax.[EnableConversion] = 1;

		-- ALT_TAX_ID 
		WITH taxAgg as (
		 SELECT [P-SYS-ID], max([P-TAX-END-DT]) as [P-TAX-END-DT]
		 FROM SRC_ProviderTaxIds 
		 WHERE EnableConversion = 1
		 GROUP BY [P-SYS-ID]		 
		)
		UPDATE REG_PROVIDER
		SET  [ALT_TAX_ID] = CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000')  
								  THEN tax.[P-FED-TAX-ID] 
								  WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') AND reg.[TAX_ID_TYPE_ID] <> 15
								  THEN prov.[P-SSN-NUM] 
						     END
			, [ALT_TAX_ID_TYPE] = CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000')  
										  THEN 16 
										  WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') AND reg.[TAX_ID_TYPE_ID] <> 15
										  THEN 15
								     END
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		JOIN taxAgg on map.SysID = taxAgg.[P-SYS-ID]
		JOIN SRC_ProviderTaxIds tax ON tax.[P-SYS-ID] = taxAgg.[P-SYS-ID] AND tax.[P-TAX-END-DT] = taxAgg.[P-TAX-END-DT]  		
		WHERE map.IsGroup = 0 AND 
		tax.[EnableConversion] = 1;


		WITH list as (
		SELECT distinct reg.TAX_ID
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID
		WHERE reg.TAX_ID_TYPE_ID = 16
		)
		UPDATE REG_PROVIDER
		SET TAX_ID = NULL, 
			TAX_ID_TYPE_ID = NULL, 
			ALT_TAX_ID = CASE reg.ALT_TAX_ID WHEN NULL THEN reg.TAX_ID ELSE reg.ALT_TAX_ID END, 
			ALT_TAX_ID_TYPE = CASE reg.ALT_TAX_ID WHEN NULL THEN 16 ELSE ALT_TAX_ID_TYPE END
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID
		JOIN list b on reg.TAX_ID = b.TAX_ID
		WHERE reg.TAX_ID_TYPE_ID = 15;

		
		-- address
		WITH lastAddr as (  
		  SELECT [P-SYS-ID], MAX([G-AUD-DT]) as [G-AUD-DT] 
		  FROM SRC_ProviderAddresses 
		  WHERE [P-ADR-TY-CD] = 'L'
		  AND [EnableConversion] = 1
		  GROUP BY [P-SYS-ID]
		  )
		UPDATE REG_PROVIDER
		SET [CONTACT_NAME] = primary_adr.[P-CONTCT-NAM] ,
			[CONTACT_ADDRESS1] = primary_adr.[P-LINE1-AD] ,
			[CONTACT_ADDRESS2] = primary_adr.[P-LINE2-AD] ,
			[CONTACT_CITY] = primary_adr.[P-CITY-NAM] ,
			[CONTACT_STATE] = primary_adr.[P-ST-CD] ,
			[CONTACT_ZIP] = primary_adr.[P-ZIP5-CD] ,
			[CONTACT_EXT_ZIP] = primary_adr.[P-ZIP4-CD] ,
			[CONTACT_PHONE_NUMBER] = primary_adr.[P-CONTCT-PHON-NUM] , 
			[CONTACT_FAX_NUMBER] = case primary_adr.[P-CONTCT-FAX-NUM] when '' then null else primary_adr.[P-CONTCT-FAX-NUM] end ,
			[CONTACT_EMAIL_ADDRESS] = CASE WHEN LEN(RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT]))) > 80 
										   THEN RTRIM(LTRIM(SUBSTRING(primary_adr.[P-CONTCT-EMAIL-AD-TEXT], 1, 80))) 
										   ELSE  RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT])) END ,
			CONTACT_QUADRANT = CASE WHEN primary_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN primary_adr.[G-QUAD-CD] ELSE NULL END,
            CONTACT_WARD = CASE WHEN SUBSTRING(primary_adr.[G-WARD-CD],1,1)	= '0' THEN SUBSTRING(primary_adr.[G-WARD-CD],2,1) 
								WHEN primary_adr.[G-WARD-CD] = '' THEN NULL
								ELSE primary_adr.[G-WARD-CD] END	    
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderAddresses primary_adr ON map.SysID = primary_adr.[P-SYS-ID] 
		JOIN lastAddr la ON primary_adr.[P-SYS-ID] = la.[P-SYS-ID] AND primary_adr.[G-AUD-DT] = la.[G-AUD-DT]
		WHERE map.IsGroup = 0
		AND primary_adr.[P-ADR-TY-CD] = 'L' 
		AND primary_adr.[EnableConversion] = 1;

		
		-- Update ENTITY_TYPE_ID for R01 and R02 provider types
		UPDATE REG_PROVIDER
		SET ENTITY_TYPE_ID = CASE WHEN src.[P-INDIV-GRP-CD] = 'G' THEN '2'
								  WHEN src.[P-NAM-ORG-IND] = 'N' THEN '1'
								  ELSE '3' END
		FROM REG_PROVIDER a
		JOIN PROVIDER_TYPE b on a.PROVIDER_TYPE_ID = b.PROVIDER_TYPE_ID
		JOIN DCConv_KeyCrossReferences map ON a.[REG_ID] = map.[RegistrationID]
		JOIN SRC_Providers src on map.SysID = src.[P-SYS-ID]
		WHERE map.IsGroup = 0
		AND b.MMIS_PROVIDER_TYPE_ID in ('R01', 'R02');

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
           --dbo.fn_ConvertLicenseTypeToPDMS(lic.[P-LIC-CERT-CD]) AS [LICENSE_TYPE_ID],
		   CASE lic.[P-LIC-CERT-CD] WHEN '' THEN NULL ELSE lic.[P-LIC-CERT-CD] END AS [LICENSE_TYPE_ID],
           lic.[P-LIC-CERT-NUM] AS [LICENSE_NUMBER],
           lic.[P-ST-CD] AS [LICENSE_STATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) AS [LICENSE_EFF_DATE],
           dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EXPIR-DT]) AS [LICENSE_END_DATE],
           1 AS [MODIFIED_STATUS_TYPE_ID],
 		   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderLicense lic ON lic.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0 AND
		lic.[EnableConversion] = 1;

		UPDATE dbo.REG_LICENSE 
		SET LICENSE_EFF_DATE = null
		WHERE LICENSE_EFF_DATE='1753-01-01';

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
		WHERE map.IsGroup = 0 AND
		dbo.fn_ConvertDCDateToPDMS(cliad.[P-CERT-EXPIR-DT]) > GETDATE() AND
		clia.[EnableConversion] = 1 AND cliad.[EnableConversion] = 1;


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
           2 AS [ENROLLMENT_STATUS_TYPE_ID]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicare mcare ON mcare.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0 AND
		dbo.fn_ConvertDCDateToPDMS(mcare.[P-MCARE-END-DT]) > GETDATE() AND
		mcare.[EnableConversion] = 1;

		-- copy service locations
		SET @currentTable = 'REG_SERVICE_LOCATION';
		INSERT INTO [dbo].[REG_SERVICE_LOCATION]
			   ([REG_ID]
			   ,[PRACTICE_NAME]
			   ,[MEDICAID_ID]
			   ,[SERVICING_ADDRESS1]
			   ,[SERVICING_ADDRESS2]
			   ,[SERVICING_CITY]
			   --,[SERVICING_COUNTY]
			   ,[SERVICING_STATE]
			   ,[SERVICING_ZIP]
			   ,[SERVICING_EXT_ZIP]
			   ,[SERVICING_COUNTRY]
			   ,[SERVICING_ADDRESS_NAME]
			   ,[SERVICING_PHONE_NUMBER]
			   ,[SERVICING_PHONE_EXT]
			   ,[SERVICING_FAX_NUMBER]
			   ,[SERVICING_EMAIL_ADDRESS]
			   ,[SERVICING_CONTACT_NAME]
			   ,[SERVICING_CONTACT_TYPE]
			   ,[SERVICING_ADDRESS_PHONE_NUMBER]  
			   ,[MAILTO_ADDRESS1]
			   ,[MAILTO_ADDRESS2]
			   ,[MAILTO_CITY]
			   --,[MAILTO_COUNTY]
			   ,[MAILTO_STATE]
			   ,[MAILTO_ZIP]
			   ,[MAILTO_EXT_ZIP]
			   ,[MAILTO_COUNTRY]
			   ,[MAILTO_ADDRESS_NAME]
			   ,[MAILTO_PHONE_NUMBER]
			   ,[MAILTO_PHONE_EXT]
			   ,[MAILTO_FAX_NUMBER]
			   ,[MAILTO_EMAIL_ADDRESS]
			   ,[MAILTO_CONTACT_NAME]			
			   ,[MAILTO_CONTACT_TYPE]	
			   ,[MAILTO_ORGANIZATION_NAME]			   
			   ,[MAILTO_ADDRESS_PHONE_NUMBER]  
			   ,[MAILTO_ADDRESS_FIRSTNAME]
			   ,[MAILTO_ADDRESS_MIDDLENAME]
			   ,[MAILTO_ADDRESS_LASTNAME]
			   ,[MAILTO_ADDRESS_TITLE]
			   ,[PAYTO_ADDRESS1]
			   ,[PAYTO_ADDRESS2]
			   ,[PAYTO_CITY]
			   --,[PAYTO_COUNTY]
			   ,[PAYTO_STATE]
			   ,[PAYTO_ZIP]
			   ,[PAYTO_EXT_ZIP]
			   ,[PAYTO_COUNTRY]
			   ,[PAYTO_ADDRESS_NAME]
			   ,[PAYTO_PHONE_NUMBER]
			   ,[PAYTO_PHONE_EXT]
			   ,[PAYTO_FAX_NUMBER]
			   ,[PAYTO_EMAIL_ADDRESS]
			   ,[PAYTO_CONTACT_NAME]
			   ,[PAYTO_CONTACT_TYPE]
			   ,[PAYTO_ORGANIZATION_NAME]			   
			   ,[PAYTO_ADDRESS_PHONE_NUMBER]  
			   ,[PAYTO_ADDRESS_FIRSTNAME]
			   ,[PAYTO_ADDRESS_MIDDLENAME]
			   ,[PAYTO_ADDRESS_LASTNAME]
			   ,[PAYTO_ADDRESS_TITLE]
			   ,[BILLING_CONTACT_FIRST_NAME]
			   ,[BILLING_CONTACT_LAST_NAME]
			   ,[TAX_ENTITY_TYPE_ID]
			   ,[CHECK_PAYABLE_TO_NAME]
			   ,[MODIFIED_STATUS_TYPE_ID]			
			   ,[REMITTANCE_ADDRESS1]
			   ,[REMITTANCE_ADDRESS2]
			   ,[REMITTANCE_CITY]
			   --,[REMITTANCE_COUNTY]
			   ,[REMITTANCE_STATE]
			   ,[REMITTANCE_ZIP]
			   ,[REMITTANCE_EXT_ZIP]
			   ,[REMITTANCE_PHONE_NUMBER]
			   ,[REMITTANCE_CONTACT_NAME]
			   ,[REMITTANCE_CONTACT_TYPE]			   
			   ,[REMITTANCE_EMAIL]
			   ,[REMITTANCE_FAX_NUMBER]
			   ,[REMITTANCE_ORGANIZATION_NAME]			   
			   ,[REMITTANCE_ADDRESS_PHONE_NUMBER]  
			   ,[REMITTANCE_ADDRESS_FIRSTNAME]
			   ,[REMITTANCE_ADDRESS_MIDDLENAME]
			   ,[REMITTANCE_ADDRESS_LASTNAME]
			   ,[REMITTANCE_ADDRESS_TITLE]
			   ,[OTHER_ADDRESS1]
			   ,[OTHER_ADDRESS2]
			   ,[OTHER_CITY]
			   --,[OTHER_COUNTY]
			   ,[OTHER_STATE]
			   ,[OTHER_ZIP]
			   ,[OTHER_EXT_ZIP]
			   ,[OTHER_PHONE_NUMBER]
			   ,[OTHER_CONTACT_NAME]
			   ,[OTHER_CONTACT_TYPE]			   
			   ,[OTHER_EMAIL]
			   ,[OTHER_FAX_NUMBER]	
			   ,[OTHER_ORGANIZATION_NAME]			   
			   ,[OTHER_ADDRESS_PHONE_NUMBER]  
			   ,[OTHER_ADDRESS_FIRSTNAME]
			   ,[OTHER_ADDRESS_MIDDLENAME]
			   ,[OTHER_ADDRESS_LASTNAME]
			   ,[OTHER_ADDRESS_TITLE]
			   ,[PRACTICE_TYPE_ID]
			   ,[W9_ADDRESS1]
			   ,[W9_ADDRESS2]
			   ,[W9_CITY] 
			   --[W9_COUNTY]
			   ,[W9_STATE]
			   ,[W9_ZIP]
			   ,[W9_EXT_ZIP]
			   ,[W9_PHONE_NUMBER]
			   ,[W9_CONTACT_NAME]   
			   ,[W9_CONTACT_TYPE]			   
			   ,[W9_FAX_NUMBER]			   
			   ,[W9_ORGANIZATION_NAME]			   
			   ,[W9_ADDRESS_PHONE_NUMBER]  
			   ,[W9_ADDRESS_FIRSTNAME]
			   ,[W9_ADDRESS_MIDDLENAME]
			   ,[W9_ADDRESS_LASTNAME]
			   ,[W9_ADDRESS_TITLE]
			   ,[LAST_MODIFIED_DATE_TIME]
			   ,[LAST_MODIFIED_USER]
			   ,[CLAIM_NOOF_ALLOWANCES]
			   ,[FISCAL_YEAR_END]
			   ,[SERVICING_QUADRANT]
			   ,[SERVICING_WARD]
			   ,[MAILTO_QUADRANT]
			   ,[MAILTO_WARD]
			   ,[PAYTO_QUADRANT]
			   ,[PAYTO_WARD]
			   ,[REMITTANCE_QUADRANT]
			   ,[REMITTANCE_WARD]
			   ,[OTHER_QUADRANT]
			   ,[OTHER_WARD]			   
			   ,[W9_QUADRANT]
			   ,[W9_WARD]
			   )
		 SELECT map.RegistrationId AS [REG_ID],
			    '' AS PRACTICE_NAME,
			   prov.[P-ID] AS [MEDICAID_ID],
			   serv_adr.[P-LINE1-AD] AS [SERVICING_ADDRESS1],
			   serv_adr.[P-LINE2-AD] AS [SERVICING_ADDRESS2],
			   serv_adr.[P-CITY-NAM] AS [SERVICING_CITY], 
			   --serv_adr.[P-CNTY-CD] AS [SERVICING_COUNTY],
			   serv_adr.[P-ST-CD] AS [SERVICING_STATE],
			   serv_adr.[P-ZIP5-CD] AS [SERVICING_ZIP],
			   serv_adr.[P-ZIP4-CD] AS [SERVICING_EXT_ZIP],
			   'US' AS [SERVICING_COUNTRY],
			   NULL AS [SERVICING_ADDRESS_NAME],
			   serv_adr.[P-CONTCT-PHON-NUM] AS [SERVICING_PHONE_NUMBER],
			   '' AS [SERVICING_PHONE_EXT],
			   serv_adr.[P-FAX-NUM] AS [SERVICING_FAX_NUMBER], 
			   RTRIM(serv_adr.[P-CONTCT-EMAIL-AD-TEXT]) AS [SERVICING_EMAIL_ADDRESS],
			   serv_adr.[P-CONTCT-NAM] as [SERVICING_CONTACT_NAME],		
			   CASE serv_adr.[P-NAM-ORG-IND] WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [SERVICING_CONTACT_TYPE],	   
			   serv_adr.[P-PHON-NUM] AS [SERVICING_ADDRESS_PHONE_NUMBER],  
			   mail_adr.[P-LINE1-AD] AS [MAILTO_ADDRESS1],
			   mail_adr.[P-LINE2-AD] AS [MAILTO_ADDRESS2],
			   mail_adr.[P-CITY-NAM] AS [MAILTO_CITY], 
			   --mail_adr.[P-CNTY-CD] AS [MAILTO_COUNTY],
			   mail_adr.[P-ST-CD] AS [MAILTO_STATE],
			   mail_adr.[P-ZIP5-CD] AS [MAILTO_ZIP],
			   mail_adr.[P-ZIP4-CD] AS [MAILTO_EXT_ZIP],
			   'US' AS [MAILTO_COUNTRY],
			   NULL AS [MAILTO_ADDRESS_NAME],
			   mail_adr.[P-CONTCT-PHON-NUM] AS [MAILTO_PHONE_NUMBER],
			   '' AS [MAILTO_PHONE_EXT],
			   mail_adr.[P-FAX-NUM] AS [MAILTO_FAX_NUMBER], 
			   RTRIM(mail_adr.[P-CONTCT-EMAIL-AD-TEXT]) AS [MAILTO_EMAIL_ADDRESS],
			   mail_adr.[P-CONTCT-NAM] as [MAILTO_CONTACT_NAME],
			   CASE mail_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [MAILTO_CONTACT_TYPE],	
			   CASE mail_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN mail_adr.[P-NAM] ELSE NULL END as [MAILTO_ORGANIZATION_NAME],					
			   mail_adr.[P-PHON-NUM] AS [MAILTO_ADDRESS_PHONE_NUMBER],  			   
			   CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-FST-NAM] ELSE NULL END as [MAILTO_ADDRESS_FIRSTNAME],
			   CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-MI-NAM] ELSE NULL END as [MAILTO_ADDRESS_MIDDLENAME],
			   CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-LAST-NAM] ELSE NULL END as [MAILTO_ADDRESS_LASTNAME],
			   CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-SFX-NAM] ELSE NULL END as [MAILTO_ADDRESS_TITLE],			   
			   pay_adr.[P-LINE1-AD] AS [PAYTO_ADDRESS1],
			   pay_adr.[P-LINE2-AD] AS [PAYTO_ADDRESS2],
			   pay_adr.[P-CITY-NAM] AS [PAYTO_CITY], 
			   --pay_adr.[P-CNTY-CD] AS [PAYTO_COUNTY],
			   pay_adr.[P-ST-CD] AS [PAYTO_STATE],
			   pay_adr.[P-ZIP5-CD] AS [PAYTO_ZIP],
			   pay_adr.[P-ZIP4-CD] AS [PAYTO_EXT_ZIP],
			   'US' AS [PAYTOTO_COUNTRY],
			   NULL AS [PAYTOTO_ADDRESS_NAME],
			   pay_adr.[P-CONTCT-PHON-NUM] AS [PAYTOTO_PHONE_NUMBER],
			   '' AS [PAYTOTO_PHONE_EXT],
			   pay_adr.[P-FAX-NUM] AS [PAYTOTO_FAX_NUMBER], 
			   RTRIM(pay_adr.[P-CONTCT-EMAIL-AD-TEXT]) AS [PAYTOTO_EMAIL_ADDRESS],
			   pay_adr.[P-CONTCT-NAM] AS [PAYTO_CONTACT_NAME],
			   CASE pay_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [PAYTO_CONTACT_TYPE],
			   CASE pay_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN pay_adr.[P-NAM] ELSE NULL END as PAYTO_ORGANIZATION_NAME,				
			   pay_adr.[P-PHON-NUM] AS [PAYTO_ADDRESS_PHONE_NUMBER],  			   			   
			   CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-FST-NAM] ELSE NULL END as [PAYTO_ADDRESS_FIRSTNAME],
			   CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-MI-NAM] ELSE NULL END as [PAYTO_ADDRESS_MIDDLENAME],
			   CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-LAST-NAM] ELSE NULL END as [PAYTO_ADDRESS_LASTNAME],
			   CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-SFX-NAM] ELSE NULL END as [PAYTO_ADDRESS_TITLE],			   
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 1) AS [BILLING_CONTACT_FIRST_NAME],
			   dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 3) AS [BILLING_CONTACT_LAST_NAME],
			   CASE WHEN tet.TAX_ENTITY_TYPE_ID IS NULL AND prov.[P-OWNER-TY-CD] <> '' 
			        THEN 9 
					ELSE tet.TAX_ENTITY_TYPE_ID END AS TAX_ENTITY_TYPE_ID ,		
			   NULL AS [CHECK_PAYABLE_TO_NAME],
				1 AS [MODIFIED_STATUS_TYPE_ID],				
			   remit.[P-LINE1-AD] AS [REMITTANCE_ADDRESS1],
			   remit.[P-LINE2-AD] AS [REMITTANCE_ADDRESS2],
			   remit.[P-CITY-NAM] AS [REMITTANCE_CITY], 
			   --remit.[P-CNTY-CD] AS [REMITTANCE_COUNTY],
			   remit.[P-ST-CD] AS [REMITTANCE_STATE],
			   remit.[P-ZIP5-CD] AS [REMITTANCE_ZIP],
			   remit.[P-ZIP4-CD] AS [REMITTANCE_EXT_ZIP],
			   remit.[P-CONTCT-PHON-NUM] AS [REMITTANCE_PHONE_NUMBER],
			   remit.[P-CONTCT-NAM] AS [REMITTANCE_CONTACT_NAME],
			   CASE remit.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [REMITTANCE_CONTACT_TYPE],			   
			   RTRIM(remit.[P-CONTCT-EMAIL-AD-TEXT]) AS [REMITTANCE_EMAIL],
			   remit.[P-FAX-NUM] AS [REMITTANCE_FAX_NUMBER], 	
			   CASE remit.[P-NAM-ORG-IND] WHEN 'Y' THEN remit.[P-NAM] ELSE NULL END as [REMITTANCE_ORGANIZATION_NAME],			   
			   remit.[P-PHON-NUM] AS [REMITTANCE_ADDRESS_PHONE_NUMBER],  			   
			   CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-FST-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_FIRSTNAME],
			   CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-MI-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_MIDDLENAME],
			   CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-LAST-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_LASTNAME],
			   CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-SFX-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_TITLE],			   
			   oth.[P-LINE1-AD] AS [OTHER_ADDRESS1],
			   oth.[P-LINE2-AD] AS [OTHER_ADDRESS2],
			   oth.[P-CITY-NAM] AS [OTHER_CITY], 
			   --oth.[P-CNTY-CD] AS [OTHER_COUNTY],
			   oth.[P-ST-CD] AS [OTHER_STATE],
			   oth.[P-ZIP5-CD] AS [OTHER_ZIP],
			   oth.[P-ZIP4-CD] AS [OTHER_EXT_ZIP],
			   oth.[P-CONTCT-PHON-NUM] AS [OTHER_PHONE_NUMBER],
			   oth.[P-CONTCT-NAM] AS [OTHER_CONTACT_NAME],
			   CASE oth.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [OTHER_CONTACT_TYPE],			   
			   RTRIM(oth.[P-CONTCT-EMAIL-AD-TEXT]) AS [OTHER_EMAIL],
			   oth.[P-FAX-NUM] AS [OTHER_FAX_NUMBER],	
			   CASE oth.[P-NAM-ORG-IND] WHEN 'Y' THEN oth.[P-NAM] ELSE NULL END as [OTHER_ORGANIZATION_NAME],			   
			   oth.[P-PHON-NUM] AS [OTHER_ADDRESS_PHONE_NUMBER],  			   
			   CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-FST-NAM] ELSE NULL END as [OTHER_ADDRESS_FIRSTNAME],
			   CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-MI-NAM] ELSE NULL END as [OTHER_ADDRESS_MIDDLENAME],
			   CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-LAST-NAM] ELSE NULL END as [OTHER_ADDRESS_LASTNAME],
			   CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-SFX-NAM] ELSE NULL END as [OTHER_ADDRESS_TITLE],			   
			   ptw.[PRACTICE_TYPE_ID],
			   CASE WHEN w9.[P-LINE1-AD] = '' THEN NULL ELSE w9.[P-LINE1-AD] END AS [W9_ADDRESS1],
			   CASE WHEN w9.[P-LINE2-AD] = '' THEN NULL ELSE w9.[P-LINE2-AD] END AS [W9_ADDRESS2],
			   CASE WHEN w9.[P-CITY-NAM]  = '' THEN NULL ELSE w9.[P-CITY-NAM] END AS [W9_CITY], 
			   --w9.[P-CNTY-CD] AS [W9_COUNTY],
			   CASE WHEN w9.[P-ST-CD]  = '' THEN NULL ELSE w9.[P-ST-CD] END AS [W9_STATE],
			   CASE WHEN w9.[P-ZIP5-CD]  = '' THEN NULL ELSE w9.[P-ZIP5-CD] END AS [W9_ZIP],
			   CASE WHEN w9.[P-ZIP4-CD]  = '' THEN NULL ELSE w9.[P-ZIP4-CD] END AS [W9_EXT_ZIP],
			   CASE WHEN w9.[P-CONTCT-PHON-NUM]  = '' THEN NULL ELSE w9.[P-CONTCT-PHON-NUM] END AS [W9_PHONE_NUMBER],
			   w9.[P-CONTCT-NAM] AS [W9_CONTACT_NAME],    
			   CASE w9.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [W9_CONTACT_TYPE],			   
			   CASE WHEN w9.[P-FAX-NUM] = '' THEN NULL ELSE w9.[P-FAX-NUM]  END AS [W9_FAX_NUMBER], 
			   CASE w9.[P-NAM-ORG-IND] WHEN 'Y' THEN w9.[P-NAM] ELSE NULL END as [W9_ORGANIZATION_NAME],
			   w9.[P-PHON-NUM] AS [W9_ADDRESS_PHONE_NUMBER],  				
			   CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-FST-NAM] ELSE NULL END as [W9_ADDRESS_FIRSTNAME],  
			   CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-MI-NAM] ELSE NULL END as [W9_ADDRESS_MIDDLENAME],
			   CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-LAST-NAM] ELSE NULL END as [W9_ADDRESS_LASTNAME],
			   CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-SFX-NAM] ELSE NULL END as [W9_ADDRESS_TITLE],				
				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
				NULL AS [CLAIM_NOOF_ALLOWANCES],
			   CASE dbo.fn_GetProviderFiscalYearEnd(map.[SysID], prov.[P-MCARE-FY-MO-NUM],prov.[P-MCAID-FY-MO-NUM],prov.[P-FACI-FY-MO-NUM])
				WHEN '01/01/1753' THEN NULL
				ELSE dbo.fn_GetProviderFiscalYearEnd(map.[SysID], prov.[P-MCARE-FY-MO-NUM],prov.[P-MCAID-FY-MO-NUM],prov.[P-FACI-FY-MO-NUM]) END AS [FISCAL_YEAR_END]
				, CASE WHEN serv_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN serv_adr.[G-QUAD-CD] ELSE NULL END as [SERVICING_QUADRANT]
				, CASE WHEN SUBSTRING(serv_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(serv_adr.[G-WARD-CD],2,1) 
					   WHEN serv_adr.[G-WARD-CD] = '' THEN NULL
					   ELSE serv_adr.[G-WARD-CD] END AS [SERVICING_WARD]				
				, CASE WHEN mail_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN mail_adr.[G-QUAD-CD] ELSE NULL END as [MAILTO_QUADRANT] 
				, CASE WHEN SUBSTRING(mail_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(mail_adr.[G-WARD-CD],2,1) 
					   WHEN mail_adr.[G-WARD-CD] = '' THEN NULL
					   ELSE mail_adr.[G-WARD-CD] END AS [MAILTO_WARD]
				, CASE WHEN pay_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN pay_adr.[G-QUAD-CD] ELSE NULL END as [PAYTO_QUADRANT]
				, CASE WHEN SUBSTRING(pay_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(pay_adr.[G-WARD-CD],2,1) 
					   WHEN pay_adr.[G-WARD-CD] = '' THEN NULL
					   ELSE pay_adr.[G-WARD-CD] END AS [PAYTO_WARD]					   				   
				, CASE WHEN remit.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN remit.[G-QUAD-CD] ELSE NULL END as [REMITTANCE_QUADRANT]
				, CASE WHEN SUBSTRING(remit.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(remit.[G-WARD-CD],2,1) 
					   WHEN remit.[G-WARD-CD] = '' THEN NULL
					   ELSE remit.[G-WARD-CD] END AS [REMITTANCE_WARD]			 
				, CASE WHEN oth.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN oth.[G-QUAD-CD] ELSE NULL END as [OTHER_QUADRANT]
				, CASE WHEN SUBSTRING(oth.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(oth.[G-WARD-CD],2,1) 
					   WHEN oth.[G-WARD-CD] = '' THEN NULL
					   ELSE oth.[G-WARD-CD] END AS [OTHER_WARD]				   				 
				, CASE WHEN w9.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN w9.[G-QUAD-CD] ELSE NULL END as [W9_QUADRANT] 
				, CASE WHEN SUBSTRING(w9.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(w9.[G-WARD-CD],2,1) 
					   WHEN w9.[G-WARD-CD] = '' THEN NULL
					   ELSE w9.[G-WARD-CD] END AS [W9_WARD]					   
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		LEFT OUTER JOIN SRC_ProviderAddresses serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] AND serv_adr.[P-ADR-TY-CD] = 'L' AND serv_adr.[EnableConversion] = 1
		LEFT OUTER JOIN SRC_ProviderAddresses mail_adr ON mail_adr.[P-SYS-ID] = map.[SysID] AND mail_adr.[P-ADR-TY-CD] = 'M' AND mail_adr.[EnableConversion] = 1
		LEFT OUTER JOIN SRC_ProviderAddresses pay_adr ON pay_adr.[P-SYS-ID] = map.[SysID] AND pay_adr.[P-ADR-TY-CD] = 'B' AND pay_adr.[EnableConversion] = 1
		LEFT OUTER JOIN SRC_ProviderAddresses remit ON remit.[P-SYS-ID] = map.[SysID] AND remit.[P-ADR-TY-CD] = 'R' AND remit.[EnableConversion] = 1
		LEFT OUTER JOIN SRC_ProviderAddresses oth ON oth.[P-SYS-ID] = map.[SysID] AND oth.[P-ADR-TY-CD] = 'O' AND oth.[EnableConversion] = 1
		LEFT OUTER JOIN SRC_ProviderAddresses w9 ON w9.[P-SYS-ID] = map.[SysID] AND w9.[P-ADR-TY-CD] = 'W' AND w9.[EnableConversion] = 1
		LEFT OUTER JOIN (SELECT min(TAX_ENTITY_TYPE_ID) as TAX_ENTITY_TYPE_ID, MMIS FROM TAX_ENTITY_TYPE GROUP BY MMIS) tet ON prov.[P-OWNER-TY-CD] = tet.MMIS
		LEFT OUTER JOIN PRACTICE_TYPE_W9 ptw ON prov.[P-PRACT-TY-CD] = ptw.PRACTICE_TYPE_MMIS_ID
		WHERE map.IsGroup = 0 AND
		prov.[EnableConversion] = 1;

		-- Set FISCAL_YEAR_END
		UPDATE REG_SERVICE_LOCATION
		SET [FISCAL_YEAR_END] = CASE prov.[P-FACI-FY-MO-NUM] 
						WHEN '01' THEN '9998-12-31'
						WHEN '02' THEN '9999-01-31'
						WHEN '03' THEN '9999-02-28'
						WHEN '04' THEN '9999-03-31'
						WHEN '05' THEN '9999-04-30'
						WHEN '06' THEN '9999-05-31'
						WHEN '07' THEN '9999-06-30'
						WHEN '08' THEN '9999-07-31'
						WHEN '09' THEN '9999-08-31'
						WHEN '10' THEN '9999-09-30'
						WHEN '11' THEN '9999-10-31'
						WHEN '12' THEN '9999-11-30'
						END
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		WHERE map.IsGroup = 0
		AND prov.[EnableConversion] = 1
		
		-- set the new patient and new refferals elements
		UPDATE REG_SERVICE_LOCATION SET 
		OFFICE_NEWPATIENT = 
			CASE 
				WHEN pq.[P-QSTNR-NPP-IND] = 'Y' THEN 1
				WHEN pq.[P-QSTNR-NPP-IND] = 'N' THEN 0
				ELSE NULL 
			END,
		OFFICE_REFFERAL = 
			CASE 
				WHEN pq.[P-QSTNR-NPR-IND] = 'Y' THEN 1
				WHEN pq.[P-QSTNR-NPR-IND] = 'N' THEN 0
				ELSE NULL 
			END
		FROM 
			REG_SERVICE_LOCATION sl 
			INNER JOIN DCConv_KeyCrossReferences map ON map.RegistrationID = sl.REG_ID
			INNER JOIN SRC_ProviderQuestions pq ON pq.[P-SYS-ID] = map.SysID
		WHERE map.IsGroup = 0 AND pq.[EnableConversion] = 1;

		-- Set SERVICING_EXT_ZIP to zeroes if null or empty
		UPDATE [dbo].[REG_SERVICE_LOCATION]
		SET [SERVICING_EXT_ZIP] = '0000'
		WHERE ISNULL([SERVICING_EXT_ZIP],'') = '';

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
           --,[COUNTY]
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
           --adr.[P-CNTY-CD] AS [COUNTY],
           adr.[P-PHON-NUM] AS [PHONE],
           '' AS [PHONE_EXT],
 			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderAddresses adr ON adr.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_LkUpAddressType adr_type ON adr.[P-ADR-TY-CD] = adr_type.[P-ADR-TY-CD]
		WHERE map.IsGroup = 0 AND adr.[P-ADR-TY-CD] NOT IN ('B', 'L', 'M') AND
		adr.[EnableConversion] = 1;

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
		WHERE map.IsGroup = 0 AND
		LEN(RTRIM(LTRIM(prov.[P-DEA-NUM]))) > 0 AND
		prov.[EnableConversion] = 1;
		
		UPDATE dbo.REG_DEA 
		SET DEA_EFF_DATE = null
		WHERE DEA_EFF_DATE='1753-01-01';


		SET @currentTable = 'REG_MEDICAID';	
		WITH lastEnroll AS (
		 SELECT [P-SYS-ID], max([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		 FROM SRC_Enrollments 
		 WHERE [P-ENROL-STAT-TY-CD] <> '52'
		 AND [EnableConversion] = 1
		 GROUP BY [P-SYS-ID]
		)
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
		INNER JOIN lastEnroll le on map.[SysId] = le.[P-SYS-ID]
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = le.[P-SYS-ID] and en.[P-STAT-EFF-DT] = le.[P-STAT-EFF-DT]
		WHERE map.IsGroup = 0 AND
		mcaid.[EnableConversion] = 1 AND
		en.[EnableConversion] = 1;

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
		WHERE map.IsGroup = 0 AND
		prov.[EnableConversion] = 1 AND
		own.[EnableConversion] = 1;
		
		INSERT INTO [dbo].[REG_NUMBER_OF_BEDS]
           ([REG_ID]
           ,[TOTAL_NUM_BEDS]
           ,[NUM_BEDS_ID_CORE]
           ,[BED_SIZE_CD])
		SELECT map.RegistrationID AS [REG_ID],
           bed.[P-TOT-BED-NUM] AS [TOTAL_NUM_BEDS],
           NULL AS [NUM_BEDS_ID_CORE],
           '' AS [BED_SIZE_CD]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderNumOfBeds bed ON bed.[P-SYS-ID] = map.SysID
		WHERE map.IsGroup = 0 AND bed.[EnableConversion] = 1 AND bed.[P-TOT-BED-NUM] > 0;

		SET @currentTable = 'REG_ENROLLMENT';
		INSERT INTO dbo.REG_ENROLLMENT
		(REG_ID, ENROLLMENT_STATUS_CODE, ENROLL_START_DATE_TIME, ENROLL_END_DATE_TIME, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT
		  map.RegistrationID as REG_ID, en.[P-ENROL-STAT-TY-CD] as ENROLLMENT_STATUS_CODE,
		  en.[P-STAT-EFF-DT] as ENROLL_START_DATE_TIME, en.[P-STAT-END-DT] as ENROLL_END_DATE_TIME, 
		  @pin_conv_run_time AS LAST_MODIFIED_DATE_TIME, dbo.fn_GetUniqueGUID(2) AS LAST_MODIFIED_USER
		FROM SRC_Enrollments en 
		INNER JOIN DCConv_KeyCrossReferences map
		ON en.[P-SYS-ID] = map.SysID 
		INNER JOIN dbo.REGISTRATION rg 
		ON map.RegistrationID = rg.REG_ID
		WHERE en.EnableConversion = 1 
		AND en.[P-ENROL-STAT-TY-CD] <> '52'
		AND en.[P-STAT-EFF-DT] <> convert(varchar(10),rg.REQUESTED_EFFECTIVE_DATE,20) 
		AND map.IsGroup = 0;

		INSERT INTO [dbo].[REG_CATEGORY_OF_SERVICE_INFO]
           ([REG_ID]
           ,[StartDate]
           ,[EndDate]
           ,[CATEGORY_OF_SERVICE_TYPE_ID]
           ,[CATEGORY_OF_SERVICE_INFO_ID]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER])
		SELECT 
			map.RegistrationID AS REG_ID,
			[P-COS-BEG-DT] AS StartDate,
			[P-COS-END-DT] AS EndDate,
			--[P-COS-CD] AS CATEGORY_OF_SERVICE_TYPE_ID,  replace with Category_of_Service_Type_ID
			cost.CATEGORY_OF_SERVICE_TYPE_ID,
			NULL AS CATEGORY_OF_SERVICE_INFO_ID,
			 @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderCatOfService catofService ON catOfService.[P-SYS-ID] = map.SysID
		LEFT JOIN CATEGORY_OF_SERVICE_TYPE cost on catofService.[P-COS-CD] = cost.MAX_CATEGORY_OF_SERVICE_TYPE
		WHERE catOfService.EnableConversion = 1 AND
		map.IsGroup = 0;


		SET @currentTable = 'REG_PROGRAM';  -- Pending DB change
		--WITH lastProg as (
		-- SELECT [P-SYS-ID], max([P-PROG-END-DT]) as [P-PROG-END-DT] 
		-- FROM SRC_ProviderProgram
		-- WHERE EnableConversion = 1
		-- GROUP BY [P-SYS-ID])
		--INSERT INTO dbo.REG_PROGRAM
  --         ([REG_ID]
		--   ,[PROG_CD]
		--   ,[PROG_EFF_DT]
		--   ,[PROG_END_DT]
		--   ,[LAST_MODIFIED_DATE_TIME]
  --         ,[LAST_MODIFIED_USER])
		--SELECT map.RegistrationID as REG_ID, 
		--	prog.[P-PROG-CD] as PROG_CD,
		--	prog.[P-PROG-BEG-DT] as PROG_EFF_DT,
		--	prog.[P-PROG-END-DT] as PROG_END_DT,
		--	@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
		--	dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		--FROM DCConv_KeyCrossReferences map 
		--JOIN SRC_ProviderProgram prog ON map.SysID = prog.[P-SYS-ID]
		--JOIN lastProg lp ON prog.[P-SYS-ID] = lp.[P-SYS-ID] and prog.[P-PROG-END-DT] = lp.[P-PROG-END-DT]
		--WHERE map.IsGroup = 0;
			


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
