using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using PDMSService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_OccurenceSpanInformation : System.Web.UI.UserControl
{

    public bool isChecked = false;
    public bool isTextchanged = false;
    DataSet dsOtherPayerInformation = new DataSet();
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
    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimOccurenceSpanInformation.Value))
                return hdnClaimOccurenceSpanInformation.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimOccurenceSpanInformation.Value = value.Trim();
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
    private DataSet dataSetOccurrenceSpanCodeInformation = new DataSet();
    public DataSet OccurenceSpanInformation
    {
        get
        {
            if (!Helper.HasRows(dataSetOccurrenceSpanCodeInformation))
            {
                if (Helper.HasRows(FetchOccurenceSpanInformation()))
                {
                    return dataSetOccurrenceSpanCodeInformation;
                }
            }
            return dataSetOccurrenceSpanCodeInformation;
        }
        set
        {
            if (value != null)
            {
                GetOccurenceSpanInformationData(value);
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
            SetButtonVisibility();
        }
    }
    public void GetOccurenceSpanInformationData(DataSet dsOtherPayerInfo)
    {
         dsOtherPayerInformation = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimOccurenceSpanInformation.Value))
        {
            if (Helper.HasRows(dsOtherPayerInfo))
            {
                dsOtherPayerInformation = dsOtherPayerInfo;
            }
            else
            {
                dsOtherPayerInformation = FetchOccurenceSpanInformation();
            }
        }
        if (Helper.HasRows(dsOtherPayerInformation))
        {
            DataTable dtOccurrenceData = new DataTable();
          
            dtOccurrenceData = dsOtherPayerInformation.Tables[0];
            
           
            if (dtOccurrenceData.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Span Code</th><th style='width:10px; scope='col'>From Date</th><th style='width:10px; scope='col'>To Date</th><th style='width:30px; scope='col'>Occurrance Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtOccurrenceData.Rows)
                {
                    string Line = dr["Line"].ToString();
                    string OccurrenceSpanCode = dr["Occurrence_Code_Span"].ToString();
                    string FromDate = dr["FromDate"].ToString();
                    string ToDate = dr["ToDate"].ToString();
                    string Claims_Occurrence_Code_Span_Information_ID = dr["Claims_Occurrence_Code_Span_Information_ID"].ToString();

                    string SpanCodeDescription = dr["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                    if (string.IsNullOrEmpty(SpanCodeDescription))
                    {
                        var dsOC = LookupTableController.GetOccurreneceCode(SpanCodeDescription, "");
                        if (dsOC != null && Helper.HasRows(dsOC))
                        {
                            var dtOC = dsOC.Tables[0];
                            if (dtOC.Rows.Count > 0)
                            {
                                DataRow row = dtOC.Rows[0];
                                lblspanOccurrenceDesc.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();

                            }
                        }
                    }
                    if (Session["ClaimStatus"].ToString() == "Pending Submission")
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Span Code' class='tNumber'>" + OccurrenceSpanCode + "</span></td><td><span  title='FromDate' class='tNumber'>" + Convert.ToDateTime(FromDate).ToString("MM/dd/yyyy") + "</span></td><td><span title='ToDate' class='tNumber'>" + Convert.ToDateTime(ToDate).ToString("MM/dd/yyyy") + "</span></td><td><span title='ToDate' class='tNumber'>" + SpanCodeDescription + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + hdnClaimOccurenceSpanInformation.Value + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceSpanItem(\"" + Claims_Occurrence_Code_Span_Information_ID + "\",\"" + hdnClaimOccurenceSpanInformation.Value + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                    }
                    else
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span title='Occurrence Span Code' class='tNumber'>" + OccurrenceSpanCode + "</span></td><td><span  title='FromDate' class='tNumber'>" + FromDate + "</span></td><td><span title='ToDate' class='tNumber'>" + ToDate + "</span></td><td><span title='ToDate' class='tNumber'>" + SpanCodeDescription + "</span></td></tr >";
                    }
                }
                table = table + "</tbody></table>";
                divOccurrenceSpan.InnerHtml = table;
               
            }
        }
        else
        {            
            divOccurrenceSpan.InnerHtml = "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // GetOccurenceSpanInformationData(null);
            ClearOccurenceSpanInfoControls();
        }
        SetButtonVisibility();      
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            Occuerance.Visible = false;
        }
        else
        {
            Occuerance.Visible = true;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!isTextchanged)
        {
            if (!string.IsNullOrEmpty(ucSearchOccurrenceSpan.OccurenceSpanCode))
            {
                txtOccurrenceSpanCode.Text = ucSearchOccurrenceSpan.OccurenceSpanCode;
                lblspanOccurrenceDesc.Text = ucSearchOccurrenceSpan.OccurenceSpanCodeDescription.Trim();
            }
        }

        if (!string.IsNullOrEmpty(ucSearchOccurrenceSpan.OccurenceSpanCode))
        {
            DataSet dsSearchOccurenceCodeDesc;
            dsSearchOccurenceCodeDesc = GetClaimOccurenceDescription();
        }
        if (isChecked)
        {
            txtOccurrenceSpanCode.Text = "";
            txtOccurrenceSpanCode.Text = string.Empty;            
        }
        isChecked = false;
        isTextchanged = false;
    }
    public bool CanUserViewDelete()
    {
        return true;
    }  

    protected void txtCheckFormDate_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtoccFromDate.Text))
        {
           // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "tmp", "<script type='text/javascript'>validateDateFromDate();</script>", false);
        }
        else
        {
            OccSpanInfoDateRequiredError1.Visible = false;
        }
        if (!string.IsNullOrEmpty(txtOccurrenceSpanCode.Text))
        {
            var dsOC = LookupTableController.GetOccurreneceCode(txtOccurrenceSpanCode.Text.Trim(), "");
            if (dsOC != null && Helper.HasRows(dsOC))
            {
                var dtOC = dsOC.Tables[0];
                if (dtOC.Rows.Count > 0)
                {
                    DataRow row = dtOC.Rows[0];
                    lblspanOccurrenceDesc.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();

                }
            }

        }
        else
        {
            lblspanOccurrenceDesc.Text = "";
        }
    }
    private DataSet FetchOccurenceSpanInformation()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsOccurenceInformation = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimOccurenceSpanInformation.Value))
        {
            parms.Add("Claim_ID", hdnClaimOccurenceSpanInformation.Value.ToString());
            dsOccurenceInformation = dataSetOccurrenceSpanCodeInformation = svc.SelectPanelsData("Claims_Occurrence_Code_Span_Information", parms);
        }
        return dsOccurenceInformation;
    }   
    protected DataSet GetClaimOccurenceDescription()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsOccurenceCodeDescription;
        parms.Add("CLAIMS_OCCURRENCE_CODE", txtOccurrenceSpanCode.Text);
        dsOccurenceCodeDescription = svc.SelectPanelsData("CLAIMS_OCCURRENCE_CODE", parms);
        return dsOccurenceCodeDescription;

    }
    
    public void ClearOccurenceSpanInfoControls()
    {
        txtOccurrenceSpanCode.Text = string.Empty;
        //lblspanOccurrenceDesc.Text = string.Empty;
        txtoccFromDate.Text = string.Empty;
        txtoccToDate.Text = string.Empty;
        dsOtherPayerInformation.Clear();
        divOccurrenceSpan.InnerHtml = "";

    }

    public void ClearGrid()
    {        
    }
    protected void lnkOccurenceSpanSearch_Click(object sender, EventArgs e)
    {
        mpeOccurenceSPanSearchPop.Show();
    }
    protected void lnkOccurenceSpanInfoSearch_Click(object sender, EventArgs e)
    {
        mpeOccurenceSPanSearchPop.Show();
    }  

    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Line", dt.Rows[i]["Line"].ToString());
                parms.Add("Claim_ID", hdnClaimOccurenceSpanInformation.Value);
                parms.Add("Occurrence_Code_Span", dt.Rows[i]["Occurrence_Code_Span"].ToString());
                parms.Add("FromDate", dt.Rows[i]["FromDate"].ToString());
                parms.Add("ToDate", dt.Rows[i]["ToDate"].ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                try
                {
                    svc.InsertPanelsData("Claims_Occurrence_Code_Span_Information", parms);
                }
                
                catch (Exception ex) { }
            }
            GetOccurenceSpanInformationData(null);
        }
    }
    public void ClearOccurrenceSpanDescription()
    {
        txtOccurrenceSpanCode.Text = string.Empty;
        lblspanOccurrenceDesc.Text = string.Empty;
    }
}