using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Medicare : BaseSectionControl
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

    private const string sectionName = "Medicare";

    private const string STATUS_COMPLETED = "Completed";

    private int RegAlternateID
    {
        get
        {
            return ViewState["RegAlternateID"] == null ? 0 : Convert.ToInt32(ViewState["RegAlternateID"]);
        }
        set
        {
            ViewState["RegAlternateID"] = value;
        }
    }
    private int RegMedicareID
    {
        get
        {
            return ViewState["RegMedicareID"] == null ? 0 : Convert.ToInt32(ViewState["RegMedicareID"]);
        }
        set
        {
            ViewState["RegMedicareID"] = value;
        }
    }

    public override bool SaveData()
    {
        // OHPNM-2229
        // if this page isn't required and they haven't selected anything in the ddlEnrollmentStatus drop down yet, so don't worry about validating and just let them continue
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0 && ddlEnrollmentStatus.SelectedIndex == 0)
        {
            return true;
        }

        if (upMedicare.Visible)
        {
            if (string.IsNullOrEmpty(this.txtMedicareNumber.Text))
            {
                lbl_ValidationMedicare.Text = "* Please enter Medicare Number";
                return false;
            }
            if (this.ddlProvState.SelectedIndex == 0)
            {
                lbl_ValidationMedicare.Text = "* Please select state";
                return false;
            }
            lbl_ValidationMedicare.Text = "";
            //OHPNM-3833 - If page is required, medicare number section is mandatory. If not filled do not save the page.
            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdMedicares.Rows.Count == 0 && (ddlEnrollmentStatus.SelectedIndex == 0 || ddlEnrollmentStatus.SelectedIndex == -1))
            {
                return false;
            }
            if (ddlEnrollmentStatus.SelectedIndex != -1)
            {
                Page.Validate("MedicareComplete");
                for (int i = 0; i < Page.Validators.Count; i++)
                {
                    BaseValidator v;
                    try
                    {
                        v = Page.Validators[i] as BaseValidator;
                        if (v != null && v.ValidationGroup.Equals("MedicareComplete") && !v.IsValid)
                            return false;
                    }
                    catch
                    {
                        continue;
                    }
                }
                if (string.IsNullOrEmpty(txtEnrollmentDate.Text))
                {
                    if (PlaceholderUploadMedicare.Controls.Count == 0)
                    {
                        LoadPlaceHolder(string.IsNullOrEmpty(hidID.Text) ? 0 : int.Parse(this.hidID.Text), string.IsNullOrEmpty(hidIsEdit.Text) ? false : Boolean.Parse(this.hidIsEdit.Text), true, sectionName);
                        return false;
                    }
                }

                if (ddlEnrollmentStatus.SelectedItem.Text.ToLower() == "in process")
                {
                    foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadMedicare.Controls)
                    {
                        if (uploadDoc.DocumentId == 0)
                        {
                            uploadDoc.IsRequired = true;
                            return uploadDoc.ValidateData("MedicareComplete");
                        }
                    }
                }               

                string status = string.Empty;
                if (this.ddlEnrollmentStatus != null && this.ddlEnrollmentStatus.SelectedItem != null)
                {
                    status = this.ddlEnrollmentStatus.SelectedItem.Text;
                }

                Dictionary<string, string> parms = new Dictionary<string, string>();
                Dictionary<string, string> parmsAlt = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parmsAlt.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("ENROLLMENT_STATUS_TYPE_ID", this.ddlEnrollmentStatus.SelectedValue);
                parms.Add("MEDICARE_NUMBER_TYPE", this.rblMedicareNumberType.SelectedValue);

                int medicareTypeID = this.rblMedicareNumberType.SelectedValue == "PTAN" ? CON.MedicareType.PTAN : CON.MedicareType.CCN;
                parms.Add("MEDICARE_TYPE_ID", medicareTypeID.ToString());

                if (!string.IsNullOrEmpty(this.txtMedicareNumber.Text))
                    parms.Add("MEDICARE_NUMBER", this.txtMedicareNumber.Text);

                if (!string.IsNullOrEmpty(this.ddlProvState.SelectedValue))
                    parms.Add("MEDICARE_STATE", ddlProvState.SelectedValue);

                //OHPNM - 15523 - Fix to Save Secondary NPI even when it is not found in PECOS file.
                if (!string.IsNullOrEmpty(this.txtNPI.Text))
                    parmsAlt.Add("ALTERNATE_ID", this.txtNPI.Text);
            

                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parmsAlt.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parmsAlt.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                int medicareId = 0;
                int reg_alternate_id_id = 0;

                bool isEdit = RegMedicareID > 0;
                if (isEdit)
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_MEDICARE_ID", RegMedicareID.ToString());
                    svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "MEDICARE", parms); //there is also a MEDICARECustom proc, but is not needed here.
                    medicareId = int.Parse(RegMedicareID.ToString());
                    if (RegAlternateID == 0 && !string.IsNullOrEmpty(this.txtNPI.Text))
                    {
                        parmsAlt.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parmsAlt.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        reg_alternate_id_id = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "ALTERNATE_ID", parmsAlt);

                        //insert into reg_npi_medicare
                        Dictionary<string, string> parmsnpi = new Dictionary<string, string>();
                        parmsnpi.Add("REG_MEDICARE_ID", medicareId.ToString());
                        parmsnpi.Add("reg_alternate_id_id", reg_alternate_id_id.ToString());
                        parmsnpi.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parmsnpi.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parmsnpi.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parmsnpi.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        svc.InsertRegistrationDataTable("NPI_MEDICARE", parmsnpi);
                    }
                    else if (!string.IsNullOrEmpty(this.txtNPI.Text))
                    {
                        parmsAlt.Add("reg_alternate_id_id", RegAlternateID.ToString());
                        reg_alternate_id_id = svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "ALTERNATE_ID", parmsAlt);
                    }

                    if (!string.IsNullOrEmpty(this.txtNPI.Text))
                    {
                        //update Enrollment status to Completed & Oldest Medicare Effective date
                        Dictionary<string, string> parmsmedicare = new Dictionary<string, string>();
                        parmsmedicare.Add("MEDICARE_STATE", ddlProvState.SelectedValue);
                        parmsmedicare.Add("NPI", this.txtNPI.Text);
                        parmsmedicare.Add("REG_MEDICARE_ID", medicareId.ToString());
                        parmsmedicare.Add("MEDICARE_NUMBER", this.txtMedicareNumber.Text);
                        parmsmedicare.Add("ALTERNATE_ID_TYPE_ID", CON.Prov_ID_TYPE.NPI2.ToString());
                        parmsmedicare.Add("REGID", this.WorkflowPage.RegistrationId.ToString());
                        svc.UpdateRegMedicareEnrollmentStatus(parmsmedicare);
                    }
                }
                else
                {
                    parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Inserted.ToString());
                    medicareId = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "MEDICARE", parms); //there is also a MEDICARECustom proc, but is not needed here.

                    if (!string.IsNullOrEmpty(this.txtNPI.Text))
                    {
                        parmsAlt.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parmsAlt.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        reg_alternate_id_id = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "ALTERNATE_ID", parmsAlt);

                        //insert into reg_npi_medicare
                        Dictionary<string, string> parmsnpi = new Dictionary<string, string>();
                        parmsnpi.Add("REG_MEDICARE_ID", medicareId.ToString());
                        parmsnpi.Add("reg_alternate_id_id", reg_alternate_id_id.ToString());
                        parmsnpi.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                        parmsnpi.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        parmsnpi.Add("Created_On_Date_Time", DateTime.Now.ToString());
                        parmsnpi.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                        svc.InsertRegistrationDataTable("NPI_MEDICARE", parmsnpi);

                        //update Enrollment status to Completed & Oldest Medicare Effective date
                        Dictionary<string, string> parmsmedicare = new Dictionary<string, string>();
                        parmsmedicare.Add("MEDICARE_STATE", ddlProvState.SelectedValue);
                        parmsmedicare.Add("NPI", this.txtNPI.Text);
                        parmsmedicare.Add("REG_MEDICARE_ID", medicareId.ToString());
                        parmsmedicare.Add("MEDICARE_NUMBER", this.txtMedicareNumber.Text);
                        parmsmedicare.Add("ALTERNATE_ID_TYPE_ID", CON.Prov_ID_TYPE.NPI2.ToString());
                        parmsmedicare.Add("REGID", this.WorkflowPage.RegistrationId.ToString());
                        svc.UpdateRegMedicareEnrollmentStatus(parmsmedicare);

                       
                    }
                }

                foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadMedicare.Controls)
                {
                    svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, medicareId);
                }
            }
        }
        else
        {

            if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 1 && grdMedicares.Rows.Count == 0)
            {
                return false;
            }

            return true;
        }
        return true;
    }

    private void LoadMedicare()
    {
        Helper.LoadDropDownListWithStates(ref ddlProvState);
        //PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MEDICARE");
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtMisc;
        txtEnrollmentDate.Enabled = false;
        txtEnrollmentDate.CssClass = "formFieldReadOnly";

        if (Helper.HasRows(ds)) grdMedicares.DataSource = this.DataList = ds.Tables[0];
        else grdMedicares.DataSource = this.DataList = null;
        grdMedicares.DataBind();

        btnAddMedicare.Visible = true;
    }

    private void LoadData(DataRow dr, bool isEdit)
    {
        int medicareId = isEdit ? Helper.GetInt("REG_MEDICARE_ID", dr) : 0;
        LoadPlaceHolder(medicareId, isEdit);
        LoadDropDowns();
        //load from reg_alternate
        hidIsEdit.Text = isEdit.ToString();
        InitFormData();

        if (isEdit)
        {
            RegMedicareID = Helper.GetInt("REG_MEDICARE_ID", dr);
            RegAlternateID = Helper.GetInt("reg_alternate_id_id", dr);
            hidID.Text = RegMedicareID.ToString();

            if (RegMedicareID > 0)
            {
                this.txtMedicareNumber.Text = Helper.GetString("MEDICARE_NUMBER", dr);
                this.txtEnrollmentDate.Text = Helper.FormatDate2(dr["MEDICARE_EFF_DATE"].ToString());
                this.txtNPI.Text = !string.IsNullOrWhiteSpace(Helper.GetString("NPI", dr)) ? Helper.GetString("NPI", dr) : this.txtNPI.Text;
                this.ddlEnrollmentStatus.SelectedValue = Helper.GetInt("ENROLLMENT_STATUS_TYPE_ID", dr).ToString();
                string medicareNumberType = Helper.GetString("MEDICARE_NUMBER_TYPE", dr);

                if (!string.IsNullOrWhiteSpace(medicareNumberType))
                {
                    this.rblMedicareNumberType.SelectedValue = medicareNumberType;
                }
                this.ddlProvState.SelectedValue = Helper.GetString("MEDICARE_STATE", dr).ToString();
                this.lblCurrentEnrollmentStatus.Text = this.ddlEnrollmentStatus.SelectedValue;
                //core table does not have npi
            }

            ShowCoreValues(HttpContext.Current.User.IsInRole("Administrator") && isEdit);
        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    private void ShowCoreValues(bool isVisible)
    {
        this.lblCurrentEnrollmentStatus.Visible = isVisible;
        this.lblCurrentMedicareNumber.Visible = isVisible;
        this.lblCurrentEnrollmentDate.Visible = isVisible;
    }

    private DataSet GetMedicareData()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_MEDICARE_ID", RegMedicareID.ToString());
        return svc.SelectRegistrationDataWithParams("usp_SelectREG_MEDICAREByID", parms);
    }

    private void LoadDropDowns()
    {
        DataSet ds = svc.GetMedicareEnrollmentStatusType();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlEnrollmentStatus, ds.Tables[0], "MEDICARE_ENROLLMENT_STATUS_TYPE_NAME", "MEDICARE_ENROLLMENT_STATUS_TYPE_ID", false);
            this.ddlEnrollmentStatus.Items.Insert(0, new ListItem(string.Empty, "0"));
        }

    }

    private void InitFormData()
    {
        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);

        this.txtMedicareNumber.Text = string.Empty;
        this.txtEnrollmentDate.Text = string.Empty;
        //this.txtNPI.Text = string.Empty;
        this.ddlEnrollmentStatus.SelectedIndex = 0;

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            this.ddlEnrollmentStatus.Enabled =
            this.txtMedicareNumber.Enabled =
            this.ddlProvState.Enabled =
            this.txtNPI.Enabled = true;
        }
        else
        {
            //this.ddlEnrollmentStatus.Enabled =
            this.txtEnrollmentDate.Enabled =
            this.txtMedicareNumber.Enabled =
            this.ddlProvState.Enabled =
            this.txtNPI.Enabled = false;
        }

    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Miscellaneous)
        {
            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
            int medicareId = 0;
            LoadPlaceHolder(medicareId, isEdit, false, sectionName);

        }

        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddMedicare.Visible = false;
            }
        }

        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
    }

    public void LoadPlaceHolder(int medicareId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();

        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, medicareId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, medicareId, pageSection, isEdit);


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

                ucUploadSectionControl.SectionName = sectionName;
                ucUploadSectionControl.RowId = medicareId;

                ucUploadSectionControl.IsRequired = true;

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (PlaceholderUploadMedicare.FindControl(ucUploadSectionControl.ID) == null)
                {
                    PlaceholderUploadMedicare.Controls.Add(ucUploadSectionControl);
                    upMedicare.Update();
                }
            }
        }

    }
    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (upMedicare.Visible)
        {
            if (ddlEnrollmentStatus.SelectedItem != null)// && ddlEnrollmentStatus.SelectedItem.Text == "Completed")
            {
                isRequired = true;
                vsMedicareSummary.ValidationGroup = "MedicareComplete";
            }
        }
        else
        {
            //OHPNM-3209 -- If "Add New" is not clicked, there are no fields on the screen to check if they are filled out.
            isRequired = true;
        }
        return isRequired;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Medicare Number";
    }
    public override void LoadControlData()
    {
        LoadMedicare();

    }

    public override void LoadData(DataRow dr = null)
    {
        // TODO: EDV what should I send for isEdit
        LoadData(dr, false);
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string Title
    {
        get { return "Medicare"; }
    }

    public override string IdText
    {
        get { return "ucMedicare_" + this.WorkflowPage.RegistrationId; }
    }

    public override string ValidationGroup
    {
        get { return "valMedicare"; }
    }

    protected void txtNPI_TextChanged(object sender, EventArgs e)
    {
        AssignEnrollmentDate();
    }

    private void AssignEnrollmentDate()
    {
        lblCurrentEnrollmentDate.Text = "";
        lblCurrentEnrollmentDate.Visible = false;

        if (!string.IsNullOrEmpty(txtNPI.Text) && !string.IsNullOrEmpty(ddlProvState.SelectedValue))
        {
            string enrollmentDate = svc.GetEnrollmentdateforNPI(txtNPI.Text, ddlProvState.SelectedValue);
            txtEnrollmentDate.Text = !string.IsNullOrEmpty(enrollmentDate) ? (Convert.ToDateTime(enrollmentDate).ToString("MM/dd/yyyy") != "01/01/1900" ? Convert.ToDateTime(enrollmentDate).ToString("MM/dd/yyyy") : "") : "";
            if (txtEnrollmentDate.Text == "")
            {
                lblCurrentEnrollmentDate.Text = "*No record found and require upload";
                lblCurrentEnrollmentDate.Visible = true;
                lblCurrentEnrollmentDate.CssClass = "failureNotification";
            }
            else
            {
                ddlEnrollmentStatus.SelectedIndex = 1;
            }
        }
    }

    protected void ddlProvState_SelectedIndexChanged(object sender, EventArgs e)
    {
        AssignEnrollmentDate();
    }
    public bool CanUserViewDelete(int index)
    {
        DataTable dt = this.DataList;
        int modifiedStatusTypeId = dt.Rows[index]["MODIFIED_STATUS_TYPE_ID"] == DBNull.Value ? 0 : Convert.ToInt32(dt.Rows[index]["MODIFIED_STATUS_TYPE_ID"]);

        bool result = Registration.CanUserViewDelete(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName);

        if(result == true && (modifiedStatusTypeId == 0 || modifiedStatusTypeId == 4))
        {
            return true;
        }
        return false;
    }

    protected void grdMedicares_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        upMedicare.Visible = true;
        DataRow dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteMedicareRow")
        {
            if (dr != null)
            {
                int REG_MEDICARE_ID = Helper.GetInt("REG_MEDICARE_ID", dr);
                if (REG_MEDICARE_ID > 0)
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    psc.DeleteRegistrationData("MEDICARE", "REG_MEDICARE_ID", REG_MEDICARE_ID);
                    LoadMedicare();
                }
            }
        }
        else if (e.CommandName == "EditMedicareRow")
        {
            this.hidIsEdit.Text = "true";
            if (Helper.HasRows(this.DataList))
                this.LoadData(dr, true);
            else
                this.LoadData(null);
        }
        ddlEnrollmentStatus.Enabled = false;
    }

    protected void btnAddMedicare_Command(object sender, CommandEventArgs e)
    {
        upMedicare.Visible = true;
        this.hidID.Text = string.Empty;
        this.hidIsEdit.Text = string.Empty;
        this.LoadData(null);
        ddlEnrollmentStatus.Enabled = false;
        if (ddlEnrollmentStatus.Items.Count > 2)
            ddlEnrollmentStatus.SelectedIndex = 2;
    }
}