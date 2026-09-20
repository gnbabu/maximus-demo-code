using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class SendAttachment
    {

        private SendAttachmentData[] identifiers;


        string documentType;


        string documentName;


        string documentExtension;
        byte[] docContents;


        [DataMember(IsRequired = true, Order = 0, Name = "Identifiers", EmitDefaultValue = false)]
        [XmlElement("Identifiers", IsNullable = true, Order = 0)]
        [MinLength(1)]
        public SendAttachmentData[] Identifiers { get; set; }
      

        [DataMember(IsRequired = true, Order = 1)]
        [XmlElement(IsNullable = true, Order = 1)]
        public string DocumentType { get => documentType; set => documentType = value; }

        [DataMember(IsRequired = true, Order = 2)]
        [XmlElement(IsNullable = true, Order = 2)]
        public string DocumentName { get => documentName; set => documentName = value; }

        [DataMember(IsRequired = true, Order = 3)]
        [XmlElement(IsNullable = true, Order = 3)]
        public string DocumentExtension { get => documentExtension; set => documentExtension = value; }


        [DataMember(IsRequired = true, Order = 4)]
        [XmlElement(IsNullable = true, Order = 4)]
        public byte[] AttachmentData64Binary { get => docContents; set => docContents = value; }

        public SendAttachment() { }
        public SendAttachment(string documentType, string documentName, string documentExtension, byte[] attachmentData64Binary, SendAttachmentData[] identifiers)
        {
            this.DocumentType = documentType;
            this.DocumentName = documentName;
            this.DocumentExtension = documentExtension;
            this.AttachmentData64Binary = attachmentData64Binary;
            this.Identifiers = identifiers;
        }
    }
}