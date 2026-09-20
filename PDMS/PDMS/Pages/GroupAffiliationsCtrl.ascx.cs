using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_GroupAffiliationsCtrl : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

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

    #region Parent Page Events

    public delegate void ValidationEventHandler();
    public event ValidationEventHandler ValidationEvent;

    public delegate void ErrorEventHandler();
    public event ErrorEventHandler ErrorEvent;

    public delegate void KeepOpenEventHandler();
    public event KeepOpenEventHandler KeepOpenEvent;

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion

    #region Properties

    public int RegAffiliationID
    {
        get
        {
            return ViewState["RegAffiliationID"] == null ? 0 : Convert.ToInt32(ViewState["RegAffiliationID"]);
        }
        set
        {
            ViewState["RegAffiliationID"] = value;
        }
    }


    public int RegProgramStatusTypeID
    {
        get
        {
            return ViewState["RegProgramStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegProgramStatusTypeID"]);
        }
        set
        {
            ViewState["RegProgramStatusTypeID"] = value;
        }
    }

    public int RegStatusTypeID
    {
        get
        {
            return ViewState["RegStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegStatusTypeID"]);
        }
        set
        {
            ViewState["RegStatusTypeID"] = value;
        }
    }

    public int GroupAffiliationStatusID
    {
        get
        {
            return ViewState["GroupAffiliationStatusID"] == null ? 0 : Convert.ToInt32(ViewState["GroupAffiliationStatusID"]);
        }
        set
        {
            ViewState["GroupAffiliationStatusID"] = value;
        }
    }

    public string AffiliateMedicaidID
    {
        get
        {
            return ViewState["AffiliateMedicaidID"] == null ? string.Empty : ViewState["AffiliateMedicaidID"].ToString();
        }
        set
        {
            ViewState["AffiliateMedicaidID"] = value;
        }
    }

    public int SpecialtyOneStatusID
    {
        get
        {
            return ViewState["SpecialtyOneStatusID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyOneStatusID"]);
        }
        set
        {
            ViewState["SpecialtyOneStatusID"] = value;
        }
    }

    public int SpecialtyTwoStatusID
    {
        get
        {
            return ViewState["SpecialtyTwoStatusID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyTwoStatusID"]);
        }
        set
        {
            ViewState["SpecialtyTwoStatusID"] = value;
        }
    }

    public int SpecialtyThreeStatusID
    {
        get
        {
            return ViewState["SpecialtyThreeStatusID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyThreeStatusID"]);
        }
        set
        {
            ViewState["SpecialtyThreeStatusID"] = value;
        }
    }

    public int SpecialtyOneID
    {
        get
        {
            return ViewState["SpecialtyOneID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyOneID"]);
        }
        set
        {
            ViewState["SpecialtyOneID"] = value;
        }
    }

    public int SpecialtyTwoID
    {
        get
        {
            return ViewState["SpecialtyTwoID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyTwoID"]);
        }
        set
        {
            ViewState["SpecialtyTwoID"] = value;
        }
    }

    public int SpecialtyThreeID
    {
        get
        {
            return ViewState["SpecialtyThreeID"] == null ? 0 : Convert.ToInt32(ViewState["SpecialtyThreeID"]);
        }
        set
        {
            ViewState["SpecialtyThreeID"] = value;
        }
    }


    public int LicensureID
    {
        get
        {
            return ViewState["LicensureID"] == null ? 0 : Convert.ToInt32(ViewState["LicensureID"]);
        }
        set
        {
            ViewState["LicensureID"] = value;
        }
    }

    public DateTime? ParentEffectiveDate
    {
        get
        {
            if (ViewState["ParentEffectiveDate"] != null)
            {
                return Convert.ToDateTime(ViewState["ParentEffectiveDate"]);
            }
            return null;
        }
        set
        {
            ViewState["ParentEffectiveDate"] = value;
        }
    }

    public bool UpdateGrid
    {
        get
        {
            return ViewState["UpdateGrid"] == null ? false : Convert.ToBoolean(ViewState["UpdateGrid"]);
        }
        set
        {
            ViewState["UpdateGrid"] = value;
        }
    }

    private Dictionary<string, string> Errors = new Dictionary<string, string>();

    private DataTable dtMatch;
    private DataTable dtIndividuals;
    private bool enableEffectiveDateCheck = true; //By default its true all the time, except when end dating an affiliations.
    private string endDate = string.Empty;

    public int NewGroupAffiliationStatusID
    {
        get
        {
            return ViewState["NewGroupAffiliationStatusID"] == null ? 0 : Convert.ToInt32(ViewState["NewGroupAffiliationStatusID"]);
        }
        set
        {
            ViewState["NewGroupAffiliationStatusID"] = value;
        }
    }
    private string IndividualMedicaidID
    {
        get
        {
            return ViewState["IndividualMedicaidID"] == null ? string.Empty : Convert.ToString(ViewState["IndividualMedicaidID"]);
        }
        set
        {
            ViewState["IndividualMedicaidID"] = value;
        }
    }
    private string showMessage = string.Empty;
    public string ParentEnrollmentStatus
    {
        get
        {
            if (ViewState["ParentEnrollmentStatus"] != null)
            {
                return Convert.ToString(ViewState["ParentEnrollmentStatus"]);
            }
            return null;
        }
        set
        {
            ViewState["ParentEnrollmentStatus"] = value;
        }
    }
    public bool Confirmed
    {
        get
        {
            return (hdnGroupAffiliationConfirm.Value == "1") ? true : false;
        }
        set
        {
            hdnGroupAffiliationConfirm.Value = (value) ? "1" : "0";
        }
    }
    private void SetDtMatch()
    {
        if (dtMatch == null)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("NPI", txtNPI.Text);
            parms.Add("Medicaid_id", txtMedicaidID.Text);
            DataSet ds = svc.SelectRegistrationDataWithParams("usp_SearchAllProviderByNPI", parms);
            dtMatch = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    private DataTable dt;
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATION");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    private DateTime? AffiliationStartDate
    {
        get
        {
            if (ViewState["AffiliationStartDate"] != null)
            {
                return Convert.ToDateTime(ViewState["AffiliationStartDate"]);
            }
            return null;
        }
        set
        {
            ViewState["AffiliationStartDate"] = value;
        }
    }
    #endregion

    #region Page Events
    protected void cvGroupAffiliation_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!string.IsNullOrEmpty(showMessage) && hdnGroupAffiliationConfirm.Value != "1")
        {
            //lblMessage.Text = showMessage;
            string SaveButtonClientID = btnSave.ClientID;
            lblmessage.Text = showMessage;

            btnSave.Text = "Continue";
            divConfirmGroupAffiliation.Style.Add("display", "block");
            divConfirmGroupAffiliation.Visible = true;
            lblmessage.Visible = true;
            hdnGroupAffiliationConfirm.Value = "1";


            args.IsValid = false;

        }
        else
        {
            lblmessage.Visible = divConfirmGroupAffiliation.Visible = false;
            btnSave.Text = "Save";
            divConfirmGroupAffiliation.Style.Add("display", "none");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }

    private string getAffiliationStatusText(int statusID)
    {
        switch (statusID)
        {
            case 1:
                return CON.GroupAffiliationStatusType.IndividualEnrollmentPendingApproval;
            case 2:
                return CON.GroupAffiliationStatusType.ConfirmGroupMember;
            case 3:
                return CON.GroupAffiliationStatusType.GroupConfirmed;
            case 4:
                return CON.GroupAffiliationStatusType.Active;
            case 5:
                return CON.GroupAffiliationStatusType.PendingRemoval;
            case 6:
                return CON.GroupAffiliationStatusType.RemovedbyGroup;
            case 7:
                return CON.GroupAffiliationStatusType.IndividualRequiresReValidation;
            case 8:
                return CON.GroupAffiliationStatusType.PendingApproval;
            case 9:
                return CON.GroupAffiliationStatusType.MemberNotFound;
            case 10:
                return CON.GroupAffiliationStatusType.TransactionRejected;
            default:
                return CON.GroupAffiliationStatusType.IndividualEnrollmentPendingApproval;

        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string rd = ddlRenderingLocations.SelectedValue;
        DataSet dsRenderingLocations = svc.SelectRenderingLocationsByRegID(this.WorkflowPage.RegistrationId);
        BindRenderingLocation(dsRenderingLocations);
        ddlRenderingLocations.SelectedValue = rd;
        if (ValidateData())
        {
            lblAffiliationStatus.Text = getAffiliationStatusText(NewGroupAffiliationStatusID);
            //showMessage = string.Empty;
            Page.Validate("GroupAffiliations");

            if (!Page.IsValid)
            {
                if (ValidationEvent != null)
                {
                    ValidationEvent();
                }
                return;
            }

            bool rtn = SaveData();
            if (!rtn)
                return;

            UpdateGrid = true;
            //On successful save, reget and check to see if status is Confirm, if so, show confirm area.
            //Reget Affiliation to see if can confirm provider.
            DataSet affiliationData = this.GetRegAffiliation();
            if (!Helper.HasRows(affiliationData))
            {
                return;
            }

            this.GroupAffiliationStatusID = Helper.GetInt("GROUP_AFFILIATION_STATUS_ID", affiliationData.Tables[0].Rows[0]);

            if (SaveEvent != null)
            {
                SaveEvent();
            }
        }
    }
    protected void rptAffilQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Label lblTypeId = (Label)e.Item.FindControl("lblQuestionTypeID");
            Label lblCommentReqdInfo = (Label)e.Item.FindControl("lblCommentReqdInfo");

            if (lblTypeId != null && lblCommentReqdInfo != null)
            {
                lblCommentReqdInfo.Text = lblTypeId.Text == "GA05" ? "If 'NO' a comment is required." : "If 'YES' a comment is required.";
            }
            if ((GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.GroupConfirmed) || (GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.ConfirmGroupMember && !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)))
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)e.Item.FindControl("rblConfirmQuestion");
                TextBox txtResponseComment = (TextBox)e.Item.FindControl("txtResponseComment");
                rblConfirmQuestion.Enabled = false;
                txtResponseComment.Enabled = false;
            }

        }

    }

    protected void btnSaveConfirm_Click(object sender, EventArgs e)
    {
        RequiredFieldValidator1.Enabled = true;
        Page.Validate("ConfirmAffiliations");

        if (!Page.IsValid)
        {
            if (ValidationEvent != null)
            {
                ValidationEvent();
            }
            return;
        }

        //Save the answers and update status to Group Confirmed (5).
        this.UpdateRegAffiliationConfirmation();
        if (Errors.Count == 0)
        {
            if (SaveEvent != null)
            {
                SaveEvent();
            }
        }
    }

    private void GetEffectiveDate()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");

        if (String.IsNullOrEmpty(Helper.GetString("CHANGE_EFFECTIVE_DATE", ds.Tables[0].Rows[0])))
        {
            this.ParentEffectiveDate = Helper.GetDateTime("REQUESTED_EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
        }
        else
        {
            this.ParentEffectiveDate = Helper.GetDateTime("CHANGE_EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
        }
        if (!string.IsNullOrEmpty(Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0])))
            this.ParentEnrollmentStatus = Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0]);

        //return true;
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        if (UpdateGrid)
        {
            if (SaveEvent != null)
                SaveEvent();
        }
        else
        {
            if (CancelEvent != null)
                CancelEvent();
        }

    }


    #endregion

    #region Public Methods

    public void LoadData(int regAffiliationID)
    {
        ClearErrorMessages();
        this.InitFormFields();
        hdnGroupAffiliationConfirm.Value = "0";
        RegAffiliationID = regAffiliationID;
        btnSave.Text = "Save";
        lblAffiliationStatus.Text = CON.GroupAffiliationStatusType.MemberNotFound;

        this.SetInitialFieldVisibility();

        if (RegAffiliationID > 0)
        {
            DataSet affiliationData = this.GetRegAffiliation();
            this.SetFormFields(affiliationData);
        }
        else
        {
            GetEffectiveDate();
        }

        this.SetEditabilityByAffilitationStatus();

        //btnSave.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSave, null) + ";");
        btnSaveConfirm.Attributes.Add("onclick", " this.disabled = true; " + this.Page.ClientScript.GetPostBackEventReference(btnSaveConfirm, null) + ";");

        //loadProviderTypes();
    }

    #endregion

    private void SetEditabilityByAffilitationStatus()
    {
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        Guid CurrentStepOwnerID = Guid.Empty;
        bool operatorUpdate = false;
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (Methods.ColumnExists("CurrentStepOwnerID", dr))
            {
                if (dr["CurrentStepOwnerID"] != DBNull.Value)
                {
                    CurrentStepOwnerID = new Guid(Methods.GetStringValue(dr["CurrentStepOwnerID"]));
                    operatorUpdate = this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && SessionVarRetriever.UserIdSelected == CurrentStepOwnerID.ToString();
                }
            }
        }
        string allowEdit = "false";
        if (Request.QueryString["AllowEdit"] != null)
        {
            allowEdit = Request.QueryString["AllowEdit"].ToString();
        }

        bool canEdit = (Helper.IsUserInAdminRole(HttpContext.Current.User.Identity.Name)) || (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name)) || (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)
                   && ((RegAffiliationID == 0 || this.RegStatusTypeID == CON.RegistrationStatusTypeId.Pending || this.RegStatusTypeID == CON.RegistrationStatusTypeId.ReturnToProvider) || (RegAffiliationID > 0 && allowEdit == "true"))) || operatorUpdate;
        //provider can not modify unless adding new affiliate registration is in return to provider or provider data entry

        InitializeFields();
        EnableAffilValidators(false);
        EnableConfirmValidators(false);
        this.divAffiliationSaveBox.Visible = true;
        //this.divMemberProfile.Visible = false;
        this.btnSave.Visible = false;
        this.btnSaveConfirm.Visible = false;

        ///RULES:
        /// End Date:  when status is Active or Active Conversion or Active Modified (in case of return to provider)
        /// NPI / Tax ID:  on initial add - RegAffiliationID = 0
        /// Name fields:  only pre confirm group member status 
        /// Delete (parent grid): only pre confirmed status
        /// Start Date:  when pre confirmed status
        /// SpecialtyOne:  only on initial add - RegAffiliationID = 0 (and it is visible)
        /// Bug7753:  
        /// Ind Aff - NPI, Tax ID, Start Date:
        /// Only prevent edits (on NPI, Tax ID and  Start Date) after group confirms affiliation. Only allow edit when status = 1 Prov Not Found, 2 - indv In progress, or 3 -
        /// ind reg pending approval.

        switch (GroupAffiliationStatusID)
        {
            case CON.GroupAffiliationStatusTypeID.NotSetYet:
                this.pnlEndDate.Enabled = false;
                EnableAllFields(canEdit);
                this.btnSave.Visible = true;
                EnableAffilValidators(true);
                break;
            case CON.GroupAffiliationStatusTypeID.MemberNotFound:
                this.pnlEndDate.Enabled = true;
                EnableAllFields(canEdit);
                this.btnSave.Visible = true;
                EnableAffilValidators(canEdit);
                break;
            case CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval:
                this.pnlEndDate.Enabled = true;
                EnableAllFields(canEdit);
                this.btnSave.Visible = true;
                EnableAffilValidators(canEdit);
                break;
            case CON.GroupAffiliationStatusTypeID.ConfirmGroupMember:
                this.pnlEndDate.Enabled = true;
                this.txtFirstName.Enabled = canEdit;
                this.txtLastName.Enabled = canEdit;
                this.btnSave.Visible = true;
                EnableConfirmValidators(canEdit);
                SetConfirmationDetailEditability();
                break;
            case CON.GroupAffiliationStatusTypeID.GroupConfirmed:
                this.pnlEndDate.Enabled = true;
                btnSave.Visible = true;
                EnableConfirmValidators(canEdit);
                SetConfirmationDetailEditability();
                break;
            case CON.GroupAffiliationStatusTypeID.Active:
                this.pnlEndDate.Enabled = true;
                this.btnSave.Visible = true;
                this.btnSaveConfirm.Visible = AnyConfirmFieldsEditable();
                EnableConfirmValidators(canEdit);
                SetConfirmationDetailEditability();
                break;
            case CON.GroupAffiliationStatusTypeID.PendingRemoval:
                this.pnlEndDate.Enabled = true;
                this.btnSave.Visible = true;
                break;
            case CON.GroupAffiliationStatusTypeID.PendingApproval:
                this.pnlEndDate.Enabled = true;
                this.btnSave.Visible = true;
                break;
            default:
                this.pnlEndDate.Enabled = true;
                this.btnSave.Visible = true;
                break;
        }
    }

    private bool AnyConfirmFieldsEditable()
    {
        bool anyedit = false;
        /*if (this.ddlLicense.Items.Count > 0 && this.ddlLicense.Enabled)
            anyedit = true;
        if (this.ddlSpecialtyOne.Items.Count > 0 && this.ddlSpecialtyOne.Enabled)
            anyedit = true;
        if (this.ddlSpecialtyTwo.Items.Count > 0 && this.ddlSpecialtyTwo.Enabled)
            anyedit = true;
        if (this.ddlSpecialtyThree.Items.Count > 0 && this.ddlSpecialtyThree.Enabled)
            anyedit = true;*/

        return anyedit;
    }

    private void InitializeFields()
    {
        txtFirstName.Enabled = false;
        txtLastName.Enabled = false;
        txtNPI.Enabled = false;

    }

    private void EnableAllFields(bool enable)
    {
        bool sentToMMIS = AlreadySentToMMIS();

        txtFirstName.Enabled = enable;
        txtLastName.Enabled = enable;
        txtNPI.Enabled = enable && !sentToMMIS;
    }

    private void EnableAffilValidators(bool enable)
    {
        this.valFNReqd.Enabled = enable;
        this.valLNReqd.Enabled = enable;

        this.valStartReqd.Enabled = enable;
        this.valStartRetroCheck.Enabled = enable;
        this.valNPIFormat.Enabled = enable;
        //  this.valNPIUnique.Enabled = enable;
    }

    private void EnableConfirmValidators(bool enable)
    {
        /*this.valQuestionsAnswered.Enabled = enable;
        this.valCommentEntered.Enabled = enable;
        this.valLicenseReqd.Enabled = enable && this.ddlLicense.Items.Count > 1;
        this.valSpecOneReqd.Enabled = enable;*/
    }

    private void SetInitialFieldVisibility()
    {
        btnSave.Visible = (!Registration.InReview(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.RegistrationStep, this.WorkflowPage.WF_TaskID, this.WorkflowPage.CurrentTaskName) || Helper.IsLoggedInUserInAdminRole());
        this.divAffiliationSaveBox.Visible = true;
        //this.divMemberProfile.Visible = false;
    }

    private bool SaveData()
    {
        bool rtn = true;
        Errors.Clear();

        if (this.RegAffiliationID > 0)
        {
            this.UpdateRegAffiliation();
        }
        else
        {
            this.InsertRegAffiliation();
        }

        this.Confirmed = false;
        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return false;
        }

        return rtn;
    }

    private Dictionary<string, string> SetCommonParms()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (!string.IsNullOrEmpty(txtFirstName.Text))
            parms.Add("FIRST_NAME", txtFirstName.Text.Trim());
        if (!string.IsNullOrEmpty(txtLastName.Text))
            parms.Add("LAST_NAME", txtLastName.Text.Trim());
        if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text))
            parms.Add("NAME", string.Concat(txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim()));
        if (!string.IsNullOrEmpty(txtNPI.Text))
        {
            parms.Add("NPI", txtNPI.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtMedicaidID.Text))
        {
            parms.Add("Medicaid_ID", txtMedicaidID.Text.Trim());

        }
        else if (!string.IsNullOrEmpty(IndividualMedicaidID))
            parms.Add("Medicaid_ID", IndividualMedicaidID);

        parms.Add("START_DATE", txtStartDate.Text);


        if (this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.PendingRemoval)
        {
            parms.Add("END_DATE", DateTime.Now.AddDays(-1).ToString());
        }
        else
        {
            parms.Add("END_DATE", txtEndDate.Text);
        }
        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        return parms;
    }

    private Dictionary<string, string> SetCommonParmsdup()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if (txtFirstName.Enabled && txtLastName.Enabled)
            parms.Add("NAME", string.Concat(txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim()));
        if (txtNPI.Enabled)
        {
            parms.Add("NPI", txtNPI.Text.Trim());
        }
        parms.Add("START_DATE", txtStartDate.Text);
        return parms;
    }
    private Dictionary<string, string> SetCommonParmsForORP()
    {
        Dictionary<string, string> parmsforORP = new Dictionary<string, string>();
        if (txtNPI.Enabled)
        {
            parmsforORP.Add("NPI", txtNPI.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtMedicaidID.Text))
        {
            parmsforORP.Add("MedicaidId", txtMedicaidID.Text.Trim());
        }

        return parmsforORP;
    }

    private Dictionary<string, string> SetCommonParmsForTerminated()
    {
        Dictionary<string, string> parmsforTerminated = new Dictionary<string, string>();
        if (txtNPI.Enabled)
        {
            parmsforTerminated.Add("NPI", txtNPI.Text.Trim());
        }
        if (txtMedicaidID.Enabled)
        {
            parmsforTerminated.Add("MedicaidId", txtMedicaidID.Text.Trim());
        }
        return parmsforTerminated;
    }

    private void InitFormFields()
    {
        this.RegAffiliationID = this.GroupAffiliationStatusID = SpecialtyOneStatusID = SpecialtyTwoStatusID = SpecialtyThreeStatusID = 0;
        this.UpdateGrid = false;
        this.txtFirstName.Text = this.txtLastName.Text = IndividualMedicaidID = string.Empty;
        txtNPI.Text = string.Empty;
        txtStartDate.Text = DateTime.Now.ToShortDateString();
		AffiliationStartDate = DateTime.Now;
        txtEndDate.Text = Convert.ToDateTime("12/31/2299").ToShortDateString();
        this.lblAffiliationStatus.Text = string.Empty;
        txtMedicaidID.Text = string.Empty;
        DataSet dsRenderingLocations = svc.SelectRenderingLocationsByRegID(this.WorkflowPage.RegistrationId);
        BindRenderingLocation(dsRenderingLocations);
        this.txtFirstName.ReadOnly = false;
        this.txtLastName.ReadOnly = false;
        this.txtNPI.ReadOnly = false;
        /*this.ddlLicense.Items.Clear();
        this.ddlSpecialtyOne.Items.Clear();
        this.ddlSpecialtyTwo.Items.Clear();
        this.ddlSpecialtyThree.Items.Clear();*/

    }
    private void BindRenderingLocation(DataSet dsRenderingLocations)
    {
        Helper.LoadList(ddlRenderingLocations, dsRenderingLocations.Tables[0], "RenderingLocations", "REG_ADDRESS_ID", true);
        foreach (ListItem item in ddlRenderingLocations.Items)
        {
            foreach (DataRow dr in dsRenderingLocations.Tables[0].Rows)
            {                
                if (item.Value == Convert.ToString(dr["REG_ADDRESS_ID"]))
                {
                    item.Attributes.Add("ADR_END_DATE", Convert.ToString(dr["ADR_END_DATE"]));
                    item.Attributes.Add("RedFlag", Convert.ToString(dr["RedFlag"]));
                    if (Convert.ToString(dr["RedFlag"]) == "Yes")
                    {
                        item.Attributes.Add("style", "background-color:#FF0000;color:black;font-weight:bold;");
                        break;
                    }

                }
            }

        }
    }

    private void SetFormFields(DataSet affiliationData)
    {
        DataRow affiliationRow = affiliationData.Tables[0].Rows[0];

        this.RegAffiliationID = Helper.GetInt("REG_AFFILIATION_ID", affiliationRow);
        //this.PartyID = Helper.GetInt("PARTY_ID", affiliationRow);
        //this.AffiliateMedicaidID = Helper.GetString("AFFILIATION_MEDICAID_ID", affiliationRow);
        this.RegProgramStatusTypeID = Helper.GetInt("REG_PROGRAM_STATUS_TYPE_ID", affiliationRow);
        this.RegStatusTypeID = Helper.GetInt("REGISTRATION_STATUS_TYPE_ID", affiliationRow);
        this.txtFirstName.Text = Helper.GetString("FIRST_NAME", affiliationRow);
        this.txtFirstName.ReadOnly = true;
        this.txtLastName.Text = Helper.GetString("LAST_NAME", affiliationRow);
        this.txtLastName.ReadOnly = true;
        txtNPI.Text = Helper.GetString("NPI", affiliationRow);
        this.txtNPI.ReadOnly = true;
        txtStartDate.Text = Helper.FormatDate2(affiliationRow["START_DATE"].ToString());
		AffiliationStartDate = Helper.GetDateTime("START_DATE", affiliationRow);
        txtEndDate.Text = Helper.FormatDate2(affiliationRow["END_DATE"].ToString());
        this.lblAffiliationStatus.Text = Helper.GetString("AffiliationStatus", affiliationRow);
        this.GroupAffiliationStatusID = Helper.GetInt("GROUP_AFFILIATION_STATUS_ID", affiliationRow);
        if (ddlRenderingLocations.Items.FindByValue(Helper.GetString("GrpRenderingLocation", affiliationRow)) != null)
        {
            ddlRenderingLocations.SelectedValue = Helper.GetString("GrpRenderingLocation", affiliationRow);
        }
        if (GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.Active)
        {
            txtEndDate.Enabled = true;
        }
        else
        {
            txtEndDate.Enabled = false;
        }
        this.txtMedicaidID.Text = Helper.GetString("Medicaid_ID", affiliationRow);
        //in proc:  ISNULL(grpprov.CHANGE_EFFECTIVE_DATE, grpprov.REQUESTED_EFFECTIVE_DATE) as GroupEffectiveDate
        string effectiveDate = Helper.GetDate("GroupEffectiveDate", affiliationRow);


        if (!string.IsNullOrEmpty(effectiveDate))
        {
            this.ParentEffectiveDate = Convert.ToDateTime(effectiveDate);
        }

        if (DisplayConfirmationDetails())
        {
            SetConfirmationDetails(affiliationData);
            //SetPartyDependentDetails();
        }
    }

    private void SetPartyDependentDetails()
    {
        /*if (PartyID == 0)
        {
            if (this.ddlProvider.Items.Count == 0)
            {
                //should never hit.
                return;
            }
            else if (this.ddlProvider.Items.Count > 2)
            {
                //user will have to select
                return;
            }
            else
            {
                //default to the first one and load the dependent fields up
                this.ddlProvider.SelectedIndex = 1;
                PartyID = Convert.ToInt32(this.ddlProvider.SelectedValue);
            }
        }

        LoadLicenses();
        if (this.LicensureID > 0)
        {
            if (Helper.ValueExistsInDropDown(this.ddlLicense, this.LicensureID.ToString()))
            {
                this.ddlLicense.SelectedValue = LicensureID.ToString();
            }
        }

        LoadSpecialties(ddlSpecialtyOne, 1);

        if (this.SpecialtyOneID > 0)
        {
            if (Helper.ValueExistsInDropDown(this.ddlSpecialtyOne, this.SpecialtyOneID.ToString()))
            {
                this.ddlSpecialtyOne.SelectedValue = SpecialtyOneID.ToString();
            }
            LoadSpecialties(ddlSpecialtyTwo, 2);

            if (this.SpecialtyTwoID > 0)
            {
                if (Helper.ValueExistsInDropDown(this.ddlSpecialtyTwo, this.SpecialtyTwoID.ToString()))
                {
                    this.ddlSpecialtyTwo.SelectedValue = SpecialtyTwoID.ToString();
                }
                LoadSpecialties(ddlSpecialtyThree, 3);

                if (this.SpecialtyThreeID > 0)
                {
                    if (Helper.ValueExistsInDropDown(this.ddlSpecialtyThree, this.SpecialtyThreeID.ToString()))
                    {
                        this.ddlSpecialtyThree.SelectedValue = SpecialtyThreeID.ToString();
                    }
                }
            }
        }*/
    }

    private void SetConfirmationDetailEditability()
    {
        bool canEdit = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) &&
            (this.RegStatusTypeID == CON.RegistrationStatusTypeId.Pending || this.RegStatusTypeID == CON.RegistrationStatusTypeId.ReturnToProvider);

        /*this.ddlProvider.Enabled = canEdit;
        this.ddlLicense.Enabled =  canEdit;
        this.ddlSpecialtyOne.Enabled = canEdit && EditableSpecialtyStatus(this.SpecialtyOneStatusID);
        this.ddlSpecialtyTwo.Enabled = canEdit && EditableSpecialtyStatus(this.SpecialtyTwoStatusID);
        this.ddlSpecialtyThree.Enabled =canEdit && EditableSpecialtyStatus(this.SpecialtyThreeStatusID);
        foreach (RepeaterItem item in this.rptAffilQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                TextBox txtComment = (TextBox)item.FindControl("txtResponseComment");

                if (rblConfirmQuestion != null)
                {
                    rblConfirmQuestion.Enabled = canEdit;
                    txtComment.Enabled = canEdit;
                }
            }
        }*/
    }

    private bool EditableSpecialtyStatus(int statusID)
    {
        bool canEdit = false;
        if (statusID == 0 || statusID == CON.GroupAffiliationSpecialtyStatusID.GroupConfirmed || statusID == CON.GroupAffiliationSpecialtyStatusID.GroupConfirmed)
        {
            canEdit = true;
        }
        return canEdit;
    }


    private bool DisplayConfirmationDetails()
    {
        bool displayDetails = false;
        if (this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.ConfirmGroupMember || this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.GroupConfirmed
            || this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.Active)
        {
            displayDetails = true;
        }
        else if (this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.Active)
        {
            displayDetails = true;
        }
        return displayDetails;
    }

    private bool AlreadySentToMMIS()
    {
        bool alreadysent = false;
        if (GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.Active)
        {
            alreadysent = true;
        }
        return alreadysent;
    }

    private int RetroReviewRequired()
    {
        int retroRequired = MAXIMUS.Core.Libraries.Constants.GroupMemberRetroStatusID.RetroReviewNotRequired;
        int days = Convert.ToInt32(AppSettings.Get("AffiliateRetroEffectiveDateWindow"));
        days = days * -1;
        //check for start date leess than 180 days from now bug 5999
        if (Convert.ToDateTime(txtStartDate.Text) < (DateTime.Today.AddDays(days))
            && string.IsNullOrEmpty(endDate) && !AlreadySentToMMIS())
        {
            retroRequired = MAXIMUS.Core.Libraries.Constants.GroupMemberRetroStatusID.RetroReviewRequired;
        }
        return retroRequired;
    }

    private int NewAffiliationStatus()
    {
        int newStatusID = this.GroupAffiliationStatusID;
        if (!string.IsNullOrEmpty(endDate) || txtEndDate.Text != "12/31/2299")
        {
            newStatusID = CON.GroupAffiliationStatusTypeID.PendingRemoval;
        }
        else if (this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.ConfirmGroupMember || (this.GroupAffiliationStatusID == CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval && !string.IsNullOrEmpty(txtMedicaidID.Text)))
        {
            newStatusID = CON.GroupAffiliationStatusTypeID.GroupConfirmed;
        }
        return newStatusID;
    }

    private void SetConfirmationDetails(DataSet affiliationData)
    {
        /*DataRow specialty1Row = affiliationData.Tables[0].Rows[0];
        DataRow specialty2Row = affiliationData.Tables[0].Rows.Count > 1 ? affiliationData.Tables[0].Rows[1] : null;
        DataRow specialty3Row = affiliationData.Tables[0].Rows.Count > 2 ? affiliationData.Tables[0].Rows[2] : null;

        LicensureID = Helper.GetInt("LICENSURE_ID", specialty1Row);

        this.SpecialtyOneID = Helper.GetInt("SPECIALTY_TYPE_ID", specialty1Row);
        this.SpecialtyOneStatusID = Helper.GetInt("GROUP_AFFILIATION_SPECIALTY_STATUS_ID", specialty1Row);

        this.SpecialtyTwoID = this.SpecialtyThreeID = 0;

        if (specialty2Row != null)
        {
            this.SpecialtyTwoID = Helper.GetInt("SPECIALTY_TYPE_ID", specialty2Row);
            this.SpecialtyTwoStatusID = Helper.GetInt("GROUP_AFFILIATION_SPECIALTY_STATUS_ID", specialty2Row);
        }
        if (specialty3Row != null)
        {
            this.SpecialtyThreeID = Helper.GetInt("SPECIALTY_TYPE_ID", specialty3Row);
            this.SpecialtyThreeStatusID = Helper.GetInt("GROUP_AFFILIATION_SPECIALTY_STATUS_ID", specialty3Row);
        }*/

        // this.LoadGroupMbrProfileMatches();
        //If have party id, trickle down load of the dependent drop downs
        /*if (Helper.ValueExistsInDropDown(this.ddlProvider, PartyID.ToString()))
        {
            this.ddlProvider.SelectedValue = PartyID.ToString();
        }

        this.LoadRegAffiliationQuestions();

        //get Question Answers
        DataSet ds = svc.SelectRegAffiliationData(RegAffiliationID, "QUESTION");
        //set Answers
        if (Helper.HasRows(ds))
        {
            this.SetRegAffilAnswers(ds);
        }*/

    }

    private void LoadRegAffiliationQuestions()
    {
        DataSet ds = svc.SelectQuestionTypesByIDBeginsWith("GA");  //the question type id in this table i=serves as unique identifier and type of question.
        /*this.rptAffilQuestions.DataSource = ds;
        this.rptAffilQuestions.DataBind();*/
    }



    private void LoadGroupMbrProfileMatches()
    {
        //DataSet ds = svc.SelectGroupMemberProvidersByAffiliationID(RegAffiliationID);
        //if(Helper.HasRows(ds))
        //this.PartyID = Helper.GetInt("PartyID", ds.Tables[0].Rows[0]);
        /*this.ddlProvider.DataSource = ds;
        this.ddlProvider.DataValueField = "PartyID";
        this.ddlProvider.DataTextField = "LongProviderName";
        this.ddlProvider.DataBind();
        this.ddlProvider.Items.Insert(0, new ListItem("", "0"));*/
    }





    private void SetRegAffilAnswers(DataSet ds)
    {
        DataTable dt = ds.Tables[0];

        /*foreach (RepeaterItem item in this.rptAffilQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");
                TextBox txtComment = (TextBox)item.FindControl("txtResponseComment");

                if (lblTypeId != null)
                {
                    if (dt.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).Count() > 0)
                    {
                        DataTable dtTemp = dt.Select(string.Format("QUESTION_TYPE_ID = '{0}'", lblTypeId.Text)).CopyToDataTable();
                        bool yesNo = Helper.GetBool("RESPONSE", dtTemp.Rows[0]);
                        rblConfirmQuestion.SelectedIndex = yesNo ? 1 : 0;
                        txtComment.Text = Helper.GetString("RESPONSE_COMMENT", dtTemp.Rows[0]);
                    }
                }
            }
        }*/
    }

    private DataSet GetRegAffiliation()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_AFFILIATION_ID", this.RegAffiliationID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_AFFILIATIONByAffiliationID", parms);

        return ds;
    }

    private void UpdateRegAffiliation()
    {
        try
        {
            Dictionary<string, string> parms = this.SetCommonParms();
            Dictionary<string, string> parmsForORP = this.SetCommonParmsForORP();
            Dictionary<string, string> parmsForTerminated = this.SetCommonParmsForTerminated();

            int newStatus = NewAffiliationStatus();
            if (newStatus == CON.GroupAffiliationStatusTypeID.PendingRemoval)
                parms.Add("GROUP_AFFILIATION_STATUS_ID", newStatus.ToString());
            else if (NewGroupAffiliationStatusID > 0)
            {
                parms.Add("GROUP_AFFILIATION_STATUS_ID", NewGroupAffiliationStatusID.ToString());
                NewGroupAffiliationStatusID = 0;
            }            
                
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            
            parms.Add("GrpRenderingLocation", ddlRenderingLocations.SelectedValue.ToString());
            parms.Add("DirectoryOptOut", chkDirectoryOptOut.Checked ? "1" : "0");
            parms["RETRO_REVIEW_REQUIRED_ID"] = RetroReviewRequired().ToString();

            parms.Add("REG_AFFILIATION_ID", this.RegAffiliationID.ToString());
            DataSet DsDuplicateForORP = new DataSet();
            DataSet DsTerminated = new DataSet();
            DsDuplicateForORP = svc.SelectRegistrationDataWithParams("Usp_SelectREGAffilationForORP", parmsForORP);
            DsTerminated = svc.SelectRegistrationDataWithParams("Usp_SelectDsTerminated", parmsForTerminated);

            if (Helper.HasRows(DsDuplicateForORP))
            {
                Errors.Add("InsertRegAffiliation2", "This Individual is currently an ORP Provider and cannot be affiliated at this time");
                return;
            }
            else if (Helper.HasRows(DsTerminated))
            {
                Errors.Add("InsertRegAffiliation2", "Unable to Update. Terminated Providers not allowed");
                return;
            }
            else
            {

                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATIONcustom", parms);
            }

        }
        catch (Exception ex)
        {
            Errors.Add("UpdateRegAffiliation1", ex.Message);
        }
    }


    private void UpdateRegAffiliationConfirmation()
    {
        Errors.Clear();
        //this.UpdateQuestions();

        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return;
        }

        this.UpdateRegAffiliation();
        if (Errors.Count > 0)
        {
            SetErrorMessages();
            return;
        }
    }

    private void UpdateQuestions()
    {
        /*Dictionary<string, string> parms = new Dictionary<string,string>();
        parms.Add("REG_AFFILIATION_ID", RegAffiliationID.ToString());

        try
        {
            foreach (RepeaterItem item in this.rptAffilQuestions.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    Label qTypeID = item.FindControl("lblQuestionTypeID") as Label;
                    RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                    TextBox txtComment = (TextBox)item.FindControl("txtResponseComment");
                    int nbr = 0;

                    if (qTypeID != null && rblConfirmQuestion != null && txtComment != null)
                    {
                        nbr = item.ItemIndex + 1;
                        parms.Add("QUESTION_TYPE_ID_0" + nbr.ToString(),  qTypeID.Text);
                        parms.Add("REPONSE_0" + nbr.ToString(), rblConfirmQuestion.SelectedValue);
                        parms.Add("RESPONSE_COMMENT_0" + nbr.ToString(),   txtComment.Text.Trim());
                    }
                }
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            //SAVE QUESTIONS 
            svc.InsertUpdateRegAffiliationQuestions(RegAffiliationID, parms);

        }
        catch(Exception ex)
        {
            Errors.Add("UpdateQuestions01", ex.Message);
        }

        return;*/
    }

    private void InsertRegAffiliation()
    {
        try
        {
            if (this.WorkflowPage.RegistrationId == 0)
            {
                Errors.Add("InsertRegAffiliation2", "Session Reg ID is null, unable to proceed with insert.");
                return;
            }
            Dictionary<string, string> parms = this.SetCommonParms();
            Dictionary<string, string> parmsdup = this.SetCommonParmsdup();
            Dictionary<string, string> parmsForORP = this.SetCommonParmsForORP();
            Dictionary<string, string> parmsForTerminated = this.SetCommonParmsForTerminated();
            //if (CheckIfIndividualExists())
            //{
            //    parms["PARTY_ID"] = PartyID.ToString();
            //}
            string RedFlag = ddlRenderingLocations.SelectedItem.Attributes["RedFlag"];
            if (NewGroupAffiliationStatusID == 0)
            {                
                if (string.IsNullOrEmpty(txtMedicaidID.Text))
                {                        
                   parms.Add("GROUP_AFFILIATION_STATUS_ID", MAXIMUS.Core.Libraries.Constants.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval.ToString());  //default 
                }                    
                else
                {
                    if (RedFlag == "Yes")
                    {
                        parms.Add("GROUP_AFFILIATION_STATUS_ID", MAXIMUS.Core.Libraries.Constants.GroupAffiliationStatusTypeID.PendingRemoval.ToString());
                    }
                    else
                    {
                        parms.Add("GROUP_AFFILIATION_STATUS_ID", MAXIMUS.Core.Libraries.Constants.GroupAffiliationStatusTypeID.GroupConfirmed.ToString());
                    }
                    
                }
                    
            }
            else
            {
                if (RedFlag == "Yes")
                {
                    parms.Add("GROUP_AFFILIATION_STATUS_ID", MAXIMUS.Core.Libraries.Constants.GroupAffiliationStatusTypeID.PendingRemoval.ToString());
                }
                else
                {
                    parms.Add("GROUP_AFFILIATION_STATUS_ID", NewGroupAffiliationStatusID.ToString());
                }
                
                NewGroupAffiliationStatusID = 0;
            }


            int days = Convert.ToInt32(AppSettings.Get("AffiliateRetroEffectiveDateWindow"));
            days = days * -1;
            //check for start date leess than 180 days from now bug 5999
            if (Convert.ToDateTime(txtStartDate.Text) < (DateTime.Today.AddDays(days)))
            {
                parms["RETRO_REVIEW_REQUIRED_ID"] = MAXIMUS.Core.Libraries.Constants.GroupMemberRetroStatusID.RetroReviewRequired.ToString();
            }
            else
            {
                parms.Add("RETRO_REVIEW_REQUIRED_ID", MAXIMUS.Core.Libraries.Constants.GroupMemberRetroStatusID.RetroReviewNotRequired.ToString());
            }
            parms.Add("GrpRenderingLocation", ddlRenderingLocations.SelectedValue.ToString());
            parms.Add("DirectoryOptOut", chkDirectoryOptOut.Checked ? "1" : "0");
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
            parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
            parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parmsdup.Add("GrpRenderingLocation", ddlRenderingLocations.SelectedValue.ToString());
            DataSet DsDuplicateForORP = new DataSet();
            DataSet DsTerminated = new DataSet();
            DsDuplicateForORP = svc.SelectRegistrationDataWithParams("Usp_SelectREGAffilationForORP", parmsForORP);
            DsTerminated = svc.SelectRegistrationDataWithParams("Usp_SelectDsTerminated", parmsForTerminated);
            if (Helper.HasRows(DsDuplicateForORP))
            {
                Errors.Add("InsertRegAffiliation2", "This Individual is currently an ORP Provider and cannot be affiliated at this time");
                return;
            }
            else if (Helper.HasRows(DsTerminated))
            {
                Errors.Add("InsertRegAffiliation2", "Unable to insert. Terminated Providers not allowed");
                return;
            }
            else
            {
                int regAffiliationID = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATIONcustom", parms);
                this.RegAffiliationID = regAffiliationID;
            }

        }
        catch (Exception ex)
        {
            Errors.Add("InsertRegAffiliation1", ex.Message);
        }
    }

    private bool CheckIfIndividualExists()
    {
        bool rtn = false;
        DataTable dt = GetExistingProviders();
        if (dt != null && dt.Rows.Count > 0)
        {
            string medicaidId = Helper.GetString("MedicaidID", dt.Rows[0]);
            if (!string.IsNullOrEmpty(medicaidId))
                rtn = true;
        }

        return rtn;
    }

    private DataTable GetExistingProviders()
    {
        DataTable dt = null;
        DataSet ds = svc.SelectRegistrationsByNPI(txtNPI.Text);
        if (Methods.HasRows(ds))
        {
            dt = FilteredForKeyFields(ds.Tables[0]);
        }
        return dt;
    }

    private DataTable FilteredForKeyFields(DataTable dt)
    {
        //this has gotten hairy - refactor
        try
        {
            StringBuilder selectPart = new StringBuilder();

            if (!string.IsNullOrEmpty(txtNPI.Text))
            {
                selectPart.Append(string.Format("NPI = '{0}'", txtNPI.Text));
            }

            if (!string.IsNullOrEmpty(txtMedicaidID.Text))
            {
                if (selectPart.Length > 0)
                {
                    selectPart.Append(" AND ");
                }
                selectPart.Append(string.Format("MedicaidID = '{0}'", txtMedicaidID.Text));
            }

            if (dt.Select(selectPart.ToString()).Count() > 0)
            {
                return dt.Select(selectPart.ToString()).CopyToDataTable();
            }
        }
        catch (Exception ex)
        {
            //this.ErrorList.Add(NextErrorKey(), ex.Message);
        }
        return null;
    }

    private void SetErrorMessages()
    {

        if (Errors.Count > 0)
        {
            ClearErrorMessages();
            SetErrors();
        }

        if (ErrorEvent != null)
        {
            //have to keep the pop up open
            ErrorEvent();
            return;
        }
    }

    private void SetErrors()
    {
        foreach (var pair in Errors)
        {
            AddErrorMessage(pair.Value);
        }
    }

    private void AddErrorMessage(string message)
    {
        lblErrorMessages.Text = string.Format("{0}{1}<br />", lblErrorMessages.Text, message);
    }

    private void ClearErrorMessages()
    {
        lblErrorMessages.Text = string.Empty;
    }

    #region Validation


    protected void Validate_RetroStartDate(object sender, ServerValidateEventArgs e)
    {
        bool isValid = true;
        //NOTE: this.ParentEffectiveDate = ISNULL(grpprov.CHANGE_EFFECTIVE_DATE, grpprov.REQUESTED_EFFECTIVE_DATE) as GroupEffectiveDate
        if (this.ParentEffectiveDate.HasValue && !string.IsNullOrEmpty(this.ParentEnrollmentStatus))
        {
            if (Convert.ToDateTime(txtStartDate.Text) < this.ParentEffectiveDate.Value.Date)
            {
                isValid = false;
            }
        }

        e.IsValid = isValid;
    }


    //protected void Validate_NPI_IsUnique(object sender, ServerValidateEventArgs e)
    //{
    //    if (dt == null) SetDt();
    //    if (dt == null)
    //    {
    //        e.IsValid = true;
    //    }
    //    else
    //    {
    //        string filterString = string.Format("NPI='{0}' AND REG_AFFILIATION_ID <> {1}", txtNPI.Text.Trim(), this.RegAffiliationID);
    //        DataRow[] rows = dt.Select(filterString);
    //        e.IsValid = rows.Length == 0;
    //    }
    //}


    protected void ValidateQuestionsAnswered(object sender, ServerValidateEventArgs e)
    {
        bool isValid = true;
        /*foreach (RepeaterItem item in this.rptAffilQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");

                if (rblConfirmQuestion.SelectedIndex == -1)
                {
                    isValid = false;
                    break;
                }
            }
        }*/
        e.IsValid = isValid;
    }


    protected void ValidateCommentEntered(object sender, ServerValidateEventArgs e)
    {
        /*bool isValid = true;
        foreach (RepeaterItem item in this.rptAffilQuestions.Items)
        {
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                RadioButtonList rblConfirmQuestion = (RadioButtonList)item.FindControl("rblConfirmQuestion");
                Label lblTypeId = (Label)item.FindControl("lblQuestionTypeID");

                if (rblConfirmQuestion.SelectedIndex != -1)
                {
                    if ((lblTypeId.Text == "GA05" && rblConfirmQuestion.SelectedValue == "0") || (lblTypeId.Text != "GA05" && rblConfirmQuestion.SelectedValue == "1"))
                    {
                        //only ga05 requires a comment if answer no. 'In compliance with Title 8 U.S.C. &sect;1324a, has employment eligibility been verified for this individual?'
                        TextBox txtResponseComment = (TextBox)item.FindControl("txtResponseComment");
                        if (string.IsNullOrEmpty(txtResponseComment.Text.Trim()))
                        {
                            isValid = false;
                            break;
                        }
                    }
                }
            }
        }
        e.IsValid = isValid;*/
    }





    public string SelectAppSetting(string appSettingKey)
    {
        return AppSettings.Get(appSettingKey, string.Empty);
    }
    #endregion

    protected void Validate_MedicaidID(object source, ServerValidateEventArgs e)
    {
        SetDtMatch();
        if (dtMatch == null)
        {
            e.IsValid = true;
        }
        else
        {
            string filterString = string.Format("NPI='{0}' AND MEDICAID_ID = '{1}'", txtNPI.Text.Trim(), txtMedicaidID.Text);
            DataRow[] rows = dtMatch.Select(filterString);
            e.IsValid = rows.Length > 0;
        }
    }
	
    private bool checkForBlankRenderingLocationsForMedicaidId(DataSet ds, string medicaidId)
    {
        DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

        // go through all current rows to make sure there's not a row for this medicaid ID with a blank rendering location; if so, they need to fix this first
        if (Helper.HasRows(dt))
        {
            // see if there is an active affliation row with the same medicaid id missing a grprenderinglocation that isn't my own (since I could be editing myself and adding that at this point)
            var rowThatNeedsFixing = (from q in ds.Tables[0].AsEnumerable()
                                      where (q["GrpRenderingLocation"] == DBNull.Value || q.Field<int?>("GrpRenderingLocation") == null) &&
                                        q.Field<int>("GROUP_AFFILIATION_STATUS_ID") != CON.GroupAffiliationStatusTypeID.RemovedbyGroup &&
                                        q.Field<string>("MedicaidID") == medicaidId &&
                                        q.Field<int>("REG_AFFILIATION_ID") != RegAffiliationID
                                      select q.Field<int>("REG_AFFILIATION_ID")).FirstOrDefault();
            if (rowThatNeedsFixing > 0)
            {
                // found one that matches; need to ask them to put in a grpRenderingLocation for the current row, so we can later see if there are overlapping affiliations
                return true;
            }
        }

        return false;
    }

    private bool checkForOverlappingSpansOrBlankLocations(string medicaidId)
    {
        bool thingsAreOk = true;

        // check and see if this affiliation would overlap any others for this group - Do not allow overlapping affiliation at the same rendering location
        // Also affiliations in a group must be unique, that means combination of REG_ID, MEDICAID_ID, START_DATE, END_DATE and GrpRenderingLocation must be unique.

        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATION");
        DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;

        if (Helper.HasRows(dt))
        {
            // OHPNM-9813 - See if there is an affiliation for this provider with the same provider but with a blank rendering location
            // if so, tell them to go enter a rendering location for that other active affiliation, so we can check to make sure it's not an overlapping one
            if (this.checkForBlankRenderingLocationsForMedicaidId(ds, medicaidId))
            {
                AddError("Please update the rendering location on the active Affiliation record before adding a new affiliation", ref thingsAreOk);
                return thingsAreOk;
            }

            // set values of the name and npi we are trying to save to use for comparison below
            string nameToTest = string.Concat(txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim()).ToLower();
            string npiToTest = txtNPI.Text.Trim();

            var rowThatIsADuplicate = (from q in ds.Tables[0].AsEnumerable()
                                      where 
                                        (
                                             (q["GrpRenderingLocation"] == DBNull.Value || q.Field<int?>("GrpRenderingLocation") == null) ||
                                             (q.Field<int?>("GrpRenderingLocation") != null && q.Field<int>("GrpRenderingLocation") == Convert.ToInt32(ddlRenderingLocations.SelectedValue.ToString()))
                                        ) &&
                                        ((q.Field<string>("MedicaidID") == medicaidId) || (q.Field<string>("NAME") == nameToTest && q.Field<string>("NPI") == npiToTest)) &&
                                        (
                                            (Convert.ToDateTime(txtStartDate.Text) >= q.Field<DateTime>("START_DATE") && Convert.ToDateTime(txtStartDate.Text) <= q.Field<DateTime>("END_DATE")) ||
                                            (Convert.ToDateTime(txtEndDate.Text) >= q.Field<DateTime>("START_DATE") && Convert.ToDateTime(txtEndDate.Text) <= q.Field<DateTime>("END_DATE"))
                                        ) &&
                                        q.Field<int>("REG_AFFILIATION_ID") != RegAffiliationID
                                      select q.Field<int>("REG_AFFILIATION_ID")).FirstOrDefault();

            if (rowThatIsADuplicate > 0)
            {
                AddError("Unable to add this affiliation as there is already an existing span for this Rendering Location", ref thingsAreOk);
                return thingsAreOk;
            }
        }

        return thingsAreOk;
    }

    private bool ValidateData()
    {
        bool rtn = true;

        //if start date is empty when editing
        if (string.IsNullOrEmpty(txtStartDate.Text))
        {
            AddError("Start date cannot be empty", ref rtn);
            return rtn;
        }
        DateTime renderingLocEndDateTime = Convert.ToDateTime (ddlRenderingLocations.SelectedItem.Attributes["ADR_END_DATE"]);
        if (renderingLocEndDateTime < Convert.ToDateTime(txtStartDate.Text))
        {
            AddError("End date of rendering location is prior to the start date of provider at this location", ref rtn);
            return rtn;
        }

        // if they are editing an affiliation, make sure they aren't picking a date past the current start date
        if (RegAffiliationID > 0)
        {
            if (Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(AffiliationStartDate))
            {
                AddError("Affiliation start date cannot be moved to a later date", ref rtn);

                return rtn;
            }

            //OHPNM-8402 -end date cannot be less than start date
            if (Convert.ToDateTime(txtEndDate.Text) < Convert.ToDateTime(txtStartDate.Text))
            {
                AddError("End date cannot be less than the start date", ref rtn);
                return rtn;
            }

        }

        dtIndividuals = GetExistingProviders();
        IndividualMedicaidID = string.Empty;
        string endDate = string.Empty;
        string TerminationDate = string.Empty;
        int revalidationPeriod = 0;
        if (Helper.HasRows(dtIndividuals))  // If we find any providers
        {
            if (!string.IsNullOrEmpty(Helper.GetString("end_date", dtIndividuals.Rows[0])))
            {
                endDate = Helper.GetString("end_date", dtIndividuals.Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("TerminationDate", dtIndividuals.Rows[0])))
            {
                TerminationDate = Helper.GetString("TerminationDate", dtIndividuals.Rows[0]);
            }
            string revalDueWindow = DataAccess.GetAppSetting("RevalidationDueWindow");
            revalidationPeriod = Convert.ToInt32(revalDueWindow) * -1;
            bool revalidationNeeded = string.IsNullOrEmpty(TerminationDate) && (!string.IsNullOrEmpty(endDate) && Convert.ToDateTime(endDate).AddDays(revalidationPeriod) <= DateTime.Today);

            if (!string.IsNullOrEmpty(Helper.GetString("medicaidid", dtIndividuals.Rows[0])))
            {
                IndividualMedicaidID = Helper.GetString("medicaidid", dtIndividuals.Rows[0]);
            }
            if (dtIndividuals.Rows.Count == 1)
            { // we have only one individual found (someone tried to enter group NPI)
                if (Helper.GetInt("entity_type_id", dtIndividuals.Rows[0]) != CON.ProviderCategoryTypeID.Individual)
                {
                    AddError("The NPI does not match with an Individual provider.", ref rtn);
                    return rtn;
                }
                if (Helper.GetString("MMISProviderTypeID", dtIndividuals.Rows[0]) == AppSettings.Get("PharmacistProviderMMISTypeID"))
                {
                    if (!AppSettings.Get("AllowedGroupProviderTypesAffiliation").Contains(this.WorkflowPage.MMISProviderTypeID))
                    {
                        AddError("Pharmacist provider cannot be added to this group.", ref rtn);
                        return rtn;
                    }
                }
                if (!CheckParentEffectiveDate(ref rtn))
                {
                    return rtn;
                }

                // OHPNM-9813 - see if we have overlapping spans or a grp rendering location that is blank for this medicaid ID that they need to fix
                bool thingsAreOkWithAffiliations = this.checkForOverlappingSpansOrBlankLocations(IndividualMedicaidID);
                if (!thingsAreOkWithAffiliations)
                {
                    return thingsAreOkWithAffiliations;
                }

                if (!string.IsNullOrEmpty(IndividualMedicaidID))
                { // case for providers with the medicaid id and enrollment code is 00. This covers converted and non-converted individuals in the system
                    // they are set confirmed status
                    if (Helper.GetDateTime("change_effective_date", dtIndividuals.Rows[0]).Date > Convert.ToDateTime(txtStartDate.Text))
                    {
                        AddError("Start Date cannot be prior to the Individual's Effective Date", ref rtn);

                        return rtn;
                    }
                    else if (!CheckIndividualStandardSpanStartDate(IndividualMedicaidID, ref rtn)) // OHPNM-18515 Not allow affiliations prior to ORP providers becoming Standard
                    {                       
                        return rtn;
                    }
                    else if (Helper.GetString("enrollment_status_code", dtIndividuals.Rows[0]) != CON.EnrollmentStatusTypeID.InActive.ToString() && revalidationNeeded)
                    {
                        NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualRequiresReValidation;
                        return rtn;
                    }
                    else if (Helper.GetString("enrollment_status_code", dtIndividuals.Rows[0]) == CON.EnrollmentStatusTypeID.InActive.ToString())
                    { // case for providers with medicaid id and enrollment code not 00. These are converted and non-converted individuals who are expired in MMIS. 
                      // They need to come and re-activate                                                
                        NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualRequiresReValidation;
                        showMessage = "Individual is currently not an active provider.  Individual must be reactivated before confirming";
                        return rtn;

                    }
                    else if (!Helper.IsDateNull(renderingLocEndDateTime) && renderingLocEndDateTime <= DateTime.Now)
                    {
                        NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.PendingRemoval;
                        return rtn;
                    }
                    else if (Helper.GetDateTime("change_effective_date", dtIndividuals.Rows[0]).Date <= Convert.ToDateTime(txtStartDate.Text))
                    {
                        NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.GroupConfirmed;
                        return rtn;
                    }


                }

                else
                {  // we dont get medicaid id for the provider. That means the individual needs to finish his registration
                    if (string.IsNullOrEmpty(Helper.GetString("change_effective_date", dtIndividuals.Rows[0])) && Convert.ToDateTime(txtStartDate.Text) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        AddError("Start Date cannot be less than current date", ref rtn);

                        return rtn;
                    }
                    else if (string.IsNullOrEmpty(Helper.GetString("change_effective_date", dtIndividuals.Rows[0])) && Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    {
                        if (string.IsNullOrEmpty(Helper.GetString("medicaidid", dtIndividuals.Rows[0])))
                            NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval;
                        return rtn;
                    }
                    if (!string.IsNullOrEmpty(Helper.GetString("change_effective_date", dtIndividuals.Rows[0])) && Convert.ToDateTime(txtStartDate.Text) < Helper.GetDateTime("change_effective_date", dtIndividuals.Rows[0]).Date)
                    {
                        AddError("Start Date cannot be prior to the Individual's Effective Date", ref rtn);

                        return rtn;
                    }
                    else if (!string.IsNullOrEmpty(Helper.GetString("change_effective_date", dtIndividuals.Rows[0])) && Convert.ToDateTime(txtStartDate.Text) > Helper.GetDateTime("change_effective_date", dtIndividuals.Rows[0]).Date)
                    {
                        NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval;
                        return rtn;
                    }

                }

            }

            else if (dtIndividuals.Rows.Count > 1)
            { // more than one provider found 

                NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval;
                AddError("More than 1 provider exists with this NPI.  Please enter the Individual's Medicaid ID or contact the MAXIMUS Help Desk", ref rtn);
                return rtn;

            }
        }
        if (string.IsNullOrEmpty(IndividualMedicaidID) && dtIndividuals == null)
        { // we didnt find a provider with the combination they put in. We are searching by only npi and medicaid id
            NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.MemberNotFound;
            return rtn;
        }

        if (dtIndividuals == null)
        { // if the provider didnt put the medicaid id, that mean the individual didnt come to the system yet. We mark the registration pending apprval
            if (!CheckParentEffectiveDate(ref rtn))
            {
                return rtn;
            }
            if (Convert.ToDateTime(txtStartDate.Text) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
            {
                AddError("Start Date cannot be less than current date", ref rtn);

                return rtn;
            }

            NewGroupAffiliationStatusID = CON.GroupAffiliationStatusTypeID.IndividualEnrollmentPendingApproval;
        }
        return rtn;

    }
    private bool CheckParentEffectiveDate(ref bool rtn)
    {
        if (ParentEffectiveDate != null)
        {
            if (Convert.ToDateTime(txtStartDate.Text) < Convert.ToDateTime(ParentEffectiveDate.GetValueOrDefault().ToShortDateString()))
            {
                AddError("Start Date cannot be less than Effective date of the group", ref rtn);

                return false;
            }
        }

        return true;
    }
    private bool CheckIndividualStandardSpanStartDate(string medicaidID, ref bool rtn)
    {
        DataSet ds = svc.GetIndividualStandardSpanStartDate(medicaidID);
        if (Methods.HasRows(ds))
        {
            DateTime stdSpanStartDate = Helper.GetDateTime("STD_SPAN_START_DATE_TIME", ds.Tables[0].Rows[0]);

            if (Convert.ToDateTime(txtStartDate.Text) < stdSpanStartDate)
            {
                AddError("Start Date cannot be less than Individuals Standard enrollment span effective date.", ref rtn);

                return false;
            }
        }

        return true;
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "GroupAffiliations";
        this.Page.Validators.Add(val);
        isGood = false;
    }
}