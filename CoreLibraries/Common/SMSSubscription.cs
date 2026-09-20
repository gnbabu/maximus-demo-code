namespace MAXIMUS.Core.Libraries
{
    public class SMSSubscription
    {
        public string list_name { get; set; }
        public Subscription[] subscriptions { get; set; }
    }
    public class Subscription
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public Mobile mobile { get; set; }
    }

    public class Mobile
    {
        public string number { get; set; }
        public string country_code { get; set; }
    }

    public class SMSSubscriptionResult
    {
        public string status_code { get; set; }
        public string request_id { get; set; }
    }
}
