/****** Object:  StoredProcedure [dbo].[Insert_DT_DOCS_STG]    Script Date: 8/5/2016  ******/
IF object_id('[dbo].[Insert_DT_DOCS_STG]','P') is not null
DROP PROCEDURE [dbo].[Insert_DT_DOCS_STG]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 08/05/2016
-- Description:	Bulk loads the DT_DOCS_STG table
-- =============================================
CREATE PROCEDURE [dbo].[Insert_DT_DOCS_STG] 
AS
BEGIN

SET NOCOUNT ON;

 CREATE TABLE #errorList
	 (
		ErrorLine varchar(max)		
	 )


 BEGIN TRY
 TRUNCATE TABLE DT_DOCS_STG;

-- Folder 1
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 1/PDMS Extract 20160515 1 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 2
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 2/PDMS Extract 20160515 2 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 3
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 3/PDMS Extract 20160515 3 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 4
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 4/PDMS Extract 20160515 4 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 5
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 5/PDMS Extract 20160515 5 Index.txt'
WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 6 
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 6/PDMS Extract 20160515 6 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 7
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 7/PDMS Extract 20160515 7 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 8
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 8/PDMS Extract 20160515 8 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 9
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 9/PDMS Extract 20160515 9 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;

-- Folder 10
 BULK INSERT DT_DOCS_STG
 FROM 'D:/ConversionData/20160515Images/PDMS Extract 20160515 10/PDMS Extract 20160515 10 Index.txt'
 WITH ( FIRSTROW = 2, FIELDTERMINATOR = '|', ROWTERMINATOR = '0x0a') ;


 UPDATE DT_DOCS_STG
 SET DTL_DOC_TYPE = NULL where DTL_DOC_TYPE = 'null'

 UPDATE DT_DOCS_STG
 SET SUB_DOC_TYPE = NULL where SUB_DOC_TYPE = 'null'
 
 END TRY
 BEGIN CATCH 
  INSERT INTO #errorList 
  SELECT 'DT_DOC_STG|' + ERROR_MESSAGE();
 END CATCH

 IF (SELECT COUNT(*) FROM #errorList) > 0
 SELECT * FROM #errorList;

END
GO