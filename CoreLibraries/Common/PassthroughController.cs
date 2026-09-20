using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;


namespace Corp.Core.Libraries
{
    public static class PassthroughController
    {

        public enum PassthroughTransactionType
        {
            ClaimAddUpdate = 1,
            ClaimInquiry = 2,
            ClaimSearch = 3,
            PriorAuthAddUpdate = 4,
            PriorAuthInquiry = 5,
            PriorAuthSearch = 6
        }

        public static int InsertPassthroughTransactionQueue(
           int passthruTransactionTypeId
            , string siTransactionKey
            , string moduleTransactionId
            , string additionalModuleId
            , string request
            , string response
            , string requestorSystem
            , string subscriberSystem
           , string medicaid
           , DateTime? createDate
           , DateTime? submitDate
           , DateTime? processDate
           , DateTime? changedDate
           , string changedBy
            , string createdBy
           , int? claimType
           , string paNumber = ""
            , string ICN = ""
            , string responseCode = ""
            , string responseType = ""
            , string responseMsg = ""
            , string responseDetails = ""
            , string ackCode = ""
            , string ackType = ""
            , string ackResponseMsg = ""
            , string ackDetails = ""
            , DateTime? dtAck = null
            , string ediResponse = ""
            , string ediToken = ""
            , string ediResMsg = ""
            )
        {
            int returnVal = 0;

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("BILLING_MEDICAID_ID", DbType.String, medicaid, true));
                parameters.Add(SqlHelper.CreateParameter("SUBMIT_DATE_TIME", DbType.DateTime, submitDate, true));
                parameters.Add(SqlHelper.CreateParameter("PROCESS_DATE_TIME", DbType.DateTime, processDate, true));
                parameters.Add(SqlHelper.CreateParameter("CREATED_DATE_TIME", DbType.DateTime, createDate, true));
                parameters.Add(SqlHelper.CreateParameter("CREATED_BY_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_REQUEST_TYPE_ID", DbType.Int32, passthruTransactionTypeId, false));
                parameters.Add(SqlHelper.CreateParameter("CLAIM_TYPE", DbType.Int32, claimType, false));
                parameters.Add(SqlHelper.CreateParameter("PA_NUMBER", DbType.String, paNumber, true));
                parameters.Add(SqlHelper.CreateParameter("ICN", DbType.String, ICN, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, responseMsg, true));
                parameters.Add(SqlHelper.CreateParameter("ACK_RESPONSE_CODE", DbType.String, ackCode, true));
                parameters.Add(SqlHelper.CreateParameter("ACK_RESPONSE_TYPE", DbType.String, ackType, true));
                parameters.Add(SqlHelper.CreateParameter("ACK_RESPONSE_MESSAGE", DbType.String, ackResponseMsg, true));
                parameters.Add(SqlHelper.CreateParameter("ACK_RESPONSE_DETAILS", DbType.String, ackDetails, true));
                parameters.Add(SqlHelper.CreateParameter("ACK_TIMESTAMP", DbType.DateTime, dtAck, true));
                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, siTransactionKey, true));
                parameters.Add(SqlHelper.CreateParameter("MODULE_TRANSACTION_ID", DbType.String, moduleTransactionId, true));
                parameters.Add(SqlHelper.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, additionalModuleId, true));
                parameters.Add(SqlHelper.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlHelper.CreateParameter("SUBSCRIBER_SYSTEM", DbType.String, subscriberSystem, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.String, request, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE", DbType.String, response, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, responseDetails, true));
                parameters.Add(SqlHelper.CreateParameter("EDI_RESPONSECODE", DbType.String, ediResponse, true));
                parameters.Add(SqlHelper.CreateParameter("EDI_RESPONSEMESSAGE", DbType.String, ediResMsg, true));
                parameters.Add(SqlHelper.CreateParameter("EDI_TOKEN", DbType.String, ediToken, true));
                returnVal = Methods.GetIntValue(InfoAccess.ExecuteScalar("insertPASSTHROUGH_TRANSACTIONQUEUE", parameters));

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return returnVal;
        }
        public static void UpdateTransactionQueueFIResponse(int transactionQueueId, string responseCode, string responseType, string responseMessage, DateTime modifiedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionQueueId, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, responseMessage, true));

                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));

                InfoAccess.ExecuteStoredProcedure("usp_UpdatePASSTHROUGH_TRANSACTIONQUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateTransactionQueueFIResponse(int transactionQueueId, string responseCode, string responseType, string responseMessage, DateTime modifiedDate, Guid modifiedBy, string siTransactionKey, string responseDetails)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionQueueId, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, responseMessage, true));

                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));

                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, siTransactionKey, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, responseDetails, true));
                InfoAccess.ExecuteStoredProcedure("usp_UpdatePASSTHROUGH_TRANSACTIONQUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectPassThroughTransactionsByType(int transactionType, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TransactionType", DbType.Int32, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectPassThroughTransactionsByType", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataSet SelectPassThroughClaimsTransactionsByType(int transactionType, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TransactionType", DbType.Int32, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectPassThroughClaimsTransactionsByType", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataSet Select1099TransactionsByType(string transactionType, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TransactionType", DbType.String, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_Select1099TransactionsByType", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectHospiceTransactionsByType(string transactionType, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TransactionType", DbType.String, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectHospiceTransactionsByType", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet SelectMemEligbTransactionsByType(string transactionType, int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TransactionType", DbType.String, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectMemEligbTransactionsByType", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static DataSet SelectDelegateAffiliationUploadResults(int pageSize, int startRowIndex, bool getTotalRowCount, out int totalResultCount)
        {
            try
            {
                DataSet ds;
                string outValue;
                totalResultCount = 0;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PageSize", DbType.Int32, pageSize, true));
                parameters.Add(SqlHelper.CreateParameter("StartRowIndex", DbType.Int32, startRowIndex, true));
                parameters.Add(SqlHelper.CreateParameter("GetTotalResultCount", DbType.Boolean, getTotalRowCount, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_SelectDelegateAffiliationUploadResults", parameters, "TotalResultCount", SqlDbType.Int, out outValue, 0);

                if (!string.IsNullOrEmpty(outValue))
                {
                    totalResultCount = Convert.ToInt32(outValue);
                }

                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetPassThroughTransactionsPAXMLByID(int transactionID)
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionID, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetPassThroughTransactionsPAXMLByID", parameters, "GetPassThroughTransactionsPAXMLByID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetPassThroughTransactionsClaimsXMLByID(int transactionID)
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionID, true));
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetPassThroughTransactionsClaimsXMLByID", parameters, "GetPassThroughTransactionsClaimsXMLByID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet Get1099TransactionsPAXMLByID(string transactionID, string keyType = "SI")
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (keyType.Equals("SI"))
                {
                    parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, transactionID, true));
                }
                else
                {
                    parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, transactionID, true));
                }
                ds = InfoAccess.ExecuteStoredProcedure("usp_Get1099ProviderFinancialXMLByID", parameters, "Get1099ProviderFinancialXMLByID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetHospiceTransactionsXMLByID(string transactionID, string keyType = "SI")
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (keyType.Equals("SI"))
                {
                    parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, transactionID, true));
                }
                else
                {
                    parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, transactionID, true));
                }
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetHospiceTransactionsXMLByID", parameters, "GetHospiceTransactionsXMLByID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetMemEligbTransactionsXMLByID(string transactionID, string keyType = "SI")
        {
            try
            {
                DataSet ds;
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (keyType.Equals("SI"))
                {
                    parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, transactionID, true));
                }
                else
                {
                    parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, transactionID, true));
                }
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetMemEligbTransactionsXMLByID", parameters, "GetMemEligbTransactionsXMLByID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}