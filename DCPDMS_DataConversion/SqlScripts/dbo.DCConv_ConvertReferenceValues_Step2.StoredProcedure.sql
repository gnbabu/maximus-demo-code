/****** Object:  StoredProcedure [dbo].[DCConv_ConvertReferenceValues_Step2]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertReferenceValues_Step2]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertReferenceValues_Step2]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/23/2016
-- Description:	Converts taxonomy and speciality reference values
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_ConvertReferenceValues_Step2] 
	@pin_run_id VARCHAR(10),
	@pin_run_reference_time DATETIME,
	@pin_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	CREATE TABLE #specialtyCodeMap (
		PDMS_ProviderTypeID INT
		,PDMS_SpecialtyTypeID INT
		,MMIS_SpecialtyTypeCode VARCHAR(3)
		);

	--DELETE
	--FROM dbo.TAXONOMY_TYPE;

	--DBCC CHECKIDENT (
	--		'dbo.TAXONOMY_TYPE'
	--		,RESEED
	--		,0
	--		);

	
	WITH spec as 
	(SELECT a.[P-SYS-ID], a.[P-SPECL-CD], b.SPECIALTY_TYPE_ID 
	 FROM SRC_ProviderSpecialty a 
	 JOIN [dbo].[SPECIALTY_TYPE] b on a.[P-SPECL-CD] = b.[MMIS_SPECIALTY_TYPE_ID]) 
	INSERT INTO #specialtyCodeMap (PDMS_ProviderTypeID, PDMS_SpecialtyTypeID, MMIS_SpecialtyTypeCode)
	SELECT DISTINCT prov.PROVIDER_TYPE_ID as PDMS_ProviderTypeID, spec.SPECIALTY_TYPE_ID as PDMS_SpecialtyTypeID, spec.[P-SPECL-CD] AS MMIS_SpecialtyTypeCode
	FROM dbo.DCConv_KeyCrossReferences map 
	JOIN dbo.REG_PROVIDER prov on map.RegistrationID = prov.REG_ID
	JOIN spec ON map.SysID = spec.[P-SYS-ID]
	LEFT JOIN TAXONOMY_TYPE tt on prov.PROVIDER_TYPE_ID = tt.PROVIDER_TYPE_ID and spec.SPECIALTY_TYPE_ID = tt.SPECIALTY_TYPE_ID 
	WHERE LEN(RTRIM(LTRIM(spec.[P-SPECL-CD]))) > 0
	AND tt.SPECIALTY_TYPE_ID IS NULL

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
	SELECT ISNULL(map.PDMS_SpecialtyTypeID,0) AS [SPECIALTY_TYPE_ID]
		,map.PDMS_ProviderTypeID AS [PROVIDER_TYPE_ID]
		,'' AS [TAXONOMY_CODE]
		,'' AS [TAXONOMY_NAME]
		,'12/31/9999' AS [EXPIRATION_DATE]
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,map.MMIS_SpecialtyTypeCode AS [MMIS_SPECIALTY_TYPE_ID]
		,1 AS [NPI_REQUIRED]
	FROM #specialtyCodeMap map
	WHERE map.PDMS_ProviderTypeID IS NOT NULL;

	DROP TABLE #specialtyCodeMap;

	CREATE TABLE #taxonomyTypeMap (
		PDMS_ProviderTypeID INT
		,MMIS_TaxonomyCode VARCHAR(80)
		,MMIS_TaxonomyAbbrev VARCHAR(25)
		,MMIS_TaxonomyLongDesc VARCHAR(35)
	)

	
	INSERT INTO #taxonomyTypeMap
	SELECT DISTINCT prov.PROVIDER_TYPE_ID AS PDMS_ProviderTypeID
		,tax.[P-TAXONOMY-CD] AS MMIS_TaxonomyCode
		,taxHd.[P-TAXON-CLS-DESC] AS MMIS_TaxonomyAbbrev
		,taxHd.[P-TAXON-LONG-DESC] AS MMIS_TaxonomyLongDesc
	FROM DCConv_KeyCrossReferences map
	JOIN dbo.REG_PROVIDER prov ON map.RegistrationID = prov.REG_ID
	INNER JOIN SRC_ProviderTaxonomy tax ON map.SysID = tax.[P-SYS-ID]
	INNER JOIN SRC_ProviderTaxonomyHeader taxHd ON taxHd.[P-TAXONOMY-CD] = tax.[P-TAXONOMY-CD]
	LEFT JOIN TAXONOMY_TYPE tt on prov.PROVIDER_TYPE_ID = tt.PROVIDER_TYPE_ID and tax.[P-TAXONOMY-CD] = tt.TAXONOMY_CODE 
	WHERE LEN(RTRIM(LTRIM(tax.[P-TAXONOMY-CD]))) > 0
	AND tt.TAXONOMY_CODE IS NULL;

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
	
	-- Additional Taxonomy Types
	-- retired 12/15/16
	--EXEC DCConv_Add_TAXONOMY_TYPE @pin_run_reference_time;

	
			
	-- When the name is empty, use the [P-TAXON-CLS-DESC] source column
	WITH src AS (
	SELECT DISTINCT tax.[P-TAXONOMY-CD], taxHd.[P-TAXON-LONG-DESC], taxHd.[P-TAXON-CLS-DESC]
	FROM SRC_ProviderTaxonomy tax 
	INNER JOIN SRC_ProviderTaxonomyHeader taxHd ON taxHd.[P-TAXONOMY-CD] = tax.[P-TAXONOMY-CD]
	)
	UPDATE TAXONOMY_TYPE
	SET TAXONOMY_NAME = src.[P-TAXON-CLS-DESC]
	FROM TAXONOMY_TYPE a
	JOIN src on a.TAXONOMY_CODE = src.[P-TAXONOMY-CD]
	WHERE a.SPECIALTY_TYPE_ID = 0 and a.TAXONOMY_NAME = '';

	
END

GO
