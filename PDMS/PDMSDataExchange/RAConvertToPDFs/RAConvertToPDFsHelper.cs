using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.RAConvertToPDFs
{
    public static class RAConvertToPDFsHelper
    {


        public static DataSet SelectRATXTFilesToProcess(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRATXTFilesToProcess", "SelectRATXTFilesToProcess");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("RAConvertToPDFsHelper Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static DataSet SelectRAZipFilesToProcess(Logging log, Guid threadId)
        {
            try
            {
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectRAZipFilesToProcess", "SelectRAZipFilesToProcess");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("RAConvertToPDFsHelper Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static string clnFileName(string fileName)
        {
            StringBuilder builder = new StringBuilder(fileName);
            builder.Replace(".txt", "");

            return builder.ToString();
        }

        public static string InsertHeader(string sourcePath, string fileName, string header, string footer)
        {
            string cleanFileName = clnFileName(fileName);

            var srcfile = sourcePath + fileName;
            using (var writer = new StreamWriter(sourcePath + cleanFileName))
            using (var reader = new StreamReader(srcfile))
            {
                writer.WriteLine(header);
                while (!reader.EndOfStream)
                    writer.WriteLine(reader.ReadLine());
                writer.WriteLine(footer);
            }
            File.Delete(srcfile);

            return cleanFileName;
        }

        public static void UpdateRATXTFilesToProcess(Logging log, Guid threadId, int remittance_advice_id, int pdf_document_id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("remittance_advice_id", DbType.Int32, remittance_advice_id, true));
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_ID_PDF", DbType.Int32, pdf_document_id, true));
                parameters.Add(SqlParms.CreateParameter("Last_Modified_Date_Time", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("Last_Modified_User", DbType.Guid, threadId, true));
                DataAccess.ExecuteScalar("usp_UpdateRATXTFilesToProcess", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("RAConvertToPDFsHelper Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }
        }

        public static int StoreDocumentRecord(Logging log, Guid threadId, string name, string filename, string desc)
        {

            int retVal = 0;
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("NAME", DbType.String, name, false));
                parameters.Add(SqlParms.CreateParameter("DESCRIPTION", DbType.String, desc, false));
                parameters.Add(SqlParms.CreateParameter("FILE_NAME", DbType.String, filename, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.String, threadId, false));

                DataSet ds = new DataSet();
                retVal = Convert.ToInt32(DataAccess.ExecuteScalar("insertDOCUMENT", parameters));


            }
            catch (Exception ex)
            {
                log.CreateLogEntry(String.Format("RAConvertToPDFsHelper Exception: {0}", ex.StackTrace), Logging.LogPriority.Error);
                throw CoreException.ThrowException(threadId, ex);
            }

            return retVal;
        }
    }
}
 