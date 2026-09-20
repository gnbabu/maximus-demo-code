using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;


public partial class PopupControls_PracticePartnership : BaseSectionControl
{
    public string _SortField
    {
        get
        {
            return (string)ViewState["SortField"] ?? "DateOfAction"; // default sort 
        }
        set
        {
            ViewState["SortField"] = value;
        }
    }

    private bool _ExportHistory
    {
        get
        {
            return Convert.ToBoolean(ViewState["ExportHistory"]);
        }
        set
        {
            ViewState["ExportHistory"] = value;
        }
    }
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private const string sectionName = "PracticePartnership";
    private string CPCProgramYear = AppSettings.Get("CPCProgramYear");
    private bool newPracticePartner = false;

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

    #region Section
    private enum PopupName { CPCGroupMember = 0, GroupAffiliationsHistory = 1 };
    #endregion




    public GroupAndFacilityAffiliations Model { get; set; }

    public EventHandler InvalidateAgreements;


    protected void Page_Load(object sender, EventArgs e)
    {

        this.ucCPCGroupMember.CancelEvent += new PopupControls_CPCGroupMember.CancelEventHandler(View_Cancel);
        this.ucCPCGroupMember.ValidationEvent += new PopupControls_CPCGroupMember.ValidationEventHandler(KeepPopupOpen);
        this.ucCPCGroupMember.SaveEvent += new PopupControls_CPCGroupMember.SaveEventHandler(View_SavePendingGroupAffiliations);

        if (this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.CPCReattest)
        {
            lblStaticText.Text = "Confirm existing members of your practice partnership by clicking on the green check mark or remove members by clicking on the red X. Add members to the partnership by clicking the Add New button.";
        }
        else
            lblStaticText.Text = "Add members of your pratice partnership to this page by clicking the Add New button. Continue to add new members untill all members are added.";
    }

