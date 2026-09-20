using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerTransaction : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public override void LoadData(DataRow dr)
    {
        //ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") ? "block" : "none";
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (this.WorkflowPage.AdditionalDisclosureList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.AdditionalDisclosureList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.SubcontractorIndividual + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.SubcontractorOrganization + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.SupplierIndividual + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.SupplierOrganization + "'");
       
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();
		ddlOwner.Items.Clear();
        Helper.LoadList(ddlOwner, dt, "NAME", "REG_OWNER_ID", true);
        hdnRegOwnerTransactionID.Value = txtCMPDate.Text = string.Empty;
        nbAmount.Text = string.Empty;
        


        if (dr != null)
        {
            hdnRegOwnerTransactionID.Value = Helper.GetString("REG_OWNER_Transaction_ID", dr);
            txtCMPDate.Text = Helper.GetDate("Transaction_DATE", dr);
            ddlOwner.SelectedValue = Helper.GetString("OWNER_TRANSACTION_ID", dr);
            nbAmount.Text = Helper.GetString("Transaction_AMOUNT", dr);
            
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();

        parms.Add("Transaction_AMOUNT", nbAmount.Text);
        parms.Add("Transaction_DATE", txtCMPDate.Text);
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("OWNER_TRANSACTION_ID", ddlOwner.SelectedValue);
        parms.Add("Person_Name", ddlOwner.SelectedItem.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerTransactionID.Value))
        {
            // Update
            parms.Add("REG_OWNER_Transaction_ID", hdnRegOwnerTransactionID.Value);
            psc.UpdateRegistrationDataTable("OWNER_Transaction", parms);
        }
        else
        {
            // Insert
            psc.InsertRegistrationDataTable("OWNER_Transaction", parms);
        }
    }

    protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    
}