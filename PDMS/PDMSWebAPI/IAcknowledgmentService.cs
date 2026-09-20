using PDMSWebAPI.Models;
using System.ServiceModel;

namespace PDMSWebAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPartialAcknowledgement" in both code and config file together.
    [ServiceContract(Namespace= "http://Maximus.OHPNM.Services", Name = "AcknowledgmentService")]
    [XmlSerializerFormat]
    public interface IAcknowledgmentService
    {
        [OperationContract]
        void TargetVendorResponse(TargetVendorResponse acknowledgementModel);

        [OperationContract]
        string EchoSoapRequest(int input);
    }
}
