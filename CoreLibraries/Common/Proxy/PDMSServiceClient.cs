using Corp.Core.Libraries;
using Corp.Core.Libraries.AttachmentServiceReference;
using Corp.Core.Libraries.ProviderManagementReference;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using System.ServiceModel;
using AttachmentMessageHeader = Corp.Core.Libraries.AttachmentServiceReference.MessageHeader;

namespace PDMSService
{
    public class PDMSServiceClient : ClientBase<IPDMSService>, IPDMSService
    {
        public void TestException()
        {
            base.Channel.TestException();
        }

        public System.Data.DataSet GetDashboardStatistics()
        {
            return base.Channel.GetDashboardStatistics();
        }

        public System.Data.DataSet GetDashboardStatisticsGroups(int workFlowId, int providerCategoryTypeId, int workflowEventTypeId, int applicationTypeId = 0, int waiverTypeId = 0, int dashboardTableId = 0)
        {
            return base.Channel.GetDashboardStatisticsGroups(workFlowId, providerCategoryTypeId, workflowEventTypeId, applicationTypeId, waiverTypeId, dashboardTableId);
        }

        public System.Data.DataSet GetDashboardStatisticsGroups_Services(int workFlowId, int providerCategoryTypeId)
        {
            return base.Channel.GetDashboardStatisticsGroups_Services(workFlowId, providerCategoryTypeId);
        }

        public DataSet GetMMISTrackingDetails()
        {
            return base.Channel.GetMMISTrackingDetails();
        }
        public DataSet GetMSPDuedateDetails()
        {
            return base.Channel.GetMSPDuedateDetails();
        }

        public DataSet SearchCostReportDetails()
        {
            return base.Channel.SearchCostReportDetails();
        }
        public DataSet MSPSearchCostReportDetails()
        {
            return base.Channel.MSPSearchCostReportDetails();
        }
        public DataSet HospitalSearchCostReportDetails()
        {
            return base.Channel.HospitalSearchCostReportDetails();
        }

        public DataSet GetContractNames(int regId)
        {
            return base.Channel.GetContractNames(regId);
        }

        public System.Data.DataSet GetDashboardStatisticsGroups_forGroupMemberProfile()
        {
            return base.Channel.GetDashboardStatisticsGroups_forGroupMemberProfile();
        }

        public System.Data.DataSet GetDashboardProviderSummary(string roleName)
        {
            return base.Channel.GetDashboardProviderSummary(roleName);
        }

        public System.Data.DataSet GetDashboardStatisticsGroupTotals(int providerCategoryTypeId)
        {
            return base.Channel.GetDashboardStatisticsGroupTotals(providerCategoryTypeId);
        }
        public int GetElapsedTimeLimitByApplicationType(int applicationTypeId)
        {
            return base.Channel.GetElapsedTimeLimitByApplicationType(applicationTypeId);
        }

        public int GetElapsedDaysByApplicationType(int applicationTypeId)
        {
            return base.Channel.GetElapsedDaysByApplicationType(applicationTypeId);
        }

        public string GetEnvironment()
        {
            return base.Channel.GetEnvironment();
        }

        //public DataSet GetChopTypes()
        //{
        //    return base.Channel.GetChopTypes();
        //}


        //public DataSet GetContractMaintenance()
        //{
        //    return base.Channel.GetContractMaintenance();
        //}

        public DataSet GetContractMaintenanceById(int contractMaintenanceId)
        {
            return base.Channel.GetContractMaintenanceById(contractMaintenanceId);
        }



        public string GetUIHeader()
        {
            return base.Channel.GetUIHeader();
        }

        public System.Data.DataSet SearchProviderWithIds(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal, int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            return base.Channel.SearchProviderWithIds(IdList, submitRosterIdList, tableId, statusID, ordinal, sortBy, pageNumber, rowsPerPage, asc);
        }

        public System.Data.DataSet SearchProviders(string firstName, string lastName, string groupName, string providerId, string medicaidId, string caqhId, string npi, string taxId, string pdmsStatusId, string caqhStatusId, string emailAddress, string pdmsStatusDate, int riskLevelId, int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            return base.Channel.SearchProviders(firstName, lastName, groupName, providerId, medicaidId, caqhId, npi, taxId, pdmsStatusId, caqhStatusId, emailAddress, pdmsStatusDate, riskLevelId, sortBy, pageNumber, rowsPerPage, asc);
        }

        public System.Data.DataSet SelectProvider_NPI_TaxID(string npi, string taxId)
        {
            return base.Channel.SelectProvider_NPI_TaxID(npi, taxId);
        }

        public System.Data.DataSet SearchGroupProviders(Dictionary<string, object> parms, out int totalResultCount)
        {
            return base.Channel.SearchGroupProviders(parms, out totalResultCount);
        }

        //public DataSet SelectRegProvidersByIdList(string providerIdList)
        //{
        //	return base.Channel.SelectRegProvidersByIdList(providerIdList);
        //}


