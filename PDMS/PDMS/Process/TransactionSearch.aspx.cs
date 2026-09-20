using System;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using Microsoft.Web.Services3.Referral;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_TransactionSearch : RegistrationProvider
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
        //Set the title same as setTitle function to avoid 508 complaint
        // Page.Title = Resources.BrandingResource.OWNER_SEARCH_TITLE;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser))
        {
            RadPageView2.Visible = true;
            TransactionMonitorPage.Visible = true;
            PNMDataUpdatesPage.Visible = true;
            DataLookupPage.Visible = true;

        }
        else
        {
            RadPageView2.Visible = false;
            TransactionMonitorPage.Visible = false;
            PNMDataUpdatesPage.Visible = false;
            DataLookupPage.Visible = false;

            var mqLoggingTab = RadTabStrip1.FindTabByText("MQ Logging");
            if (mqLoggingTab != null)
            {
                RadTabStrip1.Tabs.Remove(mqLoggingTab);
            }


            var transactionMonitorTab = RadTabStrip1.FindTabByText("Transaction Monitoring");
            if (transactionMonitorTab != null)
            {
                RadTabStrip1.Tabs.Remove(transactionMonitorTab);
            }

            var pnmUpdatesTab = RadTabStrip1.FindTabByText("Data Fix");
            if (pnmUpdatesTab != null)
            {
                RadTabStrip1.Tabs.Remove(pnmUpdatesTab);
            }

            var dataLookupTab = RadTabStrip1.FindTabByText("Data Lookup");
            if (dataLookupTab != null)
            {
                RadTabStrip1.Tabs.Remove(dataLookupTab);
            }

        }
    }

    protected void btnReturnToGroupAffiliations_Click(object sender, EventArgs e)
    {
        // TODO: EDV What functionality is this??
        //Response.Redirect("Registration.aspx?Step=4");
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        //SessionVarRetriever.ReturnToDashboard = false;
        //Response.Redirect("~/Default.aspx");
    }


}