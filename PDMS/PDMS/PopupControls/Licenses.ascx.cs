using Corp.Core.Libraries.Helper;
using MAXIMUS.Core.Libraries;
using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_Licenses : BaseSectionControl, IProfessionalLicenseView
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
    int _noOfSpecialties = 1;

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
    public int NoOfSpeaciltyFocuses
    {
        get
        {
            if (ViewState["SpeaciltyFocusesCount"] == null) ViewState["SpeaciltyFocusesCount"] = _noOfSpecialties;
            return (int)ViewState["SpeaciltyFocusesCount"];
        }
        set { ViewState["SpeaciltyFocusesCount"] = value; _noOfSpecialties = value; }
    }
    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }

    public int RegLisensureID
    {
        get
        {
            return ViewState["RegLisensureID"] == null ? 0 : Convert.ToInt32(ViewState["RegLisensureID"]);
        }
        set
        {
            ViewState["RegLisensureID"] = value;
        }
    }

    private ProfessionalLicensePresenter _presenter;

    public ProfessionalLicensePresenter presenter
    {
        get
        {
            if (_presenter == null)
            {
                _presenter = new ProfessionalLicensePresenter(this);
            }

            return _presenter;
        }
    }

    public ProfessionalLicense Model { get; set; }
    public static DataTable dtLicenseTypes = new DataTable();
    private const string SectionName = "License";
    private static string _rowindex = string.Empty;
    private DataTable dt;
    public Address AddressModel { get; set; }
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSES");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    public bool CanUserViewDelete(bool bverified)
    {        

        if (Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            //everified licenses should not have a ability to delete the record.
            return !bverified;
        }
        else
        {
            return false;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {

        LoadDynamicSpecialtyFocusControls();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ucLicenseAddress.StateVisible = true;
        ucLicenseAddress.IsAddressRequired = true;
        ucLicenseAddress.LoadState();
       

            if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Licenses)
        {
            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
            int licenseId = 0;

            if (isEdit)
            {
                licenseId = int.Parse(hidID.Text);
                RegLisensureID = licenseId;
            }

            LoadDynamicSpecialtyFocusControls();
            if (!string.IsNullOrEmpty(ucLicenseAddress.State))
            {
                ucSep1.Visible = true;
                LoadPlaceHolder(0, false, false, "License");
                PlaceholderUploadProfessionalLicense.Visible = true;
            }
            if (hidVerified.Text == "true")
            {
                ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = false;
            }
            else
            {
                ucSep1.Visible = true;
                LoadPlaceHolder(0, false, false, "License");
                PlaceholderUploadProfessionalLicense.Visible = true;
            }
        }
    }

    public void LoadPlaceHolder(int licensureId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = SectionName, bool? uploadRequired = false)
    {
        Upload upload = new Upload();

        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, licensureId, pageSection, isEdit);

        for (int i = 0; i <= ds.Tables.Count - 1; i++)
        {
            foreach (DataRow dr in ds.Tables[i].Rows)
            {

                UserControls_UploadSectionControl ucUploadSectionControl =
                    LoadControl("~/PopupControls/UploadSectionControl.ascx") as UserControls_UploadSectionControl;

                ucUploadSectionControl.Title = Helper.GetString("TITLE", dr);

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

                ucUploadSectionControl.DestinationPath = @"C:\project\temp";

                if (uploadRequired.HasValue && uploadRequired.Value)
                    ucUploadSectionControl.IsRequired = true;
                else
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

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";

                if (PlaceholderUploadProfessionalLicense.FindControl(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr)) == null)
                    PlaceholderUploadProfessionalLicense.Controls.Add(ucUploadSectionControl);
            }
        }
    }

    private bool ValidateLicense(string license_number)
    {
        bool isGood = true;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("license_number", license_number);
        if (string.IsNullOrEmpty(hidID.Text))
            hidID.Text = "0";
        parms.Add("REG_LICENSURE_ID", hidID.Text);
        DataSet ds = svc.SelectRegistrationDataWithParams("usp_SelectREG_LicenseIDByLicenseIDRegID", parms);
        if (ds != null && Helper.HasRows(ds))
        {
            AddError("* License number already exists for the provider.  Please update the expiration date of the existing license.", ref isGood, "valLicenses");
        }

        return isGood;
    }
    public override bool SaveData()
    {
        Page.Validate("valLicenses");
        bool isGood = true;
        //Do Validation Only with License Details exist.
        if (licenseDetail.Visible)
        {

            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("valLicenses") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }

            bool isValid = true;
            ProfessionalLicense lic = new ProfessionalLicense();

            if(prov_Number.Text.Trim().Length > 25)
            {
                AddError("* Enter a License Number with max 25 characters.", ref isGood, "valLicenses");
                isValid = false;
            }    

            if (!ValidateLicense(prov_Number.Text))
                isValid = false;
            if (!isValid)
                return isValid;

            lic.ELicenseVerified = (hidVerified.Text == "true") ? true : false;


            foreach (Control ctrl in PlaceholderUploadProfessionalLicense.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                uploadControl.IsRequired = !lic.ELicenseVerified;

                isValid &= uploadControl.ValidateData("valLicenses");
            }

            if (!isValid)
                return isValid;


            int licenseId = 0;


            lic.RegID = this.WorkflowPage.RegistrationId;
            lic.LicenseNumber = prov_Number.Text.Trim();
            lic.LicenseState = ddlLicenseState.SelectedValue;
            lic.LicenseStatus = ddlLicenseStatus.SelectedItem.ToString();
            if(lic.ELicenseVerified)
                 lic.LicenseSubStatus = hidLicenseSubStatus.Text;
            lic.LicenseType = ddlBoardName.SelectedValue;
            lic.RegLicensureID = string.IsNullOrEmpty(hidID.Text) ? 0 : int.Parse(hidID.Text);
            if (ddlBoardName.SelectedItem.ToString().ToLower().Trim() != "other")
            {
                lic.LicenseBoardName = ddlBoardName.SelectedItem.ToString();
            }
            else
            {
                lic.LicenseBoardName = txtBoard.Text.Trim();
            }
                       if (!string.IsNullOrEmpty(prov_Effective.Text))
                lic.LicenseEffectiveDate = Convert.ToDateTime(prov_Effective.Text);
            else
                lic.LicenseEffectiveDate = DateTime.MinValue;

            if (!string.IsNullOrEmpty(prov_End.Text))
                lic.LicenseExpirationDate = Convert.ToDateTime(prov_End.Text);
            else
                lic.LicenseExpirationDate = DateTime.MinValue;

            lic.UserID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            lic.Created_By_User = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            List<SpecialtyFocusCertificate> listSpecialty = new List<SpecialtyFocusCertificate>();

            //OHPNM-3694 : Bypass endorsement validation if license type is e-license
            if (ddlBoardName.SelectedItem.Text.ToLower().Trim() == "other" && ddlLicenseState.SelectedItem.Value != AppSettings.Get("StateCode"))
            {

                foreach (PopupControls_SpecialityFocus specialFocus in plcHolderFocus1.Controls)
                {
                    SpecialtyFocusCertificate spCert = new SpecialtyFocusCertificate();
                    if (string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) && (!string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus)
                        || !string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate)))
                    {
                        AddError("* Enter Endorsement Number.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.Focus) && (!string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus) || !string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate)))
                    {
                        AddError("* Enter Endorsement Focus.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) && (!string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus) || !string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate)))
                    {
                        AddError("* Enter Endorsement Speciality.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus) && (!string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate)))
                    {
                        AddError("* Enter Endorsement Status.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) && (!string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus)))
                    {
                        AddError("* Enter Certifying Organization.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.CertificationDate) && (!string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus)))
                    {
                        AddError("* Enter Certificate Date.", ref isGood, "valLicenses");

                    }
                    if (string.IsNullOrWhiteSpace(specialFocus.CertificationExpirationDate) && (!string.IsNullOrWhiteSpace(specialFocus.CertifyingOrganization) || !string.IsNullOrWhiteSpace(specialFocus.CertificationDate) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementNumber) || !string.IsNullOrWhiteSpace(specialFocus.Focus) || !string.IsNullOrWhiteSpace(specialFocus.CertificateSpecialty) || !string.IsNullOrWhiteSpace(specialFocus.EndorsementStatus)))
                    {
                        AddError("* Enter Certification Expiration.", ref isGood, "valLicenses");

                    }
                    if (!isGood)
                        return isGood;
                    spCert.EndorsementNumber = specialFocus.EndorsementNumber;
                    spCert.CertifyingOrganization = specialFocus.CertifyingOrganization;
                    spCert.EndorsementFocus = specialFocus.Focus;
                    spCert.EndorsementSpecialty = specialFocus.CertificateSpecialty;
                    spCert.CertificateDate = !string.IsNullOrEmpty(specialFocus.CertificationDate) ? Convert.ToDateTime(specialFocus.CertificationDate) : lic.LicenseEffectiveDate;
                    spCert.CertificateExpirationDate = !string.IsNullOrEmpty(specialFocus.CertificationExpirationDate) ? Convert.ToDateTime(specialFocus.CertificationExpirationDate) : lic.LicenseExpirationDate;
                    spCert.EndorsementStatus = specialFocus.EndorsementStatus;
                    listSpecialty.Add(spCert);
                }
            }
            else
            {
                foreach (PopupControls_SpecialityFocus specialFocus in plcHolderFocus1.Controls)
                {
                    SpecialtyFocusCertificate spCert = new SpecialtyFocusCertificate();
                    spCert.EndorsementNumber = specialFocus.EndorsementNumber;
                    spCert.CertifyingOrganization = specialFocus.CertifyingOrganization;
                    spCert.EndorsementFocus = specialFocus.Focus;
                    spCert.EndorsementSpecialty = specialFocus.CertificateSpecialty;
                    spCert.CertificateDate = !string.IsNullOrEmpty(specialFocus.CertificationDate) ? Convert.ToDateTime(specialFocus.CertificationDate) : lic.LicenseEffectiveDate;
                    spCert.CertificateExpirationDate = !string.IsNullOrEmpty(specialFocus.CertificationExpirationDate) ? Convert.ToDateTime(specialFocus.CertificationExpirationDate) : lic.LicenseExpirationDate;
                    spCert.EndorsementStatus = specialFocus.EndorsementStatus;
                    listSpecialty.Add(spCert);
                }

            }
            lic.SpecialtyFocusCertificates = listSpecialty;
            lic.Address1 = ucLicenseAddress.StreetAddress;
            lic.Address2 = ucLicenseAddress.UnitAddress;
            lic.City = ucLicenseAddress.City;
            lic.State = ucLicenseAddress.State;
            //lic.County = ucLicenseAddress.County;
            lic.RegAddressID = RegAddressId;
            if (!string.IsNullOrEmpty(ucLicenseAddress.County))
            {
                string County = ucLicenseAddress.County;
                string SplitText = "-CountyName-";
                string[] CountyCodeWithName = Regex.Split(County, SplitText);
                if (CountyCodeWithName.Length == 2)
                {
                    lic.County = CountyCodeWithName[0];
                    lic.CountyName = CountyCodeWithName[1];
                }
            }
            else
            {
                lic.County = string.Empty;
                lic.CountyName = string.Empty;
            }
            lic.Zip = ucLicenseAddress.Zip5;


            Model = lic;

          
            licenseId = presenter.SaveRegprofessionalLicenseInformation(lic);

            string notes = lic.RegLicensureID > 0 ? "Professional License Updated" : "Professional License Added";

            ProviderFeedHelper.InsertProviderFeedNotes(this.WorkflowPage.RegistrationId, 0, HttpContext.Current.User.Identity.Name, notes, null, null, null, this.WorkflowPage.WF_ProcessID);

            foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadProfessionalLicense.Controls)
            {
                svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, licenseId);
            }
            _rowindex = string.Empty;
            return true;
        }
        else
        {
            // OHPNM-2229  
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdLicenses.Rows.Count == 0)
            {
                // this page is required, but there aren't any items, and they didn't just pass validation for an item they are adding, so don't let them 'save' the data on the screen
                return false;
            }

            return true;
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Professional Licenses";
    }
    public override void LoadControlData()
    {
        LoadLicenses();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSESHistory");
            if (Helper.HasRows(ds))
            {
                grdLicensesHistory.DataSource = ds.Tables[0];
                grdLicensesHistory.DataBind();
                grdLicensesHistory.MasterTableView.ExportToExcel();
            }
        }
    }

    private void LoadLicenses()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "LICENSES");
        try
        {
            if (Helper.HasRows(ds))
            {
                DataTable dtLicenses = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
                grdLicenses.DataSource = this.DataList = dtLicenses;
                grdLicenses.DataBind();
                //btnLicensesHistory.Visible = (Helper.HasRows(dtLicenses));
                if (!string.IsNullOrEmpty(_rowindex))
                {
                    this.LoadData(this.DataList.Rows[Convert.ToInt32(_rowindex)]);
                }
            }
        }
        catch (Exception)
        {

        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddLicenses.Visible = false;
            }
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        _rowindex = e.CommandArgument.ToString();
        
        DataRow dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteLicensesCodeRow")
        {
            bool isDeleted = DeleteLicense(dr);

            if (isDeleted)
            {
                _rowindex = string.Empty;
                LoadLicenses();
                licenseDetail.Visible = false;
            }
        }
        else
        {
            licenseDetail.Visible = true;
            this.LoadData(Helper.HasRows(this.DataList) ? dr : null);
            _rowindex = string.Empty;
        }
    }

    private bool DeleteLicense(DataRow dr)
    {
        bool isValid = true;
        if (this.DataList.Rows.Count == 1 && Registration.EntryIsRequired(this.WorkflowPage.RegistrationId, CON.RegistrationPageName.Certification, Registration.GetSectionNameFromStepNumber(CON.SectionTypeID.Licenses)))
        //if only one record and required page then prevent deletion or else the page will not turn back to blue once it is green and will cause issues for page submission
        {
            AddError("* This is required section. This license cannot be deleted.", ref isValid, ValidationGroup);
            return isValid;
        }
        if (dr != null)
        {
            int REG_LICENSURE_ID = Helper.GetInt("REG_LICENSURE_ID", dr);

            if (REG_LICENSURE_ID > 0)
            {
                PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                psc.DeleteRegistrationData("LICENSE", "REG_LICENSE_ID", REG_LICENSURE_ID);
            }
            else
            {
                AddError("* License cannot be deleted.", ref isValid, ValidationGroup);
                return isValid;
            }
        }
        return isValid;
    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        licenseDetail.Visible = true;
        _rowindex = string.Empty;
        this.LoadData(null);
        //Helper.SetReadOnly(licenseDetail, false);
    }

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
        switch (e.CommandName)
        {
            case "Licenses":
                lblTitle.Text = "Licenses History";
                ucLicensesHistory.LoadData();
                mpe.Show();
                break;
            default:
                break;
        }
    }


    public override bool ValidateData()
    {
        return true;
    }

    public override void LoadData(DataRow row)
    {
        bool isEdit = false;
        isEdit = row != null;

        int licensureId = isEdit ? Helper.GetInt("REG_LICENSURE_ID", row) : 0;


        if (ddlBoardName.Items.Count == 0)
        {
            Helper.LoadList(ddlBoardName, GetLicenseBoardTypes(), "LICENSE_TYPE_NAME", "LICENSE_TYPE_ID", true);
        }
        if (ddlLicenseStatus.Items.Count == 0)
            Helper.LoadList(ddlLicenseStatus, svc.SelectLicenseCurrentStatuses(), "LICENSE_CURRENT_STATUS_DESC", "MMIS_LICENSE_CURRENT_STATUS_ID", true);

        Helper.LoadDropDownListWithStates(ref ddlLicenseState);


        hidIsEdit.Text = isEdit.ToString();
        bool isConverted = false;
        string licenseVerified = "0";
        //int RegAddressId = 0;
        bool isLicenseUpdateAllowed = IsLicenseUpdateAllowed(isConverted);
        bool isManual = false;
        bool canUserEditRegistration = Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);
        if (isEdit)
        {
            hidID.Text = row["REG_LICENSURE_ID"].ToString();
            prov_Number.Text = row["LICENSE_NUMBER"].ToString();
            licensureId = Helper.GetInt("REG_LICENSURE_ID", row);
            licenseVerified = Helper.GetString("ELICENSE_VERIFIED", row);
            RegAddressId = Helper.GetInt("REG_ADDRESSID", row);

            ddlLicenseState.SelectedValue = row["LICENSE_STATE"].ToString();
            if (ddlBoardName.Items.FindByText(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["LICENSE_TYPE_NAME"].ToString().ToLower())) != null)
            {
                if (row["LICENSE_TYPE_NAME"].ToString().ToLower().Trim() == "other")  
                {
                    txtBoard.Text = row["LICENSE_BOARD_NAME"].ToString();
                }
                ddlBoardName.SelectedIndex = ddlBoardName.Items.IndexOf(ddlBoardName.Items.FindByText(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row["LICENSE_TYPE_NAME"].ToString().ToLower())));

            }

            if (row["LICENSE_TYPE_NAME"].ToString().ToLower().Trim() != "other" && ddlLicenseState.SelectedItem.Value == AppSettings.Get("StateCode") && licensureId > 0)
            {
                string strLicenseTypeId = ddlBoardName.SelectedValue;
                //DataTable dt = dsLicenseTypes.Tables[0];
                string eLicenseBoardName = dtLicenseTypes.AsEnumerable().Where(r => r.Field<int>("LICENSE_TYPE_ID")
                                                                           == Convert.ToInt32(strLicenseTypeId)).Select(r => r.Field<string>("LICENSE_TYPE_NAME_IN_ELICENSE")).FirstOrDefault();
                bool retVal = RequestELicenseVerification(eLicenseBoardName.ToLower().Trim(), strLicenseTypeId, licensureId, prov_Number.Text.Trim());
                isManual = !retVal;
                licenseVerified = "1";
            }
            else
            {
                isManual = true;
            }

            if (isManual)
            {
                if (Helper.FindByTextCaseInsensitive(ddlLicenseStatus, row["LICENSE_STATUS"].ToString()) != null)
                {
                    ddlLicenseStatus.SelectedIndex = ddlLicenseStatus.Items.IndexOf(Helper.FindByTextCaseInsensitive(ddlLicenseStatus, row["LICENSE_STATUS"].ToString()));
                }

                prov_Effective.Text = Helper.FormatDate2(row["LICENSE_EFF_DATE"].ToString());
                prov_End.Text = Helper.FormatDate2(row["LICENSE_END_DATE"].ToString());                

                // OHPNM-2117 - if not everified or is out of state license, they need to re-enter their expiration date and upload license doc
                // TODO - not actually sure how to do the document part because to delete it would be to delete it permanently unlike the expiration date
                if (licenseVerified == "False" || AppSettings.Get("StateCode", string.Empty) != ddlLicenseState.SelectedValue)
                {
                    prov_End.Text = "";
                }

                // Load and Fill the specialty focus 
                DataTable dtSpecialty = presenter.GetLicenseSpecialtyFocusInformation(licensureId).Tables[0];

                if (Helper.HasRows(dtSpecialty))
                {
                    NoOfSpeaciltyFocuses = dtSpecialty.Rows.Count;
                    for (int i = 0; i <= dtSpecialty.Rows.Count - 1; i++)
                    {
                        LoadDynamicSpecialtyFocusControls();
                    }
                    int counter = 0;

                    foreach (PopupControls_SpecialityFocus specialFocus in plcHolderFocus1.Controls)
                    {
                        if (counter < dtSpecialty.Rows.Count)
                        {
                            specialFocus.Focus = Helper.GetString("ENDORSEMENT_FOCUS", dtSpecialty.Rows[counter]);
                            specialFocus.CertificateSpecialty = Helper.GetString("ENDORSEMENT_SPECIALITY", dtSpecialty.Rows[counter]);
                            specialFocus.CertifyingOrganization = Helper.GetString("CERTIFYING_ORGANIZATION", dtSpecialty.Rows[counter]);
                            specialFocus.CertificationDate = Helper.GetDate("CERTIFICATE_DATE", dtSpecialty.Rows[counter]);
                            specialFocus.CertificationExpirationDate = Helper.GetDate("CERTIFICATE_EXPIRATION", dtSpecialty.Rows[counter]);
                            specialFocus.EndorsementStatus = Helper.GetString("ENDORSEMENT_STATUS", dtSpecialty.Rows[counter]);
                            specialFocus.EndorsementNumber = Helper.GetString("ENDORSEMENT_NUMBER", dtSpecialty.Rows[counter]);
                            counter++;
                        }
                    }
                }

                //Load Address
                if (RegAddressId > 0)
                {
                    DataRow drAddress = presenter.GetLicenseAddress(this.WorkflowPage.RegistrationId, RegAddressId);
                    if (drAddress != null)
                        LoadLicenseAddress(Helper.GetString("ADDRESS1", drAddress), Helper.GetString("ADDRESS2", drAddress), Helper.GetString("CITY", drAddress), Helper.GetString("STATE", drAddress), Helper.GetString("ZIP", drAddress), Helper.GetString("COUNTY", drAddress));
                }
                
            }
            if (licenseVerified == "1" || licenseVerified.ToLower() == "true")
            {
                DisplayEverified(true);
                canUserEditRegistration = isLicenseUpdateAllowed = false; //As its already everified
                EnableLicenseEditScreen(!canUserEditRegistration);
                SetLicenseEndDateToReadOnly(false); //can edit only end dates.
                                                    // PlaceholderUploadProfessionalLicense.Controls.Clear(); // As not for Everified.
                ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = false;
                trEditHelpText.Visible = isEdit;
            }
            else
            {
                ucSep1.Visible = true;
                DisplayEverified(false);
                EnableLicenseEditScreen(!canUserEditRegistration);
                LoadPlaceHolder(licensureId, isEdit, false, "License", true);
                ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = true;
            }

        }
        else
        {
            InitFormFields();
            DisplayEverified(false);
            EnableLicenseEditScreen(!canUserEditRegistration);
            LoadPlaceHolder(0, isEdit, false, "License", true);
            ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = true;
        }
    }

    private DataTable GetLicenseBoardTypes()
    {

        DataTable retLicTypes = null;
        try
        {
            DataSet dsLicenseTypes = svc.SelectLicenseTypeSpecialtyTypeByProviderType(this.WorkflowPage.ProviderTypeID.ToString());
            dtLicenseTypes = retLicTypes = dsLicenseTypes.Tables[0];
        }
        catch (Exception)
        {

        }
        return retLicTypes;
    }

    protected void ValidateLCEndDate(object sender, ServerValidateEventArgs e)
    {

        DateTime? endDate = null;
        DateTime tmp;

        if (DateTime.TryParse(this.prov_End.Text.Trim(), out tmp))
        {
            endDate = tmp;
        }
        e.IsValid = (!endDate.HasValue) ? false : (endDate.Value <= DateTime.Now) ? false : true;

    }
    protected void ValidateLCStartDate(object sender, ServerValidateEventArgs e)
    {

        DateTime? startDate = null;

        DateTime tmp;
        if (DateTime.TryParse(this.prov_Effective.Text.Trim(), out tmp))
        {
            startDate = tmp;
        }
        e.IsValid = (!startDate.HasValue) ? false : (startDate.Value > DateTime.Now) ? false : true;
    }
    protected void prov_Number_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prov_Number.Text) &&
            System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z0-9]$", prov_Number.Text))
        {
            prov_Number.Text.Remove(prov_Number.Text.Length - 1);
        }
    }

    public override string ValidationGroup
    {
        get { return "valLicenses"; }
    }

    public override string Title
    {
        get { return "Professional License"; }
    }

    public override string IdText
    {
        get { return "ucLicenses_" + this.WorkflowPage.RegistrationId; }
    }

    public int RegAddressId { get; set; }
    private void AddError(string errMsg, ref bool isGood, string ValidationGroup)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = ValidationGroup;
        this.Page.Validators.Add(val);
        isGood = false;
    }
    private void DisplayEverified(bool value)
    {
        imgcheck.Visible = value;
    }
    private void LoadDynamicSpecialtyFocusControls()
    {
        int index = NoOfSpeaciltyFocuses;

        for (int i = 1; i < index; i++)
        {
            PopupControls_SpecialityFocus ucSpecialityFocus = LoadControl("~/PopupControls/SpecialityFocus.ascx") as PopupControls_SpecialityFocus;
            ucSpecialityFocus.ID = "SpecialityFocus" + i.ToString();
            ucSpecialityFocus.Title = "SpecialityFocus" + i.ToString();
            if (plcHolderFocus1.FindControl("SpecialityFocus" + i.ToString()) == null)
            {
                plcHolderFocus1.Controls.Add(ucSpecialityFocus);
            }
        }
    }
    protected void ImgButtonSpecialty_Click(object sender, CommandEventArgs e)
    {
        NoOfSpeaciltyFocuses++;
        LoadDynamicSpecialtyFocusControls();
        ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = true;
        LoadPlaceHolder(string.IsNullOrEmpty(hidID.Text) ? 0 : int.Parse(hidID.Text), string.IsNullOrEmpty(hidIsEdit.Text) ? false : true, false, "License");
    }
    private void SetLicenseEndDateToReadOnly(bool value)
    {
        prov_End.ReadOnly = value;
        prov_End.Enabled = !value;
        calEnd.Enabled = !value;
        prov_End.BackColor = (value) ? System.Drawing.Color.LightGray : System.Drawing.Color.White;
    }
    private void EnableLicenseEditScreen(bool value)
    {
        //except end date every field should be disabled
        //set to False
        prov_Number.Enabled = !value;
        prov_Effective.Enabled = !value;
        CalendarExtender1.Enabled = !value;
        prov_End.Enabled = !value;
        calEnd.Enabled = !value;
        ddlLicenseStatus.Enabled = !value;
        ddlLicenseState.Enabled = !value;
        txtBoard.Enabled = !value;
        ddlBoardName.Enabled = !value;

        //Specify the Background
        prov_Number.BackColor = prov_Effective.BackColor = txtBoard.BackColor = ddlBoardName.BackColor = prov_End.BackColor =
           ddlLicenseStatus.BackColor = ddlLicenseState.BackColor = (value) ? System.Drawing.Color.LightGray : System.Drawing.Color.White;

        if (plcHolderFocus1.Controls.Count > 0)
        {
            //set controls read only
            foreach (PopupControls_SpecialityFocus specialFocus in plcHolderFocus1.Controls)
            {
                specialFocus.SetReadonly(value);

            }
        }
        ucLicenseAddress.EnableAddressFields = !value;
    }


    protected void ddlBoardName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Decide Elicense Verification or manual
        bool isManual = false;

        if (ddlBoardName.SelectedIndex == -1 || ddlBoardName.SelectedIndex == 0)
        {
            isManual = true;
        }
        else
        {
            string boardName = ddlBoardName.SelectedItem.Text.ToLower().Trim();

            if (boardName != "other")
            {
                txtBoard.Text = string.Empty;
            }

            if (boardName != "other" && ddlLicenseState.SelectedItem.Value == AppSettings.Get("StateCode"))
            {
                string strLicenseTypeId = ddlBoardName.SelectedValue;
                //DataTable dt = dsLicenseTypes.Tables[0];
                string eLicenseBoardName = dtLicenseTypes.AsEnumerable().Where(r => r.Field<int>("LICENSE_TYPE_ID")
                                                                           == Convert.ToInt32(strLicenseTypeId)).Select(r => r.Field<string>("LICENSE_TYPE_NAME_IN_ELICENSE")).FirstOrDefault();
                bool retVal = RequestELicenseVerification(eLicenseBoardName.ToLower().Trim(), strLicenseTypeId);
                isManual = !retVal;
            }
            else
            {
                //Manual
                isManual = true;
            }
        }
        
        //add extra speacilty focus option is only for manual license screen.
        ImgSpecialtyFocus.Visible = isManual;
        DisplayEverified(!isManual);
        EnableLicenseEditScreen(!isManual);

        LoadPlaceHolder(string.IsNullOrEmpty(hidID.Text) ? 0 : int.Parse(hidID.Text), string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text), false, "License", uploadRequired: isManual);
        ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = isManual;
        lblInfoEliceseVerified.Visible = isManual;
                
    }

    private bool RequestELicenseVerification(string boardName, string licenseTypeId, int RegLicensureID = 0, string lisenseNumber = "")
    {
        int regID = this.WorkflowPage.RegistrationId;
        DataSet dsReg = svc.SelectRegistrationByRegID(regID);

        bool retVal = false;
        try
        {
            if (Helper.HasRows(dsReg))
            {
                string lname = Helper.GetString("LastName", dsReg.Tables[0].Rows[0]);
                string dob = Helper.GetDate("BirthDate", dsReg.Tables[0].Rows[0]);
                string taxID = Helper.GetString("TaxID", dsReg.Tables[0].Rows[0]);
                string last4 = string.Empty;
                if (!string.IsNullOrEmpty(lname) && !string.IsNullOrEmpty(dob) && !string.IsNullOrEmpty(taxID) && !string.IsNullOrEmpty(boardName))
                {
                    last4 = taxID.Substring(5);
                    lname = lname.Trim();
                    DateTime dtConverted = DateTime.ParseExact(dob, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                    string dobConverted = dtConverted.ToString("yyyy-MM-dd");
                    if (!string.IsNullOrEmpty(last4) && !string.IsNullOrEmpty(dobConverted))
                    {
                        ELicenseVerificationRequest eRequest = new ELicenseVerificationRequest(lname, dobConverted, last4, boardName);
                        Object response = ElicenseVerificationHelper.GetLicenseVerificationResult(eRequest, regID, licenseTypeId, this.DataList, RegLicensureID, lisenseNumber);

                        if (response != null)
                        {

                            if (!response.ToString().Contains("No license found"))
                            {
                                ELicenseVerificationResponse responseObj = (ELicenseVerificationResponse)response;
                                bool validActiveStatus = CompareELicenseStatus(responseObj);
                                if (validActiveStatus)
                                {
                                    PopulateElicenseVerificationInformation(responseObj);
                                    retVal = true;
                                    hidVerified.Text = "true";                                    
                                    ucSep1.Visible = PlaceholderUploadProfessionalLicense.Visible = false;
                                }
                            }


                        }

                    }
                }


            }
        }
        catch (Exception)
        {
            retVal = false;
        }

        return retVal;
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

    private bool CompareELicenseStatus(ELicenseVerificationResponse responseObj)
    {
        bool active = false;
        try
        {
            if (responseObj != null)
            {
                Dictionary<string, string> ValidLicenseStatus = ElicenseVerificationHelper.ActiveElicenseStatus();
                if (ValidLicenseStatus.ContainsKey(responseObj.license_sub_status) && ValidLicenseStatus.ContainsValue(responseObj.license_status))
                    active = true;
            }
        }
        catch (Exception)
        {
            //Its always better to log third party services exception.
            //To do Item for later
        }
        return active;
    }

    private void PopulateElicenseVerificationInformation(ELicenseVerificationResponse obj)
    {
        try
        {

            prov_Number.Text = obj.license_number;
            prov_Effective.Text = Helper.FormatDate2(obj.begin_date);
            prov_End.Text = Helper.FormatDate2(obj.end_date);
            if (ddlLicenseStatus.Items.Count > 0)
            {

                if (Helper.FindByTextCaseInsensitive(ddlLicenseStatus, obj.license_status) != null)
                {
                    ddlLicenseStatus.SelectedIndex = ddlLicenseStatus.Items.IndexOf(Helper.FindByTextCaseInsensitive(ddlLicenseStatus, obj.license_status));
                }
            }
            hidLicenseSubStatus.Text = obj.license_sub_status;
            //Load Focus
            if (obj.endorsements != null && obj.endorsements.Count() > 0)
            {
                NoOfSpeaciltyFocuses = 0;
                foreach (endorsement en1 in obj.endorsements)
                {
                    specialityandfocus[] sp1 = en1.specialityandfocus;
                    int count = (sp1 != null) ? sp1.Length : 0;
                    NoOfSpeaciltyFocuses += count == 0 ? 1 : count;

                }
                LoadDynamicSpecialtyFocusControls();
                int counter = 0;
                for (int i = 0; i < obj.endorsements.Count(); i++)
                {
                    endorsement en = obj.endorsements[i];
                    if (en != null)
                    {
                        specialityandfocus[] sp = en.specialityandfocus;
                        int count = (sp != null) ? sp.Length : 0;
                        count = count == 0 ? 1 : count;


                        //fill data from elicense verification data
                        for (int j = 0; j < count; j++)
                        {
                            PopupControls_SpecialityFocus specialFocus = (PopupControls_SpecialityFocus)plcHolderFocus1.Controls[counter];
                            specialFocus.EndorsementStatus = en.endorsement_status;
                            specialFocus.EndorsementNumber = en.endorsement_number;
                            if (sp.Length > 0)
                            {
                                specialFocus.Focus = sp[j].endorsement_focus;
                                specialFocus.CertificateSpecialty = sp[j].endorsement_speciality;
                                specialFocus.CertifyingOrganization = sp[j].certifying_organization;
                                specialFocus.CertificationDate = !string.IsNullOrEmpty(sp[j].certificate_date) && (sp[j].certificate_date != "XXXX") ? Helper.FormatDate2(sp[j].certificate_date) : string.Empty;
                                specialFocus.CertificationExpirationDate = !string.IsNullOrEmpty(sp[j].certificate_expiration) && (sp[j].certificate_expiration != "XXXX") ? Helper.FormatDate2(sp[j].certificate_expiration) : string.Empty;
                            }
                            counter++;
                        }
                    }
                }

            }
            //Load Address fields.
            InitLicenseAddressFields();
            LoadLicenseAddress(obj.address_1, obj.address_2, obj.city, obj.state, obj.zip, obj.county);

        }
        catch (Exception)
        {

        }
    }

    private void LoadLicenseAddress(string add1, string add2, string city, string state, string zip, string county)
    {
        ucLicenseAddress.StreetAddress = add1;
        ucLicenseAddress.UnitAddress = add2;
        ucLicenseAddress.City = city;
        ucLicenseAddress.State = state;
        ucLicenseAddress.Zip5 = string.IsNullOrEmpty(zip) ? string.Empty : zip.Length > 5 ? zip.Substring(0, 5) : zip;
        ucLicenseAddress.LoadCountiesByState(state);
        if (!string.IsNullOrEmpty(state))
        {

            ucLicenseAddress.County = county;
        }
    }

    private void InitFormFields()
    {
        hidID.Text = string.Empty;
        prov_Number.Text = string.Empty;
        ddlBoardName.SelectedIndex = 0;
        ddlLicenseState.SelectedIndex = 0;
        ddlLicenseStatus.SelectedIndex = 0;
        prov_Effective.Text = string.Empty;
        prov_End.Text = string.Empty;
        txtBoard.Text = string.Empty;
        DisplayEverified(false);
        prov_Number.BackColor = prov_Effective.BackColor = txtBoard.BackColor = ddlBoardName.BackColor = prov_End.BackColor =
          ddlLicenseStatus.BackColor = ddlLicenseState.BackColor = System.Drawing.Color.White;
        ucSep1.Visible = true;
        LoadPlaceHolder(0, false, false, "License");
        plcHolderFocus1.Controls.Clear();
        plcHolderFocus1.Controls.Add(ucSpecialtyFocus);
        ucSpecialtyFocus.LoadValues(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        ucSpecialtyFocus.SetReadonly(false);
        InitLicenseAddressFields();
    }
    private void InitLicenseAddressFields()
    {
        ucLicenseAddress.Cell1Visible = false;
        ucLicenseAddress.Cell2Visible = false;
        ucLicenseAddress.Email1Visible = false;
        ucLicenseAddress.Email2Visible = false;
        ucLicenseAddress.Fax1Visible = false;
        ucLicenseAddress.Fax2Visible = false;
        ucLicenseAddress.OfficeMgrVisible = false;
        ucLicenseAddress.OrgNameVisible = false;
        ucLicenseAddress.AddressTypeVisible = false;
        ucLicenseAddress.NameSectionVisible = false;
        ucLicenseAddress.ContactVisible = false;
        ucLicenseAddress.StateVisible = false;
    }

    public bool IsLicenseUpdateAllowed(bool isConverted)
    {
        //Disable Effective Date and License Number fields if the provider is converted
        //and has a Medicaid ID. Also disable if Reg To Live has happened at least once.       
        bool isLicenseUpdateAllowed = true;
        DataSet ds = svc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        if (Helper.HasRows(ds))
        {
            if (isConverted)
            {
                if (!string.IsNullOrEmpty(Helper.GetString("MedicaidID", ds.Tables[0].Rows[0])))
                {
                    isLicenseUpdateAllowed = false;
                }
            }
        }
        return isLicenseUpdateAllowed;
    }
    public void GetRegProfessionalLicenses(DataSet ds)
    {
        throw new NotImplementedException();
    }

    protected void grdLicenses_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btn = (ImageButton)e.Row.FindControl("btnDelete");
                string eVerified = DataBinder.Eval(e.Row.DataItem, "ELICENSE_VERIFIED").ToString();
                bool bVerified = (string.IsNullOrEmpty(eVerified)) ? false : (eVerified == "1" || eVerified.ToLower() == "true") ? true : false;
                if (btn != null)
                {
                    btn.Style["display"] = CanUserViewDelete(bVerified) ? "block" : "none";
                }
            }
        }
        catch (Exception)
        {

        }
    }

    protected string FormatAddress(object regAddressId)
    {
        string address = string.Empty;
        try
        {
            if (regAddressId != null && Convert.ToInt32(regAddressId) > 0)
            {
                DataRow drAddress = presenter.GetLicenseAddress(this.WorkflowPage.RegistrationId, Convert.ToInt32(regAddressId));
                address = Helper.GetFormattedAddress(Helper.GetString("ADDRESS1", drAddress), Helper.GetString("ADDRESS2", drAddress), Helper.GetString("CITY", drAddress), Helper.GetString("STATE", drAddress), Helper.GetString("ZIP", drAddress), string.Empty,string.Empty);
                address = string.IsNullOrEmpty(Helper.GetString("COUNTYNAME", drAddress)) ? address : address + "<BR/>" + Helper.GetString("COUNTYNAME", drAddress);
                if (address.Equals("<br/> ,"))
                {
                    address = string.Empty;
                }
            }
        }
        catch (Exception)
        {

        }
        return address;
    }
    protected string FormatSpecialtyFocus(object regLicensureId)
    {
        string specialtyFocus = string.Empty;
        try
        {
            if (regLicensureId != null && Convert.ToInt32(regLicensureId) > 0)
            {
                DataTable dtSpecialty = presenter.GetLicenseSpecialtyFocusInformation(Convert.ToInt32(regLicensureId)).Tables[0];

                specialtyFocus = Helper.GetFomattedLicenseSpecialtyFocusInfo(dtSpecialty);
            }
        }
        catch (Exception)
        {

        }
        return specialtyFocus;
    }
    public override bool HasInputValue()
    {
        bool rtn = false;
        if (ddlLicenseState.SelectedItem != null || ddlBoardName.SelectedItem != null || !string.IsNullOrEmpty(prov_Number.Text) ||
            !string.IsNullOrEmpty(prov_Effective.Text) || !string.IsNullOrEmpty(prov_End.Text) || !string.IsNullOrEmpty(ucLicenseAddress.StreetAddress) ||
            !string.IsNullOrEmpty(ucLicenseAddress.City) || !string.IsNullOrEmpty(ucLicenseAddress.Zip5))
        {

            rtn = true;


        }
        else
        {
            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.Licenses)
                                .Select(d => d.Value.IsRequired).Max();
            if (required == 0)
            {
                rtn = true;
            }
            else
            {
                rtn = false;
            }
        }

        return rtn;

    }
}