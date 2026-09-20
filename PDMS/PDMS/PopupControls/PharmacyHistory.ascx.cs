using System.Data;

public partial class PopupControls_PharmacyHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PHARMACY_PROVIDER_INFO");
        if (Helper.HasRows(ds))
        {
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }
}