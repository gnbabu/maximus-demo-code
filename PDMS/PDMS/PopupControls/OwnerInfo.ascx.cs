using MAXIMUS.Core.Libraries;
using MAXIMUS.Presentation.Interfaces.PDMS;
using Models.Data;
using Presentation;
using Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerInfo : BaseSectionControl, IOwnerInfoView
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Constants

    private const string OWNER_TYPE_PERSON = "Individual";
    private const string OWNER_TYPE_CORPORATION = "Organization";
    private const string OWNER_TYPE_MANAGING_EMPLOYEE = "Managing Employee";
    private const string PROPERTY_TYPE_PERSON = "Real Estate Individual";
    private const string PROPERTY_TYPE_CORPORATION = "Real Estate Organization";
    private const string ADDITIONAL_TYPE_SUBCONTRACTOR_PERSON = "Subcontractor Individual";
    private const string ADDITIONAL_TYPE_SUBCONTRACTOR_CORPORATION = "Subcontractor Organization";
    private const string ADDITIONAL_TYPE_SUPPLIER_PERSON = "Supplier Individual";
    private const string ADDITIONAL_TYPE_SUPPLIER_CORPORATION = "Supplier Organization";
    private const string ADDITIONAL_TYPE_EMPLOYEE = "Employee";
    private const string ADDITIONAL_TYPE_OTHER_PROVIDER = "Other Provider";



    #endregion

    #region Private variables
    private int _addressTypeId = CON.AddressType.OwnerInfo;
    private int _addressRegId = -1;
    private Address _address = new Address();
    private OwnerInfo _ownerInfo = new OwnerInfo();
    private OwnerInfoPresenter _presenter;
    private Dictionary<string, string> _addressParms = new Dictionary<string, string>();
    private bool _isEdit = false;
    private bool _isOrg = false;
    private bool _isPerson = false;
    private DataView _ownerTypeDataView = new DataView();
    private DataSet _dsOwnerTypes = new DataSet();
    #endregion

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    #region svc
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

    public OwnerInfoPresenter Presenter
    {
        get { return _presenter ?? (_presenter = new OwnerInfoPresenter(this)); }
    }

    private bool _OwnerTitleVisible;


    public bool OwnerTitleVisible
    {
        get { return _OwnerTitleVisible; }
        set
        {
            if (value != _OwnerTitleVisible)
            {
                _OwnerTitleVisible = value;
            }
        }
    }
    private bool _AffiliationTypeVisible;

    public bool AffiliationTypeVisible
    {
        get { return _AffiliationTypeVisible; }
        set
        {
            if (value != _AffiliationTypeVisible)
            {
                _AffiliationTypeVisible = value;
            }
        }
    }
    private bool _PercentageOfOwnershipVisible;

    public bool PercentageOfOwnershipVisible
    {
        get { return _PercentageOfOwnershipVisible; }
        set
        {
            if (value != _PercentageOfOwnershipVisible)
            {
                _PercentageOfOwnershipVisible = value;
            }
        }
    }

    private bool _SSNVisible;

    public bool SSNVisible
    {
        get { return _SSNVisible; }
        set
        {
            if (value != _SSNVisible)
            {
                _SSNVisible = value;
            }
        }
    }
    private bool _effectiveDateVisible;

    public bool EffectiveDateVisible
    {
        get { return _effectiveDateVisible; }
        set
        {
            if (value != _effectiveDateVisible)
            {
                _effectiveDateVisible = value;
            }
        }
    }
    private bool _DOBVisible;

    public bool DOBVisible
    {
        get { return _DOBVisible; }
        set
        {
            if (value != _DOBVisible)
            {
                _DOBVisible = value;
            }
        }
    }
    public string SaveButtonClientID
    {
        get;
        set;
    }

    public OwnerInfo RealEstateOwnerInfo { get; set; }
    public OwnerInfo AdditionalDisclosureInfo { get; set; }
    public OwnerInfo OwnerInfo { get; set; }
    public Address OwnerAddress { get; set; }
    OwnerInfo IView<OwnerInfo>.Model { get; set; }
    Address IView<Address>.Model { get; set; }

    private void SetVisibleFields()
    {
        ucAddress.AddressWrapperVisible = false;
        ucAddress.AddressTypeVisible = false;
        ucAddress.Cell1Visible = false;
        ucAddress.Cell2Visible = false;
        ucAddress.ContactVisible = false;
        ucAddress.Email1Visible = false;
        ucAddress.Email2Visible = false;
        ucAddress.Fax1Visible = false;
        ucAddress.Fax2Visible = false;
        ucAddress.OfficeMgrVisible = false;
        ucAddress.Phone1Visible = false;
        ucAddress.Phone2Visible = false;
        ucAddress.PhoneExt1Visible = false;
        ucAddress.PhoneExt2Visible = false;
    }

    public override bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(ucAddress.StreetAddress) || !string.IsNullOrEmpty(ucAddress.UnitAddress) || !string.IsNullOrEmpty(ucAddress.City) ||
            !string.IsNullOrEmpty(ucAddress.State) || !string.IsNullOrEmpty(ucAddress.Zip5) || !string.IsNullOrEmpty(ucAddress.Zip4) ||
            !string.IsNullOrEmpty(ucAddress.County) ||
            !string.IsNullOrEmpty(Helper.StripNonNumerics(ucAddress.PhoneNumber1)) || !string.IsNullOrEmpty(Helper.StripNonNumerics(ucAddress.FaxNumber1)) ||
            !string.IsNullOrEmpty(ucAddress.Email1))
        {
            rtn = true;
        }

        return rtn;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        LoadOwnerTypes();
        SelectOwnerTypes();
        GetOwnerTitle();
        GetAffliationType();
        // Set the handle to the save button on the parent page.
        // Can't get this working in declarative syntax
        SaveButtonClientID = Parent.FindControl("btnSave").ClientID;
        ucAddress.IsOwnerInfo = true;
        cmpValBirthDateFuture.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        ucAddress.AddressWrapperVisible = false;
        ucAddress.SetVisibleFields();


        //if(!IsPostBack)
        //SetControlVisibility();
    }

    private void SetControlVisibility()
    {
        trOwnTitle.Visible = OwnerTitleVisible;
        trAffType.Visible = AffiliationTypeVisible;
        trPercentageOwnership.Visible = PercentageOfOwnershipVisible;
        trSSN.Visible = SSNVisible;
        trDOB.Visible = DOBVisible;
        trEffectiveDate.Visible = EffectiveDateVisible;
    }

    public override void LoadControlData()
    {

    }

    private void LoadAddressInfo()
    {
        if (!SessionVarRetriever.IsNewOwner)
        {
            if (SessionVarRetriever.IsRealEstateOwner)
            {
                if (this.WorkflowPage.RealEstateOwnersList != null)
                {
                    if (Helper.HasRows(this.WorkflowPage.RealEstateOwnersList))
                        this.DataList = this.WorkflowPage.RealEstateOwnersList;
                }
            }

            else if (SessionVarRetriever.IsAdditionalDisclosure)
            {
                if (this.WorkflowPage.AdditionalDisclosureList != null)
                {
                    if (Helper.HasRows(this.WorkflowPage.AdditionalDisclosureList))
                        this.DataList = this.WorkflowPage.AdditionalDisclosureList;
                }
            }
            else
            {
                if (this.WorkflowPage.RegistrationOwnersList != null)
                {
                    if (Helper.HasRows(this.WorkflowPage.RegistrationOwnersList))
                        this.DataList = this.WorkflowPage.RegistrationOwnersList;
                }
            }

            DataRow row = this.DataList.Rows[0];
        }
        else
        {
            hdnRegAddressID.Value = "-1";
            ucAddress.AddressTypeId = -1;
        }
    }

    private void LoadOwnerTypes()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        ddlOwnerType.Items.Clear();
        _dsOwnerTypes = psc.SelectRegistrationData(-1, "OWNER_TYPE");
    }

    private void SelectOwnerTypes()
    {
        if (SessionVarRetriever.IsAdditionalDisclosure)
        {
            foreach (DataRow row in _dsOwnerTypes.Tables[0].Rows)
            {
                if (Convert.ToInt32(row.ItemArray[4]) == 0)
                    row.Delete();
            }
            _dsOwnerTypes.Tables[0].AcceptChanges();
        }
        else
        {
            if (SessionVarRetriever.IsRealEstateOwner)
            {
                foreach (DataRow row in _dsOwnerTypes.Tables[0].Rows)
                {
                    if (Convert.ToInt32(row.ItemArray[0]) == CON.OwnerType.PersonId ||
                        Convert.ToInt32(row.ItemArray[0]) == CON.OwnerType.ManagingEmployeeid ||
                        Convert.ToInt32(row.ItemArray[0]) == CON.OwnerType.OrganizationId ||
                        Convert.ToInt32(row.ItemArray[4]) == 1)
                        row.Delete();
                }
                _dsOwnerTypes.Tables[0].AcceptChanges();
            }
            else
            {
                foreach (DataRow row in _dsOwnerTypes.Tables[0].Rows)
                {
                    if (Convert.ToInt32(row.ItemArray[0]) == CON.OwnerType.RealEstateIndividualId ||
                        Convert.ToInt32(row.ItemArray[0]) == CON.OwnerType.RealEstateOrganizationId ||
                        Convert.ToInt32(row.ItemArray[4]) == 1)
                        row.Delete();
                }
                _dsOwnerTypes.Tables[0].AcceptChanges();
            }
        }

        _ownerTypeDataView = _dsOwnerTypes.Tables[0].AsDataView();
        _ownerTypeDataView.Sort = "OWNER_TYPE_NAME asc";
        Helper.LoadList(ddlOwnerType, _ownerTypeDataView.ToTable(), "OWNER_TYPE_NAME", "OWNER_TYPE_ID", true);
    }
    private void LoadHelpText()
    {
        if (Helper.IsModern())
        {
            ddlOwnerTitle.Attributes.Add("data-content", "What’s this ?");

            ddlOwnerTitle.Attributes.Add("placeholder", "Managing employee");
            ddlOwnerTitle.Attributes.Add("data-toggle", "popover");
            ddlOwnerTitle.Attributes.Add("data-placement", "right");
            ddlOwnerTitle.Attributes.Add("data-trigger", "focus");
            trOwnerTitlesHelpText.Visible = false;
        }
        else
        {
            trOwnerTitlesHelpText.Visible = true;
        }
    }

    public override void LoadData(DataRow addressRow)
    {
        if (!SessionVarRetriever.IsNewOwner)
        {
            LoadAddressInfo();
        }
        LoadHelpText();
        LoadOwnerTypes();
        SelectOwnerTypes();
        //this.AffiliationTypeVisible = true;
        //this.PercentageOfOwnershipVisible = true;
        //this.OwnerTitleVisible = true;
        SetControlVisibility();
        _isEdit = addressRow != null;
        ucAddress.LoadState();
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 = true;
        }
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.Administrator))
        {
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 = !regIsPending;
        }
        else
        {
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 = false;
        }

        if (!SessionVarRetriever.IsNewOwner)
        {
            if (addressRow != null)
            {
                _address.Load(addressRow);
                _ownerInfo.Load(addressRow);
                hdnRegOwnerID.Value = _ownerInfo.RegOwnerId.ToString();
                txtBirthDate.Text = _ownerInfo.DOB.HasValue ? Convert.ToDateTime(_ownerInfo.DOB).ToString("d") : string.Empty;
                //txtEndDate.Text = Convert.ToDateTime(_ownerEndDate).ToString("MM/dd/yyyy");
                if (txtBirthDate.Text == "1/1/0001" || txtBirthDate.Text == "1/1/1901") txtBirthDate.Text = string.Empty;
                txtSSN.Text = _ownerInfo.TaxId;
                nbPercentage.Text = _ownerInfo.PercentageOfOwnership.ToString();
                txtEffectiveDate.Text = _ownerInfo.BeginDate.HasValue
                    ? Convert.ToDateTime(_ownerInfo.BeginDate).ToString("d")
                    : DateTime.Now.ToString("d");
                txtEndDate.Text = _ownerInfo.EndDate.ToString("d");
                if (String.IsNullOrEmpty(this.WorkflowPage.MedicaidID))
                {

                    txtEndDate.Enabled = false;
                }
                else
                {

                    txtEndDate.Enabled = true;
                }



                if (Helper.GetString("REG_OWNER_TYPE_ID", addressRow).Trim() != string.Empty)
                    ddlOwnerType.SelectedValue = _ownerInfo.RegOwnerTypeId.ToString();
                if (ddlOwnerType.SelectedIndex > 0)
                {
                    GetAffliationType();
                    ddlOwnerType_SelectedIndexChanged(ddlOwnerType, null);
                }

                if (_ownerInfo.AffiliationTypeId > 0)
                    ddlAffliationType.SelectedValue = _ownerInfo.AffiliationTypeId.ToString();
                if (ddlOwnerType.SelectedValue == OWNER_TYPE_PERSON || ddlOwnerType.SelectedValue == OWNER_TYPE_MANAGING_EMPLOYEE || ddlOwnerType.SelectedValue == ADDITIONAL_TYPE_SUBCONTRACTOR_PERSON || ddlOwnerType.SelectedValue == ADDITIONAL_TYPE_SUPPLIER_PERSON || ddlOwnerType.SelectedValue == ADDITIONAL_TYPE_EMPLOYEE)
                {
                    lblBirthDateRequiredMarker.Visible = true;
                    RFVBirthDate.Enabled = true;
                    if (ddlOwnerType.SelectedValue == OWNER_TYPE_MANAGING_EMPLOYEE)
                        this.PercentageOfOwnershipVisible = false;
                }
                else
                {
                    lblBirthDateRequiredMarker.Visible = false;
                    RFVBirthDate.Enabled = false;
                }
                if (SessionVarRetriever.IsAdditionalDisclosure)
                {
                    lblOwnerType.Text = "Other Disclosure Type";
                    // lblEffectiveDate.Text = "Effective Date";
                    // lblEndDate.Text = "End Date";
                }


                // disable the owner page if they aren't DODD editors
                if (this.WorkflowPage.HasActiveDODDSpecialty)
                {
                    Helper.SetReadOnly(this, true, "formFieldReadOnly");
                }
                else
                {
                    Helper.SetReadOnly(this, !Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName));
                }
            }


            ucAddress.CopyPropertiesFrom(_address);
            ucAddress.NameSectionVisible = ucAddress.IsIndividual;
            ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
            if (!string.IsNullOrEmpty(_address.Title) && _address.Title != "0")
            {
                ddlOwnerTitle.ClearSelection();
                ddlOwnerTitle.Items.FindByText(_address.Title).Selected = true;
            }
        }
        else if (SessionVarRetriever.IsNewOwner)
        {
            _ownerInfo.Load(null);

            if (addressRow == null)
            {
                txtBirthDate.Text = txtEffectiveDate.Text = txtEndDate.Text = txtSSN.Text = nbPercentage.Text = hdnRegOwnerID.Value = hdnRegAddressID.Value = "";
                ucAddress.AddressTypeTitle = ucAddress.City = "";
                ucAddress.AddressTypeId = 0;
                ucAddress.ContactName = ucAddress.County = ucAddress.Email1 = ucAddress.Email2 = ucAddress.FaxNumber1 = ucAddress.FaxNumber2 = "";
                ucAddress.FirstName = ucAddress.LastName = ucAddress.Latitude = ucAddress.Longitude = ucAddress.MiddleName = ucAddress.OrgName = "";
                ucAddress.PhoneExt1 = ucAddress.PhoneExt2 = ucAddress.PhoneNumber1 = ucAddress.PhoneNumber2 = ucAddress.Quadrant = ucAddress.StreetAddress = "";
                ucAddress.Title = ucAddress.UnitAddress = ucAddress.Ward = ucAddress.Zip4 = ucAddress.Zip5 = ucAddress.State = "";

                ddlAffliationType.SelectedIndex = ddlOwnerTitle.SelectedIndex = ddlOwnerType.SelectedIndex = -1;
            }
            txtEffectiveDate.Text = _ownerInfo.BeginDate.HasValue
                      ? Convert.ToDateTime(_ownerInfo.BeginDate).ToString("MM/dd/yyyy")
                      : DateTime.Now.ToString("MM/dd/yyyy");
            txtEndDate.Text = _ownerInfo.EndDate.ToString("MM/dd/yyyy");
            if (String.IsNullOrEmpty(this.WorkflowPage.MedicaidID))
            {
                txtEndDate.Enabled = false;
            }
            else
            {
                txtEndDate.Enabled = true;
            }


        }
        if (_isEdit)
        {

            hdnRegAddressID.Value = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
            _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);
        }
        else
        {
            hdnRegAddressID.Value = "-1";
            _addressRegId = -1;
        }
        if (SessionVarRetriever.IsAdditionalDisclosure)
        {
            lblOwnerType.Text = "Other Disclosure Type";
            // lblEffectiveDate.Text = "Effective Date";
            // lblEndDate.Text = "End Date";
        }

        if (ucAddress.SaveButtonClientID != SaveButtonClientID)
            ucAddress.SaveButtonClientID = SaveButtonClientID;
        SetVisibleFields();
        txtEffectiveDate.Attributes.Remove("disabled");
    }

    //Check if the information already exists in the database
    private bool AlreadyExists()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");
        return Helper.HasRows(ds);
    }


    public override bool SaveData()
    {
        // OHPNM-14088
        if (this.WorkflowPage.OwnerInfoPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }

        string individualName = string.Concat(ucAddress.FirstName.Trim(), string.IsNullOrEmpty(ucAddress.MiddleName.Trim()) ? string.Empty : " ", ucAddress.MiddleName.Trim(),
            string.IsNullOrEmpty(ucAddress.LastName.Trim()) ? string.Empty : " ", ucAddress.LastName.Trim());

        var orgName = !string.IsNullOrEmpty(ucAddress.OrgName) ? ucAddress.OrgName : " ";

        _address.CopyPropertiesFrom(ucAddress);
        _address.AddressTypeId = _addressTypeId;
        _address.RegId = this.WorkflowPage.RegistrationId;
        if (_isPerson)
        {
            _address.ContactType = CON.ContactType.Individual;
        }
        else
        {
            _address.ContactType = CON.ContactType.Organization;
        }

        _addressParms = _address.CreateParameterList(_address);

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_OWNER_TYPE_ID", ddlOwnerType.SelectedValue);
        parms.Add("NAME", _isOrg ? orgName : individualName);
        parms.Add("FIRST_NAME", ucAddress.FirstName.Trim());
        parms.Add("MIDDLE_INITIAL", ucAddress.MiddleName.Trim());
        parms.Add("LAST_NAME", ucAddress.LastName.Trim());
        parms.Add("TAX_ID", Helper.StripNonNumerics(txtSSN.Text));
        if (ddlOwnerType.SelectedValue == "18")
        {
            parms.Add("PERCENTAGE_OF_OWNERSHIP", "0");
        }
        else
        {
            parms.Add("PERCENTAGE_OF_OWNERSHIP", nbPercentage.Text); // Custom code by default 
        }
        parms.Add("DOB", string.IsNullOrEmpty(txtBirthDate.Text) ? null : Convert.ToDateTime(txtBirthDate.Text).ToString());
        parms.Add("BEGIN_DATE", string.IsNullOrEmpty(txtEffectiveDate.Text) ? DateTime.Now.ToString() : Convert.ToDateTime(txtEffectiveDate.Text).ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("AFFILIATION_TYPE", ddlAffliationType.SelectedValue);
        parms.Add("Title", ((ddlOwnerType.SelectedItem.Text == OWNER_TYPE_MANAGING_EMPLOYEE && string.IsNullOrEmpty(ddlOwnerTitle.SelectedValue)) ?  "11" : ddlOwnerTitle.SelectedValue));
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!SessionVarRetriever.IsNewOwner)
        {
            // Update
            if (!string.IsNullOrEmpty(hdnRegAddressID.Value) && hdnRegAddressID.Value != "-1")
            {
                _address.AddressId = Convert.ToInt32(hdnRegAddressID.Value);
                _addressParms.Add("REG_ADDRESS_ID", _address.AddressId.ToString());
                _address.Update(_address, _addressParms);
            }
            parms.Add("REG_ADDRESS_ID", _address.AddressId.ToString());
            parms.Add("REG_OWNER_ID", hdnRegOwnerID.Value);
            parms.Add("END_DATE", string.IsNullOrEmpty(txtEndDate.Text) ? "12/31/2299" : txtEndDate.Text.Trim());
            parms.Add("Modified_Status_Type_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
            psc.UpdateRegistrationDataWithParams("updateREG_OWNERCustom", parms);

        }
        else
        {
            if (string.IsNullOrEmpty(hdnRegAddressID.Value) || hdnRegAddressID.Value == "-1")
                _address.AddressId = _address.Insert(_address, _addressParms);
            if (_address.AddressId > 0)
            {
                // Insert Owner if it does not already exist
                if (SessionVarRetriever.IsNewOwner)
                {
                    parms.Add("Modified_Status_Type_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());
                    parms.Add("END_DATE", "12/31/2299");
                    parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("REG_ADDRESS_ID", _address.AddressId.ToString());
                    psc.InsertRegistrationDataTable("OWNER", parms);
                }
            }
        }

        ucAddress.Confirmed = true;
        SessionVarRetriever.IsNewOwner = false;
        return true;
    }

    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOwnerInfo";
        this.Page.Validators.Add(val);
    }


    public override bool ValidateData()
    {
        bool result = true;
        if (ddlOwnerType.SelectedItem.Text == OWNER_TYPE_PERSON)
        {
            DateTime tempDate;
            if (string.IsNullOrEmpty(txtBirthDate.Text) || !DateTime.TryParse(txtBirthDate.Text.Trim(), out tempDate))
            {
                AddError("Select a valid Birth Date");
                result = false;
            }

        }


        DateTime tempDatevar;
        if (DateTime.TryParse(txtBirthDate.Text.Trim(), out tempDatevar))
        {
            if (tempDatevar > DateTime.Now || tempDatevar < DateTime.Now.AddYears(-100))
            {
                AddError("* Birth Date can not be a future date and cannot result in an age over 100 years.");
                result = false;
            }
        }
        
        //OHPNM-13679,13930 : PROD - EBI - SSN Invalid error
        if (!string.IsNullOrEmpty(txtSSN.Text) )
        {
            if (!Methods.IsSsnValid(txtSSN.Text))
            {
                if (lblTaxID.Text.Contains("SSN"))
                {
                    AddError("* Invalid SSN value");
                    result = false;
                }
            }
            else
            {
                result = true;
                //validate SSN/TAX_ID uniqueness per REG_ID
                Dictionary<string, string> parms = new Dictionary<string, string>
                {
                    {"REG_ID", this.WorkflowPage.RegistrationId.ToString()}
                };
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectOwnerAddressInfo", parms);
                if (Helper.HasRows(ds))
                {
                    DataTable dtOwners = ds.Tables[0];
                    foreach (DataRow dr in dtOwners.Rows)
                    {
                        string drSSN = dr["TAX_ID"].ToString();
                        string drOwnerTypeId = dr["REG_OWNER_TYPE_ID"].ToString();
                        string regOwnerId = dr["REG_OWNER_ID"].ToString();
                        int ownerIDType = int.Parse(dr["REG_OWNER_TYPE_ID"].ToString());
                        // only validate SSN if Owner Type is an individual - EIN has no validation
                        if (result)
                        {
                            bool checkDates = false;
                            if (!string.IsNullOrEmpty(hdnRegOwnerID.Value))
                            {
                                // If not the same owner then we need to check dates
                                if ((hdnRegOwnerID.Value != regOwnerId) && ddlOwnerType.SelectedValue == drOwnerTypeId && txtSSN.Text == drSSN)
                                {
                                    checkDates = true;
                                }
                            }
                            else
                            {
                                // If adding the same owner type and id we need to check dates
                                if (ddlOwnerType.SelectedValue == drOwnerTypeId && txtSSN.Text == drSSN)
                                {
                                    checkDates = true;
                                }
                            }

                            if (checkDates)
                            {
                                //OHPNM-11027  Check for date overlapping
                                DateTime beginDate = Helper.GetDateTime("BEGIN_DATE", dr);
                                DateTime endDate = Helper.GetDateTime("end_DATE", dr);
                                DateTime enteredBeginDate = Convert.ToDateTime(txtEffectiveDate.Text);
                                DateTime enteredEndDate = Convert.ToDateTime(txtEndDate.Text);

                                // Check if date range overlap
                                if (beginDate < enteredEndDate && endDate > enteredBeginDate)
                                {
                                    AddError("An active Owner with this same tax id already exists, if an update is required, update that record. ");
                                    result = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (trAffType.Visible == true)
        {
            rfvAffliationType.Enabled = true;
            rfvAffliationType.ErrorMessage = "*Select an AffliationType";
            if (ddlAffliationType.SelectedIndex == 0)
            {
                AddError("* Select an AffliationType");
                result = false;
            }

        }

        return result;
    }

    protected void ddlOwnerType_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlOwnerType.SelectedItem.Text)
        {
            case OWNER_TYPE_PERSON:
            case OWNER_TYPE_MANAGING_EMPLOYEE:
            case PROPERTY_TYPE_PERSON:
            case ADDITIONAL_TYPE_SUBCONTRACTOR_PERSON:
            case ADDITIONAL_TYPE_SUPPLIER_PERSON:
            case ADDITIONAL_TYPE_EMPLOYEE:
                {
                    _addressTypeId = SessionVarRetriever.IsRealEstateOwner ? CON.OwnerType.RealEstateIndividualId
                                    : SessionVarRetriever.IsAdditionalDisclosure ? ((ddlOwnerType.SelectedItem.Text == ADDITIONAL_TYPE_SUBCONTRACTOR_PERSON) ? CON.AddressType.Subcontractor : (ddlOwnerType.SelectedItem.Text == ADDITIONAL_TYPE_SUPPLIER_PERSON) ? CON.AddressType.Supplier : CON.AddressType.ProviderInfo)
                                    : CON.AddressType.OwnerInfo;
                    _isPerson = true;
                    _isOrg = false;
                    ucAddress.ContactType = CON.ContactType.Individual;
                    trDOB.Visible = true;
                    trSSN.Visible = true;
                    trEffectiveDate.Visible = true;
                    break;
                }
            case OWNER_TYPE_CORPORATION:
            case PROPERTY_TYPE_CORPORATION:
            case ADDITIONAL_TYPE_SUBCONTRACTOR_CORPORATION:
            case ADDITIONAL_TYPE_SUPPLIER_CORPORATION:
            case ADDITIONAL_TYPE_OTHER_PROVIDER:
                {
                    _addressTypeId = SessionVarRetriever.IsRealEstateOwner ? CON.OwnerType.RealEstateOrganizationId
                                    : SessionVarRetriever.IsAdditionalDisclosure ? ((ddlOwnerType.SelectedItem.Text == ADDITIONAL_TYPE_SUBCONTRACTOR_CORPORATION) ? CON.AddressType.Subcontractor : (ddlOwnerType.SelectedItem.Text == ADDITIONAL_TYPE_SUPPLIER_CORPORATION) ? CON.AddressType.Supplier : CON.AddressType.ProviderInfo)
                                    : CON.AddressType.OwnerInfo;
                    _isOrg = true;
                    _isPerson = false;
                    ucAddress.ContactType = CON.ContactType.Organization;
                    trSSN.Visible = trEffectiveDate.Visible = true;

                    trEffectiveDate.Visible = true;

                    break;
                }
        }
        if (ddlOwnerType.SelectedItem.Text.Equals(OWNER_TYPE_MANAGING_EMPLOYEE))
        {
            divPercentageOfOwnership.Visible = false;
            //if (ddlAffliationType.SelectedIndex == 0)
            //{
            //    GetAffliationType();
            //}
        }
        else
        {
            divPercentageOfOwnership.Visible = true;

            ddlAffliationType.Items.Remove(ddlAffliationType.Items.FindByValue("10"));
        }
        txtSSN.MaxLength = 9;
        txtSSN.Text = new string(txtSSN.Text.Where(char.IsDigit).ToArray());

        if (_isPerson)
        {
            ucAddress.OrgName = string.Empty;
            ucAddress.NameSectionVisible = false;
            ucAddress.OrgNameVisible = false;
            ucAddress.FirstNameSectionVisible = true;
        }
        else
        {
            ucAddress.NameSectionVisible = false;
            ucAddress.OrgNameVisible = true;
            ucAddress.FirstNameSectionVisible = false;
        }

        lblBirthDateRequiredMarker.Visible = _isPerson;
        if (_isOrg)
        {
            lblBirthDateRequiredMarker.Visible = RFVBirthDate.Enabled = false;


            divDOB.Visible = false;
        }
        else
        {
            lblBirthDateRequiredMarker.Visible = RFVBirthDate.Enabled = true;

            divDOB.Visible = true;

        }
        //ADDITIONAL_TYPE_SUPPLIER_CORPORATION,,

        if ((ddlOwnerType.SelectedItem.Text.Equals(ADDITIONAL_TYPE_SUPPLIER_CORPORATION) || ddlOwnerType.SelectedItem.Text.Equals(ADDITIONAL_TYPE_SUBCONTRACTOR_CORPORATION)
            || ddlOwnerType.SelectedItem.Text.Equals(ADDITIONAL_TYPE_OTHER_PROVIDER) || _isOrg) && !ddlOwnerType.SelectedItem.Text.Equals(ADDITIONAL_TYPE_EMPLOYEE))
        {
            lblTaxID.Text = "Tax ID*";
            RequiredFieldValidator4.Enabled = true;
            RequiredFieldValidator4.ErrorMessage = "* Tax ID is required";

            cvSSN.ErrorMessage = "* Tax ID is invalid";
        }
        else
        {
            RequiredFieldValidator4.Enabled = true;
            lblTaxID.Text = "SSN *";
            RequiredFieldValidator4.ErrorMessage = "* SSN is required";

            cvSSN.ErrorMessage = "* SSN is invalid";
        }

        ucAddress.AddressWrapperVisible = true;
        ucAddress.SetVisibleFields();
        SetVisibleFields();
    }

    public override string ValidationGroup
    {
        get { return "valOwnerInfo"; }
    }

    public override string Title
    {
        get { return "Owner Information"; }
    }

    public override string IdText
    {
        get { return "ucOwnerInfo_" + this.WorkflowPage.RegistrationId; }
    }

    public void GetOwnerAddressInfo(int regId, int addressTypeId)
    {
        throw new NotImplementedException();
    }
    protected void ddlOwnerTitle_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlAffliationType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void GetOwnerTitle()
    {

        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        ddlOwnerTitle.Items.Clear();
        DataSet dataSet = _svc.GetOwnerTitle();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlOwnerTitle, dt, "DSC_Title", "Managing_Employee_Title_ID", true);

    }
    private void GetAffliationType()
    {

        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        ddlAffliationType.Items.Clear();
        DataSet dataSet = _svc.GetAffliationType();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlAffliationType, dt, "AFFILIATION_TYPE_DESC", "AFFILIATION_TYPE_ID", true);

    }


}