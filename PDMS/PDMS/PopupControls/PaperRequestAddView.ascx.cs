using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Views_PaperRequestAddView : System.Web.UI.UserControl, IPaperRequestAddView
{
    
    #region Properties
    private PaperRequestAddPresenter _presenter;

    public PaperRequestAddPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new PaperRequestAddPresenter(this);
            }

            return _presenter;
        }
    }

    public PaperRequestQueueData Model { get; set; }

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

    public int MatchRegID
    {
        get
        {
            return ViewState["LatestRegID"] == null ? 0 : Convert.ToInt32(ViewState["LatestRegID"]);
        }
        set
        {
            ViewState["LatestRegID"] = value;
        }
    }

    public int ReferralID
    {
        get
        {
            return ViewState["ReferralID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralID"]);
        }
        set
        {
            ViewState["ReferralID"] = value;
        }
    }

    public bool PendingConvertedProvider
    {
        get
        {
            return ViewState["PendingConvertedProvider"] == null ? false : Convert.ToBoolean(ViewState["PendingConvertedProvider"]);
        }
        set
        {
            ViewState["PendingConvertedProvider"] = value;
        }
    }

    public DateTime? RevalidationDate
    {
        get
        {
            return (DateTime?)ViewState["RevalidationDate"];
        }
        set
        {
            ViewState["RevalidationDate"] = value;
        }
    }

    public bool RequestHasBeenValidated
    {
        get
        {
            return ViewState["RequestHasBeenValidated"] == null ? false : Convert.ToBoolean(ViewState["RequestHasBeenValidated"]);
        }
        set
        {
            ViewState["RequestHasBeenValidated"] = value;
        }
    }


    public string WaiverServicesDefaultProviderTypeName
    {
        get
        {
            return ViewState["WaiverServicesDefaultProviderTypeName"] == null ? string.Empty : ViewState["WaiverServicesDefaultProviderTypeName"].ToString();
        }
        set
        {
            ViewState["WaiverServicesDefaultProviderTypeName"] = value;
        }
    }

    #endregion

    #region Parent Page Events
    public delegate void InsertEventHandler(PaperRequestQueueData data);
    public event InsertEventHandler InsertEvent;

    public delegate void ErrorEventHandler(Dictionary<string, string> lstErrors);
    public event ErrorEventHandler ErrorEvent;

    public delegate void AddProviderEventHandler(PaperRequestQueueData data);
    public event AddProviderEventHandler AddProviderEvent;

    public delegate void ConnectRegistrationEventHandler(PaperRequestQueueData data);
    public event ConnectRegistrationEventHandler ConnectRegistrationEvent;

    public delegate void ViewExistingProviderEventHandler(PaperRequestQueueData data);
    public event ViewExistingProviderEventHandler ViewExistingProviderEvent;

    public delegate void ClearRequestEventHandler();
    public event ClearRequestEventHandler ClearRequestEvent;


    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void ddlApplicationType_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetApplicationDependentFields();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetCategoryDependentFields();
    }

    protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetProviderTypeDependentFields();
    }

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //If the selected value is Re-Enrollment or Update Enrollment, then Provider Number is required
        int requestTypeID = int.Parse(ddlRequestType.SelectedValue);
        valProvNumberReqd.Enabled = presenter.IsProviderNumberRequired(requestTypeID);
    }
    //protected void ddlSpecialty_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    this.GetSpecialtyDependentFields();

    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (ClearRequestEvent != null)
        {
            //remove current validation messages.
            ClearRequestEvent();
        }

        this.RequestHasBeenValidated = false;

        if (!Page.IsValid)
        {
                return;
        }

        this.Model = this.LoadModelFromForm();
        if (PaperRequestQueueID == 0)
        {
            presenter.ValidateNewPaperRequest();
        }
        else
        {
            presenter.ValidateExistingPaperRequest();
        }

    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearFormFields();
    }

    //Modal Popup responses
    protected void btnProcessYes_Click(object sender, EventArgs e)
    {
        if (PaperRequestQueueID > 0)
        {
            if (this.ddlRequestType.SelectedIndex > 0 && this.ddlRequestType.SelectedValue == CON.PaperRequestType.InitialEnrollment.ToString())
            { 
                if (AddProviderEvent != null)
                {
                    PaperRequestQueueData data = new PaperRequestQueueData();
                    data.PaperRequestQueueID = PaperRequestQueueID;
                    data.ReferralID = ReferralID;
                    data.ConvertedProvider = this.PendingConvertedProvider;  //should always be false - would have failed validation for initial enrollment

                    using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                    {
                        data.WorkflowRequestedID = svc.GetWorkflowInstance(data.ApplicationTypeID, data.ProviderCategoryTypeID, data.ProviderTypeID,
                            null, false, false, false, false); 
                    }

                    AddProviderEvent(data);
                }
            }
            else if (this.PendingConvertedProvider)
            { //for converted providers need to show pop up too.
                if (ConnectRegistrationEvent != null)
                {
                    PaperRequestQueueData data = new PaperRequestQueueData();
                    data.PaperRequestQueueID = PaperRequestQueueID;
                    data.ReferralID = ReferralID;
                    data.RegID = MatchRegID;
                    data.ProviderCategoryTypeID = Convert.ToInt32(this.ddlCategory.SelectedValue);
                    data.ProviderTypeID = Convert.ToInt32(this.ddlProviderType.SelectedValue);
                    data.ConvertedProvider = this.PendingConvertedProvider;
                    data.ApplicationTypeID = Convert.ToInt32(this.ddlApplicationType.SelectedValue);

                    bool revalDue = false;
                    if (this.RevalidationDate.HasValue)
                        revalDue = Helper.IsRevalDue(this.RevalidationDate.Value.ToString());

                    using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
                    {
                        data.WorkflowRequestedID = svc.GetWorkflowInstance(data.ApplicationTypeID,  int.Parse(this.ddlCategory.SelectedValue), 
                            int.Parse(this.ddlProviderType.SelectedValue),
                            null, revalDue, false, true, false);
                    }

                    AddProviderEvent(data);
                }
            }
            else
            {
                if (MatchRegID > 0 && ViewExistingProviderEvent != null)
                {
                    PaperRequestQueueData data = new PaperRequestQueueData();
                    data.PaperRequestQueueID = PaperRequestQueueID;
                    data.RegID = MatchRegID;
                    data.ReferralID = ReferralID;
                   //continue here data.UserID = User
                    ViewExistingProviderEvent(data);
                }
            }
        }
        this.mpe.Hide();
    }
    protected void btnProcessNo_Click(object sender, EventArgs e)
    {
        this.mpe.Hide();
    }

    #endregion

    #region Public Events
    public void InitView(PaperRequestQueueData keyData)
    {
        this.PaperRequestQueueID = keyData.PaperRequestQueueID;
        this.SetFieldVisibility();

        presenter.Init(keyData);

        this.SetFieldEditability();
    }

    public void EnableDetail(bool enable)
    {
        this.pnlPaperDetails.Enabled = enable;
    }
    #endregion

    #region Presenter Events

    public void SetPaperRequestTypes(DataSet types)
    {
        this.ddlRequestType.DataSource = types;
        this.ddlRequestType.DataValueField = "PAPER_REQUEST_TYPE_ID";
        this.ddlRequestType.DataTextField = "PAPER_REQUEST_TYPE_NAME";
        this.ddlRequestType.DataBind();
        this.ddlRequestType.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetPaperDocumentTypes(DataSet types)
    {
        this.ddlDocumentType.DataSource = types;
        this.ddlDocumentType.DataValueField = "PAPER_REQUEST_DOCUMENT_TYPE_ID";
        this.ddlDocumentType.DataTextField = "DocumentTypeWithDescription";
        this.ddlDocumentType.DataBind();
        this.ddlDocumentType.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetPaperRequestStatusTypes(DataSet types)
    {
        this.ddlRequestStatus.DataSource = types;
        this.ddlRequestStatus.DataValueField = "PAPER_REQUEST_STATUS_TYPE_ID";
        this.ddlRequestStatus.DataTextField = "PAPER_REQUEST_STATUS_TYPE_NAME";
        this.ddlRequestStatus.DataBind();
        this.ddlRequestStatus.Items.Insert(0, new ListItem("New", "0"));

        if (PaperRequestQueueID == 0)
            this.ddlRequestStatus.Enabled = false;
    }

    public void SetPaperApplicationTypes(DataSet types)
    {
        this.ddlApplicationType.DataSource = types;
        this.ddlApplicationType.DataValueField = "APPLICATION_TYPE_ID";
        this.ddlApplicationType.DataTextField = "APPLICATION_TYPE_NAME";
        this.ddlApplicationType.DataBind();
        this.ddlApplicationType.Items.Insert(0, new ListItem("", "0"));
    }
    
    public void SetProviderCategories(DataSet categories)
    {
        this.ddlCategory.DataSource = categories;
        this.ddlCategory.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
        this.ddlCategory.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
        this.ddlCategory.DataBind();
        this.ddlCategory.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetProviderTypes(DataSet types)
    {
        this.ddlProviderType.DataSource = types;
        this.ddlProviderType.DataValueField = "PROVIDER_TYPE_ID";
        this.ddlProviderType.DataTextField = "PROVIDER_TYPE_NAME";
        this.ddlProviderType.DataBind();
        this.ddlProviderType.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetSpecialtyTypes(DataSet data)
    {
        //this.ddlSpecialty.DataSource = data;
        //this.ddlSpecialty.DataValueField = "SPECIALTY_TYPE_ID";
        //this.ddlSpecialty.DataTextField = "SPECIALTY_TYPE_NAME";
        //this.ddlSpecialty.DataBind();
        //this.ddlSpecialty.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetTaxonomyTypes(DataView dv)
    {
        this.ddlTaxonomy.DataSource = dv;
        this.ddlTaxonomy.DataValueField = "TAXONOMY_TYPE_ID";
        this.ddlTaxonomy.DataTextField = "TaxonomyNameWithCode";
        this.ddlTaxonomy.DataBind();
        this.ddlTaxonomy.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetDocumentTypes(DataSet data)
    {
        this.ddlTaxonomy.DataSource = data;
        this.ddlTaxonomy.DataValueField = "TAXONOMY_TYPE_ID";
        this.ddlTaxonomy.DataTextField = "TaxonomyNameWithCode";
        this.ddlTaxonomy.DataBind();
        this.ddlTaxonomy.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetPaperRequestDetail(PaperRequestQueueData data)
    {
        this.PaperRequestQueueID = data.PaperRequestQueueID;
        this.MatchRegID = data.RegID;
        this.ReferralID = data.ReferralID;
        this.PendingConvertedProvider = data.ConvertedProvider;
        this.RevalidationDate = data.RevalidationDate;

        this.GetDropDownValues(data);

        if (this.ddlRequestType.Items.Count > 0)
        {
            this.ddlRequestType.SelectedValue = data.RequestTypeID.ToString();
        }
        if (this.ddlDocumentType.Items.Count > 0)
        {
            this.ddlDocumentType.SelectedValue = data.DocumentTypeID.ToString();
        }
        this.txtDocHandle.Text = data.DocumentHandleID.ToString();
        if (this.ddlRequestStatus.Items.Count > 0)
        {
            this.ddlRequestStatus.SelectedValue = data.RequestStatusTypeID.ToString();
        }
        if (ddlCategory.Items.Count > 0)
        {
            this.ddlCategory.SelectedValue = data.ProviderCategoryTypeID.ToString();
        }
        if (ddlApplicationType.Items.Count > 0)
        {
            this.ddlApplicationType.SelectedValue = data.ApplicationTypeID.ToString();
        }
        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedValue = data.ProviderTypeID.ToString();
        }
        //if (this.ddlSpecialty.Items.Count > 0)
        //{
        //    this.ddlSpecialty.SelectedValue = data.SpecialtyTypeID.ToString();
        //}
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedValue = data.TaxonomyTypeID.ToString();
        }
        this.txtTaxID.Text = data.TaxID;
        this.txtNPI.Text = data.NPI;
        this.txtMedicaidID.Text = data.MedicaidID;
        this.txtZipCode.Text = data.ZipCode;
        this.txtZipCodeExt.Text = data.ZipExt;
        this.txtComments.Text = data.Comments;

        presenter.RequestExceptionLog();

    }

    public void SetInsertResults()
    {
        this.btnSave.Enabled = false;
        PaperRequestQueueData data = this.Model;
        if (InsertEvent != null)
        {
            InsertEvent(data);
        }

        presenter.RequestExceptionLog();
    }

    public void SetUpdateResults(PaperRequestQueueData data)
    {
        this.RequestHasBeenValidated = true;
        this.MatchRegID = data.RegID;
        this.ReferralID = data.ReferralID;
        this.PendingConvertedProvider = data.ConvertedProvider;
        this.RevalidationDate = data.RevalidationDate;

        presenter.RequestExceptionLog();
    }

    public void SetWaiverInfo(string name)
    {
        this.WaiverServicesDefaultProviderTypeName = name;
    }

    public void SetValidationSuccess()
    {
        if (PaperRequestQueueID == 0)
        {
            presenter.InsertPaperRequestQueueItem();
        }
        else
        {
            presenter.UpdatePaperRequestQueueItem();
        }
    }

    public void SetExceptionLog(DataSet ds)
    {
        this.divException.Visible = true;
        this.gvExceptions.DataSource = ds;
        this.gvExceptions.DataBind();

        int requestTypeID = Convert.ToInt32(this.ddlRequestType.SelectedValue);
        int statusTypeID = Convert.ToInt32(this.ddlRequestStatus.SelectedValue);
        if (statusTypeID == CON.PaperRequestStatusType.Rejected)
        {
            EnableDetail(false);
            return;
        }

        if (PaperRequestQueueID > 0 && this.gvExceptions.Rows.Count == 0 && this.RequestHasBeenValidated)
        {
            //RequestHasBeenValidated is set to ensure that the paper request is ran through validation after initial load.
            this.lblMaintReg.Visible = requestTypeID == CON.PaperRequestType.InitialEnrollment ? false : true;
            this.lblNewReg.Visible = requestTypeID == CON.PaperRequestType.InitialEnrollment ? true : false;
            if (requestTypeID != CON.PaperRequestType.InitialEnrollment && MatchRegID == 0)
            {
                return;
            }
            this.mpe.Show();
        }
        else  if (PaperRequestQueueID > 0)
        {
            this.btnSave.Text = "Revalidate";
        }
    }
    
    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            if (ErrorEvent != null)
            {
                ErrorEvent(presenter.ErrorList);
                return;
            }
        }
    }

    #endregion

    #region Private Events
    
    private void SetFieldEditability()
    {
        this.txtDocHandle.Enabled = this.PaperRequestQueueID == 0;

        int requestStatus = this.ddlRequestStatus.SelectedIndex == -1 ? 0 : Convert.ToInt32(this.ddlRequestStatus.SelectedValue);
        bool isEnabled = requestStatus == CON.PaperRequestStatusType.Rejected ? false : true;
        EnableDetail(isEnabled);
    }

    private void SetFieldVisibility()
    {
        this.btnClear.Visible = this.PaperRequestQueueID == 0;
        this.divException.Visible = false;
    }

    private void ClearFormFields()
    {
        this.ddlTaxonomy.Items.Clear();
        //this.ddlSpecialty.Items.Clear();
        this.ddlProviderType.Items.Clear();
        this.ddlCategory.SelectedIndex = -1;
        this.ddlRequestType.SelectedIndex = -1;
        this.ddlDocumentType.SelectedIndex = -1;
        this.txtComments.Text = this.txtMedicaidID.Text = this.txtNPI.Text = this.txtTaxID.Text = this.txtZipCode.Text = this.txtZipCodeExt.Text = txtDocHandle.Text = string.Empty;
        this.divException.Visible = false;
        this.btnSave.Enabled = true;

        if (ClearRequestEvent != null)
        {
            ClearRequestEvent();
        }
    }


    private PaperRequestQueueData LoadModelFromForm()
    {
        PaperRequestQueueData data = new PaperRequestQueueData();

        data.PaperRequestQueueID = PaperRequestQueueID;
        data.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        data.RequestTypeID = this.ddlRequestType.SelectedIndex > -1 ? Convert.ToInt32(ddlRequestType.SelectedValue) : 0;
        data.RequestStatusTypeID = this.ddlRequestStatus.SelectedIndex > -1 ? Convert.ToInt32(ddlRequestStatus.SelectedValue) : 0;
        data.DocumentTypeID = this.ddlDocumentType.SelectedIndex > -1 ? Convert.ToInt32(ddlDocumentType.SelectedValue) : 0;
        data.DocumentHandleID = string.IsNullOrEmpty(this.txtDocHandle.Text.Trim()) ? 0 : Convert.ToInt32(this.txtDocHandle.Text.Trim());
        data.MedicaidID = txtMedicaidID.Text.Trim();
        data.TaxID = txtTaxID.Text.Trim();
        data.NPI = this.txtNPI.Text.Trim();
        data.ZipCode = this.txtZipCode.Text.Trim();
        data.ZipExt = this.txtZipCodeExt.Text.Trim();
        data.Comments = this.txtComments.Text.Trim();

        data.ProviderCategoryTypeID = ddlCategory.SelectedIndex > -1 ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
        data.ApplicationTypeID = ddlApplicationType.SelectedIndex > -1 ? Convert.ToInt32(ddlApplicationType.SelectedValue) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            data.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
            data.ProviderTypeName = ddlProviderType.SelectedIndex > -1 ? ddlProviderType.SelectedItem.Text : string.Empty;
        }
        //if (this.ddlSpecialty.Items.Count > 0)
        //{
        //    data.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        //}
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            data.TaxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        }

        if (PaperRequestQueueID == 0)
        {
            data.CreatedOn = DateTime.Now;
            data.CreatedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        }

        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        
        return data;
    }


    private void GetApplicationDependentFields()
    {
        int applicationTypeID = Convert.ToInt32(this.ddlApplicationType.SelectedValue);
        if (applicationTypeID > 0)
        {
            this.ddlCategory.Items.Clear();
            this.ddlProviderType.Items.Clear();
            //this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            presenter.RequestCategories(ddlApplicationType.SelectedIndex > -1 ? Convert.ToInt32(ddlApplicationType.SelectedValue) : 0);
        }

    }

    private void GetCategoryDependentFields()
    {
        int categoryID = Convert.ToInt32(this.ddlCategory.SelectedValue);
        if (categoryID > 0)
        {
            this.ddlProviderType.Items.Clear();
            //this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            presenter.RequestProviderTypes(ddlApplicationType.SelectedIndex > -1 ? Convert.ToInt32(ddlApplicationType.SelectedValue) : 0, categoryID);
        }

    }

    private void GetProviderTypeDependentFields()
    {
        int providerTypeID = Convert.ToInt32(this.ddlProviderType.SelectedValue);
        if (providerTypeID > 0)
        {
            //this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            //presenter.RequestSpecialities(providerTypeID);
            presenter.RequestTaxonomies(providerTypeID, 0);
        }

    }

    private void GetSpecialtyDependentFields()
    {
        int providerTypeID = Convert.ToInt32(this.ddlProviderType.SelectedValue);
        //int specialtyID = Convert.ToInt32(this.ddlSpecialty.SelectedValue);
        //if (specialtyID > 0 && providerTypeID > 0)
        if (providerTypeID > 0)
        {
            this.ddlTaxonomy.Items.Clear();
            presenter.RequestTaxonomies(providerTypeID, 0);
        }

    }

    private void GetDropDownValues(PaperRequestQueueData data)
    {
        if (data.ProviderCategoryTypeID > 0 && this.ddlProviderType.Items.Count == 0)
        {
            presenter.RequestProviderTypes(data.ApplicationTypeID, data.ProviderCategoryTypeID);
        }
        //if (data.ProviderTypeID > 0 && this.ddlSpecialty.Items.Count == 0)
        //{
        //    presenter.RequestSpecialities(data.ProviderTypeID);
        //}
        if (this.ddlTaxonomy.Items.Count == 0)
        {
            presenter.RequestTaxonomies(data.ProviderTypeID, 0);
        }
    }

    
    

    #endregion
}