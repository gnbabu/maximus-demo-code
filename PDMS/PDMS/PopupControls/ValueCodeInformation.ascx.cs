using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ValueCodeInformation : System.Web.UI.UserControl
{

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

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCode_ClaimType.Value))
                return hdnValueCode_ClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCode_ClaimType.Value = value.Trim();
        }
    }

    public string ValueCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCode.Value))
                return hdnValueCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCode.Value = value.Trim();
        }
    }
    public string ValueCodeDescription
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnValueCodeDesc.Value))
                return hdnValueCodeDesc.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnValueCodeDesc.Value = value.Trim();
        }
    }

    DataSet dsValueCodeInformation = new DataSet();

    public DataSet ValueCodeInformation
    {
        get
        {
            if (!Helper.HasRows(dsValueCodeInformation))
            {
                if (Helper.HasRows(GetValueCodeInformations()))
                {
                    return dsValueCodeInformation;
                }
            }
            return dsValueCodeInformation;
        }
        set
        {
            if (value != null)
            {
                GetValueCodeInformationData(value);
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


    private void GetValueCodeInformationData(DataSet dsValueCodeInfo)
    {
        hdnValueCodeClaimStatus.Value = "";
        DataSet dsValueCodeInformation = new DataSet();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            if (Helper.HasRows(dsValueCodeInfo))
            {
                dsValueCodeInformation = dsValueCodeInfo;
            }
            else
            {
                dsValueCodeInformation = GetValueCodeInformations();
            }
        }
        if (Helper.HasRows(dsValueCodeInformation))
        {
            DataTable dtValuecode = dsValueCodeInformation.Tables[0];
            if (dtValuecode.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Value Code</th><th style='width:10px; scope='col'>Amount</th><th style='width:30px; scope='col'>Value Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtValuecode.Rows)
                {
                    string Line = dr["Line"].ToString();
                    string VALUE_CODE = dr["VALUE_CODE"].ToString();
                    string Amount = dr["Amount"].ToString();
                    string CLAIMS_VALUE_CODE_DESC = dr["CLAIMS_VALUE_CODE_DESC"].ToString();
                    string Claims_Value_Code_Information_ID = dr["Claims_Value_Code_Information_ID"].ToString();

                    string Claim_ID = dr["Claim_ID"].ToString();
                    if (string.IsNullOrEmpty(CLAIMS_VALUE_CODE_DESC))
                    {
                        
                        var dsOC = GetValueCodeDescription();
                        if (dsOC != null && Helper.HasRows(dsOC))
                        {
                            var dtOC = dsOC.Tables[0];
                            if (dtOC.Rows.Count > 0)
                            {
                                DataRow row = dtOC.Rows[0];
                                lblValueCodeDesc.Text = row["CLAIMS_VALUE_CODE_DESC"].ToString().Trim();

                            }
                        }
                    }
                    if (Session["ClaimStatus"].ToString() == "Pending Submission")
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='VALUE CODE' class='tNumber'>" + VALUE_CODE + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Value Code Description' class='tNumber'>" + CLAIMS_VALUE_CODE_DESC + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + hdnValueCode_ClaimID.Value + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteValuecode(\"" + Claims_Value_Code_Information_ID + "\",\"" + hdnValueCode_ClaimID.Value + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        hdnValueCodeClaimStatus.Value = "Pending Submission";
                    }
                    else
                    {
                        table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='VALUE CODE' class='tNumber'>" + VALUE_CODE + "</span></td><td><span title='Amount' class='tNumber'>" + Amount + "</span></td><td><span title='Value Code Description' class='tNumber'>" + CLAIMS_VALUE_CODE_DESC + "</span></td></tr >";
                        hdnValueCodeClaimStatus.Value = "Other";
                    }
                }
                table = table + "</tbody></table>";
                divValueCodeInformation.InnerHtml = table;

            }

        }
        else
        {
            divValueCodeInformation.InnerHtml = "";
        }
    }

    #endregion

    private DataSet dsValueCodeInformations = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {           
            BindValueCodeInformationGrid(null);
        }
        else
        {         
            GetValueCodeInformationData(null);
        }        
        SetButtonVisibility();
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divValue.Visible = false;
        }
        else
        {
            divValue.Visible = true;
        }
    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ValueCode))
        {
            txtValueCode.Text = hdnValueCode.Value;
            lblValueCodeDesc.Text = hdnValueCodeDesc.Value;
        }      
        if (!string.IsNullOrEmpty(txtValueCode.Text))
        {
            hdnValueCode.Value = string.Empty;
            hdnValueCodeDesc.Value = string.Empty;
        }
    }

    protected DataSet GetValueCodeDescription()
    {

        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsValueCodeDescription;
        parms.Add("CLAIMS_VALUE_CODE", txtValueCode.Text.ToString());

        parms.Add("CLAIMS_VALUE_CODE_DESC", null);
        dsValueCodeDescription = svc.SelectPanelsData("CLAIMS_VALUE_CODE", parms);
        return dsValueCodeDescription;

    }

    private DataSet GetValueCodeInformation()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsValueCodeInformation = new DataSet();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            parms.Add("Claim_ID", hdnValueCode_ClaimID.Value.ToString());
            dsValueCodeInformation = svc.SelectPanelsData("Claims_Value_Code_Information", parms);
        }
        return dsValueCodeInformation;
    }

    public bool CanUserViewDelete()
    {
        return true;
    }       
    public void ClearValueCodePanel()
    {

        txtValueCode.Text = string.Empty;
        txtAmount.Text = string.Empty;
        lblValueCodeDesc.Text = string.Empty;
        //hdnValueCode.Value = string.Empty;
        hdnValueCodeDesc.Value = string.Empty;
        lblErrorValueCodePanel.Text = "";
    }   

    private void BindValueCodeInformationGrid(DataSet dsValueCodeData)
    {
        DataSet dsValueCodeInformation = new DataSet();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            if (Helper.HasRows(dsValueCodeData))
            {
                dsValueCodeInformation = dsValueCodeData;
            }
            else
            {
                dsValueCodeInformation = GetValueCodeInformations();

            }
            GetValueCodeInformationData(dsValueCodeInformation);
        }

        if (Helper.HasRows(dsValueCodeInformation) && Convert.ToInt32(dsValueCodeInformation.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnValueCode_ClaimID.Value))
        {
          
        }
        else
        {            
            gvValueCodeSearchPop.DataSource = null;
            gvValueCodeSearchPop.DataBind();
        }
    }

    private DataSet GetValueCodeInformations()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsValueCode = new DataSet();
        if (!string.IsNullOrEmpty(hdnValueCode_ClaimID.Value))
        {
            parms.Add("Claim_ID", hdnValueCode_ClaimID.Value);
            dsValueCodeInformation = dsValueCodeInformations = dsValueCode = svc.SelectPanelsData("Claims_Value_Code_Information", parms);
        }
        return dsValueCode;
    }


    protected void grdValueCodeInformationSearch_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void ValueCodeInformationSearchData()
    {


    }

    
    protected void OpenSearchPopUp(object sender, EventArgs e)
    {
        mpeSubmitClaimSearchValueCode.Show();
    }

    protected void txtValue_Code_TextChangedNPI2(object sender, EventArgs e)

    {

        lblErrorValueCodePanel.Text = "";
        DataSet dsValueCode;
        dsValueCode = GetValueCodeDescription();
        if (dsValueCode.Tables[0].Rows.Count != 0)
        {
            lblValueCodeDesc.Text = dsValueCode.Tables[0].Rows[0][1].ToString();
            txtValueCode.Text = dsValueCode.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            lblErrorValueCodePanel.Text = "Value Code is invalid";
            lblValueCodeDesc.Text = "";
        }

    }


    protected void gvValueCodeSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }


    protected void gvValueCodeSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }

    protected void lnkValueCodeSpan_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        txtCode.Text = string.Empty;
        txtValueDesc.Text = string.Empty;

        DataSet dsValueCodeDescription = svc.GetValueCodeDescription(cmdArgument);

        hdnValueCode.Value = cmdArgument.ToString();
        hdnValueCodeDesc.Value = dsValueCodeDescription.Tables[0].Rows[0]["CLAIMS_VALUE_CODE_DESC"].ToString();


    }

    public void RefreshData()
    {
        DataTable dt = GetData(gvValueCodeSearchPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvValueCodeSearchPop.DataSource = dt;
            gvValueCodeSearchPop.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvValueCodeSearchPop.GridViewSortDirection == SortDirection.Descending ? gvValueCodeSearchPop.GridViewSortColumn + " DESC" : gvValueCodeSearchPop.GridViewSortColumn;
        DataSet ds = GetValueCodeData();

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private DataSet GetValueCodeData()
    {
        DataSet dsSpanCode;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("CLAIMS_VALUE_CODE", txtCode.Text);
        parms.Add("CLAIMS_VALUE_CODE_DESC", txtValueDesc.Text);
        dsSpanCode = svc.SelectPanelsData("CLAIMS_VALUE_CODE", parms);
        return dsSpanCode;
    }

    protected void btnSearch_Click2(object sender, EventArgs e)
    {
        if (!((txtCode.Text == "") && (txtValueDesc.Text == "")))
        {

            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;

            this.gvValueCodeSearchPop.CurrentPageIndex = 0;
            RefreshData();
            fieldRequireError.Text = "";
            mpeSubmitClaimSearchValueCode.Show();

        }
        else
        {
            fieldRequireError.Text = "Value code and/or description is required";
            mpeSubmitClaimSearchValueCode.Show();
        }
    }  

    public void ClearGrid()
    {
     
    }

    public void SaveToDbOnAdjust(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Line", dt.Rows[i]["Line"].ToString());
                parms.Add("Value_Code", dt.Rows[i]["Value_Code"].ToString());
                parms.Add("Amount", dt.Rows[i]["Amount"].ToString());
                parms.Add("Claim_ID", hdnValueCode_ClaimID.Value);
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                try
                {
                    svc.InsertPanelsData("Claims_Value_Code_Information", parms);
                }
                catch (Exception ex)
                { }

            }
            BindValueCodeInformationGrid(null);
        }
    }
    public void ClearValueCodeInfoDescription()
    {
        txtValueCode.Text = string.Empty;
        lblValueCodeDesc.Text = string.Empty;
    }
}