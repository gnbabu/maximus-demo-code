using Amazon.Runtime.Internal.Transform;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class ProfessionalServiceLineDetails : BasePopupControl
{
    public delegate void EventHandler();
    public event EventHandler RefreshChildGrids;

    public event EventHandler RefreshOtherPayerPaidAmountDropdown;
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private int claim_Id;
    DataSet dsProfDiagnosisPointer;
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
    private bool _displayReadOnly;
    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
            SetButtonVisibility();
        }
        set
        {
            _displayReadOnly = value; 
            SetButtonVisibility();
        }

    }
    private string profStatus = string.Empty;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public string providerTypeId = string.Empty;
    public string ProfessionalStatus
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(profStatus))
                return profStatus;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                profStatus = value;
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
    public void HideDivsForReSubmitCopyandAdjust()
    {
        divPaidAmount.Visible = false;
        divPaymentAction.Visible = false;
    }
    private DataSet dsServiceDetailsProfData = new DataSet();
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divProf.Visible = false;
            
        }
        else
        {
            divProf.Visible = true;
        }
    }
    private DataSet FetchOtherServiceDetailsInformation()
    {
        DataSet dsServiceDetailsInfo = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnValueCode_ClaimID.Value, true));
        parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, "2", true));
        dsServiceDetailsProfData = dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_ServiceLine_Prof", parameters, "Claims_Service_Details");
        return dsServiceDetailsInfo;
    }
    public DataSet dataSetServiceDetails
    {
        get
        {
            if (!Helper.HasRows(dsServiceDetailsProfData))
            {
                if (Helper.HasRows(FetchOtherServiceDetailsInformation()))
                {
                    return dsServiceDetailsProfData;
                }
            }
            
            
            return dsServiceDetailsProfData;

        }

        set
        {
            if (value != null)
            {
                //List<DentalServiceDetail> listData = new List<DentalServiceDetail>();
                //SetDentalServiceDetailPanelData(listData, value);
                SetServiceDetails(value);
            }
        }

    }
    private void SetServiceDetails(DataSet dsServiceDetails)
    {
        //gvProfServiceLineDetails.DataSource = dsServiceDetails;
        //gvProfServiceLineDetails.DataBind();
        if (profStatus.ToUpper() == "PAID" && IsClaimCopy)
        {
            hdnHidePaidAmounts.Value = "1";
        }
        hdnProfServiceDetailClaimStatus.Value = "";
        DataSet dsServiceDetailsDental = new DataSet();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
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
            upProfServiceLineDetails1.InnerHtml = "";
            if (diagTable.Rows.Count > 0)
            {
                string tab = string.Empty;
                int sequence = 0;
                decimal totalcharge = 0;
                decimal totalAmoutPaid = 0;             
                tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Procedure Code</th><th style='width:10px; scope='col'>Place Of Service</th><th style='width:10px; scope='col'>*Billed Units</th><th style='width:10px; scope='col'>Paid Units</th><th style='width:10px; scope='col'>Date Of Service</th><th style='width:10px; scope='col'>Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                foreach (DataRow dr in diagTable.Rows)
                {
                    sequence++;
                    string sequencedata = null;
                    
                    sequencedata = sequence.ToString();
                    string cde_proc = dr["Procedure_code"].ToString();
                    string plc_service = dr["Place_of_Service"].ToString();
                    string bil_unt = dr["Billed_Units"].ToString();
                    string pad_unt = dr["Paid_Units"].ToString();
                    string ServiceDate = dr["Date_of_Service"].ToString();
                    string cde_clm_chrge = null;
                    string pad_amnt = null;
                    if (!string.IsNullOrEmpty(dr["Charges"].ToString()))
                    {
                        cde_clm_chrge = dr["Charges"].ToString().Contains('$') ? dr["Charges"].ToString().Replace('$', ' ').TrimStart() : dr["Charges"].ToString();
                    }
                    else { cde_clm_chrge = " "; }
                    if (!string.IsNullOrEmpty(dr["Paid_Amount"].ToString()))
                    {
                        if (profStatus.ToUpper() == "PAID" && IsClaimCopy)
                        {
                            pad_amnt = "";
                        }
                        else
                        {
                            pad_amnt = dr["Paid_Amount"].ToString().Contains('$') ? dr["Paid_Amount"].ToString().Replace('$', ' ').TrimStart() : dr["Paid_Amount"].ToString();
                        }
                    }
                    else { pad_amnt = ""; }
                    if (profStatus.ToUpper() == "PAID" && IsClaimCopy)
                    {
                        pad_unt = "";
                    }
                    string cde_clm_status = dr["Status"].ToString();
                    if (!string.IsNullOrEmpty(Session["ClaimStatus"].ToString()))
                    {
                        cde_clm_status = Session["ClaimStatus"].ToString();
                    }
                    string Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                    string Claim_Id = hdnValueCode_ClaimID.Value;
                    string serviceline = dr["Service_Line"].ToString();
                    if (!string.IsNullOrEmpty(dr["Charges"].ToString()))
                    {
                        totalcharge = totalcharge + Convert.ToDecimal(cde_clm_chrge);
                    }
                    if (!string.IsNullOrEmpty(pad_amnt))
                    {
                        totalAmoutPaid = totalAmoutPaid + Convert.ToDecimal(pad_amnt);
                    }
                    if (cde_clm_status == "Pending Submission")
                    {
                        tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequencedata + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td><span  title='Line' class='tNumber'>" + plc_service + "</span></td><td><span title='Line' class='tNumber'>" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>Pending Submission</td><td><input type='button' value = 'Edit' onClick = 'return EditProfessionalServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + serviceline + "\", this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value = 'Copy' onClick = 'return EditProfessionalServiceLineItem(\"" + Claim_service_Id + "\",\"" + Claim_Id + "\",\"" + serviceline + "\", this, 1); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteProfessionalServiceDetailLineitem(\"" + Claim_Id + "\",\"" + serviceline + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr>";
                        hdnProfServiceDetailClaimStatus.Value = "Pending Submission";
                    }
                    else
                    {
                        tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequencedata + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td><span  title='Line' class='tNumber'>" + plc_service + "</span></td><td><span title='Line' class='tNumber'>" + bil_unt + "</span></td><td>" + pad_unt + "</td><td>" + ServiceDate + "</td><td>" + cde_clm_chrge + "</td><td>" + pad_amnt + "</td><td>" + cde_clm_status + "</td></tr>";
                        hdnProfServiceDetailClaimStatus.Value = "Other";
                    }



                }
                tab = tab + "</tbody></table>";
                upProfServiceLineDetails1.InnerHtml = tab;
                ProfInfoAdd.Enabled = true;
                btnUpdateServiceDetailProfessional.Enabled = true;
                btncancelProfessional.Enabled = true;
                lblProfTotalCharges.Text = totalcharge.ToString();
                lblProfTotalAmountPaid.Text = totalAmoutPaid.ToString();
            }
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //string txtToday = DateTime.Now.ToShortDateString();
        //cvdateofservice.ValueToCompare = txtToday;
        if (!string.IsNullOrEmpty(ucSubmitClaimSearchProc.ProcedureCode))
        {
            txtServiceProcedureCode.Text = ucSubmitClaimSearchProc.ProcedureCode;
        }
        if (Session["PlaceofServiceCode"] != null)
        {
            txtProfPlaceOfService.Text = Session["PlaceofServiceCode"].ToString();
        }
        if (ddlDiagnosisPointer1.SelectedIndex <= 0)
        {
            //CheckDignosisCode();
        }

    }

    public DataTable gvServiceLineDetails
    {
        get
        {
            return (DataTable)ViewState["CurrentTable"];
        }
        set
        {
            if (value != null)
            {
                ViewState["CurrentTable"] = value;
                DataSet ds = new DataSet();
                ds.Tables.Add(value);
                SetServiceDetails(ds);
            }
        }
    }

    public string TotalCharges
    {
        get
        {
            if (!string.IsNullOrEmpty(lblProfTotalCharges.Text))
                return lblProfTotalCharges.Text;
            else
                return string.Empty;
        }
        set
        {
            lblProfTotalCharges.Text = value;
        }
    }

    public string TotalAmountPaid
    {
        get
        {
            if (!string.IsNullOrEmpty(lblProfTotalAmountPaid.Text))
                return lblProfTotalAmountPaid.Text;
            else
                return string.Empty;
        }
        set
        {
            lblProfTotalAmountPaid.Text = value;
        }
    }
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCode_ClaimID.Value))
                return hdnValueCode_ClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCode_ClaimID.Value = value.Trim();
        }
    }

    public string PlaceofServiceProf

    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtProfPlaceOfService.Text))
                return txtProfPlaceOfService.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtProfPlaceOfService.Text = value.Trim();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        hdnProviderTypeId.Value = this.WorkflowPage.ProviderTypeID.ToString();
        if (ddlUnitsOfMeasurement.Items.Count == 0 || ddlDMECertType.Items.Count == 0)
        {

            DataSet dsUnitsOfMeasurement = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Is_Professional", DbType.Boolean, 1, true));

            dsUnitsOfMeasurement = DataAccess.ExecuteStoredProcedure("usp_Select_UnitsOfMeasurement", parameters, "Units_Of_Measurement");

            ddlUnitsOfMeasurement.DataTextField = dsUnitsOfMeasurement.Tables[0].Columns["CLAIM_UNITSOFMEASUREMENT_CODE"].ToString();
            ddlUnitsOfMeasurement.DataValueField = dsUnitsOfMeasurement.Tables[0].Columns["CLAIM_UNITSOFMEASUREMENT_CODE"].ToString();

            ddlUnitsOfMeasurement.DataSource = dsUnitsOfMeasurement.Tables[0];
            ddlUnitsOfMeasurement.DataBind();

            DataSet dsDME_Cert_Type = new DataSet();
            dsDME_Cert_Type = DataAccess.ExecuteStoredProcedure("usp_Select_DME_Cert_Type", null, "DME_Cert_Type");

            ddlDMECertType.DataTextField = dsDME_Cert_Type.Tables[0].Columns["TITLE"].ToString();
            ddlDMECertType.DataValueField = dsDME_Cert_Type.Tables[0].Columns["CLAIM_DME_CERT_TYPE_CODE"].ToString();

            if (dsDME_Cert_Type != null)
            {
                ddlDMECertType.DataSource = dsDME_Cert_Type.Tables[0];
                ddlDMECertType.DataBind();
            }
            ddlDMECertType.Items.Insert(0, new ListItem("", ""));
            ddlUnitsOfMeasurement.Items.Insert(0, new ListItem("", ""));
        }
        
        if (!Page.IsPostBack)
        {
            BindData();
        }
        else
        {
            if (ddlDiagnosisPointer1.SelectedIndex <= 0)
            {
                CheckDignosisCode();
            }
        }
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            //DataSet dtProfessionalDataset = new DataSet();
            SetServiceDetails(dsServiceDetailsProfData);
        }
        SetProfessionalServiceDetailPanelData();
        //if (DisplayReadOnly == true)
        //{
        //    divProf.Visible = false;
        //}
        //else { divProf.Visible = true; }
        lblProfStatus.Text = ProfessionalStatus;

    }

    public bool CheckIfNumber(string inputVal)
    {
        decimal numericValue;
        bool isNumber = decimal.TryParse(inputVal, out numericValue);
        return isNumber;
    }


    public void BindData()
    {
        DataSet serviceLineDetails = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        DataTable claimsDataTable = new DataTable();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
        claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        if (claimsDataTable != null && claimsDataTable.Rows.Count > 999)
        {
            AddValidationErrorMessage("Max 999 Service Lines are allowed");
        }
        else
        {
            DataRow drCurrentRow = claimsDataTable.NewRow();
            int latestServiceLine = claimsDataTable.Rows.Count > 0 ? Convert.ToInt32((claimsDataTable).Rows[(claimsDataTable).Rows.Count - 1]["Service_Line"]) : 0;

            lblDetailsItemProf.Text = "0" + (latestServiceLine + 1);
        }
        if (serviceLineDetails != null && serviceLineDetails.Tables.Count > 0)
            claimsDataTable = serviceLineDetails.Tables[0];


        if (claimsDataTable != null)
        {
            if (claimsDataTable.Rows.Count > 0)
            {
                foreach (DataRow dr in claimsDataTable.Rows)
                {
                    if (!string.IsNullOrEmpty(dr["Place_of_Service"].ToString()) && dr["Place_of_Service"].ToString().Length == 1)
                    {
                        dr["Place_of_Service"] = "0" + dr["Place_of_Service"].ToString();
                    }
                }
            }
        }

        if (claimsDataTable != null)
        {
            int latestServiceLine = claimsDataTable.Rows.Count > 0 ? Convert.ToInt32((claimsDataTable).Rows[(claimsDataTable).Rows.Count - 1]["Service_Line"]) : 0;
            lblDetailsItemProf.Text = "0" + (latestServiceLine + 1);
            ViewState["CurrentTable"] = claimsDataTable;
            SetServiceDetails(serviceLineDetails);
            //gvProfServiceLineDetails.DataSource = claimsDataTable;
            //gvProfServiceLineDetails.DataBind();
        }
        SetTotalAmountBilled(claimsDataTable);
    }

    private void SetServiceDetails(DataTable dtServiceDetails)
    {
        //gvProfServiceLineDetails.DataSource = dtServiceDetails;
        //gvProfServiceLineDetails.DataBind();
    }

    protected void gvProfServiceLineDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gvProfServiceLineDetails.PageIndex = e.NewPageIndex;
        BindData();
    }
    public bool InValidDateError(TextBox FromandTodate, Label FromandToErrorlbl, Label FromandToRequiredlbl)
    {
        //lblErrorDOS.Text = string.Empty;
        lblErrorCertificationRev.Text = string.Empty;
        DateTime dt = default(DateTime);
        bool valid = DateTime.TryParseExact(FromandTodate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
        if (!valid)
        {
            FromandToRequiredlbl.Visible = false;
            FromandToErrorlbl.Text = "Enter Valid Date or Format (MM/DD/YYYY)";
            FromandTodate.Text = string.Empty;
            return true;
        }
        else
        {
            FromandToErrorlbl.Text = string.Empty;
            return false;
        }
    }
    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtCertRev.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate11();</script>", false);
        }
        else
        {
            ProffServiceLineDetailsDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtCertRev.Text))
        {
            if (InValidDateError(txtCertRev, lblErrorCertificationRev, ProffServiceLineDetailsDateRequiredError1))
            {
                return;
            }
        }
    }

    protected void txtCheckForproffDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtProffdateofservice.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate12();</script>", false);

        }
        else
        {
            ProfServiceLineDetailsDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtProffdateofservice.Text))
        {
            //if (InValidDateError(txtProffdateofservice, lblErrorDOS, ProfServiceLineDetailsDateRequiredError1))
            //{
            //    return;
            //}
        }
    }

    protected void gvProfServiceLineDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        Dictionary<string, string> parms = new Dictionary<string, string>
            {
                //{ "Service_Line", gvProfServiceLineDetails.Rows[e.RowIndex].Cells[0].Text.ToString() },
                { "Claim_ID", Convert.ToString(ClaimId) }
            };

        svc.DeletePanelsDataWithParams("Claims_Service_Details", parms);

        BindData();

        ClearProfPanelData();

    }

    protected void gvProfServiceLineDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //gvProfServiceLineDetails.EditIndex = -1;
        BindData();
        ProfInfoAdd.Enabled = true;
        ClearProfPanelData();
    }

    protected void gvProfServiceLineDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //gvProfServiceLineDetails.EditIndex = e.NewEditIndex;

        BindData();

        int id = 0;
        //if (gvProfServiceLineDetails != null && gvProfServiceLineDetails.Rows.Count > 0)
        //    id = Convert.ToInt32(gvProfServiceLineDetails.Rows[e.NewEditIndex].Cells[0].Text);

        DataSet serviceLineDetails = new DataSet();
        DataTable claimsDataTable = new DataTable();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, id, true));
        serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");

        if (serviceLineDetails != null)
            claimsDataTable = serviceLineDetails.Tables[0];

        if (claimsDataTable != null && claimsDataTable.Rows.Count > 0)
        {
            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Procedure_Code"].ToString()))
                txtServiceProcedureCode.Text = claimsDataTable.Rows[0]["Procedure_Code"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Place_of_Service"].ToString()))
                txtProfPlaceOfService.Text = claimsDataTable.Rows[0]["Place_of_Service"].ToString().Trim().Length == 1 ? "0" + claimsDataTable.Rows[0]["Place_of_Service"].ToString() : claimsDataTable.Rows[0]["Place_of_Service"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Status"].ToString()))
                lblProfStatus.Text = claimsDataTable.Rows[0]["Status"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Date_of_Service"].ToString()))
                txtProffdateofservice.Text = Convert.ToDateTime(claimsDataTable.Rows[0]["Date_of_Service"]).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier1"].ToString()) && claimsDataTable.Rows[0]["Modifier1"].ToString() != "0")
                txtModifier1.Text = claimsDataTable.Rows[0]["Modifier1"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier2"].ToString()) && claimsDataTable.Rows[0]["Modifier2"].ToString() != "0")
                txtModifier2.Text = claimsDataTable.Rows[0]["Modifier2"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier3"].ToString()) && claimsDataTable.Rows[0]["Modifier3"].ToString() != "0")
                txtModifier3.Text = claimsDataTable.Rows[0]["Modifier3"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier4"].ToString()) && claimsDataTable.Rows[0]["Modifier4"].ToString() != "0")
                txtModifier4.Text = claimsDataTable.Rows[0]["Modifier4"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Charges"].ToString()))
                txtCharges.Text = claimsDataTable.Rows[0]["Charges"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Line_Control_Number"].ToString()))
                txtLineCntrlNumber.Text = claimsDataTable.Rows[0]["Line_Control_Number"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Diagnosis_Pointer1"].ToString()))
                ddlDiagnosisPointer1.SelectedValue = claimsDataTable.Rows[0]["Diagnosis_Pointer1"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Diagnosis_Pointer2"].ToString()))
                ddlDiagnosisPointer2.SelectedValue = claimsDataTable.Rows[0]["Diagnosis_Pointer2"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Diagnosis_Pointer3"].ToString()))
                ddlDiagnosisPointer3.SelectedValue = claimsDataTable.Rows[0]["Diagnosis_Pointer3"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Diagnosis_Pointer4"].ToString()))
                ddlDiagnosisPointer4.SelectedValue = claimsDataTable.Rows[0]["Diagnosis_Pointer4"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Billed_Units"].ToString()))
                txtBilledUnits.Text = claimsDataTable.Rows[0]["Billed_Units"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Prior_Authorization_Number"].ToString()))
                txtPriorAuthNumber.Text = claimsDataTable.Rows[0]["Prior_Authorization_Number"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Referred_EPSDT_Service"].ToString()))
                ddlEpsdtService.SelectedValue = claimsDataTable.Rows[0]["Referred_EPSDT_Service"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Unit_Of_Measurement"].ToString()))
                ddlUnitsOfMeasurement.SelectedValue = claimsDataTable.Rows[0]["Unit_Of_Measurement"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Referral_Number"].ToString()))
                txtReferralNumber.Text = claimsDataTable.Rows[0]["Referral_Number"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Family_Planning"].ToString()))
                ddlFamilyPlanning.SelectedValue = claimsDataTable.Rows[0]["Family_Planning"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Paid_Units"].ToString()))
                lblProfPaidUnits.Text = claimsDataTable.Rows[0]["Paid_Units"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["DME_Cert_Type"].ToString()))
                ddlDMECertType.SelectedValue = claimsDataTable.Rows[0]["DME_Cert_Type"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Emergency"].ToString()))
                ddlEmergency.SelectedValue = claimsDataTable.Rows[0]["Emergency"].ToString().Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Paid_Amount"].ToString()))
                lblProfPaidAmount.Text = claimsDataTable.Rows[0]["Paid_Amount"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["DME_Duration"].ToString()))
                txtDuration.Text = claimsDataTable.Rows[0]["DME_Duration"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Final_EAPG"].ToString()))
                lblProfFinalEAPG.Text = claimsDataTable.Rows[0]["Final_EAPG"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Cert_Revision"].ToString()) && Convert.ToDateTime(claimsDataTable.Rows[0]["Cert_Revision"]).ToString("MM/dd/yyyy") != DateTime.MinValue.ToString("MM/dd/yyyy"))
                txtCertRev.Text = Convert.ToDateTime(claimsDataTable.Rows[0]["Cert_Revision"]).ToString("MM/dd/yyyy").Trim();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Payment_Action"].ToString()))
                lblProfPaymentAction.Text = claimsDataTable.Rows[0]["Payment_Action"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Total_Charges"].ToString()))
                lblProfTotalCharges.Text = claimsDataTable.Rows[0]["Total_Charges"].ToString();

            if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Total_Amount_Paid"].ToString()))
                lblProfTotalAmountPaid.Text = claimsDataTable.Rows[0]["Total_Amount_Paid"].ToString();
        }

        ProfInfoAdd.Enabled = false;
        //lblDetailsItemProf.Text = gvProfServiceLineDetails.Rows[e.NewEditIndex].Cells[0].Text;

    }

    protected void gvProfServiceLineDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
        {
            // e.Row.Cells[5].Visible = !DisplayReadOnly;
            if (DisplayReadOnly == true)
            {
                // divProf.Visible= !DisplayReadOnly;
            }
        }
        else if (e.Row.RowType == DataControlRowType.Header && !DisplayReadOnly)
        {
            //e.Row.Cells[5].Visible = true;
            //  divProf.Visible = true;
        }

        if (e.Row.RowType == DataControlRowType.DataRow && DisplayReadOnly)
        {
            //gvProfServiceLineDetails.Columns[10].Visible = !DisplayReadOnly;
            //gvProfServiceLineDetails.Columns[11].Visible = !DisplayReadOnly;
            if (DisplayReadOnly == true)
            {
                //  divProf.Visible = !DisplayReadOnly;
            }
            //if (Convert.ToInt32(e.Row.Cells[0].Text) <= 10)
            //{
            //    e.Row.Cells[0].Text = "0" + e.Row.Cells[0].Text;
            //}
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Convert.ToInt32(e.Row.Cells[0].Text) <= 10)
            {
                e.Row.Cells[0].Text = "0" + e.Row.Cells[0].Text;
            }
            foreach (Control c in e.Row.Cells[10].Controls)
            {
                LinkButton button = c as LinkButton;
                if (button != null)
                {
                    if (button.Text == "Delete" || button.Text == "Cancel")
                    {
                        button.CssClass = "btn btn-danger";
                        button.ControlStyle.CssClass = "btn btn-danger";
                    }
                    else if (button.Text == "Edit" || button.Text == "Update")
                    {
                        button.CssClass = "btn btn-primary";
                        button.ControlStyle.CssClass = "btn btn-primary";
                    }
                }
            }
        }
        if (DisplayReadOnly == true)
        {

            //gvProfServiceLineDetails.Columns[10].Visible = false;
            //gvProfServiceLineDetails.Columns[11].Visible = false;

            //gvProfServiceLineDetails.Columns[10].Visible = false;
            //gvProfServiceLineDetails.Columns[12].Visible = false;

            // divProf.Visible = false;
        }
        else if (DisplayReadOnly == false)
        {

            //gvProfServiceLineDetails.Columns[10].Visible = true;
            //gvProfServiceLineDetails.Columns[11].Visible = true;

            //gvProfServiceLineDetails.Columns[10].Visible = true;
            //gvProfServiceLineDetails.Columns[12].Visible = true;

            divProf.Visible = true;
        }

    }

    protected void gvProfServiceLineDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

        if (ValidateProfessionalData())
        {
            int id = 0;

            //if (gvProfServiceLineDetails != null && gvProfServiceLineDetails.Rows.Count > 0)
            //    id = Convert.ToInt32(gvProfServiceLineDetails.Rows[e.RowIndex].Cells[0].Text);

            Dictionary<string, string> parms = new Dictionary<string, string>();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
            parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, id, true));

            DataSet dsClaimServiceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
            if (Helper.HasRows(dsClaimServiceLineDetails) && Convert.ToInt32(dsClaimServiceLineDetails.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimId) && Convert.ToInt32(dsClaimServiceLineDetails.Tables[0].Rows[0]["Service_Line"]) == id)
            {
                parms.Add("Procedure_Code", txtServiceProcedureCode.Text);
                parms.Add("Status", lblProfStatus.Text);
                if (!string.IsNullOrEmpty(txtProffdateofservice.Text))
                    parms.Add("Date_of_Service", Convert.ToDateTime(txtProffdateofservice.Text).ToString("MM/dd/yyyy"));
                parms.Add("Modifier1", txtModifier1.Text);
                parms.Add("Modifier2", txtModifier2.Text);
                parms.Add("Modifier3", txtModifier3.Text);
                parms.Add("Modifier4", txtModifier4.Text);
                parms.Add("Diagnosis_Pointer1", ddlDiagnosisPointer1.SelectedValue);
                parms.Add("Diagnosis_Pointer2", ddlDiagnosisPointer2.SelectedValue);
                parms.Add("Diagnosis_Pointer3", ddlDiagnosisPointer3.SelectedValue);
                parms.Add("Diagnosis_Pointer4", ddlDiagnosisPointer4.SelectedValue);
                parms.Add("Charges", txtCharges.Text);
                parms.Add("Line_Control_Number", txtLineCntrlNumber.Text);
                parms.Add("Billed_Units", txtBilledUnits.Text);
                parms.Add("Prior_Authorization_Number", txtPriorAuthNumber.Text);
                parms.Add("Referred_EPSDT_Service", ddlEpsdtService.Text);
                parms.Add("Unit_of_Measurement", ddlUnitsOfMeasurement.SelectedValue);
                parms.Add("Referral_Number", txtReferralNumber.Text);
                parms.Add("Family_Planning", ddlFamilyPlanning.SelectedValue);
                parms.Add("Emergency", ddlEmergency.SelectedValue);
                parms.Add("DME_Cert_Type", ddlDMECertType.SelectedValue);
                if (!string.IsNullOrEmpty(lblProfPaidUnits.Text))
                    parms.Add("Paid_Units", lblProfPaidUnits.Text);
                if (!string.IsNullOrEmpty(lblProfPaidAmount.Text))
                    parms.Add("Paid_Amount", lblProfPaidAmount.Text);
                parms.Add("DME_Duration", txtDuration.Text);
                parms.Add("Final_EAPG", lblProfFinalEAPG.Text);
                parms.Add("Cert_Revision", string.IsNullOrEmpty(txtCertRev.Text) ? "" : Convert.ToDateTime(txtCertRev.Text).ToString("MM/dd/yyyy"));
                parms.Add("Payment_Action", lblProfPaymentAction.Text);
                //if (!string.IsNullOrEmpty(lblProfTotalCharges.Text))
                //    parms.Add("Total_Charges", lblProfTotalCharges.Text);
                //if (!string.IsNullOrEmpty(lblProfTotalAmountPaid.Text))
                //    parms.Add("Total_Amount_Paid", lblProfTotalAmountPaid.Text);

                if (!string.IsNullOrEmpty(lblProfTotalCharges.Text))
                {

                    if (lblProfTotalCharges.Text.StartsWith("$"))
                    {
                        lblProfTotalCharges.Text = lblProfTotalCharges.Text.Replace("$", "");
                    }

                    parms.Add("Total_Charges", lblProfTotalCharges.Text);


                }
                if (!string.IsNullOrEmpty(lblProfTotalAmountPaid.Text))
                {
                    if (lblProfTotalAmountPaid.Text.StartsWith("$"))
                    {
                        lblProfTotalAmountPaid.Text = lblProfTotalAmountPaid.Text.Replace("$", "");

                    }

                    parms.Add("Total_Amount_Paid", lblProfTotalAmountPaid.Text);


                }
                parms.Add("Place_of_Service", txtProfPlaceOfService.Text);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Claims_Service_Details_ID", dsClaimServiceLineDetails.Tables[0].Rows[0]["Claims_Service_Details_ID"].ToString());
                svc.UpdatePanelsData("Claims_Service_Details", parms);
            }

            //gvProfServiceLineDetails.EditIndex = -1;
            BindData();
            ProfInfoAdd.Enabled = true;
            ClearProfPanelData();
            RefreshOtherPayerPaidAmountDropdown();
        }
    }


    //protected void ProfInfoAdd_Click(object sender, EventArgs e)
    //{
    //    ValidateProfessionalData();
    //    if (Page.IsValid)
    //    {
    //        AddNewRowToGrid();
    //        //CurrentServiceLineNo();
    //        BindData();
    //        RefreshOtherPayerPaidAmountDropdown();
    //    }
    //}

    private void ClearProfPanelData()
    {
        ucSubmitClaimSearchProc.ProcedureCode = "";
        Session["PlaceofServiceCode"] = null;
        txtServiceProcedureCode.Text = string.Empty;
        txtProfPlaceOfService.Text = string.Empty;
        txtProffdateofservice.Text = string.Empty;
        txtModifier1.Text = string.Empty;
        txtModifier2.Text = string.Empty;
        txtModifier3.Text = string.Empty;
        txtModifier4.Text = string.Empty;
        txtCharges.Text = string.Empty;
        txtLineCntrlNumber.Text = string.Empty;
        ddlDiagnosisPointer1.ClearSelection();
        ddlDiagnosisPointer2.ClearSelection();
        ddlDiagnosisPointer3.ClearSelection();
        ddlDiagnosisPointer4.ClearSelection();
        txtBilledUnits.Text = string.Empty;
        txtPriorAuthNumber.Text = string.Empty;
        ddlUnitsOfMeasurement.ClearSelection();
        ddlEpsdtService.ClearSelection();
        ddlFamilyPlanning.ClearSelection();
        txtReferralNumber.Text = string.Empty;
        ddlDMECertType.ClearSelection();
        txtDuration.Text = string.Empty;
        txtCertRev.Text = string.Empty;

        ucSubmitClaimSearchPop.CleareField();
        upProfServiceLineDetails1.InnerHtml = "";
    }

    private bool ValidateProfessionalData()
    {
        lblpointerErr.Text = "";
        bool isValid = true;
        if (string.IsNullOrEmpty(txtServiceProcedureCode.Text) || (!string.IsNullOrEmpty(txtServiceProcedureCode.Text) && (txtServiceProcedureCode.Text.ToString().StartsWith("MR", StringComparison.InvariantCultureIgnoreCase) || txtServiceProcedureCode.Text.ToString().StartsWith("DD", StringComparison.InvariantCultureIgnoreCase) || txtServiceProcedureCode.Text.ToString().StartsWith("PNM", StringComparison.InvariantCultureIgnoreCase))))
        {
            AddValidationErrorMessage("*Procedure code is invalid");
            txtServiceProcedureCode.Text = string.Empty;
            isValid = false;
        }
        if (string.IsNullOrEmpty(txtProffdateofservice.Text))
        {
            AddValidationErrorMessage("*Date of service is required");
            isValid = false;
        }
        if (ddlDMECertType.SelectedValue != string.Empty && !CheckIfNumber(txtDuration.Text))
        {
            AddValidationErrorMessage("*DME Duration (month) allows only numeric values");
            isValid = false;
        }
        if (ddlDMECertType.SelectedValue != string.Empty && string.IsNullOrEmpty(txtDuration.Text))
        {
            AddValidationErrorMessage("*DME Duration (month) is required.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(ddlDMECertType.SelectedValue) || ddlDMECertType.SelectedValue == "I")
        {
            txtCertRev.Enabled = false;
        }
        else
        {
            txtCertRev.Enabled = true;
            if (string.IsNullOrEmpty(txtCertRev.Text))
            {
                AddValidationErrorMessage("*Certification Revision or Recertification date is required.");
                isValid = false;
            }
        }
        if (string.IsNullOrEmpty(txtProfPlaceOfService.Text))
        {
            AddValidationErrorMessage("*Place of service is invalid.");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtModifier1.Text) && (txtModifier1.Text.Length < 2 || txtModifier1.Text.Length > 2))
        {
            AddValidationErrorMessage("*Procedure code modifier 1 must be 2 characters.");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtModifier2.Text) && (txtModifier2.Text.Length < 2 || txtModifier2.Text.Length > 2))
        {
            AddValidationErrorMessage("*Procedure code modifier 2 must be 2 characters.");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtModifier3.Text) && (txtModifier3.Text.Length < 2 || txtModifier3.Text.Length > 2))
        {
            AddValidationErrorMessage("*Procedure code modifier 3 must be 2 characters.");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtModifier4.Text) && (txtModifier4.Text.Length < 2 || txtModifier4.Text.Length > 2))
        {
            AddValidationErrorMessage("*Procedure code modifier 4 must be 2 characters.");
            isValid = false;
        }

        if (string.IsNullOrEmpty(ddlDiagnosisPointer1.SelectedValue) && string.IsNullOrEmpty(ddlDiagnosisPointer2.SelectedValue) && string.IsNullOrEmpty(ddlDiagnosisPointer3.SelectedValue) && string.IsNullOrEmpty(ddlDiagnosisPointer4.SelectedValue))
        {
            AddValidationErrorMessage("*At least one diagnosis pointer is required");
            lblpointerErr.Text = "* At least one diagnosis pointer is required";

            isValid = false;
        }
        if (!CheckIfNumber(txtCharges.Text))
        {
            AddValidationErrorMessage("*Charges allowd only numeric values.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(txtCharges.Text))
        {
            AddValidationErrorMessage("*Charges is required in the service detail line.");
            lblpointerError.Text = "*Charges is required in the service detail line.";
            isValid = false;
        }
        if (string.IsNullOrEmpty(ddlUnitsOfMeasurement.SelectedValue))
        {
            AddValidationErrorMessage("*Units of measurement is required.");
            isValid = false;
        }
        if (!CheckIfNumber(txtBilledUnits.Text))
        {
            AddValidationErrorMessage("*Billed units allows only numeric values");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtBilledUnits.Text) && (Convert.ToDecimal(txtBilledUnits.Text) < 0 || Convert.ToDecimal(txtBilledUnits.Text) == 0))
        {
            AddValidationErrorMessage("*Billed units should be greater than zero");
            isValid = false;
        }
        if (!string.IsNullOrEmpty(txtProfPlaceOfService.Text))
        {
            bool revCodeExists = ClaimsController.IsValidPlaceOfservice(txtProfPlaceOfService.Text.Trim());

            if (!revCodeExists)
            {
                AddValidationErrorMessage("*Place of service is invalid.");
                isValid = false;
            }
        }


        if (!string.IsNullOrEmpty(txtServiceProcedureCode.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureCode(txtServiceProcedureCode.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure code is invalid.");
                txtServiceProcedureCode.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtModifier1.Text))
        {

            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifier1.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier1 is invalid.");
                txtModifier1.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtModifier2.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifier2.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier2 is invalid.");
                txtModifier2.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtModifier3.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifier3.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier3 is invalid.");
                txtModifier3.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtModifier4.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtModifier4.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier4 is invalid.");
                txtModifier4.Text = string.Empty;
                isValid = false;
            }
        }

        return isValid;
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
            ValidationGroup = "valProfServiceLineInfo"
        };
        this.Page.Validators.Add(val);
        return false;
    }
    private void AddNewRowToGrid()
    {

        DataTable dtCurrentTable = (DataTable)ViewState["CurrentTable"];
        //int i = dtCurrentTable != null ? dtCurrentTable.Rows.Count : 0;


        if (dtCurrentTable != null && dtCurrentTable.Rows.Count > 30)
        {
            AddValidationErrorMessage("Max 30 records are allowed");
        }

        else if (ViewState["CurrentTable"] != null)
        {
            DataRow drCurrentRow = dtCurrentTable.NewRow();

            //int latestServiceLine = ((DataTable)ViewState["CurrentTable"]).Rows.Count > 0 ? Convert.ToInt32(((DataTable)ViewState["CurrentTable"]).Rows[((DataTable)ViewState["CurrentTable"]).Rows.Count - 1]["Service_Line"]) : 0;

            //drCurrentRow["Service_Line"] = "0" + (latestServiceLine + 1);
            drCurrentRow["Service_Line"] = lblDetailsItemProf.Text;
            if (!string.IsNullOrEmpty(txtServiceProcedureCode.Text))
                drCurrentRow["Procedure_code"] = txtServiceProcedureCode.Text;
            if (!string.IsNullOrEmpty(txtProfPlaceOfService.Text))
                drCurrentRow["Place_of_Service"] = txtProfPlaceOfService.Text;
            if (!string.IsNullOrEmpty(txtBilledUnits.Text))
                drCurrentRow["Billed_Units"] = txtBilledUnits.Text;
            drCurrentRow["Unit_Of_Measurement"] = ddlUnitsOfMeasurement.SelectedValue;
            if (!string.IsNullOrEmpty(txtProffdateofservice.Text))
                drCurrentRow["Date_of_Service"] = txtProffdateofservice.Text;
            if (!string.IsNullOrEmpty(txtCharges.Text))
                drCurrentRow["Charges"] = txtCharges.Text;
            if (!string.IsNullOrEmpty(lblProfPaidUnits.Text))
                drCurrentRow["Paid_Units"] = lblProfPaidUnits.Text;
            if (!string.IsNullOrEmpty(lblProfPaidAmount.Text))
                drCurrentRow["Paid_Amount"] = lblProfPaidAmount.Text;
            if (!string.IsNullOrEmpty(lblProfStatus.Text))
                drCurrentRow["Status"] = lblProfStatus.Text;


            dtCurrentTable.Rows.Add(drCurrentRow);

            ViewState["CurrentTable"] = dtCurrentTable;


            if (!string.IsNullOrEmpty(lblProfTotalAmountPaid.Text))
            {
                if (lblProfTotalAmountPaid.Text.StartsWith("$"))
                {
                    lblProfTotalAmountPaid.Text = lblProfTotalAmountPaid.Text.Replace("$", "");

                }

            }
            if (!string.IsNullOrEmpty(lblProfTotalCharges.Text))
            {
                if (lblProfTotalCharges.Text.StartsWith("$"))
                {
                    lblProfTotalCharges.Text = lblProfTotalCharges.Text.Replace("$", "");

                }

            }

            Dictionary<string, string> parms = new Dictionary<string, string>
            {
              { "Service_Line", drCurrentRow["Service_Line"].ToString() },
                { "Procedure_Code", txtServiceProcedureCode.Text },
                { "Place_of_Service", txtProfPlaceOfService.Text },
                { "Line_Control_Number", txtLineCntrlNumber.Text  },
                { "Prior_Authorization_Number", txtPriorAuthNumber.Text },
                { "Referral_Number", txtReferralNumber.Text },
                { "Modifier1", txtModifier1.Text },
                { "Modifier2", txtModifier2.Text },
                { "Modifier3", txtModifier3.Text },
                { "Modifier4", txtModifier4.Text },
                { "Diagnosis_Pointer1", ddlDiagnosisPointer1.SelectedValue },
                { "Diagnosis_Pointer2", ddlDiagnosisPointer2.SelectedValue },
                { "Diagnosis_Pointer3", ddlDiagnosisPointer3.SelectedValue },
                { "Diagnosis_Pointer4", ddlDiagnosisPointer4.SelectedValue },
                { "Status", lblProfStatus.Text },
                { "Referred_EPSDT_Service", ddlEpsdtService.SelectedValue },
                { "Unit_Of_Measurement", ddlUnitsOfMeasurement.SelectedValue },
                { "Family_Planning", ddlFamilyPlanning.SelectedValue },
                { "Emergency", ddlEmergency.SelectedValue},
                { "DME_Cert_Type", ddlDMECertType.SelectedValue },
                { "Final_EAPG", lblProfFinalEAPG.Text},
                { "Claim_ID", Convert.ToString(ClaimId) },
                { "Date_of_Service", txtProffdateofservice.Text },
                { "Payment_Action", lblProfPaymentAction.Text },
                { "Charges",txtCharges.Text},
                { "Paid_Amount", lblProfPaidAmount.Text },
                { "Paid_Units", lblProfPaidUnits.Text},
                { "Total_Charges", lblProfTotalCharges.Text },
                { "ToTal_Amount_Paid", lblProfTotalAmountPaid.Text},
                { "Billed_Units", txtBilledUnits.Text },
                { "DME_Duration", txtDuration.Text },
                { "Cert_Revision", txtCertRev.Text},
                { "Created_Date_Time", DateTime.Now.ToString() },
                { "Claim_Type", "2" },

            };
            if (avoidDuplicateInsert())
                svc.InsertPanelsData("Claims_Service_Details", parms);
            SetTotalAmountBilled(dtCurrentTable);

            ClearProfPanelData();
        }
    }

    private bool avoidDuplicateInsert()
    {
        int lineNumberRow = 0;
        DataSet dsProfServiceLine = FetchOtherServiceDetailsInformation();
        if (Helper.HasRows(dsProfServiceLine))
        {
            int recentInsertedRecodNum = dsProfServiceLine.Tables[0].Rows.Count;
            DataRow dr = dsProfServiceLine.Tables[0].Rows[recentInsertedRecodNum - 1];
            lineNumberRow = Convert.ToInt32(dr["Service_Line"].ToString());
            int test = Convert.ToInt32(lblDetailsItemProf.Text);
            if (lineNumberRow == Convert.ToInt32(lblDetailsItemProf.Text))
            {
                return false;
            }
        }
        return true;
    }
    public void CurrentServiceLineNo()
    {
        DataSet serviceLineDetails = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        DataTable claimsDataTable = new DataTable();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
        // DataSet InstitutionalData = (DataSet)ViewState["CurrentTable"];
        DataTable dtCurrentTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        if (dtCurrentTable != null && dtCurrentTable.Rows.Count > 999)
        {
            AddValidationErrorMessage("Max 999 Service Lines are allowed");
        }
        else
        {
            DataRow drCurrentRow = dtCurrentTable.NewRow();
            int latestServiceLine = dtCurrentTable.Rows.Count > 0 ? Convert.ToInt32((dtCurrentTable).Rows[(dtCurrentTable).Rows.Count - 1]["Service_Line"]) : 0;

            lblDetailsItemProf.Text = "0" + (latestServiceLine + 1);
        }
    }
    private void SetTotalAmountBilled(DataTable dtCurrentTable)
    {
        decimal TotalAmountBilled = 0;

        foreach (DataRow row in dtCurrentTable.Rows)
        {
            decimal BilledAmount = !string.IsNullOrEmpty(row["Charges"].ToString()) ? Convert.ToDecimal(row["Charges"].ToString()) : 0;
            TotalAmountBilled += BilledAmount;
        }
        lblProfTotalCharges.Text = TotalAmountBilled.ToString();
    }

    protected void lnkSubClaimSepopSearch_Click(object sender, EventArgs e)
    {
        this.LoadData(null);
        //lblSubmitClaimSearchPop.Text = "Place of Service Search";
        mpeSubmitClaimSearchPop.Show();
    }

    protected void lnkInsProcCode_Click(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchProc.Show();
    }
    private void CheckDignosisCode()
    {
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            DataSet dsDignosisGrid = new DataSet();
            dsProfDiagnosisPointer = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnValueCode_ClaimID.Value, true));
            parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, CON.ClaimsType.Professional, true));

            dsProfDiagnosisPointer = dsDignosisGrid = DataAccess.ExecuteStoredProcedure("usp_SelectClaimsDiagnosisInformation", parameters, "claims_diagnosis_information");
            if (Helper.HasRows(dsDignosisGrid) &&
            Convert.ToInt32(dsDignosisGrid.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnValueCode_ClaimID.Value))
            {
                DataTable dt = dsDignosisGrid.Tables[0];
                Helper.LoadList(ddlDiagnosisPointer1, dt, "diag_seq", "diag_seq", true);
                //Helper.LoadList(ddlDiagnosisPointer2, dt, "diag_seq", "diag_seq", true);
                //Helper.LoadList(ddlDiagnosisPointer3, dt, "diag_seq", "diag_seq", true);
                //Helper.LoadList(ddlDiagnosisPointer4, dt, "diag_seq", "diag_seq", true);
            }
            if (!Helper.HasRows(dsDignosisGrid))
            {
                //lblpointerError.Text = "Please add data in diagnosis Panel";
            }
            else
                lblpointerError.Text = "";

        }
    }
    private DataSet LoadDiagnosisPointer()
    {
        DataSet dsProfDiagnosisPointerData = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnValueCode_ClaimID.Value, true));
        parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, CON.ClaimsType.Professional, true));

        dsProfDiagnosisPointerData = DataAccess.ExecuteStoredProcedure("usp_SelectClaimsDiagnosisInformation", parameters, "claims_diagnosis_information");
        return dsProfDiagnosisPointerData;
    }
    protected void ddlDiagnosisPointer1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiagnosisPointer1.SelectedIndex > 0)
        {
            lblpointerError.Text = "";
            lblpointerErr.Text = "";
            dsProfDiagnosisPointer = LoadDiagnosisPointer();
            if (Helper.HasRows(dsProfDiagnosisPointer) &&
            Convert.ToInt32(dsProfDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnValueCode_ClaimID.Value))
            {
                DataTable dt = dsProfDiagnosisPointer.Tables[0];
                Helper.LoadList(ddlDiagnosisPointer2, dt, "diag_seq", "diag_seq", true);
                Helper.LoadList(ddlDiagnosisPointer3, dt, "diag_seq", "diag_seq", true);
                Helper.LoadList(ddlDiagnosisPointer4, dt, "diag_seq", "diag_seq", true);
                ddlDiagnosisPointer2.Items.Remove(ddlDiagnosisPointer2.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
                ddlDiagnosisPointer3.Items.Remove(ddlDiagnosisPointer3.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
                ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
            }
        }

    }
    protected void ddlDiagnosisPointer2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiagnosisPointer1.SelectedIndex > 0)
        {
            lblpointerError.Text = "";
            lblpointerErr.Text = "";
            dsProfDiagnosisPointer = LoadDiagnosisPointer();
            if (Helper.HasRows(dsProfDiagnosisPointer) &&
            Convert.ToInt32(dsProfDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnValueCode_ClaimID.Value))
            {
                DataTable dt = dsProfDiagnosisPointer.Tables[0];
                Helper.LoadList(ddlDiagnosisPointer3, dt, "diag_seq", "diag_seq", true);
                Helper.LoadList(ddlDiagnosisPointer4, dt, "diag_seq", "diag_seq", true);
                ddlDiagnosisPointer3.Items.Remove(ddlDiagnosisPointer3.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
                ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
                ddlDiagnosisPointer3.Items.Remove(ddlDiagnosisPointer3.Items.FindByValue(ddlDiagnosisPointer2.SelectedValue));
                ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer2.SelectedValue));
            }
        }
        else
        {
            lblpointerError.Visible = true;
            lblpointerError.Text = "Please select diagnosis pointer's first dropdown.";
            ddlDiagnosisPointer4.ClearSelection();
            ddlDiagnosisPointer3.ClearSelection();
            ddlDiagnosisPointer2.ClearSelection();
            ddlDiagnosisPointer1.Focus();
        }
    }

    protected void btnCloseProc_Click(object sender, EventArgs e)
    {
        ucSubmitClaimSearchProc.CleareField();
    }
    protected void ddlDiagnosisPointer3_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiagnosisPointer2.SelectedIndex > 0)
        {
            lblpointerError.Text = "";
            lblpointerErr.Text = "";
            dsProfDiagnosisPointer = LoadDiagnosisPointer();
            if (Helper.HasRows(dsProfDiagnosisPointer) &&
            Convert.ToInt32(dsProfDiagnosisPointer.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnValueCode_ClaimID.Value))
            {
                DataTable dt = dsProfDiagnosisPointer.Tables[0];
                Helper.LoadList(ddlDiagnosisPointer4, dt, "diag_seq", "diag_seq", true);
                if (ddlDiagnosisPointer1.SelectedIndex > 0)
                    ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer1.SelectedValue));
                if (ddlDiagnosisPointer2.SelectedIndex > 0)
                    ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer2.SelectedValue));
                if (ddlDiagnosisPointer3.SelectedIndex > 0)
                    ddlDiagnosisPointer4.Items.Remove(ddlDiagnosisPointer4.Items.FindByValue(ddlDiagnosisPointer3.SelectedValue));


            }
            Session["DPointer3"] = ddlDiagnosisPointer4.Items.OfType<ListItem>();

        }
        else
        {
            lblpointerError.Text = "Please select diagnosis pointer first/second dropdown.";
            ddlDiagnosisPointer4.ClearSelection();
            ddlDiagnosisPointer3.ClearSelection();
            ddlDiagnosisPointer2.Focus();
        }
    }
    protected void ddlDiagnosisPointer4_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDiagnosisPointer3.SelectedIndex <= 0)
        {
            lblpointerErr.Text = "";
            lblpointerError.Text = "Please select diagnosis pointer's previous dropdown.";
            ddlDiagnosisPointer4.ClearSelection();
            ddlDiagnosisPointer3.Focus();
        }
    }
    public void ClearFields()
    {
        txtServiceProcedureCode.Text = string.Empty;
        txtProfPlaceOfService.Text = string.Empty;
        lblProfStatus.Text = string.Empty;
        txtProffdateofservice.Text = string.Empty;
        txtModifier1.Text = string.Empty;
        txtModifier2.Text = string.Empty;
        txtModifier3.Text = string.Empty;
        txtModifier4.Text = string.Empty;
        txtCharges.Text = string.Empty;
        txtLineCntrlNumber.Text = string.Empty;
        ddlDiagnosisPointer1.ClearSelection();
        ddlDiagnosisPointer2.ClearSelection();
        ddlDiagnosisPointer3.ClearSelection();
        ddlDiagnosisPointer4.ClearSelection();
        txtBilledUnits.Text = string.Empty;
        txtPriorAuthNumber.Text = string.Empty;
        ddlUnitsOfMeasurement.ClearSelection();
        txtReferralNumber.Text = string.Empty;
        lblProfPaidUnits.Text = string.Empty;
        ddlDMECertType.ClearSelection();
        lblProfPaidAmount.Text = string.Empty;
        txtDuration.Text = string.Empty;
        lblProfFinalEAPG.Text = string.Empty;
        txtCertRev.Text = string.Empty;
        lblProfPaymentAction.Text = string.Empty;
        lblProfTotalCharges.Text = string.Empty;
        lblProfTotalAmountPaid.Text = string.Empty;
        upProfServiceLineDetails1.InnerHtml = "";
    }
    public void ClearGrid()
    {
        //gvProfServiceLineDetails.DataSource = null;
        //gvProfServiceLineDetails.DataBind();
        lblDetailsItemProf.Text = "01";
    }
    //private bool checkDateFormat(string inputString)
    //{
    //    string[] formats = { "MM/dd/yyyy" };
    //    DateTime parsedDate;
    //    var isValidFormat = DateTime.TryParseExact(inputString, formats, new CultureInfo("en-US"), DateTimeStyles.None, out parsedDate);

    //    if (isValidFormat)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (profStatus.ToUpper() == "PAID" && IsClaimCopy)
        {
            hdnHidePaidAmounts.Value = "1";
        }
        if (Helper.HasRows(dt))
        {
            if (avoidDuplicateInsert())
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string DateService = null;
                    if (!string.IsNullOrEmpty(dt.Rows[i]["Date_of_Service"].ToString()))
                    {
                        if (checkDateFormat(dt.Rows[i]["Date_of_Service"].ToString()))
                        {
                            DateService = dt.Rows[i]["Date_of_Service"].ToString();
                        }
                        else
                        {
                            DateService = DateTime.ParseExact(dt.Rows[i]["Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                        }
                        // DateService = DateTime.ParseExact(dt.Rows[i]["Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    string lblserviceline = "";
                    int serviceline = Convert.ToInt32(dt.Rows[i]["Service_Line"].ToString());
                    lblserviceline = serviceline.ToString();
                    Dictionary<string, string> parms = new Dictionary<string, string> {
                { "Service_Line", !string.IsNullOrEmpty(lblserviceline)? lblserviceline:null},
                { "Procedure_Code",!string.IsNullOrEmpty(dt.Rows[i]["Procedure_Code"].ToString())? dt.Rows[i]["Procedure_Code"].ToString():null },
                { "Place_of_Service", !string.IsNullOrEmpty(dt.Rows[i]["Place_of_Service"].ToString())? dt.Rows[i]["Place_of_Service"].ToString():null },
                { "Line_Control_Number", !string.IsNullOrEmpty(dt.Rows[i]["Line_Control_Number"].ToString())? dt.Rows[i]["Line_Control_Number"].ToString():null  },
                { "Prior_Authorization_Number", !string.IsNullOrEmpty(dt.Rows[i]["Prior_Authorization_Number"].ToString())? dt.Rows[i]["Prior_Authorization_Number"].ToString():null },
                { "Referral_Number", !string.IsNullOrEmpty(dt.Rows[i]["Referral_Number"].ToString())? dt.Rows[i]["Referral_Number"].ToString():null},
                { "Modifier1", !string.IsNullOrEmpty(dt.Rows[i]["Modifier1"].ToString())? dt.Rows[i]["Modifier1"].ToString():null },
                { "Modifier2", !string.IsNullOrEmpty(dt.Rows[i]["Modifier2"].ToString())? dt.Rows[i]["Modifier2"].ToString():null},
                { "Modifier3", !string.IsNullOrEmpty(dt.Rows[i]["Modifier3"].ToString())? dt.Rows[i]["Modifier3"].ToString():null},
                { "Modifier4", !string.IsNullOrEmpty(dt.Rows[i]["Modifier4"].ToString())? dt.Rows[i]["Modifier4"].ToString():null},
                { "Diagnosis_Pointer1", !string.IsNullOrEmpty(dt.Rows[i]["Diagnosis_Pointer1"].ToString())? dt.Rows[i]["Diagnosis_Pointer1"].ToString():null},
                { "Diagnosis_Pointer2", !string.IsNullOrEmpty(dt.Rows[i]["Diagnosis_Pointer2"].ToString())? dt.Rows[i]["Diagnosis_Pointer2"].ToString():null},
                { "Diagnosis_Pointer3", !string.IsNullOrEmpty(dt.Rows[i]["Diagnosis_Pointer3"].ToString())? dt.Rows[i]["Diagnosis_Pointer3"].ToString():null},
                { "Diagnosis_Pointer4", !string.IsNullOrEmpty(dt.Rows[i]["Diagnosis_Pointer4"].ToString())? dt.Rows[i]["Diagnosis_Pointer4"].ToString():null},
                { "Status", "Pending Submission"},
                { "Referred_EPSDT_Service",!string.IsNullOrEmpty(dt.Rows[i]["Referred_EPSDT_Service"].ToString())? dt.Rows[i]["Referred_EPSDT_Service"].ToString():null},
                { "Unit_Of_Measurement", !string.IsNullOrEmpty(dt.Rows[i]["Unit_Of_Measurement"].ToString())? dt.Rows[i]["Unit_Of_Measurement"].ToString():null},
                { "Family_Planning", !string.IsNullOrEmpty(dt.Rows[i]["Family_Planning"].ToString())? dt.Rows[i]["Family_Planning"].ToString():null },
                { "Emergency", !string.IsNullOrEmpty(dt.Rows[i]["Emergency"].ToString())? dt.Rows[i]["Emergency"].ToString():null},
                { "DME_Cert_Type", !string.IsNullOrEmpty(dt.Rows[i]["DME_Cert_Type"].ToString())? dt.Rows[i]["DME_Cert_Type"].ToString():null},
               
                { "Final_EAPG",null},

                { "Claim_ID", hdnValueCode_ClaimID.Value },
                { "Date_of_Service",DateService },
           
                { "Payment_Action", null },
                { "Charges",!string.IsNullOrEmpty(dt.Rows[i]["Charges"].ToString())? dt.Rows[i]["Charges"].ToString():null},
                { "Paid_Amount",!string.IsNullOrEmpty(dt.Rows[i]["Paid_Amount"].ToString())  &&  hdnHidePaidAmounts.Value != "1"?dt.Rows[i]["Paid_Amount"].ToString().Replace('$', ' '):null },
                { "Paid_Units", !string.IsNullOrEmpty(dt.Rows[i]["Paid_Units"].ToString())  &&  hdnHidePaidAmounts.Value != "1"? dt.Rows[i]["Paid_Units"].ToString():null},
             
                { "Total_Charges",  !string.IsNullOrEmpty(TotalCharges)?TotalCharges.ToString().Replace('$', ' '):null },
                { "ToTal_Amount_Paid",!string.IsNullOrEmpty(TotalAmountPaid.ToString())?TotalAmountPaid.Replace('$',' '):null},
                { "Billed_Units", !string.IsNullOrEmpty(dt.Rows[i]["Billed_Units"].ToString()) ? dt.Rows[i]["Billed_Units"].ToString():null},
                { "DME_Duration", !string.IsNullOrEmpty(dt.Rows[i]["DME_Duration"].ToString())? dt.Rows[i]["DME_Duration"].ToString():null },
                { "Cert_Revision", !string.IsNullOrEmpty(dt.Rows[i]["Cert_Revision"].ToString())? dt.Rows[i]["Cert_Revision"].ToString():null},
                { "Created_Date_Time", DateTime.Now.ToString() },
                { "Claim_Type", "2" },
                { "Clear_Prior_Data", "1" }
                };
                    svc.InsertPanelsData("Claims_Service_Details", parms);

                }
                SetTotalAmountBilled(dt);
                BindData();
                RefreshOtherPayerPaidAmountDropdown();

            }
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


    protected void txtProfPlaceOfService_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        Regex nonNumericRegex = new Regex(@"\D");
        if (!string.IsNullOrEmpty(txtProfPlaceOfService.Text))
        {
            if (nonNumericRegex.IsMatch(txtProfPlaceOfService.Text))
            {
                lblError.Text = "Invalid Place Of Service.";
                return;
            }
            else
            {
                DataTable dt = LookupTableController.GetPlaceofServiceDetail(txtProfPlaceOfService.Text.TrimEnd(), null);

                if (!Helper.HasRows(dt))
                {
                    lblError.Text = "Invalid Place Of Service.";
                    return;
                }


            }
        }
    }
    public void SetProfessionalServiceDetailPanelData()
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnValueCode_ClaimID.Value, true));
            dsServiceDetailsProfData = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", parameters, "Claims_Service_Details");
            if (Helper.HasRows(dsServiceDetailsProfData))
            {
                lblDetailsItemProf.Text = (dsServiceDetailsProfData.Tables[0].Rows.Count + 1).ToString();
            }
            else
            {
                lblDetailsItemProf.Text = "1";
            }
        }
        else
        {
            lblDetailsItemProf.Text = "1";
        }
    }
}



