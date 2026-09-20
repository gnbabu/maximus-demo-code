using System.Xml.Serialization;

namespace MAXIMUS.DataExchange.PDMS
{
    [System.SerializableAttribute()]
    [XmlRoot("PaymentVerification")]
    public class RetrieveBill2PayPaymentVerification
    {
        public string SecurityToken;
        public string VendorReferenceCode;
    }
}
