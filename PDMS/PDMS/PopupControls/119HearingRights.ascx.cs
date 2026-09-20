using Corp.Core.Libraries;
using Corp.Core.Libraries.IncidentManagementService;
using MAXIMUS.Controllers.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_119HearingRights : BaseSectionControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient svc
    {
        get
        {
            if (_svc == null)
            {
                _svc = new PDMSService.PDMSServiceClient();
            }

            return _svc;
        }
    }

    private const string sectionName = "HearingRights";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox2.Show("An error occurred when loading the work queue.<br>Error: " + ex.Message + " " + ex.StackTrace, "Error");
            }
        }
    }

    protected void ddlHearingStatus_Init(object sender, EventArgs e)
    {
        DataSet dsHearingStatus = svc.SelectHearingStatus();
        if (Helper.HasRows(dsHearingStatus))
        {
            Helper.LoadDropDown(ddlHearingStatus, dsHearingStatus.Tables[0], "HEARING_STATUS_NAME", "HEARING_STATUS_ID", true);
        }
    }

    protected void ddlTerminationReasons_Init(object sender, EventArgs e)
    {
        DataSet dsTerminationReasons = svc.SelectTerminationReasons();
        if (Helper.HasRows(dsTerminationReasons))
        {
            Helper.LoadDropDown(ddlTerminationReasons, dsTerminationReasons.Tables[0], "ENROLLMENT_STATUS_REASONS_DESC", "ENROLLMENT_STATUS_REASONS_ID", true);
        }
    }

    public override bool ValidateData()
    {
        Page.Validate("val119HearingRights");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("val119HearingRights") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        bool isValid = true;
        
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofTermination)
        {
            foreach (Control ctrl in PlaceholderUploadProposedAdjudicationOrder.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                if (uploadControl.FileName == null)
                {
                    AddError("Proposed Adjudication Order Upload Document Required.", ref isValid);
                    return isValid;
                }
            }
        }
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial || ddlHearingStatus.SelectedValue.Equals(CON.HearingStatus.SettlementReached.ToString()))
        {
            foreach (Control ctrl in PlaceholderUploadSettlementAgreements.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                if (uploadControl.FileName == null)
                {
                    AddError("*Settlement Agreement Upload Document Required.", ref isValid);
                    return isValid;
                }
            }
        }
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination)
        {
            foreach (Control ctrl in PlaceholderUploadAdjudicationOrder.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                if (uploadControl.FileName == null)
                {
                    AddError("Adjudication Order Upload Document Required.", ref isValid);
                    return isValid;
                }
            }
        }
        if(this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.CSEnterTR)
        {
            if (string.IsNullOrEmpty(txtDateOfDenialTermination.Text))
            {
                AddError("Enter Date of Denial/Termination.", ref isValid);
                return isValid;
            }
            if(ddlTerminationReasons.SelectedIndex == -1)
            {
                AddError("Select a Valid Termination Reason.", ref isValid);
                return isValid;
            }
        }

        return isValid;
    }

    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "val119HearingRights";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override bool SaveData()
    {
        Page.Validate("val119HearingRights");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("val119HearingRights") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        bool isValid = true;
        foreach (Control ctrl in PlaceholderUploadProposedAdjudicationOrder.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("val119HearingRights");
        }
        foreach (Control ctrl in PlaceholderUploadSettlementAgreements.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("val119HearingRights");
        }
        foreach (Control ctrl in PlaceholderUploadAdjudicationOrder.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("val119HearingRights");
        }
        if (ValidateData())
        {
            //Insert New record
            try
            {
                bool isTerminated = false;
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());

                if (string.IsNullOrEmpty(txtDateOfProposedAdjudication.Text.Trim()))
                {
                    parms.Add("DATE_PROPOSED_ADJUDICATION_ORDER", null);
                }
                else
                {
                    parms.Add("DATE_PROPOSED_ADJUDICATION_ORDER", Convert.ToDateTime(txtDateOfProposedAdjudication.Text.Trim()).ToString());
                }
                parms.Add("PAO_RETURNED_CHECK", (chkPAOReturnedUndelivered.Checked) ? "1" : "0");
                if (string.IsNullOrEmpty(txtPAODateReturned.Text.Trim()))
                {
                    parms.Add("DATE_PAO_RETURNED", null);
                }
                else
                {
                    parms.Add("DATE_PAO_RETURNED", Convert.ToDateTime(txtPAODateReturned.Text.Trim()).ToString());
                }
                if (string.IsNullOrEmpty(txtPAODateReSent.Text.Trim()))
                {
                    parms.Add("DATE_PAO_RESENT", null);
                }
                else
                {
                    parms.Add("DATE_PAO_RESENT", Convert.ToDateTime(txtPAODateReSent.Text.Trim()).ToString());
                }
                if (ddlHearingStatus != null && ddlHearingStatus.SelectedIndex > 0)
                {
                    parms.Add("HEARING_STATUS", ddlHearingStatus.SelectedItem.Value.ToString());
                }
                else
                {
                    parms.Add("HEARING_STATUS", "0");
                }
                if (string.IsNullOrEmpty(txtDateOfHearingRequest.Text.Trim()))
                {
                    parms.Add("DATE_HEARING_REQUEST", null);
                }
                else
                {
                    parms.Add("DATE_HEARING_REQUEST", Convert.ToDateTime(txtDateOfHearingRequest.Text.Trim()).ToString());
                }
                if (string.IsNullOrEmpty(txtSettlementDate.Text.Trim()))
                {
                    parms.Add("DATE_SETTLEMENT", null);
                }
                else
                {
                    parms.Add("DATE_SETTLEMENT", Convert.ToDateTime(txtSettlementDate.Text.Trim()).ToString());
                }
                if (string.IsNullOrEmpty(txtDateOfAdjudicationOrder.Text.Trim()))
                {
                    parms.Add("DATE_ADJUDICATION_ORDER", null);
                }
                else
                {
                    parms.Add("DATE_ADJUDICATION_ORDER", Convert.ToDateTime(txtDateOfAdjudicationOrder.Text.Trim()).ToString());
                }
                if (string.IsNullOrEmpty(txtDateOfDenialTermination.Text.Trim()))
                {
                    parms.Add("DATE_TERMINATION", null);
                }
                else
                {
                    parms.Add("DATE_TERMINATION", Convert.ToDateTime(txtDateOfDenialTermination.Text.Trim()).ToString());
                }
                if (ddlTerminationReasons != null && ddlTerminationReasons.SelectedIndex > 0)
                {
                    parms.Add("TERMINATION_REASON", ddlTerminationReasons.SelectedItem.Value.ToString());
                }
                else
                {
                    parms.Add("TERMINATION_REASON", "0");
                }
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());


                if (ddlTerminationReasons != null && ddlTerminationReasons.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtDateOfDenialTermination.Text))
                {
                    isTerminated = true;
                }

                if (!string.IsNullOrEmpty(hidID.Value))
                {
                    parms.Add("REG_HEARING_RIGHTS_ID", hidID.Value.Trim());
                    svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "HEARING_RIGHTS", parms);
                }
                else
                {
                    parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                    parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "HEARING_RIGHTS", parms);

                }
                if (this.WorkflowPage.WF_WorkflowID == CON.WorkflowType.IncidentCompliance)
                {
                    DataSet dsCasenum = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_XREF");
                    string response = string.Empty;
                    foreach (DataRow dr in dsCasenum.Tables[1].Rows)
                    {
                         string requestFor = isTerminated ? "ComplianceWF" : "Termination";

                        //if (Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.E2E || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.INT02 || Helper.GetAppSetting("Environment", string.Empty) == CON.Environment.OH_UAT)
                        //{
                            UpdateProviderIncidentStatusInfo upReqInfo = new UpdateProviderIncidentStatusInfo();
                            upReqInfo.MedicaidProviderID = Helper.GetString("MEDICAID_ID", dr);
                            upReqInfo.ProviderName = Helper.GetString("PROVIDER_NAME", dr);
                            upReqInfo.ProviderStatus = isTerminated ? "Terminated" : "Elevated Screening";
                            upReqInfo.PAIUnclaimed = chkPAOReturnedUndelivered.Checked ? "True" : "False";
                            if (!string.IsNullOrEmpty(txtDateOfProposedAdjudication.Text))
                                upReqInfo.PAOMailedDate = Convert.ToDateTime(txtDateOfProposedAdjudication.Text).ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(txtDateOfHearingRequest.Text))
                                upReqInfo.HearingRequestedDate = Convert.ToDateTime(txtDateOfHearingRequest.Text).ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(txtDateOfHearingRequest.Text))
                                upReqInfo.HearingRequestDueDate = Convert.ToDateTime(txtDateOfHearingRequest.Text).AddDays(30).ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(txtSettlementDate.Text))
                                upReqInfo.SettlementReachedDate = Convert.ToDateTime(txtSettlementDate.Text).ToString("yyyy-MM-dd");
                            if (!string.IsNullOrEmpty(txtDateOfAdjudicationOrder.Text))
                                upReqInfo.AODateIssued = Convert.ToDateTime(txtDateOfAdjudicationOrder.Text).ToString("yyyy-MM-dd");

                            IncidentManagementReqRes ims = new IncidentManagementReqRes();
                            response = ims.UpdateIncidentProviderStatus(Helper.GetString("MEDICAID_ID", dr),string.Empty, this.WorkflowPage.RegistrationId, requestFor, upReqInfo);
                        //}
                        //else
                        //    response = "Success";

                        if (response == "Success" && isTerminated)
                        {
                            //Update Case Status
                            Dictionary<string, string> parms1 = new Dictionary<string, string>();
                            parms1 = new Dictionary<string, string>();
                            parms1.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                            parms1.Add("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", Helper.GetString("REG_INCIDENT_COMPLIANCE_CASE_XREF_ID", dr));
                            parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());                            
                            parms1.Add("CASE_STATUS", CON.IncidentReviewCaseStatus.Terminated.ToString());
                            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "INCIDENT_COMPLIANCE_CASE_XREF", parms1);

                            svc.UpdateIncidentAlertFlag(this.WorkflowPage.RegistrationId, DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name));

                            #region OHPNM-4561
                            Dictionary<string, string> dictParms = new Dictionary<string, string>();
                            dictParms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                            dictParms.Add("END_DATE", txtDateOfDenialTermination.Text.ToString());
                            dictParms.Add("ENROLLMENT_STATUS_CODE", CON.EnrollmentStatusTypeID.InActive.ToString());
                            dictParms.Add("ENROLLMENT_STATUS_REASONS_ID", ddlTerminationReasons.SelectedItem.Value.ToString());
                            dictParms.Add("MODIFIED_STATUS_TYPE_ID", CON.RegistrationModifiedStatusType.Changed.ToString());
                            dictParms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                            dictParms.Add("LAST_MODIFIED_USER", Guid.Parse(CON.appAdminUserId).ToString());

                            RegistrationController.UpdateRegistrationData("specialty_enroll_status", dictParms);
                            #endregion
                        }

                        if (response == "Fail")
                        {
                            AddError("Error Updating IMS", ref isValid);
                            return isValid;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("HearingRights_SaveData - " + ex.Message));
            }
        }
        return true;
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }

    protected void ddlHearingStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        EnableDisable_pnlHearingStatusHR_pnlHearingStatusSR();
    }

    private void EnableDisable_pnlHearingStatusHR_pnlHearingStatusSR()
    {
        if (ddlHearingStatus.SelectedValue.Equals(CON.HearingStatus.SettlementReached.ToString()))
        {
            pnlHearingStatusHR.Enabled = pnlHearingStatusHR.Visible = pnlHearingStatusSR.Enabled = pnlHearingStatusSR.Visible = true;
        }
        else
        {
            if (ddlHearingStatus.SelectedValue.Equals(""))
            {
                pnlHearingStatusHR.Enabled = pnlHearingStatusHR.Visible = pnlHearingStatusSR.Enabled = pnlHearingStatusSR.Visible = false;
            }
            else
            {
                pnlHearingStatusSR.Enabled = pnlHearingStatusSR.Visible = false;
                pnlHearingStatusHR.Enabled = pnlHearingStatusHR.Visible = true;
            }
        }
    }

    protected void ddlHearingStatus_DataBound(object sender, EventArgs e)
    {
        ddlHearingStatus.Items.FindByValue(CON.HearingStatus.NotRequested.ToString()).Selected = true;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        string currentTaskName = this.WorkflowPage.CurrentTaskName;
        if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            /*
            switch (currentTaskName)
            {
                case CON.ComplianceTaskName.UploadPAO:
                    pnlProposedAdjuidicationOrder.Enabled = true;
                    break;
                case CON.ComplianceTaskName.RecordDateOfMailReturn:
                    pnlProposedAdjuidicationOrder.Enabled = true;
                    break;
                case CON.ComplianceTaskName.UploadAO:
                    pnlAdjudicationOrder.Enabled = true;
                    pnlAdjudicationOrderUploadControl.Enabled = true;
                    pnlAdjudicationOrderUploadControl.Visible = true;
                    break;
                case CON.ComplianceTaskName.RecordHearingStatus:
                    pnlHearingStatus.Enabled = true;
                    break;
                case CON.ComplianceTaskName.RecordDateAOMailed:
                    pnlAdjudicationOrder.Enabled = true;
                    break;
                case CON.ComplianceTaskName.CSEnterTR:
                    pnlDenialTerminationInfo.Enabled = true;
                    break;
                default:
                    return;
            }
            */
        }
    }

    public override void LoadData(DataRow row)
    {

    }

    private void LoadData(int hearingRightsID, bool isEdit, DataRow dr)
    {

    }

    protected void ValidatePAODate(object sender, ServerValidateEventArgs e)
    {
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DateTime appSubmitDate = Helper.GetDateTime("SubmitDateTime", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(txtDateOfProposedAdjudication.Text) && !string.IsNullOrEmpty(appSubmitDate.ToString()))
            {
                DateTime enteredDate = default(DateTime);
                if (DateTime.TryParse(txtDateOfProposedAdjudication.Text, out enteredDate))
                {
                    enteredDate = Convert.ToDateTime(txtDateOfProposedAdjudication.Text);
                    if ((enteredDate < appSubmitDate && enteredDate.Date > DateTime.Now.Date))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
    }

    protected void ValidateHearingRequestDate(object sender, ServerValidateEventArgs e)
    {
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DateTime appTermDate = Helper.GetDateTime("TerminationDate", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(txtDateOfHearingRequest.Text) && !string.IsNullOrEmpty(appTermDate.ToString()))
            {
                DateTime enteredDate = default(DateTime);
                if (DateTime.TryParse(txtDateOfHearingRequest.Text, out enteredDate))
                {
                    enteredDate = Convert.ToDateTime(txtDateOfHearingRequest.Text);
                    if (!(enteredDate > appTermDate && enteredDate < DateTime.Now))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
    }

    protected void ValidateSettlementDate(object sender, ServerValidateEventArgs e)
    {
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DateTime appTermDate = Helper.GetDateTime("TerminationDate", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(txtSettlementDate.Text) && !string.IsNullOrEmpty(appTermDate.ToString()))
            {
                DateTime enteredDate = default(DateTime);
                if (DateTime.TryParse(txtSettlementDate.Text, out enteredDate))
                {
                    enteredDate = Convert.ToDateTime(txtSettlementDate.Text);
                    if (!(enteredDate > appTermDate && enteredDate < DateTime.Now))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
    }

    protected void ValidateDateOfAdjudicationOrder(object sender, ServerValidateEventArgs e)
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "HEARING_RIGHTS");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DateTime appInitialNoticeDate = Helper.GetDateTime("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(txtDateOfAdjudicationOrder.Text) && !string.IsNullOrEmpty(appInitialNoticeDate.ToString()))
            {
                DateTime enteredDate = default(DateTime);
                if (DateTime.TryParse(txtDateOfAdjudicationOrder.Text, out enteredDate))
                {
                    enteredDate = Convert.ToDateTime(txtDateOfAdjudicationOrder.Text);
                    if (!(enteredDate > appInitialNoticeDate && enteredDate < DateTime.Now))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
    }

    protected void ValidateDateOfDenialTermination(object sender, ServerValidateEventArgs e)
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "HEARING_RIGHTS");
        if (ds.Tables[0].Rows.Count > 0)
        {
            DateTime appInitialNoticeDate = Helper.GetDateTime("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(txtDateOfDenialTermination.Text) && !string.IsNullOrEmpty(appInitialNoticeDate.ToString()))
            {
                DateTime enteredDate = default(DateTime);
                if (DateTime.TryParse(txtDateOfDenialTermination.Text, out enteredDate))
                {
                    enteredDate = Convert.ToDateTime(txtDateOfDenialTermination.Text);
                    if (!(enteredDate > appInitialNoticeDate && enteredDate < DateTime.Now))
                    {
                        e.IsValid = false;
                    }
                }
            }
        }
    }

    protected void ValidateTerminationReasons(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(txtDateOfDenialTermination.Text))
        {
            e.IsValid = false;
        }
    }

    public override void LoadControlData()
    {
        LoadHearingRights();
    }

    private void LoadHearingRights()
    {
        bool isEdit = false;

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "HEARING_RIGHTS");

        hidID.Value = string.Empty;
        int hearingRightsID = 0;
        int hearingRightsDDL = 0;
        string terminationReasonDDL = string.Empty;
        if (Helper.HasRows(ds))
        {
            isEdit = true;
            DataTable dtHearingRights = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            hidID.Value = Helper.GetData("REG_HEARING_RIGHTS_ID", dtHearingRights.Rows[0]);
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0])))
            {
                txtDateOfProposedAdjudication.Text = Helper.GetDate("DATE_PROPOSED_ADJUDICATION_ORDER", ds.Tables[0].Rows[0]);
            }
            chkPAOReturnedUndelivered.Checked = Helper.GetBool("PAO_RETURNED_CHECK", ds.Tables[0].Rows[0]);
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_PAO_RETURNED", ds.Tables[0].Rows[0])))
            {
                txtPAODateReturned.Text = Helper.GetDate("DATE_PAO_RETURNED", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_PAO_RESENT", ds.Tables[0].Rows[0])))
            {
                txtPAODateReSent.Text = Helper.GetDate("DATE_PAO_RESENT", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("HEARING_STATUS", ds.Tables[0].Rows[0])))
            {
                hearingRightsDDL = Helper.GetInt("HEARING_STATUS", ds.Tables[0].Rows[0]);
                if (hearingRightsDDL > 0)
                {
                    ddlHearingStatus.SelectedValue = hearingRightsDDL.ToString();
                    
                    EnableDisable_pnlHearingStatusHR_pnlHearingStatusSR();
                }
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_HEARING_REQUEST", ds.Tables[0].Rows[0])))
            {
                txtDateOfHearingRequest.Text = Helper.GetDate("DATE_HEARING_REQUEST", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_SETTLEMENT", ds.Tables[0].Rows[0])))
            {
                txtSettlementDate.Text = Helper.GetDate("DATE_SETTLEMENT", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_ADJUDICATION_ORDER", ds.Tables[0].Rows[0])))
            {
                txtDateOfAdjudicationOrder.Text = Helper.GetDate("DATE_ADJUDICATION_ORDER", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("DATE_TERMINATION", ds.Tables[0].Rows[0])))
            {
                txtDateOfDenialTermination.Text = Helper.GetDate("DATE_TERMINATION", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetData("HEARING_STATUS", ds.Tables[0].Rows[0])))
            {
                terminationReasonDDL = Helper.GetString("TERMINATION_REASON", ds.Tables[0].Rows[0]);
                if ((!string.IsNullOrEmpty(terminationReasonDDL)) && (terminationReasonDDL != "0"))
                {
                    ddlTerminationReasons.SelectedValue = terminationReasonDDL.ToString();
                }
            }
        }
        if(this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.NewReg || this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.ChangeProviderType)
        {
            lblTerminationReasons.Text = "Denial Reasons";
        }
        if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
        {
            if (this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordDateOfMailReturn || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordHearingStatus ||
                this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.CSUploadPI || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadAO ||
                this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadPAO)
            {
                Helper.SetReadOnly(this, false, "formFieldEditable");
            }
            else
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnlyBasic");
            }
        }
        else
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnlyBasic");
        }
        LoadPlaceHolder(hearingRightsID, isEdit, false, sectionName);
    }

    public void LoadPlaceHolder(int hearingRights = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadProposedAdjudicationOrder.Controls.Clear();
        PlaceholderUploadSettlementAgreements.Controls.Clear();
        PlaceholderUploadAdjudicationOrder.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, hearingRights, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, hearingRights, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;
                string titleControl = Helper.GetString("TITLE", dr);
                ucUploadSectionControl.Title = titleControl;

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                {
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                }
                else
                {
                    ucUploadSectionControl.DocumentId = 0;
                }

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);//@"C:\project\temp";
#if DEBUG
                   ucUploadSectionControl.DestinationPath  = @"C:\Temp";
#endif
                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                {
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                }
                else
                {
                    ucUploadSectionControl.FileName = null;
                }
                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (Helper.GetString("TITLE", dr) == "Proposed Adjudication Order")
                {
                    
                    PlaceholderUploadProposedAdjudicationOrder.Controls.Add(ucUploadSectionControl);
                    PlaceholderUploadProposedAdjudicationOrder.Visible = true;
                }
                else if (Helper.GetString("TITLE", dr) == "Settlement Agreement")
                {
                    
                    PlaceholderUploadSettlementAgreements.Controls.Add(ucUploadSectionControl);
                    PlaceholderUploadSettlementAgreements.Visible = true;
                }
                else if (Helper.GetString("TITLE", dr) == "Adjudication Order")
                {
                    
                    PlaceholderUploadAdjudicationOrder.Controls.Add(ucUploadSectionControl);
                    PlaceholderUploadAdjudicationOrder.Visible = true;
                }
            }
        }
    }

    public override string Title
    {
        get { return "Application Disposition"; }
    }

    public override string IdText
    {
        get { return "uc119HearingRights_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "val119HearingRights"; }
    }
}