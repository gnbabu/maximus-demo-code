namespace PDMSRestServices.Models
{
    public class CvoQueueModel
    {
        // Composite Identifiers (GridView DataKeyNames)
        public int CredentialingId { get; set; }
        public int RegistrationId { get; set; }
        public int ProcessId { get; set; }
        public int StepId { get; set; }
        public int TaskId { get; set; }
        public int WorkflowId { get; set; }

        // Task and Workflow Information
        public string TaskName { get; set; }
        public string WorkflowName { get; set; }

        // Provider Information
        public string ProviderName { get; set; }
        public string Npi { get; set; }
        public string ProviderType { get; set; }
        public string ProviderRiskLevel { get; set; }

        // SLA / Aging
        public int AgingDays { get; set; }

        // UI / Navigation Metadata
        public string PageClassName { get; set; }
        public string SpecialtyTypeName { get; set; }
    }

    public class CommitteeQueue
    {
        public int CredentialingId { get; set; }
        // DataKeyNames
        public int RegistrationId { get; set; }
        public int TaskId { get; set; }
        public int WorkflowId { get; set; }
        public int ProcessId { get; set; }
        public string CredentialRiskLevelName { get; set; }
        public string ProviderName { get; set; }
        public string PageClassName { get; set; }
        public string TaskName { get; set; }
        public string WorkflowName { get; set; }
        public string Npi { get; set; }
        public string ProviderType { get; set; }
        public int AgingDays { get; set; }
        public string ProviderRiskLevel { get; set; }
        public int credentialing_id { get; set; }
        public string SPECIALTY_TYPE_NAME { get; set; }
    }

}
