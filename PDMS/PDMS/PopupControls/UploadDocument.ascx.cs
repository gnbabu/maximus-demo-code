using MAXIMUS.DataExchange.PDMS;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Collections.Generic;
using Corp.Core.Libraries;
public partial class UserControls_UploadDocument : System.Web.UI.UserControl
{

    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

	private enum DocumentCollectionType
	{
		None,
		Party,
		RegistrationStep,
		ScreeningActivity
	}

    private int _inMaintenance = -1;

    public delegate string RenameFile(string dir, string input);    // Set this method if you want the file uploaded to the directory to be renamed.
    public event RenameFile RenameFileMethod;
    public delegate int SuccessEventHandler(string fileName);
    public event SuccessEventHandler SuccessEvent;
    public delegate void FileDeleteEventHandler();
    public event FileDeleteEventHandler FileDeleteSuccessEvent;
    public delegate void FailEventHandler();
    public event FailEventHandler FailEvent;
    public delegate void ValidateEventHandler(ref string errMsg);
    public event ValidateEventHandler ValidateEvent;

    protected bool _DeleteDisabled = false;
    protected int _MaxFileMegaBytes;
    protected string _CssClassFileUpLoad;
    protected string _CssClassUploadButton;
    protected string _DestinationPath;
    protected string _ValidFileExtensions;

    public string CssClassFileUpLoad
    {
        get { return _CssClassFileUpLoad; }
        set
        {
            _CssClassFileUpLoad = value;
            if (!string.IsNullOrEmpty(value))  filUploadFile.CssClass = value;
        }
    }

    public string CssClassUploadButton
    {
        get { return _CssClassUploadButton; }
        set
        {
            _CssClassUploadButton = value;
            if (!string.IsNullOrEmpty(value)) UploadButton.CssClass = value;
        }
    }

    public string DestinationPath
    {
        get { return _DestinationPath; }
        set { _DestinationPath = value; }
    }

