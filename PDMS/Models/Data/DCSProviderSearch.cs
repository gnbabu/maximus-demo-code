using MAXIMUS.Models.Data.Interfaces.PDMS;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class DCSProviderSearch : Provider, ISearchCriteria
    {
        public DCSProviderSearch()
        {
        }

        public string DCSAffiliation { get; set; }

        public int SortBy { get; set; }

        public int PageNumber { get; set; }

        public int RowsPerPage { get; set; }

        public bool SortAsc { get; set; }

        public bool AdditionalCritierionSet
        {
            get
            {
                bool foundOther = false;
                if ((!string.IsNullOrEmpty(LastName))
                    || (!string.IsNullOrEmpty(OrganizationName))
                    || (!string.IsNullOrEmpty(NPI))
                    || (ProviderCategoryTypeID > 0)
                    || (!string.IsNullOrEmpty(TaxID))
                    || (ProviderTypeID > 0)
                    || (!string.IsNullOrEmpty(DCSAffiliation))
                    || (!string.IsNullOrEmpty(BaseMedicaidID))
                    )
                    foundOther = true;

                return foundOther;
            }
        }
    }
}
