namespace PDMSRestServices.Models
{
    public class ProviderReCredentialingOverdue
    {
        public string ProviderName { get; set; }

        public string ProviderType { get; set; }

        public string NPI { get; set; }

        public DateTime? ReCredentialingDueDate { get; set; }

        public DateTime? MostRecentDateApproved { get; set; }
    }
}
