using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS.MembershipProvider;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_UserProfile : System.Web.UI.UserControl
{
    private static bool has_SecurityQues = false;
    protected void Page_PreRender(object sender, EventArgs e)
    {

    }

    protected void Page_Load(object sender, EventArgs e)
    {




    }
    protected void ddlPasswordQuestion1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPasswordQuestion1.SelectedIndex == 0)
            return;

        string v1 = ddlPasswordQuestion1.SelectedValue;
        string v2 = ddlPasswordQuestion2.SelectedValue;

        Registration.SecurityQuestions(ref ddlPasswordQuestion2);
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

        Registration.SecurityQuestions(ref ddlPasswordQuestion1);
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
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

            if (!string.IsNullOrEmpty(UserName.Text.Trim()))
            {
                string username = UserName.Text.Trim();
                //check the user is logged in user then only 
                if (username == HttpContext.Current.User.Identity.Name || string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    EditUser();

                }

            }
            // Appove the user
            MembershipUser user = Membership.GetUser(UserName.Text);
            if (user != null)
            {
                if (rblIsApproved.SelectedIndex != -1)
                {
                    user.IsApproved = rblIsApproved.SelectedIndex == 0;
                    Membership.UpdateUser(user);
                }
            }
            //show only once if the days are in priornotice time frame.
            SessionVarRetriever.PasswordExpiryMsgShown = true;
            pnlChangesSaved.Visible = true;
            pnlCreateUser.Visible = false;
            pnlButtons.Visible = false;
            //BtnCancelBtmUser_Click(sender, e);
            return;
        }
        catch (PasswordException ex)
        {

            SessionVarRetriever.PasswordExpiryMsgShown = false;
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
    protected void BtnCancelBtmUser_Click(object sender, EventArgs e)
    {
        try
        {
            if (Helper.IsPasswordExpired(HttpContext.Current.User.Identity.Name))
            {
                //do log out
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();
                }
                Response.Redirect(Helper.RedirectLoginURL());

            }
            else
            {
                SessionVarRetriever.PasswordExpiryMsgShown = true;
                Response.Redirect("~/Default.aspx");
            }
        }
        catch (Exception ex)
        {
            throw ex;
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
    private void SetupProfile(string username, Guid userid)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsuai = svc.GetUserAccountInformation(userid.ToString());
        DataRow uai = Helper.HasRows(dsuai) ? dsuai.Tables[0].Rows[0] : null;

        MembershipUser usr = Membership.GetUser(username);
        if (usr != null) Helper.SetRadioButtonListSelect(rblIsApproved, usr.IsApproved.ToString());
        txtContactName.Text = uai == null ? string.Empty : Helper.GetData("Name", uai);
        txtTitle.Text = uai == null ? string.Empty : Helper.GetData("Title", uai);
        txtPhone.Text = uai == null ? string.Empty : Helper.FormatPhone(Helper.GetData("Phone", uai));
        txtPhoneExt.Text = uai == null ? string.Empty : Helper.GetData("PhoneExt", uai);

        MembershipUser user = Membership.GetUser(username);
        Email.Text = user.Email;
        ConfirmEmail.Text = user.Email;
        UserName.Enabled = false;
        UserName.Text = user.UserName;

        string[] roles = Roles.GetRolesForUser(user.UserName);


        DataSet dssq = svc.GetUserSecurityQuestions(userid.ToString());
        int q1;
        int q2;

        if (Helper.HasRows(dssq))
        {
            has_SecurityQues = true;
            DataRow sq = Helper.HasRows(dssq) ? dssq.Tables[0].Rows[0] : null;
            ddlPasswordQuestion1.SelectedIndex = int.TryParse(Helper.GetData("Question1ID", sq), out q1) ? q1 : 0;
            Answer1.Text = Helper.GetData("Answer1", sq);
            ddlPasswordQuestion2.SelectedIndex = int.TryParse(Helper.GetData("Question2ID", sq), out q2) ? q2 : 0;
            Answer2.Text = Helper.GetData("Answer2", sq);
        }

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
            user.Email = Email.Text;
            Membership.UpdateUser(user);
        }
        else
        {
            if (!string.IsNullOrEmpty(Email.Text))
            {
                user.Email = Email.Text;
                Membership.UpdateUser(user);
            }
        }
        #endregion
        #region PASSWORD
        if (!string.IsNullOrEmpty(Password.Attributes["value"]))
        {

            if (user.ChangePassword(OldPassword.Attributes["value"], Password.Attributes["value"]) && user.IsApproved)
            {
                // TODO:  How to get reg id
                svc.NotifyPasswordReset(user.Email, user.UserName, AppSettings.Get("PDMS-URL"), 0,  userid);
            }

        }
        #endregion
       
        if(!has_SecurityQues)
        {
            svc.InsertUserSecurityQuestions(userid.ToString(), Convert.ToInt32(ddlPasswordQuestion1.SelectedValue), Answer1.Text, Convert.ToInt32(ddlPasswordQuestion2.SelectedValue), Answer2.Text, null, string.Empty, DateTime.Now, loggedInUserId);
        }
        else
            svc.UpdateUserSecurityQuestions(userid.ToString(), Convert.ToInt32(ddlPasswordQuestion1.SelectedValue), Answer1.Text, Convert.ToInt32(ddlPasswordQuestion2.SelectedValue), Answer2.Text, null, string.Empty, DateTime.Now, loggedInUserId);

        svc.UpdateUserAccountInformation(userid.ToString(), txtContactName.Text, txtTitle.Text, Helper.StripNonNumerics(txtPhone.Text), txtPhoneExt.Text, DateTime.Now, loggedInUserId, false, null,true);
    }

    public void SetUserInfoVisibility(bool value)
    {
        pnlCreateUser.Visible = pnlChangesSaved.Visible = value;
    }
    public void SetCancelButtonVisibility(bool value)
    {
        BtnCancelBtmUser.Visible = value;
    }

    public void DisplayUserProfileInformation(string username)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();


            Registration.SecurityQuestions(ref ddlPasswordQuestion1);
            Registration.SecurityQuestions(ref ddlPasswordQuestion2);


            if (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "Administrator") && !username.Equals(HttpContext.Current.User.Identity.Name))
            {
                Response.Redirect(Helper.RedirectLoginURL());
            }

            if (!string.IsNullOrEmpty(username))
            {
                Guid userid = Helper.GetUserId(username);
                if (!userid.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                {


                    RequirePassword.Enabled = false;
                    SetupProfile(username, userid);
                }
            }


            pnlChangesSaved.Visible = false;


            if (!string.IsNullOrEmpty(UserName.Text))
            {
                MembershipUser user = Membership.GetUser(UserName.Text);
                if (user == null)
                {
                    btnUnlockUser.Visible = false;
                    divActive.Visible = false;
                }
                else
                {
                    btnUnlockUser.Visible = user.IsLockedOut && Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name) && user.IsApproved;
                    divActive.Visible = Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name);
                }
            }


        }


    }

    public void DisplayUserProfileInformation(string username, string password)
    {
        lblProfileTitle.Visible = false;
        if (Membership.ValidateUser(username, password))
        {
            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();


            Registration.SecurityQuestions(ref ddlPasswordQuestion1);
            Registration.SecurityQuestions(ref ddlPasswordQuestion2);

            pnlChangesSaved.Visible = false;
            btnUnlockUser.Visible = false;


            if (!string.IsNullOrEmpty(username))
            {
                Guid userid = Helper.GetUserId(username);
                if (!userid.ToString().Equals("00000000-0000-0000-0000-000000000000"))
                {
                    RequirePassword.Enabled = false;
                    SetupProfile(username, userid);
                }
            }
        }
        btnUnlockUser.Visible = false;
        divActive.Visible = false;
    }

}