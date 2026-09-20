using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Caching.Memory;
using ReportingService.Common.Text;
using ReportingService.Services.Configuration;
using ReportingService.Services.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using static ReportingService.Services.Security.SecretsMgrExtensions;

namespace ReportingService.Service.Reporting
{
    public class RptService : BaseReportingService
        
    {

        public RptService(
            IWorkContext workContext,
            BoldReportsSettings settings,
            HttpClient httpClient,
            ISecretsMgrRepository secretsMgr,
            RestApiSettings restApiSettings,
            IHttpContextAccessor httpContextAccessor
        )
        : base(workContext, settings, httpClient, secretsMgr, httpContextAccessor)
        {
            this.restApiSettings = restApiSettings;
        }

        protected RestApiSettings restApiSettings;

        //the Reporting Auth Token for the Current User.
        /// <summary>
        /// This is set by the controller by extracting it from a cookie.
        /// Or issuing a new token if it is not found or expired.
        /// </summary>
        private IList<RptItem> AllReportsCache = null;
        private IList<RptItem> UserReportsCache = null;
        private IList<RptItem> UserCategoryCache = null;
        private IList<Permission> UserPermissionsCache = null;
        //Zero-Width-Non-Breaking-Space, Zero-Width-Space
        private readonly char[] whitespace2 = new char[] { '\u200B', '\uFEFF' };

