/****** Object:  StoredProcedure [dbo].[DCConv_ConvertGroups_Step2]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_ConvertGroups_Step2]','P') is not null
DROP PROCEDURE [dbo].[DCConv_ConvertGroups_Step2]
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
CREATE PROCEDURE [dbo].[DCConv_ConvertGroups_Step2] 
(
	@pin_conv_run_id varchar(20),
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
	
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
		SET @currentTable = 'REG_TAXONOMY';
		WITH tt as (
		SELECT map.RegistrationId AS [REG_ID],
				0 AS [PRIMARY_FLAG],
				--(SELECT TAXONOMY_TYPE_ID as TAXONOMY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND PROVIDER_TYPE_ID = prov.[PROVIDER_TYPE_ID]) AS [TAXONOMY_TYPE_ID],
				tt.TAXONOMY_TYPE_ID,
				1 AS [MODIFIED_STATUS_TYPE_ID],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-BEG-DT]) AS [START_DATE],
				dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-END-DT]) AS [END_DATE],
				@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
				dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderTaxonomy tax ON tax.[P-SYS-ID] = map.[SysID]
	    INNER JOIN REG_PROVIDER prov ON prov.[REG_ID] = map.[RegistrationID]
		JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND tt.PROVIDER_TYPE_ID = prov.[PROVIDER_TYPE_ID]
		WHERE map.IsGroup = 1 AND tax.[EnableConversion] = 1
		)
		
		INSERT INTO [dbo].[REG_TAXONOMY]
				   ([REG_ID]
				   ,[PRIMARY_FLAG]
				   ,[TAXONOMY_TYPE_ID]
				   ,[MODIFIED_STATUS_TYPE_ID]
				   ,[START_DATE]
				   ,[END_DATE]
				   ,[LAST_MODIFIED_DATE_TIME]
				   ,[LAST_MODIFIED_USER])
		SELECT [REG_ID], [PRIMARY_FLAG], [TAXONOMY_TYPE_ID], [MODIFIED_STATUS_TYPE_ID],
			   [START_DATE], [END_DATE], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]
		FROM tt 
		WHERE [TAXONOMY_TYPE_ID] is not null;


		SET @currentTable = 'REG_SPECIALTY';
		WITH spec as (
		 SELECT DISTINCT map.RegistrationId AS [REG_ID],
			0 AS [PRIMARY_FLAG],
			--(SELECT SPECIALTY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(specl.[P-SPECL-CD]))) AND PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID) AS [SPECIALTY_TYPE_ID],
			tt.SPECIALTY_TYPE_ID,
			'N' AS [SPECIALTY_BOARD_CERTIFIED],
			1 AS [MODIFIED_STATUS_TYPE_ID],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-BEG-DT]) AS [START_DATE],
			dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-END-DT]) AS  [END_DATE],
			@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
			dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
			NULL AS [REG_TAXONOMY_ID] -- Must talk to Diwakar about this!!
			,specl.[P-ST-CD] AS [SPECIALTY_BOARD_STATE]
			,specl.[P-LIC-BRD-NUM] AS [SPECIALTY_BOARD_NAME]
		 FROM DCConv_KeyCrossReferences map
		 INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysId]
		 INNER JOIN REG_PROVIDER prov ON prov.[REG_ID] = map.[RegistrationID]
		 JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(specl.[P-SPECL-CD]))) AND tt.PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID
		 WHERE map.IsGroup = 1  AND
		 specl.[EnableConversion] = 1 AND 
		 map.RegistrationID IN (SELECT REG_ID FROM REGISTRATION)
		)
		INSERT INTO [dbo].[REG_SPECIALTY]
           ([REG_ID]
           ,[PRIMARY_FLAG]
           ,[SPECIALTY_TYPE_ID]
           ,[SPECIALTY_BOARD_CERTIFIED]
           ,[MODIFIED_STATUS_TYPE_ID]
           ,[START_DATE]
           ,[END_DATE]
           ,[LAST_MODIFIED_DATE_TIME]
           ,[LAST_MODIFIED_USER]
           ,[REG_TAXONOMY_ID]
		   ,[SPECIALTY_BOARD_STATE]
		   ,[SPECIALTY_BOARD_NAME])

		SELECT [REG_ID], [PRIMARY_FLAG], [SPECIALTY_TYPE_ID],[SPECIALTY_BOARD_CERTIFIED],[MODIFIED_STATUS_TYPE_ID],
		 [START_DATE],[END_DATE],[LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER], [REG_TAXONOMY_ID], 
		 [SPECIALTY_BOARD_STATE],[SPECIALTY_BOARD_NAME]
		FROM spec
		WHERE [SPECIALTY_TYPE_ID] IS NOT NULL;
		--SELECT map.RegistrationId AS [REG_ID],
		--	1 AS [PRIMARY_FLAG],
		--	(SELECT SPECIALTY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(specl.[P-SPECL-CD]))) AND PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID) AS [SPECIALTY_TYPE_ID],
		--	'N' AS [SPECIALTY_BOARD_CERTIFIED],
		--	1 AS [MODIFIED_STATUS_TYPE_ID],
		--	dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-BEG-DT]) AS [START_DATE],
		--	dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-END-DT]) AS  [END_DATE],
		--	@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
		--	dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
		--	NULL AS [REG_TAXONOMY_ID] -- Must talk to Diwakar about this!!
		--FROM DCConv_KeyCrossReferences map
		--INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysId]
		--INNER JOIN REG_PROVIDER prov ON prov.[REG_ID] = map.[RegistrationID]
		--WHERE map.IsGroup = 1  AND
		--specl.[EnableConversion] = 1 AND 
		--map.RegistrationID IN (SELECT REG_ID FROM REGISTRATION);
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
