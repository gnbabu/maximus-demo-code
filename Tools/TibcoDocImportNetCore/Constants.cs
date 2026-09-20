using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace TibcoDocImportNetCore
{
    //TODO: EDV A lot fo these constants should be enums and not integer constants
    public class Constants
    {
        public const string appAdminUserId = "3C14DD11-AF58-4CB7-972C-A0E51E47903F";
        public const string appWorkflowUserId = appAdminUserId;
        public const int appAdminPartyId = 750;
        public const string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
        public const string appPDMSAdminUserId = "E7854515-E84D-419B-B30E-720AE2C7EE4D";
        public const string emptyGuid = "00000000-0000-0000-0000-000000000000";
        

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
        public const string ProviderRoles = "ProviderOper,ProviderAdministrator";




        public const string ProviderServiceRoles =
            "ODMStateAdministrator,ProviderServices,Operator,ReadOnly,SanctionOperator,FAOperator1,FAOperator2,SiteVisitOperator,CredentialingSpecialist,LTCWorker,EnrollmentSpecialist,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ODMCredentialing,CredentialingQuality,CredentialingSupervisor,ComplianceSpecialist,TechAdmin,LTCInitial/Revalidation";


        public const string ScreeningOperatorRoles = "Operator,CredentialingSpecialist,EnrollmentSpecialist,LTCInitial/Revalidation";




        public const string OperatorRoles =
            "ProviderServices,Operator,SanctionOperator,SiteVisitOperator,CredentialingSpecialist,EnrollmentSpecialist,LTCWorker,LTCInitial/Revalidation";


        public const string CredentialingRoles =
            "CredentialingSpecialist,CredentialingChair,CommitteeQualitySpecialist,ODMCredentialingSpecialist,ODMCredentialingSupervisor,ODMCredentialingQualityAssurance,CredentialingQualityAssurance,CredentialingSupervisor";

        public const string InternalRoles = "ODMStateAdministrator,EnrollmentSpecialist,ComplianceSpecialist";
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
            public const string AppealSpecialist = "AppealSpecialist";
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
            public const string CEOCertified = "CEOCertified";
            public const string LTCInitialRevalidation = "LTCInitial/Revalidation";
            public const string TechAdmin = "TechAdmin";
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
            public const string NON_STATE_OPERATED_ICF_MR = "NON-STATE OPERATED ICF-MR";
            public const string NON_AGENCY_NURSE_RN_OR_LPN = "NON-AGENCY NURSE -- RN OR LPN";
        }

        public static class LTCFaciitiesProviderTypeID
        {
            public const int BuildingandWingIDforLTCFacilitesOnly = 90;
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
            public static int Confirmed = 1;
            public static int Modified = 2;
            public static int Terminated = 3;
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
            public const string FileLoadFailure = "File load failed, check file format. ";
            public const string JobServiceStartStopMessage = "Job Service service {0}. [ThreadId: {1}]";
            public const string JobServiceNoActiveJobs = "No active jobs";
            public const string LoadingExtractRecords = "Loading extract records: {0}";
            public const string LoadingExtractRecordsComplete = "Extract load complete: {0}";
            public const string LoadingFile = "Loading file: {0}";
            public const string LoadingFileComplete = "File load complete: {0}";
            public const string LoadingRecords = "Loading records [Total Records: {0}; Error Records: {1}]";
            public const string LoadingConfig = "Loading configuration variables";
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
            public const int IndividualNewReg = 4;
            public const int IndividualUpdateReg = 5;
            public const int IndividualUpdateOwner = 6;
            public const int IndividualRevalidation = 7;
            public const int GroupWithMembersNewReg = 8;
            public const int GroupWithMembersUpdateReg = 9;
            public const int GroupWithMembersUpdateOwner = 10;
            public const int GroupWithMembersRevalidation = 11;
            public const int InstitutionalOrFacilityNewReg = 12;
            public const int InstitutionalOrFacilityUpdateReg = 13;
            public const int InstitutionalOrFacilityUpdateOwner = 14;
            public const int InstitutionalOrFacilityRevalidation = 15;
            public const int ServicesNewReg = 16;
            public const int ServicesUpdateReg = 17;
            public const int ServicesUpdateOwner = 18;
            public const int ServicesRevalidation = 19;
            public const int PharmacyNewReg = 20;
            public const int PharmacyUpdateReg = 21;
            public const int PharmacyUpdateOwner = 22;
            public const int PharmacyRevalidation = 23;
            public const int GroupMemberProfileNewReg = 116;
            public const int GroupMemberProfileUpdateReg = 25;
            public const int GroupMemberProfileUpdateOwner = 26;
            public const int GroupMemberProfileRevalidation = 27;
            public const int PDMS = 1;
            public const int CAQH = 2;
            public const int Errors = 3;
            public const int DBHNewReg = 28;
            public const int DBHUpdateReg = 29;
            public const int DBHUpdateOwner = 30;
            public const int DBHRevalidation = 31;
            public const int HCBSNewReg = 32;
            public const int HCBSUpdateReg = 33;
            public const int HCBSUpdateOwner = 34;
            public const int HCBSRevalidation = 35;
            public const int EPDNewReg = 36;
            public const int EPDUpdateReg = 37;
            public const int EPDUpdateOwner = 38;
            public const int EPDRevalidation = 39;
            public const int ADHPNewReg = 40;
            public const int ADHPUpdateReg = 41;
            public const int ADHPUpdateOwner = 42;
            public const int ADHPRevalidation = 43;
            public const int StreamlinedNewReg = 44;
            public const int StreamlinedUpdateReg = 45;
            public const int StreamlinedUpdateOwner = 46;
            public const int StreamlinedRevalidation = 47;
            public const int EmergencyNewReg = 48;
            public const int EmergencyUpdateReg = 49;
            public const int EmergencyUpdateOwner = 50;
            public const int EmergencyRevalidation = 51;
            public const int CrossoverNewReg = 52;
            public const int CrossoverUpdateReg = 53;
            public const int CrossoverUpdateOwner = 54;
            public const int CrossoverRevalidation = 55;
            public const int PCAAideNewReg = 56;
            public const int PCAAideUpdateReg = 57;
            public const int PCAAideUpdateOwner = 58;
            public const int PCAAideRevalidation = 59;
            public const int PhysicianAssistantNewReg = 60;
            public const int PhysicianAssistantUpdateReg = 61;
            public const int PhysicianAssistantUpdateOwner = 62;
            public const int PhysicianAssistantRevalidation = 63;
            public const int MCOOnlyNewReg = 64;
            public const int MCOOnlyUpdateReg = 65;
            public const int MCOOnlyUpdateOwner = 66;
            public const int MCOOnlyRevalidation = 67;
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
            public const int EntityFacility = 3;
            public const int NonMedical = 4;
            public const int Pharmacy = 5;
            public const int GroupMemberProfile = 6;
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

            public const int NF_VENT = 347;
            public const int NF_WEAN = 352;
            public const int Pharmacy_340B = 300;
            public const int ODA_WAIVER = 258;
            public const int HCBS_ASSISTED_LIVING = 307;
            public const int DODD_WAIVER = 259;
        }


        public static class MMISSpecialtyType
        {
            public const int HOSPICE = 82;
            public const int ASSISTEDLIVINGSERVICES = 75;
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
            public const int SuspendProvider = 8;
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

            [Description("Return To Screening")] public const int ReturnToScreening = 10;

            [Description("Return To Site Visit")] public const int ReturnToSiteVisit = 11;

            [Description("Return To Provider Review")]
            public const int ReturnToProviderReview = 12;
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

            //AP JIRA#352
            public const string CountyCourtAgent = "CountyCourtAgent";
            public const string InfantMortalityLeadEntityAgents = "InfantMortalityLeadEntityAgents";

            public const string ProviderRelationsManagement = "ProviderRelationsManagement";
            public const string ProviderRelations = "ProviderRelations";
            public const string SiteVisitAdmin = "SiteVisitAdmin";
            public const string ODMStateAdministrator = "ODMStateAdministrator";
            public const string CPCSpecialist = "CPCSpecialist";
            public const string LTCCHOP = "LTCCHOP";
            public const string LTCInitialReval = "LTCInitialReval";
            public const string Internalapplicationentry = "Internalapplicationentry";
            public const string CredentialingSpecialit = "CredentialingSpecialit";
            public const string CredentialingSupervisor = "CredentialingSupervisor";
            public const string CredentialingQualityAssurance = "CredentialingQualityAssurance";
            public const string CredentialingChair = "CredentialingChair";
            public const string ODMCredentialingQualityAssurance = "ODMCredentialingQualityAssurance";
            public const string ODMCredentialingSpecialist = "ODMCredentialingSpecialist";
            public const string ODMCredentialingSupervisor = "ODMCredentialingSupervisor";

            public const string TechAdminMAX = "TechAdminMAX";
            public const string CEOCertified = "CEOCertified";



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
            public const string SiteVisit = "Site Visit Review";
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
        }

        public static class ComplianceTaskName
        {
            public const string UploadRR = "Upload Reconsideration Request";
            public const string UploadPAO = "Upload Proposed Adjudication Order";
            public const string RecordDateOfMailReturn = "Record Date Of Mail Return";
            public const string UploadAO = "Upload Adjudication Order";
            public const string RecordHearingStatus = "Record Hearing Status";
            public const string RecordDateAOMailed = "Record Date Of Adjudication Order Mailed";
            public const string TerminationReason = "Admin Enter Termination Reason";
            public const string CSUploadRR = "Upload Reconsideration Request";
            public const string CSUploadPI = "Upload Program Integrity";
            public const string CSEnterTR = "Compliance Enter Termination Reason";
        }

        public static int Max_DCS_Affiliations = 2;

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
            public const int SiteVisitRequired = 13;
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
            public const int PersonId = 15;
            public const int OrganizationId = 16;
            public const int ManagingEmployeeid = 18;
            public const int RealEstateIndividualId = 20;
            public const int RealEstateOrganizationId = 21;
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
            public const string Unknown = "U";
        }

        public static class TaxIDType
        {
            public const int EIN = 16;
            public const int SSN = 15;
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
            public const string INT = "DC Integration";
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
            public const string VOLUNTARYWITHDRAWAL = "36";
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

        public static class EnrollStatusReason
        {
            public const int TERMINATEDFEDEXCLUSIONSYSTEMREPORTINGONLY = 1;
            public const int TERMINATEDFEDEXCLUSIONSYSTEM = 01;
            public const int LICENSECERTIFICATIONREVOKED = 2;
            public const int LICENSE_CERTIFICATION_NOT_RENEWED = 3;
            public const int SUSPENDED_INDICTMENT_NON_FRAUD = 06;
            public const int STATE_INITIATED_TERMINATION = 6;
            public const int SUSPENSION_FRAUD_INDICTMENT = 13;
            public const int SUSPENSION_FRAUD_INDICTMENT_13 = 14;
            public const int DECEASED = 19;
            public const int TERMINATED_FRAUD_CONVICTION = 24;
            public const int LICENSE_SUSPEND_LICENSE_BRD = 28;
            public const int TERMD_DUE_NON_FRAUDCONVICTION = 35;
            public const int SUSPENDED_CREDIBLE_FRAUD_ALLEGATION = 37;
            public const int LICENSE_ABANDONED = 39;
            public const int DENIED = 41;
            public const int LICENSE_RESTRICTED_OR_LIMITED = 42;
            public const int MEDICARE_STATUS_NOT_ACTIVE = 64;
            public const string Termination_Failure_to_Attest = "AT";
            public const string LICENSE_CERTIFICATION_NOT_ACTIVE = "IN";

            public const int RETIRED = 38;
            public const int FAILURETOREVALIDATE = 33;
            public const int INACTIVECHOP = 54;

        }

        public static class MedicareEnrollmentStatus
        {
            public const string InProcess = "In Process";
            public const string Completed = "Completed";
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
        }
        //public static class AdminMaintenanceActions
        //{
        //    public const string DisEnroll = "Disenrollment";
        //    public const string RetroEffectiveDate = "Change Effective Date";
        //    public const string Terminate = "Terminate";
        //    public const string Reactivate = "Reactivate";
        //    public const string Update = "Update";
        //    public const string Suspend = "Suspend";
        //    public const string EditKeyIdentifiers = "Edit Key Identifiers";
        //}

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
        }

        public static class ReportSession
        {
            public const string ServiceProviderAgreements = "Service Provider Agreements";
            public const string MonthlyDatabaseChecks = "Monthly Database Checks";
            public const string SiteVisits = "Site Visits";
            public const string ApplicationFee = "Application Fee";
            public const string StatusOfProviderRegistrations = "Status Of Provider Registrations";
            public const string ProviderListingByAffiliation = "Provider Listing with Group Affiliation";
            public const string CredentialingReports = "Credentialing Reports";
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
            public const string FingerprintandBackgroundEntry = "Fingerprint and Background Entry";
            public const string FingerprintandBackgroundComplete = "Fingerprint and Background Complete";
            public const string FingerprintandBackgroundPending = "Fingerprint and Background Check Pending";
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
            public const int RealEstateIndividual = 25;
            public const int RealEstateOrganization = 26;
            public const int ProfessionalLicenseAddress = 39;
            public const int OtherServiceLocation = 40;
        }

        //public static class SectionTypeKeyValue
        //{
        //    //int is for section id and then in keyvalue pair string is the section name and int is the page type id

        //    static SectionTypeKeyValue()
        //    {
        //        PDMSService.PDMSServiceClient client = new PDMSService.PDMSServiceClient();
        //        var sectionData = client.SelectAllSections();
        //        foreach (DataRow sectionRow in sectionData.Tables[0].Rows)
        //        {
        //            SectionType.Add(int.Parse(sectionRow["REG_SECTION_TYPE_ID"].ToString()),
        //                new KeyValuePair<string, int>(sectionRow["REG_SECTION_TYPE_NAME"].ToString(),
        //                    int.Parse(sectionRow["REG_PAGE_TYPE_ID"].ToString())));
        //            SectionInformation.Add(int.Parse(sectionRow["REG_SECTION_TYPE_ID"].ToString()), sectionRow);
        //        }
        //    }

        //    public static Dictionary<int, KeyValuePair<string, int>> SectionType =
        //        new Dictionary<int, KeyValuePair<string, int>>();

        //    public static Dictionary<int, DataRow> SectionInformation = new Dictionary<int, DataRow>();

        //    public static string GetSectionDisplayName(int sectionId)
        //    {
        //        var sectionInfo = SectionInformation[sectionId];

        //        return sectionInfo["SECTION_DISPLAY_NAME"].ToString();
        //    }

        //}

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
            public const int OfficeHours = 32;
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
            public const int SubmitClaim = 10005;
            public const int SearchClaim = 10006;
            public const int HospiceEnrollment = 10007;

            public const int RetrieveReports = 10008;
            public const int ProviderFinancial = 10009;
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
            public static DateTime MaxDate = new DateTime(2299, 12, 31);
            public static DateTime MinDate = DateTime.MinValue;
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
            public const string Active = "A";
            public const string Inactve = "I";

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

        public static int WaiverEntryTaskID = 530;
        public static int RiskLevelEntryTaskID = 650;
    }
}
