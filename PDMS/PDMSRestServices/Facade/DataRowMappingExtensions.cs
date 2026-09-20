using PDMSRestServices.Models;
using System.Data;

namespace PDMSRestServices.Facade
{
    public static class AffiliationMapper
    {
        private static bool HasCol(DataRow row, string name) => row.Table.Columns.Contains(name);

        private static string S(DataRow r, string col) =>
            HasCol(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static DateTime? D(DataRow r, string col)
        {
            if (!HasCol(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParse(s, out var dt)) return dt;
            return null;
        }

        public static AffiliationMinimalDto ToMinimal(DataRow row)
        {
            return new AffiliationMinimalDto
            {
                GroupName = S(row, "GROUPNAME"),
                Npi = S(row, "NPI"),
                StartDate = D(row, "START_DATE"),
                EndDate = D(row, "END_DATE"),
                Status = S(row, "STATUS")
            };
        }

        public static HospitalAffiliationDTO ToHospitalAffiliation(DataRow row)
        {
            return new HospitalAffiliationDTO
            {
                FacilityName = S(row, "FacilityName"),
                FacilityMedicaidID = S(row, "Facility_Medicaid_ID"),
                StaffCategory = S(row, "StaffCategory"),
                StatusofPrivileges = S(row, "StatusofPrivileges"),
                Is_Primary_Facility = B(row, "Is_Primary_Facility"),
                StartDate = D(row, "StartDate"),
                EndDate = D(row, "EndDate")
            };
        }

        public static WorkflowStepsDTO MapWorkflowSteps(DataRow row)
        {
            return new WorkflowStepsDTO
            {
                TaskName = S(row, "Task Name"),
                UserName = S(row, "User Name"),
                StartDate = D(row, "Start Date"),
                EndDate = D(row, "End Date"),
                StepID = S(row, "STEP_ID")
            };
        }

        private static bool B(DataRow row, string columnName)
        {
            if (!row.Table.Columns.Contains(columnName)) return false;

            var val = row[columnName];
            if (val == DBNull.Value || val is null) return false;

            var str = val.ToString()?.Trim().ToLower();

            if (str == "1" || str == "true" || str == "y" || str == "yes") return true;
            if (str == "0" || str == "false" || str == "n" || str == "no") return false;

            return false;
        }
    }
    public static class SatellitePracticeLocationMapper
    {
        private static bool Has(DataRow r, string col) => r.Table.Columns.Contains(col);

        private static string S(DataRow r, string col)
            => Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static int I(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return 0;
            return int.TryParse(Convert.ToString(r[col]), out var v) ? v : 0;
        }

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        public static SatellitePracticeLocationDto ToDto(DataRow row) => new SatellitePracticeLocationDto
        {
            RegAddressId = I(row, "REG_ADDRESS_ID"),
            AdditionalPracticeName = S(row, "ADDN_PRACTICE_NAME"),
            AdditionalPracticeAddress = S(row, "ADDN_PRACTICE_ADDR"),
            AdditionalPracticePhoneNumber = S(row, "ADDN_PRACTICE_PHONE"),
            EffectiveDate = D(row, "EFF_DATE"),
            EndDate = D(row, "END_DATE")
        };
    }
    public static class CredentialingDocumentMapper
    {
        private static bool Has(DataRow r, string col) => r.Table.Columns.Contains(col);

        private static string S(DataRow r, string col)
            => Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static int I(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return 0;
            return int.TryParse(Convert.ToString(r[col]), out var v) ? v : 0;
        }

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        public static CredentialingDocumentDTO ToDto(DataRow row) => new CredentialingDocumentDTO
        {
            DocumentId = I(row, "DocumentId"),
            OnBaseDocumentId = I(row, "OnBaseDocumentId"),
            DocumentType = S(row, "DocumentType"),
            FileName = S(row, "FileName"),
            Source = S(row, "Source"),
            UploadedOn = D(row, "UploadedOn"),
            Status = S(row, "Status")
        };
    }
    public static class DataRowMapper
    {
        private static bool Has(DataRow r, string col) => r?.Table?.Columns?.Contains(col) == true;

        public static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        public static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        /// <summary>
        /// Generic "map a DataRow to a DTO" function using a builder lambda.
        /// </summary>
        public static TDto Map<TDto>(DataRow row, Func<Func<string, string>, Func<string, DateTime?>, TDto> build)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            // Local getters bound to this row
            string SLocal(string col) => S(row, col);
            DateTime? DLocal(string col) => D(row, col);

            return build(SLocal, DLocal);
        }
    }
    public static class ProfessionalLicenseMapper
    {
        public static ProfessionalLicenseDto ToDto(DataRow row) =>
            DataRowMapper.Map(row, (S, D) => new ProfessionalLicenseDto
            {
                LicenseTypeName = S("LICENSE_TYPE_NAME"),
                LicenseNumber = S("LICENSE_NUMBER"),
                LicenseState = S("LICENSE_STATE"),
                IssueDate = D("LICENSE_EFF_DATE"),
                ExpirationDate = D("LICENSE_END_DATE")
            });
    }
    public static class BoardCertificationMapper
    {
        public static BoardCertificationDto ToDto(DataRow row) =>
            DataRowMapper.Map(row, (S, D) => new BoardCertificationDto
            {
                BoardCertificationName = S("Board_Certification_name"),
                BoardSpecialtyName = S("Board_Specialty_name"),
                CertificationNumber = S("CERTIFICATION_NUMBER"),
                EffectiveDate = D("Effective_Date"),
                ExpirationDate = D("Expiration_Date")
            });
    }
    public static class CliaCertificationMapper
    {
        public static CliaCertificationDto ToDto(DataRow row) =>
            DataRowMapper.Map(row, (S, D) => new CliaCertificationDto
            {
                CliaNumber = S("CLIA_NUMBER"),
                CliaCertificationType = S("CLIA_CERT_TYPE"),
                CliaCertificationName = S("CLIA_CERT_NAME"),
                EffectiveDate = D("CLIA_EFF_DATE"),
                ExpirationDate = D("CLIA_END_DATE")
            });
    }
    public static class DeaCertificateMapper
    {
        public static DeaCertificateDto ToDto(DataRow row) =>
            DataRowMapper.Map(row, (S, D) => new DeaCertificateDto
            {
                DeaNumber = S("DEA_NUMBER"),
                State = S("DEA_STATE"),
                EffectiveDate = D("DEA_EFF_DATE"),
                ExpirationDate = D("DEA_END_DATE")
            });
    }
    public static class CdsCertificateMapper
    {
        public static CdsCertificateDto ToDto(DataRow row) =>
            DataRowMapper.Map(row, (S, D) => new CdsCertificateDto
            {
                CdsNumber = S("STATE_CDS_Number"),
                State = S("State"),
                EffectiveDate = D("DateIssued"),
                ExpirationDate = D("ExpirationDate")
            });
    }
    public static class RegistrationDataReader
    {
        /// <summary>
        /// Generic pipeline: validate regId -> fetch DataSet -> first table -> map rows to List&lt;TDto&gt;.
        /// Use when your fetcher signature is (int regId, string? filter) =&gt; DataSet.
        /// </summary>
        public static Task<List<TDto>> FetchAndMapAsync<TDto>(
            int regId,
            string? filter,
            Func<int, string?, DataSet> fetchDataSet,
            Func<DataRow, TDto> mapRow,
            CancellationToken ct = default)
        {
            if (regId <= 0)
                throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet ds = fetchDataSet(regId, filter);

            DataTable? table = (ds?.Tables?.Count ?? 0) > 0 ? ds.Tables[0] : null;

            var list = new List<TDto>();
            if (table is not null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    ct.ThrowIfCancellationRequested();
                    list.Add(mapRow(row));
                }
            }

