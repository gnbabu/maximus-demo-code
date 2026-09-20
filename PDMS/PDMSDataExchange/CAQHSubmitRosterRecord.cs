using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{

    internal static class CAQHSubmitRosterRecordHeaderRow
    {
        public static CAQHSubmitRosterRecord GetHeaderRow()
        {
            CAQHSubmitRosterRecord record = new CAQHSubmitRosterRecord();
            record.actionFlag = "ActionFlag";
            record.address = "Address";
            record.address2 = "Address2";
            record.appType = "AppType";
            record.birthDate = "BirthDate";
            record.caqhProviderId = "CAQHProviderId";
            record.city = "City";
            record.dea = "DEA";
            record.delegationFlag = "DelegationFlag";
            record.email = "Email";
            record.extZip = "ExtZip";
            record.fax = "Fax";
            record.firstName = "FirstName";
            record.gender = "Gender";
            record.lastName = "LastName";
            record.licenseNumber = "LicenseNumber";
            record.middleName = "MiddleName";
            record.phone = "Phone";
            record.planID = "PlanID";
            record.planProviderID = "PlanProviderID";
            record.practiceState = "PracticeState";
            record.providerType = "ProviderType";
            record.recredentialingDate1 = "RecredentialingDate1";
            record.recredentialingDate2 = "RecredentialingDate2";
            record.regionTag = "RegionTag";
            record.shortSSN = "ShortSSN";
            record.ssn = "Ssn";
            record.state = "State";
            record.suffix = "Suffix";
            record.taxId1 = "TaxId1";
            record.taxId2 = "TaxId2";
            record.taxId3 = "TaxId3";
            record.taxId4 = "TaxId4";
            record.upin = "Upin";
            record.zip = "Zip";

            return record;
        }
    }
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to generate records for the 
    ///                     CAQH submit roster process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2012.06.06
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [DelimitedRecord("|")]
     internal class CAQHSubmitRosterRecord
    {

        // if changing also change headerText variable in CAQH.cs

        public string actionFlag;

        public string firstName;

        public string middleName;

        public string lastName;

        public string suffix;

        public string gender;

        public string address;

        public string address2;

        public string city;
        
        public string state;

        //[FieldFixedLength(5)]
        public string zip;

        public string extZip;

        public string phone;

        //[FieldFixedLength(10)]
        //[FieldNullValue(typeof(string), string.Empty)]
        public string fax;

        public string email;

        public string practiceState;

        //[FieldConverter(ConverterKind.Date, "MM/dd/yyyy")]
        public string birthDate;

        public string ssn;

        public string dea;

        public string upin;

        public string providerType;

        //[FieldFixedLength(9)]
        public string taxId1;

        //[FieldFixedLength(9)]
        public string taxId2;

        //[FieldFixedLength(9)]
        public string taxId3;

        //[FieldFixedLength(9)]
        public string taxId4;

        //[FieldFixedLength(19)]
        public string licenseNumber;

        //[FieldFixedLength(10)]
        public string caqhProviderId;

        public string planProviderID;

        // [FieldConverter(ConverterKind.Date, "MM/dd/yyyy")]
        public string recredentialingDate1 = null;

        // [FieldConverter(ConverterKind.Date, "MM/dd/yyyy")]
        public string recredentialingDate2 = null;

        public string delegationFlag;

        public string regionTag;

        public string shortSSN;

        public string appType;

        public string planID = "594";

    }
}
