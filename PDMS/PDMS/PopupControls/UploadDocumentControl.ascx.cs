using Corp.Core.Libraries.Interface;
using Corp.Core.Libraries.Proxy;
using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControls_UploadDocumentControl : System.Web.UI.UserControl
{
    public delegate int SuccessEventHandler(string fileName);
    public event SuccessEventHandler SuccessEvent;

    private RadButton _btUpload = new RadButton();
    private RadAsyncUpload _fiInput = new RadAsyncUpload();
    private DropDownList _DdDocumentTypes = null;
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

    

    public string UploadDocumentInfo { get; set; }

    public bool IsRequired { get; set; }

    public bool Download { get; set; }

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
        Download = false;
        Session["Download"] = Download;
        uploadLabel.Text = Title;
        uploadInfo.Text = Description;
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
        }

        ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
        RadAsyncUpload1.AllowedFileExtensions = ValidFileExtensions.Split(',');
        //RadAsyncUpload1.Enabled = true;

        LnkButtonDelete.Click -= OnFileRemove;
        LnkButtonDelete.Click += OnFileRemove;

        LnkButtonDownload.Click -= OnFileDownload;
        LnkButtonDownload.Click += OnFileDownload;
        

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }

    protected void CustomValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        string ErrorMsg = "";
        //if (FileName != null)
        //{
        //    RadAsyncUpload1.Enabled = false;
        //    args.IsValid = false;
        //}
        ////args.IsValid = (FileName != null);
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
            
            Dictionary<string, string> parms = new Dictionary<string, string>();
            
            //Make a list of ID's for which file has been uploaded, those records needs to be upated.
            List<string> IDWithFile = new List<string>();
            string docID = "";
            

            //if row exists with REG_SECTION_UPLOAD_CONTROL_ID update FileName

            
                docID = Convert.ToString(svc.InsertUploadDocument(newFileName,"",newFileName,DateTime.Now, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString()));
            

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


        DownloadFile(Helper.CleanFilePath(LblFileName.Text), this.DocumentId);

        return;


    }

    public bool ValidateData(string validationGroup)
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
                CustomValidatorUpload.ValidationGroup = validationGroup;
                this.Page.Validators.Add(CustomValidatorUpload);
                isGood = false;
            }
        }

        return isGood;
    }

    private bool DownloadFile(string fileName, bool isDiddReferral)
    {

        DownloadRequest downloadRequest = new DownloadRequest();
        downloadRequest.FileName = fileName;
        downloadRequest.IsDiddReferral = isDiddReferral;
        var filepath = _DestinationPath + @"\" + Helper.CleanFilePath(fileName);
        //if (File.Exists(filepath))
        //{
        FileTransferServiceClient client = new FileTransferServiceClient();

        using (var fileStream = client.DownloadFile(downloadRequest).FileByteStream)
        {
            SendBinaryResponseToClient(fileStream, "attachment;filename=" + fileName, "application/pdf");
            Download = true;
        }
        //}

        return Download;

    }

    private void DownloadFile(string fileName, int documentID)
    {
        try
        {
            string filePath = string.Empty;

            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            filePath = Path.Combine(Helper.GetAppSettingFromDB("FileStorePath", string.Empty) , fileName);
#if DEBUG
                   @filePath = Path.Combine(@"C:\Temp", fileName);
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
                Session["Download"] = false;
                return;
            }


            //filePath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty) + fileName;
            //FilePath = @"C:\Projects\PDMS2_0_0\PDMS\PDMS\FileStoreLocal\" + fileName;

            bool fileExistsonLocal = System.IO.File.Exists(filePath);
            bool downloadFile = isFileLocal ? fileExistsonLocal : true;

            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] decryptedFile = onBaseInterface.RetrieveFile(filePath, documentID);
            
            
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;            
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            HttpContext.Current.Response.BinaryWrite(decryptedFile);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();

            Session["Download"] = true;

        }
        catch (ThreadAbortException)
        { }
        catch (Exception ex)
        {
#if DEBUG
                lblErrorMessages.Text =  ex.Message;
#endif

            lblErrorMessages.Text = "an error has occurred during the download operation";

            Session["Download"] = false;
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
        onBaseInterface.SubmitFile( docID, fileBytes, fileName);
    }

    public void Loadfile()
    {
        ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
        scriptManager.RegisterPostBackControl(this.LnkButtonDownload);

        DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        Download = false;
        Session["Download"] = Download;
        uploadLabel.Text = Title;
        uploadInfo.Text = Helper.HtmlEncode(Description);
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
        }

        ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
        RadAsyncUpload1.AllowedFileExtensions = ValidFileExtensions.Split(',');
        //RadAsyncUpload1.Enabled = true;

        LnkButtonDelete.Click -= OnFileRemove;
        LnkButtonDelete.Click += OnFileRemove;

        LnkButtonDownload.Click -= OnFileDownload;
        LnkButtonDownload.Click += OnFileDownload;
        
    }
}
