using MAXIMUS.Controllers.PDMS;
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

public partial class PopupControls_Form1099Address : BaseSectionControl, IForm1099AddressView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private int _addressTypeId = CON.AddressType.TaxForm1099;
    private int _addressRegId = -1;
    private int _taxFormRegId = -1;
	private Address _address = new Address();
	private Form1099 _form1099 = new Form1099();
	private Dictionary<string, string> _addressParms = new Dictionary<string, string>();
    private Dictionary<string, string> _taxParms  = new Dictionary<string, string>();
	private Form1099AddressPresenter _presenter;

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

    public Form1099AddressPresenter Presenter
    {
        get { return _presenter ?? (_presenter = new Form1099AddressPresenter(this)); }
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


    public Form1099 TaxModel { get; set; }
    public Address TaxAddress { get; set; }

    Form1099 IView<Form1099>.Model { get; set; }

	Address IView<Address>.Model { get; set; }

    public string SaveButtonClientID
    {
        get;
        set;
    }

    private void SetVisibleFields()
    {
        ucAddress.AddressTypeVisible = true;
        ucAddress.ZipExtVisible = true;
		ucAddress.IsZipExtRequired = true;
        ucAddress.Cell1Visible = false;
        ucAddress.Cell2Visible = false;
        ucAddress.ContactVisible = false;
        ucAddress.Email1Visible = true;
        ucAddress.Email2Visible = false;
        ucAddress.Fax1Visible = true;
        ucAddress.OfficeMgrVisible = false;
        ucAddress.Phone1Visible = true;        
        ucAddress.Phone2Visible = true;
        ucAddress.PhoneExt1Visible = true;
        ucAddress.PhoneExt2Visible = true;
        divDateDisplay.Visible = false;
        ucAddress.VerifyAddress = false;
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
        ucAddress.Is1099Page = true;
        LoadControlData();
        SetVisibleFields();
		loadsession();

        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        if (_address == null)
        {
	        _address = new Address();
        }

        if (_form1099 == null)
        {
			_form1099 = new Form1099();
        }

        _presenter = Presenter;
		ucAddress.RegId = WorkflowPage.RegistrationId;
        base.OnLoad(e);
        Page.Title = CON.AddressPage.Form1099Address;
        ucAddress.PageTitle = CON.AddressPage.Form1099Address;
    }

    public override void LoadControlData()
    {
        LoadAddressInfo();
        LoadForm1099Info();

        if (string.IsNullOrEmpty(txtEffectiveDate.Text) || txtEffectiveDate.Text == DateTime.MinValue.ToString("d") || txtEffectiveDate.Text == "1/1/1900")
        {
            txtEffectiveDate.Text = DateTime.Today.ToString("d");
        }

        txtEndDate.Text = CON.PNMDate.MaxDateString;
        txtEndDate.Enabled = false;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.TaxForm1099.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_1099_ADDRESS_HISTORY", parms);
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

    public override void LoadData(DataRow addressRow)
    {
        bool isEdit = false;
        isEdit = addressRow != null;
        

        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.TaxForm1099);
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
        ucAddress.LoadState();
        if (addressRow != null)
        {
            _address.Load(addressRow, drProvider);
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
	        _address.Load(drProvider);
        }
		
        
        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
        ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
		
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
                ucAddress.TitleVisible = false;
                ucAddress.NameVisible = false;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                ucAddress.TitleVisible = false;
                ucAddress.NameVisible = true;
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
                ucAddress.TitleVisible = false;
                ucAddress.NameVisible = false;
                ucAddress.ContactType = CON.ContactType.Organization;
            }
            else
            {
                ucAddress.OrgNameVisible = false;
                ucAddress.NameSectionVisible = false;
                ucAddress.FirstNameSectionVisible = false;
                //ucAddress.TitleVisible = true;
                ucAddress.NameVisible = true;
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

    private void LoadForm1099Info()
    {
        DataSet ds = Get1099FormDataByAddressId();
        DataTable dtFormInfo = Helper.HasRows(ds) ? ds.Tables[0] : null;
        DataTable regProviderInfo = Helper.HasRows(ds.Tables[1]) ? ds.Tables[1] : null;
        if (Helper.HasRows(dtFormInfo))
        {
            DataRow row = dtFormInfo.Rows[0];
            _form1099.Load(row);
            rbTaxTypeId.SelectedIndex = _form1099.TaxTypeId == 15 ? 0 : 1;
            txtTaxId.Text = _form1099.TaxId;
        }
        else
        {
            _form1099.Load(null);
            DataRow row = regProviderInfo.Rows[0];
            rbTaxTypeId.SelectedIndex = Methods.GetIntValue(row, "TAX_ID_TYPE_ID") == 15 ? 0 : 1;
            txtTaxId.Text = Methods.GetString("TAX_ID", row);            
        }

        
        rbTaxTypeId.Enabled = false;
        txtTaxId.Enabled = false;
        txtEffectiveDate.Text = _form1099.EffectiveDate.ToString("d");
        txtEndDate.Text = CON.PNMDate.MaxDateString;
        rblTaxExempt.SelectedIndex = _form1099.IsTaxExempt ? 0 : 1;
        rbFormW9.SelectedIndex = _form1099.IsFormW9 ? 0 : 1;
        rblForm147.SelectedIndex = _form1099.IsForm147 ? 0 : 1;
    }

    public override bool SaveData()
    {
        ucAddress.VerifyAddress = true;
        ucAddress.OverrideAddressValidation = prov_override.Checked;
        Page.Validate("valForm1099Address");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valForm1099Address") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

		var taxIdCheck = txtTaxId.Text = new string(txtTaxId.Text.Where(c => char.IsDigit(c)).ToArray());
		if (taxIdCheck.Length != 9)
		{
			cvSSN.ErrorMessage = "Tax ID Must be 9 digits.";
			RequiredFieldValidator4.Enabled = true;
			RequiredFieldValidator4.ErrorMessage = "* Tax ID is required";
			return false;
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
        _addressParms = _address.CreateParameterList(_address);

        _form1099.TaxTypeId = Convert.ToInt32(rbTaxTypeId.SelectedItem.Value);
		_form1099.TaxId = txtTaxId.Text;
		_form1099.EffectiveDate = Convert.ToDateTime(txtEffectiveDate.Text);
		_form1099.EndDate = Convert.ToDateTime(txtEndDate.Text);
		_form1099.IsTaxExempt = rblTaxExempt.Text != "0";
		_form1099.IsFormW9 = rbFormW9.Text != "0";
        _form1099.IsForm147 = rblForm147.Text != "0";

        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);

        if (isEdit)
        {
            if (_addressRegId <= 0 && !string.IsNullOrEmpty(hidID.Text))
            {
                _addressRegId = Convert.ToInt32(hidID.Text);
            }
            _addressParms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            _addressParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
            _addressParms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _presenter.Update(_address, _addressParms);		
        
        }
        else
        {
            _addressParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.NoChange.ToString());
            _addressParms.Add("ADDRESS_VALIDATION_OVERRIDE", prov_override.Checked ? "1" : "0");
            _addressRegId = _presenter.Insert(_address, _addressParms);           
        }
        if (_addressRegId > 0)
        {
            _form1099.AddressId = _addressRegId;
            //Check for 1099 record 

            DataSet ds = Get1099FormDataByAddressId();
            DataTable dtForm1099= Helper.HasRows(ds) ? ds.Tables[0] : null;
            if (Helper.HasRows(dtForm1099))
            {
                DataRow dr1099 = dtForm1099.Rows[0];
                _taxFormRegId = Helper.GetInt("REG_FORM_1099_INFO_ID", dr1099);
                 _taxParms = _form1099.CreateParameterList(_form1099);
                _taxParms.Add("REG_FORM_1099_INFO_ID", _taxFormRegId.ToString());
                _presenter.Update(_form1099, _taxParms);
            }
            else
            {

                _taxParms = _form1099.CreateParameterList(_form1099);
                _taxFormRegId = _presenter.Insert(_form1099, _taxParms);
                _form1099.RegFormId = _taxFormRegId;
            }
        }
        LoadAddressInfo();
		LoadForm1099Info();
        ucAddress.Confirmed = true;
        SetVisibleFields();
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
        get { return "valForm1099Address"; }
    }

    public override string Title
    {
        get { return "Form 1099 Address"; }
    }

    public override string IdText
    {
        get { return "ucForm1099Address_" + this.WorkflowPage.RegistrationId; }
    }
	
    void IForm1099AddressView.GetProviderAddressInfo(int regId, int addressTypeId)
    {
        //throw new NotImplementedException();
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
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.TaxForm1099); // REG_PROVIDER Info

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
                        ucAddress.IsIndividual = false;
                        ucAddress.OrgName = provAddress["NAME"].ToString();
                        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
                        ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
                    }
                }
                LoadData(dsPrimaryPractice.Tables[0].Rows[0]);
            }
			prov_billing.Checked = false;
		}
        
    }

    private DataSet Get1099FormDataByAddressId()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

        DataSet dsForm = RegistrationController.SelectRegistrationDataWithParams("usp_SelectREG_FORM_1099_INFO", parms);
        return dsForm;
    }

    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
		if (prov_override.Checked)
		{
			Session["Override_prop"] = "Form1099Address";
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
			Session["Override_prop"] = "Form1099Address";
		}
		else
		{
			if (Session["Override_prop"] != null)
			{
				if (Session["Override_prop"].ToString() == "primaryContactAddress" ||
					Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
					Session["Override_prop"].ToString() == "billingPaymentAddress" ||
					Session["Override_prop"].ToString() == "otherServiceLocations" ||
					Session["Override_prop"].ToString() == "homeOfficeAddress" ||
					Session["Override_prop"].ToString() == "NursingFacilityAddress" ||
					Session["Override_prop"].ToString() == "primaryServiceAddress")
				{
					Session["Override_prop"] = "False";
				}
			}
			else { Session["Override_prop"] = "False"; }
		}
	}

	protected void prov_Billing_CheckedChanged(object sender, EventArgs e)
	{
		/*================>  PLEASE DO NOT DELETE THIS NOTE  <==================================|
	   |                                                                                       |
	   |  If the user checks the "Same as Billing Location" check box                         |
	   |                                                                                       |
	   |   1. Check the "ENTITY_TYPE_ID" field from REG_PROVIDER.                              |
	   |	  a. If this is an INDIVIDUAL [CON.ProviderCategoryTypeID.Individual] (1)           |
	   |	     Populate the Individual Name fields (First, Middle, Last, etc)                 |
	   |		    from the REG_PROVIDER table.                                                |
	   |     b. If this is an ORGANIZATION [Anything other than 1]                             |  
	   |	     Populate the Organization Name field from the REG_PROVIDER "NAME" field.       |
	   |                                                                                       |   
	   |   Regardless whether they choose A. or B. above,                                      |  
	   |     populate the Address fields from the Billing Location Address (Address Type = 1)  |
	   |   If there is no Billing Location row in the REG_ADDRESS table, as a default,         | 
	   |    pull the address fields from the REG_PROVIDER table.                               |
	   *======================================================================================*/
		DataSet dsPrimaryPractice = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Billing);
		DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.TaxForm1099); // REG_PROVIDER Info

		var entityId = 0;
		var contactType = string.Empty;
		if (prov_billing.Checked)
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
						ucAddress.IsIndividual = false;
						ucAddress.OrgName = provAddress["NAME"].ToString();
						ucAddress.NameSectionVisible = ucAddress.IsIndividual;
						ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
					}
				}
				LoadData(dsPrimaryPractice.Tables[0].Rows[0]);
			}
			prov_Same.Checked = false;
		}
	}
    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblSpHistoryTitle.Text = "Form 1099 Address History";
        ucForm1099AddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}