    private void View_Cancel()
    {
        this.mpe.Hide();
    }
    private void View_SavePendingGroupAffiliations()
    {
        this.upAff.Update();
        this.LoadData();
        this.mpe.Hide();
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = new DataSet();
        ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_CPCPractice_HISTORY", parms);

        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
        }

    }

    public override void LoadControlData()
    {
        this.LoadData(null);

        lnkExcel.Visible = grdConfirmedGroupAffiliations.Rows.Count > 0;
    }

    private bool ValidateReattestMember(int memberRegId)
    {
        bool isEligibleMember = false;
        DataSet ds = svc.ValidatePracticePartners(memberRegId, this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //if (Helper.GetInt("IsAvailable", dr) == 0)
            //{
            //    AddPopUpError("CPCExistsMessage");
            //    return isEligibleMember;
            //}
            if (Helper.GetInt("IsAvailable", dr) == 2)
            {
                AddPopUpError("CPCReattestMemberStartedonOwn");
                return isEligibleMember;
            }
            else if (Helper.GetInt("IsAvailable", dr) == 3)
            {
                AddPopUpError("CPCReattestMemberNotEligible");
                return isEligibleMember;
            }
            else if (Helper.GetInt("Total_Attributed_Members", dr) < 150)
            {
                AddPopUpError("CPCReattestMember150ClaimsEligibility");
                return isEligibleMember;
            }
            else
            {
                isEligibleMember = true;
            }
        }
        else
        {
            AddPopUpError("CPCReattestMemberNotEligible");
            return isEligibleMember;
        }
        return isEligibleMember;
    }
    public override bool ValidateData()
    {
       // Validation on PP
        if(isKidsSpecialtyAdded())
        {
            if (!ValidateTotalAttributedKids())
            {
                AddPopUpError("DenialMessageCPC");
                return false;
            }
        }

        if (!ValidateUploadControls())
        {
            return false;
        }
        
        DataSet ds = new DataSet();
        ds = (DataSet)grdConfirmedGroupAffiliations.DataSource;
        DataTable dt = ds.Tables[0];
        bool ppHasMembers = dt.AsEnumerable()
                            .Where(r => r.Field<string>("MEMBER_STATUS") == "Active" &&
                                        r.Field<string>("CPC_PROGRAM_YEAR") == CPCProgramYear)
                            .Count() > 1;
        if (!ppHasMembers)
        {
            AddPopUpError("MemberCountCPC");
            return false;
        }

        return true;       
    }
    private bool isKidsSpecialtyAdded()
    {
        bool kidsSpecialtyPresent = false;
        DateTime programYear = Convert.ToDateTime(AppSettings.Get("CPCProgramStartDate") + "/" + CPCProgramYear);
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PrimaryFlag", "0");
        DataSet dsSpecialties = svc.SelectRegistrationDataWithParams("usp_SelectREG_SPECIALTY", parms);
        DateTime EndDate = new DateTime(2299, 12, 31);
        if (Helper.HasRows(dsSpecialties))
        {

            kidsSpecialtyPresent = dsSpecialties.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") <= programYear &&
                                                      r.Field<DateTime>("END_DATE") > DateTime.Now &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.Active &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : true) == true)
                                          .Any() || dsSpecialties.Tables[0].AsEnumerable()
                                          .Where(r => r.Field<string>("MMIS_SPECIALTY_TYPE_ID") == CON.MMISSpecialtyType.CPCPEDIATRICS &&
                                                      r.Field<DateTime>("START_DATE") == programYear &&
                                                      r.Field<DateTime>("END_DATE") == EndDate &&
                                                      r.Field<Int32>("ENROLL_STATUS_ID") == CON.EnrollmentStatusTypeID.InActive &&
                                                      (r["SENT_TO_SI"] != DBNull.Value ? Convert.ToBoolean(r["SENT_TO_SI"]) : false) == false)
                                          .Any();
        }
        return kidsSpecialtyPresent;
    }
    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "CPCGroupMember":
                lblTitle.Text = "Add CPC Group Member";
                ucCPCGroupMember.LoadData();
                mltPopup.ActiveViewIndex = Convert.ToInt32(PopupName.CPCGroupMember);
                mpe.Show();
                break;
            default:
                break;
        }
    }
    protected void grdConfirmedGroupAffiliations_RowCommand(object sender, GridViewCommandEventArgs e)
    {      
        int index = Convert.ToInt32(e.CommandArgument);
        int regId = this.WorkflowPage.RegistrationId;
        int regAffiliationId = string.IsNullOrEmpty(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["REG_AFFILIATION_ID"].ToString()) ? 0 :
            (int)this.grdConfirmedGroupAffiliations.DataKeys[index].Values["REG_AFFILIATION_ID"];
        string startDate = this.grdConfirmedGroupAffiliations.DataKeys[index].Values["StartDate"].ToString();
        int memberRegId = Convert.ToInt32(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["AffiliateReg_Id"]);
        int groupAffStatusID = Convert.ToInt32(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["GROUP_AFFILIATION_STATUS_ID"]);
        int memAttributedCount = Convert.ToInt32(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["TOTAL_ATTRIBUTED_MEMBERS"]);
        int memAttributedCountKids = Convert.ToInt32(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["TOTAL_ATTRIBUTED_KIDS"]);

        if (e.CommandName == "Delete")
        {
            if (groupAffStatusID == CON.GroupAffiliationStatusTypeID.GroupConfirmed)
                svc.DeleteRegAffiliation(regAffiliationId);
            else
                UpdateRegAffiliation(this.WorkflowPage.RegistrationId.ToString(), regAffiliationId, startDate, true, groupAffStatusID, memAttributedCount, memAttributedCountKids);
        }
        else if (e.CommandName == "Attest")
        {
            if (ValidateReattestMember(memberRegId))
                UpdateRegAffiliation(this.WorkflowPage.RegistrationId.ToString(), regAffiliationId, startDate, false, groupAffStatusID, memAttributedCount, memAttributedCountKids);
        }
    }

    private bool ValidateTotalAttributedKids()
    {
        int sum = 0;
        DataTable dt = this.DataList;
        foreach (DataRow row in dt.Rows)
        {           
            sum += (int)row["TOTAL_ATTRIBUTED_KIDS"];
        }
        
        if (sum >= 150)
          return true; 
        else
          return false;
    }
    private void UpdateRegAffiliation(string reg_id, int regAffiliationId, string startDate, bool isDelete, int groupAffStatusID, int memAttributedCount, int memAttributedCountKids)
    {
        DateTime currentYearEndDate = new DateTime(DateTime.Now.Year, 12, 31);
        DateTime EndDate = new DateTime(2299, 12, 31);
        string CPCProgramStartDate = AppSettings.Get("CPCProgramStartDate") + "/" + CPCProgramYear;
        DateTime? AffiliationEndDate = null;

        Dictionary<string, string> parameters = new Dictionary<string, string>();
        parameters.Add("REG_ID", reg_id);
        if (isDelete)
        {
            if (groupAffStatusID != CON.GroupAffiliationStatusTypeID.RemovedbyGroup)
            {
                if (Convert.ToDateTime(startDate).ToString("MM/dd/yyyy") == CPCProgramStartDate && Convert.ToDateTime(startDate) > DateTime.Now)
                    AffiliationEndDate = Convert.ToDateTime(startDate);
                else if (Convert.ToDateTime(startDate).ToString("MM/dd/yyyy") == CPCProgramStartDate && Convert.ToDateTime(startDate) <= DateTime.Now)
                    AffiliationEndDate = DateTime.Now;
                else
                    AffiliationEndDate = currentYearEndDate;

                parameters.Add("End_DATE", AffiliationEndDate.ToString());
                parameters.Add("GROUP_AFFILIATION_STATUS_ID", CON.GroupAffiliationStatusTypeID.PendingRemoval.ToString());
            }
        }
        else
        {
            parameters.Add("End_DATE", EndDate.ToString());            
            if (groupAffStatusID != CON.GroupAffiliationStatusTypeID.Active)
                parameters.Add("GROUP_AFFILIATION_STATUS_ID", CON.GroupAffiliationStatusTypeID.GroupConfirmed.ToString());
            else
                parameters.Add("GROUP_AFFILIATION_STATUS_ID", CON.GroupAffiliationStatusTypeID.Active.ToString());
        }

        parameters.Add("REG_AFFILIATION_ID", regAffiliationId.ToString());
        parameters.Add("Start_date", startDate);
        parameters.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
        parameters.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parameters.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parameters.Add("CPC_PROGRAM_YEAR", CPCProgramYear);
        parameters.Add("COUNT_ATTRIBUTION", memAttributedCount.ToString());
        parameters.Add("COUNT_ATTRIBUTION_KIDS", memAttributedCountKids.ToString());
        svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "AFFILIATIONcustom", parameters);
        LoadControlData();
    }

    private bool ValidateUploadControls()
    {
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);

        ds = svc.ValidatePracticePartnersUploadControl(this.WorkflowPage.RegistrationId, pageTypeID, sectionName);

        if (newPracticePartner && (!Helper.HasRows(ds) || (Helper.HasRows(ds) && ds.Tables[0].Rows.Count < 2)))
        {
            AddPopUpError("CPCValidateAttestationAndAcknowledment");
            return false;
        }

        return true;
    }
    public void LoadPlaceHolder(int taxinfoID = 0, bool isEdit = true, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadPracticePartnership.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        int documentID = 0;
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, this.WorkflowPage.WF_ProcessID, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, this.WorkflowPage.WF_ProcessID, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = documentID = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = documentID = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                ucUploadSectionControl.RowId = this.WorkflowPage.WF_ProcessID;

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                {
                    if(newPracticePartner && Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.ProviderDataEntry)
                    {
                        if (documentID > 0 && Helper.GetInt("ROW_ID", dr) != this.WorkflowPage.WF_ProcessID)
                        {
                            // archive the document
                            svc.ArchiveDocumentToGenericUploadControl(this.WorkflowPage.RegistrationId, documentID, "PracticePartnership", null, null);
                            ucUploadSectionControl.FileName = null;
                        }
                        else
                            ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                    }
                    else
                    {
                        ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                    }                    
                }
                    
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadPracticePartnership.Controls.Add(ucUploadSectionControl);
            }

        }

    }

    public override void LoadData(DataRow dr = null)
    {
        DataRow provRow = Registration.GetProviderInfo(this.WorkflowPage.RegistrationId);
        string provider_type_id = Helper.GetString("MMIS_Provider_Type_ID", provRow);
        GroupAndFacilityAffiliations model = new GroupAndFacilityAffiliations();
        model.RegID = this.WorkflowPage.RegistrationId;

        DataSet ds = svc.GetPracticePartnership(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            this.DataList = ds.Tables[0];
            newPracticePartner = ds.Tables[0].AsEnumerable().Where(r => Convert.ToDateTime(r.Field<string>("StartDate")) <= Convert.ToDateTime(AppSettings.Get("CPCProgramStartDate") + "/" + CPCProgramYear) &&
                                                      r.Field<Int32>("GROUP_AFFILIATION_STATUS_ID") == CON.GroupAffiliationStatusTypeID.GroupConfirmed).Any(); 
            //if (newPracticePartner)
            //{
                LoadPlaceHolder();
            //}
        }

        grdConfirmedGroupAffiliations.DataSource = ds;
        grdConfirmedGroupAffiliations.DataBind();
        if (inMaintenance(this.WorkflowPage.RegistrationId) || Helper.isCPCEnrollmentPeriod() || Helper.isCPCLinkReenabled(this.WorkflowPage.RegistrationId))
        {
            btnAdd.Visible = true;
        }          
    }
    //private bool isEnrollmentPeriod()
    //{
    //    bool isEnrollmentPeriod = false; 
    //    string CPCEnrollmentPeriodStartDate = AppSettings.Get("CPCEnrollmentPeriodStartDate", string.Empty);
    //    string CPCEnrollmentPeriodEndDate = AppSettings.Get("CPCEnrollmentPeriodEndDate", string.Empty);
    //    DateTime beginDate = DateTime.ParseExact(CPCEnrollmentPeriodStartDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    DateTime endDate = DateTime.ParseExact(CPCEnrollmentPeriodEndDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
    //    DateTime todayDate = DateTime.Now.Date;
    //    if (todayDate >= beginDate && todayDate <= endDate)
    //    {
    //        isEnrollmentPeriod = true;
    //    }
    //    return isEnrollmentPeriod;
    //}
    private void AddPopUpError(string errMsg)
    {
        ucMessageBox.Show(UserControls_MessageModal.MessageModalMode.Ok, "Error", GetGlobalResourceObject("BrandingResource", errMsg).ToString());
    }
    public bool ExistingGroupMember(int index)    
    {
        bool canUserEditRegistration = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
        bool showReattest = false;
        int groupAffStatusID = Convert.ToInt32(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["GROUP_AFFILIATION_STATUS_ID"]);
        string affProgramYear = string.IsNullOrEmpty(this.grdConfirmedGroupAffiliations.DataKeys[index].Values["CPC_PROGRAM_YEAR"].ToString()) ? string.Empty : this.grdConfirmedGroupAffiliations.DataKeys[index].Values["CPC_PROGRAM_YEAR"].ToString();
        //bool CanReattestMember = this.grdConfirmedGroupAffiliations.DataKeys[index].Values["CanReattestMember"].ToString() == "Y" ? true : false;
        showReattest = groupAffStatusID == CON.GroupAffiliationStatusTypeID.Active && CPCProgramYear != affProgramYear ? true : false;
        return canUserEditRegistration && showReattest;
    }
    public bool CanDeleteMember(int index)
    {
        bool canUserEditRegistration = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
        bool showDelete = this.grdConfirmedGroupAffiliations.DataKeys[index].Values["CanReattestMember"].ToString() == "Y" 
                             && this.grdConfirmedGroupAffiliations.DataKeys[index].Values["MEMBER_STATUS"].ToString() != "Removed" ? true : false;
        return canUserEditRegistration && showDelete;
    }
    private bool isMembersReattested()
    {
        foreach (GridViewRow row in grdConfirmedGroupAffiliations.Rows)
        {
            if (((LinkButton)row.FindControl("btnReattest")).Visible)
              return false;           
        }
        return true;
    }

    public override bool SaveData()
    {
        if (ValidateData() && isMembersReattested())
            return true;
        else
            return false;
    }
    private void KeepPopupOpenHealthCare()
    {
        this.mpe2.Show();
    }
    private void KeepPopupOpen()
    {
        this.mpe.Show();
    }



    public void SetRegHealthCareFacilityAffiliation(DataSet ds)
    {
        throw new NotImplementedException();
    }

    public void SetRegAssignedDelegates(DataSet ds)
    {
        throw new NotImplementedException();
    }



    public override string ValidationGroup
    {
        get { return "valGroupAndFacilityAffiliations"; }
    }

    public override string Title
    {
        get { return "Group And Facility Affiliations"; }
    }

    public override string IdText
    {
        get { return "ucGroupAndFacility_" + this.WorkflowPage.RegistrationId; }
    }

    protected string FormatAddress(object add1, object add2, object city, object state, object zip, object ext_zip)
    {

        return Helper.GetFormattedAddress(Helper.ConvertNull(add1).ToString(), Helper.ConvertNull(add2).ToString(), Helper.ConvertNull(city).ToString(), Helper.ConvertNull(state).ToString(), Helper.ConvertNull(zip).ToString(), Helper.ConvertNull(ext_zip).ToString(), string.Empty);
    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "valGroupAndFacilityAffiliations";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = null;
        ds = svc.GetPracticePartnership(this.WorkflowPage.RegistrationId);
        RadGridExportPP.DataSource = ds;
        RadGridExportPP.DataBind();
        RadGridExportPP.MasterTableView.ExportToExcel();
    }



    protected void grdConfirmedGroupAffiliations_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        LoadData();
    }

    //protected void grdConfirmedGroupAffiliations_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        int groupAffStatusID = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "GROUP_AFFILIATION_STATUS_ID"));
    //        string affProgramYear = string.IsNullOrEmpty(DataBinder.Eval(e.Row.DataItem, "CPC_PROGRAM_YEAR").ToString()) ? string.Empty : DataBinder.Eval(e.Row.DataItem, "CPC_PROGRAM_YEAR").ToString();
    //        bool CanReattestMember = DataBinder.Eval(e.Row.DataItem, "CanReattestMember").ToString() == "Y" ? true : false;

    //        bool showReattest = groupAffStatusID != CON.GroupAffiliationStatusTypeID.GroupConfirmed && CPCProgramYear != affProgramYear && CanReattestMember ? true : false;


    //        LinkButton lnkDelete = (LinkButton)e.Row.FindControl("btnDelete");
    //        LinkButton lnkReattest = (LinkButton)e.Row.FindControl("btnReattest");

    //        lnkDelete.Visible = CanReattestMember;
    //        lnkReattest.Visible = showReattest;
    //    }
    //}

    protected void grdConfirmedGroupAffiliations_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdConfirmedGroupAffiliations.PageIndex = e.NewPageIndex;
        LoadData(); // RefreshData();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.CancelPopup();
    }

    private void CancelPopup()
    {
        this.mpeHistory.Hide();
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        mpeHistory.Show();
    }

    protected void lnkHistoryExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_CPCPractice_HISTORY", parms);
            if (Helper.HasRows(ds))
            {
                grdHistoryExport.DataSource = ds.Tables[0];
                grdHistoryExport.DataBind();
                grdHistoryExport.MasterTableView.ExportToExcel();
            }
        }
    }

    protected void grd_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (_SortField.Equals(e.SortExpression))
        {
            _SortField = _SortField + " DESC";
        }
        else
        {
            _SortField = e.SortExpression;
        }
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_CPCPractice_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.PageIndex = 0;
            grd.DataSource = ds.Tables[0];
            grd.DataBind();
            mpeHistory.Show();
        }
    }

    protected void grd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_Contact_HISTORY", parms);
        if (Helper.HasRows(ds))
        {
            ds.Tables[0].DefaultView.Sort = _SortField;
            grd.DataSource = ds.Tables[0];
            grd.PageIndex = e.NewPageIndex;
            grd.DataBind();
            mpeHistory.Show();
        }
    }
}
