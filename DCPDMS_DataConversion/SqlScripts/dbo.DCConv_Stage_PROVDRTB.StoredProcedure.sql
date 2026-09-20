/****** Object:  StoredProcedure [dbo].[DCConv_Stage_PROVDRTB]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Stage_PROVDRTB]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Stage_PROVDRTB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Loads the DT_PROVDRTB_STG table with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Stage_PROVDRTB] 
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
		
		IF OBJECT_ID('tempdb..#PROVDRTB_UPDT') IS NOT NULL
		DROP TABLE #PROVDRTB_UPDT;

		CREATE TABLE #PROVDRTB_UPDT (
			[P-SYS-ID] [int] NULL,
			[P-REC-TY-CD] [varchar](1) NULL,
			[P-ID] [varchar](9) NULL,
			[P-APPL-NUM] [varchar](8) NULL,
			[P-ORIG-PROV-ID] [varchar](9) NULL,
			[P-RA-MEDM-CD] [varchar](1) NULL,
			[P-LOCN-CD] [varchar](1) NULL,
			[P-RA-SORT-SEQ-CD] [varchar](1) NULL,
			[P-RA-PRT-SUSP-CD] [varchar](1) NULL,
			[P-PRACT-TY-CD] [varchar](1) NULL,
			[P-INDIV-GRP-CD] [varchar](1) NULL,
			[P-OWNER-TY-CD] [varchar](1) NULL,
			[P-NF-CLS-CD] [varchar](1) NULL,
			[P-PHRM-CLS-CD] [varchar](1) NULL,
			[P-FACI-TY-CD] [varchar](1) NULL,
			[P-UPIN-NUM] [varchar](6) NULL,
			[P-SSN-NUM] [varchar](9) NULL,
			[P-NPI-NUM] [varchar](10) NULL,
			[P-NABP-NUM] [varchar](11) NULL,
			[P-DEA-NUM] [varchar](11) NULL,
			[P-DBA-NAM] [varchar](35) NULL,
			[P-DBA-ORG-IND] [varchar](1) NULL,
			[P-DBA-LAST-NAM] [varchar](35) NULL,
			[P-DBA-FST-NAM] [varchar](15) NULL,
			[P-DBA-MI-NAM] [varchar](1) NULL,
			[P-DBA-SFX-NAM] [varchar](5) NULL,
			[P-NAM] [varchar](35) NULL,
			[P-NAM-ORG-IND] [varchar](1) NULL,
			[P-LAST-NAM] [varchar](35) NULL,
			[P-FST-NAM] [varchar](15) NULL,
			[P-MI-NAM] [varchar](1) NULL,
			[P-SFX-NAM] [varchar](5) NULL,
			[P-W9-SIGNED-DT] [varchar](10) NULL,
			[P-MCARE-FY-MO-NUM] [varchar](2) NULL,
			[P-MCAID-FY-MO-NUM] [varchar](2) NULL,
			[P-FACI-FY-MO-NUM] [varchar](2) NULL,
			[P-APPL-DT] [varchar](10) NULL,
			[P-FACI-BEG-DT] [varchar](10) NULL,
			[P-FACI-END-DT] [varchar](10) NULL,
			[P-COST-STTLMT-DT] [varchar](10) NULL,
			[P-BLLTN-MEDM-CD] [varchar](1) NULL,
			[P-BLLTN-COPY-NUM] [varchar](9) NULL,
			[P-EMC-PSWD-DAT] [varchar](8) NULL,
			[P-MCARE-IND] [varchar](1) NULL,
			[P-FED-VAC-CHLD-IND] [varchar](1) NULL,
			[P-BKUP-WHOLD-IND] [varchar](1) NULL,
			[P-MULTI-LOCN-IND] [varchar](1) NULL,
			[P-GROSS-TAX-NUM] [varchar](9) NULL,
			[P-PROFIT-IND] [varchar](1) NULL,
			[P-REVER-DT] [varchar](10) NULL,
			[P-BLNG-CD] [varchar](1) NULL,
			[P-PROF-TECH-IND] [varchar](1) NULL,
			[P-SOLE-COMM-IND] [varchar](1) NULL,
			[P-TAX-DISCT-IND] [varchar](1) NULL,
			[P-ADD-DT] [varchar](10) NULL,
			[P-SORT-NAM] [varchar](35) NULL,
			[P-STATE-MATCH-IND] [varchar](1) NULL,
			[P-DEFER-COMP-IND] [varchar](1) NULL,
			[P-ENROLL-ACTION-CD] [varchar](1) NULL,
			[P-TPL-AUDIT-DT] [varchar](10) NULL,
			[P-TPL-AUDIT-TY-CD] [varchar](1) NULL,
			[P-APPL-COMPLETE-DT] [varchar](10) NULL,
			[P-APPL-STAT-CD] [varchar](1) NULL,
			[P-DUPL-OVRRD-IND] [varchar](1) NULL,
			[P-CANCEL-APPL-IND] [varchar](1) NULL,
			[P-SVC-LOC-CD] [varchar](30) NULL,
			[P-VFC-INFO-IND] [varchar](1) NULL,
			[G-AUD-USER-ID] [varchar](7) NULL,
			[G-AUD-DT] [varchar](10) NULL,
			[G-AUD-TM] [varchar](8) NULL,
			[P-ATYP-IND] [varchar](1) NULL,
			[P-HOUR-OPER-TX-TEXT] [varchar](200) NULL,
			[P-NABP-EFF-DT] [varchar](10) NULL,
			[P-DEA-EFF-DT] [varchar](10) NULL,
			[P-DEA-EXP-DT] [varchar](10) NULL,
			[P-OIN-NUM] [varchar](7) NULL,
			[P-EPSDT-IND] [varchar](1) NULL,
			[P-PYMT-MTHD-CD] [varchar](1) NULL,
			[P-1099-CD] [varchar](1) NULL,
			[P-EPSDT-INFO-IND] [varchar](1) NULL,
			[P-ENROL-FEE-PD-IND] [varchar](1) NULL,
			[P-DOB-DT] [varchar](10) NULL
		) 

		DECLARE @SQL NVARCHAR(max);

		SET @SQL = N'BULK INSERT #PROVDRTB_UPDT FROM ''D:/Conversion/' + @fileName + ''' WITH ( FIRSTROW = 1, FIELDTERMINATOR = ''|'', ROWTERMINATOR = ''\n'') ';
		exec(@SQL);

		IF (SELECT COUNT(*) FROM #PROVDRTB_UPDT) > 0
		BEGIN
			SET @SQL = 'TRUNCATE TABLE [' + @pin_src_database_name + '].[dbo].[DT_PROVDRTB_STG];';

			exec(@SQL)

			INSERT INTO SRC_ProviderUpdt
			  ( [P-SYS-ID],
				[P-REC-TY-CD],
				[P-ID],
				[P-APPL-NUM],
				[P-ORIG-PROV-ID],
				[P-RA-MEDM-CD],
				[P-LOCN-CD],
				[P-RA-SORT-SEQ-CD],
				[P-RA-PRT-SUSP-CD],
				[P-PRACT-TY-CD],
				[P-INDIV-GRP-CD],
				[P-OWNER-TY-CD],
				[P-NF-CLS-CD],
				[P-PHRM-CLS-CD],
				[P-FACI-TY-CD],
				[P-UPIN-NUM],
				[P-SSN-NUM],
				[P-NPI-NUM],
				[P-NABP-NUM],
				[P-DEA-NUM],
				[P-DBA-NAM],
				[P-DBA-ORG-IND],
				[P-DBA-LAST-NAM],
				[P-DBA-FST-NAM],
				[P-DBA-MI-NAM],
				[P-DBA-SFX-NAM],
				[P-NAM],
				[P-NAM-ORG-IND],
				[P-LAST-NAM],
				[P-FST-NAM],
				[P-MI-NAM],
				[P-SFX-NAM],
				[P-W9-SIGNED-DT],
				[P-MCARE-FY-MO-NUM],
				[P-MCAID-FY-MO-NUM],
				[P-FACI-FY-MO-NUM],
				[P-APPL-DT],
				[P-FACI-BEG-DT],
				[P-FACI-END-DT],
				[P-COST-STTLMT-DT],
				[P-BLLTN-MEDM-CD],
				[P-BLLTN-COPY-NUM],
				[P-EMC-PSWD-DAT],
				[P-MCARE-IND],
				[P-FED-VAC-CHLD-IND],
				[P-BKUP-WHOLD-IND],
				[P-MULTI-LOCN-IND],
				[P-GROSS-TAX-NUM],
				[P-PROFIT-IND],
				[P-REVER-DT],
				[P-BLNG-CD],
				[P-PROF-TECH-IND],
				[P-SOLE-COMM-IND],
				[P-TAX-DISCT-IND],
				[P-ADD-DT],
				[P-SORT-NAM],
				[P-STATE-MATCH-IND],
				[P-DEFER-COMP-IND],
				[P-ENROLL-ACTION-CD],
				[P-TPL-AUDIT-DT],
				[P-TPL-AUDIT-TY-CD],
				[P-APPL-COMPLETE-DT],
				[P-APPL-STAT-CD],
				[P-DUPL-OVRRD-IND],
				[P-CANCEL-APPL-IND],
				[P-SVC-LOC-CD],
				[P-VFC-INFO-IND],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM],
				[P-ATYP-IND],
				[P-HOUR-OPER-TX-TEXT],
				[P-NABP-EFF-DT],
				[P-DEA-EFF-DT],
				[P-DEA-EXP-DT],
				[P-OIN-NUM],
				[P-EPSDT-IND],
				[P-PYMT-MTHD-CD],
				[P-1099-CD],
				[P-EPSDT-INFO-IND],
				[P-ENROL-FEE-PD-IND],
				[P-DOB-DT] )
			SELECT [P-SYS-ID],
				[P-REC-TY-CD],
				[P-ID],
				[P-APPL-NUM],
				[P-ORIG-PROV-ID],
				[P-RA-MEDM-CD],
				[P-LOCN-CD],
				[P-RA-SORT-SEQ-CD],
				[P-RA-PRT-SUSP-CD],
				[P-PRACT-TY-CD],
				[P-INDIV-GRP-CD],
				[P-OWNER-TY-CD],
				[P-NF-CLS-CD],
				[P-PHRM-CLS-CD],
				[P-FACI-TY-CD],
				[P-UPIN-NUM],
				[P-SSN-NUM],
				[P-NPI-NUM],
				[P-NABP-NUM],
				[P-DEA-NUM],
				[P-DBA-NAM],
				[P-DBA-ORG-IND],
				[P-DBA-LAST-NAM],
				[P-DBA-FST-NAM],
				[P-DBA-MI-NAM],
				[P-DBA-SFX-NAM],
				[P-NAM],
				[P-NAM-ORG-IND],
				[P-LAST-NAM],
				[P-FST-NAM],
				[P-MI-NAM],
				[P-SFX-NAM],
				[P-W9-SIGNED-DT],
				[P-MCARE-FY-MO-NUM],
				[P-MCAID-FY-MO-NUM],
				[P-FACI-FY-MO-NUM],
				[P-APPL-DT],
				[P-FACI-BEG-DT],
				[P-FACI-END-DT],
				[P-COST-STTLMT-DT],
				[P-BLLTN-MEDM-CD],
				[P-BLLTN-COPY-NUM],
				[P-EMC-PSWD-DAT],
				[P-MCARE-IND],
				[P-FED-VAC-CHLD-IND],
				[P-BKUP-WHOLD-IND],
				[P-MULTI-LOCN-IND],
				[P-GROSS-TAX-NUM],
				[P-PROFIT-IND],
				[P-REVER-DT],
				[P-BLNG-CD],
				[P-PROF-TECH-IND],
				[P-SOLE-COMM-IND],
				[P-TAX-DISCT-IND],
				[P-ADD-DT],
				[P-SORT-NAM],
				[P-STATE-MATCH-IND],
				[P-DEFER-COMP-IND],
				[P-ENROLL-ACTION-CD],
				[P-TPL-AUDIT-DT],
				[P-TPL-AUDIT-TY-CD],
				[P-APPL-COMPLETE-DT],
				[P-APPL-STAT-CD],
				[P-DUPL-OVRRD-IND],
				[P-CANCEL-APPL-IND],
				[P-SVC-LOC-CD],
				[P-VFC-INFO-IND],
				[G-AUD-USER-ID],
				[G-AUD-DT],
				[G-AUD-TM],
				[P-ATYP-IND],
				[P-HOUR-OPER-TX-TEXT],
				[P-NABP-EFF-DT],
				[P-DEA-EFF-DT],
				[P-DEA-EXP-DT],
				[P-OIN-NUM],
				[P-EPSDT-IND],
				[P-PYMT-MTHD-CD],
				[P-1099-CD],
				[P-EPSDT-INFO-IND],
				[P-ENROL-FEE-PD-IND],
				[P-DOB-DT]			  
			FROM #PROVDRTB_UPDT;

			-- Clean some known data issues
			UPDATE SRC_ProviderUpdt
			SET [P-W9-SIGNED-DT] = '1753-01-01' 
			WHERE [P-W9-SIGNED-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-APPL-DT] = '1753-01-01' 
			WHERE [P-APPL-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-FACI-BEG-DT] = '1753-01-01' 
			WHERE [P-FACI-BEG-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-FACI-END-DT] = '1753-01-01' 
			WHERE [P-FACI-END-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-COST-STTLMT-DT] = '1753-01-01' 
			WHERE [P-COST-STTLMT-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-REVER-DT] = '1753-01-01' 
			WHERE [P-REVER-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-ADD-DT] = '1753-01-01' 
			WHERE [P-ADD-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-TPL-AUDIT-DT] = '1753-01-01' 
			WHERE [P-TPL-AUDIT-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-APPL-COMPLETE-DT] = '1753-01-01' 
			WHERE [P-APPL-COMPLETE-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-NABP-EFF-DT] = '1753-01-01' 
			WHERE [P-NABP-EFF-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-DEA-EFF-DT] = '1753-01-01' 
			WHERE [P-DEA-EFF-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-DEA-EXP-DT] = '1753-01-01' 
			WHERE [P-DEA-EXP-DT] IN ('0001-01-01','0101-01-01');

			UPDATE SRC_ProviderUpdt
			SET [P-DOB-DT] = '1753-01-01' 
			WHERE [P-DOB-DT] IN ('0001-01-01','0101-01-01');



			UPDATE SRC_ProviderUpdt
			SET [P-W9-SIGNED-DT] = '9999-12-31' 
			WHERE ISDATE([P-W9-SIGNED-DT]) = 1 
			AND [P-W9-SIGNED-DT] >= '9899-12-31' 
			AND [P-W9-SIGNED-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-APPL-DT] = '9999-12-31' 
			WHERE ISDATE([P-APPL-DT]) = 1 
			AND [P-APPL-DT] >= '9899-12-31' 
			AND [P-APPL-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-FACI-BEG-DT] = '9999-12-31' 
			WHERE ISDATE([P-FACI-BEG-DT]) = 1 
			AND [P-FACI-BEG-DT] >= '9899-12-31' 
			AND [P-FACI-BEG-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-FACI-END-DT] = '9999-12-31' 
			WHERE ISDATE([P-FACI-END-DT]) = 1 
			AND [P-FACI-END-DT] >= '9899-12-31' 
			AND [P-FACI-END-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-COST-STTLMT-DT] = '9999-12-31' 
			WHERE ISDATE([P-COST-STTLMT-DT]) = 1 
			AND [P-COST-STTLMT-DT] >= '9899-12-31' 
			AND [P-COST-STTLMT-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-REVER-DT] = '9999-12-31' 
			WHERE ISDATE([P-REVER-DT]) = 1 
			AND [P-REVER-DT] >= '9899-12-31' 
			AND [P-REVER-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-ADD-DT] = '9999-12-31' 
			WHERE ISDATE([P-ADD-DT]) = 1 
			AND [P-ADD-DT] >= '9899-12-31' 
			AND [P-ADD-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-TPL-AUDIT-DT] = '9999-12-31' 
			WHERE ISDATE([P-TPL-AUDIT-DT]) = 1 
			AND [P-TPL-AUDIT-DT] >= '9899-12-31' 
			AND [P-TPL-AUDIT-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-APPL-COMPLETE-DT] = '9999-12-31' 
			WHERE ISDATE([P-APPL-COMPLETE-DT]) = 1 
			AND [P-APPL-COMPLETE-DT] >= '9899-12-31' 
			AND [P-APPL-COMPLETE-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-NABP-EFF-DT] = '9999-12-31' 
			WHERE ISDATE([P-NABP-EFF-DT]) = 1 
			AND [P-NABP-EFF-DT] >= '9899-12-31' 
			AND [P-NABP-EFF-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-DEA-EFF-DT] = '9999-12-31' 
			WHERE ISDATE([P-DEA-EFF-DT]) = 1 
			AND [P-DEA-EFF-DT] >= '9899-12-31' 
			AND [P-DEA-EFF-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-DEA-EXP-DT] = '9999-12-31' 
			WHERE ISDATE([P-DEA-EXP-DT]) = 1 
			AND [P-DEA-EXP-DT] >= '9899-12-31' 
			AND [P-DEA-EXP-DT] < '9999-12-31';

			UPDATE SRC_ProviderUpdt
			SET [P-DOB-DT] = '9999-12-31' 
			WHERE ISDATE([P-DOB-DT]) = 1 
			AND [P-DOB-DT] >= '9899-12-31' 
			AND [P-DOB-DT] < '9999-12-31';


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