using System;
using System.Web;

public partial class Account_Logout : System.Web.UI.Page
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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Remove the token from the DB on logout
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty);
            System.Web.Security.FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
            Response.Redirect(Helper.RedirectLoginURL(), false);

        }
    }

    protected void btnLogoutOk_Clicked(object sender, EventArgs e)
    {
        string logoutredirectUri = System.Configuration.ConfigurationManager.AppSettings["OHredirectUri"];

       
        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();                  // Remove the session entries
        System.Web.Security.FormsAuthentication.SignOut();
        HttpContext.Current.Session.RemoveAll();
        Request.Cookies.Clear();
        Response.Cookies.Clear();
        Response.Redirect(logoutredirectUri, false);
    }
}