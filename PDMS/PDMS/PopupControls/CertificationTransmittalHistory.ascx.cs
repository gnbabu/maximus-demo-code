using System.Data;

public partial class PopupControls_CertificationTransmittalHistory : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CERTIFICATION_History");
        if (Helper.HasRows(ds))
        {
            grdCertificationTransmittalHistory.DataSource = ds.Tables[0];
            grdCertificationTransmittalHistory.DataBind();
        }
    }
}