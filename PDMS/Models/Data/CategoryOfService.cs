using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class CategoryOfService: ISearchCriteria
    {
        public CategoryOfService() { }

        public Guid? UserID { get; set; }



        public int RegCategoryOfServiceInfoID { get; set; }

        public int CategoryOfServiceID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }



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
