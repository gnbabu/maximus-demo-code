/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PTAXIDTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PTAXIDTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PTAXIDTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PTAXIDTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PTAXIDTB] 
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

		IF OBJECT_ID('tempdb..#PTAXIDTB_UPDT') IS NOT NULL
		DROP TABLE #PTAXIDTB_UPDT;

		CREATE TABLE #PTAXIDTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-TAX-BEG-DT] [varchar](10) NULL,
			[P-TAX-END-DT] [varchar](10) NULL,
			[P-FED-ID-IND] [varchar](1) NULL,
			[P-FED-TAX-ID] [varchar](9) NULL,
			[P-SSN-NUM] [varchar](9) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PTAXIDTB_UPDT FROM ''D:/Conversion/04022017/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PTAXIDTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PTAXIDTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderTaxIdUpdt
			 ([P-SYS-ID],[P-TAX-BEG-DT],[P-TAX-END-DT],[P-FED-ID-IND],[P-FED-TAX-ID],
			[P-SSN-NUM],[G-AUD-USER-ID],[G-AUD-DT],[G-AUD-TM])
			SELECT 
			[P-SYS-ID],[P-TAX-BEG-DT],[P-TAX-END-DT],[P-FED-ID-IND],[P-FED-TAX-ID],
			[P-SSN-NUM],[G-AUD-USER-ID],[G-AUD-DT],[G-AUD-TM]
			FROM #PTAXIDTB_UPDT;

			-- Clean some known data issues
			UPDATE SRC_ProviderTaxIdUpdt
			SET [P-TAX-BEG-DT] = '1753-01-01' 
			WHERE [P-TAX-BEG-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderTaxIdUpdt
			SET [P-TAX-END-DT] = '1753-01-01' 
			WHERE [P-TAX-END-DT] IN ('0001-01-01','0101-01-01');
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
        (@pin_conv_run_id,'SRC_ProviderTaxIdUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO