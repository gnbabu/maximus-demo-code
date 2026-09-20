using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchBill : System.Web.UI.UserControl
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
        this.gvSubmitClaimSearchPop.CurrentPageIndex = 0;
        RefreshData();
        //    BindGrid();
        // GetServicingProviderInfo();
    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        this.lblSResult.Visible = true;
        SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
        SessionVarRetriever.DashBoardTableId = 0;
        this.gvSubmitClaimSearchPop.CurrentPageIndex = 0;
        RefreshData();
        //  GetServicingProviderInfo();

    }

    protected void gvSubmitClaimSearchPop_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void gvSubmitClaimSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
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

    
   protected void LinkButton8_Click(object sender, EventArgs e)
    {
       
    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvSubmitClaimSearchPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvSubmitClaimSearchPop.DataSource = dt;
            gvSubmitClaimSearchPop.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvSubmitClaimSearchPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchPop.GridViewSortColumn;

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
        dt.Columns.Add(new DataColumn("TOB"));
        dt.Columns.Add(new DataColumn("TOBDesc"));

        dt.Rows.Add("110", "Full provider liable claim");
        dt.Rows.Add("111", "Hospital inpatiant claim");
        return dt;
    }
}