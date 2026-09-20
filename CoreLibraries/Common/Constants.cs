using Corp.Core.Libraries.ProviderManagementReference;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Permissions;

namespace MAXIMUS.Core.Libraries
{
    //TODO: EDV A lot fo these constants should be enums and not integer constants
    public class Constants
    {
        public const string appAdminUserId = "3C14DD11-AF58-4CB7-972C-A0E51E47903F";
        public const string providerFinancialId = "B9B74D7B-E581-4C0B-A026-C1AAA602454A";
        public const string appDataFixTab = "868B5F3E-AE0D-4DB6-B368-BE2E61BCBCE7";
        public const string appWorkflowUserId = appAdminUserId;
        public const int appAdminPartyId = 750;
        public const string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
        public const string appPDMSAdminUserId = "E7854515-E84D-419B-B30E-720AE2C7EE4D";
        public const string emptyGuid = "00000000-0000-0000-0000-000000000000";
        public const string ApiKey = "89a17762076f18d7b971d55d18320205";
        public const string errortypeid = "243";
        public const string maximusSenderID = "MMISODJFS";
        public const string renderingProvider = "Rendering Provider";
        public const string supervisingProvider = "Supervising Provider";
        public const int correspondeceType = 1;
        public const string ProviderDataEntry = "Provider Data Entry";
        public const string MmisTypeForORR = "OhioRISE-provider/plan OOH respite contract";


        // general constants
        public const string InvalidRecordKeyString = "^";
        public const decimal InvalidRecordKeyDecimal = -1.792M;
        public const char InvalidRecordKeyChar = '^';
        public const int InvalidRecordKeyInt = -1757;

        public const string sqlParameterWithWildcards = "%{0}%";
        public const string pluralEnding = "s";
        public const string id = "Id";

        public const string dbfModDate = "LastModifiedDateTime";
        public const string dbfModUser = "LastModifiedUser";
        public const string dbfRecordStatus = "RecordStatus";
        public const string dbfArchiveStatus = "ArchiveStatus";
        public const string dbfFirstName = "FirstName";
        public const string dbfMiddleName = "MiddleName";
        public const string dbfLastName = "LastName";
        public const string dbfGender = "Gender";
        public const string dbfSuffix = "Suffix";
        public const string dbfBirthDate = "BirthDate";
        public const string dbfStreet1 = "Street1";
        public const string dbfStreet2 = "Street2";
        public const string dbfCounty = "County";
        public const string dbfStateName = "StateName";
        public const string dbfState = "State";
        public const string dbfZip5 = "Zip5";
        public const string dbfZip4 = "Zip4";
        public const string dbfCountry = "Country";
        public const string dbfName = "Name";
        public const string ViewProvider = "10013";

        // Raw database constants which map directly to field names in database
        public const string fModDate = "LAST_MODIFIED_DATE_TIME";
        public const string fModUser = "LAST_MODIFIED_USER";
        public const string fFirstName = "FIRST_NAME";
        public const string fLastName = "LAST_NAME";


        public const string dbpModDate = "@" + dbfModDate;
        public const string dbpModUser = "@" + dbfModUser;
        public const string dbpRecordStatus = "@" + dbfRecordStatus;
        public const string dbpArchiveStatus = "@" + dbfArchiveStatus;

        public const string CAQHPrimary = "Primary";

        public const string CAQHLicenseActive = "Active";

        public const string
            CAQHLicenseTypeDEA = "DEA"; // this value matches LICENSE_TYPE.LICENSE_TYPE_ABBREV since the lookup occurs 

        public const string CAQHLicenseTypeLicense = "License";

        public const string CAQHTaxTypeIndividual = "Individual";
        public const string CAQHTaxTypeGroup = "Group";
        public const string ProviderRoles = "ProviderOper,ProviderAdministrator,ProviderAgent,DeemedPresumptive";
        public const string MarkerSpecialtiesMMISTypeId = "96B,9CS,96H,96M,9PR,9QM,9Q3,760,761,762,601,45A,45S,45V,452,453,458,459,457";




        public const string ProviderServiceRoles =
            "ODMStateAdministrator,ProviderServices,Operator,ReadOnly,SiteVisitOperator,CredentialingSpecialist,LTCCHOP,EnrollmentSpecialist,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ODMCredentialing,CredentialingQuality,CredentialingSupervisor,ComplianceSpecialist,TechAdmin,LTCInitial/Revalidation,APMSpecialist";


        public const string ScreeningOperatorRoles = "Operator,CredentialingSpecialist,EnrollmentSpecialist,LTCInitial/Revalidation,LTCCHOP,ComplianceSpecialist"; ////SAM763 Allow compliance spl to update match results on provider screening page.




        public const string OperatorRoles =
            "ProviderServices,Operator,SanctionOperator,SiteVisitOperator,CredentialingSpecialist,EnrollmentSpecialist,LTCCHOP,LTCInitial/Revalidation,APMSpecialist";


        public const string CredentialingRoles =
            "CredentialingSpecialist,CredentialingChair,CommitteeQualitySpecialist,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ODMCredentialingQualityAssurance,CredentialingQualityAssurance,CredentialingSupervisor";

        public const string InternalRoles = "EnrollmentSpecialist,ReadOnly,SiteVisitOperator,SiteVisitAdministrator,ODMStateAdministrator,APMSpecialist,LTCCHOP,LTCInitial/Revalidation,CredentialingSpecialist,CredentialingSupervisor,CredentialingQualityAssurance,CommitteeQualitySpecialist,CredentialingChair,ODMCredentialingQualityAssurance,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ComplianceSpecialist,ComplianceSpecialistQualityAssurance,TechAdmin,InternalApplicationsEntry,InternalPaymentInnovation,RecipientEligibility,InfantMortalityLeadEntityAgent,ProviderServices,CHOPNotificationEmailGroup,ProviderRelationsManagement,ProviderRelations";

        public const string InitiateReconsiderationRoles = "ODMStateAdministrator,ComplianceSpecialist";

        public const string SpecialityInternalRoles = "EnrollmentSpecialist,ODMStateAdministrator,LTCCHOP,LTCInitial/Revalidation,InternalApplicationsEntry";
        //Variable to hold NA related roles with respect to Plan Name
        public const string NetworkAdequacyReportRolesByPlanName = "Aetna,Humana,AmeriHealth,Anthem,Buckeye,Caresource,Molina,UnitedHealthCare,AetnaOhioRISE";
        //This role will have access to all the NA Reports
        public const string NetworkAdequacyReportRole = "ODMNAReports";
        public const string StateAdminRole = "StateAdministrator";
        public const string AccountingRoles = "FAOperator1,FAOperator2";
        public const string DIDDRoles = "DIDDOperator,DIDDCommissioner,ProviderServicesCommissioner";
        public const string SiteVisitOperatorRole = "SiteVisitOperator";
        public const string ORFAWorkerRole = "ORFAWorker";
        public const string LTCWorkerRole = "LTCWorker";
        public const string DBHOperatorRole = "DBHOperator";
        public const string DDSOperatorRole = "DDSOperator";
        public const string StateReviewerRole = "StateReviewer";
        public const string DBHReviewerRole = "DBHReviewer";
        public const string DDSReviewerRole = "DDSReviewer";
        public const string CredentialSpecialistRole = "CredentialingSpecialist";
        //public const string CredentialCommitteeMemberRole = "CredentialCommitteeMember";
        //public const string CredentialCommitteeChairRole = "CredentialCommittee";
        //public const string CredentialCommitteeRole = "CredentialCommittee";
        public const string CredentialDefaultApprovalMessage = "Provider approved by Committee Chairman on {0}.";
        public const string ComplianceSpecialist = "ComplianceSpecialist";
        public const string EnrollmentSpecialistRole = "EnrollmentSpecialist";
        public const string RSFullReview1TypeId = "11";
        public const string RSReviewReasonId = "1";

        public const string ODMCredentialingIndvAffiliationRoles = "ODMCredentialingSupervisor,ODMCredentialingSpecialist";
        public const string LTCCHOP = "LTCCHOP";

        public const string NavigateToMITSRoles = "Hospital Contact,Hospice Enroll Search,Hospice Enroll Maintenance,Prior Authorization Submit,Prior Authorization Search,Eligibility,Claim Search,Claim Submission," +
            "View Provider Reports,Sign Approve LTC Cost Report,Prepare Save LTC Cost Report,View LTC Cost Report,Prepare Save MSP Cost Reports,Sign Certify MSP Cost Reports,View MSP Cost Reports,Prenatal Visit";

        // these are both the internal (maximus) and provider roles that aren't sub roles to ProviderAgent
        public const string RemittanceRedirectionInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser,ProviderServices";

        public const string MemberEligibilityInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,RecipientEligibility,InfantMortalityLeadEntityAgent,PNMSuperUser,ProviderServices";

        public const string FinancialRedirectionInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,LTCCHOP,LTCInitial/Revalidation,Internalapplicationentry,ComplianceSpecialist,ProviderAdministrator,PNMSuperUser";

        public const string CorrespondenceInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser,ProviderServices";
        public const string CostReportInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator," +
            "ODECostReport,ODHCostReport,ODMHospitalCostReport,ODMFQHCCostReport,ODMRHCCostReport,ODMLeadInvestigationCostReport,ODMMSPCostReport,ODMOHFCostReport,PNMSuperUser,ProviderServices,DODDRatesettingCostReportSupervisor,DODDCostReportStaff,ODMRatesetting,ODMCostReportStaff,ODMMDSSectionQReport";
        public const string ClaimsInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser,ProviderServices";
        public const string HospiceInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser,ProviderServices";
        public const string ProviderReportsInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser,ProviderServices";
        public const string AttachmentsInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,ProviderServices";
        public const string PriorAuthorizationInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,PNMSuperUser";
        public const string RecipientEligibilityInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ODMStateAdministrator,ProviderAdministrator,ProviderServices";
        public const string PaymentInnovationReportInternalRoles = "ProviderAdministrator";
        public const string ViewProviderInternalRoles = "ProviderAdministrator";
        public const string ProviderAdministratorRole = "ProviderAdministrator";
        
        public const string MaintenanceInternalRoles = "EnrollmentSpecialist,SiteVisitOperator,SiteVisitAdministrator,ODMStateAdministrator,APMSpecialist,LTCCHOP,LTCInitial/Revalidation,CredentialingSpecialist,CredentialingSupervisor,CredentialingQualityAssurance,CommitteeQualitySpecialist,CredentialingChair,ODMCredentialingQualityAssurance,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ComplianceSpecialist";

        // these are the sub roles to ProviderAgent
        public const string RemittanceRedirectionSubRoles = "View Remittance Advices";
        public const string FinancialRedirectionSubRoles = "1099 Information";
        public const string CorrespondenceSubRoles = "Provider Administrator,Hospital Contact,Hospice Enroll Maintenance,Hospice Enroll Search,Prior Authorization Search,Prior Authorization Submit,Eligibility,Claim Search,Claim Submission,1099 Information,View Remittance Advices,Correspondence";
        public const string CostReportSubRoles = "Hosp Cost Report Upload,Prepare Save LTC Cost Report,View LTC Cost Report," +
            "View MSP Cost Reports,View MSP Cost Report Due Date,Sign Certify MSP Cost Reports,Prepare Save MSP Cost Reports,RHC Cost Report Upload," +
            "FQHC Cost Report Upload,View FQHC Cost Report,View RHC Cost Report,Lead Investigation Cost Report Upload,View LI Cost Report,View Hospital Cost Report,MDS Report,View OHF Cost Report,OHF Cost Report Upload,Sign Approve LTC Cost Report,View Provider Reports,Trade Files,Sign Certify Hospital Cost Report,Sign Certify FQHC Cost Report,Sign Certify RHC Cost Report,Sign Certify OHF Cost Report";
        public const string ClaimsSubRoles = "Claim Search,Claim Submission";
        public const string HospiceSubRoles = "Hospice Enroll Search,Hospice Enroll Maintenance";
        public const string ProviderReportsSubRoles = "View Provider Reports";
        public const string AttachmentsSubRoles = "Prior Authorization Submit,Claim Submission";
        public const string EftAgentSubRoles = "EFT Agent";
        public const string ORPSearchSubRoles = "Prior Authorization Submit,Prior Authorization Search,Claim Submission,Claim Search";
        public const string ORPSearchInternalRoles = "EnrollmentSpecialist,ProviderRelationsManagement,ProviderRelations,ProviderServices,ODMStateAdministrator,PNMSuperUser";
        public const string SpecialtySearchSubRoles = "Enrollment Agent,Prior Authorization Submit,Prior Authorization Search,Claim Search,Claim Submission";
        public const string PriorAuthorizationSubRoles = "Prior Authorization Submit,Prior Authorization Search";
        public const string RecipientEligibilitySubRoles = "Eligibility";
        public const string PaymentInnovationReportSubRoles = "Provider Payment Innovation Reports Agent,APM Agent,MCO Agent";
        public const string CPCAgentSubRole = "APM Agent";
        public const string EnrollmentAgentSubRole = "Enrollment Agent";
        public const string CostReportManagementAgent = "Cost Report Management Agent";
        public const string HospiceReadOnlySubRoles = "Hospice Enroll Search";
        public const string HospiceMaintenanceSubRoles = "Hospice Enroll Maintenance";
        public const string P3SubscriberID = "SPBM,MITS";
        public const string P4SubscriberID = "SPBM,MITS,FI";

        public static List<string> CostReportsAllowedProviderTypes = new List<string>() { "01", "02", "04", "05", "12", "28", "86", "88", "89" };
        //Added 88, 89 as part of OHPNM-11359 & OHPNM-19056
        public static List<string> ProviderReportsAllowedProviderTypes = new List<string>() { "01", "02", "88", "89" };


        public const string sectionName = "W-9";
        public const string StateCode = "OH";

        public const string PDMSCareStatus = "New";
        public const string PDMSApplicationStatusComplete = "Complete";
        public const string PDMSApplicationStatusApproved = "Approved";
        public const string PDMSApplicationStatusNotProcessed = "Not Processed";

        public static class ContactType
        {
            public const string Individual = "P";
            public const string Organization = "B";
        }

        public static class IsPrimary
        {
            public const int ReferringProvider = 0;
            public const int PrimaryCareProvider = 1;
        }

        public static class ClaimsAdjudicationLevel
        {
            public const int Header = 1;
            public const int Details = 2;
        }

        public static class Claims_Adjudication_Levels
        {
            public const string Header = "Header";
            public const string Detail = "Detail";
        }

        public static class ClaimsType
        {
            public const string Dental = "0";
            public const string Institutional = "1";
            public const string Professional = "2";
        }
        public static class ClaimsTypeInitial
        {
            public const string Dental = "D";
            public const string Institutional = "I";
            public const string Professional = "P";
        }

        public static class ClaimsTypeInWords
        {
            public const string Dental = "DENTAL";
            public const string Institutional = "INSTITUTIONAL";
            public const string Professional = "PROFESSIONAL";
        }

        public static class ICD_Versions
        {
            public const string ICD10 = "ICD 10";
            public const string ICD9 = "ICD 9";
        }
        public static class Sequences
        {
            public const string Principal = "Principal";
            public const string Admitting = "Admitting";
            public const string PatientReasonforVisit = "Patient Reason for Visit";
            public const string ExternalCauseofInjury = "External Cause of Injury";
            public const string Other = "Other";
        }

        public static class ClaimsAdditionalPanelProviderTypes
        {
            public const string RenderingProvider = "Rendering Provider";
            public const string SupervisingProvider = "Supervising Provider";
            public const string AssistantSurgeon = "Assistant Surgeon";
            public const string OperatingPhysician = "Operating Physician";
            public const string OtherOperatingPhysician = "Other Operating Physician";
            public const string ReferringProvider = "Referring Provider";
            public const string PrimaryCareProvider = "Primary Care Provider";
            public const string ServiceFacilityProvider = "Service Facility";
            public const string OrderingProvider = "Ordering Provider";
        }

        public static class BorderStatesForOhio
        {
            public const string WestVirginia = "WV";
            public const string Kentucky = "KY";
            public const string Indiana = "IN";
            public const string Michigan = "MI";
            public const string Pennsylvania = "PA";

        }

        public static class BorderStateText
        {
            public const string No = "0";
            public const string Yes = "1";
        }
        public static class ClaimsPanelNames
        {
            public const string ReferringProvider = "ReferringProvider";
            public const string RenderingProvider = "RenderingProvider";
            public const string SupervisingProvider = "SupervisingProvider";
            public const string AssistantSurgeonProvider = "AssistantSurgeonProvider";
            public const string OtherOperatingPhysicianInformation = "OtherOperatingPhysicianInformation";
            public const string OperatingPhysicianInfo = "OperatingPhysicianInfo";
            public const string AttendingPhysicianInformation = "AttendingPhysicianInformation";
        }

        public static class UserRole
        {
            public const string Administrator = "ODMStateAdministrator";
            public const string DIDDCommissioner = "DIDDCommissioner";
            public const string DIDDOperator = "DIDDOperator";
            public const string FAOperator1 = "FAOperator1";
            public const string FAOperator2 = "FAOperator2";
            public const string Operator = "Operator";
            public const string ProviderAdministrator = "ProviderAdministrator";
            public const string ProviderOper = "ProviderOper";
            public const string ProviderServicesCommissioner = "ProviderServicesCommissioner";
            public const string ReadOnly = "ReadOnly";
            public const string StateAdministrator = "StateAdministrator";
            public const string SiteVisitOperator = "SiteVisitOperator";
            public const string SiteVisitAdministrator = "SiteVisitAdministrator";
            public const string AppealSpecialist = "AppealSpecialist";
            // public const string DeemedPresumptive = "DeemedPresumptive";
            // Credentialing Roles
            public const string CredentialingSpecialist = "CredentialingSpecialist";
            //public const string CredentialCommitteeMember = "CredentialCommitteeMember";
            public const string CommitteeQualitySpecialist = "CommitteeQualitySpecialist";
            public const string CredentialingSupervisor = "CredentialingSupervisor";
            public const string CredentialingQuality = "CredentialingQualityAssurance";
            public const string ODMCredentialing = "ODMCredentialingQualityAssurance";
            public const string ODMCredentialingSpecialist = "ODMCredentialingSpecialist";
            public const string ODMCredentialingSupervisor = "ODMCredentialingSupervisor";
            public const string CredentialingChair = "CredentialingChair";

