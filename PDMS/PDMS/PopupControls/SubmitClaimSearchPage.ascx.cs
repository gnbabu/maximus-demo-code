using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchPage : System.Web.UI.UserControl
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
        //this.gvSubmitClaimSearchPage.CurrentPageIndex = 0;
        //RefreshData();
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
        this.gvSubmitClaimSearchPage.CurrentPageIndex = 0;
        RefreshData();
        //  GetServicingProviderInfo();

    }

    protected void gvHSCPSCodeSearch_Sorting(object sender, GridViewSortEventArgs e)
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

    protected void gvSubmitClaimSearchPage_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    //public void RefreshData()
    //{
    //    DataTable dt = GetData(gvSubmitClaimSearchPage.PageSize);
    //    gvSubmitClaimSearchPage.DataSource = dt;
    //    gvSubmitClaimSearchPage.DataBind();
    //}
    public void RefreshData()
    {
        string npi = txtHCPCSCode.Text;
        string medicadeId = txtMedicaidID.Text;
        string firstName = txtFirstName.Text;
        string lastname = txtBusinessLastName.Text;
        DataTable dt = GetData(npi, medicadeId, firstName, lastname);
        //dt = dt.Rows.Cast<System.Data.DataRow>().Take(gvSubmitClaimSearchPage.PageSize).CopyToDataTable();
        if(dt!= null)
        {
            gvSubmitClaimSearchPage.DataSource = dt;
            gvSubmitClaimSearchPage.DataBind();
        }
       
    }


    private DataTable GetData(string npi, string medicaidid, string lastName, string firstName)
    {
        DataTable dt = null;
        try
        {
            using (PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient())
            {
                var ds = psc.SearchProviderNPI(npi, medicaidid, lastName, firstName);
                if(ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
        }
        catch (Exception)
        {
            return dt;
        }
        return dt;
    }

    //private DataTable GetMockData()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add(new DataColumn("NPI"));
    //    dt.Columns.Add(new DataColumn("MedicaidID"));
    //    dt.Columns.Add(new DataColumn("BusinessLastName"));
    //    dt.Columns.Add(new DataColumn("FirstName"));

    //    dt.Rows.Add("123456789", "777777", "Ohio medical research hospital", "");
    //    dt.Rows.Add("123456789", "77777777", "Ohio medical research hospital", "");
    //    return dt;
    //}
}