using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Process_ResubmitTransaction : System.Web.UI.Page
{
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
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetGridData(false);
            Helper.LoadDropDown(ddlTransactionType, svc.SelectAllTransactionTypes().Tables[0], "TRANSACTION_TYPE", "TRANSACTION_TYPE_ID", true);
        }
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        SetGridData(true);
    }

    protected void gvTrans_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int tid;
        if (!int.TryParse(e.CommandArgument.ToString(), out tid)) return;

        Guid uid = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
        if (uid == null) return;

        switch(e.CommandName)
        {
            case "CancelTransaction":
                svc.CancelTransaction(tid, DateTime.Now, uid.ToString());
                break;
            case "ResubmitTransaction":
                svc.ResubmitProcessedTransaction(tid, DateTime.Now, uid.ToString());
                break;
            default:
                break;
        }

        SetGridData(true);
    }

    protected void gvTrans_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.FindControl("lbtnCancelTransaction").Visible = string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "CancelDateTime")));
            e.Row.FindControl("lbtnResubmitTransaction").Visible = !string.IsNullOrEmpty(Convert.ToString(DataBinder.Eval(e.Row.DataItem, "ProcessDateTime")));
        }
    }

    private void SetGridData(bool updatePanel)
    {
        DataSet ds = svc.SelectTransactions(
        ddlTransactionType.SelectedIndex > 0 ? ddlTransactionType.SelectedValue : null,
        string.IsNullOrEmpty(txtProcessStartDate.Text) ? null : txtProcessStartDate.Text,
        string.IsNullOrEmpty(txtProcessEndDate.Text) ? null : txtProcessEndDate.Text,
        string.IsNullOrEmpty(txtPartyID.Text) ? null : txtPartyID.Text,
        string.IsNullOrEmpty(txtServiceLocationID.Text) ? null : txtServiceLocationID.Text,
        string.IsNullOrEmpty(txtMedicaidID.Text) ? null : txtMedicaidID.Text,
        string.IsNullOrEmpty(txtCaqhID.Text) ? null : txtCaqhID.Text,
        null,
        null
        );

        gvTrans.DataSource = Helper.HasRows(ds) ? ds.Tables[0] : null;
        gvTrans.DataBind();

        if (updatePanel) upTransactions.Update();
    }
}