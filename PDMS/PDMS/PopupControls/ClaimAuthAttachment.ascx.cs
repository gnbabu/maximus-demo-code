using System;
using System.Data;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Collections.Specialized;
using Corp.Core.Libraries.ClaimsManagement;
using MAXIMUS.Core.Libraries;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using MAXIMUS.DataExchange.PDMS;
using MAXIMUS.Controllers.PDMS;
using System.Data.SqlClient;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using NPOI.Util;
using System.Text;

public partial class PopupControls_ClaimAuthAttachment : BasePopupControl
{
    private const string SENDER_ID = "MMISODJFS";
    private const string ATTACHMENT_FILE_SUFFIX = "AT";
    private string _DestinationPath = Helper.GetAppSettingFromDB("FileStorePath", string.Empty);
    private string _ValidFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,txt,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";
    private const string ATTACHMENT_RETENTION_DATA1 = "ATTACHMENT_RETENTION_DATA1";
    private const string ROWSTATE_ADD = "added";
    private const string ROWSTATE_UPDATE = "updated";
    private const string ROWSTATE_DELETE = "deleted"; 
    private const string Document_Type_1 = "X-Rays"; 
    private const string Document_Type_2 = "Other Prior Authorization Supporting Documentation";
    private const string Document_Type_3 = "Medical Documentation";
    private const string Document_Type_4 = "Pricing Information";
    private const string Auth_Dental = "1";
    private const string Auth_Inst = "2";
    private const string Auth_Prof = "3";
    private string MedicadeId=string.Empty;
    private const string Malicious_Files = "The file upload is in process  - please remain on this page to see results.";
    private const string Processed_Files = "Below file(s) are queued to send to destination payer.";


    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private PDMSService.PDMSServiceClient spa
    {
        get
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            return _spa;
        }
    }
    #endregion
    public WorkflowPage WorkflowPage
    {
        get { return (WorkflowPage)this.Page; }
    }
    private bool _displayReadOnly;
    public bool DisplayReadOnly
    {
        get
        {
            return _displayReadOnly;
        }
        set
        {
            _displayReadOnly = value;            
        }
    }
    public void SetButtonVisibility()
    {
        if (DisplayReadOnly == true)
        {
            pnlAttachAdd.Visible = false;
            btnAddAttachment.Visible = false;
        }
        else
        {
            pnlAttachAdd.Visible = true;
            btnAddAttachment.Visible = true;
        }
    }

    public string ClaimId
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAttachClaimID.Value))
                return hdnAttachClaimID.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAttachClaimID.Value = value.Trim();
        }
    }

    public string DestinationPayor
    {

        get
        {
            if (ViewState["DestinationPayor"] == null) ViewState["DestinationPayor"] = string.Empty;
            return (string)ViewState["DestinationPayor"];
        }
        set
        {
            ViewState["DestinationPayor"] = value;
        }
    }


    public string ReceiverCode
    {
        get
        {
            if (ViewState["ReceiverCode"] == null) ViewState["ReceiverCode"] = string.Empty;
            return (string)ViewState["ReceiverCode"];
        }
        set
        {
            ViewState["ReceiverCode"] = value;
        }
    }

    public string MedicaidId
    { get; set; }
       public string MedicaidBillingNumber
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(MedicadeId))
                return MedicadeId;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                MedicadeId = value;
        }
    }


    public string ClaimType
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(hdnAttachClaimType.Value))
                return hdnAttachClaimType.Value;
            else
                return string.Empty;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
                hdnAttachClaimType.Value = value.Trim();
        }
    }
    private DataSet dsAttachmnets = new DataSet();
    public DataSet ClaimAttachments
    {
        get
        {
            return dsAttachmnets;
        }
        set
        {
            if (value != null && Helper.HasRows(value))
            {
                dsAttachmnets = value;
                BindGrid();
            }
        }
    }

    public string AttachmentFileName
    {
        get
        {
            DateTime dateTime = DateTime.Now;
            return string.Format("{0}--{1}--{2}-", SENDER_ID, UUID, dateTime.ToString("yyyyMMdd"));
        }
    }

    public string AttachmentFileNameSuffix
    {
        get
        {
            return ATTACHMENT_FILE_SUFFIX;
        }
    }

    public string MedicaidNumber
    {
        get
        {
            //string medicaidNumber = string.Empty;

            //if (HttpContext.Current.Request.QueryString.AllKeys.Contains("MedicaidNumber"))
            //    medicaidNumber = HttpContext.Current.Request.QueryString["MedicaidNumber"];
            //return medicaidNumber;
            return this.WorkflowPage.MedicaidID;
        }
    }

    private string UUID
    {
        set { ViewState["UUID"] = value; }
        get { return ViewState["UUID"].ToString(); }
    }

    public string PreSignedUrl
    {
        set { ViewState["PreSignedUrl"] = value; }
        get { return ViewState["PreSignedUrl"].ToString(); }
    }

    public bool setReadOnly
    {
        get; set;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!Page.IsPostBack)
        {            
            Session["SelectedPage"] = "SubmitClaim";
            Session["LoadDocument"] = "False";
            UUID = Guid.NewGuid().ToString();
            BindAttachmentGrid();
        }
        else
        {
            if (Session["LoadDocument"] != null)
            {
                if ((Session["LoadDocument"] != "") && (Session["LoadDocument"] != null) && (Session["SelectedPage"] != "") && (Session["SelectedPage"] != null))
                {
                    if (!string.IsNullOrEmpty(filesize.Value))
                    {
                        double claimfilesize = Convert.ToDouble(filesize.Value.ToString());
                        if (claimfilesize > 0.8)
                        {
                            System.Threading.Thread.Sleep(3000);
                        }
                    }
                    if ((Session["LoadDocument"] == "False") && (Session["SelectedPage"] == "SubmitClaim"))
                    {
                        if (!string.IsNullOrEmpty(filesize.Value))
                        {
                            double claimfilesize = Convert.ToDouble(filesize.Value);
                            if (claimfilesize > 0.8)
                            {
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                        string eventTarget = Request["__EVENTTARGET"];
                        string eventArgument = Request["__EVENTARGUMENT"];
                        ProcessUploadAttachment(eventTarget, eventArgument);
                    }
                }
            }

            BindAttachmentGrid();
        }
        

        string textfile = txtClaimAttachmentName.Text.Trim();
        lblclaimAttachError.Visible = false;
       
        if (UploadAttachments.PostedFile != null && !String.IsNullOrEmpty(UploadAttachments.PostedFile.FileName))
        {
            hdnFile.Value = UploadAttachments.FileName;
        }

        pnlAttachAdd.Visible = !setReadOnly;
       
        if (ddlDocumentTypeclaims.Items.Count == 0)
            GetPriorClaimsDocumentType();
        if (setReadOnly == true)
        {
            Attach.Visible = false;
        }
        else
        {
            Attach.Visible = true;
        }
       
        
    }

    private void ProcessUploadAttachment(string target, string argument)
    {
        if (target == "btnPriorAdd" && argument == "addAttachment")
        {
           
                uploadAttachment();
            UUID = Guid.NewGuid().ToString();
        }
    }

    public override void LoadData(DataRow dr)
    {
        base.LoadData(dr);

        if (!Page.IsPostBack)
            BindGrid();

        if (ddlDocumentTypeclaims.Items.Count == 0)
            GetPriorClaimsDocumentType();


    }

    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadDocument")
        {
            string fileName = e.CommandArgument.ToString();
            ProcessAttachments attachments = new ProcessAttachments();
            attachments.GetAttachment(fileName);
        }
    }

    protected void gvClaimAuthAttachment_RowDataBougvAttachment_RowCommandnd(object sender, GridViewRowEventArgs e)
    {
        if (setReadOnly == true)
        {
            gvClaimAuthAttachment.Columns[5].Visible = false;
            Attach.Visible = false;
        }
    }

    private void uploadAttachment()
    {
        lblInstUploadErrMsg.Text = string.Empty;
        lblInstDocTypeErrMsg.Text = string.Empty;

        if (gvClaimAuthAttachment.Rows.Count == 10)
        {
            lblAttachmentErrorMsg.Visible = true;
            lblAttachmentErrorMsg.Text = "Maximum of 10 attachments can be submitted";
            return;
        }

        if (!UploadAttachments.HasFile)
        {
            lblInstUploadErrMsg.Visible = true;
            lblInstUploadErrMsg.Text = "No file has been uploaded.";
        }
        else
        {
            lblInstUploadErrMsg.Visible = false;
            lblInstUploadErrMsg.Text = "";
        }

       
            lblInstDocTypeErrMsg.Visible = false;
            lblInstDocTypeErrMsg.Text = "";
       

        if (lblInstDocTypeErrMsg.Visible || lblInstUploadErrMsg.Visible)
        {
            return;
        }
        if (UploadAttachments.HasFile)
        {
            lblAttachmentErrorMsg.Text = "";
            var fileBytes = UploadAttachments.FileBytes;
            var docType = ddlDocumentTypeclaims.SelectedValue;

            int documentId = UploadPriorAuthAttachment(lblAttachmentStatusMsg, UploadAttachments, lblAttachmentErrorMsg);
            string destinationPayerId = GetdestinationPayerID();
            string fileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, destinationPayerId, Path.GetExtension(UploadAttachments.FileName));


            if (documentId <= 0)
            {
                lblAttachmentErrorMsg.Visible = true;
                return;
            }

            NameValueCollection attachment = PopulateAttachment();
            SaveAttachment(attachment);

            //ContinueWaitingUntilScanCompletes(attachment.Get("DocumentName"));

            ddlDocumentTypeclaims.SelectedIndex = 0;
            BindAttachmentGrid();
        }
    }
    
    private NameValueCollection PopulateAttachment()
    {
        string MedicaidNum = string.Empty;
        var fileBytes = UploadAttachments.FileBytes;
        var docType = ddlDocumentTypeclaims.SelectedValue;
        if (Request.QueryString.AllKeys.Contains("MedicaidNumber"))
            MedicaidNum = Request.QueryString["MedicaidNumber"];
        else
            MedicaidNum = this.WorkflowPage.MedicaidID;

        //int documentId = UploadPriorAuthAttachment(lblAttachmentStatusMsg, UploadAttachments, lblAttachmentErrorMsg);
        string orgFileName = UploadAttachments.FileName.ToString();
        string destinationPayerId = GetdestinationPayerID();
        string fileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, destinationPayerId, Path.GetExtension(UploadAttachments.FileName));

        // string receiverID = "12345";
        NameValueCollection attachment = new NameValueCollection();
       
        if (ClaimType.ToString() ==CON.ClaimsType.Dental ||
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInWords.Dental.ToUpper() || 
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInitial.Dental.ToUpper() )
        { 
            attachment.Add("EDITransaction_Type_ID", "1");
            attachment.Add("Claim_Type_id", "0");
        }
        if (ClaimType.ToString() == CON.ClaimsType.Institutional ||
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInWords.Institutional.ToUpper() ||
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInitial.Institutional.ToUpper())
        {
            attachment.Add("EDITransaction_Type_ID", "3");
            attachment.Add("Claim_Type_id", "1");
        }
        if (ClaimType.ToString() == CON.ClaimsType.Professional ||
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInWords.Professional.ToUpper() ||
            ClaimType.ToString().ToUpper() == CON.ClaimsTypeInitial.Professional.ToUpper())
        {
            attachment.Add("EDITransaction_Type_ID", "2");
            attachment.Add("Claim_Type_id", "2");
        }
       
        attachment.Add("PayerRequested", "No");
        //*  attachment.Add("Member_ID", txtRecipientID.Text);
        attachment.Add("Member_ID", MedicaidBillingNumber);

        //* attachment.Add("claim_number", txtICN.Text);
       attachment.Add("claim_number", hdnAttachClaimID.Value.ToString());
        attachment.Add("provider_id", MedicaidNumber);
        attachment.Add("sender_id", SENDER_ID);
        attachment.Add("receiver_id", destinationPayerId);            
        attachment.Add("documentname", fileName);
        attachment.Add("UUID", UUID);
        attachment.Add("PA_NUMBER",null);
        attachment.Add("Provider_NPI", this.WorkflowPage.NPI);

        attachment.Add("Document_ID", DocumentID(MedicaidNum));
        attachment.Add("to_send", "false");
        attachment.Add("last_modified_date_time", DateTime.Now.ToString());
      //attachment.Add("receiver_code", ddlPrimaryDestinationPayer.SelectedItem.Value);
        attachment.Add("receiver_code", ReceiverCode);
        attachment.Add("OutboundIdentifier", generateDocumentNumber());
        attachment.Add("document_type_id", gettypenumber());
        attachment.Add("Org_FileName", orgFileName);
        return attachment;
    }
    private void SaveAttachment(NameValueCollection nvc)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertClaimsOutBoundDocumentUploads(Convert.ToInt32(nvc.Get("EDITransaction_Type_ID")), nvc.Get("PayerRequested"),
        nvc.Get("Member_ID"), Convert.ToInt32(nvc.Get("Claim_Type_ID")), nvc.Get("Claim_number"), nvc.Get("PA_NUMBER"), 
        nvc.Get("Provider_ID"), nvc.Get("Provider_NPI"), nvc.Get("Sender_ID"), nvc.Get("Receiver_ID"), 
        Convert.ToInt32(nvc.Get("DOCUMENT_TYPE_id")), nvc.Get("DocumentName"), new Guid(nvc.Get("UUID")), 
        Convert.ToBoolean(nvc.Get("TO_SEND")), new Guid(CON.appAdminUserId), nvc.Get("OutboundIdentifier"), nvc.Get("Org_FileName"), nvc.Get("Document_ID"));
        //clear();
    }


    private int UploadPriorAuthAttachment(Label lblAttStatusMsg, MMSWebControls.EncryptedFileUpload PriorAttUpload, Label lblAttErrMsg)
    {
        lblAttStatusMsg.Text = string.Empty;
        lblAttErrMsg.Text = string.Empty;
        int docID = 1;
        if (PriorAttUpload.PostedFile == null)
        {
            lblAttErrMsg.Text = "Unable to find posted file.";
            docID = 0;

        }
        if (string.IsNullOrWhiteSpace(PriorAttUpload.PostedFile.FileName))
        {
            lblAttErrMsg.Text = "Select a file for upload.";
            docID = 0;
        }
        if (string.IsNullOrWhiteSpace(_DestinationPath))
        {
            lblAttErrMsg.Text = "ERROR - DestinationPath not defined. Contact system administrator.";
            docID = 0;

        }
        if (string.IsNullOrWhiteSpace(_ValidFileExtensions))
        {
            lblAttErrMsg.Text = "ERROR - ValidFileExtensions must have a value. Contact system administrator.";
            docID = 0;
        }
        string errMsg = string.Empty;
        if (!IsValidExtension(PriorAttUpload, out errMsg))
        {
            lblAttErrMsg.Text = errMsg;
            docID = 0;
        }
        if (!PriorAttUpload.HasFile)
        {
            lblAttErrMsg.Text = "No File has been uploaded.";
            docID = 0;

        }

        if (PriorAttUpload.PostedFile.ContentLength < 1)
        {
            lblAttErrMsg.Text = "File cannot be 0Kb.";
            docID = 0;
        }
        if (PriorAttUpload.PostedFile.ContentLength > (MaxFileMegaBytes * (1000 * 1024)))
        {
            lblAttErrMsg.Text = "File cannot be more than " + String.Format("{0:0,0}", (MaxFileMegaBytes * (1000 * 1024))) + " bytes (" +
                String.Format("{0:f}", MaxFileMegaBytes) + " MB) in size.";
            docID = 0;
        }

        if (!IsSpecialCharacter(PriorAttUpload.FileName))
        {
            lblAttErrMsg.Text = "The file name can contain letters, numbers, dot(.), underscore(_) and hyphen(-): " + Helper.HtmlEncode(PriorAttUpload.FileName) + " Please remove the Special Character from the file Name before upload. ";
            docID = 0;
        }
        if (!string.IsNullOrEmpty(lblAttErrMsg.Text))
        {
            return docID;
        }

        /*try
        {
            string fileName = Helper.CleanFilePath(PriorAttUpload.FileName);
            string destinationPayerId = GetdestinationPayerID();
            string newFileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, destinationPayerId, Path.GetExtension(UploadAttachments.FileName));

            byte[] fileBytes =  PriorAttUpload.FileBytes;
          
            docID = SaveUploadedFile(fileName, newFileName);
            if (docID != 0)
            {
                Session["LoadDocument"] = "True";
                SendToCMS(docID, fileBytes, newFileName);
                lblAttStatusMsg.Text = "File Uploaded: " + Helper.HtmlEncode(newFileName);
            }
        }
        catch (Exception ex)
        {
            lblAttErrMsg.Text = "No File uploaded. Error: " + Helper.HtmlEncode(ex.Message);
        }*/
        return docID;
    }

    private void SendToCMS(int docID, byte[] fileBytes, string fileName)
    {
        OnBaseInterface onBaseInterface = new OnBaseInterface();
        onBaseInterface.SubmitFile(docID, fileBytes, fileName);
    }

    private int SaveUploadedFile(string fileName, string newFileName)
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
            docID = psc.InsertDelegateDocument(userID, fileName, newFileName, "", status, lastModifiedUser, lastModifiedDate, createdBy, createdDate);
            return docID;
        }
        catch (Exception ex)
        {
            lblAttachmentStatusMsg.Text = "File entries not saved. Error: " + Helper.HtmlEncode(ex.Message);
            return docID;
        }
    }

    public int MaxFileMegaBytes
    {
        get { return (ViewState["_MaxFileMegaBytes"] == null || Convert.ToInt32(ViewState["_MaxFileMegaBytes"]) == 0) ? 10 : Convert.ToInt32(ViewState["_MaxFileMegaBytes"]); }
        set
        {
            ViewState["_MaxFileMegaBytes"] = value;
        }
    }

    private bool IsValidExtension(MMSWebControls.EncryptedFileUpload PriorAttUpload, out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;

        string fileName = Helper.CleanFilePath(PriorAttUpload.PostedFile.FileName);

        string fileExtension = System.IO.Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();

        string[] validFileExtensions = _ValidFileExtensions.Split(',');
       
        foreach (string extension in validFileExtensions)
        {
            if (fileExtension == extension)
            {
                rtn = true;
                break;
            }
        }

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
        if (string.IsNullOrWhiteSpace(pattern)) return true;                 // TRUE if the configuration setting does not exist
        Regex objAlphaPattern = new Regex(pattern);
        return objAlphaPattern.IsMatch(strFileName);
    }

    private void SaveRecordToPriorAuthAttachment(int documentId, string fileName, string authType, DropDownList ddlPriorAuthDocType)
    {
        try
        {
            Dictionary<string, object> parms = new Dictionary<string, object>();
            var id = Helper.GetUserId(HttpContext.Current.User.Identity.Name);
            parms.Add("CLAIM_DOCUMENT_TYPE_ID", Convert.ToString(ddlPriorAuthDocType.SelectedItem.Value));
            
            parms.Add("CLAIM_DOCUMENT_ID", Convert.ToString(documentId));
            var claimType = authType.ToUpper();
            if (claimType == CON.ClaimsType.Dental)
                parms.Add("CLAIM_ATTACHMENT_AUTH_TYPE", "1");
            else if (claimType == CON.ClaimsType.Institutional)
                parms.Add("CLAIM_ATTACHMENT_AUTH_TYPE", "2");
            else if (claimType ==CON.ClaimsType.Professional)
                parms.Add("CLAIM_ATTACHMENT_AUTH_TYPE", "3");

            parms.Add("CLAIM_DOCUMENT_TYPE_DESC", Convert.ToString(ddlPriorAuthDocType.SelectedItem.Text));
            parms.Add("DOCUMENT_ID", Convert.ToString(documentId));
            parms.Add("DOCUMENT_STATUS", "1");
            parms.Add("Last_Modified_user", id.ToString());
            parms.Add("MedicaidId", this.WorkflowPage.MedicaidID);

            parms.Add("Created_by_user", id.ToString());
            Random random = new Random();
            parms.Add("Claims_Attachment_Screen_ID", random.Next());
            parms.Add("DOCUMENT_NAME", fileName);

            DataSet ds = null;
            if (Session["ATTACHMENT_RETENTION_DATA1"] != null)
            {
                ds = (DataSet)Session[ATTACHMENT_RETENTION_DATA1];
            }
            DataRow dataRowForAdd = ds.Tables[0].NewRow();

            foreach (var key in parms)
            {
                if (key.Value == null)
                    dataRowForAdd[key.Key] = DBNull.Value;

                dataRowForAdd[key.Key] = key.Value;
            }
            dataRowForAdd["RowState"] = ROWSTATE_ADD;

            ds.Tables[0].Rows.Add(dataRowForAdd);
            Session[ATTACHMENT_RETENTION_DATA1] = ds;
        }
        catch (Exception ex)
        {
            throw CoreException.ThrowException(ex);
        }
    }

    private void BindAttachmentGrid()
    {
        string logHeader = string.Format("SubmitPriorAuthorization -> GetAttachment Session");
        string logMsg = String.Format(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

        try
        {
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }

            DataSet ds = null; DataSet dsClaimData = null; DataTable dataTable = null;
            if (!string.IsNullOrEmpty(hdnAttachClaimID.Value))
            {
                string abb = hdnAttachClaimType.Value;
                dsClaimData = ClaimsController.CheckClaimIsDelet(int.Parse(hdnAttachClaimID.Value));
                if (Helper.HasRows(dsClaimData))
                {
                    ds = _spa.GetClaimsUploadAttachmentsByMedicaid(MedicaidNumber, hdnAttachClaimID.Value, hdnAttachClaimType.Value);
                    gvClaimAuthAttachment.DataSource = ds.Tables[0];
                    gvClaimAuthAttachment.DataBind();
                    gvClaimAuthAttachment.Visible = true;
                }
                else
                {
                    gvClaimAuthAttachment.DataSource = null;
                    gvClaimAuthAttachment.DataBind();
                    gvClaimAuthAttachment.Visible = false;
                }

               
            }
        }
        
        
        catch (Exception ex)
        {
            Logging log = new Logging(Guid.NewGuid(), logMsg);
            log.CreateLogEntry(string.Format("{0} {1}", logHeader, ex.ToString()), Logging.LogPriority.Error);
        }

    }

    protected void btnAttachmentDelete_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string documentId = e.CommandArgument.ToString();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, int.Parse(documentId), true));
            DataAccess.ExecuteStoredProcedure("usp_ClaimsDeleteOutboundDocumentUpload", param);
            BindAttachmentGrid();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    private void BindGrid()
    {
        if (Helper.HasRows(ClaimAttachments))
        {
            gvClaimAuthAttachment.DataSource = ClaimAttachments;
            if (dsAttachmnets.Tables[0].Rows.Count >= 10)
            {
                lblclaimAttachError.Text = "Only 10 Records Allowed";
                lblclaimAttachError.Visible = true;
            }
        }
        else
        {
          
            gvClaimAuthAttachment.DataSource = null;
        }        
        gvClaimAuthAttachment.DataBind();
        if (_displayReadOnly) { gvClaimAuthAttachment.Columns[4].Visible = false; }
        else { gvClaimAuthAttachment.Columns[4].Visible = true; }
    }

    public delegate void SaveEventHandler();
    public event SaveEventHandler SaveEvent;

    public delegate void CancelEventHandler();
    public event CancelEventHandler CancelEvent;


    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(this.WorkflowPage.ClaimRecipientId))
            {
                lblclaimAttachError.Text = "Please enter valid recipient information to upload attachments";
                lblclaimAttachError.Visible = true;
                return;
            }
            if (!string.IsNullOrEmpty(hdnFile.Value))
            {
                string filename = hdnFile.Value;
                string errMsg = string.Empty;
                bool isvalidFile = IsValidExtension(filename, out errMsg);

                if (!isvalidFile)
                {
                    lblclaimAttachError.Visible = true;
                    lblclaimAttachError.Text = errMsg;
                }              
                else
                {
                    lblclaimAttachError.Visible = false;
                    lblclaimAttachError.Text = string.Empty;
                    string lineNumber = GetLineNumber();
                    string documentId = generateDocumentNumber();
                    string documentType = ddlDocumentTypeclaims.SelectedValue.ToString();
                    //Add record
                    int transactionType = GetTransactionTypeByClaim(); ;

                    AddRecord(lineNumber, documentId);
                    //send to S3
                    ClaimsAttachmentHelper cat = new ClaimsAttachmentHelper();
                    NameValueCollection nvc = cat.SetUpClaimsNameValueCollection(DestinationPayor, filename, documentType, this.WorkflowPage.ClaimRecipientId, transactionType.ToString(), MedicaidId, documentId, ReceiverCode);
                    string APIUrl = AppSettings.Get("UploadAttachmentWebAPI");
                    if (IsFileVaild() == true)
                    {
                        cat.HttpUploadFile(APIUrl + "/api/upload/UploadAttachment", filename, "file", "multipart/form-data", nvc, UploadAttachments.FileBytes);
                    }
                    else
                    {
                        lblclaimAttachError.Text = "File size must be less than 10MB";
                        lblclaimAttachError.Enabled = true;
                        lblclaimAttachError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
        BindAttachments();
        ClearFeilds();
    }

    public string SendClaimAttachment(int transactionID,string txtMedicaidBillingNumber)
    {
        string returnmaliciousFileFound;
        if (gvClaimAuthAttachment.Rows.Count > 0)
        {
            ProcessAttachments processAttachments = new ProcessAttachments();
            //var errorMessage = processAttachments.ProcessClaimAttachments(MedicaidNumber);

            BindAttachmentGrid();

            /*var maliciousFileFound = processAttachments.UploadedFileNamesClaims.Where(c => c.isMalicious == true).Any();
            lblAttachmentErrorMsg.Visible = maliciousFileFound;

            if (maliciousFileFound)
            {
                lblAttachmentErrorMsg.Text = getFileList(processAttachments.UploadedFileNamesClaims);
                returnmaliciousFileFound = "malicious files found";
            }
            else
            {
                    returnmaliciousFileFound = "malicious files not found";
                }

            }
            else { returnmaliciousFileFound = "No Files are found"; }
            return returnmaliciousFileFound;*/

            /*StringBuilder errorMessage = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("TO_SEND", "0"));
            parms.Add(new SqlParameter("MEDICAID_NUMBER", MedicaidNumber));
            DataSet dsProv = DataAccess.ExecuteStoredProcedure("[usp_SelectClaimsAttachementsToBeSent]", parms, "ds");

            if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsProv.Tables[0].Rows)
            {*/
            for (int i = 0; i < gvClaimAuthAttachment.Rows.Count; i++)
            {
                string outbound_documents_upload_id = gvClaimAuthAttachment.DataKeys[Convert.ToInt32(i)]["OutBound_Document_Uploads_ID"].ToString();
                    processAttachments.UpdateClaimFileStatusToSend(Convert.ToInt32(outbound_documents_upload_id), transactionID,txtMedicaidBillingNumber);
            }
            // }
            //}
        }
        return "";
    }
    private bool IsFileVaild()
    {
        if (UploadAttachments.FileBytes.Length > 10485760) //10 MB limit
        {
            return false;
        }
        return true;
    }
    private bool IsValidExtension(string filename, out string errMsg)
    {
        bool rtn = false;
        errMsg = string.Empty;
        string _ValidFileExtensions = "doc,docx,pdf,xls,XLS,xlsm,xlsx,ppt,pptx,mdi,jpe,zip,jpg,jpeg,png,gif,bmp,tif,tiff,pi,ec,zip,csv,xlsm,msg,acrbak";

        // extarct and store the file extension into another variable
        string[] fileExtensions = filename.Split('.');


        string fileExtension = fileExtensions[1];
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
            errMsg = "Document extension is invalid.";

        }
        return rtn;
    }
    private string GetLineNumber()
    {
        int rows = 0;
        string Linenumber = string.Empty;
        if (gvClaimAuthAttachment != null)
        {
            rows = gvClaimAuthAttachment.Rows.Count;

        }
        Linenumber = (rows + 1).ToString();
        return Linenumber;
    }



    protected void gvClaimAuthAttachment_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvClaimAuthAttachment.PageIndex = e.NewPageIndex;
        BindGrid();
        gvClaimAuthAttachment.EditIndex = -1;
    }

    public void ClearFeilds()
    {

        ddlDocumentTypeclaims.SelectedIndex = 0;
    }
    public void ClearGrid()
    {
        gvClaimAuthAttachment.DataSource = null;
        gvClaimAuthAttachment.DataBind();
    }


    protected void ddlDocumentTypeclaims_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void gvClaimAuthAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ViewUpload")
            {
                string APIUrl = AppSettings.Get("UploadAttachmentWebAPI");
                string url = APIUrl + "/api/upload/GetFile/" + e.CommandArgument.ToString();
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
                wr.ContentType = "Application/JSON";
                wr.Method = "GET";
                wr.KeepAlive = true;
                wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
                WebResponse response = wr.GetResponse();

                MemoryStream ms = new MemoryStream();
                using (Stream responseStream = wr.GetResponse().GetResponseStream())
                {
                    byte[] buffer = new byte[0x1000];
                    int bytes;
                    while ((bytes = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, bytes);
                    }
                }


                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/force-download";
                HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + e.CommandArgument.ToString());



                byte[] decryptedFile = ms.ToArray();//encryptObj.DecryptRijndael(encryptedFile)
                HttpContext.Current.Response.BinaryWrite(decryptedFile);

                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.Close();
                HttpContext.Current.Response.End();
            }

        }
        catch (Exception ex)
        {

        }

    }
    protected void gvClaimAuthAttachment_RowDeleting(object sender, CommandEventArgs e)
    {
        try
        {
            string documentId = e.CommandArgument.ToString();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, int.Parse(documentId), true));
            DataAccess.ExecuteStoredProcedure("usp_ClaimsDeleteOutboundDocumentUpload", param);
            BindAttachmentGrid();

            
        }
        catch (Exception ex)
        {

        }


    }

    public void BindAttachments()
    {
        try
        {
            if (!string.IsNullOrEmpty(ClaimId))
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", ClaimId);
                dsAttachmnets = spa.SelectPanelsData("claims_attachment_screen", parms);
            }
            BindGrid();
            if (setReadOnly == true)
            {
                pnlAttachAdd.Visible = false;
            }
            else
            {
                pnlAttachAdd.Visible = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void GetPriorClaimsDocumentType()
    {
        int transactionType = 0;


        ddlDocumentTypeclaims.Items.Clear();


        if (!string.IsNullOrEmpty(ClaimType))
        {
            transactionType = GetTransactionTypeByClaim();

            DataSet dataSet = spa.GetClaimDocumentTypeByClaimTransactionType(transactionType);
            if (Helper.HasRows(dataSet))
            {
                DataTable dt = dataSet.Tables[0];

                Helper.LoadDropDown(ddlDocumentTypeclaims, dt, "DOCUMENT_TYPE_DESC", "DOCUMENT_TYPE_ID", false);



            }

        }
        if (setReadOnly == true)
        {
            pnlAttachAdd.Visible = false;
        }
        else
        {
            pnlAttachAdd.Visible = true;
        }

    }

    private int GetTransactionTypeByClaim()
    {
        int transactionType = 0;
        if (ClaimType == CON.ClaimsType.Dental)
        {
            transactionType = (int)CON.BillingServiceTransactionType.Dental;
        }
        else if (ClaimType == CON.ClaimsType.Institutional)
        {
            transactionType = (int)CON.BillingServiceTransactionType.Institutional;

        }
        else if (ClaimType == CON.ClaimsType.Professional)
        {
            transactionType = (int)CON.BillingServiceTransactionType.Professional;
        }
        return transactionType;
    }
    private string generateDocumentNumber()
    {

        string providerNPI = string.Empty;

        if (Request.QueryString.AllKeys.Contains("MedicaidNumber"))
            MedicaidId = Request.QueryString["MedicaidNumber"];
        else
            MedicaidId = this.WorkflowPage.MedicaidID;

        DataSet ds = spa.SelectProviderByGRPMedicaidID(MedicaidId);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        this.DataList = dtMisc;
        Guid guidnpi = Guid.NewGuid();
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            providerNPI = Helper.GetString("NPI", dr);
        }

        return providerNPI + " " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"); //guidnpi;

    }

    private string gettypenumber()
    {

        string claimType = null;

        if (ClaimType.ToString() == "0") { claimType = "Dental"; }

        if (ClaimType.ToString() == "1") { claimType = "Institutional"; }

        if (ClaimType.ToString() == "2") { claimType = "Professional"; }
        
        string selectedDocumentType = ddlDocumentTypeclaims.SelectedItem.Text;

        int typeId = spa.SelectClaimAttachmentTypeId(selectedDocumentType.ToString(), claimType.ToString());

       return typeId.ToString();

    }
    private void AddRecord(string line, string number)
    {
        string linenumberrow = null;
        lblclaimAttachError.Visible = false;
        if (!string.IsNullOrEmpty(ClaimId))
        {
            if (dsAttachmnets != null)
            {
                Dictionary<string, string> parms1 = new Dictionary<string, string>();
                parms1.Add("Claim_ID", ClaimId);
                dsAttachmnets = spa.SelectPanelsData("claims_attachment_screen", parms1);
                int rec_cnt = dsAttachmnets.Tables[0].Rows.Count;
                if (rec_cnt > 0)
                {
                    DataRow row = dsAttachmnets.Tables[0].Rows[rec_cnt - 1];
                    linenumberrow = row["Line"].ToString();
                }
            }
            if (dsAttachmnets.Tables[0].Rows.Count >= 10)
            {
                lblclaimAttachError.Text = "Only 10 Records Allowed";
                lblclaimAttachError.Visible = true;
            }
            else
            {
                Dictionary<string, string> parms = new Dictionary<string, string>();
                parms.Add("Claim_ID", ClaimId);
                parms.Add("Line_Item", line);
                parms.Add("Attachment_number", number);
                parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                parms.Add("Created_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());

                if (linenumberrow != line)
                {
                    try
                    {
                        spa.InsertPanelsData("claims_attachment_screen", parms);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                //   return true;
            }
        }
    }


    protected void UploadAttachments_DataBinding(object sender, EventArgs e)
    {
        try
        {
            //if (IsFileVaild() == true)
            //{

            int filesize = UploadAttachments.PostedFile.ContentLength;
            if (filesize > 10485760)
            {

                lblclaimAttachError.Text = "File size exceeds max 10MB limit";
                lblclaimAttachError.Visible = true;
                return;
            }
            else
            {
                hdnFile.Value = UploadAttachments.PostedFile.FileName;
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void Save(DataTable dt)
    {
        if (Helper.HasRows(dt))
        {
            // OHPNM-19074 on claim ReSubmit, will attempt to re-upload sent attachments, only do so if new attachments were provided
            if (dt.Columns.Contains("DocumentId"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, string> parms = new Dictionary<string, string>();
                    parms.Add("Claim_ID", ClaimId);
                    parms.Add("Line_Item", dt.Rows[i]["Line"].ToString());
                    parms.Add("Attachment_number", dt.Rows[i]["DocumentId"].ToString());
                    parms.Add("Last_Modified_Date", DateTime.Now.ToString());
                    parms.Add("Last_Modified_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    parms.Add("Created_Date_Time", DateTime.Now.ToString());
                    parms.Add("Created_By_User", Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString());
                    spa.InsertPanelsData("claims_attachment_screen", parms);
                }
            }
            BindAttachments();
        }
    }

   
    protected string DocumentID(string medicaidNumber)
    {
        string docID = string.Empty;
        if (!string.IsNullOrEmpty(medicaidNumber))
        {
            DataSet ds = spa.SelectProviderByGRPMedicaidID(medicaidNumber);
            DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
            this.DataList = dtMisc;
            if (Helper.HasRows(dtMisc))
            {
               DataRow dr = dtMisc.Rows[0];
               string NPI = Helper.GetString("NPI", dr);
                long epocdatetime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
                 docID = NPI + epocdatetime;
               

            }
        }
        return docID;
    }

    private string GetdestinationPayerID()
    {
       var tradingPartnerIDval = string.Empty;
        if (Session["DestinationPayerIDVal"] != null)
        { 
        tradingPartnerIDval = Session["DestinationPayerIDVal"].ToString();
        
        }
        //Return the PartnerID
        return tradingPartnerIDval;
    }

    private string getFileList(IList<ProcessedDocument> UploadedFileNamesClaims)
    {
        string processedFileList = string.Empty;
        string maliciousFileList = string.Empty;

        if (UploadedFileNamesClaims.Where(c => c.isMalicious == false).Any())
        {
            processedFileList = string.Format("<p style='color:green;'> {0}{1} </p><ul>", Processed_Files, processedFileList);
            foreach (var item in UploadedFileNamesClaims.Where(c => c.isMalicious == false))
            {
                processedFileList = string.Format("{0}<li>{1}</li>", processedFileList, item.orginalFileName);
            }
            processedFileList = string.Format("{0} </ul>", processedFileList);
        }

        //Malicious file exists
        if (UploadedFileNamesClaims.Where(c => c.isMalicious == true).Any())
        {
            maliciousFileList = string.Format("<p style='color:red;'> {0}{1} </p><ul>", Malicious_Files, maliciousFileList);
            foreach (var item in UploadedFileNamesClaims.Where(c => c.isMalicious == true))
            {
                maliciousFileList = string.Format("{0}<li>{1}</li>", maliciousFileList, item.orginalFileName);
            }
            maliciousFileList = string.Format("{0} </ul>", maliciousFileList);
        }


        return string.Format("<p>{0}</p> <p>{1}</p>", processedFileList, maliciousFileList);
    }

    

}