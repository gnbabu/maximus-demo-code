/****** Object:  StoredProcedure [dbo].[DCConv_Validate_Step2]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_Validate_Step2]','P') is not null
DROP PROCEDURE [dbo].DCConv_Validate_Step2
GO
/****** Object:  StoredProcedure [dbo].[DCConv_Validate_Step2]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/26/2016
-- Description:	Validates the data
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Validate_Step2] 
	-- Add the parameters for the stored procedure here
	@pin_conv_run_id varchar(20), 
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	--UPDATE [SRC_ProviderTaxonomy] SET [EnableConversion] = 0, 
	--[ErrorCode] = 4,[ErrorMessage] = CAST(tax.[P-SYS-ID] AS varchar(10)) + '|' + sprov.[P-ID] + '|[P-TAXONOMY-CD]|'  + tax.[P-TAXONOMY-CD]
	--FROM SRC_ProviderTaxonomy tax 
	--INNER JOIN SRC_Providers sprov ON sprov.[P-SYS-ID] = tax.[P-SYS-ID]
	--INNER JOIN REG_PROVIDER prov ON prov.REG_ID = (SELECT RegistrationID FROM DCConv_KeyCrossReferences WHERE SysID = tax.[P-SYS-ID] AND EnrollmentStartDate = (SELECT TOP 1 EnrollmentStartDate FROM DCConv_KeyCrossReferences WHERE SysID = tax.[P-SYS-ID] AND EnrollmentStartDate <= dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-BEG-DT]) ORDER BY EnrollmentStartDate))
	--WHERE 
	--(SELECT DISTINCT TAXONOMY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND	PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID) = 0;

	UPDATE [SRC_ProviderTaxonomy] SET [EnableConversion] = 0, 
	[ErrorCode] = 4,[ErrorMessage] = CAST(tax.[P-SYS-ID] AS varchar(10)) + '|' + sprov.[P-ID] + '|[P-TAXONOMY-CD]|'  + tax.[P-TAXONOMY-CD]
	FROM SRC_ProviderTaxonomy tax 
	INNER JOIN SRC_Providers sprov ON sprov.[P-SYS-ID] = tax.[P-SYS-ID]
	JOIN DCConv_KeyCrossReferences map on sprov.[P-SYS-ID] = map.SysID
	INNER JOIN REG_PROVIDER prov ON map.RegistrationID = prov.REG_ID 
	JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND	tt.PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID
	WHERE tt.TAXONOMY_TYPE_ID = 0;


	--UPDATE [SRC_ProviderSpecialty] SET [EnableConversion] = 0, 
	--[ErrorCode] = 5,[ErrorMessage] = CAST(tax.[P-SYS-ID] AS varchar(10)) + '|' + sprov.[P-ID] + '|[P-SPECL-CD]|'  + tax.[P-SPECL-CD]
	--FROM SRC_ProviderSpecialty tax 
	--INNER JOIN SRC_Providers sprov ON sprov.[P-SYS-ID] = tax.[P-SYS-ID]
	--INNER JOIN REG_PROVIDER prov ON prov.REG_ID = (SELECT RegistrationID FROM DCConv_KeyCrossReferences WHERE SysID = tax.[P-SYS-ID] AND EnrollmentStartDate = (SELECT TOP 1 EnrollmentStartDate FROM DCConv_KeyCrossReferences WHERE SysID = tax.[P-SYS-ID] AND EnrollmentStartDate <= dbo.fn_ConvertDCDateToPDMS(tax.[P-SPECL-BEG-DT]) ORDER BY EnrollmentStartDate))
	--WHERE 
	--(SELECT DISTINCT SPECIALTY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(tax.[P-SPECL-CD]))) AND PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID) = 0;

	UPDATE [SRC_ProviderSpecialty] SET [EnableConversion] = 0, 
	[ErrorCode] = 5,[ErrorMessage] = CAST(tax.[P-SYS-ID] AS varchar(10)) + '|' + sprov.[P-ID] + '|[P-SPECL-CD]|'  + tax.[P-SPECL-CD]
	FROM SRC_ProviderSpecialty tax 
	INNER JOIN SRC_Providers sprov ON sprov.[P-SYS-ID] = tax.[P-SYS-ID]
	JOIN DCConv_KeyCrossReferences map on sprov.[P-SYS-ID] = map.SysID
	INNER JOIN REG_PROVIDER prov ON map.RegistrationID = prov.REG_ID 
	JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(tax.[P-SPECL-CD]))) AND tt.PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID
	WHERE tt.SPECIALTY_TYPE_ID = 0;


END


GO
