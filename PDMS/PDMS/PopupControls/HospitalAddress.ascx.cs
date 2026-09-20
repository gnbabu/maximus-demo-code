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

public partial class PopupControls_HospitalAddress : BaseSectionControl, IHospitalAddressView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public bool _ExportHistory
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

    #region Private variables
    private List<LocationTypes> _locationList = new List<LocationTypes>();
    private int _locationTypeId = -1;
    private int _addressRegId = -1;
    private HospitalAddressPresenter _presenter;
    #endregion

    public event EventHandler LocationChangedEvent;

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private int _addressTypeId = CON.AddressType.Hospital;

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
    public bool vwIsAlreadyAdded
    {
        get
        {
            return ViewState["vwIsAlreadyAdded"] == null ? false : Convert.ToBoolean(ViewState["vwIsAlreadyAdded"]);
        }
        set
        {
            ViewState["vwIsAlreadyAdded"] = value;
        }
    }
  
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

    public HospitalAddressPresenter Presenter
    {
	    get
	    {
		    if (_presenter == null)
		    {
			    _presenter = new HospitalAddressPresenter(this);
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
        if (Request.Cookies["locationTypeValue"] != null) ddlLocation.SelectedValue = Request.Cookies["locationTypeValue"].Value;
	    ddlLocation.SelectedIndexChanged += new EventHandler(ddlLocation_SelectedIndexChanged);
	    SetVisibleFields();
        loadsession();
    }

    protected void ddlLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
	    hidLocation.Text = ddlLocation.SelectedValue;
	    _locationTypeId = _locationList.FirstOrDefault(x => x.LocationTypeName.Trim().ToUpper() ==  ddlLocation.SelectedValue.Trim().ToUpper()).LocationTypeId;
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
	    ucAddress.Phone2Visible = true;
	    ucAddress.PhoneExt1Visible = true;
	    ucAddress.PhoneExt2Visible = true;
        ucAddress.Phone1Visible = true;
        ucAddress.VerifyAddress
            = true;
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
	    if (Request.Cookies["locationTypeValue"] != null)
		    ddlLocation.SelectedValue = Request.Cookies["locationTypeValue"].Value;
	    SetVisibleFields();

        ucAddress.PageTitle = CON.AddressPage.Hospital;
    }
    public override void LoadControlData()
    {
	    LoadLocationTypes();
        LoadAddressInfo(_addressRegId);

        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("ADDRESS_TYPE_ID", CON.SectionTypeID.HospitalAddress.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_ADDRESS_SECTION_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grdHistory.DataSource = ds.Tables[0];
                grdHistory.DataBind();
                grdHistory.MasterTableView.ExportToExcel();
            }
        }
    }

    private void LoadLocationTypes()
    {
	    ddlLocation.Items.Clear();
	    _locationList.Clear();
	    ddlLocation.Items.Add(" ");
	    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
	    var ds = psc.SelectLocationTypes(CON.SectionTypeID.HospitalAddress);
	    if (Helper.HasRows(ds))
	    {
		    foreach (DataRow row in ds.Tables[0].Rows)
		    {
			    var locType = new LocationTypes
			    {
				    LocationTypeId = (int)row["LOCATION_TYPE_ID"],
				    LocationTypeName = row["LOCATION_TYPE_NAME"].ToString(),
                    AddressTypeID = (int)row["ADDRESS_TYPE_ID"]
                };
			    ddlLocation.Items.Add(locType.LocationTypeName);
			    _locationList.Add(locType);
		    }
	    }
    }
    private void LoadHospitalDetails()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressLocationInfo(this.WorkflowPage.RegistrationId, CON.SectionTypeID.HospitalAddress);
        grdHospitalAddress.DataSource = this.DataList=ds.Tables[0];
        grdHospitalAddress.DataBind();
    }
    

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        HiddenField hdnAddressId = row.FindControl("hdnRegAddressId") as HiddenField;
        int index = Convert.ToInt32(e.CommandArgument);

        divHospitalAddress.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);

        RegAddessId = !string.IsNullOrEmpty(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;

        LoadAddressInfo(RegAddessId);
    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        divHospitalAddress.Visible = true;
        this.LoadData(null);
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
        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.Hospital); // REG_PROVIDER Info

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


    private void LoadAddressInfo(int? addressId = null)
    {
	    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
	    DataSet ds = psc.SelectProviderAddressLocationInfo(this.WorkflowPage.RegistrationId, CON.SectionTypeID.HospitalAddress);
        DataTable dtAddressInfo = Helper.HasRows(ds) ?
                                        ((addressId != null && addressId > 0) ?
                                            (ds.Tables["AddressInfo"]).AsEnumerable()
                                            .Where(row => row.Field<int?>("REG_ADDRESS_ID") == addressId).CopyToDataTable() : ds.Tables["AddressInfo"])
                                   : null;
        //this.DataList = dtAddressInfo;
        if (Helper.HasRows(dtAddressInfo) && addressId != -1)
        {
            //DataRow row = this.DataList.Rows[0];
            this.LoadData(dtAddressInfo.Rows[0]);
            if (Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]) > 0)
                ucAddress.AddressTypeId = Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]);
        }
        else
        {
            this.LoadData(null);
        }
        LoadHospitalDetails();
    }

    public override void LoadData(DataRow addressRow)
    {
	    bool isEdit = false;
	    isEdit = addressRow != null;
	    

	    DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.Hospital);
	    bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
	    //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
	    DataRow drProvider = dsProvider.Tables[0].Rows[0];
        // string providerType = Helper.GetString("ENTITY_TYPE_ID", drProvider);


        if (addressRow != null)
        {
	        _locationTypeId = Helper.GetInt("LOCATION_TYPE_ID", addressRow);
            if (!string.IsNullOrEmpty(Helper.GetString("LOCATION_TYPE_NAME", addressRow)))
            {
                ddlLocation.SelectedValue = Helper.GetString("LOCATION_TYPE_NAME", addressRow);
            }
	        hidLocation.Text = ddlLocation.SelectedValue;
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
        ucAddress.LoadState();
        //Set_hidID(addressRow);
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
        ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
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

    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valHospitalAddress";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override bool SaveData()
    {
        bool isValid = true;
        Page.Validate("valHospitalAddress");

        if (!divHospitalAddress.Visible && grdHospitalAddress.Rows.Count <= 0)
        {
            AddError("Need to add record first before saving", ref isValid);
        }



        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valHospitalAddress") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        if (divHospitalAddress.Visible)
        {
            if (string.IsNullOrEmpty(hidID.Text))
            {
                Set_hidID(null);
            }

            if (_locationTypeId <= 0)
            {
                LocationTypes first = null;
                foreach (var x in _locationList)
                {
                    if (x.LocationTypeName.Trim().ToUpper() == hidLocation.Text.Trim().ToUpper())
                    {
                        first = x;
                        break;
                    }
                }

                _locationTypeId = first.LocationTypeId;
            }
            bool isEdit = RegAddessId > 0 ? true : false;
            _addressRegId  = (int)RegAddessId;
            if (_locationTypeId > 0 && !isEdit)
            {
                vwIsAlreadyAdded = Helper.HasRows(this.DataList) ?
                                              (this.DataList.AsEnumerable()
                                              .Where(row => row.Field<int?>("LOCATION_TYPE_ID") == _locationTypeId).Count() > 0 ? true : false) : false;
                if (vwIsAlreadyAdded)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Record is already added for this location type.";
                    val.ValidationGroup = "valHospitalAddress";
                    this.Page.Validators.Add(val);
                    return false;
                }
            }
            Model.CopyPropertiesFrom(ucAddress);
            Model.RegId = this.WorkflowPage.RegistrationId;
            Model.AddressTypeId = _locationList.FirstOrDefault(x => x.LocationTypeName.Trim().ToUpper() == ddlLocation.SelectedValue.Trim().ToUpper()).AddressTypeID;
            var parms = Model.CreateParameterList(Model);
            parms.Add("LOCATION_TYPE_ID", _locationTypeId.ToString());


            if (isEdit)
            {

                // _addressTypeId = (int)RegAddessId;
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
                _presenter.Update(Model, parms);
            }
            else
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.NoChange.ToString());
                _addressRegId = _presenter.Insert(Model, parms);
            }
            Model.AddressId = _addressRegId;
            LoadAddressInfo(_addressRegId);
            ucAddress.Confirmed = true;
            SetVisibleFields();
        }
        return isValid;
    }

    
    public override bool ValidateData()
    {
	    return true;
    }

   

    private void Set_hidID(DataRow row)
    {
	    if (row == null)
	    {
		    DataSet ds = svc.SelectProviderAddressLocationInfo(this.WorkflowPage.RegistrationId, MAXIMUS.Core.Libraries.Constants.SectionTypeID.HospitalAddress);
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
        get { return "valHospitalAddress"; }
    }

    public override string Title
    {
        get { return "Hospital Address"; }
    }

    public override string IdText
    {
        get { return "ucHospitalAddress_" + this.WorkflowPage.RegistrationId; }
    }

    public class LocationTypes
    {
		public int LocationTypeId { get; set; }
		public string LocationTypeName { get; set; }
        public int AddressTypeID { get; set; }
    }
    //OHPNM-8632
    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (prov_override.Checked)
        {
            Session["Override_prop"] = "hospitalAddress";
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
            Session["Override_prop"] = "hospitalAddress";
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
        ucHospitalAddressHistory.LoadData();
        mpe.Show();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}