/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PTAXONTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PTAXONTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PTAXONTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PTAXONTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PTAXONTB] 
(
    @fileName varchar(25),
	@pin_src_database_name varchar(50),
	@pin_conv_run_id varchar(20)
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		IF OBJECT_ID('tempdb..#PTAXONTB_UPDT') IS NOT NULL
		DROP TABLE #PTAXONTB_UPDT;

		CREATE TABLE #PTAXONTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-TAXONOMY-CD] [varchar](10) NULL,
			[P-TAXON-BEG-DT] [varchar](10) NULL,
			[P-TAXON-END-DT] [varchar](10) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PTAXONTB_UPDT FROM ''D:/Conversion/04022017/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PTAXONTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PTAXONTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderTaxonomyUpdt
			 (
				[P-SYS-ID],
				[P-TAXONOMY-CD],
				[P-TAXON-BEG-DT],
				[P-TAXON-END-DT],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM]
			 )
			SELECT 
				[P-SYS-ID],
				[P-TAXONOMY-CD],
				[P-TAXON-BEG-DT],
				[P-TAXON-END-DT],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM]
			FROM #PTAXONTB_UPDT;

			-- Clean some known data issues
			UPDATE SRC_ProviderTaxonomyUpdt
			SET [P-TAXON-BEG-DT] = '1753-01-01' 
			WHERE [P-TAXON-BEG-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderTaxonomyUpdt
			SET [P-TAXON-END-DT] = '1753-01-01' 
			WHERE [P-TAXON-END-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderTaxonomyUpdt
			SET [P-TAXON-BEG-DT] = '9999-12-31' 
			WHERE ISDATE([P-TAXON-BEG-DT]) = 1 
			AND [P-TAXON-BEG-DT] >= '9899-12-31' 
			AND [P-TAXON-BEG-DT] < '9999-12-31';

			UPDATE SRC_ProviderTaxonomyUpdt
			SET [P-TAXON-END-DT] = '9999-12-31' 
			WHERE ISDATE([P-TAXON-END-DT]) = 1 
			AND [P-TAXON-END-DT] >= '9899-12-31' 
			AND [P-TAXON-END-DT] < '9999-12-31';
		END

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION;
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