using AjaxControlToolkit;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_SubmitClaimSearchProc : System.Web.UI.UserControl
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
    public string ProcedureCode
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnProcedureCode.Value))
                return hdnProcedureCode.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnProcedureCode.Value = value.Trim();
        }
    }

    #endregion
    public DataTable ProcDataTable;
    protected void Page_Load(object sender, EventArgs e)
    {
       if(!IsPostBack)
        {
            gvSubmitClaimSearchProcPop.DataSource = null;
            gvSubmitClaimSearchProcPop.DataBind();
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
        if( (string.IsNullOrEmpty(txtCode.Text))&&(string.IsNullOrEmpty(txtPlaceOfServiceName.Text)) )
        {
            lblError.Visible = true;
            lblError.Text = "Procedure Code or Procedure Code Description  is required for Search.";
            ModalPopupExtender ModalPopupExtender1 = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchProc");
            ModalPopupExtender1.Show();
            CleareField();
        }
        
        else
        {
            this.lblSResult.Visible = true;
            SessionVarRetriever.DashBoardPartyIds = SessionVarRetriever.DashBoardRegistrationIds = string.Empty;
            SessionVarRetriever.DashBoardTableId = 0;

            this.gvSubmitClaimSearchProcPop.CurrentPageIndex = 0;
            lblError.Visible = false;
            RefreshData();

            if (!string.IsNullOrEmpty(txtCode.Text))
            {
                foreach (DataRow dr in ProcDataTable.Rows)
                {
                    if (dr.ItemArray[0].ToString().Trim() == txtCode.Text.Trim())
                    {
                        Session["ProcedureCode"] = txtCode.Text;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(txtPlaceOfServiceName.Text) && Session["ProcedureCode"] == null)
            {
                foreach (DataRow dr in ProcDataTable.Rows)
                {
                    if (dr.ItemArray[1].ToString().Trim() == txtPlaceOfServiceName.Text.Trim())
                    {
                        Session["ProcedureCode"] = dr.ItemArray[1].ToString();
                        break;
                    }
                }
            }

            ModalPopupExtender ModalPopupExtender1 = (ModalPopupExtender)this.Parent.FindControl("mpeSubmitClaimSearchProc");
            ModalPopupExtender1.Show();
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

    


   
    public void RefreshData()
    {
        ProcDataTable = GetData(gvSubmitClaimSearchProcPop.PageSize);
        if (Helper.HasRows(ProcDataTable))
        {
            gvSubmitClaimSearchProcPop.DataSource = ProcDataTable;
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
        DataTable dt = LookupTableController.GetProcedureCodeServiceDetail(txtCode.Text.TrimEnd(), txtPlaceOfServiceName.Text.TrimEnd(), false, false);

        if (Helper.HasRows(dt))
        {
            return dt;
        }
        else return new DataTable();
    }

    private DataTable GetMockData()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ProcCode"));
        dt.Columns.Add(new DataColumn("ProcDesc"));

        dt.Rows.Add("5006009000", "Ibuprofen");
        dt.Rows.Add("5006609000", "Ibuprofen");
        return dt;
    }

    protected void lnkProcCode_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        string cmdArgument = btn.CommandArgument;
        Session["ProcedureCode"] = cmdArgument;
        hdnProcedureCode.Value = cmdArgument.ToString();
        CleareField();

    }

    public void CleareField()
    {
        txtCode.Text = string.Empty;
        txtPlaceOfServiceName.Text = string.Empty;
        gvSubmitClaimSearchProcPop.DataSource = null;
        gvSubmitClaimSearchProcPop.DataBind();
    }
}