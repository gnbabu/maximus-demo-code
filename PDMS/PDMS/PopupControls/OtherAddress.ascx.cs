using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OtherAddress : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private int _addressTypeId = CON.AddressType.Other;
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
    }
    private void SetVisibleFields()
    {
	    ucAddress.AddressTypeVisible = false;
	    ucAddress.Cell1Visible = false;
	    ucAddress.Cell2Visible = false;
	    ucAddress.ContactVisible = false;
	    ucAddress.Email1Visible = true;
	    ucAddress.Email2Visible = false;
	    ucAddress.Fax2Visible = false;
	    ucAddress.OfficeMgrVisible = false;
	    ucAddress.Phone2Visible = true;
	    ucAddress.PhoneExt1Visible = true;
        ucAddress.PhoneExt2Visible = true;
        ucAddress.VerifyAddress = true;
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
	    base.OnLoad(e);

        ucAddress.PageTitle = CON.AddressPage.Other;
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
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.Other.ToString());
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
	    DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Other);
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
		    ucAddress.AddressTypeId = -1;
		    this.LoadData(null);
	    }
    }
    public override void LoadData(DataRow addressRow)
    {
        bool isEdit = false;
        isEdit = addressRow != null;
        ucAddress.LoadState();

        DataSet dsProvider = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
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

        if (addressRow != null)
        {
            _address.Load(addressRow, drProvider);
            //}
            //else
            //{
            // _address.Load(drProvider);
            //}
            ucAddress.CopyPropertiesFrom(_address);
            ucAddress.NameSectionVisible = ucAddress.IsIndividual;
            ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
        }

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
    public override bool SaveData()
    {
        Page.Validate("valOwnerInfo");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valOwnerInfo") && !v.IsValid)
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
	        _address.Update(_address, parms);
        }
        else
        {
	        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
	        _addressRegId = _address.Insert(_address, parms);
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
	public override string ValidationGroup
    {
        // TODO: EDV why Owner info validate??
        get { return "valOwnerInfo"; }
    }
    public override string Title
    {
        get { return "Other Address"; }
    }
    public override string IdText
    {
        get { return "ucOtherAddress_" + this.WorkflowPage.RegistrationId; }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Other Address History";
        ucOtherAddressHistory.LoadData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

}
