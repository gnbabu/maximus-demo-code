using System;
using System.Data;
using System.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_OwnerRelationships : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    public void LoadOwnerInfo()
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;

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
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();
        ddlOwner1.Items.Clear();
        Helper.LoadList(ddlOwner1, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner2.Items.Clear();
        Helper.LoadList(ddlOwner2, dt, "NAME", "REG_OWNER_ID", true);
    }

    public override void LoadData(DataRow dr)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds;
        if (ddlRelationship.Items.Count == 0)
        {
            ds = psc.SelectRelationshipTypes();
            Helper.LoadList(ddlRelationship, ds.Tables[0], "RELATIONSHIP_NAME", "RELATIONSHIP_TYPE_ID", true);
        }
        
        LoadOwnerInfo();


        if (dr != null)
        {
            ddlRelationship.SelectedValue = Helper.GetString("RELATIONSHIP_TYPE_ID", dr);
            ddlOwner1.SelectedValue = Helper.GetString("REG_OWNER1_ID", dr);
            loadddlOwner2();
            ddlOwner2.SelectedValue = Helper.GetString("REG_OWNER2_ID", dr);
        }

        Helper.SetReadOnly(this, !Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName));
        
    }

    public void SaveData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        if (ddlOwner1.SelectedIndex > 0 && ddlOwner2.SelectedIndex > 0 && ddlRelationship.SelectedIndex > 0)
        {
            int Id2 = 0;
            psc.SaveRegistrationOwnerXref(this.WorkflowPage.RegistrationId, Convert.ToInt32(ddlOwner1.SelectedValue),
                Convert.ToInt32(ddlOwner2.SelectedValue), Convert.ToInt32(ddlRelationship.SelectedValue),
                MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed,
                Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Id2);
        }

    }

    protected void ddlOwner1_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR  REG_OWNER_TYPE_ID = '" + CON.OwnerType.ManagingEmployee + "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();

        ddlOwner2.Items.Clear();
        Helper.LoadList(ddlOwner2, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner2.Items.Remove(ddlOwner1.SelectedItem);
        ddlOwner2.SelectedIndex = -1;
        
        upSSN.Update();
    }
    private void loadddlOwner2()
    {
        DataTable dt = new DataTable();
        DataRow[] dr1 = null;
        if (this.WorkflowPage.RegistrationOwnersList.Rows.Count > 0)
        {
            dr1 = this.WorkflowPage.RegistrationOwnersList.Select("REG_OWNER_TYPE_ID ='" + CON.OwnerType.Person + "' OR  REG_OWNER_TYPE_ID = '" + CON.OwnerType.ManagingEmployee + "'");
        }
        if (dr1 != null && dr1.Length > 0)
            dt = dr1.CopyToDataTable();

        ddlOwner2.Items.Clear();
        Helper.LoadList(ddlOwner2, dt, "NAME", "REG_OWNER_ID", true);
        ddlOwner2.Items.Remove(ddlOwner1.SelectedItem);
    }

    


    
}