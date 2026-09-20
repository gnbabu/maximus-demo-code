using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceCheckEligibility : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.gvHospiceCheckEligibility.CurrentPageIndex = 0;
        RefreshData();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblSResult.Visible = true;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        this.gvHospiceCheckEligibility.CurrentPageIndex = 0;
        RefreshData();

    }

    protected void gvHospiceCheckEligibility_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvHospiceCheckEligibility_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvHospiceCheckEligibility.PageSize);
        if (Helper.HasRows(dt))
        {
            gvHospiceCheckEligibility.DataSource = dt;
            gvHospiceCheckEligibility.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvHospiceCheckEligibility.GridViewSortDirection == SortDirection.Descending ? gvHospiceCheckEligibility.GridViewSortColumn + " DESC" : gvHospiceCheckEligibility.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        DataTable dt = GetMockData();

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }

    private DataTable GetMockData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("Health_Plan"));
        dt.Columns.Add(new DataColumn("Effective_Date"));
        dt.Columns.Add(new DataColumn("End_Date"));

        dt.Rows.Add("MCAID", "12/10/2020", "12/10/2020");
        return dt;
    }
}