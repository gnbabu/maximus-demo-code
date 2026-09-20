using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Services;
using System.Data.SqlClient;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Web.UI;
using System.Text;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;

public partial class PopupControls_UploadAttachments : BaseSectionControl
{

    private const string SENDER_ID = "MMISODJFS";
    private const string ATTACHMENT_FILE_SUFFIX = "AT";

    #region spa
    private PDMSService.PDMSServiceClient _spa;
    private const string Malicious_Files = "The file upload is in process  - please remain on this page to see results.";
    private const string Processed_Files = "Below file(s) are queued to send to destination payer.";


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
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if(AppSettings.Get("EnableCR739PCR1").ToLower() == "true")
        {
            txtComments.Visible = true;
            divcomments.Visible = true;
        }
        else
        {
            txtComments.Visible = false;
            divcomments.Visible = false;
        }
        if (!Page.IsPostBack) {
            UUID = Guid.NewGuid().ToString();
        }
        else
        {
            if (Request.Cookies["selectedOption"] != null && Request.Cookies["selectedOption"].Value != null && Request.Cookies["selectedOption"].Value != "10015") return;
            string eventTarget = Request["__EVENTTARGET"];
            string eventArgument = Request["__EVENTARGUMENT"];
            ProcessPostBack(eventTarget, eventArgument);
        }

        if (ddlTransactionTypeID.SelectedIndex == 0)
        {
            pnlSepMaliciousAttachmentsInfo.Visible = false;
            pnlMaliciousAttachments.Visible = false;
        }
        else
        {
            pnlSepMaliciousAttachmentsInfo.Visible = true;
            pnlMaliciousAttachments.Visible = true;
        }

        try
        {
            HandleServerCallBack();
            if (ddlTransactionTypeID.Items.Count<1)
            {
                GetTransactionType();
                //LoadDestinationPayer();
                GetDocumentTypes("");
            }
            if (!IsPostBack)
            {
                
                GetAttachments();
                for (int i = 0; i < gvAttachment.Rows.Count; i++)
                {

                    string documentId = gvAttachment.DataKeys[i]["outbound_document_uploads_id"].ToString();
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, int.Parse(documentId), true));
                    DataAccess.ExecuteStoredProcedure("usp_DeleteUnsentOutboundDocumentUpload", param);

                }
                GetAttachments();
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void Page_UnLoad(object sender, EventArgs e)
    {

    }

    public void LoadDestinationPayer()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlPrimaryDestinationPayer.Items.Clear();
        DataSet dataSet = _spa.LoadDestinationPayer();
        DataTable dt = dataSet.Tables[0];
        Helper.LoadList(ddlPrimaryDestinationPayer, dt, "DESTINATION_PAYER_DESC", "DESTINATION_PAYER_ID", true);
    }

    protected void ddlPrimaryDestinationPayer_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDestinatinationPayerID();
        tradingPatnerID.Value = GetTradingPatnerID();
    }
    public void LoadDestinatinationPayerID()
    {
        if (ddlPrimaryDestinationPayer.SelectedIndex > 0)
        {
            if(ddlTransactionTypeID.Items.Count < 1)
            {
                GetTransactionType();
            }
            string transactionType = ddlTransactionTypeID.SelectedItem.Text;
            string PrimaryDestinationPayer = ddlPrimaryDestinationPayer.SelectedIndex.ToString();
            string DestinationPayer = PrimaryDestinationPayer == "0" ? "1" : PrimaryDestinationPayer;
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("DestinationPayerMapID", DestinationPayer);
            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();
            }
            if (transactionType.Equals("Prior Auth"))
            {
                if (ddlPrimaryDestinationPayer.Items.Count > 0)
                {
                    DataSet dataSet = _spa.GetDestinationPayerIds(ddlPrimaryDestinationPayer.SelectedIndex);
                    if (dataSet != null)
                    {
                        DataTable dt = dataSet.Tables[0];
                        bool insertSelect = true;
                        if (dt != null && dt.Rows.Count == 1)
                        {
                            insertSelect = false;
                        }
                        DataTable selectedTable = dt.AsEnumerable()
                                .Where(r => r.Field<bool>("IsUploadAttachment") == true)
                                .CopyToDataTable();
                        ddlDestinationPayerID.Items.Clear();
                        Helper.LoadList(ddlDestinationPayerID, selectedTable, "DESTINATION_PAYER_DESC", "Code", insertSelect);
                    }
                }
            }
            else
            {
                DataSet dataSet = _spa.SelectPanelsData("DestinationPayerID", parms);
                DataTable dt = dataSet.Tables[0];
                bool insertSelect = true;
                if (dt != null && dt.Rows.Count == 1)
                {
                    insertSelect = false;
                }
                ddlDestinationPayerID.Items.Clear();
                Helper.LoadList(ddlDestinationPayerID, dt, "DESTINATION_PAYER_DESC", "Code", insertSelect);
            }

            ddlDestinationPayerID.Enabled = true;
        }

    }

    private void HandleServerCallBack()
    {
        if (!string.IsNullOrEmpty(Request.QueryString["payloadData"]))
        {
            string payloadData = Request.QueryString["payloadData"];
            btnSend.Enabled = true;
            btnClear.Enabled = true;
            PayloadData data = JsonConvert.DeserializeObject<PayloadData>(payloadData);
            DateTime expiryTime = DateTime.Now.AddMinutes(2);
            ProcessAttachments processAttachments = new ProcessAttachments();
            var preSignedUrl = processAttachments.generatePreSignedUrl(data.FileName, data.ContentType, expiryTime);
            string responseJson = "{preSignedUrl: '" + preSignedUrl + "'}";
            //Wrap a call to CallBack function with the JSON string as parameter.
            responseJson = string.Format("{0}({1});", Request.QueryString["callback"], responseJson);
            //Send the Response in JSON format to Client.
            Response.ContentType = "text/json";
            Response.Write(responseJson);
            Response.End();
        }
    }

    private void ProcessPostBack(string target, string argument)
    {
        if (target == "btnAdd" && argument == "addAttachment")
        {
            lblAttachmentErrorMsg.Visible = false;
            PersistAttachment();
            GetAttachments();
            UUID = Guid.NewGuid().ToString();
        }
    }


    public override void LoadControlData()
    {
    }
    public void LoadControlData(string icn, string PANumber, string MemberID,int TransactionTypeId)
    {
        txtPANumber.Text = PANumber;
        txtICN.Text = icn;
        txtRecipientID.Text = MemberID;
        txtPANumber.Enabled = false;
        txtICN.Enabled = false;
        if (ddlPrimaryDestinationPayer.Items.Count == 0)
            LoadDestinationPayer();
        ddlPrimaryDestinationPayer.SelectedIndex = 1;
        ddlPrimaryDestinationPayer.Enabled = false;
        LoadDestinatinationPayerID();
        tradingPatnerID.Value = GetTradingPatnerID();

        if (ddlDestinationPayerID.Items.Count > 1)
            ddlDestinationPayerID.SelectedIndex = 1;
        else
            ddlDestinationPayerID.SelectedIndex = 0;


        ddlDestinationPayerID.Enabled = false;

        if (TransactionTypeId == (int)CON.BillingServiceTransactionType.PriorAuth)
        {
            divPANumber.Visible = true;
            divICN.Visible = false;
        }
        else
        {
            divICN.Visible = true;
            divPANumber.Visible = false;
        }
        txtRecipientID.Enabled = false;
        if (ddlTransactionTypeID.Items.Count == 0)
        {
            GetTransactionType();
        }
        ddlTransactionTypeID.SelectedValue = TransactionTypeId.ToString();
        ddlTransactionTypeID.Enabled = false;

        if (ddlPriorAuthDocType.SelectedItem == null || ddlPriorAuthDocType.SelectedItem.Value == string.Empty)
        {
            GetDocumentTypes(ddlTransactionTypeID.SelectedItem.Text);
        }
    }

    public override bool SaveData()
    {
        return true;
    }

    public override void LoadData(DataRow row = null)
    {
    }

    public override bool ValidateData()
    {
        bool isValid = true;
        return isValid;
    }

    public override string Title
    {
        get { return "Upload Attachments"; }
    }

    public override string IdText
    {
        get { return "ucUploadDocuments_" + this.WorkflowPage.RegistrationId; }
    }
    public override string ValidationGroup
    {
        get { return "valProviderInfoHeader"; }
    }

    public string AWSEndPoint
    {
        get {return ConfigurationManager.AppSettings["AWSEndpointUrl"];}
    }


    public string AttachmentFileName
    {
        get {
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
        get {
            //string medicaidNumber = string.Empty;

            //if (HttpContext.Current.Request.QueryString.AllKeys.Contains("MedicaidNumber"))
            //    medicaidNumber = HttpContext.Current.Request.QueryString["MedicaidNumber"];
            //return medicaidNumber;
            return this.WorkflowPage.MedicaidID;
        }
    }

    private string generateDocumentNumber()
    {
        string providerNPI = string.Empty;

        DataSet ds = spa.SelectProviderByGRPMedicaidID(MedicaidNumber);
        DataTable dtMisc = Helper.HasRows(ds) ? ds.Tables[0] : null;
        if (Helper.HasRows(dtMisc))
        {
            DataRow dr = dtMisc.Rows[0];
            providerNPI = Helper.GetString("NPI", dr);
        }
        return providerNPI + DateTime.Now.ToString(" yyyy-mm-dd hh:mm:ss");
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

    public void PersistAttachment()
    {
        NameValueCollection attachment = PopulateAttachment();
        SaveAttachment(attachment);

        //ContinueWaitingUntilScanCompletes(attachment.Get("DocumentName"));

    }


    #region Private Methods

    private void GetTransactionType()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        ddlTransactionTypeID.Items.Clear();
        ddlTransactionTypeID.Items.Add("");
        DataSet dataSet = _spa.GetClaimTransactionType();
        DataTable dt = dataSet.Tables[0];
        ddlTransactionTypeID.DataSource = dt;
        ddlTransactionTypeID.DataValueField = "BILLING_SERVICE_TRANSACTION_TYPE_ID";
        ddlTransactionTypeID.DataTextField = "BILLING_SERVICE_TRANSACTION_TYPE_DESC";
        ddlTransactionTypeID.DataBind();
        
    }
    private void GetDocumentTypes(string transType = "")
    {

        if (!string.IsNullOrEmpty(transType))
        {

            if (_spa == null)
            {
                _spa = new PDMSService.PDMSServiceClient();

            }
            int transactionType = ddlTransactionTypeID.Items != null && ddlTransactionTypeID.Items.Count > 0 ? Convert.ToInt32(ddlTransactionTypeID.SelectedItem.Value) : 0;

            ddlPriorAuthDocType.Items.Clear();
            DataSet dataSet = _spa.GetClaimDocumentTypeByClaimTransactionType(transactionType);
            DataTable dt = dataSet.Tables[0];
            dt.DefaultView.Sort = "DOCUMENT_TYPE_DESC";
            ddlPriorAuthDocType.DataSource = dt;
            ddlPriorAuthDocType.DataValueField = "DOCUMENT_TYPE_ID";
            ddlPriorAuthDocType.DataTextField = "DOCUMENT_TYPE_DESC";
            ddlPriorAuthDocType.DataBind();
            ddlPriorAuthDocType.Items.Insert(0, string.Empty);            
        }
        else
        {
            ddlPriorAuthDocType.Items.Clear();
        }
    }

    protected void ddlTransactionTypeID_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDestinationPayer();
        ddlDestinationPayerID.Items.Clear();
        GetDocumentTypes(ddlTransactionTypeID.SelectedItem.Text);
        if (ddlTransactionTypeID.SelectedItem.Text.Equals("Prior Auth"))
        {
            divPANumber.Visible = true;
            divICN.Visible = false;
        }
        else
        {
            divICN.Visible = true;
            divPANumber.Visible = false;
        }
        GetAttachments();
        if (ddlTransactionTypeID.SelectedIndex > 0)
        {
            pnlSepMaliciousAttachmentsInfo.Visible = true;
            pnlMaliciousAttachments.Visible = true;

            MaliciousAttachments.MedicaidId = this.WorkflowPage.MedicaidID;
            MaliciousAttachments.ClaimType = ddlTransactionTypeID.SelectedIndex.ToString();
            MaliciousAttachments.ClaimTypeCode = "UA";
            MaliciousAttachments.RefreshGrid();
        }
        else
        {
            MaliciousAttachments.ClearGrid();
        }
    }


    private void GetAttachments()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        string MedicaidNumber = this.WorkflowPage.MedicaidID;
        string icn = txtICN.Text;
        string pa_number = txtPANumber.Text;
        DataSet ds = _spa.GetUploadAttachmentsByMedicaid(MedicaidNumber,icn,pa_number);
        if (Helper.HasRows(ds))
        {
            gvAttachment.DataSource = ds.Tables[0];
            gvAttachment.DataBind();
            if (ds.Tables[0].Rows.Count >= 10)
            {
                pnlFileUpload.Visible = false;
            }
            else
            {
                pnlFileUpload.Visible = true;
            }
            btnSend.Visible = true;
            btnClear.Visible = true;
        }
        else
        {
            gvAttachment.DataSource = null;
            gvAttachment.DataBind();

            pnlFileUpload.Visible = true;
            btnSend.Visible = false;
            btnClear.Visible = false;
        }
    }

    #endregion

    protected void gvAttachment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (gvAttachment.DataSource != null)
        {
        if (ddlTransactionTypeID.SelectedItem.Text.Equals("Prior Auth") && e.Row.Cells.Count > 1)
        {
            e.Row.Cells[1].Visible = false;
        }
        else if (e.Row.Cells.Count > 1)
        {
            e.Row.Cells[2].Visible = false;
        }
        }
    }

    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DownloadDocument") {
            string fileName = e.CommandArgument.ToString();
            ProcessAttachments attachments = new ProcessAttachments();
            attachments.GetAttachment(fileName);
        }
    }

    protected void btnAttachmentDelete_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "DeleteDocument") { 
            string documentId = e.CommandArgument.ToString();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, int.Parse(documentId), true));
            DataAccess.ExecuteStoredProcedure("usp_DeleteOutboundDocumentUpload", param);
            GetAttachments();
        }
    }

    private string GetTradingPatnerID()
    {
        if (_spa == null)
        {
            _spa = new PDMSService.PDMSServiceClient();
        }
        string tradingPatnerID = String.Empty;
        DataSet dataSet = _spa.GetPayerNames();
        if (dataSet != null)
        {
            DataTable dt = dataSet.Tables[0];
            var destinationPayerRecord = dt.Select("PRIOR_AUTH_DESTINATION_PAYER_ID =" + ddlPrimaryDestinationPayer.SelectedIndex.ToString()).FirstOrDefault();
            tradingPatnerID = destinationPayerRecord != null ? destinationPayerRecord["MCE_ID"].ToString() : string.Empty;
            if (ddlPrimaryDestinationPayer.SelectedItem.Text == "Ohio Department of Medicaid")
            {
                tradingPatnerID = SENDER_ID;
            }
        }

        return tradingPatnerID;
    }

    private NameValueCollection PopulateAttachment()
    {
        string tradingPatnerID = GetTradingPatnerID();
        string fileName = String.Format("{0}{1}--{2}--{3}{4}", AttachmentFileName, hdnTime.Value, AttachmentFileNameSuffix, tradingPatnerID, Path.GetExtension(UploadAttachments.FileName));
        string receiverID = !string.IsNullOrWhiteSpace(ddlDestinationPayerID.SelectedItem.Text) && ddlDestinationPayerID.SelectedItem.Text.Length > 0 ?
            ddlDestinationPayerID.SelectedItem.Value : string.Empty;

        NameValueCollection attachment = new NameValueCollection();
        if (ddlTransactionTypeID.SelectedItem.Text == "Prior Auth")
        {
            attachment.Add("EDITransaction_Type_ID", "2");
            attachment.Add("PA_NUMBER", txtPANumber.Text);
        }
        else
        {
            attachment.Add("EDITransaction_Type_ID", "1");
        }
        attachment.Add("PayerRequested", "Yes");
        attachment.Add("Member_ID", txtRecipientID.Text);
        attachment.Add("Claim_Type_id", ddlTransactionTypeID.SelectedItem.Value);
        attachment.Add("claim_number", txtICN.Text);
        attachment.Add("provider_id", this.WorkflowPage.MedicaidID);
        attachment.Add("sender_id", SENDER_ID);
        attachment.Add("receiver_id", receiverID);
        attachment.Add("document_type_id", ddlPriorAuthDocType.SelectedItem.Value);
        attachment.Add("documentname", fileName);
        attachment.Add("UUID", UUID);
        attachment.Add("OrginalDocumentName", UploadAttachments.FileName);
        attachment.Add("Provider_NPI",this.WorkflowPage.NPI);

        attachment.Add("to_send", "false");
        attachment.Add("last_modified_date_time", DateTime.Now.ToString());
        attachment.Add("receiver_code", ddlPrimaryDestinationPayer.SelectedItem.Value);
        attachment.Add("OutboundIdentifier", generateDocumentNumber());
        attachment.Add("ProviderComments",txtComments.Text);
        return attachment;
    }
    private void SaveAttachment(NameValueCollection nvc)
    {
        PDMSService.PDMSServiceClient psc = new PDMSService.PDMSServiceClient();
        psc.InsertOutBoundDocumentUploads(Convert.ToInt32(nvc.Get("EDITransaction_Type_ID")), nvc.Get("PayerRequested"),
        nvc.Get("Member_ID"), Convert.ToInt32(nvc.Get("Claim_Type_ID")), nvc.Get("Claim_number"), nvc.Get("PA_NUMBER"), nvc.Get("Provider_ID"), nvc.Get("Provider_NPI"), nvc.Get("Sender_ID"),
        nvc.Get("Receiver_ID"), Convert.ToInt32(nvc.Get("DOCUMENT_TYPE_id")), nvc.Get("DocumentName"), new Guid(nvc.Get("UUID")), Convert.ToBoolean(nvc.Get("TO_SEND")), new Guid(CON.appAdminUserId), nvc.Get("OutboundIdentifier"), nvc.Get("OrginalDocumentName"),
        nvc.Get("ProviderComments"));
        clear();
    }
    
   
    private void clear()
    {
        
        ddlPriorAuthDocType.SelectedIndex = -1;
        ddlPriorAuthDocType.ClearSelection();
        txtComments.Text = "";
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        ProcessAttachments processAttachments = new ProcessAttachments();
        //processAttachments.ProcessUploadedAttachments(MedicaidNumber);
        StringBuilder errorMessage = new StringBuilder();
        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("TO_SEND", "0"));
        parms.Add(new SqlParameter("MEDICAID_NUMBER", MedicaidNumber));
        DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectAllAttachementsToBeSent", parms, "ds");

        for (int i = 0; i < gvAttachment.Rows.Count; i++)
        {

            string documentId = gvAttachment.DataKeys[i]["outbound_document_uploads_id"].ToString();

            processAttachments.UpdateFileStatusToSend(Convert.ToInt32(documentId), 0);
        }

        /*if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsProv.Tables[0].Rows)
            {
                processAttachments.UpdateFileStatusToSend(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]), Convert.ToInt32(dr["PASSTHROUGH_TRANSACTIONQUEUE_ID"]));
            }
        }*/
                GetAttachments();
        /*var maliciousFileFound = processAttachments.UploadedFileNames.Where(c => c.isMalicious == true).Any();
        lblAttachmentErrorMsg.Visible = maliciousFileFound;

        if (maliciousFileFound)
        {
            lblAttachmentErrorMsg.Text = getFileList(processAttachments.UploadedFileNames);
        }
        else
        {*/
            registerScript();
        /*}*/
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        for (int i = 0; i < gvAttachment.Rows.Count; i++) {
            
        string documentId = gvAttachment.DataKeys[i]["outbound_document_uploads_id"].ToString();
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, int.Parse(documentId), true));
            DataAccess.ExecuteStoredProcedure("usp_DeleteOutboundDocumentUpload", param);
            
        }
        GetAttachments();

        if (ddlTransactionTypeID.SelectedItem.Text == "Prior Auth")
        {
            Response.Redirect("~/Process/SearchPriorAuthorization.aspx");
        }
        else 
        {
            Response.Redirect("~/Process/SearchClaims.aspx");
        }

    }
    private string getFileList(IList<ProcessedDocument> UploadedFileNames) 
    {
        string processedFileList = Processed_Files;
        string maliciousFileList = Malicious_Files;

        if (UploadedFileNames.Where(c => c.isMalicious == false).Any())
        {
            processedFileList = string.Format("<p style='color:green;'> {0} </p><ul>", processedFileList);
            foreach (var item in UploadedFileNames.Where(c => c.isMalicious == false))
            {
                processedFileList = string.Format("{0}<li>{1}</li>", processedFileList, item.orginalFileName);
            }
            processedFileList = string.Format("{0} </ul>", processedFileList);
        }

        //Malicious file exists
        if (UploadedFileNames.Where(c => c.isMalicious == true).Any())
        {
            maliciousFileList = string.Format("<p style='color:red;'> {0} </p><ul>", maliciousFileList);
            foreach (var item in UploadedFileNames.Where(c => c.isMalicious == true))
            {
                maliciousFileList = string.Format("{0}<li>{1}</li>", maliciousFileList, item.orginalFileName);
            }
            maliciousFileList = string.Format("{0} </ul>", maliciousFileList);
        }


        return string.Format("<p>{0}</p> <p>{1}</p>", processedFileList, maliciousFileList);
    }

    private void registerScript()
    {
        string alertScript = "setTimeout(function() { alert('Documents have been uploaded successfully');}, 1000);";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString() , alertScript, true);
    }
}

public class PayloadData
{
    public string FileName;
    public string ContentType;
}