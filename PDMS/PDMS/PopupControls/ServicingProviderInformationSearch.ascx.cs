using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_ServicingProviderInformation : System.Web.UI.UserControl
{

    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        this.gvSerProviderInfoSearch.CurrentPageIndex = 0;
        RefreshData();
    //    BindGrid();
       // GetServicingProviderInfo();
    }
    
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblSResult.Visible = true;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        this.gvSerProviderInfoSearch.CurrentPageIndex = 0;
        RefreshData();
      //  GetServicingProviderInfo();

    }

    protected void gvSerProviderInfoSearch_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
    //private void GetServicingProviderInfo()
    //{
    //    if (_spa == null)
    //    {
    //        _spa = new PDMSService.PDMSServiceClient();
    //    }

    //    var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
    //    var dataTable = ds.Tables["AuthorizationInfo"];

    //    gvSerProviderInfoSearch.DataSource = dataTable;
    //    gvSerProviderInfoSearch.DataBind();
    //    //gvPriorAuthSearchDiagnosis.DataSource = dataTable;
    //    //gvPriorAuthSearchDiagnosis.DataBind();
    //}
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void gvSerProviderInfoSearch_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
        //gvSerProviderInfoSearch.PageIndex = e.NewPageIndex;
        //BindGrid();
        //gvSerProviderInfoSearch.EditIndex = -1;
    }
    //private void BindGrid()
    //{
    //    //var ds = PriorAuthHospitalController.SelectPriorAuthAttachment(2320);

    //    //get the data from xml
    //    var ds = PriorAuthHospitalController.GetSubmitPriorAuthRequestResponse();
    //    var dataTable = ds.Tables["AuthorizationInfo"];
    //    gvSerProviderInfoSearch.DataSource = dataTable;
    //    gvSerProviderInfoSearch.DataBind();
    //}
    public void RefreshData()
    {
        DataTable dt = GetData(gvSerProviderInfoSearch.PageSize);
        if (Helper.HasRows(dt))
        {
            gvSerProviderInfoSearch.DataSource = dt;
            gvSerProviderInfoSearch.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvSerProviderInfoSearch.GridViewSortDirection == SortDirection.Descending ? gvSerProviderInfoSearch.GridViewSortColumn + " DESC" : gvSerProviderInfoSearch.GridViewSortColumn;

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
        dt.Columns.Add(new DataColumn("NPI"));
        dt.Columns.Add(new DataColumn("MedicaidID"));
        dt.Columns.Add(new DataColumn("BusinessLastName"));
        dt.Columns.Add(new DataColumn("FirstName"));

        dt.Rows.Add("101010", "777777", "Ohio medical research hospital", "");
        dt.Rows.Add("10101010", "77777777", "Ohio medical research hospital", "");
        return dt;
    }

   
}