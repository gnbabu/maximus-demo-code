using System;
using System.Web.UI;

public partial class Process_ContactUs : System.Web.UI.Page
{
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
        if (!IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
        }
        // SessionVarRetriever.RegistrationId = 0;
        this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName") + "- Contact Us";
        //lnkStateEmail.NavigateUrl = "mailto:" + AppSettings.Get("PDMSEMAIL");
        //lnkStateEmail.Text = AppSettings.Get("PDMSEMAIL");
    }
}