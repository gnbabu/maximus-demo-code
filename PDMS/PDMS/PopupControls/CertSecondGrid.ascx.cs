using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_CertSecondGrid : BaseSectionControl
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

    public bool _ExportHistory
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

    private const string sectionName = "CLIA";
    public DataTable dt;
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CLIA");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        OnLoad(e);
    }

    public bool CanUserViewDelete(int index)
    {
        DataTable dt = this.DataList;
        int modifiedStatusTypeId = dt.Rows[index]["MODIFIED_STATUS_TYPE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[index]["MODIFIED_STATUS_TYPE_ID"]);

        bool retCanDelete = false;
        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            DataSet dsReg = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
            if (Helper.HasRows(dsReg) && Helper.GetInt("RegistrationStatusTypeID", dsReg.Tables[0].Rows[0]) != CON.RegistrationStatusTypeId.Submitted && (modifiedStatusTypeId == 0 || modifiedStatusTypeId == 4))
            {
                retCanDelete = true;
            }
        }
        return retCanDelete;
    }

    /// <summary>
    /// For now linkage will be shown to all users except for provider.
    /// treating provier as external role.
    /// If business comes up with specific user roles who can vew linkage, need to modify this function.
    /// </summary>
    /// <returns></returns>
    public bool canViewCLIALinkage()
    {

        return !Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name);
    }

    private bool ValidateFederalCLIA(string CLIANumber)
    {
        bool isGood = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("CLIALicenseID", CLIANumber);
        if (string.IsNullOrEmpty(hidID.Text))
            hidID.Text = "0";
        parms.Add("REG_CLIA_ID", hidID.Text);
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectCLIALicenseIDByLicenseIDRegID", parms);
        if (ds != null && Helper.HasRows(ds))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* CLIA License number already added.";
            val.ValidationGroup = "CertSecondGrid";
            this.Page.Validators.Add(val);
            isGood = false;

        }
        return isGood;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.CertificationsCLIA)
        {
            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
            int cliaId = 0;
            if (isEdit)
                cliaId = int.Parse(hidID.Text);

            LoadPlaceHolder(cliaId, isEdit, false);
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "CLIA Certification";
    }
    public override void LoadControlData()
    {
        LoadCLIACertifications();
        //if (_ExportHistory)
        //{
            
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CLIAHistory");

        grdCLIAHistory.DataSource = ds.Tables[0];
        grdCLIAHistory.DataBind();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            grdCLIAHistory.MasterTableView.ExportToExcel();
        }
    }


    private void LoadCLIACertifications()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "CLIA");
        DataTable dtCertSecondGrid = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;

        grdCertSecondGrid.DataSource = this.DataList = dtCertSecondGrid;
        grdCertSecondGrid.DataSource = dtCertSecondGrid;
        grdCertSecondGrid.DataBind();
        btnCertSecondGridHistory.Visible = dtCertSecondGrid == null ? false : dtCertSecondGrid.Rows.Count > 0;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        string radiologyValue = "";
        string labValue = "";

        if (cliaAddDetail.Visible)
        {
            Page.Validate("CertSecondGrid");

            if (string.IsNullOrEmpty(prov_Number.Text))
                cvCLIANumber.IsValid = false;
            //if ((string.IsNullOrEmpty(prov_Cert.SelectedValue) || prov_Cert.SelectedValue == "0"))
            //    cvCertType.IsValid = false;
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("CertSecondGrid") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }

            bool isValid = true;
            if (!ValidateExpirationDate())
                return isValid = false;
            if (!ValidateCLIA(prov_Number.Text))
                return isValid = false;
            if (!ValidateFederalCLIA(prov_Number.Text))
                return isValid = false;
            foreach (Control ctrl in PlaceholderUploadCLIA.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isValid &= uploadControl.ValidateData("CertSecondGrid");
            }



            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("CLIA_NUMBER", prov_Number.Text);

            parms.Add("CLIA_CERT_TYPE", ddlCliaCertType.SelectedValue);

            parms.Add("CLIA_EFF_DATE", prov_Start.Text);
            parms.Add("CLIA_END_DATE", prov_End.Text);

            // parms.Add("CLIA_BILL_FOR_SVCS", prov_Bill4Services.SelectedValue);
            parms.Add("CLIA_RADIOLOGY_SVCS", radiologyValue);
            parms.Add("CLIA_LAB_SVCS", labValue);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            int cliaId = 0;
            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
            if (isEdit)
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                parms.Add("REG_CLIA_ID", hidID.Text);
                svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "CLIA", parms);
                cliaId = int.Parse(hidID.Text);
            }
            else
            {
                parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Inserted.ToString());
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                cliaId = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "CLIA", parms);
            }

            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadCLIA.Controls)
            {
                svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, cliaId);
            }

        }
        return true;
    }



    public override void LoadData(DataRow row)
    {
        bool isEdit = false;
        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        int cliaId = isEdit ? Helper.GetInt("REG_CLIA_ID", row) : 0;
        LoadPlaceHolder(cliaId, isEdit);
        if (PlaceholderUploadCLIA.Controls.Count > 0)
            PlaceholderUploadCLIA.Visible = true;
        else
            PlaceholderUploadCLIA.Visible = false;

        ddlCliaCertType.Items.Clear();
        DataSet cliaCertTypes = svc.SelectCLIACertificateTypes();
        if (Helper.HasRows(cliaCertTypes))
        {
            Helper.LoadDropDown(ddlCliaCertType, cliaCertTypes.Tables[0], "CLIA_CERT_NAME", "CLIA_CERTIFICATE_TYPE_ID", true);
            ddlCliaCertType.Items.Insert(0, new ListItem(string.Empty, "0"));
        }

        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
        {
            prov_Number.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = isPending;
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) || Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))
        {
            System.Diagnostics.Debug.WriteLine("Enrollment specialist?");
            if (Helper.IsUserInEnrollmentSpecialistRole(HttpContext.Current.User.Identity.Name))
            {
                System.Diagnostics.Debug.WriteLine("yes");
            }

            prov_Number.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = true;
        }
        else
        {
            prov_Number.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = false;
        }

        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_CLIA_ID"].ToString();

            prov_Number.Text = row["CLIA_NUMBER"].ToString();
            ddlCliaCertType.SelectedValue = row["CLIA_CERT_TYPE"].ToString();
            prov_Start.Text = Helper.FormatDate2(row["CLIA_EFF_DATE"].ToString());
            prov_End.Text = Helper.FormatDate2(row["CLIA_END_DATE"].ToString());

            //pdms_Number.Text = Helper.GetString("PDMS_CLIANumber", row);
            //pdms_State.Text = GetStateName(Helper.GetString("PDMS_CLIAState", row));
            //pdms_Start.Text = Helper.FormatDate2(Helper.GetString("PDMS_CLIAEffDate", row));
            //pdms_End.Text = Helper.FormatDate2(Helper.GetString("PDMS_CLIAEndDate", row));
            SetCLIAFieldsReadOnly(true);
        }
        else
        {
            hidID.Text = string.Empty;
            prov_Number.Text = string.Empty;
            prov_Start.Text = string.Empty;
            prov_End.Text = string.Empty;
            ddlCliaCertType.SelectedValue = "";
            SetCLIAFieldsReadOnly(true);

        }
    }

    protected void grdCertSecondGrid_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.Equals("Page") || e.CommandName.Equals("Filter") || e.CommandName.Equals("ChangePageSize") || e.CommandName.Equals("Sort"))
        {
            return;
        }
        else
        {

            int index = -1;
            DataRow dr = null;

            if (e.CommandName == "DeleteCertSecondGridRow")
            {
                index = Convert.ToInt32(e.CommandArgument);
                dr = this.DataList.Rows[index];

                bool IsDeleted = DeleteCLIACertifications(dr);
                if (IsDeleted)
                {
                    LoadCLIACertifications();
                }
            }
            else if (e.CommandName == "ShowCliaLinkage")
            {
                index = Convert.ToInt32(e.CommandArgument);
                dr = this.DataList.Rows[index];

                if (dr != null)
                {
                    pnlCliaLinkage.Visible = true;

                    string cliaNum = Helper.GetString("CLIA_NUMBER", dr);
                    string cliaCertType = Helper.GetString("CLIA_CERT_TYPE", dr);
                    string cliaEffecive = Helper.GetDate("CLIA_EFF_DATE", dr);
                    string cliaEnd = Helper.GetDate("CLIA_END_DATE", dr);

                    lbLinkCLIANumber.Text = cliaNum;
                    lblLinkCLIACertTypeRst.Text = cliaCertType;
                    lblLinkCliaEffectiveDate.Text = cliaEffecive;
                    lblLinkCliaEndDate.Text = cliaEnd;

                    CLIACertificationsInfo1.CLIANumber = CLIALabCodes1.CLIANumber = cliaNum;
                    CLIACertificationsInfo1.LoadData();
                    CLIALabCodes1.LoadData();
                }
            }
            else if (e.CommandName == "EditCertSecondGridRow")
            {
                cliaAddDetail.Visible = true;

                if (Helper.HasRows(this.DataList))
                    this.LoadData(dr);
                else
                    this.LoadData(null);
            }
        }

    }




    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        cliaAddDetail.Visible = true;
        this.LoadData(null);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "CertSecondGrid":
                lblTitle.Text = "CLIA History";
                ucCertSecondGridHistory.LoadData();
                mpe.Show();
                break;
            case "ShowCliaLinkage":

            default:
                break;
        }
    }


    protected void ValidateLC85(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        if (string.IsNullOrEmpty(prov_Start.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = Convert.ToDateTime(prov_Start.Text) <= DateTime.Now;
        }
    }

    private bool ValidateExpirationDate()
    {
        bool valid = true;

        DateTime ExpirationDate;
        if (DateTime.TryParse(prov_End.Text, out ExpirationDate))
        {
            DateTime StartDate;

            if (DateTime.TryParse(prov_Start.Text, out StartDate))
            {
                if (StartDate >= ExpirationDate)
                {
                    CustomValidator val = new CustomValidator();
                    val.IsValid = false;
                    val.ErrorMessage = "* Expiration dates need to be greater than effective date.";
                    val.ValidationGroup = "CertSecondGrid";
                    this.Page.Validators.Add(val);
                    valid = false;
                }
            }


            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
            int result = DateTime.Compare(ExpirationDate, DateTime.Now);

            if (result < 0) // expiration is before today's date
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int statusCode = Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]);
                    if (statusCode != 6)
                    {
                        CustomValidator val = new CustomValidator();
                        val.IsValid = false;
                        val.ErrorMessage = "* Expiration dates need to be greater than today's date.";
                        val.ValidationGroup = "CertSecondGrid";
                        this.Page.Validators.Add(val);
                        valid = false;
                    }
                }

            }
        }
        return valid;
    }








    public void LoadPlaceHolder(int cliaId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadCLIA.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cliaId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, cliaId, pageSection, isEdit);

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

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                ucUploadSectionControl.IsRequired = Helper.GetBool("IS_REQUIRED", dr);

                //ucUploadSectionControl.RowID = cliaId;

                //ucUploadSectionControl.PageSection = pageSection;

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadCLIA.Controls.Add(ucUploadSectionControl);
            }
        }

    }


    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (!string.IsNullOrEmpty(prov_Number.Text) || !string.IsNullOrEmpty(prov_Start.Text) || !string.IsNullOrEmpty(prov_End.Text))
        {
            isRequired = true;
        }
        else if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
        {
            isRequired = true;
        }
        if (grdCertSecondGrid != null && grdCertSecondGrid.MasterTableView.Items.Count > 0)
        {
            isRequired = true;
        }
        return isRequired;
    }

    public override string ValidationGroup
    {
        get { return "CertSecondGrid"; }
    }

    public override string Title
    {
        get { return "Edit CLIA Numbers"; }
    }

    public override string IdText
    {
        get { return "ucCLIACertification_" + this.WorkflowPage.RegistrationId; }
    }
    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
    private bool DeleteCLIACertifications(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.CertificationsCLIA)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This clia cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_CLIA_ID = Helper.GetInt("REG_CLIA_ID", dr);
            int CLIA_ID = Helper.GetInt("CLIA_ID", dr);
            if (CLIA_ID == 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("CLIA", "REG_CLIA_ID", REG_CLIA_ID);
            }
            else
            {
                AddError("* Clia cannot be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
        }
        return isValid;
    }



    protected void prov_Number_TextChanged(object sender, EventArgs e)
    {

        ValidateCLIA(prov_Number.Text);

    }

    private void SetCLIAFieldsReadOnly(bool value)
    {

        prov_Start.ReadOnly = prov_End.ReadOnly = value;
        prov_Start.Enabled = prov_End.Enabled = !value;
        ddlCliaCertType.Enabled = !value;
        calStart.Enabled = calEnd.Enabled = !value;
    }

    protected void grdCertSecondGrid_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            if (dataItem != null)
            {
                LinkButton btn = (LinkButton)dataItem.FindControl("lnkBtnCLIA");
                Label lbl = (Label)dataItem.FindControl("lblCLIANo");

                if (btn != null && lbl != null)
                {
                    if (canViewCLIALinkage())
                    {
                        btn.Style["display"] = "block";
                        lbl.Style["display"] = "none";
                    }
                    else
                    {
                        btn.Style["display"] = "none";
                        lbl.Style["display"] = "block";
                    }
                }
            }
        }
        catch (Exception)
        {
            // Optional: log error or handle gracefully
        }
    }



    private bool ValidateCLIA(string prov_number)
    {
        bool isGood = true;
        //check if the CLIA number has 10 digits then  get it from Source.
        string clia_number = prov_number.Trim();
        Regex rgx = new Regex(@"^\d{2}[Dd]{1}\d{7}$");
        Match m = rgx.Match(clia_number);

        if (m.Success)
        {
            DataSet dsClia = svc.SelectCLIACertificateDatesByCLIANumber(this.WorkflowPage.RegistrationId, clia_number);

            if (Helper.HasRows(dsClia) && Helper.HasRows(dsClia.Tables[0]))
            {
                DataTable dt = dsClia.Tables[0];
                string certificate_type = Helper.GetString("Certificate_Type", dt.Rows[0]);
                ddlCliaCertType.SelectedValue = certificate_type;

                //Get the first row and then populate and set the fields read only
                //This may change in future to allow one clia to multiple certificates dispaly.
                DataRow dr = dt.Rows[0];
                DataRow drReg = dsClia.Tables[1].Rows[0];
                prov_Start.Text = string.IsNullOrEmpty(Helper.FormatDate2(Helper.GetString("CHANGE_EFFECTIVE_DATE", drReg)))
                    ? Helper.FormatDate2(DateTime.Now.ToString()) : Helper.FormatDate2(Helper.GetString("CHANGE_EFFECTIVE_DATE", drReg));
                prov_End.Text = Helper.FormatDate2(Helper.GetString("Expiration_Date", dr));

                string cliaEffectiveDate = Helper.FormatDate2(Helper.GetString("Effective_Date", dr));
                if(Convert.ToDateTime(prov_Start.Text) < Convert.ToDateTime(cliaEffectiveDate))
                {
                    prov_Start.Text = cliaEffectiveDate;
                }
                //set fields read only
                SetCLIAFieldsReadOnly(true);
            }
            else
            {
                bool isValid = false;
                AddError("* No CLIA Information Found", ref isValid, ValidationGroup);
                prov_Start.Text = "";
                prov_End.Text = "";
                isGood = false;
            }
        }
        else
        {
            bool isValid = false;
            AddError("* Enter a valid 10 digit CLIA number in the expected format (2 digits, 'D', 7 digits).", ref isValid, ValidationGroup);
            prov_Start.Text = "";
            prov_End.Text = "";
            isGood = false;
        }
        return isGood;
    }
}