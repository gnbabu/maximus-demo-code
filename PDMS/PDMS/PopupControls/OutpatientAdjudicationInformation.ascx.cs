using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_OutpatientAdjudicationInformation : System.Web.UI.UserControl
{
    #region Poperties
    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOutpatientAdjudicationInfo.Value))
                return hdnOutpatientAdjudicationInfo.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOutpatientAdjudicationInfo.Value = value.Trim();
        }
    }
    public string HCPCSPayableAmount
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtHCPCSPayableAmt.Text))
                return txtHCPCSPayableAmt.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtHCPCSPayableAmt.Text = value.Trim();
        }

    }
    public string ClaimRemarkCodeMOA04
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtClaimRemarkCodeMOA04.Text))
                return txtClaimRemarkCodeMOA04.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtClaimRemarkCodeMOA04.Text = value.Trim();
        }

    }
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
        }
    }

    public string ReimbursementRate
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtReimbursementRate.Text))
                return txtReimbursementRate.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtReimbursementRate.Text = value.Trim();
        }

    }
    public string ClaimRemarkCodeMOA03
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtClaimRemarkCodeMOA03.Text))
                return txtClaimRemarkCodeMOA03.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtClaimRemarkCodeMOA03.Text = value.Trim();
        }

    }
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
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtReimbursementRate.Text) || !string.IsNullOrEmpty(txtClaimRemarkCodeMOA03.Text) || !string.IsNullOrWhiteSpace(txtClaimRemarkCodeMOA04.Text)
            || !string.IsNullOrWhiteSpace(txtHCPCSPayableAmt.Text))
        {
            rtn = true;
        }

        return rtn;
    }
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            GetOutpatientAdjudicationDetails();
        }
    }  
    public void GetOutpatientAdjudicationDetails()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_id", hdnOutpatientAdjudicationInfo.Value);
        try
        {
            DataSet dsOutpatientInfo = svc.SelectPanelsData("Claims_Outpatient_Adjudication_Information", parms);
            if (Helper.HasRows(dsOutpatientInfo) &&
    Convert.ToInt32(dsOutpatientInfo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOutpatientAdjudicationInfo.Value))
            {
                txtReimbursementRate.Text = dsOutpatientInfo.Tables[0].Rows[0]["Reimbursement_Rate"].ToString();
                txtClaimRemarkCodeMOA03.Text = dsOutpatientInfo.Tables[0].Rows[0]["Claim_Code_MOA03"].ToString();
                txtHCPCSPayableAmt.Text = dsOutpatientInfo.Tables[0].Rows[0]["HCPCS_PayableAmount"].ToString();
                txtClaimRemarkCodeMOA04.Text = dsOutpatientInfo.Tables[0].Rows[0]["Claim_Code_MOA04"].ToString();
                hdnAdjudicationInformationID.Value = dsOutpatientInfo.Tables[0].Rows[0]["Claims_Outpatient_Adjudication_Information_ID"].ToString();
            }
            //isLoaded = true;


        }
        catch (Exception ex) { }


    }
    public void SaveOutpatientAdjudicationDetails()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnOutpatientAdjudicationInfo.Value.ToString());
        DataSet dsOutpatientAdjInfo = svc.SelectPanelsData("Claims_Outpatient_Adjudication_Information", parms);
        if (Helper.HasRows(dsOutpatientAdjInfo) &&
            Convert.ToInt32(dsOutpatientAdjInfo.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnOutpatientAdjudicationInfo.Value))
        {
            if (txtReimbursementRate.Text.Length > 10)
            {
                parms.Add("Reimbursement_Rate", txtReimbursementRate.Text.ToString().Substring(0, 10));
            } else
            {
                parms.Add("Reimbursement_Rate", txtReimbursementRate.Text.ToString());
            }
            decimal payableAmount = 0;
            if (Decimal.TryParse(txtHCPCSPayableAmt.Text.ToString(), out payableAmount))
            {
                parms.Add("HCPCS_PayableAmount", payableAmount.ToString());
            }
            if (txtClaimRemarkCodeMOA03.Text.Length > 5)
            {
                parms.Add("Claim_Code_MOA03", txtClaimRemarkCodeMOA03.Text.ToString().Substring(0, 5));
            } else
            {
                parms.Add("Claim_Code_MOA03", txtClaimRemarkCodeMOA03.Text.ToString());
            }
            if (txtClaimRemarkCodeMOA04.Text.Length > 5)
            {
                parms.Add("Claim_Code_MOA04", txtClaimRemarkCodeMOA04.Text.ToString().Substring(0, 5));
            } else
            {
                parms.Add("Claim_Code_MOA04", txtClaimRemarkCodeMOA04.Text.ToString());
            }
            
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());

            svc.UpdatePanelsData("claims_outpatient_adjudication_information", parms);
            GetOutpatientAdjudicationDetails();
        }
        else
        {
            if (txtReimbursementRate.Text.Length > 10)
            {
                parms.Add("Reimbursement_Rate", txtReimbursementRate.Text.ToString().Substring(0, 10));
            } else
            {
                parms.Add("Reimbursement_Rate", txtReimbursementRate.Text.ToString());
            }
            decimal payableAmount = 0;
            if (Decimal.TryParse(txtHCPCSPayableAmt.Text.ToString(), out payableAmount)) {
                parms.Add("HCPCS_PayableAmount", payableAmount.ToString());
            }
            if (txtClaimRemarkCodeMOA03.Text.Length > 5)
            {
                parms.Add("Claim_Code_MOA03", txtClaimRemarkCodeMOA03.Text.ToString().Substring(0, 5));
            } else
            {
                parms.Add("Claim_Code_MOA03", txtClaimRemarkCodeMOA03.Text.ToString());
            }
            if (txtClaimRemarkCodeMOA04.Text.Length > 5)
            {
                parms.Add("Claim_Code_MOA04", txtClaimRemarkCodeMOA04.Text.ToString().Substring(0, 5));
            } else {
                parms.Add("Claim_Code_MOA04", txtClaimRemarkCodeMOA04.Text.ToString());
            }
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Created_Date_Time", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            svc.InsertPanelsData("claims_outpatient_adjudication_information", parms);
            GetOutpatientAdjudicationDetails();
        }
       
    }

    private void SetReadOnlyFieldsControl(bool value)
    {
        txtClaimRemarkCodeMOA03.ReadOnly = value;
        txtClaimRemarkCodeMOA04.ReadOnly = value;
        txtHCPCSPayableAmt.ReadOnly = value;
        txtReimbursementRate.ReadOnly = value;

    }

    public void ClearFields()
    {
        txtClaimRemarkCodeMOA03.Text = string.Empty;
        txtClaimRemarkCodeMOA04.Text = string.Empty;
        txtHCPCSPayableAmt.Text = string.Empty;
        txtReimbursementRate.Text = string.Empty;

    }


}