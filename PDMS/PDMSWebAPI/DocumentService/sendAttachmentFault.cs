using System.Runtime.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    public class SendAttachmentFault
    {

        string errorCode = string.Empty;
        string errorMessage = string.Empty;
        [DataMember]
        public string ErrorCode { get => errorCode; set => errorCode = value; }
        [DataMember]
        public string ErrorMessage { get => errorMessage; set => errorMessage = value; }
    }
}