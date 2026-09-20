using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_RedirectToCMCDashboardLeftMenu : System.Web.UI.Page
{
    public string RedirectToCMCDashboardUrl = string.Empty;
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        RedirectToCMCDashboardUrl = string.Format("~/Process/RedirectToCMCDashboard.aspx");
 
        string dashboardRedirectURL = string.Empty;

        RedirectToCMCDashboardHelper rt = new RedirectToCMCDashboardHelper();
        bool flag = rt.MakeCallToCMCRedirectAPI(string.Empty, ref dashboardRedirectURL);

        if (flag)
        {
            lblErrorMessage.Visible = false;
            Response.Redirect(dashboardRedirectURL);
        }
        else
        {
            lblErrorMessage.Visible = true;
        }
    }
}