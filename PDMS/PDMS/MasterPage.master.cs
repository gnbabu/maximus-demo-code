using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Core.Libraries;
using System.Diagnostics;
using System.Configuration;

public partial class Main_MasterPage : BaseMasterPage
{
    public int TimeOutValue = 0;
    public string signupURL = "~/Account/MyProfile.aspx";
    public string SnippetKey;
    private void SetupTNFiles()
    {
        /*Changes for - Bug CSS CleanUp*/
        string prefix = "http://";
    }

    public string OHID
    {
        get
        {
            if (string.IsNullOrEmpty(SessionVarRetriever.OhID))
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(SessionVarRetriever.UserName).ToString());
                if (Helper.HasRows(ds))
                {
                    SessionVarRetriever.OhID = ds.Tables[0].Rows[0]["OhID"].ToString();
                }
                return SessionVarRetriever.OhID;
            }
            else
            {
                return SessionVarRetriever.OhID;
            }
        }
    }

    public string HideUserProfilePopUpForClaims
    {
        get
        {
            if (Session["HideUserProfilePopUpForClaims"] == null)
                Session["HideUserProfilePopUpForClaims"] = string.Empty;
            return (string)Session["HideUserProfilePopUpForClaims"];
        }
        set
        {
            Session["HideUserProfilePopUpForClaims"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Session.Timeout = 5;
        LoginName loginName = null;
        SnippetKey = ConfigurationManager.AppSettings["ZendeskSnippetKey"];

        int formAuthTimeout = (Int16)(System.Web.Security.FormsAuthentication.Timeout.TotalMinutes);
        formAuthTimeout = formAuthTimeout >= 3 ? formAuthTimeout : 3; // should be a greater than 2 to sync Session time out and session end timer popup. 

        // DCPDMS-3213 - upped the timeout difference so the telerik and forms authentication timeouts don't happen at the same time.  RBM 8/27/2019
        TimeOutValue = Convert.ToInt32(formAuthTimeout - 2) * 60;

        if (!IsPostBack)
        {

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                RadSessionNote.Enabled = true;

                //configure the notification to automatically show 1 min before session expiration
                RadSessionNote.ShowInterval = TimeOutValue * 1000;
                //set the redirect url as a value for an easier and faster extraction in on the client
                RadSessionNote.Value = Page.ResolveClientUrl("~/Account/Logout.aspx");

                //OHPNM-13882 - Password popup not required
                //if (!SessionVarRetriever.IsOhID)    //this should not be shown for OH|ID Users.
                //    DisplayPasswordExpiryNotification();

                //refresh token code
                HttpCookie IOPAccessToken = Request.Cookies["IOPAccessToken"];
                if (IOPAccessToken == null && SessionVarRetriever.IsOhID)
                {
                    RefreshIOPTokens();
                }

                HyperLink hypProfile = (HyperLink)Helper.FindTheControl(LoginView2, "hypProfile");
                if (SessionVarRetriever.IsOhID)
                {
                    if (hypProfile != null)
                        hypProfile.Enabled = false;
                }
            }
            else
            {
                RadSessionNote.Enabled = false;
                //HyperLink hypSignup = (HyperLink)Helper.FindTheControl(LoginView2, "hypSignup");
                //hypSignup.NavigateUrl = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
                signupURL = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
            }

            // Get version number
            string versionText = "Version: " + Helper.GetAppSettingFromDB("Version");
            string env = Helper.GetAppSettingFromDB("Environment");
            if (!string.IsNullOrEmpty(env))
                versionText += " (" + env + ")";

            var buildVersion = Convert.ToString(ConfigurationManager.AppSettings["BuildVersion"]);
            if (!string.IsNullOrEmpty(buildVersion))
                versionText += " " + buildVersion;
            var buildDate = Convert.ToString(ConfigurationManager.AppSettings["BuildDate"]);
            if (!string.IsNullOrEmpty(buildDate))
                versionText += " " + buildDate;

            lblVersion.Text = versionText;

            //mltEntrust.ActiveViewIndex = 0;                 // Default is Integration
            /*if (env.IndexOf("UAT") != -1) mltEntrust.ActiveViewIndex = 1;
            else if (env.IndexOf("Production") != -1)
            {
                mltEntrust.ActiveViewIndex = 2;
            }*/
            bool loggedOut = string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name);
            bool isLandingPage = Request.Url.AbsolutePath.IndexOf("default.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0;
            bool isLoginPage = Request.Url.AbsolutePath.IndexOf(Helper.LoginPageName(), StringComparison.CurrentCultureIgnoreCase) >= 0;
            bool isRecoveryPage = Request.Url.AbsolutePath.IndexOf("PasswordReset.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0 || Request.Url.AbsolutePath.IndexOf("UserNameRecovery.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0;
            bool isExtRegisterPage = Request.Url.AbsolutePath.IndexOf(@"Account/AccountCreation.aspx", StringComparison.CurrentCultureIgnoreCase) >= 0;
            //bug2258_Default.Style["display"] = loggedOut && isLandingPage ? "block" : "none";
            //bug2258_Login.Style["display"] = loggedOut && (isLoginPage || isRecoveryPage || isExtRegisterPage) ? "block" : "none"; //additionally bug 2435

            string showChat = AppSettings.Get("PDMS-MEDChat");
            /*if (showChat == "true")
                ChatBox1.Visible = true;*/

            // LeftMenu.Visible = true;

            loginName = LoginView2.FindControl("LoginName1") as LoginName;

            if (loginName != null)
            {
                loginName.FormatString = SessionVarRetriever.DisplayUserName;
            }
        }
        else
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated && SessionVarRetriever.IsOhID)
            {
                //refresh token code

                HttpCookie IOPAccessToken = Request.Cookies["IOPAccessToken"];
                if (IOPAccessToken == null)
                {
                    RefreshIOPTokens();
                }
            }
        }

        //   this.divCallTracker.Visible = HttpContext.Current.User.Identity.IsAuthenticated && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderOper) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Administrator) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Operator));
        // This allows the page header directive to work: <script src="<%# Page.ResolveClientUrl("~/Scripts/jquery-ui.js") %>"></script>
        Page.Header.DataBind();
        loginName = LoginView2.FindControl("LoginName1") as LoginName;

        if (loginName != null)
        {
            loginName.FormatString = SessionVarRetriever.DisplayUserName;
            loginName.ToolTip = OHID;
        }

        loginName = LoginView1.FindControl("LoginName1") as LoginName;

        if (loginName != null)
        {
            loginName.ToolTip = OHID;
        }

    }
    public void RefreshIOPTokens()
    {
        string refreshToken = string.Empty;
        HttpCookie IOPRefreshToken = Request.Cookies["IOPRefreshToken"];
        if (IOPRefreshToken != null)
        {
            refreshToken = IOPRefreshToken.Value.ToString();
            ReceiveTokenOpenId.IOPAccessToken tokenobj;
            ReceiveTokenOpenId IOPrefresh = new ReceiveTokenOpenId();
            tokenobj = IOPrefresh.IOPRefreshTokens(refreshToken);

            if (tokenobj != null)
            {

                Response.Cookies.Remove("IOPAccessToken");
                Response.Cookies.Remove("IOPRefreshToken");
                Response.Cookies.Remove("IOPIDToken");

                int IOP_RefreshTokenTrustedLength = Convert.ToInt32(AppSettings.Get("IOPCookieRefreshTime"));

                // Create the Cookies to the new token object
                HttpCookie IOPAccessToken = new HttpCookie("IOPAccessToken");
                IOPAccessToken.HttpOnly = true;
                IOPAccessToken.Value = tokenobj.access_token;
                IOPAccessToken.Expires = DateTime.Now.Add(new TimeSpan(0, 0, tokenobj.expires_in));
                Response.Cookies.Add(IOPAccessToken);

                HttpCookie IOPRefreshTokenNew = new HttpCookie("IOPRefreshToken");
                IOPRefreshTokenNew.HttpOnly = true;
                IOPRefreshTokenNew.Value = tokenobj.refresh_token;
                IOPRefreshTokenNew.Expires = DateTime.Now.Add(new TimeSpan(IOP_RefreshTokenTrustedLength, 0, 0));
                Response.Cookies.Add(IOPRefreshTokenNew);

            }
            else                    // if no tokens are received during refresh tokens logout the user
            {
                string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    HttpContext.Current.Response.Redirect(logoutredirectUri, false);
                }
            }
        }
    }

    protected void btnLogoutOk_Clicked(object sender, EventArgs e)
    {
        //string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
        //if (Request.Cookies["IOPIDToken"] != null)
        //{
        //    HttpCookie IOPIDToken = Response.Cookies["IOPIDToken"];
        //    IOPIDToken.Value = null;
        //    IOPIDToken.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(IOPIDToken);
        //    //Response.Cookies.Remove("IOPIDToken");
        //}
        //if (Request.Cookies["IOPAccessToken"] != null)
        //{
        //    HttpCookie IOPAccessToken = Response.Cookies["IOPAccessToken"];
        //    IOPAccessToken.Value = null;
        //    IOPAccessToken.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(IOPAccessToken);
        //    //Response.Cookies.Remove("IOPAccessToken");
        //}

        //if (Request.Cookies["IOPRefreshToken"] != null)
        //{
        //    HttpCookie IOPRefreshToken = Response.Cookies["IOPRefreshToken"];
        //    IOPRefreshToken.Value = null;
        //    IOPRefreshToken.Expires = DateTime.Now.AddDays(-1);
        //    Response.Cookies.Add(IOPRefreshToken);
        //    //Response.Cookies.Remove("IOPRefreshToken");
        //}

        //ClearCookies();

        //HttpContext.Current.Session.Clear();
        //HttpContext.Current.Session.Abandon();                  // Remove the session entries
        //System.Web.Security.FormsAuthentication.SignOut();
        //HttpContext.Current.Session.RemoveAll();
        //Request.Cookies.Clear();
        //Response.Cookies.Clear();
        ////Response.Redirect("~/Account/Logout.aspx", false);
        //Response.Redirect(logoutredirectUri, false);
    }
    private void ClearCookies()
    {
        HttpCookie IOPAccessToken = new HttpCookie("IOPAccessToken");
        IOPAccessToken.HttpOnly = true;
        IOPAccessToken.Value = "";
        IOPAccessToken.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(IOPAccessToken);

        HttpCookie IOPRefreshToken = new HttpCookie("IOPRefreshToken");
        IOPRefreshToken.HttpOnly = true;
        IOPRefreshToken.Value = "";
        IOPRefreshToken.Expires = DateTime.Now.AddDays(-1);
        Response.Cookies.Add(IOPRefreshToken);
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();                  // Remove the session entries
            System.Web.Security.FormsAuthentication.SignOut();

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty); // clear haven token
            HttpContext.Current.Session.RemoveAll();
            Request.Cookies.Clear();
            ClearCookies();
            Response.Redirect(Helper.RedirectLoginURL(), false);
        }
    }
    private void SetMenuItemText(MenuItemCollection col, string itemText, string newItemText)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) SetMenuItemText(itm.ChildItems, itemText, newItemText);
            if (itm.Value == itemText)
            {
                itm.Text = newItemText;
                return;
            }
        }
    }

    protected void OnCallbackUpdate(object sender, Telerik.Web.UI.RadNotificationEventArgs e)
    {

    }

    protected void btnLogOut_Clicked(object sender, EventArgs e)
    {
        string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];
        //Response.Redirect("~/Account/Logout.aspx");
        if (Page.IsPostBack)
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (SessionVarRetriever.IsOhID)
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.RemoveAll();
                    Request.Cookies.Clear();
                    Response.Redirect(logoutredirectUri, false);
                }
                else
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    HttpContext.Current.Session.Abandon();                  // Remove the session entries
                    Response.Redirect(Helper.RedirectLoginURL(), false);
                }
                ClearCookies();

            }
    }
    private void RemoveMenuItem(MenuItemCollection col, string value)
    {
        foreach (MenuItem itm in col)
        {
            if (itm.ChildItems.Count > 0) RemoveMenuItem(itm.ChildItems, value);
            if (itm.Value == value)
            {
                col.Remove(itm);
                return;
            }
        }
    }
    // Remove the "PDMS to MMIS" menu option if necessary
    protected void mnuTop_PreRender(object sender, EventArgs e)
    {
        Menu mnu = (Menu)sender;
        if (mnu == null) return;
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            RemoveMenuItem(mnu.Items, "Create Account");
            RemoveMenuItem(mnu.Items, "Login");
            RemoveMenuItem(mnu.Items, "Log In");
            if (!Helper.IsLoggedInUserInAdminRole()) RemoveMenuItem(mnu.Items, "My Queue");
        }
        //This code needs to be removed in future // since Call Tracker is added to the webdata.sitemap for modernization 
        // we are removing it from the old site since it will create duplicate in the old site
        /*if (!Helper.IsModern())
        {
            RemoveMenuItem(mnu.Items, "");
            RemoveMenuItem(mnu.Items, "Call Tracker");
             
        }*/
    }

    //protected void mnuLeftNav_DataBound(object sender, EventArgs e)
    //{
    //    MenuItem mnuItemToRemove = null;
    //     MenuItem mnuItemToRemoveSiteVisit = null;
    //    foreach (MenuItem item in ((Menu)sender).Items)
    //    {
    //        if (item.NavigateUrl.Contains("?target=_blank"))
    //        {
    //            item.Target = "_blank";
    //            item.NavigateUrl = item.NavigateUrl.Replace("?target=_blank", "");
    //        }

    //        // Hide Public Search if logged in, hide GroupReview page if not logged in
    //        if ((HttpContext.Current.User.Identity.IsAuthenticated && item.NavigateUrl.Contains("PublicSearch.aspx"))
    //            || (!HttpContext.Current.User.Identity.IsAuthenticated && item.NavigateUrl.Contains("GroupReview.aspx")))
    //        {
    //            mnuItemToRemove = item;
    //        }
    //       if (SessionVarRetriever.MyQueueSelectedRoleName == CON.UserRole.SiteVisitOperator && item.Text == "Site Visit Assignment")
    //       {
    //           mnuItemToRemoveSiteVisit = item;
    //       }
    //    }
    //    if(mnuItemToRemoveSiteVisit != null)
    //mnuLeftNav.Items.Remove(mnuItemToRemoveSiteVisit);
    //    if (mnuItemToRemove != null)
    //        mnuLeftNav.Items.Remove(mnuItemToRemove);
    //}


    #region Password Expiry Region

    private void showProfilePopup()
    {
        //OHPNM-13882 - Password popup not required
        /* pnlProfile.Visible = true;
		// pnlModal.CssClass = "pnlWidth";
		 ltlPwdnotification.Visible = false;
		 btnModalProfile.Visible = false;
		 btnModalCancel.Visible = false;
		 UserProfile.SetCancelButtonVisibility(true);
		 UserProfile.DisplayUserProfileInformation(HttpContext.Current.User.Identity.Name);*/
    }

    protected void btnModalProfile_Click(object sender, EventArgs e)
    {
        //OHPNM-13882 - Password popup not required
        //showProfilePopup();
        //mpePassWordExpiry.Show();
    }
    //OHPNM-13882 - Password popup not required
    private void DisplayPasswordExpiryNotification()
    {
        /* OHPNM-13882 - Password popup not required
        DataTable dtPwdInfo = Helper.GetPasswordExpiryInfo(HttpContext.Current.User.Identity.Name);
        int PwdChangeddays = 0;
        int PwdChangeLimit = 0;
        int priorNoticeDays = 0;
        mpePassWordExpiry.Hide();
        if ((SessionVarRetriever.PasswordExpiryMsgShown == null || !SessionVarRetriever.PasswordExpiryMsgShown) && HttpContext.Current.User.Identity.IsAuthenticated)
        {

            //check the pasword info before displaying Notification
            bool isPasswordExpiryApplicable = Helper.IsPasswordExpiryApplicable(HttpContext.Current.User.Identity.Name);
            if (Helper.HasRows(dtPwdInfo) && isPasswordExpiryApplicable)
            {
                DataRow dtRow = dtPwdInfo.Rows[0];
                PwdChangeddays = (dtRow["DayCount"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["DayCount"].ToString()) : 0;
                PwdChangeLimit = (dtRow["ExpiryLimitDays"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["ExpiryLimitDays"].ToString()) : 0;
                priorNoticeDays = (dtRow["PriorNoticeDays"] != null) ? Convert.ToInt32(dtPwdInfo.Rows[0]["PriorNoticeDays"].ToString()) : 0;
                
                if (PwdChangeddays > PwdChangeLimit)
                {
                    showProfilePopup();
                    mpePassWordExpiry.Show();
                }
                else
                {

                    if (PwdChangeddays >= (PwdChangeLimit - priorNoticeDays))
                    {
                        //in notice time trame
                        mpePassWordExpiry.Show();
                    }

                }
            }


        }*/
    }

    #endregion

    public class HelpData
    {
        public string Mode { get; set; }      // popup / pdf
        public string Title { get; set; }   // HTML from RadEditor
        public string Content { get; set; }   // HTML from RadEditor
        public string PdfUrl { get; set; }    // PDF path
    }

}
