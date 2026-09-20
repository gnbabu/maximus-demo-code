using AjaxControlToolkit;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchNDC : System.Web.UI.UserControl
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

    #region Properties
    public string NDC_Code
    {
        get
        {
            return hdnNDC_Code.Value;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnNDC_Code.Value = value.Trim();
            ClearHiddenField(value);
        }
    }
    public GridView GridViewSubmitClaimSearchPop
    {
        get { return gvSubmitClaimSearchNDCPop; }
    }
    public Label LabelError
    {
        get { return fieldRequireError; }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //RefreshData();
        }
    }
    protected void ClearHiddenField(string value)
    {
        hdnNDC_Code.Value = value;
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
        ModalPopupExtender modalNDC = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchNDC");
        if (!((txtCode.Text == "") && (txtTradeName.Text == "")))
        {
            if (!string.IsNullOrEmpty(txtCode.Text.ToString()) && txtCode.Text.Length < 11)
            {
                fieldRequireError.Text = "NDC should be 11-digit number.";
                gvSubmitClaimSearchNDCPop.DataSource = null;
                gvSubmitClaimSearchNDCPop.DataBind();
                modalNDC.Show();
                return;
            }
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;
            this.gvSubmitClaimSearchNDCPop.CurrentPageIndex = 0;
            RefreshData();
            modalNDC.Show();
            //txtCode.Text = "";
            //txtTradeName.Text = "";
            fieldRequireError.Text = "";
        }
        else
        {
            fieldRequireError.Text = "NDC code or Trade name is required";
            modalNDC.Show();
            txtCode.Text = "";
            txtTradeName.Text = "";
            gvSubmitClaimSearchNDCPop.DataSource = null;
            gvSubmitClaimSearchNDCPop.DataBind();
        }

    }

    protected void gvSubmitClaimSearchNDCPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        //RefreshData();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    protected void gvSubmitClaimSearchNDCPop_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //RefreshData();
    }

    protected void lnkNDC_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;

        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer;

        txtCode.Text = string.Empty;
        txtTradeName.Text = string.Empty;

        hdnNDC_Code.Value = cmdArgument.ToString();
        gvSubmitClaimSearchNDCPop.DataSource = null;
        gvSubmitClaimSearchNDCPop.DataBind();

    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvSubmitClaimSearchNDCPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvSubmitClaimSearchNDCPop.DataSource = dt;
            gvSubmitClaimSearchNDCPop.DataBind();
        }
        else
        {
            gvSubmitClaimSearchNDCPop.DataSource = null;
            gvSubmitClaimSearchNDCPop.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = new DataSet();
        string sortColWithDirection = this.gvSubmitClaimSearchNDCPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchNDCPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchNDCPop.GridViewSortColumn;

        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records
        ds = GetNDCData();

        if (Helper.HasRows(ds))
        {
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    private DataSet GetNDCData()
    {
        List<SqlParameter> parameters = new List<SqlParameter>();
        parameters.Add(SqlParms.CreateParameter("NDCCode", DbType.String, txtCode.Text, true));
        parameters.Add(SqlParms.CreateParameter("TradeName", DbType.String, txtTradeName.Text, true));

        DataSet dataSetNDC = DataAccess.ExecuteStoredProcedure("Usp_Select_CLAIMS_NDC_CODE", parameters, "CLAIMS_NDC_CODE");

        return dataSetNDC;
    }
}