using System;
using MAXIMUS.Core.Libraries;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Maintenance_User : System.Web.UI.Page
{
    private const int pageSize = 10;
    private static bool isTechAdmin = false;
    private int EditIndex
    {
        get { return (int)ViewState["EditIndex"]; }
        set { ViewState["EditIndex"] = value; }
    }

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
        if (!Page.IsPostBack)
        {
            UMS02.Attributes.Add("autocomplete", "off");
            UMS02.Text = string.Empty;
        }
        if (!IsPostBack)
        {
            isTechAdmin = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.TechAdminMAX);
            if (isTechAdmin)
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            EditIndex = -1;
            LoadData();
            Helper.LoadList(ddlRole, Roles.GetAllRoles(), string.Empty, string.Empty, true);
            pnlSearch.Visible = isTechAdmin;
            pnlSearchResults.Visible = isTechAdmin;

            if (AppSettings.Get("AllowNonIOPUser").ToString() == "true")
                btnAddNew.Visible = true;
        }

        if(Helper.IsModern())
        {
            UMS04.CssClass = "buttonBoxFocus";
        }
        //UMS04.Style["disabled"] = UMS02.Text.Length + UMS08.Text.Length + txtSearchOrgName.Text.Length > 0 && cboxSearchRole.SelectedIndex > 0 ? "true" : "false";
        ucTransferProvider.KeepPopupOpenEvent += new PopupControls_TransferOwnership.KeepPopupOpenEventHandler(ucTransferProvider_KeepPopupOpenEvent);
        ucTransferProvider.RefreshEvent += new PopupControls_TransferOwnership.RefreshEventHandler(ucTransferProvider_RefreshEvent);
    }

    public void ucTransferProvider_KeepPopupOpenEvent()
    {
        mltPopup.ActiveViewIndex = 0;
        mpe1.Show();
    }

    public void ucTransferProvider_RefreshEvent()
    {
        mltPopup.ActiveViewIndex = 0;
        mpe1.Hide();
        BindSearchResultsGrid(1);
    }

    private int counter = 0;
    protected string GetRowColor()
    {
        if (counter++ % 2 == 0) return "#fff";
        else return "#DDDDFF";
    }

    private DataTable GetData()
    {      
        DataTable dt = new DataTable();
        dt.Columns.Add(new System.Data.DataColumn("UserName", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("Email", typeof(String)));
        dt.Columns.Add(new System.Data.DataColumn("CreationDate", typeof(DateTime)));
        dt.Columns.Add(new System.Data.DataColumn("IsApproved", typeof(bool)));
        dt.Columns.Add(new System.Data.DataColumn("IsLockedOut", typeof(bool)));
        dt.Columns.Add(new System.Data.DataColumn("IsOnline", typeof(bool)));
        dt.Columns.Add(new System.Data.DataColumn("LastActivityDate", typeof(DateTime)));

        foreach (MembershipUser usr in Membership.GetAllUsers())
        {
            DataRow dr = dt.NewRow();
            dr["UserName"] = usr.UserName;
            dr["Email"] = usr.Email;
            dr["CreationDate"] = usr.CreationDate;
            dr["IsApproved"] = usr.IsApproved;
            dr["IsLockedOut"] = usr.IsLockedOut;
            dr["IsOnline"] = usr.IsOnline;
            dr["LastActivityDate"] = usr.LastActivityDate;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private void LoadData()
    {
        
        cboxSearchRole.Items.Add(new ListItem("All", "All"));
        List<ListItem> roles = Helper.GetAllUserRoles();  // Jira 2432
        foreach (ListItem role in roles)
        {
            cboxSearchRole.Items.Add(role);
        }

    }

    protected void grdFilteredUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "user")
        {
            int i;
            if (!int.TryParse(e.CommandArgument.ToString(), out i))
                return;


            Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + grdFilteredUsers.DataKeys[i].Value.ToString());
        }
        if (e.CommandName == "transfer")
        {
            int rowIndex;
            if (!int.TryParse(e.CommandArgument.ToString(), out rowIndex))
                return;
            hidIndex.Value = rowIndex.ToString();
            if (!string.IsNullOrEmpty(hidIndex.Value) && !string.IsNullOrEmpty(grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)]["reg_id"].ToString()))
            {
                int regid = Convert.ToInt32(grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)]["reg_id"]);
                string user = grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)]["USER_NAME"].ToString();
                string medicaidID = string.IsNullOrEmpty(grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)]["Medicaid_id"].ToString()) ? "" : grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)]["Medicaid_id"].ToString();
                ucTransferProvider.LoadData(regid,medicaidID, user);
                mltPopup.ActiveViewIndex = 0;
                mpe1.Show();
            }
            else
                MessageBox2.Show("There is no Registration associated to the selected user,  please choose another user.", "Error");
        }
    }

    protected void grdFilteredUsers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        string userName = DataBinder.Eval(e.Row.DataItem, "USER_NAME").ToString();
        bool isOHID = DataBinder.Eval(e.Row.DataItem, "IsOHID").ToString() == "Yes" ? true : false;

        LinkButton lnkContact = (LinkButton)e.Row.FindControl("lbtnContact");
        LinkButton lnkUser = (LinkButton)e.Row.FindControl("lbtnUser");
        LinkButton lnkGroup = (LinkButton)e.Row.FindControl("lbtnGroup");
        //LinkButton lnkMedID = (LinkButton)e.Row.FindControl("lbtnMedicaidID");
        LinkButton lnkNPI = (LinkButton)e.Row.FindControl("lbtnNPI");
        LinkButton lnkTaxID = (LinkButton)e.Row.FindControl("lbtnTaxId");
        LinkButton lnkActive = (LinkButton)e.Row.FindControl("lbtActive");
        LinkButton lnkLocked = (LinkButton)e.Row.FindControl("lbtLocked");

    }

    private string GetValue(Control ctl, string id)
    {
        if (ctl.FindControl(id).GetType().Name.Equals("TextBox"))
        {
            TextBox txt = ctl.FindControl(id) as TextBox;
            if (txt == null) return string.Empty;
            return txt.Text;
        }
        else if (ctl.FindControl(id).GetType().Name.Equals("RadioButtonList"))
        {
            RadioButtonList rbl = ctl.FindControl(id) as RadioButtonList;
            if (rbl == null) return string.Empty;
            return rbl.SelectedValue;
        }
        return string.Empty;
    }

    private void AddRole(string username, string newRole)
    {
        // Remove all roles from user
        string[] roles = Roles.GetRolesForUser(username);
        foreach (string role in roles)
        {
            Roles.RemoveUserFromRole(username, role);
        }
        Roles.AddUserToRole(username, newRole);
    }

    private void SaveData()
    {
        try
        {
            MembershipProvider mp = System.Web.Security.Membership.Providers["AspNetSqlMembershipProvider"];
            if (Helper.GetUserId(txtUserName.Text) == Guid.Empty)
            {
                MembershipCreateStatus mcs;
                Guid provKey = Guid.NewGuid();
                mp.CreateUser(txtUserName.Text, txtPassword.Text, txtEmail.Text, null, null, true, provKey, out mcs);
                if (mcs != MembershipCreateStatus.Success)
                    throw new Exception("Create User problems: " + mcs.ToString());
            }
            MembershipUser usr = mp.GetUser(txtUserName.Text, false);
            usr.Email = txtEmail.Text;
            usr.IsApproved = Convert.ToBoolean(Helper.GetRadioButtonListSelect(rblIsApproved, 2));
            if (usr.IsApproved) usr.UnlockUser();
            mp.UpdateUser(usr);
            AddRole(usr.UserName, ddlRole.SelectedItem.Text);
            LoadData();
        }
        catch (Exception ex)
        {
            MessageBox2.Show(ex.Message, "Error");
            mpeUser.Show();
        }
    }

    private void LoadUser(string username)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + username);
    }

    protected void lnkEdit_Click(object sender, CommandEventArgs e)
    {
        if (("EditRow").Equals(e.CommandName))
        {
            string username = e.CommandArgument.ToString();
            SessionVarRetriever.UserIdSelected = Helper.GetUserId(username).ToString();
            EditIndex = 0;
            pnlCreate.Visible = false;
            LoadUser(username);
        }
    }

    private bool ValidateData()
    {
        string errMsg = string.Empty;

        if (pnlCreate.Visible)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim())) errMsg += "Enter Username<br>";
            if (string.IsNullOrEmpty(txtPassword.Text.Trim())) errMsg += "Enter Password<br>";
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim())) errMsg += "Enter Confirm Password<br>";
            if (txtPassword.Text != txtConfirmPassword.Text) errMsg += "Password and Confirm Password must match<br>";
        }

        if (string.IsNullOrEmpty(txtEmail.Text.Trim())) errMsg += "Enter Email Address<br>";
        else if (!System.Text.RegularExpressions.Regex.IsMatch(txtEmail.Text, "^[A-Z'a-z0-9._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}$"))
            errMsg += "Enter valid Email Address<br>";
        if (rblIsApproved.SelectedIndex == -1) errMsg += "Select is Approved<br>";
        if (ddlRole.SelectedIndex <= 0) errMsg += "Select a Role for the User<br>";

        if (!string.IsNullOrEmpty(errMsg))
        {
            MessageBox2.Show(errMsg, "Error");
            mpeUser.Show();
            return false;
        }
        return true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!ValidateData()) return;
        SaveData();
        LoadData();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx");
    }

    protected void UMS04_Click(object sender, EventArgs e)
    {
        BindSearchResultsGrid(1);
    }

    private void BindSearchResultsGrid(int toPageNumber)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        string role = cboxSearchRole.SelectedIndex == 0 ? string.Empty : cboxSearchRole.SelectedItem.Value; // Jira 2432 
        DataSet ds = svc.GetFilteredUsers(UMS02.Text, UMS08.Text, txtSearchOrgName.Text, txtSearchTaxId.Text, txtSearchNPI.Text, role, grdFilteredUsers.PageSize, grdFilteredUsers.CurrentRowIndex);
        int searchResultsTotalRows = Helper.GetInt("TOTAL", ds.Tables[1].Rows[0]);
       // BindPager(searchResultsTotalRows, toPageNumber);
        grdFilteredUsers.DataSource = ds.Tables[0];
        grdFilteredUsers.VirtualItemCount = searchResultsTotalRows;
        grdFilteredUsers.DataBind();
        grdFilteredUsers.PageIndexCount = grdFilteredUsers.PageCount;
        //btnTransferReg.Enabled = false;
        //pnlSearchResults.Update();
    }
    protected void grdFilteredUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // RefreshData();
        BindSearchResultsGrid(e.NewPageIndex);
    }
    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            BindSearchResultsGrid(Convert.ToInt32(e.CommandArgument));
        }
    }

    private void BindPager(int totalRows, int currentPage)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalRows / pageSize);
        List<ListItem> pagerContainer = new List<ListItem>();
        for (int i = 1; i <= totalPages; i++)
        {
            pagerContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage == i ? false : true));
        }

        if(pagerContainer.Count > 0) pagerContainer[0].Text = "First";
        if(pagerContainer.Count > 1) pagerContainer[totalPages - 1].Text = "Last";
        dlPager.DataSource = pagerContainer;
        dlPager.DataBind();
    }


    //protected void btnTransfer_Click(object sender, EventArgs e)
    //{
    //    int regId = Convert.ToInt32(grdFilteredUsers.DataKeys[Convert.ToInt32(hidIndex.Value)].Values["reg_id"]);
    //    bool val = ucTransferProvider.SaveData(regId);
    //    if (val)
    //    {
    //        mpe1.Hide();
    //        UMS04_Click(new object(), new EventArgs());
    //    }
    //    else
    //        mpe1.Show();
    //}

    protected void btnIOPUser_Click(object sender, EventArgs e)
    {
        Response.Redirect(@"~/Process/AdminUserAccounts.aspx?addIOPUser=true");
    }

    //OHPNM-15917:Adding clear button on User Maintenance
    protected void btnClear_Click(object sender, EventArgs e)
    {
        UMS02.Text = "";
        UMS08.Text = "";
        txtSearchTaxId.Text = "";
        txtSearchOrgName.Text = "";
        txtSearchNPI.Text = "";
        cboxSearchRole.SelectedIndex = 0;
    }
}