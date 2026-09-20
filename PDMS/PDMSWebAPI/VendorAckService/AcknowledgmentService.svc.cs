using MAXIMUS.Core.Libraries;
using System;
using System.Net;
using System.ServiceModel.Web;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace PDMSWebAPI.VendorAckService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PartialAcknowledgement" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PartialAcknowledgement.svc or PartialAcknowledgement.svc.cs at the Solution Explorer and start debugging.
    public class AcknowledgmentService : IAcknowledgmentService
    {
        Logging log;
        Guid VendorAckServiceId = new Guid(CON.WebApiServiceGuid.VendorAckService);

        public AcknowledgmentService()
        {
            var user = PDMSPrincipal.GetCurrentUser();
            if (user == null)
            {
                log = new Logging(VendorAckServiceId, "PDMSWebAPI:VendorAckService");
            }
            else
            {
                log = new Logging(user.UserThreadId, "PDMSWebAPI:VendorAckService");
            }
        }

        public VendorAcknowledgmentResponseReference vendorAcknowledgment(VendorAcknowledgmentRequestReference request)
        {
            log.CreateLogEntry("Calling Vendor Acknowledgment Service... ", Logging.LogPriority.Information);
            RolesAuthentication.Allow("WebAPI:AcknowledgmentService");

            Guid requestID = Guid.NewGuid();

            try
            {

                MessageHeaderVendorAckRequest amh = new MessageHeaderVendorAckRequest();
                amh.BusinessFlow = request.messageHeaderVendorAckRequest.BusinessFlow;
                amh.RequestorSystem = request.messageHeaderVendorAckRequest.RequestorSystem;
                amh.RequestorSystemId = request.messageHeaderVendorAckRequest.RequestorSystemId;
                amh.RequestorTransactionId = request.messageHeaderVendorAckRequest.RequestorTransactionId;
                amh.TargetSystem = request.messageHeaderVendorAckRequest.TargetSystem;
                amh.TargetTransactionId = request.messageHeaderVendorAckRequest.TargetTransactionId;
                amh.ModuleTransactionId = request.messageHeaderVendorAckRequest.ModuleTransactionId;
                amh.AdditionalModuleTransactionId = request.messageHeaderVendorAckRequest.AdditionalModuleTransactionId;
                amh.SITransactionKey = request.messageHeaderVendorAckRequest.SITransactionKey;

                string requestXML = VendorAckServiceHelper.vendorAckParseXMLToStringReq(log, request);
                VendorAckServiceHelper.saveRequestResponse(log, requestID, amh.BusinessFlow.ToString(), amh.RequestorSystem.ToString(),
                    amh.ModuleTransactionId.ToString(), amh.AdditionalModuleTransactionId, amh.SITransactionKey, requestXML);


                //si key is not null
                if (!VendorAckServiceHelper.checkifSIKeyExists(log, amh.SITransactionKey))
                {
                    return VendorAckServiceHelper.vendorAcknowledgementDataRespError(log, "300", "SITransactionKey doesn't exist in the system", requestID, request);
                }

                if (request.vendorAckSubResponse == null || request.vendorAckSubResponse.Length == 0)
                {
                    return VendorAckServiceHelper.vendorAcknowledgementDataRespError(log, "301", "Response node is missng the required elements", requestID, request);
                }

                VendorAckServiceHelper.processVendorAcknowledgment(log, amh.SITransactionKey, request);

                return VendorAckServiceHelper.successResponse(log, requestID, request);
            }
            catch (Exception ex)
            {
                log.CreateLogEntry("Vendor Acknowledgment Service Exception Message " + ex.Message + " Exception Stack = " + ex.StackTrace, Logging.LogPriority.Error);
                return VendorAckServiceHelper.vendorAcknowledgementDataRespError(log, "5000", "Error while Processing the request", requestID, request);
            }
        }

    }
}
