using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
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
}
