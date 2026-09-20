using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class RegProvider
    {
        public string NPI { get; set; }
        public string MEDICAID_ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string ADDRESS1 { get; set; }
        public string ADDRESS2 { get; set; }
        public string CITY { get; set; }
        public string STATE { get; set; }
        public string ZIP { get; set; }
        public string EXT_ZIP { get; set; }
        public string LAST_OR_BUSINESS_NAME { get; set; }

    }
}
