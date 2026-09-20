/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulatePROVIDER_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulatePROVIDER_TYPE]','P') is not null
DROP PROCEDURE [dbo].DCConv_LkUp_PopulatePROVIDER_TYPE
GO

/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulatePROVIDER_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulatePROVIDER_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- create the temporary DCConversion table
	IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = N'DCConv_ProviderTypeInfo')
		DROP TABLE [dbo].[DCConv_ProviderTypeInfo];

	CREATE TABLE [dbo].[DCConv_ProviderTypeInfo](
		[MMIS Value] [varchar](3) NULL,
		[MMIS Long] [varchar](100) NULL,
		[PDMS Application Type ID] [bigint] NULL,
		[PDMS Applilcation Type] [varchar](50) NULL,
		[PDMS Provider Category] [bigint] NULL
	) ON [PRIMARY];

	-- populate the table with provider type look up info
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A00', N'Physician MD', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A01', N'Physician, Group Practice', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A01', N'Physician, Group Practice', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A01', N'Physician, Group Practice', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A02', N'Doctor Of Osteopathy ', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A04', N'Podiatrist', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'A05', N'Early Intervention', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B00', N'Independent Lab', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B00', N'Independent Lab', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B01', N'Ind Xray And Lab', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B01', N'Ind Xray And Lab', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B01', N'Ind Xray And Lab', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'B01', N'Ind Xray And Lab', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'C00', N'Independent X-Ray', 1, N'Standard', 3)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'C00', N'Independent X-Ray', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'C00', N'Independent X-Ray', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D00', N'Hospital, General', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D00', N'Hospital, General', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D01', N'Hospital, LTAC', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D01', N'Hospital, LTAC', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D02', N'Hospital, Psychiatric Public', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D02', N'Hospital, Psychiatric Public', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D03', N'Hospital, Psychiatric Private', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D03', N'Hospital, Psychiatric Private', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D04', N'Hospital, Emergency Access', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D04', N'Hospital, Emergency Access', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D05', N'Residential Treatment Center', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'D05', N'Residential Treatment Center', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'E00', N'Radiation Therapy Center', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'E00', N'Radiation Therapy Center', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'F00', N'Nursing Facility', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'F00', N'Nursing Facility', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'G00', N'ICF/MR ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'G00', N'ICF/MR ', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H00', N'Pharmacy, Retail', 1, N'Standard', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H00', N'Pharmacy, Retail', 7, N'Emergency-OOS', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H01', N'Pharmacy, Institutional', 1, N'Standard', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H01', N'Pharmacy, Institutional', 7, N'Emergency-OOS', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H02', N'Pharmacy, ADAP', 1, N'Standard', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H02', N'Pharmacy, ADAP', 7, N'Emergency-OOS', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H03', N'Pharmacy, Alliance', 1, N'Standard', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'H03', N'Pharmacy, Alliance', 7, N'Emergency-OOS', 5)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'I00', N'Durable Medical Equipment', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'I00', N'Durable Medical Equipment', 6, N'Crossover/QMB', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'I00', N'Durable Medical Equipment', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J00', N'Ambulance, Private ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J00', N'Ambulance, Private ', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J01', N'Ambulance, Public ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J01', N'Ambulance, Public ', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J02', N'Ambulance, Air Transport', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'J02', N'Ambulance, Air Transport', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K00', N'Dentist', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K01', N'Dentist, Group Practice', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K01', N'Dentist, Group Practice', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K01', N'Dentist, Group Practice', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'K02', N'Dentist, Waiver (Dentist ID/DD Waiver)', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'L00', N'Home Health Agency', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'L00', N'Home Health Agency', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'L01', N'Hospice', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'L01', N'Hospice', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M00', N'Audiologist', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M01', N'Hearing Aid Dealer (Hearing Aid Dispenser)', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'M01', N'Hearing Aid Dealer (Hearing Aid Dispenser)', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N00', N'Optometrist', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'N01', N'Optician/Optical Dispensary (Optician)', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'P00', N'Schools, DC Public ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'P01', N'Schools, DC Public Charter', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'P01', N'Schools, DC Public Charter', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'P02', N'Office State Superinten of Ed', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Q01', N'Hemodialysis, Freestanding', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Q01', N'Hemodialysis, Freestanding', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Q02', N'Hemodialysis, Hospital Based', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Q02', N'Hemodialysis, Hospital Based', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R01', N'Crossover Claims UB', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R01', N'Crossover Claims UB', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R01', N'Crossover Claims UB', 6, N'Crossover/QMB', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R02', N'Crossover Claims Only 1500', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R02', N'Crossover Claims Only 1500', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'R02', N'Crossover Claims Only 1500', 6, N'Crossover/QMB', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 1, N'Standard', 1)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 5, N'Streamlined', 1)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 6, N'Crossover/QMB', 1)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 7, N'Emergency-OOS', 1)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 1, N'Standard', 2)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 6, N'Crossover/QMB', 2)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 7, N'Emergency-OOS', 2)
	--INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S00', N'Nurse Practitioner', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 1, N'Standard', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 5, N'Streamlined', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 6, N'Crossover/QMB', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 7, N'Emergency-OOS', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 1, N'Standard', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 6, N'Crossover/QMB', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'S01', N'Nurse Midwives', 7, N'Emergency-OOS', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'T00', N'Rehabiliataion Center', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'T00', N'Rehabiliataion Center', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'T01', N'Mental Health Rehab Services', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'T01', N'Mental Health Rehab Services', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'U00', N'General Non-Billing', 5, N'Streamlined', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'U09', N'Direct Incentive Financial', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'U10', N'Participant Directed', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'V00', N'Ambulatory Surgical Centers ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'V00', N'Ambulatory Surgical Centers ', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'V01', N'Birthing Centers', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'V01', N'Birthing Centers', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'W01', N'IDDD Waiver', 2, N'HCBS-Waiver', 1)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'W01', N'IDDD Waiver', 2, N'HCBS-Waiver', 2)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'W01', N'IDDD Waiver', 2, N'HCBS-Waiver', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'W02', N'EPD Waiver', 3, N'EPD-Waiver', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'W04', N'Adult Day Health 1915(i)', 4, N'ADHP 1915(i)', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X00', N'Clinic, Private', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X00', N'Clinic, Private', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X01', N'Clinic, Dental', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X01', N'Clinic, Dental', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X02', N'Clinic, Mental Health', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X02', N'Clinic, Mental Health', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X03', N'Clinic, Family Planning', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X03', N'Clinic, Family Planning', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X04', N'Clinic, Adlt Alc/Subst Abuse ', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X05', N'Clinic, Fed Qualified Health', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X05', N'Clinic, Fed Qualified Health', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X06', N'Clinic, Youth Alc/Subst Abuse', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'X06', N'Clinic, Youth Alc/Subst Abuse', 7, N'Emergency-OOS', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Z00', N'Managed Care Organization', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Z01', N'MCO, Special Needs', 1, N'Standard', 3)
	INSERT [dbo].[DCConv_ProviderTypeInfo] ([MMIS Value], [MMIS Long], [PDMS Application Type ID], [PDMS Applilcation Type], [PDMS Provider Category]) VALUES (N'Z02', N'Medical Transportation Broker', 1, N'Standard', 3)

	DELETE FROM dbo.PROVIDER_RISKLEVEL_MAPPING;
	DELETE FROM dbo.PROVTYPE_ORGTYPE_MAPPING ;
	DELETE FROM dbo.REG_PAGE_SETTING WHERE PROVIDER_TYPE_ID IN (SELECT PROVIDER_TYPE_ID FROM DBO.PROVIDER_TYPE);	
	DELETE FROM dbo.SCREENING_ACTIVITY_DEFAULT_TEMPLATE WHERE PROVIDER_TYPE_ID IN (SELECT PROVIDER_TYPE_ID FROM DBO.PROVIDER_TYPE);	
	DELETE FROM dbo.REG_SECTION_UPLOAD_CONTROL WHERE PROVIDER_TYPE_ID in (SELECT PROVIDER_TYPE_ID FROM dbo.PROVIDER_TYPE);

	
	DELETE FROM dbo.PROVIDER_TYPE;

	DBCC CHECKIDENT (
			'dbo.PROVIDER_TYPE'
			,RESEED
			,0
			);

	-- replace the PDMS reference values with those from DC
	INSERT INTO [dbo].[PROVIDER_TYPE] (
		[PROVIDER_TYPE_ABBREVIATION]
		,[PROVIDER_TYPE_NAME]
		,[LAST_MODIFIED_DATE_TIME]
		,[LAST_MODIFIED_USER]
		,[IS_USED_IN_MMIS]
		,[PROVIDER_CATEGORY_TYPE_ID]
		,[MMIS_PROVIDER_TYPE_ID]
		,[REQUIRE_NPI]
		,[PROVIDER_RISK_LEVEL_ID]
		,[APPLICATION_TYPE_ID]
		)
	SELECT pty.[Short] AS [PROVIDER_TYPE_ABBREVIATION]
		,pty.[Long] AS [PROVIDER_TYPE_NAME]
		,@pin_run_reference_time AS [LAST_MODIFIED_DATE_TIME]
		,dbo.fn_GetUniqueGUID(2) AS [LAST_MODIFIED_USER]
		,'Y' AS [IS_USED_IN_MMIS]
		,ptinfo.[PDMS Provider Category] AS [PROVIDER_CATEGORY_TYPE_ID]
		,pty.[P-TY-CD] AS [MMIS_PROVIDER_TYPE_ID]
		,1 AS [REQUIRE_NPI]
		,CASE  
			WHEN pty.[P-TY-CD] IN ('I00','L00') THEN 3
			WHEN pty.[P-TY-CD] IN ('H00','H01', 'H02', 'H03','W01','X02','L01',
				'G00','J00','J02','B00','B01','T00', 'W02') THEN 2
			ELSE 1
		END
		AS [PROVIDER_RISK_LEVEL_ID]
		,ptinfo.[PDMS Application Type ID] 
	FROM SRC_LkUpProviderType [pty]
	INNER JOIN [dbo].[DCConv_ProviderTypeInfo] ptinfo ON ptinfo.[MMIS Value] = pty.[P-TY-CD] 


END


GO
