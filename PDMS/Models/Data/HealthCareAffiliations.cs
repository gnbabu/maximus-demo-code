using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class HealthCareAffiliations: ISearchCriteria
    {
        public HealthCareAffiliations() { }

        public Guid? UserID { get; set; }

        public string FacilityMedicaidID { get; set; }

        public bool IsPrimaryFacility { get; set; }

        public bool IsRestrictedPrivilege { get; set; }

        public int RegHealthCareID { get; set; }

        public string FacilityName { get; set; }

        public string StaffCategory { get; set; }

        public string StatusOfPrivileges { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime Created_On_Date_Time { get; set; }

        public Guid? Created_By_User { get; set; }

        public int RegID { get; set; }

        public string Reason { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        public bool IsInpatientSetting { get; set; }

        public bool IsHospitalPrivileges { get; set; }

        public string HospitalPrivilegesReason { get; set; }

        //*ISearchCriteria
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }
        //*ISearchCriteria

        public string SortByColName { get; set; } 

        public string SortDirection { get; set; } //uses either SortDirection instead of SortAsc - not a common approach to paging and sorting at this time
    }
}
