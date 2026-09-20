using Corp.Core.Libraries.Interface;
using Corp.Core.Libraries.Proxy;
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
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;

public partial class UserControls_OtherDocUploadSectionControl : BaseSectionControl
{
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }

    public int RegEVVTrainingID
    {
        get
        {
            return ViewState["RegEVVTrainingID"] == null ? 0 : Convert.ToInt32(ViewState["RegEVVTrainingID"]);
        }
        set
        {
            ViewState["RegEVVTrainingID"] = value;
        }
    }

    private RadButton _btUpload = new RadButton();
    private RadAsyncUpload _fiInput = new RadAsyncUpload();
    private Label _lbl = new Label();
    private RadAjaxManagerProxy managerproxy = new RadAjaxManagerProxy();
    private RadAjaxManager _ajaxManager = new RadAjaxManager();

    protected string _DestinationPath;
    protected string _ValidFileExtensions;
    protected int _MaxFileMegaBytes;
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
            if (FileName == null)
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
    private const string sectionName = "OtherDocuments";

    public string UploadDocumentInfo { get; set; }

    public bool IsRequired { get; set; }

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

        CustomValidatorUpload.ErrorMessage = "";
        if (!this.IsPostBack)
        {
            if (FileName != null)
            {
                var filepath = DestinationPath + @"\" + FileName;
                LblFileName.Text = Helper.HtmlEncode(FileName);
                LnkButtonDelete.Visible = true;
                LnkButtonDownload.Visible = true;
            }
            else
            {
                LblFileName.Text = null;
                LnkButtonDelete.Visible = false;
                LnkButtonDownload.Visible = false;
                RadAsyncUpload1.Enabled = true;
            }
        }
        else
        {
            if (FileName != null)
            {
                var filepath = DestinationPath + @"\" + FileName;
                LblFileName.Text = Helper.HtmlEncode(FileName);
                LnkButtonDelete.Visible = true;
                LnkButtonDownload.Visible = true;
            }
            else
            {
                LblFileName.Text = null;
                LnkButtonDelete.Visible = false;
                LnkButtonDownload.Visible = false;
                RadAsyncUpload1.Enabled = true;
            }
        }

        ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
        RadAsyncUpload1.AllowedFileExtensions = ValidFileExtensions.Split(',');
        //RadAsyncUpload1.Enabled = true;

        LnkButtonDelete.Click -= OnFileRemove;
        LnkButtonDelete.Click += OnFileRemove;

        LnkButtonDownload.Click -= OnFileDownload;
        LnkButtonDownload.Click += OnFileDownload;
        if (this.WorkflowPage.CurrentTaskName != CON.RegistrationTaskName.ProviderDataEntry || (!Helper.IsUserInProviderRoles(HttpContext.Current.User.Identity.Name) && !Helper.IsUserInOperatorRolls(HttpContext.Current.User.Identity.Name)))
        {
            LnkButtonDelete.Visible = false;
        }

        // OHPNM-1917
        if (inMaintenance(this.WorkflowPage.RegistrationId))
        {
            Helper.SetReadOnly(this, true, "formFieldReadOnly");
        }
       
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
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
    protected override void OnLoad(EventArgs e)
    {
        LoadControlData();
        base.OnLoad(e);
        Page.Title = "Required Documents";
    }
    public override void LoadControlData()
    {
        sepEVV.Visible = pnlEVVTraining.Visible = CanViewEVV(this.WorkflowPage.RegistrationId);
        sepLLI.Visible = pnlLimitedLiabilityInsurance.Visible = CanViewLLInsurance(this.WorkflowPage.RegistrationId);
        if(pnlEVVTraining.Visible)
        {
            LoadEVVTraining();
        }
        if(pnlLimitedLiabilityInsurance.Visible)
        {
            LoadLimitedLiabilityInsurance();
        }
    }

    public override bool SaveData()
    {
        if(pnlEVVTraining.Visible)
        {
            Page.Validate("valEVVTraining");
            for (int i = 0; i < Page.Validators.Count; i++)
            {
                BaseValidator v;
                try
                {
                    v = Page.Validators[i] as BaseValidator;
                    if (v != null && v.ValidationGroup.Equals("valEVVTraining") && !v.IsValid)
                        return false;
                }
                catch
                {
                    continue;
                }
            }
            SaveEVVTraining();
        }
        return true;
    }

    public override bool HasInputValue()
    {
        bool rtn = false;
        if (this.WorkflowPage.RegistrationNodes[this.WorkflowPage.RegistrationStep].IsRequired == 0)
        {
            rtn = true; // OHPNM-2580 - tell Registration.aspx.cs that this page is ok if it's not required, otherwise you can't get the green checkmark
        }

        return rtn;
    }

    public override void LoadData(DataRow row)
    {
    }

    string ucUploadDocument1_RenameFileMethod(string dir, string input)
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
#if DEBUG
                   @_DestinationPath = @"C:\Temp";
