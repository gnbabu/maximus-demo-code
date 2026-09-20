using MAXIMUS.Core.Libraries;
using Presentation;
using Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HomeOfficeAddress : BaseSectionControl, IHomeOfficeAddressView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

	#region Private variables
    private int _addressTypeId = CON.AddressType.HomeOffice;
    private int _addressRegId = -1;
    private HomeOfficeAddressPresenter _presenter;

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
    #endregion

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

    public HomeOfficeAddressPresenter Presenter
    {
	    get
	    {
		    if (_presenter == null)
		    {
				_presenter = new HomeOfficeAddressPresenter(this);
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

    public string SaveButtonClientID
    {
	    get;
	    set;
    }

    private void SetVisibleFields()
    {
	    ucAddress.AddressTypeVisible = true;
	    ucAddress.Cell1Visible = false;
	    ucAddress.Cell2Visible = false;
	    ucAddress.ContactVisible = true;
	    ucAddress.Email1Visible = true;
	    ucAddress.Email2Visible = false;
	    ucAddress.Fax2Visible = true;
        ucAddress.Fax1Visible = true;
        ucAddress.OfficeMgrVisible = false;
	    ucAddress.Phone2Visible = true;
	    ucAddress.PhoneExt1Visible = true;
	    ucAddress.PhoneExt2Visible = true;
        ucAddress.Phone1Visible = true;
        ucAddress.VerifyAddress = false;
        ucAddress.ZipExtVisible = true;
        ucAddress.IsZipExtRequired = true;
      
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
        LoadAgencyInfo();
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
        ucAddress.PageTitle = CON.AddressPage.HomeAndOffice;
        Page.Title = CON.AddressPage.HomeAndOffice;
    }

    public override void LoadControlData()
    {
	    LoadAddressInfo();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.HomeOffice.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }

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

    private void LoadAgencyInfo()
    {
        if (this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88")
        {
            prov_Agency.Visible = true;
        }
    }

    public override void LoadData(DataRow addressRow)
    {
        bool isEdit = false;
	    isEdit = addressRow != null;
	    ucAddress.LoadState();
	
	    DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.HomeOffice);
	    bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
	    //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
	    DataRow drProvider = dsProvider.Tables[0].Rows[0];
	    
	    if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected,
		    this.WorkflowPage.CurrentTaskName))
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
            ucAddress.CopyPropertiesFrom(Model);
            if (Methods.Exists(addressRow, "ADDRESS_VALIDATION_OVERRIDE"))
            {
                var avo = addressRow["ADDRESS_VALIDATION_OVERRIDE"].ToString();
                if (avo == "1")
                    prov_override.Checked = true;
                else
                    prov_override.Checked = false;
            }
        }
	    else
	    {
		    Model.Load(drProvider);
	    }

	    
       // ucAddress.State = !ucAddress.IsIndividual?"OH":"";
        if (isEdit)
	    {
		    
		    hidID.Text = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
		    _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);            
        }
        else
	    {
           
		    hidID.Text = string.Empty;
		    _addressRegId = -1;
        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
        if (addressRow != null)
        {
            if (Methods.GetString("CONTACT_TYPE", addressRow) == CON.ContactType.Organization)
            {
                ucAddress.OrgNameVisible = true;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                ucAddress.TitleVisible = true;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = true;
                ucAddress.FirstNameSectionVisible = true;
                ucAddress.TitleVisible = true;
                ucAddress.ContactType = CON.ContactType.Individual;
            }
        }
        else
        {
            if (dsProvider != null && Methods.GetString("CONTACT_TYPE", drProvider) == CON.ContactType.Organization)
            {
                ucAddress.OrgNameVisible = true;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                ucAddress.TitleVisible = true;
                 ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = true;
                ucAddress.FirstNameSectionVisible = true;
                ucAddress.TitleVisible = true;
                ucAddress.ContactType = CON.ContactType.Individual;
            }
        }
        //OHPNM-11112 && OHPNM-11339
        if (Session["rblContactTypeVal"] != null)
        {
            if (Session["rblContactTypeVal"].ToString() == CON.ContactType.Organization)
            {
                ucAddress.OrgNameVisible = true;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            if (Session["rblContactTypeVal"].ToString() == CON.ContactType.Individual)
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = true;
                ucAddress.FirstNameSectionVisible = true;
                ucAddress.ContactType = CON.ContactType.Individual;
            }
        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
		
        SetVisibleFields();
        
    }


    public void SetDefaultState()
    {
        
    }
    public override bool SaveData()
    {
        ucAddress.VerifyAddress = true;
        ucAddress.OverrideAddressValidation = prov_override.Checked;
        Page.Validate("valHomeOfficeAddress");
        
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valHomeOfficeAddress") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        if (string.IsNullOrEmpty(hidID.Text))
        {
	        Set_hidID(null);
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
        var parms = Model.CreateParameterList(Model);
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit)
        {
	        if (_addressRegId <= 0 && !string.IsNullOrEmpty(hidID.Text))
	        {
		        _addressRegId = Convert.ToInt32(hidID.Text);
	        }
	        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
	        parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _presenter.Update(Model, parms);
        }
        else
        {
	        parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _addressRegId = _presenter.Insert(Model, parms);
        }
        LoadAddressInfo();
        ucAddress.Confirmed = true;
        SetVisibleFields();

        //Remove session variable for addresses
        if (Session["rblContactTypeVal"] != null)
        {
            if (!string.IsNullOrEmpty(Session["rblContactTypeVal"] as string))
            {
                Session.Remove("rblContactTypeVal");
            }
        }

        return true;
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
        get { return "valHomeOfficeAddress"; }
    }

    public override string Title
    {
        get { return "Home Office Address"; }
    }

    public override string IdText
    {
        get { return "ucHomeOfficeAddress_" + this.WorkflowPage.RegistrationId; }
    }
	
    public void GetProviderAddressInfo(int regId, int addressTypeId)
    {
	    // ;
    }
    protected void prov_Same_CheckedChanged(object sender, EventArgs e)
    {
        /*================>  PLEASE DO NOT DELETE THIS NOTE  <==================================|
		|                                                                                       |
		|  If the user checks the "Same as Practice Location" check box                         |
		|   (which will be enabled by default when adding an address)                           |
		|   1. Check the "ENTITY_TYPE_ID" field from REG_PROVIDER.                              |
		|	  a. If this is an INDIVIDUAL [CON.ProviderCategoryTypeID.Individual] (1)           |
		|	     Populate the Individual Name fields (First, Middle, Last, etc)                 |
		|		    from the REG_PROVIDER table.                                                |
		|     b. If this is an ORGANIZATION [Anything other than 1]                             |  
		|	     Populate the Organization Name field from the REG_PROVIDER "NAME" field.       |
		|                                                                                       |   
		|   Regardless whether they choose A. or B. above,                                      |  
		|     populate the Address fields from the Primary Practice Address (Address Type = 1)  |
		|   If there is no Primary Practice row in the REG_ADDRESS table, as a default,         | 
		|    pull the address fields from the REG_PROVIDER table.                               |
		*======================================================================================*/
        DataSet dsPrimaryPractice = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.PrimaryPractice);
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.HomeOffice); // REG_PROVIDER Info

        var entityId = 0;
        var contactType = string.Empty;
        if (prov_Same.Checked)
        {
            if (Helper.HasRows(dsPrimaryPractice))
            {
                DataRow addressRow = dsProvider.Tables[0].Rows[0];
                entityId = Convert.ToInt32(addressRow["ENTITY_TYPE_ID"]);
                contactType = entityId == 1 ? "Individual" : "Organization";

                if (contactType == "Individual")
                {
                    ucAddress.IsIndividual = true;
                    ucAddress.OrgNameVisible = false;
                    ucAddress.NameSectionVisible = true;
                    //ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
                }
                else
                {
                   if (Helper.HasRows(dsProvider))
                    {
                        DataRow provAddress = dsProvider.Tables[0].Rows[0];
                        ucAddress.IsIndividual = false;
                        ucAddress.OrgName = provAddress["NAME"].ToString();
                        ucAddress.NameSectionVisible = false ;
                        ucAddress.OrgNameVisible = true;
                    }
                }
                LoadData(dsPrimaryPractice.Tables[0].Rows[0]);
            }
        }
    }
    
    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "homeOfficeAddress";
        }
        else
        {
            Session["Override_prop"] = "False";
        }

    }

    protected void loadsession()
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "homeOfficeAddress";
        }
        else
        {
            if (Session["Override_prop"] != null)
            {
                if (Session["Override_prop"].ToString() == "primaryContactAddress" ||
                    Session["Override_prop"].ToString() == "primaryServiceAddress" ||
                    Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
                    Session["Override_prop"].ToString() == "otherServiceLocations" ||
                    Session["Override_prop"].ToString() == "billingPaymentAddress" ||
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
        lblSpHistoryTitle.Text = "Home Office Address History";
        ucHomeOfficeAddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}