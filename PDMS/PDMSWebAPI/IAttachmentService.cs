using PDMSWebAPI.DocumentService;
using System.ServiceModel;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReceiveDocumentsService" in both code and config file together.
    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services",Name = "AttachmentService")]
    [XmlSerializerFormat]
    public interface IAttachmentService
    {
       
        [OperationContract(Action= "SendAttachment")]
        [FaultContract(typeof(SendAttachmentFault))]
        SendAttachmentResponseMsg SendAttachment(SendAttachmentRequestMsg request);

    }
}
