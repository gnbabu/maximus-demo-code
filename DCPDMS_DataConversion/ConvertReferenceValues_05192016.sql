-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/18/2016
-- Description:	Converts and/or moves the DC reference values to PDMS
-- =============================================
ALTER PROCEDURE DCConv_ConvertReferenceValues 
	-- Add the parameters for the stored procedure here
	@pin_run_id varchar(10), 
	@pin_run_reference_time datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all"

	-- populate the provider type reference values
	DELETE FROM dbo.PROVIDER_TYPE;
	DBCC CHECKIDENT ('dbo.PROVIDER_TYPE', RESEED, 0);

	-- replace the PDMS reference values with those from DC
	INSERT INTO [dbo].[PROVIDER_TYPE]
           ([PROVIDER_TYPE_ABBREVIATION]
           ,[PROVIDER_TYPE_NAME]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[IS_USED_IN_MMIS]
           ,[PROVIDER_CATEGORY_TYPE_ID]
           ,[MMIS_PROVIDER_TYPE_ID]
           ,[REQUIRE_NPI]
           ,[PROVIDER_RISK_LEVEL_ID])
     SELECT pty.[Short] AS [PROVIDER_TYPE_ABBREVIATION],
           pty.[Long] AS [PROVIDER_TYPE_NAME],
		   @pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME],
           dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           'Y' AS [IS_USED_IN_MMIS],
           dbo.fn_GetProviderCategoryFromDCProviderType(pty.[P-TY-CD]) AS [PROVIDER_CATEGORY_TYPE_ID],
           pty.[P-TY-CD] AS [MMIS_PROVIDER_TYPE_ID],
           0 AS [REQUIRE_NPI],
           0 AS [PROVIDER_RISK_LEVEL_ID]
	FROM SRC_LkUpProviderType [pty];

	-- populate the specialty type reference values
	DELETE FROM dbo.SPECIALTY_TYPE;
	DBCC CHECKIDENT ('dbo.SPECIALTY_TYPE', RESEED, 0);

	INSERT INTO [dbo].[SPECIALTY_TYPE]
           ([SPECIALTY_TYPE_NAME]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[MMIS_SPECIALTY_TYPE_ID])
     SELECT spec.[Long] AS [SPECIALTY_TYPE_NAME],
	 		@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
            spec.[P-SPECL-CD] AS MMIS_SPECIALTY_TYPE_ID
	FROM SRC_LkUpSpecialtyType spec;

	CREATE TABLE #specialtyCodeMap
	(
		PDMS_ProviderTypeID int,
		MMIS_ProviderTypeCode varchar(3),
		PDMS_SpecialtyTypeID int,
		MMIS_SpecialtyTypeCode varchar(3)
	)

	DELETE FROM dbo.TAXONOMY_TYPE;
	DBCC CHECKIDENT ('dbo.TAXONOMY_TYPE', RESEED, 0);

	INSERT INTO #specialtyCodeMap
		SELECT DISTINCT NULL AS PDMS_ProviderTypeID,
			en.[P-TY-CD] AS MMIS_ProviderTypeCode,
			NULL AS PDMS_SpecialtyTypeID,
			spec.[P-SPECL-CD] AS MMIS_SpecialtyTypeCode
		  FROM SRC_Enrollments en
		  LEFT OUTER JOIN SRC_LkUpProviderType pt ON pt.[P-TY-CD] = en.[P-TY-CD]
		  LEFT OUTER JOIN SRC_ProviderSpecialty spec ON en.[P-SYS-ID] = spec.[P-SYS-ID]
		  LEFT OUTER JOIN SRC_LkUpSpecialtyType sc ON sc.[P-SPECL-CD] = spec.[P-SPECL-CD]
		  WHERE LEN(RTRIM(LTRIM(spec.[P-SPECL-CD]))) > 0 --AND
		  --[P-ENROL-STAT-TY-CD] = '00' AND [P-STAT-END-DT] IS NOT NULL AND 
		  --CASE WHEN ISDATE([P-STAT-END-DT]) = 1 THEN CAST([P-STAT-END-DT] AS datetime) ELSE '1/1/1753' END > '12/31/2015' AND 
		  --CASE WHEN ISDATE(spec.[P-SPECL-END-DT]) = 1 THEN spec.[P-SPECL-END-DT] ELSE '1/1/1753' END > '12/31/2015'
		  ORDER BY en.[P-TY-CD], spec.[P-SPECL-CD];

	UPDATE #specialtyCodeMap SET 
		PDMS_ProviderTypeID = pt.PROVIDER_TYPE_ID,
		PDMS_SpecialtyTypeID = sp.SPECIALTY_TYPE_ID 
	FROM #specialtyCodeMap map 
	INNER JOIN [dbo].[PROVIDER_TYPE] pt ON pt.[MMIS_PROVIDER_TYPE_ID] = map.[MMIS_ProviderTypeCode]
	INNER JOIN [dbo].[SPECIALTY_TYPE] sp ON sp.[MMIS_SPECIALTY_TYPE_ID] = map.[MMIS_SpecialtyTypeCode]
	
	INSERT INTO [dbo].[TAXONOMY_TYPE]
           ([SPECIALTY_TYPE_ID]
           ,[PROVIDER_TYPE_ID]
           ,[TAXONOMY_CODE]
           ,[TAXONOMY_NAME]
           ,[EXPIRATION_DATE]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[MMIS_SPECIALTY_TYPE_ID]
           ,[NPI_REQUIRED])
     SELECT map.PDMS_SpecialtyTypeID AS [SPECIALTY_TYPE_ID],
           map.PDMS_ProviderTypeID AS [PROVIDER_TYPE_ID],
           '' AS [TAXONOMY_CODE], 
           '' AS [TAXONOMY_NAME],
           '12/31/9999' AS [EXPIRATION_DATE], 
	 		@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
           map.MMIS_SpecialtyTypeCode AS [MMIS_SPECIALTY_TYPE_ID], 
           0 AS [NPI_REQUIRED]
	FROM #specialtyCodeMap map WHERE map.PDMS_ProviderTypeID IS NOT NULL;

	DROP TABLE #specialtyCodeMap;

	CREATE TABLE #taxonomyTypeMap
	(
		PDMS_ProviderTypeID int,
		MMIS_ProviderTypeCode varchar(3),
		MMIS_TaxonomyCode varchar(80),
		MMIS_TaxonomyAbbrev varchar(25),
		MMIS_TaxonomyLongDesc varchar(35)
	)

	INSERT INTO #taxonomyTypeMap
	SELECT DISTINCT 
	NULL AS PDMS_ProviderTypeID,
	en.[P-TY-CD] AS MMIS_ProviderTypeCode,
	tax.[P-TAXONOMY-CD] AS MMIS_TaxonomyCode,
	taxHd.[P-TAXON-CLS-DESC] AS MMIS_TaxonomyAbbrev,
	taxHd.[P-TAXON-LONG-DESC] AS MMIS_TaxonomyLongDesc
	FROM SRC_Enrollments en
	INNER JOIN SRC_ProviderTaxonomy tax ON en.[P-SYS-ID] = tax.[P-SYS-ID]
	INNER JOIN SRC_ProviderTaxonomyHeader taxHd ON taxHd.[P-TAXONOMY-CD] = tax.[P-TAXONOMY-CD]
	WHERE LEN(RTRIM(LTRIM(tax.[P-TAXONOMY-CD]))) > 0;

	UPDATE #taxonomyTypeMap SET 
		PDMS_ProviderTypeID = pt.PROVIDER_TYPE_ID
	FROM  #taxonomyTypeMap map 
	INNER JOIN [dbo].[PROVIDER_TYPE] pt ON pt.[MMIS_PROVIDER_TYPE_ID] = map.[MMIS_ProviderTypeCode];


	INSERT INTO [dbo].[TAXONOMY_TYPE]
			   ([SPECIALTY_TYPE_ID]
			   ,[PROVIDER_TYPE_ID]
			   ,[TAXONOMY_CODE]
			   ,[TAXONOMY_NAME]
			   ,[EXPIRATION_DATE]
			   ,[LAST_MODIFIED_DATE_TIME]
			   ,[LAST_MODIFIED_USER]
			   ,[MMIS_SPECIALTY_TYPE_ID]
			   ,[NPI_REQUIRED])
		 SELECT 0 AS	[SPECIALTY_TYPE_ID],
			   map.PDMS_ProviderTypeID AS [PROVIDER_TYPE_ID],
			   map.MMIS_TaxonomyCode AS [TAXONOMY_CODE],
			   map.MMIS_TaxonomyLongDesc AS [TAXONOMY_NAME],
			   '12/31/9999' AS EXPIRATION_DATE, 
	 			@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
			   '' AS [MMIS_SPECIALTY_TYPE_ID],
			   0 AS [NPI_REQUIRED]
		 FROM #taxonomyTypeMap map WHERE map.PDMS_ProviderTypeID IS NOT NULL;

	DROP TABLE #taxonomyTypeMap;

	EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";
END
GO
