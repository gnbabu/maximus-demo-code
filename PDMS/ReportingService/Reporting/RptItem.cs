using System;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RptItem
    {
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
        public bool CanDownload { get; set; }
        public bool CanSchedule { get; set; }
        public bool CanOpen { get; set; }
        public bool CanMove { get; set; }
        public bool CanCopy { get; set; }
        public bool CanClone { get; set; }
        public bool CanCreateItem { get; set; }
        public string Id { get; set; }
        public int ItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsFavorite { get; set; }
        public object[] Tags { get; set; }
        public int CreatedById { get; set; }
        public string CreatedByDisplayName { get; set; }
        public int ModifiedById { get; set; }
        public string ModifiedByFullName { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public DateTime ItemModifiedDate { get; set; }
        public DateTime ItemCreatedDate { get; set; }
        public string ReportId { get; set; }
    }

}
