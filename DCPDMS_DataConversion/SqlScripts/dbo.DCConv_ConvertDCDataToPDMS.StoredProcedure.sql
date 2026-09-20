/****** Object:  StoredProcedure [dbo].[DCConv_ConvertDCDataToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertDCDataToPDMS]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertDCDataToPDMS]
GO

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
	@pin_src_database_name varchar(50),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime,
	@pin_active_only bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	PRINT 'START TIME: ' + CAST(GETDATE() AS varchar(50));

	PRINT 'Create the utility tables'
	EXEC DCConv_CreateUtilityTables

	PRINT 'Drop and re-create synonyms'
	EXEC DCConv_DropDCStagingTableSynonyms;
	EXEC DCConv_CreateDCStagingTableSynonyms @pin_src_database_name;

	PRINT 'Clear out any old data'
	EXEC DCConv_ClearRegistrationData @pin_conv_run_id;

	PRINT 'Create the taxonomy and specialty reference value tables Step 1'
	EXEC DCConv_ConvertReferenceValues_Step1 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Validation Step 1';
	EXEC DCConv_Validate_Step1 @pin_conv_run_id, @pin_src_database_name, @pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Convert the groups Step 1'
	EXEC DCConv_ConvertGroups_Step1 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date,@pin_active_only;

	PRINT 'Convert individuals Step 1'
	EXEC DCConv_ConvertIndividuals_Step1 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date,@pin_active_only;

	PRINT 'Create the taxonomy and specialty reference value tables Step 2'
	EXEC DCConv_ConvertReferenceValues_Step2 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Validation Step 2';
	EXEC DCConv_Validate_Step2 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Convert the groups Step 2'
	EXEC DCConv_ConvertGroups_Step2 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Convert individuals Step 2'
	EXEC DCConv_ConvertIndividuals_Step2 @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Convert affiliations'
	EXEC DCConv_ConvertAffiliations @pin_conv_run_id,@pin_conv_run_time,@pin_conv_data_export_date;

	PRINT 'Convert Documents'
	EXEC DCConv_LkUp_PopulateREG_CONVERTED_DOCUMENT_XREF @pin_conv_run_time

	PRINT 'END TIME: ' + CAST(GETDATE() AS varchar(50));
END


GO
