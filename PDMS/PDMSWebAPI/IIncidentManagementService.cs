using System.ServiceModel;

namespace PDMSWebAPI.IncidentManagement
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIncidentReport" in both code and config file together.

    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services", Name = "IncidentManagementService")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface IIncidentManagementService
    {
        //[OperationContract]
        //string HealthCheck();


        [OperationContract]
        sendProviderIncidentResponse SendProviderIncident(sendProviderIncidentRequest1 Incident);
    }
}
