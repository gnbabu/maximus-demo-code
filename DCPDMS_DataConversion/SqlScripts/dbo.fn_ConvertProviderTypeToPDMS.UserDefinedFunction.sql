/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[fn_ConvertProviderTypeToPDMS]','FN') is not null
DROP FUNCTION [dbo].fn_ConvertProviderTypeToPDMS
GO

/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderTypeToPDMS]    Script Date: 8/1/2016 3:13:13 PM ******/
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
	@pin_provider_sys_id int,
	@pin_conv_data_export_date datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int;
	DECLARE @mmis_provider_type_cd varchar(3);
	DECLARE @mmis_enrollment_status_cd varchar(2);
	DECLARE @provider_entity_type int;

	SET @provider_entity_type = dbo.fn_ConvertProviderEntityTypeToPDMS(@pin_provider_sys_id, @pin_conv_data_export_date);

	SET @Result = 0;

	SELECT @mmis_provider_type_cd = [P-TY-CD], @mmis_enrollment_status_cd = [P-ENROL-STAT-TY-CD]  FROM SRC_Enrollments WHERE [P-SYS-ID] = @pin_provider_sys_id;

	SELECT @Result = ISNULL(PROVIDER_TYPE_ID,0)
	FROM dbo.PROVIDER_TYPE 
	WHERE MMIS_PROVIDER_TYPE_ID = @mmis_provider_type_cd AND
	PROVIDER_CATEGORY_TYPE_ID = @provider_entity_type AND
	APPLICATION_TYPE_ID = dbo.fn_ConvertProviderApplicationTypeToPDMS(@mmis_provider_type_cd,@mmis_enrollment_status_cd);

	-- Return the result of the function
	RETURN @Result

END

GO
