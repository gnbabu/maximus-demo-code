/****** Object:  StoredProcedure [dbo].[fn_BuildName]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_BuildName]','FN') is not null
DROP FUNCTION [dbo].fn_BuildName
GO
/****** Object:  UserDefinedFunction [dbo].[fn_BuildName]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/17/2016
-- Description:	Creates a name from a set of name parts
-- =============================================
CREATE FUNCTION [dbo].[fn_BuildName] 
(
	-- Add the parameters for the function here
	@pin_first_name varchar(15),
	@pin_middle_initial varchar(1),
	@pin_last_name varchar(35),
	@pin_suffix varchar(5),
	@pin_name_format_code int
)
RETURNS varchar(max)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(max)

	SET @pin_first_name = RTRIM(LTRIM(@pin_first_name));
	SET @pin_middle_initial = RTRIM(LTRIM(@pin_middle_initial));
	SET @pin_last_name = RTRIM(LTRIM(@pin_last_name));
	SET @pin_suffix = RTRIM(LTRIM(@pin_suffix));

	-- the format is fname mi lname, suffix
	IF (@pin_name_format_code = 1) 
	BEGIN
		SET @Result = @pin_last_name;

		if (LEN(@pin_middle_initial) > 0)
			SET @Result = @pin_middle_initial + ' ' + @Result;

		if (LEN(@pin_first_name) > 0)
			SET @Result = @pin_first_name + ' ' + @Result;

		if (LEN(@pin_suffix) > 0)
			SET @Result = @Result + ', ' + @pin_suffix;
	END
	ELSE IF (@pin_name_format_code = 2) -- this format is lname suffix, fname mi
	BEGIN
		SET @Result = @pin_last_name;

		if (LEN(@pin_suffix) > 0)
			SET @Result = @Result + ' ' + @pin_suffix;

		if (LEN(@pin_first_name) > 0)
			SET @Result = @Result + ', ' +  @pin_first_name;

		if (LEN(@pin_middle_initial) > 0)
			SET @Result = @Result + ' ' + @pin_middle_initial;
	END 
	ELSE
		SET @Result = '';

	-- Return the result of the function
	RETURN @Result

END

GO
