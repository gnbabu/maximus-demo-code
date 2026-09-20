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
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS
{
    public class DelegateAffiliationInterface : BaseJob, IJob
    {
        private Logging log = null;

        public DelegateAffiliationInterface(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "61854E39-7631-43C3-8E53-018A7A0A7BB0";

        override public void ExecuteJob()
        {
            // Default Job - Delegate Affiliation File Processing
            this.ExecuteJob(Guid.Parse("61854E39-7631-43C3-8E53-018A7A0A7BB0"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    ProcessDelegateFile();
                    break;
            }
        }

        public void ProcessDelegateFile()
        {
            // create log object
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                // create log entry
                log.CreateLogEntry(Constants.LogString.Delegate_Affiliate_Initiate);
                string fileName = string.Empty;
                int docID = 0;

                string testingEnabled = "false";

                // create local path if it does not exist


                // if testing enabled
                if (testingEnabled == bool.TrueString.ToLower())
                {

                }
                else
                {
                    DataTable dt = DelegateAffiliationHelper.SelectAllNotProcessedDelegateFiles();
                    foreach (DataRow row in dt.Rows)
                    {
                        docID = ObjectControllerHelper.GetInt("DOCUMENT_ID", row);
                        fileName = ObjectControllerHelper.GetString("DELEGATE_FILE_NAME", row);
                        
                        this.LoadDelegateAffiliationFile(log, fileName, docID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry(Constants.LogString.ProcessingComplete);
        }

        public void SendOutTransactionPayloads(Logging log, Guid threadId, int fileID)
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
                DataSet dsTrans = DelegateAffiliationHelper.SelectICDODAServicesTransactions(log, threadId, fileID);
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

                        try
                        {
                            SendTerminationTransactionPayload stp = new SendTerminationTransactionPayload();
                            stp.SendTransaction(transactionID, transactionType);
                            DelegateAffiliationHelper.UpdateICDODAServicesTransactions(log, threadId, regID, fileID, mId, true);
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
            catch (Exception ex)
            {
                if (System.Diagnostics.Debugger.IsAttached) System.Diagnostics.Debugger.Break();
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
        }



        private void LoadDelegateAffiliationFile(Logging log, string fileName, int docID)
        {
            try
            {
                DataTable dt = DelegateAffiliationHelper.GetDataTableFromExcel(fileName, docID, log, ThreadId);
                System.Data.DataColumn fileDetailsColumn = new System.Data.DataColumn("DelegateFileDetails_ID", typeof(System.Int32));
                dt.Rows.Remove(dt.Rows[0]);

                fileDetailsColumn.DefaultValue = docID;
                dt.Columns.Add(fileDetailsColumn);
                if (DelegateAffiliationHelper.BulkLoadToStg(log, ThreadId, dt, docID))
                {
                    ProcessDelegateAffiliationFile(log, ThreadId, docID, fileName);
                    SendOutEmails(log, ThreadId);
                    SendOutTransactionPayloads(log, ThreadId, docID);
                }
                else
                {
                    log.CreateLogEntry(String.Format("Failed to Bulk Load File: {0}", fileName));
                }

                //DataTable dt2 = DelegateAffiliationHelper.SelectDelegateAffiliationsRecords(log, ThreadId, FileID);
                //DelegateAffiliationHelper.ExportToExcel2(dt2, string.Format("{0}\\Response\\{1}",file.Directory, file.Name));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while processing the file {0}", ex.ToString()));
                //DelegateAffiliationHelper.LogFileRecordError(log, engine.LineNumber.ToString(), fileName, Constants.LogString.FileLoadFailure + ex.Message);
            }

            // create log entry
            log.CreateLogEntry(String.Format("Return provider {0} file load complete", fileName));
        }

        private void ProcessDelegateAffiliationFile(Logging log, Guid ThreadId, int docID, string fileName)
        {
            DelegateAffiliationHelper.ProcesssStgToReg(log, ThreadId, docID);
            DelegateAffiliationHelper.UpdateDelegateFileRecordByDocID(log, ThreadId, docID, Constants.DelegateDocumentUploadStatus.Complete);
            SaveResponseFile(log, ThreadId, docID, fileName);

        }

        public void SaveResponseFile(Logging log, Guid threadId, int docID, string fileName)
        {
            try
            {
                string DestinationPath = Path.Combine(InfoAccess.GetAppSettingFromDB("DelegateAffFilePath", string.Empty));
                string newFileName = RenameFileMethodNew(DestinationPath, fileName);
 
                newFileName = Path.ChangeExtension(newFileName, ".xlsx");

                string fullFilePath = Path.Combine(DestinationPath, newFileName);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    fullFilePath = Path.Combine(@"C:\Projects\Deployments\PDMS\PNM_DEV\FileStoreLocal\" + newFileName);
                    DestinationPath = Path.Combine(@"C:\Projects\Deployments\PDMS\PNM_DEV\FileStoreLocal\");
                }

                DataTable dt2 = DelegateAffiliationHelper.SelectDelegateAffiliationsRecords(log, ThreadId, docID);
                DelegateAffiliationHelper.ExportToExcel2(dt2, newFileName, @DestinationPath);
                int newDocID = DelegateAffiliationHelper.SaveNewUploadedFileByDocID(log, threadId, fileName, newFileName, docID);
                if (newDocID != 0)
                {
                    byte[] fileBytes = RetrieveFile(fullFilePath);
                    SendToCMS(newDocID, fileBytes, newFileName);
                }
                //save the file to local or onbase with new responses
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while Saving the file {0}", ex.ToString()));
                DelegateAffiliationHelper.UpdateDelegateFileRecordByDocID(log, threadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, DelegateDocumentUploadErrorCodes.ERR_FILE_SAVE.ToString());
            }
        }

        public byte[] RetrieveFile(string filePath)
        {
            byte[] fileBytes = null;
            fileBytes = File.ReadAllBytes(filePath);
            return fileBytes;
        }


        private void SendToCMS(int docID, byte[] fileBytes, string fileName)
        {
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            onBaseInterface.SubmitFile(docID, fileBytes, fileName);
        }

        private string RenameFileMethod(string dir, string input)
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

        private string RenameFileMethodNew(string dir, string input)
        {
            string rtn = input;

            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + DateTime.Now.Ticks.ToString();
            else rtn = input.Substring(0, pos) + "_" + DateTime.Now.Ticks.ToString() + input.Substring(pos);

            return rtn;
        }

        public void SendOutEmails(Logging log, Guid threadId)
        {
            try
            {
               
                int noticesSent = 0;
                bool noticeSent = false;
                string username = string.Empty;
                string email = string.Empty;
                string fileName = string.Empty;
                int docID = 0;
                DataSet delegatesNoticeDS = DelegateAffiliationHelper.SelectDelegateFileEmailstoSend(log, threadId);
                if (ObjectControllerHelper.HasRows(delegatesNoticeDS))
                {
                    foreach (DataRow row in delegatesNoticeDS.Tables[0].Rows)
                    {
                        username = ObjectControllerHelper.GetString("UserName", row);
                        email = ObjectControllerHelper.GetString("Email", row);
                        docID = ObjectControllerHelper.GetInt("DOCUMENT_ID", row);
                        fileName = ObjectControllerHelper.GetString("DELEGATE_FILE_NAME", row);
                        noticeSent = SendDelegateFileProcessedNotice(username, email, fileName);
                        if (noticeSent)
                        {
                            DelegateAffiliationHelper.UpdateDelegateFileRecordByDocID(log, threadId, docID, null, true);
                            noticesSent++;
                        }
                    }
                }
                log.CreateLogEntry(String.Format("Total Emails sent to Notification group {0}", noticesSent.ToString()));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while sending out Emails {0}", ex.ToString()));
            }
        }

        public bool SendDelegateFileProcessedNotice(string username, string email, string fileName)
        {
            bool noticeSent = false;
            string subject = Constants.DelegateAffiliationNotice.subject;
            string templateFile = Constants.DelegateAffiliationNotice.emailTemplateName;

            try
            {

                Notification n = new Notification();
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("USERNAME", username);
                fields.Add("FileName", fileName);
                string body = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

                string pwdTemplateContent = null;
                using (StreamReader reader = new StreamReader(templateActualPath + templateFile))
                {
                    pwdTemplateContent = reader.ReadToEnd();
                }
                body = n.ParseEmailBody(pwdTemplateContent, fields);
                EMailNotification notify = new EMailNotification(body, subject, email);
                body = notify.SendActualNotification(templateActualPath + templateFile, fields, true);

                noticeSent = true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while sending out Emails {0}", ex.ToString()));
            }
            return noticeSent;
        }


    }
}