        public System.Data.DataSet SearchOwnerProviders(string groupName, string firstName, string lastName, string taxId,
            string roleName, string IsUserAssigned, string userName, string sortColumn, int pageSize, int startRowIndex,
            bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SearchOwnerProviders(groupName, firstName, lastName, taxId,
                    roleName, IsUserAssigned, userName, sortColumn, pageSize,
                    startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public DataSet SearchProvidersPublic(string providerTypeAbbrevList, string specialtyTypeIDList, string lastName, string firstName, string middleInitial,
            string city, string state, string zip, string quadrant, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount,
            out int totalResultCount)
        {
            return base.Channel.SearchProvidersPublic(providerTypeAbbrevList, specialtyTypeIDList, lastName, firstName, middleInitial, city, state, zip, quadrant,
                sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }




        public DataSet SearchProvidersPublicNew(Dictionary<string, object> parms, out int totalResultCount)
        {
            return base.Channel.SearchProvidersPublicNew(parms, out totalResultCount);
        }

        public DataSet SearchProvidersGIS(string providerTypeAbbr, int pageSize)
        {
            return base.Channel.SearchProvidersGIS(providerTypeAbbr, pageSize);
        }

        public System.Data.DataSet SearchGroupProviderWithIds(string registrationIdList, int tableId, int statusId, int ordinal, bool isAssigned,
            string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SearchGroupProviderWithIds(registrationIdList, tableId, statusId, ordinal, isAssigned,
                sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SearchServiceLocations(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi, string taxId, int pageNumber, int rowsPerPage)
        {
            return base.Channel.SearchServiceLocations(firstName, lastName, providerId, medicaidId, caqhId, npi, taxId, pageNumber, rowsPerPage);
        }

        public System.Data.DataSet SearchErrorWithIds(string IdList, string submitRosterIdList, int tableId, int statusID, int ordinal, int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            return base.Channel.SearchErrorWithIds(IdList, submitRosterIdList, tableId, statusID, ordinal, sortBy, pageNumber, rowsPerPage, asc);
        }

        public System.Data.DataSet SearchErrors(string firstName, string lastName, string providerId, string medicaidId, string caqhId, string npi, string errorCategoryId, string errorStatusId, string errorTypeId, string hardSoft, string errorDate, int sortBy, int pageNumber, int rowsPerPage, bool asc)
        {
            return base.Channel.SearchErrors(firstName, lastName, providerId, medicaidId, caqhId, npi, errorCategoryId, errorStatusId, errorTypeId, hardSoft, errorDate, sortBy, pageNumber, rowsPerPage, asc);
        }

        public System.Data.DataSet SelectDEAByPartyId(int partyId)
        {
            return base.Channel.SelectDEAByPartyId(partyId);
        }

        public System.Data.DataSet SelectDIDDReferralTypes()
        {
            return base.Channel.SelectDIDDReferralTypes();
        }

        public System.Data.DataSet SelectDocumentsByPartyId(int partyId)
        {
            return base.Channel.SelectDocumentsByPartyId(partyId);
        }

        public System.Data.DataSet SelectLicenseByPartyId(int partyId)
        {
            return base.Channel.SelectLicenseByPartyId(partyId);
        }

        public System.Data.DataSet SelectSpecialtyByPartyId(int partyId)
        {
            return base.Channel.SelectSpecialtyByPartyId(partyId);
        }

        public System.Data.DataSet SelectProviderFull(int partyId)
        {
            return base.Channel.SelectProviderFull(partyId);
        }

        public System.Data.DataSet GetMatchingProviderByTaxNpi(string taxId, string npi, int specialtyTypeID)
        {
            return base.Channel.GetMatchingProviderByTaxNpi(taxId, npi, specialtyTypeID);
        }

        public System.Data.DataSet GetMatchingProviderByNPI(string npi, bool includeWaiverServices)
        {
            return base.Channel.GetMatchingProviderByNPI(npi, includeWaiverServices);
        }

        public System.Data.DataSet GetMatchingProvider(string NPI, string MedicaidID, string DD_Facility_num, string DD_Contract_num)
        {
            return base.Channel.GetMatchingProvider(NPI, MedicaidID, DD_Facility_num, DD_Contract_num);
        }

        public System.Data.DataSet GetAvailableProvider(string NPI, string MedicaidID, string TaxID, string DD_Facility_num, string DD_Contract_num)
        {
            return base.Channel.GetAvailableProvider(NPI, MedicaidID, TaxID, DD_Facility_num, DD_Contract_num);
        }

        public System.Data.DataSet GetMatchingProviderByTaxID(string taxID, bool includeWaiverServices)
        {
            return base.Channel.GetMatchingProviderByTaxID(taxID, includeWaiverServices);
        }

        public void TerminateProviderGroup(string medId, string taxId, string npi)
        {
            base.Channel.TerminateProviderGroup(medId, taxId, npi);
        }

        public System.Data.DataSet SelectProviderNotes(int RegID)
        {
            return base.Channel.SelectProviderNotes(RegID);
        }

        public System.Data.DataSet SelectProviderNotesByNoteID(int providerNoteID)
        {
            return base.Channel.SelectProviderNotesByNoteID(providerNoteID);
        }

        public System.Data.DataSet SelectProviderReplicaApps(int partyID)
        {
            return base.Channel.SelectProviderReplicaApps(partyID);
        }

        public System.Data.DataSet SelectProviderStandardExtracts(int partyID)
        {
            return base.Channel.SelectProviderStandardExtracts(partyID);
        }

        public System.Data.DataSet GetPartyErrorHistory(int errorId)
        {
            return base.Channel.GetPartyErrorHistory(errorId);
        }

        public bool ValidNPI(long npi, string lastName, string firstName, string middleName, string state)
        {
            return base.Channel.ValidNPI(npi, lastName, firstName, middleName, state);
        }

        public DataSet GetFacilityTypes(string facilityCode, string facilityDesc)
        {
            return base.Channel.GetFacilityTypes(facilityCode, facilityDesc);
        }
        public DataSet GetFacilityTypeByFacilityCode(string facilityCode, string facilityDesc)
        {
            return base.Channel.GetFacilityTypeByFacilityCode(facilityCode, facilityDesc);
        }
        public DataSet GetPayerNames()
        {
            return base.Channel.GetPayerNames();
        }

        public bool VerifyProviderNPI(long npi)
        {
            return base.Channel.VerifyProviderNPI(npi);
        }
        public DataSet SearchProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            return base.Channel.SearchProviderNPI(npi, medicaidID, lastName, firstName);
        }

        public DataSet SearchHospiceProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            return base.Channel.SearchHospiceProviderNPI(npi, medicaidID, lastName, firstName);
        }

        public DataSet SearchClaimProviderNPI(string npi, string medicaidID, string lastName, string firstName)
        {
            return base.Channel.SearchClaimProviderNPI(npi, medicaidID, lastName, firstName);
        }

        public void ValidateServiceLocations(string taxId, string userId)
        {
            base.Channel.ValidateServiceLocations(taxId, userId);
        }

        public void UpdatePNMApplicationStatus(int regID, int appStatus, Guid userID, int processID)
        {
            base.Channel.UpdatePNMApplicationStatus(regID, appStatus, userID, processID);
        }

        public void EmailNotify(int errorId, int toPartyId, string toEmail, int resolvingActionTypeId, int resolvingReasonTypeId, string terminateEnrollment, DateTime eventDate, string providerName, string npi, string userId, int regId)
        {
            base.Channel.EmailNotify(errorId, toPartyId, toEmail, resolvingActionTypeId, resolvingReasonTypeId, terminateEnrollment, eventDate, providerName, npi, userId, regId);
        }

        public void OverrideErrorSubmitRoster(int errorId, int submitRosterId, int resolvingActionTypeId, int resolvingReasonTypeId, string userId)
        {
            base.Channel.OverrideErrorSubmitRoster(errorId, submitRosterId, resolvingActionTypeId, resolvingReasonTypeId, userId);
        }

        public void OverrideError(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK, string userId)
        {
            base.Channel.OverrideError(errorId, partyId, resolvingActionTypeId, resolvingReasonTypeId, medicaidPK, userId);
        }

        public int IsUserInSubRole(int regid, string userId, string roleName)
        {
            return base.Channel.IsUserInSubRole(regid, userId, roleName);
        }

        public int GetUpdateRecordOption(int errorTypeId)
        {
            return base.Channel.GetUpdateRecordOption(errorTypeId);
        }

        public System.Data.DataSet GetContiguousZipcode(string zip)
        {
            return base.Channel.GetContiguousZipcode(zip);
        }

        public System.Data.DataSet GetCountiesList()
        {
            return base.Channel.GetCountiesList();
        }

        public void UpdateRecord(int errorId, int partyId, int resolvingActionTypeId, int resolvingReasonTypeId, int medicaidPK, int opt, string newValue, string userId)
        {
            base.Channel.UpdateRecord(errorId, partyId, resolvingActionTypeId, resolvingReasonTypeId, medicaidPK, opt, newValue, userId);
        }

        public void UpdateProvider(int partyId, string medicaidId, string userId)
        {
            base.Channel.UpdateProvider(partyId, medicaidId, userId);
        }

        public void UpdateProviderID(int partyId, string providerID, string userId)
        {
            base.Channel.UpdateProviderID(partyId, providerID, userId);
        }

        public void UpdateMMISStatus(int partyId, int mmisStatusId, int medicaidPK, string userId)
        {
            base.Channel.UpdateMMISStatus(partyId, mmisStatusId, medicaidPK, userId);
        }

        public void UpdateMMISServiceLocation(int partyId, int serviceAddressPopID, int? medicaidPK, DateTime? terminated, string userId)
        {
            base.Channel.UpdateMMISServiceLocation(partyId, serviceAddressPopID, medicaidPK, terminated, userId);
        }

        public void UpdateError(int errorId, int errorStatusTypeId, int? communicationEventId, int resolvingActionTypeId, int? resolvingReasonTypeId, string userId)
        {
            base.Channel.UpdateError(errorId, errorStatusTypeId, communicationEventId, resolvingActionTypeId, resolvingReasonTypeId, userId);
        }

        public void UpdateErrorRegistration(int regErrorId, int errorStatusTypeId, int? communicationEventId, int? resolvingActionTypeId, int? resolvingReasonTypeId, string userId)
        {
            base.Channel.UpdateErrorRegistration(regErrorId, errorStatusTypeId, communicationEventId, resolvingActionTypeId, resolvingReasonTypeId, userId);
        }

        public void UpdatePDMSStatus(int errorId, int partyId, int pdmsStatusId, string userId)
        {
            base.Channel.UpdatePDMSStatus(errorId, partyId, pdmsStatusId, userId);
        }

        public void UpdatePDMSDataAdmin(int partyId, string providerID, string licenseNumber, string licenseState, string licenseEffDate, string licenseEndDate, string deaNumber, string deaState, string deaEffDate, string deaEndDate, string providerTypeID, string providerSpecialtyTypeID, string baseMedicaidID, string emailAddress, string credAddress1, string credAddress2, string credCity, string credState, string credZip, string credExtendedZip, string credPhoneAreaCode, string credPhoneNumber, string credFaxAreaCode, string credFaxNumber, string licenseTypeID, string userId)
        {
            base.Channel.UpdatePDMSDataAdmin(partyId, providerID, licenseNumber, licenseState, licenseEndDate, licenseEndDate, deaNumber, deaState, deaEffDate, deaEndDate, providerTypeID, providerSpecialtyTypeID, baseMedicaidID, emailAddress, credAddress1, credAddress2, credCity, credState, credZip, credExtendedZip, credPhoneAreaCode, credPhoneNumber, credFaxAreaCode, credFaxNumber, licenseTypeID, userId);
        }

        public void UpdatePDMSDataAdminSvcLoc(int partyId, string serviceLocationID, string medicaidID, string servicingAddressName, string servicingAddress1, string servicingAddress2, string servicingCity, string servicingState, string servicingZip, string servicingExtendedZip, string servicingCounty, string servicingPhoneAreaCode, string servicingPhoneNumber, string servicingEmailAddress, string mailtoAddressName, string mailtoAddress1, string mailtoAddress2, string mailtoCity, string mailtoState, string mailtoZip, string mailtoExtendedZip, string mailtoPhoneAreaCode, string mailtoPhoneNumber, string mailtoEmailAddress, string paytoAddressName, string paytoAddress1, string paytoAddress2, string paytoCity, string paytoState, string paytoZip, string paytoExtendedZip, string paytoPhoneAreaCode, string paytoPhoneNumber, string paytoEmailAddress, string userId)
        {
            base.Channel.UpdatePDMSDataAdminSvcLoc(partyId, serviceLocationID, medicaidID, servicingAddressName, servicingAddress1, servicingAddress2, servicingCity, servicingState, servicingZip, servicingExtendedZip, servicingCounty, servicingPhoneAreaCode, servicingPhoneNumber, servicingEmailAddress, mailtoAddressName, mailtoAddress1, mailtoAddress2, mailtoCity, mailtoState, mailtoZip, mailtoExtendedZip, mailtoPhoneAreaCode, mailtoPhoneNumber, mailtoEmailAddress, paytoAddressName, paytoAddress1, paytoAddress2, paytoCity, paytoState, paytoZip, paytoExtendedZip, paytoPhoneAreaCode, paytoPhoneNumber, paytoEmailAddress, userId);
        }

        public void UpdatePDMSDataAdminReg(int regId, string medicaidID, string userId)
        {
            base.Channel.UpdatePDMSDataAdminReg(regId, medicaidID, userId);
        }

        public void TerminateProviderRecord(int partyId, int medicaidPK, string medicaidId, string cdeEnrollStatus, string userId, bool referralProvider)
        {
            base.Channel.TerminateProviderRecord(partyId, medicaidPK, medicaidId, cdeEnrollStatus, userId, referralProvider);
        }

        public void InsertPartyError(string errorCode, int errorStatusTypeId, int? partyId, int? submitRosterId, int? rosterExceptionId, string userId)
        {
            base.Channel.InsertPartyError(errorCode, errorStatusTypeId, partyId, submitRosterId, rosterExceptionId, userId);
        }

        public void InsertProviderNote(int regID, int noteTypeID, string noteText, DateTime noteDate, string userId, int errorID)
        {
            base.Channel.InsertProviderNote(regID, noteTypeID, noteText, noteDate, userId, errorID);
        }

        public void InsertDocument(int partyId, string name, string description, string fileName, string userId)
        {
            base.Channel.InsertDocument(partyId, name, description, fileName, userId);
        }

        public void UpdateEnrollmentStatusReasonID(int regID, int reasonCodeID, string userId)
        {

            base.Channel.UpdateEnrollmentStatusReasonID(regID, reasonCodeID, userId);
        }

        public void InsertUserProvGrp_XREF(string userID, string taxID, string npi)
        {
            base.Channel.InsertUserProvGrp_XREF(userID, taxID, npi);
        }

        public void DeleteDocument(int documentID)
        {
            base.Channel.DeleteDocument(documentID);
        }

        public System.Data.DataSet GetStgProviderById(string regId)
        {
            return base.Channel.GetStgProviderById(regId);
        }

        public System.Data.DataSet SelectForProviderFile(string npi, string ssn)
        {
            return base.Channel.SelectForProviderFile(npi, ssn);
        }

        public System.Data.DataSet SelectProviderContactEmail(int regId)
        {
            return base.Channel.SelectProviderContactEmail(regId);
        }

        public string SelectProviderContactEmailAddress(int regId)
        {
            return base.Channel.SelectProviderContactEmailAddress(regId);
        }

        public System.Data.DataSet SelectConversionProvider(string userId = null, string userName = null, int regId = 0, string taxId = null, string npi = null)
        {
            return base.Channel.SelectConversionProvider(userId, userName, regId, taxId, npi);
        }

        public System.Data.DataSet SelectConversionAffiliation(string regAffiliationId)
        {
            return base.Channel.SelectConversionAffiliation(regAffiliationId);
        }
        public bool ValidateIsPrimarySpeciality(int regID, int specialityID)
        {
            return base.Channel.ValidateIsPrimarySpeciality(regID, specialityID);
        }
        public DataSet ValidatePracticePartners(int memberRegID, int convenerRegID)
        {
            return base.Channel.ValidatePracticePartners(memberRegID, convenerRegID);
        }
        public DataSet ValidatePracticePartnersUploadControl(int regID, int pageTypeID, string sectionName)
        {
            return base.Channel.ValidatePracticePartnersUploadControl(regID, pageTypeID, sectionName);
        }
        public DataSet GetProviderNameByMedicaidID(string medicaidId)
        {
            return base.Channel.GetProviderNameByMedicaidID(medicaidId);
        }
        public System.Data.DataSet GetPracticePartnership(int regID)
        {
            return base.Channel.GetPracticePartnership(regID);
        }

        public System.Data.DataSet Search_NPPES_EntityType(string NPI)
        {
            return base.Channel.Search_NPPES_EntityType(NPI);
        }
        public bool GetValidateNPI(string NPI, int regID = 0)
        {
            return base.Channel.GetValidateNPI(NPI, regID);
        }

        public List<string> GetMedicaidIdsByUserName(string username)
        {
            return base.Channel.GetMedicaidIdsByUserName(username);
        }

        public List<string> GetMedicaidIdsByMedicaidId(string medicaidId)
        {
            return base.Channel.GetMedicaidIdsByMedicaidId(medicaidId);
        }

        public System.Data.DataSet GetSubmitRosterByNPI(string providerNPI)
        {
            return base.Channel.GetSubmitRosterByNPI(providerNPI);
        }

        public System.Data.DataSet GetMMISMatch(string providerNPI, string ssn)
        {
            return base.Channel.GetMMISMatch(providerNPI, ssn);
        }

        public System.Data.DataSet GetMMISServiceAddressByNPI(string providerNPI)
        {
            return base.Channel.GetMMISServiceAddressByNPI(providerNPI);
        }

        public System.Data.DataSet GetMMISServiceAddressByValues(string providerID, string npi, string ssn)
        {
            return base.Channel.GetMMISServiceAddressByValues(providerID, npi, ssn);
        }

        public System.Data.DataSet GetMMISServiceValues(string providerID, string npi, string ssn)
        {
            return base.Channel.GetMMISServiceValues(providerID, npi, ssn);
        }

        public System.Data.DataSet GetServiceAddresses(int submitRosterId)
        {
            return base.Channel.GetServiceAddresses(submitRosterId);
        }

        public void SaveMMISData(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId)
        {
            base.Channel.SaveMMISData(isGroup, medicaidPK, medicaidId, groupMedicaidId, groupNPI, userId);
        }

        public void SaveMMISDataOptional(bool isGroup, int medicaidPK, string medicaidId, string groupMedicaidId, string groupNPI, string userId)
        {
            base.Channel.SaveMMISDataOptional(isGroup, medicaidPK, medicaidId, groupMedicaidId, groupNPI, userId);
        }

        public void SavePartyStatus(int partyId, int providerStatusChangeTypeId, int pdmsStatusTypeId, int? medicaidPK, string userId)
        {
            base.Channel.SavePartyStatus(partyId, providerStatusChangeTypeId, pdmsStatusTypeId, medicaidPK, userId);
        }

        public void SubmitReturnRosters(System.Data.DataSet ReturnRosters)
        {
            base.Channel.SubmitReturnRosters(ReturnRosters);
        }

        public System.Data.DataSet GetStates()
        {
            return base.Channel.GetStates();
        }

        public System.Data.DataSet SelectCDSRequireStates()
        {
            return base.Channel.SelectCDSRequireStates();
        }
        public System.Data.DataSet SelectRDMCodeSetsByName(string codeSetName)
        {
            return base.Channel.SelectRDMCodeSetsByName(codeSetName);
        }
        public System.Data.DataSet CheckIfCPCPrimary(int reg_id)
        {
            return base.Channel.CheckIfCPCPrimary(reg_id);
        }
        public System.Data.DataSet SelectRDMCodeSetstatusByName(string codeSetName)
        {
            return base.Channel.SelectRDMCodeSetstatusByName(codeSetName);
        }
        public int SelectRDMCodeSetChangeCount(string codeSetName)
        {
            return base.Channel.SelectRDMCodeSetChangeCount(codeSetName);
        }

        public System.Data.DataSet SelectRDMCodeSets()
        {
            return base.Channel.SelectRDMCodeSets();
        }

        public void ApproveRDMCodeSetsByName(string codeSetName, bool isApproved, Guid userID)
        {
            base.Channel.ApproveRDMCodeSetsByName(codeSetName, isApproved, userID);
        }

        #region WPC Codeset methods
        public System.Data.DataSet SelectWPCCodeSets()
        {
            return base.Channel.SelectWPCCodeSets();
        }

        public System.Data.DataSet SelectWPCCodeSetsByName(string codeSetName)
        {
            return base.Channel.SelectWPCCodeSetsByName(codeSetName);
        }
        public System.Data.DataSet SelectWPCCodeSetStatusByName(string codeSetName)
        {
            return base.Channel.SelectWPCCodeSetStatusByName(codeSetName);
        }
        public int SelectWPCCodeSetChangeCount(string codeSetName)
        {
            return base.Channel.SelectWPCCodeSetChangeCount(codeSetName);
        }

        public void ApproveWPCCodeSetsByName(string codeSetName, bool isApproved)
        {
            base.Channel.ApproveWPCCodeSetsByName(codeSetName, isApproved);
        }
        #endregion

        #region C4 Codeset methods
        public System.Data.DataSet SelectC4CodeSets()
        {
            return base.Channel.SelectC4CodeSets();
        }
        public System.Data.DataSet SelectC4CodeSetsByName(string codeSetName)
        {
            return base.Channel.SelectC4CodeSetsByName(codeSetName);
        }
        public System.Data.DataSet SelectC4CodeSetStatusByName(string codeSetName)
        {
            return base.Channel.SelectC4CodeSetStatusByName(codeSetName);
        }

        public System.Data.DataSet GetLookupDataUpdateTrace(string tablename)
        {
            return base.Channel.GetLookupDataUpdateTrace(tablename);
        }

        public void SaveLookupDataUpdateTrace(string tablename, string approvalStatus, string userId)
        {
            base.Channel.SaveLookupDataUpdateTrace(tablename, approvalStatus, userId);
        }

        public void ApproveC4CodeSetsByName(string codeSetName, bool isApproved)
        {
            base.Channel.ApproveC4CodeSetsByName(codeSetName, isApproved);
        }
        public int SelectC4CodeSetChangeCount(string codeSetName)
        {
            return base.Channel.SelectC4CodeSetChangeCount(codeSetName);
        }
        #endregion

        #region CMS Codeset methods
        public System.Data.DataSet SelectCMSCodeSets()
        {
            return base.Channel.SelectCMSCodeSets();
        }
        public System.Data.DataSet SelectCMSCodeSetsByName(string codeSetName)
        {
            return base.Channel.SelectCMSCodeSetsByName(codeSetName);
        }
        public System.Data.DataSet SelectCMSCodeSetStatusByName(string codeSetName)
        {
            return base.Channel.SelectCMSCodeSetStatusByName(codeSetName);
        }
        public void ApproveCMSCodeSetsByName(string codeSetName, bool isApproved)
        {
            base.Channel.ApproveCMSCodeSetsByName(codeSetName, isApproved);
        }
        public int SelectCMSCodeSetChangeCount(string codeSetName)
        {
            return base.Channel.SelectCMSCodeSetChangeCount(codeSetName);
        }
        #endregion

        #region NUBC Codeset methods
        public System.Data.DataSet SelectNUBCCodeSets()
        {
            return base.Channel.SelectNUBCCodeSets();
        }
        public System.Data.DataSet SelectNUBCCodeSetsByName(string codeSetName)
        {
            return base.Channel.SelectNUBCCodeSetsByName(codeSetName);
        }
        public System.Data.DataSet SelectNUBCCodeSetStatusByName(string codeSetName)
        {
            return base.Channel.SelectNUBCCodeSetStatusByName(codeSetName);
        }
        public void ApproveNUBCCodeSetsByName(string codeSetName, bool isApproved)
        {
            base.Channel.ApproveNUBCCodeSetsByName(codeSetName, isApproved);
        }
        public int SelectNUBCCodeSetChangeCount(string codeSetName)
        {
            return base.Channel.SelectNUBCCodeSetChangeCount(codeSetName);
        }
        #endregion

        public System.Data.DataSet GetApplicationTypes()
        {
            return base.Channel.GetApplicationTypes();
        }
        public System.Data.DataSet GetWaiverTypes()
        {
            return base.Channel.GetWaiverTypes();
        }

        public System.Data.DataSet GetCAQHErrors()
        {
            return base.Channel.GetCAQHErrors();
        }

        public System.Data.DataSet GetCAQHStatuses()
        {
            return base.Channel.GetCAQHStatuses();
        }

        public System.Data.DataSet GetOwnershipTypes()
        {
            return base.Channel.GetOwnershipTypes();
        }
        public System.Data.DataSet GetPracticeTypes()
        {
            return base.Channel.GetPracticeTypes();
        }
        public System.Data.DataSet GetContentTypes(int contentTypeID)
        {
            return base.Channel.GetContentTypes(contentTypeID);
        }

        public System.Data.DataSet GetMMISErrors()
        {
            return base.Channel.GetMMISErrors();
        }

        public System.Data.DataSet GetMMISStatuses()
        {
            return base.Channel.GetMMISStatuses();
        }

        public System.Data.DataSet GetProviderTypes()
        {
            return base.Channel.GetProviderTypes();
        }

        public System.Data.DataSet GetCategoryFeeScheduleTypes()
        {
            return base.Channel.GetCategoryFeeScheduleTypes();
        }
        public System.Data.DataSet GetAffiliateFilesByUser(Guid userID)
        {
            return base.Channel.GetAffiliateFilesByUser(userID);
        }
        public System.Data.DataSet GetBulkAgentFilesByUser(Guid userID)
        {
            return base.Channel.GetBulkAgentFilesByUser(userID);
        }

        public System.Data.DataSet GetPAAssignmentGroups()
        {
            return base.Channel.GetPAAssignmentGroups();
        }

        public System.Data.DataSet GetPAAssignmentProcCodes()
        {
            return base.Channel.GetPAAssignmentProcCodes();
        }

        public System.Data.DataSet GetTypeofPractice()
        {
            return base.Channel.GetTypeofPractice();
        }

        public System.Data.DataSet GetEnrollmentStatusType()
        {
            return base.Channel.GetEnrollmentStatusType();
        }

        public System.Data.DataSet GetTerminationStatuses()
        {
            return base.Channel.GetTerminationStatuses();
        }

        public System.Data.DataSet GetMedicaidEnrollmentStatusType()
        {
            return base.Channel.GetMedicaidEnrollmentStatusType();
        }

        public System.Data.DataSet GetMedicareEnrollmentStatusType()
        {
            return base.Channel.GetMedicareEnrollmentStatusType();
        }

        public System.Data.DataSet GetTermReason()
        {
            return base.Channel.GetTermReason();
        }

        public System.Data.DataSet GetTaxIdType()
        {
            return base.Channel.GetTaxIdType();
        }

        public System.Data.DataSet GetProviderTypes2()
        {
            return base.Channel.GetProviderTypes2();
        }

        public System.Data.DataSet GetCredentialingProviderTypes()
        {
            return base.Channel.GetCredentialingProviderTypes();
        }

        public System.Data.DataSet GetProviderTypesByProviderCategory(string CategoryID)
        {
            return base.Channel.GetProviderTypesByProviderCategory(CategoryID);
        }

        public System.Data.DataSet GetProviderTypeById(int providerTypeId)
        {
            return base.Channel.GetProviderTypeById(providerTypeId);
        }

        public System.Data.DataSet GetProviderTypesByTypeId(int applicationTypeId, int providerCategorytypeid, string roleName)
        {
            return base.Channel.GetProviderTypesByTypeId(applicationTypeId, providerCategorytypeid, roleName);
        }

        public System.Data.DataSet SelectOwnerCategoryType()
        {
            return base.Channel.SelectOwnerCategoryType();
        }

        public System.Data.DataSet SelectTypeOfCoverage()
        {
            return base.Channel.SelectTypeOfCoverage();
        }


        public System.Data.DataSet SelectDefendentType()
        {
            return base.Channel.SelectDefendentType();
        }

        public System.Data.DataSet SelectClaimStatus()
        {
            return base.Channel.SelectClaimStatus();
        }

        public System.Data.DataSet SelectProviderPaymentType()
        {
            return base.Channel.SelectProviderPaymentType();
        }

        public System.Data.DataSet SelectDMEQualificationType(int CategoryID)
        {
            return base.Channel.SelectDMEQualificationType(CategoryID);
        }

        public System.Data.DataSet SelectProductServiceSubCategoryType(int CategoryID)
        {
            return base.Channel.SelectProductServiceSubCategoryType(CategoryID);
        }

        public System.Data.DataSet SelectProductServiceCategoryType()
        {
            return base.Channel.SelectProductServiceCategoryType();
        }

        public System.Data.DataSet SelectCategoryOfServiceType(int ProviderTypeID)
        {
            return base.Channel.SelectCategoryOfServiceType(ProviderTypeID);
        }

        public System.Data.DataSet GetProviderCategories(bool includeGroupMemberProfile)
        {
            return base.Channel.GetProviderCategories(includeGroupMemberProfile);
        }

        public System.Data.DataSet GetProviderCategoriesByApplication(int applicationTypeID, int waiverTypeID)
        {
            return base.Channel.GetProviderCategoriesByApplication(applicationTypeID, waiverTypeID);
        }

        public System.Data.DataSet SelectRegistrationProviderTypesByCategory(int applicationTypeId, int categoryTypeID, int WaiverTypeID)
        {
            return base.Channel.SelectRegistrationProviderTypesByCategory(applicationTypeId, categoryTypeID, WaiverTypeID);
        }

        public System.Data.DataSet GetMaritalStatus()
        {
            return base.Channel.GetMaritalStatus();
        }

        public System.Data.DataSet GetAppealStatus()
        {
            return base.Channel.GetAppealStatus();
        }

        public System.Data.DataSet GetGovernmentType()
        {
            return base.Channel.GetGovernmentType();
        }

        public System.Data.DataSet GetPROFITType()
        {
            return base.Channel.GetPROFITType();
        }


        public System.Data.DataSet GetPROFITStatus()
        {
            return base.Channel.GetPROFITStatus();
        }

        public System.Data.DataSet GetTYPE_OF_OWNERSHIP()
        {
            return base.Channel.GetTYPE_OF_OWNERSHIP();
        }

        public System.Data.DataSet GetAccountTypeEntity()
        {
            return base.Channel.GetAccountTypeEntity();
        }

        public System.Data.DataSet GetEFT_TYPE()
        {
            return base.Channel.GetEFT_TYPE();
        }

        public System.Data.DataSet GetPDMSStatuses()
        {
            return base.Channel.GetPDMSStatuses();
        }

        public System.Data.DataSet GetPDMSErrors()
        {
            return base.Channel.GetPDMSErrors();
        }

        public System.Data.DataSet GetEnrollmentRejectReasons()
        {
            return base.Channel.GetEnrollmentRejectReasons();
        }



        public System.Data.DataSet GetResolvingActionTypes(bool documentsOnly, int partyId, int errorTypeId)
        {
            return base.Channel.GetResolvingActionTypes(documentsOnly, partyId, errorTypeId);
        }

        public System.Data.DataSet GetResolvingReasonTypes(int resolvingActionTypeId, int errorTypeId)
        {
            return base.Channel.GetResolvingReasonTypes(resolvingActionTypeId, errorTypeId);
        }

        public System.Data.DataSet GetMMISEnrollmentStatuses()
        {
            return base.Channel.GetMMISEnrollmentStatuses();
        }

        public System.Data.DataSet GetDocumentTypes()
        {
            return base.Channel.GetDocumentTypes();
        }

        public System.Data.DataSet GetErrorTypes()
        {
            return base.Channel.GetErrorTypes();
        }

        public System.Data.DataSet GetErrorResolvingActions(int errorTypeID)
        {
            return base.Channel.GetErrorResolvingActions(errorTypeID);
        }

        public System.Data.DataSet GetErrorStatusTypes()
        {
            return base.Channel.GetErrorStatusTypes();
        }

        public System.Data.DataSet GetErrorCategoryTypes()
        {
            return base.Channel.GetErrorCategoryTypes();
        }

        public System.Data.DataSet GetErrorDispositions()
        {
            return base.Channel.GetErrorDispositions();
        }

        public System.Data.DataSet GetProviderNoteTypes()
        {
            return base.Channel.GetProviderNoteTypes();
        }

        public System.Data.DataSet GetPermissionStatuses()
        {
            return base.Channel.GetPermissionStatuses();
        }

        public System.Data.DataSet GetPermissionApplications()
        {
            return base.Channel.GetPermissionApplications();
        }

        public System.Data.DataSet GetSecurityQuestions()
        {
            return base.Channel.GetSecurityQuestions();
        }

        public System.Data.DataSet SelectProviderTypesWithAbbrev(string roleName)
        {
            return base.Channel.SelectProviderTypesWithAbbrev(roleName);
        }

        public System.Data.DataSet SelectProviderTypesPublicSearch()
        {
            return base.Channel.SelectProviderTypesPublicSearch();
        }

        public System.Data.DataSet SelectSpecialtyTypes(string providerTypeID = null)
        {
            return base.Channel.SelectSpecialtyTypes(providerTypeID);
        }

        public System.Data.DataSet SelectODMCredentialingIsChecked(int REG_ID)
        {
            return base.Channel.SelectODMCredentialingIsChecked(REG_ID);
        }

        public System.Data.DataSet SelectDelegates()
        {
            return base.Channel.SelectDelegates();
        }

        public System.Data.DataSet GetIndvAffPageRequired(int regID)
        {
            return base.Channel.GetIndvAffPageRequired(regID);
        }

        public System.Data.DataSet SelectGroupSpecialtiesByProviderType(int providerTypeID)
        {
            return base.Channel.SelectGroupSpecialtiesByProviderType(providerTypeID);
        }

        public System.Data.DataSet SelectGroupSpecialtiesByProviderTypeRole(int providerTypeID, bool isInternalUser, int regid)
        {
            return base.Channel.SelectGroupSpecialtiesByProviderTypeRole(providerTypeID, isInternalUser, regid);
        }

        public System.Data.DataSet GetSpecialtiesByMMISId(string mmisSpecialtiesId)
        {
            return base.Channel.GetSpecialtiesByMMISId(mmisSpecialtiesId);
        }

        public System.Data.DataSet GetActiveSpecialities(string medicaidId)
        {
            return base.Channel.GetActiveSpecialities(medicaidId);
        }

        public System.Data.DataSet GetServiceActiveEnrollSpan(string medicaidId)
        {
            return base.Channel.GetServiceActiveEnrollSpan(medicaidId);
        }

        public System.Data.DataSet SelectAllSpecialtiesByProviderType(int providerTypeID)
        {
            return base.Channel.SelectAllSpecialtiesByProviderType(providerTypeID);
        }

        public System.Data.DataSet SelectSpecialtiesByProviderType(int providerTypeID)
        {
            return base.Channel.SelectSpecialtiesByProviderType(providerTypeID);
        }

        public System.Data.DataSet SelectTaxonomyTypes()
        {
            return base.Channel.SelectTaxonomyTypes();
        }

        public System.Data.DataSet SelectTaxonomyTypesBySpecProvType(int specialtyTypeId, int providerTypeId)
        {
            return base.Channel.SelectTaxonomyTypesBySpecProvType(specialtyTypeId, providerTypeId);
        }


        public System.Data.DataSet SelectAllLicenseTypes(bool isUsedInMMIS)
        {
            return base.Channel.SelectAllLicenseTypes(isUsedInMMIS);
        }

        public System.Data.DataSet SelectTaxEntityTypes()
        {
            return base.Channel.SelectTaxEntityTypes();
        }

        public System.Data.DataSet SelectPracticeTypeW9()
        {
            return base.Channel.SelectPracticeTypeW9();
        }

        public System.Data.DataSet SelectAllTransactionTypes()
        {
            return base.Channel.SelectAllTransactionTypes();
        }

        public System.Data.DataSet SelectReportDocumentTypes()
        {
            return base.Channel.SelectReportDocumentTypes();
        }

        public System.Data.DataSet CheckSubmitRoster(string providerNPI, string ssn)
        {
            return base.Channel.CheckSubmitRoster(providerNPI, ssn);
        }

        public bool CheckProviderRevalidated(int regId)
        {
            return base.Channel.CheckProviderRevalidated(regId);
        }


        public System.Data.DataSet GetEmptySubmitRoster()
        {
            return base.Channel.GetEmptySubmitRoster();
        }

        public System.Data.DataSet GetEmptyServiceAddress()
        {
            return base.Channel.GetEmptyServiceAddress();
        }

        public System.Data.DataSet GetSubmitRosterById(int SubmitRosterId)
        {
            return base.Channel.GetSubmitRosterById(SubmitRosterId);
        }

        public void AddSubmitRosterExisting(System.Data.DataSet ds)
        {
            base.Channel.AddSubmitRosterExisting(ds);
        }

        public int SubmitSubmitRosters(System.Data.DataSet SubmitRosters, int pdmsStatusId)
        {
            return base.Channel.SubmitSubmitRosters(SubmitRosters, pdmsStatusId);
        }

        public void SubmitServiceAddress(int submitRosterId, System.Data.DataSet ds)
        {
            base.Channel.SubmitServiceAddress(submitRosterId, ds);
        }

        public void InsertAddress(string Street1, string Street2, string City, string State, string Zip, string Email, string Name, int AddressType_ID, string ExtensionData, int XREF_ID)
        {
            base.Channel.InsertAddress(Street1, Street2, City, State, Zip, Email, Name, AddressType_ID, ExtensionData, XREF_ID);
        }

        public void InsertProviderGroupAddress(string Street1, string Street2, string City, string State, string Zip, string Email, int AddressType_ID, string ExtensionData, int ProviderGroupId)
        {
            base.Channel.InsertProviderGroupAddress(Street1, Street2, City, State, Zip, Email, AddressType_ID, ExtensionData, ProviderGroupId);
        }

        public void InsertDEA(DateTime? EffectiveDateTime, DateTime? EndDateTime, int XREF_ID)
        {
            base.Channel.InsertDEA(EffectiveDateTime, EndDateTime, XREF_ID);
        }

        public void InsertLicense(int? License, string StateCode, string TypeCode, DateTime? EffectiveDateTime, DateTime? EndDateTime, int XREF_ID)
        {
            base.Channel.InsertLicense(License, StateCode, TypeCode, EffectiveDateTime, EndDateTime, XREF_ID);
        }

        public void InsertPhone(int PhoneType_ID, string Number, int XFER_ID)
        {
            base.Channel.InsertPhone(PhoneType_ID, Number, XFER_ID);
        }

        public void InsertImportedFolder(string FolderName, DateTime ImportDate)
        {
            base.Channel.InsertImportedFolder(FolderName, ImportDate);
        }

        public List<string> GetImportedFolderNames()
        {
            return base.Channel.GetImportedFolderNames();
        }

        public int InsertProviderGroup(string ServiceName, string MedicareId, string NpiId, string TaxId, DateTime? GroupEndDateTime, string ExtensionData, int? Response_ID, int? Request_ID, int XFER_ID)
        {
            return base.Channel.InsertProviderGroup(ServiceName, MedicareId, NpiId, TaxId, GroupEndDateTime, ExtensionData, Response_ID, Request_ID, XFER_ID);
        }

        public int InsertSubmitProviders_Request(int? ProviderId, int? CaqhId, string DeaId, int? SsnId, int? NpiId, int? MedicareId, int? TaxId, string Action, string LastName, string FirstName, string MiddleName, string SuffixName, string Country, DateTime? BirthDateTime, DateTime? AttestDateTime, string ProviderTypeCode, string ProviderSpecialtyCode, string TaxonomyCode, string DisclosureRec, DateTime? DisclosureDateTime, string EnrollmentStatusCode, DateTime? TermDateTime, string StatusCode)
        {
            return base.Channel.InsertSubmitProviders_Request(ProviderId, CaqhId, DeaId, SsnId, NpiId, MedicareId, TaxId, Action, LastName, FirstName, MiddleName, SuffixName, Country, BirthDateTime, AttestDateTime, ProviderTypeCode, ProviderSpecialtyCode, TaxonomyCode, DisclosureRec, DisclosureDateTime, EnrollmentStatusCode, TermDateTime, StatusCode);
        }

        public void InsertSubmitProviders_Response(string ExtensionData, string CorrelationId, string Errors, string TransactionId)
        {
            base.Channel.InsertSubmitProviders_Response(ExtensionData, CorrelationId, Errors, TransactionId);
        }

        public void InsertRetrieveProviders_Request(string Sak_Trans)
        {
            base.Channel.InsertRetrieveProviders_Request(Sak_Trans);
        }

        public int InsertRetrieveProviders_Response(string ExtensionData, int? CorrelationId, string Errors, int? ProviderId, string CountryCode, int? MedicareId, string ErrorCode, string StatusCode)
        {
            return base.Channel.InsertRetrieveProviders_Response(ExtensionData, CorrelationId, Errors, ProviderId, CountryCode, MedicareId, ErrorCode, StatusCode);
        }

        public Dictionary<string, int> SelectAddressType()
        {
            return base.Channel.SelectAddressType();
        }

        public Dictionary<string, int> SelectPhoneType()
        {
            return base.Channel.SelectPhoneType();
        }

        public Dictionary<string, int> SelectTransmissionType()
        {
            return base.Channel.SelectTransmissionType();
        }

        public System.Data.DataSet GetAppSetting(string key)
        {
            return base.Channel.GetAppSetting(key);
        }

        public System.Data.DataSet GetAddressStagingTypes()
        {
            return base.Channel.GetAddressStagingTypes();
        }

        public void SubmitStagingProviders(System.Data.DataSet StagingProviders)
        {
            base.Channel.SubmitStagingProviders(StagingProviders);
        }

        public System.Data.DataSet GetRosterExceptions()
        {
            return base.Channel.GetRosterExceptions();
        }

        public int InsertRegistration(string formCompletionName, string formCompletionPhone, int registrationStatusTypeId, int diddReferralId, string changedBy)
        {
            return base.Channel.InsertRegistration(formCompletionName, formCompletionPhone, registrationStatusTypeId, diddReferralId, changedBy);
        }

        public int InsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {
            return base.Channel.InsertRegistrationUserXref(regId, userId, createdDateTime, createdBy);
        }
        public int OnlyInsertRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {
            return base.Channel.OnlyInsertRegistrationUserXref(regId, userId, createdDateTime, createdBy);
        }
        public void UpdateRegistrationUserXref(string regId, string userId, DateTime createdDateTime, string createdBy)
        {
            base.Channel.UpdateRegistrationUserXref(regId, userId, createdDateTime, createdBy);
        }
        public System.Data.DataSet SelectLicTotNumOfBeds(int regId)
        {
            return base.Channel.SelectLicTotNumOfBeds(regId);
        }
        public void InsertDentalRegistrationData(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.InsertDentalRegistrationData(tableName, parms);
        }
        public System.Data.DataSet SelectReg_Dental_Licenses(int regId)
        {
            return base.Channel.SelectReg_Dental_Licenses(regId);
        }

        public System.Data.DataSet SelectReg_Dental_LicenseType(int regDentalId)
        {
            return base.Channel.SelectReg_Dental_LicenseType(regDentalId);
        }

        public System.Data.DataSet SelectVisionProvidersByID(int regId)
        {
            return base.Channel.SelectVisionProvidersByID(regId);
        }

        public System.Data.DataSet SelectPharmacyProvidersByID(int regId)
        {
            return base.Channel.SelectPharmacyProvidersByID(regId);
        }

        public System.Data.DataSet SelectREG_POLICY(int regId)
        {
            return base.Channel.SelectREG_POLICY(regId);
        }

        public System.Data.DataSet SelectREG_INSURANCE_TYPE()
        {
            return base.Channel.SelectREG_INSURANCE_TYPE();
        }

        public int InsertRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate, DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            return base.Channel.InsertRegServicesType(regId, ServiceTypeID, LicenseNo, PrimaryLocation, IsParticipate, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER);
        }

        public int UpdateRegServicesType(int regId, int ServiceTypeID, int LicenseNo, int PrimaryLocation, bool IsParticipate, DateTime LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            return base.Channel.UpdateRegServicesType(regId, ServiceTypeID, LicenseNo, PrimaryLocation, IsParticipate, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER);
        }

        public void UpdatePage_Configuration(int Page_Configuration_ID, string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext)
        {
            base.Channel.UpdatePage_Configuration(Page_Configuration_ID, PageName, SectionName, LINK_TEXT, SHOW_AS_LINK_OR_TEXT, reference_path, DocumentId, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER, section_display_order, displaytext);
        }

        public void InsertPage_Configuration(string PageName, string SectionName, string LINK_TEXT, string SHOW_AS_LINK_OR_TEXT, string reference_path, int DocumentId, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER, int section_display_order, string displaytext)
        {
            base.Channel.InsertPage_Configuration(PageName, SectionName, LINK_TEXT, SHOW_AS_LINK_OR_TEXT, reference_path, DocumentId, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER, section_display_order, displaytext);
        }

        public int InsertUploadDocument(string Name, string Description, string FileName, DateTime? LAST_MODIFIED_DATE_TIME, string LAST_MODIFIED_USER)
        {
            return base.Channel.InsertUploadDocument(Name, Description, FileName, LAST_MODIFIED_DATE_TIME, LAST_MODIFIED_USER);
        }

        public void UpdateRegistration(Dictionary<string, string> parms)
        {
            base.Channel.UpdateRegistration(parms);
        }

        public void UpdateRegistrationCustom(Dictionary<string, string> parms)
        {
            base.Channel.UpdateRegistrationCustom(parms);
        }

        public void UpdateCMCLinksVisibility(Dictionary<string, string> parms)
        {
            base.Channel.UpdateCMCLinksVisibility(parms);
        }

        public void UpdateRegistrationAffiliations(int regId)
        {
            base.Channel.UpdateRegistrationAffiliations(regId);
        }

        public void SaveRegistrationProgramStatus(int regId, int regProgramStatusTypeId, string changedBy)
        {
            base.Channel.SaveRegistrationProgramStatus(regId, regProgramStatusTypeId, changedBy);
        }

        public void DeleteRegistrationUserXref(int regId)
        {
            base.Channel.DeleteRegistrationUserXref(regId);
        }

        public void RemoveReceiveACHData(int regAchRequestId, string changedBy)
        {
            base.Channel.RemoveReceiveACHData(regAchRequestId, changedBy);
        }

        public void RemoveQuestionData(int regId)
        {
            base.Channel.RemoveQuestionData(regId);
        }

        public System.Data.DataSet SelectQuestionType(string questionTypeId)
        {
            return base.Channel.SelectQuestionType(questionTypeId);
        }

        public System.Data.DataSet SelectRegistration(int regId)
        {
            return base.Channel.SelectRegistration(regId);
        }

        public System.Data.DataSet SelectRevalidationProviders(int regId)
        {
            return base.Channel.SelectRevalidationProviders(regId);
        }

        public System.Data.DataSet SearchRegistration(string taxId, string npi, string ssn, string userId)
        {
            return base.Channel.SearchRegistration(taxId, npi, ssn, userId);
        }

        public DataSet VerifyTaxIdExistsAsEINForSSN(string taxID)
        {
            return base.Channel.VerifyTaxIdExistsAsEINForSSN(taxID);
        }
        public DataSet SelectIncidentCaseDetails(string casenumber)
        {
            return base.Channel.SelectIncidentCaseDetails(casenumber);
        }

        public DataSet InsertIncidentFlags(int regId, bool POCBYMAIL, string lastmodifiedUser)
        {
            return base.Channel.InsertIncidentFlags(regId, POCBYMAIL, lastmodifiedUser);
        }

        public DataSet UpdateIncidentData(string caseNumber, bool NODNeeded, DateTime NODIssuedDate, string csNotes, string Incident_id, string lastmodifiedUser)
        {
            return base.Channel.UpdateIncidentData(caseNumber, NODNeeded, NODIssuedDate, csNotes, Incident_id, lastmodifiedUser);
        }

        public void UpdatePOCMail(int regId, bool pocBymail, string lastmodifiedUser)
        {
            base.Channel.UpdatePOCMail(regId, pocBymail, lastmodifiedUser);
        }
        public System.Data.DataSet FindRegistrationNPIDuplicate(string npi, string taxID, string userId)
        {
            return base.Channel.FindRegistrationNPIDuplicate(npi, taxID, userId);
        }
        public System.Data.DataSet GetDEARecordRegistration(string DEANumber)
        {
            return base.Channel.GetDEARecordRegistration(DEANumber);
        }
        public System.Data.DataSet SelectRegSectionStatusCMC(int regId, int workFlowId, string tableName)
        {
            return base.Channel.SelectRegSectionStatusCMC(regId, workFlowId, tableName);
        }

        public DataSet GetCMCProviderAttestationControls(int regId)
        {
            return base.Channel.GetCMCProviderAttestationControls(regId);
        }

        public System.Data.DataSet SelectRegistrationData(int regId, string tableName)
        {
            return base.Channel.SelectRegistrationData(regId, tableName);
        }

        public bool IsEnrollmentActive(string npi, string providerType)
        {
            return base.Channel.IsEnrollmentActive(npi, providerType);
        }
        public System.Data.DataSet SelectAddressCustomData(int regId, int addresstype, string tableName)
        {
            return base.Channel.SelectAddressCustomData(regId, addresstype, tableName);
        }

        public System.Data.DataSet SelectAddressCustomDataPagingNew(int regId, int addresstype, int PageNumber, int RowsPerPage)
        {
            return base.Channel.SelectAddressCustomDataPagingNew(regId, addresstype, PageNumber, RowsPerPage);
        }

        public System.Data.DataSet SelectAddressADDRESSCustomByID(int regId, int addresstype, int regAddrID)
        {
            return base.Channel.SelectAddressADDRESSCustomByID(regId, addresstype, regAddrID);
        }

        public System.Data.DataSet SelectAddressCustomDataSortedNew(int regId, int addresstype, int PageNumber, int RowsPerPage, string sortOrder, string sortColumn)
        {
            return base.Channel.SelectAddressCustomDataSortedNew(regId, addresstype, PageNumber, RowsPerPage, sortOrder, sortColumn);
        }

        public string GetEnrollmentdateforNPI(string npiNumber, string state)
        {
            return base.Channel.GetEnrollmentdateforNPI(npiNumber, state);
        }
        public System.Data.DataSet SelectRegistrationAuditData(int regId, string tableName)
        {
            return base.Channel.SelectRegistrationAuditData(regId, tableName);
        }

        public System.Data.DataSet SelectRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms)
        {
            return base.Channel.SelectRegistrationDataWithParams(storedProc, parms);
        }

        public System.Data.DataSet SelectRenderingLocationsByRegID(int regID)
        {
            return base.Channel.SelectRenderingLocationsByRegID(regID);
        }

        public string CheckRegistrationExistsByRegId(int regId)
        {
            return base.Channel.CheckRegistrationExistsByRegId(regId);
        }

        public System.Data.DataSet SelectRegistrationByUserId(string userId)
        {
            return base.Channel.SelectRegistrationByUserId(userId);
        }

        public System.Data.DataSet SelectRegDocuments(int regId, int regPageTypeId, string regPageSection, string regPageTypeExclusions, int? screeningActivityID, string roleName = "")
        {
            return base.Channel.SelectRegDocuments(regId, regPageTypeId, regPageSection, regPageTypeExclusions, screeningActivityID, roleName);
        }

        public System.Data.DataSet SelectAgreementInitials(int regId)
        {
            return base.Channel.SelectAgreementInitials(regId);
        }

        public System.Data.DataSet SelectRegNotes(int regId, int regPageTypeId)
        {
            return base.Channel.SelectRegNotes(regId, regPageTypeId);
        }

        public System.Data.DataSet SelectRegErrorTypes(int regPageTypeId, string userId, string userRole = "")
        {
            return base.Channel.SelectRegErrorTypes(regPageTypeId, userId, userRole);
        }

        public System.Data.DataSet SelectRegSubcontractors(int regId, int subcontractorTypeId)
        {
            return base.Channel.SelectRegSubcontractors(regId, subcontractorTypeId);
        }

        public int InsertRegistrationDataTable(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertRegistrationDataTable(tableName, parms);
        }

        public void UpdateORPFlag(int regId, bool flag)
        {
            base.Channel.UpdateORPFlag(regId, flag);
        }

        public string GetORPFlagforRegID(int regId)
        {
            return base.Channel.GetORPFlagforRegID(regId);
        }

        public void UpdateRegistrationDataTable(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.UpdateRegistrationDataTable(tableName, parms);
        }

        public void UpdateRegistrationDataWithParams(string storedProc, Dictionary<string, string> parms)
        {
            base.Channel.UpdateRegistrationDataWithParams(storedProc, parms);
        }

        public int InsertRegistrationData(int regId, string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertRegistrationData(regId, tableName, parms);
        }
        public int InsertRegistrationDataObject(Dictionary<string, object> parms)
        {
            return base.Channel.InsertRegistrationDataObject(parms);
        }


        public int InsertRegDocument(int regId, int regPageTypeId, string regPageSection, string name, string description, string fileName, string userId, int? screeningActivityID, string wftaskname, string documentUploadPage)
        {
            return base.Channel.InsertRegDocument(regId, regPageTypeId, regPageSection, name, description, fileName, userId, screeningActivityID, wftaskname, documentUploadPage);
        }

        public void ProcessGlobalAdminChangeRequest(string currentOHID, string newOHID, int docID, int onBaseDocID, DateTime createdDateTime, string lastModifiedUser)
        {
            base.Channel.ProcessGlobalAdminChangeRequest(currentOHID, newOHID, docID,  onBaseDocID, createdDateTime, lastModifiedUser);
        }

        public int GetRegistrationIdByUserId(string userId, string currentAdminOHID)
        {
            return base.Channel.GetRegistrationIdByUserId(userId, currentAdminOHID);
        }

        public void InsertRegProviderNote(int regId, int regPageTypeId, int regSectionTypeId, int noteTypeID, string noteText, DateTime noteDate, int stepId, string userId)
        {
            base.Channel.InsertRegProviderNote(regId, regPageTypeId, regSectionTypeId, noteTypeID, noteText, noteDate, stepId, userId);
        }
        public void InsertProviderDisenrollment(int regId, string providerDisenrollmentId, DateTime disenrollDate, string modifiedBy, bool isInsert = true)
        {
            base.Channel.InsertProviderDisenrollment(regId, providerDisenrollmentId, disenrollDate, modifiedBy, isInsert);
        }

        public int UpdateRegistrationData(int regId, string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.UpdateRegistrationData(regId, tableName, parms);
        }

        public int InsertUploadFile(Dictionary<string, string> parms)
        {
            return base.Channel.InsertUploadFile(parms);
        }

        public string GetProviderTypeByRegId(int regId)
        {
            return base.Channel.GetProviderTypeByRegId(regId);
        }

        public string GetProviderTypeIdByRegId(int regId)
        {
            return base.Channel.GetProviderTypeIdByRegId(regId);
        }

        public void SaveRegistrationSectionStatus(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId, string changedBy)
        {
            base.Channel.SaveRegistrationSectionStatus(regId, regPageTypeId, regSectionTypeId, regProviderStatusTypeId, regProviderServicesStatusTypeId, changedBy);
        }

        public void SaveRegistrationPageStatus(int regId, int regPageTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId, string changedBy, int? FinancialReviewStatus, int? LTCReviewStatus, int? StateReviewStatus, int? DBHReviewStatus, int? DDSReviewStatus, int? DHCFReviewStatus, int? DHCFApplicationFeeReviewStatus, int? StateApplicationReviewStatus, int? DBHReviewerReviewStatus, int? DDSReviewerReviewStatus)
        {
            base.Channel.SaveRegistrationPageStatus(regId, regPageTypeId, regProviderStatusTypeId, regProviderServicesStatusTypeId, changedBy, FinancialReviewStatus, LTCReviewStatus, StateReviewStatus, DBHReviewStatus, DDSReviewStatus, DHCFReviewStatus, DHCFApplicationFeeReviewStatus, StateApplicationReviewStatus, DBHReviewerReviewStatus, DDSReviewerReviewStatus);
        }

        public void SyncRegistrationPageStatus(int regId, int regPageTypeId, string changedBy)
        {
            base.Channel.SyncRegistrationPageStatus(regId, regPageTypeId, changedBy);
        }

        public void SaveRegistrationQuestion(int regId, string questionTypeId, int response, int modifiedStatusTypeId, string changedBy, string responseComment = null)
        {
            base.Channel.SaveRegistrationQuestion(regId, questionTypeId, response, modifiedStatusTypeId, changedBy, responseComment);
        }

        public void SaveRegistrationAgreementInitials(int regId, string questionTypeId, int response, int modifiedStatusTypeId, string changedBy, string responseComment = null, string Initials = null)
        {
            base.Channel.SaveRegistrationAgreementInitials(regId, questionTypeId, response, modifiedStatusTypeId, changedBy, responseComment, Initials);
        }

        public void SaveRegistrationOwnerXref(int regId, int regOwner1Id, int regOwner2Id, int relationshipTypeId, int modifiedStatusTypeId, string changedBy, int relationshipTypeId2)
        {
            base.Channel.SaveRegistrationOwnerXref(regId, regOwner1Id, regOwner2Id, relationshipTypeId, modifiedStatusTypeId, changedBy, relationshipTypeId2);
        }

        public System.Data.DataSet SelectRegPageStatus(int regId)
        {
            return base.Channel.SelectRegPageStatus(regId);
        }

        public bool CheckEnrolledProvider(int regId)
        {
            return base.Channel.CheckEnrolledProvider(regId);
        }

        public bool CheckHRTermFieldsByRegID(int regId)
        {
            return base.Channel.CheckHRTermFieldsByRegID(regId);
        }

        public bool CheckRRTermFieldsByRegID(int regId)
        {
            return base.Channel.CheckRRTermFieldsByRegID(regId);
        }

        public DataSet GetHRTermFieldsByRegID(int regId)
        {
            return base.Channel.GetHRTermFieldsByRegID(regId);
        }

        public DataSet GetRRTermFieldsByRegID(int regId)
        {
            return base.Channel.GetRRTermFieldsByRegID(regId);
        }

        public System.Data.DataSet SelectRelationshipTypes()
        {
            return base.Channel.SelectRelationshipTypes();
        }

        public System.Data.DataSet SelectREG_ERRORcustom(int regId, int pageTypeId, int sectionTypeId, bool includeClosed, bool includePartyErrors)
        {
            return base.Channel.SelectREG_ERRORcustom(regId, pageTypeId, sectionTypeId, includeClosed, includePartyErrors);
        }

        public void DeleteRegistrationData(string tableName, string idColumnName, int id)
        {
            base.Channel.DeleteRegistrationData(tableName, idColumnName, id);
        }
        public void DeleteDocumentMailById(int priorAuthDocumentMailId)
        {
            base.Channel.DeleteDocumentMailById(priorAuthDocumentMailId);
        }

        public void DeleteRegistrationDataWithParams(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.DeleteRegistrationDataWithParams(tableName, parms);
        }

        public void InsertUserWorkflowPermission(string USERID, int WORKFLOW_ID, string LAST_MODIFIED_USER)
        {
            base.Channel.InsertUserWorkflowPermission(USERID, WORKFLOW_ID, LAST_MODIFIED_USER);
        }

        public void DeleteUserWorkflowPermission(string USERID)
        {
            base.Channel.DeleteUserWorkflowPermission(USERID);
        }

        public System.Data.DataSet SelectUserWorkflowPermission(string USERID)
        {
            return base.Channel.SelectUserWorkflowPermission(USERID);
        }

        public System.Data.DataSet TransferPDMStoRegistration(bool ownershipChanged, int partyId, string userId, DateTime requestedEffectiveDate, DateTime changeEffectiveDate, string formCompletionName, string formCompletionPhone)
        {
            return base.Channel.TransferPDMStoRegistration(ownershipChanged, partyId, userId, requestedEffectiveDate, changeEffectiveDate, formCompletionName, formCompletionPhone);
        }

        public System.Data.DataSet TransferPDMStoRegistrationIndividual(int partyId, string userId, string firstLastName)
        {
            return base.Channel.TransferPDMStoRegistrationIndividual(partyId, userId, firstLastName);
        }

        public System.Data.DataSet GetGroupNames(int regId)
        {
            return base.Channel.GetGroupNames(regId);
        }

        public System.Data.DataSet GetGroupAffiliationHistory(int regId, int pageSize, int pageNumber)
        {
            return base.Channel.GetGroupAffiliationHistory(regId, pageSize, pageNumber);
        }

        public System.Data.DataSet GetWFProcessByRegId(int regId)
        {
            return base.Channel.GetWFProcessByRegId(regId);
        }

        public bool RegistrationNeverSubmitted(int regId)
        {
            return base.Channel.RegistrationNeverSubmitted(regId);
        }

        public DataSet CheckOwnerAddressExists(int regId, int addressTypeId)
        {
            return base.Channel.CheckOwnerAddressExists(regId, addressTypeId);
        }

        public System.Data.DataSet GetFilteredUsers(string username, string contactname, string orgName, string taxId, string NPI, string role, int pageSize, int startIndex)
        {
            return base.Channel.GetFilteredUsers(username, contactname, orgName, taxId, NPI, role, pageSize, startIndex);
        }

        public void DeleteUserApplicationType(string userId, int applicationTypeId)
        {
            base.Channel.DeleteUserApplicationType(userId, applicationTypeId);
        }

        public void DeleteUserProviderType(string userId, int providerTypeId)
        {
            base.Channel.DeleteUserProviderType(userId, providerTypeId);
        }

        public void DeleteUserStatusType(string userId, int statusTypeId)
        {
            base.Channel.DeleteUserStatusType(userId, statusTypeId);
        }

        public System.Data.DataSet GetUserSecurityQuestions(string userId)
        {
            return base.Channel.GetUserSecurityQuestions(userId);
        }

        public System.Data.DataSet GetExistingProviders(string taxId, string NPI)
        {
            return base.Channel.GetExistingProviders(taxId, NPI);
        }

        public System.Data.DataSet GetUserApplicationTypes(string userId)
        {
            return base.Channel.GetUserApplicationTypes(userId);
        }

        public System.Data.DataSet GetUserProviderTypes(string userId)
        {
            return base.Channel.GetUserProviderTypes(userId);
        }

        public System.Data.DataSet GetUserStatusTypes(string userId)
        {
            return base.Channel.GetUserStatusTypes(userId);
        }

        public System.Data.DataSet GetUserSecurityQuestionsByNameEmail(string userName, string email)
        {
            return base.Channel.GetUserSecurityQuestionsByNameEmail(userName, email);
        }

        public System.Data.DataSet GetUsernameForProvider(string email, string taxId, string NPI, string medicaidId, string zip)
        {
            return base.Channel.GetUsernameForProvider(email, taxId, NPI, medicaidId, zip); ;
        }
        public System.Data.DataSet GetAffiliateSpecialtyExportData(int regID)
        {
            return base.Channel.GetAffiliateSpecialtyExportData(regID);
        }

        public System.Data.DataSet GetAffiliateLicenseExportData(int regID)
        {
            return base.Channel.GetAffiliateLicenseExportData(regID);
        }
        public System.Data.DataSet GetUserAccountInformation(string userId)
        {
            return base.Channel.GetUserAccountInformation(userId);
        }

        public System.Data.DataSet SelectCMCProviderDetails(string medicaidId, Guid userID)
        {
            return base.Channel.SelectCMCProviderDetails(medicaidId, userID);
        }

        public void InsertUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt, string medId, int provCat, int provType,
            string taxId, int taxIDTypeID, string npi, string groupName, int zip, int diddReferralId, int? taxonomyTypeId, int? specialtyTypeId, DateTime? changedDate, string changedBy, bool is_OHID, string OHID, string UserType, string IOPUserName, int? regId = null)
        {
            base.Channel.InsertUserAccountInformation(userId, contactName, title, phone, phoneExt, medId, provCat, provType, taxId, taxIDTypeID, npi, groupName, zip,
                diddReferralId, taxonomyTypeId, specialtyTypeId, changedDate, changedBy, is_OHID, OHID, UserType, IOPUserName, regId);
        }

