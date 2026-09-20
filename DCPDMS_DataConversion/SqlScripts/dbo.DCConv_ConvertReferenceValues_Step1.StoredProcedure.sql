/****** Object:  StoredProcedure [dbo].[DCConv_ConvertReferenceValues_Step1]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertReferenceValues_Step1]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertReferenceValues_Step1]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Richard Mays
-- Create date: 5/18/2016
-- Description:	Converts and/or moves the DC reference values to PDMS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertReferenceValues_Step1]
	-- Add the parameters for the stored procedure here
	@pin_run_id VARCHAR(10),
	@pin_run_reference_time DATETIME,
	@pin_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	-- convert all of the reference data
	--EXEC DCConv_LkUp_PopulateAPPLICATION_CATEGORY_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateAPPLICATION_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCERTIFICATION_ACTION_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCERTIFICATION_ELIGIBILITY_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCERTIFICATION_LTC_BED_BREAKDOWN_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCERTIFIED_BEDS @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCITIZENSHIP_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateCOUNTY @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateDEFENDENT_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateDEGREE_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateINSURANCE_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulatePROVIDER_CATEGORY_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulatePROVIDER_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateORGANIZATION_TYPE @pin_run_reference_time  -- moved from following OWNER_CATEGORY_TYPE to following PROVIDER_TYPE
	--EXEC DCConv_LkUp_PopulatePROVIDER_RISKLEVEL_MAPPING @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulatePROVIDER_RISK_LEVEL @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulatePROVTYPE_ORGTYPE_MAPPING @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateREG_PAGE_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateREG_PAGE_SETTING @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateREG_PAGE_SETTING_ACTION @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulateSCREENING_ACTIVITY_DEFAULT_TEMPLATE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateSCREENING_ACTIVITY_OWNER_TEMPLATE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateSCREENING_ACTIVITY_RISK_LEVEL_TEMPLATE @pin_run_reference_time
	
	EXEC DCConv_LkUp_PopulateDOCUMENT @pin_run_reference_time
	EXEC DCConv_LkUp_PopulateREG_SECTION_UPLOAD_CONTROL @pin_run_reference_time
	
	--EXEC DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE_PROVIDER_TYPE_XREF @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateENROLLMENT_STATUS_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateTYPE_OF_OWNERSHIP @pin_run_reference_time  
	--EXEC DCConv_LkUp_PopulateTYPE_OF_PRACTICE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulatePROVIDER_TYPE_FEE @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulateLICENSE_TYPE @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulateTAX_ENTITY_TYPE @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateWF_PROCESS @pin_run_reference_time
	--EXEC DCConv_LkUp_PopulateWF_STEP @pin_run_reference_time	
	--EXEC DCConv_LkUp_PopulateEMAIL @pin_run_reference_time

	DELETE FROM dbo.WF_PARAMETER WHERE PROCESS_ID IS NOT NULL;
		
	

	-- populate the specialty type reference values
	DELETE FROM SPECIALTY_TYPE_SPECIALTY_CATEGORY_XREF;
	DELETE FROM dbo.SPECIALTY_EMPLOYEE_TYPE;

	--DELETE
	--FROM dbo.SPECIALTY_TYPE;


	--DBCC CHECKIDENT (
	--		'dbo.SPECIALTY_TYPE'
	--		,RESEED
	--		,0
	--		);

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
	FROM SRC_LkUpSpecialtyType spec
	LEFT JOIN [dbo].[SPECIALTY_TYPE] st 
	ON spec.[P-SPECL-CD] = st.[MMIS_SPECIALTY_TYPE_ID]
	WHERE st.[MMIS_SPECIALTY_TYPE_ID] IS NULL;

	
	-- Additional specialties from Ben
	-- retired 12/15/16
	--DECLARE @MMIS_SPECIALTY_TYPE_ID varchar(10) = NULL,
	--	@SPECIALTY_TYPE_NAME varchar(256) = NULL,
	--	@SPECIALTY_TYPE_ID int = NULL,
	--	@MMIS_PROVIDER_TYPE_ID varchar(10) = NULL,
	--	@DateTimeNow datetime = GETDATE(),
	--	@AdminUser uniqueidentifier = '5C8EEA1D-B312-41A7-999C-DEDBCCA3688D';

	--SET @MMIS_PROVIDER_TYPE_ID = 'W02'

	/* Create new records for EPD specialties with text not found in SPECIALTY_TYPE */
	--SET @MMIS_SPECIALTY_TYPE_ID = '715';
	--SET @SPECIALTY_TYPE_NAME = 'Personal Care Aid'

	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME 
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	--SET @MMIS_SPECIALTY_TYPE_ID = '710';
	--SET @SPECIALTY_TYPE_NAME = 'Personal Emergency Response Systems (PERS)'

	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	--SET @MMIS_SPECIALTY_TYPE_ID = '709';
	--SET @SPECIALTY_TYPE_NAME = 'Environmental Adaptation Accessiblity (EAA)'
	
	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	--SET @MMIS_SPECIALTY_TYPE_ID = '742';
	--SET @SPECIALTY_TYPE_NAME = 'Assisted Living Facility'
	
	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	--SET @MMIS_SPECIALTY_TYPE_ID = '703';
	--SET @SPECIALTY_TYPE_NAME = 'Participant Directed Services - Personal Care Aide'
		
	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	--SET @MMIS_SPECIALTY_TYPE_ID = '739';
	--SET @SPECIALTY_TYPE_NAME = 'Participant Directed Services - Goods & Services'
	
	--UPDATE SPECIALTY_TYPE
	--SET SPECIALTY_TYPE_NAME = @SPECIALTY_TYPE_NAME
	--WHERE MMIS_SPECIALTY_TYPE_ID = @MMIS_SPECIALTY_TYPE_ID


	EXEC DCConv_LkUp_PopulateSPECIALTY_TYPE_SPECIALTY_CATEGORY_XREF @pin_run_reference_time;
	EXEC DCConv_LkUp_PopulateSPECIALTY_EMPLOYEE_TYPE @pin_run_reference_time;
	
	-- Some custom cleanup
	-- retired 12/15/16
	--update REG_PAGE_SETTING 
	--set REG_PAGE_NAME = 'DME' 
	--where REG_PAGE_NAME = '???'

	-- Make one of the two REG_PAGE_SETTING into GRoup and facility affilaition instead of Invididual providers for gorups
	-- retired 12/15/16
	--update REG_PAGE_SETTING 
	--set REG_PAGE_NAME = 'Group And Facility Affiliation' 
	--where REG_PAGE_SETTING_ID in (
	--	select REG_PAGE_SETTING_ID 
	--	from REG_PAGE_SETTING 
	--	WHERE REG_PAGE_NAME LIKE '%Individual%' 
	--	and PROVIDER_TYPE_ID IN (
	--		select PROVIDER_TYPE_ID 
	--		from PROVIDER_TYPE 
	--		WHERE PROVIDER_CATEGORY_TYPE_ID = 2	) 
	--	and IS_VISIBLE = 0 
	--	and REG_PAGE_SETTING_ID NOT IN ( 23446, 23487, 23569));


	-- Make one of the two REG_PAGE_SETTING into GRoup and facility affilaition instead of Invididual providers for PROVIDER_CATEGORY_TYPE_ID = 3
	-- retired 12/15/16
	--with CTE AS
	--(select *, ROW_NUMBER() OVER(PARTITION BY ENTITY_TYPE_ID, PROVIDER_TYPE_ID ORDER BY REG_PAGE_SETTING_ID DESC) AS RowNumber
	-- from REG_PAGE_SETTING 
	-- WHERE REG_PAGE_NAME LIKE '%Individual%' 
	-- and PROVIDER_TYPE_ID IN (
	--	select PROVIDER_TYPE_ID from PROVIDER_TYPE WHERE PROVIDER_CATEGORY_TYPE_ID = 3 ))
	--update REG_PAGE_SETTING 
	--set REG_PAGE_NAME = 'Group And Facility Affiliation' 
	--where REG_PAGE_SETTING_ID in (
	--	select REG_PAGE_SETTING_ID from CTE where RowNumber = 2);


	-- Make one of the two REG_PAGE_SETTING into GRoup and facility affilaition instead of Invididual providers for PROVIDER_CATEGORY_TYPE_ID = 5
	-- retired 12/15/16
	--with CTE AS
	--(select *, ROW_NUMBER() OVER(PARTITION BY ENTITY_TYPE_ID, PROVIDER_TYPE_ID ORDER BY REG_PAGE_SETTING_ID DESC) AS RowNumber
	-- from REG_PAGE_SETTING 
	-- WHERE REG_PAGE_NAME LIKE '%Individual%' 
	-- and PROVIDER_TYPE_ID IN (
	--	select PROVIDER_TYPE_ID from PROVIDER_TYPE WHERE PROVIDER_CATEGORY_TYPE_ID = 5 )) 
	--update REG_PAGE_SETTING 
	--set REG_PAGE_NAME = 'Group And Facility Affiliation' 
	--where REG_PAGE_SETTING_ID in (
	--	select REG_PAGE_SETTING_ID from CTE where RowNumber = 2	)
	

END


GO
