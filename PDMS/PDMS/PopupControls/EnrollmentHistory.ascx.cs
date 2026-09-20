using System.Data;

public partial class PopupControls_EnrollmentHistory : BasePopupControl
{
    public void LoadData(int regId)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(regId, "EnrollmentHistory");

        grd.DataSource = ds;
        grd.DataBind();

    }
}