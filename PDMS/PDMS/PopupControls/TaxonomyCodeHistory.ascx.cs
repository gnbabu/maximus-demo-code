using System.Data;

public partial class PopupControls_TaxonomyCodeHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "TAXONOMY_History");
        if (Helper.HasRows(ds))
        {
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }
}