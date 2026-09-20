using FileHelpers;
using System.ComponentModel.DataAnnotations;

namespace MAXIMUS.DataExchange.PDMS.MCPN
{
    [DelimitedRecord(",")]
    [IgnoreEmptyLines(true)]
    public class MCPAffiliationRecord
    {
        [FieldQuoted]
        public string recordType;

        [FieldQuoted]
        public string trackingNumber;

        [FieldQuoted]
        public string startDate;

        [FieldQuoted]
        public string endDate;

        [FieldQuoted]
        public string recordTypeNumber;

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
        public string languages;

        [FieldQuoted]
        public string tpaName;

        [FieldQuoted]
        public string comments;

        [FieldQuoted]
        public string panelPCPCount;

        [FieldQuoted]
        public string mcpnSpecialties;

        [FieldQuoted]
        public string mitsSpecialties;

        [FieldQuoted]
        public string mitsProviderType;

        [FieldQuoted]
        [FieldOptional]
        public string errors;

        [Required(AllowEmptyStrings = false, ErrorMessage = "084")]// Field is required cannot be left blank
        [RegularExpression(MCPShared.regEx1NumOrBlank, ErrorMessage = "084")]
        public string RecordType { get => recordType; set => recordType = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "097")]// Field is required cannot be left blank
        [RegularExpression(@"(^\d{11}$)", ErrorMessage = "096")]//Tracking Number: Invalid Format
        public string TrackingNumber { get => trackingNumber; set => trackingNumber = value; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "089")]// Field is required cannot be left blank

        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "088")]//Start Date: Invalid Format

        public string StartDate { get => startDate; set => startDate = value; }
        [RegularExpression(MCPShared.regExDateStringFormat, ErrorMessage = "021")]//End Date: Invalid Format
        public string EndDate { get => endDate; set => endDate = value; }
        public string RecordTypeNumber { get => recordTypeNumber; set => recordTypeNumber = value; }
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

        [RegularExpression(MCPShared.regEx2LenNumOrBlank, ErrorMessage = "049")]
        public string Languages { get => languages; set => languages = value; }
        public string TpaName { get => tpaName; set => tpaName = value; }
        public string Comments { get => comments; set => comments = value; }
        public string PanelPCPCount { get => panelPCPCount; set => panelPCPCount = value; }
        public string McpnSpecialties { get => mcpnSpecialties; set => mcpnSpecialties = value; }
        public string MitsSpecialties { get => mitsSpecialties; set => mitsSpecialties = value; }
    }
}
