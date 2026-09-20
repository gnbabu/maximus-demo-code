using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerOtherInfo : BasePopupControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;


    public void LoadOwnerInfo()
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        DataRow[] drOtherProviders = null;

        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {

            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR  REG_OWNER_TYPE_ID = '" + CON.OwnerType.ManagingEmployee + "'");
           
        }
        else
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "OWNER");

            if (Helper.HasRows(ds))
            {
                this.WorkflowPage.RegistrationOwnersList = ds.Tables[0];
                dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR  REG_OWNER_TYPE_ID = '" + CON.OwnerType.ManagingEmployee + "'");
            }
        }
        if (this.WorkflowPage.AdditionalDisclosureList.Rows.Count > 0)
        {
            drOtherProviders = this.WorkflowPage.AdditionalDisclosureList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.OTHERPROVIDER + "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();
        ddlOwner1.Items.Clear();
        Helper.LoadList(ddlOwner1, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner2.Items.Clear();
        Helper.LoadList(ddlOwner2, (drOtherProviders!=null&& drOtherProviders.Length>0)? drOtherProviders.CopyToDataTable():null, "NAME", "REG_OWNER_ID", true);
    }

    public override void LoadData(DataRow dr)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (ddlRelationship.Items.Count == 0)
        {
            ds = psc.SelectRelationshipTypes();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["RELATIONSHIP_TYPE_ID"]) != 24)
                {
                    row.Delete();
                }
            }
            ds.Tables[0].AcceptChanges();

            Helper.LoadList(ddlRelationship, ds.Tables[0], "RELATIONSHIP_NAME", "RELATIONSHIP_TYPE_ID", true);
        }

        LoadOwnerInfo();


        if (dr != null)
        {
            hdnRegOtherOwnerID.Value = Helper.GetString("REG_OWNER_OTHER_ID", dr);
            ddlRelationship.SelectedValue = Helper.GetString("RELATIONSHIP_TYPE_ID", dr);
            ddlOwner1.SelectedValue = Helper.GetString("REG_OWNER_ID", dr);
           // loadddlOwner2();
            ddlOwner2.SelectedValue = Helper.GetString("OWNER_OTHER_ID", dr);
        }

        Helper.SetReadOnly(this, !Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName));
        
    }

    public void SaveData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        
        
        if (ddlOwner1.SelectedIndex > 0 && ddlOwner2.SelectedIndex > 0 && ddlRelationship.SelectedIndex > 0)
        {

            parms.Add("REG_OWNER_ID", ddlOwner1.SelectedValue);
            parms.Add("NAME", ddlOwner1.SelectedItem.Text);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (!string.IsNullOrEmpty(hdnRegOtherOwnerID.Value))
            {
                // Update
                parms.Add("@REG_OWNER_OTHER_ID", hdnRegOtherOwnerID.Value);
                psc.UpdateRegistrationDataTable("OWNER_OTHER", parms);
            }
            else
            {
                // Insert

                parms.Add("OWNER_OTHER_ID", ddlOwner2.SelectedValue);
                parms.Add("RELATIONSHIP_TYPE_ID", ddlRelationship.SelectedValue);
                psc.InsertRegistrationDataTable("OWNER_OTHER", parms);
            }
            
            

        }

    }

    protected void ddlOwner1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();

        //ddlOwner2.Items.Clear();
        //Helper.LoadList(ddlOwner2, dt, "NAME", "REG_OWNER_ID", true);
        //ddlOwner2.SelectedIndex = -1;
        //ddlOwner2.Items.RemoveAt(ddlOwner2.SelectedIndex);
        upSSN.Update();
    }
    private void loadddlOwner2()
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();

        Helper.LoadList(ddlOwner2, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner2.Items.RemoveAt(ddlOwner2.SelectedIndex);
    }

    protected void ddlOwner2_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableControls();
    }


    private void EnableControls()
    {
    }
}