using Corp.Core.Libraries.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoldReports_ReportsEditReport : WorkflowPage
{
    public string ReportPath { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ReportPath = Request.QueryString["reportPath"];
        if (!IsPostBack)
        {
            string reportName = Request.QueryString["name"];

            if (string.IsNullOrEmpty(reportName))
            {
                reportName = "Report Management Editor";
            }
            SetBreadcrumb(BreadcrumbMap.Current("ReportEditor", reportName));
        }
    }
}