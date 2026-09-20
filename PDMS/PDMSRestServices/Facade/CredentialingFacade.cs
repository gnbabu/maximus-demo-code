using Amazon.Runtime.Internal;
using Corp.Core.Libraries;
using Corp.Core.Libraries.ProviderManagementReference;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Microsoft.AspNetCore.Mvc;
using MigraDoc.DocumentObjectModel.Tables;
using PDMSRestServices.Models;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Workflow;
using static MAXIMUS.Core.Libraries.Constants;



namespace PDMSRestServices.Facade
{
    public static class CredentialingFacade
    {

        public static async Task<List<AffiliationMinimalDto>> SelectGroupAffiliationByRegIDAsync(int regId, CancellationToken ct = default)
        {
            var list = new List<AffiliationMinimalDto>();

            if (regId <= 0) throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet pendingAffiliationsDS = RegistrationController.SelectGroupAffiliationByRegID(regId);

            DataTable? pendingTable = pendingAffiliationsDS?.Tables?.Count > 0 ? pendingAffiliationsDS.Tables[0] : null;

            if (pendingTable != null || pendingTable.Rows.Count != 0)
            {

                foreach (DataRow row in pendingTable.Rows)
                {
                    var dto = AffiliationMapper.ToMinimal(row);
                    list.Add(dto);
                }

            }

            return list;
        }

        public static async Task<List<AffiliationMinimalDto>> SelectPendingAffiliationByRegIDAsync(int regId, CancellationToken ct = default)
        {
            var list = new List<AffiliationMinimalDto>();

            if (regId <= 0) throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet pendingAffiliationsDS = RegistrationController.SelectPendingAffiliationByRegID(regId);
            DataSet confirmAffiliationsDS = RegistrationController.SelectConfirmedAffiliationByRegID(regId);

            DataTable? pendingTable = pendingAffiliationsDS?.Tables?.Count > 0 ? pendingAffiliationsDS.Tables[0] : null;

            if (pendingTable != null || pendingTable.Rows.Count != 0)
            {

                foreach (DataRow row in pendingTable.Rows)
                {
                    var dto = AffiliationMapper.ToMinimal(row);
                    dto.Status = "Pending";
                    list.Add(dto);
                }

            }

            DataTable? confirmTable = confirmAffiliationsDS?.Tables?.Count > 0 ? confirmAffiliationsDS.Tables[0] : null;

            if (confirmTable != null || confirmTable.Rows.Count != 0)
            {

                foreach (DataRow row in confirmTable.Rows)
                {
                    var dto = AffiliationMapper.ToMinimal(row);
                    dto.Status = "Confirmed";
                    list.Add(dto);
                }

            }

            return list;
        }
        public static async Task<List<HospitalAffiliationDTO>> SelectHealthCareFacilityAffiliationByRegIDAsync(int regId, CancellationToken ct = default)
        {
            var list = new List<HospitalAffiliationDTO>();

            if (regId <= 0) throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet affiliationsDS = RegistrationController.SelectHealthCareFacilityAffiliationByRegID(regId);


            DataTable? affiliationsDSTable = affiliationsDS?.Tables?.Count > 0 ? affiliationsDS.Tables[0] : null;

            if (affiliationsDSTable != null || affiliationsDSTable.Rows.Count != 0)
            {

                foreach (DataRow row in affiliationsDSTable.Rows)
                {
                    var dto = AffiliationMapper.ToHospitalAffiliation(row);

                    list.Add(dto);
                }
            }
            return list;
        }


        public static async Task<List<WorkflowStepsDTO>> SelectWorkflowStepsByRegIDAsync(int processID, CancellationToken ct = default)
        {
            var list = new List<WorkflowStepsDTO>();

            if (processID <= 0) throw new ArgumentOutOfRangeException(nameof(processID));

            DataSet workflowStepsDS = RegistrationController.GetWorkflowStepsForProcessID(processID);


            DataTable? workflowStepsDSTable = workflowStepsDS?.Tables?.Count > 0 ? workflowStepsDS.Tables[0] : null;

            if (workflowStepsDSTable != null || workflowStepsDSTable.Rows.Count != 0)
            {

                foreach (DataRow row in workflowStepsDSTable.Rows)
                {
                    var dto = AffiliationMapper.MapWorkflowSteps(row);

                    list.Add(dto);
                }
            }
            return list;
        }

