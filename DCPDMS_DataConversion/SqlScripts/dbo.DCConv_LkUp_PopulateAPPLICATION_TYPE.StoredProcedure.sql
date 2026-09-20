/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateAPPLICATION_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateAPPLICATION_TYPE]','P') is not null
DROP PROCEDURE [dbo].[DCConv_LkUp_PopulateAPPLICATION_TYPE]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the APPLICATION_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateAPPLICATION_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DELETE FROM dbo.REG_PAGE_SETTING WHERE APPLICATION_TYPE_ID in (SELECT APPLICATION_TYPE_ID FROM [dbo].[APPLICATION_TYPE]);
	DELETE FROM dbo.SCREENING_ACTIVITY_DEFAULT_TEMPLATE 
	WHERE PROVIDER_TYPE_ID in ( SELECT PROVIDER_TYPE_ID 
								FROM dbo.PROVIDER_TYPE a JOIN dbo.APPLICATION_TYPE b ON a.APPLICATION_TYPE_ID = b.APPLICATION_TYPE_ID);
	
	DELETE FROM dbo.REG_SECTION_UPLOAD_CONTROL 
	WHERE APPLICATION_TYPE_ID in (SELECT APPLICATION_TYPE_ID FROM [dbo].[APPLICATION_TYPE])
	OR PROVIDER_TYPE_ID in ( SELECT PROVIDER_TYPE_ID 
								FROM dbo.PROVIDER_TYPE a JOIN dbo.APPLICATION_TYPE b ON a.APPLICATION_TYPE_ID = b.APPLICATION_TYPE_ID);

	DELETE FROM dbo.CATEGORY_OF_SERVICE_TYPE_PROVIDER_TYPE_XREF;

	DELETE FROM dbo.PROVIDER_TYPE WHERE APPLICATION_TYPE_ID in (SELECT APPLICATION_TYPE_ID FROM [dbo].[APPLICATION_TYPE]);

	DELETE FROM [dbo].[APPLICATION_TYPE];

	--SET IDENTITY_INSERT [dbo].[APPLICATION_TYPE] ON;

	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (1, N'Standard Application', N'Use this application if you are applying to become a new individual, group, facility, or institutional provider for the District of Columbia Medicaid program. This application includes ASARS and PRTF providers.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (2, N'HCBS-Waiver', N'Use this application if you are applying to provide home and community based waiver services. You must have received your pre-approval notice from DDA before initiating this application.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (3, N'EPD-Waiver', N'Use this application if you are applying for elderly and persons with physical disabilities (EPD) waiver provider. You must have received your pre-approval notice from long-term care before initiating this process.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (4, N'ADHP 1915(i)', N'Use this application if you are applying as a 1915(i) state plan home and community-based services provider. You must have received your pre-approval notice from long-term care before initiating this process.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (5, N'Streamlined', N'Use this application if you are applying solely for the purposed of ordering/referring.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (6, N'Crossover/QMB', N'Use this application if you are interested in rendering services to QMB beneficiaries. To provide these services you must enroll in the DC Medicaid program.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')
	INSERT [dbo].[APPLICATION_TYPE] ([APPLICATION_TYPE_ID], [APPLICATION_TYPE_NAME], [APPLICATION_TYPE_DESC], [IS_USED_IN_MMIS], [MMIS_APPLICATION_TYPE_ID], [LAST_MODIFIED_DATE_TIME], [LAST_MODIFIED_USER]) VALUES (7, N'Emergency-OOS', N'Use this application on a one-time basis if you are a provider that has rendered reimbursable services to DC Medicaid-eligible recipients. This application is not intended for providers who will provide services to the general DC Medicaid population. Claims may be submitted up to 365 days from the date the service was rendered.', NULL, NULL, CAST(0x0000A620010D7644 AS DateTime), N'5d0689a8-d885-4211-b9fd-56757474ab4d')

	--SET IDENTITY_INSERT [dbo].[APPLICATION_TYPE] OFF;

END

GO
