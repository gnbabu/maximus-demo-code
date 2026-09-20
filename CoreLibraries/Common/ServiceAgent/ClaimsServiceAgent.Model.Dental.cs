using System;

namespace Corp.Core.Libraries
{

    [Serializable]
    public class DentalServiceDetail
    {
        public string num_dtl_total { get; set; }
        public string cde_proc { get; set; }
        public string plc_service { get; set; }

        public string qty_billed { get; set; }
        public string qty_allowed { get; set; }
        public string dte_first_svc { get; set; }
        //public string dte_last_svc { get; set; }
        public string amt_billed { get; set; }
        public string amt_monetary { get; set; }
        public string cde_clm_status { get; set; }
        public string mdf_first { get; set; }
        public string mdf_secnd { get; set; }
        public string mdf_thrd { get; set; }
        public string mdf_forth { get; set; }
        public string cde_clm_chrge { get; set; }
        public string digno_first { get; set; }
        public string digno_sec { get; set; }
        public string digno_third { get; set; }
        public string digno_forth { get; set; }
        public string prior_auth { get; set; }
        public string pad_amnt { get; set; }
        public string line_ctr_num { get; set; }
        public string orl_cvt_first { get; set; }
        public string orl_cvt_sec { get; set; }
        public string orl_cvt_third { get; set; }
        public string orl_cvt_forth { get; set; }
        public string orl_cvt_fifth { get; set; }
        public string bil_unt { get; set; }
        public string ref_num { get; set; }
        public string prosthesis_cd { get; set; }
        public string pad_unt { get; set; }


        public DateTime? ServiceDate
        {
            get; set; 
            //{
            //    if (!string.IsNullOrEmpty(dte_first_svc))
            //    {
            //        return Convert.ToDateTime(dte_first_svc);
            //    }
            //    return null;
            //}
        }
    }

    [Serializable]
    public class ToothQuadrantInfo
    {
        public string num_dtl { get; set; }
        public string cde_tooth_nbr { get; set; }
        public string cde_quadrant { get; set; }
        public string cde_tooth_surface { get; set; }
        public string cde_tooth_surface1 { get; set; }
        public string cde_tooth_surface2 { get; set; }
        public string cde_tooth_surface3 { get; set; }
        public string cde_tooth_surface4 { get; set; }
        public string cde_tooth_surface5 { get; set; }
        
        public string tooth_surface1 { get; set; }
        public string tooth_surface2 { get; set; }
        public string tooth_surface3 { get; set; }
        public string tooth_surface4 { get; set; }
        public string tooth_surface5 { get; set; }


    }

    [Serializable]
    public class ClaimsAdjustment
    {
        public string cde_clm_adj_group { get; set; }
        public string cde_clm_adj_reason { get; set; }
        public string amt_adjustment { get; set; }
        public string num_cas_seq { get; set; }
    }   


}
