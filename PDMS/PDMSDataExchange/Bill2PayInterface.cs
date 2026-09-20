using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace MAXIMUS.DataExchange.PDMS
{
    public class Bill2PayInterface : BaseJob, IJob
    {
#region "Class Level Declarations"

#endregion

#region "Constructors"
        public Bill2PayInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }
#endregion

#region "Public Methods"
        override public void ExecuteJob()
        {
            // Default Job - B2P Retrieve Payment Information
            this.ExecuteJob(Guid.Parse("E7101842-ABE3-4CD7-8D15-36E5D3F1C93F"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                // MMIS Retrieve Payment Information
                case "E7101842-ABE3-4CD7-8D15-36E5D3F1C93F":
                    this.RetrievePaymentInfo();
                    break;
            }
        }

        public string SendPayment()
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://betapaymentcenter.bill2pay.com/default.aspx?client=register");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            return response.StatusDescription.ToString();
        }

        public string RetrievePaymentInfo()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(this.ThreadId, logMsg);
            string testingEnabled = AppSettings.Get("B2P-InterfaceTesting", bool.TrueString).ToLower();

            try
            {
                if (testingEnabled == bool.TrueString.ToLower())
                {
                    this.UpdateRegistration(null, log);
                }
                else
                {
                    // create log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingB2PRecordsStart));

                    // retrieve and set required variables
                    string localPath = AppSettings.Get("B2P-ReturnPaymentStatusLocalPath");
                    string fileName = AppSettings.Get("B2P-ReturnPaymentStatusFile");
                    string dtmWildcard = AppSettings.Get("B2P-ReturnPaymentStatusFileWildcardDTM");
                    DirectoryInfo localDirectory;
                    
                    // Get file name
                    fileName = fileName.Replace(dtmWildcard, DateTime.Now.ToString(dtmWildcard));

                    // create local path if it does not exist
                    Directory.CreateDirectory(localPath);
                    localDirectory = new DirectoryInfo(localPath);

                    FileInfo file = localDirectory.GetFiles(fileName).FirstOrDefault();

                    if (file != null)
                    {
                        this.LoadBill2PayResponseFile(log, file);
                    }

                    // create log entry
                    log.CreateLogEntry(String.Format(Constants.LogString.RetrievingB2PRecordsEnd));
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);

            return string.Empty;
        }

        private void LoadBill2PayResponseFile(Logging log, FileInfo file)
        {
            string fileName = file.Name;

            // create log entry
            log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

            // open the file with FileHelper class and set internal variables
            RetrieveBill2Pay[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(RetrieveBill2Pay));

            try
            {
                log.CreateLogEntry(String.Format(Constants.LogString.RetrievingMMISRecordsStart));

                // Read records from file
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(file.FullName) as RetrieveBill2Pay[];

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString()
                    , engine.ErrorManager.ErrorCount.ToString()));

                this.UpdateRegistration(records, log);

                // record all bad errors
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        LogFileRecordError(log, engine.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }

        private void UpdateRegistration(RetrieveBill2Pay[] records, Logging log)
        {
            DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRegWithPendingPayment", "PaymentPending");
            DataTable dt = ds.Tables[0];
            DateTime now = DateTime.Now;

            foreach (DataRow row in dt.Rows)
            {
                string applicationFeeeStatus = Constants.ApplicationFeePaymentStatus.Pending;
                string regID = row["REG_ID"].ToString();
                string applicationFeeID = row["REG_APPLICATION_FEE_ID"].ToString();

                if (records != null)
                {
                    RetrieveBill2Pay bill2PayInfo = records.FirstOrDefault(record => record.VendorReferenceCode == regID);
                    if (bill2PayInfo == null)
                    {
                        log.CreateLogEntry(String.Format(Constants.LogString.RetrievingApplicationFeeRecordNotFound, regID));
                        continue;
                    }
                    applicationFeeeStatus = bill2PayInfo.StatusCode == "0" ? Constants.ApplicationFeePaymentStatus.Paid : Constants.ApplicationFeePaymentStatus.Pending;
                }
                else
                {
                    applicationFeeeStatus = Constants.ApplicationFeePaymentStatus.Paid;
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                Dictionary<string, string> parms = new Dictionary<string, string>();

                parms.Add("REG_ID", regID);
                parms.Add("REG_APPLICATION_FEE_ID", applicationFeeID);
                parms.Add("APPLICATION_FEE_STATUS_ID", applicationFeeeStatus);
                parms.Add("LAST_MODIFIED_DATE_TIME", now.ToString());
                parms.Add("FEE_STATUS_DATE_TIME", now.ToString());
                parms.Add("LAST_MODIFIED_USER", Constants.appPDMSDataExchangeUserId);

                RegistrationController.UpdateRegistrationData("APPLICATION_FEE", parms);
            }
        }

        private void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
        }
    }
#endregion
}