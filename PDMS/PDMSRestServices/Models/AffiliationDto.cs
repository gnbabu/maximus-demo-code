namespace PDMSRestServices.Models
{
    public   class AffiliationMinimalDto
    {
        public string GroupName { get; set; } = string.Empty;
        public string Npi { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }   
        public DateTime? EndDate { get; set; }   
        public string Status { get; set; } = string.Empty;
    }
    public class HospitalAffiliationDTO
    {
        public string FacilityName { get; set; } = string.Empty;
        public string FacilityMedicaidID { get; set; } = string.Empty;

        public string StaffCategory { get; set; } = string.Empty;

        public string StatusofPrivileges { get; set; } = string.Empty;

        public bool Is_Primary_Facility { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

}
