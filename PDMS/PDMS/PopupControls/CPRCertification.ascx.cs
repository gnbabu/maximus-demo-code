using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CPRCertification : BaseSectionControl, ICPRCertificationsView
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    private const string sectionName = "CPRCertification";

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

    public int RegCPRCertificationID
    {
        get
        {
            return ViewState["RegCPRCertificationID"] == null ? 0 : Convert.ToInt32(ViewState["RegCPRCertificationID"]);
        }
        private set
        {
            ViewState["RegCPRCertificationID"] = value;
        }
    }
    public int RegFirstAidCertificationID
    {
        get
        {
            return ViewState["RegFirstAidCertificationID"] == null ? 0 : Convert.ToInt32(ViewState["RegFirstAidCertificationID"]);
        }
        private set
        {
            ViewState["RegFirstAidCertificationID"] = value;
        }
    }

    public DataTable dtLoadCPRCertification
    {
        get
        {
            return ViewState["dtLoadCPRCertification"] == null ? null : (DataTable)ViewState["dtLoadCPRCertification"];
        }
        set
        {
            ViewState["dtLoadCPRCertification"] = value;
        }
    }
    public DataTable dtLoadFACertification
    {
        get
        {
            return ViewState["dtLoadFACertification"] == null ? null : (DataTable)ViewState["dtLoadFACertification"];
        }
        set
        {
            ViewState["dtLoadFACertification"] = value;
        }
    }


    #region Parent Page Events

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    #endregion
    private CPRCertificationsPresenter _presenter;

    public CPRCertificationsPresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new CPRCertificationsPresenter(this);
            }

            return _presenter;
        }
    }

    public CPRCertifications Model { get; set; }
    public bool CanUserViewDelete()
    {
        DataSet ds1 = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds1) && Helper.GetInt("RegistrationStatusTypeID", ds1.Tables[0].Rows[0]) != CON.RegistrationStatusTypeId.Submitted)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #region Page Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (RegCPRCertificationID > 0)
            LoadPlaceHolder(RegCPRCertificationID);
        else if (RegFirstAidCertificationID > 0)
        {
            LoadPlaceHolder(RegFirstAidCertificationID);
        }
        else
            LoadPlaceHolder();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
    }



    public override bool ValidateData()
    {
        if (pnlFieldsCPRCertification.Visible == false && pnlFieldsFirstAidCertification.Visible == false)
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "No data entered to Save";
            val.ValidationGroup = "valCPRCertifications";
            this.Page.Validators.Add(val);
        }
        if (rblCPRCertificationID.SelectedValue == "True" && (string.IsNullOrEmpty(txtClassifications.Text) || string.IsNullOrWhiteSpace(txtClassifications.Text)))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "List the Training Organization for CPR Certified.";
            val.ValidationGroup = "valCPRCertifications";
            this.Page.Validators.Add(val);
        }
        if (rblFirstAidID.SelectedValue == "True" && (string.IsNullOrEmpty(txtFirstAidTOrg.Text) || string.IsNullOrWhiteSpace(txtFirstAidTOrg.Text)))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "List the Training Organization for First Aid Certified.";
            val.ValidationGroup = "valCPRCertifications";
            this.Page.Validators.Add(val);
        }
        if (rblCPRCertificationID.SelectedValue == "True" && (string.IsNullOrEmpty(txtExpirationDate.Text) || string.IsNullOrWhiteSpace(txtExpirationDate.Text)))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Expiration Date cannot be empty or spaces for CPR Certification.";
            val.ValidationGroup = "valCPRCertifications";
            this.Page.Validators.Add(val);
        }
        else if (rblCPRCertificationID.SelectedValue == "True")
        {
            DateTime value;
            if (DateTime.TryParse(txtExpirationDate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtExpirationDate.Text)) > 0)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Expiration Date cannot be before todays date for CPR Certified.";
                    val.ValidationGroup = "valCPRCertifications";
                    this.Page.Validators.Add(val);
                }
            }
            else
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Expiration Date is not a date format";
                val.ValidationGroup = "valCPRCertifications";
                this.Page.Validators.Add(val);
            }

        }
        if (rblFirstAidID.SelectedValue == "True" && (string.IsNullOrEmpty(txtFirstAidExpDate.Text) || string.IsNullOrWhiteSpace(txtFirstAidExpDate.Text)))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "Expiration Date cannot be empty or spaces for First Aid Section.";
            val.ValidationGroup = "valCPRCertifications";
            this.Page.Validators.Add(val);
        }
        else if (rblFirstAidID.SelectedValue == "True")
        {
            DateTime value;
            if (DateTime.TryParse(txtFirstAidExpDate.Text, out value))
            {
                if (DateTime.Compare(DateTime.Now.Date, Convert.ToDateTime(txtFirstAidExpDate.Text)) > 0)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "Expiration Date cannot be before todays date for First Aid Certified.";
                    val.ValidationGroup = "valCPRCertifications";
                    this.Page.Validators.Add(val);
                }
            }
            else
            {
                CustomValidator val = new CustomValidator();
                val.IsValid = false;
                val.ErrorMessage = "Expiration Date is not a date format";
                val.ValidationGroup = "valCPRCertifications";
                this.Page.Validators.Add(val);
            }

        }
        return true;
    }
    #endregion

    #region Public Methods

    public override void LoadData(DataRow dr)
    {
        if (RegCPRCertificationID > 0)
        {
            LoadPlaceHolder(RegCPRCertificationID);
        }
        else if (RegFirstAidCertificationID > 0)
        {
            LoadPlaceHolder(RegFirstAidCertificationID);
        }
        else
            LoadPlaceHolder();
    }

    private void LoadCPRCertificationData(DataRow dr)
    {
        int cprcertificationID = dr != null ? Helper.GetInt("REG_CPR_Certification_ID", dr) : 0;
        if (dr != null)
        {
            rblCPRCertificationID.SelectedValue = Helper.GetString("IsCPRCertified", dr);
            txtClassifications.Text = Helper.GetString("Classifications", dr);
            txtExpirationDate.Text = Helper.GetDate("ExpirationDate", dr);
            RegCPRCertificationID = cprcertificationID;
            LoadPlaceHolder(cprcertificationID, true, false);

            if (rblCPRCertificationID.SelectedValue == "False")
            {
                txtExpirationDate.Enabled = false;
                txtClassifications.Enabled = false;
            }
            else
            {
                txtClassifications.Enabled = true;
                txtExpirationDate.Enabled = true;
            }
        }
        else
        {
            RegCPRCertificationID = 0;
            txtExpirationDate.Text = "";
            txtClassifications.Text = "";
        }
    }
    private void LoadFirstAidCertificationData(DataRow dr)
    {
        int firstAidCertificationID = dr != null ? Helper.GetInt("REG_FIRSTAID_CERTIFICATION_ID", dr) : 0;
        if (dr != null)
        {
            rblFirstAidID.SelectedValue = Helper.GetString("IsFirstAidCertified", dr);
            txtFirstAidTOrg.Text = Helper.GetString("FirstAidClassifications", dr);
            txtFirstAidExpDate.Text = Helper.GetDate("FirstAidExpirationDate", dr);
            if(rblFirstAidID.SelectedIndex == 0)
            {
                txtFirstAidTOrg.Enabled = true;
                txtFirstAidExpDate.Enabled = true;
                rfvtxtFirstAidExpDate.Enabled = true;
                cvtxtFirstAidExpDate.Enabled = true;
                rfvtxtFirstAidTOrg.Enabled = true;
            }
            else
            {
                txtFirstAidTOrg.Enabled = false;
                txtFirstAidExpDate.Enabled = false;
                rfvtxtFirstAidExpDate.Enabled = false;
                cvtxtFirstAidExpDate.Enabled = false;
                rfvtxtFirstAidTOrg.Enabled = false;
            }
            RegFirstAidCertificationID = firstAidCertificationID;
            LoadPlaceHolder(firstAidCertificationID, true, false);
        }
        else
        {
            RegFirstAidCertificationID = 0;
            txtFirstAidExpDate.Text = "";
            txtFirstAidTOrg.Text = "";
        }
    }

    protected void btnAdd_Click(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "CPRCertification")
        {

            pnlFieldsFirstAidCertification.Visible = pnlFieldsFirstAidCertification.Enabled = false;
            pnlFieldsCPRCertification.Visible = pnlFieldsCPRCertification.Enabled = true;
            rblCPRCertificationID.SelectedValue = "False";
            txtExpirationDate.Text = "";
            txtClassifications.Text = "";
            RegCPRCertificationID = 0;
        }
        else if (e.CommandName == "FirstAidCertification")
        {
            pnlFieldsCPRCertification.Visible = pnlFieldsCPRCertification.Enabled = false;
            pnlFieldsFirstAidCertification.Visible = pnlFieldsFirstAidCertification.Enabled = true;
            rblFirstAidID.SelectedValue = "False";
            txtFirstAidExpDate.Text = "";
            txtFirstAidTOrg.Text = "";
            RegFirstAidCertificationID = 0;
        }
    }

    public void LoadPlaceHolder(int cprcertificationId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadCPRCertification.Controls.Clear();
        PlaceholderUploadFirstAidCertification.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cprcertificationId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cprcertificationId, pageSection, isEdit);


        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {
                //IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                string title = Helper.GetString("TITLE", dr);
                ucUploadSectionControl.Title = title;

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);
                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                else
                    ucUploadSectionControl.DocumentId = 0;

                ucUploadSectionControl.DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);

                ucUploadSectionControl.RowId = cprcertificationId;


                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.SectionName = sectionName;

                ucUploadSectionControl.RemoveEvent += new UserControls_UploadSectionControl.RevoveEventHandler(ChangePageStatusToNotComplete);

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (title.Equals("CPR Document Upload"))
                {
                    ucUploadSectionControl.IsRequired = true;
                    PlaceholderUploadCPRCertification.Controls.Add(ucUploadSectionControl);
                }
                else if (title.Equals("First Aid Document Upload"))
                {
                    ucUploadSectionControl.IsRequired = true;
                    PlaceholderUploadFirstAidCertification.Controls.Add(ucUploadSectionControl);
                }

            }
        }

    }

    private void ChangePageStatusToNotComplete()
    {
        Registration.SetProviderSectionNodeStatusId(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep, CON.RegistrationProviderStatusTypeId.NotComplete);
    }

    private void LoadCPRCerts()
    {
        DataSet ds;
        // Load CPR Certification Grid
        ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CPR_Certification");
        ViewState["dtLoadCPRCertification"] = Helper.HasRows(ds) ? ds.Tables[0] : null;
        grdCPRCertification.DataSource = ViewState["dtLoadCPRCertification"];
        grdCPRCertification.DataBind();



        // Load First Aid Certification Grid
        ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "FIRSTAID_CERTIFICATION");
        ViewState["dtLoadFACertification"] = Helper.HasRows(ds) ? ds.Tables[0] : null;
        grdFirstAidCertification.DataSource = ViewState["dtLoadFACertification"];
        grdFirstAidCertification.DataBind();

        btnAddCPRCertification.Visible = true;
        btnAddFirstAidCertification.Visible = true;
    }

    protected void grdCPRCertification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CPR_Certification");
        DataTable dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        DataRow dr = dt.Rows[index];
        if (e.CommandName == "DeleteCPRRow")
        {
            bool IsDeleted = DeleteCPRCerts(dt, index);
            if (IsDeleted) LoadCPRCerts();
        }
        else
        {
            pnlFieldsFirstAidCertification.Visible = pnlFieldsFirstAidCertification.Enabled = false;
            pnlFieldsCPRCertification.Visible = pnlFieldsCPRCertification.Enabled = true;
            if (Helper.HasRows(dt))
                this.LoadCPRCertificationData(dr);
            else
                this.LoadCPRCertificationData(null);
        }
    }

    protected void grdFirstAidCertification_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "FIRSTAID_CERTIFICATION");
        DataTable dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        DataRow dr = dt.Rows[index];

        if (e.CommandName == "DeleteFirstAidRow")
        {
            bool IsDeleted = DeleteFirstAidCerts(dt, index);
            if (IsDeleted) LoadCPRCerts();
        }
        else
        {
            pnlFieldsCPRCertification.Visible = pnlFieldsCPRCertification.Enabled = false;
            pnlFieldsFirstAidCertification.Visible = pnlFieldsFirstAidCertification.Enabled = true;
            if (Helper.HasRows(dt))
                this.LoadFirstAidCertificationData(dr);
            else
                this.LoadFirstAidCertificationData(null);
        }
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
    }

    protected void grdCPRCertification_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 0;
            e.Row.Cells[idx].Text = e.Row.Cells[idx].Text == "True" ? "Yes" : "No";
        }
    }

    protected void grdFirstAidCertification_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int idx = 0;
            e.Row.Cells[idx].Text = e.Row.Cells[idx].Text == "True" ? "Yes" : "No";
        }
    }

    protected void OnrblFirstAidChanged(object sender, EventArgs e)
    {
        EnableFirstAid();
    }

    protected void OnrblCPRChanged(object sender, EventArgs e)
    {
        EnableCPR();
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadCPRCerts();
    }

    #endregion

    public void InitializeFields()
    {
        txtClassifications.Text = string.Empty;
        txtExpirationDate.Text = string.Empty;
    }

    private void EnableAllFields(bool enable)
    {

    }

    private void EnableCPR()
    {
        if (rblCPRCertificationID.SelectedIndex == 0)
        {
            //
            txtExpirationDate.Enabled = true;
            //rfvtxtExpirationDate.Enabled = true;
            cvtxtExpirationDate.Enabled = true;
            //
            txtClassifications.Enabled = true;
            //rfvtxtClassifications.Enabled = true;
        }
        else
        {
            //
            txtExpirationDate.Enabled = false;
            rfvtxtExpirationDate.Enabled = false;
            cvtxtExpirationDate.Enabled = false;
            txtExpirationDate.Text = "";
            //
            txtClassifications.Enabled = false;
            rfvtxtClassifications.Enabled = false;
            txtClassifications.Text = "";
        }
    }

    private void EnableFirstAid()
    {
        if (rblFirstAidID.SelectedIndex == 0)
        {
            //
            txtFirstAidExpDate.Enabled = true;
            //rfvtxtFirstAidExpDate.Enabled = true;
            cvtxtFirstAidExpDate.Enabled = true;
            //
            txtFirstAidTOrg.Enabled = true;
            //rfvtxtFirstAidTOrg.Enabled = true;
        }
        else
        {
            //
            txtFirstAidExpDate.Enabled = false;
            rfvtxtFirstAidExpDate.Enabled = false;
            cvtxtFirstAidExpDate.Enabled = false;
            txtFirstAidExpDate.Text = "";
            //
            txtFirstAidTOrg.Enabled = false;
            rfvtxtFirstAidTOrg.Enabled = false;
            txtFirstAidTOrg.Text = "";
        }
    }

    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(rblCPRCertificationID.SelectedValue) || !string.IsNullOrEmpty(txtExpirationDate.Text) || !string.IsNullOrEmpty(txtClassifications.Text))
            isRequired = true;
        return isRequired;
    }

    public override bool SaveData()
    {
        bool rtn = false;
        Page.Validate("valCPRCertifications");


        if (ValidateData())
        {
            bool isValid = true;
            if (pnlFieldsCPRCertification.Visible == true)
            {
                foreach (Control ctrl in PlaceholderUploadCPRCertification.Controls)
                {
                    UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                    isValid &= uploadControl.ValidateData("valCPRCertifications");
                }
                rtn = true;
            }
            else if (pnlFieldsFirstAidCertification.Visible == true)
            {
                foreach (Control ctrl in PlaceholderUploadFirstAidCertification.Controls)
                {
                    UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                    isValid &= uploadControl.ValidateData("valCPRCertifications");
                }
                rtn = true;
            }

            if (!isValid)
                return isValid;

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && !string.IsNullOrEmpty(v.ControlToValidate) && v.ValidationGroup.Equals("valCPRCertifications") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }

            CPRCertifications CPR = new CPRCertifications();
            if (RegCPRCertificationID > 0)
            {
                CPR.RegCPRCertificationID = RegCPRCertificationID;
            }
            if (RegFirstAidCertificationID > 0)
            {
                CPR.RegFirstAidCertificationID = RegFirstAidCertificationID;
            }
            CPR.RegID = this.WorkflowPage.RegistrationId;
            CPR.IsCPRCertified = Convert.ToBoolean(rblCPRCertificationID.SelectedValue);
            CPR.IsFirstAidCertified = Convert.ToBoolean(rblFirstAidID.SelectedValue);
            CPR.Classifications = txtClassifications.Text;
            CPR.FirstAidClassifications = txtFirstAidTOrg.Text;

            if (!string.IsNullOrEmpty(txtExpirationDate.Text))
                CPR.ExpirationDate = Convert.ToDateTime(txtExpirationDate.Text);
            else
                CPR.ExpirationDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(txtFirstAidExpDate.Text))
                CPR.FirstAidExpirationDate = Convert.ToDateTime(txtFirstAidExpDate.Text);
            else
                CPR.FirstAidExpirationDate = DateTime.MinValue;
            CPR.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);


            Model = CPR;
            if (pnlFieldsCPRCertification.Visible == true)
            {
                RegCPRCertificationID = Convert.ToInt16(presenter.SaveRegCPRCertification(CPR));
                foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadCPRCertification.Controls)
                {
                    svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, RegCPRCertificationID);
                }
            }
            else if (pnlFieldsFirstAidCertification.Visible == true)
            {
                RegFirstAidCertificationID = Convert.ToInt16(presenter.SaveRegFirstAidCertification(CPR));
                foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadFirstAidCertification.Controls)
                {
                    svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, RegFirstAidCertificationID);
                }
            }
            if (grdFirstAidCertification.Rows.Count > 0 || grdCPRCertification.Rows.Count > 0)
            {
                rtn = true;
            }
        }

        else
        {

            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdCPRCertification.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }

        return rtn;
    }

    private void InitFormFields()
    {
        this.rblCPRCertificationID.SelectedIndex = 1;
        this.txtClassifications.Text = txtExpirationDate.Text = string.Empty;
    }

    private void SetFormFields(DataSet affiliationData)
    {

    }

    public void GetRegCPRCertification(DataSet ds)
    {
        //;
    }
    private bool DeleteCPRCerts(DataTable dt, int index)
    {
        bool isValid = true;
        if (dt.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.CPRCertification)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This CPR & First Aid Certification record cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dt != null)
        {
            if (dt.Rows.Count == 1)
            {
                AddError("* CPR Certification cannot be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
            else
            {
                DataRow dr = dt.Rows[index];
                int REG_CPR_Certification_ID = Helper.GetInt("REG_CPR_Certification_ID", dr);
                svc.DeleteRegistrationData("CPR_Certification", "REG_CPR_Certification_ID", REG_CPR_Certification_ID);
            }
        }
        return isValid;
    }

    private bool DeleteFirstAidCerts(DataTable dt, int index)
    {
        bool isValid = true;
        if (dt.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.CPRCertification)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This CPR & First Aid Certification record cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dt != null)
        {
            if (dt.Rows.Count == 1)
            {
                AddError("* First Aid Certification cannot be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
            else
            {
                DataRow dr = dt.Rows[index];
                int REG_FIRSTAID_Certification_ID = Helper.GetInt("REG_FIRSTAID_CERTIFICATION_ID", dr);
                svc.DeleteRegistrationData("FIRSTAID_CERTIFICATION", "REG_FIRSTAID_CERTIFICATION_ID", REG_FIRSTAID_Certification_ID);
            }
        }
        return isValid;
    }

    public override string ValidationGroup
    {
        get { return "valCPRCertifications"; }
    }

    public override string Title
    {
        get { return "CPR Certification"; }
    }

    public override string IdText
    {
        get { return "ucCPRCertification_" + this.WorkflowPage.RegistrationId; }
    }
    private void AddError(string errMsg, ref bool isGood, string validationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = validationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
}