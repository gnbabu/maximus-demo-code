using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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
        DataSet GetDashboardStatistics();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroups(int workFlowId);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDashboardStatisticsGroupTotals();

        #endregion

        #region "CoreLibraries"

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetEnvironment();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        string GetUIHeader();

        #endregion

        #region "Person"

        //[OperationContract]
        //[FaultContract(typeof(Exception))]
        //void SubmitPersons(DataSet Persons);

        //[OperationContract]
        //[FaultContract(typeof(Exception))]
        //DataSet GetFilteredPersons(string firstName, string lastName, string providerId
        //    , string medicaidId, string caqhId, string status, object mmisStatus, object errorType);
        #endregion

        #region "Provider"
        [OperationContract]
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
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
            , int sortBy
            , int pageNumber
            , int rowsPerPage
            , bool asc);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchGroupProviders(string groupName, string medicaidId, string npi, string taxId, 
            string pdmsStatus, string emailAddress, string pdmsStatusDate, string tennCareStatus, int sortBy, int pageNumber, 
            int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchGroupProviderWithIds(string registrationIdList, int tableId, int statusId, int ordinal,
            int sortBy, int pageNumber, int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchServiceLocations(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi,
            string taxId, int pageNumber, int rowsPerPage);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchErrorWithIds(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal, int sortBy, 
            int pageNumber, int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchErrors(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi,
            string errorCategoryId, string errorStatusId, string errorTypeId, string hardSoft, string errorDate, int sortBy, int pageNumber, 
            int rowsPerPage, bool asc);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectDEAByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectDocumentsByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectLicenseByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSpecialtyByPartyId(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderFull(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProvider(string taxId, string npi);
        
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void TerminateProviderGroup(string medId, string taxId, string npi);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderNotes(int partyId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderNotesByNoteID(int providerNoteID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderReplicaApps(int partyID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderStandardExtracts(int partyID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPartyErrorHistory(int errorId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool ValidNPI(Int64 npi, string lastName, string firstName, string middleName, string state);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void ValidateServiceLocations(string taxId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void EmailNotify(int errorId, int toPartyId, string toEmail, int resolvingActionTypeId, int resolvingReasonTypeId,
            string terminateEnrollment, DateTime eventDate, string providerName, string npi, string userId, int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void OverrideErrorSubmitRoster(int errorId, int submitRosterId, int resolvingActionTypeId, int resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void OverrideError(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetUpdateRecordOption(int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetContiguousZipcode(string zip);
 
        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateRecord(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK,
            int opt, string newValue, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateProvider(int partyId, string medicaidId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateProviderID(int partyId, string providerID, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateMMISStatus(int partyId, int mmisStatusId, int medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateMMISServiceLocation(int partyId, int serviceAddressPopID, int? medicaidPK, DateTime? terminated, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateError(int errorId, int errorStatusTypeId, int? communicationEventId, int resolvingActionTypeId,
            int? resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateErrorRegistration(int regErrorId, int errorStatusTypeId, int? communicationEventId, int? resolvingActionTypeId,
           int? resolvingReasonTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdatePDMSStatus(int errorId, int partyId, int pdmsStatusId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdatePDMSDataAdmin(int partyId, string providerID,
            string licenseNumber, string licenseState, string licenseEffDate, string licenseEndDate,
            string deaNumber, string deaState, string deaEffDate, string deaEndDate,
            string providerTypeID, string providerSpecialtyTypeID, string baseMedicaidID,
            string emailAddress, string credAddress1, string credAddress2, string credCity, string credState,
            string credZip, string credExtendedZip, string credPhoneAreaCode, string credPhoneNumber,
            string credFaxAreaCode, string credFaxNumber, string licenseTypeID, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdatePDMSDataAdminSvcLoc(int partyId, string serviceLocationID, string medicaidID,
            string servicingAddressName, string servicingAddress1, string servicingAddress2,
            string servicingCity, string servicingState, string servicingZip, string servicingExtendedZip,
            string servicingCounty, string servicingPhoneAreaCode, string servicingPhoneNumber,
            string mailtoAddressName, string mailtoAddress1, string mailtoAddress2,
            string mailtoCity, string mailtoState, string mailtoZip, string mailtoExtendedZip,
            string mailtoPhoneAreaCode, string mailtoPhoneNumber,
            string paytoAddressName, string paytoAddress1, string paytoAddress2,
            string paytoCity, string paytoState, string paytoZip, string paytoExtendedZip,
            string paytoPhoneAreaCode, string paytoPhoneNumber, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdatePDMSDataAdminReg(int regId, string medicaidID, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void TerminateProviderRecord(int partyId, int medicaidPK, string cdeEnrollStatus, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertPartyError(string errorCode, int errorStatusTypeId, int? partyId, int? submitRosterId, int? rosterExceptionId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertProviderNote(int partyId, int enteringUserID, int noteTypeID, string noteText, DateTime noteDate, string userId, int errorID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertDocument(int partyId, string name, string description, string fileName, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserProvGrp_XREF(string userID, string taxID, string npi);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteDocument(int documentID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetStgProviderById(string regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectForProviderFile(string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderContactEmail(int partyId);


        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectConversionProvider(string userId = null, string userName = null, int regId = 0, string taxId = null, string npi = null);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectConversionAffiliation(string regAffiliationId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet Search_NPPES_EntityType(string NPI);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        string GetProviderIdByUserName(string username);
        #endregion

        #region "Return Roster"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetSubmitRosterByNPI(string providerNPI);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISMatch(string providerNPI, string ssn);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISServiceAddressByNPI(string providerNPI);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISServiceAddressByValues(string providerID, string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISServiceValues(string providerID, string npi, string ssn);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetServiceAddresses(int submitRosterId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveMMISData(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveMMISDataOptional(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SavePartyStatus(int partyId, int providerStatusChangeTypeId, int pdmsStatusTypeId, int? medicaidPK, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SubmitReturnRosters(DataSet ReturnRosters);

        #endregion

        #region "State"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetStates();

        #endregion

        #region "Lookup Tables"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetApplicationTypes();

        [FaultContract(typeof(Exception))]
        [OperationContract]
        DataSet GetCAQHErrors();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetCAQHStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetContentTypes(int contentTypeID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISErrors();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetProviderTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetProviderTypes2();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetProviderCategories();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetProviderCategoriesPhaseII();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPDMSStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPDMSErrors();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetEnrollmentRejectReasons();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetResolvingActionTypes(bool documentsOnly, int partyId, int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetResolvingReasonTypes(int resolvingActionTypeId, int errorTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetMMISEnrollmentStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetErrorTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetErrorResolvingActions(int errorTypeID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetErrorStatusTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetErrorCategoryTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetErrorDispositions();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetProviderNoteTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPermissionStatuses();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetPermissionApplications();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetSecurityQuestions();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSpecialtyTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectSpecialtiesByProviderType(int providerTypeID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectTaxonomyTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectAllLicenseTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectTaxEntityTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectAllTransactionTypes();

        //NE Demo
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectProviderRiskLevels();

        //NE Demo
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectReportDocumentTypes();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectLicenseRestrictionCodes();

        #endregion

        #region "Submit Roster"
       
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet CheckSubmitRoster(string providerNPI, string ssn);
 
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetEmptySubmitRoster();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetEmptyServiceAddress();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetSubmitRosterById(int SubmitRosterId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void AddSubmitRosterExisting(DataSet ds);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int SubmitSubmitRosters(DataSet SubmitRosters, int pdmsStatusId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SubmitServiceAddress(int submitRosterId, DataSet ds);

        [OperationContract]
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
         void InsertDEA(
                                DateTime? EffectiveDateTime,
                                DateTime? EndDateTime,
                                int XREF_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
         void InsertLicense(
                                   int? License,
                                   string StateCode,
                                   string TypeCode,
                                   DateTime? EffectiveDateTime,
                                   DateTime? EndDateTime,
                                   int XREF_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
         void InsertPhone(
                            int PhoneType_ID,
                            string Number,
                            int XFER_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
         void InsertImportedFolder(
                                    string FolderName,
                                    DateTime ImportDate);

        [OperationContract]
        [FaultContract(typeof(Exception))]
         List<string> GetImportedFolderNames();

        [OperationContract]
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
         void InsertSubmitProviders_Response(
                                        string ExtensionData,
                                        string CorrelationId,
                                        string Errors,
                                        string TransactionId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
         void InsertRetrieveProviders_Request(string Sak_Trans);

        [OperationContract]
        [FaultContract(typeof(Exception))]
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
        [FaultContract(typeof(Exception))]
         Dictionary<string, int> SelectAddressType();

        [OperationContract]
        [FaultContract(typeof(Exception))]
         Dictionary<string, int> SelectPhoneType();

        [OperationContract]
        [FaultContract(typeof(Exception))]
         Dictionary<string, int> SelectTransmissionType();
        #endregion

        #region "Interfaces - Staging"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetAppSetting(string key);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetAddressStagingTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SubmitStagingProviders(DataSet StagingProviders);
        
        #endregion

        #region "Roster Exceptions"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetRosterExceptions();

        #endregion

        #region "Registration"

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int InsertRegistration(string formCompletionName, string formCompletionPhone, int registrationStatusTypeId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int InsertRegistrationForConversion(string regId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateRegistration(Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveRegistrationProgramStatus(int regId, int regProgramStatusTypeId, DateTime? termDate, string enrollmentStatusCode, 
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void RemoveReceiveACHData(int regId, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void RemoveQuestionData(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectQuestionType(string questionTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistration(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SearchRegistration(string taxId, string npi, string ssn, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistrationData(int regId, string tableName);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistrationByUserId(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegDocuments(int regId, int regPageTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegNotes(int regId, int regPageTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegErrorTypes(int regPageTypeId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegSubcontractors(int regId, int subcontractorTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int InsertRegistrationDataTable(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateRegistrationDataTable(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms);

        // Overloaded methods - use the Insert/Update methods above if possible
        [OperationContract]
        [FaultContract(typeof(Exception))]
        int InsertRegistrationData(int regId, string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertRegDocument(int regId, int regPageTypeId, string name, string description, string fileName, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertRegProviderNote(int regId, int regPageTypeId, int enteringPartyID, int noteTypeID, string noteText, DateTime noteDate, int stepId, string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int UpdateRegistrationData(int regId, string tableName, Dictionary<string, string> parms);
        // End Overloaded methods

        [OperationContract]
        [FaultContract(typeof(Exception))]
        string GetProviderTypeByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveRegistrationPageStatus(int regId, int regPageTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId,
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveRegistrationQuestion(int regId, string questionTypeId, int response, int modifiedStatusTypeId,
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void SaveRegistrationOwnerXref(int regId, int regOwner1Id, int regOwner2Id, int relationshipTypeId, int modifiedStatusTypeId,
            string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRelationshipTypes();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectREG_ERRORcustom(int regId, int pageTypeId, bool includeClosed, bool includePartyErrors);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserWorkflowPermission(string USERID, int WORKFLOW_ID, string LAST_MODIFIED_USER);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteUserWorkflowPermission(string USERID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectUserWorkflowPermission(string USERID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet TransferPDMStoRegistration(int partyId, string userId, DateTime requestedEffectiveDate, DateTime changeEffectiveDate, 
            string formCompletionName, string formCompletionPhone);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetGroupNames(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetGroupAffiliationHistory(int regId, int pageSize, int pageNumber);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetWFProcessByRegId(int regId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool RegistrationNeverSubmitted(int regId);


        #endregion

        #region "User Accounts"
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetFilteredUsers(string username, string contactname, string orgName, string role, int pageSize, int pageNumber);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteUserApplicationType(string userId, int applicationTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteUserProviderType(string userId, int providerTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void DeleteUserStatusType(string userId, int statusTypeId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserSecurityQuestions(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetExistingProviders(string taxId, string NPI);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserApplicationTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserProviderTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserStatusTypes(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserSecurityQuestionsByNameEmail(string userName, string email);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUsernameForProvider(string email, string taxId, string NPI,
            string medicaidId, string zip);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetUserAccountInformation(string userId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertPartyUserXref(int partyId, string userId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt,
            string medId, int provCat, int provType, string taxId, string npi, string groupName, int zip,
            DateTime? changedDate, string changedBy, int? regId = null);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserApplicationType(string userId, int applicationTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserProviderType(string userId, int providerTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserStatusType(string userId, int statusTypeId, DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void InsertUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        Boolean IsAnswerCorrect(string userName, int questionNumber, string answer);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void NotifyProviderAccountCreation(string recipients, string LEGAL_BUSINESS_NAME, string NPI, string USER_NAME, string LOGIN_URL, int regId = 0, int partyId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void NotifyPasswordReset(string recipients, string username, string pdmsUrl, int regId = 0, int partyId = 0, Guid userid = new Guid());

        [OperationContract]
        [FaultContract(typeof(Exception))]
        bool NotifyUserName(string email, string taxID, string npi, string mcaid, string zip);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt,
            DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateUserSecurityQuestions(string userId, int? question1Id, string answer1,
            int? question2Id, string answer2, int? question3Id, string answer3,
            DateTime? changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectActiveUsersByRole(string role);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectUsersInRoles(string roles);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectEmailNotifications(Guid userId, int regId, int partyId, int pageSize, int pageNumber, string columnName = null, string sortDirection = null);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetEmailAttachments(string COMMUNICATION_EVENT_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistrationStatuses(string UserId, int REG_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectRegistrationXREF(int REG_ID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void TransferRegistrationToNewUser(int REG_ID, string toUserId);
        #endregion

        #region PDMS Workflow

        [OperationContract]
        [FaultContract(typeof(Exception))]
        string WF_ProcessTask(int processID, string assemblyName, string className);
 
        [OperationContract]
        [FaultContract(typeof(Exception))]
        List<string> WF_ProcessAllTasks();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int WF_StartStep(int processID, string ownerID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void WF_CancelWorkflowProcess(int processID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectStepActions(int stepID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectStepInfo(int stepID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectProcess(int processID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectProcessParameters(int processID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectStepParameters(int stepID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void WF_SaveProcessParameter(int processID, string parameterName, string parameterValue);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void WF_SaveStepParameter(int stepID, string parameterName, string parameterValue);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void WF_TakeAction(int processID, string action, string notes);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectActiveOwnerSteps(string ownerID, bool includeOtherSteps);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectActiveOwnerStepsProvider(string ownerID, bool includeOtherSteps);
 
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectUnassignedSteps(string groupName);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectRegistrationWorkflows(string ownerID);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet WF_SelectWorkflows();

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void updateWF_STEP_Owner(int stepId, string ownerId);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        int GetWorkflowInstance(int? applicationTypeID, int? providerCategoryTypeID, int? providerTypeID, int? referralTypeID,
            bool revalDue, bool referral, bool conversion, bool groupMember);
        #endregion

        #region Admin
        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectTransactions(string TransactionTypeId = null, string ProcessStartDate = null, string ProcessEndDate = null, string PartyID = null, string ServiceLocationID = null, string MedicaidID = null, string CAQHId = null, string OrderBy = null, string SortOrder = null);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void CancelTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void ResubmitProcessedTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser);

        #endregion

		#region Screening

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectGroupProviderScreeningData(int regID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectIndividualProviderScreeningData(int regAffiliationID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectAffiliationsScreeningData(int regID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectOwnersScreeningData(int regID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectProviderScreeningActivityData(int screeningID);


		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectProviderScreeningActivityMatchData(int screeningActivityID, int regID, int regAffiliationID, int regOwnerID, string tableName);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectScreeningActivityDocuments(int screeningActivityID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		void InsertScreeningActivityDocument(int screeningActivityID, string name, string description, string fileName, string userId);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		void InsertProviderScreening(int regID, string changedBy);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		void UpdateScreeningActivityStatus(int screeningActivityID, int screeningActivityStatusID, string adverseAction, string changedBy);

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
        void UpdateScreeningStatus(int screeningID, int screeningStatusID, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void UpdateScreeningResult(int screeningID, int screeningResultID, string changedBy);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectSiteVisitScreeningData(int regID);

		[OperationContract]
		[FaultContract(typeof(Exception))]
		DataSet SelectSiteVisitData(int siteVisitScreeningID);

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
		void InsertSiteVisitAttempt(int siteVisitID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments, DateTime changedDate, string changedBy);

        [OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet SelectPendingSiteVisits();
		
		//Akta Package 5 Start
		 [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAssignement();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetAuthorization();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetPlanName();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetSpecialIndicator();

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetDiagnosisCodeType();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet GetServiceCodeType();

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
        DataSet GetStatus();


        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthHospital(int PriorAuthHospitalID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthHospital(string tableName, Dictionary<string, string> parms);

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
        DataSet SelectPriorAuthAttachment(int PriorAuthAttachmentID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        int InsertPriorAuthDocumentByMail(string tableName, Dictionary<string, string> parms);
		//Akta Package 5 End

        #endregion

        #region Action History

        [OperationContract]
        [FaultContract(typeof(Exception))]
        void LogReportDownload(string UserId, string DocumentId, string ProviderId, string DocumentFormat, int REPORT_DOCUMENT_TYPE_ID);
        #endregion
		
		[OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetCHOPHistory(int regId, int pageSize, int pageNumber);
		
		
		[OperationContract]
        [FaultContract(typeof(Exception))]
        DataSet GetClosureHistory(int regId, int pageSize, int pageNumber);
		
    }
}