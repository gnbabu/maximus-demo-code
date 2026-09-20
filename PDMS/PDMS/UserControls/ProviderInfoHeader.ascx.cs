using System;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ProviderInfoHeader : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region SVC
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }
    #endregion
    #region Properties
    public string TaskName { get; set; }
    public string ReadOnlyOrEdit { get; set; }
    public string StatusDate { get; set; }
    public string ValidationGroup { set { vsProviderInfoHeader.ValidationGroup = value; } }
    #endregion

    public void LoadData(int RegistrationPageType)
    {
        txtRegistrationPageType.Text = RegistrationPageType.ToString();

        switch (RegistrationPageType)
        {
            case CON.RegistrationPageType.Identification:
                LoadIdentification();
                break;
            case CON.RegistrationPageType.LicensesClassifications:
                LoadLicensesClassifications();
                break;
            case CON.RegistrationPageType.PracticeLocations:
                LoadPracticeLocations();
                break;
            case CON.RegistrationPageType.GroupAffiliations:
                LoadGroupAffiliations();
                break;
            case CON.RegistrationPageType.OwnerInformation:
                LoadOwnerInformation();
                break;
            case CON.RegistrationPageType.SubstituteW9Form:
                LoadSubstituteW9Form();
                break;
            case CON.RegistrationPageType.ACHAuthorization:
                LoadACHAuthorization();
                break;
            case CON.RegistrationPageType.Agreements:
                LoadAgreements();
                break;
            default:
                break;
        }
    }

    private void LoadDetails()
    {
        DataSet pro = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        DataSet med = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAID");
        DataSet reg = svc.SelectRegistration(this.WorkflowPage.RegistrationId);

        LC125.Text = Helper.HasRows(pro) ? Helper.GetData("NAME", pro.Tables[0]) : string.Empty;
        LC126.Text = Helper.HasRows(med) ? Helper.GetData("MEDICAID_NUMBER", med.Tables[0]) : string.Empty;
        LC127.Text = TaskName;
        LC128.Text = Helper.HasRows(reg) ? Helper.GetData("REGISTRATION_STATUS_TYPE", reg.Tables[0]) : string.Empty;
        LC129.Text = Helper.HasRows(reg) ? String.Format("MM/dd/yyyy", StatusDate) : string.Empty;
        LC130.Text = ReadOnlyOrEdit;
        SetupAssignedTo();
    }

    private void LoadIdentification()
    {

    }

    private void LoadLicensesClassifications()
    {

    }

    private void LoadPracticeLocations()
    {

    }

    private void LoadGroupAffiliations()
    {

    }

    private void LoadOwnerInformation()
    {

    }

    private void LoadSubstituteW9Form()
    {

    }

    private void LoadACHAuthorization()
    {

    }

    private void LoadAgreements()
    {

    }

    private void SetupAssignedTo()
    {
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
        {
            DataSet ds = svc.SelectActiveUsersByRole("WHAT IS THE ROLE???");
            if (Helper.HasRows(ds))
            {
                ddlAssignedTo.DataValueField = "UserId";
                ddlAssignedTo.DataTextField = "UserName";
                ddlAssignedTo.DataSource = ds.Tables[0];
                ddlAssignedTo.DataBind();
                ddlAssignedTo.Items.Insert(0, "N/A");
            }
        }
    }
}