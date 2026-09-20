using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RptGroup
    {
        public string Description { get; set; }
        public int Id { get; set; }
        public bool IsActiveDirectoryGroup { get; set; }
        public bool IsAzureADGroup { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class Groups
    {
        public RptGroup[] GroupList { get; set; }
    }
}
