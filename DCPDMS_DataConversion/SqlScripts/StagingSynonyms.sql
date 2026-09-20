
IF OBJECT_ID('dbo.SRC_ProviderLicenseTrkLog', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderLicenseTrkLog 
 CREATE SYNONYM SRC_ProviderLicenseTrkLog FOR [DC_PDMS_CONVSRC00].[dbo].[PLICNSTB_TRK_LOG];


IF OBJECT_ID('dbo.SRC_EnrollmentTrkLog', 'SN') IS NOT NULL
 DROP SYNONYM SRC_EnrollmentTrkLog 
 CREATE SYNONYM SRC_EnrollmentTrkLog FOR [DC_PDMS_CONVSRC00].[dbo].[PENROLTB_TRK_LOG];


IF OBJECT_ID('dbo.SRC_ProviderAddressTrkLog', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderAddressTrkLog 
 CREATE SYNONYM SRC_ProviderAddressTrkLog FOR [DC_PDMS_CONVSRC00].[dbo].[PADDRSTB_TRK_LOG];



IF OBJECT_ID('dbo.SRC_ProviderUpdt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderUpdt;
 CREATE SYNONYM SRC_ProviderUpdt FOR [DC_PDMS_CONVSRC00].[dbo].[DT_PROVDRTB_STG];


IF OBJECT_ID('dbo.SRC_ProviderLicenseUpdt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderLicenseUpdt;
 CREATE SYNONYM SRC_ProviderLicenseUpdt FOR [DC_PDMS_CONVSRC00].[dbo].[DT_PLICNSTB_STG];

IF OBJECT_ID('dbo.SRC_ProviderLicenseUpdtExcpt', 'SN') IS NOT NULL
   DROP SYNONYM SRC_ProviderLicenseUpdtExcpt 
 CREATE SYNONYM SRC_ProviderLicenseUpdtExcpt FOR [DC_PDMS_CONVSRC00].[dbo].[PLICNSTB_EXCPT];


IF OBJECT_ID('dbo.SRC_ProviderAddressUpdt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderAddressUpdt;
 CREATE SYNONYM SRC_ProviderAddressUpdt FOR [DC_PDMS_CONVSRC00].[dbo].[DT_PADDRSTB_STG];


IF OBJECT_ID('dbo.SRC_ProviderAddressUpdtExcpt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_ProviderAddressUpdtExcpt;
 CREATE SYNONYM SRC_ProviderAddressUpdtExcpt FOR [DC_PDMS_CONVSRC00].[dbo].[PADDRSTB_EXCPT];

 
IF OBJECT_ID('dbo.SRC_EnrollmentUpdt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_EnrollmentUpdt;
 CREATE SYNONYM SRC_EnrollmentUpdt FOR [DC_PDMS_CONVSRC00].[dbo].[DT_PENROLTB_STG];

 
IF OBJECT_ID('dbo.SRC_EnrollmentUpdtExcpt', 'SN') IS NOT NULL
 DROP SYNONYM SRC_EnrollmentUpdtExcpt;
 CREATE SYNONYM SRC_EnrollmentUpdtExcpt FOR [DC_PDMS_CONVSRC00].[dbo].[PENROLTB_EXCPT];

