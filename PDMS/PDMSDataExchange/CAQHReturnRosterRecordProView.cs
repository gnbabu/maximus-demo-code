using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to load records from the 
    ///                     CAQH retrieve roster process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2015.02.19
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [DelimitedRecord("|")]
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHReturnRosterRecordProView
    {
#pragma warning disable 649
        public string planID;

        public string authorizationFlag;

        public string status;

        public string statusDate;

        public string planProviderId;

        public string providerId;

        public string firstName;

        public string middleName;

        public string lastName;

        public string providerTypeAbbreviation;

        public string address;

        public string address2;

        public string city;

        public string state;

        public string zip;

        public string birthDate;

        public string licenseNumber;

        public string licenseState;

        public string upin;

        public string deaNumber;

        public string npi;

        public string rosterStatus;

        public string nonResponderFlag;

        public string delegationFlag;

        public string affiliationFlag;

        public string primaryPracticeState;

        public string anniversaryDate;
#pragma warning restore 649
    }
}
