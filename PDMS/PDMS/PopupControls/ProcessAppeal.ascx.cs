using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_ProcessAppeal : BasePopupControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        
    }
    public void PageLoad()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.GetAppealStatus();
        DataSet ds1 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
        /*if (!IsPostBack)
        {*/
            Helper.LoadList(ddlAppealStatus, ds.Tables["AppealStatus"], "Appeal_Status_Name", "Appeal_Status_ID", true);

            // TODO: EDV put the note types in cache
            ds = psc.GetTermReason();
            Helper.LoadList(ddlreason, ds.Tables["TermReason"], "TERM_REASON_NAME", "TERM_REASON_ID", true);
        /*}*/
        DataTable dtProvider = new DataTable();
        if (Helper.HasRows(ds1))
        {
            if (!string.IsNullOrEmpty(Helper.GetString("TERM_DATE", ds1.Tables[0].Rows[0])))
            {
                txtProviderEndDate.Text = Helper.GetString("TERM_DATE", ds1.Tables[0].Rows[0]);
            }
        }
        /*if (!IsPostBack)
        {*/

            //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            //Dictionary<string, string> parms = new Dictionary<string, string>();
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");

            txtProviderEndDate.Enabled = false;
            if (Helper.HasRows(ds))
            {
                dtProvider = ds.Tables[0];
                int partyID = Helper.GetInt("PARTY_ID", dtProvider.Rows[0]);
                if (string.IsNullOrEmpty(Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0])))
                {
                    ddlAppealStatus.SelectedValue = CON.AppealStatus.NoticeSent.ToString();
                    ddlAppealStatus.Enabled = false;
                    txtAppealStartDate.Text = DateTime.Now.Date.ToShortDateString();
                    // for new registration
                    if (partyID == null || partyID < 0 || partyID == 0)
                    {
                        txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(90).ToShortDateString();
                        txtAppealEndDate.Enabled = false;
                    }//for revalidation
                    else if (partyID > 0)
                    {
                        txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(30).ToShortDateString();
                        txtAppealEndDate.Enabled = false;
                    }
                }
                if (!string.IsNullOrEmpty(Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0])))
                {
                    if (Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0]) == CON.AppealStatus.NoticeSent.ToString())
                    {
                        ddlAppealStatus.Items.Remove(CON.AppealStatus.NoticeSent.ToString());
                    }
                    else
                    {
                        ddlAppealStatus.SelectedValue = Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0]);
                    }
                }

                if (!string.IsNullOrEmpty(Helper.GetString("TERM_REASON_ID", ds1.Tables[0].Rows[0])))
                {
                    ddlreason.SelectedValue = Helper.GetString("TERM_REASON_ID", ds1.Tables[0].Rows[0]);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("TERM_DATE", ds1.Tables[0].Rows[0])))
                {
                    txtProviderEndDate.Text = Helper.GetString("TERM_DATE", ds1.Tables[0].Rows[0]);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("APPEAL_START_DATE", dtProvider.Rows[0])))
                {
                    txtAppealStartDate.Text = Helper.GetString("APPEAL_START_DATE", dtProvider.Rows[0]);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("APPEAL_END_DATE", dtProvider.Rows[0])))
                {
                    txtAppealEndDate.Text = Helper.GetString("APPEAL_END_DATE", dtProvider.Rows[0]);
                }
                if (!string.IsNullOrEmpty(Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0])))
                {
                    rblImmediate.SelectedValue = Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0]);
                }
                /*if (!string.IsNullOrEmpty(Helper.GetString("COMMENTS", dtProvider.Rows[0])))
                {
                    txtComments.Text = Helper.GetString("COMMENTS", dtProvider.Rows[0]);
                }*/
            }
        
            if (!Helper.HasRows(ds))
            {
                ddlAppealStatus.SelectedValue = CON.AppealStatus.NoticeSent.ToString();
                ddlAppealStatus.Enabled = false;
                txtAppealStartDate.Text = DateTime.Now.Date.ToShortDateString();
                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationNew)
                {
                    txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(90).ToShortDateString();
                    txtAppealEndDate.Enabled = false;
                }
                else if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationRevalidation)
                {
                    txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(30).ToShortDateString();
                    txtAppealEndDate.Enabled = false;
                }

                txtProviderEndDate.Enabled = false;
            }
        /*}*/
    }
    public void SaveData()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
        string dtProviderEndDate = "";
        int regAppealID = 0;

        if (Helper.HasRows(ds))
        {
            DataTable dtProvider = ds.Tables[0];
            regAppealID = Helper.GetInt("REG_APPEAL_ID", dtProvider.Rows[0]);
            if (!string.IsNullOrEmpty(Helper.GetString("TERM_DATE", dtProvider.Rows[0])))
            {
                dtProviderEndDate = Convert.ToDateTime(Helper.GetString("TERM_DATE", dtProvider.Rows[0])).Date.ToShortDateString();
            }
        }

                //ExistingComments = Helper.GetString("COMMENTS", dtProvider.Rows[0]) + " ";
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("APPEAL_START_DATE", txtAppealStartDate.Text);
        parms.Add("APPEAL_END_DATE", txtAppealEndDate.Text);
        parms.Add("APPEAL_STATUS_ID", ddlAppealStatus.SelectedValue);
        
        parms.Add("IMMEDIATE_DENY_OR_TERMINATE", rblImmediate.SelectedValue);
        parms.Add("STATE_REVIEW_SCREENING_STATUS_ID", CON.StateReviewScreeningStatus.Confirm.ToString());
        parms.Add("COMMENTS", txtComments.Text.Trim());
        
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isProviderEndDateChanged = false;
        if (!string.IsNullOrEmpty(txtProviderEndDate.Text) && !string.IsNullOrEmpty(dtProviderEndDate))
        {
            if (Convert.ToDateTime(txtProviderEndDate.Text).ToShortDateString() != dtProviderEndDate)
            {
                isProviderEndDateChanged = true;
            }
        }
        if (dtProviderEndDate == "" && !string.IsNullOrEmpty(txtProviderEndDate.Text))
        {
            isProviderEndDateChanged = true;
        }
        if (rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString() || isProviderEndDateChanged)
        {
            parms.Add("SEND_TO_MMIS", "Y");
        }
        /*else
        {
            parms.Add("SEND_TO_MMIS", "N");
        }*/
        if (regAppealID == 0)
        {
            psc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL", parms);
        }
        else
        {
            parms.Add("REG_APPEAL_ID", regAppealID.ToString());
            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL", parms);
        }

        if (!string.IsNullOrEmpty(txtProviderEndDate.Text) || !string.IsNullOrEmpty(ddlreason.SelectedValue))
        {
            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

            if (!string.IsNullOrEmpty(txtProviderEndDate.Text))
            {
                parms.Add("TERM_DATE", Convert.ToDateTime(txtProviderEndDate.Text).ToShortDateString());
                //updating end date when it is a new registration and when it is denied - for bug 7520
                if (rblImmediate.Visible == false && (ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString() || ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealLost_ProcessDenial_Termination.ToString()))
                {
                    parms.Add("END_DATE", Convert.ToDateTime(txtProviderEndDate.Text).ToShortDateString());
                }
            }
            if(!string.IsNullOrEmpty(ddlreason.SelectedValue))
            parms.Add("TERM_REASON_ID", ddlreason.SelectedValue);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistrationDataWithParams("updateREG_PROVIDERCustom", parms);
        }

        if (!String.IsNullOrEmpty(txtComments.Text.Trim()))
        {
            int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
            if (RegPageTypeId == 0)
                RegPageTypeId = this.WorkflowPage.RegistrationStep;
            if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
            {
                if (rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString() || ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealLost_ProcessDenial_Termination.ToString() ||
                ddlAppealStatus.SelectedValue == CON.AppealStatus.NoAppeal.ToString())
                {
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId,0, CON.ProviderNoteTypeId.RegistrationRejection, txtComments.Text.Trim(),
                                    DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                else if (ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealWon_ReinstateProvider.ToString())
                {
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtComments.Text.Trim(),
                                    DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                else if (ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString() || ddlAppealStatus.SelectedValue == CON.AppealStatus.ProviderRequestedAppeal.ToString())
                {
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0,  CON.ProviderNoteTypeId.Internal, txtComments.Text.Trim(),
                                    DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
            }
        }
        if (ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString() && rblImmediate.SelectedValue != CON.ImmediateDenyOrTerminate.Immediate.ToString())
        {
            Response.Redirect("~/Process/MyQueue.aspx");
        }
        else
        {
            Refresh(true);
        }
    }

    public void Refresh(bool forceRedirect = false)
    {
        // TODO: EDV Test the functionality. There shouldnt be a need to redirect
        //if (forceRedirect)
        //{
        //    string url = "~/Process/Registration.aspx?Step=" + this.WorkflowPage.RegistrationStep.ToString();
        //    Response.Redirect(url);
        //}
        //else if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }

    protected void txtAppealStartDate_TextChanged(object sender, EventArgs e)
    {
        txtAppealEndDate.Enabled = true;
        txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(30).ToShortDateString();
        txtAppealEndDate.Enabled = false;
    }
    protected void rblImmediate_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString())
        {
            txtProviderEndDate.Enabled = true;
        }
        else
        {
            txtProviderEndDate.Enabled = false;
        }
        rblImmediate.Focus();
    }
    protected void ddlAppealStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAppealStatus.SelectedValue == CON.AppealStatus.NoAppeal.ToString() || ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealLost_ProcessDenial_Termination.ToString())
        {
            txtProviderEndDate.Enabled = true;
        }
        else
        {
            txtProviderEndDate.Enabled = false;
        }
        ddlAppealStatus.Focus();
    }
    public void LoadControls()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");

        DataSet dsAppeal = psc.GetAppealStatus();

        Helper.LoadList(ddlAppealStatus, dsAppeal.Tables["AppealStatus"], "Appeal_Status_Name", "Appeal_Status_ID", true);

        // TODO: EDV put the note types in cache
        dsAppeal = psc.GetTermReason();
        Helper.LoadList(ddlreason, dsAppeal.Tables["TermReason"], "TERM_REASON_NAME", "TERM_REASON_ID", true);
        txtProviderEndDate.Enabled = false;
        if (!Helper.HasRows(ds))
        {
            //no rows, default status to notice sent and disable.
            ddlAppealStatus.SelectedValue = CON.AppealStatus.NoticeSent.ToString();
            ddlAppealStatus.Enabled = false;
            return;
        }

        if (Helper.HasRows(ds))
        {
            DataTable dtProvider = ds.Tables[0];
            int partyID = Helper.GetInt("PARTY_ID", dtProvider.Rows[0]);
            if (partyID > 0)
            {
                rblImmediate.Visible = true;
                lblImmediate.Visible = true;
            }
            else
            {
                rblImmediate.Visible = false;
                lblImmediate.Visible = false;
            }
            string appealStatusID = Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0]);
            if (string.IsNullOrEmpty(appealStatusID))
            {
                ddlAppealStatus.SelectedValue = CON.AppealStatus.NoticeSent.ToString();
                ddlAppealStatus.Enabled = false;
                txtAppealStartDate.Text = DateTime.Now.Date.ToShortDateString();
                //for new registration
                if (partyID == null || partyID < 0 || partyID == 0)
                {
                    txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(90).ToShortDateString();
                    txtAppealEndDate.Enabled = false;
                }//for revalidation
                else if (partyID > 0)
                {
                    txtAppealEndDate.Text = Convert.ToDateTime(txtAppealStartDate.Text).AddDays(30).ToShortDateString();
                    txtAppealEndDate.Enabled = false;
                }
            }
            if (!string.IsNullOrEmpty(appealStatusID))
            {
                if (appealStatusID != "" && appealStatusID != null //?
                    && ddlAppealStatus.Items.Count == 6)
                {
                    ddlAppealStatus.Items.RemoveAt(CON.AppealStatus.NoticeSent);
                    rblImmediate.Enabled = false;
                }

                if (Helper.ValueExistsInDropDown(ddlAppealStatus, appealStatusID))
                {
                    ddlAppealStatus.SelectedValue = appealStatusID;
                }
            }

            if (!string.IsNullOrEmpty(Helper.GetString("TERM_REASON_ID", dtProvider.Rows[0])))
            {
                ddlreason.SelectedValue = Helper.GetString("TERM_REASON_ID", dtProvider.Rows[0]);
            }
            DateTime tmp;
            if (DateTime.TryParse(Helper.GetString("TERM_DATE", dtProvider.Rows[0]), out tmp))
            {
                txtProviderEndDate.Text =  tmp.ToString("MM/dd/yyyy");
            }
            if (DateTime.TryParse(Helper.GetString("APPEAL_START_DATE", dtProvider.Rows[0]), out tmp))
            {
                txtAppealStartDate.Text = tmp.ToString("MM/dd/yyyy");
            }
            if (DateTime.TryParse(Helper.GetString("APPEAL_END_DATE", dtProvider.Rows[0]), out tmp))
            {
                txtAppealEndDate.Text = tmp.ToString("MM/dd/yyyy");
            }
            if (!string.IsNullOrEmpty(Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0])))
            {
                rblImmediate.SelectedValue = Helper.GetString("IMMEDIATE_DENY_OR_TERMINATE", dtProvider.Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("COMMENTS", dtProvider.Rows[0])))
            {
                txtComments.Text = Helper.GetString("COMMENTS", dtProvider.Rows[0]);
            }
        }
    }
    /*protected void btnCancel_Click(object sender, EventArgs e)
    {
       
        if (CancelEvent != null)
            CancelEvent();
    }*/
    public bool ValidateData()
    {
        bool isGood = true;
        if (rblImmediate.Visible == true && rblImmediate.SelectedIndex < 0)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Select Immediate Yes or No.";
            val.ValidationGroup = "valProcessAppealInfo";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        if (txtProviderEndDate.Enabled == true && rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString() && ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString()
            && String.IsNullOrEmpty(txtProviderEndDate.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Select a Provider End Date.";
            val.ValidationGroup = "valProcessAppealInfo";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        //required field check for provider end date when it is a new registration and when it is denied - for bug 7520
        if (txtProviderEndDate.Enabled == true && rblImmediate.Visible == false && (ddlAppealStatus.SelectedValue == CON.AppealStatus.NoAppeal.ToString() || ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealLost_ProcessDenial_Termination.ToString())
        && String.IsNullOrEmpty(txtProviderEndDate.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Select a Provider End Date.";
            val.ValidationGroup = "valProcessAppealInfo";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        if (ddlAppealStatus.SelectedValue == "")
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "*Select a Appeal Status.";
            val.ValidationGroup = "valProcessAppealInfo";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        if ((rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString() && ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString()) ||
            ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealWon_ReinstateProvider.ToString())
        {
            if (!CheckUploadedDocuments())
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                if (rblImmediate.SelectedValue == CON.ImmediateDenyOrTerminate.Immediate.ToString() && ddlAppealStatus.SelectedValue == CON.AppealStatus.NoticeSent.ToString())
                {
                    val.ErrorMessage = "*Please upload the Denial/Termination Letter.";
                }
                else if (ddlAppealStatus.SelectedValue == CON.AppealStatus.AppealWon_ReinstateProvider.ToString())
                {
                    val.ErrorMessage = "*Please upload the approval Letter.";
                }
                val.ValidationGroup = "valProcessAppealInfo";
                this.Page.Validators.Add(val);
                isGood = false;
            }
        }
        return isGood;
    }

    private bool CheckUploadedDocuments()
    {
        // Check if there are documents uploaded
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string sectionname = "";
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProcessAppeal)
        {
            sectionname = CON.RegistrationTaskName.ProcessAppeal;
        }
        else if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProcessAppealSiteVisit)
        {
            sectionname = CON.RegistrationTaskName.ProcessAppealSiteVisit;
        }


        DataSet dsDoc = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, sectionname,
            CON.RegistrationPageType.Certification.ToString(), null);
        bool isGood = true;
        if (!Helper.HasRows(dsDoc)) isGood = false;

        return isGood;
    }

}