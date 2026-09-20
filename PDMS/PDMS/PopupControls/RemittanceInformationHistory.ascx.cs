using System;
using System.Data;

public partial class PopupControls_RemittanceInformationHistory : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadData();
    }

    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION_History");
        if (Helper.HasRows(ds))
        {
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

    public override void LoadData(DataRow row)
    {

    }

    public override string ValidationGroup
    {
        get { return "RemittanceInformationHistory"; }
    }

    public override string Title
    {
        get { return "Remittance Information History"; }
    }

    public override string IdText
    {
        get { return "ucRemittanceInformationHistory_" + this.WorkflowPage.RegistrationId; }
    }
}