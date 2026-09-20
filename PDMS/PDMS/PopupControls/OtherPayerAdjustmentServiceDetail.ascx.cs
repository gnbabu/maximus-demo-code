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

public partial class PopupControls_OtherPayerAdjustmentServiceDetail : System.Web.UI.UserControl
{
    private bool fromInquirySvc = false;
    #region SVC
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
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOPAdjustmentClaimID.Value))
                return hdnOPAdjustmentClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOPAdjustmentClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOPAdjustmentClaimType.Value))
                return hdnOPAdjustmentClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOPAdjustmentClaimType.Value = value.Trim();
            SetVisibleField(value);
        }
    }

    private DataSet dsOtherPayerAdjustmentInfo = new DataSet();

    public DataSet datasetOtherPayerAdjustmentInfo
    {
        get
        {
            if (!Helper.HasRows(dsOtherPayerAdjustmentInfo))
            {
                if (Helper.HasRows(GetOtherPayerAdjustmentInfo()))
                {
                    return dsOtherPayerAdjustmentInfo;
                }
            }
            return dsOtherPayerAdjustmentInfo;
        }
        set
        {
            if (value != null)
            {
                BindOtherPayerAdjustmentInfoGrid(value);
            }
        }
    }

    public string ICNNumber = string.Empty;
    public string ICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(ICNNumber))
                return ICNNumber;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                ICNNumber = value.Trim();
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
            if (!FromInquirySvc)
            {
                BindOtherPayerAdjustmentInfoGrid(null);
            }
        }
    }
    #endregion

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!(ddlServiceLine.SelectedIndex > 0))
        {
            GetServiceLineNoFromServiceDetailPanel();
        }
        if (ddlOPPAdjustmentHealthPlanID.SelectedIndex <= 0)
        {
            GetHealthPlanIDForOPAdjustmentServiceDetail();
        }
        if (Session["ReasonCode"] != null)
        {
            if (Session["ReasonCode"].ToString().Trim() != null && String.IsNullOrEmpty(hdnAddedReasonCodetoGrid.Value))
            {
                txtOtherPayerReasonCode.Text = Session["ReasonCode"].ToString().Trim();
            }
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetServiceLineNoFromServiceDetailPanel();
            GetHealthPlanIDForOPAdjustmentServiceDetail();
            GetAdjustmentGroupOPP();
            if (!FromInquirySvc)
            {
                BindOtherPayerAdjustmentInfoGrid(null);
            }
        }
        Session["ReasonCode"] = string.Empty;
        //btnOPPAdjustmentAdd.Attributes.Add("onclick", "OtherDisableEnableConditionAddButton();");

        if((hdnReasonCode12.Value != null) || (hdnReasonCode12.Value != ""))
        {
            txtOtherPayerReasonCode.Text = hdnReasonCode12.Value;
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
    protected void SetVisibleField(string claimtype)
    {
        if (claimtype == CON.ClaimsType.Institutional)
        {
            divAdjRevenueCode.Visible = true;
            HeaderOPARevenue.Visible = true;
            //foreach (DataControlField col in gvOPPAdjustmentInfo.Columns)
            //{
            //    if (col.HeaderText == "Revenue Code")
            //    {
            //        col.Visible = true;
            //    }
            //}
        }
        else
        {
            divAdjRevenueCode.Visible = false;
            HeaderOPARevenue.Visible = false;
            //foreach (DataControlField col in gvOPPAdjustmentInfo.Columns)
            //{
            //    if (col.HeaderText == "Revenue Code")
            //    {
            //        col.Visible = false;
            //    }
            //}
        }
    }

    private DataSet GetServiceLineNoFromServiceDetailPanel()
    {
        ddlServiceLine.Items.Clear();
        List<SqlParameter> param = new List<SqlParameter>();
        DataSet dsServiceLine = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
        {
            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnOPAdjustmentClaimID.Value, true));
            dsServiceLine = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
            if (Helper.HasRows(dsServiceLine) &&
                Convert.ToInt32(dsServiceLine.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOPAdjustmentClaimID.Value))
            {
                DataTable dt = dsServiceLine.Tables[0];
                Helper.LoadList(ddlServiceLine, dt, "Service_Line", "Service_Line", true);
            }
        }
        return dsServiceLine;
    }

    private DataTable GetHealthPlanIDForOPAdjustmentServiceDetail()
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        DataSet dsHealthPlanID = new DataSet();
        DataTable dthealthplanid = new DataTable();
        if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
        {
            param.Add("Claim_ID", hdnOPAdjustmentClaimID.Value.ToString());
            dsHealthPlanID = svc.SelectPanelsData("claims_other_payer_information", param);

            if (Helper.HasRows(dsHealthPlanID) && Convert.ToInt32(dsHealthPlanID.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOPAdjustmentClaimID.Value))
            {
                DataTable dt = dsHealthPlanID.Tables[0];
                IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                    where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Details) &&
                                                    row.Field<string>("Claim_Adjudication_Level") != null)
                                                    select row;
                if (healthplanid.Count() > 0)
                {
                    dthealthplanid = healthplanid.CopyToDataTable();
                    Helper.LoadList(ddlOPPAdjustmentHealthPlanID, dthealthplanid, "Health_Plan_ID", "Claims_Other_Payer_Information_ID", true);
                }
            }
        }
        return dthealthplanid;
    }

    private DataTable GetAdjustmentGroupOPP()
    {
        DataSet dataSet = svc.GetAdjustmentGroup();
        DataTable dtAdjustGroup = dataSet.Tables[0];
        if (Helper.HasRows(dtAdjustGroup))
        {
            Helper.LoadList(ddlOPPAdjustmentGroup, dtAdjustGroup, "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC", "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID", true);
        }
        return dtAdjustGroup;
    }

    public void BindOtherPayerAdjustmentInfoGrid(DataSet dsOPAInfo)
    {
        hdnOtherPayerAdjustmentClaimStatus.Value = "";
        DataSet dsOtherPayerAdjInfo = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value) || !string.IsNullOrEmpty(ICN))
        {
            if (Helper.HasRows(dsOPAInfo))
            {
                dsOtherPayerAdjInfo = dsOPAInfo;
            }
            else
            {
                dsOtherPayerAdjInfo = GetOtherPayerAdjustmentInfo();
            }
        }

        if (Helper.HasRows(dsOtherPayerAdjInfo))
        {
            DataTable dtOtherPayerAdjInfo = dsOtherPayerAdjInfo.Tables[0];
            if (dtOtherPayerAdjInfo.Rows.Count > 0)         {

                string table = string.Empty;
                if (ClaimType == Convert.ToString(CON.ClaimType.INSTITUTIONAL))
                {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Revenue Code</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                }
                else
                {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px;' scope='col'>Service Line</th><th style='width:10px;' scope='col'>Procedure Code</th><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:30px; scope='col'>Reason Code</th><th style='width:30px; scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";
                }
                foreach (DataRow dr in dtOtherPayerAdjInfo.Rows)
                {
                    string serviceline = dr["Service_Line"].ToString();
                    string Procedure_Code = dr["Procedure_Code"].ToString();
                    string Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                    string Adjustment_Group = dr["Adjustment_Group"].ToString();
                    string Reason_Code = dr["Reason_Code"].ToString();
                    string Other_Payer_Adjustment_Service_Detail_ID = dr["Other_Payer_Adjustment_Service_Detail_ID"].ToString();
                    string Claim_ID = dr["Claim_ID"].ToString();
                    string Amount = dr["Amount"].ToString();
                    string Quantity = dr["Quantity"].ToString();
                    if (ClaimType == Convert.ToString(CON.ClaimType.INSTITUTIONAL))
                    {
                        string Revenue_Code = dr["Revenue_Code"].ToString();
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Revenue_Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                hdnOtherPayerAdjustmentClaimStatus.Value = "Pending Submission";
                            }
                            else
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span title='Revenue_Code' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quantity' class='tNumber'>" + Quantity + "</span></td></tr >";
                                hdnOtherPayerAdjustmentClaimStatus.Value = "Other";
                            }
                        }
                    }
                    else
                    {
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + Claim_ID + "\",\"" + ClaimType + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOtherPayerAdjustmentInfo(\"" + Other_Payer_Adjustment_Service_Detail_ID + "\",\"" + ClaimType + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                                hdnOtherPayerAdjustmentClaimStatus.Value = "Pending Submission";
                            }
                            else
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Service Line' class='tNumber'>" + serviceline + "</span></td><td><span  title='Procedure Code' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Helath Plan ID' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Adjustment Group' class='tNumber'>" + Adjustment_Group + "</span></td><td><span title='Reason Code' class='tNumber'>" + Reason_Code + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Quntity' class='tNumber'>" + Quantity + "</span></td></tr >";
                                hdnOtherPayerAdjustmentClaimStatus.Value = "Other";
                            }
                        }
                    }
                }
                table = table + "</tbody></table>";
                divOtherPayerAdjustment.InnerHtml = table;
            }
        }
        else
        {
            divOtherPayerAdjustment.InnerHtml = "";
        }
        if (DisplayReadOnly == true)
        {
            divOtherPayer.Visible = false;
        }
        else
        {
            divOtherPayer.Visible = true;

        }
    }

    private DataSet GetOtherPayerAdjustmentInfo()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsOPAdjustmentInfo = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
        {
            parms.Add("Claim_ID", hdnOPAdjustmentClaimID.Value);
            dsOtherPayerAdjustmentInfo = dsOPAdjustmentInfo = svc.SelectPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);
        }
        return dsOPAdjustmentInfo;
    }

    public void ClearOtherPayerAdjustmentPanel()
    {
        GetServiceLineNoFromServiceDetailPanel();
        GetHealthPlanIDForOPAdjustmentServiceDetail();
        GetAdjustmentGroupOPP();
        //GetReasonCodesOPP();
        txtOtherPayerReasonCode.Text = string.Empty;
        lblAdjProc_Code.Text = string.Empty;
        txtOPPAdjustmentAmount.Text = string.Empty;
        txtOPPAdjustmentQuantity.Text = string.Empty;
        lblAdjRevenueCode.Text = string.Empty;
        txtOPPAdjustmentAmount.Text = "";
        lblAdjProc_Code.Text = "";
        hdnReasonCode12.Value = "";
        divOtherPayerAdjustment.InnerHtml = "";
        hdnOPAdjustmentClaimID.Value = "";

    }

    public void ClearOtherPayerAdjustmentGrid()
    {
        //gvOPPAdjustmentInfo.DataSource = null;
        //gvOPPAdjustmentInfo.DataBind();
        divOtherPayerAdjustment.InnerHtml = "";
    }
    protected void ddlServiceLine_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
        {
            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnOPAdjustmentClaimID.Value, true));
            DataSet dsServiceLineNo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
            if (Helper.HasRows(dsServiceLineNo))
            {
                DataTable dt = dsServiceLineNo.Tables[0];
                IEnumerable<DataRow> dtDetails = from row in dt.AsEnumerable()
                                                 where row.Field<int>("Service_Line") == Convert.ToInt32(ddlServiceLine.SelectedItem.Text)
                                                 select row;
                if (dtDetails.Any())
                {
                    DataTable dtServiceLine = dtDetails.CopyToDataTable();
                    lblAdjProc_Code.Text = dtServiceLine.Rows[0]["cde_proc"].ToString();
                    if (ClaimType == CON.ClaimsType.Institutional)
                    {
                        lblAdjRevenueCode.Text = dtServiceLine.Rows[0]["Revenue_Code"].ToString();
                    }
                }
            }
        }
    }

    protected void btnOPPAdjustmentAdd_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            lblErrorMessage.Text = string.Empty;
            DataSet Otherpayer = new DataSet();
            Otherpayer = datasetOtherPayerAdjustmentInfo;
            DataTable dt = Otherpayer.Tables[0];
            IEnumerable<DataRow> dtDetails = from row in dt.AsEnumerable()
                                             where row.Field<string>("Reason_Code") == txtOtherPayerReasonCode.Text.ToString().Trim()
                                             && row.Field<string>("Adjustment_Group") == ddlOPPAdjustmentGroup.SelectedItem.ToString()
                                             && row.Field<string>("Health_Plan_ID") == ddlOPPAdjustmentHealthPlanID.SelectedItem.ToString()
                                             && row.Field<int>("Service_Line") == Convert.ToInt32(ddlServiceLine.SelectedItem.ToString())
                                             select row;
            if (dtDetails.Any())
            {
                lblErrorMessage.Text = "Same reason code cannot be reported multiple times with same adjustment group for the same payer for same service line. ";
                return;
            }
            else
            {
                IEnumerable<DataRow> duplicateRecords = from row in dt.AsEnumerable()
                                                        where row.Field<string>("Adjustment_Group") == ddlOPPAdjustmentGroup.SelectedItem.ToString()
                                                         && row.Field<string>("Health_Plan_ID") == ddlOPPAdjustmentHealthPlanID.SelectedItem.Text.Trim()
                                                         && row.Field<int>("Service_Line") == Convert.ToInt32(ddlServiceLine.SelectedItem.ToString())
                                                        select row;
                if (duplicateRecords.Any() && duplicateRecords.Count() == 6)
                {
                    lblErrorMessage.Text = "Each adjustment group can be repeated up to 6 times for one other payer";
                    return;
                }
                IEnumerable<DataRow> duplicateadjustment = from row in dt.AsEnumerable()
                                                           where row.Field<string>("Adjustment_Group") == ddlOPPAdjustmentGroup.SelectedItem.ToString()
                                                            && row.Field<string>("Health_Plan_ID") == ddlOPPAdjustmentHealthPlanID.SelectedItem.Text.Trim()
                                                           select row;
                if (duplicateadjustment.Any() && duplicateadjustment.Count() == 30)
                {
                    lblErrorMessage.Text = "One payer can be reported up to 30 adjustment lines.";
                    return;
                }
            }

            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Service_Line", ddlServiceLine.SelectedItem.Text);
            parms.Add("Procedure_Code", lblAdjProc_Code.Text);
            parms.Add("Health_Plan_ID", ddlOPPAdjustmentHealthPlanID.SelectedItem.Text);
            parms.Add("Adjustment_Group", ddlOPPAdjustmentGroup.SelectedItem.Text);
            parms.Add("Reason_Code", txtOtherPayerReasonCode.Text.Trim());
            parms.Add("Amount", txtOPPAdjustmentAmount.Text);
            parms.Add("Quantity", txtOPPAdjustmentQuantity.Text);

            if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
            {
                if (ClaimType == CON.ClaimsType.Institutional)
                {
                    parms.Add("Revenue_Code", lblAdjRevenueCode.Text);
                }
                else
                {
                    parms.Add("Revenue_Code", null);
                }

                parms.Add("Claim_ID", hdnOPAdjustmentClaimID.Value);
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);
                BindOtherPayerAdjustmentInfoGrid(null);
                ClearOtherPayerAdjustmentPanel();
            }
        }
    }

    protected void gvOPPAdjustmentInfo_RowEditing(object sender, GridViewEditEventArgs e)
    {
        // gvOPPAdjustmentInfo.EditIndex = e.NewEditIndex;
        lblErrorMessage.Text = "";
        lblOtherPayerAdjustmenterrormsg.Text = "";
        BindOtherPayerAdjustmentInfoGrid(null);
        GetHealthPlanIDForOPAdjustmentServiceDetail();
        ClearOtherPayerAdjustmentPanel();
    }

    protected void gvOPPAdjustmentInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //  gvOPPAdjustmentInfo.EditIndex = -1;
        BindOtherPayerAdjustmentInfoGrid(null);
        lblErrorMessage.Text = "";
        lblOtherPayerAdjustmenterrormsg.Text = "";
    }

    protected void gvOPPAdjustmentInfo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowState & DataControlRowState.Edit) > 0)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            DropDownList ddlServiceLine = (DropDownList)e.Row.FindControl("ddlServiceLine");
            DropDownList ddlOPPHealhPlanId = (DropDownList)e.Row.FindControl("ddlOPPAdjustmentHealthPlanID");
            DropDownList ddlOPPAdjustmentGroup = (DropDownList)e.Row.FindControl("ddlOPPAdjustmentGroup");
            TextBox txtHeaderreasonCode = (TextBox)e.Row.FindControl("txtHeaderreasonCode");
            hdnAddedReasonCodetoGrid.Value = txtHeaderreasonCode.Text.Trim();
            if (ddlServiceLine != null)
            {
                ddlServiceLine.DataSource = GetServiceLineNoFromServiceDetailPanel();
                ddlServiceLine.DataTextField = "Service_Line";
                ddlServiceLine.DataValueField = "Claims_Service_Details_ID";
                ddlServiceLine.DataBind();
            }

            if (ddlOPPHealhPlanId != null)
            {
                ddlOPPHealhPlanId.DataSource = GetHealthPlanIDForOPAdjustmentServiceDetail();
                ddlOPPHealhPlanId.DataTextField = "Health_Plan_ID";
                ddlOPPHealhPlanId.DataValueField = "Claims_Other_Payer_Information_ID";
                ddlOPPHealhPlanId.DataBind();
            }

            if (ddlOPPAdjustmentGroup != null)
            {
                ddlOPPAdjustmentGroup.DataSource = GetAdjustmentGroupOPP();
                ddlOPPAdjustmentGroup.DataTextField = "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC";
                ddlOPPAdjustmentGroup.DataValueField = "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID";
                ddlOPPAdjustmentGroup.DataBind();
            }


            if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
            {
                if (ClaimType == CON.ClaimsType.Dental)
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = !DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = !DisplayReadOnly;
                }
                else if (ClaimType == CON.ClaimsType.Professional)
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = DisplayReadOnly;
                }
                else
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = !DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = !DisplayReadOnly;
                }

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ClaimType == CON.ClaimsType.Dental)
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = DisplayReadOnly;
                }
                else if (ClaimType == CON.ClaimsType.Professional)
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = DisplayReadOnly;
                }
                else
                {
                    //gvOPPAdjustmentInfo.Columns[10].Visible = DisplayReadOnly;
                    //gvOPPAdjustmentInfo.Columns[11].Visible = DisplayReadOnly;
                }
            }

        }
        if (DisplayReadOnly == true)
        {
            if (ClaimType == CON.ClaimsType.Dental)
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = false;
                //gvOPPAdjustmentInfo.Columns[11].Visible = false;
                divOtherPayer.Visible = false;
            }
            else if (ClaimType == CON.ClaimsType.Professional)
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = false;
                //gvOPPAdjustmentInfo.Columns[11].Visible = false;
                divOtherPayer.Visible = false;
            }
            else
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = false;
                //gvOPPAdjustmentInfo.Columns[11].Visible = false;
                divOtherPayer.Visible = false;
            }
        }

        else
        {
            if (ClaimType == CON.ClaimsType.Dental)
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = true;
                //gvOPPAdjustmentInfo.Columns[11].Visible = true;
                divOtherPayer.Visible = true;
            }
            else if (ClaimType == CON.ClaimsType.Professional)
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = true;
                //gvOPPAdjustmentInfo.Columns[11].Visible = true;
                divOtherPayer.Visible = true;
            }
            else
            {
                //gvOPPAdjustmentInfo.Columns[10].Visible = true;
                //gvOPPAdjustmentInfo.Columns[11].Visible = true;
                divOtherPayer.Visible = true;
            }
        }
    }

    protected void txtOtherPayerReasonCode_TextChanged(object sender, EventArgs e)
    {
        lblOtherPayerAdjustmenterrormsg.Text = "";
        if (!string.IsNullOrEmpty(txtOtherPayerReasonCode.Text))
        {
            DataTable dt = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Text.Trim(), "");

            if (!Helper.HasRows(dt))
            {
                if (dt.Rows.Count >= 0)
                {
                    lblOtherPayerAdjustmenterrormsg.Text = "Reason code is invalid";
                    txtOtherPayerReasonCode.Text = string.Empty;
                    return;
                }
                else
                {
                    lblOtherPayerAdjustmenterrormsg.Text = "";
                    Session["ReasonCode"] = txtOtherPayerReasonCode.Text.Trim();
                }
            }
            else
            {
                lblOtherPayerAdjustmenterrormsg.Text = "";
                Session["ReasonCode"] = txtOtherPayerReasonCode.Text.Trim();
            }

        }
    }
    public void SaveToDbOnAdjust(DataTable dt)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("CLAIM_ID", DbType.Int32, hdnOPAdjustmentClaimID.Value, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_Clear_Other_Payer_Adjustment_Service_Detail", param, "Claims_Other_Payer_Adjustment_Service_Detail");

        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", dt.Rows[i]["Service_Line"].ToString());
                parms.Add("Procedure_Code", dt.Rows[i]["Procedure_Code"].ToString());
                parms.Add("Health_Plan_ID", dt.Rows[i]["Health_Plan_ID"].ToString());
                parms.Add("Adjustment_Group", dt.Rows[i]["Adjustment_Group"].ToString());
                parms.Add("Reason_Code", dt.Rows[i]["Reason_Code"].ToString());
                parms.Add("Amount", dt.Rows[i]["Amount"].ToString());
                string Qunatity = string.Empty;
                if (!string.IsNullOrEmpty(dt.Rows[i]["Quantity"].ToString()))
                {
                    decimal quantity = 0;
                    if (decimal.TryParse(dt.Rows[i]["Quantity"].ToString(), out quantity))
                    {
                        Qunatity = Convert.ToString(Math.Round(quantity));
                    }
                }
                parms.Add("Quantity", Qunatity);

                if (!string.IsNullOrEmpty(hdnOPAdjustmentClaimID.Value))
                {
                    if (ClaimType == CON.ClaimsType.Institutional)
                    {
                        parms.Add("Revenue_Code", dt.Rows[i]["Revenue_Code"].ToString());
                    }
                    else
                    {
                        parms.Add("Revenue_Code", null);
                    }

                    parms.Add("Claim_ID", hdnOPAdjustmentClaimID.Value);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    svc.InsertPanelsData("Claims_Other_Payer_Adjustment_Service_Detail", parms);
        
                    BindOtherPayerAdjustmentInfoGrid(null);
                }
            }
            BindOtherPayerAdjustmentInfoGrid(null);
        }
    }
}
