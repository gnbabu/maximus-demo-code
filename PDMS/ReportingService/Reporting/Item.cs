using ReportingService.Common.Text;
using System;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Item
    {
        public string Id { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CreatedById { get; set; }
        public string CloneOf { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public DateTime ItemCreatedDate { get; set; }
        public DateTime ItemModifiedDate { get; set; }
        public string CreatedDate { get; set; }
        public string ModifiedDate { get; set; }
        public int ModifiedById { get; set; }
        public string Extension { get; set; }
        public bool IsPublic { get; set; }
        public string[] Tags { get; set; }
        public bool IsActive { get; set; }
        public int Version { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ItemDefinition: Item
    {
        public Base64String ItemXml { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ItemContent
    {
        public string PublishedItemId { get; set; }
        public int ItemType { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string FileContent { get; set; }
    }
}
