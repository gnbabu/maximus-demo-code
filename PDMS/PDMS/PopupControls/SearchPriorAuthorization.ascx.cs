using Corp.Core.Libraries;
using Corp.Core.Libraries.PriorAuthServiceReference;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Xml.Serialization;
using Telerik.Web.UI.PageLayout;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Security.Cryptography;
using NPOI.SS.Formula.Functions;

public partial class PopupControls_SearchPriorAuthorization : BaseSectionControl
{
    public delegate void EventHandler(int step, InquirePriorAuthResponse inquirePriorAuthResponse);
    public delegate void TrackingNumberEventHandler(int step, string medicaidId, string trackingNo);
    public event EventHandler SearchLinkClick;
    public event TrackingNumberEventHandler TrackingNumberSearchLink;
    #region spa
    private PDMSService.PDMSServiceClient _spa;
    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;

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
    public void RefreshWorkflowPage(bool forceRedirect = false)
    {
        // There is no force redirect. It is always refresh
        if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        cvtxtSubmissiondate.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        cvBirthDate.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        PASearchHelpTextID.Text = AppSettings.Get("PASearchHelpText", CON.PASearchHelpText.PASearchHelpText1);
        //cvPAEffDate1.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");

        //ddlCBMMXP.Visible = false;
        if (!IsPostBack)
        {
            try
            {
                LoadProviderInformation();
                GetAssignments();
                GetStatus();
                //GetProcedureCodeType();
                //GetDiagnoisCode();
                GetPayerNames();
                // GetICDCode();
                //GetManagedcareplan();
                txtPatientTrackingNumber.Attributes.Add("maxlength", txtPatientTrackingNumber.MaxLength.ToString());
                cpePasearch.Collapsed = true;
                cpePasearch.ClientState = "true";

                if (Request.QueryString.Count > 0)
                {
                    if (Request.QueryString.AllKeys.Contains("errorCode"))
                    {
                        int errorCode = int.Parse(System.Web.Security.AntiXss.AntiXssEncoder.HtmlEncode(Request["errorCode"].ToString(), true));

                        if (errorCode > 0 && errorCode == MAXIMUS.Core.Libraries.Constants.ErrorCodes.TransactionFailed)
                        {
                            lblGeneralErr.Visible = true;
                            lblGeneralErr.Text = "No records Found";
                        }
                        else if (errorCode > 0 && errorCode == MAXIMUS.Core.Libraries.Constants.ErrorCodes.ResponseNull)
                        {
                            MessageBox2.Show("PA inquiry results are empty.", "Error");
                            lblGeneralErr.Visible = true;
                            lblGeneralErr.Text = "PA inquiry results are empty.";
                        }
                        else if (errorCode > 0 && errorCode == MAXIMUS.Core.Libraries.Constants.ErrorCodes.NoRecords)
                        {
                            lblGeneralErr.Visible = true;
                            lblGeneralErr.Text = "No Data Found";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-PageLoad");
                MessageBox2.Show(string.Format("An error has occurred while page load event. Reference Id : {0}", logNumber), "Error");
            }
        }
    }

    private void GetStatus()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlStatus.Items.Clear();
            DataSet dataSet = _spa.GetStatus();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];
                dt.DefaultView.Sort = "PRIOR_AUTH_STATUS_TYPE";
                dt = dt.DefaultView.ToTable();
                ddlStatus.DataSource = dt;
                Helper.LoadList(ddlStatus, dt, "PRIOR_AUTH_STATUS_TYPE", "PRIOR_AUTH_STATUS_ID", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetStatus method", ex);
        }
    }

    private void GetPayerNames()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlPayerName.Items.Clear();
            DataSet dataSet = _spa.GetPayerNames();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];

