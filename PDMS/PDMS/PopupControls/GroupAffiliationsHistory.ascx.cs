using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_GroupAffiliationsHistory : System.Web.UI.UserControl
{
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    private const int pageSize = 10;
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
   
    public void LoadData()
    {
        BindSearchResultsGrid(1);
    }

    private void BindSearchResultsGrid(int toPageNumber)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetGroupAffiliationHistory(this.WorkflowPage.RegistrationId, pageSize, toPageNumber);
        int searchResultsTotalRows = Helper.GetInt("TOTAL", ds.Tables[1].Rows[0]);
        BindPager(searchResultsTotalRows, toPageNumber);
        grd.DataSource = ds.Tables[0];
        grd.DataBind();
        upGrd.Update();
    }

    protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            BindSearchResultsGrid(Convert.ToInt32(e.CommandArgument));
        }
    }

    private void BindPager(int totalRows, int currentPage)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalRows / pageSize);
        List<ListItem> pagerContainer = new List<ListItem>();
        for (int i = 1; i <= totalPages; i++)
        {
            pagerContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage == i ? false : true));
        }

        if (pagerContainer.Count > 0) pagerContainer[0].Text = "First";
        if (pagerContainer.Count > 1) pagerContainer[totalPages - 1].Text = "Last";
        dlPager.DataSource = pagerContainer;
        dlPager.DataBind();
    }
    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
}