using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimICD10ProcedureCodes : BasePopupControl
{
    #region spa
    private PDMSService.PDMSServiceClient _spa;



    #endregion
    #region Properties

    
    public string ICD_PROCEDURE_CODE_ID
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnICD_PROCEDURE_CODE_ID.Value))
                return hdnICD_PROCEDURE_CODE_ID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnICD_PROCEDURE_CODE_ID.Value = value.Trim();
        }
    }

    public string ICD_PROCEDURE_CODE_Description
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnICD_Description.Value))
                return hdnICD_Description.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnICD_Description.Value = value.Trim();
        }
    }

    #endregion

    public void clearhiddendata()
    {
        hdnICD_PROCEDURE_CODE_ID.Value = string.Empty;
        hdnICD_Description.Value = string.Empty;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvSubmitClaimSearchProcPop.DataSource = null;
            gvSubmitClaimSearchProcPop.DataBind();
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if ((string.IsNullOrEmpty(txtCode.Text)) && (string.IsNullOrEmpty(txtPlaceOfServiceName.Text)))
        {
            lblError.Visible = true;
            lblError.Text = "ICD Procedure  Code or ICD Procedure Code Description  is required for Search.";
            ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
            mpeSubmitClaimSearchPop.Show();
            CleareField();
        }
        else
        {
            // Clear these out prior to doing the Search
            if (!string.IsNullOrEmpty(txtCode.Text.ToString()) && txtCode.Text.Length < 7)
            {
                ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
                mpeSubmitClaimSearchPop.Show();
                lblError.Text = "ICD Procedure Code should be 7-digit number.";
                gvSubmitClaimSearchProcPop.DataSource = null;
                gvSubmitClaimSearchProcPop.DataBind();
                lblError.Visible = true;

            }
            else
            {
                this.lblSResultICD.Visible = true;
                SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
                SessionVarRetriever.DashBoardTableId = 0;
                gvSubmitClaimSearchProcPop.CurrentPageIndex = 0;
                RefreshData();
                ddlICDPrCodeInstiutional.SelectedIndex = 0;
                lblError.Visible = false;
                ModalPopupExtender mpeSubmitClaimSearchPop = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchPop");
                mpeSubmitClaimSearchPop.Show();
            }
        }

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
    protected void gvSubmitClaimSearchPop_Sorting(object sender, GridViewSortEventArgs e)
    {
        RefreshData();
    }
    public void RefreshData()
    {
        DataTable dt = GetData(gvSubmitClaimSearchProcPop.PageSize);
        if (Helper.HasRows(dt))
        {
            gvSubmitClaimSearchProcPop.DataSource = dt;
            gvSubmitClaimSearchProcPop.DataBind();
        }
        else
        {
            gvSubmitClaimSearchProcPop.DataSource = null;
            gvSubmitClaimSearchProcPop.DataBind();
        }
    }
    private DataTable GetData(int pageSize)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sortColWithDirection = this.gvSubmitClaimSearchProcPop.GridViewSortDirection == SortDirection.Descending ? gvSubmitClaimSearchProcPop.GridViewSortColumn + " DESC" : gvSubmitClaimSearchProcPop.GridViewSortColumn;
        
        // If list of IDs has been passed in, display in search results list.
        //write sp for getting records

        DataTable dt = LookupTableController.GetICDProcedureCode(txtCode.Text,txtPlaceOfServiceName.Text,ddlICDPrCodeInstiutional.SelectedValue);
        //DataTable dt = GetMockData();

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }

   

    protected void lnkProcCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        GridViewRow grdrow = (GridViewRow)((LinkButton)sender).NamingContainer; 
        hdnICD_PROCEDURE_CODE_ID.Value = cmdArgument.ToString();
        hdnICD_Description.Value = grdrow.Cells[2].Text;
        CleareField();
        lblError.Visible = false;
    }

    #region svc
    private PDMSService.PDMSServiceClient _svc;
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
    #endregion
    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;
    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;


    protected void btnSave_Click(object sender, EventArgs e)
    {
        _spa = new PDMSService.PDMSServiceClient();
        this.ValidateData();
        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }
        try
        {



            return;
        }
        catch (Exception ex)
        {

        }

    }
    private void ValidateData()
    {

    }

    public void CleareField()
    {
        txtCode.Text = string.Empty;
        txtPlaceOfServiceName.Text = string.Empty;
        ddlICDPrCodeInstiutional.SelectedIndex = 0;
        gvSubmitClaimSearchProcPop.DataSource = null;
        gvSubmitClaimSearchProcPop.DataBind();
    }


}