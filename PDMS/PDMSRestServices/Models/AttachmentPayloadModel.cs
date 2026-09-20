namespace PDMSRestServices.Models
{
    public class AttachmentPayloadModel
    {
        public string MedicaidId { get; set; }
        public string EDITransactionTypeId { get; set; }
        public string? PaNumber { get; set; }        
        public string MemberId { get; set; }
        public string? ClaimTypeId { get; set; }
        public string? ClaimNumber { get; set; }
        public string ProviderId { get; set; }
        public string ReceiverId { get; set; }
        public string DocumentTypeId { get; set; }
        public string DocumentName { get; set; }
        public string OrginalDocumentName { get; set; }
        public string Provider_NPI { get; set; }        
        public string ProviderComments { get; set; }
        public string DocumentId { get; set; }
    }
}
