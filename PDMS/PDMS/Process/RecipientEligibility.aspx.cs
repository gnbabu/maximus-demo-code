using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Process_Recipient_Eligibility : WorkflowPage
{
    private PDMSService.PDMSServiceClient _spa;


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
    public bool OwnershipChangedFlag { get; set; }
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
        // Page.Title = Resources.BrandingResource.Billing_and_Other_Services;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
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
        if (Helper.IsLoggedInUserInAdminRole() || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAgent) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.RecipientEligibility) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.InfantMortalityLeadEntityAgent))
        {
            EnableControls();
        }
        else
        {
            //DisableControls();
        }
    }
    protected override void OnPreRender(EventArgs e)
    {
        //Master.FindControl
        base.OnPreRender(e);
    }

    private void DisableControls()
    {
        pnlRecipientEligibility.Enabled = false;
    }

    private void EnableControls()
    {
        pnlRecipientEligibility.Enabled = true;
    }
    private void LoadData(int step)
    {

    }
}