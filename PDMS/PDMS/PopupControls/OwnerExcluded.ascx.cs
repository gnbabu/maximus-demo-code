using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerExcluded : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegOwnerExcludedID.Value = txtExclusionBeginDate.Text = txtExclusionEndDate.Text = txtExclusionReason.Text = string.Empty;
        Helper.LoadList(ddlOwner, this.WorkflowPage.RegistrationOwnersList, "NAME", "REG_OWNER_ID", true);
        ddlOwner.SelectedIndex = 0;

        if (dr != null)
        {
            hdnRegOwnerExcludedID.Value = Helper.GetString("REG_OWNER_EXCLUDED_ID", dr);
            txtExclusionBeginDate.Text = Helper.GetDate("EXCLUSION_BEGIN_DATE", dr);
            txtExclusionEndDate.Text = Helper.GetDate("EXCLUSION_END_DATE", dr);
            txtExclusionReason.Text = Helper.GetString("EXCLUSION_REASON", dr);
            ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_OWNER_ID", ddlOwner.SelectedValue);
        parms.Add("EXCLUSION_BEGIN_DATE", txtExclusionBeginDate.Text);
        parms.Add("EXCLUSION_END_DATE", txtExclusionEndDate.Text);
        parms.Add("EXCLUSION_REASON", txtExclusionReason.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerExcludedID.Value))
        {
            // Update
            parms.Add("REG_OWNER_EXCLUDED_ID", hdnRegOwnerExcludedID.Value);
            psc.UpdateRegistrationDataTable("OWNER_EXCLUDED", parms);
        }
        else
        {
            // Insert
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("OWNER_EXCLUDED", parms);
        }
    }
}