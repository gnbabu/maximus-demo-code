using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_SearchPriorAuthorization : WorkflowPage
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

            CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
        HiddenField hdnAPIToken = (HiddenField)Page.Master.FindControl("hdnAccessToken");
        HiddenField hdnAPIRefreshToken = (HiddenField)Page.Master.FindControl("hdnRefreshToken");
        hdnAPIToken.Value = APIToken.AccessToken;
        hdnAPIRefreshToken.Value = APIToken.RefreshToken;


        if (!IsPostBack)
        {

            if (Session["RegId"] != null)
            {
                this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            }
            //MedicaidID
            if (Request.QueryString.AllKeys.Contains("MedicaidID"))
                this.WorkflowPage.MedicaidID = Convert.ToString(int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["MedicaidID"].ToString(), true)));

            this.FillRegistrationData();
        }
    }
}