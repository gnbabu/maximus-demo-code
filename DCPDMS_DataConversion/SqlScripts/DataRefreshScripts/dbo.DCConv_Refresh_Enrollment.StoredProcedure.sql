/****** Object:  StoredProcedure [dbo].[DCConv_Refresh_Enroll]    Script Date: 2/1/2017  ******/
IF object_id('[dbo].[DCConv_Refresh_Enroll]','P') is not null
DROP PROCEDURE [dbo].[DCConv_Refresh_Enroll]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 2/1/2017
-- Description:	Refreshes REG_ENROLLMENT with update from MMIS
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_Refresh_Enroll] 
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
		DECLARE @LastAuditDT varchar(26) = '2017-01-03-12.52.42.000000'  ;
		

		SELECT @LastAuditDT = LastAuditDT 
		FROM SRC_EnrollmentTrkLog
		WHERE TrackingLogID = (SELECT max(TrackingLogID) FROM SRC_EnrollmentTrkLog);


		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_EnrollmentUpdt
		  WHERE [G-AUD-TS] > @LastAuditDT
		  )

		SELECT a.[P-SYS-ID], c.REG_ID
		INTO #ExcludeList
		FROM updtList a
		JOIN DCConv_KeyCrossReferences b on a.[P-SYS-ID] = b.SysID
		JOIN REGISTRATION_USER_XREF c on b.RegistrationID = c.REG_ID
		WHERE c.LAST_MODIFIED_DATE_TIME > '2017-01-03 12:52:42.897';

		INSERT [dbo].[SRC_EnrollmentUpdtExcpt]
		  (REG_ID, [P-SYS-ID], [P-STAT-EFF-DT],	[P-TY-CD], [P-STAT-END-DT],	[P-ENROL-STAT-TY-CD],
			[G-AUD-USER-ID], [G-AUD-TS], [P-BRND-DISCT-PCT], [P-GENR-DISCT-PCT], [P-DISP-FEE-AMT],
			[P-RE-ENROL-STAT-CD], [P-ENROL-APP-SRC-CD], LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT c.RegistrationID as REG_ID, 
		  b.[P-SYS-ID], [P-STAT-EFF-DT], b.[P-TY-CD], b.[P-STAT-END-DT], b.[P-ENROL-STAT-TY-CD],
		  b.[G-AUD-USER-ID], b.[G-AUD-TS], b.[P-BRND-DISCT-PCT], b.[P-GENR-DISCT-PCT], b.[P-DISP-FEE-AMT],
		  b.[P-RE-ENROL-STAT-CD], b.[P-ENROL-APP-SRC-CD], 
		  @pin_conv_run_time as LAST_MODIFIED_DATE_TIME, dbo.fn_GetUniqueGUID(2) as LAST_MODIFIED_USER
		FROM #ExcludeList a
		JOIN SRC_EnrollmentUpdt b on a.[P-SYS-ID] = b.[P-SYS-ID]
		JOIN DCConv_KeyCrossReferences c on a.[P-SYS-ID] = c.SysID;


		-- Process new records
		WITH updtList as (
		  SELECT DISTINCT [P-SYS-ID]
		  FROM SRC_EnrollmentUpdt
		  WHERE [G-AUD-TS] > @LastAuditDT
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

		-- Update DCConv_KeyCrossReferences
		IF OBJECT_ID('tempdb..#lastEnroll') IS NOT NULL
		 DROP TABLE #lastEnroll;
		CREATE TABLE #lastEnroll ([P-SYS-ID] int, [P-STAT-EFF-DT] varchar(10));

		INSERT INTO #lastEnroll
		SELECT en.[P-SYS-ID], max(en.[P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		FROM SRC_EnrollmentUpdt en 
		JOIN #IncludeList c on en.[P-SYS-ID] = c.[P-SYS-ID]
		WHERE en.EnableConversion = 1 
		AND en.[P-ENROL-STAT-TY-CD] <> '52'  
		GROUP BY en.[P-SYS-ID];

		--UPDATE DCConv_KeyCrossReferences
		--SET EnrollmentStartDate = en.[P-STAT-EFF-DT]
		--FROM DCConv_KeyCrossReferences map
		--JOIN #lastEnroll en on map.SysID = en.[P-SYS-ID]
		--INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		--WHERE prov.[P-REC-TY-CD] = 'P' 
		--AND prov.[P-INDIV-GRP-CD] in ('I','G','B')
		--AND prov.[EnableConversion] = 1
		SELECT map.SysID, map.REGISTRATIONID, dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) as EnrollmentStartDate
		INTO #UpdateDCConv_KeyCrossReferences
		FROM DCConv_KeyCrossReferences map
		JOIN #lastEnroll en on map.SysID = en.[P-SYS-ID]
		INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = en.[P-SYS-ID]
		WHERE prov.[P-REC-TY-CD] = 'P' 
		AND prov.[P-INDIV-GRP-CD] in ('I','G','B')
		AND prov.[EnableConversion] = 1


		INSERT #Commands (Command)
		SELECT 'UPDATE DCConv_KeyCrossReferences SET EnrollmentStartDate = ''' + convert(varchar(10),EnrollmentStartDate,120) + ''' WHERE REGISTRATIONID = ' + convert(varchar,REGISTRATIONID) + ';' as Command
		FROM #UpdateDCConv_KeyCrossReferences;


		-- Update REGISTRATION
		WITH lastEnroll AS (
		 SELECT [P-SYS-ID], max([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
		 FROM SRC_EnrollmentUpdt 
		 WHERE [P-ENROL-STAT-TY-CD] <> '52'
		 AND [EnableConversion] = 1
		 GROUP BY [P-SYS-ID]
		)
		--UPDATE REGISTRATION
		--SET [REQUESTED_EFFECTIVE_DATE] = dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) ,
		-- [CHANGE_EFFECTIVE_DATE] = dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]),
		-- [LAST_MODIFIED_DATE_TIME] = @pin_conv_run_time, [LAST_MODIFIED_USER] = dbo.fn_GetUniqueGUID(2)
		--FROM REGISTRATION reg
		--JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.REGISTRATIONID
		--INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		--INNER JOIN lastEnroll en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
		--JOIN #IncludeList c on map.SysID = c.[P-SYS-ID]
		--WHERE prov.[EnableConversion] = 1;
		SELECT  reg.REG_ID, dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) as REQUESTED_EFFECTIVE_DATE,
		 dbo.fn_ConvertDCDateToPDMS(en.[P-STAT-EFF-DT]) as CHANGE_EFFECTIVE_DATE
		INTO #UpdateREGISTRATION
		FROM REGISTRATION reg
		JOIN DCConv_KeyCrossReferences map ON reg.REG_ID = map.REGISTRATIONID
		INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN lastEnroll en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
		JOIN #IncludeList c on map.SysID = c.[P-SYS-ID]
		WHERE prov.[EnableConversion] = 1;


		INSERT #Commands (Command)
		SELECT DISTINCT 'UPDATE REGISTRATION SET REQUESTED_EFFECTIVE_DATE = ''' + convert(varchar(10),REQUESTED_EFFECTIVE_DATE,120) + '''' 
		 + ', CHANGE_EFFECTIVE_DATE = ''' + convert(varchar(10),CHANGE_EFFECTIVE_DATE, 120) 
		 + ''', LAST_MODIFIED_DATE_TIME = ''' +  convert(varchar,@pin_conv_run_time,120) 
		 + ''', LAST_MODIFIED_USER = ''' + convert(varchar(36),dbo.fn_GetUniqueGUID(2))
		 + ''' WHERE REG_ID = ' + convert(varchar, REG_ID) as Command
		from #UpdateREGISTRATION


		-- Delete REG_ENROLLMENT
		INSERT #Commands (Command)
		SELECT DISTINCT 'DELETE FROM REG_ENROLLMENT WHERE REG_ID = ' + convert(varchar,a.REG_ID) as Command
		FROM REG_ENROLLMENT a
		JOIN DCConv_KeyCrossReferences b on a.REG_ID = b.REGISTRATIONID
		JOIN #IncludeList c on b.SysID = c.[P-SYS-ID]

		-- Insert REG_ENROLLMENT
		--INSERT INTO dbo.REG_ENROLLMENT
		--(REG_ID, ENROLLMENT_STATUS_CODE, ENROLL_START_DATE_TIME, ENROLL_END_DATE_TIME, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
		SELECT
		  map.RegistrationID as REG_ID, en.[P-ENROL-STAT-TY-CD] as ENROLLMENT_STATUS_CODE,
		  en.[P-STAT-EFF-DT] as ENROLL_START_DATE_TIME, en.[P-STAT-END-DT] as ENROLL_END_DATE_TIME, 
		  @pin_conv_run_time AS LAST_MODIFIED_DATE_TIME, 
		  dbo.fn_GetUniqueGUID(2) AS LAST_MODIFIED_USER
		into #InsertREG_ENROLLMENT
		FROM SRC_EnrollmentUpdt en 
		INNER JOIN DCConv_KeyCrossReferences map
		ON en.[P-SYS-ID] = map.SysID 
		--INNER JOIN dbo.REGISTRATION rg 	ON map.RegistrationID = rg.REG_ID
		JOIN #UpdateREGISTRATION rg  ON map.RegistrationID = rg.REG_ID
		JOIN #IncludeList c on map.SysID = c.[P-SYS-ID] 
		WHERE en.EnableConversion = 1 
		AND en.[P-ENROL-STAT-TY-CD] <> '52'
		AND en.[P-STAT-EFF-DT] <> convert(varchar(10),rg.REQUESTED_EFFECTIVE_DATE,20);


		DECLARE @insrt varchar(MAX);
		SET @insrt = 'INSERT INTO dbo.REG_ENROLLMENT (REG_ID, ENROLLMENT_STATUS_CODE, ENROLL_START_DATE_TIME, ENROLL_END_DATE_TIME, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER) VALUES ('

		INSERT #Commands (Command)
		SELECT @insrt + convert(varchar,REG_ID) + ',''' 
		 + ENROLLMENT_STATUS_CODE + ''','''
		 + ENROLL_START_DATE_TIME + ''','''
		 + ENROLL_END_DATE_TIME + ''','''
		 + convert(varchar(20),LAST_MODIFIED_DATE_TIME,120)  + ''','''
		 + convert(varchar(36),LAST_MODIFIED_USER) + ''')' as Command
		FROM #InsertREG_ENROLLMENT;
	


		-- Update REG_PROVIDER
		WITH enrollCode as (
		  SELECT enr.[P-SYS-ID], 
		   enr.[P-ENROL-STAT-TY-CD] as ENROLLMENT_STATUS_CODE,
		   dbo.fn_ConvertDCDateToPDMS(enr.[P-STAT-END-DT]) AS END_DATE
		  FROM SRC_EnrollmentUpdt enr
		  JOIN (SELECT [P-SYS-ID], MAX([P-STAT-EFF-DT]) as [P-STAT-EFF-DT]
				FROM SRC_EnrollmentUpdt
				WHERE EnableConversion = 1
				GROUP BY [P-SYS-ID]) sub
		  ON enr.[P-SYS-ID] = sub.[P-SYS-ID] and enr.[P-STAT-EFF-DT] = sub.[P-STAT-EFF-DT]
		 )
		--UPDATE REG_PROVIDER 
		--SET ENROLLMENT_STATUS_CODE = ec.ENROLLMENT_STATUS_CODE,
		--    END_DATE = ec.END_DATE,
		--    APPLICATION_TYPE_ID = dbo.fn_ConvertProviderApplicationTypeToPDMS(en.[P-TY-CD], en.[P-ENROL-STAT-TY-CD])   -- SKIP THIS ONE!!
		--FROM REG_PROVIDER reg
		--JOIN DCConv_KeyCrossReferences map on reg.REG_ID = map.RegistrationID
		--INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		--INNER JOIN SRC_EnrollmentUpdt en ON en.[P-SYS-ID] = map.SysID AND convert(varchar(10),map.EnrollmentStartDate,20) = en.[P-STAT-EFF-DT]
		--JOIN enrollCode ec ON prov.[P-SYS-ID] = ec.[P-SYS-ID]
		--JOIN #IncludeList c on map.SysID = c.[P-SYS-ID]
		--WHERE prov.[EnableConversion] = 1 and en.[EnableConversion] = 1;
		SELECT reg.REG_ID, ec.ENROLLMENT_STATUS_CODE, ec.END_DATE,
		 --dbo.fn_ConvertProviderApplicationTypeToPDMS(en.[P-TY-CD], en.[P-ENROL-STAT-TY-CD]) as APPLICATION_TYPE_ID,  -- SKIP THIS ONE
		 @pin_conv_run_time AS LAST_MODIFIED_DATE_TIME, 
		  dbo.fn_GetUniqueGUID(2) AS LAST_MODIFIED_USER
		INTO #UpdateREG_PROVIDER
		FROM REG_PROVIDER reg
		JOIN #UpdateDCConv_KeyCrossReferences map on reg.REG_ID = map.RegistrationID
		INNER JOIN SRC_ProviderUpdt prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_EnrollmentUpdt en ON en.[P-SYS-ID] = map.SysID AND convert(varchar(10),map.EnrollmentStartDate,20) = en.[P-STAT-EFF-DT]
		JOIN enrollCode ec ON prov.[P-SYS-ID] = ec.[P-SYS-ID]
		JOIN #IncludeList c on map.SysID = c.[P-SYS-ID]
		WHERE prov.[EnableConversion] = 1 and en.[EnableConversion] = 1;


		INSERT #Commands (Command)
		SELECT DISTINCT 'UPDATE REG_PROVIDER SET ENROLLMENT_STATUS_CODE =  ''' + convert(varchar,ENROLLMENT_STATUS_CODE) + '''' 
		 + ', END_DATE = ''' + convert(varchar(10),END_DATE, 120) 
		-- + ''', APPLICATION_TYPE_ID = ''' +  convert(varchar,APPLICATION_TYPE_ID)  -- SKIP THIS ONE 
		 + ''', LAST_MODIFIED_DATE_TIME = ''' +  convert(varchar,LAST_MODIFIED_DATE_TIME,120) 
		 + ''', LAST_MODIFIED_USER = ''' + convert(varchar(36),LAST_MODIFIED_USER)
		 + ''' WHERE REG_ID = ' + convert(varchar, REG_ID) as Command 
		 FROM #UpdateREG_PROVIDER


		-- REG_MEDICAID?  SKIP THIS ONE


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
		INSERT INTO SRC_EnrollmentTrkLog (ExtractFileName, LastAuditDT, RunDT)
		SELECT @fileName as ExtractFileName, max([G-AUD-TS]) as LastAuditDT, @pin_conv_run_time as RunDT
		FROM SRC_EnrollmentUpdt

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