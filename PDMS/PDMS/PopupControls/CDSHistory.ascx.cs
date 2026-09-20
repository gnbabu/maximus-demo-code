using System;
using System.Data;

public partial class PopupControls_CDSHistory : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        LoadControlData();
    }

    public override void LoadControlData()
    {
        LoadData();
    }
    public void LoadData()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "state_cds_number_history");
        if (Helper.HasRows(ds))
        {
            grdCDSHistory.DataSource = ds.Tables[0];
            grdCDSHistory.DataBind();
        }
    }

    public override void LoadData(DataRow row)
    {

    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "StateCDSHistory"; }
    }

    public override string Title
    {
        get { return "State CDS Number History"; }
    }

    public override string IdText
    {
        get { return "ucCDSHistory_" + this.WorkflowPage.RegistrationId; }
    }
}