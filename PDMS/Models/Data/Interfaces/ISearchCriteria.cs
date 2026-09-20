namespace MAXIMUS.Models.Data.Interfaces.PDMS
{
    public interface ISearchCriteria
    {
        int SortBy { get; set; }
        int PageNumber { get; set; }
        int RowsPerPage { get; set; }
        bool SortAsc { get; set; }
    }
}
