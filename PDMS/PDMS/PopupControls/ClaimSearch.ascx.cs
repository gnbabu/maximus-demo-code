using Corp.Core.Libraries;
using System;
using System.Data;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Corp.Core.Libraries.FI.ClaimsSearchReference;
using Corp.Core.Libraries.ClaimsManagement;
using MAXIMUS.Core.Libraries;
using CON = MAXIMUS.Core.Libraries.Constants;
using Dental = Corp.Core.Libraries.FI.ClaimsDentalService;
using Professional = Corp.Core.Libraries.FI.ClaimsProfessionalService;
using Institutional = Corp.Core.Libraries.FI.ClaimsInstitutionalService;
using System.Text;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public partial class PopupControls_ClaimSearch : BaseSectionControl
{
    private DataTable dtClaimsResponse;
    private DataTable dtResponseHeader;
    private DataTable dtClaimSearchResponse;
    private Logging log;
    private int TotalRecord;
    DateTime serviceFromDate;
    DateTime serviceToDate;
    private Guid m_threadId;
    private int Pageindex;
    private string SearchClaimText = "Please enter all available information before performing a search for an accurate retrieval. An open-ended search will delay or yield no results.";

    private Guid ThreadId
    {
        get
        {
            return this.m_threadId;
        }
        set
        {
            this.m_threadId = value;
        }
    }
    bool allowDBSearch = false;
    bool IsGridpaging = false;

    public override bool ValidateData()
    {
        bool isValid = true;

        if (!String.IsNullOrEmpty(txtDateofServfrom.Text))
        {
            //  Does not allow future date.  Does not allow greater than TO Date.  Does not allow more than 48 months.
            if (String.IsNullOrEmpty(txtDateofServto.Text))
            {
                isValid = AddValidationErrorMessage("* Date of Service (To) must exist if there is a Date of Service (From) date");
            }
            else if (!DateTime.TryParse(txtDateofServfrom.Text, out serviceFromDate))
            {
                isValid = AddValidationErrorMessage("* Date of Service (From) is invalid");
            }
            else if (!DateTime.TryParse(txtDateofServto.Text, out serviceToDate))
            {
                isValid = AddValidationErrorMessage("* Date of Service (To) is invalid");
            }
            else if (serviceFromDate > DateTime.Today)
            {
                isValid = AddValidationErrorMessage("* Date of Service (From) cannot be future date");
            }
            else if (serviceToDate > DateTime.Today)
            {
                isValid = AddValidationErrorMessage("* Date of Service (To) cannot be future date");
            }
            else if (serviceToDate < serviceFromDate)
            {
                isValid = AddValidationErrorMessage("* Date of Service (To) cannot be before the Date of Service (From)");
            }
            else if ((((serviceToDate.Year - serviceFromDate.Year) * 12) + serviceToDate.Month - serviceFromDate.Month) > 48)
            {
                isValid = AddValidationErrorMessage("* Date of Service (To) cannot be more than 48 months from Date of Service (From)");
            }


        }
        if (!String.IsNullOrEmpty(txtRaDate.Text))
        {
            DateTime dtRA = Convert.ToDateTime(txtRaDate.Text.Trim());

            if (dtRA > DateTime.Today)
            {
                isValid = AddValidationErrorMessage("* RA Date cannot be future date.");
            }


        }

        if (!String.IsNullOrEmpty(txtDateofServto.Text))
        {
            if (String.IsNullOrEmpty(txtDateofServfrom.Text))
            {
                isValid = AddValidationErrorMessage("* Date of Service (From) must exist if there is a Date of Service (To)");
            }
        }
        if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            string billingMedicaid = txtMedicaidBillingNumber.Text.Trim();
            if (billingMedicaid.Length < 12)
                isValid = AddValidationErrorMessage("* 12-digit number is required.");
        }
        if (string.IsNullOrEmpty(ddlMangedCarePlan.SelectedValue) && !string.Equals(ddlClaimStatus.SelectedItem.Text.ToString(), "Pending Submission"))
        {
            isValid = AddValidationErrorMessage("* Payor name is required");
            gvClaimSearchResult.DataSource = null;
            gvClaimSearchResult.DataBind();
        }

        return isValid;
    }

    public override void LoadData(DataRow dr)
    {
        //base.LoadData(dr);
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucClaimSearch" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valClaimSearch"; }
    }

    public override string Title
    {
        get { return "Claim Search"; }
    }


    public override bool SaveData()
    {
        return true;
    }

    public override void LoadControlData()
    {


    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ddlPageSize.SelectedIndex = 2;
        SearchClaimHelpTxt.Text = AppSettings.Get("SearchClaimHelpText", SearchClaimText);
        if (!IsPostBack)
        {
            ddlPageSize.SelectedIndex = 2;
            ClearForm();
            GetClaimStatus();
            GetPayerType();
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
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        try
        {
            gvClaimSearchResult.PageIndex = 0;
            gvClaimSearchResult.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindClaimSearchResult();
        }
        catch (Exception ex)
        {
            lblGeneralErr.Visible = true;
            lblGeneralErr.Text = "No results found.";
            Logging log = new Logging(Guid.NewGuid(), "Page Size:");
            log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
        }
    }
    private void GetClaimStatus()
    {

        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        ddlClaimStatus.Items.Clear();

        DataSet dataSet = _svc.GetClaimStatus();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlClaimStatus, dt, "PRIOR_AUTH_CLAIM_STATUS_TYPE", "PRIOR_AUTH_CLAIM_STATUS_ID", true);
        ddlClaimStatus.SelectedIndex = 0;
    }

    private void GetPayerType()
    {

        if (_svc == null)
        {
            _svc = new PDMSService.PDMSServiceClient();
        }
        ddlMangedCarePlan.Items.Clear();

        DataSet dataSet = _svc.LoadDestinationPayer();
        DataTable dt = dataSet.Tables[0];
        if (Helper.HasRows(dt))
        {
            Helper.LoadList(ddlMangedCarePlan, dt, "DESTINATION_PAYER_DESC", "DESTINATION_PAYER_ID", true);
            ddlMangedCarePlan.SelectedIndex = 0;
        }
    }
    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "ClaimSearch";
        this.Page.Validators.Add(val);
        return false;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearForm();
    }

    protected void ClearForm()
    {
        txtICNTCN.Text = "";
        ddlClaimType.SelectedIndex = 0;
        txtMedicaidBillingNumber.Text = "";
        txtPatAccountNumber.Text = "";
        txtDateofServfrom.Text = "";
        txtDateofServto.Text = "";
        ddlClaimStatus.SelectedIndex = 0;
        ddlMangedCarePlan.SelectedIndex = 0;
        lblGeneralErr.Text = "";
        lblGeneralErr.Visible = false;
        txtRaDate.Text = "";
        txtRenderingProviderId.Text = "";
        txtAmountBilled.Text = "";
        txtPrescriptionNumber.Text = "";
        gvClaimSearchResult.DataSource = null;
        gvClaimSearchResult.DataBind();
    }

    SearchClaimsRequestPayloadType payload = new SearchClaimsRequestPayloadType();
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetClaimData();
    }

    private void GetClaimData()
    {
        DataSet dsresponse = new DataSet();

        bool isValiddates = this.ValidateData();
        if (!isValiddates)
            return;
        Page.Validate("ClaimSearch");
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }

        try
        {
            if (!IsGridpaging)
            {
                gvClaimSearchResult.DataSource = null;
                gvClaimSearchResult.DataBind();
            }
            if (!string.IsNullOrEmpty(ddlClaimStatus.SelectedValue))
            {
                payload.Status = ddlClaimStatus.SelectedItem.ToString().ToUpper();

                if (payload.Status == "PENDING SUBMISSION")

                //if (payload.Status != "PAID" && payload.Status != "DENIED" && payload.Status != "REVERSED" && payload.Status != "IN PROCESS" && payload.Status != "OPEN" && payload.Status != "SUSPENDED")
                {
                    allowDBSearch = true;
                }
            }

            if (!string.IsNullOrEmpty(txtICNTCN.Text))
            {
                payload.ICN = txtICNTCN.Text.Trim();
            }
            //string billingmedicaid = ((Label)Parent.FindControl("lblProMedicaidID2")).Text;
            string billingmedicaid = this.WorkflowPage.MedicaidID;
            if (!string.IsNullOrEmpty(billingmedicaid))
            {
                payload.BillingProviderID = billingmedicaid;
            }
            //ohpnm-7794
            if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
            {
                payload.MemberMedicaidId = txtMedicaidBillingNumber.Text.Trim();
            }
            if (!string.IsNullOrEmpty(txtPatAccountNumber.Text))
            {
                payload.PatientAccountNumber = txtPatAccountNumber.Text;
            }
            if (!string.IsNullOrEmpty(txtPrescriptionNumber.Text))
            {
                payload.PrescriptionNumber = txtPrescriptionNumber.Text;
            }
            if (!string.IsNullOrEmpty(ddlClaimType.SelectedValue))
            {
                if (ddlClaimType.SelectedItem.ToString().ToUpper() == "DENTAL" || ddlClaimType.SelectedItem.ToString().ToUpper().Trim() == "D" || ddlClaimType.SelectedItem.ToString() == "0")
                    payload.ClaimType = "D";
                if (ddlClaimType.SelectedItem.ToString().ToUpper() == "INSTITUTIONAL" || ddlClaimType.SelectedItem.ToString().ToUpper().Trim() == "I" || ddlClaimType.SelectedItem.ToString() == "1")
                    payload.ClaimType = "I";
                if (ddlClaimType.SelectedItem.ToString().ToUpper() == "PROFESSIONAL" || ddlClaimType.SelectedItem.ToString().ToUpper().Trim() == "P" || ddlClaimType.SelectedItem.ToString() == "2")
                    payload.ClaimType = "P";
            }
            if (!string.IsNullOrEmpty(txtAmountBilled.Text))
            {
                payload.TotalCharges = Convert.ToDecimal(txtAmountBilled.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtRaDate.Text))
            {
                payload.RemittanceAdviceDateSpecified = true;
                payload.RemittanceAdviceDate = Convert.ToDateTime(txtRaDate.Text.Trim());
            }

            if (!string.IsNullOrEmpty(txtRenderingProviderId.Text))
            {
                payload.RenderingProviderID = txtRenderingProviderId.Text.Trim();
            }

            if (!string.IsNullOrEmpty(txtDateofServfrom.Text))
            {
                payload.FromDOSSpecified = true;
                payload.FromDOS = Convert.ToDateTime(txtDateofServfrom.Text.Trim());
            }
            if (!string.IsNullOrEmpty(txtDateofServto.Text))
            {
                payload.ThruDOSSpecified = true;
                payload.ThruDOS = Convert.ToDateTime(txtDateofServto.Text.Trim());
            }
            if (!string.IsNullOrEmpty(ddlMangedCarePlan.SelectedValue))
            {
                payload.PayorType = ddlMangedCarePlan.SelectedItem.Text;
                //if (ddlMangedCarePlan.SelectedValue == "1")
                //{
                //    //allowDBSearch = true;
                //    payload.PayorType = "FFS";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "2")
                //{
                //    payload.PayorType = "0021920";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "3")
                //{
                //    payload.PayorType = "0002937";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "4")
                //{
                //    payload.PayorType = "0021914";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "5")
                //{
                //    payload.PayorType = "0004202";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "6")
                //{
                //    payload.PayorType = "0003150";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "7")
                //{
                //    payload.PayorType = "00021919";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "8")
                //{
                //    payload.PayorType = "0007316";
                //}
                //else if (ddlMangedCarePlan.SelectedValue == "9")
                //{
                //    payload.PayorType = "0007610";
                //}
            }
            if (!string.IsNullOrEmpty(ddlPageSize.SelectedValue))
            {
                payload.PageSize = ddlPageSize.SelectedValue;
            }
            if (!string.IsNullOrEmpty(ddlPageSize.SelectedValue))
            {
                if (IsGridpaging)
                {
                    if ((gvClaimSearchResult.Rows.Count == 0) || (!string.IsNullOrEmpty(txtICNTCN.Text)))
                    {
                        payload.Offset = "0";
                    }
                    else
                    {
                        payload.Offset = ((gvClaimSearchResult.PageIndex) * (Convert.ToInt64(ddlPageSize.SelectedValue))).ToString();
                    }
                }
                else
                {
                    if ((gvClaimSearchResult.Rows.Count == 0) || (!string.IsNullOrEmpty(txtICNTCN.Text)))
                    {
                        payload.Offset = "0";
                    }
                }
            }
            if (allowDBSearch)
            {
                Logging log = new Logging(Guid.NewGuid(), "Claim DB Search");
                log.CreateLogEntry(string.Format("{0} {1}", "", "Claim Search in db for Medicade id: " + this.WorkflowPage.MedicaidID.Trim()), Logging.LogPriority.Information);
                dsresponse = GetDBData();
            }
            else
            {
                payload.TotalChargesSpecified = true;
                try
                {
                    Logging log = new Logging(Guid.NewGuid(), "Claim Search for FI");
                    log.CreateLogEntry(string.Format("{0} {1}", "", "Claim Search in db for Medicade id: " + this.WorkflowPage.MedicaidID.Trim()), Logging.LogPriority.Information);
                    var response = ClaimsSearchServiceAgent.CallClaimsSearchService(payload);
                    var xDoc = XDocument.Parse(response);

                    XmlReaderSettings settings = new XmlReaderSettings();
                    settings.DtdProcessing = DtdProcessing.Ignore;
                    settings.XmlResolver = null;
                    XmlReader xmlReader = XmlReader.Create(new StringReader(xDoc.Root.ToString()), settings);
                    dsresponse.ReadXml(xmlReader);


                }
                catch (Exception ex)
                {
                    lblGeneralErr.Text = "Claims Search returned invalid result: " + ex.Message;
                    lblGeneralErr.Visible = true;
                    PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimSearch, string.Empty, string.Empty, string.Empty,
                       payload.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                       0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, ex.Message, ex.Message);
                }
            }

           


            if (Helper.HasRows(dsresponse))
            {
                if (dtResponseHeader == null)
                {
                    dtResponseHeader = dsresponse.Tables["ResponseHeader"];
                }
                if (dtClaimsResponse == null)
                {
                    dtClaimsResponse = dsresponse.Tables["ClaimHeaderResponse"];
                }
                if (dtClaimSearchResponse == null)
                {
                    dtClaimSearchResponse = dsresponse.Tables["SearchClaimsResponse"];
                    if (Helper.HasRows(dtClaimSearchResponse))
                    {
                        if (dtClaimSearchResponse.Rows[0].Table.Columns.Contains("TotalRecords"))
                        {
                            if (!string.IsNullOrEmpty(dtClaimSearchResponse.Rows[0]["TotalRecords"].ToString()))
                                TotalRecord = Convert.ToInt32(dtClaimSearchResponse.Rows[0]["TotalRecords"].ToString());
                        }
                        hdnTotalCount.Value = Convert.ToString(TotalRecord);
                    }
                }

                if (allowDBSearch)
                {
                    lblGeneralErr.Visible = false;
                    lblGeneralErr.Text = "";
                    BindClaimSearchResult();
                    pnlClaimSearchResult.Visible = true;
                }
                else if (dtResponseHeader.Columns.Contains("ResponseType") && dtResponseHeader.Rows[0]["ResponseType"].ToString().Equals("FAILURE"))
                {
                    lblGeneralErr.Visible = true;
                    lblGeneralErr.Text = dtResponseHeader.Rows[0]["ResponseType"] + " " + dtResponseHeader.Rows[0]["ResponseCode"] + ": " + dtResponseHeader.Rows[0]["ResponseMessage"] + "<br />" + dtResponseHeader.Rows[0]["ResponseDetails"];
                    pnlClaimSearchResult.Visible = false;
                }
                else if (dtClaimsResponse == null)
                {
                    lblGeneralErr.Visible = true;
                    lblGeneralErr.Text = "No results found.";
                    pnlClaimSearchResult.Visible = false;
                }
                else
                {
                    lblGeneralErr.Visible = false;
                    lblGeneralErr.Text = "";
                    BindClaimSearchResult();
                    pnlClaimSearchResult.Visible = true;
                }
            }
        }
        catch
        {
            lblGeneralErr.Visible = true;
            lblGeneralErr.Text = "No results found.";
            pnlClaimSearchResult.Visible = false;
        }
    }

    private DataSet GetDBData()
    {
        DataSet dsresponse;
        List<SqlParameter> parameters = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(txtMedicaidBillingNumber.Text))
        {
            parameters.Add(SqlParms.CreateParameter("MedicalBillingNumber", DbType.String, txtMedicaidBillingNumber.Text.Trim(), true));
        }
        if (!string.IsNullOrEmpty(txtPatAccountNumber.Text))
        {
            parameters.Add(SqlParms.CreateParameter("PatientAccountNumber", DbType.String, txtPatAccountNumber.Text.Trim(), true));
        }
        if (!string.IsNullOrEmpty(ddlClaimType.SelectedValue))
        {
            parameters.Add(SqlParms.CreateParameter("ClaimType", DbType.String, ddlClaimType.SelectedValue.ToString(), true));
        }
        if (!string.IsNullOrEmpty(txtPrescriptionNumber.Text))
        {
            parameters.Add(SqlParms.CreateParameter("PrescriptionNumber", DbType.String, txtPrescriptionNumber.Text.Trim(), true));
        }
        if (!string.IsNullOrEmpty(txtAmountBilled.Text))
        {
            parameters.Add(SqlParms.CreateParameter("AmountBilled", DbType.String, txtAmountBilled.Text.Trim(), true));
        }
        if (!string.IsNullOrEmpty(ddlMangedCarePlan.SelectedValue))
        {
            parameters.Add(SqlParms.CreateParameter("PayorName", DbType.String, ddlMangedCarePlan.SelectedValue.ToString(), true));
        }
        if (!string.IsNullOrEmpty(txtRenderingProviderId.Text))
        {
            parameters.Add(SqlParms.CreateParameter("RenderingProviderID", DbType.String, txtRenderingProviderId.Text.Trim(), true));
        }
        if (!string.IsNullOrEmpty(txtDateofServfrom.Text))
        {
            parameters.Add(SqlParms.CreateParameter("FromDOS", DbType.DateTime, Convert.ToDateTime(txtDateofServfrom.Text.Trim()), true));
        }
        if (!string.IsNullOrEmpty(txtDateofServto.Text))
        {
            parameters.Add(SqlParms.CreateParameter("ToDOS", DbType.DateTime, Convert.ToDateTime(txtDateofServto.Text.Trim()), true));
        }
        if (!string.IsNullOrEmpty(this.WorkflowPage.MedicaidID))
        {
            parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, this.WorkflowPage.MedicaidID.Trim(), true));
        }
        dsresponse = DataAccess.ExecuteStoredProcedure("usp_Search_Claim", parameters, "Search_Claim");
        dtClaimsResponse = dsresponse != null ? dsresponse.Tables[0] : null;
        Logging log = new Logging(Guid.NewGuid(), "Claim DB Search");
        log.CreateLogEntry(string.Format("{0} {1}", "", "Claim Search in db for Medicade id: " + this.WorkflowPage.MedicaidID.Trim() + "with DB result : " + dsresponse.Tables[0].ToString()), Logging.LogPriority.Information);
        BindClaimSearchResult();
        return dsresponse;
    }

    protected void ddlClaimStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlMangedCarePlan_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GetClaimData();
    }

    protected void ddlClaimType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void grdClaimSearchResult_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void grdClaimSearchResult_SelectedIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gvClaimSearchResult.PageIndex = e.NewPageIndex;
        BindClaimSearchResult();
    }

    protected void BindClaimSearchResult()
    {
        try
        {
            if (!string.IsNullOrEmpty(ddlMangedCarePlan.SelectedValue))
            {

                if (ddlMangedCarePlan.SelectedValue == "1")
                {
                    // allowDBSearch = true;
                    payload.PayorType = "FFS";//"MMISODJFS";
                }
                else if (ddlMangedCarePlan.SelectedValue == "2")
                {
                    payload.PayorType = "0021920";
                }
                else if (ddlMangedCarePlan.SelectedValue == "3")
                {
                    payload.PayorType = "0002937";
                }
                else if (ddlMangedCarePlan.SelectedValue == "4")
                {
                    payload.PayorType = "0021914";
                }
                else if (ddlMangedCarePlan.SelectedValue == "5")
                {
                    payload.PayorType = "0004202";
                }
                else if (ddlMangedCarePlan.SelectedValue == "6")
                {
                    payload.PayorType = "0003150";
                }
                else if (ddlMangedCarePlan.SelectedValue == "7")
                {
                    payload.PayorType = "00021919";
                }
                else if (ddlMangedCarePlan.SelectedValue == "8")
                {
                    payload.PayorType = "0007316";
                }
                else if (ddlMangedCarePlan.SelectedValue == "9")
                {
                    payload.PayorType = "0007610";
                }
            }
            if (dtClaimsResponse != null)
            {
                Session["dtClaimsResponse"] = dtClaimsResponse;
                RefreshGridData();
            }
            else if (Session["dtClaimsResponse"] != null)
            {
                dtClaimsResponse = (DataTable)Session["dtClaimsResponse"];
                RefreshGridData();
            }
        }
        catch (Exception ex)
        {
            lblGeneralErr.Visible = true;
            lblGeneralErr.Text = "No results found.";
            Logging log = new Logging(Guid.NewGuid(), "BindClaimSearchResult:");
            log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
        }
    }

    private void RefreshGridData()
    {
        try
        {
            DataTable dt = new DataTable();
            IEnumerable<DataRow> dtResponse;
            string ClaimType = string.Empty;
            string DentalclaimType = string.Empty;
            string ProfclaimType = string.Empty;
            string InsticlaimType = string.Empty;

            //Reading the claim type from xml
            if (!string.IsNullOrEmpty(ddlClaimType.SelectedItem.Text))
            {
                if (Helper.HasRows(dtClaimsResponse))
                {
                    foreach (DataRow dr in dtClaimsResponse.Rows)
                    {
                        if (dr["ClaimType"].ToString().ToUpper() == "DENTAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "D" || dr["ClaimType"].ToString() == "0")
                            DentalclaimType = dr["ClaimType"].ToString();
                        if (dr["ClaimType"].ToString().ToUpper() == "PROFESSIONAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "P" || dr["ClaimType"].ToString() == "2")
                            ProfclaimType = dr["ClaimType"].ToString();
                        if (dr["ClaimType"].ToString().ToUpper() == "INSTITUTIONAL" || dr["ClaimType"].ToString().ToUpper().Trim() == "I" || dr["ClaimType"].ToString() == "1")
                            InsticlaimType = dr["ClaimType"].ToString();
                    }
                }
            }

            if (ddlClaimType.SelectedItem.Text.ToLower() == CON.ClaimType.PROFESSIONAL.ToString().ToLower())
            {
                ClaimType = allowDBSearch ? "Professional" : ProfclaimType;
            }
            if (ddlClaimType.SelectedItem.Text.ToLower() == CON.ClaimType.DENTAL.ToString().ToLower())
            {
                ClaimType = allowDBSearch ? "Dental" : DentalclaimType;
            }
            if (ddlClaimType.SelectedItem.Text.ToLower() == CON.ClaimType.INSTITUTIONAL.ToString().ToLower())
            {
                ClaimType = allowDBSearch ? "Institutional" : InsticlaimType;
            }
            if (!String.IsNullOrEmpty(ddlClaimType.SelectedItem.Text) && String.IsNullOrEmpty(ddlClaimStatus.SelectedItem.Text))
            {
                dtResponse = from row in dtClaimsResponse.AsEnumerable()
                             where row.Field<string>("ClaimType").ToLower().Trim() == ClaimType.ToLower()
                             select row;
                if (dtResponse.Any())
                {
                    dt = dtResponse.CopyToDataTable();
                }
            }
            else if (!String.IsNullOrEmpty(ddlClaimType.SelectedItem.Text) && !String.IsNullOrEmpty(ddlClaimStatus.SelectedItem.Text))
            {
                dtResponse = from row in dtClaimsResponse.AsEnumerable()
                             where (row.Field<string>("ClaimType").ToLower().Trim() == ClaimType.ToLower() &&
                             row.Field<string>("ClaimStatus").ToLower().Trim() == ddlClaimStatus.SelectedItem.Text.ToLower())
                             select row;
                if (dtResponse.Any())
                {
                    dt = dtResponse.CopyToDataTable();
                }
            }
            else if (!String.IsNullOrEmpty(ddlClaimStatus.SelectedItem.Text))
            {
                dtResponse = from row in dtClaimsResponse.AsEnumerable()
                             where (row.Field<string>("ClaimStatus").ToLower().Trim() == ddlClaimStatus.SelectedItem.Text.ToLower())
                             select row;
                if (dtResponse.Any())
                {
                    dt = dtResponse.CopyToDataTable();
                }
            }

            else if (String.IsNullOrEmpty(ddlClaimType.SelectedItem.Text) && !String.IsNullOrEmpty(ddlMangedCarePlan.SelectedItem.Text))
            {
                dtResponse = from row in dtClaimsResponse.AsEnumerable()
                             where (row.Field<string>("PayorType")) == payload.PayorType
                             select row;
                if (dtResponse.Any())
                {
                    dt = dtResponse.CopyToDataTable();
                }
            }
            //Check if optional fields don't exist and add those columns to datatable
            if (!dt.Columns.Contains("PatientAccountNumber"))
                dt.Columns.Add("PatientAccountNumber");
            if (!dt.Columns.Contains("MemberId"))
                dt.Columns.Add("MemberId");
            if (!dt.Columns.Contains("TotalPaidAmount"))
                dt.Columns.Add("TotalPaidAmount");
            if (!dt.Columns.Contains("RemittanceAdviceDate"))
                dt.Columns.Add("RemittanceAdviceDate");

            foreach (DataRow row in dt.Rows)
            {
                row["RemittanceAdviceDate"] = !string.IsNullOrEmpty(row["RemittanceAdviceDate"].ToString()) ? Convert.ToDateTime(row["RemittanceAdviceDate"]).ToString("MM/dd/yyyy") : "";
                row["FromDOS"] = !string.IsNullOrEmpty(row["FromDOS"].ToString()) ? Convert.ToDateTime(row["FromDOS"]).ToString("MM/dd/yyyy") : "";
                row["ThruDOS"] = !string.IsNullOrEmpty(row["ThruDOS"].ToString()) ? Convert.ToDateTime(row["ThruDOS"]).ToString("MM/dd/yyyy") : "";
                if (row["ClaimType"].ToString().ToUpper() == "PROFESSIONAL" || row["ClaimType"].ToString().ToUpper() == "P"
                        || row["ClaimType"].ToString().ToUpper() == "2")
                { row["ClaimType"] = CON.ClaimType.PROFESSIONAL.ToString(); }
                if (row["ClaimType"].ToString().ToUpper() == "INSTITUTIONAL" || row["ClaimType"].ToString().ToUpper() == "I"
                    || row["ClaimType"].ToString().ToUpper() == "1")
                { row["ClaimType"] = CON.ClaimType.INSTITUTIONAL.ToString(); }
                if (row["ClaimType"].ToString().ToUpper() == "DENTAL" || row["ClaimType"].ToString().ToUpper() == "D"
                    || row["ClaimType"].ToString().ToUpper() == "0")
                { row["ClaimType"] = CON.ClaimType.DENTAL.ToString(); }
            }
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    if (dt.Rows[i]["ClaimType"].ToString().ToUpper() == "PROFESSIONAL" || dt.Rows[i]["ClaimType"].ToString().ToUpper().Trim() == "P" || dt.Rows[i]["ClaimType"].ToString() == "2")
            //    {
            //        dt.Rows[i]["ClaimType"] = CON.ClaimType.PROFESSIONAL.ToString();
            //    }
            //    else if (dt.Rows[i]["ClaimType"].ToString().ToUpper() == "INSTITUTIONAL" || dt.Rows[i]["ClaimType"].ToString().ToUpper().Trim() == "I" || dt.Rows[i]["ClaimType"].ToString() == "1")
            //    {
            //        dt.Rows[i]["ClaimType"] = CON.ClaimType.INSTITUTIONAL.ToString();
            //    }
            //    else if (dt.Rows[i]["ClaimType"].ToString().ToUpper() == "DENTAL" || dt.Rows[i]["ClaimType"].ToString().ToUpper().Trim() == "D" || dt.Rows[i]["ClaimType"].ToString() == "0")
            //    {
            //        dt.Rows[i]["ClaimType"] = CON.ClaimType.DENTAL.ToString();
            //    }
            //}
            gvClaimSearchResult.DataSource = dt;
            //int totalpageing = 0;
            if (allowDBSearch)
            {
                gvClaimSearchResult.VirtualItemCount = dt.Rows.Count;
                gvClaimSearchResult.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                gvClaimSearchResult.DataBind();
            }
            else
            {
                //if (TotalRecord > Convert.ToInt32(ddlPageSize.SelectedValue))
                //{

                //    int pagesize = (dt.Rows.Count / Convert.ToInt32(ddlPageSize.SelectedValue));
                //    totalpageing = (TotalRecord / Convert.ToInt32(ddlPageSize.SelectedValue));
                //    totalpageing = Convert.ToInt32(ddlPageSize.SelectedValue);
                //}
                //else
                //{ totalpageing = 1; }
                if (!string.IsNullOrEmpty(hdnTotalCount.Value))
                    gvClaimSearchResult.VirtualItemCount = Convert.ToInt32(hdnTotalCount.Value);
                gvClaimSearchResult.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
                gvClaimSearchResult.DataBind();
            }
            // updatepnlSearch.Update();
        }
        catch (Exception ex)
        {
            lblGeneralErr.Visible = true;
            lblGeneralErr.Text = "No results found.";
            Logging log = new Logging(Guid.NewGuid(), "RefreshGridData:");
            log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
        }
    }

    protected void grdClaimSearchResult_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ShowClaimDetails")
        {
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string icn = commandArgs[0];
            string providerId = commandArgs[1];
            string claimType = commandArgs[2];
            //depending on the claim type call different claims service here
            try
            {
                string payerTypeData = GetPayerTypeData();
                icn = string.IsNullOrEmpty(icn) ? "" : HttpUtility.UrlEncode(Helper.Encrypt(icn.Trim()));
                claimType = HttpUtility.UrlEncode(Helper.Encrypt(claimType));
                payerTypeData = HttpUtility.UrlEncode(Helper.Encrypt(payerTypeData));
                string url = string.Format("~/Process/SubmitClaim.aspx?icn={0}&ct={1}&pt={2}", icn, claimType, payerTypeData);
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                lblGeneralErr.Text = "Service call failed for ICN: " + icn;
                lblGeneralErr.Visible = true;
                Logging log = new Logging(Guid.NewGuid(), "Inquiry Search");
                log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
            }
        }
        else if (e.CommandName == "RedirectClaim")
        {
            //int index = 0;
            //int.TryParse(e.CommandArgument.ToString(), out index);
            string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
            string icn = commandArgs[0];
            string patientControlNumber = commandArgs[1];
            string claimType = commandArgs[2];
            string claimPCN = commandArgs[4];
            string claimId = null;
            if (string.IsNullOrEmpty(icn))
            {
                DataSet dsresponse;
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(patientControlNumber))
                {
                    parameters.Add(SqlParms.CreateParameter("MedicalBillingNumber", DbType.String, patientControlNumber.Trim(), true));
                }
                if (!string.IsNullOrEmpty(claimPCN))
                {
                    parameters.Add(SqlParms.CreateParameter("PatientAccountNumber", DbType.String, claimPCN.Trim(), true));
                }
                if (!string.IsNullOrEmpty(this.WorkflowPage.MedicaidID))
                {
                    parameters.Add(SqlParms.CreateParameter("MedicaidID", DbType.String, this.WorkflowPage.MedicaidID.Trim(), true));
                }

                dsresponse = DataAccess.ExecuteStoredProcedure("usp_Search_Claim", parameters, "Search_Claim");
                if (Helper.HasRows(dsresponse))
                {
                    dtClaimsResponse = dsresponse != null ? dsresponse.Tables[0] : null;
                    claimId = dtClaimsResponse.Rows[0]["Claim_ID"].ToString();
                }
            }


            string MedicaidID = this.WorkflowPage.MedicaidID;
            Dictionary<string, string> param = new Dictionary<string, string>();

            DataSet ClaimsData = new DataSet();

            param.Add("Medicaid_Id", MedicaidID);
            param.Add("Claim_ID", claimId);

            if (claimType.ToUpper().Trim() == "DENTAL" || claimType.ToUpper().Trim() == "D" || claimType == "0")
                ClaimsData = svc.SelectPanelsData("claims_search_details", param);
            else if (claimType.ToUpper() == "INSTITUTIONAL" || claimType.ToUpper().Trim() == "I" || claimType == "1")
                ClaimsData = svc.SelectPanelsData("claims_search_details_inst", param);
            else if (claimType.ToUpper() == "PROFESSIONAL" || claimType.ToUpper().Trim() == "P" || claimType == "2")
                ClaimsData = svc.SelectPanelsData("claims_search_details_prof", param);

            try
            {
                if (CheckRowsInDataSet(ClaimsData))
                {
                    icn = string.IsNullOrEmpty(icn) ? "" : HttpUtility.UrlEncode(Helper.Encrypt(icn.Trim()));
                    claimType = HttpUtility.UrlEncode(Helper.Encrypt(claimType));
                    patientControlNumber = HttpUtility.UrlEncode(Helper.Encrypt(patientControlNumber));
                    claimPCN = HttpUtility.UrlEncode(Helper.Encrypt(claimPCN));
                    claimId = HttpUtility.UrlEncode(Helper.Encrypt(claimId));
                    string url = string.Format("~/Process/SubmitClaim.aspx?icn={0}&ct={1}&pcn={2}&pan={3}&cid={4}", icn, claimType, patientControlNumber, claimPCN, claimId);
                    Response.Redirect(url);
                    //RedirectClaimLinkClick(CON.SectionTypeID.SubmitClaim, claimType, claimId, ClaimsData);
                }
            }
            catch (Exception ex)
            {
                lblGeneralErr.Text = "Redirection failed for claimType: " + claimType;
                lblGeneralErr.Visible = true;
                Logging log = new Logging(Guid.NewGuid(), "SearchClaims");
                log.CreateLogEntry(string.Format("{0} {1}", "", ex.StackTrace.ToString()), Logging.LogPriority.Error);
            }
        }
    }

    private bool CheckRowsInDataSet(DataSet claimsData)
    {
        foreach (DataTable dt in claimsData.Tables)
        {
            if (dt.Rows.Count > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void GetDentalClaimInquiry(string icn)
    {
        try
        {
            string payerTypeData = GetPayerTypeData();
            DentalReqRes drr = new DentalReqRes();
            ThreadId = Guid.NewGuid();
            Logging log = new Logging(ThreadId, "DentalClaimInquireClaimRequestPayload");
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search InquireClaimRequestPayloadTypePayorType :", payerTypeData + "with ICN" + icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search ICN :", icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search MedicadeID :", this.WorkflowPage.MedicaidID + " with ICN" + icn), Logging.LogPriority.Information);
            try
            {
                var dentalInquireResponse = drr.InquireClaimRequest(payerTypeData, icn, this.WorkflowPage.MedicaidID);
                log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search dentalInquireResponse :", dentalInquireResponse + " with ICN" + icn), Logging.LogPriority.Information);

                //Check for any errors from the service
                if (dentalInquireResponse != null)
                {
                    if (dentalInquireResponse.Errors == null || string.IsNullOrEmpty(dentalInquireResponse.Errors[0].ErrorCode))
                    {
                        lblGeneralErr.Visible = false;
                        ClaimLinkClick(CON.SectionTypeID.SubmitClaim, dentalInquireResponse, null, null);

                    }
                    else
                    {
                        StringBuilder errs = new StringBuilder();
                        foreach (Dental.ErrorDetailsType edt in dentalInquireResponse.Errors)
                        {
                            errs.AppendLine(edt.ErrorCode + " : " + edt.ErrorDescription + "<br/>");
                        }
                        lblGeneralErr.Text = "Claims dental inquiry service call failed for ICN: " + icn + " with the following error(s):<br/>" + errs.ToString();
                        lblGeneralErr.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
                lblGeneralErr.Text = "Claims dental inquiry service call failed for ICN: " + icn;

                lblGeneralErr.Visible = true;
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
               "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Dental Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
            }
        }
        catch (FaultException ex)
        {
            lblGeneralErr.Text = "Error: An error occured while processing the dental claim inquiry request";
            lblGeneralErr.Visible = true;
            Logging log = new Logging(ThreadId, "DentalClaimSearch");
            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
        icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
        0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Dental Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), "DentalClaimSearch");
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                    icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                    0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Dental Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);

            log.CreateLogEntry(string.Format("{0} {1}", "Dental Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
        }
    }
    private string GetPayerTypeData()
    {
        string payerTypeData = null;
        if (!string.IsNullOrEmpty(ddlMangedCarePlan.SelectedValue))
        {
            if (ddlMangedCarePlan.SelectedValue == "1")
            {
                //allowDBSearch = true;
                payerTypeData = "FFS";
            }
            else if (ddlMangedCarePlan.SelectedValue == "2")
            {
                payerTypeData = "0021920";
            }
            else if (ddlMangedCarePlan.SelectedValue == "3")
            {
                payerTypeData = "0002937";
            }
            else if (ddlMangedCarePlan.SelectedValue == "4")
            {
                payerTypeData = "0021914";
            }
            else if (ddlMangedCarePlan.SelectedValue == "5")
            {
                payerTypeData = "0004202";
            }
            else if (ddlMangedCarePlan.SelectedValue == "6")
            {
                payerTypeData = "0003150";
            }
            else if (ddlMangedCarePlan.SelectedValue == "7")
            {
                payerTypeData = "00021919";
            }
            else if (ddlMangedCarePlan.SelectedValue == "8")
            {
                payerTypeData = "0007316";
            }
            else if (ddlMangedCarePlan.SelectedValue == "9")
            {
                payerTypeData = "0007610";
            }
        }
        return payerTypeData;
    }
    private void GetProfessionalClaimInquiry(string icn)
    {
        string payerTypeData = GetPayerTypeData();
        try
        {
            ProfessionalReqRes prr = new ProfessionalReqRes();
            ThreadId = Guid.NewGuid();
            Logging log = new Logging(ThreadId, "ProfessionalClaimInquireClaimRequestPayload");
            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search InquireClaimRequestPayloadTypePayorType :", payerTypeData + "with ICN" + icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search ICN :", icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search MedicadeID :", this.WorkflowPage.MedicaidID + " with ICN" + icn), Logging.LogPriority.Information);

            var proInquireResponse = prr.InquireClaimRequest(payerTypeData, icn, this.WorkflowPage.MedicaidID);
            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search professionalInquireResponse :", proInquireResponse + " with ICN" + icn), Logging.LogPriority.Information);

            if (proInquireResponse != null)
            {
                if (proInquireResponse.Errors == null || string.IsNullOrEmpty(proInquireResponse.Errors[0].ErrorCode))
                {
                    lblGeneralErr.Visible = false;
                    ClaimLinkClick(CON.SectionTypeID.SubmitClaim, null, proInquireResponse, null);
                }
                else
                {
                    StringBuilder errs = new StringBuilder();
                    foreach (Professional.ErrorDetailsType edt in proInquireResponse.Errors)
                    {
                        errs.AppendLine(edt.ErrorCode + " : " + edt.ErrorDescription + "<br/>");
                    }
                    lblGeneralErr.Text = "Claims professional inquiry service call failed for ICN: " + icn + " with the following error(s):<br/>" + errs.ToString();
                    lblGeneralErr.Visible = true;
                }
            }

        }
        catch (FaultException ex)
        {
            lblGeneralErr.Text = "Error: An error occured while processing the professional claim inquiry request";

            Logging log = new Logging(Guid.NewGuid(), "ProfessionalClaimSearch");
            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                 "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                  0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Professional Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), "ProfessionalClaimSearch");

            log.CreateLogEntry(string.Format("{0} {1}", "Professional Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                  "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                  0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Professional Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);

        }
    }
    private void GetInstitutionalClaimInquiry(string icn)
    {
        string payerTypeData = GetPayerTypeData();
        try
        {
            InstitutionalReqRes irr = new InstitutionalReqRes();
            ThreadId = Guid.NewGuid();
            Logging log = new Logging(ThreadId, "InstitutionalClaimInquireClaimRequestPayload");
            log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search InquireClaimRequestPayloadTypePayorType :", payerTypeData + "with ICN" + icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search ICN :", icn), Logging.LogPriority.Information);
            log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search MedicadeID :", this.WorkflowPage.MedicaidID + " with ICN" + icn), Logging.LogPriority.Information);
            try
            {
                var instInquireResponse = irr.InquireClaimRequest(payerTypeData, icn, this.WorkflowPage.MedicaidID);
                log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search InstitutionalInquireResponse :", instInquireResponse + " with ICN" + icn), Logging.LogPriority.Information);
                if (instInquireResponse != null)
                {
                    if (instInquireResponse.Errors == null || string.IsNullOrEmpty(instInquireResponse.Errors[0].ErrorCode))
                    {
                        lblGeneralErr.Visible = false;
                        ClaimLinkClick(CON.SectionTypeID.SubmitClaim, null, null, instInquireResponse);

                    }
                    else
                    {
                        StringBuilder errs = new StringBuilder();
                        foreach (Institutional.ErrorDetailsType edt in instInquireResponse.Errors)
                        {
                            errs.AppendLine(edt.ErrorCode + " : " + edt.ErrorDescription + "<br/>");
                        }
                        lblGeneralErr.Text = "Claims institutional inquiry service call failed for ICN: " + icn + " with the following error(s):<br/>" + errs.ToString();
                        lblGeneralErr.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
                PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                    "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                    0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Institutional Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
            }
        }
        catch (FaultException ex)
        {
            lblGeneralErr.Text = "Error: An error occured while processing the institutional claim inquiry request";

            Logging log = new Logging(Guid.NewGuid(), "InstitutionalClaimInquiry");
            log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                  "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                  0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Institutional Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);
        }
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), "InstitutionalClaimInquiry");

            log.CreateLogEntry(string.Format("{0} {1}", "Institutional Claim Inquiry Search", ex.ToString()), Logging.LogPriority.Error);
            PassthroughController.InsertPassthroughTransactionQueue((int)PassthroughController.PassthroughTransactionType.ClaimInquiry, string.Empty, string.Empty, string.Empty,
                  "ICN number: " + icn.ToString(), null, null, null, string.Empty, DateTime.Now, null, DateTime.Now, DateTime.Now, Methods.GetCurrentUserId().ToString(), Methods.GetCurrentUserId().ToString(),
                  0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "Institutional Inquery" + ex.Message.ToString(), string.Empty, string.Empty, string.Empty, string.Empty, DateTime.Now, string.Empty, string.Empty, null);

        }
    }

    public delegate void EventHandler(int step, Dental.InquireClaimResponse inquireClaimDentalResponse,
        Professional.InquireClaimResponse inquireClaimProfessionalResponse,
        Institutional.InquireClaimResponse inquireClaimInstResponse);
    public event EventHandler ClaimLinkClick;

    public delegate void RedirectClaimEventHandler(int step, string claimType, string claimId, DataSet ClaimsData);
    public event RedirectClaimEventHandler RedirectClaimLinkClick;

    protected void lnkView_Click(object sender, EventArgs e)
    {
        //View Claim using inquire Webservice call to redirect to Submit claim page.
    }


    protected void gvClaimSearchResult_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if ((e.Row.DataItem as DataRowView).Row["ClaimStatus"].ToString() != "Pending Submission")
            {

                var customerId = e.Row.FindControl("lnkPatientAccountNumber") as LinkButton;

                customerId.Style.Add("text-decoration", "none !important");
                customerId.Style.Add("pointer-events", "none");
                customerId.Style.Add("color", "black");
            }
        }
    }

    protected void gvClaimSearchResult_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvClaimSearchResult.PageIndex = e.NewPageIndex;
        Pageindex = gvClaimSearchResult.PageIndex;
        IsGridpaging = true;
        GetClaimData();
        //BindClaimSearchResult();
    }
    protected void gvClaimSearchResult_Sorting(object sender, GridViewSortEventArgs e)
    {

        if (gvClaimSearchResult.Attributes["CurrentSortDirection"] == "ASC")
        {
            gvClaimSearchResult.Attributes["CurrentSortDirection"] = "DESC";
        }
        else
        {
            gvClaimSearchResult.Attributes["CurrentSortDirection"] = "ASC";
        }
        string currentDirection = gvClaimSearchResult.Attributes["CurrentSortDirection"];
        DataTable dt = new DataTable();
        if (Session["dtClaimsResponse"] != null)
        {
            dt = (DataTable)Session["dtClaimsResponse"];
        }

        IEnumerable<DataRow> uniqueContacts;
        if (currentDirection.ToUpper() == "ASC")
        {
            uniqueContacts = dt.AsEnumerable().OrderBy(x => x.Field<string>("ICN"));
        }
        else
        {
            uniqueContacts = dt.AsEnumerable().OrderByDescending(x => x.Field<string>("ICN"));
        }
        if (uniqueContacts != null)
        {
            dtClaimsResponse = uniqueContacts.CopyToDataTable();
        }


        if (Helper.HasRows(dtClaimsResponse))
        {
            if (!dtClaimsResponse.Columns.Contains("PatientAccountNumber"))
                dtClaimsResponse.Columns.Add("PatientAccountNumber");
            if (!dtClaimsResponse.Columns.Contains("MemberId"))
                dtClaimsResponse.Columns.Add("MemberId");
            if (!dtClaimsResponse.Columns.Contains("TotalPaidAmount"))
                dtClaimsResponse.Columns.Add("TotalPaidAmount");
            if (!dtClaimsResponse.Columns.Contains("RemittanceAdviceDate"))
                dtClaimsResponse.Columns.Add("RemittanceAdviceDate");
            foreach (DataRow row in dtClaimsResponse.Rows)
            {
                row["RemittanceAdviceDate"] = !string.IsNullOrEmpty(row["RemittanceAdviceDate"].ToString()) ? Convert.ToDateTime(row["RemittanceAdviceDate"]).ToString("MM/dd/yyyy") : "";
                row["FromDOS"] = !string.IsNullOrEmpty(row["FromDOS"].ToString()) ? Convert.ToDateTime(row["FromDOS"]).ToString("MM/dd/yyyy") : "";
                row["ThruDOS"] = !string.IsNullOrEmpty(row["ThruDOS"].ToString()) ? Convert.ToDateTime(row["ThruDOS"]).ToString("MM/dd/yyyy") : "";
                if (row["ClaimType"].ToString().ToUpper() == "PROFESSIONAL" || row["ClaimType"].ToString().ToUpper() == "P"
                    || row["ClaimType"].ToString().ToUpper() == "2")
                { row["ClaimType"] = CON.ClaimType.PROFESSIONAL.ToString(); }
                if (row["ClaimType"].ToString().ToUpper() == "INSTITUTIONAL" || row["ClaimType"].ToString().ToUpper() == "I"
                    || row["ClaimType"].ToString().ToUpper() == "1")
                { row["ClaimType"] = CON.ClaimType.INSTITUTIONAL.ToString(); }
                if (row["ClaimType"].ToString().ToUpper() == "DENTAL" || row["ClaimType"].ToString().ToUpper() == "D"
                    || row["ClaimType"].ToString().ToUpper() == "0")
                { row["ClaimType"] = CON.ClaimType.DENTAL.ToString(); }
            }

            gvClaimSearchResult.DataSource = dtClaimsResponse;
            gvClaimSearchResult.VirtualItemCount = dt.Rows.Count;//Convert.ToInt32(hdnTotalCount.Value);
            gvClaimSearchResult.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvClaimSearchResult.DataBind();
            gvClaimSearchResult.PageIndex = Pageindex;
            Session["dtClaimsResponse"] = dtClaimsResponse;

        }

    }
    protected void gvClaimSearchResult_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (gvClaimSearchResult.Attributes["CurrentSortField"] != null && gvClaimSearchResult.Attributes["CurrentSortDirection"] != null)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (cell.HasControls())
                    {
                        LinkButton sortLink = null;
                        if (cell.Controls[0] is LinkButton)
                        {
                            sortLink = (LinkButton)cell.Controls[0];
                        }
                        if (sortLink != null && gvClaimSearchResult.Attributes["CurrentSortField"] == sortLink.CommandArgument)
                        {
                            Image img = new Image();
                            img.Width = System.Web.UI.WebControls.Unit.Pixel(10);
                            img.Height = System.Web.UI.WebControls.Unit.Pixel(10);
                            if (gvClaimSearchResult.Attributes["CurrentSortDirection"] == "ASC")
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortAsc.gif";
                            }
                            else
                            {
                                img.ImageUrl = "~/App_Themes/Default/Grid/SortDesc.gif";
                            }
                            cell.Controls.Add(new LiteralControl("&nbsp;"));
                            cell.Controls.Add(img);
                        }
                    }
                }
            }
        }
    }
}