using System.ServiceModel;

namespace PDMSWebAPI.PartialProviderManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract( Namespace = "http://Maximus.OHPNM.Services/", Name = "PartialProviderManagement")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface IPartialProviderManagement
    {

        [OperationContract]
        submitPartialProviderResponse SubmitPartialProvider(submitPartialProviderRequest partialRequest);

        //[OperationContract]
        //Fault SubmitParitialProviderResponse(string SiTransactionKey, string ModuleTranscationID, string ResponseCode, string ReponseType, int ODSProviderDemographicsID, string ResponseMessage, string ResponseDetails);

        // TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    //[DataContract(Namespace = "http://Maximus.OHPNM.Services")]
    //public class CompositeType
    //{
    //    bool boolValue = true;
    //    string stringValue = "Hello ";

    //    [DataMember]
    //    public bool BoolValue
    //    {
    //        get { return boolValue; }
    //        set { boolValue = value; }
    //    }

    //    [DataMember]
    //    public string StringValue
    //    {
    //        get { return stringValue; }
    //        set { stringValue = value; }
    //    }
    //}

}
