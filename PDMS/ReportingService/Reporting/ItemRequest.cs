using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ItemRequest
    {
        public Guid ItemId { get; set; }

        public DataSourceMappingInfo DatasourceDetails { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? CategoryId { get; set; }

        public string VersionId { get; set; }

        public byte[] ItemContent { get; set; }

        public string UploadedReportName { get; set; }

        public List<DataSourceMappingInfo> DataSourceMappingInfo { get; set; }

        public List<DatasetMappingInfo> DatasetMappingInfo { get; set; }

        public string VersionComment { get; set; }

        public ItemType ItemType { get; set; }

        public string ServerPath { get; set; }

        public bool FavoriteValue { get; set; }

        public bool IsPublic { get; set; }

        public List<string> ReportReferences { get; set; }

    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DataSourceMappingInfo
    {
        public string Name { get; set; }

        public string DataSourceId { get; set; }

        public string DataSourceName { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DatasetMappingInfo
    {
        public string Name { get; set; }

        public string DataSetId { get; set; }

        public string DataSetName { get; set; }
    }
}
