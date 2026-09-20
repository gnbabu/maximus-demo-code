/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderApplicationTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertProviderApplicationTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertProviderApplicationTypeToPDMS
GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderApplicationTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Richard Mays
-- Create date: 7/18/2016
-- Description:	Converts the MMIS data to get the application type
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderApplicationTypeToPDMS] 
(
	-- Add the parameters for the function here
	@pin_provider_type_code varchar(3),
	@pin_provider_enrollment_status varchar(2)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int

	SET @Result = 0;

	IF @pin_provider_type_code IN ('A01', 'K01','S00', 'A00', 'A02', 'A04', 'K00', 
		'K02', 'M00', 'N00', 'N01', 'S01', 'A05', 'B00', 'B01', 'C00', 'D00', 'D01', 
		'D02', 'D03', 'D04', 'D05', 'E00', 'F00', 'G00', 'I00', 'J00', 'J01', 'J02',
		'L00', 'L01', 'M01', 'P00', 'P01', 'P02', 'Q01', 'Q02', 'T00', 'T01', 'V00', 
		'V01', 'W03', 'X00', 'X01', 'X02', 'X03', 'X04', 'X05', 'X06', 'Z00', 'Z01', 
		'Z02', 'H00', 'H01', 'H02', 'H03') AND 
		@pin_provider_enrollment_status <> '80'
	BEGIN
		--Set Application Type = Main
		SET @Result = 1;
	END

	IF @pin_provider_type_code = 'W04' AND @pin_provider_enrollment_status <> '80'
	BEGIN
		-- Set Application Type = ADHP 1915(i)
		SET @Result = 4;
	END

	IF @pin_provider_type_code = 'W01' AND @pin_provider_enrollment_status <> '80'
	BEGIN
		-- Set Application Type = HCBS-Waiver
		SET @Result = 2;
	END

	IF @pin_provider_type_code = 'W02' AND @pin_provider_enrollment_status <> '80'
	BEGIN
		-- Set Application Type = EPD-Waiver
		SET @Result = 3;
	END

	IF  @pin_provider_enrollment_status = '80'
	BEGIN
		-- Set Application Type = Streamlined
		SET @Result = 5;
	END

	IF @pin_provider_type_code IN ('R01', 'R02') 
	BEGIN
		-- Set Application Type = Crossover/QMB
		SET @Result = 6;
	END

	--IF @pin_provider_enrollment_status = '52'
	--BEGIN
	--	-- Set Application Type = Emergency-OOS
	--	SET @Result = 7;
	--END
	-- Return the result of the function
	RETURN @Result

END


GO
