using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_WorkHistoryDetails : BaseSectionControl
{
    private Address _address = new Address();
    private int _addressTypeId = CON.AddressType.WorkHistory;
    private int _addressRegId = -1;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public string SaveButtonClientID
    {
        get;
        set;
    }
    //public delegate void ReloadPopupEventHandler();
    //public event ReloadPopupEventHandler ReloadPopupEvent;
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


    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Convert.ToBoolean(Session["ViewProviderFile"]))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            btnAddWorkItem.Visible = false;
            imgAddGap.Visible = false;
        }
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }

        CredentialHelper.APIToken APIToken = ApplicationCache.RestAPIAccessToken();
        hdnAccessToken.Value = APIToken.AccessToken;
        hdnPDMSWebAPI.Value = ConfigurationManager.AppSettings["PDMSWebAPI"].ToString();
        hdnWrkHistoryRegId.Value = this.WorkflowPage.RegistrationId.ToString();
        hdnRedirectToNewWorkHistory.Value = Convert.ToString(ConfigurationManager.AppSettings["RedirectToNewWorkHistory"]);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_WorkHistory_History", parms);

        if (ds.Tables.Count > 0)
        {
            Session["dtWorkHistoryResponse"] = ds.Tables[0];
            grdWorkHistory.CurrentPageIndex = 0;
            grdWorkHistory.DataSource = ds.Tables[0];
            grdWorkHistory.DataBind();
        }
    }
    protected override void OnLoad(EventArgs e)
    {

        if (_address == null)
        {
            _address = new Address();
        }
        tbAdditional.Attributes.Add("maxlength", tbAdditional.MaxLength.ToString());
        tbReasonForDepart.Attributes.Add("maxlength", tbReasonForDepart.MaxLength.ToString());

        ucAddress.RegId = WorkflowPage.RegistrationId;
        base.OnLoad(e);
        LoadControlData();
        Page.Title = CON.AddressPage.WorkHistory;
        ucAddress.PageTitle = CON.AddressPage.WorkHistory;
    }
    public override void LoadControlData()
    {
        LoadAddressDetails(null);
        LoadWorkDetails();

    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public override void LoadData(System.Data.DataRow dr = null)
    {
        LoadAddressDetails(null);
        hdnRegWorkHistoryId.Value = string.Empty;
        if (dr != null)
        {
            LoadAddressInfo();
            pnlWorkItem.Visible = true;
            hdnRegWorkHistoryId.Value = Helper.GetData("REG_WORKHISTORY_ID", dr);
            //if (!string.IsNullOrEmpty(Helper.GetData("TITLE", dr)))
            //{
            //    tbTitle.Text = Helper.GetString("TITLE", dr);
            //}


            if (!string.IsNullOrEmpty(Helper.GetData("IS_CURRENT_EMPLOYER", dr)))
            {
                chkIsCurrentEmployer.Checked = Helper.GetBool("IS_CURRENT_EMPLOYER", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("MILTARY_RESERVE", dr)))
            {
                ddlmiltaryreserve.SelectedValue = Helper.GetBool("MILTARY_RESERVE", dr) ? "1" : "0";
            }
            if (!string.IsNullOrEmpty(Helper.GetData("NAME", dr)))
            {
                tbOrgName.Text = Helper.GetString("NAME", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("WORKED_FROM", dr)))
            {
                tbFrom.Text = Helper.GetDate("WORKED_FROM", dr).ToString();
            }
            if (!string.IsNullOrEmpty(Helper.GetData("WORKED_TO", dr)))
            {
                txtEndDate.Text = Helper.GetDate("WORKED_TO", dr).ToString();
            }

            if (!string.IsNullOrEmpty(Helper.GetData("ADDITIONAL_INFO", dr)))
            {
                tbAdditional.Text = Helper.GetString("ADDITIONAL_INFO", dr);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("REASON_FOR_DEPARTURE", dr)))
            {
                tbReasonForDepart.Text = Helper.GetString("REASON_FOR_DEPARTURE", dr);
            }

            lblTo.Text = !chkIsCurrentEmployer.Checked ? "* End Date:" : " End Date:";
        }
        else
        {
            //  pnlWorkItem.Visible = false;
        }
    }

    public void LoadGapData(System.Data.DataRow dr = null)
    {
        try
        {
            hdnRegGapId.Value = string.Empty;
            ////Load Gap
            //if (!string.IsNullOrEmpty(Helper.GetData("HAS_GAP", dr)))
            //{
            //   rblGap.SelectedValue = Helper.GetInt("HAS_GAP", dr).ToString();
            //   pnlGap.Visible = (rblGap.SelectedItem.Value.ToString() == "1") ? true : false;
            //}
            if (dr != null)
            {

                pnlGap.Visible = true;
                hdnRegGapId.Value = Helper.GetData("REG_WORKGAPS_ID", dr);
                if (!string.IsNullOrEmpty(Helper.GetData("GAP_START_DATE", dr)))
                {
                    tbGapStartDate.Text = Helper.GetDate("GAP_START_DATE", dr).ToString();
                }
                if (!string.IsNullOrEmpty(Helper.GetData("GAP_END_DATE", dr)))
                {
                    tbGapEndDate.Text = Helper.GetDate("GAP_END_DATE", dr).ToString();
                }
                if (!string.IsNullOrEmpty(Helper.GetData("GAP_REASON", dr)))
                {
                    tbGapText.Text = Helper.GetString("GAP_REASON", dr);
                }

            }

        }
        catch (Exception ex)
        {
        }
    }
    public override bool SaveData()
    {

        Page.Validate("vgWorkHistoryDetails");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("vgWorkHistoryDetails") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }
        if (pnlWorkItem.Visible)
        {
            if (ValidateData())
            {
                SaveAddressDetails();
                //Insert New record
                try
                {


                    if (_address.AddressId > 0)
                    {
                        Dictionary<string, string> parms = new Dictionary<string, string>();

                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

                        //parms.Add("title", tbTitle.Text.Trim());
                        parms.Add("name", tbOrgName.Text.Trim());
                        if (tbFrom.Text.Trim() == string.Empty)
                            parms.Add("worked_from", null);
                        else
                            parms.Add("worked_from", Convert.ToDateTime(tbFrom.Text.Trim()).ToString());
                        // parms.Add("worked_from", Convert.ToDateTime(tbFrom.Text.Trim()).ToString());
                        if (txtEndDate.Text.Trim() == string.Empty)
                            parms.Add("worked_to", null);
                        else
                            parms.Add("worked_to", Convert.ToDateTime(txtEndDate.Text.Trim()).ToString());


                        parms.Add("additional_info", tbAdditional.Text.Trim());
                        parms.Add("REASON_FOR_DEPARTURE", tbReasonForDepart.Text.Trim());


                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("MILTARY_RESERVE", ddlmiltaryreserve.SelectedValue);
                        parms.Add("IS_CURRENT_EMPLOYER", chkIsCurrentEmployer.Checked ? "1" : "0");
                        parms.Add("REG_ADDRESS_ID", _address.AddressId.ToString());
                        if (!string.IsNullOrEmpty(hdnRegWorkHistoryId.Value))
                        {

                            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                            parms.Add("REG_WORKHISTORY_ID", hdnRegWorkHistoryId.Value.Trim());
                            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "WORKHISTORY", parms);
                        }
                        else
                        {
                            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                            svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "WORKHISTORY", parms);
                        }


                        LoadAddressInfo();
                        ResetFeilds();
                        return true;
                    }


                }
                catch (Exception ex)
                {
                    throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("ucWorkHistoryDetails_SaveData - " + ex.Message));
                }
            }
            return false;
        }
        else
        {
            //if page is required, none of the grid have the data and gap in the work is not being entered then dont let them save the data
            if(this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1)
            {
                if(grdWork.Rows.Count == 0 && gvGaps.Rows.Count == 0 && !pnlGap.Visible)
                {
                        return false;
                }
            }
        }

        //Save Gaps Data if exists.
        SaveWorkGaps();

        return true;
    }
    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Other);
            if (Helper.HasRows(ds))
            {
                hidID.Value = ds.Tables[0].Rows[0]["REG_ADDRESS_ID"].ToString();
            }
        }
        else
        {
            hidID.Value = row["REG_ADDRESS_ID"].ToString();
        }
    }
    private void SaveAddressDetails()
    {
        if (string.IsNullOrEmpty(hidID.Value))
        {
            Set_hidID(null);
        }
        _address.CopyPropertiesFrom(ucAddress);
        _address.RegId = this.WorkflowPage.RegistrationId;
        _address.AddressTypeId = _addressTypeId;

        var parms = _address.CreateParameterList(_address);
        bool isEdit = string.IsNullOrEmpty(hdnRegWorkHistoryId.Value) ? false : true;

        if (isEdit)
        {

            _addressRegId = (int)RegAddessId;
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_ADDRESS_ID", _addressRegId.ToString());
            _address.Update(_address, parms);
        }
        else
        {
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            _addressRegId = _address.Insert(_address, parms);

        }
        _address.AddressId = _addressRegId;

    }
    private void LoadAddressDetails(DataRow addressRow)
    {
        bool isEdit = false;
        isEdit = addressRow != null;
        ucAddress.LoadState();

        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.WorkHistory);
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

        // hdnRegWorkHistoryId.Value = isEdit.ToString();

        if (addressRow != null)
        {
            _address.Load(addressRow, drProvider);
            ucAddress.CopyPropertiesFrom(_address);
        }
        else
        {
            _address.Load(drProvider);
        }

        ucAddress.NameSectionVisible = ucAddress.IsIndividual;
        ucAddress.OrgNameVisible = !ucAddress.IsIndividual;
        ucAddress.Phone2Visible = false;
        _address.CanText1 = ucAddress.CanText1;
        _address.CanText2 = ucAddress.CanText2;


        if (isEdit)
        {

            hidID.Value = Helper.GetInt("REG_ADDRESS_ID", addressRow).ToString();
            _addressRegId = Helper.GetInt("REG_ADDRESS_ID", addressRow);
        }
        else
        {
            hidID.Value = string.Empty;
            _addressRegId = -1;
        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
        SetVisibleFields();
    }
    private void SetVisibleFields()
    {
        // ucAddress.AddressTypeVisible = true;
        //ucAddress.Cell1Visible = true;
        //ucAddress.Cell2Visible = true;
        ucAddress.ContactVisible = true;
        ucAddress.Email1Visible = true;
        ucAddress.Email2Visible = true;
        ucAddress.Fax1Visible = true;
        //ucAddress.OfficeMgrVisible = true;
        ucAddress.Phone1Visible = true;
        ucAddress.Phone2Visible = false;
        ucAddress.PhoneExt1Visible = true;
        ucAddress.PhoneNotRequired = true;
        // ucAddress.PhoneExt2Visible = true;
        //    ucAddress.GetGeocode = true;
        //    ucAddress.VerifyAddress = true;

    }
    private void SaveWorkGaps()
    {
        try
        {
            if (pnlGap.Visible)
            {
                Page.Validate("Gap");
                using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
                {

                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                    //gap reason
                    parms.Add("gap_reason", tbGapText.Text.Trim());
                    if (!string.IsNullOrEmpty(tbGapStartDate.Text))
                    {
                        parms.Add("gap_start", Convert.ToDateTime(tbGapStartDate.Text.Trim()).ToString());
                    }
                    //gap end date
                    if (!string.IsNullOrEmpty(tbGapEndDate.Text))
                    {
                        parms.Add("gap_end", Convert.ToDateTime(tbGapEndDate.Text.Trim()).ToString());
                    }
                    parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    if (!string.IsNullOrEmpty(hdnRegGapId.Value))
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("REG_WORKGAPS_ID", hdnRegGapId.Value.Trim());
                        psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "WORKGAPS", parms);
                    }
                    else
                    {
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                        psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "WORKGAPS", parms);
                    }
                }
            }

        }
        catch (Exception)
        {
        }
    }

    public override bool ValidateData()
    {
        // OHPNM-2417 - if it's the current employer, then the End Date can be blank; otherwise it's required
        if (pnlWorkItem.Visible && !chkIsCurrentEmployer.Checked && string.IsNullOrEmpty(txtEndDate.Text))
        {
            bool isValid = false;
            AddError("*Enter End Date", ref isValid, "vgWorkHistoryDetails");
            return isValid;
        }

        if (!string.IsNullOrEmpty(txtEndDate.Text))
        {
            DateTime enddate;
            bool validDate = DateTime.TryParse(txtEndDate.Text.Trim(), out enddate);
            if (validDate == false)
            {
                bool isValid = false;
                AddError("*Enter Valid End Date", ref isValid, "vgWorkHistoryDetails");
                return isValid;
            }
        }

        if (!string.IsNullOrEmpty(tbFrom.Text))
        {
            DateTime fromDate;
            bool validDate = DateTime.TryParse(tbFrom.Text.Trim(), out fromDate);
            if (validDate == false)
            {
                bool isValid = false;
                AddError("*Enter Valid From Date", ref isValid, "vgWorkHistoryDetails");
                return isValid;
            }
        }
        return true;
    }

    public override string Title
    {
        get { return "Work History Details"; }
    }

    public override string IdText
    {
        get { return "ucWorkHistoryDetails_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "vgWorkHistoryDetails"; }
    }

    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(this.tbOrgName.Text) ||
            !string.IsNullOrEmpty(tbFrom.Text) || !string.IsNullOrEmpty(txtEndDate.Text) ||
             !string.IsNullOrEmpty(tbReasonForDepart.Text) || !string.IsNullOrEmpty(tbAdditional.Text)
           || !string.IsNullOrEmpty(tbGapStartDate.Text) || !string.IsNullOrEmpty(tbGapEndDate.Text)
           || !string.IsNullOrEmpty(tbGapText.Text))
        {
            rtn = true;

        }

        else
        {
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.EmploymentHistory)
                                .Select(d => d.Value.IsRequired).Max();
            if (required == 0)
            {
                rtn = true;
            }
            else
            {
                rtn = false;
            }
        }

        return rtn;
    }

    protected void grdWork_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        HiddenField hdnAddressId = row.FindControl("hdnRegAddressId") as HiddenField;
        //        Label hdnAddressId = row.FindControl("hdnRegAddressId") as Label;

        ResetFeilds();
        int index = Convert.ToInt32(e.CommandArgument);

        pnlWorkHistoryList.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);

        RegAddessId = !string.IsNullOrEmpty(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;

        LoadAddressInfo(RegAddessId);
    }

    private void LoadWorkDetails()
    {
        if (!pnlWorkHistoryList.Visible) return;

        using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_WORKHISTORY", parms);
            if (Helper.HasRows(ds)) grdWork.DataSource = this.DataList = ds.Tables[0];
            else grdWork.DataSource = this.DataList = null;
            grdWork.DataBind();

            btnWorkHistory.Visible = (grdWork.Rows.Count > 0);

            //Load Gaps

            parms.Clear();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet dsGaps = psc.SelectRegistrationDataWithParams("usp_SelectREG_WORKGAPS", parms);

            if (Helper.HasRows(dsGaps))
            {
                gvGaps.DataSource = dsGaps.Tables[0];

            }
            else { gvGaps.DataSource = null; }
            gvGaps.DataBind();


        }

    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        pnlWorkItem.Visible = true;
        this.LoadData(null);
    }
    protected void imgAddGap_Click(object sender, CommandEventArgs e)
    {
        pnlGap.Visible = true;
        this.LoadData(null);
    }
    protected string GetContactDetails(object name, object email, object phone)
    {
        string contact = string.Empty;
        string contactName = string.IsNullOrEmpty(name.ToString()) ? "" : name.ToString();
        string contactEmail = string.IsNullOrEmpty(email.ToString()) ? "" : email.ToString();
        string contactPhone = string.IsNullOrEmpty(phone.ToString()) ? "" : Helper.FormatPhone(phone.ToString());
        contact = Helper.GetFormattedContact(contactName, contactEmail, contactPhone);

        return contact;
    }


    protected void chkIsCurrentEmployer_Changed(object sender, EventArgs e)
    {
        //pnlGap.Visible = (rblGap.SelectedItem.Value.ToString() == "1") ? true : false;
        this.txtEndDate.Enabled = chkIsCurrentEmployer.Checked ? false : true;
        lblTo.Text = !chkIsCurrentEmployer.Checked ? "* End Date:" : " End Date:";
        // RfvToDate.Visible= !chkIsCurrentEmployer.Checked;
    }
    protected void gvGaps_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        pnlGap.Visible = true;
        DataTable dtGaps = gvGaps.DataSource as DataTable;
        DataRow row = dtGaps.NewRow();

        if (Helper.HasRows(dtGaps))
            this.LoadGapData(dtGaps.Rows[index]);
        else
            this.LoadGapData(null);
    }
    private void ResetFeilds()
    {
        txtEndDate.Text = "";



        chkIsCurrentEmployer.Checked = false;

        tbFrom.Text = "";
        tbReasonForDepart.Text = "";
        tbAdditional.Text = "";

    }


    private void LoadAddressInfo(int? addressId = null)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, _addressTypeId);

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


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        grdWorkHistory.MasterTableView.ExportToExcel();
    }
}