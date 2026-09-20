using MAXIMUS.Core.Libraries;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.HRSA
{
    public class HRSAProcessHelper
    {
        public static void SaveProcessedFileDetails(Logging log, Guid threadId, FileInfo currentFile)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("HRSAFileDetails_FILE_PATH", DbType.String, currentFile.DirectoryName, true));
                parameters.Add(SqlParms.CreateParameter("HRSAFileDetails_FILE_NAME", DbType.String, currentFile.Name, true));
                parameters.Add(SqlParms.CreateParameter("HRSAFileDetails_LOAD_DATE_TIME", DbType.String, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                DataAccess.ExecuteStoredProcedure("usp_SaveHRSAFileDetails", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("HRSA Process Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
            }
        }

        public static string RenameFileMethod(string dir, string input)
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

        public static void ReadExcelAndStageData(Logging log, Guid threadId, FileInfo file)
        {
            string fileName = file.Name;
            var tasks = new List<Task>();

            try
            {
                log.CreateLogEntry(String.Format("Reading data from file {0}", fileName));
                string StrSheetName = AppSettings.Get("HRSAFile-SheetName", "false");
                //OHPNM - 15192 - Perm Fix for HRSA 340B file to getting updated in PNM.
                var excelRows = MiniExcel.Query(file.FullName,false, StrSheetName).ToList();
                int lastIndex = 0;
                for (int i = 0; i < excelRows.Count; i++)
                {
                    string header = excelRows[i].A;
                    if (header == "340B ID")
                    {
                        lastIndex = i;
                        break;
                    }
                }
                excelRows.RemoveRange(0, lastIndex + 1);
                //excelRows.RemoveRange(0, 3); //Remove excel file name and header rows before import
                var totalRecords = excelRows.Count;

                log.CreateLogEntry(String.Format("Reading from excel complete. Total Records: {0}", totalRecords));

                bool isMakeExcelFileCopy = Convert.ToBoolean(AppSettings.Get("HRSA-MakeFileCopy", "false"));

                log.CreateLogEntry(String.Format("Copy file data enabled: {0}", isMakeExcelFileCopy.ToString()));

                if (isMakeExcelFileCopy)
                {
                    //Create data table and add data columns to bulk import in DB
                    log.CreateLogEntry("Creating data table to copy file details");

                    deleteHRSAFileDataCopy();

                    DataTable dtHRSADataTable = CreateHRSAStagingTable();

                    for (int i = 0; i < excelRows.Count; i++)
                    {
                        var dr = dtHRSADataTable.NewRow();

                        //OHPNM - 15192 - Perm Fix for HRSA 340B file to getting updated in PNM.
                        dr["Record ID"] = i + 1;
                        //dr["HRSA340BID"] = (excelRows[i].D);
                        //dr["ParticipatingStartDate"] = (excelRows[i].H);
                        //dr["TermDate"] = (excelRows[i].I);
                        //dr["Medicaid Number"] = (excelRows[i].W);
                        //dr["NPI"] = (excelRows[i].X);
                        
                        dr["HRSA340BID"] = (excelRows[i].A);
                        dr["ParticipatingStartDate"] = (excelRows[i].E);
                        dr["TermDate"] = (excelRows[i].F);
                        dr["Medicaid Number"] = (excelRows[i].Y);
                        dr["NPI"] = (excelRows[i].Z);

                        dtHRSADataTable.Rows.Add(dr);
                    }

                    log.CreateLogEntry("Create data table complete");

                    log.CreateLogEntry("Bulk upload to STAGE_HRSA_DATA table initiated");

                    //Stage data using SQL Bulk Importer
                    tasks.Add(BulkLoadToStg(dtHRSADataTable, "STAGE_HRSA_DATA", log));
                }

                bool isStageHRSAData = Convert.ToBoolean(AppSettings.Get("HRSA-StageData", "false"));

                log.CreateLogEntry(String.Format("Import HRSA data to HRSA340B/ HRSA340B_ID enabled: {0}", isStageHRSAData.ToString()));

                if (isStageHRSAData)
                {
                    //Create data table and add data columns to bulk import in DB
                    log.CreateLogEntry("Creating data table to import records to HRSA340B/ HRSA340B_ID");

                    LoadHRSADataAndIDs(excelRows, log, threadId, tasks);
                }

                //Save entry of the staged file
                SaveProcessedFileDetails(log, threadId, file);

                log.CreateLogEntry(String.Format("HRSA file - {0} load complete", fileName));
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(Constants.LogString.FileLoadFailure + ex.Message);
            }
        }

        private static void deleteHRSAFileDataCopy()
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_deleteHRSAFileDataCopy");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void LoadHRSADataAndIDs(List<dynamic> rows, Logging log, Guid threadId, List<Task> tasks)
        {
            //Create tables and add data columns
            DataTable dtHRSA340B_IDS = CreateHRSAIDsDataTable();
            DataTable dtHRSA340B = CreateHRSADataTable();

            //Clear existing data from HRSA340B/HRSA340B_IDS
            ClearHRSAData();

            for (int i = 0; i < rows.Count; i++)
            {
                var newRec = dtHRSA340B.NewRow();
                newRec["Record ID"] = i + 1;
                
                //OHPNM - 15192 - Perm Fix for HRSA 340B file to getting updated in PNM.
                //newRec["HRSA340BID"] = ValidateString(rows[i].D, 20);
                //newRec["ParticipatingStartDate"] = rows[i].H;
                //newRec["TermDate"] = ValidateDate(rows[i].I);
                newRec["HRSA340BID"] = ValidateString(rows[i].A, 20);
                newRec["ParticipatingStartDate"] = rows[i].E;
                newRec["TermDate"] = ValidateDate(rows[i].F);

                dtHRSA340B.Rows.Add(newRec);

                //var medicaidNumber = rows[i].W;
                var medicaidNumber = rows[i].Y;
                if (medicaidNumber != null && medicaidNumber.ToString() != "")
                {
                    string[] medEntries = medicaidNumber.ToString().Split(',');

                    foreach (var medEntry in medEntries)
                    {
                        var newIdRec = dtHRSA340B_IDS.NewRow();
                        newIdRec["Record ID"] = i + 1;
                        newIdRec["Type"] = "Medicaid";
                        if (medEntry.StartsWith("("))
                        {
                            newIdRec["State"] = medEntry.Trim().Substring(1, 2);
                            newIdRec["ID"] = ValidateString(medEntry.Trim().Substring(4, medEntry.Length - 4), 49);
                        }
                        else
                        {
                            var splitEntry = medEntry.Split('(');
                            newIdRec["ID"] = splitEntry.Length > 0 ? ValidateString(splitEntry[0], 49) : string.Empty;
                            newIdRec["State"] = splitEntry.Length > 1 ? splitEntry[1].Trim().Replace(')', ' ').Substring(0, 2) : string.Empty;
                        }
                        dtHRSA340B_IDS.Rows.Add(newIdRec);
                    }
                }

                //var npis = rows[i].X;
                var npis = rows[i].Z;

                if (npis != null && npis.ToString() != "")
                {
                    string[] npiEntries = npis.ToString().Split(',');

                    foreach (var npiEntry in npiEntries)
                    {
                        var newIdRec = dtHRSA340B_IDS.NewRow();
                        newIdRec["Record ID"] = i + 1;
                        newIdRec["Type"] = "NPI";
                        if (npiEntry.StartsWith("("))
                        {
                            newIdRec["State"] = npiEntry.Trim().Substring(1, 2);
                            newIdRec["ID"] = ValidateString(npiEntry.Trim().Substring(4, npiEntry.Length - 4), 49);
                        }
                        else
                        {
                            var splitEntry = npiEntry.Split('(');
                            newIdRec["ID"] = splitEntry.Length > 0 ? ValidateString(splitEntry[0], 49) : string.Empty;
                            newIdRec["State"] = splitEntry.Length > 1 ? splitEntry[1].Trim().Replace(')', ' ').Substring(0, 2) : string.Empty;
                        }
                        dtHRSA340B_IDS.Rows.Add(newIdRec);
                    }
                }
            }

            log.CreateLogEntry("Create data table complete");

            log.CreateLogEntry("Bulk upload to HRSA340B/HRSA340B_ID table initiated");

            //Import data using SQL Bulk Importer
            tasks.Add(BulkLoadToStg(dtHRSA340B, "HRSA340B", log));
            tasks.Add(BulkLoadToStg(dtHRSA340B_IDS, "HRSA340B_IDs", log));
            Task.WaitAll(tasks.ToArray());
            log.CreateLogEntry("Bulk upload to HRSA340B/HRSA340B_ID tables and Copy file data complete");

            updateHRSADetails(threadId, log);
        }

        private static void updateHRSADetails(Guid threadId, Logging log)
        {
            try
            {
                log.CreateLogEntry("Updating HRSA records");

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));

                int timeout = Methods.GetIntValue(AppSettings.Get("HRSA-SqlTimeout", "1500"));

                DataAccess.ExecuteStoredProcedure("usp_UpdateRegHRSA340BDetails", parameters, timeout);

                log.CreateLogEntry("Updating HRSA records complete");
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("Error updateHRSADetails: {0}", ex.Message));
            }
        }

        private static void ClearHRSAData()
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_DeleteHRSAData");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ClearHRSAStagingData()
        {
            try
            {
                DataAccess.ExecuteStoredProcedure("usp_deleteStageHRSAData");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region private methods


        private static DataTable CreateHRSADataTable()
        {
            DataTable dtHRSA340B = new DataTable();

            dtHRSA340B.Columns.Add("Record ID", typeof(long));
            dtHRSA340B.Columns.Add("Grant Number", typeof(string));
            dtHRSA340B.Columns.Add("Site ID", typeof(string));
            dtHRSA340B.Columns.Add("Medicare Provider Number", typeof(string));
            dtHRSA340B.Columns.Add("HRSA340BID", typeof(string));
            dtHRSA340B.Columns.Add("CE ID", typeof(float));
            dtHRSA340B.Columns.Add("Program Code", typeof(string));
            dtHRSA340B.Columns.Add("Participating", typeof(string));
            dtHRSA340B.Columns.Add("ParticipatingStartDate", typeof(DateTime));
            dtHRSA340B.Columns.Add("TermDate", typeof(DateTime));
            dtHRSA340B.Columns.Add("Termination Code", typeof(float));
            dtHRSA340B.Columns.Add("Entity Name", typeof(string));
            dtHRSA340B.Columns.Add("Entity Sub-Division Name", typeof(string));
            dtHRSA340B.Columns.Add("Nofo Number", typeof(string));
            dtHRSA340B.Columns.Add("Assistance Received From Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Assistance Received To Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Address 3", typeof(string));
            dtHRSA340B.Columns.Add("City", typeof(string));
            dtHRSA340B.Columns.Add("State", typeof(string));
            dtHRSA340B.Columns.Add("Zip", typeof(string));
            dtHRSA340B.Columns.Add("Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Medicaid Number", typeof(string));
            dtHRSA340B.Columns.Add("NPI", typeof(string));
            dtHRSA340B.Columns.Add("Billing Organization", typeof(string));
            dtHRSA340B.Columns.Add("Billing Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Billing Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Billing City", typeof(string));
            dtHRSA340B.Columns.Add("Billing State", typeof(string));
            dtHRSA340B.Columns.Add("Billing Zip", typeof(string));
            dtHRSA340B.Columns.Add("Billing Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Organization", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Shipping City", typeof(string));
            dtHRSA340B.Columns.Add("Shipping State", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Zip", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Name", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Title", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Tel", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Tel Ext", typeof(string));
            dtHRSA340B.Columns.Add("Contact Name", typeof(string));
            dtHRSA340B.Columns.Add("Contact Title", typeof(string));
            dtHRSA340B.Columns.Add("Contact Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Contact Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Name", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Title", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Certified/Decertified Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Rural", typeof(string));
            dtHRSA340B.Columns.Add("Nature Of Support", typeof(string));
            dtHRSA340B.Columns.Add("Entry Comments", typeof(string));
            dtHRSA340B.Columns.Add("InKind Support Description", typeof(string));
            dtHRSA340B.Columns.Add("Support Received From Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Support Received To Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Pharmacy Name", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Begin Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Pharmacy Term Date", typeof(DateTime));
            dtHRSA340B.Columns.Add("Pharmacy Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy City", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy State", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Zip", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Comments", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Contract Pharmacy Comments", typeof(string));
            dtHRSA340B.Columns.Add("Edit Date", typeof(DateTime));
            return dtHRSA340B;
        }

        private static DataTable CreateHRSAIDsDataTable()
        {
            DataTable dtHRSA340B_IDS = new DataTable();

            dtHRSA340B_IDS.Columns.Add("Record ID", typeof(long));
            dtHRSA340B_IDS.Columns.Add("State", typeof(string));
            dtHRSA340B_IDS.Columns.Add("Type", typeof(string));
            dtHRSA340B_IDS.Columns.Add("ID", typeof(string));
            return dtHRSA340B_IDS;
        }

        private static DataTable CreateHRSAStagingTable()
        {
            DataTable dtHRSA340B = new DataTable();
            dtHRSA340B.Columns.Add("Record ID", typeof(long));
            dtHRSA340B.Columns.Add("Grant Number", typeof(string));
            dtHRSA340B.Columns.Add("Site ID", typeof(string));
            dtHRSA340B.Columns.Add("Medicare Provider Number", typeof(string));
            dtHRSA340B.Columns.Add("HRSA340BID", typeof(string));
            dtHRSA340B.Columns.Add("CE ID", typeof(string));
            dtHRSA340B.Columns.Add("Program Code", typeof(string));
            dtHRSA340B.Columns.Add("Participating", typeof(string));
            dtHRSA340B.Columns.Add("ParticipatingStartDate", typeof(string));
            dtHRSA340B.Columns.Add("TermDate", typeof(string));
            dtHRSA340B.Columns.Add("Termination Code", typeof(string));
            dtHRSA340B.Columns.Add("Entity Name", typeof(string));
            dtHRSA340B.Columns.Add("Entity Sub-Division Name", typeof(string));
            dtHRSA340B.Columns.Add("Nofo Number", typeof(string));
            dtHRSA340B.Columns.Add("Assistance Received From Date", typeof(string));
            dtHRSA340B.Columns.Add("Assistance Received To Date", typeof(string));
            dtHRSA340B.Columns.Add("Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Address 3", typeof(string));
            dtHRSA340B.Columns.Add("City", typeof(string));
            dtHRSA340B.Columns.Add("State", typeof(string));
            dtHRSA340B.Columns.Add("Zip", typeof(string));
            dtHRSA340B.Columns.Add("Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Medicaid Number", typeof(string));
            dtHRSA340B.Columns.Add("NPI", typeof(string));
            dtHRSA340B.Columns.Add("Billing Organization", typeof(string));
            dtHRSA340B.Columns.Add("Billing Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Billing Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Billing Address 3", typeof(string));
            dtHRSA340B.Columns.Add("Billing City", typeof(string));
            dtHRSA340B.Columns.Add("Billing State", typeof(string));
            dtHRSA340B.Columns.Add("Billing Zip", typeof(string));
            dtHRSA340B.Columns.Add("Billing Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Organization", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Shipping City", typeof(string));
            dtHRSA340B.Columns.Add("Shipping State", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Zip", typeof(string));
            dtHRSA340B.Columns.Add("Shipping Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Name", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Title", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Tel", typeof(string));
            dtHRSA340B.Columns.Add("Authorizing Official Tel Ext", typeof(string));
            dtHRSA340B.Columns.Add("Contact Name", typeof(string));
            dtHRSA340B.Columns.Add("Contact Title", typeof(string));
            dtHRSA340B.Columns.Add("Contact Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Contact Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Name", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Title", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Signed By Date", typeof(string));
            dtHRSA340B.Columns.Add("Certified/Decertified Date", typeof(string));
            dtHRSA340B.Columns.Add("Rural", typeof(string));
            dtHRSA340B.Columns.Add("Nature Of Support", typeof(string));
            dtHRSA340B.Columns.Add("Entry Comments", typeof(string));
            dtHRSA340B.Columns.Add("InKind Support Description", typeof(string));
            dtHRSA340B.Columns.Add("Support Received From Date", typeof(string));
            dtHRSA340B.Columns.Add("Support Received To Date", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Name", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Begin Date", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Term Date", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Address 1", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Address 2", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy City", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy State", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Zip", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Second Zip", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Comments", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Telephone", typeof(string));
            dtHRSA340B.Columns.Add("Pharmacy Telephone Ext", typeof(string));
            dtHRSA340B.Columns.Add("Contract Pharmacy Comments", typeof(string));
            dtHRSA340B.Columns.Add("Edit Date", typeof(string));
            return dtHRSA340B;
        }

        private static async Task BulkLoadToStg(DataTable dtHRSADataTable, string desinationTable, Logging log)
        {
            string connString = AppSettings.GetConnectionString();

            bool isBatchingEnabled = Methods.GetBoolean(AppSettings.Get("HRSA-UseBCPBatch", "false"));
            int batchSize = Methods.GetIntValue(AppSettings.Get("HRSA-BCPBatchSize", "50000"));
            int timeout = Methods.GetIntValue(AppSettings.Get("HRSA-SqlTimeout", "0"));

            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlTransaction transaction = null;
                connection.Open();
                using (transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.CheckConstraints, transaction))
                        {
                            bulkCopy.BulkCopyTimeout = timeout;
                            if (isBatchingEnabled)
                            {
                                log.CreateLogEntry(String.Format("Batch size: {0}", batchSize.ToString()));
                                log.CreateLogEntry(String.Format("Command timeout: {0}", timeout.ToString()));
                                bulkCopy.BatchSize = batchSize;
                            }
                            bulkCopy.DestinationTableName = desinationTable;
                            await bulkCopy.WriteToServerAsync(dtHRSADataTable);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        log.CreateLogEntry(String.Format("Error BulkLoadToStg: {0}", ex.Message));
                        throw ex;
                    }
                }
            }
        }

        private static string ValidateString(object input, int maxLength)
        {
            if (input == null) return string.Empty;
            return input.ToString().Trim().Length > maxLength ? input.ToString().Trim().Substring(0, maxLength - 1) : input.ToString().Trim();
        }

        private static object ValidateFloat(object input)
        {
            if (input == null || input.ToString() == "") return DBNull.Value;

            var success = Single.TryParse(input.ToString().Trim(), out float parsedValue);
            if (success)
            {
                return parsedValue;
            }
            else
            {
                return DBNull.Value;
            }
        }

        private static object ValidateDate(object input)
        {
            if (input == null || input.ToString() == "") return DBNull.Value;

            var success = DateTime.TryParse(input.ToString().Trim(), out DateTime parsedValue);
            if (success)
            {
                return parsedValue;
            }
            else
            {
                return DBNull.Value;
            }
        }

        #endregion private methods
    }
}
