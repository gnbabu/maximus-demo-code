using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchPop : System.Web.UI.UserControl
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
        
        if(!IsPostBack)
        {
            gvSubmitClaimSearchPop.DataSource = null;
            gvSubmitClaimSearchPop.DataBind();
        }
       
    }

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    // Clear these out prior to doing the Search
        
    //    //  GetServicingProviderInfo();

    //}

    protected void gvSubmitClaimSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void gvSubmitClaimSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
        
    }
    


    protected void LinkButton8_Click(object sender, EventArgs e)
    {

    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvSubmitClaimSearchPop.PageSize);
        
            gvSubmitClaimSearchPop.DataSource = dt;
            gvSubmitClaimSearchPop.DataBind();
       // }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        string sortColWithDirection = this.gvSubmitClaimSearchPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchPop.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        //DataTable dt = GetMockData();

        
        DataTable dt = LookupTableController.GetPlaceofServiceDetail(txtCode.Text.TrimEnd(), txtPlaceOfServiceName.Text.TrimEnd());

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }

    
    protected void lnkCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        Session["PlaceofServiceCode"] = cmdArgument;
        txtCode.Text = "";
        txtPlaceOfServiceName.Text = "";
        gvSubmitClaimSearchPop.DataSource = null;
        gvSubmitClaimSearchPop.DataBind();


    }

    protected void btnSearch_Click1(object sender, EventArgs e)
    {
        ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
        lblSResultPlaceofService.Visible = false;
        lblSResultPlaceofService.Text = string.Empty;

        if (string.IsNullOrEmpty(txtCode.Text) && string.IsNullOrEmpty(txtPlaceOfServiceName.Text))
        {
            mpeSubmitClaimSearchPop.Show();
            lblSResultPlaceofService.Text = "Place of service code or name is required";
            lblSResultPlaceofService.Visible = true;
        }
        else
        {
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            //this.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 .CurrentPageIndex = 0;
            RefreshData();
            mpeSubmitClaimSearchPop.Show();
        }
    }

    public void CleareField()
    {
        txtCode.Text = string.Empty;
        txtPlaceOfServiceName.Text = string.Empty;
        gvSubmitClaimSearchPop.DataSource = null;
        gvSubmitClaimSearchPop.DataBind();
    }

}