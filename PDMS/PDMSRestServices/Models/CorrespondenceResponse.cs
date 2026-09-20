namespace PDMSRestServices.Models
{
    public class CorrespondenceResponse
    {
        public long CommunicationEventId { get; set; }
        public long RecipientId { get; set; }
        public string Subject { get; set; }
        public string CorrespondenceType { get; set; }
        public string PaNumber { get; set; }
        public string HtnNumber { get; set; }
        public DateTime? DateSent { get; set; }
        public DateTime? DateViewed { get; set; }
        public string Body { get; set; }
        public string EmailTo { get; set; }
        public long? DocumentId { get; set; }
        public long? DocumentIdPdf { get; set; }
        public long? OnbaseDocumentId { get; set; }
        public long? DocumentAttachmentXrefId { get; set; }
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FileNamePdf { get; set; }
        public string Eligibility { get; set; }
    }

    public class CorrespondenceRequest
    {
        // Filters
        public string? CorrespondenceType { get; set; }
        public string? UserId { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string? RegId { get; set; }
        public string? Npi { get; set; }
        public string? MedicaidId { get; set; }

        // Sorting
        public string? SortField { get; set; }
        public string? SortDirection { get; set; }

        // Paging
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }

    public class UpdateViewedRequest
    {
        public string DocumentId { get; set; }
    }
}
