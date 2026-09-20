using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using CON = MAXIMUS.Core.Libraries.Constants;
using System.Text;

namespace PDMSWebAPI.PriorAuthAckService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PriorAuthAckService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PriorAuthAckService.svc or PriorAuthAckService.svc.cs at the Solution Explorer and start debugging.
    public class PriorAuthAckService : IPriorAuthAckService
    {

        Logging log;
        Guid ClaimsAckServiceID = new Guid(CON.WebApiServiceGuid.PriorAuthService);

        public PriorAuthAckService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                log = new Logging(ClaimsAckServiceID, "PDMSWebAPI:PriorAuthAckService");
            }
            else
            {
                log = new Logging(user.UserThreadId, "PDMSWebAPI:PriorAuthAckService");
            }
        }

        public ReceivePriorAuthUpdatesResponse ReceivePriorAuthUpdatesData(ReceivePriorAuthUpdatesRequest request)
        {
            log.CreateLogEntry("Calling PriorAuth Acknowledgment Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:PriorAuthAckService");
            bool wsMaintenance = Convert.ToBoolean(AppSettings.Get("wsPriorAuthAckServiceMaintenance", bool.FalseString));

            bool wsTemplateProcessing = Convert.ToBoolean(AppSettings.Get("wsPriorAuthAckTemplateProcessing", bool.FalseString));


            if (wsMaintenance)
            {
                throw new WebFaultException<string>("Service down for Maintenance", HttpStatusCode.InternalServerError);
            }
            Guid requestID = Guid.NewGuid();

            try
            {
                MessageHeaderType amh = new MessageHeaderType();
                amh.BusinessFlow = request.MessageHeader.BusinessFlow;
                amh.StateCode = request.MessageHeader.StateCode;
                amh.RequestorSystem = request.MessageHeader.RequestorSystem;
                amh.SubscriberSystem = request.MessageHeader.SubscriberSystem;
                amh.RequestTimestamp = request.MessageHeader.RequestTimestamp;
                amh.ModuleTransactionId = request.MessageHeader.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.MessageHeader.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.MessageHeader.SITransactionKey;

                string requestXML = PriorAuthAckServiceHelper.priorAuthAckParseXMLToStringReq(log, request);
                PriorAuthAckServiceHelper.saveRequestResponse(log, requestID, CON.PriorAuthAckService.AddUpdatePriorAuth,
                    amh.BusinessFlow.ToString(), amh.StateCode.ToString(), amh.RequestorSystem.ToString(), 
                    amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);


                if (wsTemplateProcessing)
                {
                    PriorAuthAckServiceHelper.processTemplateBody(log, requestID, request);
                }
                
                return PriorAuthAckServiceHelper.addUpdateSuccessResponse(log, requestID, CON.PriorAuthAckService.AddUpdatePriorAuth, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Claims Acknowledgment Service Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                return PriorAuthAckServiceHelper.priorAuthAUDataRespError(log, CON.PriorAuthAckService.AddUpdatePriorAuth, "5000", "Error while Processing the request", requestID, request);
            }
        }
    }
}
