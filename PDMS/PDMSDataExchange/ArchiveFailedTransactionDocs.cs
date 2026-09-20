using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace MAXIMUS.DataExchange.PDMS
{
    public class ArchiveFailedTransactionDocs : BaseJob, IJob
    {
        private RegionEndpoint AWSRegionEndpoint { get; set; } = RegionEndpoint.GetBySystemName(AppSettings.Get("SecretsRegion"));
        private int xmlStatus = 5;

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
        public int XmlStatus
        {
            get
            {
                return xmlStatus;
            }
            set
            {
                xmlStatus = value;
            }

        }

        public ArchiveFailedTransactionDocs(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }
        private string AWSSecretsRegion
        {
            get { return AppSettings.Get("SecretsRegion"); }

        }
        private string AWSS3ArchiveBucketName
        {
            get { return AppSettings.Get("AWSS3FailedTransArchiveBucketName"); }
        }
        private string AWSS3ReadBucketName
        {
            get { return AppSettings.Get("AWSS3ReadBucketName"); }
        }
        
        private IAmazonS3 _s3Client;
        private IAmazonS3 _s3ClientDest;
        private Logging log = null;
        private const string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
        private Guid UserId
        {
            get { return new Guid(appPDMSDataExchangeUserId); }
        }
        override public void ExecuteJob()
        {
            // TODO: Need to change the Guid to match the job in the database
            this.ExecuteJob(Guid.Parse("B9883D2E-A9FE-451C-B7D0-ADEC2F421591"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string jobGuid = jobId.ToString().ToUpper();

            var secretsManager = new SecretsManager(AppSettings.Get("SecretsRegion"));
            var result = secretsManager.GetSuperSecretPassword(AppSettings.Get("S3UploadSecrets"));
            result.Wait();
            string AWSAccessKey = result.Result["AWSAccessKey"];
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

            _s3ClientDest = new AmazonS3Client(AWSAccessKey, AWSSecretKey, AWSRegionEndpoint);

            if (jobGuid == "B9883D2E-A9FE-451C-B7D0-ADEC2F421591")
            {
                string archiveFailedTranDocs = AppSettings.Get("ArchiveFailedTransactionDocs").ToString();
                if (archiveFailedTranDocs == "true")
                {
                    ProcessArchiveAttachments();
                }
            }

        }
        Logging logs;
        public void ProcessArchiveAttachments()
        {
            StringBuilder errorMessage = new StringBuilder();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("TO_SEND", "1"));
            //parms.Add(new SqlParameter("MEDICAID_NUMBER", medicaidNumber));
            DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectFailedTransactionAttachementsToBeSent", parms, "ds");

            if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsProv.Tables[0].Rows)
                {
                    try
                    {
                        Attachments_ControlFile controlFile = PopulateAttachmentControlFile(dr);

                        bool isExistsinPromote = false;
                        isExistsinPromote = IsFileExistsInS3(controlFile.DocumentName);
                        if (isExistsinPromote)
                        {
                            MoveFileBetweenBuckets(controlFile.DocumentName);
                            UpdateFileStatus(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        UpdateExcptnInOutboundDocUploadTbl(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]), message, null);

                        continue;
                    }
                }
            }
            parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("TO_SEND", "1"));
            dsProv = DataAccess.ExecuteStoredProcedure("usp_SelectFailedClaimsAttachementsToBeSent", parms, "ds");

            if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsProv.Tables[0].Rows)
                {
                    try
                    {
                        Attachments_ControlFile controlFile = PopulateClaimAttachmentControlFile(dr);

                        bool isExistsinPromote = false;
                        isExistsinPromote = IsFileExistsInS3(controlFile.DocumentName);
                        if (isExistsinPromote)
                        {
                            MoveFileBetweenBuckets(controlFile.DocumentName);
                            UpdateClaimFileStatus(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = ex.ToString();
                        UpdateExcptnInClaimsOutboundDocUploadTbl(Convert.ToInt32(dr["OutBound_Document_Uploads_ID"]), message, null);

                        continue;
                    }

                }

            }

        }

        public bool IsFileExistsInS3(string fileNameInS3)
        {
            try
            {
                GetObjectMetadataResponse response = _s3Client.GetObjectMetadata(AWSS3ReadBucketName, fileNameInS3);
                return true;
            }
            catch (Amazon.S3.AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    log.CreateLogEntry(string.Format("AWSS3ReadBucketName: {0}, fileNameInS3: {1}, errorCode: {2} ", AWSS3ReadBucketName, fileNameInS3, ex.ErrorCode));
                }
            }
            return false;
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
                if (dataRow["PA_NUMBER"].ToString() == "")
                {
                    controlFile.Attachment_ControlNumber = dataRow["OutBound_Document_Uploads_ID"].ToString();
                }
                else
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
            if (AppSettings.Get("EnableCR739PCR1").ToLower() == "true")
            {
                controlFile.ProviderComments = dataRow["providerComments"].ToString();
            }
            else
            {
                controlFile.ProviderComments = "";
            }

            return controlFile;
        }
        
        public bool MoveFileBetweenBuckets(string fileName)
        {
            // Copy the file to the destination bucket
            var copyRequest = new CopyObjectRequest
            {
                SourceBucket = AWSS3ReadBucketName,
                SourceKey = fileName,
                DestinationBucket = AWSS3ArchiveBucketName,
                DestinationKey = fileName
            };
            _s3ClientDest.CopyObjectAsync(copyRequest).Wait();

            // Delete the file from the source bucket
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = AWSS3ReadBucketName,
                Key = fileName
            };
            _s3Client.DeleteObjectAsync(deleteRequest).Wait();

            return true;
        }

        public void UpdateFileStatus(int outbound_document_uploads_id, int? xmlStatus)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
            param.Add(SqlParms.CreateParameter("xmlStatus", DbType.Int32, xmlStatus, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
            DataAccess.ExecuteStoredProcedure("usp_UpdateOutbound_Document_Uploads_IS_SENT", param);

        }

        public void UpdateExcptnInClaimsOutboundDocUploadTbl(int outbound_document_uploads_id, string exceptionLog, int? xmlStatus)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
            param.Add(SqlParms.CreateParameter("xmlStatus", DbType.Int32, xmlStatus, true));
            param.Add(SqlParms.CreateParameter("ExceptionInfo", DbType.String, exceptionLog, true));
            DataAccess.ExecuteStoredProcedure("usp_WF_InsExceptInClaims_OutBound_Document_Uploads", param);

        }

        public void UpdateExcptnInOutboundDocUploadTbl(int outbound_document_uploads_id, string exceptionLog, int? xmlStatus)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
            param.Add(SqlParms.CreateParameter("xmlStatus", DbType.Int32, xmlStatus, true));
            param.Add(SqlParms.CreateParameter("exceptionLog", DbType.String, exceptionLog, true));
            DataAccess.ExecuteStoredProcedure("usp_WF_InsExceptInOutBound_Document_Uploads", param);

        }

        private void UpdateClaimFileStatus(int outbound_document_uploads_id, int? xmlStatus)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
            param.Add(SqlParms.CreateParameter("xmlStatus", DbType.Int32, xmlStatus, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
            DataAccess.ExecuteStoredProcedure("usp_Claims_UpdateOutbound_Document_Uploads_IS_SENT", param);

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
    }
}