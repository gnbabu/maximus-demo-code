using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_Disclosures : BaseSectionControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string Title
    {
        get { return "Disclosures"; }
    }

    public override string IdText
    {
        get { return "ucDisclosures_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valDisclosures"; }
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private bool AddValidationErrorMessage(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valProviderInfoHeader";
        this.Page.Validators.Add(val);
        return false;
    }

    public override void LoadControlData()
    {
        ucD01.LoadData();
        ucD02.LoadData();
        ucD03.LoadData();
        ucD04.LoadData();
        ucD05.LoadData();
        ucD06.LoadData();
        ucD07.LoadData();
        ucD09.LoadData();
        ucD10.LoadData();
        ucD11.LoadData();
        ucD12.LoadData();
        ucD13.LoadData();
        ucD14.LoadData();
        ucD15.LoadData();
        ucD16.LoadData();
    }

    public override void LoadData(DataRow dr = null)
    {
        //throw new NotImplementedException();
    }

    public override bool SaveData()
    {
        return true;
        //throw new NotImplementedException();
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }

    public override bool ValidateData()
    {
        bool isGood = true;

        ucD01.ValidateData(ref isGood);
        ucD02.ValidateData(ref isGood);
        ucD03.ValidateData(ref isGood);
        ucD04.ValidateData(ref isGood);
        ucD05.ValidateData(ref isGood);
        ucD06.ValidateData(ref isGood);
        ucD07.ValidateData(ref isGood);
        ucD09.ValidateData(ref isGood);
        ucD10.ValidateData(ref isGood);
        ucD11.ValidateData(ref isGood);
        ucD12.ValidateData(ref isGood);
        ucD13.ValidateData(ref isGood);
        ucD14.ValidateData(ref isGood);
        ucD15.ValidateData(ref isGood);
        ucD16.ValidateData(ref isGood);

        return isGood;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}