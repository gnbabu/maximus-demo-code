using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class OwnerConvictionOnBehalf: ISearchCriteria
    {
        public OwnerConvictionOnBehalf() { }

        public Guid? UserID { get; set; }


        public int REG_OWNER_CONVICTION_ON_BEHALF_ID { get; set; }

        public string Name { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string City { get; set; }
        public string State { get; set; }

        public string Zip { get; set; }

        public string ExtZip { get; set; }
        public string County { get; set; }

        public string SSN { get; set; }


        public string TaxID { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RegID { get; set; }

        public string TimeOfOffense { get; set; }

        public string MAtterOfOffense { get; set; }

        public string JuriDateOfOffense { get; set; }
        public string ProgramAreaOffense { get; set; }
        public string SanctionPeriodOffense { get; set; }
        public string FederalConviction { get; set; }
        public string StateOffense { get; set; }
        

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