            public const string ProviderAgent = "ProviderAgent";
            public const string EnrollmentSpecialist = "EnrollmentSpecialist";
            public const string LTCInitialRevalidation = "LTCInitial/Revalidation";
            public const string TechAdmin = "TechAdmin"; 
            public const string PNMSuperUser = "PNMSuperUser";
            public const string LTCCHOP = "LTCCHOP";
            public const string ComplianceSpecialist = "ComplianceSpecialist";
            public const string ProviderServices = "ProviderServices";
            public const string APMSpecialist = "APMSpecialist";

            public const string MITSAdmin = "MITSAdmin";
            public const string MITSBusinessRelations = "MITSBusinessRelations";
            public const string MITSProviderAssistance = "MITSProviderAssistance";
            public const string MITSProviderEnrollment = "MITSProviderEnrollment";
            public const string MITSUATSuperuser = "MITSUATSuperuser";
        }

        public static class ProviderType //akash	
        {
            public const string OTHER_ACCREDITED_HOME_HEALTH_AGENCY = "OTHER ACCREDITED HOME HEALTH AGENCY";
            public const string Non_Agency_Personal_Care_Aide = "Non-Agency Personal Care Aide";
            public const string Non_Agency_Home_Care_Attendant = "Non-Agency Home Care Attendant";
            public const string Private_Duty_Nurse = "Private Duty Nurse";
            public const string WAIVERED_SERVICES_ORGANIZATION = "WAIVERED SERVICES ORGANIZATION";
            public const string Waivered_Services_Individual = "Waivered Services Individual";
            public const string HOME_AND_COMMUNITY_BASED_ODA_ASSISTED_LIVING = "HOME AND COMMUNITY BASED ODA ASSISTED LIVING";
            public const string NURSING_FACILITY = "NURSING FACILITY";
            public const string NON_STATE_OPERATED_ICF_MR = "NON-STATE OPERATED ICF-DD";
            public const string NON_AGENCY_NURSE_RN_OR_LPN = "NON-AGENCY NURSE -- RN OR LPN";
            public const string UNPAID_SUPPORT_BROKER = "UNPAID_SUPPORT_BROKER";
        }

        public static class LTCFaciitiesProviderTypeID
        {
            public const string BuildingandWingIDforLTCFacilitesOnly = "LT";
        }


        public const string
            AspnetUserAccountCreate = "AspNetUserAccount-CreateTime"; // Date/Time last aspnet account created

        public const string
            AspnetUserAccountSeconds =
                "AspNetUserAccount-Seconds"; // Number of seconds allowed between each aspnet account creation

        public const string StateOwnedEntityTaxID = "626001445";
        public const string StateOwnedEntityProviderTypeName = "State Owned Entity";
        public const string Exempt = "Exempt";

        public const string HospitalProviderTypeName = "Hospitals (HOSP)";
        public const string ApplicationPDF = "ApplicationPDF";

        // MMIS constants
        public const string MMISRecordAccepted = "ACCEPTED";
        public const string MMISRecordRejected = "REJECTED";
        public const string AffiliateProfitStatus = "88";
        public const string TrimChar = " ";
        public const string TrimCharNFOCUS = "0";
        public const string DefaultOwnerType = "05";

        // OnBase constants
        public const string OnBaseSuccess = "SUCCESS";
        public const string OnBaseError = "ERROR";
        //Registration complete Time Limit Message
        public const string RegApplicationCompleteTimeLimitMessage = "Please note that you have {0} days to complete your application. After {1} days, your information will be deleted and you will have to re-start the process from the beginning of the application.";
        //Registration due Time Message
        public const string RegApplicationDueTimeMessage = "You will have {0} days to submit your update.  After {0} days, your information will be removed, and you will have to restart your update.";

        // messages for all custom generated exceptions
        public static class CustomExceptionMessages
        {
            public const string LengthZero = "Length must be > 0";
            public const string RecordCreateException = "A {0} cannot be created in this service call.";
            public const string RecordDeleteException = "A {0} cannot be deleted in this service call.";
            public const string RecordUpdateException = "A {0} cannot be updated in this service call.";
        }

        // Resolving Action
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// To regeneration list of the database run the following query
        /// SELECT
        ///  'public const int ' + REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(RESOLVING_ACTION_TYPE, '= N' , ''),'/',''),' ',''),'–',''),'=Y',''),'-','')
        ///  + ' = ' + CONVERT(varchar(4),RESOLVING_ACTION_TYPE_ID) + ';'
        /// FROM RESOLVING_ACTION_TYPE
        /// ORDER BY RESOLVING_ACTION_TYPE_ID        
        /// </remarks>
        public static class ResolvingActionType
        {
            public const int Rejection = 1;
            public const int AdditionalInfo = 2;
            public const int Termination = 3;
            public const int Override = 4;
            public const int UpdateRecord = 5;
            public const int Warning = 6;
            public const int SystemGeneratedEmail = 7;
        }

        // Status passed to tell the Service Address how to handle the storing of the record
        public static class ActionCodeType
        {
            public const int Confirmed = 1;
            public const int Modified = 2;
            public const int Terminated = 3;
        }

        // Error Status
        public static class ErrorStatusType
        {
            public const int Open = 1;
            public const int Pending = 2;
            public const int Closed = 3;
            public const int DataUpdate = 4;
        }

        // Error Category type
        public static class ErrorCategoryType
        {
            public const int PDMSError = 1;
            public const int CAQHError = 2;
            public const int MMISError = 3;
            public const int RegError = 4;
            public const int PaperReqError = 5;
        }

        public static class WaiverServiceCategoryType
        {
            public const int PASSPORT = 1;
            public const int Choices = 2;
            public const int AssistedLiving = 3;
            public const int Other = 4;
        }

        // static directories
        public static class Directories
        {
            public const string Archive = @"\Archive\";
            public const string Test = @"\Test\";
        }

        // Log Strings
        public static class LogString
        {
            public const string ActionFileNotRetrieved = "*** File not retrieved***";
            public const string ActionFileNotSent = "*** File not sent***";

            public const string AppSettingsValueDisabled =
                "{0}, AppSetting {1} is either set to false or does not exist";

            public const string BeginTaskName = "Begin Task '{0}'";
            public const string EndTaskName = "End Task '{0}'";
            public const string ExceptionEncountered = "Exception encountered: {0}";

            public const string ExceptionEncounteredDetail =
                "Exception encountered [Message :: Source :: Data :: StackTrace]: {0} :: {1} :: {2} :: {3}";

            public const string ExceptionInvalidMMISError = "Invalid error number provided by the MMIS [{0}]";

            public const string ExtractErrorRecordFailure =
                "Failure when attempting to insert/update record [{0}; {1}; {2}] ";

            public const string ExtractLoadProvider = "Loading provider: Action={0}, CAQH PIN={1}, File={2}";
            public const string ExtractLoadReplicaApp = "Loading replica app: CAQH PIN={0}, File={1}";

            public const string ExtractLoadPDMSProvider =
                "Loading provider into PDMS from Staging: Action={0}, CAQH PIN={1}";

            public const string FileCountToProcess = "Files to process: {0}";
            public const string FileGenerated = "File generation complete [File:{0}]";
            public const string FileLoadFailure = "File load failed, check file format. ";
            public const string JobServiceStartStopMessage = "Job Service service {0}. [ThreadId: {1}]";
            public const string JobServiceNoActiveJobs = "No active jobs";
            public const string LoadingExtractRecords = "Loading extract records: {0}";
            public const string LoadingExtractRecordsComplete = "Extract load complete: {0}";
            public const string LoadingFile = "Loading file: {0}";
            public const string LoadingFileComplete = "File load complete: {0}";
            public const string LoadingFileNotFound = "File match not found or substantial format issues - verify file: {0}";
            public const string LoadingRecords = "Loading records [Total Records: {0}; Error Records: {1}]";
            public const string LoadingConfig = "Loading configuration variables";
            public const string LoadingSkipped = "Skipping load of file: {0}, loaded previously";
            public const string LoadingDirectory = "Load files from directory: {0}";
            public const string RetrievingMMISRecord = "Record Retrieved: Party ID: {0}";

            public const string RetrievingMMISRecordError =
                "Record error during process: Staging PK: {0}, SakTrans: {1}";

            public const string RetrievingMMISRecordNotProcessed =
                "Record will not be processed due to empty Status Code: Staging PK: {0}, SakTrans: {1}";

            public const string RetrievingMMISRecordGroupFailed =
                "Error when attempted to update group information: Staging PK: {0}, MedicaidId {1}";

            public const string RetrievingMMISRecordsStart = "Retrieving record(s) from MMIS";
            public const string RetrievingB2PRecordsStart = "Retrieving payment record(s) from Bill2Pay";
            public const string RetrievingMMISRecordsEnd = "Record(s) retrieval complete";
            public const string RetrievingB2PRecordsEnd = "Bill2Pay Record(s) retrieval complete";
            public const string RetrievingMMISRecordNotFound = "Retrieved record cannot be found: Party id: {0}";

            public const string RetrievingApplicationFeeRecordNotFound =
                "Application fee info not in the returned file for Reg id: {0}";

            public const string RetrievingMMISGroupAffiliationStart =
                "Retrieving affiliations for group, party id: {0}";

            public const string SumittingMMISGroupAffiliationStart =
                "Submitting group affiliations, total number of groups: {0}";

            public const string SumittingMMISGroupAffiliationEnd = "End Submitting affiliations";

            public const string SumittingMMISGroupAffiliationGroupStart =
                "Begin Submitting affiliations for group party id: {0}";

            public const string SumittingMMISGroupAffiliationGroupEnd =
                "End Submitting affiliations for group party id: {0}";

            public const string SumittingMMISMCOAffiliationGroupStart =
                "Begin Submitting affiliations for MCO party id: {0}";

            public const string SumittingMMISMCOAffiliationGroupEnd =
                "End Submitting affiliations for MCO party id: {0}";

            public const string SubmittingMMISAdditionalSpecialtyStart =
                "Start Specialty Submit for Staging Specialty ID: {0}";

            public const string SubmittingMMISAdditionalSpecialtyEnd =
                "Start Specialty Submit for Staging Specialty ID: {0}";

            public const string SumittingMMISIndividualAffiliateStart =
                "Submitting group member, group party id: {0}, member party id: {1}";

            public const string SumittingMMISIndividualAffiliateEnd = "End Submitting group member party id: {0}";

            public const string SessionError = " [Error: {0}]";
            public const string SessionEstablishing = "Establishing sFTP session to {0}";
            public const string SessionResults = "Results details: [{0}] {1}";
            public const string SessionRetrievingFiles = "Attempting to retrieve file(s) [{0}]";
            public const string SessionSendingFiles = "Attempting to send file(s) [{0}]";

            public const string SubmittingMMISRecord =
                "Record Submitted: Staging PK: {0}, Party ID: {1}, Transaction Queue ID: {2}";

            public const string SubmittingMMISRecordNotProcessed =
                "Record will not be processed due to empty Status Code: Staging PK: {0}, SakTrans: {1}";

            public const string SubmittingMMISRecordsStart = "Submitting record(s) to MMIS Record Count: {0}";
            public const string SubmittingMMISRecordsEnd = "Record(s) submission complete";

            public const string SubmittingMMISGroupsStart =
                "Submitting groups to MMIS Record Count: {0}, Transaction Type: {1}";

            public const string SubmittingMMISGroupsEnd = "Groups submission complete Transaction Type: {0}";
            public const string SubmittingMMISIndividualsStart = "Submitting individuals to MMIS Record Count: {0}";
            public const string SubmittingMMISIndividualsEnd = "Individuals submission complete";

            public const string MethodSignature = "{0}::{1}";
            public const string GenericMethodSignature = "{0}::{1} Name:{2}";

            public const string ProcessingStarted = "Processing started: {0}";
            public const string ProcessingComplete = "Processing complete";
            public const string ProcessingFilesInDir = "Processing files in directory: {0}";




            public const string PracticeOrganization =
                "Failure when adding Practice Organization [PartyId {0}, MedicaidPK {1}, RoleType {2}]";



            public const string TestDirectoryNotFound =
                "The test directory does not exist, no records will be imported";

            public const string TestDirectoryLoad =
                "InterfaceTestingCAQH parameter set, looping all over all files located in test directory: {0}";

            public const string UpdateProviderFileIssues = "Provider update file issues: CAQH PIN={0}, File={1}";
            public const string WorkflowTaskFailed = "Workflow Task not correctly configured. {0}; {1}; Exception: {2}";

            public const string WorkflowParameterNotNumeric =
                "Workflow parameter is not a number or not defined.  Parm: {1} Value: {2}; Task: {0}.";

            public const string WorkflowParameterNotBoolean =
                "Workflow parameter is not a true/false or not defined.  Parm: {1} Value: {2}; Task: {0}.";

            public const string WorkflowParameterNotValid =
                "Workflow parameter is not valid or not defined.  Parm: {1} Task: {0}.";


            public const string WorkflowError = "Workflow error encountered.  Error: {1} Task: {0}.";
            public const string ExtractLoadIndex = "Loading extract index: File={0}";



            public const string WorkflowParameterNotDefined =
                "Workflow parameter is not defined.  Parm: {1} Task: {0}.";

            //ICDS_ODA_WAIVER_SERVICE
            public const string ICDS_ODA_Initiate = "Loading AppSettings Configurations";

            public const string Delegate_Affiliate_Initiate = "Loading AppSettings Configurations";

            //public const string ICDS_ODA_ImportFile = "Loading Configurations";
            //public const string ICDS_ODA_ImportFile = "Loading Configurations";
            //public const string ICDS_ODA_ImportFile = "Loading Configurations";

            public const string Delegate_Affiliates_Processing = "Retrieving Records from Delegate File";

            public const string SSAffiliation_Processing = "Started SSA Afffiliations job processing";
            public const string SSAffiliation_Ended = "Finished SSA Afffiliations job processing";

            public const string TransactionMonitoring_Processing = "Started TransactionMonitoring job processing";
            public const string TransactionMonitoring_Ended = "Finished TransactionMonitoring job processing";

            public const string GroupAffiliation_Processing = "Started Group Affiliations job processing";
            public const string GrroupAffiliation_Ended = "Finished Group Affiliations job processing";

            //onbase doc upload job
            public const string OnBaseDocUploadJobStart = "Started OnBase Doc Upload Job processing";
            public const string OnBaseDocUploadJobEnd = "Finished OnBase Doc Upload Job processing";

        }

        /// <summary>
        ///     PDMS Status Types
        /// </summary>
        /// <remarks>
        /// To regeneration list of the database run the following query
        /// SELECT
        /// 'public const int ' + REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(PDMS_STATUS_DESCRIPTION, '= N' , ''),'/',''),' ',''),'–',''),'=Y',''),'-','')
        /// + ' = ' + CONVERT(varchar(4),PDMS_STATUS_TYPE_ID) + ';'
        /// FROM PDMS_STATUS_TYPE
        /// ORDER BY PDMS_STATUS_TYPE_ID
        /// </remarks>
        public static class PDMSStatusType
        {
            public const int PendingCAQHSubmission = 1;
            public const int SubmittedtoCAQH = 2;
            public const int SubmittedtoMMIS = 3;
            public const int PendingMMISSubmission = 4;
            public const int Processed = 5;
            public const int Rejected = 6;
            public const int Errors = 7;
            public const int Terminated = 8;
            public const int PendingMMISTerm = 9;
            public const int PendingCAQHDelete = 10;
        }




        // MMIS Status Types
        public static class MMISStatusType
        {
            public const int Submitted = 1;
            public const int Processed = 201;
            public const int Errors = 202;
            public const int NA = 203;
        }

        // The true/false value used in some of the reference fields in the PDMS data model
        public static class BooleanDataModel
        {
            public const string True = "Y";
            public const string False = "N";
        }

        // The true/false value used in some of the reference fields in the PDMS data model
        public static class BooleanCAQH
        {
            public const string True = "T";
            public const string False = "F";
        }

        // The CAQH Xml Import file Associate Type (under a practice)
        public static class CAQHAssociateType
        {
            public const string OfficeMgr = "Office Mgr";
            public const string MailTo = "Billing Contact";
            public const string PayTo = "Payment Contact";
            public const string Credentialing = "Credentialing Contact";
        }

        // CAQH Data Summary File Statuses
        public static class CAQHSummaryFileStatus
        {

            public const string New = "N";
            public const string ReattestNoChanges = "U";
            public const string ReattestChanges = "R";
        }

        // CAQH Status Types (List are the ones currently needed, add more if necessary)
        public static class CAQHStatusType
        {
            public const int RemovedFromRoster = 15;
            public const int NoCAQHResponse = 16;
        }

        // Dashboard Table Types - Used by AdminHome.aspx
        public static class DashboardTableType
        {
            public const int IndividualNewReg = 1;
            public const int IndividualUpdateReg = 2;
            public const int IndividualUpdateOwner = 3;
            public const int IndividualRevalidation = 4;
            public const int GroupWithMembersNewReg = 5;
            public const int GroupWithMembersUpdateReg = 6;
            public const int GroupWithMembersUpdateOwner = 7;
            public const int GroupWithMembersRevalidation = 8;
            public const int InstitutionalOrFacilityNewReg = 9;
            public const int InstitutionalOrFacilityUpdateReg = 10;
            public const int InstitutionalOrFacilityUpdateOwner = 11;
            public const int InstitutionalOrFacilityRevalidation = 12;
            public const int OrganizationNewReg = 13;
            public const int OrganizationUpdateReg = 14;
            public const int OrganizationUpdateOwner = 15;
            public const int OrganizationRevalidation = 16;
            public const int PharmacyNewReg = 17;
            public const int PharmacyUpdateReg = 18;
            public const int PharmacyUpdateOwner = 19;
            public const int PharmacyRevalidation = 20;
            public const int OrderingReferringPrescribingNewReg = 21;
            public const int OrderingReferringPrescribingUpdateReg = 22;
            public const int OrderingReferringPrescribingUpdateOwner = 23;
            public const int OrderingReferringPrescribingRevalidation = 24;
            public const int ChangeOfOperatorNewReg = 25;
            public const int ChangeOfOperatorUpdateReg = 26;
            public const int ChangeOfOperatorUpdateOwner = 27;
            public const int ChangeOfOperatorRevalidation = 28;
            public const int MCPOnlyNewReg = 29;
            public const int MCPOnlyUpdateReg = 30;
            public const int MCPOnlyUpdateOwner = 31;
            public const int MCPOnlyRevalidation = 32;
            public const int CPCNewReg = 33;
            public const int CPCUpdateReg = 34;
            public const int CPCUpdateOwner = 35;
            public const int CPCRevalidation = 36;

