using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_AddProviderFeed : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        txtDate.Text = DateTime.Now.ToString("yyyy/MM/dd");

    }
    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        // mpeConfirmAdd.Hide();
    }

    protected void btnSaveAdd_Click(object sender, EventArgs e)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        parms.Add("Reg_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("Notes_Date", DateTime.Now.ToString()); //Note_Ref_ID
        parms.Add("Initiated_By", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Person_Reviewed_By", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Enrollment_Type", txtEnrollmentType.Text.ToString());
        parms.Add("Final_Disposition", txtFinalDisposition.Text.ToString());
        parms.Add("NOTES", txtNewNote.Text.ToString());
        parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("Last_Modified_Date_time", DateTime.Now.ToString());
        parms.Add("CreatedBy", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("CreatedOn", DateTime.Now.ToString());
        try
        {
            psc.InsertRegistrationDataTable("provider_feed", parms);
        }
        catch (Exception ex)
        {

        }
        parms.Clear();
    }
}