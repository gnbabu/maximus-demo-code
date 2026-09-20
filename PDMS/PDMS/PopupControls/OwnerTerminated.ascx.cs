using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerTerminated : BasePopupControl
{
    
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegOwnerTerminatedID.Value = txtTerminationBeginDate.Text = txtTerminationEndDate.Text = txtTerminationReason.Text = string.Empty;
        Helper.LoadDropDownListWithStates(ref ddlState);
        Helper.LoadList(ddlOwner, this.WorkflowPage.RegistrationOwnersList, "NAME", "REG_OWNER_ID", true);
        ddlOwner.SelectedIndex = 0;

        if (dr != null)
        {
            hdnRegOwnerTerminatedID.Value = Helper.GetString("REG_OWNER_TERMINATED_ID", dr);
            txtTerminationBeginDate.Text = Helper.GetDate("TERMINATION_BEGIN_DATE", dr);
            txtTerminationEndDate.Text = Helper.GetDate("TERMINATION_END_DATE", dr);
            txtTerminationReason.Text = Helper.GetString("TERMINATION_REASON", dr);
            ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
            ddlState.SelectedValue = Helper.GetString("TERMINATION_STATE", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_OWNER_ID", ddlOwner.SelectedValue);
        parms.Add("TERMINATION_BEGIN_DATE", txtTerminationBeginDate.Text);
        parms.Add("TERMINATION_END_DATE", txtTerminationEndDate.Text);
        parms.Add("TERMINATION_REASON", txtTerminationReason.Text);
        parms.Add("TERMINATION_STATE", ddlState.SelectedValue);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerTerminatedID.Value))
        {
            // Update
            parms.Add("REG_OWNER_TERMINATED_ID", hdnRegOwnerTerminatedID.Value);
            psc.UpdateRegistrationDataTable("OWNER_TERMINATED", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("OWNER_TERMINATED", parms);
        }
    }
}