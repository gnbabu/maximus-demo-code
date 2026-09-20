using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
public partial class PopupControls_Appeals_ascx : BaseSectionControl
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

    private const string sectionName = "Appeals";

    private DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isEdit = string.IsNullOrEmpty(hidIsEdit.Text) ? false : Convert.ToBoolean(hidIsEdit.Text);
        //this.WorkflowPage.FillRegistrationData();

        //LoadPlaceHolder(this.WorkflowPage.RegistrationStep);
        //LoadAppealNotices();


    }
    public override bool HasInputValue()
    {
        return true;
    }
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public override void LoadControlData()
    {
        LoadAppeals();
    }

    public void LoadAppeals()
    {
        bool isEdit = false;
        
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL_NOTICE");
        if(this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofTermination)
        {
            RequiredFieldValidator1.Enabled = RequiredFieldValidator2.Enabled = RequiredFieldValidator3.Enabled = false;
        }
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination)
        {
            RequiredFieldValidator2.Enabled = RequiredFieldValidator3.Enabled = false;
        }
            DataSet dsTermStatus = psc.GetTerminationStatuses();
        this.ddlTermReason.DataSource = dsTermStatus;
        this.ddlTermReason.DataValueField = "ENROLLMENT_STATUS_CODE";
        this.ddlTermReason.DataTextField = "ENROLLMENT_STATUS_CODE_DESCRIPTION";
        this.ddlTermReason.DataBind();
        this.ddlTermReason.Items.Insert(0, new ListItem("", ""));
        hidID.Text = "0";
        int appealsID = 0;
        if (Helper.HasRows(ds))
        {
            isEdit = true;
            DataTable dtAppealNotices = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
            txtDateOfInitialNotice.Text = Helper.FormatDate2(dtAppealNotices.Rows[0]["INITIAL_NOTICE_DATE"].ToString());
            txtDateOfFinalNotice.Text = Helper.FormatDate2(dtAppealNotices.Rows[0]["FINAL_NOTICE_DATE"].ToString());
            hidID.Text = Helper.GetString("REG_APPEAL_NOTICE_ID",dtAppealNotices.Rows[0]);
            appealsID = Helper.GetInt("REG_APPEAL_NOTICE_ID", dtAppealNotices.Rows[0]);
            ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDER");
            if (!string.IsNullOrEmpty(Helper.GetString("Enrollment_Status_Code", ds.Tables[0].Rows[0])))
            {
                ddlTermReason.SelectedValue = Helper.GetString("ENROLLMENT_STATUS_CODE", ds.Tables[0].Rows[0]);
            }
            if (!string.IsNullOrEmpty(Helper.GetString("Term_Date", ds.Tables[0].Rows[0])))
                txtDateOfDenialOrTermination.Text = Helper.GetDate("Term_Date", ds.Tables[0].Rows[0]);
        }
        LoadPlaceHolder(appealsID, isEdit, false, "Appeals");
    }

    public void LoadPlaceHolder(int appealsId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderInitial.Controls.Clear();
        PlaceholderFinal.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, appealsId, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, appealsId, pageSection, isEdit);

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
                if (Helper.GetString("TITLE", dr) == "Initial Notice")
                    PlaceholderInitial.Controls.Add(ucUploadSectionControl);
                else if (Helper.GetString("TITLE", dr) == "Final Notice")
                    PlaceholderFinal.Controls.Add(ucUploadSectionControl);
            }
        }

    }

    //public void LoadPlaceHolder(int pageStep)
    //    {
    //    Upload upload = new Upload();
    //    string sectionName = Registration.GetSectionNameFromStepNumber(pageStep);
    //    int pageID = Registration.GetPageIDFromSectionID(pageStep);

    //    DataSet ds = new DataSet();
    //    //if(Helper.HasRows(ds))
    //    //section level documents should be shown under respective sections
    //    //LoadUploadSection(ds, false); 
    //    ds = null;
        
    //        sectionName = "Appeals";


    //        if (this.WorkflowPage.RegistrationStep == 46)
    //    {
    //        ds = upload.LoadUserSectionControl(pageID, sectionName);
    //    }


    //    if(ds != null)
    //    {
    //        LoadUploadSection(ds, false, sectionName);
    //    }

    //    /*sectionName = "FinalNotice";
    //if (this.WorkflowPage.RegistrationStep == 46)
    //{
    //    ds = upload.LoadUserSectionControl(pageID, sectionName);
    //}



    //LoadUploadSection(ds, true, sectionName);*/


    //}
    private void LoadUploadSection(DataSet ds, bool isPageLevelUpload, string sectionName)
    {
        int table = ds.Tables.Count;
        PlaceholderInitial.Controls.Clear();
        PlaceholderFinal.Controls.Clear();
        if (PlaceholderInitial.Controls.Count == 0 && PlaceholderFinal.Controls.Count == 0)
        {
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

                    if ((ds.Tables[i].Columns.Contains("IS_REQUIRED")))
                        ucUploadSectionControl.IsRequired = Convert.ToBoolean(Helper.GetString("IS_REQUIRED", dr));
                    if (isPageLevelUpload && this.WorkflowPage.WorkflowEventTypeId == CON.WorkflowEventType.UpdateReg)
                    {
                        ucUploadSectionControl.IsRequired = false;
                    }

                    if ((ds.Tables[i].Columns.Contains("FILE_NAME")))
                        ucUploadSectionControl.FileName = Helper.GetString("FILE_NAME", dr);
                    else
                        ucUploadSectionControl.FileName = null;

                    ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";

                    //check if a control already exist with same ID
                    //var placeHolder = PlaceholderUploadSectionControl.FindControl("");
                    if(Helper.GetString("TITLE", dr) == "Initial Notice")
                        PlaceholderInitial.Controls.Add(ucUploadSectionControl);
                    else if (Helper.GetString("TITLE", dr) == "Final Notice")
                        PlaceholderFinal.Controls.Add(ucUploadSectionControl);
                }
            }
        }
    }

    public override bool SaveData()
    {
        Page.Validate("valAppeals");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valAppeals") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        bool isValid = true;
        foreach (Control ctrl in PlaceholderInitial.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("valAppeals");
        }
        foreach (Control ctrl in PlaceholderFinal.Controls)
        {
            UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
            isValid &= uploadControl.ValidateData("valAppeals");
        }

        if (!isValid)
            return isValid;
        int APPEAL_NOTICE_ID = 0;
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
        if(!string.IsNullOrEmpty(txtDateOfInitialNotice.Text))
        parms.Add("INITIAL_NOTICE_DATE", txtDateOfInitialNotice.Text);
        if(!string.IsNullOrEmpty(txtDateOfFinalNotice.Text))
        parms.Add("FINAL_NOTICE_DATE", txtDateOfFinalNotice.Text);
        if(this.WorkflowPage.CurrentTaskName == "Upload Initial Notice of Denial" || this.WorkflowPage.CurrentTaskName == "Upload Initial Notice of Termination")
        parms.Add("APPEAL_NOTICE_STATUS_ID", CON.AppealNoticeStatusID.InitialNoticeSent.ToString());
        else if (this.WorkflowPage.CurrentTaskName == "Upload Final Notice of Denial" || this.WorkflowPage.CurrentTaskName == "Upload Final Notice of Termination")
            parms.Add("APPEAL_NOTICE_STATUS_ID", CON.AppealNoticeStatusID.FinalNoticeSent.ToString());


        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        bool isEdit = string.IsNullOrEmpty(hidID.Text) || hidID.Text == "0" ? false : true;
        if (isEdit)
        {
            
            parms.Add("REG_APPEAL_NOTICE_ID", hidID.Text);
            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL_NOTICE", parms);
            APPEAL_NOTICE_ID = int.Parse(hidID.Text); // we are only getting reg id back from UpdateRegistrationData
        }
        else
        {
            
            APPEAL_NOTICE_ID = svc.InsertRegistrationData(this.WorkflowPage.RegistrationId, "APPEAL_NOTICE", parms);
        }

        if(!string.IsNullOrEmpty(txtDateOfDenialOrTermination.Text))
        {
            
            parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("TERM_DATE", txtDateOfDenialOrTermination.Text);
            if(ddlTermReason.SelectedIndex > 0)
            {
                parms.Add("ENROLLMENT_STATUS_CODE", ddlTermReason.SelectedValue);
            }
            parms.Add("END_DATE", txtDateOfDenialOrTermination.Text);

            svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "PROVIDERCustom", parms);

            Dictionary<string, string> parmsChangeEffDate = new Dictionary<string, string>();
            parmsChangeEffDate.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            //parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parmsChangeEffDate.Add("CHANGE_EFFECTIVE_DATE", txtDateOfDenialOrTermination.Text);
            svc.UpdateRegistration(parmsChangeEffDate);
        }


        return true;
    }
    public override bool ValidateData()
    {
        Page.Validate("valAppeals");

        for (int i = 0; i < Page.Validators.Count; i++)
        {
            BaseValidator v;
            try
            {
                v = Page.Validators[i] as BaseValidator;
                if (v != null && v.ValidationGroup.Equals("valAppeals") && !v.IsValid)
                    return false;
            }
            catch
            {
                continue;
            }
        }

        bool isValid = true;
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadInitialNoticeofTermination)
        {
            foreach (Control ctrl in PlaceholderInitial.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                if (uploadControl.FileName == null)
                {
                    AddError("Initial Upload Document required.", ref isValid);
                    return isValid;
                }
            }
        }
        if (this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofDenial || this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.UploadFinalNoticeofTermination)
        {
            foreach (Control ctrl in PlaceholderFinal.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                if (uploadControl.FileName == null)
                {
                    AddError("Final Upload Document required.", ref isValid);
                    return isValid;
                }
            }
        }


        return isValid;
    }
    private void AddError(string msg, ref bool isGood)
    {
        CustomValidator val = new CustomValidator();
        val.IsValid = false;
        val.ErrorMessage = msg;
        val.ValidationGroup = "valAppeals";
        this.Page.Validators.Add(val);
        isGood = false;
    }

    public override void LoadData(DataRow row)
    {
    }

    public override string ValidationGroup
    {
        get { return "Appeals"; }
    }

    public override string Title
    {
        get { return "Appeals"; }
    }

    public override string IdText
    {
        get { return "ucAppeals_" + this.WorkflowPage.RegistrationId; }
    }

}