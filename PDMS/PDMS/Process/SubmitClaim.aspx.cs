using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_SubmitClaim : WorkflowPage
{
    private Guid m_threadId;
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
        CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
        HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
        HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");
        hdnAPIToken.Value = APIToken.AccessToken;
        hdnAPIRefreshToken.Value = APIToken.RefreshToken;

        if (!IsPostBack)
        {
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