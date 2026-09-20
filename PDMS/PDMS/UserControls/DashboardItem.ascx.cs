using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_DashboardItem : System.Web.UI.UserControl
{
    public string DashboardLabel
    {
        get
        {
            if (ViewState["DashboardLabel"] == null) ViewState["DashboardLabel"] = string.Empty;
            return ViewState["DashboardLabel"].ToString();
        }
        set { ViewState["DashboardLabel"] = value; }
    }

    public int TableId
    {
        get
        {
            if (ViewState["TableId"] == null) ViewState["TableId"] = 0;
            return Convert.ToInt32(ViewState["TableId"]);
        }
        set { ViewState["TableId"] = value; }
    }

    public int StatusID
    {
        get
        {
            if (ViewState["StatusID"] == null) ViewState["StatusID"] = 0;
            return Convert.ToInt32(ViewState["StatusID"]);
        }
        set { ViewState["StatusID"] = value; }
    }

    public int Ordinal
    {
        get
        {
            if (ViewState["Ordinal"] == null) ViewState["Ordinal"] = 0;
            return Convert.ToInt32(ViewState["Ordinal"]);
        }
        set { ViewState["Ordinal"] = value; }
    }

    public int Quantity
    {
        get
        {
            if (ViewState["Quantity"] == null) ViewState["Quantity"] = 0;
            return Convert.ToInt32(ViewState["Quantity"]);
        }
        set
        {
            mltDashboardItem.ActiveViewIndex = (value > 0 ? 1 : 0);
            if (value > 0) lnkQuantity.Text = lblQuantityForNonAdmin.Text = value.ToString(); 
            else lblQuantity.Text = value.ToString();
            ViewState["Quantity"] = value;
            lnkQuantity.Visible = Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Operator)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.StateAdministrator)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.DBHOperatorRole)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.LTCWorkerRole)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.DDSOperatorRole)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.SiteVisitOperatorRole)
                                    || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.ProviderRoles);
            lblQuantityForNonAdmin.Visible = !lnkQuantity.Visible;
        }
    }

    public Boolean IsAssigned
    {
        get
        {
            if (ViewState["IsAssigned"] == null) ViewState["IsAssigned"] = 0;
            return Convert.ToBoolean(ViewState["IsAssigned"]);
        }
        set { ViewState["IsAssigned"] = value; }
    }

    protected void lnkQuantity_Click(object sender, EventArgs e)
    {
        string url = string.Empty;
        if (TableId >= CON.DashboardTableType.SendToWaiverSearch)
        {
            url = "~/Process/WaiverProviderSearch.aspx?TableId=" + TableId.ToString() + "&StatusID=" + StatusID.ToString() + "&Ordinal=" +
                Ordinal.ToString() + "&DashboardLabel=" + DashboardLabel + "&IsAssigned=" + IsAssigned;
            SessionVarRetriever.ReturnToDashboard = true;
        }
        else
        {
            url = "~/Process/GroupReview.aspx?TableId=" + TableId.ToString() + "&StatusID=" + StatusID.ToString() + "&Ordinal=" +
                Ordinal.ToString() + "&DashboardLabel=" + DashboardLabel + "&IsAssigned=" + IsAssigned;
            SessionVarRetriever.ReturnToDashboard = true;
        }
        LinkButton lnk = (LinkButton)sender;
        if (lnk != null) url += "&Count=" + lnk.Text;
        Response.Redirect(url);
    }
}