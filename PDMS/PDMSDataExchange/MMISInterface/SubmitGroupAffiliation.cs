using FileHelpers;
using System;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     MMIS submit provider group affiliation process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [DelimitedRecord("|")]
    public class SubmitGroupAffiliation : MMISRequest
    {
        public string actionFlag;

        public string providerCategory;

        public string atypicalIndicator;

        public string transactionId;

        public string groupMedicaidId;

        public string pdmsProviderId;

        public string medicaidId;

        public string npi;
        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? npiStartDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? npiEndDate;

        public string businessName;

        public string firstName;

        public string lastName;

        public string middleName;

        public string gender;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? dateofBirth;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? dateofDeath;

        public string profitStatus;

        public string riskLevel;
        
        public string providerType;

        public string specialty;

        public string specialty2;

        public string specialty3;

        public string licenseNumber;

        public string enrollmentStatus;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? affiliationEligibilityStartDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? affiliationEligibilityEndDate;

        public string ssn;

        public string ftin;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? termDate;

        public string termReason;

        public string poaExemptIndicator;

        public string ncpdpNumber;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? ncpdpFromDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? ncpdpToDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? rebateExemptionFromDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? rebateExemptionToDate;

        [FieldConverter(typeof(CustomDateConverter), "yyyyMMdd")]
        public DateTime? agreementDate;

        public string cliaNumber;

        public string medicareNumber;

        public string typeOfPractice;

        public string comments;
    }
}
