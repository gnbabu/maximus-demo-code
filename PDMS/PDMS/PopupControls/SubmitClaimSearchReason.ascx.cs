using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchReason : System.Web.UI.UserControl
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
        if (!IsPostBack)
        {
            gvSubmitClaimSearchPop.DataSource = null;
            gvSubmitClaimSearchPop.DataBind();
        }
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
       
        if ((string.IsNullOrEmpty(txtCode.Text)) && (string.IsNullOrEmpty(txtPlaceOfServiceName.Text)))
        {
            lblError.Visible = true;
            lblError.Text = "Reason Code or Reason Code Description  is required for Search.";
            ModalPopupExtender mpeSubmitClaimSearchReason = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchReason");
            mpeSubmitClaimSearchReason.Show();
            CleareField();
        }
        else
        {
            // Clear these out prior to doing the Search
            //this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvSubmitClaimSearchPop.CurrentPageIndex = 0;
            lblError.Visible = false;
            RefreshData();
            ModalPopupExtender mpeSubmitClaimSearchReason = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchReason");
            mpeSubmitClaimSearchReason.Show();
            //txtPlaceOfServiceName.Text = "";
            //txtCode.Text = "";
        }
    }

    protected void gvSubmitClaimSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
        CleareField();
    }

    protected void gvSubmitClaimSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
        
    }
   

    
   protected void LinkButton8_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        Session["ICDProcCode"] = cmdArgument;
        CleareField();

    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvSubmitClaimSearchPop.PageSize);
        gvSubmitClaimSearchPop.DataSource = dt;
        gvSubmitClaimSearchPop.DataBind();
    }
    

    private DataTable GetMockData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ReasonCode"));
        dt.Columns.Add(new DataColumn("ReasonDesc"));
       

        dt.Rows.Add("45","Charge Exceeds Fee Schedule");
        dt.Rows.Add("46","Reason Code 46");
        return dt;
    }

    protected void lnkReasonCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        Session["ReasonCode"] = cmdArgument;
        CleareField();
    }

    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        string sortColWithDirection = this.gvSubmitClaimSearchPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchPop.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        // DataTable dt = GetMockData();
        DataTable dt = LookupTableController.GetCrcReasoneCode(txtCode.Text.Trim(),txtPlaceOfServiceName.Text.Trim());
        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }

    public void CleareField()
    {
        txtCode.Text = string.Empty;
        txtPlaceOfServiceName.Text = string.Empty;
        gvSubmitClaimSearchPop.DataSource = null;
        gvSubmitClaimSearchPop.DataBind();
    }
}