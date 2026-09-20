using System.Collections.Generic;
using System.Xml.Serialization;

namespace MAXIMUS.DataExchange.PDMS
{
    [System.SerializableAttribute()]
    [XmlRoot("PaymentInformation")]
    public class SendBill2PayRequest
    {
        public string SecurityToken;
        public string VendorReferenceCode;
        public List<CartItem> CartItems;
    }

    public class CartItem
    {
        public string ProductName;
        public string AccountNumber1;
        public string Amount;
    }
}
