using MAXIMUS.Core.Libraries;
using MMSWebControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using static MAXIMUS.Core.Libraries.Constants;
using Corp.Core.Libraries;

namespace MAXIMUS.DataExchange.PDMS
{
    public class PPRRNotice : BaseJob, IJob
    {
        const string PPRRJobId = "9BFC85A9-C276-40EA-A74E-423679ECBC74";
        string adminUser = "5C8EEA1D-B312-41A7-999C-DEDBCCA3688D";
        static string InBoundPath = AppSettings.Get("PPRRInBoundPath", string.Empty);
        static string OutBoundPath = AppSettings.Get("PPRROutBoundPath", string.Empty);
        string FileExtractPath = InBoundPath + @"\Extract";
        string CompletedPath = InBoundPath + @"\Completed";
        string ProcessSuccessFolderPath = InBoundPath + @"\Processed-Success";
        string ProcessFailureFolderPath = InBoundPath + @"\Processed-Failure";

        private Logging log = null;

        public PPRRNotice(Guid threadId) : base(threadId)
        {
            this.ThreadId = threadId;
        }

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse(PPRRJobId));
        }

        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            ProcessPPRReports(jobGuid);
        }

        private void ProcessPPRReports(string jobGuid)
        {
            log = new Logging(this.ThreadId);

            log.CreateLogEntry("PPRR Job: Job has started", Logging.LogPriority.Information);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                InBoundPath = @"C:\Temp\CR073\Inbound";
                OutBoundPath = @"C:\Temp\CR073\Outbound";

                FileExtractPath = InBoundPath + @"\Extract";
                CompletedPath = InBoundPath + @"\Completed";
                ProcessSuccessFolderPath = InBoundPath + @"\Processed-Success";
                ProcessFailureFolderPath = InBoundPath + @"\Processed-Failure";
            }

            try
            {
                //Create folders for Processed (success and failure) files
                if (!Directory.Exists(ProcessSuccessFolderPath))
                {
                    log.CreateLogEntry("PPRR Job: Creating the Process Success folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(ProcessSuccessFolderPath);
                }
                if (!Directory.Exists(ProcessFailureFolderPath))
                {
                    log.CreateLogEntry("PPRR Job: Creating the Process Failure folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(ProcessFailureFolderPath);                
                }
                if (!Directory.Exists(CompletedPath))
                {
                    log.CreateLogEntry("PPRR Job: Creating the Completed folder", Logging.LogPriority.Information);
                    Directory.CreateDirectory(CompletedPath);
                }

                DirectoryInfo inBoundDirectory = new DirectoryInfo(InBoundPath);


                log.CreateLogEntry("PPRR Job: Getting the zip files", Logging.LogPriority.Information);
                FileInfo[] unProcessedFiles = inBoundDirectory.GetFiles("*.zip");

                if (unProcessedFiles.Length > 0)
                {
                    log.CreateLogEntry("PPRR Job: Total unprocessed zip files count: "+unProcessedFiles.Length, Logging.LogPriority.Information);

                    //Create a folder for extracted files
                    if (!Directory.Exists(FileExtractPath))
                    {
                        log.CreateLogEntry("PPRR Job: Creating the folder for extracted files", Logging.LogPriority.Information);
                        Directory.CreateDirectory(FileExtractPath);
                    }

                    DirectoryInfo extractDirectory = new DirectoryInfo(FileExtractPath);

                    log.CreateLogEntry("PPRR Job: Looping through the zip files", Logging.LogPriority.Information);
                    foreach (FileInfo zipfile in unProcessedFiles)
                    {
                        List<string> successFilesList = new List<string>();
                        Dictionary<string, string> errorFilesList = new Dictionary<string, string>();

                        log.CreateLogEntry("PPRR Job: Empty the Extract folder", Logging.LogPriority.Information);
                        EmptyDirectory(FileExtractPath);

                        //  unzip files
                        FileCompression fc = new FileCompression(this.ThreadId);
                        log.CreateLogEntry("PPRR Job: Started unzipping the file "+ zipfile.FullName, Logging.LogPriority.Information);
                        fc.UnzipFile(zipfile.FullName, FileExtractPath);

                        log.CreateLogEntry("PPRR Job: Looping through the individual files", Logging.LogPriority.Information);
                        foreach (FileInfo file in extractDirectory.GetFiles().Where(x => !x.Name.ToUpper().Contains("MANIFEST")))
                        {
                            string medicaidID = string.Empty;
                            bool replacementReport = CheckIfReplacementFile(file.Name);
                            string providerName = string.Empty;
                            byte[] fileBytes = null;

                            log.CreateLogEntry("PPRR Job: Validate file: "+file.FullName, Logging.LogPriority.Information);
                            string errorMessage = ValidateFile(file, out medicaidID, out providerName, out fileBytes);

                            if (string.IsNullOrWhiteSpace(errorMessage))
                            {
                                log.CreateLogEntry("PPRR Job: Validation successful for the file: "+file.FullName, Logging.LogPriority.Information);

                                log.CreateLogEntry("PPRR Job: Upload the file: "+file.FullName, Logging.LogPriority.Information);
                                //Upload the file.
                                bool flagFileUploadSuccess = UploadDocument(fileBytes, file.Name, medicaidID, replacementReport);

                                if (flagFileUploadSuccess)
                                {
                                    //Add an entry to the database to capture the file processed basic information
                                    log.CreateLogEntry("PPRR Job: Recording file process details to database, file name: " + file.FullName, Logging.LogPriority.Information);
                                    var flagSuccess = RecordFileProcessDetailsToDatabase(file.Name, zipfile.Name, true, null);

                                    if (flagSuccess)
                                    {
                                        successFilesList.Add(file.Name);
                                        
                                        log.CreateLogEntry("PPRR Job: Moving the file to Success folder, file name: " + file.FullName, Logging.LogPriority.Information);

                                        if (replacementReport)
                                            file.MoveTo(ProcessSuccessFolderPath + "\\" + file.Name.Replace(file.Extension, "") + "_REPLACEMENT" + file.Extension);
                                        else
                                            file.MoveTo(ProcessSuccessFolderPath + "\\" + file.Name);
                                    }
                                    else
                                    {
                                        log.CreateLogEntry("PPRR Job: Moving the file to Failure folder, file name: " + file.FullName, Logging.LogPriority.Information);
                                        errorFilesList.Add(file.Name, "Error recording the file process details to the database");
                                        file.MoveTo(ProcessFailureFolderPath + "\\" + file.Name);
                                    }
                                }
                                else
                                {
                                    errorFilesList.Add(file.Name, "Error Uploading the document to PNM");
                                    file.MoveTo(ProcessFailureFolderPath + "\\" + file.Name);
                                }
                            }
                            else
                            {
                                log.CreateLogEntry("PPRR Job: Validation failed for the file: "+file.FullName, Logging.LogPriority.Information);
                                log.CreateLogEntry("PPRR Job: Recording file process details to database, file name: " + file.FullName, Logging.LogPriority.Information);
                                RecordFileProcessDetailsToDatabase(file.Name, zipfile.Name, false, errorMessage);
                                errorFilesList.Add(file.Name, errorMessage);
                                log.CreateLogEntry("PPRR Job: Moving the file to Failure folder, file name: " + file.FullName, Logging.LogPriority.Information);
                                file.MoveTo(ProcessFailureFolderPath + "\\" + file.Name);
                            }
                        }

                        log.CreateLogEntry("PPRR Job: Moved the zip file to Completed folder, zip filename: " + zipfile.FullName, Logging.LogPriority.Information);
                        //Moved the zip file to Completed folder
                        zipfile.MoveTo(CompletedPath + "\\" + zipfile.Name);

                        log.CreateLogEntry("PPRR Job: Create response file and add entries", Logging.LogPriority.Information);
                        //Create response file and add entry
                        string responseFileName = OutBoundPath + "\\" + zipfile.Name.Replace("PPRR", "PPRR.ManifestReturn").Replace(zipfile.Extension, ".txt");
                        File.AppendAllLines(responseFileName, new[] { "Total Files Process: " + (successFilesList.Count + errorFilesList.Count) });
                        File.AppendAllLines(responseFileName, new[] { "Files Successfully Processed: " + successFilesList.Count });
                        File.AppendAllLines(responseFileName, new[] { "Files with errors: " + errorFilesList.Count });
                        File.AppendAllLines(responseFileName, new[] { "" });
                        for (int i = 0; i < successFilesList.Count; i++)
                        {
                            string text = string.Format("{0}. Filename: {1}", (i + 1), successFilesList[i]);
                            text += System.Environment.NewLine + "Status: SUCCESS";

                            File.AppendAllLines(responseFileName, new[] { text });
                        }
                        for (int i = 0; i < errorFilesList.Count; i++)
                        {
                            var errorItem = errorFilesList.ElementAt(i);
                            string text = string.Format("{0}. Filename: {1}", (i + 1), errorItem.Key);
                            text += System.Environment.NewLine + "Status: FAILED. Error Message: " + errorItem.Value;

                            File.AppendAllLines(responseFileName, new[] { text });
                        }
                        log.CreateLogEntry("PPRR Job: Response file created successfully", Logging.LogPriority.Information);
                    }

                    EmptyDirectory(FileExtractPath);
                    extractDirectory.Delete();
                }
                log.CreateLogEntry("PPRR Job: Job completed successfully", Logging.LogPriority.Information);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to process PPR Reports. Exception Message: "
                                + ex.Message + " Exception Stack: " + ex.StackTrace, Logging.LogPriority.Error);
            }
        }

        private string ValidateFile(FileInfo file, out string medicaidID, out string providerName, out byte[] fileBytes)
        {
            string errorMessage = string.Empty;
            try
            {
                string vendorID = "V002";
                string reportFormatType = "PPRR";
                string reportSubType = "PPRR";
                medicaidID = string.Empty;
                providerName = string.Empty;
                DateTime dt;
                fileBytes = null;

                string[] fileNameArray = file.Name.Replace(file.Extension, "").Split('.');

                //Check the file extension
                if (!file.Extension.ToUpper().Contains("XLS"))
                    return "The PPR Report File name extension must be XLS OR XLSX";

                //Check the vendor prefix.
                if (fileNameArray[0] == null || fileNameArray[0].ToUpper() != vendorID)
                    return "Vendor prefix must be V002";

                //Check the report format Type.
                if (fileNameArray[1] == null || fileNameArray[1].ToUpper() != reportFormatType)
                    return "Invalid PPR Report Type";

                //Check the report Sub type.
                if (fileNameArray[2] == null || fileNameArray[2].ToUpper() != reportSubType)
                    return "Invalid PPR Report Type";

                //Check the effective date format.
                if (fileNameArray[3] == null || !DateTime.TryParseExact(fileNameArray[3], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return "Invalid Effective Date format";

                //Check the end date format .
                if (fileNameArray[4] == null || !DateTime.TryParseExact(fileNameArray[4], "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    return "Invalid End Date format";

                if (fileNameArray[6] != null)
                {
                    medicaidID = fileNameArray[6];
                    //Get provider name.
                    providerName = GetProvderName(medicaidID);
                }
                if (fileNameArray[6] == null || string.IsNullOrWhiteSpace(providerName))
                    return "Provider Medicaid ID does not exist";

                fileBytes = File.ReadAllBytes(FileExtractPath + "\\" + file.Name);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Error in ValidateFile method. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }

            return errorMessage;
        }

        private void EmptyDirectory(string directoryPath)
        {
            DirectoryInfo extractDirectory = new DirectoryInfo(directoryPath);
            foreach (FileInfo file in extractDirectory.GetFiles())
            {
                file.Delete();
            }
        }

        public bool UploadDocument(byte[] documentBytes, string documentName, string medicaidID, bool replacementReport = false)
        {
            string request = string.Empty;
            try
            {
                int documentTypeId = GetDocumentType(DocumentType.PotentiallyPreventableReadmissionsId);

                int documentID = StoreDocumentRecord(documentName, documentName, "Potentially Preventable Readmissions Report");

                if (documentID > 0)
                {
                    int attachmnetXrefId = StoreDocumentAttachmnetXref(documentID, null, documentTypeId
                                            , RetrieveReportTypeID.POTENTIALLYPREVENTABLEREADMISSIONS);

                    bool flagSuccess = StoreDocumentAttachmentIdentifiers(DocumentXREFType.PID, medicaidID, documentID);

                    if (flagSuccess)
                    {
                        log.CreateLogEntry("PPRR Job: Uploading file to Onbase, file name: "+documentName, Logging.LogPriority.Information);
                        //UpdateOnbase.
                        SendUpdateToOnbase(documentName, documentID, documentBytes);

                        log.CreateLogEntry("PPRR Job: Saving the file to file store, file name: "+documentName, Logging.LogPriority.Information);
                        SaveFileToFileStore(documentName, documentID, documentBytes);

                        log.CreateLogEntry("PPRR Job: Sending PPR Report Notice", Logging.LogPriority.Information);
                        SendPPRNewReportNotice(medicaidID, GetProvderName(medicaidID), replacementReport);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                string logmessage = string.Format("PPRR Job: Exception while storing the Data in SendAttachment  {0}", ex.ToString());

                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
            }
            return false;
        }

        private int StoreDocumentAttachmnetXref(int documentID, string SITransactionKey, int documentTypeId, int RetrieveReportTypeId = 0, int msgID = 0)
        {
            string headerId = string.Empty;
            int retVal = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("NOTES", DbType.String, string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_RECEIVED_DATE", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("SITRANSACTIONKEY", DbType.String, SITransactionKey, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_ID", DbType.Int32, documentTypeId, false));
                parameters.Add(SqlParms.CreateParameter("RetrieveReport_Type_ID", DbType.Int32, RetrieveReportTypeId, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("STG_MESSAGE_HEADER_ID", DbType.Int32, msgID, false));
                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_ATTACHMENT_XREF", parameters));

            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to insert record into the Document_XREF_Attachment table. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }

            return retVal;
        }

        private int StoreDocumentRecord(string name, string filename, string desc)
        {
            int retVal = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));

                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT", parameters));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to store the Document. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }

            return retVal;
        }

        private void SendUpdateToOnbase(string fileName, int documentId, byte[] attachmentBytes)
        {
            try
            {
                Encryption encryption = new Encryption();
                var encryptedBytes = encryption.EncryptRijndael(attachmentBytes);

                OnBaseInterface onBaseInterface = new OnBaseInterface();
                onBaseInterface.SubmitFile(documentId, encryptedBytes, fileName);
            }
            catch (Exception ex)
            {
                string logmessage = string.Format("PPRR Job: Exception while sending SendAttachment file to OnBase:{0},{1},{2}", fileName, documentId, ex.ToString());
                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
            }
        }

        private void SaveFileToFileStore(string fileName, int documentID, byte[] attachmentData64Binary)
        {
            try
            {
                string DestinationPath = AppSettings.Get("FileStorePath", string.Empty);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    DestinationPath = @"C:\Temp\CR073";
                }
                string totalFileName = Path.Combine(DestinationPath + fileName);
                File.WriteAllBytes(totalFileName, attachmentData64Binary);

            }
            catch (Exception ex)
            {
                string logmessage = string.Format("PPRR Job: Exception while saving the file to filestore folder. File:{0},{1},{2}", fileName, documentID, ex.ToString());
                log.CreateLogEntry(logmessage, Logging.LogPriority.Error);
            }
        }

        private int GetDocumentType(string documentType)
        {

            int retVal = 0;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE_CODE", DbType.String, documentType, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelectDOCUMENT_TYPE_ByCode", parameters, "DocumentTypes");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = Convert.ToInt32(dsType.Tables[0].Rows[0]["DOCUMENT_TYPE_ID"].ToString());
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to execute the stored procedure 'usp_SelectDOCUMENT_TYPE_ByCode'. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }

            return retVal;
        }

        private string GetProvderName(string medicaidID)
        {
            string retVal = string.Empty;
            DataSet dsType = new DataSet();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GRPMedicaid_ID", DbType.String, medicaidID, false));
                dsType = DataAccess.ExecuteStoredProcedure("usp_SelectRegistrationsByMedicaidID", parameters, "ProviderInfo");
                if (dsType != null && dsType.Tables[0] != null && dsType.Tables[0].Rows.Count > 0)
                    retVal = dsType.Tables[0].Rows[0]["Name"].ToString();
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to execute the stored procedure 'usp_SelectRegistrationsByMedicaidID'. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            }

            return retVal;
        }

        private bool CheckIfReplacementFile(string fileName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FileName", DbType.String, fileName, false));
                return Convert.ToBoolean(DataAccess.ExecuteScalar("usp_CheckIfFileIsReplacementPPRR", parameters));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to execute the stored procedure 'usp_CheckIfFileIsReplacementPPRR'. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
                throw ex;
            }
        }

        private bool StoreDocumentAttachmentIdentifiers(int xrefTypeID, string IndexId, int documentID)
        {
            bool flagSuccess = false;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, documentID, false));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_XREF_TYPE_ID", DbType.Int32, xrefTypeID, false));
                parameters.Add(SqlParms.CreateParameter("INDEXID", DbType.String, IndexId.Trim(), false));
                parameters.Add(SqlParms.CreateParameter("CREATED_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_MODIFIED_USER", DbType.String, adminUser, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, adminUser, false));

                var retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT_INDEX", parameters));

                if (retVal > 0) flagSuccess = true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to store Document Attachment Identifiers. Exception Message: " + ex.Message + " Exception Stack = "
                                 + ex.StackTrace, Logging.LogPriority.Error);
            }

            return flagSuccess;
        }

        private void SendPPRNewReportNotice(string medicaidID, string providerName, bool replacementReport = false)
        {
            Dictionary<string, object> fields = new Dictionary<string, object>();
            DateTime now = DateTime.Now;
            string body = string.Empty,
                emailFrom = AppSettings.Get("SmtpFromEmailAddress", string.Empty),
                emailTo = string.Empty,
                subject = "New Medicaid Potentially Preventable Readmissions (PPR) Report",
                templateName = "PPRReportNotice.txt",
                templatesDirectory = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);

            try
            {
                fields.Add("HospitalName", providerName);
                fields.Add("MedicaidProviderID", medicaidID);

                if (replacementReport)
                {
                    subject = "Replacement Medicaid Potentially Preventable Readmissions (PPR) Report";
                    fields.Add("ReportType", "replacement");
                }
                else
                {
                    fields.Add("ReportType", "new");
                }

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    emailTo = "naveengattoju@maximus.com";
                    templatesDirectory = @"C:\GitRepository\ohpnm-src\PDMS\ProviderDataManagementSystemService\Documents";
                }

                EMailNotification notify = new EMailNotification(body, subject, emailTo);
                body = notify.SendActualNotification(templatesDirectory + "\\" + templateName, fields, true);

                // Create the communicaton event and email
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CommunicationEventType", DbType.String, "PROVIDER EMAIL OUT", true));
                string comTypeID = DataAccess.ExecuteScalar("sp_SelectCommunicationEventTypes", parameters);

                // create parameters objects and fill with values
                parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_TYPE_ID", DbType.Int32, Convert.ToInt32(comTypeID), false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_VALUE", DbType.String, string.Empty, false));
                parameters.Add(SqlParms.CreateParameter("COMMUNICATION_EVENT_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_FROM", DbType.String, emailFrom, false));
                parameters.Add(SqlParms.CreateParameter("EMAIL_TO", DbType.String, emailTo, false));
                parameters.Add(SqlParms.CreateParameter("SUBJECT", DbType.String, subject, false));
                parameters.Add(SqlParms.CreateParameter("BODY", DbType.String, body, false));
                parameters.Add(SqlParms.CreateParameter("TEMPLATE_NAME", DbType.String, templateName, false));
                parameters.Add(SqlParms.CreateParameter("KEY_VALUE_PAIR", DbType.String, ObjectControllerHelper.GetKeyValueString(fields), false));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, 0, false));
                parameters.Add(SqlParms.CreateParameter("USER_ID", DbType.Guid, new Guid("00000000-0000-0000-0000-000000000000"), false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, Constants.appPDMSDataExchangeUserId, true));
                parameters.Add(SqlParms.CreateParameter("isEmailSent", DbType.Boolean, notify.isEmailSent, true));
                parameters.Add(SqlParms.CreateParameter("log_message", DbType.String, notify.log_message, true));
                string comId = DataAccess.ExecuteScalar("sp_insertCOMMUNICATIONEVENT_AND_EMAIL", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Error encountered during SendEmail" + ex.Message, Logging.LogPriority.Error);
            }
        }

        private bool RecordFileProcessDetailsToDatabase(string fileName, string zipFileName, bool processSuccess, string errorMessage = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("File_Name", DbType.String, fileName, false));
                parameters.Add(SqlParms.CreateParameter("Zip_File_Name", DbType.String, zipFileName, false));
                parameters.Add(SqlParms.CreateParameter("Processed_Date", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("Processed_Successfully", DbType.Boolean, processSuccess, false));
                parameters.Add(SqlParms.CreateParameter("Error_Message", DbType.String, errorMessage, true));

                var retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertPPRR_PROCESS_RESULT", parameters));

                if (retVal > 0)
                    return true;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("PPRR Job: Failed to Record File Process Details To Database for the file: "+fileName
                                    +". Exception Message: " + ex.Message + " Exception Stack = "
                                    + ex.StackTrace, Logging.LogPriority.Error);
            }

            return false;
        }
    }
}