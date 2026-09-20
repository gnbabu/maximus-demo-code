using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

public partial class PopupControls_OwnerOriginal : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        hdnRegOriginalOwnerID.Value = txtOwnerName.Text = txtTransferDate.Text = txtTransferPlace.Text = string.Empty;
        nbTaxID.Text = string.Empty;

        if (dr != null)
        {
            hdnRegOriginalOwnerID.Value = Helper.GetString("REG_ORIGINAL_OWNER_ID", dr);
            txtOwnerName.Text = Helper.GetString("NAME", dr);
            txtTransferDate.Text = Helper.GetDate("TRANSFER_DATE", dr);
            txtTransferPlace.Text = Helper.GetString("TRANSFER_PLACE", dr);
            nbTaxID.Text = Helper.GetString("TAX_ID", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("NAME", txtOwnerName.Text);
        parms.Add("TAX_ID", nbTaxID.Text);
        parms.Add("TRANSFER_PLACE", txtTransferPlace.Text);
        parms.Add("TRANSFER_DATE", txtTransferDate.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOriginalOwnerID.Value))
        {
            // Update
            parms.Add("REG_ORIGINAL_OWNER_ID", hdnRegOriginalOwnerID.Value);
            psc.UpdateRegistrationDataTable("ORIGINAL_OWNER", parms);
        }
        else
        {
            // Insert
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("ORIGINAL_OWNER", parms);
        }
    }
}