/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_Address]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_Address]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_Address]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_LICENSE with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_Address] 
(
    @fileName varchar(25),
	@pin_conv_run_time datetime,
	@pin_conv_run_id varchar(20)
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		-- Get list of exceptions that we can't handle because the provider has been in PDMS 
		 DECLARE @LastAuditDT varchar(20) = '2017-01-03 12.52.42' -- prod value


		SELECT @LastAuditDT = LastAuditDT 
		FROM SRC_ProviderAddressTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_ProviderAddressTrkLog);


		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderAddressUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )

		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';

		INSERT [dbo].[SRC_ProviderAddressUpdtExcpt]
		  (REG_ID, [P-SYS-ID], [P-TAX-KEY-ID], [P-ADR-TY-CD], [P-NAM-ORG-IND], [P-NAM],	[P-LAST-NAM],
			[P-FST-NAM], [P-MI-NAM], [P-SFX-NAM], [P-ST-CD], [P-CNTY-CD], [P-LINE1-AD],	[P-LINE2-AD],
			[P-CITY-NAM], [P-ZIP4-CD], [P-ZIP5-CD], [P-PHON-NUM], [P-FAX-NUM], [P-CONTCT-NAM],
			[P-CONTCT-PHON-NUM], [P-CONTCT-FAX-NUM], [P-CONTCT-EMAIL-AD-TEXT], [P-RTRN-MAIL-IND],
			[P-BARCODE-ROUT-DAT], [P-BARCODE-CHK-DAT], [G-AUD-USER-ID], [G-AUD-DT], [G-AUD-TM],
			[G-QUAD-CD], [G-WARD-CD], LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT c.RegistrationID as REG_ID, 
		  b.[P-SYS-ID], b.[P-TAX-KEY-ID], b.[P-ADR-TY-CD], b.[P-NAM-ORG-IND], b.[P-NAM], b.[P-LAST-NAM],
		  b.[P-FST-NAM], b.[P-MI-NAM], b.[P-SFX-NAM], b.[P-ST-CD], b.[P-CNTY-CD], b.[P-LINE1-AD], b.[P-LINE2-AD],
		  b.[P-CITY-NAM], b.[P-ZIP4-CD], b.[P-ZIP5-CD], b.[P-PHON-NUM], b.[P-FAX-NUM], b.[P-CONTCT-NAM],
		  b.[P-CONTCT-PHON-NUM], b.[P-CONTCT-FAX-NUM], b.[P-CONTCT-EMAIL-AD-TEXT], b.[P-RTRN-MAIL-IND],
		  b.[P-BARCODE-ROUT-DAT], b.[P-BARCODE-CHK-DAT], b.[G-AUD-USER-ID], b.[G-AUD-DT], b.[G-AUD-TM],
		  b.[G-QUAD-CD], b.[G-WARD-CD], @pin_conv_run_time as LAST_MODIFIED_DATE_TIME, dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_ProviderAddressUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;


		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderAddressUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )

		SELECT a.[P-SYS-ID]
		INTO #IncludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		LEFT JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID AND c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897'
		WHERE c.REG_ID IS NULL;

		IF OBJECT_ID('tempdb..#Commands') IS NOT NULL
		DROP TABLE #Commands;

		CREATE TABLE #Commands (CmdID int identity(1,1), Command varchar(max));

		INSERT #Commands (Command)
		SELECT 'BEGIN TRY' as Command;

		INSERT #Commands (Command)
		SELECT 'BEGIN TRANSACTION' as Command;


		-- Update REG_PROVIDER
		WITH lastAddr as (  
		  SELECT a.[P-SYS-ID], MAX(a.[G-AUD-DT]) as [G-AUD-DT] 
		  FROM SRC_ProviderAddressUpdt a
		  JOIN #IncludeList b ON a.[P-SYS-ID] = b.[P-SYS-ID] 
		  WHERE a.[P-ADR-TY-CD] = 'L'
		  AND a.[EnableConversion] = 1
		  GROUP BY a.[P-SYS-ID]
		  )
		SELECT reg.REG_ID, 
			primary_adr.[P-CONTCT-NAM] as [CONTACT_NAME],
			primary_adr.[P-LINE1-AD] as [CONTACT_ADDRESS1],
			primary_adr.[P-LINE2-AD] as [CONTACT_ADDRESS2],
			primary_adr.[P-CITY-NAM] as [CONTACT_CITY],
			primary_adr.[P-ST-CD] as [CONTACT_STATE],
			primary_adr.[P-ZIP5-CD] as [CONTACT_ZIP],
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(primary_adr.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(primary_adr.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				primary_adr.[P-ZIP4-CD]	
			END	as [CONTACT_EXT_ZIP],
			primary_adr.[P-CONTCT-PHON-NUM] as [CONTACT_PHONE_NUMBER], 
			case primary_adr.[P-CONTCT-FAX-NUM] when '' then null else primary_adr.[P-CONTCT-FAX-NUM] end as [CONTACT_FAX_NUMBER],
			CASE WHEN LEN(RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT]))) > 80 
				 THEN RTRIM(LTRIM(SUBSTRING(primary_adr.[P-CONTCT-EMAIL-AD-TEXT], 1, 80))) 
				 ELSE  RTRIM(LTRIM(primary_adr.[P-CONTCT-EMAIL-AD-TEXT])) END as [CONTACT_EMAIL_ADDRESS],
			CASE WHEN primary_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN primary_adr.[G-QUAD-CD] ELSE NULL END as [CONTACT_QUADRANT],
            CASE WHEN SUBSTRING(primary_adr.[G-WARD-CD],1,1)	= '0' THEN SUBSTRING(primary_adr.[G-WARD-CD],2,1) 
				 WHEN primary_adr.[G-WARD-CD] = '' THEN NULL
				 ELSE primary_adr.[G-WARD-CD] END as [CONTACT_WARD]   
		INTO #updateCONTACT
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderAddressUpdt primary_adr ON map.SysID = primary_adr.[P-SYS-ID] 
		JOIN lastAddr la ON primary_adr.[P-SYS-ID] = la.[P-SYS-ID] AND primary_adr.[G-AUD-DT] = la.[G-AUD-DT]
		JOIN #IncludeList il ON primary_adr.[P-SYS-ID] = il.[P-SYS-ID] 
		WHERE primary_adr.[P-ADR-TY-CD] = 'L' 
		AND primary_adr.[EnableConversion] = 1;

		INSERT #Commands (Command)
		SELECT 'UPDATE REG_PROVIDER SET [CONTACT_NAME] = ' + CASE WHEN [CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_NAME],'''','''''') ) + '''' END
		 + ', [CONTACT_ADDRESS1] = ' + CASE WHEN [CONTACT_ADDRESS1]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_ADDRESS1],'''','''''') ) + '''' END
		 + ', [CONTACT_ADDRESS2] = ' + CASE WHEN [CONTACT_ADDRESS2] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_ADDRESS2],'''','''''') ) + '''' END
		 + ', [CONTACT_CITY] = ' + CASE WHEN [CONTACT_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_CITY],'''','''''') ) + '''' END
		 + ', [CONTACT_STATE] = ' + CASE WHEN [CONTACT_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_STATE],'''','''''') ) + '''' END
		 + ', [CONTACT_ZIP] = ' + CASE WHEN [CONTACT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_ZIP],'''','''''') ) + '''' END
		 + ', [CONTACT_EXT_ZIP] = ' + CASE WHEN [CONTACT_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_EXT_ZIP],'''','''''') ) + '''' END
		 + ', [CONTACT_PHONE_NUMBER] = ' + CASE WHEN [CONTACT_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_PHONE_NUMBER],'''','''''') ) + '''' END
		 + ', [CONTACT_FAX_NUMBER] = ' + CASE WHEN [CONTACT_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_FAX_NUMBER],'''','''''') ) + '''' END
		 + ', [CONTACT_EMAIL_ADDRESS] = ' + CASE WHEN [CONTACT_EMAIL_ADDRESS] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_EMAIL_ADDRESS],'''','''''') ) + '''' END
		 + ', [CONTACT_QUADRANT] = ' + CASE WHEN [CONTACT_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_QUADRANT],'''','''''') ) + '''' END
         + ', [CONTACT_WARD] = ' + CASE WHEN [CONTACT_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([CONTACT_WARD],'''','''''') ) + '''' END 
		 + ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
		 + ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
		 + ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #updateCONTACT


		-- REG_SERVICE_LOCATION

		-- Servicing addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [SERVICING_ADDRESS1] = serv_adr.[P-LINE1-AD] ,
		--	[SERVICING_ADDRESS2] = serv_adr.[P-LINE2-AD] ,
		--	[SERVICING_CITY] = serv_adr.[P-CITY-NAM] , 
		--	[SERVICING_STATE] = serv_adr.[P-ST-CD] ,
		--	[SERVICING_ZIP] = serv_adr.[P-ZIP5-CD] ,
		--	[SERVICING_EXT_ZIP] = serv_adr.[P-ZIP4-CD] ,
		--	[SERVICING_PHONE_NUMBER] = serv_adr.[P-CONTCT-PHON-NUM] ,
		--	[SERVICING_FAX_NUMBER] = serv_adr.[P-FAX-NUM] , 
		--	[SERVICING_EMAIL_ADDRESS] = RTRIM(serv_adr.[P-CONTCT-EMAIL-AD-TEXT]) ,
		--	[SERVICING_CONTACT_NAME] = serv_adr.[P-CONTCT-NAM] ,		
		--	[SERVICING_CONTACT_TYPE] = CASE serv_adr.[P-NAM-ORG-IND] WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,	   
		--	[SERVICING_ADDRESS_PHONE_NUMBER] = serv_adr.[P-PHON-NUM] , 
		--	[SERVICING_QUADRANT] = CASE WHEN serv_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN serv_adr.[G-QUAD-CD] ELSE NULL END ,
		--	[SERVICING_WARD] = CASE WHEN SUBSTRING(serv_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(serv_adr.[G-WARD-CD],2,1) 
		--							WHEN serv_adr.[G-WARD-CD] = '' THEN NULL
		--							ELSE serv_adr.[G-WARD-CD] END  
		SELECT reg.REG_ID,
		    serv_adr.[P-LINE1-AD] as [SERVICING_ADDRESS1]  ,
			serv_adr.[P-LINE2-AD] as [SERVICING_ADDRESS2]  ,
			serv_adr.[P-CITY-NAM] as [SERVICING_CITY]  , 
			serv_adr.[P-ST-CD] as [SERVICING_STATE]  ,
			serv_adr.[P-ZIP5-CD] as [SERVICING_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(serv_adr.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(serv_adr.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				serv_adr.[P-ZIP4-CD]	
			END as [SERVICING_EXT_ZIP]  ,
			serv_adr.[P-CONTCT-PHON-NUM] as [SERVICING_PHONE_NUMBER]  ,
			serv_adr.[P-FAX-NUM] as [SERVICING_FAX_NUMBER]  , 
			RTRIM(serv_adr.[P-CONTCT-EMAIL-AD-TEXT]) as [SERVICING_EMAIL_ADDRESS]  ,
			serv_adr.[P-CONTCT-NAM] as [SERVICING_CONTACT_NAME]  ,		
			CASE serv_adr.[P-NAM-ORG-IND] WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [SERVICING_CONTACT_TYPE]  ,	   
			serv_adr.[P-PHON-NUM] as [SERVICING_ADDRESS_PHONE_NUMBER]  , 
			CASE WHEN serv_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN serv_adr.[G-QUAD-CD] ELSE NULL END as [SERVICING_QUADRANT]  ,
			CASE WHEN SUBSTRING(serv_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(serv_adr.[G-WARD-CD],2,1) 
				 WHEN serv_adr.[G-WARD-CD] = '' THEN NULL
				 ELSE serv_adr.[G-WARD-CD] END as [SERVICING_WARD] 
		INTO #UpdateServAddr		 		
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] AND serv_adr.[P-ADR-TY-CD] = 'L'
		JOIN #IncludeList il ON serv_adr.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND serv_adr.[EnableConversion] = 1;
		
		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [SERVICING_ADDRESS1] = ' + CASE WHEN [SERVICING_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_ADDRESS1],'''','''''') ) + '''' END
			+ ', [SERVICING_ADDRESS2] = ' + CASE WHEN [SERVICING_ADDRESS2]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_ADDRESS2],'''','''''') ) + '''' END
			+ ', [SERVICING_CITY] = ' + CASE WHEN [SERVICING_CITY]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_CITY],'''','''''') ) + '''' END 
			+ ', [SERVICING_STATE] = ' + CASE WHEN [SERVICING_STATE]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_STATE],'''','''''') ) + '''' END
			+ ', [SERVICING_ZIP] = ' + CASE WHEN [SERVICING_ZIP]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_ZIP],'''','''''') ) + '''' END
			+ ', [SERVICING_EXT_ZIP] = ' + CASE WHEN [SERVICING_EXT_ZIP]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [SERVICING_PHONE_NUMBER] = ' + CASE WHEN [SERVICING_PHONE_NUMBER]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [SERVICING_FAX_NUMBER] = ' + CASE WHEN [SERVICING_FAX_NUMBER]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_FAX_NUMBER],'''','''''') ) + '''' END 
			+ ', [SERVICING_EMAIL_ADDRESS] = ' + CASE WHEN [SERVICING_EMAIL_ADDRESS]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_EMAIL_ADDRESS],'''','''''') ) + '''' END
			+ ', [SERVICING_CONTACT_NAME] = ' + CASE WHEN [SERVICING_CONTACT_NAME]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_CONTACT_NAME],'''','''''') ) + '''' END		
			+ ', [SERVICING_CONTACT_TYPE] = ' + CASE WHEN [SERVICING_CONTACT_TYPE]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_CONTACT_TYPE],'''','''''') ) + '''' END	   
			+ ', [SERVICING_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [SERVICING_ADDRESS_PHONE_NUMBER]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END 
			+ ', [SERVICING_QUADRANT] = ' + CASE WHEN [SERVICING_QUADRANT]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_QUADRANT],'''','''''') ) + '''' END
			+ ', [SERVICING_WARD] = ' + CASE WHEN [SERVICING_WARD]  IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([SERVICING_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #UpdateServAddr

		-- Mail to addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [MAILTO_ADDRESS1] = mail_adr.[P-LINE1-AD] ,
		--	[MAILTO_ADDRESS2] = mail_adr.[P-LINE2-AD] ,
		--	[MAILTO_CITY] = mail_adr.[P-CITY-NAM]  , 
		--	[MAILTO_STATE] = mail_adr.[P-ST-CD] ,
		--	[MAILTO_ZIP] = mail_adr.[P-ZIP5-CD] ,
		--	[MAILTO_EXT_ZIP] = mail_adr.[P-ZIP4-CD] ,
		--	[MAILTO_PHONE_NUMBER] = mail_adr.[P-CONTCT-PHON-NUM] ,
		--	[MAILTO_FAX_NUMBER] = mail_adr.[P-FAX-NUM] , 
		--	[MAILTO_EMAIL_ADDRESS] = RTRIM(mail_adr.[P-CONTCT-EMAIL-AD-TEXT]) ,
		--	[MAILTO_CONTACT_NAME] = mail_adr.[P-CONTCT-NAM] ,
		--	[MAILTO_CONTACT_TYPE] = CASE mail_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,	
		--	[MAILTO_ORGANIZATION_NAME] = CASE mail_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN mail_adr.[P-NAM] ELSE NULL END ,					
		--	[MAILTO_ADDRESS_PHONE_NUMBER] = mail_adr.[P-PHON-NUM] ,  			   
		--	[MAILTO_ADDRESS_FIRSTNAME] = CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-FST-NAM] ELSE NULL END ,
		--	[MAILTO_ADDRESS_MIDDLENAME] = CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-MI-NAM] ELSE NULL END ,
		--	[MAILTO_ADDRESS_LASTNAME] = CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-LAST-NAM] ELSE NULL END ,
		--	[MAILTO_ADDRESS_TITLE] = CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-SFX-NAM] ELSE NULL END  ,
		--	[MAILTO_QUADRANT] = CASE WHEN mail_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN mail_adr.[G-QUAD-CD] ELSE NULL END ,  
		--	[MAILTO_WARD] = CASE WHEN SUBSTRING(mail_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(mail_adr.[G-WARD-CD],2,1) 
		--						 WHEN mail_adr.[G-WARD-CD] = '' THEN NULL
		--						 ELSE mail_adr.[G-WARD-CD] END 
		SELECT reg.REG_ID,
			mail_adr.[P-LINE1-AD] as [MAILTO_ADDRESS1]  ,
			mail_adr.[P-LINE2-AD] as [MAILTO_ADDRESS2]  ,
			mail_adr.[P-CITY-NAM] as [MAILTO_CITY]  , 
			mail_adr.[P-ST-CD] as [MAILTO_STATE]  ,
			mail_adr.[P-ZIP5-CD] as [MAILTO_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(mail_adr.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(mail_adr.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				mail_adr.[P-ZIP4-CD]	
			END as [MAILTO_EXT_ZIP]  ,
			mail_adr.[P-CONTCT-PHON-NUM] as [MAILTO_PHONE_NUMBER]  ,
			mail_adr.[P-FAX-NUM] as [MAILTO_FAX_NUMBER]  , 
			RTRIM(mail_adr.[P-CONTCT-EMAIL-AD-TEXT]) as [MAILTO_EMAIL_ADDRESS]  ,
			mail_adr.[P-CONTCT-NAM] as [MAILTO_CONTACT_NAME]  ,
			CASE mail_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [MAILTO_CONTACT_TYPE]  ,	
			CASE mail_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN mail_adr.[P-NAM] ELSE NULL END as [MAILTO_ORGANIZATION_NAME]  ,					
			mail_adr.[P-PHON-NUM] as [MAILTO_ADDRESS_PHONE_NUMBER]  ,  			   
			CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-FST-NAM] ELSE NULL END as [MAILTO_ADDRESS_FIRSTNAME]  ,
			CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-MI-NAM] ELSE NULL END as [MAILTO_ADDRESS_MIDDLENAME]  ,
			CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-LAST-NAM] ELSE NULL END as [MAILTO_ADDRESS_LASTNAME]  ,
			CASE mail_adr.[P-NAM-ORG-IND] WHEN 'N' THEN mail_adr.[P-SFX-NAM] ELSE NULL END as [MAILTO_ADDRESS_TITLE]  ,
			CASE WHEN mail_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN mail_adr.[G-QUAD-CD] ELSE NULL END as [MAILTO_QUADRANT]  ,  
			CASE WHEN SUBSTRING(mail_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(mail_adr.[G-WARD-CD],2,1) 
				 WHEN mail_adr.[G-WARD-CD] = '' THEN NULL
				 ELSE mail_adr.[G-WARD-CD] END as [MAILTO_WARD]
		INTO #UpdateMailAddr
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt mail_adr ON mail_adr.[P-SYS-ID] = map.[SysID] AND mail_adr.[P-ADR-TY-CD] = 'M' 
		JOIN #IncludeList il ON mail_adr.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND mail_adr.[EnableConversion] = 1;

		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [MAILTO_ADDRESS1] = ' + CASE WHEN [MAILTO_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS1],'''','''''') ) + '''' END
			+ ', [MAILTO_ADDRESS2] = ' + CASE WHEN [MAILTO_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS1],'''','''''') ) + '''' END
			+ ', [MAILTO_CITY] = ' + CASE WHEN [MAILTO_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_CITY],'''','''''') ) + '''' END 
			+ ', [MAILTO_STATE] = ' + CASE WHEN [MAILTO_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_STATE],'''','''''') ) + '''' END
			+ ', [MAILTO_ZIP] = ' + CASE WHEN [MAILTO_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ZIP],'''','''''') ) + '''' END
			+ ', [MAILTO_EXT_ZIP] = ' + CASE WHEN [MAILTO_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [MAILTO_PHONE_NUMBER] = ' + CASE WHEN [MAILTO_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [MAILTO_FAX_NUMBER] = ' + CASE WHEN [MAILTO_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_FAX_NUMBER],'''','''''') ) + '''' END
			+ ', [MAILTO_EMAIL_ADDRESS] = ' + CASE WHEN [MAILTO_EMAIL_ADDRESS] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_EMAIL_ADDRESS],'''','''''') ) + '''' END
			+ ', [MAILTO_CONTACT_NAME] = ' + CASE WHEN [MAILTO_CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_CONTACT_NAME],'''','''''') ) + '''' END
			+ ', [MAILTO_CONTACT_TYPE] = ' + CASE WHEN [MAILTO_CONTACT_TYPE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_CONTACT_TYPE],'''','''''') ) + '''' END	
			+ ', [MAILTO_ORGANIZATION_NAME] = ' + CASE WHEN [MAILTO_ORGANIZATION_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ORGANIZATION_NAME],'''','''''') ) + '''' END					
			+ ', [MAILTO_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [MAILTO_ADDRESS_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END 			   
			+ ', [MAILTO_ADDRESS_FIRSTNAME] = ' + CASE WHEN [MAILTO_ADDRESS_FIRSTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS_FIRSTNAME],'''','''''') ) + '''' END
			+ ', [MAILTO_ADDRESS_MIDDLENAME] = ' + CASE WHEN [MAILTO_ADDRESS_MIDDLENAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS_MIDDLENAME],'''','''''') ) + '''' END
			+ ', [MAILTO_ADDRESS_LASTNAME] = ' + CASE WHEN [MAILTO_ADDRESS_LASTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS_LASTNAME],'''','''''') ) + '''' END
			+ ', [MAILTO_ADDRESS_TITLE] = ' + CASE WHEN [MAILTO_ADDRESS_TITLE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_ADDRESS_TITLE],'''','''''') ) + '''' END
			+ ', [MAILTO_QUADRANT] = ' + CASE WHEN [MAILTO_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_QUADRANT],'''','''''') ) + '''' END 
			+ ', [MAILTO_WARD] = ' + CASE WHEN [MAILTO_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([MAILTO_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #UpdateMailAddr

		-- Pay to addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [PAYTO_ADDRESS1] = pay_adr.[P-LINE1-AD] ,
		--	[PAYTO_ADDRESS2] = pay_adr.[P-LINE2-AD] ,
		--	[PAYTO_CITY] = pay_adr.[P-CITY-NAM] , 
		--	[PAYTO_STATE] = pay_adr.[P-ST-CD] ,
		--	[PAYTO_ZIP] = pay_adr.[P-ZIP5-CD] ,
		--	[PAYTO_EXT_ZIP] = pay_adr.[P-ZIP4-CD] ,
		--	[PAYTO_PHONE_NUMBER] = pay_adr.[P-CONTCT-PHON-NUM] ,
		--	[PAYTO_FAX_NUMBER] = pay_adr.[P-FAX-NUM] , 
		--	[PAYTO_EMAIL_ADDRESS] = RTRIM(pay_adr.[P-CONTCT-EMAIL-AD-TEXT]) ,
		--	[PAYTO_CONTACT_NAME] = pay_adr.[P-CONTCT-NAM] ,
		--	[PAYTO_CONTACT_TYPE] = CASE pay_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,
		--	[PAYTO_ORGANIZATION_NAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN pay_adr.[P-NAM] ELSE NULL END ,				
		--	[PAYTO_ADDRESS_PHONE_NUMBER] = pay_adr.[P-PHON-NUM] ,  			   			   
		--	[PAYTO_ADDRESS_FIRSTNAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-FST-NAM] ELSE NULL END ,
		--	[PAYTO_ADDRESS_MIDDLENAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-MI-NAM] ELSE NULL END ,
		--	[PAYTO_ADDRESS_LASTNAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-LAST-NAM] ELSE NULL END ,
		--	[PAYTO_ADDRESS_TITLE] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-SFX-NAM] ELSE NULL END ,			   
		--	[BILLING_CONTACT_FIRST_NAME] = dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 1) ,
		--	[BILLING_CONTACT_LAST_NAME] =  dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 3) ,
		--	[PAYTO_QUADRANT] = CASE WHEN pay_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN pay_adr.[G-QUAD-CD] ELSE NULL END , 
		--	[PAYTO_WARD] = CASE WHEN SUBSTRING(pay_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(pay_adr.[G-WARD-CD],2,1) 
		--			   WHEN pay_adr.[G-WARD-CD] = '' THEN NULL
		--			   ELSE pay_adr.[G-WARD-CD] END 
		SELECT reg.REG_ID, 
			pay_adr.[P-LINE1-AD] as [PAYTO_ADDRESS1]  ,
			pay_adr.[P-LINE2-AD] as [PAYTO_ADDRESS2]  ,
			pay_adr.[P-CITY-NAM] as [PAYTO_CITY]  , 
			pay_adr.[P-ST-CD] as [PAYTO_STATE]  ,
			pay_adr.[P-ZIP5-CD] as [PAYTO_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(pay_adr.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(pay_adr.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				pay_adr.[P-ZIP4-CD]	
			END as [PAYTO_EXT_ZIP]  ,
			pay_adr.[P-CONTCT-PHON-NUM] as [PAYTO_PHONE_NUMBER]  ,
			pay_adr.[P-FAX-NUM] as [PAYTO_FAX_NUMBER]  , 
			RTRIM(pay_adr.[P-CONTCT-EMAIL-AD-TEXT]) as [PAYTO_EMAIL_ADDRESS]  ,
			[PAYTO_CONTACT_NAME] = pay_adr.[P-CONTCT-NAM] ,
			[PAYTO_CONTACT_TYPE] = CASE pay_adr.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,
			[PAYTO_ORGANIZATION_NAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'Y' THEN pay_adr.[P-NAM] ELSE NULL END ,				
			[PAYTO_ADDRESS_PHONE_NUMBER] = pay_adr.[P-PHON-NUM] ,  			   			   
			[PAYTO_ADDRESS_FIRSTNAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-FST-NAM] ELSE NULL END ,
			[PAYTO_ADDRESS_MIDDLENAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-MI-NAM] ELSE NULL END ,
			[PAYTO_ADDRESS_LASTNAME] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-LAST-NAM] ELSE NULL END ,
			[PAYTO_ADDRESS_TITLE] = CASE pay_adr.[P-NAM-ORG-IND] WHEN 'N' THEN pay_adr.[P-SFX-NAM] ELSE NULL END ,			   
			[BILLING_CONTACT_FIRST_NAME] = dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 1) ,
			[BILLING_CONTACT_LAST_NAME] =  dbo.fn_ParseName(pay_adr.[P-CONTCT-NAM], 3) ,
			[PAYTO_QUADRANT] = CASE WHEN pay_adr.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN pay_adr.[G-QUAD-CD] ELSE NULL END , 
			[PAYTO_WARD] = CASE WHEN SUBSTRING(pay_adr.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(pay_adr.[G-WARD-CD],2,1) 
					   WHEN pay_adr.[G-WARD-CD] = '' THEN NULL
					   ELSE pay_adr.[G-WARD-CD] END 
		INTO #UpdatePayAddr
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt pay_adr ON pay_adr.[P-SYS-ID] = map.[SysID] AND pay_adr.[P-ADR-TY-CD] = 'B' 
		JOIN #IncludeList il ON pay_adr.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND pay_adr.[EnableConversion] = 1;

		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [PAYTO_ADDRESS1] = ' + CASE WHEN [PAYTO_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS1],'''','''''') ) + '''' END
			+ ', [PAYTO_ADDRESS2] = ' + CASE WHEN [PAYTO_ADDRESS2] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS2],'''','''''') ) + '''' END
			+ ', [PAYTO_CITY] = ' + CASE WHEN [PAYTO_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_CITY],'''','''''') ) + '''' END
			+ ', [PAYTO_STATE] = ' + CASE WHEN [PAYTO_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_STATE],'''','''''') ) + '''' END
			+ ', [PAYTO_ZIP] = ' + CASE WHEN [PAYTO_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ZIP],'''','''''') ) + '''' END
			+ ', [PAYTO_EXT_ZIP] = ' + CASE WHEN [PAYTO_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [PAYTO_PHONE_NUMBER] = ' + CASE WHEN [PAYTO_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [PAYTO_FAX_NUMBER] = ' + CASE WHEN [PAYTO_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_FAX_NUMBER],'''','''''') ) + '''' END 
			+ ', [PAYTO_EMAIL_ADDRESS] = ' + CASE WHEN [PAYTO_EMAIL_ADDRESS] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_EMAIL_ADDRESS],'''','''''') ) + '''' END
			+ ', [PAYTO_CONTACT_NAME] = ' + CASE WHEN [PAYTO_CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_CONTACT_NAME],'''','''''') ) + '''' END
			+ ', [PAYTO_CONTACT_TYPE] = ' + CASE WHEN [PAYTO_CONTACT_TYPE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_CONTACT_TYPE],'''','''''') ) + '''' END
			+ ', [PAYTO_ORGANIZATION_NAME] = ' + CASE WHEN [PAYTO_ORGANIZATION_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ORGANIZATION_NAME],'''','''''') ) + '''' END			
			+ ', [PAYTO_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [PAYTO_ADDRESS_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END  			   			   
			+ ', [PAYTO_ADDRESS_FIRSTNAME] = ' + CASE WHEN [PAYTO_ADDRESS_FIRSTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS_FIRSTNAME],'''','''''') ) + '''' END
			+ ', [PAYTO_ADDRESS_MIDDLENAME] = ' + CASE WHEN [PAYTO_ADDRESS_MIDDLENAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS_MIDDLENAME],'''','''''') ) + '''' END
			+ ', [PAYTO_ADDRESS_LASTNAME] = ' + CASE WHEN [PAYTO_ADDRESS_LASTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS_LASTNAME],'''','''''') ) + '''' END
			+ ', [PAYTO_ADDRESS_TITLE] = ' + CASE WHEN [PAYTO_ADDRESS_TITLE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_ADDRESS_TITLE],'''','''''') ) + '''' END			   
			+ ', [BILLING_CONTACT_FIRST_NAME] = ' + CASE WHEN [BILLING_CONTACT_FIRST_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([BILLING_CONTACT_FIRST_NAME],'''','''''') ) + '''' END
			+ ', [BILLING_CONTACT_LAST_NAME] =  ' + CASE WHEN [BILLING_CONTACT_LAST_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([BILLING_CONTACT_LAST_NAME],'''','''''') ) + '''' END
			+ ', [PAYTO_QUADRANT] = ' + CASE WHEN [PAYTO_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_QUADRANT],'''','''''') ) + '''' END 
			+ ', [PAYTO_WARD] = ' + CASE WHEN [PAYTO_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([PAYTO_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #UpdatePayAddr

		-- Remit addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [REMITTANCE_ADDRESS1] = remit.[P-LINE1-AD] ,
		--	[REMITTANCE_ADDRESS2] = remit.[P-LINE2-AD] ,
		--	[REMITTANCE_CITY] = remit.[P-CITY-NAM] , 
		--	[REMITTANCE_STATE] = remit.[P-ST-CD] ,
		--	[REMITTANCE_ZIP] = remit.[P-ZIP5-CD] ,
		--	[REMITTANCE_EXT_ZIP] = remit.[P-ZIP4-CD] ,
		--	[REMITTANCE_PHONE_NUMBER] = remit.[P-CONTCT-PHON-NUM] ,
		--	[REMITTANCE_CONTACT_NAME] = remit.[P-CONTCT-NAM]  ,
		--	[REMITTANCE_CONTACT_TYPE] = CASE remit.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,			   
		--	[REMITTANCE_EMAIL] = RTRIM(remit.[P-CONTCT-EMAIL-AD-TEXT]) ,
		--	[REMITTANCE_FAX_NUMBER] = remit.[P-FAX-NUM] , 	
		--	[REMITTANCE_ORGANIZATION_NAME] = CASE remit.[P-NAM-ORG-IND] WHEN 'Y' THEN remit.[P-NAM] ELSE NULL END ,			   
		--	[REMITTANCE_ADDRESS_PHONE_NUMBER] = remit.[P-PHON-NUM] ,  			   
		--	[REMITTANCE_ADDRESS_FIRSTNAME] = CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-FST-NAM] ELSE NULL END ,
		--	[REMITTANCE_ADDRESS_MIDDLENAME] = CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-MI-NAM] ELSE NULL END ,
		--	[REMITTANCE_ADDRESS_LASTNAME] = CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-LAST-NAM] ELSE NULL END ,
		--	[REMITTANCE_ADDRESS_TITLE] = CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-SFX-NAM] ELSE NULL END , 
		--	[REMITTANCE_QUADRANT] = CASE WHEN remit.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN remit.[G-QUAD-CD] ELSE NULL END , 
		--	[REMITTANCE_WARD] = CASE WHEN SUBSTRING(remit.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(remit.[G-WARD-CD],2,1) 
		--			   WHEN remit.[G-WARD-CD] = '' THEN NULL
		--			   ELSE remit.[G-WARD-CD] END  	
		SELECT reg.REG_ID,
		    remit.[P-LINE1-AD] as [REMITTANCE_ADDRESS1]  ,
			remit.[P-LINE2-AD] as [REMITTANCE_ADDRESS2]  ,
			remit.[P-CITY-NAM] as [REMITTANCE_CITY]  , 
			remit.[P-ST-CD] as [REMITTANCE_STATE]  ,
			remit.[P-ZIP5-CD] as [REMITTANCE_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(remit.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(remit.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				remit.[P-ZIP4-CD]	
			END as [REMITTANCE_EXT_ZIP]  ,
			remit.[P-CONTCT-PHON-NUM] as [REMITTANCE_PHONE_NUMBER]  ,
			remit.[P-CONTCT-NAM] as [REMITTANCE_CONTACT_NAME]  ,
			CASE remit.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [REMITTANCE_CONTACT_TYPE]  ,			   
			RTRIM(remit.[P-CONTCT-EMAIL-AD-TEXT]) as [REMITTANCE_EMAIL]  ,
			remit.[P-FAX-NUM] as [REMITTANCE_FAX_NUMBER]  , 	
			CASE remit.[P-NAM-ORG-IND] WHEN 'Y' THEN remit.[P-NAM] ELSE NULL END as [REMITTANCE_ORGANIZATION_NAME]  ,			   
			remit.[P-PHON-NUM] as [REMITTANCE_ADDRESS_PHONE_NUMBER]  ,  			   
			CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-FST-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_FIRSTNAME]  ,
			CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-MI-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_MIDDLENAME]  ,
			CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-LAST-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_LASTNAME]  ,
			CASE remit.[P-NAM-ORG-IND] WHEN 'N' THEN remit.[P-SFX-NAM] ELSE NULL END as [REMITTANCE_ADDRESS_TITLE]  , 
			CASE WHEN remit.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN remit.[G-QUAD-CD] ELSE NULL END as [REMITTANCE_QUADRANT]  , 
			CASE WHEN SUBSTRING(remit.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(remit.[G-WARD-CD],2,1) 
			     WHEN remit.[G-WARD-CD] = '' THEN NULL
				 ELSE remit.[G-WARD-CD] END as [REMITTANCE_WARD]
		INTO #UpdateRemitAddr
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID  
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt remit ON remit.[P-SYS-ID] = map.[SysID] AND remit.[P-ADR-TY-CD] = 'R' 
		JOIN #IncludeList il ON remit.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND remit.[EnableConversion] = 1;

		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [REMITTANCE_ADDRESS1] = ' + CASE WHEN [REMITTANCE_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS1],'''','''''') ) + '''' END
			+ ', [REMITTANCE_ADDRESS2] = ' + CASE WHEN [REMITTANCE_ADDRESS2] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS2],'''','''''') ) + '''' END
			+ ', [REMITTANCE_CITY] = ' + CASE WHEN [REMITTANCE_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_CITY],'''','''''') ) + '''' END 
			+ ', [REMITTANCE_STATE] = ' + CASE WHEN [REMITTANCE_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_STATE],'''','''''') ) + '''' END
			+ ', [REMITTANCE_ZIP] = ' + CASE WHEN [REMITTANCE_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ZIP],'''','''''') ) + '''' END
			+ ', [REMITTANCE_EXT_ZIP] = ' + CASE WHEN [REMITTANCE_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [REMITTANCE_PHONE_NUMBER] = ' + CASE WHEN [REMITTANCE_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [REMITTANCE_CONTACT_NAME] = ' + CASE WHEN [REMITTANCE_CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_CONTACT_NAME],'''','''''') ) + '''' END
			+ ', [REMITTANCE_CONTACT_TYPE] = ' + CASE WHEN [REMITTANCE_CONTACT_TYPE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_CONTACT_TYPE],'''','''''') ) + '''' END			   
			+ ', [REMITTANCE_EMAIL] = ' + CASE WHEN [REMITTANCE_EMAIL] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_EMAIL],'''','''''') ) + '''' END
			+ ', [REMITTANCE_FAX_NUMBER] = ' + CASE WHEN [REMITTANCE_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_FAX_NUMBER],'''','''''') ) + '''' END 	
			+ ', [REMITTANCE_ORGANIZATION_NAME] = ' + CASE WHEN [REMITTANCE_ORGANIZATION_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ORGANIZATION_NAME],'''','''''') ) + '''' END			   
			+ ', [REMITTANCE_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [REMITTANCE_ADDRESS_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END  			   
			+ ', [REMITTANCE_ADDRESS_FIRSTNAME] = ' + CASE WHEN [REMITTANCE_ADDRESS_FIRSTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS_FIRSTNAME],'''','''''') ) + '''' END
			+ ', [REMITTANCE_ADDRESS_MIDDLENAME] = ' + CASE WHEN [REMITTANCE_ADDRESS_MIDDLENAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS_MIDDLENAME],'''','''''') ) + '''' END
			+ ', [REMITTANCE_ADDRESS_LASTNAME] = ' + CASE WHEN [REMITTANCE_ADDRESS_LASTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS_LASTNAME],'''','''''') ) + '''' END
			+ ', [REMITTANCE_ADDRESS_TITLE] = ' + CASE WHEN [REMITTANCE_ADDRESS_TITLE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_ADDRESS_TITLE],'''','''''') ) + '''' END 
			+ ', [REMITTANCE_QUADRANT] = ' + CASE WHEN [REMITTANCE_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_QUADRANT],'''','''''') ) + '''' END
			+ ', [REMITTANCE_WARD] = ' + CASE WHEN [REMITTANCE_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([REMITTANCE_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #UpdateRemitAddr

		-- Other addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [OTHER_ADDRESS1] = oth.[P-LINE1-AD] ,
		--	[OTHER_ADDRESS2] = oth.[P-LINE2-AD] ,
		--	[OTHER_CITY] = oth.[P-CITY-NAM] , 
		--	[OTHER_STATE] = oth.[P-ST-CD] ,
		--	[OTHER_ZIP] = oth.[P-ZIP5-CD] ,
		--	[OTHER_EXT_ZIP] = oth.[P-ZIP4-CD] ,
		--	[OTHER_PHONE_NUMBER] = oth.[P-CONTCT-PHON-NUM] ,
		--	[OTHER_CONTACT_NAME] = oth.[P-CONTCT-NAM] ,
		--	[OTHER_CONTACT_TYPE] = CASE oth.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,			   
		--	[OTHER_EMAIL] = RTRIM(oth.[P-CONTCT-EMAIL-AD-TEXT]) ,
		--	[OTHER_FAX_NUMBER] = oth.[P-FAX-NUM] ,	
		--	[OTHER_ORGANIZATION_NAME] = CASE oth.[P-NAM-ORG-IND] WHEN 'Y' THEN oth.[P-NAM] ELSE NULL END ,			   
		--	[OTHER_ADDRESS_PHONE_NUMBER] = oth.[P-PHON-NUM] ,  			   
		--	[OTHER_ADDRESS_FIRSTNAME] = CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-FST-NAM] ELSE NULL END ,
		--	[OTHER_ADDRESS_MIDDLENAME] = CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-MI-NAM] ELSE NULL END ,
		--	[OTHER_ADDRESS_LASTNAME] = CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-LAST-NAM] ELSE NULL END ,
		--	[OTHER_ADDRESS_TITLE] = CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-SFX-NAM] ELSE NULL END ,
		--	[OTHER_QUADRANT] = CASE WHEN oth.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN oth.[G-QUAD-CD] ELSE NULL END ,
		--	[OTHER_WARD] = CASE WHEN SUBSTRING(oth.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(oth.[G-WARD-CD],2,1) 
		--			   WHEN oth.[G-WARD-CD] = '' THEN NULL
		--			   ELSE oth.[G-WARD-CD] END  	
		SELECT reg.REG_ID,
			oth.[P-LINE1-AD] as [OTHER_ADDRESS1]  ,
			oth.[P-LINE2-AD] as [OTHER_ADDRESS2]  ,
			oth.[P-CITY-NAM] as [OTHER_CITY]  , 
			oth.[P-ST-CD] as [OTHER_STATE]  ,
			oth.[P-ZIP5-CD] as [OTHER_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(oth.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(oth.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				oth.[P-ZIP4-CD]	
			END as [OTHER_EXT_ZIP]  ,
			oth.[P-CONTCT-PHON-NUM] as [OTHER_PHONE_NUMBER]  ,
			oth.[P-CONTCT-NAM] as [OTHER_CONTACT_NAME]  ,
			CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [OTHER_CONTACT_TYPE]  ,			   
			RTRIM(oth.[P-CONTCT-EMAIL-AD-TEXT]) as [OTHER_EMAIL]  ,
			oth.[P-FAX-NUM] as [OTHER_FAX_NUMBER]  ,	
			CASE oth.[P-NAM-ORG-IND] WHEN 'Y' THEN oth.[P-NAM] ELSE NULL END as [OTHER_ORGANIZATION_NAME]  ,			   
			oth.[P-PHON-NUM] as [OTHER_ADDRESS_PHONE_NUMBER]  ,  			   
			CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-FST-NAM] ELSE NULL END as [OTHER_ADDRESS_FIRSTNAME]  ,
			CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-MI-NAM] ELSE NULL END as [OTHER_ADDRESS_MIDDLENAME]  ,
			CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-LAST-NAM] ELSE NULL END as [OTHER_ADDRESS_LASTNAME]  ,
			CASE oth.[P-NAM-ORG-IND] WHEN 'N' THEN oth.[P-SFX-NAM] ELSE NULL END as [OTHER_ADDRESS_TITLE]  ,
			CASE WHEN oth.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN oth.[G-QUAD-CD] ELSE NULL END as [OTHER_QUADRANT]  ,
			CASE WHEN SUBSTRING(oth.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(oth.[G-WARD-CD],2,1) 
				 WHEN oth.[G-WARD-CD] = '' THEN NULL
				 ELSE oth.[G-WARD-CD] END as [OTHER_WARD]
		INTO #UpdateOthAddr
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt oth ON oth.[P-SYS-ID] = map.[SysID] AND oth.[P-ADR-TY-CD] = 'O'
		JOIN #IncludeList il ON oth.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND oth.[EnableConversion] = 1;


		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [OTHER_ADDRESS1] = ' + CASE WHEN [OTHER_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS1],'''','''''') ) + '''' END
			+ ', [OTHER_ADDRESS2] = ' + CASE WHEN [OTHER_ADDRESS2] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS2],'''','''''') ) + '''' END
			+ ', [OTHER_CITY] = ' + CASE WHEN [OTHER_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_CITY],'''','''''') ) + '''' END
			+ ', [OTHER_STATE] = ' + CASE WHEN [OTHER_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_STATE],'''','''''') ) + '''' END
			+ ', [OTHER_ZIP] = ' + CASE WHEN [OTHER_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ZIP],'''','''''') ) + '''' END
			+ ', [OTHER_EXT_ZIP] = ' + CASE WHEN [OTHER_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [OTHER_PHONE_NUMBER] = ' + CASE WHEN [OTHER_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [OTHER_CONTACT_NAME] = ' + CASE WHEN [OTHER_CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_CONTACT_NAME],'''','''''') ) + '''' END
			+ ', [OTHER_CONTACT_TYPE] = ' + CASE WHEN [OTHER_CONTACT_TYPE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_CONTACT_TYPE],'''','''''') ) + '''' END			   
			+ ', [OTHER_EMAIL] = ' + CASE WHEN [OTHER_EMAIL] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_EMAIL],'''','''''') ) + '''' END
			+ ', [OTHER_FAX_NUMBER] = ' + CASE WHEN [OTHER_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_FAX_NUMBER],'''','''''') ) + '''' END	
			+ ', [OTHER_ORGANIZATION_NAME] = ' + CASE WHEN [OTHER_ORGANIZATION_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ORGANIZATION_NAME],'''','''''') ) + '''' END			   
			+ ', [OTHER_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [OTHER_ADDRESS_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END  			   
			+ ', [OTHER_ADDRESS_FIRSTNAME] = ' + CASE WHEN [OTHER_ADDRESS_FIRSTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS_FIRSTNAME],'''','''''') ) + '''' END
			+ ', [OTHER_ADDRESS_MIDDLENAME] = ' + CASE WHEN [OTHER_ADDRESS_MIDDLENAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS_MIDDLENAME],'''','''''') ) + '''' END
			+ ', [OTHER_ADDRESS_LASTNAME] = ' + CASE WHEN [OTHER_ADDRESS_LASTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS_LASTNAME],'''','''''') ) + '''' END
			+ ', [OTHER_ADDRESS_TITLE] = ' + CASE WHEN [OTHER_ADDRESS_TITLE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_ADDRESS_TITLE],'''','''''') ) + '''' END
			+ ', [OTHER_QUADRANT] = ' + CASE WHEN [OTHER_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_QUADRANT],'''','''''') ) + '''' END
			+ ', [OTHER_WARD] = ' + CASE WHEN [OTHER_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([OTHER_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #UpdateOthAddr

		-- W9 addr
		--UPDATE REG_SERVICE_LOCATION
		--SET [W9_ADDRESS1] = CASE WHEN w9.[P-LINE1-AD] = '' THEN NULL ELSE w9.[P-LINE1-AD] END ,
		--	[W9_ADDRESS2] = CASE WHEN w9.[P-LINE2-AD] = '' THEN NULL ELSE w9.[P-LINE2-AD] END  ,
		--	[W9_CITY] = CASE WHEN w9.[P-CITY-NAM]  = '' THEN NULL ELSE w9.[P-CITY-NAM] END , 
		--	[W9_STATE] = CASE WHEN w9.[P-ST-CD]  = '' THEN NULL ELSE w9.[P-ST-CD] END ,
		--	[W9_ZIP] = CASE WHEN w9.[P-ZIP5-CD]  = '' THEN NULL ELSE w9.[P-ZIP5-CD] END ,
		--	[W9_EXT_ZIP] = CASE WHEN w9.[P-ZIP4-CD]  = '' THEN NULL ELSE w9.[P-ZIP4-CD] END ,
		--	[W9_PHONE_NUMBER] = CASE WHEN w9.[P-CONTCT-PHON-NUM]  = '' THEN NULL ELSE w9.[P-CONTCT-PHON-NUM] END ,
		--	[W9_CONTACT_NAME] = w9.[P-CONTCT-NAM] ,    
		--	[W9_CONTACT_TYPE] = CASE w9.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END ,			   
		--	[W9_FAX_NUMBER] = CASE WHEN w9.[P-FAX-NUM] = '' THEN NULL ELSE w9.[P-FAX-NUM]  END , 
		--	[W9_ORGANIZATION_NAME] = CASE w9.[P-NAM-ORG-IND] WHEN 'Y' THEN w9.[P-NAM] ELSE NULL END ,
		--	[W9_ADDRESS_PHONE_NUMBER] = w9.[P-PHON-NUM] ,  				
		--	[W9_ADDRESS_FIRSTNAME] = CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-FST-NAM] ELSE NULL END ,  
		--	[W9_ADDRESS_MIDDLENAME] = CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-MI-NAM] ELSE NULL END ,
		--	[W9_ADDRESS_LASTNAME] = CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-LAST-NAM] ELSE NULL END ,
		--	[W9_ADDRESS_TITLE] = CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-SFX-NAM] ELSE NULL END ,
		--	[W9_QUADRANT] = CASE WHEN w9.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN w9.[G-QUAD-CD] ELSE NULL END , 
		--	[W9_WARD] = CASE WHEN SUBSTRING(w9.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(w9.[G-WARD-CD],2,1) 
		--			   WHEN w9.[G-WARD-CD] = '' THEN NULL
		--			   ELSE w9.[G-WARD-CD] END  	
		SELECT reg.REG_ID, 
		    CASE WHEN w9.[P-LINE1-AD] = '' THEN NULL ELSE w9.[P-LINE1-AD] END as [W9_ADDRESS1]  ,
			CASE WHEN w9.[P-LINE2-AD] = '' THEN NULL ELSE w9.[P-LINE2-AD] END as [W9_ADDRESS2]  ,
			CASE WHEN w9.[P-CITY-NAM]  = '' THEN NULL ELSE w9.[P-CITY-NAM] END as [W9_CITY]  , 
			CASE WHEN w9.[P-ST-CD]  = '' THEN NULL ELSE w9.[P-ST-CD] END as [W9_STATE]  ,
			CASE WHEN w9.[P-ZIP5-CD]  = '' THEN NULL ELSE w9.[P-ZIP5-CD] END as [W9_ZIP]  ,
			CASE WHEN LEN(RTRIM(LTRIM(ISNULL(w9.[P-ZIP5-CD], '')))) > 0 AND
			LEN(RTRIM(LTRIM(ISNULL(w9.[P-ZIP4-CD], '')))) = 0 THEN
				'0000'
			ELSE
				w9.[P-ZIP4-CD]	
			END as [W9_EXT_ZIP]  ,
			CASE WHEN w9.[P-CONTCT-PHON-NUM]  = '' THEN NULL ELSE w9.[P-CONTCT-PHON-NUM] END as [W9_PHONE_NUMBER]  ,
			w9.[P-CONTCT-NAM] as [W9_CONTACT_NAME]  ,    
			CASE w9.[P-NAM-ORG-IND]	WHEN 'N' THEN 'Individual' WHEN 'Y' THEN 'Organization' ELSE NULL END as [W9_CONTACT_TYPE]  ,			   
			CASE WHEN w9.[P-FAX-NUM] = '' THEN NULL ELSE w9.[P-FAX-NUM]  END as [W9_FAX_NUMBER]  , 
			CASE w9.[P-NAM-ORG-IND] WHEN 'Y' THEN w9.[P-NAM] ELSE NULL END as [W9_ORGANIZATION_NAME]  ,
			w9.[P-PHON-NUM] as [W9_ADDRESS_PHONE_NUMBER]  ,  				
			CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-FST-NAM] ELSE NULL END as [W9_ADDRESS_FIRSTNAME]  ,  
			CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-MI-NAM] ELSE NULL END as [W9_ADDRESS_MIDDLENAME]  ,
			CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-LAST-NAM] ELSE NULL END as [W9_ADDRESS_LASTNAME]  ,
			CASE w9.[P-NAM-ORG-IND] WHEN 'N' THEN w9.[P-SFX-NAM] ELSE NULL END as [W9_ADDRESS_TITLE]  ,
			CASE WHEN w9.[G-QUAD-CD] in ('NW','NE','SE','SW') THEN w9.[G-QUAD-CD] ELSE NULL END as [W9_QUADRANT]  , 
			CASE WHEN SUBSTRING(w9.[G-WARD-CD],1,1) = '0' THEN SUBSTRING(w9.[G-WARD-CD],2,1) 
				 WHEN w9.[G-WARD-CD] = '' THEN NULL
				 ELSE w9.[G-WARD-CD] END  as [W9_WARD]
		INTO #Update_W9Addr
		FROM REG_SERVICE_LOCATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.[SysID]
		JOIN SRC_ProviderAddressUpdt w9 ON w9.[P-SYS-ID] = map.[SysID] AND w9.[P-ADR-TY-CD] = 'W' 
		JOIN #IncludeList il ON w9.[P-SYS-ID] = il.[P-SYS-ID] 		
		WHERE prov.[EnableConversion] = 1 AND w9.[EnableConversion] = 1;


		INSERT INTO #Commands (Command)
		SELECT 'UPDATE REG_SERVICE_LOCATION SET [W9_ADDRESS1] = ' + CASE WHEN [W9_ADDRESS1] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS1],'''','''''') ) + '''' END
		 	+ ', [W9_ADDRESS2] = ' + CASE WHEN [W9_ADDRESS2] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS2],'''','''''') ) + '''' END
			+ ', [W9_CITY] = ' + CASE WHEN [W9_CITY] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_CITY],'''','''''') ) + '''' END 
			+ ', [W9_STATE] = ' + CASE WHEN [W9_STATE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_STATE],'''','''''') ) + '''' END
			+ ', [W9_ZIP] = ' + CASE WHEN [W9_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ZIP],'''','''''') ) + '''' END
			+ ', [W9_EXT_ZIP] = ' + CASE WHEN [W9_EXT_ZIP] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_EXT_ZIP],'''','''''') ) + '''' END
			+ ', [W9_PHONE_NUMBER] = ' + CASE WHEN [W9_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_PHONE_NUMBER],'''','''''') ) + '''' END
			+ ', [W9_CONTACT_NAME] = ' + CASE WHEN [W9_CONTACT_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_CONTACT_NAME],'''','''''') ) + '''' END    
			+ ', [W9_CONTACT_TYPE] = ' + CASE WHEN [W9_CONTACT_TYPE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_CONTACT_TYPE],'''','''''') ) + '''' END		   
			+ ', [W9_FAX_NUMBER] = ' + CASE WHEN [W9_FAX_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_FAX_NUMBER],'''','''''') ) + '''' END 
			+ ', [W9_ORGANIZATION_NAME] = ' + CASE WHEN [W9_ORGANIZATION_NAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ORGANIZATION_NAME],'''','''''') ) + '''' END
			+ ', [W9_ADDRESS_PHONE_NUMBER] = ' + CASE WHEN [W9_ADDRESS_PHONE_NUMBER] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS_PHONE_NUMBER],'''','''''') ) + '''' END 				
			+ ', [W9_ADDRESS_FIRSTNAME] = ' + CASE WHEN [W9_ADDRESS_FIRSTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS_FIRSTNAME],'''','''''') ) + '''' END 
			+ ', [W9_ADDRESS_MIDDLENAME] = ' + CASE WHEN [W9_ADDRESS_MIDDLENAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS_MIDDLENAME],'''','''''') ) + '''' END
			+ ', [W9_ADDRESS_LASTNAME] = ' + CASE WHEN [W9_ADDRESS_LASTNAME] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS_LASTNAME],'''','''''') ) + '''' END
			+ ', [W9_ADDRESS_TITLE] = ' + CASE WHEN [W9_ADDRESS_TITLE] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_ADDRESS_TITLE],'''','''''') ) + '''' END
			+ ', [W9_QUADRANT] = ' + CASE WHEN [W9_QUADRANT] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_QUADRANT],'''','''''') ) + '''' END 
			+ ', [W9_WARD] = ' + CASE WHEN [W9_WARD] IS NULL THEN 'NULL' ELSE '''' + RTRIM(REPLACE([W9_WARD],'''','''''') ) + '''' END
			+ ', [LAST_MODIFIED_DATE_TIME] = ''' +  convert(varchar,@pin_conv_run_time,120) 
			+ ''', [LAST_MODIFIED_USER] = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
			+ ''' WHERE REG_ID = ' + convert(varchar,REG_ID) + ';' as Command
		FROM #Update_W9Addr



		-- REG_ADDITIONAL_ADDRESSES?  -- Skip this one



		INSERT #Commands (Command)
		SELECT 'COMMIT' as Command;

		INSERT #Commands (Command)
		SELECT 'END TRY' as Command;

		INSERT #Commands (Command)
		SELECT 'BEGIN CATCH' as Command;

		INSERT #Commands (Command)
		SELECT 'PRINT ERROR_MESSAGE();';

		INSERT #Commands (Command)
		SELECT 'ROLLBACK' as Command;

		INSERT #Commands (Command)
		SELECT 'END CATCH' as Command;

		-- Log the load process
		INSERT INTO SRC_ProviderAddressTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max(CONCAT([G-AUD-DT],' ',[G-AUD-TM])) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_ProviderAddressUpdt;

		SELECT Command FROM #Commands order by CmdID ;


		COMMIT ;
	END TRY
	BEGIN CATCH
		ROLLBACK ;
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
        (@pin_conv_run_id,'SRC_ProviderLicenseUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO