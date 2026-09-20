using CustomControls;
using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ScreeningHistory : System.Web.UI.UserControl
{
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
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    public class ScreeningSelectedEventArgs : EventArgs
	{
		public int ScreeningID { get; set; }
		public DateTime? ScreeningStartDate { get; set; }
		public DateTime? ScreeningEndDate { get; set; }

		public ScreeningSelectedEventArgs(int screeningID, DateTime? screeningStartDate, DateTime? screeningEndDate)
			: base()
		{
			ScreeningID = screeningID;
			ScreeningStartDate = screeningStartDate;
			ScreeningEndDate = screeningEndDate;
		}

	}

	#region Events

	public delegate void ScreeningSelectedEventHandler(ScreeningSelectedEventArgs args);
	public event ScreeningSelectedEventHandler ScreeningSelected;


	#endregion

	protected override void OnLoad(EventArgs e)
	{
        if(this.Visible)
        LoadGroupProviderScreeningHistory(this.WorkflowPage.RegistrationId);

        base.OnLoad(e);
	}

	private void BindScreeningData(int regID)
	{
        DataSet ds = svc.SelectGroupProviderScreeningData(regID);
		if (ds != null && ds.Tables.Count > 0)
		{
			DataTable dtScreeningsFull = ds.Tables[0];
            DataTable dtScreenings;

            int cnt = dtScreeningsFull.Rows.Count;
            lnkExcel.Visible = cnt > 0 ? true : false;
            lnkPDF.Visible = cnt > 0 ? true : false;
            if (!chkShowFullHistory.Checked)
            {
                if(this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
                    dtScreenings = new DataView(dtScreeningsFull, "WORKFLOW_ID <> 12 or Row_Num = 1", "START_DATE_TIME desc ,END_DATE_TIME asc", DataViewRowState.CurrentRows).ToTable();
                else
                    dtScreenings = new DataView(dtScreeningsFull, "WORKFLOW_ID <> 12", "START_DATE_TIME desc ,END_DATE_TIME asc", DataViewRowState.CurrentRows).ToTable();
            }
            else
                dtScreenings = dtScreeningsFull;

            grdScreeningHistory.DataSource = dtScreenings;
            grdScreeningHistory.DataBind();

            /// If there is only one row, automatically load the screening details.
            if (this.WorkflowPage.ActiveScreeningID > 0 && this.WorkflowPage.ActiveScreeningID != (int)dtScreenings.Rows[0]["SCREENING_ID"])
            {
                grdScreeningHistory.SelectRowByDataKey<int>(this.WorkflowPage.ActiveScreeningID);
            }
            else
            {
                if (dtScreenings.Rows.Count == 1)
                {
                    grdScreeningHistory.SelectRowByDataKey<int>((int)dtScreenings.Rows[0]["SCREENING_ID"]);
                }
                else if (dtScreenings.Rows.Count > 1)
                {
                    /// If there are multiple screening rows, filter by pending status. If there is only one with the filter applied, load that one by default
                    dtScreenings.DefaultView.RowFilter = string.Format("SCREENING_STATUS_ID = {0}", CON.ScreeningStatusId.InProgress);

                    if (dtScreenings.DefaultView.Count >= 1)
                    {
                        grdScreeningHistory.SelectRowByDataKey<int>((int)dtScreenings.DefaultView.ToTable().Rows[0]["SCREENING_ID"]);
                    }
                    else if (dtScreenings.DefaultView.Count == 0)
                    {
                        grdScreeningHistory.SelectRowByDataKey<int>((int)dtScreenings.Rows[0]["SCREENING_ID"]);
                    }
                }
            }
		}
	}

	public void LoadGroupProviderScreeningHistory(int regID)
	{
        //DataSet ds = svc.SelectGroupProviderScreeningData(regID);
        BindScreeningData(regID);
	}



    public bool ScreeningComplete(int screeningID)
    {
        bool isComplete = false;

        GridView gv = this.grdScreeningHistory;

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


	protected void grdScreeningHistory_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (grdScreeningHistory.SelectedIndex >= 0)
		{
			int screeningID = (int)grdScreeningHistory.SelectedDataKey.Value;
			DateTime? screeningStartDate = null;
			DateTime? screeningEndDate = null;

			DateTime tmp;
			if (DateTime.TryParse(grdScreeningHistory.SelectedRow.Cells[1].Text, out tmp))
			{
				screeningStartDate = tmp;
			}

			if (DateTime.TryParse(grdScreeningHistory.SelectedRow.Cells[2].Text, out tmp))
			{
				screeningEndDate = tmp;
			}


			if (screeningID > 0)
			{
				if (ScreeningSelected != null)
				{
					ScreeningSelected(new ScreeningSelectedEventArgs(screeningID, screeningStartDate, screeningEndDate));
				}

			}
		}
		else
		{
			/// Hide the details area.
			if (ScreeningSelected != null)
			{
				ScreeningSelected(new ScreeningSelectedEventArgs(-1, null, null));
			}
		}
	}

    protected void chkShowFullHistory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            
            BindScreeningData(this.WorkflowPage.RegistrationId);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void lnkPDF_Click(object sender, EventArgs e)
    {
        DataTable dt = GetData(this.WorkflowPage.RegistrationId);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToPdf();
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = GetData(this.WorkflowPage.RegistrationId);
        RadGridExport.DataSource = dt;
        RadGridExport.DataBind();
        RadGridExport.MasterTableView.ExportToExcel();

    }

    private DataTable GetData(int regID)
    {
        DataSet ds = svc.SelectGroupProviderScreeningData(regID);
        DataTable dtScreenings = null;
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtScreeningsFull = ds.Tables[0];
            

            int cnt = dtScreeningsFull.Rows.Count;
            if (!chkShowFullHistory.Checked)
            {
                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
                    dtScreenings = new DataView(dtScreeningsFull, "WORKFLOW_ID <> 12 or Row_Num = 1", "START_DATE_TIME desc ,END_DATE_TIME asc", DataViewRowState.CurrentRows).ToTable();
                else
                    dtScreenings = new DataView(dtScreeningsFull, "WORKFLOW_ID <> 12", "START_DATE_TIME desc ,END_DATE_TIME asc", DataViewRowState.CurrentRows).ToTable();
            }
            else
                dtScreenings = dtScreeningsFull;
        }
        return dtScreenings;
    }
}