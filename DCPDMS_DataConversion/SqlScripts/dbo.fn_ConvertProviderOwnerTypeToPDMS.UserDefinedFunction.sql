/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderOwnerTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertProviderOwnerTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertProviderOwnerTypeToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderOwnerTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/18/2016
-- Description:	Converts the DC owner type to PDMS
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderOwnerTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_owner_type_code varchar(1)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 15;

	-- Return the result of the function
	RETURN @Result

END

GO
