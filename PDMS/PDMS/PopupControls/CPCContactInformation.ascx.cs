using MAXIMUS.Core.Libraries;
using System;
using System.Net;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using Amazon.Runtime.Internal.Transform;

public partial class PopupControls_CPCContactInformation : BaseSectionControl
{
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "DateOfAction"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    #region Private variables
    private int _addressTypeId = CON.AddressType.CPCAddressType;
    private int _addressRegId = -1;
    private Address _address = new Address();

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
		LoadControlData();
        btnSave.Visible = Helper.IsUpdateCPCContact && this.WorkflowPage.CurrentTaskName == "" && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);

        //SAM818 CMC Contact Info page is read only if not in CMC wf
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }

        if(this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.CMC && this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.CPC)
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }
	private void SetVisibleFieldsForUpdate()
	{
		txtPrimaryContactName.Enabled = true;
		txtTitle.Enabled = true;
		txtPhoneNo1.Enabled = true;
		txtPhoneExt1.Enabled = true;
		txtEmail1.Enabled = true;
		//btnSave.Visible = true;
	}

	private void SetVisibleFields(bool isEnabled)
	{
		txtPrimaryContactName.Enabled = isEnabled;
		txtTitle.Enabled = isEnabled;
		txtPhoneNo1.Enabled = isEnabled;
		txtPhoneExt1.Enabled = isEnabled;
		txtEmail1.Enabled = isEnabled;
		//btnSave.Visible = false;
	}

    public override bool HasInputValue()
    {
        bool rtn = false;
     

        return rtn;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (_address == null)
        {
            _address = new Address();
        }
       
        base.OnLoad(e);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CMCAddressType.ToString());
        else
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CPCAddressType.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Contact_HISTORY", parms);

        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }

    public override void LoadControlData()
    {
        LoadAddressInfo();
    }

    private void LoadAddressInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
        {
            ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.CMCAddressType);
        }
        else
        {
            ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, _addressTypeId);
        }
        DataTable dtAddressInfo = Helper.HasRows(ds) ? ds.Tables["AddressInfo"] : null;
        this.DataList = dtAddressInfo;
        if (Helper.HasRows(this.DataList))
        {
            DataRow row = this.DataList.Rows[0];
            this.LoadData(row);
          
        }
        else
        {
            this.LoadData(null);
        }
    }

    public override void LoadData(DataRow addressRow)
    {
        bool isEdit = addressRow != null;
        bool regIsPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        if (addressRow != null)
        {
            if (addressRow["REG_ADDRESS_ID"].ToString() != null)
            {
                txtPrimaryContactName.Text = Helper.GetString("CONTACT_NAME", addressRow).Trim();
                txtTitle.Text = Helper.GetString("TITLE", addressRow).Trim();
                txtPhoneNo1.Text = Helper.GetString("PHONE1", addressRow).Trim();
                txtPhoneExt1.Text = Helper.GetString("PHONE1_EXT", addressRow).Trim();
                txtEmail1.Text = Helper.GetString("EMAIL1", addressRow).Trim();
                hidID.Text = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
                hidIsEdit.Text = isEdit.ToString();
                _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);
                if (Helper.GetString("CAN_TEXT1", addressRow).ToLower() == "true")
                {
                    radioButtonTextSend.SelectedValue = "1";
                }
                else
                {
                    radioButtonTextSend.SelectedValue = "0";
                }              
            }
        }
        if(Helper.IsUpdateCPCContact && this.WorkflowPage.CurrentTaskName == "" && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            SetVisibleFields(true);
        else
            SetVisibleFields(regIsPending);
    }

    public override bool SaveData()
    {
        Page.Validate("CPCContactInformation");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("CPCContactInformation") && !v.IsValid)
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

        SaveFormData();
        return true;
    }
    private void SaveFormData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.PrimaryPractice);
        DataTable dtPrimaryAddressInfo = Helper.HasRows(ds) ? ds.Tables["AddressInfo"] : null;
        DataRow drPrimary = dtPrimaryAddressInfo.Rows[0];
        _address.StreetAddress = Helper.GetString("ADDRESS1", drPrimary);
        _address.UnitAddress = Helper.GetString("ADDRESS2", drPrimary);
        _address.City = Helper.GetString("CITY", drPrimary);
        _address.State = Helper.GetString("STATE", drPrimary);
        _address.Zip5 = Helper.GetString("ZIP", drPrimary);
        _address.Zip4 = Helper.GetString("EXT_ZIP", drPrimary);
        _address.County = Helper.GetString("COUNTY", drPrimary);
        _address.ContactType = Helper.GetString("CONTACT_TYPE",drPrimary);
        _address.OrgName = Helper.GetString("PRACTICE_NAME", drPrimary);
        _address.CountyDisplay = Helper.GetString("CountyName", drPrimary);

        _address.ContactName = txtPrimaryContactName.Text;
        _address.RegId = this.WorkflowPage.RegistrationId;
        _address.Title = txtTitle.Text;
        _address.PhoneNumber1 = txtPhoneNo1.Text;
        _address.PhoneExt1 = txtPhoneExt1.Text;
        _address.Email1 = txtEmail1.Text;
        _address.AddressTypeId = (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC) ? CON.AddressType.CMCAddressType : CON.AddressType.CPCAddressType;
        _address.CanText1 = radioButtonTextSend.SelectedValue.ToString();
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
        if (_address.CanText1 == "1" )
        {

            Subscription subscriptions = new Subscription();
            var firstAndLastName = GetFirstAndLastName();
            subscriptions.first_name = firstAndLastName.Item1;
            subscriptions.last_name = firstAndLastName.Item2;
            subscriptions.mobile = new Mobile();
            subscriptions.mobile.country_code = "1";
            if (_address.PhoneNumber1 != null)
            {
                subscriptions.mobile.number = Methods.StripNonNumerics(_address.PhoneNumber1);
            }
            SMSSubscriptionResult result = new SMSSubscriptionResult();
            result = Methods.SubscriptionAsync(subscriptions).Result;
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("REG_ID", _address.RegId));
            param.Add(new SqlParameter("Request_ID", result.request_id));
            param.Add(new SqlParameter("STATUS_CODE", result.status_code));
            param.Add(new SqlParameter("PHONE1", Methods.StripNonNumerics(_address.PhoneNumber1)));
            Guid updateUser = Methods.GetCurrentUserId();
            if (updateUser == Guid.Empty) updateUser = Guid.Parse(MAXIMUS.Core.Libraries.Constants.appPDMSDataExchangeUserId);
            param.Add(new SqlParameter("UpdateUser", updateUser));
            DataAccess.ExecuteStoredProcedure("sp_insertSUBSCRIPTION", param);
        }
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

    protected void cvValidatePhoneLength(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Helper.StripNonNumerics(txtPhoneNo1.Text).Length == 10 ? true : false;
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveFormData();
        string canMakeWSRequest = AppSettings.Get("MakeWSRequestCallToSI", "false");

        if (Helper.IsUpdateCPCContact && this.WorkflowPage.CurrentTaskName == "" && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && canMakeWSRequest.Equals("true"))
        {
            int transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
            int serviceLocationID = 0;
            string txnResult = string.Empty;

            DataSet ds = _svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
            if (Helper.HasRows(ds))
            {
                DataRow row = ds.Tables[0].Rows[0];
                serviceLocationID = Helper.GetInt("REG_SERVICE_LOCATION_ID", row);
                
                int tqId = TransactionController.InsertTransactionQueue(
                       transactionType,
                       this.WorkflowPage.RegistrationId,
                       serviceLocationID,
                       DateTime.Now,
                       null,
                       null,
                       DateTime.Now,
                       Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(),
                       true);
                svc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.TransactionQueueID, tqId.ToString());

                RegistrationController.PopulateStagingData(tqId, CON.TransactionTypeValues.MITS, string.Empty);
                ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                ProviderManagementReqRes prr = new ProviderManagementReqRes();

                txnResult = prr.providerManagementUpdateRequest(tqId, true);

                TransactionController.UpdateTransactionQueue(tqId, DateTime.Now, null, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());                

            }
        }

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.CancelPopup();
    }

    private void CancelPopup()
    {
        this.mpeHistory.Hide();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpeHistory.Show();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
                parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CMCAddressType.ToString());
            else
                parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CPCAddressType.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Contact_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grdHistoryExport.DataSource = ds.Tables[0];
                grdHistoryExport.DataBind();
                grdHistoryExport.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CMCAddressType.ToString());
        else
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CPCAddressType.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Contact_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC)
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CMCAddressType.ToString());
        else
            parms.Add("ADDRESS_TYPE_ID", CON.AddressType.CPCAddressType.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Contact_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
            mpeHistory.Show();
        }
    }
}