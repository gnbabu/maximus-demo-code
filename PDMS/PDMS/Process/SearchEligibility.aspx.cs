using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_SearchEligibility : WorkflowPage
{
    public bool OwnershipChangedFlag { get; set; }

    private PDMSService.PDMSServiceClient _spa;
    private DataTable dtProvider = null;

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
        Guid threadId = Guid.NewGuid();
        string apiusr = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

        try
        {
            // Find hidden fields in the master page
            HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
            HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");

            CredentialHelper.APIToken apiToken = ApplicationCache.RestAPIAccessToken();

            if (hdnAPIToken != null)
            {
                hdnAPIToken.Value = apiToken.AccessToken;
            }

            if (hdnAPIRefreshToken != null)
            {
                hdnAPIRefreshToken.Value = apiToken.RefreshToken;
            }


            Logging log = new Logging(threadId, "Fetching token - Search Claims");
            log.CreateLogEntry("Token successfully generated and assigned to hidden access tokens for User:" + apiusr, Logging.LogPriority.Information);

        }
        catch (Exception ex)
        {
            // Log any errors that occur during token retrieval
            Logging log = new Logging(threadId, "Fetching token - Search Claims");
            log.CreateLogEntry("Token successfully generated and assigned to hidden access tokens for User:" + apiusr, Logging.LogPriority.Error);
        }

        if (!IsPostBack)
        {
            string MedicaidNumber = string.Empty;
            string regID = string.Empty;

            if (Session["RegId"] != null)
            {
                this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            }
            this.FillRegistrationData();

        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

}