            // Waiver Types
            public const int WaiverODMNewReg = 101;
            public const int WaiverODMUpdateReg = 102;
            public const int WaiverODMUpdateOwner = 103;
            public const int WaiverODMRevalidation = 104;

            public const int WaiverODANewReg = 105;
            public const int WaiverODAUpdateReg = 106;
            public const int WaiverODAUpdateOwner = 107;
            public const int WaiverODARevalidation = 108;

            public const int WaiverDODDNewReg = 109;
            public const int WaiverDODDUpdateReg = 110;
            public const int WaiverDODDUpdateOwner = 111;
            public const int WaiverDODDRevalidation = 112;

            public const int NonMedicaidDODDNewReg = 113;
            public const int NonMedicaidDODDUpdateReg = 114;
            public const int NonMedicaidDODDUpdateOwner = 115;
            public const int NonMedicaidDODDRevalidation = 116;


            // Table Id's above 200 will be directed to WaiverProviderSearch
            public const int SendToWaiverSearch = 200;
        }

        // The type of call to the data access service
        public static class DataAccessCallType
        {
            public const string SQL = "SQL";
            public const string StoredProcedure = "Stored Procedure";
        }

        // The PROVIDER_STAGING.ACTION value
        public static class MMISAction
        {
            public const string Add = "A";
            public const string Update = "U";
        }

        // Provider Status Change Type
        public static class ProviderStatusChangeType
        {
            public const int CAQH = 1;
            public const int MMIS = 2;
            public const int PDMS = 3;
        }

        // Provider Category Types
        public static class ProviderCategoryTypeID
        {
            public const int Individual = 1;
            public const int Group = 2;
            public const int Organization = 3;
            public const int EntityFacility = 4;
            public const int Pharmacy = 5;
            public const int GroupMemberProfile = 6;
        }
        public static class ClaimSearchSoapAction
        {
            public const string ClaimSearchSoap = "\"http://service.operationmgmt.fi/ClaimsService/SearchClaims\"";
        }
        // MH 10/27/2015 - Do not add specialty type ids to this list as the specialty type ids can change on each data refresh
        // Please add specialty name in appsettings and use that instead
        public static class SpecialtyTypeID
        {
            public const int ICF = 2180;
            public const int ICFOther = 2183;
            public const int SNF = 2181;
            public const int SNFOther = 2185;
            public const int ICF_IID = 2148;
            public const int ICF_IIDOther = 2187;

            public const int NF_VENT = 349;
            public const int NF_WEAN = 354;
            public const int Pharmacy_340B = 300;
            public const int ODA_WAIVER = 258;
            public const int HCBS_ASSISTED_LIVING = 307;
            public const int DODD_WAIVER = 259;
        }


        public static class MMISSpecialtyType
        {
            public const string HOSPICE = "82";
            public const string ASSISTEDLIVINGSERVICES = "75";
            public const string CPCSINGLEPRACTICE = "990";
            public const string CPCPRACTICEPARTNERSHIP = "991";
            public const string CPCPEDIATRICS = "997";
            public const string ODAWaiver = "480";
            public const string DODDWaiver = "490";
            public const string HCBSAssistedLiving = "740";
            public const string Pharmacy_340B = "701";
            public const string OHRISE_FMS = "FMS";
            public const string OHRISE = "OHS";
            public const string OHRISE_CANS = "ORC";
            public const string OHRISE_CME = "ORE";
            public const string OHRISE_MRSS = "ORM";
            public const string OHRISE_IHBT = "847";
            public const string OHRISE_WaiverHomeRespite = "ORR";
            public const string MATERNALANDINFANTSUPPORT = "MIS";
            public const string OHRISE_MCOProviderOnly = "190";
            public const string OHRISE_CANSValue = "408";
            public const string FAMILY_CONNECT = "387";
            public const string OHRISE_BHR = "BHR";
            public const string OHRISE_TSS = "TSS";
            public const string LACTATION_CONSULTANT_SERVICES = "091";
            public const string PEDIATRIC_RECOVERY_SPECIALTY = "100";
            public const string STRUCTIRED_FAMILY_CAREGIVER_SERVICE = "45P";
        }

        public static class ContractTypeID
        {
            public const int DIDD = 1;
            public const int ICF_IID = 2;
        }

        #region "Registration Constants"

        // Registration Modified Status Type
        public static class RegistrationModifiedStatusType
        {
            public const int NoChange = 1;
            public const int Changed = 2;
            public const int Deleted = 3;
            public const int Inserted = 4;
        }

        // Registration Page Type
        public static class RegistrationPageType
        {
            public const int Identification = 1;
            public const int LicensesClassifications = 2;
            public const int PracticeLocations = 3;
            public const int GroupAffiliations = 4;
            public const int OwnerInformation = 5;
            public const int SubstituteW9Form = 6;
            public const int ACHAuthorization = 7;
            public const int Agreements = 8;
            public const int SummarySubmissionHidden = 9; // Client requested Summary be hidden
            public const int Services = 10;
            public const int Contracts = 11;
            public const int Certification = 12;
            public const int ProviderScreening = 13;
            public const int AffiliationScreening = 14;
            public const int OwnerScreening = 15;
            public const int SiteVisitScreening = 16;
            public const int ApplicationFee = 17;
            public const int SubstituteW4Form = 18;
            public const int HouseholdMembers = 19;
            public const int HouseholdMemberScreening = 20;
            public const int GroupAndFacilityAffiliation = 21;
            public const int BackgroundCheck = 22;
            public const int OrientationInformation = 23;
            public const int DMEInformation = 24;
            public const int WaiverServices = 25;
            public const int MCOAffiliations = 27;
            public const int ProviderCredentialing = 28;
            public const int ContractMaintenance = 34;
            public const int ProviderFinancial = 32;
            public const int ERemittanceAdvice = 31;
            public const int UserMaintenance = 35;
			public const int ProviderReview = 36;
            public const int HearingRights = 61;
        }

        public static class RegistrationPageName
        {
            //As defined in REG_PAGE_SETTING (may differ from reg page type...good candidate for refactoring how these 2 tables relate to each other).
            public const string Services = "Services";
            public const string Contracts = "Contracts";
            public const string Certification = "Certification";
            public const string ApplicationFee = "Application Fee";
            public const string Agreements = "Agreements";
            public const string ProviderScreening = "Provider Screening";
            public const string OwnerScreening = "Owner Screening";
            public const string IndividualMemberScreening = "Individual Member Screening";
            public const string HouseholdMemberScreening = "Household Member Screening";
            public const string SiteVisitScreening = "Site Visit Screening";
            public const string ProviderSiteVisit = "Provider Site Visit";
            public const string GroupAffiliations = "Individual Providers";
            public const string FingerprintandBackgroundCheck = "Fingerprint and Background Check";
            public const string FingerprintCheck = "Fingerprint Check";
            public const string OrientationSession = "Orientation Session";
            public const string OrientationInformation = "Orientation Session Screening";
            public const string SubstituteW9 = "W9 Form";
            public const string MCOAffiliations = "MCO Affiliations";
            public const string BackgroundCheck = "Background Check";
            public const string ProviderFinancial = "Financial Information";  //AKASH Added
            public const string Reconsideration = "Reconsideration";

        }

        // Registration Provider Status
        public static class RegistrationProviderStatusTypeId
        {
            public const int Complete = 1;
            public const int NotComplete = 2;
            public const int Modified = 3;
        }

        //State Review Status
        public static class StateReviewStatusID
        {
            public const int Approve = 1;
            public const int Deny = 2;
            public const int Terminate = 3;

        }

        //LTC Review Status
        public static class LTCReviewStatusID
        {
            public const int Approve = 1;
            public const int NotCompleted = 2;
            public const int Deny = 3;
            public const int Terminate = 4;

        }

        //DBH Review Status
        public static class DBHReviewStatusID
        {
            public const int Approve = 1;

            public const int Deny = 2;
            public const int Terminate = 3;

        }

        //DDS Review Status
        public static class DDSReviewStatusID
        {
            public const int Approve = 1;

            public const int Deny = 2;
            public const int Terminate = 3;

        }

        //Financial Review Status
        public static class FinancialReviewStatusID
        {
            public const int Completed = 1;
            public const int NotCompleted = 2;
            public const int Pending = 3;

        }

        //DHCF Review Status
        public static class DHCFReviewStatusID
        {
            public const int Approve = 1;
            public const int Deny = 2;
            public const int Terminate = 3;

        }

        //DHCF Application Fee Review Status
        public static class DHCFApplicationFeeReviewStatusID
        {
            public const int Approve = 1;
            public const int Deny = 2;


        }

        //State Application Fee Review Status
        public static class StateApplicationFeeReviewStatusID
        {
            public const int Approve = 1;
            public const int Deny = 2;


        }

        public static class StatusType
        {
            public const string RegistrationProviderServicesStatusTypeId = "PSStatusTypeId";
            public const string RegistrationProviderStatusTypeId = "ProviderStatusTypeId";
        }

        // Registration Provider Services Status
        public static class RegistrationProviderServicesStatusTypeId
        {
            public const int Approved = 1;
            public const int ReturnToProvider = 2;
            public const int Pending = 3;
            public const int NotApplicable = 4;
        }

        // Registration Status
        public static class RegistrationStatusTypeId
        {
            public const int Pending = 1;
            public const int Submitted = 2;
            public const int Approved = 3;
            public const int Deny = 4;
            public const int ReturnToProvider = 5;
            public const int CloseSanctionReview = 6;
            public const int ReinstateProvider = 7;
            public const int Suspended = 23;
            public const int TerminateProvider = 9;
            public const int UpdateProvider = 10;
            public const int Save = 11;
            public const int Deleted = 12;
            public const int ReturnToProviderServices = 13;
            public const int StateReview = 14;
            public const int SiteVisit = 15;
            public const int Disenrolled = 17;
            public const int ApplicationFeeApproved = 19;
            public const int ReturnToSiteVisit = 20;
            public const int ReturnToProviderReview = 21;
            public const int ReturnToStateReview = 22;
            public const int PendingClosure = 24;
            public const int PendingCHOP = 25;
            public const int ReturnToProviderForSiteVisit = 26;
            public const int NotProcessed = 27;
        }

        public static class RegistrationStatusType
        {
            public const string Pending = "Not Submitted";
            public const string Submitted = "Submitted";
            public const string Approved = "Approved";
            public const string Deny = "Denied";
            public const string ReturnToProvider = "Return to Provider";

            public const string TerminateProvider = "TerminateProvider";

            public const string Deleted = "Deleted";
            public const string ReturnToProviderServices = "Return to Provider Services";
            public const string StateReview = "State Review";
            public const string SiteVisit = "Site Visit";
            public const string Disenrolled = "Disenrolled";
            public const string ApplicationFeeApproved = "Application Fee Approved";
            public const string ReturnToSiteVisit = "Return To Site Visit";
            public const string ReturnToProviderReview = "Return To Provider Review";
            public const string NotProcessed = "Not Processed";
        }

        //Appeal Status
        public static class AppealStatus
        {
            public const int NoticeSent = 1;
            public const int NoAppeal = 2;
            public const int ProviderRequestedAppeal = 3;
            public const int AppealWon_ReinstateProvider = 4;
            public const int AppealLost_ProcessDenial_Termination = 5;
        }

        //Appeal Status
        public static class AppealStatusName
        {
            public const string NoticeSent = "Notice Sent";
            public const string NoAppeal = "No Appeal";
            public const string ProviderRequestedAppeal = "Provider Requested Appeal";
            public const string AppealWon_ReinstateProvider = "Appeal Won Reinstate Provider";
            public const string AppealLost_ProcessDenial_Termination = "Appeal Lost Process Denial Termination";
        }

        // Registration Program Status
        public static class RegistrationProgramStatusTypeId
        {
            public const int New = 1;
            public const int Maintenance = 2;
            public const int Terminated = 3;
            public const int Denied = 4;
            public const int Suspended = 5;
            public const int Conversion = 6;
            public const int Revalidation = 7;
            public const int MonthlyScreening = 8;
            public const int Disenrolled = 9;
            public const int PendingTermination = 10;
            public const int EnforceMoratoria = 11;
            public const int AnnualScreening = 12;
            public const int NotProcessed = 13;
        }

        // Registration Subcontractor Type
        public static class RegistrationSubcontractorType
        {
            public const int OwnershipAtLeast5Percent = 1;
            public const int BusinessDoneLast5Years = 2;
        }

        #endregion

        // Roster Status
        public static class RosterStatusType
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }

        // TimeZone Strings
        public static class TimeZone
        {
            public const string AST = "Alaskan Standard Time";
            public const string CST = "Central Standard Time";
            public const string EST = "Eastern Standard Time";
            public const string HST = "Hawaiian Standard Time";
            public const string MST = "Mountain Standard Time";
            public const string PST = "Pacific Standard Time";
        }

        // The current status of the line item as it is routed through the approvals (FCR_ITEM_STATUS)
        public static class FCRItemStatus
        {
            public const string Empty = "";
            public const string PendingBureauApproval = "B";
            public const string PaymentComplete = "C";
            public const string PendingFiscalApproval = "F";
            public const string PendingMMISSubmission = "M";
            public const string PendingPreparerApproval = "P";
            public const string SubmittedToMMIS = "S";
            public const string InternalReturn = "P01";
            public const string MMISReturn = "P02";
            public const string Cancelled = "X";
        }

        public static class IncidentReviewCaseStatus
        {
            public const int New = 1;
            public const int ReviewComplete = 2;
            public const int Terminated = 3;
        }

        public static class IncidentReviewCaseStatusstr
        {
            public const string New = "New";
            public const string ReviewComplete = "ReviewComplete";
            public const string ReviewNotComplete = "ReviewNotComplete";
            public const string Terminate = "Terminate";
            public const string NodIssued = "IssueNod";
        }
        // MMIS Service Location Matching
        public static class MMISServiceLocationMatch
        {
            public const int Terminated = 0;
            public const int Matched = 1;
            public const int SystemMatched = 2;
            public const int SystemTerminated = 3;
        }

        // Provider Note Type
        public static class ProviderNoteTypeId
        {
            [Description("Error Handling")] public const int ErrorHandling = 1;

            [Description("Provider Contact")] public const int ProviderContact = 2;

            [Description("Registration Rejection")]
            public const int RegistrationRejection = 3;

            [Description("Termination")] public const int Termination = 4;

            [Description("Miscellaneous")] public const int Miscellaneous = 5;

            [Description("Note On Approved")] public const int NoteOnApproved = 6;

            [Description("Internal")] public const int Internal = 7;

            [Description("Disenrollment")] public const int Disenrollment = 8;

            [Description("Return To Screening")] public const int ReturnToScreening = 10;

            [Description("Return To Site Visit")] public const int ReturnToSiteVisit = 11;

            [Description("Return To Provider Review")]
            public const int ReturnToProviderReview = 12;

            [Description("Return To Provider Credentialing")]
            public const int ReturnToProviderCredentialing = 13;

            [Description("Return To Provider Credentialing Internal")]
            public const int ReturnToProviderCredentialingInternal = 14;
        }

        // User Roles
        public static class UserRoleType
        {
            public const string Administrator = "ODMStateAdministrator";
            public const string DCSAdministrator = "DCSAdministrator";
            public const string DIDDCommissioner = "DIDDCommissioner";
            public const string DIDDOperator = "DIDDOperator";
            public const string FAOperator = "FAOperator1";
            public const string FAOperatorII = "FAOperator2";
            public const string Operator = "Operator";
            public const string ProviderAdministrator = "ProviderAdministrator";
            public const string ProviderOper = "ProviderOper";
            public const string ProviderServices = "ProviderServices";
            public const string ProviderServicesCommissioner = "ProviderServicesCommissioner";
            public const string ReadOnly = "ReadOnly";
            public const string SanctionOperator = "SanctionOperator";
            public const string StateAdministrator = "StateAdministrator";
            public const string SiteVisitOperator = "SiteVisitOperator";
            public const string ProviderAgent = "ProviderAgent";
            public const string EnrollmentSpecialist = "EnrollmentSpecialist";
            public const string InternalApplicationsEntry = "InternalApplicationsEntry";
            public const string ComplianceSpecialist = "ComplianceSpecialist";
            public const string PaymentInnovationsReports = "InternalPaymentInnovation";
            public const string DeemedPresumptive = "DeemedPresumptive";
            public const string PnmSuperUser = "PNMSuperUser";

            //AP JIRA#352
            public const string RecipientEligibility = "RecipientEligibility";
            public const string InfantMortalityLeadEntityAgent = "InfantMortalityLeadEntityAgent";
            public const string SignApproveLTCCR = "Sign Approve LTC Cost Report";
            public const string PrepareSaveLTCCR = "Prepare Save LTC Cost Report";
            public const string ViewLTCCostReport = "View LTC Cost Report";
            public const string ProviderRelationsManagement = "ProviderRelationsManagement";
            public const string ProviderRelations = "ProviderRelations";
            public const string SiteVisitAdministrator = "SiteVisitAdministrator";
            public const string ODMStateAdministrator = "ODMStateAdministrator";
            public const string APMSpecialist = "APMSpecialist";
            public const string LTCCHOP = "LTCCHOP";
            public const string LTCInitialReval = "LTCInitial/Revalidation";
            public const string Internalapplicationentry = "Internalapplicationentry";
            public const string CredentialingSpecialit = "CredentialingSpecialit";
            public const string CredentialingSupervisor = "CredentialingSupervisor";
            public const string CredentialingQualityAssurance = "CredentialingQualityAssurance";
            public const string CredentialingChair = "CredentialingChair";
            public const string ODMCredentialingQualityAssurance = "ODMCredentialingQualityAssurance";
            public const string ODMCredentialingSpecialist = "ODMCredentialingSpecialist";
            public const string ODMCredentialingSupervisor = "ODMCredentialingSupervisor";
            public const string PrepareSaveMSPCostReports = "Prepare Save MSP Cost Reports";
            public const string ViewMSPCostReports = "ViewMSPCostReports";
            public const string SignCertifyMSPCostReports = "Sign Certify MSP Cost Reports";
            public const string InternalPaymentInnovation = "InternalPaymentInnovation";
            public const string TechAdminMAX = "TechAdmin";

            public const string MITSAdmin = "MITSAdmin";
            public const string MITSBusinessRelations = "MITSBusinessRelations";
            public const string MITSProviderAssistance = "MITSProviderAssistance";
            public const string MITSProviderEnrollment = "MITSProviderEnrollment";
            public const string MITSUATSuperuser = "MITSUATSuperuser";

            public const string CPCAgent = "APM Agent";

