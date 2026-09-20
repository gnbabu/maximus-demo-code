using PdmsWebEvents;
using StateSingleSignOn.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StateSingleSignOn
{
    public partial class SSO : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LoginUser_LoggingIn(object sender, EventArgs e)
        {
            Page.Validate("LoginUserValidationGroup");
            if (!Page.IsValid) return;

            MembershipUser user = Membership.GetUser(UserName.Text);
            SessionVarRetrieverSS.UserIdSelected = HelperSS.GetUserId(UserName.Text).ToString();
            if (user == null)
            {
                lblErrormessage.Visible = true;
                return;
            }

            if (user.IsLockedOut)
            {
                AccountLockedEvent accountLockedEvent = new AccountLockedEvent("Account Locked", this, UserName.Text);
                accountLockedEvent.Raise();
            }
            bool isInactivated = HelperSS.IsPasswordExpiredAndInactivated(UserName.Text);
            if (isInactivated && !user.IsApproved)
            {
                //pnlLockedOut.Visible = true;
                //ltrlLockedOut.Text = (string)GetGlobalResourceObject("BrandingResource", "pnlLockedInactivatedMsg");
                //pnlLockedOut.CssClass = "wdAll error-message";
                //e.Cancel = true;
                //return;
            }

            if (!user.IsApproved)
            {
                //newLogin.Style["display"] = "none";
                //pnlLockedOut.Visible = true;
                // e.Cancel = true;
            }

            // For MITS users         
            if (HelperSS.IsMITSuserForce_Password_Reset(HelperSS.GetUserId(user.UserName).ToString()))
            {
                if (!HelperSS.IsTempPasswordSent(HelperSS.GetUserId(user.UserName).ToString()))  //if they are first time logging in then generate a temp password and send email
                {
                    Response.Redirect("~/Account/FirstLoginDetails.aspx");
                    //e.Cancel = true;
                    //return;
                }
                else
                {
                    if (Membership.ValidateUser(UserName.Text, password.Text))
                    {
                        //divLogin.Visible = pnlNewsBox.Visible = false;
                        //FailureTextInvalid.Visible = ucUserProfile.Visible = true;
                        //FailureTextInvalid.Text = "* Please Reset Password, Confirm Account Information and Security Questions before logging in.";
                        //ucUserProfile.DisplayUserProfileInformation(UserName.Text, password.Text);
                        //e.Cancel = true;
                        //return;
                    }
                    else
                    {
                        //Login1_LoginError(sender, e);
                        //return;
                    }
                }
            }
            else
            {

                if (Membership.ValidateUser(UserName.Text, password.Text))
                {
                    FormsAuthentication.SetAuthCookie(UserName.Text, true);
                    SessionVarRetrieverSS.UserName = UserName.Text;
                    PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                    DataSet ds = svc.GetUserAccountInformation(HelperSS.GetUserId(UserName.Text).ToString());
                    if (HelperSS.HasRows(ds))
                    {
                        SessionVarRetrieverSS.DisplayUserName = ds.Tables[0].Rows[0]["Name"].ToString();
                        SessionVarRetrieverSS.OhID = ds.Tables[0].Rows[0]["OhID"].ToString();
                        SessionVarRetrieverSS.IsOhID = true;
                    }
                    // showLegalAgreement();
                    Session["LogKey"] = Guid.NewGuid().ToString();

                    if (SessionVarRetrieverSS.IsOhID)
                    {
                        FormsAuthentication.SetAuthCookie(SessionVarRetrieverSS.UserName, true);
                        string relink = "https://mesc.mpc-release.maximus.com/MESC_DEMO/Account/Login.aspx?username=";
#if DEBUG
                        relink = "http://localhost:64176/Account/Login.aspx?username=";
#endif
                        Response.Redirect(relink + SessionVarRetrieverSS.UserName, false);
                    }
                    else
                    {
                        if (HttpContext.Current.User.Identity.IsAuthenticated)
                        {
                            Response.Redirect("http://localhost:64176/Account/login.aspx?username="+ SessionVarRetrieverSS.UserName, false);
                        }
                    }

                    //e.Cancel = true;
                    //return;
                }
                else
                {
                    lblErrormessage.Visible = true;
                }
            }
        }
        protected void btnGotoIOP_Click(object sender, EventArgs e)
        {
            string uri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
            SessionVarRetrieverSS.IsOhID = true;
            if (SessionVarRetrieverSS.IsNewUser)
            {
                Response.Redirect(uri, false);
            }
            else
            {
                //GetAuthorizationCode();
            }
        }
        protected void lnkCreateAccount_Clicked(object sender, EventArgs e)
        {
            string uri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
            SessionVarRetrieverSS.IsNewUser = true;
            Response.Redirect(uri, false);
        }
    }
}