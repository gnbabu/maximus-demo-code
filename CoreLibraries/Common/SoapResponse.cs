using System.Xml.Serialization;

namespace Corp.Core.Libraries
{
    [XmlRootAttribute(Namespace = "http://mes.gov/providermanagement", IsNullable = false, ElementName = "EnrollProviderResponse")]
    public class SoapResponse
    {
        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", IsNullable = true, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }
    }

    [XmlRootAttribute(Namespace = "http://mes.gov/financialservice", IsNullable = false, ElementName = "Inquire1099Response")]
    public class SoapProviderFinancialInquireResponse
    {
        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", IsNullable = true, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }
    }

    [XmlRootAttribute(Namespace = "http://mes.gov/financialservice", IsNullable = false, ElementName = "InquireTransactionHistoryResponse")]
    public class SoapProviderFinancialHistoryResponse
    {
        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", IsNullable = true, ElementName = "AdditionalModuleTransactionId")]
        public string AdditionalModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/financialservice", IsNullable = true, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }
    }


    [XmlRootAttribute(Namespace = "http://mes.gov/providermanagement", IsNullable = false, ElementName = "UpdateProviderResponse")]
    public class SoapUpdateResponse
    {
        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/providermanagement", IsNullable = true, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }
    }

    [XmlRootAttribute(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = false, ElementName = "SubmitPartialProviderResponse")]
    public class PartialSoapResponse
    {
        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "AdditionalModuleTransactionId")]
        public string AdditionalModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ODSProviderDemographicsId")]
        public string ODSProviderDemographicsId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = false, ElementName = "Response")]
        public ResponseElement[] ResponseElement { get; set; }
    }

    [XmlRootAttribute(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = false, ElementName = "SubmitPartialProviderResponse")]
    public class PartialPubSubSoapResponse
    {
        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "SITransactionKey")]
        public string SITransactionKey { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ModuleTransactionId")]
        public string ModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "AdditionalModuleTransactionId")]
        public string AdditionalModuleTransactionId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ODSProviderDemographicsId")]
        public string ODSProviderDemographicsId { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = false, ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = false, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }
    }


    public class ResponseElement
    {
        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseCode")]
        public string ResponseCode { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseType")]
        public string ResponseType { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseMessage")]
        public string ResponseMessage { get; set; }

        [XmlElement(Namespace = "http://mes.gov/partialprovidermanagement", IsNullable = true, ElementName = "ResponseDetails")]
        public string ResponseDetails { get; set; }

    }


    [XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false, ElementName = "Fault")]
    public class FaultResponse
    {
        [XmlElement(Namespace = "", ElementName = "faultcode")]
        public string faultcode { get; set; }

        [XmlElement(Namespace = "", ElementName = "faultstring")]
        public string faultstring { get; set; }

    }
}
