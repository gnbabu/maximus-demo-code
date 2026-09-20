using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class PopupControls_Reconsideration : BaseSectionControl
{
    private const string sectionName = "Reconsideration";

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
    private int RegReconsiderationID
    {
        get
        {
            return ViewState["RegReconsiderationID"] == null ? 0 : Convert.ToInt32(ViewState["RegReconsiderationID"]);
        }
        set
        {
            ViewState["RegReconsiderationID"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      
    }

    protected void ddlEnrollmentReasons_Init(object sender, EventArgs e)
    {
        DataSet dsEnrollmentReasons = svc.GetEnrollStatusReasons();
        if (Helper.HasRows(dsEnrollmentReasons))
        {
            Helper.LoadDropDown(ddlEnrollmentStatusReason, dsEnrollmentReasons.Tables[0], "ENROLLMENT_STATUS_REASONS_DESC", "ENROLLMENT_STATUS_REASONS_ID", true);
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
    }
    public void LoadPlaceHolder(int reconsiderationId = 0, bool isEdit = false, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadIntegrity.Controls.Clear();
        PlaceholderUploadNotesProgramIntegrityMeeting.Controls.Clear();
        DataSet ds = null;
        int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        if (loadViewState)
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, pageSection, false);
        else
            ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, pageSection, isEdit);

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
                ucUploadSectionControl.ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
                if (Helper.GetString("TITLE", dr) == "Request Reconsideration") //research on how to config appsetting or something instead of code it in here
                    PlaceholderUploadIntegrity.Controls.Add(ucUploadSectionControl);
               else
                    PlaceholderUploadNotesProgramIntegrityMeeting.Controls.Add(ucUploadSectionControl);
            }
        }

    }
    public override bool SaveData()
    {
        try
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("RECONSIDERATION_REQUEST_DATE", txtReconsidertionRequestDate.Text);
            if (!string.IsNullOrEmpty(txtDateofMeeting.Text))
            {
                parms.Add("MEETING_DATE", txtDateofMeeting.Text);
            }            
            parms.Add("EFFECTIVE_DATE", txtEffectiveDate.Text);
            parms.Add("TERMINATION_REASON", txtTerminationReason.Text);
            parms.Add("ENROLLMENT_STATUS_REASON_ID", ddlEnrollmentStatusReason.SelectedValue);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            int reconsiderationID = svc.UpdateRegistrationData(this.WorkflowPage.RegistrationId, "RECONSIDERATION", parms);
            
        }
        catch (Exception ex)
        {
            throw MAXIMUS.Core.Libraries.CoreException.ThrowException(new Exception("Reconsideration_SaveData - " + ex.Message));

        }

        return true;
    }

    public override void LoadControlData()
    {
        LoadReconsideration();
    }

    public void LoadReconsideration()
    {
        DataSet ds = svc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "RECONSIDERATION");
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
        int reconsiderationId = isEdit ? Helper.GetInt("REG_RECONSIDERATION_ID", dr) : 0;
        LoadPlaceHolder(reconsiderationId, isEdit);
        LoadDropDowns();

       // InitFormData();

        if (isEdit)
        {
            RegReconsiderationID = Helper.GetInt("REG_RECONSIDERATION_ID", dr);

            if (RegReconsiderationID > 0)
            {
                this.txtReconsidertionRequestDate.Text = Helper.FormatDate2(dr["RECONSIDERATION_REQUEST_DATE"].ToString());
                this.txtDateofMeeting.Text = Helper.FormatDate2(dr["MEETING_DATE"].ToString());
                string term_reason = Helper.GetString("TERMINATION_REASON", dr);
                if (string.IsNullOrEmpty(term_reason))
                {
                    this.txtTerminationReason.Text = Helper.GetString("ENROLLMENT_STATUS_REASONS_DESC", dr);
                }
                else
                {
                    this.txtTerminationReason.Text = Helper.GetString("TERMINATION_REASON", dr);
                }
                if(this.ddlEnrollmentStatusReason.Items.FindByValue(Helper.GetInt("ENROLLMENT_STATUS_REASONS_ID", dr).ToString()) != null)
                this.ddlEnrollmentStatusReason.SelectedValue = Helper.GetInt("ENROLLMENT_STATUS_REASONS_ID", dr).ToString();
                this.txtEffectiveDate.Text = Helper.FormatDate2(dr["EFFECTIVE_DATE"].ToString());
                //core table does not have npi
            }

           // ShowCoreValues(HttpContext.Current.User.IsInRole("Administrator") && isEdit);

            if(this.WorkflowPage.WF_TaskID == CON.ComplianceEnterTerminationReason)
            {
                pnlTerminationReconsider.Enabled =  pnlTerminationReconsider.Visible = true;
            }
            else
            {
                pnlTerminationReconsider.Enabled = pnlTerminationReconsider.Visible = false;

            }
        }
    }
    private void ShowCoreValues(bool isVisible)
    {

        this.txtReconsidertionRequestDate.Visible = true;
        this.txtDateofMeeting.Visible = true;
        /*
        if (isVisible)
        {
            this.txtReconsidertionRequestDate.Visible = false;
            this.txtDateofMeeting.Visible = true;
        }
        else
        {
            this.txtReconsidertionRequestDate.Visible = true;
            this.txtDateofMeeting.Visible = false;
        }
        */
    }
    private void LoadDropDowns()
    {
        DataSet ds = svc.GetEnrollStatusReasons();

        if (Helper.HasRows(ds))
        {
            Helper.LoadDropDown(this.ddlEnrollmentStatusReason, ds.Tables[0], "ENROLLMENT_STATUS_REASONS_DESC", "ENROLLMENT_STATUS_REASONS_ID", false);
            this.ddlEnrollmentStatusReason.Items.Insert(0, new ListItem(string.Empty, "0"));
        }
    }
    public override void LoadData(DataRow dr)
    {
    }

    public override bool ValidateData()
    {
        return true;
    }

    public override string ValidationGroup
    {
        get { return "valReconsideration"; }
    }

    public override string Title
    {
        get { return "Reconsideration SEARCH"; }
    }

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public override string IdText
    {
        get { return "ucReconsideration_" + this.WorkflowPage.RegistrationId; }
    }
    
}