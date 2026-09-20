using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_ORProviderSearch : WorkflowPage
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
        if (Request.QueryString.Count > 0)
        {
            ProgressBarPanel.Visible = false;
        }
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
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
}