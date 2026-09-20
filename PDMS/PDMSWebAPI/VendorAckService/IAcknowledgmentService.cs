using System.ServiceModel;

namespace PDMSWebAPI.VendorAckService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAcknowledgmentService" in both code and config file together.
    [ServiceContract(Namespace = "http://mes.gov/acknowledgment", Name = "VendorAcknowledgmentResponse")]
    [XmlSerializerFormat]
    public interface IAcknowledgmentService
    {

        [OperationContract(Action = "http://mes.gov/vendorAcknowledgmentReqRes")]
        VendorAcknowledgmentResponseReference vendorAcknowledgment(VendorAcknowledgmentRequestReference vendorAcknowledgmentRequest);

    }
}
