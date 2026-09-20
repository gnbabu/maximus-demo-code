using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataComparer
{
    public class STG_HCPCS_LVL2_CODE
    {
        public string ID { get; set; }
        public string CODE { get; set; }
        public string SEQ_NUM { get; set; }
        public string LONG_DESCRIPTION { get; set; }
        public string SHORT_DESC { get; set; }
        public string ADD_DATE { get; set; }
        public string ADD_EFF_DATE { get; set; }
        public string TERM_DATE { get; set; }
        public string ACT_CDE { get; set; }
        public string LOAD_DATE { get; set; }
    }

    public class QueryList
    {
        public string Query { get; set; }
    }
}
