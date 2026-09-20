using Corp.Core.Libraries.FI.ClaimsInstitutionalService;

namespace Corp.Core.Libraries.ClaimsManagement
{
    /// <summary>
    /// Used to deserialize Institutional Claims XML response from the service to AddUpdateClaimResponse
    /// </summary>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class InstitutionalEnvSubmitRes
    {
        private object headerField;

        private InstitutionalEnvelopeBodySub bodyField;

        /// <remarks/>
        public object Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public InstitutionalEnvelopeBodySub Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class InstitutionalEnvelopeBodySub
    {

        private AddUpdateClaimsResponse addUpdateClaimsResponse;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://service.operationmgmt.fi/ClaimsService/institutional")]
        public AddUpdateClaimsResponse AddUpdateClaimsResponse
        {
            get
            {
                return this.addUpdateClaimsResponse;
            }
            set
            {
                this.addUpdateClaimsResponse = value;
            }
        }
    }
}