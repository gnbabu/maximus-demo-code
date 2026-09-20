using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class SendAttachmentPayLoad
    {
        private PDMSWebAPI.DocumentService.SendAttachmentInformation attachmentInfoField;

        [DataMember(IsRequired = true, Order = 0)]
        [XmlElement(IsNullable = true, Order = 0)]
        public SendAttachmentInformation AttachmentInfo { get => attachmentInfoField; set => attachmentInfoField = value; }
    }
}