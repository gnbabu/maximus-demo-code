using Corp.Core.Libraries.FI.ClaimsProfessionalService;

namespace Corp.Core.Libraries.ClaimsManagement
{
    /// <summary>
    /// Used to deserialize Professional Claims XML response from the service to InquireClaimResponse
    /// </summary>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRoot("Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public class ProfessionalEnvRes
    {
        private object headerField;

        private ProfessionalEnvelopeBodyInq bodyField;

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
        public ProfessionalEnvelopeBodyInq Body
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
    public partial class ProfessionalEnvelopeBodyInq
    {

        private InquireClaimResponse inquireClaimResponseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://service.operationmgmt.fi/ClaimsService/professional")]
        public InquireClaimResponse InquireClaimResponse
        {
            get
            {
                return this.inquireClaimResponseField;
            }
            set
            {
                this.inquireClaimResponseField = value;
            }
        }
    }
}