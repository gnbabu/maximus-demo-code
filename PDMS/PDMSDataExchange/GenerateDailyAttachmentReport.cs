using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

using Amazon.S3;

using Amazon.S3.Transfer;

using Amazon;

using System.IO.Compression;
using System.Reflection;
using Newtonsoft.Json;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.S3.Model;
using System.Net.Mail;


namespace MAXIMUS.DataExchange.PDMS
{


    public class GenerateDailyAttachmentReport : BaseJob, IJob
    {
        private RegionEndpoint AWSRegionEndpoint { get; set; } = RegionEndpoint.GetBySystemName(AppSettings.Get("SecretsRegion"));

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
        public GenerateDailyAttachmentReport(Guid threadId)
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
            get { return AppSettings.Get("AWSS3ArchiveBucketName"); }
        }
        private IAmazonS3 _s3Client;
        private Logging log = null;
        private const string appPDMSDataExchangeUserId = "FD41DDFC-DBD6-4BFD-BCB2-27B3BB31D211";
        private Guid UserId
        {
            get { return new Guid(appPDMSDataExchangeUserId); }
        }
        override public void ExecuteJob()
        {

            this.ExecuteJob(Guid.Parse("CAC43B9C-0DB5-406F-8964-2CB3E1B3BF5A"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            string jobGuid = jobId.ToString().ToUpper();

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

            if (jobGuid == "CAC43B9C-0DB5-406F-8964-2CB3E1B3BF5A")
            {
                ProcessAttachments();

            }

        }
        Logging logs;
        public void ProcessAttachments()
        {
            StringBuilder errorMessage = new StringBuilder();

            DataSet dsProv = DataAccess.ExecuteStoredProcedure("usp_DailyAttachmentsReport", "ds");
            log.CreateLogEntry("Create Daily Report ");

            if (dsProv.Tables.Count > 0 && dsProv.Tables[0].Rows.Count > 0)
            {
                DataTable table = new DataTable();
                table.Columns.Add("DocumentName", typeof(string));
                table.Columns.Add("TransactionType", typeof(string));
                table.Columns.Add("SI_TRANSACTION_KEY", typeof(string));
                log.CreateLogEntry("Retrieved files sent to SI");
                foreach (DataRow dr in dsProv.Tables[0].Rows)
                {
                    bool isExistsinArchieve = false;
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(dr["DocumentName"].ToString()).Replace("--AT--", "--CFAT--") + ".zip";
                    log.CreateLogEntry("Check if files exists in Archive "+ fileName);
                    isExistsinArchieve = IsFileExistsInS3Archieve(fileName);
                    if (isExistsinArchieve)
                    {

                        log.CreateLogEntry("Files exists in Archive " + fileName);
                        // Iterate through data source object and fill the table
                        table.Rows.Add(dr["DocumentName"].ToString(), dr["TransactionType"].ToString(), dr["SI_TRANSACTION_KEY"].ToString());
                        //Creat CSV File



                    }

                }
                string sCsvFilePath = AppSettings.Get("AttachmentReportFSXPath") + "DailyAttachmentSent_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                log.CreateLogEntry("Start Creating file " + sCsvFilePath);
                CreateCSVFile(table, sCsvFilePath);
            }

        }
        public void CreateCSVFile(DataTable dt, string strFilePath)
        {
            try
            { 
            StreamWriter sw = new StreamWriter(strFilePath, true);

            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < iColCount - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);

            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        sw.Write(dr[i].ToString());
                    }
                    if (i < iColCount - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
                log.CreateLogEntry("Completed Creating file " + strFilePath);
                SendEmailNotification(strFilePath);
                
            }
            catch(Exception ex) {
            log.CreateLogEntry(ex.Message);
            }


            //Send email
            //Stream containing your CSV (convert it into bytes, using the encoding of your choice)

    }
        static byte[] GetData(string strFilePath)
        {
            //this method just returns some binary data.
            //it could come from anywhere, such as Sql Server
            string s = "this is some text";
            
            byte[] data = File.ReadAllBytes(strFilePath);
            return data;
        }
        private void SendEmailNotification(string strFilePath)
        {
            try { 
            byte[] data = GetData(strFilePath);

            //save the data to a memory stream
            MemoryStream ms = new MemoryStream(data);
            string to = "OHPNMCodeJunkies@maximus.com";
            const char delimiter1 = ',';
            const char delimiter2 = ';';
            string subject= "Daily Attachment Report"; bool isHTML; bool sendSSL;
             bool bypassTestEmail = false; string cc = ""; string bcc = "";
                //Add a new attachment to the E-mail message, using the correct MIME type
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, new ContentType("text/csv"));
                //attachment.Name = strFilePath;
                

                MailMessage emailMessage = new MailMessage();
                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(ms, "DailyAttachmentSent_" + DateTime.Now.ToString("yyyyMMdd") + ".csv", "text/csv"));
                string emailSubject = ConstructSubject(subject);
            string testing = bool.TrueString.ToLower();
            bool testEmail = false;
                // if testing enabled
                testing = AppSettings.Get("TestEmailEnabled", bool.TrueString.ToLower());
                if (testing.ToLower() == bool.TrueString.ToLower() && bypassTestEmail == false)
                {
                    string logTestingMessage = "Test email address(es) being substituted for:" + to;
                    logTestingMessage = String.Format(logTestingMessage, emailSubject, to);
                    //log.CreateLogEntry(logTestingMessage, +logCnt);
                    to = AppSettings.Get("TestEmailAddress", "OHPNMCodeJunkies@maximus.com");
                    testEmail = true;
                }
                else
                {
                    if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                }

                // add the To email address(es)
                char delimiter = delimiter1;
                if (to.IndexOf(delimiter1) == -1)
                {
                    delimiter = delimiter2;
                }
                string[] Addrs = to.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string adr in Addrs)
                {
                    emailMessage.To.Add(new MailAddress(adr));
                }
                if (!string.IsNullOrWhiteSpace(cc))
                {
                    delimiter = delimiter1;
                    if (cc.IndexOf(delimiter1) == -1)
                    {
                        delimiter = delimiter2;
                    }
                    string[] bccAddrs = cc.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string adr in bccAddrs)
                    {
                        emailMessage.CC.Add(new MailAddress(adr));
                    }
                }

                // add the BCC email address(es)
                string bccEmail = AppSettings.Get("SmtpBCC", string.Empty);
                bccEmail = bccEmail + bcc;
                if (!string.IsNullOrWhiteSpace(bccEmail))
                {
                    delimiter = delimiter1;
                    if (bccEmail.IndexOf(delimiter1) == -1)
                    {
                        delimiter = delimiter2;
                    }
                    string[] bccAddrs = bccEmail.Split(new char[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string adr in bccAddrs)
                    {
                        emailMessage.Bcc.Add(new MailAddress(adr));
                    }
                }
                string replyToEmail = AppSettings.Get("SmtpReplyTo", string.Empty);
                string emailfromAddress = AppSettings.Get("SmtpFromEmailAddress", string.Empty);

                if (!string.IsNullOrEmpty(replyToEmail))
                {
                    emailMessage.ReplyToList.Add(replyToEmail);
                }
                if (!string.IsNullOrEmpty(emailfromAddress))
                {
                    emailMessage.From = new MailAddress(emailfromAddress);
                }

                emailMessage.Subject = emailSubject;
                emailMessage.Body = "Attachments successfully sent over to SI for Claims and Prior Authorizations.";
                emailMessage.IsBodyHtml = false;
                //Send your message

                SmtpClient smtpclient;
                smtpclient = new SmtpClient();

                smtpclient.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(AppSettings.Get("SmtpClientPortNumber")))
                {
                    smtpclient.Port = (int)Methods.GetIntValue(AppSettings.Get("SmtpClientPortNumber"), false);
                }

                smtpclient.Host = AppSettings.Get("SmtpClientHost");
                smtpclient.UseDefaultCredentials = Convert.ToBoolean(AppSettings.Get("SmtpUseDefaultCredentials", bool.FalseString.ToLower()));
                smtpclient.EnableSsl = false;

                // send it
                smtpclient.Send(emailMessage);
                log.CreateLogEntry("Email sent successfully " );
            }
            catch(Exception ex)
            {
                log.CreateLogEntry(ex.Message);
            }




    }
    private string ConstructSubject(string subject)
    {
        string rtn = subject;
        string testing = AppSettings.Get("SubjectWithEnvironment", bool.TrueString.ToLower());
        if (testing.ToLower() == bool.TrueString.ToLower() &&
            !string.IsNullOrEmpty(AppSettings.Get("Environment", string.Empty)))
        {
            // If not already in subject add it
            string append = "[" + AppSettings.Get("Environment", string.Empty) + "]";
            if (subject.IndexOf(append) == -1)
            {
                rtn += " " + append;
            }
        }
        return rtn;
    }
    public bool IsFileExistsInS3Archieve(string fileNameInS3)
        {
            try
            {
                log.CreateLogEntry("check if exists in "+ AWSS3ArchiveBucketName + " " + fileNameInS3);
                GetObjectMetadataResponse response = _s3Client.GetObjectMetadata(AWSS3ArchiveBucketName, fileNameInS3);
                log.CreateLogEntry(string.Format("AWSS3WriteBucketName: {0}, fileNameInS3: {1}, response: {2} ", AWSS3ArchiveBucketName, fileNameInS3, response.HttpStatusCode));
                return true;
            }
            catch (Amazon.S3.AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    log.CreateLogEntry(string.Format("AWSS3WriteBucketName: {0}, fileNameInS3: {1}, errorCode: {2} ", AWSS3ArchiveBucketName, fileNameInS3, ex.ErrorCode));
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(ex.Message);
            }

            return false;
        }

    }


}