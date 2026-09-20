using System;
using System.Collections.Generic;
using System.Data;

public partial class PopupControls_MalpracticeClaimHistory : BaseSectionControl
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
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "malpractice_claim_history");
        if (Helper.HasRows(ds))
        {
            grdMalPracticeClaimHistory.DataSource = ds.Tables[0];
            grdMalPracticeClaimHistory.DataBind();
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
        get { return "MalpracticeClaimHistory"; }
    }

    public override string Title
    {
        get { return "Malpractice Claims History"; }
    }

    public override string IdText
    {
        get { return "ucMalpracticeClaimHistory_" + this.WorkflowPage.RegistrationId; }
    }
}