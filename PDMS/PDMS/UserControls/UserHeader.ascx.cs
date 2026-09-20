using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_UserHeader : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private void SetDisplay()
    {
        btnEmail.Visible = 
        lnkEmail.Visible = 
        lnkNotes.Visible = lnkEventHistory.Visible = false;
        btnEventHistory.Visible = btnNotes.Visible = false;

        bool settingValueBool = false;
        var settingValue = AppSettings.Get("eRA-ShowToProvider");
        var result = bool.TryParse(settingValue, out settingValueBool);

        if (Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist)|| Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist))
        {
            lnkEmail.Visible = lnkNotes.Visible = true;
            btnEmail.Visible = btnNotes.Visible = true;
            lnkEmail.Enabled = lnkNotes.Enabled = false;
            btnEmail.Enabled = btnNotes.Enabled = false;
            if (this.WorkflowPage.RegistrationId > 0)
            {
                lnkEmail.Enabled = lnkNotes.Enabled = true;
                btnEmail.Enabled = btnNotes.Enabled = true;
            }

        }

        //Consolidated Issues - DR94: Internal Processing "Hide Event History, Notes and Email internally unless a provider file is actively being viewed."
        if (!HttpContext.Current.Request.Url.AbsoluteUri.Contains("Registration.aspx") && !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            btnEmail.Visible = lnkEmail.Visible =
            btnNotes.Visible = lnkNotes.Visible =
            btnEventHistory.Visible = lnkEventHistory.Visible = false;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) SetDisplay();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Only show the header if the user is logged in
        pnlHeader.Visible = (HttpContext.Current.User.Identity.IsAuthenticated);
        if (pnlHeader.Visible)
        {
            lblUser.Text = "User: " + HttpContext.Current.User.Identity.Name;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.GetUserAccountInformation(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (Helper.HasRows(ds)) lblUser.Text = "User: " + Helper.GetString("Name", ds.Tables[0].Rows[0]) + " (" + 
                HttpContext.Current.User.Identity.Name + ")"; 
            lblDate.Text = String.Format("{0:D}", DateTime.Now);
        }
        ucNotes.RefreshEvent += new UserControls_Notes.RefreshEventHandler(ucNotes_RefreshEvent);

        //bug 2191
        lnkEmail.Visible = btnEmail.Visible = !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        lnkHelp.Visible = btnHelp.Visible = false;
    }

    void ucNotes_AddNoteEvent()
    {
        mpeNotesGrid.Show();
    }

    void ucNotes_RefreshEvent()
    {
        mpeNotesGrid.Show();
    }

    protected void lnkHome_Click(object sender, EventArgs e)
    {
        // TODO: EDV Why ahve this logic here? Cant we just rediect to default
        if (Helper.IsLoggedInUserInAdminRole()) Response.Redirect("~/Process/AdminHome.aspx");
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderAdministrator)) Response.Redirect("~/Process/ProviderHomeNew.aspx");
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.SiteVisitOperator)) Response.Redirect("~/Process/MySiteVisitQueue.aspx");
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DCSAdministrator)) Response.Redirect("~/Process/DCSHome.aspx");
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ProviderOper)) Response.Redirect("~/Process/ProviderOperatorHome.aspx");
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.DIDDOperator)) Response.Redirect("~/Process/WaiverProviderSearch.aspx");
        else if (Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingChair) || Roles.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist))
        {
            Response.Redirect("~/Process/CredentialQueue.aspx");
        }
        else Response.Redirect("~/Process/MyQueue.aspx");
    }
    
    protected void lnkNotes_Click(object sender, EventArgs e)
    {
        ucNotes.CurrentPageIndex = 0;
        if (this.WorkflowPage.RegistrationId > 0) ucNotes.LoadNotes(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
        ucNotes.LoadData();
        mpeNotesGrid.Show();
    }

    protected void lnkEventHistory_Click(object sender, EventArgs e)
    {
        //The parameters will be provider specific later.
        ucMessageModal.Show(2, "Event History", "Here is some Event history that will be displayed", UserControls_MessageModalControls.ShowMe.EventHistory);
    }

    protected void lnkEmail_Click(object sender, EventArgs e)
    {
        ucEmails.Setup();
        mpeEmailsGrid.Show();
    }

    protected void lnkHelp_Click(object sender, EventArgs e)
    {
        ucMessageModal.Show(2, "Help", "Do you need help? You have come to the right place.");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        if (HttpContext.Current.User.Identity.IsAuthenticated)
        {
            if (SessionVarRetriever.IsOhID)
            {
                // Remove the token from the DB on logout
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.SaveUserIOPToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty, Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty, false, string.Empty, string.Empty, string.Empty);
                Response.Redirect(Helper.RedirectLoginURL(), false);
                return;
            }
            else
            {
                PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
                svc.SaveUserPNMToken(Helper.GetUserId(HttpContext.Current.User.Identity.Name), string.Empty); // clear haven token
                System.Web.Security.FormsAuthentication.SignOut();
                HttpContext.Current.Session.Abandon();                  // Remove the session entries
                Response.Redirect(Helper.RedirectLoginURL(), false);
            }

        }
    }

    protected void btnHome_Click(object sender, ImageClickEventArgs e)
    {
        lnkHome_Click(sender, new EventArgs());
    }

    protected void btnNotes_Click(object sender, ImageClickEventArgs e)
    {
        lnkNotes_Click(sender, new EventArgs());
    }

    protected void btnEventHistory_Click(object sender, ImageClickEventArgs e)
    {
        lnkEventHistory_Click(sender, new EventArgs());
    }

    protected void btnEmail_Click(object sender, ImageClickEventArgs e)
    {
        lnkEmail_Click(sender, new EventArgs());
    }

    protected void btnHelp_Click(object sender, ImageClickEventArgs e)
    {
        lnkHelp_Click(sender, new EventArgs());
    }

    protected void btnLogout_Click(object sender, ImageClickEventArgs e)
    {
        lnkLogout_Click(sender, new EventArgs());
    }

    protected void btnDocsAndRpts_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Account/DocumentsAndReports.aspx");
    }

    protected void lnkDocsAndRpts_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Account/DocumentsAndReports.aspx");
    }
}