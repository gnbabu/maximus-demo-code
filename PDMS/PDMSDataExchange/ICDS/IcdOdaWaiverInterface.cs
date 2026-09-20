using Corp.Core.Libraries;
using FileHelpers;
using MAXIMUS.Controllers.PDMS;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MAXIMUS.DataExchange.PDMS.ICDS
{
    public class IcdOdaWaiverInterface : BaseJob, IJob
    {

        private int logCnt = 0;
        private Logging log = null;

        public IcdOdaWaiverInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "87C34ADD-1890-4A33-B640-0A5B9F0571EC";
        private readonly string TermappID = "8EE2C7A7-88F6-40A6-8997-6AEEDD0BDAF4";
        private readonly string SendPYappID = "8F4988B4-87F2-4D07-B7ED-8EBF2CC58DE0";

        override public void ExecuteJob()
        {
            // Default Job - ICD Process ODA Waiver Services File
            this.ExecuteJob(Guid.Parse("87C34ADD-1890-4A33-B640-0A5B9F0571EC"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();

            if (jobGuid == "87C34ADD-1890-4A33-B640-0A5B9F0571EC")
            {
                ImportODAServices();
            }
            if (jobGuid == "8EE2C7A7-88F6-40A6-8997-6AEEDD0BDAF4")
            {
                TerminateRegistrations();
            }
            if (jobGuid == "8F4988B4-87F2-4D07-B7ED-8EBF2CC58DE0")
            {
                SendOutTransactionPayloads();               
            }
        }

        public void TerminateRegistrations()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(TermappID), logMsg);

            if (AppSettings.Get("AllowTerminateRegistrations").ToString() == "true")
            {
                TerminateRegistrations(log, ThreadId);
                SendOutEmails(log, ThreadId);
            }
            else
            {
                log.CreateLogEntry("Terminate registrations is not executed as this option is disabled based on the settings.");
            }
        }

        public void SendOutTransactionPayloads()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(SendPYappID), logMsg);

            SendOutTransactionPayloads(log, ThreadId);
        }

        public void ImportODAServices()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            int filePrimaryKey = -1;
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                // create log entry
                log.CreateLogEntry(Constants.LogString.ICDS_ODA_Initiate);
                string icdWildcard = AppSettings.Get("ICDS-ODA-FileWildcardEXT");

                // retrieve and set required variables
                string localPath = AppSettings.Get("ICDS-ODA-ImportLocalPath");
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    localPath = @"C:\Projects\Deployments\PDMS\PNM_DEV\ICDS-ODA\";
                }
                string archiveFolder = localPath + "Archive";
                DirectoryInfo localDirectory;
                string testingEnabled = "false";

                // create local path if it does not exist
                Directory.CreateDirectory(localPath);
                localDirectory = new DirectoryInfo(localPath);

                // create archive path if it does not exist
                if (!Directory.Exists(archiveFolder))
                {
                    Directory.CreateDirectory(archiveFolder);
                }

                // if testing enabled
                if (testingEnabled == bool.TrueString.ToLower())
                {
                    //nothing for now
                }
                else
                {
                    foreach (FileInfo file in localDirectory.GetFiles(icdWildcard).OrderBy(f => f.LastWriteTime))
                    {
                        filePrimaryKey = IcdOdaWaiverHelper.ImportFileToStaging(log,this.ThreadId, file);

                        if (filePrimaryKey != 0)
                        {
                            log.CreateLogEntry(String.Format(Constants.LogString.LoadingFile, file.Name));
                            this.LoadODAWaiverFile(log, file, filePrimaryKey);
                        }
                        else
                        {
                            log.CreateLogEntry(String.Format("Skipping load of file [{0}], loaded previously", file.Name), +logCnt);
                        }
                        string archiveDirectory = localPath + @"Archive\";
                        string newFileName = string.Empty;
                        Directory.CreateDirectory(archiveDirectory);
                        if (File.Exists(archiveDirectory + file.Name))
                        {
                            newFileName = IcdOdaWaiverHelper.RenameFileMethod(archiveDirectory, file.Name);
                            file.MoveTo(archiveDirectory + newFileName);
                        }
                        else
                        {
                            file.MoveTo(archiveDirectory + file.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }

        public void SendOutTransactionPayloads(Logging log, Guid threadId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            int total = 0;
            int regID = 0;
            string mId = string.Empty;
            int sId = 0;
            int tId = 0;
            int transactionType = 0;
            int transactionID = 0;

            try
            {
                log.CreateLogEntry("Fetching termination transactions to send.");
                string icdmakeWSCall = AppSettings.Get("ICDS-ODA-makeWSCall");
                if (icdmakeWSCall.ToLower().Equals("true"))
                {
                    DataSet dsTrans = IcdOdaWaiverHelper.SelectODATerminationRegIDs(log, threadId, 3);
                    if (ObjectControllerHelper.HasRows(dsTrans))
                    {
                        foreach (DataRow row in dsTrans.Tables[0].Rows)
                        {
                            regID = ObjectControllerHelper.GetInt("REG_ID", row);
                            DataSet ds1 = RegistrationController.SelectSvcLocIDTranType(regID);
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                mId = ds1.Tables[0].Rows[0]["MEDICAID_ID"].ToString();
                                sId = Convert.ToInt32(ds1.Tables[0].Rows[0]["REG_SERVICE_LOCATION_ID"]);
                                tId = Convert.ToInt32(ds1.Tables[0].Rows[0]["TRANSACTION_TYPE_ID"]);
                            }

                            transactionType = (int)TransactionController.TransactionTypeNew.SendMMISUpdate;
                            transactionID = TransactionController.InsertTransactionQueue(
                                    transactionType,
                                    regID,
                                    sId,
                                    DateTime.Now,
                                    null,
                                    null,
                                    DateTime.Now,
                                    Constants.appWorkflowUserId);

                            log.CreateLogEntry("Sending for transaction ID - " + transactionID);

                            try
                            {
                                SendTerminationTransactionPayload stp = new SendTerminationTransactionPayload();
                                stp.SendTransaction(transactionID, transactionType);
                                IcdOdaWaiverHelper.UpdateODATerminationProvidersRegIDs(log, threadId, regID, null, null, null, true);
                                total += 1;
                            }
                            catch (Exception ex)
                            {
                                log.CreateLogEntry("Failed to send for transaction id :- " + transactionID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                            }

                        }
                    }
                    log.CreateLogEntry("Finish sending transactions. Total transactions successfully sent - " + total);
                }
                else
                {
                    log.CreateLogEntry("No transaction to send lower env");
                }
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }

        private void LoadODAWaiverFile(Logging log, FileInfo file, int FileID)
        {
            string fileName = file.Name;

            // create log entry
            log.CreateLogEntry(String.Format("Loading return file {0}", fileName));

            // open the file with FileHelper class and set internal variables
            IcdOdaWaiverServiceRecords[] records;
            FileHelperEngine engine = new FileHelperEngine(typeof(IcdOdaWaiverServiceRecords));

            try
            {
                log.CreateLogEntry(Constants.LogString.RetrievingMMISRecordsStart);

                // Read records from file
                engine.ErrorManager.ErrorMode = ErrorMode.SaveAndContinue;
                records = engine.ReadFile(file.FullName) as IcdOdaWaiverServiceRecords[];

                // create log entry
                log.CreateLogEntry(String.Format(Constants.LogString.LoadingRecords, engine.TotalRecords.ToString(), engine.ErrorManager.ErrorCount.ToString()));
                if (engine.ErrorManager.HasErrors)
                {
                    foreach (ErrorInfo err in engine.ErrorManager.Errors)
                    {
                        IcdOdaWaiverHelper.LogFileRecordError(log, err.LineNumber.ToString(), fileName, err.ExceptionInfo.ToString());
                    }
                }
                XmlSerializer serializer = new XmlSerializer(records.GetType());
                StringWriter sw = new StringWriter();
                serializer.Serialize(sw, records);
                DataSet ds = new DataSet();
                StringReader reader = new StringReader(sw.ToString());

                ds.ReadXml(reader);
                DataTable dt = ds.Tables[0];
                System.Data.DataColumn createdByColumn = new System.Data.DataColumn("CREATED_BY_USER", typeof(System.Guid));
                System.Data.DataColumn fileDetailsColumn = new System.Data.DataColumn("ICDSODAFileDetails_ID", typeof(System.Int32));
                createdByColumn.DefaultValue = new Guid(appID);
                dt.Columns.Add(createdByColumn);
                fileDetailsColumn.DefaultValue = FileID;
                dt.Columns.Add(fileDetailsColumn);
                if (IcdOdaWaiverHelper.BulkLoadToStg(log,ThreadId, dt))
                {
                    //initaite the next process
                    if (IcdOdaWaiverHelper.ProcesssStgToReg(log, ThreadId, FileID))
                    {
                        //all good                        
                    }
                }
                else
                {
                    log.CreateLogEntry(String.Format("Failed to Bulk Load File: {0}", fileName));
                }
            }
            catch (Exception ex)
            {
                IcdOdaWaiverHelper.LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }

        public void SendOutEmails(Logging log, Guid threadId)
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);

            try
            {
                log.CreateLogEntry("Checking for providers that need notifications sent");
                int odaNoticeTotal = 0;
                int odaProvNoticeTotal = 0;
                DataSet odaNoticeDS = IcdOdaWaiverHelper.SelectODATerminationRegIDs(log, threadId, 2);
                //if (ObjectControllerHelper.HasRows(odaNoticeDS))
                //{
                    //odaProvNoticeTotal += SendEmail(odaNoticeDS, "EMAIL_TEMPLATE_ODA_PROVIDER_TERMINATION");
                //}
                if (ObjectControllerHelper.HasRows(odaNoticeDS))
                {
                    odaNoticeTotal += SendEmail(odaNoticeDS, "EMAIL_TEMPLATE_ODA_TERMINATION", log, ThreadId);
                }
                log.CreateLogEntry(String.Format("Total Emails sent to Providers {0}", odaProvNoticeTotal.ToString()));
                log.CreateLogEntry(String.Format("Total Emails sent to Notification group {0}", odaNoticeTotal.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }


        private int SendEmail(DataSet ds, string Template_Name, Logging log, Guid threadId)
        {
            int total = 0;
            if (ObjectControllerHelper.HasRows(ds))
            {
                //Notification notify = new Notification(this.ThreadId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int regID = ObjectControllerHelper.GetInt("REG_ID", row);
                    try
                    {
                        string subject = "";
                        string bccList = "";
                        string recipients = "";

                        if (Template_Name.Equals("EMAIL_TEMPLATE_ODA_PROVIDER_TERMINATION"))
                        {
                            GetRecipients(regID, out recipients, out bccList);
                            if (!string.IsNullOrEmpty(recipients))
                            {
                                EMailNotification enotify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                                if (enotify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                                {
                                    IcdOdaWaiverHelper.UpdateODATerminationProvidersRegIDs(log, threadId, regID, null, null, true, null);
                                    total += 1;
                                }
                                else
                                {
                                    log.CreateLogEntry("Reg Id: " + regID + " - problems sending notification email to Provider", Logging.LogPriority.Error);
                                }
                            }
                        }
                        else if (Template_Name.Equals("EMAIL_TEMPLATE_ODA_TERMINATION"))
                        {
                            EMailNotification notify = new EMailNotification(string.Empty, subject, recipients, this.ThreadId, bccList);
                            if (notify.SendWorkFlowEngineNotification(regID.ToString(), Template_Name))
                            {
                                IcdOdaWaiverHelper.UpdateODATerminationProvidersRegIDs(log, threadId, regID, null, true, null, null);
                                total += 1;
                            }
                            else
                            {
                                log.CreateLogEntry("Reg Id: " + regID + " - problems sending notification email", Logging.LogPriority.Error);
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    { // not rethrowing so we can send emails to other providers 
                        log.CreateLogEntry("Failed to send Notification for Reg Id: " + regID + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                    }
                }
            }
            return total;
        }

        public void TerminateRegistrations(Logging log, Guid threadId)
        {
            // Create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            try
            {
                log.CreateLogEntry("Starting termination for Providers");
                int total = 0;
                
                DataSet termDS = IcdOdaWaiverHelper.SelectODATerminationRegIDs(log, threadId, 1);

                if (ObjectControllerHelper.HasRows(termDS))
                {
                    foreach (DataRow row in termDS.Tables[0].Rows)
                    {
                        int reg_id = ObjectControllerHelper.GetInt("REG_ID", row);
                        DateTime dtt = ObjectControllerHelper.GetDateTime("TERM_DATE", row);

                        try
                        {
                            IcdOdaWaiverHelper.TerminateProviderByRegID(log, threadId, reg_id, dtt);
                            IcdOdaWaiverHelper.UpdateODATerminateProvidersByRegIDs(log, threadId, reg_id, true);
                            log.CreateLogEntry("End Terminate RegID:" + reg_id.ToString());
                            total++;
                        }
                        catch (Exception ex)
                        { // not rethrowing so we can disenroll other providers 
                            log.CreateLogEntry("Failed to terminate provider with Reg Id: " + reg_id + " Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                        }
                    }
                }
                log.CreateLogEntry(String.Format("Total providers terminated {0}", total.ToString()));
            }
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }


        private void GetRecipients(int regId, out string recList, out string bccList)
        {
            string rtn = string.Empty;
            recList = string.Empty;
            bccList = string.Empty;
            try
            {
                //int sendToTypeID = Constants.SendToTypeID.StateAdmin;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_PROVIDER", parameters, "EmailRecipients");
                recList = AddEmail(recList, ds, "CONTACT_EMAIL_ADDRESS");
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, regId, false));
                ds = DataAccess.ExecuteStoredProcedure("usp_SelectREG_Users", parameters, "EmailRecipients");
                recList = AddEmail(recList, ds, "Email");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Reg Id: " + regId.ToString() + ", GetRecipients - " + ex.Message, Logging.LogPriority.Error);
            }
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

    }
}
