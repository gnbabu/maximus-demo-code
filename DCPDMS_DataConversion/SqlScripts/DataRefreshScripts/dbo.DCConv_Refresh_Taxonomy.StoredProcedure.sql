/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_Taxonomy]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_Taxonomy]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_Taxonomy]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_TAXONOMY with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_Taxonomy] 
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
		FROM SRC_ProviderTaxonomyTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_ProviderTaxonomyTrkLog);

		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderTaxonomyUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )
		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';
		INSERT [dbo].[SRC_ProviderTaxonomyUpdtExcpt] (
			[REG_ID], 			
			[P-SYS-ID],
			[P-TAXONOMY-CD],
			[P-TAXON-BEG-DT],
			[P-TAXON-END-DT],
			[G-AUD-USER-ID],
			[G-AUD-DT],
			[G-AUD-TM],
			[LAST_MODIFIED_DATE_TIME], 
			[LAST_MODIFIED_USER]
		)
		SELECT 
			c.RegistrationID as REG_ID, 
			b.[P-SYS-ID],
			b.[P-TAXONOMY-CD],
			b.[P-TAXON-BEG-DT],
			b.[P-TAXON-END-DT],
			b.[G-AUD-USER-ID],
			b.[G-AUD-DT],
			b.[G-AUD-TM], 
			@pin_conv_run_time as LAST_MODIFIED_DATE_TIME, 
			dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_ProviderTaxonomyUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;

		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderTaxonomyUpdt
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
		IF (NOT EXISTS(SELECT TAXONOMY_TYPE_ID FROM TAXONOMY_TYPE WHERE PROVIDER_TYPE_ID = 125 AND TAXONOMY_CODE = '332B00000X'))
		BEGIN
			INSERT INTO [dbo].[TAXONOMY_TYPE] ([SPECIALTY_TYPE_ID],[PROVIDER_TYPE_ID],[TAXONOMY_CODE],[TAXONOMY_NAME],[EXPIRATION_DATE],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER],[MMIS_SPECIALTY_TYPE_ID],[NPI_REQUIRED]) 
			VALUES 
			(0, 125, '332B00000X', 'Durable Medical Equipment','12/31/9999',dbo.fn_ConvertDCDateToPDMS(@pin_conv_run_time),dbo.fn_GetUniqueGUID(2), 0, 1);
		END
		 -- Delete records
		 -- Create SQL commands rather than use a single delete statement
		 INSERT #Commands (Command)
		 SELECT DISTINCT 'DELETE FROM REG_TAXONOMY WHERE REG_ID = ' + convert(varchar,a.REG_ID) + ';'  as Command 
		 FROM REG_TAXONOMY a
		 JOIN DCConv_KeyCrossReferences b on a.REG_ID = b.REGISTRATIONID
		 JOIN #IncludeList c on b.SysID = c.[P-SYS-ID]
	  
		DECLARE @insrt varchar(max);
		SET @insrt = 'INSERT INTO [dbo].[REG_TAXONOMY] ([REG_ID],[PRIMARY_FLAG],[TAXONOMY_TYPE_ID],[MODIFIED_STATUS_TYPE_ID],[START_DATE],';
		SET @insrt = @insrt + '[END_DATE],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER]) VALUES (';
	
		INSERT INTO #Commands
		SELECT DISTINCT 
			@insrt + CAST(map.RegistrationId AS varchar(50)) + ',' +
			'0,' + 
			CAST(tt.TAXONOMY_TYPE_ID AS varchar(20)) + ',' +
			'1,' + 
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-BEG-DT]), 101) + ''',' + 
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(tax.[P-TAXON-END-DT]), 101) + ''', ' +
			'''' + CONVERT(varchar(10), dbo.fn_ConvertDCDateToPDMS(@pin_conv_run_time), 101) + ''',' +
			'''' + CAST(dbo.fn_GetUniqueGUID(2) AS varchar(40)) + ''');' 
		 AS Command
		FROM #IncludeList incList		
		INNER JOIN DCConv_KeyCrossReferences map ON map.SysID = incList.[P-SYS-ID]
		INNER JOIN SRC_ProviderTaxonomyUpdt tax ON tax.[P-SYS-ID] = map.[SysID]
		INNER JOIN REG_PROVIDER prov ON prov.[REG_ID] = map.[RegistrationID]
		JOIN dbo.TAXONOMY_TYPE tt ON RTRIM(LTRIM(UPPER(tt.TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND tt.PROVIDER_TYPE_ID = prov.[PROVIDER_TYPE_ID]
		WHERE EXISTS(SELECT REG_ID FROM dbo.REGISTRATION WHERE REG_ID =  map.RegistrationID) AND
		(SELECT TAXONOMY_TYPE_ID FROM dbo.TAXONOMY_TYPE WHERE RTRIM(LTRIM(UPPER(TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(tax.[P-TAXONOMY-CD]))) AND PROVIDER_TYPE_ID = prov.[PROVIDER_TYPE_ID]) IS NOT NULL AND
		tax.[EnableConversion] = 1;

		-- now set primary flag
		INSERT INTO #Commands (Command)
		SELECT 	'WITH lastEnd as (SELECT REG_ID, MAX(ISNULL(END_DATE,''9999-12-31'')) as END_DATE	FROM REG_TAXONOMY GROUP BY REG_ID)' +
		'UPDATE REG_TAXONOMY SET PRIMARY_FLAG = 1 FROM REG_TAXONOMY rs ' + 
		'JOIN (	SELECT minID.REG_ID, min(minID.REG_TAXONOMY_ID) as REG_TAXONOMY_ID FROM REG_TAXONOMY minID JOIN lastEnd ON minID.REG_ID = lastEnd.REG_ID AND ISNULL(minID.END_DATE,''9999-12-31'') = lastEnd.END_DATE GROUP BY minID.REG_ID) sub ' +
		'ON rs.REG_TAXONOMY_ID = sub.REG_TAXONOMY_ID; ';

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
		INSERT INTO SRC_ProviderTaxonomyTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max(CONCAT([G-AUD-DT],' ',[G-AUD-TM])) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_ProviderTaxonomyUpdt

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
        (@pin_conv_run_id,'SRC_ProviderTaxonomyUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO