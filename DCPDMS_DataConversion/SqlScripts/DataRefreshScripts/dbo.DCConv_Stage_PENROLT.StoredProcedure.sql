/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PENROLT]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PENROLT]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PENROLT]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PENROLT_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PENROLT] 
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
		
		IF OBJECT_ID('tempdb..#PENROLT_UPDT') IS NOT NULL
		DROP TABLE #PENROLT_UPDT;

		CREATE TABLE #PENROLT_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-STAT-EFF-DT] [varchar](10) NULL,
			[P-TY-CD] [varchar](3) NULL,
			[P-STAT-END-DT] [varchar](10) NULL,
			[P-ENROL-STAT-TY-CD] [varchar](2) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-TS] [varchar](26) NULL,
			[P-BRND-DISCT-PCT] [varchar](18) NULL,
			[P-GENR-DISCT-PCT] [varchar](18) NULL,
			[P-DISP-FEE-AMT] [varchar](18) NULL,
			[P-RE-ENROL-STAT-CD] [varchar](1) NULL,
			[P-ENROL-APP-SRC-CD] [varchar](1) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PENROLT_UPDT FROM ''D:/Conversion/04022017/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PENROLT_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PENROLTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_EnrollmentUpdt
			  ( [P-SYS-ID],
				[P-STAT-EFF-DT],
				[P-TY-CD],
				[P-STAT-END-DT],
				[P-ENROL-STAT-TY-CD],
				[G-AUD-USER-ID],
				[G-AUD-TS],
				[P-BRND-DISCT-PCT],
				[P-GENR-DISCT-PCT],
				[P-DISP-FEE-AMT],
				[P-RE-ENROL-STAT-CD],
				[P-ENROL-APP-SRC-CD] )
			SELECT [P-SYS-ID],
				[P-STAT-EFF-DT],
				[P-TY-CD],
				[P-STAT-END-DT],
				[P-ENROL-STAT-TY-CD],
				[G-AUD-USER-ID],
				[G-AUD-TS],
				[P-BRND-DISCT-PCT],
				[P-GENR-DISCT-PCT],
				[P-DISP-FEE-AMT],
				[P-RE-ENROL-STAT-CD],
				[P-ENROL-APP-SRC-CD]		  
			FROM #PENROLT_UPDT;


			-- Clean some known data issues
			UPDATE SRC_EnrollmentUpdt
			SET [P-STAT-EFF-DT] = '1753-01-01' 
			WHERE [P-STAT-EFF-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_EnrollmentUpdt
			SET [P-STAT-END-DT] = '1753-01-01' 
			WHERE [P-STAT-END-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_EnrollmentUpdt
			SET [P-STAT-EFF-DT] = '9999-12-31' 
			WHERE ISDATE([P-STAT-EFF-DT]) = 1 
			AND [P-STAT-EFF-DT] >= '9899-12-31' 
			AND [P-STAT-EFF-DT] < '9999-12-31';

			UPDATE SRC_EnrollmentUpdt
			SET [P-STAT-END-DT] = '9999-12-31' 
			WHERE ISDATE([P-STAT-END-DT]) = 1 
			AND [P-STAT-END-DT] >= '9899-12-31' 
			AND [P-STAT-END-DT] < '9999-12-31';


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