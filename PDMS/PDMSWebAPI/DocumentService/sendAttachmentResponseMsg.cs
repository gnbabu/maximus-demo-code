using System.ServiceModel;

namespace PDMSWebAPI.DocumentService
{
    [MessageContract(WrapperNamespace = "http://Maximus.OHPNM.Services", IsWrapped = true, WrapperName = "SendAttachmentResponse")]
    [XmlSerializerFormat]
    public class SendAttachmentResponseMsg
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://Maximus.OHPNM.Services", Order = 0)]
        public string SITransactionKey;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://Maximus.OHPNM.Services", Order = 1)]
        public string ModuleTransactionId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://Maximus.OHPNM.Services", Order = 2)]
        [System.Xml.Serialization.XmlElement("Response")]
        public AttachmentResponse[] Response;

        public SendAttachmentResponseMsg()
        {
        }

        public SendAttachmentResponseMsg(string SITransactionKey, string ModuleTransactionId, AttachmentResponse[] Response)
        {
            this.SITransactionKey = SITransactionKey;
            this.ModuleTransactionId = ModuleTransactionId;
            this.Response = Response;
        }
    }
}