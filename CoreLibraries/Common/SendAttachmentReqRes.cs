using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Corp.Core.Libraries
{
    public static class SendAttachmentReqRes
    {
        public static void SaveAttachmentServiceReqRes(string SITransKey, string requestType, string request, string response, string responseCode,
            string responseMsg, string responseType, string responseDetail, Guid createdBy, string responseError, string pnmKey = "")
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
            parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmKey, true));

            InfoAccess.ExecuteStoredProcedure("insertUpdateOUTBOUND_UPLOAD_ATTACHMENT_REQ_RES", parameters);
        }
    }
}
