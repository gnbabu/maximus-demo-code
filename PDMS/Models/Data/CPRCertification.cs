using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class CPRCertifications: ISearchCriteria
    {
        public CPRCertifications() { }

        public Guid? UserID { get; set; }

        public bool IsCPRCertified { get; set; }

        public int RegCPRCertificationID { get; set; }

        public int RegFirstAidCertificationID { get; set; }

        public string Classifications { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool IsFirstAidCertified { get; set; }

        public DateTime FirstAidExpirationDate { get; set; }

        public string FirstAidClassifications { get; set; }

        public DateTime Created_On_Date_Time { get; set; }

        public Guid? Created_By_User { get; set; }

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
