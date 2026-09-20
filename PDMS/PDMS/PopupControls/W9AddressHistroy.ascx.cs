using System.Data;

public partial class PopupControls_W9AddressHistroy : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION_History");
        if (Helper.HasRows(ds))
        {
            grdW9Address1.DataSource = ds.Tables[0];
            grdW9Address1.DataBind();
        }

    }
}