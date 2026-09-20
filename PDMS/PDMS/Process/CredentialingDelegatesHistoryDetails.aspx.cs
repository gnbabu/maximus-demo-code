using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_AffiliationHistoryDetails : System.Web.UI.Page
{
    private Guid m_threadId;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())

            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";
    }
    private Guid ThreadId
    {
        get
        {
            return this.m_threadId;
        }
        set
        {
            this.m_threadId = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Guid threadId = Guid.NewGuid();
        string apiusr = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

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

        if (!IsPostBack)
        {

        }
    }
}