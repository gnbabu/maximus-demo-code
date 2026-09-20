using MAXIMUS.Core.Libraries;
using Presentation;
using Presentation.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_NursingFacilityAddress : BaseSectionControl, INursingFacilityAddressView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private List<LocationTypes> _locationList = new List<LocationTypes>();
    private int _locationTypeId = -1;
    private int _addressRegId = -1;
    private NursingFacilityAddressPresenter _presenter;
    #endregion

    public event EventHandler LocationChangedEvent;

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private int _addressTypeId = CON.AddressType.NursingFacility;
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
    public int? RegAddessId
    {
        get
        {
            return ViewState["RegAddessId"] == null ? 0 : Convert.ToInt32(ViewState["RegAddessId"]);
        }
        set
        {
            ViewState["RegAddessId"] = value;
        }
    }
    public DataTable dtNursingFacilityAddress
    {
        get
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("SECTION_TYPE_ID", CON.SectionTypeID.NursingFacilityAddress.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESSSECTIONCustom", parms);
            return Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    public NursingFacilityAddressPresenter Presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new NursingFacilityAddressPresenter(this);
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
    public void GetProviderAddressInfo(int regId, int addressTypeId)
    {
        throw new NotImplementedException();
    }

    public string SaveButtonClientID
    {
        get;
        set;
    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (divNursingfacilityAddressDetails.Visible)
        {
            if (!string.IsNullOrEmpty(ucAddress.StreetAddress) || !string.IsNullOrEmpty(ucAddress.UnitAddress) || !string.IsNullOrEmpty(ucAddress.City) ||
                !string.IsNullOrEmpty(ucAddress.State) || !string.IsNullOrEmpty(ucAddress.Zip5) || !string.IsNullOrEmpty(ucAddress.Zip4) ||
                !string.IsNullOrEmpty(ucAddress.County) ||
                !string.IsNullOrEmpty(Helper.StripNonNumerics(ucAddress.PhoneNumber1)) || !string.IsNullOrEmpty(Helper.StripNonNumerics(ucAddress.FaxNumber1)) ||
                !string.IsNullOrEmpty(ucAddress.Email1))
            {
                rtn = true;
            }
        }
        else
        {
            rtn = true;
        }

        return rtn;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "NursingFacilityAddress";
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
                    Session["Override_prop"].ToString() == "billingPaymentAddress")
                {
                    Session["Override_prop"] = "False";
                }
            }
            else { Session["Override_prop"] = "False"; }
        }
        prov_Same.InputAttributes.Add("aria-label", "Same as Practice Location");
        prov_override.InputAttributes.Add("aria-label", "Override Address Validation");

    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        hidLocation.Text = ddlLocation.SelectedValue;
        _locationTypeId = Convert.ToInt32(ddlLocation.SelectedValue);
        if (LocationChangedEvent != null)
            this.LocationChangedEvent(sender, e);
    }

    private void SetVisibleFields()
    {
        ucAddress.AddressTypeVisible = true;
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
        ucAddress.Phone2Visible = true;
        ucAddress.PhoneExt1Visible = true;
        ucAddress.PhoneExt2Visible = true;
        ucAddress.VerifyAddress = true;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (Model == null)
        {
            Model = new Address();
        }

        _presenter = Presenter;
        LoadNursingFacility();
        ucAddress.RegId = WorkflowPage.RegistrationId;
        LoadControlData();
        if (Request.Cookies["locationTypeValue"] != null) ddlLocation.SelectedValue = Request.Cookies["locationTypeValue"].Value;
        ddlLocation.SelectedIndexChanged += new EventHandler(ddlLocation_SelectedIndexChanged);


        if (Request.Cookies["locationTypeValue"] != null)
            ddlLocation.SelectedValue = Request.Cookies["locationTypeValue"].Value;

        SetVisibleFields();
        base.OnLoad(e);

        ucAddress.PageTitle = CON.AddressPage.NursingFacility;
    }
    private void LoadNursingFacility()
    {
        grdNursingFacilityAddress.DataSource = dtNursingFacilityAddress;
        grdNursingFacilityAddress.DataBind();
    }
    public override void LoadControlData()
    {
        LoadLocationTypes();
        this.LoadData(null);
    }

    private void LoadLocationTypes()
    {
        ddlLocation.Items.Clear();
        _locationList.Clear();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        var ds = psc.SelectLocationTypes(CON.SectionTypeID.NursingFacilityAddress);
        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(ddlLocation, ds.Tables[0], "LOCATION_TYPE_NAME", "LOCATION_TYPE_ID", true);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var locType = new LocationTypes
                {
                    LocationTypeId = (int)row["LOCATION_TYPE_ID"],
                    LocationTypeName = row["LOCATION_TYPE_NAME"].ToString(),
                    AddressTypeID = (int)row["ADDRESS_TYPE_ID"]
                };
                _locationList.Add(locType);
            }
        }
    }
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        this.LoadData(null);
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        HiddenField hdnAddressId = row.FindControl("hdnRegAddressId") as HiddenField;
        //        Label hdnAddressId = row.FindControl("hdnRegAddressId") as Label;
        RegAddessId = !string.IsNullOrEmpty(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;
        int index = Convert.ToInt32(e.CommandArgument);
        switch (e.CommandName)
        {
            case "NursingFacilityAddressDelete":
                svc.DeleteRegistrationData("ADDRESS", "REG_ADDRESS_ID", (int)RegAddessId);
                this.LoadNursingFacility();
                break;
            case "NursingFacilityAddressEdit":
                divNursingfacilityAddressDetails.Visible = true;
                if (Helper.HasRows(this.DataList))
                    this.LoadData(this.DataList.Rows[index]);
                LoadAddressInfo(RegAddessId);
                break;
            default:
                break;
        }

    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        RegAddessId = null;

        this.LoadData(null);
        divNursingfacilityAddressDetails.Visible = true;

    }
    private void LoadAddressDetails(DataRow addressRow)
    {
        bool isEdit = false;
        isEdit = addressRow != null;


        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.NursingFacility);
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        // string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);
        if (addressRow != null)
        {
            _locationTypeId = Helper.GetInt("LOCATION_TYPE_ID", addressRow);
            if (_locationTypeId > 0)
            {
                ddlLocation.SelectedValue = Convert.ToString(_locationTypeId);
                hidLocation.Text = ddlLocation.SelectedValue;
            }
        }

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

        hidIsEdit.Text = isEdit.ToString();
        Set_hidID(addressRow);
        ucAddress.LoadState();
        if (addressRow != null)
        {
            Model.Load(addressRow, drProvider);
            ucAddress.CopyPropertiesFrom(Model);

        }
        else
        {
            Model.Load(drProvider);
        }


        ucAddress.IsIndividual = false;
        ucAddress.ContactType = CON.ContactType.Organization;
        ucAddress.AddressTypeReadOnly = true;
        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
        ucAddress.OrgNameVisible = !ucAddress.IsIndividual; //As Nursing Facility is an Organization
        ucAddress.SetAddressTypeReadOnly();
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
        SetVisibleFields();
    }
    private void LoadAddressInfo(int? addressId = null)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressLocationInfo(this.WorkflowPage.RegistrationId, CON.SectionTypeID.NursingFacilityAddress);

        DataTable dtAddressInfo = Helper.HasRows(ds) ?
                                         ((addressId != null && addressId > 0) ?
                                             (ds.Tables["AddressInfo"]).AsEnumerable()
                                             .Where(row => row.Field<int?>("REG_ADDRESS_ID") == addressId).CopyToDataTable() : ds.Tables["AddressInfo"])
                                    : null;
        //this.DataList = dtAddressInfo;
        if (Helper.HasRows(dtAddressInfo))
        {
            //DataRow row = this.DataList.Rows[0];
            this.LoadAddressDetails(dtAddressInfo.Rows[0]);
            if (Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]) > 0)
                ucAddress.AddressTypeId = Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]);
        }
        else
        {
            this.LoadAddressDetails(null);
        }
    }

    public override void LoadData(DataRow addressRow)
    {
        LoadAddressDetails(addressRow);
        if (addressRow == null)
        {
            ddlLocation.SelectedIndex = -1;
            hidID.Text = string.Empty;
            ucAddress.StreetAddress = string.Empty;
            ucAddress.UnitAddress = string.Empty;
            ucAddress.City = string.Empty;
            ucAddress.State = "";
            ucAddress.Zip5 = string.Empty;
            ucAddress.Zip4 = string.Empty;
            ucAddress.PhoneExt1 = string.Empty;
            ucAddress.PhoneExt2 = string.Empty;
            ucAddress.OrgName = string.Empty;
            ucAddress.PhoneNumber1 = string.Empty;
            ucAddress.PhoneNumber2 = string.Empty;
            ucAddress.FaxNumber1 = string.Empty;
            ucAddress.FaxNumber2 = string.Empty;
            ucAddress.Email1 = string.Empty;
            ucAddress.OfficeManager = string.Empty;
            ucAddress.ContactName = string.Empty;

            //prov_Phone.Text = string.Empty;
            //prov_PhoneExt.Text = string.Empty;
            ucAddress.LoadState();

        }


    }

    public override bool SaveData()
    {

        if (divNursingfacilityAddressDetails.Visible)
        {
            Page.Validate("valNursingFacilityAddress");


            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("valNursingFacilityAddress") && !v.IsValid)
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

            if (!string.IsNullOrWhiteSpace(ddlLocation.SelectedValue))
            {
                _locationTypeId = Convert.ToInt32(ddlLocation.SelectedValue);
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
            Model.AddressTypeId = _locationList.FirstOrDefault(x => x.LocationTypeId.ToString() == ddlLocation.SelectedValue).AddressTypeID;
            var parms = Model.CreateParameterList(Model);
            parms.Add("LOCATION_TYPE_ID", _locationTypeId.ToString());
            bool isEdit = RegAddessId > 0 ? true : false;

            if (isEdit)
            {
                if (_addressRegId <= 0 && !string.IsNullOrEmpty(hidID.Text))
                {
                    _addressRegId = Convert.ToInt32(hidID.Text);
                }
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
                _presenter.Update(Model, parms);
            }
            else
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.NoChange.ToString());
                _addressRegId = _presenter.Insert(Model, parms);
            }
            this.LoadData(null);
            ucAddress.Confirmed = true;
            SetVisibleFields();
            return true;
        }
        else
        {
            // OHPNM-9413
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdNursingFacilityAddress.Rows.Count == 0)
            {
                // this page is required, but there aren't any items, so don't let them 'save' the data on the screen
                return false;
            }

            //if no details screen visible then no need to save.
            return true;

        }       

    }

    public override bool ValidateData()
    {
        return true;
    }

    //private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    //{
    //    CustomValidator val = new CustomValidator();
    //    val.IsValid = false;
    //    val.ErrorMessage = errMsg;
    //    val.ValidationGroup = ValidationGroup;
    //    this.Page.Validators.Add(val);
    //    isGood = false;
    //}

    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectProviderAddressLocationInfo(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.SectionTypeID.NursingFacilityAddress);
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
    protected string FormatPhone(object phn)
    {
        string phone = phn is string ? phn.ToString() : string.Empty;
        return Helper.FormatPhone(phone);
    }
    protected string FormatAddress(object add1, object add2, object city, object state, object zip, object ext_zip)
    {

        return Helper.GetFormattedAddress(Helper.ConvertNull(add1).ToString(), Helper.ConvertNull(add2).ToString(), Helper.ConvertNull(city).ToString(), Helper.ConvertNull(state).ToString(), Helper.ConvertNull(zip).ToString(), Helper.ConvertNull(ext_zip).ToString(),string.Empty);
    }
    public override string ValidationGroup
    {
        get { return "valNursingFacilityAddress"; }
    }

    public override string Title
    {//Display Name for Nursing Facility is LOng Term Care
        get { return "Long Term Care Addresses"; }
    }

    public override string IdText
    {
        get { return "ucNursingFacilityAddress_" + this.WorkflowPage.RegistrationId; }
    }

    public class LocationTypes
    {
        public int LocationTypeId { get; set; }
        public string LocationTypeName { get; set; }
        public int AddressTypeID { get; set; }
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
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.TaxForm1099); // REG_PROVIDER Info

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
        }

    }

    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "NursingFacilityAddress";
        }
        else
        {
            Session["Override_prop"] = "False";
        }

    }

}