            // OHPNM-4660
            public const string FQHCCostReportUpload = "FQHC Cost Report Upload";
            public const string ViewFQHCCostReport = "View FQHC Cost Report";
            public const string RHCCostReportUpload = "RHC Cost Report Upload";
            public const string ViewRHCCostReport = "View RHC Cost Report";
            public const string LeadInvestigationCostReportUpload = "Lead Investigation Cost Report Upload";
            public const string ViewLICostReport = "View LI Cost Report";
            public const string ViewHospitalCostReport = "View Hospital Cost Report";
            public const string MDSReport = "MDS Report";
            public const string OHFCostReportUpload = "OHF Cost Report Upload";
            public const string ViewOHFCostReport = "View OHF Cost Report";
        }

        // User Roles
        public static class PbmUserRoleType
        {
            public const string Administrator = "Administrator";
            public const string LabelerServices = "LabelerServices";
            public const string Operator = "Operator";
            public const string ProviderServices = "ProviderServices";
            public const string StateAdministrator = "StateAdministrator";
        }

        public static class ProcessParameter
        {
            public const string PartyID = "PARTY_ID";
            public const string RegistrationID = "REGISTRATION_ID";
            public const string RegProgramStatusTypeID = "REG_PROGRAM_STATUS_TYPE_ID";
            public const string ServiceLocationID = "SERVICE_LOCATION_ID";
            public const string TransactionQueueID = "TRANS_QUEUE_ID";
            public const string AdditionalApplicationID = "ADDITIONAL_APPLICATION_ID";
            public const string IsAddODMorODAMedicaidSvc = "ISADD_ODM_ODA_MEDICAID_SVC";
            public const string ApplicationComplete = "APPLICATION_COMPLETE";
            public const string IsProviderDisenrolling = "IS_PROVIDER_DISENROLLING";
            public const string UpdateCpcContact = "UPDATE_CPC_CONTACT";
            public const string IsCPCDenied = "IS_CPC_DENIED";
            public const string WaiverServiceUpdateTypeId = "WAIVER_SERVICE_UPDATE_TYPE_ID";
            public const string RtpReminderHasBeenSent = "RTP_REMINDER_HAS_BEEN_SENT";
            public const string ForceWfToReview = "FORCE_WF_TO_REVIEW";
            public const string UseGap = "USE_GAP";
            public const string IsSubmittedAgreement = "IS_AGREEEMENT_SUBMITTED";
            public const string NpiMedIDSelection = "NPI_MEDID_RB_SELECTION";
            public const string RegistrationStatusTypeID = "REGISTRATION_STATUS_TYPE_ID";
            public const string IsReapplication = "IS_REAPPLICATION";
            public const string IsReactivation = "IS_REACTIVATION";
            public const string IsSendHistoryinTxn = "SEND_HISTORY_IN_TXN";
            public const string IsTerminated = "IS_TERMINATED";
            public const string WorkflowEventTypeID = "WORKFLOW_EVENT_TYPE_ID";
            public const string IsDDClosureDateChanged = "IS_DD_CLOSURE_DATE_CHANGED";
            public const string ShowRTPBCICheckBox = "SHOW_RTP_BCI_CHECKBOX";
            public const string IsAddBCIRTPEmail = "IS_ADD_BCI_RTP_EMAIL";
            public const string IsRevertSuspension = "IS_REVERT_SUSPENSION";
            public const string IsConvertFrmORPWF = "IS_CONVERT_FROM_ORP_WF";
            public const string ReferToComplianceReasonID = "REFER_TO_COMPLIANCE_REASONS_ID";
            public const string IsStreamlinedApp = "IS_STREAMLINED_APP";
        }

        public static class WorkflowType
        {
            public const int RegistrationNew = 1;
            public const int RegistrationUpdateProvider = 2;
            //public const int RegistrationUpdateOwner = 3;
            public const int UpdateAffiliates = 4;
            public const int RegistrationDIDDReferral = 5;
            public const int RegistrationRevalidation = 6;
            public const int AdminUpdate = 9;
            public const int GroupMemberProfile = 11;
            public const int PeriodicDatabaseChecks = 12;
            public const int RegistrationReactivation = 19; //TBD
            public const int RegistrationExpressTermination = 14;
            public const int EPD = 15;
            public const int ADHP = 16;
            public const int Streamlined = 17;
            public const int CredentialingApplication = 18;
            public const int IncidentCompliance = 20;
            public const int RiskAlertClosure = 21;
            public const int RiskAlertCHOP = 22;
            public const int CHOP = 3;
            public const int Reconsideration = 24;

            public const int BumpUp = 19;
            public const int HearingRights = 25;
            public const int CPC = 26;
            public const int CMC = 27;
            public const int SiteVistEvent = 28;
        }

        public static class TaskParameter
        {
            public const string EmailFrom = "EMAIL_FROM";
            public const string EmailBodyTemplate = "EMAIL_BODY_TEMPLATE";
            public const string EmailSubject = "EMAIL_SUBJECT";
            public const string DataEntryMode = "DATA_ENTRY_MODE";
            public const string NotMaintenanceStatus = "NOT_MAINTENANCE_STATUS";
            public const string PageTypeIdList = "PAGE_TYPE_ID_LIST";
            public const string OwnershipInfoChanged = "OWNERSHIP_INFO_CHANGED";
            public const string UpdateAffiliationsOnly = "UPDATE_AFFILIATIONS_ONLY";

            public sealed class DataEntry
            {

                private readonly String name;
                private readonly int value;

                public static readonly DataEntry ALL = new DataEntry(1, "ALL");
                public static readonly DataEntry ACCOUNTINFO = new DataEntry(2, "ACCOUNT_INFO");
                public static readonly DataEntry READONLY = new DataEntry(3, "READ_ONLY");

                private DataEntry(int value, String name)
                {
                    this.name = name;
                    this.value = value;
                }

                public override String ToString()
                {
                    return name;
                }

            }

            public const string ReviewMode = "REVIEW_MODE";

            public static class ReviewModeType
            {
                public const int PROVIDER_SERVICES = 1;
                public const int ACCOUNTING = 2;
            }

            /// To regeneration list of the database run the following query
            /// SELECT 
            /// 'public const int ' + REPLACE(REG_PROGRAM_STATUS_TYPE_INTERNAL, ' ' , '')
            /// + ' = ' + CONVERT(varchar(4),REG_PROGRAM_STATUS_TYPE_ID) + ';'
            /// + ' // External Status: ' + REG_PROGRAM_STATUS_TYPE_EXTERNAL
            /// FROM REG_PROGRAM_STATUS_TYPE pst
            /// ORDER BY REG_PROGRAM_STATUS_TYPE_ID
            public const string ProgramStatus = "PROGRAM_STATUS";

            public static class ProviderStatusType
            {
                public const int New = 1; // External Status: New
                public const int Maintenance = 2; // External Status: Active
                public const int Terminated = 3; // External Status: Terminated
                public const int Denied = 4; // External Status: Denied
                public const int Suspended = 5; // External Status: Suspended
                public const int Conversion = 6; // External Status: Active
                public const int Revalidation = 7; // External Status: Active
                public const int SanctionReview = 8; // External Status: Active
            }

            public const string SeedTaskName = "SEED_TASK_NAME";
        }

        public const int RegistrationDataPageCount = 8;

        /// To regeneration list of the database run the following query
        /// SELECT 'public const int ' + REPLACE(TRANSACTION_TYPE, ' ' , '')
        ///  + ' = ' + CONVERT(varchar(4),TRANSACTION_TYPE_ID) + ';'
        /// FROM TRANSACTION_TYPE
        /// ORDER BY TRANSACTION_TYPE_ID
        public static class TransactionType
        {
            public const int RequestMedicaidIDfromMMIS = 1;
            public const int SendProviderUpdatestoMMIS = 2;
            public const int SendProviderStatusChangetoMMIS = 3;
            public const int SendGroupAffiliationstoMMIS = 4;
            public const int SendPaymentInfotoMMIS = 5;
            public const int SendProviderUpdatestoCAQH = 6;
            public const int RequestBaseMedicaidIDfromMMIS = 7;
            public const int RetrieveAllGroupsfromMMIS = 8;
            public const int RetrieveGroupAffiliationsfromMMIS = 9;
            public const int ResendAffiliationNotifications = 10;
            public const int SendDIDDAddProviderUpdatetoMMIS = 11;
            public const int SendDIDDAddAffiliationtoMMIS = 12;
            public const int SendDIDDTerminateAffiliationtoMMIS = 13;
            public const int SendDCSAddProviderUpdatetoMMIS = 14;
            public const int SendDCSAddAffiliationtoMMIS = 15;
            public const int SendDCSTerminateAffiliationtoMMIS = 16;
            public const int SendDIDDTerminateProviderUpdatetoMMIS = 17;
            public const int SendDCSTerminateProviderUpdatetoMMIS = 18;
            public const int SendWaiverServicesUpdatetoMMIS = 19;
            public const int SendSpecialty2ToMMIS = 21;
            public const int SendSpecialty3ToMMIS = 22;
            public const int SendNPIUpdateToMMIS = 23;
            public const int SendCBSAUpdateToMMIS = 24;
            public const int SendMCOAffiliationToMMIS = 25;
        }

        public static class DiddReferralType
        {
            public const int NormalContract = 1;
            public const int CreateAmendment = 2;
            public const int ContractExpansion = 3;
            public const int AssistedLiving = 4;
        }

        public static class WaiverType
        {
            public const int AD = 1;
            public const int CDD = 2;
            public const int PAS = 3;
            public const int DDAC = 4;
            public const int DDAD = 5;
            public const int DODD = 3;
            public const int ODA = 2;
        }

        public static class WaiverApplicationTypeID
        {
            public const int ODM = 1;
            public const int ODA = 2;
            public const int DODD = 3;
            public const int NonMedicaidDODD = 4;
        }

        public static class GroupAffiliationStatusType
        {
            public const string PendingConfirmation = "Pending Confirmation";
            public const string ProviderNotFound = "Provider Not Found";
            public const string Confirmed = "Confirmed";
            public const string Termed = "Termed";
            public const string PendingCAQHConfirmation = "Pending CAQH Confirmation";
            public const string RemovedbyIndividual = "Removed by Individual";
            public const string PendingGroupConfirmation = "Pending Group Confirmation";
            public const string PendingCAQHRegistration = "Pending CAQH Registration";

            public const string IndividualEnrollmentPendingApproval = "Individual Enrollment Pending Approval";
            public const string ConfirmGroupMember = "Confirm Group Member";
            public const string GroupConfirmed = "Confirmed";
            public const string Active = "Active";
            public const string PendingRemoval = "Pending Removal";
            public const string RemovedbyGroup = "Removed";
            public const string IndividualRequiresReValidation = "Individual Requires Revalidation";
            public const string PendingApproval = "Pending Approval";
            public const string MemberNotFound = "Member Not Found";
            public const string TransactionRejected = "Transaction Rejected";
        }

        public static class GroupAffiliationStatusTypeID
        {
            public const int NotSetYet = 0;
            public const int IndividualEnrollmentPendingApproval = 1;
            public const int ConfirmGroupMember = 2;
            public const int GroupConfirmed = 3;
            public const int Active = 4;
            public const int PendingRemoval = 5;
            public const int RemovedbyGroup = 6;
            public const int IndividualRequiresReValidation = 7;
            public const int PendingApproval = 8;
            public const int MemberNotFound = 9;
            public const int TransactionRejected = 10;
        }

        public static class GroupAffiliationStatusTypeDescription
        {
            public const string ActiveConversion = "Active (Conversion)";
        }

        public static class GroupAffiliationSpecialtyStatusID
        {
            public const int NotSetYet = 0;
            public const int GroupConfirmed = 1;
            public const int Active = 2;
            public const int ActiveConversion = 3;
        }

        public static class GroupMemberRetroStatusID
        {
            public const int RetroReviewNotRequired = 0;
            public const int RetroReviewRequired = 1;
            public const int RetroReviewCompleted = 2;
        }

        /* Since the MMIS values are hard-coded into the ID, these can be constants (see bug2178 and bug2178.sql) */
        public static class GroupProviderType
        {
            public const string GroupSingleSpecialty = "Single-Specialty";
            public const string GroupMultiSpecialty = "Multi-Specialty";
            public const string GroupFQHC = "Federally Qualified Health Clinic";
            public const string GroupRHC = "Rural Health Clinic";
        }

        public static class SubmissionMethodType
        {
            public const string Electronic = "Electronic";
            public const string Mail = "Mail";
        }

        public static class ReportDocumentType
        {
            public const int RemittanceAdvice = 1;
        }

        public static class DocumentType
        {
            public const string MedicaidHospitalCostReportId = "033";
            public const string MedicareCostReportECId = "034";
            public const string MedicareCostReportPIId = "035";
            public const string OtherDOCId = "036";
            public const string PotentiallyPreventableReadmissionsId = "272";
        }

        public static class EligibilityType
        {
            public const int DIDD = 1;
            public const int DCS = 2;
        }

        public static class PartyEligibilityStatus
        {
            public const int Pending = 1;
            public const int Submitted = 2;
            public const int Confirmed = 3;
            public const int PendingSubmission = 4;
        }

        public static class PartyEligibilityStatusName
        {
            public const string Pending = "Pending";
            public const string Submitted = "Submitted";
            public const string Confirmed = "Confirmed";
            public const string PendingSubmission = "Pending Submission";
        }

        public static class ViewType
        {
            public const string Edit = "Edit";
            public const string Add = "Add";
            public const string ReadOnly = "ReadOnly";
        }

        public static class RegistrationTaskName
        {
            public const string ProviderDataEntry = "Provider Data Entry";
            public const string ProviderDataEntryAccountInfo = "Provider Data Entry - Account Info";
            public const string ProviderReview = "Provider Review";
            public const string AccountReview = "Account Review";
            public const string AdminReview = "Admin Review";
            public const string FandAReview = "F&A Review";
            public const string ProviderScreening = "Provider Screening";
            public const string StateReview = "State Review";
            public const string StateReviewSiteVisit = "State Review (Site Visit)";
            public const string SiteVisitPCG = "Conduct Site Visit Step (PCG)";
            public const string SiteVisitCompliance = "Compliance Site Visit Review Step (Compliance)";
            public const string ProcessAppeal = "Process Appeal";
            public const string ProcessAppealSiteVisit = "Process Appeal(Site Visit)";
            public const string RetroReview = "Retro Review";
            public const string ApplicationFeeReview = "Application Fee Review";
            public const string GroupMemberRetroReview = "Group Member Retro Review";
            public const string FinancialReview = "Financial Review";
            public const string LTCReview = "LTC (Readiness Review)";
            public const string DDSReview = "DDS Review/Interview";
            public const string PendingApproval = "Pending Approval";
            public const string PendingDenial = "Pending Denial";
            public const string DBHProviderReview = "DBH Provider Review";
            public const string OrientationSessionScreening = "Orientation Session Screening";
            public const string PendingTerminate = "Pending Terminate";
            public const string ReviewApproval = "Review Approval";
            public const string ReviewTermination = "Review Termination";
            public const string ReviewDenial = "Review Denial";
            public const string PendingAppeal = "Pending Appeal";
            public const string ReviewApplicationFee = "Review Application Fee";
            public const string DenyHardshipRequest = "Deny Hardship Request";
            public const string ApplicationFeeDenialReview = "Application Fee Denial Review";
            public const string DBHReviewerReview = "DBH Reviewer Review";
            public const string DenialTerminationNotification = "Denial/Termination Notification";
            public const string DDSReviewerReview = "DDS Reviewer Review";
            public const string UploadInitialNoticeofDenial = "Upload Initial Notice of Denial";
            public const string UploadInitialNoticeofTermination = "Upload Initial Notice of Termination";
            public const string UploadFinalNoticeofDenial = "Upload Final Notice of Denial";
            public const string UploadFinalNoticeofTermination = "Upload Final Notice of Termination";
            public const string CredentialCommitteeReview = "Committee Review";
            public const string CredentialCommitteeChairReview = "Committee Chairman Review";
            public const string ProviderCredentialing = "Provider Credentialing";
            public const string InformationrequiredIncreasedRiskLevel = "Information required - Increased Risk Level";
            public const string ProviderCredentialingODM = "Provider Credentialing ODM";
            public const string CredentialingSupervisorReview = "Credentialing Supervisor Review";
            public const string ODMCredentialingSupervisorReview = "ODM Credentialing Supervisor Review";
            public const string CredentialingQualityReview = "Credentialing Quality Review";
            public const string ODMCredentialingReview = "ODM Credentialing Review";
            public const string PlaceRiskAlertClosure = "Place Risk alert";
            public const string TransactionMonitoring = "Transaction Monitoring";
            public const string IncidentComplianceReview = "Incident Compliance Review";
            public const string IssueAcknowledgementLetter = "Issue Acknowledge Letter";
            public const string ContractMaintenance = "Contract Maintenance";
            public const string PlaceRiskAlertchopandEffectiveDate = "Place RiskAlert chop and Effective Date";
            public const string ReceiveWaiverAcknowledgementFromSIDODD = "Recieve Acknowledgment From SI/DODD";
            public const string CredentialingCommitteeReview = "Credentialing Committee Review";
            public const string CredentialingReconsideration = "Credentialing Reconsideration";
        }

        public static class ComplianceTaskName
        {
            public const string UploadRR = "Upload Reconsideration Request";
            public const string UploadPAO = "Application Disposition";
            public const string RecordDateOfMailReturn = "Record Date Of Mail Return";
            public const string UploadAO = "Upload Adjudication Order";
            public const string RecordHearingStatus = "Record Hearing Status";
            public const string RecordDateAOMailed = "Record Date Of Adjudication Order Mailed";
            public const string TerminationReason = "Admin Enter Termination Reason";
            public const string CSUploadRR = "Upload Reconsideration Request";
            public const string CSUploadPI = "Upload Program Integrity";
            public const string CSEnterTR = "Compliance Enter Termination Reason";
            public const string ReconsiderationEnterTR = "Enter Termination Reason";
        }

        public const int Max_DCS_Affiliations = 2;

        public static class ScreeningStatusId
        {
            public const int InProgress = 1;
            public const int Complete = 2;
            public const int Failed = 12;
        }

        public static class ScreeningStatusName
        {
            public const string InProgress = "In Progress";
            public const string Complete = "Complete";
        }

        public static class ScreeningResultId
        {
            public const int Pending = 1;
            public const int Approved = 2;
            public const int Denied = 3;
        }

