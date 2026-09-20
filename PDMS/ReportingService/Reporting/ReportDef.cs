using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ReportDef
    {
        public string FileContent { get; set; }
        public string ItemName { get; set; }
        public string Extension { get; set; }
        public bool Status { get; set; }
        public string StatusMessage { get; set; }
    }

}