        public static async Task<List<CredentialingDocumentDTO>> SelectDocumentsAsync(int regId, string section, CancellationToken ct = default)
        {
            var list = new List<CredentialingDocumentDTO>();

            if (regId <= 0) throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet credentialingDocumentsDS = RegistrationController.SelectCredentialingDocuments(regId, section);

            DataTable? credentialingDocumentsDSTable = credentialingDocumentsDS?.Tables?.Count > 0 ? credentialingDocumentsDS.Tables[0] : null;

            if (credentialingDocumentsDSTable != null || credentialingDocumentsDSTable.Rows.Count != 0)
            {

                foreach (DataRow row in credentialingDocumentsDSTable.Rows)
                {
                    var dto = CredentialingDocumentMapper.ToDto(row);

                    list.Add(dto);
                }
            }
            return list;
        }
        public static async Task<List<SatellitePracticeLocationDto>> SelectAddressCustomDataPagingNewAsync(int regId, int addresstype, CancellationToken ct = default)
        {
            var list = new List<SatellitePracticeLocationDto>();

            if (regId <= 0) throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet addressLocationsDS = RegistrationController.SelectAddressCustomDataPagingNew(regId, addresstype, 1, 50);


            DataTable? addressLocationsDSTable = addressLocationsDS?.Tables?.Count > 0 ? addressLocationsDS.Tables[0] : null;

            if (addressLocationsDSTable != null || addressLocationsDSTable.Rows.Count != 0)
            {

                foreach (DataRow row in addressLocationsDSTable.Rows)
                {
                    var dto = SatellitePracticeLocationMapper.ToDto(row);

                    list.Add(dto);
                }
            }
            return list;
        }

        public static Task<List<SpecialtyDto>> SelectSpecialtyAsync(int regId, CancellationToken ct = default) => RegistrationDataReader.FetchAndMapAsync(
         regId, filter: null, fetchDataSet: (id, _) =>
         RegistrationController.SelectRegistrationDataWithParams("usp_GetSpecialtyByRegID",
             new Dictionary<string, string> { ["REG_ID"] = id.ToString() }), mapRow: SpecialtyMapper.MapFromDataRow, ct: ct);


        public static Task<List<TaxonomyDto>> SelectTaxonomyAsync(int regId, CancellationToken ct = default) => RegistrationDataReader.FetchAndMapAsync(
        regId, filter: null, fetchDataSet: (id, _) =>
        RegistrationController.SelectRegistrationDataWithParams("usp_GetTaxonomyByRegID",
            new Dictionary<string, string> { ["REG_ID"] = id.ToString() }), mapRow: TaxonomyMapper.MapFromDataRow, ct: ct);

