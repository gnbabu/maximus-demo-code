using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MAXIMUS.DataExchange.PDMS.CMCProgram
{
    public static class CMCProgramHelper
    {

        public static int CheckImportFileToStaging(Logging log, Guid threadId, FileInfo currentFile)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CMCProgramFileDetails_FILE_PATH", DbType.String, currentFile.DirectoryName, true));
                parameters.Add(SqlParms.CreateParameter("CMCProgramFileDetails_FILE_NAME", DbType.String, currentFile.Name, true));
                parameters.Add(SqlParms.CreateParameter("CMCProgramFileDetails_LOAD_DATE_TIME", DbType.String, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                string newIdString = DataAccess.ExecuteScalar("usp_SaveCMCProgramFileDetails", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("CMC Program Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
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

        public static void LogFileRecordError(Logging log, string lineNumber, string fileName, string errorMessage)
        {
            string fullMessage = "Record failed. Line No: {0}; File: {1}; Reason: {2}";
            log.CreateLogEntry(String.Format(fullMessage, lineNumber, fileName, errorMessage), Logging.LogPriority.DataLoadIssues);
        }

        public static void SaveCMCProgramRecords(List<CMCProgramServiceRecords> records, int fileID, Guid threadID, Logging log)
        {
            try
            {
                List<SqlParameter> parameters = null;
                foreach (CMCProgramServiceRecords rec in records)
                {
                    if (parameters == null)
                    {
                        parameters = new List<SqlParameter>();
                    }
                    else
                    {
                        parameters.Clear();
                    }
                    parameters.Add(SqlParms.CreateParameter("CMCFileDetails_ID", DbType.Int32, fileID, false));
                    parameters.Add(SqlParms.CreateParameter("Practice_Medicaid_ID", DbType.String, rec.PracticeMedicaidID, false));
                    parameters.Add(SqlParms.CreateParameter("Federal_Tax_ID", DbType.String, rec.FederalTaxID, false));
                    parameters.Add(SqlParms.CreateParameter("Service_Location_Address_Address_1", DbType.String, rec.PracticeServiceAddr1, false));
                    parameters.Add(SqlParms.CreateParameter("Service_Location_Address_Address_2", DbType.String, rec.PracticeServiceAddr2, false));
                    parameters.Add(SqlParms.CreateParameter("Service_Location_Address_City", DbType.String, rec.PracticeServiceCity, false));
                    parameters.Add(SqlParms.CreateParameter("Service_Location_Address_State", DbType.String, rec.PracticeServiceState, false));
                    parameters.Add(SqlParms.CreateParameter("Service_Location_Address_Zip", DbType.String, rec.PracticeServiceZip, false));
                    parameters.Add(SqlParms.CreateParameter("Qualifying_Enrollment_Count", DbType.String, rec.QualEnrollCount, false));


                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadID, false));
                    parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadID, false));
                    parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, false));


                    DataAccess.ExecuteStoredProcedure("usp_InsertCMCProgramFileRecordByFileID", parameters);

                }
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("CMC Program Exception: {0}", ex.ToString()), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadID, ex);
            }
        }
    }
}
