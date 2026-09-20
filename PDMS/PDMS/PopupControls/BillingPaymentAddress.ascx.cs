using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_BillingPaymentAddress : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private int _addressTypeId = CON.AddressType.Billing;
    private int _addressRegId = -1;
    private Address _address = new Address();
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
		
        // OHPNM-8313 - if they have 490 DODD specialty and update isn't being initiated by DODD, don't let them change this info: Name, Billing Payment Address, Ownership
        if ((this.WorkflowPage.HasActiveDODDSpecialty && this.WorkflowPage.WaiverServiceUpdateTypeID != 1) 
            || (this.WorkflowPage.WaiverTypeID == CON.WaiverApplicationTypeID.NonMedicaidDODD)) // OHPNM-15326 - DD Non medicaid provider shoud not update Billing payment address in PNM
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            this.WorkflowPage.BillingPageIsReadOnly = true;
        }
        else
        {
            this.WorkflowPage.BillingPageIsReadOnly = false;
        }

        loadsession();
        prov_Same.InputAttributes.Add("aria-label", "Same as Practice Location");
        prov_override.InputAttributes.Add("aria-label", "Override Address Validation");
    }

    private void SetVisibleFields()
    {
        ucAddress.AddressTypeVisible = true;
        ucAddress.ZipExtVisible = true;
        ucAddress.IsZipExtRequired = true;
        ucAddress.Phone1Visible = true;
        ucAddress.Fax1Visible = true;
        //ucAddress.NameSectionVisible = true;
        ucAddress.Cell1Visible = false;
	    ucAddress.Cell2Visible = false;
	    ucAddress.ContactVisible = true;
	    ucAddress.Email1Visible = true;
	    ucAddress.Email2Visible = false;
	    ucAddress.Fax2Visible = true;
	    ucAddress.OfficeMgrVisible = false;
	    ucAddress.Phone2Visible = true;
	    ucAddress.PhoneExt1Visible = true;
	    ucAddress.PhoneExt2Visible = true;
	    ucAddress.VerifyAddress = false;
	    ucAddress.GetGeocode = true;
        ucAddress.TitleRequired = false;

        //ucAddress.FirstNameSectionVisible = true;
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
	    ucAddress.RegId = WorkflowPage.RegistrationId;
	    ucAddress.AddressTypeId = CON.AddressType.Billing;
	    base.OnLoad(e);
        Page.Title = CON.AddressPage.BillingAndPayment;
        ucAddress.PageTitle = CON.AddressPage.BillingAndPayment;
    }
    public override void LoadControlData()
    {
        LoadAddressInfo();
        SetVisibleFields();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.Billing.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_HISTORY", parms);
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
        DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Billing);
        DataTable dtAddressInfo = Helper.HasRows(ds) ? ds.Tables["AddressInfo"] : null;
        this.DataList = dtAddressInfo;
        if (Helper.HasRows(this.DataList))
        {
            DataRow row = this.DataList.Rows[0];
            this.LoadData(row);
            if (Helper.GetInt("ADDRESS_TYPE_ID", row) > 0)
                ucAddress.AddressTypeId = _addressTypeId;
        }
        else
        {
            //ucAddress.AddressTypeId = -1;
            this.LoadData(null);
        }
    }
    public override void LoadData(DataRow addressRow)
    {
	    bool isEdit = false;
	    isEdit = addressRow != null;
	    

	    DataSet dsPractice = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.Billing);
	    bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
	    //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
	    DataRow drPractice = dsPractice.Tables[0].Rows[0];
	    // string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);
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
			ucAddress.EnableState = false;
	    }

	    hidIsEdit.Text = isEdit.ToString();
        ucAddress.LoadState();
        if (addressRow != null)
	    {
			_address.Load(addressRow, drPractice);
            ucAddress.CopyPropertiesFrom(_address);
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
		    _address.Load(drPractice);
	    }
	    
	    ucAddress.NameSectionVisible = ucAddress.IsIndividual;
	    //ucAddress.OrgNameVisible = !ucAddress.IsIndividual;

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
                ucAddress.NameVisible = false;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = true;
                ucAddress.FirstNameSectionVisible = true;
                ucAddress.TitleVisible = true;
                ucAddress.NameVisible = false;
                ucAddress.ContactType = CON.ContactType.Individual;
            }
        }
        else
        {
            if (drPractice != null && Methods.GetString("CONTACT_TYPE", drPractice) == CON.ContactType.Organization)
            {
                ucAddress.OrgNameVisible = true;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                ucAddress.TitleVisible = true;
                ucAddress.NameVisible = false;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = true;
                ucAddress.FirstNameSectionVisible = true;
                ucAddress.TitleVisible = true;
                ucAddress.NameVisible = false;
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
    public override bool SaveData()
    {
        // OHPNM-13974 - if this page is read only, don't validate or save
        if (this.WorkflowPage.BillingPageIsReadOnly)
        {
            // this is a read only page; no need to validate or save
            return true;
        }
        ucAddress.VerifyAddress = true;
        ucAddress.OverrideAddressValidation = prov_override.Checked;
        Page.Validate("BillingPaymentAddress");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("BillingPaymentAddress") && !v.IsValid)
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
            if (Request.Cookies["countyValue"] != null) ucAddress.County = Request.Cookies["countyValue"].Value;
            if (Request.Cookies["countydisplay"] != null)
            {
                ucAddress.County = Request.Cookies["countydisplay"].Value;
                _address.County = Request.Cookies["countydisplay"].Value;
            }
            else if (Request.Cookies["countyValue"] != null)
            {

                ucAddress.County = Request.Cookies["countyValue"].Value;
                _address.County = Request.Cookies["countyValue"].Value;
            }
        }

        _address.CopyPropertiesFrom(ucAddress);
        _address.RegId = this.WorkflowPage.RegistrationId;
        _address.AddressTypeId = _addressTypeId;        
        var parms = _address.CreateParameterList(_address);

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit)
        {
	        if (_addressRegId <= 0 && !string.IsNullOrEmpty(hidID.Text))
	        {
		        _addressRegId = Convert.ToInt32(hidID.Text);
	        }
	        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
	        parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _address.Update(_address, parms);
        }
        else
        {
	        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _addressRegId = _address.Insert(_address, parms);
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
            DataSet ds = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Other);
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


    protected void prov_Same_CheckedChanged(object sender, EventArgs e)
    {
        /*================>  PLEASE DO NOT DELETE THIS NOTE  <==================================|
	    |                                                                                       |
	    |  If the user checks the "Same as Practice Location" check box                         |
	    |                                                                                       |
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
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.Billing); // REG_PROVIDER Info
		
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
					ucAddress.NameSectionVisible = ucAddress.IsIndividual;
				    ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
                }
			    else
			    {
				    if (Helper.HasRows(dsProvider))
				    {
					    DataRow provAddress = dsProvider.Tables[0].Rows[0];
					    ucAddress.IsIndividual =false;
					    ucAddress.OrgName = provAddress["NAME"].ToString();
                        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
					    ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
                    }
			    }
                LoadData(dsPrimaryPractice.Tables[0].Rows[0]);
		    }
	    }
	   
    }


    public override string ValidationGroup
    {
	    get { return "BillingPaymentAddress"; }
    }

    public override string Title
    {
	    get { return "Billing Location"; }
    }

    public override string IdText
    {
	    get { return "ucBillingPaymentAddress_" + this.WorkflowPage.RegistrationId; }
    }

    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "billingPaymentAddress";
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
            Session["Override_prop"] = "billingPaymentAddress";
        }
        else
        {
            if (Session["Override_prop"] != null)
            {
                if (Session["Override_prop"].ToString() == "primaryContactAddress" ||
                    Session["Override_prop"].ToString() == "primaryServiceAddress" ||
                    Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
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
        lblTitle.Text = "Billing/Payment Address History";
        ucBillingPaymentAddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

}
