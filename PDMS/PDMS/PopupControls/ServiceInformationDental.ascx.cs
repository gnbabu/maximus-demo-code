using Corp.Core.Libraries;
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

public partial class PopupControls_ServiceInformationDental : System.Web.UI.UserControl
{
    private ClaimsServiceAgent ClaimService = null;
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
        txtCode.ReadOnly = value;
        txtPatientAmountPaid.ReadOnly = value;
        txtPlaceofService.ReadOnly = value;
        txtPlaceOfServiceName.ReadOnly = value;
        txtPreClaimID.ReadOnly = value;
        //if (value)
        //{
        ddlDentalReleaseofInfo.Enabled = !value;
        ddlEPSDTCondition.Enabled = !value;

        //}
        lnkPlaceofServiceSearch.Visible = !value;



    }
    public string ReleaseofInformation
    {
        get { return ddlDentalReleaseofInfo.SelectedValue; }
        set
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.Trim() == "True")
                {
                    ddlDentalReleaseofInfo.ClearSelection();
                    ddlDentalReleaseofInfo.Items.FindByValue("Y").Selected = true;
                }
                else if (value.Trim() == "False")
                {
                    ddlDentalReleaseofInfo.ClearSelection();
                    ddlDentalReleaseofInfo.Items.FindByValue("N").Selected = true;
                }

                else if (value.Trim() == "I")
                {
                    ddlDentalReleaseofInfo.ClearSelection();
                    ddlDentalReleaseofInfo.Items.FindByValue("N").Selected = true;
                }
                else {
                    ddlDentalReleaseofInfo.ClearSelection();
                    ddlDentalReleaseofInfo.Items.FindByValue(value).Selected = true;
                }
            
            }
        }
    }
    public string ReleaseofInformationDisplay
    {
        get
        {
            if (ddlDentalReleaseofInfo.SelectedItem != null && !string.IsNullOrEmpty(ddlDentalReleaseofInfo.SelectedItem.Text))
                return ddlDentalReleaseofInfo.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlDentalReleaseofInfo.Items.FindByText(value.ToString().Trim()) != null)
                ddlDentalReleaseofInfo.SelectedItem.Text = value.Trim();
        }
    }

    public string PatientAmountPaid
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPatientAmountPaid.Text))
                return txtPatientAmountPaid.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPatientAmountPaid.Text = value;
        }
    }
    public string DateOfService
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(lblServiceDateInformation.Text))
            {
                return lblServiceDateInformation.Text;
            }   
            else
            {
                string servicedate = null;
                servicedate = GetServisedetailsDate();
                if(!string.IsNullOrEmpty(servicedate))
                {
                    return servicedate;
                }
                else
                {
                    return string.Empty; 
                }
            }    
        }
        set {
            lblServiceDateInformation.Text = value;
            hdnDateOfService.Value = value;
        }
    }

    public string PredeterminationClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPreClaimID.Text))
                return txtPreClaimID.Text;

            else
                return string.Empty;
        }
        set
        { txtPreClaimID.Text = value; }
    }
    public string SpecialProgramCode
    {
        get
        {
            if (!string.IsNullOrEmpty(ddlEPSDTCondition.SelectedItem.Text))
                return ddlEPSDTCondition.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                // ddlEPSDTCondition.SelectedItem.Text = value.Trim();
                ddlEPSDTCondition.SelectedIndex = ddlEPSDTCondition.Items.IndexOf(ddlEPSDTCondition.Items.FindByValue(value.Trim()));
        }
    }
    public string SpecialProgramCodeDisplay
    {
        get
        {
            if (ddlEPSDTCondition.SelectedItem != null && !string.IsNullOrEmpty(ddlEPSDTCondition.SelectedItem.Text))
                return ddlEPSDTCondition.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value) && ddlEPSDTCondition.Items.FindByText(value.ToString().Trim()) != null)
                ddlEPSDTCondition.SelectedIndex = ddlEPSDTCondition.Items.IndexOf(ddlEPSDTCondition.Items.FindByText(value.Trim()));

        }
    }

    public string PlaceofService
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPlaceofService.Text))
                return txtPlaceofService.Text;

            else
                return string.Empty;
        }
        set { txtPlaceofService.Text = value; }
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
        if (!string.IsNullOrEmpty(ddlDentalReleaseofInfo.SelectedItem.Text) || !string.IsNullOrWhiteSpace(ddlEPSDTCondition.SelectedItem.Text)
            || !string.IsNullOrWhiteSpace(txtPatientAmountPaid.Text) || !string.IsNullOrWhiteSpace(txtPlaceofService.Text) || !string.IsNullOrWhiteSpace(txtPreClaimID.Text)
            || !string.IsNullOrWhiteSpace(txtPlaceOfServiceName.Text))
        {
            rtn = true;
        }

        return rtn;
    }
    #endregion
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
    #region local Function
    private void GetEPSDTCondition()
    {


        ddlEPSDTCondition.Items.Clear();
        DataSet dataSet = spa.GetEPSDTCondition();
        DataTable dt = dataSet.Tables[0];
        var row_EPSDTCondition = from row in dt.AsEnumerable()
                                 where row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 1 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 2 ||
                                 row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 3 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 4
                                 select row;
        var row_professionalCondition = from row in dt.AsEnumerable()
                                        where row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 5 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 6 ||
                                        row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 7 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 8
                                        select row;
        var row_ProffReferralEPSDTService = from row in dt.AsEnumerable()
                                            where row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 9 || row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 10 ||
                                            row.Field<int>("PRIOR_AUTH_CLAIM_EPSDT_ID") == 11
                                            select row;
        DataTable dt_EPSDTCondition = row_EPSDTCondition.CopyToDataTable();
        DataTable dt_professionalCondition = row_professionalCondition.CopyToDataTable();
        DataTable dt_ProffReferralEPSDTService = row_ProffReferralEPSDTService.CopyToDataTable();
        Helper.LoadList(ddlEPSDTCondition, dt_EPSDTCondition, "PRIOR_AUTH_CLAIM_EPSDT_DESC", "PRIOR_AUTH_CLAIM_EPSDT_ID", true); //1,2,3,4
                                                                                                                                 //  Helper.LoadList(ddlProffReferralEPSDTService, dt_ProffReferralEPSDTService, "PRIOR_AUTH_CLAIM_EPSDT_DESC", "PRIOR_AUTH_CLAIM_EPSDT_ID", true);//9,10,11

    }
    public void ClearServiceInformationDentalFields()
    {


        ddlEPSDTCondition.SelectedIndex = 0;
        ddlDentalReleaseofInfo.SelectedIndex = 0;
        txtPlaceofService.Text = string.Empty;
        txtPatientAmountPaid.Text = string.Empty;
        //lblServiceDateInformation.Text = string.Empty;
        txtPreClaimID.Text = string.Empty;

    }

    public void ClearServiceInformationDentalFieldsCancel()
    {
        ClearServiceInformationDentalFields();
        hdnClaimId.Value = string.Empty;
    }
    private void AssignValidationSummary(string validationSummary)
    {
        // rfvDentalReleaseofInfo.ValidationGroup = validationSummary;
        // rfvPlaceOfService.ValidationGroup = validationSummary;
    }
    private bool ValidateServiceInformation()
    {
        bool isValid = true;
        if (String.IsNullOrWhiteSpace(ddlDentalReleaseofInfo.SelectedItem.Text))
        {
            AddValidationErrorMessage("<div>*Release of Information is required</div>");
            isValid = false;
        }
        return isValid;
    }
    private bool AddErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "validateClaims";
        this.Page.Validators.Add(val);
        return false;
    }
    public void SaveServiceInformation(String validationSummary, int actionButtonType)
    {
        if (actionButtonType == CON.ActionButtonType.Submit)
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (ValidateServiceInformation())
            {
                SaveToDB();
            }
        }
        else
        {
            if (HasInputValue())
            {
                SaveToDB();
            }
        }

    }
    private void SaveToDB()
    {
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
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
            DataSet dsServiceInformation = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Information", parameters, "Claims_Service_Information");
            string tableName = "Claims_Service_Information";
            if (Helper.HasRows(dsServiceInformation) &&
                Convert.ToInt32(dsServiceInformation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                parms.Add("Special_Program_Code", string.IsNullOrEmpty(ddlEPSDTCondition.SelectedValue) ? null : ddlEPSDTCondition.SelectedValue.ToString());
                if (ddlDentalReleaseofInfo.SelectedValue == "Y")
                {
                    parms.Add("Release_of_Information", "1");

                }
                else if (ddlDentalReleaseofInfo.SelectedValue == "N")
                {
                    parms.Add("Release_of_Information", "0");

                }

                parms.Add("Place_of_Service", string.IsNullOrEmpty(txtPlaceofService.Text) ? null : txtPlaceofService.Text);
                parms.Add("patient_Amount_Paid", string.IsNullOrEmpty(txtPatientAmountPaid.Text) ? null : txtPatientAmountPaid.Text);
                parms.Add("Date_of_Service_StartDate", string.IsNullOrEmpty(hdnStartDate.Value) ? null : hdnStartDate.Value);
                parms.Add("Date_of_Service_EndDate", string.IsNullOrEmpty(hdnEndDate.Value) ? null : hdnEndDate.Value);
                parms.Add("Created_Date_Time", string.IsNullOrEmpty(DateTime.Now.ToString()) ? null : DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("PreDeter_ClaimID", string.IsNullOrEmpty(txtPreClaimID.Text) ? null : txtPreClaimID.Text);
                parms.Add("Claims_Service_Information_ID", dsServiceInformation.Tables[0].Rows[0]["Claims_Service_Information_ID"].ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                ClaimsController.UpdatePanelsData(tableName, parms);
                ClearServiceInformationDentalFields();
                GetDentalServiceInformationDetails();
                GetServisedetailsData();
            }
            else if (!string.IsNullOrEmpty(hdnClaimId.Value))
            {
                parms.Add("Special_Program_Code", string.IsNullOrEmpty(ddlEPSDTCondition.SelectedValue) ? null : ddlEPSDTCondition.SelectedValue.ToString());
                if (ddlDentalReleaseofInfo.SelectedValue == "Y")
                {
                    parms.Add("Release_of_Information", "1");

                }
                else if (ddlDentalReleaseofInfo.SelectedValue == "N")
                {
                    parms.Add("Release_of_Information", "0");

                }
                parms.Add("Place_of_Service", string.IsNullOrEmpty(txtPlaceofService.Text) ? null : txtPlaceofService.Text);
                parms.Add("patient_Amount_Paid", string.IsNullOrEmpty(txtPatientAmountPaid.Text) ? null : txtPatientAmountPaid.Text);
                parms.Add("Date_of_Service_StartDate", string.IsNullOrEmpty(hdnStartDate.Value) ? null : hdnStartDate.Value);
                parms.Add("Date_of_Service_EndDate", string.IsNullOrEmpty(hdnEndDate.Value) ? null : hdnStartDate.Value);
                parms.Add("Created_Date_Time", string.IsNullOrEmpty(DateTime.Now.ToString()) ? null : DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("PreDeter_ClaimID", string.IsNullOrEmpty(txtPreClaimID.Text) ? null : txtPreClaimID.Text);
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                ClaimsController.InsertPanelsData(tableName, parms);
                ClearServiceInformationDentalFields();
                GetDentalServiceInformationDetails();
            }
        }
    }
    private void SearchDentalservice()
    {
        try
        {
            var dataset = PriorAuthHospitalController.DentalSearch();

            var ServiceSearchData = dataset.Tables["DentalSearch"];
            if (Helper.HasRows(ServiceSearchData))
            {
                gvPlaceofServiceSearch.DataSource = ServiceSearchData;
                gvPlaceofServiceSearch.DataBind();
            }
        }
        catch (Exception)
        {

        }

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
            DataSet dsDentalServiceInformationfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Information", parameters, "Claims_Service_Information");

            if (Helper.HasRows(dsDentalServiceInformationfo) &&
            Convert.ToInt32(dsDentalServiceInformationfo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Special_Program_Code"].ToString()))
                    //ddlEPSDTCondition.SelectedValue= dsDentalServiceInformationfo.Tables[0].Rows[0]["Special_Program_Code"].ToString().Trim();
                    ddlEPSDTCondition.SelectedIndex = ddlEPSDTCondition.Items.IndexOf(ddlEPSDTCondition.Items.
                        FindByValue(dsDentalServiceInformationfo.Tables[0].Rows[0]["Special_Program_Code"].ToString().Trim()));
                //ddlEPSDTCondition.Items.FindByValue(dsDentalServiceInformationfo.Tables[0].Rows[0]["Special_Program_Code"].ToString().Trim()).Selected = true;
                if (!string.IsNullOrEmpty(dsDentalServiceInformationfo.Tables[0].Rows[0]["Release_of_Information"].ToString()))
                {
                    if (dsDentalServiceInformationfo.Tables[0].Rows[0]["Release_of_Information"].ToString() == "True")
                        ddlDentalReleaseofInfo.SelectedValue = "Y";
                    if (dsDentalServiceInformationfo.Tables[0].Rows[0]["Release_of_Information"].ToString() == "False")
                        ddlDentalReleaseofInfo.SelectedValue = "N";

                }
                txtPlaceofService.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Place_of_Service"].ToString();
                txtPatientAmountPaid.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["patient_Amount_Paid"].ToString();
                //lblServiceDateInformation.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["Date_of_Service_StartDate"].ToString();
                txtPreClaimID.Text = dsDentalServiceInformationfo.Tables[0].Rows[0]["PreDeter_ClaimID"].ToString();
            }
        }
        catch (Exception ex) { }
    }
    #endregion
    #region "Page Event"
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetEPSDTCondition();
            GetDentalServiceInformationDetails();

        }
        SearchDentalservice();

        if (Session["PlaceofServiceCode"] != null)
        {
            txtPlaceOfServiceName.Text = Session["PlaceofServiceCode"].ToString();
        }

        
            releaseOfInformationDentalError.Text = "";
            ddlDentalReleaseofInfo.Style.Add("background-color", "white");
       
            placeOfInformationError.Text = "";
            txtPlaceofService.Style.Add("background-color", "white");

    }
    protected void LinkButton8_Click(object sender, EventArgs e)
    {
        if (((System.Web.UI.Control)sender).ID == "lnkPLaceServiceName")
        {
            SearchDentalservice();

        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["PlaceofServiceCode"] != null)
        {
            txtPlaceofService.Text = Session["PlaceofServiceCode"].ToString();
        }
        GetServisedetailsData();
    }
    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valOwnerInfo";
        this.Page.Validators.Add(val);
        //isGood = false;
        return false;
    }
    protected void grdPlaceofServiceSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void lnkSubClaimSepopSearch_Click(object sender, EventArgs e)
    {


        mpeSubmitClaimSearchPop.Show();
    }
    #endregion





    private DataSet FetchServiceDetailsInformation()
    {
        DataSet dsServiceDetailsInfo = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
        dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
        return dsServiceDetailsInfo;
    }

    protected void txtPlaceofService_TextChanged(object sender, EventArgs e)
    {
        lblServiceInfoDentalErr.Text = "";
        if (!string.IsNullOrEmpty(txtPlaceofService.Text))
        {
            var dsOC = LookupTableController.GetPlaceofServiceDetail(txtPlaceofService.Text.TrimEnd(), null);

            DataTable dsServiceDetail = FetchServiceDetailsInformation().Tables[0];

            var PlaceofService = dsServiceDetail.AsEnumerable()
                .Select(row => row["plc_service"]);
            if (PlaceofService != null)
            {
                foreach (var row in PlaceofService)
                {
                    if (!string.IsNullOrEmpty(row.ToString()))
                    {
                        if (Convert.ToInt32(row) == Convert.ToInt32(txtPlaceofService.Text.TrimEnd()))
                        {
                            lblServiceInfoDentalErr.Text = "*Place of service in service detail should only be entered if different than claim level ";
                            txtPlaceofService.Text = "";
                            return;
                        }
                    }
                    lblServiceInfoDentalErr.Text = "";
                }

            }
            if (!Helper.HasRows(dsOC))
            {
                lblServiceInfoDentalErr.Text = "Place of service code is invalid";
            }
            else
            {
                lblServiceInfoDentalErr.Text = "";
            }

        }
    }


    public void GetServisedetailsData()
    {
        hdnStartDate.Value = "";
        hdnEndDate.Value = "";
        List<SqlParameter> parameters = new List<SqlParameter>();
        DataTable claimsDataTable = new DataTable();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        DataSet serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
        claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        if (Helper.HasRows(claimsDataTable))
        {
            hdnDateOfService.Value = "";
            if (claimsDataTable.Rows.Count == 1)
            {
                if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["ServiceDate"].ToString()))
                {
                    string dateOfService = claimsDataTable.Rows[0].Field<DateTime>("ServiceDate").ToString();
                    if (!string.IsNullOrEmpty(dateOfService))
                    {
                        hdnDateOfService.Value = lblServiceDateInformation.Text = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                        hdnStartDate.Value = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                        hdnEndDate.Value = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        lblServiceDateInformation.Text = "";
                    }
                }
            }

            else if (claimsDataTable.Rows.Count > 1)
            {
                DataView dv = claimsDataTable.DefaultView;
                dv.Sort = "ServiceDate ASC";
                DataTable dtasc = dv.ToTable();
                DataView dvdesc = claimsDataTable.DefaultView;
                dvdesc.Sort = "ServiceDate desc";
                DataTable dtdesc = dvdesc.ToTable();
                if (!string.IsNullOrEmpty(dtasc.Rows[0]["ServiceDate"].ToString()))/* != null)*/
                    {
                    string dateOfServiceAsc = dtasc.Rows[0].Field<DateTime>("ServiceDate").ToString("MM/dd/yyyy");
                    string dateOfServiceDesc = dtdesc.Rows[0].Field<DateTime>("ServiceDate").ToString("MM/dd/yyyy");

                    if (dateOfServiceAsc == dateOfServiceDesc)
                    {
                        hdnDateOfService.Value = lblServiceDateInformation.Text = dateOfServiceAsc;
                        hdnStartDate.Value = dateOfServiceAsc;
                        hdnEndDate.Value = dateOfServiceAsc;
                    }
                    else if ((!string.IsNullOrEmpty(dateOfServiceAsc) && !string.IsNullOrEmpty(dateOfServiceDesc)))
                    {
                        hdnDateOfService.Value = lblServiceDateInformation.Text = dateOfServiceAsc + "  -  " + dateOfServiceDesc;
                        hdnStartDate.Value = dateOfServiceAsc;
                        hdnEndDate.Value = dateOfServiceAsc;
                    }
                    else
                    {
                        lblServiceDateInformation.Text = "";
                    }
                }
                else
                {
                    lblServiceDateInformation.Text = "";
                }
            }

        }
        else
        {
            lblServiceDateInformation.Text = "";
        }
    }
    public string GetServisedetailsDate()
    {
        string servicedatedental = null;
        hdnStartDate.Value = "";
        hdnEndDate.Value = "";
        List<SqlParameter> parameters = new List<SqlParameter>();
        DataTable claimsDataTable = new DataTable();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        DataSet serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
        claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        if (Helper.HasRows(claimsDataTable))
        {
            hdnDateOfService.Value = "";
            if (claimsDataTable.Rows.Count == 1)
            {
                if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["ServiceDate"].ToString()))
                {
                    string dateOfService = claimsDataTable.Rows[0].Field<DateTime>("ServiceDate").ToString();
                    if (!string.IsNullOrEmpty(dateOfService))
                    {
                        servicedatedental = hdnDateOfService.Value = lblServiceDateInformation.Text = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                        hdnStartDate.Value = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                        hdnEndDate.Value = Convert.ToDateTime(dateOfService).ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        lblServiceDateInformation.Text = "";
                    }
                }
            }

            else if (claimsDataTable.Rows.Count > 1)
            {
                DataView dv = claimsDataTable.DefaultView;
                dv.Sort = "ServiceDate ASC";
                DataTable dtasc = dv.ToTable();
                DataView dvdesc = claimsDataTable.DefaultView;
                dvdesc.Sort = "ServiceDate desc";
                DataTable dtdesc = dvdesc.ToTable();
                if (!string.IsNullOrEmpty(dtasc.Rows[0]["ServiceDate"].ToString()))/* != null)*/
                {
                    string dateOfServiceAsc = dtasc.Rows[0].Field<DateTime>("ServiceDate").ToString("MM/dd/yyyy");
                    string dateOfServiceDesc = dtdesc.Rows[0].Field<DateTime>("ServiceDate").ToString("MM/dd/yyyy");

                    if (dateOfServiceAsc == dateOfServiceDesc)
                    {
                        servicedatedental = hdnDateOfService.Value = lblServiceDateInformation.Text = dateOfServiceAsc;
                        hdnStartDate.Value = dateOfServiceAsc;
                        hdnEndDate.Value = dateOfServiceAsc;
                    }
                    else if ((!string.IsNullOrEmpty(dateOfServiceAsc) && !string.IsNullOrEmpty(dateOfServiceDesc)))
                    {
                        servicedatedental = hdnDateOfService.Value = lblServiceDateInformation.Text = dateOfServiceAsc + "  -  " + dateOfServiceDesc;
                        hdnStartDate.Value = dateOfServiceAsc;
                        hdnEndDate.Value = dateOfServiceAsc;
                    }
                    else
                    {
                        lblServiceDateInformation.Text = "";
                    }
                }
                else
                {
                    lblServiceDateInformation.Text = "";
                }
            }

        }
        else
        {
            lblServiceDateInformation.Text = "";
        }
        return servicedatedental;
    }
}