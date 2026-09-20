using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Reflection;
using System.Text.RegularExpressions;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;

namespace MAXIMUS.DataExchange.PDMS.HCPCLVL2
{
    public class HCPCLVL2Helper
    {
        //Save STG_NUBC_RFD_<TableName> in a transaction
        public static void SaveHcpcLvl2Table(string tableName, Logging log, string code, int? deleted, int? futureUse, string desc, string additionalInfo, DateTime loadDate, SqlTransaction trans)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CODE", DbType.String, code, true));
                parameters.Add(SqlParms.CreateParameter("DESC", DbType.String, desc, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL", DbType.String, additionalInfo, false));
                parameters.Add(SqlParms.CreateParameter("DELETED", DbType.Int16, deleted, false));
                parameters.Add(SqlParms.CreateParameter("FUTUREUSE", DbType.Int32, futureUse, false));
                parameters.Add(SqlParms.CreateParameter("LOADDATE", DbType.DateTime, loadDate, true));

                var table = "usp_Insert" + tableName;

                DataAccess.ExecuteStoredProcedure(trans, table, parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error saving {0}: {1}", tableName, ex.StackTrace), Logging.LogPriority.Error);
            }
        }

        public static DataTable GetDataTableFromExcel(String fileName, Logging log, Guid threadID)
        {
            try
            {
                XSSFWorkbook wb;
                XSSFSheet sheetNewExcelFormat = null;
                String Sheet_name;

                using (FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    wb = new XSSFWorkbook(file);
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
                log.CreateLogEntry(String.Format("Delegate Affiliation Job Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                return null;
            }
        }


        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string directoryPath = AppSettings.Get("HCPCLvl2DataFolder");

            string logFilePath = directoryPath + @"\Logs\";
            logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
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


        public static void SendEmail(string jobId, StringBuilder strSuccess, StringBuilder strError)
        {
            try
            {
                // generate an email
                string subject = AppSettings.Get("Jobs-NotificationSubject");
                string emailSubject = String.Format(subject, "HCPCS Level 2 DataLoad");
                string strBody = String.Empty;
                if (strSuccess.Length > 0)
                {
                    strBody = "The following HCPCS Level 2 Codeset worksheets have been loaded successfully: </br>" + strSuccess.ToString();
                }
                if (strError.Length > 0)
                {
                    strBody = strBody + "</br>The following HCPCS Level2 Codeset worksheets have NOT been loaded: </br>" + strError.ToString();
                }
                EMailNotification notify = new EMailNotification(strBody, emailSubject, "OHPNMCodejunkies@maximus.com,rohitnagvenkar@maximus.com", new Guid(CON.HCPCSLvl2.HCPCSLvl2AppID));
                Guid jobGuid = new Guid(jobId);
                notify.SendJobNotification(true, jobGuid);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
        }

        private static int ReadData(XSSFSheet sh, DataTable DT, int i)
        {
            if (DT.Columns.Count == 0)
            {
                DT.Columns.Add("HCPC");
                DT.Columns.Add("SEQNUM");
                DT.Columns.Add("RECID");
                DT.Columns.Add("LONG DESCRIPTION");
                DT.Columns.Add("SHORT DESCRIPTION");
                DT.Columns.Add("PRICE1");
                DT.Columns.Add("PRICE2");
                DT.Columns.Add("PRICE3");
                DT.Columns.Add("PRICE4");
                DT.Columns.Add("MULT_PI");
                DT.Columns.Add("CIM1");
                DT.Columns.Add("CIM2");
                DT.Columns.Add("CIM3");
                DT.Columns.Add("MCM1");
                DT.Columns.Add("MCM2");
                DT.Columns.Add("MCM3");
                DT.Columns.Add("STATUTE");
                DT.Columns.Add("LABCERT1");
                DT.Columns.Add("LABCERT2");
                DT.Columns.Add("LABCERT3");
                DT.Columns.Add("LABCERT4");
                DT.Columns.Add("LABCERT5");
                DT.Columns.Add("LABCERT6");
                DT.Columns.Add("LABCERT7");
                DT.Columns.Add("LABCERT8");
                DT.Columns.Add("XREF1");
                DT.Columns.Add("XREF2");
                DT.Columns.Add("XREF3");
                DT.Columns.Add("XREF4");
                DT.Columns.Add("XREF5");
                DT.Columns.Add("COV");
                DT.Columns.Add("ASC_GRP");
                DT.Columns.Add("ASC_DT");
                DT.Columns.Add("OPPS");
                DT.Columns.Add("OPPS_PI");
                DT.Columns.Add("OPPS_DT");
                DT.Columns.Add("PROCNOTE");
                DT.Columns.Add("BETOS");
                DT.Columns.Add("TOS1");
                DT.Columns.Add("TOS2");
                DT.Columns.Add("TOS3");
                DT.Columns.Add("TOS4");
                DT.Columns.Add("TOS5");
                DT.Columns.Add("ANEST_BU");
                DT.Columns.Add("ADD DT");
                DT.Columns.Add("ACT EFF DT");
                DT.Columns.Add("TERM DT");
                DT.Columns.Add("ACTION CD");
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


        public static bool BulkLoadToStg(Logging log, Guid threadId, DataTable dt)
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
                        bulkCopy.DestinationTableName = "dbo.STG_HCPCS_LVL2_CODE";
                        sqlBulkCopy = bulkCopy;

                        try
                        {
                            bulkCopy.ColumnMappings.Add("HCPC", "CODE");
                            bulkCopy.ColumnMappings.Add("SEQNUM", "SEQ_NUM");
                            bulkCopy.ColumnMappings.Add("LONG DESCRIPTION", "LONG_DESCRIPTION");
                            bulkCopy.ColumnMappings.Add("SHORT DESCRIPTION", "SHORT_DESC");
                            bulkCopy.ColumnMappings.Add("ADD DT", "ADD_DATE");
                            bulkCopy.ColumnMappings.Add("ACT EFF DT", "ADD_EFF_DATE");
                            bulkCopy.ColumnMappings.Add("TERM DT", "TERM_DATE");
                            bulkCopy.ColumnMappings.Add("ACTION CD", "ACT_CDE");
                            bulkCopy.ColumnMappings.Add("LOAD_DATE", "LOAD_DATE");
                            bulkCopy.WriteToServer(dt);
                            isErrorsFree = true;
                        }
                        catch (Exception ex)
                        {
                            log.CreateLogEntry(String.Format("Error saving {0}: {1}", "dbo.STG_HCPCS_LVL2_CODE", ex.StackTrace), Logging.LogPriority.Error);
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

    }
}
