using System.Runtime.Serialization;
using System.ServiceModel;
using System.Xml.Serialization;

namespace PDMSWebAPI.VendorAckService
{
    [MessageContract(WrapperNamespace = "http://mes.gov/acknowledgment", IsWrapped = false, WrapperName = "VendorAcknowledgmentResponse")]
    [XmlSerializerFormat]
    public class VendorAcknowledgmentResponseReference
    {

        public string VendorAcknowledgmentResultField;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://mes.gov/acknowledgment", Order = 0)]
        [System.Xml.Serialization.XmlElement("VendorAcknowledgmentResponse")]
        public VendorAcknowledgmentResult resse;

    }

    [DataContract(Namespace = "http://mes.gov/acknowledgment")]
    [XmlSerializerFormat]
    public class VendorAcknowledgmentResult
    {
        private string BusinessFlowField;
        private string RequestorSystemField;
        private string RequestorSystemIdField;
        private string RequestorTransactionIdField;
        private string TargetSystemField;
        private string TargetTransactionIdField;
        private string ModuleTransactionIdField;
        private string AdditionalModuleTransactionIdField;
        private string SITransactionKeyField;
        private string TimeStampField;
        private VendorAckSubResponse[] responseField;


        [DataMember(IsRequired = false, Order = 0)]
        [XmlNamespaceDeclarations()]
        public XmlSerializerNamespaces xmlsn
        {
            get
            {
                XmlSerializerNamespaces xsn = new XmlSerializerNamespaces();
                xsn.Add("ack", "http://mes.gov/acknowledgment");
                return xsn;
            }
            set
            {
                //Just provide an empty setter. 
            }
        }
        public string BusinessFlow { get => BusinessFlowField; set => BusinessFlowField = value; }

        [DataMember(IsRequired = false, Order = 1)]
        public string RequestorSystem { get => RequestorSystemField; set => RequestorSystemField = value; }

        [DataMember(IsRequired = false, Order = 2)]
        public string RequestorSystemId { get => RequestorSystemIdField; set => RequestorSystemIdField = value; }

        [DataMember(IsRequired = false, Order = 3)]
        public string RequestorTransactionId { get => RequestorTransactionIdField; set => RequestorTransactionIdField = value; }

        [DataMember(IsRequired = false, Order = 4)]
        public string TargetSystem { get => TargetSystemField; set => TargetSystemField = value; }

        [DataMember(IsRequired = false, Order = 5)]
        public string TargetTransactionId { get => TargetTransactionIdField; set => TargetTransactionIdField = value; }

        [DataMember(IsRequired = false, Order = 6)]
        public string ModuleTransactionId { get => ModuleTransactionIdField; set => ModuleTransactionIdField = value; }

        [DataMember(IsRequired = false, Order = 7)]
        public string AdditionalModuleTransactionId { get => AdditionalModuleTransactionIdField; set => AdditionalModuleTransactionIdField = value; }

        [DataMember(IsRequired = false, Order = 8)]
        public string SITransactionKey { get => SITransactionKeyField; set => SITransactionKeyField = value; }

        [DataMember(IsRequired = false, Order = 9)]
        public string TimeStamp { get => TimeStampField; set => TimeStampField = value; }

        [DataMember(IsRequired = false, Order = 10)]
        [System.Xml.Serialization.XmlElement("Response")]
        public VendorAckSubResponse[] Response { get => responseField; set => responseField = value; }
    }

    [DataContract(Namespace = "http://mes.gov/acknowledgment")]
    [XmlSerializerFormat]
    
    public class VendorAckSubResponse
    {
        private string seqNum;

        private string respCode;

        private string respType;

        private string respMess;

        private string responseDetails;

        [DataMember(IsRequired = false, Order = 0)]
        public string SequenceNumber { get => seqNum; set => seqNum = value; }

        [DataMember(IsRequired = false, Order = 1)]
        public string ResponseCode { get => respCode; set => respCode = value; }

        [DataMember(IsRequired = false, Order = 2)]
        public string ResponseType { get => respType; set => respType = value; }

        [DataMember(IsRequired = false, Order = 3)]
        public string ResponseMessage { get => respMess; set => respMess = value; }

        [DataMember(IsRequired = false, Order = 4)]
        public string ResponseDetails { get => responseDetails; set => responseDetails = value; }
    }



}