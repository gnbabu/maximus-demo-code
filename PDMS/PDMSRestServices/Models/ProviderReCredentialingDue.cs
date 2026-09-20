namespace PDMSRestServices.Models
{
    public class ProviderReCredentialingDue
    {
        public string NPI { get; set; }

        public string ProviderName { get; set; }

        public string ProviderSpecialty { get; set; }

        public DateTime? ReCredentialingDueDate { get; set; }

        public string Status { get; set; }
    }
}
