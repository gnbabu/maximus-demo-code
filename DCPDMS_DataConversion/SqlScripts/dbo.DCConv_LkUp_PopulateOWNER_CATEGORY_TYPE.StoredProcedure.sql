/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE
GO
/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the OWNER_CATEGORY_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateOWNER_CATEGORY_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [dbo].[OWNER_CATEGORY_TYPE];

	SET IDENTITY_INSERT [dbo].[OWNER_CATEGORY_TYPE] ON 

INSERT [dbo].[OWNER_CATEGORY_TYPE] ([OWNER_CATEGORY_TYPE_ID], [OWNER_CATEGORY_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (1, N'Are you the only provider person in your practice?', CAST(0x0000A61D008BDF6A AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
INSERT [dbo].[OWNER_CATEGORY_TYPE] ([OWNER_CATEGORY_TYPE_ID], [OWNER_CATEGORY_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (2, N'Do you all practice with other provider person(s) in all the same location(s)', CAST(0x0000A61D008BDF6A AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
INSERT [dbo].[OWNER_CATEGORY_TYPE] ([OWNER_CATEGORY_TYPE_ID], [OWNER_CATEGORY_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (3, N'Are you any other practice type?', CAST(0x0000A61D008BDF6A AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
SET IDENTITY_INSERT [dbo].[OWNER_CATEGORY_TYPE] OFF

END

GO
