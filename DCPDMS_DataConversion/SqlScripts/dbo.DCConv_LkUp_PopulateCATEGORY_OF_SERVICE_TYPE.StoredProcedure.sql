/****** Object:  StoredProcedure [dbo].[DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE]    Script Date: 8/1/2016 3:13:13 PM ******/
IF object_id('[dbo].[DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE]','P') is not null
DROP PROCEDURE [dbo].[DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Richard Mays
-- Create date: 7/21/2016
-- Description:	Fills the CATEGORY_OF_SERVICE_TYPE
-- =============================================
CREATE PROCEDURE [dbo].[DCConv_LkUp_PopulateCATEGORY_OF_SERVICE_TYPE] 
	@pin_run_reference_time datetime
AS
BEGIN
 -- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @AdminUser UNIQUEIDENTIFIER = '5D0689A8-D885-4211-B9FD-56757474AB4D',
		@DateTimeNow DATETIME
	SELECT @DateTimeNow = GETDATE()
	
	DELETE FROM [dbo].[CATEGORY_OF_SERVICE_TYPE];

	SET IDENTITY_INSERT [dbo].[CATEGORY_OF_SERVICE_TYPE] ON;

	INSERT INTO [dbo].[CATEGORY_OF_SERVICE_TYPE] ([CATEGORY_OF_SERVICE_TYPE_ID], [MAX_CATEGORY_OF_SERVICE_TYPE],[CATEGORY_OF_SERVICE_TYPE_SHORTNAME],[CATEGORY_OF_SERVICE_TYPE_NAME], [CATEGORY_OF_SERVICE_TYPE_MNEMONIC],[LAST_MODIFIED_DATE_TIME],[LAST_MODIFIED_USER])
		SELECT        1       As CategoryOfServiceTypeID,'01' As MaxCategoryOfServiceType,'IP Hosp Sv' As CategoryOfServiceShortName,'Inpatient Hospital Services'      As CategoryOfServiceName,'IP-HOSP'       	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        2       As CategoryOfServiceTypeID,'02' As MaxCategoryOfServiceType,'OP Hosp Sv' As CategoryOfServiceShortName,'Outpatient Hospital Services'     As CategoryOfServiceName,'OP-HOSP'       	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        3       As CategoryOfServiceTypeID,'03' As MaxCategoryOfServiceType,'Lab/RadSvc' As CategoryOfServiceShortName,'Laboratory/Radiology Services'    As CategoryOfServiceName,'LAB-RAD'       	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        4       As CategoryOfServiceTypeID,'04' As MaxCategoryOfServiceType,'SNF Svc'    As CategoryOfServiceShortName,'SNF Services'                     As CategoryOfServiceName,'NURS-FAC'      	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        5       As CategoryOfServiceTypeID,'05' As MaxCategoryOfServiceType,'Physician'  As CategoryOfServiceShortName,'Physician Services'               As CategoryOfServiceName,'PHYSICIAN'     	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        6       As CategoryOfServiceTypeID,'06' As MaxCategoryOfServiceType,'MRDDAWaivr' As CategoryOfServiceShortName,'MRDDA Waiver Services'            As CategoryOfServiceName,'MRDDA-WAIVER'  	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        7       As CategoryOfServiceTypeID,'07' As MaxCategoryOfServiceType,'HomeHlth'   As CategoryOfServiceShortName,'Home Health Services'             As CategoryOfServiceName,'HOME-HEALTH'   	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        8       As CategoryOfServiceTypeID,'08' As MaxCategoryOfServiceType,'LTACSvcs'   As CategoryOfServiceShortName,'LTAC (LT Acute Care) Services'    As CategoryOfServiceName,'LTAC-SVCS'     	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        9       As CategoryOfServiceTypeID,'09' As MaxCategoryOfServiceType,'MntlHlthCl' As CategoryOfServiceShortName,'Mental Health Clinic Services'    As CategoryOfServiceName,'MNTL-HLTH-CLNC'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        10      As CategoryOfServiceTypeID,'10' As MaxCategoryOfServiceType,'EPSDTScrng' As CategoryOfServiceShortName,'EPSDT Screening Services'         As CategoryOfServiceName,'EPSDT-SCREENING'       As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        11      As CategoryOfServiceTypeID,'11' As MaxCategoryOfServiceType,'EPSDTSvcs'  As CategoryOfServiceShortName,'EPSDT Services'                   As CategoryOfServiceName,'EPSDT' 		As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        12      As CategoryOfServiceTypeID,'12' As MaxCategoryOfServiceType,'Dental'     As CategoryOfServiceShortName,'Dental Services'                  As CategoryOfServiceName,'DENTAL'        	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        13      As CategoryOfServiceTypeID,'13' As MaxCategoryOfServiceType,'OptomSvcs'  As CategoryOfServiceShortName,'Optometric Services'              As CategoryOfServiceName,'OPTOMETRIC-SVCS'       As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        14      As CategoryOfServiceTypeID,'14' As MaxCategoryOfServiceType,'DayTrtmt'   As CategoryOfServiceShortName,'Day Treatment Services'           As CategoryOfServiceName,'DAY-TRTMT-SVCS'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        15      As CategoryOfServiceTypeID,'15' As MaxCategoryOfServiceType,'Drug Svcs'  As CategoryOfServiceShortName,'Prescription Drug Services'       As CategoryOfServiceName,'DRUG'  		As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        16      As CategoryOfServiceTypeID,'16' As MaxCategoryOfServiceType,'ElderlyWvr' As CategoryOfServiceShortName,'Elderly Waiver Services'          As CategoryOfServiceName,'ELDERLY-WAIVER-SVC'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        17      As CategoryOfServiceTypeID,'17' As MaxCategoryOfServiceType,'WaterFiltr' As CategoryOfServiceShortName,'Water Filter Services'            As CategoryOfServiceName,'WATER-FILTER-SVCS'     As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        18      As CategoryOfServiceTypeID,'18' As MaxCategoryOfServiceType,'HearSvcs'   As CategoryOfServiceShortName,'Hearing Services'                 As CategoryOfServiceName,'HEARING-SVCS'  	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        19      As CategoryOfServiceTypeID,'19' As MaxCategoryOfServiceType,'ICFSvcs'    As CategoryOfServiceShortName,'ICF(Interm Care Fac) Services'    As CategoryOfServiceName,'ICF'   		As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        20      As CategoryOfServiceTypeID,'20' As MaxCategoryOfServiceType,'ICFMRSvcs'  As CategoryOfServiceShortName,'ICF MR Services'                  As CategoryOfServiceName,'ICF-MR'        	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        21      As CategoryOfServiceTypeID,'21' As MaxCategoryOfServiceType,'ResTrmtSvc' As CategoryOfServiceShortName,'Residential Treatment Services'   As CategoryOfServiceName,'RESL-TRMT-SVCS'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        22      As CategoryOfServiceTypeID,'22' As MaxCategoryOfServiceType,'FamPlngSvc' As CategoryOfServiceShortName,'Family Planning Services'         As CategoryOfServiceName,'FAM-PLNG-SVCS' 	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        23      As CategoryOfServiceTypeID,'23' As MaxCategoryOfServiceType,'FedQHC'     As CategoryOfServiceShortName,'Federally Qualified Health Ctr'   As CategoryOfServiceName,'FED-QUAL-HC'   	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        24      As CategoryOfServiceTypeID,'24' As MaxCategoryOfServiceType,'Med Sup'    As CategoryOfServiceShortName,'Medical Supply (DME) Services'    As CategoryOfServiceName,'MED-SUPPLY'    	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        25      As CategoryOfServiceTypeID,'25' As MaxCategoryOfServiceType,'ThpySvs'    As CategoryOfServiceShortName,'Therapy Services'                 As CategoryOfServiceName,'THPY-SVC'      	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        26      As CategoryOfServiceTypeID,'26' As MaxCategoryOfServiceType,'PsychSvcs'  As CategoryOfServiceShortName,'Psychiatric Services'             As CategoryOfServiceName,'PSYCHIATRIC-SVCS'      As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        27      As CategoryOfServiceTypeID,'27' As MaxCategoryOfServiceType,'InsPremium' As CategoryOfServiceShortName,'Insurance Premiums'               As CategoryOfServiceName,'INSURANCE-PREMIUM'     As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        28      As CategoryOfServiceTypeID,'28' As MaxCategoryOfServiceType,'NurseSvc'   As CategoryOfServiceShortName,'Nurse Practitioner Services'      As CategoryOfServiceName,'NURSE-SVC'     	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        29      As CategoryOfServiceTypeID,'29' As MaxCategoryOfServiceType,'Amb Surg'   As CategoryOfServiceShortName,'Ambulatory Surgery Services'      As CategoryOfServiceName,'AMB-SURG-CTR'  	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        30      As CategoryOfServiceTypeID,'30' As MaxCategoryOfServiceType,'SterilSvcs' As CategoryOfServiceShortName,'Sterilization Services'           As CategoryOfServiceName,'STERILIZATION-SVCS'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        31      As CategoryOfServiceTypeID,'31' As MaxCategoryOfServiceType,'Hospice'    As CategoryOfServiceShortName,'Hospice Services'                 As CategoryOfServiceName,'HOSPICE'       	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        32      As CategoryOfServiceTypeID,'32' As MaxCategoryOfServiceType,'ClinCntrSv' As CategoryOfServiceShortName,'Clinic Center Services'           As CategoryOfServiceName,'CLINIC-CENTER-SVCS'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        33      As CategoryOfServiceTypeID,'33' As MaxCategoryOfServiceType,'CaseMgmtSv' As CategoryOfServiceShortName,'Case Management Services'         As CategoryOfServiceName,'CASE-MGMT-SVCS'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        34      As CategoryOfServiceTypeID,'34' As MaxCategoryOfServiceType,'HemoHospSv' As CategoryOfServiceShortName,'Hospital Based Hemodialysis'      As CategoryOfServiceName,'HEMODIALYSIS-HOSP'     As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        35      As CategoryOfServiceTypeID,'35' As MaxCategoryOfServiceType,'HemoFreeSt' As CategoryOfServiceShortName,'Free Standing Hemodialysis'       As CategoryOfServiceName,'HEMODIALYSIS-FREES'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        36      As CategoryOfServiceTypeID,'36' As MaxCategoryOfServiceType,'HMOPymts'   As CategoryOfServiceShortName,'HMO Payments'                     As CategoryOfServiceName,'HMO-PYMT'      	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        37      As CategoryOfServiceTypeID,'37' As MaxCategoryOfServiceType,'AmbulSvcs'  As CategoryOfServiceShortName,'Ambulance Services'               As CategoryOfServiceName,'AMBULANCE-SVCS'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        38      As CategoryOfServiceTypeID,'38' As MaxCategoryOfServiceType,'ERAmbulSvc' As CategoryOfServiceShortName,'Emergency Ambulance Services'     As CategoryOfServiceName,'AMBULANCE-EMRGCY'      As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        39      As CategoryOfServiceTypeID,'39' As MaxCategoryOfServiceType,'AirTrans'   As CategoryOfServiceShortName,'Air Transportation Services'      As CategoryOfServiceName,'AIR-TRANS-SVCS'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        40      As CategoryOfServiceTypeID,'40' As MaxCategoryOfServiceType,'MedicareA'  As CategoryOfServiceShortName,'Medicare Part A Services'         As CategoryOfServiceName,'MEDICARE-PTA'  	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        41      As CategoryOfServiceTypeID,'41' As MaxCategoryOfServiceType,'MedicareB'  As CategoryOfServiceShortName,'Medicare Part B Services'         As CategoryOfServiceName,'MEDICARE-PTB'  	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        42      As CategoryOfServiceTypeID,'42' As MaxCategoryOfServiceType,'PractOther' As CategoryOfServiceShortName,'Other Practitioner Services'      As CategoryOfServiceName,'PRACTITIONER-OTHER'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        43      As CategoryOfServiceTypeID,'43' As MaxCategoryOfServiceType,'TransOSSI'  As CategoryOfServiceShortName,'Transportation OSSE Services'     As CategoryOfServiceName,'TRANS-OSSE'    	As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        44      As CategoryOfServiceTypeID,'44' As MaxCategoryOfServiceType,'GenNonBlng' As CategoryOfServiceShortName,'General Non-Billing Services'     As CategoryOfServiceName,'GEN-NON-BLNG-SVC'      As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        45      As CategoryOfServiceTypeID,'46' As MaxCategoryOfServiceType,'MCO Abortn' As CategoryOfServiceShortName,'MCO Abortion Payment'             As CategoryOfServiceName,'MCO-ABORTION-PYMT'     As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        46      As CategoryOfServiceTypeID,'47' As MaxCategoryOfServiceType,'DntlRdcdRt' As CategoryOfServiceShortName,'Dental Services Reduced Rate'     As CategoryOfServiceName,'DENTAL-REDUCED-RT'     As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        47      As CategoryOfServiceTypeID,'48' As MaxCategoryOfServiceType,'PrtcpntDir' As CategoryOfServiceShortName,'Participant Directed'             As CategoryOfServiceName,'PARTICIPANT-DIRECT'    As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        48      As CategoryOfServiceTypeID,'49' As MaxCategoryOfServiceType,'WelChldRed' As CategoryOfServiceShortName,'Well Child Reduction'             As CategoryOfServiceName,'WELL-CHILD-RED'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        49      As CategoryOfServiceTypeID,'56' As MaxCategoryOfServiceType,'NETBroker'  As CategoryOfServiceShortName,'NET Broker Services'              As CategoryOfServiceName,'NET-BROKER-SVC'        As COSMnemonic,  @DateTimeNow,   @AdminUser
		UNION ALL SELECT        50      As CategoryOfServiceTypeID,'98' As MaxCategoryOfServiceType,'Unknown'    As CategoryOfServiceShortName,'Unknown'                          As CategoryOfServiceName,'UNKNOWN'       	As COSMnemonic,  @DateTimeNow,   @AdminUser

	SET IDENTITY_INSERT [dbo].[CATEGORY_OF_SERVICE_TYPE] OFF;

END

GO
