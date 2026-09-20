using System;

public partial class PopupControls_EmploymentHistory : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
    }

    public override bool SaveData()
    {
        return true;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string Title
    {
        get { return "Employment History"; }
    }

    public override string IdText
    {
        get { return "ucEmploymentHistory_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "vgEmploymentHistory"; }
    }
}