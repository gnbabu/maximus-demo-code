using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_SearchClaims : WorkflowPage
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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
            //MedicaidID
            if (Request.QueryString.AllKeys.Contains("MedicaidID"))
                this.WorkflowPage.MedicaidID = Convert.ToString(int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["MedicaidID"].ToString(), true)));

            string MedicaidNumber = string.Empty;
            string regID = string.Empty;

            if (Session["RegId"] != null)
            {
                this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            }
            this.FillRegistrationData();
            MedicaidNumber = this.WorkflowPage.MedicaidID;
            regID = this.WorkflowPage.RegistrationId.ToString();
        }
    }

}