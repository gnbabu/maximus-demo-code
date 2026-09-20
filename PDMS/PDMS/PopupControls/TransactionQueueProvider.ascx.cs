using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
// JIRA 2961
public partial class UserControls_TransactionQueueProvider : BaseSectionControl
{
    public delegate void RegistrationViewEventHandler(int registrationId);
    public event RegistrationViewEventHandler RegistrationViewEvent;

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }


    public override string ValidationGroup
    {
        //get { return "valWorkflowSteps"; }
        get { return "NA"; }

    }

    public override string Title
    {
        get { return "Transaction Queue"; }
    }

    public override string IdText
    {
        //get { return "ucWorkflowSteps_" + this.WorkflowPage.RegistrationId; }
        get { return "NA"; }

    }



    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Transaction Queue";
    }
    public override void LoadControlData()
    {

    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // no action
        }


        RefreshData();
    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool SaveData()
    {
        return false;
    }

    public override bool ValidateData()
    {
        return false; // true;
    }



    #region Properties
    private bool UserCanRapidAdminRegistrations
    {
        get
        {
            return Helper.IsLoggedInUserInAdminRole() || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name);
        }
    }

    private int SelectedRegID
    {
        get
        {
            return ViewState["SelectedRegID"] == null ? 0 : Convert.ToInt32(ViewState["SelectedRegID"]);
        }
        set
        {
            ViewState["SelectedRegID"] = value;
        }
    }

    #endregion

    #region Page Events



    #endregion



    #region Public Methods

    public void RefreshData()
    {
        int totalResultCount = 0;
        // new telerik grid 
        DataTable dtQ = GetDataQ(out totalResultCount, 15);

        RadGridQ.DataSource = dtQ;
        RadGridQ.VirtualItemCount = totalResultCount;
        RadGridQ.DataBind();

    }
    #endregion

    #region Private Methods

    private DataTable GetDataQ(out int totalResultCount, int pageSize)
    {

        var gc = RadGridQ.MasterTableView.Columns.FindByUniqueName("Col_0") as GridBoundColumn;
        gc.ItemStyle.Font.Bold = false; // change default first column font bold 

        string txtStatus = string.Empty;
        string txtRegId = this.WorkflowPage.RegistrationId.ToString();  //     string.Empty;
        string txtMedicaidId = string.Empty;
        string txtFromDate = string.Empty;
        string txtToDate = string.Empty;
        string sortColWithDirection = string.Empty;
        int intPageSize = 15;
        int intRowIndex = 0;
        //int intTotalResultCount = 0;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        //// this.gvTransactions.GridViewSortColumn
        //string sortColWithDirection = this.gvTransactions.GridViewSortDirection == SortDirection.Descending ? gvTransactions.GridViewSortColumn + " DESC" : gvTransactions.GridViewSortColumn;
        //totalResultCount = 0;

        //ds = psc.SearchTransactions(ddlStatus.SelectedValue, txtRegId.Text, txtMedicaidId.Text, txtFromDate.Text, txtToDate.Text
        //    , sortColWithDirection, pageSize, gvTransactions.CurrentRowIndex, true, out totalResultCount);

        //DataSet ds;
        // this.gvTransactions.GridViewSortColumn
        //string sortColWithDirection = this.gvTransactions.GridViewSortDirection == SortDirection.Descending ? gvTransactions.GridViewSortColumn + " DESC" : gvTransactions.GridViewSortColumn;
        //int totalResultCount = 0;

        ds = psc.SearchTransactions(
            txtStatus,
            txtRegId,
            txtMedicaidId,
            txtFromDate,
            txtToDate, sortColWithDirection,
            intPageSize,
            intRowIndex,
            true,
            null,
            out totalResultCount);


        if (Helper.HasRows(ds))
        {
            //totalResultCount = ds.Tables[0].Rows.Count;
            return ds.Tables[0];
        }
        else return new DataTable();
    }

    #endregion
}