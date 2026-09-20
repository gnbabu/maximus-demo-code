using MAXIMUS.Core.Libraries;
using MAXIMUS.Services.PDMS;
using PDMSWebAPI.ProviderApplicationStatusInq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.ProviderInquiryServiceData
{
    public static class IVRServiceDataHelper
    {
        public static ProvideAppStatusInquiryDataResponse provAppStatusInquiryDataRespError(Logging log, string errorCode, string errorDescription, Guid requestID
            , ProvideAppStatusInquiryDataRequest request)
        {
            string SITransactionKey = request.AppStatusInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.AppStatusInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.AppStatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                AppStatusInquiryResponseAppStatusInquiryError rtnError = new AppStatusInquiryResponseAppStatusInquiryError();
                rtnError.errorCode = errorCode;
                rtnError.errorDescription = errorDescription;

                AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
                rt.appStatusInquiryError = rtnError;
                rt.ResponseCode = errorCode;
                rt.ResponseMessage = errorDescription;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
                ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();
                rtn.AppStatusInquiryResponse = rt;
                responsePayload = appStatusParseXMLToStringRes(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return provideAppStatusInquiryDataResponseGenericError(log, errorCode, errorDescription, request);
            }
        }

        public static string appStatusParseXMLToStringReq(Logging log, ProvideAppStatusInquiryDataRequest req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideAppStatusInquiryDataRequest));
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

        public static string appStatusParseXMLToStringRes(Logging log, ProvideAppStatusInquiryDataResponse res)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideAppStatusInquiryDataResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, res, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return "fail";
            }
        }

        public static ProvideAppStatusInquiryDataResponse provideAppStatusInquiryDataResponseGenericError(Logging log, string errorCode, string errorDescription,
            ProvideAppStatusInquiryDataRequest request)
        {
            string SITransactionKey = request.AppStatusInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.AppStatusInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.AppStatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            AppStatusInquiryResponseAppStatusInquiryError rtnError = new AppStatusInquiryResponseAppStatusInquiryError();
            rtnError.errorCode = errorCode;
            rtnError.errorDescription = errorDescription;

            AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
            rt.SITransactionKey = SITransactionKey;
            rt.ModuleTransactionId = moduleTransactionID;
            rt.AdditionalModuleTransactionId = addnModuleTransactionID;
            rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
            rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
            rt.ResponseMessage = errorDescription;
            ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();
            rtn.AppStatusInquiryResponse = rt;
            return rtn;
        }

        public static DataSet checkApplicationStatus(Logging log, int regID, int serviceOP)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                parameters.Add(SqlParms.CreateParameter("ServiceOP", DbType.Int32, serviceOP, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationStatusForIVR", parameters, "CheckApplicationStatusForIVR");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static ProvideAppStatusInquiryDataResponse successResponse(Logging log, string appStatus, string appStatusDescp, Guid requestID,
            ProvideAppStatusInquiryDataRequest request)
        {
            string SITransactionKey = request.AppStatusInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.AppStatusInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.AppStatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                AppStatusInquiryResponseAppStatusInquiry rtnInq = new AppStatusInquiryResponseAppStatusInquiry();
                rtnInq.appStatusCode = appStatus;
                rtnInq.appStatusDescription = appStatusDescp;
                AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
                rt.appStatusInquiry = rtnInq;
                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = "Success";
                rt.ResponseType = AppStatusInquiryResponseResponseType.Success;
                rt.ResponseCode = "1000";
                rt.ResponseMessage = "Request Received and Processed Successfully";
                ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();

                rtn.AppStatusInquiryResponse = rt;
                responsePayload = appStatusParseXMLToStringRes(log, rtn);
                saveRequestResponse(log, requestID, SITransactionKey, appStatus, "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return provAppStatusInquiryDataRespError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public static void saveRequestResponse(Logging log, Guid requestID, string param1, string param2, string responseStatus, string responsePayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                if (responseStatus.Equals("Success"))
                {
                    parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, param1, true));
                    parameters.Add(SqlParms.CreateParameter("APP_STATUS_INQUIRY_RES", DbType.String, param2, true));
                }
                else
                {
                    parameters.Add(SqlParms.CreateParameter("ERROR_CODE", DbType.String, param1, true));
                    parameters.Add(SqlParms.CreateParameter("ERROR_DESCP", DbType.String, param2, true));
                }
                parameters.Add(SqlParms.CreateParameter("RESPONSE_STATUS", DbType.String, responseStatus, true));
                parameters.Add(SqlParms.CreateParameter("RESPONSE_PAYLOAD", DbType.String, responsePayload, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.IVRService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_IVR_APPSTATUS_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static void saveRequestResponse(Logging log, Guid requestID, string BusinessFlow, string StateCode, string requestorSystem,
            string moduleTransactionID, string AdditionalModuleTransactionID, string SITransactionKey, string requestPayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                parameters.Add(SqlParms.CreateParameter("BUSINESS_FLOW", DbType.String, BusinessFlow, true));
                parameters.Add(SqlParms.CreateParameter("STATE_CODE", DbType.String, StateCode, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_ID", DbType.String, moduleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, AdditionalModuleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestPayload, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid(CON.WebApiServiceGuid.IVRService), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_IVR_APPSTATUS_REQ_RES", parameters);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static DataSet checkStatusApplicationStatus(Logging log, string providerNumber, string npi, int serviceOP)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNumber", DbType.String, providerNumber, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, npi, true));
                parameters.Add(SqlParms.CreateParameter("ServiceOP", DbType.Int32, serviceOP, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationStatusForIVR", parameters, "CheckApplicationStatusForIVR");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        //

        public static ProvideStatusInquiryDataResponse provStatusInquiryDataResponseGenericError(Logging log, string SITransactionKey, string errorCode, string errorDescription)
        {
            StatusInquiryResponseStatusInquiryError rtnError = new StatusInquiryResponseStatusInquiryError();
            rtnError.errorCode = errorCode;
            rtnError.errorDescription = errorDescription;

            StatusInquiryResponse rt = new StatusInquiryResponse();
            rt.SITransactionKey = SITransactionKey;
            rt.responseStatus = "Failure";
            rt.statusInquiryError = rtnError;
            ProvideStatusInquiryDataResponse rtn = new ProvideStatusInquiryDataResponse();
            rtn.StatusInquiryResponse = rt;
            return rtn;
        }

        public static ProvideStatusInquiryDataResponse provideStatusInquiryDataResponseError(Logging log, string errorCode, string errorDescription,
            Guid requestID, ProvideStatusInquiryDataRequest request)
        {

            string SITransactionKey = request.StatusInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.StatusInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.StatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                StatusInquiryResponseStatusInquiryError rtnError = new StatusInquiryResponseStatusInquiryError();
                rtnError.errorCode = errorCode;
                rtnError.errorDescription = errorDescription;

                StatusInquiryResponse rt = new StatusInquiryResponse();
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Failure";
                rt.statusInquiryError = rtnError;

                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
                rt.ResponseMessage = errorDescription;
                rt.ResponseCode = errorCode;

                ProvideStatusInquiryDataResponse rtn = new ProvideStatusInquiryDataResponse();
                rtn.StatusInquiryResponse = rt;
                responsePayload = provStatusParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return IVRServiceDataHelper.provStatusInquiryDataResponseGenericError(log, SITransactionKey, errorCode, errorDescription);
            }
        }

        public static ProvideStatusInquiryDataResponse provideStatusInquiryDataResponseSuccessResp(Logging log, string appStatus, string appStatusDescp, Guid requestID,
            ProvideStatusInquiryDataRequest request)
        {
            string SITransactionKey = request.StatusInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.StatusInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.StatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                StatusInquiryResponseStatusInquiry rtnInq = new StatusInquiryResponseStatusInquiry();
                rtnInq.status = appStatus;
                StatusInquiryResponse rt = new StatusInquiryResponse();
                rt.statusInquiry = rtnInq;
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Success";

                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = "Success";
                rt.ResponseType = StatusInquiryResponseResponseType.Success;
                rt.ResponseCode = "1000";
                rt.ResponseMessage = "Request Received and Processed Successfully";

                ProvideStatusInquiryDataResponse rtn = new ProvideStatusInquiryDataResponse();

                rtn.StatusInquiryResponse = rt;
                responsePayload = provStatusParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, SITransactionKey, appStatus, "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return provideStatusInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public static string provStatusParseXMLToStringResp(Logging log, ProvideStatusInquiryDataResponse res)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideStatusInquiryDataResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, res, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static string provStatusParseXMLToStringReq(Logging log, ProvideStatusInquiryDataRequest req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideStatusInquiryDataRequest));
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

        //

        public static string provStatusParseXMLToStringReq(Logging log, ProvideAuthInquiryDataRequest req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideAuthInquiryDataRequest));
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

        public static ProvideAuthInquiryDataResponse provideAuthInquiryDataResponseError(Logging log, string errorCode, string errorDescription,
            Guid requestID, ProvideAuthInquiryDataRequest request)
        {

            string SITransactionKey = request.AuthInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.AuthInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.AuthInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                AuthInquiryResponseAuthInquiryError rtnError = new AuthInquiryResponseAuthInquiryError();
                rtnError.errorCode = errorCode;
                rtnError.errorDescription = errorDescription;

                AuthInquiryResponse rt = new AuthInquiryResponse();
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Failure";
                rt.authInquiryError = rtnError;

                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
                rt.ResponseMessage = errorDescription;
                rt.ResponseCode = errorCode;

                ProvideAuthInquiryDataResponse rtn = new ProvideAuthInquiryDataResponse();
                rtn.AuthInquiryResponse = rt;
                responsePayload = provAuthParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return IVRServiceDataHelper.provideAuthInquiryDataResponseGenericError(log, SITransactionKey, errorCode, errorDescription);
            }
        }

        public static string provAuthParseXMLToStringResp(Logging log, ProvideAuthInquiryDataResponse res)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideAuthInquiryDataResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, res, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static ProvideAuthInquiryDataResponse provideAuthInquiryDataResponseGenericError(Logging log, string SITransactionKey, string errorCode, string errorDescription)
        {
            AuthInquiryResponseAuthInquiryError rtnError = new AuthInquiryResponseAuthInquiryError();
            rtnError.errorCode = errorCode;
            rtnError.errorDescription = errorDescription;

            AuthInquiryResponse rt = new AuthInquiryResponse();
            rt.SITransactionKey = SITransactionKey;
            rt.responseStatus = "Failure";
            rt.authInquiryError = rtnError;
            ProvideAuthInquiryDataResponse rtn = new ProvideAuthInquiryDataResponse();
            rtn.AuthInquiryResponse = rt;
            return rtn;
        }

        public static ProvideAuthInquiryDataResponse provideAuthInquiryDataResponseSuccessResp(Logging log, string appStatus, string appStatusDescp, Guid requestID,
            ProvideAuthInquiryDataRequest request)
        {
            string SITransactionKey = request.AuthInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.AuthInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.AuthInquiryRequest.MessageHeader.AdditionalModuleTransactionId;

            try
            {
                string responsePayload = string.Empty;
                AuthInquiryResponseAuthInquiry rtnInq = new AuthInquiryResponseAuthInquiry();
                rtnInq.status = appStatus;
                AuthInquiryResponse rt = new AuthInquiryResponse();
                rt.authInquiry = rtnInq;
                rt.SITransactionKey = SITransactionKey;
                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = "Success";
                rt.ResponseType = AuthInquiryResponseResponseType.Success;
                rt.ResponseCode = "1000";
                rt.ResponseMessage = "Request Received and Processed Successfully";
                rt.responseStatus = "Success";
                ProvideAuthInquiryDataResponse rtn = new ProvideAuthInquiryDataResponse();

                rtn.AuthInquiryResponse = rt;
                responsePayload = provAuthParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, SITransactionKey, appStatus, "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return provideAuthInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }


        public static DataSet checkAuthApplicationStatus(Logging log, string providerNumber, string NPI, string TaxID, int serviceOP)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("ProviderNumber", DbType.String, providerNumber, true));
                parameters.Add(SqlParms.CreateParameter("NPI", DbType.String, NPI, true));
                parameters.Add(SqlParms.CreateParameter("TaxID", DbType.String, TaxID, true));
                parameters.Add(SqlParms.CreateParameter("ServiceOP", DbType.Int32, serviceOP, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationStatusForIVR", parameters, "CheckApplicationStatusForIVR");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        // ------------------------------------------------------------------------------------------------

        public static string provGroupParseXMLToStringReq(Logging log, ProvideGroupAffInquiryDataRequest req)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideGroupAffInquiryDataRequest));
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

        public static ProvideGroupAffInquiryDataResponse provideGroupInquiryDataResponseError(Logging log, string errorCode, string errorDescription,
            Guid requestID, ProvideGroupAffInquiryDataRequest request)
        {

            string SITransactionKey = request.GroupAffInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.GroupAffInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.GroupAffInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                GroupAffInquiryResponseGroupAffInquiryError rtnError = new GroupAffInquiryResponseGroupAffInquiryError();
                rtnError.errorCode = errorCode;
                rtnError.errorDescription = errorDescription;

                GroupAffInquiryResponse rt = new GroupAffInquiryResponse();
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Failure";
                rt.groupAffInquiryError = rtnError;

                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
                rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
                rt.ResponseMessage = errorDescription;
                rt.ResponseCode = errorCode;

                ProvideGroupAffInquiryDataResponse rtn = new ProvideGroupAffInquiryDataResponse();
                rtn.GroupAffInquiryResponse = rt;
                responsePayload = provGroupParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, errorCode, errorDescription, "Failure", responsePayload);
                log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return IVRServiceDataHelper.provideGroupInquiryDataResponseGenericError(log, errorCode, errorDescription, request);
            }
        }

        public static string provGroupParseXMLToStringResp(Logging log, ProvideGroupAffInquiryDataResponse res)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(ProvideGroupAffInquiryDataResponse));
                var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var stream2 = new StringWriter();
                var writer = XmlWriter.Create(stream2, settings);
                x.Serialize(writer, res, emptyNs);
                string xml = stream2.ToString();
                return xml;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        public static ProvideGroupAffInquiryDataResponse provideGroupInquiryDataResponseGenericError(Logging log, string errorCode, string errorDescription,
            ProvideGroupAffInquiryDataRequest request)
        {
            string SITransactionKey = request.GroupAffInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.GroupAffInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.GroupAffInquiryRequest.MessageHeader.AdditionalModuleTransactionId;

            GroupAffInquiryResponseGroupAffInquiryError rtnError = new GroupAffInquiryResponseGroupAffInquiryError();
            rtnError.errorCode = errorCode;
            rtnError.errorDescription = errorDescription;

            GroupAffInquiryResponse rt = new GroupAffInquiryResponse();
            rt.SITransactionKey = SITransactionKey;
            rt.responseStatus = "Failure";
            rt.groupAffInquiryError = rtnError;

            rt.ModuleTransactionId = moduleTransactionID;
            rt.AdditionalModuleTransactionId = addnModuleTransactionID;
            rt.responseStatus = Constants.ResponseTypes.PNM_Failure;
            rt.ResponseDetails = Constants.ResponseDetails.PNM_IVR_Default_Error;
            rt.ResponseMessage = errorDescription;
            rt.ResponseCode = errorCode;

            ProvideGroupAffInquiryDataResponse rtn = new ProvideGroupAffInquiryDataResponse();
            rtn.GroupAffInquiryResponse = rt;
            return rtn;
        }

        public static ProvideGroupAffInquiryDataResponse provideGroupInquiryDataResponseSuccessResp(Logging log, string affBeginDate, string appStatus, Guid requestID,
            ProvideGroupAffInquiryDataRequest request)
        {

            string SITransactionKey = request.GroupAffInquiryRequest.MessageHeader.SITransactionKey;
            string moduleTransactionID = request.GroupAffInquiryRequest.MessageHeader.ModuleTransactionId;
            string addnModuleTransactionID = request.GroupAffInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
            try
            {
                string responsePayload = string.Empty;
                DateTime myDate;
                if (!DateTime.TryParse(affBeginDate, out myDate))
                {
                    log.CreateLogEntry("Error while formatting the date", Logging.LogPriority.Error);
                    myDate = DateTime.MinValue;
                }
                GroupAffInquiryResponseGroupAffInquiry rtnInq = new GroupAffInquiryResponseGroupAffInquiry();
                if (myDate != DateTime.MinValue)
                {
                    rtnInq.affBeginDate = myDate.ToString("yyyy-MM-dd");
                }

                rtnInq.affBeginDateSpecified = true;
                rtnInq.affStatus = appStatus;
                GroupAffInquiryResponse rt = new GroupAffInquiryResponse();
                rt.groupAffInquiry = rtnInq;
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Success";

                rt.ModuleTransactionId = moduleTransactionID;
                rt.AdditionalModuleTransactionId = addnModuleTransactionID;
                rt.responseStatus = "Success";
                rt.ResponseType = GroupAffInquiryResponseResponseType.Success;
                rt.ResponseCode = "1000";
                rt.ResponseMessage = "Request Received and Processed Successfully";

                ProvideGroupAffInquiryDataResponse rtn = new ProvideGroupAffInquiryDataResponse();

                rtn.GroupAffInquiryResponse = rt;
                responsePayload = provGroupParseXMLToStringResp(log, rtn);
                saveRequestResponse(log, requestID, SITransactionKey, appStatus, "Success", responsePayload);
                log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return provideGroupInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public static DataSet checkGroupApplicationStatus(Logging log, string groupNumber, string providerNumber, int serviceOP)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("GroupNumber", DbType.String, groupNumber, true));
                parameters.Add(SqlParms.CreateParameter("ProviderNumber", DbType.String, providerNumber, true));
                parameters.Add(SqlParms.CreateParameter("ServiceOP", DbType.Int32, serviceOP, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationStatusForIVR", parameters, "CheckApplicationStatusForIVR");
                return ds;
            }
            catch (Exception ex)
            {
                log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

    }
}