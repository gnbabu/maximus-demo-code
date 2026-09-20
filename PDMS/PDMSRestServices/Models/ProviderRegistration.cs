namespace PDMSRestServices.Models
{
    public class ProfessionalLicenseDto
    {
        public string LicenseTypeName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string LicenseState { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    public class SatellitePracticeLocationDto
    {
        public int RegAddressId { get; set; }
        public string AdditionalPracticeName { get; set; } = string.Empty;
        public string AdditionalPracticeAddress { get; set; } = string.Empty;
        public string AdditionalPracticePhoneNumber { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class BoardCertificationDto
    {
        public string BoardCertificationName { get; set; } = string.Empty;
        public string BoardSpecialtyName { get; set; } = string.Empty;
        public string CertificationNumber { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }

    public class SpecialtyDto
    {
        public int REG_SPECIALTY_ID { get; set; }
        public string SpecialtyName { get; set; } = string.Empty;
        public string IsPrimary { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class TaxonomyDto
    {
        public int REG_TAXONOMY_ID { get; set; }
        public string TaxonomyCode { get; set; } = string.Empty;
        public string TaxonomyName { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    public class CliaCertificationDto
    {
        public string CliaNumber { get; set; } = string.Empty;
        public string CliaCertificationType { get; set; } = string.Empty;
        public string CliaCertificationName { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    public class DeaCertificateDto
    {
        public string DeaNumber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    public class CdsCertificateDto
    {
        public string CdsNumber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
    public class EducationDto
    {
        public string School { get; set; } = string.Empty;
        public string EducationType { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DegreeOrCertificate { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string EDUCATION_TYPE_DESC { get; set; } = string.Empty;
    }
    public class WorkHistoryDto
    {
        public int WorkHistoryId { get; set; }           // RECORD_ID
        public int RegId { get; set; }              // REG_ID
        public string RecordType { get; set; } = ""; // 'Employment' or 'Gap'

        public string EmployerName { get; set; } = "";   // EMPLOYER_NAME

        public string StartDate { get; set; } = "";      // START_DATE
        public string EndDate { get; set; } = "";        // END_DATE

        public string FullAddress { get; set; } = "";    // FULL_ADDRESS

        public string ContactPhoneNumber { get; set; } = ""; // CONTACT_PHONE_NUMBER

        public string ReasonTextTitle { get; set; } = ""; // e.g., "Reason for Departure" / "Reason for Gap"
        public string ReasonText { get; set; } = "";      // REASON_TEXT

        public string MilitaryReserve { get; set; } = ""; // MILTARY_RESERVE
        public string ZipCode { get; set; } = "";
    }

    public class SubmittedAgreementDto
    {
        public DateTime? AgreementSubmittedDate { get; set; }
        public DateTime? AgreementEffectiveDate { get; set; }
        public string SubmittedBy { get; set; } = string.Empty;
        public int RegId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string OnbaseDocumentId { get; set; } = string.Empty;
    }
    public class InsuranceDto
    {
        public string CarrierName { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string PolicyHolder { get; set; } = string.Empty;
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string TypeOfCoverageName { get; set; } = string.Empty;
        public decimal? CoverageAmountPerOccurance { get; set; }
        public decimal? CoverageAmountPerAggregate { get; set; }
    }
    public class MalpracticeClaimDto
    {
        public DateTime? DateOfOccurrence { get; set; }
        public DateTime? DateClaimFiled { get; set; }
        public string CarrierInvolved { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string StatusOfClaim { get; set; } = string.Empty;
        public DateTime? ClaimSettledDate { get; set; }
        public string MethodOfResolution { get; set; } = string.Empty;
        public decimal? SettlementAmount { get; set; }
        public string RoleInCase { get; set; } = string.Empty;
        public string AllegedInjury { get; set; } = string.Empty;
    }
    public class UploadedDocDto
    {
        public int? DocumentId { get; set; }             
        public string Name { get; set; } = string.Empty; 
        public string FileName { get; set; } = string.Empty;  
        public DateTime? LastModifiedDateTime { get; set; }  
        public string Username { get; set; } = string.Empty;  
    }
}
