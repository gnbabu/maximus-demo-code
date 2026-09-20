/****** Object:  StoredProcedure [dbo].[fn_ConvertProviderTypeToPDMSByMMISAndEntityType]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[fn_ConvertProviderTypeToPDMSByMMISAndEntityType]','FN') is not null
DROP FUNCTION [fn_ConvertProviderTypeToPDMSByMMISAndEntityType]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_ConvertProviderTypeToPDMSByMMISAndEntityType]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 5/12/2016
-- Description:	Returns the PDMS equivalent for the Provider Type
-- =============================================
CREATE FUNCTION [dbo].[fn_ConvertProviderTypeToPDMSByMMISAndEntityType] 
(
	-- Add the parameters for the function here
	@pin_mmis_provider_type varchar(3),
	@pin_provider_entity_type datetime
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result int;

	SET @Result = 0;

	SELECT @Result = ISNULL(PROVIDER_TYPE_ID,0)
	FROM dbo.PROVIDER_TYPE 
	WHERE MMIS_PROVIDER_TYPE_ID = @pin_mmis_provider_type AND
	PROVIDER_CATEGORY_TYPE_ID = @pin_provider_entity_type;

	-- Return the result of the function
	RETURN @Result

END

GO
