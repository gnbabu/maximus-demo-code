namespace PDMSRestServices.Models
{
    public class CredentialActivityDetailsResponse
    {
        public CredentialActivityDetails Activity { get; set; }
        public ProviderRegistrationSummary Provider { get; set; }
    }
    public class CredentialActivityDetails
    {
        public int CredentialActivityId { get; set; }
        public int CredentialingId { get; set; }
        public int ActivityTypeId { get; set; }

        public string Notes { get; set; }

        public DateTime? OriginalEffectiveDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string VerificationSourceUsed { get; set; }
        public DateTime? VerificationDate { get; set; }
        public DateTime? LastActionDate { get; set; }
        public DateTime? AttestationDate { get; set; }

        public bool IsBoardVerificationRequired { get; set; }

        public DateTime? MaternityLicenseDate { get; set; }
        public DateTime? SiteVisitDate { get; set; }

        public string ScreeningActivityTypeName { get; set; }

        public string ExternalCheckUrl { get; set; }
        public string ExternalCheckUrlDescription { get; set; }

        public int ActivityDataRankId { get; set; }
        public string DataRankDisplayName { get; set; }

        public string VerifiedBy { get; set; }

        public string ExtraExternalCheckUrl { get; set; }
        public string ExtraExternalCheckUrlDescription { get; set; }
    }
    public class ProviderRegistrationSummary
    {
        public string OrganizationName { get; set; }
        public string Npi { get; set; }
        public string Gender { get; set; }
        public string TaxId { get; set; }
        public DateTime? BirthDate { get; set; }

        public string IndividualName { get; set; }

        public int PecosRiskLevelId { get; set; }
        public DateTime? PecosLastRevalidationDate { get; set; }
        public string PecosEnrolledState { get; set; }

        public string CitizenshipTypeName { get; set; }
        public string ImmigrationStatusName { get; set; }

        public string AlienNumber { get; set; }

        public int ProviderTypeId { get; set; }
        public string ProviderTypeName { get; set; }

        public int EntityType { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string MailingAddressStateName { get; set; }
    }
    public class ProviderMalpracticeInsurance
    {
        public string CarrierName { get; set; }

        public string PolicyHolder { get; set; }

        public string PolicyNumber { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public decimal? CoverageAmountPerOccurance { get; set; }

        public decimal? CoverageAmountPerAggregate { get; set; }
    }
    public class ProviderDEA
    {
        public string DEANumber { get; set; }

        public string DEAState { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
    public class ProviderLicenses
    {
        public int RegLicensureId { get; set; }

        public int? LicenseRestrictionCodeId { get; set; }

        public string LicenseNumber { get; set; }

        public string LicenseState { get; set; }

        public string LicenseTypeName { get; set; }

        public string LicenseStatus { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }


    public class ProviderWorkResult
    {
        public int RecordId { get; set; }

        public int RegId { get; set; }

        public string RecordType { get; set; }

        public string EmployerName { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string FullAddress { get; set; }

        public string ContactPhoneNumber { get; set; }

        public string ReasonTextTitle { get; set; }

        public string ReasonText { get; set; }

        public bool? MilitaryReserve { get; set; }
    }

    public class ProviderSpecialty
    {
        public string BoardCertificationName { get; set; }

        public string BoardSpecialtyName { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
    public class ProviderEducation
    {
        public string School { get; set; }

        public string EducationType { get; set; }

        public string FieldOfStudy { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
    public class ProviderCDS
    {
        public string CDSNumber { get; set; }

        public string State { get; set; }

        public DateTime? DateIssued { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
    public class NPDBResults
    {
        public string Name { get; set; }

        public string Gender { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string TaxId { get; set; }

        public string AddressLine1 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        /// <summary>
        /// Convenience property for UI / API display.
        /// </summary>
        public string FullAddress =>
            $"{AddressLine1}, {City}, {State}, {ZipCode}";
    }
    public class ProviderDocument
    {
        public int DocumentId { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        public string Username { get; set; }

        public string RoleName { get; set; }

        public string OnBaseDocumentId { get; set; }

        public bool IsConversion { get; set; }
    }
    public class ProviderDocumentRequest
    {
        public int RegId { get; set; }
        public int RegPageTypeId { get; set; }
        public string DocSectionName { get; set; }
        public string Exclusions { get; set; }
        public bool ShowEducationWorkDocs { get; set; }
        public string UserRole { get; set; }
        public int RegistrationStep { get; set; }
    }


    public class DeleteRequest
    {
        public int DocumentId { get; set; }
        public string UserId { get; set; }
    }
}




