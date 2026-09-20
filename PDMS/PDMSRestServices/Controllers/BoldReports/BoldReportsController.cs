using Microsoft.AspNetCore.Mvc;
using ReportingService.Service.Reporting;
using ReportingService.Services.Security;
using System.Text;

namespace MainBoldReportsAPI.Web.Controllers.Reports
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class BoldReportsController : BoldBaseReportsController
    {
        public BoldReportsController(
            IWorkContext workContext,
            ReportingService.Service.Reporting.RptService reportingSvc,
            IEncryptionService encryptionService,
            ReportingUserAdminService reportAdminSvc)
            : base(workContext, reportingSvc, encryptionService, reportAdminSvc)
        {
        }


        [HttpGet("{category?}")]
        public async Task<IActionResult> Home(string? category)
        {
            IList<RptItem> reports;
            try
            {
                reports = await _reportingSvc.GetUserReports(category);
            }
            catch (HttpRequestException)
            {
                reports = new RptItem[] { };
            }

            return View(reports);
        }
        public async Task<IActionResult> CustomReports()
        {
            IList<RptItem> reports;
            try
            {
                reports = await _reportingSvc.GetUserReports("Custom Reports");
            }
            catch (HttpRequestException)
            {
                reports = new RptItem[] { };
            }

            return View("Home", reports);
        }
        public async Task<IActionResult> MyDrafts()
        {
            IList<RptItem> reports;
            try
            {
                reports = await _reportingSvc.GetUserDrafts();
            }
            catch (HttpRequestException)
            {
                reports = new RptItem[] { };
            }

            return View(reports);
        }

        [HttpGet("{category}/{name}")]
        public async Task<IActionResult> Viewer(string category, string name)
        {
            var rid = new ReportIdentifier(null, new ReportCategory(category, null), name, false);
            var rpt = await _reportingSvc.GetReportDetails(rid.ReportId);
            if (rpt != null && !rpt.CanRead) return Forbid();
            return View("viewer", rid);
        }

        [HttpGet("{category}/{name?}")]
        public async Task<IActionResult> Editor(string category, string? name, string? rid, bool? isDraft)
        {

            var isRptId = Guid.TryParse(rid, out var guidRid1);
            var isCatId = Guid.TryParse(category, out var guidRid2);
            Guid? guidRid = isRptId ? guidRid1 : (isCatId ? guidRid2 : null);
            RptItem rptObj = null;
            if (isDraft.HasValue && isDraft.Value)
                rptObj = (isRptId) ? await _reportingSvc.GetUserDraft(guidRid.ToString()) : null;
            else
                rptObj = (isRptId) ? await _reportingSvc.GetReportDetails(guidRid.ToString()) : null;
            var catObj = (!isCatId) ? await _reportingSvc.GetCategory(category) : null;
            ReportCategory catParam;
            if (catObj == null && rptObj != null)
            {
                catParam = new ReportCategory(rptObj.CategoryName, rptObj.CategoryId);
            }
            else
                catParam = new ReportCategory(catObj?.Name, catObj?.Id);
            var rptId = new ReportIdentifier(guidRid?.ToString(), catParam, name, isDraft.HasValue && isDraft.Value);
            var rpt = await _reportingSvc.GetReportDetails(rptId.ReportId);
            if (rpt != null && (!rpt.CanRead || !rpt.CanWrite)) return Forbid();
            return View("editor", rptId);
        }

        public IActionResult QueryDesigner()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> OpenBoldUI()
        {
            var user = await _workContext.GetCurrentUserAsync();

            var rUser = await _reportingAdminSvc.GetUser(user.Email);

            return Redirect(_reportingAdminSvc.BoldUIUrl(rUser, user));
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> DownloadItem(string itemId)
        {
            try
            {
                var id = Guid.TryParse(itemId, out var guidId) ? guidId : Guid.Empty;
                var result = await _reportingSvc.DownloadItem(id);
                return File(Encoding.Unicode.GetBytes(result.ItemJSON), "application/octet-stream", result.Name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        public class ExportPreviewRequest
        {
            public List<Dictionary<string, object>> Columns { get; set; } = new List<Dictionary<string, object>>() { new Dictionary<string, object>() };
            public List<Dictionary<string, object>> Data { get; set; } = new List<Dictionary<string, object>>() { new Dictionary<string, object>() };
            public char Delimiter { get; set; } = ',';
        }

        public async Task<IActionResult> ExportPreviewToDelimited([FromBody] ExportPreviewRequest request)
        {
            try
            {
                var ext = request.Delimiter == ',' ? "csv" : "txt";
                var stream = _reportingSvc.ToDelimitedTextStream(request.Delimiter, request.Columns, request.Data);
                return File(stream, "application/octet-stream", $"PreviewExport.{ext}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
 
    }
}
