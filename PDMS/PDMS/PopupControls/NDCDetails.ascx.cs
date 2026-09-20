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

public partial class PopupControls_NDCDetails : System.Web.UI.UserControl
{
    public bool IsChecked = false;
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
            if (!string.IsNullOrWhiteSpace(hdnNDC_ClaimID.Value))
                return hdnNDC_ClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnNDC_ClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnNDC_ClaimType.Value))
                return hdnNDC_ClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnNDC_ClaimType.Value = value.Trim();
        }
    }

    private DataSet dsNDCDetails = new DataSet();

    public DataSet datasetNDCDetails
    {
        get
        {
            if (!Helper.HasRows(dsNDCDetails))
            {
                if (Helper.HasRows(GetNDCDetails()))
                {
                    return dsNDCDetails;
                }
            }
            return dsNDCDetails;
        }
        set
        {
            if (value != null)
            {
                BindNDCDetailGrid(value);
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
        }
    }
    #endregion

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (IsChecked == false)
        {
            txtNDCCode.Text = ucSubmitClaimSearchNDC.NDC_Code;
        }
        IsChecked = false;

        if (!string.IsNullOrEmpty(txtNDCCode.Text))
        {
            ucSubmitClaimSearchNDC.NDC_Code = string.Empty;
        }
        if (ddlNDCServiceLine.SelectedIndex <= 0 )//|| gvNDCDetails.Rows.Count > 0)
        {
            GetServiceLineNoForNDCDetails();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!IsPostBack)
        //{
        //    BindNDCDetailGrid(null);
        //}
        if (!Page.IsPostBack)
        {
            GetServiceLineNoForNDCDetails();
          
        }
        //if (!string.IsNullOrEmpty(hdnNDC_ClaimID.Value))
        //{
        //    BindNDCDetailGrid(null);
        //}
        // btnNDCAdd.Attributes.Add("onclick", "NDCDisableEnableConditionCodeAddButton();");
        GetUnitsOfMeasureForNDC();
        SetButtonVisibility();
    }
    public void SetButtonVisibility()
    {
        //if (DisplayReadOnly == true)
        //{
        //    divNDC.Visible = false;
        //}
        //else
        //{
        //    divNDC.Visible = true;
        //}
    }

    public  DataSet GetServiceLineNoForNDCDetails()
    {
        List<SqlParameter> param = new List<SqlParameter>();
        DataSet dsServiceLine = new DataSet();
        DataTable dtServiceLine=new DataTable();
        ddlNDCServiceLine.Items.Clear();
        if (!string.IsNullOrEmpty(hdnNDC_ClaimID.Value))
        {
            param.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnNDC_ClaimID.Value, true));
            dsServiceLine = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Service_Details_Data", param, "Claims_Service_Details");
            dtServiceLine= dsServiceLine.Tables[0];
            if (Helper.HasRows(dsServiceLine) &&
                Convert.ToInt32(dsServiceLine.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnNDC_ClaimID.Value))
            {
                DataSet dsNDCDetail = GetNDCDetails();
                 DataTable dtServiceLineNDC = dsNDCDetail.Tables[0];
                var Service_Line = dtServiceLineNDC.AsEnumerable()
                    .Select(row => row.Field<int>("Service_Line")).ToArray();
                if (Helper.HasRows(dtServiceLineNDC))
                {
                    foreach (int row in Service_Line)
                    {
                        for (int i = dtServiceLine.Rows.Count - 1; i >= 0; i--)
                        {
                            DataRow dr = dtServiceLine.Rows[i];
                            string ServiceLine= dr["Service_Line"].ToString();
                            if (ServiceLine.Equals(row.ToString()))
                                {
                                    dr.Delete();
                                    dtServiceLine.AcceptChanges();
                                }
                        }
                    }
                }
            }
            List<NDCServiceLine> NDCServiceLineList = new List<NDCServiceLine>();
                if (Helper.HasRows(dtServiceLine))
                {
                    foreach (DataRow dr in dtServiceLine.Rows)
                    {
                        NDCServiceLine ServiceLineData = new NDCServiceLine();
                        ServiceLineData.Service_Line = dr["Service_Line"].ToString();
                        ServiceLineData.Claims_Service_Details_ID = dr["Claims_Service_Details_ID"].ToString();
                        NDCServiceLineList.Add(ServiceLineData);
                    }
                }
                Helper.LoadList(ddlNDCServiceLine, NDCServiceLineList, "Service_Line", "Claims_Service_Details_ID", true);

            }
        
        return dsServiceLine;
    }
    public class NDCServiceLine
    {
        public string Service_Line { get; set; }
        public string Claims_Service_Details_ID { get; set; }

    }
    private DataTable GetUnitsOfMeasureForNDC()
    {
        DataSet dataSet = svc.GetUnitsOfMeasure();
        DataTable dtUnitOfMeasure = dataSet.Tables[0];
        //  Convert DataTable to DataView
        DataView dv = dtUnitOfMeasure.DefaultView;
        dv.Sort = "UNITSOFMEASURE_RANK";
        dtUnitOfMeasure = dv.ToTable();

        if (Helper.HasRows(dtUnitOfMeasure))
        {
            Helper.LoadList(ddlUnitOfMeasure, dtUnitOfMeasure, "PRIOR_AUTH_CLAIM_UNITSOFMEASURE_DESC", "PRIOR_AUTH_CLAIM_UNITSOFMEASURE_CODE", true);
            ddlUnitOfMeasure.Items.RemoveAt(0);
        }
        return dtUnitOfMeasure;
    }

    public void BindNDCDetailGrid(DataSet dsNDCData)
    {
        DataSet dsNDCDetail = new DataSet();
        if (!string.IsNullOrEmpty(hdnNDC_ClaimID.Value) || !string.IsNullOrEmpty(ICN))
        {
            if (Helper.HasRows(dsNDCData))
            {
                dsNDCDetail = dsNDCData;
            }
            else
            {
                dsNDCDetail = GetNDCDetails();
            }
        }

        if (Helper.HasRows(dsNDCDetail))
        {
            DataTable dtNdcDetail = dsNDCDetail.Tables[0];
            if (dtNdcDetail.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class=\"gridview\"  cellspacing=\"0\" align=\"Middle\" rules=\"rows\" border=\"1\" style=\"margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;\"><tbody><tr class=\"gridViewHeader\"><th style=\"width:15px; scope=\"col\">Service Line</th><th style=\"width:18px; scope=\"col\">NDC</th><th style=\"width:18px; scope=\"col\">Unit Of Measure</th><th style=\"scope=\"col\">Prescription Number</th><th style=\"width:30px; scope=\"col\">Total Unit</th><th style=\"width:10px;\" scope=\"col\">&nbsp;</th><th style=\"width:10px;\" scope=\"col\">&nbsp;</th></tr>";

                foreach (DataRow dr in dtNdcDetail.Rows)
                {
                    string ServiceLine = dr["Service_Line"].ToString();
                    string NDCCode = dr["NDC"].ToString();
                    string UnitMeasure = dr["Units_of_Measure"].ToString();
                    string PrescriptionNuber = dr["Prescription_Number"].ToString();
                    string TotalUnit = dr["Total_Unit"].ToString();
                    string Claims_NDC_Details_Screen_ID = dr["Claims_NDC_Details_Screen_ID"].ToString();
                    string Claim_ID = hdnNDC_ClaimID.Value;

                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            table = table + "<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">" + ServiceLine + "</span></td><td><span  title=\"NDC\" class=\"tNumber\">" + NDCCode + "</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">" + UnitMeasure + "</span></td><td><span title=\"Prescription Number\">" + PrescriptionNuber + "</span></td><td><span title=\"Total Unit\" class=\"tNumber\">" + TotalUnit + "</span></td><td><input type=\"button\" value = \"Edit\" onClick = \"return EditNDCDetailsItem('" + Claims_NDC_Details_Screen_ID + "','" + Claim_ID + "'); return true;\" class=\"btn btn-primary\" sytle = \"margin-left:3px\"></td><td><input type=\"button\" value=\"Delete\" onclick=\"return DeleteNDCDetailsItem('" + Claims_NDC_Details_Screen_ID + "');\" class=\"btn btn-danger\" sytle=\"margin-left:3px\"></td></tr >";
                        }
                        else
                        {
                            table = table + "<tr style=\"border: 1px solid black; border-collapse: collapse;\" class=\"gridViewRow\"><td><span title=\"Service Line\" class=\"tNumber\">" + ServiceLine + "</span></td><td><span  title=\"NDC\" class=\"tNumber\">" + NDCCode + "</span></td><td><span title=\"Unit Of Measure\" class=\"tNumber\">" + UnitMeasure + "</span></td><td><span title=\"Prescription Number\">" + PrescriptionNuber + "</span></td><td><span title=\"Total Unit\" class=\"tNumber\">" + TotalUnit + "</span></td></tr >";
                        }
                    }
                }
                table = table + "</tbody></table>";
                divNDCDetails.InnerHtml = table;
            }
        }
        else
        {
            divNDCDetails.InnerHtml = "";
        }
    }


    private DataSet GetNDCDetails()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsNDC = new DataSet();
        if (!string.IsNullOrEmpty(hdnNDC_ClaimID.Value))
        {
            parms.Add("Claim_ID", hdnNDC_ClaimID.Value);
            dsNDCDetails = dsNDC = svc.SelectPanelsData("Claims_NDC_Details", parms);
        }
        return dsNDC;
    }

    public void ClearNDCDetailPanel()
    {
        GetServiceLineNoForNDCDetails();
        GetUnitsOfMeasureForNDC();
        txtNDCCode.Text = string.Empty;
        txtPrescriptionNumber.Text = string.Empty;
        txtTotalUnit.Text = string.Empty;
        ucSubmitClaimSearchNDC.NDC_Code = string.Empty;
        lblNDCDetailsmessage.Text = "";
        divNDCDetails.InnerHtml = "";
    }

    protected void lnkNDCCodeSearch_Click(object sender, EventArgs e)
    {
        ucSubmitClaimSearchNDC.GridViewSubmitClaimSearchPop.DataSource = null;
        ucSubmitClaimSearchNDC.GridViewSubmitClaimSearchPop.DataBind();
        mpeSubmitClaimSearchNDC.Show();
        ucSubmitClaimSearchNDC.LabelError.Text = "";
    }

   
    protected void txtNDCode_TextChanged(object sender, EventArgs e)
    {
        lblNDCDetailsmessage.Text = "";
        IsChecked = true;
        DataSet datasetNDC = ValidateNDCData();
        if (!Helper.HasRows(datasetNDC))
        {
            txtNDCCode.Text = "";
            lblNDCDetailsmessage.Text = "NDC is not found";
        }
        else
        {
            lblNDCDetailsmessage.Text = "";
        }
    }
    private DataSet ValidateNDCData()
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("NDCCode", DbType.String, txtNDCCode.Text, true));
        DataSet dataSetNDC = DataAccess.ExecuteStoredProcedure("Usp_Select_CLAIMS_NDC_CODE", parameters, "CLAIMS_NDC_CODE");
        return dataSetNDC;
    }
    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Service_Line", dt.Rows[i]["Service_Line"].ToString());
                parms.Add("NDC", dt.Rows[i]["NDC"].ToString());
                parms.Add("Units_of_Measure", GetUnitOfMeasureCode(dt.Rows[i]["Units_of_Measure"].ToString()));
                parms.Add("Prescription_Number", dt.Rows[i]["Prescription_Number"].ToString());
                parms.Add("Total_Unit", dt.Rows[i]["Total_Unit"].ToString());
                parms.Add("Claim_ID", hdnNDC_ClaimID.Value);
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_date", DateTime.Now.ToString());
                parms.Add("Last_Modified_user", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.InsertPanelsData("Claims_NDC_Details", parms);

                BindNDCDetailGrid(null);
            }
        }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private string GetUnitOfMeasureCode(string unit)
    {
        string unitOfMeasure = string.Empty;
        if (!string.IsNullOrWhiteSpace(unit) && Helper.HasRows(this.WorkflowPage.NDCunitOfMeasure.Tables[0]))
        {
            var dt1 = this.WorkflowPage.NDCunitOfMeasure.Tables[0].AsEnumerable().Where(r => r.Field<String>("PRIOR_AUTH_CLAIM_UNITSOFMEASURE_CODE") == unit);
            var newDt = dt1.CopyToDataTable();
            if (Helper.HasRows(newDt))
            {
                unitOfMeasure = newDt.Rows[0]["PRIOR_AUTH_CLAIM_UNITSOFMEASURE_CODE"].ToString();
            }
        }
        return unitOfMeasure;
    }
}