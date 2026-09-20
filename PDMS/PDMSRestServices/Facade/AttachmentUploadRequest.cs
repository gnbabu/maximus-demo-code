namespace PDMSRestServices.Facade
{
    public class AttachmentUploadRequest
    {
        public string FileName { get; set; }

        public string ApiFileName { get; set; }
        public string TradingPartnerID { get; set; }
        public Guid UUID { get; set; } = Guid.NewGuid();
        public string PayerRequested { get; set; } = "Yes";
        public string PaNumber { get; set; }
        public string MemberId { get; set; }
        public string ProviderId { get; set; }
        public string ProviderNPI { get; set; }        
        public string ClaimNumber { get; set; }
        public string OriginalDocumentName { get; set; }
        public bool ToSend { get; set; } = false;        
        public string ReceiverId { get; set; }
        public int ClaimTypeId { get; set; }
        public int DocumentTypeId { get; set; }        
        public string ClaimType { get; set; }
        public string Timestamp { get; set; }
        public string MedicaidId { get; set; }
    }
}
