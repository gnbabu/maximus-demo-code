using MAXIMUS.Core.Libraries;
using MAXIMUS.DataExchange.PDMS.CredRoster;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;
using System.IO.Packaging;
using ClosedXML.Excel;
using MAXIMUS.Controllers.PDMS;

namespace MAXIMUS.DataExchange.PDMS.NUBC
{
    public class NubcImport : BaseJob, IJob
    {
        private PDMSService.PDMSServiceClient _svc;

        private string directoryPath;
        private string inputFilesPath;
        private string notProcessedPath;
        private string processedPath;
        private string logsPath;

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

        #region "Constructors"

        public NubcImport(Guid threadId)
            : base(threadId)
        {
            this.ThreadId = threadId;
        }

        #endregion

        #region "Logging Objects"

        private int logCnt = 0;
        private Logging log = null;

        #endregion

        #region "Public Methods"

        override public void ExecuteJob()
        {
            // Default Job
            this.ExecuteJob(Guid.Parse("C998F8B4-2E08-4496-9AD3-ABCA4DF589C9"));
        }

        override public void ExecuteJob(Guid jobId)
        {
            // This will be an excel file with 15 worksheets
            LoadExcelData(jobId);
        }

        #endregion

        #region "Private Methods"
        private void LoadExcelData(Guid jobId)
        {
            string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
            log = new Logging(this.ThreadId, logMsg);
            //Create directories if not exist
            GetFileDirectories();

            log.CreateLogEntry("Start import of NUBC excel data");
            bool fileHasError = false;
            ////string directoryPath = AppSettings.Get("NubcDataFolder");
            //inputFilesPath = directoryPath + @"\InputFiles";
            //notProcessedPath = directoryPath + @"\NotProcessed";
            //processedPath = directoryPath + @"\Processed";
            //logsPath = directoryPath + @"\Logs";

            WriteLog("Start import of NUBC excel data");
            StringBuilder strSuccess = new StringBuilder();
            StringBuilder strError = new StringBuilder();
            try
            {
                // Get all .xlsx files in the directory
                string[] xlsxFiles = Directory.GetFiles(inputFilesPath, "*.xlsx");
                WriteLog("# of NUBC excel data file: " + xlsxFiles.Length);
                // Check if there are any files and get the first one since each file requires review and approve to process
                if (xlsxFiles.Length > 0)
                {
                    string firstXlsxFile = xlsxFiles[0];
                    WriteLog("NUBC excel data file name: " + firstXlsxFile);
                    log.CreateLogEntry("NUBC excel data file name: " + firstXlsxFile, Logging.LogPriority.Information);
                    try
                    {
                        using (var workbook = new XLWorkbook(firstXlsxFile))
                        {
                            WriteLog("Workbook created, and ready to loop through worksheets");

                            string connString = AppSettings.GetConnectionString();
                            WriteLog("DB Connection string: " + connString);
                            using (SqlConnection conn = new SqlConnection(connString))
                            {
                                conn.Open();
                                // Loop through all worksheets
                                foreach (var worksheet in workbook.Worksheets)
                                {
                                    WriteLog("Worksheet name: " + worksheet.Name);
                                    //Get the dataset from mapping table and see if the worksheet name exist
                                    List<SqlParameter> sqlParms = new List<SqlParameter>();
                                    sqlParms.Add(SqlParms.CreateParameter("WorkSheetName", DbType.String, worksheet.Name, false));
                                    DataSet dsMap = DataAccess.ExecuteStoredProcedure("usp_SelectNUBC_RFD_Mapping", sqlParms, "NUBC_RFD_Mapping");
                                    if (ObjectControllerHelper.HasRows(dsMap))
                                    {
                                        WriteLog("Worksheet mapping found for: " + worksheet.Name);
                                        DataRow row = dsMap.Tables[0].Rows[0];
                                        var tableName = ObjectControllerHelper.GetString("STG_RFD_NUBC_TABLENAME", row);
                                        if (!string.IsNullOrEmpty(tableName))
                                        {
                                            WriteLog("Worksheet mapping table exist for: " + worksheet.Name);
                                            log.CreateLogEntry("Worksheet mapping table exist for: " + worksheet.Name, Logging.LogPriority.Information);
                                            //if Exist then process all the records from a single worksheet
                                            // Start a transaction
                                            SqlTransaction transaction = conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                                            DateTime dt = DateTime.Now;
                                            var hasRecords = false;
                                            try
                                            {
                                                foreach (var wRow in worksheet.RowsUsed())
                                                {
                                                    int? deleted = null;
                                                    if (!wRow.Cell(2).IsEmpty())
                                                        deleted = Convert.ToInt16(wRow.Cell(2).Value.ToString());
                                                    int? futureUse = null;
                                                    if (!wRow.Cell(3).IsEmpty())
                                                        futureUse = Convert.ToInt16(wRow.Cell(3).Value.ToString());
                                                    NubcHelper.SaveNubcTable(tableName, log, wRow.Cell(1).Value.ToString(), deleted, futureUse, wRow.Cell(4).Value.GetText(), wRow.Cell(5).IsEmpty() ? null : wRow.Cell(5).Value.GetText(), dt, transaction);
                                                    hasRecords = true;
                                                }
                                                if (hasRecords)
                                                {
                                                    //Commit transaction
                                                    transaction.Commit();
                                                    strSuccess.AppendLine(worksheet.Name);
                                                    WriteLog("Has records and data commited successfully for Worksheet: " + worksheet.Name);
                                                    log.CreateLogEntry("Data commited successfully for Worksheet: " + worksheet.Name, Logging.LogPriority.Information);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                //Rollback transaction
                                                transaction.Rollback();
                                                fileHasError = true;
                                                strError.AppendLine(worksheet.Name);
                                                WriteLog("Error saving staging data for - " + tableName + " for - " + worksheet.Name + "; Exception:" + ex.ToString());
                                                log.CreateLogEntry("Error saving staging data for - " + tableName + " for - " + worksheet.Name + "; Exception:" + ex.ToString(), Logging.LogPriority.Error);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!fileHasError)
                        {
                            WriteLog("Processed without Error!");
                            DirectoryInfo processedDir = new DirectoryInfo(processedPath);
                            string processedPath1 = Path.Combine(processedDir.FullName, Path.GetFileName(firstXlsxFile)); // Combine the directory and file name
                            if (processedDir.Exists)
                            {
                                FileInfo file = new FileInfo(firstXlsxFile);
                                if (file.Exists)
                                {
                                    // Check if a file with the same name already exists in the destination
                                    if (File.Exists(processedPath1))
                                    {
                                        // Optional: Overwrite the file
                                        File.Delete(processedPath1);
                                    }

                                    // Move the file
                                    file.MoveTo(processedPath1);
                                    log.CreateLogEntry("File " + firstXlsxFile + " successfully processed and moved to Processed folder: " + processedDir, Logging.LogPriority.Information);
                                    WriteLog("Processed without Error and file moved to Processed folder!");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        log.CreateLogEntry("Failed to load NUBC codes file to staging tables. Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                        strError.AppendLine("Failed to load NUBC codes file to staging tables. Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace);
                        WriteLog("Failed to load NUBC codes file to staging tables. Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace);
                        //throw CoreException.ThrowException(this.ThreadId, ex);
                    }
                    SendEmail(jobId.ToString(), strSuccess, strError);
                    WriteLog("Sent Email notification.");
                }
                else
                {
                    log.CreateLogEntry("No .xlsx files found in the directory - " + inputFilesPath, Logging.LogPriority.Information);
                    WriteLog("No .xlsx files found in the directory.");
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine($"An error occurred: {ex.Message}");
                log.CreateLogEntry("An error occurred: - " + ex.ToString(), Logging.LogPriority.Error);
                WriteLog($"An error occurred: {ex.ToString()}");
            }                
        }

        private void SendEmail(string jobId, StringBuilder strSuccess, StringBuilder strError)
        {
            try
            {
                // generate an email
                string subject = AppSettings.Get("Jobs-NotificationSubject");
                string emailSubject = String.Format(subject, "NUBC DataLoad");
                string strBody = String.Empty;
                if (strSuccess.Length > 0)
                {
                    strBody = "The following NUBC Codeset worksheets have been loaded successfully: </br>" + strSuccess.ToString();
                }
                if (strError.Length > 0)
                {
                    strBody = strBody + "</br>The following NUBC Codeset worksheets have NOT been loaded: </br>" + strError.ToString();
                }
                EMailNotification notify = new EMailNotification(strBody, emailSubject, "OHPNMCodejunkies@maximus.com,srinivasakandru@maximus.com", this.ThreadId);
                Guid jobGuid = new Guid(jobId);
                notify.SendJobNotification(true, jobGuid);
            }
            catch (Exception ex)
            {
                WriteLog($"Error Sending email: {ex.ToString()}");
            }
        }

        private void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            //string directoryPath = AppSettings.Get("NubcDataFolder");

            string logFilePath = logsPath + @"\";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + ".txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(System.DateTime.Now + " - " + strLog);
            log.Close();
        }

        private void GetFileDirectories()
        {
            directoryPath = AppSettings.Get("NubcDataFolder");
            inputFilesPath = directoryPath + @"\InputFiles";
            notProcessedPath = directoryPath + @"\NotProcessed";
            processedPath = directoryPath + @"\Processed";
            logsPath = directoryPath + @"\Logs";

            //Create directories if not exist
            if (!System.IO.Directory.Exists(directoryPath))
            {
                System.IO.Directory.CreateDirectory(directoryPath);
            }

            if (!System.IO.Directory.Exists(inputFilesPath))
            {
                System.IO.Directory.CreateDirectory(inputFilesPath);
            }

            if (!System.IO.Directory.Exists(notProcessedPath))
            {
                System.IO.Directory.CreateDirectory(notProcessedPath);
            }

            if (!System.IO.Directory.Exists(processedPath))
            {
                System.IO.Directory.CreateDirectory(processedPath);
            }

            if (!System.IO.Directory.Exists(logsPath))
            {
                System.IO.Directory.CreateDirectory(logsPath);
            }
        }
        #endregion
    }
}
