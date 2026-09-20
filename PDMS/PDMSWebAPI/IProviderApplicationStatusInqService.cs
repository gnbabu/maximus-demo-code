using PDMSWebAPI.ProviderApplicationStatusInq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PDMSWebAPI.ProviderInquiryService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProviderApplicationStatusInqService" in both code and config file together.
    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services/", Name = "ProviderApplicationStatusInqService")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface IProviderApplicationStatusInqService
    {
        [OperationContract]
        ProvideAppStatusInquiryDataResponse ProvideAppStatusInquiryData(ProvideAppStatusInquiryDataRequest request);
    }
}
