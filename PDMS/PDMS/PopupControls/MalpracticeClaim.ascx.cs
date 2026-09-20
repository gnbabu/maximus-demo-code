using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_MalpracticeClaim : BaseSectionControl, IMalpracticeClaimView
{
    private Models.Data.Address _address = new Models.Data.Address();
    private int _addressTypeId = CON.AddressType.WorkHistory;
    private int _addressRegId = -1;
    private DateTime sqlMinDateTime = new DateTime(1753, 1, 1);
    public string SaveButtonClientID
    {
        get;
        set;
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void KeepPopupOpenEventHandler();

    public delegate void ReloadPopupEventHandler();
    public bool OwnershipChangedFlag { get; set; }
    private MalpracticeClaimInfoPresenter _presenter;
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


    public MalpracticeClaimInfoPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new MalpracticeClaimInfoPresenter(this);
            }

            return _presenter;
        }
    }

    public MalpracticeClaimInfo Model { get; set; }
    public string MedicaidId
    {
        get
        {
            if (ViewState["MedicaidId"] == null) ViewState["MedicaidId"] = string.Empty;
            return ViewState["MedicaidId"].ToString();
        }
        set { ViewState["MedicaidId"] = value; }
    }

    public string NPI
    {
        get
        {
            if (ViewState["UserAccount_NPI"] == null) ViewState["UserAccount_NPI"] = string.Empty;
            return ViewState["UserAccount_NPI"].ToString();
        }
        set { ViewState["UserAccount_NPI"] = value; }
    }
    public int RegMalpracticeClaimID
    {
        get
        {
            return ViewState["RegMalpracticeClaimID"] == null ? 0 : Convert.ToInt32(ViewState["RegMalpracticeClaimID"]);
        }
        set
        {
            ViewState["RegMalpracticeClaimID"] = value;
        }
    }


    public bool IndividualProviderType
    {
        get
        {
            return ViewState["IndividualProviderType"] == null ? false : Convert.ToBoolean(ViewState["IndividualProviderType"].ToString());
        }
        set { ViewState["IndividualProviderType"] = value; }
    }


    public bool InProviderDataEntry
    {
        get
        {
            return ViewState["InProviderDataEntry"] == null ? false : Convert.ToBoolean(ViewState["InProviderDataEntry"]);
        }
        set
        {
            ViewState["InProviderDataEntry"] = value;
        }
    }

    private void LoadDropDowns()
    {
        // TODO : EDV What is this doing here??

        DataSet ds = svc.GetProviderTypes();
    }

    public override void LoadData(DataRow dr)
    {

        //Need to split this out into Get Data, Load Data, Set Editability and Set Visibility methods.
        //load radio button
        if (dr != null)
        {
            hdnMalPracticeId.Value = Helper.GetData("REG_MALPRACTICE_CLAIM_ID", dr);
            LoadAddressInfo();
            txtDateOccurence.Text = Helper.GetDate("DateOccurance", dr);
            txtDateClaimFiled.Text = Helper.GetDate("DateClaimFiled", dr);
            txtProfessionalLiability.Text = Helper.GetString("ProfessionalCarrier", dr);
            if (!String.IsNullOrEmpty(Helper.GetString("DEFENDENT_TYPE_ID", dr)))
            {
                if (rblDefendant.Items.FindByValue(Helper.GetString("DEFENDENT_TYPE_ID", dr)) != null)
                {
                    rblDefendant.SelectedValue = Helper.GetString("DEFENDENT_TYPE_ID", dr);
                }
            }
            txtnoofOtherDefendents.Text = Helper.GetString("NUMBEROFOTHERDEFENDENT", dr);
            txtAllegations.Text = Helper.GetString("Allegations", dr);
            txtAllegedInjury.Text = Helper.GetString("AllegedInjury", dr);
            txtDateSettled.Text = Helper.GetDate("CLAIM_SETTLED_DATE", dr)==sqlMinDateTime.ToString("MM/dd/yyyy")?string.Empty: Helper.GetDate("CLAIM_SETTLED_DATE", dr);
            if (!String.IsNullOrEmpty(Helper.GetString("Claim_Status_ID", dr)))
            {
                if (ddlClaimStatus.Items.FindByValue(Helper.GetString("Claim_Status_ID", dr)) != null)
                {
                    ddlClaimStatus.SelectedValue = Helper.GetString("Claim_Status_ID", dr);
                }
            }        
            RegMalpracticeClaimID = Helper.GetInt("REG_MALPRACTICE_CLAIM_ID", dr);
            txtAllegations.Text = Helper.GetString("Allegations", dr);
            txtPolicyNumber.Text = Helper.GetString("PolicyNumber", dr);
            if (ddlIsNPDB.Items.FindByValue(Helper.GetString("Is_NPDB", dr)) != null)
            {
                ddlIsNPDB.SelectedValue = Helper.GetString("Is_NPDB", dr);
            }
            txtPolicyNumber.Text = Helper.GetString("POLICYNUMBER", dr);
            txtnoofOtherDefendents.Text = Helper.GetString("NUMBEROFOTHERDEFENDENT", dr);
            if (ddlMethodofResoultion.Items.FindByValue(Helper.GetString("METHOD_OF_RESULTION_ID", dr)) != null)
            {
                ddlMethodofResoultion.SelectedValue = Helper.GetString("METHOD_OF_RESULTION_ID", dr);
            }
            txtSettledAmount.Text = Helper.GetString("SETTLEMENT_AMOUNT", dr);
            if (ddlIsDead.Items.FindByValue(Helper.GetString("RESULTED_IN_DEATH", dr)) != null)
            {
                ddlIsDead.SelectedValue = Helper.GetString("RESULTED_IN_DEATH", dr);
            }
            txtRoleinCase.Text = Helper.GetString("INVOLVEMENT", dr);
        }
        else
        {
            txtDateOccurence.Text = string.Empty;
            txtDateClaimFiled.Text = string.Empty;
            txtProfessionalLiability.Text = string.Empty;
            rblDefendant.SelectedIndex = -1;
            txtnoofOtherDefendents.Text = string.Empty;
            txtAllegations.Text = string.Empty;
            txtAllegedInjury.Text = string.Empty;
            txtDateSettled.Text = string.Empty;
            ddlIsNPDB.SelectedIndex = -1;
            txtPolicyNumber.Text = string.Empty;
            ddlIsNPDB.SelectedIndex = -1;
            txtnoofOtherDefendents.Text = string.Empty;
            ddlMethodofResoultion.SelectedIndex = -1;
            txtSettledAmount.Text = string.Empty;
            ddlIsDead.SelectedIndex = -1;
            ddlClaimStatus.SelectedIndex = -1;
            RegMalpracticeClaimID = 0;
            txtRoleinCase.Text = string.Empty;
        }
        
    }

    private bool IsIndividualProvider(DataRow dr)
    {
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        return entityTypeID == CON.ProviderCategoryTypeID.Individual || entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile;
    }

    public override bool SaveData()
    {
        // OHPNM-2229  
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && (!rblMalpractice.Visible || (rblMalpractice.Visible && rblMalpractice.SelectedIndex == -1)) && grdMalpracticeClaim.Rows.Count == 0)
        {
            // if this page is required and they haven't clicked "Add New" (!rblMalpractice.Visible) OR
            // they have clicked new but haven't selected a radio button (!rblMalpractice.Visible || (rblMalpractice.Visible && rblMalpractice.SelectedIndex == -1))
            // then don't let them "Save" the data on the screen
            return false;
        }
        SaveMalpracticeRadioButton();
        if (rblMalpractice.SelectedValue != "1")
        {            
            return true;
        }

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            return true;
        }

        if (malpracticeDetail.Visible)
        {
            if (!ValidateInput())
            {
                return false;
            }
            SaveAddressDetails();
            if (_address.AddressId > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                Dictionary<string, string> parms = new Dictionary<string, string>();
                DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
                bool doUpdate = Helper.HasRows(ds);
                MalpracticeClaimInfo obj = new MalpracticeClaimInfo();

                if (doUpdate)
                {
                    obj.DateOccurance = (String.IsNullOrEmpty(txtDateOccurence.Text)) ? (DateTime?)null : Convert.ToDateTime(txtDateOccurence.Text);
                    obj.DateClaimFiled = (String.IsNullOrEmpty(txtDateClaimFiled.Text)) ? (DateTime?)null : Convert.ToDateTime(txtDateClaimFiled.Text);
                    obj.ProfessionalCarrier = txtProfessionalLiability.Text;
                    obj.DefendentTypeID = (String.IsNullOrEmpty(rblDefendant.SelectedValue)) ? (int?)null : Convert.ToInt32(rblDefendant.SelectedValue);
                    obj.DateClaimSettled = (String.IsNullOrEmpty(txtDateSettled.Text)) ? sqlMinDateTime : Convert.ToDateTime(txtDateSettled.Text);
                    obj.Allegations = txtAllegations.Text;
                    obj.AllegedInjury = txtAllegedInjury.Text;
                    obj.ClaimStatusID = (String.IsNullOrEmpty(ddlClaimStatus.SelectedValue)) ? (int?)null : Convert.ToInt32(ddlClaimStatus.SelectedValue);
                    if (RegMalpracticeClaimID != 0)
                        obj.REG_MALPRACTICE_CLAIM_ID = Convert.ToInt32(RegMalpracticeClaimID);
                    obj.RegID = this.WorkflowPage.RegistrationId;
                    obj.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                    obj.PolicyNumber = txtPolicyNumber.Text.Trim();
                    obj.ISNPDB = ddlIsNPDB.SelectedValue.ToUpper() == "TRUE" ? true : false;
                    if (!string.IsNullOrEmpty(txtnoofOtherDefendents.Text))
                    {
                        obj.NoofOtherDefendents = Convert.ToInt32(txtnoofOtherDefendents.Text);
                    }
                    else
                    {
                        obj.NoofOtherDefendents = 0;
                    }
                    obj.MethodOfResolutionId = string.IsNullOrEmpty(ddlMethodofResoultion.SelectedValue) ? (int?)null : Convert.ToInt32(ddlMethodofResoultion.SelectedValue);
                    obj.SettledAmount = string.IsNullOrEmpty(txtSettledAmount.Text) ? 0.0 : Convert.ToDouble(txtSettledAmount.Text);
                    obj.ResultedInDeath = ddlIsDead.SelectedValue.ToUpper() == "TRUE" ? true : false;
                    obj.Involvement = txtRoleinCase.Text.Trim();

                    obj.RegAddressId = _address.AddressId;

                    Model = obj;
                    presenter.SaveRegMalpracticeClaimInfo(Model);
                }
            }
        }
        return true;
    }

    private void SaveMalpracticeRadioButton()
    {
        if (rblMalpractice.SelectedIndex != -1)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("HAS_MALPRACTICE", rblMalpractice.SelectedValue);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);
        }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator()
        {
            IsValid = false,
            ErrorMessage = errMsg,
            ValidationGroup = "valMalpracticeClaimInfo"
        };
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private void AddError(string errorMessage, string validationGroup, ref bool isGood)
    {
        CustomValidator val = new CustomValidator()
        {
            IsValid = false,
            ErrorMessage = errorMessage,
            ValidationGroup = validationGroup
        };
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public bool ValidateInput()
    {
        bool isGood = true;
        this.Page.Validate("valMalpracticeClaimInfo");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valMalpracticeClaimInfo") && !v.IsValid)
                {
                    isGood = v.IsValid;
                    return isGood;
                }
            }
            catch
            {
                continue;
            }
        }
        if (rblMalpractice.SelectedValue == "1" && malpracticeDetail.Visible)
        {
            if (string.IsNullOrEmpty(txtDateOccurence.Text.Trim()))
            {
                AddError("Date Of Occurence is required.", ref isGood);
            }
            if (string.IsNullOrEmpty(txtDateClaimFiled.Text.Trim()))
            {
                AddError("Date Claim Filed is required.", ref isGood);
            }
            if (ddlClaimStatus.SelectedIndex == -1)
            {
                AddError("Claim Status is required.", ref isGood);
            }
            if (string.IsNullOrEmpty(txtProfessionalLiability.Text.Trim()))
            {
                AddError("Professional Liability is required.", ref isGood);
            }
            if (ddlMethodofResoultion.SelectedIndex == 0 && ddlClaimStatus.SelectedItem.Value == "2")
            {
                AddError("Method of resolution is required.", ref isGood);
            }
            if (rblDefendant.SelectedIndex == -1)
            {
                AddError("Select Defendant Type.", ref isGood);
            }
            if (string.IsNullOrEmpty(txtRoleinCase.Text.Trim()))
            {
                AddError("Enter your role in case.", ref isGood);
            }
            if (string.IsNullOrEmpty(txtAllegations.Text.Trim()))
            {
                AddError("Allegations is required.", ref isGood);
            }
        }
        if (
            (ddlMethodofResoultion.SelectedItem.Text.Trim() ==CON.MethodOfResoultion.Settled) ||
            (ddlMethodofResoultion.SelectedItem.Text.Trim() == CON.MethodOfResoultion.Mediation) ||
            (ddlMethodofResoultion.SelectedItem.Text.Trim() == CON.MethodOfResoultion.Arbitration) ||
            (ddlMethodofResoultion.SelectedItem.Text.Trim() == CON.MethodOfResoultion.JudgementforthePlaintiff)
            )           
        {
            // Date filed must be filled in if a case was filed
            if (txtSettledAmount.Text.Trim() == string.Empty)
            {
                AddError("Settled amount is required.", ref isGood);
            }
            if (txtDateSettled.Text.Trim() == string.Empty)
            {
                AddError("Date Claim Settled is required.", ref isGood);
            }
        }
        return isGood;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();

        base.OnLoad(e);
        Page.Title = "Malpractice Claims History";
    }
    public override void LoadControlData()
    {
        LoadMalpracticeClaimInfo();
        LoadAddressDetails(null);
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnMalpracticeClaims.Visible = false;
            }
        }
        MethodofResolutionLabelChange();
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }


    private void LoadMalpracticeClaimInfo()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectMalpracticeClaimByRegID(this.WorkflowPage.RegistrationId);
        DataTable dtMalpracticeClaims = Helper.HasRows(ds) ? ds.Tables["RegData"] : null;
        grdMalpracticeClaim.DataSource = this.DataList = dtMalpracticeClaims;
        grdMalpracticeClaim.DataBind();


        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            string hasMalpractice = Helper.GetString("HAS_MALPRACTICE", ds.Tables[0].Rows[0]);
            if (hasMalpractice.ToLower() == "true")
            {
                rblMalpractice.SelectedValue = "1";
                //HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
                //table.Style.Add("display", "block");
            }
            else if (hasMalpractice.ToLower() == "false")
            {
                rblMalpractice.SelectedValue = "0";
                HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
                table.Style.Add("display", "none");
                pnlMalpracticeClaim.Visible = true;
            }
            else
            {
                rblMalpractice.ClearSelection();
                HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
                table.Style.Add("display", "none");
            }
        }
        DataSet dsMethodResolutions = svc.SelectReferenceDataWithoutParam("usp_SelectMethodResolution");
        Helper.LoadList(ddlMethodofResoultion, dsMethodResolutions.Tables[0], "Resolution_Desc", "Resolution_Id", false);
        DataSet ds1 = presenter.GetDefendentType();
        Helper.LoadList(rblDefendant, ds1.Tables[0], "DEFENDENT_Type_Name", "DEFENDENT_Type_ID", false);
        ds1 = presenter.GetClaimStatus();
        Helper.LoadList(ddlClaimStatus, ds1.Tables[0], "claim_status_Name", "claim_status_ID", false);
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        HiddenField hdnAddressId = row.FindControl("hdnRegAddressId") as HiddenField;

        ResetFeilds();
        int index = Convert.ToInt32(e.CommandArgument);

        malpracticeDetail.Visible = true;
        pnlMalpracticeClaim.Visible = true;
        rblMalpractice.SelectedValue = "1";
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);

        RegAddessId = !string.IsNullOrEmpty(hdnAddressId.Value) ? int.Parse(hdnAddressId.Value) : (int?)null;
        if (RegAddessId.HasValue)
        {
            LoadAddressInfo(RegAddessId);
        }
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        malpracticeDetail.Visible = true;
        pnlMalpracticeClaim.Visible = true;
        ResetFeilds();
    }

    protected void ResetFeilds()
    {
        this.LoadAddressDetails(null);
        txtAllegedInjury.Text = string.Empty;
        txtAllegations.Text = string.Empty;
        txtDateClaimFiled.Text = string.Empty;
        txtDateSettled.Text = string.Empty;
        txtDateOccurence.Text = string.Empty;
        txtnoofOtherDefendents.Text = string.Empty;
        txtPolicyNumber.Text = string.Empty;
        txtProfessionalLiability.Text = string.Empty;
        txtRoleinCase.Text = string.Empty;
        txtSettledAmount.Text = string.Empty;
    }

    public override bool ValidateData()
    {
        bool isGood = true;
        bool isStateadmininPeriodicchecks = this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks && Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);

        if (!isStateadmininPeriodicchecks)
        {
            string validationGroup = "valProviderInfoHeader";
            if (rblMalpractice.Visible && rblMalpractice.SelectedValue == "1"
                && ((this.grdMalpracticeClaim.Rows.Count == 0 && !ValidateInput())
                    || (this.grdMalpracticeClaim.Rows.Count > 0 && !ValidateInput())))
            {
                AddError("*Must add a malpractice claim if there was one in the last 10 years.", validationGroup, ref isGood);
            }

            if (rblMalpractice.Visible && rblMalpractice.SelectedIndex == -1)
            {
                AddError("* A Yes or No answer is required for malpractice question.", validationGroup, ref isGood);
            }
        }

        return isGood;
    }

    public void GetMalpracticeClaimInfo(DataSet ds)
    {

    }
    protected void rblMalpractice_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblMalpractice.SelectedValue == "1")
        {
            malpracticeDetail.Visible = true;
            HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
            table.Style.Add("display", "block");
        }
        else 
        {
            HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
            table.Style.Add("display", "none");
        }
    }
    //protected void rblMalpractice_PreRender(object sender, EventArgs e)
    //{

    //    // TODO: EDV This is not working
    //    RadioButtonList rdoList = (RadioButtonList)sender;
    //    foreach (ListItem item in rdoList.Items)
    //    {
    //        item.Attributes.Add("onclick", string.Format("showAddMalpracticeClaimBtn('{0}')", item.Value));
    //    }
    //    if (rdoList.SelectedValue == "1")
    //    {
    //        malpracticeDetail.Visible = true;
    //        HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
    //        table.Style.Add("display", "block");
    //    }
    //    else
    //    {
    //        HtmlControl table = (HtmlControl)this.FindControl("ParentTable");
    //        table.Style.Add("display", "none");
    //    }
    //}

    public override string ValidationGroup
    {
        get { return "valMalpracticeClaimInfo"; }
    }

    public override string Title
    {
        get { return "Malpractice Claims"; }
    }

    public override string IdText
    {
        get { return "ucMalpracticeClaim_" + this.WorkflowPage.RegistrationId; }
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
        bool isEdit = string.IsNullOrEmpty(hdnMalPracticeId.Value) ? false : true;

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

        DataSet dsProvider = svc.SelectRegProviderInfo(this.WorkflowPage.RegistrationId, CON.AddressType.RealEstateIndividual);
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

        if (addressRow != null)
        {
            _address.Load(addressRow, drProvider);
        }
        else
        {
            _address.Load(drProvider);
        }
        ucAddress.CopyPropertiesFrom(_address);
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
    private void SetVisibleFields()
    {
        ucAddress.ContactVisible = true;
        ucAddress.Phone1Visible = true;
        ucAddress.PhoneExt1Visible = true;
        ucAddress.VerifyAddress = true;
        ucAddress.Address1Title = "Carrier Address Line1*";
        ucAddress.Address2Title = "Carrier Address Line2";
        ucAddress.SetPhone1Label = "Phone Number";
        ucAddress.VisibleCounty = false;
        ucAddress.ContactVisible = false;
        ucAddress.OrgNameVisible = false;
        ucAddress.IsCountyHidden = true;

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
        else
        {
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.MalpracticeClaimsHistory)
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

    private void LoadAddressInfo(int? addressId = null)
    {
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectProviderAddressInfo(this.WorkflowPage.RegistrationId, _addressTypeId);
            if (Helper.HasRows(ds))
            {
                if (ds.Tables.Contains("AddressInfo"))
                {
                    DataTable dtAddressInfo = Helper.HasRows(ds) ?
                                         ((addressId != null && addressId > 0) ?
                                             (ds.Tables["AddressInfo"]).AsEnumerable()
                                             .Where(row => row.Field<int?>("REG_ADDRESS_ID") == addressId).CopyToDataTable() : ds.Tables["AddressInfo"])
                                    : null;
                    if (Helper.HasRows(dtAddressInfo))
                    {
                        this.LoadAddressDetails(dtAddressInfo.Rows[0]);
                        if (Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]) > 0)
                            ucAddress.AddressTypeId = Helper.GetInt("ADDRESS_TYPE_ID", dtAddressInfo.Rows[0]);
                    }
                    else
                    {
                        this.LoadAddressDetails(null);
                    }
                }
                else
                {
                    this.LoadAddressDetails(null);
                }
            }
            else
            {
                this.LoadAddressDetails(null);
            }
        } catch (Exception ex)
        {
            this.LoadAddressDetails(null);
        }
    }

    protected void MethodofResolutionLabelChange()
    {
        if (ddlClaimStatus.SelectedItem.Value=="1")
        {
            lblmethodResolution.Text = "Method of Resolution ";
            ddlMethodofResoultion.Items.Insert(0, "");
        }
        else
        {
            lblmethodResolution.Text = "Method of Resolution *";
        }
    }
    protected void ddlClaimStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        MethodofResolutionLabelChange();
    }
}