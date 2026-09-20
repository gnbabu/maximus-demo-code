using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Telerik.Web.UI.Skins;

public partial class PopupControls_WorkHistory : System.Web.UI.UserControl
{
    private DataTable dataTable;
    private string GridViewSortDirection
    {

        get { return ViewState["SortDirection"] == null ? "ASC" : ViewState["SortDirection"].ToString(); }

        set
        {
            ViewState["SortDirection"] = value;
        }
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private string GridViewSortExpression
    {
        get
        {
            return ViewState["SortExpression"] == null ? "Name" : ViewState["SortExpression"].ToString();
        }
        set
        {
            ViewState["SortExpression"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData((DataTable)Session["dtWorkHistoryResponse"]);
        }
    }
    public void LoadData(DataTable dt)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_WorkHistory_History", parms);

        if (ds.Tables.Count > 0)
        {
            Session["dtWorkHistoryResponse"] = ds.Tables[0];
            grdWorkHistory.CurrentPageIndex = 0;
            grdWorkHistory.DataSource = ds.Tables[0];
            grdWorkHistory.DataBind();

            foreach (GridColumn column in grdWorkHistory.MasterTableView.OwnerGrid.Columns)
            {
                column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                column.CurrentFilterValue = string.Empty;
            }
            grdWorkHistory.MasterTableView.FilterExpression = string.Empty;
            grdWorkHistory.CurrentPageIndex = 0;
            grdWorkHistory.PageSize = 10;
            grdWorkHistory.Rebind();
        }
    }
    protected string GetContactDetails(object name, object email, object phone)
    {
        string contact = string.Empty;
        string contactName = string.IsNullOrEmpty(name.ToString()) ? "" : name.ToString();
        string contactEmail = string.IsNullOrEmpty(email.ToString()) ? "" : email.ToString();
        string contactPhone = string.IsNullOrEmpty(phone.ToString()) ? "" : phone.ToString();
        contact = Helper.GetFormattedContact(contactName, contactEmail, contactPhone);

        return contact;
    }

    protected void grdWorkHistory_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        dataTable = (DataTable)Session["dtWorkHistoryResponse"];
        grdWorkHistory.DataSource = dataTable;
        grdWorkHistory.CurrentPageIndex = e.NewPageIndex;        
        grdWorkHistory.DataBind();
    }

    protected void grdWorkHistory_ClearAllFilters_Click(object sender, EventArgs e)
    {
        foreach (GridColumn column in grdWorkHistory.MasterTableView.Columns)
        {
            column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;

            column.CurrentFilterValue = string.Empty;
            column.AndCurrentFilterValue = string.Empty;
        }
        grdWorkHistory.MasterTableView.FilterExpression = string.Empty;
        grdWorkHistory.MasterTableView.Rebind();
    }

    protected void grdWorkHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        dataTable = (DataTable)Session["dtWorkHistoryResponse"];
        grdWorkHistory.DataSource = dataTable;        
    }

    protected void grdWorkHistory_Sorting(object sender, GridViewSortEventArgs e)
    {
        dataTable = (DataTable)Session["dtWorkHistoryResponse"];
        GridViewSortExpression = e.SortExpression;      
        grdWorkHistory.DataSource = SortDataTable(dataTable); ;
        grdWorkHistory.DataBind();        
    }

    protected void grdWorkHistory_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e) 
    {
        dataTable = (DataTable)Session["dtWorkHistoryResponse"];
        grdWorkHistory.DataSource = dataTable;
    }
    protected void grdWorkHistory_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        dataTable = (DataTable)Session["dtWorkHistoryResponse"];
        grdWorkHistory.DataSource = dataTable;        
    }
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }

    protected DataView SortDataTable(DataTable myDataTable)
    {
        if (myDataTable != null)
        {
            DataView myDataView = new DataView(myDataTable);
            if (GridViewSortExpression != string.Empty)
            {
                myDataView.Sort = string.Format("{0} {1}",
                GridViewSortExpression, GetSortDirection());
            }
            return myDataView;
        }
        else
        {
            return new DataView();
        }
    }
}