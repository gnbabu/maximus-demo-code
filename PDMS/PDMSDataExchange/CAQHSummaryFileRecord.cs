using FileHelpers;

namespace MAXIMUS.DataExchange.PDMS
{
    [DelimitedRecord("|")]   
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHSummaryFileRecord
    {
#pragma warning disable 649
        [FieldTrim(TrimMode.Both)]
        public decimal pin;

        [FieldTrim(TrimMode.Both)]
        public string planProviderId;

        [FieldTrim(TrimMode.Both)]
        public string licenseNumber;

        [FieldTrim(TrimMode.Both)]
        public string lastName;

        [FieldTrim(TrimMode.Both)]
        public string firstName;

        [FieldTrim(TrimMode.Both)]
        public string middleName;

        [FieldTrim(TrimMode.Both)]
        public string primaryPracticeState;

        [FieldTrim(TrimMode.Both)]
        public string status;

        [FieldTrim(TrimMode.Both)]
        public string xPath;

        [FieldTrim(TrimMode.Both)]
        public string table;

        [FieldTrim(TrimMode.Both)]
        public string element;

        [FieldTrim(TrimMode.Both)]
        public string oldData;

        [FieldTrim(TrimMode.Both)]
        public string newData;

        [FieldTrim(TrimMode.Both)]
        public string anniversaryDate;

        //[FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        //[FieldOptional]
        //[FieldFixedLength(10)]
        //public string reasonCode;
#pragma warning restore 649
    }
}
