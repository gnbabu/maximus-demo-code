namespace PDMSRestServices.Models
{

    public class TransactionDTO

    {

        public string TransactionKey { get; set; }

        public string RequestType { get; set; }

        public DateTime? RequestTimestamp { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public int? DurationMs { get; set; }

    }
}

