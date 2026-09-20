using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;

public partial class Process_ProviderDetailsNew : RegistrationProvider, IUserAccountInformationView
{
    #region Properties
    private UserAccountInformationPresenter _presenter;

    public UserAccountInformationPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new UserAccountInformationPresenter(this);
            }

            return _presenter;
        }
    }
    #endregion

    #region Page Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())

            Page.Theme = "Modernization";
        else
            Page.Theme = "Default";

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitPageForUser();
        }

        this.ucProviderManagementView.EditProviderKeyFieldsEvent += new UserControls_ProviderManagementView.EditProviderKeyFieldsEventHandler(View_NewRegistrationRequest);
        this.ucProviderManagementView.AddODMorODAMedicaidEvent += new UserControls_ProviderManagementView.AddODMorODAMedicaidEventHandler(View_NewRegistrationRequest);
        this.ucProviderManagementView.CancelWorkflowEvent += new UserControls_ProviderManagementView.CancelWorkflowEventHandler(View_ResetSelectedRecord);
        this.ucProviderManagementView.CancelExistingServicesEvent += new UserControls_ProviderManagementView.CancelExistingServicesEventHandler(View_CancelEvent);
        this.ucProviderManagementView.ErrorEvent += new UserControls_ProviderManagementView.ErrorEventHandler(View_ErrorEvent);
        this.ucProviderManagementView.ProviderTypeChangeEvent += new UserControls_ProviderManagementView.ProviderTypeChangeHandler(View_ProviderTypeChangeRequest);
    }

    private void View_NewRegistrationRequest(ProviderManagerData keyData)
    {
        (this.Page as RegistrationProvider).RegistrationId = keyData.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
        //this.lblNewRegistration.Text = keyData.KeyFieldEditRequest ? Resources.BrandingResource.POPUP_TITLE_EDIT_KEY_REGISTRATION_FIELDS
        //                                        : keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationRevalidation ? Resources.BrandingResource.POPUP_TITLE_REVALIDATE_REGISTRATION
        //                                        : keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationUpdateProvider ? Resources.BrandingResource.POPUP_TITLE_UPDATE_REGISTRATION
        //                                        : Resources.BrandingResource.POPUP_TITLE_NEW_REGISTRATION;
        ////this.ucProviderSummaryView.EnablePage(false);
        //this.ucProviderAddView.InitView(keyData);
        //this.pnlNewRegistration.Visible = true;
        //this.mpe.Show();
        if (keyData.KeyFieldEditRequest)
            Response.Redirect("~/Process/NewProvider.aspx?EditKeyFieldDataRequest=true&RegID=" + keyData.RegID.ToString());
        if(keyData.IsAddODMorODAMedicaid)
            Response.Redirect("~/Process/NewProvider.aspx?AddODMorODAMedicaid=true&RegID=" + keyData.RegID.ToString());
    }
    private void View_ProviderTypeChangeRequest(ProviderManagerData keyData)
    {
        (this.Page as RegistrationProvider).RegistrationId = keyData.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
        //this.lblNewRegistration.Text = keyData.KeyFieldEditRequest ? Resources.BrandingResource.POPUP_TITLE_EDIT_KEY_REGISTRATION_FIELDS
        //                                        : keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationRevalidation ? Resources.BrandingResource.POPUP_TITLE_REVALIDATE_REGISTRATION
        //                                        : keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationUpdateProvider ? Resources.BrandingResource.POPUP_TITLE_UPDATE_REGISTRATION
        //                                        : Resources.BrandingResource.POPUP_TITLE_NEW_REGISTRATION;
        ////this.ucProviderSummaryView.EnablePage(false);
        //this.ucProviderAddView.InitView(keyData);
        //this.pnlNewRegistration.Visible = true;
        //this.mpe.Show();
        if (keyData.ProviderTypeChangeRequest)
            Response.Redirect("~/Process/NewProvider.aspx?ProviderTypeChangeRequest=true&RegID=" + keyData.RegID.ToString());

    }
    #endregion

    #region Presenter Events




    #endregion

    #region Private Methods

    private void InitPageForUser()
    {
        Guid userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        presenter.Init(userID);
        //  TODO: EDV Pass on REG ID to ProviderDetails.
        ProviderManagerData keyData = LoadKeyFields(userID);
        keyData.RegID = int.Parse(Request["regID"]);
        View_ProviderDetail(keyData);
    }

    private ProviderManagerData LoadKeyFields(Guid userID)
    {
        ProviderManagerData keyData = new ProviderManagerData();
        keyData.UserID = userID;
        return keyData;
    }

    private void View_CancelEvent()
    {
    }
     
    private void View_ProviderDetail(ProviderManagerData keyData)
    {
        if (keyData.RegID == null || keyData.RegID == 0)
            return;

        this.ucProviderManagementView.InitView(keyData);

        this.ucProviderManagementView.Visible = true;
    }

    

    private void View_RefreshPage()
    {
        InitPageForUser();
    }

    private void View_ErrorEvent(Dictionary<string, string> lstErrors)
    {
        ClearErrorMessages();
        SetValidationErrors(lstErrors);
    }

    private void SetValidationErrors(Dictionary<string, string> lstErrors)
    {
        foreach (var pair in lstErrors)
        {
            AddErrorMessage(pair.Value);
        }
    }

    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }

    private void View_ResetSelectedRecord()
    {

        this.ucProviderManagementView.Visible = false;
        this.InitPageForUser();
        
    }

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    
    #endregion

    public void SetUserInformation(DataSet ds)
    {
        
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            View_ErrorEvent(presenter.ErrorList);
        }
    }

    public UserAccountInformation Model { get; set; }
}