using MAXIMUS.Models.Data.PDMS;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;

public partial class Process_PaperApplication : RegistrationProvider
{
    public int PaperRequestQueueID
    {
        get
        {
            return ViewState["PaperRequestQueueID"] == null ? 0 : Convert.ToInt32(ViewState["PaperRequestQueueID"]);
        }
        set
        {
            ViewState["PaperRequestQueueID"] = value;
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

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Helper.IsLoggedInUserInAdminRole())
            {
                SessionVarRetriever.MyQueueSelectedRoleName =
                SessionVarRetriever.MyQueueSelectedRoleValue =
                SessionVarRetriever.UserIdSelected = null;
            }
            InitPage();
        }

        this.ucPaperRequestAddView.InsertEvent += new Views_PaperRequestAddView.InsertEventHandler(View_InsertEvent);
        this.ucPaperRequestAddView.AddProviderEvent += new Views_PaperRequestAddView.AddProviderEventHandler(View_NewRegistrationRequest);
        this.ucPaperRequestAddView.ConnectRegistrationEvent += new Views_PaperRequestAddView.ConnectRegistrationEventHandler(View_NewRegistrationRequest);
        this.ucPaperRequestAddView.ErrorEvent += new Views_PaperRequestAddView.ErrorEventHandler(View_ErrorEvent);
        this.ucPaperRequestAddView.ClearRequestEvent += new Views_PaperRequestAddView.ClearRequestEventHandler(View_ClearEvent);
        this.ucPaperRequestAddView.ViewExistingProviderEvent += new Views_PaperRequestAddView.ViewExistingProviderEventHandler(View_ExistingDetail);

        this.ucProviderAddView.InsertEvent += new Views_ProviderAddView.InsertEventHandler(View_NewRegistrationInsert);
        this.ucProviderAddView.CancelEvent += new Views_ProviderAddView.CancelEventHandler(View_CancelNewRegistrationRequest);
        this.ucProviderAddView.ValidationEvent += new Views_ProviderAddView.ValidationEventHandler(View_ValidationEventRequest);
        this.ucProviderAddView.KeepPopupOpenEvent += new Views_ProviderAddView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

        this.ucProviderManagementView.CancelWorkflowEvent += new UserControls_ProviderManagementView.CancelWorkflowEventHandler(View_RefreshPage);
        this.ucProviderManagementView.ErrorEvent += new UserControls_ProviderManagementView.ErrorEventHandler(View_ErrorEvent);

        this.ucPaperDocView.ErrorEvent += new Views_PaperDocumentView.ErrorEventHandler(View_ErrorEvent);


    }
    #endregion

    #region Private Methods

    private void InitPage()
    {
        Guid userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        PaperRequestQueueData data = new PaperRequestQueueData();
        PaperRequestQueueID = Request["qid"] == null ? 0 : Convert.ToInt32(Request["qid"].ToString());

        data.UserID = userID;
        data.PaperRequestQueueID = PaperRequestQueueID;
        this.lblPageTitle.Text = PaperRequestQueueID == 0 ? Resources.BrandingResource.PAPER_APPLICATION_ADD_TITLE : Resources.BrandingResource.PAPER_APPLICATION_EDIT_TITLE;
        this.ucPaperRequestAddView.InitView(data);
        if (PaperRequestQueueID > 0)
        {
            this.ucPaperDocView.InitView(PaperRequestQueueID);
        }
        else
        {
            this.ucPaperDocView.Visible = false;
        }
    }

    private void View_NewRegistrationRequest(PaperRequestQueueData queueData)
    {
        this.ucPaperRequestAddView.EnableDetail(false);

        ProviderManagerData keyData = new ProviderManagerData();
        keyData.UserID = queueData.UserID;
        keyData.PaperRequestQueueID = queueData.PaperRequestQueueID;
        keyData.TaxID = queueData.TaxID;
        keyData.ReferralID = queueData.ReferralID;
        keyData.RegID = queueData.RegID;
        keyData.WorkflowIDRequested = queueData.WorkflowRequestedID;
        keyData.ConvertedProvider = queueData.ConvertedProvider;
        keyData.ApplicationTypeID = queueData.ApplicationTypeID;
        this.ucProviderAddView.InitView(keyData);
        this.pnlNewRegistration.Visible = true;
        this.mpe.Show();
    }


    private void View_NewRegistrationInsert(ProviderManagementData data)
    {
        (this.Page as RegistrationProvider).RegistrationId = data.RegID;
        (this.Page as RegistrationProvider).IsReadOnly = false;
    }

    private void View_ClearEvent()
    {
        ClearErrorMessages();
        this.lblInformationalMessage.Text = string.Empty;
    }

    private void View_InsertEvent(PaperRequestQueueData data)
    {
        PaperRequestQueueID = data.PaperRequestQueueID;
        this.ucPaperRequestAddView.PaperRequestQueueID = PaperRequestQueueID;
        this.ucPaperDocView.PaperRequestQueueID = PaperRequestQueueID;
        ClearErrorMessages();
        lblInformationalMessage.Text = "Your request was successfully inserted.";
        this.ucPaperDocView.Visible = true;
    }

    private void View_ExistingDetail(PaperRequestQueueData data)
    {
        this.ucPaperRequestAddView.EnableDetail(false);

        ProviderManagerData keyData = new ProviderManagerData();
        keyData.RegID = data.RegID;
        keyData.ReferralID = data.ReferralID;
        keyData.PaperRequestQueueID = data.PaperRequestQueueID;
        this.ucProviderManagementView.InitView(keyData);
        this.ucProviderManagementView.Visible = true;
    }

    private void View_RefreshPage()
    {
       //TODO:
    }

    private void View_ErrorEvent(Dictionary<string, string> lstErrors)
    {
        ClearErrorMessages();
        SetValidationErrors(lstErrors);
    }


    private void View_ValidationEventRequest()
    {
        View_KeepPopupOpen();
    }

    private void View_KeepPopupOpen()
    {
        this.mpe.Show();
    }

    private void View_CancelNewRegistrationRequest()
    {
        this.mpe.Hide();
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

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    #endregion

}