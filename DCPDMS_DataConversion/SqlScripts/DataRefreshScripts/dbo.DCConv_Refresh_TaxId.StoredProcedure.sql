/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_TaxId]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_TaxId]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_TaxId]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_TaxId with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_TaxId] 
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
		FROM SRC_ProviderTaxIdTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_ProviderTaxIdTrkLog);


		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderTaxIdUpdt
		  WHERE CONCAT([G-AUD-DT],' ',[G-AUD-TM]) > @LastAuditDT
		  )
		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';

		INSERT [dbo].[SRC_ProviderTaxIdUpdtExcpt]
		  (REG_ID, [P-SYS-ID], [P-TAX-BEG-DT], [P-TAX-END-DT], [P-FED-ID-IND], [P-FED-TAX-ID], [P-SSN-NUM],
		   [G-AUD-USER-ID],[G-AUD-DT], [G-AUD-TM], LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT c.RegistrationID as REG_ID, 
		  b.[P-SYS-ID], b.[P-TAX-BEG-DT], b.[P-TAX-END-DT], b.[P-FED-ID-IND], b.[P-FED-TAX-ID], b.[P-SSN-NUM],
		  b.[G-AUD-USER-ID],b.[G-AUD-DT], b.[G-AUD-TM], 
		  @pin_conv_run_time as LAST_MODIFIED_DATE_TIME, dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_ProviderTaxIdUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;


		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_ProviderTaxIdUpdt
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


		 -- Generate TAXID update commands
		 -- Create SQL commands rather than use a single delete statement
		 IF OBJECT_ID('tempdb..#taxIdsToUse') IS NOT NULL
			DROP TABLE #taxIdsToUse;

		 SELECT [P-SYS-ID], max([P-TAX-END-DT]) as [P-TAX-END-DT]
		 INTO #taxIdsToUse
		 FROM SRC_ProviderTaxIdUpdt
		 WHERE EnableConversion = 1
		 GROUP BY [P-SYS-ID]

		 -- create tax_id update
		 INSERT #Commands (Command)
		 SELECT DISTINCT 'UPDATE REG_PROVIDER SET [TAX_ID] = ''' + 
		 CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000')  
			THEN prov.[P-SSN-NUM] 
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000') 
			THEN tax.[P-FED-TAX-ID]
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') 
			THEN prov.[P-SSN-NUM]
 		 END + ''', [TAX_ID_TYPE_ID] = ' + 
		 CASE WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000')  
			THEN '15'
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000') 
			THEN '16'
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') 
			THEN '15'
		END +
		' WHERE REG_ID = ' + CAST(map.RegistrationID AS varchar(20))
		FROM REG_PROVIDER reg
		INNER JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN #taxIdsToUse taxAgg on map.SysID = taxAgg.[P-SYS-ID]
		INNER JOIN SRC_ProviderTaxIdUpdt tax ON tax.[P-SYS-ID] = taxAgg.[P-SYS-ID] AND tax.[P-TAX-END-DT] = taxAgg.[P-TAX-END-DT]  		
		INNER JOIN #IncludeList inc on map.SysID = inc.[P-SYS-ID]
		WHERE tax.[EnableConversion] = 1;

		-- create alt tax id update
		INSERT INTO #Commands (Command)
		SELECT DISTINCT 'UPDATE REG_PROVIDER SET [ALT_TAX_ID] = ''' + 
			CASE 
			WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000')  
				THEN tax.[P-FED-TAX-ID] 
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') AND reg.[TAX_ID_TYPE_ID] <> 15
			  THEN prov.[P-SSN-NUM] 
			END + 
			''', [ALT_TAX_ID_TYPE] = ' +
			CASE 
			WHEN reg.[ENTITY_TYPE_ID] = 1 AND RTRIM(tax.[P-FED-TAX-ID]) NOT IN ('','000000000')  
				THEN '16' 
			WHEN reg.[ENTITY_TYPE_ID] <> 1 AND RTRIM(prov.[P-SSN-NUM]) NOT IN ('','000000000') AND reg.[TAX_ID_TYPE_ID] <> 15
				THEN '15'
			END +
		 ' WHERE REG_ID = ' + CAST(map.RegistrationID AS varchar(20))
		FROM REG_PROVIDER reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.RegistrationID 
		JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		JOIN #taxIdsToUse taxAgg on map.SysID = taxAgg.[P-SYS-ID]
		JOIN SRC_ProviderTaxIdUpdt tax ON tax.[P-SYS-ID] = taxAgg.[P-SYS-ID] AND tax.[P-TAX-END-DT] = taxAgg.[P-TAX-END-DT]  		
		INNER JOIN #IncludeList inc on map.SysID = inc.[P-SYS-ID]
		WHERE tax.[EnableConversion] = 1;


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
		INSERT INTO SRC_ProviderTaxIdTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max(CONCAT([G-AUD-DT],' ',[G-AUD-TM])) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_ProviderTaxIdUpdt

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
        (@pin_conv_run_id,'SRC_ProviderTaxIdUpdt','', @ErrorLine, @ErrorNumber, @ErrorMessage, @ErrorProcedure);
		
		-- this will be a fatal error
		RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
	END CATCH
END
GO