        public static class ScreeningActivityStatusId
        {
            public const int NotApplicable = 1;
            public const int Pending = 2;
            public const int MatchConfirm = 3;
            public const int NoMatch = 4;
            public const int Match = 5;
            public const int RequestSubmitted = 6;
            public const int Verified = 7;
            public const int NotVerified = 8;
            public const int MedicareEnrolled = 9;
            public const int AdditionalInformation = 10;
            public const int Confirmed = 11;
            public const int Failed = 12;
        }

        public static class ScreeningActivityTypeId
        {
            public const int NPPESVerification = 6;
        }

        public static class ActivityDataRankId
        {
            public const int Pass = 1;
            public const int Fail = 2;
            public const int Unclear = 3;
            public const int Conditional = 4;
            public const int Unconfirmed = 5;
            public const int pending = 6;

        }

        public static class ScreeningFor
        {
            public const string Provider = "GROUP";
            public const string Affiliation = "AFFILIATION";

            public const string
                ActiveAffiliation = "ACTIVE AFFILIATION"; /*the currently active affiliation being screened */

            public const string Owner = "OWNER";
            public const string HouseholdMember = "HOUSEHOLD MEMBER";
            public const string SiteVist = "SITE VISIT";
        }

        public static class SiteVisitRecommendationID
        {
            public const int Approved = 1;
            public const int Failed = 2;
            public const int FollowupVisit = 3;
            public const int FailedFollowup = 4;
        }

        public static class SiteVisitStatusID
        {
            public const int NotCompleted = 1;
            public const int Completed = 2;
        }


        public static class SiteVisitAttemptStatusID
        {
            public const int Inprogress = 1;
            public const int Completed = 2;
            public const int WaitingOnProvider = 3;
        }

        public static class SiteVisitAttemptTypeID
        {
            public const int Initial = 1;
            public const int Follow_Up = 2;
            public const int FailedFollow_Up = 3;
        }


        public static class SiteVisitScreeningStatusID
        {
            public const int InProgress = 1;
            public const int Passed = 2;
            public const int Failed = 3;
            public const int OutOfStatePass = 4;
            public const int OutOfStateFail = 5;
            public const int DOHPass = 6;
            public const int DOHFail = 7;
            public const int Medicare = 8;
            public const int DBHPass = 9;
            public const int DBHFail = 10;
            public const int DDSPass = 11;
            public const int DDSFail = 12;
            public const int SiteVisitRequired = 19;
            public const int SiteVisitNotRequired = 20;
            public const int DBHWillConduct = 14;
            public const int DDSWillConduct = 15;
            public const int SiteVisitConductedPreviously = 16;
        }

        public static class RegistrationWorkflowActionName
        {
            public const string Approve = "Approve";

            //removed for bug 7520 - to make deny and deny provider consistent in MMIS and NFOCUS
            //public const string DenyProvider = "Deny Provider";
            public const string ReferToState = "Refer To State";
            public const string RequestSiteVisit = "Request Site Visit";
            public const string StateReview = "State Review";
            public const string ApproveProvider = "Approve Provider";
            public const string TerminateProvider = "Terminate Provider";
            public const string Deny = "Deny";
            public const string Terminate = "Terminate";
            public const string InitiateAppeal = "Initiate Appeal";
            public const string SendToInterface = "Send To Interface";
            public const string ReturnToScreening = "Return To Screening";
            public const string ReturnToSiteVisit = "Return To Site Visit";
            public const string ReturnToProviderReview = "Return To Provider Review";
            public const string ReferToDBH = "Refer to DBH";
            public const string ReferToDDS = "Refer to DDS";
            public const string SubmitForReview = "Submit for Review";
            public const string SubmitUpdate = "Submit Update";
            public const string ApproveUpdate = "Approve Update";
            public const string ReviewComplete = "Review Complete";
            public const string OrderNewSiteVisit = "Order New Site Visit";
            public const string LTEnrollment = "Create LT Enrollment";
            public const string IssueNotice = "Issue DODD NOD Notice";
            public const string sitevisitcomplete = "Site Visit Complete";
            public const string planofcorrection = "Plan of Correction";
            public const string NotProcessed = "Not Processed";
        }
        public static class CostReportSubmissionStatus
        {
            public const int Hold = 1;
            public const int Rejected = 2;
            public const int Approved = 3;
            public const int Notified = 4;
            public const int All = 5;
            public const int Pending = 6;
            public const int Clear = 7;
            public const int Obsoleted = 8;
        }

        public enum CostReportSubmissionResponseStatus
        {
            All = 1,
            Pending = 2,
            Clear = 3,
            Obsoleted = 4
        }

        public static class FiscalYear
        {
            public const int Year = 2019;
            public const int Year1 = 2020;
            public const int Year2 = 2021;

        }

        public static class ProviderPaymentType
        {
            public const string Mail = "1";
            public const string EFT = "2";
            public const string Other = "3";
        }

        public static class WaiverPaymentType
        {
            public const string DirectDeposit = "1";
            public const string ReliaCard = "2";
        }

        public static class ApplicationFeePaymentType
        {
            public const string PayByeCheck = "Pay By eCheck";
            public const string PayByPaperCheck = "Pay By Paper Check";
            public const string RequestWaiver = "Request Waiver of Application Fee";
        }

        public static class WaiverReason
        {
            public const string MedicareEnrolled = "1";
            public const string PaidinAnotherState = "2";
            public const string PaidinThePast5Years = "3";
            public const string MedicareEnrollmentPending = "4";
        }

        public static class OwnerType
        {
            public const string Person = "15";
            public const string Organization = "16";
            public const string ManagingEmployee = "18";
            public const string GroupMemberProfile = "19";
            public const string RealEstateIndividual = "20";
            public const string RealEstateOrganization = "21";
            public const string SubcontractorIndividual = "22";
            public const string SubcontractorOrganization = "23";
            public const string SupplierIndividual = "24";
            public const string SupplierOrganization = "25";
            public const string Employee = "26";
            public const string OTHERPROVIDER = "27";
            public const int PersonId = 15;
            public const int OrganizationId = 16;
            public const int ManagingEmployeeid = 18;
            public const int RealEstateIndividualId = 20;
            public const int RealEstateOrganizationId = 21;
            public const int SubcontractorIndividualId = 22;
            public const int SubcontractorOrganizationId = 23;
            public const int SupplierIndividualId = 24;
            public const int SupplierOrganizationId = 25;
            public const int EmployeeId = 26;
            public const int OTHERPROVIDERId = 27;
        }


        public const int PartyType_GroupMemberProfile = 19;
        public const int PartyType_Organization = 16;

        public static class ApplicationFeePaymentStatus
        {
            public const string Paid = "1";
            public const string Pending = "2";
            public const string Waived = "3";
        }

        public static class ApplicationFeePaymentTypeID
        {
            public const string CreditCard = "1";
            public const string RequestWaiver = "2";
        }

        public static class ApplicationFeePaymentStatusName
        {
            public const string Pending = "Pending";
            public const string Paid = "Paid";
            public const string Waived = "Waived";
        }

        public static class WaiverReasonName
        {
            public const string MedicareEnrollmentPending = "MedicareEnrollmentPending";
            public const string PaidinThePast5Years = "PaidinThePast5Years";
            public const string MedicareEnrolled = "MedicareEnrolled";
            public const string PaidinAnotherState = "PaidinAnotherState";
        }

        public static class GenderName
        {
            public const string Male = "Male";
            public const string Female = "Female";
            public const string Unknown = "Unknown";
        }

        public static class GenderInitial
        {
            public const string Male = "M";
            public const string Female = "F";
            public const string Unknown = "N";
        }

        public static class TaxIDType
        {
            public const int EIN = 16;
            public const int SSN = 15;
        }

        public static class OwnerTypeIDs
        {
            public const int Individual = 15;
            public const int Organization = 16;
            public const int ManagingEmployee = 18;
            public const int RealEstateIndividual = 20;
            public const int RealEstateOrganization = 21;
            public const int SubcontractorIndividual = 22;
            public const int SubcontractorOrganization = 23;
            public const int SupplierIndividual = 24;
            public const int SupplierOrganization = 25;
            public const int Employee = 26;
            public const int OtherProvider = 27;
        }

        public static class SiteVisitTypeID
        {
            public const string PreEnrollment = "1";
            public const string PostEnrollment = "2";
        }

        public static class SiteVisitType
        {
            public const string PreEnrollment = "Pre-Enrollment";
            public const string PostEnrollment = "Post-Enrollment";
        }

        public static class SiteVisitAttempt
        {
            public const string Initial = "Initial";
            public const string Follow_up = "Follow - Up";
            public const string Failed_Follow_up = "Failed - Follow-Up";
        }

        public static class Environment
        {
            public const string UAT = "DC UAT";
            public const string PROD = "DC Production";
            public const string INT = "OH Integration01";
            public const string E2E = "OH E2E";
            public const string INT02 = "OH INT02";
            public const string OH_UAT = "OH UAT";
        }

        public static class StateReviewScreeningStatus
        {
            public const int Confirm = 1;
            public const int OverrideScreening = 2;
        }

        public static class ImmediateDenyOrTerminate
        {
            public const int Immediate = 1;
            public const int ProcessAppeal = 2;
        }

        public static class PaperRequestStatusType
        {
            public const int Received = 1;
            public const int InitialReview = 2;
            public const int Rejected = 3;
            public const int Processed = 4;
        }

        public static class PaperRequestType
        {
            public const int InitialEnrollment = 1;
            public const int ReEnrollment = 2;
            public const int Reactivation = 3;
            public const int Revalidation = 4;
            public const int NewFTIN = 5;
            public const int AddMember = 6;
            public const int AdditionalInformation = 7;
        }
        public static class EnrollmentStatusTypeID
        {
            public const int Active = 1;
            public const int InActive = 2;
            public const int ORPActive = 3;
        }
        public static class EnrollmentStatusCode
        {
            public const string Active = "00";
            public const string TerminatedVoluntary = "40";
            public const string TerminatedDeath = "41";
            public const string TerminatedBadAddress = "42";
            public const string TerminatedNumberChanged = "43";
            public const string TerminatedNoReenroll = "44";
            public const string TerminatedLegalAction = "45";
            public const string TerminatedChangeOwner = "46";
            public const string TerminatedDisciplinary = "47";
            public const string TerminatedDHCF = "48";
            public const string TerminatedLicenseRevoked = "49";
            public const string TerminatedNoClaimActivity = "50";
            public const string TerminatedLicenseExpired = "51";
            public const string OutOfState = "52";
            public const string DMHDenial = "53";
            public const string DenyPaymentForNewAdmission = "54";
            public const string TerminatedNoReverification = "58";
            public const string TerminatedPurged = "60";
            public const string TerminatedConversionLegacy = "70";
            public const string ReferringProviderOnly = "80";
            public const string VOLUNTARYWITHDRAWAL = "34";
            public const string TerminatedTechnical = "07";
            public const string TerminatedEmergency = "08";
            public const string ProviderDidNotRevalidate = "03";
            public const string DHCFWorkflowTermination = "99";
            public const string MCORenderingProvidersOnly = "90";
            public const string SuspendClaims = "59";
        }

        public static class EnrollStatus
        {
            public const int ACTIVE = 1;
            public const int INACTIVE = 2;
            public const int REPORTINGONLY = 3;
        }

        public static class EnrollStatusCodeDesc
        {
            public const string ACTIVE = "ACTIVE";
            public const string INACTIVE = "INACTIVE";
            public const string REPORTINGONLY = "REPORTING ONLY";
        }

        public static class EnrollStatusReason
        {
            public const string TERMINATEDFEDEXCLUSIONSYSTEM = "01";
            public const string LICENSECERTIFICATIONREVOKED = "2";
            public const string LICENSE_CERTIFICATION_NOT_RENEWED = "3";
            public const string SUSPENDED_INDICTMENT_NON_FRAUD = "06";
            public const string STATE_INITIATED_TERMINATION = "6";
            public const string SUSPENSION_FRAUD_INDICTMENT = "13";
            public const string SUSPENSION_FRAUD_INDICTMENT_13 = "14";
            public const string DECEASED = "19";
            public const string TERMINATED_FRAUD_CONVICTION = "24";
            public const string LICENSE_SUSPEND_LICENSE_BRD = "28";
            public const string TERMD_DUE_NON_FRAUDCONVICTION = "35";
            public const string SUSPENDED_CREDIBLE_FRAUD_ALLEGATION = "37";
            public const string LICENSE_ABANDONED = "39";
            public const string DENIED = "41";
            public const string LICENSE_RESTRICTED_OR_LIMITED = "42";
            public const string MEDICARE_STATUS_NOT_ACTIVE = "64";
            public const string Termination_Failure_to_Attest = "AT";
            public const string LICENSE_CERTIFICATION_NOT_ACTIVE = "IN";
            public const string RETIRED = "38";
            public const string FAILURETOREVALIDATE = "31";
            public const string INACTIVECHOP = "55";
            public const string APPLICATIONPENDING = "16";
            public const string ACTIVE = "15";
            public const string STATEINITIATEDTERMINATION = "6";
            public const string TERMINATED_BY_SISTER_STATE_AGENCY = "67";
            public const string DeniedTerminatedByCommittee = "DC";
            public const string INACTIVE = "44";
            public const string TERMD_NONCOMPLIANCE_RULES = "05";
			public const string ProvChangedProvNumbers = "21";
            public const int CLOSED = 60;
        }

        public static class MedicareEnrollmentStatus
        {
            public const string InProcess = "In Process";
            public const string Completed = "Completed";
        }

        public static class MedicareType
        {
            public const int CCN = 1;
            public const int PTAN = 16;
        }

        public static class MedicaidEnrollmentStatus
        {
            public const string InProcess = "In Process";
            public const string Completed = "Completed";
        }

        public static class MedicaidEnrollmentStatusID
        {
            public const string InProcess = "1";
            public const string Completed = "2";
        }


        public static class AdminMaintenanceActions
        {
            public const string DisEnroll = "Disenrollment";
            public const string RetroEffectiveDate = "Change Effective Date";
            public const string Terminate = "Terminate";
            public const string Reactivate = "Reactivate";
            public const string Update = "Update";
            public const string Suspend = "Suspend";
            public const string EditKeyIdentifiers = "Edit Key Identifiers";
            public const string RestrictedServices = "Restricted Services";
            public const string CredentialingEvent = "Credentialing Event";
            public const string ProviderReconsideration = "Provider Reconsideration";
            public const string InitiateCHOP = "Initiate CHOP";
            public const string InitiateClosure = "Initiate Closure";
            public const string RemoveExclusion = "Remove Exclusion";   //SAM758
            public const string InitiateHearingRights = "Initiate 119 Hearing";
            public const string EnableCPClinks = "CPC - Enable Attestation Links";
            public const string EnableCMCLinks = "CMC - Enable Late Attestations";
            public const string NotProcessed = "Not Processed";
            public const string NewTerminationDate = "New Termination Date";
            public const string CredentialReconsideration = "Credentialed Provider Reconsideration";
            public const string AdminRevertSuspension = "Revert Suspension";
            public const string SiteVisitEvent = "New Site Visit Event";
        }

        public static class ExpressAdminActionDisplayNames
        {
            public const string ProviderDisrollment = "Provider Disrollment";
            public const string SuspendProvider = "Suspend Provider";
            public const string ProviderTermination = "Provider Termination";
            public const string ReactivateProvider = "Reactivate Provider";
        }


        public static class RetroEfectiveDateOptions
        {
            public const string ApproveProviderRequest = "Approve Provider Request";
            public const string UseSubmissionDate = "Use Application Submission Date";
            public const string OverrideEffectiveDate = "Override Effective Date";
        }

        public static class AppSettingsKeyName
        {
            public const string RevalidationDueDateDelta = "RevalidationDueDateDelta";
            public const string RevalidationDueWindow = "RevalidationDueWindow";
            public const string PDMSURL = "PDMS-URL";
            public const string StateContactPhone = "StateContact_PhoneNumber";
            public const string DisableAccountAfterPwdExpiry = "DisableAccountAfterPasswordExpiry";
            public const string DisableAccountAfterPwdExpiryDays = "DisableAccountAfterPasswordExpiryDays";
            public const string CPCProgramYear = "CPCProgramYear";
        }

        public static class ReportSession
        {
            public const string ServiceProviderAgreements = "Service Provider Agreements";
            public const string MonthlyDatabaseChecks = "Monthly Database Checks";
            public const string SiteVisits = "Site Visits";
            public const string ApplicationFee = "Application Fee";
            public const string StatusOfProviderRegistrations = "Status Of Provider Registrations";
            public const string FacilitiesCHOPsandClosures = "Facilities, CHOPs, and Closures";
            public const string ProviderListingByAffiliation = "Provider Listing with Group Affiliation";
            public const string CredentialingReports = "Credentialing Reports";
            public const string CredentialingCleanFileReport = "CredentialingCleanFileReport";
            public const string CredentialingFlaggedFilesReport = "CredentialingFlaggedFilesReport";
            public const string CPCReports = "CPC Reports";
            public const string CMCReports = "CMC Reports";
            public const string NetworkAdequacyreports = "Network Adequacy Reports";
            public const string ODMStaffReports = "State Staff Reporting";
            public const string ComplianceMonitoringReports = "Compliance Monitoring";
        }

        public static class TypeOfOwnershipID
        {
            public const int Public = 1;
            public const int Private = 2;
            public const int Others = 3;
        }

        public static class SendToTypeID
        {
            public const int Provider = 1;
            public const int StateAdmin = 2;
            public const int RDWorker = 3;
            public const int ProviderAndRDWorker = 4;
            public const int AffiliatesGroupProvider = 5;
            public const int CHOPEmailGroup = 14;
            public const int CHOPEmailGroupAndEnteringProvider = 13;
            public const int ODANotificationEmailGroup = 16;
            public const int PCGNotificationEmailGroup = 15;
            public const int DODDNotificationEmailGroup = 18;
            public const int CPCWorkflowCancelledNotice = 20;
            public const int FreeFormEmails = 21;
        }

        public static class NoticeTypeID
        {
            public const int Initial = 1;
            public const int Final = 2;
            public const int OverDue = 3;
        }

        public static class WorkflowEventType
        {
            public const int NewReg = 1;
            public const int UpdateReg = 2;
            public const int RevalReg = 3;
            public const int BumpUp = 4;
            public const int Reconsideration = 5;
            public const int CPCReattest = 6;
            public const int CMCEnroll = 7;
            public const int CMCUpdate = 8;
            public const int CMCReAttest = 9;
			public const int ChangeProviderType = 10;
            public const int CredentialReconsideration = 11;
            public const int SiteVisitEvent = 12;
        }

        public static class TransferToUsers
        {
            public const int Provider = 1;
            public const int ProviderAndPaperOperator = 2;
        }

