using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerPenalty : BasePopupControl
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

		DataTable RegistrationTable = new DataTable();
		DataTable AddDisclosureTable = new DataTable();
		DataRow[] RegistrationTableRow = null;
		DataRow[] AddDisclosureTableRow = null;
		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
		ddlOwner.Items.Clear();
		if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0 || this.WorkflowPage.AdditionalDisclosureList.Rows.Count > 0)
		{
			RegistrationTableRow = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.ManagingEmployee + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.Employee + "'");
			AddDisclosureTableRow = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Employee + "'");
		}
		if ((RegistrationTableRow != null && RegistrationTableRow.Length > 0) || (AddDisclosureTableRow != null && AddDisclosureTableRow.Length > 0))
		{
			AddDisclosureTable = AddDisclosureTableRow.Length > 0 ? AddDisclosureTableRow.CopyToDataTable() : null;
			RegistrationTable = RegistrationTableRow.Length > 0 ? RegistrationTableRow.CopyToDataTable() : null;
		}
		
		
		if (AddDisclosureTable != null && RegistrationTable != null)
		{
			RegistrationTable.Merge(AddDisclosureTable, true);
			Helper.LoadList(ddlOwner, RegistrationTable, "NAME", "REG_OWNER_ID", true);
		}
		else if (AddDisclosureTable == null && RegistrationTable != null)
		{
			Helper.LoadList(ddlOwner, RegistrationTable, "NAME", "REG_OWNER_ID", true);
		}
		else if (RegistrationTable == null && AddDisclosureTable != null)
		{
			Helper.LoadList(ddlOwner, AddDisclosureTable, "NAME", "REG_OWNER_ID", true);
		}

		if (dr != null)
        {
            hdnRegOwnerPenaltyID.Value = Helper.GetString("REG_OWNER_SANCTION_ID", dr);
            ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
            txtExplaination.Text = Helper.GetString("Explanation", dr).ToString();

        }

    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        // OwnerConvictionOnBehalf obj = new OwnerConvictionOnBehalf();        
        parms.Add("REG_OWNER_ID", ddlOwner.SelectedValue);
        parms.Add("Explanation", txtExplaination.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerPenaltyID.Value))
        {
            // Update
            parms.Add("REG_OWNER_SANCTION_ID", hdnRegOwnerPenaltyID.Value);
            psc.UpdateRegistrationDataTable("OWNER_SANCTION", parms);
        }
        else
        {
            // Insert
            
            psc.InsertRegistrationDataTable("OWNER_SANCTION", parms);
        }
    }
    protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ucAddress_StateChangedEvent(object sender, EventArgs e)
    {


    }
}