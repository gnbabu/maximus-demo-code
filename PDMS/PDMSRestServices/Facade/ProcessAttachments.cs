using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Configuration;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using Amazon;
using System.Web;
using System.Threading.Tasks;
using System.Linq;
 



namespace PDMSRestServices.Controllers.Facade
{

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


        
        

       

        public async Task<byte[]> GetAttachmentAsync(string fileName)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = AWSS3ReadBucketName,
                    Key = fileName
                };



                using GetObjectResponse response = await _s3Client.GetObjectAsync(request);
                using MemoryStream ms = new MemoryStream();
                using Stream responseStream = response.ResponseStream;
                using BinaryReader reader = new BinaryReader(responseStream);

                byte[] fileBytes = reader.ReadBytes((int)response.ContentLength);
                return fileBytes;

            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Error retrieving file: {ex.Message}");
            }
        }

        public string generatePreSignedUrl(string key, string contentType, DateTime expiryTime, string method)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
            {
                ContentType = contentType,
                BucketName = AWSS3WriteBucketName,
                Key = key,
                Expires = expiryTime,
                Verb = ConvertToHttpVerb(method),
                Protocol = Protocol.HTTPS
            };

            string urlString = _s3Client.GetPreSignedURL(request);

            if (string.IsNullOrWhiteSpace(urlString))
                return "Couldn't generate the pre-signed URL";

            return urlString;
        }

        public static HttpVerb ConvertToHttpVerb(string method)
        {
            var verb= Enum.TryParse<HttpVerb>(method, true, out HttpVerb parsedVerb) ? parsedVerb : HttpVerb.GET;
            return verb;
        }


        public void UpdateFileStatus(int outbound_document_uploads_id)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(SqlParms.CreateParameter("outbound_document_uploads_id", DbType.Int32, outbound_document_uploads_id, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            param.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, UserId, true));
            DataAccess.ExecuteStoredProcedure("usp_UpdateOutbound_Document_Uploads_IS_SENT", param);

        }

        public void UpdateFileStatusToSend(int outbound_document_uploads_id, int transactionID)
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
        public void UpdateClaimFileStatusToSend(int outbound_document_uploads_id, int transactionID, string txtMedicaidBillingNumber)
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
}

