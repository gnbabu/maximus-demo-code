using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Pages_Orientation : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public bool OwnershipChangedFlag { get; set; }
    private DataTable dtOrientation = null;

    public bool CanEdit
    {
        get
        {
            //Must be a Site Visit Operator and must not be coming from provider search
            return Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Identification)
        {
            btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup + 
                "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            ucOrientationInfo.ReloadPopupEvent += new PopupControls_OrientationInfo.ReloadPopupEventHandler(ucOrientationInfo_ReloadPopupEvent);
            ucOrientationInfo.OwnershipChangedFlag = OwnershipChangedFlag;
        }
    }

    void ucOrientationInfo_ReloadPopupEvent()
    {
        ucOrientationInfo.OwnershipChangedFlag = OwnershipChangedFlag;
        mpe.Show();
    }

    private void LoadOrientationInfo()
    {
        grdOrientationInfo.DataSource = ucOrientationInfo.DataList = null;
        //if (!Helper.HasRows(dtOrientation))
        //{
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION");
            if (Helper.HasRows(ds)) dtOrientation = ds.Tables[0];
        //}
        grdOrientationInfo.DataSource = ucOrientationInfo.DataList = dtOrientation;
        grdOrientationInfo.DataBind();

        btnAddOrientationInfo.Visible = (this.WorkflowPage.RegistrationIdSelected == 0 ? (grdOrientationInfo.Rows.Count == 0) : false);
        btnHistoryOrientationInfo.Visible = (grdOrientationInfo.Rows.Count > 0);
        SetTakeActionVisibility();
    }

    public override void LoadData(DataRow row)
    {
    }

    public override bool SaveData()
    {
        return true;
    }

    public override bool ValidateData()
    {
        bool isGood = true;
        if (grdOrientationInfo.Rows.Count == 0)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Enter an Organization";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        if (!GridDataValid())
            isGood = false;

        string NPIErrorMessage = string.Empty;
        if (!Registration.IsValidNppesNpi(this.WorkflowPage.RegistrationId, ref NPIErrorMessage) && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* " + NPIErrorMessage;
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);
            isGood = false;
        }

        return isGood;
    }

    private bool IsClinic()
    {
        string provName = Helper.GetProviderTypeName(this.WorkflowPage.RegistrationId);
        return provName.Equals(CON.GroupProviderType.GroupFQHC) || provName.Equals(CON.GroupProviderType.GroupRHC);
    }

    private bool ProviderIs044()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
        if (!Helper.HasRows(ds)) return false;
        string medId = Helper.GetString("MEDICAID_ID", ds.Tables[0].Rows[0]);
        if (string.IsNullOrEmpty(medId)) return false;
        string medIdType = medId.Substring(0, 3);
        return medIdType.Equals("044");
    }

    private bool IsIndividualProvider(DataRow dr)
    {
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        return entityTypeID == CON.ProviderCategoryTypeID.Individual; //|| entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile;
    }
    /* Need for conversion providers */
    private bool GridDataValid()
    {
        bool isValid = true;
        int testint = 0;
        Int64 testlong = 0;
        DateTime testdate;
        DataSet ds;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string name;
        string npi;
        string taxid;
        string entityTypeId;
        string providerTypeId;
        string reqDate;
        string address1;
        string city;
        string state;
        string zip;
        string zipExt;
        string phone;
        string fax;
        string email;
        string title;
        int citizenship_Type_ID = 0;
        DateTime? birthDate = null;
        DateTime? deathDate = null;

        #region OrientationInfo
        ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                citizenship_Type_ID = Helper.GetInt("CITIZENSHIP_TYPE_ID", row);
                name = Helper.GetData("NAME", row);
                npi = Helper.GetData("NPI", row);
                taxid = Helper.GetData("TAX_ID", row);
                entityTypeId = Helper.GetData("ENTITY_TYPE_ID", row);
                providerTypeId = Helper.GetData("PROVIDER_TYPE_ID", row);
                DateTime tmp;
                if (DateTime.TryParse(Helper.GetDate("BIRTH_DATE", row), out tmp))
                {
                    birthDate = tmp;
                }
                if (DateTime.TryParse(Helper.GetDate("DEATH_DATE", row), out tmp))
                {
                    deathDate = tmp;
                }
                
                int regId = 0;
                int.TryParse(Helper.GetData("REG_ID", row), out regId);
                DataSet dsReg = psc.SelectRegistration(regId);
                reqDate = Helper.GetData("REQUESTED_EFFECTIVE_DATE", dsReg.Tables[0].Rows[0]);
                Boolean isPriorEffectiveDate = false;

                DateTime reg_Create_Date_Time = ObjectControllerHelper.GetDateTime("REG_CREATE_DATE_TIME", dsReg.Tables[0].Rows[0]);
                string medicaidId = Helper.GetString("MEDICAID_ID", dsReg.Tables[0].Rows[0]);
                int partyID = Helper.GetInt("Party_ID", dsReg.Tables[0].Rows[0]);
                if (reqDate != null && IsNewReg(medicaidId, partyID, entityTypeId))
                {
                    int days = Convert.ToInt32(AppSettings.Get("RetroEffectiveDateWindow"));
                    isPriorEffectiveDate = (Convert.ToDateTime(reqDate).AddDays(days) < reg_Create_Date_Time) ? true : false;
                }

                if (IsIndividualProvider(row))
                {
                    if (citizenship_Type_ID == 0)
                        isValid = AddValidationErrorMessage("* Select Citizenship Status");
                }
                if (string.IsNullOrEmpty(name))
                    isValid = AddValidationErrorMessage("* Legal Business Name is required");

                if (string.IsNullOrEmpty(taxid))
                    isValid = AddValidationErrorMessage("* Tax ID is required");
                else if (!Int64.TryParse(taxid, out testlong))
                    isValid = AddValidationErrorMessage("* Tax ID requires 9 digits");
                else if (Int64.TryParse(taxid, out testlong) && taxid.Length != 9)
                    isValid = AddValidationErrorMessage("* Tax ID requires 9 digits");

                if (string.IsNullOrEmpty(entityTypeId))
                    isValid = AddValidationErrorMessage("* Category is required");

                if (string.IsNullOrEmpty(providerTypeId))
                    isValid = AddValidationErrorMessage("* Provider Type is required");

                if (string.IsNullOrEmpty(reqDate))
                    isValid = AddValidationErrorMessage("* Requested Effective Date is required");
                else if (!DateTime.TryParse(reqDate, out testdate))
                    isValid = AddValidationErrorMessage("* Requested Effective Date is invalid");

                if (isPriorEffectiveDate) 
                {
                    // Must upload retro documentation, if selected yes.
                    DataSet dsDoc = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.Identification, null, string.Empty, null);
                    if (!Helper.HasRows(dsDoc))
                    {
                        isValid = AddValidationErrorMessage(string.Concat("*", Resources.BrandingResource.RETRO_EFFECTIVE_DATE_DOC_REQUIRED));
                    }
                }
                if (entityTypeId == CON.ProviderCategoryTypeID.Individual.ToString() && !Validate_BirthDateAfterDeathDate(birthDate, deathDate))
                {
                    isValid = AddValidationErrorMessage("* Death Date must be prior to Birth Date");
                }
                if (entityTypeId == CON.ProviderCategoryTypeID.Individual.ToString() && !Validate_BirthDateDeathDate(birthDate, deathDate))
                {
                    isValid = false;
                }


                // On an ownership change make sure they change either the NPI or Tax ID
                //if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationUpdateOwner)
                //{
                //    DataSet dsHistory = psc.SelectRegistrationAuditData(this.WorkflowPage.RegistrationId, "PROVIDER");
                //    if (Helper.HasRows(dsHistory))
                //    {
                //        bool hasChanged = false;
                //        foreach (DataRow rowHistory in dsHistory.Tables[0].Rows)
                //        {
                //            if (npi != Helper.GetString("NPI", rowHistory))
                //            {
                //                hasChanged = true;
                //                break;
                //            }
                //            else if (taxid != Helper.GetString("TAX_ID", rowHistory))
                //            {
                //                hasChanged = true;
                //                break;
                //            }
                //        }
                //        if (hasChanged)
                //        {
                //            psc.UpdateRegistrationAffiliations(this.WorkflowPage.RegistrationId);
                //        }
                //        else
                //        {
                //            isValid = AddValidationErrorMessage("* An Ownership Change requires a Tax ID and/or NPI change. " +
                //            "If you did not intend to change the ownership to obtain a new Medicaid ID, please " +
                //            "return to the Home Page and cancel the Ownership Change request.");
                //        }
                //    }
                //}
            }
        }

        #endregion

        #region Primary Contact
        if (Helper.HasRows(ds))
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                name = Helper.GetData("CONTACT_NAME", row);
                address1 = Helper.GetData("CONTACT_ADDRESS1", row);
                city = Helper.GetData("CONTACT_CITY", row);
                state = Helper.GetData("CONTACT_STATE", row);
                zip = Helper.GetData("CONTACT_ZIP",row);
                zipExt = Helper.GetData("CONTACT_EXT_ZIP", row);
                phone = Helper.GetData("CONTACT_PHONE_NUMBER", row);
                fax = Helper.GetData("CONTACT_FAX_NUMBER", row);
                email = Helper.GetData("CONTACT_EMAIL_ADDRESS", row);
                title = Helper.GetData("CONTACT_TITLE", row);

                if (string.IsNullOrEmpty(name))
                    isValid = AddValidationErrorMessage("* Name is required");

                if (string.IsNullOrEmpty(address1))
                    isValid = AddValidationErrorMessage("* Address is required");

                if (string.IsNullOrEmpty(city))
                    isValid = AddValidationErrorMessage("* City is required");

                if (string.IsNullOrEmpty(state))
                    isValid = AddValidationErrorMessage("* State is required");

                if (string.IsNullOrEmpty(zip))
                    isValid = AddValidationErrorMessage("* Zip is required");
                else if (!int.TryParse(zip, out testint) || zip.Length != 5)
                    isValid = AddValidationErrorMessage("* Enter 5 digits for the Zip (First 5)");

                if (!string.IsNullOrEmpty(zipExt))
                    if (zipExt.Length != 4 || !int.TryParse(zipExt, out testint))
                        isValid = AddValidationErrorMessage("* Enter 4 digits for the Zip (Last 4)");

                if (string.IsNullOrEmpty(phone))
                    isValid = AddValidationErrorMessage("* Phone Number is required");
                else if (phone.Length != 10 || !Int64.TryParse(Helper.StripNonNumerics(phone), out testlong))
                    isValid = AddValidationErrorMessage("* Phone number is invalid");

                if (!string.IsNullOrEmpty(fax))
                    if (fax.Length != 10 || !Int64.TryParse(Helper.StripNonNumerics(fax), out testlong))
                        isValid = AddValidationErrorMessage("* Fax number is invalid");

                if (string.IsNullOrEmpty(email))
                    isValid = AddValidationErrorMessage("* Email Address is required");
                else if (!Helper.IsValidEmail(email))
                    isValid = AddValidationErrorMessage("* Email Address is invalid");
                //bug 6544 - changes not showing title field to nfocus provider
                if (!this.WorkflowPage.IsWaiverServiceProvider && string.IsNullOrEmpty(title))
                {
                    isValid = AddValidationErrorMessage("* Title is required");
                }
            }
        }
        #endregion

        return isValid;
    }

    private bool IsNewReg(string medicaidId, int partyID, string entityTypeId)
    {
        return string.IsNullOrEmpty(medicaidId) && partyID == 0 && Convert.ToInt32(entityTypeId) != CON.ProviderCategoryTypeID.GroupMemberProfile;
    }

    private bool Validate_BirthDateDeathDate(DateTime? birthDate, DateTime? deathDate)
    {
        bool isValid = true;

        if (birthDate.HasValue)
        {
            if (birthDate.Value < DateTime.Now.AddYears(-100) || birthDate.Value > DateTime.Now)
            {
                isValid = false;
                AddValidationErrorMessage("* Birth Date can not be a future date and cannot result in an age over 100 years.");
            }

        }
        if (deathDate.HasValue)
        {
            if (deathDate.Value > DateTime.Now)
            {
                isValid = false;
                AddValidationErrorMessage("Death Date can not be a future date.");
            }
        }


        return isValid;
    }
    private bool Validate_BirthDateAfterDeathDate(DateTime? birthDate, DateTime? deathDate)
    {
        bool isValid = true;

        if (deathDate.HasValue && birthDate.HasValue)
        {
            if (deathDate.Value < birthDate.Value)
            {
                isValid = false;
            }
        }

        return isValid;
    }


    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }

    private void SetButtons(string commandName)
    {
        // Save is disabled by default
        btnSave.Visible = false;
        btnCancel.Text = "Close";

        if (CanEdit)
        {
            btnSave.Visible = true;
            btnCancel.Text = "Cancel";
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr;

        SetButtons(e.CommandName);
        switch (e.CommandName)
        {
            case "OrientationInfo":
                lblTitle.Text = "Orientation Information";
                btnSave.ValidationGroup = "valOrientationInfo";
                //dr = ucOrientationInfo.DataList.Rows[index];
                dr = dtOrientation.Rows[index];
                ucOrientationInfo.LoadData(dr);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
            default:
                break;
        }
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        SetButtons(e.CommandName);
        switch (e.CommandName)
        {
            case "OrientationInfo":
                lblTitle.Text = "Organization Information";
                btnSave.ValidationGroup = "valOrientationInfo";
                ucOrientationInfo.LoadData(null);
                mltPopup.ActiveViewIndex = 0;
                mpe.Show();
                break;
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        SetButtons("History");
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER_History");
        DataTable dt = null;
        if (Helper.HasRows(ds)) dt = ds.Tables[0];

        switch (e.CommandName)
        {
            case "OrientationInfo":
                lblTitle.Text = "Organization Information History";
                mltPopup.ActiveViewIndex = 1;
                ucOrientationInfoHistory.LoadData(dt);
                mpe.Show();
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

        if (mltPopup.ActiveViewIndex == 0)
        {
            if (!ucOrientationInfo.ValidateData())
            {
                btnSave.Enabled = true;
                mpe.Show();
                return;
            }
            ucOrientationInfo.SaveData();
            // Only invalidate if NOT provider services
            if (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName)) 
                InvalidateAgreements(this, new EventArgs());
            
            LoadOrientationInfo();
        }
        btnSave.Enabled = true;
        SetTakeActionVisibility();
    }

    protected void grdOrientationInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[2].Text == "9999999999") e.Row.Cells[2].Text = string.Empty;
        }
    }

    private void SetTakeActionVisibility()
    {
        bool isTakeActionVisible = false;
        isTakeActionVisible = true;

        if (this._toggleTakeActionVisibility != null)
        {
            this._toggleTakeActionVisibility(new ToggleTakeActionVisibilityEventArgs(isTakeActionVisible));
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        DataTable tbl = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION");
        if (Helper.HasRows(ds)) tbl = ds.Tables[0];
        LoadOrientationInfo();
    }

    public override string ValidationGroup
    {
        get { return "valOrientation"; }
    }

    public override string Title
    {
        get { return "Orientation Screening"; }
    }

    public override string IdText
    {
        get { return "ucOrientation_" + this.WorkflowPage.RegistrationId; }
    }

}