using MAXIMUS.DataExchange.PDMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
public partial class PopupControls_TransferOwnership : System.Web.UI.UserControl
{
    public delegate void UpdateSuccessEventHandler(string message);
    public event UpdateSuccessEventHandler UpdateSuccessEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;

    public delegate void KeepPopupOpenEventHandler();
    public event KeepPopupOpenEventHandler KeepPopupOpenEvent;

    public delegate void RefreshEventHandler();
    public event RefreshEventHandler RefreshEvent;


    public int RegID
    {
        get
        {
            return ViewState["RegID"] == null ? 0 : Convert.ToInt32(ViewState["RegID"]);
        }
        set
        {
            ViewState["RegID"] = value;
        }
    }
    public void LoadData(int regID, string medicaidID, string fromUser)
    {
        RegID = regID;
        lblMedicaidID.Text = medicaidID;
        lblAssignedTo.Text = fromUser;
        
        TabPanel2.Visible = true;
        pnlUploadDocs.Visible = true;
        LoadDocumentData(regID, CON.RegistrationPageType.UserMaintenance, "OtherDocuments");
    }
    
    //protected void tabFile_ActiveTabChanged(object sender, EventArgs e)
    //{
    //    LoadTabData(tabFile.ActiveTabIndex);
    //}
    //private void LoadTabData(int idx)
    //{
    //    switch (idx)
    //    {
    //        case 0:
    //            TabPanel1.Visible = true;
    //            tab1.Style.Add("display", "block");
    //            // TabPanel2.Visible = true;
    //            break;
    //        case 1:
    //            TabPanel1.Visible = true;
    //            tab1.Style.Add("display","none"); 
    //            TabPanel2.Visible = true;
    //            LoadDocumentData(RegID, CON.RegistrationPageType.Agreements, "OtherDocuments");
    //            break;
    //    }
    //    if (KeepPopupOpenEvent != null) KeepPopupOpenEvent();
    //}
    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        tabFile.ActiveTabIndex = 0;
        lblError.Visible = false;
        if (gvUploadedDocs.Rows.Count < 1)
        {
            lblError.Visible = true;
            lblError.Text = "Upload Document is Required.";
            goto Failure;
        }
        if (!string.IsNullOrEmpty(txtTransferto.Text))
        {
            Guid transferTouserID = Helper.GetUserId(txtTransferto.Text);
            if (transferTouserID == null || transferTouserID.ToString() == "00000000-0000-0000-0000-000000000000")
            {
                lblError.Visible = true;
                lblError.Text = "User Not found. Please enter correct userID.";
                goto Failure;
            }
            else
            {
                if (Helper.IsMITSuser(transferTouserID.ToString()))
                {
                    lblError.Visible = true;
                    lblError.Text = "Please enter a user with OH|ID.";
                    goto Failure;
                }
                else
                {
                    if (Helper.IsUserInRole(txtTransferto.Text, CON.UserRole.ProviderAdministrator))
                    {
                        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
                        Dictionary<string, string> parms = new Dictionary<string, string>();
                        psc.TransferRegistrationToNewUser(RegID, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), Helper.GetUserId(txtTransferto.Text).ToString(), false);
                        ClearFields();
                        if (RefreshEvent != null) RefreshEvent();
                        return;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Selected UserID is not a Provider Administrator.";
                        goto Failure;
                    }
                       
                }
            }

        }
        else
        {
            lblError.Visible = true;
            goto Failure;
        }

    Failure:
        if (KeepPopupOpenEvent != null) KeepPopupOpenEvent();
        return;
    }
    private void ClearFields()
    {
        lblError.Visible = false;
        lblError.Text = string.Empty;
        lblStatusMsg.Visible = false;
        lblStatusMsg.Text = string.Empty;
        txtTransferto.Text = string.Empty;
        txtName.Text = string.Empty;
        txtDescription.Text = string.Empty;
    }

    #region UploadDocument
    public static string DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    public static string ValidFileExtensions = "doc,docx,pdf,ppt,rtf,xls,xlsx,bmp,gif,jpg,jpeg,png,tiff,txt";
    public static int MaxFileMegaBytes = 80;

    public void LoadDocumentData(int regId, int regPageTypeId, string regPageSection = "", int? screeningActivityID = null)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        string exclusions = string.Empty;
        exclusions = CON.RegistrationPageType.Certification.ToString() + "," + CON.RegistrationPageType.Contracts.ToString();
        DataSet ds = psc.SelectRegDocuments(regId, regPageTypeId, regPageSection, exclusions, screeningActivityID, SessionVarRetriever.MyQueueSelectedRoleName);
        gvUploadedDocs.DataSource = Helper.HasRows(ds) ? ds : null;
        gvUploadedDocs.DataBind();
        //lblStatusMsg.Visible = false;
    }

    protected void gvUploadedDocs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Delete")
        {
            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            psc.DeleteDocument(Convert.ToInt32(e.CommandArgument));

            if (RegID > 0)
                LoadDocumentData(RegID, CON.RegistrationPageType.UserMaintenance, "OtherDocuments");
        }
    }
    protected void gvUploadedDocs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        if (KeepPopupOpenEvent != null) KeepPopupOpenEvent();
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
            int OnBaseId;
            if (int.TryParse(DataBinder.Eval(e.Row.DataItem, "ONBASE_DOCUMENT_ID").ToString(), out OnBaseId))
            {
                string isEnycrypted = "";
                if (Boolean.Parse(DataBinder.Eval(e.Row.DataItem, "IS_CONVERSION").ToString()) == true) { isEnycrypted = "&UseEncryption=N"; }
                string url = "../ViewFile.aspx?OnBaseId=" + ob.ObfuscateId(OnBaseId) + isEnycrypted;
                img.OnClientClick = "window.open('" + url + "');return false;";
            }
            else
            {
                img.Visible = false;
            }

        }
        img = (ImageButton)e.Row.FindControl("imgCancel");
        if (img != null)
        {
            img.Visible = false;
            if (Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString() == DataBinder.Eval(e.Row.DataItem, "LAST_MODIFIED_USER").ToString())
            {

                //if (DataBinder.Eval(e.Row.DataItem, "WFTaskName").ToString() == "Transfer Ownership")
                //{
                    img.Visible = true;
                //}
            }
            if (img.Visible) img.Attributes.Add("onclick", "javascript:return confirm('Uploaded document will be deleted. Are you sure?');");          
        }
    }
    protected void UploadButton_Click(object sender, EventArgs e)
    {
        tabFile.ActiveTabIndex = 1;
        lblStatusMsg.Visible = true;
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
        if (string.IsNullOrEmpty(DestinationPath))
        {
            lblStatusMsg.Text = "ERROR - DestinationPath must have a value";
            goto Failure;
        }
        if (string.IsNullOrEmpty(ValidFileExtensions))
        {
            lblStatusMsg.Text = "ERROR - ValidFileExtensions must have a value";
            goto Failure;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(out errMsg))
        {
            lblStatusMsg.Text = errMsg;
            goto Failure;
        }
        if (!filUploadFile.HasFile)
        {
            lblStatusMsg.Text = "No File has been uploaded.";
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
            lblStatusMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + filUploadFile.FileName + " Please remove the Special Character from the file Name before upload. ";
            goto Failure;
        }

        try
        {
        
            if (System.Diagnostics.Debugger.IsAttached)
                DestinationPath = @"C:\Temp\";

            //string _txtName = string.Empty;
            //if (!string.IsNullOrEmpty(txtName.Text))
            //{
            //    _txtName = new string(txtName.Text.Except(@".\").ToArray());
            //}

            //string newFileName = string.IsNullOrEmpty(_txtName) ? filUploadFile.FileName : _txtName.Trim();
            string newFileName = filUploadFile.FileName;
            byte[] fileBytes = filUploadFile.EncryptedFileBytes;
            newFileName = RenameFileMethod(@DestinationPath, newFileName);      // Rename the file
            //filUploadFile.SaveAs(@_DestinationPath + newFileName);
          
            File.WriteAllBytes(Path.Combine(@DestinationPath, newFileName), fileBytes);
            lblStatusMsg.Text = "File Uploaded: " + newFileName;

            PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
            int docId = psc.InsertRegDocument(RegID, CON.RegistrationPageType.UserMaintenance, "OtherDocuments", txtName.Text,
                 txtDescription.Text, newFileName, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), 0, string.Empty, string.Empty);

            txtName.Text = "";
            txtDescription.Text = "";

            SendToCMS(docId, fileBytes, newFileName);
            LoadDocumentData(RegID, CON.RegistrationPageType.UserMaintenance, "OtherDocuments");
            if (KeepPopupOpenEvent != null) KeepPopupOpenEvent();
            return;
        }
        catch (Exception ex)
        {
            lblStatusMsg.Text = "No File uploaded. Error: " + ex.Message;
            goto Failure;
        }
    Failure:      
        if (KeepPopupOpenEvent != null) KeepPopupOpenEvent();
        return;
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(RegID, docID, fileBytes, fileName);
    }
    private bool IsValidExtension(out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = filUploadFile.PostedFile.FileName;

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
    private bool IsSpecialCharacter(string strFileName)
    {
        string pattern = Helper.GetAppSettingFromDB("RegexPatternForSpecialCharacter", string.Empty);
        if (string.IsNullOrEmpty(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }
    string RenameFileMethod(string dir, string input)
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
    #endregion
}