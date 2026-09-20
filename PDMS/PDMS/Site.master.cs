using System;
using System.Web;
using System.Web.UI;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblUsername.Text = string.Empty;
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            lblUsername.Text = HttpContext.Current.User.Identity.Name + "&nbsp;&nbsp;|&nbsp;";
        }
        if (!Page.IsPostBack)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            String env = psc.GetEnvironment();
            lblEnvironment.Text = psc.GetUIHeader();
            mltEntrust.ActiveViewIndex = 0;                 // Default is Integration
            if (env.IndexOf("UAT") != -1) mltEntrust.ActiveViewIndex = 1;
            else if (env.IndexOf("Production") != -1)
            {
                mltEntrust.ActiveViewIndex = 2;
                pnlEnvironment.Visible = false;
            }
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        // Make sure the user is logged in before we disconnect them
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Abandon();
        }
        Response.Redirect(Helper.RedirectLoginURL());
    }
}
