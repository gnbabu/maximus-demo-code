using CON = MAXIMUS.Core.Libraries.Constants;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using MAXIMUS.Core.Libraries;
using Quartz.Util;

namespace MAXIMUS.DataExchange.PDMS.LTCHomeProvider
{
    public static class LTCHomeProviderHelper
    {
        public static void CreateLogEntry(string processName, string message, int priority)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ThreadId", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                parameters.Add(SqlParms.CreateParameter("Message", DbType.String, message, true));
                parameters.Add(SqlParms.CreateParameter("ProcessName", DbType.String, processName, true));
                parameters.Add(SqlParms.CreateParameter("Machine", DbType.String, Environment.MachineName, true));
                parameters.Add(SqlParms.CreateParameter("User", DbType.String, Environment.UserDomainName + @"\" + Environment.UserName, true));
                parameters.Add(SqlParms.CreateParameter("Priority", DbType.Int32, priority, true));

                DataAccess.ExecuteScalar("usp_CreateLogEntryLTCHomeProvider", parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), ex);
            }
        }
        public static void UpdateLTCHPFileStatus(int id, string fileStatus, int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OUTBOUND_LTCHPFILE_DTLS_ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("FILE_STATUS", DbType.String, fileStatus, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, ops, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateOUTBOUND_LTCHPFILE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateLTCHPFileStatus", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
        }
        public static void UpdateLTCHPFileStatus(int id, int recordCount, string fileStatus, int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("OUTBOUND_LTCHPFILE_DTLS_ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("RECORD_COUNT", DbType.Int32, recordCount, true));
                parameters.Add(SqlParms.CreateParameter("FILE_STATUS", DbType.String, fileStatus, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, ops, true));

                DataAccess.ExecuteScalar("usp_InsertUpdateOUTBOUND_LTCHPFILE_DTLS", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("UpdateLTCHPFileStatus", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
        }

        public static int InsertLTCHPFileStatus(string fileName, Guid fileUUID, string fileStatus, int ops)
        {
            int fileID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("RECORD_COUNT", DbType.Int32, 0, true));
                parameters.Add(SqlParms.CreateParameter("FILE_UUID", DbType.Guid, fileUUID, true));
                parameters.Add(SqlParms.CreateParameter("FILE_STATUS", DbType.String, fileStatus, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                parameters.Add(SqlParms.CreateParameter("SOPS", DbType.Int32, ops, true));

                fileID = Convert.ToInt32(DataAccess.ExecuteScalar("usp_InsertUpdateOUTBOUND_LTCHPFILE_DTLS", parameters));
               
            }
            catch (Exception ex)
            {
                CreateLogEntry("InsertLTCHPFileStatus", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return fileID;
        }

        public static DataTable getAllLTCProvideFileDetails(Guid fileUUID)
        {
            DataTable dt = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_UUID", DbType.Guid, fileUUID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetAllLTCProvideFileDetails", parameters, "GetAllLTCProvideFileDetails");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                CreateLogEntry("getAllLTCProvideFileDetails", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return dt;
        }

        public static DataTable getAllLTCHomeFileDetails(Guid fileUUID)
        {
            DataTable dt = null;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_UUID", DbType.Guid, fileUUID, true));
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_GetAllLTCHomeFileDetails", parameters, "GetAllLTCHomeFileDetails");
                if (ds != null)
                {
                    dt = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {
                CreateLogEntry("getAllLTCHomeFileDetails", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
            return dt;
        }


        public static void stageLTCHomeFile(Guid fileUUID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_UUID", DbType.Guid, fileUUID, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                DataAccess.ExecuteStoredProcedure("usp_StageLTCHomeFile", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("stageLTCHomeFile", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
        }

        public static void stageLTCProviderFile(Guid fileUUID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_UUID", DbType.Guid, fileUUID, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, new Guid(CON.LTCHomeProvider.LTCHomeProviderAppID), true));
                DataAccess.ExecuteStoredProcedure("usp_StageLTCProviderFile", parameters);
            }
            catch (Exception ex)
            {
                CreateLogEntry("stageLTCProviderFile", String.Format("LTCHomeProviderFile Exception: {0} {1}", ex.Message, ex.StackTrace), CON.LTCHomeProvider.LogPriorityError);
            }
        }
    }
}
