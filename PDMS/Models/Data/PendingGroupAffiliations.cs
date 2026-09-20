using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class PendingGroupAffiliations: ISearchCriteria
    {
        public PendingGroupAffiliations() { }

        public Guid? UserID { get; set; }

        public DateTime Created_On_Date_Time { get; set; }

        public Guid? Created_By_User { get; set; }

        public string NPI { get; set; }

        public int RegPendingAffiliationID { get; set; }

        public string GroupName { get; set; }

        public int RegID { get; set; }

        public string MedicaidID { get; set; }

        public DateTime LastModifiedDateTime { get; set; }

        //*ISearchCriteria
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }
        //*ISearchCriteria

        public string SortByColName { get; set; } 

        public string SortDirection { get; set; } //uses either SortDirection instead of SortAsc - not a common approach to paging and sorting at this time

        public string IndvName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int GrpRegID { get; set; }
        public string IndvNPI { get; set; }
        public int grpAffiliationStatus { get; set; }
    }
}
