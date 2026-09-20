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

namespace MAXIMUS.DataExchange.PDMS.onBaseDocUpload
{
    public class OnBaseDocUploadHelper
    {

        public static int GetDocumentIDByFileName(Logging log, Guid threadId, FileInfo currentFile)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, currentFile.Name, true));

                string newIdString = DataAccess.ExecuteScalar("usp_GetDocumentIDByFileName", parameters);
                int newId = Int32.Parse(newIdString);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static int GetOnbaseIDByDocId(Logging log, Guid threadId, int docId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docId, true));

                string newIdString = DataAccess.ExecuteScalar("usp_GetOnbaseIDByDocId", parameters);
                int newId = 0;
                var result = int.TryParse(newIdString, out newId);

                return newId;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void OnBaseDocUploadTracking(Logging log, Guid threadId, string currentFile, int docID, int onBaseID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, currentFile, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseID, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_BY_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("CREATED_ON_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                DataAccess.ExecuteScalar("usp_InsertONBASE_DOCUMENT_UPLOAD_TRACKER", parameters);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateOnBaseDocumentID(Logging log, Guid threadId, int docID, int onBaseID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docID, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                DataAccess.ExecuteScalar("usp_UpdateOnBaseIDByDocumentID", parameters);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateOnBaseByDocumentName(Logging log, Guid threadId, string docName, int onBaseID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_NAME", DbType.String, docName, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                DataAccess.ExecuteScalar("usp_UpdateOnBaseIDByDocumentName", parameters);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateOnBaseByDocumentId(Logging log, Guid threadId, int docId, int onBaseID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID", DbType.Int32, docId, true));
                parameters.Add(SqlParms.CreateParameter("ONBASE_DOCUMENT_ID", DbType.Int32, onBaseID, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                DataAccess.ExecuteScalar("usp_UpdateOnBaseIDByDocumentId", parameters);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload UpdateOnBaseByDocumentId() Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static string InsertOnbaseUploadProcess(Logging log, Guid threadId, string fileName, string processStatusCd)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                parameters.Add(SqlParms.CreateParameter("PROCESS_STATUS_CD", DbType.String, processStatusCd, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                return DataAccess.ExecuteScalar("usp_INSERT_ONBASE_DOCUMENT_PROCESS", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void UpdateOnbaseUploadProcess(Logging log, Guid threadId, int id, string processStatusCd)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, true));
                parameters.Add(SqlParms.CreateParameter("PROCESS_STATUS_CD", DbType.String, processStatusCd, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, threadId, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                DataAccess.ExecuteScalar("USP_UPDATE_ONBASE_DOCUMENT_PROCESS", parameters);

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet GetOnbaseUploadProcessByFileName(Logging log, Guid threadId, string fileName)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, fileName, true));
                return DataAccess.ExecuteStoredProcedure("usp_SELECT_ONBASE_DOCUMENT_PROCESS_BY_FILENAME", parameters, "ONBASE_DOCUMENT_PROCESS");

            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static void DeleteOnbaseUploadProcessById(Logging log, Guid threadId, int id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ID", DbType.Int32, id, true));
                DataAccess.ExecuteScalar("USP_DELETE_ONBASE_DOCUMENT_PROCESS_BY_ID", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("OnBaseDocUpload Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }
    }
}
