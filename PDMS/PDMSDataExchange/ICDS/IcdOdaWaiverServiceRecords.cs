using FileHelpers;
using System;
using System.Globalization;

namespace MAXIMUS.DataExchange.PDMS.ICDS
{
    [DelimitedRecord("|")]
    [IgnoreEmptyLines]
    public class IcdOdaWaiverServiceRecords
    {
        [FieldQuoted]
        private string medicaidID;

        [FieldQuoted]
        private string providerName;

        [FieldQuoted]
        private string serviceCountyName;

        [FieldQuoted]
        private string businessPhone;

        [FieldQuoted]
        private string businessAddress;

        [FieldQuoted]
        private string businessCity;

        [FieldQuoted]
        private string businessState;

        [FieldQuoted]
        private string businessZipCode;

        [FieldQuoted]
        private string odaServiceCode;

        [FieldQuoted]
        private string serviceDescp;

        [FieldQuoted]
        [FieldConverter(ConverterKind.Decimal, ".")]
        private decimal serviceRate;

        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        private DateTime contractedServiceBeginDate;

        [FieldQuoted]
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        private DateTime contractedServiceEndDate;

        [FieldQuoted]
        private string odaApprovalDate;

        [FieldQuoted]
        private string lvlOneSanctionInd;

        [FieldQuoted]
        private string lvlTwoSanctionInd;

        [FieldQuoted]
        private string lvlThreeSanctionInd;

        [FieldQuoted]
        private string ceaseReferralsInd;

        [FieldQuoted]
        private string recordLvlInd;

        [FieldQuoted]
        private string npi;

        [FieldQuoted]
        private string error;

        public string MEDICAID_ID { get => medicaidID; set => medicaidID = value; }
        public string ProviderName { get => providerName; set => providerName = value; }
        public string ServiceCountyName { get => serviceCountyName; set => serviceCountyName = value; }
        public string BusinessPhone { get => businessPhone; set => businessPhone = value; }
        public string BusinessAddress { get => businessAddress; set => businessAddress = value; }
        public string BusinessCity { get => businessCity; set => businessCity = value; }
        public string BusinessState { get => businessState; set => businessState = value; }
        public string BusinessZipCode { get => businessZipCode; set => businessZipCode = value; }
        public string OdaServiceCode { get => odaServiceCode; set => odaServiceCode = value; }
        public string ServiceDescp { get => serviceDescp; set => serviceDescp = value; }
        public decimal ServiceRate { get => serviceRate; set => serviceRate = value; }
        public DateTime ContractedServiceBeginDate { get => contractedServiceBeginDate; set => contractedServiceBeginDate = value; }
        public DateTime ContractedServiceEndDate { get => contractedServiceEndDate; set => contractedServiceEndDate = value; }
        public string OdaApprovalDate { get => odaApprovalDate; set => odaApprovalDate = value; }
        public string LvlOneSanctionInd { get => lvlOneSanctionInd; set => lvlOneSanctionInd = value; }
        public string LvlTwoSanctionInd { get => lvlTwoSanctionInd; set => lvlTwoSanctionInd = value; }
        public string LvlThreeSanctionInd { get => lvlThreeSanctionInd; set => lvlThreeSanctionInd = value; }
        public string CeaseReferralsInd { get => ceaseReferralsInd; set => ceaseReferralsInd = value; }
        public string RecordLvlInd { get => recordLvlInd; set => recordLvlInd = value; }
        public string NPI { get => npi; set => npi = value; }
        public string Error { get => error; set => error = value; }

    }

    /*
    public class CustomDateConverter : ConverterBase
    {
        protected override bool CustomNullHandling
        {
            /// you need to tell the converter not 
            /// to handle empty values automatically
            get { return true; }
        }

        public override object StringToField(string from)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;
            string format = "yyyyMMdd";
            if (string.IsNullOrEmpty(from))
            {
                return Convert.ToDateTime("1900-01-01");
            }
            else
            {
                return DateTime.ParseExact(from, format, provider);
            }
             
        }

        public override string FieldToString(object from)
        {
            return ((DateTime) from).ToString("yyyyMMdd");
        }
    }

    */
}
