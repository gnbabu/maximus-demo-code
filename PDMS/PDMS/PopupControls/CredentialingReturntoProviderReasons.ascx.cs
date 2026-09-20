using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using Microsoft.Extensions.Primitives;
using Microsoft.Web.Services3.Addressing;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CredentialingReturntoProviderReasons : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.BindReturnReasons();
        }
    }

    public void LoadControls()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
        DataSet ds = psc.SelectREG_ERRORcustom(this.WorkflowPage.RegistrationId, pageTypeId, this.WorkflowPage.RegistrationStep,
            false, true);
        if (ds.Tables.Count > 0)
        {
            //OHPNM-4556
            if(Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.IncidentComplianceReview
                && lsvReturnReasons.Items.Count <= 0)
            {
                    this.BindReturnReasons();
            }
            // Turn them all off
            foreach (ListViewItem itm in lsvReturnReasons.Items)
            {
                CheckBox chk = (CheckBox)itm.FindControl("chkReason");
                chk.Checked = false;
            }
            // Set them if a match
            foreach (ListViewItem itm in lsvReturnReasons.Items)
            {
                SetReturnReason(itm, ds.Tables[0]);
                if (ds.Tables.Count > 1) SetReturnReason(itm, ds.Tables[1]);
            }
        }
    }
    private void SetReturnReason(ListViewItem itm, DataTable dt)
    {
        CheckBox chk = (CheckBox)itm.FindControl("chkReason");
        HiddenField hdn = (HiddenField)itm.FindControl("hdnErrorTypeID");
        if (chk != null && hdn != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (Helper.GetString("ERROR_TYPE_ID", row) == hdn.Value)
                {
                    chk.Checked = true;
                    break;
                }
            }
        }
    }

    private void BindReturnReasons()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        // Page is 0 per Teresa return errors show on all pages based upon User's Role.
        DataSet ds = psc.SelectRegErrorTypes(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
        if (Helper.HasRows(ds))
        {
            lsvReturnReasons.DataSource = ds.Tables[0];
            lsvReturnReasons.DataBind();
        }

    }

    public void SaveData()
    {
        bool fnd = false;
        foreach (ListViewItem itm in lsvReturnReasons.Items)
        {
            CheckBox chk = (CheckBox)itm.FindControl("chkReason");
            HiddenField hdn = (HiddenField)itm.FindControl("hdnErrorTypeID");
            if (chk != null && hdn != null)
            {
                if (chk.Checked)
                {
                    fnd = true;
                    break;
                }
            }
        }
        string providerReason = string.Empty;
        var sb = new StringBuilder();
        int reasonCnt = 0;
        if (!fnd)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please select at least one Return to Provider reason.";
            val.ValidationGroup = "valReturnToProviderReasons";
            this.Page.Validators.Add(val);
        }
        else
        {
            this.Page.Validate("valReturnToProviderReasons");
            if (Page.IsValid)
            {
                //Save data
                int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
                Registration.MarkErrorsAsClosed(this.WorkflowPage.RegistrationId, pageTypeId, this.WorkflowPage.RegistrationStep);
                // Return to Provider, save the reasons
                foreach (ListViewItem itm in lsvReturnReasons.Items)
                {
                    CheckBox chk = (CheckBox)itm.FindControl("chkReason");
                    HiddenField hdn = (HiddenField)itm.FindControl("hdnErrorTypeID");
                    HiddenField hdnText = (HiddenField)itm.FindControl("hdnErrorType");
                    if (chk != null && hdn != null)
                    {
                        if (chk.Checked)
                        {
                            reasonCnt++;

                            if (!string.IsNullOrWhiteSpace(hdnText.ToString()))
                            {
                                sb.Append(" Reason: ")
                                  .Append(reasonCnt)
                                  .Append(" ")
                                  .Append(hdnText.Value)
                                  .Append(" ");
                            }


                            InsertError(hdn.Value);
                        }
                    }
                }
                providerReason = sb.ToString();
            }
            else
            {
                return;
            }
        }

        string comments = string.Empty;
        //OHPNM - 15588 - Notes of an RTP should be show the correct Type.
        //if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialing || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderCredentialingODM ||
        //  this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialingSupervisorReview || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingSupervisorReview ||
        //  this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialCommitteeReview || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ODMCredentialingReview)
        //{}

        if (!string.IsNullOrEmpty(txtProviderNotes.Text))
        {
            comments = comments + " Notes to provider: " + txtProviderNotes.Text.Trim();
        }

        if (!string.IsNullOrEmpty(txtInternalNotes.Text))
        {
            comments = comments + " Internal Comments:" + txtInternalNotes.Text.Trim();
        }

        StringBuilder notes = new StringBuilder();
        notes.Append("RTP - ");
        if(!string.IsNullOrEmpty(providerReason))
            notes.Append(providerReason+" - ");
        if (!string.IsNullOrEmpty(comments))
            notes.Append(comments);

        ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, notes.ToString(), null, null, null, this.WorkflowPage.WF_ProcessID);
    }
    private void InsertError(string errorTypeId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("ERROR_TYPE_ID", errorTypeId);
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("REG_SECTION_TYPE_ID", this.WorkflowPage.RegistrationStep.ToString());
        int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        parms.Add("REG_PAGE_TYPE_ID", RegPageTypeId.ToString());
        parms.Add("ERROR_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        int regErrorId = psc.InsertRegistrationDataTable("ERROR", parms);

        // Add the status of "Open"
        psc.UpdateErrorRegistration(regErrorId, CON.ErrorStatusType.Open, null, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
    }

}