using MAXIMUS.Core.Libraries;
using PDMSWebAPI.ProviderApplicationStatusInq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.ProviderInquiryServiceData
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProviderAppStatusInqServiceData" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProviderAppStatusInqServiceData.svc or ProviderAppStatusInqServiceData.svc.cs at the Solution Explorer and start debugging.
    public class ProviderAppStatusIVRServiceData : IProviderAppStatusIVRServiceData
    {
        Logging log;
        Guid ProviderApplicationStatusInqServiceID = new Guid(CON.WebApiServiceGuid.IVRService);

        public ProviderAppStatusIVRServiceData()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                log = new Logging(ProviderApplicationStatusInqServiceID, "PDMSWebAPI:ProviderApplicationStatusInqService");
            }
            else
            {
                log = new Logging(user.UserThreadId, "PDMSWebAPI:ProviderApplicationStatusInqService");
            }
        }


        ProvideAppStatusInquiryDataResponse IProviderAppStatusIVRServiceData.ProvideAppStatusInquiryData(ProvideAppStatusInquiryDataRequest request)
        {
            log.CreateLogEntry("Calling Provider Application Status Inquiry Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:ProviderApplicationStatusInqService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsIVRInqMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                AppStatusInquiryRequestMessageHeader amh = new AppStatusInquiryRequestMessageHeader();
                amh.BusinessFlow = request.AppStatusInquiryRequest.MessageHeader.BusinessFlow;
                amh.StateCode = request.AppStatusInquiryRequest.MessageHeader.StateCode;
                amh.RequestorSystem = request.AppStatusInquiryRequest.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.AppStatusInquiryRequest.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.AppStatusInquiryRequest.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.AppStatusInquiryRequest.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.AppStatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.AppStatusInquiryRequest.MessageHeader.SITransactionKey;

                string requestXML = IVRServiceDataHelper.appStatusParseXMLToStringReq(log, request);
                IVRServiceDataHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.StateCode.ToString(), 
                    amh.RequestorSystem.ToString(), amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);

                string regIDstr = request.AppStatusInquiryRequest.appStatusInquiry.atn;
                int regID = 0;

                DateTime dtreqTimeStamp;

                if (!amh.SubscriberSystem.Contains("PNM")) { 
                    return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "3005", "Invalid SubsriberSystem", requestID, request);
                }

                if (!string.IsNullOrEmpty(amh.RequestTimestamp) && !DateTime.TryParse(amh.RequestTimestamp, out dtreqTimeStamp))
                {
                    return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "3006", "Invalid RequestTimeStamp", requestID, request);
                }

                if (!Int32.TryParse(regIDstr, out regID))
                {
                    return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "8001", "Invalid Reg ID", requestID, request);
                }
                DataSet dsReg = null;
                DataRow drReg = null;
                string appStatusCode = string.Empty;
                string appStatusDescp = string.Empty;
                dsReg = IVRServiceDataHelper.checkApplicationStatus(log, regID, 1);
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
                        return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "8001", "Invalid Reg ID", requestID, request);
                    case "8002":
                        return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "8002", "No active enrollment application for this Reg ID", requestID, request);
                    case "8003":
                        return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "8003", "Application cancelled/deleted", requestID, request);
                }
                return IVRServiceDataHelper.successResponse(log, appStatusCode, appStatusDescp, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("App Status Inquiry data "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
                AppStatusInquiryRequestMessageHeader amh = new AppStatusInquiryRequestMessageHeader();
                amh.SITransactionKey = request.AppStatusInquiryRequest.MessageHeader.SITransactionKey;
                return IVRServiceDataHelper.provAppStatusInquiryDataRespError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }
            

        public ProvideStatusInquiryDataResponse ProvideStatusInquiryData(ProvideStatusInquiryDataRequest request)
        {
            log.CreateLogEntry("Calling Provider Status Inquiry Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:ProviderApplicationStatusInqService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsIVRInqMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                StatusInquiryRequestMessageHeader amh = new StatusInquiryRequestMessageHeader();
                amh.BusinessFlow = request.StatusInquiryRequest.MessageHeader.BusinessFlow;
                amh.StateCode = request.StatusInquiryRequest.MessageHeader.StateCode;
                amh.RequestorSystem = request.StatusInquiryRequest.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.StatusInquiryRequest.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.StatusInquiryRequest.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.StatusInquiryRequest.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.StatusInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.StatusInquiryRequest.MessageHeader.SITransactionKey;

                string requestXML = IVRServiceDataHelper.provStatusParseXMLToStringReq(log, request);
                IVRServiceDataHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.StateCode.ToString(),
                    amh.RequestorSystem.ToString(), amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);

                string providerID = request.StatusInquiryRequest.statusInquiry.providerId;
                string npi = request.StatusInquiryRequest.statusInquiry.npi;

                DateTime dtreqTimeStamp;
                if (!amh.SubscriberSystem.Contains("PNM"))
                {
                    return IVRServiceDataHelper.provideStatusInquiryDataResponseError(log, "3005", "Invalid SubsriberSystem", requestID, request);
                }

                if (!string.IsNullOrEmpty(amh.RequestTimestamp) && !DateTime.TryParse(amh.RequestTimestamp, out dtreqTimeStamp))
                {
                    return IVRServiceDataHelper.provideStatusInquiryDataResponseError(log, "3006", "Invalid RequestTimeStamp", requestID, request);
                }

                DataSet dsReg = null;
                DataRow drReg = null;
                string appStatusCode = string.Empty;
                string appStatusDescp = string.Empty;
                dsReg = IVRServiceDataHelper.checkStatusApplicationStatus(log, providerID, npi, 2);
                drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    appStatusCode = Methods.GetString("appStatus", drReg);
                }
                switch (appStatusCode)
                {
                    case "":
                    case "8004":
                        return IVRServiceDataHelper.provideStatusInquiryDataResponseError(log, "8004", "No record found", requestID, request);
                    case "8003":
                        return IVRServiceDataHelper.provideStatusInquiryDataResponseError(log, "8003", "Application cancelled/deleted", requestID, request);
                }
                return IVRServiceDataHelper.provideStatusInquiryDataResponseSuccessResp(log, appStatusCode, appStatusDescp, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("App Status Inquiry data "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
                AppStatusInquiryRequestMessageHeader amh = new AppStatusInquiryRequestMessageHeader();
                amh.SITransactionKey = request.StatusInquiryRequest.MessageHeader.SITransactionKey;
                return IVRServiceDataHelper.provideStatusInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        

        public ProvideAuthInquiryDataResponse ProvideAuthInquiryData(ProvideAuthInquiryDataRequest request)
        {
            log.CreateLogEntry("Calling Provider Auth Inquiry Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:ProviderApplicationStatusInqService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsIVRInqMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                AuthInquiryRequestMessageHeader amh = new AuthInquiryRequestMessageHeader();
                amh.BusinessFlow = request.AuthInquiryRequest.MessageHeader.BusinessFlow;
                amh.StateCode = request.AuthInquiryRequest.MessageHeader.StateCode;
                amh.RequestorSystem = request.AuthInquiryRequest.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.AuthInquiryRequest.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.AuthInquiryRequest.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.AuthInquiryRequest.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.AuthInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.AuthInquiryRequest.MessageHeader.SITransactionKey;

                string requestXML = IVRServiceDataHelper.provStatusParseXMLToStringReq(log, request);
                IVRServiceDataHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.StateCode.ToString(),
                    amh.RequestorSystem.ToString(), amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);

                string providerID = request.AuthInquiryRequest.authInquiry.providerId;
                string NPI = request.AuthInquiryRequest.authInquiry.npi;
                string taxID = request.AuthInquiryRequest.authInquiry.taxId;

                DateTime dtreqTimeStamp;
                if (!amh.SubscriberSystem.Contains("PNM"))
                {
                    return IVRServiceDataHelper.provideAuthInquiryDataResponseError(log, "3005", "Invalid SubsriberSystem", requestID, request);
                }

                if (!string.IsNullOrEmpty(amh.RequestTimestamp) && !DateTime.TryParse(amh.RequestTimestamp, out dtreqTimeStamp))
                {
                    return IVRServiceDataHelper.provideAuthInquiryDataResponseError(log, "3006", "Invalid RequestTimeStamp", requestID, request);
                }

                DataSet dsReg = null;
                DataRow drReg = null;
                string appStatusCode = string.Empty;
                string appStatusDescp = string.Empty;
                dsReg = IVRServiceDataHelper.checkAuthApplicationStatus(log, providerID, NPI, taxID, 3);
                drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    appStatusCode = Methods.GetString("appStatus", drReg);
                }
                switch (appStatusCode)
                {
                    case "":
                    case "8004":
                        return IVRServiceDataHelper.provideAuthInquiryDataResponseError(log, "8004", "No record found", requestID, request);
                    case "8003":
                        return IVRServiceDataHelper.provideAuthInquiryDataResponseError(log, "8003", "Application cancelled/deleted", requestID, request);
                }
                return IVRServiceDataHelper.provideAuthInquiryDataResponseSuccessResp(log, appStatusCode, appStatusDescp, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("App Status Inquiry data "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
                AppStatusInquiryRequestMessageHeader amh = new AppStatusInquiryRequestMessageHeader();
                amh.SITransactionKey = request.AuthInquiryRequest.MessageHeader.SITransactionKey;
                return IVRServiceDataHelper.provideAuthInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        public ProvideGroupAffInquiryDataResponse ProvideGroupAffInquiryData(ProvideGroupAffInquiryDataRequest request)
        {
            log.CreateLogEntry("Calling Provider Group Status Inquiry Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:ProviderApplicationStatusInqService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsIVRInqMaintenance", bool.FalseString));
            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                GroupAffInquiryRequestMessageHeader amh = new GroupAffInquiryRequestMessageHeader();
                amh.BusinessFlow = request.GroupAffInquiryRequest.MessageHeader.BusinessFlow;
                amh.StateCode = request.GroupAffInquiryRequest.MessageHeader.StateCode;
                amh.RequestorSystem = request.GroupAffInquiryRequest.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.GroupAffInquiryRequest.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.GroupAffInquiryRequest.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.GroupAffInquiryRequest.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.GroupAffInquiryRequest.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.GroupAffInquiryRequest.MessageHeader.SITransactionKey;

                string requestXML = IVRServiceDataHelper.provGroupParseXMLToStringReq(log, request);
                IVRServiceDataHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.StateCode.ToString(),
                    amh.RequestorSystem.ToString(), amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);

                string groupNumber = request.GroupAffInquiryRequest.groupAffInquiry.groupNumber;
                string providerNumber = request.GroupAffInquiryRequest.groupAffInquiry.providerNumber;

                DateTime dtreqTimeStamp;

                if (!amh.SubscriberSystem.Contains("PNM"))
                {
                    return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "3005", "Invalid SubsriberSystem", requestID, request);
                }

                if (!string.IsNullOrEmpty(amh.RequestTimestamp) && !DateTime.TryParse(amh.RequestTimestamp, out dtreqTimeStamp))
                {
                    return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "3006", "Invalid RequestTimeStamp", requestID, request);
                }

                if (string.IsNullOrEmpty(groupNumber) || string.IsNullOrWhiteSpace(groupNumber))
                {
                    return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "8008", "Invalid Group Number", requestID, request);
                }

                if (string.IsNullOrEmpty(providerNumber) || string.IsNullOrWhiteSpace(providerNumber))
                {
                    return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "8009", "Invalid Provider Number", requestID, request);
                }
                DataSet dsReg = null;
                DataRow drReg = null;
                string affBeginDate = string.Empty;
                string affStatus = string.Empty;
                dsReg = IVRServiceDataHelper.checkGroupApplicationStatus(log, groupNumber, providerNumber, 4);
                drReg = Methods.HasRows(dsReg) ? dsReg.Tables[0].Rows[0] : null;
                if (drReg != null)
                {
                    affBeginDate = Methods.GetString("affBeginDate", drReg);
                    affStatus = Methods.GetString("affStatus", drReg);
                }
                switch (affStatus)
                {
                    case "":
                    case "8008":
                        return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "8008", "Invalid Group Number", requestID, request);
                    case "8011":
                        return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "8011", "Application cancelled/deleted for this Group Number", requestID, request);
                }
                return IVRServiceDataHelper.provideGroupInquiryDataResponseSuccessResp(log, affBeginDate, affStatus, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Group Status Inquiry data "
                                                 + " Exception Message " + ex.Message + " Exception Stack = "
                                                 + ex.StackTrace, Logging.LogPriority.Error);
                AppStatusInquiryRequestMessageHeader amh = new AppStatusInquiryRequestMessageHeader();
                amh.SITransactionKey = request.GroupAffInquiryRequest.MessageHeader.SITransactionKey;
                return IVRServiceDataHelper.provideGroupInquiryDataResponseError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

        
    }
}
