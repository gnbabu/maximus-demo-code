using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to load records from the 
    ///                     CAQH retrieve roster process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2012.06.08
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
#pragma warning disable 649
    [DelimitedRecord("|")]
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHReturnRosterRecord
    {

        public string planID;

        public string authorizationFlag;

        public string status;

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

        public string upin;

        public string deaNumber;

        public string rosterStatus;

        public string nonResponderFlag;

        public string primaryPracticeState;

        public string anniversaryDate;
    }
    #pragma warning restore 649
}
