using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_ProviderFinancials : WorkflowPage
{
    private PDMSService.PDMSServiceClient _spa;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
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
        if (!IsPostBack)
        {
            if (Session["RegId"] != null)
            {
                this.RegistrationId = Convert.ToInt32(Session["RegId"]);
            }
            else if(PreviousPage != null)
            {
                this.RegistrationId = PreviousPage.RegistrationId;
                this.RegistrationStep = PreviousPage.RegistrationStep;
                this.CommandName = PreviousPage.CommandName;
                this.RegistrationIdSelected = PreviousPage.IsReadOnly ? PreviousPage.RegistrationId : 0;
                this.MedicaidID = PreviousPage.MedicaidID;
            }
            else if (Request.QueryString.Count > 0)
            {
                if (Request.QueryString.AllKeys.Contains("RegId"))
                    this.RegistrationId = int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["RegId"].ToString(), true));
                if (Request.QueryString.AllKeys.Contains("Step"))
                    this.RegistrationStep = int.Parse(Request["Step"]);

            }
            
            this.FillRegistrationData();
        }

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
}