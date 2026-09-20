using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.ClaimsAckService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ClaimsAckService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ClaimsAckService.svc or ClaimsAckService.svc.cs at the Solution Explorer and start debugging.
    public class ClaimsAckService : IClaimsAckService
    {

        Logging log;
        Guid ClaimsAckServiceID = new Guid(CON.WebApiServiceGuid.ClaimsService);

        public ClaimsAckService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                log = new Logging(ClaimsAckServiceID, "PDMSWebAPI:ClaimsAckService");
            }
            else
            {
                log = new Logging(user.UserThreadId, "PDMSWebAPI:ClaimsAckService");
            }
        }


        public ClaimsAckServiceReference.claimAcknowledgementResponse ClaimAcknowledgementData(ClaimsAckServiceReference.claimAcknowledgementRequest request)
        {
            log.CreateLogEntry("Calling Claims Acknowledgment Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:ClaimsAckService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsClaimsAckServiceMaintenance", bool.FalseString));

            bool wsTemplateProcessing = Convert.ToBoolean(AppSettings.Get("wsClaimsAckTemplateProcessing", bool.FalseString));

            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                ClaimsAckServiceReference.MessageHeader amh = new ClaimsAckServiceReference.MessageHeader();
                amh.BusinessFlow = request.MessageHeader.BusinessFlow;
                amh.StateCode = request.MessageHeader.StateCode;
                amh.RequestorSystem = request.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.MessageHeader.SITransactionKey;

                string requestXML = ClaimsAckServiceHelper.claimsAckParseXMLToStringReq(log, request);
                ClaimsAckServiceHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.StateCode.ToString(),
                    amh.RequestorSystem.ToString(), amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);

                if (wsTemplateProcessing)
                {
                    ClaimsAckServiceHelper.processTemplateBody(log, requestID, request);
                }

                
                return ClaimsAckServiceHelper.successResponse(log, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Claims Acknowledgment Service Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                return ClaimsAckServiceHelper.claimAcknowledgementDataRespError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }
    }
}
