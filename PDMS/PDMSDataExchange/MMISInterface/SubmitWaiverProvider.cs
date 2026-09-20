using FileHelpers;
using System;
using System.Globalization;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    public class CustomDateConverter : ConverterBase
    {
        private string formatString;
        public CustomDateConverter(string format)
        {
            formatString = format;
        }
        public override object StringToField(string from)
        {
            if (from == null)
            {
                return string.Empty;
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParseExact(from, formatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    return dt;
                }
            }
            return from;
        }

        public override string FieldToString(object from)
        {
            string value = string.Empty;
            if (from != null)
            {
                DateTime dt;
                if (DateTime.TryParse(from.ToString(), out dt))
                {
                    if (dt.Year > 1753)
                    {
                        value = dt.ToString(formatString);
                    }
                }
            }
            return value;
        }
    }

    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     submit waiver provider process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [DelimitedRecord("|")]
    public class SubmitWaiverProvider : MMISRequest
    {
        public string actionFlag;

        public string transactionId;

        public string pdmsProviderId;

        public string organizationId;

        public string businessName;
        
        public string firstName;

        public string lastName;

        public string middleName;

        public string gender;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]  
        public DateTime? dateofBirth;

        public string profitStatus;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]  
        public DateTime? eligibilityStartDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? eligibilityEndDate;
        
        public string address;

        public string address2;

        public string city;

        public string state;

        public string zip;

        public string zipPlusFour;

        public string phone;

        public string mailAddress;

        public string mailAddress2;

        public string mailCity;

        public string mailState;

        public string mailZip;

        public string mailZip4;

        public string mailPhone;

        public string dba;

        public string contactEMail;

        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]  
        public DateTime? termDate;

        public string termReason;

        public string ftinType;

        public string ftin;

        public string taxFormType;

        public string taxClassificationType;

        public string bankName;

        public string routingNumber;

        public string accountNumber;

        public string accountType;

        public string accountTypeEntity;

        public string accountName;

        public string debitIndicator;
    }
}

