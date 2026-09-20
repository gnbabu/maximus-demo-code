using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CredentialCommitteeResult : System.Web.UI.UserControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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
    public bool ShowCommitteeData
    {
        set
        {
            pnlCommittee.Visible = value;
        }
    }
    public DataSet GetCommitteeData
    {
        get
        {
            return svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CREDENTIALING");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        // SAM459
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration
            && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialingReconsideration
            && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
                || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
        {
            SetPageReadOnly(true);
        }

    }
    private void SetPageReadOnly (bool isReadOnly)
    {
        ddlCommResult.Enabled = !isReadOnly;
        txtDecisionDate.Enabled = !isReadOnly;
        txtDenialTermReason.Enabled = !isReadOnly;
        txtDecisionDate.Enabled = !isReadOnly;
        txtSummary.Enabled = !isReadOnly;
        btnConfirmComm.Visible = !isReadOnly;
        btnCancelCommittee.Visible = !isReadOnly;
    }

    private DataSet GetCredentialCommitteeDetails(int regid, int CredId)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID ", regid.ToString());
        parms.Add("CREDENTIALING_ID", CredId.ToString());
        DataSet dsForm = RegistrationController.SelectRegistrationDataWithParams("usp_selectreg_credentialing_id", parms);
        return dsForm;
    }

    public void LoadCommitteeData(int Credid = 0)
    {
        try
        {
            DataSet dsCommResult = svc.SelectCredentialingResult();
            if (Helper.HasRows(dsCommResult))
            {
                Helper.LoadDropDown(ddlCommResult, dsCommResult.Tables[0], "CREDENTIALING_RESULT_NAME", "CREDENTIALING_RESULT_ID", true);
            }

            txtDenialTermReason.Visible = false;
            lblDenialTermReason.Visible = false;

            //DataSet dsCommittee = GetCommitteeData;
            DataSet dsCommittee = new DataSet();
            if (Credid > 0)
            {
                dsCommittee = GetCredentialCommitteeDetails(this.WorkflowPage.RegistrationId, Credid);
            }
            else
            {
                dsCommittee = GetCommitteeData;
            }

            if (Helper.HasRows(dsCommittee))
            {
                ddlCommResult.SelectedValue = dsCommittee.Tables[0].Rows[0]["COMMITTEE_RESULT_ID"].ToString();
                txtDecisionDate.Text = dsCommittee.Tables[0].Rows[0]["COMMITTEE_DECISION_DATE"].ToString();
                txtDenialTermReason.Text = dsCommittee.Tables[0].Rows[0]["COMMITTEE_DENIAL_TERM_REASON"].ToString();
                txtDenialTermReason.Text = dsCommittee.Tables[0].Rows[0]["COMMITTEE_DENIAL_TERM_REASON"].ToString();
                txtSummary.Text = dsCommittee.Tables[0].Rows[0]["COMMITTEE_SUMMARY"].ToString();
                txtCommdiscuss.Text = dsCommittee.Tables[0].Rows[0]["COMMITTEE_DISCUSSION"].ToString();

                if (ddlCommResult.SelectedValue == CON.CredentialingResult.Denied.ToString() || 
                    ddlCommResult.SelectedValue == CON.CredentialingResult.Terminated.ToString()|| 
                    ddlCommResult.SelectedValue == CON.CredentialingResult.Failed.ToString())
                {
                    txtDenialTermReason.Visible = true;
                    lblDenialTermReason.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlCommResult_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        txtDenialTermReason.Visible = false;
        lblDenialTermReason.Visible = false;
        if (ddlCommResult.SelectedValue == CON.CredentialingResult.Denied.ToString() ||
            ddlCommResult.SelectedValue == CON.CredentialingResult.Terminated.ToString() ||
            ddlCommResult.SelectedValue == CON.CredentialingResult.Failed.ToString())
        {
            txtDenialTermReason.Visible = true;
            lblDenialTermReason.Visible = true;
        }
    }

    protected void RoleCustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        //Check if Credentialing is NOT in low risk and role is in Cred chair then show an error
        DataSet ds = svc.SelectProviderCredentialingData(this.WorkflowPage.RegistrationId);
        DataRow[] selectedRows = ds.Tables[0].Select("credentialing_id = " + this.WorkflowPage.ActiveScreeningID);
        
        var hasNonLowCred = false;
        if (selectedRows != null && selectedRows.Length == 1)
        {
            if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair))
            {
                if (selectedRows[0]["RISK_LEVEL_ID"].ToString() != "1")
                {
                    hasNonLowCred = true;
                }
                if (hasNonLowCred)
                {
                    args.IsValid = false;
                }
                else
                    args.IsValid = true;
            }
        }
        else
        {
            args.IsValid = true;
        }
    }


    protected void btnConfirmComm_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CREDENTIALING_ID", this.WorkflowPage.ActiveScreeningID.ToString());
            parms.Add("Committee_Result_id", ddlCommResult.SelectedValue);
            parms.Add("COMMITTEE_DENIAL_TERM_REASON", txtDenialTermReason.Text);
            parms.Add("Committee_summary", txtSummary.Text);
            parms.Add("Committee_Date", txtDecisionDate.Text);
            parms.Add("Committee_Discussion", txtCommdiscuss.Text);
            parms.Add("Last_Modified_Date_Time", DateTime.Now.ToString());
            parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            svc.UpdateRegistrationDataTable("CREDENTIALING", parms);

            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Credential Committee Note - " + txtCommdiscuss.Text, null, null, null, this.WorkflowPage.WF_ProcessID);

            svc.UpdateCredentialResult(this.WorkflowPage.RegistrationId, Convert.ToInt32(ddlCommResult.SelectedValue), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (ddlCommResult.SelectedValue == CON.CredentialingResult.Denied.ToString() || ddlCommResult.SelectedValue == CON.CredentialingResult.Terminated.ToString()
                || ddlCommResult.SelectedValue == CON.CredentialingResult.Failed.ToString())
            {
                svc.UpdateCredentialStatus(this.WorkflowPage.RegistrationId, ddlCommResult.SelectedValue == CON.CredentialingResult.Denied.ToString() ? CON.CredentilaingStatus.CredentialingCommitteeDenial : CON.CredentilaingStatus.CredentialingCommitteeTermination, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                svc.TerminateProvider(this.WorkflowPage.RegistrationId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.EnrollStatus.INACTIVE.ToString(), CON.EnrollStatusReason.DeniedTerminatedByCommittee, false, false);


                Next_Step("Fail");
            }

            if (ddlCommResult.SelectedValue == CON.CredentialingResult.Pass.ToString() || ddlCommResult.SelectedValue == CON.CredentialingResult.Pass1Year.ToString())
            {
                svc.UpdateCredentialStatus(this.WorkflowPage.RegistrationId, CON.CredentilaingStatus.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                Next_Step("Approve");
            }
        }
    }

    private void Next_Step(string action)
    {
        svc.WF_TakeAction(this.WorkflowPage.WF_ProcessID, action, string.Empty);
        Response.Redirect("~/Process/CredentialQueue.aspx");
    }

    protected void btnCancelCommittee_Click (object sender, EventArgs e)
    {
        ddlCommResult.SelectedIndex = -1;
        txtDecisionDate.Text = string.Empty;
        txtSummary.Text = string.Empty;
        txtCommdiscuss.Text = string.Empty;
    }

    //protected void dtlCommittee_ItemDataBound(object sender, DataListItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item||e.Item.ItemType==ListItemType.AlternatingItem)
    //    {
    //        Label lblstatus = (Label)e.Item.FindControl("lblCmtAction");

    //        if (lblstatus != null)
    //        {
    //            string formattedDiscription = GetFormattedDescription(lblstatus.Text.Trim());
    //            lblstatus.Text=formattedDiscription;

    //        }
    //    }
    //}

    //private string GetFormattedDescription(string description)
    //{
    //    string retDescription = description;
    //    switch (description)
    //    {
    //        case CON.CommitteeCredentialActivityStatus.Pending:
    //            retDescription = "<b><font color='black'>" + description +"</font></b>";
    //            break;
    //        case CON.CommitteeCredentialActivityStatus.Fail:
    //            retDescription = "<b><font color='red'>" + description + "</font></b>";;
    //            break;
    //        case CON.CommitteeCredentialActivityStatus.Pass:
    //        case CON.CommitteeCredentialActivityStatus.ApproveWithRestrictions:
    //            retDescription = "<b><font color='green'>" + description +"</font></b>";;
    //            break;
    //        default:
    //            break;
    //    }
    //    return retDescription;


    //}
}