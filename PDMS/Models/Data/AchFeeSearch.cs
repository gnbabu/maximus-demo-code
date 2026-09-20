using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public class AchFeeSearch : ISearchCriteria
    {
        public string Name { get; set; }
        public string TaxID { get; set; }
        public string NPI { get; set; }
        public string MedicaidID { get; set; }
        public DateTime? FeePaidDateFrom { get; set; }
        public DateTime? FeePaidDateTo { get; set; }
        public DateTime? FeeDueDateFrom { get; set; }
        public DateTime? FeeDueDateTo { get; set; }

        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }

        public bool AdditionalCritierionSet
        {
            get
            {
                bool foundOther = false;
                if ((!string.IsNullOrEmpty(Name))
                    || (!string.IsNullOrEmpty(TaxID))
                    || (!string.IsNullOrEmpty(NPI))
                    || (!string.IsNullOrEmpty(MedicaidID))
                    || (FeeDueDateFrom.HasValue)
                    || (FeeDueDateTo.HasValue)
                    )
                    foundOther = true;

                return foundOther;
            }
        }
    }
}
