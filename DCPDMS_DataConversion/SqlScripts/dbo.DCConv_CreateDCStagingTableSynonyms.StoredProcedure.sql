/****** Object:  StoredProcedure [dbo].[DCConv_CreateDCStagingTableSynonyms]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_CreateDCStagingTableSynonyms]','P') is not null
DROP PROCEDURE [dbo].[DCConv_CreateDCStagingTableSynonyms]
GO

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
	 @pin_src_database_name varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @sql varchar(max);

	-- create synonyms for each DC data source table
	SET @sql = 'CREATE SYNONYM SRC_Providers FOR  [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_Enrollments FOR [' + @pin_src_database_name + '].[dbo].[DT_PENROLTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderAddresses FOR [' + @pin_src_database_name + '].[dbo].[DT_PADDRSTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderAltIDs FOR [' + @pin_src_database_name + '].[dbo].[DT_PALTIDTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderTaxonomy FOR [' + @pin_src_database_name + '].[dbo].[DT_PTAXONTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderSpecialty FOR [' + @pin_src_database_name + '].[dbo].[DT_PSPECLTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderLicense FOR [' + @pin_src_database_name + '].[dbo].[DT_PLICNSTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderCLIA FOR [' + @pin_src_database_name + '].[dbo].[DT_PCLIAPTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderCLIADetails FOR [' + @pin_src_database_name + '].[dbo].[DT_PCLIACTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderMedicare FOR [' + @pin_src_database_name + '].[dbo].[DT_PMCARETB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderMedicaid FOR [' + @pin_src_database_name + '].[dbo].[DT_PREVMCTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderOwner FOR [' + @pin_src_database_name + '].[dbo].[DT_POWNINTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderTaxonomyHeader FOR [' + @pin_src_database_name + '].[dbo].[DT_PTXNHDTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderAffiliates FOR [' + @pin_src_database_name + '].[dbo].[DT_PAFFILTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderTaxIds FOR [' + @pin_src_database_name + '].[dbo].[DT_PTAXIDTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderNumOfBeds FOR [' + @pin_src_database_name + '].[dbo].[DT_PNUBEDTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderCatOfService FOR [' + @pin_src_database_name + '].[dbo].[DT_PRVCOSTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderQuestions FOR [' + @pin_src_database_name + '].[dbo].[DT_PRQSTNTB_STG];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_ProviderProgram FOR [' + @pin_src_database_name + '].[dbo].[DT_PPROGMTB_STG];';
	
	SET @sql = @sql + 'CREATE SYNONYM SRC_LkUpAddressType FOR [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_ADDRESS_TYPE];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_LkUpProviderType FOR [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_TYPE_CODE];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_LkUpSpecialtyType FOR [' + @pin_src_database_name + '].[dbo].[LKUP_PROVIDER_SPECIALTY_CODE];';
	SET @sql = @sql + 'CREATE SYNONYM SRC_Documents FOR [' + @pin_src_database_name + '].[dbo].[DT_DOCS_STG];';
	EXEC(@sql);
END


GO
