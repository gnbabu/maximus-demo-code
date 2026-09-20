/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateREG_SECTION_UPLOAD_CONTROL]    Script Date: 8/17/2016  ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateREG_SECTION_UPLOAD_CONTROL]','P') is not null
DROP PROCEDURE [dbo].[DCConv_LkUp_PopulateREG_SECTION_UPLOAD_CONTROL]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Uma
-- Create date:11/22/2016License 
-- Description:	Fills the APPLICATION_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateREG_SECTION_UPLOAD_CONTROL] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with select distinct statements.
	SET NOCOUNT ON;

	DECLARE @APPLICATION_TYPE_ID int = 0,
			@PROVIDER_CATEGORY_TYPE_ID int = 0,
			@PROVIDER_TYPE_ID int = 0,
			@REG_PAGE_TYPE_ID int = 0,
			@MMIS_PROVIDER_TYPE_ID varchar(5) = '',
			@SECTION_TITLE varchar(150) = '',
			@SECTION_DESC varchar(500) = ''
			

	delete from [REG_SECTION_UPLOAD_CONTROL]

	DBCC CHECKIDENT ('dbo.REG_SECTION_UPLOAD_CONTROL', RESEED, 0);  


	insert into [REG_SECTION_UPLOAD_CONTROL]
	--ROW 3 Service Agreement - signed (use blue ink and mail it) - UI takes care of it 
	-- SOO issue


	--Row 4 Budget Summary Information 
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Budget Summary Information', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'F00','D05')
	--Row 5 Basic Organizational Chart
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Basic Organizational Chart', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('P00','P01','F00','H00','H01','H02','H03','W02')
	-- Row 6 Certification of Registration (Out of State Providers:)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,1, 'Certification of Registration', 'Certification of Registration', 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','X01','K00','K01','K02','I00','M01','L00','L01', 'G00','F00','H01','H00','A01','T00','D05','W04','H02','H03','S01',,'S00','N01','A04')
	union
	--12/12 removed 'M00','V01','X00','X03','X02','Q01','Q02','D01','D03','D02','B01','B00','C00','T01',
	--Track why M01,L01 isn't showing
	--12/19 remove S01, N00,N01,A02,A00

	--Row 7 Certificate of Occupancy or ( Lease) 

	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Certificate of Occupancy or ( Lease)', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V01','X00','X03','X02','X01','I00','C00','M01','Q01','Q02','L00','L01','D00','D01','D03','D02','G00','B01','B00','C00','F00','N01',	'H01',	'H00','T00',	'D05', 'W04', 'X04','W01','H02','H03', 'W02')
	--12/12 -- Added S01
	--12/19 - S01 remove
	-- Row 8 Claim Form (Required for Out of state except MD & VA)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,1, 'Claim Form', 'Claim Form', 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02','J00','K00', 'K01', 'K02', 'A02', 'C00', 'M01', 'Q01', 'L00', 'L01', 'D00','D01','D03','D02', 'B01', 'B00', 'S01','S00','F00', 'N01','N00','H01','H00','A00','A01','A04','T00','H02','H03') 
	-- Row 9 CLIA  --made it optional
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'CLIA', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'CLIA', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V00','V01','X00','X03','X02','X01','Q01','D00','D01','D03','D02','B01','B00','C00','S01')
	--12/12 Added S01
	-- Row 10 DEA License - (if applicable) 
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'DEA License', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'DEA', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V00','V01','K00','K02','A02','D01','D03','D02','S01','S00','H01',	'H00','H02','H03','A00',	'A04')
	-- Row 11 Disclosure of Ownership
	
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Disclosure of Ownership', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','V00','V01','X00','X03','X05','X02','X01','K00','K01','K02','I00','A02','M01','Q01','Q02','L00','L01','D00','D01','D03','D02','G00','B01','B00','C00', 'T01','S01','S00','F00','N01',	'N00',	'H01',	'H00','H02',	'A02',	'A00',	'A01',	'A04',	'T00',	'D05',	'W04', 'W02', 'P01', 'P00','H03','X04','W01')
	--12/12 Added ,'X04','W01'

	--Row 12 Fleet Info
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Fleet Info', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01')

	--Row 13 Hospital License
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Hospital License (Include Dialysis License if applicable)', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('Q01','Q02','D00','D01','D03','D02')

	--Row 14 Certificate of Need ( CON) or a Certificate of Acquisition (when applicable)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Certificate of Need ( CON) or a Certificate of Acquisition (when applicable)', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V00','V01','X03','X01','L00','L01','D00','D01','D03','D02','G00','F00','N01','T00','W04')
	
	--Row 15 List of Board Members (For facilities and corporations)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'List of Board Members', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','V00','V01','X00','X03','X02','X01','K02','I00','Q01','L00','L01','D00','D01','D03','D02','F00','N01','H00','H02','H03','T00','D05','W04','P01','W02', 'H01')
	--12/12 'S01','W02'
	--12/19 removed S01
	
	--ROW 16 List of Key Personnell (Staff and Job Titles)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'List of Key Personnell (Staff and Job Titles)', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V01','I00', 'H00','H02', 'W02','P00','P01','W04','W01','H03','H01')
	--12/12 Added 'S01'
	--ROW 17 Medicaid Enrollment Proof - if Practicing Out-of-state  
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Medicaid Enrollment Proof', 'if Practicing Out-of-state', 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Medicaid', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'I00')

	-- ROW 18 Medicare Enrollment Certification  Required for Dialysis Facilities (Only if approved)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Medicare Enrollment Certification', 'Required for Dialysis Facilities (Only if approved)', 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Medicare', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','M00','X00','X03','X02','X01','I00','A02','M01','Q01','Q02','L00','D00','D01','D03','D02','B01','B00','C00','S01','S00','F00','N01',	'N00',	'H01',	'H00','H02',	'A02',	'A00',	'A01',	'A04',	'A03',	'T00','P01','P00', 'H03')
	-- required for 'L01' ???
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Medicare Enrollment Certification', 'Required for Dialysis Facilities (Only if approved)', 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Medicare', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'L01')
	--12/12 L01 Required for Dialysis Facilities (Only if approved)

	-- ROW 19 NPI # and Taxonomy 
	union  
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,1, 'NPI # and Taxonomy', 'NPI # and Taxonomy', 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02', 'J00', 'J01', 'V00', 'M00', 'V01', 'X00', 'X03','X05','X02','K00', 'K01', 'K02', 'I00', 'A02', 'M01', 'Q01', 'Q02','L00', 'L01', 'D00', 'D03', 'D02', 'G00', 'B01', 'B00', 'T01', 'S01', 'S00', 'F00', 'P02', 'N01', 'N00','H01', 'H00','H02', 'A02', 'A00', 'A01', 'A04', 'T00', 'D05', 'W04', 'W02','P00','P01','D01','H03','X04','H02','W01','A05','X01','C00')

	-- ROW 20 Organizational Documents	
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Organizational Documents', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'V00','V01','X00','X03','X02','X01','I00','F00','P02'	,'N01',	'H01',	'H00','H02','T00','P00','P01','H03','D05')
	-- 12/12 added S01
	--12/19 -- remove S01
	
	--ROW 21 Out of State Facility Rate   PFRT Rate
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Out of State Facility Rate', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'X05','D00','D01','D03','D02','F00','D05')

	-- 12/19 - remove W02

	--ROW 22 Professional License or Business License
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Professional License', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'License', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','V00','M00','V01','X00','X03','X05','X02','X01','K00','K02','I00','A02','M01','L00','L01','D00','D01','D03','D02','G00','B01','B00','C00','S01','S00','F00','N01',	'N00',	'H01',	'H00','H02',	'A02',	'A00',	'A04',	'T00',	'D05',	'W04','X04','W01','H02','H03','S00')
	 -- 12/12 S00 ADDED
	-- ROW 23 Program Policies/ Procedures
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Program Policies/ Procedures', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'L00','L01','F00','P02','D05','W04')
	
	-- ROW 24  Proof of Liability Insurance of at least $1,000,000.00
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Proof of Liability Insurance ', 'of at least $1,000,000.00', 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Insurance', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','V00','M00','V01','X00','X03','X05','X02','X01','P00','P01','K00','K01','K02','I00','A02','M01','Q01','Q02','L00','L01','D00','D01','D03','D02','G00','B01','B00','C00','T01','S01','S00','F00','P02'	,'N01',	'N00',	'H01',	'H00','H02',	'A02',	'A00',	'A01',	'A04',	'A03',	'T00',	'D05',	'W04', 'W01', 'W02','H03','X04')
	
	--Row 25 Proof of Surety Bond of at $50,000 or more
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Proof of Surety Bond of at $50,000 or more', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('I00','L00')
	
	-- Row 26 Quality Management Plan
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Quality Management Plan', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'L00','L01','D01','D03','D02','F00','P02','T00','D05','D00')
	
	-- Row 27 Residential Treatment Addendum
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,8, 'Residential Treatment Addendum', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'D05')
	
	-- ROW 28 Staff - Criminal Background checks  
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,8, 'Staff - Criminal Background checks', 'For unlicensed staff providing direct services to Medicaid  Recipients  ( Any staff making home deliveries)', 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','I00','L00','F00','H01','H00','H02','W04','W01','H03')
	--12/12 - L01, H00, H02 isn't working???
	
	-- ROW 29 Staff - Driving Records 
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Staff - Driving Records ', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','P02','W04')-- 'N01','H01','H00','H02','A00','A01','A04',
	--12/12 -- P02, A02 not showing doc
	-- ROW 30 Staff Documentation - Primary Staff certifications, licenses
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Staff Documentation - Primary Staff certifications, licenses ', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02','V01','I00','T00', 'W01','W04')
	--12/12 -- S01
	--12/19 -- remove S01
	-- ROW 31 State Controlled Substance Registration if Applicable
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'State Controlled Substance Registration if Applicable', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','V00','V01','X05','X02', 'X01','K00','A02','S00','A00', 'A01', 'A04','PO2')
	--12/12 -- S00,P02,N01,N00,H01,H00,a01 NOT SHOWING DOC
	--12/19 - remove  'B01','B00','C00', 'H00','PO2','N01','N00','H01','S01'
	-- ROW 32 --State Pharmacist License for each Pharmacist at location
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'State Pharmacist License', ' for each Pharmacist at location', 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Pharmacist', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'H00','H02', 'H01','H03')
	
	-- ROW 33 State Pharmacy License (If out of state must register with DC DOH)
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'State Pharmacy License', 'If out of state must register with DC DOH', 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', 'Pharmacy', null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'H00','H02', 'H01','H03')
	
	-- ROW 34 W-9
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,6, 'W-9', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'J02',	'J00','J01','V00','V01','X00','X03','X05','X02','X01','K01','I00','W02', 'M01','Q01','Q02','L00','L01','D00','D01','D03','D02','G00','B01','B00','C00','T01','F00','P02','H01',	'H00','H02', 'A01','T00','D05',	'W04','P00','P01','X04','W01','H03','A00','A02','A04','K00','K02','M00','N00','N01','S00','S01')
	
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,6, 'W-9', null, 1, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ( 'A00','A02','A04','K00','K02','M00','N00','N01','S00','S01')

	--12/12 -- L01 NOT SHOWING DOC
	-- Make it optional for the DCPDMS-1739

	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,6, 'W-9', null, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('A00','A02','A04','K00','K02','M00','N00','N01','S00','S01','U00') and APPLICATION_TYPE_ID = 5


	--ROW 35 Copy of ASARS Certification
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID, 1, 'Copy of ASARS Certification', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', NULL, NULL
		FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = 'X04' --AND APPLICATION_TYPE_ID = 1 AND PROVIDER_CATEGORY_TYPE_ID = 3

    -- ROW 36 Proof of IDDD waiver services
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID, 25, 'Proof of IDDD waiver services', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', NULL, NULL
		FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = 'W01' --AND APPLICATION_TYPE_ID = 1 AND PROVIDER_CATEGORY_TYPE_ID = 3

   -- ROW 37 Information Session Certificate
   union
	select distinct 3, 3, PROVIDER_TYPE_ID, 25, 'Information Session Certificate', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', NULL, NULL
		FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = 'W02' 
    
  -- ROW 38 Certificate of good standing			
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,1, 'Certificate of Good Standing', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W02','W04')
	--12/12 aDDING W04
  
  -- ROW 39 Personnel/Employee Training Program
	 union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Personnel/Employee Training Program', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')
  
  -- ROW 40	Intake & Discharge Policy & Procedure
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Intake & Discharge Policy & Procedure', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

 -- ROW 41 Personnel Policy & Procedure
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Personnel Policy & Procedure', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

 -- ROW 42 Incident Reporting & Investigation Policy & Procedures
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Incident Reporting & Investigation Policy & Procedures', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 43 Handling Complaints and Grievances
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Handling Complaints and Grievances', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 44 Quality Improvement Plan - Individual outcomes - Program Outcomes
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Quality Improvement Plan-Individual outcomes-Program Outcomes', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 45 Reporting Fraud, Waste and Abuse
    union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Reporting Fraud, Waste and Abuse', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 46 Plan for Compliance & Monitoring 
	 union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Plan for Compliance & Monitoring ', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 47 NPI number policy and practice of assignment to all direct care personnel
	union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'NPI number policy and practice of assignment to all direct care personnel', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 48 Certificaiton of personnel and program
	 union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Certificaiton of personnel and program', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 49 Verificaiton of good standing
   union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Verificaiton of good standing', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 50 Most recent audited financial statement
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Most Recent Audited Financial Statement', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W02', 'W04')

