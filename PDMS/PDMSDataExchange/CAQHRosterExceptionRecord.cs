using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to load records from the 
    ///                     CAQH roster exception process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2012.06.08
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [DelimitedRecord("|")]
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHRosterExceptionRecord
    {
#pragma warning disable 649
        public string planID;

        public string exceptionDescription;

        public string providerId;

        public string prodProviderId;

        public string planProviderId;

        public string prodPlanProviderId;

        public string lastName;

        public string firstName;

        public string middleName;

        public string address;

        public string city;

        public string state;

        public string zip;

        public string extZip;

        public string birthDate;

        public string ssn;

        public string deaNumber;

        public string upin;

        public string licenseNumber;

        public string providerStatus;

        public string exceptionDate;

        public string providerTypeAbbreviation;
#pragma warning restore 649
    }
}
