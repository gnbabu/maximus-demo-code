using System;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CredentialCommitteeMember : System.Web.UI.UserControl
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

    public delegate void MemberActivityUpdateEventHandler(EventArgs args);
    public event MemberActivityUpdateEventHandler MemberActivityUpdated;
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }

    private void LoadStatusDropdown()
    {
        try
        {
            if (ddlResult.Items.Count == 0)
            {
                DataSet ds = svc.GetCommitteeActivityStatuses();
                if (Helper.HasRows(ds))
                {
                    Helper.LoadDropDown(ddlResult, ds.Tables[0], "COMMITTEE_ACTIVITY_STATUS_NAME", "COMMITTEE_ACTIVITY_STATUS_ID", false);
                }
            }
            
        }
        catch
        {
        }
    }
    public void LoadControlData()
    {
        
            LoadCredentialMember();

       
    }
    public void LoadCredentialMember()
    {
        try
        {
            if (Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name))
            {
                pnlmember.Visible = true;
                DataSet ds = svc.SelectCredentialingCommitteeActivity(this.WorkflowPage.RegistrationId);
                if (Helper.HasRows(ds))
                {
                    DataView dv = new DataView(ds.Tables[0]);
                    dv.RowFilter = "member_username = '" + HttpContext.Current.User.Identity.Name + "'";
                    DataTable dt = dv.ToTable();
                    if (Helper.HasRows(dt))
                    {
                        LoadStatusDropdown();
                        txtactioncomments.Text = dt.Rows[0]["COMMENTS"].ToString();
                        lblCommiteeRole.Text = dt.Rows[0]["ROLE"].ToString();
                        lblMemberName.Text = dt.Rows[0]["MEMBER_NAME"].ToString();
                        hdnCredentialingId.Value = dt.Rows[0]["CREDENTIALING_ID"].ToString();
                        int activityStatus = string.IsNullOrEmpty(hdnCredentialAction.Value) ? Convert.ToInt32(dt.Rows[0]["COMMITTEE_ACTION_STATUS_ID"]) : Convert.ToInt32(hdnCredentialAction.Value);
                        if (ddlResult.Items.FindByValue(activityStatus.ToString()) != null)
                        {
                            ddlResult.SelectedValue = activityStatus.ToString();
                        }
                    }


                }
            }
            else
            {
                pnlmember.Visible = false;
            }


        }
        catch
        {
        }
    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hdnCredentialingId.Value))
            {
                string comments = txtactioncomments.Text.Trim();
                int credentialingId = Convert.ToInt32(hdnCredentialingId.Value);
                int activityStatusId = string.IsNullOrEmpty(ddlResult.SelectedItem.Value) ? 1 : Convert.ToInt32(ddlResult.SelectedItem.Value.ToString());
                svc.UpdateCredentialingCommitteeMember(credentialingId, activityStatusId, HttpContext.Current.User.Identity.Name, comments, DateTime.Now, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                pnlmember.Visible = false;


                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) && (this.WorkflowPage.CurrentTaskName =="Committee Chairman Review"))
                {
                    //Update credentialing status
                    svc.UpdateCredentialStatus(credentialingId, CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }


                if (MemberActivityUpdated != null)
                {
                    MemberActivityUpdated(new EventArgs());
                }
            }
        }
        catch
        {
        }
    }
    protected void ddlResult_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlResult.SelectedIndex > 0)
        {
            hdnCredentialAction.Value = ddlResult.SelectedItem.Value;
        }
    }
}