        public static class ApplicationType
        {
            public const int Standard = 1;
            public const int ORP = 2;
            public const int ChangeOfOperator = 3;
            public const int MCP = 4;
            public const int Waiver = 5;
            public const int Internal = 6;
            public const int CPC = 7;

        }

        public static class OrientationStatusType
        {
            public const int PacketSent = 1;
            public const int Scheduled = 2;
            public const int ReScheduled = 3;
            public const int Completed = 4;
            public const int ProviderSignedAgreementForDMEOnly = 5;
            public const int DHCFSignedAgreementForDMEOnly = 6;
            public const int Failed = 7;
        }

        public static class BackgroundVerficationStatusType
        {
            public const int OutofStateBackgroundCheckPass = 1;
            public const int OutofStateBackgroundCheckFail = 2;
            public const int Medicare = 3;
            public const int NoBackgroundCheckConducted = 4;
        }

        public static class BackgroundCheckTaskNames
        {
            public const string FingerprintandBackgroundEntry = "Record Whether background check needed";
            public const string FingerprintandBackgroundComplete = "Fingerprint and Background Complete";
            public const string FingerprintandBackgroundPending = "Fingerprint and Background Check Pending";
            public const string FingerprintAndBackgroundReSubmit = "Fingerprint and Background ReSubmit";
            
        }

        public static class ProviderRiskLevel
        {
            public const int RiskUndefined = 0;
            public const int RiskLimited = 1;
            public const int RiskModerate = 2;
            public const int RiskHigh = 3;
        }

        public static class BackgroundPerformedByType
        {
            public const int ODM = 1;
            public const int ODA = 2;
            public const int DODD = 3;
        }

        public static class BackgroundStatusType
        {
            public const int Yes = 1;
            public const int No = 2;
        }

        public static class BackgroundResultType
        {
            public const int Passed = 1;
            public const int Failed = 2;
            public const int Pending = 3;
            public const int FailedToAppear = 4;
            public const int PoorQualityFingerprints = 5;
        }

        public static class HearingStatus
        {
            public const int NotRequested = 1;
            public const int InProgress = 2;
            public const int SettlementReached = 3;
            public const int VoluntaryWithdrawn = 4;
        }

        public static class CallReportingOutputType
        {
            public const int Grid = 1;
            public const int Excel = 2;
            public const int PDF = 3;
        }

        public static class AddressType
        {
            public const int PrimaryPractice = 1;
            public const int Billing = 2;
            public const int Correspondence = 3;
            public const int Remittance = 4;
            public const int Other = 5;
            public const int NursingFacility = 6;
            public const int HomeOffice = 7;
            public const int PrimaryContact = 8;
            public const int Hospital = 9;
            public const int TaxForm1099 = 10;
            public const int FaxFormW9 = 11;
            public const int DurableMedicalEquipment = 12;
            public const int DMERegisteredAgent = 13;
            public const int Education = 14;
            public const int Insurance = 15;
            public const int OwnerInfo = 16;
            public const int OwnerConviction = 17;
            public const int OwnerConvictionOnBehalf = 18;
            public const int OwnerOther = 19;
            public const int ProviderInfo = 20;
            public const int Subcontractor = 21;
            public const int SubcontractorOwner = 22;
            public const int Supplier = 23;
            public const int WorkHistory = 24;
            public const int RealEstateIndividual = 20;
            //public const int RealEstateOrganization = 27;
            public const int AlternateServiceLocation = 26;
            public const int ProfessionalLicenseAddress = 39;
            public const int FranchiseFeeAddress = 33;
            public const int HcapAddress = 37;
            public const int OtherServiceLocation = 40;
            public const int CPCAddressType = 34;
            public const int CMCAddressType = 36;
            public const int AddressFromJOB = 99;
        }

        public static class SectionTypeKeyValue
        {
            //int is for section id and then in keyvalue pair string is the section name and int is the page type id

            static SectionTypeKeyValue()
            {
                PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
                var sectionData = client.SelectAllSections();
                foreach (DataRow sectionRow in sectionData.Tables[0].Rows)
                {
                    SectionType.Add(int.Parse(sectionRow["REG_SECTION_TYPE_ID"].ToString()),
                        new KeyValuePair<string, int>(sectionRow["REG_SECTION_TYPE_NAME"].ToString(),
                            int.Parse(sectionRow["REG_PAGE_TYPE_ID"].ToString())));
                    SectionInformation.Add(int.Parse(sectionRow["REG_SECTION_TYPE_ID"].ToString()), sectionRow);
                }
            }

            public static Dictionary<int, KeyValuePair<string, int>> SectionType =
                new Dictionary<int, KeyValuePair<string, int>>();

            public static Dictionary<int, DataRow> SectionInformation = new Dictionary<int, DataRow>();

            public static string GetSectionDisplayName(int sectionId)
            {
                var sectionInfo = SectionInformation[sectionId];

                return sectionInfo["SECTION_DISPLAY_NAME"].ToString();
            }

        }

        public static class SectionTypeID
        {
            //public const int AdditionalSpecialties = 3001; 
            public const int BehaviouralHealthInformation = 21;
            public const int CategoryOfService = 19;
            public const int CertificationsCLIA = 5;
            public const int CPRCertification = 11;
            public const int DentalLicense = 12;
            public const int FederalDEA = 42;
            public const int Insurance = 6;
            public const int Licenses = 7;
            public const int Miscellaneous = 26;
            public const int NumberOfBeds = 10;
            public const int NursingProfessionalCertification = 9;
            public const int OrgInfo = 1;
            public const int Disclosures = 105;
            public const int PharmacyProviders = 24;
            public const int PrimaryContactAddress = 2;
            public const int ReimbursementRates = 25;
            public const int Specialties = 3;
            public const int SpecialtiesTaxonomies = 4;
            public const int StateCDSNumber = 43;
            public const int VisionProviders = 18;
            public const int RECONSIDERATION = 63;
            public const int ChangeOperatorInfo = 65;
            public const int DaysNotice = 66;
            public const int ClosureNotice = 67;
            public const int WaiverServiceDisplay = 68;

            public const int ContractMaintenance = 69;



            //Practice/Service locations

            public const int BuildingHistory = 70;
            public const int BillingPaymentAddress = 28;
            public const int CorrespondenceAddress = 29;
            public const int OfficeHours = 32; // doesn't exist anymore
            public const int OtherAddress = 31;
            public const int PrimaryServiceAddress = 27;
            public const int RemittanceAddress = 30;

            //Group and facility affiliation
            public const int GroupAndFacilityAffiliations = 33;
            //public const int ConfirmedGroupAffiliations = 27;
            //public const int HealthCareAffiliations = 28;

            //IndividualProviders
            public const int Affiliations = 34;
            public const int OwnerInformation = 35;
            //w9
            public const int W9Form = 36;
            //public const int AdditionalQuestionsDME = 32;
            //public const int ProfitStatus = 33;
            //public const int TypeOfEntity = 34;
            //public const int PracticeType = 35;

            //DME
            public const int ApplicationFee = 17;
            public const int DME = 37;

            //waiverservices
            public const int WaiverServices = 38;

            //Agreements
            public const int MalpracticeClaimsHistory = 39;
            public const int AgreementQuestions = 40;
            public const int HearingRights = 61;
            public const int IncidentComplianceReview = 62;

            // CPC Pages OHPNM-233
            public const int CpcContactInformation = 73;
            public const int CpcSpecialties = 74;
            public const int PracticePartnership = 75;
            public const int CPCAttestation = 76;
            //public const int StreamlinedAgreement = 41;

            public const int ACHAuthorization = 45;
            public const int Agreements = 8;
            public const int Appeals = 50;
            public const int BoardCertification = 52;
            public const int EmploymentHistory = 49;
            public const int HomeOfficeAddress = 53;
            public const int HospitalAddress = 56;
            public const int MCOAffiliation = 47;
            public const int MMISTransactions = 100;
            public const int TransactionQueue = 101;     // JIRA 2961
            public const int NursingFacilityAddress = 54;
            public const int OtherDocuments = 44;
            public const int ProviderCredentialing = 51;
            public const int SatellitePracticeLocations = 46;
            public const int WorkflowSteps = 99;
            public const int WorkHistory = 48;
            public const int Address1099Form = 55;


            public const int NursingFacilityVentilator = 64;
            public const int RestrictedService = 72;
            //Added Vishwa
            public const int CredentialingContact = 71;

            public const int ERemittanceAdvice = 57;
            public const int SearcheRA = 10001;
            public const int SubmitPriorAuthorization = 10002;
            public const int SearchPriorAuthorization = 10004;
            public const int SearchEligibility = 10003;
            public const int SearchEligibilityV2 = 10016;
            //public const int ERemittanceAdvice = 10012;
            public const int SubmitClaim = 10005;
            public const int SearchClaim = 10006;
            public const int SearchClaimV2 = 10017;
            public const int HospiceEnrollment = 10007;
            public const int RetrieveReports = 10008;
            public const int ProviderFinancial = 10009;
            public const int CostReports = 10010;
            public const int SearchCostReports = 10011;
            public const int Correspondence = 10013;
            public const int ProviderReports = 10014;
            public const int UploadAttachments = 10015;
            public const int ProviderFeed = 10000;
            public const int ORPProviderSearch = 10018;

            public const int CMCContactInformation = 77;
            public const int CMCSpecialties = 78;
            public const int CMCAttestation = 79;
			public const int NPIandMedId = 80;
        }

        public static class ApplicationTypeName
        {
            public const string StandardApplication = "Standard Application";
            public const string IDDWaiver = "IDD Waiver";
            public const string EPDWaiver = "EPD-Waiver";
            public const string ADHP = "ADHP 1915(i)";
            public const string Streamlined = "Streamlined";
            public const string CrossoverQMB = "Crossover/QMB";
            public const string EmergencyOOS = "Emergency-OOS";
            public const string PCAAide = "PCA Aide";
            public const string PhysicianAssistant = "Physician Assistant";
            public const string MedicaidWaiver_ODA = "Medicaid Waiver(ODA)";
            public const string MedicaidWaiver_DODD = "Medicaid Waiver(DODD)";
        }

        public static class ErrorCodes
        {
            public const int NoRecords = 101;
            public const int ResponseNull = 102;
            public const int MBResponseNull = 103;
            public const int TransactionFailed = 104;
        }

        public static class MCOAffiliationstatusTypeID
        {
            public const int NotSetYet = 0;
            public const int ProviderEnrollmentPendingApproval = 1;
            public const int ProviderRequiresReEnrollment = 2;
            public const int Confirmed = 3;
            public const int Active = 4;
            public const int PendingRemoval = 5;
            public const int Removed = 6;

        }

        public static class AppealNoticeStatusID
        {
            public const int InitialNoticeSent = 1;
            public const int FinalNoticeSent = 2;
        }

        public static class CommitteeCredentialActivityStatusId
        {
            public const string Pending = "1";
            public const string Pass = "2";
            public const string Fail = "3";
            public const string ApproveWithRestrictions = "4";
        }

        public static class CommitteeCredentialActivityStatus
        {
            public const string Pending = "Pending";
            public const string Pass = "Pass";
            public const string Fail = "Fail";
            public const string ApproveWithRestrictions = "Approve with Restrictions";
        }

        public static class SystemUpdateUsers
        {
            public const string appDataExchangeUser = "DataExchange";
            public const string appDBScriptsUser = "DBScripts";
        }

        public static class PasswordExpiryNotices
        {
            public const string subject = "PDMS Password Expiring in 14 Days";
            public const string emailTemplateName = "PasswordExpiryNotification.txt";
        }

        public static class PNMDate
        {
            public static readonly DateTime MaxDate = new DateTime(2299, 12, 31);
            public static readonly DateTime MinDate = DateTime.MinValue;
            public const string MaxDateString = "12/31/2299";
            public const string DateFormat = "yyyyMMdd";
            public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }

        public static class AgreementsProviderType
        {
            public const string PsychiatricTreamentFacility = "03";
            public const string NursingFacility = "86";
            public const string IntermediateCareFacility = "89";
        }
        public static class ProviderTypeNumerics
        {
            public const int NURSING_FACILITY = 86;
            public const int STATE_OPERATED_ICF_MR = 88;
            public const int NONSTATE_OPERATED_ICF_MR = 89;

            public const int NON_AGENCYPERSONALCAREAIDE = 25;
            public const int NON_AGENCYHOMECAREATTENDANT = 26;
            public const int WAIVEREDSERVICESINDIVIDUAL = 55;
        }

        public static class LTCProviderTypes
        {
            public const string NursingFacility = "86";
            public const string STATE_OPERATED_ICF_MR = "88";
            public const string NONSTATE_OPERATED_ICF_MR = "89";
        }
        public static class RSProviderTypes
        {
            public const string NursingFacility = "86";
            public const string NonStateOperated_ICF_MR = "89";
            public const string StateOperated_ICF_MR = "88";
            public const string MedicaidSchoolProgram = "28";
            public const string Hospital = "01";
            public const string PsychiatricHospital = "02";
            public const string PsychiatricResidentialTreatmentFacility = "03";
        }
        public static class RSStatusCode
        {
            public const int Active = 1;
            public const int Inactve = 2;

        }
        public static class RSIncludeExclude
        {
            public const string Include = "I";
            public const string Exclude = "E";

        }
        public static class CredentilaingStatus
        {
            public const int ApplicationReceived = 1;
            public const int InProcess = 2;
            public const int PendingProcess = 3;
            public const int Incomplete = 4;
            public const int QualityReview = 5;
            public const int ChairReview = 6;
            public const int Approved = 7;
            public const int AdministrativeDenial = 8;
            public const int AdministrativeTermination = 9;
            public const int CredentialingCommitteeDenial = 10;
            public const int CredentialingCommitteeTermination = 11;
            public const int ODMCredentialingReview = 12;
            public const int CredentialingSupervisorReview = 13;
            public const int ProcessDiscontinued = 14;
            public const int CommitteeReview = 15;
        }

        public static class CredentialingResult
        {
            public const int Pending = 1;
            public const int Pass = 2;
            public const int Denied = 3;
            public const int Tabled = 4;
            public const int Pass1Year = 5;
            public const int Terminated = 6;
            public const int Failed = 7;
        }

        public static class RiskLevelStatus
        {
            public const int NotDefined = 0;
            public const int Increased = 1;
            public const int Decreased = 2;
        }

        public const int CHOPEntryTaskID = 239;
        public const int WaiverEntryTaskID = 530;
        public const int ProviderReviewEntryTaskID = 5;
        public const int RiskLevelEntryTaskID = 650;
        public const int ReconsiderationLevelEntryTaskID = 291;// bringing reconsideration into standard application workflow
        public const int UploadProgramIntegrityTaskID = 292;
        public const int ReconsiderationUploadControlID = 579;
        public const int EnterTerminationReason = 542;
        public const int ReconsiderationBackToStandardWF = 3;
        public const int ComplianceBackToStandardWF = 3;
        public const int ComplianceEnterTerminationReason = 357;
        public const int CPCEntryTaskID = 788;
        public const int DataReceivedandSavedFromSI = 536;
        public const int CMCEntryTaskID = 850;
        public const int CredentialReconsiderationEntryTaskID = 450;
        public const int SiteVistEventEntryTaskID = 150;

        public static class ComplianceTaskID
        {
            public const int SiteVisitReview1Task = 201;
            public const int SiteVisitReview2Task = 211;
            public const int SiteVisitReview3Task = 663;
            public const int SiteVisitReview4Task = 664;
        }
        public static class ProviderManagementServiceResponse
        {
            public const string enrollProvider = "EnrollProviderResponse";
            public const string inquireProvider = "InquireProviderResponse";
            public const string inquireProviderHistory = "InquireProviderHistoryResponse";
            public const string updateProvider = "UpdateProviderResponse";
        }

        public static class PriorAuthServiceResponse
        {
            public const string AddUpdatePriorAuth = "AddUpdatePriorAuth";
            public const string InquirePriorAuth = "InquirePriorAuth";
            public const string SearchPriorAuth = "SearchPriorAuth";
        }

        public static class AcknowledgementServiceResponse
        {
            public const string targetvendorResponse = "TargetVendorResponse";
        }

        public static class HospiceServiceResponse
        {
            public const string searchResponses = "SearchResponses";
            public const string inquireResponse = "InquireResponse";
            public const string addUpdateResponse = "AddUpdateResponse";
        }

        public static class PartialProviderManagementServiceResponse
        {
            public const string submitPartialProvider = "SubmitPartialProviderResponse";
        }

        public static class PartialProviderSubscriberSystems
        {
            public const string PSM = "PSM";
            public const string PCW = "PCW";
        }

        public static class ProviderManagementSubscriberSystems
        {
            public const string MITS = "MITS";
            public const string ALL = "ALL";
            public const string FI = "FI";
            public const string SPBM = "SPBM";
            public const string EVV = "EVV";
            public const string SI = "SI";
        }

        public static class ProviderManagementRequestorSystems
        {
            public const string PNM = "PNM";
        }

        public static class TransactionResult
        {
            public const string TransactionPassed = "Transaction Passed";
            public const string TransactionFailed = "Transaction Failed";
        }

        public static class Services
        {
            public const string ProviderManagement = "ProviderManagement";
            public const string PartialProviderManagement = "PartialProviderManagement";
        }

        public static class AttachmentServiceSoapAction
        {
            public const string AttachmentService = "\"http://mes.gov/sendAttachment\"";
        }

        public static class ProviderManagementSoapAction
        {
            public const string ProviderManagement = "\"http://mes.gov/enrollProvider\"";
            public const string UpdateProviderManagement = "\"http://mes.gov/updateProvider\"";
            public const string PartialProviderManagement = "\"http://mes.gov/submitPartialProviderReqRes\"";
            public const string PartialProviderManagementPubSub = "\"http://mes.gov/submitPartialProvider\"";
        }
        public static class IMSSoapAction
        {
            public const string IncidentManagementService = "\"http://mes.gov/updateProviderIncidentStatus\"";
        }

        public static class FinancialServiceSoapAction
        {
            public const string Inquire1099 = "\"http://mes.gov/inquire1099\"";
            public const string InquireTransactionHistory = "\"http://mes.gov/inquireTransactionHistory\"";
        }
        public static class EliglibityServiceSoapAction
        {
            public const string Membermanagement = "\"http://mes.gov/membermanagement\"";
            public const string EligVerifResponse = "\"http://service.membermgmt.fi/MemberEligibilityInquiryService/InquireMemberEligibility\"";
        }
        public static class PriorAuthSearchSoapAction
        {
            public const string CareManagement = "\"http://mes.gov/searchAuthorization\"";
            public const string CareManagementResponse = "";

