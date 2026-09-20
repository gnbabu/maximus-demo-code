/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PADDRSTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PADDRSTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PADDRSTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PADDRSTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PADDRSTB] 
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
		
		IF OBJECT_ID('tempdb..#PADDRSTB_UPDT') IS NOT NULL
		DROP TABLE #PADDRSTB_UPDT;

		CREATE TABLE #PADDRSTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-TAX-KEY-ID] [varchar](9) NULL,
			[P-ADR-TY-CD] [varchar](1) NULL,
			[P-NAM-ORG-IND] [varchar](1) NULL,
			[P-NAM] [varchar](35) NULL,
			[P-LAST-NAM] [varchar](35) NULL,
			[P-FST-NAM] [varchar](15) NULL,
			[P-MI-NAM] [varchar](1) NULL,
			[P-SFX-NAM] [varchar](5) NULL,
			[P-ST-CD] [varchar](2) NULL,
			[P-CNTY-CD] [varchar](2) NULL,
			[P-LINE1-AD] [varchar](30) NULL,
			[P-LINE2-AD] [varchar](30) NULL,
			[P-CITY-NAM] [varchar](20) NULL,
			[P-ZIP4-CD] [varchar](4) NULL,
			[P-ZIP5-CD] [varchar](5) NULL,
			[P-PHON-NUM] [varchar](10) NULL,
			[P-FAX-NUM] [varchar](10) NULL,
			[P-CONTCT-NAM] [varchar](35) NULL,
			[P-CONTCT-PHON-NUM] [varchar](10) NULL,
			[P-CONTCT-FAX-NUM] [varchar](10) NULL,
			[P-CONTCT-EMAIL-AD-TEXT] [varchar](128) NULL,
			[P-RTRN-MAIL-IND] [varchar](1) NULL,
			[P-BARCODE-ROUT-DAT] [varchar](2) NULL,
			[P-BARCODE-CHK-DAT] [varchar](1) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL,
			[G-QUAD-CD] [varchar](2) NULL,
			[G-WARD-CD] [varchar](2) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PADDRSTB_UPDT FROM ''D:/Conversion/04022017/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PADDRSTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PADDRSTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderAddressUpdt
			  ( [P-SYS-ID],
				[P-TAX-KEY-ID],
				[P-ADR-TY-CD],
				[P-NAM-ORG-IND],
				[P-NAM],
				[P-LAST-NAM],
				[P-FST-NAM],
				[P-MI-NAM],
				[P-SFX-NAM],
				[P-ST-CD],
				[P-CNTY-CD],
				[P-LINE1-AD],
				[P-LINE2-AD],
				[P-CITY-NAM],
				[P-ZIP4-CD],
				[P-ZIP5-CD],
				[P-PHON-NUM],
				[P-FAX-NUM],
				[P-CONTCT-NAM],
				[P-CONTCT-PHON-NUM],
				[P-CONTCT-FAX-NUM],
				[P-CONTCT-EMAIL-AD-TEXT],
				[P-RTRN-MAIL-IND],
				[P-BARCODE-ROUT-DAT],
				[P-BARCODE-CHK-DAT],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM],
				[G-QUAD-CD],
				[G-WARD-CD] )
			SELECT [P-SYS-ID],
				[P-TAX-KEY-ID],
				[P-ADR-TY-CD],
				[P-NAM-ORG-IND],
				[P-NAM],
				[P-LAST-NAM],
				[P-FST-NAM],
				[P-MI-NAM],
				[P-SFX-NAM],
				[P-ST-CD],
				[P-CNTY-CD],
				[P-LINE1-AD],
				[P-LINE2-AD],
				[P-CITY-NAM],
				[P-ZIP4-CD],
				[P-ZIP5-CD],
				[P-PHON-NUM],
				[P-FAX-NUM],
				[P-CONTCT-NAM],
				[P-CONTCT-PHON-NUM],
				[P-CONTCT-FAX-NUM],
				[P-CONTCT-EMAIL-AD-TEXT],
				[P-RTRN-MAIL-IND],
				[P-BARCODE-ROUT-DAT],
				[P-BARCODE-CHK-DAT],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM],
				[G-QUAD-CD],
				[G-WARD-CD]			  
			FROM #PADDRSTB_UPDT;


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