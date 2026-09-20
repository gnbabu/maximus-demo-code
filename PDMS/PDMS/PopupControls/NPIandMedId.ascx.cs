using Presentation.Interfaces;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_NPIandMedId : BaseSectionControl, INPIandMedIdView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override bool HasInputValue()
    {
        bool rtn = true;
        // do some validation
        return rtn;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Page.Title = "NPI and Med ID";
        ucEnrollmentData.InitViewForProvider(this.WorkflowPage.RegistrationId, this.WorkflowPage.WF_ProcessID);
    }

    public override void LoadControlData()
    {

    }

    public override void LoadData(DataRow addressRow)
    {
    }

    public override bool SaveData()
    {
        bool isValid = true;
        // Page.Validate("");
        int result = ucEnrollmentData.SaveData();
        if (result == 1)
        {
            AddError("* Enter Provider Effective Date.", ref isValid, ValidationGroup);
        }
        else if (result == 2)
        {
            AddError("* Enter Provider Effective Date.", ref isValid, ValidationGroup);
        }
        else if (result == 3)
        {
            AddError("* Date spans cannot overlap.", ref isValid, ValidationGroup);
        }
        else if (result == 4)
        {
            AddError("* End date for inactive span must be entered.", ref isValid, ValidationGroup);
        }
        else if (result == 5)
        {
            AddError("* End date cannot be less than effective date.", ref isValid, ValidationGroup);
        }
        else if (result == 6)
        {
            AddError("* Select a Radio button.", ref isValid, ValidationGroup);
        }
        else if (result == 7)
        {
            AddError("* End date for Reporting Only span must be entered.", ref isValid, ValidationGroup);
        }
        return isValid;
    }

    private void AddError(string errMsg, ref bool isGood, string validationGroup)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = errMsg,
            ValidationGroup = validationGroup
        };
        this.Page.Validators.Add(validator);
        isGood = false;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "NPIandMedId"; }
    }

    public override string Title
    {
        get { return "NPI and Med ID"; }
    }

    public override string IdText
    {
        get { return "ucNPIandMedId_" + this.WorkflowPage.RegistrationId; }
    }
}