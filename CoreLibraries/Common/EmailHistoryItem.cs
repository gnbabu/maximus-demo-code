using System;

namespace MAXIMUS.Core.Libraries
{
    public class EmailHistoryItem
    {
        public int HistoryId { get; set; }
        public int BatchId { get; set; }
        public string NPI { get; set; }
        public string TaxId { get; set; }
        public int PartyId { get; set; }
        public string EmailFields { get; set; }
        public DateTime SendDateTime { get; set; }
        public string LastModifiedUser { get; set; }
    }
}
