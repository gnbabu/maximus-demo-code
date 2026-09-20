using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerResidency : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override void LoadData(DataRow dr)
    {
        hdnRegOwnerResidentFlag.Value = string.Empty;
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR REG_OWNER_TYPE_ID ='" + CON.OwnerType.ManagingEmployee + "'");
            
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();
        Helper.LoadList(ddlOwner, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner.SelectedIndex = 0;

        if (dr != null)
        {
            hdnRegOwnerResidentFlag.Value = Helper.GetString("RESIDENT_FLAG", dr);
            ddlOwner.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
        }
    }

    public void SaveData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(hdnRegOwnerResidentFlag.Value))
        {
            hdnRegOwnerResidentFlag.Value = "1";
            parms.Add("RESIDENT_FLAG", hdnRegOwnerResidentFlag.Value);
        }
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_OWNER_ID", ddlOwner.SelectedValue);
        parms.Add("NAME", ddlOwner.SelectedItem.Text);
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (!string.IsNullOrEmpty(hdnRegOwnerResidentFlag.Value))
        {
            // Update
            psc.UpdateRegistrationDataTable("OWNER_RESIDENCY", parms);
        }
        //else
        //{
        //    // Insert
        //    psc.InsertRegistrationDataTable("OWNER", parms);
        //}
    }
    protected void ddlOwner_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Ownerid = ddlOwner.SelectedValue;
        DataTable dt = this.WorkflowPage.RegistrationOwnersList;
        foreach (DataRow dr in dt.Rows)
        {
            if (Helper.GetString("REG_OWNER_ID", dr) == Ownerid)
            {
               
            }
        }
       
    }

}