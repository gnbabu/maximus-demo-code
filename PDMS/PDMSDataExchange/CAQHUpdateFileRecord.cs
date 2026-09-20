using FileHelpers;

namespace MAXIMUS.DataExchange.PDMS
{
    [DelimitedRecord("|")]   
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHUpdateFileRecord
    {
        [FieldTrim(TrimMode.Both)]
        public string planProviderId;

        [FieldTrim(TrimMode.Both)]
        public string firstName;

        [FieldTrim(TrimMode.Both)]
        public string middleName;

        [FieldTrim(TrimMode.Both)]
        public string lastName;

        [FieldTrim(TrimMode.Both)]
        public string suffix;

        [FieldTrim(TrimMode.Both)]
        public string NPI;

        [FieldTrim(TrimMode.Both)]
        public string providerId;

        [FieldTrim(TrimMode.Both)]
        public string attestId;

        [FieldTrim(TrimMode.Both)]
        public string newSequenceID; // Revision 2 DirectoryMaintenance files [started on 2/1/2013]

        [FieldTrim(TrimMode.Both)]
        public string attestDate;

        [FieldTrim(TrimMode.Both)]
        public string sectionChange;

        [FieldTrim(TrimMode.Both)]
        public string fieldChange;

        [FieldTrim(TrimMode.Both)]
        public string changeType;

        [FieldTrim(TrimMode.Both)]
        public string previousValue;

        [FieldTrim(TrimMode.Both)]
        public string currentValue;

        //[FieldConverter(ConverterKind.Date, "yyyy-MM-dd")]
        //[FieldOptional]
        //[FieldFixedLength(10)]
        //public string reasonCode;

    }
}