            return Task.FromResult(list);
        }

        /// <summary>
        /// Overload when your fetcher signature is (int regId) =&gt; DataSet.
        /// </summary>
        public static Task<List<TDto>> FetchAndMapAsync<TDto>(
            int regId,
            Func<int, DataSet> fetchDataSet,
            Func<DataRow, TDto> mapRow,
            CancellationToken ct = default)
        {
            if (regId <= 0)
                throw new ArgumentOutOfRangeException(nameof(regId));

            DataSet ds = fetchDataSet(regId);

            DataTable? table = (ds?.Tables?.Count ?? 0) > 0 ? ds.Tables[0] : null;

            var list = new List<TDto>();
            if (table is not null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    ct.ThrowIfCancellationRequested();
                    list.Add(mapRow(row));
                }
            }

            return Task.FromResult(list);
        }
    }
    public static class EducationMapper
    {
        private static bool Has(DataRow r, string col) => r.Table.Columns.Contains(col);

        private static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        public static EducationDto ToDto(DataRow row) => new EducationDto
        {
            School = S(row, "SCHOOL"),
            EducationType = S(row, "EDUCATION_TYPE"),
            StartDate = D(row, "START_YEAR"),
            EndDate = D(row, "END_YEAR"),
            DegreeOrCertificate = S(row, "DEGREE_AWARD_NAME"),

            Address1 = S(row, "ADDRESS1"),
            Address2 = S(row, "ADDRESS2"),
            City = S(row, "CITY"),
            State = S(row, "STATE"),
            Zip = S(row, "ZIP"),
            Country = S(row, "COUNTRY"),
            PhoneNumber = S(row, "CONTACT_PHONE"),
            EDUCATION_TYPE_DESC = S(row, "EDUCATION_TYPE_DESC")
        };
    }
    public static class WorkHistoryMapper
    {
        private static bool Has(DataRow r, string col) => r?.Table?.Columns?.Contains(col) == true;

        private static string S(DataRow r, string col)
            => Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static int I(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return 0;
            // Supports both int and string inputs
            if (r[col] is int i) return i;
            var s = Convert.ToString(r[col]);
            return int.TryParse(s, out var parsed) ? parsed : 0;
        }

        public static WorkHistoryDto ToDto(DataRow row)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            var dto = new WorkHistoryDto
            {
                WorkHistoryId = I(row, "RECORD_ID"),
                RegId = I(row, "REG_ID"),
                RecordType = S(row, "RECORD_TYPE"),

                EmployerName = S(row, "EMPLOYER_NAME"),

                // Dates are strings from the stored proc: 'MM/DD/YYYY' or 'Present'
                StartDate = S(row, "START_DATE"),
                EndDate = S(row, "END_DATE"),

                FullAddress = S(row, "FULL_ADDRESS"),
                ContactPhoneNumber = S(row, "CONTACT_PHONE_NUMBER"),

                ReasonTextTitle = S(row, "REASON_TEXT_TITLE"),
                ReasonText = S(row, "REASON_TEXT"),

                MilitaryReserve = S(row, "MILTARY_RESERVE"),

                ZipCode = S(row, "ZIP")
            };

            return dto;
        }
    }
    public static class SubmittedAgreementMapper
    {
        private static bool Has(DataRow r, string col) => r?.Table?.Columns?.Contains(col) == true;

        private static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static int I(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return 0;
            return int.TryParse(Convert.ToString(r[col]), out var v) ? v : 0;
        }

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        public static SubmittedAgreementDto ToDto(DataRow row) => new SubmittedAgreementDto
        {
            AgreementSubmittedDate = D(row, "AGREEMENT_SUBMITTED_DATE"),
            AgreementEffectiveDate = D(row, "AGREEMENT_EFFECTIVE_DATE"),
            SubmittedBy = S(row, "UserName"),
            RegId = I(row, "REG_ID"),
            FileName = S(row, "FILE_NAME"),
            OnbaseDocumentId = S(row, "ONBASE_DOCUMENT_ID")
        };
    }
    public static class InsuranceMapper
    {
        private static bool Has(DataRow r, string col) => r?.Table?.Columns?.Contains(col) == true;

        private static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        private static decimal? M(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return decimal.TryParse(s, out var m) ? m : (decimal?)null;
        }

        public static InsuranceDto ToDto(DataRow row) => new InsuranceDto
        {
            CarrierName = S(row, "CarrierName"),
            PolicyNumber = S(row, "POLICY_NUMBER"),
            PolicyHolder = S(row, "PolicyHolder"),
            EffectiveDate = D(row, "EFFECTIVE_DATE"),
            ExpirationDate = D(row, "EXPIRATION_DATE"),
            TypeOfCoverageName = S(row, "TYPE_OF_COVERAGE_NAME"),
            CoverageAmountPerOccurance = M(row, "CoverageAmountPerOccurance"),
            CoverageAmountPerAggregate = M(row, "CoverageAmountPerAggregate")
        };
    }
    public static class MalpracticeClaimMapper
    {
        private static bool Has(DataRow r, string col) => r?.Table?.Columns?.Contains(col) == true;

        private static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        private static decimal? M(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return decimal.TryParse(s, out var m) ? m : (decimal?)null;
        }

        public static MalpracticeClaimDto ToDto(DataRow row) => new MalpracticeClaimDto
        {
            // Note: Source column 'DateOccurance' is misspelled in the data. Mapping as-is.
            DateOfOccurrence = D(row, "DateOccurance"),
            DateClaimFiled = D(row, "DateClaimFiled"),
            CarrierInvolved = S(row, "ProfessionalCarrier"),
            PolicyNumber = S(row, "PolicyNumber"),
            StatusOfClaim = S(row, "Claim_Status_Name"),
            ClaimSettledDate = D(row, "CLAIM_SETTLED_DATE"),
            MethodOfResolution = S(row, "Resolution_Desc"),
            SettlementAmount = M(row, "SETTLEMENT_AMOUNT"),
            RoleInCase = S(row, "INVOLVEMENT"),
            AllegedInjury = S(row, "AllegedInjury")
        };
    }
    public static class UploadedDocMapper
    {
        private static bool Has(DataRow r, string col) =>
            r?.Table?.Columns?.Contains(col) == true;

        private static string S(DataRow r, string col) =>
            Has(r, col) && r[col] != DBNull.Value ? Convert.ToString(r[col]) ?? "" : "";

        private static DateTime? D(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return DateTime.TryParse(s, out var dt) ? dt : (DateTime?)null;
        }

        private static int? I(DataRow r, string col)
        {
            if (!Has(r, col) || r[col] == DBNull.Value) return null;
            var s = Convert.ToString(r[col]);
            return int.TryParse(s, out var i) ? i : (int?)null;
        }

        public static UploadedDocDto ToDto(DataRow row) => new UploadedDocDto
        {
            DocumentId = I(row, "DOCUMENT_ID"),
            Name = S(row, "NAME"),
            FileName = S(row, "FILE_NAME"),
            LastModifiedDateTime = D(row, "LAST_MODIFIED_DATE_TIME"),
            Username = S(row, "Username")
        };
    }
    public static class ProviderSearchResponseMapper
    {
        public static IList<ProviderSearchResponse> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));
            if (dataSet.Tables.Count == 0) return new List<ProviderSearchResponse>();
            return FromDataTable(dataSet.Tables[0]);
        }
        public static IList<ProviderSearchResponse> FromDataTable(DataTable table)
        {
            var results = new List<ProviderSearchResponse>();
            if (table == null) return results;

            int id = 1;
            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row, id));
                id++;
            }

            return results;
        }
        public static ProviderSearchResponse FromDataRow(DataRow row, int id)
        {
            if (row == null) throw new ArgumentNullException(nameof(row));

            return new ProviderSearchResponse
            {
                ID = id,
                RegID = GetString(row, "RegID"),
                CredId = GetString(row, "CredId"),
                OrganizationName = GetString(row, "OrganizationName"),
                NPI = GetString(row, "NPI"),
                AssignedTo = GetString(row, "AssignedTo"),
                BaseMedicaidID = GetString(row, "BaseMedicaidID"),
                SpecialtyTypeName = GetString(row, "SPECIALTY_TYPE_NAME"),
                ProviderTypeName = GetString(row, "PROVIDER_TYPE_NAME"),
                TaxId = GetString(row, "TaxId"),
                CurrentStepID = GetString(row, "CurrentStepID")
            };
        }



        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }
        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName)) return null;
            var val = row[columnName];
            return val == DBNull.Value ? null : Convert.ToString(val)?.Trim();
        }
    }
    public static class CvoQueueModelMapper
    {
        public static IList<CvoQueueModel> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<CvoQueueModel>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<CvoQueueModel> FromDataTable(DataTable table)
        {
            var results = new List<CvoQueueModel>();
            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static CvoQueueModel FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new CvoQueueModel
            {
                RegistrationId = GetInt(row, "REG_ID"),
                CredentialingId = GetInt(row, "CREDENTIALING_ID"),
                ProcessId = GetInt(row, "PROCESS_ID"),
                StepId = GetInt(row, "STEP_ID"),
                TaskId = GetInt(row, "TASK_ID"),
                WorkflowId = GetInt(row, "WORKFLOW_ID"),
                TaskName = GetString(row, "TASK_NAME"),
                WorkflowName = GetString(row, "WORKFLOW_NAME"),
                ProviderName = GetString(row, "PROVIDER_NAME"),
                Npi = GetString(row, "NPI"),
                ProviderType = GetString(row, "PROVIDER_TYPE_NAME"),
                ProviderRiskLevel = GetString(row, "PROVIDER_RISK_LEVEL_NAME"),
                AgingDays = GetInt(row, "AGING"),
                PageClassName = GetString(row, "CLASS_NAME"),
                SpecialtyTypeName = GetString(row, "SpecialtyTypeName")
            };
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName)) return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static int GetInt(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName)) return 0;

            var val = row[columnName];
            return val == DBNull.Value
                ? 0
                : Convert.ToInt32(val);
        }
    }
    public static class CommitteeQueueModelMapper
    {
        public static IList<CommitteeQueue> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<CommitteeQueue>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<CommitteeQueue> FromDataTable(DataTable table)
        {
            var results = new List<CommitteeQueue>();
            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static CommitteeQueue FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new CommitteeQueue
            {

                RegistrationId = GetInt(row, "REG_ID"),
                CredentialingId = GetInt(row, "credentialing_id"),
                TaskId = GetInt(row, "TASK_ID"),
                WorkflowId = GetInt(row, "WORKFLOW_ID"),
                ProcessId = GetInt(row, "PROCESS_ID"),
                CredentialRiskLevelName = GetString(row, "CredentialRiskLevelName"),
                ProviderName = GetString(row, "PROVIDER_NAME"),
                TaskName = GetString(row, "TASK_NAME"),
                WorkflowName = GetString(row, "WORKFLOW_NAME"),
                Npi = GetString(row, "NPI"),
                ProviderType = GetString(row, "PROVIDER_TYPE_NAME"),
                ProviderRiskLevel = GetString(row, "PROVIDER_RISK_LEVEL_NAME"),
                AgingDays = GetInt(row, "AGING"),
                PageClassName = GetString(row, "CLASS_NAME"),
                credentialing_id = GetInt(row, "credentialing_id"),
                SPECIALTY_TYPE_NAME = GetString(row, "SPECIALTY_TYPE_NAME")
            };
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static int GetInt(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return 0;

            var val = row[columnName];
            return val == DBNull.Value
                ? 0
                : Convert.ToInt32(val);
        }
    }
    public static class CorrespondenceMapper
    {
        public static List<CorrespondenceResponse> MapFromDataSet(DataSet dataSet)
        {
            if (dataSet == null || dataSet.Tables.Count == 0)
                return new List<CorrespondenceResponse>();

            return MapFromDataTable(dataSet.Tables[0]);
        }

        public static List<CorrespondenceResponse> MapFromDataTable(DataTable table)
        {
            var result = new List<CorrespondenceResponse>();

            if (table == null)
                return result;

            foreach (DataRow row in table.Rows)
            {
                result.Add(MapFromDataRow(row));
            }

            return result;
        }

        public static CorrespondenceResponse MapFromDataRow(DataRow row)
        {
            return new CorrespondenceResponse
            {
                CommunicationEventId = GetLong(row, "COMMUNICATION_EVENT_ID"),
                RecipientId = GetLong(row, "RECIPIENT_ID"),

                Subject = GetString(row, "SUBJECT"),
                CorrespondenceType = GetString(row, "CORRESPODENCE_TYPE"),

                PaNumber = GetString(row, "PA_NUMBER"),
                HtnNumber = GetString(row, "HTN_NUMBER"),

                DateSent = GetNullableDate(row, "DATE_SENT"),
                DateViewed = GetNullableDate(row, "DATE_VIEWED"),

                Body = GetString(row, "BODY"),
                EmailTo = GetString(row, "EMAIL_TO"),

                DocumentId = GetNullableLong(row, "DOCUMENT_ID"),
                DocumentIdPdf = GetNullableLong(row, "DOCUMENT_ID_PDF"),
                OnbaseDocumentId = GetNullableLong(row, "ONBASE_DOCUMENT_ID"),
                DocumentAttachmentXrefId = GetNullableLong(row, "DOCUMENT_ATTACHMENT_XREF_ID"),

                DocumentType = GetString(row, "DOCUMENT_TYPE"),
                FileName = GetString(row, "FILE_NAME"),
                FileNamePdf = GetString(row, "FILE_NAME_PDF"),

                Eligibility = GetString(row, "ELIGIBILITY")
            };
        }

        #region Helper Methods



        private static long GetLong(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return 0;

            var value = row[columnName];

            if (value == DBNull.Value)
                return 0;

            // Handle empty or whitespace strings
            if (value is string s && string.IsNullOrWhiteSpace(s))
                return 0;

            return long.TryParse(value.ToString(), out var result)
                ? result
                : 0;
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return null;

            var value = row[columnName];

            if (value == DBNull.Value)
                return null;

            var result = value.ToString();
            return string.IsNullOrWhiteSpace(result) ? null : result;
        }

        private static long? GetNullableLong(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return null;

            var value = row[columnName];

            if (value == DBNull.Value)
                return null;

            // Handle numeric types directly
            if (value is long l) return l;
            if (value is int i) return i;
            if (value is short s) return s;
            if (value is decimal d) return (long)d;

            // Handle string / mixed types safely
            return long.TryParse(value.ToString(), out var result)
                ? result
                : null;
        }

        private static DateTime? GetNullableDate(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName))
                return null;

            var value = row[columnName];

            if (value == DBNull.Value)
                return null;

            // Already a DateTime
            if (value is DateTime dt)
                return dt;

            // Safe string parsing
            return DateTime.TryParse(value.ToString(), out var result)
                ? result
                : null;
        }

        #endregion
    }
    public static class ProviderCredentialActivityMapper
    {
        public static IList<ProviderCredentialActivity> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<ProviderCredentialActivity>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<ProviderCredentialActivity> FromDataTable(DataTable table)
        {
            var results = new List<ProviderCredentialActivity>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static ProviderCredentialActivity FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new ProviderCredentialActivity
            {
                CredentialActivityId = GetInt(row, "CREDENTIAL_ACTIVITY_ID"),
                ActivityTypeId = GetInt(row, "ACTIVITY_TYPE_ID"),
                DataRankTypeId = GetInt(row, "DATARANK_TYPE_ID"),
                ScreeningActivityTypeName = GetString(row, "SCREENING_ACTIVITY_TYPE_NAME"),
                DataRankName = GetString(row, "DATARANKNAME"),
                OriginalEffectiveDate = GetNullableDate(row, "ORIGINAL_EFFECTIVE_DATE"),
                RenewalDate = GetNullableDate(row, "RENEWAL_DATE"),
                ExpirationDate = GetNullableDate(row, "EXPIRATION_DATE"),
                VerificationSourceDisplayName = GetString(row, "VERIFICATION_SOURCE_DISPLAYNAME"),
                LastActionDate = GetNullableDate(row, "LAST_ACTION_DATE"),
                VerifiedBy = GetString(row, "VERIFIED_BY"),
                Notes = GetString(row, "NOTES")
            };
        }

        #region Helpers

        private static bool HasColumn(DataRow row, string columnName) =>
            row.Table != null && row.Table.Columns.Contains(columnName);

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value ? null : Convert.ToString(val)?.Trim();
        }

        private static int GetInt(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return 0;

            var val = row[columnName];
            return val == DBNull.Value ? 0 : Convert.ToInt32(val);
        }

        private static DateTime? GetNullableDate(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(val);
        }

        #endregion
    }
    public static class CredentialActivityHistoryMapper
    {
        public static IList<CredentialActivityHistory> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<CredentialActivityHistory>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<CredentialActivityHistory> FromDataTable(DataTable table)
        {
            var results = new List<CredentialActivityHistory>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static CredentialActivityHistory FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new CredentialActivityHistory
            {
                CredentialingId = GetInt(row, "credentialing_id"),
                CredentialingType = GetString(row, "NAME"),
                CredentialingStartDate = GetNullableDate(row, "START_DATE_TIME"),
                CredentialingEndDate = GetNullableDate(row, "END_DATE_TIME"),
                CredentialingStatusName = GetString(row, "CREDENTIALING_STATUS_NAME"),
                CredentialingResultName = GetString(row, "CREDENTIALING_RESULT_NAME"),
                CredentialingRiskLevel = GetString(row, "RiskLevelName")
            };
        }

        #region Helper Methods

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null &&
                   row.Table.Columns.Contains(columnName);
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var value = row[columnName];
            return value == DBNull.Value
                ? null
                : Convert.ToString(value)?.Trim();
        }

        private static int GetInt(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return 0;

            var value = row[columnName];
            return value == DBNull.Value
                ? 0
                : Convert.ToInt32(value);
        }

        private static DateTime? GetNullableDate(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var value = row[columnName];
            return value == DBNull.Value
                ? (DateTime?)null
                : Convert.ToDateTime(value);
        }

        #endregion
    }
    public static class CredentialActivityDetailsMapper
    {
        public static CredentialActivityDetails FromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new CredentialActivityDetails
            {
                CredentialActivityId = GetInt(row, "CREDENTIAL_ACTIVITY_ID"),
                CredentialingId = GetInt(row, "CREDENTIALING_ID"),
                ActivityTypeId = GetInt(row, "ACTIVITY_TYPE_ID"),

                Notes = GetString(row, "NOTES"),

                OriginalEffectiveDate = GetDate(row, "ORIGINAL_EFFECTIVE_DATE"),
                RenewalDate = GetDate(row, "RENEWAL_DATE"),
                ExpirationDate = GetDate(row, "EXPIRATION_DATE"),

                VerificationSourceUsed = GetString(row, "VERIFICATION_SOURCE_USED"),
                VerificationDate = GetDate(row, "VERIFICATION_DATE"),
                LastActionDate = GetDate(row, "LAST_ACTION_DATE"),
                AttestationDate = GetDate(row, "ATTESTATION_DATE"),

                IsBoardVerificationRequired = GetBool(row, "isBoardVerificationRequired"),

                MaternityLicenseDate = GetDate(row, "MATERNITY_LICENSE_DATE"),
                SiteVisitDate = GetDate(row, "SITE_VISIT_DATE"),

                ScreeningActivityTypeName = GetString(row, "SCREENING_ACTIVITY_TYPE_NAME"),

                ExternalCheckUrl = GetString(row, "EXTERNAL_CHECK_URL"),
                ExternalCheckUrlDescription = GetString(row, "EXTERNAL_CHECK_URL_DESCRIPTION"),

                ActivityDataRankId = GetInt(row, "ACTIVITY_DATARANK_ID"),
                DataRankDisplayName = GetString(row, "display_NAME"),

                VerifiedBy = GetString(row, "VERIFIED_BY"),

                ExtraExternalCheckUrl = GetString(row, "EXTRA_EXTERNAL_CHECK_URL"),
                ExtraExternalCheckUrlDescription = GetString(row, "EXTRA_EXTERNAL_CHECK_URL_DESCRIPTION")
            };
        }

        #region Helpers

        private static bool HasColumn(DataRow row, string col) =>
            row.Table.Columns.Contains(col);

        private static string GetString(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToString(row[col]).Trim()
                : null;

        private static int GetInt(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToInt32(row[col])
                : 0;

        private static DateTime? GetDate(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToDateTime(row[col])
                : (DateTime?)null;

        private static bool GetBool(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value &&
            Convert.ToBoolean(row[col]);

        #endregion
    }
    public static class ProviderRegistrationSummaryMapper
    {
        public static ProviderRegistrationSummary FromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderRegistrationSummary
            {
                OrganizationName = GetString(row, "ORGANIZATION_NAME"),
                Npi = GetString(row, "NPI"),
                Gender = GetString(row, "GENDER"),
                TaxId = GetString(row, "TAX_ID"),
                BirthDate = GetDate(row, "BIRTH_DATE"),
                IndividualName = GetString(row, "INDIVIDUAL_NAME"),

                PecosRiskLevelId = GetInt(row, "PECOS_RISK_LEVEL_ID"),
                PecosLastRevalidationDate = GetDate(row, "PECOS_LAST_REVALIDATION_DATE"),
                PecosEnrolledState = GetString(row, "PECOS_ENROLLED_STATE"),

                CitizenshipTypeName = GetString(row, "CITIZENSHIP_TYPE_NAME"),
                ImmigrationStatusName = GetString(row, "IMMIGRATION_STATUS_NAME"),

                AlienNumber = GetString(row, "ALIEN_NUMBER"),

                ProviderTypeId = GetInt(row, "PROVIDER_TYPE_ID"),
                ProviderTypeName = GetString(row, "PROVIDER_TYPE_NAME"),

                EntityType = GetInt(row, "ENTITYTYPE"),

                FirstName = GetString(row, "FIRSTNAME"),
                MiddleName = GetString(row, "MIDDLENAME"),
                LastName = GetString(row, "LASTNAME"),

                MailingAddressStateName = GetString(row, "MAILINGADDRESSSTATENAME")
            };
        }

        #region Helpers (same as above)
        private static bool HasColumn(DataRow row, string col) =>
            row.Table.Columns.Contains(col);

        private static string GetString(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToString(row[col]).Trim()
                : null;

        private static int GetInt(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToInt32(row[col])
                : 0;

        private static DateTime? GetDate(DataRow row, string col) =>
            HasColumn(row, col) && row[col] != DBNull.Value
                ? Convert.ToDateTime(row[col])
                : (DateTime?)null;
        #endregion
    }
    public static class ProviderMalpracticeInsuranceMapper
    {
        public static List<ProviderMalpracticeInsurance> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderMalpracticeInsurance>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                result.Add(MapFromDataRow(row));
            }

            return result;
        }

        public static ProviderMalpracticeInsurance MapFromDataRow(DataRow row)
        {
            return new ProviderMalpracticeInsurance
            {
                CarrierName = row["CarrierName"]?.ToString(),

                PolicyHolder = row["PolicyHolder"]?.ToString(),

                PolicyNumber = row["POLICY_NUMBER"]?.ToString(),

                EffectiveDate = row["EFFECTIVE_DATE"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(row["EFFECTIVE_DATE"]),

                ExpirationDate = row["EXPIRATION_DATE"] == DBNull.Value
                                    ? null
                                    : Convert.ToDateTime(row["EXPIRATION_DATE"]),

                CoverageAmountPerOccurance = row["CoverageAmountPerOccurance"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(row["CoverageAmountPerOccurance"]),

                CoverageAmountPerAggregate = row["CoverageAmountPerAggregate"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(row["CoverageAmountPerAggregate"])
            };
        }
    }
    public static class ProviderDEAMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderDEA objects.
        /// </summary>
        public static List<ProviderDEA> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderDEA>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                result.Add(MapFromDataRow(row));
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to a ProviderDEA object.
        /// </summary>
        public static ProviderDEA MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderDEA
            {
                DEANumber = row["DEA_NUMBER"]?.ToString(),

                DEAState = row["DEA_STATE"]?.ToString(),

                EffectiveDate = row["DEA_EFF_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["DEA_EFF_DATE"]),

                ExpirationDate = row["DEA_END_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["DEA_END_DATE"])
            };
        }
    }
    public static class ProviderLicensesMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderLicenses objects.
        /// </summary>
        public static List<ProviderLicenses> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderLicenses>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var license = MapFromDataRow(row);
                if (license != null)
                {
                    result.Add(license);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a DataRow to a ProviderLicenses object.
        /// </summary>
        public static ProviderLicenses MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderLicenses
            {
                RegLicensureId = row["REG_LICENSURE_ID"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(row["REG_LICENSURE_ID"]),

                LicenseRestrictionCodeId = row["LICENSE_RESTRICTION_CODE_ID"] == DBNull.Value
                    ? null
                    : Convert.ToInt32(row["LICENSE_RESTRICTION_CODE_ID"]),

                LicenseNumber = row["LICENSE_NUMBER"]?.ToString(),

                LicenseState = row["LICENSE_STATE"]?.ToString(),

                LicenseTypeName = row["LICENSE_TYPE_NAME"]?.ToString(),

                LicenseStatus = row["LICENSE_STATUS"]?.ToString(),

                EffectiveDate = row["LICENSE_EFF_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["LICENSE_EFF_DATE"]),

                ExpirationDate = row["LICENSE_END_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["LICENSE_END_DATE"])
            };
        }
    }
    public static class SpecialtyMapper
    {

        /// <summary>
        /// Maps a DataRow to a Specialty object.
        /// </summary>
        public static SpecialtyDto MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new SpecialtyDto
            {
                REG_SPECIALTY_ID = Convert.ToInt32(row["REG_SPECIALTY_ID"]),

                SpecialtyName = row["SPECIALTY_TYPE_NAME"].ToString(),

                IsPrimary = row["IsPrimary"].ToString(),

                StartDate = row["START_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["START_DATE"]),

                EndDate = row["END_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["END_DATE"])
            };
        }

    }

    public static class TaxonomyMapper
    {

        /// <summary>
        /// Maps a DataRow to a Specialty object.
        /// </summary>
        public static TaxonomyDto MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new TaxonomyDto
            {

                REG_TAXONOMY_ID = Convert.ToInt32(row["REG_TAXONOMY_ID"]),
                TaxonomyCode = row["TAXONOMY_CODE"].ToString(),
                TaxonomyName = row["TAXONOMY_NAME"].ToString(),

                StartDate = row["START_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["START_DATE"]),

                EndDate = row["END_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["END_DATE"]),
                ExpirationDate = row["EXPIRATION_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["EXPIRATION_DATE"])
            };
        }

    }



    public static class ProviderWorkResultMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderWorkResult objects.
        /// </summary>
        public static List<ProviderWorkResult> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderWorkResult>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var item = MapFromDataRow(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to a ProviderWorkResult object.
        /// </summary>
        public static ProviderWorkResult MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderWorkResult
            {
                RecordId = row["RECORD_ID"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(row["RECORD_ID"]),

                RegId = row["REG_ID"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(row["REG_ID"]),

                RecordType = row["RECORD_TYPE"]?.ToString(),

                EmployerName = row["EMPLOYER_NAME"]?.ToString(),

                StartDate = row["START_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["START_DATE"]),

                EndDate = row["END_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["END_DATE"]),

                FullAddress = row["FULL_ADDRESS"]?.ToString(),

                ContactPhoneNumber = row["CONTACT_PHONE_NUMBER"]?.ToString(),

                ReasonTextTitle = row["REASON_TEXT_TITLE"]?.ToString(),

                ReasonText = row["REASON_TEXT"]?.ToString(),

                MilitaryReserve = row["MILTARY_RESERVE"] == DBNull.Value
                    ? null
                    : Convert.ToBoolean(row["MILTARY_RESERVE"])
            };
        }
    }
    public static class ProviderSpecialtyMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderSpecialty objects.
        /// </summary>
        public static List<ProviderSpecialty> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderSpecialty>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var specialty = MapFromDataRow(row);
                if (specialty != null)
                {
                    result.Add(specialty);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to a ProviderSpecialty object.
        /// </summary>
        public static ProviderSpecialty MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderSpecialty
            {
                BoardCertificationName = row["BOARD_CERTIFICATION_NAME"]?.ToString(),

                BoardSpecialtyName = row["BOARD_SPECIALTY_NAME"]?.ToString(),

                EffectiveDate = row["EFFECTIVE_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["EFFECTIVE_DATE"]),

                ExpirationDate = row["EXPIRATION_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["EXPIRATION_DATE"])
            };
        }
    }
    public static class ProviderEducationMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderEducation objects.
        /// </summary>
        public static List<ProviderEducation> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderEducation>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var education = MapFromDataRow(row);
                if (education != null)
                {
                    result.Add(education);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to a ProviderEducation object.
        /// </summary>
        public static ProviderEducation MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderEducation
            {
                School = row["SCHOOL"]?.ToString(),

                EducationType = row["EDUCATION_TYPE"]?.ToString(),

                FieldOfStudy = row["FIELDOFSTUDY"]?.ToString(),

                StartDate = row["START_YEAR"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["START_YEAR"]),

                EndDate = row["END_YEAR"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["END_YEAR"])
            };
        }
    }
    public static class ProviderCDSMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderCDS objects.
        /// </summary>
        public static List<ProviderCDS> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderCDS>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var cds = MapFromDataRow(row);
                if (cds != null)
                {
                    result.Add(cds);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow into a ProviderCDS object.
        /// </summary>
        public static ProviderCDS MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderCDS
            {
                CDSNumber = row["STATE_CDS_NUMBER"]?.ToString(),

                State = row["State"]?.ToString(),

                DateIssued = row["DateIssued"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["DateIssued"]),

                ExpirationDate = row["ExpirationDate"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["ExpirationDate"])
            };
        }
    }
    public static class NPDBResultsMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of NPDBResults.
        /// </summary>
        public static List<NPDBResults> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<NPDBResults>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var item = MapFromDataRow(row);
                if (item != null)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to NPDBResults.
        /// </summary>
        public static NPDBResults MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new NPDBResults
            {
                Name = row["NAME"]?.ToString(),

                Gender = row["GENDER"]?.ToString(),

                DateOfBirth = row["BIRTH_DATE"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["BIRTH_DATE"]),

                TaxId = row["TAX_ID"]?.ToString(),

                AddressLine1 = row["CONTACT_ADDRESS1"]?.ToString(),

                City = row["CONTACT_CITY"]?.ToString(),

                State = row["CONTACT_STATE"]?.ToString(),

                ZipCode = row["CONTACT_ZIP"]?.ToString()
            };
        }
    }
    public static class ProviderReCredentialingDueModelMapper
    {
        public static IList<ProviderReCredentialingDue> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<ProviderReCredentialingDue>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<ProviderReCredentialingDue> FromDataTable(DataTable table)
        {
            var results = new List<ProviderReCredentialingDue>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static ProviderReCredentialingDue FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new ProviderReCredentialingDue
            {
                NPI = GetString(row, "Provider NPI"),
                ProviderName = GetString(row, "Provider Name"),
                ProviderSpecialty = GetString(row, "Provider Specialty"),
                ReCredentialingDueDate = GetNullableDateTime(row, "Re-Cred Due Date"),
                Status = GetString(row, "Status")
            };
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return null;

            return Convert.ToDateTime(row[columnName]);
        }
    }
    public static class ProviderCredentialingApprovedModelMapper
    {
        public static IList<ProviderCredentialingApproved> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<ProviderCredentialingApproved>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<ProviderCredentialingApproved> FromDataTable(DataTable table)
        {
            var results = new List<ProviderCredentialingApproved>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static ProviderCredentialingApproved FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new ProviderCredentialingApproved
            {
                CredentialingTaskType = GetString(row, "Credentialing Task Type "),
                StartDate = GetNullableDateTime(row, "Start Date"),
                ProviderName = GetString(row, "Provider Name"),
                ProviderType = GetString(row, "Provider Type"),
                ProviderSpecialty = GetString(row, "Provider Specialty"),
                MedicaidId = GetString(row, "Medicaid Id"),
                NPI = GetString(row, "Provider NPI"),
                PrimaryCity = GetString(row, "Primary City"),
                PrimaryState = GetString(row, "Primary State"),
                PrimaryCounty = GetString(row, "Primary County"),
                ApprovedDate = GetNullableDateTime(row, "Approved Date"),
                ApprovedBy = GetString(row, "Approved By")
            };
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return null;

            return Convert.ToDateTime(row[columnName]);
        }
    }
    public static class ProviderCredentialingCommitteeDecisionModelMapper
    {
        public static IList<ProviderCredentialingCommitteeDecision> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<ProviderCredentialingCommitteeDecision>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<ProviderCredentialingCommitteeDecision> FromDataTable(DataTable table)
        {
            var results = new List<ProviderCredentialingCommitteeDecision>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static ProviderCredentialingCommitteeDecision FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new ProviderCredentialingCommitteeDecision
            {
                CredentialingTaskType = GetString(row, "Credentialing Task Type "),
                StartDate = GetNullableDateTime(row, "Start Date"),
                ProviderType = GetString(row, "Provider Type"),
                ProviderName = GetString(row, "Provider Name"),
                ProviderSpecialty = GetString(row, "Provider Specialty"),
                MedicaidId = GetString(row, "Medicaid Id"),
                NPI = GetString(row, "Provider NPI"),
                PrimaryCity = GetString(row, "Primary City"),
                PrimaryState = GetString(row, "Primary State"),
                PrimaryCounty = GetString(row, "Primary County"),
                CommitteeDecision = GetString(row, "Committee Dicision"),
                ApprovedDate = GetNullableDateTime(row, "Approved Date"),
                DecisionDate = GetNullableDateTime(row, "Dicision Date"),
                ProviderEndDate = GetNullableDateTime(row, "END_DATE")
            };
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return null;

            return Convert.ToDateTime(row[columnName]);
        }
    }
    public static class ProviderReCredentialingOverdueModelMapper
    {
        public static IList<ProviderReCredentialingOverdue> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<ProviderReCredentialingOverdue>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<ProviderReCredentialingOverdue> FromDataTable(DataTable table)
        {
            var results = new List<ProviderReCredentialingOverdue>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static ProviderReCredentialingOverdue FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new ProviderReCredentialingOverdue
            {
                ProviderName = GetString(row, "Provider Name"),
                ProviderType = GetString(row, "Provider Type"),
                NPI = GetString(row, "Provider NPI"),
                ReCredentialingDueDate = GetNullableDateTime(row, "Re-Cred Due Date"),
                MostRecentDateApproved = GetNullableDateTime(row, "Most Recent Date Approved")
            };
        }

        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return null;

            return Convert.ToDateTime(row[columnName]);
        }
    }
    public static class CredentialingFileProcessingSummaryModelMapper
    {
        public static IList<CredentialingFileProcessingSummary> FromDataSet(DataSet dataSet)
        {
            if (dataSet == null)
                throw new ArgumentNullException(nameof(dataSet));

            if (dataSet.Tables.Count == 0)
                return new List<CredentialingFileProcessingSummary>();

            return FromDataTable(dataSet.Tables[0]);
        }

        public static IList<CredentialingFileProcessingSummary> FromDataTable(DataTable table)
        {
            var results = new List<CredentialingFileProcessingSummary>();

            if (table == null)
                return results;

            foreach (DataRow row in table.Rows)
            {
                results.Add(FromDataRow(row));
            }

            return results;
        }

        public static CredentialingFileProcessingSummary FromDataRow(DataRow row)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            return new CredentialingFileProcessingSummary
            {
                CSName = GetString(row, "CSName"),
                CleanFiles = GetInt(row, "Cleanfiles"),
                FlaggedFiles = GetInt(row, "FalggedFiles"),
                TotalFilesProcessed = GetInt(row, "Total Files Processed"),
                PercentCleaned = GetString(row, "percent of cleaned process"),
                PercentFlagged = GetString(row, "percent of flagged process")
            };
        }
        private static string GetString(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName))
                return null;

            var val = row[columnName];
            return val == DBNull.Value
                ? null
                : Convert.ToString(val)?.Trim();
        }

        private static bool HasColumn(DataRow row, string columnName)
        {
            return row.Table != null && row.Table.Columns.Contains(columnName);
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("Column name cannot be null or empty.", nameof(columnName));

            if (!row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
                return null;

            return Convert.ToDateTime(row[columnName]);
        }
        private static int GetInt(DataRow row, string columnName)
        {
            if (!HasColumn(row, columnName)) return 0;

            var val = row[columnName];
            return val == DBNull.Value
                ? 0
                : Convert.ToInt32(val);
        }
    }
    public static class ProviderDocumentMapper
    {
        /// <summary>
        /// Maps a DataSet to a list of ProviderDocument objects.
        /// </summary>
        public static List<ProviderDocument> MapFromDataSet(DataSet dataSet)
        {
            var result = new List<ProviderDocument>();

            if (dataSet == null ||
                dataSet.Tables.Count == 0 ||
                dataSet.Tables[0].Rows.Count == 0)
            {
                return result;
            }

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var document = MapFromDataRow(row);
                if (document != null)
                {
                    result.Add(document);
                }
            }

            return result;
        }

        /// <summary>
        /// Maps a single DataRow to ProviderDocument.
        /// </summary>
        public static ProviderDocument MapFromDataRow(DataRow row)
        {
            if (row == null)
                return null;

            return new ProviderDocument
            {
                DocumentId = row["DOCUMENT_ID"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(row["DOCUMENT_ID"]),

                Name = row["NAME"]?.ToString(),

                FileName = row["FILE_NAME"]?.ToString(),

                LastModifiedDateTime = row["LAST_MODIFIED_DATE_TIME"] == DBNull.Value
                    ? null
                    : Convert.ToDateTime(row["LAST_MODIFIED_DATE_TIME"]),

                Username = row["Username"]?.ToString(),

                RoleName = row["RoleName"]?.ToString(),

                OnBaseDocumentId = row["ONBASE_DOCUMENT_ID"]?.ToString(),

                IsConversion = row["IS_CONVERSION"] != DBNull.Value &&
                               Convert.ToBoolean(row["IS_CONVERSION"])
            };
        }
    }

    public static class MembershipUserMapper
    {
        public static Task<List<TDto>> FetchAndMapAsync<TKey, TDto>(TKey id, Func<TKey, DataSet> fetchDataSet, Func<DataRow, TDto> mapRow, Func<TKey, bool>? isValidId = null,
                      CancellationToken ct = default)
        {
            if (isValidId != null && !isValidId(id))
                throw new ArgumentOutOfRangeException(nameof(id));

            DataSet ds = fetchDataSet(id);

            DataTable? table =
                (ds?.Tables?.Count ?? 0) > 0 ? ds.Tables[0] : null;

            var list = new List<TDto>();

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    ct.ThrowIfCancellationRequested();
                    list.Add(mapRow(row));
                }
            }

            return Task.FromResult(list);
        }


        public static MembershipUserDto ToDto(DataRow row) =>
            new MembershipUserDto
            {
                UserName = row["UserName"].ToString()!,
                Email = row["Email"].ToString()!,
                IsApproved = (bool)row["IsApproved"],
                IsLockedOut = (bool)row["IsLockedOut"],
                CreateDate = (DateTime)row["CreateDate"],
                LastLoginDate = (DateTime)row["LastLoginDate"],
                LastActivityDate = (DateTime)row["LastActivityDate"]
            };
    }

    public static class UserRoleMapper
    {
        public static List<UserRoleDto> Map(DataSet userData)
        {
            var roles = new List<UserRoleDto>();

            if (userData == null ||
                userData.Tables.Count == 0 ||
                userData.Tables[0].Rows.Count == 0)
            {
                return roles;
            }

            DataTable table = userData.Tables[0];

            foreach (DataRow row in table.Rows)
            {
                roles.Add(new UserRoleDto
                {
                    RoleId = row.Table.Columns.Contains("RoleId") && row["RoleId"] != DBNull.Value
                        ? Convert.ToString(row["RoleId"])
                        : "",

                    RoleName = row.Table.Columns.Contains("RoleName") && row["RoleName"] != DBNull.Value
                        ? row["RoleName"].ToString()
                        : string.Empty,
                });
            }

            return roles;
        }
    }
}
