using MAXIMUS.Models.Data.Interfaces.PDMS;
using System;

namespace MAXIMUS.Models.Data.PDMS
{
    public partial class CommunicationEventData: ISearchCriteria
    {
        public CommunicationEventData() { }

        public Guid? UserID { get; set; }

        public string CommunicationEventType { get; set; }

        public string EmailFrom { get; set; }

        public string EmailTo { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string TemplateName { get; set; }

        public string KeyValuePair { get; set; }

        public string NPI { get; set; }

        public int CommunicationEventID { get; set; }

        public int RegID { get; set; }

        public int PartyID { get; set; }

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
