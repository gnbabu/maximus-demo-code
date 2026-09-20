using FileHelpers;
using MAXIMUS.Core.Libraries;
using System;

namespace MAXIMUS.DataExchange.PDMS.MMISInterface
{
    /// <summary>
    ///     Description:    This c# class is used by the MMIS classes to generate records for the 
    ///                     MMIS submit provider process.
    /// </summary>
    /// <remarks>
    ///     Helpful Links:  http://www.filehelpers.com/attributes.html
    /// </remarks>
    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProvider : MMISRequest
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string max_trans_id;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string max_action;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string max_pdms_id;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string id;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_medm_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string locn_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_sort_seq_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_prt_suscd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string pract_ty_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string indiv_grcd;

        //[FieldFixedLength(1)]
        //[FieldTrim(TrimMode.Both, Constants.TrimChar)]
        //public string fed_id_ind;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ssn_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string npi_num;

        [FieldFixedLength(11)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string nabnum;

        [FieldFixedLength(11)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dea_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? dea_eff_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? dea_exdt;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dba_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dba_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dba_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dba_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string dba_mi_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mi_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string faci_fy_mo_num;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mcare_ind;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mcare_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? mcare_beg_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? mcare_end_dt;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string profit_ind;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string blng_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string prof_tech_ind;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string atyind;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string pymt_mthd_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w1099_cd;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string tot_bed_num;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_tax_key_id;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string billing_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_billing_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_billing_ward_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string servicing_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_servicing_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_servicing_ward_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_to_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string mail_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_mail_to_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_mail_to_ward_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string other_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_other_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_other_ward_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ra_mailing_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_ra_mailing_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_ra_mailing_ward_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_nam_org_ind;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_nam;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_last_nam;

        [FieldFixedLength(15)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_fst_nam;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_mi_nam;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_sfx_nam;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_st_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_cnty_cd;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_line1_ad;

        [FieldFixedLength(30)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_line2_ad;

        [FieldFixedLength(20)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_city_nam;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_zip4_cd;

        [FieldFixedLength(5)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_zip5_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_phon_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_fax_num;

        [FieldFixedLength(35)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_contct_nam;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_contct_phon_num;

        [FieldFixedLength(128)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string w9_1099_contct_email_ad;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_w9_1099_quad_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string g_w9_1099_ward_cd;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string member_id;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string max_member_pdms_id;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? affl_beg_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? affl_end_dt;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string affl_ty_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string enrol_stat_ty_cd;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string aff_ssn_num;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string clia_num;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string clia_cert_ty_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? clia_cert_eff_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? clia_cert_expir_dt;

        [FieldFixedLength(250)]
        [FieldConverter(typeof(StatusArrayConverter))] 
        public SubmitProviderStatus[] ProviderStatus = new SubmitProviderStatus[10];

        [FieldFixedLength(420)]
        [FieldConverter(typeof(LicCertArrayConverter))] 
        public SubmitProviderLicense[] LicenseCerts = new SubmitProviderLicense[12];

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? prog_beg_dt_1;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? prog_end_dt_1;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string prog_cd_1;

        [FieldFixedLength(660)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(ProviderCOSArrayConverter))]
        public SubmitProviderCOS[] ProviderCOS = new SubmitProviderCOS[30];

        [FieldFixedLength(3150)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(ProviderSpecialtyArrayConverter))]
        public SubmitProviderSpecialty[] ProviderSpecialty = new SubmitProviderSpecialty[50];

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? tax_beg_dt_1;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? tax_end_dt_1;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string fed_id_ind_1;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string fed_tax_id_1;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ssn_num_1;

        [FieldFixedLength(300)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(ProviderTaxonomyArrayConverter))]
        public SubmitProviderTaxonomy[] ProviderTaxonomy = new SubmitProviderTaxonomy[10];

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string bkuwhold_ind;

        [FieldFixedLength(4)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string sec_bnote_yr_num;

        [FieldFixedLength(9)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string tax_key_id;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string owner_ty_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public string appl_dt;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string qstnr_npp_ind;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string qstnr_npr_ind;
    }

    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProviderStatus
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? stat_eff_dt;

        [FieldFixedLength(3)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string ty_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? stat_end_dt;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string enrol_stat_ty_cd;
    }

    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProviderLicense
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        /*[FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]*/
        public string lic_eff_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string lic_cert_num;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string lic_cert_cd;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string st_cd;

        [FieldFixedLength(1)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string lic_rstrct_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? lic_expir_dt;
    }

    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProviderCOS
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? cos_beg_dt;

        [FieldFixedLength(2)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string cos_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? cos_end_dt;
   }
    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProviderSpecialty
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? specl_beg_dt;

        [FieldFixedLength(3)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string specl_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? specl_end_dt;

        [FieldFixedLength(40)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string lic_brd_num;
    }

    [IgnoreEmptyLines(true)]
    [FixedLengthRecord(FixedMode.ExactLength)]
    public class SubmitProviderTaxonomy
    {
        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        public string taxonomy_cd;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? taxon_beg_dt;

        [FieldFixedLength(10)]
        [FieldTrim(TrimMode.Both, Constants.TrimChar)]
        [FieldConverter(typeof(CustomDateConverter), "yyyy-MM-dd")]
        public DateTime? taxon_end_dt;

    }
}
