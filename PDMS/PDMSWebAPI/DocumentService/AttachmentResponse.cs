using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{

    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [XmlSerializerFormat]
    public class AttachmentResponse
    {


        string indexid;

        string status;

        string statusCode;

        string statusDescription;
        string responseDetails;

        [DataMember(IsRequired = false, Order = 0)]
        public string IndexId { get => indexid; set => indexid = value; }


        [DataMember(IsRequired = true, Order = 1)]
        [XmlElement(IsNullable = true)]
        public string ResponseCode { get => statusCode; set => statusCode = value; }

        [DataMember(IsRequired = true, Order = 2)]
        [XmlElement(IsNullable = true)]
        public string ResponseType { get => status; set => status = value; }

        [DataMember(IsRequired = true, Order = 3)]
        public string ResponseMessage { get => statusDescription; set => statusDescription = value; }

        [DataMember(Order = 4)]
        public string ResponseDetails { get => responseDetails; set => responseDetails = value; }
    }
}