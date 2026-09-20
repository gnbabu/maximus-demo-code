/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateINSURANCE_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateINSURANCE_TYPE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulateINSURANCE_TYPE
GO
/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateINSURANCE_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the INSURANCE_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateINSURANCE_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [dbo].[INSURANCE_TYPE];

	SET IDENTITY_INSERT [dbo].[INSURANCE_TYPE] ON 

INSERT [dbo].[INSURANCE_TYPE] ([INSURANCE_TYPE_ID], [NAME], [IS_ACTIVE]) VALUES (1, N'Insurance Type 1', 1)
INSERT [dbo].[INSURANCE_TYPE] ([INSURANCE_TYPE_ID], [NAME], [IS_ACTIVE]) VALUES (2, N'Insurance Type 2', 1)
INSERT [dbo].[INSURANCE_TYPE] ([INSURANCE_TYPE_ID], [NAME], [IS_ACTIVE]) VALUES (3, N'Insurance Type 3', 1)
SET IDENTITY_INSERT [dbo].[INSURANCE_TYPE] OFF

END

GO
