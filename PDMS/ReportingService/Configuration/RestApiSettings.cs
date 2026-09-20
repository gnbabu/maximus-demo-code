namespace ReportingService.Services.Configuration
{
    /// <summary>
    /// Rest API settings
    /// These Values gets pulled from the Database Settings Table
    /// </summary>
    public class RestApiSettings : ISettings
    {
        public string BaseUrl { get; set; }
    }
}