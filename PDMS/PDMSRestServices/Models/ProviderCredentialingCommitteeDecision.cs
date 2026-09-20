namespace PDMSRestServices.Models
{
    public class ProviderCredentialingCommitteeDecision
    {
        public string CredentialingTaskType { get; set; }
        public DateTime? StartDate { get; set; }
        public string ProviderType { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSpecialty { get; set; }
        public string MedicaidId { get; set; }
        public string NPI { get; set; }
        public string PrimaryCity { get; set; }
        public string PrimaryState { get; set; }
        public string PrimaryCounty { get; set; }
        public string CommitteeDecision { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public DateTime? DecisionDate { get; set; }
        public DateTime? ProviderEndDate { get; set; }
    }
}
