/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateDEFENDENT_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateDEFENDENT_TYPE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulateDEFENDENT_TYPE
GO
/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateDEFENDENT_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the DEFENDENT_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateDEFENDENT_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [dbo].[DEFENDENT_TYPE];

	SET IDENTITY_INSERT [dbo].[DEFENDENT_TYPE] ON 

INSERT [dbo].[DEFENDENT_TYPE] ([DEFENDENT_TYPE_ID], [DEFENDENT_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (1, N'Primary Defendent', CAST(0x0000A64100DD7712 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
INSERT [dbo].[DEFENDENT_TYPE] ([DEFENDENT_TYPE_ID], [DEFENDENT_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (2, N'Co-Defendent', CAST(0x0000A64100DD7712 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
SET IDENTITY_INSERT [dbo].[DEFENDENT_TYPE] OFF

END

GO
