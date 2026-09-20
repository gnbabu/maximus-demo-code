using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ApproveProvider : BasePopupControl
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
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
        txtEffectiveDate.Text = DateTime.Now.Date.ToShortDateString();
    }
    public void SaveData()
    {
        this.Page.Validate("valApproveProviderInfo");
        if (Page.IsValid)
        {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
        DataTable dtAppeal = new DataTable();
        if (Helper.HasRows(ds)) dtAppeal = ds.Tables[0];
        string ExistingComments = "";
        if (Helper.HasRows(ds))
        {
            if (!string.IsNullOrEmpty(Helper.GetString("REG_APPEAL_ID", dtAppeal.Rows[0])) && !string.IsNullOrEmpty(txtComments.Text))
            {
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("REG_APPEAL_ID", Helper.GetString("REG_APPEAL_ID", dtAppeal.Rows[0]));
                ExistingComments = Helper.GetString("COMMENTS", dtAppeal.Rows[0]) + " ";
                parms.Add("COMMENTS", ExistingComments + txtComments.Text);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL", parms);
            }
        }


        parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("CHANGE_EFFECTIVE_DATE", txtEffectiveDate.Text);
        psc.UpdateRegistration(parms);
        parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        //set revalidation due date
        if (rblClosedEndAgreement.SelectedIndex == 0)
        {
            parms.Add("END_DATE", Convert.ToDateTime(txtEffectiveDate.Text).AddYears(1).ToString());   
        }
        else if (rblClosedEndAgreement.SelectedIndex == 1)
        {
            int endYears = 5;

            //If High-Risk, the end date is 3 years away
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
            if (Helper.HasRows(ds))
            {
                DataTable dtProvider = ds.Tables[0];
                int riskLevelID = Helper.GetInt("PROVIDER_RISK_LEVEL_ID", dtAppeal.Rows[0]);
                if (riskLevelID == CON.ProviderRiskLevel.RiskHigh)
                    endYears = 3;
            }

            parms.Add("END_DATE", Convert.ToDateTime(txtEffectiveDate.Text).AddYears(endYears).ToString());
        }
        psc.UpdateRegistrationDataWithParams("updateREG_PROVIDERCustom", parms);
        if (!string.IsNullOrEmpty(txtComments.Text))
        {
            int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtComments.Text.Trim(),
                                DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        }
        }
    }


}