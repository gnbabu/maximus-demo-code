/****** Object:  StoredProcedure [dbo].[fn_ConvertDCDateToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertDCDateToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertDCDateToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertDCDateToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Converts a raw DC date string to a datetime
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertDCDateToPDMS] 
(
	-- Add the parameters for the function here
	@pin_date_string varchar(20)
)
RETURNS datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result datetime

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = CAST(CASE WHEN ISDATE(@pin_date_string) = 1 THEN @pin_date_string ELSE '1/1/1753' END AS datetime);

	-- Return the result of the function
	RETURN @Result

END

GO
