using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.DocumentService
{
    [DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    [KnownType(typeof(SendAttachmentMessageHeader))]
    [XmlSerializerFormat]
    public class InqMessageHeader
    {

        private string businessFlowField;

        private string stateCodeField;

        private string moduleTransactionIdField;

        private string additionalModuleTransactionIdField;

        private string requestorSystemField;

        private string[] subscriberSystemField;

        private string requestTimestampField;


        [DataMember(IsRequired = true, Order = 0)]
        [XmlElement(IsNullable = true, Order = 0)]
        public string BusinessFlow { get => businessFlowField; set => businessFlowField = value; }

        [DataMember(IsRequired = true, Order = 1)]
        [XmlElement(IsNullable = true, Order = 1)]
        public string StateCode { get => stateCodeField; set => stateCodeField = value; }

        [DataMember(IsRequired = true, Order = 4)]
        [XmlElement(IsNullable = true, Order = 4)]
        public string RequestorSystem { get => requestorSystemField; set => requestorSystemField = value; }


        [DataMember(Order = 2)]
        [XmlElement(Order = 2)]
        public string ModuleTransactionId { get => moduleTransactionIdField; set => moduleTransactionIdField = value; }
        [DataMember(Order = 3)]
        [XmlElement(Order = 3)]
        public string AdditionalModuleTransactionId { get => additionalModuleTransactionIdField; set => additionalModuleTransactionIdField = value; }

        [DataMember(Order = 5)]
        [System.Xml.Serialization.XmlArrayAttribute(Order = 5)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Subscriber", IsNullable = false)]
        public string[] SubscriberSystem { get => subscriberSystemField; set => subscriberSystemField = value; }

        [DataMember(IsRequired = true, Order = 6)]
        [XmlElement(IsNullable = true, Order = 6)]
        public string RequestTimestamp { get => requestTimestampField; set => requestTimestampField = value; }
    }
}