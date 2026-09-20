using System;

public partial class PopupControls_PharmacySection : BaseSectionControl
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
        ucPharmacyProviders.LoadControlData();
        ucPharmacyPharmacist.LoadControlData();
    }

    public override void LoadData(System.Data.DataRow dr = null)
    {
        throw new NotImplementedException();
    }

    public override bool SaveData()
    {
        return ucPharmacyProviders.SaveData() && ucPharmacyPharmacist.SaveData();
    }
    public override bool HasInputValue()
    {
        return ucPharmacyProviders.HasInputValue() && ucPharmacyPharmacist.HasInputValue();
    }
    public override bool ValidateData()
    {
        return ucPharmacyProviders.ValidateData() && ucPharmacyPharmacist.ValidateData();
    }

    public override string Title
    {
        get { return "Pharmacy Details"; }
    }

    public override string IdText
    {
        get { return "ucPharmacy_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valPharmacy"; }
    }
}