                ddlAssignment.DataSource = dt;
                Helper.LoadList(ddlPayerName, dt, "PRIOR_AUTH_DESTINATION_PAYER_DESC", "PRIOR_AUTH_DESTINATION_PAYER_ID", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetPayerNames method", ex);
        }
    }

    protected void PageSize_Changed(object sender, EventArgs e)
    {
        //this.GetCustomersPageWise(1);
    }

    private void GetAssignments()
    {
        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            ddlAssignment.Items.Clear();

            DataSet dataSet = _spa.GetAssignements();
            if (dataSet != null)
            {
                DataTable dt = dataSet.Tables[0];

                ddlAssignment.DataSource = dt;
                Helper.LoadList(ddlAssignment, dt, "PRIOR_AUTH_Assignment_Type_DESC", "PRIOR_AUTH_Assignment_Type_MMIS", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at GetAssignments method", ex);
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucSearchPriorAuthorization_" + this.WorkflowPage.RegistrationId; }
    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        bool isGood = true;
        if (!string.IsNullOrEmpty(txtRevenuecode.Text) || !string.IsNullOrWhiteSpace(txtRevenuecode.Text))
        {
            if (txtRevenuecode.Text.Length < 3)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "3 characters is required";
                val.ValidationGroup = "valProviderInfoheader";
                this.Page.Validators.Add(val);
                isGood = false;
            }
        }

        if (!string.IsNullOrEmpty(txtProcedureCode.Text) || !string.IsNullOrWhiteSpace(txtProcedureCode.Text))
        {
            if (txtProcedureCode.Text.Length < 5)
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "5 characters is required";
                val.ValidationGroup = "valProviderInfoheader";
                this.Page.Validators.Add(val);
                isGood = false;
            }
        }

        return isGood;
    }

    protected void ValidatetxtProcedureCode(object sender, ServerValidateEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtProcedureCode.Text) || !string.IsNullOrWhiteSpace(txtProcedureCode.Text))
        {
            if (txtProcedureCode.Text.Length < 5)
            {
                e.IsValid = false;
            }
        }
    }

    protected void ValidatetxtRevenuecode(object sender, ServerValidateEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtRevenuecode.Text) || !string.IsNullOrWhiteSpace(txtRevenuecode.Text))
        {
            if (txtRevenuecode.Text.Length < 4)
            {
                e.IsValid = false;
            }
        }
    }

    public override string Title
    {
        get { return "PRIOR AUTHORIZATION SEARCH"; }
    }


    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {

    }
    private void SearchAuthData()
    {
        try
        {
            string PayerName = ddlPayerName.SelectedValue;
            string AssignmentType = ddlAssignment.SelectedValue;
            string DiagnosisCode = txtDiagnoisCode.Text.Trim();
            string ICDProcedureCode = txtICDCode.Text.Trim();
            string MemberMedicaidId = txtMedicaidBillingNumber.Text.Trim();
            string OrderingProviderID = txtorderProvnpi.Text.Trim();
            string PriorAuthNumber = txtPriorAuthNumber.Text.Trim();
            string CPTHCPCSServiceCode = txtProcedureCode.Text.Trim();
            string RevenueCode = txtRevenuecode.Text.Trim();
            string PatientEventTrackingNumber = txtPatientTrackingNumber.Text.Trim();
            string RequestingProviderID = hdnNPI.Value;

            DateTime? SubDate;
            SubDate = !string.IsNullOrEmpty(txtSubmissiondate.Text.Trim()) ? DateTime.ParseExact(txtSubmissiondate.Text.Trim(), "MM/dd/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;

            DateTime? PAEffDate;
            PAEffDate = !string.IsNullOrEmpty(txtPAEffDate.Text.Trim()) ? DateTime.ParseExact(txtPAEffDate.Text.Trim(), "MM/dd/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;

            DateTime? PAExpDate;
            PAExpDate = !string.IsNullOrEmpty(txtPAExpDate.Text.Trim()) ? DateTime.ParseExact(txtPAExpDate.Text.Trim(), "MM/dd/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;

            DateTime? DateOfBirth;
            DateOfBirth = !string.IsNullOrEmpty(txtBirthDate.Text.Trim()) ? DateTime.ParseExact(txtBirthDate.Text.Trim(), "MM/dd/yyyy",
                                              System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;

            string PAStatus = ddlStatus.SelectedValue;

            // call database function to load data
            var ds = PriorAuthHospitalController.GetSearchAuthData(PayerName, AssignmentType, DiagnosisCode, ICDProcedureCode, MemberMedicaidId,
                                    OrderingProviderID, PriorAuthNumber, CPTHCPCSServiceCode, RevenueCode, PatientEventTrackingNumber,
                                    SubDate, PAEffDate, PAExpDate, DateOfBirth, PAStatus);
            // Bind data to grid 
            //DataSet ds = new DataSet();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                var dtsearchauth = new DataTable();
                dtsearchauth.TableName = "SearchAuthorizationResponse";
                dtsearchauth.Columns.Add("PriorAuthorizationID");
                dtsearchauth.Columns.Add("PayorType");
                dtsearchauth.Columns.Add("MemberID");
                dtsearchauth.Columns.Add("PatientTrackingNumber");
                dtsearchauth.Columns.Add("LastName");
                dtsearchauth.Columns.Add("FirstMName");
                dtsearchauth.Columns.Add("StatusCode");
                dtsearchauth.Columns.Add("ICDProcedureCode");
                dtsearchauth.Columns.Add("ProcedureCode");
                dtsearchauth.Columns.Add("DiagnosisCode");
                dtsearchauth.Columns.Add("RevenueCode");
                dtsearchauth.Columns.Add("AuthSubmissionDate");
                dtsearchauth.Columns.Add("AuthorizationEndDate");
                dtsearchauth.Columns.Add("AssignmentType");
                dtsearchauth.Columns.Add("OrderingProviderID");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dtsearchauth.NewRow();
                    dr["PriorAuthorizationID"] = dt.Rows[i]["PriorAuthNumber"];
                    dr["PayorType"] = dt.Rows[i]["PayorType"];
                    dr["MemberID"] = dt.Rows[i]["MemberMedicaidId"];
                    dr["PatientTrackingNumber"] = dt.Rows[i]["PatientEventTrackingNumber"];
                    dr["LastName"] = dt.Rows[i]["MemberLastName"];
                    dr["FirstMName"] = dt.Rows[i]["MemberFirstName"] + " " + dt.Rows[i]["MemberMiddleName"];
                    dr["StatusCode"] = dt.Rows[i]["PAStatus"];
                    dr["ICDProcedureCode"] = dt.Rows[i]["ICDProcedureCode"];
                    dr["ProcedureCode"] = dt.Rows[i]["CPTHCPCSServiceCode"];
                    dr["DiagnosisCode"] = dt.Rows[i]["DiagnosisCode"];
                    dr["RevenueCode"] = dt.Rows[i]["RevenueCode"];

                    dr["AuthSubmissionDate"] = dt.Rows[i]["PAStartDate"];// System.DateTime.Now.ToString();
                    dr["AuthorizationEndDate"] = dt.Rows[i]["PAEndDate"];//System.DateTime.Now.ToString();
                    dr["AssignmentType"] = dt.Rows[i]["AssignmentTypeCode"];
                    dr["OrderingProviderID"] = dt.Rows[i]["OrderingProviderID"];
                    dtsearchauth.Rows.Add(dr);
                }
                if (dtsearchauth.Rows.Count > 0)
                {
                    gvPasearch.DataSource = dtsearchauth;
                    gvPasearch.DataBind();

                    pnlPasearch.Visible = true;
                    pnlsepPasearch.Visible = true;
                    cpePasearch.Collapsed = false;
                    cpePasearch.ClientState = "false";
                }
                else
                {
                    cpePasearch.Collapsed = true;
                    cpePasearch.ClientState = "true";

                }
            }
            else
            {
                cpePasearch.Collapsed = true;
                cpePasearch.ClientState = "true";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at SearchAuthData method", ex);
        }
    }

    //PA Status INPROCESS, APPROVED, DENIED, PEND, MEDREVIEW,CLOSED
    public string paStatusLookUp(string paStat)
    {
        string str = string.Empty;
        switch (paStat)
        {
            case "8":
                str = "INPROCESS";
                break;
            case "1":
                str = "APPROVED";
                break;
            case "2":
                str = "PARTIALLY APPROVED";
                break;
            case "3":
                str = "DENIED";
                break;
            case "4":
                str = "PEND";
                break;
            case "6":
                str = "CLOSED";
                break;
            case "14":
                str = "PENDING ADDTL INFO";
                break;
        }
        return str;
    }

    #region SEARCH PRIOR AUTH
    private void SearchAuthSA(int pageStartIndex)
    {
        string logHeader = string.Format("PriorAuthSearch Get Transaction History for Medicaid ID - ");
        string logMsg = string.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
        int logTransactionId = 0;
        SearchPriorAuthRequestPayloadType requestPayLoad = new SearchPriorAuthRequestPayloadType();

        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        DataSet dataSet = _spa.GetPayerNames();
        if (dataSet != null)
        {
            DataTable dt = dataSet.Tables[0];
            if (!string.IsNullOrEmpty(ddlPayerName.SelectedValue))
            {
                DataRow dr = dt.Select("PRIOR_AUTH_DESTINATION_PAYER_ID = " + ddlPayerName.SelectedValue).First();

                if (dr.Table.Columns.Contains("MCE_ID"))
                {
                    if (dr["MCE_ID"].ToString().Equals("MMISODFS") || dr["MCE_ID"].ToString().Equals("MMISODJFS"))
                    {
                        requestPayLoad.PayorType = "FFS";
                    }
                    else
                    {
                        requestPayLoad.PayorType = dr["MCE_ID"].ToString();
                    }
                }
            }
        }
        requestPayLoad.AssignmentTypeCode = !string.IsNullOrEmpty(ddlAssignment.SelectedValue) ? ddlAssignment.SelectedValue : null;

        if (!string.IsNullOrEmpty(txtDiagnoisCode.Text) && !string.IsNullOrWhiteSpace(txtDiagnoisCode.Text))
        {
            requestPayLoad.DiagnosisCode = txtDiagnoisCode.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtICDCode.Text) && !string.IsNullOrWhiteSpace(txtICDCode.Text))
        {
            requestPayLoad.ICDProcedureCode = txtICDCode.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text) && !string.IsNullOrWhiteSpace(txtMedicaidBillingNumber.Text))
        {
            requestPayLoad.MemberMedicaidId = txtMedicaidBillingNumber.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtorderProvnpi.Text) && !string.IsNullOrWhiteSpace(txtorderProvnpi.Text))
        {
            requestPayLoad.OrderingProviderID = txtorderProvnpi.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtPriorAuthNumber.Text) && !string.IsNullOrWhiteSpace(txtPriorAuthNumber.Text))
        {
            requestPayLoad.PriorAuthNumber = txtPriorAuthNumber.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtProcedureCode.Text) && !string.IsNullOrWhiteSpace(txtProcedureCode.Text))
        {
            requestPayLoad.CPTHCPCSServiceCode = txtProcedureCode.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtRevenuecode.Text) && !string.IsNullOrWhiteSpace(txtRevenuecode.Text))
        {
            requestPayLoad.RevenueCode = txtRevenuecode.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtPatientTrackingNumber.Text) && !string.IsNullOrWhiteSpace(txtPatientTrackingNumber.Text))
        {
            requestPayLoad.PatientEventTrackingNumber = txtPatientTrackingNumber.Text.Trim();
        }

        DateTime? DateOfBirth;

        if (!string.IsNullOrEmpty(txtBirthDate.Text) && !string.IsNullOrWhiteSpace(txtBirthDate.Text))
        {
            DateOfBirth = !string.IsNullOrEmpty(txtBirthDate.Text.Trim()) ? DateTime.ParseExact(txtBirthDate.Text.Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;

            if (DateOfBirth != null)
            {
                requestPayLoad.DateOfBirth = DateOfBirth.Value;
                requestPayLoad.DateOfBirthSpecified = true;
            }
        }

        requestPayLoad.RequestingProviderID = hdnNPI.Value;
        requestPayLoad.PageSize = gvPasearch.PageSize.ToString();
        requestPayLoad.Offset = Convert.ToString(gvPasearch.PageIndex * gvPasearch.PageSize);
        DateTime? SubDate;
        SubDate = !string.IsNullOrEmpty(txtSubmissiondate.Text.Trim()) ? DateTime.ParseExact(txtSubmissiondate.Text.Trim(), "MM/dd/yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;
        if (SubDate != null)
        {
            requestPayLoad.SubmittedDate = SubDate.Value;
            requestPayLoad.SubmittedDateSpecified = true;
        }

        DateTime? PAEffDate;
        PAEffDate = !string.IsNullOrEmpty(txtPAEffDate.Text.Trim()) ? DateTime.ParseExact(txtPAEffDate.Text.Trim(), "MM/dd/yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;
        if (PAEffDate != null)
        {
            requestPayLoad.PAStartDate = PAEffDate.Value;
            requestPayLoad.PAStartDateSpecified = true;
        }
        DateTime? PAExpDate;
        PAExpDate = !string.IsNullOrEmpty(txtPAExpDate.Text.Trim()) ? DateTime.ParseExact(txtPAExpDate.Text.Trim(), "MM/dd/yyyy",
                                          System.Globalization.CultureInfo.InvariantCulture) : (DateTime?)null;
        if (PAExpDate != null)
        {
            requestPayLoad.PAEndDate = PAExpDate.Value;
            requestPayLoad.PAEndDateSpecified = true;
        }

        if (!string.IsNullOrEmpty(ddlStatus.SelectedValue))
        {
            requestPayLoad.PAStatus = paStatusLookUp(ddlStatus.SelectedValue);
        }

        try
        {
            int SearchPriorAuth = 6;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(SearchPriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(CON.appAdminUserId));
            logTransactionId = transactionID;
            var searchPAReq = new SearchPriorAuthRequest();
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);
            MessageHeaderTypeSubscriber[] fsSub = new MessageHeaderTypeSubscriber[1];
            fsSub[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType msgHeader = new MessageHeaderType();
            msgHeader.BusinessFlow = MessageHeaderTypeBusinessFlow.SearchPriorAuth;
            msgHeader.StateCode = "OH";
            msgHeader.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;
            msgHeader.ModuleTransactionId = transactionID.ToString();
            msgHeader.SubscriberSystem = fsSub;
            msgHeader.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            msgHeader.SITransactionKey = sitTransactionKey;

            searchPAReq.MessageHeader = msgHeader;
            searchPAReq.RequestPayload = requestPayLoad;

            var response = new PriorAuthServiceReqRes().priorAuthSearchOperation(transactionID, searchPAReq);
            if (!string.IsNullOrEmpty(response) && !response.Equals(CON.TransactionResult.TransactionFailed))
            {
                try
                {
                    DataSet ds = new DataSet();
                    System.Xml.XmlReaderSettings settings = new System.Xml.XmlReaderSettings();
                    settings.DtdProcessing = System.Xml.DtdProcessing.Ignore;
                    settings.XmlResolver = null;
                    System.Xml.XmlReader xmlReader = System.Xml.XmlReader.Create(new StringReader(response.ToString()), settings);
                    ds.ReadXml(xmlReader);

                    DataTable dt = ds.Tables["ResponsePayload"];
                    var dsGrid = new DataSet();
                    var dtsearchauth = new DataTable();
                    dtsearchauth.TableName = "SearchAuthorizationResponse";
                    dtsearchauth.Columns.Add("PriorAuthorizationID");
                    dtsearchauth.Columns.Add("PayorType");
                    dtsearchauth.Columns.Add("MemberID");
                    dtsearchauth.Columns.Add("PatientTrackingNumber");
                    dtsearchauth.Columns.Add("LastName");
                    dtsearchauth.Columns.Add("FirstMName");
                    dtsearchauth.Columns.Add("StatusCode");
                    dtsearchauth.Columns.Add("ICDProcedureCode");
                    dtsearchauth.Columns.Add("ProcedureCode");
                    dtsearchauth.Columns.Add("DiagnosisCode");
                    dtsearchauth.Columns.Add("RevenueCode");
                    dtsearchauth.Columns.Add("AuthSubmissionDate");
                    dtsearchauth.Columns.Add("AuthorizationEndDate");
                    dtsearchauth.Columns.Add("AssignmentType");
                    dtsearchauth.Columns.Add("OrderingProviderID");

                    var dtsearchauthRes = new DataTable();
                    dtsearchauthRes.TableName = "SearchAuthPaginationResponse";
                    dtsearchauthRes.Columns.Add("SearchPriorAuthResponse_Id");
                    dtsearchauthRes.Columns.Add("Offset");
                    dtsearchauthRes.Columns.Add("TotalRecords");
                    dtsearchauthRes.Columns.Add("Body_Id");

                    DataTable dtRes = ds.Tables["SearchPriorAuthResponse"];
                    for (int i = 0; i < dtRes.Rows.Count; i++)
                    {
                        if (dtRes.Columns.Contains("TotalRecords"))
                        {
                            gvPasearch.VirtualItemCount = Convert.ToInt32(dtRes.Rows[i]["TotalRecords"]);
                            gvPasearch.PageSize = !string.IsNullOrEmpty(ddlPageSize.SelectedValue) ? Convert.ToInt32(ddlPageSize.SelectedValue) : 0;
                        }
                        gvPasearch.PageIndex = pageStartIndex;


                        //if (dtRes.Columns.Contains("Offset"))
                        //{
                        //    if (pageStartIndex != 0)
                        //    {
                        //        var _pageindex = Convert.ToInt32(dtRes.Rows[i]["Offset"]);
                        //        if (_pageindex >= 0) gvPasearch.PageIndex = _pageindex;
                        //        // else gvPasearch.PageIndex = pageStartIndex;
                        //        else gvPasearch.PageIndex = 0;
                        //    }
                        //    else
                        //        gvPasearch.PageIndex = pageStartIndex;
                        //    //gvPasearch.PageIndex = Convert.ToInt32(dtRes.Rows[i]["Offset"]); //-1, 0 or greater 1 its has to be 

                        //}

                    }
                    // DataTable dtClone = dt.Rows.Cast<System.Data.DataRow>().Skip((pageStartIndex) * Convert.ToInt32(ddlPageSize.SelectedValue)).Take(Convert.ToInt32(ddlPageSize.SelectedValue)).CopyToDataTable();
                    //dt = dtClone;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dtsearchauth.NewRow();

                        if (dt.Columns.Contains("PayorType"))
                        {
                            dr["PayorType"] = dt.Rows[i]["PayorType"];
                        }

                        if (dt.Columns.Contains("PatientEventTrackingNumber"))
                        {
                            dr["PatientTrackingNumber"] = dt.Rows[i]["PatientEventTrackingNumber"];
                        }
                        if (dt.Columns.Contains("MemberMedicaidId"))
                        {
                            dr["MemberID"] = dt.Rows[i]["MemberMedicaidId"];
                        }
                        if (dt.Columns.Contains("PriorAuthNumber"))
                        {
                            dr["PriorAuthorizationID"] = dt.Rows[i]["PriorAuthNumber"];
                        }
                        if (dt.Columns.Contains("MemberLastName"))
                        {
                            dr["LastName"] = dt.Rows[i]["MemberLastName"];
                        }
                        if (dt.Columns.Contains("MemberFirstName"))
                        {
                            dr["FirstMName"] = dt.Rows[i]["MemberFirstName"];

                            if (dt.Columns.Contains("MemberMiddleName"))
                            {
                                dr["FirstMName"] = dt.Rows[i]["MemberFirstName"] + " " + dt.Rows[i]["MemberMiddleName"];
                            }
                        }
                        if (dt.Columns.Contains("PAStatus"))
                        {
                            dr["StatusCode"] = dt.Rows[i]["PAStatus"];
                        }
                        if (dt.Columns.Contains("ICDProcedureCode"))
                        {
                            dr["ICDProcedureCode"] = dt.Rows[i]["ICDProcedureCode"];
                        }
                        if (dt.Columns.Contains("CPTHCPCSServiceCode"))
                        {
                            dr["ProcedureCode"] = dt.Rows[i]["CPTHCPCSServiceCode"];
                        }
                        if (dt.Columns.Contains("DiagnosisCode"))
                        {
                            dr["DiagnosisCode"] = dt.Rows[i]["DiagnosisCode"];
                        }
                        if (dt.Columns.Contains("RevenueCode"))
                        {
                            dr["RevenueCode"] = dt.Rows[i]["RevenueCode"];
                        }

                        if (dt.Columns.Contains("PAStartDate"))
                        {
                            dr["AuthSubmissionDate"] = dt.Rows[i]["PAStartDate"];// System.DateTime.Now.ToString();
                        }
                        if (dt.Columns.Contains("PAEndDate"))
                        {
                            dr["AuthorizationEndDate"] = dt.Rows[i]["PAEndDate"];//System.DateTime.Now.ToString();
                        }
                        if (dt.Columns.Contains("AssignmentTypeCode"))
                        {
                            dr["AssignmentType"] = dt.Rows[i]["AssignmentTypeCode"];
                        }
                        if (dt.Columns.Contains("OrderingProviderID"))
                        {
                            dr["OrderingProviderID"] = dt.Rows[i]["OrderingProviderID"];
                        }
                        if (dt.Columns.Contains("TotalRecords"))
                        {
                            dr["TotalRecords"] = dt.Rows[i]["TotalRecords"];
                        }
                        dtsearchauth.Rows.Add(dr);
                    }
                    if (dtsearchauth.Rows.Count > 0)
                    {
                        gvPasearch.DataSource = dtsearchauth;
                        gvPasearch.DataBind();
                        gvPasearch.Visible = true;
                        pnlPasearch.Visible = true;
                        pnlsepPasearch.Visible = true;
                        cpePasearch.Collapsed = false;
                        cpePasearch.ClientState = "false";
                    }
                    else
                    {
                        pnlPasearch.Visible = true;
                        pnlsepPasearch.Visible = true;
                        gvPasearch.DataSource = null;
                        gvPasearch.DataBind();
                        gvPasearch.ShowHeaderWhenEmpty = true;
                        gvPasearch.Visible = true;
                        cpePasearch.Collapsed = true;
                        cpePasearch.ClientState = "false";

                    }
                }

                catch (Exception ex)
                {
                    string message = logTransactionId != 0 ? logTransactionId.ToString() : "No transaction generated";
                    throw new Exception("Error at SearchAuthSA method for transaction id -  " + message, ex);
                }
            }
            else
            {
                pnlPasearch.Visible = true;
                pnlsepPasearch.Visible = true;
                DataTable dt = new DataTable();
                gvPasearch.DataSource = dt;
                gvPasearch.DataBind();
                gvPasearch.ShowHeaderWhenEmpty = true;
                gvPasearch.Visible = true;
                cpePasearch.Collapsed = false;
                cpePasearch.ClientState = "false";
                // cpePasearchTracking.Collapsed = false;
                //cpePasearchTracking.ClientState = "false";
            }

        }
        catch (Exception exx)
        {
            //Logging log = new Logging(Guid.NewGuid(), logMsg);
            string response = "Response NULL";
            //log.CreateLogEntry(string.Format("{0} {1}", logHeader, exx.ToString() + "Response date :-" + response), Logging.LogPriority.Error);
            throw new Exception("Error at SearchAuthSA method and Response date :- " + response, exx);
        }

        #endregion


    }

    private string ConvertDatasetToXml(DataSet ds)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var xmlSerializer = new XmlSerializer(typeof(DataSet));
                xmlSerializer.Serialize(streamWriter, ds);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }
    }

    public bool ValidateData(string msg)
    {

        //bool isGood = true;
        //RequiredFieldValidator req = new RequiredFieldValidator();
        bool isValid = true;
        DateTime value;
        DateTime dateOfBirth;
        DateTime.TryParse(txtBirthDate.Text, out dateOfBirth);
        if (String.IsNullOrEmpty(txtPriorAuthNumber.Text))
        {
            AddValidationErrorMessage("*Enter PriorAuthNumber");
            isValid = false;
        }

        else if (String.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            AddValidationErrorMessage("*Enter Medicaid Billing Number");
            isValid = false;

        }
        else if (String.IsNullOrEmpty(txtBirthDate.Text))
        {
            AddValidationErrorMessage("*Select a valid Date of Birth");
            isValid = false;

        }
        else if (!string.IsNullOrEmpty(txtBirthDate.Text) && DateTime.TryParse(txtBirthDate.Text, out value) && DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtBirthDate.Text)) < 0)
        {
            AddValidationErrorMessage("* DOB Does not allow future date.", ref isValid);
        }
        else if ((dateOfBirth != null) && (dateOfBirth.Year < 1900))
        {
            AddValidationErrorMessage("* DOB Must be beyond 1900.", ref isValid);
        }
        else if (!string.IsNullOrEmpty(txtBirthDate.Text) && !DateTime.TryParse(txtBirthDate.Text, out value))
        {
            AddValidationErrorMessage("* DOB is not a date format.", ref isValid);
        }
        else if (String.IsNullOrEmpty(txtSubmissiondate.Text))
        {
            AddValidationErrorMessage("*Select a valid Submission Date");
            isValid = false;

        }

        else if (String.IsNullOrEmpty(txtSubmissiondate.Text))
        {
            AddValidationErrorMessage("*Select a valid Submission Date");
            isValid = false;

        }
        else if (String.IsNullOrEmpty(txtProcedureCode.Text))
        {
            AddValidationErrorMessage("*Enter 5 characters is required");
            isValid = false;

        }
        else if (String.IsNullOrEmpty(txtRevenuecode.Text))
        {
            AddValidationErrorMessage("*Enter Revenue code");
            isValid = false;

        }
        else if (String.IsNullOrEmpty(txtDiagnoisCode.Text))
        {
            AddValidationErrorMessage("*Diagnosis Code is required");
            isValid = false;

        }
        if (!String.IsNullOrEmpty(txtBirthDate.Text))
        {
            if (DateTime.TryParse(txtBirthDate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtBirthDate.Text)) < 0)
                {
                    AddError("* DOB Does not allow future date.", ref isValid);
                }
            }
            else
            {
                AddError("* Date From is not a date format.", ref isValid);
            }
        }

        if (!String.IsNullOrEmpty(txtSubmissiondate.Text))
        {
            if (DateTime.TryParse(txtSubmissiondate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtSubmissiondate.Text)) < 0)
                {
                    AddError("* Date To cannot be Does not allow future date.", ref isValid);
                }
            }
            else
            {
                AddError("* Date To is not a date format.", ref isValid);
            }
        }

        return isValid;
    }

    public override string ValidationGroup
    {
        get { return "valProviderInfoHeader"; }
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
    private void AddValidationErrorMessage(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valERemittanceAdvice";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ValidateData();
            if (Page.IsValid)
            {
                if (!string.IsNullOrEmpty(txtPatientTrackingNumber.Text.Trim()) && !string.IsNullOrEmpty(ddlStatus.SelectedValue) &&
                     !string.IsNullOrEmpty(ddlPayerName.SelectedValue))
                {
                    gvPasearch.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                    pnlPasearch.Visible = true;
                    gvPasearch.DataSource = null;
                    gvPasearch.DataBind();
                    gvPasearchTracking.Visible = false;
                    gvPasearch.Visible = false;
                    cpePasearch.Collapsed = false;
                    cpePasearch.ClientState = "false";
                    SearchBindGrid();
                }
                else
                {
                    gvPasearch.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                    pnlPasearch.Visible = true;
                    gvPasearchTracking.DataSource = null;
                    gvPasearchTracking.DataBind();
                    gvPasearchTracking.Visible = false;
                    gvPasearch.Visible = true;
                    SearchAuthSA(0);
                }
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-BtnSearchClick");
            MessageBox2.Show(string.Format("An error has occurred while search click event. Reference Id : {0}", logNumber), "Error");
        }
    }

    protected void grdPasearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //Sample code needs to be removed once Search service is integrated
            string PayorType = string.Empty;
            string PriorAuthNumber = gvPasearch.SelectedRow.Cells[1].Text;
            PayorType = gvPasearch.SelectedRow.Cells[2].Text;

            PriorAuthNumber = HttpUtility.UrlEncode(Helper.Encrypt(PriorAuthNumber.Trim()));
            PayorType = HttpUtility.UrlEncode(Helper.Encrypt(PayorType));
            string Npi = HttpUtility.UrlEncode(Helper.Encrypt(this.hdnNPI.Value));
            string url = string.Format("~/Process/SubmitPriorAuthorization.aspx?PA={0}&PayorId={1}&NPI={2}", PriorAuthNumber, PayorType, Npi);
            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }

            //InquirePriorAuth(PriorAuthNumber, PayorType);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-GrdPASearchSelectedIndexChanged");
            MessageBox2.Show(string.Format("An error has occurred while grid select change event. Reference Id : {0}", logNumber), "Error");
        }
    }


    public void InquirePriorAuth(string priorAuthNUmber, string payorId)
    {
        string logHeader = string.Format("PriorAuthInquire Get Transaction History for Medicaid ID - ");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


        DataSet dsresponse = null;
        try
        {
            string priorAuthUserName = AppSettings.Get("PriorAuthServiceWSUserName");
            string client_secret = "";
            string secretName = "HospiceWSPassword_OH_PNM_";
            try
            {
                var secret = new SecretsManager(AppSettings.Get("SecretsRegion"));
                string environmentName = AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "");
                var secretResult = secret.GetSuperSecretPassword(String.Concat(environmentName, "/WebService/DBPassword"));
                secretResult.Wait();

                client_secret = secretResult.Result[String.Concat(secretName, environmentName)];
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid(), System.Reflection.MethodBase.GetCurrentMethod().ToString());
                client_secret = "NO SECRET CONNECTIVITY";
                client_secret += " ~|~ ";
                client_secret += AppSettings.Get("SecretsRegion");
                client_secret += " ~|~ ";
                client_secret += String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword");
                client_secret += " ~|~ ";
                client_secret += String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""));
                client_secret += " ~|~ ";
                client_secret += ex.Message;
                log.CreateLogEntry(string.Format("Failure Gathering Secret: Region: {0}; Environment {1}; Dictionary: {2}; Secret: {3}; ErrorMessage: {4}; Stack Trace: {5}", AppSettings.Get("SecretsRegion"), AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), String.Concat(AppSettings.Get("EnvironmentName").Replace("OH_PNM_", ""), "/WebService/DBPassword"), String.Concat(secretName, AppSettings.Get("EnvironmentName").Replace("OH_PNM_", "")), ex.Message, ex.StackTrace), Logging.LogPriority.Error);
            }
            string priorAuthPassword = client_secret;
            string sitTransactionKey = ProviderManagementHelper.GetUniqueKey(32);

            int InquirePriorAuth = 5;
            int transactionID = InfoAccessController.InsertPASSTHROUGH_TRANSACTIONQUEUE(InquirePriorAuth, DateTime.Now, DateTime.Now, DateTime.Now, new Guid(MAXIMUS.Core.Libraries.Constants.appAdminUserId));

            MessageHeaderTypeSubscriber[] mhts = new MessageHeaderTypeSubscriber[1];
            mhts[0] = MessageHeaderTypeSubscriber.FI;

            MessageHeaderType mht = new MessageHeaderType();
            mht.BusinessFlow = MessageHeaderTypeBusinessFlow.InquirePriorAuth;
            mht.SubscriberSystem = mhts;
            mht.StateCode = "OH";
            mht.ModuleTransactionId = transactionID.ToString();
            mht.RequestTimestamp = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
            mht.SITransactionKey = sitTransactionKey;
            mht.RequestorSystem = MessageHeaderTypeRequestorSystem.PNM;

            InquirePriorAuthRequestPayloadType inqReqPay = new InquirePriorAuthRequestPayloadType();
            inqReqPay.PayorType = payorId;
            inqReqPay.PriorAuthNumber = priorAuthNUmber;
            inqReqPay.ProviderId = hdnNPI.Value;


            InquirePriorAuthRequest inq = new InquirePriorAuthRequest();
            inq.MessageHeader = mht;
            inq.RequestPayload = inqReqPay;

            string response = new PriorAuthServiceReqRes().priorAuthInquiryOperation(transactionID, inq);
            if (MAXIMUS.Core.Libraries.Constants.TransactionResult.TransactionFailed == response)
            {
                lblGeneralErr.Visible = true;
                lblGeneralErr.Text = "No records Found";
                return;
            }

            string res = InfoAccessController.GetResponsePayloadByPassThroughTransactionID(transactionID);
            if (string.IsNullOrEmpty(res))
            {
                MessageBox2.Show("PA inquiry results are empty.", "Error");
                return;
            }
            var xDocResp = XDocument.Parse(res);
            var serializerResp = new XmlSerializer(typeof(EnvelopeInqResponse));
            EnvelopeInqResponse sam = (EnvelopeInqResponse)serializerResp.Deserialize(new StringReader(xDocResp.ToString()));

            //priorAuthNUmber = HttpUtility.UrlEncode(Helper.Encrypt(priorAuthNUmber.Trim()));
            //payorId = HttpUtility.UrlEncode(Helper.Encrypt(payorId));
            //string Npi= HttpUtility.UrlEncode(Helper.Encrypt(this.hdnNPI.Value));
            //string url = string.Format("~/Process/SubmitPriorAuthorization.aspx?PA={0}&PayorId={1}&NPI={2}", priorAuthNUmber, payorId, Npi);
            //if (!string.IsNullOrEmpty(url))
            //{
            //    Response.Redirect(url);
            //}

            // SearchLinkClick(MAXIMUS.Core.Libraries.Constants.SectionTypeID.SubmitPriorAuthorization, sam.Body.InquirePriorAuthResponse);

        }
        catch (ThreadAbortException ex1)
        {
            lblGeneralErr.Visible = true;
            lblGeneralErr.Text = "No Data Found";
            // do nothing
        }
        catch (Exception exx)
        {
            //Logging log = new Logging(Guid.NewGuid(), logMsg);
            string response = "Response NULL";
            if (dsresponse != null)
            {
                response = dsresponse.ToString();
            }
            //log.CreateLogEntry(string.Format("{0} {1}", logHeader, exx.ToString() + "Response date :-" + response), Logging.LogPriority.Error);
            throw new Exception(string.Format("Error at method InquirePriorAuth and Response date :- {0}", response), exx);
        }
    }

    protected void lnkPriorAuthView_Click(object sender, EventArgs e)
    {
        try
        {
            //View Prior Auth using inquire Webservice call to redirect to Submit Prior Auth page.
            LinkButton btn = (LinkButton)sender;
            List<string> args = btn.CommandArgument.ToString().Split(',').ToList<string>();
            string PriorAuthNumber = args[0];
            string PayorType = args[1];

            PriorAuthNumber = HttpUtility.UrlEncode(Helper.Encrypt(PriorAuthNumber.Trim()));
            PayorType = HttpUtility.UrlEncode(Helper.Encrypt(PayorType));
            string Npi = HttpUtility.UrlEncode(Helper.Encrypt(this.hdnNPI.Value));
            string url = string.Format("~/Process/SubmitPriorAuthorization.aspx?PA={0}&PayorId={1}&NPI={2}", PriorAuthNumber, PayorType, Npi);
            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }

            //InquirePriorAuth(PriorAuthNumber, PayorType);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-LinkPriorAuthViewClick");
            MessageBox2.Show(string.Format("An error has occurred while prior auth view link click event. Reference Id : {0}", logNumber), "Error");
        }
    }

    protected void lnkPriorAuthView2_Click(object sender, EventArgs e)
    {

    }
    protected void lnkPriorAuthAttachment_Click(object sender, EventArgs e)
    {
        try
        {
            //View Prior Auth using inquire Webservice call to redirect to Submit Prior Auth page.
            LinkButton btn = (LinkButton)sender;
            List<string> args = btn.CommandArgument.ToString().Split(',').ToList<string>();
            string PriorAuthorizationID = args[0];
            string MemberID = args[1];

            PriorAuthorizationID = HttpUtility.UrlEncode(Helper.Encrypt(PriorAuthorizationID.Trim()));
            MemberID = HttpUtility.UrlEncode(Helper.Encrypt(MemberID));
            string url = string.Format("~/Process/StandaloneUploadAttachments.aspx?PANumber={0}&MemberID={1}&TransactionTypeID={2}", PriorAuthorizationID, MemberID,4);
            if (!string.IsNullOrEmpty(url))
            {
                Response.Redirect(url);
            }

            //InquirePriorAuth(PriorAuthNumber, PayorType);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-LinkPriorAuthViewClick");
            MessageBox2.Show(string.Format("An error has occurred while prior auth view link click event. Reference Id : {0}", logNumber), "Error");
        }
    }

    protected void lnkTrackingNumberView_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton lbtn = (LinkButton)sender;
            var cmdArgument = lbtn.CommandArgument.Split(',');
            var cmdname = lbtn.CommandName;
            if (cmdname == "Tracking")
            {
                this.WorkflowPage.RegistrationStep = 10002;
                //Raise the event and subscribe to it in Billing&OtherServices page

                string MedicaidId = HttpUtility.UrlEncode(Helper.Encrypt(cmdArgument[0]));
                string TrackingNo = HttpUtility.UrlEncode(Helper.Encrypt(cmdArgument[1]));

                string url = string.Format("~/Process/SubmitPriorAuthorization.aspx?MedicaidId={0}&TrackingNo={1}", MedicaidId, TrackingNo);
                if (!string.IsNullOrEmpty(url))
                {
                    Response.Redirect(url);
                }

                TrackingNumberSearchLink(MAXIMUS.Core.Libraries.Constants.SectionTypeID.SubmitPriorAuthorization, cmdArgument[0], cmdArgument[1]);
            }
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-LinkTrackingNumberViewClick");
            MessageBox2.Show(string.Format("An error has occurred while tracking number view link click event. Reference Id : {0}", logNumber), "Error");
        }
    }
    private void SearchBindGrid()
    {
        try
        {
            string medicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
            var _tracking_number = txtPatientTrackingNumber.Text.Trim();
            int? _status_type = Convert.ToInt32(ddlStatus.SelectedValue);
            int? _payerid = Convert.ToInt32(ddlPayerName.SelectedValue);
            DataSet ds = null;
            if ((_status_type ?? 0) == 13) //--saved
            {
                DataTable dt = null;
                ds = PriorAuthHospitalController.SearchPRIORAUTHTRACKINGByMedID(medicaidNumber, _tracking_number, (int)_status_type, _payerid);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvPasearchTracking.Visible = true;
                    gvPasearchTracking.DataSource = ds;
                    gvPasearchTracking.DataBind();

                    cpePasearch.Collapsed = false;
                    cpePasearch.ClientState = "false";
                }
                else
                {
                    gvPasearchTracking.Visible = true;
                    gvPasearchTracking.DataSource = null;
                    gvPasearchTracking.DataBind();
                    cpePasearch.Collapsed = false;
                    cpePasearch.ClientState = "false";
                }
            }
            else
            {
                gvPasearchTracking.Visible = true;
                gvPasearchTracking.DataSource = null;
                gvPasearchTracking.DataBind();
                cpePasearch.Collapsed = false;
                cpePasearch.ClientState = "false";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at SearchBindGrid method", ex);
        }
    }
    private void BindGrid()
    {
        try
        {
            //get the data from xml
            var ds = PriorAuthHospitalController.SearchPriorAuthResponse();
            DataTable dt = new DataTable();

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                if (ds.Tables[i].TableName == "AuthorizationInfo")
                {
                    dt = ds.Tables[i].Copy();
                }
                else
                {
                    if (ds.Tables[i].TableName == "AuthorizationService" || ds.Tables[i].TableName == "AuthorizationDiagnosis")
                    {
                        for (int column = 0; column < ds.Tables[i].Columns.Count; column++)
                        {
                            if (!dt.Columns.Contains(ds.Tables[i].Columns[column].ColumnName))
                            {
                                dt.Columns.Add(ds.Tables[i].Columns[column].ColumnName);
                            }
                        }
                        for (int row = 0; row < ds.Tables[i].Rows.Count; row++)
                        {
                            for (int column = 0; column < ds.Tables[i].Columns.Count; column++)
                            {
                                dt.Rows[row][ds.Tables[i].Columns[column].ColumnName] = ds.Tables[i].Rows[row][column];
                            }
                        }
                    }
                }
            }


            gvPasearch.DataSource = dt;
            gvPasearch.DataBind();
            if (dt.Rows.Count > 0)
            {
                pnlPasearch.Visible = true;
                pnlsepPasearch.Visible = true;
                cpePasearch.Collapsed = false;
                cpePasearch.ClientState = "false";
            }
            else
            {
                cpePasearch.Collapsed = true;
                cpePasearch.ClientState = "true";

            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at Bind method", ex);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        try
        {
            txtPatientTrackingNumber.Text = string.Empty;
            txtBirthDate.Text = string.Empty;
            txtDiagnoisCode.Text = string.Empty;
            txtPriorAuthNumber.Text = string.Empty;
            txtBirthDate.Text = string.Empty;
            txtMedicaidBillingNumber.Text = string.Empty;
            txtProcedureCode.Text = string.Empty;
            txtorderProvnpi.Text = string.Empty;
            txtICDCode.Text = string.Empty;
            txtRevenuecode.Text = string.Empty;
            txtSubmissiondate.Text = string.Empty;
            txtPAEffDate.Text = string.Empty;
            txtPAExpDate.Text = string.Empty;
            ddlAssignment.ClearSelection();
            ddlStatus.ClearSelection();
            ddlPayerName.ClearSelection();
            lblGeneralErr.Text = "";
            lblGeneralErr.Visible = false;
            gvPasearch.Visible = false;

            gvPasearchTracking.Visible = false;
            cpePasearch.Collapsed = false;
            cpePasearch.ClientState = "false";

            GetAssignments();
            GetStatus();
            GetPayerNames();
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-BtnClearClick");
            MessageBox2.Show(string.Format("An error has occurred while copy click event. Reference Id : {0}", logNumber), "Error");
        }
    }
    private void LoadProviderInformation()
    {
        try
        {
            string medicaidNumber = this.WorkflowPage.MedicaidID;   //Request.QueryString["MedicaidNumber"];
            if (medicaidNumber != null)
            {
                DataSet ds = svc.SelectProviderByGRPMedicaidID(medicaidNumber);
                DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
                this.DataList = dtMisc;
                if (Helper.HasRows(dtMisc))
                {
                    DataRow dr = dtMisc.Rows[0];
                    this.hdnNPI.Value = Helper.GetString("NPI", dr);
                }
            }
            else
            {
                throw new Exception("Medicaid Number is invalid");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error at LoadProviderInformation method", ex);
        }
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

    protected void gvPasearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvPasearch.PageIndex = e.NewPageIndex;
            SearchAuthSA(e.NewPageIndex);
        }
        catch (Exception ex)
        {
            string logNumber = CreateAndReturnLogThreadNumber(ex, "SearchPA-gvPASearchPageIndexChanging");
            MessageBox2.Show(string.Format("An error has occurred while grid page index change event. Reference Id : {0}", logNumber), "Error");
        }
    }
    public static string CreateAndReturnLogThreadNumber(Exception ex, string errorKey = "")
    {
        Logging logging = new Logging();
        string logMessage = errorKey + " " + logging.GetRecursiveException(ex);
        logging.CreateLogEntry(logMessage, CON.WebPageProcessName.SearchPriorAuthorization);
        return logging.ThreadId.ToString();
    }
}
