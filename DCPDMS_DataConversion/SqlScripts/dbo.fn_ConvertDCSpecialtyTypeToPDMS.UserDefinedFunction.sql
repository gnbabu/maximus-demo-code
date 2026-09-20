/****** Object:  StoredProcedure [dbo].[fn_ConvertDCSpecialtyTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertDCSpecialtyTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertDCSpecialtyTypeToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertDCSpecialtyTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/13/2016
-- Description:	Converts a DC specialty code to PDMS
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertDCSpecialtyTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_specialty_type varchar(3),
		@pin_sys_id int,
	@pin_data_export_date datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	SET @Result = 0;

	SELECT @Result = SPECIALTY_TYPE_ID 
	FROM dbo.TAXONOMY_TYPE 
	WHERE 
	RTRIM(LTRIM(UPPER(MMIS_SPECIALTY_TYPE_ID))) = RTRIM(LTRIM(UPPER(@pin_specialty_type))) AND
	PROVIDER_TYPE_ID = dbo.fn_ConvertProviderTypeToPDMS(@pin_sys_id, @pin_data_export_date);


	-- Return the result of the function
	RETURN @Result

END

GO
