using System;

namespace PDMSRestServices.Models
{
    public class ProviderApprovalRequest
    {
        public int RegId { get; set; }
        public int CredentialingId { get; set; }
        public int ProcessId { get; set; }
        public string UserId { get; set; }
        public bool IsUserInPSRoles { get; set; }
        public bool IsUserInCredentialingRole { get; set; }
    }

    public class ApproveProvidersResponse
    {
        public int TotalRequested { get; set; }
        public int TotalSucceeded { get; set; }
        public List<ApproveProviderResult> Results { get; set; }
    }

    public class ApproveProviderResult
    {
        public int RegId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
