/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PLICNSTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PLICNSTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PLICNSTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PLICNSTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PLICNSTB] 
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

		IF OBJECT_ID('tempdb..#PLICNSTB_UPDT') IS NOT NULL
		DROP TABLE #PLICNSTB_UPDT;

		CREATE TABLE #PLICNSTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-LIC-EFF-DT] [varchar](10) NULL,
			[P-LIC-CERT-NUM] [varchar](10) NULL,
			[P-LIC-CERT-CD] [varchar](2) NULL,
			[P-ST-CD] [varchar](2) NULL,
			[P-LIC-RSTRCT-CD] [varchar](1) NULL,
			[P-LIC-VRFY-IND] [varchar](1) NULL,
			[P-LIC-BRD-NUM] [varchar](40) NULL,
			[P-LIC-EXPIR-DT] [varchar](10) NULL,
			[P-LIC-PERMIT-ID] [varchar](30) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PLICNSTB_UPDT FROM ''D:/Conversion/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PLICNSTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PLICNSTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderLicenseUpdt
			 ([P-SYS-ID], [P-LIC-EFF-DT], [P-LIC-CERT-NUM], [P-LIC-CERT-CD], [P-ST-CD], [P-LIC-RSTRCT-CD],
			  [P-LIC-VRFY-IND], [P-LIC-BRD-NUM], [P-LIC-EXPIR-DT], [P-LIC-PERMIT-ID], [G-AUD-USER-ID],
			  [G-AUD-DT], [G-AUD-TM])
			SELECT 
			  [P-SYS-ID], [P-LIC-EFF-DT], [P-LIC-CERT-NUM], [P-LIC-CERT-CD], [P-ST-CD], [P-LIC-RSTRCT-CD],
			  [P-LIC-VRFY-IND], [P-LIC-BRD-NUM], [P-LIC-EXPIR-DT], [P-LIC-PERMIT-ID], [G-AUD-USER-ID],
			  [G-AUD-DT], [G-AUD-TM]
			FROM #PLICNSTB_UPDT;

			-- Clean some known data issues
			UPDATE SRC_ProviderLicenseUpdt
			SET [P-LIC-EFF-DT] = '1753-01-01' 
			WHERE [P-LIC-EFF-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderLicenseUpdt
			SET [P-LIC-EXPIR-DT] = '1753-01-01' 
			WHERE [P-LIC-EXPIR-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderLicenseUpdt
			SET [P-LIC-EFF-DT] = '9999-12-31' 
			WHERE ISDATE([P-LIC-EFF-DT]) = 1 
			AND [P-LIC-EFF-DT] >= '9899-12-31' 
			AND [P-LIC-EFF-DT] < '9999-12-31';

			UPDATE SRC_ProviderLicenseUpdt
			SET [P-LIC-EXPIR-DT] = '9999-12-31' 
			WHERE ISDATE([P-LIC-EXPIR-DT]) = 1 
			AND [P-LIC-EXPIR-DT] >= '9899-12-31' 
			AND [P-LIC-EXPIR-DT] < '9999-12-31';



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