using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Corp.Core.Libraries
{
    public static class RecipientEligibilityDA
    {
        public static void SaveMemberEligibilityServiceReqRes(string SITransKey, string request, string response, Guid createdBy, string PNMTransKey = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransKey, true));
            parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, PNMTransKey, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.String, request, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_PAYLOAD", DbType.String, response, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));

            InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_MEMBER_ELIGIBILITY_SERVICE_REQ_RES", parameters);
        }

        public static void SaveMemberEligibilityServiceReqRes(string SITransKey, string medicaidID, string requestType, string request, string response, string responseCode,
            string responseMsg, string responseType, Guid createdBy, string pnmKey = "")
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransKey, true));
            parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmKey, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, requestType, true));
            parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.String, request, true));
            parameters.Add(SqlHelper.CreateParameter("RESPONSE_PAYLOAD", DbType.String, response, true));
            parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_CODE", DbType.String, responseCode, true));
            parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_TYPE", DbType.String, responseType, true));
            parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, responseMsg, true));
            parameters.Add(SqlHelper.CreateParameter("MEDICAID_ID", DbType.String, medicaidID, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
            parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));

            InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_MEMBER_ELIGIBILITY_SERVICE_REQ_RES", parameters);
        }


        public static DataSet RetrieveWebAPITestingResponse(string APIName)
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("APIName", DbType.String, APIName, true));
            ds = InfoAccess.ExecuteStoredProcedure("usp_RetrieveWebAPITestingResponse", parameters, "RetrieveWebAPITestingResponse");
            return ds;
        }

        public static DataSet RetrieveWebAPITestingResponseBySearchParam(List<SqlParameter> parameters, string APIName = "")
        {
            DataSet ds = new DataSet();

            if (APIName.Equals("ClaimsSearchWebAPI") || APIName.Equals("ClaimsInquiryInstitutionalWebAPI") ||
                APIName.Equals("ClaimsInquiryProfessionalWebAPI") || APIName.Equals("ClaimsInquiryDentalWebAPI"))
            {
                ds = InfoAccess.ExecuteStoredProcedure("usp_RetrieveWebAPITestingResponse_ClaimSearch", parameters, "RetrieveWebAPITestingResponse");
            }
            else if (APIName.Equals("PriorAuthSearchWebAPI") || APIName.Equals("PriorAuthAddUpdateWebAPI") ||
                APIName.Equals("PriorAuthInquiryWebAPI"))
            {
                ds = InfoAccess.ExecuteStoredProcedure("usp_RetrieveWebAPITestingResponse_PASearch", parameters, "RetrieveWebAPITestingResponse");
            }
            else
            {
                ds = InfoAccess.ExecuteStoredProcedure("usp_RetrieveWebAPITestingResponse_ME", parameters, "RetrieveWebAPITestingResponse");
            }

            return ds;
        }

    }
}