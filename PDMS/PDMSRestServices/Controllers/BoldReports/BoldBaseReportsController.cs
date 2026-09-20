using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using ReportingService.Service.Reporting;
using ReportingService.Services.Http;
using ReportingService.Services.Security;

namespace MainBoldReportsAPI.Web.Controllers.Reports
{
    public class BoldBaseReportsController : Controller
    {
        protected readonly IWorkContext _workContext;
        protected readonly ReportingService.Service.Reporting.RptService _reportingSvc;
        protected readonly IEncryptionService _encryptionService;
        protected readonly ReportingUserAdminService _reportingAdminSvc;

        public BoldBaseReportsController(IWorkContext workContext, ReportingService.Service.Reporting.RptService reportingSvc
            , IEncryptionService encryptionService , ReportingUserAdminService reportAdminSvc)
        {
            _workContext = workContext;
            _reportingSvc = reportingSvc;
            _encryptionService = encryptionService;
            _reportingAdminSvc = reportAdminSvc;
        }

        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            await base.OnActionExecutionAsync(context, next);
        }

        public class ReportCategory
        {
            public ReportCategory() { }
            public ReportCategory(string name, string id)
            {
                Name = name;
                Id = id;
            }

            public string Name { get; set; }
            public string Id { get; set; }
        }
        public class ReportIdentifier
        {
            public ReportIdentifier() { }
            //public ReportIdentifier(ReportCategory category, string name)
            //{
            //    ReportName = name;
            //    ReportCategory = category;
            //    ParseReportPath(ReportCategory, ReportName, null, false);
            //}

            public ReportIdentifier(string rid, ReportCategory? category, string name, bool isDraft)
            {
                IsDraft = isDraft;
                ReportId = rid;
                ReportName = name;
                ReportCategory = isDraft ? new ReportCategory("draft","") : category;
                ParseReportPath(ReportCategory, ReportName, ReportId, IsDraft);
            }

            public bool IsDraft { get; protected set; } = false;
            public string ReportId { get; protected set; }
            public string ReportName { get; protected set; }
            public ReportCategory? ReportCategory { get; protected set; }
            public string ReportPath { get; protected set; }

            public void ParseReportPath(ReportCategory? category, string? name, string? rid, bool isDraft)
            {
                var isRptId = Guid.TryParse(rid, out Guid rptId);
                var isId = Guid.TryParse(category?.Id, out Guid id);
                if (isId && string.IsNullOrWhiteSpace(name)) ReportPath = id.ToString();
                else if (!isRptId) ReportPath = $"{category?.Name}/{name}";
                else ReportPath = $"{category?.Name}/{name}?rid={rid}";
            }
        }

        [NonAction]
        public async Task<RptAuthToken> SetAuthToken()
        {
            var user = await _workContext.GetCurrentUserAsync();

            var token = await _reportingSvc.GetCurrentUserToken(); // ✅ FIX

            var options = new CookieOptions()
            {
                Domain = HttpContext.Request.Host.Host,
                Path = "/reports",
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict
            };

            var cookieValue = _encryptionService.EncryptText(
                JsonConvert.SerializeObject(token)
            );

            HttpContext.Response.Cookies.Append(
                $"{MPCCookieDefaults.Prefix}{MPCCookieDefaults.reportingAuthToken}",
                cookieValue,
                options
            );

            return token;
        }

    }
}
