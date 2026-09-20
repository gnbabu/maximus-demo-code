/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE
GO
/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the SCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateSCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SCREENING_ACTIVITY_PROVIDER_TYPE_TEMPLATE];

	-- add inserts here

END

GO
