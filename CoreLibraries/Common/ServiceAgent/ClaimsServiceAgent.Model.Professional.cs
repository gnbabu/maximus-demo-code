using System;

namespace Corp.Core.Libraries
{
    [Serializable]
    public class Diagnosis
    {
        public string cde_diag_seq { get; set; }
        public string qlf_code_list { get; set; }
        public string cde_diag { get; set; }
        public string presentOnAdmission { get; set; }
        public string diagnosisDescription { get; set; }
    }

    [Serializable]
    public class ProfessionalServiceDetail
    {
        public string num_dtl { get; set; }
        public string cde_proc { get; set; }
        public string qty_billed { get; set; }
        public string procedureModifier { get; set; }
        public string diagnosisPointer { get; set; }
        public string placeOfService { get; set; }
        public string dte_svc { get; set; }
        public string refferralEpsdt { get; set; }

        public string unitsOfMeasure { get; set; }
        public string amt_monetary { get; set; }
        public string paidUnits { get; set; }
        public string paidAmt { get; set; }
        public string cde_clm_status { get; set; }

        public DateTime? ServiceDate
        {
            get
            {
                if (!string.IsNullOrEmpty(dte_svc))
                {
                    return Convert.ToDateTime(dte_svc);
                }
                return null;
            }
        }
    }


    [Serializable]
    public class NDCDetail
    {
        public string seqNo { get; set; }
        public string sak_short { get; set; }
        public string cde_prod_serv_id { get; set; }
        public string cde_unit_measure { get; set; }
        public string num_prescription_id { get; set; }
        public string amt_drug_unit_price { get; set; }
        public string qty_units_svc { get; set; }
    }

}
