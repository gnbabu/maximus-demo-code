using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchRevenueCode : System.Web.UI.UserControl
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

    public string RevenueCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnRevenueCode.Value))
                return hdnRevenueCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnRevenueCode.Value = value.Trim();
        }
    }

    public DataTable RevenueCodeDataTable;

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            gvSubmitClaimSearchRevPop.DataSource = null;
            gvSubmitClaimSearchRevPop.DataBind();
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // Clear these out prior to doing the Search
        if ((string.IsNullOrEmpty(txtCode.Text)) && (string.IsNullOrEmpty(txtPlaceOfServiceName.Text)))
        {
            lblRevenueError.Visible = true;
            lblRevenueError.Text = "Revenue Code or Revenue Code Description  is required for Search.";
            ModalPopupExtender ModalPopupExtender1 = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchRevenueCode");
            ModalPopupExtender1.Show();
            CleareField();
        }

        else
        {

            this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvSubmitClaimSearchRevPop.CurrentPageIndex = 0;
            RefreshData();

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                foreach (DataRow dr in RevenueCodeDataTable.Rows)
                {
                    if (dr.ItemArray[0].ToString().Trim() == txtCode.Text.Trim())
                    {
                        Session["RevenueCode"] = txtCode.Text;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtPlaceOfServiceName.Text) && Session["RevenueCode"] == null)
            {
                foreach (DataRow dr in RevenueCodeDataTable.Rows)
                {
                    if (dr.ItemArray[1].ToString().Trim() == txtPlaceOfServiceName.Text.Trim())
                    {
                        Session["RevenueCode"] = dr.ItemArray[1].ToString();
                        break;
                    }
                }
            }

            ModalPopupExtender ModalPopupExtender1 = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchRevenueCode");
            ModalPopupExtender1.Show();
        }
    }
    protected void gvSubmitClaimSearchRevPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
        CleareField();
    }

    protected void gvSubmitClaimSearchRevPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        RefreshData();
    }
    protected void LinkButton8_Click(object sender, EventArgs e)
    {

    }

    public void RefreshData()
    {
        lblRevenueError.Text = "";
        RevenueCodeDataTable = GetData(gvSubmitClaimSearchRevPop.PageSize);
        if (Helper.HasRows(RevenueCodeDataTable))
        {
            gvSubmitClaimSearchRevPop.DataSource = RevenueCodeDataTable;
            gvSubmitClaimSearchRevPop.DataBind();
        }
        else
        {
            gvSubmitClaimSearchRevPop.DataSource = null;
            gvSubmitClaimSearchRevPop.DataBind();
        }
    }

    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvSubmitClaimSearchRevPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchRevPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchRevPop.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        DataTable dt = LookupTableController.GetRevenueCode(txtCode.Text.TrimEnd(), txtPlaceOfServiceName.Text.TrimEnd());

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }


    protected void lnkRevCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        Session["RevenueCode"] = cmdArgument;
        hdnRevenueCode.Value = cmdArgument.ToString();
        CleareField();
    }

    public void CleareField()
    {
        txtCode.Text = string.Empty;
        txtPlaceOfServiceName.Text = string.Empty;
        gvSubmitClaimSearchRevPop.DataSource = null;
        gvSubmitClaimSearchRevPop.DataBind();
    }
}