using Corp.Core.Libraries;
using Corp.Core.Libraries.AttachmentServiceReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using AttachmentMessageHeader = Corp.Core.Libraries.AttachmentServiceReference.MessageHeader;

namespace MAXIMUS.Services.PDMS
{
    [ServiceContract]
    public interface IPDMSService
    {

        #region "Other"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TestException();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetActivityTypes();

        [OperationContract]                  //akash
        [FaultContract(typeof(ServiceException))]
        DataSet GetCertificationTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatistics();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegChopParent(Dictionary<string, object> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRegWaiverServices(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroups(int workFlowId, int providerCategoryTypeId, int workflowEventTypeId, int applicationTypeID = 0, int waiverTypeId = 0, int dashboardTableId = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroups_Services(int workFlowId, int providerCategoryTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroups_forGroupMemberProfile();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardProviderSummary(string roleName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroupTotals(int providerCategoryTypeId);

        #endregion

        #region "CoreLibraries"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEnvironment();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUIHeader();

        //[OperationContract]
        // Class1
        // Conss GetProduct();

        #endregion

        #region "Person"

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void SubmitPersons(DataSet Persons);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetFilteredPersons(string firstName, string lastName, string providerId
        //    , string medicaidId, string caqhId, string status, object mmisStatus, object errorType);
        #endregion
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMSPDuedateDetails();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRDMCodeSetsByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckIfCPCPrimary(int reg_id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ApproveRDMCodeSetsByName(string codeSetName, bool isApproved, Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRDMCodeSetstatusByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectRDMCodeSetChangeCount(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectWPCCodeSetChangeCount(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectCMSCodeSetChangeCount(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectC4CodeSetChangeCount(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectNUBCCodeSetChangeCount(string codeSetName);

        #region "Provider"
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProviderWithIds(
            string IdList
            , string submitRosterIdList
            , int tableId
            , int statusID
            , int ordinal
            , int sortBy
            , int pageNumber
            , int rowsPerPage
            , bool asc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProviders(
            string firstName
            , string lastName
            , string groupName
            , string providerId
            , string medicaidId
            , string caqhId
            , string npi
            , string taxId
            , string pdmsStatusId
            , string caqhStatusId
            , string emailAddress
            , string pdmsStatusDate
            , int riskLevelId
            , int sortBy
            , int pageNumber
            , int rowsPerPage
            , bool asc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProvider_NPI_TaxID(string npi, string taxId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchGroupProviders(Dictionary<string, object> parms, out int totalResultCount);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchOwnerProviders(string groupName, string firstName, string lastName, string taxId,
            string roleName, string IsUserAssigned, string userName, string sortColumn, int pageSize,
            int startRowIndex, bool getTotalRowCount, out int totalResultCount);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProvidersPublic(string providerTypeAbbrevList, string specialtyTypeIDList, string lastName, string firstName, string middleInitial,
            string city, string state, string zip, string quadrant, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProvidersPublicNew(Dictionary<string, object> parms, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProvidersGIS(string providerTypeAbbr, int pageSize);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchGroupProviderWithIds(string registrationIdList, int tableId, int statusId, int ordinal, bool isAssigned,
            string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchServiceLocations(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi,
            string taxId, int pageNumber, int rowsPerPage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchErrorWithIds(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal, int sortBy,
            int pageNumber, int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchErrors(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi,
            string errorCategoryId, string errorStatusId, string errorTypeId, string hardSoft, string errorDate, int sortBy, int pageNumber,
            int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDEAByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDocumentsByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSpecialtyByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderFull(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMatchingProviderByTaxNpi(string taxId, string npi, int specialtyTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMatchingProviderByTaxID(string taxID, bool includeWaiverServices);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMatchingProviderByNPI(string NPI, bool includeWaiverServices);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMatchingProvider(string NPI, string MedicaidID, string DD_Facility_num, string DD_Contract_num);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAvailableProvider(string NPI, string MedicaidID, string TaxID, string DD_Facility_num, string DD_Contract_num);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TerminateProviderGroup(string medId, string taxId, string npi);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderNotes(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderNotesByNoteID(int providerNoteID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderReplicaApps(int partyID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetContractMaintenanceById(int contractMaintenanceId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderStandardExtracts(int partyID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPartyErrorHistory(int errorId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ValidNPI(Int64 npi, string lastName, string firstName, string middleName, string state);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyProviderNPI(Int64 npi);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProviderNPI(string npi, string medicaidID, string lastName, string firstName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchHospiceProviderNPI(string npi, string medicaidID, string lastName, string firstName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchClaimProviderNPI(string npi, string medicaidID, string lastName, string firstName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetFacilityTypes(string facilityCode, string facilityDesc);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetFacilityTypeByFacilityCode(string facilityCode, string facilityDesc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorDiagnosisServiceDetail(int authType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int CreatePriorDiagnosisServiceDetail(int? SequenceId, int? diagCodeTypeId, string diagCode, string diagCodeDesc, string diagnosisDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorDiagnosisServiceDetail(int lineNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPayerNames();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ValidateServiceLocations(string taxId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void EmailNotify(int errorId, int toPartyId, string toEmail, int resolvingActionTypeId, int resolvingReasonTypeId,
            string terminateEnrollment, DateTime eventDate, string providerName, string npi, string userId, int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void OverrideErrorSubmitRoster(int errorId, int submitRosterId, int resolvingActionTypeId, int resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void OverrideError(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetUpdateRecordOption(int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetContiguousZipcode(string zip);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCountiesList();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRecord(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK,
            int opt, string newValue, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProvider(int partyId, string medicaidId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProviderID(int partyId, string providerID, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateMMISStatus(int partyId, int mmisStatusId, int medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateMMISServiceLocation(int partyId, int serviceAddressPopID, int? medicaidPK, DateTime? terminated, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateError(int errorId, int errorStatusTypeId, int? communicationEventId, int resolvingActionTypeId,
            int? resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateErrorRegistration(int regErrorId, int errorStatusTypeId, int? communicationEventId, int? resolvingActionTypeId,
           int? resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePDMSStatus(int errorId, int partyId, int pdmsStatusId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePDMSDataAdmin(int partyId, string providerID,
            string licenseNumber, string licenseState, string licenseEffDate, string licenseEndDate,
            string deaNumber, string deaState, string deaEffDate, string deaEndDate,
            string providerTypeID, string providerSpecialtyTypeID, string baseMedicaidID,
            string emailAddress, string credAddress1, string credAddress2, string credCity, string credState,
            string credZip, string credExtendedZip, string credPhoneAreaCode, string credPhoneNumber,
            string credFaxAreaCode, string credFaxNumber, string licenseTypeID, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePDMSDataAdminSvcLoc(int partyId, string serviceLocationID, string medicaidID,
            string servicingAddressName, string servicingAddress1, string servicingAddress2,
            string servicingCity, string servicingState, string servicingZip, string servicingExtendedZip,
            string servicingCounty, string servicingPhoneAreaCode, string servicingPhoneNumber, string servicingEmailAddress,
            string mailtoAddressName, string mailtoAddress1, string mailtoAddress2,
            string mailtoCity, string mailtoState, string mailtoZip, string mailtoExtendedZip,
            string mailtoPhoneAreaCode, string mailtoPhoneNumber, string mailtoEmailAddress,
            string paytoAddressName, string paytoAddress1, string paytoAddress2,
            string paytoCity, string paytoState, string paytoZip, string paytoExtendedZip,
            string paytoPhoneAreaCode, string paytoPhoneNumber, string paytoEmailAddress, string userId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int IsUserInSubRole(int regid, string userId, string roleName);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePDMSDataAdminReg(int regId, string medicaidID, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TerminateProviderRecord(int partyId, int medicaidPK, string medicaidId, string cdeEnrollStatus, string userId, bool referralProvider = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertPartyError(string errorCode, int errorStatusTypeId, int? partyId, int? submitRosterId, int? rosterExceptionId, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertProviderNote(int regID, int noteTypeID, string noteText, DateTime noteDate, string userId, int errorID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertDocument(int partyId, string name, string description, string fileName, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserProvGrp_XREF(string userID, string taxID, string npi);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteDocument(int documentID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetStgProviderById(string regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectForProviderFile(string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderContactEmail(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string SelectProviderContactEmailAddress(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectConversionProvider(string userId = null, string userName = null, int regId = 0, string taxId = null, string npi = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectConversionAffiliation(string regAffiliationId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet Search_NPPES_EntityType(string NPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ValidateIsPrimarySpeciality(int regID, int specialityID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet ValidatePracticePartners(int memberRegID, int convenerRegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet ValidatePracticePartnersUploadControl(int regID, int pageTypeID, string sectionName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GetValidateNPI(string npi, int regID = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetMedicaidIdsByUserName(string username);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetMedicaidIdsByMedicaidId(string medicaidId);
        #endregion

        #region "Return Roster"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSubmitRosterByNPI(string providerNPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISMatch(string providerNPI, string ssn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISServiceAddressByNPI(string providerNPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISServiceAddressByValues(string providerID, string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISServiceValues(string providerID, string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceAddresses(int submitRosterId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveMMISData(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveMMISDataOptional(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SavePartyStatus(int partyId, int providerStatusChangeTypeId, int pdmsStatusTypeId, int? medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SubmitReturnRosters(DataSet ReturnRosters);

        #endregion

        #region "State"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetStates();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCDSRequireStates();

        #endregion

        #region "Lookup Tables"
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRDMCodeSets();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectWPCCodeSets();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectWPCCodeSetsByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectWPCCodeSetStatusByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ApproveWPCCodeSetsByName(string codeSetName, bool isApproved);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectC4CodeSets();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectC4CodeSetsByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectC4CodeSetStatusByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetLookupDataUpdateTrace(string tablename);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveLookupDataUpdateTrace(string tablename, string approvalStatus, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ApproveC4CodeSetsByName(string codeSetName, bool isApproved);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCMSCodeSets();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCMSCodeSetsByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCMSCodeSetStatusByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ApproveCMSCodeSetsByName(string codeSetName, bool isApproved);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectNUBCCodeSets();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectNUBCCodeSetsByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectNUBCCodeSetStatusByName(string codeSetName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ApproveNUBCCodeSetsByName(string codeSetName, bool isApproved);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetApplicationTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWaiverTypes();

        [FaultContract(typeof(ServiceException))]
        [OperationContract]
        DataSet GetCAQHErrors();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCAQHStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetContentTypes(int contentTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetSpecialtiesByMMISId(string mmisSpecialtiesId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetActiveSpecialities(string medicaidId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceActiveEnrollSpan(string medicaidId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISErrors();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateEnrollmentStatusReasonID(int regID, int reasonCodeID, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTypeofPractice();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEnrollmentStatusType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTerminationStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMedicaidEnrollmentStatusType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMedicareEnrollmentStatusType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTermReason();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTaxIdType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderTypes2();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCredentialingProviderTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetCategoryFeeScheduleTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetAffiliateFilesByUser(Guid userID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetBulkAgentFilesByUser(Guid userID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetProviderTypesByProviderCategory(string CategoryID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderTypeById(int providerTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderTypesByTypeId(int applicationTypeId, int providerCategorytypeid, string roleName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderCategories(bool includeGroupMemberProfile);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderCategoriesByApplication(int applicationTypeID, int waiverTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPAAssignmentGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPAAssignmentProcCodes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPDMSStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPDMSErrors();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEnrollmentRejectReasons();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetOwnershipTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPracticeTypes();




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetResolvingActionTypes(bool documentsOnly, int partyId, int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetResolvingReasonTypes(int resolvingActionTypeId, int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISEnrollmentStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetErrorTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetErrorResolvingActions(int errorTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetErrorStatusTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetErrorCategoryTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetErrorDispositions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderNoteTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPermissionStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPermissionApplications();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSecurityQuestions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEducationTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDegreeAwardTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Guid CreatePasswordRequest(Guid userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet IsActiveReset(string recordid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSpecialtyTypes(string providerTypeID = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectODMCredentialingIsChecked(int REG_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDelegates();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetIndvAffPageRequired(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetBehaviourHealthAffRequired(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypesWithAbbrev(string roleName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypesPublicSearch();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectGroupSpecialtiesByProviderType(int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectGroupSpecialtiesByProviderTypeRole(int providerTypeID, bool isInternalUser, int regid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllSpecialtiesByProviderType(int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSpecialtiesByProviderType(int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyTypesBySpecProvType(int specialtyTypeId, int providerTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllLicenseTypes(bool isUsedInMMIS);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxEntityTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPracticeTypeW9();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllTransactionTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReportDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMaritalStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetGovernmentType();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPROFITType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPROFITStatus();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTYPE_OF_OWNERSHIP();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAccountTypeEntity();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEFT_TYPE();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAppealStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBackgroundStatusTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBackgroundVerificationStatusTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBackgroundResultTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBackgroundPerformedByTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectOrientationStatusTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyTypesWithProviderSpecialty();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllSpecialtyTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertTaxonomyType(int specialtyTypeID, int providerTypeID, string taxonomyCode,
            string taxonomyName, DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser,
            string mmisSpecialtyTypeID, bool npiRequired);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateTaxonomyType(int taxonomyTypeID, int specialtyTypeID, int providerTypeID, string taxonomyCode,
            string taxonomyName, DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser,
            string mmisSpecialtyTypeID, bool npiRequired);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllProviderTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllPAAssignProcedureGroups();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllSpecialtyTypesUnfiltered();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllApplicationTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertSpecialtyType(string specialtyTypeName, DateTime lastModifiedDateTime, string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateSpecialtyType(int specialtyTypeID, string specialtyTypeName, DateTime lastModifiedDateTime, string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertProviderType(string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
          string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProviderType(int providerTypeID, string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
          string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProviderCategoryType(int providerCategoryTypeID, string providerCategoryTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
            bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertProviderCategoryType(string providerCategoryTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
            bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPAAssignProcedureGrp(string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd, string createdByUser, DateTime createdOnDate, DateTime lastModifiedDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePAAssignProcedureGrp(int id, string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimProcEffectiveDate(string procCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertApplicationType(string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateApplicationType(int applicationTypeID, string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegPageSettingActionAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegPageTypeAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegPageSettingAll(string filterExpression);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionUploadControlAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypeFeeAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectApplicationFeePaymentTypeAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestDocumentTypeAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionTypeAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertApplicationFeePaymentType(string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPaperRequestDocumentType(string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertProviderTypeFee(int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
            Guid lastModifiedUser, int entityTypeID, int applicationTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegPageSettingAction(string regPageName, string roleName, bool takeActionAllowed,
            DateTime lastModifiedDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegPageSetting(int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegPageType(string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser, int? sequenceID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegSectionUploadControl(int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
         int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
            string regPageSection, string regPageName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateApplicationFeePaymentType(int applicationFeePaymentTypeID, string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePaperRequestDocumentType(int paperRequestDocumentTypeID, string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProviderTypeFee(int providerTypeFeeID, int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
            Guid lastModifiedUser, int entityTypeID, int applicationTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegPageSettingAction(int regPageSettingActionID, string regPageName, string roleName, bool takeActionAllowed,
           DateTime lastModifiedDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegPageSetting(int regPageSettingID, int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegPageType(int regPageTypeID, string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser,
            int? sequenceID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegSectionUploadControl(int regSectionUploadControlID, int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
            int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
                string regPageSection, string regPageName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAppSettingsAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllAutomatedReports();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDataFixTablesAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectWebAPITestingAll();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUpdateAutomatedReportsMain(Dictionary<string, object> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertDynamicFieldConfiguration(Dictionary<string, string> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUIControlVisibilityConfig(Dictionary<string, string> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDynamicFieldDisplayName(Dictionary<string, string> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDynamicFieldConfiguration(Dictionary<string, string> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUIControlVisibility(Dictionary<string, string> parms);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAutomatedReportsSub(Guid ReportID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertDataFixTables(string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertWebAPITesting(string APIName, string APIDescp, string APIXML, bool APIEnabled);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDataFixTables(int id, string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteDataFixTable(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateWebAPITesting(int id, string APIName, string APIDescp, string APIXML, bool APIEnabled);

        #endregion

        #region "Submit Roster"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckSubmitRoster(string providerNPI, string ssn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckProviderRevalidated(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEmptySubmitRoster();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEmptyServiceAddress();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSubmitRosterById(int SubmitRosterId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AddSubmitRosterExisting(DataSet ds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SubmitSubmitRosters(DataSet SubmitRosters, int pdmsStatusId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SubmitServiceAddress(int submitRosterId, DataSet ds);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertAddress(
                           string Street1,
                           string Street2,
                           string City,
                           string State,
                           string Zip,
                           string Email,
                           string Name,
                           int AddressType_ID,
                           string ExtensionData,
                           int XREF_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertProviderGroupAddress(
                                   string Street1,
                                   string Street2,
                                   string City,
                                   string State,
                                   string Zip,
                                   string Email,
                                   int AddressType_ID,
                                   string ExtensionData,
                                   int ProviderGroupId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertDEA(
                               DateTime? EffectiveDateTime,
                               DateTime? EndDateTime,
                               int XREF_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertLicense(
                                  int? License,
                                  string StateCode,
                                  string TypeCode,
                                  DateTime? EffectiveDateTime,
                                  DateTime? EndDateTime,
                                  int XREF_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertPhone(
                           int PhoneType_ID,
                           string Number,
                           int XFER_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertImportedFolder(
                                   string FolderName,
                                   DateTime ImportDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> GetImportedFolderNames();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertProviderGroup(
                                               string ServiceName,
                                               string MedicareId,
                                               string NpiId,
                                               string TaxId,
                                               DateTime? GroupEndDateTime,
                                               string ExtensionData,
                                               int? Response_ID,
                                               int? Request_ID,
                                               int XFER_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertSubmitProviders_Request(
                                               int? ProviderId,
                                               int? CaqhId,
                                               string DeaId,
                                               int? SsnId,
                                               int? NpiId,
                                               int? MedicareId,
                                               int? TaxId,
                                               string Action,
                                               string LastName,
                                               string FirstName,
                                               string MiddleName,
                                               string SuffixName,
                                               string Country,
                                               DateTime? BirthDateTime,
                                               DateTime? AttestDateTime,
                                               string ProviderTypeCode,
                                               string ProviderSpecialtyCode,
                                               string TaxonomyCode,
                                               string DisclosureRec,
                                               DateTime? DisclosureDateTime,
                                               string EnrollmentStatusCode,
                                               DateTime? TermDateTime,
                                               string StatusCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertSubmitProviders_Response(
                                       string ExtensionData,
                                       string CorrelationId,
                                       string Errors,
                                       string TransactionId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRetrieveProviders_Request(string Sak_Trans);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRetrieveProviders_Response(
                                               string ExtensionData,
                                               int? CorrelationId,
                                               string Errors,
                                               int? ProviderId,
                                               string CountryCode,
                                               int? MedicareId,
                                               string ErrorCode,
                                               string StatusCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, int> SelectAddressType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, int> SelectPhoneType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Dictionary<string, int> SelectTransmissionType();
        #endregion

        #region "Interfaces - Staging"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAppSetting(string key);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAddressStagingTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet VerifyTaxIdExistsAsEINForSSN(string taxID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SubmitStagingProviders(DataSet StagingProviders);

        #endregion

        #region "Roster Exceptions"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRosterExceptions();

        #endregion


        #region "Registration"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectOwnerCategoryType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTypeOfCoverage();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDefendentType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectClaimStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderPaymentType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDMEQualificationType(int CategoryID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProductServiceSubCategoryType(int CategoryID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProductServiceCategoryType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCategoryOfServiceType(int ProviderTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegistration(string formCompletionName, string formCompletionPhone, int registrationStatusTypeId, int diddReferralId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int OnlyInsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicTotNumOfBeds(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReg_Dental_Licenses(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReg_Dental_LicenseType(int regDentalId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertDentalRegistrationData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectVisionProvidersByID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPharmacyProvidersByID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectREG_POLICY(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectREG_INSURANCE_TYPE();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate, DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int UpdateRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate, DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePage_Configuration(int Page_Configuration_ID, string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertPage_Configuration(string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertUploadDocument(string Name, string Description, string FileName, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistration(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationCustom(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateCMCLinksVisibility(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationAffiliations(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationProgramStatus(int regId, int regProgramStatusTypeId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegistrationUserXref(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void RemoveReceiveACHData(int regId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void RemoveQuestionData(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectQuestionType(string questionTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistration(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRevalidationProviders(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchRegistration(string taxId, string npi, string ssn, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet FindRegistrationNPIDuplicate(string npi, string taxID, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDEARecordRegistration(string DEANumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationData(int regId, string tableName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsEnrollmentActive(string npi, string providerType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAddressCustomData(int regId, int addresstype, string tableName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAddressCustomDataPagingNew(int regId, int addresstype, int PageNumber, int RowsPerPage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAddressADDRESSCustomByID(int regId, int addresstype, int regAddrID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAddressCustomDataSortedNew(int regId, int addresstype, int PageNumber, int RowsPerPage, string sortOrder, string sortColumn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEnrollmentdateforNPI(string npiNumber, string state);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationAuditData(int regId, string tableName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRenderingLocationsByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string CheckRegistrationExistsByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationByUserId(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegDocuments(int regId, int regPageTypeId, string regPageSection, string regPageTypeExclusions, int? screeningActivityID, string roleName = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAgreementInitials(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegNotes(int regId, int regPageTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegErrorTypes(int regPageTypeId, string userId, string userRole = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSubcontractors(int regId, int subcontractorTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegistrationDataTable(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateORPFlag(int regId, bool flag);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetORPFlagforRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationDataTable(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms);

        // Overloaded methods - use the Insert/Update methods above if possible
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegistrationData(int regId, string tableName, Dictionary<string, string> parms);

        // Overloaded methods - use the Insert/Update methods above if possible
        [ServiceKnownType(typeof(DataTable))]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegistrationDataObject(Dictionary<string, object> parms);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertRegDocument(int regId, int regPageTypeId, string regPageSection, string name, string description, string fileName, string userId, int? screeningActivityID, string wftaskname, string documentUploadPage);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ProcessGlobalAdminChangeRequest(string currentOHID, string newOHID, int docID, int onBaseDocID, DateTime createdDateTime, string lastModifiedUser);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetRegistrationIdByUserId(string userId, string currentAdminOHID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegProviderNote(int regId, int regPageTypeId, int regSectionTypeId, int noteTypeID, string noteText, DateTime noteDate, int stepId, string userId);




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int UpdateRegistrationData(int regId, string tableName, Dictionary<string, string> parms);
        // End Overloaded methods

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertUploadFile(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetProviderTypeByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetProviderTypeIdByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationSectionStatus(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationPageStatus(int regId, int regPageTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            string changedBy, int? FinancialReviewStatus, int? LTCReviewStatus, int? StateReviewStatus, int? DBHReviewStatus, int? DDSReviewStatus, int? DHCFReviewStatus, int? DHCFApplicationFeeReviewStatus, int? StateApplicationReviewStatus, int? DBHReviewerReviewStatus, int? DDSReviewerReviewStatus);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteDocumentMailById(int priorAuthDocumentMailId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SyncRegistrationPageStatus(int regId, int regPageTypeId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationQuestion(int regId, string questionTypeId, int response, int modifiedStatusTypeId,
            string changedBy, string responseComment);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationAgreementInitials(int regId, string questionTypeId, int response, int modifiedStatusTypeId,
            string changedBy, string responseComment, string Initials);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationOwnerXref(int regId, int regOwner1Id, int regOwner2Id, int relationshipTypeId, int modifiedStatusTypeId,
            string changedBy, int relationshipTypeId2);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegPageStatus(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckEnrolledProvider(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckHRTermFieldsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckRRTermFieldsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetHRTermFieldsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRRTermFieldsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRelationshipTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectREG_ERRORcustom(int regId, int pageTypeId, int sectionTypeId, bool includeClosed, bool includePartyErrors);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegistrationData(string tableName, string idColumnName, int id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegistrationDataWithParams(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserWorkflowPermission(string USERID, int WORKFLOW_ID, string LAST_MODIFIED_USER);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteUserWorkflowPermission(string USERID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUserWorkflowPermission(string USERID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet TransferPDMStoRegistration(bool ownershipChanged, int partyId, string userId, DateTime requestedEffectiveDate,
            DateTime changeEffectiveDate, string formCompletionName, string formCompletionPhone);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet TransferPDMStoRegistrationIndividual(int partyId, string userId, string firstLastName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetGroupNames(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetGroupAffiliationHistory(int regId, int pageSize, int pageNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWFProcessByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool RegistrationNeverSubmitted(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionUploadControl(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionUploadControlandDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id, int rowId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelecIncidentComplianceDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id, int rowId, string IncidentCaseNum);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionUploadDocument(int regPageTypeId, int regID, int? providerTypeId, string regPageSection, int rowId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCurrentWFTaskInfo(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckOwnerAddressExists(int regId, int addressTypeId);

        #endregion

        #region "User Accounts"
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetFilteredUsers(string username, string contactname, string orgName, string taxId, string NPI, string role, int pageSize, int startIndex);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteUserApplicationType(string userId, int applicationTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteUserProviderType(string userId, int providerTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteUserStatusType(string userId, int statusTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserSecurityQuestions(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetExistingProviders(string taxId, string NPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserApplicationTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserProviderTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserStatusTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserSecurityQuestionsByNameEmail(string userName, string email);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUsernameForProvider(string email, string taxId, string NPI,
            string medicaidId, string zip);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserAccountInformation(string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCMCProviderDetails(string medicaidId, Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt,
            string medId, int provCat, int provType, string taxId, int taxIDTypeID, string npi, string groupName, int zip, int diddReferralId,
            int? taxonomyTypeId, int? specialtyTypeId, DateTime? changedDate, string changedBy, bool is_OHID, string OHID, string UserType, string IOPUserName, int? regId = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveUserIOPToken(Guid userId, string tokenResp, Guid changedBy, string userClaimResp, bool updateLastLoginDt, string IOPaccessTkn, string IOPrefreshTkn, string IOPidTkn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveUserPNMToken(Guid userId, string tokenResp);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserApplicationType(string userId, int applicationTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserProviderType(string userId, int providerTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserStatusType(string userId, int statusTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Boolean IsAnswerCorrect(string userName, int questionNumber, string answer);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyProviderAccountCreation(string recipients, string LEGAL_BUSINESS_NAME, string NPI, string USER_NAME, string LOGIN_URL, int regId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyPasswordReset(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyPasswordResetEmail(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool NotifyUserName(string email, string mcaid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt,
            DateTime? changedDate, string changedBy, bool FORCE_PASSWORD_RESET, string UserType, bool isTempPwdSent = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectActiveUsersByRole(string role);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDateSigned(string RegID, DateTime Datesigned, Guid User);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUsersInRoles(string roles);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUsersInProviderAdminRole(string roles, int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUsersInRolesByTaxID(string roles, string taxID, string excludeUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectEmailNotifications(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string columnName = null, string sortDirection = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectEmailNotificationsByRegID(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string sortBy, string sortDirection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEmailAttachments(string COMMUNICATION_EVENT_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationStatuses(string UserId, int REG_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationXREF(int REG_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void TransferRegistrationToNewUser(int REG_ID, string fromUserId, string toUserId, bool isFromUser);
        #endregion

        #region PDMS Workflow

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string WF_ProcessTask(int processID, string assemblyName, string className);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<string> WF_ProcessAllTasks();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int WF_StartStep(int processID, string ownerID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void WF_CancelWorkflowProcess(int processID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectStepActions(int stepID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectStepInfo(int stepID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectProcess(int processID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectProcessParameters(int processID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectStepParameters(int stepID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void WF_SaveProcessParameter(int processID, string parameterName, string parameterValue);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void WF_SaveStepParameter(int stepID, string parameterName, string parameterValue);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void WF_TakeAction(int processID, string action, string notes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectActiveOwnerSteps(string ownerID, bool includeOtherSteps);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectActiveOwnerStepsProvider(string ownerID, bool includeOtherSteps, string ownerRole);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectUnassignedSteps(string groupName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectRegistrationWorkflows(string ownerID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectWorkflowByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectWorkflows();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void updateWF_STEP_Owner(int stepId, string ownerId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void updateWF_STEP_Assignment(int stepId, string assignedByUser);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetWorkflowInstance(int? applicationTypeID, int? providerCategoryTypeID, int? providerTypeID, int? referralTypeID,
            bool revalDue, bool referral, bool conversion, bool groupMember);
        #endregion

        #region Admin
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTransactions(string TransactionTypeId = null, string ProcessStartDate = null, string ProcessEndDate = null, string PartyID = null, string ServiceLocationID = null, string MedicaidID = null, string CAQHId = null, string OrderBy = null, string SortOrder = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void CancelTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ResubmitProcessedTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser, bool isCreatedByJob = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISTransactionsForRegistration(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWorkflowStepsForRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWorkflowStepsForProcessID(int ProcessId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetApplicationDetailsbyProcessID(int ProcessId, int regId);
        #endregion

        #region Documents and Reports
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void LogAccess(string UserId, string DocumentId, string ProviderId, string DocumentFormat, int REPORT_DOCUMENT_TYPE_ID, MAXIMUS.Core.Libraries.Enumerations.LogAccessType AccessType, string NPI);
        #endregion

        #region DIDD Services

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRegistrationStatusForDIDDReferral(int referralID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDRegions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDWaivers();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDServices();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDServicesByDIDD_Service_ID(int DIDD_Service_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferral(int referralId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralByID(int referralId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchProviderAllTaxID(string TaxID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDuplicateReferrals(int Referral_ID, string TaxID, string ZipCode, string ZipExt);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralByApplicationNo(string applicationNo, string taxID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchDIDDReferral(string name, string applicationNo, string taxId, string medicaidId, string npi, string emailAddress, string createdBy, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchDIDDReferralByRegIDs(string RegIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, bool isAssigned, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchDIDDReferralByReferralIDs(string ReferralIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet InsertDIDDReferral(string firstName, string lastName, string groupIdentityName, string applicationNo, string taxId, string NPI, string email,
            DateTime? contractFromDate, DateTime? contractToDate, bool isActive, string zip, string zipExt, int submitTypeID, int locationTypeID, DateTime createdOn, Guid createdBy, int referralType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyProviderOfDIDDReferral(string firstName, string lastName, string groupIdentityName, string appNumber, DateTime? contractFromDate, DateTime? contractToDate, string email, string inputSubject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDIDDReferral(int diddReferralID, string firstName, string lastName, string groupIdentityName, string taxId, string NPI, string email,
            DateTime? contractFromDate, DateTime? contractToDate, string zip, string zipExt, int submitTypeID, int locationTypeID,
            DateTime modifiedDate, Guid modifiedUser, int referralType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDIDDReferralSuffix(int diddReferralID, string regionSuffix);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertDIDDReferralService(int diddReferralId, int diddServiceId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralService(int referralServiceId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralServiceByReferralServiceID(int referralServiceID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertDIDDREFERRALWAIVER(int diddReferralId, int diddWaiverId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertDIDDREFERRALREGION(int diddReferralId, int diddRegionId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int UpdateDIDDReferralService(int diddReferralServiceId, int diddReferralId, int diddServiceId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int DeleteDIDDREFERRALSERVICE(int diddReferralId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int DeleteDIDDREFERRALREGION(int diddReferralId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int DeleteDIDDREFERRALWAIVER(int diddReferralId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDContractDetails(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSignatureHistoryByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectContractHistoryByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCurrentContractInfoByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertUpdateContractByContractTypeID(int regID, int contractTypeID, string adminName, DateTime? contractStartDate, DateTime? contractEndDate, string signedBy, DateTime changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDReferralByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDIDDReferralStartDate(int regID, DateTime startDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateDIDDReferralServiceDates(int referralServiceID, DateTime startDate, DateTime? endDate, DateTime modifiedOn, Guid modifiedBy);

        #endregion

        #region DCS / Party Eligibility Services
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckAttestToFeePaymentRequired(int regID);


        #endregion

        #region ACH Fee Services

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom,
            DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo, int sortBy, int pageNumber, int rowsPerPage, bool sortAsc);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom,
            DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertAchFeeInformation(int partyID, DateTime paymentDate, string edisonID, string paymentNumber, DateTime createdDateTime, Guid createdByUserID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectACHFeeInformationByPartyID(int partyID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDocumentTypeByName(string documentName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ResetContractSignatures(int regID, DateTime changedDate, Guid changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPageConfigurationsSectionsByPageName(string PageName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPageConfigurationsByPageName(string PageName, Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPageConfigurations();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBumpUpReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderRiskLevels();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderContractType(string typeBorR);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderArea(string programCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReferenceDataWithoutParam(string storedProc);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMCP();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProgramType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderCPCAccreditation();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectEnrollmentStatusReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReconsiderEnrollmentStatusReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsNotProcessedEligible(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderContractStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllTaxonomyCodes();

        #endregion

        #region Action History

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectActionHistoryData(string name, string npi, string taxID, string actionType, string actionStatus, string taskName, string startDate,
             string endDate, string userName, string roleName, int pageNumber, int rowsPerPage, int sortBy, bool isASC);


        #endregion

        #region Screening

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectGroupProviderScreeningData(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectAffiliationsScreeningData(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectOwnersScreeningData(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectOwnersScreeningEndDate(int regID);


        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectOwnersScreeningDataWithEndDate(int regID, int screeningID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectHouseholdMemberScreeningData(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderScreeningActivityData(int screeningID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderTypeWithLicenses(int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderScreeningActivityMatchData(int screeningActivityTypeID, int screeningActivityID, int regID, int regAffiliationID, int regOwnerID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectScreeningActivityDocuments(int screeningActivityID, string userRole);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertScreeningActivityDocument(int screeningActivityID, string name, string description, string fileName, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool RemoveScreeningExlusionData(int regID, int exclTypeID, string comments, Guid userID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertProviderScreening(int regID, int workflowID, string userID, int DODDactivityStatus, string OwnerDODDActivityStatus, out bool isExactMatchFound, out bool isSoftMatchFound);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet TerminateProvider(int regID, DateTime termDate, string changedBy, string enrollmentStatusCode, string enrollmentStatusReason, bool createTrans, bool is_frm_job = false);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningActivityStatus(int screeningActivityID, int screeningActivityStatusID, string adverseAction, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningStatus(int screeningID, int screeningStatusID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningPreviousStatus(int regID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningScreeningStatusID(int screeningId);


        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningResult(int screeningID, int screeningResultID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertScreeningAdverseAction(int screeningActivityID, string description, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectScreeningAdverseActions(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectScreeningStatus(int screeningID, int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectAdverseActions(int screeningActivityID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectScreeningFailedActivities(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSiteVisitScreeningData(int regID, int? screeningActivityTypeID = null);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSiteVisitData(int RegID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSiteVisitAttemptData(int siteVisitID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSiteVisitDetails(int siteVisitID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSiteVisitAttemptDetails(int siteVisitAttemptID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateSiteVisitResult(int siteVisitID, int resultID, int? recommendationID, DateTime? dateCompleted, string completedBy, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertSiteVisitAttempt(int siteVisitID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments, DateTime changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateSiteVisitAttempt(int siteVisitAttemptID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments, DateTime changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID, int? siteVisitFindingsID, DateTime? NodIssueDate, DateTime? POCDate, string complianceComments);
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateSiteVisitAttemptComplianceSpecialist(int siteVisitAttemptID, int? siteVisitFindingsID, string comments, DateTime? nodIssueDate, DateTime? pocDate, string complianceUser);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateSiteVisitAttemptDueByDate(int siteVisitID, DateTime? requiredDate);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateSiteVisitAttemptStatus(int registraionId, int siteVisitStatusID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectPendingSiteVisits();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPendingSiteVisits(string ownerid);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetSiteVisitsReferredToState();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUnassignedSiteVisits(int attempt, string assignedto, int regID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitRecommendations();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitResults();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitMethods();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitFindings();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitScreeningStatuses(int? providerTypeID, int? isInitialStatus);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectServiceRemovedCheckBox(int regID);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectApplicationFeeWaiverReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectApplicationFeePaymentType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectApplicationFeeStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ResetApplicationFee(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectGroupAffiliationStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectIndividualAffiliationPrivilegesStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectIndividualAffiliationStaffCategoryStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMMISTrackingDetails();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchCostReportDetails();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet MSPSearchCostReportDetails();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet HospitalSearchCostReportDetails();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsPendingConvertedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllRegistrationsByTaxID(string taxID, Guid userID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsWithSameNPIOverlapEffectiveDate(int reg_id, DateTime effective_date, string npi);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUserNameByUserID(string userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertTransactionMonitorNote(int regID, int noteTypeID, string noteText, DateTime noteDate, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateTransactionAssignedStatus(int transactionQueueID, string assignedStatus, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchTransactionsMonitor(string regId, string medicaidID, string fromDate, string toDate, string sortBy, int pageSize, int pageNumber, bool asc, bool nwffailures, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTransactionsMonitorCounts();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchByREGID(string regId, string tableName, string tableType, int pageSize, int pageNumber, string medId = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllRegistrationsByUserIDAndTaxID(Guid userID, string taxID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string loggedinUserID ,out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByAgentUserIDAndSubRole(Guid userID, string subroleName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool IsDeemedEligble(string userName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllAffiliationsByRegId(int regId, int affiliationStatus, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSubmittedAgreementPDFs(int regId, int regPageTypeID, string regPageSection, int pageSize, int startRowIndexSub, int startRowIndexUpd);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMCPAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string name, string npi, string ssn, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectConvertedRegistrationByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectConfirmedAffiliationByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPracticePartnership(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetProviderNameByMedicaidID(string medicaidId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet SelectPendingAffiliationByID(int pendingAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMalpracticeClaimByID(int MalpracticeClaimID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet DeleteMalpracticeClaimByID(int MalpracticeClaimID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAffiliationByMedicaidID(string medicaid_ID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPendingAffiliationByMedicaidID(string medicaid_ID, int regID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAffiliationByStatusMedicaidID(string medicaid_ID, int regID, int grpAffiliationStatusId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByMedicaidID(string medicaid_ID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectExitingProviderByMedicaidID(string medicaid_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHealthCareAffiliationByID(int healthCareAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegDMEProductServiceCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePNMApplicationStatus(int regID, int appStatus, Guid userID, int processID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegDMEProductServiceSubCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegDMEProductServiceQualificationByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_QUALIFICATION_Type_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePendingAffiliationByID(int pendingAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteHealthCareFacilityAffiliationByID(int healthCareAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteAssignedDelegateByID(int regAssignedDelegateID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectNursingProfessionalCertificationByID(int nursingProfessionalID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBoardCertificationByID(int boardCertificationID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCPRCertificationByID(int CPRID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderByGRPMedicaidID(string GRPMedicaid_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderByRegID(string Reg_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospitalFacilityByMedicaidID(string GRPMedicaid_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAffiliationByGRPMedicaidID(int reg_ID, string GRPMedicaid_ID, string GRPTaxID, string GRPNPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPendingAffiliationByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCredentialingDelegatesByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMalpracticeClaimByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHealthCareFacilityAffiliationByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByTaxID(string taxID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByNPI(string NPI);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetIndividualStandardSpanStartDate(string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByAdminUpdateKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
           string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationsByKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
           string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxId, int applicationType);

        // JIRA 3041 - Include DDContractNumber
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertNewProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt,
            bool IsPaperApplication, int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int waiverTypeID, bool retroEffectiveDate, string ddFacilityNumber, string DDContractNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertNewCPCProviderRegistration(int regID, string cpcType, string CPC_Program_Year, Guid userID, string CPC_Practice_Type);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCPCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID, string CPC_Program_Year, string CPC_Practice_Type);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CanReEnableCPCLinks(int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CanReEnableCMCLinks(int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CanStartCredentialReconsideration(int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCMCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckCMCLinkEnabledByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckCMCEnrollmentPeriod();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckCMCInvited(int RegId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet IsCMCEnrolled(int RegId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertNewLinkedProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt,
            bool IsPaperApplication, int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateConvertedProviderRegistration(int regID, Guid userID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
            int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatetoMedicaidProviderRegistration(int regID, Guid userID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int taxonomyTypeID, string zipCode, string zipExt, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate,
            int applicationTypeID, int waiverTypeID, int workflowEventTypeID, int WaiverServiceUpdateTypeID, string gender);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyInfoByCode(string taxonomyCode, int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReferral_UnusedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDIDDServicesByWaiverID(int waiverID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectQuestionTypesByIDBeginsWith(string beginsWith);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegAffiliationData(int regAffiliationID, string tableName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUpdateRegAffiliationQuestions(int regAffiliationID, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderLicenseByPartyID(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderSpecialtyByPartyID(int partyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePageConfiguration(int PageConfigurationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegAffiliation(int regAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectScreeningActivityStatusByActivityTypeID(int activityTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCitizenshipTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectImmigrationStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCertifiedBeds();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestStatusTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestQueueByUserID(Guid userID, int paperRequestTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderOperatorDashboardByUserID(Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestQueueByQueueID(int queueID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestErrorsByQueueID(int queueID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestMatchData(int queueID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestByDocumentHandle(int documentHandleID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperRequestDocumentsByQueueID(int paperRequestQueueID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPaperRequestQueue(int requestTypeID, int documentTypeID, int documentHandle, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
                int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePaperRequestQueue(int paperRequestID, int requestTypeID, int documentTypeID, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
                int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime modifiedOn, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPaperRequestError(int paperRequestID, int errorTypeID, int errorStatusTypeID, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPaperRequestDocument(int requestID, string name, string description, string fileName, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ValidatePaperRequest(int paperRequestID, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SelectNextUnassignedPaperRequest(Guid userID, DateTime requestedOn);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyDuplicateTaxonomySpecialtyForReg(int regId, int taxonomyTypeId, int specialtyTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyDuplicateSpecialtyForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyFutureDatedSpecialtyEnrollmentForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet VerifyIfCreatesInvalidSpecialtySpanForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate, DateTime endDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyActiveSpecialtyForRegBySpecialtyTypeID(int regId, int specialtyTypeId, DateTime startDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SetAddBCITextRTPEmailFlag(int regId, int specialtyTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool VerifyDuplicateTaxonomyForReg(int regId, string taxonomyCode, int regTaxonomyId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void LinkPaperRequestQueueToReg(int paperRequestID, int regID, DateTime modifiedOn, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet DisenrollProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, bool isFromJob = true);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SuspendProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, string enrollmentStatusReason = "");
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ReactivateProvider(int regID, DateTime newEffectiveDate, DateTime revalidationDate, string comments, string enrollmentstatuscode, DateTime modifiedOn, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void RetroEffectiveDateProvider(int regID, DateTime effectiveDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, DateTime newRevalDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GenerateDIDDContract(int regID, out string filename);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GenerateICFIIDContract(int regID, out string filename);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool GenerateApplication(int regID, string userName, bool saveDocument, out string filename);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet InsertExpressTerminatationWorkflow(int regID, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet PerformMoratoriaRematch(int regID, DateTime modifiedOn, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCountiesByStateAbbreviation(string stateAbbreviation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReports();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegistrationProviderTypesByCategory(int applicationTypeId, int categoryTypeID, int WaiverTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyTypeByID(int taxonomyTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDiddReferralLocationTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectGroupMemberProvidersByAffiliationID(int affiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypesByMMISProviderTypeID(string mmisProviderTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegistration(int regID, bool allowPostApplicationCompleteDelete);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void AdminUpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             string taxid, int taxIDTypeID, string npi,
            int taxonomyTypeID, string zipCode, string zipExt, string gender, DateTime modifiedOn, Guid modifiedBy, DateTime? birthDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             int taxIDTypeID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, int specialtyTypeID,
            int taxonomyTypeID, string practiceName, string zipCode, string zipExt, string gender, DateTime modifiedOn, Guid modifiedBy,
            DateTime npiStartDate, DateTime? npiEndDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void CancelRegistration(int regID, DateTime modifiedDate, Guid modifiedBy, int processId, string commandName = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void CancelRegistrationCPC(int regID, DateTime modifiedDate, Guid modifiedBy, int processID, int workflowEventTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCallTrackingReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCallTrackingSources();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCallTrackingNextActions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCallTrackingResolutions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertCallTrackingCall(int sourceID, int subjectID, int nextActionID, int resolutionID, DateTime startTime, DateTime endTime, TimeSpan duration, string regID, string callerOther, string reasonOther, string callDetails, string NPI, string medicaidID, DateTime createdOn, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCallsByRange(DateTime beginDate, DateTime endDate, int sourceID, int subjectID, int nextActionID, int resolutionID, int callID, string NPI, string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderPaymentInfoByRegID(int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseRestrictionCodes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectConvertedDocuments(int regID, int pageNumber, int pageSize);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPaperDocuments(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpateRegDocumentXref(int regId, int documentId, int rowId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateIncidentAlertFlag(int regId, DateTime changedDate, Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateIncidentReviewStatus(int regId, int caseStatus, DateTime changedDate, Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCLIACertificateTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool IsDDSProvider(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteRegistrationDocument(int documentID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetAlertsByTaxID(string taxID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet InsertIntoVersionTables(int RegID, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectTasksByRole(string role);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateTaskRankForRole(string role, int taskId, int rank);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectWorkflowRoles();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetElapsedTimeLimitByApplicationType(int applicationTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetElapsedDaysByApplicationType(int applicationTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetWorkflowProcessesForRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllSections();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTimesApplicationReturnedToProviderFromLTC(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMCOAffiliationStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMCOs();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegMCOAffiliation(int regMCOAffiliationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateToConvertToFeeForService(int currentRegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool HasNewOwnersOrChangeInOwnerInformation(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool HasPrimaryPracticeLocationChange(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderScreeningByProcessID(int processID);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet SelectServiceLocationByMedicaidID(string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserRoleCompatibility();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserRolesByUser(string userName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetLoweredUserNameByUserName(string userName, string userId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUserRolesByOHID(string ohid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAllUserRoles();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet IsRegistrationWentThroughAppeals(int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWorkflowSteps(int workflowId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetWorkflowRoles(int workflowId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertProviderCredentialing(int regID, int workflowID, string userID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderCredentialingData(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderCredentialActivityData(int regID, int credentialingID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectWFStepInfo(int stepId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet VerifyCurrentAssignedUserForStep(Guid userId, int stepId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegEnrollmentByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet UpdateRegForReactivateByProvider(int regId, DateTime modifiedOn, string modifiedBy, int workflowID);

        #region Password Expiry Rules
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDataRankTypes();
        #endregion

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderCredentialActivityMatchData(int credentialActivityID, int regID);

        //[OperationContract]
        //[FaultContract(typeof(Exception))]
        //void UpdateCredentialActivityDataRank(int credentialActivityID, int datarankId, string adverseAction, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectCredentialingCommitteeActivity(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialingCommitteeMember(int credentialingId, int actionStatusId, string memberUserName, string comments, DateTime actionDate, DateTime? changedDate, string userID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetCommitteeActivityStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        string GetReviewStatusByCommiteeMember(int regId, string userName);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialStatus(int regID, int statusID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialDiscontinueDate(int regID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialResult(int regID, int resultID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet WF_SelectUnassignedStepsByUserProviderType(string groupName, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool HasPendingCredentialActivities(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void updateWF_STEP_OwnerWithStartDate(int stepId, string ownerId, DateTime stepStartDate);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetRegistrationAutoApproveSections(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectActiveOwnerStepsCredentialProvider(string ownerID, bool includeOtherSteps, string ownerRole);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertTaxonomyCode(int specialtyTypeID, int providerTypeID, string taxonomyCode, string taxonomyDetail, DateTime expirationdate, string mmis_specialty_type_id, DateTime lastModifiedDate, Guid lastModifieduserID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Core.Libraries.NPPESAPIResult ValidNPIinNPPESApi(long npi);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool ValidateNPIUniqueness(int regId, string npi, bool isNursingFacility);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxonomyTypesToExclude(int providerTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaskGroupByTaskName(string taskName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderAddressInfo(int regId, int addressTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderReturnStatus(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderAddressLocationInfo(int regId, int sectionTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPowerAgentByUserId(string loggedinUserId, int pageSize, int startRowIndex,string providerAdminUserID, out int totalResultCount);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderAdminsForPowerAgent(string powerAgentUserId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet AddPowerAgent(string providerAdminUserId, string OHID, bool hasAccessManagement, string createdByUserId, bool overrideValidation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet DeactivatePowerAgent(string createdByUserId, string OHID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet UpdateAccessManagementForPowerAgent(string createdByUserId, string OHID, bool hasAccessManagement);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet ValidateGlobalAdminChange(string currentOHID, string newOHID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCPCProviderAttestationControls(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUsermembershipInfoByUserName(string username);


        #region Mass Email

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmailTemplate GetTemplateByName(string templateName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        EmailTemplate GetTemplateById(int templateId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddEmailTempalte(string name, string body, string notes = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmailTemplate> GetTAllemplates();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool AddItemToQueue(EmailQueueItem item);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmailQueueItem> GetQueueItemsToProcess(int batchId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int BatchJobCreate(int recordCount, int templateId, string subject);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void BatchJobUpdate(int batchJobId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmailBatch> EmailBatchJobsToProcess();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmailQueueItem> GetEmailsInQueue();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<EmailBatch> GetAllBatchJobs();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLocationTypes(int sectionTypeId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegProviderInfo(int regId, int addressTypeId);

        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseTypeSpecialtyTypeByProviderType(string providertypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseCurrentStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string SelectLicenseTypeIdByAbbrev(string licenseAbbrevation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCLIALabCodesByNumber(string cliaNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCLIACertificateInfoByNumber(string cliaNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTaxInfo(int regid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAdditionalApplications(int RegId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertTaxInfo(int regid, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBoardCertificationTypeByProviderTypeID(int ProviderTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEnrollStatusReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBoardSpecialtyTypeByBoardCertificationID(int BoardCertificationID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAllAgentsByProviderAdmin(string UserId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAgentRolesByProviderAdmin(string UserId, int RegID, int Offset, int PageSize, string UserName, bool isCostRptMgmtAgent);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAgentRolesByProviderAdminCount(string UserId, int RegID, string AgentUserName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAgentFacilities_ContractsByCEOUser(string CEOUserID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SavePendingAgentsByProviderAdmin(string AgentUserID, string ProvAdminUserID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteProviderAgentMapping(int RegId, string ProvAdminID, Guid AgentUserID);

        [ServiceKnownType(typeof(DataTable))]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveAgentRolesByProviderAdmin(Dictionary<string, object> parms);

        [ServiceKnownType(typeof(DataTable))]
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveSecondaryUserFacility_ContractByCEOUser(Dictionary<string, object> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertNewProviderAgent(string AgentUserID, string ProvAdminUserID, int RegID, bool IsActive, string ChangedBy);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckAgentRolesByProviderAdmin(string UserId, int RegID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPublicProviderLblDataByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPublicProviderLblLocDataByRegID(int regId, int regAddressId = 0);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHearingStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTerminationReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReasonBackgroundCheckNotRequired();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospiceApplicationActionType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospiceBenifitSegmentIndicatorType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospiceReasonUpdateType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospicePayerType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectHospiceDocumentType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCounty(string state);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectState();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetICDDiagnosis(string code, string icdVersion, string diagnosisDes);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyBackgroundCheckWithoutPrints(string subject, string owner, string file, string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegOwnerByOwnerID(int OwnerID);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetContractMaintenance();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet VerifyZipCodeIsOH(string zipCode);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetChopTypes();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetContractNames(int RegId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyRiskLevelBumpUp(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid());
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertProviderDisenrollment(int regId, string providerDisenrollmentId, DateTime disenrollDate, string modifiedBy, bool isInsert = true);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderDisenrollmentxref();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderDisenrollment(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCurrentAndPreviousApplicationsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionUploadControlByRegIDAndPageSection(int regId, string regPageSection);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckCDSNumberSectionRequired(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckIfDODDInitialApplication(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckIfODAInitialApplication(int regID);

        [OperationContract]     //akash
        [FaultContract(typeof(ServiceException))]
        DataSet GetLTCReviewTypes();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetInquireHospiceResponse(string hospiceTrackNo, string providerMedId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchHospiceResponse(long hospiceTrackNo, string providerMedId, string reciefId, string providerNpi, int offset, int count);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void NotifyProviderTempPassword(string recipients, string USER_NAME, string LOGIN_URL, string tempPswd, int regId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPendingAgentsByProviderAdmin(string UserId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderByProviderAdmin(string UserId, string eMail);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRSLFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, string comment, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertUpdateODHFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, Guid modifiedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool IsRiskLevelAssigned(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialRisk(int CredentialingID, int riskID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectCredentialingResult();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectCredentialingComments(int regID, int credentialingId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SetUserActiveStatus(Guid agentuserId, Guid adminuserId, bool status, int regID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectIncidentCaseDetails(string casenumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet InsertIncidentFlags(int regId, bool POCBYMAIL, string lastmodifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet UpdateIncidentData(string caseNumber, bool NODNeeded, DateTime NODIssuedDate, string csNotes, string Incident_id, string lastmodifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePOCMail(int regId, bool pocBymail, string lastmodifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetChopTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetOwnerTitle();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAffliationType();
        //AP
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectServiceLocationByMedicaidID(string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectServiceLocationByRegID(string regID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceCodeType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceTypeCode(); //Ap 3/1/2022

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceCodeTypeForPriorAuth();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProcedureCodeType();



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPricingFormula();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCredentialActivityUrl(int activityTypeId, int IsIndividual);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetReviewReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRestrictedServiceStatuses();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetIncludeExclude();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteRegRestriction(int regRestrictionID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRestrictedServicesHistory(int regId);

        //Akta Package 5 start
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAssignements();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAssignementByAuth(string priorAuthType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDestinationPayer();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimDestinationPayer();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAuthorization();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPlanName();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetManagedcareplan();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSpecialIndicator();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDiagnosisCodeType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDiagnoisCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetICDCode();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAccidentcounty();

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetServiceCodeType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorPlacement();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorNewPlacement();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorDocumentType();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetStatusByDetails(string providerNPI, string medicalID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetStatus();

        //  MQsearch --  AP

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet LoadQueueNames();

        //  SUBMITCLaim --  AP


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorClaimsDocumentType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSequenceType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDischargeStatus();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAdmitSource();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetEPSDTCondition();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAdmissionType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAccidentrelatedto();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAccidentState();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimStatus();




        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetReason();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetOtherPhysician();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPatientRelationship();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimFilingIndicator();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPayerSequence();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPresentAddimission();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]


        DataSet GetUnitsOfMeasure();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSubmiclaimtProviderType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]


        DataSet GetCorrespondenceType(); //OHPNM-3814

        [OperationContract]
        [FaultContract(typeof(ServiceException))]


        CorrespodenceInfo GetSearchCorrespondenceType(int correspondeceType, Guid userId, DateTime? fromDate, DateTime? toDate, int regId, string npi, string medicareId, string currentSortOrder, string currentSortField, int pageSize, int pageIndex);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]

        DataSet GetAdjustmentGroup();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthHospital(int PriorAuthHospitalID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthHospital(string tableName, Dictionary<string, string> parms);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertUpdatePriorAuthPanelData(string tableName, Dictionary<string, string> parms);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorAuthPanelData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthPanelData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthDiagnosis(int PriorAuthDiagnosisID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthDiagnosis(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthServiceDetail(int PriorAuthServiceDetailID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthServiceDetail(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthDentalProsthodontics(int PriorAuthDentalProsthodonticsID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthDentalProsthodontics(string tableName, Dictionary<string, string> parms);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthNote(int PriorAuthNoteID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthNote(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPrioHospitalData(string tableName, int priohospitalId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSubmitClaimData(string tableName, int submitclaimid);


        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet SelectPriorAuthAttachment(int PriorAuthAttachmentID);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthDocumentByMail(string tableName, Dictionary<string, object> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int insertPRIOR_AUTH_PROFFSERVICE_DETAIL(string tableName, Dictionary<string, object> parms);

        //Akta Package 5 End

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCRProviderType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegCostReport(string UserId, int REG_ID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLTCCostReportById(string UserId, int REG_COST_REPORT_ID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectMSPCostReportById(string UserId, int REG_COST_REPORT_ID);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegMSPCostReport(string UserId, int REG_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetFacilityProgrameTypes(string mmisProviderTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCostReportTypes(string mmisProviderTypeId);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegChopPrimaryServiceAddress(Dictionary<string, object> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectVerificationSource(int activityTypeId, int IsIndividual, int includeInactive);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateCredentialActivity(int credentialActivityID, int reg_id, int activityTypeId, int dataRankId, string notes, DateTime? OrigEffDate, DateTime? renewalDate,
           DateTime? expiraationDate, int veriSourId, DateTime? verificationDate, DateTime? lastActionDate, string verifiedBy, DateTime? attestationDate, bool isBoardVerificationRequired, DateTime? maternityLicDate, DateTime? siteAccredDate, bool isMediaCareOutput);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchRetrieveReports(Dictionary<string, object> parms, string reportType, out int totalResultCount);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchRAReports(Dictionary<string, object> parms, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertCommunicationEventHistoryEvent(int communicationEventId, DateTime downloadDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRetrieveReports(int index, bool isDownloaded, DateTime downloadDate, DateTime lastModifiedDate, Guid lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCostReportDocument(int documentId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ChangeStatusDeniedProvider(DateTime changedDate, Guid changedBy, int enrollStatusReason, int enrollStatus, int regID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        List<AttachmentResponse> SendAttachments(List<SendAttachment> sendAttachments, AttachmentMessageHeader messageHeader);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSettelementTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetLetterTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetReportTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPeriodTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void saveFaultCodeException(string faultString, string faultCode, int transactionID, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "");


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void saveSoapResponseCodeException(string ModuleTransactionId, string SITransactionKey, string ResponseCode, string ResponseDetails, string ResponseMessage,
            string ResponseType, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetStagingProviderEnrollmentData(int TransactionID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTransactionIDsByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectTransactionDetailsByTransactionID(int transactionID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPECOSRevalidationData(int reg_id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCountryCodes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchTransactions(string transactionStatus, string regId, string medicaidID, string fromDate, string toDate, string sortBy, int pageSize, int pageNumber, bool asc, string SubscriberFailed, out int totalResultCount);

        // MQSearch--AP
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchMQSearch(out int totalCount, int que, DateTime? fromDate, DateTime? toDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchMQSearchData(out int totalCount, int que, DateTime? fromDate, DateTime? toDate, string RegId, string MedicaidId);

        // GetSearchMQMessage--AP
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSearchMQMessage(int logId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAffiliateSpecialtyExportData(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAffiliateLicenseExportData(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectBuildingMedicaidIDByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegApplicationRecord(int regId, DateTime regCreadtedDate, DateTime lastModifiedDate, Guid lastModifiedUser, int processId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateScreeningStatus_PeriodicWF(int regID, DateTime insertDate, string insertBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetRegSectionUploadControlIDByRegID(int regID, string regPageSection, string title);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRescentTransactionIDByRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet CheckIfActiveCMCProvider(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectSiteVisitDataByRegID(int regId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetHistoricNotes(int regId);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllApplicationStatus(string sourceSystem);
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectAllRegistrationStatus();

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetPriorAuthDocumentMail(string tableName, int hospitalId, int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool CheckFedExclusionsMatch(int regID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool CheckDODDAbuserRegistryMatch(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void CreateContractForSpecialty386(int regId, DateTime startDate, Guid changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckDDFacilityNumberExists(string facilityNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertDelegateUsers(Guid delegateUserID, bool activeDelegate, bool phoneMatch, Guid lastModifiedUser, DateTime lastModifiedDate,
            Guid createdBy, DateTime createdDate, string contactName, string Emailaddr);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectUserIDDelegatesData();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckDelegateUserIDisActive(string userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertDelegateDocument(Guid userID, string fileName, string newFileName, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertBulkAgentDocument(Guid userID, string fileName, string newFileName, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate);



        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool CheckNPPESInactiveMatch(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectCLIACertificateDatesByCLIANumber(int regId, string cliaNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectOfficeInformationData(int regId, int regAddressId, string tableName);

        #region Prior Auth  Service Details

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProcedureCodePopupSearch(string procCode = "", string procCodeDesc = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRevenueCodePopupSearch(string revenueCode = "", string revenueCodeDesc = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetLevelOfCare();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRequestedUnitMeasures();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int CreatePriorAuthServiceDetail(int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId,
            decimal? reqUnitsFee, DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId,
            int? authUnits, int? remainigUnits, decimal? authDollars, DateTime? authTDOS, DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime,
            Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int UpdatePriorAuthServiceDetail(int detailedId, int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId, decimal? reqUnitsFee,
          DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId, int? authUnits, int? remainingUnits, decimal? authDollars, DateTime? authTDOS,
          DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime, Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthServiceDetail();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorAuthServiceDetail(int lineNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int CreatePriorAuthProviderNotes(string notesType = "", string noteDesc = "", string noteStatus = "", string noteReasonCodeDesc = "",
           string revProvDesc = "", string prior_Auth_Type = "", string reasonCode = "", string reasonDesc = "", string status = "", Guid? createdBy = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int UpdatePriorAuthProviderNotes(Int32 noteid, string notesType, string noteDesc, string noteStatus,
          string noteReasonCodeDesc, string revProvDesc, DateTime lastModifiedBy, string prior_Auth_Type, string reasonCode, Guid? createdBy = null);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthProviderNotes(string authType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorAuthProviderNote(int noteid);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //void DeletePriorAuthAttachment(int documentId);

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet SelectPriorAuthAttachment(string priorAuthType);

        #endregion

        #region Dental and Professional

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthDentalServiceDetail();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthProfessionalServiceDetail();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorAuthDentalServiceDetail(int lineNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePriorAuthProfessionalServiceDetail(int lineNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthDentalOralCavity();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthDentalProsthesis();

        //[OperationContract]
        //[FaultContract(typeof(ServiceException))]
        //DataSet GetPriorAuthDentalToothSurface();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPriorAuthDentalToothNumber();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthServiceDetails(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePriorAuthServiceDetails(string authType, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthNotes(Dictionary<string, string> parms);

        #endregion

        #region Prior Auth Saving

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPriorAuthAttachmentSaveID(string priorAuthType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPriorAuthServiceDetailSaveID(string priorAuthType, string medicaidId);

        //OHPNM-5582 //GetPriorAuthDetailSaveID(string priorAuthType, string medicaidId)
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int GetPriorAuthDetailSaveID(int paType, string medicaidId, string trackingNo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int SavePriorAuth(int authType, Dictionary<string, object> parms);

        #endregion
        //OHPNM-5582  //SearchPRIORAUTHTRACKING(string trackingNumber, int statustype, int? payerId)
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchPRIORAUTHTRACKING(string trackingNumber, int statustype, int? payerId);

        //OHPNM-5582 //SearchPriorTrackingSubmitPA(string medicaidId)
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SearchPriorTrackingSubmitPA(string medicaidId, string trackingNo);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectRegSectionStatusCMC(int regId, int workFlowId, string tableName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCMCProviderAttestationControls(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPanelsData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdatePanelsData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPanelsData(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectDentalClaimData(int claimID, string tableName, string claimPanelName = "", int IsPrimary = 0);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetToothSurface();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePanelsData(string tableName, string idColumnName, int id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetToothServiceLine(int id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAccidentUSState();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetInsuranceTypes();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimAdjudicationLevel();
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetFilteredOtherPayerSequence();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderSpecialtySearchResults(string npi, string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetReasonCodesOPP();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet LoadDestinationPayer(bool loadAll = false);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDestinationPayerIds(int destinationPayerMapId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDelayReason();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetOccurreneceCode(string code, string codedesc);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeletePanelsDataWithParams(string tableName, Dictionary<string, string> parms);

        #region Upload Attachment

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimTransactionType();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClaimDocumentTypeByClaimTransactionType(int transType);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUploadAttachmentsByMedicaid(string medicaid, string icn, string pa_number);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCPCAttachmentsDocType();

        #endregion

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetConditionCodeDescription(string ConditionCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetValueCodeDescription(string ValueCode);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void DeleteAttachmentsForMedicaidID(string Medicaid_ID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateRegMedicareEnrollmentStatus(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetMQLastExecutionDT(string MQAppID);

        // [OperationContract]
        // [FaultContract(typeof(ServiceException))]
        // void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
        //string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
        //string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Generate1099WithPayerInfo(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName, string payerName, string payerAddress1, string payerAddress2, string payerCity, string payerState, string payerZip);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool Generate1099(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetNPIOrMedicaidIdByRegId(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Common.DataModels.LearningCategoryDocs[] GetLearningDocs();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        Common.DataModels.LearningDoc GetLearningDoc(string fileId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DenyCPCFromProviderReview(int regID, string CPCProgramYear, int ProcessID, DateTime LastModifiedDate, Guid ModifiedBy, int WorkflowEventTypeID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCPCandCMCdtlsForRegID(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool UserCanAccessReg(string userId, int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        bool CheckRestrictedServicesExistsByRegID(int regId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveRegistrationSectionStatusRTP(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRegAddressHospitalNoByRegIdOrMedicaidId(int regId, string medicaidID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegAddressHospitalNo(int regId, string facilityNumberType, int regAddressId, DateTime lastModifiedDateTime, string lastModifiedUser);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetPayerMCEID(int DestinationPayer);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        int SelectClaimAttachmentTypeId(string typename, string claim);



        //[OperationContract]

        //[FaultContract(typeof(ServiceException))]

        //bool ValidateNPIUniqueness(int regId, string npi, bool isNursingFacility);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertClaimsOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
        string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
        string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string OrgDocumentName, string documentID);


        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetPriorAuthStatusCodes(string priorAuthStatus);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID, int regID);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetPriorAuthDocumentMail(string tableName, int hospitalId, int regId);



        //OHPNM-10741

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        int InsertPAOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,

 string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,

 string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalDocumentName);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        void DeletePriorAuthAttachment(int documentId);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet SelectPriorAuthAttachment(string priorAuthType);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetPriorAuthDentalToothSurface();





        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetIndAmbulanceDropOffCode(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetIndProviderNote(string Claim_ID, string providerNoteID, string claim_type);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetIndDiagCode(string claim_id, string claim_diag_info_id, string claim_type);





        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetClaimsUploadAttachmentsByMedicaid(string medicaid, string claimID, string claimType);




        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetSubCapitaPayerIDs(int destPayerID);

        //DataSet GetClaimsUploadAttachmentsByMedicaid(string medicaid, string claimID);





        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetOccurrenceSpanInfo(string Claims_Occurrence_Code_Span_Information_ID, string Claim_ID);

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetValueCodeData(string Claims_Value_Code_Information_ID, string Claim_ID);

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetConditionCodeData(string Claims_Condition_Code_Information_ID, string Claim_ID);



        //[OperationContract]

        //[FaultContract(typeof(ServiceException))]

        //void UpdateScreeningStatus_PeriodicWF(int regID, DateTime insertDate, string insertBy);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetICDProcCode(string claim_id, string claim_icd_procdeure_code);

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet EditNDCCodeData(string Claims_NDC_Details_Screen_ID, string ClaimId);

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet EditOccurrenceInfoData(string Claims_Occurrence_Information_ID, string Claim_ID);


        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet Get_ToothQuadrantInfoData(string Claim_ID, string Claims_Tooth_and_Surface_Information_ID);



        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet EditOtherPayerAdjustmentInfoServiceDetails(string Other_Payer_Adjustment_Service_Detail_ID, string Claim_ID);


        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet GetOPPAServiceDetail(string ClaimId, string OPPAServiceDetailId);

        //OHPNM-14497
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void ArchiveDocumentToGenericUploadControl(int regID, int documentID, string regPageSection, string wfTaskName, string docUploadPage);

        [OperationContract]

        [FaultContract(typeof(ServiceException))]

        DataSet EditAdditionalProviderInformation(string Claim_ID, string Claims_Additional_Provider_Information_Service_ID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSubCapitaPayerIDsByMCE_ID(string mce_Id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertRegNPIEnrollment(int regId, int processId, string providerEffectiveDate);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested,
                string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId,
                string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalDocumentName, string providerComments);



        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetExistingRegDataForNPI(string NPI, int regId, string mmisProviderTypeID, bool isKeyFieldEditRequest);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void CheckEligibletoConvertFromORP(int regID, out bool isEligible);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPowerAgentInformation(string loggedinUserID, string provAdminUserID, int regID);

        /// <summary>
        /// Get the screening Activity data based on the REG_ID and SCREENING_ACTIVITY_TYPE_ID
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="screeningActivityTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetProviderScreeningActivityByRegIdAndActivityType(int regId, int screeningActivityTypeId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetCHOPHistory(int regId, int pageSize, int pageNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetClosureHistory(int regId, int pageSize, int pageNumber);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int ValidateNPIandMEDidData(int regID, int ProcessID, string currentAction);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypeChangeRequestByOldReg(int regID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectProviderTypeChangeRequestByNewReg(int regID, int ProcessID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void SaveProviderTypeChangeRequest(int oldRegId, int newRegId, int processId, int status, DateTime createDate, Guid createdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        void UpdateProviderTypeChangeStatus(int newRegId, int status, DateTime UpdDate, string UpdBy);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetNpiMedIDEnrollmentSpanActions();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDataFixActions(int regid = 0, string medicaid_id = "");

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetRegIdByMedId(string Medid);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet VerifyCPCKidsAttestationSelected(int regID);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDataFixTablesDetails(string tabletype);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDataFixTableSpecificDetails(string tableName);


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetTableColumnsByTableName(string tableName, string tableOperation);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetUploadAttachmentStatus(string memberid, string providerid, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectReferTocomplianceReasons();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUserIOPRefreshTokenInfoByGuid(Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUserIOPAccessTokenInfoByGuid(Guid userID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertProviderFeedNotes(int regId, int regProviderFeedId, string initiatedBy, string note,
            string personReviewedBy = null, string enrollmentType = null, string finalDisposition = null, int processID = 0);
        
    }
}