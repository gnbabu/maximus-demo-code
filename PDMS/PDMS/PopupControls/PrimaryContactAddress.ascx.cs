using MAXIMUS.Core.Libraries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_PrimaryContactAddress : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private int _addressTypeId = CON.AddressType.PrimaryContact;
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

	
    #endregion

    public string SaveButtonClientID
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!Page.IsPostBack)
        LoadControlData();
        SetVisibleFields();
        SetVisibleByConfig();
        loadSession();
        primaryContactAddress_override.InputAttributes.Add("aria-label", "Override Address Validation");
    }
    private void SetVisibleByConfig()
    {
        string pageName = "PrimaryContactInfo";
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>
        {
            { "reg_id", this.WorkflowPage.RegistrationId.ToString() },
            { "pagename", pageName }
        };        

        ucAddress.PageName = pageName;

        DataSet dsSPTyp = psc.SelectUIControlVisibility (parms);
        foreach (DataRow row in dsSPTyp.Tables[0].Rows)
        {
            string controlid = row["controlid"].ToString();
            bool isvisible = Convert.ToBoolean(row["isvisible"].ToString());

            if (controlid == "Name" && isvisible)
            {
                lblName.Visible = true;
                divContactName.Visible = true;
            }
            else
            {
                lblName.Visible = false;
                divContactName.Visible = false;
            }
        }
    }

    private void SetVisibleFields()
    {
	    ucAddress.AddressTypeVisible = false;
        ucAddress.TitleVisible = true;
        ucAddress.OrgNameVisible = false;
        ucAddress.FirstNameSectionVisible = false;
	    ucAddress.Cell1Visible = true;
	    ucAddress.Cell2Visible = true;
        ucAddress.ZipExtVisible = true;
	    ucAddress.ContactVisible = false;
	    ucAddress.Email1Visible = true;
	    ucAddress.Email2Visible = true;
        ucAddress.Fax1Visible = true;
        ucAddress.Fax2Visible = true;
	    ucAddress.OfficeMgrVisible = true;
	    ucAddress.Phone1Visible = true;
        ucAddress.Phone2Visible = true;
	    ucAddress.PhoneExt1Visible = true;
	    ucAddress.PhoneExt2Visible = true;
	    ucAddress.GetGeocode = true;
        ucAddress.VerifyAddress = false; //enabled it in SaveData() method in order to fire the address validation only once per save.
        ucAddress.IsZipExtRequired = false;
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
	    if (_address == null)
	    {
		    _address = new Address();
	    }
		ucAddress.RegId = WorkflowPage.RegistrationId;
        base.OnLoad(e);
        Page.Title = CON.AddressPage.PrimaryAddress;
        ucAddress.PageTitle = CON.AddressPage.PrimaryAddress;
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
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.PrimaryContact.ToString());
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


        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }

        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.PrimaryContact);
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        //DETERMINE IF PROVIDER IS INDIVIDUAL OR ORGANIZATION
        DataRow drProvider = dsProvider.Tables[0].Rows[0];
        
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            ucAddress.EnableStreetAddress =
            ucAddress.EnableUnitAddress =
            ucAddress.EnableCity =
            ucAddress.EnableState =
            ucAddress.EnableZip5 =
            ucAddress.EnableZip4 = true;
        }
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
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

        try
        {
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
                        primaryContactAddress_override.Checked = true;
                    else
                        primaryContactAddress_override.Checked = false;
                }
            }
            else
            {
                _address.Load(drProvider);
            }
        }
        catch (Exception ex) {
            //Seems to be throwing nullreference exception here - do nothing except log error
            string logMsg = String.Format(CON.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(Guid.NewGuid(), logMsg);
            log.CreateLogEntry(ex.ToString());
        }
        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
        ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
        txtPrimaryContactName.Text = ucAddress.ContactName;
        _address.CanText1 = ucAddress.CanText1;
        _address.CanText2 = ucAddress.CanText2;
		

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

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
        SetVisibleFields();
    }

    public override bool SaveData()
    {
        ucAddress.VerifyAddress = true;
        ucAddress.OverrideAddressValidation = primaryContactAddress_override.Checked;
        Page.Validate("PrimaryContactAddress");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("PrimaryContactAddress") && !v.IsValid)
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
        _address.ContactName = txtPrimaryContactName.Text;
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
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", primaryContactAddress_override.Checked ? "1" : "0");
            _address.Update(_address, parms);
        }
        else
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("ADDRESS_VALIDATION_OVERRIDE", primaryContactAddress_override.Checked ? "1" : "0");
            _addressRegId = _address.Insert(_address, parms);
        }
        if (_address.CanText1 == "1" || _address.CanText2 == "1")
        {

            Subscription subscriptions = new Subscription();
            var firstAndLast = GetFirstAndLastName();
            subscriptions.first_name = firstAndLast.Item1;
            subscriptions.last_name = firstAndLast.Item2;
            subscriptions.mobile = new Mobile();
            subscriptions.mobile.country_code = "1";
            if (_address.PhoneNumber1 != null)
            {
                subscriptions.mobile.number = Methods.StripNonNumerics(_address.PhoneNumber1);
            }
            else if (_address.PhoneNumber2 != null)
            {
                subscriptions.mobile.number = Methods.StripNonNumerics(_address.PhoneNumber2);
            }
            SMSSubscriptionResult result = new SMSSubscriptionResult();
            result = Methods.SubscriptionAsync(subscriptions).Result;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("REG_ID", _address.RegId));
            param.Add(new SqlParameter("Request_ID", result.request_id));
            param.Add(new SqlParameter("STATUS_CODE", result.status_code));
            param.Add(new SqlParameter("PHONE1", Methods.StripNonNumerics(_address.PhoneNumber1)));
            param.Add(new SqlParameter("PHONE2", Methods.StripNonNumerics(_address.PhoneNumber2)));
            Guid updateUser = Methods.GetCurrentUserId();
            if (updateUser == Guid.Empty) updateUser = Guid.Parse(MAXIMUS.Core.Libraries.Constants.appPDMSDataExchangeUserId);
            param.Add(new SqlParameter("UpdateUser", updateUser));
            DataAccess.ExecuteStoredProcedure("sp_insertSUBSCRIPTION", param);
        }

        LoadAddressInfo();
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
        get { return "PrimaryContactAddress"; }
    }

    public override string Title
    {
        get { return "Primary Contact Address"; }
    }

    public override string IdText
    {
        get { return "ucPrimaryContactAddress_" + this.WorkflowPage.RegistrationId; }
    }


    public void GetProviderAddressInfo(int regId, int addressTypeId)
    {
	    throw new NotImplementedException();
    }

    public Tuple<string, string> GetFirstAndLastName()
    {
        string firstName = "";
        string lastName = "";

        if (!String.IsNullOrEmpty(_address.ContactName))
        {
            var space = _address.ContactName.Trim().IndexOf(' ');
            if (space > 0)
            {
                firstName = _address.ContactName.Trim().Substring(0, space);
                lastName = _address.ContactName.Trim().Substring(space + 1);
            }
            else
            {
                firstName = _address.ContactName;
                lastName = _address.ContactName;
            }
        }
        else if (!String.IsNullOrEmpty(_address.OrgName))
        {
            var space = _address.OrgName.Trim().IndexOf(' ');
            if (space > 0)
            {
                firstName = _address.OrgName.Trim().Substring(0, space);
                lastName = _address.OrgName.Trim().Substring(space + 1);
            }
            else
            {
                firstName = _address.OrgName;
                lastName = _address.OrgName;
            }
        }
        else
        {
            firstName = _address.FirstName;
            lastName = _address.LastName;
        }

        return new Tuple<string, string>(firstName, lastName);
    }
    protected void prov_Override_CheckedChanged(object sender, EventArgs e)
    {
        if (primaryContactAddress_override.Checked)
        {
            Session["Override_prop"] = "primaryContactAddress";
        }
        else
        {
            Session["Override_prop"] = "False";
        }

    }

    protected void loadSession()
    {

        if (primaryContactAddress_override.Checked)
        {
            Session["Override_prop"] = "primaryContactAddress";
        }
        else
        {
            if (Session["Override_prop"] != null)
            {
                if (Session["Override_prop"].ToString() == "CorrespondenceAddress" ||
                    Session["Override_prop"].ToString() == "primaryServiceAddress" ||
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
        lblTitle.Text = "Primary Contact Address History";
        ucPrimaryContactAddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

}