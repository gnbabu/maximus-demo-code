using MAXIMUS.DataExchange.PDMS;
using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
public partial class UserControls_UploadFile : System.Web.UI.UserControl
{
   
    public delegate int SuccessEventHandler(string fileName);
    public event SuccessEventHandler SuccessEvent;
    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;
    public delegate void FileDeleteEventHandler();
    public event FileDeleteEventHandler FileDeleteSuccessEvent;
    public delegate void FailEventHandler();
    public event FailEventHandler FailEvent;
    public delegate void ValidateEventHandler(ref string errMsg);
    public event ValidateEventHandler ValidateEvent;
    protected string _DestinationPath;

    public string DestinationPath
    {
        get { return _DestinationPath; }
        set { _DestinationPath = value; }
    }


    public string ValidFileExtensions
    {
        get { return ViewState["_ValidFileExtensions"] == null ? null : ViewState["_ValidFileExtensions"].ToString(); }
        set
        {
            ViewState["_ValidFileExtensions"] = value;
        }
    }

    public int MaxFileMegaBytes
    {
        get { return (ViewState["_MaxFileMegaBytes"] == null || Convert.ToInt32(ViewState["_MaxFileMegaBytes"]) == 0) ? 80 : Convert.ToInt32(ViewState["_MaxFileMegaBytes"]); }
        set
        {
            ViewState["_MaxFileMegaBytes"] = value;
        }
    }

    public bool SetUploadButtonEnabled
    {
        set { UploadButton.Enabled = value; }
    }

    public string UpdateButtonText
    {
        set { UploadButton.Text = value; }
    }

    public string DocumentName
    {
        get { return string.IsNullOrWhiteSpace(txtName.Text) ? Helper.CleanFilePath(filUploadFile.FileName) : Helper.CleanFilePath(txtName.Text); }
    }

    public string DocumentDescription
    {
        get { return txtDescription.Text; }
    }

    public int RedirectToPage
    {
        get { return (ViewState["_RedirectToPage"] == null || Convert.ToInt32(ViewState["_RedirectToPage"]) == 0) ? 80 : Convert.ToInt32(ViewState["_RedirectToPage"]); }
        set
        {
            ViewState["_RedirectToPage"] = value;       //1 -- Affiliate Update, 2- Agent Bulk Upload
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        lblStatusMsg.Text = string.Empty;
        lblErrorMsg.Text = string.Empty;
    }

    private bool IsValidExtension(out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = Helper.CleanFilePath(filUploadFile.PostedFile.FileName);

        // extarct and store the file extension into another variable
        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        // string type array having list of allowed file type extensions
        string[] validFileExtensions = ValidFileExtensions.Split(',');
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
        lblErrorMsg.Text = string.Empty;

        if (filUploadFile.PostedFile == null)
        {
            lblErrorMsg.Text = "Unable to find posted file.";
            goto Failure;
        }
        if (string.IsNullOrWhiteSpace(filUploadFile.PostedFile.FileName))
        {
            lblErrorMsg.Text = "Select a file for upload.";
            goto Failure;
        }
        if (string.IsNullOrWhiteSpace(DestinationPath))
        {
            lblErrorMsg.Text = "ERROR - DestinationPath not defined. Contact system administrator.";
            goto Failure;
        }
        if (string.IsNullOrWhiteSpace(ValidFileExtensions))
        {
            lblErrorMsg.Text = "ERROR - ValidFileExtensions must have a value. Contact system administrator.";
            goto Failure;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg))
        {
            lblErrorMsg.Text = errMsg;
            goto Failure;
        }
        if (!filUploadFile.HasFile)
        {
            lblErrorMsg.Text = "No File has been uploaded.";
            goto Failure;
        }
        if (filUploadFile.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblErrorMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            goto Failure;
        }

        if (!IsSpecialCharacter(filUploadFile.FileName))
        {
            lblErrorMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(filUploadFile.FileName) + " Please remove the Special Character from the file Name before upload. ";
            goto Failure;
        }
        if (ValidateEvent != null)
        {
            ValidateEvent(ref errMsg);
            if (!string.IsNullOrWhiteSpace(errMsg))
            {
                lblErrorMsg.Text = Helper.HtmlEncode(errMsg);
                goto Failure;
            }
        }
        try
        {
            if (System.Diagnostics.Debugger.IsAttached)
                DestinationPath = @"C:\Temp\";

            if (!Directory.Exists(DestinationPath))
            {
                Directory.CreateDirectory(DestinationPath);
            }
            string fileName = Helper.CleanFilePath(filUploadFile.FileName);
            byte[] fileBytes = filUploadFile.EncryptedFileBytes;

            string newFileName = RenameFileMethod(DestinationPath, fileName);

            //filUploadFile.SaveAs(@_DestinationPath + newFileName);
            File.WriteAllBytes(Path.Combine(@DestinationPath, newFileName), fileBytes);
            
            string fileDescp = txtDescription.Text.Trim();

            int docID =  SaveUploadedFile(fileName, newFileName, fileDescp);            
            if (docID != 0)
            {

                SendToCMS(docID, fileBytes, newFileName);
                lblStatusMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
                btnCancel_Click(sender, e);
                if (RedirectToPage == 1)
                {
                    Response.Redirect("~/Process/AffiliateUpdate.aspx");
                }
                else if (RedirectToPage == 2)
                {
                    Response.Redirect("~/Process/AgentBulkUpload.aspx");
                }
            }
            
            txtDescription.Text = string.Empty;
            return;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
            txtDescription.Text = string.Empty;
        }
    Failure:
        if (FailEvent != null) FailEvent();
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

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(docID, fileBytes, fileName);
    }



    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelEvent != null)
            CancelEvent();
        ClearControls();
    }

    private void ClearControls()
    {
        lblStatusMsg.Text = string.Empty;
        lblErrorMsg.Text = string.Empty;
        txtName.Text = string.Empty;
        txtDescription.Text = string.Empty;
    }

    private int SaveUploadedFile(string fileName, string newFileName, string fileDescription)
    {
        int docID = 0;
        try
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            Guid userID = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid lastModifiedUser = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            Guid createdBy = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            DateTime lastModifiedDate = DateTime.Now;
            DateTime createdDate = DateTime.Now;
            int status = CON.DelegateDocumentUploadStatus.Processing;
            if (RedirectToPage == 1)    // From Affiliate Update
            {
                docID = psc.InsertDelegateDocument(userID, fileName, newFileName, fileDescription, status, lastModifiedUser, lastModifiedDate, createdBy, createdDate);
            }
            else if (RedirectToPage == 2)   // From Agent Bulk Upload
            {
                docID = psc.InsertBulkAgentDocument(userID, fileName, newFileName, fileDescription, status, lastModifiedUser, lastModifiedDate, createdBy, createdDate);
            }
            return docID;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "File entries not saved. Error: " + Helper.HtmlEncode(ex.Message);
            return docID;
        }

    }

    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrWhiteSpace(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

}