/****** Object:  StoredProcedure [dbo].[DCConv_DropDCStagingTableSynonyms]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_DropDCStagingTableSynonyms]','P') is not null
DROP PROCEDURE [dbo].[DCConv_DropDCStagingTableSynonyms]
GO

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
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_Columns'))
		DROP SYNONYM SRC_Columns;
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

	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderTaxIds'))
		DROP SYNONYM SRC_ProviderTaxIds;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpAddressType'))
		DROP SYNONYM SRC_LkUpAddressType;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpProviderType'))
		DROP SYNONYM SRC_LkUpProviderType;
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_LkUpSpecialtyType'))
		DROP SYNONYM SRC_LkUpSpecialtyType;

	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderNumOfBeds'))
		DROP SYNONYM SRC_ProviderNumOfBeds;

	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderProgram'))
		DROP SYNONYM SRC_ProviderProgram;		
	
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderCatOfService'))
		DROP SYNONYM SRC_ProviderCatOfService;
		
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_ProviderQuestions'))
		DROP SYNONYM SRC_ProviderQuestions;
		
	IF (EXISTS(SELECT * FROM sys.synonyms WHERE name = 'SRC_Documents'))
		DROP SYNONYM SRC_Documents;
END


GO
