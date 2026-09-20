using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.VendorAckService
{

    [MessageContract(WrapperNamespace = "http://mes.gov/acknowledgment", IsWrapped = true, WrapperName = "VendorAcknowledgmentRequest")]
    [XmlSerializerFormat]
    public class VendorAcknowledgmentRequestReference
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://mes.gov/acknowledgment", Order = 0)]
        [System.Xml.Serialization.XmlElement("MessageHeader")]
        public MessageHeaderVendorAckRequest messageHeaderVendorAckRequest;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://mes.gov/acknowledgment", Order = 1)]
        [System.Xml.Serialization.XmlElement("Response")]
        public VendorAckSubReqResponse[] vendorAckSubResponse;

        public VendorAcknowledgmentRequestReference()
        {
        }

    }

    [DataContract(Namespace = "http://mes.gov/acknowledgment")]
    [XmlSerializerFormat]
    public class MessageHeaderVendorAckRequest
    {
        string businessFlow;
        string requestorSystem;
        string requestorSystemId;
        string requestorTransactionId;
        string targetSystem;
        string targetTransactionId;
        string moduleTransactionId;
        string additionalModuleTransactionId;
        string sITransactionKey;
        string timeStamp;

        [DataMember(IsRequired = false, Order = 0)]
        [XmlElement(IsNullable = true)]
        public string BusinessFlow { get => businessFlow; set => businessFlow = value; }

        [DataMember(IsRequired = false, Order = 1)]
        [XmlElement(IsNullable = true)]
        public string RequestorSystem { get => requestorSystem; set => requestorSystem = value; }

        [DataMember(IsRequired = false, Order = 2)]
        public string RequestorSystemId { get => requestorSystemId; set => requestorSystemId = value; }

        [DataMember(IsRequired = false, Order = 3)]
        public string RequestorTransactionId { get => requestorTransactionId; set => requestorTransactionId = value; }

        [DataMember(IsRequired = false, Order = 4)]
        [XmlElement(IsNullable = true)]
        public string TargetSystem { get => targetSystem; set => targetSystem = value; }

        [DataMember(IsRequired = false, Order = 5)]
        public string TargetTransactionId { get => targetTransactionId; set => targetTransactionId = value; }

        [DataMember(IsRequired = false, Order = 6)]
        public string ModuleTransactionId { get => moduleTransactionId; set => moduleTransactionId = value; }

        [DataMember(IsRequired = false, Order = 7)]
        public string AdditionalModuleTransactionId { get => additionalModuleTransactionId; set => additionalModuleTransactionId = value; }

        [DataMember(IsRequired = false, Order = 8)]
        [XmlElement(IsNullable = true)]
        public string SITransactionKey { get => sITransactionKey; set => sITransactionKey = value; }

        [DataMember(IsRequired = false, Order = 9)]
        [XmlElement(IsNullable = true)]
        public string TimeStamp { get => timeStamp; set => timeStamp = value; }

    }

    [DataContract(Namespace = "http://mes.gov/acknowledgment")]
    [XmlSerializerFormat]
    public class VendorAckSubReqResponse
    {
        string seqNum;

        string respCode;

        string respType;

        string respMess;

        string responseDetails;

        [DataMember(IsRequired = false, Order = 0)]
        public string SequenceNumber { get => seqNum; set => seqNum = value; }

        [DataMember(IsRequired = false, Order = 1)]
        [XmlElement(IsNullable = true)]
        public string ResponseCode { get => respCode; set => respCode = value; }

        [DataMember(IsRequired = false, Order = 2)]
        [XmlElement(IsNullable = true)]
        public string ResponseType { get => respType; set => respType = value; }

        [DataMember(IsRequired = false, Order = 3)]
        [XmlElement(IsNullable = true)]
        public string ResponseMessage { get => respMess; set => respMess = value; }

    }

}