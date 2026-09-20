using System;

public partial class PopupControls_MiscellaneousSection : BaseSectionControl
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
        ucMedicare.LoadControlData();
        ucMedicaid.LoadControlData();
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
        throw new NotImplementedException();
    }
    public override bool HasInputValue()
    {
        return ucMedicare.HasInputValue() && ucMedicaid.HasInputValue();
    }
    public override bool SaveData()
    {
        return ucMedicare.SaveData() && ucMedicaid.SaveData();
    }

    public override bool ValidateData()
    {
        return ucMedicare.ValidateData() && ucMedicaid.ValidateData();
    }

    public override string ValidationGroup
    {
        get { return "valMiscellaneous"; }
    }

    public override string Title
    {
        get { return "Miscellaneous"; }
    }

    public override string IdText
    {
        get { return "ucMiscellaneous_" + this.WorkflowPage.RegistrationId; }
    }
}