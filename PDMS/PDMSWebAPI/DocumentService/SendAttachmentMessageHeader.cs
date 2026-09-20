using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class SendAttachmentMessageHeader : InqMessageHeader
    {
        private string sITransactionKeyField;

        [DataMember( Order = 7)]
        [XmlElement( Order = 7)]
        public string SITransactionKey { get => sITransactionKeyField; set => sITransactionKeyField = value; }
    }
}