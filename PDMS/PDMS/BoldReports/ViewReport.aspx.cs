using Corp.Core.Libraries.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoldReports_ReportsViewReport : WorkflowPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string reportName = Request.QueryString["name"];

            if (string.IsNullOrEmpty(reportName))
            {
                reportName = "Report Management Viewer";
            }
            SetBreadcrumb(BreadcrumbMap.Current("ReportViewer", reportName));
        }
    }
}