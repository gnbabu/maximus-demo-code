using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.VendorAckService
{
    public static class VendorAckServiceHelper
    {

        public static void processVendorAcknowledgment(Logging log, string SIKey, VendorAcknowledgmentRequestReference req)
        {
            try
            {
                string responseCode = req.vendorAckSubResponse[0].ResponseCode.ToString();
                string responseType = req.vendorAckSubResponse[0].ResponseType.ToString();
                string responseMessage = req.vendorAckSubResponse[0].ResponseMessage.ToString();

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SIKey, true));

                parameters.Add(SqlParms.CreateParameter("ACK_RESPONSE_CODE", DbType.String, responseCode, true));
                parameters.Add(SqlParms.CreateParameter("ACK_RESPONSE_TYPE", DbType.String, responseType, true));
                parameters.Add(SqlParms.CreateParameter("ACK_RESPONSE_MESSAGE", DbType.String, responseMessage, true));

                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.VendorAckService), true));
                DataAccess.ExecuteStoredProcedure("usp_updateVendorACKTQBySIKey", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static bool checkifSIKeyExists(Logging log, string SIKey)
        {
            try
            {
                DataSet ds = null;
                Boolean isExists = false;

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SIKey, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_checkifSIKeyExistsBySIKey", parameters, "VendorACKTQBySIKey");
                if (Methods.HasRows(ds))
                {
                    isExists = true;
                }
                return isExists;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return false;
            }
        }

        public static VendorAcknowledgmentResponseReference successResponse(Logging log, Guid requestID, VendorAcknowledgmentRequestReference request)
        {
            try
            {

                VendorAckSubResponse[] resp = new VendorAckSubResponse[1];

                VendorAckSubResponse res = new VendorAckSubResponse();
                res.SequenceNumber = "1";
                res.ResponseCode = "1001"; // SUCCESS
                res.ResponseType = "SUCCESS";
                res.ResponseMessage = "Request Processed Successfully";

                resp[0] = res;

                VendorAcknowledgmentResult ackRes1 = new VendorAcknowledgmentResult();
                ackRes1.BusinessFlow = request.messageHeaderVendorAckRequest.BusinessFlow;
                ackRes1.RequestorSystem = request.messageHeaderVendorAckRequest.RequestorSystem;
                ackRes1.RequestorSystemId = request.messageHeaderVendorAckRequest.RequestorSystemId;
                ackRes1.RequestorTransactionId = request.messageHeaderVendorAckRequest.RequestorTransactionId;
                ackRes1.TargetSystem = request.messageHeaderVendorAckRequest.TargetSystem;
                ackRes1.TargetTransactionId = request.messageHeaderVendorAckRequest.TargetTransactionId;
                ackRes1.ModuleTransactionId = request.messageHeaderVendorAckRequest.ModuleTransactionId;
                ackRes1.AdditionalModuleTransactionId = request.messageHeaderVendorAckRequest.AdditionalModuleTransactionId;
                ackRes1.SITransactionKey = request.messageHeaderVendorAckRequest.SITransactionKey;
                ackRes1.TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                ackRes1.Response = resp;

                VendorAcknowledgmentResponseReference r1 = new VendorAcknowledgmentResponseReference();
                r1.resse = ackRes1;

                return r1;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return vendorAcknowledgementResponseGenericError(log, request);
            }
        }

        public static VendorAcknowledgmentResponseReference vendorAcknowledgementDataRespError(Logging log, string errorCode, string errorDescription, Guid requestID
            , VendorAcknowledgmentRequestReference request)
        {
            try
            {
                VendorAckSubResponse[] resp = new VendorAckSubResponse[1];

                VendorAckSubResponse res = new VendorAckSubResponse();
                res.SequenceNumber = "1";
                res.ResponseCode = "1000"; // failure
                res.ResponseType = "FAILURE";
                res.ResponseMessage = "Error while processing the request";

                resp[0] = res;

                VendorAcknowledgmentResult ackRes1 = new VendorAcknowledgmentResult();
                ackRes1.BusinessFlow = request.messageHeaderVendorAckRequest.BusinessFlow;
                ackRes1.RequestorSystem = request.messageHeaderVendorAckRequest.RequestorSystem;
                ackRes1.RequestorSystemId = request.messageHeaderVendorAckRequest.RequestorSystemId;
                ackRes1.RequestorTransactionId = request.messageHeaderVendorAckRequest.RequestorTransactionId;
                ackRes1.TargetSystem = request.messageHeaderVendorAckRequest.TargetSystem;
                ackRes1.TargetTransactionId = request.messageHeaderVendorAckRequest.TargetTransactionId;
                ackRes1.ModuleTransactionId = request.messageHeaderVendorAckRequest.ModuleTransactionId;
                ackRes1.AdditionalModuleTransactionId = request.messageHeaderVendorAckRequest.AdditionalModuleTransactionId;
                ackRes1.SITransactionKey = request.messageHeaderVendorAckRequest.SITransactionKey;
                ackRes1.TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                ackRes1.Response = resp;

                VendorAcknowledgmentResponseReference r1 = new VendorAcknowledgmentResponseReference();
                r1.resse = ackRes1;

                return r1;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return vendorAcknowledgementResponseGenericError(log, request);
            }
        }


        public static VendorAcknowledgmentResponseReference vendorAcknowledgementResponseGenericError(Logging log, VendorAcknowledgmentRequestReference request)
        {
            VendorAckSubResponse[] resp = new VendorAckSubResponse[1];

            VendorAckSubResponse res = new VendorAckSubResponse();
            res.SequenceNumber = "1";
            res.ResponseCode = "1000"; // failure
            res.ResponseType = "FAILURE";
            res.ResponseMessage = "Error while processing the request";

            resp[0] = res;

            VendorAcknowledgmentResult ackRes1 = new VendorAcknowledgmentResult();
            ackRes1.BusinessFlow = request.messageHeaderVendorAckRequest.BusinessFlow;
            ackRes1.RequestorSystem = request.messageHeaderVendorAckRequest.RequestorSystem;
            ackRes1.RequestorSystemId = request.messageHeaderVendorAckRequest.RequestorSystemId;
            ackRes1.RequestorTransactionId = request.messageHeaderVendorAckRequest.RequestorTransactionId;
            ackRes1.TargetSystem = request.messageHeaderVendorAckRequest.TargetSystem;
            ackRes1.TargetTransactionId = request.messageHeaderVendorAckRequest.TargetTransactionId;
            ackRes1.ModuleTransactionId = request.messageHeaderVendorAckRequest.ModuleTransactionId;
            ackRes1.AdditionalModuleTransactionId = request.messageHeaderVendorAckRequest.AdditionalModuleTransactionId;
            ackRes1.SITransactionKey = request.messageHeaderVendorAckRequest.SITransactionKey;
            ackRes1.TimeStamp = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            ackRes1.Response = resp;

            VendorAcknowledgmentResponseReference r1 = new VendorAcknowledgmentResponseReference();
            r1.resse = ackRes1;

            return r1;
        }


        public static void saveRequestResponse(Logging log, Guid requestID, string BusinessFlow, string requestorSystem,
            string moduleTransactionID, string AdditionalModuleTransactionID, string SITransactionKey, string requestPayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                parameters.Add(SqlParms.CreateParameter("BUSINESS_FLOW", DbType.String, BusinessFlow, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlParms.CreateParameter("MODULE_TRANSACTION_QUEUE_ID", DbType.String, moduleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, AdditionalModuleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestPayload, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.VendorAckService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_ACKNOWLEDGMENT_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string vendorAckParseXMLToStringReq(Logging log, VendorAcknowledgmentRequestReference req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(VendorAcknowledgmentRequestReference));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, req, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

    }
}