#endif

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

            //RadAsyncUpload1.Enabled = false;
            LblFileName.Text = Helper.HtmlEncode(newFileName);
            LnkButtonDelete.Visible = true;
            LnkButtonDownload.Visible = true;
            FileName = newFileName;

            //SAVE DOC TO TABLE
            string stepTxt = Registration.GetStepText(Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep));
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());
            parms.Add("REG_PAGE_TYPE_ID", Registration.GetPageIDFromSectionID(this.WorkflowPage.RegistrationStep).ToString());
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
            var filepath = _DestinationPath + @"\" + Helper.CleanFilePath(LblFileName.Text);
            File.Delete(filepath);
            svc.DeleteRegistrationDocument(DocumentId);
            LblFileName.Text = "";
            LnkButtonDelete.Visible = false;
            LnkButtonDownload.Visible = false;
            FileName = null;
        }
        catch (Exception ex)
        {
            lblErrorMessages.Text = "an error has occurred during the download operation: " + Helper.HtmlEncode(ex.Message);
        }

    }

    protected void OnFileDownload(object sender, EventArgs e)
    {


        DownloadFile(LblFileName.Text);

        return;


    }

    public override bool ValidateData()
    {
        bool isGood = true;
        if (IsRequired)
        {
            // EDV: For validation, we should jsut look at DocumentId
            // as this is persisted in viewstate across postbacks. FileName
            // variable is not. I am leaving it as is for now
            if (FileName == null && DocumentId == 0)
            {
                CustomValidator val = new CustomValidator();
                CustomValidatorUpload.IsValid = false;
                CustomValidatorUpload.ErrorMessage = "* Please upload file: " + this.Title;
                CustomValidatorUpload.ValidationGroup = "valUpload";
                this.Page.Validators.Add(CustomValidatorUpload);
                isGood = false;
            }
        }
        if (PlaceholderUploadEVVTraining.Visible)
        {
            foreach (Control ctrl in PlaceholderUploadEVVTraining.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isGood &= uploadControl.ValidateData("valEVVTraining");
            }
        }
        if (PlaceholderUploadLimitedLiabilityInsurance.Visible)
        {
            foreach (Control ctrl in PlaceholderUploadLimitedLiabilityInsurance.Controls)
            {
                UserControls_UploadSectionControl uploadControl = (UserControls_UploadSectionControl)ctrl;
                isGood &= uploadControl.ValidateData("valLimitedLiablityInsurance");
            }
        }
        if(pnlEVVTraining.Visible && rblEVVTrainingCompleted.SelectedValue != "True")
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Your application cannot be completed until this section is satisfied with a Yes response.";
            val.ValidationGroup = "valEVVTraining";
            this.Page.Validators.Add(val);
            isGood = false;
            return isGood;
        }
        if (pnlEVVTraining.Visible && !Helper.IsValidDate(txtDateOfEVVTraining.Text,true))
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Select a valid Date for EVV Training.";
            val.ValidationGroup = "valEVVTraining";
            this.Page.Validators.Add(val);
            isGood = false;
            return isGood;
        }
        if (pnlLimitedLiabilityInsurance.Visible && rblLimitedLiabilityInsurance.SelectedValue != "True")
        {
            CustomValidator val = new CustomValidator();
            val.IsValid = false;
            val.ErrorMessage = "* Your application cannot be completed until this section is satisfied with a Yes response.";
            val.ValidationGroup = "valLimitedLiablityInsurance";
            this.Page.Validators.Add(val);
            isGood = false;
            return isGood;
        }

        return isGood;
    }

    private void DownloadFile(string fileName, bool isDiddReferral)
    {

        DownloadRequest downloadRequest = new DownloadRequest();
        downloadRequest.FileName = fileName;
        downloadRequest.IsDiddReferral = isDiddReferral;
        var filepath = _DestinationPath + @"\" + fileName;

        FileTransferServiceClient client = new FileTransferServiceClient();

        using (var fileStream = client.DownloadFile(downloadRequest).FileByteStream)
        {
            SendBinaryResponseToClient(fileStream, "attachment;filename=" + fileName, "application/pdf");
        }

    }

    private void DownloadFile(string fileName)
    {
        try
        {
            string filePath = string.Empty;

            fileName = Helper.CleanFilePath(fileName).Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty), fileName);
