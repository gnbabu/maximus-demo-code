using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Usercontrols_Address : System.Web.UI.UserControl
{
    #region " Member Variables "
    private bool _stateVisible = true;
	string prov_override = string.Empty;
	#endregion

	#region svc

	private PDMSService.PDMSServiceClient _svc;
	public bool IsIndividual = false;
   
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

	#region " Events "

	public event EventHandler StateChangedEvent;
	public event EventHandler CountyChangedEvent;
	public event EventHandler AddressTypeChangedEvent;

    #endregion

    #region Boolean values to determine which fields appear 

    [DefaultValue(false)]
    public bool AddressWrapperVisible { get; set; }
    [DefaultValue(false)]
    public bool AddressTypeVisible { get; set; }

    [DefaultValue(false)]
    public bool FloorDepartmetVisible { get; set; }

    [DefaultValue(false)]
    public bool AddressTypeReadOnly { get; set; }

    [DefaultValue(false)]
    public bool OrgNameVisible { get; set; }

	[DefaultValue(false)]
	public bool NameVisible { get; set; }

	[DefaultValue(false)]
    public bool NameSectionVisible { get; set; }

    [DefaultValue(true)]
    public bool FirstNameSectionVisible { get; set; }

    [DefaultValue(false)]
    public bool Cell1Visible { get; set; }

    [DefaultValue(false)]
    public bool Cell2Visible { get; set; }

    [DefaultValue(true)]
    public bool Email1Visible { get; set; }

    [DefaultValue(false)]
    public bool Email2Visible { get; set; }

    [DefaultValue(true)]
    public bool Phone1Visible { get; set; }

    [DefaultValue(true)]
    public bool Phone2Visible { get; set; }

    //[DefaultValue(true)]
   [DefaultValue(false)]
    public bool PhoneExt1Visible { get; set; }

    [DefaultValue(false)]
    public bool PhoneExt2Visible { get; set; }

	[DefaultValue(false)]
	public bool EffectiveAndEndDateVisible { get; set; }

	[DefaultValue(true)]
    public bool Fax1Visible { get; set; }

    [DefaultValue(true)]
    public bool Fax2Visible { get; set; }

    [DefaultValue(false)]
    public bool ContactVisible { get; set; }

    [DefaultValue(false)]
    public bool OfficeMgrVisible { get; set; }
	
    [DefaultValue(false)]
    public bool QuadrantVisible { get; set; }
	
    [DefaultValue(false)]
	public bool VerifyAddress { get; set; }

    [DefaultValue(false)]
    public bool OverrideAddressValidation { get; set; }

    [DefaultValue(0)]
    public int AddressTypeId { get; set; }
    [DefaultValue(0)]
    public int RegId { get; set; }


    public string PageTitle
    {
        get
        {
            return ViewState["PageTitle"] == null ? string.Empty : ViewState["PageTitle"].ToString();
        }
        set
        {
            ViewState["PageTitle"] = value;
        }
    }

    [DefaultValue(true)]
    public bool StateVisible { get { return _stateVisible; } set { _stateVisible = value; } }

    [DefaultValue(true)]
    public bool CountyRequired { get; set; }

    [DefaultValue(true)]
    public bool PhoneNotRequired { get; set; } 

    [DefaultValue(true)]
    public bool ZipExtVisible { get; set; }

	[DefaultValue(true)]
	public bool IsZipExtRequired { get; set; }
	[DefaultValue(true)]

	public bool IsAddressRequired { get; set; }
	[DefaultValue(false)]

	public bool TitleRequired { get; set; }

    [DefaultValue(true)]
    public bool TitleVisible { get; set; }

	[DefaultValue(false)]
	public bool Is1099Page { get; set; }
	[DefaultValue(false)]
	public bool IsCountyHidden { get; set; }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

	#endregion Boolean values to determine which fields appear 

	#region " Properties "

	public string AddressTypeTitle
    {
	    set { lblAddressTypeTitle.Text = value; }
    }

    public string OrgNameLabelName
    {
	    set { lblOrganizationName.Text = value; }
    }

    public string UnitAddress
	{
		get { return txtAddress2.Text; }
		set { txtAddress2.Text = value; }
	}
    public string Address2Title
	{
		set { lblAddress2.Text = value; }
	}


    public string FloorDepartment
    {
        get { return txtFloorDept.Text; }
        set { txtFloorDept.Text = value; }
    }
    public string FloorDepartmentTitle
    {
        set { lblFloorDept.Text = value; }
    }

    public string StreetAddress
	{
		get { return txtAddress1.Text; }
		set { txtAddress1.Text = value; }
	}

	public string Address1Title
	{
		set { lblAddress1.Text = value; }
	}

	public string County
	{
		get { return ddlCounty.SelectedValue; }
		set
		{
			if (!string.IsNullOrEmpty( value)  && ddlCounty.Items.FindByValue(value.ToString().Trim()) != null)
				ddlCounty.SelectedValue = value.Trim();
		}
	}
    public string CountyDisplay
    {
        get
        {
            if (ddlCounty.SelectedItem != null&& !string.IsNullOrEmpty(ddlCounty.SelectedItem.Text))
                return ddlCounty.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlCounty.Items.FindByText(value.ToString().Trim()) != null)
            {
				var selectedcounty = ddlCounty.Items.FindByText(value.ToString().Trim());
				ddlCounty.SelectedValue = selectedcounty.Value;
				ddlCounty.SelectedItem.Text = value.Trim();
			}  
        }
    }

    public string City
	{
		get { return txtCity.Text; }
		set { txtCity.Text = value; }
	}

	public string State
	{
		get { return ddlState.SelectedValue; }
		set
		{
			if (ddlState.Items.FindByValue(value) != null)
			{
				ddlState.SelectedValue = value;
				SelectState(value);
			}
		}
	}

	[DefaultValue(" ")]
    public string Zip5
	{
		get { return nbZipFirst5.Text; }
		set { nbZipFirst5.Text =(value!=null)?value:string.Empty; }
	}

    [DefaultValue(" ")]
    public string Zip4
	{
		get { return nbZipLast4.Text; }
		set { nbZipLast4.Text = (value != null) ? value : string.Empty; }
	}

	[DefaultValue("No")]
	public string BorderStateInd
	{
		get { return hdnBorderStateInd.Value; }
		set { hdnBorderStateInd.Value = (value != null) ? value : string.Empty; }
	}

	public string PhoneNumber1
	{
		get { return txtPhoneNo1.Text; }
		set { txtPhoneNo1.Text = value; }
	}

	public string PhoneExt1
	{
		get { return txtPhoneExt1.Text; }
		set { txtPhoneExt1.Text = value; }
	}

	public string PhoneNumber2
	{
		get { return txtPhoneNo2.Text; }
		set { txtPhoneNo2.Text = value; }
	}

	public string PhoneExt2
	{
		get { return txtPhoneExt2.Text; }
		set { txtPhoneExt2.Text = value; }
	}

	public string FaxNumber1
	{
		get { return txtFaxNo1.Text; }
		set { txtFaxNo1.Text = value; }
	}

	public string FaxNumber2
	{
		get { return txtFaxNo2.Text; }
		set { txtFaxNo2.Text = value; }
	}

	public string CanText1
	{
		get { return rbCell1.SelectedValue; }
		set { rbCell1.SelectedValue = value == "True" ? "1" : "0"; }
	}

	public string CanText2
	{
		get { return rbCell2.SelectedValue; }
		set { rbCell2.SelectedValue = value == "True" ? "1" : "0"; }
	}

	public string Email1
	{
		get { return txtEmail1.Text; }
		set { txtEmail1.Text = value; }
	}

	public string Email2
	{
		get { return txtEmail2.Text; }
		set { txtEmail2.Text = value; }
	}

	public string FirstName
	{
		get { return txtFirst.Text; }
		set { txtFirst.Text = value; }
	}

	public string MiddleName
	{
		get { return txtMiddle.Text; }
		set { txtMiddle.Text = value; }
	}

	public string LastName
	{
		get { return txtLast.Text; }
		set { txtLast.Text = value; }
	}

	public string Title
	{
		get { return txtTitle.Text; }
		set { txtTitle.Text = value; }
	}

	public string Name
    {
		get { return txt1099FormName.Text; }
		set { txt1099FormName.Text = value; }
    }
   

	public string ContactName
	{
		get { return txtContact.Text; }
		set { txtContact.Text = value; }
	}
	private string _ContactType = "P"; // Individual

	public string ContactType
	{
		get { return rblContactType.SelectedValue; }
		set
		{
			if (!string.IsNullOrEmpty(value))
				_ContactType = value;
		}
	}

	public string OrgName
	{
		get { return txtOrgName.Text; }
		set { txtOrgName.Text = value; }
	}

	public string OfficeManager
	{
		get { return txtOfficeMgr.Text; }
		set { txtOfficeMgr.Text = value; }
	}
	public string EffectiveDate
	{
		get { return txtEffectiveDate.Text; }
		set { txtEffectiveDate.Text = value; }
	}
	public string EndDate
	{
		get { return txtEndDate.Text; }
		set { txtEndDate.Text = value; }
	}

	public string Quadrant
	{
		get { return ddlQuadrant.SelectedValue; }
		set
		{
			if (ddlQuadrant.Items.FindByValue(value) != null)
			{
				ddlQuadrant.SelectedValue = value;
			}
		}
	}

	public string Ward
	{
		get { return ddlWard.SelectedValue; }
		set
		{
			if (ddlWard.Items.FindByValue(value) != null)
			{
				ddlWard.SelectedValue = value;
			}
		}
	}

    public string Longitude
	{
		get { return hdnLongitude.Value; }
		set { hdnLongitude.Value = value; }
	}

	public string Latitude
	{
		get { return hdnLatitude.Value; }
		set { hdnLatitude.Value = value; }
	}

	[DefaultValue(false)]
	public bool IsPrimaryPractice { get; set; }

	[DefaultValue(false)]
	public bool IsLiabilityInsurance { get; set; }

	[DefaultValue(false)]
	public bool IsOwnerInfo { get; set; }

	[DefaultValue(false)]
	public bool IsPrimaryContact { get; set; }

	[DefaultValue(" ")]
    public string ValidationGroup
	{
		get { return cvAddress.ValidationGroup; }
		set { setValidationGroup(value); }
	}

	public string SaveButtonClientID
	{
		get { return (hdnSaveButtonClientID.Value); }
		set { hdnSaveButtonClientID.Value = System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(value,true); }
	}

	public bool Confirmed
	{
		get { return (hdnAddressConfirm.Value == "1") ? true : false; }
		set { hdnAddressConfirm.Value = (value) ? "1" : "0"; }
	}

	public bool EnableStreetAddress
	{
		get { return txtAddress1.Enabled; }
		set { txtAddress1.Enabled = value; }
	}

	public bool EnableUnitAddress
	{
		get { return txtAddress2.Enabled; }
		set { txtAddress2.Enabled = value; }
	}
    public bool EnableFloorDepartment
    {
        get { return txtFloorDept.Enabled; }
        set { txtFloorDept.Enabled = value; }
    }

    public bool EnableCounty
	{
		get { return ddlCounty.Enabled; }
		set { ddlCounty.Enabled = value; }
	}

	public bool EnableCity
	{
		get { return txtCity.Enabled; }
		set { txtCity.Enabled = value; }
	}

	public bool EnableState
	{
		get { return ddlState.Enabled; }
		set { ddlState.Enabled = value; }
	}

	public bool EnableZip5
	{
		get { return nbZipFirst5.Enabled; }
		set { nbZipFirst5.Enabled = value; }
	}

	public bool EnableZip4
	{
		get { return nbZipLast4.Enabled; }
		set { nbZipLast4.Enabled = value; }
	}

    public bool EnablePhoneNo1
    {
        get { return txtPhoneNo1.Enabled; }
        set { txtPhoneNo1.Enabled = value; }
    }
    public bool EnablePhoneExt1
    {
        get { return txtPhoneExt1.Enabled; }
        set { txtPhoneExt1.Enabled = value; }
    }
    public bool EnablePhoneNo2
    {
        get { return txtPhoneNo2.Enabled; }
        set { txtPhoneNo2.Enabled = value; }
    }
    public bool EnablePhoneExt2
    {
        get { return txtPhoneExt2.Enabled; }
        set { txtPhoneExt2.Enabled = value; }
    }
    public bool GetGeocode { get; set; }

	public bool VisibleCounty
	{
		get { return trCounty.Visible; }
		set { trCounty.Visible = value; }
	}

    public string SetPhone1Label
    {
        get { return lblPhone1.Text; }
        set { lblPhone1.Text = value; }
    }
	public bool VisibleDivAddressDupllicate
	{
		get { return divAddressDuplicate.Visible; }
		set { divAddressDuplicate.Visible = value; }
	}

    public string PageName
    {
        get
        {
            return ViewState["PageName"] == null ? string.Empty : ViewState["PageName"].ToString();
        }
        set
        {
            ViewState["PageName"] = value;
        }
    }

    //New property, to enable and disable address fields based on indicator
    public bool EnableAddressFields
	{
		get
		{
			if ((txtAddress1.Enabled) && (txtAddress2.Enabled) && (txtFloorDept.Enabled) && (txtCity.Enabled) && (ddlState.Enabled)
			    && (ddlCounty.Enabled) && (nbZipFirst5.Enabled) && (nbZipLast4.Enabled))
				return true;
			else
				return false;
		}
		set
		{
			txtAddress1.Enabled = txtAddress2.Enabled=txtFloorDept.Enabled = txtCity.Enabled =
			ddlState.Enabled = ddlCounty.Enabled = nbZipFirst5.Enabled = nbZipLast4.Enabled = value;
			if (!value)
			{
				txtAddress1.BackColor = txtAddress2.BackColor = txtFloorDept.BackColor = txtCity.BackColor =
				ddlState.BackColor = ddlCounty.BackColor =
				nbZipFirst5.BackColor = nbZipLast4.BackColor = System.Drawing.Color.LightGray;
			}
			else
			{
				txtAddress1.BackColor = txtAddress2.BackColor = txtFloorDept.BackColor = txtCity.BackColor =
				ddlState.BackColor =
				ddlCounty.BackColor = nbZipFirst5.BackColor = nbZipLast4.BackColor = System.Drawing.Color.White;
			}
		}
	}

	#endregion

	#region " Event Handlers "

	protected override void OnLoad(EventArgs e)
	{
		txtAddress1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
		txtAddress2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtFloorDept.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
        txtCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
		ddlState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
		nbZipFirst5.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
		nbZipLast4.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
		hdnAddressConfirm.Value = "0";
		base.OnLoad(e);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		if (Session["Override_prop"] != null)
        {
		 prov_override = Session["Override_prop"].ToString();

		}
		if (!string.IsNullOrWhiteSpace(_ContactType) && (ViewState["hdnContactType"]== null))
		{
			ViewState["hdnContactType"] = _ContactType;
			rblContactType.SelectedValue = _ContactType;
		}
		if (!IsPostBack)
		{
			if (!string.IsNullOrEmpty(this.WorkflowPage.MedicaidID)
			&& ((this.WorkflowPage.MMISProviderTypeID == "86" || this.WorkflowPage.MMISProviderTypeID == "89" || this.WorkflowPage.MMISProviderTypeID == "88"))
			&& (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || (!Helper.IsUserInRole(HttpContext.Current.User.Identity.Name,CON.UserRoleType.LTCCHOP) && !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCInitialReval))))
			{
				EnableStreetAddress =
			EnableUnitAddress =
			EnableFloorDepartment =
			EnableCity =
			EnableState =
			EnableCounty =
			EnableZip5 =
			EnableZip4 = false;
			}
			else
			{
				EnableStreetAddress =
			EnableUnitAddress =
			EnableFloorDepartment =
			EnableCity =
			EnableState =
			EnableCounty =
			EnableZip5 =
			EnableZip4 = true;
			}
			if (ddlState.Items == null || ddlState.Items.Count <= 0)
            {
                Helper.LoadDropDownListWithStates(ref ddlState);
            }
			
			txtAddress1.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
			txtAddress2.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            txtFloorDept.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
            txtCity.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
			ddlState.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
			nbZipFirst5.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
			nbZipLast4.Attributes.Add("onchange", "allowSave(); setAddressConfirm('0');");
			hdnAddressConfirm.Value = "0";
        }

		
		
        //ddlState.SelectedIndexChanged += new EventHandler(ddlState_SelectedIndexChanged);
		ddlCounty.SelectedIndexChanged += new EventHandler(ddlCounty_SelectedIndexChanged);

        if (IsPrimaryPractice)
			rfValidatorZipExtFormat.Enabled = true;

		string addressValidationEnabledString = "0";

		if (this.Visible)
		{
			addressValidationEnabledString = AppSettings.Get("AddressValidationEnabled");
		}

		bool addressValidationEnabled = (addressValidationEnabledString == "1");
		cvAddress.Enabled = addressValidationEnabled;

		btnCancelAddressCorrection.OnClientClick = "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
		                                           + "return false;";

		
		SetVisibleFields();
		
        // OHPNM-13974 - if we are on the billing payment address page, and it's disabled, we need to make the Save button skip validation, so address subnode skips validation too
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.BillingPaymentAddress && this.WorkflowPage.BillingPageIsReadOnly)
        {
            // disable validation for this read only page
            reqvalCounty.Enabled = false;
            reqValidatorOrgName.Enabled = false;
            RequiredFieldValidator2.Enabled = false;
            reqValidatorTitle.Enabled = false;
            reqValidatorFirstName.Enabled = false;
            reqValidatorLastName.Enabled = false;
            rfValidatorCity.Enabled = false;
            rfValidatorState.Enabled = false;
            rfvQuadrant.Enabled = false;
            rfValidatorAddr1.Enabled = false;
            rfvWard.Enabled = false;
            rfValidatorZip.Enabled = false;
            rfValidatorZipExt.Enabled = false;
            rfvPhone1.Enabled = false;
            reqEffDate.Enabled = false;
            rfvEmail1.Enabled = false;
            RequiredFieldValidator1.Enabled = false;
        }

		SetVisibilityByConfig();
    }


	public void SetVisibleFields()
	{
        if (string.IsNullOrEmpty(txtEffectiveDate.Text))
            txtEffectiveDate.Text = DateTime.Today.ToShortDateString();
		divAddressWrapper.Visible = true;
		divAddressType.Visible = AddressTypeVisible;
        divfloorDept.Visible=FloorDepartmetVisible;
		divOrgName.Visible = OrgNameVisible;
        divTitle.Visible = NameSectionVisible;
		Div1099FormName.Visible = NameVisible;
        divFullName.Visible = FirstNameSectionVisible;
		trCell1.Visible = Cell1Visible;
		trCell2.Visible = Cell2Visible;
		trEmail1.Visible = Email1Visible;
		trEmail2.Visible = Email2Visible;
		trPhone1.Visible = Phone1Visible;
		trPhoneExt1.Visible = PhoneExt1Visible;
		trPhone2.Visible = Phone2Visible;
		trPhoneExt2.Visible = PhoneExt2Visible;
		trEffectiveDate.Visible = EffectiveAndEndDateVisible;
		trEndDate.Visible = EffectiveAndEndDateVisible;

		trFax1.Visible = Fax1Visible;
        trFax2.Visible = Fax2Visible;
		trContactName.Visible = ContactVisible;
		trOfficeMgr.Visible = OfficeMgrVisible;
		GetGeocode = true;
        trState.Visible = StateVisible;
		if (AddressTypeId == CON.AddressType.Billing || AddressTypeId == CON.AddressType.PrimaryPractice || AddressTypeId == CON.AddressType.Correspondence)
		{
			lblCountyreq.Text = "County*";
			reqvalCounty.Enabled = true;
		}
		else
		{
			lblCountyreq.Text = "County";
			reqvalCounty.Enabled = false;
		}
		//reqvalCounty.Enabled = CountyRequired;
        rfvPhone1.Visible = !PhoneNotRequired;
        reqValidatorTitle.Visible = TitleRequired;
        divTitle.Visible = TitleVisible;
        lblPhone1.Text = !PhoneNotRequired ? "Phone Number 1*": "Phone Number 1";
        trZip1Ext.Visible = ZipExtVisible;
		rfValidatorZipExt.Enabled = IsZipExtRequired;
        //divQuadWard.Visible = QuadrantVisible;
        if (IsZipExtRequired) { LblExtZip.Text = "Ext Zip*"; }
        else { LblExtZip.Text = "Ext Zip"; }		
		if (IsAddressRequired)
		{
			lblAddress1.Text = "Address 1";
			lblCity.Text = "City";
			lblState.Text = "State";
			lblZip.Text = "Zip";

			rfValidatorZipExt.Enabled = !IsAddressRequired;
			rfValidatorAddr1.Enabled = !IsAddressRequired;
			rfValidatorState.Enabled = !IsAddressRequired;
			rfValidatorCity.Enabled = !IsAddressRequired;
		}		
	}

    public void SetAddressTypeReadOnly()
    {
        rblContactType.Enabled =!AddressTypeReadOnly;
    }
	protected void cvAddress_ServerValidate(object source, ServerValidateEventArgs args)
	{
		if (!VerifyAddress)
			return;
		
		string newStreetAddress = string.Empty;
		string newUnitAddress = string.Empty;
        string floorDept = string.Empty;
        string newAddressLine3 = string.Empty;
		string newCity = string.Empty;
		string newState = string.Empty;
		string newCounty = string.Empty;
		string newCountyNumber = string.Empty;
		string newZip5 = string.Empty;
		string newZip4 = string.Empty;

		string origUnitAddress = UnitAddress.ToUpper().Trim();
		string origStreetAddress = StreetAddress.ToUpper().Trim();
		string origCity = City.ToUpper().Trim();
		string origState = State.ToUpper().Trim();
		string origCounty = CountyDisplay.ToUpper().Trim();
		string origZip5 = Zip5;
		string origZip4 = Zip4;

		int returnCode = 0;
		List<string> errorCodes;

		if (origStreetAddress.ToUpper().Contains("PO B") ||
		    origStreetAddress.ToUpper().Contains("P.O. B") ||
		    origStreetAddress.ToUpper().Contains("P.O B") ||
		    origStreetAddress.ToUpper().Contains("PO. B") ||
		    origStreetAddress.ToUpper().Contains("P O B") ||
		    origStreetAddress.ToUpper().Contains("P O. B") ||
		    origStreetAddress.Trim().ToUpper().StartsWith("BOX"))
		{

			//PO box is not allowed for primary service address
			//if (IsPrimaryPractice)
			//{
			//	args.IsValid = false;
			//	cvAddress.ErrorMessage = "Address Line 1 cannot be a PO Box";
			//	return;
			//}

			if (!(origStreetAddress.ToUpper().Trim().StartsWith("PO B") ||
			      origStreetAddress.ToUpper().Trim().StartsWith("P.O. B") ||
			      origStreetAddress.ToUpper().Trim().StartsWith("P.O B") ||
			      origStreetAddress.ToUpper().Trim().StartsWith("PO. B") ||
			      origStreetAddress.ToUpper().Trim().StartsWith("P O B") ||
			      origStreetAddress.ToUpper().Trim().StartsWith("P O. B")))
			{
				args.IsValid = false;
				cvAddress.ErrorMessage = "If the address is a PO Box, it must start with 'P.O. Box'";
				return;
			}

			args.IsValid = true;
			return;
		}

        if (Phone1Visible)
        {
	        if (Helper.StripNonNumerics(txtPhoneNo1.Text).Length < 10)
	        {
		        cvAddress.ErrorMessage = "* Enter 10 digit phone number 1";
		        args.IsValid = false;
		        return;
	        }
        }

        if (Email1Visible)
        {
	        if (!Helper.IsValidEmail(txtEmail1.Text))
	        {
		        cvAddress.ErrorMessage = "* Enter valid email 1";
		        args.IsValid = false;
		        return;
            }
        }

        if ((IsPrimaryPractice) && (origZip4.ToString() == ""))
		{
			rfValidatorZipExtFormat.Enabled = true;
			cvAddress.ErrorMessage = "* Enter Zip Ext (Last 5 digits)";
			args.IsValid = false;
			return;
		}

		if (!string.IsNullOrEmpty(txtPhoneExt1.Text.Trim()) && txtPhoneExt1.Text.Length > 5)
		{
			cvPhoneExt1.Enabled = true;
			cvAddress.ErrorMessage = "* Phone Extension 1 cannot be more than 5 digits";
			args.IsValid = false;
			return;
		}
		else
		{
			args.IsValid = true;
		}

		if (!string.IsNullOrEmpty(txtPhoneExt2.Text.Trim()) && txtPhoneExt2.Text.Length > 5)
		{
			cvtxtPhoneExt2.Enabled = true;
			cvAddress.ErrorMessage = "* Phone Extension 2 cannot be more than 5 digits";
			args.IsValid = false;
			return;
		}
		else
		{
			args.IsValid = true;
		}
		if (!Helper.IsValidDate(txtEffectiveDate.Text.Trim(), true))
		{
			cvEffectiveDate.Enabled = true;
			cvAddress.ErrorMessage = "A valid Start Date (mm/dd/yyyy) is required.";
			args.IsValid = false;
			return;
		}
		else
		{
			args.IsValid = true;
		}

		if (Convert.ToDateTime(txtEffectiveDate.Text.Trim()) > DateTime.Now.AddYears(1).Date)
		{
			cvEffectiveDate.Enabled = true;
			cvAddress.ErrorMessage = "The effective date (mm/dd/yyyy) cannot be more than 1 year in the future.";
			args.IsValid = false;
			return;
		}


		if (string.IsNullOrEmpty(txtEndDate.Text.Trim()))
		{
			DateTime dtEndDate;
			bool validateEndDate = DateTime.TryParseExact(txtEndDate.Text.Trim(), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEndDate);
			if (!validateEndDate)
				txtEndDate.Text = "12/31/2299";
		}

		if (txtEndDate.Text != "" && trEndDate.Visible == true)
		{
			if (!Helper.IsValidDate(txtEndDate.Text.Trim(), false))
			{
				cvEndDate.Enabled = true;
				cvAddress.ErrorMessage = "End Date (mm/dd/yyyy) is not valid.";
				args.IsValid = false;
				return;
			}
			else
			{
				args.IsValid = true;
			}
			if (Convert.ToDateTime(txtEndDate.Text) <= Convert.ToDateTime(txtEffectiveDate.Text))
			{
				cvEndDate.Enabled = true;
				cvAddress.ErrorMessage = "End date cannot be prior to effective date.";
				args.IsValid = false;
				return;
			}
			else
			{
				args.IsValid = true;
			}
		}

		if (!OverrideAddressValidation) //&& hdnAddressConfirm.Value != "1")
		{
			AddressVerificatonDetail detail;
			IntelligentSearchAgent intSearch = new IntelligentSearchAgent();
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			try
			{

				var addressVerificationRequest = new AddressVerificationRequest()
				{
					AddressLine = origStreetAddress + ", ",
					AddressLine2 = origUnitAddress,

					City = origCity,
					State = origState,
					PostalCode = origZip5 + "-" + origZip4
				};


                detail = intSearch.GetAddressVerificaton(addressVerificationRequest, AddressTypeId, Helper.GetUserId(HttpContext.Current.User.Identity.Name), PageTitle, RegId);
				if (detail != null)
				{
					Longitude = Convert.ToString(detail.Longitude);
					Latitude = Convert.ToString(detail.Latitude);
				}

				errorCodes = ErrorCodeStringToList(detail.ErrorCodes);

				if (!int.TryParse(detail.ReturnCodes, out returnCode))
				{
					throw new Exception(string.Format(
						"Return code from address verification web service is invalid. Return code is: '{0}'",
						detail.ReturnCodes));
				}
			}
			catch (Exception ex)
			{
				if (hdnAddressConfirm.Value == "1")
				{
					divWSError.Style["display"] = "none";
					args.IsValid = true;
				}
				else
				{
					HandleWSException(ex);
					args.IsValid = false;
				}

				return;
			}

			// If no error, check for ReturnText. This is a suggestion, but treat like an error
			if (returnCode == 1)
			{
				bool isCorrected = false;

				if (detail.AddressLine2 != "")
					newUnitAddress = detail.AddressLine2;
				if (detail.StreetName.Trim().ToUpper() == "PO BOX")
					newStreetAddress = detail.StreetName + " " + detail.StreetNumber;
				else
				{
					string preDirectional = (detail.PreDirectional == "") ? "" : detail.PreDirectional + " ";
					if (prov_override == "False")
						newStreetAddress = detail.StreetNumber + " " + preDirectional + detail.StreetName + " " +
										   detail.StreetSuffix;
					else
					{
						newStreetAddress = origStreetAddress; //OHPNM-13217 
					}
				}

				newCity = detail.City;
				newState = detail.State;
				newCounty = detail.County;
				newCountyNumber = detail.CountyNumber;

				if (prov_override == "False")
				{
					string[] zipParts = detail.PostalCode.Split("-".ToCharArray());
					if (zipParts.Length == 2)
					{
						newZip5 = zipParts[0];
						newZip4 = zipParts[1];
					}
					else if (zipParts.Length == 1)
					{
						newZip5 = zipParts[0];
						newZip4 = "";
					}
					else
					{
						newZip5 = "";
						newZip4 = "";
					}
				}
				else
				{
					newCity = origCity;
					newState = origState;
					newCounty = origCounty;
					newZip5 = origZip5;
					newZip4 = origZip4;
				}


				if (origUnitAddress.ToUpper().Trim() != newUnitAddress.ToUpper().Trim())
					isCorrected = true;

				if (origStreetAddress.ToUpper() != newStreetAddress.ToUpper())
					isCorrected = true;

				if (origCity.ToUpper() != newCity.ToUpper())
					isCorrected = true;

				if (origState.ToUpper() != newState.ToUpper())
					isCorrected = true;

				if (origCounty.ToUpper().Replace(" COUNTY","") != newCounty.ToUpper())
					isCorrected = true;


				if (origZip5.ToUpper() != newZip5.ToUpper())
					isCorrected = true;

				if (origZip4.ToUpper() != newZip4.ToUpper())
					isCorrected = true;

				if (1 != 0)
					cvAddress.ErrorMessage = "";

				// If the user already confirmed the usps correction or no correction was needed
				if (hdnAddressConfirm.Value == "1" || isCorrected == false)
				{
					divConfirmAddress.Style["display"] = "none";
					args.IsValid = true;
				}
				else // the user has not confirmed changes and a correction was needed. Show the changes in a jquery-ui dialog.
				{

					// SP call for filtering County name w.r.t county number and state
					List<SqlParameter> parameters = new List<SqlParameter>();
					parameters = new List<SqlParameter>();
					parameters.Add(SqlParms.CreateParameter("StateAbbreviation", DbType.String, newState, false));
					parameters.Add(SqlParms.CreateParameter("CountyNumber", DbType.String, newCountyNumber, false));
					// This below county name will be passed as params for sendData JS so that contains will filter exact match of county name from dropdown ddlcounty.. 
					string countyName = DataAccess.ExecuteScalar("usp_SelectCOUNTY_ByStateAbbreviation_AND_CountyNumber", parameters);

					this.paraUSPSAddress.InnerHtml = "";

					//this.paraUSPSAddress.InnerHtml += "<br>";
					if (newStreetAddress != "")
						this.paraUSPSAddress.InnerHtml += newStreetAddress + "<br>";
					if (UnitAddress != "")
						this.paraUSPSAddress.InnerHtml += newUnitAddress + "<br>";
					if (newCounty != "")
						this.paraUSPSAddress.InnerHtml += newCounty + "<br>";
					this.paraUSPSAddress.InnerHtml += newCity + ", "
															  + newState + " "
															  + newZip5 + "-"
															  + newZip4 + "<br>";
					//OHPNM-13217
					if (prov_override == "billingPaymentAddress" || prov_override == "CorrespondenceAddress" || prov_override == "homeOfficeAddress" ||
						 prov_override == "hospitalAddress" || prov_override == "NursingFacilityAddress" || prov_override == "primaryContactAddress" ||
						  prov_override == "primaryServiceAddress" || prov_override == "otherServiceLocations" || prov_override == "Form1099Address"
						)
					{
						args.IsValid = true; Session["Override_prop"] = "False";
						ScriptManager.RegisterStartupScript(Page, this.GetType(), "sendData",
							"sendData('" + newStreetAddress + "', '"
							 + newUnitAddress + "', '"
							 + floorDept + "', '"
							 + newAddressLine3 + "', '"
							 + newCity + "', '"
							 + newState + "', '"
							 + countyName + "', '"
							 + newZip5 + "', '"
							 + newZip4 + "'); "
							, true);
					}
					else
					{
						ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm",
						"$(function() {var $divConfirm = $( \"#" + divConfirmAddress.ClientID + "\" );"
						+ "setTimeout(function() {"
						+ "$divConfirm.dialog({ modal: true }); "
						+ "$divConfirm.css(\"display\", \"block\"); "
						+ "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
						+ "}, 100); "
						+ "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);
						args.IsValid = false;

						btnConfirmAddress.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
														  + "$(\"#" + divConfirmAddress.ClientID + "\").dialog(\"close\"); "
														  + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
														  + "sendData('" + newStreetAddress + "', '"
														  + newUnitAddress + "', '"
														  + floorDept + "', '"
														  + newAddressLine3 + "', '"
														  + newCity + "', '"
														  + newState + "', '"
														  + countyName + "', '"
														  + newZip5 + "', '"
														  + newZip4 + "'); "
														  + "return false;";
					}
				}
			}
			else if (returnCode > 1 || (returnCode < 0 && errorCodes.Contains("11")))
			{
				divConfirmAddress.Style["display"] = "none";
				cvAddress.ErrorMessage =
					string.Format(
						"Multiple possible addresses, but no exact match made. Number of possible addresses is {0}",
						Math.Abs(returnCode));
				args.IsValid = false;
				if (prov_override != "False") { args.IsValid = true; Session["Override_prop"] = "False"; }
			}
			else if (returnCode == -99 || (returnCode == -1 && errorCodes.Contains("07")))
			{
				divConfirmAddress.Style["display"] = "none";
				cvAddress.ErrorMessage =
					string.Format(
						"Address validation failed. Could not find a valid destination for a mailing or package.");
				args.IsValid = false;
				if (prov_override != "False") { args.IsValid = true; Session["Override_prop"] = "False"; }
			}
			else if (returnCode == -3 && errorCodes.Contains("05"))
			{
				divConfirmAddress.Style["display"] = "none";
				cvAddress.ErrorMessage = string.Format("Street name normalized, but no matching address was found.");
				args.IsValid = false;
				if (prov_override != "False") { args.IsValid = true; Session["Override_prop"] = "False"; }
			}
			else
			{
				divConfirmAddress.Style["display"] = "none";
				cvAddress.ErrorMessage = "Address not found. Details:";
				cvAddress.ErrorMessage += ErrorCodesToHTML(errorCodes);
				args.IsValid = false;
				if (prov_override != "False") { args.IsValid = true; Session["Override_prop"] = "False"; }
			}
		}
	}

    private void SetVisibilityByConfig()
    {
		string pageName = this.PageName;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>
    {
        { "reg_id", this.WorkflowPage.RegistrationId.ToString() },
        { "pagename", pageName }
    };
	
        DataSet dsSPTyp = psc.SelectUIControlVisibility(parms);
		if (dsSPTyp != null && dsSPTyp.Tables.Count > 0)
		{
			foreach (DataRow row in dsSPTyp.Tables[0].Rows)
			{
				string controlid = row["controlid"].ToString();
				bool isvisible = Convert.ToBoolean(row["isvisible"].ToString());

				if (controlid == "Title" && TitleVisible)
				{
					divTitle.Visible = isvisible;
					txtTitle.Visible = isvisible;
					Label5.Visible = isvisible;
				}

				if (controlid == "First Name" && FirstNameSectionVisible)
				{
					divFullName.Visible = isvisible;
				}

				if (controlid == "Middle Name" && FirstNameSectionVisible)
				{
					divFullName.Visible = isvisible;
				}

				if (controlid == "Last Name" && FirstNameSectionVisible)
				{
					divFullName.Visible = isvisible;
				}

				if (controlid == "Address 1")
				{
                    divAddressWrapper.Visible = isvisible;
					trAddress1.Visible = isvisible;
                }

				if (controlid == "Address 2") 
				{ 
                    divAddressWrapper.Visible = isvisible;
                    trAddress2.Visible = isvisible;
                }

				if (controlid == "Suite/Dept/Floor" && FloorDepartmetVisible)
				{
					divfloorDept.Visible = isvisible;
                }
				if (controlid == "City") 
				{
                    trCity.Visible = isvisible;

                }
				if (controlid == "State" && StateVisible) 
				{ 
					trState.Visible = isvisible;
				}

				if (controlid == "Quadrant" && QuadrantVisible) 
				{ 
					divQuadWard.Visible = isvisible;
				}

				if (controlid == "Ward" && QuadrantVisible) 
				{
                    divQuadWard.Visible = isvisible;
                }

				if (controlid == "County" && VisibleCounty) 
				{ 
					trCounty.Visible = isvisible;
                }

				if (controlid == "Zip") 
				{
                    trZip1.Visible = isvisible;
                }
				if (controlid == "Ext Zip" && ZipExtVisible) 
				{ 
					trZip1Ext.Visible = isvisible;
				}
				if (controlid == "Phone Number 1" && Phone1Visible) 
				{ 
					trPhone1.Visible = isvisible;
				}
				if (controlid == "Phone Ext 1" && PhoneExt1Visible)
				{
					trPhoneExt1.Visible = isvisible;
				}

				if (controlid == "Phone Number 2" && Phone2Visible) 
				{
                    trPhone2.Visible = isvisible;
                }

				if (controlid == "Phone Ext 2" && PhoneExt2Visible) 
				{
                    trPhoneExt2.Visible = isvisible;
                }

				if (controlid == "Effective Date" && EffectiveAndEndDateVisible ) 
				{
                    trEffectiveDate.Visible = isvisible;                   
                }

				if (controlid == "End Date" && EffectiveAndEndDateVisible) 
				{ 
					trEndDate.Visible = isvisible; 
				}
				if (controlid == "Fax number 1" && Fax1Visible)
				{
					trFax1.Visible = isvisible;
				}
				if (controlid == "Fax number 2" && Fax2Visible)
				{
					trFax2.Visible = isvisible;
				}
				if (controlid == "Contact Name" && ContactVisible) 
				{
                    trContactName.Visible = isvisible;
                }
				if (controlid == "Email Address 1" && Email1Visible)
				{
					trEmail1.Visible = isvisible;
				}
				if (controlid == "Email 2" && Email2Visible)
				{
					trEmail1.Visible = isvisible;
				}

				if (controlid == "Office Manager" && OfficeMgrVisible) 
				{
					trOfficeMgr.Visible = isvisible;
				}

			}
		}
    }

    protected void rblContactType_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (RegId == 0)
			RegId = SessionVarRetriever.WorkFlowRegID;
		RadioButtonList btn = (RadioButtonList)sender;
		if (btn.Text == CON.ContactType.Individual) 
		{
            HttpContext.Current.Session["rblContactTypeVal"] = CON.ContactType.Individual;
            divName.Visible = true;
			NameSectionVisible = true; 
             FirstNameSectionVisible = true;
            divOrgName.Visible = false;
			OrgNameVisible = false;
			if(Is1099Page)
				NameVisible = true;
		}
		else
		{
            HttpContext.Current.Session["rblContactTypeVal"] = CON.ContactType.Organization;
            divName.Visible = false;
			NameSectionVisible = false;
            FirstNameSectionVisible = false;
            divOrgName.Visible = true;
			OrgNameVisible = true;
			NameVisible = false;
		}


		//DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
		DataSet dsProvider = svc.SelectRegistrationData(RegId, "PROVIDER");
		DataRow drProvider = dsProvider.Tables[0].Rows[0];
		string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);

		if (ContactType  == CON.ContactType.Individual ) 
		{
			if (providerType == CON.ProviderCategoryTypeID.Individual.ToString()) //Individual provider choosing individual address type
			{
				NameSectionVisible = true;
				OrgNameVisible = false;
				if (Is1099Page)
				{
					NameVisible = true;
					FirstNameSectionVisible = false;
				}
				else
					FirstNameSectionVisible = true;
            }
			else //organization provider trying to enter Individual details, so don't prepopulate it
			{
				NameSectionVisible = true;
				OrgNameVisible = false;
                if (Is1099Page)
                {
					NameVisible = true;
					FirstNameSectionVisible = false;
				}
				else
					FirstNameSectionVisible = true;
                StreetAddress = string.Empty;
                UnitAddress = string.Empty;
                City = string.Empty;
                State = string.Empty;
                Zip5 = string.Empty;
                Zip4 = string.Empty;
            }
		}

		if (ContactType  == CON.ContactType.Organization )
		{
			if (providerType != CON.ProviderCategoryTypeID.Individual.ToString()) //organization provider trying to enter organization, keep it prepoulated
			{
				NameSectionVisible = false;
				OrgNameVisible = true;
				NameVisible = false;
				if (Is1099Page)
					TitleVisible = true;
			}
			else//individual provider trying to enter organization, show org name as blank
			{
				NameSectionVisible = false;
				OrgNameVisible = true;
				NameVisible = false;
				if (Is1099Page)
					TitleVisible = true;
				ContactName = string.Empty;
			}
		}
        SetVisibleFields();

    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
	{
		SelectState(ddlState.SelectedValue);
		if (StateChangedEvent != null)
			this.StateChangedEvent(sender, e);
        if (IsCountyHidden)
        {
			trCounty.Visible = false;
		}
		ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "validate", "checkphonenumber()", true);
		//Page.ClientScript.RegisterStartupScript(this.GetType(), "validate", "checkphonenumber()", true);

	}

    protected void ddlCounty_SelectedIndexChanged(object sender, EventArgs e)
    {
	    County = ddlCounty.SelectedValue;

	    if (CountyChangedEvent != null)
		    CountyChangedEvent(sender, e);
    }


    protected void HandleWSException(Exception ex)
	{
		this.paraWSError.InnerHtml = ex.Message;
		if (ex.InnerException != null)
		{
			this.paraWSError.InnerHtml += "<br /><br />Details: " + ex.InnerException.Message;
		}

		ScriptManager.RegisterStartupScript(Page, this.GetType(), "showConfirm",
			"$(function() {var $divConfirm = $( \"#" + divWSError.ClientID + "\" );"
			+ "setTimeout(function() {"
			+ "$divConfirm.dialog({ modal: true, position: { my: \"left center\", at: \"right center\", of: \"#AddressTable\" } }); "
			+ "$divConfirm.css(\"display\", \"block\"); "
			+ "$divConfirm.dialog(\"widget\").css(\"z-index\", (maxZIndex() + 10)); "
			+ "}, 100); "
			+ "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", true); }); ", true);

		btnConfirmWSError.OnClientClick = "$(\"#" + hdnAddressConfirm.ClientID + "\").val(\"1\"); "
		                                  + "$(\"#" + divWSError.ClientID + "\").dialog(\"close\"); "
		                                  + "$(\"#" + SaveButtonClientID + "\").prop(\"disabled\", false); "
		                                  + "return false;";

	}

    #endregion

    #region " Methods "   

    protected XElement CreateElement(string name, object value, string attribName = "", string attribValue = "")
	{
		XElement rtn;

		rtn = new XElement(name, value);

		if (attribName != "")
			rtn.SetAttributeValue(attribName, attribValue);

		return rtn;
	}

	public void LoadState()
	{
        if (string.IsNullOrEmpty(State))
        {
			Helper.LoadDropDownListWithStates(ref ddlState, true);

			//ddlState.SelectedValue = AppSettings.Get("StateCode");

            SelectState(ddlState.SelectedValue);
        }

    }

	public void SelectState(string stateID)
	{
		if (stateID == CON.BorderStatesForOhio.WestVirginia || stateID == CON.BorderStatesForOhio.Kentucky || stateID == CON.BorderStatesForOhio.Indiana || stateID == CON.BorderStatesForOhio.Michigan || stateID == CON.BorderStatesForOhio.Pennsylvania)
		{
			hdnBorderStateInd.Value = "Yes";
		}
		else
		{
			hdnBorderStateInd.Value = "No";
		}
		if (stateID == "DC")
		{
			ddlCounty.Enabled = false;
			reqvalCounty.Enabled = false;
			ddlCounty.SelectedValue = "";
            divQuadWard.Visible = true;
            ddlCounty.Visible = false;
            trCounty.Visible = false;
            //QuadrantVisible = true;
            //SetVisibleFields();
            return;
		}
        if (stateID != "DC")
        {
			if (EnableAddressFields == true)
			{
				// address fields are enabled, so enable county (otherwise, don't enable county since all of address fields are disabled)
				ddlCounty.Enabled = true;
				reqvalCounty.Enabled = true && CountyRequired;
			}
            //ddlCounty.SelectedValue = "";
            divQuadWard.Visible = false;
            ddlCounty.Visible = true;
            trCounty.Visible = true;
            this.LoadCountiesByState(stateID);
            return;
        }
        if (stateID == AppSettings.Get("StateCode"))
		{
			if (EnableAddressFields == true)
			{
				// address fields are enabled, so enable county (otherwise, don't enable county since all of address fields are disabled)
				ddlCounty.Enabled = true;
				reqvalCounty.Enabled = false;
			}
			ddlCounty.SelectedValue = "";
		}
		else
		{
			if (EnableAddressFields == true)
			{
				// address fields are enabled, so enable county (otherwise, don't enable county since all of address fields are disabled)
				ddlCounty.Enabled = true;
				if (AddressTypeId == CON.AddressType.Billing || AddressTypeId == CON.AddressType.PrimaryPractice || AddressTypeId == CON.AddressType.Correspondence)
				{
					reqvalCounty.Enabled = true;
				}
				else reqvalCounty.Enabled = false;
				//reqvalCounty.Enabled = true;
			}
		}
		
		

			this.LoadCountiesByState(stateID);
        //QuadrantVisible = false;
        //AddressWrapperVisible = !string.IsNullOrEmpty(AddressType);
        //SetVisibleFields();
    }

	public void LoadCountiesByState(string stateAbbreviation)
	{
		try
		{
			ddlCounty.Items.Clear();
			DataSet ds = svc.SelectCountiesByStateAbbreviation(stateAbbreviation);
			Helper.LoadDropDown(ddlCounty, ds.Tables[0], "COUNTY_NAME", "MMIS_COUNTY_CODEWithName", true);
		}
		catch(Exception ex) {
            //Seems to be throwing nullreference exception here - do nothing except log error
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(Guid.NewGuid(), logMsg);
			log.CreateLogEntry(ex.ToString());
        }
	}

    protected bool ValidateCounty(out string errorMessage)
	{
		bool rtn = true;
		errorMessage = "";

		if (ddlState.SelectedValue == "DC")
			return true;

		if (ddlState.SelectedValue != "")
		{
			if (ddlCounty.SelectedValue == "")
			{
				errorMessage = "* County is required when state is entered";
				rtn = false;
			}
		}

		return rtn;
	}

	private void setValidationGroup(string groupName)
	{
		rfValidatorAddr1.ValidationGroup = groupName;
		rfValidatorCity.ValidationGroup = groupName;
		rfValidatorState.ValidationGroup = groupName;
		reqvalCounty.ValidationGroup = groupName;
		rfvQuadrant.ValidationGroup = groupName;
		rfvWard.ValidationGroup = groupName;
		rfValidatorZip.ValidationGroup = groupName;
		rfValidatorZipFormat.ValidationGroup = groupName;
		cvAddress.ValidationGroup = groupName;
		rfvPhone1.ValidationGroup = groupName;
        rfvEmail1.ValidationGroup = groupName;
        reqValidatorTitle.ValidationGroup = groupName;
        reqValidatorFirstName.ValidationGroup = groupName;
        reqValidatorLastName.ValidationGroup = groupName;
        reqValidatorOrgName.ValidationGroup = groupName;
		RequiredFieldValidator2.ValidationGroup = groupName;
		reqEffDate.ValidationGroup = groupName;
	}

	private List<string> ErrorCodeStringToList(string errorCodesString)
	{
		List<string> errorCodesList = new List<string>();
		string errorCode = "";
		char[] errorCodeCharArray = errorCodesString.ToCharArray();

		foreach (char c in errorCodeCharArray)
		{
			errorCode += c;

			if (errorCode.Length == 2)
			{
				errorCodesList.Add(errorCode);
				errorCode = string.Empty;
			}
		}

		return errorCodesList;
	}

	private string ErrorCodesToHTML(List<string> errorCodes)
	{
		string html = "<ul>";
		string errorString = string.Empty;

		ResourceManager rm = Resources.AddressValidation.ResourceManager;

		foreach (string errorCode in errorCodes)
		{
			errorString = "ADDR_ERROR_CODE_" + errorCode;
			html += "<li>" + errorCode.ToString() + ": " + rm.GetString(errorString) + "</li>";
		}

		html += "</ul>";

		return html;
	}

    protected void cvPhoneExt1_ServerValidate(object source, ServerValidateEventArgs args)
    {
		if (!string.IsNullOrEmpty(txtPhoneExt1.Text.Trim()) && txtPhoneExt1.Text.Length > 5)
        {
			args.IsValid = false;
        }
        else
        {
			args.IsValid = true;
		}
    }

    protected void cvtxtPhoneExt2_ServerValidate(object source, ServerValidateEventArgs args)
    {
		if (!string.IsNullOrEmpty(txtPhoneExt2.Text.Trim()) && txtPhoneExt2.Text.Length > 5)
		{
			args.IsValid = false;
		}
		else
		{
			args.IsValid = true;
		}
	}
    #endregion
}
