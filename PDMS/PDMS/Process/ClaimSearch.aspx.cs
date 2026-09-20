using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_ClaimSearch : WorkflowPage
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
            //MedicaidID
            if (Request.QueryString.AllKeys.Contains("MedicaidID"))
                this.WorkflowPage.MedicaidID = Convert.ToString(int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["MedicaidID"].ToString(), true)));

            //if (Session["RegId"] != null)
            //{
            //    this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            //}
            //else if (PreviousPage != null)
            //{
            //    this.RegistrationId = PreviousPage.RegistrationId;
            //    this.RegistrationStep = PreviousPage.RegistrationStep;
            //    this.CommandName = PreviousPage.CommandName;
            //    this.RegistrationIdSelected = PreviousPage.IsReadOnly ? PreviousPage.RegistrationId : 0;
            //    this.MedicaidID = PreviousPage.MedicaidID;
            //}
            //else if (Request.QueryString.Count > 0)
            //{
            //    if (Request.QueryString.AllKeys.Contains("RegId"))
            //        this.RegistrationId = int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["RegId"].ToString(), true));
            //    if (Request.QueryString.AllKeys.Contains("Step"))
            //        this.RegistrationStep = int.Parse(Request["Step"]);

            //}

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