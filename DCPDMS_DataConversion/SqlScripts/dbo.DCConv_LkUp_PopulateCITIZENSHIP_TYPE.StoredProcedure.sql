/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateCITIZENSHIP_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateCITIZENSHIP_TYPE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulateCITIZENSHIP_TYPE
GO
/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateCITIZENSHIP_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the CITIZENSHIP_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateCITIZENSHIP_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM [dbo].[CITIZENSHIP_TYPE];

	SET IDENTITY_INSERT [dbo].[CITIZENSHIP_TYPE] ON 

INSERT [dbo].[CITIZENSHIP_TYPE] ([CITIZENSHIP_TYPE_ID], [CITIZENSHIP_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (1, N'I am a Citizen of the United States', CAST(0x0000A4B900DDD873 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
INSERT [dbo].[CITIZENSHIP_TYPE] ([CITIZENSHIP_TYPE_ID], [CITIZENSHIP_TYPE_NAME], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (2, N'I am a qualified alien under the Federal Immigration and Nationality Act, my immigration status and alien number are as follows:', CAST(0x0000A4B900DDD873 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
SET IDENTITY_INSERT [dbo].[CITIZENSHIP_TYPE] OFF

END

GO
