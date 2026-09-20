using Corp.Core.Libraries.DataModels;
using Corp.Core.Libraries.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries.ServiceAgent
{
    public class BoldReportServiceAgent
    {
        private readonly TokenServiceAgent _tokenService;

        public BoldReportServiceAgent()
        {
            _tokenService = new TokenServiceAgent();
        }

        public BoldReportServiceAgent(TokenServiceAgent tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Generates a Bold Reports authentication token (bearer format).
        /// </summary>
        public string GenerateToken()
        {
            return _tokenService.GenerateToken();
        }

        public List<ReportModel> GetAllReports()
        {
            // Call your Bold Reports API or DB
            // Example structure:
            return new List<ReportModel>
        {
            new ReportModel { ReportName = "Sales Report", ReportPath = "/Reports/Sales" },
            new ReportModel { ReportName = "Claims Report", ReportPath = "/Reports/Claims" }
        };
        }

        /// <summary>
        /// Retrieves all reports available to the specified user from Bold Reports Server.
        /// </summary>
        public List<ReportModel> GetAllUserReports(string userId)
        {
            string token = _tokenService.GenerateToken();
            string apiUrl = BoldReportsAppSettings.ItemsApiUrl + "?itemType=Report";

            try
            {
                using (var client = new CustomWebClient())
                {
                    client.Headers["Content-type"] = "application/json";
                    client.Headers["Authorization"] = token;
                    client.Encoding = Encoding.UTF8;

                    string responseData = client.DownloadString(new Uri(apiUrl));
                    var apiItems = JsonConvert.DeserializeObject<List<ApiItemResponse>>(responseData);

                    if (apiItems == null)
                        return new List<ReportModel>();

                    var reports = apiItems.Select((item, index) => new ReportModel
                    {
                        ReportId = index + 1,
                        Id = item.Id,
                        ReportName = !string.IsNullOrEmpty(item.Name) ? item.Name : item.ReportName,
                        Name = item.Name,
                        ReportPath = !string.IsNullOrEmpty(item.ItemLocation)
                            ? item.ItemLocation
                            : item.Id.ToString(),
                        Description = item.Description ?? string.Empty,
                        CreatedDate = item.ItemCreatedDate,
                        CategoryName = item.CategoryName ?? string.Empty,
                        ItemLocation = item.ItemLocation,
                        CreatedByDisplayName = item.CreatedByDisplayName ?? string.Empty
                    }).ToList();

                    return reports;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string errorBody = reader.ReadToEnd();
                        throw new ApplicationException(
                            "Bold Reports API error: " + errorBody, ex);
                    }
                }

                throw new ApplicationException(
                    "Failed to retrieve reports from Bold Reports Server: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retrieves a specific report by its sequential ID from the reports list.
        /// </summary>
        public ReportModel GetReportById(int reportId)
        {
            var reports = GetAllUserReports(null);
            return reports.FirstOrDefault(r => r.ReportId == reportId);
        }

        /// <summary>
        /// Retrieves a specific report by its GUID.
        /// </summary>
        public ReportModel GetReportByGuid(string reportGuid)
        {
            var reports = GetAllUserReports(null);
            Guid guid;
            if (Guid.TryParse(reportGuid, out guid))
            {
                return reports.FirstOrDefault(r => r.Id == guid);
            }
            return null;
        }

        /// <summary>
        /// Retrieves a specific report by its server path.
        /// </summary>
        public ReportModel GetReportByPath(string reportPath)
        {
            var reports = GetAllUserReports(null);
            return reports.FirstOrDefault(r =>
                string.Equals(r.ReportPath, reportPath, StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Maps to the Bold Reports REST API item response structure.
    /// </summary>
    internal class ApiItemResponse
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool CanSchedule { get; set; }
        public bool CanDownload { get; set; }
        public bool CanOpen { get; set; }
        public bool CanMove { get; set; }
        public bool CanCopy { get; set; }
        public bool CanClone { get; set; }
        public bool CanCreateItem { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ModifiedById { get; set; }
        public string CreatedByDisplayName { get; set; }
        public int CreatedById { get; set; }
        public string ModifiedByFullName { get; set; }
        public string ItemLocation { get; set; }
        public int ItemType { get; set; }
        public Guid Id { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public DateTime ItemModifiedDate { get; set; }
        public DateTime ItemCreatedDate { get; set; }
        public Guid ReportId { get; set; }
        public string ReportName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Custom WebClient with extended timeout for Bold Reports API calls.
    /// </summary>
    internal class CustomWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            var request = base.GetWebRequest(uri);
            request.Timeout = 4 * 60 * 1000;
            var httpRequest = request as HttpWebRequest;
            if (httpRequest != null)
            {
                httpRequest.KeepAlive = false;
            }
            return request;
        }
    }
}
