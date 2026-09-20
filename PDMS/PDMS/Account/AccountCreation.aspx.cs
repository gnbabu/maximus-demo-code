using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Account_AccountCreation : System.Web.UI.Page
{
    public static class ConvertedProviderType
    {
        public static int IndividualOrGroupMemberProfile = 1;
        public static int NFOCUS = 2;
        public static int Group = 3;
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

    #region Properties
    private int cancelProgress;

    private bool AdditionalInfoAdded = false;
    private bool EmailSent = false;

    private bool ConvertedProviderFound
    {
        get
        {
            return ViewState["ConvertedProviderFound"] == null ? false : Convert.ToBoolean(ViewState["ConvertedProviderFound"]);
        }
        set
        {
            ViewState["ConvertedProviderFound"] = value;
        }
    }
    private int ConvertedTaxonomyTypeID
    {
        get
        {
            return ViewState["ConvertedTaxonomyTypeID"] == null ? 0 : Convert.ToInt32(ViewState["ConvertedTaxonomyTypeID"]);
        }
        set
        {
            ViewState["ConvertedTaxonomyTypeID"] = value;
        }
    }
     
    #endregion

    #region Page Events
    protected override void OnInit(EventArgs e)
    {
        TextBox txt = (TextBox)wizStepProfileInfo.ContentTemplateContainer.FindControl("Question");
        if (txt != null) txt.Attributes.CssStyle.Add("visibility", "hidden");
        base.OnInit(e);
    }

    void Page_LoadComplete(object sender, EventArgs e)
    {
        SetupButtonDisablesOfMultiClick();
    }

    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            Page.Theme = "Modernization";
        }
        else
        {
            Page.Theme = "Default";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Disable conversion xtra entry field validators
            EnableExistingProviderValidators(false, false, false, false);
            DisplayExistingProviderFields(false, false, false, false);
            // Load Security Questions
            Registration.SecurityQuestions(ref ddlPasswordQuestion1);
            Registration.SecurityQuestions(ref ddlPasswordQuestion2);
        }

    }
    
    protected void wizCreateUser_CreatedUser(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        // Success!!! aspnet_User was created now add additional data elements needed
        MembershipUser  user = Membership.GetUser(wizCreateUser.UserName);
        if (user == null)
            return;

        Roles.AddUserToRole(user.UserName, CON.UserRoleType.ProviderAdministrator);
        user.IsApproved = false;                            
        Membership.UpdateUser(user);

        bool rtn = false;
        Guid userID = new Guid(user.ProviderUserKey.ToString());
        rtn = InsertSecurityQandA(userID);
        if (!rtn)
            return;

        rtn = InsertPasswordHistory(userID);
        if (!rtn)
            return;

        AdditionalInfoAdded = InsertUserAccountInformation(userID);
        if (user != null)
        {
            EmailSent = SendNotificationToProvider(user);
            wizCreateUser.MoveTo(wizCreateUser.CompleteStep);
        }
    }

    protected void wizCreateUser_CreatingUser(object sender, LoginCancelEventArgs e)
    {
            if (Membership.GetUser(wizCreateUser.UserName) != null)
            {
                e.Cancel = true;
                return;
            }

            Page.Validate("RegisterUserValidationGroup");
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v.ValidationGroup.Equals("RegisterUserValidationGroup") && !v.IsValid)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                catch
                {
                    continue;
                }
            }

            // Wait until the last account creation has had enough time to finish
            WaitUntilCreate();
    }

    protected void wizCreateUser_ActiveStepChanged(object sender, EventArgs e)
    {
        switch (wizCreateUser.ActiveStepIndex)
        {
            case 0:
                ucStep1.Selected = true;
                ucStep2.Selected = ucStep3.Selected = false;
                break;
            case 1:
                cancelProgress++;
                ucStep2.Selected = true;
                ucStep1.Selected = ucStep3.Selected = false;
                break;
            case 2:
                ucStep3.Selected = true;
                ucStep1.Selected = ucStep2.Selected = false;
                Label lblSent =  ((Label)wizCreateUser.CompleteStep.ContentTemplateContainer.FindControl("lblEmailSent"));
                Label lblFailed =  ((Label)wizCreateUser.CompleteStep.ContentTemplateContainer.FindControl("lblEmailFailed"));
                if (lblSent != null)
                {
                    lblSent.Visible = EmailSent;
                }
                if (lblFailed != null)
                {
                    lblFailed.Visible = !EmailSent;
                }
                break;
        }
    }

    protected void wizCreateUser_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        Page.Validate();
        if (!Page.IsValid)
        {
            e.Cancel = true;
            return;
        }

        System.Web.UI.WebControls.CreateUserWizard wiz = (System.Web.UI.WebControls.CreateUserWizard)sender;

        if (wiz != null)
        {
            if (wiz.StartNextButtonText.ToUpper() == "CANCEL")
            {
                // User is cancelling send them back to the main home page
                Response.Redirect("~/Default.aspx");
                return;
            }
        }

        if (!e.Cancel) e.Cancel = (cancelProgress > 0);
        if (e.Cancel) return;

        cancelProgress = 0;
        switch (wizCreateUser.ActiveStepIndex)
        {
            case 0:
                // If we had validation failurs.
                DataSet convertedProviders = null;
                if (this.divExistingFound.Visible == false)
                {

                    if (FoundConvertedProvider(out convertedProviders)
                        && this.divExistingFound.Visible == false) //if divExistingFound is true, then need to jump to next step
                    {
                        //Check to see if have just Group Member Profile Record
                        bool onlyIndivProviders = HasOnlyIndividualProviders(convertedProviders);
                        bool hasNFOCUSProvider = HasNFOCUSProvider(convertedProviders);
                        bool onlyNFOCUSProvider = OnlyNFOCUSProvider(convertedProviders);

                        EnableExistingProviderValidators(true, onlyIndivProviders, hasNFOCUSProvider, onlyNFOCUSProvider);

                        DisplayExistingProviderFields(true, onlyIndivProviders, hasNFOCUSProvider, onlyNFOCUSProvider);
                        e.Cancel = true;
                        return;
                    }
                    else if (rblTaxIDType.SelectedItem.Text == "SSN" && !trNPI.Visible)
                    {
                        this.trNPI.Visible =  true;
                        this.ltlNPIApplicable.Visible = false;
                        cvNPI.Enabled = true;
                        cvRequiredNPI.Enabled = true;
                        cvValidateType1NPI.Enabled = true;
                        e.Cancel = true;
                        return;
                    }
                }

                ucStep1.Selected = true;
                ucStep2.Selected = ucStep3.Selected = false;
                break;
            case 1:
                // If we had failures in creating the additional info then stay on this page.
                if (!AdditionalInfoAdded)
                {
                    e.Cancel = true;
                    return;
                }
                ucStep2.Selected = true;
                ucStep1.Selected = ucStep3.Selected = false;
                break;
        }
        e.Cancel = false;
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void wizCreateUser_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
    {
        e.Cancel = false;
        if (wizCreateUser.ActiveStepIndex == 1) wizCreateUser.ActiveStepIndex = 0;
    }

    protected void Validate_UserNameExists(object sender, ServerValidateEventArgs e)
    {
        if (wizCreateUser.ActiveStepIndex != 1)
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = Membership.GetUser(wizCreateUser.UserName) == null;
            cancelProgress += e.IsValid ? 0 : 1;
        }
    }

    protected void UserName_Validating(object sender, ServerValidateEventArgs e)
    {
        e.IsValid = Regex.IsMatch(wizCreateUser.UserName, @"^[a-zA-Z0-9@.]+$") && 0 < wizCreateUser.UserName.Length;
    }

    protected void ddlPasswordQuestion1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPasswordQuestion1.SelectedIndex == 0)
            return;

        string v1 = ddlPasswordQuestion1.SelectedValue;
        string v2 = ddlPasswordQuestion2.SelectedValue;

        Registration.SecurityQuestions(ref ddlPasswordQuestion2);
        ddlPasswordQuestion2.SelectedValue = v2;
        ListItem li = ddlPasswordQuestion2.Items.FindByValue(v1);
        ddlPasswordQuestion2.Items.Remove(li);
        ddlPasswordQuestion1.Focus();
    }

    protected void ddlPasswordQuestion2_SelectedIndexChanged(object sender, EventArgs e)
    {
        Page.Validate();

        if (ddlPasswordQuestion2.SelectedIndex == 0)
            return;

        string v1 = ddlPasswordQuestion1.SelectedValue;
        string v2 = ddlPasswordQuestion2.SelectedValue;

        Registration.SecurityQuestions(ref ddlPasswordQuestion1);
        ddlPasswordQuestion1.SelectedValue = v1;
        ListItem li = ddlPasswordQuestion1.Items.FindByValue(v2);
        ddlPasswordQuestion1.Items.Remove(li);
        ddlPasswordQuestion2.Focus();
    }

    protected void wizCreateUser_CancelButtonClick(object sender, EventArgs e)
    {
        btnModalOk.Visible = true;
        btnModalCancel.Visible = true;
        mpeChangesSaved.Show();
    }

    protected void btnModalOk_Click(object sender, EventArgs e)
    {
        mpeChangesSaved.Hide();
        Response.Redirect(Helper.RedirectLoginURL());
    }

  
    protected void ValidateConvertedFound(object sender, ServerValidateEventArgs e)
    {
        ConvertedProviderFound = false;
        ConvertedTaxonomyTypeID = 0;

        //only run this check when values have been input inputget the records and filter them
        DataSet convertedProviders = GetMatchingOnTaxID();
        DataTable dt = FilterForConvertedProviderMatch(convertedProviders);

        ConvertedProviderFound = dt != null && dt.Rows.Count > 0;
        if (ConvertedProviderFound)
        {
            ConvertedTaxonomyTypeID = Helper.GetInt("TAXONOMY_TYPE_ID", dt.Rows[0]);
        }         
        e.IsValid = dt != null && dt.Rows.Count > 0;
    }

    protected void ValidateConvertedFoundOnNPI(object sender, ServerValidateEventArgs e)
    {
        ConvertedProviderFound = false;
        ConvertedTaxonomyTypeID = 0;
        Boolean isValid = true;

        //only run this check when values have been input inputget the records and filter them
        DataSet convertedProviders = GetMatchingOnNPI();
        DataTable dt = FilterForConvertedProviderMatch(convertedProviders);

        ConvertedProviderFound = dt != null && dt.Rows.Count > 0;
        if (ConvertedProviderFound)
        {
            String TaxID = Helper.GetString("TAX_ID", dt.Rows[0]);
            if (TaxID == "")
            {
                ConvertedTaxonomyTypeID = Helper.GetInt("TAXONOMY_TYPE_ID", dt.Rows[0]);
                this.txtTaxonomyCode.Text = Helper.GetString("TAXONOMY_CODE", dt.Rows[0]);
                this.txtZipCode.Text = Helper.GetString("SERVICING_ZIP", dt.Rows[0]);
                this.txtZipCodeExt.Text = Helper.GetString("SERVICING_EXT_ZIP", dt.Rows[0]);
                this.txtMedicaidID.Text = Helper.GetString("MEDICAID_ID", dt.Rows[0]);
                isValid = true;
            }
            else
            {
                if (TaxID != this.txtTaxID.Text)
                    isValid = false;
                else
                    isValid = true;
            }
        }
        e.IsValid =  isValid;
    }

    protected void ValidateType1NPI(object sender, ServerValidateEventArgs e)
    {
        bool isValid = true;
        if (rblTaxIDType.SelectedValue == CON.TaxIDType.SSN.ToString() && !string.IsNullOrEmpty(txtNPI.Text))
        {
            bool isNPIAPIEnabled = AppSettings.Get("NPI-Registry-Enabled").ToString().Equals("true", StringComparison.InvariantCultureIgnoreCase) ? true : false;

            if (isNPIAPIEnabled)
            {
                MAXIMUS.Core.Libraries.NPPESAPIResult result = svc.ValidNPIinNPPESApi(Convert.ToInt64(txtNPI.Text));

                if (result.result_count > 0)
                {
                    int nppesTypeID = Convert.ToInt32(result.results[0].enumeration_type.Split(new string[] { "NPI-" }, StringSplitOptions.None).Last());

                    if (nppesTypeID != 1)
                        isValid = false;
                }
                else
                    isValid = false;
            }
        }
       
        e.IsValid = isValid;
    }

    
    protected void ValidateRequiredNPI(object sender, ServerValidateEventArgs e)
    {
        bool isValid = true;
        if (rblTaxIDType.SelectedValue == CON.TaxIDType.SSN.ToString() && string.IsNullOrEmpty(txtNPI.Text))
            isValid = false;
        e.IsValid = isValid;
    }
    #endregion

    #region Private Methods

    // Wait a specific number of seconds before creating the account
    // Wait until the previous account is created
    // Use UTC time so that an account can be created from anywhere
    private void WaitUntilCreate()
    {
        string test = AppSettings.Get(CON.AspnetUserAccountCreate);
        if (!string.IsNullOrEmpty(test))
        {
            DateTime lastTime = Convert.ToDateTime(test);
            int seconds = 5;                // Default
            test = AppSettings.Get(CON.AspnetUserAccountSeconds);
            if (string.IsNullOrEmpty(test)) AppSettings.Set(CON.AspnetUserAccountSeconds, seconds.ToString(), true);
            else seconds = Convert.ToInt32(test);
            System.TimeSpan span;
            do
            {
                System.Threading.Thread.Sleep(1000);            // Sleep 1 second
                span = DateTime.UtcNow - lastTime;
            }
            while (span.Seconds < seconds);
        }
        AppSettings.Set(CON.AspnetUserAccountCreate, DateTime.UtcNow.ToString(), false);
    }

    private void AddPopUpError(string errMsg)
    {
        MessageBox2.Show(UserControls_MessageModal.MessageModalMode.Continue, "Error", errMsg);
    }

    private void RecordException(Exception ex, string methodName, bool throwEx)
    {
        string errMsg = ex.Message + " [" + methodName + "]";
        if (throwEx) throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception(errMsg));
        else CoreException.ThrowException(new Exception(errMsg));
        AddPopUpError(errMsg);
    }

    private bool FoundConvertedProvider(out DataSet ds)
    {
        bool foundConverted = false;
        ds = GetMatchingOnTaxID();

        if (Helper.HasRows(ds))
        {
            foundConverted = true;
        }
        return foundConverted;

    }

    private bool HasOnlyIndividualProviders(DataSet convertedProviders)
    {
        bool onlyIndivProviders = false;

        StringBuilder selectPart = new StringBuilder();
        selectPart.Append("PROVIDER_CATEGORY_TYPE_ID NOT IN (1,6)");

        DataTable dt = convertedProviders.Tables[0];
        if (dt.Select(selectPart.ToString()).Count() == 0)
        {
            onlyIndivProviders = true;
        }
        return onlyIndivProviders;
    }

    private bool HasNFOCUSProvider(DataSet convertedProviders)
    {
        bool hasNFOCUSProvider = false;

        StringBuilder selectPart = new StringBuilder();
        selectPart.Append("PROVIDER_TYPE_ID IN (82,128)");

        DataTable dt = convertedProviders.Tables[0];
        if (dt.Select(selectPart.ToString()).Count() > 0)
        {
            hasNFOCUSProvider = true;
        }
        return hasNFOCUSProvider;
    }

    private bool OnlyNFOCUSProvider(DataSet convertedProviders)
    {
        bool onlyNFOCUSProvider = false;

        StringBuilder selectPart = new StringBuilder();
        selectPart.Append("PROVIDER_TYPE_ID NOT IN (82,128)");

        DataTable dt = convertedProviders.Tables[0];
        if (dt.Select(selectPart.ToString()).Count()==  0)
        {
            onlyNFOCUSProvider = true;
        }
        return onlyNFOCUSProvider;
    }

    private bool InsertUserAccountInformation(Guid userID)
    {
        bool rtn = false;

        string phn = ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtPhone")).Text.Trim();

        try
        {
            string medicaidID = ConvertedProviderFound ? this.txtMedicaidID.Text.Trim() : string.Empty;
            int taxonomyTypeID = ConvertedProviderFound ? ConvertedTaxonomyTypeID : 0;
            string npi = ConvertedProviderFound ? this.txtNPI.Text.Trim() : string.Empty;
            int zip = ConvertedProviderFound ? string.IsNullOrEmpty(this.txtZipCode.Text.Trim()) || string.IsNullOrEmpty(this.txtZipCodeExt.Text.Trim()) ? 0 
                : Convert.ToInt32(string.Concat(this.txtZipCode.Text.Trim(), this.txtZipCodeExt.Text.Trim())) : 0;
            //max value for int is  2,147,483,647
            //2147483647
            //999999999

            svc.InsertUserAccountInformation(
                userID.ToString(),
                ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtContactName")).Text.Trim(),
                ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtTitle")).Text.Trim(),
                Helper.StripNonNumerics(phn), //Phone
                ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtPhoneExt")).Text.Trim(),
                medicaidID,
                0, //placeholder for Provider Type Category ID
                0, //placeholder for Provider Type
                this.txtTaxID.Text.Trim(), //TaxID
                Convert.ToInt32(this.rblTaxIDType.SelectedValue),
                npi, 
                string.Empty, //placeholder for Organization Name
                zip, //placeholder for Zip 
                0, //placeholder for referralID
                taxonomyTypeID, 
                0, //placeholder for SpecialtyTypeID
                DateTime.Now,
                userID.ToString(),
                false,
                null,
                null,
                null,
                regId: null);
        }
        catch (Exception ex)
        {
            RecordException(ex, "InsertUserAccountInformation", true);
            return rtn;
        }

        rtn = true;
        return rtn;
    }

    private bool InsertPasswordHistory(Guid userID)
    {
        bool rtn = false;
        try
        {
            //save password history
            using (PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient())
            {
                Guid loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
                UserController.CopyPasswordToPwdHistory(userID, loggedInUserId);
                rtn = true;
            }
        }
        catch (Exception ex)
        {
            RecordException(ex, "InsertPasswordHistory", true);
            return rtn;
        }

        return rtn;
    }

    private bool InsertSecurityQandA(Guid userID)
    {
        bool rtn = false;
        try
        {
            svc.InsertUserSecurityQuestions(
                userID.ToString(),
                Convert.ToInt32(((DropDownList)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("ddlPasswordQuestion1")).SelectedValue),
                ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtAnswer1")).Text,
                Convert.ToInt32(((DropDownList)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("ddlPasswordQuestion2")).SelectedValue),
                ((TextBox)wizCreateUser.CreateUserStep.ContentTemplateContainer.FindControl("txtAnswer2")).Text,
                question3Id: null,
                answer3: string.Empty,
                changedDate: DateTime.Now,
                changedBy: userID.ToString());

            rtn = true;
        }
        catch (Exception ex)
        {
            RecordException(ex, "InsertUserSecurityQuestions", true);
            return rtn;
        }
        return rtn;
    }

    private bool SendNotificationToProvider(MembershipUser user)
    {
        /* Send Email To Provider */
        bool rtn = false;
        try
        {
            string urlFromDB = Helper.GetAppSettingFromDB("PDMS-URL", string.Empty);
            //http://ucocdmmweb01pdm/DC_PDMS_Dev/Account/Login.aspx
            Uri uri = new Uri(urlFromDB);
            string registerUrl = string.Format("?id={0:N}", user.ProviderUserKey.ToString().Trim());
            registerUrl = new Uri(uri, registerUrl).ToString();
            string dummyOrgName = string.Empty;
            string dummyNPI = string.Empty;
            int dummyRegID = 0;
            svc.NotifyProviderAccountCreation(wizCreateUser.Email, dummyOrgName, dummyNPI, wizCreateUser.UserName,
                registerUrl, dummyRegID, Helper.GetUserId(user.UserName));

            rtn = true;
        }
        catch (Exception ex)
        {
            // Log the error but continue onward.  This means the email did not go out but the account was created.
            RecordException(ex, "CreateAdditionalUserData - NotifyProviderAccountCreation", false);
            rtn = false;
        }

        return rtn;
    }

    private void SetupButtonDisablesOfMultiClick()
    {
        Button btnStartNext = (Button)Helper.FindTheControl(this, "StartNextButton");
        btnStartNext.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnStartNext, null) + ";");

        Button cancelBtn = (Button)Helper.FindTheControl(this, "CancelButton");
        cancelBtn.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(cancelBtn, null) + ";");

        Button stepPreviousBtn = (Button)Helper.FindTheControl(this, "StepPreviousButton");
        stepPreviousBtn.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(stepPreviousBtn, null) + ";");

        Button stepNextBtn = (Button)Helper.FindTheControl(this, "StepNextButton");
        stepNextBtn.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(stepNextBtn, null) + ";");

        Button btnCancel = (Button)Helper.FindTheControl(this, "btnCancel");
        btnCancel.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnCancel, null) + ";");
    }

    private void DisplayExistingProviderFields(bool isVisible, bool onlyIndivProviders, bool hasNFOCUSProviders, bool onlyNFOCUSProviders)
    {
        //Nfocus provider can be Entity Type 4 or 1, so could be an individual.
        //Nfocus requires more fields than an individual/sole proprietor does
        this.divExistingFound.Visible = isVisible;
        this.trNPI.Visible = isVisible == false ? false : onlyNFOCUSProviders ? false : true;
        this.trTaxonomy.Visible = isVisible == false ? false : onlyIndivProviders || onlyNFOCUSProviders ? false : true;
        this.trZip.Visible = isVisible == false ? false : hasNFOCUSProviders ? true : onlyIndivProviders ? false : true;
        this.trZipExt.Visible = isVisible == false ? false : onlyNFOCUSProviders ? false : onlyIndivProviders ? false : true;
        this.trMedicaidID.Visible = isVisible == false ? false : hasNFOCUSProviders ? true : onlyIndivProviders ? false : true;
        this.trMedicaidInfo.Visible = isVisible == false ? false : hasNFOCUSProviders ? true : onlyIndivProviders ? false : true;

    }

    private void EnableExistingProviderValidators(bool isEnabled, bool onlyIndivProviders, bool hasNFOCUSProviders, bool onlyNFOCUSProviders)
    {
        this.cvConvertedFound.Enabled = isEnabled;
        this.valZipReqd.Enabled = isEnabled == false ? false : hasNFOCUSProviders ? true : onlyIndivProviders ? false : true;
        this.valZipExtRqd.Enabled = isEnabled == false ? false : onlyNFOCUSProviders ? false : onlyIndivProviders ? false : true;
        this.valMediaidIDReqd.Enabled = isEnabled == false ? false : hasNFOCUSProviders ? true : onlyIndivProviders ? false : true;
    }

    private DataSet GetMatchingOnTaxID()
    {
        string taxID = this.txtTaxID.Text.Trim();
        return svc.GetMatchingProviderByTaxID(taxID, true);
    }

    private DataSet GetMatchingOnNPI()
    {       
        string NPI = this.txtNPI.Text.Trim();

        return svc.GetMatchingProviderByNPI(NPI, true);
    }
   
    private DataTable FilterForConvertedProviderMatch(DataSet ds)
    {
        if (!Helper.HasRows(ds))
            return null;

        try
        {
            DataTable dt = ds.Tables[0];
            StringBuilder selectPart = new StringBuilder();
            selectPart.Append(string.Format("NPI ='{0}'", this.txtNPI.Text.Trim()));
            if (this.trTaxonomy.Visible)
            {
                selectPart.Append(string.Format(" AND TAXONOMY_CODE = '{0}'", this.txtTaxonomyCode.Text.Trim()));
            }
            if (this.trZip.Visible)
            {
                selectPart.Append(string.Format(" AND SERVICING_ZIP = '{0}'", this.txtZipCode.Text.Trim()));
            }
            if (this.trZipExt.Visible)
            {
                selectPart.Append(string.Format(" AND SERVICING_EXT_ZIP = '{0}'", this.txtZipCodeExt.Text.Trim()));
            }
            if (this.trMedicaidID.Visible)
            {
                selectPart.Append(string.Format(" AND MEDICAID_ID = '{0}'", this.txtMedicaidID.Text.Trim()));
            }

            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
            return null;
        }
        catch (Exception ex)
        {
            RecordException(ex, "Validation - FilterForConvertedProviderMatch", true);
        }
        return null;
    }

    #endregion

}
