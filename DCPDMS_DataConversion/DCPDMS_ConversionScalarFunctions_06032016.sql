USE [DC_PDMS_RBM01]
GO

/****** Object:  UserDefinedFunction [dbo].[fn_BuildName]    Script Date: 6/3/2016 11:53:11 AM ******/
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

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertDCDateToPDMS]    Script Date: 6/3/2016 11:53:12 AM ******/
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

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertDCSpecialtyTypeToPDMS]    Script Date: 6/3/2016 11:53:12 AM ******/
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
	@pin_specialty_type varchar(3)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 4;

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertEnrollmentStatusToPDMS]    Script Date: 6/3/2016 11:53:13 AM ******/
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

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertLicenseTypeToPDMS]    Script Date: 6/3/2016 11:53:13 AM ******/
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

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderEntityTypeToPDMS]    Script Date: 6/3/2016 11:53:14 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/11/2016
-- Description:	Analyzes the DC data to get the Provider Entity Type
-- The provider entity type is also know as the provider category
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderEntityTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_provider_sys_id int,
	@pin_conv_data_export_date datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int
	DECLARE @ssn varchar(9);
	DECLARE @hasMembers bit;

	SELECT @ssn = RTRIM(LTRIM([P-SSN-NUM])) 
	FROM SRC_Providers 
	WHERE [P-SYS-ID] = @pin_provider_sys_id;

	SELECT @hasMembers = 
		CASE WHEN 
			(SELECT COUNT(*) 
			FROM SRC_ProviderAffiliates 
			WHERE [P-GROUP-SYS-ID] = @pin_provider_sys_id AND 
			[P-MEMBER-SYS-ID] IS NOT NULL AND
			dbo.fn_ConvertDCDateToPDMS([P-AFFL-END-DT]) > @pin_conv_data_export_date) > 0 
		THEN 1 ELSE 0 END;

	IF LEN(@ssn) > 0
		SET @Result = 1;
	ELSE IF @hasMembers = 1
		SET @Result = 2;
	ELSE 
		SET @Result = 3;
	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderOwnerTypeToPDMS]    Script Date: 6/3/2016 11:53:14 AM ******/
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

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderTaxonomyCodeToPDMS]    Script Date: 6/3/2016 11:53:14 AM ******/
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
	@pin_taxonomy_code varchar(10)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 5;

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderTypeToPDMS]    Script Date: 6/3/2016 11:53:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Returns the PDMS equivalent for the Provider Type
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_dc_provider_type_code varchar(3)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- this is a stub
	SELECT @Result =6;

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertTerminationReasonToPDMS]    Script Date: 6/3/2016 11:53:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Converts the DC data to find the termination reason
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertTerminationReasonToPDMS] 
(
	-- Add the parameters for the function here
	@pin_enrollment_status_code varchar(2)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 2

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_GetProviderCategoryFromDCProviderType]    Script Date: 6/3/2016 11:53:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/19/2016
-- Description:	Gets the provider category based on the provider type
-- =============================================
CREATE FUNCTION [dbo].[fn_GetProviderCategoryFromDCProviderType] 
(
	-- Add the parameters for the function here
	@pin_provider_type_cd varchar(3)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Result = 1

	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_GetProviderFiscalYearEnd]    Script Date: 6/3/2016 11:53:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/17/2016
-- Description:	Finds the FY end date for a provider from the DC data
-- =============================================
CREATE FUNCTION [dbo].[fn_GetProviderFiscalYearEnd] 
(
	-- Add the parameters for the function here
	@pin_provider_sys_id int,
	@pin_mcare_fy_month int, 
	@pin_mcaid_fy_month int, 
	@pin_facility_fy_month int
)
RETURNS datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result datetime

	SET @Result = '1/1/1753';
	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[fn_ParseName]    Script Date: 6/3/2016 11:53:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 5/18/2016
-- Descripti@on:	Splits a full name into fname, mi, lname, suffix
-- =============================================
CREATE FUNCTION [dbo].[fn_ParseName] 
(
	-- Add the parameters for the function here
	@pin_full_name varchar(50),
	@pin_name_part_code int -- 1 = fname, 2 - mi, 3 = lname, 4 = suffix

)
RETURNS varchar(50)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result varchar(50)

	SET @Result = dbo.GetStringPartByDelimeter(@pin_full_name, ' ', @pin_name_part_code);


	-- Return the result of the function
	RETURN @Result

END

GO

/****** Object:  UserDefinedFunction [dbo].[GetStringPartByDelimeter]    Script Date: 6/3/2016 11:53:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create function [dbo].[GetStringPartByDelimeter] (
    @value as nvarchar(max),
    @delimeter as nvarchar(max),
    @position as int
) returns NVARCHAR(MAX) 
AS BEGIN
    declare @startPos as int
    declare @endPos as int
    set @endPos = -1
    while (@position > 0 and @endPos != 0) begin
        set @startPos = @endPos + 1
        set @endPos = charindex(@delimeter, @value, @startPos)

        if(@position = 1) begin
            if(@endPos = 0)
                set @endPos = len(@value) + 1

            return substring(@value, @startPos, @endPos - @startPos)
        end

        set @position = @position - 1
    end

    return null
end
GO

