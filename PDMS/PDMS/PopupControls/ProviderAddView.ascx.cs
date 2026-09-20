using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using MAXIMUS.Core.Libraries;
using System.Collections.Generic;
using System.Data.SqlClient;

public partial class Views_ProviderAddView : System.Web.UI.UserControl, IProviderAddView
{
    public static class EntityType
    {
        public const string Individual = "Individual";
        public const string Organization = "Organization";

    }
    #region Properties
    private ProviderAddPresenter _presenter;

    public ProviderAddPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProviderAddPresenter(this);
            }

            return _presenter;
        }
    }
    private ProviderManagerData _keyDataPage;
    private ProviderManagerData keyDataPage
    {
        get
        {
            return _keyDataPage;
        }
        set
        {
            _keyDataPage = value;
        }
    }
    public ProviderManagementData Model { get; set; }

    [SerializableAttribute()]
    public class InitialRegKeyFields
    {
        public int ProviderTypeID { get; set; }
        public string NPI { get; set; }
        public int TaxIDTypeID { get; set; }
        public int SpecialtyTypeID { get; set; }
        public int TaxonomyTypeID { get; set; }
        public string ZipCode { get; set; }
        public string ZipExt { get; set; }
        public int WorkflowID { get; set; }
        public DateTime RequestedEffectiveDate { get; set; }
        public int ReferralID { get; set; }
        public int PaperRequestID { get; set; }
        public int ApplicationTypeID { get; set; }

        public InitialRegKeyFields(int providerTypeID, string npi, int taxIDTypeID, int specialtyTypeID, int taxonomyTypeID,
            string zipCode, string zipExt, int workflowID, DateTime requestedEffectiveDate, int referralID, int paperRequestID,
            int applicationTypeID, string applicationTypeName)
            : base()
        {
            ProviderTypeID = providerTypeID;
            NPI = npi;
            TaxIDTypeID = taxIDTypeID;
            SpecialtyTypeID = specialtyTypeID;
            TaxonomyTypeID = taxonomyTypeID;
            ZipCode = zipCode;
            ZipExt = zipExt;
            WorkflowID = workflowID;
            RequestedEffectiveDate = requestedEffectiveDate;
            ReferralID = referralID;
            PaperRequestID = paperRequestID;
            ApplicationTypeID = applicationTypeID;
        }
    }

    private InitialRegKeyFields InitialKeyFields
    {
        get
        {
            return ViewState["InitialKeyFields"] == null ? null : ViewState["InitialKeyFields"] as InitialRegKeyFields;
        }
        set
        {
            ViewState["InitialKeyFields"] = value;
        }
    }


    public int WorkflowIDRequested
    {
        get
        {
            return ViewState["WorkflowIDRequested"] == null ? CON.WorkflowType.RegistrationNew : Convert.ToInt32(ViewState["WorkflowIDRequested"]);
        }
        set
        {
            ViewState["WorkflowIDRequested"] = value;
        }
    }


    public bool KeyFieldEditRequest
    {
        get
        {
            return ViewState["EditKeyFieldDataRequest"] == null ? false : Convert.ToBoolean(ViewState["EditKeyFieldDataRequest"]);
        }
        set
        {
            ViewState["EditKeyFieldDataRequest"] = value;
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

    public int ReferralTypeID
    {
        get
        {
            return ViewState["ReferralTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ReferralTypeID"]);
        }
        set
        {
            ViewState["ReferralTypeID"] = value;
        }
    }

    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

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

    public int SpecialtyTypeID
    {
        get
        {
            return ViewState["SpecialtyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set
        {
            ViewState["SpecialtyTypeID"] = value;
        }
    }

    public int TaxonomyTypeID
    {
        get
        {
            return ViewState["TaxonomyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["TaxonomyTypeID"]);
        }
        set
        {
            ViewState["TaxonomyTypeID"] = value;
        }
    }

    public bool EditingNPI
    {
        get
        {
            return ViewState["EditingNPI"] == null ? false : Convert.ToBoolean(ViewState["EditingNPI"]);
        }
        set
        {
            ViewState["EditingNPI"] = value;
        }
    }

    public DateTime? OriginalNPIEndDate
    {
        get
        {
            return ViewState["NPIEndDate"] as DateTime?;
        }
        set
        {
            ViewState["NPIEndDate"] = value;
        }
    }

    public int ApplicationTypeID
    {
        get
        {
            return ViewState["ApplicationTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ApplicationTypeID"]);
        }
        set
        {
            ViewState["ApplicationTypeID"] = value;
        }
    }

    public DateTime? EndDate
    {
        get
        {
            return ViewState["EndDate"] as DateTime?;
        }
        set
        {
            ViewState["EndDate"] = value;
        }
    }

    public int TaxIDTypeID
    {
        get
        {
            return ViewState["TaxIDTypeID"] == null ? 0 : Convert.ToInt32(ViewState["TaxIDTypeID"]);
        }
        set
        {
            ViewState["TaxIDTypeID"] = value;
        }
    }

    public bool CheckNPIRequired
    {
        get
        {
            return ViewState["CheckNPIRequired"] == null ? false : Convert.ToBoolean(ViewState["CheckNPIRequired"]);
        }
        set
        {
            ViewState["CheckNPIRequired"] = value;
        }
    }

    public bool IsConvertedProviderOperatorUpdate
    {
        get
        {
            return ViewState["IsConvertedProviderOperatorUpdate"] == null ? false : Convert.ToBoolean(ViewState["IsConvertedProviderOperatorUpdate"]);
        }
        set
        {
            ViewState["IsConvertedProviderOperatorUpdate"] = value;
        }
    }

    #endregion

    #region Parent Page Events
    public delegate void InsertEventHandler(ProviderManagementData data);
    public event InsertEventHandler InsertEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    public delegate void KeyFieldUpdateEventHandler();
    public event KeyFieldUpdateEventHandler KeyFieldUpdateEvent;

    #endregion

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        ClearErrorMessages();
    }

    protected void Page_PreRender(object sender, EventArgs e)
    { 
    }

    protected void Validate_NPIEdit(object sender, ServerValidateEventArgs args)
    {
        if (EditingNPI)
        {
            if (txtNPI.Text == txtOldNPI.Text)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void Validate_AppNumber(object source, ServerValidateEventArgs args)
    {
        if (KeepPopupOpenEvent != null)
        {
            //have to keep the pop up open
            KeepPopupOpenEvent();
            return;
        }

    }

    protected void Validate_TaxIDType(object sender, ServerValidateEventArgs args)
    {
     
        if (ddlCategory.SelectedValue == Enumerations.ProviderCategoryType.IndividualSolo.ToString("D") && Enumerations.TaxIdTypeId.SSN.ToString("D") != args.Value)
        {
            args.IsValid = false;

        }
        else
        {
            args.IsValid = true;
        }        
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SetCategoryDependentFields();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }
    }

    protected void ddlProviderType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetProviderTypeDependentFields();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
            return;
        }
    }

    protected void ddlSpecialty_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.GetSpecialtyDependentFields();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
            return;
        }
    }

    protected void rblEntityType_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.SetWaiverDropDownValues();

        if (this.ReferralID > 0)
        {
            this.SetTaxIDBasedOnOrganization();
        }

        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
            return;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                //have to keep the pop up open
                ValidationEvent();
                return;
            }
        }
        if (this.IsConvertedProviderOperatorUpdate)
        {
            if(ddlTaxonomyNPPES.SelectedIndex==0)
            {
                //lblErrorMessages.Text = "Taxonomy is Required";
                return;
            }
            SaveProviderNoPopup();            
        }
        else
        {
            if (this.KeyFieldEditRequest)
            {
                this.Model = this.LoadModelFromInitialKeyFields();
            }
            else
            {
                this.Model = this.LoadModelFromForm();
            }
            presenter.ValidateProviderInformation();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
        {
            CancelEvent();
        }
    }

    protected void btnKFEYes_Click(object sender, EventArgs e)
    {


        //reload the model
        this.Model = this.LoadModelFromInitialKeyFields();

        //continue with updates or delete/insert
        if (KeyFieldUpdatesRequireDelete())
        {
            presenter.RequestRegistrationRecreate();
        }
        else
        {
            presenter.UpdateProviderKeyFields();
            if (EditingNPI)
                SaveNPIHistory();
        }

        if (presenter.hasErrors)
        {
            SetErrorMessages();
        }
    }

    protected void btnKFECancel_Click(object sender, EventArgs e)
    {
        this.mpeKFEVerify.Hide();
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }
    }

    protected void lnkEditNPI_Click(object sender, EventArgs e)
    {
        if (!EditingNPI)
        {
            trOldNPI.Visible = true;
            trOldNPIStartDate.Visible = true;
            trOldNPIEndDate.Visible = true;
            txtOldNPIEndDate.Visible = true;
            trNPIStartDate.Visible = true;
            trNPIEndDate.Visible = true;
            txtNPIEndDate.Visible = true;
            txtNPIStartDate.Visible = true;
            txtNPI.Enabled = true;
            txtNPI.CssClass = "formField";
            EditingNPI = true;
        }
        else
        {
            trOldNPI.Visible = false;
            trOldNPIStartDate.Visible = false;
            trOldNPIEndDate.Visible = false;
            txtOldNPIEndDate.Visible = false;
            trNPIStartDate.Visible = false;
            trNPIEndDate.Visible = false;
            txtNPIEndDate.Visible = false;
            txtNPIStartDate.Visible = false;
            txtNPI.Enabled = false;
            txtNPI.CssClass = "formFieldReadOnly";

            //Rolled back edited values
            txtNPI.Text = txtOldNPI.Text;
            EditingNPI = false;
            txtOldNPIEndDate.Text = OriginalNPIEndDate.HasValue ? OriginalNPIEndDate.Value.ToShortDateString() : string.Empty;
        }
        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }

    }
    protected void lnkEditEndDate_Click(object sender, EventArgs e)
    {
        trNPIEndDate.Visible = true;
        txtNPIEndDate.Visible = true;
        trNPIStartDate.Visible = false;
        txtNPI.Enabled = false;
        txtNPI.CssClass = "formFieldReadOnly";

        if (KeepPopupOpenEvent != null)
        {
            KeepPopupOpenEvent();
        }
    }

    #endregion

    #region Public Events
    public void InitView(ProviderManagerData keyData)
    {
        this.keyDataPage = keyData;
        this.RegID = keyData.RegID;
        this.WorkflowIDRequested = keyData.WorkflowIDRequested;
        this.ReferralID = keyData.ReferralID;
        this.PaperRequestQueueID = keyData.PaperRequestQueueID;
        this.txtTaxID.Text = keyData.TaxID;
        this.KeyFieldEditRequest = keyData.KeyFieldEditRequest;
        this.ReferralTypeID = keyData.ReferralTypeID;
        this.ApplicationTypeID = keyData.ApplicationTypeID;
        this.TaxIDTypeID = keyData.TaxIDTypeID;

        this.InitFormData();

        this.SetDefaultFields(keyData);

        presenter.Init(keyData);

        if (keyData.RegID > 0)
        {
            if (KeyFieldEditRequest)
            {
                presenter.RequestExistingRegData();
            }
            else
            {
                presenter.RequestConvertedRegData();
            }
        }
        else if (this.PaperRequestQueueID > 0)
        {
            presenter.RequestPaperRequestData();
        }
        else if (this.ReferralID > 0)
        {
            presenter.RequestReferralData();
        }
        else if (this.ddlCategory.SelectedIndex > 0)
        {
            this.SetCategoryDependentFields();
        }

        if (keyData.ReferralTypeID == CON.DiddReferralType.AssistedLiving)
        {
            this.SetAssistedLivingDefaults();
        }

        if (keyData.ConvertedProvider)
        {
            this.LockDownKeyIdentifierFields();
            this.trTaxonomy.Visible = false;
            this.divTaxonomy.Visible = true;   
        }

        if (keyData.KeyFieldEditRequest)
        {
            this.SetDisplayForKeyFieldEdit();
        }


        if (this.ReferralID > 0)
        {
            this.SetTaxIDBasedOnOrganization();
        }

        // BO: This is not the right way to do this, but I don't have time to mess with it. For now, only set the application type text 
        // to the keyData ApplicationTypeName if it's empty. It won't be empty during New Reg, but will be empty during Edit Key Provider Identifiers.
        //if (this.txtApplicationType.Text == string.Empty)
        //    this.txtApplicationType.Text = keyData.ApplicationTypeName;
        if (keyData.ApplicationTypeID > 0)
        {
            this.ddlApplicationType.SelectedValue = keyData.ApplicationTypeID.ToString();
            SetRegApplicationMessage(keyData.ApplicationTypeID.ToString());
        }

        //TODO: ProviderAddPresenter NPIRequired() would be a better check but it's tied to taxonomy which is edited on form and private so needs to be done in future
        bool isNFocus = keyData.ReferralID > 0 && keyData.ReferralTypeID != CON.DiddReferralType.AssistedLiving;
        if ((keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationUpdateProvider ||
        keyData.WorkflowIDRequested == CON.WorkflowType.RegistrationRevalidation ||
        (keyData.RegistrationProgramStatusTypeID == CON.RegistrationProgramStatusTypeId.Conversion)) && !isNFocus)
        {
            
            this.SetDisplayForNPIEdit();
        }
		
		SetTaxonomyTypesFromNPPES(null, false);
    }

    #endregion

    #region Presenter Events

    public void SetProviderCategories(DataView categories)
    {
        this.ddlCategory.DataSource = categories;
        this.ddlCategory.DataValueField = "PROVIDER_CATEGORY_TYPE_ID";
        this.ddlCategory.DataTextField = "PROVIDER_CATEGORY_TYPE_NAME";
        this.ddlCategory.DataBind();
        this.ddlCategory.Items.Insert(0, new ListItem("", "0"));

        SetCategoryDefault();
    }

    public void SetProviderTypes(DataView types)
    {
        types.Table.Columns.Add("IdWithName", typeof(string), "MMIS_PROVIDER_TYPE_ID + ' - ' + PROVIDER_TYPE_NAME");
        this.ddlProviderType.DataSource = types;
        this.ddlProviderType.DataValueField = "PROVIDER_TYPE_ID";
        this.ddlProviderType.DataTextField = "IdWithName";
        this.ddlProviderType.DataBind();
        this.ddlProviderType.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetSpecialtyTypes(DataSet data)
    {
        this.ddlSpecialty.DataSource = data;
        this.ddlSpecialty.DataValueField = "SPECIALTY_TYPE_ID";
        this.ddlSpecialty.DataTextField = "SPECIALTY_TYPE_NAME";
        this.ddlSpecialty.DataBind();
        this.ddlSpecialty.Items.Insert(0, new ListItem("", "0"));
    }

    public void SetTaxonomyTypes(DataView dv)
    {
        this.ddlTaxonomy.DataSource = dv;
        this.ddlTaxonomy.DataValueField = "TAXONOMY_TYPE_ID";
        this.ddlTaxonomy.DataTextField = "TaxonomyNameWithCode";
        this.ddlTaxonomy.DataBind();
        this.ddlTaxonomy.Items.Insert(0, new ListItem("", "0"));
    }

    private DataTable dsTaxonomyTypeToExclude()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet dsTaxonomyTypeToExclude = psc.SelectTaxonomyTypesToExclude(Model.ProviderTypeID);
        DataTable dtFilterTaxonomyType = null;
        if (Methods.HasRows(dsTaxonomyTypeToExclude))
            dtFilterTaxonomyType = dsTaxonomyTypeToExclude.Tables[0];
        return dtFilterTaxonomyType;
    }

    public void SetTaxonomyTypesFromNPPES(Result rs, bool showTaxonomoyDropdown)
    {
        if (!showTaxonomoyDropdown && rs == null)
        {
            this.ddlTaxonomyNPPES.DataSource = null;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            divTaxonomy.Visible = false;
            this.hdnName.Value = "";
            this.hdnNPI.Value = "";
            return;
        }

        DataView dv = null;

        //some taxonomies by provider type is filtered CR 63: Remove Taxonomy Code 251E00000X for EPD Waiver
        DataTable dtFilterTaxonomyType = dsTaxonomyTypeToExclude();

        DataTable dt = NPPESAPIHelper.LoadTaxonomyFromNPPES(rs, dtFilterTaxonomyType);

        if (Methods.HasRows(dt))
        {
            dv = dt.AsDataView();

            this.ddlTaxonomyNPPES.DataSource = dv;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            this.ddlTaxonomyNPPES.Items.Insert(0, new ListItem("", "0"));
            this.valTaxonmyCmp.Enabled = true;
            this.valTaxonmyCmp.Visible = true;
            this.trTaxonomy.Visible = false;

            //We are storing the NPI and Name in hidden variables here since if this code is hit it means the NPPES API is validated
            // and we dont want to call it again unless Name and NPI is changed
            int ProviderCategoryTypeID = ddlCategory.SelectedIndex > -1 ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
            if (ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
            {
                string Name = string.Concat(txtFirstName.Text.Trim(), string.IsNullOrEmpty(txtMI.Text.Trim()) ? string.Empty : " ", txtMI.Text.Trim(), string.IsNullOrEmpty(txtLastName.Text.Trim()) ? string.Empty : " ", txtLastName.Text.Trim());
                this.hdnName.Value = Name;
            }
            this.hdnNPI.Value = txtNPI.Text;
        }
        else
        {
            this.ddlTaxonomyNPPES.DataSource = null;
            this.ddlTaxonomyNPPES.DataValueField = "TAXONOMYcode";
            this.ddlTaxonomyNPPES.DataTextField = "TaxonomyNameWithCode";
            this.ddlTaxonomyNPPES.DataBind();
            presenter.ErrorList.Add(presenter.NextValidationKey(), NPPESAPIHelper.ErrorMessageNoTaxonomy);
        }
        this.divTaxonomy.Visible = true;

    }

    
    public void SetPracticeTypes(DataView data)
    {
        this.ddlPracticeType.Items.Clear();

        this.ddlPracticeType.DataSource = data;
        this.ddlPracticeType.DataValueField = "TYPE_OF_PRACTICE_ID";
        this.ddlPracticeType.DataTextField = "TYPE_OF_PRACTICE_NAME";
        this.ddlPracticeType.DataBind();
        if (this.ddlPracticeType.Items.Count > 1)
        {
            this.ddlPracticeType.Items.Insert(0, new ListItem("", "-1"));
        }

        SetPracticeTypeDefault();

    }

    public void SetApplicationTypes(DataView applicationTypes,DataView waiverTypes)
    {
        this.ddlApplicationType.DataSource = applicationTypes;
        this.ddlApplicationType.DataValueField = "APPLICATION_TYPE_ID";
        this.ddlApplicationType.DataTextField = "APPLICATION_TYPE_NAME";
        this.ddlApplicationType.DataBind();
    }


    public void SetDeleteOnlySuccess()
    {
        //NOT IMPLEMENTED YET, PLACE HOLDER code will close pop up after delete and refresh home page.
        if (KeyFieldUpdateEvent != null)
        {
            KeyFieldUpdateEvent();
        }

    }

    public void SetRecreateSuccess()
    {
        //Utilized for Key Field Edits.
        //Refresh home page.
        if (KeyFieldUpdateEvent != null)
        {
            KeyFieldUpdateEvent();
        }
    }

    public void SetKeyFieldUpdateResults()
    {
        //Utilized for Key Field Edits.
        //On successful update, refresh home page. 
        if (KeyFieldUpdateEvent != null)
        {
            KeyFieldUpdateEvent();
        }
    }

    public void SetMultipleMedicaidsFound(DataTable dt)
    {
        //bind to a repeater - pulled over from Register
        this.rblMedaidIds.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            rblMedaidIds.Items.Add(new ListItem(row["MedicaidID"].ToString(), row["RegID"].ToString()));
        }
        this.divMultipleMedicaidIds.Visible = true;
        this.cvMedicaidIDs.Enabled = true;
    }

    public void SetConvertedProviderData(ProviderManagementData data)
    {
        SetExistingRegData(data);

        //enabled editing if 0000 or null for bug 6453
        string zipExt = data.ZipExt;
        if (string.IsNullOrEmpty(zipExt) || zipExt == "0000")
        {
            this.txtZipCodeExt.Enabled = true;
        }
    }

    public void SetExistingProviderData(ProviderManagementData data)
    {
        //Called when editing key fields, need to save off initial key field values
        InitialKeyFields = new InitialRegKeyFields(data.ProviderTypeID, data.NPI, data.TaxIDTypeID, data.SpecialtyTypeID,
                data.TaxonomyTypeID, data.ZipCode, data.ZipExt, data.WorkflowID, data.RequestedEffectiveDate.Value, data.ReferralID, data.PaperRequestQueueID,
                data.ApplicationTypeID, data.ApplicationTypeName);

        SetExistingRegData(data);
    }

    public void SetPaperRequestData(PaperRequestQueueData data)
    {
        this.SpecialtyTypeID = data.SpecialtyTypeID;
        this.TaxonomyTypeID = data.TaxonomyTypeID;
        this.ReferralID = data.ReferralID;
        this.ApplicationTypeID = data.ApplicationTypeID;

        this.txtProviderName.Text = data.ProviderName;
        this.ddlApplicationType.SelectedValue = data.ApplicationTypeID.ToString();
        // NEW
        presenter.RequestProviderCategories(data.WorkflowRequestedID, data.ApplicationTypeID, 0);

        this.ddlCategory.SelectedValue = Helper.ValueExistsInDropDown(this.ddlCategory, data.ProviderCategoryTypeID.ToString()) ? data.ProviderCategoryTypeID.ToString() : "0";

        if (this.ReferralID > 0)
        {
            if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            {
                this.rblEntityType.SelectedIndex = 0;

            }
            else if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
            {
                this.rblEntityType.SelectedIndex = 1;
            }
        }

        this.ddlCategory_SelectedIndexChanged(null, null);

        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedValue = Helper.ValueExistsInDropDown(this.ddlProviderType, data.ProviderTypeID.ToString()) ? data.ProviderTypeID.ToString() : "0";
            this.ddlProviderType_SelectedIndexChanged(null, null);
        }
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedValue = Helper.ValueExistsInDropDown(this.ddlSpecialty, data.SpecialtyTypeID.ToString()) ? data.SpecialtyTypeID.ToString() : "0";
            this.ddlSpecialty_SelectedIndexChanged(null, null);
        }
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedValue = Helper.ValueExistsInDropDown(this.ddlTaxonomy, data.TaxonomyTypeID.ToString()) ? data.TaxonomyTypeID.ToString() : "0";
        }

        this.txtTaxID.Text = data.TaxID;
        this.txtNPI.Text = data.NPI;
        txtOldNPI.Text = data.NPI;
        this.txtZipCode.Text = data.ZipCode;
        this.txtZipCodeExt.Text = data.ZipExt;

        txtFirstName.Text = string.Empty;
        txtMI.Text = string.Empty;
        txtLastName.Text = string.Empty;
        txtBirthDate.Text = string.Empty;
        this.rblGender.SelectedIndex = -1;
        this.rblTaxIDType.SelectedIndex = -1;
    }

    public void SetExistingReferralData(ReferralData data)
    {
        this.txtFirstName.Text = data.FirstName;
        this.txtLastName.Text = data.LastName;
        this.txtProviderName.Text = data.GroupName;

        if (data.ReferralTypeID != CON.DiddReferralType.AssistedLiving)
        {
            this.rblEntityType.SelectedIndex = string.IsNullOrEmpty(data.GroupName) ? 0 : 1;
            SetWaiverDropDownValues();
        }
    }

    public void SetValidationSuccess()
    {
        if (this.KeyFieldEditRequest)
        {
            //for key field edits must verify update
            this.ltlKFEConfirm.Text = KeyFieldUpdatesRequireDelete() ? Resources.BrandingResource.KEY_FIELD_EDIT_DELETE_CONFIRMATION_MESSAGE
                : Resources.BrandingResource.KEY_FIELD_EDIT_UPDATE_CONFIRMATION_MESSAGE;

            this.mpeKFEVerify.Show();
            if (KeepPopupOpenEvent != null)
            {
                KeepPopupOpenEvent();
            }

            return;
        }

        if (this.Model.RegID > 0)
        {
            //Set re-enrollment due date on conversion to 90 days from now
            //this.Model.EndDate = DateTime.Now.AddDays(90);
            presenter.ConnectConvertedProvider();

            if (EditingNPI)
                SaveNPIHistory();
        }
        else
        {
            presenter.InsertProvider();
        }

        if (InsertEvent != null)
        {
            ProviderManagementData data = this.Model;
            InsertEvent(data);
        }
    }

    public void SetErrorMessages()
    {
        if (presenter.hasErrors)
        {
            ClearErrorMessages();
            SetValidationErrors();
        }

        if (KeepPopupOpenEvent != null)
        {
            //have to keep the pop up open
            KeepPopupOpenEvent();
            return;
        }
    }

    #endregion

    #region Private Events

    private void SetPaperEntryFieldAppearance()
    {
        if (this.PaperRequestQueueID == 0)
        {
            trTaxIDType.Visible = true;
            rblTaxIDType.Enabled = true;
            trMedicaid.Visible = false;
        }
        else
        {
            //for bug number 6077
            trTaxIDType.Visible = true;
            rblTaxIDType.Enabled = true;
            trMedicaid.Visible = true;
        }
    }

    private void SetDefaultFields(ProviderManagerData keyData)
    {
        if (keyData.TaxIDTypeID > 0)
        {
            this.rblTaxIDType.SelectedValue = keyData.TaxIDTypeID.ToString();
            this.rblTaxIDType.Enabled = false;
        }
        else
            this.rblTaxIDType.SelectedIndex = -1;

        this.txtZipCode.Text = keyData.ZipCode;
        this.txtZipCodeExt.Text = keyData.ZipExt;
    }

    private void InitFormData()
    {
        this.ResetFieldEditability();

        this.SetPaperEntryFieldAppearance();

        txtNPI.Text = txtMedicaidID.Text = txtZipCode.Text = txtZipCodeExt.Text = txtProviderName.Text = txtApplicationNumber.Text = txtOldNPI.Text = string.Empty;

        this.ddlTaxonomy.Items.Clear();
        this.ddlSpecialty.Items.Clear();
        this.ddlProviderType.Items.Clear();
        this.ddlCategory.Items.Clear();
        this.ddlPracticeType.Items.Clear();
        this.ddlApplicationType.Items.Clear();

        txtFirstName.Text = 
        txtMI.Text = txtLastName.Text =
        txtBirthDate.Text = string.Empty;
        //txtApplicationType.Text = string.Empty;
        this.rblGender.SelectedIndex = -1;
        this.txtEffectiveDate.Text = DateTime.Now.ToShortDateString();

        this.trRED.Visible = this.RegID == 0 && this.WorkflowIDRequested != CON.WorkflowType.GroupMemberProfile;//cannot edit for existing/converted providers or group member profiles
        this.SetVisibilityByProviderCategory(WorkflowIDRequested == CON.WorkflowType.GroupMemberProfile ? CON.ProviderCategoryTypeID.Individual : CON.ProviderCategoryTypeID.Group);

        this.SetEditabilityByWorkflowRequestType();

        //this drop down taxonomy is displayed only for converted providers or for KeyFieldEditRequest for provider or admin
        this.trTaxonomy.Visible = this.RegID > 0;
        //CheckNPIRequired will call NPPESAPI we need to check when new provider or converted/existing or when user changed NPI for KeyFieldEditRequest or AdminKeyFieldEditRequest
        CheckNPIRequired = this.RegID == 0 || (this.RegID > 0 && !this.KeyFieldEditRequest ); //&& !this.AdminKeyFieldEditRequest
        this.hdnName.Value = "";
        this.hdnNPI.Value = "";

        this.divTaxonomy.Visible = false; // this is displayed dynamically from NPPES API in SetTaxonomyTypesFromNPPES

        this.SetDynamicValidationMessages();

        this.divKeyFieldEditInfo.Visible = KeyFieldEditRequest;

        trNPIStartDate.Visible = false;
        trNPIEndDate.Visible = false;
        trOldNPI.Visible = false;
        trOldNPIStartDate.Visible = false;
        trOldNPIEndDate.Visible = false;
        txtOldNPIEndDate.Visible = false;
        this.txtNPIStartDate.Visible = false;
        this.txtNPIEndDate.Visible = false;
        this.txtNPIStartDate.Text = DateTime.Now.ToShortDateString();
        this.txtNPIEndDate.Text = DateTime.Now.ToShortDateString();
        lnkEditNPI.Visible = false;
        lnkEditEndDate.Visible = false;
        EditingNPI = false;
        OriginalNPIEndDate = null;

        txtEffectiveDate.Enabled = false;

        lnkConversionConvertToFeeForService.Visible = false;
    }

    private void SetDynamicValidationMessages()
    {
        this.valAppNbrRqd.ErrorMessage = string.Format("* {0}", Resources.BrandingResource.REFERRAL_NUMBER_REQUIRED);
        this.valApplicationNbr.ErrorMessage = string.Format("* {0}", Resources.BrandingResource.REFERRAL_NUMBER_NOT_FOUND);
    }

    private ProviderManagementData LoadModelFromForm(bool? changeApplicationTypeID = false)
    {
        ProviderManagementData data = new ProviderManagementData();

        data.RegID = RegID;
        data.WorkflowIDRequested = WorkflowIDRequested;
        data.ReferralID = ReferralID;
        data.PaperRequestQueueID = PaperRequestQueueID;
        data.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        data.ApplicationTypeID = this.ApplicationTypeID;
        //data.ApplicationTypeName = txtApplicationType.Text;
        data.ProviderCategoryTypeID = ddlCategory.SelectedIndex > -1 ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            data.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            data.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        if (divTaxonomy.Visible || data.CheckNPIRequired) //if NPI changed
        {
            if (ddlTaxonomyNPPES.Items.Count > 0)
            {
                data.TaxonomyCode = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedValue : "0";
                data.TaxonomyDetail = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedItem.Text.Replace("(" + ddlTaxonomyNPPES.SelectedValue + ")", "").Trim() : "";
                if (ddlTaxonomyNPPES.SelectedIndex > 0)
                {
                    List<SqlParameter> taxonomyparams = new List<SqlParameter>();
                    taxonomyparams.Add(SqlParms.CreateParameter("TAXONOMY_CODE", DbType.String, data.TaxonomyCode, true));
                    taxonomyparams.Add(SqlParms.CreateParameter("PROVIDER_TYPE_ID", DbType.Int32, data.ProviderTypeID, true));
                    DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetLastTaxonomy", taxonomyparams, "test");
                    if (ds.Tables[0].Rows.Count > 0) 
                    {
                        int taxonomy_id = (int)ds.Tables[0].Rows[0][0];
                        data.TaxonomyTypeID = ddlTaxonomyNPPES.SelectedIndex > -1 ? taxonomy_id : 0;
                    } 
                }
            }
        }

        
        //if (ddlTaxonomy.Items.Count > 0)
        //{
        //    data.TaxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        //}

        data.TypeOfPracticeID = null;
        data.DBA = null;
        data.ProviderName = txtProviderName.Text.Trim();
        data.FirstName = txtFirstName.Text.Trim();
        data.MiddleInitial = txtMI.Text.Trim();
        data.LastName = txtLastName.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            data.ProviderName = data.IndividualFullName;
        }
        data.TaxID = txtTaxID.Text;
        data.IsPaperApplication = PaperRequestQueueID == 0 ? false : true;

        data.TaxIDTypeID = rblTaxIDType.SelectedIndex == -1 ? 0 : Convert.ToInt32(rblTaxIDType.SelectedValue);

        data.NPI = this.txtNPI.Text.Trim();
        data.PracticeLocation = string.Empty;
        data.ZipCode = this.txtZipCode.Text.Trim();
        data.ZipExt = this.txtZipCodeExt.Text.Trim();
        data.MedicaidID = this.txtMedicaidID.Text.Trim();

        data.Gender = rblGender.SelectedIndex == -1 ? string.Empty : rblGender.SelectedValue;
        if (!string.IsNullOrEmpty(txtBirthDate.Text))
        {
            data.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
        }

        if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
        {
            DateTime now = DateTime.Now;
            DateTime effectiveDate;

            DateTime.TryParse(txtEffectiveDate.Text, out effectiveDate);
            data.RequestedEffectiveDate = effectiveDate;
        }


        //Update start date if editing NPI. Keep Old Start Date if not.
        string startDateText = EditingNPI ? txtNPIStartDate.Text : txtOldNPIStartDate.Text;
        DateTime npiStartDate;
        DateTime.TryParse(startDateText, out npiStartDate);
        data.NPIStartDate = npiStartDate;

        //Update end date if editing npi. Keep same npi if not editing, end date can be nullable
        if (EditingNPI)
        {
            string endDateText = txtNPIEndDate.Text;
            DateTime npiEndDate;
            if (DateTime.TryParse(endDateText, out npiEndDate))
                data.NPIEndDate = npiEndDate;
            else
                data.NPIEndDate = null;
        }
        else
        {
            data.NPIEndDate = OriginalNPIEndDate;
        }



        data.RegCreateDateTime = DateTime.Now;
        data.ReferralNumber = this.txtApplicationNumber.Text.Trim();

        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        data.WorkflowID = WorkflowIDRequested;
        data.RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Pending;

        this.cvMedicaidIDs.Enabled = false;
        this.divMultipleMedicaidIds.Visible = false;
        if(this.EndDate != null)
        data.EndDate = this.EndDate;
        if (changeApplicationTypeID == true)
        {
            data.WorkflowIDRequested = data.WorkflowID = 1;
            data.ApplicationTypeID = 1;
            //data.ConvertedProvider = true;
        }
   
        data.CheckNPIRequired = this.CheckNPIRequired =  //check to see if we need to check from API again
            ((data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual 
                || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
                && this.hdnName.Value != data.ProviderName) || this.hdnNPI.Value != data.NPI;

        
        return data;
    }

    /// <summary>
    /// Used during key field edits when change a key field that requires a delete of existing registration and insert of new
    /// </summary>
    /// <returns></returns>
    private ProviderManagementData LoadModelFromInitialKeyFields()
    {
        ProviderManagementData data = new ProviderManagementData();

        data.KeyFieldEditRequest = this.KeyFieldEditRequest;

        //Saved off values that are not editable by user for this functionality
        data.RegID = RegID;
        data.WorkflowIDRequested = InitialKeyFields.WorkflowID;
        data.ApplicationTypeID = InitialKeyFields.ApplicationTypeID;
        data.ReferralID = InitialKeyFields.ReferralID;
        data.PaperRequestQueueID = InitialKeyFields.PaperRequestID;
        data.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        //Pull from form
        data.ProviderCategoryTypeID = ddlCategory.SelectedIndex > -1 ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            data.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            data.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        if (ddlTaxonomy.Items.Count > 0)
        {
            data.TaxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        }
        data.TypeOfPracticeID = null;
        data.DBA = null;
        data.ProviderName = txtProviderName.Text.Trim();
        data.FirstName = txtFirstName.Text.Trim();
        data.MiddleInitial = txtMI.Text.Trim();
        data.LastName = txtLastName.Text.Trim();
        if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
        {
            data.ProviderName = data.IndividualFullName;
        }
        data.TaxID = txtTaxID.Text;
        data.IsPaperApplication = PaperRequestQueueID == 0 ? false : true;

        data.TaxIDTypeID = rblTaxIDType.SelectedIndex == -1 ? 0 : Convert.ToInt32(rblTaxIDType.SelectedValue);
        data.NPI = this.txtNPI.Text.Trim();
        data.PracticeLocation = string.Empty;
        data.ZipCode = this.txtZipCode.Text.Trim();
        data.ZipExt = this.txtZipCodeExt.Text.Trim();
        data.MedicaidID = this.txtMedicaidID.Text.Trim();

        data.Gender = rblGender.SelectedIndex == -1 ? string.Empty : rblGender.SelectedValue;
        if (!string.IsNullOrEmpty(txtBirthDate.Text))
        {
            data.BirthDate = Convert.ToDateTime(txtBirthDate.Text);
        }

        if (!string.IsNullOrEmpty(txtEffectiveDate.Text))
        {
            DateTime now = DateTime.Now;
            DateTime effectiveDate;

            DateTime.TryParse(txtEffectiveDate.Text, out effectiveDate);
            data.RequestedEffectiveDate = effectiveDate;

            //bug 8055 - retro_effective_date field is removed and instead it is calculated based on the new field reg_create_date_time
            /*if (txtEffectiveDate.Enabled)
            {
                int months = Convert.ToInt32(AppSettings.Get("RetroEffectiveDateWindow"));
                data.RetroEffectiveDate = effectiveDate.AddDays(months) < now ? true : false;
            }*/
        }

        //Update start date if editing NPI. Keep Old Start Date if not.
        string startDateText = EditingNPI ? txtNPIStartDate.Text : txtOldNPIStartDate.Text;
        DateTime npiStartDate;
        DateTime.TryParse(startDateText, out npiStartDate);
        data.NPIStartDate = npiStartDate;

        //Update end date if editing npi. Keep same npi if not editing, end date can be nullable
        if (EditingNPI)
        {
            string endDateText = txtNPIEndDate.Text;
            DateTime npiEndDate;
            if (DateTime.TryParse(endDateText, out npiEndDate))
                data.NPIEndDate = npiEndDate;
            else
                data.NPIEndDate = null;
        }
        else
        {
            data.NPIEndDate = OriginalNPIEndDate;
        }


        data.ReferralNumber = this.txtApplicationNumber.Text.Trim();

        data.LastModifiedDate = DateTime.Now;
        data.LastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);

        data.WorkflowID = WorkflowIDRequested;
        data.RegistrationStatusTypeID = CON.RegistrationStatusTypeId.Pending;

        data.CheckNPIRequired = this.CheckNPIRequired =  //check to see if we need to check from API again
           ((data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual
               || data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile)
               && this.hdnName.Value != data.ProviderName) || this.hdnNPI.Value != data.NPI;

        if (data.CheckNPIRequired)
        {
            if (ddlTaxonomyNPPES.Items.Count > 0)
            {
                data.TaxonomyCode = ddlTaxonomyNPPES.SelectedIndex > -1 ? ddlTaxonomyNPPES.SelectedValue : "0";
                data.TaxonomyTypeID = 0;
            }
        }
        this.cvMedicaidIDs.Enabled = false;
        this.divMultipleMedicaidIds.Visible = false;

        return data;
    }


    private void SetValidationErrors()
    {
        foreach (var pair in presenter.ErrorList)
        {
            AddErrorMessage(pair.Value);
        }
    }

    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }

    private void SetRegApplicationMessage(string ApplicationTypeId)
    {
        string regMessage = CON.RegApplicationCompleteTimeLimitMessage;
        try
        {
            if (!string.IsNullOrEmpty(ApplicationTypeId))
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                int ElapsedTimeHours = psc.GetElapsedTimeLimitByApplicationType(Helper.ConvertStringToInt32(ApplicationTypeId));
                int DaysToCompleteRegistration = Helper.ConvertHoursToDays(ElapsedTimeHours);
                regMessage = string.Format(regMessage, DaysToCompleteRegistration.ToString(), DaysToCompleteRegistration.ToString());
                lblElapsedTimeMsg.Text = regMessage;
            }
        }
        catch (Exception ex)
        {
            throw ex;

        }

    }

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }


    private void SetEditabilityByWorkflowRequestType()
    {
        switch (this.WorkflowIDRequested)
        {
            case CON.WorkflowType.GroupMemberProfile:
                this.SetEditabilityOnWaiverServicesFields(true);
                this.SetVisibilityOnWaiverServicesFields(false);
                this.SetValidationOnWaiverServiceOnlyFields(false);
                this.SetValidationOnNonGroupMemberProfileFields(false);
                this.SetEditabilityOnGroupMemberProfileFields(false);
                break;
            case CON.WorkflowType.RegistrationDIDDReferral:
                this.SetEditabilityOnWaiverServicesFields(false);
                this.SetVisibilityOnWaiverServicesFields(true);
                this.SetValidationOnWaiverServiceOnlyFields(true);
                break;
            default:
                this.SetEditabilityOnGroupMemberProfileFields(true);
                this.SetValidationOnNonGroupMemberProfileFields(true);
                this.SetEditabilityOnWaiverServicesFields(true);
                this.SetValidationOnWaiverServiceOnlyFields(false);
                this.SetVisibilityOnWaiverServicesFields(this.ReferralID > 0);
                break;
        }
    }

    private void SetEditabilityOnAssistedLivingFields()
    {
        this.ddlCategory.Enabled = false;
        this.ddlProviderType.Enabled = false;
        this.ddlSpecialty.Enabled = false;
        this.SetValidationOnNonGroupMemberProfileFields(true);

        // SetEditabilityOnWaiverServicesFields
        this.trEntityType.Visible = false;
        this.trCategory.Visible = true;
        this.trProviderType.Visible = true;
        this.txtZipCode.Enabled = true;
        this.txtZipCodeExt.Enabled = true;

        // SetValidationOnWaiverServiceOnlyFields
        this.cvPracticeType.Enabled = true;
        this.valAppNbrRqd.Enabled = true;
        this.valApplicationNbr.Enabled = true;

        // SetVisibilityOnWaiverServicesFields
        this.trNPI.Visible = true;
        this.trTaxonomy.Visible = true;
        this.trSpecialty.Visible = true;
        this.trReferral.Visible = true;

    }

    private void SetAssistedLivingDefaults()
    {
        ddlCategory.SelectedValue = CON.ProviderCategoryTypeID.EntityFacility.ToString();
        this.SetCategoryDependentFields();

        // Set provider type
        string providerTypeName = this.SelectAppSetting("NursingHomeProviderTypeName");
        if (!string.IsNullOrEmpty(providerTypeName))
        {
            ddlProviderType.SelectedValue = FindDropDownValueWithoutId(ddlProviderType, providerTypeName);
        }
        
        this.GetProviderTypeDependentFields();

        // Set specialty
        string providerSpecialtyName = SelectAppSetting("AssistedLivingServicesSpecialtyName");
        if (!string.IsNullOrEmpty(providerSpecialtyName))
        {
            ddlSpecialty.SelectedValue = FindDropDownValueWithoutId(ddlSpecialty, providerTypeName);
        }

        this.GetSpecialtyDependentFields();
        this.SetEditabilityOnAssistedLivingFields();
    }

    private void ResetFieldEditability()
    {
        this.ddlCategory.Enabled = true;
        this.ddlProviderType.Enabled = true;
        this.ddlSpecialty.Enabled = true;
        this.ddlTaxonomy.Enabled = true;
        this.txtNPI.Enabled = true;
        this.txtNPI.CssClass = "formField";

    }

    private void LockDownKeyIdentifierFields()
    {
        this.ddlCategory.Enabled = false;
        this.ddlProviderType.Enabled = false;
        this.ddlSpecialty.Enabled = false;
        this.txtMedicaidID.Enabled = string.IsNullOrEmpty(this.txtMedicaidID.Text.Trim()) ? true : false;
        this.txtMedicaidID.CssClass = this.txtMedicaidID.Enabled ? "formField" : "formFieldReadOnly";
        //Some providers will come over without a valid taxonomy, i.e. Not Defined.
        this.ddlTaxonomy.Enabled = this.ddlTaxonomy.SelectedIndex > 0 ? false : true;
        //Some providers will be coming over with null NPI values, however their provider type might now require an NPI.
        this.txtNPI.Enabled = string.IsNullOrEmpty(this.txtNPI.Text.Trim()) ? true : false;
        this.txtNPI.CssClass = this.txtNPI.Enabled ? "formField" : "formFieldReadOnly";
        this.rblTaxIDType.Enabled = false;

            lnkConversionConvertToFeeForService.Visible = false;

    }

    private void SetDisplayForNPIEdit()
    {
        if (this.KeyFieldEditRequest)
        {
            //These values should already be set when editing key fields
            this.ddlProviderType.Enabled = false;
            this.ddlSpecialty.Enabled = false;
            this.ddlTaxonomy.Enabled = false;
            this.rblTaxIDType.Enabled = false;
        }


        this.txtNPI.Enabled = string.IsNullOrWhiteSpace(this.txtNPI.Text) ? true : false;
        this.txtNPI.CssClass = this.txtNPI.Enabled ? "formField" : "formFieldReadOnly";

        //Links that conversion and revalidation should have access to
        this.lnkEditNPI.Visible = !this.txtNPI.Enabled;
        this.trNPI.Visible = true;  //could be hidden for some waiver services that need to edit npi

        //Always disable for now
        this.lnkEditEndDate.Visible = false;

        EditingNPI = false;
    }



    private void SetDisplayForKeyFieldEdit()
    {

        //key fields that can be edited.
        this.ddlProviderType.Enabled = true;
        this.ddlSpecialty.Enabled = true;
        this.ddlTaxonomy.Enabled = true;
        this.rblTaxIDType.Enabled = true;
        this.txtNPI.Enabled = true;

        //Only conversion/revalidation/update should have access to
        this.lnkEditNPI.Visible = false;
        this.lnkEditEndDate.Visible = false;

        this.rblEntityType.Enabled = false;
        this.ddlCategory.Enabled = false;
        this.txtApplicationNumber.Enabled = false;
        this.txtApplicationNumber.CssClass = this.txtApplicationNumber.Enabled ? "formField" : "formFieldReadOnly";



        this.txtZipCode.Enabled = true;
        this.txtZipCodeExt.Enabled = true;

        //hide
        this.trRED.Visible = false;
        this.trGender.Visible = false;
        this.trDOB.Visible = false;
        this.trMedicaid.Visible = false;
    }

    private string SelectAppSetting(string appSettingKey)
    {
            return AppSettings.Get(appSettingKey, string.Empty);
    }

    private void SetEditabilityOnGroupMemberProfileFields(bool isEditable)
    {
        this.ddlCategory.Enabled = isEditable;
        this.txtEffectiveDate.Enabled = isEditable;
    }


    private void SetEditabilityOnWaiverServicesFields(bool isEditable)
    {
        this.trEntityType.Visible = !isEditable; //only visible for waiver services
        this.trCategory.Visible = isEditable;
        this.trProviderType.Visible = isEditable;
        this.ddlCategory.Enabled = isEditable;
        this.ddlProviderType.Enabled = isEditable;
        this.txtZipCode.Enabled = isEditable;
        this.txtZipCodeExt.Enabled = isEditable;
        //enabled editing if 0000 or null for bug 6453
        if (string.IsNullOrEmpty(this.txtZipCodeExt.Text.Trim()) || this.txtZipCodeExt.Text.Trim() == "0000")
        {
            this.txtZipCodeExt.Enabled = true;
        }
    }

    private void SetVisibilityOnWaiverServicesFields(bool isVisible)
    {
        this.trNPI.Visible = !isVisible; //hide
        this.trTaxonomy.Visible = !isVisible; //hide
        this.trSpecialty.Visible = !isVisible; //hide
        this.trReferral.Visible = isVisible; //show for referral
    }

 

    private void SetVisibilityByProviderCategory(int categoryID)
    {
         switch (categoryID)
        {
            case CON.ProviderCategoryTypeID.Individual:
            case CON.ProviderCategoryTypeID.GroupMemberProfile:
                this.SetVisibilityOnIndividualOnlyFields(true);
                this.SetVisibilityOnGroupOnlyFields(false);
                this.SetValidationOnIndividualOnlyFields(true);
                this.SetValidationOnGroupOnlyFields(false);
                break;
            case CON.ProviderCategoryTypeID.Pharmacy:
                this.SetVisibilityOnIndividualOnlyFields(false);
                this.SetVisibilityOnGroupOnlyFields(true);
                this.SetValidationOnIndividualOnlyFields(false);
                this.SetValidationOnGroupOnlyFields(true);
                break;
            default:
                this.SetVisibilityOnIndividualOnlyFields(false);
                this.SetVisibilityOnGroupOnlyFields(true);
                this.SetValidationOnIndividualOnlyFields(false);
                this.SetValidationOnGroupOnlyFields(true);
                break;
        }

    }

    private void SetVisibilityByProviderType()
    {
        //Set after load/selection of provider type
    }

    private void SetEditabilityByProviderType()
    {
        //Set after load/selection of provider type
    }

    private void SetVisibilityOnLocationFields(bool isVisible)
    {
        this.trZipCode.Visible = isVisible;
        this.trZipExt.Visible = isVisible;
    }

    private void SetVisibilityOnIndividualOnlyFields(bool isVisible)
    {
        this.trFN.Visible = isVisible;
        this.trMI.Visible = isVisible;
        this.trLN.Visible = isVisible;
        this.trGender.Visible = isVisible;
        this.trDOB.Visible = isVisible;
        //this.trZipCode.Visible = isVisible;
        //this.trZipExt.Visible = isVisible;

    }

    private void SetVisibilityOnGroupOnlyFields(bool isVisible)
    {
        this.trOrgName.Visible = isVisible;
        this.trOrgNameHint.Visible = isVisible;
    }

    private void SetValidationOnIndividualOnlyFields(bool enabled)
    {
        this.cvPracticeType.Enabled = enabled;
        this.valFNReqd.Enabled = enabled;
        this.valLNReqd.Enabled = enabled;
        //Tax ID Type has only 2 values and one is defaulted to turned on on load, so no validation.
        this.valGenderReqd.Enabled = enabled;
        this.cvDOBFormat.Enabled = enabled;
        this.valDOBReqd.Enabled = enabled;
    }

    private void SetValidationOnGroupOnlyFields(bool enabled)
    {
        this.valNameReqd.Enabled = enabled;
    }


    private void SetValidationOnWaiverServiceOnlyFields(bool enabled)
    {
        this.cvPracticeType.Enabled = !enabled;
        this.valAppNbrRqd.Enabled = enabled;
        this.valApplicationNbr.Enabled = enabled;

    }

    private void SetValidationOnNonGroupMemberProfileFields(bool enabled)
    {
        this.valZipExtRqd.Enabled = enabled;
        this.valZipExtFormat.Enabled = enabled;
        this.valZipReqd.Enabled = enabled;
        this.valZipFormat.Enabled = enabled;
    }

    private void SetCategoryDefault()
    {
        if (this.ddlCategory.Items.Count == 0)
            return;

        //hide: npi, default then hide taxonomy code, default then hide provider category and type, hid type of practice
        switch (WorkflowIDRequested)
        {
            case CON.WorkflowType.GroupMemberProfile:
                this.ddlCategory.SelectedValue = CON.ProviderCategoryTypeID.GroupMemberProfile.ToString();
                this.rblTaxIDType.SelectedValue = CON.TaxIDType.SSN.ToString();
                this.rblTaxIDType.Enabled = false;
                break;           
            default:
                break;
        }
    }

    private void SetPracticeTypeDefault()
    {
        if (this.ddlPracticeType.Items.Count > 0)
        {
            this.ddlPracticeType.SelectedIndex = 0;
        }
    }

    private void SetCategoryDependentFields()
    {
        int categoryID = Convert.ToInt32(this.ddlCategory.SelectedValue);
        if (categoryID > 0)
        {
            this.ddlProviderType.Items.Clear();
            this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            this.ddlPracticeType.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestCategoryDependentFields(this.ApplicationTypeID, categoryID, "", WorkflowIDRequested,0);
        }

        this.SetVisibilityByProviderCategory(categoryID);
      
    }

    private void GetProviderTypeDependentFields()
    {
        int categoryID = Helper.ConvertStringToInt32(this.ddlCategory.SelectedValue);
        int providerTypeID = Helper.ConvertStringToInt32(this.ddlProviderType.SelectedValue);
        if (providerTypeID > 0)
        {
            this.ddlSpecialty.Items.Clear();
            this.ddlTaxonomy.Items.Clear();
            this.ddlPracticeType.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestSpecialities(providerTypeID);
            presenter.RequestPracticeTypes(categoryID, providerTypeID);
            presenter.RequestTaxonomies(providerTypeID, 0); /* Don't filter taxonomies on specialty. Just on provider type*/
        }

        this.SetVisibilityByProviderType();
        this.SetEditabilityByProviderType();
    }

    private void GetSpecialtyDependentFields()
    {
        int providerTypeID = Helper.ConvertStringToInt32(this.ddlProviderType.SelectedValue);
        int specialtyID = Helper.ConvertStringToInt32(this.ddlSpecialty.SelectedValue);
        if (specialtyID > 0 && providerTypeID > 0)
        {
            this.ddlTaxonomy.Items.Clear();
            Model = this.LoadKeyFields();
            presenter.RequestTaxonomies(providerTypeID, 0); /* Don't filter taxonomies on specialty. Just on provider type*/
        }
    }

    private void SetTaxIDBasedOnOrganization()
    {
        switch (rblEntityType.SelectedValue)
        {
            case EntityType.Organization:
                //Organizations can have EIN or SSN
                this.rblTaxIDType.Enabled = true;
                break;
            case EntityType.Individual:
                //Individuals can only have SSN
                this.rblTaxIDType.SelectedValue = CON.TaxIDType.SSN.ToString();
                this.rblTaxIDType.Enabled = false;
                break;
            default:
                break;
        }
    }

    private void SetWaiverDropDownValues()
    {        //Field created for Waiver Services - will drive the defaulted values for category and provider type (will only be one provider type).
        //entity type, itself, is not saved anywhere.
        switch (rblEntityType.SelectedValue)
        {
            case EntityType.Organization:
                this.ddlCategory.SelectedValue = CON.ProviderCategoryTypeID.EntityFacility.ToString();
                this.SetCategoryDependentFields();
                break;
            case EntityType.Individual:
                this.ddlCategory.SelectedValue = CON.ProviderCategoryTypeID.Individual.ToString();
                this.SetCategoryDependentFields();
                break;
            default:
                break;
        }

        int providerTypeID = 0;
        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedIndex = 1;
            providerTypeID = Convert.ToInt32(this.ddlProviderType.SelectedValue);
            presenter.RequestSpecialities(providerTypeID);
            int categoryID = Convert.ToInt32(this.ddlCategory.SelectedValue);
            presenter.RequestPracticeTypes(categoryID, providerTypeID);
        }
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedIndex = 1;
            presenter.RequestTaxonomies(providerTypeID, Convert.ToInt32(this.ddlSpecialty.SelectedValue));
        }
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedIndex = 1;
        }

    }

    private void SetExistingRegData(ProviderManagementData data)
    {
        this.RegID = data.RegID;
        this.SpecialtyTypeID = data.SpecialtyTypeID;
        this.TaxonomyTypeID = data.TaxonomyTypeID;
        this.txtProviderName.Text = data.ProviderName != null ? data.ProviderName.Trim() : string.Empty;        
        this.hdnName.Value = data.ProviderName != null ? data.ProviderName.Trim() : string.Empty;
        this.ddlCategory.SelectedValue = Helper.ValueExistsInDropDown(this.ddlCategory, data.ProviderCategoryTypeID.ToString()) ? data.ProviderCategoryTypeID.ToString() : "0";
        ddlCategory_SelectedIndexChanged(null, null);
        if (ddlProviderType.Items.Count > 0)
        {
            this.ddlProviderType.SelectedValue = Helper.ValueExistsInDropDown(this.ddlProviderType, data.ProviderTypeID.ToString()) ? data.ProviderTypeID.ToString() : "0";
        }
        ddlProviderType_SelectedIndexChanged(null, null);
        if (this.ddlSpecialty.Items.Count > 0)
        {
            this.ddlSpecialty.SelectedValue = Helper.ValueExistsInDropDown(this.ddlSpecialty, data.SpecialtyTypeID.ToString()) ? data.SpecialtyTypeID.ToString() : "0";
        }
        ddlSpecialty_SelectedIndexChanged(null, null);
        if (this.ddlTaxonomy.Items.Count > 0)
        {
            this.ddlTaxonomy.SelectedValue = Helper.ValueExistsInDropDown(this.ddlTaxonomy, data.TaxonomyTypeID.ToString()) ? data.TaxonomyTypeID.ToString() : "0";
        }
        this.txtTaxID.Text = data.TaxID != null ? data.TaxID.Trim() : string.Empty;
        this.txtNPI.Text = data.NPI != null ? data.NPI.Trim() : string.Empty;
        this.hdnNPI.Value = data.NPI != null ? data.NPI.Trim() : string.Empty;
        txtOldNPI.Text = data.NPI != null ? data.NPI.Trim() : string.Empty;
        this.txtZipCode.Text = data.ZipCode != null ? data.ZipCode.Trim() : string.Empty;
        this.txtZipCodeExt.Text = data.ZipExt != null ? data.ZipExt.Trim() : string.Empty;
        txtNPIStartDate.Text = txtNPIEndDate.Text = string.Empty;
        txtOldNPIEndDate.Text = data.NPIEndDate.HasValue ? data.NPIEndDate.Value.ToShortDateString() :
            string.Empty;
        OriginalNPIEndDate = data.NPIEndDate;
        txtOldNPIStartDate.Text = data.NPIStartDate.HasValue ? data.NPIStartDate.Value.ToShortDateString() :
            string.Empty;
        txtFirstName.Text = data.FirstName != null ? data.FirstName.Trim() : string.Empty;
        txtMI.Text = data.MiddleInitial != null ? data.MiddleInitial.Trim() : string.Empty;
        txtLastName.Text = data.LastName != null ? data.LastName.Trim() : string.Empty;
        txtBirthDate.Text = data.BirthDate.HasValue ? data.BirthDate.Value.ToShortDateString() : string.Empty;
        if (ddlPracticeType.Items.Count > 0 && Helper.ValueExistsInDropDown(this.ddlPracticeType, 
            data.TypeOfPracticeID.HasValue? data.TypeOfPracticeID.Value.ToString() : string.Empty))
        {
            this.ddlPracticeType.SelectedValue = data.TypeOfPracticeID.Value.ToString();
        }
        if (data.Gender != null && data.Gender.Length == 1)
        {
            this.rblGender.SelectedIndex = data.Gender == CON.GenderInitial.Male ? 0 : data.Gender == CON.GenderInitial.Female ? 1 : data.Gender == CON.GenderInitial.Unknown ? 2 : -1;
        }
        else
        {
            this.rblGender.SelectedIndex = data.Gender == CON.GenderName.Male ? 0 : data.Gender == CON.GenderName.Female ? 1 : data.Gender == CON.GenderName.Unknown ? 2 : -1;
        }
        if (data.TaxIDTypeID > 0)
        {
            this.rblTaxIDType.SelectedIndex = data.TaxIDTypeID == CON.TaxIDType.EIN ? 0 : data.TaxIDTypeID == CON.TaxIDType.SSN ? 1 : 0;
            this.rblTaxIDType.Enabled = false;
        }
        this.txtMedicaidID.Text = data.MedicaidID;

        this.SetVisibilityByProviderCategory(data.ProviderCategoryTypeID);

        if (this.ReferralID > 0)
        {
            if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.Individual)
            {
                this.rblEntityType.SelectedIndex = 0;
            }
            else if (data.ProviderCategoryTypeID == CON.ProviderCategoryTypeID.EntityFacility)
            {
                this.rblEntityType.SelectedIndex = 1;
            }
        }
        this.txtApplicationNumber.Text = data.ReferralNumber;
        //this.txtApplicationType.Text = data.ApplicationTypeName;
        if (data.ApplicationTypeID > 0)
            this.ddlApplicationType.SelectedValue = data.ApplicationTypeID.ToString();
        this.EndDate = data.RevalidationDate;
    }

    private bool KeyFieldUpdatesRequireDelete()
    {
        bool updateNeeded = false;

        int providerTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
        int specialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        int taxonomyTypeID = ddlTaxonomy.SelectedIndex > -1 ? Convert.ToInt32(ddlTaxonomy.SelectedValue) : 0;
        int typeOfPracticeID = ddlPracticeType.SelectedIndex > -1 ? Convert.ToInt32(ddlPracticeType.SelectedValue) : -1;
        int taxIDTypeID = Convert.ToInt32(rblTaxIDType.SelectedValue);

        if (providerTypeID != InitialKeyFields.ProviderTypeID
            || specialtyTypeID != InitialKeyFields.SpecialtyTypeID
            || taxIDTypeID != InitialKeyFields.TaxIDTypeID)
        {
            updateNeeded = true;
        }

        return updateNeeded;
    }

    private ProviderManagementData LoadKeyFields()
    {
        ProviderManagementData keyData = new ProviderManagementData();
        keyData.RegID = RegID;
        keyData.WorkflowIDRequested = WorkflowIDRequested;
        keyData.ReferralID = ReferralID;
        keyData.ReferralTypeID = ReferralTypeID;
        keyData.TaxIDTypeID = TaxIDTypeID;

        keyData.ProviderCategoryTypeID = ddlCategory.SelectedIndex > -1 ? Convert.ToInt32(ddlCategory.SelectedValue) : 0;
        if (ddlProviderType.Items.Count > 0)
        {
            keyData.ProviderTypeID = ddlProviderType.SelectedIndex > -1 ? Convert.ToInt32(ddlProviderType.SelectedValue) : 0;
        }
        if (ddlSpecialty.Items.Count > 0)
        {
            keyData.SpecialtyTypeID = ddlSpecialty.SelectedIndex > -1 ? Convert.ToInt32(ddlSpecialty.SelectedValue) : 0;
        }
        return keyData;
    }

    private void SaveNPIHistory()
    {
        ProviderManagementData pmd = this.Model;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.Model.RegID.ToString());
        parms.Add("NPI", txtOldNPI.Text);

        if (!string.IsNullOrWhiteSpace(txtOldNPIStartDate.Text))
        {
            DateTime npiStartDate;
            DateTime.TryParse(txtOldNPIStartDate.Text, out npiStartDate);
            parms.Add("NPI_START_DATE", npiStartDate.ToShortDateString());
        }

        if (!string.IsNullOrWhiteSpace(txtOldNPIEndDate.Text))
        {
            DateTime npiEndDate;
            DateTime.TryParse(txtOldNPIEndDate.Text, out npiEndDate);
            parms.Add("NPI_END_DATE", npiEndDate.ToShortDateString());
        }

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        svc.InsertRegistrationDataTable("NPI_HISTORY", parms);
    }

    private bool HasNPIWaitingForMMIS()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(Model.RegID, "NPI_HISTORY");
        if (!Helper.HasRows(ds))
            return false;
        else
        {
            var nullMMISStatus = ds.Tables[0].AsEnumerable().Where(s => s.Field<string>("MMIS_STATUS") == null);
            if (nullMMISStatus == null || nullMMISStatus.CopyToDataTable().Rows.Count == 0)
                return false;
            else
                return true;
        }
    }

    private void removeIndividualSoloCategoryFromDDL()
    {

    }
    // This is used in the case where the Operator is doing the Update on a Converted provider
    public void SaveProviderNoPopup()
    {
        this.IsConvertedProviderOperatorUpdate = true;
        if (this.KeyFieldEditRequest)
        {
            this.Model = this.LoadModelFromInitialKeyFields();
        }
        else
        {
            this.Model = this.LoadModelFromForm();
        }
        
        presenter.ValidateConvertedProviderForOperatorUpdates();
        

        if (!presenter.hasErrors)
        {
            if(this.IsConvertedProviderOperatorUpdate)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.UpdateRegistration(new Dictionary<string, string>() { { "REG_ID", this.Model.RegID.ToString() }, { "WAIVER_SERVICE_UPDATE_TYPE_ID", CON.WaiverServiceUpdateType.OperatorUpdate.ToString() } });
                psc.WF_SaveProcessParameter(this.Model.ProcessID, "WAIVER_SERVICE_UPDATE_TYPE_ID", CON.WaiverServiceUpdateType.OperatorUpdate.ToString());
            }

            Response.Redirect("~/Process/ProviderUpdateSummary.aspx?RegId=" + this.Model.RegID);
        }
    }
    protected void lnkConversionConvertToFeeForService_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                //have to keep the pop up open
                ValidationEvent();
                return;
            }
        }
        //presenter.ConvertToFeeForServive(RegID);
        //this.keyDataPage = LoadKeyDataFields();
        //this.keyDataPage.WorkflowIDRequested = 1;
        //this.keyDataPage.ApplicationTypeID = 1;
        //this.keyDataPage.ConvertedProvider = true;
        //this.InitView(keyDataPage);

        this.Model = this.LoadModelFromForm(true);
        presenter.ConvertToFeeForServive(RegID);
        //load model with updated values
        ProviderManagementData data = new ProviderManagementData();
        data = presenter.GetConvertedData();
        this.Model.ProviderTypeID = data.ProviderTypeID;
        this.Model.ApplicationTypeID = data.ApplicationTypeID;
        this.Model.WorkflowEventType = CON.WorkflowEventType.RevalReg;
        presenter.ValidateProviderInformation();
    }
    private ProviderManagerData LoadKeyDataFields()
    {
        ProviderManagerData keyData = new ProviderManagerData();
        //keyData.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        keyData.TaxID = txtTaxID.Text;
        keyData.TaxIDTypeID = TaxIDTypeID;
        keyData.RegID = RegID;


        keyData.ApplicationTypeName = "Standard Application";

        return keyData;
    }
    private string FindDropDownValueWithoutId(DropDownList ddl, string text)
    {
        string retVal = string.Empty;

        if (!string.IsNullOrEmpty(text))
        {
            foreach(ListItem item in ddl.Items)
            {
                string name = item.Text.Trim();
                if (name.IndexOf('-') > 0) {
                    name = name.Substring(name.IndexOf('-') + 1);
                }
                if (name.Equals(text, StringComparison.OrdinalIgnoreCase)){
                    retVal = item.Value;
                }
            }
        }

        return retVal;
    }

    #endregion

    private void TaxonomyFromNPI()
    {
        if (this.IsConvertedProviderOperatorUpdate && string.IsNullOrEmpty(txtNPI.Text.Trim()))
        {
            SaveProviderNoPopup();
        }
        else
        {
            if (this.KeyFieldEditRequest)
            {
                this.Model = this.LoadModelFromInitialKeyFields();
            }
            else
            {
                this.Model = this.LoadModelFromForm();
            }
            presenter.ValidateProviderInformation();
        }
    }

    protected void txtNPI_TextChanged(object sender, EventArgs e)
    {
        if (this.ApplicationTypeID == CON.ApplicationType.CPC)
        {
            return;
        }
        else
        {
            TaxonomyFromNPI();
        }
    }   

    protected void txtZipCodeExt_TextChanged(object sender, EventArgs e)
    {
        if (this.ApplicationTypeID == CON.ApplicationType.CPC)
        {
            return;
        }
        else
        {
            TaxonomyFromNPI();
        }
    }

    protected void ddlTaxonomy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ApplicationTypeID == CON.ApplicationType.CPC)
        {
            return;
        }
        else
        {
            TaxonomyFromNPI();
            divTaxonomy.Visible = true;
        }
    }

    protected void rblGender_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ApplicationTypeID == CON.ApplicationType.CPC)
        {
            return;
        }
        else
        {
            TaxonomyFromNPI();
        }
    }
}