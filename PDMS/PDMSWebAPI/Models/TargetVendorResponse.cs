using System;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.Models
{

    [MessageContract(WrapperNamespace = "http://Maximus.OHPNM.Services", IsWrapped = true, WrapperName = "TargetVendorResponse")]
    [XmlSerializerFormat]
    public class TargetVendorResponse
    {

        [MessageBodyMember(Order =0, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string RequestorSystem { get; set; }

        [MessageBodyMember(Order =1, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string RequestorSystemId { get; set; }

        [MessageBodyMember(Order =2, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string TargetSystem { get; set; }

        [MessageBodyMember(Order =3, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string TargetSystemId { get; set; }

        [MessageBodyMember(Order = 4, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string SITransactionKey { get; set; }

        [MessageBodyMember(Order =5, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string ResponseCode { get; set; }

        [MessageBodyMember(Order =6, Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string ResponseType { get; set; }

        [MessageBodyMember(Order = 7,Namespace = "http://Maximus.OHPNM.Services"), XmlElement(IsNullable = true)]
        public string ResponseMessage { get; set; }

        [MessageBodyMember(Order = 8, Namespace = "http://Maximus.OHPNM.Services")]
        public DateTime TimeStamp { get; set; }

        [MessageBodyMember(Order = 9, Namespace = "http://Maximus.OHPNM.Services")]
        public string ModuleTransactionId { get; set; }

        [MessageBodyMember(Order =10, Namespace = "http://Maximus.OHPNM.Services")]
        public string AdditionalModuleTransactionId { get; set; }

       
    }
    
}