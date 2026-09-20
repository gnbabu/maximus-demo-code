
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using PDMSRestServices.Facade;
using PDMSRestServices.Helpers;
using PDMSRestServices.Models;
using System.Data;
using System.Reflection;
using System.Text;


namespace PDMSRestServices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {

        private static readonly List<Reports> StandardReports =
            [
                new Reports { ReportId = 1, ReportName = "Credentialing Clean File Report" },
                new Reports { ReportId = 2, ReportName = "Credentialing Flagged Files Report" },
                new Reports { ReportId = 3, ReportName = "Provider Re-credentialing Due Report" },
                new Reports { ReportId = 4, ReportName = "Credentialing – Provider Contracting > 36 Months Report" },
                new Reports { ReportId = 5, ReportName = "Credentialing – Clean and Flagged Files Summary Report" }
            ];
        private static readonly List<Reports> CustomReports =
            [
                new Reports { ReportId = 1, ReportName = "Custom Report" }

            ];

        private static List<ReportDetails> reports = new List<ReportDetails>
        {
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 01, 10), ProviderType = "Provider Type A", ProviderName = "Provider A", ProviderSpecialty = "Specialty A", MedicaidId = "123456789", ProviderNpi = new DateTime(2024, 12, 12), PrimaryCityState = "City A", PrimaryCount = "Count A", CommitDecision = new DateTime(2023, 01, 15) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 02, 14), ProviderType = "Provider Type B", ProviderName = "Provider B", ProviderSpecialty = "Specialty B", MedicaidId = "234567890", ProviderNpi = new DateTime(2024, 11, 11), PrimaryCityState = "City B", PrimaryCount = "Count B", CommitDecision = new DateTime(2023, 02, 19) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 03, 20), ProviderType = "Provider Type C", ProviderName = "Provider C", ProviderSpecialty = "Specialty C", MedicaidId = "345678901", ProviderNpi = new DateTime(2024, 10, 10), PrimaryCityState = "City C", PrimaryCount = "Count C", CommitDecision = new DateTime(2023, 03, 25) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 04, 15), ProviderType = "Provider Type D", ProviderName = "Provider D", ProviderSpecialty = "Specialty D", MedicaidId = "456789012", ProviderNpi = new DateTime(2024, 09, 09), PrimaryCityState = "City D", PrimaryCount = "Count D", CommitDecision = new DateTime(2023, 04, 20) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 05, 10), ProviderType = "Provider Type E", ProviderName = "Provider E", ProviderSpecialty = "Specialty E", MedicaidId = "567890123", ProviderNpi = new DateTime(2024, 08, 08), PrimaryCityState = "City E", PrimaryCount = "Count E", CommitDecision = new DateTime(2023, 05, 15) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 06, 05), ProviderType = "Provider Type F", ProviderName = "Provider F", ProviderSpecialty = "Specialty F", MedicaidId = "678901234", ProviderNpi = new DateTime(2024, 07, 07), PrimaryCityState = "City F", PrimaryCount = "Count F", CommitDecision = new DateTime(2023, 06, 10) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 07, 15), ProviderType = "Provider Type G", ProviderName = "Provider G", ProviderSpecialty = "Specialty G", MedicaidId = "789012345", ProviderNpi = new DateTime(2024, 06, 06), PrimaryCityState = "City G", PrimaryCount = "Count G", CommitDecision = new DateTime(2023, 07, 20) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 08, 05), ProviderType = "Provider Type H", ProviderName = "Provider H", ProviderSpecialty = "Specialty H", MedicaidId = "890123456", ProviderNpi = new DateTime(2024, 05, 05), PrimaryCityState = "City H", PrimaryCount = "Count H", CommitDecision = new DateTime(2023, 08, 10) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 09, 12), ProviderType = "Provider Type I", ProviderName = "Provider I", ProviderSpecialty = "Specialty I", MedicaidId = "901234567", ProviderNpi = new DateTime(2024, 04, 04), PrimaryCityState = "City I", PrimaryCount = "Count I", CommitDecision = new DateTime(2023, 09, 17) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 10, 20), ProviderType = "Provider Type J", ProviderName = "Provider J", ProviderSpecialty = "Specialty J", MedicaidId = "012345678", ProviderNpi = new DateTime(2024, 03, 03), PrimaryCityState = "City J", PrimaryCount = "Count J", CommitDecision = new DateTime(2023, 10, 25) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 11, 10), ProviderType = "Provider Type K", ProviderName = "Provider K", ProviderSpecialty = "Specialty K", MedicaidId = "123456790", ProviderNpi = new DateTime(2024, 02, 02), PrimaryCityState = "City K", PrimaryCount = "Count K", CommitDecision = new DateTime(2023, 11, 15) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 12, 01), ProviderType = "Provider Type L", ProviderName = "Provider L", ProviderSpecialty = "Specialty L", MedicaidId = "234567891", ProviderNpi = new DateTime(2024, 01, 01), PrimaryCityState = "City L", PrimaryCount = "Count L", CommitDecision = new DateTime(2023, 12, 06) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 01, 20), ProviderType = "Provider Type M", ProviderName = "Provider M", ProviderSpecialty = "Specialty M", MedicaidId = "345678902", ProviderNpi = new DateTime(2024, 12, 20), PrimaryCityState = "City M", PrimaryCount = "Count M", CommitDecision = new DateTime(2023, 01, 25) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 02, 10), ProviderType = "Provider Type N", ProviderName = "Provider N", ProviderSpecialty = "Specialty N", MedicaidId = "456789013", ProviderNpi = new DateTime(2024, 11, 15), PrimaryCityState = "City N", PrimaryCount = "Count N", CommitDecision = new DateTime(2023, 02, 15) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 03, 05), ProviderType = "Provider Type O", ProviderName = "Provider O", ProviderSpecialty = "Specialty O", MedicaidId = "567890124", ProviderNpi = new DateTime(2024, 10, 10), PrimaryCityState = "City O", PrimaryCount = "Count O", CommitDecision = new DateTime(2023, 03, 10) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 04, 25), ProviderType = "Provider Type P", ProviderName = "Provider P", ProviderSpecialty = "Specialty P", MedicaidId = "678901235", ProviderNpi = new DateTime(2024, 09, 05), PrimaryCityState = "City P", PrimaryCount = "Count P", CommitDecision = new DateTime(2023, 04, 30) },
            new ReportDetails { CredentialingTaskType = "License Verification", StartDate = new DateTime(2023, 05, 30), ProviderType = "Provider Type Q", ProviderName = "Provider Q", ProviderSpecialty = "Specialty Q", MedicaidId = "789012346", ProviderNpi = new DateTime(2024, 08, 15), PrimaryCityState = "City Q", PrimaryCount = "Count Q", CommitDecision = new DateTime(2023, 06, 04) }
        };



        [HttpGet("reports-by-date-range")]
        public IActionResult GetReportsByDateRange()
        {
            //var filteredReports = Reports.Where(r => r.StartDate >= startDate && r.StartDate <= endDate).ToList();

            //if (!filteredReports.Any())
            //{
            //    return NotFound("No reports found within the given date range.");
            //}

            return Ok(reports);
        }

        [HttpGet("standard")]
        [ProducesResponseType(typeof(ReportsResponse), StatusCodes.Status200OK)]
        public ActionResult<ReportsResponse> GetStandardReports()
        {
            return Ok(new ReportsResponse
            {
                Count = StandardReports.Count,
                Data = StandardReports
            });
        }
        
        [HttpGet("custom")]
        [ProducesResponseType(typeof(ReportsResponse), StatusCodes.Status200OK)]
        public ActionResult<ReportsResponse> GetCustomReports()
        {
            return Ok(new ReportsResponse
            {
                Count = CustomReports.Count,
                Data = CustomReports
            });
        }

        [HttpGet("GetProviderReCredentialingDueReport")]
        [ProducesResponseType(typeof(List<ProviderReCredentialingDue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProviderReCredentialingDue>> GetProviderReCredentialingDueReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetProviderReCredentialingDueReport(
                startDate.Value,
                endDate.Value);

            return Ok(result);
        }

        [HttpGet("GetProviderCredentialingCleanFile")]
        [ProducesResponseType(typeof(List<ProviderCredentialingApproved>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProviderCredentialingApproved>> GetProviderCredentialingCleanFile([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade
                .GetProviderCredentialingApprovedReport(startDate.Value, endDate.Value);

            return Ok(result);
        }

        [HttpGet("GetProviderCredentialingFlaggedFiles")]
        [ProducesResponseType(typeof(List<ProviderCredentialingCommitteeDecision>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProviderCredentialingCommitteeDecision>>
        GetProviderCredentialingFlaggedFiles(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade
                .GetProviderCredentialingCommitteeDecisionReport(
                    startDate.Value,
                    endDate.Value);

            return Ok(result);
        }


        [HttpGet("GetProviderCredentialinggraterthan36MonthsReport")]
        [ProducesResponseType(typeof(List<ProviderReCredentialingOverdue>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<ProviderReCredentialingOverdue>> GetProviderCredentialinggraterthan36MonthsReport([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade
                .GetProviderReCredentialingOverdueReport(
                    startDate.Value,
                    endDate.Value);

            return Ok(result);
        }

        [HttpGet("GetCredentialngCleanandFlaggedfiles")]
        [ProducesResponseType(typeof(List<CredentialingFileProcessingSummary>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<List<CredentialingFileProcessingSummary>> GetCredentialngCleanandFlaggedfiles([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade
                .GetCredentialingFileProcessingSummaryReport(
                    startDate.Value,
                    endDate.Value);

            return Ok(result);
        }

        [HttpGet("DownloadProviderReCredentialingDueReport")]
        public IActionResult DownloadProviderReCredentialingDueReport(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string format)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetProviderReCredentialingDueReport(
                startDate.Value,
                endDate.Value);

            var columns = new List<ReportColumn<ProviderReCredentialingDue>>
            {
                new() { Header = "Provider Name", Value = x => x.ProviderName },
                new() { Header = "Provider Specialty", Value = x => x.ProviderSpecialty },
                new() { Header = "Provider NPI", Value = x => x.NPI },
                new() { Header = "Provider Re - Cred Due Date", Value = x => x.ReCredentialingDueDate?.ToString("MM/dd/yyyy") },
                new() { Header = "Status", Value = x => x.Status }
            };


            switch (format?.ToLower())
            {
                case "csv":
                    return GenerateCsv(result, "ReCredentialingDue.csv");

                case "excel":
                    return GenerateExcel(result, "ReCredentialingDue.xlsx");

                case "pdf":
                    return GeneratePdf(result, "ReCredentialingDue.pdf", columns);

                default:
                    return BadRequest(new { message = "Invalid format" });
            }
        }

        [HttpGet("DownloadProviderCredentialingCleanFile")]
        public IActionResult DownloadProviderCredentialingCleanFile(
           [FromQuery] DateTime? startDate,
           [FromQuery] DateTime? endDate,
           [FromQuery] string format)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetProviderCredentialingApprovedReport(
                startDate.Value,
                endDate.Value);

            var columns = new List<ReportColumn<ProviderCredentialingApproved>>
            {
                new() { Header = "Credentialing Task Type", Value = x => x.CredentialingTaskType },
                new() { Header = "Start Date", Value = x => x.StartDate?.ToString("MM/dd/yyyy") },
                new() { Header = "Provider Name", Value = x => x.ProviderName },
                new() { Header = "Provider Type", Value = x => x.ProviderType },
                new() { Header = "Provider Specialty", Value = x => x.ProviderSpecialty },
                new() { Header = "Provider NPI", Value = x => x.MedicaidId },
                new() { Header = "Provider MED ID", Value = x => x.NPI },
                new() { Header = "Primary Practice City", Value = x => x.PrimaryCity },
                new() { Header = "Primary Practice State", Value = x => x.PrimaryState },
                new() { Header = "Primary Practice County", Value = x => x.PrimaryCounty },
                new() { Header = "Date Approved", Value = x => x.ApprovedDate?.ToString("MM/dd/yyyy") },
                new() { Header = "Approved By", Value = x => x.ApprovedBy },
            };

            switch (format?.ToLower())
            {
                case "csv":
                    return GenerateCsv(result, "CredentialingCleanFile.csv");

                case "excel":
                    return GenerateExcel(result, "CredentialingCleanFile.xlsx");

                case "pdf":
                    return GeneratePdf(result, "CredentialingCleanFile.pdf", columns);

                default:
                    return BadRequest(new { message = "Invalid format" });
            }
        }

        [HttpGet("DownloadProviderCredentialingFlaggedFiles")]
        public IActionResult DownloadProviderCredentialingFlaggedFiles(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string format)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetProviderCredentialingCommitteeDecisionReport(
                startDate.Value,
                endDate.Value);


            var columns = new List<ReportColumn<ProviderCredentialingCommitteeDecision>>
            {
                new() { Header = "Credentialing Task Type", Value = x => x.CredentialingTaskType },
                new() { Header = "Start Date", Value = x => x.StartDate?.ToString("MM/dd/yyyy") },
                new() { Header = "Provider Name", Value = x => x.ProviderName },
                new() { Header = "Provider Type", Value = x => x.ProviderType },
                new() { Header = "Provider Specialty", Value = x => x.ProviderSpecialty },
                new() { Header = "Provider NPI", Value = x => x.MedicaidId },
                new() { Header = "Provider MED ID", Value = x => x.NPI },
                new() { Header = "Primary Practice City", Value = x => x.PrimaryCity },
                new() { Header = "Primary Practice State", Value = x => x.PrimaryState },
                new() { Header = "Primary Practice County", Value = x => x.PrimaryCounty },
                new() { Header = "Committee Decision", Value = x => x.CommitteeDecision },
                new() { Header = "Decision Date", Value = x => x.DecisionDate?.ToString("MM/dd/yyyy") }
            };

            switch (format?.ToLower())
            {
                case "csv":
                    return GenerateCsv(result, "CredentialingFlaggedFiles.csv");

                case "excel":
                    return GenerateExcel(result, "CredentialingFlaggedFiles.xlsx");

                case "pdf":
                    return GeneratePdf(result, "CredentialingFlaggedFiles.pdf", columns);

                default:
                    return BadRequest(new { message = "Invalid format" });
            }
        }

        [HttpGet("DownloadProviderCredentialinggraterthan36MonthsReport")]
        public IActionResult DownloadProviderCredentialinggraterthan36MonthsReport(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string format)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetProviderReCredentialingOverdueReport(
                startDate.Value,
                endDate.Value);


            var columns = new List<ReportColumn<ProviderReCredentialingOverdue>>
            {
                new() { Header = "Provider Name", Value = x => x.ProviderName },
                new() { Header = "Provider Type", Value = x => x.ProviderType },
                new() { Header = "Provider NPI", Value = x => x.NPI },
                new() { Header = "Provider Re- Cred Due Date", Value = x => x.ReCredentialingDueDate?.ToString("MM/dd/yyyy") },
                new() { Header = "Most Recent Date Approved", Value = x => x.MostRecentDateApproved?.ToString("MM/dd/yyyy") }
            };

            switch (format?.ToLower())
            {
                case "csv":
                    return GenerateCsv(result, "ReCredentialingOver36Months.csv");

                case "excel":
                    return GenerateExcel(result, "ReCredentialingOver36Months.xlsx");

                case "pdf":
                    return GeneratePdf(result, "ReCredentialingOver36Months.pdf", columns);

                default:
                    return BadRequest(new { message = "Invalid format" });
            }
        }

        [HttpGet("DownloadCredentialngCleanandFlaggedfiles")]
        public IActionResult DownloadCredentialngCleanandFlaggedfiles(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string format)
        {
            if (!startDate.HasValue)
                return BadRequest(new { message = "startDate is required." });

            if (!endDate.HasValue)
                return BadRequest(new { message = "endDate is required." });

            if (startDate > endDate)
                return BadRequest(new { message = "startDate cannot be greater than endDate." });

            var result = CredentialingFacade.GetCredentialingFileProcessingSummaryReport(
                startDate.Value,
                endDate.Value);


            var columns = new List<ReportColumn<CredentialingFileProcessingSummary>>
            {
                new() { Header = "Credentialing Specialist Name", Value = x => x.CSName },
                new() { Header = "Total Files Processed", Value = x => x.CleanFiles },
                new() { Header = "Total # of Flagged Files", Value = x => x.FlaggedFiles },
                new() { Header = "Total # of Clean Files", Value = x => x.TotalFilesProcessed },
                new() { Header = "Percent of Flagged Processed", Value = x => x.PercentCleaned },
                new() { Header = "Percent of Clean Processed", Value = x => x.PercentFlagged }
            };

            switch (format?.ToLower())
            {
                case "csv":
                    return GenerateCsv(result, "CredentialingSummary.csv");

                case "excel":
                    return GenerateExcel(result, "CredentialingSummary.xlsx");

                case "pdf":
                    return GeneratePdf(result, "CredentialingSummary.pdf", columns);

                default:
                    return BadRequest(new { message = "Invalid format" });
            }
        }

        private FileContentResult GenerateCsv<T>(IList<T> data, string fileName)
        {
            var sb = new StringBuilder();

            if (data == null || !data.Any())
                return File(Encoding.UTF8.GetBytes(string.Empty), "text/csv", fileName);

            var props = typeof(T).GetProperties();

            sb.AppendLine(string.Join(",", props.Select(p => p.Name)));

            foreach (var item in data)
            {
                var values = props.Select(p =>
                {
                    var val = p.GetValue(item, null);
                    return $"\"{val}\"";
                });

                sb.AppendLine(string.Join(",", values));
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());

            return File(bytes, "text/csv", fileName);
        }


        FileResult GeneratePdf<T>(
            IList<T> data,
            string fileName,
            List<ReportColumn<T>> columns)
        {
            var document = new PdfDocument();
            var page = document.AddPage();

            page.Orientation = PageOrientation.Landscape;

            var gfx = XGraphics.FromPdfPage(page);
            var tf = new XTextFormatter(gfx);
            var font = new XFont("OpenSans", 7);

            double margin = 40;
            double x = margin;
            double y = margin;

            double pageWidth = page.Width - (2 * margin);
            double colWidth = pageWidth / columns.Count;

            // ✅ HEADER HEIGHT
            double headerHeight = 0;
            foreach (var col in columns)
            {
                double h = GetTextHeight(gfx, col.Header, font, colWidth);
                headerHeight = Math.Max(headerHeight, h);
            }

            // ✅ DRAW HEADER
            double currentX = x;

            foreach (var col in columns)
            {
                var rect = new XRect(currentX, y, colWidth, headerHeight);

                gfx.DrawRectangle(XPens.LightGray, rect);

                tf.DrawString(col.Header, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                currentX += colWidth;
            }

            y += headerHeight;

            // ✅ DATA
            foreach (var item in data)
            {
                double rowHeight = 0;

                // ✅ Calculate dynamic height
                foreach (var col in columns)
                {
                    var text = col.Value(item)?.ToString() ?? "";
                    double h = GetTextHeight(gfx, text, font, colWidth);
                    rowHeight = Math.Max(rowHeight, h);
                }

                currentX = x;

                foreach (var col in columns)
                {
                    var text = col.Value(item)?.ToString() ?? "";

                    var rect = new XRect(currentX, y, colWidth, rowHeight);

                    gfx.DrawRectangle(XPens.LightGray, rect);

                    tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                    currentX += colWidth;
                }

                y += rowHeight;

                // ✅ PAGE BREAK
                if (y > page.Height - margin)
                {
                    page = document.AddPage();
                    page.Orientation = PageOrientation.Landscape;

                    gfx = XGraphics.FromPdfPage(page);
                    tf = new XTextFormatter(gfx);

                    y = margin;

                    // ✅ RE-DRAW HEADER
                    currentX = x;

                    foreach (var col in columns)
                    {
                        var rect = new XRect(currentX, y, colWidth, headerHeight);

                        gfx.DrawRectangle(XPens.LightGray, rect);
                        tf.DrawString(col.Header, font, XBrushes.Black, rect, XStringFormats.TopLeft);

                        currentX += colWidth;
                    }

                    y += headerHeight;
                }
            }

            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return File(stream.ToArray(), "application/pdf", fileName);
            }
        }

        double GetTextHeight(XGraphics gfx, string text, XFont font, double width)
        {
            var size = gfx.MeasureString(text, font);

            int lines = (int)Math.Ceiling(size.Width / width);

            return Math.Max(lines * size.Height, size.Height);
        }


        private FileResult GenerateExcel<T>(IList<T> data, string fileName)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                var props = typeof(T).GetProperties();

                // ✅ Header row
                for (int i = 0; i < props.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = props[i].Name;
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                }

                // ✅ Data rows
                for (int row = 0; row < data.Count; row++)
                {
                    for (int col = 0; col < props.Length; col++)
                    {
                        var value = props[col].GetValue(data[row]);
                        worksheet.Cell(row + 2, col + 1).Value = value?.ToString();
                    }
                }

                // ✅ Auto fit columns
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var bytes = stream.ToArray();

                    return File(
                        bytes,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        fileName
                    );
                }
            }
        }


    }
}
