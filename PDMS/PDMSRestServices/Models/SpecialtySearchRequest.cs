namespace PDMSRestServices.Models
{
    public class SpecialtySearchRequest
    {
        public string ProviderNPI { get; set; }
        public string ProviderMedID { get; set; }
    }
    public class SpecialtySearchResult
    {
        public string MedicaidID { get; set; }
        public string ProviderType { get; set; }
        public string SpecialtyType { get; set; }
        public string PrimaryFlag { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string EnrollStatusDesc { get; set; }
    }

    public class SpecialtySearchResponse
    {
        public List<SpecialtySearchResult> SpecialtySearchResult { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescp { get; set; }
    }
}
