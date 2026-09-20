using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{

    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class SendAttachmentInformation
    {

        string SourceIdField;

        SendAttachment[] SendAttachmentField;

        [DataMember(IsRequired = true, Order = 0)]
        [XmlElement(IsNullable = true, Order = 0)]
        public string SourceId { get => SourceIdField; set => SourceIdField = value; }

        [XmlElement("AttachmentData", IsNullable = true, Order = 1)]
        [DataMember(IsRequired = true, Order = 1, EmitDefaultValue = false)]
        public SendAttachment[] AttachmentData { get => SendAttachmentField; set => SendAttachmentField = value; }
    }
}