using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using NPOI.SS.Formula.Functions;
using NPOI.XSSF.Streaming.Values;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ServiceInformationProfessional : System.Web.UI.UserControl
{
    private Logging log = null;
    private bool fromInquirySvc = false;
    #region " Properties "

    public bool FromInquirySvc
    {
        get
        {
            return fromInquirySvc;
        }
        set
        {
            fromInquirySvc = value;
        }
    }

    public string ReleaseofInformation
    {
        get
        {
            return ddlProfessionalDentalReleaseofInfo.SelectedValue;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                if (value == "I")
                {
                    ddlProfessionalDentalReleaseofInfo.SelectedValue = "N";
                }
                else
                {
                    ddlProfessionalDentalReleaseofInfo.SelectedValue = value.Trim();
                }
        }
    }
    public string ReleaseofInformationDisplay
    {
        get
        {
            if (ddlProfessionalDentalReleaseofInfo.SelectedItem != null && !string.IsNullOrEmpty(ddlProfessionalDentalReleaseofInfo.SelectedItem.Text))
                return ddlProfessionalDentalReleaseofInfo.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlProfessionalDentalReleaseofInfo.SelectedItem.Text = value.Trim();
        }
    }
    public string EPSDTConditionIndicator
    {

        get { return ddlProfessionalEPSDTCondition.SelectedValue; }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlProfessionalEPSDTCondition.SelectedValue = value.Trim();
            else
            {
                ddlProfessionalEPSDTCondition.SelectedValue = "";
                ddlESPSDTCODEPRofessionalFirst.SelectedValue = "";
                ddlESPSDTCODEPRofessionalSecond.SelectedValue = "";
                ddlESPSDTCODEPRofessionalThird.SelectedValue = "";
                EPSDTConditionCodeFirstDisplay = "";
                EPSDTConditionCodeSecondDisplay = "";
                EPSDTConditionCodeThirdDisplay = "";
            }
        }
    }
    public string EPSDTConditionIndicatorDisplay
    {
        get
        {
            if (ddlProfessionalEPSDTCondition.SelectedItem != null && !string.IsNullOrEmpty(ddlProfessionalEPSDTCondition.SelectedItem.Text))
                return ddlProfessionalEPSDTCondition.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlProfessionalEPSDTCondition.SelectedValue = value.Trim();
        }
    }
    public string PatientAmountPaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProfessionalPatientAmountPaid.Text))
                return txtProfessionalPatientAmountPaid.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtProfessionalPatientAmountPaid.Text = value;
        }
    }
    public string PlaceOfService
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblPlcServiceProfessional.Text))
                return lblPlcServiceProfessional.Text;

            else
                return string.Empty;
        }
        set { lblPlcServiceProfessional.Text = value; }
    }
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }
    public Boolean SetEnable
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
            {
                ddlESPSDTCODEPRofessionalFirst.Enabled = value;
                ddlESPSDTCODEPRofessionalSecond.Enabled = value;
                ddlESPSDTCODEPRofessionalThird.Enabled = value;
            }
        }


    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtProfessionalHospitalDischargeDate.ReadOnly = value;
        txtProfessionalLstmenstural.ReadOnly = value;
        txtProfessionalPatientAmountPaid.ReadOnly = value;
        ddlESPSDTCODEPRofessionalFirst.Enabled = !value;
        ddlESPSDTCODEPRofessionalSecond.Enabled = !value;
        ddlESPSDTCODEPRofessionalThird.Enabled = !value;
        ddlProfessionalDentalReleaseofInfo.Enabled = !value;
        ddlProfessionalEPSDTCondition.Enabled = !value;
        ddlSpcPrgmIndProfessional.Enabled = !value;
        txtProfessionalLstmenstural.Enabled = !value;
        calextnderProfessional.Enabled = !value;
        ceProfessionalHospitalDischargeDate.Enabled = !value;

    }
    
    private string getCodeFromEPSDValue(string value)
    {
        string numericValue = value;
        if (value.Equals("5"))
        {
            numericValue = "S2";
        }
        if (value.Equals("6"))
        {
            numericValue = "ST";
        }
        if (value.Equals("7"))
        {
            numericValue = "NU";
        }
        if (value.Equals("8"))
        {
            numericValue = "AV";
        }
        return numericValue;
    }
    private string getNumericEPSDValue(string value)
    {
        string numericValue = "";
        if (value.Equals("S2"))
        {
            numericValue = "5";
        }
        if (value.Equals("ST"))
        {
            numericValue = "6";
        }
        if (value.Equals("NU"))
        {
            numericValue = "7";
        }
        if (value.Equals("AV"))
        {
            numericValue = "8";
        }
        return numericValue;
    }
    
    public string EPSDTConditionCodeFirst
    {
        get { return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalFirst.Value); }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ddlESPSDTCODEPRofessionalFirst.Items.FindByText(value) == null)
                {
                    
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalFirst.Items.Add(new ListItem(value, numericValue));
                }
                ddlESPSDTCODEPRofessionalFirst.Items.FindByText(value).Selected = true;
                EPSDTConditionCodeFirstDisplay = value;
            }
            hdnEPSDTCodeProfessionalFirst.Value = value;
        }
    }
    public string EPSDTConditionCodeFirstDisplay
    {
        get
        {
            return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalFirst.Value);
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ddlESPSDTCODEPRofessionalFirst.Items.FindByText(value.ToString().Trim()) != null)
                    ddlESPSDTCODEPRofessionalFirst.Items.FindByText(value).Selected = true;
                else
                {
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalFirst.Items.Add(new ListItem(value.Trim(), numericValue));
                    ddlESPSDTCODEPRofessionalFirst.Items.FindByText(value).Selected = true;
                }
            }
            hdnEPSDTCodeProfessionalFirst.Value = value;
        }
    }
    public string EPSDTConditionCodeSecond
    {
        get { return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalSecond.Value); }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ddlESPSDTCODEPRofessionalSecond.Items.FindByText(value) == null)
                {
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalSecond.Items.Add(new ListItem(value, numericValue));
                }
                ddlESPSDTCODEPRofessionalSecond.Items.FindByText(value).Selected = true;
                EPSDTConditionCodeSecondDisplay = value;
                hdnEPSDTCodeProfessionalSecond.Value = value;
            }
        }
    }
    public string EPSDTConditionCodeSecondDisplay
    {
        get
        {
            return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalSecond.Value);
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ddlESPSDTCODEPRofessionalSecond.Items.FindByText(value.ToString().Trim()) != null)
                    ddlESPSDTCODEPRofessionalSecond.Items.FindByText(value).Selected = true;
                else
                {
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalSecond.Items.Add(new ListItem(value.Trim(), numericValue));
                    ddlESPSDTCODEPRofessionalSecond.Items.FindByText(value).Selected = true;
                }
            }
            hdnEPSDTCodeProfessionalSecond.Value = value;
        }
    }
    public string EPSDTConditionCodeThird
    {
        get { return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalThird.Value); }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (ddlESPSDTCODEPRofessionalThird.Items.FindByText(value) == null)
                {
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalThird.Items.Add(new ListItem(value, numericValue));
                }
                ddlESPSDTCODEPRofessionalThird.Items.FindByText(value).Selected = true;
                EPSDTConditionCodeThirdDisplay = value;
            }
            hdnEPSDTCodeProfessionalThird.Value = value;
        }
    }
    public string EPSDTConditionCodeThirdDisplay
    {
        get
        {
            return getCodeFromEPSDValue(hdnEPSDTCodeProfessionalThird.Value);
        }
        set
        {
            if (!string.IsNullOrEmpty(value)) {
                if (ddlESPSDTCODEPRofessionalThird.Items.FindByText(value.ToString().Trim()) != null)
                    ddlESPSDTCODEPRofessionalThird.Items.FindByText(value).Selected = true;
                else
                {
                    string numericValue = getNumericEPSDValue(value.Trim());
                    ddlESPSDTCODEPRofessionalThird.Items.Add(new ListItem(value.Trim(), numericValue));
                    ddlESPSDTCODEPRofessionalThird.Items.FindByText(value).Selected = true;
                }
            }
            hdnEPSDTCodeProfessionalThird.Value = value;
        }
    }
    public string HospitalDischargeDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProfessionalHospitalDischargeDate.Text))
                return txtProfessionalHospitalDischargeDate.Text;

            else
                return string.Empty;
        }
        set
        { txtProfessionalHospitalDischargeDate.Text = value; }
    }
    public string SpecialProgramIndicator
    {
        get
        {
            if (ddlSpcPrgmIndProfessional.SelectedItem != null && !string.IsNullOrEmpty(ddlSpcPrgmIndProfessional.SelectedItem.Text))
                return ddlSpcPrgmIndProfessional.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlSpcPrgmIndProfessional.SelectedItem.Text = value.Trim();
        }
    }
    public string SpecialProgramIndicatorDisplay
    {
        get
        {
            if (ddlSpcPrgmIndProfessional.SelectedItem != null && !string.IsNullOrEmpty(ddlSpcPrgmIndProfessional.SelectedItem.Text))
                return ddlSpcPrgmIndProfessional.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlSpcPrgmIndProfessional.Items.FindByText(value.ToString().Trim()) != null)
                ddlSpcPrgmIndProfessional.SelectedItem.Text = value.Trim();
        }
    }

    public string LastMenstrualPeriod
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProfessionalLstmenstural.Text))
                return txtProfessionalLstmenstural.Text;

            else
                return string.Empty;
        }
        set { txtProfessionalLstmenstural.Text = value; }
    }
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimId.Value))
                return hdnClaimId.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimId.Value = value.Trim();
        }
    }
    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimType.Value))
                return hdnClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimType.Value = value.Trim();
        }
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(ddlProfessionalDentalReleaseofInfo.SelectedItem.Text) || !string.IsNullOrWhiteSpace(ddlProfessionalEPSDTCondition.SelectedItem.Text)
            || !string.IsNullOrWhiteSpace(txtProfessionalPatientAmountPaid.Text) || !string.IsNullOrWhiteSpace(txtProfessionalHospitalDischargeDate.Text) || !string.IsNullOrWhiteSpace(ddlSpcPrgmIndProfessional.SelectedItem.Text)
            || !string.IsNullOrWhiteSpace(txtProfessionalLstmenstural.Text))
        {
            rtn = true;
        }

        return rtn;
    }


    #endregion
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    #endregion
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
    public string Claim_Id { get; set; }
    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimId.Value);
            DataSet dsDateserviceDetailinformation = ClaimsController.SelectPanelsData("claims_service_information_professional", parms);

        }
        if (!string.IsNullOrEmpty(Claim_Id))
        {
            hdnClaimId.Value = Claim_Id;
        }
        if (!Page.IsPostBack)
        {
            GetDentalServiceInformationDetails();
        }
        GetEPSDTConditionSource();
        GetSpecialProgramIndicator();
        DropDwonEnableCheck();

        ddlProfessionalDentalReleaseofInfo.Style.Add("background-color", "white");
        releaseInformationError.Text = "";


    }

    protected void Page_PreRender(object sender, EventArgs e)
    {

        string txtToday = DateTime.Now.ToShortDateString();

        cpVldtProfessionalLstMenPrd.ValueToCompare = txtToday;
        //cvProfessionalHospitalDischargeDate.ValueToCompare = txtToday;

        GetServisedetailsData();


    }
    public void GetDentalServiceInformationDetails()
    {

        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_id", hdnClaimId.Value);
        List<SqlParameter> parameters = new List<SqlParameter>();
        foreach (KeyValuePair<string, string> pair in parms)
        {
            if (string.IsNullOrEmpty(pair.Value))
            {
                parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
            }
            else
            {
                parameters.Add(new SqlParameter(pair.Key, pair.Value));
            }
        }
        try
        {
            DataSet dsProffServiceInformation = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Service_Information_Professional", parameters, "claims_service_information_professional");

            if (Helper.HasRows(dsProffServiceInformation) &&
            Convert.ToInt32(dsProffServiceInformation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["Release_of_Information"].ToString()))
                {
                    // OHPNM-15912 something weird happening where this selection is cleared in some circumstance.  Only set the SelectedIndex if the SelectedIndex would not be the default
                    if (ddlProfessionalDentalReleaseofInfo.Items.IndexOf(ddlProfessionalDentalReleaseofInfo.Items.FindByValue(dsProffServiceInformation.Tables[0].Rows[0]["Release_of_Information"].ToString())) >= 1)
                    {
                        ddlProfessionalDentalReleaseofInfo.SelectedIndex = ddlProfessionalDentalReleaseofInfo.Items.IndexOf(ddlProfessionalDentalReleaseofInfo.Items.FindByValue(dsProffServiceInformation.Tables[0].Rows[0]["Release_of_Information"].ToString()));
                    }
                }
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code1"].ToString()))
                {
                    ddlESPSDTCODEPRofessionalFirst.SelectedIndex = ddlESPSDTCODEPRofessionalFirst.Items.IndexOf(ddlESPSDTCODEPRofessionalFirst.Items.FindByText(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code1"].ToString()));
                }

                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code2"].ToString()))
                {
                    ddlESPSDTCODEPRofessionalSecond.SelectedIndex = ddlESPSDTCODEPRofessionalSecond.Items.IndexOf(ddlESPSDTCODEPRofessionalSecond.Items.FindByText(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code2"].ToString()));
                }
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code3"].ToString()))
                {
                    ddlESPSDTCODEPRofessionalThird.SelectedIndex = ddlESPSDTCODEPRofessionalThird.Items.IndexOf(ddlESPSDTCODEPRofessionalThird.Items.FindByText(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Code3"].ToString()));
                }
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["Special_Program_Indicator"].ToString()))
                {
                    ddlSpcPrgmIndProfessional.SelectedIndex = ddlSpcPrgmIndProfessional.Items.IndexOf(ddlSpcPrgmIndProfessional.Items.FindByText(dsProffServiceInformation.Tables[0].Rows[0]["Special_Program_Indicator"].ToString()));
                }
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Indicator"].ToString()))
                {
                    ddlProfessionalEPSDTCondition.SelectedIndex = ddlProfessionalEPSDTCondition.Items.IndexOf(ddlProfessionalEPSDTCondition.Items.FindByValue(dsProffServiceInformation.Tables[0].Rows[0]["EPSDT_Condition_Indicator"].ToString()));

                }
                if (!string.IsNullOrEmpty(dsProffServiceInformation.Tables[0].Rows[0]["Special_Program_Indicator"].ToString()))
                {
                    ddlSpcPrgmIndProfessional.SelectedIndex = ddlSpcPrgmIndProfessional.Items.IndexOf(ddlSpcPrgmIndProfessional.Items.FindByText(dsProffServiceInformation.Tables[0].Rows[0]["Special_Program_Indicator"].ToString()));
                }

                if (!DBNull.Value.Equals(dsProffServiceInformation.Tables[0].Rows[0]["Hospital_Discharge_Date"]))
                {
                    txtProfessionalHospitalDischargeDate.Text = Convert.ToDateTime(dsProffServiceInformation.Tables[0].Rows[0]["Hospital_Discharge_Date"]).ToString("MM/dd/yyyy");
                }
                else
                {
                    txtProfessionalHospitalDischargeDate.Text = string.Empty;
                }
                if (!DBNull.Value.Equals(dsProffServiceInformation.Tables[0].Rows[0]["Last_Menstrual_Period"]))
                {
                    txtProfessionalLstmenstural.Text = Convert.ToDateTime(dsProffServiceInformation.Tables[0].Rows[0]["Last_Menstrual_Period"]).ToString("MM/dd/yyyy");
                }
                else
                {
                    txtProfessionalLstmenstural.Text = string.Empty;
                }
                lblPlcServiceProfessional.Text = dsProffServiceInformation.Tables[0].Rows[0]["Place_of_Service"].ToString();
                txtProfessionalPatientAmountPaid.Text = dsProffServiceInformation.Tables[0].Rows[0]["Patient_Amount_Paid"].ToString();

            }
            //isLoaded = true;




        }
        catch (Exception ex) { }




    }
    private void AssignValidationSummary(string validationSummary)
    {
        // rfvddlProfessionalReleaseOfINformation.ValidationGroup = validationSummary;
    }
    private void SaveToDb()
    {
        string logHeader = string.Format("Professional Service information save data in Database");
        log = new Logging();

        if ((!string.IsNullOrEmpty(hdnClaimId.Value)) && (ddlProfessionalDentalReleaseofInfo.SelectedIndex >= 0))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnClaimId.Value);
            List<SqlParameter> parameters = new List<SqlParameter>();

            foreach (KeyValuePair<string, string> pair in parms)
            {
                if (string.IsNullOrEmpty(pair.Value))
                {
                    parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                }
                else
                {
                    parameters.Add(new SqlParameter(pair.Key, pair.Value));
                }
            }


            DataSet dsServiceInformation = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Service_Information_Professional", parameters, "claims_service_information_professional");
            string tableName = "claims_service_information_professional";
            if (Helper.HasRows(dsServiceInformation) &&
                Convert.ToInt32(dsServiceInformation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                parms.Add("Claims_Service_Information_Professional_ID", dsServiceInformation.Tables[0].Rows[0]["Claims_Service_Information_Professional_ID"].ToString());
                parms.Add("Release_of_Information", ddlProfessionalDentalReleaseofInfo.SelectedValue);
                parms.Add("Place_of_Service", string.IsNullOrEmpty(lblPlcServiceProfessional.Text) ? null : lblPlcServiceProfessional.Text);
                parms.Add("Patient_Amount_Paid", string.IsNullOrEmpty(txtProfessionalPatientAmountPaid.Text) ? null : txtProfessionalPatientAmountPaid.Text);
                if (ddlESPSDTCODEPRofessionalFirst.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code1", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text);
                }
                else
                {
                    parms.Add("EPSDT_Condition_Code1", null);

                }
                if (ddlESPSDTCODEPRofessionalSecond.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code2", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text);
                }
                else
                {
                    parms.Add("EPSDT_Condition_Code2", null);

                }
                if (ddlESPSDTCODEPRofessionalThird.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code3", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalThird.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalThird.SelectedItem.Text);
                }
                else { parms.Add("EPSDT_Condition_Code3", null); }
                parms.Add("EPSDT_Condition_Indicator", ddlProfessionalEPSDTCondition.SelectedValue);
                if (ddlSpcPrgmIndProfessional.Items.Count > 0)
                {
                    parms.Add("Special_Program_Indicator", string.IsNullOrEmpty(ddlSpcPrgmIndProfessional.SelectedItem.Text) ? null : ddlSpcPrgmIndProfessional.SelectedItem.Text);
                }
                else {
                    parms.Add("Special_Program_Indicator", null);
                }
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Hospital_Discharge_Date", txtProfessionalHospitalDischargeDate.Text);
                parms.Add("Last_Menstrual_Period", txtProfessionalLstmenstural.Text);
                try
                {
                    svc.UpdatePanelsData(tableName, parms);
                }
                catch (Exception ex)
                {
                    log.CreateLogEntry("Professional Service information Update Data in Database for claim id " + hdnClaimId.Value + ": " + ex.ToString());
                }

                ClearServiceInformationDentalFields();
                GetDentalServiceInformationDetails();
            }
            else if (!string.IsNullOrEmpty(hdnClaimId.Value))
            {
                string PatientAmountPaid = null;
                if (!string.IsNullOrEmpty(txtProfessionalPatientAmountPaid.Text))
                {
                    decimal AmountPaid = Convert.ToDecimal(txtProfessionalPatientAmountPaid.Text);
                    PatientAmountPaid = Convert.ToString(Math.Round(AmountPaid));
                }
                parms.Add("Release_of_Information", ddlProfessionalDentalReleaseofInfo.SelectedValue);
                parms.Add("Place_of_Service", string.IsNullOrEmpty(lblPlcServiceProfessional.Text) ? null : lblPlcServiceProfessional.Text);
                parms.Add("Patient_Amount_Paid", string.IsNullOrEmpty(txtProfessionalPatientAmountPaid.Text) ? null : PatientAmountPaid);
                if (ddlESPSDTCODEPRofessionalFirst.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code1", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text);
                }
                else { parms.Add("EPSDT_Condition_Code1", null); }
                if (ddlESPSDTCODEPRofessionalSecond.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code2", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text);
                }
                else { parms.Add("EPSDT_Condition_Code2", null); }

                if (ddlESPSDTCODEPRofessionalThird.SelectedIndex >= 0)
                {
                    parms.Add("EPSDT_Condition_Code3", string.IsNullOrEmpty(ddlESPSDTCODEPRofessionalThird.SelectedItem.Text) ? null : ddlESPSDTCODEPRofessionalThird.SelectedItem.Text);
                }
                else { parms.Add("EPSDT_Condition_Code3", null); }
                parms.Add("EPSDT_Condition_Indicator", ddlProfessionalEPSDTCondition.SelectedValue);
                if (ddlSpcPrgmIndProfessional.SelectedIndex >= 0)
                {
                    parms.Add("Special_Program_Indicator", string.IsNullOrEmpty(ddlSpcPrgmIndProfessional.SelectedItem.Text) ? null : ddlSpcPrgmIndProfessional.SelectedItem.Text);
                } else
                {
                    parms.Add("Special_Program_Indicator", null);
                }
                parms.Add("Created_Date_Time", string.IsNullOrEmpty(DateTime.Now.ToString()) ? null : DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Hospital_Discharge_Date", txtProfessionalHospitalDischargeDate.Text);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Menstrual_Period", txtProfessionalLstmenstural.Text);


                try
                {
                    svc.InsertPanelsData(tableName, parms);
                }
                catch (Exception ex)
                {
                    log.CreateLogEntry("Professional Service information Insert Data in Database for claim id " + hdnClaimId.Value + ": " + ex.ToString());
                }
                ClearServiceInformationDentalFields();
                GetDentalServiceInformationDetails();
            }
        }
    }
    public void SaveServiceInformation(string validationSummary, int actionButton)
    {
        if (actionButton == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                SaveToDb();
            }
        }
        else
        {
            SaveToDb();
        }
    }

    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {


        if (!string.IsNullOrEmpty(txtProfessionalHospitalDischargeDate.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate20();</script>", false);
        }
        else
        {
            ProffHospDateRequiredError1.Visible = false;
        }
    }

    public void ClearServiceInformationDentalFields()
    {

        // OHPNM-15912 leaving this for now, as this appears to only be called on DB Save
        // ddlProfessionalDentalReleaseofInfo.SelectedIndex = 0;
        ddlESPSDTCODEPRofessionalFirst.ClearSelection();
        ddlESPSDTCODEPRofessionalSecond.ClearSelection();
        ddlESPSDTCODEPRofessionalThird.ClearSelection();
        if (ddlSpcPrgmIndProfessional.Items.Count > 0)
        {
            ddlSpcPrgmIndProfessional.SelectedIndex = 0;
        }
        ddlProfessionalEPSDTCondition.SelectedIndex = 0;
        txtProfessionalPatientAmountPaid.Text = string.Empty;
        lblPlcServiceProfessional.Text = string.Empty;
        txtProfessionalHospitalDischargeDate.Text = string.Empty;
        txtProfessionalLstmenstural.Text = string.Empty;
        txtProfessionalHospitalDischargeDate.Text = string.Empty;
    }


    private void GetEPSDTConditionSource()
    {
        if (fromInquirySvc)
            return;

        string firstDropdownValue = EPSDTConditionCodeFirstDisplay;
        string secondDropdownValue = EPSDTConditionCodeSecondDisplay;
        string thirdDropdownValue = EPSDTConditionCodeThirdDisplay;
        ddlESPSDTCODEPRofessionalFirst.Items.Clear();
        ddlESPSDTCODEPRofessionalSecond.Items.Clear();
        ddlESPSDTCODEPRofessionalThird.Items.Clear();

        DataSet dataSet = LookupTableController.GetEPSDTCondition();
        DataTable dt = dataSet.Tables[0];

        var row_EPSDTCondition = from row in dt.AsEnumerable()
                                 where
                                 row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 6
                                 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 7 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 8
                                 select row;
        DataTable dt_EPSDTConditionFilter = row_EPSDTCondition.CopyToDataTable();
        Helper.LoadList(ddlESPSDTCODEPRofessionalFirst, dt_EPSDTConditionFilter, "PRIOR_AUTH_CLAIM_EPSDT_CODE", "PRIOR_AUTH_CLAIM_EPSDT_ID", true);

        if (!String.IsNullOrEmpty(firstDropdownValue))
        {
            if (ddlESPSDTCODEPRofessionalFirst.Items.FindByText(firstDropdownValue) != null)
            {
                ddlESPSDTCODEPRofessionalFirst.Items.FindByText(firstDropdownValue.Trim()).Selected = true;
            }
            else
            {
                string numericValue = getNumericEPSDValue(firstDropdownValue);
                ddlESPSDTCODEPRofessionalFirst.Items.Add(new ListItem(firstDropdownValue.Trim(), numericValue));
                ddlESPSDTCODEPRofessionalFirst.Items.FindByText(firstDropdownValue.Trim()).Selected = true;
            }
            hdnEPSDTCodeProfessionalFirst.Value = firstDropdownValue;
        }
        if (!String.IsNullOrEmpty(secondDropdownValue))
        {
            if (ddlESPSDTCODEPRofessionalSecond.Items.FindByText(secondDropdownValue) != null)
            {
                ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text = secondDropdownValue;
            } else
            {
                string numericValue = getNumericEPSDValue(secondDropdownValue);
                ddlESPSDTCODEPRofessionalSecond.Items.Add(new ListItem(secondDropdownValue.Trim(), numericValue));
                ddlESPSDTCODEPRofessionalSecond.Items.FindByText(secondDropdownValue.Trim()).Selected = true;
            }
            hdnEPSDTCodeProfessionalSecond.Value = secondDropdownValue;
        }
        if (!String.IsNullOrEmpty(thirdDropdownValue))
        {
            if (ddlESPSDTCODEPRofessionalSecond.Items.FindByText(thirdDropdownValue) != null)
            {
                ddlESPSDTCODEPRofessionalThird.SelectedItem.Text = thirdDropdownValue;
            } else
            {
                string numericValue = getNumericEPSDValue(thirdDropdownValue);
                ddlESPSDTCODEPRofessionalThird.Items.Add(new ListItem(thirdDropdownValue.Trim(), numericValue));
                ddlESPSDTCODEPRofessionalSecond.Items.FindByText(secondDropdownValue.Trim()).Selected = true;
            }
            hdnEPSDTCodeProfessionalThird.Value = thirdDropdownValue;
        }
    }

    public DataTable GetSecondDropdown()
    {
        DataSet dataSet = LookupTableController.GetEPSDTCondition();
        DataTable dt = dataSet.Tables[0];

        var row_EPSDTCondition = from row in dt.AsEnumerable()
                                 where
                                 row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 6
                                 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 7 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 8
                                 select row;
        DataTable dt_EPSDTConditionFilter = row_EPSDTCondition.CopyToDataTable();
        for (int i = dt_EPSDTConditionFilter.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = dt_EPSDTConditionFilter.Rows[i];
            if (Convert.ToString(dr["PRIOR_AUTH_CLAIM_EPSDT_CODE"]) == ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text.ToString())
            {
                dr.Delete();
            }
        }
        dt_EPSDTConditionFilter.AcceptChanges();


        return dt_EPSDTConditionFilter;
    }
    public DataTable GetThirdDropdown()
    {

        DataSet dataSet = LookupTableController.GetEPSDTCondition();
        DataTable dt = dataSet.Tables[0];

        string firstValue = EPSDTConditionCodeFirstDisplay;
        string secondValue = EPSDTConditionCodeSecondDisplay;

        if (firstValue.Equals("S2"))
        {
            firstValue = "5";
        }
        if (firstValue.Equals("ST"))
        {
            firstValue = "6";
        }
        if (firstValue.Equals("NU"))
        {
            firstValue = "7";
        }
        if (firstValue.Equals("AV"))
        {
            firstValue = "8";
        }
        if (secondValue.Equals("S2"))
        {
            secondValue = "5";
        }
        if (secondValue.Equals("ST"))
        {
            secondValue = "6";
        }
        if (secondValue.Equals("NU"))
        {
            secondValue = "7";
        }
        if (secondValue.Equals("AV"))
        {
            secondValue = "8";
        }


        var row_EPSDTCondition = from row in dt.AsEnumerable()
                                 where
                                 row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 6
                                 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 7 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 8 && row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") != Convert.ToInt32(firstValue)
                                 && row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") != Convert.ToInt32(secondValue)
                                 select row;
        DataTable dt_EPSDTConditionFilter = row_EPSDTCondition.CopyToDataTable();

        for (int i = dt_EPSDTConditionFilter.Rows.Count - 1; i >= 0; i--)
        {
            DataRow dr = dt_EPSDTConditionFilter.Rows[i];
            if (Convert.ToString(dr["PRIOR_AUTH_CLAIM_EPSDT_CODE"]) == ddlESPSDTCODEPRofessionalFirst.SelectedItem.Text.ToString()
                || Convert.ToString(dr["PRIOR_AUTH_CLAIM_EPSDT_CODE"]) == ddlESPSDTCODEPRofessionalSecond.SelectedItem.Text.ToString())
            {
                dr.Delete();
            }
        }
        dt_EPSDTConditionFilter.AcceptChanges();

        return dt_EPSDTConditionFilter;


    }

    protected void dropdownFirst_Selected_IndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EPSDTConditionCodeFirstDisplay))
        {
            ddlESPSDTCODEPRofessionalSecond.Items.Clear();
            ddlESPSDTCODEPRofessionalThird.Items.Clear();
            Helper.LoadList(ddlESPSDTCODEPRofessionalSecond, GetSecondDropdown(), "PRIOR_AUTH_CLAIM_EPSDT_CODE", "PRIOR_AUTH_CLAIM_EPSDT_ID", true);
        }
        else
        {
            ddlESPSDTCODEPRofessionalSecond.Items.Clear();
            ddlESPSDTCODEPRofessionalThird.Items.Clear();
        }
    }
    protected void dropdownSecond_Selected_IndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(EPSDTConditionCodeFirstDisplay) && !string.IsNullOrWhiteSpace(EPSDTConditionCodeSecondDisplay))
        {
            ddlESPSDTCODEPRofessionalThird.Items.Clear();
            Helper.LoadList(ddlESPSDTCODEPRofessionalThird, GetThirdDropdown(), "PRIOR_AUTH_CLAIM_EPSDT_CODE", "PRIOR_AUTH_CLAIM_EPSDT_ID", true);
        }
        else
        {
            ddlESPSDTCODEPRofessionalSecond.Items.Clear();
            ddlESPSDTCODEPRofessionalThird.Items.Clear();
        }
    }

    public void GetSpecialProgramIndicator()
    {


        DataSet dataSet = LookupTableController.GetSpecialProgramIndicator();
        DataTable dt = dataSet.Tables[0];

        Helper.LoadList(ddlSpcPrgmIndProfessional, dt, "Claims_Special_Program_Indicator_Description", "Claims_Special_Program_Indicator_ID", true);



    }



    public void DropDwonEnableCheck()
    {
        if (ddlProfessionalEPSDTCondition.SelectedIndex > 0)
        {
            ddlESPSDTCODEPRofessionalFirst.Enabled = true;
            ddlESPSDTCODEPRofessionalFirst.BackColor = System.Drawing.Color.White;
            ddlESPSDTCODEPRofessionalSecond.Enabled = true;
            ddlESPSDTCODEPRofessionalSecond.BackColor = System.Drawing.Color.White;
            ddlESPSDTCODEPRofessionalThird.Enabled = true;
            ddlESPSDTCODEPRofessionalThird.BackColor = System.Drawing.Color.White;

        }
        else
        {
            ddlESPSDTCODEPRofessionalFirst.BackColor = System.Drawing.Color.LightGray;
            ddlESPSDTCODEPRofessionalFirst.Enabled = false;
            ddlESPSDTCODEPRofessionalSecond.BackColor = System.Drawing.Color.LightGray;
            ddlESPSDTCODEPRofessionalSecond.Enabled = false;
            ddlESPSDTCODEPRofessionalThird.BackColor = System.Drawing.Color.LightGray;
            ddlESPSDTCODEPRofessionalThird.Enabled = false;
        }
    }


    protected void ddlProfessionalEPSDTCondition_SelectedIndexChanged(object sender, EventArgs e)
    {

        DropDwonEnableCheck();
    }

    public void GetServisedetailsData()
    {

        if (ClaimType == CON.ClaimsType.Professional)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            DataTable claimsDataTable = new DataTable();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
            DataSet serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
            claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
            if (Helper.HasRows(claimsDataTable) && (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Place_of_Service"].ToString())))
            {
                if (Helper.HasRows(claimsDataTable))
                {
                    String placeofservice = claimsDataTable.Rows[0].Field<String>("Place_of_Service");
                    if (placeofservice != "0")
                    {
                        lblPlcServiceProfessional.Text = placeofservice.ToString();

                    }
                    else
                    {
                        lblPlcServiceProfessional.Text = string.Empty;
                    }

                }
            }
        }
    }

}