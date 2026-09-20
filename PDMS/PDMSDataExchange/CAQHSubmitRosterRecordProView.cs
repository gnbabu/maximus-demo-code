using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{

    internal static class CAQHSubmitRosterRecordProViewHeaderRow
    {
        public static CAQHSubmitRosterRecordProView GetHeaderRow()
        {
            CAQHSubmitRosterRecordProView record = new CAQHSubmitRosterRecordProView();
            record.actionFlag = "Action_Flag";
            record.firstName = "Provider_First_Name";
            record.middleName = "Provider_Middle_Name";
            record.lastName = "Provider_Last_Name";
            record.suffix = "Provider_Name_Suffix";
            record.gender = "Provider_Gender";
            record.address = "Provider_Address1";
            record.address2 = "Provider_Address2";
            record.city = "Provider_Address_City";
            record.state = "Provider_Address_State";
            record.zip = "Provider_Address_Zip";
            record.extZip = "Provider_Address_Zip_Extn";
            record.phone = "Provider_Phone";
            record.fax = "Provider_Fax";
            record.email = "Provider_Email";
            record.practiceState = "Provider_Practice_State";
            record.birthDate = "Provider_Birthdate";
            record.ssn = "Provider_SSN";
            record.shortSSN = "Short_SSN";
            record.dea = "Provider_DEA";
            record.upin = "Provider_UPIN";
            record.providerType = "Provider_Type";
            record.taxId = "Provider_Tax_ID";
            record.npi = "Provider_NPI";
            record.licenseState = "Provider_License_State";
            record.licenseNumber = "Provider_License_Number";
            record.caqhProviderId = "CAQH_Provider_ID";
            record.planProviderID = "PO_Provider_ID";
            record.lastRecredentialingDate = "Last_Recredential_Date";
            record.nextRecredentialingDate = "Next_Recredential_Date";
            record.delegationFlag = "Delegation_Flag";
            record.appType = "Application_Type";
            record.affiliationFlag = "Affiliation_Flag";
            record.organizationId = "Organization_ID";
            record.regionId = "Region_ID";

            return record;
        }
    }
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to generate records for the 
    ///                     CAQH submit roster process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2015.02.17
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [DelimitedRecord("|")]
     internal class CAQHSubmitRosterRecordProView
    {

        // if changing also change headerText variable in CAQH.cs
        public string actionFlag;

        //[FieldFixedLength(150)]
        public string firstName;

        //[FieldFixedLength(150)]
        public string middleName;

        //[FieldFixedLength(150)]
        public string lastName;

        public string suffix;

        public string gender;

        //[FieldFixedLength(150)]
        public string address;

        //[FieldFixedLength(150)]
        public string address2;

        //[FieldFixedLength(150)]
        public string city;
        
        public string state;

        //[FieldFixedLength(5)]
        public string zip;

        public string extZip;

        public string phone;

        //[FieldFixedLength(10)]
        //[FieldNullValue(typeof(string), string.Empty)]
        public string fax;

        //[FieldFixedLength(150)]
        public string email;

        //[FieldFixedLength(2)]
        public string practiceState;

        //[FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public string birthDate;

        public string ssn;

        public string shortSSN;

        public string dea;

        public string upin;

        public string providerType;

        //[FieldFixedLength(9)]
        public string taxId;

        //[FieldFixedLength(10)]
        public string npi;

        //[FieldFixedLength(2)]
        public string licenseState;

        //[FieldFixedLength(50)]
        public string licenseNumber;

        //[FieldFixedLength(10)]
        public string caqhProviderId;

        public string planProviderID;

        // [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public string lastRecredentialingDate = null;

        // [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public string nextRecredentialingDate = null;

        //[FieldFixedLength(1)]
        public string delegationFlag;

        //[FieldFixedLength(1)]
        public string appType;

        //[FieldFixedLength(2)]
        public string affiliationFlag;

        //[FieldFixedLength(5)]
        public string organizationId = "594";

        //[FieldFixedLength(5)]
        public string regionId;
    }
}
