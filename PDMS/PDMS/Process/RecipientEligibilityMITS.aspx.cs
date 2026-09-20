using MAXIMUS.Core.Libraries;
using System;
using System.Web;

public partial class Process_RecipientEligibilityMITS : System.Web.UI.Page
{
    public string RecipientEligibilityToken = "";
    public string RecipientEligibilityProviderId = "";
    public string RecipientEligibilityURL = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        string ReturnURL = "";
        string url = "";
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        var flag = AppSettings.Get("RedirectToPNM3BMemberEligbility");
        if (flag == "1" || flag == "2") //Go to PNM
        {
            url = string.Format("~/Process/FinancialProviderInformation.aspx");
        }
        else
        {
            // check for Haven Redirect
            HttpCookie MITSRedirectURL = Request.Cookies["MITSRedirectURL"];
            if (MITSRedirectURL != null)
            {
                try
                {
                    if (MITSRedirectURL.Value.Length > 4)
                    {  // this is a redirect on an expired or missing token from an embedded PDFlink
                        ReturnURL = Server.UrlEncode(MITSRedirectURL.Value);
                    }
                    // get rid of cookie so we don't go back
                    MITSRedirectURL.Value = null;
                    MITSRedirectURL.Expires = DateTime.Now.AddDays(-10);
                    Response.SetCookie(MITSRedirectURL);
                    Response.Cookies.Remove("MITSRedirectURL");

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            if (Request.QueryString["MedicaidId"] != null)
            {
                this.RecipientEligibilityProviderId = Request.QueryString["MedicaidId"];
            }
            // haven handoff data
            this.RecipientEligibilityURL = System.Configuration.ConfigurationManager.AppSettings["RecipientEligibilityURL"];
            this.RecipientEligibilityToken = MaximusJWT.GetNewToken(HttpContext.Current.User.Identity.Name, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), System.Configuration.ConfigurationManager.AppSettings["PNMSecretKey"]);

            // save token to db
            svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.RecipientEligibilityToken);

            url = this.RecipientEligibilityURL + "?AuthToken=" + this.RecipientEligibilityToken + "&ProviderId=" + this.RecipientEligibilityProviderId + "&ReturnURL=" + ReturnURL;
        }
        Response.Redirect(url);
    }
}