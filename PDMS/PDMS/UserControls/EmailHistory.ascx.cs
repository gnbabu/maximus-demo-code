using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class UserControls_EmailHistory : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindHistoryGrid();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        BindHistoryGrid();
    }

    private void BindHistoryGrid()
    {
        try
        {

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            List<EmailBatch> results = svc.GetAllBatchJobs();

            results = results.OrderBy(x => x.JobComplete).ThenByDescending(x => x.EmailBatchId).ToList();

            grdHistory.DataSource = results;
            grdHistory.DataBind();

        }
        catch (Exception)
        {
            throw;
        }
    }


    protected void grdHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdHistory.PageIndex = e.NewPageIndex;
        BindHistoryGrid();
    }



}