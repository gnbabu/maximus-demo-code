using Corp.Core.Libraries;
using MAXIMUS.Core.Libraries;
using MMSWebControls;
using NPOI.HSSF.UserModel;
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
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static MAXIMUS.Core.Libraries.Constants;

namespace MAXIMUS.DataExchange.PDMS
{
    public class DelegateAffiliationHelper
    {
        public static int ImportFileToStaging(Logging log, Guid threadId, FileInfo currentFile)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateAffiliationsFileDetails_FILE_PATH", DbType.String, currentFile.DirectoryName, true));
                parameters.Add(SqlParms.CreateParameter("DelegateAffiliationsFileDetails_FILE_NAME", DbType.String, currentFile.Name, true));
                parameters.Add(SqlParms.CreateParameter("DelegateAffiliationsFileDetails_LOAD_DATE_TIME", DbType.DateTime, GetFileDateFromName(threadId, currentFile), true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                string newIdString = DataAccess.ExecuteScalar("usp_SaveDelegateAffiliationFileDetails", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataTable SelectDelegateAffiliationsRecords(Logging log, Guid threadId, int fileID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, fileID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectDelegateAffiliationsRecords", parameters, "delegatesDS");
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DateTime GetFileDateFromName(Guid threadId, FileInfo currentFile)
        {
            try
            {
                string fileName = Path.GetFileNameWithoutExtension(currentFile.FullName);
                DateTime fileDateTime = currentFile.CreationTime;
                return fileDateTime;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
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
                        bulkCopy.DestinationTableName = "dbo.STG_DelegateAffiliations";
                        sqlBulkCopy = bulkCopy;

                        try
                        {
                            bulkCopy.ColumnMappings.Add("Update Type", "UpdateType");
                            bulkCopy.ColumnMappings.Add("Group Med ID", "GroupMedID");
                            bulkCopy.ColumnMappings.Add("Affiliate MED ID", "AffiliateMEDID");
                            bulkCopy.ColumnMappings.Add("Affiliate NPI", "AffiliateNPI");
                            bulkCopy.ColumnMappings.Add("Affiliate Start Date", "AffiliateStartDate");
                            bulkCopy.ColumnMappings.Add("Affiliate End Date", "AffiliateEndDate");
                            bulkCopy.ColumnMappings.Add("Rendering Location Addr Line 1", "RenderingLocationAddrLine1");
                            bulkCopy.ColumnMappings.Add("Rendering Location Addr Line 2", "RenderingLocationAddrLine2");
                            bulkCopy.ColumnMappings.Add("Rendering Location Addr City", "RenderingLocationAddrCity");
                            bulkCopy.ColumnMappings.Add("Rendering Location Addr State", "RenderingLocationAddrState");
                            bulkCopy.ColumnMappings.Add("Rendering Location Addr Zip", "RenderingLocationAddrZip");
                            bulkCopy.ColumnMappings.Add("Rendering location phone", "RenderingLocationPhone");
                            bulkCopy.ColumnMappings.Add("Response", "Response");
                            bulkCopy.ColumnMappings.Add("DelegateFileDetails_ID", "DelegateFileDetails_ID");
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
                                Match match = Regex.Match(ex.Message.ToString(), pattern);
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

                            log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", !string.IsNullOrEmpty(message) ? message : ex.ToString()), Logging.LogPriority.Error);
                            UpdateDelegateFileRecordByDocID(log, threadId, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, errorCode);
                            //reject the file
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
            }
            return isErrorsFree;
        }

        public static void UpdateDelegateFileRecordByDocID(Logging log, Guid threadId, int docID, int? status = null, bool? emailSent = null, string errorCodes = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateFileDoc_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("STATUS", DbType.Int32, status, true));
                parameters.Add(SqlParms.CreateParameter("Error_Codes", DbType.String, errorCodes, true));
                parameters.Add(SqlParms.CreateParameter("EMAIL_SENT", DbType.Boolean, emailSent, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                string newIdString = DataAccess.ExecuteScalar("usp_UpdateDelegateFileRecordByDocID", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
            }
        }

        public static int SaveNewUploadedFileByDocID(Logging log, Guid threadId, string oldFileName, string newFileName, int docID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateFileDoc_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, oldFileName, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_DELEGATE_FILE_NAME", DbType.String, newFileName, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                return ObjectControllerHelper.ConvertStringToInt32(DataAccess.ExecuteScalar("usp_InsertResponseDelegateFileRecordByDocID", parameters));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                return 0;
            }
        }


        public static void ProcesssStgToReg(Logging log, Guid threadId, int fileID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, fileID, true));
                string newIdString = DataAccess.ExecuteScalar("usp_ProcessDelegateAffiliationsRecords", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static byte[] GetDocumentFromOnBase(string fileName, int documentID)
        {
            fileName = fileName.Replace("\"", string.Empty);
            fileName = fileName.Replace(",", "");
            string filePath = Path.Combine(InfoAccess.GetAppSettingFromDB("FileStorePathWeb", string.Empty), fileName);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                filePath = Path.Combine(@"D:\New folder\FileStoreP3\" + fileName);
            }

            bool onBaseTest = bool.Parse(AppSettings.Get("OnBase-InterfaceTesting", bool.TrueString)); //true : not using onBase aka local files
            OnBaseInterface onBaseInterface = new OnBaseInterface();
            byte[] retrievedFile = onBaseInterface.RetrieveFile(filePath, documentID);
            return retrievedFile;
        }

        public static DataTable GetDataTableFromExcel(String fileName, int docID, Logging log, Guid threadID)
        {
            try
            {
                XSSFWorkbook wb;
                XSSFSheet sheetNewExcelFormat = null;
                HSSFSheet sheetOldExcelFormat = null;
                String Sheet_name;

                //using (var fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
                HSSFWorkbook hssfwb;
                bool isXls = fileName.EndsWith(".xls");

                using (MemoryStream ms = new MemoryStream(GetDocumentFromOnBase(fileName, docID)))
                {
                    if (isXls)
                    {
                        hssfwb = new HSSFWorkbook(ms);
                        Sheet_name = hssfwb.GetSheetAt(0).SheetName;  //get first sheet name
                        sheetOldExcelFormat = (HSSFSheet)hssfwb.GetSheet(Sheet_name);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(ms);
                        Sheet_name = wb.GetSheetAt(0).SheetName;  //get first sheet name
                        sheetNewExcelFormat = (XSSFSheet)wb.GetSheet(Sheet_name);
                    }
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
                else if (sheetOldExcelFormat != null)
                {
                    while (sheetOldExcelFormat.GetRow(i) != null)
                    {
                        i = ReadData(sheetOldExcelFormat, DT, i);
                    }
                }

                return DT;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                UpdateDelegateFileRecordByDocID(log, threadID, docID, Constants.DelegateDocumentUploadStatus.Rejected, null, DelegateDocumentUploadErrorCodes.ERR_FILE_REJ_RETR.ToString());
                return null;
            }
        }

        private static int ReadData(XSSFSheet sh, DataTable DT, int i)
        {
            // add neccessary columns
            //if (DT.Columns.Count < sh.GetRow(i).Cells.Count)
            //{
            //    for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
            //    {
            //        DT.Columns.Add(sh.GetRow(i).GetCell(j).ToString(), typeof(string));
            //    }
            //}
            if (DT.Columns.Count == 0)
            {
                DT.Columns.Add("Update Type");
                DT.Columns.Add("Group Med ID");
                DT.Columns.Add("Affiliate MED ID");
                DT.Columns.Add("Affiliate NPI");
                DT.Columns.Add("Affiliate Start Date");
                DT.Columns.Add("Affiliate End Date");
                DT.Columns.Add("Rendering Location Addr Line 1");
                DT.Columns.Add("Rendering Location Addr Line 2");
                DT.Columns.Add("Rendering Location Addr City");
                DT.Columns.Add("Rendering Location Addr State");
                DT.Columns.Add("Rendering Location Addr Zip");
                DT.Columns.Add("Rendering location phone");
                DT.Columns.Add("Response");
            }
            //We will skip the rows which do not have valid data for the first Cell.
            //We expect the value in first cell to be either 'A' or 'E'

            // add row
            DT.Rows.Add();
            // write row value
            for (int j = 0; j < DT.Columns.Count; j++)
            {
                var cell = sh.GetRow(i).GetCell(j);
                if(j == 0 && cell != null && !(cell.RichStringCellValue.String.Equals("A") || cell.RichStringCellValue.String.Equals("E"))) 
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

        private static int ReadData(HSSFSheet sh, DataTable DT, int i)
        {
            // add neccessary columns
            if (DT.Columns.Count < sh.GetRow(i).Cells.Count)
            {
                for (int j = 0; j < sh.GetRow(i).Cells.Count; j++)
                {
                    DT.Columns.Add(sh.GetRow(i).GetCell(j).ToString(), typeof(string));
                }
            }

            // add row
            DT.Rows.Add();
            // write row value
            for (int j = 0; j < DT.Columns.Count; j++)
            {
                var cell = sh.GetRow(i).GetCell(j);

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

        public static void ExportToExcel2(DataTable data, string fileName, string filePath)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();
            IRow rowHead = sheet.CreateRow(0);

            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }

           /* for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }*/

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                workbook.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            byte[] bytes = bos.ToByteArray();
            Encryption es = new Encryption();

            var result = Path.ChangeExtension(fileName, ".xlsx");

            byte[] ENbytes = es.EncryptRijndael(bytes);
            File.WriteAllBytes(Path.Combine(filePath, fileName), ENbytes);

            GC.Collect();
        }

        public static DataTable SelectAllNotProcessedDelegateFiles()
        {
            DataSet ds = new DataSet();

            ds = DataAccess.ExecuteStoredProcedure("usp_SelectAllNotProcessedDelegateFiles", "AllNotProcessedDelegateFilesDS");
            ds.Tables[0].TableName = "AllNotProcessedDelegateFilesDT";
            DataTable dt = ds.Tables[0];
            return dt;
        }

        public static DataSet SelectDelegateFileEmailstoSend(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectDelegateFileEmailstoSend", "DelegateFileEmailstoSend");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet SelectICDODAServicesTransactions(Logging log, Guid threadId, int FileID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, FileID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectICDODAServicesTransactions", parameters, "DelegateFileEmailstoSend");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateICDODAServicesTransactions(Logging log, Guid threadId, int reg_ID, int fileID, string medID, bool? sentToSI = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GROUP_MEDICAID_ID", DbType.String, medID, true));
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID, true));
                parameters.Add(SqlParms.CreateParameter("DelegateFileDetails_ID", DbType.Int32, fileID, true));
                parameters.Add(SqlParms.CreateParameter("SENT_TO_SI", DbType.Boolean, sentToSI, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                DataAccess.ExecuteScalar("usp_UpdateICDODAServicesTransactions", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
    }
}