#if DEBUG
                   @filePath = @"C:\Temp";
#endif

            //Local dev Test
            //filePath = @"C:\Projects\Upload\" + fileName;

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


            //filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName;
            //FilePath = @"C:\Projects\PDMS2_0_0\PDMS\PDMS\FileStoreLocal\" + fileName;

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath);

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
        catch (Exception ex)
        {
            lblErrorMessages.Text = "an error has occurred during the download operation. Error message: " + ex.Message;
        }
    }

    private void SendBinaryResponseToClient(Stream response, string contentHeader, string contentType)
    {
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.Buffer = true;
        Response.ContentType = contentType;
        Response.AddHeader("Content-Disposition", contentHeader);
        response.CopyTo(Response.OutputStream);

        HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
        // Technically, I should be doing Response.End(), but due to a bug in ASP.NET 
        // we tried doing comlete request http://support.microsoft.com/kb/312629/en-us
        // but complete request is putting all the page up there. So, we are swallowing the
        // thread abort exception here.
        // Response.End();
        try
        {
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }
        catch (Exception ex)
        {
            CoreException.ThrowException(ex);
        }
        finally
        {
            HttpContext.Current.Response.SuppressContent = true;
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(this.WorkflowPage.RegistrationId, docID, fileBytes, fileName);
    }

    public override string ValidationGroup
    {
        get { return "valUpload"; }
    }

    public override string Title
    {
        get { return "Other Documents"; }
    }

    public override string IdText
    {
        get { return "ucOtherDocument_" + this.WorkflowPage.RegistrationId; }
    }

    private void SaveEVVTraining()
    {
        Dictionary<string, string> parms = new Dictionary<string, string>();
        parms.Add("REG_ID", this.WorkflowPage.RegistrationId.ToString());




        

            parms.Add("TRAINING_COMPLETED", rblEVVTrainingCompleted.SelectedValue.ToString());
        
        

            parms.Add("TRAINING_COMPLETED_DATE", txtDateOfEVVTraining.Text.ToString());
        

        

        parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
        parms.Add("LAST_MODIFIED_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

        PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
        if (RegEVVTrainingID == 0)
        {
            parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
            parms.Add("CREATED_BY_USER", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

            RegEVVTrainingID = svc.InsertRegistrationDataTable("EVV_TRAINING", parms);
        }
        else
        {
            parms.Add("REG_EVV_TRAINING_ID", RegEVVTrainingID.ToString());
            svc.UpdateRegistrationDataTable("EVV_TRAINING", parms);
        }
        foreach (UserControls_UploadSectionControl uploadDoc in PlaceholderUploadEVVTraining.Controls)
        {
            svc.UpateRegDocumentXref(this.WorkflowPage.RegistrationId, uploadDoc.DocumentId, RegEVVTrainingID);
        }
    }
    private void LoadEVVTraining()
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        DataSet ds = psc.SelectRegistrationData(this.WorkflowPage.RegistrationId, "EVV_TRAINING");
        DataTable dtEVVTraining = Helper.HasRows(ds) ? ds.Tables["RegistrationData"] : null;
        this.DataList = dtEVVTraining;
        
        if (Helper.HasRows(ds))
        {
            RegEVVTrainingID = Helper.GetInt("REG_EVV_TRAINING_ID", dtEVVTraining.Rows[0]);
            bool trainingCompleted = Helper.GetBool("TRAINING_COMPLETED", dtEVVTraining.Rows[0]);
            rblEVVTrainingCompleted.SelectedValue = trainingCompleted ? "True": "False";
            txtDateOfEVVTraining.Text = Helper.GetDate("TRAINING_COMPLETED_DATE", dtEVVTraining.Rows[0]);
            
        }
    }
    private void LoadLimitedLiabilityInsurance()
    {
        LoadPlaceHolderLLInsurance(true, false, "OtherDocuments");
    }



    public void LoadPlaceHolderLLInsurance( bool isEdit = true, bool loadViewState = false, string pageSection = sectionName)
    {
        Upload upload = new Upload();
        PlaceholderUploadLimitedLiabilityInsurance.Controls.Clear();
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
                string titleControl = Helper.GetString("TITLE", dr);
                ucUploadSectionControl.Title = titleControl;

                ucUploadSectionControl.Description = Helper.GetString("DESCRIPTION", dr);

                ucUploadSectionControl.ID = Helper.GetString("REG_SECTION_UPLOAD_CONTROL_ID", dr);

                if ((ds.Tables[i].Columns.Contains("DOCUMENT_ID")))
                {
                    ucUploadSectionControl.DocumentId = Helper.GetInt("DOCUMENT_ID", dr);
                    rblLimitedLiabilityInsurance.SelectedValue = "True";
                }
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
                PlaceholderUploadLimitedLiabilityInsurance.Controls.Add(ucUploadSectionControl);
            }
        }
    }
    private bool CanViewEVV(int regID)
    {
        bool isGood = false;
        int controlID = 0;
        controlID = svc.GetRegSectionUploadControlIDByRegID(this.WorkflowPage.RegistrationId, "OtherDocuments", "EVV Training");
        //Upload upload = new Upload();
        //int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        //DataSet ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, "EVVTraining", false);
        if (controlID > 0)  //if (Helper.HasRows(ds))
        {
            if(Registration.CanProviderSpecialtyViewEVV(regID))
            {
                isGood = true;
            }
        }
        return isGood;
    }
    private bool CanViewLLInsurance(int regID)
    {
        bool isGood = false;
        int controlID = 0;
        controlID = svc.GetRegSectionUploadControlIDByRegID(this.WorkflowPage.RegistrationId, "LimitedLiabilityInsurance", "Limited Liability Insurance");
        //Upload upload = new Upload();
        //int pageTypeID = Registration.GetRegPageTypeId(this.WorkflowPage.RegistrationStep);
        //DataSet ds = upload.LoadUserSectionControlEdit(this.WorkflowPage.RegistrationId, this.WorkflowPage.ApplicationTypeID, this.WorkflowPage.EntityTypeID, this.WorkflowPage.ProviderTypeID, pageTypeID, 0, "LimitedLiabilityInsurance", false);
        if (controlID > 0)  //if (Helper.HasRows(ds))
        {
            isGood = true;
        }
        return isGood;
    }
	protected void rblEVVTrainingCompleted_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		if (pnlEVVTraining.Visible && rblEVVTrainingCompleted.SelectedValue != "True")
		{
			CustomValidator val = new CustomValidator();
			val.IsValid = false;
			val.ErrorMessage = "* Your application cannot be completed until this section is satisfied with a Yes response.";
			val.ValidationGroup = "valEVVTraining";
			this.Page.Validators.Add(val);
			
		}
		else
		{
			CustomValidator val = new CustomValidator();
			val.IsValid = true;
			val.ErrorMessage = " ";
			val.ValidationGroup = "valEVVTraining";
			this.Page.Validators.Add(val);
			
		}

	}
}
