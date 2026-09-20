using Corp.Core.Libraries;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_HeaderOtherPayerAdjustmentMappingProfessional : System.Web.UI.UserControl
{
    private bool fromInquirySvc = false;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(hdnClaimId.Value) && !FromInquirySvc)
            {
                List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();

                SetOtherPayerAdjustmentInfoPanelData(listData, null);
            }
            GetHealthPlanIDForHeaderOtherPayer();
            GetAdjustmentGroup();
        }
        else
        {
            btnOtherPayerAdjustmentInfoAdd.Attributes.Add("onclick", "HeaderDisableEnableConditionAddButton();");
        }

        SetButtonVisibility();
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
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            Searchother.Visible = false;
        }
        else
        {
            Searchother.Visible = true;
        }
    }
    #region "property"
    public string IcnNumber = string.Empty;
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
    private DataSet dsOtherPayerAdjustmentInformation = new DataSet();
    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimId.Value) || !string.IsNullOrEmpty(ICN))
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
    private DataSet FetchOtherPayerAdjustmentInformation()
    {
        DataSet dsOtherPayerInfo = new DataSet();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimId.Value.ToString());
        dsOtherPayerAdjustmentInformation = dsOtherPayerInfo = svc.SelectPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parms);
        return dsOtherPayerInfo;
    }
    public DataSet dataSetOtherPayer
    {
        get
        {
            if (!Helper.HasRows(dsOtherPayerAdjustmentInformation))
            {
                if (Helper.HasRows(FetchOtherPayerAdjustmentInformation()))
                {
                    return dsOtherPayerAdjustmentInformation;
                }
            }
            return dsOtherPayerAdjustmentInformation;
        }

        set
        {
            if (value != null)
            {
                List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
                SetOtherPayerAdjustmentInfoPanelData(listData, value);
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
            //List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
            //SetOtherPayerAdjustmentInfoPanelData(listData, null);
        }
    }
    #endregion

    public string Claim_Id { get; set; }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Session["ReasonCode"] != null)
        {
            if ((Session["ReasonCode"] != null) && (Session["ReasonCode"] != ""))
            {
                txtOtherPayerReasonCode.Text = Session["ReasonCode"].ToString();
            }
        }
        if (ddlOtherPayerHealthPlanID.SelectedIndex < 0 || ddlOtherPayerHealthPlanID.SelectedIndex == 0)
        {
            GetHealthPlanIDForHeaderOtherPayer();
        }
    }
    //protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    gvOtherPayerAdjustmentInfo.EditIndex = e.NewEditIndex;
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //}
    public void SetOtherPayerAdjustmentInfoPanelData(List<OtherPayerAdjustmentInfo> OtherPayerAdjInfo, DataSet dsOtherPayerInfo)
    {
        DataSet dsotherPayerAdjus = new DataSet();
        //if (!string.IsNullOrEmpty(hdnClaimId.Value))
        //{
        //    if (Helper.HasRows(dsOtherPayerInfo))
        //    {
        //        dsotherPayerAdjus = dsOtherPayerInfo;
        //    }
        //    else
        //    {
        //        dsotherPayerAdjus = FetchOtherPayerAdjustmentInformation();
        //    }
        //}

        //if (Helper.HasRows(dsotherPayerAdjus))
        //{
        //    //gvOtherPayerAdjustmentInfo.DataSource = dsotherPayerAdjus;
        //    //gvOtherPayerAdjustmentInfo.DataBind();
        //}
        //else
        //{
        //    //gvOtherPayerAdjustmentInfo.DataSource = null;
        //    //gvOtherPayerAdjustmentInfo.DataBind();
        //}
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            if (Helper.HasRows(dsOtherPayerInfo))
            {
                dsotherPayerAdjus = dsOtherPayerInfo;
            }
            else
            {
                dsotherPayerAdjus = FetchOtherPayerAdjustmentInformation();
            }
        }
        if (Helper.HasRows(dsotherPayerAdjus))
        {
            DataTable otherpayerAdjusTable = dsotherPayerAdjus.Tables[0];
            providerHeaderAdjustmentinstiOutputProf.InnerHtml = "";
            if (otherpayerAdjusTable.Rows.Count > 0)
            {
                string tab = string.Empty;
                tab = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th scope='col'>Health Plan ID</th><th style='width:10px;' scope='col'>Adjustment Group</th><th style='width:10px;' scope='col'>Reason Code</th><th style='width:10px;' scope='col'>Amount</th><th style='width:30px; scope='col'>Quantity</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in otherpayerAdjusTable.Rows)
                {
                    string HealthPlanID = dr["Health_Plan_ID"].ToString();
                    string AdjustmentGroup = dr["cde_adjustment_group"].ToString();
                    string ReasonCode = dr["cde_reason_code"].ToString();
                    string PayerAmount = dr["cde_amount"].ToString();
                    string PayerQuantity = dr["cde_quantity"].ToString();
                    string Claim_ID = dr["Claim_ID"].ToString();
                    string Claims_Header_Other_Payer_Adjustment_Information_ID = dr["Claims_Header_Other_Payer_Adjustment_Information_ID"].ToString();

                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td><td><input type='button' value = 'Edit' onClick = 'return EditFieldsHeaderAdjustmentInstitutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteHeaderAdjustmentOtherPayerInfo_Institutional(\"" + Claim_ID + "\",\"" + Claims_Header_Other_Payer_Adjustment_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        }
                        else
                        {
                            tab = tab + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + HealthPlanID + "</span></td><td><span title='Line' class='tNumber'>" + AdjustmentGroup + "</span></td><td><span  title='Line' class='tNumber'>" + ReasonCode + "</span></td><td><span title='Line' class='tNumber'>" + PayerAmount + "</span></td><td>" + PayerQuantity + "</td></tr>";
                        }
                    }
                }
                tab = tab + "</tbody></table>";
                providerHeaderAdjustmentinstiOutputProf.InnerHtml = tab;
            }

        }
        else { providerHeaderAdjustmentinstiOutputProf.InnerHtml = ""; }
    }
    //protected void btnOtherPayerAdjustmentInfoAdd_Click(object sender, EventArgs e)
    //{
    //    lblErrorMessage.Text = "";
    //    if (Page.IsPostBack)
    //    {
    //        if (!string.IsNullOrEmpty(hdnClaimId.Value))
    //        {
    //            lblErrorMessage.Text = string.Empty;
    //            DataSet Otherpayer = new DataSet();
    //            Otherpayer = dataSetOtherPayer;
    //            int samepayer = 0;
    //            for (int i = 0; i < Otherpayer.Tables[0].Rows.Count; i++)
    //            {
    //                String OtherPayerReasonCode = Otherpayer.Tables[0].Rows[i]["cde_reason_code"].ToString();
    //                String drpdownHelthplanId = Otherpayer.Tables[0].Rows[i]["Health_Plan_ID"].ToString();
    //                String drpdownAdjustment_Group = Otherpayer.Tables[0].Rows[i]["cde_adjustment_group"].ToString();
    //                if (!string.IsNullOrEmpty(txtOtherPayerReasonCode.Text))
    //                {
    //                    if (txtOtherPayerReasonCode.Text == OtherPayerReasonCode && drpdownAdjustment_Group == ddlOtherPayerAdjustmentGroup.SelectedItem.Text
    //                        && drpdownHelthplanId == ddlOtherPayerHealthPlanID.SelectedItem.Text)
    //                    {
    //                        lblErrorMessage.Text = "Same reason code cannot be reported multiple times with same adjustment group for the same payer. ";
    //                        return;
    //                    }
    //                }
    //                if (drpdownHelthplanId == ddlOtherPayerHealthPlanID.SelectedItem.Text && drpdownAdjustment_Group == ddlOtherPayerAdjustmentGroup.SelectedItem.Text)
    //                {
    //                    samepayer++;
    //                    if (samepayer == 6)
    //                    {
    //                        lblErrorMessage.Text = "Each adjustment group can be repeated up to 6 times for one other payer";
    //                        return;
    //                    }

    //                }
    //            }
    //            DataTable dt = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Text.Trim(), null);
    //            if (Helper.HasRows(dt))
    //            {
    //                if (dt.Rows.Count >= 1)
    //                {
    //                    txtOtherPayerReasonCode.Text = txtOtherPayerReasonCode.Text.ToUpper();
    //                }
    //                else
    //                {
    //                    lblErrorMessage.Text = "Invalid Reason Code";
    //                    return;
    //                }
    //            }
    //            Dictionary<string, string> parms = new Dictionary<string, string>();
    //            parms.Add("Claim_id", hdnClaimId.Value);
    //            parms.Add("Health_Plan_ID", ddlOtherPayerHealthPlanID.SelectedItem.Text);
    //            parms.Add("Adjustment_Group", ddlOtherPayerAdjustmentGroup.SelectedItem.Text);
    //            parms.Add("Reason_Code", string.IsNullOrEmpty(txtOtherPayerReasonCode.Text) ? null : txtOtherPayerReasonCode.Text);
    //            parms.Add("Amount", string.IsNullOrEmpty(txtOtherPayerAmount.Text) ? null : txtOtherPayerAmount.Text);
    //            parms.Add("Quantity", string.IsNullOrEmpty(txtOtherPayerQuantity.Text) ? null : txtOtherPayerQuantity.Text);
    //            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
    //            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //            parms.Add("Created_Date_Time", DateTime.Now.ToString());
    //            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //            svc.InsertPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parms);
    //        }
    //        List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //        SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //        ClearOtherPayerAdjustmentInfoFields();
    //    }
    //}

    public DataSet GetHealthPlanIDForHeaderOtherPayer()
    {
        Dictionary<string, string> param = new Dictionary<string, string>();
        param.Add("Claim_ID", hdnClaimId.Value.ToString());
        DataSet ds = svc.SelectPanelsData("claims_other_payer_information", param);

        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            if (Helper.HasRows(ds) &&
            Convert.ToInt32(ds.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value)
             && (ds.Tables[0].Rows[0]["Claim_Adjudication_Level"] != null))
            {
                DataTable dt = ds.Tables[0];
                IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                    where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Header))
                                                    select row;
                if (healthplanid.Count() > 0)
                {
                    DataTable dthealthplanid = healthplanid.CopyToDataTable();
                    Helper.LoadList(ddlOtherPayerHealthPlanID, dthealthplanid, "Health_Plan_ID", "Claims_Other_Payer_Information_ID", true);
                }
            }
        }
        return ds;
    }
    public void ClearOtherPayerAdjustmentInfoFields()
    {
        txtOtherPayerReasonCode.Text = string.Empty;
        txtOtherPayerAmount.Text = string.Empty;
        txtOtherPayerQuantity.Text = string.Empty;
        ddlOtherPayerAdjustmentGroup.Items.Clear();
        ddlOtherPayerHealthPlanID.Items.Clear();
        //GetAdjustmentGroup();
        providerHeaderAdjustmentinstiOutputProf.InnerHtml = "";
    }

    public void ClearGrid()
    {
        //gvOtherPayerAdjustmentInfo.DataSource = null;
        //gvOtherPayerAdjustmentInfo.DataBind();
    }

    public void GetAdjustmentGroup()
    {
        ddlOtherPayerAdjustmentGroup.Items.Clear();
        DataSet dataSet = svc.GetAdjustmentGroup();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlOtherPayerAdjustmentGroup, dt, "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC", "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID", true);
    }
    protected void lnkOtherPayerReasonSearch_Click(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchReason.Show();
    }
    //protected void OnRowCancelingEdit(object sender, EventArgs e)
    //{
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    gvOtherPayerAdjustmentInfo.EditIndex = -1;
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //}

    //protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    string keyVal = gvOtherPayerAdjustmentInfo.DataKeys[e.RowIndex].Value.ToString();
    //    DropDownList ddlOtherPayerHealthPlanID = (DropDownList)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("ddlOtherPayerHealthPlanGVID");
    //    DropDownList ddlOtherPayerAdjustmentGroup = (DropDownList)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("ddlOtherPayerAdjustmentGroup");
    //    if (!string.IsNullOrEmpty(hdnClaimId.Value))
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("Claim_Id", hdnClaimId.Value);
    //        parms.Add("Claims_Header_Other_Payer_Adjustment_Information_ID", keyVal);
    //        parms.Add("Health_Plan_ID", ddlOtherPayerHealthPlanID.SelectedItem.Text);
    //        parms.Add("Adjustment_Group", ddlOtherPayerAdjustmentGroup.SelectedItem.Text);
    //        parms.Add("Reason_Code", string.IsNullOrEmpty(e.NewValues["cde_reason_code"].ToString()) ? null : e.NewValues["cde_reason_code"].ToString());
    //        parms.Add("Amount", string.IsNullOrEmpty(e.NewValues["cde_amount"].ToString()) ? null : e.NewValues["cde_amount"].ToString());
    //        parms.Add("Quantity", string.IsNullOrEmpty(e.NewValues["cde_quantity"].ToString()) ? null : e.NewValues["cde_quantity"].ToString());
    //        parms.Add("Last_Modified_Date", DateTime.Now.ToString());
    //        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //        parms.Add("Created_Date_Time", DateTime.Now.ToString());
    //        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //        svc.UpdatePanelsData("claims_header_other_payer_adjustment_information", parms);
    //        List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //        gvOtherPayerAdjustmentInfo.EditIndex = -1;
    //        SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //    }
    //}

    //protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    int index = Convert.ToInt32(e.RowIndex);
    //    HiddenField hdnOtherPayerAdjustmentInfo = (HiddenField)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("hdnOtherPayerAdjustmentInfo");
    //    svc.DeletePanelsData("claims_header_other_payer_adjustment_information", "Claims_Header_Other_Payer_Adjustment_Information_ID", Convert.ToInt32(hdnOtherPayerAdjustmentInfo.Value));
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //}

    //protected void gvOtherPayerAdjustmentInfo_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    gvOtherPayerAdjustmentInfo.EditIndex = -1;
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //    lblErrorMessage.Text = "";
    //}
    //protected void gvOtherPayerAdjustmentInfo_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    int keyVal = Convert.ToInt32(gvOtherPayerAdjustmentInfo.DataKeys[e.RowIndex]["Claims_Header_Other_Payer_Adjustment_Information_ID"]);
    //    DropDownList ddlOtherPayerHealthPlanID = (DropDownList)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("ddlOtherPayerHealthPlanGVID");
    //    DropDownList ddlOtherPayerAdjustmentGroup = (DropDownList)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("ddlOtherPayerAdjustmentGroup");
    //    TextBox txtHeaderreasonCode = (TextBox)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("txtHeaderreasonCode");
    //    HiddenField hdnreasoncode = (HiddenField)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("hdnreasoncodepf");
    //    HiddenField hdnAdjustgrp = (HiddenField)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("hdnAdjustgrppf");
    //    HiddenField hdnOtherPayerAdjustmentInfo = (HiddenField)gvOtherPayerAdjustmentInfo.Rows[e.RowIndex].FindControl("hdnOtherPayerAdjustmentInfopf");

    //    Page.Validate("vgBilledUnits");
    //    lblErrorMessage.Text = "";
    //    if ((ddlOtherPayerAdjustmentGroup != null) && (txtHeaderreasonCode != null) && (hdnreasoncode != null) && (hdnAdjustgrp != null) && (hdnOtherPayerAdjustmentInfo != null))
    //    {
    //        if ((txtHeaderreasonCode.Text == hdnreasoncode.Value) && (ddlOtherPayerAdjustmentGroup.SelectedItem.Text == hdnAdjustgrp.Value)
    //            && (ddlOtherPayerHealthPlanID.SelectedItem.Text == hdnOtherPayerAdjustmentInfo.Value)) { }
    //        else
    //        {
    //            DataSet Otherpayer = new DataSet();
    //            Otherpayer = dataSetOtherPayer;
    //            DataTable dt = Otherpayer.Tables[0];
    //            IEnumerable<DataRow> dtDetails = from row in dt.AsEnumerable()
    //                                             where row.Field<string>("cde_reason_code") == txtHeaderreasonCode.Text
    //                                             && row.Field<string>("cde_adjustment_group") == ddlOtherPayerAdjustmentGroup.SelectedItem.Text.Trim()
    //                                             && row.Field<string>("Health_Plan_ID") == ddlOtherPayerHealthPlanID.SelectedItem.Text.Trim()
    //                                             select row;
    //            if (dtDetails.Any())
    //            {
    //                lblErrorMessage.Text = "Same reason code cannot be reported multiple times with same adjustment group for the same payer. ";
    //                return;
    //            }
    //        }

    //    }
    //    DataTable dtReasoncd = LookupTableController.GetCrcReasoneCode(txtHeaderreasonCode.Text, null);
    //    if (!Helper.HasRows(dtReasoncd))
    //    {
    //        if (dtReasoncd.Rows.Count == 0)
    //        {
    //            lblErrorMessage.Text = "Invalid Reason Code";
    //            return;
    //        }

    //    }

    //    if (!string.IsNullOrEmpty(hdnClaimId.Value))
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("Claim_Id", hdnClaimId.Value);
    //        parms.Add("Claims_Header_Other_Payer_Adjustment_Information_ID", keyVal.ToString());
    //        parms.Add("Health_Plan_ID", ddlOtherPayerHealthPlanID.SelectedItem.Text);
    //        parms.Add("Adjustment_Group", ddlOtherPayerAdjustmentGroup.SelectedItem.Text);
    //        parms.Add("Reason_Code", txtHeaderreasonCode.Text);
    //        parms.Add("Amount", string.IsNullOrEmpty(e.NewValues["cde_amount"].ToString()) ? null : e.NewValues["cde_amount"].ToString());
    //        //parms.Add("Quantity", string.IsNullOrEmpty(e.NewValues["cde_quantity"].ToString()) ? null : e.NewValues["cde_quantity"].ToString());
    //        parms.Add("Last_Modified_Date", DateTime.Now.ToString());
    //        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    //        parms.Add("Created_Date_Time", DateTime.Now.ToString());
    //        parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

    //        svc.UpdatePanelsData("claims_header_other_payer_adjustment_information", parms);
    //        List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //        gvOtherPayerAdjustmentInfo.EditIndex = -1;
    //        this.SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //    }
    //}
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (DisplayReadOnly == true)
        {
            Searchother.Visible = false;
        }
        else
        {
            Searchother.Visible = true;
        }
        if (e.Row.RowType == DataControlRowType.Header && DisplayReadOnly)
        {
            e.Row.Cells[5].Visible = !DisplayReadOnly;
        }
        if (e.Row.RowType == DataControlRowType.DataRow )
        {
            if (DisplayReadOnly)
            {
                e.Row.Cells[5].Visible = !DisplayReadOnly;
            }
        }
        if ((e.Row.RowState & DataControlRowState.Edit) > 0)
        {
            //_svc = new PDMSService.PDMSServiceClient();
            DropDownList HealthPlanID = (DropDownList)e.Row.FindControl("ddlOtherPayerHealthPlanGVID");
            DropDownList AdjustmentGroup = (DropDownList)e.Row.FindControl("ddlOtherPayerAdjustmentGroup");
            DataSet dataSet = svc.GetAdjustmentGroup();
            DataSet ds = new DataSet();
            if (!string.IsNullOrEmpty(hdnClaimId.Value))
            {
                Dictionary<string, string> param = new Dictionary<string, string>();
                param.Add("Claim_ID", hdnClaimId.Value.ToString());
                ds = svc.SelectPanelsData("claims_other_payer_information", param);

                if (!string.IsNullOrEmpty(hdnClaimId.Value))
                {
                    if (Helper.HasRows(ds) &&
                    Convert.ToInt32(ds.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimId.Value)
                     && (ds.Tables[0].Rows[0]["Claim_Adjudication_Level"] != null))
                    {
                        DataTable dt = ds.Tables[0];
                        IEnumerable<DataRow> healthplanid = from row in dt.AsEnumerable()
                                                            where (row.Field<string>("Claim_Adjudication_Level") == Convert.ToString(CON.ClaimsAdjudicationLevel.Header))
                                                            select row;
                        if (healthplanid.Count() > 0)
                        {
                            DataTable dthealthplanid = healthplanid.CopyToDataTable();
                            HealthPlanID.DataSource = dthealthplanid;
                            HealthPlanID.DataTextField = "Health_Plan_ID";
                            HealthPlanID.DataValueField = "Claims_Other_Payer_Information_ID";
                            Helper.LoadList(HealthPlanID, dthealthplanid, "Health_Plan_ID", "Claims_Other_Payer_Information_ID", true);
                        }
                    }
                }
            }

            AdjustmentGroup.DataSource = dataSet;
            AdjustmentGroup.DataTextField = "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_DESC";
            AdjustmentGroup.DataValueField = "PRIOR_AUTH_CLAIM_ADJUSTMENTGROUP_ID";
            AdjustmentGroup.DataBind();
            AdjustmentGroup.SelectedValue = DataBinder.Eval(e.Row.DataItem, "cde_adjustment_group").ToString();
        }
        if (DisplayReadOnly)
        {
            Searchother.Visible = false;
        }
        else
        {
            Searchother.Visible = true;
        }
    }
    //protected void gvOtherPayerAdjustmentInfo_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    int keyVal = Convert.ToInt32(gvOtherPayerAdjustmentInfo.DataKeys[e.RowIndex].Value);
    //    DeleteOtherPayerAdjusMentInfo(keyVal);
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //}
    private void DeleteOtherPayerAdjusMentInfo(int Id)
    {
        svc.DeletePanelsData("claims_header_other_payer_adjustment_information", "Claims_Header_Other_Payer_Adjustment_Information_ID", Convert.ToInt32(Id));
    }
    //protected void gvOtherPayerAdjustmentInfo_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    string keyVal = gvOtherPayerAdjustmentInfo.DataKeys[e.NewEditIndex].Value.ToString();
    //    gvOtherPayerAdjustmentInfo.EditIndex = e.NewEditIndex;
    //    List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
    //    SetOtherPayerAdjustmentInfoPanelData(listData, null);
    //}


    protected void txtOtherPayerReasonCode_TextChanged(object sender, EventArgs e)
    {
        lblHeaderDentalerror.Text = "";
        if (!string.IsNullOrEmpty(txtOtherPayerReasonCode.Text))
        {
            DataTable dt = LookupTableController.GetCrcReasoneCode(txtOtherPayerReasonCode.Text.Trim(), "");


            if (!Helper.HasRows(dt))
            {
                if (dt.Rows.Count >= 0)
                {
                    lblHeaderDentalerror.Text = "Reason code is invalid";
                }
                else
                {
                    lblHeaderDentalerror.Text = "";
                }
            }
            else
            {
                lblHeaderDentalerror.Text = "";
            }

        }
    }

    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_id", hdnClaimId.Value);
                parms.Add("Health_Plan_ID", dt.Rows[i]["Health_Plan_ID"].ToString());
                parms.Add("Adjustment_Group", dt.Rows[i]["cde_adjustment_group"].ToString());
                parms.Add("Reason_Code", dt.Rows[i]["cde_reason_code"].ToString());
                parms.Add("Amount", dt.Rows[i]["cde_amount"].ToString());
                parms.Add("Quantity", dt.Rows[i]["cde_quantity"].ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertPanelsData("Claims_Header_Other_Payer_Adjustment_Information", parms);
                List<OtherPayerAdjustmentInfo> listData = new List<OtherPayerAdjustmentInfo>();
                SetOtherPayerAdjustmentInfoPanelData(listData, null);

            }
            SetOtherPayerAdjustmentInfoPanelData(null, null);
        }
    }
}