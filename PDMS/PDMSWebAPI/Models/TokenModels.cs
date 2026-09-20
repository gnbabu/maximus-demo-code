namespace PDMSWebAPI.Models
{
    public class CheckTokenResult
    {
        public string IsValid { get; set; }
        public string Message { get; set; }
    }

    public class TestTokenResult
    {
        public string UserFound { get; set; }
        public string AuthToken { get; set; }
        public string ErrorMessage { get; set; }
    }

}