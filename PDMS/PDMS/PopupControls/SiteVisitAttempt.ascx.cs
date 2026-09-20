using Corp.Core.Libraries.Helper;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class UserControls_SiteVisitAttempt : System.Web.UI.UserControl
{
    #region svc

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

    #endregion

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private const string sectionName = "Site Visit";

    #region Properties

    public bool SiteVisitOperatorFromProviderSearch
    {
        get
        {
            //Must be a Site Visit Operator and must not be coming from provider search
            return (Helper.IsUserInSiteVisitOperatorRole(HttpContext.Current.User.Identity.Name) || Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)) && this.WorkflowPage.RegistrationIdSelected == 0 && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitPCG;
        }
    }

    public bool ComplianceSpecFromProviderSearch
    {
        get
        {
            //Must be a Site Visit Operator and must not be coming from provider search
            return ((Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.RegistrationIdSelected == 0 && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance)
                 ||(string.IsNullOrEmpty(this.WorkflowPage.CurrentTaskName) || (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.SiteVisitPCG)));
        }
    }
    public bool ProviderRole
    {
        get
        {
            return Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
        }
    }
    public bool IsReadOnly
    {
        get
        {
            if (ViewState["IsReadOnly"] == null)
                ViewState["IsReadOnly"] = -1;

            return (bool)ViewState["IsReadOnly"];
        }
        set
        {
            ViewState["IsReadOnly"] = value;
        }
    }

    public int ScreeningActivityID
    {
        get
        {
            if (ViewState["ScreeningActivityID"] == null)
                ViewState["ScreeningActivityID"] = -1;

            return (int)ViewState["ScreeningActivityID"];
        }
        set
        {
            ViewState["ScreeningActivityID"] = value;
        }
    }

    public int SiteVisitID
    {
        get
        {
            if (ViewState["SiteVisitID"] == null)
                ViewState["SiteVisitID"] = -1;

            return (int)ViewState["SiteVisitID"];
        }
        set
        {
            ViewState["SiteVisitID"] = value;
        }
    }

    public int SiteVisitAttemptID
    {
        get
        {
            if (ViewState["SiteVisitAttemptID"] == null)
                ViewState["SiteVisitAttemptID"] = -1;

            return (int)ViewState["SiteVisitAttemptID"];
        }
        set
        {
            ViewState["SiteVisitAttemptID"] = value;
        }
    }
    
    public bool SiteVisitAttemptVisible
    {
        get
        {
            return this.pnlSiteVisitAttemptDetails.Visible;
        }
        set
        {
            this.pnlSiteVisitAttemptDetails.Visible = value;
        }
    }

    public DateTime SiteVisitStartDate
    {
        get
        {
            if (ViewState["SiteVisitStartDate"] == null)
                ViewState["SiteVisitStartDate"] = DateTime.MinValue.ToString();

            return DateTime.Parse(ViewState["SiteVisitStartDate"].ToString());
        }
        set
        {
            ViewState["SiteVisitStartDate"] = value;
        }
    }

    public string SiteVisitTypeName
    {
        get
        {
            if (ViewState["SiteVisitTypeName"] == null)
                ViewState["SiteVisitTypeName"] = DateTime.MinValue.ToString();

            return ViewState["SiteVisitTypeName"].ToString();
        }
        set
        {
            ViewState["SiteVisitTypeName"] = value;
        }
    }

    public int SiteVisitAttemptStatus
    {
        get
        {
            return (int)ViewState["SiteVisitAttemptStatus"];
        }
        set
        {
            ViewState["SiteVisitAttemptStatus"] = value;
        }
    }

    public int SitevisitAttemptCount {
        get
        {
            return (int)ViewState["SitevisitAttemptCount"];
        }
        set
        {
            ViewState["SitevisitAttemptCount"] = value;
        }
    }

    

    #endregion

    #region Events

    public delegate void SiteVisitAttemptCancelEventHandler();
    public event SiteVisitAttemptCancelEventHandler SiteVisitAttemptCancel;

    public delegate void SiteVisitAttemptUpdateEventHandler();
    public event SiteVisitAttemptUpdateEventHandler SiteVisitAttemptUpdated;

    public delegate void RefreshEventHandler(int step);
    public event RefreshEventHandler RefreshEvent;
    #endregion

    public void LoadSiteVisitAttempt(int siteVisitID, int? siteVisitAttemptID, int screeningActivityID, DateTime siteVisitStartDate, string siteVisitTypeName, int siteVisitAttemptStatus, int sitevisitAttemptCount)
    {
        SiteVisitID = siteVisitID;
        SiteVisitAttemptID = siteVisitAttemptID.HasValue ? siteVisitAttemptID.Value : -1;
        ScreeningActivityID = screeningActivityID;
        SiteVisitStartDate = siteVisitStartDate;
        SiteVisitTypeName = siteVisitTypeName;
        SiteVisitAttemptStatus = siteVisitAttemptStatus;
        SitevisitAttemptCount = sitevisitAttemptCount;

        LoadDropDowns();

        InitInputFields();

        SetEditability();

        if (SiteVisitAttemptID > 0)
        {
            LoadExistingSiteVisitAttempt();
        }
        else
        {
            LoadNewSiteVisitAttempt();
        }
        

        this.pnlSiteVisitAttemptDetails.Visible = true;
        this.WorkflowPage.SiteVisitAttemptID = SiteVisitAttemptID;
    }

    protected override void OnLoad(EventArgs e)
    {
        /// Set up file uploader
        //ucUploadDocument.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        //if (Helper.IsUserInRoll(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly))
        //{
        //    ucUploadDocument.SetUploadButtonEnabled = false;
        //}
        LoadDropDowns();
        //InitInputFields();
        SetEditability();
        base.OnLoad(e);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (SiteVisitOperatorFromProviderSearch)
        {
            bool result = ValidateResultChange();

            if (result)
            {
                SaveAttempt(false);

                InitInputFields();

                this.pnlSiteVisitAttemptDetails.Visible = false;
                if (SiteVisitAttemptUpdated != null)
                {
                    SiteVisitAttemptUpdated();
                }
                Refresh(true);
            }
        }
        else if(ComplianceSpecFromProviderSearch)
        {

            if (validateFindingsSelection())
            {
                SaveAttempt(true);
                //UpdateComplianceAttempt();
                Refresh(true);
            }
        }
    }
    public void Refresh(bool forceRedirect = false)
    {
        if (forceRedirect)
        {
            string url = "~/Process/Registration.aspx?Step=" + this.WorkflowPage.RegistrationStep.ToString() + "&RegId=" + this.WorkflowPage.RegistrationId.ToString();
            Response.Redirect(url);
        }
        else if (RefreshEvent != null) RefreshEvent(this.WorkflowPage.RegistrationStep);
    }
    protected void btnCancelSiteVisit_Click(object sender, EventArgs e)
    {
        this.pnlSiteVisitAttemptDetails.Visible = false;
        if (SiteVisitAttemptCancel != null)
        {
            SiteVisitAttemptCancel();
        }
    }


    #region Private Methods
    private void InitInputFields()
    {
        /// Clear the input fields.
        txtVisitDate.Text = string.Empty;
        //ddlResult.SelectedIndex = 0;
        ddlRecommendation.SelectedIndex = 0;
        txtComments.Text = string.Empty;
    }

    private void LoadDropDowns()
    {
        DataSet ds = null;
        if (this.ddlRecommendation.Items.Count == 0)
        {
            ds = svc.SelectSiteVisitRecommendations();
            Helper.LoadList(this.ddlRecommendation, ds.Tables[0], "SITE_VISIT_RECOMMENDATION_NAME", "SITE_VISIT_RECOMMENDATION_ID", true);
        }
        if (this.ddlMethod.Items.Count == 0)
        {
            ds = svc.SelectSiteVisitMethods();
            Helper.LoadList(this.ddlMethod, ds.Tables[0], "SITE_VISIT_METHOD_NAME", "SITE_VISIT_METHOD_ID", true);
        }

        if (this.ddlfindings.Items.Count == 0)
        {
            ds = svc.SelectSiteVisitFindings();
            Helper.LoadList(this.ddlfindings, ds.Tables[0], "SITE_VISIT_FINDINGS_NAME", "SITE_VISIT_FINDINGS_ID", true);
        }


    }

    private void LoadNewSiteVisitAttempt()
    {
        //Attempt Does not Exist, get the Type of site visit for which you are performing an attempt
        if (SiteVisitID > 0)
        {
            DataSet ds = svc.SelectSiteVisitDetails(SiteVisitID);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtMatch = ds.Tables[0];
                DataRow drMatch = dtMatch.Rows[0];
                //lblSiteVisitType.Text = Helper.GetString("SITE_VISIT_TYPE_NAME", drMatch);
                lblOrganizationName.Text = Helper.GetString("ORGANIZATION_NAME", drMatch);

                //TODO:  why load completed date of site visit and recommendation if adding a new one?
                //DateTime? temp = Helper.GetDateTime("COMPLETED_DATE", drMatch);
                //txtVisitDate.Text = temp.HasValue ? temp.Value.ToString("MM/dd/yyyy") : string.Empty;
            }
            //when no attempts recorded for a site visit then insert a new attempt with initial ateempt
            //string changedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
            //svc.InsertSiteVisitAttempt(SiteVisitID, CON.SiteVisitStatusID.NotCompleted, null, DateTime.Now, changedBy, null, DateTime.Now, changedBy, 1, null);
        }
    }

    private void LoadExistingSiteVisitAttempt()
    {
        if (SiteVisitAttemptID > -1)
        {
            DataSet ds = svc.SelectSiteVisitAttemptDetails(SiteVisitAttemptID);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dtAttempt = ds.Tables[0];
                DataRow drAttempt = dtAttempt.Rows[0];

                lblOrganizationName.Text = Helper.GetString("ORGANIZATION_NAME", drAttempt);
                lblAttempt.Text = Helper.GetString("SITE_VISIT_ATTEMPT_TYPE_NAME", drAttempt);
                //if (SitevisitAttemptCount == 0 || (SitevisitAttemptCount == 1 && SiteVisitAttemptStatus == CON.SiteVisitAttemptStatusID.Inprogress))
                //{
                //    lblAttempt.Text = "Initial";
                //}
                //else
                //{
                //    lblAttempt.Text = "Follow-Up";
                //}
                

                DateTime? temp = Helper.GetDateTime("PERFORMED_DATE", drAttempt);
                txtVisitDate.Text = temp.HasValue ? temp.Value.ToString("MM/dd/yyyy") : string.Empty;
                lblVisitDate.Text = temp.HasValue ? temp.Value.ToString("MM/dd/yyyy") : string.Empty;
                lblPerformedBy.Text = Helper.GetString("PERFORMED_BY_USERNAME", drAttempt);
                
                this.ddlRecommendation.SelectedValue = Helper.GetString("SITE_VISIT_RECOMMENDATION_ID", drAttempt);
                lblRecommendation.Text = Helper.GetString("SITE_VISIT_RECOMMENDATION_NAME", drAttempt);
                this.txtComments.Text = Helper.GetString("COMMENTS", drAttempt).Trim();
                if (ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.FailedFollowup)
                {
                    DateTime? temp1 = Helper.GetDateTime("PROVIDER_RESPONSE_DATE", drAttempt);
                    
                    txtResponseDate.Visible = true;
                    if (temp1.Value != DateTime.MinValue)
                        txtResponseDate.Text = temp1.HasValue ? temp1.Value.ToString("MM/dd/yyyy") : string.Empty;
                    else
                        txtResponseDate.Text = "";
                    txtResponseDate.Enabled = false; 
                }
                else
                {
                    txtResponseDate.Visible = false;
                }
                this.ddlMethod.SelectedValue = Helper.GetString("SITE_VISIT_METHOD_ID", drAttempt);
                this.lblMethod.Text = Helper.GetString("SITE_VISIT_METHOD_NAME", drAttempt);
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                DataSet ds1 = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
                int registrationStatusTypeID = 0;
                int currentTaskID = 0;
                if (ComplianceSpecFromProviderSearch)
                {
                    DataSet dssr = psc.SelectServiceRemovedCheckBox(this.WorkflowPage.RegistrationId);
                    txtComplianceComments.Text = Helper.GetString("SPECIALIST_COMMENTS", drAttempt);
                    DateTime? NODIssueDt = Helper.GetDateTime("NOD_ISSUE_DATE", drAttempt);
                    DateTime? pocDate = Helper.GetDateTime("POC_DATE", drAttempt);
                    txtIssueDate.Text = NODIssueDt.HasValue && NODIssueDt.Value!=System.Data.SqlTypes.SqlDateTime.MinValue ? NODIssueDt.Value.ToString("MM/dd/yyyy") : string.Empty;
                    txtplanofCorr.Text = pocDate.HasValue && pocDate.Value != System.Data.SqlTypes.SqlDateTime.MinValue ? pocDate.Value.ToString("MM/dd/yyyy") : string.Empty;
                    ddlfindings.SelectedValue = Helper.GetString("SITE_VISIT_FINDINGS_ID", drAttempt);
                    if (dssr != null && dssr.Tables.Count > 0 && dssr.Tables[0].Rows.Count > 0)
                    {
                        chkServiceRemoved.Checked = Helper.GetBool("SERVICE_REMOVED", dssr.Tables[0].Rows[0]);
                        hdnchkValue.Value = Helper.GetString("reg_application_ID", dssr.Tables[0].Rows[0]);
                    }
                    LoadPlaceHolder();
                }
                if (Helper.HasRows(ds1))
                {
                    registrationStatusTypeID = Helper.GetInt("RegistrationStatusTypeID", ds1.Tables[0].Rows[0]);
                    currentTaskID = Helper.GetInt("CurrentTaskID", ds1.Tables[0].Rows[0]);
                }
                if(registrationStatusTypeID == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    LoadPlaceHolder();
                }
                if (Helper.GetString("SITE_VISIT_RECOMMENDATION_ID", drAttempt) != "" )
                {
                    //OHPNM-1576 pschwarz enable visit date and recommendation drop downs if in site visit
                    if (currentTaskID == CON.ComplianceTaskID.SiteVisitReview1Task ||
                       currentTaskID == CON.ComplianceTaskID.SiteVisitReview2Task ||
                       currentTaskID == CON.ComplianceTaskID.SiteVisitReview3Task ||
                       currentTaskID == CON.ComplianceTaskID.SiteVisitReview4Task )
                    {
                        txtVisitDate.Enabled = true;
                        ddlRecommendation.Enabled = true;
                    }
                    else if (registrationStatusTypeID != CON.RegistrationStatusTypeId.ReturnToSiteVisit)
                    {
                        txtVisitDate.Enabled = false;
                        ddlRecommendation.Enabled = false;
                    }
                    else if (registrationStatusTypeID == CON.RegistrationStatusTypeId.ReturnToSiteVisit)
                    {
                        if (ddlRecommendation.SelectedIndex != CON.SiteVisitRecommendationID.Failed)
                        {
                            txtVisitDate.Enabled = false;
                            ddlRecommendation.Enabled = false;
                        }
                        else
                        {
                            txtVisitDate.Enabled = true;
                            ddlRecommendation.Enabled = true;
                        }
                    }
                }
                else
                {
                    txtVisitDate.Enabled = SiteVisitOperatorFromProviderSearch;
                    ddlRecommendation.Enabled = SiteVisitOperatorFromProviderSearch;
                    txtVisitDate.Text = "";

                }
            }
        }
    }

    private bool ValidateResultChange()
    {
        bool result = true;

        DataSet ds = svc.SelectRegDocuments(this.WorkflowPage.RegistrationId, CON.RegistrationPageType.SiteVisitScreening, string.Empty, string.Empty, SiteVisitAttemptID);
            if (!Helper.HasRows(ds))
            {
                if (ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.Approved
                || ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.Failed
                || ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.FailedFollowup)
                {
                    CustomValidator cusval = new CustomValidator();
                    cusval.IsValid = false;
                    cusval.ErrorMessage = "You must upload a copy of your Site Visit Report.";
                    cusval.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(cusval);
                    return false;
                }               
            }
            else
            {
                if( (ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.Approved
                || ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.Failed
                || ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.FailedFollowup)
                && ds.Tables[0].Rows.Count < 1)
                {
                    CustomValidator cusval = new CustomValidator();
                    cusval.IsValid = false;
                    cusval.ErrorMessage = "You must upload a copy of your Site Visit Report.";
                    cusval.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(cusval);
                    return false;
                }
            }
        

            if (string.IsNullOrEmpty(ddlRecommendation.SelectedValue))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must select a recommendation for the completed site visit.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                result = false;
            }
            if (string.IsNullOrEmpty(txtComments.Text))
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "You must enter comments for the completed site visit attempt.";
                val.ValidationGroup = "valProviderInfoHeader";
                this.Page.Validators.Add(val);

                result = false;
            }
            if (ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.Approved)
            {
                if ((lblAttempt.Text != "Initial" && lblAttempt.Text != "Follow-Up" && lblAttempt.Text != "") && txtResponseDate.Visible && String.IsNullOrEmpty(txtResponseDate.Text))
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Provider Response Date must be entered in order to approve.";
                    val.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(val);

                    result = false;
                }
                if (txtResponseDate.Visible && (Convert.ToDateTime(txtResponseDate.Text) > (this.SiteVisitStartDate).AddDays(30)))
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Recommendation should be set to ‘Failed’, Provider did not respond within 30 days.";
                    val.ValidationGroup = "valProviderInfoHeader";
                    this.Page.Validators.Add(val);

                    result = false;
                }

            }
            
        //}
        return result;
    }

    private bool validateFindingsSelection()
    {
        bool result = true;
        if (string.IsNullOrEmpty(ddlfindings.SelectedValue))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "You must select a Findings for the completed site visit review.";
            val.ValidationGroup = "valProviderInfoHeader";
            this.Page.Validators.Add(val);

            result = false;
        }
        return result;
    }

    private void SaveAttempt(bool isComplianceAttempt)
    {
        int statusID = isComplianceAttempt ? CON.SiteVisitStatusID.Completed : CON.SiteVisitStatusID.NotCompleted;
        Page.Validate("SiteVisit");

        int? recommendationID;
        if (string.IsNullOrEmpty(ddlRecommendation.SelectedValue))
        {
            recommendationID = null;
        }
        else
        {
            recommendationID = int.Parse(ddlRecommendation.SelectedValue);
        }

        DateTime? completedDate = null;
        if (!string.IsNullOrEmpty(txtVisitDate.Text))
        {
            completedDate = DateTime.Parse(txtVisitDate.Text);
        }
        bool isValid = true;
        foreach (Control ctrl in PlaceholderUploadSiteVisit.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("SiteVisit");
        }

        string completedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string changedBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        string comments = txtComments.Text.Trim();
        int? siteVisitAttemptTypeID = null;
        int? siteVisitAttemptStatusID = null;
        DateTime? providerResponseDate = null;
        if (txtResponseDate.Visible == true && txtResponseDate.Text != "")
            providerResponseDate = Convert.ToDateTime(txtResponseDate.Text);
        if (!isComplianceAttempt)
        {
            siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.Initial;
            if (lblAttempt.Text == "Initial" || lblAttempt.Text == "")
                siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.Initial;
            else if (lblAttempt.Text == "Follow-Up")
                siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.Follow_Up;
            else
                siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.FailedFollow_Up;

            siteVisitAttemptStatusID = CON.SiteVisitAttemptStatusID.Inprogress;
        }
        
        DateTime? requiredDate = null;
        int? siteVisitMethoID;
        if (string.IsNullOrEmpty(this.ddlMethod.SelectedValue))
        {
            siteVisitMethoID = null;
        }
        else
        {
            siteVisitMethoID = int.Parse(this.ddlMethod.SelectedValue);
        }
        int? findingsId = null; 
        DateTime? nodIssueDate = null;
        DateTime? pocDate = null;
        string complianceComments = string.Empty;
        if (isComplianceAttempt)
        {
            complianceComments = txtComplianceComments.Text.Trim();
            if (string.IsNullOrEmpty(ddlfindings.SelectedValue))
            {
                findingsId = null;
            }
            else
            {
                findingsId = int.Parse(ddlfindings.SelectedValue);
            }


            if (!string.IsNullOrEmpty(txtIssueDate.Text))
            {
                nodIssueDate = DateTime.Parse(txtIssueDate.Text);
            }


            if (!string.IsNullOrEmpty(txtplanofCorr.Text))
            {
                pocDate = DateTime.Parse(txtplanofCorr.Text);
            }
            svc.UpdateSiteVisitAttempt(SiteVisitAttemptID, statusID, recommendationID, completedDate, completedBy, comments, DateTime.Now, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethoID, findingsId, nodIssueDate, pocDate, complianceComments);
            Dictionary<string, string> parms = new Dictionary<string, string>();
            var chkvalue = chkServiceRemoved.Checked ? 1 : 0;
            if (hdnchkValue.Value != "")
            {
                parms.Add("reg_application_ID", hdnchkValue.Value.ToString());
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("SERVICE_REMOVED", chkvalue.ToString());
                parms.Add("CREATED_ON_DATE_TIME", null);
                parms.Add("CREATED_BY_USER", null);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", completedBy);
                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "APPLICATION", parms);
            }
        }
        else
        {
            /// Insert / Update site visit attempt
            if (SiteVisitAttemptID > 0 && SiteVisitAttemptStatus != CON.SiteVisitAttemptStatusID.Completed)
            {
                if (recommendationID == CON.SiteVisitRecommendationID.FollowupVisit)
                {
                    requiredDate = Convert.ToDateTime(completedDate).AddDays(10);
                    svc.UpdateSiteVisitAttemptDueByDate(SiteVisitID, requiredDate);
                }

                else if (recommendationID == CON.SiteVisitRecommendationID.FailedFollowup)
                {
                    requiredDate = Convert.ToDateTime(completedDate).AddDays(20);
                }
                svc.UpdateSiteVisitAttempt(SiteVisitAttemptID, statusID, recommendationID, completedDate, completedBy, comments, DateTime.Now, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethoID, findingsId, nodIssueDate, pocDate, complianceComments);
                if (recommendationID != null && recommendationID == CON.SiteVisitRecommendationID.FollowupVisit)
                {
                    siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.Follow_Up;
                    siteVisitAttemptStatusID = CON.SiteVisitAttemptStatusID.Inprogress;
                    svc.InsertSiteVisitAttempt(SiteVisitID, CON.SiteVisitStatusID.NotCompleted, null, null, null, null, DateTime.Now, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethoID);
                }
              
            }

            else
            {
                svc.InsertSiteVisitAttempt(SiteVisitID, statusID, recommendationID, completedDate, completedBy, comments, DateTime.Now, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethoID);
                if (recommendationID != null && recommendationID == CON.SiteVisitRecommendationID.FollowupVisit)
                {
                    siteVisitAttemptTypeID = CON.SiteVisitAttemptTypeID.Follow_Up;
                    siteVisitAttemptStatusID = CON.SiteVisitAttemptStatusID.Inprogress;
                    svc.InsertSiteVisitAttempt(SiteVisitID, CON.SiteVisitStatusID.NotCompleted, null, null, null, null, DateTime.Now, changedBy, siteVisitAttemptTypeID, providerResponseDate, siteVisitAttemptStatusID, requiredDate, siteVisitMethoID);
                }
               
            }
        }

        if (txtComplianceComments.Text.Trim()!="")
        {
            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Site Visit Attempt - Comment - " + txtComplianceComments.Text.Trim(), personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.NotSubmitted, processID: this.WorkflowPage.WF_ProcessID);
        }
        if (!string.IsNullOrEmpty(comments))
        {
            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, "Site Visit Attempt - Comment - " + comments, personReviewedBy: HttpContext.Current.User.Identity.Name, finalDisposition: CON.FinalDisposition.NotSubmitted, processID: this.WorkflowPage.WF_ProcessID);
        }

    }

    private void UpdateComplianceAttempt()
    {
        int statusID = CON.SiteVisitStatusID.Completed;
        Page.Validate("SiteVisit");

        int? findingsId;
        if (string.IsNullOrEmpty(ddlfindings.SelectedValue))
        {
            findingsId = null;
        }
        else
        {
            findingsId = int.Parse(ddlfindings.SelectedValue);
        }
        string comments = txtComplianceComments.Text.Trim();

        DateTime? nodIssueDate = null;
        if (!string.IsNullOrEmpty(txtIssueDate.Text))
        {
            nodIssueDate = DateTime.Parse(txtIssueDate.Text);
        }

        DateTime? pocDate = null;
        if (!string.IsNullOrEmpty(txtplanofCorr.Text))
        {
            pocDate = DateTime.Parse(txtplanofCorr.Text);
        }
        bool isValid = true;
        foreach (Control ctrl in PlaceholderUploadSiteVisit.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("SiteVisit");
        }

        string complianceUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
        svc.UpdateSiteVisitAttemptComplianceSpecialist(SiteVisitAttemptID, findingsId, comments, nodIssueDate, pocDate, complianceUser);
        Dictionary<string, string> parms = new Dictionary<string, string>();
        var chkvalue = chkServiceRemoved.Checked ? 1 : 0;
        if (hdnchkValue.Value != "")
        {
            parms.Add("reg_application_ID", hdnchkValue.Value.ToString());
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("SERVICE_REMOVED", chkvalue.ToString()); 
            parms.Add("CREATED_ON_DATE_TIME", null);
            parms.Add("CREATED_BY_USER", null);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", complianceUser);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "APPLICATION", parms);
        }
    }

    private void SetEditability()
    {
        this.txtVisitDate.Enabled = SiteVisitOperatorFromProviderSearch;
        this.txtComments.Enabled = SiteVisitOperatorFromProviderSearch;
        this.ddlRecommendation.Enabled = SiteVisitOperatorFromProviderSearch;
        this.dvtxtVisitDate.Visible = SiteVisitOperatorFromProviderSearch;
        this.dvddlRecommendation.Visible = SiteVisitOperatorFromProviderSearch;
        this.dvddlMethod.Visible = SiteVisitOperatorFromProviderSearch;
        this.dvlblMethod.Visible = ComplianceSpecFromProviderSearch;
        this.dvddlFindings.Visible= ComplianceSpecFromProviderSearch;
        this.dvlblVisitDate.Visible = ComplianceSpecFromProviderSearch;
        this.dvlblRecommendation.Visible = ComplianceSpecFromProviderSearch;
        this.dvplaceHolder.Visible = ComplianceSpecFromProviderSearch;
        this.dvPlanOfCorrection.Visible = ComplianceSpecFromProviderSearch||ProviderRole;
        this.sepLogSiteVisitAttempt.Visible = !ProviderRole;
        this.dvPlaceHolderUpload.Visible = ComplianceSpecFromProviderSearch;
        this.dvplaceHolder1Upload.Visible = ComplianceSpecFromProviderSearch;
        this.dvOpcomments.Visible = !ComplianceSpecFromProviderSearch;
        this.dvComplianceComments.Visible = ComplianceSpecFromProviderSearch;
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistration(this.WorkflowPage.RegistrationId);
        int status;
        if (Helper.HasRows(ds))
        {
            if (int.TryParse(Helper.GetData("REGISTRATION_STATUS_TYPE_ID", ds.Tables[0].Rows[0]), out status))
            {
                if (status == CON.RegistrationStatusTypeId.ReturnToProviderForSiteVisit)
                {
                    this.txtVisitDate.Visible = this.txtComments.Visible = this.ddlRecommendation.Visible = this.dvtxtVisitDate.Visible = this.dvddlRecommendation.Visible = 
                    this.dvddlMethod.Visible = this.dvlblMethod.Visible = this.dvddlFindings.Visible = this.dvlblVisitDate.Visible = this.dvlblRecommendation.Visible = 
                    this.dvplaceHolder.Visible = this.dvOpcomments.Visible = this.dvComplianceComments.Visible = 
                    this.dvPerformedBy.Visible = this.dvRecommendation.Visible = this.dvSiteVisitProcess.Visible = this.dvVisitDate.Visible = dvSave.Visible = false;

                    this.dvPlaceHolderUpload.Visible = this.dvplaceHolder1Upload.Visible = true;
					this.dvOrganizationName.Visible = this.dvAttempt.Visible = false;
                }
            }
        }

    }

    #endregion
    protected void ddlRecommendation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlRecommendation.SelectedIndex == CON.SiteVisitRecommendationID.FailedFollowup)
        {
            txtResponseDate.Visible = true;
            txtResponseDate.Enabled = true;
        }
        else
        {
            txtResponseDate.Visible = false;
        }
    }

   

    public void LoadPlaceHolder(int licensureId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadSiteVisit.Controls.Clear();
        PlaceholderUploadSiteVisit1.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, isEdit);

        int table = ds.Tables.Count;
        int rowCount = 0;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                rowCount++;
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.RowId = licensureId;

                ucUploadSectionControl.SectionName = pageSection;

                if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
                    ucUploadSectionControl.RemovePermissions = false;



                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (rowCount == 1)
                {
                    PlaceholderUploadSiteVisit.Controls.Add(ucUploadSectionControl);
                }
                if (rowCount == 2)
                {
                    PlaceholderUploadSiteVisit1.Controls.Add(ucUploadSectionControl);
                }
            }
        }

    }
}