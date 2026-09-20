using System.Data;

public partial class PopupControls_MedicareHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAREHistory");
        if (Helper.HasRows(ds))
        {
            grdMedicare.DataSource = ds.Tables[0];
            grdMedicare.DataBind();
        }
    }
}