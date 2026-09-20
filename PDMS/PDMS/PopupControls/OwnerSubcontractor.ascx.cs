using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerSubcontractor : BasePopupControl
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
    public int RegSubContractorTypeId
    {
        get { return (int)ViewState["RegSubContractorTypeId"]; }
        set { ViewState["RegSubContractorTypeId"] = value; }
    }
    public override void LoadData(DataRow dr)
    {

        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //ddlOwnerType.Items.Clear();
        //_dsOwnerTypes = psc.SelectRegistrationData(-1, "OWNER_TYPE");
        if (this.WorkflowPage.AdditionalDisclosureList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.AdditionalDisclosureList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.SubcontractorIndividual + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.SubcontractorOrganization +  "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();
		ddlSubcontractor.Items.Clear();
		Helper.LoadList(ddlSubcontractor, dt, "NAME", "REG_OWNER_ID", true);     
        if (dr != null)
        {
            hdnRegSubcontractorID.Value = Helper.GetString("REG_SUBCONTRACTOR_ID", dr);
            ddlSubcontractor.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
        }
        else
        {
            hdnRegSubcontractorID.Value = string.Empty;
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataRow[] dr1 = this.WorkflowPage.AdditionalDisclosureList.Select("REG_OWNER_ID ='" + ddlSubcontractor.SelectedValue  + "'");
        DataTable dt = dr1.CopyToDataTable();
        parms.Add("SUBCONTRACTOR_TYPE_ID", Helper.GetInt("REG_OWNER_TYPE_ID", dt.Rows[0]).ToString());        
        parms.Add("NAME", ddlSubcontractor.SelectedItem.Text);
        parms.Add("MODIFIED_STATUS_TYPE_ID",
            MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegSubcontractorID.Value))
        {
            // Update
            parms.Add("REG_SUBCONTRACTOR_ID", hdnRegSubcontractorID.Value);
            psc.UpdateRegistrationDataTable("SUBCONTRACTOR", parms);
        }
        else
        {
            // Insert
            
            psc.InsertRegistrationDataTable("SUBCONTRACTOR", parms);
        }
    }

    protected void ddlSubcontractor_SelectedIndexChanged(object sender, EventArgs e)
    {
       
    }
    protected void ucAddress_StateChangedEvent(object sender, EventArgs e)
    {
       
    }
}