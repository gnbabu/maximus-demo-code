using Corp.Core.Libraries.ServiceAgent;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Security.Policy;
using System.Web.UI.WebControls;
using System.Xml;

namespace Corp.Core.Libraries
{
    public static class InfoAccessController
    {

        public static DataSet GetStagingProviderEnrollmentByTransactionID(int transactionID)
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("TransactionID", DbType.Int32, transactionID, false));
            
            ds = InfoAccess.ExecuteStoredProcedure("usp_GetStagingProviderEnrollmentByRegId_New", parameters, "StagingProviderEnrollment");
            return ds;
        }
    

        public static void SaveFaultCodeException(string faultString, string faultCode, int transactionID, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("ModuleTransactionId", DbType.Int32, transactionID, false));
                parameters.Add(SqlHelper.CreateParameter("FaultString", DbType.String, faultString, false));
                parameters.Add(SqlHelper.CreateParameter("FaultCode", DbType.String, faultCode, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, false));
                parameters.Add(SqlHelper.CreateParameter("PNM_TRANSACTION_KEY", DbType.String, pnmTransactionKey, false));
                InfoAccess.ExecuteStoredProcedure("updateTransaction_Queue", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveSoapResponseCodeException(int ModuleTransactionId, string SITransactionKey, string ResponseCode, string ResponseDetails,
            string ResponseMessage, string ResponseType, DateTime modifiedDate, Guid modifiedBy, string rawResponse, string pnmTransactionKey = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("ModuleTransactionId", DbType.Int32, ModuleTransactionId, false));
                parameters.Add(SqlHelper.CreateParameter("SITransactionKey", DbType.String, SITransactionKey, false));
                parameters.Add(SqlHelper.CreateParameter("ResponseCode", DbType.String, ResponseCode, false));
                parameters.Add(SqlHelper.CreateParameter("ResponseDetails", DbType.String, ResponseDetails, false));
                parameters.Add(SqlHelper.CreateParameter("ResponseMessage", DbType.String, ResponseMessage, false));
                parameters.Add(SqlHelper.CreateParameter("ResponseType", DbType.String, ResponseType, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, false));
                parameters.Add(SqlHelper.CreateParameter("PNM_TRANSACTION_KEY", DbType.String, pnmTransactionKey, false));
                InfoAccess.ExecuteStoredProcedure("updateTransaction_Queue", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveSoapPriorAuthRequestPayload(int TransactionId, DateTime modifiedDate, Guid modifiedBy, string rawResponse)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, TransactionId, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.Xml, rawResponse, false));
                InfoAccess.ExecuteStoredProcedure("usp_UpdatePASSTHROUGH_TRANSACTIONQUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveSoapPriorAuthResponseCodeException(int TransactionId, string SITransactionKey, string ResponseCode, string ResponseDetails,
    string ResponseMessage, string ResponseType, DateTime modifiedDate, Guid modifiedBy, string rawResponse)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, TransactionId, false));
                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, ResponseCode, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, ResponseDetails, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, ResponseMessage, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, ResponseType, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE", DbType.Xml, rawResponse, false));
                InfoAccess.ExecuteStoredProcedure("usp_UpdatePASSTHROUGH_TRANSACTIONQUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveFaultCodePriorAuthException(string faultString, string faultCode, int transactionID, DateTime modifiedDate, Guid modifiedBy, string rawResponse)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionID, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, faultString, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, faultCode, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE", DbType.Xml, rawResponse, false));
                InfoAccess.ExecuteStoredProcedure("usp_UpdatePASSTHROUGH_TRANSACTIONQUEUE", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdatePriorAuthTransactionQueueSIResponse(int transactionQueueId, string responseCode, string responseType, string responseMessage, DateTime modifiedDate, Guid modifiedBy)
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

        public static void SaveSoapResponseCodePPException(int ModuleTransactionId, string SITransactionKey, DateTime modifiedDate, Guid modifiedBy, 
            string rawResponse, ResponseElement[] resp, string pnmTransactionKey = "")
        {
            try
            {
                int IsPartial = 1;
                List<SqlParameter> parameters;


                foreach (ResponseElement respOne in resp)
                {

                    parameters = new List<SqlParameter>();
                    parameters.Add(SqlHelper.CreateParameter("ModuleTransactionId", DbType.Int32, ModuleTransactionId, false));
                    parameters.Add(SqlHelper.CreateParameter("ResponseCode", DbType.String, respOne.ResponseCode, false));
                    parameters.Add(SqlHelper.CreateParameter("ResponseDetails", DbType.String, respOne.ResponseDetails, false));
                    parameters.Add(SqlHelper.CreateParameter("ResponseMessage", DbType.String, respOne.ResponseMessage, false));
                    parameters.Add(SqlHelper.CreateParameter("ResponseType", DbType.String, respOne.ResponseType, false));
                    parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                    parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                    InfoAccess.ExecuteStoredProcedure("usp_InsertOUTBOUND_PROV_MGMT_RESPONSE", parameters);
                }

                parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("ModuleTransactionId", DbType.Int32, ModuleTransactionId, false));
                parameters.Add(SqlHelper.CreateParameter("SITransactionKey", DbType.String, SITransactionKey, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                parameters.Add(SqlHelper.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, false));
                parameters.Add(SqlHelper.CreateParameter("PNM_TRANSACTION_KEY", DbType.String, pnmTransactionKey, false));
                parameters.Add(SqlHelper.CreateParameter("IsPartial", DbType.Int32, IsPartial, false));
                InfoAccess.ExecuteStoredProcedure("updateTransaction_Queue", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void PopulateStagingData(int transactionID, string transactionType, string Result, bool SendHistory = false)
        {
            try
            {
                Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                PreProcessingDataFixes.PreProcessingDataFixesMain(log, transactionID);
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                log.CreateLogEntry(string.Format("Error in PreProcessingDataFixesMain: {0} ", ex.Message + " " + ex.StackTrace));
            }

            try
            {
                // generated by sp_Admin_StoredProcBuilder on Sep 11 2013  5:21PM
                // create parameters objects and fill with values
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                parameters.Add(SqlHelper.CreateParameter("transactionType", DbType.String, transactionType, true));
                parameters.Add(SqlHelper.CreateParameter("Result", DbType.String, Result, true));
                parameters.Add(SqlHelper.CreateParameter("SendHistory", DbType.Boolean, SendHistory, true));
                InfoAccess.ExecuteStoredProcedure("usp_TransferPNMToStaging", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            try
            {
                PostProcessStageDataCleanup(transactionID);
            }
            catch (Exception ex)
            {
                Logging log = new Logging(Guid.NewGuid(), "PopulateStagingData");
                log.CreateLogEntry(string.Format("Error in PostProcessStageDataCleanup: {0} ", ex.Message + " " + ex.StackTrace));
            }
        }

        public static void PostProcessStageDataCleanup(int transactionID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("transaction_queue_id", DbType.Int32, transactionID, true));
                InfoAccess.ExecuteStoredProcedure("usp_PostStageCleanupData", parameters);
            }
            catch (Exception Ex)
            {
                string logMsg = String.Format(Constants.LogString.MethodSignature, MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name);
                Logging log = new Logging(Guid.NewGuid(), logMsg);
                log.CreateLogEntry(string.Format("Error in PostStageCleanupData: {0} ", Ex));
            }
        }

        public static void UpdateTransactionQueue(
            int transactionQueueId
            , DateTime? submitDate
            , DateTime? processDate
            , DateTime? changedDate
            , string changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transactionQueueId, true));
                parameters.Add(SqlHelper.CreateParameter("SUBMIT_DATE_TIME", DbType.DateTime, submitDate, true));
                parameters.Add(SqlHelper.CreateParameter("PROCESS_DATE_TIME", DbType.DateTime, processDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, changedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, true));

                InfoAccess.ExecuteStoredProcedure("usp_SaveTransaction", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int SaveIMSReqRes(int RegID, string IncidentCaseNumber, string RequestFor, string Request, string modifiedBy)
        {
            int rowID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, RegID, false));
                parameters.Add(SqlHelper.CreateParameter("INCIDENT_CASE_NUMBER", DbType.String, IncidentCaseNumber, false));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_FOR", DbType.String, RequestFor, false));
                parameters.Add(SqlHelper.CreateParameter("REQUEST", DbType.String, Request, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                rowID = Convert.ToInt32(InfoAccess.ExecuteScalar("insertINCIDENT_MANAGEMENT_SERVICE_REQ_RES", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowID;
        }

        public static void UpdateIMSReqRes(int rowID, string response, string modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("INCIDENT_MANAGEMENT_SERVICE_REQ_RES_ID", DbType.Int32, rowID, false));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE", DbType.String, response, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, false));
                InfoAccess.ExecuteStoredProcedure("updateINCIDENT_MANAGEMENT_SERVICE_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveProviderMgmtXMLRecord(int moduleTransactionID, string SITransKey, string requestor_System, string subscriber_System, string additional_Module_TransID, string requestPayload, string responseCode, string responseMessage, string responseType, string responseDtls, string odsProvInfoId, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("MODULE_TRANSACTION_ID", DbType.Int32, moduleTransactionID, true));
                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransKey, true));
                parameters.Add(SqlHelper.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestor_System, true));
                parameters.Add(SqlHelper.CreateParameter("SUBSCRIBER_SYSTEM", DbType.String, subscriber_System, true));
                parameters.Add(SqlHelper.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, additional_Module_TransID, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_MESSAGE", DbType.String, responseMessage, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("RESPONSE_DETAILS", DbType.String, responseDtls, true));
                parameters.Add(SqlHelper.CreateParameter("ODSProviderDemographicsId", DbType.String, odsProvInfoId, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.Xml, requestPayload, true));
                InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_PROV_MGMT_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void UpdateTransactionQueueSIResponse(int transactionQueueId, string responseCode, string responseType, string responseMessage, DateTime modifiedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                parameters.Add(SqlHelper.CreateParameter("TRANSACTION_QUEUE_ID", DbType.Int32, transactionQueueId, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, responseMessage, true));

                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));

                InfoAccess.ExecuteStoredProcedure("usp_UpdatePNMErrorTransactionResponse", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateFinancialServiceSIResponse(string providerID, string pnmKey, string siKey, string reqType, string responseCode, string responseType, string responseMessage, DateTime modifiedDate, Guid modifiedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PROVIDER_ID", DbType.String, providerID, true));
                parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmKey, true));
                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, siKey, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, reqType, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, responseMessage, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, modifiedDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, modifiedBy, true));

                InfoAccess.ExecuteStoredProcedure("usp_UpdateFinancialServiceSIResponse", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveFinancialServiceXMLRecord(string providerID, string pnmKey, string reqType, string requestPayload, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PROVIDER_ID", DbType.String, providerID, true));
                parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmKey, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, reqType, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.Xml, requestPayload, true));
                InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_FINANCIAL_SERVICE_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SaveFinancialServiceReqRes(string pnmTransKey, string request_Type, string siKey, string respCode, string respType, string respMsg, 
            string requestPayload, string responsePayload, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmTransKey, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, request_Type, true));
                parameters.Add(SqlHelper.CreateParameter("SI_TRANSACTION_KEY", DbType.String, siKey, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_CODE", DbType.String, respCode, true));
                parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_TYPE", DbType.String, respType, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.Xml, requestPayload, true));
                if (string.IsNullOrEmpty(responsePayload) || !Methods.IsValidXml(responsePayload))
                {
                    parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, responsePayload, true));
                }
                else
                {
                    parameters.Add(SqlHelper.CreateParameter("SI_RESPONSE_MESSAGE", DbType.String, respMsg, true));
                    parameters.Add(SqlHelper.CreateParameter("RESPONSE_PAYLOAD", DbType.Xml, responsePayload, true));
                }
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));

                InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_FINANCIAL_SERVICE_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static void SaveFinancialServiceRawResponse(string pnmTransKey, string request_Type, string rawResponse, Guid createdBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PNM_KEY", DbType.String, pnmTransKey, true));
                parameters.Add(SqlHelper.CreateParameter("REQUEST_TYPE", DbType.String, request_Type, true));
                parameters.Add(SqlHelper.CreateParameter("RAW_RESPONSE", DbType.String, rawResponse, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, createdBy, true));
                InfoAccess.ExecuteStoredProcedure("insertOUTBOUND_FINANCIAL_SERVICE_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int InsertPASSTHROUGH_TRANSACTIONQUEUE(
            int transactionTypeId
            , DateTime? createDate
            , DateTime? submitDate
            , DateTime? processDate
            , Guid changedBy)
        {
            int rowID = 0;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_REQUEST_TYPE_ID", DbType.Int32, transactionTypeId, false));
                parameters.Add(SqlHelper.CreateParameter("SUBMIT_DATE_TIME", DbType.DateTime, submitDate, false));
                parameters.Add(SqlHelper.CreateParameter("PROCESS_DATE_TIME", DbType.DateTime, processDate, false));
                parameters.Add(SqlHelper.CreateParameter("CREATED_DATE_TIME", DbType.DateTime, createDate, false));
                parameters.Add(SqlHelper.CreateParameter("CREATED_BY_USER", DbType.Guid, changedBy, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, false));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, changedBy, false));
                rowID = Convert.ToInt32(InfoAccess.ExecuteScalar("usp_InsertPASSTHROUGH_TRANSACTIONQUEUE", parameters));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return rowID;
        }



        public static void InsertPassthroughTransactionQueueAttachment(
            int PassthroughTransactionQueueId
            , int OutboundDocumentUploadsId
            , string AttachmentControlNumber
            , DateTime? createDate
            , Guid changedBy)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PassthroughTransactionQueueId", DbType.Int32, PassthroughTransactionQueueId, false));
                parameters.Add(SqlHelper.CreateParameter("OUTBOUND_DOCUMENT_UPLOADS_ID", DbType.Int32, OutboundDocumentUploadsId, false));
                parameters.Add(SqlHelper.CreateParameter("AttachmentControlNumber", DbType.String, AttachmentControlNumber, false));
                parameters.Add(SqlHelper.CreateParameter("CreatedDateTime", DbType.DateTime, createDate, false));
                parameters.Add(SqlHelper.CreateParameter("CreatedUser", DbType.Guid, changedBy, false));
                InfoAccess.ExecuteStoredProcedure("usp_InsertPassthroughTransactionQueueAttachment", parameters, "PassthroughTransactionQueueAttachment");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetResponsePayloadByPassThroughTransactionID(int transactionID)
        {
            string responsePayload = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("PASSTHROUGH_TRANSACTIONQUEUE_ID", DbType.Int32, transactionID, false));
                responsePayload = InfoAccess.ExecuteScalar("usp_GetResponsePayloadByPassThroughTransactionID", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responsePayload;
        }

        public static DataSet getServiceProviderDetailsByMedID(string medID)
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("MEDICAID_ID", DbType.String, medID, false));



            ds = InfoAccess.ExecuteStoredProcedure("usp_GetServiceProviderDetailsByMedID", parameters, "GetServiceProviderDetailsByMedID");
            return ds;
        }



        public static DataSet GetServiceProviderDetailsByGRPMedicaidID(string medID)
        {
            DataSet ds = new DataSet();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("GRPMedicaid_ID", DbType.String, medID, false));
            ds = InfoAccess.ExecuteStoredProcedure("usp_SelectProviderByGRPMedicaidID", parameters, "RegData_" + medID);
            ds.Tables[0].TableName = "RegData";
            return ds;
        }

       

        public static DataSet SelectRegAddressesWithoutCountyByRegID(int regId, int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlHelper.CreateParameter("SPOPERATION", DbType.Int32, ops, true));
                DataSet ds = new DataSet();
                ds = InfoAccess.ExecuteStoredProcedure("usp_DMLRegAddressesWithoutCountyByRegID", parameters, "RegAddressesWithoutCounty_");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateRegAddressesWithoutCountyByRegID(int regId, int regAddrID, string county, string state,  int ops)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlHelper.CreateParameter("SPOPERATION", DbType.Int32, ops, true));
                parameters.Add(SqlHelper.CreateParameter("REG_ADDRESS_ID", DbType.Int32, regAddrID, true));
                parameters.Add(SqlHelper.CreateParameter("COUNTY", DbType.String, county, true));
                parameters.Add(SqlHelper.CreateParameter("STATE", DbType.String, state, true));
                InfoAccess.ExecuteStoredProcedure("usp_DMLRegAddressesWithoutCountyByRegID", parameters, "UPRegAddressesWithoutCounty_");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetRegIDfromTransactionID(int txID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("TRANSACTION_ID", DbType.Int32, txID, true));
                DataSet ds = new DataSet();
                ds = InfoAccess.ExecuteStoredProcedure("usp_GetRegIDfromTransactionID", parameters, "GetRegIDfromTransactionID");
                return ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteDuplicateAddresses(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_DeleteDuplicateAddress", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateRegAffiliations(int Reg_Id)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
            InfoAccess.ExecuteStoredProcedure("usp_UpdateRegAffiliations", parameters);
        }

        public static void InsertAdditionalApplicationIdIfNotExistsInWFPARAMETER(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_InsertADDITIONALAPPLICATIONIDIfNotExistsInWF_PARAMETER", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void UpdateBoardCertification(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_Update_REG_BoardCertificate_markdelete", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateRegCLIA(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_Update_REG_CLIA_markdelete", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
		public static void UpdateRegSpecialty(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_Update_REG_Specialty_markdelete", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateRegAddress(int Reg_Id)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, Reg_Id, true));
                InfoAccess.ExecuteStoredProcedure("usp_Update_REG_ADDRESS_markdelete", parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int InsertUpdateAddressVerificationXMLRecord(string requestPayload, string responsePayload, string wsOperation, int searchesLeft,
            Guid lastmodBy, DateTime lastmodDate, int ops, Logging log, int eddrvID, string AddressLine1, string DeliveryLine1, string AddressLine2, string DeliveryLine2
            ,string City, string State, string ZipAddon, string CountyNumber, string CountyName, string Latitude, string Longitude, string ErrorCodes, string ReturnCodes
            , string StreetName, string StreetNumber, string StreetSuffix, string PreDirectional, string ErrorDesc, string SecondaryDesignation, string SecondaryNumber
            , string AddressZipAddon)
        {
            int rowID = 0;
            string withOutEncoding = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                if (eddrvID != 0)
                {
                    parameters.Add(SqlHelper.CreateParameter("OUTBOUND_EADDRV_REQ_RES_ID", DbType.Int32, eddrvID, true));
                }
                parameters.Add(SqlHelper.CreateParameter("WS_OPERATION", DbType.String, wsOperation, true));
                if (!string.IsNullOrEmpty(requestPayload))
                {
                    withOutEncoding = requestPayload.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    parameters.Add(SqlHelper.CreateParameter("REQUEST_PAYLOAD", DbType.Xml, withOutEncoding, true));
                }
                if (!string.IsNullOrEmpty(responsePayload))
                {
                    withOutEncoding = responsePayload.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                    parameters.Add(SqlHelper.CreateParameter("RESPONSE_PAYLOAD", DbType.Xml, withOutEncoding, true));
                }
                parameters.Add(SqlHelper.CreateParameter("SEARCHES_LEFT", DbType.Int32, searchesLeft, true));

                parameters.Add(SqlHelper.CreateParameter("AddressLine1", DbType.String, AddressLine1, true));
                parameters.Add(SqlHelper.CreateParameter("DeliveryLine1", DbType.String, DeliveryLine1, true));
                parameters.Add(SqlHelper.CreateParameter("AddressLine2", DbType.String, AddressLine2, true));
                parameters.Add(SqlHelper.CreateParameter("DeliveryLine2", DbType.String, DeliveryLine2, true));
                parameters.Add(SqlHelper.CreateParameter("City", DbType.String, City, true));
                parameters.Add(SqlHelper.CreateParameter("State", DbType.String, State, true));
                parameters.Add(SqlHelper.CreateParameter("AddressZipAddon", DbType.String, AddressZipAddon, true));
                parameters.Add(SqlHelper.CreateParameter("ZipAddon", DbType.String, ZipAddon, true));
                parameters.Add(SqlHelper.CreateParameter("CountyNumber", DbType.String, CountyNumber, true));
                parameters.Add(SqlHelper.CreateParameter("CountyName", DbType.String, CountyName, true));
                parameters.Add(SqlHelper.CreateParameter("Latitude", DbType.String, Latitude, true));
                parameters.Add(SqlHelper.CreateParameter("Longitude", DbType.String, Longitude, true));


                parameters.Add(SqlHelper.CreateParameter("ErrorCodes", DbType.String, ErrorCodes, true));
                parameters.Add(SqlHelper.CreateParameter("ReturnCodes", DbType.String, ReturnCodes, true));
                parameters.Add(SqlHelper.CreateParameter("StreetName", DbType.String, StreetName, true));
                parameters.Add(SqlHelper.CreateParameter("StreetNumber", DbType.String, StreetNumber, true));
                parameters.Add(SqlHelper.CreateParameter("StreetSuffix", DbType.String, StreetSuffix, true));
                parameters.Add(SqlHelper.CreateParameter("PreDirectional", DbType.String, PreDirectional, true));
                parameters.Add(SqlHelper.CreateParameter("ErrorDesc", DbType.String, ErrorDesc, true));

                parameters.Add(SqlHelper.CreateParameter("SecondaryDesignation", DbType.String, SecondaryDesignation, true));
                parameters.Add(SqlHelper.CreateParameter("SecondaryNumber", DbType.String, SecondaryNumber, true));

                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, lastmodDate, true));
                parameters.Add(SqlHelper.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, lastmodBy, true));
                parameters.Add(SqlHelper.CreateParameter("SOPS", DbType.Int32, ops, true));
                if (ops == 1) //insert
                {
                    string tmp = InfoAccess.ExecuteScalar("usp_InsertUpdateOUTBOUND_EADDRV_REQ_RES", parameters);
                    if (int.TryParse(tmp, out rowID))
                    { }
                }
                else // update
                {
                    InfoAccess.ExecuteStoredProcedure("usp_InsertUpdateOUTBOUND_EADDRV_REQ_RES", parameters);
                }
                return rowID;
            }
            catch (Exception ex)
            {
                log = new Logging(new Guid());
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                return 0;
            }
        }

        public static DataTable CheckAddressVerificationRequired(AddressVerificationRequest address, int addressTypeId, Logging log)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("AddressLine", DbType.String, address.AddressLine, true));
                parameters.Add(SqlHelper.CreateParameter("AddressLine2", DbType.String, address.AddressLine2, true));
                parameters.Add(SqlHelper.CreateParameter("City", DbType.String, address.City, true));
                parameters.Add(SqlHelper.CreateParameter("State", DbType.String, address.State, true));
                parameters.Add(SqlHelper.CreateParameter("PostalCode", DbType.String, address.PostalCode, true));
                parameters.Add(SqlHelper.CreateParameter("AddressTypeId", DbType.String, addressTypeId, true));
                DataSet ds = InfoAccess.ExecuteStoredProcedure("usp_CheckAddressVerificationRequired", parameters, "CheckAddressVerificationRequiredDS");
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                log = new Logging(new Guid());
                log.CreateLogEntry("Intelligent Search Address Verification: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                return null;
            }
        }


        public static void AddAddressVerificationLog(int regId, Guid username, string PageTitle, string CallType, int eddrvID)
        {
            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlHelper.CreateParameter("REG_ID", DbType.Int32, regId, true));
                parameters.Add(SqlHelper.CreateParameter("PAGE_NAME", DbType.String, PageTitle, true));
                parameters.Add(SqlHelper.CreateParameter("CALL_TYPE", DbType.String, CallType, true));

                parameters.Add(SqlHelper.CreateParameter("OUTBOUND_EADDRV_REQ_RES_ID", DbType.Int32, eddrvID, true));
                parameters.Add(SqlHelper.CreateParameter("CREATED_BY", DbType.Guid, username, true));
                InfoAccess.ExecuteStoredProcedure("usp_AddAddress_Verification_CallTrace", parameters, "AddressVerificationCallTrace");
            }
            catch (Exception ex)
            {
                Logging log = new Logging(new Guid());
                log.CreateLogEntry("Add Address Verification Log: Error " + ex.Message + " " + ex.StackTrace, Logging.LogPriority.Error);
                //supress Address Verification Call trace insert exceptions
            }
        }

    }
}
