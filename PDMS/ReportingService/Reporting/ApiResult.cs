using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ApiResult
    {
        public bool ApiStatus { get; set; }
        public bool Status { get; set; }
        public string PublishedItemId { get; set; }
        public int Version { get; set; }
        public string StatusMessage { get; set; }
    }
}
