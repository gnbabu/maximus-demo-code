/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_License]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_License]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_License]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_LICENSE with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_License] 
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
		FROM SRC_ProviderLicenseTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_ProviderLicenseTrkLog);


		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderLicenseUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )

		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';

		INSERT [dbo].[SRC_ProviderLicenseUpdtExcpt]
		  (REG_ID, [P-SYS-ID], [P-LIC-EFF-DT], [P-LIC-CERT-NUM], [P-LIC-CERT-CD], [P-ST-CD], [P-LIC-RSTRCT-CD],
		   [P-LIC-VRFY-IND], [P-LIC-BRD-NUM], [P-LIC-EXPIR-DT], [P-LIC-PERMIT-ID], [G-AUD-USER-ID],
		   [G-AUD-DT], [G-AUD-TM], LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT c.RegistrationID as REG_ID, 
		  b.[P-SYS-ID], b.[P-LIC-EFF-DT], b.[P-LIC-CERT-NUM], b.[P-LIC-CERT-CD], b.[P-ST-CD], b.[P-LIC-RSTRCT-CD],
		  b.[P-LIC-VRFY-IND], b.[P-LIC-BRD-NUM], b.[P-LIC-EXPIR-DT], b.[P-LIC-PERMIT-ID], b.[G-AUD-USER-ID],
		  b.[G-AUD-DT], b.[G-AUD-TM], 
		  @pin_conv_run_time as LAST_MODIFIED_DATE_TIME, dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_ProviderLicenseUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;


		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderLicenseUpdt
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
		 SELECT DISTINCT 'DELETE FROM REG_LICENSE WHERE REG_ID = ' + convert(varchar,a.REG_ID) + ';'  as Command 
		 FROM REG_LICENSE a
		 JOIN DCConv_KeyCrossReferences b on a.REG_ID = b.REGISTRATIONID
		 JOIN #IncludeList c on b.SysID = c.[P-SYS-ID]
	  

		-- Insert records
		--INSERT INTO [dbo].[REG_LICENSE]
	 --       ([REG_ID]
	 --       ,[LICENSE_TYPE_ID]
	 --       ,[LICENSE_NUMBER]
	 --       ,[LICENSE_STATE]
	 --       ,[LICENSE_EFF_DATE]
	 --       ,[LICENSE_END_DATE]
	 --       ,[MODIFIED_STATUS_TYPE_ID]
	 --       ,[LAST_MODIFIED_DATE_TIME]
	 --       ,[LAST_MODIFIED_USER])
		--SELECT map.RegistrationId AS [REG_ID],
	 --       CASE lic.[P-LIC-CERT-CD] WHEN '' THEN NULL ELSE lic.[P-LIC-CERT-CD] END AS [LICENSE_TYPE_ID],
	 --       lic.[P-LIC-CERT-NUM] AS [LICENSE_NUMBER],
	 --       lic.[P-ST-CD] AS [LICENSE_STATE],
	 --       CASE dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) WHEN '1753-01-01' THEN NULL ELSE dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) END AS [LICENSE_EFF_DATE],
	 --       dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EXPIR-DT]) AS [LICENSE_END_DATE],
	 --       1 AS [MODIFIED_STATUS_TYPE_ID],
	 --		@pin_conv_run_time AS [LAST_MODIFIED_DATE_TIME],
		--	dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		--FROM #IncludeList a		
		--INNER JOIN SRC_ProviderLicenseUpdt lic ON a.[P-SYS-ID] = lic.[P-SYS-ID] 
		--JOIN DCConv_KeyCrossReferences map ON a.[P-SYS-ID] = map.SysID
		--WHERE lic.[EnableConversion] = 1;

		DECLARE @insrt varchar(max);
		SET @insrt = 'INSERT INTO [dbo].[REG_LICENSE] ([REG_ID],[LICENSE_TYPE_ID],[LICENSE_NUMBER],[LICENSE_STATE],[LICENSE_EFF_DATE],[LICENSE_END_DATE],'
		SET @insrt = @insrt + '[LICENSE_BOARD_NAME],';
		SET @insrt = @insrt + '[MODIFIED_STATUS_TYPE_ID],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER], IS_CONVERTED) VALUES ('

		INSERT #Commands (Command)
		SELECT @insrt + convert(varchar, map.RegistrationId) + ', ' +
			CASE lic.[P-LIC-CERT-CD] WHEN '' THEN 'NULL' ELSE '''' + lic.[P-LIC-CERT-CD] + '''' END + ', ''' +
			RTRIM(lic.[P-LIC-CERT-NUM]) + ''', ''' +
			lic.[P-ST-CD] + ''', ' +
			CASE dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]) WHEN '1753-01-01' THEN 'NULL' ELSE '''' + convert(varchar(10), dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EFF-DT]),120) + '''' END  + ', ''' +
			convert(varchar(10),dbo.fn_ConvertDCDateToPDMS(lic.[P-LIC-EXPIR-DT]),120) + ''', ''' +
			lic.[P-LIC-BRD-NUM] + ''',' + 
			'1, ''' +
			convert(varchar, @pin_conv_run_time,120) + ''', ''' +
			convert(varchar(max), dbo.fn_GetUniqueGUID(2)) + ''',1) ;' as Command
		FROM #IncludeList a		
		INNER JOIN SRC_ProviderLicenseUpdt lic ON a.[P-SYS-ID] = lic.[P-SYS-ID] 
		JOIN DCConv_KeyCrossReferences map ON a.[P-SYS-ID] = map.SysID
		WHERE lic.[EnableConversion] = 1;

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
		INSERT INTO SRC_ProviderLicenseTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max(CONCAT([G-AUD-DT],' ',[G-AUD-TM])) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_ProviderLicenseUpdt

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
        (@pin_conv_run_id,'SRC_ProviderLicenseUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO