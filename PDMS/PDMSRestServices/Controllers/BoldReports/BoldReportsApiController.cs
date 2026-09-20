using Microsoft.AspNetCore.Mvc;
using PDMSRestServices.Models;
using ReportingService.Service.Reporting;
using ReportingService.Services.Security;

namespace MainBoldReportsAPI.Web.Controllers.Reports
{
    [Route("reports/api/[action]")]
    public class BoldReportsApiController : BoldBaseReportsController
    {
        public BoldReportsApiController(IWorkContext workContext, ReportingService.Service.Reporting.RptService reportingSvc
            , IEncryptionService encryptionService, ReportingUserAdminService reportAdminSvc)
            : base(workContext, reportingSvc, encryptionService, reportAdminSvc) { }

        [HttpGet]
        public async Task<IActionResult> IndexAll()
        {
            var results = await _reportingSvc.IndexAll();
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> SearchReports(string keywords)
        {
            var results = await _reportingSvc.SearchReports(keywords);
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> UserCategories()
        {
            var results = await _reportingSvc.GetUserCategories(true);
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> UserTemplates()
        {
            var results = await _reportingSvc.GetUserTemplates();
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> IsReportNameUnique(string category, string name)
        {
            var results = await _reportingSvc.IsReportNameUnique(category, name);
            return Ok(results);
        }

        [HttpPost]
        public async Task<IActionResult> CloneReport([FromForm] CloneRequest req)
        {
            var statusResult = await _reportingSvc.CloneReport(
                req.SrcId,
                req.TgtCategory,
                req.TgtName
            );

            if (!statusResult.Success)
                return BadRequest(statusResult.Errors);

            return Ok(statusResult);
        }


        [HttpGet]
        public async Task<IActionResult> ItemDependancies(string itemId)
        {
            try
            {
                var id = Guid.TryParse(itemId, out var guidId) ? guidId : Guid.Empty;
                var result = await _reportingSvc.ItemDependancies(id);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost()]
        public async Task<IActionResult> Draft(string? templateId, string name)
        {
            var statusResult = await _reportingSvc.DraftFromReport(templateId, name);
            if (!statusResult.Success) return BadRequest(statusResult.Errors);
            return Ok(statusResult.Value);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Draft([FromRoute]Guid id)
        {
            var statusResult = await _reportingSvc.DeleteDraft(id);
            if (!statusResult.Success) return BadRequest(statusResult.Errors);
            return Ok(statusResult.Value);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReport([FromBody] DeleteReportRequest req)
        {
            var statusResult = await _reportingSvc.DeleteReport(req.Id);

            if (!statusResult.Success)
                return BadRequest(statusResult.Errors);

            return Ok(statusResult.Value);
        }

        [HttpGet]
        public async Task<IActionResult> UserReports(string category)
        {
            var results = await _reportingSvc.GetUserReports(category);
            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> AllReportsGrouped()
        {
            var reports = await _reportingSvc.GetUserReports(null);

            var grouped = reports
                .GroupBy(r => r.CategoryName)
                .Select(g => new {
                    category = g.Key,
                    reports = g
                });

            return Ok(grouped);
        }

        [HttpGet]
        public async Task<IActionResult> DataSources()
        {
            try
            {
                var results = await _reportingSvc.GetDataSources();

                if (results == null || !results.Any())
                {
                    return Ok(new List<object>()); // ✅ always return array
                }

                return Ok(results.Select(x => new {
                    Id = x.Id,
                    Name = x.Name
                }));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<RptAuthToken> Token()
        {
            var user = await _workContext.GetCurrentUserAsync();
            return await _reportingSvc.GetUserAuthToken(user.Email);
        }

    }
}
