using MathNet.Numerics.LinearAlgebra.Factorization;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OrgInfo : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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

    public delegate void ReloadPopupEventHandler();
    public event ReloadPopupEventHandler ReloadPopupEvent;
    public bool OwnershipChangedFlag { get; set; }

    public string MedicaidId
    {
        get
        {
            if (ViewState["MedicaidId"] == null) ViewState["MedicaidId"] = string.Empty;
            return ViewState["MedicaidId"].ToString();
        }
        set { ViewState["MedicaidId"] = value; }
    }

    public int RegProgramStatusTypeID
    {
        get
        {
            return ViewState["RegProgramStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegProgramStatusTypeID"]);
        }
        set
        {
            ViewState["RegProgramStatusTypeID"] = value;
        }
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

    public string UserAccount_TaxonomyTypeID
    {
        get
        {
            if (ViewState["UserAccount_TaxonomyTypeID"] == null) ViewState["UserAccount_TaxonomyTypeID"] = string.Empty;
            return ViewState["UserAccount_TaxonomyTypeID"].ToString();
        }
        set { ViewState["UserAccount_TaxonomyTypeID"] = value; }
    }

    public string UserAccount_SpecialtyTypeID
    {
        get
        {
            if (ViewState["UserAccount_SpecialtyTypeID"] == null) ViewState["UserAccount_SpecialtyTypeID"] = string.Empty;
            return ViewState["UserAccount_SpecialtyTypeID"].ToString();
        }
        set { ViewState["UserAccount_SpecialtyTypeID"] = value; }
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
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        this.LoadProviderTypes(psc);
        this.LoadProviderGenders(psc);
        // TODO: EDV put the note types in cache
        DataSet ds = psc.GetTypeofPractice();
        DataTable dtPractice = ds.Tables[0];
        if (Convert.ToInt32(hdnProviderCategoryId.Value) == CON.ProviderCategoryTypeID.Pharmacy)
        {
            dtPractice = FilterPracticeTypes(ds, Convert.ToInt32(hdnProviderCategoryId.Value));
        }
        // DCPDMS-2853 - make type of practice not visible all the time, although it is still used for saving the provider info.
        ddlTypeofPractice.Visible = false;
        lblTypeofPractice.Visible = false;
        Helper.LoadList(ddlTypeofPractice, dtPractice, "TYPE_OF_PRACTICE_NAME", "TYPE_OF_PRACTICE_ID", true);

        // TODO: EDV put the note types in cache
        ds = psc.GetTermReason();
        Helper.LoadList(ddlTerminationReason, ds.Tables["TermReason"], "TERM_REASON_NAME", "TERM_REASON_ID", true);

        // TODO: EDV put the note types in cache
        ds = psc.GetTaxIdType();
        Helper.LoadList(ddlTaxIdType, ds.Tables["TaxIdType"], "TAX_ID_TYPE", "TAX_ID_TYPE_ID", true);


        ds = psc.GetOwnershipTypes();
        Helper.LoadList(ddlOwnershiptype, ds.Tables["OwnershipTypes"], "DESC_OWNERSHIP_TYPE", "OWNERSHIP_TYPE_ID", true);

        ds = psc.GetPracticeTypes();
        Helper.LoadList(ddlPracticeType, ds.Tables["PracticeTypes"], "Practice_Type_Desc", "Practice_Type_ID", true);


        ds = psc.SelectReferenceDataWithoutParam("usp_selectProviderTitles");
        Helper.LoadList(ddlTitle, ds.Tables[0], "title_desc", "title_id", true);

        ds = psc.GetCountryCodes();
        Helper.LoadList(ddlBirthCountry, ds.Tables[0], "COUNTRY_DESC", "COUNTRY_CODE", true);

       
    }

    private DataTable FilterPracticeTypes(DataSet practiceTypes, int categoryTypeID)
    {
        StringBuilder selectPart = new StringBuilder();
        //Filter by category
        selectPart.Append(string.Format("PROVIDER_CATEGORY_TYPE_ID = '{0}'", categoryTypeID));

        DataTable dt = practiceTypes.Tables[0];
        if (dt.Select(selectPart.ToString()).Count() > 0)
        {
            return dt.Select(selectPart.ToString()).CopyToDataTable();
        }
        return dt;
    }

    private void LoadProviderGenders(PDMSService.PDMSServiceClient psc)
    {
        DataSet ds = null;
        //select All Provider Gender
        if (ddlGender.Items.Count == 0)
        {
            ds = psc.SelectReferenceDataWithoutParam("usp_SelectPROVIDER_GENDER");
            var dsForGender = ds.Tables[0].AsEnumerable()
              .Where(r => r.Field<string>("PROVIDER_GENDER_INITIAL") != "B").CopyToDataTable();
            if (Helper.HasRows(dsForGender))
            {
                Helper.LoadList(ddlGender, dsForGender, "PROVIDER_GENDER_NAME", "PROVIDER_GENDER_INITIAL", false);
            }
        }
    }

    private void LoadProviderTypes(PDMSService.PDMSServiceClient psc)
    {
        try
        {
            DataSet dsType = null;

            if (!string.IsNullOrEmpty(hdnProviderCategoryId.Value))
            {
                dsType = psc.GetProviderTypesByTypeId(Convert.ToInt32(hdnApplicationTypeId.Value), Convert.ToInt32(hdnProviderCategoryId.Value), Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
            }

            if (Helper.HasRows(dsType))
            {
                dsType.Tables[0].Columns.Add("IdWithName", typeof(string), "MMIS_PROVIDER_TYPE_ID + ' - ' + PROVIDER_TYPE_NAME");

                DataTable dt = dsType.Tables["ProviderType"];
                Helper.LoadList(ddlProviderType, dt, "IdWithName", "PROVIDER_TYPE_ID", true);

                DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
                if (Helper.HasRows(ds))
                {
                    // ddlProviderType.SelectedItem.Value = Helper.GetString("PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]);
                    ddlProviderType.SelectedItem.Text = Helper.GetString("MMIS_PROVIDER_TYPE_ID", ds.Tables[0].Rows[0]).Trim() + " - " + Helper.GetString("PROVIDER_TYPE_NAME", ds.Tables[0].Rows[0]).Trim();
                }

            }
            else
            {
                ddlProviderType.DataSource = dsType;
                ddlProviderType.DataBind();
            }
        }
        catch { }
    }
    public bool IsDBAVisible()
    {
       
        if (((this.WorkflowPage.ApplicationTypeID == CON.ApplicationType.Waiver) && (this.WorkflowPage.EntityTypeID == CON.ProviderCategoryTypeID.Individual)) &&
            (this.WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NON_AGENCYPERSONALCAREAIDE.ToString()) &&
            (this.WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NON_AGENCYHOMECAREATTENDANT.ToString()) &&
            (this.WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.WAIVEREDSERVICESINDIVIDUAL.ToString())
            )
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void LoadOrgInfo()
    {
        divDBA.Visible = IsDBAVisible();
        this.DataList = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        if (Helper.HasRows(ds)) this.DataList = ds.Tables[0];

        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[0]);
        else
            this.LoadData(null);
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Provider Information";

        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
    }
    public override void LoadControlData()
    {
        LoadOrgInfo();
    }

    public override void LoadData(DataRow dr)
    {
        //Need to split this out into Get Data, Load Data, Set Editability and Set Visibility methods.
        LoadHelpText();
        int diddReferralId = 0;
        int entityTypeID = 0;
        nbNPI.Text = nbTaxID.Text = nbProviderNumber.Text = txtNPIStartDate.Text = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        this.trTermDate.Visible = false;
        this.trTermReason.Visible = false;
        Helper.SetReadOnly(this, false);


        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }

        if (dr != null)
        {
            entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
            hdnProviderCategoryId.Value = entityTypeID.ToString();
            hdnApplicationTypeId.Value = Helper.GetInt("APPLICATION_TYPE_ID", dr).ToString();

            IndividualProviderType = IsIndividualProvider(dr);
            LoadDropDowns();

            NPI = nbNPI.Text = Helper.GetString("NPI", dr);

            RegProgramStatusTypeID = Helper.GetInt("REG_PROGRAM_STATUS_TYPE_ID", dr);
            InProviderDataEntry = Helper.GetBool("InProviderDataEntry", dr);

            //all 9's for npi is a TN check
            nbNPI.Text = NPI == "9999999999" ? string.Empty : NPI;
            if (Helper.GetDate("NPI_START_DATE", dr).ToString() == "" || Helper.GetDate("NPI_START_DATE", dr).ToString() == null)

            {
                bool isNPIAPIEnabled = AppSettings.Get("NPI-Registry-Enabled").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false;

                if (isNPIAPIEnabled && !string.IsNullOrWhiteSpace((NPI)))
                {
                    MAXIMUS.Core.Libraries.NPPESAPIResult result = psc.ValidNPIinNPPESApi(Convert.ToInt64(NPI));

                    if (result.result_count > 0)
                    {
                        txtNPIStartDate.Text = DateTime.ParseExact(result.results[0].basic.enumeration_date, "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                }

            }
            else
            {
                txtNPIStartDate.Text = Helper.GetDate("NPI_START_DATE", dr).ToString();

            }

            // For All
            nbTaxID.Text = Helper.GetString("TAX_ID", dr);
            if (!string.IsNullOrEmpty(Helper.GetString("TERM_DATE", dr)))
            {
                lblTermDate.Text = Helper.GetDate("TERM_DATE", dr);
                ddlTerminationReason.SelectedValue = Helper.GetString("TERM_REASON_ID", dr);
                this.trTermDate.Visible = true;
            }
            if (!string.IsNullOrEmpty(Helper.GetString("PROVIDER_TYPE_ID", dr)))
            {
                ddlProviderType.SelectedItem.Value = Helper.GetString("PROVIDER_TYPE_ID", dr);
            }
            string mmisProviderTypeID = this.WorkflowPage.MMISProviderTypeID;
            if ((mmisProviderTypeID == CON.ProviderTypeNumerics.NURSING_FACILITY.ToString()
                || mmisProviderTypeID == CON.ProviderTypeNumerics.NONSTATE_OPERATED_ICF_MR.ToString()
                || mmisProviderTypeID == CON.ProviderTypeNumerics.STATE_OPERATED_ICF_MR.ToString())
                && this.WorkflowPage.WorkflowEventTypeId != 1 && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            {
                txtDBA.Enabled = false;
            }
            if (mmisProviderTypeID == CON.ProviderTypeNumerics.NURSING_FACILITY.ToString() 
                || mmisProviderTypeID == CON.ProviderTypeNumerics.NONSTATE_OPERATED_ICF_MR.ToString())
            {
                lblDBA.Text = "DBA*";
            }
            else
            {
                lblDBA.Text = "DBA";
            }            

                if (ddlTypeofPractice.Visible)
            {
                ddlTypeofPractice.SelectedValue = Helper.GetString("TYPE_OF_PRACTICE_ID", dr);
            }
            string statusCode = Helper.GetString("ENROLL_STATUS_DESC", dr);
            this.lblEnrollmentStatus.Text = string.IsNullOrEmpty(statusCode) ? "Not Set Yet" : statusCode;
            this.lblEnrollmentStatusReason.Text = string.IsNullOrEmpty(Helper.GetString("ENROLLMENT_STATUS_REASON", dr)) ? "Not Set Yet" : Helper.GetString("ENROLLMENT_STATUS_REASON", dr);
            ddlOwnershiptype.SelectedValue = Helper.GetString("OWNERSHIP_TYPE_ID", dr);
            ddlPracticeType.SelectedValue = Helper.GetString("PRACTICE_TYPE_ID", dr);
            if (!dr.IsNull("IS_OHIO_RESIDENT"))
            {
                rblIsOhioResident.SelectedValue = Helper.GetBool("IS_OHIO_RESIDENT", dr) ? "1" : "0";
            }
            txtBirthCity.Text = Helper.GetString("BIRTH_CITY", dr);

            if (CON.ProviderCategoryTypeID.Individual.ToString()== Helper.GetString("PROVIDER_CATEGORY_TYPE_ID", dr))
            { 
                this.txtCaqh.Text = Helper.GetString("CAQH", dr);
            } 
            else 
            {
                this.txtCaqh.Visible = false;
                this.LabelCaqh.Visible = false;
            }

            txtBirthState.Text = Helper.GetString("BIRTH_STATE", dr);
            string birthCountryCode = Helper.GetString("BIRTH_COUNTRY", dr);
            if(!string.IsNullOrEmpty(birthCountryCode.Trim()))
            {
                if(ddlBirthCountry.Items.FindByValue(birthCountryCode.Trim())!=null)
                    ddlBirthCountry.SelectedValue = birthCountryCode;
            }

            if (CON.MMISProviderType.Franchise_Fee_Only_Non_Medicaid_Provider == Helper.GetString("MMIS_PROVIDER_TYPE_ID", dr))
            {
                this.nbOdhNumber.Text = Helper.GetString("ODH_NUMBER", dr);
            }
            else
            {
                this.trOdhNumber.Visible = false;
            }

            diddReferralId = Helper.GetInt("DIDD_REFERRAL_ID", dr);
            MedicaidId = nbProviderNumber.Text = Helper.GetString("MEDICAID_ID", dr);

            //general enable/disable based on if user can edit - need to know if in provider data entry, so set after get records
            Helper.SetReadOnly(this, !Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName, this.WorkflowPage.CommandName));

            this.nbTaxID.ReadOnly = true;
            nbTaxID.TabIndex = 0;
            this.nbNPI.ReadOnly = true;
            nbNPI.TabIndex = 0;

            Helper.SetReadOnly(ddlProviderType, true);
            Helper.SetReadOnly(trProviderNumber, true);
            Helper.SetReadOnly(txtLegalBusinessName, true);
            Helper.SetReadOnly(txtFirstName, true) ;
            Helper.SetReadOnly(txtMiddleInitial, true);
            Helper.SetReadOnly(txtLastName, true);
            Helper.SetReadOnly(trTaxIdType, true);
            Helper.SetReadOnly(trNPIStartDate, true);
            Helper.SetReadOnly(trNPI, true);
            Helper.SetReadOnly(trGender, true);
            Helper.SetReadOnly(trRevalidationDate, true);




            nbProviderNumber.Enabled = false;
            nbProviderNumber.CssClass = "formFieldReadOnly";
            nbProviderNumber.TabIndex = -1;

            bool showReqEffDt = entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile && IsNewReg();
            //Always display effective date for non-group member profiles, for existing providers.
            bool showChgEffDt = (entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile && (!IsNewReg())) ||
                (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name));

            

            string revalDate = Helper.GetString("END_DATE", dr);

            txtRevalidationDate.Text = string.IsNullOrEmpty(revalDate) ? "Not Set Yet" : Helper.FormatDate2(revalDate);

            txtLegalBusinessName.Text = Helper.GetString("NAME", dr);
            hdnProviderNameChange.Value = Helper.GetString("NAME", dr);
            divCountryDetails.Visible = IndividualProviderType;

            divDBA.Visible = (WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NON_AGENCYPERSONALCAREAIDE.ToString() || WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NON_AGENCYHOMECAREATTENDANT.ToString()
                || WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.WAIVEREDSERVICESINDIVIDUAL.ToString()) ? false : true;

            // Only for non-individuals
            if (IsDBAVisible())
            {
                txtDBA.Text = Helper.GetString("DBA", dr);
            }
           

            //Waiver Services unique field - informational only.
            trEntityType.Visible = this.WorkflowPage.IsWaiverServiceProvider;
            Helper.SetReadOnly(trEntityType, true); //always readonly

            if (this.WorkflowPage.IsWaiverServiceProvider)
            {
                this.rblEntityType.SelectedIndex = IndividualProviderType ? 0 : 1;
            }

            trNPI.Visible = !this.WorkflowPage.IsWaiverServiceProvider;
            trNPIStartDate.Visible = !this.WorkflowPage.IsWaiverServiceProvider;
            trTaxIdType.Visible = this.WorkflowPage.IsWaiverServiceProvider;  //for now, only visible for waiver services.
            if (ddlTaxIdType.Items.FindByValue(Helper.GetString("TAX_ID_TYPE_ID", dr)) != null)
            {
                ddlTaxIdType.SelectedValue = Helper.GetString("TAX_ID_TYPE_ID", dr);
            }
            Helper.SetReadOnly(trTaxIdType, true); //always readonly

            // Sole Proprietor or GMP
            if (IndividualProviderType)
            {
                //trCitizenAlien.Visible = entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile;
                txtFirstName.Text = Helper.GetString("FIRST_NAME", dr);
                txtLastName.Text = Helper.GetString("LAST_NAME", dr);
                txtMiddleInitial.Text = Helper.GetString("MIDDLE_INITIAL", dr);

                if ((!string.IsNullOrEmpty(Helper.GetString("TITLE", dr)) && !string.IsNullOrWhiteSpace(Helper.GetString("TITLE", dr))))
                {
                    ddlTitle.SelectedValue = Helper.GetString("TITLE", dr);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("GENDER", dr)))
                {
                    ddlGender.SelectedValue = Helper.GetString("GENDER", dr);
                }
                if (!string.IsNullOrEmpty(Helper.GetDate("BIRTH_DATE", dr)))
                {
                    txtBirthDate.Text = Helper.GetDate("BIRTH_DATE", dr);
                }




                trFirstName.Visible = trLastName.Visible = trMiddleInt.Visible = trTitle.Visible = trGender.Visible = trBirthDate.Visible = true;
                trRevalidationDate.Visible = entityTypeID != CON.ProviderCategoryTypeID.GroupMemberProfile;

            }
            else
            {
                lblOhioResident.Visible = false;
                rblIsOhioResident.Visible = false;
                trFirstName.Visible = trLastName.Visible = trMiddleInt.Visible = trTitle.Visible = trGender.Visible = trBirthDate.Visible = false;
            }

            // OHPNM-1917
            if (inMaintenance(this.WorkflowPage.RegistrationId))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
            //OHPNM-9104
            int stepid = this.WorkflowPage.WF_StepID;
            string TaskName = string.Empty;
            DataSet dsStep = psc.WF_SelectStepInfo(stepid);
            if (dsStep.Tables.Count > 0 && dsStep.Tables[0].Rows.Count > 0)
            {
                TaskName = dsStep.Tables[0].Rows[0]["TASK_NAME"].ToString();

                if (TaskName == CON.RegistrationTaskName.ProviderReview
                    && Methods.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))
                {
                    Helper.SetReadOnly(ddlPracticeType, false);
                    Helper.SetReadOnly(ddlOwnershiptype, false);
                }
            }
        }


        //not sure what the business logic is for this for NE, why hide this from the provider?
        trProviderNumber.Visible = false;




        //bug6544
        if (this.WorkflowPage.IsWaiverServiceProvider)
        {
            ddlTitle.Visible = lblTitle.Visible = trTitle.Visible = false;
        }

    }

    private void LoadHelpText()
    {
        if (Helper.IsModern())
        {
            txtLegalBusinessName.Attributes.Add("data-content", "Business Name as it appears on your IRS assignment letter");

            txtLegalBusinessName.Attributes.Add("placeholder", "Business Name as on your IRS Assignment letter");
            txtLegalBusinessName.Attributes.Add("data-toggle", "popover");
            txtLegalBusinessName.Attributes.Add("data-placement", "right");
            txtLegalBusinessName.Attributes.Add("data-trigger", "focus");
            trBusinessHelpText.Visible = false;
        }
        else
        {
            trBusinessHelpText.Visible = true;
        }
    }

    private bool IsExternalUser()
    {
        return true;
    }

    private bool IsInternalUser()
    {
        return true;
    }

    private bool IsNewConvertedProvider(int partyID)
    {
        //reg program status of conversion AND no party id
        return Registration.IsConversionProvider(RegProgramStatusTypeID) && partyID <= 0;
    }

    // Todo This is only temporary, will be moved to a common file
    private bool IsIndividualProvider(DataRow dr)
    {
        int entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", dr);
        return entityTypeID == CON.ProviderCategoryTypeID.Individual || entityTypeID == CON.ProviderCategoryTypeID.GroupMemberProfile;
    }

    private bool IsNewReg()
    {
        return string.IsNullOrEmpty(MedicaidId);
    }

    public override bool SaveData()
    {
        if (!ValidateData())
        {
            return false;
        }
        if ((this.WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NURSING_FACILITY.ToString() 
            || this.WorkflowPage.MMISProviderTypeID == CON.ProviderTypeNumerics.NONSTATE_OPERATED_ICF_MR.ToString()) 
            && string.IsNullOrEmpty(txtDBA.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Enter DBA ";
            val.ValidationGroup = "valOrgInfo";
            this.Page.Validators.Add(val);
            return false;
        }

        Page.Validate("valOrgInfo");
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valOrgInfo") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        if (!Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            return true;
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        Dictionary<string, string> parmsPaper = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        bool doUpdate = Helper.HasRows(ds);
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parmsPaper.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        // Provider Services is reviewing and they need to update the Change Effective Date and the Medicaid ID
        if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName))
        {

            if (MedicaidId != nbProviderNumber.Text)
                psc.UpdatePDMSDataAdminReg(this.WorkflowPage.RegistrationId, nbProviderNumber.Text,
                    Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        }

        string mName = string.IsNullOrEmpty(txtMiddleInitial.Text.Trim()) ? "" : txtMiddleInitial.Text.Trim() + " ";

        string fullName = IndividualProviderType ? txtFirstName.Text.Trim() + " " + mName + txtLastName.Text.Trim() : txtLegalBusinessName.Text.Trim();
        if (Convert.ToInt32(hdnProviderCategoryId.Value) > 0 && Convert.ToInt32(ddlProviderType.SelectedValue) > 0)
        {
            parms.Add("NAME", fullName);
            parmsPaper.Add("ENTITY_NAME", fullName);
            // Only for non-individuals
            if (IsDBAVisible())
            {
                parms.Add("DBA", txtDBA.Text);
                parmsPaper.Add("DBA_NAME", txtDBA.Text);
            }

            if (string.IsNullOrWhiteSpace(nbNPI.Text) && NPI == "9999999999") nbNPI.Text = NPI;
            parms.Add("NPI", nbNPI.Text); parmsPaper.Add("NPI", nbNPI.Text);
            parms.Add("NPI_START_DATE", txtNPIStartDate.Text);
            parms.Add("TAX_ID", nbTaxID.Text);
            if (ddlTaxIdType.SelectedValue == "15")
                parmsPaper.Add("SSN", nbTaxID.Text);
            else
                parmsPaper.Add("TAX_ID", nbTaxID.Text);

            if (MedicaidId != null)
                parmsPaper.Add("MEDICAID_ID", MedicaidId);

            parms.Add("ENTITY_TYPE_ID", hdnProviderCategoryId.Value);
            parms.Add("PROVIDER_TYPE_ID", ddlProviderType.SelectedValue);
            if (!hdnProviderNameChange.Value.Equals(fullName))
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            }
            else
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            }


            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parmsPaper.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parmsPaper.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            // Only for individual provider
            if (IsIndividualProvider(ds.Tables[0].Rows[0]))
            {
                parms.Add("FIRST_NAME", txtFirstName.Text.Trim());
                parms.Add("LAST_NAME", txtLastName.Text.Trim());
                parms.Add("MIDDLE_INITIAL", txtMiddleInitial.Text.Trim());
                parms.Add("TITLE", ddlTitle.SelectedValue.Trim());
                parms.Add("GENDER", ddlGender.SelectedValue);
                if (!string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
                {
                    parms.Add("BIRTH_DATE", txtBirthDate.Text);
                    parmsPaper.Add("BIRTH_DATE", txtBirthDate.Text);

                }

                parms.Add("TAX_ID_TYPE_ID", ddlTaxIdType.SelectedValue);

            }
            parms.Add("TYPE_OF_PRACTICE_ID", ddlTypeofPractice.SelectedValue);
            if (txtRevalidationDate.Enabled && !string.IsNullOrEmpty(txtRevalidationDate.Text))
            {

                //updateReg = true;
                parms.Add("END_DATE", txtRevalidationDate.Text);
            }
        }



        if (chkMTMed.Checked)
            parms.Add("MTMED_ENROLLED", "1");
        else
            parms.Add("MTMED_ENROLLED", "0");

        if (chkMTChip.Checked)
            parms.Add("MTCHIP_ENROLLED", "1");
        else
            parms.Add("MTCHIP_ENROLLED", "0");

        if (chkMTboth.Checked)
            parms.Add("MTBOTH_ENROLLED", "1");
        else
            parms.Add("MTBOTH_ENROLLED", "0");

        parms.Add("PRACTICE_TYPE_ID", ddlPracticeType.SelectedValue);
        parms.Add("OWNERSHIP_TYPE_ID", ddlOwnershiptype.SelectedValue);
        parms.Add("BIRTH_COUNTRY", ddlBirthCountry.SelectedValue);
        parms.Add("BIRTH_STATE", txtBirthState.Text);
        parms.Add("BIRTH_CITY", txtBirthCity.Text);
        parms.Add("IS_OHIO_RESIDENT", rblIsOhioResident.SelectedValue);
        parms.Add("CAQH", txtCaqh.Text.Trim());

        //parms.Add("ENROLLMENT_STATUS_REASON", lblEnrollmentStatusReason.Text);
        if (doUpdate)
        {
            // Update
            psc.UpdateRegistrationDataTable("PROVIDERCustom", parms);

            DataSet dsPaper = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER_PAPER_PROVIDER");

            if (Helper.HasRows(dsPaper))
            {
                DataRow drpaper = dsPaper.Tables[0].Rows[0];
                string paperID = Helper.GetString("REG_OWNER_PAPER_PROVIDER_ID", drpaper);
                parmsPaper.Add("REG_OWNER_PAPER_PROVIDER_ID", paperID);
                psc.UpdateRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);
            }
            else
            {
                parmsPaper.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parmsPaper.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                psc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);
            }
        }
        else
        {
            // Insert
            parmsPaper.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parmsPaper.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("PROVIDERCustom", parms);
            psc.InsertRegistrationDataTable("OWNER_PAPER_PROVIDER", parmsPaper);
            //insert the primary specialty, if provided during account creation
            if (!string.IsNullOrEmpty(UserAccount_TaxonomyTypeID) && !string.IsNullOrEmpty(UserAccount_SpecialtyTypeID))
            {
                int taxonomyTypeID = Convert.ToInt32(UserAccount_TaxonomyTypeID);
                int specialtyTypeID = Convert.ToInt32(UserAccount_SpecialtyTypeID);
                DataSet dsExistingSpec = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SPECIALTY");
                if (!Helper.HasRows(dsExistingSpec))
                {
                    bool rtn = Registration.InsertAccountCreationSpecialtyTaxonomy(this.WorkflowPage.RegistrationId, taxonomyTypeID, specialtyTypeID);
                }
            }
        }

        DataRow dr = ds.Tables[0].Rows[0];

        if (CON.MMISProviderType.Franchise_Fee_Only_Non_Medicaid_Provider == Helper.GetString("MMIS_PROVIDER_TYPE_ID", dr))
        {
            string OdhNumber = Helper.GetString("ODH_NUMBER", dr);
            if (string.IsNullOrEmpty(OdhNumber) || nbOdhNumber.Text != OdhNumber)
            {
                DateTime npiStartDate = DateTime.Now;
                if (!DateTime.TryParse(txtNPIStartDate.Text, out npiStartDate)) { }
                psc.InsertUpdateODHFacilityHomeNumber(this.WorkflowPage.RegistrationId, npiStartDate, Convert.ToInt32(this.nbOdhNumber.Text), Helper.GetUserId(HttpContext.Current.User.Identity.Name));
            }
        }
        


        bool updateReg = false;

        parms = new Dictionary<string, string>();

        if (updateReg)
        {
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            psc.UpdateRegistration(parms);
        }

        return true;
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valOrgInfo";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && ddlTypeofPractice.Visible && ddlTypeofPractice.SelectedIndex <= 0)   // Must be a Provider
        {
                AddError("* Enter Type of Practice", ref isGood);
        }
        
        //death date cannot be prior to birthdate
        Validate_BirthDateAfterDeathDate(ref isGood);
        Validate_BirthDateDeathDate(ref isGood);

        int entityTypeID = 0;

        // Get REG_PROVIDER data
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet dsProvider = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        if (Helper.HasRows(dsProvider))
        {
            DataRow row = dsProvider.Tables[0].Rows[0];
            entityTypeID = Helper.GetInt("ENTITY_TYPE_ID", row);
        }

        if (entityTypeID == 0 || hdnProviderCategoryId.Value == "")
            AddError("* Category is required", ref isGood);
        if (ddlProviderType.SelectedValue == "")
            AddError("* Provider Type is required", ref isGood);

        if (MtDentalPrgms.Visible)
        {
            if (!chkMTMed.Checked && !chkMTChip.Checked && !chkMTboth.Checked)
                AddError("* You should at least enroll for one Program Type.", ref isGood);

            if ((chkMTMed.Checked && chkMTChip.Checked) || (chkMTChip.Checked && chkMTboth.Checked) || (chkMTMed.Checked && chkMTboth.Checked) ||
                (chkMTMed.Checked && chkMTChip.Checked && chkMTboth.Checked))
                AddError("* You should enroll to only one Program Type.", ref isGood);

        }
        string NPIErrorMessage = string.Empty;
        if (!Registration.IsValidNppesNpi(this.WorkflowPage.RegistrationId, ref NPIErrorMessage)
            && Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            AddError("* " + NPIErrorMessage, ref isGood);
        }
        if (IndividualProviderType && string.IsNullOrEmpty(rblIsOhioResident.SelectedValue))
        {
            AddError("You should answer about residency of state.", ref isGood);
        }
        return isGood;
    }
    private void Validate_BirthDateAfterDeathDate(ref bool isGood)
    {
        DateTime? birthDate = null;
        //DateTime? deathDate = null;

        if (string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
        {
            return;
        }

        DateTime tmp;
        if (DateTime.TryParse(this.txtBirthDate.Text.Trim(), out tmp))
        {
            birthDate = tmp;
        }

        if (!birthDate.HasValue)
        {
            //date parsing failed on one or both values
            AddError("Invalid birth date", ref isGood);
        }

        return;
    }

    private void Validate_BirthDateDeathDate(ref bool isGood)
    {
        DateTime? birthDate = null;

        if (string.IsNullOrEmpty(txtBirthDate.Text.Trim()))
        {
            return;
        }

        DateTime tmp;
        if (DateTime.TryParse(this.txtBirthDate.Text.Trim(), out tmp))
        {
            birthDate = tmp;
        }

        if (birthDate.HasValue)
        {
            if (birthDate.Value < DateTime.Now.AddYears(-100) || birthDate.Value > DateTime.Now)
            {
                AddError("* Birth Date can not be a future date and cannot result in an age over 100 years.", ref isGood);
            }

        }



        return;
    }

    private bool IsNewReg(string medicaidId, int entityTypeId)
    {
        return string.IsNullOrEmpty(medicaidId) && entityTypeId != CON.ProviderCategoryTypeID.GroupMemberProfile;
    }



    protected void chkCSHN_CheckedChanged(object sender, EventArgs e)
    {
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
        {

            if (MtDentalPrgms.Visible)
            {
                if (chkMTMed.Checked || chkMTboth.Checked)
                {
                    txtMTEndDate1.Enabled = true;
                    txtMTEndDate1.CssClass = "formField";
                    reqMTEndDate1.Enabled = true;
                    cmpMTEndDate1.Enabled = true;
                }
                if (chkMTChip.Checked || chkMTboth.Checked)
                {
                    txtMTEndDate2.Enabled = true;
                    txtMTEndDate2.CssClass = "formField";
                    reqMTEndDate2.Enabled = true;
                    cmpMTEndDate2.Enabled = true;
                }
            }
        }

    }



    public override string ValidationGroup
    {
        get { return "valOrgInfo"; }
    }

    public override string Title
    {
        get { return "Provider Information"; }
    }

    public override string IdText
    {
        get { return "ucOrgInfo_" + this.WorkflowPage.RegistrationId; }
    }
}