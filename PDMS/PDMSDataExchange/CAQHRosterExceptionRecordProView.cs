using FileHelpers;


namespace MAXIMUS.DataExchange.PDMS
{
    /// <summary>
    ///     Description:    This c# class is used by the CAQH classes to load records from the 
    ///                     CAQH exception roster process.
    /// </summary>
    /// <remarks>
    ///     Author(s):      jfetters
    ///     Date:           2015.02.19
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [DelimitedRecord("|")]
    [IgnoreEmptyLines(true)]
    [IgnoreFirst(1)]
    internal class CAQHRosterExceptionRecordProView
    {
#pragma warning disable 649
        public string actionFlag;

        public string firstName;

        public string middleName;

        public string lastName;

        public string suffix;

        public string gender;

        public string address1;

        public string address2;

        public string city;

        public string state;

        public string zip;

        public string practiceState;

        public string birthDate;

        public string ssn;

        public string shortSSN;

        public string deaNumber;

        public string upin;

        public string providerType;

        // public string planID;

        public string npi;

        public string licenseState;

        public string licenseNumber;

        public string providerId;

        public string providerStatus;

        public string planProviderId;

        public string applicationType;

        public string delegationFlag;

        public string affiliationFlag;

        public string organizationId;

        public string rosterFileName;
        
        public string exceptionDescription;

        public string rosterRecordNumber;

        public string exceptionDate;
#pragma warning restore 649
    }
}
