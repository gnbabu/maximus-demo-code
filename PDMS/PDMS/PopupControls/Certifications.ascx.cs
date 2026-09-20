using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Certifications : BaseSectionControl
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

    private const string sectionName = "DEA";
    private DataTable dt;
    private void SetDt()
    {
        if (dt == null)
        {
            DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
            dt = Helper.HasRows(ds) ? ds.Tables[0] : null;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.WorkflowPage.RegistrationStep == CON.SectionTypeID.FederalDEA)
        {
            bool isEdit = string.IsNullOrEmpty(hidIsEdit.Value) ? false : Convert.ToBoolean(hidIsEdit.Value);
            int deaId = 0;
            if (isEdit)
                deaId = int.Parse(hidID.Text);

            LoadPlaceHolder(deaId, isEdit, false);
            if (IsPostBack)
            {
                if (this.WorkflowPage.IsSaved && !isEdit)
                {
                    //rblCurrentDEARegistration.SelectedValue = "Y";
                    rblCurrentDEARegistration_SelectedIndexChanged(null, null);
                }
            }

            var Nodes = this.WorkflowPage.RegistrationNodes;
            int required = 0;
            if (Nodes.Count > 0)
            {
                required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.FederalDEA)
                                .Select(d => d.Value.IsRequired).Max();
            }

            rfvCurrentDEARegistration.Visible = false;
            if (required == 1 && grdCertification.Rows.Count == 0)
            {
                rfvCurrentDEARegistration.Visible = true;
            }
            else
            {
                rfvCurrentDEARegistration.Visible = false;
            }

        }

        //OHPNM-3487 - on click of View provider file read only/Add new button should be disabled.
        if (Session["ViewProviderFile"] != null)
        {
            if (Convert.ToBoolean(Session["ViewProviderFile"]))
            {
                Helper.SetReadOnly(this, true, "formFieldReadOnly");
            }
        }

        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
    }

    protected void lnkExcel_Click(object sender, EventArgs e)
    {
        _ExportHistory = true;
        LoadControlData();
    }

    private bool ValidateFederalDEA(string federalDEANumber)
    {
        bool isGood = true;
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("federal_DEALicenseID", federalDEANumber);
        if (string.IsNullOrEmpty(hidID.Text))
            hidID.Text = "0";
        parms.Add("reg_dea_id", hidID.Text);
        DataSet ds = psc.SelectRegistrationDataWithParams("usp_SelectFederalDEALicenseIDByLicenseIDRegID", parms);
        if (ds != null && Helper.HasRows(ds))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* DEA License number already added.";
            val.ValidationGroup = "Certifications";
            this.Page.Validators.Add(val);
            isGood = false;
        }
        return isGood;
    }

    public override bool HasInputValue()
    {
        var Nodes = this.WorkflowPage.RegistrationNodes;
        int required = 0;
        if (Nodes.Count > 0)
        {
            required = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.FederalDEA)
                                .Select(d => d.Value.IsRequired).Max();
        }

        bool isRequired = false;
        if ((!string.IsNullOrEmpty(prov_Number.Text) || !string.IsNullOrEmpty(prov_State.SelectedValue) || !string.IsNullOrEmpty(prov_Start.Text) || !string.IsNullOrEmpty(prov_End.Text) ||
            !string.IsNullOrEmpty(txtPrescribeProvider.Text) || !string.IsNullOrEmpty(txtPrescribeProviderDEA.Text) || !string.IsNullOrEmpty(ddlPrecribeProviderState.SelectedValue) || !string.IsNullOrEmpty(txtPrecibeComments.Text) && required == 1) || required == 0)
        {
            isRequired = true;
        }

        return isRequired;
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override bool SaveData()
    {
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Value) ? false : Convert.ToBoolean(hidIsEdit.Value);

        bool isGood = !isEdit ? ValidateFederalDEA(prov_Number.Text) : isEdit;
        Page.Validate("Certifications");
        var node = this.WorkflowPage.RegistrationNodes.Where(x => x.Value.MenuPath == "Federal DEA Registration")
            .Select(x => new { Key = x.Key, Value = x.Value }).FirstOrDefault();
        bool isFederalDEARequired = false;
        if (node != null && node.Value.IsRequired == 1)
        {
            isFederalDEARequired = true;
        }
        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("Certifications") && !v.IsValid)
                    return false;
                if (Helper.HasRows(this.DataList) && isFederalDEARequired)
                {

                    string expression;
                    expression = string.Format("DEA_END_DATE > #{0}#", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt"));
                    DataRow[] foundRows;
                    foundRows = DataList.Select(expression);
                    if (foundRows.Length > 0)
                    {
                        // return true;
                    }
                }
            }
            catch
            {
                continue;
            }
        }

        bool isValid = true;

        foreach (Control ctrl in PlaceholderUploadDEA.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("Certifications");
        }

        if (!isValid)
            return isValid;

        //if (!ValidateExpirationDate())
        //    return isValid = false;
        //if (!ValidateFederalDEA(prov_Number.Text))
        //    return isValid = false;

        // OHPNM-2419 - don't save anything at all if there are no entry fields on the screen (they've clicked "Y" and they've clicked "Add" or are editing one OR they've clicked "N")
        //if (!divAddFederalDEA.Visible && !divDEAQuestionMapping.Visible)
        //{
        //    // There really isn't anything to save
        //    return false;
        //}


        Dictionary<string, string> parms = new Dictionary<string, string>();
        if ((rblCurrentDEARegistration.SelectedValue == "Y" || rblCurrentDEARegistration.SelectedValue == "N") && isEdit == true)
        {
            parms.Add("DEA_EFF_DATE", prov_Start.Text);
            parms.Add("DEA_END_DATE", (!string.IsNullOrEmpty(prov_End.Text) ? prov_End.Text : Convert.ToDateTime("12/31/2299").ToString("MM/dd/yyyy")));
            parms.Add("DEA_STATUS", ddldeastatus.SelectedValue);
        }
        else if (rblCurrentDEARegistration.SelectedValue == "Y" || rblCurrentDEARegistration.SelectedValue == "N")
        {
            parms.Add("DEA_EFF_DATE", prov_Start.Text);
            if (rblCurrentDEARegistration.SelectedValue == "Y")
                parms.Add("DEA_END_DATE", (!string.IsNullOrEmpty(prov_End.Text) ? prov_End.Text : Convert.ToDateTime("12/31/2299").ToString("MM/dd/yyyy")));
            else
                parms.Add("DEA_END_DATE", prov_End.Text);
            parms.Add("DEA_STATUS", ddldeastatus.SelectedValue);
        }
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        parms.Add("DEA_NUMBER", rblCurrentDEARegistration.SelectedValue == "Y" ? prov_Number.Text : txtPrescribeProviderDEA.Text);
        parms.Add("DEA_STATE", rblCurrentDEARegistration.SelectedValue == "Y" ? prov_State.SelectedValue : ddlPrecribeProviderState.SelectedValue);

        parms.Add("PRESCRIBE_PROVIDER_NAME", rblCurrentDEARegistration.SelectedValue == "Y" ? "" : txtPrescribeProvider.Text);
        parms.Add("PRESCRIBING_COMMENTS", rblCurrentDEARegistration.SelectedValue == "Y" ? "" : txtPrecibeComments.Text);

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
        parms.Add("DEA_VERIFIED", rblCurrentDEARegistration.SelectedValue == "Y" ? "1" : "0");
        int deaId = 0;

        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
        if (Helper.HasRows(ds))
        {
            if (rblCurrentDEARegistration.SelectedValue == "Y")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["DEA_NUMBER"].ToString() == prov_Number.Text.ToString() && Convert.ToDateTime(dr["DEA_EFF_DATE"]).ToString("MM/dd/yyyy") == Convert.ToDateTime(prov_Start.Text).ToString("MM/dd/yyyy") && isEdit == false)
                    {
                        lblErrorMsg.Text = "Duplicate DEA numbers within the same date span not allowed.";
                        AddNewCertificationItem();
                        return false;
                    }
                }
            }
            else if (rblCurrentDEARegistration.SelectedValue == "N")
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr["DEA_NUMBER"].ToString() == prov_Number.Text.ToString() && !DBNull.Value.Equals(dr["DEA_EFF_DATE"]))
                    {
                        if (Convert.ToDateTime(dr["DEA_EFF_DATE"]).ToString("MM/dd/yyyy") == Convert.ToDateTime(prov_Start.Text).ToString("MM/dd/yyyy"))
                        {
                            lblErrorMsg.Text = "Duplicate DEA numbers within the same date span not allowed.";
                            AddNewCertificationItem();
                            return false;
                        }
                    }
                }
            }
        }

        if (isEdit)
        {
            lblErrorMsg.Text = "";
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Changed.ToString());
            parms.Add("REG_DEA_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "DEA", parms);
            deaId = int.Parse(hidID.Text);
        }
        else
        {
            lblErrorMsg.Text = "";
            parms.Add("MODIFIED_STATUS_TYPE_ID", MAXIMUS.Core.Libraries.Constants.RegistrationModifiedStatusType.Inserted.ToString());
            if ((rblCurrentDEARegistration.SelectedValue == "Y" && divAddFederalDEA.Visible == true) || (rblCurrentDEARegistration.SelectedValue == "N" && divDEAQuestionMapping.Visible == true))
            {
                deaId = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "DEA", parms);
            }
        }

        foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadDEA.Controls)
        {
            svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, deaId);
        }
        this.WorkflowPage.IsSaved = true;
        return true;
    }

    private void LoadDEACertification()
    {
        txtPrecibeComments.Attributes.Add("maxlength", txtPrecibeComments.MaxLength.ToString());

        Helper.LoadDropDownListWithStates(ref prov_State);
        Helper.LoadDropDownListWithStates(ref ddlPrecribeProviderState);
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEA");
        DataTable dtDEACertification = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtDEACertification;


        if (Helper.HasRows(this.DataList))
        {
            //this.LoadData(this.DataList.Rows[0]);
            grdCertification.DataSource = this.DataList = ds.Tables[0];
        }
        else
        {
            // this.LoadData(null);
            grdCertification.DataSource = null;
        }
        grdCertification.DataBind();
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Federal DEA Regestration";
    }
    public override void LoadControlData()
    {
        LoadDEACertification();
        if (_ExportHistory)
        {
            _ExportHistory = false;
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "DEAHistory");
            if (Helper.HasRows(ds))
            {
                grdDEAHistory.DataSource = ds.Tables[0];
                grdDEAHistory.DataBind();
                grdDEAHistory.MasterTableView.ExportToExcel();
            }
        }

    }

    public override void LoadData(DataRow row)
    {
        bool isEdit = false;
        if (row == null)
            isEdit = false;
        else
            isEdit = true;

        this.WorkflowPage.IsSaved = false;
        int deaId = isEdit ? Helper.GetInt("REG_DEA_ID", row) : 0;
        LoadPlaceHolder(deaId, isEdit);


        bool isPending = Helper.RegistrationIsPending(this.WorkflowPage.RegistrationId);
        if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, MAXIMUS.Core.Libraries.Constants.UserRoleType.Administrator))
        {
            prov_Number.Enabled =
            prov_State.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = !isPending;
        }
        else if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
        {
            prov_Number.Enabled =
            prov_State.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = true;
        }
        else
        {
            prov_Number.Enabled =
            prov_State.Enabled =
            prov_Start.Enabled =
            prov_End.Enabled = false;
        }
        //ParentTable.Rows[0].Cells[2].Style["display"] = HttpContext.Current.User.IsInRole("Administrator") && isEdit ? "block" : "none";
        hidIsEdit.Value = isEdit.ToString();

        if (row != null)
        {

            rblCurrentDEARegistration.SelectedValue = row["DEA_VERIFIED"].ToString().ToLower() == "true" ? "Y" : "N";
            //rblCurrentDEARegistration.Attributes.Add("disable" = row["DEA_VERIFIED"].ToString().ToLower() == "true" ? "Y" : "N";
            //divAddFederalDEA.Visible = row["DEA_VERIFIED"].ToString() == "1" ? true : false ;
            //divDEAQuestionMapping.Visible = row["DEA_VERIFIED"].ToString() == "0" ? true : false;
            rblCurrentDEARegistration_SelectedIndexChanged(null, null);
            divAddFederalDEA.Visible = true;
            hidID.Text = row["REG_DEA_ID"].ToString();
            prov_Number.Text = txtPrescribeProviderDEA.Text = row["DEA_NUMBER"].ToString();
            prov_State.SelectedValue = ddlPrecribeProviderState.SelectedValue = row["DEA_STATE"].ToString();
            prov_Start.Text = Helper.FormatDate2(row["DEA_EFF_DATE"].ToString());
            prov_End.Text = Helper.FormatDate2(row["DEA_END_DATE"].ToString());
            ddlPrecribeProviderState.SelectedValue = row["DEA_STATE"].ToString();
            txtPrecibeComments.Text = row["PRESCRIBING_COMMENTS"].ToString();
            txtPrescribeProvider.Text = row["PRESCRIBE_PROVIDER_NAME"].ToString();

            if (rblCurrentDEARegistration.SelectedValue == "Y")
            {
                var deaStatus = Convert.ToString(row["DEA_STATUS"]);
                if (!string.IsNullOrEmpty(deaStatus))
                {
                    ddldeastatus.SelectedValue = deaStatus;
                }
                else
                {
                    ddldeastatus.SelectedValue = "Active";
                }

            }

            //pdms_Number.Text = Helper.GetString("PDMS_DEA_NUMBER", row);
            //pdms_State.Text = GetStateName(Helper.GetString("PDMS_DEA_STATE", row));
            //pdms_Start.Text = Helper.FormatDate2(Helper.GetString("PDMS_DEA_EFF_DATE", row));
            //pdms_End.Text = Helper.FormatDate2(Helper.GetString("PDMS_DEA_END_DATE", row));

        }
        this.rblCurrentDEARegistration.Enabled = !isEdit;
    }

    protected void ValidateLC72(object sender, ServerValidateEventArgs e)
    {
        SetDt();
        if (!string.IsNullOrEmpty(prov_Number.Text))
        {


            e.IsValid = !string.IsNullOrEmpty(prov_State.SelectedValue) && !string.IsNullOrEmpty(prov_Start.Text) && !string.IsNullOrEmpty(prov_End.Text);
        }
        else
        {
            e.IsValid = true;
        }
    }

    protected void ValidateLC74(object sender, ServerValidateEventArgs e)
    {
        SetDt();

        if (string.IsNullOrEmpty(prov_Start.Text) || !IsValidDate(prov_Start.Text))
        {
            e.IsValid = true;
            return;
        }

        e.IsValid = Convert.ToDateTime(prov_Start.Text) <= DateTime.Now;
    }


    private bool IsValidDate(string dateString)
    {
        if (string.IsNullOrWhiteSpace(dateString))
            return false;

        dateString = dateString.Trim();

        DateTime tempDate;

        bool isValid = DateTime.TryParse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out tempDate);
        return isValid;

    }

    protected void prov_Number_TextChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(prov_Number.Text))
        {
            //if (System.Text.RegularExpressions.Regex.IsMatch("^[a-zA-Z0-9]$", prov_Number.Text))
            //{
            //    prov_Number.Text.Remove(prov_Number.Text.Length - 1);
            //}

            DataTable dtDEARecordRegistrationDetails = svc.GetDEARecordRegistration(prov_Number.Text).Tables[0];
            if (Helper.HasRows(dtDEARecordRegistrationDetails))
            {
                //prov_Start.Text = Helper.FormatDate2(dtDEARecordRegistrationDetails.Rows[0]["start_date"].ToString());
                prov_End.Text = Helper.FormatDate2(dtDEARecordRegistrationDetails.Rows[0]["EXPIRATION_DATE"].ToString());
                prov_State.SelectedValue = dtDEARecordRegistrationDetails.Rows[0]["STATE"].ToString();
            }
            else
            {
                prov_Start.Text = "";
                prov_End.Text = "";
                prov_State.SelectedIndex = -1;
            }
        }

    }
    protected void grdCertificationk_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int index = Convert.ToInt32(e.CommandArgument);
        DataRow dr = this.DataList.Rows[index];

        if (e.CommandName == "DeleteCertificationDetails")
        {
            if (dr != null)
            {
                int REG_DEA_ID = Helper.GetInt("REG_DEA_ID", dr);
                if (REG_DEA_ID > 0)
                {
                    PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                    psc.DeleteRegistrationData("DEA", "REG_DEA_ID", REG_DEA_ID);
                    LoadDEACertification();
                }
            }
        }

        else
        {
            if (Helper.HasRows(this.DataList))
                this.LoadData(this.DataList.Rows[index]);
            else
                this.LoadData(null);
        }
    }

    public void AddgrdCertificationRowData()
    {

    }

    private bool ValidateExpirationDate()
    {
        bool valid = true;
        //if (Convert.ToDateTime(prov_Start.Text) >= Convert.ToDateTime(prov_End.Text))
        //{
        //    //CustomValidator val = new CustomValidator();
        //    //val.IsValid = false;
        //    //val.ErrorMessage = "* Expiration dates need to be greater than effective date.";
        //    //val.ValidationGroup = "Certifications";
        //    //this.Page.Validators.Add(val);
        //    //valid = false;
        //}
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationByRegID(this.WorkflowPage.RegistrationId);
        //if (Convert.ToDateTime(prov_End.Text) >= DateTime.Now)
        //{
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        int statusCode = Helper.GetInt("RegistrationProgramStatusTypeID", ds.Tables[0].Rows[0]);
        //        if (statusCode != 6)
        //        {
        //            int result = DateTime.Compare(Convert.ToDateTime(prov_End.Text), DateTime.Now);

        //            if (result < 0) // expiration is before today's date
        //            {
        //                CustomValidator val = new CustomValidator();
        //                val.IsValid = false;
        //                val.ErrorMessage = "* Expiration dates need to be greater than effective date.";
        //                val.ValidationGroup = "Certifications";
        //                this.Page.Validators.Add(val);
        //                valid = false;
        //            }
        //        }
        //    }
        //}

        return valid;
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

    public void LoadPlaceHolder(int deaId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();

        PlaceholderUploadDEA.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, deaId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, deaId, pageSection, isEdit);

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
                PlaceholderUploadDEA.Controls.Add(ucUploadSectionControl);
            }
        }

    }

    public override string ValidationGroup
    {
        get { return "Certifications"; }
    }

    public override string Title
    {
        get { return "Federal DEA Registration"; }
    }

    public override string IdText
    {
        get { return "ucDEACertification_" + this.WorkflowPage.RegistrationId; }
    }


    protected void rblCurrentDEARegistration_SelectedIndexChanged(object sender, EventArgs e)
    {
        prov_Number.Text = "";
        prov_Start.Text = "";
        prov_End.Text = "";
        prov_State.SelectedIndex = -1;
        if (rblCurrentDEARegistration.SelectedValue == "Y")
        {
            btnAddCertification.Visible = true;
            AddNewCertificationItem();
            divDEAQuestionMapping.Visible = false;
        }
        else if (rblCurrentDEARegistration.SelectedValue == "N")
        {
            divDEAQuestionMapping.Visible = true;
            divAddFederalDEA.Visible = false;
            btnAddCertification.Visible = false;
        }
        else
        {
            divDEAQuestionMapping.Visible = false;
            divAddFederalDEA.Visible = false;
            btnAddCertification.Visible = false;
        }
    }
    protected void lbtnAdd_Click(object sender, CommandEventArgs e)
    {
        AddNewCertificationItem();

    }

    private void AddNewCertificationItem()
    {
        divAddFederalDEA.Visible = true;
        prov_Number.Text = string.Empty;
        prov_State.SelectedValue = null;
        prov_Start.Text = string.Empty;
        prov_End.Text = string.Empty;
        txtPrescribeProvider.Text = string.Empty;
        txtPrescribeProviderDEA.Text = string.Empty;
        txtPrecibeComments.Text = string.Empty;
        //hidIsEdit.Value = string.Empty;
        this.WorkflowPage.IsSaved = false;
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

    protected void btnHistory_Click(object sender, CommandEventArgs e)
    {
        // TODO: EDV Here is where we should show/hide history
        switch (e.CommandName)
        {
            case "Certifications":
                lblTitle.Text = "DEA History";
                ucCertificationsHistory.LoadData();
                mpe.Show();
                break;
            default:
                break;
        }
    }
}