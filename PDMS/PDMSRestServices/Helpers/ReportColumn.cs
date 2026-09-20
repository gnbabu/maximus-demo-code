namespace PDMSRestServices.Helpers
{
    public class ReportColumn<T>
    {
        public string Header { get; set; }
        public Func<T, object> Value { get; set; }
    }
}
