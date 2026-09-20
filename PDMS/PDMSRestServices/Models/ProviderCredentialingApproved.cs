namespace PDMSRestServices.Models
{
    public class ProviderCredentialingApproved
    {
        public string CredentialingTaskType { get; set; }
        public DateTime? StartDate { get; set; }
        public string ProviderName { get; set; }
        public string ProviderType { get; set; }
        public string ProviderSpecialty { get; set; }
        public string MedicaidId { get; set; }
        public string NPI { get; set; }
        public string PrimaryCity { get; set; }
        public string PrimaryState { get; set; }
        public string PrimaryCounty { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
    }
}
