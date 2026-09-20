using Corp.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_RecipientInformationPanel : System.Web.UI.UserControl
{

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    #endregion
    public string ShowRecipeintErrorMessage
    {        
        set
        {
            lblErrorMsgDetils.Text = value.Trim();
        }
    }
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimIdRecipient.Value))
                return hdnClaimIdRecipient.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdRecipient.Value = value.Trim();
        }
    }
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }
    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType_Recipient.Value))
                return hdnClaimType_Recipient.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                hdnClaimType_Recipient.Value = value.Trim();
                DisplayPanelControls();
            }
        }
    }
    public string MedicaidBillingNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtMedicaidBillingNumber.Text))
                return txtMedicaidBillingNumber.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtMedicaidBillingNumber.Text = value;
        }
    }
    public string PatientControlNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPatientControlNumber.Text))
                return txtPatientControlNumber.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPatientControlNumber.Text = value;
        }
    }
    public string BirthDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBirthDate.Text))
                return txtBirthDate.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBirthDate.Text = value;
        }
    }
    public string AddressLine
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAddress.Text))
                return lblAddress.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblAddress.Text = value;
        }
    }
    public string AddressLine2
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblAddressLine2.Text))
                return lblAddressLine2.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblAddressLine2.Text = value;
        }
    }

    public string SSN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblssn.Text))
                return lblssn.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblssn.Text = value;
        }
    }
    public string LastName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblLastName.Text))
                return lblLastName.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblLastName.Text = value;
        }
    }
    public string Gender
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBirthDate.Text))
                return lblGender.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblGender.Text = value;
        }
    }
    public string FirstName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblfrstmi.Text))
                return lblfrstmi.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblfrstmi.Text = value;
        }
    }
    public string MiddleName
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblMiddleName.Text))
                return lblMiddleName.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblMiddleName.Text = value;
        }
    }
    public string MedicalRecordNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtMedRecNumber.Text))
                return txtMedRecNumber.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtMedRecNumber.Text = value;
        }
    }
    public string PregnancyIndicatorValue
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropdownPregnancyIndicator.SelectedValue))
                return dropdownPregnancyIndicator.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && value != "N")
            {
                dropdownPregnancyIndicator.ClearSelection();
                dropdownPregnancyIndicator.SelectedItem.Text = value.Trim();
            }
        }
    }
    public string PregnancyIndicatorText
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dropdownPregnancyIndicator.SelectedItem.Text))
                return dropdownPregnancyIndicator.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value) && dropdownPregnancyIndicator.Items.FindByText(value) != null)
            {
                dropdownPregnancyIndicator.ClearSelection();
                dropdownPregnancyIndicator.SelectedItem.Text = value.Trim();
            }

        }
    }
    public string City
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblCity.Text))
                return lblCity.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblCity.Text = value;
        }
    }
    public string State
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblState.Text))
                return lblState.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblState.Text = value;
        }
    }
    public string ZipCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblZipcode.Text))
                return lblZipcode.Text;
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                lblZipcode.Text = value;
        }
    }


    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text) || !string.IsNullOrEmpty(txtBirthDate.Text) || !string.IsNullOrWhiteSpace(dropdownPregnancyIndicator.SelectedItem.Text)
            || !string.IsNullOrWhiteSpace(txtPatientControlNumber.Text))
        {
            rtn = true;
        }

        return rtn;
    }

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
    protected void Page_Load(object sender, EventArgs e)
    {
        DisplayPanelControls();

        if (!IsPostBack)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnClaimIdRecipient.Value))
                {
                    GetRecipientInfo();
                }
            }
            catch
            {
            }
        }
        if ((txtPatientControlNumber.Text != null) && (txtPatientControlNumber.Text != ""))
        { patientControlNumberError.Text = ""; }

        if (txtMedicaidBillingNumber.Text.Length == 12 && txtBirthDate.Text != "")
        {
            if (Session["ClaimStatus"] != null)
            {
                if (Session["ClaimStatus"].ToString().ToUpper() == "PENDING SUBMISSION")
                {
                    RecipientInformation(); // force refresh of recipient information, override whatever might be in the session
                }
            }
        }

        //Clearing yellow selection on UI on fresh claims
        medicaidrequiredError.Text = "";
        txtMedicaidBillingNumber.Style.Add("background-color", "white");
        birthDateRequiredError.Text = "";
        txtBirthDate.Style.Add("background-color", "white");
        patientControlNumberError.Text = "";
        txtPatientControlNumber.Style.Add("background-color", "white");

        hdnMedicaidID.Value = this.WorkflowPage.MedicaidID;
        hdnUserName.Value = HttpContext.Current.User.Identity.Name.ToString();
    }

    private void DisplayPanelControls()
    {
        if (hdnClaimType_Recipient.Value == CON.ClaimsType.Dental)
        {
            divMedRecNumber.Visible = false;
            divPregnancyIndicatorEmptyhide.Visible = false;
            divpregnancyIndicatordrpdwn.Visible = false;
            divEmptyforDental.Visible = true;
            divMedicalRecordHideEmpty.Visible = true;
        }
        if (hdnClaimType_Recipient.Value == CON.ClaimsType.Institutional)
        {
            divMedRecNumber.Visible = true;
            divPregnancyIndicatorEmptyhide.Visible = true;
            divpregnancyIndicatordrpdwn.Visible = false;
            divEmptyforDental.Visible = false;
            divMedicalRecordHideEmpty.Visible = false;
        }
        if (hdnClaimType_Recipient.Value == CON.ClaimsType.Professional)
        {
            divMedRecNumber.Visible = true;
            divPregnancyIndicatorEmptyhide.Visible = true;
            divpregnancyIndicatordrpdwn.Visible = true;
            divEmptyforDental.Visible = false;
            divMedicalRecordHideEmpty.Visible = false;
        }
    }

    protected void txtCheckForPerson_TextChanged(object sender, EventArgs e)
    {
        
        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate();</script>", false);
        
        if (Regex.IsMatch(txtBirthDate.Text, "((0[1-9]|1[0-2])\\/((0|1)[0-9]|2[0-9]|3[0-1])\\/((19|20)\\d\\d))$"))
        {
            if (!string.IsNullOrEmpty(txtBirthDate.Text) && DateTime.Parse(txtBirthDate.Text) > DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")))
            {
                txtBirthDate.Text = string.Empty;

            }
            RecipientInformation();
        }

        if ((txtBirthDate.Text != ""))
            { birthDateRequiredError.Visible = false; }

    }
    private void RecipientInformation()
    {
        if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text) &&
              !string.IsNullOrEmpty(txtBirthDate.Text) && DateTime.Parse(txtBirthDate.Text) <= DateTime.Parse(DateTime.Now.ToString("MM/dd/yyyy")))
        {
            if (txtMedicaidBillingNumber.Text.Length == 12)
            {
                try
                {
                    string medicaidID = this.WorkflowPage.MedicaidID;
                    Guid userId = Helper.GetUserId(HttpContext.Current.User.Identity.Name.ToString());
                    var recipientInfo = FindRecipient(txtMedicaidBillingNumber.Text, medicaidID, userId, txtBirthDate.Text);
                    if (!string.IsNullOrEmpty(recipientInfo.FirstName))
                    {
                        this.WorkflowPage.ClaimRecipientId = txtMedicaidBillingNumber.Text.Trim();
                        if (this.WorkflowPage.RecipientInfo != null)
                        {
                            if (!this.WorkflowPage.RecipientInfo.ContainsKey("FirstName"))
                                this.WorkflowPage.RecipientInfo.Add("FirstName", recipientInfo.FirstName);
                            if (!this.WorkflowPage.RecipientInfo.ContainsKey("LastName"))
                                this.WorkflowPage.RecipientInfo.Add("LastName", recipientInfo.LastName);
                            if (!this.WorkflowPage.RecipientInfo.ContainsKey("Address"))
                                this.WorkflowPage.RecipientInfo.Add("Address", recipientInfo.AddressLine1);
                        }
                        lblLastName.Text = recipientInfo.LastName;
                        lblfrstmi.Text = recipientInfo.FirstName;
                        lblMiddleName.Text = recipientInfo.MiddleName;
                        lblGender.Text = recipientInfo.Gender;  // recipientInfo.Gender == "F" ? "Female" : recipientInfo.Gender == "M" ? "Male" : "Unknown";
                        lblAddress.Text = recipientInfo.AddressLine1;
                        lblAddressLine2.Text = recipientInfo.AddressLine2;
                        lblssn.Text = recipientInfo.SSN;
                        lblCity.Text = recipientInfo.City;
                        lblZipcode.Text = recipientInfo.ZipCode5;
                        lblState.Text = recipientInfo.StateCode;
                        if (recipientInfo.Errors != null && recipientInfo.Errors.Count > 0)
                        {
                            StringBuilder errorMsg = new StringBuilder();
                            foreach (var ed in recipientInfo.Errors)
                            {
                                if (errorMsg.Length > 0)
                                {
                                    errorMsg.Append("; ");
                                    errorMsg.Append(ed.Code + " : " + ed.Description);
                                }
                                else
                                    errorMsg.Append(ed.Code + " : " + ed.Description);
                            }
                            lblErrorMsgDetils.Text = errorMsg.ToString();
                        }
                        else
                            lblErrorMsgDetils.Text = "";
                    }
                    else
                    {
                        txtMedicaidBillingNumber.Text = "";
                        txtBirthDate.Text = "";
                        lblLastName.Text = "";
                        lblfrstmi.Text = "";
                        lblGender.Text = "";
                        lblAddress.Text = "";
                        lblMiddleName.Text = "";
                        lblAddressLine2.Text = "";
                        lblssn.Text = "";
                        lblCity.Text = "";
                        lblZipcode.Text = "";
                        lblState.Text = "";
                        lblErrorMsgDetils.Text = "Valid recipient information is required";
                    }
                }
                catch (FaultException ex)
                {
                    lblErrorMsgDetils.Text = "Error: An error occurred while processing the request - " + ex.Message;
                    ClearAutoPopulatedFields();
                }
                catch
                {
                    lblErrorMsgDetils.Text = "Error: An error occurred while processing the request";
                    ClearAutoPopulatedFields();
                }
            }
            else
            {
                lblErrorMsgDetils.Text = "Medicaid Billing Number 12-digit number is required";
                ClearAutoPopulatedFields();
            }
        }
        else
        {
            ClearAutoPopulatedFields();
        }
        if (txtMedicaidBillingNumber.Text != "") { medicaidrequiredError.Text = string.Empty;
            if (txtMedicaidBillingNumber.Text.Length != 12) { lblErrorMsgDetils.Text = "Medicaid Billing Number 12-digit number is required"; }
            //else { lblErrorMsg.Text = string.Empty; }
        }
        if ((txtPatientControlNumber.Text != null) && (txtPatientControlNumber.Text != ""))
        { patientControlNumberError.Text = ""; }
    }

    public void GetRecipientInfo()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdRecipient.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimIdRecipient.Value.ToString());
            DataSet dsRecipient = svc.SelectPanelsData("Claims_Recipent_Information", parms);
            if (Helper.HasRows(dsRecipient) && Convert.ToInt32(dsRecipient.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdRecipient.Value))
            {
                txtMedicaidBillingNumber.Text = dsRecipient.Tables[0].Rows[0]["Medicaid_Bill_Number"].ToString();
                if (!string.IsNullOrWhiteSpace(dsRecipient.Tables[0].Rows[0]["Date_of_Birth"].ToString()))
                {
                    txtBirthDate.Text = Convert.ToDateTime(dsRecipient.Tables[0].Rows[0]["Date_of_Birth"]).ToString("MM/dd/yyyy");
                }
                lblfrstmi.Text = dsRecipient.Tables[0].Rows[0]["First_Name"].ToString();
                lblLastName.Text = dsRecipient.Tables[0].Rows[0]["Last_Name"].ToString();
                lblMiddleName.Text = dsRecipient.Tables[0].Rows[0]["Middle_Name"].ToString();
                lblGender.Text = dsRecipient.Tables[0].Rows[0]["Gender"].ToString();
                lblAddress.Text = dsRecipient.Tables[0].Rows[0]["Address_Line1"].ToString();
                lblCity.Text = dsRecipient.Tables[0].Rows[0]["City"].ToString();
                lblState.Text = dsRecipient.Tables[0].Rows[0]["State"].ToString();
                lblZipcode.Text = dsRecipient.Tables[0].Rows[0]["Zip_Code"].ToString();
                txtPatientControlNumber.Text = dsRecipient.Tables[0].Rows[0]["Patient_Control_Number"].ToString();
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Professional)
                {
                    txtMedRecNumber.Text = dsRecipient.Tables[0].Rows[0]["Medical_Record_Number"].ToString();
                    dropdownPregnancyIndicator.SelectedValue = dsRecipient.Tables[0].Rows[0]["Pregnancy_Indicator"].ToString();
                }
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Institutional)
                {
                    txtMedRecNumber.Text = dsRecipient.Tables[0].Rows[0]["Medical_Record_Number"].ToString();
                }
            }
        }
    }

    public void SaveRecipientInfo(string validationSummary, int action)
    {        
        if (ValidateDateofBirth())
        {
            if (action == CON.ActionButtonType.Save)
            {
                SaveToDB();
            }
            else
            {
                Page.Validate(validationSummary);
                if (Page.IsValid)
                {
                    SaveToDB();
                }
            }
        }

    }
    public bool ValidateDateofBirth()
    {
        bool isDatecorrect = true;
        lblDOBErrorMessage.Text = string.Empty;        
        if (!string.IsNullOrEmpty(txtBirthDate.Text.ToString()))
        {            
            if (!Regex.IsMatch(txtBirthDate.Text, "((0[1-9]|1[0-2])\\/((0|1)[0-9]|2[0-9]|3[0-1])\\/((19|20)\\d\\d))$"))
            {
                lblDOBErrorMessage.Text = "Date of Birth should be in the MM/dd/yyyy format";                
                isDatecorrect = false;
            }
        }
        return isDatecorrect;
    }
    private void SaveToDB()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdRecipient.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();

            parms.Add("Claim_ID", hdnClaimIdRecipient.Value.ToString());
            DataSet dsRecipientProvider = svc.SelectPanelsData("claims_recipent_information", parms);
            if (Helper.HasRows(dsRecipientProvider) && Convert.ToInt32(dsRecipientProvider.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdRecipient.Value))
            {
                parms.Add("Medicaid_Bill_Number", txtMedicaidBillingNumber.Text.ToString());
                parms.Add("Date_of_Birth", txtBirthDate.Text.ToString());
                parms.Add("Patient_Control_Number", txtPatientControlNumber.Text.ToString());
                parms.Add("Last_Name", lblLastName.Text.ToString());
                parms.Add("First_Name", lblfrstmi.Text.ToString());
                parms.Add("Middle_Name", lblMiddleName.Text.ToString());
                parms.Add("Gender", lblGender.Text.ToString());
                parms.Add("Address_Line1", lblAddress.Text.ToString());
                parms.Add("Address_Line2", lblAddressLine2.Text.ToString());
                parms.Add("City", lblCity.Text.ToString());
                parms.Add("State", lblState.Text.ToString());
                parms.Add("Zip_Code", lblZipcode.Text.ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Professional)
                {
                    parms.Add("Pregnancy_Indicator", dropdownPregnancyIndicator.SelectedValue);
                    parms.Add("Medical_Record_Number", txtMedRecNumber.Text.ToString());
                }
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Institutional)
                {
                    parms.Add("Medical_Record_Number", txtMedRecNumber.Text.ToString());
                }
                svc.UpdatePanelsData("Claims_Recipent_Information", parms);
                GetRecipientInfo();
            }
            else
            {
                parms.Add("Medicaid_Bill_Number", txtMedicaidBillingNumber.Text.ToString());
                parms.Add("Date_of_Birth", txtBirthDate.Text.ToString());
                parms.Add("Patient_Control_Number", txtPatientControlNumber.Text.ToString());
                parms.Add("Last_Name", lblLastName.Text.ToString());
                parms.Add("First_Name", lblfrstmi.Text.ToString());
                parms.Add("Middle_Name", lblMiddleName.Text.ToString());
                parms.Add("Gender", lblGender.Text.ToString());
                parms.Add("Address_Line1", lblAddress.Text.ToString());
                parms.Add("Address_Line2", lblAddressLine2.Text.ToString());
                parms.Add("City", lblCity.Text.ToString());
                parms.Add("State", lblState.Text);
                parms.Add("Zip_Code", lblZipcode.Text.ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Professional)
                {
                    parms.Add("Pregnancy_Indicator", dropdownPregnancyIndicator.SelectedValue);
                    parms.Add("Medical_Record_Number", txtMedRecNumber.Text.ToString());
                }
                if (hdnClaimType_Recipient.Value == CON.ClaimsType.Institutional)
                {
                    parms.Add("Medical_Record_Number", txtMedRecNumber.Text.ToString());
                }
                svc.InsertPanelsData("Claims_Recipent_Information", parms);
                GetRecipientInfo();
            }
        }
    }

    public RecipientInformation FindRecipient(string medicaidbillingnumber, string medicaidID, Guid userId, string dateofBirth)
    {
        RecipientEligibilitySearchReqRes recipientEligibility = new RecipientEligibilitySearchReqRes();
        RecipientInformation recipientInfo = recipientEligibility.GetRecipientInformation(medicaidbillingnumber, medicaidID, userId, dateofBirth);
        return recipientInfo;
    }
    private void SetReadOnlyFieldsControl(bool value)
    {

        txtBirthDate.ReadOnly = value;
        txtMedRecNumber.ReadOnly = value;
        txtMedicaidBillingNumber.ReadOnly = value;
        txtPatientControlNumber.ReadOnly = value;
        dropdownPregnancyIndicator.Enabled = !value;
        ceBirthdate.Enabled = !value;

    }

    public void ClearField()
    {
        lblLastName.Text = "";
        lblfrstmi.Text = "";
        lblGender.Text = "";
        lblAddress.Text = "";
        lblMiddleName.Text = "";
        lblAddressLine2.Text = "";
        lblssn.Text = "";
        lblCity.Text = "";
        lblZipcode.Text = "";
        lblState.Text = "";
        lblErrorMsgDetils.Text = "";
        txtMedicaidBillingNumber.Text = "";
        txtBirthDate.Text = "";
        txtPatientControlNumber.Text = "";
        txtMedRecNumber.Text = "";
        // dropdownPregnancyIndicator.SelectedIndex = 0;
    }
    public void ClearAutoPopulatedFields()
    {
        lblLastName.Text = string.Empty;
        lblfrstmi.Text = string.Empty;
        lblGender.Text = string.Empty;
        lblAddress.Text = string.Empty;
        lblMiddleName.Text = string.Empty;
        lblAddressLine2.Text = string.Empty;
        lblssn.Text = string.Empty;
        lblCity.Text = string.Empty;
        lblZipcode.Text = string.Empty;
        lblState.Text = string.Empty;
    }

    protected void txtMedicaidBillingNumber_TextChanged(object sender, EventArgs e)
    {
        ClearAutoPopulatedFields();
        txtBirthDate.Text = string.Empty;
        txtPatientControlNumber.Text = string.Empty;
        txtMedRecNumber.Text = string.Empty;
        dropdownPregnancyIndicator.SelectedIndex = 0;
    }

    protected void validate(object sender, EventArgs e)
    {
        if ((txtPatientControlNumber.Text != null) && (txtPatientControlNumber.Text != ""))
        { patientControlNumberError.Text = ""; }
    }
}