using Corp.Core.Libraries.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BoldReports_QueryDesigner : WorkflowPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetBreadcrumb(BreadcrumbMap.Current("ReportQueryDesigner"));
    }
}