/****** Object:  StoredProcedure [dbo].[fn_ConvertEnrollmentStatusToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertEnrollmentStatusToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertEnrollmentStatusToPDMS
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertEnrollmentStatusToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Converts the Enrollment status
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertEnrollmentStatusToPDMS] 
(
	-- Add the parameters for the function here
	@pin_enrollment_status_type varchar(2)
)
RETURNS varchar(10)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(10);

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = '01';

	-- Return the result of the function
	RETURN @Result

END

GO