            public const string AddUpdatePriorAuth = "\"http://service.caremgmt.fi/PriorAuthService/AddUpdatePriorAuth\"";
            public const string InquirePriorAuth = "\"http://service.caremgmt.fi/PriorAuthService/InquirePriorAuth\"";
            public const string SearchPriorAuth = "\"http://service.caremgmt.fi/PriorAuthService/SearchPriorAuth\"";

        }
        public static class claimFrequencyCodes
        {
            public const string standardClaim = "1";
            public const string adjustClaim = "7";
            public const string voidClaim = "8";
        }

        public static class WebServiceURI
        {
            public const string ProviderManagement = "https://dp.test.oh.healthinteractive.net:443/providerManagement/serviceentrypoint/v1.9";
            public const string PartialProviderManagement = "https://dp.test.oh.healthinteractive.net:443/partialProviderManagement/ReqRes/serviceentrypoint/v1.9";
            public const string PartialProviderPubSubManagement = "https://dp.test.oh.healthinteractive.net:443/partialProviderManagement/PubSub/serviceentrypoint/v1.9";
            public const string AcknowledgementService = "https://dp.test.oh.healthinteractive.net:443/acknowledgment/serviceentrypoint/v1.9";
            public const string HospiceService = "https://dp.test.oh.healthinteractive.net:443/hospiceService/serviceentrypoint/v1.9";
            public const string FinancialService = "https://dp.test.oh.healthinteractive.net:443/financialService/serviceentrypoint/v1.9";
            public const string DocumentService = "https://dp.test.oh.healthinteractive.net:443/attachment/serviceentrypoint/v1.9";
            public const string RecipientEligibilityService = "http://10.118.47.72:9095/MemberEligibilityInquiryServiceSOAPVS";
            //TODO need to delete the ClaimsService when Dental/Insti/prof services are done.
            public const string ClaimsService = "https://dp.test.oh.healthinteractive.net:443/claimsSearch/serviceentrypoint/v1.11";
            public const string CostReportService = "https://dp.test.oh.healthinteractive.net:443/costReports/serviceentrypoint/v1.9";
            public const string IncidentManagementService = "https://dp.test.oh.healthinteractive.net:443/incidentManagement/serviceentrypoint/v1.9";
            public const string CareManagementService = "https://dp.test.oh.healthinteractive.net:443/careManagement/serviceentrypoint/v1.9";
            public const string ClaimsDentalService = "http://10.118.47.72:9103/ClaimsDentalServiceSOAPVS";
            public const string ClaimsProfessionalService = "http://10.118.47.72:9105/ClaimsProfessionalServiceSOAPVS";
            public const string ClaimsInstitutionalService = "http://10.118.47.72:9097/ClaimsInstitutionalServiceSOAPVS";
        }

        public static class ResponseCodes
        {
            public const string SI_Acknowledgment_SUCCESS = "1001";
            public const string MITS_Acknowledgment_SUCCESS = "1001";
            public const string WAIVER_SI_SUCCESS = "1001";
            public const string WAIVER_SI_SUCCESS_ACK = "1000";
            public const string WAIVER_SI_WARNING = "7503";

            public const string PNM_Document_Failed = "PNM-2000";

            public const string PNM_Default_Error = "PNM-9000";
            public const string PNM_Default_Error_Exception = "PNM-9006";
            public const string PNM_Staging_Error_Exception = "PNM-9007";
            public const string MITS_SI_SUCCESS_ACK = "1000";
            public const string SI_Warning = "5501";
        }

        public static class ResponseTypes
        {
            public const string PNM_Success = "Success";
            public const string PNM_Failure = "Failure";
            public const string PNM_Document_Failed = "Failure";
            public const string PNM_Default_Error = "Failure";
            public const string SI_failue = "Failure";
            public const string SI_success = "Success";
            public const string SPA_success = "SUCCESS";
        }

        public static class ResponseMessage
        {
            public const string PNM_Document_Failed = "PNM Staging Failed";

            public const string PNM_Default_Error_Exception = "Trasaction failed to generate. Please try resending the transaction";
            public const string PNM_Staging_Error_Exception = "Staging Failed. Please try resending the transaction";
            public const string PNM_Default_Error = "Transaction failed to process. Please try resending the transaction";
        }

        public static class ResponseDetails
        {
            public const string PNM_Default_Error = "PNM Internal Error";
            public const string PNM_IVR_Default_Error = "The request received has errors and could not be processed. Please correct the request and resend for processing";
        }

        public static class TransactionTypeNew
        {
            public const int RequestMedicaidIDfromMMIS = 1;
            public const int SendProviderUpdatestoMMIS = 2;
            public const int SendProviderStatusChangetoMMIS = 3;
            public const int SendGroupAffiliationstoMMIS = 4;
            public const int SendPaymentInfotoMMIS = 5;
            public const int SendProviderUpdatestoCAQH = 6;
        }

        public static class TransactionTypeValues
        {
            public const string MITS = "MITS";
            public const string PSM_Partial = "PSM Partial";
            public const string PCW_Partial = "PCW Partial";
            public const string PSM_FULL = "PSM Full";
            public const string PCW_Full = "PCW Full";
        }

        public static class TransactionTypeIds
        {
            public const int MITS_UPDATE = 1;
            public const int MITS_ENROLL = 2;
            public const int PSM_Partial = 3;
            public const int PCW_Partial = 4;
            public const int PSM_FULL = 5;
            public const int PCW_Full = 6;
        }
        //OHPNM-10741
        public static class BillingServicetypeId
        {
            public const int Claims = 1;
            public const int PA = 2;

        }

        public static class TransactionResultIds
        {
            public const int ACK_RECEIVED_PASSED = 1;
            public const int ACK_RECEIVED_FAILED = 2;
            public const int ACK_RECEIVED_WAIT = 3;
        }

        public static class ApplicationStatus
        {
            public const string Pending_External_Medicaid_Approval = "Pending External Medicaid Approval";
            public const string Pending_External_Medicaid_Approval_ID = "19";
            public const string Approved = "Approved";

            public const string Recommended_Certification = "Recommended Certification";
            public const string Recommended_Certification_ID = "51";
            public const string Pending_Review_ID = "15";
            public const string ClosedAs_Denied_ID = "21";
            public const string PSM_Service_Update_Complete = "PSM Service Update Complete";
            public const string PSM_Service_Update_Complete_ID = "53";

            public const string Closed_By_ODM = "20";
            public const string Closed_By_ODM_Status = "Closed By ODM";
            public const string Closed_As_Denied = "21";
            public const string Certified = "22";
            public const string Certified_Name = "Certified";
            public const string Closed_As_Legal_Override = "23";
            public const string Approved_ID = "42";

            public const string Closed_Partial_Approval = "24";
            public const string Closed_as_ODM_Denial = "Closed as ODM Denial";
            public const string Closed_as_ODM_Denial_ID = "61";
            public const string Recommended_Denial_ID = "52";
            public const string Recommended_Denial = "Recommended Denial";

            public const string Supplemental_Application_Required_ID = "17";
            public const string Supplemental_Application_Required = "Supplemental Application Required";
            public const string ClosedForReapplication = "63";
            public const string ClosedasDisapprove = "56";
            public const string WithdrawnbyProvider = "60";
            public const string Expired = "57";

            public const string PendingReview = "Pending Review";
            public const string ClosedAsExpired = "Expired";
            public const string ClosedAsWithdrawnbyProvider = "Withdrawn by Provider";
            public const string ClosedAsDenied = "Closed As Denied";
            public const string ClosedAsAdjudicationOrderIssued = "29";

            public const string ODA_Closed = "65";
            public const string Closed_As_Complete = "Closed As Complete";
            public const string Closed_As_Complete_ID = "68";
            public const string Waiting_on_Agreement = "69";
            public const string Pending_Effective_Date = "70";
            public const string Capital_Funds_Review = "33";
            public const string In_Review = "41";
            public const string Sanction_Review = "43";

            public const string Specialties_Update = "UPDATE REGISTRATION";
            public const string NPI_Reapplication = "Reapplication";
            public const string NPI_Reactivation = "Reactivation";
            public const string NPI_Revalidation = "Revalidation REGISTRATION";
            public const string NPI_Reconsideration = "RECONSIDERATION";
            public const string NPI_InitialEnrollment = "NEW REGISTRATION";
        }

        public static class PSMApplicationStatusID
        {
            public const int ACCEPTED = 1;
            public const int DENIED = 3;
            public const int NOT_PROCESSED = 11;
        }
        public static class WaiverServiceUpdateType
        {
            public const int DODD = 1;
            public const int ODA = 2;
            public const int ODM = 3;
            public const int OperatorUpdate = 4;
        }
        public static class MMISProviderType
        {
            public const string Acupuncturist = "23";
            public const string AMBULANCE = "82";
            public const string AMBULATORY_SURGERY_CENTER = "46";
            public const string Anesthesia_Assistant_Individual = "68";
            public const string AUDIOLOGIST_INDIVIDUAL = "43";
            public const string BEHAVIOR_ANALYST = "53";
            public const string Behavioral_Health_Para_Professionals = "96";
            public const string Building_and_Wing_ID_for_LTC_Facilites_Only = "LT";
            public const string CERTIFIED_REGISTERED_NURSE_ANESTHETIST_INDIVIDUAL = "73";
            public const string CHEMICAL_DEPENDENCY = "54";
            public const string Chiropractor_Individual = "27";
            public const string CLINIC = "50";
            public const string CLINICAL_COUNSELING = "47";
            public const string Clinical_Nurse_Specialist_Individual = "65";
            public const string Converted_Inactive_ProviderType = "CV";
            public const string CPC_Entity = "99";
            public const string Dentist_Individual = "30";
            public const string DODD_TARGETED_CASE_MANAGEMENT = "85";
            public const string DURABLE_MEDICAL_EQUIPMENT_SUPPLIER = "76";
            public const string END_STAGE_RENAL_DISEASE_CLINIC = "59";
            public const string Enhanced_Care_Management = "78";
            public const string EYEGLASS_VOLUME_PURCHASE_CONTRACT_VENDER = "15";
            public const string FEDERALLY_QUALIFIED_HEALTH_CENTER = "12";
            public const string Franchise_Fee_Only_Non_Medicaid_Provider = "FF";
            public const string FREE_STANDING_BIRTH_CENTER = "11";
            public const string Health_Maintenance_Organization = "77";
            public const string HELP_ME_GROW = "06";
            public const string HOME_AND_COMMUNITY_BASED_ODA_ASSISTED_LIVING = "74";
            public const string Hospice = "44";
            public const string Hospital = "01";
            public const string Independent_Diagnostic_Testing_Facility = "79";
            public const string INDEPENDENT_LABORATORY = "80";
            public const string Licensee = "LI";
            public const string MANAGED_CARE_ORGANIZATION_PANEL_PROVIDER_ONLY = "19";
            public const string MARRIAGE_AND_FAMILY_THERAPY = "52";
            public const string MEDICAID_SCHOOL_PROGRAM = "28";
            public const string Medicare_Certified_Home_Health_Agency = "60";
            public const string MENTAL_HEALTH_CLINIC = "51";
            public const string Non_Agency_Home_Care_Attendant = "26";
            public const string NON_AGENCY_NURSE_RN_OR_LPN = "38";
            public const string Non_Agency_Personal_Care_Aide = "25";
            public const string NON_STATE_OPERATED_ICF_MR = "89";
            public const string Nurse_Midwife_Individual = "71";
            public const string Nurse_Practitioner_Individual = "72";
            public const string NURSING_FACILITY = "86";
            public const string Occupational_Therapist_Individual = "41";
            public const string OHIO_DEPARTMENT_OF_MENTAL_HEALTH_PROVIDER = "84";
            public const string OMHAS_CERTIFIED_LICENSED_TREATMENT_PROGRAM = "95";
            public const string Operator = "OP";
            public const string OPTICIAN_OCULARIST = "75";
            public const string Optometrist_Individual = "35";
            public const string OTHER_ACCREDITED_HOME_HEALTH_AGENCY = "16";
            public const string OUTPATIENT_HEALTH_FACILITY = "04";
            public const string PACE = "08";
            public const string PHARMACY = "70";
            public const string Physical_Therapist_Individual = "39";
            public const string Physician_Osteopath_Individual = "20";
            public const string PHYSICIAN_ASSISTANT = "24";
            public const string Podiatrist_Individual = "36";
            public const string PORTABLE_XRAY_SUPPLIER = "81";
            public const string Professional_Dental_Group = "31";
            public const string Professional_Medical_Group = "21";
            public const string Psychiatric_Hospital = "02";
            public const string PSYCHIATRIC_RESIDENTIAL_TREATMENT_FACILITY = "03";
            public const string PSYCHOLOGY = "42";
            public const string REGISTERED_DIETITIAN_NUTRITIONIST = "07";
            public const string Rural_Health_Clinic = "05";
            public const string SOCIAL_WORK = "37";
            public const string SPEECH_LANGUAGE_PATHOLOGIST_INDIVIDUAL = "40";
            public const string State_of_Ohio_Department_Agency = "93";
            public const string STATE_OPERATED_ICF_MR = "88";
            public const string Supported_Living = "SL";
            public const string Unpaid_Support_Broker = "SB";
            public const string Veteran_Home = "VH";
            public const string Waivered_Services_Individual = "55";
            public const string WAIVERED_SERVICES_ORGANIZATION = "45";
            public const string WHEELCHAIR_VAN = "83";
        }

        public static class StageDocumentsTitle
        {
            public const string FourtyFiveDaysNotice = "45 DaysNotice";
            public const string NOD = "Notice Of Deficiency";
        }

        public static class UploadControlDocumentTitles
        {
            public const string RequestReconsideration = "Request Reconsideration";
            public const string ClosureNotice = "ClosureNotice";
        }

        public static class MethodOfResoultion
        {
            public const string Dismissed = "Dismissed";
            public const string Settled = "Settled";
            public const string Mediation = "Mediation";
            public const string Arbitration = "Arbitration";
            public const string JudgementfortheDefendant = "Judgement for the Defendant";
            public const string JudgementforthePlaintiff = "Judgement for the Plaintiff";

        }
        public static class PriorAuthHospital
        {
            public const int HospitalId = 1;
        }

        public static class CPCType
        {
            public const string Individual = "I";
            public const string Convener = "P";
        }

        public static class CPC_CommunicationLetterTypeID
        {
            public static int CPC_INVITATION_LETTER = 1;
            public static int CPC_FIRST_REMAINDER_LETTER = 2;
            public static int CPC_FINAL_REMAINDER_LETTER = 3;
            public static int CPC_NON_RENEWAL_NOTIFICATION_LETTER = 4;
        }

        public static class CMC_CommunicationLetterTypeID
        {
            public static int CMC_INVITATION_LETTER = 1;
            public static int CMC_FIRST_REMAINDER_LETTER = 2;
            public static int CMC_NON_RENEWAL_NOTIFICATION_LETTER = 4;
        }

        public static class CPCLetterTemplateType
        {
            public static string CPCInvitation = "CPC_INVITATION_LETTER";
            public static string CPCRemainder = "CPC_REMINDER_LETTER";
            public static string CPCFinalRemainder = "CPC_FINAL_REMINDER_LETTER";
            public static string CPCNonRenewal = "CPC_NON_RENEWAL_NOTIFICATION_LETTER";
        }

        public static class CMCLetterTemplateType
        {
            public static string CMCInvitation = "EMAIL_TEMPLATE_CMC_INVITATION";
            public static string CMCRemainder = "EMAIL_TEMPLATE_CMC_REMINDER";
            public static string CMCNonRenewal = "EMAIL_TEMPLATE_CMC_NON_RENEWAL";
        }

        public static class COMMUNICATION_EVENT_TYPE
        {
            public const string print = "PRINT";
        }
        public static class Prov_ID_TYPE
        {
            public static int NPI2 = 6;
            public static int NPI = 7;
            public static int MedicaidProviderID = 3;
        }

        public const string ManualUpdate = "PubSub";

        public static class AMAStatusID
        {
            public static int Successful = 1;//"200"
            public static int NoContent = 2;//"204"
            public static int BadRequest = 3;//"400"
            public static int Unauthorized = 4;//"401"
            public static int InternalServerError = 5;//"500"
            public static int ProfileNotFoundByEntityID = 6;//"202"
            public static int ProfileOrderwasnotPlacedByEntityID = 7;//"409"
        }
        public static class AMAStatusCode
        {
            public const string SuccessfulCode = "200";
            public const string Ok = "OK";
            public const string NoContentCode = "204";
            public const string NoContent = "NoContent";
            public const string BadRequestCode = "400";
            public const string BadRequest = "BadRequest";
            public const string UnauthorizedCode = "401";
            public const string Unauthorized = "Unauthorized";
            public const string InternalServerErrorCode = "500";
            public const string InternalServerError = "InternalServerError";
            public const string ProfileNotFoundByEntityIDCode = "202";
            public const string ProfileNotFoundByEntityID = "ProfileNotFoundByEntityID";
            public const string Accepted = "Accepted";
            public const string ProfileOrderwasnotPlacedByEntityIDCode = "409";
            public const string ProfileOrderwasnotPlacedByEntityID = "ProfileOrderwasnotPlacedByEntityID";
            public const string Conflict = "Conflict";
        }

        public static class DelegateAffiliationNotice
        {
            public const string subject = "Delegate Affiliation File Processed";
            public const string emailTemplateName = "DelegateAffiliateEmailNotice.txt";
        }

        public static class DelegateDocumentUploadStatus
        {
            public const int Processing = 1;
            public const int Complete = 2;
            public const int Rejected = 3;
        }

        public static class AgentBulkUploadStatus
        {
            public const int Processing = 1;
            public const int Complete = 2;
        }

        public enum DelegateDocumentUploadErrorCodes
        {
            [Description("File is rejected at Bulk Load To Stg")]
            ERR_FILE_REJ_BULK,
            [Description("File rejected while retrieving")]
            ERR_FILE_REJ_RETR,
            [Description("Error while Saving the file")]
            ERR_FILE_SAVE,
            [Description("Column: {0} contains data with a length greater than: {1}")]
            ERR_FILE_DATA_LEN
        }
        public static class WaiverTransactionStatusType
        {
            public const int NotProcessed = 1;
            public const int Processed = 2;
            public const int Failed = 3;
            public const int InProcess = 4;
        }

