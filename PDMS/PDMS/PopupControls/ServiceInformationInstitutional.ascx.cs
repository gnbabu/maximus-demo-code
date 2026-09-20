using AjaxControlToolkit.HtmlEditor.ToolbarButtons;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ServiceInformationInstitutional : System.Web.UI.UserControl
{
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
    #region " Properties "
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtAdmissionDate.ReadOnly = value;
        txtAdmissionHr.ReadOnly = value;
        txtDischargeHr.ReadOnly = value;
        txtFromDate.ReadOnly = value;
        txtPatPaidAmt.ReadOnly = value;
        txtSubmittedDRG.ReadOnly = value;
        txtToDate.ReadOnly = value;
        txtTypeofBill.ReadOnly = value;
        ddlAdmissionType.Enabled = !value;
        ddlAdmitSource.Enabled = value;
        ddlPatientStatus.Enabled = !value;
        ddlReleaseOfInfo.Enabled = !value;
        ceToDate.Enabled = !value;
        ceFromDate.Enabled = !value;
        lnkPlaceofServiceSearch.Visible = !value;
        lnkPlaceofServiceSearch.Enabled = !value;
    }
    public string AdmissionDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAdmissionDate.Text))
                return txtAdmissionDate.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtAdmissionDate.Text = value;
        }
    }
    public string AdmissionHour
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtAdmissionHr.Text))
                return txtAdmissionHr.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtAdmissionHr.Text = value;
        }
    }
    public string DischargeHour
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtDischargeHr.Text))
                return txtDischargeHr.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtDischargeHr.Text = value;
        }
    }

    public void AdmitSourceEnableStatus()
    {
        if (!string.IsNullOrEmpty(AdmissionType) && (ddlAdmissionType.Enabled))
        {
            ddlAdmitSource.Enabled = true;
        }
        else
        { ddlAdmitSource.Enabled = false; }
    }
    public string AdmitSource
    {
        get
        {
            if (!string.IsNullOrEmpty(ddlAdmitSource.SelectedItem.Text))
                return ddlAdmitSource.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            { 
                GetAdmitSource();
                if (!string.IsNullOrEmpty(AdmissionType) &&(ddlAdmissionType.Enabled))
                { ddlAdmitSource.Enabled = true; }
                else
                { ddlAdmitSource.Enabled = false; }
                if (value.Length == 1)
                {
                    ddlAdmitSource.SelectedValue = value.Trim();

                }
                else
                {
                    ddlAdmitSource.SelectedItem.Text = value.Trim();

                }
               // ddlAdmitSource.Enabled = SetReadOnlyFields;
            }
        }
            
    }
    public string AdmissionType
    {
        get
        {
            if (ddlAdmissionType.SelectedItem != null && !string.IsNullOrEmpty(ddlAdmissionType.SelectedValue))
                return ddlAdmissionType.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                    ddlAdmissionType.SelectedValue=value.Trim();
            }
        }
    }
    public string ReleaseofInformation
    {
        get
        {
            if (ddlReleaseOfInfo.SelectedItem != null && !string.IsNullOrEmpty(ddlReleaseOfInfo.SelectedValue))
                return ddlReleaseOfInfo.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlReleaseOfInfo.Items.FindByValue(value.ToString().Trim()) != null)
            {
                ddlReleaseOfInfo.ClearSelection();
                ddlReleaseOfInfo.SelectedValue = value.Trim();
            }
        }
    }


    public string ReleaseofInformationDisplay
    {
        get
        {
            if (ddlReleaseOfInfo.SelectedItem != null && !string.IsNullOrEmpty(ddlReleaseOfInfo.SelectedItem.Text))
                return ddlReleaseOfInfo.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlReleaseOfInfo.Items.FindByText(value.ToString().Trim()) != null)
            {
                ddlReleaseOfInfo.ClearSelection();
                ddlReleaseOfInfo.SelectedItem.Text = value.Trim();
            }
        }
    }

    public string TypeofBill
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtTypeofBill.Text))
                return txtTypeofBill.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtTypeofBill.Text = value;
        }
    }
    public string FromDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtFromDate.Text))
                return txtFromDate.Text;

            else
                return string.Empty;
        }
        set { txtFromDate.Text = value; }
    }
    public string Todate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtToDate.Text))
                return txtToDate.Text;

            else
                return string.Empty;
        }
        set { txtToDate.Text = value; }
    }

    public string PatientPaidAmount
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPatPaidAmt.Text))
                return txtPatPaidAmt.Text;

            else
                return string.Empty;
        }
        set
        { txtPatPaidAmt.Text = value; }
    }
    public string PatientStatus
    {
        get
        {
            if (ddlPatientStatus.SelectedItem != null && !string.IsNullOrEmpty(ddlPatientStatus.SelectedValue))
                return ddlPatientStatus.SelectedValue;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                GetPatientStatus(false);
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    int counter = 0;
                    foreach (ListItem li in ddlPatientStatus.Items)
                    {
                        if (li.Value.Equals(value))
                        {
                            ddlPatientStatus.SelectedIndex = counter;
                            break;
                        }
                        counter++;
                    }
                }
            }
        }
    }
    public string PatientStatusDisplay
    {
        get
        {
            if (ddlPatientStatus.SelectedItem != null && !string.IsNullOrEmpty(ddlPatientStatus.SelectedItem.Text))
                return ddlPatientStatus.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlPatientStatus.Items.FindByText(value.ToString().Trim()) != null)
            {
                ddlPatientStatus.ClearSelection();
                ddlPatientStatus.SelectedItem.Text = value.Trim();
            }
        }
    }

    public string SubmittedDRG
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtSubmittedDRG.Text))
                return txtSubmittedDRG.Text;

            else
                return string.Empty;
        }
        set { txtSubmittedDRG.Text = value; }
    }
    public string FinalDRG
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblFinalDRG.Text))
                return lblFinalDRG.Text;

            else
                return string.Empty;
        }
        set { lblFinalDRG.Text = value; }
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
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtTypeofBill.Text) || !string.IsNullOrEmpty(txtFromDate.Text) || !string.IsNullOrWhiteSpace(txtToDate.Text)
            || !string.IsNullOrWhiteSpace(ddlReleaseOfInfo.SelectedItem.Text) || !string.IsNullOrWhiteSpace(ddlPatientStatus.SelectedItem.Text)
            || !string.IsNullOrWhiteSpace(ddlAdmissionType.SelectedItem.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    public string ClaimFrequencyCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimFrequencyCode.Value))
                return hdnClaimFrequencyCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimFrequencyCode.Value = value.Trim();
        }
    }


    #endregion
    public void HideDivsForReSubmitCopyandAdjust()
    {
        divFinalDRG.Visible = false;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetAdmissionType();
            GetAdmitSource();
            GetPatientStatus();
            GetInstitutionalServiceInformationDetails();

        }
        if (!string.IsNullOrEmpty(hdnAdmitSrc.Value.ToString())) 
        { 
            ddlAdmitSource.SelectedItem.Text = hdnAdmitSrc.Value.ToString(); 
        }
        
            releaseInformationError.Text = "";
            ddlReleaseOfInfo.Style.Add("background-color", "white");
        
            fromDateRequiredError.Text = "";
            txtFromDate.Style.Add("background-color", "white");
        
            toDateRequiredError.Text = "";
            txtToDate.Style.Add("background-color", "white");
       
            patientStatusError.Text = "";
            ddlPatientStatus.Style.Add("background-color", "white");
        
            admissionTypeError.Text = "";
            ddlAdmissionType.Style.Add("background-color", "white");
        
            admitsourceError.Text = "";
            ddlAdmitSource.Style.Add("background-color", "white");
       
            lblServiceInfoError.Text = "";                              // making error message label clear
            txtAdmissionDate.Style.Add("background-color", "white");    // making gold highlighted selection textbox to white

            lblInstTypeBill.Text = "";
            txtTypeofBill.Style.Add("background-color", "white"); 
    }


    protected void ReportToDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtToDate.Text, true) && Helper.IsValidDate(txtToDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtToDate.Text).Subtract(Convert.ToDateTime(txtToDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }
    protected void ReportFromDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtFromDate.Text, true) && Helper.IsValidDate(txtFromDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtFromDate.Text).Subtract(Convert.ToDateTime(txtFromDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    /// <summary>
    /// This Method will get AdmissionType
    /// </summary>
    ///
    private void GetAdmissionType()
    {


        ddlAdmissionType.Items.Clear();
        DataSet dataSet = LookupTableController.GetClaimsAdmissionType();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlAdmissionType, dt, "Claims_Admission_type_Description", "Claims_Admission_Type_code", true);

    }
    private void GetPatientStatus(bool fromSet = true)
    {
        ddlPatientStatus.Items.Clear();
        DataSet dataSet = LookupTableController.GetPatientStatus();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlPatientStatus, dt, "Claims_Patient_Status_Description", "Claims_Patient_Status_Code", fromSet);
    }

    /// <summary>
    /// This Method will get AdmitSource
    /// </summary>
    ///
    private void GetAdmitSource()
    {


        ddlAdmitSource.Items.Clear();
        DataSet dataSet = LookupTableController.GetClaimAdmitSource();
        DataTable dt = dataSet.Tables[0];
        if (ddlAdmissionType.SelectedValue == "4")
        {
            var row_EPSDTCondition = from row in dt.AsEnumerable()
                                     where
                                     row.Field<int>("Claims_Admit_Source_ID") == 10 || row.Field<int>("Claims_Admit_Source_ID") == 11
                                     select row;
            DataTable dt_AddmissionSourseFilter = row_EPSDTCondition.CopyToDataTable();
            Helper.LoadList(ddlAdmitSource, dt_AddmissionSourseFilter, "Claims_Admit_Source_Description", "Claims_Admit_Source_Code", true);

        }
        else
        {
            var row_EPSDTCondition = from row in dt.AsEnumerable()
                                     where
                                     row.Field<int>("Claims_Admit_Source_ID") != 10  && row.Field<int>("Claims_Admit_Source_ID") != 11
                                     select row;
            DataTable dt_AddmissionSourseFilter = row_EPSDTCondition.CopyToDataTable();
            Helper.LoadList(ddlAdmitSource, dt_AddmissionSourseFilter, "Claims_Admit_Source_Description", "Claims_Admit_Source_Code", true);
            //Helper.LoadList(ddlAdmitSource, dt, "Claims_Admit_Source_Description", "Claims_Admit_Source_ID", true);

        }

    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valServiceInformationInstitutional";
        this.Page.Validators.Add(val);
        //isGood = false;
        return false;
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        string txtToday = DateTime.Now.ToShortDateString();
     //   cvFromDateService.ValueToCompare = txtToday;
       // cvToDateService.ValueToCompare = txtToday;
        if (Session["TypeBillCode"] != null)
        {
            txtTypeofBill.Text = Session["TypeBillCode"].ToString();
            lblInstErr.Text = lblInstTypeBill.Text = "";
            typeOfBillNumberOfDigitsError.Visible = false;
        }
    }
    private bool ValidateServiceInformation()
    {
        lblFromToDate.Text = "";
        fromDateRequiredError.Text = "";
        toDateRequiredError.Text = "";
        bool isValid = true;
        //if (txtTypeofBill.Text == CON.ClaimsTypeOfBill.TypeOfBill0111 && string.IsNullOrWhiteSpace(txtDischargeHr.Text))
        //{
        //    AddValidationErrorMessage("*Discharge hour is required");
        //    isValid = false;
        //}
        if (string.IsNullOrWhiteSpace(txtToDate.Text))
        {
            toDateRequiredError.Text = "*TO date is required";

            isValid = false;
        }
        if (string.IsNullOrWhiteSpace(txtFromDate.Text))
        {
            fromDateRequiredError.Text = "*From Date is Required";

            isValid = false;
        }
        //OHPNM-16159-Institutional Claims- Admission date only required for inpatient claims, not required for outpatient claims.
        if ( (txtTypeofBill.Text.StartsWith("011") || txtTypeofBill.Text.StartsWith("012") || txtTypeofBill.Text.StartsWith("018") || txtTypeofBill.Text.StartsWith("021") || txtTypeofBill.Text.StartsWith("022") || txtTypeofBill.Text.StartsWith("028") || txtTypeofBill.Text.StartsWith("041") || txtTypeofBill.Text.StartsWith("065") || txtTypeofBill.Text.StartsWith("066") || txtTypeofBill.Text.StartsWith("086")) && ( string.IsNullOrWhiteSpace(txtAdmissionDate.Text) || string.IsNullOrWhiteSpace(txtAdmissionHr.Text) )  )
        {
            AddValidationErrorMessage("*Admission date and time is required");
            isValid = false;
        }
        if((!string.IsNullOrEmpty(txtAdmissionDate.Text)&&!string.IsNullOrEmpty(txtToDate.Text)))
        {
            if (Convert.ToDateTime((txtAdmissionDate.Text)) > Convert.ToDateTime((txtToDate.Text)))
            {
                //lblFromToDate.Text = "*Admission date and time is invalid(Greater Than ToDate)";
                //isValid = false;
            }
        }      


        //if ((txtTypeofBill.Text.Equals("0111") || txtTypeofBill.Text.Equals("0121") || txtTypeofBill.Text.Equals("0181")) || txtTypeofBill.Text.Equals("0211") || txtTypeofBill.Text.Equals("0211") || txtTypeofBill.Text.Equals("0281") || txtTypeofBill.Text.Equals("0411") || txtTypeofBill.Text.Equals("0651") || txtTypeofBill.Text.Equals("0661") || txtTypeofBill.Text.Equals("0861"))
        //{
        //    if (string.IsNullOrEmpty(txtDischargeHr.Text))
        //    {
        //        lblFromToDate.Text = "*Discharge Hour is required for this Type of bill";
        //        isValid = false;
        //    }
        //    //txtDischargeHr
        //}

        //if (!string.IsNullOrEmpty(txtTypeofBill.Text))
        //{
        //    if (txtTypeofBill.Text.EndsWith("7") || txtTypeofBill.Text.EndsWith("8"))
        //    {
        //        lblFromToDate.Text = "Search and inquire the claim to adjust or void";
        //        isValid = false;
        //    }
        //}
        return isValid;       
    }

    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
       

        if (!string.IsNullOrEmpty(txtFromDate.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate();</script>", false);
        }
        else{
            fromDateFutureDateError.Visible = false;
        }
    }
    protected void txtCheckAdminForDate_TextChanged(object sender, EventArgs e)
    {
        if (ddlReleaseOfInfo.SelectedValue != "0")
       
        {
            releaseInformationError.Text = "";
            ddlReleaseOfInfo.Style.Add("background-color", "white");
        }

        if ((txtFromDate.Text.ToString() != null) && (txtFromDate.Text.ToString() != ""))
        {
            fromDateRequiredError.Text = "";
            txtFromDate.Style.Add("background-color", "white");
        }
        if ((txtToDate.Text.ToString() != null) && (txtToDate.Text.ToString() != ""))
        {
            toDateRequiredError.Text = "";
            txtToDate.Style.Add("background-color", "white");
        }
        if ((ddlPatientStatus.SelectedItem.Text.ToString() != null) && (ddlPatientStatus.SelectedItem.Text.ToString() != ""))
        {
            patientStatusError.Text = "";
            ddlPatientStatus.Style.Add("background-color", "white");
        }
        if ((ddlAdmissionType.SelectedItem.Text.ToString() != null) && (ddlAdmissionType.SelectedItem.Text.ToString() != ""))
        {
            admissionTypeError.Text = "";
            ddlAdmissionType.Style.Add("background-color", "white");
        }
        if (((txtAdmissionDate.Text.ToString() != null) && (txtAdmissionDate.Text.ToString() != "")) && ((txtAdmissionHr.Text.ToString() != null) && (txtAdmissionHr.Text.ToString() != "")))
        { lblServiceInfoError.Text = ""; }
        if (!string.IsNullOrEmpty(txtAdmissionDate.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate22();</script>", false);
        }
        else
        {
            lblFromToDate.Visible = false;
        }
    }

    public void SaveServiceInformation(string validationSummary)
    {
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            if (ValidateServiceInformation())
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
            DataSet dsServiceInformation = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Service_Information_Institutional", parameters,
                "Claims_Service_Information_Institutional");
            string tableName = "Claims_Service_Information_Institutional";
            if (Helper.HasRows(dsServiceInformation) &&
                Convert.ToInt32(dsServiceInformation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {

                parms.Add("Type_of_Bill", string.IsNullOrEmpty(txtTypeofBill.Text) ? null : txtTypeofBill.Text);
                parms.Add("Release_of_Information", string.IsNullOrEmpty(ddlReleaseOfInfo.SelectedValue) ? null : ddlReleaseOfInfo.SelectedValue);
                parms.Add("Admission_Date_Hour", string.IsNullOrEmpty(txtAdmissionDate.Text) ? null : txtAdmissionDate.Text);
                parms.Add("Patient_Paid_Amount", string.IsNullOrEmpty(txtPatPaidAmt.Text) ? null : txtPatPaidAmt.Text);
                parms.Add("From_Date", string.IsNullOrEmpty(txtFromDate.Text) ? null : txtFromDate.Text);
                parms.Add("To_Date", string.IsNullOrEmpty(txtToDate.Text) ? null : txtToDate.Text);
                parms.Add("Patient_Status", string.IsNullOrEmpty(ddlPatientStatus.SelectedValue) ? null : ddlPatientStatus.SelectedValue);
                parms.Add("Admission_type", ddlAdmissionType.SelectedValue);
                parms.Add("Admit_Source", ddlAdmitSource.SelectedItem.Text);
                parms.Add("Submitted_DRG", string.IsNullOrEmpty(txtSubmittedDRG.Text) ? null : txtSubmittedDRG.Text);
                parms.Add("Final_DRG", string.IsNullOrEmpty(lblFinalDRG.Text) ? null : lblFinalDRG.Text);
                parms.Add("Discharge_Hour", string.IsNullOrEmpty(txtDischargeHr.Text) ? null : txtDischargeHr.Text);
                parms.Add("Claims_Service_Information_Institutional_ID", dsServiceInformation.Tables[0].Rows[0]["Claims_Service_Information_Institutional_ID"].ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Admission_Hour", string.IsNullOrEmpty(txtAdmissionHr.Text) ? null : txtAdmissionHr.Text);

                    try
                    {
                        ClaimsController.UpdatePanelsData(tableName, parms);
                    }
                    catch (Exception ex)

                    { }
                    
                    GetInstitutionalServiceInformationDetails();
            }
            else if (!string.IsNullOrEmpty(hdnClaimId.Value))
            {

                parms.Add("Type_of_Bill", string.IsNullOrEmpty(txtTypeofBill.Text) ? null : txtTypeofBill.Text);
                parms.Add("Release_of_Information", string.IsNullOrEmpty(ddlReleaseOfInfo.SelectedValue) ? null : ddlReleaseOfInfo.SelectedValue);
                parms.Add("Admission_Date_Hour", string.IsNullOrEmpty(txtAdmissionDate.Text) ? null : txtAdmissionDate.Text);
                parms.Add("Discharge_Hour", string.IsNullOrEmpty(txtDischargeHr.Text) ? null : txtDischargeHr.Text);
                parms.Add("Patient_Paid_Amount", string.IsNullOrEmpty(txtPatPaidAmt.Text) ? null : txtPatPaidAmt.Text);
                parms.Add("From_Date", string.IsNullOrEmpty(txtFromDate.Text) ? null : txtFromDate.Text);
                parms.Add("To_Date", string.IsNullOrEmpty(txtToDate.Text) ? null : txtToDate.Text);
                parms.Add("Created_Date_Time", string.IsNullOrEmpty(DateTime.Now.ToString()) ? null : DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Patient_Status", string.IsNullOrEmpty(ddlPatientStatus.SelectedValue) ? null : ddlPatientStatus.SelectedValue);
                parms.Add("Admission_type", ddlAdmissionType.SelectedValue);
                parms.Add("Admit_Source", ddlAdmitSource.SelectedItem.Text);
                parms.Add("Submitted_DRG", string.IsNullOrEmpty(txtSubmittedDRG.Text) ? null : txtSubmittedDRG.Text);
                parms.Add("Final_DRG", string.IsNullOrEmpty(lblFinalDRG.Text) ? null : lblFinalDRG.Text);
                parms.Add("Last_Modified_date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Admission_Hour", string.IsNullOrEmpty(txtAdmissionHr.Text) ? null : txtAdmissionHr.Text);

                    try
                    {
                    ClaimsController.InsertPanelsData(tableName, parms);
                }
                    catch (Exception ex)
              
                { }

                }
            }
           

            }
        }


    protected void ddlAdmissionType_SelectedIndexChanged(object sender, EventArgs e)
    {

        GetAdmitSource();
    }


    public void ClearServiceInformationInstitutionalFields()
    {
        txtTypeofBill.Text = string.Empty;
        txtAdmissionHr.Text = string.Empty;
        txtPatPaidAmt.Text = string.Empty;
        txtFromDate.Text = string.Empty;
        txtSubmittedDRG.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtAdmissionHr.Text = string.Empty;
        txtDischargeHr.Text = string.Empty;
        lblFinalDRG.Text = string.Empty;
        txtAdmissionDate.Text = string.Empty;
        GetAdmissionType();
        GetAdmitSource();
        ddlReleaseOfInfo.SelectedIndex = 0;
        ddlPatientStatus.SelectedIndex = 0;
        hdnAdmitSrc.Value = string.Empty;
        ddlAdmitSource.SelectedIndex = 0;
        ddlAdmissionType.SelectedIndex = 0;
        lblFromToDate.Text = string.Empty;
        hdnClaimFrequencyCode.Value=string.Empty;

    }
    
    public void GetInstitutionalServiceInformationDetails()
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
            DataSet dsDentalServiceInformationfo = DataAccess.ExecuteStoredProcedure("Usp_Select_Claims_Service_Information_Institutional", parameters, "claims_service_information_institutional");

            if (Helper.HasRows(dsDentalServiceInformationfo) &&
            Convert.ToInt32(dsDentalServiceInformationfo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Type_of_Bill"].ToString()))
                {
                    txtTypeofBill.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Type_of_Bill"].ToString();
                    if (!txtTypeofBill.Text.StartsWith("0"))
                    {
                        txtTypeofBill.Text = "0" + txtTypeofBill.Text;
                    }
                }
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["From_Date"].ToString()))
                    txtFromDate.Text = Convert.ToDateTime(dsDentalServiceInformationfo.Tables[0].Rows[0]["From_Date"]).ToString("MM/dd/yyyy");
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["To_Date"].ToString()))
                    txtToDate.Text = Convert.ToDateTime(dsDentalServiceInformationfo.Tables[0].Rows[0]["To_Date"]).ToString("MM/dd/yyyy");
                txtPatPaidAmt.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Patient_Paid_Amount"].ToString();
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_Date_Hour"].ToString()))
                    txtAdmissionDate.Text = Convert.ToDateTime(dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_Date_Hour"]).ToString("MM/dd/yyyy");
                txtSubmittedDRG.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Submitted_DRG"].ToString();
                lblFinalDRG.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Final_DRG"].ToString();
                    ddlReleaseOfInfo.SelectedValue = dsDentalServiceInformationfo.Tables[0].Rows[0]["Release_of_Information"].ToString();
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Patient_Status"].ToString()))
                {
                    int counter = 0;
                    foreach (ListItem li in ddlPatientStatus.Items)
                    {
                        if (li.Value.Equals(dsDentalServiceInformationfo.Tables[0].Rows[0]["Patient_Status"].ToString().Trim()))
                        {
                            ddlPatientStatus.SelectedIndex = counter;
                            break;
                        }
                        counter++;
                    }
                }
                    if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_type"].ToString()))
                    ddlAdmissionType.SelectedIndex = ddlAdmissionType.Items.IndexOf(ddlAdmissionType.Items.FindByValue(dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_type"].ToString()));
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Admit_Source"].ToString()))
                    hdnAdmissionTypeValue.Value= ddlAdmitSource.SelectedItem.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Admit_Source"].ToString();
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Discharge_Hour"].ToString()))
                    txtDischargeHr.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Discharge_Hour"].ToString();                
                txtAdmissionHr.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_Hour"].ToString().Length == 3 ? dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_Hour"].ToString().PadLeft(4, '0') : dsDentalServiceInformationfo.Tables[0].Rows[0]["Admission_Hour"].ToString();

            }

        }
        catch (Exception ex) { }




    }

    protected void txtDischargeHr_TextChanged(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(txtDischargeHr.Text))
        //{

        //    txtAdmissionHr.Text = txtDischargeHr.Text;
        //}
    }


    protected void lnkSubClaimSepopSearch_Click(object sender, EventArgs e)
    {

        //lblSubmitClaimSearchPop.Text = "Type of Bill Search";
        mpeSubmitClaimSearchPop.Show();
    }


    protected void txtTypeofBill_TextChanged(object sender, EventArgs e)
    {
        lblInstErr.Text = "";
        if (txtTypeofBill.Text.Length > 0)
        {
            if (!txtTypeofBill.Text.StartsWith("0"))
            {
                lblInstErr.Text = "Type of bill must start from 0.";
            }

            if (txtTypeofBill.Text.Length < 4)
            { typeOfBillNumberOfDigitsError.Visible = true; }
            else
            {
                typeOfBillNumberOfDigitsError.Visible = false;
                if (!string.IsNullOrEmpty(txtTypeofBill.Text) && lblInstErr.Text == "")
                {
                    DataSet dsTOB = MAXIMUS.Controllers.PDMS.LookupTableController.GetInstitutionalTypeOfBill(txtTypeofBill.Text, "");

                    if (!Helper.HasRows(dsTOB))
                    {
                        lblInstErr.Text = "Type of Bill code is invalid";
                    }
                    else
                    {
                        lblInstErr.Text = "";
                    }

                }
            }
        }
        else
        {
            lblInstErr.Text = lblInstTypeBill.Text = "";
            typeOfBillNumberOfDigitsError.Visible = false;
        }


    }

}