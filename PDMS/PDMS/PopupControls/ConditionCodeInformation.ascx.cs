using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class PopupControls_ConditionCodeInformation : System.Web.UI.UserControl
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
            if (!string.IsNullOrWhiteSpace(hdnConditionClaimID.Value))
                return hdnConditionClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnConditionClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnConditionCodeClaimType.Value))
                return hdnConditionCodeClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnConditionCodeClaimType.Value = value.Trim();
        }
    }
    private DataSet dsConditionCodeInformation = new DataSet();

    public DataSet datasetConditionCode
    {
        get
        {
            if (!Helper.HasRows(dsConditionCodeInformation))
            {
                if (Helper.HasRows(GetConditionCodeInformationData()))
                {
                    return dsConditionCodeInformation;
                }
            }
            return dsConditionCodeInformation;
        }
        set
        {
            if (value != null)
            {
                GetConditionCodeInformation(value);
            }
        }
    }
    public void GetConditionCodeInformation(DataSet dsConditionCodeDatas)
    {
        DataSet dsConditionCode = new DataSet();
        if (!string.IsNullOrEmpty(hdnConditionClaimID.Value))
        {
            if (Helper.HasRows(dsConditionCodeDatas))
            {
                dsConditionCode = dsConditionCodeDatas;
            }
            else
            {
                dsConditionCode = GetConditionCodeInformationData();
            }
        }
        if (Helper.HasRows(dsConditionCode))
        {
            DataTable dtOccurrenceData = new DataTable();

            dtOccurrenceData = dsConditionCode.Tables[0];


            if (dtOccurrenceData.Rows.Count > 0)
            {
                string table = string.Empty;

                table = "<table class='gridview'  cellspacing='0' align='Middle' rules='rows' border='1' style='margin-left: 40px; float: left;width:90%;border-collapse:collapse;margin-right: 0px;margin-top: 0px;'><tbody><tr class='gridViewHeader'><th style='width:10px; scope='col'>Line</th><th style='width:10px; scope='col'>Condition Code</th><th style='width:10px; scope='col'>Condition Code Description</th><th style='width:10px;' scope='col'>&nbsp;</th><th style='width:10px;' scope='col'>&nbsp;</th></tr>";

                foreach (DataRow dr in dtOccurrenceData.Rows)
                {
                    string Line = dr["Line"].ToString();
                    string ConditionCode = dr["Condition_Code"].ToString();
                    string ConditionCodeDesc = dr["Claims_Condition_Code_Description"].ToString();
                    string Claims_Condition_Code_Information_ID = dr["Claims_Condition_Code_Information_ID"].ToString();
                    string ClaimID = hdnConditionClaimID.Value;
                    if (string.IsNullOrEmpty(ConditionCodeDesc))
                    {
                        var dsOC = svc.GetConditionCodeDescription(ConditionCode);
                        if (dsOC != null && Helper.HasRows(dsOC))
                        {
                            var dtOC = dsOC.Tables[0];
                            if (dtOC.Rows.Count > 0)
                            {
                                DataRow row = dtOC.Rows[0];
                                lblConditionDescription.Text = row["CLAIMS_OCCURRENCE_CODE_DESC"].ToString().Trim();

                            }
                        }
                    }
                    if (Session["ClaimStatus"] != null)
                    {
                        if (Session["ClaimStatus"].ToString() == "Pending Submission")
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Condition Code' class='tNumber'>" + ConditionCode + "</span></td><td><span title='Condition Description' class='tNumber'>" + ConditionCodeDesc + "</span></td><td><input type='button' value = 'Edit' onClick = 'return EditConditioncode(\"" + Claims_Condition_Code_Information_ID + "\",\"" + ClaimID + "\"); return true;' class='btn btn-primary' sytle = 'margin-left:3px'></td><td><input type='button' value='Delete' onclick='return DeleteConditioncode(\"" + Claims_Condition_Code_Information_ID + "\");' class='btn btn-danger' sytle='margin-left:3px'></td></tr >";
                        }
                        else
                        {
                            table = table + "<tr style='border: 1px solid black; border-collapse: collapse;' class='gridViewRow'><td><span title='Line' class='tNumber'>" + Line + "</span></td><td><span  title='Condition Code' class='tNumber'>" + ConditionCode + "</span></td><td><span title='Condition Description' class='tNumber'>" + ConditionCodeDesc + "</span></td></tr >";
                        }
                    }

                }
                table = table + "</tbody></table>";
                divConditionCodeInformation.InnerHtml = table;

            }
        }
        else
        {            
            divConditionCodeInformation.InnerHtml = "";
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

        txtConditionCode.Text = ucConditionCodeSearch.Condition_Code;
        lblConditionDescription.Text = ucConditionCodeSearch.Condition_Code_Description;        
        if (!string.IsNullOrEmpty(txtConditionCode.Text))
        {
            ucConditionCodeSearch.Condition_Code = string.Empty;
            ucConditionCodeSearch.Condition_Code_Description = string.Empty;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {       
        SetButtonVisibility();
        // GetConditionCodeInformation(null);
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            divcon.Visible = false;
        }
        else
        {
            divcon.Visible = true;
        }
    }
    public void GetConditionCodeDescription()
    {
        lblConditionCodemessage.Text = "";
        DataSet dsConditionCodeDescription = new DataSet();
        if (!string.IsNullOrEmpty(txtConditionCode.Text.ToString()))
        {
            bool isAlphaBet = Regex.IsMatch(txtConditionCode.Text.ToString(), "[a-z]", RegexOptions.IgnoreCase);
            if (!isAlphaBet)
            {
                int ConditionCode = Convert.ToInt32(txtConditionCode.Text);
                dsConditionCodeDescription = svc.GetConditionCodeDescription(ConditionCode.ToString("00"));
                txtConditionCode.Text = ConditionCode.ToString("00");
            }
            else
            {
                txtConditionCode.Text = txtConditionCode.Text.ToString().ToUpper();
                dsConditionCodeDescription = svc.GetConditionCodeDescription(txtConditionCode.Text.ToString());
            }

            if (Helper.HasRows(dsConditionCodeDescription))
            {
                lblConditionDescription.Text = dsConditionCodeDescription.Tables[0].Rows[0]["CLAIMS_CONDITION_CODE_DESCRIPTION"].ToString();
            }
            else
            {
                lblConditionCodemessage.Text = "Condition code is invalid";
                lblConditionDescription.Text = string.Empty;
                txtConditionCode.Text = "";
                return;
            }
        }
    }

    public void ClearConditionCodeDescription()
    {
        txtConditionCode.Text = string.Empty;
        lblConditionDescription.Text = string.Empty;
    }

    private void BindConditionCodeInfoGrid(DataSet dsCode)
    {
        DataSet dsConditionCode = new DataSet();
        if (!string.IsNullOrEmpty(hdnConditionClaimID.Value) || !string.IsNullOrEmpty(ICN))
        {
            if (Helper.HasRows(dsCode))
            {
                dsConditionCode = dsCode;
            }
            else
            {
                dsConditionCode = GetConditionCodeInformationData();
            }
        }      

    }

    private DataSet GetConditionCodeInformationData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet dsConditionCodeDetail = new DataSet();
        if (!string.IsNullOrEmpty(hdnConditionClaimID.Value))
        {
            parms.Add("Claim_ID", hdnConditionClaimID.Value);
            dsConditionCodeInformation = dsConditionCodeDetail = svc.SelectPanelsData("Claims_Condition_Code_Information", parms);
        }
        return dsConditionCodeDetail;
    }
    public void ClearConditionCodeGrid()
    {
       
    }  

    protected void txtConditionCode_TextChanged(object sender, EventArgs e)
    {
        GetConditionCodeDescription();
        ucConditionCodeSearch.Condition_Code_Description = lblConditionDescription.Text;
        ucConditionCodeSearch.Condition_Code = txtConditionCode.Text;

    }

    protected void lnkConditionCodeSearch_Click(object sender, EventArgs e)
    {
        mpeConditionCodeSearch.Show();
    }   

    public void SaveToDbOnAdjust(DataTable dt)
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, hdnConditionClaimID.Value, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_Clear_Condition_Code_Information", parameters);

        if (Helper.HasRows(dt))
        {
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Line", dt.Rows[i]["Line"].ToString()) ;
                parms.Add("Condition_Code", dt.Rows[i]["Condition_Code"].ToString());
                parms.Add("Claim_ID", hdnConditionClaimID.Value);
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    
                svc.InsertPanelsData("Claims_Condition_Code_Information", parms);
            }
            GetConditionCodeInformation(null);
        }
    }
}