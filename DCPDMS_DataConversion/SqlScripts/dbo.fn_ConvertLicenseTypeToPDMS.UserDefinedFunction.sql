/****** Object:  StoredProcedure [dbo].[fn_ConvertLicenseTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertLicenseTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertLicenseTypeToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertLicenseTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/17/2016
-- Description:	Converts a DC license type to a PDMS type
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertLicenseTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_license_type_cd varchar(2)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 2;

	-- Return the result of the function
	RETURN @Result

END

GO
