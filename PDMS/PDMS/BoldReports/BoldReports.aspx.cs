using Corp.Core.Libraries.DataModels;
using Corp.Core.Libraries.Helper;
using Corp.Core.Libraries.ServiceAgent;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;

public partial class BoldReports_BoldReports : WorkflowPage
{
    private readonly BoldReportServiceAgent _reportService = new BoldReportServiceAgent();
    private readonly TokenServiceAgent _tokenService = new TokenServiceAgent();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadReport();
        }
    }

    private void LoadAllReports()
    {
        try
        {
            string userId = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            List<ReportModel> reports = _reportService.GetAllUserReports(userId);

            if (reports == null || reports.Count == 0)
            {
                ShowError("No reports available.");
                return;
            }

            gvReports.DataSource = reports;
            gvReports.DataBind();

            pnlReportList.Visible = true;
            pnlViewer.Visible = false;
        }
        catch (Exception ex)
        {
            ShowError("Error loading reports: " + ex.Message);
        }
    }

    protected void gvReports_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ViewReport")
        {
            string reportPath = e.CommandArgument as string;

            if (string.IsNullOrEmpty(reportPath))
            {
                ShowError("Invalid report path.");
                return;
            }

            Response.Redirect("~/MesCred/Reports/BoldReports.aspx?reportPath=" + Server.UrlEncode(reportPath));
        }
    }


    private void LoadReport()
    {
        try
        {

            if (!BoldReportsAppSettings.IsInitialized)
            {

                BoldReportsAppSettings.Initialize(
                    ConfigurationManager.AppSettings["BoldReports:BaseUrl"],
                    ConfigurationManager.AppSettings["BoldReports:SiteIdentifier"],
                    ConfigurationManager.AppSettings["BoldReports:ReportServiceUrl"],
                    ConfigurationManager.AppSettings["BoldReports:ReportServerUrl"],
                    ConfigurationManager.AppSettings["BoldReports:TokenEndpoint"],
                    ConfigurationManager.AppSettings["BoldReports:UserName"],
                    ConfigurationManager.AppSettings["BoldReports:Password"],
                    ConfigurationManager.AppSettings["BoldReports:EmbedSecret"]
                );
            }

            string reportPathParam = (Request.QueryString["reportPath"] ?? "").Trim();
            string reportIdParam = (Request.QueryString["reportId"] ?? "").Trim();

            if (string.IsNullOrEmpty(reportPathParam) && string.IsNullOrEmpty(reportIdParam))
            {
                pnlViewer.Visible = false;
                LoadAllReports();
                return;
            }

            // Force a fresh token for the viewer (not reusing a potentially stale cached token)
            _tokenService.InvalidateToken();
            string token = _tokenService.GenerateToken();

            if (string.IsNullOrEmpty(token))
            {
                ShowError("Failed to generate authentication token.");
                return;
            }

            ReportModel report = null;
            string resolvedReportPath;

            if (!string.IsNullOrEmpty(reportPathParam))
            {
                resolvedReportPath = HttpUtility.UrlDecode(reportPathParam);
            }
            else
            {
                int reportIdInt;
                if (int.TryParse(reportIdParam, out reportIdInt))
                {
                    report = _reportService.GetReportById(reportIdInt);
                }
                else
                {
                    report = _reportService.GetReportByGuid(reportIdParam);
                }

                if (report == null)
                {
                    ShowError("Report not found. The specified report ID does not exist or you do not have access.");
                    return;
                }

                if (string.IsNullOrEmpty(report.ReportPath))
                {
                    ShowError("Report path is missing for this report.");
                    return;
                }

                resolvedReportPath = report.ReportPath;
            }

            if (string.IsNullOrEmpty(resolvedReportPath))
            {
                ShowError("Report path could not be determined. Ensure the report has a valid path or GUID.");
                return;
            }

            // Validate that the URLs are properly configured
            string reportServiceUrl = BoldReportsAppSettings.ReportServiceUrl;
            string reportServerUrl = BoldReportsAppSettings.ReportServerUrl;

            if (string.IsNullOrEmpty(reportServiceUrl) || reportServiceUrl.Contains("your-server") || reportServiceUrl.Contains("your-boldreports"))
            {
                ShowError("BoldReports:ReportServiceUrl is not configured. Please update Web.config with your actual Bold Reports Server URL.");
                return;
            }

            // Pass values via hidden fields (no JavaScript encoding needed, avoids token corruption)
            hfReportPath.Value = resolvedReportPath;
            hfAuthToken.Value = token;
            hfReportServiceUrl.Value = reportServiceUrl;
            hfReportServerUrl.Value = reportServerUrl;

            string reportTitle = report != null ? report.ReportName : resolvedReportPath;
            litReportTitle.Text = Server.HtmlEncode(reportTitle);

            pnlViewer.Visible = true;
            pnlReportList.Visible = false;
            pnlViewerError.Visible = false;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ShowError(string message)
    {
        pnlViewerError.Visible = true;
        litViewerError.Text = Server.HtmlEncode(message);

        pnlViewer.Visible = false;
        pnlReportList.Visible = false; // ✅ ADD THIS
    }
}