using Corp.Core.Libraries;
using DocumentFormat.OpenXml.Office2010.Word;
using MAXIMUS.Core.Libraries;
using MMSWebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.PeerToPeer;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS.ProviderAgentBulkUpload
{
    public class ProviderAgentBulkUploadProcessor : BaseJob, IJob
    {
        private Logging log = null;

        public ProviderAgentBulkUploadProcessor(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        private readonly string appID = "B1524615-06D6-4657-8942-E4311B9274EA";

        override public void ExecuteJob()
        {
            this.ExecuteJob(Guid.Parse("B1524615-06D6-4657-8942-E4311B9274EA"));
        }
        override public void ExecuteJob(Guid jobId)
        {
            string jobGuid = jobId.ToString().ToUpper();
            switch (jobGuid)
            {
                default:
                    ProcessAgentUpload();
                    break;
            }
        }

        public void ProcessAgentUpload()
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(new Guid(appID), logMsg);
            try
            {
                // create log entry
                log.CreateLogEntry("Start Provider Agent Bulk Upload Processor Job");
                string fileName = string.Empty;
                int docID = 0;
                int onBaseDocID = 0;
                bool sendEmail = false;
                string recipientEmail = string.Empty, contactName = string.Empty;
               
                DataSet dsFiles = DataAccess.ExecuteStoredProcedure("usp_SelectAgentFilesToProcess", "dsAgentFilesToProcess"); 
                if (Methods.HasRows(dsFiles))
                {
                    foreach (DataRow row in dsFiles.Tables[0].Rows)
                    {
                        docID = ObjectControllerHelper.GetInt("DOCUMENT_ID", row);
                        fileName = ObjectControllerHelper.GetString("AGENT_FILE_NAME", row);
                        onBaseDocID = ObjectControllerHelper.GetInt("ONBASE_DOCUMENT_ID", row);
                        sendEmail = ObjectControllerHelper.GetBool("EMAIL_SENT", row);
                        recipientEmail = ObjectControllerHelper.GetString("Email", row);
                        contactName = ObjectControllerHelper.GetString("CONTACT_NAME", row);
                        LoadAndProcessFile(log, fileName, docID, onBaseDocID, sendEmail, recipientEmail, contactName);
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(this.ThreadId, ex);
            }
            log.CreateLogEntry("Provider Agent Bulk Upload Processor Job Complete");
        }

        public void LoadAndProcessFile(Logging log, string fileName, int docID, int onBaseDocID, bool sendEmail, string recipientEmail, string contactName)
        {           
            //Read data from excel file uploaded
            DataTable dt = GetDataTableFromExcel(fileName, docID, onBaseDocID, log, ThreadId);
            System.Data.DataColumn fileDetailsColumn = new System.Data.DataColumn("AGENT_BULK_UPLOAD_DOCUMENT_ID", typeof(System.Int32));
            System.Data.DataColumn fileDetailsColumn1 = new System.Data.DataColumn("RESPONSE", typeof(System.String));
            dt.Rows.Remove(dt.Rows[0]);

            fileDetailsColumn.DefaultValue = docID;
            dt.Columns.Add(fileDetailsColumn);

            fileDetailsColumn1.DefaultValue = string.Empty;
            dt.Columns.Add(fileDetailsColumn1);
            if (dt.Rows.Count > 0)
            {
                if (BulkLoadToStg(log, ThreadId, dt, docID))
                {
                    ProcessAgentUploadFile(log, ThreadId, docID);
                    SaveResponseFile(log, ThreadId, docID, fileName);                   
                    
                    //if (sendEmail)
                    //{
                    //    if (!string.IsNullOrEmpty(recipientEmail))
                    //    {
                    //        SendUploadProcessedEmail(log, ThreadId, docID, recipientEmail, fileName, contactName);
                    //    }
                    //    else
                    //    {
                    //        log.CreateLogEntry(String.Format("Provider Agent Bulk Upload recipient email Empty for DOC ID:{0}",docID));
                    //    }
                    //}
                }
                else
                {
                    log.CreateLogEntry(String.Format("Provider Agent Bulk Upload Failed to Bulk Load File: {0}", fileName));
                    UpdateAgentFileRecordByDocID(log, ThreadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, "INVALID_FILE");

                }
            }
            else
            {
                log.CreateLogEntry(String.Format("Provider Agent Bulk Upload Failed to Bulk Load File: {0}", fileName));
                UpdateAgentFileRecordByDocID(log, ThreadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, "INVALID_FILE");

            }
        }

        public void ProcessAgentUploadFile(Logging log,Guid threadId, int docID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("AGENT_BULK_UPLOAD_DOC_ID", DbType.Int32, docID, true));
                DataAccess.ExecuteStoredProcedure("usp_ProcessAgentBulkUploadRecords", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Bulk Upload Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
        public static byte[] GetDocumentFromOnBase(string fileName, int documentID, int onBaseDocID)
        {
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
          
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] retrievedFile = onBaseInterface.RetrieveFilebyOnBaseDocID(onBaseDocID.ToString());
            return retrievedFile;
        }
        public static DataTable GetDataTableFromExcel(String fileName, int docID, int onBaseDocID, Logging log, Guid threadID)
        {
            try
            {
                XSSFWorkbook wb;
                XSSFSheet sheetNewExcelFormat = null;               
                String Sheet_name;               

                using (MemoryStream ms = new MemoryStream(GetDocumentFromOnBase(fileName, docID, onBaseDocID)))
                {                    
                    wb = new XSSFWorkbook(ms);
                    Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
                    sheetNewExcelFormat = (XSSFSheet)wb.GetSheet(Sheet_name);                    
                }

                DataTable DT = new DataTable();
                DT.Rows.Clear();
                DT.Columns.Clear();

                // get sheet
                int i = 0;
                if (sheetNewExcelFormat != null)
                {
                    while (sheetNewExcelFormat.GetRow(i) != null)
                    {
                        i = ReadData(sheetNewExcelFormat, DT, i);
                    }
                }

                return DT;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                UpdateAgentFileRecordByDocID(log, threadID, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, DelegateDocumentUploadErrorCodes.ERR_FILE_REJ_RETR.ToString());
                return null;
            }
        }
        private static int ReadData(XSSFSheet sh, DataTable DT, int i)
        {
            if (DT.Columns.Count == 0)
            {
                DT.Columns.Add("Update Type");
                DT.Columns.Add("Provider Administrator OH ID PROD");
                DT.Columns.Add("Medicaid ID");
                DT.Columns.Add("Agent OH ID PROD");
                DT.Columns.Add("GrantedRole to AGENT");
            }
            //We will skip the rows which do not have valid data for the first Cell.
            //We expect the value in first cell to be either 'A' or 'D'

            // add row
            DT.Rows.Add();
            // write row value
            for (int j = 0; j < DT.Columns.Count; j++)
            {
                var cell = sh.GetRow(i).GetCell(j);
                if (j == 0 && cell != null && !(cell.RichStringCellValue.String.Equals("A") || cell.RichStringCellValue.String.Equals("D")))
                {
                    break;
                }
                if (cell != null)
                {
                    // TODO: you can add more cell types capatibility, e. g. formula
                    switch (cell.CellType)
                    {
                        case NPOI.SS.UserModel.CellType.Numeric:
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                DateTime date = Convert.ToDateTime(cell.DateCellValue);
                                DT.Rows[i][j] = date.ToString("MM/dd/yyyy");
                            }
                            else
                            {
                                DT.Rows[i][j] = sh.GetRow(i).GetCell(j).NumericCellValue.ToString().Trim();
                            }
                            break;

                        case NPOI.SS.UserModel.CellType.String:
                            DT.Rows[i][j] = sh.GetRow(i).GetCell(j).StringCellValue.Trim();
                            break;
                    }
                }
            }
            i++;
            return i;
        }
        public static bool BulkLoadToStg(Logging log, Guid threadId, DataTable dt, int docID)
        {
            bool isErrorsFree = false;
            SqlBulkCopy sqlBulkCopy = null;
            try
            {
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {

                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.STG_AGENT_BULK_UPLOAD_RECORDS";
                        sqlBulkCopy = bulkCopy;

                        try
                        {
                            bulkCopy.ColumnMappings.Add("Update Type", "UPDATE_TYPE");
                            bulkCopy.ColumnMappings.Add("Provider Administrator OH ID PROD", "PROVIDER_ADMINISTRATOR_OHID");
                            bulkCopy.ColumnMappings.Add("Medicaid ID", "MEDICAID_ID");
                            bulkCopy.ColumnMappings.Add("Agent OH ID PROD", "AGENT_OH_ID");
                            bulkCopy.ColumnMappings.Add("GrantedRole to AGENT", "AGENT_SUB_ROLE");
                            bulkCopy.ColumnMappings.Add("AGENT_BULK_UPLOAD_DOCUMENT_ID", "AGENT_BULK_UPLOAD_DOCUMENT_ID");
                            bulkCopy.ColumnMappings.Add("RESPONSE", "RESPONSE");
                            bulkCopy.WriteToServer(dt);
                            isErrorsFree = true;
                        }
                        catch (Exception ex)
                        {
                            string message = string.Empty;
                            string errorCode = string.Empty;
                            if (ex.Message.Contains("Received an invalid column length from the bcp client for colid"))

                            {
                                string pattern = @"\d+";
                                System.Text.RegularExpressions.Match match = Regex.Match(ex.Message.ToString(), pattern);
                                var index = Convert.ToInt32(match.Value) - 1;

                                FieldInfo fi = typeof(SqlBulkCopy).GetField("_sortedColumnMappings", BindingFlags.NonPublic | BindingFlags.Instance);
                                var sortedColumns = fi.GetValue(sqlBulkCopy);
                                var items = (Object[])sortedColumns.GetType().GetField("_items", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sortedColumns);

                                FieldInfo itemdata = items[index].GetType().GetField("_metadata", BindingFlags.NonPublic | BindingFlags.Instance);
                                var metadata = itemdata.GetValue(items[index]);
                                var column = metadata.GetType().GetField("column", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                var length = metadata.GetType().GetField("length", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).GetValue(metadata);
                                message = (String.Format("Column: {0} contains data with a length greater than: {1}", column, length));
                                errorCode = DelegateDocumentUploadErrorCodes.ERR_FILE_DATA_LEN.ToString();
                            }
                            else
                            {
                                message = "File is Rejected at BulkLoadToStg.";
                                errorCode = DelegateDocumentUploadErrorCodes.ERR_FILE_REJ_BULK.ToString();
                            }

                            log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception: {0}", !string.IsNullOrEmpty(message) ? message : ex.ToString()), Logging.LogPriority.Error);
                            UpdateAgentFileRecordByDocID(log, threadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, errorCode);
                            //reject the file
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
            }
            return isErrorsFree;
        }

        public static void UpdateAgentFileRecordByDocID(Logging log, Guid threadId, int docID, int? status = null, bool? emailSent = null, string errorCodes = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, true));
                parameters.Add(SqlParms.CreateParameter("ERROR_CODES", DbType.String, errorCodes, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_SENT", DbType.Boolean, emailSent, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                DataAccess.ExecuteStoredProcedure("usp_UpdateAgentFileRecordByDocID", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }
        public void SaveResponseFile(Logging log, Guid threadId, int docID, string fileName)
        {
            try
            {               
                string DestinationPath = Path.Combine(InfoAccess.GetAppSettingFromDB("FileStorePath_JobServer", string.Empty)+ @"AgentBulkUploadResponseFiles");
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    DestinationPath = Path.Combine(@"C:\Projects\Deployments\AgentBulkUploadResponseFiles\");
                }

                if (!Directory.Exists(DestinationPath))
                {
                    Directory.CreateDirectory(DestinationPath);
                }

                string newFileName = RenameFileMethodNew(DestinationPath, fileName);

                newFileName = Path.ChangeExtension(newFileName, ".xlsx");

                string fullFilePath = Path.Combine(DestinationPath, newFileName);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    fullFilePath = Path.Combine(@"C:\Projects\Deployments\AgentBulkUploadResponseFiles\" + newFileName);
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOC_ID", DbType.Int32, docID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectAgentUploadRecordsByDocID", parameters,"dsAgentFilesToProcess");
                if(Methods.HasRows(ds))
                {
                    DataTable dt2 = ds.Tables[0];
                    DelegateAffiliationHelper.ExportToExcel2(dt2, newFileName, @DestinationPath);
                    int newDocID = SaveResponseFileByDocID(log, threadId, fileName, newFileName, docID);
                    if (newDocID != 0)
                    {
                        byte[] fileBytes = RetrieveFile(fullFilePath);

                        OnBaseInterface onBaseInterface = new OnBaseInterface();
                        int onbaseDocID = onBaseInterface.GetSubmitedFileIDFromJobServer(newDocID, fileBytes, newFileName);

                        if (onbaseDocID > 0)
                        {
                            List<SqlParameter> sqlParams = new List<SqlParameter>();

                            sqlParams.Add(SqlHelper.CreateParameter("DOCUMENT_ID", DbType.Int32, newDocID, true));
                            sqlParams.Add(SqlHelper.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onbaseDocID, true));

                            InfoAccess.ExecuteStoredProcedure("usp_SetOnBaseDocumentID", sqlParams);
                            // Delete file written to the local path 
                            File.Delete(fullFilePath);
                            UpdateAgentFileRecordByDocID(log, ThreadId, docID, Constants.DelegateDocumentUploadStatus.Complete);
                        }

                        //SendToCMS(newDocID, fileBytes, newFileName);
                    }
                }
                else
                {
                    log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception blank file uploaded - {0}", fileName));
                    UpdateAgentFileRecordByDocID(log, threadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, "ERR_FILE_SAVE_BLANK_FILE");
                }
               
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception Error while Saving the file {0}", ex.ToString()));
                UpdateAgentFileRecordByDocID(log, threadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, DelegateDocumentUploadErrorCodes.ERR_FILE_SAVE.ToString());
            }
        }

        public static int SaveResponseFileByDocID(Logging log, Guid threadId, string oldFileName, string newFileName, int docID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOC_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, oldFileName, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_AGENT_FILE_NAME", DbType.String, newFileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertResponseAgentUploadByDocID", parameters));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Provider Agent Upload Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                return 0;
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
        private string RenameFileMethodNew(string dir, string input)
        {
            string rtn = input;

            int pos = input.LastIndexOf(".");
            if (pos == -1) rtn = input + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            else rtn = input.Substring(0, pos) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + input.Substring(pos);

            return rtn;
        }
        public void SendUploadProcessedEmail(Logging log, Guid threadId, int docID, string recipientEmail, string fileName, string userName)
        {
            try
            {                
                if (SendEmail(userName, recipientEmail, fileName))
                {
                    UpdateAgentFileRecordByDocID(log, threadId, docID, null, true);
                    log.CreateLogEntry(String.Format("Sent email for file name: {0} to email : {1}", fileName,recipientEmail));
                }                    
              
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error while sending out Emails {0}", ex.ToString()));
            }
        }
        public bool SendEmail(string username, string email, string fileName)
        {
            bool noticeSent = false;
            string subject = "Agent Bulk Upload File Processed";
            //string templateFile = "AgentBulkUploadEmailNotice.txt";
            
            try
            {

                Notification n = new Notification();
                Dictionary<string, object> fields = new Dictionary<string, object>();

                fields.Add("USERNAME", username);
                fields.Add("FileName", fileName);
                string body = string.Empty;
                string templateActualPath = AppSettings.Get("PDMS_SVC_TemplatesPath", string.Empty);
                
                string pwdTemplateContent = null;
                using (StreamReader reader = new StreamReader(templateActualPath + @"/AgentBulkUploadEmailNotice.txt"))
                {
                    pwdTemplateContent = reader.ReadToEnd();
                }
                body = n.ParseEmailBody(pwdTemplateContent, fields);
                EMailNotification notify = new EMailNotification(body, subject, email);
                body = notify.SendActualNotification(templateActualPath + @"/AgentBulkUploadEmailNotice.txt", fields, true);
               
                noticeSent = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return noticeSent;
        }

    }
}
