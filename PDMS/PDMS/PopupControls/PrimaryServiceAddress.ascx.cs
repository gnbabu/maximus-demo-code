using MAXIMUS.Core.Libraries;
using Presentation;
using Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PrimaryServiceAddress : BaseSectionControl, IPrimaryServiceAddressView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private int _addressTypeId = CON.AddressType.PrimaryPractice;
    private int _addressRegId = -1;
    private int _inMaintenance = -1;
    private PrimaryServiceAddressPresenter _presenter;
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


    public PrimaryServiceAddressPresenter Presenter
    {
	    get
	    {
		    if (_presenter == null)
		    {
			    _presenter = new PrimaryServiceAddressPresenter(this);
		    }
		    return _presenter;
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
    public bool IsDocumentUploaded
    {
        get
        {
            return ViewState["IsDocumentUploaded"] == null ? false : Convert.ToBoolean(ViewState["IsDocumentUploaded"]);
        }
        set
        {
            ViewState["IsDocumentUploaded"] = value;
        }
    }

    public int AddressID
    {
	    get
	    {
		    return ViewState["AddressID"] == null ? 0 : Convert.ToInt32(ViewState["AddressID"]);
	    }
	    set
	    {
		    ViewState["AddressID"] = value;
	    }
    }
    public Address Model { get; set; }
    public void GetProviderAddressInfo(int regId, int addressTypeId)
    {
	    throw new NotImplementedException();
    }

    public string SaveButtonClientID
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        LoadControlData();
        SetVisibleFields();
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
        loadsession();
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
        prov_override.InputAttributes.Add("aria-label", "Override Address Validation");
    }

    private void SetVisibleFields()
    {
        //OHPNM-3523 // OHPNM-16669 SAM530 Allow PT86,88,89 to update primary service address page
        if (!string.IsNullOrEmpty(this.WorkflowPage.MedicaidID)
            && ((this.WorkflowPage.MMISProviderTypeID == "86" || this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88"))
            && (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) && !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval))))
        {
            ucAddress.EnableAddressFields = false;
        }
        ucAddress.AddressTypeVisible = false;
        ucAddress.ZipExtVisible = true;
        ucAddress.IsZipExtRequired = true;
        ucAddress.Cell1Visible = false;
	    ucAddress.Cell2Visible = false;
	    ucAddress.ContactVisible = true;
	    ucAddress.Email1Visible = true;
	    ucAddress.Email2Visible = false;
	    ucAddress.Fax1Visible = true;
        ucAddress.Fax2Visible = true;
        ucAddress.OfficeMgrVisible = false;
	    ucAddress.Phone1Visible = true;
	    ucAddress.PhoneExt1Visible = true;
        ucAddress.Phone2Visible = true;
        ucAddress.PhoneExt2Visible = true;
	    ucAddress.VerifyAddress = false;        
        //ucAddress.OrgNameVisible = false;
        //ucAddress.FirstNameSectionVisible = false;
        //ucAddress.NameSectionVisible = false;

        ucAddress.Address1Title = "Primary Service Address*";


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

    protected override void OnLoad(EventArgs e)
    {
	    if (Model == null)
	    {
		    Model = new Address();
	    }

	    _presenter = Presenter;
        ucAddress.RegId = WorkflowPage.RegistrationId;
        base.OnLoad(e);
        Page.Title = CON.AddressPage.PrimaryService;
        ucAddress.PageTitle = CON.AddressPage.PrimaryService;
    }

    public override void LoadControlData()
    {
	    LoadProviderName();
	    LoadAddressInfo();

        GetDocumentDetailsCount();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.PrimaryPractice.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }
    }

    private void LoadProviderName()
    {
	    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
	    DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        DataRow drProvider = Helper.HasRows(ds) ? ds.Tables[0].Rows[0] : null;
        if (drProvider != null)
			txtProviderName.Text = Helper.GetString("NAME", drProvider);
        txtProviderName.Enabled = false;
		txtProviderName.BorderColor = Color.Blue;
    }

    private void LoadAddressInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, _addressTypeId);
        DataTable dtAddressInfo = Helper.HasRows(ds) ? ds.Tables["AddressInfo"] : null;
        this.DataList = dtAddressInfo;
        if (Helper.HasRows(this.DataList))
        {
            DataRow row = this.DataList.Rows[0];
            this.LoadData(row);
            if (Helper.GetInt("ADDRESS_TYPE_ID", row) > 0)
	            ucAddress.AddressTypeId = Helper.GetInt("ADDRESS_TYPE_ID", row);
        }
        else
        {
            this.LoadData(null);
        }
    }

    public override void LoadData(DataRow addressRow)
    {
        bool isEdit = false;
		isEdit = addressRow != null;
		ucAddress.LoadState();

		DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.PrimaryPractice);
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
		//DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);
        if (providerType == CON.ProviderCategoryTypeID.Individual.ToString())
        {
			divProvidername.Visible = true;
            ucAddress.FirstNameSectionVisible = false;
            ucAddress.OrgNameVisible = false;
        }
        else
        {
            divProvidername.Visible = false;
            ucAddress.FirstNameSectionVisible = false;
            ucAddress.OrgNameVisible = true;
            LoadServicesAddressInfo();
        }
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected,this.WorkflowPage.CurrentTaskName))
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

        hidIsEdit.Text = isEdit.ToString();
        
        if (addressRow != null)
        {
	        Model.Load(addressRow, drProvider);
            if (Methods.Exists(addressRow, "ADDRESS_VALIDATION_OVERRIDE"))
            {
                var avo = addressRow["ADDRESS_VALIDATION_OVERRIDE"].ToString();
                if (avo == "1")
                    prov_override.Checked = true;
                else
                    prov_override.Checked = false;
            }
        }
      
        ucAddress.CopyPropertiesFrom(Model);
        // ucAddress.OrgNameVisible = !ucAddress.IsIndividual;

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            // store off if they are in maintenance, so we can pass to sub-screens to disable them when needed ... not really sure why this screen isn't disabled in maintenance though =/
            _inMaintenance = 1;
        }
        else
        {
            _inMaintenance = 0;
        }

        if (isEdit)
        {

	        hidID.Text = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
            hdnPracticeChange.Value = Helper.GetString("ADDRESS1", addressRow).ToString();
            _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);
            OfficeHours.LoadOfficeInformation(this.WorkflowPage.RegistrationId, Helper.GetInt("REG_ADDRESS_ID", addressRow), _inMaintenance);
        }
        else
        {
	        hidID.Text = string.Empty;
	        _addressRegId = -1;
            OfficeHours.LoadOfficeInformation(this.WorkflowPage.RegistrationId, - 1, _inMaintenance);
        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
        SetVisibleFields();
    }

    private void LoadServicesAddressInfo()
    {
        if (this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88")
        {
            prov_Services_Address.Visible = true;
        }
    }

    public override bool SaveData()
    {
        ucAddress.VerifyAddress = true;
        ucAddress.OverrideAddressValidation = prov_override.Checked;
        Page.Validate("PrimaryServiceAddress");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("PrimaryServiceAddress") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        if (!ValidatePOBOX())
        {
           
            if(!IsDocumentUploaded)
            { return IsDocumentUploaded; }
            
        }
        if (string.IsNullOrEmpty(hidID.Text))
        {
            Set_hidID(null);
        }

        // validate office hours data if applicable
        if (OfficeHours.HasInputValue() && OfficeHours.ValidateData() == false)
        {
            return false;
        }
        if (string.IsNullOrEmpty(ucAddress.County))
        {
	        if (ViewState["hdnCounty"] != null && !string.IsNullOrEmpty(ViewState["hdnCounty"].ToString()))
	        {
		        ucAddress.County = ViewState["hdnCounty"].ToString();
	        }
            if (Request.Cookies["countydisplay"] != null)
            {
                ucAddress.County = Request.Cookies["countydisplay"].Value;
                Model.County = Request.Cookies["countydisplay"].Value;
            }
            else if (Request.Cookies["countyValue"] != null)
            {

                ucAddress.County = Request.Cookies["countyValue"].Value;
                Model.County = Request.Cookies["countyValue"].Value;
            }
        }

        Model.CopyPropertiesFrom(ucAddress);
        Model.RegId = this.WorkflowPage.RegistrationId;
        Model.AddressTypeId = _addressTypeId;

        var entityId = 0;
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.PrimaryContact);
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        entityId = Convert.ToInt32(drProvider["ENTITY_TYPE_ID"]);
        Model.ContactType = entityId == 1 ? CON.ContactType.Individual : CON.ContactType.Organization;

        var parms = Model.CreateParameterList(Model);
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit)
        {
	        if (_addressRegId <= 0 && !string.IsNullOrEmpty(hidID.Text))
	        {
		        _addressRegId = Convert.ToInt32(hidID.Text);
	        }
            if (!hdnPracticeChange.Value.Equals(ucAddress.StreetAddress))
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
            }	        
	        parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _presenter.Update(Model, parms);
        }
        else
        {
	        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Inserted.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _addressRegId = _presenter.Insert(Model, parms);
        }
        
        OfficeHours.SaveData(_addressRegId);
        LoadAddressInfo();
        ucAddress.Confirmed = true;
        SetVisibleFields();

        return true;
    }
    protected void PlaceholderUploadSectionControl_PreRender(object sender, EventArgs e)
    {

    }
    private void GetDocumentDetailsCount()
    {
        int documentuploadedCount=svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.SectionTypeID.PrimaryServiceAddress, string.Empty, string.Empty, null).Tables[0].Rows.Count;
        IsDocumentUploaded = documentuploadedCount > 0 ? true : false;
        
    }
    private bool ValidatePOBOX()
    {
        string streetAddress = ucAddress.StreetAddress.Trim().ToUpper();        
        bool valid = true;
        if ( streetAddress.Contains("PO B") ||
             streetAddress.Contains("P.O. B") ||
             streetAddress.Contains("P.O B") ||
             streetAddress.Contains("PO. B") ||
             streetAddress.Contains("P O B") ||
             streetAddress.Contains("P O. B") ||
             streetAddress.StartsWith("BOX") || streetAddress.StartsWith("P.O. Box") )    //OHPNM-16669 SAM530 -  Safe at Home Declaration do not display in revalidation.
        {
            if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !this.WorkflowPage.IsReapplication && !this.WorkflowPage.IsReactivation)
                valid = true;
            else
            {
                valid = false;
                //TODO Check For existance of the upload 
                mpepoboxmsg.Show();
            }
        }
        return valid;
    }

    public override bool ValidateData()
    {
        return true;
    }

    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, _addressTypeId);
            if (Helper.HasRows(ds))
            {
                hidID.Text = ds.Tables[0].Rows[0]["REG_ADDRESS_ID"].ToString();
            }
        }
        else
        {
            hidID.Text = row["REG_ADDRESS_ID"].ToString();
        }
    }

    public override string ValidationGroup
    {
        get { return "PrimaryServiceAddress"; }
    }

    public override string Title
    {
        get { return "Primary Service Address"; }
    }

    public override string IdText
    {
        get { return "ucPrimaryServiceAddress_" + this.WorkflowPage.RegistrationId; }
    }


    protected void btnUploadPOBox_Click(object sender, EventArgs e)
    {

    }

    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "primaryServiceAddress";
        }
        else
        {
            Session["Override_prop"] = "False";
        }

    }

    public void loadsession()
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "primaryServiceAddress";
        }
        else
        {
            //OHPNM-16669 SAM530 - For PT 86, 88, 89,  remove USPS validation from primary service address page during revalidation.
            if ((this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg && !this.WorkflowPage.IsReactivation && !this.WorkflowPage.IsReapplication)
                 && (this.WorkflowPage.MMISProviderTypeID == "86" || this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88"))
            {
                Session["Override_prop"] = "primaryServiceAddress";
            }
            if (Session["Override_prop"] != null)
            {
                if (Session["Override_prop"].ToString() == "primaryContactAddress" ||
                    Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
                    Session["Override_prop"].ToString() == "billingPaymentAddress" ||
                    Session["Override_prop"].ToString() == "otherServiceLocations" ||
                    Session["Override_prop"].ToString() == "homeOfficeAddress" ||
                    Session["Override_prop"].ToString() == "NursingFacilityAddress" ||
					Session["Override_prop"].ToString() == "Form1099Address")
                {
                    Session["Override_prop"] = "False";
                }
            }
            else { Session["Override_prop"] = "False"; }
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Primary Service Address History";
        ucPrimaryServiceAddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}