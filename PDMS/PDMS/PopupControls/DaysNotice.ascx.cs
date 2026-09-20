using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControlsDaysNotice : BaseSectionControl
{
    private const string SectionName = "45DaysNotice";
	private const int pageSize = 10;

    #region svc
    private PDMSService.PDMSServiceClient _svc;
    private PDMSService.PDMSServiceClient Svc
    {
        get
        {
            return _svc ?? (_svc = new PDMSService.PDMSServiceClient());
        }
    }
    private int Reg45DaysNoticeId
    {
        get
        {
            return ViewState["Reg45DaysNoticeID"] == null ? 0 : Convert.ToInt32(ViewState["Reg45DaysNoticeID"]);
        }
        set
        {
            ViewState["Reg45DaysNoticeID"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        DataSet dataSet = Svc.GetLTCReviewTypes();
        var reviewTypesTable = Helper.HasRows(dataSet) ? dataSet.Tables[0] : null;
        if (reviewTypesTable != null)
        {
            Helper.LoadList(ddlReviewType, reviewTypesTable, "DSC_Review_Type", "Review_TYPE_ID", true);
        }

        Load45DayNotice();

        // OHPNM-11563
        if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep) && this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsEditable != 1)
        {
            // disable the 45 days notice page since it's only visible to this role and not editable
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "45 Days Notice";
    }

    public override bool SaveData()
    {
        ValidateData();
       Page.Validate("valdayNotice");
        if (Page.IsValid)
        {
            try
            {
                var dateofRiskAlert = this.txtDateofRiskAlert.Text;
                var effectiveDateChop = this.txtEffectiveDateCHOP.Text;
                var endDateRiskAlertStatus = this.txtEndDateRiskAlertStatus.Text;
                var enteringProviderEmail = this.txtEnteringProviderEmail.Text;
                var enteringProviderName = this.txtEnteringProviderName.Text;
                var ownerNameFromCostReport = this.txtOwnerNameFromCostReport.Text;
                var reviewType = this.ddlReviewType.SelectedValue;
                var chopWithdrawn = this.ChkBoxCHOPWithdrawn.Checked;
                var indicationofSuccessorLiability = this.ChkBoxIndicationofSuccessorLiability.Checked;
                var chopFinalDate = this.txtFinalChopDate.Text;

                //Update risk alert end date with chop withdrawn date
                if (ChkBoxCHOPWithdrawn.Checked && string.IsNullOrEmpty(endDateRiskAlertStatus))
                    endDateRiskAlertStatus = DateTime.Now.ToString("d");

                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("CREATED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Risk_Alert_Date", dateofRiskAlert);
                parms.Add("Chop_Effective_Date", effectiveDateChop);
                parms.Add("Proposed_Effective_Date", effectiveDateChop);
                parms.Add("Risk_Alert_Status_End_Date", endDateRiskAlertStatus);
                parms.Add("Entering_Provider_EMAIL", enteringProviderEmail);
                parms.Add("Entering_Provider_Name", enteringProviderName);
                parms.Add("Owner_Name_from_Cost_Report", ownerNameFromCostReport);
                parms.Add("Review_Type_ID", reviewType);
                var isDaysNotice = true;
                parms.Add("Is_DaysNotice", isDaysNotice.ToString());
                parms.Add("CHOP_Withdrawn", chopWithdrawn ? "1" : "0");
                parms.Add("Indication_of_Successor_Liability", indicationofSuccessorLiability ? "1" : "0");
                parms.Add("CHOP_Final_Date", chopFinalDate);
                parms.Add("PROCESS_ID", this.WorkflowPage.WF_ProcessID.ToString());
                int ltcRiskAlertId = Svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "REG_LTC_RISK_ALERT", parms);

                foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUpload45DayNotice.Controls)
                {
                    Svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, ltcRiskAlertId);
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        return false;
    }

    public override void LoadControlData()
    {
        if (Helper.IsLoggedInUserInCommitteeQualitySpecialistRole())
        {
            PlaceholderUpload45DayNotice.Visible = false;
        }
        Load45DayNotice();

    }
    public void Load45DayNotice()
    {

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PROCESS_ID", this.WorkflowPage.WF_ProcessID.ToString());
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectREG_DaysNotice", parms);

        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtMisc;
        if (Helper.HasRows(dtMisc))
            this.LoadData(dtMisc.Rows[0], true);
        else
            this.LoadData(null, false);
    }
    private void LoadData(DataRow dr, bool isEdit)
    {
        int dayNoticeId = isEdit ? Helper.GetInt("REG_LTC_RISK_ALERT_ID", dr) : 0;

        LoadPlaceHolder(dayNoticeId, isEdit);

        if (isEdit)
        {
            Reg45DaysNoticeId = Helper.GetInt("REG_LTC_RISK_ALERT_ID", dr);
            int alertProcessId = Helper.GetInt("PROCESS_ID", dr);

            // load data into the controls if there is risk data and it's for the current process id
            if (Reg45DaysNoticeId > 0 && alertProcessId == this.WorkflowPage.WF_ProcessID)
            {
                this.txtDateofRiskAlert.Text = Helper.FormatDate2(dr["Risk_Alert_Date"].ToString());
                this.txtEffectiveDateCHOP.Text = Helper.FormatDate2(dr["Chop_Effective_Date"].ToString());
                this.txtEndDateRiskAlertStatus.Text = Helper.FormatDate2(dr["Risk_Alert_Status_End_Date"].ToString());
                this.txtEnteringProviderEmail.Text = Helper.GetString("Entering_Provider_EMAIL", dr);
                this.txtEnteringProviderName.Text = Helper.GetString("Entering_Provider_Name", dr);
                this.txtOwnerNameFromCostReport.Text = Helper.GetString("Owner_Name_from_Cost_Report", dr);
                this.ddlReviewType.SelectedValue = Helper.GetInt("Review_Type_ID", dr).ToString();
                this.ChkBoxCHOPWithdrawn.Checked = Helper.GetBool("CHOP_Withdrawn", dr);
                this.ChkBoxIndicationofSuccessorLiability.Checked = Helper.GetBool("Indication_of_Successor_Liability", dr);
                this.txtFinalChopDate.Text = Helper.FormatDate2(dr["CHOP_Final_Date"].ToString());
            }
        }
		
		BindSearchResultsGrid(1);
    }
	    
	protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "PageNo")
        {
            BindSearchResultsGrid(Convert.ToInt32(e.CommandArgument));
        }
    }

    private void BindSearchResultsGrid(int toPageNumber)
    {
        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.GetCHOPHistory(this.WorkflowPage.RegistrationId, pageSize, toPageNumber);
        int searchResultsTotalRows = Helper.GetInt("TOTAL", ds.Tables[1].Rows[0]);
        BindPager(searchResultsTotalRows, toPageNumber);
        grd.DataSource = ds.Tables[0];
        grd.DataBind();
        upGrd.Update();
    }

    private void BindPager(int totalRows, int currentPage)
    {
        int totalPages = (int)Math.Ceiling((decimal)totalRows / pageSize);
        List<ListItem> pagerContainer = new List<ListItem>();
        for (int i = 1; i <= totalPages; i++)
        {
            pagerContainer.Add(new ListItem(i.ToString(), i.ToString(), currentPage == i ? false : true));
        }

        dlPager.DataSource = pagerContainer;
        dlPager.DataBind();
    }
	
    public void LoadPlaceHolder(int dayNoticeId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = SectionName)
    {
        Upload upload = new Upload();
        PlaceholderUpload45DayNotice.Controls.Clear();
        DataSet ds = null;
        int pageTypeId = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeId, dayNoticeId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeId, dayNoticeId, pageSection, isEdit);

        int table = ds.Tables.Count;
        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);
                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);
                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                ucUploadSectionControl.DocumentId = ds.Tables[i].Columns.Contains("DOCUMENT_ID") ? Helper.GetInt("DOCUMENT_ID", dr) : 0;
                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
