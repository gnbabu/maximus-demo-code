using MAXIMUS.Models.Data.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Services;
using System.Web.UI;

public partial class Process_ProviderOperatorHome : RegistrationProvider
{
    public UserAccountInformation Model { get; set; }

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
   
        if (!Page.IsPostBack)
        {
            InitPageForUser();
        }


        this.ucProviderOperatorView.NextInQueueEvent += new Views_ProviderOperatorView.NextInQueueEventtHandler(View_NewQueueAssignment);
        this.ucProviderOperatorView.SelectMyProviderEvent += new Views_ProviderOperatorView.SelectMyProviderEventHandler(View_Detail);
        this.ucProviderOperatorView.ErrorEvent += new Views_ProviderOperatorView.ErrorEventHandler(View_ErrorEvent);

        this.ucProviderManagementView.CancelWorkflowEvent += new UserControls_ProviderManagementView.CancelWorkflowEventHandler(View_ResetSelectedRecord);
        this.ucProviderManagementView.EditProviderKeyFieldsEvent += new UserControls_ProviderManagementView.EditProviderKeyFieldsEventHandler(View_EditKeyFieldsRequest);
        this.ucProviderManagementView.ErrorEvent += new UserControls_ProviderManagementView.ErrorEventHandler(View_ErrorEvent);

        this.ucProviderAddView.KeyFieldUpdateEvent += new Views_ProviderAddView.KeyFieldUpdateEventHandler(View_ResetSelectedRecord);
        this.ucProviderAddView.CancelEvent += new Views_ProviderAddView.CancelEventHandler(View_CancelEvent);
        this.ucProviderAddView.ValidationEvent += new Views_ProviderAddView.ValidationEventHandler(View_KeepPopupOpen);
        this.ucProviderAddView.KeepPopupOpenEvent += new Views_ProviderAddView.KeepPopupOpenEventHandler(View_KeepPopupOpen);

    }
    
    private void View_NewQueueAssignment(int paperRequestQueueID)
    {
        //call init on paperrequest view
        Response.Redirect(string.Format("~/Process/PaperApplication.aspx?qid={0}", paperRequestQueueID));
    }

   
    private void InitPageForUser()
    {
        this.ucProviderOperatorView.InitView();
   }
    
    private void View_Detail(ProviderManagerData keyData)
    {
        if (keyData.RegID == 0 && keyData.PaperRequestQueueID ==0)
            return;

        if (keyData.RegID > 0)
        {
            this.ucProviderManagementView.InitView(keyData);
            this.ucProviderManagementView.Visible = true;
        }
        else if (keyData.PaperRequestQueueID > 0)
        {
            Response.Redirect(string.Format("~/Process/PaperApplication.aspx?qid={0}", keyData.PaperRequestQueueID));
        }
    }

    private void View_EditKeyFieldsRequest(ProviderManagerData keyData)
    {
        this.lblKFETitle.Text = Resources.BrandingResource.POPUP_TITLE_EDIT_KEY_REGISTRATION_FIELDS;
        this.ucProviderAddView.InitView(keyData);
        this.mpe.Show();
    }

    private void View_ResetSelectedRecord()
    {
        this.mpe.Hide();
        this.ucProviderManagementView.Visible = false;
        this.ucProviderOperatorView.RefreshData();
    }

    private void View_KeepPopupOpen()
    {
        this.mpe.Show();
    }

    private void View_CancelEvent()
    {
        this.mpe.Hide();
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

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    public class Data
    {
        public string ColumnName = "";
        public int Value = 0;
        public string Category = "";
        public Data(string category, int value, string columnName)
        {
            Category = category;
            ColumnName = columnName;
            Value = value;
        }
    }  
    
}

     