        public virtual async Task<IList<RptSearchResult>> IndexAll()
        {
            var url = "/reporting/api/" + _settings.SiteName + "/IndexReports/IndexAll";

            var token = GetIncomingToken();

            var searchResults = await ReportApiHelper.Get<RptSearchResult[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptSearchResult[0];

            return searchResults;
        }

        public virtual async Task<IList<RptSearchResult>> SearchReports(string keywords)
        {
            var url = "/reporting/api/" + _settings.SiteName + "/searchMetadata?keywords=" + keywords;

            var token = GetIncomingToken();

            var searchResults = await ReportApiHelper.Get<RptSearchResult[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptSearchResult[0];

            return searchResults;
        }

        protected virtual async Task<IList<RptItem>> _GetUserReports(string category)
        {
            // Build endpoint URL
            var endpoint = $"/reporting/api/site/{_settings.SiteName}/v4.0/items?itemType=Report";

            // Build full URL
            var requestUrl = $"{_settings.BaseUrl}{endpoint}";

            var token = GetIncomingToken();

            // Call API only if cache is empty
            if (UserReportsCache == null)
            {
                UserReportsCache = await ReportApiHelper.Get<RptItem[]>(
                    _client,
                    requestUrl,
                    token
                ) ?? Array.Empty<RptItem>();
            }

            if (category != null && category.StartsWith("/"))
            {
                category = category.Substring(1);
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                return UserReportsCache;
            }

            var reports = UserReportsCache.Where(x => string.Equals(x.CategoryName, category, StringComparison.InvariantCultureIgnoreCase)).ToArray();

            return reports;
        }

        protected virtual async Task<IList<RptItem>> _GetUserCategories()
        {
            if (UserCategoryCache != null) return UserCategoryCache;

            var catPerms = await GetPermissionByType(PermissionEntity.ReportsInCategory, AccessLevel.Create);
            var allRptPerms = await GetPermissionByType(PermissionEntity.AllReports, AccessLevel.Create);

            var url = "/reporting/api/site/" + _settings.SiteName + "/v4.0/items?itemType=Category";

            var token = GetIncomingToken();

            UserCategoryCache = await ReportApiHelper.Get<RptItem[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptItem[0];

            var cats = allRptPerms.Any()
                ? UserCategoryCache.Select(x => new { Cat = x, Perm = catPerms.FirstOrDefault() }).ToList()
                : UserCategoryCache.Join(
                    catPerms,
                    x => x.Id,
                    y => y.ItemId,
                    (x, y) => new { Cat = x, Perm = y }
                ).ToList();

            cats.ForEach(x => x.Cat.CanCreateItem = true);

            return UserCategoryCache;
        }

        /// <summary>
        /// Get permissions for the current user.
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<IList<Permission>> _GetUserPermissions()
        {
            var token = GetIncomingToken();

            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/permissions";

            var permissions = await ReportApiHelper.Get<Permission[]>(
                _client,
                _settings.BaseUrl + url,
                token
            );

            return permissions != null
                ? permissions.ToList()
                : new List<Permission>();
        }

        public virtual async Task<IList<Permission>> GetPermissionByType(PermissionEntity entityType, AccessLevel? accessLevel)
        {
            var perms = await _GetUserPermissions();
            return perms.Where(x => String.Equals(x.PermissionEntity, entityType.ToString(), StringComparison.OrdinalIgnoreCase) && (x.PermissionAccess == accessLevel.ToString())).ToArray();
        }


        public virtual async Task<IList<RptItem>> GetUserReports(string category)
        {
            var items = await _GetUserReports(category);
            if (items == null) return new RptItem[] { };
            items = items.Where(x => !_settings.ExcludeCategories.Contains(x.CategoryName, StringComparer.InvariantCultureIgnoreCase)).ToArray();
            var sortedItems = items.OrderBy(x => x.CategoryName).ThenBy(x => x.Name).ToArray();
            return sortedItems;
        }

        public virtual async Task<IList<RptItem>> GetAllReports()
        {
            var token = GetIncomingToken(); // ✅ FIX

            var url = "/reporting/api/site/" + _settings.SiteName + "/v4.0/items?itemType=Report";

            var items = await ReportApiHelper.Get<RptItem[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptItem[0];

            if (items == null)
                return new RptItem[0];

            var sortedItems = items
                .OrderBy(x => x.CategoryName)
                .ThenBy(x => x.Name)
                .ToArray();

            return sortedItems;
        }

        public virtual async Task<bool> IsReportNameUnique(string category, string name)
        {
            if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(name)) return false;
            var items = await GetAllReports();
            return !items.Any(x => (string.Equals(x.CategoryName, category, StringComparison.InvariantCultureIgnoreCase) && string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)));
        }

        public virtual async Task<IList<RptItem>> GetUserDrafts()
        {
            var url = "/reporting/api/site/" + _settings.SiteName + "/v4.0/reports/drafts";

            var token = GetIncomingToken();

            var items = await ReportApiHelper.Get<RptItem[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptItem[0];

            if (items == null)
                return new RptItem[0];

            var sortedItems = items
                .OrderBy(x => x.Name)
                .ToArray();

            return sortedItems;
        }

        public virtual async Task<RptItem> GetUserDraft(string Id)
        {
            var drafts = await GetUserDrafts();
            return drafts.FirstOrDefault(x => x.Id == Id);
        }

        public virtual async Task<IList<RptItem>> GetUserCategories(bool? canCreateReports)
        {
            var items = await _GetUserCategories();

            if (items == null || !items.Any()) return new RptItem[] { };

            items = items
                .Where(x => !_settings.ExcludeCategories
                    .Contains(x.CategoryName, StringComparer.InvariantCultureIgnoreCase))
                .ToArray();

            var sortedItems = items
                .OrderBy(x => x.CategoryName)
                .ToList();

            return sortedItems;
        }
        public virtual async Task<IList<RptItem>> GetUserTemplates()
        {

            var cats = await _GetUserCategories();
            if (cats == null || !cats.Any()) return new RptItem[] { };
            cats = cats.Where(x => x.CanRead && _settings.TemplateCategories.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)).ToArray();
            var items = await _GetUserReports(null);
            if (items == null) return new RptItem[] { };
            var templates = items.Where(x => cats.Any(y => string.Equals(y.Name, x.CategoryName, StringComparison.InvariantCultureIgnoreCase)) && x.CanRead);
            var sortedTemplates = templates.OrderBy(x => x.Name).ToArray();
            return sortedTemplates;
        }

        public virtual async Task<RptItem> GetCategory(string Name)
        {
            var items = await _GetUserCategories();
            return items.FirstOrDefault(x => x.Name == Name);
        }

        public virtual async Task<RptItem> GetReportDetails(string reportId)
        {
            var isGuid = false;
            var guidId = Guid.Empty;
            isGuid = Guid.TryParse(reportId, out guidId);
            if (isGuid)
            {
                var item = await GetItem(guidId);
                if (item == null) return null;
                reportId = $"{item.CategoryName}/{item.Name}";
            }
            var reports = await _GetUserReports(null);
            var report = reports.FirstOrDefault(x => $"{x.CategoryName}/{x.Name}" == reportId);
            return report;
        }

        public virtual async Task<Item> GetItem(Guid id)
        {
            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/items/" + id.ToString();

            var token = GetIncomingToken();

            var item = await ReportApiHelper.Get<Item>(
                _client,
                _settings.BaseUrl + url,
                token
            );

            return item != null ? item : default(Item);
        }

        public virtual async Task<FuncResult<string>> CloneReport(string SrcId, string tgtCategory, string tgtName)
        {
            Console.WriteLine($"SrcId: {SrcId}");
            Console.WriteLine($"Category: {tgtCategory}");
            Console.WriteLine($"Name: {tgtName}");

            var reports = await GetReportDetails(SrcId);
            var cat = await GetCategory(tgtCategory);

            if (!reports.CanRead)
                return new FuncResult<string>(false, "Permission Denied: Report is not readable");

            if (!cat.CanWrite)
                return new FuncResult<string>(false, "Permission Denied: Category is not writeable");

            var url = "/reporting/api/site/" + _settings.SiteName + "/v2.0/report/" + reports.Id + "/copy";

            var apiBody = new
            {
                CategoryId = cat.Id,
                Name = tgtName,
                IsPublic = false,
                IsCopyDataSource = false,
                IsCopyDataset = false
            };

            var token = GetIncomingToken();

            var apiResult = await ReportApiHelper.Post<ApiResult>(
                _client,
                _settings.BaseUrl + url,
                token,
                apiBody
            );


            // ✅ FULL DEBUG
            Console.WriteLine("===== CLONE DEBUG =====");

            if (apiResult == null)
            {
                Console.WriteLine("apiResult = NULL ❌");
            }
            else
            {
                Console.WriteLine($"Status: {apiResult.Status}");
                Console.WriteLine($"PublishedItemId: {apiResult.PublishedItemId}");
                Console.WriteLine($"Message: {apiResult.StatusMessage}");
            }

            Console.WriteLine("========================");


            if (apiResult == null)
                apiResult = new ApiResult();

            return new FuncResult<string>(
                apiResult.Status,
                apiResult.PublishedItemId,   // ✅ THIS IS THE KEY FIX
                apiResult.StatusMessage
            );
        }

        public virtual async Task<FuncResult<Item>> DraftFromReport(string SrcId, string tgtName)
        {
            var srcRpt = !string.IsNullOrWhiteSpace(SrcId)
                ? await GetReportDetails(SrcId)
                : null;

            if (srcRpt != null && !srcRpt.CanRead)
                return new FuncResult<Item>(false, "Permission Denied: Source Report is not readable");

            var urlSrcId = HttpUtility.UrlEncode(SrcId);
            var urlTgtName = HttpUtility.UrlEncode(tgtName);

            var url = "/reporting/api/" + _settings.SiteName + "/draft?Name=" + urlTgtName + "&SrcReportId=" + urlSrcId;

            var apiBody = new { };

            var token = GetIncomingToken();

            var apiResult = await ReportApiHelper.Post<Item>(
                _client,
                _settings.BaseUrl + url,
                token,
                apiBody
            );

            if (apiResult == null)
                apiResult = new Item();

            return new FuncResult<Item>(
                apiResult != null && apiResult.Id != null,
                apiResult,
                "Error Creating Draft Report"
            );
        }

        public virtual async Task<FuncResult<bool>> DeleteDraft(Guid Id)
        {
            var draftRpt = await GetUserDraft(Id.ToString());

            if (draftRpt == null || string.IsNullOrWhiteSpace(draftRpt.Id) || draftRpt.Id == Guid.Empty.ToString())
                return new FuncResult<bool>(false, "Draft Not Found");

            if (!draftRpt.CanDelete)
                return new FuncResult<bool>(false, "Delete Permission Denied");

            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/items/" + draftRpt.Id;

            var apiBody = new { };

            var token = GetIncomingToken();

            var apiResult = await ReportApiHelper.Delete<ApiResult>(
                _client,
                _settings.BaseUrl + url,
                token,
                apiBody
            );

            if (apiResult == null)
                apiResult = new ApiResult();

            return new FuncResult<bool>(
                apiResult.ApiStatus,
                apiResult.ApiStatus,
                "Error Deleteing Draft"
            );
        }

        public class ItemJsonFile
        {
            public string Name { get; set; }
            public string ItemJSON { get; set; }
        }

        public virtual async Task<string[]> ItemDependancies(Guid itemId)
        {
            var result = new List<string>(4);
            result.Add(itemId.ToString());

            var item = await GetItem(itemId);

            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return new string[0];

            var rptItem = item.ToItemDefinition();
            rptItem.ItemXml = await DownloadItemDef(item);

            var dp = DependantItemIds(rptItem);

            foreach (var id in dp)
            {
                var depIds = await ItemDependancies(Guid.Parse(id));
                result.AddRange(depIds);
            }

            return result.Distinct().ToArray();
        }

        public virtual async Task<ItemJsonFile> DownloadItem(Guid itemId)
        {

            var item = await GetItem(itemId);
            if (item == null || string.IsNullOrWhiteSpace(item.Id)) throw new ArgumentOutOfRangeException("itemId", itemId, "{0} not Found");
            var catName = (string.IsNullOrWhiteSpace(item.CategoryName) ? "" : item.CategoryName + "-");
            var filename = $"{catName}{item.Name}{item.Extension}";
            var rptItem = item.ToItemDefinition();
            rptItem.ItemXml = await DownloadItemDef(item);
            return new ItemJsonFile() { Name = filename, ItemJSON = JsonSerializer.Serialize(rptItem) };
        }

        protected virtual async Task<Base64String> DownloadItemDef(Item item)
        {
            var token = GetIncomingToken(); // ✅ FIX

            var url = "";
            ItemContent itemContent = null;

            switch (item.ItemType)
            {
                case "Report":
                case "ReportPart":
                    url = "/reporting/api/site/" + _settings.SiteName + "/reports/download";

                    itemContent = await ReportApiHelper.Post<ItemContent>(
                        _client,
                        _settings.BaseUrl + url,
                        token, // ✅ FIX
                        new { ItemId = item.Id }
                    );
                    break;

                case "Dataset":
                    {
                        var req = new
                        {
                            ItemType = (ItemType)Enum.Parse(typeof(ItemType), item.ItemType),
                            ReportReferences = new string[] { item.Id.Trim().Trim(whitespace2) }
                        };

                        url = "/reporting/api/site/" + _settings.SiteName + "/reports/datasets/get";

                        var itemContentList = await ReportApiHelper.Post<ItemContent[]>(
                            _client,
                            _settings.BaseUrl + url,
                            token, // ✅ FIX
                            req
                        );

                        itemContent = itemContentList?.FirstOrDefault();
                        break;
                    }

                case "Datasource":
                    {
                        var req = new
                        {
                            ItemType = (ItemType)Enum.Parse(typeof(ItemType), item.ItemType),
                            ReportReferences = new string[] { item.Id.Trim().Trim(whitespace2) }
                        };

                        url = "/reporting/api/site/" + _settings.SiteName + "/reports/data-sources/download";

                        var itemContentList = await ReportApiHelper.Post<ItemContent[]>(
                            _client,
                            _settings.BaseUrl + url,
                            token, // ✅ FIX
                            req
                        );

                        itemContent = itemContentList?.FirstOrDefault();
                        break;
                    }
            }

            return new Base64String(itemContent != null ? itemContent.FileContent : null);
        }

        protected virtual string[] DependantItemIds(ItemDefinition item)
        {
            var dependancies = new List<string>(4);
            if (item.ItemXml.DecodedBytes == null || item.ItemXml.DecodedBytes.Length == 0) dependancies.ToArray();

            var itemDef = item.ItemXml.DecodedString;
            itemDef = itemDef?.TrimStart().TrimStart(whitespace2);
            if (string.IsNullOrWhiteSpace(itemDef)) return dependancies.ToArray();
            XmlReaderSettings xmlSettings = new XmlReaderSettings { NameTable = new NameTable() };
            XmlNamespaceManager xmlns = new XmlNamespaceManager(xmlSettings.NameTable);
            xmlns.AddNamespace("rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            xmlns.AddNamespace("df", "");
            XmlParserContext context = new XmlParserContext(null, xmlns, "", XmlSpace.Default);
            XmlReader reader = XmlReader.Create(new StringReader(itemDef), xmlSettings, context);
            var xmlItemDef = XDocument.Load(reader);
            var ns = xmlItemDef.Root.Name.Namespace;
            var rd = xmlItemDef.Root.GetNamespaceOfPrefix("rd");

            var datasets = xmlItemDef.Descendants(ns + "SharedDataSetReference").Select(x => x.Value?.Trim().Trim(whitespace2)).ToArray();
            dependancies.AddRange(datasets);
            var datasources = xmlItemDef.Descendants(ns + "DataSourceReference").Select(x => x.Value?.Trim().Trim(whitespace2)).ToArray();
            dependancies.AddRange(datasources);
            var rptParts = xmlItemDef.Descendants(rd + "ReferenceId").Select(x => x.Value?.Trim().Trim(whitespace2)).ToArray();
            dependancies.AddRange(rptParts);

            for (var i = 0; i < dependancies.Count; i++)
            {
                var xd = dependancies[i].Split('/');
                if (xd.Length > 1) dependancies[i] = xd[1].Trim().Trim(whitespace2);
            }

            return dependancies.ToArray();
        }

        public virtual Stream ToDelimitedTextStream(char delimiter, List<Dictionary<string, object>> Columns, List<Dictionary<string, object>> Data)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.Delimiter = delimiter.ToString();
            var ms = new MemoryStream();
            using (TextWriter writer = new StreamWriter(ms, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), bufferSize: 1024, leaveOpen: true))
            using (var csv = new CsvWriter(writer, config))
            {
                //write header
                if (Columns != null && Columns.Count > 0)
                {
                    foreach (var col in Columns)
                    {
                        if (col == null || col.Count == 0) continue;
                        var colName = col.ContainsKey("HeaderText") ? col["HeaderText"] : col.ContainsKey("Name") ?  col["Name"]?.ToString() : "";
                        csv.WriteField(colName);
                    }
                    csv.NextRecord();
                }
                //write data
                if (Data != null && Data.Count > 0)
                {
                    foreach (var row in Data)
                    {
                        if (row == null || row.Count == 0) continue;
                        foreach (var col in Columns)
                        {
                            if (col == null || col.Count == 0) continue;
                            csv.WriteField(row[col["Name"]?.ToString()]);
                        }
                        csv.NextRecord();
                    }
                }
                csv.Flush();
                writer.Flush();
            }
            ms.Position = 0;
            return ms;
        }

        public virtual async Task<IList<RptItem>> GetDataSources()
        {
            var url = $"/reporting/api/site/{_settings.SiteName}/v4.0/items?itemType=Datasource";

            var token = GetIncomingToken();

            var items = await ReportApiHelper.Get<RptItem[]>(
                _client,
                _settings.BaseUrl + url,
                token
            ) ?? new RptItem[0];

            if (items == null)
                return new RptItem[0];

            // ✅ Optional: filter like your other methods
            var filtered = items
                .Where(x => !_settings.ExcludeCategories
                    .Contains(x.CategoryName, StringComparer.InvariantCultureIgnoreCase))
                .OrderBy(x => x.Name)
                .ToList();

            return filtered;
        }

        public async virtual Task<byte[]> HtmlToPdfStream(string html)
        {
            var url = restApiSettings.BaseUrl + "PdfConverter/ConvertHtmlToPdf";

            var formFields = new Dictionary<string, string>()
            {
                { "html", html }
            };

            using (var request = new HttpRequestMessage(HttpMethod.Post, url))
            {
                request.Content = new FormUrlEncodedContent(formFields);

                using (var response = await _client.SendAsync(
                    request,
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();

                    var responseStream = await response.Content.ReadAsByteArrayAsync();
                    return responseStream;
                }
            }
        }

        public virtual async Task<FuncResult<bool>> DeleteReport(Guid id)
        {
            var item = await GetItem(id);

            if (item == null || string.IsNullOrWhiteSpace(item.Id))
                return new FuncResult<bool>(false, "Report Not Found");

            var url = "/reporting/api/site/" + _settings.SiteName + "/v1.0/items/" + item.Id;

            var token = GetIncomingToken();

            var apiResult = await ReportApiHelper.Delete<ApiResult>(
                _client,
                _settings.BaseUrl + url,
                token,
                new { }
            );

            if (apiResult == null)
                apiResult = new ApiResult();

            return new FuncResult<bool>(
                apiResult.ApiStatus,
                apiResult.ApiStatus,
                "Error deleting report"
            );
        }
    }
}
