using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingService.Service.Reporting
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class RptAuthToken
    {
        public string access_token { get; set; }

        public string token_type { get; set; }

        public long expires_in { get; set; }

        public DateTime issued_at { get; set; }

        public DateTime expires_at { get; set; }

        public string email { get; set; }

        public string error { get; set; }

        public string error_description { get; set; }

        public int UserId { get; set; }

    }
}
