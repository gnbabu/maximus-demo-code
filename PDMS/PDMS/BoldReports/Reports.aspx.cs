using Corp.Core.Libraries.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static Main_MasterPage;

public partial class BoldReports_Reports : WorkflowPage
{
    public HelpData GetHelpFromDb(string pageName)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        DataSet ds = psc.SelectPageConfigurationsByPageName(
            pageName,
            Helper.GetUserId(HttpContext.Current.User.Identity.Name)
        );

        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            return null;

        DataRow dr = ds.Tables[0].Rows[0];

        string pdfFileName = Helper.GetString("pdfurl", dr);

        string url = null;

        if (!string.IsNullOrEmpty(pdfFileName))
        {
            url = "/MES_CRED/pages/ShowFiles.aspx?mode=inline&FileName=" + pdfFileName;
        }

        return new HelpData
        {
            Mode = Helper.GetString("mode", dr),
            Title = Helper.GetString("title", dr),
            Content = Helper.GetString("displaytext", dr),
            PdfUrl = url   // ✅ FIX: return URL, not filename
        };
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetBreadcrumb(BreadcrumbMap.Current("Reports"));


        if (!IsPostBack)
            Page.DataBind();

    }
}