    public string ValidFileExtensions
    {
        get { return _ValidFileExtensions; }
        set { _ValidFileExtensions = value; }
    }
    
    
    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 80;       // Default is 5MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80;
        }
    }

    public bool DeleteDisabled
    {
        get { return _DeleteDisabled; }
        set { _DeleteDisabled = value; }
    }

    public bool SetUploadButtonEnabled
    {
        get { return UploadButton.Enabled; }
        set { UploadButton.Enabled = value; }
    }

    public string UpdateButtonText
    {
        set { UploadButton.Text = value; }
    }

    public string DocumentName
    {
        get { return string.IsNullOrEmpty(txtName.Text) ? Helper.CleanFilePath(filUploadFile.FileName) : Helper.CleanFilePath(txtName.Text); }
        set { txtName.Text = value; }
    }

    public string DocumentDescription
    {
        get { return txtDescription.Text; }
        set { txtDescription.Text = value; }
    }

	private DocumentCollectionType CollectionType
	{
		get
		{
			if (ViewState["CollectionType"] == null)
				ViewState["CollectionType"] = DocumentCollectionType.None;

			return (DocumentCollectionType)ViewState["CollectionType"];
		}
		set
		{
			ViewState["CollectionType"] = value;
		}
	}

	public int ScreeningActivityID
	{
		get
		{
			if (ViewState["ScreeningActivityID"] == null)
				ViewState["ScreeningActivityID"] = -1;

			return (int)ViewState["ScreeningActivityID"];
		}
		set
		{
			ViewState["ScreeningActivityID"] = value;
		}
	}


    public string DocumentSection
    {
        get
        {
            if (ViewState["DocumentSection"] == null)
                ViewState["DocumentSection"] = string.Empty;

            return ViewState["DocumentSection"].ToString();
        }
        set
        {
            ViewState["DocumentSection"] = value;
        }
    }
    public void LoadData(int partyId)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectDocumentsByPartyId(partyId);
        if (Helper.HasRows(ds))
        {
            //bug 2346 - "The internal uploads should not be displayed to the providers."
            if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name))
            {
                DataRow[] rows = ds.Tables[0].Select(string.Format("RoleName IN ('{0}','{1}' )",MAXIMUS.Core.Libraries.Constants.UserRoleType.ProviderAdministrator, MAXIMUS.Core.Libraries.Constants.UserRoleType.ProviderOper));
                gvUploadedDocs.DataSource = rows.Length == 0 ? null : rows.CopyToDataTable();
            }
            else
            {
                gvUploadedDocs.DataSource = ds.Tables[0];
            }
        }
        else gvUploadedDocs.DataSource = null;
        gvUploadedDocs.DataBind();
    }

    public void LoadPaperDocs(DataSet ds)
    {
         gvUploadedDocs.DataSource = ds.Tables[0];
         gvUploadedDocs.DataBind();
         gvUploadedDocs.Columns[3].Visible = false;
    }

    
    public void LoadData(int regId, int regPageTypeId, string regPageSection = "", int? screeningActivityID = null)
	{
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        //regPageSection = "OtherDocuments";       
        // For all pages show everything except Certification documents
        // For Certification show just Certification documents
        string exclusions = string.Empty;
        bool showEducationWorkDocs = false;
        if (regPageTypeId != CON.RegistrationPageType.Certification)
        {
            //DCPDMS-2114//DCPDMS2189
            if (regPageTypeId == CON.RegistrationPageType.Agreements)
            {
                // Show all documents and exclude Certifications
                regPageTypeId = 8;
                //exclusions = CON.RegistrationPageType.Certification.ToString();
            }
            if (regPageTypeId == CON.RegistrationPageType.ProviderScreening)
            {
                if (this.WorkflowPage.CurrentTaskName.ToString() == "DBH Reviewer Review")
                    regPageSection = "DBH Review";
                else
                // Show all documents and exclude Certifications
                regPageTypeId = 13;
                regPageSection = "Provider Screening";
                //exclusions = CON.RegistrationPageType.Certification.ToString();
            }
            else if (regPageTypeId == CON.RegistrationPageType.OwnerScreening)
            {              
                regPageTypeId = 15;
                regPageSection = "Owner Screening";             
            }
            else if (regPageTypeId == CON.RegistrationPageType.BackgroundCheck)
            {
                regPageTypeId = 22;
                regPageSection = "Fingerprint Check";
            }
            else if (regPageTypeId == CON.RegistrationPageType.OrientationInformation)
            {
                regPageTypeId = 23;
                regPageSection = "Orientation Session Screening";
            }
            else if (regPageTypeId == CON.RegistrationPageType.SiteVisitScreening)
            {
                regPageTypeId = 16;
                regPageSection = "SiteVisit";
            }
            else if (regPageTypeId == 44)
            {
                regPageTypeId = 8;
                regPageSection = "OtherDocuments";
            }
            else if (regPageTypeId == 37)
            {
                regPageTypeId = 24;
                regPageSection = "DME";
                DMELabel.Visible = true;
            }
            else if (regPageTypeId == CON.SectionTypeID.ProviderCredentialing)
            {
               regPageTypeId = 28;
               if (
                   Helper.IsLoggedInUserInAdminRole()
                   || Helper.IsUserInCredentialingRole(HttpContext.Current.User.Identity.Name)
                   )
               {
                   showEducationWorkDocs = true;
               }
            }
            else if (regPageTypeId == CON.SectionTypeID.PracticePartnership)
            {
                regPageTypeId = 4;
                regPageSection = "PracticePartnership";
            }
            else
            {
                // Show all documents and exclude Certifications
                //regPageTypeId = 0;
            }
            exclusions = CON.RegistrationPageType.Certification.ToString();
        }
        // Exclude contracts
        if(regPageTypeId != CON.RegistrationPageType.Contracts)
            exclusions += "," + CON.RegistrationPageType.Contracts.ToString();
        string userRole = Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) ? Helper.GetUserRole(HttpContext.Current.User.Identity.Name) : SessionVarRetriever.MyQueueSelectedRoleName;
        
        DataSet ds = psc.SelectRegDocuments(regId, regPageTypeId, regPageSection, exclusions, screeningActivityID,userRole);

        if (showEducationWorkDocs)
        {
            DataSet dsEducation = psc.SelectRegDocuments(regId, CON.SectionTypeID.WorkHistory, string.Empty, exclusions, screeningActivityID, userRole);

            DataSet dsWorkHistory = psc.SelectRegDocuments(regId, CON.SectionTypeID.EmploymentHistory, string.Empty, exclusions, screeningActivityID, userRole);
            ds.Merge(dsEducation);
            ds.Merge(dsWorkHistory);
        }

        if (Helper.HasRows(ds))
        {
            
            DataView dv = new DataView(ds.Tables[0]);
            //dv.RowFilter = "REG_PAGE_NAME LIKE '%" + DocumentSection + "%'";
                if(!(this.WorkflowPage.RegistrationStep==CON.SectionTypeID.ClosureNotice || this.WorkflowPage.RegistrationStep==CON.SectionTypeID.DaysNotice))
                    dv.RowFilter = "REG_PAGE_NAME IS NULL OR REG_PAGE_NAME LIKE '%" + regPageSection + "%'";
                DataTable newTable = new DataTable();
                newTable = dv.ToTable();
            int id = this.WorkflowPage.ApplicationTypeID;
            if (Helper.HasRows(newTable) && regPageTypeId == CON.RegistrationPageType.ApplicationFee)
            {
                // this bit below doesn't seem right; disabling for now
                /* DataTable appFeeTable = new DataTable();
                var appTable = newTable.AsEnumerable().Where(r => r.Field<string>("REG_PAGE_SECTION") != CON.sectionName);
                if (appTable.Any())
                {
                    appFeeTable = appTable.AsEnumerable().CopyToDataTable();
                    gvUploadedDocs.DataSource = appFeeTable;
                }
                else
                    gvUploadedDocs.DataSource = null;
                */

                // remove the bottom document from the main document upload control
                DataTable appFeeTable = new DataTable();
                var appTable = newTable.AsEnumerable().Where(r => r.Field<string>("Document_Upload_Page") == "Application Fee Information");
                if (appTable.Any())
                {
                    appFeeTable = appTable.AsEnumerable().CopyToDataTable();
                    gvUploadedDocs.DataSource = appFeeTable;
                }
                else
                    gvUploadedDocs.DataSource = null;
            }
            else
                gvUploadedDocs.DataSource = newTable;
            //}
        }
        else gvUploadedDocs.DataSource = null;

		CollectionType = DocumentCollectionType.RegistrationStep;
        gvUploadedDocs.DataBind();

        // OHPNM-1917
        if (_inMaintenance == -1)
        {
            this.areWeInMaintenance();
        }

        #region An internal user needs to be able to upload a document to a provider record(OHPNM-8683)
        // OHPNM-1917
        if (_inMaintenance == 1)
        {
            bool isInternalUser = Helper.IsUserInInternalRoles(HttpContext.Current.User.Identity.Name);
            if (!isInternalUser)
            {
                Helper.SetReadOnly(divupload, true, "formFieldReadOnly");
                Helper.SetReadOnly(UploadButton, true);
            }
        }
        #endregion
        if (regPageTypeId == CON.SectionTypeID.HearingRights)
        {
            if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
            {
                if (this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordDateOfMailReturn || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordHearingStatus ||
                    this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.CSUploadPI || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadAO ||
                    this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadPAO)
                {
                    Helper.SetReadOnly(divupload, false, "formFieldEditable");
                    Helper.SetReadOnly(UploadButton, false);
                }
                else
                {
                    Helper.SetReadOnly(divupload, true, "formFieldReadOnly");
                    Helper.SetReadOnly(UploadButton, true);
                }
            }
            else
            {
                Helper.SetReadOnly(divupload, true, "formFieldReadOnly");
                Helper.SetReadOnly(UploadButton, true);
            }
        }
    }

    private void areWeInMaintenance()
    {
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            _inMaintenance = 1;
        }
        else
        {
            _inMaintenance = 0;
        }
    }

    public void LoadDataForScreeningActivity(int screeningActivityID)
	{
		ScreeningActivityID = screeningActivityID;
		PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectScreeningActivityDocuments(screeningActivityID, Helper.GetUserRole(HttpContext.Current.User.Identity.Name));
		if (Helper.HasRows(ds))
		{
			gvUploadedDocs.DataSource = ds.Tables[0];
		}
		else
		{
			gvUploadedDocs.DataSource = null;
		}

		CollectionType = DocumentCollectionType.ScreeningActivity;
        gvUploadedDocs.DataBind();
        // Read-Only users do not have the ability to delete documents
        gvUploadedDocs.Columns[5].Visible = !Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRoleType.ReadOnly);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatusMsg.Text = string.Empty;
    }

    private bool IsValidExtension(out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = Helper.CleanFilePath(filUploadFile.PostedFile.FileName);

        // extarct and store the file extension into another variable
        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        // string type array having list of allowed file type extensions
        string[] validFileExtensions = _ValidFileExtensions.Split(',');
        // loop over the array of valid file extensions to compare them with uploaded file
        foreach (string extension in validFileExtensions)
        {
            if (fileExtension == extension)
            {
                rtn = true;
                break;
            }
        }

        // display the message based on the flag value
        if (!rtn)
        {
            errMsg = "Files with extension <b>\"" + fileExtension + "\"</b> are not allowed.<br />";
            errMsg += "You can upload files with the following extensions only:";
            foreach (string str in validFileExtensions)
            {
                errMsg += " ." + str + ",";
            }
            errMsg = errMsg.Substring(0, errMsg.Length - 1);            // Remove "," at end
        }
        return rtn;
    }

    protected void UploadButton_Click(object sender, EventArgs e)
    {
        lblStatusMsg.Text = string.Empty;
        if (filUploadFile.PostedFile == null)
        {
            lblStatusMsg.Text = "Unable to find posted file.";
            goto Failure;
        }
        if (string.IsNullOrEmpty(filUploadFile.PostedFile.FileName))
        {
            lblStatusMsg.Text = "Select a file for upload";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_DestinationPath))
        {
            lblStatusMsg.Text = "ERROR - DestinationPath must have a value";
            goto Failure;
        }
        if (string.IsNullOrEmpty(_ValidFileExtensions))
        {
            lblStatusMsg.Text = "ERROR - ValidFileExtensions must have a value";
            goto Failure;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg))
        {
            lblStatusMsg.Text = Helper.HtmlEncode(errMsg);
            goto Failure;
        }
        if (!filUploadFile.HasFile)
        {
            lblStatusMsg.Text = "No File has been uploaded.";
            goto Failure;
        }
        if (filUploadFile.PostedFile.ContentLength < 1)
        {
            lblStatusMsg.Text = "File cannot be 0Kb.";
            goto Failure;
        }
        if (filUploadFile.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblStatusMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            goto Failure;
        }

        if (!IsSpecialCharacter(filUploadFile.FileName))
        {
            lblStatusMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(filUploadFile.FileName) + " Please remove the Special Character from the file Name before upload. ";
            goto Failure;
        }
        if (ValidateEvent != null)
        {
            ValidateEvent(ref errMsg);
            if (!string.IsNullOrEmpty(errMsg))
            {
                lblStatusMsg.Text = errMsg;
                goto Failure;
            }
        }
        try
        {
            // Use a different destination if debugging
            //#if DEBUG
                //_DestinationPath = "C:\\Projects\\PDMS\\PDMS\\FileStoreLocal\\";
            //#endif
            //_DestinationPath = "C:\\Projects\\Upload\\";

            if (System.Diagnostics.Debugger.IsAttached)
                _DestinationPath = @"C:\Temp\";

            string newFileName = Helper.CleanFilePath(filUploadFile.FileName);
            byte[] fileBytes = filUploadFile.EncryptedFileBytes;
            if (RenameFileMethod != null) newFileName = RenameFileMethod(@_DestinationPath, newFileName);      // Rename the file
            //filUploadFile.SaveAs(@_DestinationPath + newFileName);
            File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);
            lblStatusMsg.Text = "File Uploaded: " + newFileName;
            if (SuccessEvent != null)
            {
                int docID = SuccessEvent(@_DestinationPath + newFileName);
                SendToCMS(docID, fileBytes, newFileName);
            }
            return;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
    Failure:
        if (FailEvent != null) FailEvent();
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(this.WorkflowPage.RegistrationId, docID, fileBytes, fileName);
    }

    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    protected void gvUploadedDocs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gvUploadedDocs_RowEditing(object sender, GridViewEditEventArgs e)
    {
    }

    protected void gvUploadedDocs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        ImageButton img = (ImageButton)e.Row.FindControl("imgView");
        if (img != null) 
        {
            OnBaseInterface ob = new OnBaseInterface();
            int OnBaseId = 0;
            if(!String.IsNullOrEmpty( DataBinder.Eval(e.Row.DataItem, "ONBASE_DOCUMENT_ID").ToString()))
                OnBaseId = int.Parse(DataBinder.Eval(e.Row.DataItem, "ONBASE_DOCUMENT_ID").ToString());
            string isEnycrypted = "";
            if (Boolean.Parse(DataBinder.Eval(e.Row.DataItem, "IS_CONVERSION").ToString()) == true) 
            { isEnycrypted = "&UseEncryption=Y"; }
            string url = "../ViewFile.aspx?OnBaseId=" + ob.ObfuscateId(OnBaseId) + isEnycrypted;
            img.OnClientClick = "window.open('" + url + "');return false;";
        }
        img = (ImageButton)e.Row.FindControl("imgCancel");
        if (img != null)
        {
            if (DeleteDisabled) img.Visible = false;
            else
            {
                img.Visible = false;
                // Document role is Provider, you must be a Provider role in order to delete
                // Document role is NOT Provider, you cannot be in the Provider role in order to delete
                if (DataBinder.Eval(e.Row.DataItem, "RoleName").ToString() == MAXIMUS.Core.Libraries.Constants.UserRoleType.ProviderAdministrator)
                {
                    if (Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)) img.Visible = true;
                }
                else if (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name)) img.Visible = true;
                if (img.Visible) img.Attributes.Add("onclick", "javascript:return confirm('Uploaded document will be deleted. Are you sure?');");
            }

            //if (DataBinder.Eval(e.Row.DataItem, "REG_PAGE_SECTION").ToString() == "OtherDocuments")
            //{
            img.Visible = false;
            if (Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString() == DataBinder.Eval(e.Row.DataItem, "LAST_MODIFIED_USER").ToString())
            {

                if (DataBinder.Eval(e.Row.DataItem, "WFTaskName").ToString() == this.WorkflowPage.CurrentTaskName)
                {
                    img.Visible = true;
                }
            }

            if (_inMaintenance == -1)
            {
                this.areWeInMaintenance();
            }

            // OHPNM-4952 - disable delete button if in maintenance
            if (_inMaintenance == 1)
            {
                img.Visible = false;
            }
            //}
        }
    }

    protected void gvUploadedDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.DeleteDocument(Convert.ToInt32(e.CommandArgument));

            if (this.WorkflowPage.RegistrationId > 0 && this.WorkflowPage.RegistrationStep > 0)
                LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);

            if (FileDeleteSuccessEvent != null)
            {
                FileDeleteSuccessEvent();
            }
        }
    }

    public int NumberOfDocuments()
    {
        return gvUploadedDocs.Rows.Count;
    }
	
    public Boolean inMaintenance(int registrationId)
    {
        // get reference to service client
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();

        // get registration work flow current step
        int currentStepID = -1;
        DataSet wfProcessDs = psc.GetWFProcessByRegId(registrationId);
        if (Helper.HasRows(wfProcessDs))
        {
            currentStepID = Helper.GetInt("CURRENT_STEP_ID", wfProcessDs.Tables[0].Rows[0]);
        }

        // get registration program status type id
        int registrationProgramStatusTypeID = -1;
        DataSet regDs = psc.SelectRegistrationByRegID(registrationId);
        if (Helper.HasRows(regDs))
        {
            registrationProgramStatusTypeID = Helper.GetInt("RegProgramStatusTypeID", regDs.Tables[0].Rows[0]);
        }

        if (registrationProgramStatusTypeID == 2 && currentStepID == 0)
        {
            // in maintenance
            return true;
        }

        return false;
    }
}