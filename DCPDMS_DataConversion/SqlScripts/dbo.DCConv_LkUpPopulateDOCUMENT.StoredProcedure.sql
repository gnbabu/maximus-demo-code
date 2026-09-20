/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateDOCUMENT]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateDOCUMENT]','P') is not null
DROP PROCEDURE [dbo].[DCConv_LkUp_PopulateDOCUMENT]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Andrew Zovistoski
-- Create date: 08/05/2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateDOCUMENT] 
(
	@pin_run_reference_time datetime
)
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @reseed int;

	DELETE FROM dbo.DOCUMENT WHERE IS_CONVERSION = 1;

	SELECT @reseed = MAX(DOCUMENT_ID) FROM dbo.DOCUMENT;

	--DBCC CHECKIDENT('DOCUMENT', RESEED, @reseed) WITH NO_INFOMSGS;
	DBCC CHECKIDENT('DOCUMENT', RESEED, 1) WITH NO_INFOMSGS;

	INSERT INTO dbo.DOCUMENT
	 ([NAME], [FILE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER], [IS_CONVERSION])
	SELECT FILE_NM as [NAME], FILE_NM as [FILE_NM], 
	 @pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME], 
	 dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER],
	 1 as [IS_CONVERSION]
	FROM SRC_Documents 

	
END
GO