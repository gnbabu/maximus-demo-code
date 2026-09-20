using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class UserControls_EmailQueueListing : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        GetItemsInQueue();
    }

    public void GetItemsInQueue()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        List<EmailQueueItem> data = svc.GetEmailsInQueue();
        grdQueueItems.DataSource = data;
        grdQueueItems.DataBind();
        btnSendBulkEmailInBatch.Visible = data.Count > 0;
        btnSendBulkEmailNow.Visible = data.Count > 0;
    }

    protected void grdQueueItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdQueueItems.PageIndex = e.NewPageIndex;
        GetItemsInQueue();
    }

    protected void btnSendBulkEmailNow_Click(object sender, EventArgs e)
    {
        try
        {
            MassEmailProcessor massEmailProcessor = new MassEmailProcessor(Guid.NewGuid());
            massEmailProcessor.ExecuteJob();
            GetItemsInQueue();
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void btnSendBulkEmailInBatch_Click(object sender, EventArgs e)
    {
        //MassEmailProcessor massEmailProcessor = new MassEmailProcessor(Guid.NewGuid());
        //massEmailProcessor.ExecuteJob();
    }
}