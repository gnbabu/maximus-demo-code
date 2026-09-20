using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS.MembershipProvider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_AdminUserAccounts : System.Web.UI.Page
{
    private enum CurrentPage { User, Permissions };
    private static string AllowAdminCreateUser = string.Empty;
    private static bool isTechAdmin = false;

    #region Events


    protected override void OnInit(EventArgs e)
    {
        ProviderTypeFilter.DropDownControl.AutoPostBack = true;
        ProviderTypeFilter.DropDownControl.SelectedIndexChanged += new EventHandler(ProviderTypeFilter_SelectedIndexChanged);
        base.OnInit(e);
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
        Page.Title = "User Accounts";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            isTechAdmin = Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.TechAdminMAX);
            if (isTechAdmin)
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            AllowAdminCreateUser = AppSettings.Get("AllowAdminAddnonIOPUser").ToString();      // Get the appsetting key to Allow ODM State Administrator to create non IOP users in PNM.
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            AddOrEdit.Text = "Add";
            #region Initialize

            InitializeUserRoleSelector();
            BindProviderTypes(svc, string.Empty);
            BindWorkflows(svc);
            SetupProviderTypeFilter(svc);

            #endregion            

            if (!string.IsNullOrEmpty(Request.QueryString["addIOPUser"]) && Request.QueryString["addIOPUser"] == "true")
                AllowAdminCreateUser = "false";

            Registration.SecurityQuestions(ref ddlPasswordQuestion1);
            Registration.SecurityQuestions(ref ddlPasswordQuestion2);

            string username = Request.QueryString["username"];
            EditUserName.Text = username;

            if (!isTechAdmin && !username.Equals(HttpContext.Current.User.Identity.Name))
            {
                Response.Redirect(Helper.RedirectLoginURL());
            }

            if (!string.IsNullOrEmpty(username))
            {
                Guid userid = Helper.GetUserId(username);
                if (!userid.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                {
                    AddOrEdit.Text = "Edit";
                    RequirePassword.Enabled = false;
                    SetupProfile(username, userid);
                }
                MembershipUser user = Membership.GetUser(username);
                if (user == null)
                {
                    btnDelete.Visible = false;
                }
                IsUserActive(username);
            }

            if (!string.IsNullOrEmpty(UserName.Text))
            {
                MembershipUser user = Membership.GetUser(UserName.Text);
                if (user == null)
                {
                    btnUnlockUser.Visible = false;
                }
                else
                {
                    btnUnlockUser.Visible = user.IsLockedOut && isTechAdmin && user.IsApproved;
                }
            }

            //IsDeactivatedProvider(username);
            if (AddOrEdit.Text == "Add")
            {
                if (AllowAdminCreateUser == "true")      // Allow ODM administrator to create new non IOP users for testing purpose
                {
                    pnlCreateUser.Visible = true;
                    pnlPermissions.Visible = false;
                    pnlStepControl.Visible = false;
                    pnlConfirmation.Visible = false;
                    pnlChangesSaved.Visible = false;
                    pnlProviderTypes.Visible = false;
                    pnlQueueAssignment.Visible = false;
                    txtUserName.Enabled = false;
                    txtIOPEmail.Enabled = false;
                    txtOHID.Enabled = false;
                    txtIOPEmail.Visible = false;
                    lblIOPEmail.Visible = false;
                    txtOHID.Visible = false;
                    lblOHID.Visible = false;
                    hdnIsOHid.Value = "False";
                }
                else                                        // Allow ODM administrator to only add new Internal Users with OH|ID and assign them roles.
                {
                    ucStep1.Selected = false;
                    ucStep1.Text = "User Maintenance Search";
                    ucStepPermissions.Selected = true;
                    pnlPermissions.Visible = true;
                    pnlConfirmation.Visible = false;
                    pnlChangesSaved.Visible = false;
                    pnlProviderTypes.Visible = true;
                    pnlQueueAssignment.Visible = false;
                    pnlCreateUser.Visible = false;
                    txtUserName.Enabled = true;
                    txtIOPEmail.Visible = true;
                    txtIOPEmail.Visible = true;
                    lblIOPEmail.Visible = true;
                    txtOHID.Visible = true;
                    lblOHID.Visible = true;
                    txtOHID.Enabled = true;
                    hdnIsOHid.Value = "True";
                    btnPrevBtm.Visible = false;
                    btnPrevTop.Visible = false;
                }

            }
            else
            {
                txtUserName.Enabled = false;
                // OHPNM-15301 allow update of email address in the case IOP email changed
                txtIOPEmail.Enabled = true;
                txtIOPContactName.Enabled = true;
                txtOHID.Enabled = false;
                txtUserName.Text = UserName.Text;
                if (hdnIsOHid.Value == "True")
                {
                    ucStep1.Selected = false;
                    ucStep1.Text = "User Maintenance Search";
                    ucStepPermissions.Selected = true;
                    pnlPermissions.Visible = true;
                    pnlConfirmation.Visible = false;
                    pnlChangesSaved.Visible = false;
                    pnlProviderTypes.Visible = true;
                    pnlQueueAssignment.Visible = false;
                    pnlCreateUser.Visible = false;
                    btnPrevBtm.Visible = false;
                    btnPrevTop.Visible = false;
                    txtIOPEmail.Visible = true;
                    lblIOPEmail.Visible = true;
                    txtOHID.Visible = true;
                    lblOHID.Visible = true;
                }
                else      // For Converted Users
                {
                    pnlStepControl.Visible = false;
                    pnlPermissions.Visible = false;
                    pnlConfirmation.Visible = false;
                    pnlChangesSaved.Visible = false;
                    pnlProviderTypes.Visible = false;
                    pnlQueueAssignment.Visible = false;
                    pnlCreateUser.Visible = true;
                    txtIOPEmail.Visible = false;
                    lblIOPEmail.Visible = false;
                    txtOHID.Visible = false;
                    lblOHID.Visible = false;
                }
            }

            btnPrevTop.Visible = btnPrevBtm.Visible = pnlCreateUser.Visible;
            btnSaveTopUser.Visible = btnSaveBtmUser.Visible = pnlCreateUser.Visible;
            btnSaveTopPerm.Visible = btnSaveBtmPerm.Visible = !pnlCreateUser.Visible;
        }
        else
        {
            if (!string.IsNullOrEmpty(UserName.Text))
                txtUserName.Text = UserName.Text;
        }
        mmsUserRoles.OnItemSelected = this.UpdateSelectableUserRoles;
        mmsUserRoles.OnItemRemoved = this.UpdateSelectableUserRoles;
        mmsProviderTypes.OnItemSelected = this.UpdateSelectableProviderTypes;
        mmsProviderTypes.OnItemRemoved = this.UpdateSelectableProviderTypes;

        if (isTechAdmin)
        {
            DisplayHideOldPasswordFields(false);
        }
        SetEntries(upUser);
    }

    protected void UpdateSelectableProviderTypes(ListItem ptItem)
    {
        foreach (ListItem li in mmsProviderTypes.SourceListBox.Items)
        {
            li.Enabled = true;
        }
        ListItem[] liList = new ListItem[mmsProviderTypes.SelectedListBox.Items.Count];
        mmsProviderTypes.SelectedListBox.Items.CopyTo(liList, 0);

        foreach (ListItem liSelected in mmsProviderTypes.SelectedListBox.Items)
        {
            foreach (ListItem liSource in mmsProviderTypes.SourceListBox.Items)
            {
                liSource.Enabled = true;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // OHPNM-14877 -- clicking save does not update altered Contact name or Email address
            List<SqlParameter> userParameters = new List<SqlParameter>();
            userParameters.Add(SqlParms.CreateParameter("username", DbType.String, UserName.Text, true));
            userParameters.Add(SqlParms.CreateParameter("NewContactName", DbType.String, txtIOPContactName.Text, true));
            userParameters.Add(SqlParms.CreateParameter("NewEmailAddress", DbType.String, txtIOPEmail.Text, true));
            DataAccess.ExecuteStoredProcedure("usp_UpdateContactNameAndEmail", userParameters);

            Password.Attributes.Add("value", Password.Text);
            OldPassword.Attributes.Add("value", OldPassword.Text.Trim());

            Page.Validate("AdminUserAccounts");

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v.ValidationGroup.Equals("AdminUserAccounts") && !v.IsValid)
                        return;
                }
                catch
                {
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(EditUserName.Text))
            {
                if (Helper.IsUserInProviderRoles(EditUserName.Text) || !isTechAdmin)
                {
                    EditUser();
                    lblModal.Text = "Account Update Successful!";
                    btnModalOk.Visible = false;
                    btnModalCancel.Visible = true;
                    btnModalCancel.Text = "OK";
                    mpeChangesSaved.Show();
                }
                else
                {
                    SetPageVisible(CurrentPage.Permissions);
                }
            }
            else
            {
                SetPageVisible(CurrentPage.Permissions);
            }

            // Appove the user
            MembershipUser user = Membership.GetUser(UserName.Text);
            if (user != null)
            {
                if (rblIsApproved.SelectedIndex != -1)
                {
                    user.IsApproved = rblIsApproved.SelectedIndex == 0;
                    Membership.UpdateUser(user);

					//OHPNM-11445 Updating last_modifed_user & last_modified_Date_time for aspnet_Users and aspnet_membership 
					string username = UserName.Text;
					PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
					//MembershipUser user = Membership.GetUser(username);
					Guid userid = Helper.GetUserId(user.UserName);
					string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

					List<SqlParameter> parameters = new List<SqlParameter>();
					parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, username, false));
					parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, loggedInUserId, false));
					DataAccess.ExecuteStoredProcedure("aspnet_Users_UpdateUser_Membership_Custom", parameters);
				}
            }
        }
        catch (PasswordException ex)
        {
            if (ex.Message == "PasswordExists")
            {
                PasswordExists.Visible = true;
                PasswordExists.Text = "Passwords used in the past 12 months cannot be used.";
            }
            if (ex.Message == "PasswordMatchFailed")
            {
                PasswordExists.Visible = true;
                PasswordExists.Text = "Password match failed!!";
            }

        }

    }

    protected void btnSubmitPermissions_Click(object sender, EventArgs e)
    {
        Page.Validate("AdminUserAccounts");
        if (!Page.IsValid)
            return;

        if (mmsUserRoles.SelectedListBox.Items.Count == 0)
        {
            pnlValidationSummary.Visible = true;
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "Please Select a User Role";
            upValidationSummary.Update();
            return;
        }

        if(hdnIsOHid.Value == "True")
        {
            if(txtOHID.Visible && txtOHID.Enabled && string.IsNullOrEmpty(txtOHID.Text))
            {
                pnlValidationSummary.Visible = true;
                URP03_ERR.Visible = true;
                URP03_ERR.Text = "Please Enter OH|ID (SOUID)";
                upValidationSummary.Update();
                return;
            }

            if (txtIOPEmail.Visible && txtIOPEmail.Enabled)
            {
                if (string.IsNullOrEmpty(txtIOPEmail.Text) || txtIOPEmail.Text.Length <= 0)
                {
                    pnlValidationSummary.Visible = true;
                    URP03_ERR.Visible = true;
                    URP03_ERR.Text = "Please enter Email used during creating the OH|ID";
                    upValidationSummary.Update();
                    return;
                }
                else if (!Regex.IsMatch(txtIOPEmail.Text, @"^([0-9a-zA-Z+]([-.\w]*[0-9a-zA-Z+])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"))
                {
                    pnlValidationSummary.Visible = true;
                    URP03_ERR.Visible = true;
                    URP03_ERR.Text = "Please enter a valid Email used during creating the OH|ID";
                    upValidationSummary.Update();
                    return;
                }
            }            
        }

        if (mmsUserRoles.SelectedListBox.Items.Count != 0)
        {
            var isProviderAdminSelected = mmsUserRoles.SelectedListBox.Items.Cast<ListItem>().Any(p => p.Text == "Provider Administrator"); // Jira 2432
            List<ListItem> allRoles = Helper.GetAllUserRoles(); // Jira 2432
            foreach (ListItem liSelected in mmsUserRoles.SelectedListBox.Items)
            {
                ListItem thisRole = allRoles.Where(r => r.Text == liSelected.Text).FirstOrDefault(); // Jira 2432

                if (thisRole.Value == "ProviderAgent" && mmsUserRoles.SelectedListBox.Items.Count > 1)
                {
                    pnlValidationSummary.Visible = true;
                    URP03_ERR.Visible = true;
                    URP03_ERR.Text = "ProviderAgent role cannot be selected with other roles.";
                    upValidationSummary.Update();
                    return;
                }
                if (thisRole.Value == "ProviderAdministrator" && mmsUserRoles.SelectedListBox.Items.Count > 2)
                {
                    pnlValidationSummary.Visible = true;
                    URP03_ERR.Visible = true;
                    URP03_ERR.Text = "Provider role  can only be selected with DeemedPresumptive roles.";
                    upValidationSummary.Update();
                    return;
                }
                else if (isProviderAdminSelected && (mmsUserRoles.SelectedListBox.Items.Count == 2 && thisRole.Value != "ProviderAdministrator")
                                && (mmsUserRoles.SelectedListBox.Items.Count == 2 && thisRole.Value != "DeemedPresumptive"))
                {
                    pnlValidationSummary.Visible = true;
                    URP03_ERR.Visible = true;
                    URP03_ERR.Text = "Administrator role can only be selected with DeemedPresumptive roles.";
                    upValidationSummary.Update();
                    return;
                }
            }
        }

        UserName.Text = txtUserName.Text;
        DataSet dsUAI = new DataSet();

        MembershipUser user = null;
        user = Membership.GetUser(txtUserName.Text);
        if (user != null && AddOrEdit.Text.Equals("Add"))
        {
            pnlValidationSummary.Visible = true;
            URP03_ERR.Visible = true;
            URP03_ERR.Text = "User already exists with this username.";
            upValidationSummary.Update();
            return;
        }

        if (AddOrEdit.Text.Equals("Add"))
        {
            AddUser();
            AddPermissions();
            lblModal.Text = "Account Creation Successful!";
            if(hdnIsOHid.Value == "True")
            {
                pnlValidationSummary.Visible = false;
                URP03_ERR.Visible = false;
                PasswordExists.Visible = false;
            }
        }
        else if (AddOrEdit.Text.Equals("Edit"))
        {
            if (hdnIsOHid.Value == "True")
            {
                // OHPNM-15301 -- allow update of Contact Name / Email Address for IOP
                EditIOPUserContact();
            }
            else
            {
                EditUser();
            }
            EditPermissions();

            pnlValidationSummary.Visible = false;
            URP03_ERR.Visible = false;
            PasswordExists.Visible = false;
            lblModal.Text = "Account Update Successful!";
        }

        upValidationSummary.Update();

        btnModalOk.Visible = false;
        btnModalCancel.Visible = true;
        btnModalCancel.Text = "OK";
        mpeChangesSaved.Show();
        AddOrEdit.Text = "Edit";
        PasswordExists.Visible = false;

        // To Do RG -Uncomment this below code later to hide the Create User panel and only show Permissions
        //if (AddOrEdit.Text.Equals("Edit") && hdnIsOHid.Value == "True" && !Helper.IsUserInProviderRoles(UserName.Text))
        //{
        //    EditPermissions();
        //}

        //pnlValidationSummary.Visible = false;
        //URP03_ERR.Visible = false;
        //PasswordExists.Visible = false;
        //lblModal.Text = "Account Update Successful!";

        //upValidationSummary.Update();

        //btnModalOk.Visible = false;
        //btnModalCancel.Visible = true;
        //btnModalCancel.Text = "OK";
        //mpeChangesSaved.Show();
        //AddOrEdit.Text = "Edit";
        //PasswordExists.Visible = false;
    }

    protected void UpdateSelectableUserRoles(ListItem roleItem)
    {


        foreach (ListItem li in mmsUserRoles.SourceListBox.Items)
        {
            li.Enabled = true;
        }
        ListItem[] liList = new ListItem[mmsUserRoles.SelectedListBox.Items.Count];
        mmsUserRoles.SelectedListBox.Items.CopyTo(liList, 0);

        if (mmsUserRoles.SelectedListBox.Items.Count > 0)  // Jira 2432
        {
            bool isAgent = false;
            bool isProvAdmin = false;
            bool isDeemed = false;
            foreach(ListItem liSelect in mmsUserRoles.SelectedListBox.Items)
            {
                if(liSelect.Value == "ProviderAgent")
                {
                    isAgent = true;
                    break;
                }
                if (liSelect.Value == "ProviderAdministrator")
                {
                    isProvAdmin = true;
                    break;
                }
                if (liSelect.Value == "DeemedPresumptive")
                {
                    isDeemed = true;
                    break;
                }
            }
            if (isAgent)
            {
                foreach (ListItem liSource in mmsUserRoles.SourceListBox.Items)
                {
                    liSource.Enabled = false;
                }
            }
            if(isProvAdmin)
            {
                foreach (ListItem liSource in mmsUserRoles.SourceListBox.Items)
                {
                    if (liSource.Value != "DeemedPresumptive")
                    {
                        liSource.Enabled = false;
                    }
                }
            }
            if(isDeemed)
            {
                foreach (ListItem liSource in mmsUserRoles.SourceListBox.Items)
                {
                    if (liSource.Value != "ProviderAdministrator")
                    {
                        liSource.Enabled = false;
                    }
                }
            }
        }
    }

    protected void ProviderTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ProviderTypeFilter.DropDownControl.SelectedIndex != -1)
        {
            BindProviderTypes(new PDMSService.PDMSServiceClient(), ProviderTypeFilter.DropDownControl.SelectedValue);
        }
    }

    protected void ddlPasswordQuestion1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPasswordQuestion1.SelectedIndex == 0)
            return;

        string v1 = ddlPasswordQuestion1.SelectedValue;
        string v2 = ddlPasswordQuestion2.SelectedValue;

        ddlPasswordQuestion2.SelectedValue = v2;
        ListItem li = ddlPasswordQuestion2.Items.FindByValue(v1);
        ddlPasswordQuestion2.Items.Remove(li);

        upSQ2.Update();
    }

    protected void ddlPasswordQuestion2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPasswordQuestion2.SelectedIndex == 0)
            return;

        string v1 = ddlPasswordQuestion1.SelectedValue;
        string v2 = ddlPasswordQuestion2.SelectedValue;

        ddlPasswordQuestion1.SelectedValue = v1;
        ListItem li = ddlPasswordQuestion1.Items.FindByValue(v2);
        ddlPasswordQuestion1.Items.Remove(li);

        upSQ1.Update();
    }

    protected void Validate_cvEmailRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Email.Text.Trim().Length > 0;
    }

    protected void Validate_cvEmailFormat(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Email.Text.Trim().Length > 0 ? Regex.IsMatch(Email.Text, @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$") : true;
    }

    protected void Validate_cvCEmailMatch(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Email.Text.Equals(ConfirmEmail.Text);
    }
    
    protected void UserName_Validating(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Regex.IsMatch(UserName.Text, @"^[a-zA-Z0-9@.]+$") && 0 < UserName.Text.Length;
    }

    protected void Password_Validating(Object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Regex.IsMatch(Password.Text, @"^(?=.*[0-9])(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[!@#$%\^&*\(\)\-_+=;:'""\/\[\]{},.<>|`]).{8,20}$");
    }

    protected void Validate_cvPhoneRequired(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = !string.IsNullOrEmpty(Helper.StripNonNumerics(txtPhone.Text));
    }

    protected void Validate_cvPhoneNumber(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Helper.StripNonNumerics(txtPhone.Text).Length == 10 ? true: false;
    }

    protected void Validate_UserNameExists(object sender, ServerValidateEventArgs e)
    {
        if (AddOrEdit.Text == "Add")
            e.IsValid = Membership.GetUser(UserName.Text) == null;
        else
            e.IsValid = true;
    }

    protected void ListViewButtons_Click(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "AppAdd":
                foreach (int index in lbAppFrom.GetSelectedIndices())
                    lbAppTo.Items.Add(lbAppFrom.Items[index]);
                for (int i = lbAppFrom.GetSelectedIndices().ToList().Count - 1; i >= 0; i--)
                    lbAppFrom.Items.RemoveAt(lbAppFrom.GetSelectedIndices()[i]);
                Helper.SortListBox(ref lbAppFrom, true);
                Helper.SortListBox(ref lbAppTo, true);
                break;
            case "AppRemove":
                foreach (int index in lbAppTo.GetSelectedIndices())
                    lbAppFrom.Items.Add(lbAppTo.Items[index]);
                for (int i = lbAppTo.GetSelectedIndices().ToList().Count - 1; i >= 0; i--)
                    lbAppTo.Items.RemoveAt(lbAppTo.GetSelectedIndices()[i]);
                Helper.SortListBox(ref lbAppFrom, true);
                Helper.SortListBox(ref lbAppTo, true);
                break;
            case "AppFromAll":
                foreach (ListItem item in lbAppFrom.Items)
                    item.Selected = true;
                break;
            case "AppFromNone":
                foreach (ListItem item in lbAppFrom.Items)
                    item.Selected = false;
                break;
            case "AppToAll":
                foreach (ListItem item in lbAppTo.Items)
                    item.Selected = true;
                break;
            case "AppToNone":
                foreach (ListItem item in lbAppTo.Items)
                    item.Selected = false;
                break;
            case "StatAdd":
                foreach (int index in lbStatFrom.GetSelectedIndices())
                    lbStatTo.Items.Add(lbStatFrom.Items[index]);
                for (int i = lbStatFrom.GetSelectedIndices().ToList().Count - 1; i >= 0; i--)
                    lbStatFrom.Items.RemoveAt(lbStatFrom.GetSelectedIndices()[i]);
                Helper.SortListBox(ref lbStatFrom, true);
                Helper.SortListBox(ref lbStatTo, true);
                break;
            case "StatRemove":
                foreach (int index in lbStatTo.GetSelectedIndices())
                    lbStatFrom.Items.Add(lbStatTo.Items[index]);
                for (int i = lbStatTo.GetSelectedIndices().ToList().Count - 1; i >= 0; i--)
                    lbStatTo.Items.RemoveAt(lbStatTo.GetSelectedIndices()[i]);
                Helper.SortListBox(ref lbStatFrom, true);
                Helper.SortListBox(ref lbStatTo, true);
                break;
            case "StatFromAll":
                foreach (ListItem item in lbStatFrom.Items)
                    item.Selected = true;
                break;
            case "StatFromNone":
                foreach (ListItem item in lbStatFrom.Items)
                    item.Selected = false;
                break;
            case "StatToAll":
                foreach (ListItem item in lbStatTo.Items)
                    item.Selected = true;
                break;
            case "StatToNone":
                foreach (ListItem item in lbStatTo.Items)
                    item.Selected = false;
                break;
            default:
                break;
        }
    }
    protected void btnPrev(object sender, EventArgs e)
    {
        if (pnlCreateUser.Visible)
        {
            //user button should not be visible here.
            return;
        }
        else if (pnlPermissions.Visible)
        {
            Response.Redirect(@"~/Maintenance/User.aspx");
        }
    }

    protected void btnCancel(object sender, EventArgs e)
    {
        if (pnlCreateUser.Visible)
        {
            //if (UserHasChanges()) changed in bug 2150
            {
                btnModalOk.Visible = true;
                btnModalOk.Text = "YES";
                btnModalCancel.Visible = true;
                btnModalCancel.Text = "Cancel";
                lblModal.Text = "Are you sure you want to cancel? Your information/changes will not be saved. Click YES to confirm.";
                mpeChangesSaved.Show();
            }
        }
        else if (pnlPermissions.Visible)
        {
            if (hdnIsOHid.Value == "True")
            {
                btnModalOk.Visible = true;
                btnModalOk.Text = "YES";
                btnModalCancel.Visible = true;
                btnModalCancel.Text = "Cancel";
                lblModal.Text = "Are you sure you want to cancel? Your information/changes will not be saved. Click YES to confirm.";
                mpeChangesSaved.Show();
            }
            else
                Response.Redirect(@"~/Process/AdminUserAccounts.aspx?username=" + UserName.Text);
        }
    }

    protected void btnUnlockUser_Click(object sender, EventArgs e)
    {
        MembershipUser user = Membership.GetUser(UserName.Text);
        if (user != null)
        {
            user.IsApproved = true;
            Membership.UpdateUser(user);
            user.UnlockUser();
            btnUnlockUser.Visible = false;
            upUser.Update();
        }
    }

    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        mpeChangesSaved.Hide();
        //bug 2150
        string username = HttpContext.Current.User.Identity.Name;
        string qstrUsername = Request.QueryString["username"];
        bool isProvider = Helper.IsUserInProviderRoles(username);

        if (isProvider)
        {
            Response.Redirect("~/Process/ProviderHomeNew.aspx");
        }
        else
        {
            if (isTechAdmin)
            {
               
                Response.Redirect("~/Maintenance/User.aspx");
                
            }
            else
            {
                Response.Redirect("~/Default.aspx");
            }
        }
    }

    #endregion

    public void SetEntries(System.Web.UI.Control theControl)
    {
        if (theControl.GetType() == typeof(TextBox))
        {
            TextBox txt = (TextBox)theControl;
            txt.Attributes.Add("onKeyPress", "doClick('" + btnSaveTopUser.ClientID + "',event)");
        }
        else if (theControl.GetType() == typeof(eWorld.UI.NumericBox))
        {
            eWorld.UI.NumericBox num = (eWorld.UI.NumericBox)theControl;
            num.Attributes.Add("onKeyPress", "doClick('" + btnSaveTopUser.ClientID + "',event)");
        }
        else if (theControl.GetType() == typeof(DropDownList))
        {
            DropDownList drp = (DropDownList)theControl;
            Helper.DisableBackSpace(drp);
            drp.Attributes.Add("onKeyPress", "doClick('" + btnSaveTopUser.ClientID + "',event)");
        }
        else if (theControl.GetType() == typeof(RadioButtonList))
        {
            RadioButtonList rbl = (RadioButtonList)theControl;
            Helper.DisableBackSpace(rbl);
            rbl.Attributes.Add("onKeyPress", "doClick('" + btnSaveTopUser.ClientID + "',event)");
        }

        foreach (System.Web.UI.Control ctl in theControl.Controls)
        {
            SetEntries(ctl);
        }
    }

    #region Private Methods

    //OHPNM-15917:Add a button on User Maintenance to reactivate IOP users
    private void IsUserActive(string username)
    {
        List<SqlParameter> userParameters = new List<SqlParameter>();
        userParameters.Add(SqlParms.CreateParameter("username", DbType.String, username, true));
        DataSet ds = DataAccess.ExecuteStoredProcedure("usp_IsCheckUserActive", userParameters,"USERACTIVE");
        if(ds.Tables.Count>0  && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
               ViewState["UserId"] = row["USERID"].ToString();
               bool isOHID = Convert.ToBoolean(row["IS_OHID"].ToString());
               bool isDeleted = Convert.ToBoolean(row["IS_DELETED"].ToString());
                if(isOHID && isDeleted)
                {
                    btnReactivate.Visible = true;
                    btnDelete.Visible = false;
                }
                 if (!isDeleted)
                {
                    btnReactivate.Visible = false;
                    btnDelete.Visible = true;
                }
            }
        }
    }
    private void IsDeactivatedProvider(string username)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        string uname = string.IsNullOrEmpty(username) ? HttpContext.Current.User.Identity.Name : username;
        DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(uname).ToString());
        MembershipUser usr = Membership.GetUser(uname);
        DataRow[] deactivated = Helper.HasRows(ds) ? ds.Tables[0].Select("DEACTIVATED_USER IS NOT NULL") : new DataRow[0];

        if (deactivated.Length > 0 && Helper.IsUserInProviderRoles(uname))
        {
            rblIsApproved.Enabled = false;
            MessageBox2.Show(@"This user's registration has been transferred to another user. This user can not be activated.", "Can not activate user");
        }
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            hdnIsOHid.Value = row["IS_OHID"].ToString();
        }
    }

    private void SetupProviderTypeFilter(PDMSService.PDMSServiceClient svc)
    {
        DataSet ds = svc.GetProviderCategories(true);
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            ProviderTypeFilter.DropDownControl.Items.Add(new ListItem(row["PROVIDER_CATEGORY_TYPE_NAME"].ToString(), row["PROVIDER_CATEGORY_TYPE_ID"].ToString()));
        }
        ProviderTypeFilter.DropDownControl.Items.Insert(0, string.Empty);
    }

    private void BindProviderTypes(PDMSService.PDMSServiceClient svc, string CategoryID)
    {
        //lbProvFrom.Items.Clear();

        DataSet ds = null;
        ds = svc.GetProviderTypesByProviderCategory(CategoryID);
        if (ViewState["UserProviderType"] == null)
        {
            ViewState["UserProviderType"] = ds;
        }

        if (Helper.HasRows(ds))
        {
            mmsProviderTypes.SourceListBox.Items.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ListItem item = new ListItem(row["PROVIDER_TYPE_NAME"].ToString(), row["PROVIDER_TYPE_ID"].ToString());
                mmsProviderTypes.SourceListBox.Items.Add(item);
            }
        }
    }

    private void BindWorkflows(PDMSService.PDMSServiceClient svc)
    {
        DataSet ds = svc.WF_SelectWorkflows();
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string name = row["WORKFLOW_NAME"].ToString();
                string id = row["WORKFLOW_ID"].ToString();

                if (lbAppTo.Items.FindByValue(id) != null)
                    continue;

                if (name.Contains("Sanction") || name.Contains("MMIS")) //not part of the workflow values described in PDMS II User Account User Permissions
                    continue;

                ListItem item = new ListItem(name, id);
                lbAppFrom.Items.Add(item);
            }
        }
    }

    private void EditIOPUserContact()
    {
        string username = UserName.Text;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        MembershipUser user = Membership.GetUser(username);
        Guid userid = Helper.GetUserId(user.UserName);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        if (!string.IsNullOrEmpty(txtIOPEmail.Text))
        {
            if (!(txtIOPEmail.Text.Equals(user.Email)))
            {
                user.Email = txtIOPEmail.Text;
                Membership.UpdateUser(user);
            }
        }
        svc.UpdateUserAccountInformation(userid.ToString(), txtIOPContactName.Text, txtTitle.Text, Helper.StripNonNumerics(txtPhone.Text), string.Empty, DateTime.Now, loggedInUserId, false, null);
    }
    
    private void EditUser()
    {
        string username = UserName.Text;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        MembershipUser user = Membership.GetUser(username);
        Guid userid = Helper.GetUserId(user.UserName);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        #region EMAIL
        if (string.IsNullOrEmpty(user.Email))
        {
            if (!string.IsNullOrEmpty(Email.Text))
            {
                user.Email = Email.Text;
                Membership.UpdateUser(user);
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(Email.Text))
            {
                user.Email = Email.Text;
                Membership.UpdateUser(user);
            } 
        }

		//OHPNM-11445 Updating last_modifed_user & last_modified_Date_time for aspnet Users and membership
		List<SqlParameter> parameters = new List<SqlParameter>();
		parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, username, false));
		parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, loggedInUserId, false));
		DataAccess.ExecuteStoredProcedure("aspnet_Users_UpdateUser_Membership_Custom", parameters);

		#endregion
		#region PASSWORD

		if (!string.IsNullOrEmpty(Password.Attributes["value"]))
        {


            if (user.ChangePassword(user.ResetPassword(), Password.Attributes["value"]) && user.IsApproved)
            {
                // TODO: EDV How to get reg id
                //svc.NotifyPasswordReset(user.Email, user.UserName, AppSettings.Get("PDMS-URL"), 0, userid);
            }
        }
        #endregion 
        svc.UpdateUserAccountInformation(userid.ToString(), txtContactName.Text, txtTitle.Text, Helper.StripNonNumerics(txtPhone.Text), string.Empty, DateTime.Now, loggedInUserId, false, null);
        svc.UpdateUserSecurityQuestions(userid.ToString(), Convert.ToInt32(ddlPasswordQuestion1.SelectedValue), Answer1.Text, Convert.ToInt32(ddlPasswordQuestion2.SelectedValue), Answer2.Text, null, string.Empty, DateTime.Now, loggedInUserId);
    }

    private void EditPermissions()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        string username = UserName.Text;
        MembershipUser user = Membership.GetUser(username);
        Guid userid = Helper.GetUserId(user.UserName);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        #region ROLE
        SaveSelectedUserRoles();
        #endregion
        #region PROVIDER TYPE
        DataSet pt = svc.GetUserProviderTypes(userid.ToString());
        if (Helper.HasRows(pt))
        {
            foreach (DataRow row in pt.Tables[0].Rows)
            {
                svc.DeleteUserProviderType(userid.ToString(), Convert.ToInt32(Helper.GetData("PROVIDER_TYPE_ID", row)));
            }
        }

        foreach (ListItem item in mmsProviderTypes.SelectedListBox.Items)
        {
            svc.InsertUserProviderType(user.ProviderUserKey.ToString(), Convert.ToInt32(item.Value), (DateTime?)DateTime.Now, user.ProviderUserKey.ToString());
        }
        #endregion
        #region QUEUE Assignment
        svc.DeleteUserWorkflowPermission(userid.ToString());

        foreach (ListItem item in lbAppTo.Items)
        {
            svc.InsertUserWorkflowPermission(userid.ToString(), Convert.ToInt32(item.Value), loggedInUserId);
        }
        #endregion
    }

    /// <summary>
    /// Save the roles currently selected for the user
    /// </summary>
    private void SaveSelectedUserRoles()
    {
        List<ListItem> allRoles = Helper.GetAllUserRoles();
        string userName = UserName.Text.Trim();
        string userRole = string.Empty;
        // add any new roles that were selected for the user
        foreach (ListItem li in mmsUserRoles.SelectedListBox.Items)
        {
            // find RoleName based on Description selected Desription  // jira 2432
            ListItem thisRole = allRoles.Where(r => r.Text == li.Text).FirstOrDefault();
            if (!Roles.IsUserInRole(userName,thisRole.Value ))   
            {
				//Roles.AddUserToRole(userName, thisRole.Value);

				//OHPNM-11445 Adding User Roles for aspnet_UsersInRoles 
				MembershipUser user = Membership.GetUser(UserName.Text);
				Guid userid = Helper.GetUserId(user.UserName);
				string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("Userid", DbType.Guid, userid, false));
				parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, loggedInUserId, false));
				parameters.Add(SqlParms.CreateParameter("Rolename", DbType.String, thisRole.Value, false));
				DataAccess.ExecuteStoredProcedure("aspnet_Addusersinroles_Custom", parameters);
			}
            if (thisRole.Value == "ProviderAdministrator" || thisRole.Value == "ProviderAgent")
            {
                userRole = thisRole.Value;
            }
                
        }

        //remove any roles for the user that were removed from the control
        string[] rolesForUser = Roles.GetRolesForUser(userName);
        foreach (string roleName in rolesForUser)
        {
            ListItem thisRole = allRoles.Where(r => r.Value == roleName).FirstOrDefault();
            if (mmsUserRoles.SelectedListBox.Items.FindByText(thisRole.Text) == null)
            {
				//Roles.RemoveUserFromRole(userName, thisRole.Value);

				//OHPNM-11445 Soft deleting removed user role
				List<SqlParameter> parameters = new List<SqlParameter>();
				parameters.Add(SqlParms.CreateParameter("Userid", DbType.Guid, Helper.GetUserId(userName), false));
				parameters.Add(SqlParms.CreateParameter("Login_user", DbType.Guid, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), false));
				parameters.Add(SqlParms.CreateParameter("RoleName", DbType.String, thisRole.Value, false));
				DataAccess.ExecuteStoredProcedure("aspnet_Removeusersinroles_Custom", parameters);
			}
        }

        if (Roles.GetRolesForUser(userName).Contains("ProviderAdministrator") || Roles.GetRolesForUser(userName).Contains("ProviderAgent"))
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("NEW_ROLE_VALUE", DbType.String, userRole, false));
            parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, Helper.GetUserId(userName), false));
            DataAccess.ExecuteStoredProcedure("usp_DeleteAgentAssociation", parameters);
        }
    }

    private void AddUser()
    {
        bool isOHID = hdnIsOHid.Value == "True" ? true : false;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        string pwd = isOHID ? Membership.GeneratePassword(10,2) : Password.Attributes["value"];
        MembershipUser user = Membership.CreateUser(isOHID ? txtOHID.Text : UserName.Text, pwd);
        user.Email = isOHID ? txtIOPEmail.Text : Email.Text;
        Membership.UpdateUser(user);
        Guid userid = Helper.GetUserId(user.UserName);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string IOPUserName = isOHID ? txtUserName.Text : null;

        svc.InsertUserAccountInformation(userid.ToString(), isOHID ? txtIOPContactName.Text : txtContactName.Text, txtTitle.Text, Helper.StripNonNumerics(txtPhone.Text), string.Empty,
            string.Empty, 0, 0, string.Empty, 0, string.Empty, string.Empty, 0, 0, null, null, DateTime.Now, loggedInUserId, isOHID, txtOHID.Text, null, IOPUserName, regId: null);
        if (!isOHID)
            svc.InsertUserSecurityQuestions(userid.ToString(), Convert.ToInt32(ddlPasswordQuestion1.SelectedValue), Answer1.Text, Convert.ToInt32(ddlPasswordQuestion2.SelectedValue), Answer2.Text, null, string.Empty, DateTime.Now, loggedInUserId);

		//OHPNM-11445 Updating Created_by_user & created_Date_time & last_modifed_user & last_modified_Date_time for aspnet_users and aspnet_Membership
		List<SqlParameter> parameters = new List<SqlParameter>();
		parameters.Add(SqlParms.CreateParameter("UserName", DbType.String, isOHID ? txtOHID.Text : UserName.Text, false));
		parameters.Add(SqlParms.CreateParameter("Created_by_user", DbType.Guid, loggedInUserId, false));
		DataAccess.ExecuteStoredProcedure("aspnet_Users_CreateUser_Membership_Custom", parameters);
	}

    private void AddPermissions()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        MembershipUser user = Membership.GetUser(UserName.Text);
        Guid userid = Helper.GetUserId(user.UserName);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        SaveSelectedUserRoles();

        foreach (ListItem item in mmsProviderTypes.SelectedListBox.Items)
        {
            svc.InsertUserProviderType(user.ProviderUserKey.ToString(), Convert.ToInt32(item.Value), (DateTime?)DateTime.Now, user.ProviderUserKey.ToString());
        }

        foreach (ListItem item in lbAppTo.Items)
        {
            svc.InsertUserWorkflowPermission(userid.ToString(), Convert.ToInt32(item.Value), loggedInUserId);
        }
    }

    private void InitializeUserRoleSelector()
    {
        if (ViewState["UserRoleCompatibility"] == null)
        {
            ViewState["UserRoleCompatibility"] = Helper.GetUserRoleCompatibility();
        }

        List<ListItem> roles = Helper.GetAllUserRoles();

        foreach (ListItem role in roles)
        {
            if (role.Text != "MCO Administrator Role")
                mmsUserRoles.SourceListBox.Items.Add(role);
        }

    }

    private void SetPageVisible(CurrentPage page)
    {
        pnlCreateUser.Visible =
            pnlPermissions.Visible =
                pnlConfirmation.Visible =
                    ucStep1.Selected =
                        ucStepPermissions.Selected =
                            btnPrevTop.Visible =
                                btnPrevBtm.Visible =
                                    btnSaveTopUser.Visible =
                                        btnSaveBtmUser.Visible =
                                            btnSaveTopPerm.Visible =
                                                btnSaveBtmPerm.Visible = false;

        switch (page)
        {
            case CurrentPage.User:
                pnlCreateUser.Visible =
                    ucStep1.Selected =
                        btnSaveTopUser.Visible =
                            btnSaveBtmUser.Visible = true;
                btnCancTop.Text =
                    btnCancBtm.Text = "Cancel";
                break;
            case CurrentPage.Permissions:
                pnlPermissions.Visible =
                    ucStepPermissions.Selected =
                        btnSaveTopPerm.Visible =
                            btnSaveBtmPerm.Visible =
                                btnPrevTop.Visible =
                                    btnPrevBtm.Visible =
                                        pnlProviderTypes.Visible = true;
                btnCancTop.Text =
                    btnCancBtm.Text = "Cancel";
                break;
            default:
                break;
        }
    }

    private void DisplayHideOldPasswordFields(bool value)
    {
        lblOldPwd.Visible = value;
        RequiredOldPassword.Enabled = value;
        OldPassword.Visible = value;
    }

    private void SetupProfile(string username, Guid userid)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsuai = svc.GetUserAccountInformation(userid.ToString());
        DataRow uai = Helper.HasRows(dsuai) ? dsuai.Tables[0].Rows[0] : null;

        MembershipUser usr = Membership.GetUser(username);
        if (usr != null) Helper.SetRadioButtonListSelect(rblIsApproved, usr.IsApproved.ToString());
        txtContactName.Text = uai == null ? string.Empty : Helper.GetData("Name", uai);
        txtIOPContactName.Text = uai == null ? string.Empty : Helper.GetData("Name", uai);
        txtTitle.Text = uai == null ? string.Empty : Helper.GetData("Title", uai);
        txtPhone.Text = uai == null ? string.Empty : Helper.FormatPhone(Helper.GetData("Phone", uai));
        hdnIsOHid.Value = uai["IS_OHID"].ToString();
        txtOHID.Text = uai["OHID"].ToString();

        MembershipUser user = Membership.GetUser(username);
        Email.Text = txtIOPEmail.Text = user.Email;
        ConfirmEmail.Text = user.Email;
        UserName.Enabled = false;
        UserName.Text = user.UserName;


        DataSet ds = svc.GetUserRolesByUser(user.UserName);  // Jira 2432
        if (Helper.HasRows(ds))
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                ListItem selectedItem = mmsUserRoles.SelectItemByText(Helper.GetString("RoleName", dr));
                UpdateSelectableUserRoles(selectedItem);
            }
        }

        DataSet dssq = svc.GetUserSecurityQuestions(userid.ToString());
        int q1;
        int q2;

        if (Helper.HasRows(dssq))
        {
            DataRow sq = Helper.HasRows(dssq) ? dssq.Tables[0].Rows[0] : null;
            ddlPasswordQuestion1.SelectedIndex = int.TryParse(Helper.GetData("Question1ID", sq), out q1) ? q1 : 0;
            Answer1.Text = Helper.GetData("Answer1", sq);
            ddlPasswordQuestion2.SelectedIndex = int.TryParse(Helper.GetData("Question2ID", sq), out q2) ? q2 : 0;
            Answer2.Text = Helper.GetData("Answer2", sq);
        }

        DataSet pt = svc.GetUserProviderTypes(userid.ToString());
        if (Helper.HasRows(pt))
        {
            foreach (DataRow row in pt.Tables[0].Rows)
            {
                ListItem item = new ListItem(row["PROVIDER_TYPE_NAME"].ToString(), row["PROVIDER_TYPE_ID"].ToString());
                mmsProviderTypes.SelectItemByText(item.Value);
                UpdateSelectableProviderTypes(item);
            }
        }

        DataSet wp = svc.SelectUserWorkflowPermission(userid.ToString());
        if (Helper.HasRows(wp))
        {
            foreach (DataRow row in wp.Tables[0].Rows)
            {
                string t = Helper.GetString("NAME", row);
                string v = Helper.GetString("WORKFLOW_ID", row);
                ListItem item = new ListItem(t, v);
                if (lbAppFrom.Items.FindByValue(item.Value) == null)
                    lbAppTo.Items.Add(item);
            }
            Helper.SortListBox(ref lbAppTo, true);
        }
    }

	#endregion

	//OHPNM-13583 Soft delete user
	protected void btnDelete_Click(object sender, EventArgs e)
	{
		MembershipUser user = Membership.GetUser(UserName.Text);
		Guid userid = Helper.GetUserId(user.UserName);
		string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("User_ID", DbType.Guid, userid, false));
        parameters.Add(SqlParms.CreateParameter("LoggedIn_UserID", DbType.Guid, loggedInUserId, false));
        DataAccess.ExecuteStoredProcedure("usp_SoftDelete_UserData", parameters);

        lblModal.Text = "User Deleted Successfully!";
        btnModalOk.Visible = false;
        btnModalCancel.Visible = true;
        btnModalCancel.Text = "OK";
        mpeChangesSaved.Show();

    }

    //OHPNM-15917:Add a button on User Maintenance to reactivate IOP users
    protected void btnReactivate_Click(object sender, EventArgs e)
    {
        String userid = Convert.ToString(ViewState["UserId"]);
        string loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("userId", DbType.Guid, userid, false));
        parameters.Add(SqlParms.CreateParameter("LoggedIn_UserID", DbType.Guid, loggedInUserId, false));
        DataAccess.ExecuteStoredProcedure("usp_ReactivateUser", parameters);
        lblModal.Text = "User Reactivated Successfully!";
        btnModalOk.Visible = false;
        btnModalCancel.Visible = true;
        btnModalCancel.Text = "OK";
        mpeChangesSaved.Show();
    }
}