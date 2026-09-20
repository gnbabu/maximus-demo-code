

using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml.Office2010.Word;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace MAXIMUS.DataExchange.PDMS.PASRR
{
    public class PASRRProcessInterface : BaseJob, IJob
    {
        private int logCnt = 0;
        private Logging log = null;

        FileInfo _currentFile;

        FileInfo CurrentFile
        {
            get
            {
                return _currentFile;
            }
            set
            {
                _currentFile = value;
            }
        }

        public PASRRProcessInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "2A0B5D77-E595-432F-83DC-9A5737A19D43";

        public override void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse("2A0B5D77-E595-432F-83DC-9A5737A19D43"));
        }

        public override void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    this.ThreadId = new Guid(appID);
                    UnzipPASRRFile();
                    UploadToOnBase();
                    break;
            }
        }

        private void UnzipPASRRFile()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {
                

                
                string localPath = AppSettings.Get("PASRR-ImportFileLocalPath");
                string extractPath = AppSettings.Get("PASRR-ExtractFileLocalPath");

                DirectoryInfo localDirectory;
                localDirectory = new DirectoryInfo(localPath);

                log.CreateLogEntry("Begin PASRR extraction", "PASRRProcessInterface::ImportPASRRFile");


                string[] fileNames = Directory.GetFiles(localPath, "*.Zip");
                System.IO.Compression.ZipFile.ExtractToDirectory(fileNames[0], extractPath);
                

                log.CreateLogEntry("End PASRR extraction", "PASRRProcessInterface::ImportPASRRFile");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("ExecuteJob failed. PASRR extraction", "PASRRProcessInterface::ImportPASRRFile");
            }
        }
        private void UploadToOnBase()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            Logging log = new Logging(ThreadId, logMsg);

            try
            {

                string extractPath = AppSettings.Get("PASRR-ExtractFileLocalPath");



                log.CreateLogEntry("Begin PASRR Upload to OnBase", "PASRRProcessInterface::ImportPASRRFile");


                string[] fileNames = Directory.GetFiles(extractPath);

                OnBaseInterface onBaseInterface = new OnBaseInterface();
                Guid guid = Guid.NewGuid();
                
                foreach (string file in fileNames)
                {
                    DocRegXref retValXref = new DocRegXref();
                    string fileName = Path.GetFileName(file);
                    string medicaid_id = fileName.Substring(6, 7);
                    retValXref = SaveDocument(log, guid, fileName, "PASRR Report",medicaid_id);
                    int reg_id = retValXref.Reg_ID;
                    int docID = retValXref.Doc_ID;
                    byte[] fileBytes = File.ReadAllBytes(file);
                    onBaseInterface.SubmitFile( docID, fileBytes, fileName);

                    //Send Email notification here
                    var sentEmail = SendEmail(reg_id, medicaid_id);
                }

                log.CreateLogEntry("End PASRR Upload to OnBase", "PASRRProcessInterface::ImportPASRRFile");

                
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("ExecuteJob failed. PASRR extraction", "PASRRProcessInterface::ImportPASRRFile");
            }
        }

        private bool SendEmail(int regID, string medId)
        {
            bool emailSent = false;
            try
            {
                string subject = "New Medicaid Preadmission Screening and Resident Review (PASRR Report) ";
                //DataSet dsEmails = RegistrationController.SelectRegistrationEmails(regID, subject);
                string Template_Name = "EMAIL_TEMPLATE_PASRR";
                //if (!ObjectControllerHelper.HasRows(dsEmails))
                //{
                    string recipients = "";
                    GetRecipients(regID, out recipients);

                    EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId);
                    if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                        emailSent = true;
                    else
                    {
                        log.CreateLogEntry("Reg Id: " + regID + " - problems sending Preadmission Screening and Resident Review email",
                        Logging.LogPriority.Error);
                    }
                //}
                //else
                //{
                //    log.CreateLogEntry("Skipping sending Preadmission Screening and Resident Review email for reg Id = " + regID + " as we found an email");
                //}
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Failed to send Preadmission Screening and Resident Review notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
            }
            return emailSent;


        }

        private string AddEmail(string addressList, DataSet ds, string colName)
        {
            string rtn = addressList;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string email = dr[colName].ToString();
                    if (!string.IsNullOrEmpty(email) && rtn.IndexOf(email) == -1)
                    {
                        if (!string.IsNullOrEmpty(rtn)) rtn += ",";
                        rtn += email;
                    }
                }
            }
            return rtn;
        }

        private void GetRecipients(int regId, out string recList)
        {
            string rtn = string.Empty;
            recList = string.Empty;
            try
            {
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PA_Users", parameters, "ProviderAdmins");
                recList = AddEmail(recList, ds, "EMAIL");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Reg Id: " + regId.ToString() + ", GetRecipients - " + ex.Message, Logging.LogPriority.Error);
            }

            //return rtn;
        }

        public static DocRegXref SaveDocument(Logging log, Guid threadId, string filename, string desc,string Medicaid_ID)
        {
            DocRegXref retValXref = new DocRegXref();

            try
            {
                
                List<SqlParameter> parameters = new List<SqlParameter>();
                
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, System.DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, threadId, false));
                parameters.Add(SqlParms.CreateParameter("CREATE_DATE_TIME", DbType.DateTime, System.DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("Created_By_User", DbType.String, threadId, false));
                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, Medicaid_ID, false));

                DataSet retVal = new DataSet();
                retVal = DataAccess.ExecuteStoredProcedure("usp_insertPASRR_Reports", parameters,"PASRRReports");

                
                retValXref.Doc_ID = Convert.ToInt32(retVal.Tables[0].Rows[0]["doc_id"]);
                retValXref.Reg_ID = Convert.ToInt32(retVal.Tables[0].Rows[0]["reg_id"]);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Save Document failed. PASRR extraction", "PASRRProcessInterface::ImportPASRRFile");
                throw CoreException.ThrowException(threadId, ex);
            }

            return retValXref;
        }
    }
    public class DocRegXref
    {
        private int reg_id;
        private int Docid;
        public int Reg_ID
        {
            get
            {
                return this.reg_id;
            }
            set
            {
                this.reg_id = value;
            }
        }
        public int Doc_ID
        {
            get
            {
                return this.Docid;
            }
            set
            {
                this.Docid = value;
            }
        }
    }
}
