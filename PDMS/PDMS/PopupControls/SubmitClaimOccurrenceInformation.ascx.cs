using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System.Data.SqlClient;
using System.Linq;
using Amazon.Runtime.Internal.Transform;

public partial class PopupControls_SubmitClaimOccurrenceInformation : BasePopupControl
{
    #region spa
    private PDMSService.PDMSServiceClient _spa;

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
    #endregion

    private int _ClaimId;

    private const int maxRecordsAllowed = 24;
    private DataSet _occurenceInfo;
    private string _ICN = string.Empty;
    private bool _displayReadOnly = false;
    public DataSet OccurrenceInformation
    {
        get
        {
            if (_occurenceInfo == null)
            {
                _occurenceInfo = GetClaimOccurrenceInfoByClaimID();
            }

            return _occurenceInfo;
        }
        set
        {
            _occurenceInfo = value;
            SetOccurence(value);
        }
    }
    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;
            //BindGrid();
            SetButtonVisibility();
        }
    }
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

    public string ICN
    {
        get
        {
            return _ICN;
        }
        set
        {
            _ICN = value;
        }
    }
    public bool ShowOccuurenceInforimation
    {
        set
        {
             OccurrenceIn.Visible = true;
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtOccurrenceCode.Text) || !string.IsNullOrEmpty(txtOccurenceDate.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);
        BindGrid();
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
       // OccurrenceInfoDataEntry.Visible = DisplayAdd();
        if (!IsPostBack)
        {
            BindGrid();
            gvOccurrenceCodeSearch.DataSource = null;
            gvOccurrenceCodeSearch.DataBind();
            // SetOccurence(null);
        }
        else
        {
           // btnAdd.Attributes.Add("onclick", "DisableEnableOccurenceInformationAddButton();");

        }
        SetButtonVisibility();
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            OccurrenceIn.Visible = false;
        }
        else
        {
            OccurrenceIn.Visible = true;
        }
    }
    protected bool DisplayAdd()
    {
        int rows = 0;
        bool canshow = true;
        if (Helper.HasRows(OccurrenceInformation))
        {
            rows = OccurrenceInformation.Tables[0].Rows.Count;
            canshow = (rows < maxRecordsAllowed) ? true : false;
        }
        if (canshow) canshow = !DisplayReadOnly;
        return canshow;
    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    protected void Page_PreRender(object sender, EventArgs e)
    {
        //string txtToday = DateTime.Now.ToShortDateString();
        //cvDate.ValueToCompare = txtToday;
        if (!string.IsNullOrEmpty(hdnValueOccuranceCode.Value) && !string.IsNullOrEmpty(hdnValueOccuranceCodeDesc.Value))
        {
            txtOccurrenceCode.Text = hdnValueOccuranceCode.Value;
            if(string.IsNullOrEmpty(lblOccurrenceDesc.Text) && !string.IsNullOrEmpty(hdnValueOccuranceCodeDesc.Value))
            lblOccurrenceDesc.Text = hdnValueOccuranceCodeDesc.Value;            
            if (!string.IsNullOrEmpty(txtOccurrenceCode.Text))
            {
                hdnValueOccuranceCode.Value = string.Empty;
                hdnValueOccuranceCodeDesc.Value = string.Empty;
            }
        }
        if (!string.IsNullOrEmpty(hdnValueOccuranceCode.Value) && !string.IsNullOrEmpty(hdnValueOccuranceCodeDesc.Value))
        {
            txtOccurrenceCode.Text = hdnValueOccuranceCode.Value;

            if (string.IsNullOrEmpty(lblOccurrenceDesc.Text) && !string.IsNullOrEmpty(hdnValueOccuranceCodeDesc.Value))
                lblOccurrenceDesc.Text = hdnValueOccuranceCodeDesc.Value;
            
            if (!string.IsNullOrEmpty(txtOccurrenceCode.Text))
            {
                hdnValueOccuranceCode.Value = string.Empty;
                hdnValueOccuranceCodeDesc.Value = string.Empty;
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();
        this.ValidateData();
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        return;
    }
    private void ValidateData()
    {
    }

    

    protected void ReportOccurenceDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (Helper.IsValidDate(txtOccurenceDate.Text, true) && Helper.IsValidDate(txtOccurenceDate.Text, true))
        {
            TimeSpan ts = Convert.ToDateTime(txtOccurenceDate.Text).Subtract(Convert.ToDateTime(txtOccurenceDate.Text));
            args.IsValid = (ts.Days < 366 && ts.Days > -366);
        }
        else
            args.IsValid = true;

        if (!args.IsValid)
        {
            //What is supposed to happen here?
        }
    }
    public void SetOccurence(DataSet dsOccurence)
    {      

        DataSet dsOccurences = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            if (Helper.HasRows(dsOccurence))
            {
                dsOccurences = dsOccurence;
            }
            else
            {
                dsOccurences = GetClaimOccurrenceInfoByClaimID();
            }
        }
        if (Helper.HasRows(dsOccurences))
        {
            
            DataTable diagTable = new DataTable();

            
            if (Helper.HasRows(dsOccurences))
            {
                diagTable = dsOccurences.Tables[0];
            }
            else
            {
                diagTable = dsOccurences.Tables[0];
            }
            divOccurrenceInfo.InnerHtml = "";
            if (diagTable.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Code</th><th style='width:10px; scope='col'>Occurrence Date</th><th style='width:30px; scope='col'>Occurrance Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in diagTable.Rows)
                {
                    string OccInfoLine = dr["Claims_Occurrence_Line"].ToString();
                    string OccurrenceInfoCode = dr["OccurrenceCode"].ToString();
                    string OccurrenceDate = Convert.ToDateTime(dr["OccurrenceDate"]).ToString("MM/dd/yyyy");
                    string OccurrenceDesc = dr["OccurrenceDesc"].ToString();
                    string ClaimsOccurrenceInformationID = dr["ClaimsOccurrenceInformationID"].ToString();
                    string Claim_Id = hdnClaimId.Value.ToString();
                    if (string.IsNullOrEmpty(OccurrenceDesc))
                    {
                        var dsOC = LookupTableController.GetOccurreneceCode(OccurrenceInfoCode, "");
                        if (dsOC != null && Helper.HasRows(dsOC))
                        {
                            var dtOC = dsOC.Tables[0];
                            if (dtOC.Rows.Count > 0)
                            {
                                DataRow row = dtOC.Rows[0];
                                lblOccurrenceDesc.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();
                                hdnValueOccuranceCodeDesc.Value = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                                hdnValueOccuranceCode.Value = txtOccurrenceCode.Text;
                            }
                        }
                    }
                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + OccInfoLine + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfoCode + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_Id + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + Claim_Id + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        }
                        else
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + OccInfoLine + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfoCode + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceDesc + "</span></td></tr >";
                        }
                    }
                }
                table = table + "</tbody></table>";
                divOccurrenceInfo.InnerHtml = table;
                if(diagTable.Rows.Count >= maxRecordsAllowed)
                {
                    OccurrenceIn.Visible = false;
                }
            }


        }
        else { divOccurrenceInfo.InnerHtml = ""; }
    }

    private void BindGrid()
    {
        if (Helper.HasRows(OccurrenceInformation))
        {            
            DataTable dtOccurrenceData = OccurrenceInformation.Tables[0];
            if (dtOccurrenceData.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Occurrence Code</th><th style='width:10px; scope='col'>Occurrence Date</th><th style='width:30px; scope='col'>Occurrance Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtOccurrenceData.Rows)
                {
                    string OccInfoLine = dr["Claims_Occurrence_Line"].ToString();
                    string OccurrenceInfoCode = dr["OccurrenceCode"].ToString();
                    string OccurrenceDate = Convert.ToDateTime(dr["OccurrenceDate"]).ToString("MM/dd/yyyy");
                    string OccurrenceDesc = dr["OccurrenceDesc"].ToString();
                    string ClaimsOccurrenceInformationID = dr["ClaimsOccurrenceInformationID"].ToString();
                    string ClaimId = hdnClaimId.Value.ToString();            
                    if (string.IsNullOrEmpty(OccurrenceDesc))
                    {
                        var dsOC = LookupTableController.GetOccurreneceCode(OccurrenceInfoCode, "");
                        if (dsOC != null && Helper.HasRows(dsOC))
                        {
                            var dtOC = dsOC.Tables[0];
                            if (dtOC.Rows.Count > 0)
                            {
                                DataRow row = dtOC.Rows[0];
                                lblOccurrenceDesc.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();
                                hdnValueOccuranceCodeDesc.Value = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                                hdnValueOccuranceCode.Value = txtOccurrenceCode.Text;
                            }
                        }
                    }
                    if (Session["ClaimStatus"].ToString() == "Pending Submission")
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + OccInfoLine + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfoCode + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + ClaimId + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteOccurrenceItem(\"" + ClaimsOccurrenceInformationID + "\",\"" + ClaimId + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                    }
                    else
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + OccInfoLine + "</span></td><td><span title='Occurrence Code' class='tNumber'>" + OccurrenceInfoCode + "</span></td><td><span  title='OccurrenceDate' class='tNumber'>" + OccurrenceDate + "</span></td><td><span title='OccurenceCode Description' class='tNumber'>" + OccurrenceDesc + "</span></td></tr >";
                    }
                }
                table = table + "</tbody></table>";
                divOccurrenceInfo.InnerHtml = table;

            }

        }
        else
        {            
            divOccurrenceInfo.InnerHtml = "";
        }
    }

    protected void lnkSubmitClaimOccurrenceInformation_Click(object sender, EventArgs e)
    {
        mpeSubmitOccuurenceSearch.Show();
    }
    
    protected void SubmitClaimOccurrenceInformationAdd_Click(object sender, EventArgs e)
    {
        Page.Validate("newOccurrence");
        if (Page.IsValid)
        {
            if ((Convert.ToDateTime(txtOccurenceDate.Text)) >= DateTime.Now)
            {
                OccInfoDateRequiredError1.Text= "Future Date not allowed.";
            }
            else if(string.IsNullOrEmpty(lblOccurrenceInfomessage.Text))
            {
                OccInfoDateRequiredError1.Text = "";
                string occurrencecode = txtOccurrenceCode.Text.Trim();
                string occDate = txtOccurenceDate.Text.Trim();
                int rows = 0;
                string line = string.Empty;
                if (Helper.HasRows(OccurrenceInformation))
                {
                    rows = OccurrenceInformation.Tables[0].Rows.Count;
                }
                line = (rows + 1).ToString();
                DateTime dtoccurrence = string.IsNullOrEmpty(occDate) ? DateTime.Now : Convert.ToDateTime(occDate);
                SaveOccurrenceInformation(occurrencecode, line, dtoccurrence);
                ClearOccurrenceCodeDataEntry();
            }
        }
    }

   

    private DataSet GetClaimOccurrenceInfoByClaimID()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsOccInfo = new DataSet();
        if (!string.IsNullOrEmpty(hdnClaimId.Value))
        {
            parms.Add("Claim_ID", hdnClaimId.Value);
            dsOccInfo = spa.SelectPanelsData("Claims_Occurrence_Information", parms);
        }
        return dsOccInfo;
    }
    private void DeleteOccurrenceInfo(int Id)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claims_Occurrence_Information_ID", Id.ToString());
        spa.DeletePanelsData("usp_delete_claims_occurrence_information", "Claims_Occurrence_Information_ID", Id);
        _occurenceInfo = null;
    }
    private void SaveOccurrenceInformation(string occurrenceCode, string line, DateTime dtoccurrence, int occurrenceId = 0, bool isEdit = false)
    {
        if (!string.IsNullOrEmpty(ClaimId))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", ClaimId);
            parms.Add("Claims_Occurrence_Line", line);
            parms.Add("Claims_Occurrence_Code", occurrenceCode);
            parms.Add("Claims_Occurrence_Date", dtoccurrence.ToString());

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (!isEdit)
            {
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                spa.InsertPanelsData("Claims_Occurrence_Information", parms);
            }
            else
            {
                parms.Add("Claims_Occurrence_Information_ID", occurrenceId.ToString());
                spa.UpdatePanelsData("Claims_Occurrence_Information", parms);
            }
        }
        _occurenceInfo = null;
        BindGrid();
    } 
    protected void lnkOccurencesrch_Click(object sender, EventArgs e)
    {
        mpeSubmitOccuurenceSearch.Show();
    }

    protected void gvOccurrenceCodeSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOccurrenceCodeSearch.PageIndex = e.NewPageIndex;

        gvOccurrenceCodeSearch.EditIndex = -1;
    }
    protected void BtnSearchOccurrence_Click(object sender, EventArgs e)
    {
        if ((string.IsNullOrEmpty(txtSearchoOccuCode.Text)) && (string.IsNullOrEmpty(txtSearchOccurrenceDesc.Text)))
        {
            lblError.Visible = true;
            lblError.Text = "Occurrence code and Occurrence code description  is required for Search.";
            mpeSubmitOccuurenceSearch.Show();
            txtSearchoOccuCode.Text = "";
            txtSearchOccurrenceDesc.Text = "";
            gvOccurrenceCodeSearch.DataSource = null;
            gvOccurrenceCodeSearch.DataBind();
        }
        else
        {
            lblError.Visible = false;
            string code = txtSearchoOccuCode.Text.Trim();
            string codedesc = txtSearchOccurrenceDesc.Text.Trim();
            DataSet dsOccurrecnce = spa.GetOccurreneceCode(code, codedesc);
            if (Helper.HasRows(dsOccurrecnce))
            {
                gvOccurrenceCodeSearch.DataSource = dsOccurrecnce;
                gvOccurrenceCodeSearch.DataBind();
            }
            else {
                gvOccurrenceCodeSearch.DataSource = null;
                gvOccurrenceCodeSearch.DataBind();
            }
        }
        mpeSubmitOccuurenceSearch.Show();
    }

    public void ClearOccurrenceCodeDataEntry()
    {
        txtOccurrenceCode.Text = string.Empty;
        txtOccurenceDate.Text = string.Empty;
        lblOccurrenceDesc.Text = string.Empty;
        divOccurrenceInfo.InnerHtml = "";
    }

    public void ClearGrid()
    {        
    }


    protected void lnkoccurrencecode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
        string occurCode = commandArgs[0];
        string occurDesc = commandArgs[1];
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        txtOccurrenceCode.Text = occurCode;
        lblOccurrenceDesc.Text = occurDesc;
        hdnValueOccuranceCode.Value = occurCode;
        hdnValueOccuranceCodeDesc.Value = occurDesc;
        txtSearchoOccuCode.Text = "";
        txtSearchOccurrenceDesc.Text = "";
        gvOccurrenceCodeSearch.DataSource = null;
        gvOccurrenceCodeSearch.DataBind();
        if (mpeSubmitOccuurenceSearch != null)
            mpeSubmitOccuurenceSearch.Hide();
    }

    protected void txtOccurrenceCode_TextChanged(object sender, EventArgs e)
    {
        lblOccurCodeError.Text = "";
        if (!string.IsNullOrEmpty(txtOccurrenceCode.Text))
        {
            var dsOC = LookupTableController.GetOccurreneceCode(txtOccurrenceCode.Text.Trim(), "");
            if (dsOC != null && Helper.HasRows(dsOC))
            {
                var dtOC = dsOC.Tables[0];
                if (dtOC.Rows.Count > 0)
                {
                    DataRow row = dtOC.Rows[0];
                    lblOccurrenceDesc.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();
                    hdnValueOccuranceCodeDesc.Value =row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString();
                    hdnValueOccuranceCode.Value = txtOccurrenceCode.Text;
                }
            }
            else
            {
                lblOccurCodeError.Text = "Incorrect Occurrence Code";
            }
        }
        else
        {
            lblOccurrenceDesc.Text = "";
        }
    }    
    public void SavetoDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt)) { 
                for(int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", ClaimId);
                parms.Add("Claims_Occurrence_Line", dt.Rows[i]["Claims_Occurrence_Line"].ToString());
                parms.Add("Claims_Occurrence_Code", dt.Rows[i]["OccurrenceCode"].ToString());
                parms.Add("Claims_Occurrence_Date", dt.Rows[i]["OccurrenceDate"].ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                try
                {
                    spa.InsertPanelsData("Claims_Occurrence_Information", parms);
                }catch(Exception ex)
                {

                }
                
                
            }
            SetOccurence(null);

        }
    }

    public void ClearOccurrenceDescription()
    {
        txtOccurrenceCode.Text = string.Empty;
        lblOccurrenceDesc.Text = string.Empty;
    }
}