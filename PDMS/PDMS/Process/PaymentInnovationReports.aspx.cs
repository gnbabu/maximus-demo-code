using MAXIMUS.Core.Libraries;
using System;
using System.Web;

public partial class Process_PaymentInnovationReports : System.Web.UI.Page
{
    public string PaymentInnovationsToken = "";
    public string PaymentInnovationsProviderId = "";
    public string PaymentInnovationsURL = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        string ReturnURL = "";
        string url = "";
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();

        // check for Haven Redirect
        HttpCookie HavenRedirectURL = Request.Cookies["HavenRedirectURL"];
        if (HavenRedirectURL != null)
        {
            try
            {
                if (HavenRedirectURL.Value.Length > 4)
                {  // this is a redirect on an expired or missing token from an embedded PDFlink
                    ReturnURL = Server.UrlEncode(HavenRedirectURL.Value);
                }
                // get rid of cookie so we don't go back
                HavenRedirectURL.Value = null;
                HavenRedirectURL.Expires = DateTime.Now.AddDays(-10);
                Response.SetCookie(HavenRedirectURL);
                Response.Cookies.Remove("HavenRedirectURL");

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        if (Request.QueryString["MedicaidId"] != null)
        {
            this.PaymentInnovationsProviderId = Request.QueryString["MedicaidId"];
        }
        // haven handoff data
        this.PaymentInnovationsURL = AppSettings.Get("PaymentInnovationsURL"); 
        
        this.PaymentInnovationsToken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);

        // save token to db
        svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.PaymentInnovationsToken);

        url = this.PaymentInnovationsURL + "?AuthToken=" + this.PaymentInnovationsToken + "&ProviderId=" + this.PaymentInnovationsProviderId + "&ReturnURL=" + ReturnURL;
        Response.Redirect(url);
    }
}