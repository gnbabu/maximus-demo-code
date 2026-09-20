using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class SendAttachmentData
    {
        string _docXrefType = string.Empty;
        string _indexId = string.Empty;

        [DataMember(IsRequired = true, Order = 0)]
        [XmlElement(IsNullable = true, Order = 0)]
        public string DocXrefType { get => _docXrefType; set => _docXrefType = value; }

        [DataMember(IsRequired = true, Order = 1)]
        [XmlElement(IsNullable = true, Order = 1)]
        public string IndexId { get => _indexId; set => _indexId = value; }

        public SendAttachmentData()
        { }

        public SendAttachmentData(string docXrefType, string indexId)
        {
            this.IndexId = indexId;
            this.DocXrefType = docXrefType;
        }
    }
}