        public void SaveUserIOPToken(Guid userId, string tokenResp, Guid changedBy, string userClaimResp, bool updateLastLoginDt, string IOPaccessTkn, string IOPrefreshTkn, string IOPidTkn)
        {
            base.Channel.SaveUserIOPToken(userId, tokenResp, changedBy, userClaimResp, updateLastLoginDt, IOPaccessTkn, IOPrefreshTkn, IOPidTkn);
        }

        public void SaveUserPNMToken(Guid userId, string tokenResp)
        {
            base.Channel.SaveUserPNMToken(userId, tokenResp);
        }
        public void InsertUserApplicationType(string userId, int applicationTypeId, DateTime? changedDate, string changedBy)
        {
            base.Channel.InsertUserApplicationType(userId, applicationTypeId, changedDate, changedBy);
        }

        public void InsertUserProviderType(string userId, int providerTypeId, DateTime? changedDate, string changedBy)
        {
            base.Channel.InsertUserProviderType(userId, providerTypeId, changedDate, changedBy);
        }

        public void InsertUserStatusType(string userId, int statusTypeId, DateTime? changedDate, string changedBy)
        {
            base.Channel.InsertUserStatusType(userId, statusTypeId, changedDate, changedBy);
        }

        public void InsertUserSecurityQuestions(string userId, int? question1Id, string answer1, int? question2Id, string answer2, int? question3Id, string answer3, DateTime? changedDate, string changedBy)
        {
            base.Channel.InsertUserSecurityQuestions(userId, question1Id, answer1, question2Id, answer2, question3Id, answer3, changedDate, changedBy);
        }

        public bool IsAnswerCorrect(string userName, int questionNumber, string answer)
        {
            return base.Channel.IsAnswerCorrect(userName, questionNumber, answer);
        }

        public void NotifyProviderAccountCreation(string recipients, string LEGAL_BUSINESS_NAME, string NPI, string USER_NAME, string LOGIN_URL, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyProviderAccountCreation(recipients, LEGAL_BUSINESS_NAME, NPI, USER_NAME, LOGIN_URL, regId, userid);
        }

        public void NotifyPasswordReset(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyPasswordReset(recipients, username, pdmsUrl, regId, userid);
        }

        public bool NotifyUserName(string email, string mcaid)
        {
            return base.Channel.NotifyUserName(email, mcaid);
        }

        public void UpdateUserAccountInformation(string userId, string contactName, string title, string phone, string phoneExt, DateTime? changedDate, string changedBy, bool FORCE_PASSWORD_RESET, string UserType, bool isTempPwdSent = false)
        {
            base.Channel.UpdateUserAccountInformation(userId, contactName, title, phone, phoneExt, changedDate, changedBy, FORCE_PASSWORD_RESET, UserType, isTempPwdSent);
        }

        public void UpdateUserSecurityQuestions(string userId, int? question1Id, string answer1, int? question2Id, string answer2, int? question3Id, string answer3, DateTime? changedDate, string changedBy)
        {
            base.Channel.UpdateUserSecurityQuestions(userId, question1Id, answer1, question2Id, answer2, question3Id, answer3, changedDate, changedBy);
        }

        public System.Data.DataSet SelectActiveUsersByRole(string role)
        {
            return base.Channel.SelectActiveUsersByRole(role);
        }

        public void UpdateDateSigned(string RegID, DateTime Datesigned, Guid User)
        {
            base.Channel.UpdateDateSigned(RegID, Datesigned, User);
        }

        public System.Data.DataSet SelectUsersInRoles(string roles)
        {
            return base.Channel.SelectUsersInRoles(roles);
        }

        public System.Data.DataSet SelectUsersInProviderAdminRole(string roles, int regID)
        {
            return base.Channel.SelectUsersInProviderAdminRole(roles, regID);
        }

        public System.Data.DataSet SelectUsersInRolesByTaxID(string roles, string taxID, string excludeUser)
        {
            return base.Channel.SelectUsersInRolesByTaxID(roles, taxID, excludeUser);
        }

        public System.Data.DataSet SelectEmailNotifications(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string columnName = null, string sortDirection = null)
        {
            return base.Channel.SelectEmailNotifications(userId, regId, pageSize, pageNumber, subject, npi, columnName, sortDirection);
        }
        public System.Data.DataSet SelectEmailNotificationsByRegID(Guid userId, int regId, int pageSize, int pageNumber, string subject, string npi, string sortBy, string sortDirection)
        {
            return base.Channel.SelectEmailNotificationsByRegID(userId, regId, pageSize, pageNumber, subject, npi, sortBy, sortDirection);
        }

        public System.Data.DataSet GetEmailAttachments(string COMMUNICATION_EVENT_ID)
        {
            return base.Channel.GetEmailAttachments(COMMUNICATION_EVENT_ID);
        }

        public System.Data.DataSet SelectRegistrationStatuses(string UserId, int REG_ID)
        {
            return base.Channel.SelectRegistrationStatuses(UserId, REG_ID);
        }

        public System.Data.DataSet SelectRegistrationXREF(int REG_ID)
        {
            return base.Channel.SelectRegistrationXREF(REG_ID);
        }

        public void TransferRegistrationToNewUser(int REG_ID, string fromUserId, string toUserId, bool isFromUser)
        {
            base.Channel.TransferRegistrationToNewUser(REG_ID, fromUserId, toUserId, isFromUser);
        }

        public string WF_ProcessTask(int processID, string assemblyName, string className)
        {
            return base.Channel.WF_ProcessTask(processID, assemblyName, className);
        }

        public List<string> WF_ProcessAllTasks()
        {
            return base.Channel.WF_ProcessAllTasks();
        }

        public int WF_StartStep(int processID, string ownerID)
        {
            return base.Channel.WF_StartStep(processID, ownerID);
        }

        public void WF_CancelWorkflowProcess(int processID)
        {
            base.Channel.WF_CancelWorkflowProcess(processID);
        }

        public System.Data.DataSet WF_SelectStepActions(int stepID)
        {
            return base.Channel.WF_SelectStepActions(stepID);
        }

        public System.Data.DataSet WF_SelectStepInfo(int stepID)
        {
            return base.Channel.WF_SelectStepInfo(stepID);
        }

        public System.Data.DataSet WF_SelectProcess(int processID)
        {
            return base.Channel.WF_SelectProcess(processID);
        }

        public System.Data.DataSet WF_SelectProcessParameters(int processID)
        {
            return base.Channel.WF_SelectProcessParameters(processID);
        }

        public System.Data.DataSet WF_SelectStepParameters(int stepID)
        {
            return base.Channel.WF_SelectStepParameters(stepID);
        }

        public void WF_SaveProcessParameter(int processID, string parameterName, string parameterValue)
        {
            base.Channel.WF_SaveProcessParameter(processID, parameterName, parameterValue);
        }

        public void WF_SaveStepParameter(int stepID, string parameterName, string parameterValue)
        {
            base.Channel.WF_SaveStepParameter(stepID, parameterName, parameterValue);
        }

        public void WF_TakeAction(int processID, string action, string notes)
        {
            base.Channel.WF_TakeAction(processID, action, notes);
        }

        public System.Data.DataSet WF_SelectActiveOwnerSteps(string ownerID, bool includeOtherSteps)
        {
            return base.Channel.WF_SelectActiveOwnerSteps(ownerID, includeOtherSteps);
        }

        public System.Data.DataSet WF_SelectActiveOwnerStepsProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            return base.Channel.WF_SelectActiveOwnerStepsProvider(ownerID, includeOtherSteps, ownerRole);
        }

        public System.Data.DataSet WF_SelectUnassignedSteps(string groupName)
        {
            return base.Channel.WF_SelectUnassignedSteps(groupName);
        }

        public System.Data.DataSet WF_SelectRegistrationWorkflows(string ownerID)
        {
            return base.Channel.WF_SelectRegistrationWorkflows(ownerID);
        }

        public System.Data.DataSet WF_SelectWorkflowByRegId(int regId)
        {
            return base.Channel.WF_SelectWorkflowByRegId(regId);
        }

        public DataSet SearchTransactionsMonitor(string regId, string medicaidID, string fromDate, string toDate, string sortBy, int pageSize, int pageNumber, bool asc, bool nwffailures, out int totalResultCount)
        {
            return base.Channel.SearchTransactionsMonitor(regId, medicaidID, fromDate, toDate, sortBy, pageSize, pageNumber, asc, nwffailures, out totalResultCount);
        }

        public DataSet SearchByREGID(string regId, string tableName, string tableType, int pageSize, int pageNumber, string medId = null)
        {
            return base.Channel.SearchByREGID(regId, tableName, tableType, pageSize, pageNumber, medId);
        }

        public void InsertTransactionMonitorNote(int regID, int noteTypeID, string noteText, DateTime noteDate, string userId)
        {
            base.Channel.InsertTransactionMonitorNote(regID, noteTypeID, noteText, noteDate, userId);
        }

        public DataSet GetTransactionsMonitorCounts()
        {
            return base.Channel.GetTransactionsMonitorCounts();
        }

        public void UpdateTransactionAssignedStatus(int transactionQueueID, string assignedStatus, string userId)
        {
            base.Channel.UpdateTransactionAssignedStatus(transactionQueueID, assignedStatus, userId);
        }


        public System.Data.DataSet WF_SelectWorkflows()
        {
            return base.Channel.WF_SelectWorkflows();
        }

        public void updateWF_STEP_Owner(int stepId, string ownerId)
        {
            base.Channel.updateWF_STEP_Owner(stepId, ownerId);
        }

        public void updateWF_STEP_Assignment(int stepId, string assignedByUser)
        {
            base.Channel.updateWF_STEP_Assignment(stepId, assignedByUser);
        }

        public System.Data.DataSet SelectTransactions(string TransactionTypeId = null, string ProcessStartDate = null, string ProcessEndDate = null, string PartyID = null, string ServiceLocationID = null, string MedicaidID = null, string CAQHId = null, string OrderBy = null, string SortOrder = null)
        {
            return base.Channel.SelectTransactions(TransactionTypeId, ProcessStartDate, ProcessEndDate, PartyID, ServiceLocationID, MedicaidID, CAQHId, OrderBy, SortOrder);
        }

