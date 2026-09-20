namespace PDMSRestServices.Models
{
    public class ORPSearchRequest
    {
        public string ProviderNPI { get; set; }
        public DateTime DOS { get; set; }
    }

    public class ORPSearchDisplay
    {
        public string ProviderNPI { get; set; }
        public string ProviderName { get; set; }
    }

    public class ORPSearchResponse
    {
        public List<ORPSearchDisplay> ORPSearchDisplay { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseDescp { get; set; }
    }
}
