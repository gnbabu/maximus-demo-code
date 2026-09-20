using FileHelpers;
using System.ComponentModel.DataAnnotations;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines(true)]
  
    public class MCPServiceLocationRecord
    {
        [FieldQuoted]
        public string recordType;

        [FieldQuoted]
        public string trackingNumber;

        [FieldQuoted]
        public string providerGroupTrackingNumber;

        [FieldQuoted]
        public string healthCenterTrackingNumber;

        [FieldQuoted]
        public string groupLocationTrackingNumber;

        [FieldQuoted]
        public string npiNumber;

        [FieldQuoted]
        public string startDate;

        [FieldQuoted]
        public string endDate;

        [FieldQuoted]
        public string addressLine1;

        [FieldQuoted]
        public string addressLine2;

        [FieldQuoted]
        public string city;

        [FieldQuoted]
        public string state;

        [FieldQuoted]
        public string zip;

        [FieldQuoted]
        public string zip4;

        [FieldQuoted]
        public string countyCode;

        [FieldQuoted]
        public string phone;

        [FieldQuoted]
        public string phoneExtension;

        [FieldQuoted]
        public string tpaName;

        [FieldQuoted]
        public string programCode;

        [FieldQuoted]
        public string isPCP;

        [FieldQuoted]
        public string panelCapacity;

        [FieldQuoted]
        public string existingPatientsOnly;

        [FieldQuoted]
        public string gendersAccepted;

        [FieldQuoted]
        public string ageLimitLow;

        [FieldQuoted]
        public string ageLimitHigh;

        [FieldQuoted]
        public string acceptNewborns;

        [FieldQuoted]
        public string acceptPregnantWomen;

        [FieldQuoted]
        public string acceptFamilyMembers;

        [FieldQuoted]
        public string fulltimeEquivalency;

        [FieldQuoted]
        public string languages;

        [FieldQuoted]
        public string specialtiesAndBoardCertified;

        [FieldQuoted]
        public string availability24Hours;

        [FieldQuoted]
        public string comments;

        [FieldQuoted]
        public string mitsSpecialties;

        [FieldQuoted]
        public string panelPCPCount;

        [FieldQuoted]
        [FieldOptional]
        public string errors;



        [Required(AllowEmptyStrings = false, ErrorMessage = "084")]// Field is required cannot be left blank
        [RegularExpression(MCPShared.regEx1NumOrBlank, ErrorMessage = "084")]
        public string RecordType { get => recordType; set => recordType = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "097")]// Field is required cannot be left blank
        [RegularExpression(@"(^\d{11}$)", ErrorMessage = "096")]//Tracking Number: Invalid Format
        public string TrackingNumber { get => trackingNumber; set => trackingNumber = value; }
        public string ProviderGroupTrackingNumber { get => providerGroupTrackingNumber; set => providerGroupTrackingNumber = value; }
        public string HealthCenterTrackingNumber { get => healthCenterTrackingNumber; set => healthCenterTrackingNumber = value; }
        public string GroupLocationTrackingNumber { get => groupLocationTrackingNumber; set => groupLocationTrackingNumber = value; }
        [RegularExpression(MCPShared.regEx10LenNumOrBlank, ErrorMessage = "063")]
        public string NpiNumber { get => npiNumber; set => npiNumber = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "089")]// Field is required cannot be left blank

        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "088")]//Start Date: Invalid Format
        public string StartDate { get => startDate; set => startDate = value; }

        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "021")]//End Date: Invalid Format
        public string EndDate { get => endDate; set => endDate = value; }
        public string AddressLine1 { get => addressLine1; set => addressLine1 = value; }
        public string AddressLine2 { get => addressLine2; set => addressLine2 = value; }
        public string City { get => city; set => city = value; }
        [RegularExpression(MCPShared.regExState, ErrorMessage = "091")]
        public string State { get => state; set => state = value; }
        public string Zip { get => zip; set => zip = value; }
        public string Zip4 { get => zip4; set => zip4 = value; }
        public string CountyCode { get => countyCode; set => countyCode = value; }
        public string Phone { get => phone; set => phone = value; }
        public string PhoneExtension { get => phoneExtension; set => phoneExtension = value; }
        public string TpaName { get => tpaName; set => tpaName = value; }
        [RegularExpression(MCPShared.regEx1NumOrBlank, ErrorMessage = "078")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "079")]// Field is required cannot be left blank
        public string ProgramCode { get => programCode; set => programCode = value; }
        [RegularExpression(MCPShared.regExBoolean, ErrorMessage = "044")]
        public string IsPCP { get => isPCP; set => isPCP = value; }
        [RequiredIfAttribute("IsPCP", "1", "068")]
        public string PanelCapacity { get => panelCapacity; set => panelCapacity = value; }
        [RequiredIfAttribute("IsPCP", "1", "024")]
        [RegularExpression(MCPShared.regExBoolean, ErrorMessage = "023")]
        public string ExistingPatientsOnly { get => existingPatientsOnly; set => existingPatientsOnly = value; }
        [RegularExpression(MCPShared.regEx1NumOrBlank, ErrorMessage = "033")]
        public string GendersAccepted { get => gendersAccepted; set => gendersAccepted = value; }
        [RegularExpression(MCPShared.regEx2LenNumOrBlank, ErrorMessage = "014")]
        public string AgeLimitLow { get => ageLimitLow; set => ageLimitLow = value; }
        [RegularExpression(MCPShared.regEx2LenNumOrBlank, ErrorMessage = "012")]
        public string AgeLimitHigh { get => ageLimitHigh; set => ageLimitHigh = value; }
        [RegularExpression(MCPShared.regExBoolean, ErrorMessage = "004")]
        public string AcceptNewborns { get => acceptNewborns; set => acceptNewborns = value; }
        [RegularExpression(MCPShared.regExBoolean, ErrorMessage = "006")]
        public string AcceptPregnantWomen { get => acceptPregnantWomen; set => acceptPregnantWomen = value; }
        [RegularExpression(MCPShared.regExBoolean, ErrorMessage = "002")]
        public string AcceptFamilyMembers { get => acceptFamilyMembers; set => acceptFamilyMembers = value; }
        public string FulltimeEquivalency { get => fulltimeEquivalency; set => fulltimeEquivalency = value; }
        [RegularExpression(MCPShared.regEx2LenNumOrBlank, ErrorMessage = "049")]
        public string Languages { get => languages; set => languages = value; }
        public string SpecialtiesAndBoardCertified { get => specialtiesAndBoardCertified; set => specialtiesAndBoardCertified = value; }
        public string Availability24Hours { get => availability24Hours; set => availability24Hours = value; }
        public string Comments { get => comments; set => comments = value; }
        public string MitsSpecialties { get => mitsSpecialties; set => mitsSpecialties = value; }
        public string PanelPCPCount { get => panelPCPCount; set => panelPCPCount = value; }


    }
}