-- ROW 51 Articles of Incorporation
		union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Articles of Incorporation', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W02','W04')

-- ROW 52 By-laws or similar documents regulating the conduct of internal affairs
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'By-laws or similar documents regulating the conduct of internal affairs', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 53 Vision Mission Statement
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,5, 'Vision Mission Statement', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W04')

-- ROW 54 Organization chart - detailed
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Organization Chart - Detailed', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W02', 'W04')
	
-- ROW 55 Knowledge and understand of how to operate a 1915(i) as proposed in the District's overall plan
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID, 25, 'Knowledge and understand of how to operate a 1915(i)', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', NULL, NULL
		FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = 'W04' 

-- ROW 56 Life Safety Code/Fire & Safety Inspection report (most recent)
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID, 25, 'Life Safety Code/Fire & Safety Inspection report', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', NULL, NULL
		FROM PROVIDER_TYPE WHERE MMIS_PROVIDER_TYPE_ID = 'W04' 

-- ROW 57 Job descriptions qualifications/credentials
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,2, 'Job Descriptions Qualifications/Credentials', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID IN ('W02','W04')
-- ROW 58 Pre-approval number and Tracking Number
union
	select distinct APPLICATION_TYPE_ID, PROVIDER_CATEGORY_TYPE_ID, PROVIDER_TYPE_ID,1, 'Pre-approval number and Tracking Number', NULL, 0, @pin_run_reference_time, '5D0689A8-D885-4211-B9FD-56757474AB4D', null, null from PROVIDER_TYPE
	where MMIS_PROVIDER_TYPE_ID ='W04'

	
	
--------------------------------------------------------ALL ROW COMPLETE----------------------------------------------------------------------	

END
