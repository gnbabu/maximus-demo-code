using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class Diagnosis
    {
        public Diagnosis(string service_code, string icdVersion, string service_desc)
        {
            this.ICD10Diag = service_code;
            this.DiagDesc = service_desc;
            this.ICDVersion = icdVersion;
        }
        private string _service_code;
        private string _service_desc;
        private string _icdVersion;
        public string ICD10Diag { get; set; }

        public string ICDVersion { get; set; }

        public string DiagDesc { get; set; }
    }
    public class DiagnosisCodeTypes
    {
        public string PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_DESC { get; set; }

        public string PRIOR_AUTH_DIAGNOSIS_CODE_TYPE_ID { get; set; }
    }
    public class DiagnosisNewLineAdd
    {
        public List<PriorAuthDiagnosis> priorAuthDiagnoses { get; set; }

        public List<DiagnosisCodeTypes> diagnosisCodeTypes { get; set; }
    }
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

    public class DiagnosisSave
    {
        public string claimType { get; set; }
        public string diagnosisCode { get; set; }
        public string diagnosisCodeDesc { get; set; }
        public string diagonsisType { get; set; }
        public string diagnosisTypeText { get; set; }
        public string diagUpdateDate { get; set; }
        public string MedicaidId { get; set; }
        public string action { get; set; }
        public string diagId { get; set; }
    }
}