using FileHelpers;
using MAXIMUS.DataExchange.PDMS.MMISInterface;
using System;

namespace MAXIMUS.DataExchange.PDMS
{
    [IgnoreEmptyLines(true)]
    [DelimitedRecord(",")]
    class RetrieveBill2Pay
    {
#pragma warning disable 0649
        public string StatusCode;
        public string ConfirmationNumber;
        public string VendorReferenceCode;
        public string AccountNumber1;
        public string RegistrationNumber;
        public string RegistrantFirstName;
        public string RegistrantLastName;
        public string TotalPaymentAmount;
        [FieldConverter(typeof(CustomDateConverter), "MMddyyyy")]
        public DateTime? TransactionDate;
#pragma warning restore 0649
    }
}
