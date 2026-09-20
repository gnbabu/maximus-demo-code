/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PSPECLTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PSPECLTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PSPECLTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PSPECLTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PSPECLTB] 
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

		IF OBJECT_ID('tempdb..#PSPECLTB_UPDT') IS NOT NULL
		DROP TABLE #PSPECLTB_UPDT;

		CREATE TABLE #PSPECLTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-SPECL-BEG-DT] [varchar](10) NULL,
			[P-SPECL-CD] [varchar](3) NULL,
			[P-SPECL-END-DT] [varchar](10) NULL,
			[P-LIC-CERT-NUM] [varchar](10) NULL,
			[P-ST-CD] [varchar](2) NULL,
			[P-LIC-BRD-NUM] [varchar](40) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PSPECLTB_UPDT FROM ''D:/Conversion/04022017/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PSPECLTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PSPECLTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderSpecialtyUpdt
			 (
				[P-SYS-ID],
				[P-SPECL-BEG-DT],
				[P-SPECL-CD],
				[P-SPECL-END-DT],
				[P-LIC-CERT-NUM],
				[P-ST-CD],
				[P-LIC-BRD-NUM],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM]
			 )
			SELECT 
				[P-SYS-ID],
				[P-SPECL-BEG-DT],
				[P-SPECL-CD],
				[P-SPECL-END-DT],
				[P-LIC-CERT-NUM],
				[P-ST-CD],
				[P-LIC-BRD-NUM],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM]
			FROM #PSPECLTB_UPDT;

			-- Clean some known data issues
			UPDATE SRC_ProviderSpecialtyUpdt
			SET [P-SPECL-BEG-DT] = '1753-01-01' 
			WHERE [P-SPECL-BEG-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderSpecialtyUpdt
			SET [P-SPECL-END-DT] = '1753-01-01' 
			WHERE [P-SPECL-END-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderSpecialtyUpdt
			SET [P-SPECL-BEG-DT] = '9999-12-31' 
			WHERE ISDATE([P-SPECL-BEG-DT]) = 1 
			AND [P-SPECL-BEG-DT] >= '9899-12-31' 
			AND [P-SPECL-BEG-DT] < '9999-12-31';

			UPDATE SRC_ProviderSpecialtyUpdt
			SET [P-SPECL-END-DT] = '9999-12-31' 
			WHERE ISDATE([P-SPECL-END-DT]) = 1 
			AND [P-SPECL-END-DT] >= '9899-12-31' 
			AND [P-SPECL-END-DT] < '9999-12-31';
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