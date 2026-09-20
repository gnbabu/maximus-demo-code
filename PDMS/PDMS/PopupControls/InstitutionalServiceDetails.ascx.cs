using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using Models.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Charting.Styles;

public partial class PopupControls_InstitutionalServiceDetails : System.Web.UI.UserControl
{
    public delegate void EventHandler();
    public event EventHandler RefreshChildGrids;
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private bool showServiceDetails = true;
    private int claim_Id;
    #endregion

    //property
    private string FromDateInstitutional = string.Empty;
    private string ToDateInstitutional = string.Empty;
    private bool fromInquirySvc = false;

    public string ServiceInfoDetailofServiceofFromDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(FromDateInstitutional))
                return FromDateInstitutional;

            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                FromDateInstitutional = value.Trim();
        }

    }
    public string ServiceInfoDetailofServiceofToDate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ToDateInstitutional))
                return ToDateInstitutional;

            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ToDateInstitutional = value.Trim();
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
        divFinalEAPG.Visible = false;
        divPaymentAction.Visible = false;
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
    public string TotalAmountBilled
    {
        get
        {
            if (!string.IsNullOrEmpty(lblInsTotalAmountBilled.Text))
                return lblInsTotalAmountBilled.Text;
            else
                return string.Empty;
        }
        set
        {
            lblInsTotalAmountBilled.Text = value;
        }
    }
    private DataSet dsServiceDetailsInsData = new DataSet();
    public DataSet dataSetServiceDetails
    {
        get
        {
            if (!Helper.HasRows(dsServiceDetailsInsData))
            {
                if (Helper.HasRows(FetchOtherServiceDetailsInformation()))
                {
                    return dsServiceDetailsInsData;
                }
            }
            return dsServiceDetailsInsData;

        }

        set
        {
            if (value != null)
            {
                SetServiceDetails(value);
            }
        }

    }
    private DataSet FetchOtherServiceDetailsInformation()
    {
        DataSet dsServiceDetailsInfo = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnValueCode_ClaimID.Value, true));
        parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, "1", true));
        dsServiceDetailsInsData = dsServiceDetailsInfo = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_ServiceLine_Prof", parameters, "Claims_Service_Details");
        return dsServiceDetailsInfo;
    }
    public string TotalAmountPaid
    {
        get
        {
            if (!string.IsNullOrEmpty(lblInsTotalAmountPaid.Text))
                return lblInsTotalAmountPaid.Text;
            else
                return string.Empty;
        }
        set
        {
            lblInsTotalAmountPaid.Text = value;
        }
    }
    private string InStitutionalStatusval = string.Empty;
    public string InstitutionalStatus
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(InStitutionalStatusval))
                return InStitutionalStatusval;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                InStitutionalStatusval = value;
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

    public bool ShowServiceDetails { get { return showServiceDetails; } set { showServiceDetails = value; } }
    public DataSet gvServiceLineDetails
    {
        get
        {
            if (ViewState["CurrentTable"] != null)
                return (DataSet)ViewState["CurrentTable"];
            else
                return null;
        }
        set
        {
            if (value != null)
            {
                SetServiceDetails(value);
                ViewState["CurrentTable"] = value;
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
            BindData();

        }
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ucSubmitClaimSearchProc.ProcedureCode))
        {
            txtInsProcCode.Text = ucSubmitClaimSearchProc.ProcedureCode;
        }

        if (!string.IsNullOrEmpty(ucSubmitClaimSearchRevenueCode.RevenueCode))
        {
            txtInstRevenueCode.Text = ucSubmitClaimSearchRevenueCode.RevenueCode;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //InsFutureToDOS.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        //InsFutureToDOS2.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        //InsFutureFromDOS.ValueToCompare = DateTime.Now.ToString("MM/dd/yyyy");
        hdnProviderTypeId.Value = this.WorkflowPage.ProviderTypeID.ToString();

        if (ddlInsUnitsOfMeasurement.Items.Count == 0)
        {
            DataSet dsUnitsOfMeasurement = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Is_Institutional", DbType.Boolean, 1, true));

            dsUnitsOfMeasurement = DataAccess.ExecuteStoredProcedure("usp_Select_UnitsOfMeasurement", parameters, "Units_Of_Measurement");

            ddlInsUnitsOfMeasurement.DataTextField = dsUnitsOfMeasurement.Tables[0].Columns["CLAIM_UNITSOFMEASUREMENT_CODE"].ToString();
            ddlInsUnitsOfMeasurement.DataValueField = dsUnitsOfMeasurement.Tables[0].Columns["CLAIM_UNITSOFMEASUREMENT_CODE"].ToString();

            ddlInsUnitsOfMeasurement.DataSource = dsUnitsOfMeasurement.Tables[0];
            ddlInsUnitsOfMeasurement.DataBind();

            ddlInsUnitsOfMeasurement.Items.Insert(0, new ListItem("", ""));

        }
        if (!Page.IsPostBack)
        {
            BindData();
            lblInstStatus.Text = InstitutionalStatus;
        }
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value) && !FromInquirySvc)
        {
            //DataSet dtProfessionalDataset = new DataSet();
            SetServiceDetails(dsServiceDetailsInsData);
        }
        SetButtonVisibility();
        //if (serviceDetailInsti.Visible == true)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>displayInstiServiceDetailtable();</script>", false);
        //}
        if (serviceDetailInsti.Visible == true)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>clearInstiServiceDetailFields();</script>", false);
        }
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            ServiceLine1.Visible = false;
        }
        else
        {
            ServiceLine1.Visible = true;
        }
    }

    protected void BindData()
    {
        DataSet serviceLineDetails = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
        DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;

        ViewState["CurrentTable"] = serviceLineDetails;
        if (claimsDataTable != null)
        {
            int latestServiceLine = claimsDataTable.Rows.Count > 0 ? Convert.ToInt32((claimsDataTable).Rows[(claimsDataTable).Rows.Count - 1]["Service_Line"]) : 0;
            lblDetailsItemInst.Text = "0" + (latestServiceLine + 1);

        }

        SetTotalAmountBilled(claimsDataTable);



    }

    private void SetServiceDetails(DataSet dsServiceDetails)
    {
        try
        {
            hdnInstiServiceLineClaimStatus.Value = "";
            DataSet dsServiceDetailsDental = new DataSet();
            if (InstitutionalStatus.ToUpper() == "PAID" && IsClaimCopy)
            {
                hdnHidePaidAmounts.Value = "1";
            }
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


                serviceDetailInsti.InnerHtml = "";
                if (diagTable.Rows.Count > 0)
                {
                    string tab = string.Empty;
                    int sequence = 0;
                    decimal totalcharge = 0;
                    decimal totalAmoutPaid = 0;

                    tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>*Revenue Code</th><th style='width:10px; scope='col'>Procedure Type</th><th style='width:10px; scope='col'>Procedure Code</th><th style='width:10px; scope='col'>*Unit</th><th style='width:10px; scope='col'>Unit Of Measurement</th><th style='width:10px; scope='col'>*From DOS</th><th style='width:10px; scope='col'>To DOS</th><th style='width:10px; scope='col'>*Total Charges</th><th style='width:10px; scope='col'>Paid Amount</th><th style='width:10px; scope='col'>Status</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                    foreach (DataRow dr in diagTable.Rows)
                    {

                        sequence++;
                        string sequencedata = null;

                        sequencedata = sequence.ToString();

                        string revenue_Code = dr["Revenue_Code"].ToString();
                        string procedure_type = dr["Procedure_Type"].ToString();
                        string cde_proc = dr["Procedure_code"].ToString();
                        string unit = dr["Billed_Units"].ToString();
                        string unit_of_measurement = dr["Unit_Of_Measurement"].ToString();
                        string total_charge = dr["Total_Charges"].ToString();
                        string fromDOS = dr["Date_of_Service"].ToString();
                        string toDOS = dr["To_Date_of_Service"].ToString();

                        string paid_amt;
                        if (!string.IsNullOrEmpty(dr["Paid_Amount"].ToString()))
                        {
                            if (InstitutionalStatus.ToUpper() == "PAID" && IsClaimCopy)
                            {
                                paid_amt = "";
                            }
                            else
                            {
                                paid_amt = dr["Paid_Amount"].ToString().Contains('$') ? dr["Paid_Amount"].ToString().Replace('$', ' ').TrimStart() : dr["Paid_Amount"].ToString();
                            }
                        }
                        else { paid_amt = ""; }
                        string status = dr["Status"].ToString();
                        // string Claim_service_Id = dr["Claims_Service_Details_ID"].ToString();
                        string Claim_Id = hdnValueCode_ClaimID.Value;
                        string serviceline = dr["Service_Line"].ToString();
                        if (!string.IsNullOrEmpty(dr["Total_Charges"].ToString()))
                        {
                            totalcharge = totalcharge + Convert.ToDecimal(total_charge);
                        }
                        if (!string.IsNullOrEmpty(paid_amt))
                        {
                            totalAmoutPaid = totalAmoutPaid + Convert.ToDecimal(paid_amt);
                        }
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequencedata + "</span></td><td><span title='Line' class='tNumber'>" + revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + procedure_type + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td>" + unit + "</td><td>" + unit_of_measurement + "</td><td>" + fromDOS + "</td><td>" + toDOS + "</td><td>" + total_charge + "</td><td>" + paid_amt + "</td><td>Pending Submission</td><td><input type='button' value = 'Edit' onClick = 'return EditInstiServiceLineItem(\"" + 1 + "\",\"" + Claim_Id + "\",\"" + serviceline + "\", this); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value = 'Copy' onClick = 'return EditInstiServiceLineItem(\"" + 1 + "\",\"" + Claim_Id + "\",\"" + serviceline + "\", this, 1); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteInstiServiceDetailLineitem(\"" + Claim_Id + "\",\"" + serviceline + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                hdnInstiServiceLineClaimStatus.Value = "Pending Submission";
                            }
                            else
                            {
                                tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + sequencedata + "</span></td><td><span title='Line' class='tNumber'>" + revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + procedure_type + "</span></td><td><span title='Line' class='tNumber'>" + cde_proc + "</span></td><td>" + unit + "</td><td>" + unit_of_measurement + "</td><td>" + fromDOS + "</td><td>" + toDOS + "</td><td>" + total_charge + "</td><td>" + paid_amt + "</td><td>" + status + "</td></tr >";
                                hdnInstiServiceLineClaimStatus.Value = "Other";
                            }
                        }
                    }
                    if (Session["ClaimStatus"].ToString() != "Pending Submission")
                    {
                        tab = tab + "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td> Total Charges:</td><td> " + totalcharge.ToString() + "</td></tr>";
                        tab = tab + "<tr><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td></td><td>Total Amount Paid: </td><td>" + totalAmoutPaid.ToString() + "</td></tr>";
                        tab = tab + "</tbody></table>";
                    }
                    else { tab = tab + "</tbody></table>"; }
                    //  instibillSec.Visible = false;
                    InstInfoAdd.Enabled = true;
                    btnCancelInsti.Enabled = true;
                    btnUpdateServiceDetailInsti.Enabled = true;
                    serviceDetailInsti.InnerHtml = tab;
                    lblInsTotalAmountBilled.Text = totalcharge.ToString();
                    lblInsTotalAmountPaid.Text = totalAmoutPaid.ToString();
                }

            }
        }
        catch (Exception ex) { }
    }

    protected void txtCheckForDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtInsFromDOS.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate9();</script>", false);
        }
        else
        {
            InstServiceDetailDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtInsFromDOS.Text))
        {
            if (InValidDateError(txtInsFromDOS, lblErrorFromDOS, InstServiceDetailDateRequiredError1))
            {
                return;
            }
        }
    }

    protected void txtCheckForToDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtInsToDOS.Text))
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDate10();</script>", false);
        }
        else
        {
            InstServiceDetailToDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtInsToDOS.Text))
        {
            if (InValidDateError(txtInsToDOS, lblErrorToDOS, InstServiceDetailToDateRequiredError1))
            {
                return;
            }
        }

    }
    public bool InValidDateError(TextBox FromandTodate, Label FromandToErrorlbl, Label FromandToRequiredlbl)
    {
        lblErrorToDOS.Text = string.Empty;
        lblErrorFromDOS.Text = string.Empty;
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
    //protected void gvInstServiceLineDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    gvInstServiceLineDetails.EditIndex = -1;
    //    BindData();
    //    InstInfoAdd.Enabled = true;
    //    ClearInstPanelData();
    //}

    //protected void gvInstServiceLineDetails_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    gvInstServiceLineDetails.EditIndex = e.NewEditIndex;       

    //    int id = Convert.ToInt32(gvInstServiceLineDetails.Rows[e.NewEditIndex].Cells[0].Text);

    //    DataSet serviceLineDetails = new DataSet();
    //    List<SqlParameter> parameters = new List<SqlParameter>();
    //    parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
    //    parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, id, true));
    //    serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
    //    DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;

    //    if (claimsDataTable != null && claimsDataTable.Rows.Count > 0)
    //    {
    //        txtInsProcCode.Text = claimsDataTable.Rows[0]["Procedure_Code"].ToString();
    //        lblInstStatus.Text = InstitutionalStatus;
    //        txtInsFromDOS.Text = !string.IsNullOrEmpty(claimsDataTable.Rows[0]["Date_of_Service"].ToString()) ? Convert.ToDateTime(claimsDataTable.Rows[0]["Date_of_Service"]).ToString("MM/dd/yyyy") : "";
    //        txtInsToDOS.Text = !string.IsNullOrEmpty(claimsDataTable.Rows[0]["To_Date_of_Service"].ToString()) ? Convert.ToDateTime(claimsDataTable.Rows[0]["To_Date_of_Service"]).ToString("MM/dd/yyyy") : "";
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier1"].ToString()) && claimsDataTable.Rows[0]["Modifier1"].ToString() != "0")
    //            txtInsModifier1.Text = claimsDataTable.Rows[0]["Modifier1"].ToString();
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier2"].ToString()) && claimsDataTable.Rows[0]["Modifier2"].ToString() != "0")
    //            txtInsModifier2.Text = claimsDataTable.Rows[0]["Modifier2"].ToString();
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier3"].ToString()) && claimsDataTable.Rows[0]["Modifier3"].ToString() != "0")
    //            txtInsModifier3.Text = claimsDataTable.Rows[0]["Modifier3"].ToString();
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Modifier4"].ToString()) && claimsDataTable.Rows[0]["Modifier4"].ToString() != "0")
    //            txtInsModifier4.Text = claimsDataTable.Rows[0]["Modifier4"].ToString();
    //        txtInsTotalCharges.Text = claimsDataTable.Rows[0]["Total_Charges"].ToString();
    //        txtInsLineCntrlNumber.Text = claimsDataTable.Rows[0]["Line_Control_Number"].ToString();
    //        txtInsUnit.Text = claimsDataTable.Rows[0]["Billed_Units"].ToString();
    //        txtInstRevenueCode.Text = claimsDataTable.Rows[0]["Revenue_Code"].ToString();
    //        lblInstProcType.Text = "HCPCS";
    //        lblInsFinalEAPG.Text = claimsDataTable.Rows[0]["Final_EAPG"].ToString();
    //        ddlInsUnitsOfMeasurement.SelectedValue = claimsDataTable.Rows[0]["Unit_Of_Measurement"].ToString();
    //        lblInsPaymentAction.Text = claimsDataTable.Rows[0]["Payment_Action"].ToString();
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Non_Covered_Charges"].ToString()) && claimsDataTable.Rows[0]["Non_Covered_Charges"].ToString() != "0")
    //            tblInsNonCovCharges.Text = claimsDataTable.Rows[0]["Non_Covered_Charges"].ToString();
    //        if (!string.IsNullOrEmpty(claimsDataTable.Rows[0]["Paid_Amount"].ToString()) && claimsDataTable.Rows[0]["Paid_Amount"].ToString() != "0")
    //            lblInsPaidAmount.Text = claimsDataTable.Rows[0]["Paid_Amount"].ToString();
    //        lblInsTotalAmountPaid.Text = claimsDataTable.Rows[0]["Total_Amount_Paid"].ToString();
    //        lblInsTotalAmountBilled.Text = claimsDataTable.Rows[0]["Total_Amount_Billed"].ToString();
    //    }

    //    InstInfoAdd.Enabled = false;
    //    BindData();
    //    lblDetailsItemInst.Text = gvInstServiceLineDetails.Rows[e.NewEditIndex].Cells[0].Text;

    //}

    //protected void gvInstServiceLineDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    Dictionary<string, string> parms = new Dictionary<string, string>
    //        {
    //            { "Service_Line", gvInstServiceLineDetails.Rows[e.RowIndex].Cells[0].Text.ToString() },
    //            { "Claim_ID", Convert.ToString(ClaimId) }
    //        };

    //    svc.DeletePanelsDataWithParams("Claims_Service_Details", parms);

    //    BindData();
    //    ClearInstPanelData();
    //}

    private void ClearInstPanelData()
    {
        txtInstRevenueCode.Text = "";
        lblInstStatus.Text = InstitutionalStatus;
        lblInstProcType.Text = "HCPCS";
        txtInsFromDOS.Text = "";
        txtInsToDOS.Text = "";
        txtInsUnit.Text = "";
        ddlInsUnitsOfMeasurement.SelectedValue = "";
        txtInsTotalCharges.Text = "";
        // tblInsNonCovCharges.Text = "";
        txtInsProcCode.Text = "";
        txtInsLineCntrlNumber.Text = "";
        txtInsModifier1.Text = "";
        txtInsModifier2.Text = "";
        txtInsModifier3.Text = "";
        txtInsModifier4.Text = "";
        txtInstRevenueCode.Text = "";
    }

    //protected void gvInstServiceLineDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{

    //   // ValidateInstitutionalData();
    //    if (ValidateInstitutionalData() && ValidateClaimsInstitutionalServiceDetailUpdateDate())
    //    {
    //        int id = Convert.ToInt32(gvInstServiceLineDetails.Rows[e.RowIndex].Cells[0].Text);

    //        Dictionary<string, string> parms = new Dictionary<string, string>();

    //        List<SqlParameter> parameters = new List<SqlParameter>();
    //        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
    //        parameters.Add(SqlParms.CreateParameter("Service_Line", DbType.Int32, id, true));

    //        DataSet dsClaimServiceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLineDetails", parameters, "Claims_Service_Line_Details");
    //        if (dsClaimServiceLineDetails != null && Helper.HasRows(dsClaimServiceLineDetails) && Convert.ToInt32(dsClaimServiceLineDetails.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(ClaimId) && Convert.ToInt32(dsClaimServiceLineDetails.Tables[0].Rows[0]["Service_Line"]) == id)
    //        {
    //            parms.Add("Procedure_Code", txtInsProcCode.Text);
    //            parms.Add("Status", lblInstStatus.Text);
    //            parms.Add("Date_of_Service", Convert.ToDateTime(txtInsFromDOS.Text).ToString("MM/dd/yyyy"));
    //            if (!string.IsNullOrEmpty(txtInsToDOS.Text))
    //                parms.Add("To_Date_of_Service", Convert.ToDateTime(txtInsToDOS.Text).ToString("MM/dd/yyyy"));
    //            parms.Add("Modifier1", txtInsModifier1.Text);
    //            parms.Add("Modifier2", txtInsModifier2.Text);
    //            parms.Add("Modifier3", txtInsModifier3.Text);
    //            parms.Add("Modifier4", txtInsModifier4.Text);
    //            if (!string.IsNullOrEmpty(txtInsTotalCharges.Text))
    //                parms.Add("Total_Charges", txtInsTotalCharges.Text);
    //            parms.Add("Line_Control_Number", txtInsLineCntrlNumber.Text);
    //            if (!string.IsNullOrEmpty(txtInsUnit.Text))
    //                parms.Add("Billed_Units", txtInsUnit.Text);
    //            parms.Add("Revenue_Code", txtInstRevenueCode.Text);
    //            parms.Add("Procedure_Type", lblInstProcType.Text);
    //            parms.Add("Final_EAPG", lblInsFinalEAPG.Text);
    //            parms.Add("Unit_of_Measurement", ddlInsUnitsOfMeasurement.SelectedValue);
    //            parms.Add("Payment_Action", lblInsPaymentAction.Text);
    //            parms.Add("Non_Covered_Charges", tblInsNonCovCharges.Text);
    //            if (!string.IsNullOrEmpty(lblInsPaidAmount.Text))
    //                parms.Add("Paid_Amount", lblInsPaidAmount.Text);
    //            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
    //            if (!string.IsNullOrEmpty(lblInsTotalAmountPaid.Text))
    //            {

    //                if (lblInsTotalAmountPaid.Text.StartsWith("$"))
    //                {
    //                    lblInsTotalAmountPaid.Text = lblInsTotalAmountPaid.Text.Replace("$", "");
    //                }                    

    //                    parms.Add("Total_Amount_Paid", lblInsTotalAmountPaid.Text);


    //            }
    //            if (!string.IsNullOrEmpty(lblInsTotalAmountBilled.Text))
    //            {
    //                if (lblInsTotalAmountBilled.Text.StartsWith("$"))
    //                {
    //                    lblInsTotalAmountBilled.Text = lblInsTotalAmountBilled.Text.Replace("$", "");

    //                }                   

    //                    parms.Add("Total_Amount_Billed", lblInsTotalAmountBilled.Text);


    //            }
    //              //  parms.Add("Total_Amount_Billed", lblInsTotalAmountBilled.Text);
    //            parms.Add("Claims_Service_Details_ID", dsClaimServiceLineDetails.Tables[0].Rows[0]["Claims_Service_Details_ID"].ToString());
    //            svc.UpdatePanelsData("Claims_Service_Details", parms);
    //        }

    //        gvInstServiceLineDetails.EditIndex = -1;
    //        BindData();
    //        InstInfoAdd.Enabled = true;
    //        ClearInstPanelData();
    //    }
    //}
    public void CurrentServiceLineNo()
    {
        DataSet serviceLineDetails = new DataSet();
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
        serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
        DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        DataTable dtCurrentTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;
        if (dtCurrentTable != null && dtCurrentTable.Rows.Count > 999)
        {
            AddValidationErrorMessage("Max 999 Service Lines are allowed");
        }
        else
        {
            DataRow drCurrentRow = dtCurrentTable.NewRow();
            int latestServiceLine = dtCurrentTable.Rows.Count > 0 ? Convert.ToInt32((dtCurrentTable).Rows[(dtCurrentTable).Rows.Count - 1]["Service_Line"]) : 0;

            lblDetailsItemInst.Text = "0" + (latestServiceLine + 1);
        }
    }

    private void AddNewRowToGrid()
    {
        DataSet InstitutionalData = (DataSet)ViewState["CurrentTable"];
        DataTable dtCurrentTable = InstitutionalData != null ? InstitutionalData.Tables[0] : null;
        if (dtCurrentTable != null && dtCurrentTable.Rows.Count > 999)
        {
            AddValidationErrorMessage("Max 999 Service Lines are allowed");
        }

        else if (ViewState["CurrentTable"] != null)
        {
            DataRow drCurrentRow = dtCurrentTable.NewRow();
            // int latestServiceLine = dtCurrentTable.Rows.Count > 0 ? Convert.ToInt32((dtCurrentTable).Rows[(dtCurrentTable).Rows.Count - 1]["Service_Line"]) : 0;

            drCurrentRow["Service_Line"] = lblDetailsItemInst.Text;
            drCurrentRow["Revenue_code"] = txtInstRevenueCode.Text;
            drCurrentRow["Procedure_Type"] = lblInstProcType.Text;
            drCurrentRow["Procedure_code"] = txtInsProcCode.Text.ToUpper();
            drCurrentRow["Billed_Units"] = txtInsUnit.Text;
            drCurrentRow["Unit_Of_Measurement"] = ddlInsUnitsOfMeasurement.SelectedValue;
            drCurrentRow["Date_of_Service"] = txtInsFromDOS.Text;

            if (!string.IsNullOrEmpty(txtInsToDOS.Text))
            {
                drCurrentRow["To_Date_of_Service"] = txtInsToDOS.Text;
            }

            if (!string.IsNullOrEmpty(lblInsPaidAmount.Text))
            {
                drCurrentRow["Paid_Amount"] = lblInsPaidAmount.Text;
            }

            if (!string.IsNullOrEmpty(txtInsTotalCharges.Text))
            {
                drCurrentRow["Total_Charges"] = txtInsTotalCharges.Text;
            }

            drCurrentRow["Status"] = lblInstStatus.Text;


            dtCurrentTable.Rows.Add(drCurrentRow);

            DataSet InstiData = new DataSet();

            DataTable UpdateddtCopy = dtCurrentTable.Copy();

            InstiData.Tables.Add(UpdateddtCopy);

            ViewState["CurrentTable"] = InstiData;


            //gvInstServiceLineDetails.DataSource = dtCurrentTable;
            //gvInstServiceLineDetails.DataBind();
            if (!string.IsNullOrEmpty(lblInsTotalAmountPaid.Text))
            {
                if (lblInsTotalAmountPaid.Text.StartsWith("$"))
                {
                    lblInsTotalAmountPaid.Text = lblInsTotalAmountPaid.Text.Replace("$", "");

                }

            }
            if (!string.IsNullOrEmpty(lblInsTotalAmountBilled.Text))
            {
                if (lblInsTotalAmountBilled.Text.StartsWith("$"))
                {
                    lblInsTotalAmountBilled.Text = lblInsTotalAmountBilled.Text.Replace("$", "");

                }

            }

            Dictionary<string, string> parms = new Dictionary<string, string>
            {
                { "Service_Line", drCurrentRow["Service_Line"].ToString() },
                { "Procedure_Code", txtInsProcCode.Text != null? txtInsProcCode.Text.ToUpper() : txtInsProcCode.Text },
                { "Status", lblInstStatus.Text },
                { "Date_of_Service", txtInsFromDOS.Text },
                { "To_Date_of_Service", txtInsToDOS.Text },
                { "Modifier1", txtInsModifier1.Text.ToUpper() },
                { "Modifier2", txtInsModifier2.Text.ToUpper() },
                { "Modifier3", txtInsModifier3.Text.ToUpper() },
                { "Modifier4", txtInsModifier4.Text.ToUpper() },
                { "Total_Charges", txtInsTotalCharges.Text },
                { "Line_Control_Number", txtInsLineCntrlNumber.Text  },
                { "Billed_Units", txtInsUnit.Text },
                { "Revenue_Code", txtInstRevenueCode.Text },
                { "Claim_ID", Convert.ToString(ClaimId) },
                { "Procedure_Type", lblInstProcType.Text },
                { "Final_EAPG", lblInsFinalEAPG.Text },
                { "Unit_of_Measurement", ddlInsUnitsOfMeasurement.SelectedValue },
                { "Payment_Action", lblInsPaymentAction.Text },
                //{ "Non_Covered_Charges", !string.IsNullOrEmpty(tblInsNonCovCharges.Text) ? tblInsNonCovCharges.Text : "0" },
                { "Paid_Amount", !string.IsNullOrEmpty(lblInsPaidAmount.Text) ? lblInsPaidAmount.Text : "0"},
                { "Total_Amount_Paid", !string.IsNullOrEmpty(lblInsTotalAmountPaid.Text) ? lblInsTotalAmountPaid.Text : "0"},
                { "Total_Amount_Billed", !string.IsNullOrEmpty(lblInsTotalAmountBilled.Text) ? lblInsTotalAmountBilled.Text : "0"},
                { "Created_Date_Time", DateTime.Now.ToString() },
                { "Claim_Type", "1" }
    };

            svc.InsertPanelsData("Claims_Service_Details", parms);

            SetTotalAmountBilled(dtCurrentTable);
            CurrentServiceLineNo();
            ClearInstPanelData();
            BindData();
        }
    }

    private void SetTotalAmountBilled(DataTable dtCurrentTable)
    {
        decimal TotalAmountBilled = 0;

        foreach (DataRow row in dtCurrentTable.Rows)
        {
            decimal BilledAmount = !string.IsNullOrEmpty(row["Total_Charges"].ToString()) ? Convert.ToDecimal(row["Total_Charges"].ToString()) : 0;
            TotalAmountBilled += BilledAmount;
        }
        lblInsTotalAmountBilled.Text = TotalAmountBilled.ToString();
    }
    //protected void gvInstServiceLineDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
    //        {
    //            //e.Row.Cells[5].Visible = !DisplayReadOnly;
    //            if (DisplayReadOnly == true)
    //            {
    //                //ServiceLine1.Visible = !DisplayReadOnly;
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow && DisplayReadOnly)
    //        {

    //            //Button btEdit = (Button)e.Row.Cells[10].FindControl("btnEdit");
    //            //Button btDelete = (Button)e.Row.Cells[11].FindControl("btnDelete");

    //            //if (Convert.ToInt32(e.Row.Cells[0].Text) <= 10)
    //            //{
    //            //    e.Row.Cells[0].Text = "0" + e.Row.Cells[0].Text;
    //            //}
    //           // gvInstServiceLineDetails.Columns[10].Visible = !DisplayReadOnly;
    //            gvInstServiceLineDetails.Columns[11].Visible = !DisplayReadOnly ;
    //            if (DisplayReadOnly == true)
    //            {
    //                // ServiceLine1.Visible = !DisplayReadOnly;
    //            }
    //            if (DisplayReadOnly == true)
    //            {
    //                //ServiceLine1.Visible = false;
    //               // gvInstServiceLineDetails.Columns[10].Visible = false;
    //                gvInstServiceLineDetails.Columns[11].Visible = false;
    //                //ServiceLine1.Visible = false;
    //            }
    //            else
    //            {
    //                //ServiceLine1.Visible = true;
    //               // gvInstServiceLineDetails.Columns[10].Visible = true;
    //                gvInstServiceLineDetails.Columns[11].Visible = true;
    //                // ServiceLine1.Visible = true;
    //            }
    //        }
    //        else if(e.Row.RowType == DataControlRowType.DataRow) 
    //            {

    //           // gvInstServiceLineDetails.Columns[10].Visible = true;
    //            gvInstServiceLineDetails.Columns[11].Visible = true;
    //            //Button btEdit = (Button)e.Row.Cells[10].FindControl("btnEdit");
    //            //Button btDelete = (Button)e.Row.Cells[11].FindControl("btnDelete");
    //            //if (btEdit != null)
    //            //    btEdit.Visible = !DisplayReadOnly;
    //            //if (btDelete != null)
    //            //    btDelete.Visible = !DisplayReadOnly;
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            if (Convert.ToInt32(e.Row.Cells[0].Text) <= 10)
    //            {
    //                e.Row.Cells[0].Text = "0" + e.Row.Cells[0].Text;
    //            }
    //            //ohpnm- 7413 Paid amount- Do not display paid amount as 0.00 before submission of the claim. Display blank.           
    //            e.Row.Cells[9].Text = e.Row.Cells[9].Text == "0" ? "" : e.Row.Cells[9].Text;
    //            foreach (Control c in e.Row.Cells[11].Controls)
    //            {
    //                LinkButton button = c as LinkButton;
    //                if (button != null)
    //                {
    //                    if (button.Text == "Delete" || button.Text == "Cancel")
    //                    {
    //                        //button.CssClass = "btn btn-danger";
    //                        button.CssClass = "i btn-danger";
    //                        button.ControlStyle.CssClass = "btn btn-danger";
    //                    }
    //                    else if (button.Text == "Edit" || button.Text == "Update")
    //                    {
    //                        button.CssClass = "btn btn-primary";
    //                        button.ControlStyle.CssClass = "btn btn-primary";
    //                    }
    //                }
    //            }
    //        }
    //        //if (DisplayReadOnly)
    //        //{
    //        //    ServiceLine1.Visible = false;
    //        //}else
    //        //{
    //        //    ServiceLine1.Visible = true;
    //        //}

    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = msg,
            Text = msg,
            SetFocusOnError = true,
            ValidationGroup = "valInstServiceLineInfo"
        };
        this.Page.Validators.Add(val);
        return false;
    }

    private bool ValidateInstitutionalData()
    {
        bool isValid = true;

        if (string.IsNullOrEmpty(txtInsProcCode.Text) && (!string.IsNullOrEmpty(txtInsModifier1.Text) || !string.IsNullOrEmpty(txtInsModifier2.Text) || !string.IsNullOrEmpty(txtInsModifier3.Text) || !string.IsNullOrEmpty(txtInsModifier4.Text)))
        {
            AddValidationErrorMessage("*Modifiers can only be entered if procedure code is reported");
            isValid = false;
        }

        if (!string.IsNullOrEmpty(txtInsFromDOS.Text) && !string.IsNullOrEmpty(txtInsToDOS.Text) && Convert.ToDateTime(txtInsFromDOS.Text) > Convert.ToDateTime(txtInsToDOS.Text))
        {
            AddValidationErrorMessage("*From date cannot be greater than To date");
            isValid = false;
        }

        if (!string.IsNullOrEmpty(txtInsFromDOS.Text) && Convert.ToDateTime(txtInsFromDOS.Text) > DateTime.Now)
        {
            AddValidationErrorMessage("*From date cannot be future date");
            isValid = false;
        }

        if (!string.IsNullOrEmpty(txtInsToDOS.Text) && Convert.ToDateTime(txtInsToDOS.Text) > DateTime.Now)
        {
            AddValidationErrorMessage("*To date cannot be future date");
            isValid = false;
        }

        if (Convert.ToDecimal(txtInsUnit.Text) <= 0)
        {
            AddValidationErrorMessage("*Unit should be greater than zero");
            isValid = false;
        }
        if (string.IsNullOrEmpty(ddlInsUnitsOfMeasurement.SelectedValue))
        {
            AddValidationErrorMessage("*Unit of Measurement is required");
            isValid = false;
        }
        //&& string.IsNullOrEmpty(tblInsNonCovCharges.Text
        if (string.IsNullOrEmpty(txtInsTotalCharges.Text))
        {
            AddValidationErrorMessage("*Either the Total Charges Amount  must be greater than $0.00");
            isValid = false;
        }
        //if (!string.IsNullOrEmpty(tblInsNonCovCharges.Text) && !string.IsNullOrEmpty(txtInsTotalCharges.Text) && Convert.ToDecimal(tblInsNonCovCharges.Text) > Convert.ToDecimal(txtInsTotalCharges.Text))
        //{
        //    AddValidationErrorMessage("*Non covered charges cannot be greater than total charges.");
        //    isValid = false;
        //}

        if (!string.IsNullOrEmpty(txtInstRevenueCode.Text))
        {
            bool revCodeExists = ClaimsController.IsValidRevenueCode(txtInstRevenueCode.Text.Trim());

            if (!revCodeExists)
            {
                AddValidationErrorMessage("*Revenue code is invalid.");
                txtInstRevenueCode.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtInsProcCode.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureCode(txtInsProcCode.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure code is invalid.");
                txtInsProcCode.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtInsModifier1.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtInsModifier1.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier1 is invalid.");
                txtInsModifier1.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtInsModifier2.Text))
        {

            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtInsModifier2.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier2 is invalid.");
                txtInsModifier2.Text = string.Empty;
                isValid = false;
            }
        }

        if (!string.IsNullOrEmpty(txtInsModifier3.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtInsModifier3.Text.Trim());
            if (!procCodeExists)
                if (!procCodeExists)
                {
                    AddValidationErrorMessage("*Procedure modifier3 is invalid.");
                    txtInsModifier3.Text = string.Empty;
                    isValid = false;
                }
        }

        if (!string.IsNullOrEmpty(txtInsModifier4.Text))
        {
            bool procCodeExists = ClaimsController.IsValidProcedureModifier(txtInsModifier4.Text.Trim());
            if (!procCodeExists)
            {
                AddValidationErrorMessage("*Procedure modifier4 is invalid.");
                txtInsModifier4.Text = string.Empty;
                isValid = false;
            }
        }

        return isValid;
    }

    //protected void gvInstServiceLineDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    gvInstServiceLineDetails.PageIndex = e.NewPageIndex;
    //    BindData();
    //}

    public bool ValidateClaimsInstitutionalServiceDetailDate()
    {
        bool returnValue = true;

        if ((string.IsNullOrEmpty(ServiceInfoDetailofServiceofFromDate)) && (string.IsNullOrEmpty(ServiceInfoDetailofServiceofToDate)))
        {

            AddValidationErrorMessage("Please enter From Date and To Date in the service information panel before entering service details.");
            return returnValue = false;

        }
        if (ServiceInfoDetailofServiceofFromDate != "")
        {
            if ((Convert.ToDateTime((txtInsFromDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)))
            {
                if ((Convert.ToDateTime((txtInsFromDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) && (Convert.ToDateTime((txtInsFromDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)))
                {

                    AddValidationErrorMessage("From DOS should be reported be within the statement period");
                    returnValue = false;
                }
            }
            if ((Convert.ToDateTime((txtInsFromDOS.Text)) < Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)) && Convert.ToDateTime((txtInsFromDOS.Text)) < Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)
                 && (Convert.ToDateTime((txtInsFromDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) && (Convert.ToDateTime((txtInsFromDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)))
            {
                AddValidationErrorMessage("From DOS should be reported be within the statement period");
                returnValue = false;
            }
        }
        if (!string.IsNullOrEmpty(txtInsToDOS.Text))
        {
            if ((!string.IsNullOrEmpty(ServiceInfoDetailofServiceofToDate))
                && ((Convert.ToDateTime((txtInsToDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) &&
              Convert.ToDateTime((txtInsToDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)
                ))

            {
                if ((!string.IsNullOrEmpty(ServiceInfoDetailofServiceofToDate)) && (Convert.ToDateTime((txtInsToDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) &&
                    (Convert.ToDateTime((txtInsToDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)))
                {
                    AddValidationErrorMessage("To DOS should be reported be within the statement period");
                    returnValue = false;
                }
            }

        }
        if (!string.IsNullOrEmpty(txtInsFromDOS.Text))
        {
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
            DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;

            if (claimsDataTable != null && claimsDataTable.Rows.Count > 0)
            {
                DataView dv = claimsDataTable.DefaultView;
                dv.Sort = "Service_Line ASC";
                DataTable dt = dv.ToTable();
                string fromDategv = !string.IsNullOrEmpty(dt.Rows[0]["Date_of_Service"].ToString()) ? Convert.ToDateTime(dt.Rows[0]["Date_of_Service"]).ToString("MM/dd/yyyy") : "";
                if (!(Convert.ToDateTime(fromDategv) <= Convert.ToDateTime(txtInsFromDOS.Text)))
                {
                    AddValidationErrorMessage("Revenue code must be submitted in the ascending order of date of service");
                    return returnValue = false;

                }
            }

        }
        return returnValue;
    }
    public bool ValidateClaimsInstitutionalServiceDetailUpdateDate()
    {
        bool returnValue = true;
        if (ServiceInfoDetailofServiceofFromDate != "")
        {
            if ((Convert.ToDateTime((txtInsFromDOS.Text)) < Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)))
            {
                if ((Convert.ToDateTime((txtInsFromDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) && (Convert.ToDateTime((txtInsFromDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)))
                {

                    AddValidationErrorMessage("From DOS should be reported be within the statement period");
                    returnValue = false;
                }
            }
        }
        if (!string.IsNullOrEmpty(txtInsToDOS.Text))
        {
            if ((!string.IsNullOrEmpty(ServiceInfoDetailofServiceofToDate))
                && ((Convert.ToDateTime((txtInsToDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) &&
              Convert.ToDateTime((txtInsToDOS.Text)) > Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)
                ))

            {
                if ((!string.IsNullOrEmpty(ServiceInfoDetailofServiceofToDate)) && (Convert.ToDateTime((txtInsToDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofToDate)) &&
                   (Convert.ToDateTime((txtInsToDOS.Text)) != Convert.ToDateTime(ServiceInfoDetailofServiceofFromDate)))
                {
                    AddValidationErrorMessage("To DOS should be reported be within the statement period");
                    returnValue = false;
                }
            }

        }

        if (!string.IsNullOrEmpty(txtInsFromDOS.Text))
        {
            DataSet serviceLineDetails = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, true));
            serviceLineDetails = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Professional_ServiceLine", parameters, "Claims_Service_Details");
            DataTable claimsDataTable = serviceLineDetails != null ? serviceLineDetails.Tables[0] : null;

            if (claimsDataTable != null && claimsDataTable.Rows.Count > 0)
            {
                DataView dv = claimsDataTable.DefaultView;
                dv.Sort = "Service_Line ASC";
                DataTable dt = dv.ToTable();
                string fromDategv = !string.IsNullOrEmpty(dt.Rows[0]["Date_of_Service"].ToString()) ? Convert.ToDateTime(dt.Rows[0]["Date_of_Service"]).ToString("MM/dd/yyyy") : "";
                if (!(Convert.ToDateTime(fromDategv) <= Convert.ToDateTime(txtInsFromDOS.Text)))
                {
                    AddValidationErrorMessage("Revenue code must be submitted in the ascending order of date of service");
                    return returnValue = false;

                }
            }

        }
        return returnValue;
    }

    protected void InstInfoAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid && ValidateInstitutionalData() && ValidateClaimsInstitutionalServiceDetailDate())
        {
            AddNewRowToGrid();
        }

    }

    protected void lnkInsProcCode_Click(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchProc.Show();
    }

    protected void lnkOPNPI3_Click(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchRevenueCode.Show();
    }

    protected void btnCloseProc_Click(object sender, EventArgs e)
    {
        ucSubmitClaimSearchProc.CleareField();
    }

    protected void btnCloseRevCode_Click(object sender, EventArgs e)
    {
        ucSubmitClaimSearchRevenueCode.CleareField();
    }

    public void ClearFields()
    {
        ucSubmitClaimSearchProc.ProcedureCode = string.Empty;
        ucSubmitClaimSearchRevenueCode.RevenueCode = string.Empty;
        lblInsTotalAmountBilled.Text = String.Empty;
        lblInsTotalAmountPaid.Text = String.Empty;
        txtInstRevenueCode.Text = String.Empty;
        txtInsFromDOS.Text = String.Empty;
        lblInstStatus.Text = String.Empty;
        txtInsToDOS.Text = String.Empty;
        txtInsUnit.Text = String.Empty;
        txtInsProcCode.Text = String.Empty;
        lblInsFinalEAPG.Text = String.Empty;
        ddlInsUnitsOfMeasurement.ClearSelection();
        txtInsModifier1.Text = String.Empty;
        txtInsModifier2.Text = String.Empty;
        txtInsModifier3.Text = String.Empty;
        txtInsModifier4.Text = String.Empty;
        lblInsPaymentAction.Text = String.Empty;
        txtInsTotalCharges.Text = String.Empty;
        txtInsLineCntrlNumber.Text = String.Empty;
        // tblInsNonCovCharges.Text = String.Empty;
        lblInsPaidAmount.Text = String.Empty;
        lblInsTotalAmountBilled.Text = string.Empty; lblInsTotalAmountPaid.Text = string.Empty;
        serviceDetailInsti.InnerHtml = "";
        ucSubmitClaimSearchProc.CleareField();
        ucSubmitClaimSearchRevenueCode.CleareField();
    }

    public void ClearGrid()
    {
        // gvInstServiceLineDetails.DataSource = null;
        // gvInstServiceLineDetails.DataBind();
        lblDetailsItemInst.Text = "01";
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
    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (InstitutionalStatus.ToUpper() == "PAID" && IsClaimCopy)
        {
            hdnHidePaidAmounts.Value = "1";
        }
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string dateService = null;
                string ToDateService = null;
                if (!string.IsNullOrEmpty(dt.Rows[i]["Date_of_Service"].ToString()))
                {
                    //dateService = DateTime.ParseExact(dt.Rows[i]["Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    if (checkDateFormat(dt.Rows[i]["Date_of_Service"].ToString()))
                    {
                        dateService = dt.Rows[i]["Date_of_Service"].ToString();
                    }
                    else
                    {
                        dateService = DateTime.ParseExact(dt.Rows[i]["Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                }
                if (!string.IsNullOrEmpty(dt.Rows[i]["To_Date_of_Service"].ToString()))
                {
                    if (checkDateFormat(dt.Rows[i]["To_Date_of_Service"].ToString()))
                    {
                        ToDateService = dt.Rows[i]["To_Date_of_Service"].ToString();
                    }
                    else
                    {
                        ToDateService = DateTime.ParseExact(dt.Rows[i]["To_Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                    }
                    //ToDateService = DateTime.ParseExact(dt.Rows[i]["To_Date_of_Service"].ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                }


                //drCurrentRow["Service_Line"] = lblDetailsItemInst.Text;
                //drCurrentRow["Revenue_code"] = txtInstRevenueCode.Text;
                //drCurrentRow["Procedure_Type"] = lblInstProcType.Text;
                //drCurrentRow["Procedure_code"] = txtInsProcCode.Text;
                //drCurrentRow["Billed_Units"] = txtInsUnit.Text;
                //drCurrentRow["Unit_Of_Measurement"] = ddlInsUnitsOfMeasurement.SelectedValue;
                //drCurrentRow["Date_of_Service"] = txtInsFromDOS.Text;

                //if (!string.IsNullOrEmpty(txtInsToDOS.Text))
                //{
                //    drCurrentRow["To_Date_of_Service"] = txtInsToDOS.Text;
                //}

                //if (!string.IsNullOrEmpty(lblInsPaidAmount.Text))
                //{
                //    drCurrentRow["Paid_Amount"] = lblInsPaidAmount.Text;
                //}

                //if (!string.IsNullOrEmpty(txtInsTotalCharges.Text))
                //{
                //    drCurrentRow["Total_Charges"] = txtInsTotalCharges.Text;
                //}

                //drCurrentRow["Status"] = lblInstStatus.Text;
                string lblserviceline = "";
                int serviceline = Convert.ToInt32(dt.Rows[i]["Service_Line"].ToString());
                lblserviceline = serviceline.ToString();
                Dictionary<string, string> parms = new Dictionary<string, string>
            {
                { "Service_Line", lblserviceline },
                { "Procedure_Code", dt.Rows[i]["Procedure_code"].ToString().ToUpper()},
                { "Date_of_Service",dateService },
                { "To_Date_of_Service", ToDateService },
                //{ "Modifier1", dt.Rows[i]["Modifier1"].ToString()},
                //{ "Modifier2", dt.Rows[i]["Modifier2"].ToString() },
                //{ "Modifier3", dt.Rows[i]["Modifier3"].ToString() },
                //{ "Modifier4",  dt.Rows[i]["Modifier4"].ToString() },
                { "Total_Charges", dt.Rows[i]["Total_Charges"].ToString() },
                //{ "Line_Control_Number", dt.Rows[i]["Line_Control_Number"].ToString()  },
                { "Billed_Units", dt.Rows[i]["Billed_Units"].ToString()},
                { "Revenue_Code", dt.Rows[i]["Revenue_Code"].ToString()},
                { "Claim_ID", Convert.ToString(ClaimId) },
                { "Procedure_Type", dt.Rows[i]["Procedure_Type"].ToString() },
                { "Unit_of_Measurement", dt.Rows[i]["Unit_Of_Measurement"].ToString() },
               // { "Non_Covered_Charges", !string.IsNullOrEmpty(dt.Rows[i]["Non_Covered_Charges"].ToString()) ? dt.Rows[i]["Non_Covered_Charges"].ToString() : "0" },
                { "Paid_Amount", !string.IsNullOrEmpty(dt.Rows[i]["Paid_Amount"].ToString())  &&  hdnHidePaidAmounts.Value != "1" ? dt.Rows[i]["Paid_Amount"].ToString() : "0"},
                { "Created_Date_Time", DateTime.Now.ToString() },
                { "Claim_Type", "1" },
                { "Clear_Prior_Data", "1" }

            };

                svc.InsertPanelsData("Claims_Service_Details", parms);
            }

            SetServiceDetails(null);

        }
    }
}
