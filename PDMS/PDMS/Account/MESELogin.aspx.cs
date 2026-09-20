using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MAXIMUS.Core.Libraries;
using PdmsWebEvents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_MESELogin : System.Web.UI.Page
{
    private string uname = string.Empty;
    private string displayUname = string.Empty;
    private static bool IsIOPTimeOutException = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isValidToken = false;
        Button btn;

        if (!IsPostBack)
        {
            lblErrormessage.Visible = false;
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


            string ShowNewLogin = AppSettings.Get("ShowNewLogin", "false");

            if (Request.QueryString["username"] != null)
            {
                string username = Request.QueryString["username"];
                FormsAuthentication.SetAuthCookie(username, true);
                SessionVarRetriever.UserName = username;
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(username).ToString());
                if (Helper.HasRows(ds))
                {
                    SessionVarRetriever.DisplayUserName = ds.Tables[0].Rows[0]["Name"].ToString();
                    SessionVarRetriever.OhID = ds.Tables[0].Rows[0]["OhID"].ToString();
                    SessionVarRetriever.IsOhID = true;
                }

                Session["LogKey"] = Guid.NewGuid().ToString();

                FormsAuthentication.SetAuthCookie(SessionVarRetriever.UserName, true);
                Response.Redirect("~/Default.aspx", false);
            }
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                Request.Cookies[".ASPXAUTH"].Expires = DateTime.Now.AddDays(-1);
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Session.Abandon();
                // Remove the session entries
                Response.Redirect(Request.RawUrl);
            }
            if (Request.QueryString["id"] != null)
            {
                try
                {
                    // Get user by guid id from the url
                    Guid userID = new Guid(Request.QueryString["id"]);
                    MembershipUser user = Membership.GetUser((object)userID);
                    if (user != null)
                    {
                        // Activate the user's account
                        user.IsApproved = true;
                        Membership.UpdateUser(user);
                    }
                }
                catch { }
            }
            if (Request.UrlReferrer != null && !IsIOPTimeOutException)
            {
                if (Request.Params["code"] == null && Request.UrlReferrer.ToString().Contains("ohid"))
                {
                    GetAuthorizationCode();
                }
            }
            if (Request.Params["code"] != null && SessionVarRetriever.IsOhID && !IsIOPTimeOutException)
            {
                try
                {
                    var code = Request.Params["code"];
                    ReceiveTokenOpenId.IOPAccessToken tokenobj;
                    ReceiveTokenOpenId token = new ReceiveTokenOpenId();
                    tokenobj = token.GetAccessToken(code.ToString(), ref uname, ref displayUname, ref IsIOPTimeOutException, ref isValidToken);
                    if (isValidToken)
                    {
                        if (tokenobj != null)
                        {
                            HttpCookie IOPAccessToken = new HttpCookie("IOPAccessToken");
                            IOPAccessToken.HttpOnly = true;
                            IOPAccessToken.Value = tokenobj.access_token;
                            IOPAccessToken.Expires = DateTime.Now.Add(new TimeSpan(0, 0, tokenobj.expires_in));
                            Response.Cookies.Add(IOPAccessToken);

                            HttpCookie IOPRefreshToken = new HttpCookie("IOPRefreshToken");
                            IOPRefreshToken.HttpOnly = true;
                            IOPRefreshToken.Value = tokenobj.refresh_token;
                            IOPRefreshToken.Expires = DateTime.Now.Add(new TimeSpan(2, 0, 0));
                            Response.Cookies.Add(IOPRefreshToken);

                            HttpCookie IOPIDToken = new HttpCookie("IOPIDToken");
                            IOPIDToken.HttpOnly = true;
                            IOPIDToken.Value = tokenobj.id_token;
                            IOPIDToken.Expires = DateTime.Now.Add(new TimeSpan(0, 0, tokenobj.expires_in));
                            Response.Cookies.Add(IOPIDToken);

                            SessionVarRetriever.UserName = uname;
                            SessionVarRetriever.DisplayUserName = displayUname;
                            MembershipUser user = Membership.GetUser(uname);
                            return;
                        }
                    }
                }

                catch (WebException ex)
                {
                    string errorCode = "";

                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        errorCode = ((int)response.StatusCode).ToString();
                    }
                    else
                    {
                        errorCode = "connectfailure";
                    }

                    Response.Redirect("~/Account/Login.aspx", false);
                    IsIOPTimeOutException = false;
                    return;
                }
                catch (Exception ex)
                {
                    Response.Redirect("~/Account/Login.aspx", false);
                    IsIOPTimeOutException = false;
                    return;
                }

            }
            if (IsIOPTimeOutException)
            {
                IsIOPTimeOutException = false;
            }

            if (Request.QueryString["HavenReturnUrl"] != null)
            {
                HttpCookie HavenRedirectURL = new HttpCookie("HavenRedirectURL");
                HavenRedirectURL.HttpOnly = true;
                HavenRedirectURL.Value = Request.QueryString["HavenReturnUrl"];
                HavenRedirectURL.Expires = DateTime.Now.Add(new TimeSpan(0, 0, 300));
                Response.Cookies.Add(HavenRedirectURL);
            }

            if (Request.QueryString["MITSReturnUrl"] != null)
            {
                HttpCookie MITSRedirectURL = new HttpCookie("MITSRedirectURL");
                MITSRedirectURL.HttpOnly = true;
                MITSRedirectURL.Value = Request.QueryString["MITSReturnUrl"];
                MITSRedirectURL.Expires = DateTime.Now.Add(new TimeSpan(0, 0, 300));
                Response.Cookies.Add(MITSRedirectURL);
            }
        }
    }
    private string BuildQueryString(IDictionary<string, string> parameters)
    {
        var list = new List<string>();

        foreach (var parameter in parameters)
        {
            list.Add(string.Format("{0}={1}", parameter.Key, HttpUtility.UrlEncode(parameter.Value)));
        }

        return string.Join("&", list);
    }

    public void GetAuthorizationCode()
    {
        SessionVarRetriever.IsOhID = true;

        string clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];
        string redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];
        string authorityAddr = System.Configuration.ConfigurationManager.AppSettings["Authority"];

        var parameters = new Dictionary<string, string>
                {
                    { "response_type", "code" },
                    { "client_id", clientId },
                    { "redirect_uri", redirectUri  },
                    { "prompt", "login"},
                    { "scope", "openid profile sub userid firstname lastname email mobile eidmlast4ssn telephoneNumber eidmaliases initials suffix eidmbirthdate offline_access"},
                    { "state", "OH" }
                };
        var requestUrl = string.Format("{0}?{1}", authorityAddr, BuildQueryString(parameters));

        Response.Redirect(requestUrl);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Page.Validate("LoginUserValidationGroup");
        if (!Page.IsValid) return;

        MembershipUser user = Membership.GetUser(txtUserName.Text);
        SessionVarRetriever.UserIdSelected = Helper.GetUserId(txtUserName.Text).ToString();
        if (user == null)
        {
            lblErrormessage.Visible = true;
            return;
        }

        if (user.IsLockedOut)
        {
            AccountLockedEvent accountLockedEvent = new AccountLockedEvent("Account Locked", this, txtUserName.Text);
            accountLockedEvent.Raise();
        }
        bool isInactivated = Helper.IsPasswordExpiredAndInactivated(txtUserName.Text);

        // For MITS users         
        if (Helper.IsMITSuserForce_Password_Reset(Helper.GetUserId(user.UserName).ToString()))
        {
            if (!Helper.IsTempPasswordSent(Helper.GetUserId(user.UserName).ToString()))
            {
                Response.Redirect("~/Account/FirstLoginDetails.aspx");
            }
        }
        else
        {

            if (Membership.ValidateUser(txtUserName.Text, txtPassword.Text))
            {
                FormsAuthentication.SetAuthCookie(txtUserName.Text, true);
                SessionVarRetriever.UserName = txtUserName.Text;
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                DataSet ds = svc.GetUserAccountInformation(Helper.GetUserId(txtUserName.Text).ToString());
                if (Helper.HasRows(ds))
                {
                    SessionVarRetriever.DisplayUserName = ds.Tables[0].Rows[0]["Name"].ToString();
                    SessionVarRetriever.OhID = ds.Tables[0].Rows[0]["OhID"].ToString();
                    SessionVarRetriever.IsOhID = true;
                }

                Session["LogKey"] = Guid.NewGuid().ToString();

                if (SessionVarRetriever.IsOhID)
                {
                    FormsAuthentication.SetAuthCookie(SessionVarRetriever.UserName, true);
                    string relink = "https://mesc.mpc-release.maximus.com/MESC_DEMO/Default.aspx";
#if DEBUG
                        relink = "http://localhost:64176/Account/Login.aspx?username=";
#endif
                    Response.Redirect(relink + SessionVarRetriever.UserName, false);

                     Session["LogKey"] = Guid.NewGuid().ToString();

                  
                    Response.Redirect("~/Default.aspx", false);



                }
                else
                {
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        Response.Redirect("http://localhost:64176/Account/login.aspx?username=" + SessionVarRetriever.UserName, false);
                    }
                }

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
        SessionVarRetriever.IsOhID = true;
        if (SessionVarRetriever.IsNewUser)
        {
            Response.Redirect(uri, false);
        }
        else
        {
            GetAuthorizationCode();
        }
    }
}