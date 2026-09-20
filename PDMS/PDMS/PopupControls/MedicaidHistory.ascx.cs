using System.Data;

public partial class PopupControls_MedicaidHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAIDHistory");
        if (Helper.HasRows(ds))
        {
            grdMedicaid.DataSource = ds.Tables[0];
            grdMedicaid.DataBind();
        }
    }
}