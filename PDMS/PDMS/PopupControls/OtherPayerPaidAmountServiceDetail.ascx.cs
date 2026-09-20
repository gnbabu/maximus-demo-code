using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Claims = Models.Data.Claims;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OtherPayerPaidAmountServiceDetail : BasePopupControl
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
            if (!string.IsNullOrWhiteSpace(hdnOPPAmountClaimID.Value))
                return hdnOPPAmountClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOPPAmountClaimID.Value = value.Trim();
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

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOPPAmountClaimType.Value))
                return hdnOPPAmountClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOPPAmountClaimType.Value = value.Trim();
            SetVisibleField(value);
        }
    }

    private DataSet dsOtherPayerPaidAmount = new DataSet();

    public DataSet datasetOtherPayerPaidAmount
    {
        get
        {
            if (!Helper.HasRows(dsOtherPayerPaidAmount))
            {
                if (Helper.HasRows(GetOtherPayerPaidAmount()))
                {
                    return dsOtherPayerPaidAmount;
                }
            }
            return dsOtherPayerPaidAmount;
        }
        set
        {
            if (value != null)
            {
                BindOtherPayerAdjudicationInfoGrid(value);
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
            BindOtherPayerAdjudicationInfoGrid(null);
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetServiceLineNoFromServiceDetailPanel();
            GetHealthPlanIDForOPPAmountServiceDetail();
            if (!FromInquirySvc)
            {
                BindOtherPayerAdjudicationInfoGrid(null);
            }
        }
        SetButtonVisibility();
        //btnOPPAdd.Attributes.Add("onclick", "ServiceDisableEnableConditionAddButton();");
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divotherp.Visible = false;
        }
        else
        {
            divotherp.Visible = true;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {

        if (ddlDetailId.SelectedIndex <= 0)
        {
            GetServiceLineNoFromServiceDetailPanel();
            GetHealthPlanIDForOPPAmountServiceDetail();
        }  
    }

    protected void SetVisibleField(string claimtype)
    {
        if (claimtype == CON.ClaimsType.Institutional)
        {
            divRevenueCode.Visible = true;
            HeaderRevenue.Visible = true;
        }
        else
        {
            divRevenueCode.Visible = false;
            HeaderRevenue.Visible = false;
        }
    }

    public DataSet GetServiceLineNoFromServiceDetailPanel()
    {
        ddlDetailId.Items.Clear();
        List<SqlParameter> param = new List<SqlParameter>();
        DataSet dsServiceNo = new DataSet();
        DataSet dsOPPAmount = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPPAmountClaimID.Value))
        {
            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnOPPAmountClaimID.Value, true));
            dsServiceNo = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
            if (Helper.HasRows(dsServiceNo) &&
                Convert.ToInt32(dsServiceNo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOPPAmountClaimID.Value))
            {
                DataTable dtServiceNo = dsServiceNo.Tables[0];
                Helper.LoadList(ddlDetailId, dtServiceNo, "Service_Line", "Claims_Service_Details_ID", true);
            }
        }
        return dsServiceNo;
    }

    private void RemoveAlreadyAddedHealthPlanID(DropDownList ddlhealthplanId, string serviceLine)
    {
        // Removing already added provider for detail/ServiceLine number.        
        DataTable dtHealthPlanID = FetchHealthPlanIDWithServiceLine(serviceLine).Tables[0];

        var HealthPlanID = dtHealthPlanID.AsEnumerable()
            .Select(row => row.Field<string>("Health_Plan_ID"));
        if (Helper.HasRows(dtHealthPlanID))
        {
            foreach (string row in HealthPlanID)
            {
                ddlhealthplanId.Items.Remove(ddlhealthplanId.Items.FindByText(row));
            }
        }
        else
        {
            GetHealthPlanIDForOPPAmountServiceDetail();
        }
    }

    protected DataSet FetchHealthPlanIDWithServiceLine(string serviceline)
    {
        DataSet dsServiceLine = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_id", hdnOPPAmountClaimID.Value);
        parms.Add("Service_Line", serviceline);
        dsServiceLine = svc.SelectPanelsData("claims_other_payer_adjudication_servicedetail_with_serviceline", parms);
        return dsServiceLine;
    }

    public DataSet GetHealthPlanIDForOPPAmountServiceDetail()
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        DataSet dsHealthID = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPPAmountClaimID.Value))
        {
            param.Add("Claim_ID", hdnOPPAmountClaimID.Value.ToString());
            dsHealthID = svc.SelectPanelsData("claims_other_payer_information", param);

            if (Helper.HasRows(dsHealthID) && Convert.ToInt32(dsHealthID.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOPPAmountClaimID.Value))
            {
                DataTable dt = dsHealthID.Tables[0];
                IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                    where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Details) &&
                                                    row.Field<string>("Claim_Adjudication_Level") != null)
                                                    select row;
                if (healthplanid.Count() > 0)
                {
                    DataTable dthealthplanid = healthplanid.CopyToDataTable();
                    Helper.LoadList(ddlOPPHealhPlanId, dthealthplanid, "Health_Plan_ID", "Claims_Other_Payer_Information_ID", true);
                }
            }
        }
        
        return dsHealthID;
    }

    public void ClearOtherPayerAdjudicationPanel()
    {
        GetServiceLineNoFromServiceDetailPanel();
        GetHealthPlanIDForOPPAmountServiceDetail();
        lbltOPPProcedureCode.Text = string.Empty;
        txtOPPAmount.Text = string.Empty;
        lblOPPPaidDate.Text = "";
        txtOtherPaidServiceCount.Text = string.Empty;
        lbltOPPRevenueCode.Text = string.Empty;
        lblErrorMsgOtherPayerPaid.Text = "";
        hdnOPPAmountClaimID.Value = "";
        hdnOtherPayerPaidClaimStatus.Value = "";
        OPPAServiceDetailOutput.InnerHtml = "";

    }

    public void BindOtherPayerAdjudicationInfoGrid(DataSet dsOPPAmt)
    {
        DataSet dsOtherPayerPaidAmt = new DataSet();
        hdnOtherPayerPaidClaimStatus.Value = "";
        if (!string.IsNullOrEmpty(hdnOPPAmountClaimID.Value) || !string.IsNullOrEmpty(ICN))
        {
            if (Helper.HasRows(dsOPPAmt))
            {
                dsOtherPayerPaidAmt = dsOPPAmt;
            }
            else
            {
                dsOtherPayerPaidAmt = GetOtherPayerPaidAmount();
            }
        }

        if (Helper.HasRows(dsOtherPayerPaidAmt))
        {
            string table = string.Empty;
            DataTable dtOPPAServiceList = dsOtherPayerPaidAmt.Tables[0];

            if (dtOPPAServiceList.Rows.Count > 0)
            {
                if (hdnOPPAmountClaimType.Value == CON.ClaimsType.Institutional)
                {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Revenue Code</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                }
                else
                {
                    table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Service Line</th><th style='width:10px; scope='col'>Procedure Code</th > <th style='width:10px; scope=' col'> Health Plan ID</th ><th style='width:10px; scope=' col'> Amount Paid</th ><th style='width:10px; scope=' col'> Paid Date</th ><th style='width:10px; scope=' col'> Paid Service Unit Count</th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th > <th style='width: 10px; scope = ' col' ></th ><th style='width: 10px; ' scope='col'>&nbsp;</th><th style='width: 10px; ' scope='col'>&nbsp;</th></tr > ";
                }
                foreach (DataRow dr in dtOPPAServiceList.Rows)
                {

                    string Service_Line = dr["Service_Line"].ToString();
                    var Revenue_Code = dr["Revenue_Code"].ToString(); ;
                    var Procedure_Code = dr["Procedure_Code"].ToString();
                    var Health_Plan_ID = dr["Health_Plan_ID"].ToString();
                    var Amount_Paid = dr["Amount_Paid"].ToString();
                    var Paid_Date = dr["Paid_Date"].ToString();
                    var Paid_unit_Count = dr["Paid_unit_Count"].ToString();
                    var OPPAServiceDetailId = dr["Other_Payer_Adjudication_Information_Service_Detail_ID"].ToString();
                    var ClaimId = dr["Claim_ID"].ToString();
                    if (hdnOPPAmountClaimType.Value == CON.ClaimsType.Institutional)
                    {
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                                hdnOtherPayerPaidClaimStatus.Value = "Pending Submission";
                            }
                            else
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span title='Line' class='tNumber'>" + Revenue_Code + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td></tr > ";
                                hdnOtherPayerPaidClaimStatus.Value = "Other";
                            }
                        }

                    }
                    else if (hdnOPPAmountClaimType.Value == CON.ClaimsType.Dental || hdnOPPAmountClaimType.Value == CON.ClaimsType.Professional)
                    {
                        if (Session["ClaimStatus"] != null)
                        {
                            if (Session["ClaimStatus"].ToString() == "Pending Submission")
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOPPAServiceDetailLineItem(\"" + OPPAServiceDetailId + "\",\"" + ClaimId + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOPPAServiceDetailLItem(\"" + OPPAServiceDetailId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr > ";
                                hdnOtherPayerPaidClaimStatus.Value = "Pending Submission";
                            }
                            else
                            {
                                table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Service_Line + "</span></td><td><span  title='Line' class='tNumber'>" + Procedure_Code + "</span></td><td><span title='Line' class='tNumber'>" + Health_Plan_ID + "</span></td><td><span title='Line' class='tNumber'>" + Amount_Paid + "</span></td><td><span title='Line' class='tNumber'>" + Paid_Date + "</span></td><td><span title='Line' class='tNumber'>" + Paid_unit_Count + "</span></td></tr > ";
                                hdnOtherPayerPaidClaimStatus.Value = "Other";
                            }
                        }
                    }
                };
                table = table + "</tbody></table>";
                OPPAServiceDetailOutput.InnerHtml = table;

            }
            else
            {
                OPPAServiceDetailOutput.InnerHtml = "";
            }
        }

        if (DisplayReadOnly == true)
        {
            divotherp.Visible = false;
        }
        else if (DisplayReadOnly == false)
        {
            divotherp.Visible = true;
        }
    }

    private DataSet GetOtherPayerPaidAmount()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsOPPAmount = new DataSet();
        if (!string.IsNullOrEmpty(hdnOPPAmountClaimID.Value))
        {
            parms.Add("Claim_ID", hdnOPPAmountClaimID.Value);
            dsOtherPayerPaidAmount = dsOPPAmount = svc.SelectPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
        }
        return dsOPPAmount;
    }
    public void SaveToDBonAdjust(DataTable dt)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("CLAIM_ID", DbType.Int32, hdnOPPAmountClaimID.Value, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_Clear_Other_Payer_Amount_For_Claim", param, "Claims_Service_Details");

        if (Helper.HasRows(dt))
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", int.Parse(dt.Rows[i]["Service_line"].ToString()).ToString());
                parms.Add("Procedure_Code", dt.Rows[i]["Procedure_Code"].ToString());
                parms.Add("Health_Plan_ID", dt.Rows[i]["Health_Plan_ID"].ToString());
                parms.Add("Amount_Paid", dt.Rows[i]["Amount_Paid"].ToString());
                parms.Add("Paid_Date", dt.Rows[i]["Paid_Date"].ToString());
                parms.Add("Paid_unit_Count", dt.Rows[i]["Paid_unit_Count"].ToString());
                if (!string.IsNullOrEmpty(hdnOPPAmountClaimID.Value))
                {
                    if (ClaimType == CON.ClaimsType.Institutional)
                    {
                        parms.Add("Revenue_Code", dt.Rows[i]["Paid_unit_Count"].ToString());
                    }
                    else
                    {
                        parms.Add("Revenue_Code", null);
                    }
                    parms.Add("Claim_ID", hdnOPPAmountClaimID.Value);
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    svc.InsertPanelsData("Claims_Other_Payer_Adjudication_Information_Service_Detail", parms);
                    BindOtherPayerAdjudicationInfoGrid(null);
                }
            }
            
        }
    }
}