        public static Task<List<ProfessionalLicenseDto>> SelectProfessionalLicensesAsync(int regId, string licenseType, CancellationToken ct = default) =>
         RegistrationDataReader.FetchAndMapAsync(regId, licenseType, RegistrationController.SelectRegistrationData, ProfessionalLicenseMapper.ToDto, ct);
        public static Task<List<BoardCertificationDto>> SelectBoardCertificationsAsync(int regId, string certificationType, CancellationToken ct = default) => RegistrationDataReader.FetchAndMapAsync(regId, certificationType,
                RegistrationController.SelectRegistrationData, BoardCertificationMapper.ToDto, ct);
        public static Task<List<CliaCertificationDto>> SelectCliaCertificationsAsync(int regId, string? certTypeFilter = null, CancellationToken ct = default) =>
            RegistrationDataReader.FetchAndMapAsync(regId, certTypeFilter, RegistrationController.SelectRegistrationData, CliaCertificationMapper.ToDto, ct);
        public static Task<List<DeaCertificateDto>> SelectDeaCertificatesAsync(int regId, string? stateFilter = null, CancellationToken ct = default) =>
            RegistrationDataReader.FetchAndMapAsync(regId, stateFilter, RegistrationController.SelectRegistrationData, DeaCertificateMapper.ToDto, ct);
        public static Task<List<CdsCertificateDto>> SelectCdsCertificatesAsync(int regId, string? stateFilter = null, CancellationToken ct = default) =>
            RegistrationDataReader.FetchAndMapAsync(regId, stateFilter, RegistrationController.SelectRegistrationData, CdsCertificateMapper.ToDto, ct);
        public static Task<List<EducationDto>> SelectEducationAsync(int regId, CancellationToken ct = default) => RegistrationDataReader.FetchAndMapAsync(
         regId, filter: null, fetchDataSet: (id, _) =>
         RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_EDUCATION",
             new Dictionary<string, string> { ["REG_ID"] = id.ToString() }), mapRow: EducationMapper.ToDto, ct: ct);
        public static Task<List<WorkHistoryDto>> SelectWorkHistoryAsync(int regId, CancellationToken ct = default) => RegistrationDataReader.FetchAndMapAsync(
                     regId, filter: null, fetchDataSet: (id, _) => RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_WORKHISTORY",
                         new Dictionary<string, string> { ["REG_ID"] = id.ToString() }), mapRow: WorkHistoryMapper.ToDto, ct: ct);

        public static Task<List<SubmittedAgreementDto>> SelectSubmittedAgreementsAsync(int regId, int regPageTypeId, string regPageSection, CancellationToken ct = default) =>
                 RegistrationDataReader.FetchAndMapAsync(regId, filter: null, fetchDataSet: (id, _) =>
                 RegistrationController.SelectSubmittedAgreementPDFs(id, regPageTypeId, regPageSection, 100, 0, 0), mapRow: SubmittedAgreementMapper.ToDto, ct: ct);
        public static Task<List<InsuranceDto>> SelectInsuranceAsync(int regId, CancellationToken ct = default)
                => RegistrationDataReader.FetchAndMapAsync(regId, filter: null, fetchDataSet: (id, _) =>
                RegistrationController.SelectRegistrationData(regId, "INSURANCE"), mapRow: InsuranceMapper.ToDto, ct: ct);
        public static Task<List<MalpracticeClaimDto>> SelectMalpracticeClaimsAsync(int regId, CancellationToken ct = default) =>
                RegistrationDataReader.FetchAndMapAsync(regId, filter: null, fetchDataSet: (id, _) =>
                RegistrationController.SelectMalpracticeClaimByRegID(regId), mapRow: MalpracticeClaimMapper.ToDto, ct: ct);
        public static IList<ProviderSearchResponse> SearchGroupProviders(ProviderSearchRequest request, out int totalResultCount)
        {

            Dictionary<string, object> parms = request.ToParams();
            DataSet ds = ProviderController.SearchGroupProviders(parms, out totalResultCount);
            var results = ProviderSearchResponseMapper.FromDataSet(ds);
            return results;
        }
        public static IList<CvoQueueModel> GetCvoQueue(string userId, string roleName)
        {
            // includeOtherSteps is always false as per existing WF usage
            bool includeOtherSteps = false;

            // Call workflow/controller layer
            DataSet ds = RegistrationController.SelectActiveOwnerStepsProvider(
                userId,
                includeOtherSteps,
                roleName
            );

            // Map DataSet to strongly typed models
            var results = CvoQueueModelMapper.FromDataSet(ds);

            return results;
        }
        public static IList<CommitteeQueue> GetCommitteeQueue(string userId, string roleName, bool includeOtherSteps = false)
        {

            DataSet ds = RegistrationController.WF_SelectActiveOwnerStepsCredentialProvider(userId, includeOtherSteps, roleName);

            // Map DataSet to strongly typed models
            var results = CommitteeQueueModelMapper.FromDataSet(ds);

            return results;
        }
        public static List<CorrespondenceResponse> GetCorrespondence(CorrespondenceRequest request)
        {
            // Call stored procedure / controller to retrieve DataSet
            int? regId = string.IsNullOrWhiteSpace(request.RegId)
                ? (int?)null
                : Convert.ToInt32(request.RegId);

            CorrespodenceInfo CorrespodenceInfo =
                PriorAuthHospitalController.GetSearchCorrespondenceType(
                    Convert.ToInt16(request.CorrespondenceType),
                    Guid.Parse(request.UserId),
                    request.FromDate,
                    request.ToDate,
                    regId,                     // ✅ nullable
                    request.Npi,
                    request.MedicaidId,
                    request.SortDirection,
                    request.SortField,
                    request.PageSize,
                    request.PageIndex);


            return CorrespondenceMapper.MapFromDataSet(CorrespodenceInfo.CorrespondenceInfo);

        }

        public static void UpdateViewed(string documentId)
        {
            if (string.IsNullOrEmpty(documentId))
                throw new ArgumentException("DocumentId must be provided.");

            PriorAuthHospitalController.UpdateViewed(documentId);
        }

        public static IList<ProviderCredentialActivity> GetProviderCredentialActivitiesHistory(int credentialingId, int regID)
        {
            // 🔹 STEP 1: Validate via Match Data
            DataSet matchDataSet =
                CredentialController.SelectProviderCredentialActivityMatchData(
                    credentialingId, regID);

            if (!HasValidMatch(matchDataSet))
            {
                // Business rule: do not return activities unless match exists
                return new List<ProviderCredentialActivity>();
            }

            // 🔹 STEP 2: Load Credential Activities
            DataSet activityDataSet =
                CredentialController.SelectProviderCredentialActivityData(
                    regID, credentialingId);

            if (activityDataSet == null || activityDataSet.Tables.Count == 0)
                return new List<ProviderCredentialActivity>();

            return ProviderCredentialActivityMapper.FromDataTable(
                activityDataSet.Tables[0]);
        }

        public static IList<ProviderCredentialActivity> GetProviderCredentialActivities(int registrationId)
        {
            int credentialId = ResolveCredentialId(registrationId);

            // 🔹 STEP 1: Validate via Match Data
            DataSet matchDataSet =
                CredentialController.SelectProviderCredentialActivityMatchData(
                    credentialId, registrationId);

            if (!HasValidMatch(matchDataSet))
            {
                // Business rule: do not return activities unless match exists
                return new List<ProviderCredentialActivity>();
            }

            // 🔹 STEP 2: Load Credential Activities
            DataSet activityDataSet =
                CredentialController.SelectProviderCredentialActivityData(
                    registrationId, credentialId);

            if (activityDataSet == null || activityDataSet.Tables.Count == 0)
                return new List<ProviderCredentialActivity>();

            return ProviderCredentialActivityMapper.FromDataTable(
                activityDataSet.Tables[0]);
        }
        public static IList<CredentialActivityHistory> GetCredentialActivityHistory(int registrationId)
        {

            DataSet matchDataSet = CredentialController.SelectProviderCredentialingData(registrationId);

            return CredentialActivityHistoryMapper.FromDataTable(matchDataSet.Tables[0]);
        }
        private static int ResolveCredentialId(int registrationId)
        {
            DataSet ds =
                CredentialController.SelectProviderCredentialingData(registrationId);

            if (ds != null &&
                ds.Tables.Count > 0 &&
                ds.Tables[0].Rows.Count > 0)
            {
                return Convert.ToInt32(ds.Tables[0].Rows[0]["CREDENTIALING_ID"]);
            }

            // Matches legacy fallback behavior
            return registrationId;
        }
        private static bool HasValidMatch(DataSet matchDataSet)
        {
            return matchDataSet != null &&
                   matchDataSet.Tables.Count > 1 &&
                   matchDataSet.Tables[1].Rows.Count > 0;
        }
        public static IList<ProviderReCredentialingDue> GetProviderReCredentialingDueReport(DateTime startDate, DateTime endDate)
        {
            DataSet ds = RegistrationController.Reports_SelectProviderReCredentialingDueReport(
                startDate,
                endDate);

            // Map DataSet to strongly typed models
            var results = ProviderReCredentialingDueModelMapper.FromDataSet(ds);

            return results;
        }
        public static IList<ProviderCredentialingApproved> GetProviderCredentialingApprovedReport(DateTime startDate, DateTime endDate)
        {
            DataSet ds = RegistrationController
                .Reports_SelectProviderCredentialingApproved(startDate, endDate);

            return ProviderCredentialingApprovedModelMapper.FromDataSet(ds);
        }
        public static IList<ProviderCredentialingCommitteeDecision> GetProviderCredentialingCommitteeDecisionReport(DateTime startDate, DateTime endDate)
        {
            DataSet ds = RegistrationController
                .Reports_SelectProviderCredentialingCommitteeDecision(startDate, endDate);

            return ProviderCredentialingCommitteeDecisionModelMapper.FromDataSet(ds);
        }
        public static IList<ProviderReCredentialingOverdue> GetProviderReCredentialingOverdueReport(DateTime startDate, DateTime endDate)
        {
            DataSet ds = RegistrationController
                .Reports_SelectProviderReCredentialingOverdue(startDate, endDate);

            return ProviderReCredentialingOverdueModelMapper.FromDataSet(ds);
        }
        public static IList<CredentialingFileProcessingSummary> GetCredentialingFileProcessingSummaryReport(DateTime startDate, DateTime endDate)
        {
            DataSet ds = RegistrationController
                .Reports_SelectCredentialingFileProcessingSummary(startDate, endDate);

            return CredentialingFileProcessingSummaryModelMapper.FromDataSet(ds);
        }
        public static IList<ProviderDocument> GetProviderDocuments(ProviderDocumentRequest request)
        {
            // Base dataset
            DataSet ds = RegistrationController.SelectRegDocuments(request.RegId, request.RegPageTypeId, string.Empty,
                request.Exclusions, null, request.UserRole);

            // Include Education / Work History documents if required
            if (request.ShowEducationWorkDocs)
            {
                DataSet dsEducation = RegistrationController.SelectRegDocuments(request.RegId, SectionTypeID.WorkHistory, string.Empty,
                    request.Exclusions, null, request.UserRole);

                DataSet dsWorkHistory = RegistrationController.SelectRegDocuments(request.RegPageTypeId, SectionTypeID.WorkHistory, string.Empty,
                    request.Exclusions, null, request.UserRole);

                ds.Merge(dsEducation);
                ds.Merge(dsWorkHistory);
            }

            // No rows scenario
            if (!Helper.HasRows(ds))
            {
                return ProviderDocumentMapper.MapFromDataSet(new DataSet());
            }

            // Apply registration page filtering
            DataView view = new DataView(ds.Tables[0]);

            if (request.RegistrationStep != SectionTypeID.ClosureNotice && request.RegistrationStep != SectionTypeID.DaysNotice)
            {
                view.RowFilter = "REG_PAGE_NAME IS NULL OR REG_PAGE_NAME LIKE '%'";
            }

            DataTable filteredTable = view.ToTable();

            // Application Fee specific logic
            if (Helper.HasRows(filteredTable) && request.RegPageTypeId == RegistrationPageType.ApplicationFee)
            {
                var appFeeRows = filteredTable.AsEnumerable().Where(r => r.Field<string>("Document_Upload_Page") == "Application Fee Information");

                DataSet appFeeDataSet = new DataSet();

                if (appFeeRows.Any())
                {
                    appFeeDataSet.Tables.Add(appFeeRows.CopyToDataTable());
                }
                return ProviderDocumentMapper.MapFromDataSet(appFeeDataSet);
            }

            // Default return path
            DataSet resultDataSet = new DataSet();

            resultDataSet.Tables.Add(filteredTable);

            List<ProviderDocument> providerDocuments = ProviderDocumentMapper.MapFromDataSet(resultDataSet);

            return providerDocuments;
        }


        public static Task<List<MembershipUserDto>> GetMemberUserAsync(Guid userId, CancellationToken ct = default)
                 => MembershipUserMapper.FetchAndMapAsync<Guid, MembershipUserDto>(userId, fetchDataSet: id => RegistrationController.SelectRegistrationDataWithParams(
                       "aspnet_Membership_GetUserByUserId",
                       new Dictionary<string, string>
                       {
                           ["UserId"] = id.ToString(),
                           ["CurrentTimeUtc"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                           ["UpdateLastActivity"] = "0"
                       }), mapRow: MembershipUserMapper.ToDto, isValidId: id => id != Guid.Empty, ct: ct);


        public static ApproveProvidersResponse ApproveProviders(List<ProviderApprovalRequest> requests)
        {
            if (requests == null || requests.Count == 0)
            {
                return new ApproveProvidersResponse
                {
                    TotalRequested = 0,
                    TotalSucceeded = 0,
                    Results = new List<ApproveProviderResult>()
                };
            }

            var results = new List<ApproveProviderResult>();

            foreach (var approvalRequest in requests)
            {
                try
                {
                    if (approvalRequest.RegId <= 0 ||
                        approvalRequest.CredentialingId <= 0 ||
                        approvalRequest.ProcessId <= 0)
                    {
                        results.Add(new ApproveProviderResult
                        {
                            RegId = approvalRequest.RegId,
                            Success = false,
                            Message = "Invalid identifiers supplied."
                        });
                        continue;
                    }

                    // ================= Business Logic =================

                    var parms = new Dictionary<string, string>
                    {
                        { "REG_ID", approvalRequest.RegId.ToString() },
                        { "CREDENTIALING_ID", approvalRequest.CredentialingId.ToString() },
                        { "Committee_Result_id", Constants.CredentialingResult.Pass.ToString() },
                        { "COMMITTEE_DENIAL_TERM_REASON", string.Empty },
                        { "Committee_summary", string.Empty },
                        { "Committee_Date", DateTime.Now.ToString() },
                        { "Committee_Discussion", string.Empty },
                        { "Last_Modified_Date_Time", DateTime.Now.ToString() },
                        { "Last_Modified_User", approvalRequest.UserId }
                    };

                    RegistrationController.UpdateRegistrationData("CREDENTIALING", parms);

                    CredentialController.UpdateCredentialResult(
                        approvalRequest.RegId,
                        Constants.CredentialingResult.Pass,
                        DateTime.Now,
                        approvalRequest.UserId
                    );

                    CredentialController.UpdateCredentialStatus(
                        approvalRequest.RegId,
                        Constants.CredentilaingStatus.Approved,
                        DateTime.Now,
                        approvalRequest.UserId
                    );

                    CredentialController.TakeAction(
                        approvalRequest.ProcessId,
                        "Approve",
                        string.Empty
                    );

                    int regPageTypeId = 28;
                    int regSectionTypeId = 51;

                    if (approvalRequest.IsUserInPSRoles ||
                        approvalRequest.IsUserInCredentialingRole)
                    {
                        RegistrationController.SaveRegistrationSectionStatus(
                            approvalRequest.RegId,
                            regPageTypeId,
                            regSectionTypeId,
                            null,
                            1,
                            DateTime.Now,
                            approvalRequest.UserId
                        );
                    }
                    else
                    {
                        RegistrationController.SaveRegistrationSectionStatus(
                            approvalRequest.RegId,
                            regPageTypeId,
                            regSectionTypeId,
                            1,
                            null,
                            DateTime.Now,
                            approvalRequest.UserId
                        );
                    }

                    results.Add(new ApproveProviderResult
                    {
                        RegId = approvalRequest.RegId,
                        Success = true,
                        Message = "Provider credentialing approved successfully."
                    });
                }
                catch (OperationCanceledException)
                {
                    results.Add(new ApproveProviderResult
                    {
                        RegId = approvalRequest.RegId,
                        Success = false,
                        Message = "Request canceled by client."
                    });
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    results.Add(new ApproveProviderResult
                    {
                        RegId = approvalRequest.RegId,
                        Success = false,
                        Message = ex.Message
                    });
                }
                catch (Exception)
                {
                    results.Add(new ApproveProviderResult
                    {
                        RegId = approvalRequest.RegId,
                        Success = false,
                        Message = "Internal Server Error"
                    });
                }
            }

            return new ApproveProvidersResponse
            {
                TotalRequested = requests.Count,
                TotalSucceeded = results.Count(r => r.Success),
                Results = results
            };
        }


        public static async Task<AssignedToResponse> GetAssignedToResponse(int regId, int currentStepId, bool isUserinRole, CancellationToken ct = default)
        {
            // Default empty response
            AssignedToResponse response = new AssignedToResponse();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("StepID", DbType.Int32, currentStepId, true));
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_WF_SelectStepInfo", parameters, "StepInfo");

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];

                string groupNameList = row["GROUP_NAME_LIST"]?.ToString();
                string stepOwner = row["STEP_OWNER_ID"]?.ToString();
                string taskName = row["TASK_NAME"]?.ToString();

                response = await SetupAssignedToAsync(StepOwner: stepOwner, GroupNameList: groupNameList,
                    TaskName: taskName, regId: regId, isUserinRole: isUserinRole, ct: ct);
            }

            return response;
        }

        public static async Task<UpdateCredentialStatusResponse> UpdateCredentialStatus(int registrationId, int statusId, string userId, CancellationToken ct = default)
        {
            var response = new UpdateCredentialStatusResponse();

            CredentialController.UpdateCredentialStatus(registrationId, statusId, DateTime.Now, userId);

            response.message = "Success";
            return response;
        }

        public static void InitiateCredentialingWorkflow(string npi, Guid userid)
        {
            CredentialController.InitiateCredentialingWorkflow(
                npi,
                userid
            );
        }

        public static void CancelCredentialingEvent(string npi, Guid userid)
        {
            CredentialController.CancelCredentialingEvent(
                npi,
                userid
            );
        }


        public static async Task<List<HelpData>> GetHelpText(
            string pageName,
            string userId,
            CancellationToken ct = default)
        {
            var response = new List<HelpData>();

            DataSet ds = CredentialController.SelectPageContentByPageName(pageName, userId);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var item = new HelpData
                    {
                        Mode = row["mode"]?.ToString(),
                        Title = row["title"]?.ToString(),
                        Content = row["displaytext"]?.ToString(),
                        PdfUrl = row["pdfurl"]?.ToString()
                    };

                    response.Add(item);
                }
            }

            return response;
        }

        public static async Task<UpdateCredentialStatusResponse> DeleteProviderDocument(
            int documentId,
            string userId,
            CancellationToken ct = default)
        {
            var response = new UpdateCredentialStatusResponse();

            if (documentId <= 0)
            {
                response.message = "Invalid DocumentId";
                return response;
            }

            var isDeleted = CredentialController.DeleteProviderDocument(documentId, userId);

            if (!isDeleted)
            {
                response.message = "Document not found or already deleted.";
                return response;
            }

            response.message = "Success";
            return response;
        }

        public static async Task<AssignedToUserResponse> AssignedToUser(string regIdBulk, string assignedUser, string assignedUserId, string userId, CancellationToken ct = default)
        {
            var response = new AssignedToUserResponse();

            if (string.IsNullOrWhiteSpace(regIdBulk))
                throw new ArgumentException("Invalid regId");

            var regIdList = regIdBulk
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => id.Trim())
                .Select(id =>
                {
                    if (!int.TryParse(id, out int parsedId) || parsedId <= 0)
                        throw new ArgumentException($"Invalid RegId value: {id}");
                    return parsedId;
                })
                .ToList();

            foreach (var regId in regIdList)
            {
                DataSet userData = UserController.GetUserRolesByUser(assignedUser);
                UserRoleDto userRole = UserRoleMapper.Map(userData).SingleOrDefault();

                if (userRole == null)
                {
                    response.message = "Assigned user details not found.";
                    return response;
                }


                DataSet credentialingData = CredentialController.SelectProviderCredentialingData(regId);

                if (!Helper.HasRows(credentialingData))
                {
                    response.message = "No Credentialing data found for the selected RegId.";
                    return response;
                }

                DataTable credTable = credentialingData.Tables[0];


                int activeCredentialingId = Helper.GetInt("credentialing_id", credTable.Rows[0]);

                DataRow[] selectedRows = activeCredentialingId > 0
                    ? credTable.Select($"credentialing_id = {activeCredentialingId}")
                    : credTable.Select($"CREDENTIALING_STATUS_ID IN ({MAXIMUS.Core.Libraries.Constants.CredentilaingStatus.InProcess}, " + $"{MAXIMUS.Core.Libraries.Constants.CredentilaingStatus.PendingProcess})");

                if (selectedRows.Length != 1)
                    return response;

                DataRow credentialRow = selectedRows[0];


                if (userRole.RoleName == MAXIMUS.Core.Libraries.Constants.UserRole.CredentialingChair && credentialRow["RISK_LEVEL_ID"]?.ToString() != "1")
                {
                    response.message = "This is a medium or high risk file and must be dispositioned by a Credentials Committee Quality Specialist.";
                    return response;
                }

                int stepId = GetWorkflowStepId(regId);
                if (stepId == 0)
                {
                    response.message = "StepId not found for the selected RegId.";
                    return response;
                }

                CredentialController.updateWF_STEP_Owner(stepId, string.IsNullOrWhiteSpace(assignedUser) ? null : assignedUserId);

                CredentialController.updateWF_STEP_Assignment(stepId, userId);

                if (userRole.RoleName == MAXIMUS.Core.Libraries.Constants.UserRole.CredentialingSpecialist || userRole.RoleName == MAXIMUS.Core.Libraries.Constants.UserRole.ODMCredentialingSpecialist)
                {
                    CredentialController.UpdateCredentialStatus(regId, MAXIMUS.Core.Libraries.Constants.CredentilaingStatus.InProcess, DateTime.Now, userId);
                }
            }

            response.message = "Success";
            return response;
        }

        private static int GetWorkflowStepId(int regId)
        {
            DataSet ds = RegistrationController.GetGroupNames(regId);

            if (!Helper.HasRows(ds))
                return 0;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                return Helper.GetInt("STEP_ID", row);
            }

            return 0;
        }
        private static string GetGroupNames(int regid)
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = RegistrationController.GetGroupNames(regid);
            if (Helper.HasRows(ds))
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    sb.Append(row["GROUP_NAME"].ToString() + ",");
                }
            }

            string toReturn = sb.ToString();
            toReturn = toReturn.Length > 0 ? toReturn.Remove(sb.Length - 1) : string.Empty;

            return sb.ToString();
        }
        private static async Task<AssignedToResponse> SetupAssignedToAsync(string StepOwner, string GroupNameList, string TaskName, int regId, bool isUserinRole, CancellationToken ct = default)
        {
            var response = new AssignedToResponse();

            if (!string.IsNullOrWhiteSpace(StepOwner))
            {
                var users = await GetMemberUserAsync(new Guid(StepOwner), ct);
                var member = users.FirstOrDefault();

                if (member != null)
                {
                    response.CurrentlyAssignedTo = member.UserName;
                }
            }


            if (!isUserinRole || TaskName == MAXIMUS.Core.Libraries.Constants.RegistrationTaskName.ProviderDataEntry)
            {
                return response;
            }


            string roleList = string.IsNullOrEmpty(GroupNameList) ? GetGroupNames(regId) : GroupNameList;

            // ProviderServices and Operator are the same role
            roleList = roleList.Replace("providerservices", "providerservices,operator");

            string[] roles = roleList.Split(',');

            var userMap = new Dictionary<string, string>();


            foreach (string role in roles.Distinct())
            {
                ct.ThrowIfCancellationRequested();

                DataSet ds;

                if (!role.Trim().Equals(MAXIMUS.Core.Libraries.Constants.ProviderAdministratorRole))
                {
                    ds = UserController.SelectUsersInRoles(role.Trim());
                }
                else
                {
                    ds = UserController.SelectUsersInProviderAdminRole(role.Trim(), regId);
                }

                if (!Helper.HasRows(ds))
                    continue;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string userId = row["UserId"].ToString();
                    string userName = row["UserName"].ToString();

                    if (!userMap.ContainsKey(userId))
                    {
                        userMap.Add(userId, userName);
                    }
                }
            }


            response.Users = userMap.OrderBy(u => u.Value).Select(u => new AssignableUser
            {
                UserId = u.Key,
                UserName = u.Value
            }).ToList();

            return response;
        }


        public static List<AdminActionModel> GetAdminActions()
        {
            return new List<AdminActionModel>
        {
            new() { Action = "Credentialing Event" },
            new() { Action = "Cancel Credentialing Event" }
        };
        }

        public static void UpdateHidePreviousNotesFlag(string regId, string credentialingId, bool hidePreviousNotesFlag, string userId)
        {
            RegistrationController.UpdateHidePreviousNotesFlag(regId, credentialingId, hidePreviousNotesFlag, userId);
        }

        public static void SaveCredentialingPendingVerification(int regID, int credentialingID, bool isInPendingVerification, string userId)
        {
            CredentialController.SaveCredentialingPendingVerification(regID, credentialingID, isInPendingVerification, userId);
        }

        public static bool GetCredentialingPendingVerification(int regId, int credentialingId)
        {
            return CredentialController.GetCredentialingPendingVerification(regId, credentialingId);
        }

        public static int InsertRegDocumentFacade(int regId, int regPageTypeId, string regPageSection, string name, string description, string fileName, string userId, int? screeningActivityID, string wftaskname, string documentUploadPage)
        {
            return CredentialController.InsertRegDocumentCltr(regId, regPageTypeId, regPageSection, name, description, fileName, DateTime.Now, userId, screeningActivityID, wftaskname, documentUploadPage);
        }

        public static void UpdateOnBaseIDFacade(int documentID, int onBaseDocumentID, string changedBy)
        {
            CredentialController.UpdateOnBaseIDCltr(documentID, onBaseDocumentID, changedBy);
        }

    }
}
