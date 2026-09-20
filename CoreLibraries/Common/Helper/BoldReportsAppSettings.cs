namespace Corp.Core.Libraries.Helper
{
    public static class BoldReportsAppSettings
    {
        public static string BoldReportsBaseUrl { get; set; } = string.Empty;

        public static string SiteIdentifier { get; set; } = "site1";

        public static string ReportServiceUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_reportServiceUrl))
                    return _reportServiceUrl;

                return BoldReportsBaseUrl + "/reporting/reportservice/api/Viewer";
            }
        }
        private static string _reportServiceUrl;

        public static string ReportServerUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(_reportServerUrl))
                    return _reportServerUrl;

                return BoldReportsBaseUrl + "/reporting/api/site/" + SiteIdentifier;
            }
        }
        private static string _reportServerUrl;

        public static string ItemsApiUrl
        {
            get
            {
                return BoldReportsBaseUrl + "/reporting/api/site/" + SiteIdentifier + "/v4.0/items";
            }
        }

        public static string TokenEndpoint
        {
            get
            {
                if (!string.IsNullOrEmpty(_tokenEndpoint))
                    return _tokenEndpoint;

                return BoldReportsBaseUrl + "/reporting/api/site/" + SiteIdentifier + "/token";
            }
        }
        private static string _tokenEndpoint;

        public static string UserName { get; set; } = string.Empty;

        public static string Password { get; set; } = string.Empty;

        public static string EmbedSecret { get; set; } = string.Empty;

        public static bool IsInitialized { get; private set; } = false;


        // ✅ Optional: one-shot initializer (cleaner usage)
        public static void Initialize(
            string baseUrl,
            string siteIdentifier,
            string reportServiceUrl,
            string reportServerUrl,
            string tokenEndpoint,
            string userName,
            string password,
            string embedSecret)
        {
            BoldReportsBaseUrl = baseUrl ?? string.Empty;
            SiteIdentifier = siteIdentifier ?? "site1";
            _reportServiceUrl = reportServiceUrl;
            _reportServerUrl = reportServerUrl;
            _tokenEndpoint = tokenEndpoint;
            UserName = userName ?? string.Empty;
            Password = password ?? string.Empty;
            EmbedSecret = embedSecret ?? string.Empty;
            IsInitialized = true;

        }
    }
}