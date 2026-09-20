using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PopupControls_ClosureNotice : BaseSectionControl
{
    private const string sectionName = "ClosureNotice";
	private const int pageSize = 10;

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
    private int RegClosureNoticeID
    {
        get
        {
            return ViewState["RegClosureNoticeID"] == null ? 0 : Convert.ToInt32(ViewState["RegClosureNoticeID"]);
        }
        set
        {
            ViewState["RegClosureNoticeID"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        //LoadClosureNotice();

        DataSet dataSet = svc.GetLTCReviewTypes();
        DataTable dt = dataSet.Tables[0];

        ddlReviewType.DataSource = dt;
        ddlReviewType.DataTextField = "DSC_Review_Type";
        ddlReviewType.DataValueField = "Review_TYPE_ID";
        ddlReviewType.DataBind();
        LoadClosureNotice();

        // OHPNM-11563
        if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep) && this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsEditable != 1)
        {
            // disable the closure notice page since it's only visible to this role and not editable
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override bool SaveData()
    {
      bool valid = ValidateData();
        Page.Validate("valClosureNotice");
        if (Page.IsValid && valid)
        {
            try
            {
                var DateofRiskAlert = this.txtDateofRiskAlert.Text;
                var proposedEffectiveDate = this.txtEffectiveDateCHOP.Text;
                var effectiveDate = this.txtEffectiveDate.Text;
                var reviewType = this.ddlReviewType.SelectedValue;
                Dictionary<string, string> parms = new Dictionary<string, string>();
             
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("CREATED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Risk_Alert_Date", DateofRiskAlert);
                parms.Add("Proposed_Effective_Date", proposedEffectiveDate);
                parms.Add("Closure_Effective_Date", effectiveDate);
                parms.Add("Review_Type_ID", reviewType);
                var Is_DaysNotice = false;
                parms.Add("Is_DaysNotice", Is_DaysNotice.ToString());
                parms.Add("PROCESS_ID", this.WorkflowPage.WF_ProcessID.ToString());
                svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "REG_LTC_RISK_ALERT", parms);
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
        LoadClosureNotice();
    }
    public void LoadClosureNotice()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("PROCESS_ID", this.WorkflowPage.WF_ProcessID.ToString());
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_ClosureNotice", parms);

        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtMisc;

        if (Helper.HasRows(dtMisc))
            this.LoadData(dtMisc.Rows[0], true);
        //this.LoadData(null, false);
        else
            this.LoadData(null, false);
    }
    private void LoadData(DataRow dr, bool isEdit)
    {
        int closureNoticeId = 0;
        if (checkRolesToLoadPlaceholder()) {
            LoadPlaceHolder(closureNoticeId, isEdit);
        } 

        if (isEdit)
        {
            RegClosureNoticeID = Helper.GetInt("REG_LTC_RISK_ALERT_ID", dr);
            int alertProcessId = Helper.GetInt("PROCESS_ID", dr);

            // load data into the controls if there is risk data and it's for the current process id
            if (RegClosureNoticeID > 0 && alertProcessId == this.WorkflowPage.WF_ProcessID)
            {
                this.txtDateofRiskAlert.Text = Helper.FormatDate2(dr["Risk_Alert_Date"].ToString());
                this.txtEffectiveDateCHOP.Text = Helper.FormatDate2(dr["Proposed_Effective_Date"].ToString());
                this.txtEffectiveDate.Text = Helper.FormatDate2(Helper.GetString("Closure_Effective_Date", dr));
                this.ddlReviewType.SelectedValue = Helper.GetInt("Review_Type_ID", dr).ToString();
            }

            //ShowCoreValues(HttpContext.Current.User.IsInRole("Administrator") && isEdit);
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
        DataSet ds = svc.GetClosureHistory(this.WorkflowPage.RegistrationId, pageSize, toPageNumber);
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

    private bool checkRolesToLoadPlaceholder()
    {
        bool loadPlaceHolder = true;
        if (Helper.IsLoggedInUserInODMCredentialingQualityAssuranceRole() 
            || Helper.IsLoggedInUserInCommitteeQualitySpecialistRole()
            || Helper.IsLoggedInUserInCredentialingQualityAssuranceRole()
            )
        {
            loadPlaceHolder = false;
        }
        return loadPlaceHolder;
    }

    public void LoadPlaceHolder(int noticeId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadClosureNotice.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, noticeId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, noticeId, pageSection, isEdit);

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
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);//@"C:\project\temp";

#if DEBUG
                   ucUploadSectionControl.DestinationPath  = @"C:\Temp";
#endif
                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                //OHPNM-3665
                if (!isEdit)
                {
                    ucUploadSectionControl.FileName = null;
                    ucUploadSectionControl.DocumentId = 0;
                }
                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if(PlaceholderUploadClosureNotice.FindControl(ucUploadSectionControl.ID)==null)
                PlaceholderUploadClosureNotice.Controls.Add(ucUploadSectionControl);

                // OHPNM-11563 - disable the closure notice page's upload control since it's only visible to this role and not editable
                if (this.WorkflowPage.RegistrationNodes.ContainsKey(this.WorkflowPage.RegistrationStep) && this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsEditable != 1)
                {
                    ucUploadSectionControl.IsDisabled = true;
                }
            }
        }

    }
    private void ShowCoreValues(bool isVisible)
    {
        if (isVisible)
        {
            this.closureNiticeInoutControls.Visible = true;
        }
        else
        {
            this.closureNiticeInoutControls.Visible = false;
        }
    }

    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        if (string.IsNullOrEmpty(this.txtDateofRiskAlert.Text))
        {
            lblerrormsg.Text = "Please select Date of Risk Alert";
            return false;
        }
        if (string.IsNullOrEmpty(this.txtEffectiveDateCHOP.Text))
        {
            lblerrormsg.Text = "Please select Proposed Effective Date.";
            return false;
        }
      

        if (string.IsNullOrEmpty(this.ddlReviewType.SelectedValue))
        {
            lblerrormsg.Text = "Please select the review type.";
            return false;
        }
        bool isValid = true;
        foreach (Control ctrl in PlaceholderUploadClosureNotice.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid = uploadControl.ValidateData("valClosureNotice");
            if(uploadControl.DocumentId==0)
            {
                lblerrormsg.Text = "Please Upload Closure notice.";
                return false;
            }
        }
        return isValid;
    }
    private void AddError(string msg)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valClosureNotice";
        this.Page.Validators.Add(val);
    }

    public override string ValidationGroup
    {
        get { return "valClosureNotice"; }
    }

    public override string Title
    {
        get { return "Closure Risk Alert"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucClosureNotice_" + this.WorkflowPage.RegistrationId; }
    }

    //protected void saveNotice_Click(object sender, EventArgs e)
    //{
    //    SaveData();
    //}

    protected void cvProposedEffectiveDate_ServerValidate(object source, ServerValidateEventArgs args)
    {
        bool isValid = true;
        try
        {
            string proposteddate = this.txtEffectiveDateCHOP.Text;
            DateTime dtProposedValid = DateTime.Now.AddYears(1);

         if(!string.IsNullOrEmpty(proposteddate))
            {
                DateTime entereddT = Convert.ToDateTime(proposteddate);
                isValid = (entereddT <= dtProposedValid);
                if(isValid)
                {
                    DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "Provider");
                    string strEffective = ds.Tables[0].Rows[0]["CHANGE_EFFECTIVE_DATE"].ToString();
                    if (!string.IsNullOrEmpty(strEffective))
                    {
                        DateTime enrollEffectiveDate = Convert.ToDateTime(strEffective);
                        isValid = (entereddT >= enrollEffectiveDate);
                    }
                }
            }
            else
            { isValid = false; }
            
        }
        catch
        {
        }
        args.IsValid = isValid;
    }
}