using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class NursingProfessionalCertifications: ISearchCriteria
    {
        public NursingProfessionalCertifications() { }

        public Guid? UserID { get; set; }







        public bool IsPrimaryFacility { get; set; }

        public bool IsRestrictedPrivilege { get; set; }

        public int RegNursingProfessionalCertificationID { get; set; }

        public string CertificationName { get; set; }

        public string ReceivedFrom { get; set; }

        

        public DateTime Expiration { get; set; }



        public int RegID { get; set; }



        public DateTime LastModifiedDateTime { get; set; }

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
