using System;
using System.Collections.Generic;
using System.Data;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_BoardCertificationHistory : BaseSectionControl
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
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "board_certification_history");
        if (Helper.HasRows(ds))
        {
            grdBoardCertificationHistory.DataSource = ds;
            grdBoardCertificationHistory.DataBind();
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
        get { return "BoardCertificationHistory"; }
    }

    public override string Title
    {
        get { return "Board Certification History"; }
    }

    public override string IdText
    {
        get { return "ucBoardCertificationHistory_" + this.WorkflowPage.RegistrationId; }
    }
}