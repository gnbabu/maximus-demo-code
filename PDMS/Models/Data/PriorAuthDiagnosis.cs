using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class PriorAuthDiagnosis
    {
        public int PRIOR_AUTH_DIAGNOSIS_ID { get; set; }
        public string PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC { get; set; }
        public string PRIOR_AUTH_DIAGNOSIS_CODE { get; set; }
        public string PRIOR_AUTH_DIAGNOSIS_DESC { get; set; }
        public string PRIOR_AUTH_DIAGNOSIS_DATE { get; set; }
        public int PRIOR_AUTH_DIAGNOSIS_STATUS { get; set; }
        public int PRIOR_AUTH_TYPE { get; set; }
        public int PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID { get; set; }
    }

    public class DiagnosisNewLineAdd
    {
        public List<PriorAuthDiagnosis> priorAuthDiagnoses { get; set; }

        public List<DiagnosisCodeTypes> diagnosisCodeTypes { get; set; }
    }
}
