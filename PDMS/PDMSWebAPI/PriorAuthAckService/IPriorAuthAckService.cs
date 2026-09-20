using System.ServiceModel;

namespace PDMSWebAPI.PriorAuthAckService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPriorAuthAckService" in both code and config file together.
    [ServiceContract(Namespace = "http://service.caremgmt.fi/PriorAuthService", Name = "PriorAuthAcknowledgmentService")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface IPriorAuthAckService
    {
        [OperationContract(Action = "http://service.caremgmt.fi/PriorAuthService/ReceivePriorAuthUpdates")]
        ReceivePriorAuthUpdatesResponse ReceivePriorAuthUpdatesData(ReceivePriorAuthUpdatesRequest request);

    }
}
