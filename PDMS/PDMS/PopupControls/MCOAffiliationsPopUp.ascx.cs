using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class PopupControls_MCOAffiliationsPopUp : BasePopupControl
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

    #region Parent Page Events
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
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

    public int RegMCOAffiliationID
    {
        get
        {
            return ViewState["RegMCOAffiliationID"] == null ? 0 : Convert.ToInt32(ViewState["RegMCOAffiliationID"]);
        }
        set
        {
            ViewState["RegMCOAffiliationID"] = value;
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


    public int PartyID
    {
        get
        {
            return ViewState["PartyID"] == null ? 0 : Convert.ToInt32(ViewState["PartyID"]);
        }
        set
        {
            ViewState["PartyID"] = value;
        }
    }

    public int MCOAffiliationstatusTypeID
    {
        get
        {
            return ViewState["MCOAffiliationstatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["MCOAffiliationstatusTypeID"]);
        }
        set
        {
            ViewState["MCOAffiliationstatusTypeID"] = value;
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

    public DateTime? MCOEffectiveDate
    {
        get
        {
            if (ViewState["MCOEffectiveDate"] != null)
            {
                return Convert.ToDateTime(ViewState["MCOEffectiveDate"]);
            }
            return null;
        }
        set
        {
            ViewState["MCOEffectiveDate"] = value;
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
    public DateTime? TerminationDate
    {
        get
        {
            if (ViewState["TerminationDate"] != null)
            {
                return Convert.ToDateTime(ViewState["TerminationDate"]);
            }
            return null;
        }
        set
        {
            ViewState["TerminationDate"] = value;
        }
    }
    
    private Dictionary<string, string> Errors = new Dictionary<string, string>();



    private DataTable dtMatch;
    private DataTable dtIndividuals;
    public int NewMCOAffiliationstatusTypeID
    {
        get
        {
            return ViewState["NewMCOAffiliationstatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["NewMCOAffiliationstatusTypeID"]);
        }
        set
        {
            ViewState["NewMCOAffiliationstatusTypeID"] = value;
        }
    }
    private string IndividualMedicaidID
    {
        get
        {
            return ViewState["IndividualMedicaidID"] == null ? string.Empty: Convert.ToString(ViewState["IndividualMedicaidID"]);
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
    public int MCOPartyID
    {
        get
        {
            return ViewState["MCOPartyID"] == null ? 0 : Convert.ToInt32(ViewState["MCOPartyID"]);
        }
        set
        {
            ViewState["MCOPartyID"] = value;
        }
    }
    public string MCOMedicaidID
    {
        get
        {
            return ViewState["MCOMedicaidID"] == null ? string.Empty : Convert.ToString(ViewState["MCOMedicaidID"]);
        }
        set
        {
            ViewState["MCOMedicaidID"] = value;
        }
    }
    public int RegistrationProgramStatusTypeID
    {
        get
        {
            return ViewState["RegistrationProgramStatusTypeID"] == null ? 0 : Convert.ToInt32(ViewState["RegistrationProgramStatusTypeID"]);
        }
        set
        {
            ViewState["RegistrationProgramStatusTypeID"] = value;
        }
    }
    private void SetDtMatch()
    {
        if (dtMatch == null)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            
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
    public string Sl_Tracking_Number
    {
        get
        {
            return ViewState["Sl_Tracking_Number"] == null ? "0" : ViewState["Sl_Tracking_Number"].ToString();
        }
        set
        {
            ViewState["Sl_Tracking_Number"] = value;
        }
    }
    #endregion


    protected void cvGroupAffiliation_ServerValidate(object source, ServerValidateEventArgs args)
    {
        if (!string.IsNullOrEmpty(showMessage) && hdnGroupAffiliationConfirm.Value != "1")
        {
            //lblMessage.Text = showMessage;
           
            lblmessage.Text = showMessage;
            
            
            divConfirmGroupAffiliation.Style.Add("display", "block");
            divConfirmGroupAffiliation.Visible = true;
            lblmessage.Visible = true;
            hdnGroupAffiliationConfirm.Value = "1";


            args.IsValid = false;

        }
        else
        {
            lblmessage.Visible = divConfirmGroupAffiliation.Visible = false;
            
            divConfirmGroupAffiliation.Style.Add("display", "none");
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }
    public void LoadData(int RegMCOAffiliationID,string typ)
    {
        //this.InitFormFields();
        hdnGroupAffiliationConfirm.Value = "0";
        this.RegMCOAffiliationID = RegMCOAffiliationID;


        //this.SetInitialFieldVisibility();

        if (RegMCOAffiliationID > 0)
        {
            DataSet affiliationData = null;
            if (typ == "MCP")
                affiliationData = this.GetRegAffiliation(RegMCOAffiliationID);
            else if(typ == "PGMCP")
                affiliationData = this.GetRegPGAffiliation(RegMCOAffiliationID);

            this.SetFormFields(affiliationData);
        }
        else
        {
            //GetEffectiveDate();
        }

        //this.SetEditabilityByAffilitationStatus();


        //loadProviderTypes();
    }


    //    protected void btnSave_Click(object sender, EventArgs e)
    //    {
    //        if (ValidateData())
    //        {



    //            //showMessage = string.Empty;
    //            Page.Validate("MCOAffiliations");

    //            if (!Page.IsValid)
    //            {
    //                if (ValidationEvent != null)
    //                {
    //                    ValidationEvent();
    //                }
    //                return;
    //            }

    //            if (this.MCOAffiliationstatusTypeID == CON.MCOAffiliationstatusTypeID.Active)
    //            {
    //                //only thing they can set in this status is end date, so if end date is empty cancel out
    //                if (string.IsNullOrEmpty(txtEndDate.Text))
    //                {
    //                    if (CancelEvent != null)
    //                    {
    //                        CancelEvent();
    //                    }
    //                    return;
    //                }
    //            }

    //            bool rtn = SaveData();
    //            if (!rtn)
    //                return;

    //            UpdateGrid = true;

    //            //On successful save, reget and check to see if status is Confirm, if so, show confirm area.
    //            //Reget Affiliation to see if can confirm provider.
    //            DataSet affiliationData = this.GetRegAffiliation();
    //            if (!Helper.HasRows(affiliationData))
    //            {
    //                return;
    //            }



    //            if (SaveEvent != null)
    //            {
    //                SaveEvent();
    //            }

    //        }
    //    }


    //    protected void btnSaveEndDate_Click(object sender, EventArgs e)
    //    {
    //        if (ValidateData())
    //        {



    //            Page.Validate("MCOAffiliations");

    //            if (!Page.IsValid)
    //            {
    //                if (ValidationEvent != null)
    //                {
    //                    ValidationEvent();
    //                }
    //                return;
    //            }

    //            bool rtn = SaveData();
    //            if (!rtn)
    //                return;

    //            UpdateGrid = true;
    //            if (SaveEvent != null)
    //            {
    //                SaveEvent();
    //            }
    //        }
    //    }





    //    protected void rptAffilQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //    {
    //        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //        {
    //            Label lblTypeId = (Label)e.Item.FindControl("lblQuestionTypeID");
    //            Label lblCommentReqdInfo = (Label)e.Item.FindControl("lblCommentReqdInfo");

    //            if (lblTypeId != null && lblCommentReqdInfo != null)
    //            {
    //                lblCommentReqdInfo.Text= lblTypeId.Text == "GA05" ? "If 'NO' a comment is required." : "If 'YES' a comment is required.";
    //            }
    //            if ((MCOAffiliationstatusTypeID == CON.MCOAffiliationstatusTypeID.Confirmed))
    //            {
    //                RadioButtonList rblConfirmQuestion = (RadioButtonList)e.Item.FindControl("rblConfirmQuestion");
    //                TextBox txtResponseComment = (TextBox)e.Item.FindControl("txtResponseComment");
    //                rblConfirmQuestion.Enabled = false;
    //                txtResponseComment.Enabled = false;
    //            }

    //        }

    // }

    //    protected void btnSaveConfirm_Click(object sender, EventArgs e)
    //    {


    //            Page.Validate("ConfirmAffiliations");

    //            if (!Page.IsValid)
    //            {
    //                if (ValidationEvent != null)
    //                {
    //                    ValidationEvent();
    //                }
    //                return;
    //            }

    //            //Save the answers and update status to Group Confirmed (5).
    //            this.UpdateRegAffiliationConfirmation();
    //            if (Errors.Count == 0)
    //            {
    //                if (SaveEvent != null)
    //                {
    //                    SaveEvent();
    //                }
    //            }

    //    }
    //    private void GetEffectiveDate()
    //    {

    //        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    //        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");

    //        if (String.IsNullOrEmpty( Helper.GetString("CHANGE_EFFECTIVE_DATE", ds.Tables[0].Rows[0])))
    //        {
    //            this.ParentEffectiveDate = Helper.GetDateTime("REQUESTED_EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
    //        }
    //        else
    //        {
    //            this.ParentEffectiveDate = Helper.GetDateTime("CHANGE_EFFECTIVE_DATE", ds.Tables[0].Rows[0]);
    //        }
    //        if (!string.IsNullOrEmpty(Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0])))
    //            this.ParentEnrollmentStatus = Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0]);

    //        //return true;
    //    }
    //    protected void btnClose_Click(object sender, EventArgs e)
    //    {
    //        if (UpdateGrid)
    //        {
    //            if (SaveEvent != null)
    //                SaveEvent();
    //        }
    //        else
    //        {
    //            if (CancelEvent != null)
    //                CancelEvent();
    //        }

    //    }


    //    #endregion

    //    #region Public Methods

    //    public void LoadData(int RegMCOAffiliationID)
    //    {
    //        this.InitFormFields();
    //        hdnGroupAffiliationConfirm.Value = "0";
    //        this.RegMCOAffiliationID = RegMCOAffiliationID;


    //        //this.SetInitialFieldVisibility();

    //        if (RegMCOAffiliationID > 0)
    //        {

    //            DataSet affiliationData = this.GetRegAffiliation(RegMCOAffiliationID);
    //            this.SetFormFields(affiliationData);
    //        }
    //        else
    //        {
    //            GetEffectiveDate();
    //        }

    //        this.SetEditabilityByAffilitationStatus();


    //        //loadProviderTypes();
    //    }

    //    #endregion

    //    private void SetEditabilityByAffilitationStatus()
    //    {
    //        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
    //        Guid CurrentStepOwnerID = Guid.Empty;
    //        bool operatorUpdate = false;
    //        if (Helper.HasRows(ds))
    //        {
    //            DataRow dr = ds.Tables[0].Rows[0];
    //        if (Methods.ColumnExists("CurrentStepOwnerID", dr))
    //        {
    //            if (dr["CurrentStepOwnerID"] != DBNull.Value)
    //            {
    //                CurrentStepOwnerID = new Guid(Methods.GetStringValue(dr["CurrentStepOwnerID"]));
    //                operatorUpdate = this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry && Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name) && SessionVarRetriever.UserIdSelected == CurrentStepOwnerID.ToString();
    //            }
    //        }
    //        }


    //        bool canEdit = (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)
    //                    && ((RegMCOAffiliationID == 0 || this.RegStatusTypeID == CON.RegistrationStatusTypeId.Pending || this.RegStatusTypeID == CON.RegistrationStatusTypeId.ReturnToProvider) || (RegMCOAffiliationID > 0 && Request.QueryString["AllowEdit"].ToString() == "true"))) || operatorUpdate;
    //        //provider can not modify unless adding new affiliate registration is in return to provider or provider data entry

    //        InitializeFields();
    //        EnableAffilValidators(false);
    //        EnableConfirmValidators(false);
    //        this.divAffiliationSaveBox.Visible = true;
    //        //this.divMemberProfile.Visible = false;
    //       // this.btnSave.Visible = false;



    ////        switch (MCOAffiliationstatusTypeID)
    ////        {
    ////            case CON.MCOAffiliationstatusTypeID.NotSetYet:
    ////                EnableAllFields(canEdit);
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = false;
    ////                this.btnSave.Visible = canEdit;
    ////                EnableAffilValidators(true);
    ////                break;

    ////            case CON.MCOAffiliationstatusTypeID.ProviderEnrollmentPendingApproval:
    ////                EnableAllFields(canEdit);
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = false;
    ////                this.btnSave.Visible = canEdit;
    ////                EnableAffilValidators(canEdit);
    ////                break;

    ////            case CON.MCOAffiliationstatusTypeID.Confirmed:
    ////                //member is confirmed not sent to MMIS yet, hide save at the top, no changes there.
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = false;
    ////                btnSave.Visible = btnSaveConfirm.Visible = btnSaveEndDate.Visible = false;
    ////                //this.divMemberProfile.Visible = true;
    ////                this.btnSave.Visible = false;
    ////                //this.btnSaveConfirm.Visible = canEdit;
    ////                EnableConfirmValidators(canEdit);

    ////                break;

    ////            case CON.MCOAffiliationstatusTypeID.Active:
    //////           case CON.MCOAffiliationstatusTypeID.ActiveModified:
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = true;
    ////                this.txtEndDate.Enabled = canEdit;
    ////                this.btnSaveEndDate.Visible = canEdit;
    ////                this.valEndCustom.Enabled = true;
    ////                //this.divAffiliationSaveBox.Visible =false;
    ////                //this.divMemberProfile.Visible = true;
    ////                this.btnSave.Visible = false;

    ////                EnableConfirmValidators(canEdit && PartyID >0);

    ////                break;
    ////            case CON.MCOAffiliationstatusTypeID.PendingRemoval:
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = true;
    ////                this.txtEndDate.Enabled = canEdit;
    ////                this.valEndCustom.Enabled = canEdit;
    ////                //this.divAffiliationSaveBox.Visible = this.PartyID > 0;
    ////                this.btnSave.Visible = canEdit;
    ////                break;
    ////            default:
    ////                this.tdEndDateLbl.Visible = this.tdEndDateFld.Visible = this.trEndDateHint.Visible = false;
    ////                this.valEndCustom.Enabled = canEdit;
    ////                break;
    ////        }
    //   }



    //    private void InitializeFields()
    //    {


    //        //txtStartDate.Enabled = false;
    //        //txtEndDate.Enabled = false;

    //    }

    //    private void EnableAllFields(bool enable)
    //    {
    //        bool sentToMMIS = AlreadySentToMMIS();



    //        //txtStartDate.Enabled = enable && !sentToMMIS;
    //        //txtEndDate.Enabled = enable && sentToMMIS;

    //    }

    //    private void EnableAffilValidators(bool enable)
    //    {
    //        //this.valFNReqd.Enabled = enable;


    //        //this.valStartReqd.Enabled = enable;
    //        //this.valStartRetroCheck.Enabled = enable;
    //        //this.valEndCustom.Enabled = enable;

    //    }

    //    private void EnableConfirmValidators(bool enable)
    //    {
    //        /*this.valQuestionsAnswered.Enabled = enable;
    //        this.valCommentEntered.Enabled = enable;
    //        this.valLicenseReqd.Enabled = enable && this.ddlLicense.Items.Count > 1;
    //        this.valSpecOneReqd.Enabled = enable;*/
    //    }

    //    private void SetInitialFieldVisibility()
    //    {
    //        bool isVisible = this.RegMCOAffiliationID > 0;



    //        this.divAffiliationSaveBox.Visible = true;
    //        //this.divMemberProfile.Visible = false;
    //    }

    //    private bool SaveData()
    //    {
    //        bool rtn = true;
    //        Errors.Clear();

    //            if (this.RegMCOAffiliationID > 0)
    //            {
    //                this.UpdateRegAffiliation();
    //            }
    //            else
    //            {
    //                this.InsertRegAffiliation();
    //            }

    //            this.Confirmed = false;
    //        if (Errors.Count > 0)
    //        {
    //            SetErrorMessages();
    //            return false;
    //        }

    //        return rtn;
    //    }

    //    private Dictionary<string, string> SetCommonParms()
    //    {
    //        Dictionary<string, string> parms = new Dictionary<string, string>();
    //        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());





    //        parms.Add("START_DATE", txtStartDate.Text);
    //        parms.Add("END_DATE", string.IsNullOrEmpty(txtEndDate.Text) ? null : txtEndDate.Text);
    //        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
    //        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

    //        return parms;
    //    }

    //    private void InitFormFields()
    //    {
    //        this.RegMCOAffiliationID = this.MCOAffiliationstatusTypeID =  0;
    //        this.UpdateGrid = false;


    //        txtStartDate.Text = DateTime.Now.ToShortDateString();
    //        txtEndDate.Text = string.Empty;

    //        txtMedicaidID.Text = string.Empty;
    //        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
    //        DataSet ds = psc.SelectMCOs();


    //    }

    private void SetFormFields(DataSet affiliationData)
    {
        DataRow affiliationRow = affiliationData.Tables[0].Rows[0];



        txtStartDate.Text = Helper.FormatDate2(affiliationRow["START_DATE"].ToString());
        txtEndDate.Text = Helper.FormatDate2(affiliationRow["END_DATE"].ToString());
        this.txtAgeLimitHigh.Text = Helper.GetString("age_limit_high", affiliationRow);
        this.txtSpecialities.Text = Helper.GetString("MCP_SPECIALITY_NAME", affiliationRow);
        this.txtMitsSpecialiaties.Text = Helper.GetString("MITS_SPECIALITY_NAME", affiliationRow);
        this.txtAgeLimitLow.Text = Helper.GetString("age_limit_Low", affiliationRow);
        this.txtProviderType.Text = Helper.GetString("Provider_name", affiliationRow);
        this.txtgoupAffliation.Text = Helper.GetString("AFFILIATION_TYPE_ID", affiliationRow);
        // this.txtTracingNumber.Text = Helper.GetString("TRACKING_NUMBER", affiliationRow);
        this.txtTracingNumber.Text = Sl_Tracking_Number;
        this.txtTPA.Text = Helper.GetString("TPA_NAME", affiliationRow);
        this.txtProgramCode.Text = Helper.GetString("PROGRAM_CODE", affiliationRow);
        this.txtNewborns.Text = Helper.GetString("ACCEPT_NEW_BORNS", affiliationRow).ToUpper() == "TRUE" ? "Yes" : "No";
        this.txtPanelCapacity.Text = Helper.GetString("PANEL_CAPACITY", affiliationRow);
        this.txtPCPIndicator.Text = Helper.GetString("PANEL_PCP_COUNT", affiliationRow);
        this.txtPregWoman.Text = Helper.GetString("ACCEPT_PREGNANT_WOMAN", affiliationRow).ToUpper() == "TRUE" ? "Yes" : "No";
        this.txtPlanName.Text = Helper.GetString("Name", affiliationRow);
        this.txtComments.Text = Helper.GetString("COMMENTS", affiliationRow);
        this.txtExtPatientsOnly.Text = Helper.GetString("EXISTING_PATIENTS_ONLY", affiliationRow).ToUpper()=="TRUE"?"Yes":"No";
        this.txtFamMembers.Text = Helper.GetString("ACCEPT_FAMILY_MEMBERS", affiliationRow).ToUpper() == "TRUE" ? "Yes" : "No";
        this.txtLanSpoken.Text = Helper.GetString("LANGUAGES_SPOKEN", affiliationRow);
        this.txtMedicaidID.Text = Helper.GetString("MEDICAID_ID", affiliationRow);
        this.txtProviderType.Text = Helper.GetString("MITS_Provider_Type", affiliationRow);
        this.txtPlanName.Text = Helper.GetString("plan_name", affiliationRow);
        
    }

    private DataSet GetRegAffiliation(int? RegMCOAffiliationID = null)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_MCO_AFFILIATION_ID", RegMCOAffiliationID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_MCO_AFFILIATIONByMCOAffiliationID", parms);

        return ds;
    }
    private DataSet GetRegPGAffiliation(int? RegMCOPGAffiliationID = null)
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_MCO_PG_AFFILIATION_ID", RegMCOPGAffiliationID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_MCO_PG_AFFILIATIONByMCOAffiliationID", parms);

        return ds;
    }

    private bool ValidateData()
    {
        bool rtn = true;
        
       
        return rtn;

    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "MCOAffiliations";
        this.Page.Validators.Add(val);
        isGood = false;
    }
}