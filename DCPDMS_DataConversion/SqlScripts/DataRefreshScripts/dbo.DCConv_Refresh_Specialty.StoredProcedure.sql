/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_Specialty]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_Specialty]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_Specialty]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_SPECIALTY with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_Specialty] 
(
    @fileName varchar(25),
	@pin_conv_run_time datetime,
	@pin_conv_run_id varchar(20)
)
AS
BEGIN
	SET NOCOUNT ON;

	BEGIN TRY
		BEGIN TRANSACTION

		-- Get list of exceptions that we can't handle because the provider has been in PDMS 
		DECLARE @LastAuditDT varchar(20) = '2017-01-03 12.52.42'

		SELECT @LastAuditDT = LastAuditDT 
		FROM SRC_ProviderSpecialtyTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_ProviderSpecialtyTrkLog);

		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderSpecialtyUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )
		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';

		INSERT [dbo].[SRC_ProviderSpecialtyUpdtExcpt] (
			[REG_ID], 			
			[P-SYS-ID],
			[P-SPECL-BEG-DT],
			[P-SPECL-CD],
			[P-SPECL-END-DT],
			[P-LIC-CERT-NUM],
			[P-ST-CD],
			[P-LIC-BRD-NUM],
			[G-AUD-USER-ID],
			[G-AUD-DT],
			[G-AUD-TM], 
			[LAST_MODIFIED_DATE_TIME], 
			[LAST_MODIFIED_USER]
		)
		SELECT 
			c.RegistrationID as REG_ID, 
			b.[P-SYS-ID],
			b.[P-SPECL-BEG-DT],
			b.[P-SPECL-CD],
			b.[P-SPECL-END-DT],
			b.[P-LIC-CERT-NUM],
			b.[P-ST-CD],
			b.[P-LIC-BRD-NUM],
			b.[G-AUD-USER-ID],
			b.[G-AUD-DT],
			b.[G-AUD-TM], 
			@pin_conv_run_time as LAST_MODIFIED_DATE_TIME, 
			dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_ProviderSpecialtyUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;


		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderSpecialtyUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )
		SELECT a.[P-SYS-ID]
		INTO #IncludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		LEFT JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID AND c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897'
		WHERE c.REG_ID IS NULL;

		IF OBJECT_ID('tempdb..#Commands') IS NOT NULL
		DROP TABLE #Commands;

		CREATE TABLE #Commands (CmdID int identity(1,1), Command varchar(max));

		INSERT #Commands (Command)
		SELECT 'BEGIN TRY' as Command;
		INSERT #Commands (Command)
		SELECT 'BEGIN TRANSACTION' as Command;

		 -- Delete records
		 -- Create SQL commands rather than use a single delete statement
		 INSERT #Commands (Command)
		 SELECT DISTINCT 'DELETE FROM REG_SPECIALTY WHERE REG_ID = ' + convert(varchar,a.REG_ID) + ';'  as Command 
		 FROM REG_SPECIALTY a
		 JOIN DCConv_KeyCrossReferences b on a.REG_ID = b.REGISTRATIONID
		 JOIN #IncludeList c on b.SysID = c.[P-SYS-ID]

   
		DECLARE @insrt varchar(max);
		SET @insrt = 'INSERT INTO [dbo].[REG_SPECIALTY] ([REG_ID],[PRIMARY_FLAG],[SPECIALTY_TYPE_ID],[SPECIALTY_BOARD_CERTIFIED],';
		SET @insrt = @insrt + '[MODIFIED_STATUS_TYPE_ID],[START_DATE],[END_DATE],[LAST_MODIFIED_DATE_TIME],';
        SET @insrt = @insrt + '[LAST_MODIFIED_USER],[REG_TAXONOMY_ID],[SPECIALTY_BOARD_STATE],[SPECIALTY_BOARD_NAME]) VALUES (';
		
		INSERT INTO #Commands
		SELECT DISTINCT 
			@insrt + CAST(map.RegistrationId AS varchar(50)) + ',' +
			'0,' + 
			CAST(tt.SPECIALTY_TYPE_ID AS varchar(20)) + ',' + 
			'''N'',' +
			'1,' +
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-BEG-DT]), 101) + ''',' +
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(specl.[P-SPECL-END-DT]), 101) + ''',' +
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(@pin_conv_run_time), 101) + ''',' +
			'''' + CAST(dbo.fn_GetUniqueGUID(2) AS varchar(40)) + ''',' +
			'NULL,' +
			'''' + specl.[P-ST-CD] + ''',' +
			'''' + specl.[P-LIC-BRD-NUM] + ''');' AS Command
		FROM #IncludeList incList		
		INNER JOIN DCConv_KeyCrossReferences map ON map.SysID = incList.[P-SYS-ID]
		INNER JOIN SRC_ProviderSpecialtyUpdt specl ON specl.[P-SYS-ID] = map.[SysId]
		INNER JOIN REG_PROVIDER prov ON prov.[REG_ID] = map.[REGISTRATIONID]
		INNER JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(specl.[P-SPECL-CD]))) AND tt.PROVIDER_TYPE_ID = prov.PROVIDER_TYPE_ID
		WHERE tt.SPECIALTY_TYPE_ID IS NOT NULL
		AND specl.[EnableConversion] = 1;

		-- add command to set the primary specialty and fix null specialty start dates
		INSERT INTO #Commands (Command)
		SELECT 'WITH lastEnd as (SELECT REG_ID, MAX(ISNULL(END_DATE,''9999-12-31'')) as END_DATE ' +
		'FROM REG_SPECIALTY GROUP BY REG_ID) ' +
		'UPDATE REG_SPECIALTY ' +
		'SET PRIMARY_FLAG = 1 ' +
		'FROM REG_SPECIALTY rs '+
		'JOIN (	SELECT minID.REG_ID, min(minID.REG_SPECIALTY_ID) as REG_SPECIALTY_ID ' +
		'		FROM REG_SPECIALTY minID ' +
		'		JOIN lastEnd ON minID.REG_ID = lastEnd.REG_ID AND ISNULL(minID.END_DATE,''9999-12-31'') = lastEnd.END_DATE ' +
		'		GROUP BY minID.REG_ID) sub ' +
		'ON rs.REG_SPECIALTY_ID = sub.REG_SPECIALTY_ID; ' +
		'UPDATE REG_SPECIALTY ' +
		'SET [START_DATE] = reg.CHANGE_EFFECTIVE_DATE ' +
		'FROM REG_SPECIALTY rs ' +
		'JOIN DCConv_KeyCrossReferences map on rs.REG_ID = map.RegistrationID ' +
		'JOIN REGISTRATION reg on map.RegistrationID = reg.REG_ID ' +
		'WHERE rs.START_DATE = ''1753-01-01''';
		INSERT #Commands (Command)
		SELECT 'COMMIT' as Command;

		INSERT #Commands (Command)
		SELECT 'END TRY' as Command;

		INSERT #Commands (Command)
		SELECT 'BEGIN CATCH' as Command;

		INSERT #Commands (Command)
		SELECT 'PRINT ERROR_MESSAGE();';

		INSERT #Commands (Command)
		SELECT 'ROLLBACK' as Command;

		INSERT #Commands (Command)
		SELECT 'END CATCH' as Command;


		-- Log the load process
		INSERT INTO SRC_ProviderSpecialtyTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max(CONCAT([G-AUD-DT],' ',[G-AUD-TM])) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_ProviderSpecialtyUpdt

		SELECT Command FROM #Commands order by CmdID

		COMMIT ;
	END TRY
	BEGIN CATCH
		ROLLBACK ;
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
        (@pin_conv_run_id,'SRC_ProviderSpecialtyUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO