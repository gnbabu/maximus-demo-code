using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using MAXIMUS.DataExchange.PDMS;
using System.Configuration;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Amazon;
using System.Web;
using System.Threading.Tasks;
using System.Linq;

public class ProcessAttachments
{

    private const string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
    private PDMSService.PDMSServiceClient _svc;
    private IAmazonS3 _s3Client;

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
    private string AWSS3WriteBucketName
    {
        get { return AppSettings.Get("AWSS3BucketName"); }
    }

    private string AWSS3ReadBucketName
    {
        get { return AppSettings.Get("AWSS3ReadBucketName"); }
    }

    private string AWSSecretsRegion
    {
        get { return AppSettings.Get("SecretsRegion"); ; }

    }

    private Guid UserId
    {
        get { return new Guid(appPDMSDataExchangeUserId); }
    }

    private RegionEndpoint _AWSRegionEndpoint = RegionEndpoint.USWest2;
    private RegionEndpoint AWSRegionEndpoint
    {
        get { return _AWSRegionEndpoint; }
        set { _AWSRegionEndpoint = value; }
    }

    public IList<ProcessedDocument> UploadedFileNames = new List<ProcessedDocument>();
    public IList<ProcessedDocument> UploadedFileNamesClaims = new List<ProcessedDocument>();


    private Logging log = new Logging();

    public ProcessAttachments()
    {
        var secretsManager = new SecretsManager(AppSettings.Get("SecretsRegion"));
        log.CreateLogEntry(" In ProcessAttachments-  Before getting S3UploadSecrets");
        var result = secretsManager.GetSuperSecretPassword(AppSettings.Get("S3UploadSecrets"));
        result.Wait();
        string AWSAccessKey = result.Result["AWSAccessKey"];
        log.CreateLogEntry("AWSAccessKey " + AWSAccessKey, Logging.LogPriority.Information);
        if (string.IsNullOrEmpty(AWSAccessKey))
        {
            throw new Exception("AWSAccessKey is not found in the secrets manager.");
        }
        string AWSSecretKey = result.Result["AWSSecretKey"];
        if (string.IsNullOrEmpty(AWSSecretKey))
        {
            throw new Exception("AWSSecretKey is not found in the secrets manager.");
        }
        AWSRegionEndpoint = RegionEndpoint.GetBySystemName(AWSSecretsRegion);
        _s3Client = new AmazonS3Client(AWSAccessKey, AWSSecretKey, AWSRegionEndpoint);
    }

    public void ProcessUploadedAttachments(string medicaidNumber)
    {
        StringBuilder errorMessage = new StringBuilder();
        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("TO_SEND", "1"));
        parms.Add(new SqlParameter("MEDICAID_NUMBER", medicaidNumber));
        DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectAllAttachementsToBeSent", parms, "ds");

        if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsProv.Tables[0].Rows)
            {
                Attachments_ControlFile controlFile = PopulateAttachmentControlFile(dr);
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Attachments_ControlFile));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, controlFile, emptyNs);
                string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>" + Environment.NewLine;
                xml = xml + stream2.ToString();
                Guid g = Guid.NewGuid();

                string fileName = System.IO.Path.GetFileNameWithoutExtension(controlFile.DocumentName).Replace("--AT--", "--CF--") + ".xml";
                byte[] title = new UTF8Encoding(true).GetBytes(xml);
                MemoryStream stream = new MemoryStream(title);
                // string folderName = AppSettings.Get("AWSS3FolderName");
                sendXMLDocument(stream, fileName);
                UpdateFileStatus(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]));
            }

            UploadedFileNames = new List<ProcessedDocument>();

            foreach (DataRow dr in dsProv.Tables[0].Rows)
            {
                Attachments_ControlFile controlFile = PopulateAttachmentControlFile(dr, true);
                try
                {
                    if (!IsFileExistsInS3(controlFile.DocumentName))
                    {
                        UploadedFileNames.Add(new ProcessedDocument(controlFile.OriginalDocumentName, controlFile.DocumentName, true));
                    }
                    else
                    {
                        UploadedFileNames.Add(new ProcessedDocument(controlFile.OriginalDocumentName, controlFile.DocumentName, false));
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }

    private Attachments_ControlFile PopulateAttachmentControlFile(DataRow dataRow, Boolean isSend = false)
    {
        Attachments_ControlFile controlFile = new Attachments_ControlFile();
        string claimsorPA = dataRow["EDITransaction_Type"].ToString();

        controlFile.DocumentName = dataRow["DocumentName"].ToString();
        controlFile.DocumentType = dataRow["DOCUMENT_TYPE"].ToString();
        controlFile.EDITransaction_Type = dataRow["EDITransaction_Type"].ToString();
        controlFile.Member_ID = dataRow["Member_ID"].ToString();
        controlFile.PayerRequested = dataRow["PayerRequested"].ToString();
        if (claimsorPA == "PA")
        {
            controlFile.PA_number = dataRow["PA_NUMBER"].ToString();
        }
        else
        {
            controlFile.Claim_number = dataRow["Claim_number"].ToString();
        }
        controlFile.Provider_ID = dataRow["Provider_ID"].ToString();
        controlFile.Provider_NPI = dataRow["Provider_NPI"].ToString();
        controlFile.ReceiverID = dataRow["Receiver_ID"].ToString();
        controlFile.SenderID = dataRow["Sender_ID"].ToString();
        DateTime timedate = Convert.ToDateTime(dataRow["Create_DATE_TIME"].ToString());
        controlFile.Timestamp = Convert.ToString(timedate.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        controlFile.UUID = dataRow["UUID"].ToString();
        if (isSend)
        {
            controlFile.OriginalDocumentName = dataRow["OriginalDocumentName"].ToString();
        }
        return controlFile;
    }


    public string ProcessClaimAttachments(string medicaidNumber)
    {
        StringBuilder errorMessage = new StringBuilder();
        List<SqlParameter> parms = new List<SqlParameter>();
        parms.Add(new SqlParameter("TO_SEND", "1"));
        parms.Add(new SqlParameter("MEDICAID_NUMBER", medicaidNumber));
        DataSet dsProv = DataAccess.ExecuteStoredProcedure("[usp_SelectClaimsAttachementsToBeSent]", parms, "ds");

        if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in dsProv.Tables[0].Rows)
            {
                Attachments_ControlFile controlFile = PopulateClaimAttachmentControlFile(dr);
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Attachments_ControlFile));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, controlFile, emptyNs);
                string xml = "<?xml version=\"1.0\" encoding=\"UTF - 8\"?>" + Environment.NewLine;
                xml = xml + stream2.ToString();
                Guid g = Guid.NewGuid();

                string fileName = System.IO.Path.GetFileNameWithoutExtension(controlFile.DocumentName).Replace("--AT--", "--CF--") + ".xml";
                byte[] title = new UTF8Encoding(true).GetBytes(xml);
                MemoryStream stream = new MemoryStream(title);
                // string folderName = AppSettings.Get("AWSS3FolderName");
                sendXMLDocument(stream, fileName);
                UpdateClaimFileStatus(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]));
            }

            //UploadedFileNamesClaims = new List<ProcessedDocument>();

            foreach (DataRow dr in dsProv.Tables[0].Rows)
            {
                Attachments_ControlFile controlFile = PopulateClaimAttachmentControlFile(dr, true);
                string orgFileName = PopulateClaimAttachmentOrgFile(dr);
                //string fileName = System.IO.Path.GetFileNameWithoutExtension(controlFile.DocumentName);

                try
                {
                    if (!IsFileExistsInS3(controlFile.DocumentName))
                    {
                        UploadedFileNamesClaims.Add(new ProcessedDocument(controlFile.OriginalDocumentName, controlFile.DocumentName, true));
                    }
                    else
                    {
                        UploadedFileNamesClaims.Add(new ProcessedDocument(controlFile.OriginalDocumentName, controlFile.DocumentName, false));
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        return errorMessage.ToString();
    }

    private Attachments_ControlFile PopulateClaimAttachmentControlFile(DataRow dataRow, Boolean isSend = false)
    {
        Attachments_ControlFile controlFile = new Attachments_ControlFile();
        controlFile.Attachment_ControlNumber = dataRow["Document_ID"].ToString();
        // controlFile.Claim_number = dataRow["Claim_number"].ToString();
        controlFile.DocumentName = dataRow["DocumentName"].ToString().Replace(" ", "").Replace("(", "").Replace(")", ""); ;
        controlFile.DocumentType = dataRow["DOCUMENT_TYPE"].ToString();
        controlFile.EDITransaction_Type = dataRow["EDITransaction_Type"].ToString();
        controlFile.Member_ID = dataRow["Member_ID"].ToString();
        controlFile.PayerRequested = "No";// dataRow["PayerRequested"].ToString();
        controlFile.Provider_ID = dataRow["Provider_ID"].ToString();
        controlFile.Provider_NPI = dataRow["Provider_NPI"].ToString();
        controlFile.ReceiverID = dataRow["Receiver_ID"].ToString();
        controlFile.SenderID = dataRow["Sender_ID"].ToString();
        DateTime timedate = Convert.ToDateTime(dataRow["Create_DATE_TIME"].ToString());
        controlFile.Timestamp = Convert.ToString(timedate.ToString("yyyy-MM-ddTHH:mm:ssZ"));
        controlFile.UUID = dataRow["UUID"].ToString();
        if (isSend)
        {
            controlFile.OriginalDocumentName = dataRow["OriginalDocumentName"].ToString();
        }
        return controlFile;
    }

    private string PopulateClaimAttachmentOrgFile(DataRow dataRow)
    {
        string filename = dataRow["OriginalDocumentName"].ToString();
        return filename;

    }

    private static void WriteLog(string msg, string logProcessName)
    {
        // create log object
        Logging log = new Logging();
        log.CreateLogEntry(msg, Logging.LogPriority.Information);
    }

    public bool sendXMLDocument(Stream stream, string fileNameInS3)
    {
        TransferUtility utility = new TransferUtility(_s3Client);
        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();
        request.BucketName = AWSS3WriteBucketName;
        request.Key = fileNameInS3; //file name up in S3  
        request.InputStream = stream;
        utility.Upload(request); //commensing the transfer  
        log.CreateLogEntry("Uploaded XML " + fileNameInS3);
        return true; //indicate that the file was sent  
    }

    public bool IsFileExistsInS3(string fileNameInS3)
    {
        try
        {
            GetObjectMetadataResponse response = _s3Client.GetObjectMetadata(AWSS3ReadBucketName, fileNameInS3);
            log.CreateLogEntry(string.Format("AWSS3ReadBucketName: {0}, fileNameInS3: {1}, response: {2} ", AWSS3ReadBucketName, fileNameInS3, response.HttpStatusCode));
            return true;
        }
        catch (Amazon.S3.AmazonS3Exception ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                log.CreateLogEntry(string.Format("AWSS3WriteBucketName: {0}, fileNameInS3: {1}, errorCode: {2} ", AWSS3ReadBucketName, fileNameInS3, ex.ErrorCode));
            }
        }
        return false;
    }

    public string generatePreSignedUrl(string key, string contentType, DateTime expiryTime)
    {
        GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
        {
            ContentType = contentType,
            BucketName = AWSS3WriteBucketName,
            Key = key,
            Expires = expiryTime,
            Verb = HttpVerb.PUT,
            Protocol = Protocol.HTTPS
        };

        string urlString = _s3Client.GetPreSignedURL(request);

        if (string.IsNullOrWhiteSpace(urlString))
            return "Couldn't generate the pre-signed URL";

        return urlString;
    }

    /// <summary>
    /// Download the S3 uploaded file.
    /// </summary>
    /// <param name="fileName"></param>
    public void GetAttachment(string fileName)
    {
        GetObjectRequest request = new GetObjectRequest();
        MemoryStream ms = new MemoryStream();
        request.BucketName = AWSS3ReadBucketName;
        request.Key = fileName;
        GetObjectResponse response = _s3Client.GetObject(request);
        using (Stream responseStream = response.ResponseStream)
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
        HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + request.Key);

        byte[] memoryBytes = ms.ToArray();
        HttpContext.Current.Response.BinaryWrite(memoryBytes);

        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.Close();
        HttpContext.Current.Response.End();
    }


    public void UpdateFileStatus(int outbound_document_uploads_id)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
        DataAccess.ExecuteStoredProcedure("usp_UpdateOutbound_Document_Uploads_IS_SENT", param);

    }

    public void UpdateFileStatusToSend(int outbound_document_uploads_id,int transactionID)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
        param.Add(SqlParms.CreateParameter("transactionID", DbType.Int32, transactionID, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
        DataAccess.ExecuteStoredProcedure("usp_UpdateOutbound_Document_Uploads_To_SEND", param);

    }
    public void UpdateFileStatusIsMalicious(int outbound_document_uploads_id)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
        DataAccess.ExecuteStoredProcedure("usp_UpdateOutbound_Document_Uploads_Is_Malicious", param);

    }
    public void UpdateClaimFileStatusToSend(int outbound_document_uploads_id,int transactionID, string txtMedicaidBillingNumber)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
        param.Add(SqlParms.CreateParameter("transactionID", DbType.Int32, transactionID, true));
        param.Add(SqlParms.CreateParameter("memberID", DbType.String, txtMedicaidBillingNumber, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_UpdateOutbound_Document_Uploads_To_SEND", param);

    }
    public void UpdateClaimFileStatus(int outbound_document_uploads_id)
    {
        List<SqlParameter> param = new List<SqlParameter>();
        param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
        param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
        DataAccess.ExecuteStoredProcedure("usp_Claims_UpdateOutbound_Document_Uploads_IS_SENT", param);

    }
}
public class ProcessedDocument
{
    public string orginalFileName;
    public string awsFileName;
    public bool isMalicious;

    public ProcessedDocument(string orginalFileName, string awsFileName, bool isMalicious)
    {
        this.orginalFileName = orginalFileName;
        this.awsFileName = awsFileName;
        this.isMalicious = isMalicious;
    }

}

