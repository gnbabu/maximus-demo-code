using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_InPatientAdjudicationInformation : System.Web.UI.UserControl
{
    public Boolean SetReadOnlyFields
    {
        set
        {
            if (!string.IsNullOrWhiteSpace(value.ToString()))
                SetReadOnlyFieldsControl(value);
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
    public string Covered_Days_or_Visits_Count
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtCoveredDays.Text))
                return txtCoveredDays.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtCoveredDays.Text = value.Trim();
        }
    }
    public string Claim_DRG_Amount
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxClaimDRGAmount.Text))
                return txtBoxClaimDRGAmount.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxClaimDRGAmount.Text = value.Trim();
        }
    }
    public string Claim_Remark_Code05
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxClaimRemarkCode05.Text))
                return txtBoxClaimRemarkCode05.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxClaimRemarkCode05.Text = value.Trim();
        }
    }
    public string Claim_Remark_Code20
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtBoxClaimRemarkCode20.Text))
                return txtBoxClaimRemarkCode20.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtBoxClaimRemarkCode20.Text = value.Trim();
        }
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtBoxClaimDRGAmount.Text) || !string.IsNullOrEmpty(txtBoxClaimRemarkCode05.Text) || !string.IsNullOrWhiteSpace(txtBoxClaimRemarkCode20.Text)
            || !string.IsNullOrWhiteSpace(txtCoveredDays.Text))
        {
            rtn = true;
        }

        return rtn;
    }

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            try
            {

                BindData();

            }
            catch (Exception ex)
            {

            }
        }

    }


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

    public void BindData()
    {
        GetInPatientAdjudicationInformation();
    }

    public void SaveInPatientAdjudicationInformation()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnClaimId.Value);
            parms.Add("Covered_Days_or_Visits_Count", txtCoveredDays.Text);
            parms.Add("Claim_DRG_Amount", txtBoxClaimDRGAmount.Text);
            parms.Add("Claim_Remark_Code", txtBoxClaimRemarkCode05.Text);
            parms.Add("Claim_Remark_Code1", txtBoxClaimRemarkCode20.Text);
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("Last_Modified_Date", DateTime.Now.ToString());
            if (!string.IsNullOrEmpty(hdnInPatientAdjudification.Value))
            {
                parms.Add("Claims_Inpatient_Adjudication_Information_ID", hdnInPatientAdjudification.Value.ToString());
                try
                {
                    svc.UpdatePanelsData("claims_inpatient_adjudication_information", parms);
                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                try
                {
                    svc.InsertPanelsData("claims_inpatient_adjudication_information", parms);
                }
                catch (Exception ex)
                {

                }

            }
            GetInPatientAdjudicationInformation();
        

    }
    public void GetInPatientAdjudicationInformation()
    {
        if (!String.IsNullOrEmpty(hdnClaimId.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimId.Value);

            DataSet dsInPatientAdjudificationInfo = svc.SelectPanelsData("claims_inpatient_adjudication_information", parms);
            if (Helper.HasRows(dsInPatientAdjudificationInfo))
            {
                txtCoveredDays.Text = dsInPatientAdjudificationInfo.Tables[0].Rows[0]["Covered_Days_or_Visits_Count"].ToString();
                txtBoxClaimDRGAmount.Text = dsInPatientAdjudificationInfo.Tables[0].Rows[0]["Claim_DRG_Amount"].ToString();
                txtBoxClaimRemarkCode05.Text = dsInPatientAdjudificationInfo.Tables[0].Rows[0]["Claim_Remark_Code"].ToString();
                txtBoxClaimRemarkCode20.Text = dsInPatientAdjudificationInfo.Tables[0].Rows[0]["Claim_Remark_Code1"].ToString();
                hdnInPatientAdjudification.Value = dsInPatientAdjudificationInfo.Tables[0].Rows[0]["Claims_Inpatient_Adjudication_Information_ID"].ToString();

            }
        }
    }
    public void ClearInPatientAdjudicationFields()
    {
        txtCoveredDays.Text = "";
        txtBoxClaimDRGAmount.Text = "";
        txtBoxClaimRemarkCode05.Text = "";
        txtBoxClaimRemarkCode20.Text = "";
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtCoveredDays.ReadOnly = value;
        txtBoxClaimDRGAmount.ReadOnly = value;
        txtBoxClaimRemarkCode05.ReadOnly = value;
        txtBoxClaimRemarkCode20.ReadOnly = value;
       
    }
}
