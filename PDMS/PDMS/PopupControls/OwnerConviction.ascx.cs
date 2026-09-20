using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerConviction : BasePopupControl
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
        DataRow[] RegistrationTableRow = null;
       
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
		ddlOwner.Items.Clear();
        txtExplaination.Text = string.Empty;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
			RegistrationTableRow = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.ManagingEmployee + "'");
        }
		if ((RegistrationTableRow != null && RegistrationTableRow.Length > 0) )
		{
			RegistrationTable = RegistrationTableRow.Length>0? RegistrationTableRow.CopyToDataTable():null;		}
		if ( RegistrationTable != null)
		{
			Helper.LoadList(ddlOwner, RegistrationTable, "NAME", "REG_OWNER_ID", true);
		}
		
		if (dr != null)
        {
            hdnRegOwnerConvictionID.Value = Helper.GetString("REG_OWNER_CONVICTION_ID", dr);
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


        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerConvictionID.Value))
        {
            // Update
            parms.Add("REG_OWNER_CONVICTION_ID", hdnRegOwnerConvictionID.Value); 
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistrationDataTable("OWNER_CONVICTION", parms);
        }
        else
        {
            // Insert
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.InsertRegistrationDataTable("OWNER_CONVICTION", parms);
        }
    }
    protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
    {
        
    }
    protected void ucAddress_StateChangedEvent(object sender, EventArgs e)
    {

     
    }
}