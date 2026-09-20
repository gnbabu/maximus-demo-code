using ReportingService.Services.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BoldReportsSettings: ISettings
    {
        protected string[] _excludeCategories;
        protected string[] _templateCategories;

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public class Secrets
        {
            public string EmbedSecret { get; set; }
            public string JwtSigningKey { get; set; }
        }
        public string BaseUrl { get; set; }
        public string SecretsMgrKey { get; set; }
        public string ServiceAccount { get; set; }
        public string SiteName { get; set; }

        [JsonIgnore]
        public string[] ExcludeCategories
        {
            get
            {
                if (_excludeCategories == null)
                {
                    if (string.IsNullOrWhiteSpace(ExcludeCategoriesJSON))
                        _excludeCategories = new string[0];
                    else
                        _excludeCategories = JsonSerializer.Deserialize<string[]>(ExcludeCategoriesJSON);
                }

                return _excludeCategories;
            }
        }

        [JsonIgnore]
        public string[] TemplateCategories
        {
            get
            {
                if (_templateCategories == null)
                {
                    if (string.IsNullOrWhiteSpace(TemplateCategoriesJSON))
                        _templateCategories = new string[0];
                    else
                        _templateCategories = JsonSerializer.Deserialize<string[]>(TemplateCategoriesJSON);
                }

                return _templateCategories;
            }
        }


        public string ExcludeCategoriesJSON { get; set; }
        public string TemplateCategoriesJSON { get; set; }
    }
}
