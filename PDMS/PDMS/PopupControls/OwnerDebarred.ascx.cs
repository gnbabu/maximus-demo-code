using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerDebarred : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegOwnerDebarredID.Value = txtDebarmentDate.Text = txtDebarmentDuration.Text = txtDebarmentReason.Text = string.Empty;
        Helper.LoadList(ddlOwner, this.WorkflowPage.RegistrationOwnersList, "NAME", "REG_OWNER_ID", true);
        ddlOwner.SelectedIndex = 0;

        if (dr != null)
        {
            hdnRegOwnerDebarredID.Value = Helper.GetString("REG_OWNER_DEBARRED_ID", dr);
            txtDebarmentDate.Text = Helper.GetDate("DEBARMENT_DATE", dr);
            txtDebarmentDuration.Text = Helper.GetString("DEBARMENT_DURATION", dr);
            txtDebarmentReason.Text = Helper.GetString("DEBARMENT_REASON", dr);
            ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_OWNER_ID", ddlOwner.SelectedValue);
        parms.Add("DEBARMENT_DATE", txtDebarmentDate.Text);
        parms.Add("DEBARMENT_DURATION", txtDebarmentDuration.Text);
        parms.Add("DEBARMENT_REASON", txtDebarmentReason.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerDebarredID.Value))
        {
            // Update
            parms.Add("REG_OWNER_DEBARRED_ID", hdnRegOwnerDebarredID.Value);
            psc.UpdateRegistrationDataTable("OWNER_DEBARRED", parms);
        }
        else
        {
            // Insert
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("OWNER_DEBARRED", parms);
        }
    }
}