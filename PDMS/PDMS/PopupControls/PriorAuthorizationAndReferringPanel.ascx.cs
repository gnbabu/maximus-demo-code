using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_PriorAuthorizationAndReferringPanel : System.Web.UI.UserControl
{
    #region svc
    private PDMSService.PDMSServiceClient _svc;
    #endregion
    #region Properties
    public string PriorAuthorizationNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPriorAuthNumber.Text))
                return txtPriorAuthNumber.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPriorAuthNumber.Text = value.Trim();
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
    public string ReferralNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtReferralNumber.Text))
                return txtReferralNumber.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtReferralNumber.Text = value.Trim();
        }

    }
    public string ClaimID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnClaimIdPriorAuthRef.Value))
                return hdnClaimIdPriorAuthRef.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnClaimIdPriorAuthRef.Value = value.Trim();

        }
    }

    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(txtPriorAuthNumber.Text) || !string.IsNullOrEmpty(txtReferralNumber.Text))
        {
            rtn = true;
        }

        return rtn;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                if (!string.IsNullOrEmpty(hdnClaimIdPriorAuthRef.Value))
                {
                    GetPriorAuthReferralDetails();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    public void GetPriorAuthReferralDetails()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdPriorAuthRef.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimIdPriorAuthRef.Value);
            DataSet dsPrior = svc.SelectPanelsData("Claim_Prior_Authorization_Referral", parms);

            if (Helper.HasRows(dsPrior) &&
                Convert.ToInt32(dsPrior.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdPriorAuthRef.Value))
            {
                txtPriorAuthNumber.Text = dsPrior.Tables[0].Rows[0]["Prior_Authorization_Number"].ToString();
                txtReferralNumber.Text = dsPrior.Tables[0].Rows[0]["Referral_Number"].ToString();
            }

        }
    }

    public void SavePriorAuthReferNumber()
    {
        if (!string.IsNullOrEmpty(hdnClaimIdPriorAuthRef.Value))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_id", hdnClaimIdPriorAuthRef.Value);
            DataSet dsIndividualClaims = svc.SelectPanelsData("Claim_Prior_Authorization_Referral", parms);

            if (Helper.HasRows(dsIndividualClaims) &&
            Convert.ToInt32(dsIndividualClaims.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnClaimIdPriorAuthRef.Value))
            {
                parms.Add("Prior_Authorization_Number", txtPriorAuthNumber.Text);
                parms.Add("Referral_Number", txtReferralNumber.Text);
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                svc.UpdatePanelsData("Claim_Prior_Authorization_Referral", parms);
                GetPriorAuthReferralDetails();
            }
            else
            {
                parms.Add("Prior_Authorization_Number", txtPriorAuthNumber.Text);
                parms.Add("Referral_Number", txtReferralNumber.Text);
                parms.Add("Last_Modified_User", null);
                parms.Add("Last_Modified_Date", null);
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                svc.InsertPanelsData("Claim_Prior_Authorization_Referral", parms);
                GetPriorAuthReferralDetails();
            }
        }
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        txtPriorAuthNumber.ReadOnly = value;
        txtReferralNumber.ReadOnly = value;
    }

    public void ClearFields()
    {
        txtPriorAuthNumber.Text = string.Empty;
        txtReferralNumber.Text = string.Empty;
    }
}