#if DEBUG
                   ucUploadSectionControl.DestinationPath  = @"C:\Temp";
#endif
                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);
                ucUploadSectionControl.FileName = dr.Table.Columns.Contains("FILE_NAME") ? Helper.GetString("FILE_NAME", dr) : null;
                ucUploadSectionControl.SectionName = SectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUpload45DayNotice.Controls.Add(ucUploadSectionControl);

                // OHPNM-11563 - disable the 45 day notice page's upload control since it's only visible to this role and not editable           
                if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep) && this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsEditable != 1)
                {
                    ucUploadSectionControl.IsDisabled = true;
                }
            }
        }

    }
    private void ShowCoreValues(bool isVisible)
    {
        this.dayNoticeInputControls.Visible = isVisible;
    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        bool isValid = true;
        if (string.IsNullOrEmpty(this.txtDateofRiskAlert.Text))
        {
            AddError("Please select Date of Risk Alert");
            isValid = false;
        }
        if (string.IsNullOrEmpty(this.txtEffectiveDateCHOP.Text))
        {
            AddError("Please select Effective Date.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(this.txtEndDateRiskAlertStatus.Text))
        {
            AddError("Please select EndDate Risk Alert Status.");
            isValid = false;
        }

        if (string.IsNullOrEmpty(this.txtEnteringProviderEmail.Text))
        {
            AddError("Please enter the Entering Provider Email.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(this.txtEnteringProviderName.Text))
        {
            AddError("Please enter the Entering ProviderName.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(this.txtOwnerNameFromCostReport.Text))
        {
            AddError("Please enter Owner Name From CostReport.");
            isValid = false;
        }
        if (string.IsNullOrEmpty(this.ddlReviewType.SelectedValue))
        {
            AddError("Please select the review type.");
            isValid = false;
        }
        foreach (Control ctrl in PlaceholderUpload45DayNotice.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid = uploadControl.ValidateData("valDaysNotice");
        }
        return isValid;
       
    }
    private void AddError(string message)
    {
        var validator = new CustomValidator
        {
            IsValid = false,
            ErrorMessage = message,
            ValidationGroup = ValidationGroup// "valProviderInfoHeader"
        };
        this.Page.Validators.Add(validator);
    }
   
    public override string ValidationGroup
    {
        get { return "valDaysNotice"; }
    }
 
    public override string Title
    {
        get { return "DaysNotice SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucDaysNotice_" + this.WorkflowPage.RegistrationId; }
    }

    public delegate void NameLinkEventHandler(int partyId, int submitRosterId);
    public event NameLinkEventHandler NameLinkEvent;


    protected void ChkBoxCHOPWithdrawn_CheckedChanged(object sender, EventArgs e)
    {
        if (ChkBoxCHOPWithdrawn.Checked)
            txtEndDateRiskAlertStatus.Text = DateTime.Now.ToString("d");
    }
}
