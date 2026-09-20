using Corp.Core.Libraries;
using DocumentFormat.OpenXml.Spreadsheet;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MAXIMUS.Core.Libraries;
using Microsoft.Web.Services3.Addressing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

public partial class Controls_GlobalAdminChange : System.Web.UI.UserControl
{
    // Events to communicate with parent page
    public event EventHandler AdminChanged;

    // Store validation results to avoid multiple database calls
    private DataSet _validationResults;
    private bool _validationPerformed = false;

    private string _DestinationPath;
    private string _ValidFileExtensions;
    private int _MaxFileMegaBytes;

    private bool IsPowerAgent
    {
        get
        {
            return Helper.IsUserPowerAgent(Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), SessionVarRetriever.SelectedProviderAdminUserID, 0);
        }
    }

    public string DestinationPath
    {
        get
        {
            if (string.IsNullOrEmpty(_DestinationPath))
            {
                _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
            }
            return _DestinationPath;
        }
        set { _DestinationPath = value; }
    }

    public string ValidFileExtensions
    {
        get
        {
            if (string.IsNullOrEmpty(_ValidFileExtensions))
            {
                _ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
            }
            return _ValidFileExtensions;
        }
        set { _ValidFileExtensions = value; }
    }

    public int MaxFileMegaBytes
    {
        get
        {
            if (_MaxFileMegaBytes == 0) _MaxFileMegaBytes = 5; // Default to 5MB
            return _MaxFileMegaBytes;
        }
        set
        {
            _MaxFileMegaBytes = value;
            if (_MaxFileMegaBytes > 80) _MaxFileMegaBytes = 80; // Keep the 80MB upper limit
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Initialize control
            InitializeFileUploadSettings();
            List<ListItem> items = new List<ListItem>();

            if (IsPowerAgent)
            {
                string powerAgentId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                items = new List<ListItem>
                {
                    new ListItem("Select OHID", "0"),
                    new ListItem(Helper.GetUserName(SessionVarRetriever.SelectedProviderAdminUserID), SessionVarRetriever.SelectedProviderAdminUserID)
                };
            }
            else
            {
                string adminUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();
                items = new List<ListItem>
                {
                    new ListItem("Select OHID", "0"),
                    new ListItem(Helper.GetUserName(adminUserId), adminUserId)
                };

            }
            ddlProviderAdmins.DataSource = items;
            ddlProviderAdmins.DataTextField = "Text";
            ddlProviderAdmins.DataValueField = "Value";
            ddlProviderAdmins.DataBind();
        }
        lblMessage.Visible = false;
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        mpeConfirmPowerAgent.Hide();
        mpeUpload.Show();
    }
    protected void btnCancelMpe_Click(object sender, EventArgs e)
    {
        mpeConfirmPowerAgent.Hide();
    }

    protected void btnUploadChange_Click(object sender, EventArgs e)
    {
        PerformValidationIfNeeded();

        if (_validationResults == null || _validationResults.Tables.Count == 0)
        {
            lblValidationMessage.Visible = true;
            lblValidationMessage.Text = "Validation error occurred. Please try again.";
            return;
        }

        if (Helper.HasRows(_validationResults))
        {
            DataRow row = _validationResults.Tables[0].Rows[0];
            bool isValid = Convert.ToBoolean(row["IsValid"]);
            string errorCode = row["ErrorCode"].ToString();
            string errorMessage = row["ErrorMessage"].ToString();

            if (!isValid)
            {
                lblValidationMessage.Visible = true;
                // Map database error messages to UI requirements
                switch (errorCode)
                {
                    case "OHID_NOT_EXISTS":
                        lblValidationMessage.Text = "OH ID does not exist";
                        break;
                    case "INVALID_ROLE":
                    case "NOT_POWER_AGENT":
                        lblValidationMessage.Text = "OH ID is not a provider admin or provider agent with power agent role";
                        break;
                    case "OHID_EMPTY":
                        lblValidationMessage.Text = "New OH ID is required";
                        break;
                    case "SAME_OHID":
                        lblValidationMessage.Text = "Current and New OH ID cannot be the same.";
                        break;
                    case "YES_POWER_AGENT":
                        mpeConfirmPowerAgent.Show();
                        break;
                    default:
                        lblValidationMessage.Text = string.IsNullOrEmpty(errorMessage) ?
                            "New OH ID validation failed" : errorMessage;
                        break;
                }
            }
            else   //success
            {
                // Show upload modal
                mpeUpload.Show();
            }
        }
        else
        {
            lblValidationMessage.Visible = true;
            lblValidationMessage.Text = "Validation error occurred. Please try again.";
        }

    }
    private void ValidateAndUploadGlobalAdminFile()
    {
        lblFileInfo.Text = string.Empty;

        // Check if file was posted
        if (fuAdminChangeForm.PostedFile == null)
        {
            lblFileInfo.Text = "Unable to find posted file.";
            return;
        }

        // Check if filename is empty
        if (string.IsNullOrEmpty(fuAdminChangeForm.PostedFile.FileName))
        {
            lblFileInfo.Text = "Select a file for upload.";
            return;
        }

        // Check destination path
        if (string.IsNullOrEmpty(DestinationPath))
        {
            lblFileInfo.Text = "ERROR - DestinationPath must have a value.";
            return;
        }

        // Check valid extensions
        if (string.IsNullOrEmpty(ValidFileExtensions))
        {
            lblFileInfo.Text = "ERROR - ValidFileExtensions must have a value.";
            return;
        }

        // Validate file extension
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg))
        {
            lblFileInfo.Text = Helper.HtmlEncode(errMsg);
            return;
        }

        // Check if file has content
        if (!fuAdminChangeForm.HasFile)
        {
            lblFileInfo.Text = "No File has been uploaded.";
            return;
        }

        // Check file size - cannot be 0KB
        if (fuAdminChangeForm.PostedFile.ContentLength < 1)
        {
            lblFileInfo.Text = "File cannot be 0Kb.";
            return;
        }

        // Check maximum file size
        if (fuAdminChangeForm.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblFileInfo.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) +
                              " bytes (" + String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            return;
        }

        // Check for special characters in filename
        if (!IsSpecialCharacter(fuAdminChangeForm.FileName))
        {
            lblFileInfo.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " +
                              Helper.HtmlEncode(fuAdminChangeForm.FileName) +
                              " Please remove the Special Character from the file Name before upload.";
            return;
        }

        try
        {
            // 1) Canonicalize base directory and ensure it exists
            string baseDir = DestinationPath;

            // Dev override (still isolated)
            if (System.Diagnostics.Debugger.IsAttached)
            {
                baseDir = @"C:\Temp\OH_PNM_Uploads";
            }

            if (string.IsNullOrWhiteSpace(baseDir))
            {
                lblFileInfo.Text = "Upload storage not configured.";
                return;
            }

            baseDir = Path.GetFullPath(baseDir.Trim());
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }

            // 2) Extract client filename only for display/metadata (never for path)
            string clientName = fuAdminChangeForm.PostedFile.FileName ?? string.Empty;
            string originalBaseName = Path.GetFileName(clientName);
            string safeDisplayName = SanitizeFileName(originalBaseName);

            // 3) Enforce allowlisted extensions derived from ValidFileExtensions
            HashSet<string> allowedExts = ParseAllowedExtensions(ValidFileExtensions);
            string ext = Path.GetExtension(safeDisplayName);
            if (!string.IsNullOrEmpty(ext))
            {
                ext = ext.ToLowerInvariant();
            }

            if (string.IsNullOrEmpty(ext) || !allowedExts.Contains(ext))
            {
                lblFileInfo.Text = "Only the following file types are allowed: " + string.Join(", ", allowedExts.ToArray());
                return;
            }

            // 4) Generate a server-side filename and enforce containment
            string storedFileName = Guid.NewGuid().ToString("N") + ext;
            string combined = Path.Combine(baseDir, storedFileName);
            string finalPath = Path.GetFullPath(combined);

            if (!IsUnderBaseDirectory(finalPath, baseDir))
            {
                lblFileInfo.Text = "Invalid upload path.";
                return;
            }

            // 5) Save safely to disk without overwriting
            byte[] fileBytes = fuAdminChangeForm.EncryptedFileBytes; // preserving your data source
            using (FileStream fs = new FileStream(finalPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                fs.Write(fileBytes, 0, fileBytes.Length);
            }

            string name = "Global Admin Change - " + storedFileName;
            string description = "Global Administrator Change Form for admin - " + ddlProviderAdmins.SelectedItem.Text + " to New admin - " + txtNewOHID.Text;

            // Insert document record into database and get the new document ID
            string currentUserId = Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString();

            // Get one registration assigned to the current administrator OHID to insert details into reg_document_xref table
            int registrationId = svc.GetRegistrationIdByUserId(currentUserId, ddlProviderAdmins.SelectedItem.Text);

            // save document details in DB
            int documentId = svc.InsertRegDocument(registrationId, CON.RegistrationPageType.UserMaintenance, "OtherDocuments", name,
                 description, safeDisplayName, currentUserId, 0, string.Empty, string.Empty);

            // send document to OnBase and get OnBase Document ID
            int onBaseDocID = SendToCMS(documentId, fileBytes, storedFileName, currentUserId, registrationId);

            // process the global admin change request.
            svc.ProcessGlobalAdminChangeRequest(ddlProviderAdmins.SelectedItem.Text, txtNewOHID.Text, documentId, onBaseDocID, DateTime.Now, currentUserId);
            // Show success message
            lblFileInfo.Text = "File Uploaded: " + safeDisplayName;

            // SUCCESS: Only now hide the modal
            mpeUpload.Hide();
            ddlProviderAdmins.SelectedValue = "0";
            txtNewOHID.Text = string.Empty;

            lblMessage.Text = "Global admin change successful";
            lblMessage.Visible = true;

            // Raise event
            if (AdminChanged != null)
                AdminChanged(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            lblFileInfo.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }
    }

    //protected void btnDownload_Click(object sender, EventArgs e)
    //{
    //    // Download the form
    //    try
    //    {
    //        string templateActualPath =  AppSettings.Get("PowerAgentTemplateFSXPath");
    //        string fileName = "Global Administrator Change Form 5-12-2025.docx";
    //        string filepath = templateActualPath + fileName;
    //        byte[] file = File.ReadAllBytes(filepath);
    //        HttpContext.Current.Response.Clear();
    //        HttpContext.Current.Response.Buffer = true;
    //        HttpContext.Current.Response.ContentType = "application/force-download";
    //        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + "Global Administrator Change Form 5-12-2025.docx");
    //        HttpContext.Current.Response.BinaryWrite(file);
    //        HttpContext.Current.Response.Flush();
    //        HttpContext.Current.Response.Close();
    //        HttpContext.Current.Response.End();
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle error - could show a message to user
    //        // For now, silently fail
    //    }
    //}

    protected void btnSaveUpload_Click(object sender, EventArgs e)
    {
        // Always show modal first (in case postback closed it)
        mpeUpload.Show();
        // Call the comprehensive validation and upload method
        ValidateAndUploadGlobalAdminFile();
    }

    protected void btnCancelUpload_Click(object sender, EventArgs e)
    {
        mpeUpload.Hide();
    }

    #region Private Helper Methods

    private void PerformValidationIfNeeded()
    {
        if (!_validationPerformed)
        {
            string currentOHID = ddlProviderAdmins.SelectedItem.Text;
            string newOHID = txtNewOHID.Text.Trim();

            try
            {
                // Call the data layer method - replace 'svc' with your actual service reference
                _validationResults = svc.ValidateGlobalAdminChange(currentOHID, newOHID);
                _validationPerformed = true;
            }
            catch (Exception ex)
            {
                // Handle exception - validation will fail
                _validationResults = null;
                _validationPerformed = true;
            }
        }
    }

    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private bool IsValidExtension(out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = Helper.CleanFilePath(fuAdminChangeForm.PostedFile.FileName);

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

    private void InitializeFileUploadSettings()
    {
        // Set file upload configuration
        DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
        ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt"; // Only allow document types
        MaxFileMegaBytes = 5; // 5MB maximum file size
    }

    private string RenameFileIfExists(string dir, string input)
    {
        string rtn = input;
        int idx = 0;
        while (System.IO.File.Exists(Path.Combine(dir, rtn)))
        {
            idx += 1;
            int pos = input.LastIndexOf(".");
            if (pos == -1)
                rtn = input + "_" + idx.ToString();
            else
                rtn = input.Substring(0, pos) + "_" + idx.ToString() + input.Substring(pos);
        }
        return rtn;
    }

    private static string SanitizeFileName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "file";

        // Normalize to reduce Unicode trickery
        name = name.Normalize(NormalizationForm.FormKC);

        // Remove invalid filename chars
        char[] invalid = Path.GetInvalidFileNameChars();
        for (int i = 0; i < invalid.Length; i++)
        {
            name = name.Replace(invalid[i].ToString(), string.Empty);
        }

        // Allowlist characters; collapse whitespace
        name = Regex.Replace(name, @"[^a-zA-Z0-9\.\-_ ]", "");
        name = Regex.Replace(name, @"\s+", " ").Trim();

        if (string.IsNullOrEmpty(name) || name == "." || name == "..")
            name = "file";

        return name;
    }

    private int SendToCMS(int docID, byte[] fileBytes, string fileName, string currentUserId, int regID)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        int onBaseDocID = onBaseInterface.GetSubmitedFileID(regID, docID, fileBytes, fileName);

        return onBaseDocID;
    }
    private static bool IsUnderBaseDirectory(string candidateFullPath, string baseFullPath)
    {
        string basePath = Path.GetFullPath(baseFullPath);
        string sep = Path.DirectorySeparatorChar.ToString();
        if (!basePath.EndsWith(sep))
        {
            basePath = basePath + sep;
        }

        string candidate = Path.GetFullPath(candidateFullPath);
        return candidate.StartsWith(basePath, StringComparison.OrdinalIgnoreCase);
    }

    private static HashSet<string> ParseAllowedExtensions(string validFileExtensions)
    {
        HashSet<string> set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        if (string.IsNullOrWhiteSpace(validFileExtensions)) return set;

        string[] parts = validFileExtensions.Split(new[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            string t = parts[i].Trim().ToLowerInvariant();
            string ext = t.StartsWith(".") ? t : "." + t;
            if (!set.Contains(ext)) set.Add(ext);
        }
        return set;
    }
    #endregion

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
    #endregion
}