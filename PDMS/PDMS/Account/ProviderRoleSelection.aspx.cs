using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_ProviderRoleSelection : System.Web.UI.Page
{
    private DataTable dtprov;
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
        foreach (ListItem li in rblProviderRole.Items)
        {
            if (li.Text == "CEO Certified (DODD)")
                li.Attributes.Add("title", "Select this role to apply to become a DODD Agency Provider, Independent Provider, Licensee, or Operator.");
            if (li.Text == "Secondary User (DODD)")
                li.Attributes.Add("title", "Select this role to gain access to an existing Provider, Licensee, or Operator contract, or to gain access to an existing Facility.");
        }
    }
    protected void btnSaveAcc_Click(object sender, EventArgs e)     //JIRA OHPNM-5474 - Commented the code to disable PIN notification for PNM
    {
        if (!Page.IsValid) return;

        if (rblProviderRole.SelectedIndex != -1)
        {
            hdnRoleName.Value = rblProviderRole.SelectedValue;
            string roleName = rblProviderRole.SelectedValue == "DODDAgent" ? "ProviderAgent" : rblProviderRole.SelectedValue == "CEOCertified" ? "ProviderAdministrator" : rblProviderRole.SelectedValue;

            //Roles.AddUserToRole(HttpContext.Current.User.Identity.Name, roleName);
            MembershipUser user = Membership.GetUser(HttpContext.Current.User.Identity.Name);
            Membership.UpdateUser(user);
            if (rblProviderRole.SelectedValue == "DODDAgent")
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.UpdateUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), HttpContext.Current.User.Identity.Name, null, null, null, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false, "DODD Agent");
            }
            if (rblProviderRole.SelectedValue == "CEOCertified")
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.UpdateUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), HttpContext.Current.User.Identity.Name, null, null, null, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false, "DODD Admin");
            }

			//OHPNM-11445 Adding User Roles for aspnet_UsersInRoles 			
			Guid userid = Helper.GetUserId(user.UserName);
			string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

			List<SqlParameter> parameters = new List<SqlParameter>();
			parameters.Add(SqlParms.CreateParameter("Userid", DbType.Guid, userid, false));
			parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, loggedInUserId, false));
			parameters.Add(SqlParms.CreateParameter("Rolename", DbType.String, roleName, false));
			DataAccess.ExecuteStoredProcedure("aspnet_Addusersinroleselection_Custom", parameters);

			pnlRoleSelect.Visible = false;
            btnSaveAcc.Visible = false;
            Response.Redirect("~/Default.aspx", false);
        }
        else
        {
            UNR11_ERR.Text = "* Select a Role";
            UNR11_ERR.Visible = true;
            return;
        }
    }
}