using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_SubstituteW9Form : BaseSectionControl
{
    Regex regTaxID = new Regex(@"(?!0{9})(?!9{9})^\d{9}$");

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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

    #region dtProvider
    DataTable _dtProvider;

    public DataTable dtProvider
    {
        get
        {
            if (_dtProvider == null)
            {
                DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
                _dtProvider = Helper.HasRows(ds) ? ds.Tables[0] : null;
            }

            return _dtProvider;
        }
    }
    #endregion

    public EventHandler InvalidateAgreements;
    public int EntityTypeId
    {
        get
        {
            if (ViewState["EntityTypeId"] == null) ViewState["EntityTypeId"] = 0;
            return Convert.ToInt32(ViewState["EntityTypeId"]);
        }
        set { ViewState["EntityTypeId"] = value; }
    }


    public int MMISSpecialtyTypeID
    {
        get
        {
            if (ViewState["MMISSpecialtyTypeID"] == null) ViewState["MMISSpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["MMISSpecialtyTypeID"]);
        }
        set { ViewState["MMISSpecialtyTypeID"] = value; }
    }
    
    public int ProviderTypeId
    {
        get
        {
            if (ViewState["ProviderTypeId"] == null) ViewState["ProviderTypeId"] = 0;
            return Convert.ToInt32(ViewState["ProviderTypeId"]);
        }
        set { ViewState["ProviderTypeId"] = value; }
    }

    public int DIDDReferralId
    {
        get
        {
            if (ViewState["DIDDReferralId"] == null) ViewState["DIDDReferralId"] = 0;
            return Convert.ToInt32(ViewState["DIDDReferralId"]);
        }
        set { ViewState["DIDDReferralId"] = value; }
    }

    public int SpecialtyTypeID
    {
        get
        {
            if (ViewState["SpecialtyTypeID"] == null) ViewState["SpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set { ViewState["SpecialtyTypeID"] = value; }
    }
    bool IdentifyingInfoExists = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //DataSet ds = svc.SelectProviderPaymentType();
            //Helper.LoadList(rblProviderPaymentType, ds.Tables[0], "Provider_Payment_Type_name", "Provider_Payment_Type_ID", false);
            //rblProviderPaymentType.DataBind();
        }
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.W9Form)
        {
            SetVisibility();
            SetEditability();
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName))
        {
        }

        radioCategory.Items.Clear();
        DataTable dtTaxEntCat = svc.SelectTaxEntityTypes().Tables[0];
        int i = 1; 
        foreach (DataRow row in dtTaxEntCat.Rows)
        {
            radioCategory.Items.Add(new ListItem(i.ToString() + ". " + row["TAX_ENTITY_TYPE"].ToString(), row["TAX_ENTITY_TYPE_ID"].ToString()));
            i += 1;
        }

        radioPracticeType.Items.Clear();
        DataTable dtPracticeType = svc.SelectPracticeTypeW9().Tables[0];
        i = 1;
        foreach (DataRow row in dtPracticeType.Rows)
        {
            radioPracticeType.Items.Add(new ListItem(i.ToString() + ". " + row["PRACTICE_TYPE_NAME"].ToString(), row["PRACTICE_TYPE_ID"].ToString()));
            i += 1;
        }

        // Load the registered state drop down
        // TODO: EDV Get this from cache.
        DataSet dsStates = svc.GetStates();
        Helper.LoadList(ddlStateRegistered, dsStates, "StateName", "StateId", true);

        //commented based on requirement changes
        /*DataSet ds = svc.GetGovernmentType();
        foreach (DataRow row in ds.Tables["GovernmentType"].Rows)
        {
            ListItem item = new ListItem(row["Government_Type_Name"].ToString(), row["Government_Type_ID"].ToString());
            ddlGovernment.Items.Add(item);
        }
        ddlGovernment.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        ds = svc.GetPROFITType();
        foreach (DataRow row in ds.Tables["PROFIT_Type"].Rows)
        {
            ListItem item = new ListItem(row["PROFIT_Name"].ToString(), row["PROFIT_ID"].ToString());
            ddlProfit.Items.Add(item);
        }
        ddlProfit.Items.Insert(0, new ListItem(string.Empty, string.Empty));*/
        DataSet ds = svc.GetPROFITStatus();
        foreach (DataRow row in ds.Tables["PROFIT_STATUS"].Rows)
        {
            ListItem item = new ListItem(row["PROFIT_STATUS_DISPLAY"].ToString(), row["PROFIT_STATUS_ID"].ToString());
            rblProfitStatus.Items.Add(item);
        }
        //rblProfitStatus.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        /*ds = svc.GetTYPE_OF_OWNERSHIP();
        foreach (DataRow row in ds.Tables["TYPE_OF_OWNERSHIP"].Rows)
        {
            ListItem item = new ListItem(row["TYPE_OF_OWNERSHIP_DISPLAY"].ToString(), row["TYPE_OF_OWNERSHIP_ID"].ToString());
            ddlTypeOfOwnership.Items.Add(item);
        }
        ddlTypeOfOwnership.Items.Insert(0, new ListItem(string.Empty, string.Empty));*/
        LoadProvider();
        SetVisibility();
        SetEditability();
    }

    public override bool SaveData()
    {
        if (Registration.PreviewingRegistrationSection())
        {
            return false;
        }

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return true;

        //if (ds.Tables[0].Rows[0]["TAX_ENTITY_TYPE_ID"].ToString() == radioCategory.SelectedValue) return true;

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_SERVICE_LOCATION_ID", ds.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"].ToString());
        if (radioCategory.SelectedValue != null && radioCategory.SelectedValue != "")
        {
            parms.Add("TAX_ENTITY_TYPE_ID", radioCategory.SelectedValue);
        }
        if (!string.IsNullOrEmpty(radioPracticeType.SelectedValue))
        {
            parms.Add("PRACTICE_TYPE_ID", radioPracticeType.SelectedValue);
        }
        /*if (ddlGovernment.SelectedValue != null && ddlGovernment.SelectedValue != "")
        {
            parms.Add("GOVERNMENT_TYPE_ID", ddlGovernment.SelectedValue);
        }
        if (ddlProfit.SelectedValue != null && ddlProfit.SelectedValue != "")
        {
            parms.Add("PROFIT_ID", ddlProfit.SelectedValue);
        }*/
        if (rblProfitStatus.SelectedValue != null && rblProfitStatus.SelectedValue != "")
        {
            parms.Add("PROFIT_STATUS_ID", rblProfitStatus.SelectedValue);
        }

        if (!string.IsNullOrEmpty(txtFiscalYearEnd.Text))
        {
            parms.Add("FISCAL_YEAR_END", txtFiscalYearEnd.Text);
        }

        // Set type of ownership
        int typeOfOwnershiID = CON.TypeOfOwnershipID.Others;

        if (MMISSpecialtyTypeID == 88)
        {
            if (radioCategory.SelectedValue == "9")
            {
                typeOfOwnershiID = CON.TypeOfOwnershipID.Public;
            }
            else
            {
                typeOfOwnershiID = CON.TypeOfOwnershipID.Private;
            }
        }
        parms.Add("TYPE_OF_OWNERSHIP_ID", typeOfOwnershiID.ToString());

        /*if (ddlTypeOfOwnership.SelectedValue != null && ddlTypeOfOwnership.SelectedValue != "")
        {
            parms.Add("TYPE_OF_OWNERSHIP_ID", ddlTypeOfOwnership.SelectedValue);
        }  
            //parms.Add("ISNON_PROFIT", rblNonProfit.SelectedValue);
        
            parms.Add("ISREGISTERED_IN_STATE", rblRegisteredBusiness.SelectedValue);
        */
        if (!string.IsNullOrEmpty(ddlStateRegistered.SelectedValue))
        {
            parms.Add("TAX_ENTITY_STATE_ID", ddlStateRegistered.SelectedValue);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION", parms);

        // Update SSN / EIN 
        if (Helper.HasRows(dtProvider))
        {
           
          // string taxID = dtProvider.Rows[0]["TAX_ID"].ToString();
            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            
                if (!txtSSN.ReadOnly)
                {
                    if (txtSSN.Text.Trim() != string.Empty) //Don't add ALT_TAX_ID_TYPE if ALT_TAX_ID is null
                    {
                        parms.Add("ALT_TAX_ID", txtSSN.Text.Trim());
                        parms.Add("ALT_TAX_ID_TYPE", "15");
                    }
                }
            
                if (!txtEIN.ReadOnly)
                {
                    if (txtEIN.Text.Trim() != string.Empty) //Don't add ALT_TAX_ID_TYPE if ALT_TAX_ID is null
                    {
                        parms.Add("ALT_TAX_ID", txtEIN.Text.Trim());
                        parms.Add("ALT_TAX_ID_TYPE", "16");
                    }
                }
            

            svc.UpdateRegistrationDataTable("PROVIDERCustom", parms);
        }

        if (InvalidateAgreements != null) InvalidateAgreements(this, new EventArgs());

        return true;
    }

    public string ValidateDataPS()
    {
        string rtn = string.Empty;
        if (string.IsNullOrEmpty(radioCategory.SelectedValue) && pnlTaxClassification.Visible)
        {
            rtn = "* Tax Category is required.";
            return rtn;
        }

        return rtn;
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        if (Registration.PreviewingRegistrationSection())
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Previous registration sections must be completed first.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (!txtSSN.ReadOnly && !string.IsNullOrEmpty(txtSSN.Text.Trim()) && !regTaxID.IsMatch(txtSSN.Text.Trim()))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false; 
            val.ErrorMessage = "* Enter a 9 digit SSN.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (!txtEIN.ReadOnly && !string.IsNullOrEmpty(txtEIN.Text.Trim()) && !regTaxID.IsMatch(txtEIN.Text.Trim()))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Enter a 9 digit EIN.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (pnlFiscalYearEndHosp.Visible && string.IsNullOrEmpty(txtFiscalYearEnd.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Enter Fiscal Year End.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (pnlFiscalYearEndHosp.Visible && !string.IsNullOrEmpty(txtFiscalYearEnd.Text))
        {
            DateTime dt;
            if (DateTime.TryParseExact(txtFiscalYearEnd.Text, "MM/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt) == false &&
                DateTime.TryParseExact(txtFiscalYearEnd.Text, "M/dd/yyyy", null, System.Globalization.DateTimeStyles.None, out dt) == false &&
                DateTime.TryParseExact(txtFiscalYearEnd.Text, "MM/d/yyyy", null, System.Globalization.DateTimeStyles.None, out dt) == false &&
                DateTime.TryParseExact(txtFiscalYearEnd.Text, "M/d/yyyy", null, System.Globalization.DateTimeStyles.None, out dt) == false)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "* Enter Valid Date in Fiscal Year End field.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);
                isGood = false;
            }
        }


        if (string.IsNullOrEmpty(ddlStateRegistered.SelectedValue) && pnlTaxClassification.Visible)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* State Registered is required.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (string.IsNullOrEmpty(radioCategory.SelectedValue) && pnlTaxClassification.Visible)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Tax Category is required.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (string.IsNullOrEmpty(radioPracticeType.SelectedValue) && pnlPracticeType.Visible)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Practice Type is required.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (string.IsNullOrEmpty(rblProfitStatus.SelectedValue) && pnlProfitStatus.Visible)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Profit Status is required.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        return isGood;
    }

    #region Load
    private void LoadProvider()
    {
        if (!Helper.HasRows(dtProvider))
            return;

        bool editable = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName); // Provider, admin and operator can edit.

        RS02.Enabled = 
        radioPracticeType.Enabled=
        radioCategory.Enabled = rblProfitStatus.Enabled = txtFiscalYearEnd.Enabled = editable;

        RS02.Text = dtProvider.Rows[0]["NAME"].ToString();

        setTaxIDFields();

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (Helper.HasRows(ds))
        {
            if (ds.Tables[0].Rows[0]["TAX_ENTITY_TYPE_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TAX_ENTITY_TYPE_ID"].ToString()) &&
                ds.Tables[0].Rows[0]["TAX_ENTITY_TYPE_ID"].ToString() != "0")
                radioCategory.SelectedValue = ds.Tables[0].Rows[0]["TAX_ENTITY_TYPE_ID"].ToString();
            if (ds.Tables[0].Rows[0]["PRACTICE_TYPE_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PRACTICE_TYPE_ID"].ToString()) &&
                ds.Tables[0].Rows[0]["PRACTICE_TYPE_ID"].ToString() != "0")
                radioPracticeType.SelectedValue = ds.Tables[0].Rows[0]["PRACTICE_TYPE_ID"].ToString();
            /*if (ds.Tables[0].Rows[0]["GOVERNMENT_TYPE_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["GOVERNMENT_TYPE_ID"].ToString()))
                ddlGovernment.Items.FindByValue(ds.Tables[0].Rows[0]["GOVERNMENT_TYPE_ID"].ToString()).Selected = true;
            if (ds.Tables[0].Rows[0]["PROFIT_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PROFIT_ID"].ToString()))
                ddlProfit.Items.FindByValue(ds.Tables[0].Rows[0]["PROFIT_ID"].ToString()).Selected = true;*/
            /*if (ds.Tables[0].Rows[0]["ISNON_PROFIT"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ISNON_PROFIT"].ToString()))
                rblNonProfit.SelectedValue = ds.Tables[0].Rows[0]["ISNON_PROFIT"].ToString();*/
            /*if (ds.Tables[0].Rows[0]["ISREGISTERED_IN_STATE"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ISREGISTERED_IN_STATE"].ToString()))
                rblRegisteredBusiness.SelectedValue = ds.Tables[0].Rows[0]["ISREGISTERED_IN_STATE"].ToString();*/
            if (ds.Tables[0].Rows[0]["PROFIT_STATUS_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["PROFIT_STATUS_ID"].ToString()))
                rblProfitStatus.Items.FindByValue(ds.Tables[0].Rows[0]["PROFIT_STATUS_ID"].ToString()).Selected = true;
            /*if (ds.Tables[0].Rows[0]["TYPE_OF_OWNERSHIP_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TYPE_OF_OWNERSHIP_ID"].ToString()))
                ddlTypeOfOwnership.Items.FindByValue(ds.Tables[0].Rows[0]["TYPE_OF_OWNERSHIP_ID"].ToString()).Selected = true;*/
            if (ds.Tables[0].Rows[0]["FISCAL_YEAR_END"] != null && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FISCAL_YEAR_END"].ToString()) && Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "FiscalYearEnd", this.WorkflowPage.IsWaiverServiceProvider))
            {
                txtFiscalYearEnd.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["FISCAL_YEAR_END"]).ToShortDateString();
            }
            if (ds.Tables[0].Rows[0]["TAX_ENTITY_STATE_ID"] != null &&
                !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["TAX_ENTITY_STATE_ID"].ToString()) &&
                ds.Tables[0].Rows[0]["TAX_ENTITY_STATE_ID"].ToString() != "0")
                ddlStateRegistered.SelectedValue = ds.Tables[0].Rows[0]["TAX_ENTITY_STATE_ID"].ToString();

            grdW9Address.DataSource = ds.Tables[0];
            grdW9Address.DataBind();
        }

    }

    private void setTaxIDFields()
    {
        if (!Helper.HasRows(dtProvider))
            return;

        int taxIDTypeID = 0;
        if (int.TryParse(dtProvider.Rows[0]["TAX_ID_TYPE_ID"].ToString(), out taxIDTypeID) && taxIDTypeID == 15)
        {
            
            if (!String.IsNullOrEmpty(dtProvider.Rows[0]["TAX_ID"].ToString()))
            {
                txtSSN.Text = dtProvider.Rows[0]["TAX_ID"].ToString();
                txtSSN.ReadOnly = true;
                txtSSN.CssClass = "formFieldReadOnly formField300";
                txtSSN.Enabled  = false;
            }

            if (!String.IsNullOrEmpty(dtProvider.Rows[0]["ALT_TAX_ID"].ToString()))
            {
                txtEIN.Text = dtProvider.Rows[0]["ALT_TAX_ID"].ToString();
            }
        }

        if (int.TryParse(dtProvider.Rows[0]["TAX_ID_TYPE_ID"].ToString(), out taxIDTypeID) && taxIDTypeID == 16)
        {

            if (!String.IsNullOrEmpty(dtProvider.Rows[0]["TAX_ID"].ToString()))
            {
                txtEIN.Text = dtProvider.Rows[0]["TAX_ID"].ToString();
                txtEIN.ReadOnly = true;
                txtEIN.CssClass = "formFieldReadOnly formFieldAuto";
                txtEIN.Enabled = false;
            }

            if (!String.IsNullOrEmpty(dtProvider.Rows[0]["ALT_TAX_ID"].ToString()))
            {
                txtSSN.Text = dtProvider.Rows[0]["ALT_TAX_ID"].ToString();
            }
        }
    }

    #endregion
    private void SetEditability()
    {
        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInAdminRole())
        {
            //rblProviderPaymentType.Enabled = true;
            //txtPaymentEffectiveDate.Enabled = true;
            txtFiscalYearEnd.Enabled = false;
            radioCategory.Enabled = false;
            ddlStateRegistered.Enabled = false;
            radioPracticeType.Enabled = false;
            rblProfitStatus.Enabled = false;
        }
        else
        {
            //rblProviderPaymentType.Enabled = false;
            //txtPaymentEffectiveDate.Enabled = false;
        }
    }
    private void SetVisibility()
    {
        if (EntityTypeId == 0)
        {
            int diddReferralId = 0, entityTypeId = 0, providerTypeId = 0, specialtyTypeID = 0, taxIDTypeID = 0;
            string taxID = string.Empty;
            Registration.SetEntityProviderTypesDIDD(this.WorkflowPage.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
            EntityTypeId = entityTypeId;
            ProviderTypeId = providerTypeId;
            DIDDReferralId = diddReferralId;
            SpecialtyTypeID = specialtyTypeID;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            MMISSpecialtyTypeID = Helper.GetInt("MMISSpecialtyTypeID", ds.Tables[0].Rows[0]);
        }

        // Set whether the page is visible or not
        pnlFiscalYearEndHosp.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "FiscalYearEnd", this.WorkflowPage.IsWaiverServiceProvider); //ProviderTypeId == 9; // Show fiscal year end if the provider type is hospital.

        pnlTaxClassification.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "TypeOfEntity", this.WorkflowPage.IsWaiverServiceProvider, this.WorkflowPage.CurrentTaskName);

        pnlProfitStatus.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "ProfitStatus", this.WorkflowPage.IsWaiverServiceProvider);
        pnlW9Address.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "W9Address", this.WorkflowPage.IsWaiverServiceProvider, this.WorkflowPage.CurrentTaskName);
        /*DCPDMS-2147 */
        // pnlPracticeType.Visible = Registration.PageIsVisible(EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "PracticeType", SessionVarRetriever.CurrentTaskName);
        //if(Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsLoggedInUserInAdminRole())
        //{
        //    pnlTaxClassification.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "TypeOfEntity", this.WorkflowPage.IsWaiverServiceProvider, CON.RegistrationTaskName.ProviderReview);
        //    pnlW9Address.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "W9Address", this.WorkflowPage.IsWaiverServiceProvider, CON.RegistrationTaskName.ProviderReview);
        //    pnlPracticeType.Visible = Registration.PageIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Substitute W9", "PracticeType", this.WorkflowPage.IsWaiverServiceProvider, CON.RegistrationTaskName.ProviderReview);
        //}
    }

    private bool CheckUploadedDocuments()
    {
        // Check if there are documents uploaded per license
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        // If the page is not visible no reason to check further
        if (!Registration.PageIsVisible(this.WorkflowPage.RegistrationId, Registration.GetStepText(CON.RegistrationPageType.ApplicationFee), this.WorkflowPage.IsWaiverServiceProvider)) return true;

        DataSet dsDoc = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.ApplicationFee, "W-9",
            CON.RegistrationPageType.Certification.ToString(), null);
        bool isGood = true;
        if (!Helper.HasRows(dsDoc)) isGood = false;

        return isGood;
    }

    #region W9 Address...

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string providerTypeName = svc.GetProviderTypeByRegId(this.WorkflowPage.RegistrationId);
        btnSave.Visible = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
        btnCancel.Text = btnSave.Visible ? "Cancel" : "Close";
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;

        switch (e.CommandName)
        {
            case "W9Address":
                DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
                dr = ds.Tables[0].Rows[index];
                lblTitle.Text = "Edit W9 Address";
                btnSave.ValidationGroup = "W9Address";
                ucW9Address.LoadData(dr, true);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
            default:
                break;
        }
    }



    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            mpe.Show();
            return;
        }
        if (ucW9Address.SaveData())
        {
            LoadProvider();
        }
        else
        {
            btnSave.Enabled = true;
            mpe.Show();
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        btnSave.Visible = false;
        btnCancel.Text = "Close";
        switch (e.CommandName)
        {
            case "W9AddressHistory":
                lblTitle.Text = "W9 Address History";
                ucW9AddressHistory.LoadData();
                mltPopup.ActiveViewIndex = 1;
                mpe.Show();
                break;
            default:
                break;
        }
    }

    #endregion

    public override void LoadData(DataRow dr = null)
    {
        
    }

    public override string ValidationGroup
    {
        get { return "valSubstituteW9Form"; }
    }

    public override string Title
    {
        get { return "W9 Form Information"; }
    }

    public override string IdText
    {
        get { return "ucSubstituteW9Form_" + this.WorkflowPage.RegistrationId; }
    }
}