namespace PDMSWebAPI.Models
{
    //[DataContractFormat]
    //public class ProviderIncidentReportModel
    //{
    //    [DataMember (IsRequired = true, Order = 1)]
    //    public MessageHeader MessageHeader { get; set; }

    //    [DataMember (IsRequired = true, Order = 2)]
    //    public Payload Payload { get; set; }

    //}

    //[DataContractFormat]
    //public class Payload
    //{
    //    [DataMember(IsRequired = true, Order = 1)]
    //    public List<SendProviderIncidentRequest> SendProviderIncidentInfo { get; set; }

    //}

    //// ENUMS FOR HEADER
    //[DataContract(Name = "BusinessFlow")]
    //public enum BusinessFlowEnum
    //{
    //    [EnumMember]
    //    SendProviderIncident = 10,
    //    [EnumMember]
    //    UpdateProviderIncidentStatus = 20
    //}

    //// ENUMS FOR HEADER
    //[DataContract(Name = "RequestorSystem")]
    //public enum RequestorSystemEnum
    //{
    //    [EnumMember]
    //    PNM = 10,
    //    [EnumMember]
    //    IMS = 20
    //}

    //[DataContractFormat]
    //public class MessageHeader
    //{
    //    [DataMember]
    //    public BusinessFlowEnum BusinessFlow { get; set; }

    //    [DataMember (IsRequired = true)]
    //    public string StateCode { get; set; }

    //    [DataMember (IsRequired = true)]
    //    public RequestorSystemEnum RequestorSystem { get; set; }

    //    [DataMember(IsRequired = true)]
    //    public string SubscriberSystem { get; set; }

    //    [DataMember]
    //    public string ModuleTransactionId { get; set; }

    //    [DataMember]
    //    public string AdditionalModuleTransactionId { get; set; }

    //    [DataMember]
    //    public DateTime RequestTimestamp { get; set; }

    //    [DataMember]
    //    public string SITransactionKey { get; set; }
    //}


    //[DataContractFormat]
    //public class SendProviderIncidentRequestOld
    //{
    //    [DataMember (IsRequired = true)]
    //    public string MedicaidProviderID { get; set; }

    //    [DataMember]
    //    public string ProviderName { get; set; }

    //    [DataMember]
    //    public string IMSCaseNumber { get; set; }

    //    [DataMember]
    //    public DateTime CaseSubmissionDate { get; set; }

    //    [DataMember]
    //    public DateTime NODReferralDate { get; set; }

    //    [DataMember]
    //    public string ReasonForNOD { get; set; }

    //    [DataMember]
    //    public string ReferralSentBy { get; set; }

    //    [DataMember (IsRequired = true)]
    //    public List<IncidentDetails> IncidentDetails { get; set; }
    //}


    //[DataContractFormat]
    //public class IncidentDetails
    //{
    //    [DataMember]
    //    public string IncidentCategory { get; set; }

    //    [DataMember]
    //    public bool IncidentSubcategory { get; set; }

    //    [DataMember]
    //    public string IncidentType { get; set; }

    //    [DataMember]
    //    public string IncidentId { get; set; }

    //    [DataMember]
    //    public string Substantiated { get; set; }

    //    [DataMember]
    //    public string IMSAssociateID { get; set; }
    //}

    //[DataContractFormat]
    //public class SendProviderIncidentResponseModel
    //{
    //    [DataMember]
    //    public string SITransactionKey { get; set; }

    //    [DataMember]
    //    public string ResponseCode { get; set; }

    //    [DataMember]
    //    public string ResponseType { get; set; }

    //    [DataMember]
    //    public string ResponseMessage { get; set; }
    //}

}

