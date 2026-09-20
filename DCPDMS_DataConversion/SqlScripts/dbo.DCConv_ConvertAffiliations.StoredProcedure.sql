/****** Object:  StoredProcedure [dbo].[DCConv_ConvertAffiliations]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertAffiliations]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertAffiliations]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 6/7/2016
-- Description:	Converts the DC affiliations to PDMS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertAffiliations] 
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM dbo.[REG_AFFILIATION];
	
	DBCC CHECKIDENT ('REG_AFFILIATION');  

	INSERT INTO [dbo].[REG_AFFILIATION]
           ([REG_ID]
           ,[NAME]
           ,[NPI]
           ,[SSN]
           ,[TAXONOMY_TYPE_ID]
           ,[GROUP_AFFILIATION_STATUS_ID]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[START_DATE]
           ,[END_DATE]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[INVALID_FLAG]
           ,[MEDICAID_ID]
           ,[LICENSURE_ID]
           ,[FIRST_NAME]
           ,[LAST_NAME]
           ,[RETRO_REVIEW_REQUIRED_ID]
           ,[PARTY_ID]
           ,[NPI_START_DATE]
           ,[NPI_END_DATE]
           ,[PROVIDER_TYPE_ID]
		   --,[SENT_TO_MMIS]  -- Pending modification to table
		   )
     SELECT DISTINCT
		-- get the registration ID of the most recent enrollment that occurred before the start date of the affiliation
			ISNULL((SELECT keyTable.[RegistrationId]
			FROM DCConv_KeyCrossReferences keyTable 
			WHERE keyTable.[SysID] = affil.[P-GROUP-SYS-ID]),0) AS [REG_ID], -- group reg id
           CASE WHEN LEN(RTRIM(LTRIM(grpMember.[P-NAM]))) = 0 THEN RTRIM(LTRIM(grpMember.[P-DBA-NAM])) ELSE RTRIM(LTRIM(grpMember.[P-NAM])) END AS [NAME],
           grpMember.[P-NPI-NUM] AS NPI, 
           grpMember.[P-SSN-NUM] AS SSN,
           NULL AS [TAXONOMY_TYPE_ID],
           4 AS GROUP_AFFILIATION_STATUS_ID,
           1 AS MODIFIED_STATUS_TYPE_ID, 
           dbo.fn_ConvertDCDateToPDMS(affil.[P-AFFL-BEG-DT]) AS [START_DATE],
           dbo.fn_ConvertDCDateToPDMS(affil.[P-AFFL-END-DT]) AS [END_DATE],
		   @pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           0 AS INVALID_FLAG, 
		   grpMember.[P-ID] AS [MEDICAID_ID], 
           NULL AS [LICENSURE_ID],
		   CASE WHEN LEN(RTRIM(LTRIM(grpMember.[P-FST-NAM]))) = 0 THEN grpMember.[P-DBA-FST-NAM] ELSE grpMember.[P-FST-NAM] END AS [FIRST_NAME],
		   CASE WHEN LEN(RTRIM(LTRIM(grpMember.[P-LAST-NAM]))) = 0 THEN grpMember.[P-DBA-LAST-NAM] ELSE grpMember.[P-LAST-NAM] END AS [LAST_NAME],
           NULL AS RETRO_REVIEW_REQUIRED_ID, 
           NULL AS PARTY_ID, 
           NULL AS NPI_START_DATE, 
           NULL AS NPI_END_DATE, 
           NULL AS PROVIDER_TYPE_ID
		   --, 1 as SENT_TO_MMIS  -- Pending modification to table
		FROM SRC_ProviderAffiliates affil
		INNER JOIN SRC_Providers grp ON grp.[P-SYS-ID] = affil.[P-GROUP-SYS-ID]
		INNER JOIN SRC_Enrollments grpEn ON grpEn.[P-SYS-ID] = affil.[P-GROUP-SYS-ID]
		INNER JOIN SRC_Providers grpMember ON grpMember.[P-SYS-ID] = affil.[P-MEMBER-SYS-ID]
		WHERE grp.[P-REC-TY-CD] = 'P' AND grpEn.[P-ENROL-STAT-TY-CD] = '00' AND 
		grpEn.[P-STAT-END-DT] IS NOT NULL AND 
		--dbo.fn_ConvertDCDateToPDMS(grpEn.[P-STAT-END-DT])  > @pin_conv_data_export_date​ AND
		--dbo.fn_ConvertDCDateToPDMS(affil.[P-AFFL-END-DT]) > @pin_conv_data_export_date AND
		grpMember.[P-REC-TY-CD] = 'P' AND dbo.fn_ConvertDCDateToPDMS(affil.[P-AFFL-END-DT]) > getdate();

		-- Remove invalid records
		select distinct MEDICAID_ID
		into #drops
		from REG_SERVICE_LOCATION a
		join (select distinct REG_ID
			  from REG_PROVIDER 
			  where [ENROLLMENT_STATUS_CODE] <> '00'
			  ) b on a.REG_ID = b.REG_ID;

		delete from REG_AFFILIATION where MEDICAID_ID in (select distinct MEDICAID_ID from #drops);

		
END


GO
