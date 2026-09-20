using PDMSWebAPI.ProviderApplicationStatusInq;
using System.ServiceModel;

namespace PDMSWebAPI.ProviderInquiryServiceData
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProviderApplicationStatusInqService" in both code and config file together.
    [ServiceContract(Namespace = "http://Maximus.OHPNM.Services/", Name = "ProviderApplicationStatusInqService")]
    [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
    public interface IProviderAppStatusIVRServiceData
    {
        [OperationContract]
        ProvideAppStatusInquiryDataResponse ProvideAppStatusInquiryData(ProvideAppStatusInquiryDataRequest request);

        [OperationContract]
        ProvideStatusInquiryDataResponse ProvideStatusInquiryData(ProvideStatusInquiryDataRequest request);

        [OperationContract]
        ProvideAuthInquiryDataResponse ProvideAuthInquiryData(ProvideAuthInquiryDataRequest request);

        [OperationContract]
        ProvideGroupAffInquiryDataResponse ProvideGroupAffInquiryData(ProvideGroupAffInquiryDataRequest request);
    }
}