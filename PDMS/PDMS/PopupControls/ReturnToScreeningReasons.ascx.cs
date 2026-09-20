using Corp.Core.Libraries.Helper;
using System;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ReturnToScreeningReasons : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
    }
    public void LoadControls()
    {
        txtComments.Text = "";
    }
    public void SaveData(string actionText)
    {
        this.Page.Validate("valReturnToScreeningReasons");
        if (Page.IsValid)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int providerNoteTypeId = 0;
            if (actionText == CON.RegistrationWorkflowActionName.ReturnToScreening)
                providerNoteTypeId = CON.ProviderNoteTypeId.ReturnToScreening;
            else if (actionText == CON.RegistrationWorkflowActionName.ReturnToSiteVisit)
                providerNoteTypeId = CON.ProviderNoteTypeId.ReturnToSiteVisit;
            else if (actionText == CON.RegistrationWorkflowActionName.ReturnToProviderReview)
                providerNoteTypeId = CON.ProviderNoteTypeId.ReturnToProviderReview;
            if (!string.IsNullOrEmpty(txtComments.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                //if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                //        Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                //        Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                //    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId,0, providerNoteTypeId, txtComments.Text.Trim(),
                //                DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                //else
                //    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, providerNoteTypeId, txtComments.Text.Trim(),
                //                DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                string noteText = actionText + " - " + txtComments.Text;

                ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, noteText,
                            personReviewedBy: HttpContext.Current.User.Identity.Name, processID: this.WorkflowPage.WF_ProcessID);

            }
        }
    }


}