using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Medicaid : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
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

    private const string sectionName = "Medicaid";

    public override bool SaveData()
    {
        if (medicaidDetail.Visible)
        {
            if (!ValidateLC105())
                return false;

            Page.Validate("Medicaid");
            if ((prov_Medicaid_Enrollment_Status.SelectedItem != null && prov_Medicaid_Enrollment_Status.SelectedItem.Text == "Completed") && string.IsNullOrEmpty(prov_Date_Enrolled.Text) && string.IsNullOrEmpty(prov_State.Text) && string.IsNullOrEmpty(prov_NPI.Text))
            {
                bool isGood = true;
                AddError("*Please Enter Date Enrolled, State, NPI", ref isGood);
            }
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("Medicaid") && !v.IsValid)
                        return false;

                }
                catch
                {
                    continue;
                }
            }

            bool isValid = true;
            foreach (Control ctrl in PlaceholderUploadMedicaid.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isValid &= uploadControl.ValidateData("Medicaid");
            }

            if (!isValid)
                return isValid;

            if (prov_State.SelectedValue != string.Empty)
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
                parms.Add("MEDICAID_NUMBER", txtOtherStateMedicaidNumber.Text);
                parms.Add("MEDICAID_EFF_DATE", prov_Date_Enrolled.Text);
                //parms.Add("MEDICAID_END_DATE", prov_End.Text);
                parms.Add("MEDICAID_STATE", prov_State.SelectedValue);
                parms.Add("ENROLLMENT_STATUS_TYPE_ID", this.prov_Medicaid_Enrollment_Status.SelectedValue);
                parms.Add("NPI", prov_NPI.Text);
                parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("IS_DIFFENTBILLWITHNPI", rbldiffbillfornpi.SelectedValue.ToString());
                int medicaidId = 0;
                bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
                if (isEdit)
                {
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
                    parms.Add("REG_MEDICAID_ID", hidID.Text);
                    svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAIDcustom", parms);
                    medicaidId = int.Parse(hidID.Text); // we are only getting reg id back from UpdateRegistrationData
                }
                else
                {
                    parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.NoChange.ToString());
                    medicaidId = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAIDcustom", parms);
                }

                foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadMedicaid.Controls)
                {
                    svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, medicaidId);
                }
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

        int medicaidId = isEdit ? Helper.GetInt("REG_MEDICAID_ID", row) : 0;
        LoadPlaceHolder(medicaidId, isEdit);
        Helper.LoadDropDownListWithStates(ref prov_State);
        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        LoadDropDowns();

        if (Registration.CanUserEditRegistration(this.WorkflowPage.RegistrationIdSelected, this.WorkflowPage.CurrentTaskName))
        {
            txtOtherStateMedicaidNumber.Enabled =
            prov_NPI.Enabled =
            prov_State.Enabled =
            prov_Date_Enrolled.Enabled = true;
        }
        else
        {
            txtOtherStateMedicaidNumber.Enabled =
            prov_NPI.Enabled =
            prov_State.Enabled =
            prov_Date_Enrolled.Enabled = false;
        }

        //ParentTable.Rows[0].Cells[1].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Text = isEdit.ToString();

        if (isEdit)
        {
            hidID.Text = row["REG_MEDICAID_ID"].ToString();
            txtOtherStateMedicaidNumber.Text = row["MEDICAID_NUMBER"].ToString();
            prov_State.SelectedValue = row["MEDICAID_STATE"].ToString();
            prov_NPI.Text = row["NPI"].ToString();
            prov_Date_Enrolled.Text = Helper.FormatDate2(row["MEDICAID_EFF_DATE"].ToString());
            rbldiffbillfornpi.SelectedValue = Helper.GetString("IS_DIFFENTBILLWITHNPI", row);
            //prov_End.Text =  Helper.FormatDate2(row["MEDICAID_END_DATE"].ToString());
            //pdms_Description.Text = Helper.GetString("PDMS_MEDICAIDNumber", row);
            //pdms_State.Text = GetStateName(Helper.GetString("PDMS_MEDICAIDState", row));
            //pdms_Start.Text = Helper.FormatDate2(Helper.GetString("PDMS_MEDICAIDEffDate", row));
            //pdms_End.Text = Helper.FormatDate2(Helper.GetString("PDMS_MEDICAIDEndDate", row));
            if (Helper.GetInt("ENROLLMENT_STATUS_TYPE_ID", row) > 0)
                this.prov_Medicaid_Enrollment_Status.SelectedValue = Helper.GetInt("ENROLLMENT_STATUS_TYPE_ID", row).ToString();
            lblEnrollment_Status.Visible = prov_Medicaid_Enrollment_Status.Visible = true;

            if (string.IsNullOrWhiteSpace(prov_NPI.Text)
                || string.IsNullOrWhiteSpace(txtOtherStateMedicaidNumber.Text)
                || string.IsNullOrWhiteSpace(prov_Date_Enrolled.Text))
            {
                prov_Medicaid_Enrollment_Status.SelectedValue = CON.MedicaidEnrollmentStatusID.InProcess;
                prov_Medicaid_Enrollment_Status.Enabled = true;
                //lblInProgress.Visible = true;
            }
            else
            {
                //prov_Medicaid_Enrollment_Status.SelectedValue = CON.MedicaidEnrollmentStatus.Completed;
                prov_Medicaid_Enrollment_Status.Enabled = false;
                //lblInProgress.Visible = false;
            }
        }
        else
        {
            hidID.Text = string.Empty;
            prov_NPI.Text = string.Empty;
            prov_State.SelectedIndex = 0;
            prov_Date_Enrolled.Text = string.Empty;
            txtOtherStateMedicaidNumber.Text = string.Empty;
            // lblEnrollment_Status.Visible = prov_Medicaid_Enrollment_Status.Visible = prov_Medicaid_Enrollment_Status.Enabled = true;
            prov_Medicaid_Enrollment_Status.SelectedIndex = 0;
            divMedicaidInfo.Visible = false;
            ////prov_End.Text = string.Empty;
        }
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        hdnRegId.Value = this.WorkflowPage.RegistrationId.ToString();
    }
    public override void LoadControlData()
    {
        LoadMedicaid();
    }

    private void LoadMedicaid()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "MEDICAID");
        DataTable dtMedicaid = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtMedicaid;
        grdMedicaid.DataSource = dtMedicaid;
        grdMedicaid.DataBind();
        //btnHistory.Visible = false;
        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
            Helper.SetReadOnly(btnAddMedicaid, true);
        }
        //OHPNM-3487 - on click of View provider file Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
                btnAddMedicaid.Visible = false;
            }
        }
    }

    public override bool ValidateData()
    {
        return true;
    }

    protected bool ValidateLC105()
    {
        bool isValid = true;
        if (string.IsNullOrEmpty(prov_NPI.Text))
        {
            isValid = true;
        }
        else
        {
            if (string.IsNullOrEmpty(prov_State.SelectedValue))
                AddError("* State is required.", ref isValid);
        }
        return isValid;
    }

    /*protected void ValidateLC106_Future(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(prov_Start.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = Convert.ToDateTime(prov_Start.Text) <= DateTime.Now;
        }
    }

    protected void ValidateLC106_Required(object sender, ServerValidateEventArgs e)
    {
        if (string.IsNullOrEmpty(prov_Number.Text))
        {
            e.IsValid = true;
        }
        else
        {
            e.IsValid = !string.IsNullOrEmpty(prov_Start.Text);
        }
    }

    protected void ValidateLC_Less(object sender, ServerValidateEventArgs e)
    {
        DateTime start;
        DateTime end;

        start = DateTime.TryParse(prov_Start.Text, out start) ? start : DateTime.MinValue;
        end = DateTime.TryParse(prov_End.Text, out end) ? end : DateTime.MaxValue;

        if (start > end)
        {
            e.IsValid = false;
            return;
        }
    }*/

    protected void prov_NPI_TextChanged(object sender, EventArgs e)
    {
        /*if (System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z0-9]$", prov_Number.Text))
        {
            prov_NPI.Text.Remove(prov_NPI.Text.Length - 1);
        }*/
    }

    private string GetStateName(string id)
    {
        int i;
        if (!int.TryParse(id, out i))
        {
            return string.Empty;
        }

        DataTable dt = ApplicationCache.StateAbbreviations().Select("StateId = " + id).CopyToDataTable();

        if (Helper.HasRows(dt))
        {
            return dt.Rows[0]["StateName"].ToString();
        }
        else
        {
            return string.Empty;
        }
    }



    protected void prov_Medicaid_Enrollment_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (prov_Medicaid_Enrollment_Status.SelectedItem.Text == "In Process")
        {
            lblState.Visible = prov_State.Visible = true;
            divMedicaidInfo.Visible = false;
        }
        else
        {
            lblState.Visible = prov_State.Visible = true;
            divMedicaidInfo.Visible = true;
        }
    }
    public void LoadPlaceHolder(int medicaidId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadMedicaid.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, medicaidId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, medicaidId, pageSection, isEdit);

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

                if ((dr.Table.Columns.Contains("FILE_NAME")))
                    ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                else
                    ucUploadSectionControl.FileName = null;

                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                PlaceholderUploadMedicaid.Controls.Add(ucUploadSectionControl);
            }
        }

    }
    private void LoadDropDowns()
    {
        DataSet ds = svc.GetMedicaidEnrollmentStatusType();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.prov_Medicaid_Enrollment_Status, ds.Tables[0], "MEDICAID_ENROLLMENT_STATUS_TYPE_NAME", "MEDICAID_ENROLLMENT_STATUS_TYPE_ID", false);
            this.prov_Medicaid_Enrollment_Status.Items.Insert(0, new ListItem(string.Empty, "0"));
        }

    }

    private void AddError(string errMsg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = errMsg;
        val.ValidationGroup = "Medicaid";
        this.Page.Validators.Add(val);
        isGood = false;
    }
    public override bool HasInputValue()
    {
        bool isRequired = false;
        if (medicaidDetail.Visible)
        {
            if (prov_Medicaid_Enrollment_Status.SelectedItem != null) // && prov_Medicaid_Enrollment_Status.SelectedItem.Text == "Completed")
            {
                isRequired = true;
            }
        }
        else
        {
            //OHPNM-3209 -- If "Add New" is not clicked, there are no fields on the screen to check if they are filled out.
            isRequired = true;
        }
        return isRequired;
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);

        medicaidDetail.Visible = true;
        if (Helper.HasRows(this.DataList))
            this.LoadData(this.DataList.Rows[index]);
        else
            this.LoadData(null);
    }

    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        medicaidDetail.Visible = true;
        this.LoadData(null);
    }

    public override string ValidationGroup
    {
        get { return "Medicaid"; }
    }

    public override string Title
    {
        get { return "Miscellaneous"; }
    }

    public override string IdText
    {
        get { return "ucMedicaid_" + this.WorkflowPage.RegistrationId; }
    }


    protected void prov_State_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}