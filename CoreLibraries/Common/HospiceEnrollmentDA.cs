using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Corp.Core.Libraries
{
    public static class HospiceEnrollmentDA
    {
        public static void InsertHospiceEnrollmentServiceReqRes(string SITransKey, string requestType, string request, string response, string responseCode,
            string responseMsg, string responseType, string responseDetail, Guid createdBy, string responseError, string pnmTransactionKey)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, requestType, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.String, request, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));
            parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmTransactionKey, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_TIMESTAMP", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("OPS", DbType.Int32, 1, true)); // 1 - Insert

            InfoAccess.ExecuteStoredProcedure("InsertOUTBOUND_HOSPICE_ENROLLMENT_REQ_RES", parameters);
        }
        public static void UpdateHospiceEnrollmentServiceReqRes(string SITransKey, string requestType, string request, string response, string responseCode,
            string responseMsg, string responseType, string responseDetail, Guid createdBy, string responseError, string pnmTransactionKey)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransKey, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, requestType, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.String, request, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_PAYLOAD", DbType.String, response, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, responseCode, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, responseMsg, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, responseType, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, responseDetail, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_ERROR", DbType.String, responseError, true));
            parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmTransactionKey, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_TIMESTAMP", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("OPS", DbType.Int32, 2, true)); // 2 - Update

            InfoAccess.ExecuteStoredProcedure("UpdateOUTBOUND_HOSPICE_ENROLLMENT_REQ_RES", parameters);
        }
    }
}
