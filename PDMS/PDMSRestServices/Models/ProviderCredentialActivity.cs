namespace PDMSRestServices.Models
{
    public class ProviderCredentialActivity
    {
        public int CredentialActivityId { get; set; }
        public int ActivityTypeId { get; set; }
        public int DataRankTypeId { get; set; }
        public string ScreeningActivityTypeName { get; set; }
        public string DataRankName { get; set; }
        public DateTime? OriginalEffectiveDate { get; set; }
        public DateTime? RenewalDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string VerificationSourceDisplayName { get; set; }
        public DateTime? LastActionDate { get; set; }
        public string VerifiedBy { get; set; }
        public string Notes { get; set; }
    }
    public class CredentialActivityHistory
    {
        public int CredentialingId { get; set; }

        public string CredentialingType { get; set; }          // NAME

        public DateTime? CredentialingStartDate { get; set; }  // START_DATE_TIME

        public DateTime? CredentialingEndDate { get; set; }    // END_DATE_TIME

        public string CredentialingStatusName { get; set; }    // CREDENTIALING_STATUS_NAME

        public string CredentialingResultName { get; set; }    // CREDENTIALING_RESULT_NAME

        public string CredentialingRiskLevel { get; set; }     // RiskLevelName
    }
}
