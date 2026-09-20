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

namespace MAXIMUS.DataExchange.PDMS.ICDS
{
    public static class IcdOdaWaiverHelper
    {

        //constants
        internal const string dateFormat = "yyyyMMdd";
        internal const string defaultBoolString = "false";
        internal const string defaultMCPExtension = ".dat";
        internal const string defaultCulture = "en-US";

        public static int ImportFileToStaging(Logging log, Guid threadId, FileInfo currentFile)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ICDSODAFileDetails_FILE_PATH", DbType.String, currentFile.DirectoryName, true));
                parameters.Add(SqlParms.CreateParameter("ICDSODAFileDetails_FILE_NAME", DbType.String, currentFile.Name, true));
                parameters.Add(SqlParms.CreateParameter("ICDSODAFileDetails_LOAD_DATE_TIME", DbType.DateTime, GetFileDateFromName(threadId, currentFile), true));                
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));

                string newIdString = DataAccess.ExecuteScalar("usp_SaveICDSODAFileDetails", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet SelectODATerminationRegIDs(Logging log, Guid threadId, int actionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ActionID", DbType.Int32, actionID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectODATerminationProvidersRegIDs", parameters, "ODATerminationProvidersRegIDs");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateODATerminationProvidersRegIDs(Logging log, Guid threadId, int reg_ID, bool? terminated = null, bool? odaEmailSent = null, bool? provEmailSent = null, bool? sentToSI = null)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID, true));
                parameters.Add(SqlParms.CreateParameter("TERMINATED", DbType.Boolean, terminated, true));
                parameters.Add(SqlParms.CreateParameter("ODA_EMAIL_SENT", DbType.Boolean, odaEmailSent, true));
                parameters.Add(SqlParms.CreateParameter("PROV_EMAIL_SENT", DbType.Boolean, provEmailSent, true));
                parameters.Add(SqlParms.CreateParameter("SENT_TO_SI", DbType.Boolean, sentToSI, true));
                DataAccess.ExecuteScalar("usp_UpdateODATerminationProvidersRegIDs", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateODATerminateProvidersByRegIDs(Logging log, Guid threadId, int reg_ID, bool terminated)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID, true));
                parameters.Add(SqlParms.CreateParameter("TERMINATED", DbType.Boolean, terminated, true));
                DataAccess.ExecuteScalar("usp_UpdateODATerminationProvidersRegIDs", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void TerminateProviderByRegID(Logging log, Guid threadId, int reg_ID, DateTime drr)
        {
            try
            {
                log.CreateLogEntry("Begin Terminate RegID:" + reg_ID.ToString());
                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                sqlParms2.Add(SqlParms.CreateParameter("REG_ID", DbType.Int32, reg_ID, false));
                sqlParms2.Add(SqlParms.CreateParameter("TERM_DATE", DbType.DateTime, drr, false));
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                sqlParms2.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, false));
                sqlParms2.Add(SqlParms.CreateParameter("INSERT_TRANSACTION", DbType.Boolean, false, false));
                sqlParms2.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_CODE", DbType.String, Constants.EnrollStatus.INACTIVE.ToString(), false));
                sqlParms2.Add(SqlParms.CreateParameter("ENROLLMENT_STATUS_REASON_CODE", DbType.String, Constants.EnrollStatusReason.TERMINATED_BY_SISTER_STATE_AGENCY, false));
                sqlParms2.Add(SqlParms.CreateParameter("IS_FRM_JOB", DbType.Boolean, true, false));
                DataAccess.ExecuteStoredProcedure("usp_TerminateProvider_ODA", sqlParms2);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }


        public static bool ProcesssStgToReg(Logging log, Guid threadId, int fileID)
        {
            try
            {
                bool isComplete = false;
                List<SqlParameter> sqlParms2 = new List<SqlParameter>();
                sqlParms2.Add(SqlParms.CreateParameter("ICDSODAFileDetails_ID", DbType.Int32, fileID, false));
                DataAccess.ExecuteStoredProcedure("usp_ProcessICDSODAServiceRecords", sqlParms2);
                isComplete = true;
                return isComplete;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static bool BulkLoadToStg(Logging log, Guid threadId, DataTable dt)
        {
            bool isErrorsFree = false;
            try
            {                
                string connString = AppSettings.GetConnectionString();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                   
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        connection.Open();
                        bulkCopy.BulkCopyTimeout = 0;
                        bulkCopy.DestinationTableName = "dbo.STG_ICDODAServices";
                        try
                        {
                            bulkCopy.ColumnMappings.Add("MEDICAID_ID", "MEDICAID_ID");
                            bulkCopy.ColumnMappings.Add("ProviderName", "ProviderName");
                            bulkCopy.ColumnMappings.Add("ServiceCountyName", "ServiceCountyName");
                            bulkCopy.ColumnMappings.Add("BusinessPhone", "BusinessPhone");
                            bulkCopy.ColumnMappings.Add("BusinessAddress", "BusinessAddress");
                            bulkCopy.ColumnMappings.Add("BusinessCity", "BusinessCity");
                            bulkCopy.ColumnMappings.Add("BusinessState", "BusinessState");
                            bulkCopy.ColumnMappings.Add("BusinessZipCode", "BusinessZipCode");
                            bulkCopy.ColumnMappings.Add("OdaServiceCode", "OdaServiceCode");
                            bulkCopy.ColumnMappings.Add("ServiceDescp", "ServiceDescp");
                            bulkCopy.ColumnMappings.Add("ServiceRate", "ServiceRate");
                            bulkCopy.ColumnMappings.Add("ContractedServiceBeginDate", "ContractedServiceBeginDate");
                            bulkCopy.ColumnMappings.Add("ContractedServiceEndDate", "ContractedServiceEndDate");
                            bulkCopy.ColumnMappings.Add("OdaApprovalDate", "OdaApprovalDate");
                            bulkCopy.ColumnMappings.Add("LvlOneSanctionInd", "LvlOneSanctionInd");
                            bulkCopy.ColumnMappings.Add("LvlTwoSanctionInd", "LvlTwoSanctionInd");
                            bulkCopy.ColumnMappings.Add("LvlThreeSanctionInd", "LvlThreeSanctionInd");
                            bulkCopy.ColumnMappings.Add("CeaseReferralsInd", "CeaseReferralsInd");
                            bulkCopy.ColumnMappings.Add("RecordLvlInd", "RecordLvlInd");
                            bulkCopy.ColumnMappings.Add("NPI", "NPI");
                            bulkCopy.ColumnMappings.Add("CREATED_BY_USER", "CREATED_BY_USER");
                            bulkCopy.ColumnMappings.Add("ICDSODAFileDetails_ID", "ICDSODAFileDetails_ID");
                            bulkCopy.WriteToServer(dt);
                            isErrorsFree = true;
                        }
                        catch (Exception ex)
                        {
                            throw CoreException.ThrowException(threadId, ex);
                        }
                    }
                }
            }catch(Exception ex)
            {
                log.CreateLogEntry(String.Format("ICD Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
            return isErrorsFree;
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
    }
}
