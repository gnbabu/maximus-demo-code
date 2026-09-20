namespace PDMSRestServices.Models
{

    public class Reports
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; } = string.Empty;
    }

    public class ReportsResponse
    {
        public int Count { get; set; }
        public IReadOnlyList<Reports> Data { get; set; } = Array.Empty<Reports>();
    }
    public class ReportDetails
    {
        public string CredentialingTaskType { get; set; }
        public DateTime StartDate { get; set; }
        public string ProviderType { get; set; }
        public string ProviderName { get; set; }
        public string ProviderSpecialty { get; set; }
        public string MedicaidId { get; set; }
        public DateTime ProviderNpi { get; set; }
        public string PrimaryCityState { get; set; }
        public string PrimaryCount { get; set; }
        public DateTime CommitDecision { get; set; }
    }
}
