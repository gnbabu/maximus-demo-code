using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_SubmitClaimDelaySubmission : BasePopupControl
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
            if (!string.IsNullOrWhiteSpace(hdnDelayReasonClaimID.Value))
                return hdnDelayReasonClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnDelayReasonClaimID.Value = value.Trim();
        }
    }

    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnDelayReasonClaimType.Value))
                return hdnDelayReasonClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnDelayReasonClaimType.Value = value.Trim();
            SetVisibleField(value);
        }
    }

    public string PreviouslyDeniedICN
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(txtPreviouslyDeniedICN.Text))
                return txtPreviouslyDeniedICN.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                txtPreviouslyDeniedICN.Text = value;
        }
    }

    public string DelayReason_Dental
    {
        get
        {
            if (!string.IsNullOrEmpty(ddlReason.SelectedItem.Text))
                return ddlReason.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlReason.SelectedValue = value.Trim();
        }
    }

    public string DelayReason_Prof_Insti
    {
        get
        {
            if (ddlProfReason.SelectedItem != null && !string.IsNullOrEmpty(ddlProfReason.SelectedItem.Text))
                return ddlProfReason.SelectedItem.Text;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrEmpty(value))
                ddlProfReason.SelectedValue = value.Trim();
        }
    }
    public bool HasInputValue()
    {
        bool rtn = false;
        if (!string.IsNullOrEmpty(ddlReason.SelectedItem.Text) || !string.IsNullOrEmpty(ddlProfReason.SelectedItem.Text) || !string.IsNullOrWhiteSpace(txtPreviouslyDeniedICN.Text))
        {
            rtn = true;
        }

        return rtn;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            GetDelayReason();
        }
        GetDelaySubmissionData();
    }
    protected void SetVisibleField(string claimtype)
    {
        if (claimtype == CON.ClaimsType.Dental)
        {
            dvDentalReason.Visible = true;
            dvProfReason.Visible = false;
        }
        else
        {
            dvDentalReason.Visible = false;
            dvProfReason.Visible = true;
        }
    }
    private void SetReadOnlyFieldsControl(bool value)
    {
        ddlProfReason.Enabled = !value;
        ddlReason.Enabled = !value;
        txtPreviouslyDeniedICN.ReadOnly = value;

    }
    private void GetDelayReason()
    {
        DataSet dataSet = svc.GetDelayReason();
        DataTable dtDelayReason = dataSet.Tables[0];
        if (Helper.HasRows(dtDelayReason))
        {
            var row_DelayReason = from row in dtDelayReason.AsEnumerable()
                                  where row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 1 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 2 ||
                                  row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 3 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 4 ||
                                  row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 5 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 6 ||
                                  row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 7 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 8 ||
                                  row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 9 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 10 ||
                                  row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 11 || row.Field<int>("PRIOR_AUTH_DELAY_REASON_Code") == 15
                                  select row;
            DataTable dtDelayReasons = row_DelayReason.CopyToDataTable();
            Helper.LoadList(ddlReason, dtDelayReasons, "PRIOR_AUTH_DELAY_REASON_DESC", "PRIOR_AUTH_DELAY_REASON_ID", true);
            Helper.LoadList(ddlProfReason, dtDelayReasons, "PRIOR_AUTH_DELAY_REASON_DESC", "PRIOR_AUTH_DELAY_REASON_ID", true);
        }
    }
    private void AssignValidationSummary(string validationSummary)
    {
        //rfvReason.ValidationGroup = validationSummary;
        //rfvddlProfReason.ValidationGroup = validationSummary;
    }
    public void SaveDelayedSubmissionResubmissionData(string validationSummary,int actionButton)
    {
        if(actionButton == CON.ActionButtonType.Save)
        {
            SaveToDb();
        }
        else
        {
            AssignValidationSummary(validationSummary);
            Page.Validate(validationSummary);
            if (Page.IsValid)
            {
                SaveToDb();
            }
        }
       

    }
    private void SaveToDb()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("Claim_ID", hdnDelayReasonClaimID.Value.ToString());

        DataSet dsReasonForDelay = svc.SelectPanelsData("Claims_Delayed_Submission_Resubmission_Information", parms);
        if (!string.IsNullOrEmpty(hdnDelayReasonClaimID.Value.ToString()))
        {
            if (Helper.HasRows(dsReasonForDelay) &&
            Convert.ToInt32(dsReasonForDelay.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnDelayReasonClaimID.Value))
            {
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Dental)
                {
                    parms.Add("Reason_for_Delay", ddlReason.SelectedItem.Text);
                }
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Institutional || hdnDelayReasonClaimType.Value == CON.ClaimsType.Professional)
                {
                    parms.Add("Reason_for_Delay", ddlProfReason.SelectedItem.Text);
                    parms.Add("Previously_Denied_ICN", txtPreviouslyDeniedICN.Text);
                }
                svc.UpdatePanelsData("Claims_Delayed_Submission_Resubmission_Information", parms);
            }
            else
            {
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Dental)
                {
                    parms.Add("Reason_for_Delay", ddlReason.SelectedItem.Text);
                }
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Institutional || hdnDelayReasonClaimType.Value == CON.ClaimsType.Professional)
                {
                    parms.Add("Reason_for_Delay", ddlProfReason.SelectedItem.Text);
                    parms.Add("Previously_Denied_ICN", txtPreviouslyDeniedICN.Text);
                }
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                svc.InsertPanelsData("Claims_Delayed_Submission_Resubmission_Information", parms);
            }
            GetDelaySubmissionData();
        }
    }
    public void GetDelaySubmissionData()
    {
        if (!string.IsNullOrEmpty(hdnDelayReasonClaimID.Value.ToString()))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("Claim_ID", hdnDelayReasonClaimID.Value.ToString());
            DataSet dsDelaySub = svc.SelectPanelsData("Claims_Delayed_Submission_Resubmission_Information", parms);

            if (Helper.HasRows(dsDelaySub) &&
                Convert.ToInt32(dsDelaySub.Tables[0].Rows[0]["Claim_ID"]) == Convert.ToInt32(hdnDelayReasonClaimID.Value))
            {
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Dental)
                {
                    ddlReason.SelectedItem.Text = dsDelaySub.Tables[0].Rows[0]["Reason_for_Delay"].ToString();
                }
                if (hdnDelayReasonClaimType.Value == CON.ClaimsType.Professional || hdnDelayReasonClaimType.Value == CON.ClaimsType.Institutional)
                {
                    ddlProfReason.SelectedItem.Text = dsDelaySub.Tables[0].Rows[0]["Reason_for_Delay"].ToString();
                    txtPreviouslyDeniedICN.Text = dsDelaySub.Tables[0].Rows[0]["Previously_Denied_ICN"].ToString();
                }
            }
        }
    }

    public void ClearFilelds()
    {
        ddlReason.SelectedValue = "";
        txtPreviouslyDeniedICN.Text = string.Empty;
    }
}