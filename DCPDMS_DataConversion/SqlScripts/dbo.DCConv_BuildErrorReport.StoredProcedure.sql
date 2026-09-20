/****** Object:  StoredProcedure [dbo].[DCConv_BuildErrorReport]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_BuildErrorReport]','P') is not null
DROP PROCEDURE [dbo].DCConv_BuildErrorReport
GO
/****** Object:  StoredProcedure [dbo].[DCConv_BuildErrorReport]    Script Date: 8/1/2016 3:13:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 7/15/2016
-- Description:	Builds the data for the conversion error report
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_BuildErrorReport] 
	@pin_conv_run_id varchar(20), 
	@pin_conv_run_time datetime,
	@pin_conv_data_export_date datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @numberConverted int, @numberFailed int;

	DELETE FROM [dbo].[DCConv_ConversionErrors] WHERE RunID = @pin_conv_run_id;
	DELETE FROM [dbo].[DCConv_Counts] WHERE ConversionRunID = @pin_conv_run_id;

	INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
    SELECT DISTINCT @pin_conv_run_id,'PENROLTB',
	CASE 
		WHEN en.EnableConversion = 0 THEN 'PENROLTB'
		WHEN prov.EnableConversion = 0 THEN 'PROVDRTB'
	END, 
	CASE 
		WHEN en.EnableConversion = 0 THEN en.[P-SYS-ID]
		WHEN prov.EnableConversion = 0 THEN prov.[P-SYS-ID]
	END,en.LineID,
	CASE 
		WHEN en.EnableConversion = 0 THEN en.ErrorCode
		WHEN prov.EnableConversion = 0 THEN prov.ErrorCode
	END,
	CASE 
		WHEN en.EnableConversion = 0 THEN en.ErrorMessage
		WHEN prov.EnableConversion = 0 THEN prov.ErrorMessage
	END 
	FROM DCConv_KeyCrossReferences map 
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
	WHERE prov.EnableConversion = 0 OR en.EnableConversion = 0

	 -- find the number of converted records
	 SELECT @numberConverted = COUNT(*) FROM DCConv_KeyCrossReferences map 
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = prov.[P-SYS-ID] 
		WHERE prov.EnableConversion = 1 AND en.EnableConversion = 1;
	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PENROLTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PENROLTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PROVDRTB',
	 CASE 
		WHEN prov.EnableConversion = 0 THEN 'PROVDRTB'
		WHEN primary_adr.EnableConversion = 0 THEN 'PADDRSTB'
		WHEN tax.EnableConversion = 0 THEN 'PTAXIDTB'
	 END,
	 prov.[P-SYS-ID],prov.[LineID],
	 CASE 
		WHEN prov.EnableConversion = 0 THEN prov.ErrorCode
		WHEN primary_adr.EnableConversion = 0 THEN primary_adr.ErrorCode
		WHEN tax.EnableConversion = 0 THEN tax.ErrorCode
	 END,
	 CASE 
		WHEN prov.EnableConversion = 0 THEN prov.ErrorMessage
		WHEN primary_adr.EnableConversion = 0 THEN primary_adr.ErrorMessage
		WHEN tax.EnableConversion = 0 THEN tax.ErrorMessage
	 END
  	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_ProviderAddresses primary_adr ON primary_adr.[P-SYS-ID] = map.SysID AND primary_adr.[P-ADR-TY-CD] = 'L' AND primary_adr.[G-AUD-DT] = (SELECT MAX([G-AUD-DT]) FROM SRC_ProviderAddresses WHERE [P-SYS-ID] = map.SysID AND [P-ADR-TY-CD] = 'L')
		LEFT OUTER JOIN SRC_ProviderTaxIds tax ON tax.[P-SYS-ID] = map.SysID AND dbo.fn_ConvertDCDateToPDMS(tax.[P-TAX-END-DT]) > @pin_conv_data_export_date  
	WHERE (prov.EnableConversion = 0 AND prov.ErrorMessage NOT LIKE '%DEA%') OR primary_adr.EnableConversion = 0 OR tax.EnableConversion = 0
	 
	 -- find the number of converted records
	SELECT @numberConverted = COUNT(*) FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.SysID
		INNER JOIN SRC_ProviderAddresses primary_adr ON primary_adr.[P-SYS-ID] = map.SysID AND primary_adr.[P-ADR-TY-CD] = 'L' AND primary_adr.[G-AUD-DT] = (SELECT MAX([G-AUD-DT]) FROM SRC_ProviderAddresses WHERE [P-SYS-ID] = map.SysID AND [P-ADR-TY-CD] = 'L')
		LEFT OUTER JOIN SRC_ProviderTaxIds tax ON tax.[P-SYS-ID] = map.SysID AND dbo.fn_ConvertDCDateToPDMS(tax.[P-TAX-END-DT]) > @pin_conv_data_export_date  
	WHERE prov.EnableConversion = 1 AND primary_adr.EnableConversion = 1 AND tax.EnableConversion = 1

	SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PROVDRTB';

	INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PROVDRTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)


	INSERT INTO [dbo].[DCConv_ConversionErrors]
	    (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
    SELECT DISTINCT @pin_conv_run_id,'PTAXONTB','PTAXONTB',tax.[P-SYS-ID],tax.[LineID],ErrorCode,ErrorMessage
	FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderTaxonomy tax ON tax.[P-SYS-ID] = map.[SysID]
	WHERE EnableConversion = 0

	  -- find the number of converted records
	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderTaxonomy tax ON tax.[P-SYS-ID] = map.[SysID]
	 WHERE EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PTAXONTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PTAXONTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)


	INSERT INTO [dbo].[DCConv_ConversionErrors]
	    (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PSPECLTB','PSPECLTB',[P-SYS-ID],specl.[LineID],ErrorCode,ErrorMessage
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysId]
	 WHERE EnableConversion = 0

	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderSpecialty specl ON specl.[P-SYS-ID] = map.[SysID]
	 WHERE EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PSPECLTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PSPECLTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PLICNSTB','PLICNSTB',lic.[P-SYS-ID],lic.[LineID],ErrorCode,ErrorMessage
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderLicense lic ON lic.[P-SYS-ID] = map.[SysID]
	 WHERE lic.EnableConversion = 0;

	SELECT @numberConverted = COUNT(*) FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderLicense lic ON lic.[P-SYS-ID] = map.[SysID]
	 WHERE lic.EnableConversion = 1;

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PLICNSTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PLICNSTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)
	
	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PCLIAPTB',
	 CASE
		WHEN clia.EnableConversion = 0 THEN 'PCLIAPTB'
		WHEN cliad.EnableConversion = 0 THEN 'PCLIACTB'
	 END,clia.[P-SYS-ID],clia.[LineID],
	 CASE
		WHEN clia.EnableConversion = 0 THEN clia.ErrorCode
		WHEN cliad.EnableConversion = 0 THEN cliad.ErrorCode
	 END,
	 CASE
		WHEN clia.EnableConversion = 0 THEN clia.ErrorMessage
		WHEN cliad.EnableConversion = 0 THEN cliad.ErrorMessage
	 END
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderCLIA clia ON clia.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderCLIADetails cliad ON clia.[P-CLIA-NUM] = cliad.[P-CLIA-NUM]
	 WHERE clia.EnableConversion = 0 OR cliad.EnableConversion = 0
	
	 SELECT @numberConverted = COUNT(*) FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderCLIA clia ON clia.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderCLIADetails cliad ON clia.[P-CLIA-NUM] = cliad.[P-CLIA-NUM]
	 WHERE clia.EnableConversion = 1 AND cliad.EnableConversion = 1;

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PCLIAPTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PCLIAPTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

 	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PMCARETB','PMCARETB',0,mcare.[LineID],ErrorCode,ErrorMessage
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicare mcare ON mcare.[P-SYS-ID] = map.[SysID]
	 WHERE EnableConversion = 0

	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicare mcare ON mcare.[P-SYS-ID] = map.[SysID]
	 WHERE EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PMCARETB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PMCARETB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'SERVICE_PADDRSTB',
		CASE 
			WHEN prov.EnableConversion = 0 THEN 'PROVDRTB'
			WHEN serv_adr.EnableConversion = 0 THEN 'PADDRSTB_L'
			WHEN mail_adr.EnableConversion = 0 THEN 'PADDRSTB_M'
			WHEN pay_adr.EnableConversion = 0 THEN 'PADDRSTB_B'
		END,prov.[P-SYS-ID],
		CASE 
			WHEN prov.EnableConversion = 0 THEN serv_adr.LineID
			WHEN serv_adr.EnableConversion = 0 THEN serv_adr.LineID
			WHEN mail_adr.EnableConversion = 0 THEN mail_adr.LineID
			WHEN pay_adr.EnableConversion = 0 THEN pay_adr.LineID
		END,
		CASE 
			WHEN prov.EnableConversion = 0 THEN prov.ErrorCode
			WHEN serv_adr.EnableConversion = 0 THEN serv_adr.ErrorCode
			WHEN mail_adr.EnableConversion = 0 THEN mail_adr.ErrorCode
			WHEN pay_adr.EnableConversion = 0 THEN pay_adr.ErrorCode
		END,
		CASE 
			WHEN prov.EnableConversion = 0 THEN prov.ErrorMessage
			WHEN serv_adr.EnableConversion = 0 THEN serv_adr.ErrorMessage
			WHEN mail_adr.EnableConversion = 0 THEN mail_adr.ErrorMessage
			WHEN pay_adr.EnableConversion = 0 THEN pay_adr.ErrorMessage
		END
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		LEFT OUTER JOIN SRC_ProviderAddresses serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] AND serv_adr.[P-ADR-TY-CD] = 'L' 
		LEFT OUTER JOIN SRC_ProviderAddresses mail_adr ON mail_adr.[P-SYS-ID] = map.[SysID] AND mail_adr.[P-ADR-TY-CD] = 'M' 
		LEFT OUTER JOIN SRC_ProviderAddresses pay_adr ON pay_adr.[P-SYS-ID] = map.[SysID] AND pay_adr.[P-ADR-TY-CD] = 'B' 
	 WHERE prov.EnableConversion = 0 OR serv_adr.EnableConversion = 0 OR 
		mail_adr.EnableConversion = 0 OR pay_adr.EnableConversion = 0;

	 SELECT @numberConverted = COUNT(*)
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		LEFT OUTER JOIN SRC_ProviderAddresses serv_adr ON serv_adr.[P-SYS-ID] = map.[SysID] 
	 WHERE prov.EnableConversion = 1 AND serv_adr.EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='SERVICE_PADDRSTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'SERVICE_PADDRSTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'ADDITIONAL_PADDRSTB','PADDRSTB',adr.[P-SYS-ID],adr.LineID,ErrorCode,ErrorMessage
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderAddresses adr ON adr.[P-SYS-ID] = map.[SysID]
 	 WHERE adr.[P-ADR-TY-CD] NOT IN ('B', 'L', 'M') AND
	 adr.[EnableConversion] = 0;

	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderAddresses adr ON adr.[P-SYS-ID] = map.[SysID]
 	 WHERE adr.[P-ADR-TY-CD] NOT IN ('B', 'L', 'M') AND
	 adr.[EnableConversion] = 1;

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='ADDITIONAL_PADDRSTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'ADDITIONAL_PADDRSTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'DEA','PROVDRTB',prov.[P-SYS-ID],prov.LineID,ErrorCode,ErrorMessage
	 FROM DCConv_KeyCrossReferences map
	 INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysId]
	 WHERE EnableConversion = 0 AND ErrorMessage LIKE '%DEA%';

	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
	 INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysId]
	 WHERE EnableConversion = 1;

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='DEA';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'DEA', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

 	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'PREVMCTB',
	 CASE 
		WHEN mcaid.EnableConversion = 0 THEN 'PREVMCTB'
		WHEN en.EnableConversion = 0 THEN 'PENROLTB'
	 END,mcaid.[P-SYS-ID],mcaid.LineID,
 	 CASE 
		WHEN mcaid.EnableConversion = 0 THEN mcaid.ErrorCode
		WHEN en.EnableConversion = 0 THEN en.ErrorCode
	 END,
	 CASE 
		WHEN mcaid.EnableConversion = 0 THEN mcaid.ErrorMessage
		WHEN en.EnableConversion = 0 THEN en.ErrorMessage
	 END
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicaid mcaid ON mcaid.[P-SYS-ID] = map.[SysId]
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = map.[SysID]
	 WHERE mcaid.EnableConversion = 0 OR en.EnableConversion = 0

	 SELECT @numberConverted = COUNT(*)
	 FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_ProviderMedicaid mcaid ON mcaid.[P-SYS-ID] = map.[SysId]
		INNER JOIN SRC_Enrollments en ON en.[P-SYS-ID] = map.[SysID]
	 WHERE mcaid.EnableConversion = 1 AND en.EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='PREVMCTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'PREVMCTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)

	 INSERT INTO [dbo].[DCConv_ConversionErrors]
           (RunID,[DataType],[SourceTable],[SysID],[LineID],[ErrorCode],[ErrorMessage])
     SELECT DISTINCT @pin_conv_run_id,'POWNINTB',
	 CASE 
		WHEN own.EnableConversion = 0 THEN 'POWNINTB'
		WHEN prov.EnableConversion = 0 THEN 'PROVDRTB'
	 END,
	 own.[P-SYS-ID],own.LineID,
	 CASE 
		WHEN own.EnableConversion = 0 THEN own.ErrorCode
		WHEN prov.EnableConversion = 0 THEN prov.ErrorCode
	 END,
 	 CASE 
		WHEN own.EnableConversion = 0 THEN own.ErrorMessage
		WHEN prov.EnableConversion = 0 THEN prov.ErrorMessage
	 END
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderOwner own ON own.[P-SYS-ID] = map.[SysID]
	 WHERE prov.EnableConversion = 0 OR own.EnableConversion = 0

	 SELECT @numberConverted = COUNT(*)
		FROM DCConv_KeyCrossReferences map
		INNER JOIN SRC_Providers prov ON prov.[P-SYS-ID] = map.[SysID]
		INNER JOIN SRC_ProviderOwner own ON own.[P-SYS-ID] = map.[SysID]
	 WHERE prov.EnableConversion = 1 AND own.EnableConversion = 1

	 SELECT @numberFailed = COUNT(*) FROM DCConv_ConversionErrors WHERE DataType='POWNINTB';

	 INSERT INTO [dbo].[DCConv_Counts] ([ConversionRunID],[DataTypeDescription],[NumberOfRecordsConverted],[NumberOfFailedRecords],[TotalRecordsProcessed])
		VALUES (@pin_conv_run_id,'POWNINTB', @numberConverted, @numberFailed, @numberConverted + @numberFailed)
END


GO
