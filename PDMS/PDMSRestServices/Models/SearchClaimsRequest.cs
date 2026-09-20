namespace PDMSRestServices.Models
{

    public class SearchClaimsRequest
    {
        public bool? AllowDBSearch { get; set; }
        public bool? IsGridpaging { get; set; }
        public string PayorType { get; set; }
        public string ICN { get; set; }
        public string PatientAccountNumber { get; set; }
        public string MemberMedicaidId { get; set; }
        public string RenderingProviderID { get; set; }
        public string BillingProviderID { get; set; }
        public string PrescriptionNumber { get; set; }
        public string ClaimType { get; set; }
        public string Status { get; set; }
        public decimal? TotalCharges { get; set; }
        public bool? TotalChargesSpecified { get; set; }
        public string FromDOS { get; set; }
        public bool? FromDOSSpecified { get; set; }
        public string ThruDOS { get; set; }
        public bool? ThruDOSSpecified { get; set; }
        public string RemittanceAdviceDate { get; set; }
        public bool? RemittanceAdviceDateSpecified { get; set; }
        public string MaxRecords { get; set; }
        public string Offset { get; set; }
        public string PageSize { get; set; }

        public string UserName { get; set; }
    }
    
}
