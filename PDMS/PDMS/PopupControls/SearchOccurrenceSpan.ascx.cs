using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SearchOccurrenceSpan : System.Web.UI.UserControl
{
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
    #region svc
    private PDMSService.PDMSServiceClient _svc;

    #endregion
    public string OccurenceSpanCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOccurrenceSpanCode.Value))
                return hdnOccurrenceSpanCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOccurrenceSpanCode.Value = value.Trim();
        }
    }
    public string OccurenceSpanCodeDescription
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnOccurrenceSpanCodeDec.Value))
                return hdnOccurrenceSpanCodeDec.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnOccurrenceSpanCodeDec.Value = value.Trim();
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            gvOccurenceCodeSpanSearchPop.DataSource = null;
            gvOccurenceCodeSpanSearchPop.DataBind();            
            lblSResult.Text = "";
        }

    }
    protected void Page_PreRender(object sender, EventArgs e)
    {
    }
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ModalPopupExtender modalOccurrenceSpan = (ModalPopupExtender)this.Parent.FindControl("mpeOccurenceSPanSearchPop");
        lblSResult.Text = "";
        if (string.IsNullOrEmpty(txtCode.Text) && string.IsNullOrEmpty(txtCodeDescription.Text))
        {
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            lblSResult.Visible = true;
            lblSResult.Text = "Occurrence Span code or Occurrence Description is required";
            modalOccurrenceSpan.Show();            
           // divGridOccurrenceSpanCode.Visible = false;

            this.gvOccurenceCodeSpanSearchPop.CurrentPageIndex = 0;
        }
        else
        {

            lblSResult.Text = "";
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            modalOccurrenceSpan.Show();            
            RefreshData();
            txtCode.Text = "";
            txtCodeDescription.Text = "";
            hdnStatus.Value = "SEARCH";
        }
    }
    protected void gvOccurenceCodeSpanSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    protected void gvOccurenceCodeSpanSearchPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        DataSet ds = GetData(gvOccurenceCodeSpanSearchPop.PageSize);
        if (Helper.HasRows(ds))
        {
            gvOccurenceCodeSpanSearchPop.DataSource = ds.Tables[0];
            gvOccurenceCodeSpanSearchPop.DataBind();
        }
        else
        {
            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                lblSResult.Text = "Occurrence Span Code is invalid";
            }
            if (!string.IsNullOrEmpty(txtCodeDescription.Text)){
                lblSResult.Text = "Occurrence description is invalid";
            }
            lblSResult.Visible = true;
            //divGridOccurrenceSpanCode.Visible = false;           

        }
    }
    private DataSet GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvOccurenceCodeSpanSearchPop.GridViewSortDirection == SortDirection.Descending ? gvOccurenceCodeSpanSearchPop.GridViewSortColumn + " DESC" : gvOccurenceCodeSpanSearchPop.GridViewSortColumn;
        DataSet ds = GetOccurenceSpanCodeSearch();

        if (Helper.HasRows(ds))
        {
            return ds;
        }
        else return new DataSet();
    }
    private DataSet GetOccurenceSpanCodeSearch()
    {
        DataSet dsSpanCode;       
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("@CLAIMS_OCCURRENCE_CODE", txtCode.Text);
        parms.Add("@CLAIMS_OCCURRENCE_CODE_DESC", txtCodeDescription.Text);
        dsSpanCode = svc.SelectPanelsData("CLAIMS_OCCURRENCE_CODE", parms);
        return dsSpanCode;
    }

    protected void lnkOccurrenceCodeSpan_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;
        txtCode.Text = string.Empty;
        txtCodeDescription.Text = string.Empty;
        hdnOccurrenceSpanCode.Value = cmdArgument.ToString();
        hdnOccurrenceSpanCodeDec.Value = grdrow.Cells[1].Text;
        hdnStatus.Value = "";
    }
}