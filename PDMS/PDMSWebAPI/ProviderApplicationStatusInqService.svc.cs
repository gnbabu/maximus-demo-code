using MAXIMUS.Core.Libraries;
using PDMSWebAPI.ProviderApplicationStatusInq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace PDMSWebAPI.ProviderInquiryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProviderApplicationStatusInqService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProviderApplicationStatusInqService.svc or ProviderApplicationStatusInqService.svc.cs at the Solution Explorer and start debugging.
    public class ProviderApplicationStatusInqService : IProviderApplicationStatusInqService
    {

        Logging Log;
        Guid ProviderApplicationStatusInqServiceID = new Guid("024725F9-C6FE-4CFA-A7C7-A386A49E183D");

        public ProviderApplicationStatusInqService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                Log = new Logging(ProviderApplicationStatusInqServiceID, "PDMSWebAPI:ProviderApplicationStatusInqService");
            }
            else
            {
                Log = new Logging(user.UserThreadId, "PDMSWebAPI:ProviderApplicationStatusInqService");
            }
        }


        ProvideAppStatusInquiryDataResponse IProviderApplicationStatusInqService.ProvideAppStatusInquiryData(ProvideAppStatusInquiryDataRequest request)
        {
            Log.CreateLogEntry("Calling Provider Application Status Inquiry Service... ", Logging.LogPriority.Information);

            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsIVRInqMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();
            
            try
            {                
                string BusinessFlow = request.MessageHeader.BusinessFlow;
                string StateCode = request.MessageHeader.StateCode;
                string requestorSystem = request.MessageHeader.RequestorSystem;
                string subsriberSystem = request.MessageHeader.SubscriberSystem;
                string reqTimeStamp = request.MessageHeader.RequestTimestamp;
                string moduleTransactionID = request.MessageHeader.ModuleTransactionId;
                string AdditionalModuleTransactionID = request.MessageHeader.AdditionalModuleTransactionId;
                string SITransactionKey = request.MessageHeader.SITransactionKey;

                string requestXML = parseXMLToStringRequest(request);
                saveRequestResponse(requestID, BusinessFlow, StateCode, requestorSystem, subsriberSystem, moduleTransactionID, AdditionalModuleTransactionID, SITransactionKey, requestXML);

                string regIDstr = request.appStatusInquiry.atn;
                int regID = 0;
                
                DateTime dtreqTimeStamp;

                if (!BusinessFlow.Equals("AppStatusInquiryRequest"))
                {
                    return errorResponse(SITransactionKey,"3002", "Invalid BusinessFlow", requestID);
                }

                if (!StateCode.Equals("OH"))
                {
                    return errorResponse(SITransactionKey, "3002", "Invalid StateCode", requestID);
                }

                if (string.IsNullOrEmpty(requestorSystem))
                {
                    return errorResponse(SITransactionKey, "3002", "Invalid requestorSystem", requestID);
                }

                if (string.IsNullOrEmpty(subsriberSystem))
                {
                    return errorResponse(SITransactionKey, "3002", "Invalid subsriberSystem", requestID);
                }

                if (!string.IsNullOrEmpty(reqTimeStamp) && !DateTime.TryParse(reqTimeStamp, out dtreqTimeStamp))
                {
                    return errorResponse(SITransactionKey, "3002", "Invalid reqTimeStamp", requestID);
                }

                if (!Int32.TryParse(regIDstr, out regID))
                {
                    return errorResponse(SITransactionKey, "8001", "Invalid Reg ID", requestID);
                }
                DataSet dsReg = null;
                DataRow drReg = null;
                string appStatusCode = string.Empty;
                string appStatusDescp = string.Empty;
                dsReg = checkApplicationStatus(regID);
                drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;                
                if (drReg != null)
                {
                    appStatusCode = Methods.GetString("appStatusCode", drReg);
                    appStatusDescp = Methods.GetString("appStatusDescp", drReg);
                }
                switch (appStatusCode)
                {
                    case "":
                    case "8001":
                        return errorResponse(SITransactionKey, "8001", "Invalid Reg ID", requestID);
                    case "8002":
                        return errorResponse(SITransactionKey, "8002", "No active enrollment application for this Reg ID", requestID);
                    case "8003":
                        return errorResponse(SITransactionKey, "8003", "Application cancelled/deleted", requestID);
                }
                return successResponse(SITransactionKey, appStatusCode, appStatusDescp, requestID);                
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry("App Status Inquiry data "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);

                return errorResponse(request.MessageHeader.SITransactionKey, "500", "Error while Processing the request", requestID);
            }
        }

        private ProvideAppStatusInquiryDataResponse errorResponse(string SITransactionKey, string errorCode, string errorDescription, Guid requestID)
        {
            try
            {
                string responsePayload = string.Empty;
                AppStatusInquiryResponseAppStatusInquiryError rtnError = new AppStatusInquiryResponseAppStatusInquiryError();
                rtnError.errorCode = errorCode;
                rtnError.errorDescription = errorDescription;

                AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Failure";
                rt.appStatusInquiryError = rtnError;
                ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();
                rtn.AppStatusInquiryResponse = rt;
                responsePayload = parseXMLToStringResponse(rtn);
                saveRequestResponse(requestID, errorCode, errorDescription, "Failure", responsePayload);
                Log.CreateLogEntry("Request Ended with a Failure... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return genericErrorResponse(SITransactionKey, errorCode, errorDescription);
            }
        }

        private ProvideAppStatusInquiryDataResponse genericErrorResponse(string SITransactionKey, string errorCode, string errorDescription)
        {
            AppStatusInquiryResponseAppStatusInquiryError rtnError = new AppStatusInquiryResponseAppStatusInquiryError();
            rtnError.errorCode = errorCode;
            rtnError.errorDescription = errorDescription;

            AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
            rt.SITransactionKey = SITransactionKey;
            rt.responseStatus = "Failure";
            rt.appStatusInquiryError = rtnError;
            ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();
            rtn.AppStatusInquiryResponse = rt;
            return rtn;
        }


        private ProvideAppStatusInquiryDataResponse successResponse(string SITransactionKey, string appStatus, string appStatusDescp, Guid requestID)
        {
            try
            {
                string responsePayload = string.Empty;
                AppStatusInquiryResponseAppStatusInquiry rtnInq = new AppStatusInquiryResponseAppStatusInquiry();
                rtnInq.appStatusCode = appStatus;
                rtnInq.appStatusDescription = appStatusDescp;
                AppStatusInquiryResponse rt = new AppStatusInquiryResponse();
                rt.appStatusInquiry = rtnInq;
                rt.SITransactionKey = SITransactionKey;
                rt.responseStatus = "Success";
                ProvideAppStatusInquiryDataResponse rtn = new ProvideAppStatusInquiryDataResponse();

                rtn.AppStatusInquiryResponse = rt;
                responsePayload = parseXMLToStringResponse(rtn);
                saveRequestResponse(requestID, SITransactionKey, appStatus, "Success", responsePayload);
                Log.CreateLogEntry("Request Ended with a Success... ", Logging.LogPriority.Information);
                return rtn;
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                return errorResponse(SITransactionKey, "5000", "Error while Processing the request", requestID);
            }
        }

        private DataSet checkApplicationStatus(int regID)
        {
            try
            {
                DataSet ds = new DataSet();
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("RegId", DbType.Int32, regID, true));
                ds = DataAccess.ExecuteStoredProcedure("usp_CheckApplicationStatusByRegId", parameters, "CheckApplicationStatusByRegId");
                return ds;
            }
            catch (Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        private void saveRequestResponse(Guid requestID, string param1, string param2, string responseStatus, string responsePayload)
        {
            try {
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
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid("5D0689A8-D885-4211-B9FD-56757474AB4D"), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_IVR_APPSTATUS_REQ_RES", parameters);
            }catch(Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        private void saveRequestResponse(Guid requestID, string BusinessFlow, string StateCode, string requestorSystem, string subsriberSystem, string moduleTransactionID, string AdditionalModuleTransactionID, string SITransactionKey, string requestPayload)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("REQUEST_ID", DbType.Guid, requestID, true));
                parameters.Add(SqlParms.CreateParameter("BUSINESS_FLOW", DbType.String, BusinessFlow, true));
                parameters.Add(SqlParms.CreateParameter("STATE_CODE", DbType.String, StateCode, true));
                parameters.Add(SqlParms.CreateParameter("REQUESTOR_SYSTEM", DbType.String, requestorSystem, true));
                parameters.Add(SqlParms.CreateParameter("SUBSCRIBER_SYSTEM", DbType.String, subsriberSystem, true));
                parameters.Add(SqlParms.CreateParameter("TRANSACTION_ID", DbType.String, moduleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("ADDITIONAL_MODULE_TRANSACTION_ID", DbType.String, AdditionalModuleTransactionID, true));
                parameters.Add(SqlParms.CreateParameter("REQUEST_PAYLOAD", DbType.String, requestPayload, true));
                parameters.Add(SqlParms.CreateParameter("SI_TRANSACTION_KEY", DbType.String, SITransactionKey, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_DATE_TIME", DbType.DateTime, DateTime.Now, true));
                parameters.Add(SqlParms.CreateParameter("LAST_MODIFIED_USER", DbType.Guid, new Guid("5D0689A8-D885-4211-B9FD-56757474AB4D"), true));
                DataAccess.ExecuteStoredProcedure("usp_InsertUpdateINBOUND_IVR_APPSTATUS_REQ_RES", parameters);
            }catch(Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
}

        private string parseXMLToStringRequest(ProvideAppStatusInquiryDataRequest req)
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
            }catch(Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

        private string parseXMLToStringResponse(ProvideAppStatusInquiryDataResponse res)
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
            }catch(Exception ex)
            {
                Log.CreateLogEntry(string.Format("{0} error while processing the request, {1}", ex.StackTrace, ex.Message), Logging.LogPriority.Error);
                throw;
            }
        }

    }

}
