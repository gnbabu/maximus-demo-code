namespace PDMSRestServices.Models
{
    public class CredentialingFileProcessingSummary
    {
        public string CSName { get; set; }

        public int CleanFiles { get; set; }

        public int FlaggedFiles { get; set; }

        public int TotalFilesProcessed { get; set; }

        public string PercentCleaned { get; set; }

        public string PercentFlagged { get; set; }
    }
}
