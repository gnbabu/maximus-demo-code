using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_RegistrationNavigation : System.Web.UI.UserControl
{
    public delegate bool SaveDataEventHandler(int step);
    public event SaveDataEventHandler SaveDataEvent;

    public delegate void ValidateDataEventHandler(int step, ref bool isValid);
    public event ValidateDataEventHandler ValidateDataEvent;
    public delegate void ValidateSaveNextDataEventHandler(int step, ref bool isValid);
    public event ValidateSaveNextDataEventHandler ValidateSaveNextDataEvent;
    public delegate void ValidateIsRequiredDataEventHandler(int step, ref bool isValid);
    public event ValidateIsRequiredDataEventHandler ValidateIsRequiredDataEvent;
    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;

    public delegate void SaveDataPSEventHandler(int step, int action);
    public event SaveDataPSEventHandler SaveDataPSEvent;

    public delegate string ValidateDataPSEventHandler(int step);
    public event ValidateDataPSEventHandler ValidateDataPSEvent;

    public int EntityTypeId
    {
        get
        {
            if (ViewState["EntityTypeId"] == null) ViewState["EntityTypeId"] = 0;
            return Convert.ToInt32(ViewState["EntityTypeId"]);
        }
        set { ViewState["EntityTypeId"] = value; }
    }
    public int ProviderTypeId
    {
        get
        {
            if (ViewState["ProviderTypeId"] == null) ViewState["ProviderTypeId"] = 0;
            return Convert.ToInt32(ViewState["ProviderTypeId"]);
        }
        set { ViewState["ProviderTypeId"] = value; }
    }

    public int DIDDReferralId
    {
        get
        {
            if (ViewState["DIDDReferralId"] == null) ViewState["DIDDReferralId"] = 0;
            return Convert.ToInt32(ViewState["DIDDReferralId"]);
        }
        set { ViewState["DIDDReferralId"] = value; }
    }

    public int SpecialtyTypeID
    {
        get
        {
            if (ViewState["SpecialtyTypeID"] == null) ViewState["SpecialtyTypeID"] = 0;
            return Convert.ToInt32(ViewState["SpecialtyTypeID"]);
        }
        set { ViewState["SpecialtyTypeID"] = value; }
    }
    public bool IsTakeActionVisible
    {
        get
        {
            if (ViewState["IsTakeActionVisible"] == null)
                ViewState["IsTakeActionVisible"] = false;

            return (bool)ViewState["IsTakeActionVisible"];
        }
        set
        {
            ViewState["IsTakeActionVisible"] = value;
        }
    }


    public bool IsUploadAction
    {
        get
        {
            if (ViewState["IsUploadAction"] == null)
                ViewState["IsUploadAction"] = false;

            return (bool)ViewState["IsUploadAction"];
        }
        set
        {
            ViewState["IsUploadAction"] = value;
        }
    }

    public string RequiredText
    {
        get { return lblRequired.Text; }
        set
        {
            lblRequired.Visible = true;
            lblRequired.Text = value;

        }
    }
    public string Title
    {
        get { return lblTitle.Text; }
        set { lblTitle.Text = lblTitlePS.Text = lblScreeningReviewTitle.Text = value; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public string CurrentStep
    {
        get { return lblCurrentStepText.Text; }
        set { lblCurrentStepText.Text = value; }
    }

    private bool _ShowRegistrationId = false;
    public bool ShowRegistrationId
    {
        get { return _ShowRegistrationId; }
        set
        {
            _ShowRegistrationId = value;
            lblRegistrationId.Visible = lblRegistrationIdAdmin.Visible = lblRegistrationIdPS.Visible = lblRegID.Visible = false;
            if (_ShowRegistrationId && this.WorkflowPage.RegistrationId > 0)
            {
                lblRegistrationId.Visible = lblRegistrationIdAdmin.Visible = lblRegistrationIdPS.Visible = lblRegID.Visible = true;
                lblRegistrationId.Visible = lblRegistrationIdAdmin.Visible = lblRegistrationIdPS.Visible = lblRegID.Visible = true;
                lblRegistrationId.Text = lblRegistrationIdAdmin.Text = lblRegistrationIdPS.Text = lblRegID.Text = "(" +
                    this.WorkflowPage.RegistrationId.ToString() + ")";
                lblRegistrationId.ToolTip = "Registration Id";
            }
        }
    }

    private void SetButtons(int idx)
    {
        string stepTxt = Registration.GetStepText(this.WorkflowPage.RegistrationStep);
        pnlAppealProcessStatus.Visible = false;

        if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC && this.WorkflowPage.RegistrationStep == CON.SectionTypeID.CMCSpecialties)
        {
            btnSave.Visible = false;
            btnCancelChanges.Visible = false;
            btnPrevious.Visible = true;
            return;
        }

        if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ODMCredentialingQualityAssurance))
            || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingChair))
            || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.CredentialingQualityAssurance))
            || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.PnmSuperUser)))
        {
            btnSave.Visible = false;
            btnCancelChanges.Visible = true;
            btnPrevious.Visible = true;

        }

        if (Helper.IsModern())
        {
            btnSave.Visible = true;
            btnSaveNext.Visible = true;
        }
        if (idx == 1)
        {
            // Provider Services view
            mltNavigation.ActiveViewIndex = 1;
            btnPreviousPS.Visible = (this.WorkflowPage.RegistrationSequence != 1);
            btnNextPS.Visible = (this.WorkflowPage.RegistrationSequence != this.WorkflowPage.RegistrationNodes.Count);
            if ((this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderReview && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)) || (!Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)))
            {
                //pnlTakeAction.Visible = false;
                btnTakeActionApprove.Visible = false;
                btnTakeActionReject.Visible = false;
            }
            if (!Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_StepID))
            {
                //pnlTakeAction.Visible = false;
                btnTakeActionApprove.Visible = false;
                btnTakeActionReject.Visible = false;
            }
            if (this.WorkflowPage.RegistrationIdSelected > 0)             // Registration is Read Only
            {
                btnSavePS.Visible = btnTakeAction.Visible = false;
                if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName) && this.WorkflowPage.RegistrationStep != CON.RegistrationPageType.Agreements && this.WorkflowPage.RegistrationStep != CON.RegistrationPageType.GroupAffiliations)
                {
                    btnSavePS.Visible = true;
                }
                // return;
            }

            // Check the WF_TASK_PAGE_ACTION table
            DataRow row = Registration.GetTakeActionRow(Registration.GetStepText(this.WorkflowPage.RegistrationStep), this.WorkflowPage.WF_TaskID);
            if (row != null)
            {
                // If we have a row from the REG_PAGE_SETTING_ACTION file then its decision is final
                if (!Helper.GetBool("TAKE_ACTION_ALLOWED", row))
                {
                    btnSavePS.Visible = btnTakeAction.Visible = false;
                    return;
                }

                else
                    if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
                {

                    // For application fee hide the take action button if echeck is not cleared or pecos verification has not been selected
                    //pnlTakeAction.Visible = IsTakeActionVisible;
                    //btnTakeAction.Visible = IsTakeActionVisible;
                    btnTakeActionApprove.Visible = IsTakeActionVisible;
                    btnTakeActionReject.Visible = IsTakeActionVisible;

                }
                else
                {
                    btnTakeAction.Visible = true;          // Action is allowed, continue
                }
                //ToDo bug 2920-Operator and Admin able to approve and manage applications																	  
                if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)
                    && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_StepID))
                {
                    //pnlTakeAction.Visible = true;
                    btnTakeActionApprove.Visible = true;
                    btnTakeActionReject.Visible = true;
                    btnTakeAction.Visible = false;
                    mltTakeAction.SetActiveView(vwPSReview);
                }
            }
            else
                if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening ||
                        this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.AffiliationScreening ||
                        this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerScreening ||
                        this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.HouseholdMemberScreening ||
                        //this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.SiteVisitScreening ||
                        this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.BackgroundCheck ||
                        this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OrientationInformation ||
                        this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ProviderCredentialing
                    )
            {
                //OHPNM-18085 - Credentialing spl should not change risk level when not in Provider Credentialing step or not assigned to logged in user.
                String loggedInUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                if (string.IsNullOrEmpty(this.WorkflowPage.CurrentTaskName) || this.WorkflowPage.WF_TaskType == "System" || this.WorkflowPage.WF_TaskType == "Email"
                    || (this.WorkflowPage.WF_TaskType == "Queue" && (this.WorkflowPage.WF_StepOwner != loggedInUserId || this.WorkflowPage.WF_StepOwner == "")))
                {
                    // disable take action button
                    btnTakeAction.Visible = false;
                }
                else
                {
                    /// For screening pages, ignore Page settings, and use value set in viewstate previously by the screening controls.
                    /// Only applies to Screening or Site Visit steps.
                    btnTakeAction.Visible = IsTakeActionVisible;
                }
            }

            else
            {
                // Previous code for decision making Take Action visible
                bool mmisError = this.HasMMISError();
                //btnTakeAction.Visible = applicationStatus == "Submitted" || applicationStatus == "Application Fee Approved" || mmisError;
                if ((this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee) &&
                    Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name)) btnTakeAction.Visible = true;
            }
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview &&
                (Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CHOP)
                || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.APMSpecialist) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC))
                && this.WorkflowPage.RegistrationStep != CON.SectionTypeID.WorkflowSteps && this.WorkflowPage.RegistrationStep != CON.SectionTypeID.TransactionQueue  // JIRA 3276
                && Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_StepID))
            {
                //pnlTakeAction.Visible = true;
                btnTakeActionApprove.Visible = true;
                btnTakeActionReject.Visible = true;
                btnTakeAction.Visible = false;

                if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Appeals || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ContractMaintenance
                    || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.RestrictedService || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.WaiverServiceDisplay
                    || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.HearingRights)   // OHPNM-14835 - Do not show Approve /reject buttons on Application disposition page
                {
                    btnTakeActionApprove.Visible = false;
                    btnTakeActionReject.Visible = false;
                }

                //SAM818 CMC Contact Info page is read only if not in CMC wf
                if (this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.CMC && this.WorkflowPage.RegistrationStep == CON.SectionTypeID.CMCContactInformation)
                {
                    btnSavePS.Visible = false;
                    btnTakeActionApprove.Visible = false;
                    btnTakeActionReject.Visible = false;
                    btnTakeAction.Visible = false;
                }

            }



            if (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderReview && !(this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview && this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ApplicationFee))
            {
                //pnlTakeAction.Visible = false;
                btnTakeActionApprove.Visible = false;
                btnTakeActionReject.Visible = false;
            }
            btnSavePS.Visible = Helper.IsLoggedInUserInAdminRole() &&
                (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerInformation);
            if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName)
                //&& this.WorkflowPage.RegistrationStep != CON.RegistrationPageType.Agreements && this.WorkflowPage.RegistrationStep != CON.RegistrationPageType.GroupAffiliations
                )
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                var ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
                if (Helper.HasRows(ds))
                {
                    int currentStatus = Convert.ToInt32(ds.Tables[0].Rows[0]["RegProgramStatusTypeID"]);
                    if (this.WorkflowPage.CurrentTaskName != "Provider Review" && currentStatus == CON.RegistrationProgramStatusTypeId.Maintenance)
                    {
                        btnSavePS.Visible = false;
                    }
                    else
                    {
                        btnSavePS.Visible = true;
                    }
                }
                if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ContractMaintenance)
                {
                    btnSavePS.Visible = false;
                }
            }
            //OHPNM-18916, 18885 - Do not show Save button on owner or provider screening page so that the section status will not be changed to Modified and thus Approve button is shown correctly on provider review
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerScreening)
            {
                btnSavePS.Visible = btnSave.Visible = btnTakeActionApprove.Visible = false;
                btnTakeActionReject.Visible = false;
            }

            if (btnTakeAction.Visible)
            {
                if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.AffiliationScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.HouseholdMemberScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.SiteVisitScreening ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.BackgroundCheck ||
                    this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OrientationInformation)
                {
                    mltTakeAction.SetActiveView(vwScreeningComplete);
                }
                else if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ProviderCredentialing)
                {
                    mltTakeAction.SetActiveView(vwCredScreeningComplete);
                }
                else
                {
                    foreach (ListItem li in rblPSReview.Items)
                    {
                        li.Attributes.Add("onclick", "javascript:TogglePanel('" + rblPSReview.ClientID + "','" + pnlPSReview.ClientID + "');");
                        if (li.Text == "Return to Provider" && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "FAOperator2"))
                            li.Text = "Return to Accounting";
                    }
                    if (rblPSReview.SelectedIndex >= 0) pnlPSReview.Attributes.Add("style", "display:block;");
                    else pnlPSReview.Attributes.Add("style", "display:none;");

                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    // Page is 0 per Teresa return errors show on all pages based upon User's Role.
                    DataSet ds = psc.SelectRegErrorTypes(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), this.WorkflowPage.MyQueueSelectedRoleName);
                    if (Helper.HasRows(ds))
                    {
                        lsvReturnReasons.DataSource = ds.Tables[0];
                        lsvReturnReasons.DataBind();
                    }
                }
            }

            if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.WorkflowSteps || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.TransactionQueue) // // JIRA 3276
            {
                btnTakeActionApprove.Visible = false;
                btnTakeActionReject.Visible = false;
            }
            //if (pnlTakeAction.Visible)
            if (btnTakeActionApprove.Visible || btnTakeActionReject.Visible)
            {
                foreach (ListItem li in rblPSReview.Items)
                {
                    li.Attributes.Add("onclick", "javascript:TogglePanel('" + rblPSReview.ClientID + "','" + pnlPSReview.ClientID + "');");
                    if (li.Text == "Return to Provider" && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "FAOperator2"))
                        li.Text = "Return to Accounting";
                }
                if (rblPSReview.SelectedIndex >= 0) pnlPSReview.Attributes.Add("style", "display:block;");
                else pnlPSReview.Attributes.Add("style", "display:none;");
                // if (!Page.IsPostBack)
                // {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                // Page is 0 per Teresa return errors show on all pages based upon User's Role.
                //DataSet ds = psc.SelectRegErrorTypes(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                string userRole = (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview
                                    && Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)) ?
                                       CON.EnrollmentSpecialistRole : SessionVarRetriever.MyQueueSelectedRoleName;
                DataSet ds = psc.SelectRegErrorTypes(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), userRole);
                if (Helper.HasRows(ds))
                {
                    lsvReturnReasons.DataSource = ds.Tables[0];
                    lsvReturnReasons.DataBind();
                }
                // }
                // SAM505
                DataSet ds1 = psc.WF_SelectProcessParameters(this.WorkflowPage.WF_ProcessID);
                if (Helper.HasRows(ds1))
                {
                    DataRow dr = ds1.Tables[0].Rows[0];
                    divChkBCIText.Visible = string.IsNullOrEmpty(dr[CON.ProcessParameter.ShowRTPBCICheckBox].ToString()) ? false : Convert.ToBoolean(dr[CON.ProcessParameter.ShowRTPBCICheckBox]);
                }
            }
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.OrientationSessionScreening)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ORIENTATION");
                DataTable dtOrientation = null;
                DataRow[] dr1 = null;
                if (Helper.HasRows(ds))
                {
                    dtOrientation = ds.Tables[0];
                    dr1 = dtOrientation.Select("ORIENTATION_STATUS_TYPE_ID ='" + CON.OrientationStatusType.Completed.ToString() + "' OR ORIENTATION_STATUS_TYPE_ID ='" + CON.OrientationStatusType.Failed.ToString() + "'");
                }
                if (dr1 != null && dr1.Length > 0)
                    btnTakeAction.Visible = true;
                else
                    btnTakeAction.Visible = false;
                chkScreeningComplete.Text = "Orientation Session Screening Complete";
            }
            if ((this.WorkflowPage.CurrentTaskName == "Fingerprint and Background Entry" || this.WorkflowPage.CurrentTaskName == "Fingerprint and Background Complete") && stepTxt == CON.RegistrationPageName.FingerprintandBackgroundCheck)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds2 = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "BACKGROUND_CHECK");
                DataRow[] drSendNotice = ds2.Tables[0].Select("BACKGROUND_STATUS_TYPE_ID IN (1,2)");
                if (drSendNotice.Length == ds2.Tables[0].Rows.Count)
                    btnTakeAction.Visible = true;
                else
                    btnTakeAction.Visible = false;
            }
            if (this.WorkflowPage.CurrentTaskName == "Record Termination")
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

                DataSet dsTermStatus = psc.GetTerminationStatuses();
                this.ddlStatus.DataSource = dsTermStatus;
                this.ddlStatus.DataValueField = "ENROLLMENT_STATUS_CODE";
                this.ddlStatus.DataTextField = "ENROLLMENT_STATUS_CODE_DESCRIPTION";
                this.ddlStatus.DataBind();
                this.ddlStatus.Items.Insert(0, new ListItem("", ""));

                DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
                if (Helper.HasRows(ds))
                {
                    DataRow rowTerm = ds.Tables[0].Rows[0];
                    string effectiveDate = Helper.GetDateTime("ChangeEffectiveDate", rowTerm).ToString("MM/dd/yyyy");
                    string reenrollmentDueDate = Helper.GetDateTime("RevalidationDate", rowTerm).ToString("MM/dd/yyyy");
                    string termDate = Helper.GetDateTime("TerminationDate", rowTerm).ToString("MM/dd/yyyy");

                    this.txtEffectiveDate.Text = (effectiveDate == "01/01/1753") ? "" : effectiveDate;
                    this.txtReenrollmentDueDate.Text = (reenrollmentDueDate == "01/01/1753") ? "" : reenrollmentDueDate;
                    this.txtTermDate.Text = (termDate == "01/01/1753") ? "" : termDate;
                    this.txtEnrollmentStatus.Text = Helper.GetString("EnrollmentStatusCodeDescription", rowTerm);
                    this.ddlStatus.SelectedIndex = 0;


                }
                mltTakeAction.SetActiveView(vwOperatorTerminate);
            }
            if (Registration.IsComplianceTaskType(this.WorkflowPage.CurrentTaskName))
            {
                btnSavePS.Visible = btnCancelChanges.Visible = btnCancelPS.Visible = true;
            }

            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PlaceRiskAlertchopandEffectiveDate)
            {
                btnSavePS.Visible = btnCancelChanges.Visible = btnCancelPS.Visible = true;
            }

        }
        else if (idx == 2)
        {

            mltNavigation.ActiveViewIndex = 3;
            if (this.WorkflowPage.RegistrationIdSelected > 0)             // Registration is Read Only
            {
                btnSATakeAction.Visible = false;
                return;
            }
            // Check the WF_TASK_PAGE_ACTION table
            DataRow row = Registration.GetTakeActionRow(Registration.GetStepText(this.WorkflowPage.RegistrationStep), this.WorkflowPage.WF_TaskID);
            if (row != null)
            {
                // If we have a row from the REG_PAGE_SETTING_ACTION file then its decision is final
                if (!Helper.GetBool("TAKE_ACTION_ALLOWED", row))
                {
                    btnSATakeAction.Visible = false;
                    return;
                }
                else
                {
                    btnSATakeAction.Visible = true;          // Action is allowed, continue
                }
            }
            if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name)
                && (stepTxt == CON.RegistrationPageName.ProviderScreening || stepTxt == CON.RegistrationPageName.IndividualMemberScreening
                        || stepTxt == CON.RegistrationPageName.OwnerScreening || stepTxt == CON.RegistrationPageName.HouseholdMemberScreening
                        || stepTxt == CON.RegistrationPageName.SiteVisitScreening
                        ))
            {
                if (stepTxt != CON.RegistrationPageName.SiteVisitScreening)
                    btnSATakeAction.Visible = Registration.IsStateAdminTaskType(this.WorkflowPage.CurrentTaskName);
                mltTakeAction.SetActiveView(vwScreeningReview);
            }
            else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name)
                && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.RetroReview))
            {
                //Retro Effective Date Review
                btnSATakeAction.Visible = true;
                mltTakeAction.SetActiveView(vwRetroEffectiveDateReview);
            }
            else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name)
                && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.GroupMemberRetroReview))
            {
                //Retro Effective Date Review

                btnSATakeAction.Visible = GetGroupMemberRetro();
                mltTakeAction.SetActiveView(vwMemberRetroEffectiveDateReview);
            }
        }
        else if (idx == 4)
        {
            mltNavigation.ActiveViewIndex = 4;
        }
        else if (idx == 5)
        {
            mltNavigation.ActiveViewIndex = 5;
            mltTakeAction.SetActiveView(vwFinancialReviewComplete);
        }
        else if (idx == 6 || idx == 7)
        {
            mltNavigation.ActiveViewIndex = 1;
            /*
            DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int WorkFlowEventID = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", dr);
            if (WorkFlowEventID == CON.WorkflowEventType.UpdateReg || WorkFlowEventID == CON.WorkflowEventType.RevalReg || this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
            {
                rblApproveOrDeny.Items.RemoveAt(1);
                ListItem li = new ListItem();
                li.Text = "Terminate Application";
                li.Value = "TerminateApplication";
                if (rblApproveOrDeny.Items.Count == 1)
                    rblApproveOrDeny.Items.Add(li);
            }
            mltTakeAction.SetActiveView(vwApplicationDisposition);
            if ((this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Identification && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.LTCReview) ||
                (stepTxt == CON.RegistrationPageName.ProviderScreening && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.PendingApproval &&
                this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.PendingDenial && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.PendingTerminate
                && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.LTCReview)
                || ((stepTxt == CON.RegistrationPageName.Agreements) && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingApproval || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingDenial
                || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingTerminate)))
            {
                btnApplicationDisposition.Visible = true;
            }
            else
            {
                btnApplicationDisposition.Visible = false;
            }
            if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Appeals && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadFinalNoticeofDenial && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadFinalNoticeofTermination && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadInitialNoticeofDenial && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadInitialNoticeofTermination)
            {
                btnSavePS.Visible = false;
            }
            if (this.WorkflowPage.RegistrationStep != CON.SectionTypeID.Appeals && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofTermination))
            {
                btnSavePS.Visible = false;
            }
            */
        }
        else if (idx == 8)
        {
            mltNavigation.ActiveViewIndex = 6;
            mltTakeAction.SetActiveView(vwApplicationDisposition);
            if (stepTxt == CON.RegistrationPageName.Agreements)
            {
                btnApplicationDisposition.Visible = true;
            }
            else
            {
                btnApplicationDisposition.Visible = false;
            }
        }
        else if (idx == 9)
        {
            DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int WorkFlowEventID = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", dr);
            if (WorkFlowEventID == CON.WorkflowEventType.UpdateReg || WorkFlowEventID == CON.WorkflowEventType.RevalReg || this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks)
            {
                rblDHCFRecommendation.Items.RemoveAt(1);
                ListItem li = new ListItem();
                li.Text = "Reviewed and Recommend Terminate";
                li.Value = "TerminateApplication";
                if (rblDHCFRecommendation.Items.Count == 1)
                    rblDHCFRecommendation.Items.Add(li);
            }
            mltNavigation.ActiveViewIndex = 1;
            mltTakeAction.SetActiveView(vwDHCFRecommendation);
            if (stepTxt == CON.RegistrationPageName.Agreements)
            {
                btnTakeAction.Visible = true;
            }
            else
            {
                btnTakeAction.Visible = false;
            }
            if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Appeals && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadFinalNoticeofDenial && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadFinalNoticeofTermination && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadInitialNoticeofDenial && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.UploadInitialNoticeofTermination)
            {
                btnSavePS.Visible = false;
            }
        }
        else if (idx == 10)
        {
            mltNavigation.ActiveViewIndex = 1;
            mltTakeAction.SetActiveView(vwDHCFApplicationFeeRecommendation);
            if (stepTxt == CON.RegistrationPageName.ApplicationFee)
            {
                btnTakeAction.Visible = true;
            }
            else
            {
                btnTakeAction.Visible = false;
            }
        }
        else if (idx == 11)
        {
            mltNavigation.ActiveViewIndex = 1;
            mltTakeAction.SetActiveView(vwStateApplicationFeeRecommendation);
            if (stepTxt == CON.RegistrationPageName.ApplicationFee)
            {
                btnTakeAction.Visible = true;
            }
            else
            {
                btnTakeAction.Visible = false;
            }
        }
        else if (idx == 12)
        {
            DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int WorkFlowEventID = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", dr);
            if (WorkFlowEventID == CON.WorkflowEventType.UpdateReg || WorkFlowEventID == CON.WorkflowEventType.RevalReg)
            {
                rblDBHRecommendation.Items.RemoveAt(1);
                ListItem li = new ListItem();
                li.Text = "Reviewed and Recommend Terminate";
                li.Value = "TerminateApplication";
                if (rblDBHRecommendation.Items.Count == 1)
                    rblDBHRecommendation.Items.Add(li);
            }
            mltNavigation.ActiveViewIndex = 1;
            mltTakeAction.SetActiveView(vwDBHRecommendation);
            if (stepTxt == CON.RegistrationPageName.ProviderScreening || stepTxt == CON.RegistrationPageName.OwnerScreening)
            {
                btnTakeAction.Visible = true;
            }
            else
            {
                btnTakeAction.Visible = false;
            }

            valCommentReqd.Enabled = false;
        }
        else if (idx == 13)
        {
            DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int WorkFlowEventID = Helper.GetInt("WORKFLOW_EVENT_TYPE_ID", dr);
            if (WorkFlowEventID == CON.WorkflowEventType.UpdateReg || WorkFlowEventID == CON.WorkflowEventType.RevalReg)
            {
                rblDDSRecommendation.Items.RemoveAt(1);
                ListItem li = new ListItem();
                li.Text = "Reviewed and Recommend Terminate";
                li.Value = "TerminateApplication";
                if (rblDDSRecommendation.Items.Count == 1)
                    rblDDSRecommendation.Items.Add(li);
            }
            mltNavigation.ActiveViewIndex = 1;
            mltTakeAction.SetActiveView(vwDDSRecommendation);
            if (stepTxt == CON.RegistrationPageName.Agreements)
            {
                btnTakeAction.Visible = true;
            }
            else
            {
                btnTakeAction.Visible = false;
            }

            valCommentReqd.Enabled = false;
        }
        else if (idx == 14)
        {
            btnSave.Visible = btnSaveNext.Visible = btnPreviousPS.Visible = btnNextPS.Visible = true;
            //mltNavigation.ActiveViewIndex = 1;
        }
        else
        {
            // Provider view
            mltNavigation.ActiveViewIndex = 0;
            bool regSubmitted = Helper.ApplicationStatusType(this.WorkflowPage.RegistrationId).Equals("Submitted");
            if (Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName, this.WorkflowPage.CommandName))            // Registration is in Review
            {
                btnSave.Visible = false;
                return;
            }
            else
            {
                btnSave.Visible = !regSubmitted;             // A provider can view when submitted, but not save.
            }
            if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
            {
                if (this.WorkflowPage.IsAddODMorODAMedSvc && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                {
                    btnPrevious.Visible = true;
                    btnSaveNext.Visible = true;
                }
                else
                {
                    btnPrevious.Visible = false;
                    btnSaveNext.Visible = false;
                }

                // SAM537 Suspended provider can start ODM update and only change primary contact addr, billing addr, correspondence addr,1099 address and home office addr
                bool enableCR537 = Convert.ToBoolean(AppSettings.Get("EnableCR537", "false"));
                if (enableCR537 && this.WorkflowPage.IsSuspendedProvider && !Helper.IsRevertSuspensionWF(this.WorkflowPage.WF_ProcessID))
                {
                    if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.PrimaryContactAddress || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.BillingPaymentAddress ||
                        this.WorkflowPage.RegistrationStep == CON.SectionTypeID.CorrespondenceAddress || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Address1099Form ||
                        this.WorkflowPage.RegistrationStep == CON.SectionTypeID.HomeOfficeAddress || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.RestrictedService)
                    {
                        btnSave.Visible = true;
                    }
                    else
                        btnSave.Visible = false;
                }
            }
            else if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.RevalReg)
            {
                if (this.WorkflowPage.RegistrationStep == 1 && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                {
                    btnPrevious.Visible = true;
                    btnSaveNext.Visible = true;
                }
            }
            else
            {
                btnPrevious.Visible = (this.WorkflowPage.RegistrationSequence != 1)
                                    && !regSubmitted;   // A provider can view when submitted, but not save.
                btnSaveNext.Visible = (this.WorkflowPage.RegistrationSequence <= this.WorkflowPage.RegistrationNodes.Count)
                                    && !regSubmitted;   // A provider can view when submitted, but not save.
            }

            //SAM818 CMC Contact Info page is read only if not in CMC wf
            if (this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.CMC && this.WorkflowPage.RegistrationStep == CON.SectionTypeID.CMCContactInformation
                 && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
            {
                btnSaveNext.Visible = false;
                btnSave.Visible = false;
            }

        }

        pnlAppealProcessStatus.Visible = false;
        if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name)
            && (stepTxt == CON.RegistrationPageName.ProviderScreening || stepTxt == CON.RegistrationPageName.IndividualMemberScreening
                    || stepTxt == CON.RegistrationPageName.OwnerScreening || stepTxt == CON.RegistrationPageName.HouseholdMemberScreening
                    || stepTxt == CON.RegistrationPageName.SiteVisitScreening
                    ))
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
            DataTable dtProvider = new DataTable();

            if (Helper.HasRows(ds))
            {
                dtProvider = ds.Tables[0];
                int partyID = Helper.GetInt("PARTY_ID", dtProvider.Rows[0]);
                int appealStatusID = Helper.GetInt("APPEAL_STATUS_ID", dtProvider.Rows[0]);
                string taskName = Helper.GetString("TASK_NAME", dtProvider.Rows[0]);

                if ((taskName == CON.RegistrationTaskName.ProcessAppeal
                    || taskName == CON.RegistrationTaskName.ProcessAppealSiteVisit)
                    && appealStatusID != 0 && appealStatusID != null)
                {
                    pnlAppealProcessStatus.Visible = true;
                    lblAppealBeginDateValue.Text = Convert.ToDateTime(Helper.GetString("APPEAL_START_DATE", dtProvider.Rows[0])).Date.ToShortDateString();
                    lblAppealEndDateValue.Text = Convert.ToDateTime(Helper.GetString("APPEAL_END_DATE", dtProvider.Rows[0])).Date.ToShortDateString();
                    lblAppealStatusValue.Text = GetAppealStatusName(Helper.GetString("APPEAL_STATUS_ID", dtProvider.Rows[0]));
                }
                else
                {
                    pnlAppealProcessStatus.Visible = false;
                }
            }
        }
        if (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderReview && !(this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview && this.WorkflowPage.RegistrationStep == CON.SectionTypeID.ApplicationFee))
        {
            // pnlTakeAction.Visible = false;
            btnTakeActionApprove.Visible = false;
            btnTakeActionReject.Visible = false;
        }
        if (this.WorkflowPage.RegistrationStep != CON.SectionTypeID.Appeals && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial ||
                this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofDenial ||
                this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofTermination))
        {
            btnSaveApplicationDisposition.Visible = btnSavePS.Visible = false;
        }
        //if(pnlTakeAction.Visible)
        if (btnTakeActionApprove.Visible || btnTakeActionReject.Visible)
            ValidateNPI();


        // SAM459
        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CredentialReconsideration
             && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.CredentialingReconsideration
             && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CommitteeQualitySpecialist)
                 || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ODMCredentialingSupervisor)))
        {
            btnSave.Visible = false;
            btnCancelChanges.Visible = true;
            btnPrevious.Visible = true;
        }
    }



    private bool GetGroupMemberRetro()
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATION");
        DataTable dt = null;
        string filter =
            //bug 2338 - Do not show Removed by Group or Removed by Individual
            "(AffiliationStatus <> '" + CON.GroupAffiliationStatusType.RemovedbyGroup + "'"
            + " AND AffiliationStatus <> '" + CON.GroupAffiliationStatusType.RemovedbyIndividual + "')"
            + " OR END_DATE IS NOT NULL";

        if (Helper.HasRows(ds))
        {
            DataRow[] rows = ds.Tables[0].Select(filter, "NAME ASC");
            if (rows.Length > 0)
            {
                dt = rows.CopyToDataTable();
            }
            else
            {
                dt = new DataTable();
            }
        }
        else
        {
            dt = new DataTable();
        }
        DataRow[] dr = dt.Select("RETRO_REVIEW_REQUIRED_ID = '" + CON.GroupMemberRetroStatusID.RetroReviewCompleted + "'");
        if (dr.Length > 0)
        {
            return false;
        }
        return true;
    }
    private void InitRetroReviewData()
    {
        this.lblRetroSubmitDate.Text = string.Empty;
        this.lblRetroRequestedEffectiveDate.Text = string.Empty;
        this.txtRetroApprovedEffectiveDate.Text = string.Empty;
        this.txtRetroComments.Text = string.Empty;
    }

    private void LoadRetroDecisionDropDown()
    {
        if (this.rblRetroReviewDecision.Items.Count > 0)
        {
            //init
            this.rblRetroReviewDecision.SelectedIndex = -1;
            return;
        }

        this.rblRetroReviewDecision.Items.Add(new ListItem(CON.RetroEfectiveDateOptions.ApproveProviderRequest, CON.RetroEfectiveDateOptions.ApproveProviderRequest));
        this.rblRetroReviewDecision.Items.Add(new ListItem(CON.RetroEfectiveDateOptions.UseSubmissionDate, CON.RetroEfectiveDateOptions.UseSubmissionDate));
        this.rblRetroReviewDecision.Items.Add(new ListItem(CON.RetroEfectiveDateOptions.OverrideEffectiveDate, CON.RetroEfectiveDateOptions.OverrideEffectiveDate));
    }

    private void LoadRetroReviewData()
    {
        InitRetroReviewData();
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistration(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            DateTime tmp;
            if (DateTime.TryParse(Helper.GetString("REQUESTED_EFFECTIVE_DATE", dr), out tmp))
            {
                this.lblRetroRequestedEffectiveDate.Text = tmp.ToString("MM/dd/yyyy");
            }
            if (DateTime.TryParse(Helper.GetString("SUBMIT_DATE_TIME", dr), out tmp))
            {
                this.lblRetroSubmitDate.Text = tmp.ToString("MM/dd/yyyy");
            }
        }

        LoadRetroDecisionDropDown();
    }
    private void LoadGroupMemberRetroReviewData()
    {
        //InitGroupMemberRetroReviewData();
        //load group member details to grid
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet dsReg = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "GroupMemberRequiresRetroReview");


        grdMemberRetroEffectiveDateReview.DataSource = dsReg;
        grdMemberRetroEffectiveDateReview.DataBind();
    }

    private void SaveGroupMemberRetroDecision()
    {
        //save start_date

        //save
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        foreach (GridViewRow row in grdMemberRetroEffectiveDateReview.Rows)
        {
            if (((DropDownList)row.FindControl("ddlDecision")).SelectedIndex > 0 && !String.IsNullOrEmpty(((TextBox)row.FindControl("txtApprovedEffectiveDate")).Text))
            {
                Dictionary<string, string> parms = this.SetCommonParms();
                parms.Add("START_DATE", ((TextBox)row.FindControl("txtApprovedEffectiveDate")).Text);
                parms.Add("RETRO_REVIEW_REQUIRED_ID", MAXIMUS.Core.Libraries.Constants.GroupMemberRetroStatusID.RetroReviewCompleted.ToString());
                //parms.Add("GROUP_AFFILIATION_STATUS_ID", MAXIMUS.Core.Libraries.Constants.GroupAffiliationStatusTypeID.GroupConfirmed.ToString());
                parms.Add("REG_AFFILIATION_ID", grdMemberRetroEffectiveDateReview.DataKeys[row.RowIndex].Values["REG_AFFILIATION_ID"].ToString());
                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATIONcustom", parms);
                //insert provider note;
                string comments = ((TextBox)row.FindControl("txtComments")).Text;
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                    svc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.RegistrationRejection, comments.Trim(),
                             DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }


        }

    }

    private Dictionary<string, string> SetCommonParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        /*parms.Add("FIRST_NAME", txtFirstName.Text.Trim());
        parms.Add("LAST_NAME", txtLastName.Text.Trim());
        parms.Add("NAME", string.Concat(txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim()));
        parms.Add("NPI", txtNPI.Text.Trim());
        parms.Add("SSN", txtTaxID.Text.Trim());
        parms.Add("START_DATE", txtStartDate.Text);
        parms.Add("END_DATE", string.IsNullOrEmpty(txtEndDate.Text) ? null : txtEndDate.Text);
        parms.Add("INVALID_FLAG", chkInvalid.Checked ? "1" : "0");*/
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }

    private void SaveRetroDecision()
    {
        DateTime effectiveDate = DateTime.Now;
        switch (this.rblRetroReviewDecision.SelectedValue)
        {
            case CON.RetroEfectiveDateOptions.ApproveProviderRequest:
                //Set Effective Date = Requested Effective Date
                effectiveDate = Convert.ToDateTime(this.lblRetroRequestedEffectiveDate.Text.Trim());
                break;
            case CON.RetroEfectiveDateOptions.UseSubmissionDate:
                //Set Effective Date = Application Submission Date
                effectiveDate = Convert.ToDateTime(this.lblRetroSubmitDate.Text.Trim());
                break;
            case CON.RetroEfectiveDateOptions.OverrideEffectiveDate:
                //Set Effective Date = Approved Effective Date - input field
                effectiveDate = Convert.ToDateTime(this.txtRetroApprovedEffectiveDate.Text.Trim());
                break;
            default:
                return;
        }

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        svc.RetroEffectiveDateProvider(this.WorkflowPage.RegistrationId, effectiveDate, this.txtRetroComments.Text.Trim(), DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true, DateTime.Now);

    }

    private string GetAppealStatusName(string appealStatusID)
    {
        string AppealStatusName = "";
        switch (appealStatusID)
        {
            case "1":
                AppealStatusName = CON.AppealStatusName.NoticeSent;
                break;
            case "2":
                AppealStatusName = CON.AppealStatusName.NoAppeal;
                break;
            case "3":
                AppealStatusName = CON.AppealStatusName.ProviderRequestedAppeal;
                break;
            case "4":
                AppealStatusName = CON.AppealStatusName.AppealWon_ReinstateProvider;
                break;
            case "5":
                AppealStatusName = CON.AppealStatusName.AppealLost_ProcessDenial_Termination;
                break;

        }
        return AppealStatusName;
    }

    private void SetupButtonDisablesOfMultiClick()
    {
        //if required trigger validation on client side
        //btnSave.Attributes.Add("onclick", " this.disabled = true; /*" + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";*/");
        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnSave.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
            "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");

        //Multiple click for approve button
        btnTakeActionApprove.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnTakeActionApprove.ValidationGroup +
           "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnTakeActionApprove, null) + ";");

        btnSavePS.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSavePS.ValidationGroup +
         "')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSavePS, null) + ";");

        //btnSaveNext.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSave.ValidationGroup +
        //"')){this.disabled=true;} else { return false; } " + this.Page.ClientScript.GetPostBackEventReference(btnSaveNext, null) + ";");

        //btnSaveNext.Attributes.Add("onclick", " this.disabled = true; __doPostBack('"+ btnSaveNext.ClientID + "','')" + ";WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions('" + btnSaveNext.ClientID + "', '', false, '', '', false, false));");
        PostBackOptions myPostBackOptions = new PostBackOptions(btnSaveNext);
        myPostBackOptions.ActionUrl = "Registration.aspx";
        //myPostBackOptions.AutoPostBack = false;
        myPostBackOptions.RequiresJavaScriptProtocol = true;
        myPostBackOptions.PerformValidation = false;
        myPostBackOptions.ValidationGroup = "";
        btnSaveNext.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnSaveNext.ValidationGroup +
            "')){this.disabled=true;}  " + this.Page.ClientScript.GetPostBackEventReference(btnSaveNext, null) + ";");
        btnPrevious.Attributes.Add("onclick", "if(Page_ClientValidate('" + btnPrevious.ValidationGroup +
    "')){this.disabled=true;}  " + this.Page.ClientScript.GetPostBackEventReference(btnPrevious, null) + ";");
        //btnSaveNext.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSaveNext, null) + ";");
        //btnSaveNext.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(myPostBackOptions) + ";");
        btnPrevious.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnPrevious, null) + ";");
        btnSavempe.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSavempe, null) + ";");
        //btnSaveNext.CausesValidation = false;
        /*if (!IsPostBack)
        {
            if (rblScreeningReview.SelectedValue == CON.StateReviewScreeningStatus.Confirm.ToString())
            {
                txtScreeningReviewComments.Style["display"] = "block";
                lblScreeningReviewComments.Style["display"] = "block";
            }
            else
            {
                txtScreeningReviewComments.Style["display"] = "none";
                lblScreeningReviewComments.Style["display"] = "none";
            }
        }*/

    }

    protected override void OnInit(EventArgs e)
    {
        base.OnInit(e);
        ucUploadDocumentDHCF.RenameFileMethod += new UserControls_UploadDocument.RenameFile(ucUploadDocument_RenameFileMethod);
        ucUploadDocumentDHCF.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocumentDHCF_SuccessEvent);
        ucUploadDocumentDHCF.FileDeleteSuccessEvent += new UserControls_UploadDocument.FileDeleteEventHandler(ucUploadDocument_FileDeleteEvent);

        ucUploadDocumentDBH.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocumentDBH_SuccessEvent);

        ucUploadDocumentDBHReview.RenameFileMethod += new UserControls_UploadDocument.RenameFile(ucUploadDocument_RenameFileMethod);
        ucUploadDocumentDBHReview.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocumentDBHReview_SuccessEvent);
        ucUploadDocumentDBHReview.FileDeleteSuccessEvent += new UserControls_UploadDocument.FileDeleteEventHandler(ucUploadDocument_FileDeleteEvent);

        ucUploadDocumentDDSReview.RenameFileMethod += new UserControls_UploadDocument.RenameFile(ucUploadDocument_RenameFileMethod);
        ucUploadDocumentDDSReview.SuccessEvent += new UserControls_UploadDocument.SuccessEventHandler(ucUploadDocumentDDSReview_SuccessEvent);
        ucUploadDocumentDDSReview.FileDeleteSuccessEvent += new UserControls_UploadDocument.FileDeleteEventHandler(ucUploadDocument_FileDeleteEvent);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.WorkflowPage.BumpStep = 0;

        ucUploadDocumentDHCF.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocumentDHCF.SetUploadButtonEnabled = false;

        ucUploadDocumentDBH.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocumentDBH.SetUploadButtonEnabled = false;

        ucUploadDocumentDBHReview.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocumentDBHReview.SetUploadButtonEnabled = false;

        ucUploadDocumentDDSReview.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly)) ucUploadDocumentDDSReview.SetUploadButtonEnabled = false;

        if (EntityTypeId == 0)
        {
            int diddReferralId = 0, entityTypeId = 0, providerTypeId = 0, specialtyTypeID = 0, taxIDTypeID = 0;
            string taxID = string.Empty;
            Registration.SetEntityProviderTypesDIDD(this.WorkflowPage.RegistrationId, ref entityTypeId, ref providerTypeId, ref diddReferralId, ref specialtyTypeID, ref taxID, ref taxIDTypeID);
            EntityTypeId = entityTypeId;
            ProviderTypeId = providerTypeId;
            DIDDReferralId = diddReferralId;
            SpecialtyTypeID = specialtyTypeID;
        }
        string clientID = string.Empty;
        clientID = this.ClientID + "bhmpe";
        mpe.BehaviorID = clientID;
        if (!IsPostBack)
        {
            //btnTakeActionReject.OnClientClick = ;
            btnTakeActionReject.Attributes.Add("OnClick", "javascript: showPopup('" + this.ClientID + "bhmpe'); return false; ");
            btnApplicationDisposition.Attributes.Remove("onclick");
            btnApplicationDisposition.Attributes.Add("onclick", "javascript: showTakeActionPopup('" + mpe.ClientID + "'); return false; ");

        }
    }


    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (Helper.IsModern())
        {
            btnSaveNext.CssClass = "buttonBoxFocus";
            btnSaveNext.CssClass = "buttonBoxNext";
            btnNextPS.CssClass = "buttonBoxNext";
            btnSave.CssClass = "buttonBoxFocus";
            btnPrevious.CssClass = "buttonBoxPrevious";
            btnPreviousPS.CssClass = "buttonBoxPrevious";
            btnTakeActionApprove.CssClass = "buttonBoxFocus";
        }

        // Make sure sequence is set correctly; OHPNM-20089 if the sequence is not well defined, use the first
        if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
        {
            this.WorkflowPage.RegistrationSequence = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].Sequence;
        }
        else
        {
            if (this.WorkflowPage.RegistrationNodes.Count >= 1)
            {
                this.WorkflowPage.RegistrationSequence = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key].Sequence;
            }
        }

        SetupButtonDisablesOfMultiClick();

        string currentTaskName = this.WorkflowPage.CurrentTaskName;
        if ((Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name) || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CHOP)) && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry)
        {
            // Provider Services view
            SetButtons(1);

            //if(this.WorkflowPage.WF_WorkflowID==CON.WorkflowType.RiskAlertClosure)
            //{
            //    SetButtons(14);
            //}
        }
        else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.ProcessAppeal:
                case CON.RegistrationTaskName.ProcessAppealSiteVisit:
                    SetButtons(4);
                    break;
                case CON.RegistrationTaskName.PendingApproval:
                case CON.RegistrationTaskName.PendingDenial:
                case CON.RegistrationTaskName.PendingTerminate:

                    SetButtons(7);
                    break;
                case CON.RegistrationTaskName.ApplicationFeeDenialReview:
                case CON.RegistrationTaskName.ApplicationFeeReview:
                    SetButtons(11);
                    break;
                default:
                    SetButtons(2);
                    break;
            }
        }
        else if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.UploadInitialNoticeofDenial:
                case CON.RegistrationTaskName.UploadInitialNoticeofTermination:
                case CON.RegistrationTaskName.UploadFinalNoticeofDenial:
                case CON.RegistrationTaskName.UploadFinalNoticeofTermination:
                    SetButtons(7);
                    break;
            }
        }
        else if (Helper.IsUserInORFAWorkerRole(HttpContext.Current.User.Identity.Name))
        {
            SetButtons(5);
        }
        else if (Helper.IsUserInStateReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.ReviewApproval:
                case CON.RegistrationTaskName.ReviewDenial:
                case CON.RegistrationTaskName.ReviewTermination:
                case CON.RegistrationTaskName.DenialTerminationNotification:
                    ucUploadDocumentDHCF.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
                    ucUploadDocumentDHCF.Visible = true;
                    SetButtons(9);
                    break;
                case CON.RegistrationTaskName.ReviewApplicationFee:
                    SetButtons(10);
                    break;
            }
        }
        else if (Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.DBHReviewerReview:
                    ucUploadDocumentDBHReview.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
                    ucUploadDocumentDBHReview.Visible = true;
                    SetButtons(12);
                    break;
            }
        }
        else if (Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name))
        {
            SetButtons(6);
        }
        else if (Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.DBHProviderReview:
                    ucUploadDocumentDBH.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
                    ucSepUploadAppDisposition.Visible = ucUploadDocumentDBH.Visible = true;
                    SetButtons(7);
                    break;
            }
        }
        else if (Helper.IsUserInDBHReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.DBHReviewerReview:
                    ucUploadDocumentDBH.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
                    ucSepUploadAppDisposition.Visible = ucUploadDocumentDBH.Visible = true;
                    SetButtons(12);
                    break;
            }
        }
        else if (Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name))
        {
            SetButtons(8);
        }
        else if (Helper.IsUserInDDSReviewerRole(HttpContext.Current.User.Identity.Name))
        {
            switch (currentTaskName)
            {
                case CON.RegistrationTaskName.DDSReviewerReview:
                    ucUploadDocumentDDSReview.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
                    ucUploadDocumentDDSReview.Visible = true;
                    SetButtons(13);
                    break;
            }
        }
        else if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.LTCInitialRevalidation) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RiskAlertClosure)
        {
            btnPrevious.Visible = btnSave.Visible = btnSaveNext.Visible = btnSavePS.Visible = btnCancelChanges.Visible = btnNextPS.Visible = false;
            SetButtons(14);
        }
        else if ((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.LTCCHOP) || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.TechAdmin)) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RiskAlertCHOP)
        {
            btnPrevious.Visible = btnSave.Visible = btnSaveNext.Visible = btnSavePS.Visible = btnCancelChanges.Visible = btnNextPS.Visible = false;
            SetButtons(14);
        }
        else
        {
            // Provider view
            SetButtons(0);
            // For the Agreements page, make the Save button this Save button.
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Agreements)
            {
                Button btn = (Button)Helper.FindTheControl(this.Page, "btnSaveSignature");
                if (btn != null) btn.Attributes.Add("onclick", " this.disabled = true; " +
                    this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
            }
        }
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.WorkflowSteps || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.MMISTransactions || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.TransactionQueue) // JIRA 3276
        {
            btnPrevious.Visible = btnSave.Visible = btnSaveNext.Visible = btnSavePS.Visible = btnCancelChanges.Visible = btnNextPS.Visible = false;
        }
        if (this.WorkflowPage.CurrentTaskName == "Committee Review" || this.WorkflowPage.CurrentTaskName == "Committee Chairman Review")
        {
            btnPrevious.Visible = btnSave.Visible = btnSaveNext.Visible = btnSavePS.Visible = btnCancelChanges.Visible = btnNextPS.Visible = false;
        }
        if (this.WorkflowPage.CurrentTaskName == "Provider Review")
        {
            btnSaveNext.Visible = true;
        }

        if (this.WorkflowPage.CurrentTaskName != "Provider Review" && this.WorkflowPage.CurrentTaskName != "Application Fee Review")
            mpe.BehaviorID = "";

        // if the Reject button is visible and we're not posting back...
        // DCPDMS-2028 - populating the rejection reasons for a postback when the operator clicks the "I confirm that payment information has been verified in PECOS." checkbox.
        if (btnTakeActionReject.Visible && (!Page.IsPostBack || (Page.IsPostBack && currentTaskName == CON.RegistrationTaskName.ApplicationFeeReview && Helper.IsUserInPSRoles(HttpContext.Current.User.Identity.Name))))
        {
            // read the rejection reason choices from the database.
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            // Page is 0 per Teresa return errors show on all pages based upon User's Role.
            string userRole = (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview
                                    && Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)) ?
                                       CON.EnrollmentSpecialistRole : SessionVarRetriever.MyQueueSelectedRoleName;
            DataSet ds = psc.SelectRegErrorTypes(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), userRole);
            if (Helper.HasRows(ds))
            {
                lsvReturnReasons.DataSource = ds.Tables[0];
                lsvReturnReasons.DataBind();
            }
        }

        //ucUploadDocumentDHCF.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, ucUploadDocumentDHCF.DocumentSection);

    }
    private bool Save()
    {
        bool isValid = false;
        bool isRequired = false;
        if (ValidateIsRequiredDataEvent != null)
        {
            ValidateIsRequiredDataEvent(this.WorkflowPage.RegistrationStep, ref isRequired);
            if (!isRequired)
            {
                if (this.WorkflowPage.RegistrationStep == 45)
                {
                    //OHPNM-14215 - Not set the Provider_status_type_id on reg_Section_Status to Modified if clicked on Save in Provider review. Because of this the ApplicationComplete/Approve button is not getting shown even though they approve all the pages
                    if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && !this.WorkflowPage.IsAddODMorODAMedSvc
                        && this.WorkflowPage.CurrentTaskName != "Provider Review" && this.WorkflowPage.CurrentTaskName != "Provider Screening")
                        Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Modified);
                    else
                    {
                        Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Complete);
                    }
                }
                return true;
            }
        }
        if (ValidateDataEvent != null)
        {
            ValidateDataEvent(this.WorkflowPage.RegistrationStep, ref isValid);
            if (!isValid)
            {
                btnSave.Enabled = true;
                return false;
            }

        }
        else
        {
            // No event handler attached to the event - this is fatal
            return false;
        }

        if (SaveDataEvent != null)
        {
            if (!SaveDataEvent(this.WorkflowPage.RegistrationStep))
            {
                btnSave.Enabled = true;
                return false;                             // Save failed for some reason, get outahere
            }
            if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && !this.WorkflowPage.IsAddODMorODAMedSvc)
            {
                Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Modified);

            }
            else
            {
                if (this.WorkflowPage.WF_WorkflowID == 18) //TODO hardcoded for credentialing
                    Registration.SetProviderSectionNodeStatusIdForCredentialing(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Complete);
                else
                {
                    Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.Complete);

                }
            }

            bool enableCR701 = Convert.ToBoolean(AppSettings.Get("EnableCR701", "false"));
            if (enableCR701 && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview
                             && Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))
                             && (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.NPIandMedId || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Specialties))
            {
                var workflowEventTypeName = GetWorkflowEventTypeName();

                if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Specialties && workflowEventTypeName.ToLower() == CON.ApplicationStatus.Specialties_Update.ToLower())
                {
                    specialityConfirmPopup.Show();
                }
                else if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.NPIandMedId && (
                    workflowEventTypeName.ToLower() == CON.ApplicationStatus.NPI_InitialEnrollment.ToLower() ||
                    workflowEventTypeName.ToLower() == CON.ApplicationStatus.NPI_Reactivation.ToLower() ||
                    workflowEventTypeName.ToLower() == CON.ApplicationStatus.NPI_Reapplication.ToLower() ||
                    workflowEventTypeName.ToLower() == CON.ApplicationStatus.NPI_Reconsideration.ToLower() ||
                    workflowEventTypeName.ToLower() == CON.ApplicationStatus.NPI_Revalidation.ToLower()))
                {
                    specialityConfirmPopup.Show();
                }
            }
        }
        else
        {
            // No event handler attached to the event - this is fatal
            return false;
        }

        btnSave.Enabled = true;
        return isValid;
    }

    private string GetWorkflowEventTypeName()
    {
        string workflowEventTypeName = string.Empty;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("UserID", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectRegistrationHeader", parms);
        if (Helper.HasRows(ds))
        {
            workflowEventTypeName = Helper.GetString("workflow_event_type_name", ds.Tables[0].Rows[0]);
        }
        return workflowEventTypeName;
    }

    /// <summary>
    /// Validates and saves the page data, then moves to next step or not 
    /// according to the input parameter.
    /// </summary>
    /// <param name="action">indicates if the step should be changed
    /// action = 0 - don't change the step
    /// action = 1 - increment the step by one
    /// action = 2 - decrement the step by one
    /// </param>
    /// <returns>true if the save was successful; false if not</returns>
    private bool SaveAndRefresh(int action = 0)
    {
        bool isValid = false;
        isValid = Save();
        if (isValid && action != 3)
        {
            if (action == 1)
            {
                BumpStep(true);
            }
            else if (action == 2)
            {
                BumpStep(false);
            }
            Refresh(true);
        }

        return isValid;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveAndRefresh();
    }
    protected void btnSpecialityConfirmMP_Click(object sender, EventArgs e)
    {
        UpdateRegSectionStatus();

        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.NPIandMedId)
        {
            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "NPI and Med ID confirmed", processID: this.WorkflowPage.WF_ProcessID);
        }
        else if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Specialties)
        {
            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Specialties confirmed", processID: this.WorkflowPage.WF_ProcessID);
        }

        chkSpecialityMP.Checked = false;
    }

    private void UpdateRegSectionStatus()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("regId", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("ohId", SessionVarRetriever.OhID);
        parms.Add("sectionTypeID", this.WorkflowPage.RegistrationStep.ToString());
        psc.UpdateRegistrationDataWithParams("usp_Update_REG_Section_Status", parms);
    }

    protected void btnPrevious_Click(object sender, EventArgs e)
    {
        bool isValid = false;
        // Bug #3320 and 2920
        if (Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_StepID)
           && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.InformationrequiredIncreasedRiskLevel))
        {
            isValid = Save();
        }
        else
        {
            isValid = true;
        }
        if (isValid)
        {
            BumpStep(false);
            btnPrevious.Enabled = true;
            Refresh(true);
        }
        else
            return;

    }

    protected void btnPreviousPS_Click(object sender, EventArgs e)
    {
        // Bug 2210 - validate and save the practice type and the tax category for Operator Review of the W9 page.
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview &&
            Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) &&
            this.WorkflowPage.RegistrationStep == 36)
        {
            if (SaveAndRefresh(2))
            {
                btnPreviousPS.Enabled = true;
            }
        }
        else
        {
            BumpStep(false);
            Refresh();
        }
    }

    protected void btnNextPS_Click(object sender, EventArgs e)
    {
        bool isValid = false;
        // Bug 2210 - validate and save the practice type and the tax category for Operator Review of the W9 page.
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview &&
            Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) &&
            this.WorkflowPage.RegistrationStep == 36)
        {
            if (SaveAndRefresh(1))
            {
                btnPreviousPS.Enabled = true;
            }
        }
        else
        {
            isValid = true;
        }

        if (ValidateSaveNextDataEvent != null && isValid)
        {
            ValidateSaveNextDataEvent(this.WorkflowPage.RegistrationStep, ref isValid);
        }
        if (isValid)
        {
            BumpStep(true);
            Refresh();
        }
    }

    public void Refresh(bool forceRedirect = false)
    {
        // There is no force redirect. It is always refresh
        if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }

    private void BumpNextModifiedSection(bool moveForward)
    {
        // Check that the current Sequence matches what it should be and if not set it to the correct value
        int seq = 0;

        if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
        {
            seq = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].Sequence;
        }
        else
        {
            seq = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key].Sequence;
        }

        if (this.WorkflowPage.RegistrationSequence != seq) this.WorkflowPage.RegistrationSequence = seq;

        int increment = 1;

        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview)
        {
            List<RegistrationNode> modifiedNodes = this.WorkflowPage.RegistrationNodes.Where(s => s.Value.StatusId == CON.RegistrationProviderStatusTypeId.Modified || s.Value.ProviderStatusId == CON.RegistrationProviderStatusTypeId.Modified).Select(x => x.Value).ToList();
            int idx = modifiedNodes.FindIndex(n => n.Sequence == seq);
            if (modifiedNodes.Count > idx + 1 && moveForward == true)
            {
                if (idx == -1)
                {
                    foreach (RegistrationNode rn in modifiedNodes)
                    {
                        if (rn.Sequence >= seq)
                        {
                            increment = rn.Sequence;
                            break;
                        }
                    }
                }
                else
                    increment = modifiedNodes[idx + 1].Sequence;
            }
            else if ((idx + 1) == modifiedNodes.Count && moveForward == true)
                increment = idx;
            /*else if (moveForward == false && idx == 0)
            {
                increment = idx;
            }
            else if (modifiedNodes.FindIndex(t => t.Step == this.WorkflowPage.RegistrationStep) >= 0)
            {
                if (idx == -1)
                {
                    foreach (RegistrationNode rn in modifiedNodes)
                    {
                        if (rn.Sequence <= seq)
                        {
                            increment = rn.Sequence;
                            break;
                        }
                    }
                }
                else
                    increment = modifiedNodes[idx - 1].Sequence;
            }*/
            if (moveForward)
            {
                this.WorkflowPage.RegistrationSequence = increment;
                this.WorkflowPage.RegistrationStep = Registration.GetSequenceStep(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);
            }
            else
            {
                this.WorkflowPage.RegistrationSequence = increment;
                this.WorkflowPage.RegistrationStep = Registration.GetSequenceStep(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);
            }
            if ((this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC || this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC) && this.WorkflowPage.RegistrationStep == 1)
                this.WorkflowPage.RegistrationStep = CON.SectionTypeID.CpcContactInformation;

            this.WorkflowPage.BumpStep = this.WorkflowPage.RegistrationStep;

        }
    }
    private void BumpStep(bool moveForward)
    {
        // Check that the current Sequence matches what it should be and if not set it to the correct value

        int seq = 0;
        if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
        {
            seq = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].Sequence;
        }
        else
        {
            if (this.WorkflowPage.RegistrationNodes.Count >= 1)
            {
                seq = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key].Sequence;
            }
        }
        if (this.WorkflowPage.RegistrationSequence != seq) this.WorkflowPage.RegistrationSequence = seq;

        int increment = 1;
        int unapprovedSeq = Registration.GetNextUnApprovedSectionSequence(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);
        if (moveForward)
        {
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.Operator)
                || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.APMSpecialist) && this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC)))
            {
                //Set Next auto approved 
                this.WorkflowPage.RegistrationSequence = unapprovedSeq;
                this.WorkflowPage.RegistrationStep = Registration.GetSequenceStep(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);
            }
            else
            {
                this.WorkflowPage.RegistrationSequence += increment;
                this.WorkflowPage.RegistrationStep = Registration.GetSequenceStep(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);

                // OHPNM-14127 - Fix to show correct CPC Contact info page.
                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CPC && this.WorkflowPage.RegistrationStep == 1)
                    this.WorkflowPage.RegistrationStep = CON.SectionTypeID.CpcContactInformation;
                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.CMC && this.WorkflowPage.RegistrationStep == 1)
                    this.WorkflowPage.RegistrationStep = CON.SectionTypeID.CMCContactInformation;
            }

        }
        else
        {
            this.WorkflowPage.RegistrationSequence -= increment;
            this.WorkflowPage.RegistrationStep = Registration.GetSequenceStep(this.WorkflowPage.RegistrationNodes, this.WorkflowPage.RegistrationSequence);
        }
        //this.WorkflowPage.RegistrationStep += increment;
        this.WorkflowPage.BumpStep = this.WorkflowPage.RegistrationStep;

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

    private void MarkReasons()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
        DataSet ds = psc.SelectREG_ERRORcustom(this.WorkflowPage.RegistrationId, pageTypeId, this.WorkflowPage.RegistrationStep,
            false, true);
        if (ds.Tables.Count > 0)
        {
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

    private bool HasMMISError()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
        DataSet ds = psc.SelectREG_ERRORcustom(this.WorkflowPage.RegistrationId, pageTypeId, this.WorkflowPage.RegistrationStep,
            false, true);
        bool hasMMISError = false;

        if (ds.Tables.Count > 1)
        {
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                if (Helper.GetInt("ERROR_CATEGORY_TYPE_ID", row) == CON.ErrorCategoryType.MMISError)
                {
                    hasMMISError = true;
                    break;
                }
            }
        }
        return hasMMISError;
    }

    protected void btnTakeAction_Click(object sender, EventArgs e)
    {
        if (this.mltTakeAction.ActiveViewIndex == 1)
        {
            lblMpeTitle.Text = "Electronic Signature";
            bool isICFIIDProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.RegistrationDIDDReferral;
            //Admin and contract start input fields are only applicable for ICF/IID contracts
            this.pnlAdminInfo.Visible = valContractStartReqd.Enabled = valAdminReqd.Enabled = isICFIIDProvider;
            //valContractStartReqd.Enabled = isICFIIDProvider;

            if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationDIDDReferral)
            { // if the workflow is DIDD based, go get the contract dates and populate

                this.pnlAdminInfo.Visible = true;
                this.pnlAdminName.Visible = false;
                valContractStartReqd.Enabled = true;
                //Get the current contract information:  signatures, referral id, if applicable, and document id, if found, of the latest rendered document.
                DataSet regData = Registration.GetCurrentContractInformation(this.WorkflowPage.RegistrationId);
                if (Helper.HasRows(regData))
                {
                    int DIDDReferralID = Helper.GetInt("DIDDReferralID", regData.Tables[0].Rows[0]);
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    DataSet diddReferral = psc.SelectDIDDReferral(DIDDReferralID);
                    if (!Helper.HasRows(diddReferral)) return;

                    DataRow row = diddReferral.Tables[0].Rows[0];
                    txtContractStartDate.Text = DateTime.Parse(Helper.GetString("DIDDCONTRACT_FROMDATE", row)).ToShortDateString();
                    txtContractStartDate.ReadOnly = true;
                    txtContractStartDate.Enabled = false;
                    txtContractEndDate.Text = DateTime.Parse(Helper.GetString("DIDDCONTRACT_TODATE", row)).ToShortDateString();
                    txtContractEndDate.ReadOnly = true;
                    txtContractEndDate.Enabled = false;

                }

            }
        }
        else if (this.mltTakeAction.ActiveViewIndex == 3)
        {
            lblMpeTitle.Text = "Take Action - " + "Screening Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 4)
        {
            btnSavempe.ValidationGroup = "valRetroReview";
            lblMpeTitle.Text = "Take Action - " + "RetroEffective Date Review";
            LoadRetroReviewData();
        }
        else if (this.mltTakeAction.ActiveViewIndex == 5)
        {
            btnSavempe.ValidationGroup = "valRetroReview";
            lblMpeTitle.Text = "Take Action - " + "Group Member RetroEffective Date Review";
            LoadGroupMemberRetroReviewData();
        }
        else if (this.mltTakeAction.ActiveViewIndex == 6)
        {
            btnSavempe.ValidationGroup = "valFinancialReviewComplete";
            lblMpeTitle.Text = "Take Action - " + "Financial Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 7)
        {
            btnSavempe.ValidationGroup = "valApplicationDisposition";
            lblMpeTitle.Text = "Take Action - " + "Application Disposition";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 8)
        {
            btnSavempe.ValidationGroup = "valDHCFRecommendation";
            lblMpeTitle.Text = "Take Action - " + "DHCF Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 9)
        {
            btnSavempe.ValidationGroup = "valDHCFApplicationFeeRecommendation";
            lblMpeTitle.Text = "Take Action - " + "DHCF Application Fee Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 10)
        {
            btnSavempe.ValidationGroup = "valStateApplicationFeeRecommendation";
            lblMpeTitle.Text = "Take Action - " + "State Application Fee Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 11)
        {
            btnSavempe.ValidationGroup = "valDBHRecommendation";
            lblMpeTitle.Text = "Take Action - " + "DBH Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 12)
        {
            btnSavempe.ValidationGroup = "valOperatorTerminate";
            lblMpeTitle.Text = "Take Action - " + "Termination";
            Separator1.Visible = false;
        }
        else if (this.mltTakeAction.ActiveViewIndex == 13)
        {
            btnSavempe.ValidationGroup = "valDDSRecommendation";
            lblMpeTitle.Text = "Take Action - " + "DDS Review";
        }
        else if (this.mltTakeAction.ActiveViewIndex == 14)
        {
            btnSavempe.ValidationGroup = "valCredScreeningComplete";
            lblMpeTitle.Text = "Take Action - Credentialing";
            lblVerifiedby.Text = HttpContext.Current.User.Identity.Name;
        }
        else
        {
            lblMpeTitle.Text = "Take Action - " + Registration.GetStepText(this.WorkflowPage.RegistrationStep);
            rblPSReview.SelectedIndex = -1;
            chkScreeningComplete.Checked = false;
            pnlPSReview.Attributes.Add("style", "display:none;");
            MarkReasons();
            ValidateNPI();
            txtComments.Text = txtInternalComments.Text = string.Empty;
            txtScreeningComments.Text = string.Empty;
        }
        mpe.Show();
    }

    private void AddNPIError(string errMsg, ref bool isGood)
    {
        valAlert.Visible = true;
        valAlert.Text = errMsg;
    }

    private void ValidateNPI()
    {
        string errorMessage = string.Empty; ;
        if (!Registration.IsValidNppesNpi(this.WorkflowPage.RegistrationId, ref errorMessage)
            && !((Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.FAOperator))
                    || (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.FAOperatorII)))
            )
        {
            bool dummy = true; //don't prevent validation

            //AddNPIError(errorMessage, ref dummy);
            AddError(errorMessage);
        }
    }


    private void AddError(string errMsg, ref bool isGood, string valGroup = "valTakeAction")
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = valGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }

    private bool ValidateProviderServicesReview()
    {
        bool isGood = true;
        // Approve and Return to Provider options
        if (ValidateDataPSEvent != null)
        {
            string errMsg = ValidateDataPSEvent(this.WorkflowPage.RegistrationStep);
            if (!string.IsNullOrEmpty(errMsg))
            {
                // Cannot be a PECOS error and Return to Provider
                if (!(errMsg.ToUpper().IndexOf("PECOS") != -1 && rblPSReview.SelectedIndex == 1))
                    AddError(errMsg, ref isGood);
            }
        }

        if (rblPSReview.SelectedIndex == -1) AddError("Select Approve or Return to Provider", ref isGood);
        else if (rblPSReview.SelectedIndex == 0)
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Identification)
            {
                // If approving you must have entered the Effective Date
                DataRow dr = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
                //change_effective_date will be filled out during before promote_to_active workflow
                /*if (dr != null)
                {
                    if (string.IsNullOrEmpty(Helper.GetString("CHANGE_EFFECTIVE_DATE", dr)))
                    {
                        AddError("Enter Effective Date (Organization Information)", ref isGood);
                    }
                }*/
                // If ownership change and approving you must clear the Medicaid Id
                //if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationUpdateOwner)
                //{
                //    DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "SERVICE_LOCATION");
                //    if (Helper.HasRows(ds))
                //    {
                //        if (!string.IsNullOrEmpty(Helper.GetString("MEDICAID_ID", ds.Tables[0].Rows[0])))
                //        {
                //            AddError("OWNERSHIP CHANGE - You must clear the Medicaid ID", ref isGood);
                //        }
                //    }
                //}
            }
            else if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
            {
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "FAOperator1") && (rblPSReview.SelectedIndex == 0))
                {
                    if (psc.CheckAttestToFeePaymentRequired(this.WorkflowPage.RegistrationId))
                    {
                        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_FEE_INFORMATION");
                        if (!Helper.HasRows(ds)) AddError("* At least one fee record is required when specialty is ICF/IDD.", ref isGood);
                    }
                }
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, "FAOperator2") && (rblPSReview.SelectedIndex == 0))
                {
                    DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "ACH_EDISON");
                    if (!Helper.HasRows(ds)) AddError("* Vendor Information is required", ref isGood);
                }
            }
            else if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Certification)
            {
                DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CERTIFICATION");
                if (Helper.HasRows(ds))
                {
                    DataSet dsDocs = psc.SelectRegDocuments(this.WorkflowPage.RegistrationId,
                        CON.RegistrationPageType.Certification, string.Empty, string.Empty, null);
                    if (ds.Tables[0].Rows.Count > dsDocs.Tables[0].Rows.Count)
                        AddError("* Please upload at least one C&T form for each Certification", ref isGood);
                }
            }
        }
        else if (rblPSReview.SelectedIndex == 1)
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
            if (!fnd) AddError("Please select at least one Return to Provider reason", ref isGood);
        }

        return isGood;
    }

    private bool ValidateScreeningComplete()
    {
        bool isGood = true;

        // Screening Complete Action
        if (!chkScreeningComplete.Checked) AddError("Please check the \"Screening Complete\" box to complete", ref isGood, "valTakeAction");

        if ((this.Page as WorkflowPage).ActiveScreeningID == -1)
        {
            //fatal error
            AddError("ActiveScreeningID is not set, cannot complete", ref isGood, "valTakeAction");
        }

        return isGood;
    }

    private bool ValidateCredentialingComplete()
    {
        bool isGood = true;
        if (ddlcredRiskLevel.SelectedValue == "0") AddError("Please Select a Data Rank to complete", ref isGood, "valCredScreeningComplete");

        return isGood;
    }

    private bool ValidateAppeal()
    {
        bool isGood = true;
        if (rblScreeningReview.SelectedIndex < 0)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please select confirm or override screening";
            val.ValidationGroup = "valAppealProcess";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        if (string.IsNullOrEmpty(txtAppealProcessComments.Text))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Please enter the comments";
            val.ValidationGroup = "valAppealProcess";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        return isGood;
    }


    private bool ValidateLTCReviewComplete()
    {
        bool isGood = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Specialty_Approval", parameters);
        bool isVisible = Registration.SectionIsVisible(this.WorkflowPage.RegistrationId, EntityTypeId, ProviderTypeId, DIDDReferralId, "Waiver Services", "WaiverServices");
        // Financial Review Complete Action
        if (rblApproveOrDeny.SelectedIndex < 0) AddError("Please check the \"LTC Review Complete\" box to complete", ref isGood, "valApplicationDisposition");
        if (!Helper.HasRows(ds) && rblApproveOrDeny.SelectedValue == "ApproveApplication" && isVisible) AddError("You must approve at least one waiver service on the waiver services screen to approve the application", ref isGood, "valApplicationDisposition");



        return isGood;
    }
    private bool ValidateDDSReviewComplete()
    {
        bool isGood = true;
        if (rblApproveOrDeny.SelectedIndex < 0) AddError("Please select \"Approve Application\" or \"Deny Application\" to complete.", ref isGood, "valApplicationDisposition");

        if (!DDSWaiverSelectionsReviewed(true) && rblApproveOrDeny.SelectedValue == "ApproveApplication") AddError("You must approve at least one waiver service on the waiver services screen to approve the application", ref isGood, "valApplicationDisposition");

        return isGood;
    }

    private bool DDSWaiverSelectionsReviewed(Boolean IsOperator)
    {
        bool rtn = false;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parameters = new Dictionary<string, string>();
        DataSet ds = new DataSet();
        parameters.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Specialty_Approval", parameters);

        if (Helper.HasRows(ds))
        {
            if (Helper.HasRows(ds.Tables[0]))
            {
                DataTable dt = ds.Tables[0];
                DataRow[] rows = null;

                if (IsOperator)
                    rows = dt.Select("IsOperatorApproved = true");
                else
                    rows = dt.Select("IsReviewerApproved = true");

                if (rows.Length > 0)
                    rtn = true;
            }
        }


        return rtn;
    }

    private bool ValidateFinancialReviewComplete()
    {
        bool isGood = true;

        // Financial Review Complete Action
        if (!chkFinancialReviewComplete.Checked) AddError("Please check the \"Financial Review Complete\" box to complete", ref isGood, "valFinancialReviewComplete");



        return isGood;
    }

    private bool ValidateGroupMemberRetro()
    {
        bool isGood = true;
        int counter = 0;
        foreach (GridViewRow row in grdMemberRetroEffectiveDateReview.Rows)
        {
            if (((DropDownList)row.FindControl("ddlDecision")).SelectedIndex > 0 && !String.IsNullOrEmpty(((TextBox)row.FindControl("txtApprovedEffectiveDate")).Text))
            {
                counter++;
            }
            else
            {
                AddError("Enter all the fields for " + row.Cells[0].Text, ref isGood, "valGroupReviewRetro");
            }

        }

        return isGood;
    }
    private bool ValidateRetro()
    {
        bool isGood = true;

        // Screening Complete Action
        if (rblRetroReviewDecision.SelectedIndex == -1) AddError("A Decision is required.", ref isGood, "valRetro");
        if (string.IsNullOrEmpty(this.txtRetroApprovedEffectiveDate.Text.Trim())) AddError("A valid Approved Effective Date is required (mm/dd/yyyy)..", ref isGood, "valRetro");
        if (string.IsNullOrEmpty(this.txtRetroComments.Text.Trim())) AddError("Comments are required.", ref isGood, "valRetro");

        return isGood;
    }
    private bool ValidateDHCFReviewComplete()
    {
        bool isGood = true;

        // DHCF Review Complete Action
        if (rblDHCFRecommendation.SelectedIndex < 0) AddError("Please select the \"Recommendation \" to complete", ref isGood, "valDHCFRecommendation");

        if (rblDHCFRecommendation.SelectedValue == "DenyApplication")
        {
            if (string.IsNullOrEmpty(txtDHCFRecommendation.Text))
            {
                AddError("Comments are required for Recommend Denial.", ref isGood, "valDHCFRecommendation");
            }
            // commented out based on DCPDMS-2204 -- RBM 2/22/2018
            //if (ucUploadDocumentDHCF.NumberOfDocuments() < 1)
            //{
            //    AddError("Please upload documents for denial.", ref isGood, "valDHCFRecommendation");
            //}
        }
        if (rblDHCFRecommendation.SelectedValue == "TerminateApplication")
        {
            if (string.IsNullOrEmpty(txtDHCFRecommendation.Text))
            {
                AddError("Comments are required for Recommend Termination.", ref isGood, "valDHCFRecommendation");
            }
            // commented out based on DCPDMS-2204 -- RBM 3/1/2018
            //if (ucUploadDocumentDHCF.NumberOfDocuments() < 1)
            //{
            //    AddError("Please upload documents for termination.", ref isGood, "valDHCFRecommendation");
            //}
        }

        return isGood;
    }
    private bool ValidateDHCFApplicationFeeReviewComplete()
    {
        bool isGood = true;

        // DHCF Application Fee Review Complete Action
        if (rblDHCFApplicationFeeRecommendation.SelectedIndex < 0) AddError("Please select the \"Recommendation \" to complete", ref isGood, "valDHCFApplicationFeeRecommendation");



        return isGood;
    }
    private bool ValidateStateApplicationFeeReviewComplete()
    {
        bool isGood = true;

        // State Application Fee Review Complete Action
        if (rblStateApplicationFeeRecommendation.SelectedIndex < 0) AddError("Please select the \"Recommendation \" to complete", ref isGood, "valStateApplicationFeeRecommendation");



        return isGood;
    }

    private bool ValidateDBHReviewComplete()
    {
        bool isGood = true;

        // DBH Review Complete Action
        if (rblDBHRecommendation.SelectedIndex < 0) AddError("Please select the \"Recommendation \" to complete", ref isGood, "valDBHRecommendation");

        if (rblDBHRecommendation.SelectedValue == "DenyApplication" && ucUploadDocumentDBHReview.NumberOfDocuments() < 1)
        {
            AddError("Please upload documents for denial.", ref isGood, "valDBHRecommendation");
        }


        if (rblDBHRecommendation.SelectedValue == "ApproveApplication" && ucUploadDocumentDBHReview.NumberOfDocuments() < 1)
        {
            AddError("Please upload documents for DBH Review.", ref isGood, "valDBHRecommendation");
        }
        return isGood;
    }

    private bool ValidateDDSReviewerReviewComplete()
    {
        bool isGood = true;

        // DBH Review Complete Action
        if (rblDDSRecommendation.SelectedIndex < 0) AddError("Please select the \"Recommendation \" to complete", ref isGood, "valDDSRecommendation");

        // Waiver services must be reviewed. Passing False since this method is called for Reviewer
        if (!DDSWaiverSelectionsReviewed(false) && rblDDSRecommendation.SelectedValue == "ApproveApplication")
        {
            AddError("You must approve at least one waiver service on the waiver services screen to approve the application", ref isGood, "valDDSRecommendation");
        }

        return isGood;
    }

    private bool ValidateOperatorTerminationComplete()
    {
        bool isGood = true;

        if (string.IsNullOrEmpty(txtTermDate.Text.Trim())) AddError("Termination Effective Date is required.", ref isGood, "valOperatorTerminate");
        if (ddlStatus.SelectedIndex == 0) AddError("New Enrollment Status is required.", ref isGood, "valOperatorTerminate");
        if (string.IsNullOrEmpty(txtTermComments.Text)) AddError("Comments are required.", ref isGood, "valOperatorTerminate");

        return isGood;
    }

    private bool ValidateMpe()
    {
        bool isGood = true;

        if (mltTakeAction.ActiveViewIndex == 0)
        {
            isGood = this.ValidateProviderServicesReview();
        }
        else if (mltTakeAction.ActiveViewIndex == 2)
        {
            isGood = this.ValidateScreeningComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 3)
        {
            isGood = this.ValidateAppeal();
        }
        else if (mltTakeAction.ActiveViewIndex == 4)
        {
            isGood = this.ValidateRetro();
        }
        else if (mltTakeAction.ActiveViewIndex == 5)
        {
            isGood = this.ValidateGroupMemberRetro();
        }
        else if (mltTakeAction.ActiveViewIndex == 6)
        {
            isGood = this.ValidateFinancialReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 7)
        {
            if (Helper.IsUserInLTCWorkerRole(HttpContext.Current.User.Identity.Name) &&
                this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.LTCReview)
            {
                isGood = this.ValidateLTCReviewComplete();
            }
            else if (Helper.IsUserInDDSOperatorRole(HttpContext.Current.User.Identity.Name) &&
    this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DDSReview)
            {
                isGood = this.ValidateDDSReviewComplete();
            }
            else if (Helper.IsUserInStateAdminRole(HttpContext.Current.User.Identity.Name) ||
                (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DBHProviderReview &&
                 Helper.IsUserInDBHOperatorRole(HttpContext.Current.User.Identity.Name)))
            {
                isGood = this.ValidateApplicationDisposition();
            }
        }
        else if (mltTakeAction.ActiveViewIndex == 8)
        {
            isGood = this.ValidateDHCFReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 9)
        {
            isGood = this.ValidateDHCFApplicationFeeReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 10)
        {
            isGood = this.ValidateStateApplicationFeeReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 11)
        {
            isGood = this.ValidateDBHReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 12)
        {
            isGood = ValidateOperatorTerminationComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 13)
        {
            isGood = this.ValidateDDSReviewerReviewComplete();
        }
        else if (mltTakeAction.ActiveViewIndex == 14)
        {
            isGood = this.ValidateCredentialingComplete();
        }
        else
        {
            isGood = this.ValidateContractSignature();
        }

        return isGood;
    }

    private bool ValidateApplicationDisposition()
    {
        bool isValid = true;
        string currentTaskName = this.WorkflowPage.CurrentTaskName;
        if (rblApproveOrDeny.SelectedIndex < 0) AddError("Please select \"Approve Application\" or \"Deny Application\" to complete.", ref isValid, "valApplicationDisposition");
        if (currentTaskName == CON.RegistrationTaskName.PendingApproval)
        {
            if ((rblApproveOrDeny.SelectedValue == "DenyApplication" || rblApproveOrDeny.SelectedValue == "TerminateApplication") && string.IsNullOrEmpty(txtApplicationDispositionComments.Text))
            {
                isValid = false;
                AddError("Comments is required.", ref isValid, "valApplicationDisposition");
            }
        }
        else if (currentTaskName == CON.RegistrationTaskName.PendingDenial || currentTaskName == CON.RegistrationTaskName.PendingTerminate)
        {
            if ((rblApproveOrDeny.SelectedValue == "ApproveApplication") && string.IsNullOrEmpty(txtApplicationDispositionComments.Text))
            {
                isValid = false;
                AddError("Comments is required.", ref isValid, "valApplicationDisposition");
            }
        }
        else if (currentTaskName == CON.RegistrationTaskName.DBHProviderReview)
        {
            //if (ucUploadDocumentDBH.Visible && ucUploadDocumentDBH.NumberOfDocuments() < 1)
            //{
            //    isValid = false;
            //    AddError("Please upload a DBH review document.", ref isValid, "valApplicationDisposition");
            //}
        }
        return isValid;
    }

    private bool ValidateContractSignature()
    {
        bool isValid = true;

        bool isICFIIDProvider = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.WF_WorkflowID != CON.WorkflowType.RegistrationDIDDReferral;
        if (!chkSignature.Checked)
        {
            AddError("Please check the \"Sign Contract\" box to complete.", ref isValid, "valSignature");
            isValid = false;
        }

        if (isICFIIDProvider)
        {
            if (string.IsNullOrEmpty(this.txtAdministrator.Text.Trim()))
            {
                AddError("Administrator Name is required.", ref isValid, "valSignature");
                isValid = false;
            }
        }

        DateTime? contractStartDate = null;
        DateTime? contractEndDate = null;

        if (!Helper.IsValidDate(this.txtContractStartDate.Text.Trim(), true))
        {
            AddError("A valid Contract Start Date (mm/dd/yyyy) is required.", ref isValid, "valSignature");
            isValid = false;
        }
        else if (!string.IsNullOrEmpty(this.txtContractStartDate.Text.Trim()))
        {
            contractStartDate = Convert.ToDateTime(txtContractStartDate.Text.Trim());
        }

        if (!Helper.IsValidDate(this.txtContractEndDate.Text.Trim(), true))
        {
            AddError("A valid Contract End Date (mm/dd/yyyy) is required.", ref isValid, "valSignature");
            isValid = false;
        }
        else if (!string.IsNullOrEmpty(this.txtContractEndDate.Text.Trim()))
        {
            contractEndDate = Convert.ToDateTime(txtContractEndDate.Text.Trim());
        }

        if (!isValid)
            return isValid;

        if (contractStartDate.HasValue && contractEndDate.HasValue)
        {
            if (contractStartDate.Value > contractEndDate.Value)
            {
                AddError("Contract End Date may not be prior to Contract Start Date.", ref isValid, "valSignature");
                isValid = false;
            }
        }
        return isValid;
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

    protected void btnSavempe_Click(object sender, EventArgs e)
    {

        if (!ValidateMpe())
        {
            btnSavempe.Enabled = true;
            mpe.Show();
            return;
        }

        if (mltTakeAction.ActiveViewIndex == 3)
        {
            SaveAppeal();
            mpe.Hide();
            Refresh(true);
        }

        if (SaveDataPSEvent != null)
        {
            int action = rblPSReview.SelectedIndex == 0 ? CON.RegistrationProviderServicesStatusTypeId.Approved : CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider;
            SaveDataPSEvent(this.WorkflowPage.RegistrationStep, action);
        }

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        if (mltTakeAction.ActiveViewIndex == 0)
        {
            // Approve and Return to Provider options
            // Mark any open reasons as closed
            int pageTypeId = Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep);
            Registration.MarkErrorsAsClosed(this.WorkflowPage.RegistrationId, pageTypeId, this.WorkflowPage.RegistrationStep);

            if (rblPSReview.SelectedIndex == 1)
            {
                // Return to Provider, save the reasons
                foreach (ListViewItem itm in lsvReturnReasons.Items)
                {
                    CheckBox chk = (CheckBox)itm.FindControl("chkReason");
                    HiddenField hdn = (HiddenField)itm.FindControl("hdnErrorTypeID");
                    if (chk != null && hdn != null)
                    {
                        if (chk.Checked) InsertError(hdn.Value);
                    }
                }
                if (!string.IsNullOrEmpty(txtComments.Text))
                {

                    int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                    if (RegPageTypeId == 0)
                        RegPageTypeId = this.WorkflowPage.RegistrationStep;
                    if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.RegistrationRejection, txtComments.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

                //SAM505
                if (chkBCIText.Checked)
                    psc.WF_SaveProcessParameter(this.WorkflowPage.WF_ProcessID, CON.ProcessParameter.IsAddBCIRTPEmail, "1");
            }
            else if (rblPSReview.SelectedIndex == 0)
            {
                if (!string.IsNullOrEmpty(txtComments.Text))
                {
                    int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                    if (RegPageTypeId == 0)
                        RegPageTypeId = this.WorkflowPage.RegistrationStep;
                    if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.NoteOnApproved, txtComments.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
            }

            if (!string.IsNullOrWhiteSpace(txtInternalComments.Text) && (rblPSReview.SelectedIndex == 0 || rblPSReview.SelectedIndex == 1))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep,
                    CON.ProviderNoteTypeId.Internal, txtInternalComments.Text.Trim(), DateTime.Now, this.WorkflowPage.WF_StepID,
                    Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }


            string _title = Title;
            if (_title.Contains("Individual Providers")) _title = "Individual Providers";
            else if (_title.Contains("Certification")) _title = "Certification";
            else if (_title.Contains("Disclosure of Ownership and Control Interest Statement")) _title = "Owner Information";

            Registration.SetSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderServicesStatusTypeId.ReturnToProvider);
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview)
            {
                Refresh(true);
            }
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
            {
                BumpNextModifiedSection(true);
            }
            else if (Helper.IsUserInAccountingRolls(HttpContext.Current.User.Identity.Name))
            {
                if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
                    BumpStep(true);                         // Advance if on Substitute W-9 form
            }
            else if (Helper.IsUserInDIDDRoles(HttpContext.Current.User.Identity.Name))
            {
                if (this.WorkflowPage.RegistrationStep != CON.RegistrationPageType.Services)
                    BumpStep(true);                         // Advance only if NOT Services Page
            }
            else if (this.WorkflowPage.RegistrationSequence != this.WorkflowPage.RegistrationNodes.Count && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ApplicationFeeReview)
                BumpStep(true);                             // Advance if not at the end
            else
                BumpStep(true);
            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 1)
        {
            // Signature option
            Dictionary<string, string> parms = new Dictionary<string, string>();
            string signedBy = Helper.GetUserRole(HttpContext.Current.User.Identity.Name);
            int contractTypeID = this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationDIDDReferral ? CON.ContractTypeID.DIDD : CON.ContractTypeID.ICF_IID;
            if (Helper.IsStringInList(signedBy, CON.UserRoleType.ProviderAdministrator) || Helper.IsStringInList(signedBy, CON.UserRoleType.ProviderAgent))
            {
                parms.Add("SUBMIT_DATE_TIME", DateTime.Now.ToString());
            }
            else if (Helper.IsStringInList(signedBy, CON.UserRoleType.DIDDCommissioner))
            {
                parms.Add("DIDD_COMMISSIONER_DATE_TIME", DateTime.Now.ToString());
            }
            else
            {
                parms.Add("PS_COMMISSIONER_DATE_TIME", DateTime.Now.ToString());
            }
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            psc.UpdateRegistration(parms); //TODO: remove this when no longer use these fields in the registration table.
            string adminName = txtAdministrator.Text.Trim();
            DateTime? contractStartDate = null;
            DateTime? contractEndDate = null;
            if (!string.IsNullOrEmpty(this.txtContractStartDate.Text))
            {
                contractStartDate = Convert.ToDateTime(txtContractStartDate.Text);
            }
            if (!string.IsNullOrEmpty(this.txtContractEndDate.Text))
            {
                contractEndDate = Convert.ToDateTime(txtContractEndDate.Text);
            }
            psc.InsertUpdateContractByContractTypeID(this.WorkflowPage.RegistrationId, contractTypeID, adminName, contractStartDate, contractEndDate, signedBy, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            Registration.SetNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderServicesStatusTypeId.Approved);
            Refresh();
        }
        else if (mltTakeAction.ActiveViewIndex == 2)
        {

            bool isScreeningComplete = true;
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerScreening)
            {
                /// Screening Pages
                psc.UpdateScreeningStatus((this.Page as WorkflowPage).OwnerScreeningID, CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                //do not update the entire node until take action performed on all owners being screened
                isScreeningComplete = Registration.ScreeningComplete(this.WorkflowPage.RegistrationId, CON.ScreeningFor.Owner);
            }
            else if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening)
            {
                /// Screening Pages
                psc.UpdateScreeningStatus((this.Page as WorkflowPage).ActiveScreeningID, CON.ScreeningStatusId.Complete, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


            }

            if (isScreeningComplete)
            {
                Registration.SetNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderServicesStatusTypeId.Approved);
            }

            if (!string.IsNullOrEmpty(txtComments.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                {
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtComments.Text.Trim(), DateTime.Now, this.WorkflowPage.WF_StepID,
                        Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
            }
            if (!string.IsNullOrEmpty(txtScreeningComments.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;

                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.ProviderScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OwnerScreening ||
                     Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.BackgroundCheck ||
                     Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation)
                {
                    if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening)
                    {
                        ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Provider Screening Complete - " + txtScreeningComments.Text,
                            personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Submitted, processID: this.WorkflowPage.WF_ProcessID);
                    }
                    else if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.OwnerScreening)
                    {

                        ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Owner Screening Complete - " + txtScreeningComments.Text,
                             personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Submitted, processID: this.WorkflowPage.WF_ProcessID);
                    }
                    else if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.BackgroundCheck)
                    {
                        ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Background Check Screening Complete - " + txtScreeningComments.Text,
                             personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.Submitted, processID: this.WorkflowPage.WF_ProcessID);
                    }
                    else
                    {
                        //TO DO : Update this code to send notes to ProviderFeedHelper.InsertProviderFeedNotes
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtScreeningComments.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }
            }

            // Set the Fingerprint page status to Approved if the Screening Complete checkbox is selected
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.BackgroundCheck || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.OrientationInformation
                || Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) == CON.RegistrationPageType.SiteVisitScreening)
            {
                if (chkScreeningComplete.Checked)
                {
                    RegistrationNode appNode = null;
                    if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
                    {
                        appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
                    }
                    else
                    {
                        if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                        {
                            appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                        }
                    }
                    psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, null, null, null, null);
                }
            }

            Refresh();
        }
        else if (mltTakeAction.ActiveViewIndex == 4)
        {
            /// Retroeffective date state review
            SaveRetroDecision();
            RegistrationNode appNode = null;
            if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
            {
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                }
            }
            psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, CON.RegistrationProviderServicesStatusTypeId.Approved, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, null, null, null, null);
            appNode.StatusId = CON.RegistrationProviderServicesStatusTypeId.Approved;
            Refresh();
        }
        else if (mltTakeAction.ActiveViewIndex == 5)
        {
            /// Retroeffective date state review
            SaveGroupMemberRetroDecision();

            Refresh();
        }
        else if (mltTakeAction.ActiveViewIndex == 6)
        {
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening)
            {
                if (chkFinancialReviewComplete.Checked)
                {
                    RegistrationNode appNode = null;
                    if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
                    {
                        appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
                    }
                    else
                    {
                        if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                        {
                            appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                        }
                    }
                    psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.FinancialReviewStatusID.Completed, null, null, null, null, null, null, null, null, null);
                }
            }

            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 7)
        {
            if ((this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.EPD && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Agreements) ||
                (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.EPD && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Identification && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.LTCReview) ||
                (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.ADHP && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Identification && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.LTCReview) ||
                (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.ADHP && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Agreements && this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.LTCReview) ||
                (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationNew && ((this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ProviderScreening && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DBHProviderReview))
                                                                                            || (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Agreements && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingApproval || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingDenial
                                                                                                            || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingTerminate)))) ||
                 this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.PeriodicDatabaseChecks ||
                 (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.Streamlined && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.Agreements))
            {
                if (rblApproveOrDeny.SelectedIndex >= 0)
                {
                    RegistrationNode appNode = null;
                    if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
                    {
                        appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
                    }
                    else
                    {
                        if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                        {
                            appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                        }
                    }
                    int ApproveOrDeny = 0;
                    if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.LTCReview)
                    {
                        ApproveOrDeny = rblApproveOrDeny.SelectedValue == "ApproveApplication" ? CON.LTCReviewStatusID.Approve : (rblApproveOrDeny.SelectedValue == "DenyApplication" ? CON.LTCReviewStatusID.Deny : CON.LTCReviewStatusID.Terminate);
                        psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, ApproveOrDeny, null, null, null, null, null, null, null, null);
                    }
                    else if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingApproval || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.PendingTerminate)
                    {
                        ApproveOrDeny = rblApproveOrDeny.SelectedValue == "ApproveApplication" ? CON.StateReviewStatusID.Approve : (rblApproveOrDeny.SelectedValue == "DenyApplication" ? CON.StateReviewStatusID.Deny : CON.StateReviewStatusID.Terminate);
                        psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, ApproveOrDeny, null, null, null, null, null, null, null);
                    }
                    else if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DBHProviderReview)
                    {
                        ApproveOrDeny = rblApproveOrDeny.SelectedValue == "ApproveApplication" ? CON.DBHReviewStatusID.Approve : (rblApproveOrDeny.SelectedValue == "DenyApplication" ? CON.DBHReviewStatusID.Deny : CON.DBHReviewStatusID.Terminate);
                        psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, ApproveOrDeny, null, null, null, null, null, null);
                    }

                }
            }
            else if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.RegistrationNew)
            {
                RegistrationNode appNode = null;
                if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
                }
                else
                {
                    if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                    {
                        appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                    }
                }
                int ApproveOrDeny = 0;
                if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DDSReview)
                {
                    ApproveOrDeny = rblApproveOrDeny.SelectedValue == "ApproveApplication" ? CON.LTCReviewStatusID.Approve : (rblApproveOrDeny.SelectedValue == "DenyApplication" ? CON.LTCReviewStatusID.Deny : CON.LTCReviewStatusID.Terminate);
                    psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, ApproveOrDeny, null, null, null, null, null);
                }
            }
            if (!string.IsNullOrEmpty(txtApplicationDispositionComments.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                {
                    if (rblApproveOrDeny.SelectedValue == "ApproveApplication")
                    {
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.NoteOnApproved, txtApplicationDispositionComments.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                    else
                    {
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.RegistrationRejection, txtApplicationDispositionComments.Text.Trim(),
        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }
                else
                {
                    if (rblApproveOrDeny.SelectedValue == "ApproveApplication")
                    {
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.NoteOnApproved, txtApplicationDispositionComments.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                    else
                    {
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.RegistrationRejection, txtApplicationDispositionComments.Text.Trim(),
        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    }
                }
            }
            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 8)
        {

            RegistrationNode appNode = null;
            if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
            {
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                }
            }
            int ApproveOrDeny = 0;
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ReviewApproval || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ReviewDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ReviewTermination || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DenialTerminationNotification)
            {
                ApproveOrDeny = rblDHCFRecommendation.SelectedValue == "ApproveApplication" ? CON.DHCFReviewStatusID.Approve : (rblDHCFRecommendation.SelectedValue == "DenyApplication" ? CON.DHCFReviewStatusID.Deny : CON.DHCFReviewStatusID.Terminate);
                psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, ApproveOrDeny, null, null, null, null);
            }

            if (!string.IsNullOrEmpty(txtDHCFRecommendation.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                {
                    if (ApproveOrDeny == CON.DHCFReviewStatusID.Terminate)
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Termination, txtDHCFRecommendation.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    else
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtDHCFRecommendation.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                else
                {
                    if (ApproveOrDeny == CON.DHCFReviewStatusID.Terminate)
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Termination, txtDHCFRecommendation.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    else
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtDHCFRecommendation.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }

            }

            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 9 || mltTakeAction.ActiveViewIndex == 10)
        {

            RegistrationNode appNode = null;
            if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
            {
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                }
            }
            if (this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.ApplicationFee)
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            int ApproveOrDeny = 0;
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ReviewApplicationFee)
            {
                ApproveOrDeny = rblDHCFApplicationFeeRecommendation.SelectedValue == "ApproveApplication" ? CON.DHCFReviewStatusID.Approve : CON.DHCFReviewStatusID.Deny;
                psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, ApproveOrDeny, null, null, null);
            }

            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeDenialReview)
            {
                ApproveOrDeny = rblStateApplicationFeeRecommendation.SelectedValue == "ApproveApplication" ? CON.DHCFReviewStatusID.Approve : CON.DHCFReviewStatusID.Deny;
                psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, null, ApproveOrDeny, null, null);
            }

            if (!string.IsNullOrEmpty(txtDHCFRecommendation.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtDHCFRecommendation.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                else
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtDHCFRecommendation.Text.Trim(),
    DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            }
            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 11)
        {
            RegistrationNode appNode = null;
            if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
            {
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                }
            }

            int ApproveOrDeny = 0;
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DBHReviewerReview)
            {
                ApproveOrDeny = rblDBHRecommendation.SelectedValue == "ApproveApplication" ? CON.DBHReviewStatusID.Approve : (rblDBHRecommendation.SelectedValue == "DenyApplication" ? CON.DBHReviewStatusID.Deny : CON.DBHReviewStatusID.Terminate);
                psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, null, null, ApproveOrDeny, null);
            }

            if (!string.IsNullOrEmpty(txtDBHRecommendation.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtDBHRecommendation.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                else
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtDBHRecommendation.Text.Trim(),
    DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }
            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 12)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("TERM_DATE", txtTermDate.Text);
            parms.Add("ENROLLMENT_STATUS_CODE", ddlStatus.SelectedValue);

            psc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDERCustom", parms);

            if (!string.IsNullOrEmpty(txtTermComments.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtTermComments.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                else
                    psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtTermComments.Text.Trim(),
     DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            }

            //psc.DisenrollProvider(this.WorkflowPage.RegistrationId, DateTime.Parse(txtTermDate.Text), /*txtTermComments.Text*/, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name), true, ddlStatus.SelectedValue);
            Refresh(true);
        }
        else if (mltTakeAction.ActiveViewIndex == 13)
        {
            RegistrationNode appNode = null;
            if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep))
            {
                appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep];
            }
            else
            {
                if (this.WorkflowPage.RegistrationNodes.Count >= 1)
                {
                    appNode = this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationNodes.First().Key];
                }
            }
            int ApproveOrDeny = 0;
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.DDSReviewerReview)
            {
                ApproveOrDeny = rblDDSRecommendation.SelectedValue == "ApproveApplication" ? CON.DDSReviewStatusID.Approve : (rblDDSRecommendation.SelectedValue == "DenyApplication" ? CON.DDSReviewStatusID.Deny : CON.DDSReviewStatusID.Terminate);
                psc.SaveRegistrationPageStatus(this.WorkflowPage.RegistrationId, appNode.Step, null, null, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, null, null, null, null, null, null, null, ApproveOrDeny);
            }

            if (!string.IsNullOrEmpty(txtDDSRecommendation.Text))
            {
                int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
                if (RegPageTypeId == 0)
                    RegPageTypeId = this.WorkflowPage.RegistrationStep;
                if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
                    Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)

                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.Internal, txtDDSRecommendation.Text.Trim(),
                            DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    else
                        psc.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.Internal, txtDDSRecommendation.Text.Trim(),
                        DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                }
                Refresh(true);
            }
        else if (mltTakeAction.ActiveViewIndex == 14)   //Credentialing
            {
                psc.UpdateCredentialRisk((this.Page as WorkflowPage).ActiveScreeningID, Convert.ToInt32(ddlcredRiskLevel.SelectedValue), Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                Refresh();
            }
            else if (mltTakeAction.ActiveViewIndex == 15)
            {

                Refresh();
            }
        }

    private void SaveAppeal()
    {
        PDMSService.PDMSServiceClient psc1 = new PDMSService.PDMSServiceClient();
        DataSet ds = psc1.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL");
        if (Helper.HasRows(ds))
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("REG_APPEAL_ID", Helper.GetString("REG_APPEAL_ID", ds.Tables[0].Rows[0]));
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (rblScreeningReview.SelectedValue == "2")
            {
                parms.Add("STATE_REVIEW_SCREENING_STATUS_ID", "2");
            }
            else if (rblScreeningReview.SelectedValue == "1")
            {
                //show processappeal popup
                parms.Add("STATE_REVIEW_SCREENING_STATUS_ID", "1");
                //ShowPopup(ucProcessAppeal, "Process Appeal", "", 0, 0);

            }

            /*if (rblScreeningReview.SelectedValue == "2")
            {*/
            parms.Add("COMMENTS", txtAppealProcessComments.Text);


            /*}*/
            psc1.UpdateRegistrationDataTable("APPEAL", parms);
            //ShowPopup(ucProcessAppeal, "Process Appeal", "", 0, 0);
            //ucProcessAppeal.PageLoad();
        }
        else
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            if (rblScreeningReview.SelectedValue == CON.StateReviewScreeningStatus.OverrideScreening.ToString())
            {
                parms.Add("STATE_REVIEW_SCREENING_STATUS_ID", CON.StateReviewScreeningStatus.OverrideScreening.ToString());
            }
            else if (rblScreeningReview.SelectedValue == CON.StateReviewScreeningStatus.Confirm.ToString())
            {
                //show processappeal popup
                parms.Add("STATE_REVIEW_SCREENING_STATUS_ID", CON.StateReviewScreeningStatus.Confirm.ToString());


            }

            if (rblScreeningReview.SelectedValue == CON.StateReviewScreeningStatus.Confirm.ToString())
            {
                //
                parms.Add("COMMENTS", txtAppealProcessComments.Text);
            }
            psc1.InsertRegistrationDataTable("APPEAL", parms);
            //ShowPopup(ucProcessAppeal, "Process Appeal", "", 0, 0);
            //ucProcessAppeal.PageLoad();
        }
        /*if (!string.IsNullOrEmpty(txtScreeningComments.Text))
        {*/
        int RegPageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (RegPageTypeId == 0)
            RegPageTypeId = this.WorkflowPage.RegistrationStep;
        if (Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.ProviderScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OwnerScreening &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.SiteVisitScreening && Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.BackgroundCheck &&
Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep) != CON.RegistrationPageType.OrientationInformation)
            psc1.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, this.WorkflowPage.RegistrationStep, CON.ProviderNoteTypeId.RegistrationRejection, txtAppealProcessComments.Text.Trim(),
                             DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        else
            psc1.InsertRegProviderNote(this.WorkflowPage.RegistrationId, RegPageTypeId, 0, CON.ProviderNoteTypeId.RegistrationRejection, txtAppealProcessComments.Text.Trim(),
                 DateTime.Now, this.WorkflowPage.WF_StepID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        /*}*/

    }

    private void ShowPopup(BasePopupControl ctl, string title, string valGroup, int index, int viewIndex)
    {
        lblMpeTitle1.Text = title;
        DataRow dr = null;
        if (index >= 0 && ctl.DataList.Rows.Count > 0) dr = ctl.DataList.Rows[index];
        ctl.LoadData(dr);
        mltPopup.ActiveViewIndex = viewIndex;
        mpe1.Show();
    }


    private void AddError(string errMsg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valProviderInfoHeader";
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = this.Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valProviderInfoHeader") && v.ErrorMessage == errMsg)
                    return;
            }
            catch
            {
                continue;
            }
        }
        this.Page.Validators.Add(val);
    }

    public void SetTakeActionVisibility(bool isVisible)
    {
        IsTakeActionVisible = isVisible;
    }

    protected void btnProcessAppeal_Click(object sender, EventArgs e)
    {

        ShowPopup(ucProcessAppeal, "Process Appeal", "", 0, 0);
        ucProcessAppeal.LoadControls();
    }
    protected void btnSaveProcessAppeal_Click(object sender, EventArgs e)
    {

        if (!ucProcessAppeal.ValidateData())
        {
            mpe1.Show();
            return;
        }


        ucProcessAppeal.SaveData();

    }
    protected void grdMemberRetroEffectiveDateReview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            /*var row = grdMemberRetroEffectiveDateReview.Rows[e.Row.RowIndex];
            DropDownList ddldecision = (DropDownList)row.FindControl("ddlDecision");
            ListItemCollection lst = new ListItemCollection();
            //ddldecision.Items = new ListItemCollection();
            lst.Insert(1, new ListItem(CON.RetroEfectiveDateOptions.ApproveProviderRequest, CON.RetroEfectiveDateOptions.ApproveProviderRequest));
            lst.Insert(2, new ListItem(CON.RetroEfectiveDateOptions.UseSubmissionDate, CON.RetroEfectiveDateOptions.UseSubmissionDate));
            lst.Insert(3, new ListItem(CON.RetroEfectiveDateOptions.OverrideEffectiveDate, CON.RetroEfectiveDateOptions.OverrideEffectiveDate));
            ddldecision.DataSource = lst;
            ddldecision.DataBind();*/
        }
    }
    protected void grdMemberRetroEffectiveDateReview_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void ddlDecision_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl = (DropDownList)sender;
        GridViewRow row = ((GridViewRow)ddl.Parent.Parent);
        TextBox txtApprovedEffectiveDate = (TextBox)row.FindControl("txtApprovedEffectiveDate");
        if (txtApprovedEffectiveDate != null)
        {
            if (ddl.SelectedItem.Text == CON.RetroEfectiveDateOptions.ApproveProviderRequest)
            {
                txtApprovedEffectiveDate.Enabled = true;
                txtApprovedEffectiveDate.Text = ((DateTime)grdMemberRetroEffectiveDateReview.DataKeys[row.RowIndex]["START_DATE"]).ToShortDateString();
                txtApprovedEffectiveDate.Enabled = false;
                /*int selectedidx = grdMemberRetroEffectiveDateReview.SelectedIndex;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Enabled = true;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Text = grdMemberRetroEffectiveDateReview.DataKeys[selectedidx]["START_DATE"].ToString();
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Enabled = false;*/
            }
            else if (ddl.SelectedItem.Text == CON.RetroEfectiveDateOptions.UseSubmissionDate)
            {
                txtApprovedEffectiveDate.Enabled = true;
                txtApprovedEffectiveDate.Text = ((DateTime)grdMemberRetroEffectiveDateReview.DataKeys[row.RowIndex]["SUBMIT_DATE_TIME"]).ToShortDateString();
                txtApprovedEffectiveDate.Enabled = false;
                /*int selectedidx = grdMemberRetroEffectiveDateReview.SelectedIndex;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Enabled = true;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Text = grdMemberRetroEffectiveDateReview.DataKeys[selectedidx]["SUBMIT_DATE_TIME"].ToString();
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Enabled = false;*/
            }
            else if (ddl.SelectedItem.Text == CON.RetroEfectiveDateOptions.OverrideEffectiveDate)
            {
                txtApprovedEffectiveDate.Enabled = true;
                txtApprovedEffectiveDate.Text = "";
                /*int selectedidx = grdMemberRetroEffectiveDateReview.SelectedIndex;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Enabled = true;
                ((TextBox)grdMemberRetroEffectiveDateReview.Rows[selectedidx].FindControl("txtApprovedEffectiveDate")).Text = "";*/

            }
        }
        upMemberRetro.Update();
    }
    protected void grdMemberRetroEffectiveDateReview_RowEditing(object sender, GridViewEditEventArgs e)
    {

    }

    string ucUploadDocument_RenameFileMethod(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(dir + rtn))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + idx.ToString();
            else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }

    int ucUploadDocumentDHCF_SuccessEvent(string fileName)
    {
        int docId = -1;

        try
        {
            string saveName = fileName;
            int pos = fileName.LastIndexOf("\\");
            if (pos != -1) saveName = fileName.Substring(pos + 1);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            int? screeningActivityID = null;
            if (ucUploadDocumentDHCF.ScreeningActivityID > 0)
                screeningActivityID = ucUploadDocumentDHCF.ScreeningActivityID;



            docId = psc.InsertRegDocument(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, ucUploadDocumentDHCF.DocumentSection, ucUploadDocumentDHCF.DocumentName,
                ucUploadDocumentDHCF.DocumentDescription, saveName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), screeningActivityID, this.WorkflowPage.CurrentTaskName, this.lblTitle.Text);
            ucUploadDocumentDHCF.DocumentName = ucUploadDocumentDHCF.DocumentDescription = string.Empty;
            ucUploadDocumentDHCF.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
            mpe.Show();
        }
        catch
        {
            // Should this log to the event log?
        }
        return docId;
    }

    int ucUploadDocumentDBH_SuccessEvent(string fileName)
    {
        int docId = -1;

        try
        {
            string saveName = fileName;
            int pos = fileName.LastIndexOf("\\");
            if (pos != -1) saveName = fileName.Substring(pos + 1);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            docId = psc.InsertRegDocument(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, ucUploadDocumentDBH.DocumentSection, ucUploadDocumentDBH.DocumentName,
                ucUploadDocumentDBH.DocumentDescription, saveName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, this.lblTitle.Text);
            ucUploadDocumentDBH.DocumentName = ucUploadDocumentDBH.DocumentDescription = string.Empty;
            ucUploadDocumentDBH.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
            mpe.Show();
        }
        catch
        {
            // Should this log to the event log?
        }
        return docId;
    }

    int ucUploadDocumentDBHReview_SuccessEvent(string fileName)
    {
        int docId = -1;

        try
        {
            string saveName = fileName;
            int pos = fileName.LastIndexOf("\\");
            if (pos != -1) saveName = fileName.Substring(pos + 1);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            docId = psc.InsertRegDocument(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, ucUploadDocumentDBHReview.DocumentSection, ucUploadDocumentDBHReview.DocumentName,
                ucUploadDocumentDBHReview.DocumentDescription, saveName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, this.lblTitle.Text);
            ucUploadDocumentDBHReview.DocumentName = ucUploadDocumentDBHReview.DocumentDescription = string.Empty;
            ucUploadDocumentDBHReview.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
            mpe.Show();
        }
        catch
        {
            // Should this log to the event log?
        }
        return docId;
    }

    int ucUploadDocumentDDSReview_SuccessEvent(string fileName)
    {
        int docId = -1;

        try
        {
            string saveName = fileName;
            int pos = fileName.LastIndexOf("\\");
            if (pos != -1) saveName = fileName.Substring(pos + 1);

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

            docId = psc.InsertRegDocument(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, ucUploadDocumentDDSReview.DocumentSection, ucUploadDocumentDDSReview.DocumentName,
                ucUploadDocumentDDSReview.DocumentDescription, saveName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), null, null, this.lblTitle.Text);
            ucUploadDocumentDDSReview.DocumentName = ucUploadDocumentDDSReview.DocumentDescription = string.Empty;
            ucUploadDocumentDDSReview.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
            mpe.Show();
        }
        catch
        {
            // Should this log to the event log?
        }
        return docId;
    }

    void ucUploadDocument_FileDeleteEvent()
    {

        mpe.Show();

    }
    protected void btnSaveNext_Click(object sender, EventArgs e)
    {
        bool isValid = false;
        //Bug # 3320 & 2920
        if (Registration.IsCurrentAssignedUserForStep(Helper.GetUserId(HttpContext.Current.User.Identity.Name), this.WorkflowPage.WF_StepID)
        && (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.InformationrequiredIncreasedRiskLevel))
        {
            //OHPNM-6391-Issue 2. After green check mark, when user clicks on 'Next' instead of going to next screen system is showing 'USPS' pop up message again.
            isValid = Save();

        }
        else
        {
            isValid = true;
        }
        if (ValidateSaveNextDataEvent != null && isValid)
        {
            ValidateSaveNextDataEvent(this.WorkflowPage.RegistrationStep, ref isValid);
        }
        //{
        if (isValid)
        {
            BumpStep(true);
            Refresh(true);
        }
        else
            return;
        //}
    }

    protected void btnTakeActionApprove_Click(object sender, EventArgs e)
    {
        bool processApproval = true;
        // Bug 2210 - validate and save the practice type and the tax category for Operator Review of the W9 page.
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview &&
             Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && (this.WorkflowPage.RegistrationStep == 1 || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.NPIandMedId))
        //  (this.WorkflowPage.RegistrationStep == 36 || this.WorkflowPage.RegistrationStep == 1))
        {
            processApproval = SaveAndRefresh(3);
        }

        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderReview &&
             Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && (this.WorkflowPage.RegistrationStep == 8
             || this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Agreements))
        //  (this.WorkflowPage.RegistrationStep == 36 || this.WorkflowPage.RegistrationStep == 1))
        {
            processApproval = SaveAndRefresh();
        }

        // if there was no data on the screen to be saved, or the screen data was saved
        // successfully, then process the approval
        if (processApproval)
        {
            Registration.SetSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderServicesStatusTypeId.Approved);
            if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ApplicationFeeReview)
            {
                Refresh(true);
            }
            else
            {
                if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg && !this.WorkflowPage.IsAddODMorODAMedSvc && !this.WorkflowPage.IsReactivation)
                    BumpNextModifiedSection(true);
                else
                    BumpStep(true);

                Refresh(true);
            }
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }

    protected void btnTakeActionReject_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelChanges_Click(object sender, EventArgs e)
    {
        Refresh(true);
    }

}