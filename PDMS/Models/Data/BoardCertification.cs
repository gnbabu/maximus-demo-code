using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class BoardCertifications: ISearchCriteria
    {
        public BoardCertifications() { }

        public Guid? UserID { get; set; }

        public Guid? CreatedUserId { get; set; }

        public int RegBoardCertificationID { get; set; }

        public int BoardCertificationID { get; set; }

        public int BoardSpecialtyID { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string CertificationNumber { get; set; }

        public string BoardSpecialty { get; set; }


        public int RegID { get; set; }



        public DateTime LastModifiedDateTime { get; set; }

        public DateTime CreatedDateTime { get; set; }

        //*ISearchCriteria
        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }
        //*ISearchCriteria

        public string SortByColName { get; set; } 

        public string SortDirection { get; set; } //uses either SortDirection instead of SortAsc - not a common approach to paging and sorting at this time

        public bool IsPrimary { get; set; }
    }
}
