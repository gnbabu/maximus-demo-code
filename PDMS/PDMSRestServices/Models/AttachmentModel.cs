namespace PDMSRestServices.Models
{
    public class AttachmentModel
    {
        public int OutBound_Document_Uploads_ID { get; set; }         // Used for Delete button
        public string DocumentName { get; set; }                      // Used for download icon
        public string Claim_number { get; set; }                      // ICN
        public string PA_NUMBER { get; set; }                         // PA NUMBER
        public string Member_ID { get; set; }                         // Recipient ID
        public string DOCUMENT_TYPE_DESC { get; set; }                // Document Type
        public string OUTBOUND_DOCUMENT_UPLOAD_IDENTIFIER { get; set; } // Document ID
    }
}
