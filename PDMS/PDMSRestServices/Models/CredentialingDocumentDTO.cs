namespace PDMSRestServices.Models
{
    public class CredentialingDocumentDTO
    {
        public long DocumentId { get; set; }
        public long OnBaseDocumentId { get; set; }
        public string DocumentType { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public string Source { get; set; } = string.Empty;
        public DateTime? UploadedOn { get; set; }
        public string Status { get; set; } = string.Empty;
    }


}
