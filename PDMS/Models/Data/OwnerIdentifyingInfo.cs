using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class OwnerIdentifyingInfo: ISearchCriteria
    {
        public OwnerIdentifyingInfo() { }

        public Guid? UserID { get; set; }




        public int REG_OWNER_PAPER_PROVIDER_ID { get; set; }

        public string EntityName { get; set; }
        public string DBAName { get; set; }

        public int OWNER_CATEGORY_TYPE_ID { get; set; }

        

        public DateTime BirthDate { get; set; }

        public string SSN { get; set; }
        public string MedicaidID { get; set; }
        public string NPI { get; set; }
        public string TaxID { get; set; }

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
