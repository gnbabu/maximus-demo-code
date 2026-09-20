using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Address = Models.Data.Address;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Insurance : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private Address _address = new Address();
    private int _addressTypeId = CON.AddressType.Insurance;
    private int _addressRegId = -1;
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

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    private const string sectionName = "Insurance";
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

    public override void LoadData(DataRow dr)
    {

      
        //ucAddress.Address1Title = "Carrier Address";
        // ucAddress.IsLiabilityInsurance = true;
        //ucAddress.LoadState();
        LoadAddressDetails(null);

        int insuranceId = dr != null ? Helper.GetInt("REG_INSURANCE_ID", dr) : 0;
        LoadPlaceHolder(insuranceId, dr != null);
        SetVisibility();

        if (dr != null)
        {
            LoadAddressInfo();

            if (!string.IsNullOrEmpty(Helper.GetString("IsMalpracticeClaimed", dr)))
                rblMalpracticeInsurace.SelectedValue = Helper.GetBool("IsMalpracticeClaimed", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("MalpracticeInsuranceReason", dr)))
                txtMalpracticeReason.Text = Helper.GetString("MalpracticeInsuranceReason", dr);

            if (!string.IsNullOrEmpty(Helper.GetString("IS_UNLIMITED_COVERAGE", dr)))
                ddlIsUnlimitedCoverage.SelectedValue = Helper.GetBool("IS_UNLIMITED_COVERAGE", dr) ? "1" : "0";
            if (!string.IsNullOrEmpty(Helper.GetString("TYPE_OF_COVERAGE_ID", dr)) && Helper.GetInt("TYPE_OF_COVERAGE_ID", dr) > 0)
                ddlTypeofCoverage.SelectedValue = Helper.GetString("TYPE_OF_COVERAGE_ID", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("TAIL_NOSE_COVERAGE", dr)))
                ddlIsPolicyTailCoverageInclude.SelectedValue = Helper.GetBool("TAIL_NOSE_COVERAGE", dr) ? "1" : "0";
            rblMalpracticeInsurace_SelectedIndexChanged(null, null);
            hdnRegInsuranceID.Value = Helper.GetString("REG_INSURANCE_ID", dr);
            hdnPolicyUploaded.Value = Helper.GetString("POLICY_UPLOADED", dr);
            txtPolicyNumber.Text = Helper.GetString("POLICY_NUMBER", dr);

            txtEffectiveDate.Text = Helper.GetDate("EFFECTIVE_DATE", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("ORGINAL_EFFECTIVE_DATE", dr)))
                txtOrginaEffDate.Text = Helper.GetDate("ORGINAL_EFFECTIVE_DATE", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("IS_SELF_INSURED", dr)))
                ddlSelfInsured.SelectedValue = Helper.GetBool("IS_SELF_INSURED", dr) ? "1" : "0";
            txtExpirationDate.Text = Helper.GetDate("EXPIRATION_DATE", dr);
            txtCarrierName.Text = Helper.GetString("CarrierName", dr);
            //txtAgentName.Text = Helper.GetString("AgentName", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("AgentTelephoneNumber", dr)))
                txtPhoneNumber.Text = Helper.FormatPhone(Helper.GetString("AgentTelephoneNumber", dr));
            if (!string.IsNullOrEmpty(Helper.GetString("AgentFaxNumber", dr)))
                txtFaxNumber.Text = Helper.FormatPhone(Helper.GetString("AgentFaxNumber", dr));
            txtemail.Text = Helper.GetString("AgentEmailAddress", dr);
            txtPolicyHolder.Text = Helper.GetString("PolicyHolder", dr);
            txtAmtPerOccurrence.Text = Helper.GetString("CoverageAmountPerOccurance", dr);
            txtAmtPerAggregate.Text = Helper.GetString("CoverageAmountPerAggregate", dr);
            //ddlTypeOfCoverage.SelectedValue = Helper.GetString("TYPE_OF_COVERAGE_ID", dr);
            chkFTCA.Checked = Helper.GetBool("IsFTCA", dr);
        }

        ucAddress.SaveButtonClientID = SaveButtonClientID;
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }
    private void SetVisibleFields()
    {

        ucAddress.AddressTypeVisible = true;
        //ucAddress.Cell1Visible = true;
        //ucAddress.Cell2Visible = true;
        //ucAddress.address1 = false;
        ucAddress.Email1Visible = true;
        ucAddress.Email2Visible = true;
        //ucAddress.Fax2Visible = true;
        //ucAddress.OfficeMgrVisible = true;
        ucAddress.Phone1Visible = true;
        ucAddress.Phone2Visible = true;
        ucAddress.PhoneExt1Visible = true;
        ucAddress.PhoneExt2Visible = true;
        //    ucAddress.GetGeocode = true;
        ucAddress.VerifyAddress = true;
        ucAddress.Address1Title = "Carrier address 1";
        ucAddress.Address2Title = "Carrier address 2";
        
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Insurance)
        {
            SetVisibility();
        }
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddWorkItem.Visible = false;
            }
        }
    }

    private void LoadInsurances()
    {
        LoadTypeofCoverages();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "INSURANCE");
        DataTable dtInsurance = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtInsurance;
        LoadGrid();
        //if (Helper.HasRows(this.DataList))
        //    this.LoadData(this.DataList.Rows[0]);
        //else
        //    this.LoadData(null);

    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        divMalPracticeClaims.Visible = true;
        rblMalpracticeInsurace.SelectedIndex = -1;
        rblMalpracticeInsurace.Enabled = true;
        rblMalpracticeInsurace_SelectedIndexChanged(null, null);
        this.LoadData(null);
    }
    private void LoadGrid()
    {
        grdInsurance.DataSource = this.DataList;
        grdInsurance.DataBind();
        rblMalpracticeInsurace_SelectedIndexChanged(null, null);
    }
    private void LoadTypeofCoverages()
    {
        DataSet ds = svc.SelectReferenceDataWithoutParam("usp_SelectType_OF_Coverage");
        if (Helper.HasRows(ds))
        {
            Helper.LoadList(ddlTypeofCoverage, ds.Tables[0], "TYPE_OF_COVERAGE_NAME", "TYPE_OF_COVERAGE_ID", true);
            ddlTypeofCoverage.SelectedIndex = 0;
        }
    }

    protected void grdInsurance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //Checking the RowType of the Row  
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblIsMalpracticeClaimed = e.Row.FindControl("lblIsMalpracticeClaimed") as Label;
           

            if (!string.IsNullOrEmpty(lblIsMalpracticeClaimed.Text))
            {
                lblIsMalpracticeClaimed.Text = lblIsMalpracticeClaimed.Text.ToUpper() == "TRUE" ? "Yes" : "No";
            }
         
        }

    }
    
    protected void grdInsurance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        HiddenField hdnAddressId = row.FindControl("hdnRegAddressId") as HiddenField;
        
        //ResetFeilds();
        int index = Convert.ToInt32(e.CommandArgument);

        divMalPracticeClaims.Visible = true;
        rblMalpracticeInsurace.Enabled = false;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);

        RegAddessId = !string.IsNullOrEmpty(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;

        LoadAddressInfo(RegAddessId);

    }
    private void Set_hidID(DataRow row)
    {
        if (row == null)
        {
            DataSet ds = svc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, CON.AddressType.Insurance);
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
        bool isEdit = string.IsNullOrEmpty(hdnRegInsuranceID.Value) ? false : true;

        if (rblMalpracticeInsurace.SelectedValue == "0")
        {
            ucAddress.StreetAddress = "";
            ucAddress.UnitAddress = "";

            ucAddress.City = "";
            ucAddress.State = "";
            ucAddress.Zip5 = "";
            ucAddress.Zip4 = "";
            ucAddress.County = "";
        }
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
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Professional Liability Insurance";
    }
    public override void LoadControlData()
    {
        this.LoadInsurances();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_INSURANCE_History", parms);
            if (Helper.HasRows(ds))
            {
                grd.DataSource = ds.Tables[0];
                grd.DataBind();
                grd.MasterTableView.ExportToExcel();
            }
        }
    }

    private bool ValidateExpirationDate()
    {
        bool valid = true;
        if (Convert.ToDateTime(txtEffectiveDate.Text) >= Convert.ToDateTime(txtExpirationDate.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Expiration dates need to be greater than effective date.";
            val.ValidationGroup = "valInsurance";
            this.Page.Validators.Add(val);
            valid = false;
        }

        int result = DateTime.Compare(Convert.ToDateTime(txtExpirationDate.Text), DateTime.Now);

        if (result < 0) // expiration is before today's date
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Expiration dates need to be greater than current Date";
            val.ValidationGroup = "valInsurance";
            this.Page.Validators.Add(val);
            valid = false;
        }


        return valid;
    }
    public override bool SaveData()
    {
        bool rtn = false;
        Page.Validate("valInsurance");

        bool isValid = true;
        foreach (Control ctrl in PlaceholderUploadInsurance.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("valInsurance");
        }

        if (!isValid)
            return isValid;
        if (divProfessionalInsurance.Visible == true)
        {
            if (!ValidateExpirationDate())
                return isValid = false;
        }

        Dictionary<string, string> parms = new Dictionary<string, string>();

        if (Page.IsValid)
        {
            // OHPNM-2229
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && 
               ((grdInsurance.Rows.Count == 0 && !divMalPracticeClaims.Visible) || 
               (divProfessionalInsurance.Visible && rblMalpracticeInsurace.SelectedIndex == -1)) 
               )
            {
                // this page is required, and they either don't have anything in the table (grdInsurance.Rows.Count == 0) and the yes/no question isn't visible (they haven't clicked "Add New") (!divMalPracticeClaims.Visible)
                // OR they haven't answered the yes/no question: (divProfessionalInsurance.Visible && rblMalpracticeInsurace.SelectedIndex == -1)
                // (any other scenario is caught by the validator)
                return false;
            }

            // OHPNM-2419 - if address data is on the screen (rblMalpracticeInsurace=1/divProfessionalInsurance.Visible = true)
            // or they have the "reason" on the screen (rblMalpracticeInsurace=0/divMalpracticeReason.Visible = true)
            // then allow "something" to be saved (for rblMalpracticeInsurace=1 save the "Yes" answer and address data/for rblMalpracticeInsurace=0 update/add the insurance row to represent the "No")
            if (divMalpracticeReason.Visible || divProfessionalInsurance.Visible)
            {
                // We're going to save 'something'
                try
                {
                    SaveAddressDetails();
                    if (_address.AddressId > 0 || divProfessionalInsurance.Visible == false)
                    {
                        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                        parms.Add("POLICY_UPLOADED", "1");                  // Default to "1" as this column has been removed from grid


                        parms.Add("POLICY_NUMBER", divProfessionalInsurance.Visible ? Helper.CleanTextOnlyKeyboardCharacters(txtPolicyNumber.Text) : "");

                        if (!string.IsNullOrEmpty(txtEffectiveDate.Text)) parms.Add("EFFECTIVE_DATE", divProfessionalInsurance.Visible ? txtEffectiveDate.Text : "");
                        if (!string.IsNullOrEmpty(txtOrginaEffDate.Text)) parms.Add("ORGINAL_EFFECTIVE_DATE", divProfessionalInsurance.Visible ? txtOrginaEffDate.Text : "");
                        if (!string.IsNullOrEmpty(ddlSelfInsured.SelectedValue)) parms.Add("IS_SELF_INSURED", divProfessionalInsurance.Visible ? ddlSelfInsured.SelectedValue : "");

                        if (!string.IsNullOrEmpty(txtExpirationDate.Text)) parms.Add("EXPIRATION_DATE", divProfessionalInsurance.Visible ? txtExpirationDate.Text : "");
                        parms.Add("CarrierName", divProfessionalInsurance.Visible ? Helper.CleanTextOnlyKeyboardCharacters(txtCarrierName.Text) : "");
                        //parms.Add("ADDRESS1", ucAddress.StreetAddress);
                        //parms.Add("ADDRESS2", ucAddress.UnitAddress);
                        //parms.Add("ADDRESS3", "");
                        //parms.Add("CITY", ucAddress.City);
                        //parms.Add("STATE", ucAddress.State);
                        //parms.Add("ZIP", ucAddress.Zip5);
                        //parms.Add("ZIPEXT", ucAddress.Zip4);
                        //parms.Add("COUNTY", ucAddress.County);
                        //parms.Add("AgentName", divProfessionalInsurance.Visible ? txtAgentName.Text : "");
                        parms.Add("AgentTelephoneNumber", divProfessionalInsurance.Visible ? Helper.StripNonNumerics(txtPhoneNumber.Text) : "");
                        parms.Add("AgentFaxNumber", divProfessionalInsurance.Visible ? Helper.StripNonNumerics(txtFaxNumber.Text) : "");
                        parms.Add("AgentEmailAddress", divProfessionalInsurance.Visible ? txtemail.Text : "");
                        parms.Add("PolicyHolder", divProfessionalInsurance.Visible ? txtPolicyHolder.Text : "");

                        parms.Add("CoverageAmountPerOccurance", divProfessionalInsurance.Visible ? txtAmtPerOccurrence.Text.Trim('$') : "");
                        parms.Add("CoverageAmountPerAggregate", divProfessionalInsurance.Visible ? txtAmtPerAggregate.Text.Trim('$') : "");
                        parms.Add("TYPE_OF_COVERAGE_ID", divProfessionalInsurance.Visible ? ddlTypeofCoverage.SelectedValue.ToString() : "");
                        parms.Add("IS_UNLIMITED_COVERAGE", divProfessionalInsurance.Visible ? ddlIsUnlimitedCoverage.SelectedValue.ToString() : "");
                        parms.Add("TAIL_NOSE_COVERAGE", divProfessionalInsurance.Visible ? ddlIsPolicyTailCoverageInclude.SelectedValue.ToString() : "");


                        //parms.Add("TYPE_OF_COVERAGE_ID", ddlTypeOfCoverage.SelectedValue);
                        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parms.Add("IsMalpracticeClaimed", rblMalpracticeInsurace != null ? rblMalpracticeInsurace.SelectedValue.ToString() : "");
                        parms.Add("MalpracticeInsuranceReason", (txtMalpracticeReason != null && divProfessionalInsurance.Visible == false) ? txtMalpracticeReason.Text : "");
                        parms.Add("IsFTCA",chkFTCA.Checked ? "True" : "False") ;
                        parms.Add("REG_ADDRESS_ID", _address.AddressId.ToString());
                        int insuranceId = 0;
                        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                        if (!string.IsNullOrEmpty(hdnRegInsuranceID.Value))
                        {
                            // Update
                            parms.Add("REG_INSURANCE_ID", hdnRegInsuranceID.Value);
                            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "INSURANCE", parms);
                            insuranceId = int.Parse(hdnRegInsuranceID.Value);  // we are only getting reg id back from UpdateRegistrationData
                        }
                        else
                        {
                            // Insert
                            insuranceId = psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "INSURANCE", parms);
                        }

                        foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadInsurance.Controls)
                        {
                            psc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, insuranceId);
                        }
                        rtn = true;
                    }

                }
                catch (Exception ex) { throw ex; }
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdInsurance.Rows.Count == 0)
                {
                    // Don't save anything, nothing to save. 
                    //This page is required, but there aren't any items, so don't let user 'save' the data on the screen
                    rtn = false;
                }
                else 
                {   //Nothing to save.
                    rtn = true;
                }
                
            }
        }
        ucAddress.Confirmed = true;
        return rtn;
    }

    private void SetVisibility()
    {
        ucAddress.EnableState = true;
        bool show = Registration.IsDMEProvider(this.WorkflowPage.RegistrationId);
        txtPhoneNumber.Visible = txtFaxNumber.Visible = txtemail.Visible =
            RequiredFieldValidator5.Enabled = RequiredFieldValidator3.Enabled = RequiredFieldValidator7.Enabled =
            lblPhone.Visible = lblFax.Visible = lblemail.Visible = show;
    }
    protected void rblMalpracticeInsurace_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblMalpracticeInsurace.SelectedValue == "1")
        {
            divMalpracticeReason.Visible = false; divProfessionalInsurance.Visible = true;
        }
        else if (rblMalpracticeInsurace.SelectedValue == "0")
        {
            divMalpracticeReason.Visible = true; divProfessionalInsurance.Visible = false;
        }
    }
    protected void ucAddress_StateChangedEvent(object sender, EventArgs e)
    {
        if (KeepPopupOpenEvent != null)
            KeepPopupOpenEvent();

        if (ucAddress.State != "")
        {
            //LoadCountiesByState(ucAddress.State);
        }
    }

    public void LoadPlaceHolder(int insuranceId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadInsurance.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, insuranceId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, insuranceId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadInsurance.Controls.Add(ucUploadSectionControl);
            }
        }

    }

    public override bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtPolicyNumber.Text) || !string.IsNullOrEmpty(txtEffectiveDate.Text) || !string.IsNullOrEmpty(txtOrginaEffDate.Text) || !string.IsNullOrEmpty(ddlSelfInsured.SelectedValue) || !string.IsNullOrEmpty(txtExpirationDate.Text) || !string.IsNullOrEmpty(txtCarrierName.Text) ||
        !string.IsNullOrEmpty(ucAddress.StreetAddress) || !string.IsNullOrEmpty(ucAddress.City) || !string.IsNullOrEmpty(ucAddress.State) || !string.IsNullOrEmpty(ucAddress.Zip5) ||
        /*!string.IsNullOrEmpty(txtAgentName.Text)*/  !string.IsNullOrEmpty(txtPolicyHolder.Text) || !string.IsNullOrEmpty(txtAmtPerOccurrence.Text) || !string.IsNullOrEmpty(txtAmtPerAggregate.Text))
        {

            rtn = true;


        }
        else
        {
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.AddressType.Insurance)
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

    public override bool ValidateData()
    {
        bool isValid = true;

        Page.Validate("valInsurance");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valInsurance") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        return isValid;
    }
    private void LoadAddressDetails(DataRow addressRow)
    {
        bool isEdit = false;
        isEdit = addressRow != null;
        ucAddress.LoadState();

        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId,CON.AddressType.Insurance);
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
    public override string ValidationGroup
    {
        get { return "valInsurance"; }
    }

    public override string Title
    {
        get { return "Professional Liability Insurance"; }
    }

    public override string IdText
    {
        get { return "ucInsurance_" + this.WorkflowPage.RegistrationId; }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        lblTitle.Text = "Insurance History";
        ucInsuranceHistory.LoadControlData();
        mpe.Show();
    }


    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }
}