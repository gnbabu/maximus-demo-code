/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateREG_CONVERTED_DOCUMENT_XREF]   Script Date: 8/1/2016  ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateREG_CONVERTED_DOCUMENT_XREF]','P') is not null
DROP PROCEDURE [dbo].[DCConv_LkUp_PopulateREG_CONVERTED_DOCUMENT_XREF]
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
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateREG_CONVERTED_DOCUMENT_XREF] 
(
	@pin_run_reference_time datetime
)
AS
BEGIN

	SET NOCOUNT ON;

	TRUNCATE TABLE dbo.REG_CONVERTED_DOCUMENT_XREF;

	INSERT INTO dbo.REG_CONVERTED_DOCUMENT_XREF
	 (REG_ID, DOCUMENT_ID, DOC_TYPE, DTL_DOC_TYPE, SUB_DOC_TYPE, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER)
	SELECT DISTINCT ISNULL(c.REG_ID,0) as REG_ID, a.DOCUMENT_ID, b.DOC_TYPE, b.DTL_DOC_TYPE, b.SUB_DOC_TYPE, 
		 @pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME], 
		 dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
	FROM dbo.DOCUMENT a
	JOIN SRC_Documents b on a.[FILE_NAME] = b.[FILE_NM]
	LEFT JOIN dbo.REG_SERVICE_LOCATION c on b.[PROV_ID] = c.[MEDICAID_ID]
	WHERE a.IS_CONVERSION = 1;

END

GO

