using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_HospiceHLTCFProviderSearch : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.gvHospiceHLTCFProviderSearch.CurrentPageIndex = 0;
        //RefreshData();
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblSResult.Visible = true;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        //this.gvHospiceHLTCFProviderSearch.CurrentPageIndex = 0;
        RefreshData();
        ScriptManager.RegisterStartupScript(this, this.GetType(), "script", "OpenNPIModal()", true);
    }

    protected void gvHospiceHLTCFProviderSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void gvHospiceHLTCFProviderSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        string npi = txtNPI.Text;
        string medicadeId = txtMedicaidID.Text;
        string firstName = txtFirstName.Text;
        string lastname = txtBusinessLastName.Text;
        DataTable dt = SearchProviderNPI(npi, medicadeId, firstName, lastname).Tables[0];
        dt = dt.Rows.Cast<System.Data.DataRow>().Take(gvHospiceHLTCFProviderSearch.PageSize).CopyToDataTable();
        gvHospiceHLTCFProviderSearch.DataSource = dt;
        gvHospiceHLTCFProviderSearch.DataBind();
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        //string sortColWithDirection = this.gvHospiceHLTCFProviderSearch.GridViewSortDirection == SortDirection.Descending ? gvHospiceHLTCFProviderSearch.GridViewSortColumn + " DESC" : gvHospiceHLTCFProviderSearch.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        DataTable dt = GetMockData();

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }
    public DataSet SearchProviderNPI(string npi, string medicaidID, string lastName, string firstName)
    {
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                return psc.SearchProviderNPI(npi, medicaidID, lastName, firstName);
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    private DataTable GetMockData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("NPI"));
        dt.Columns.Add(new DataColumn("MedicaidID"));
        dt.Columns.Add(new DataColumn("BusinessLastName"));
        dt.Columns.Add(new DataColumn("FirstName"));

        dt.Rows.Add("101010", "777777", "Ohio medical research hospital", "");
        dt.Rows.Add("10101010", "77777777", "Ohio medical research hospital", "");
        return dt;
    }
}