        public void CancelTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser)
        {
            base.Channel.CancelTransaction(transactionId, lastModifiedDateTime, lastModifiedUser);
        }

        public void ResubmitProcessedTransaction(int transactionId, DateTime lastModifiedDateTime, string lastModifiedUser, bool isCreatedByJob = false)
        {
            base.Channel.ResubmitProcessedTransaction(transactionId, lastModifiedDateTime, lastModifiedUser, isCreatedByJob);
        }

        public DataSet GetMMISTransactionsForRegistration(int regId)
        {
            return base.Channel.GetMMISTransactionsForRegistration(regId);
        }

        public DataSet GetWorkflowStepsForRegID(int regId)
        {
            return base.Channel.GetWorkflowStepsForRegID(regId);
        }

        public DataSet GetWorkflowStepsForProcessID(int processId)
        {
            return base.Channel.GetWorkflowStepsForProcessID(processId);
        }

        public DataSet GetApplicationDetailsbyProcessID(int ProcessId, int regId)
        {
            return base.Channel.GetApplicationDetailsbyProcessID(ProcessId, regId);
        }
        public void LogAccess(string UserId, string DocumentId, string ProviderId, string DocumentFormat, int REPORT_DOCUMENT_TYPE_ID, MAXIMUS.Core.Libraries.Enumerations.LogAccessType AccessType, string NPI)
        {
            base.Channel.LogAccess(UserId, DocumentId, ProviderId, DocumentFormat, REPORT_DOCUMENT_TYPE_ID, AccessType, NPI);
        }

        public System.Data.DataSet SelectDIDDRegions()
        {
            return base.Channel.SelectDIDDRegions();
        }

        public System.Data.DataSet SelectDIDDWaivers()
        {
            return base.Channel.SelectDIDDWaivers();
        }

        public System.Data.DataSet SelectDIDDServices()
        {
            return base.Channel.SelectDIDDServices();
        }

        public System.Data.DataSet SelectDIDDServicesByDIDD_Service_ID(int DIDD_Service_ID)
        {
            return base.Channel.SelectDIDDServicesByDIDD_Service_ID(DIDD_Service_ID);
        }

        public System.Data.DataSet SelectDIDDReferral(int referralId)
        {
            return base.Channel.SelectDIDDReferral(referralId);
        }

        public System.Data.DataSet SelectDIDDReferralByID(int referralId)
        {
            return base.Channel.SelectDIDDReferralByID(referralId);
        }

        public System.Data.DataSet SelectDuplicateReferrals(int Referral_ID, string TaxID, string ZipCode, string ZipExt)
        {
            return base.Channel.SelectDuplicateReferrals(Referral_ID, TaxID, ZipCode, ZipExt);
        }

        public System.Data.DataSet SearchProviderAllTaxID(string TaxID)
        {
            return base.Channel.SearchProviderAllTaxID(TaxID);
        }

        public DataSet GetRegistrationStatusForDIDDReferral(int referralID)
        {
            return base.Channel.GetRegistrationStatusForDIDDReferral(referralID);
        }


        public System.Data.DataSet SelectDIDDReferralByApplicationNo(string applicationNo, string taxID)
        {
            return base.Channel.SelectDIDDReferralByApplicationNo(applicationNo, taxID);
        }

        public System.Data.DataSet SelectCurrentContractInfoByRegID(int regID)
        {
            return base.Channel.SelectCurrentContractInfoByRegID(regID);
        }

        public System.Data.DataSet SelectContractHistoryByRegID(int regID)
        {
            return base.Channel.SelectContractHistoryByRegID(regID);
        }

        public System.Data.DataSet SelectDIDDReferralByRegID(int regID)
        {
            return base.Channel.SelectDIDDReferralByRegID(regID);
        }

        public System.Data.DataSet SelectSignatureHistoryByRegID(int regID)
        {
            return base.Channel.SelectSignatureHistoryByRegID(regID);
        }

        public void UpdateDIDDReferral(int diddReferralID, string firstName, string lastName,
            string groupIdentityName, string taxId, string NPI, string email, DateTime? contractFromDate, DateTime? contractToDate, string zip, string zipExt, int submitType, int locationTypeID, DateTime modifiedDate, Guid modifiedUser, int referralType)
        {
            base.Channel.UpdateDIDDReferral(diddReferralID, firstName, lastName, groupIdentityName, taxId, NPI, email, contractFromDate, contractToDate, zip, zipExt, submitType, locationTypeID, modifiedDate, modifiedUser, referralType);
        }

        public void UpdateDIDDReferralSuffix(int diddReferralId, string regionSuffix)
        {
            base.Channel.UpdateDIDDReferralSuffix(diddReferralId, regionSuffix);
        }

        public System.Data.DataSet SearchDIDDReferral(string name, string applicationNo, string taxId, string medicaidId, string npi, string emailAddress, string createdBy, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SearchDIDDReferral(name, applicationNo, taxId, medicaidId, npi, emailAddress, createdBy, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SearchDIDDReferralByRegIDs(string RegIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, bool isAssigned, out int totalResultCount)
        {
            return base.Channel.SearchDIDDReferralByRegIDs(RegIDs, pageSize, startRowIndex, tableId, statusId, ordinal, isAssigned, out totalResultCount);
        }

        public System.Data.DataSet SearchDIDDReferralByReferralIDs(string ReferralIDs, int pageSize, int startRowIndex, int tableId, int statusId, int ordinal, out int totalResultCount)
        {
            return base.Channel.SearchDIDDReferralByReferralIDs(ReferralIDs, pageSize, startRowIndex, tableId, statusId, ordinal, out totalResultCount);
        }

        public DataSet InsertDIDDReferral(string firstName, string lastName, string groupIdentityName, string applicationNo, string taxId, string NPI,
            string email, DateTime? contractFromDate, DateTime? contractToDate, bool isActive, string zip, string zipExt, int submitTypeID, int locationTypeID, DateTime createdOn, Guid createdBy, int referralType)
        {
            return base.Channel.InsertDIDDReferral(firstName, lastName, groupIdentityName, applicationNo, taxId, NPI, email, contractFromDate, contractToDate, isActive, zip, zipExt,
                submitTypeID, locationTypeID, createdOn, createdBy, referralType);
        }

        public void NotifyProviderOfDIDDReferral(string firstName, string lastName, string groupIdentityName, string appNumber, DateTime? contractFromDate, DateTime? contractToDate, string email, string inputSubject)
        {
            base.Channel.NotifyProviderOfDIDDReferral(firstName, lastName, groupIdentityName, appNumber, contractFromDate, contractToDate, email, inputSubject);
        }

        public int InsertDIDDReferralService(int diddReferralId, int diddServiceId)
        {
            return base.Channel.InsertDIDDReferralService(diddReferralId, diddServiceId);
        }

        public System.Data.DataSet SelectDIDDReferralService(int referralServiceId)
        {
            return base.Channel.SelectDIDDReferralService(referralServiceId);
        }

        public System.Data.DataSet SelectDIDDReferralServiceByReferralServiceID(int referralServiceID)
        {
            return base.Channel.SelectDIDDReferralServiceByReferralServiceID(referralServiceID);
        }

        public int InsertDIDDREFERRALWAIVER(int diddReferralId, int diddWaiverId)
        {
            return base.Channel.InsertDIDDREFERRALWAIVER(diddReferralId, diddWaiverId);
        }

        public int InsertDIDDREFERRALREGION(int diddReferralId, int diddRegionId)
        {
            return base.Channel.InsertDIDDREFERRALREGION(diddReferralId, diddRegionId);
        }

        public int UpdateDIDDReferralService(int diddReferralServiceId, int diddReferralId, int diddServiceId)
        {
            return base.Channel.UpdateDIDDReferralService(diddReferralServiceId, diddReferralId, diddServiceId);
        }

        public int DeleteDIDDREFERRALSERVICE(int diddReferralId)
        {
            return base.Channel.DeleteDIDDREFERRALSERVICE(diddReferralId);
        }

        public int DeleteDIDDREFERRALREGION(int diddReferralId)
        {
            return base.Channel.DeleteDIDDREFERRALREGION(diddReferralId);
        }

        public int DeleteDIDDREFERRALWAIVER(int diddReferralId)
        {
            return base.Channel.DeleteDIDDREFERRALWAIVER(diddReferralId);
        }

        public bool CheckAttestToFeePaymentRequired(int regID)
        {
            return base.Channel.CheckAttestToFeePaymentRequired(regID);
        }

        public bool CheckCDSNumberSectionRequired(int regID)
        {
            return base.Channel.CheckCDSNumberSectionRequired(regID);
        }
        public bool CheckIfDODDInitialApplication(int regID)
        {
            return base.Channel.CheckIfDODDInitialApplication(regID);
        }
        public bool CheckIfODAInitialApplication(int regID)
        {
            return base.Channel.CheckIfODAInitialApplication(regID);
        }

        public System.Data.DataSet SearchACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom, DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo, int sortBy, int pageNumber, int rowsPerPage, bool sortAsc)
        {
            return base.Channel.SearchACHFeeInformation(name, taxID, npi, medicaidID, feePaidDateFrom, feePaidDateTo, feeDueDateFrom, feeDueDateTo, sortBy, pageNumber, rowsPerPage, sortAsc);
        }

        public System.Data.DataSet SelectACHFeeInformation(string name, string taxID, string npi, string medicaidID, DateTime? feePaidDateFrom, DateTime? feePaidDateTo, DateTime? feeDueDateFrom, DateTime? feeDueDateTo)
        {
            return base.Channel.SelectACHFeeInformation(name, taxID, npi, medicaidID, feePaidDateFrom, feePaidDateTo, feeDueDateFrom, feeDueDateTo);
        }

        public int InsertAchFeeInformation(int partyID, DateTime paymentDate, string edisonID, string paymentNumber, DateTime createdDateTime, Guid createdByUserID)
        {
            return base.Channel.InsertAchFeeInformation(partyID, paymentDate, edisonID, paymentNumber, createdDateTime, createdByUserID);
        }

        public System.Data.DataSet SelectACHFeeInformationByPartyID(int partyID)
        {
            return base.Channel.SelectACHFeeInformationByPartyID(partyID);
        }

        public System.Data.DataSet SelectDIDDContractDetails(int regID)
        {
            return base.Channel.SelectDIDDContractDetails(regID);
        }

        public void UpdateDIDDReferralServiceDates(int referralServiceID, DateTime startDate, DateTime? endDate, DateTime modifiedOn, Guid modifiedBy)
        {
            base.Channel.UpdateDIDDReferralServiceDates(referralServiceID, startDate, endDate, modifiedOn, modifiedBy);
        }

        public System.Data.DataSet SelectDocumentTypeByName(string documentName)
        {
            return base.Channel.SelectDocumentTypeByName(documentName);
        }

        public int InsertUpdateContractByContractTypeID(int regID, int contractTypeID, string adminName, DateTime? contractStartDate, DateTime? contractEndDate, string signedBy, DateTime changedDate, string changedBy)
        {
            return base.Channel.InsertUpdateContractByContractTypeID(regID, contractTypeID, adminName, contractStartDate, contractEndDate, signedBy, changedDate, changedBy);
        }


        public void ResetContractSignatures(int regID, DateTime changedDate, Guid changedBy)
        {
            base.Channel.ResetContractSignatures(regID, changedDate, changedBy);
        }

        public System.Data.DataSet SelectPageConfigurationsSectionsByPageName(string PageName)
        {
            return base.Channel.SelectPageConfigurationsSectionsByPageName(PageName);
        }
       
        public System.Data.DataSet SelectPageConfigurationsByPageName(string PageName, Guid userID)
        {
            return base.Channel.SelectPageConfigurationsByPageName(PageName, userID);
        }

        public System.Data.DataSet SelectPageConfigurations()
        {
            return base.Channel.SelectPageConfigurations();
        }
        public System.Data.DataSet SelectBumpUpReasons()
        {
            return base.Channel.SelectBumpUpReasons();
        }
        public System.Data.DataSet SelectProviderRiskLevels()
        {
            return base.Channel.SelectProviderRiskLevels();
        }
        public System.Data.DataSet SelectProviderContractType(string typeBorR)
        {
            return base.Channel.SelectProviderContractType(typeBorR);
        }
        public System.Data.DataSet SelectProviderArea(string programCode)
        {
            return base.Channel.SelectProviderArea(programCode);
        }
        public System.Data.DataSet SelectReferenceDataWithoutParam(string storedProc)
        {
            return base.Channel.SelectReferenceDataWithoutParam(storedProc);
        }

        public System.Data.DataSet SelectMCP()
        {
            return base.Channel.SelectMCP();
        }

        public System.Data.DataSet SelectProgramType()
        {
            return base.Channel.SelectProgramType();
        }

        public System.Data.DataSet SelectProviderCPCAccreditation()
        {
            return base.Channel.SelectProviderCPCAccreditation();
        }
        public System.Data.DataSet SelectEnrollmentStatusReasons()
        {
            return base.Channel.SelectEnrollmentStatusReasons();
        }
        public System.Data.DataSet SelectReconsiderEnrollmentStatusReasons()
        {
            return base.Channel.SelectReconsiderEnrollmentStatusReasons();
        }
        public bool IsNotProcessedEligible(int regId)
        {
            return base.Channel.IsNotProcessedEligible(regId);
        }
        public System.Data.DataSet SelectProviderContractStatus()
        {
            return base.Channel.SelectProviderContractStatus();
        }

        public System.Data.DataSet SelectAllTaxonomyCodes()
        {
            return base.Channel.SelectAllTaxonomyCodes();
        }

        public System.Data.DataSet SelectActionHistoryData(string name, string npi, string taxID, string actionType, string actionStatus, string taskName, string startDate,
                string endDate, string userName, string roleName, int pageNumber, int rowsPerPage, int sortBy, bool isASC)
        {
            return base.Channel.SelectActionHistoryData(name, npi, taxID, actionType, actionStatus, taskName, startDate, endDate, userName, roleName, pageNumber, rowsPerPage, sortBy, isASC);
        }

        public DataSet SelectGroupProviderScreeningData(int regId)
        {
            return base.Channel.SelectGroupProviderScreeningData(regId);
        }

        public DataSet SelectAffiliationsScreeningData(int regID)
        {
            return base.Channel.SelectAffiliationsScreeningData(regID);
        }

        public DataSet SelectOwnersScreeningData(int regID)
        {
            return base.Channel.SelectOwnersScreeningData(regID);
        }

        public DataSet SelectOwnersScreeningDataWithEndDate(int regID, int screeningID)
        {
            return base.Channel.SelectOwnersScreeningDataWithEndDate(regID, screeningID);
        }

        public DataSet SelectOwnersScreeningEndDate(int regID)
        {
            return base.Channel.SelectOwnersScreeningEndDate(regID);
        }


        public DataSet SelectHouseholdMemberScreeningData(int regID)
        {
            return base.Channel.SelectHouseholdMemberScreeningData(regID);
        }

        public DataSet SelectProviderScreeningActivityData(int screeningID)
        {
            return base.Channel.SelectProviderScreeningActivityData(screeningID);
        }

        public DataSet SelectProviderTypeWithLicenses(int providerTypeID)
        {
            return base.Channel.SelectProviderTypeWithLicenses(providerTypeID);
        }
        public DataSet GetCPCandCMCdtlsForRegID(int regID)
        {
            return base.Channel.GetCPCandCMCdtlsForRegID(regID);
        }
        public DataSet SelectReferTocomplianceReasons()
        {
            return base.Channel.SelectReferTocomplianceReasons();
        }
        public DataSet SelectProviderScreeningActivityMatchData(int screeningActivityTypeID, int screeningActivityID, int regID, int regAffiliationID, int regOwnerID)
        {
            return base.Channel.SelectProviderScreeningActivityMatchData(screeningActivityTypeID, screeningActivityID, regID, regAffiliationID, regOwnerID);
        }

        public DataSet SelectScreeningActivityDocuments(int screeningActivityID, string userRole)
        {
            return base.Channel.SelectScreeningActivityDocuments(screeningActivityID, userRole);
        }

        public void InsertScreeningActivityDocument(int screeningActivityID, string name, string description, string fileName, string userId)
        {
            base.Channel.InsertScreeningActivityDocument(screeningActivityID, name, description, fileName, userId);
        }
        public bool RemoveScreeningExlusionData(int regID, int exclTypeID, string comments, Guid userID)
        {
            return base.Channel.RemoveScreeningExlusionData(regID, exclTypeID, comments, userID);
        }

        public void InsertProviderScreening(int regID, int workflowID, string userID, int DODDactivityStatus, string OwnerDODDActivityStatus, out bool isExactMatchFound, out bool isSoftMatchFound)
        {
            base.Channel.InsertProviderScreening(regID, workflowID, userID, DODDactivityStatus, OwnerDODDActivityStatus, out isExactMatchFound, out isSoftMatchFound);
        }

        public void DenyCPCFromProviderReview(int regID, string CPCProgramYear, int ProcessID, DateTime LastModifiedDate, Guid ModifiedBy, int WorkflowEventTypeID)
        {
            base.Channel.DenyCPCFromProviderReview(regID, CPCProgramYear, ProcessID, LastModifiedDate, ModifiedBy, WorkflowEventTypeID);
        }

        public DataSet TerminateProvider(int regID, DateTime termDate, string changedBy, string enrollmentStatusCode, string enrollmentStatusReason, bool createTrans, bool is_frm_job = false)
        {
            return base.Channel.TerminateProvider(regID, termDate, changedBy, enrollmentStatusCode, enrollmentStatusReason, createTrans, is_frm_job);
        }

        public void UpdateScreeningActivityStatus(int screeningActivityID, int screeningActivityStatusID, string adverseAction, string changedBy)
        {
            base.Channel.UpdateScreeningActivityStatus(screeningActivityID, screeningActivityStatusID, adverseAction, changedBy);
        }

        public void UpdateScreeningStatus(int screeningID, int screeningStatusID, string changedBy)
        {
            base.Channel.UpdateScreeningStatus(screeningID, screeningStatusID, changedBy);
        }
        public void UpdateScreeningPreviousStatus(int regID, string changedBy)
        {
            base.Channel.UpdateScreeningPreviousStatus(regID, changedBy);
        }

        public void UpdateScreeningResult(int screeningID, int screeningResultID, string changedBy)
        {
            base.Channel.UpdateScreeningResult(screeningID, screeningResultID, changedBy);
        }

        public void UpdateScreeningScreeningStatusID(int screeningId)
        {
            base.Channel.UpdateScreeningScreeningStatusID(screeningId);
        }


        public void InsertScreeningAdverseAction(int screeningActivityID, string description, string changedBy)
        {
            base.Channel.InsertScreeningAdverseAction(screeningActivityID, description, changedBy);
        }

        public void UpdateScreeningStatus_PeriodicWF(int regID, DateTime insertDate, string insertBy)
        {
            base.Channel.UpdateScreeningStatus_PeriodicWF(regID, insertDate, insertBy);
        }

        public DataSet SelectScreeningAdverseActions(int regID)
        {
            return base.Channel.SelectScreeningAdverseActions(regID);
        }

        public DataSet SelectScreeningStatus(int screeningID, int regID)
        {
            return base.Channel.SelectScreeningStatus(screeningID, regID);
        }

        public DataSet SelectAdverseActions(int screeningActivityID)
        {
            return base.Channel.SelectAdverseActions(screeningActivityID);
        }

        public DataSet SelectScreeningFailedActivities(int regID)
        {
            return base.Channel.SelectScreeningFailedActivities(regID);
        }

        public DataSet SelectSiteVisitScreeningData(int regID, int? screeningActivityTypeID = null)
        {
            return base.Channel.SelectSiteVisitScreeningData(regID, screeningActivityTypeID);
        }

        public DataSet SelectSiteVisitData(int RegID)
        {
            return base.Channel.SelectSiteVisitData(RegID);
        }

        public DataSet SelectSiteVisitAttemptData(int siteVisitID)
        {
            return base.Channel.SelectSiteVisitAttemptData(siteVisitID);
        }

        public DataSet SelectSiteVisitDetails(int siteVisitID)
        {
            return base.Channel.SelectSiteVisitDetails(siteVisitID);
        }

        public DataSet SelectSiteVisitAttemptDetails(int siteVisitAttemptID)
        {
            return base.Channel.SelectSiteVisitAttemptDetails(siteVisitAttemptID);
        }

        public void UpdateSiteVisitResult(int siteVisitID, int resultID, int? recommendationID, DateTime? dateCompleted, string completedBy, string changedBy)
        {
            base.Channel.UpdateSiteVisitResult(siteVisitID, resultID, recommendationID, dateCompleted, completedBy, changedBy);
        }

        public void InsertSiteVisitAttempt(int siteVisitID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments, DateTime changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID)
        {
            base.Channel.InsertSiteVisitAttempt(siteVisitID, siteVisitStatusID, siteVisitRecommendationID, dtPerformed, performedBy, comments, changedDate, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethodID);
        }

        public void UpdateSiteVisitAttempt(int siteVisitAttemptID, int siteVisitStatusID, int? siteVisitRecommendationID, DateTime? dtPerformed, string performedBy, string comments, DateTime changedDate, string changedBy, int? siteVisitAttemptTypeID, DateTime? providerResponseDate, int? siteVisitAttemptStatusID, DateTime? requiredDate, int? siteVisitMethodID, int? siteVisitFindingsID, DateTime? NodIssueDate, DateTime? POCDate, string complianceComments)
        {
            base.Channel.UpdateSiteVisitAttempt(siteVisitAttemptID, siteVisitStatusID, siteVisitRecommendationID, dtPerformed, performedBy, comments, changedDate, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethodID, siteVisitFindingsID, NodIssueDate, POCDate, complianceComments);
        }
        public void UpdateSiteVisitAttemptStatus(int registraionId, int siteVisitStatusID)
        {
            base.Channel.UpdateSiteVisitAttemptStatus(registraionId, siteVisitStatusID);
        }

        public void UpdateSiteVisitAttemptComplianceSpecialist(int siteVisitAttemptID, int? siteVisitFindingsID, string comments, DateTime? nodIssueDate, DateTime? pocDate, string complianceUser)
        {
            base.Channel.UpdateSiteVisitAttemptComplianceSpecialist(siteVisitAttemptID, siteVisitFindingsID, comments, nodIssueDate, pocDate, complianceUser);
        }

        public void UpdateSiteVisitAttemptDueByDate(int siteVisitID, DateTime? requiredDate)
        {
            base.Channel.UpdateSiteVisitAttemptDueByDate(siteVisitID, requiredDate);
        }

        public DataSet SelectServiceRemovedCheckBox(int regID)
        {
            return base.Channel.SelectServiceRemovedCheckBox(regID);
        }

        public DataSet SelectPendingSiteVisits()
        {
            return base.Channel.SelectPendingSiteVisits();
        }

        public DataSet GetPendingSiteVisits(string ownerid)
        {
            return base.Channel.GetPendingSiteVisits(ownerid);
        }

        public int GetSiteVisitsReferredToState()
        {
            return base.Channel.GetSiteVisitsReferredToState();
        }

        public DataSet GetUnassignedSiteVisits(int attempt, string assignedto, int regID)
        {
            return base.Channel.GetUnassignedSiteVisits(attempt, assignedto, regID);
        }

        public System.Data.DataSet SelectSiteVisitRecommendations()
        {
            return base.Channel.SelectSiteVisitRecommendations();
        }

        public System.Data.DataSet SelectSiteVisitResults()
        {
            return base.Channel.SelectSiteVisitResults();
        }

        public System.Data.DataSet SelectSiteVisitMethods()
        {
            return base.Channel.SelectSiteVisitMethods();
        }

        public System.Data.DataSet SelectSiteVisitFindings()
        {
            return base.Channel.SelectSiteVisitFindings();
        }

        public System.Data.DataSet SelectSiteVisitScreeningStatuses(int? providerTypeID, int? isInitialStatus)
        {
            return base.Channel.SelectSiteVisitScreeningStatuses(providerTypeID, isInitialStatus);
        }

        public System.Data.DataSet SelectApplicationFeeWaiverReasons()
        {
            return base.Channel.SelectApplicationFeeWaiverReasons();
        }

        public System.Data.DataSet SelectApplicationFeePaymentType()
        {
            return base.Channel.SelectApplicationFeePaymentType();
        }

        public System.Data.DataSet SelectApplicationFeeStatuses()
        {
            return base.Channel.SelectApplicationFeeStatuses();
        }
        public void ResetApplicationFee(int regID)
        {
            base.Channel.ResetApplicationFee(regID);
        }

        public System.Data.DataSet SelectGroupAffiliationStatuses()
        {
            return base.Channel.SelectGroupAffiliationStatuses();
        }

        public System.Data.DataSet SelectIndividualAffiliationPrivilegesStatus()
        {
            return base.Channel.SelectIndividualAffiliationPrivilegesStatus();
        }

        public System.Data.DataSet SelectRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectRegistrationsByUserID(userID, onlyProviderCategoryTypeID, excludeProviderCategoryTypeID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectAllRegistrationsByUserIDAndTaxID(Guid userID, string taxID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectAllRegistrationsByUserIDAndTaxID(userID, taxID, onlyProviderCategoryTypeID, excludeProviderCategoryTypeID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectAllRegistrationsByUserID(Guid userID, int onlyProviderCategoryTypeID, int excludeProviderCategoryTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string loggedinUserID, out int totalResultCount)
        {
            return base.Channel.SelectAllRegistrationsByUserID(userID, onlyProviderCategoryTypeID, excludeProviderCategoryTypeID, sortColumn, pageSize, startRowIndex, getTotalRowCount, loggedinUserID, out totalResultCount);
        }

        public System.Data.DataSet SelectRegistrationsByAgentUserIDAndSubRole(Guid userID, string subroleName)
        {
            return base.Channel.SelectRegistrationsByAgentUserIDAndSubRole(userID, subroleName);
        }

        public bool IsDeemedEligble(string userName)
        {
            return base.Channel.IsDeemedEligble(userName);
        }

        public System.Data.DataSet SelectRegistrationsPendingConvertedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectRegistrationsPendingConvertedByTaxID(taxID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }
        public System.Data.DataSet SelectRegistrationsWithSameNPIOverlapEffectiveDate(int reg_id, DateTime effective_date, string npi)
        {
            return base.Channel.SelectRegistrationsWithSameNPIOverlapEffectiveDate(reg_id, effective_date, npi);
        }
        public System.Data.DataSet SelectAllRegistrationsByTaxID(string taxID, Guid userID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectAllRegistrationsByTaxID(taxID, userID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectAllAffiliationsByRegId(int regId, int affiliationStatus, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectAllAffiliationsByRegId(regId, affiliationStatus, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectSubmittedAgreementPDFs(int regId, int regPageTypeID, string regPageSection, int pageSize, int startRowIndexSub, int startRowIndexUpd)
        {
            return base.Channel.SelectSubmittedAgreementPDFs(regId, regPageTypeID, regPageSection, pageSize, startRowIndexSub, startRowIndexUpd);
        }

        public System.Data.DataSet SelectMCPAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectMCPAffiliationsByRegId(regId, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }
        public System.Data.DataSet SearchAffiliationsByRegId(int regId, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, string name, string npi, string ssn, out int totalResultCount)
        {
            return base.Channel.SearchAffiliationsByRegId(regId, sortColumn, pageSize, startRowIndex, getTotalRowCount, name, npi, ssn, out totalResultCount);
        }

        public System.Data.DataSet SelectRegistrationByRegID(int regID)
        {
            return base.Channel.SelectRegistrationByRegID(regID);
        }

        public System.Data.DataSet SelectUserNameByUserID(string userID)
        {
            return base.Channel.SelectUserNameByUserID(userID);
        }

        public System.Data.DataSet SelectCredentialActivityUrl(int activityTypeId, int IsIndividual)
        {
            return base.Channel.SelectCredentialActivityUrl(activityTypeId, IsIndividual);
        }

        public System.Data.DataSet SelectConvertedRegistrationByRegID(int regID)
        {
            return base.Channel.SelectConvertedRegistrationByRegID(regID);
        }

        public System.Data.DataSet SelectConfirmedAffiliationByRegID(int regID)
        {
            return base.Channel.SelectConfirmedAffiliationByRegID(regID);
        }

        public System.Data.DataSet SelectPendingAffiliationByID(int pendingAffiliationID)
        {
            return base.Channel.SelectPendingAffiliationByID(pendingAffiliationID);
        }

        public System.Data.DataSet SelectMalpracticeClaimByID(int MalpracticeClaimID)
        {
            return base.Channel.SelectMalpracticeClaimByID(MalpracticeClaimID);
        }

        public System.Data.DataSet SelectProviderPaymentInfoByRegID(int RegID)
        {
            return base.Channel.SelectProviderPaymentInfoByRegID(RegID);
        }
        public System.Data.DataSet DeleteMalpracticeClaimByID(int MalpracticeClaimID)
        {
            return base.Channel.DeleteMalpracticeClaimByID(MalpracticeClaimID);
        }

        public System.Data.DataSet SelectAffiliationByMedicaidID(string medicaid_ID)
        {
            return base.Channel.SelectAffiliationByMedicaidID(medicaid_ID);
        }
        public System.Data.DataSet SelectPendingAffiliationByMedicaidID(string medicaid_ID, int regID)
        {
            return base.Channel.SelectPendingAffiliationByMedicaidID(medicaid_ID, regID);
        }

        public System.Data.DataSet SelectAffiliationByStatusMedicaidID(string medicaid_ID, int regID, int grpAffiliationStatusId)
        {
            return base.Channel.SelectAffiliationByStatusMedicaidID(medicaid_ID, regID, grpAffiliationStatusId);
        }

        public System.Data.DataSet SelectHealthCareAffiliationByID(int healthCareAffiliationID)
        {
            return base.Channel.SelectHealthCareAffiliationByID(healthCareAffiliationID);
        }

        public void DeleteRegDMEProductServiceCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID)
        {
            base.Channel.DeleteRegDMEProductServiceCategoryByRegID(regID, DME_PRODUCT_SERVICE_CATEGORY_Type_ID);
        }

        public void DeleteRegDMEProductServiceSubCategoryByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID)
        {
            base.Channel.DeleteRegDMEProductServiceSubCategoryByRegID(regID, DME_PRODUCT_SERVICE_CATEGORY_Type_ID, DME_PRODUCT_SERVICE_SUB_CATEGORY_Type_ID);
        }

        public void DeleteRegDMEProductServiceQualificationByRegID(int regID, int DME_PRODUCT_SERVICE_CATEGORY_Type_ID, int DME_QUALIFICATION_Type_ID)
        {
            base.Channel.DeleteRegDMEProductServiceQualificationByRegID(regID, DME_PRODUCT_SERVICE_CATEGORY_Type_ID, DME_QUALIFICATION_Type_ID);
        }

        public void DeletePendingAffiliationByID(int pendingAffiliationID)
        {
            base.Channel.DeletePendingAffiliationByID(pendingAffiliationID);
        }

        public void DeleteHealthCareFacilityAffiliationByID(int healthCareAffiliationID)
        {
            base.Channel.DeleteHealthCareFacilityAffiliationByID(healthCareAffiliationID);
        }

        public void DeleteAssignedDelegateByID(int regAssignedDelegateID)
        {
            base.Channel.DeleteAssignedDelegateByID(regAssignedDelegateID);
        }

        public System.Data.DataSet SelectNursingProfessionalCertificationByID(int nursingProfessionalID)
        {
            return base.Channel.SelectNursingProfessionalCertificationByID(nursingProfessionalID);
        }

        public System.Data.DataSet SelectBoardCertificationByID(int boardCertificationID)
        {
            return base.Channel.SelectBoardCertificationByID(boardCertificationID);
        }
        public System.Data.DataSet SelectCPRCertificationByID(int CPRID)
        {
            return base.Channel.SelectCPRCertificationByID(CPRID);
        }

        public System.Data.DataSet SelectProviderByGRPMedicaidID(string Medicaid_ID)
        {
            return base.Channel.SelectProviderByGRPMedicaidID(Medicaid_ID);
        }

        public System.Data.DataSet SelectProviderByRegID(string Reg_ID)
        {
            return base.Channel.SelectProviderByRegID(Reg_ID);
        }

        public System.Data.DataSet SelectHospitalFacilityByMedicaidID(string GRPMedicaid_ID)
        {
            return base.Channel.SelectHospitalFacilityByMedicaidID(GRPMedicaid_ID);
        }

        public System.Data.DataSet SelectAffiliationByGRPMedicaidID(int reg_ID, string GRPMedicaid_ID, string GRPTaxID, string GRPNPI)
        {
            return base.Channel.SelectAffiliationByGRPMedicaidID(reg_ID, GRPMedicaid_ID, GRPTaxID, GRPNPI);
        }

        public System.Data.DataSet SelectPendingAffiliationByRegID(int regID)
        {
            return base.Channel.SelectPendingAffiliationByRegID(regID);
        }

        public System.Data.DataSet SelectCredentialingDelegatesByRegID(int regID)
        {
            return base.Channel.SelectCredentialingDelegatesByRegID(regID);
        }

        public System.Data.DataSet SelectMalpracticeClaimByRegID(int regID)
        {
            return base.Channel.SelectMalpracticeClaimByRegID(regID);
        }

        public System.Data.DataSet SelectHealthCareFacilityAffiliationByRegID(int regID)
        {
            return base.Channel.SelectHealthCareFacilityAffiliationByRegID(regID);
        }

        public System.Data.DataSet SelectRegistrationsByTaxID(string taxID)
        {
            return base.Channel.SelectRegistrationsByTaxID(taxID);
        }

        public System.Data.DataSet SelectRegistrationsByNPI(string NPI)
        {
            return base.Channel.SelectRegistrationsByNPI(NPI);
        }
        public System.Data.DataSet GetIndividualStandardSpanStartDate(string medicaidID)
        {
            return base.Channel.GetIndividualStandardSpanStartDate(medicaidID);
        }

        public System.Data.DataSet SelectRegistrationsByAdminUpdateKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
            string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxid)
        {
            return base.Channel.SelectRegistrationsByAdminUpdateKeyFields(NPI, TaxonomyCode, ProviderTypeID,
              MMISProviderTypeID, ZipCode, ZipExt, IsFilterOtherWaivers, ServicesProviderTypeName, RegID, taxid);
        }

        public System.Data.DataSet SelectRegistrationsByKeyFields(string NPI, string TaxonomyCode, int ProviderTypeID,
            string MMISProviderTypeID, string ZipCode, string ZipExt, bool IsFilterOtherWaivers, string ServicesProviderTypeName, int RegID, string taxId, int applicationType)
        {
            return base.Channel.SelectRegistrationsByKeyFields(NPI, TaxonomyCode, ProviderTypeID,
              MMISProviderTypeID, ZipCode, ZipExt, IsFilterOtherWaivers, ServicesProviderTypeName, RegID, taxId, applicationType);
        }

        // JIRA 3041 - include DDContractNumber
        public int InsertNewProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt,
            bool IsPaperApplication, int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int waiverTypeID, bool retroEffectiveDate, string ddFacilityNumber, string DDContractNumber)
        {
            return base.Channel.InsertNewProviderRegistration(userID, applicationTypeID, providerName, dba, firstName, middleName, lastName, birthDate, gender, taxIDTypeID, taxID, npi, providerTypeID, providerCategoryTypeID,
                 practiceTypeID, requestedEffectiveDate, registrationStatusTypeId, referralID, specialtyTypeID, taxonomyTypeID, practiceName, zipCode, zipExt, IsPaperApplication, paperRequestQueueID,
                  createdDate, createdBy, workflowID, RegCreateDateTime, workflowEventTypeID, waiverTypeID, retroEffectiveDate, ddFacilityNumber, DDContractNumber);
        }

        public int InsertNewCPCProviderRegistration(int regID, string cpcType, string CPC_Program_Year, Guid userID, string CPC_Practice_Type)
        {
            return base.Channel.InsertNewCPCProviderRegistration(regID, cpcType, CPC_Program_Year, userID, CPC_Practice_Type);
        }

        public System.Data.DataSet GetCPCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID, string CPC_Program_Year, string CPC_Practice_Type)
        {
            return base.Channel.GetCPCLinkVisibility(RegID, CurrentStepID, MMISProviderTypeID, EntityTypeID, CPC_Program_Year, CPC_Practice_Type);
        }

        public bool CanReEnableCPCLinks(int RegID)
        {
            return base.Channel.CanReEnableCPCLinks(RegID);
        }
        public bool CanReEnableCMCLinks(int RegID)
        {
            return base.Channel.CanReEnableCMCLinks(RegID);
        }
        public bool CanStartCredentialReconsideration(int RegID)
        {
            return base.Channel.CanStartCredentialReconsideration(RegID);
        }

        public System.Data.DataSet CheckCMCLinkEnabledByRegID(int RegID)
        {
            return base.Channel.CheckCMCLinkEnabledByRegID(RegID);
        }
        public System.Data.DataSet CheckCMCEnrollmentPeriod()
        {
            return base.Channel.CheckCMCEnrollmentPeriod();
        }
        public System.Data.DataSet CheckCMCInvited(int RegId)
        {
            return base.Channel.CheckCMCInvited(RegId);
        }

        public System.Data.DataSet IsCMCEnrolled(int RegId)
        {
            return base.Channel.IsCMCEnrolled(RegId);
        }
        public System.Data.DataSet GetCMCLinkVisibility(int RegID, int CurrentStepID, string MMISProviderTypeID, int EntityTypeID)
        {
            return base.Channel.GetCMCLinkVisibility(RegID, CurrentStepID, MMISProviderTypeID, EntityTypeID);
        }

        public int InsertNewLinkedProviderRegistration(Guid userID, int applicationTypeID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
           string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
           int registrationStatusTypeId, int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt,
           bool IsPaperApplication, int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime? RegCreateDateTime, int workflowEventTypeID, int RegID)
        {
            return base.Channel.InsertNewLinkedProviderRegistration(userID, applicationTypeID, providerName, dba, firstName, middleName, lastName, birthDate, gender, taxIDTypeID, taxID, npi, providerTypeID, providerCategoryTypeID,
                 practiceTypeID, requestedEffectiveDate, registrationStatusTypeId, referralID, specialtyTypeID, taxonomyTypeID, practiceName, zipCode, zipExt, IsPaperApplication, paperRequestQueueID,
                  createdDate, createdBy, workflowID, RegCreateDateTime, workflowEventTypeID, RegID);
        }

        public void UpdateConvertedProviderRegistration(int regID, Guid userID, string providerName, string dba, string firstName, string middleName, string lastName, DateTime? birthDate,
            string gender, int taxIDTypeID, string taxID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int referralID, int specialtyTypeID, int taxonomyTypeID, string practiceName, string zipCode, string zipExt, bool isPaperApplication,
            int paperRequestQueueID, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate)
        {
            base.Channel.UpdateConvertedProviderRegistration(regID, userID, providerName, dba, firstName, middleName, lastName, birthDate, gender, taxIDTypeID, taxID, npi, providerTypeID, providerCategoryTypeID,
                 practiceTypeID, requestedEffectiveDate, referralID, specialtyTypeID, taxonomyTypeID, practiceName, zipCode, zipExt,
                 isPaperApplication, paperRequestQueueID, createdDate, createdBy, workflowID, npiStartDate, npiEndDate, endDate);
        }

        public void UpdatetoMedicaidProviderRegistration(int regID, Guid userID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, DateTime? requestedEffectiveDate,
            int taxonomyTypeID, string zipCode, string zipExt, DateTime createdDate, Guid createdBy, int workflowID, DateTime npiStartDate, DateTime? npiEndDate, DateTime? endDate,
            int applicationTypeID, int waiverTypeID, int workflowEventTypeID, int waiverServiceUpdateTypeID, string gender)
        {
            base.Channel.UpdatetoMedicaidProviderRegistration(regID, userID, npi, providerTypeID, providerCategoryTypeID,
                 practiceTypeID, requestedEffectiveDate, taxonomyTypeID, zipCode, zipExt, createdDate, createdBy, workflowID, npiStartDate, npiEndDate,
                 endDate, applicationTypeID, waiverTypeID, workflowEventTypeID, waiverServiceUpdateTypeID, gender);
        }


        public DataSet SelectTaxonomyInfoByCode(string taxonomyCode, int providerTypeID)
        {
            return base.Channel.SelectTaxonomyInfoByCode(taxonomyCode, providerTypeID);
        }

        public System.Data.DataSet SelectReferral_UnusedByTaxID(string taxID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectReferral_UnusedByTaxID(taxID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectDIDDServicesByWaiverID(int waiverID)
        {
            return base.Channel.SelectDIDDServicesByWaiverID(waiverID);
        }
        public System.Data.DataSet SelectQuestionTypesByIDBeginsWith(string beginsWith)
        {
            return base.Channel.SelectQuestionTypesByIDBeginsWith(beginsWith);
        }

        public System.Data.DataSet SelectRegAffiliationData(int regAffiliationID, string tableName)
        {
            return base.Channel.SelectRegAffiliationData(regAffiliationID, tableName);
        }

        public void InsertUpdateRegAffiliationQuestions(int regAffiliationID, Dictionary<string, string> parms)
        {
            base.Channel.InsertUpdateRegAffiliationQuestions(regAffiliationID, parms);
        }

        public System.Data.DataSet SelectProviderLicenseByPartyID(int partyId)
        {
            return base.Channel.SelectProviderLicenseByPartyID(partyId);
        }

        public System.Data.DataSet SelectProviderSpecialtyByPartyID(int partyId)
        {
            return base.Channel.SelectProviderSpecialtyByPartyID(partyId);
        }

        public void DeletePageConfiguration(int PageConfigurationID)
        {
            base.Channel.DeletePageConfiguration(PageConfigurationID);
        }

        public void DeleteRegAffiliation(int regAffiliationID)
        {
            base.Channel.DeleteRegAffiliation(regAffiliationID);
        }

        public System.Data.DataSet SelectScreeningActivityStatusByActivityTypeID(int activityTypeID)
        {
            return base.Channel.SelectScreeningActivityStatusByActivityTypeID(activityTypeID);
        }

        public System.Data.DataSet SelectImmigrationStatuses()
        {
            return base.Channel.SelectImmigrationStatuses();
        }

        public System.Data.DataSet SelectCitizenshipTypes()
        {
            return base.Channel.SelectCitizenshipTypes();
        }

        public System.Data.DataSet SelectCertifiedBeds()
        {
            return base.Channel.SelectCertifiedBeds();
        }

        public void UpdateDIDDReferralStartDate(int regID, DateTime startDate)
        {
            base.Channel.UpdateDIDDReferralStartDate(regID, startDate);
        }

        public System.Data.DataSet SelectPaperRequestTypes()
        {
            return base.Channel.SelectPaperRequestTypes();
        }

        public System.Data.DataSet SelectPaperDocumentTypes()
        {
            return base.Channel.SelectPaperDocumentTypes();
        }

        public System.Data.DataSet SelectPaperRequestStatusTypes()
        {
            return base.Channel.SelectPaperRequestStatusTypes();
        }

        public System.Data.DataSet SelectPaperRequestQueueByUserID(Guid userID, int paperRequestTypeID, string sortColumn, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.SelectPaperRequestQueueByUserID(userID, paperRequestTypeID, sortColumn, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public System.Data.DataSet SelectProviderOperatorDashboardByUserID(Guid userID)
        {
            return base.Channel.SelectProviderOperatorDashboardByUserID(userID);
        }

        public System.Data.DataSet SelectPaperRequestQueueByQueueID(int queueID)
        {
            return base.Channel.SelectPaperRequestQueueByQueueID(queueID);
        }

        public System.Data.DataSet SelectPaperRequestErrorsByQueueID(int queueID)
        {
            return base.Channel.SelectPaperRequestErrorsByQueueID(queueID);
        }

        public System.Data.DataSet SelectPaperRequestMatchData(int queueID)
        {
            return base.Channel.SelectPaperRequestMatchData(queueID);
        }

        public System.Data.DataSet SelectPaperRequestByDocumentHandle(int documentHandleID)
        {
            return base.Channel.SelectPaperRequestByDocumentHandle(documentHandleID);
        }

        public System.Data.DataSet SelectPaperRequestDocumentsByQueueID(int paperRequestQueueID)
        {
            return base.Channel.SelectPaperRequestDocumentsByQueueID(paperRequestQueueID);
        }


        public int InsertPaperRequestQueue(int requestTypeID, int documentTypeID, int documentHandle, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
                int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime createdOn, Guid createdBy)
        {
            return base.Channel.InsertPaperRequestQueue(requestTypeID, documentTypeID, documentHandle, requestStatusTypeID, applicationTypeID, providerTypeID, specialtyTypeID,
                taxonomyTypeID, taxonomyTypeCode, taxID, NPI, medicaidID, zip, zipExt, comments, createdOn, createdBy);
        }

        public void UpdatePaperRequestQueue(int paperRequestID, int requestTypeID, int documentTypeID, int requestStatusTypeID, int applicationTypeID, int providerTypeID, int specialtyTypeID,
                int taxonomyTypeID, string taxonomyTypeCode, string taxID, string NPI, string medicaidID, string zip, string zipExt, string comments, DateTime modifiedOn, Guid modifiedBy)
        {
            base.Channel.UpdatePaperRequestQueue(paperRequestID, requestTypeID, documentTypeID, requestStatusTypeID, applicationTypeID, providerTypeID, specialtyTypeID,
                taxonomyTypeID, taxonomyTypeCode, taxID, NPI, medicaidID, zip, zipExt, comments, modifiedOn, modifiedBy);
        }

        public int InsertPaperRequestError(int paperRequestQueueID, int errorTypeID, int errorStatusTypeID, DateTime createdOn, Guid createdBy)
        {
            return base.Channel.InsertPaperRequestError(paperRequestQueueID, errorTypeID, errorStatusTypeID, createdOn, createdBy);
        }

        public int InsertPaperRequestDocument(int requestID, string name, string description, string fileName, DateTime createdOn, Guid createdBy)
        {
            return base.Channel.InsertPaperRequestDocument(requestID, name, description, fileName, createdOn, createdBy);
        }

        public void ValidatePaperRequest(int paperRequestID, DateTime createdOn, Guid createdBy)
        {
            base.Channel.ValidatePaperRequest(paperRequestID, createdOn, createdBy);
        }

        public int SelectNextUnassignedPaperRequest(Guid userID, DateTime requestedOn)
        {
            return base.Channel.SelectNextUnassignedPaperRequest(userID, requestedOn);
        }

        public bool VerifyDuplicateTaxonomySpecialtyForReg(int regId, int taxonomyTypeId, int specialtyTypeId)
        {
            return base.Channel.VerifyDuplicateTaxonomySpecialtyForReg(regId, taxonomyTypeId, specialtyTypeId);
        }

        public bool VerifyDuplicateSpecialtyForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate)
        {
            return base.Channel.VerifyDuplicateSpecialtyForReg(regId, specialtyTypeId, regspecialtyid, startDate);
        }
        public bool VerifyFutureDatedSpecialtyEnrollmentForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate)
        {
            return base.Channel.VerifyFutureDatedSpecialtyEnrollmentForReg(regId, specialtyTypeId, regspecialtyid, startDate);
        }
        public DataSet VerifyIfCreatesInvalidSpecialtySpanForReg(int regId, int specialtyTypeId, int regspecialtyid, DateTime startDate, DateTime endDate)
        {
            return base.Channel.VerifyIfCreatesInvalidSpecialtySpanForReg(regId, specialtyTypeId, regspecialtyid, startDate, endDate);
        }
        public bool VerifyActiveSpecialtyForRegBySpecialtyTypeID(int regId, int specialtyTypeId, DateTime startDate)
        {
            return base.Channel.VerifyActiveSpecialtyForRegBySpecialtyTypeID(regId, specialtyTypeId, startDate);
        }

        public void SetAddBCITextRTPEmailFlag(int regId, int specialtyTypeId)
        {
            base.Channel.SetAddBCITextRTPEmailFlag(regId, specialtyTypeId);
        }
        public bool VerifyDuplicateTaxonomyForReg(int regId, string taxonomyCode, int regTaxonomyId)
        {
            return base.Channel.VerifyDuplicateTaxonomyForReg(regId, taxonomyCode, regTaxonomyId);
        }

        public void LinkPaperRequestQueueToReg(int paperRequestID, int regID, DateTime modifiedOn, Guid modifiedBy)
        {
            base.Channel.LinkPaperRequestQueueToReg(paperRequestID, regID, modifiedOn, modifiedBy);
        }

        public DataSet DisenrollProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, bool isFromJob = true)
        {
            return base.Channel.DisenrollProvider(regID, termDate, comments, modifiedOn, modifiedBy, insertTransaction, enrollmentStatusCode, insertPreviousEnrollmentSpan, isFromJob);
        }

        public DataSet SuspendProvider(int regID, DateTime termDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, string enrollmentStatusCode = "", bool insertPreviousEnrollmentSpan = false, string enrollmentStatusReason = "")
        {
            return base.Channel.SuspendProvider(regID, termDate, comments, modifiedOn, modifiedBy, insertTransaction, enrollmentStatusCode, insertPreviousEnrollmentSpan, enrollmentStatusReason);
        }
        public void ReactivateProvider(int regID, DateTime newEffectiveDate, DateTime revalidationDate, string comments, string enrollmentStatusCode, DateTime modifiedOn, Guid modifiedBy)
        {
            base.Channel.ReactivateProvider(regID, newEffectiveDate, revalidationDate, comments, enrollmentStatusCode, modifiedOn, modifiedBy);
        }

        public void RetroEffectiveDateProvider(int regID, DateTime effectiveDate, string comments, DateTime modifiedOn, Guid modifiedBy, bool insertTransaction, DateTime newRevalDate)
        {
            base.Channel.RetroEffectiveDateProvider(regID, effectiveDate, comments, modifiedOn, modifiedBy, insertTransaction, newRevalDate);
        }

        public DataSet InsertExpressTerminatationWorkflow(int regID, DateTime createdOn, Guid createdBy)
        {
            return base.Channel.InsertExpressTerminatationWorkflow(regID, createdOn, createdBy);
        }


        public bool GenerateDIDDContract(int regID, out string filename)
        {
            return base.Channel.GenerateDIDDContract(regID, out filename);
        }

        public bool GenerateICFIIDContract(int regID, out string filename)
        {
            return base.Channel.GenerateICFIIDContract(regID, out filename);
        }

        public bool GenerateApplication(int regID, string userName, bool saveDocument, out string filename)
        {
            return base.Channel.GenerateApplication(regID, userName, saveDocument, out filename);
        }

        public DataSet PerformMoratoriaRematch(int regID, DateTime modifiedOn, Guid modifiedBy)
        {
            return base.Channel.PerformMoratoriaRematch(regID, modifiedOn, modifiedBy);
        }

        public DataSet SelectCountiesByStateAbbreviation(string stateAbbreviation)
        {
            return base.Channel.SelectCountiesByStateAbbreviation(stateAbbreviation);
        }

        public DataSet SelectReports()
        {
            return base.Channel.SelectReports();
        }

        public DataSet SelectTaxonomyTypeByID(int taxonomyTypeID)
        {
            return base.Channel.SelectTaxonomyTypeByID(taxonomyTypeID);
        }

        public System.Data.DataSet SelectDiddReferralLocationTypes()
        {
            return base.Channel.SelectDiddReferralLocationTypes();
        }

        public System.Data.DataSet SelectGroupMemberProvidersByAffiliationID(int affiliationID)
        {
            return base.Channel.SelectGroupMemberProvidersByAffiliationID(affiliationID);
        }

        public System.Data.DataSet SelectProviderTypesByMMISProviderTypeID(string mmisProviderTypeID)
        {
            return base.Channel.SelectProviderTypesByMMISProviderTypeID(mmisProviderTypeID);
        }

        public void DeleteRegistration(int regID, bool allowPostApplicationCompleteDelete)
        {
            base.Channel.DeleteRegistration(regID, allowPostApplicationCompleteDelete);
        }

        public void AdminUpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             string taxid, int taxIDTypeID, string npi,
            int taxonomyTypeID, string zipCode, string zipExt, string gender, DateTime modifiedOn, Guid modifiedBy, DateTime? birthDate)
        {
            base.Channel.AdminUpdateRegistrationKeyFields(regID, providerName, firstName, middleName, lastName,
             taxid, taxIDTypeID, npi, taxonomyTypeID,
              zipCode, zipExt, gender, modifiedOn, modifiedBy, birthDate);
        }
        public void UpdateRegistrationKeyFields(int regID, string providerName, string firstName, string middleName, string lastName,
             int taxIDTypeID, string npi, int providerTypeID, int providerCategoryTypeID, int? practiceTypeID, int specialtyTypeID,
            int taxonomyTypeID, string practiceName, string zipCode, string zipExt, string gender, DateTime modifiedDate, Guid modifiedBy,
            DateTime npiStartDate, DateTime? npiEndDate)
        {
            base.Channel.UpdateRegistrationKeyFields(regID, providerName, firstName, middleName, lastName,
             taxIDTypeID, npi, providerTypeID, providerCategoryTypeID, practiceTypeID, specialtyTypeID, taxonomyTypeID,
             practiceName, zipCode, zipExt, gender, modifiedDate, modifiedBy, npiStartDate, npiEndDate);
        }

        public System.Data.DataSet SelectBackgroundStatusTypes()
        {
            return base.Channel.SelectBackgroundStatusTypes();
        }

        public System.Data.DataSet SelectBackgroundVerificationStatusTypes()
        {
            return base.Channel.SelectBackgroundVerificationStatusTypes();
        }

        public System.Data.DataSet SelectBackgroundResultTypes()
        {
            return base.Channel.SelectBackgroundResultTypes();
        }

        public System.Data.DataSet SelectBackgroundPerformedByTypes()
        {
            return base.Channel.SelectBackgroundPerformedByTypes();
        }

        public System.Data.DataSet SelectOrientationStatusTypes()
        {
            return base.Channel.SelectOrientationStatusTypes();
        }

        public DataSet GetCallTrackingReasons()
        {
            return base.Channel.GetCallTrackingReasons();
        }

        public DataSet GetCallTrackingSources()
        {
            return base.Channel.GetCallTrackingSources();
        }

        public DataSet GetCallTrackingNextActions()
        {
            return base.Channel.GetCallTrackingNextActions();
        }

        public DataSet GetCallTrackingResolutions()
        {
            return base.Channel.GetCallTrackingResolutions();
        }

        public int InsertCallTrackingCall(int sourceID, int subjectID, int nextActionID, int resolutionID, DateTime startTime, DateTime endTime, TimeSpan duration, string regID, string callerOther, string reasonOther, string callDetails, string NPI, string medicaidID, DateTime createdOn, Guid createdBy)
        {
            return base.Channel.InsertCallTrackingCall(sourceID, subjectID, nextActionID, resolutionID, startTime, endTime, duration, regID, callerOther, reasonOther, callDetails, NPI, medicaidID, createdOn, createdBy);
        }

        public DataSet GetLTCReviewTypes()
        {
            return base.Channel.GetLTCReviewTypes();
        }

        public DataSet SelectCallsByRange(DateTime beginDate, DateTime endDate, int sourceID, int subjectID, int nextActionID, int resolutionID, int callID, string NPI, string medicaidID)
        {
            return base.Channel.SelectCallsByRange(beginDate, endDate, sourceID, subjectID, nextActionID, resolutionID, callID, NPI, medicaidID);
        }

        public int GetWorkflowInstance(int? applicationTypeID, int? providerCategoryTypeID, int? providerTypeID, int? referralTypeID,
            bool revalDue, bool referral, bool conversion, bool groupMember)
        {
            return base.Channel.GetWorkflowInstance(applicationTypeID, providerCategoryTypeID, providerTypeID, referralTypeID, revalDue,
                referral, conversion, groupMember);
        }

        public DataSet SelectRegSectionUploadControl(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id)
        {
            return base.Channel.SelectRegSectionUploadControl(regPageTypeId, applicationTypeID, providerTypeId, ProviderCategoryTypeId, regPageSection, reg_id);
        }

        public DataSet SelectRegSectionUploadControlandDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id, int rowId)
        {
            return base.Channel.SelectRegSectionUploadControlandDocument(regPageTypeId, applicationTypeID, providerTypeId, ProviderCategoryTypeId, regPageSection, reg_id, rowId);
        }

        public DataSet SelecIncidentComplianceDocument(int regPageTypeId, int? applicationTypeID, int? providerTypeId, int? ProviderCategoryTypeId, string regPageSection, int? reg_id, int rowId, string IncidentCaseNum)
        {
            return base.Channel.SelecIncidentComplianceDocument(regPageTypeId, applicationTypeID, providerTypeId, ProviderCategoryTypeId, regPageSection, reg_id, rowId, IncidentCaseNum);
        }

        public DataSet SelectRegSectionUploadDocument(int regPageTypeId, int regId, int? providerTypeId, string regPageSection, int rowId)
        {
            return base.Channel.SelectRegSectionUploadDocument(regPageTypeId, regId, providerTypeId, regPageSection, rowId);
        }


        public DataSet SelectLicenseRestrictionCodes()
        {
            return base.Channel.SelectLicenseRestrictionCodes();
        }

        public DataSet SelectConvertedDocuments(int regID, int pageNumber, int pageSize)
        {
            return base.Channel.SelectConvertedDocuments(regID, pageNumber, pageSize);
        }

        public DataSet SelectPaperDocuments(int regID)
        {
            return base.Channel.SelectPaperDocuments(regID);
        }

        public void UpateRegDocumentXref(int regId, int documentId, int rowId)
        {
            base.Channel.UpateRegDocumentXref(regId, documentId, rowId);
        }

        public void UpdateIncidentAlertFlag(int regId, DateTime changedDate, Guid userID)
        {
            base.Channel.UpdateIncidentAlertFlag(regId, changedDate, userID);
        }

        public void UpdateIncidentReviewStatus(int regId, int caseStatus, DateTime changedDate, Guid userID)
        {
            base.Channel.UpdateIncidentReviewStatus(regId, caseStatus, changedDate, userID);
        }

        public DataSet SelectCLIACertificateTypes()
        {
            return base.Channel.SelectCLIACertificateTypes();
        }
        public bool IsDDSProvider(int regID)
        {
            return base.Channel.IsDDSProvider(regID);
        }

        public void DeleteRegistrationDocument(int documentID)
        {
            base.Channel.DeleteRegistrationDocument(documentID);
        }

        public Guid CreatePasswordRequest(Guid userId)
        {
            return base.Channel.CreatePasswordRequest(userId);
        }



        public void NotifyPasswordResetEmail(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyPasswordResetEmail(recipients, username, pdmsUrl, regId, userid);
        }
        public DataSet IsActiveReset(string recordid)
        {
            return base.Channel.IsActiveReset(recordid);
        }

        public DataSet SelectWFStepInfo(int stepId)
        {
            return base.Channel.SelectWFStepInfo(stepId);
        }

        public DataSet VerifyCurrentAssignedUserForStep(Guid userId, int stepId)
        {
            return base.Channel.VerifyCurrentAssignedUserForStep(userId, stepId);
        }

        public DataSet SelectCurrentWFTaskInfo(int regId)
        {
            return base.Channel.SelectCurrentWFTaskInfo(regId);
        }

        public DataSet SelectTaxonomyTypesWithProviderSpecialty()
        {
            return base.Channel.SelectTaxonomyTypesWithProviderSpecialty();
        }

        public DataSet SelectAllSpecialtyTypes()
        {
            return base.Channel.SelectAllSpecialtyTypes();
        }

        public int InsertTaxonomyType(int specialtyTypeID, int providerTypeID, string taxonomyCode,
            string taxonomyName, DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser,
            string mmisSpecialtyTypeID, bool npiRequired)
        {
            return base.Channel.InsertTaxonomyType(specialtyTypeID, providerTypeID, taxonomyCode,
                    taxonomyName, expirationDate, lastModifiedDateTime, lastModifiedUser,
                    mmisSpecialtyTypeID, npiRequired);
        }

        public void UpdateTaxonomyType(int taxonomyTypeID, int specialtyTypeID, int providerTypeID, string taxonomyCode,
            string taxonomyName, DateTime expirationDate, DateTime lastModifiedDateTime, string lastModifiedUser,
            string mmisSpecialtyTypeID, bool npiRequired)
        {
            base.Channel.UpdateTaxonomyType(taxonomyTypeID, specialtyTypeID, providerTypeID, taxonomyCode,
                    taxonomyName, expirationDate, lastModifiedDateTime, lastModifiedUser,
                    mmisSpecialtyTypeID, npiRequired);
        }

        public DataSet SelectAllProviderTypes()
        {
            return base.Channel.SelectAllProviderTypes();
        }

        public DataSet SelectAllPAAssignProcedureGroups()
        {
            return base.Channel.SelectAllPAAssignProcedureGroups();
        }

        public DataSet SelectAllSpecialtyTypesUnfiltered()
        {
            return base.Channel.SelectAllSpecialtyTypesUnfiltered();
        }

        public DataSet SelectAllApplicationTypes()
        {
            return base.Channel.SelectAllApplicationTypes();
        }

        public int InsertSpecialtyType(string specialtyTypeName, DateTime lastModifiedDateTime, string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible)
        {
            return base.Channel.InsertSpecialtyType(specialtyTypeName, lastModifiedDateTime, lastModifiedUser, mmisSpecialtyTypeID, externalSpecialtyTypeName, isVisible);
        }

        public void UpdateSpecialtyType(int specialtyTypeID, string specialtyTypeName, DateTime lastModifiedDateTime, string lastModifiedUser, string mmisSpecialtyTypeID, string externalSpecialtyTypeName, bool isVisible)
        {
            base.Channel.UpdateSpecialtyType(specialtyTypeID, specialtyTypeName, lastModifiedDateTime, lastModifiedUser, mmisSpecialtyTypeID, externalSpecialtyTypeName, isVisible);
        }

        public void UpdateProviderType(int providerTypeID, string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
            string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID)
        {
            base.Channel.UpdateProviderType(providerTypeID, providerTypeAbbreviation, providerTypeName, lastModifiedDateTime, lastModifiedUser, isUsedInMMIS, providerCategoryTypeID,
                mmisProviderTypeID, requireNPI, providerRiskLevelID, applicationTypeID);
        }

        public int InsertProviderType(string providerTypeAbbreviation, string providerTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
         string isUsedInMMIS, int providerCategoryTypeID, string mmisProviderTypeID, bool requireNPI, int providerRiskLevelID, int applicationTypeID)
        {
            return base.Channel.InsertProviderType(providerTypeAbbreviation, providerTypeName, lastModifiedDateTime, lastModifiedUser, isUsedInMMIS, providerCategoryTypeID,
                mmisProviderTypeID, requireNPI, providerRiskLevelID, applicationTypeID);
        }

        public void UpdateProviderCategoryType(int providerCategoryTypeID, string providerCategoryTypeName,
            DateTime lastModifiedDateTime, string lastModifiedUser, bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource)
        {
            base.Channel.UpdateProviderCategoryType(providerCategoryTypeID, providerCategoryTypeName, lastModifiedDateTime,
                lastModifiedUser, isActivePhase2, mmisProviderCategoryTypeID, imageSource);
        }

        public int InsertProviderCategoryType(string providerCategoryTypeName, DateTime lastModifiedDateTime, string lastModifiedUser,
            bool isActivePhase2, string mmisProviderCategoryTypeID, string imageSource)
        {
            return base.Channel.InsertProviderCategoryType(providerCategoryTypeName, lastModifiedDateTime, lastModifiedUser, isActivePhase2, mmisProviderCategoryTypeID, imageSource);
        }

        public int InsertApplicationType(string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible)
        {
            return base.Channel.InsertApplicationType(applicationTypeName, applicationTypeDescription, isUsedInMMIS, mmisApplicationTypeID,
                lastModifiedDateTime, lastModifiedUser, isVisible);
        }


        public void UpdateApplicationType(int applicationTypeID, string applicationTypeName, string applicationTypeDescription,
            bool isUsedInMMIS, string mmisApplicationTypeID, DateTime lastModifiedDateTime, string lastModifiedUser, bool isVisible)
        {
            base.Channel.UpdateApplicationType(applicationTypeID, applicationTypeName, applicationTypeDescription, isUsedInMMIS, mmisApplicationTypeID,
                lastModifiedDateTime, lastModifiedUser, isVisible);
        }

        public int InsertPAAssignProcedureGrp(string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd, string createdByUser, DateTime createdOnDate, DateTime lastModifiedDate)
        {
            return base.Channel.InsertPAAssignProcedureGrp(cdePAAssign, dsc50, procFrom, procTo, procFromOrder, procToOrder, dteEffective, dteEnd, createdByUser, createdOnDate, lastModifiedDate);
        }

        public void UpdatePAAssignProcedureGrp(int id, string cdePAAssign, string dsc50, string procFrom, string procTo, int procFromOrder, int procToOrder, DateTime dteEffective, DateTime dteEnd)
        {
            base.Channel.UpdatePAAssignProcedureGrp(id, cdePAAssign, dsc50, procFrom, procTo, procFromOrder, procToOrder, dteEffective, dteEnd);
        }

        public DataSet GetClaimProcEffectiveDate(string procCode)
        {
            return base.Channel.GetClaimProcEffectiveDate(procCode);
        }
        public DataSet SelectRegPageSettingActionAll()
        {
            return base.Channel.SelectRegPageSettingActionAll();
        }

        public DataSet SelectRegPageTypeAll()
        {
            return base.Channel.SelectRegPageTypeAll();
        }

        public DataSet SelectRegPageSettingAll(string filterExpression)
        {
            return base.Channel.SelectRegPageSettingAll(filterExpression);
        }

        public DataSet SelectRegSectionUploadControlAll()
        {
            return base.Channel.SelectRegSectionUploadControlAll();
        }

        public DataSet SelectProviderTypeFeeAll()
        {
            return base.Channel.SelectProviderTypeFeeAll();
        }

        public DataSet SelectApplicationFeePaymentTypeAll()
        {
            return base.Channel.SelectApplicationFeePaymentTypeAll();
        }

        public DataSet SelectPaperRequestDocumentTypeAll()
        {
            return base.Channel.SelectPaperRequestDocumentTypeAll();
        }

        public DataSet SelectRegEnrollmentByRegID(int regId)
        {
            return base.Channel.SelectRegEnrollmentByRegID(regId);
        }
        public DataSet SelectRegSectionTypeAll()
        {
            return base.Channel.SelectRegSectionTypeAll();
        }

        public DataSet UpdateRegForReactivateByProvider(int regId, DateTime modifiedOn, string modifiedBy, int workflowID)
        {
            return base.Channel.UpdateRegForReactivateByProvider(regId, modifiedOn, modifiedBy, workflowID);
        }
        public int InsertApplicationFeePaymentType(string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            return base.Channel.InsertApplicationFeePaymentType(paymentTypeName, lastModifiedDate, lastModifiedUser);
        }

        public int InsertPaperRequestDocumentType(string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode)
        {
            return base.Channel.InsertPaperRequestDocumentType(paperRequestDocumentTypeName, lastModifiedDate, lastModifiedUser, paperRequestDocumentTypeDescription, paperRequestDocumentTypeOnbaseCode);
        }

        public int InsertProviderTypeFee(int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
             Guid lastModifiedUser, int entityTypeID, int applicationTypeID)
        {
            return base.Channel.InsertProviderTypeFee(providerTypeID, isFeeRequired, feeAmount, lastModifiedDate, lastModifiedUser, entityTypeID, applicationTypeID);
        }

        public int InsertRegPageSettingAction(string regPageName, string roleName, bool takeActionAllowed,
            DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            return base.Channel.InsertRegPageSettingAction(regPageName, roleName, takeActionAllowed, lastModifiedDate, lastModifiedUser);
        }

        public int InsertRegPageSetting(int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired)
        {
            return base.Channel.InsertRegPageSetting(entityTypeID, providerTypeID, regPageName, regPageSection, isVisible, isEditable, lastModifiedDate, lastModifiedUser, taskName,
                isRequired, applicationTypeID, regPageTypeID, regSectionTypeID, isReviewRequired);
        }

        public int InsertRegPageType(string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser, int? sequenceID)
        {
            return base.Channel.InsertRegPageType(regPageName, lastModifiedDate, lastModifiedUser, sequenceID);
        }

        public int InsertRegSectionUploadControl(int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
         int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
            string regPageSection, string regPageName)
        {
            return base.Channel.InsertRegSectionUploadControl(applicationTypeID, providerCategoryTypeID, providerTypeID, regPageTypeID,
                title, description, isRequired, lastModifiedDate, lastModifiedUser, regPageSection, regPageName);
        }

        public void UpdateApplicationFeePaymentType(int applicationFeePaymentTypeID, string paymentTypeName, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            base.Channel.UpdateApplicationFeePaymentType(applicationFeePaymentTypeID, paymentTypeName, lastModifiedDate, lastModifiedUser);
        }

        public void UpdatePaperRequestDocumentType(int paperRequestDocumentTypeID, string paperRequestDocumentTypeName,
            DateTime lastModifiedDate, Guid lastModifiedUser, string paperRequestDocumentTypeDescription,
            string paperRequestDocumentTypeOnbaseCode)
        {
            base.Channel.UpdatePaperRequestDocumentType(paperRequestDocumentTypeID, paperRequestDocumentTypeName, lastModifiedDate, lastModifiedUser, paperRequestDocumentTypeDescription,
                paperRequestDocumentTypeOnbaseCode);
        }

        public void UpdateProviderTypeFee(int providerTypeFeeID, int providerTypeID, bool isFeeRequired, decimal feeAmount, DateTime lastModifiedDate,
            Guid lastModifiedUser, int entityTypeID, int applicationTypeID)
        {
            base.Channel.UpdateProviderTypeFee(providerTypeFeeID, providerTypeID, isFeeRequired, feeAmount, lastModifiedDate, lastModifiedUser, entityTypeID, applicationTypeID);
        }


        public void UpdateRegPageSettingAction(int regPageSettingActionID, string regPageName, string roleName, bool takeActionAllowed,
           DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            base.Channel.UpdateRegPageSettingAction(regPageSettingActionID, regPageName, roleName, takeActionAllowed, lastModifiedDate, lastModifiedUser);
        }

        public void UpdateRegPageSetting(int regPageSettingID, int entityTypeID, int providerTypeID, string regPageName,
            string regPageSection, bool isVisible, bool isEditable, DateTime lastModifiedDate, Guid lastModifiedUser,
            string taskName, bool isRequired, int applicationTypeID, int regPageTypeID, int regSectionTypeID, bool isReviewRequired)
        {
            base.Channel.UpdateRegPageSetting(regPageSettingID, entityTypeID, providerTypeID, regPageName, regPageSection, isVisible, isEditable, lastModifiedDate,
                lastModifiedUser, taskName, isRequired, applicationTypeID, regPageTypeID, regSectionTypeID, isReviewRequired);
        }

        public void UpdateRegPageType(int regPageTypeID, string regPageName, DateTime lastModifiedDate, Guid lastModifiedUser,
            int? sequenceID)
        {
            base.Channel.UpdateRegPageType(regPageTypeID, regPageName, lastModifiedDate, lastModifiedUser, sequenceID);
        }

        public void UpdateRegSectionUploadControl(int regSectionUploadControlID, int applicationTypeID, int providerCategoryTypeID, int providerTypeID,
            int regPageTypeID, string title, string description, bool isRequired, DateTime lastModifiedDate, Guid lastModifiedUser,
                string regPageSection, string regPageName)
        {
            base.Channel.UpdateRegSectionUploadControl(regSectionUploadControlID, applicationTypeID, providerCategoryTypeID, providerTypeID, regPageTypeID,
                title, description, isRequired, lastModifiedDate, lastModifiedUser, regPageSection, regPageName);
        }

        public DataSet SelectAppSettingsAll()
        {

            return base.Channel.SelectAppSettingsAll();

        }

        public DataSet SelectAllAutomatedReports()
        {

            return base.Channel.SelectAllAutomatedReports();

        }

        public DataSet SelectDataFixTablesAll()
        {

            return base.Channel.SelectDataFixTablesAll();

        }

        public DataSet SelectWebAPITestingAll()
        {

            return base.Channel.SelectWebAPITestingAll();

        }


        public void UpdateAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite)
        {
            base.Channel.UpdateAppSettings(appSettingsKey, appSettingsValue, appSettingsReadOnly, lastUpdatedDate, lastActivityUserId, appSettingsNotes, appSettingsEnvironSpecific, CanOverwrite);
        }

        public void InsertAppSettings(string appSettingsKey, string appSettingsValue, bool appSettingsReadOnly, DateTime lastUpdatedDate, Guid lastActivityUserId, string appSettingsNotes, bool appSettingsEnvironSpecific, bool CanOverwrite)
        {
            base.Channel.InsertAppSettings(appSettingsKey, appSettingsValue, appSettingsReadOnly, lastUpdatedDate, lastActivityUserId, appSettingsNotes, appSettingsEnvironSpecific, CanOverwrite);
        }

        public void InsertDataFixTables(string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            base.Channel.InsertDataFixTables(tableName, tableValue, tableType, isVisible, pkColumns, columsHide, readonlyColumns, lastmodifiedDate, lastmodifiedUser);
        }

        public void InsertWebAPITesting(string APIName, string APIDescp, string APIXML, bool APIEnabled)
        {
            base.Channel.InsertWebAPITesting(APIName, APIDescp, APIXML, APIEnabled);
        }

        public void UpdateWebAPITesting(int id, string APIName, string APIDescp, string APIXML, bool APIEnabled)
        {
            base.Channel.UpdateWebAPITesting(id, APIName, APIDescp, APIXML, APIEnabled);
        }

        public void InsertUpdateAutomatedReportsMain(Dictionary<string, object> parms)
        {
            base.Channel.InsertUpdateAutomatedReportsMain(parms);
        }
        public void InsertDynamicFieldConfiguration(Dictionary<string, string> parms)
        {
            base.Channel.InsertDynamicFieldConfiguration(parms);
        }
        public void InsertUIControlVisibilityConfig(Dictionary<string, string> parms)
        {
            base.Channel.InsertUIControlVisibilityConfig(parms);
        }
        public DataSet SelectDynamicFieldDisplayName(Dictionary<string, string> parms)
        {
            return base.Channel.SelectDynamicFieldDisplayName(parms);
        }
        public DataSet SelectDynamicFieldConfiguration(Dictionary<string, string> parms)
        {
            return base.Channel.SelectDynamicFieldConfiguration(parms);
        }
        public DataSet SelectUIControlVisibility(Dictionary<string, string> parms)
        {
            return base.Channel.SelectUIControlVisibility(parms);
        }
        public DataSet SelectAutomatedReportsSub(Guid ReportID)
        {
            return base.Channel.SelectAutomatedReportsSub(ReportID);
        }

        public void UpdateDataFixTables(int id, string tableName, string tableValue, string tableType,
            bool isVisible, string pkColumns, string columsHide, string readonlyColumns, DateTime lastmodifiedDate, Guid lastmodifiedUser)
        {
            base.Channel.UpdateDataFixTables(id, tableName, tableValue, tableType, isVisible, pkColumns, columsHide, readonlyColumns, lastmodifiedDate, lastmodifiedUser);
        }

        public void DeleteDataFixTable(int id)
        {
            base.Channel.DeleteDataFixTable(id);
        }

        public DataSet GetAlertsByTaxID(string taxID)
        {
            return base.Channel.GetAlertsByTaxID(taxID);
        }

        public DataSet InsertIntoVersionTables(int RegID, string userId)
        {
            return base.Channel.InsertIntoVersionTables(RegID, userId);
        }

        public DataSet SelectTasksByRole(string role)
        {
            return base.Channel.SelectTasksByRole(role);
        }

        public void UpdateTaskRankForRole(string role, int taskId, int rank)
        {
            base.Channel.UpdateTaskRankForRole(role, taskId, rank);
        }


        public DataSet SelectWorkflowRoles()
        {
            return base.Channel.SelectWorkflowRoles();
        }

        public DataSet GetWorkflowProcessesForRegID(int regId)
        {
            return base.Channel.GetWorkflowProcessesForRegID(regId);
        }

        public DataSet SelectAllSections()
        {
            return base.Channel.SelectAllSections();
        }

        public System.Data.DataSet SelectTimesApplicationReturnedToProviderFromLTC(int regID)
        {
            return base.Channel.SelectTimesApplicationReturnedToProviderFromLTC(regID);
        }

        public System.Data.DataSet SelectMCOAffiliationStatuses()
        {
            return base.Channel.SelectMCOAffiliationStatuses();
        }
        public System.Data.DataSet SelectMCOs()
        {
            return base.Channel.SelectMCOs();
        }
        public void DeleteRegMCOAffiliation(int regMCOAffiliationID)
        {
            base.Channel.DeleteRegMCOAffiliation(regMCOAffiliationID);
        }
        public void UpdateToConvertToFeeForService(int currentRegID)
        {
            base.Channel.UpdateToConvertToFeeForService(currentRegID);
        }
        public bool HasNewOwnersOrChangeInOwnerInformation(int regID)
        {
            return base.Channel.HasNewOwnersOrChangeInOwnerInformation(regID);
        }
        public bool HasPrimaryPracticeLocationChange(int regID)
        {
            return base.Channel.HasPrimaryPracticeLocationChange(regID);
        }
        public DataSet SelectProviderScreeningByProcessID(int processID)
        {
            return base.Channel.SelectProviderScreeningByProcessID(processID);
        }
        //public DataSet SelectServiceLocationByMedicaidID(string medicaidID)
        //{
        //    return base.Channel.SelectServiceLocationByMedicaidID(medicaidID);
        //}
        public System.Data.DataSet GetUserRoleCompatibility()
        {
            return base.Channel.GetUserRoleCompatibility();
        }
        public System.Data.DataSet GetAllUserRoles()
        {
            return base.Channel.GetAllUserRoles();
        }
        public System.Data.DataSet GetUserRolesByUser(string userName)
        {
            return base.Channel.GetUserRolesByUser(userName);
        }
        public System.Data.DataSet GetLoweredUserNameByUserName(string userName, string userId)
        {
            return base.Channel.GetLoweredUserNameByUserName(userName, userId);
        }

        public System.Data.DataSet GetUserRolesByOHID(string ohid)
        {
            return base.Channel.GetUserRolesByOHID(ohid);
        }

        public DataSet IsRegistrationWentThroughAppeals(int RegID)
        {
            return base.Channel.IsRegistrationWentThroughAppeals(RegID);
        }
        public DataSet GetWorkflowSteps(int workflowId)
        {
            return base.Channel.GetWorkflowSteps(workflowId);
        }
        public DataSet GetWorkflowRoles(int workflowId)
        {
            return base.Channel.GetWorkflowRoles(workflowId);
        }
        public DataSet GetEducationTypes()
        {
            return base.Channel.GetEducationTypes();
        }
        public DataSet GetDegreeAwardTypes()
        {
            return base.Channel.GetDegreeAwardTypes();
        }
        public void InsertProviderCredentialing(int regID, int workflowID, string userID)
        {
            base.Channel.InsertProviderCredentialing(regID, workflowID, userID);
        }
        public DataSet SelectProviderCredentialingData(int regId)
        {
            return base.Channel.SelectProviderCredentialingData(regId);
        }
        public DataSet SelectProviderCredentialActivityData(int regID, int credentialingID)
        {
            return base.Channel.SelectProviderCredentialActivityData(regID, credentialingID);
        }
        public DataSet SelectDataRankTypes()
        {
            return base.Channel.SelectDataRankTypes();
        }
        public DataSet SelectProviderCredentialActivityMatchData(int credentialActivityID, int regID)
        {
            return base.Channel.SelectProviderCredentialActivityMatchData(credentialActivityID, regID);
        }
        //public void UpdateCredentialActivityDataRank(int credentialActivityID, int datarankId, string adverseAction, DateTime? changedDate, string changedBy)
        //{
        //    base.Channel.UpdateCredentialActivityDataRank(credentialActivityID, datarankId, adverseAction, changedDate, changedBy);
        //}
        public DataSet SelectCredentialingCommitteeActivity(int regID)
        {
            return base.Channel.SelectCredentialingCommitteeActivity(regID);
        }
        public void UpdateCredentialingCommitteeMember(int credentialingId, int actionStatusId, string memberUserName, string comments, DateTime actionDate, DateTime? changedDate, string userID)
        {
            base.Channel.UpdateCredentialingCommitteeMember(credentialingId, actionStatusId, memberUserName, comments, actionDate, changedDate, userID);
        }
        public DataSet GetCommitteeActivityStatuses()
        {
            return base.Channel.GetCommitteeActivityStatuses();
        }
        public string GetReviewStatusByCommiteeMember(int regId, string userName)
        {
            return base.Channel.GetReviewStatusByCommiteeMember(regId, userName);
        }
        public void UpdateCredentialStatus(int regID, int statusID, string changedBy)
        {
            base.Channel.UpdateCredentialStatus(regID, statusID, changedBy);
        }

        public void UpdateCredentialDiscontinueDate(int regID, string changedBy)
        {
            base.Channel.UpdateCredentialDiscontinueDate(regID, changedBy);
        }

        public void UpdateCredentialResult(int regId, int resultID, string changedBy)
        {
            base.Channel.UpdateCredentialResult(regId, resultID, changedBy);
        }
        public System.Data.DataSet WF_SelectUnassignedStepsByUserProviderType(string groupName, string userId)
        {
            return base.Channel.WF_SelectUnassignedStepsByUserProviderType(groupName, userId);
        }

        public bool HasPendingCredentialActivities(int regId)
        {
            return base.Channel.HasPendingCredentialActivities(regId);
        }
        public void updateWF_STEP_OwnerWithStartDate(int stepId, string ownerId, DateTime stepStartDate)
        {
            base.Channel.updateWF_STEP_OwnerWithStartDate(stepId, ownerId, stepStartDate);
        }
        public DataSet GetRegistrationAutoApproveSections(int regId)
        {
            return base.Channel.GetRegistrationAutoApproveSections(regId);
        }
        public DataSet WF_SelectActiveOwnerStepsCredentialProvider(string ownerID, bool includeOtherSteps, string ownerRole)
        {
            return base.Channel.WF_SelectActiveOwnerStepsCredentialProvider(ownerID, includeOtherSteps, ownerRole);
        }
        public int InsertTaxonomyCode(int specialtyTypeID, int providerTypeID, string taxonomyCode, string taxonomyDetail, DateTime expirationdate, string mmis_specialty_type_id, DateTime lastModifiedDate, Guid lastModifieduserID)
        {
            return base.Channel.InsertTaxonomyCode(specialtyTypeID, providerTypeID, taxonomyCode, taxonomyDetail, expirationdate, mmis_specialty_type_id, lastModifiedDate, lastModifieduserID);
        }

        public MAXIMUS.Core.Libraries.NPPESAPIResult ValidNPIinNPPESApi(long npi)
        {
            return base.Channel.ValidNPIinNPPESApi(npi);
        }

        public bool ValidateNPIUniqueness(int regId, string npi, bool isNursingFacility)
        {
            return base.Channel.ValidateNPIUniqueness(regId, npi, isNursingFacility);
        }

        public DataSet SelectTaxonomyTypesToExclude(int providerTypeID)
        {
            return base.Channel.SelectTaxonomyTypesToExclude(providerTypeID);
        }
        public System.Data.DataSet SelectTaskGroupByTaskName(string taskName)
        {
            return base.Channel.SelectTaskGroupByTaskName(taskName);
        }

        public DataSet SelectProviderAddressInfo(int regId, int addressTypeId)
        {
            return base.Channel.SelectProviderAddressInfo(regId, addressTypeId);
        }

        public DataSet SelectProviderReturnStatus(int regId)
        {
            return base.Channel.SelectProviderReturnStatus(regId);
        }

        public DataSet SelectProviderAddressLocationInfo(int regId, int sectionTypeId)
        {
            return base.Channel.SelectProviderAddressLocationInfo(regId, sectionTypeId);
        }

        public DataSet GetPowerAgentByUserId(string loggedinUserID, int pageSize, int startRowIndex,string providerAdminUserID, out int totalResultCount)
        {
            return base.Channel.GetPowerAgentByUserId(loggedinUserID, pageSize, startRowIndex,providerAdminUserID, out totalResultCount);
        }

        public DataSet GetProviderAdminsForPowerAgent(string poweragentUserId)
        {
            return base.Channel.GetProviderAdminsForPowerAgent(poweragentUserId);
        }

        public DataSet AddPowerAgent(string providerAdminUserId, string OHID, bool hasAccessManagement, string createdByUserId, bool overrideValidation)
        {
            return base.Channel.AddPowerAgent(providerAdminUserId, OHID, hasAccessManagement, createdByUserId, overrideValidation);
        }

        public DataSet DeactivatePowerAgent(string createdByUserId, string OHID)
        {
            return base.Channel.DeactivatePowerAgent(createdByUserId, OHID);
        }

        public DataSet UpdateAccessManagementForPowerAgent(string createdByUserId, string OHID, bool hasAccessManagement)
        {
            return base.Channel.UpdateAccessManagementForPowerAgent(createdByUserId, OHID, hasAccessManagement);
        }

        public DataSet ValidateGlobalAdminChange(string currentOHID, string newOHID)
        {
            return base.Channel.ValidateGlobalAdminChange(currentOHID, newOHID);
        }
            

        public DataSet GetCPCProviderAttestationControls(int regId)
        {
            return base.Channel.GetCPCProviderAttestationControls(regId);
        }

        public DataSet SelectLocationTypes(int sectionTypeId)
        {
            return base.Channel.SelectLocationTypes(sectionTypeId);
        }

        public DataSet SelectRegProviderInfo(int regId, int addressTypeId)
        {
            return base.Channel.SelectRegProviderInfo(regId, addressTypeId);
        }


        #region Password expiry rules

        public DataSet GetUsermembershipInfoByUserName(string username)
        {
            return base.Channel.GetUsermembershipInfoByUserName(username);
        }

        #endregion


        #region MASS EMAIL

        public EmailTemplate GetTemplateByName(string templateName)
        {
            return base.Channel.GetTemplateByName(templateName);
        }


        public EmailTemplate GetTemplateById(int templateId)
        {
            return base.Channel.GetTemplateById(templateId);
        }

        public bool AddEmailTempalte(string name, string body, string notes = null)
        {
            return base.Channel.AddEmailTempalte(name, body, notes);
        }

        public List<EmailTemplate> GetTAllemplates()
        {
            return base.Channel.GetTAllemplates();
        }

        public bool AddItemToQueue(EmailQueueItem item)
        {
            return base.Channel.AddItemToQueue(item);
        }

        public List<EmailQueueItem> GetQueueItemsToProcess(int batchId)
        {
            return base.Channel.GetQueueItemsToProcess(batchId);
        }

        public int BatchJobCreate(int recordCount, int templateId, string subject)
        {
            return base.Channel.BatchJobCreate(recordCount, templateId, subject);
        }

        public void BatchJobUpdate(int batchJobId)
        {
            base.Channel.BatchJobUpdate(batchJobId);
        }

        public List<EmailBatch> EmailBatchJobsToProcess()
        {
            return base.Channel.EmailBatchJobsToProcess();
        }

        public List<EmailQueueItem> GetEmailsInQueue()
        {
            return base.Channel.GetEmailsInQueue();
        }

        public List<EmailBatch> GetAllBatchJobs()
        {
            return base.Channel.GetAllBatchJobs();
        }

        #endregion



        public System.Data.DataSet SelectLicenseTypeSpecialtyTypeByProviderType(string providerTypeId)
        {
            return base.Channel.SelectLicenseTypeSpecialtyTypeByProviderType(providerTypeId);
        }

        public System.Data.DataSet SelectLicenseStatuses()
        {
            return base.Channel.SelectLicenseStatuses();
        }
        public System.Data.DataSet SelectLicenseCurrentStatuses()
        {
            return base.Channel.SelectLicenseCurrentStatuses();
        }
        public string SelectLicenseTypeIdByAbbrev(string licenseAbbrev)
        {
            return base.Channel.SelectLicenseTypeIdByAbbrev(licenseAbbrev);
        }

        public DataSet GetActivityTypes()
        {
            return base.Channel.GetActivityTypes();
        }
        public DataSet GetCertificationTypes()         //akash
        {
            return base.Channel.GetCertificationTypes();
        }

        public DataSet SelectCLIALabCodesByNumber(string cliaNumber)
        {
            return base.Channel.SelectCLIALabCodesByNumber(cliaNumber);
        }

        public DataSet SelectCLIACertificateInfoByNumber(string cliaNumber)
        {
            return base.Channel.SelectCLIACertificateInfoByNumber(cliaNumber);
        }

        public DataSet GetEnrollStatusReasons()      //akash
        {
            return base.Channel.GetEnrollStatusReasons();
        }

        public DataSet SelectTaxInfo(int regid)
        {
            return base.Channel.SelectTaxInfo(regid);
        }

        public DataSet SelectAdditionalApplications(int regid)
        {
            return base.Channel.SelectAdditionalApplications(regid);
        }

        public DataSet SelectProviderDisenrollmentxref()
        {
            return base.Channel.SelectProviderDisenrollmentxref();
        }
        public DataSet SelectProviderDisenrollment(int regId)
        {
            return base.Channel.SelectProviderDisenrollment(regId);
        }

        public DataSet SelectCurrentAndPreviousApplicationsByRegID(int regId)
        {
            return base.Channel.SelectCurrentAndPreviousApplicationsByRegID(regId);
        }

        public DataSet SelectRegSectionUploadControlByRegIDAndPageSection(int regId, string regPageSection)
        {
            return base.Channel.SelectRegSectionUploadControlByRegIDAndPageSection(regId, regPageSection);
        }

        public int InsertTaxInfo(int regid, Dictionary<string, string> parms)
        {
            return base.Channel.InsertTaxInfo(regid, parms);
        }
        public System.Data.DataSet SelectBoardCertificationTypeByProviderTypeID(int ProviderTypeID)
        {
            return base.Channel.SelectBoardCertificationTypeByProviderTypeID(ProviderTypeID);
        }
        public System.Data.DataSet SelectBoardSpecialtyTypeByBoardCertificationID(int BoardCertificationID)
        {
            return base.Channel.SelectBoardSpecialtyTypeByBoardCertificationID(BoardCertificationID);
        }
        //Akta Package 5 start
        public DataSet GetAssignements()
        {
            return base.Channel.GetAssignements();
        }

        public DataSet GetAssignementByAuth(string priorAuthType)
        {
            return base.Channel.GetAssignementByAuth(priorAuthType);
        }

        public DataSet GetClaimDestinationPayer()
        {
            return base.Channel.GetClaimDestinationPayer();
        }

        public DataSet GetDestinationPayer()
        {
            return base.Channel.GetDestinationPayer();
        }

        public DataSet GetDestinationPayerIds(int destinationPayerMapId)
        {
            return base.Channel.GetDestinationPayerIds(destinationPayerMapId);
        }

        public DataSet GetAuthorization()
        {
            return base.Channel.GetAuthorization();
        }
        public DataSet GetPlanName()
        {
            return base.Channel.GetPlanName();
        }
        public DataSet GetManagedcareplan()
        {
            return base.Channel.GetManagedcareplan();
        }

        public DataSet GetSpecialIndicator()
        {
            return base.Channel.GetSpecialIndicator();
        }
        public DataSet GetDiagnosisCodeType()
        {
            return base.Channel.GetDiagnosisCodeType();
        }
        public DataSet GetDiagnoisCode()
        {
            return base.Channel.GetDiagnoisCode();
        }
        public DataSet GetICDCode()
        {
            return base.Channel.GetICDCode();
        }

        //public DataSet GetServiceCodeType()
        //{
        //    return base.Channel.GetServiceCodeType();
        //}
        public DataSet GetPriorPlacement()
        {
            return base.Channel.GetPriorPlacement();
        }
        public DataSet GetPriorNewPlacement()
        {
            return base.Channel.GetPriorNewPlacement();
        }
        public DataSet GetPriorDocumentType()
        {
            return base.Channel.GetPriorDocumentType();
        }
        public DataSet GetStatus()
        {
            return base.Channel.GetStatus();
        }

        public DataSet GetStatusByDetails(string providerNPI, string medicalID)
        {
            return base.Channel.GetStatusByDetails(providerNPI, medicalID);
        }

        public DataSet GetSequenceType()
        {
            return base.Channel.GetSequenceType();
        }
        public DataSet GetClaimStatus()
        {
            return base.Channel.GetClaimStatus();
        }
        public DataSet LoadQueueNames() // MQSearch-- AP
        {
            return base.Channel.LoadQueueNames();
        }


        public DataSet GetDischargeStatus()
        {
            return base.Channel.GetDischargeStatus();
        }
        public DataSet GetAdmitSource()
        {
            return base.Channel.GetAdmitSource();
        }
        public DataSet GetEPSDTCondition()
        {
            return base.Channel.GetEPSDTCondition();
        }
        public DataSet GetAdmissionType()
        {
            return base.Channel.GetAdmissionType();
        }
        public DataSet GetAccidentrelatedto()
        {
            return base.Channel.GetAccidentrelatedto();
        }

        public DataSet GetAccidentState()
        {
            return base.Channel.GetAccidentState();
        }

        public DataSet GetAccidentcounty()
        {
            return base.Channel.GetAccidentcounty();
        }
        public DataSet GetReason()
        {
            return base.Channel.GetReason();
        }
        public DataSet GetOtherPhysician()
        {
            return base.Channel.GetOtherPhysician();
        }

        public DataSet GetPatientRelationship()
        {
            return base.Channel.GetPatientRelationship();
        }

        public DataSet GetClaimFilingIndicator()
        {
            return base.Channel.GetClaimFilingIndicator();
        }
        public DataSet GetAdjustmentGroup()
        {
            return base.Channel.GetAdjustmentGroup();
        }

        public DataSet GetSubmiclaimtProviderType()
        {
            return base.Channel.GetSubmiclaimtProviderType();
        }
        public DataSet GetCorrespondenceType()////OHPNM-3814
        {
            return base.Channel.GetCorrespondenceType();
        }
        public CorrespodenceInfo GetSearchCorrespondenceType(int correspondeceType, Guid userId, DateTime? fromDate, DateTime? toDate, int regId, string npi, string medicaidId, string currentSortOrder, string currentSortField, int pageSize = 0, int pageIndex = 0)
        {
            return base.Channel.GetSearchCorrespondenceType(correspondeceType, userId, fromDate, toDate, regId, npi, medicaidId, currentSortOrder, currentSortField, pageSize, pageIndex);
        }

        public DataSet GetPayerSequence()
        {
            return base.Channel.GetPayerSequence();
        }
        public DataSet GetPresentAddimission()
        {
            return base.Channel.GetPresentAddimission();
        }

        public DataSet GetUnitsOfMeasure()
        {
            return base.Channel.GetUnitsOfMeasure();
        }

        public DataSet GetPriorClaimsDocumentType()
        {
            return base.Channel.GetPriorClaimsDocumentType();
        }

        public System.Data.DataSet SelectPriorAuthHospital(int PriorAuthHospitalID)
        {
            return base.Channel.SelectPriorAuthHospital(PriorAuthHospitalID);
        }

        public int InsertPriorAuthHospital(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthHospital(tableName, parms);
        }

        public int InsertUpdatePriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertUpdatePriorAuthPanelData(tableName, parms);
        }

        public void DeletePriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.DeletePriorAuthPanelData(tableName, parms);
        }

        public DataSet GetPriorAuthPanelData(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.GetPriorAuthPanelData(tableName, parms);
        }

        public System.Data.DataSet SelectPriorAuthDiagnosis(int PriorAuthDiagnosisID)
        {
            return base.Channel.SelectPriorAuthDiagnosis(PriorAuthDiagnosisID);
        }
        public int InsertPriorAuthDiagnosis(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthDiagnosis(tableName, parms);
        }

        public System.Data.DataSet SelectPriorAuthServiceDetail(int PriorAuthServiceDetailID)
        {
            return base.Channel.SelectPriorAuthServiceDetail(PriorAuthServiceDetailID);
        }
        public int InsertPriorAuthServiceDetail(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthServiceDetail(tableName, parms);
        }

        public System.Data.DataSet SelectPriorAuthDentalProsthodontics(int PriorAuthDentalProsthodonticsID)
        {
            return base.Channel.SelectPriorAuthDentalProsthodontics(PriorAuthDentalProsthodonticsID);
        }
        public int InsertPriorAuthDentalProsthodontics(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthDentalProsthodontics(tableName, parms);
        }
        public System.Data.DataSet SelectPriorAuthNote(int PriorAuthNoteID)
        {
            return base.Channel.SelectPriorAuthNote(PriorAuthNoteID);
        }
        public int InsertPriorAuthNote(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthNote(tableName, parms);
        }

        //public System.Data.DataSet SelectPriorAuthAttachment(int PriorAuthAttachmentID)
        //{
        //    return base.Channel.SelectPriorAuthAttachment(PriorAuthAttachmentID);
        //}

        //public System.Data.DataSet SelectPriorAuthAttachment(string priorAuthType)
        //{
        //    return base.Channel.SelectPriorAuthAttachment(priorAuthType);
        //}
        //public int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms)
        //{
        //    return base.Channel.InsertPriorAuthAttachment(tableName, parms);
        //}
        //public System.Data.DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID)
        //{
        //    return base.Channel.SelectPriorAuthDocumentByMail(PriorAuthDocumentByMailID);
        //}
        public int InsertPriorAuthDocumentByMail(string tableName, Dictionary<string, object> parms)
        {
            return base.Channel.InsertPriorAuthDocumentByMail(tableName, parms);
        }
        public int insertPRIOR_AUTH_PROFFSERVICE_DETAIL(string tableName, Dictionary<string, object> parms)

        {
            return base.Channel.insertPRIOR_AUTH_PROFFSERVICE_DETAIL(tableName, parms);
        }
        public DataSet GetPrioHospitalData(string tableName, int priohospitalId)
        {
            return base.Channel.GetPrioHospitalData(tableName, priohospitalId);
        }

        public int CreatePriorDiagnosisServiceDetail(int? SequenceId, int? diagCodeTypeId, string diagCode, string diagCodeDesc, string diagnosisDate)
        {
            return base.Channel.CreatePriorDiagnosisServiceDetail(SequenceId, diagCodeTypeId, diagCode, diagCodeDesc, diagnosisDate);
        }

        public void DeletePriorDiagnosisServiceDetail(int lineNumber)
        {
            base.Channel.DeletePriorDiagnosisServiceDetail(lineNumber);
        }

        public DataSet GetPriorDiagnosisServiceDetail(int authType)
        {
            return base.Channel.GetPriorDiagnosisServiceDetail(authType);
        }

        public DataSet GetPriorAuthDocumentMail(string tableName, int hospitalId, int regId)
        {
            return base.Channel.GetPriorAuthDocumentMail(tableName, hospitalId, regId);
        }

        public DataSet GetSubmitClaimData(string tableName, int submitclaimid)
        {
            return base.Channel.GetSubmitClaimData(tableName, submitclaimid);

        }
        //Akta Package 5 End

        public System.Data.DataSet GetAllAgentsByProviderAdmin(string UserId)
        {
            return base.Channel.GetAllAgentsByProviderAdmin(UserId);
        }
        public System.Data.DataSet SelectAgentRolesByProviderAdmin(string UserId, int RegID, int Offset, int PageSize, string UserName, bool isCostRptMgmtAgent)
        {
            return base.Channel.SelectAgentRolesByProviderAdmin(UserId, RegID, Offset, PageSize, UserName, isCostRptMgmtAgent);
        }

        public System.Data.DataSet GetAgentRolesByProviderAdminCount(string UserId, int RegID, string AgentUserName)
        {
            return base.Channel.GetAgentRolesByProviderAdminCount(UserId, RegID, AgentUserName);
        }
        public System.Data.DataSet SelectAgentFacilities_ContractsByCEOUser(string CEOUserID)
        {
            return base.Channel.SelectAgentFacilities_ContractsByCEOUser(CEOUserID);
        }
        public void SavePendingAgentsByProviderAdmin(string AgentUserID, string ProvAdminUserID)
        {
            base.Channel.SavePendingAgentsByProviderAdmin(AgentUserID, ProvAdminUserID);
        }
        public void SaveAgentRolesByProviderAdmin(Dictionary<string, object> parms)
        {
            base.Channel.SaveAgentRolesByProviderAdmin(parms);
        }
        public void DeleteProviderAgentMapping(int RegId, string ProvAdminID, Guid AgentUserID)
        {
            base.Channel.DeleteProviderAgentMapping(RegId, ProvAdminID, AgentUserID);
        }
        public void SaveSecondaryUserFacility_ContractByCEOUser(Dictionary<string, object> parms)
        {
            base.Channel.SaveSecondaryUserFacility_ContractByCEOUser(parms);
        }

        public void InsertNewProviderAgent(string AgentUserID, string ProvAdminUserID, int RegID, bool IsActive, string ChangedBy)
        {
            base.Channel.InsertNewProviderAgent(AgentUserID, ProvAdminUserID, RegID, IsActive, ChangedBy);
        }
        public System.Data.DataSet CheckAgentRolesByProviderAdmin(string UserId, int RegID)
        {
            return base.Channel.CheckAgentRolesByProviderAdmin(UserId, RegID);
        }
        public System.Data.DataSet SelectPublicProviderLblDataByRegID(int regId)
        {
            return base.Channel.SelectPublicProviderLblDataByRegID(regId);
        }
        public System.Data.DataSet SelectPublicProviderLblLocDataByRegID(int regId, int regAddressId = 0)
        {
            return base.Channel.SelectPublicProviderLblLocDataByRegID(regId, regAddressId);
        }
        public System.Data.DataSet SelectHearingStatus()
        {
            return base.Channel.SelectHearingStatus();
        }

        public System.Data.DataSet SelectTerminationReasons()
        {
            return base.Channel.SelectTerminationReasons();
        }
        public System.Data.DataSet SelectReasonBackgroundCheckNotRequired()
        {
            return base.Channel.SelectReasonBackgroundCheckNotRequired();
        }
        public System.Data.DataSet SelectHospiceApplicationActionType()
        {
            return base.Channel.SelectHospiceApplicationActionType();
        }
        public System.Data.DataSet SelectHospiceBenifitSegmentIndicatorType()
        {
            return base.Channel.SelectHospiceBenifitSegmentIndicatorType();
        }
        public System.Data.DataSet SelectHospiceReasonUpdateType()
        {
            return base.Channel.SelectHospiceReasonUpdateType();
        }
        public System.Data.DataSet SelectHospicePayerType()
        {
            return base.Channel.SelectHospicePayerType();
        }
        public System.Data.DataSet SelectHospiceDocumentType()
        {
            return base.Channel.SelectHospiceDocumentType();
        }
        public System.Data.DataSet SelectCounty(string state)
        {
            return base.Channel.SelectCounty(state);
        }
        public System.Data.DataSet SelectState()
        {
            return base.Channel.SelectState();
        }

        public System.Data.DataSet GetICDDiagnosis(string code, string icdVersion, string diagnosisDes)
        {
            return base.Channel.GetICDDiagnosis(code, icdVersion, diagnosisDes);
        }
        public void NotifyBackgroundCheckWithoutPrints(string subject, string owner, string file, string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyBackgroundCheckWithoutPrints(subject, owner, file, recipients, username, pdmsUrl, regId, userid);
        }

        public DataSet VerifyZipCodeIsOH(string zipCode)
        {
            return base.Channel.VerifyZipCodeIsOH(zipCode);
        }
        public DataSet SelectRegOwnerByOwnerID(int OwnerID)
        {
            return base.Channel.SelectRegOwnerByOwnerID(OwnerID);
        }
        public void NotifyRiskLevelBumpUp(string recipients, string username, string pdmsUrl, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyRiskLevelBumpUp(recipients, username, pdmsUrl, regId, userid);
        }


        public DataSet GetInquireHospiceResponse(string hospiceTrackNo, string providerMedId)
        {
            return base.Channel.GetInquireHospiceResponse(hospiceTrackNo, providerMedId);
        }

        public DataSet SearchHospiceResponse(long hospiceTrackNo, string providerMedId, string reciefId, string providerNpi, int offset, int count)
        {
            return base.Channel.SearchHospiceResponse(hospiceTrackNo, providerMedId, reciefId, providerNpi, offset, count);
        }

        public DataSet SelectRegistrationsByMedicaidID(string medicaid_ID)
        {
            return base.Channel.SelectRegistrationsByMedicaidID(medicaid_ID);
        }
        public DataSet SelectExitingProviderByMedicaidID(string medicaid_ID)
        {
            return base.Channel.SelectExitingProviderByMedicaidID(medicaid_ID);
        }

        public void InsertRegChopParent(Dictionary<string, object> parms)
        {
            base.Channel.InsertRegChopParent(parms);

        }

        public void NotifyProviderTempPassword(string recipients, string USER_NAME, string LOGIN_URL, string tempPswd, int regId = 0, Guid userid = new Guid())
        {
            base.Channel.NotifyProviderTempPassword(recipients, USER_NAME, LOGIN_URL, tempPswd, regId, userid);
        }
        public System.Data.DataSet SelectPendingAgentsByProviderAdmin(string UserId)
        {
            return base.Channel.SelectPendingAgentsByProviderAdmin(UserId);
        }
        public System.Data.DataSet SelectProviderByProviderAdmin(string UserId, string eMail)
        {
            return base.Channel.SelectProviderByProviderAdmin(UserId, eMail);
        }


        public DataSet GetRegWaiverServices(int regId)
        {
            return base.Channel.GetRegWaiverServices(regId);
        }

        public void UpdateRSLFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, string comment, Guid modifiedBy)
        {
            base.Channel.UpdateRSLFacilityHomeNumber(regID, LTEffectiveDate, homeNumber, comment, modifiedBy);
        }

        public void InsertUpdateODHFacilityHomeNumber(int regID, DateTime LTEffectiveDate, int homeNumber, Guid modifiedBy)
        {
            base.Channel.InsertUpdateODHFacilityHomeNumber(regID, LTEffectiveDate, homeNumber, modifiedBy);
        }

        public bool IsRiskLevelAssigned(int regId)
        {
            return base.Channel.IsRiskLevelAssigned(regId);
        }
        public void UpdateCredentialRisk(int CredentialingID, int riskID, string changedBy)
        {
            base.Channel.UpdateCredentialRisk(CredentialingID, riskID, changedBy);
        }
        public DataSet SelectCredentialingResult()
        {
            return base.Channel.SelectCredentialingResult();
        }

        public DataSet SelectCredentialingComments(int regID, int credentialingId)
        {
            return base.Channel.SelectCredentialingComments(regID, credentialingId);
        }

        public void SetUserActiveStatus(Guid agentuserId, Guid adminuserId, bool status, int regID)
        {
            base.Channel.SetUserActiveStatus(agentuserId, adminuserId, status, regID);
        }

        public void ChangeStatusDeniedProvider(DateTime changedDate, Guid changedBy, int enrollStatusReason, int enrollStatus, int regID)
        {
            base.Channel.ChangeStatusDeniedProvider(changedDate, changedBy, enrollStatusReason, enrollStatus, regID);
        }

        public DataSet GetChopTypes()
        {
            return base.Channel.GetChopTypes();
        }


        public System.Data.DataSet GetOwnerTitle()
        {
            return base.Channel.GetOwnerTitle();
        }
        public System.Data.DataSet GetAffliationType()
        {
            return base.Channel.GetAffliationType();
        }
        //ap
        public DataSet SelectServiceLocationByMedicaidID(string medicaidID)
        {
            return base.Channel.SelectServiceLocationByMedicaidID(medicaidID);
        }
        public DataSet GetServiceCodeType()
        {
            return base.Channel.GetServiceCodeType();
        }
        public DataSet GetServiceTypeCode() //Ap 3/1/2022
        {
            return base.Channel.GetServiceTypeCode();
        }

        public DataSet GetServiceCodeTypeForPriorAuth()
        {
            return base.Channel.GetServiceCodeTypeForPriorAuth();
        }
        public DataSet GetProcedureCodeType()
        {
            return base.Channel.GetProcedureCodeType();
        }



        public DataSet GetPricingFormula()
        {
            return base.Channel.GetPricingFormula();
        }

        public DataSet GetReviewReasons()
        {
            return base.Channel.GetReviewReasons();
        }
        public DataSet GetRestrictedServiceStatuses()
        {
            return base.Channel.GetRestrictedServiceStatuses();
        }
        public DataSet GetIncludeExclude()
        {
            return base.Channel.GetIncludeExclude();
        }
        public void DeleteRegRestriction(int regRestrictionID)
        {
            base.Channel.DeleteRegRestriction(regRestrictionID);
        }
        public System.Data.DataSet GetRestrictedServicesHistory(int regId)
        {
            return base.Channel.GetRestrictedServicesHistory(regId);
        }

        public void InsertRegChopPrimaryServiceAddress(Dictionary<string, object> parms)
        {
            base.Channel.InsertRegChopPrimaryServiceAddress(parms);
        }
        public DataSet SelectVerificationSource(int activityTypeId, int IsIndividual, int includeInactive)
        {
            return base.Channel.SelectVerificationSource(activityTypeId, IsIndividual, includeInactive);
        }

        public System.Data.DataSet SelectRegCostReport(string UserId, int REG_ID)
        {
            return base.Channel.SelectRegCostReport(UserId, REG_ID);
        }
        public DataSet SelectLTCCostReportById(string UserId, int REG_COST_REPORT_ID)
        {
            return base.Channel.SelectLTCCostReportById(UserId, REG_COST_REPORT_ID);
        }
        public DataSet SelectMSPCostReportById(string UserId, int REG_COST_REPORT_ID)
        {
            return base.Channel.SelectMSPCostReportById(UserId, REG_COST_REPORT_ID);
        }
        public System.Data.DataSet SelectRegMSPCostReport(string UserId, int REG_ID)
        {
            return base.Channel.SelectRegMSPCostReport(UserId, REG_ID);
        }
        public System.Data.DataSet SelectCostReportDocument(int documentId)
        {
            return base.Channel.SelectCostReportDocument(documentId);
        }
        public DataSet GetCRProviderType()
        {
            return base.Channel.GetCRProviderType();
        }
        public DataSet GetFacilityProgrameTypes(string mmisProviderTypeId)
        {
            return base.Channel.GetFacilityProgrameTypes(mmisProviderTypeId);
        }
        public DataSet GetSettelementTypes()
        {
            return base.Channel.GetSettelementTypes();
        }
        public DataSet GetLetterTypes()
        {
            return base.Channel.GetLetterTypes();
        }
        public DataSet GetReportTypes()
        {
            return base.Channel.GetReportTypes();
        }
        public DataSet GetPeriodTypes()
        {
            return base.Channel.GetPeriodTypes();
        }
        public DataSet GetCostReportTypes(string mmisProviderTypeId)
        {
            return base.Channel.GetCostReportTypes(mmisProviderTypeId);
        }


        public void UpdateCredentialActivity(int credentialActivityID, int reg_id, int activityTypeId, int dataRankId, string notes, DateTime? OrigEffDate, DateTime? renewalDate,
          DateTime? expiraationDate, int veriSourId, DateTime? verificationDate, DateTime? lastActionDate, string verifiedBy, DateTime? attestationDate, bool isBoardVerificationRequired,
           DateTime? maternityLicDate, DateTime? siteAccredDate, bool isMediaCareOutput)
        {
            base.Channel.UpdateCredentialActivity(credentialActivityID, reg_id, activityTypeId, dataRankId, notes, OrigEffDate, renewalDate, expiraationDate, veriSourId,
                    verificationDate, lastActionDate, verifiedBy, attestationDate, isBoardVerificationRequired, maternityLicDate, siteAccredDate, isMediaCareOutput);
        }

        public System.Data.DataSet SearchRetrieveReports(Dictionary<string, object> parms, string reportType, out int totalResultCount)
        {
            return base.Channel.SearchRetrieveReports(parms, reportType, out totalResultCount);
        }
        public System.Data.DataSet SearchRAReports(Dictionary<string, object> parms, out int totalResultCount)
        {
            return base.Channel.SearchRAReports(parms, out totalResultCount);
        }

        public void InsertCommunicationEventHistoryEvent(int communicationEventId, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            base.Channel.InsertCommunicationEventHistoryEvent(communicationEventId, lastModifiedDate, lastModifiedUser);
        }
        public void UpdateRetrieveReports(int index, bool isDownloaded, DateTime downloadDate, DateTime lastModifiedDate, Guid lastModifiedUser)
        {
            base.Channel.UpdateRetrieveReports(index, isDownloaded, downloadDate, lastModifiedDate, lastModifiedUser);
        }
        public List<AttachmentResponse> SendAttachments(List<SendAttachment> sendAttachments, AttachmentMessageHeader messageHeader)
        {
            return base.Channel.SendAttachments(sendAttachments, messageHeader);
        }

        public void saveFaultCodeException(string faultString, string faultCode, int transactionID, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            base.Channel.saveFaultCodeException(faultString, faultCode, transactionID, modifiedDate, modifiedBy, rawResponse, pnmTransactionKey);
        }

        public void saveSoapResponseCodeException(string ModuleTransactionId, string SITransactionKey, string ResponseCode, string ResponseDetails,
            string ResponseMessage, string ResponseType, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            base.Channel.saveSoapResponseCodeException(ModuleTransactionId, SITransactionKey, ResponseCode,
                ResponseDetails, ResponseMessage, ResponseType, modifiedDate, modifiedBy, rawResponse, pnmTransactionKey);
        }

        public DataSet GetStagingProviderEnrollmentData(int TransactionID)
        {
            return base.Channel.GetStagingProviderEnrollmentData(TransactionID);
        }

        public DataSet SelectTransactionIDsByRegID(int regID)
        {
            return base.Channel.SelectTransactionIDsByRegID(regID);
        }

        public DataSet SelectTransactionDetailsByTransactionID(int transactionID)
        {
            return base.Channel.SelectTransactionDetailsByTransactionID(transactionID);
        }

        public DataSet SelectPECOSRevalidationData(int regID)
        {
            return base.Channel.SelectPECOSRevalidationData(regID);
        }

        public DataSet GetCountryCodes()
        {
            return base.Channel.GetCountryCodes();
        }

        public DataSet SearchTransactions(string transactionStatus, string regId, string medicaidID, string fromDate, string toDate, string sortBy, int pageSize, int pageNumber, bool asc, string SubscriberFailed, out int totalResultCount)
        {
            return base.Channel.SearchTransactions(transactionStatus, regId, medicaidID, fromDate, toDate, sortBy, pageSize, pageNumber, asc, SubscriberFailed, out totalResultCount);
        }

        public DataSet SearchMQSearch(out int totalCount, int que, DateTime? fromDate, DateTime? toDate)
        {
            return base.Channel.SearchMQSearch(out totalCount, que, fromDate, toDate);
        }

        public DataSet SearchMQSearchData(out int totalCount, int que, DateTime? fromDate, DateTime? toDate, string RegId, string MedicaidId)
        {
            return base.Channel.SearchMQSearchData(out totalCount, que, fromDate, toDate, RegId, MedicaidId);
        }
        public DataSet GetSearchMQMessage(int logId)
        {
            return base.Channel.GetSearchMQMessage(logId);
        }


        public DataSet SelectBuildingMedicaidIDByRegID(int regId)
        {
            return base.Channel.SelectBuildingMedicaidIDByRegID(regId);
        }

        public void InsertRegApplicationRecord(int regId, DateTime regCreadtedDate, DateTime lastModifiedDate, Guid lastModifiedUser, int processId)
        {
            base.Channel.InsertRegApplicationRecord(regId, regCreadtedDate, lastModifiedDate, lastModifiedUser, processId);
        }

        public int GetRegSectionUploadControlIDByRegID(int regID, string regPageSection, string title)
        {
            return base.Channel.GetRegSectionUploadControlIDByRegID(regID, regPageSection, title);
        }

        public DataSet SelectRescentTransactionIDByRegID(int regID)
        {
            return base.Channel.SelectRescentTransactionIDByRegID(regID);
        }

        public DataSet CheckIfActiveCMCProvider(int regID)
        {
            return base.Channel.CheckIfActiveCMCProvider(regID);
        }
        public DataSet GetProviderSpecialtySearchResults(string npi, string medicaidID)
        {
            return base.Channel.GetProviderSpecialtySearchResults(npi, medicaidID);
        }
        public System.Data.DataSet GetHistoricNotes(int regId)
        {
            return base.Channel.GetHistoricNotes(regId);
        }
        public System.Data.DataSet SelectSiteVisitDataByRegID(int regId)
        {
            return base.Channel.SelectSiteVisitDataByRegID(regId);
        }

        public System.Data.DataSet SelectAllApplicationStatus(string sourceSystem)
        {
            return base.Channel.SelectAllApplicationStatus(sourceSystem);
        }
        public System.Data.DataSet SelectAllRegistrationStatus()
        {
            return base.Channel.SelectAllRegistrationStatus();
        }

        public bool CheckFedExclusionsMatch(int regID)
        {
            return base.Channel.CheckFedExclusionsMatch(regID);
        }

        public bool CheckDODDAbuserRegistryMatch(int regID)
        {
            return base.Channel.CheckDODDAbuserRegistryMatch(regID);
        }

        public void CreateContractForSpecialty386(int regId, DateTime startDate, Guid changedBy)
        {
            base.Channel.CreateContractForSpecialty386(regId, startDate, changedBy);
        }

        public bool CheckDDFacilityNumberExists(string facilityNumber)
        {
            return base.Channel.CheckDDFacilityNumberExists(facilityNumber);
        }

        public int InsertDelegateUsers(Guid delegateUserID, bool activeDelegate, bool phoneMatch, Guid lastModifiedUser, DateTime lastModifiedDate,
            Guid createdBy, DateTime createdDate, string contactName, string Emailaddr)
        {
            return base.Channel.InsertDelegateUsers(delegateUserID, activeDelegate, phoneMatch, lastModifiedUser, lastModifiedDate,
            createdBy, createdDate, contactName, Emailaddr);
        }

        public DataSet SelectUserIDDelegatesData()
        {
            return base.Channel.SelectUserIDDelegatesData();
        }

        public bool CheckDelegateUserIDisActive(string userID)
        {
            return base.Channel.CheckDelegateUserIDisActive(userID);
        }

        public int InsertDelegateDocument(Guid userID, string fileName, string newFileName, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate)
        {
            return base.Channel.InsertDelegateDocument(userID, fileName, newFileName, fileDescription, status, lastModifiedUser, lastModifiedDate,
                createdBy, createdDate);
        }

        public int InsertBulkAgentDocument(Guid userID, string fileName, string newFileName, string fileDescription, int status, Guid lastModifiedUser, DateTime lastModifiedDate,
                Guid createdBy, DateTime createdDate)
        {
            return base.Channel.InsertBulkAgentDocument(userID, fileName, newFileName, fileDescription, status, lastModifiedUser, lastModifiedDate,
                createdBy, createdDate);
        }


        public bool CheckNPPESInactiveMatch(int regID)
        {
            return base.Channel.CheckNPPESInactiveMatch(regID);
        }


        public DataSet SelectCLIACertificateDatesByCLIANumber(int regId, string cliaNumber)
        {
            return base.Channel.SelectCLIACertificateDatesByCLIANumber(regId, cliaNumber);
        }

        public System.Data.DataSet SelectOfficeInformationData(int regId, int regAddressId, string tableName)
        {
            return base.Channel.SelectOfficeInformationData(regId, regAddressId, tableName);
        }

        public DataSet GetProcedureCodePopupSearch(string procCode = "", string procCodeDesc = "")
        {
            return base.Channel.GetProcedureCodePopupSearch(procCode, procCodeDesc);
        }

        public DataSet GetRevenueCodePopupSearch(string revenueCode = "", string revenueCodeDesc = "")
        {
            return base.Channel.GetRevenueCodePopupSearch(revenueCode, revenueCodeDesc);
        }

        public DataSet GetLevelOfCare()
        {
            return base.Channel.GetLevelOfCare();
        }

        public DataSet GetRequestedUnitMeasures()
        {
            return base.Channel.GetRequestedUnitMeasures();
        }

        public int CreatePriorAuthServiceDetail(int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId, decimal? reqUnitsFee, DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId, int? authUnits, int? remainigUnits, decimal? authDollars, DateTime? authTDOS, DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime, Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy)
        {
            return base.Channel.CreatePriorAuthServiceDetail(codeTypeId, revenueCode, procedureCode,
                                                            reqUnits, unitMeasurementId,
                                                            reqUnitsFee, reqFDOS, reqTDOS, statusId,
                                                            procCodeDesc, providerServiceNote, levelofCareId,
                                                            authUnits, remainigUnits, authDollars, authTDOS,
                                                            authFDOS, serviceTrackingNo,
                                                            lastModifiedDateTime, lastModifiedBy,
                                                            createdDate, createdBy);
        }

        public int UpdatePriorAuthServiceDetail(int detailedId, int? codeTypeId, string revenueCode, string procedureCode, string reqUnits, int? unitMeasurementId, decimal? reqUnitsFee, DateTime reqFDOS, DateTime reqTDOS, int? statusId, string procCodeDesc, string providerServiceNote, int? levelofCareId, int? authUnits, int? remainingUnits, decimal? authDollars, DateTime? authTDOS, DateTime? authFDOS, int? serviceTrackingNo, DateTime lastModifiedDateTime, Guid lastModifiedBy, DateTime? createdDate, Guid? createdBy)
        {
            return base.Channel.UpdatePriorAuthServiceDetail(detailedId, codeTypeId, revenueCode, procedureCode, reqUnits,
                                                 unitMeasurementId, reqUnitsFee,
                                                   reqFDOS, reqTDOS, statusId, procCodeDesc,
                                                   providerServiceNote, levelofCareId, authUnits, remainingUnits,
                                                   authDollars, authTDOS,
                                                  authFDOS, serviceTrackingNo, lastModifiedDateTime, lastModifiedBy,
                                                  createdDate, createdBy);
        }

        public DataSet GetPriorAuthServiceDetail()
        {
            return base.Channel.GetPriorAuthServiceDetail();
        }
        public void DeletePriorAuthServiceDetail(int lineNumber)
        {
            base.Channel.DeletePriorAuthServiceDetail(lineNumber);
        }

        public int CreatePriorAuthProviderNotes(string notesType = "", string noteDesc = "", string noteStatus = "", string noteReasonCodeDesc = "",
           string revProvDesc = "", string prior_Auth_Type = "", string reasonCode = "", string reasonDesc = "", string status = "", Guid? createdBy = null)
        {
            return base.Channel.CreatePriorAuthProviderNotes(notesType, noteDesc, noteStatus, noteReasonCodeDesc, revProvDesc,
                       prior_Auth_Type, reasonCode, reasonDesc, status, createdBy);
        }

        public int UpdatePriorAuthProviderNotes(Int32 noteid, string notesType, string noteDesc, string noteStatus,
          string noteReasonCodeDesc, string revProvDesc, DateTime lastModifiedBy, string prior_Auth_Type, string reasonCode, Guid? createdBy = null)
        {
            return base.Channel.UpdatePriorAuthProviderNotes(noteid, notesType, noteDesc, noteStatus,
            noteReasonCodeDesc, revProvDesc, lastModifiedBy, prior_Auth_Type, reasonCode, createdBy);
        }

        public DataSet GetPriorAuthProviderNotes(string authType)
        {
            return base.Channel.GetPriorAuthProviderNotes(authType);
        }
        public void DeletePriorAuthProviderNote(int noteid)
        {
            base.Channel.DeletePriorAuthProviderNote(noteid);
        }
        //public void DeletePriorAuthAttachment(int documentId)
        //{
        //    base.Channel.DeletePriorAuthAttachment(documentId);
        //}
        public DataSet GetPriorAuthDocumentTypes()
        {
            return base.Channel.GetPriorAuthDocumentTypes();
        }

        public DataSet GetPriorAuthDentalServiceDetail()
        {
            return base.Channel.GetPriorAuthDentalServiceDetail();
        }
        public DataSet GetPriorAuthProfessionalServiceDetail()
        {
            return base.Channel.GetPriorAuthProfessionalServiceDetail();
        }
        public void DeletePriorAuthDentalServiceDetail(int lineNumber)
        {
            base.Channel.DeletePriorAuthDentalServiceDetail(lineNumber);
        }
        public void DeletePriorAuthProfessionalServiceDetail(int lineNumber)
        {
            base.Channel.DeletePriorAuthProfessionalServiceDetail(lineNumber);
        }

        public DataSet GetPriorAuthDentalOralCavity()
        {
            return base.Channel.GetPriorAuthDentalOralCavity();
        }

        public DataSet GetPriorAuthDentalProsthesis()
        {
            return base.Channel.GetPriorAuthDentalProsthesis();
        }

        //public DataSet GetPriorAuthDentalToothSurface()
        //{
        //    return base.Channel.GetPriorAuthDentalToothSurface();
        //}

        public DataSet GetPriorAuthDentalToothNumber()
        {
            return base.Channel.GetPriorAuthDentalToothNumber();
        }
        public int InsertPriorAuthServiceDetails(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthServiceDetails(tableName, parms);
        }
        public void UpdatePriorAuthServiceDetails(string authType, Dictionary<string, string> parms)
        {
            base.Channel.UpdatePriorAuthServiceDetails(authType, parms);
        }
        public int InsertPriorAuthNotes(Dictionary<string, string> parms)
        {
            return base.Channel.InsertPriorAuthNotes(parms);
        }

        #region Prior Auth SaveID

        public int GetPriorAuthAttachmentSaveID(string priorAuthType)
        {
            return base.Channel.GetPriorAuthAttachmentSaveID(priorAuthType);
        }
        public int GetPriorAuthServiceDetailSaveID(string priorAuthType, string medicaidId)
        {
            return base.Channel.GetPriorAuthServiceDetailSaveID(priorAuthType, medicaidId);
        }
        //OHPNM-5582
        public int GetPriorAuthDetailSaveID(int paType, string medicaidId, string trackingNo)
        {
            return base.Channel.GetPriorAuthDetailSaveID(paType, medicaidId, trackingNo);
        }
        public int SavePriorAuth(int authType, Dictionary<string, object> parms)
        {
            return base.Channel.SavePriorAuth(authType, parms);
        }

        #endregion
        //OHPNM-5582-SearchPRIORAUTHTRACKING(string trackingNumber, int statustype, int? payerId);
        public DataSet SearchPRIORAUTHTRACKING(string trackingNumber, int statustype, int? payerId)
        {
            return base.Channel.SearchPRIORAUTHTRACKING(trackingNumber, statustype, payerId);
        }
        //OHPNM-5582-SearchPriorTrackingSubmitPA(string medicaidId)
        public DataSet SearchPriorTrackingSubmitPA(string medicaidId, string trackingNo)
        {
            return base.Channel.SearchPriorTrackingSubmitPA(medicaidId, trackingNo);
        }
        public int InsertPanelsData(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.InsertPanelsData(tableName, parms);
        }
        public void UpdatePanelsData(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.UpdatePanelsData(tableName, parms);
        }
        public DataSet SelectPanelsData(string tableName, Dictionary<string, string> parms)
        {
            return base.Channel.SelectPanelsData(tableName, parms);
        }

        public DataSet SelectDentalClaimData(int claimID, string tableName, string claimPanelName = "", int IsPrimary = 0)
        {
            return base.Channel.SelectDentalClaimData(claimID, tableName, claimPanelName, IsPrimary);
        }
        public void DeletePanelsData(string tableName, string idColumnName, int id)
        {
            base.Channel.DeletePanelsData(tableName, idColumnName, id);
        }

        public void DeletePanelsDataWithParams(string tableName, Dictionary<string, string> parms)
        {
            base.Channel.DeletePanelsDataWithParams(tableName, parms);
        }

        public DataSet GetToothServiceLine(int claimId)
        {
            return base.Channel.GetToothServiceLine(claimId);
        }

        public DataSet GetAccidentUSState()
        {
            return base.Channel.GetAccidentUSState();
        }
        public DataSet GetFilteredOtherPayerSequence()
        {
            return base.Channel.GetFilteredOtherPayerSequence();
        }

        public DataSet GetToothSurface()
        {
            return base.Channel.GetToothSurface();
        }
        public DataSet GetInsuranceTypes()
        {
            return base.Channel.GetInsuranceTypes();
        }
        public DataSet GetClaimAdjudicationLevel()
        {
            return base.Channel.GetClaimAdjudicationLevel();
        }


        public DataSet GetReasonCodesOPP()
        {
            return base.Channel.GetReasonCodesOPP();
        }
        public DataSet LoadDestinationPayer(bool loadAll = false)
        {
            return base.Channel.LoadDestinationPayer(loadAll);
        }


        public DataSet GetDelayReason()
        {
            return base.Channel.GetDelayReason();
        }

        public DataSet GetClaimTransactionType()
        {
            return base.Channel.GetClaimTransactionType();
        }

        public DataSet GetClaimDocumentTypeByClaimTransactionType(int transType)
        {
            return base.Channel.GetClaimDocumentTypeByClaimTransactionType(transType);
        }

        public DataSet GetUploadAttachmentsByMedicaid(string medicaid, string icn, string pa_number)
        {
            return base.Channel.GetUploadAttachmentsByMedicaid(medicaid, icn, pa_number);
        }
        public DataSet GetCPCAttachmentsDocType()
        {
            return base.Channel.GetCPCAttachmentsDocType();
        }
        public DataSet GetOccurreneceCode(string code = "", string codeDesc = "")
        {
            return base.Channel.GetOccurreneceCode(code, codeDesc);
        }

        public DataSet GetConditionCodeDescription(string ConditionCode)
        {
            return base.Channel.GetConditionCodeDescription(ConditionCode);

        }

        public DataSet GetValueCodeDescription(string ValueCode)
        {
            return base.Channel.GetValueCodeDescription(ValueCode);

        }

        public void DeleteAttachmentsForMedicaidID(string Medicaid_ID)
        {
            base.Channel.DeleteAttachmentsForMedicaidID(Medicaid_ID);
            return;
        }


        public void UpdateRegMedicareEnrollmentStatus(Dictionary<string, string> parms)
        {
            base.Channel.UpdateRegMedicareEnrollmentStatus(parms);
        }

        public System.Data.DataSet GetMQLastExecutionDT(string MQAppID)
        {
            return base.Channel.GetMQLastExecutionDT(MQAppID);
        }

        //public void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested, string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId, string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier)
        //{
        //    base.Channel.InsertOutBoundDocumentUploads(editTransactionTypeId, payerRequested,
        //    memberId, claimTypeId, claimNumber, paNumber, providerId, providerNPI, senderId,
        //    receiverId, documentTypeId, documentName, uuid, toSend, appAdminUserId, documentIdentifier);
        //}


        public bool Generate1099WithPayerInfo(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName, string payerName, string payerAddress1, string payerAddress2, string payerCity, string payerState, string payerZip)
        {
            return base.Channel.Generate1099WithPayerInfo(regID, taxId, fedWithhold, payments, year, userName, appPath, out pdfFileName, payerName, payerAddress1, payerAddress2, payerCity, payerState, payerZip);
        }

        public bool Generate1099(int regID, string taxId, string fedWithhold, string payments, string year, string userName, string appPath, out string pdfFileName)
        {
            return base.Channel.Generate1099(regID, taxId, fedWithhold, payments, year, userName, appPath, out pdfFileName);
        }

        public void CancelRegistration(int regID, DateTime modifiedDate, Guid modifiedBy, int processId, string commandName = "")
        {
            base.Channel.CancelRegistration(regID, modifiedDate, modifiedBy, processId, commandName);
        }

        public void CancelRegistrationCPC(int regID, DateTime modifiedDate, Guid modifiedBy, int processID, int workflowEventTypeID)
        {
            base.Channel.CancelRegistrationCPC(regID, modifiedDate, modifiedBy, processID, workflowEventTypeID);
        }

        public DataSet SelectIndividualAffiliationStaffCategoryStatus()
        {
            return base.Channel.SelectIndividualAffiliationStaffCategoryStatus();
        }

        public string GetNPIOrMedicaidIdByRegId(int regID)
        {
            return base.Channel.GetNPIOrMedicaidIdByRegId(regID);
        }

        public Common.DataModels.LearningCategoryDocs[] GetLearningDocs()
        {
            return base.Channel.GetLearningDocs();
        }

        public Common.DataModels.LearningDoc GetLearningDoc(string fileId)
        {
            return base.Channel.GetLearningDoc(fileId);
        }

        public bool UserCanAccessReg(string userId, int regId)
        {
            return base.Channel.UserCanAccessReg(userId, regId);
        }

        public DataSet GetBehaviourHealthAffRequired(int regID)
        {
            return base.Channel.GetBehaviourHealthAffRequired(regID);
        }
        public bool CheckRestrictedServicesExistsByRegID(int regId)
        {
            return base.Channel.CheckRestrictedServicesExistsByRegID(regId);
        }

        public void SaveRegistrationSectionStatusRTP(int regId, int regPageTypeId, int regSectionTypeId, int? regProviderStatusTypeId, int? regProviderServicesStatusTypeId, string changedBy)
        {
            base.Channel.SaveRegistrationSectionStatusRTP(regId, regPageTypeId, regSectionTypeId, regProviderStatusTypeId, regProviderServicesStatusTypeId, changedBy);
        }

        public DataSet SelectServiceLocationByRegID(string regID)
        {
            return base.Channel.SelectServiceLocationByRegID(regID);
        }

        public DataSet GetRegAddressHospitalNoByRegIdOrMedicaidId(int regId, string medicaidID)
        {
            return base.Channel.GetRegAddressHospitalNoByRegIdOrMedicaidId(regId, medicaidID);
        }

        public void InsertRegAddressHospitalNo(int regId, string facilityNumberType, int regAddressId, DateTime lastModifiedDateTime, string lastModifiedUser)
        {
            base.Channel.InsertRegAddressHospitalNo(regId, facilityNumberType, regAddressId, lastModifiedDateTime, lastModifiedUser);
        }



        public DataSet GetPayerMCEID(int DestinationPayer)

        {

            return base.Channel.GetPayerMCEID(DestinationPayer);

        }



        public int SelectClaimAttachmentTypeId(string typename, string claim)

        {

            return base.Channel.SelectClaimAttachmentTypeId(typename, claim);

        }




        public System.Data.DataSet SelectPriorAuthAttachment(string priorAuthType)

        {

            return base.Channel.SelectPriorAuthAttachment(priorAuthType);

        }

        public int InsertPriorAuthAttachment(string tableName, Dictionary<string, string> parms)

        {

            return base.Channel.InsertPriorAuthAttachment(tableName, parms);

        }

        public System.Data.DataSet SelectPriorAuthDocumentByMail(int PriorAuthDocumentByMailID, int regID)

        {

            return base.Channel.SelectPriorAuthDocumentByMail(PriorAuthDocumentByMailID, regID);

        }



        public void DeletePriorAuthAttachment(int documentId)

        {

            base.Channel.DeletePriorAuthAttachment(documentId);

        }



        public DataSet GetPriorAuthDentalToothSurface()

        {

            return base.Channel.GetPriorAuthDentalToothSurface();

        }



        public DataSet GetIndProviderNote(string Claim_ID, string providerNoteID, string claim_type)

        {

            return base.Channel.GetIndProviderNote(Claim_ID, providerNoteID, claim_type);

        }

        public DataSet GetIndDiagCode(string claim_id, string claim_diag_info_id, string claim_type)

        {

            return base.Channel.GetIndDiagCode(claim_id, claim_diag_info_id, claim_type);

        }



        public DataSet GetIndAmbulanceDropOffCode(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID)

        {

            return base.Channel.GetIndAmbulanceDropOffCode(Claims_Ambulance_Pick_Up_Drop_Off_Location_ID);

        }



        public DataSet GetClaimsUploadAttachmentsByMedicaid(string medicaid, string claimID, string claimType)

        {

            return base.Channel.GetClaimsUploadAttachmentsByMedicaid(medicaid, claimID, claimType);

        }




        public void InsertClaimsOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested, string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId, string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalFileName, string documentID)

        {

            base.Channel.InsertClaimsOutBoundDocumentUploads(editTransactionTypeId, payerRequested,

            memberId, claimTypeId, claimNumber, paNumber, providerId, providerNPI, senderId,

            receiverId, documentTypeId, documentName, uuid, toSend, appAdminUserId, documentIdentifier, originalFileName, documentID);

        }



        public void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested, string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId, string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalDocumentName, string providerComments)

        {

            base.Channel.InsertOutBoundDocumentUploads(editTransactionTypeId, payerRequested,

            memberId, claimTypeId, claimNumber, paNumber, providerId, providerNPI, senderId,

            receiverId, documentTypeId, documentName, uuid, toSend, appAdminUserId, documentIdentifier, originalDocumentName, providerComments);

        }



        public DataSet GetSubCapitaPayerIDs(int destPayerID)

        {

            return base.Channel.GetSubCapitaPayerIDs(destPayerID);

        }



        public DataSet GetSubCapitaPayerIDsByMCE_ID(string mce_Id)

        {

            return base.Channel.GetSubCapitaPayerIDsByMCE_ID(mce_Id);

        }


        //public void UpdateScreeningStatus_PeriodicWF(int regID, DateTime insertDate, string insertBy)

        // {

        //     base.Channel.UpdateScreeningStatus_PeriodicWF(regID, insertDate, insertBy);

        // }



        public DataSet GetOccurrenceSpanInfo(string Claims_Occurrence_Code_Span_Information_ID, string Claim_ID)

        {

            return base.Channel.GetOccurrenceSpanInfo(Claims_Occurrence_Code_Span_Information_ID, Claim_ID);

        }

        public DataSet GetValueCodeData(string Claims_Value_Code_Information_ID, string Claim_ID)

        {

            return base.Channel.GetValueCodeData(Claims_Value_Code_Information_ID, Claim_ID);

        }

        public DataSet GetConditionCodeData(string Claims_Value_Code_Information_ID, string Claim_ID)

        {

            return base.Channel.GetConditionCodeData(Claims_Value_Code_Information_ID, Claim_ID);

        }

        public void InsertRegNPIEnrollment(int regId, int processId, string providerEffectiveDate)
        {
            base.Channel.InsertRegNPIEnrollment(regId, processId, providerEffectiveDate);
        }

        public DataSet EditNDCCodeData(string Claims_NDC_Details_Screen_ID, string ClaimId)

        {

            return base.Channel.EditNDCCodeData(Claims_NDC_Details_Screen_ID, ClaimId);

        }



        public DataSet GetICDProcCode(string claim_id, string claim_icd_procdeure_code)

        {

            return base.Channel.GetICDProcCode(claim_id, claim_icd_procdeure_code);

        }

        public DataSet EditOccurrenceInfoData(string Claims_Occurrence_Information_ID, string Claim_ID)

        {

            return base.Channel.EditOccurrenceInfoData(Claims_Occurrence_Information_ID, Claim_ID);

        }

        public DataSet EditOtherPayerAdjustmentInfoServiceDetails(string Other_Payer_Adjustment_Service_Detail_ID, string Claim_ID)

        {

            return base.Channel.EditOtherPayerAdjustmentInfoServiceDetails(Other_Payer_Adjustment_Service_Detail_ID, Claim_ID);

        }



        public DataSet Get_ToothQuadrantInfoData(string Claim_ID, string Claims_Tooth_and_Surface_Information_ID)

        {

            return base.Channel.Get_ToothQuadrantInfoData(Claim_ID, Claims_Tooth_and_Surface_Information_ID);

        }

        public DataSet GetOPPAServiceDetail(string ClaimId, string OPPAServiceDetailId)

        {

            return base.Channel.GetOPPAServiceDetail(ClaimId, OPPAServiceDetailId);

        }

        public DataSet VerifyCPCKidsAttestationSelected(int regID)
        {

            return base.Channel.VerifyCPCKidsAttestationSelected(regID);

        }

        // OHPNM-14997
        public void ArchiveDocumentToGenericUploadControl(int regID, int documentID, string regPageSection, string wfTaskName, string docUploadPage)

        {

            base.Channel.ArchiveDocumentToGenericUploadControl(regID, documentID, regPageSection, wfTaskName, docUploadPage);

        }

        //public DataSet SearchTransactionsMonitor(string regId, string medicaidID, string fromDate, string toDate, string sortBy, int pageSize, int pageNumber, bool asc, out int totalResultCount)

        //{

        //    return base.Channel.SearchTransactionsMonitor(regId, medicaidID, fromDate, toDate, sortBy, pageSize, pageNumber, asc, out totalResultCount);

        //}



        //public DataSet SearchByREGID(string regId, string tableName, int pageSize, int pageNumber, string medId = null)

        //{

        //    return base.Channel.SearchByREGID(regId, tableName, pageSize, pageNumber, medId);

        //}



        //public void InsertTransactionMonitorNote(int regID, int noteTypeID, string noteText, DateTime noteDate, string userId)

        //{

        //    base.Channel.InsertTransactionMonitorNote(regID, noteTypeID, noteText, noteDate, userId);

        //}



        //public DataSet GetTransactionsMonitorCounts()

        //{

        //    return base.Channel.GetTransactionsMonitorCounts();

        //}



        //public void UpdateTransactionAssignedStatus(int transactionQueueID, string assignedStatus, string userId)

        //{

        //    base.Channel.UpdateTransactionAssignedStatus(transactionQueueID, assignedStatus, userId);

        //}

        public DataSet EditAdditionalProviderInformation(string Claim_ID, string Claims_Additional_Provider_Information_Service_ID)

        {

            return base.Channel.EditAdditionalProviderInformation(Claim_ID, Claims_Additional_Provider_Information_Service_ID);

        }

        //OHPNM-10741
        public int InsertPAOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested, string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId, string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalDocumentName)
        {
            return base.Channel.InsertPAOutBoundDocumentUploads(editTransactionTypeId, payerRequested,
            memberId, claimTypeId, claimNumber, paNumber, providerId, providerNPI, senderId,
            receiverId, documentTypeId, documentName, uuid, toSend, appAdminUserId, documentIdentifier, originalDocumentName);
        }

        //public void InsertOutBoundDocumentUploads(int editTransactionTypeId, string payerRequested, string memberId, int claimTypeId, string claimNumber, string paNumber, string providerId, string providerNPI, string senderId, string receiverId, int documentTypeId, string documentName, Guid uuid, bool toSend, Guid appAdminUserId, string documentIdentifier, string originalDocumentName)
        //{
        //    base.Channel.InsertOutBoundDocumentUploads(editTransactionTypeId, payerRequested,
        //    memberId, claimTypeId, claimNumber, paNumber, providerId, providerNPI, senderId,
        //    receiverId, documentTypeId, documentName, uuid, toSend, appAdminUserId, documentIdentifier, originalDocumentName);
        //}

        public DataSet GetPriorAuthStatusCodes(string priorAuthStatus)
        {
            return base.Channel.GetPriorAuthStatusCodes(priorAuthStatus);
        }

        // Gets the data if any previous registrations have same NPI  
        public DataSet GetExistingRegDataForNPI(string NPI, int regId, string mmisProviderTypeID, bool isKeyFieldEditRequest)
        {
            return base.Channel.GetExistingRegDataForNPI(NPI, regId, mmisProviderTypeID, isKeyFieldEditRequest);
        }

        public void CheckEligibletoConvertFromORP(int regID, out bool isEligible)
        {
            base.Channel.CheckEligibletoConvertFromORP(regID, out isEligible);
        }
        public DataSet GetPowerAgentInformation(string loggedinUserID, string provAdminUserID, int regID)
        {
           return base.Channel.GetPowerAgentInformation(loggedinUserID, provAdminUserID, regID);
        }        
        /// <summary>
        /// Get the screening Activity data based on the REG_ID and SCREENING_ACTIVITY_TYPE_ID
        /// </summary>
        /// <param name="regId"></param>
        /// <param name="screeningActivityTypeId"></param>
        /// <returns></returns>
        public DataSet GetProviderScreeningActivityByRegIdAndActivityType(int regId, int screeningActivityTypeId)
        {
            return base.Channel.GetProviderScreeningActivityByRegIdAndActivityType(regId, screeningActivityTypeId);
        }


        public System.Data.DataSet GetCHOPHistory(int regId, int pageSize, int pageNumber)
        {
            return base.Channel.GetCHOPHistory(regId, pageSize, pageNumber);
        }

        public System.Data.DataSet GetClosureHistory(int regId, int pageSize, int pageNumber)
        {
            return base.Channel.GetClosureHistory(regId, pageSize, pageNumber);
        }

        public int ValidateNPIandMEDidData(int regID, int ProcessID, string currentAction)
        {
            return base.Channel.ValidateNPIandMEDidData(regID, ProcessID, currentAction);
        }
        public System.Data.DataSet SelectProviderTypeChangeRequestByOldReg(int regID)
        {
            return base.Channel.SelectProviderTypeChangeRequestByOldReg(regID);
        }
        public System.Data.DataSet SelectProviderTypeChangeRequestByNewReg(int regID, int ProcessID)
        {
            return base.Channel.SelectProviderTypeChangeRequestByNewReg(regID, ProcessID);
        }
        public void SaveProviderTypeChangeRequest(int oldRegId, int newRegId, int processId, int status, DateTime createDate, Guid createdBy)
        {
            base.Channel.SaveProviderTypeChangeRequest(oldRegId, newRegId, processId, status, createDate, createdBy);
        }
        public void UpdateProviderTypeChangeStatus(int newRegId, int status, DateTime UpdDate, string UpdBy)
        {
            base.Channel.UpdateProviderTypeChangeStatus(newRegId, status, UpdDate, UpdBy);
        }
        public DataSet GetNpiMedIDEnrollmentSpanActions()
        {
            return base.Channel.GetNpiMedIDEnrollmentSpanActions();
        }
        public DataSet GetDataFixActions(int regid = 0, string medicaid_id = "")
        {
            return base.Channel.GetDataFixActions(regid, medicaid_id);
        }

        public DataSet GetRegIdByMedId(string Medid)
        {
            return base.Channel.GetRegIdByMedId(Medid);
        }

        public DataSet GetDataFixTablesDetails(string tabletype)
        {
            return base.Channel.GetDataFixTablesDetails(tabletype);
        }

        public DataSet GetDataFixTableSpecificDetails(string tableName)
        {
            return base.Channel.GetDataFixTableSpecificDetails(tableName);
        }

        public DataSet GetTableColumnsByTableName(string tableName, string tableOperation)
        {
            return base.Channel.GetTableColumnsByTableName(tableName, tableOperation);
        }
        public System.Data.DataSet GetUploadAttachmentStatus(string memberid, string providerid, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            return base.Channel.GetUploadAttachmentStatus(memberid, providerid, pageSize, startRowIndex, getTotalRowCount, out totalResultCount);
        }

        public string GetUserIOPRefreshTokenInfoByGuid(Guid userID)
        {
            return base.Channel.GetUserIOPRefreshTokenInfoByGuid(userID);
        }

        public string GetUserIOPAccessTokenInfoByGuid(Guid userID)
        {
            return base.Channel.GetUserIOPAccessTokenInfoByGuid(userID);
        }

        public int InsertProviderFeedNotes(int regId, int regProviderFeedId, string initiatedBy, string note,
            string personReviewedBy = null, string enrollmentType = null, string finalDisposition = null, int processID = 0)
        {
            return base.Channel.InsertProviderFeedNotes(regId, regProviderFeedId, initiatedBy, note, personReviewedBy, enrollmentType, finalDisposition, processID);
        }
    }
}