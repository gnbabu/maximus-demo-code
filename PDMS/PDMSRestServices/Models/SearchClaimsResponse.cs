using System.Xml.Serialization;

namespace PDMSRestServices.Models
{

    public class ClaimHeaderResponse
    {
        public string PayorType { get; set; }
        public string ICN { get; set; }
        public string ClaimType { get; set; }
        public string ClaimStatus { get; set; }
        public string PatientAccountNumber { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public double TotalPaidAmount { get; set; }
        public DateTime ClaimPaidDate { get; set; }
        public DateTime AdjudicationDate { get; set; }
        public DateTime ClaimSubmissionDate { get; set; }
        public double TotalCharges { get; set; }
        public DateTime FromDOS { get; set; }
        public DateTime ThruDOS { get; set; }
        public string OriginalClaimICN { get; set; }
        public string RemittanceAdviceDate { get; set; }
        public string ClaimID { get; set; }
    }

    public class SearchClaimsResponse
    {
        public ResponseHeader ResponseHeader { get; set; }
        public List<ClaimHeaderResponse> ClaimHeaderResponse { get; set; }
        public int Offset { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

    }
}
