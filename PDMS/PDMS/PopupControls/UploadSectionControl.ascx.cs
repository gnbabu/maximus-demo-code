using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Data.SqlClient;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Linq;
using Corp.Core.Libraries;

public partial class UserControls_UploadSectionControl : System.Web.UI.UserControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public delegate int SuccessEventHandler(string fileName);
    public event SuccessEventHandler SuccessEvent;

    public delegate void RevoveEventHandler();
    public event RevoveEventHandler RemoveEvent;

    protected string _DestinationPath;
    protected string _ValidFileExtensions;
    protected int _MaxFileMegaBytes;
    protected bool _RemovePermissions = true;
    public string UploadDocumentMessage { get; set; }
    protected string _FileName;

    public string FileName
    {
        get
        {
            return _FileName;
        }
  
        set
        {
            _FileName = value;
            if (string.IsNullOrEmpty(FileName))
                RadAsyncUpload1.Enabled = true;
            else
                RadAsyncUpload1.Enabled = false;
        }
    }

    public int DocumentId
    {
        get
        {
            return string.IsNullOrEmpty(hdndocumentId.Value) ? 0 : int.Parse(hdndocumentId.Value);
        }
        set { hdndocumentId.Value = value.ToString(); }
    }

    public int RowId
    {
        get
        {
            return string.IsNullOrEmpty(hdnRowId.Value) ? 0 : int.Parse(hdnRowId.Value);
        }
        set { hdnRowId.Value = value.ToString(); }
    }

    public string SectionName
    {
        get
        {
            return string.IsNullOrEmpty(hdnSectionName.Value) ? null : hdnSectionName.Value;
        }
        set { hdnSectionName.Value = value.ToString(); }
    }

    public bool RemovePermissions
    {
        get { return _RemovePermissions; }
        set { _RemovePermissions = value; }
    }
    public string UploadDocumentInfo { get; set; }

    public bool IsRequired { get; set; }

    public bool IsDisabled { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 80;       // on base can handle upto 80MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80;
        }
    }

    public string ValidFileExtensions
    {
        get { return _ValidFileExtensions; }
        set { _ValidFileExtensions = value; }
    }

    public string DestinationPath
    {
        get { return _DestinationPath; }
        set { _DestinationPath = value; }
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

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.LnkButtonDownload);

        DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        uploadLabel.Text = Helper.HtmlEncode(Title);
        uploadInfo.Text = Helper.HtmlEncode(Description);
        CustomValidatorUpload.ErrorMessage = "";
        if (!this.IsPostBack)
        {
            if (FileName != null && !string.IsNullOrEmpty(FileName))
            {
                var filepath = DestinationPath + @"\" + FileName;
                LblFileName.Text = Helper.HtmlEncode(FileName);
                if(RemovePermissions)
                    LnkButtonDelete.Visible = true;
                LnkButtonDownload.Visible = true;
            }
            else
            {
                LblFileName.Text = null;
                LnkButtonDelete.Visible = false;
                LnkButtonDownload.Visible = false;
                RadAsyncUpload1.Enabled = true;
                LnkButtonArchive.Visible = false;
            }
        }
        else 
        {
            if (FileName != null && !string.IsNullOrEmpty(FileName))
            {
                var filepath = DestinationPath + @"\" + FileName;
                LblFileName.Text = Helper.HtmlEncode(FileName);
                if (RemovePermissions)
                    LnkButtonDelete.Visible = true;
                LnkButtonDownload.Visible = true;
            }
            else
            {
                LblFileName.Text = null;
                LnkButtonDelete.Visible = false;
                LnkButtonDownload.Visible = false;
                RadAsyncUpload1.Enabled = true;
                LnkButtonArchive.Visible = false;
            }
        }

        ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
        RadAsyncUpload1.AllowedFileExtensions = ValidFileExtensions.Split(',');

        LnkButtonDelete.Click -= OnFileRemove;
        LnkButtonDelete.Click += OnFileRemove;

        LnkButtonDownload.Click -= OnFileDownload;
        LnkButtonDownload.Click += OnFileDownload;

        if (!((this.WorkflowPage.RegistrationStep == CON.SectionTypeID.Appeals) && Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.AppealSpecialist)) &&
            !((this.WorkflowPage.RegistrationStep == CON.SectionTypeID.IncidentComplianceReview) && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)) &&
            (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry || (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && !Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name))))
        {
            LnkButtonDelete.Visible = false;
        }

        //OHPNM-14997 - Allow Compliance spl to upload 2nd NOD or POC on SiteVisitScreening page.
        if(this.WorkflowPage.CurrentTaskName == CON.RegistrationTaskName.SiteVisitCompliance 
            && this.WorkflowPage.RegistrationStep == CON.RegistrationPageType.SiteVisitScreening
            && Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name)
            && (Title == "Notice Of Deficiency" || Title == "Plan of Correction"))
        {
            if (FileName != null && !string.IsNullOrEmpty(FileName))
            {
                LnkButtonArchive.Visible = true;
            }
        }

        if (IsRequired)
        {
            lblUploadRequired.Text = "Required Document";
        }
        else
        {
            lblUploadRequired.Text = "Optional Document";
        }

        divUploadControlHint.Visible = uploadLabel.Text == "ODI Application" ? true : false;
        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            if (!Helper.IsUserInInternalRole(CON.MaintenanceInternalRoles))
            {
                Helper.SetReadOnly(upShowNames1, true, "formFieldReadOnly");
            }
        }

        //if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.CredentialingSpecialist))
        //{
        //    div45DaysNotice.Visible = false;
        //}
        //ohpnm-9537
        if (SectionName == "HearingRights")
        {
            if (Helper.IsUserComplianceSpecialist(HttpContext.Current.User.Identity.Name))
            {
                if (this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordDateOfMailReturn || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.RecordHearingStatus ||
                    this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.CSUploadPI || this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadAO ||
                    this.WorkflowPage.CurrentTaskName == CON.ComplianceTaskName.UploadPAO)
                {
                    Helper.SetReadOnly(upShowNames1, false, "formFieldEditable");
                }
                else
                {
                    Helper.SetReadOnly(upShowNames1, true, "formFieldReadOnly");
                }
            }
            else
            {
                Helper.SetReadOnly(upShowNames1, true, "formFieldReadOnly");
            }
        }

        // OHPNM-11563 - added way to disable upload control if container page is disabled
        if (IsDisabled) {
            RadAsyncUpload1.Enabled = false;
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        // TODO: check whether this empty method is needed
    }

    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private byte[] EncryptedFileBytes(UploadedFile file)
    {
        byte[] bytes = new byte[file.ContentLength];
        file.InputStream.Read(bytes, 0, (int)file.ContentLength);

        Encryption encryption = new Encryption();
        return encryption.EncryptRijndael(bytes);

    }

    private string RenameFileMethod(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(dir + rtn))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + idx.ToString();
            else rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }


    protected void RadAsyncUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {
        if (e.IsValid && e.File != null)
        {
            //#if DEBUG
            //                   @_DestinationPath = @"C:\Temp";
            //#endif
            if (System.Diagnostics.Debugger.IsAttached)
                _DestinationPath = @"C:\Temp\";

            string newFileName = Helper.CleanFilePath(e.File.GetName());
            if (e.File.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
            {
                lblErrorMessages.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                    String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
                return;
            }
            byte[] fileBytes = EncryptedFileBytes(e.File);
            newFileName = RenameFileMethod(@_DestinationPath, newFileName);      // Rename the file
            bool isSplchr = IsSpecialCharacter(newFileName);
            if (!isSplchr)
            {
                newFileName = Regex.Replace(newFileName, @"[^\w\.]+", "");
            }
            File.WriteAllBytes(Path.Combine(@_DestinationPath, newFileName), fileBytes);

            LblFileName.Text = Helper.HtmlEncode(newFileName);
            if(RemovePermissions)
                LnkButtonDelete.Visible = true;
            LnkButtonDownload.Visible = true;
            FileName = newFileName;

            string stepTxt = Registration.GetStepText(Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep));
            //if (this.WorkflowPage.RegistrationStep == 44 && stepTxt == "Agreements")
            //{
            //    SectionName = stepTxt;
            //}
            if (this.WorkflowPage.RegistrationStep == 50 && stepTxt == "Appeals")
            {
                SectionName = "Appeals";

            }
            //SAVE DOC TO TABLE
            
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("REG_PAGE_TYPE_ID", Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep).ToString());
            if (this.WorkflowPage.RegistrationStep == 17 && string.IsNullOrEmpty(SectionName) && stepTxt == CON.RegistrationPageName.ApplicationFee)
                parms.Add("REG_PAGE_SECTION", CON.RegistrationPageName.ApplicationFee.Replace(" ", ""));
            else
            parms.Add("REG_PAGE_SECTION", SectionName);
            parms.Add("SCREENING_ACTIVITY_ID", null);
            parms.Add("NAME", newFileName);
            parms.Add("DESCRIPTION", Description);
            parms.Add("FILE_NAME", newFileName);
            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
            parms.Add("REG_SECTION_UPLOAD_CONTROL_ID", ID);
            parms.Add("ROW_ID", RowId.ToString());
           
            DataRow row = Registration.GetRegistration(this.WorkflowPage.RegistrationId);
            DataSet dsUploadFiles = svc.SelectRegSectionUploadDocument(Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep), this.WorkflowPage.RegistrationId, Helper.GetInt("PROVIDER_TYPE_ID", row), SectionName, RowId);

            //Make a list of ID's for which file has been uploaded, those records needs to be upated.
            List<string> IDWithFile = new List<string>();
            string docID = "";
            if (Helper.HasRows(dsUploadFiles))
            {
                DataTable dtSectionControl = dsUploadFiles.Tables[0];

                foreach (DataRow dr in dtSectionControl.Rows)
                {
                    if (ID == (Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr)))
                    {
                        IDWithFile.Add(Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr));
                        docID = Helper.GetString("DOCUMENT_ID", dr);
                    }
                }
            }

            //if row exists with REG_SECTION_UPLOAD_CONTROL_ID update FileName

            if ((IDWithFile.Count > 0) && (IDWithFile.Contains(ID)))
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();

                parms1.Add("DOCUMENT_ID", docID);
                parms1.Add("NAME", newFileName);
                parms1.Add("DESCRIPTION", Description);
                parms1.Add("FILE_NAME", newFileName);
                parms1.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
                parms1.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                svc.UpdateRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationId), "DOCUMENT", parms1);
            }
            else
            {
                docID = Convert.ToString(svc.InsertRegistrationData(Convert.ToInt32(this.WorkflowPage.RegistrationStep), "DOCUMENT", parms));
            }

            SendToCMS(System.Convert.ToInt32(docID), fileBytes, newFileName);
            FileName = newFileName;
            DocumentId = int.Parse(docID);
            RadAsyncUpload1.Enabled = false;

        }
    }

    protected void OnFileRemove(object sender, EventArgs e)
    {
        try
        {

            RadAsyncUpload1.Enabled = true;
#if DEBUG
                   _DestinationPath = @"C:\Temp";
#endif
            // OHPNM-3601 - Do not delete the document from the file system as it might be needed for audit purposes
            //var filepath = string.Format("{0}\\{1}", _DestinationPath , Helper.CleanFilePath(LblFileName.Text));
            //File.Delete(filepath);
            svc.DeleteRegistrationDocument(DocumentId);
            if (RemoveEvent != null)
            {
                RemoveEvent();
            }
            LblFileName.Text = "";
            LnkButtonDelete.Visible = false;
            LnkButtonDownload.Visible = false;
            FileName = null;

            // they just deleted the file, it really doesn't have the concept of an ID anymore
            DocumentId = 0;
        }
        catch (Exception ex)
        {
            lblErrorMessages.Text = "an error has occurred during the download operation: " + Helper.HtmlEncode(ex.Message);
        }

    }

    protected void OnFileDownload(object sender, EventArgs e)
    {
        DownloadFile(LblFileName.Text);
    }

    //OHPNM-14997 - Archive NOD on POC from their upload section control to Generic Upload control to allow them to upload another NOD or POC
    protected void OnFileArchive(object sender, EventArgs e)
    {        
        svc.ArchiveDocumentToGenericUploadControl(this.WorkflowPage.RegistrationId, DocumentId, "SiteVisit", "Site Visit Review", "Workflow Steps");
        if (RemoveEvent != null)
        {
            RemoveEvent();
        }
        LblFileName.Text = "";
        LnkButtonDelete.Visible = false;
        LnkButtonDownload.Visible = false;
        LnkButtonArchive.Visible = false;
        FileName = null;
        DocumentId = 0;

        UserControls_UploadDocument ucUploadDocument = LoadControl("~/PopupControls/UploadDocument.ascx") as UserControls_UploadDocument;
        ucUploadDocument.LoadData(this.WorkflowPage.RegistrationId, this.WorkflowPage.RegistrationStep);
    }

    public bool ValidateData(string validationGroup)
    {
        bool isGood = true;
        var Nodes = this.WorkflowPage.RegistrationNodes;

        int pagerequired = 0;
        int pagerequiredW9 = 0;

        var otherDocs = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.OtherDocuments)
                                .Select(d => d.Value.IsRequired);
        var otherDocsW9 = Nodes.Where(s => s.Value.Step == CON.SectionTypeID.W9Form)
                                .Select(d => d.Value.IsRequired);
        if (otherDocs.Any())
        {
            pagerequired = otherDocs.Max();
        }
        if (otherDocsW9.Any())
        {
            pagerequiredW9 = otherDocsW9.Max();
        }
        if (IsRequired)
        {
            // EDV: For validation, we should jsut look at DocumentId
            // as this is persisted in viewstate across postbacks. FileName
            // variable is not. I am leaving it as is for now
            if (string.IsNullOrEmpty(FileName) && DocumentId == 0 && (pagerequired == 1 || pagerequiredW9 == 1))
            {
                CustomValidator val = new CustomValidator();
                CustomValidatorUpload.IsValid = false;
                CustomValidatorUpload.ErrorMessage = "* Please upload file: " + this.Title;
                CustomValidatorUpload.ValidationGroup = validationGroup;
                isGood = false;
            }
        }

        return isGood;
    }

    private void DownloadFile(string fileName)
    {
        try
        {
            string filePath = string.Empty;

            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName);
#if DEBUG
                   @filePath = Path.Combine(@"C:\Temp", fileName);
#endif


            // If isFileLocal is false, that means file is on onbase. 
            bool isFileLocal = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString));

            if (isFileLocal && !File.Exists(filePath))
            {
#if DEBUG
                    lblErrorMessages.Text = "File '" + filePath + "' does not exist";
#endif

                lblErrorMessages.Text = "File or directory does not exist.";
                return;
            }

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath, DocumentId);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);            
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

        }
        catch (ThreadAbortException)
        { }
        catch (SqlException ex)
        {
            // OHPNM-4137
            if (ex.Message.Contains("Subquery returned more than 1 value"))
            {
                lblErrorMessages.Text = "test data in the database where duplicate filenames exist is causing an error; 'Remove' your file and upload it with a different name";
            }
            else
            {
                lblErrorMessages.Text = "a database error has occurred during the download operation";
            }
        }
        catch (Exception ex)
        {
#if DEBUG
                lblErrorMessages.Text =  ex.Message;
#endif

            lblErrorMessages.Text = "an error has occurred during the download operation";
        }

    }
    
    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(this.WorkflowPage.RegistrationId, docID, fileBytes, fileName);
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
