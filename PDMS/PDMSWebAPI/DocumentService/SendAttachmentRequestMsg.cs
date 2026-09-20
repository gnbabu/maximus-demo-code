using System;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{



    [Serializable]
    [MessageContract(WrapperNamespace = "http://Maximus.OHPNM.Services", IsWrapped = true,WrapperName = "SendAttachment")]
    [XmlSerializerFormat]
    public class SendAttachmentRequestMsg
    {


        [MessageBodyMember(Order=0,Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public SendAttachmentMessageHeader MessageHeader { get; set; }



        [MessageBodyMember(Order = 1, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public SendAttachmentPayLoad Payload;

        public SendAttachmentRequestMsg()
        {
        }

        public SendAttachmentRequestMsg(SendAttachmentMessageHeader MessageHeader,SendAttachmentPayLoad Payload)
        {
            this.MessageHeader = MessageHeader;
            this.Payload = Payload;
        }
    }
}