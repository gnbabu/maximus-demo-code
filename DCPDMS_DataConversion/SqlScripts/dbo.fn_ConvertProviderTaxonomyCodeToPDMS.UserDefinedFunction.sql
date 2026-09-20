/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderTaxonomyCodeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertProviderTaxonomyCodeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertProviderTaxonomyCodeToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderTaxonomyCodeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/13/2016
-- Description:	Converts a DC taxonomy code to the PDMS equivalent
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderTaxonomyCodeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_taxonomy_code varchar(10),
	@pin_sys_id int,
	@pin_data_export_date datetime
	
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	SET @Result = 0;

	SELECT @Result = TAXONOMY_TYPE_ID 
	FROM dbo.TAXONOMY_TYPE 
	WHERE 
	RTRIM(LTRIM(UPPER(TAXONOMY_CODE))) = RTRIM(LTRIM(UPPER(@pin_taxonomy_code))) AND
	PROVIDER_TYPE_ID = dbo.fn_ConvertProviderTypeToPDMS(@pin_sys_id, @pin_data_export_date);

	-- Return the result of the function
	RETURN @Result

END

GO
