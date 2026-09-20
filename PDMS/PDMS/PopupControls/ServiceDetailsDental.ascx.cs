using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PopupControls_ServiceDetailsDental : System.Web.UI.UserControl
{
    public delegate void EventHandler();
    public event EventHandler RefreshChildGrids;
    private bool fromInquirySvc = false;
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

    public void HideDivsForReSubmitCopyandAdjust()
    {
        divPaidAmount.Visible = false;
        divPaidUnit.Visible = false;
    }
    public void SetButtonVisibility()
    {

    }
    DataSet dsDentalDiagnosisPointer;
    #region "property"
    private DataSet dsServiceDetails = new DataSet();
    private ClaimsServiceAgent ClaimService = null;
    private string PriorAuthorizationNumber = string.Empty;
    private string ReferralNumber = string.Empty;
    private string PlaceofService = string.Empty;
    public string IcnNumber = string.Empty;
    private string dentalStatus = string.Empty;
    public string DentalStatus
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(dentalStatus))
                return dentalStatus;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                dentalStatus = value;
        }
    }
    private bool IsClaimCopyVal = false;
    public bool IsClaimCopy
    {
        get
        {
            return IsClaimCopyVal;
        }
        set
        {
            IsClaimCopyVal = value;
        }
    }
    public string ServiceInfoPlaceofService
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(PlaceofService))
                return PlaceofService;

            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                PlaceofService = value.Trim();
        }

    }
    public string ReferringPanelPriorAuthorizationNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(PriorAuthorizationNumber))
                return PriorAuthorizationNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                PriorAuthorizationNumber = value.Trim();
        }

    }
    public string ReferringPanelReferralNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ReferralNumber))
                return ReferralNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ReferralNumber = value.Trim();
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
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(IcnNumber))
                return IcnNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                IcnNumber = value.Trim();
        }
    }
    private DataSet dsServiceDetailsDentalData = new DataSet();

    public string DateOfservice
    {
        get
        {
            DataSet ds = FetchOtherServiceDetailsInformation();
            if (Helper.HasRows(ds))
            {
                // trust that the 
                String firstDate = ds.Tables[0].Rows[0]["ServiceDate"].ToString().Replace("-", "");
                String lastDate = ds.Tables[0].Rows[ds.Tables[0].Rows.Count - 1]["ServiceDate"].ToString().Replace("-", "");
                if (firstDate.Equals(lastDate))
                {
                    return firstDate;
                }
                else if (string.IsNullOrEmpty(firstDate) && !string.IsNullOrEmpty(lastDate))
                {
                    return lastDate;
                }
                else if (!string.IsNullOrEmpty(firstDate) && string.IsNullOrEmpty(lastDate))
                {
                    return firstDate;
                }
                else
                {
                    return firstDate + "-" + lastDate;
                }
            }
            else
            {
                return string.Empty;
            }
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtdateofserviceDental.Text = value.Trim();
        }
    }

    public DataSet dataSetServiceDetails
    {
        get
        {
            if (!Helper.HasRows(dsServiceDetailsDentalData))
            {
                if (Helper.HasRows(FetchOtherServiceDetailsInformation()))
                {
                    return dsServiceDetailsDentalData;
                }
            }
            return dsServiceDetailsDentalData;

        }

        set
        {
            if (value != null)
            {
                SetServiceDetails(value);
            }
        }

    }
    private bool _displayReadOnly;
    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            //List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
            //SetDentalServiceDetailPanelData(listData, null);
        }
    }
    #endregion
    public PopupControls_ServiceDetailsDental()
    {
        ClaimService = new ClaimsServiceAgent(ViewState);
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["ddlDignosisDentalThirdIndex"] = ddlDignosisDentalThird.SelectedIndex;
        EnableDropDowns();
        List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
        clExtender.EndDate = DateTime.Now.Date;

        if (!Page.IsPostBack)
        {
            // CheckDignosisCode();
            ddlOralCavityDentalFirst.Items.Clear();
            ddlOralCavityDentalSecond.Items.Clear();
            ddlOralCavityDentalThird.Items.Clear();
            ddlOralCavityDentalFourth.Items.Clear();
            ddlOralCavityDentalFifth.Items.Clear();

            var dsOralCavity = LookupTableController.GetOralCavity();
            if (dsOralCavity != null)
            {
                var dtOralcavity = dsOralCavity.Tables[0];

                Helper.LoadList(ddlOralCavityDentalFirst, dtOralcavity, "CODE", "CODE", true);
                Helper.LoadList(ddlOralCavityDentalSecond, dtOralcavity, "CODE", "CODE", true);
                Helper.LoadList(ddlOralCavityDentalThird, dtOralcavity, "CODE", "CODE", true);
                Helper.LoadList(ddlOralCavityDentalFourth, dtOralcavity, "CODE", "CODE", true);
                Helper.LoadList(ddlOralCavityDentalFifth, dtOralcavity, "CODE", "CODE", true);
            }
        }
        else
        {

            btnServiceDetailsDentailAdd.Attributes.Add("onclick", "DisableEnable_Add_ServiceDetail_Dental();");
            // btnUpdateServiceDetailDental.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnUpdateServiceDetailDental, null) + ";");

        }

        if (!string.IsNullOrEmpty(hdnClaimId.Value) && !FromInquirySvc)
        {
            SetDentalServiceDetailPanelData();
            SetServiceDetails(dsServiceDetailsDentalData);
        }

        //SetDentalServiceDetailPanelData(listData, null);
        GetPriorAuthReferralDetails();
        lblStatusServiceDetails.Text = DentalStatus;
        //if (Session["ClaimStatus"] != null)
        //{
        //    if (Session["ClaimStatus"].ToString() == "Pending Submission" && providerDentalServicedetailOutput.Visible == true)
        //    {
        //        hdnDentalServiecLineClaimStatus.Value = "Pending Submission";
        //        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displayDentalServiceDetailTable();</script>", false);
        //    }
        //}
        //if (providerDentalServicedetailOutput.Visible == true)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>clearDentalServiceDetailFields();</script>", false);
        //}
        hdnProviderTypeId.Value = this.WorkflowPage.ProviderTypeID.ToString();
    }
    private DataSet FetchOtherServiceDetailsInformation()
    {
        DataSet dsServiceDetailsInfo = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
        parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, "0", true));
        dsServiceDetailsDentalData = dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_DataForClaims", parameters, "Claims_Service_Details");
        return dsServiceDetailsInfo;
    }
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
    DataSet dsDentalclaimGrid;
    protected void Page_PreRender(object sender, EventArgs e)
    {
        string txtToday = DateTime.Now.ToShortDateString();

        if (!string.IsNullOrEmpty(ucSubmitClaimSearchProc.ProcedureCode))
        {
            txtProducerCodeDental.Text = ucSubmitClaimSearchProc.ProcedureCode;


        }
        if (Session["PlaceofServiceCode"] != null)
        {
            txtplaceofserviceDental.Text = Session["PlaceofServiceCode"].ToString();
            Session["PlaceofServiceCode"] = null;
        }
        if (ddlDiagnosisDentalFirst.SelectedIndex <= 0)
        {
            CheckDignosisCode();
        }
        GetPriorAuthReferralDetails();

    }
    public void GetPriorAuthReferralDetails()
    {

        if (!string.IsNullOrEmpty(ReferringPanelPriorAuthorizationNumber))
        {
            txtPriorAuthNumberDental.Enabled = true;
        }
        else
        {
            txtPriorAuthNumberDental.Enabled = false;

        }
        if (!string.IsNullOrEmpty(ReferringPanelReferralNumber))
        {
            txtReferralDental.Enabled = true;
        }
        else
        {
            txtReferralDental.Enabled = false;

        }

    }
    protected void btnServiceDetailsDentailAdd_Click(object sender, EventArgs e)
    {

        if (Page.IsValid)
        {
            if (ValidateDentalData())
            {
                string cde_proc_add = txtProducerCodeDental.Text;
                List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
                dsDentalclaimGrid = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    var dentalservicedetail = new DentalServiceDetail();
                    if (cde_proc_add != string.Empty && !string.IsNullOrEmpty(txtChargesDental.Text)
                        && !string.IsNullOrEmpty(txtBillUnitDental.Text)
                        && !string.IsNullOrEmpty(txtdateofserviceDental.Text))
                    {
                        dentalservicedetail.num_dtl_total = lblDetailsItemDental.Text;
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        parms.Add("Claim_id", hdnClaimId.Value);
                        DataSet dsSerialNo = svc.SelectPanelsData("Claims_Service_Details", parms);

                        //lblDetailsItemDental.Text = (listData.Count + 1).ToString();
                        dentalservicedetail.cde_proc = txtProducerCodeDental.Text.ToUpper();
                        dentalservicedetail.plc_service = txtplaceofserviceDental.Text;
                        dentalservicedetail.mdf_first = txtModifierDentalFirst.Text.ToUpper();
                        dentalservicedetail.mdf_secnd = txtModifierDentalSecond.Text.ToUpper();
                        dentalservicedetail.mdf_thrd = txtModifierDentalThird.Text.ToUpper();
                        dentalservicedetail.mdf_forth = txtModifierDentalFourth.Text.ToUpper();
                        dentalservicedetail.cde_clm_chrge = string.IsNullOrEmpty(txtChargesDental.Text) ? "0" : txtChargesDental.Text;
                        dentalservicedetail.line_ctr_num = txtLineControllerNoDental.Text;
                        dentalservicedetail.digno_first = ddlDiagnosisDentalFirst.SelectedValue;
                        dentalservicedetail.digno_sec = ddlDignosisDentalSecond.SelectedValue;
                        dentalservicedetail.digno_third = ddlDignosisDentalThird.SelectedValue;
                        dentalservicedetail.digno_forth = ddlDignosisDentalFourth.SelectedValue;
                        dentalservicedetail.pad_amnt = lblpaidAmountDental.Text;
                        dentalservicedetail.prior_auth = txtPriorAuthNumberDental.Text;
                        dentalservicedetail.orl_cvt_first = ddlOralCavityDentalFirst.SelectedValue;
                        dentalservicedetail.orl_cvt_sec = ddlOralCavityDentalSecond.SelectedValue;
                        dentalservicedetail.orl_cvt_third = ddlOralCavityDentalThird.SelectedValue;
                        dentalservicedetail.orl_cvt_forth = ddlOralCavityDentalFourth.SelectedValue;
                        dentalservicedetail.orl_cvt_fifth = ddlOralCavityDentalFifth.SelectedValue;
                        dentalservicedetail.bil_unt = txtBillUnitDental.Text;
                        dentalservicedetail.ref_num = txtReferralDental.Text;
                        dentalservicedetail.prosthesis_cd = ddlInlyneCodeDental.SelectedValue;
                        dentalservicedetail.pad_unt = lblPaidUnitDental.Text;
                        dentalservicedetail.qty_billed = string.IsNullOrEmpty(lblpaidAmountDental.Text) ? "0" : lblpaidAmountDental.Text;
                        if (!string.IsNullOrEmpty(txtdateofserviceDental.Text))
                            dentalservicedetail.ServiceDate = Convert.ToDateTime(txtdateofserviceDental.Text);
                        dentalservicedetail.amt_billed = string.IsNullOrEmpty(txtChargesDental.Text) ? "0" : txtChargesDental.Text;
                        dentalservicedetail.cde_clm_status = "Pending Submission";
                    }

                    SaveClaimsDentalServiceDetails(lblDetailsItemDental.Text, txtProducerCodeDental.Text, txtplaceofserviceDental.Text, txtModifierDentalFirst.Text,
                           txtModifierDentalSecond.Text, txtModifierDentalThird.Text, txtModifierDentalFourth.Text, txtChargesDental.Text, txtdateofserviceDental.Text, txtLineControllerNoDental.Text,
                           ddlDiagnosisDentalFirst.SelectedValue.ToString(), ddlDignosisDentalSecond.SelectedValue.ToString(), ddlDignosisDentalThird.SelectedValue.ToString(), ddlDignosisDentalFourth.SelectedValue.ToString(),
                          lblpaidAmountDental.Text, txtPriorAuthNumberDental.Text, ddlOralCavityDentalFirst.SelectedValue.ToString(), ddlOralCavityDentalSecond.SelectedValue.ToString(),
                          ddlOralCavityDentalThird.SelectedValue.ToString(), ddlOralCavityDentalFourth.SelectedValue.ToString(), ddlOralCavityDentalFifth.SelectedValue.ToString(), txtChargesDental.Text,
                          txtBillUnitDental.Text, txtReferralDental.Text, ddlInlyneCodeDental.SelectedValue.ToString(), lblPaidUnitDental.Text, ddlInlyneCodeDental.SelectedValue.ToString(),
                          lblStatusServiceDetails.Text, "Claims_Service_Details");
                    //SetDentalServiceDetailPanelData(listData, null);
                    ClearDentalControl();
                    //int rowCount =gvServiceDetailDental.Rows.Count;
                    if (Helper.HasRows(dsDentalclaimGrid))
                    {
                        if (dsDentalclaimGrid.Tables[0].Rows.Count == 50)
                        {
                            btnServiceDetailsDentailAdd.Visible = false;
                        }
                        else
                        {
                            btnServiceDetailsDentailAdd.Visible = true;
                            btnUpdateServiceDetailDental.Visible = false;
                            btnCancelDental.Visible = false;
                        }

                    }
                }
            }
        }
    }

    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtdateofserviceDental.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate2();</script>", false);
        }
        else
        {
            birthDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtdateofserviceDental.Text))
        {
            if (InValidDateError())
            {
                return;
            }
        }
    }
    public bool InValidDateError()
    {
        lblErrorDOS.Text = string.Empty;
        DateTime dt = default(DateTime);
        bool valid = DateTime.TryParseExact(txtdateofserviceDental.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        if (!valid)
        {
            birthDateRequiredError1.Visible = false;
            lblErrorDOS.Text = "Enter Valid Date or Format (MM/DD/YYYY)";
            txtdateofserviceDental.Text = string.Empty;
            return true;
        }
        else
        {
            lblErrorDOS.Text = string.Empty;
            return false;
        }
    }
    protected void lnkProducerCodeDental_Click(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchProc.Show();
    }

    public void ClearDentalControl()
    {
        lblDenTotalCharges.Text = "";
        lblDenTotalAmountPaid.Text = "";
        txtProducerCodeDental.Text = "";
        txtplaceofserviceDental.Text = "";
        txtModifierDentalFirst.Text = "";
        txtModifierDentalSecond.Text = "";
        txtModifierDentalThird.Text = "";
        txtModifierDentalFourth.Text = null;
        txtChargesDental.Text = null;
        txtdateofserviceDental.Text = "";
        txtLineControllerNoDental.Text = null;
        ddlDiagnosisDentalFirst.ClearSelection();
        ddlDignosisDentalSecond.ClearSelection();
        ddlDignosisDentalThird.ClearSelection();
        ddlDignosisDentalFourth.ClearSelection();
        lblpaidAmountDental.Text = "0";
        txtPriorAuthNumberDental.Text = null;
        ddlOralCavityDentalFirst.SelectedIndex = 0;
        ddlOralCavityDentalSecond.SelectedIndex = 0;
        ddlOralCavityDentalThird.SelectedIndex = 0;
        ddlOralCavityDentalFourth.SelectedIndex = 0;
        ddlOralCavityDentalFifth.SelectedIndex = 0;
        txtBillUnitDental.Text = null;
        txtReferralDental.Text = null;
        ddlInlyneCodeDental.SelectedIndex = 0;
        lblPaidUnitDental.Text = "0";
        txtdateofserviceDental.Text = "";
        List<DentalServiceDetail> listData = ClaimService.DentalServiceDetails;
        listData = listData.OrderBy(o => o.num_dtl_total).ToList();
        providerDentalServicedetailOutput.InnerHtml = "";
    }

    public void ClearGrid()
    {
        providerDentalServicedetailOutput.InnerHtml = "";

    }
    protected void ReportProcedureDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtdateofserviceDental.Text, true) && Helper.IsValidDate(txtdateofserviceDental.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtdateofserviceDental.Text).Subtract(Convert.ToDateTime(txtdateofserviceDental.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {

        }
    }

    public void SaveClaimsDentalServiceDetails(string _slLine, string _producerDentalCode, string _placeofserviceDental, string _modifierDentalFirst,
        string _modifierDentalSecond, string _modifierDentalThird, string _modifierDentalFourth, string _chargesDental, string _dentalDateOfService,
        string _lineControllerNoDental, string _diagnosisDentalFirst, string _dignosisDentalSecond, string _dignosisDentalThird,
        string _dignosisDentalFourth, string _paidAmountDental,
        string _priorAuthNumberDental, string _oralCavityDentalFirst, string _oralCavityDentalSecond, string _OralCavityDentalThird,
        string _oralCavityDentalFourth, string _oralcavityDentalFifth, string _totalChages, string _billUnitDental, string _referralDental, string _inlyneCodeDental, string _paidUnitDental, string _prosthesisCrownInlayCode,
        string _statusServiceDetails, string tableName)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimId.Value);

        //DataSet dsSerialNo = svc.SelectPanelsData("Claims_Service_Details", parms);
        //if (Helper.HasRows(dsSerialNo))
        //{
        //    _slLine = (Convert.ToInt32(dsSerialNo.Tables[0].Rows[0]["totalcount_Serialno"]) + 1).ToString();
        //}
        //else
        //{ _slLine = "1"; }
        _slLine = (Convert.ToInt32(lblDetailsItemDental.Text)).ToString();
        parms.Add("Service_Line", _slLine);
        parms.Add("Procedure_Code", string.IsNullOrEmpty(_producerDentalCode) ? null : _producerDentalCode);
        parms.Add("Date_of_Service", _dentalDateOfService.ToString());
        parms.Add("Line_Control_Number", string.IsNullOrEmpty(_lineControllerNoDental) ? null : _lineControllerNoDental);
        parms.Add("Prior_Authorization_Number", string.IsNullOrEmpty(_priorAuthNumberDental) ? null : _priorAuthNumberDental);
        parms.Add("Referral_Number", string.IsNullOrEmpty(_referralDental) ? null : _referralDental);
        parms.Add("Place_of_Service", string.IsNullOrEmpty(_placeofserviceDental) ? null : _placeofserviceDental);
        parms.Add("Modifier1", txtModifierDentalFirst.Text);
        parms.Add("Modifier2", txtModifierDentalSecond.Text);
        parms.Add("Modifier3", txtModifierDentalThird.Text);
        parms.Add("Modifier4", txtModifierDentalFourth.Text);
        parms.Add("Diagnosis_Pointer1", ddlDiagnosisDentalFirst.SelectedValue.ToString());
        parms.Add("Diagnosis_Pointer2", ddlDignosisDentalSecond.SelectedValue.ToString());
        parms.Add("Diagnosis_Pointer3", ddlDignosisDentalThird.SelectedValue.ToString());
        parms.Add("Diagnosis_Pointer4", ddlDignosisDentalFourth.SelectedValue.ToString());

        parms.Add("Oral_Cavity1", ddlOralCavityDentalFirst.SelectedValue.ToString());
        parms.Add("Oral_Cavity2", ddlOralCavityDentalSecond.SelectedValue.ToString());
        parms.Add("Oral_Cavity3", ddlOralCavityDentalThird.SelectedValue.ToString());
        parms.Add("Oral_Cavity4", ddlOralCavityDentalFourth.SelectedValue.ToString());
        parms.Add("Oral_Cavity5", ddlOralCavityDentalFifth.SelectedValue.ToString());
        parms.Add("Prosthesis_Crown_InlayCode", string.IsNullOrEmpty(_prosthesisCrownInlayCode) ? null : _prosthesisCrownInlayCode);
        parms.Add("Status", _statusServiceDetails);
        parms.Add("Charges", string.IsNullOrEmpty(_chargesDental) ? null : _chargesDental);
        if (!string.IsNullOrEmpty(_paidAmountDental))
        {
            string paidamount = _paidAmountDental.Contains('$') ? _paidAmountDental.Replace('$', ' ').TrimStart() : _paidAmountDental;
            parms.Add("Paid_Amount", paidamount);
        }
        else
        {
            parms.Add("Paid_Amount", null);

        }
        parms.Add("Billed_Units", string.IsNullOrEmpty(_billUnitDental) ? null : _billUnitDental);
        parms.Add("Paid_Units", string.IsNullOrEmpty(_paidUnitDental) ? null : _paidUnitDental);
        parms.Add("Total_Charges", string.IsNullOrEmpty(_totalChages) ? null : _totalChages);
        parms.Add("Last_modified_date", DateTime.Now.ToString());
        parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Created_date_time", DateTime.Now.ToString());
        parms.Add("Created_by_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Claim_Type", "0");
        ClaimsController.InsertPanelsData(tableName, parms);
    }

    public void SetDentalServiceDetailPanelData()
    {

        List<SqlParameter> parameters = new List<SqlParameter>();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
            dsDentalclaimGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");

            if (Helper.HasRows(dsDentalclaimGrid))
            {
                lblDetailsItemDental.Text = (dsDentalclaimGrid.Tables[0].Rows.Count + 1).ToString();
            }
            else
            {
                lblDetailsItemDental.Text = "1";
                providerDentalServicedetailOutput.InnerHtml = "";
            }
        }
        else
        {
            lblDetailsItemDental.Text = "1";
            providerDentalServicedetailOutput.InnerHtml = "";

        }

    }

    private void SetServiceDetails(DataSet dsServiceDetails)
    {
        //gvServiceDetailDental.DataSource = dsServiceDetails;
        //gvServiceDetailDental.DataBind();
        if (DentalStatus.ToUpper() == "PAID" && IsClaimCopy)
        {
            hdnHidePaidAmounts.Value = "1";
        }
        hdnDentalServiecLineClaimStatus.Value = "";
        DataSet dsServiceDetailsDental = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            if (Helper.HasRows(dsServiceDetails))
            {
                dsServiceDetailsDental = dsServiceDetails;
            }
            else
            {
                dsServiceDetailsDental = FetchOtherServiceDetailsInformation();
            }
        }
        if (Helper.HasRows(dsServiceDetailsDental))
        {
            DataTable diagTable = dsServiceDetailsDental.Tables[0];
            providerDentalServicedetailOutput.InnerHtml = "";
            if (diagTable.Rows.Count > 0)
            {
                string tab = string.Empty;
                int sequence = 0;
                decimal totalcharge = 0;
                decimal totalAmoutPaid = 0;

                tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Procedure Code</th><th style='width:10px; scope='col'>Place Of Service</th><th style='width:10px; scope='col'>*Billed Units</th><th style='width:10px; scope='col'>Paid Units</th><th style='width:10px; scope='col'>Date Of Service</th><th style='width:10px; scope='col'>Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                foreach (DataRow dr in diagTable.Rows)
                {

                    sequence++;
                    string sequencedata = null;

                    sequencedata = sequence.ToString();

                    string cde_proc = dr["cde_proc"].ToString();
                    string plc_service = dr["plc_service"].ToString();
                    string bil_unt = dr["bil_unt"].ToString();
                    string pad_unt = dr["pad_unt"].ToString();
                    string ServiceDate = dr["ServiceDate"].ToString();
                    string cde_clm_chrge = null;
                    string pad_amnt = null;
                    if (!string.IsNullOrEmpty(dr["cde_clm_chrge"].ToString()))
                    {
                        cde_clm_chrge = dr["cde_clm_chrge"].ToString().Contains('$') ? dr["cde_clm_chrge"].ToString().Replace('$', ' ').TrimStart() : dr["cde_clm_chrge"].ToString();
                    }
                    else { cde_clm_chrge = " "; }
                    if (!string.IsNullOrEmpty(dr["pad_amnt"].ToString()))
                    {
                        if (DentalStatus.ToUpper() == "PAID" && IsClaimCopy)
                        {
                            pad_amnt = "";
                        }
                        else
                        {
                            pad_amnt = dr["pad_amnt"].ToString().Contains('$') ? dr["pad_amnt"].ToString().Replace('$', ' ').TrimStart() : dr["pad_amnt"].ToString();
                        }
                    }
                    else { pad_amnt = ""; }
                    if (DentalStatus.ToUpper() == "PAID" && IsClaimCopy)
                    {
                        pad_unt = "";
                    }
                    string cde_clm_status = dr["cde_clm_status"].ToString();
                    string Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    string Claim_Id = hdnClaimId.Value;
                    string serviceline = dr["Service_Line"].ToString();
                    if (!string.IsNullOrEmpty(dr["cde_clm_chrge"].ToString()))
                    {
                        totalcharge = totalcharge + Convert.ToDecimal(cde_clm_chrge);
                    }
                    if (!string.IsNullOrEmpty(pad_amnt))
                    {
                        totalAmoutPaid = totalAmoutPaid + Convert.ToDecimal(pad_amnt);
                    }
                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            tab = tab + "<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Line\" class=\"tNumber\">" + sequencedata + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + cde_proc + "</span></td><td><span  title=\"Line\" class=\"tNumber\">" + plc_service + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td> Pending Submission </td><td><input type=\"button\" value = \"Edit\" onClick = \"return EditDentalServiceLineItem('" + Claim_service_Id + "', '" + Claim_Id + "', '" + serviceline + "', this); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td>    <td><input type=\"button\" value = \"Copy\" onClick = \"return EditDentalServiceLineItem('" + Claim_service_Id + "', '" + Claim_Id + "','" + serviceline + "', this, 1); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td>                <td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteDentalServiceDetailLineitem('" + Claim_Id + "','" + serviceline + "');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >";
                            hdnDentalServiecLineClaimStatus.Value = "Pending Submission";
                        }
                        else
                        {
                            tab = tab + "<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Line\" class=\"tNumber\">" + sequencedata + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + cde_proc + "</span></td><td><span  title=\"Line\" class=\"tNumber\">" + plc_service + "</span></td><td><span title=\"Line\" class=\"tNumber\">" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>" + cde_clm_status + "</td></tr >";
                            hdnDentalServiecLineClaimStatus.Value = "Other";
                        }
                    }

                }
                tab = tab + "</tbody></table>";
                btnServiceDetailsDentailAdd.Enabled = true;
                btnUpdateServiceDetailDental.Enabled = true;
                btnCancelDental.Enabled = true;
                providerDentalServicedetailOutput.InnerHtml = tab;
                lblDenTotalCharges.Text = totalcharge.ToString();
                lblDenTotalAmountPaid.Text = totalAmoutPaid.ToString();
            }
        }
    }

    private void CheckDignosisCode()
    {
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            DataSet dsDignosisGrid = new DataSet();
            dsDignosisGrid = LoadDiagnosisPointer();
            if (Helper.HasRows(dsDignosisGrid) &&
                Convert.ToInt32(dsDignosisGrid.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                DataTable dt = dsDignosisGrid.Tables[0];
                if (ddlDiagnosisDentalFirst.SelectedIndex < 0)
                    Helper.LoadList(ddlDiagnosisDentalFirst, dt, "seq_Desc", "diag_seq", true);
                if (ddlDignosisDentalSecond.SelectedIndex <= 0)
                    Helper.LoadList(ddlDignosisDentalSecond, dt, "seq_Desc", "diag_seq", true);
                if (ddlDignosisDentalThird.SelectedIndex <= 0)
                    Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
                if (ddlDignosisDentalFourth.SelectedIndex <= 0)
                    Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);
            }

        }
    }
    protected void btnCloseProc_Click(object sender, EventArgs e)
    {
        ucSubmitClaimSearchProc.CleareField();
    }

    private DataSet LoadDiagnosisPointer()
    {
        DataSet dsDentalDiagnosisPointerData = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnClaimId.Value, true));
        parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, hdnClaimType.Value, true));

        dsDentalDiagnosisPointerData = DataAccess.ExecuteStoredProcedure("usp_SelectClaimsDiagnosisInformation", parameters, "claims_diagnosis_information");
        return dsDentalDiagnosisPointerData;
    }
    protected void ddlDiagnosisDentalFirst_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDropDowns();
        lblpointerError.Text = "";
        dsDentalDiagnosisPointer = LoadDiagnosisPointer();
        if (Helper.HasRows(dsDentalDiagnosisPointer) &&
            Convert.ToInt32(dsDentalDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
        {
            DataTable dt = dsDentalDiagnosisPointer.Tables[0];
            if (ddlDiagnosisDentalFirst.SelectedIndex > 0)
            {
                Helper.LoadList(ddlDignosisDentalSecond, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);
                ddlDignosisDentalSecond.Items.Remove(ddlDignosisDentalSecond.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
            }



            else
            {
                Helper.LoadList(ddlDignosisDentalSecond, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);
            }
        }
    }

    protected void ddlDignosisDentalThird_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDropDowns();
        lblpointerError.Text = "";
        dsDentalDiagnosisPointer = LoadDiagnosisPointer();
        if (Helper.HasRows(dsDentalDiagnosisPointer) &&
           Convert.ToInt32(dsDentalDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
        {
            DataTable dt = dsDentalDiagnosisPointer.Tables[0];
            Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
            Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);

            string ddlDignosisDentalThirdIndex = ViewState["ddlDignosisDentalThirdIndex"].ToString();


            if (ddlDiagnosisDentalFirst.SelectedIndex > 0)
            {
                ddlDignosisDentalSecond.Items.Remove(ddlDignosisDentalSecond.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));

            }
            if (ddlDignosisDentalSecond.SelectedIndex > 0)
            {
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));

            }
            if (int.Parse(ddlDignosisDentalThirdIndex) > 0)
            {
                ddlDignosisDentalThird.SelectedIndex = int.Parse(ddlDignosisDentalThirdIndex);
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalThird.SelectedValue));

            }
            if (ddlDignosisDentalSecond.SelectedIndex > 0)
            {
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));

            }
            else
            {
                if (ddlDignosisDentalFourth.SelectedIndex > 0)
                {
                    ddlDignosisDentalThird.SelectedIndex = 0;
                    ddlDignosisDentalFourth.SelectedIndex = 0;
                }
                lblpointerError.Visible = true;
                lblpointerError.Text = "Please select diagnosis pointer's Third dropdown.";
                ddlDignosisDentalThird.ClearSelection();
                ddlDignosisDentalFourth.ClearSelection();
                ddlDignosisDentalSecond.Focus();
            }

        }

    }

    protected void ddlDignosisDentalSecond_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDropDowns();
        //if (ddlDiagnosisDentalFirst.SelectedIndex > 0)

        //{
        lblpointerError.Text = "";
        dsDentalDiagnosisPointer = LoadDiagnosisPointer();
        if (Helper.HasRows(dsDentalDiagnosisPointer) &&
            Convert.ToInt32(dsDentalDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
        {
            DataTable dt = dsDentalDiagnosisPointer.Tables[0];
            Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
            Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);


            if ((ddlDiagnosisDentalFirst.SelectedIndex > 0))
            {
                ddlDignosisDentalSecond.Items.Remove(ddlDignosisDentalSecond.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));

            }
            if (ddlDignosisDentalSecond.SelectedIndex > 0)
            {
                ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));
                ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));

            }

            else
            {

                lblpointerError.Visible = true;
                lblpointerError.Text = "Please select diagnosis pointer's Second dropdown.";
                Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);
                ddlDignosisDentalSecond.ClearSelection();
                ddlDiagnosisDentalFirst.Focus();
            }



        }
    }
    protected void ddlDignosisDentalFourth_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDropDowns();
        if (ddlDiagnosisDentalFirst.SelectedIndex > 0)
        {
            ddlDignosisDentalSecond.Items.Remove(ddlDignosisDentalSecond.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
            ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));
            ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDiagnosisDentalFirst.SelectedValue));

        }
        if (ddlDignosisDentalSecond.SelectedIndex > 0)
        {
            ddlDignosisDentalThird.Items.Remove(ddlDignosisDentalThird.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));
            ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalSecond.SelectedValue));

        }
        if (ddlDignosisDentalThird.SelectedIndex > 0)
        {
            ddlDignosisDentalFourth.Items.Remove(ddlDignosisDentalFourth.Items.FindByValue(ddlDignosisDentalThird.SelectedValue));
        }
        lblpointerError.Text = "";
        if (ddlDignosisDentalThird.SelectedIndex <= 0)
        {
            lblpointerError.Visible = true;
            lblpointerError.Text = "Please select diagnosis pointer's Third dropdown.";
            ddlDignosisDentalFourth.ClearSelection();
            ddlDignosisDentalSecond.Focus();
        }
    }

    protected void txtplaceofserviceDental_TextChanged(object sender, EventArgs e)
    {
        lblPlaceOfServiceErr.Text = "";
        if (!string.IsNullOrEmpty(txtplaceofserviceDental.Text) && !string.IsNullOrEmpty(hdnClaimId.Value))
        {
            DataSet dt = LookupTableController.GetPlaceOfServiceDinfo(Convert.ToInt32(hdnClaimId.Value.ToString()), Convert.ToInt32(txtplaceofserviceDental.Text));


            if (Helper.HasRows(dt))
            {
                if (dt.Tables[0].Rows.Count > 0)
                {
                    lblPlaceOfServiceErr.Text = "Place of service already enter Service Information Header in previous service line .";
                }
                else
                { lblPlaceOfServiceErr.Text = ""; }

            }


        }
    }
    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator
        {
            IsValid = false,
            ForeColor = System.Drawing.Color.FromName("red"),
            ErrorMessage = msg,
            SetFocusOnError = true,
            Text = msg,
            ValidationGroup = "validateDentalServiceCus"
        };
        this.Page.Validators.Add(val);
        return false;
    }

    private bool ValidateDentalData()
    {
        bool isValid = true;
        string plcService = ServiceInfoPlaceofService;
        string PriorAuthorizationNumberData = PriorAuthorizationNumber;
        string ReferralNumberData = ReferralNumber;
        if (string.IsNullOrEmpty(txtProducerCodeDental.Text) || (!string.IsNullOrEmpty(txtProducerCodeDental.Text) && (txtProducerCodeDental.Text.ToString().StartsWith("MR", StringComparison.InvariantCultureIgnoreCase) || txtProducerCodeDental.Text.ToString().StartsWith("DD", StringComparison.InvariantCultureIgnoreCase) || txtProducerCodeDental.Text.ToString().StartsWith("PNM", StringComparison.InvariantCultureIgnoreCase))))
        {
            AddValidationErrorMessage("*Procedure code is invalid");
            txtProducerCodeDental.Text = string.Empty;
            isValid = false;
        }
        if (string.IsNullOrEmpty(txtdateofserviceDental.Text))
        {
            AddValidationErrorMessage("*Date of service is required");
            isValid = false;
        }

        if ((!string.IsNullOrEmpty(txtModifierDentalFirst.Text) && (txtModifierDentalFirst.Text.Length < 2 || txtModifierDentalFirst.Text.Length > 2))
            || (!string.IsNullOrEmpty(txtModifierDentalSecond.Text) && (txtModifierDentalSecond.Text.Length < 2 || txtModifierDentalSecond.Text.Length > 2))
            || (!string.IsNullOrEmpty(txtModifierDentalThird.Text) && (txtModifierDentalThird.Text.Length < 2 || txtModifierDentalFourth.Text.Length > 2))
            || (!string.IsNullOrEmpty(txtModifierDentalFourth.Text) && (txtModifierDentalFourth.Text.Length < 2 || txtModifierDentalFourth.Text.Length > 2)))
        {
            AddValidationErrorMessage("*Procedure code modifier must be 2 characters.");
            isValid = false;
        }
        //if (!string.IsNullOrEmpty(txtModifierDentalSecond.Text) && (txtModifierDentalSecond.Text.Length < 2 || txtModifierDentalSecond.Text.Length > 2))
        //{
        //    AddValidationErrorMessage("**Procedure code modifier must be 2 characters.");
        //    isValid = false;
        //}
        //if (!string.IsNullOrEmpty(txtModifierDentalThird.Text) && (txtModifierDentalThird.Text.Length < 2 || txtModifierDentalFourth.Text.Length > 2))
        //{
        //    AddValidationErrorMessage("**Procedure code modifier must be 2 characters.");
        //    isValid = false;
        //}
        //if (!string.IsNullOrEmpty(txtModifierDentalFourth.Text) && (txtModifierDentalFourth.Text.Length < 2 || txtModifierDentalFourth.Text.Length > 2))
        //{
        //    AddValidationErrorMessage("**Procedure code modifier must be 2 characters");
        //    isValid = false;
        //}

        if (!string.IsNullOrEmpty(txtBillUnitDental.Text) && (Convert.ToDecimal(txtBillUnitDental.Text) < 0 || Convert.ToDecimal(txtBillUnitDental.Text) == 0))
        {
            AddValidationErrorMessage("*Billed units should be greater than zero");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtplaceofserviceDental.Text))
        {
            bool revCodeExists = ClaimsController.IsValidPlaceOfservice(txtplaceofserviceDental.Text.Trim());

            if (!revCodeExists)
            {
                AddValidationErrorMessage("*Place of service is invalid.");
                isValid = false;
            }
        }
        if (!string.IsNullOrEmpty(txtProducerCodeDental.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureCode(txtProducerCodeDental.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure code is invalid.");
                isValid = false;
            }
        }
        if (!string.IsNullOrEmpty(txtModifierDentalFirst.Text))
        {

            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifierDentalFirst.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier1 is invalid.");
                txtModifierDentalFirst.Text = string.Empty;
                isValid = false;
            }
        }
        if (!string.IsNullOrEmpty(txtModifierDentalSecond.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifierDentalSecond.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier2 is invalid.");
                txtModifierDentalSecond.Text = string.Empty;
                isValid = false;
            }
        }
        if (!string.IsNullOrEmpty(txtModifierDentalThird.Text))
        {

            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifierDentalThird.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier3 is invalid.");
                txtModifierDentalThird.Text = string.Empty;
                isValid = false;
            }
        }
        if (!string.IsNullOrEmpty(txtModifierDentalFourth.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifierDentalFourth.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier4 is invalid.");
                txtModifierDentalFourth.Text = string.Empty;
                isValid = false;
            }
        }
        if (txtReferralDental.Enabled && (ReferralNumberData == txtReferralDental.Text))
        {
            AddValidationErrorMessage("*Referral number in the service detail should only be entered if it is different than the header");
            isValid = false;
        }
        if (txtPriorAuthNumberDental.Enabled && (PriorAuthorizationNumberData == txtPriorAuthNumberDental.Text))
        {
            AddValidationErrorMessage("*PA number in the service detail should only be entered if it is different than the header ");
            isValid = false;
        }
        //if(   !string.IsNullOrEmpty (txtplaceofserviceDental.Text)&&ServiceInfoPlaceofService == txtplaceofserviceDental.Text)
        //{
        //    AddValidationErrorMessage("*Place of service in service detail should only be entered if different than claim level ");
        //    isValid = false;
        //}

        return isValid;
    }
    public void RefreshDropDown()
    {
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            DataSet dsDignosisGrid = new DataSet();
            dsDignosisGrid = LoadDiagnosisPointer();
            if (Helper.HasRows(dsDignosisGrid) &&
                Convert.ToInt32(dsDignosisGrid.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value))
            {
                DataTable dt = dsDignosisGrid.Tables[0];
                Helper.LoadList(ddlDiagnosisDentalFirst, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalSecond, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalThird, dt, "seq_Desc", "diag_seq", true);
                Helper.LoadList(ddlDignosisDentalFourth, dt, "seq_Desc", "diag_seq", true);
            }
            else
            {
                ddlDiagnosisDentalFirst.Items.Clear();
                ddlDignosisDentalSecond.Items.Clear();
                ddlDignosisDentalThird.Items.Clear();
                ddlDignosisDentalFourth.Items.Clear();
            }

        }
    }
    public void SaveToDbOnAdjust(DataTable dt)
    {
        try
        {
            if (DentalStatus.ToUpper() == "PAID" && IsClaimCopy)
            {
                hdnHidePaidAmounts.Value = "1";
            }
            if (Helper.HasRows(dt))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    string lblserviceline = "";
                    int serviceline = Convert.ToInt32(dt.Rows[i]["Service_Line"].ToString());
                    lblserviceline = serviceline.ToString();

                    parms.Add("Service_Line", lblserviceline);
                    parms.Add("Procedure_Code", dt.Rows[i]["cde_proc"].ToString().ToUpper());
                    if (dt.Rows[i]["ServiceDate"].ToString() != null)
                    {
                        if (checkDateFormat(dt.Rows[i]["ServiceDate"].ToString()))
                        {
                            parms.Add("Date_of_Service", dt.Rows[i]["ServiceDate"].ToString());
                        }
                        else
                        {
                            parms.Add("Date_of_Service", DateTime.ParseExact(dt.Rows[i]["ServiceDate"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy"));
                        }
                    }
                    parms.Add("Line_Control_Number", dt.Rows[i]["line_ctr_num"].ToString());
                    parms.Add("Prior_Authorization_Number", dt.Rows[i]["prior_auth"].ToString());
                    parms.Add("Referral_Number", dt.Rows[i]["ref_num"].ToString());
                    parms.Add("Place_of_Service", dt.Rows[i]["plc_service"].ToString());
                    parms.Add("Modifier1", dt.Rows[i]["mdf_first"].ToString().ToUpper());
                    parms.Add("Modifier2", dt.Rows[i]["mdf_secnd"].ToString().ToUpper());
                    parms.Add("Modifier3", dt.Rows[i]["mdf_thrd"].ToString().ToUpper());
                    parms.Add("Modifier4", dt.Rows[i]["mdf_forth"].ToString().ToUpper());
                    parms.Add("Diagnosis_Pointer1", dt.Rows[i]["digno_first"].ToString());
                    parms.Add("Diagnosis_Pointer2", dt.Rows[i]["digno_sec"].ToString());
                    parms.Add("Diagnosis_Pointer3", dt.Rows[i]["digno_third"].ToString());
                    parms.Add("Diagnosis_Pointer4", dt.Rows[i]["digno_forth"].ToString());

                    parms.Add("Oral_Cavity1", dt.Rows[i]["orl_cvt_first"].ToString());
                    parms.Add("Oral_Cavity2", dt.Rows[i]["orl_cvt_sec"].ToString());
                    parms.Add("Oral_Cavity3", dt.Rows[i]["orl_cvt_third"].ToString());
                    parms.Add("Oral_Cavity4", dt.Rows[i]["orl_cvt_forth"].ToString());
                    parms.Add("Oral_Cavity5", dt.Rows[i]["orl_cvt_fifth"].ToString());
                    parms.Add("Prosthesis_Crown_InlayCode", dt.Rows[i]["prosthesis_cd"].ToString());
                    parms.Add("Status", dt.Rows[i]["cde_clm_status"].ToString());
                    parms.Add("Charges", dt.Rows[i]["cde_clm_chrge"].ToString());
                    if (!string.IsNullOrEmpty(dt.Rows[i]["pad_amnt"].ToString()) && hdnHidePaidAmounts.Value != "1")
                    {
                        string paidamount = dt.Rows[i]["pad_amnt"].ToString().Contains('$') ? dt.Rows[i]["pad_amnt"].ToString().Replace('$', ' ').TrimStart() : dt.Rows[i]["pad_amnt"].ToString();
                        parms.Add("Paid_Amount", paidamount);
                    }
                    else
                    {
                        parms.Add("Paid_Amount", null);

                    }
                    //parms.Add("Paid_Amount", dt.Rows[i]["pad_amnt"].ToString());
                    parms.Add("Billed_Units", dt.Rows[i]["bil_unt"].ToString());
                    parms.Add("Paid_Units", !string.IsNullOrEmpty(dt.Rows[i]["pad_unt"].ToString()) && hdnHidePaidAmounts.Value != "1" ? dt.Rows[i]["pad_unt"].ToString() : null);
                    parms.Add("Total_Charges", dt.Rows[i]["amt_billed"].ToString());
                    parms.Add("Last_modified_date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Created_date_time", DateTime.Now.ToString());
                    parms.Add("Created_by_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Claim_ID", hdnClaimId.Value);
                    parms.Add("Claim_Type", "0");
                    parms.Add("Clear_Prior_Data", "1");
                    ClaimsController.InsertPanelsData("Claims_Service_Details", parms);
                }
                //List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
                //SetDentalServiceDetailPanelData(listData, null);
                ClearDentalControl();
                //int rowCount =gvServiceDetailDental.Rows.Count;
                if (Helper.HasRows(dsDentalclaimGrid))
                {
                    if (dsDentalclaimGrid.Tables[0].Rows.Count == 50)
                    {
                        btnServiceDetailsDentailAdd.Visible = false;
                    }
                    else
                    {
                        btnServiceDetailsDentailAdd.Visible = true;
                        btnUpdateServiceDetailDental.Visible = false;
                        btnCancelDental.Visible = false;
                    }

                }
            }
            // List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ClearDentalControl();
            SetServiceDetails(null);
            //int rowCount =gvServiceDetailDental.Rows.Count;
            if (Helper.HasRows(dsDentalclaimGrid))
            {
                if (dsDentalclaimGrid.Tables[0].Rows.Count == 50)
                {
                    btnServiceDetailsDentailAdd.Visible = false;
                }
                else
                {
                    btnServiceDetailsDentailAdd.Visible = true;
                    btnUpdateServiceDetailDental.Visible = false;
                    btnCancelDental.Visible = false;
                }

            }
        }
        catch { }
    }

    public void EnableDropDowns()
    {
        if (ddlDiagnosisDentalFirst.SelectedIndex <= 0)
        {

            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.SelectedIndex = -1;
            ddlDignosisDentalSecond.Enabled = false;
            ddlDignosisDentalThird.SelectedIndex = -1;
            ddlDignosisDentalThird.Enabled = false;
            ddlDignosisDentalFourth.SelectedIndex = -1;
            ddlDignosisDentalFourth.Enabled = false;
            return;
        }
        else
        {
            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.Enabled = true;
            ddlDignosisDentalThird.Enabled = true;
            ddlDignosisDentalFourth.Enabled = true;
        }
        if (ddlDignosisDentalSecond.SelectedIndex <= 0)
        {
            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.SelectedIndex = -1;
            ddlDignosisDentalSecond.Enabled = true;
            ddlDignosisDentalThird.SelectedIndex = -1;
            ddlDignosisDentalThird.Enabled = false;
            ddlDignosisDentalFourth.SelectedIndex = -1;
            ddlDignosisDentalFourth.Enabled = false;
            return;
        }
        else
        {
            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.Enabled = true;
            ddlDignosisDentalThird.Enabled = true;
            ddlDignosisDentalFourth.Enabled = true;

        }
        if (ddlDignosisDentalThird.SelectedIndex <= 0)
        {
            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.Enabled = true;
            ddlDignosisDentalThird.Enabled = true;
            ddlDignosisDentalFourth.SelectedIndex = -1;
            ddlDignosisDentalFourth.Enabled = false;
            return;
        }
        else
        {
            ddlDiagnosisDentalFirst.Enabled = true;
            ddlDignosisDentalSecond.Enabled = true;
            ddlDignosisDentalThird.Enabled = true;
            ddlDignosisDentalFourth.Enabled = true;
        }

    }

    private bool checkDateFormat(string inputString)
    {
        string[] formats = { "MM/dd/yyyy" };
        DateTime parsedDate;
        var isValidFormat = DateTime.TryParseExact(inputString, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);

        if (isValidFormat)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}