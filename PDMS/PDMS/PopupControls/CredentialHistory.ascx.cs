using CustomControls;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_CredentialHistory : System.Web.UI.UserControl
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

	public class CredentialSelectedEventArgs : EventArgs
	{
		public int CredentialID { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }

        public CredentialSelectedEventArgs(int credentialID, DateTime? startDate, DateTime? endDate)
			: base()
		{
			CredentialID = credentialID;
			StartDate = startDate;
			EndDate = endDate;
		}

	}
    public int CredentialStatusID
    {
        get
        {
            if (ViewState["CredentialStatusID"] == null)
                ViewState["CredentialStatusID"] = 0;

            return (int)ViewState["CredentialStatusID"];
        }
        set
        {
            ViewState["CredentialStatusID"] = value;
        }
    }
    #region Events

    public delegate void CredentialSelectedEventHandler(CredentialSelectedEventArgs args);
	public event CredentialSelectedEventHandler CredentialSelected;

   
	#endregion

	protected override void OnLoad(EventArgs e)
	{
		
		
		base.OnLoad(e);
	}

	private void BindCredentialData(DataSet ds)
	{
		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		{
            grdCredentialHistory.DataSource = ds.Tables[0];
            grdCredentialHistory.DataBind();
            CredentialStatusID = Convert.ToInt32(ds.Tables[0].Rows[0]["CREDENTIALING_STATUS_ID"]);
            chkPendingVerification.Checked = CredentialStatusID == CON.CredentilaingStatus.PendingProcess;
            DataTable dtScreenings = ds.Tables[0];

            if (this.WorkflowPage.ActiveScreeningID > 0 && this.WorkflowPage.ActiveScreeningID != (int)dtScreenings.Rows[0]["credentialing_id"])
            {
                grdCredentialHistory.SelectRowByDataKey<int>(this.WorkflowPage.ActiveScreeningID);
            }
            else
            {
                /// If there is only one row, automatically load the screening details.
                if (dtScreenings.Rows.Count == 1)
                {
                    grdCredentialHistory.SelectRowByDataKey<int>((int)dtScreenings.Rows[0]["credentialing_id"]);
                }
                else if (dtScreenings.Rows.Count > 1)
                {
                    /// If there are multiple screening rows, filter by pending status. If there is only one with the filter applied, load that one by default
                    dtScreenings.DefaultView.RowFilter = string.Format("CREDENTIALING_STATUS_ID = {0}", CON.CredentilaingStatus.InProcess);

                    if (dtScreenings.DefaultView.Count >= 1)
                    {
                        grdCredentialHistory.SelectRowByDataKey<int>((int)dtScreenings.DefaultView.ToTable().Rows[0]["credentialing_id"]);
                    }
                    else if (dtScreenings.DefaultView.Count == 0)
                    {
                        grdCredentialHistory.SelectRowByDataKey<int>((int)dtScreenings.Rows[0]["credentialing_id"]);
                    }
                }
            }
		}
	}

	public void LoadProviderCredentialHistory(int regID)
	{
		DataSet ds = svc.SelectProviderCredentialingData(regID);
        BindCredentialData(ds);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist) ||
             Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSpecialist))
        {
            pnlPendingVerification.Visible = true;
        }
        else pnlPendingVerification.Visible = false;
    }



    public bool ScreeningComplete(int screeningID)
    {
        bool isComplete = false;

        GridView gv = this.grdCredentialHistory;

        if (screeningID == 0)
        {
            return false;
        }

        foreach (GridViewRow row in gv.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                if ((int)gv.DataKeys[row.RowIndex]["SCREENING_ID"] == screeningID)
                {
                    LinkButton btn = row.FindControl("btnSelect") as LinkButton;
                    if (btn != null && btn.Text == CON.ScreeningStatusName.Complete)
                    {
                        isComplete = true;
                    }
                }
            }
        }

        return isComplete;
    }


    protected void grdCredentialHistory_SelectedIndexChanged(object sender, EventArgs e)
	{
        if (grdCredentialHistory.SelectedIndex >= 0)
		{
            int credentialID = (int)grdCredentialHistory.SelectedDataKey.Value;
			DateTime? StartDate = null;
			DateTime? EndDate = null;

			DateTime tmp;
            if (DateTime.TryParse(grdCredentialHistory.SelectedRow.Cells[1].Text, out tmp))
			{
				StartDate = tmp;
			}

            if (DateTime.TryParse(grdCredentialHistory.SelectedRow.Cells[2].Text, out tmp))
			{
				EndDate = tmp;
			}


            if (credentialID > 0)
			{
				if (CredentialSelected != null)
				{
                    CredentialSelected(new CredentialSelectedEventArgs(credentialID, StartDate, EndDate));
				}

			}
		}
		else
		{
			/// Hide the details area.
			if (CredentialSelected != null)
			{
				CredentialSelected(new CredentialSelectedEventArgs(-1, null, null));
			}
		}
	}
    protected void chkPendingVerification_CheckedChanged(object sender, EventArgs e)
    {
        if (chkPendingVerification.Checked && CredentialStatusID != CON.CredentilaingStatus.PendingProcess)
            svc.UpdateCredentialStatus(this.WorkflowPage.RegistrationId, CON.CredentilaingStatus.PendingProcess, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        else if (!chkPendingVerification.Checked && CredentialStatusID == CON.CredentilaingStatus.PendingProcess)
            svc.UpdateCredentialStatus(this.WorkflowPage.RegistrationId, CON.CredentilaingStatus.InProcess, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        LoadProviderCredentialHistory(this.WorkflowPage.RegistrationId);
    }
}