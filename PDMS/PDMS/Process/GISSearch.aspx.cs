using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Process_GISSearch : System.Web.UI.Page
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
        //Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName") + "Provider Search (Public)";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                if (Helper.IsLoggedInUserInAdminRole())
                {
                    SessionVarRetriever.MyQueueSelectedRoleName =
                    SessionVarRetriever.MyQueueSelectedRoleValue =
                    SessionVarRetriever.UserIdSelected = null;
                }
                //this.Page.Title = (string)GetGlobalResourceObject("BrandingResource", "PortalName");
                BindDropDown();

                if (Helper.IsModern())
                {
                    btnSearch.CssClass = "buttonBoxFocus";
                }
            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        RefreshData();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ddlProviderType.SelectedIndex = 0;
        RefreshData();
    }

    public void RefreshData()
    {
        DataTable dt = GetData(100);
        RadMap1.DataSource = dt;
        RadMap1.DataBind();
    }

    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string providerTypeId = ddlProviderType.SelectedValue;
        DataSet ds = psc.SearchProvidersGIS(providerTypeId, pageSize);
        return ds.Tables[0];
    }

    private void BindDropDown()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectProviderTypesWithAbbrev(Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
		Helper.LoadDropDown(ddlProviderType, ds.Tables[0], "PROVIDER_TYPE_NAME", "MMIS_PROVIDER_TYPE_ID", true);
    }

    protected void RddOnProviderTypeSelected_ItemSelected(object sender, DropDownListEventArgs e)
    {
    }

}
