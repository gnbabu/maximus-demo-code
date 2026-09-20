using System.Collections.Generic;

namespace Corp.Core.Libraries
{

    public partial class ClaimsServiceAgent
    {
        public List<Diagnosis> Diagnoses
        {
            get
            {
                return ViewState["Diagnosis"] == null ? new List<Diagnosis>() : (List<Diagnosis>)ViewState["Diagnosis"];
            }
            set
            {
                ViewState["Diagnosis"] = value;
            }

        }

        public List<ProfessionalServiceDetail> ProfessionalServiceDetails
        {
            get
            {
                return ViewState["ProfessionalServiceDetail"] == null ? new List<ProfessionalServiceDetail>() : (List<ProfessionalServiceDetail>)ViewState["ProfessionalServiceDetail"];
            }
            set
            {
                ViewState["ProfessionalServiceDetail"] = value;
            }

        }
    }
}
