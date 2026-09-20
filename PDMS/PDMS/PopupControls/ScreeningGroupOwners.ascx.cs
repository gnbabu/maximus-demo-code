using CustomControls;
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.Util;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_ScreeningGroupOwners : System.Web.UI.UserControl
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

	#region Events


	public class OwnerScreeningSelectedEventArgs : UserControls_ScreeningHistory.ScreeningSelectedEventArgs
	{
		public int RegOwnerID { get; set; }
		public string Name { get; set; }

		public OwnerScreeningSelectedEventArgs(int screeningID, DateTime? screeningStartDate, DateTime? screeningEndDate, int regOwnerID, string name)
			: base(screeningID, screeningStartDate, screeningEndDate)
		{
			RegOwnerID = regOwnerID;
			Name = name;
		}
	}

	public delegate void OwnerScreeningSelectedEventHandler(OwnerScreeningSelectedEventArgs args);
	public event OwnerScreeningSelectedEventHandler OwnerScreeningSelected;


	#endregion

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

	protected void Page_Load(object sender, EventArgs e)
	{

	}
	DataSet Screening = new DataSet();	

	public bool ScreeningComplete(int screeningID)
	{
		bool isComplete = false;

		if (screeningID == 0)
		{
			return false;
		}

        foreach (GridDataItem item in gvGroupOwners.MasterTableView.Items)
        {
            
            if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("SCREENING_ID").ToString()) && screeningID == Convert.ToInt32((item).GetDataKeyValue("SCREENING_ID")))
			{
				if(!string.IsNullOrWhiteSpace((item).GetDataKeyValue("SCREENING_STATUS_ID").ToString()) && Convert.ToInt32((item).GetDataKeyValue("SCREENING_STATUS_ID")) == 2)
				{
                    isComplete = true;
                }
            }
               
        }  

		return isComplete;
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
        DataSet ds = svc.SelectOwnersScreeningData(this.WorkflowPage.RegistrationId);
		
		DataTable dtScreenings = null;
        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtScreeningsFull = ds.Tables[0];
            int cnt = dtScreeningsFull.Rows.Count;
             dtScreenings = dtScreeningsFull;
        }
        return dtScreenings;
    }

    #region TelerikGrid
	public void LoadGroupOwners(int regID, int ProvscreeningID)
	{
		RefreshData(regID, ProvscreeningID);
		gvGroupOwners.CurrentPageIndex = 0;
        gvGroupOwners.Rebind();
        
    }
	private void RefreshData(int regID, int ProvscreeningID)
	{
        DataSet ds = svc.SelectOwnersScreeningDataWithEndDate(regID, ProvscreeningID);

        Screening = ds.Clone();
        int j = 0;
        Screening.Tables[0].Rows.Clear();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            string screeningId = row["SCREENING_ID"].ToString();
            string statusid = row["SCREENING_STATUS_ID"].ToString();



            string column = row["OwnerEndDate"].ToString();
            DateTime RefDate = DateTime.Now;
            DateTime actDate = DateTime.Parse(column);

            if (statusid == "1" && actDate < RefDate)
            {

                Screening.Tables[0].Rows.Add(row.ItemArray);

            }

        }

        foreach (DataRow row in Screening.Tables[0].Rows)
        {
            string screeningId = row["SCREENING_ID"].ToString();

            svc.UpdateScreeningScreeningStatusID(Convert.ToInt32(screeningId));
        }

        ds = svc.SelectOwnersScreeningDataWithEndDate(regID, ProvscreeningID);

        if (ds != null && ds.Tables.Count > 0)
        {
            DataTable dtOwners = ds.Tables[0];
            DataTable dtRowCnt = ds.Tables[1];

            gvGroupOwners.DataSource = dtOwners;
            gvGroupOwners.VirtualItemCount = Convert.ToInt32(dtRowCnt.Rows[0]["TOTAL_ROW_CNT"]);
        }
        else
        {
            gvGroupOwners.DataSource = null;
            gvGroupOwners.VirtualItemCount = 0;
        }
        lnkExcel.Visible = lnkPDF.Visible = gvGroupOwners.Items.Count > 0;
    }
    protected void gvGroupOwners_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
       RefreshData(this.WorkflowPage.RegistrationId, this.WorkflowPage.ProviderScreeningID);
    }
    protected void gvGroupOwners_PageIndexChanging(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        gvGroupOwners.CurrentPageIndex = e.NewPageIndex;
        ViewState["GridData"] = e.NewPageIndex;
		this.WorkflowPage.OwnerScreeningID = -1;
    }
    protected void gvGroupOwners_SelectedIndexChanged(object sender, EventArgs e)
	{
        GridDataItem item = (GridDataItem)gvGroupOwners.SelectedItems[0];
		int regOwnerID = Convert.ToInt32((item).GetDataKeyValue("REG_OWNER_ID")); 
        int screeningID = 0; //If all screenings are complete, this will be null.
		
		if(!string.IsNullOrWhiteSpace((item).GetDataKeyValue("SCREENING_ID").ToString()))
		{
            if ((this.WorkflowPage.OwnerScreeningID > 0 || this.WorkflowPage.OwnerScreeningID == -1) 
				&& this.WorkflowPage.OwnerScreeningID != Convert.ToInt32((item).GetDataKeyValue("SCREENING_ID")))
            {
                this.WorkflowPage.OwnerScreeningID = screeningID = Convert.ToInt32((item).GetDataKeyValue("SCREENING_ID"));
            }
            else
                screeningID = this.WorkflowPage.OwnerScreeningID;
        }

        if (this.WorkflowPage.OwnerScreeningID > 0)
        {
            DateTime? startDate = null;
            if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("START_DATE_TIME").ToString()))
            {
                startDate = Convert.ToDateTime((item).GetDataKeyValue("START_DATE_TIME"));
            }

            DateTime? endDate = null;
            if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("END_DATE_TIME").ToString()))
            {
                endDate = Convert.ToDateTime((item).GetDataKeyValue("END_DATE_TIME"));
            }


            string name = item["NAME"].Text;

            if (regOwnerID > 0 && screeningID > 0)
            {
                if (OwnerScreeningSelected != null)
                {
                    OwnerScreeningSelected(new OwnerScreeningSelectedEventArgs(screeningID, startDate, endDate, regOwnerID, name));
                }
            }

        }
        else
        {
            if (OwnerScreeningSelected != null)
            {
                OwnerScreeningSelected(new OwnerScreeningSelectedEventArgs(-1, null, null, -1, null));
            }
        }
    }
    protected void gvGroupOwners_RowCommand(object sender, GridCommandEventArgs e)
	{

	}
    protected void gvGroupOwners_RowDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
	{
		if(e.Item is GridDataItem)
		{
            GridDataItem item = (GridDataItem)e.Item;
            int screeningID = 0;
			string name = string.Empty;
			int regOwnerID = 0;
            DateTime? startDate = null;
            DateTime? endDate = null;

            if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("SCREENING_ID").ToString()))
                screeningID = Convert.ToInt32((item).GetDataKeyValue("SCREENING_ID"));

            if ((this.WorkflowPage.OwnerScreeningID > 0 && screeningID == this.WorkflowPage.OwnerScreeningID) || (gvGroupOwners.VirtualItemCount == 1))
			{
                item.Selected = true;

                name = item["NAME"].Text;
                regOwnerID = Convert.ToInt32((item).GetDataKeyValue("REG_OWNER_ID"));
                
                if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("START_DATE_TIME").ToString()))
                {
                    startDate = Convert.ToDateTime((item).GetDataKeyValue("START_DATE_TIME"));
                }

                
                if (!string.IsNullOrWhiteSpace((item).GetDataKeyValue("END_DATE_TIME").ToString()))
                {
                    endDate = Convert.ToDateTime((item).GetDataKeyValue("END_DATE_TIME"));
                }

                if (OwnerScreeningSelected != null)
                {
                    OwnerScreeningSelected(new OwnerScreeningSelectedEventArgs(screeningID, startDate, endDate, regOwnerID, name));
                }

            }
			else
			{
				if(gvGroupOwners.MasterTableView.Items.Count > 0)
				{
                    gvGroupOwners.MasterTableView.Items[0].Selected = true;
                    screeningID = Convert.ToInt32(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("SCREENING_ID"));
                    name = gvGroupOwners.MasterTableView.Items[0]["NAME"].Text;
                    regOwnerID = Convert.ToInt32(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("REG_OWNER_ID"));
                    //startDate = Convert.ToDateTime(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("START_DATE_TIME"));
                    //endDate = Convert.ToDateTime(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("END_DATE_TIME"));

                    if (!string.IsNullOrWhiteSpace(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("START_DATE_TIME").ToString()))
                    {
                        startDate = Convert.ToDateTime(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("START_DATE_TIME"));
                    }

                    if (!string.IsNullOrWhiteSpace(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("END_DATE_TIME").ToString()))
                    {
						endDate = Convert.ToDateTime(gvGroupOwners.MasterTableView.Items[0].GetDataKeyValue("END_DATE_TIME"));
                    }

                    if (OwnerScreeningSelected != null)
                    {
                        OwnerScreeningSelected(new OwnerScreeningSelectedEventArgs(screeningID, startDate, endDate, regOwnerID, name));
                    }
                }
					 
            }

        }
    }
    #endregion
}