        public static class RetrieveReportTypeID
        {
            public const int MDSFINALANNUALREPORTS = 1;
            public const int MDSFINALQUARTERLYREPORTS = 2;
            public const int MDSFINALSEMIANNUALREPORTS = 3;
            public const int MDSPRELIMINARYQUARTERLYREPORTS = 4;
            public const int MDSWEEKLYREPORTS = 5;
            public const int LTCRATEPACKAGES = 6;
            public const int LTCCorrespondence = 7;
            public const int POTENTIALLYPREVENTABLEREADMISSIONS = 8;
        }

        public static class DocumentXREFType
        {
            public const int PID = 19;
        }
        public static class AcknowledgementResponse
        {
            public const string EFTFailure = "R02";
        }

        public static class ProviderFileType
        {
            public const int Prescribing = 1;
            public const int Dispensing = 2;
        }

        public static class CMCProgramLinks
        {
            public const string ODMWebsite = "https://medicaid.ohio.gov/wps/portal/gov/medicaid/resources-for-providers/special-programs-and-initiatives/payment-innovation/comprehensive-primary-care/comprehensive-primary-care";
        }

        public enum InqMessageHeaderBusinessFlow
        {
            /// <remarks/>
            InquireMemberEligibility,

            /// <remarks/>
            InquireMemberRegistry,
        }

        public enum ClaimsRequestType
        {
            InquireDentalClaim,
            AddUpdateDentalClaim,
            InquireProfessionalClaim,
            AddUpdateProfessionalClaim,
            InquireInstitutionalClaim,
            AddUpdateInstitutionalClaim
        }


        public enum ClaimType
        {
            DENTAL = 1,
            PROFESSIONAL = 2,
            INSTITUTIONAL = 3
        }

        public static class EnrollStatusReasonID
        {
            public const int Active = 15;
            public const int InActive = 47;
            public const int NonParticipatingRPTOnly = 11;
            public const int ORPNonBilling = 74;
            public const int VoluntaryWithdrawal = 36;
            public const int TerminatedFedExclusionSystem = 1;
            public const int DeniedOrTerminatedByCommittee = 79;
            public const int TerminatedFederalExclusion = 9;
        }

        public static class RetrieveReports
        {
            public const string POTENTIALLYPREVENTABLEREADMISSIONS = "POTENTIALLY PREVENTABLE READMISSIONS";
            public const string PRIORAUTHSUBMISSIONRESPONSE = "278 Prior Auth Submission Response";
            public const string CLAIMSUBMISSIONRESPONSE = "277 Claim Submission Response";
            public const string PASRRReports = "PASRR Reports";
        }

        public static class PriorAuthAckService
        {
            public const int AddUpdatePriorAuth = 1;
            public const int InquirePriorAuth = 2;
            public const int ReceivePriorAuthUpdates = 3;
            public const int SearchPriorAuth = 4;
        }

        public static class WebApiServiceGuid
        {
            public const string ClaimsService = "E401DB55-3282-4459-A78B-42367957656D";
            public const string PriorAuthService = "5BA7127A-956F-49BE-97AA-E7CF27C5D32C";
            public const string IVRService = "7AB845D1-B600-4B93-BD13-B1306454F9DA";
            public const string DocumentService = "FE6A4997-822F-4FB4-A2EB-D24BE7AE7FD9";
            public const string IncidentServiceSend = "2AC34DD3-6A6D-4147-B856-281E62D3DA9B";
            public const string VendorAckService = "C2DAC3DA-D475-448C-A26A-ED51517DEB55";
        }

        public static class WebPageProcessName
        {
            public const string SubmitPriorAuthorization = "SubmitPriorAuthorization";
            public const string SearchPriorAuthorization = "SearchPriorAuthorization";
            public const string PriorAuthServiceController = "PriorAuthServiceController";
            public const string PriorAuthAttachments = "PriorAuthAttachments";
        }

        public static class WebPageLogGuid
        {
            public const string PriorAuthWebPageLog = "AFE321D0-38E3-4C53-BEE0-50D046ADD0FB";
        }

        public enum BillingServiceTransactionType
        {
            Dental = 1,
            Professional = 2,
            Institutional = 3,
            PriorAuth = 4
        }

        public static class PriorAuthBusinessFlow
        {
            public const int AddUpdatePriorAuth = 0;
            public const int InquirePriorAuth = 1;
            public const int SearchPriorAuth = 2;
        }

        public static class PriorAuthSubscriber
        {
            public const int FI = 0;
            public const int EDI = 1;

        }

        public static class ClaimsTypeOfBill
        {
            public const string TypeOfBill0111 = "0111";

        }

        public static class PrintCenterHeader
        {
            public const string Double = "double";
            public const string Single = "single";
        }
        public static class ActionButtonType
        {
            public const int Save = 0;
            public const int Submit = 1;

        }
        public static class AccidentRelatedTo
        {
            public const string Employment = "2";


        }
        public static class ClaimFilingIndicator
        {
            public const string Medicare_Part_A = "16";
            public const string Medicare_Part_B = "17";


        }
        public static class OtherPayerSequence
        {
            public const string UNKNOWN = "12";
        }

        public static class ResponseFileTemplates
        {
            public const string CA277 = "Claims277CATemplate.txt";
            public const string PA278 = "PriorAuth278Template.txt";
        }

        public static class MQAppID
        {
            public const string MQAppIDKey = "87AA9B06-8907-4A65-AAB7-CD04EFE84AC5";
        }

        public static class PAClaimsType
        {
            public const int Dental = 0;
            public const int Institutional = 1;
            public const int Professional = 2;
        }
        public static class PresentOnAdmissionIndicator
        {
            public const string No = "No";
            public const string Yes = "Yes";
            public const string Unknown = "Unknown";
            public const string NotApplicable = "Not Applicable";

        }

        public static class AdditionalApplicationType
        {
            public const string Initial_ID = "24";
        }

        public static class CMCCPCTerminationType
        {
            public const string subject = "CPC CMC Termination information";
            public const string emailTemplateName = "CMCCPCTermination.txt";
        }
        public static class ClaimStatus
        {
            public const string Delete = "1";
            public const string Update = "2";
        }
        public static class EFTNotificationType
        {
            public const string subject = "EFT Update Notification";
            public const string emailTemplateName = "EFTInformationUpdate.txt";
        }

        public static string DefaultDateString = "00010101";

        public static string PANumber_StartsWith = "AUTH";
        public static string PA_Instititional_Proc_Code_Psychiatric_Inpatient = "psychiatric inpatient";

        public static class AdjustmentGroup
        {
            public const string CO = "CO";
            public const string CR = "CR";
            public const string OA = "OA";
            public const string PI = "PI";
            public const string PR = "PR";
        }

        public static class ProviderTypeChangeRequestStatus
        {
            public const int InProcess = 0;
            public const int Complete = 1;
            public const int Cancelled = 2;
        }
		
        public static class EnrollmentSpanOptions
        {
            public const string ProviderTypeChange = "providerTypeChange";
            public const string Gap = "gap";
            public const string NoGap = "noGap";
            public const string NewApp = "newApp";
            public const string ChangeEffectiveDate = "changeEffectiveDate";
            public const string ConvertFromORPWFNoGap = "convertFromORPnoGap";
            public const string ConvertFromORPWFGap = "convertFromORPGap";
        }

        public static class BCITextRTPEmail
        {
            public const string Default = "0";
            public const string ToSend = "1";
            public const string Sent = "2";
        }

        public static class DataFixTypes
        {
            public const string REG = "Registration";
            public const string STG = "Staging";
        }

        public static class TableOperations
        {
            public const string Insert = "Insert";
            public const string Update = "Update";
            public const string Delete = "Delete";

        }
        public static class SequencesCode
        {
            public const string Principal = "1";
            public const string Admitting = "2";
            public const string PatientReasonforVisit = "4";
            public const string ExternalCauseofInjury = "5";
            public const string Other = "3";
        }

        public static class ProviderFinancial
        {
            public const string Inquire1099 = "1099Inquire";
            public const string History1099 = "1099History";
        }

        public static class ProviderFinancialErrors
        {
            public const string PFFailureType = "Failure";
            public const string PF1099ErrorCode0 = "PF-7000";
            public const string PF1099ErrorCode1 = "PF-7001";
            public const string PF1099ErrorCode2 = "PF-7002";
            public const string PF1099ErrorCode3 = "PF-7003";
            public const string PF1099ErrorCode4 = "PF-7004";
            public const string PF1099ErrorCode5 = "PF-7005";
            public const string PF1099ErrorCode6 = "PF-7006";
            public const string PF1099ErrorCode7 = "PF-7007";
            public const string PF1099ErrorCode8 = "PF-7008";
            public const string PF1099ErrorCode9 = "PF-7009";
            public const string PF1099ErrorCode10 = "PF-70010";
            public const string PF1099ErrorCode11 = "PF-70011";
            public const string PF1099ErrorCode12 = "PF-70012";
            public const string PF1099ErrorCode13 = "PF-70013";
            public const string PF1099ErrorCode14 = "PF-70014";
            public const string PF1099Message = "Internal Error. Please try resending the transaction";

            public const string History1099 = "1099History";
        }

        public static class MemberEligibilityErrors
        {
            public const string MEFailureType = "Failure";
            public const string MEErrorCode0 = "PF-7000";
            public const string MEErrorCode1 = "PF-7001";
            public const string MEErrorCode2 = "PF-7002";
            public const string MEErrorCode3 = "PF-7003";
            public const string MEErrorCode4 = "PF-7004";
            public const string MEErrorCode5 = "PF-7005";
            public const string MEErrorCode6 = "PF-7006";
            public const string MEErrorCode7 = "PF-7007";
            public const string MEErrorCode8 = "PF-7008";
            public const string MEErrorCode9 = "PF-7009";
            public const string MEErrorCode10 = "PF-70010";
            public const string MEErrorCode11 = "PF-70011";
            public const string MEErrorCode12 = "PF-70012";
            public const string MEErrorCode13 = "PF-70013";
            public const string MEErrorCode14 = "PF-70014";
            public const string MEMessage = "Internal Error. Please try resending the transaction";
        }

        public static class PASearchHelpText
        {
            public const string PASearchHelpText1 = "Newly submitted PAs may take up to 30 minutes to show up in a search or inquiry.";
        }

        public static class DelegateAffilationReprocessing
        {
            public const int ReProcesOnlyStaging = 0;
            public const int Default = 1;
            public const int UpdateUploadDocID = 2;
            public const int UpdateResponseDocID = 3;
        }

        public static class AutomatedReports
        {
            public const string AutomatedReportsAppID = "D1AFF5D8-D50A-4E3E-B4F3-9EBD0C290215";
            public const string AutomatedReportsProcessAppID = "D1AFF5D8-D50A-4E3E-B4F3-9EBD0C290215";
            public const string AutomatedReportsEmailAppID = "32C1CB30-6B17-47D6-B37D-F956F1EEC5D3";
            public const string AutomatedReportsSchedulerAppID = "017A8737-D07B-4DC7-92F4-DD1695510A27";
            public const string AutomatedReportsMainUI = "40A28BBA-B3EA-4918-B04B-4B15F4D9562E";
            public const string Cancelled = "Cancelled";
            public const string Failed = "Failed";
            public const string Completed = "Completed";
            public const string ToEmail = "ToEmail";
            public const string Scheduled = "Scheduled";
            public const string ToProcess = "ToProcess";

            public const string AutomatedReportsMainUISuccessInsert = "Record Inserted Successfully";
            public const string AutomatedReportsMainUISuccessUpdate = "Record Updated Successfully";
            public const string AutomatedReportsMainUISuccessDelete = "Record Deleted Successfully";

            public const string AutomatedReportsEmailSuccess = "Email Sent Successfully";
            public const string AutomatedReportsEmailFailed = "Failed to send Email";
            public const string AutomatedReportsMainUIInvalidGuid = "Invalid Guid Entered";

            public const string AutomatedReportsProcessBodySuccess = "Auto Generated Report Body Success";
            public const string AutomatedReportsProcessAttachmentSuccess = "Auto Generated Report Attachment Success";

            public const string AutomatedReportsProcessDocDescp = "Auto Generated Report";
            public const string AutomatedReportsProcessonBaseFail = "Failed to upload to onBase";
            public const string AutomatedReportsProcessDocIDFail = "Failed to generate docID";
            public const string AutomatedReportsProcessGenericFail = "Failed to generate the report, Please check the logs";
            public const string AutomatedReportsProcessFailed = "Failed to retrieve records";

            public const int LogPriorityError = 200;
            public const int LogPriorityInfo = 300;

            public const bool AutomatedReportsEmailSent = true;

            public const string AutomationReportEnrollment = "DDBFD445-AA00-46D7-833F-4CFD11666DFB";
            public const string AutomationReportAttachment = "A91B21C3-8B1A-43C9-8B09-DDFA092C6FC4";
            public const string AutomationReportWSTransactions = "4BF75641-9A68-4665-A7FF-9DD970AD38BB";
            public const string AutomationReportUploadAttachment = "D1F05849-5C20-4933-ACE6-E3B938F62E63";
            public const string AutomationReportDailyUploadAttachment = "C02DA3F6-393B-4E62-9C49-5CF4F99A9EC8";

            public const int AutomationReportMainInsert = 0;
            public const int AutomationReportMainUpdate = 1;
            public const int AutomationReportMainDelete = 2;
            public const int AutomationReportSubDelete = 3;

            public const string AutomationReportSubAttachNote = "Attached is a detailed report for the errored-out transactions. This email was generated automatically; please review the report and follow up if further action is needed.";

            public const string AutomationReportSubFinalNote = "This email was generated automatically; please review the report and follow up if further action is needed.";

            public const string AutomationReportSubBodyNote = "<span style=\"font-style: italic\">No failed transactions for this report.</span>";
        }


        public static class RemittanceAdviceMCE
        {
            public const string RemittanceAdviceMCEAppID = "7948AA0F-D41D-407E-B142-F5F3448BA229";
            public const string RemittanceAdviceMCEDescp = "Remittance Advice";

            public const int LogPriorityError = 200;
            public const int LogPriorityInfo = 300;

            public const int RemittanceAdviceMCEPageTypeID = 8;

            public const string RemittanceAdviceMCEResponseSuccess = "Success";
            public const string RemittanceAdviceMCEResponseFailure = "Failure";
            public const string RemittanceAdviceMCEResponseInProcess = "InProcess";

            public const int RemittanceAdviceMCEInsert = 1;
            public const int RemittanceAdviceMCEUpdate = 2;

        }

        public static class ReferToComplianceReasons
        {
            public const int ConvictionsOrDisclosures = 1;
            public const int BoardActiononLicense = 2;
            public const int Termination = 3;
            public const int ApprovalofAdditionalSpecialties = 4;
            public const int RTPIssues = 5;
            public const int OhioRiseIssues = 6;
            public const int ExclusionReview = 7;
            public const int ReapplicationReactivationApproval = 8;
            public const int BCIComplianceReview = 9;
            public const int Other = 10;
            public const int SiteVisit = 11;    //SAM538
        }

        public static class HCPCSLvl2
        {
            public const string HCPCSLvl2AppID = "EC5BF352-9B74-4081-B18A-BFABDFBDDE5F";
        }



        public static class PNMRESTAPIKeys
        {
            public const string SMAEnabledUVEnabled = "1";
            public const string SMAEnabledUVDisabled = "2";
            public const string SMADisabledUVEnabled = "3";
        }

        public static class LTCHomeProvider
        {
            public const string LTCHomeProviderAppID = "D7774686-EBB7-4C5D-B873-FCF17E16A05C";
            public const string LTCHomeProviderFileStatusSuccess = "Success";
            public const string LTCHomeProviderFileStatusFailed = "Failed";
            public const string LTCHomeProviderFileStatusInProcess = "InProcess";

            public const int LTCHomeProviderInsert = 1;
            public const int LTCHomeProviderUpdate = 2;

            public const int LogPriorityError = 200;
            public const int LogPriorityInfo = 300;

        }

        public static class ReSendNotices
        {
            public const string ReSendNoticesAppID = "F956B20F-E61F-43DD-AE2F-4322D4A2E75D";

            public const int ReSendNoticesSelect = 0;
            public const int ReSendNoticesInsert = 1;
            public const int ReSendNoticesUpdate = 2;
            public const int ReSendNoticesDelete = 3;
            public const int ReSendNoticesDeleteAll = 4;

            public const int LogPriorityError = 200;
            public const int LogPriorityInfo = 300;

            public const string ToProcess = "ToProcess";
            public const string PaperNoticeFailed = "Paper Notice Failed";
            public const string PaperNoticeSuccess = "Paper Notice Success";
            public const string EmailNoticeFailed = "Email Notice Failed";
            public const string EmailNoticeSuccess = "Email Notice Success";
            public const string PaperEmailNoticeFailure = "Paper and Email Notices Failure";
            public const string PaperEmailNoticeSuccess = "Paper and Email Notices Sent Successfully";

            public const string Failed = "Failed";
            public const string Success = "Success";

        }

        public static class SpecialtyUploadTitles
        {
            public const string LactationConsultant = "Proof of International Board of Lactation Consultant";
            public const string DCYFamilyConnects = "DCY Family Connects Documentation of Certification";
            public const string OhioRISEProviderPlan = "OhioRISE - Provider/Plan BH Respite or Waiver Services";
            public const string PediatricRecovery = "Proof of Pediatric Recovery";
        }

        public static class FinalDisposition
        {
            public const string Submitted = "Submitted";
            public const string NotProcessed = "Not Processed";
            public const string NotSubmitted = "Not Submitted";
            public const string Approved = "Approved";
            public const string ReturnToProvider = "Return To Provider";
            public const string NA = "N/A";
            public const string Terminated = "Terminated";
            public const string Disenrolled = "Disenrolled";
            public const string Suspended = "Suspended";
            public const string Closure = "Closure";
        }
        public static class EnrollmentType
        {
            public static string AdHocComment = "Ad-hoc Comment";
            public static string DataFix = "Data Fix (Superuser)";
            public static string Job = "Job";
        }

        public static class AddressPage
        {
            public const string BillingAndPayment = "Billing and Payment Address";
            public const string Correspondence = "Correspondence Address";
            public const string HomeAndOffice = "Home Office Address";
            public const string Hospital = "Hospital Address";
            public const string NursingFacility = "Nursing Facility Address";
            public const string Other = "Other Address";
            public const string PrimaryService = "Primary Service Address";
            public const string WorkHistory = "Work History";
            public const string AmbulanceInformation = "Ambulance Information";
            public const string AmbulancePickupAndDrop = "Ambulance Pickup And DropOff";
            public const string OtherPaymentInformation = "Other Payment Information";
            public const string PrimaryAddress = "Primary Contact Information";
            public const string Form1099Address = "1099 Address";
        }
    }
}
