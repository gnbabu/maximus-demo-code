using FileHelpers;
using System.ComponentModel.DataAnnotations;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines(true)]
    public class MCPProviderGroupRecord
    {
        [FieldQuoted]
        public string recordType;

        [FieldQuoted]
        public string trackingNumber;

        [FieldQuoted]
        public string mpn;

        [FieldQuoted]
        public string prn;

        [FieldQuoted]
        public string npi;

        [FieldQuoted]
        public string licenseNumber;

        [FieldQuoted]
        public string startDate;

        [FieldQuoted]
        public string endDate;

        [FieldQuoted]
        public string firstName;

        [FieldQuoted]
        public string middleInitial;

        [FieldQuoted]
        public string lastName;

        [FieldQuoted]
        public string genderCode;

        [FieldQuoted]
        public string providerType;

        [FieldQuoted]
        public string hospitalPrivilegesTrackingNumber;

        [FieldQuoted]
        public string primarySpecialty;

        [FieldQuoted]
        public string primarySpecialtyTrackingNumber;

        [FieldQuoted]
        public string primaryMITSProviderType;

        [FieldQuoted]
        public string secondaryMITSProviderType;

        [FieldQuoted]
        public string primaryMITSSpecialty;

        [FieldQuoted]
        [FieldOptional]
        public string errors;

        [Required(AllowEmptyStrings = false, ErrorMessage = "084")]// Field is required cannot be left blank
        [RegularExpression(MCPShared.regEx1NumOrBlank, ErrorMessage = "084")]
        public string RecordType { get => recordType; set => recordType = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "097")]// Field is required cannot be left blank
        [RegularExpression(@"(^\d{11}$)", ErrorMessage = "096")]//Tracking Number: Invalid Format
        public string TrackingNumber { get => trackingNumber; set => trackingNumber = value; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "142")]//MPN: Field is required cannot be left blank
        [RegularExpression(MCPShared.regExUnicodeLen7, ErrorMessage = "058")]
        public string Mpn { get => mpn; set => mpn = value; }
        public string Prn { get => prn; set => prn = value; }
        [RegularExpression(MCPShared.regEx10LenNumOrBlank, ErrorMessage = "063")]
        public string Npi { get => npi; set => npi = value; }
        public string LicenseNumber { get => licenseNumber; set => licenseNumber = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "089")]// Field is required cannot be left blank

        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "088")]//Start Date: Invalid Format
        public string StartDate { get => startDate; set => startDate = value; }

        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "021")]//End Date: Invalid Format
        public string EndDate { get => endDate; set => endDate = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string MiddleInitial { get => middleInitial; set => middleInitial = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string GenderCode { get => genderCode; set => genderCode = value; }
        public string ProviderType { get => providerType; set => providerType = value; }
        public string HospitalPrivilegesTrackingNumber { get => hospitalPrivilegesTrackingNumber; set => hospitalPrivilegesTrackingNumber = value; }
        public string PrimarySpecialty { get => primarySpecialty; set => primarySpecialty = value; }
        public string PrimarySpecialtyTrackingNumber { get => primarySpecialtyTrackingNumber; set => primarySpecialtyTrackingNumber = value; }
        public string PrimaryMITSProviderType { get => primaryMITSProviderType; set => primaryMITSProviderType = value; }
        public string SecondaryMITSProviderType { get => secondaryMITSProviderType; set => secondaryMITSProviderType = value; }
        public string PrimaryMITSSpecialty { get => primaryMITSSpecialty; set => primaryMITSSpecialty = value; }

    }
}
