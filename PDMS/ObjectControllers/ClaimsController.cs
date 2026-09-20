using Corp.Core.Libraries.ClaimsReference;
using MAXIMUS.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MAXIMUS.Controllers.PDMS
{
    public class AddUPdateClaimsRequestResponse
    {
        public PhysicianClaims PhysicianClaims { get; set; }
        public InstitutionalClaims InstitutionalClaims { get; set; }

        public DentalClaims DentalClaims { get; set; }
        //public DentalClaimsPDntl DentalClaimsPDntl { get; set; }
        public ClaimResponse ClaimResponse { get; set; }
    }
    public class ClaimsDetailModel
    {
        public PhysicianClaimsModel PhysicianClaimsEntity { get; set; }
    }

    #region PhysicianClaims
    public class PhysicianClaimsModel
    {
        public pPhysModel pPhys { get; set; }
        public clmTrailerModel clmTrailer { get; set; }
    }

    public class clmTrailerModel
    {
        public string icnRegionCode { get; set; }
    }

    #region pPhys
    public class pPhysModel
    {
        public string ind_attachment { get; set; }
        public string dte_accident { get; set; }
        public string num_dtl_total { get; set; }
        public string cde_clm_status { get; set; }
        public string cde_clm_type { get; set; }
        public string tpl_amt { get; set; }
        public string dte_billed { get; set; }
        public string amt_billed { get; set; }
        public string amt_patnt_liab { get; set; }
        public string num_pat_acct { get; set; }
        public string dte_first_svc { get; set; }
        public string dte_last_svc { get; set; }
        public string dte_to_hosp { get; set; }
        public string amt_net_billed { get; set; }
        public string amt_co_pay { get; set; }
        public string ind_accident { get; set; }
        public string cde_county { get; set; }
        public string cde_claim_frequency { get; set; }
        public string prov_billing { get; set; }
        public string num_icn { get; set; }
        public string ind_prov_sign { get; set; }
        public string ind_encounter { get; set; }
        public string cde_med_rec_num { get; set; }
        public string ind_carrier_denied { get; set; }
        public physHdrKeyModel physHdrKey { get; set; }
        public clmHdrDeliveryModel clmHdrDelivery { get; set; }
        public List<detailModel> detail { get; set; }
        public List<diag_xrefModel> diag_xref { get; set; }
    }

    #region physHdrKey
    public class physHdrKeyModel
    {
        public string cde_special_program { get; set; }
        public string id_medicaid { get; set; }
        public string clm_recip_fst_nam { get; set; }
        public string clm_lst_nam_recip { get; set; }
        public string qlf_sub_dob_fmt { get; set; }
        public string dte_subscriber_dob { get; set; }
        public string cde_subscriber_gender { get; set; }
        public string cde_med_assignment { get; set; }
        public string ind_benefits_Assignment { get; set; }
        public string cde_release_information { get; set; }
        public string cde_patient_signature { get; set; }
        public string ind_benefits_assignment { get; set; }
        public string cde_accident_state { get; set; }
        public string cde_accident_country { get; set; }
        public string id_provider { get; set; }
        public string id_perf_prov { get; set; }
        public string id_prov_refer { get; set; }
        public string id_prov_referring_2 { get; set; }
        public string cde_related_cause_1 { get; set; }
        public clmSbrModel clmSbr { get; set; }
        public List<clmEntity_physHdrKeyModel> clmEntity { get; set; }
        public List<profOthPyrHdrModel> profOthPyrHdr { get; set; }
        public List<clmNteModel> clmNte { get; set; }
        public List<clmRefModel> clmRef { get; set; }
        public List<clmDtpModel> clmDtp { get; set; }
        public List<clmPwkModel> clmPwk { get; set; }
        public List<clmCrcModel> clmCrc { get; set; }
    }
    public class clmSbrModel
    {
        public string cde_claim_filing_ind { get; set; }
        public string cde_insured_group_number { get; set; }
        public string cde_payer_responsib { get; set; }
        public string cde_ind_relationship { get; set; }
        public string cde_insurance_type { get; set; }
        public string nam_insured_group { get; set; }
    }
    public class profOthPyrHdrModel
    {
        public string ind_release_of_info { get; set; }
        public string ind_ben_assignment { get; set; }
        public string dte_other_sub_dob { get; set; }
        public string cde_release_of_info { get; set; }
        public string amt_alwd_oth_pyr { get; set; }
        public string cde_other_sub_gender { get; set; }
        public string ind_benefits_assignment { get; set; }
        public string dte_clm_adjudication { get; set; }
        public string qlf_claim_adjud_dt { get; set; }
        public string qlf_clm_adjud_dt_fmt { get; set; }
        public List<clmPayerEntityModel> clmPayerEntity { get; set; }
        public clmSbrModel clmSbr { get; set; }
        public List<clmAmtModel> clmAmt { get; set; }
        public List<clmCasModel> clmCas { get; set; }
    }
    public class clmAmtModel
    {
        public string qlf_amount { get; set; }
        public string amt_monetary { get; set; }
    }
    public class clmCasModel
    {
        public string cde_clm_adj_reason { get; set; }
        public string cde_clm_adj_group { get; set; }
        public string amt_adjustment { get; set; }
        public string qty_adjustment { get; set; }
        public string num_cas_seq { get; set; }
        public string num_dtl_svd { get; set; }
        public string num_dtl { get; set; }
    }
    public class clmNteModel
    {
        public string cde_note { get; set; }
        public string dsc_note { get; set; }
        public string num_dtl { get; set; }
    }
    public class clmDtpModel
    {
        public string qlf_date_time { get; set; }
        public string dte_time_period { get; set; }
    }
    public class clmPwkModel
    {
        public string cde_rpt_transmission { get; set; }
        public string cde_attachment_control { get; set; }
    }
    public class clmCrcModel
    {
        public string cde_category { get; set; }
        public string cde_condition { get; set; }
        public string ind_condition { get; set; }
    }
    #endregion
    public class clmHdrDeliveryModel
    {
        public string dte_delivery { get; set; }
    }

    #region detail
    public class detailModel
    {

        public string amt_detail_tpl { get; set; }
        public string amt_co_pay { get; set; }
        public string cde_diag_treat_ind { get; set; }
        public string cde_tooth_nbr { get; set; }
        public string cde_quadrant { get; set; }
        public string ind_pregnancy { get; set; }
        public string num_dtl { get; set; }
        public string dte_first_svc { get; set; }
        public string dte_last_svc { get; set; }
        public string qty_billed { get; set; }
        public string qty_allowed { get; set; }
        public string ind_emergency { get; set; }
        public string cde_proc_mod { get; set; }
        public string cde_modifier_2 { get; set; }
        public string cde_modifier_3 { get; set; }
        public string amt_billed { get; set; }
        public string amt_alwd { get; set; }
        public string cde_pos { get; set; }
        public string cde_clm_status { get; set; }
        public string cde_modifier_4 { get; set; }
        public string cde_prov_spec { get; set; }
        public string cde_epsdt_fp { get; set; }

        public physDtlKeyModel physDtlKey { get; set; }
        public clmEapgDtlModel clmEapgDtl { get; set; }

        public string num_seq { get; set; }
        public string cde_tooth_surface { get; set; }
    }

    public class ClmAmount
    {
        public string amt_monetary { get; set; }
        public string qlf_amount { get; set; }
        public DateTime dte_service_adjud { get; set; }
    }

    #region physDtlKey
    public class physDtlKeyModel
    {
        public string cde_proc { get; set; }
        public string ind_less_90 { get; set; }
        public string cde_pos_sub { get; set; }
        public string qlf_procedure_qlf { get; set; }
        public string qlf_svc_line_dte_fmt { get; set; }
        public string id_perf_prov { get; set; }
        public string id_prov_referring { get; set; }
        public List<clmEntityModel> clmEntity { get; set; }
        public List<clmNdcDtlModel> clmNdcDtl { get; set; }
    }

    #region clmEntity
    public class clmEntityModel
    {
        public string qlf_type_org { get; set; }
        public string qlf_entity_type { get; set; }
        public string cde_provider_type { get; set; }
        public string cde_prov_taxonomy { get; set; }
        public List<clmEntNmAdrModel> clmEntNmAdr { get; set; }
    }
    public class clmEntity_physHdrKeyModel
    {
        public string qlf_type_org { get; set; }
        public string qlf_entity_type { get; set; }
        public string cde_provider_type { get; set; }
        public string cde_prov_taxonomy { get; set; }
        public clmEntNmAdrModel clmEntNmAdr { get; set; }
    }
    public class clmEntNmAdrModel
    {
        public string qlf_entity_type { get; set; }
        public string ind_primary_id { get; set; }
        public partyIdentifierModel partyIdentifier { get; set; }
        public clmNameNm1Model clmNameNm1 { get; set; }
    }
    public class partyIdentifierModel
    {
        public string qty_billed_sub { get; set; }
        public string qty_billed_orig { get; set; }
        public string cde_proc_sub { get; set; }
        public string qlf_id_type { get; set; }
        public string cde_party_id { get; set; }
    }
    public class clmNameNm1Model
    {
        public string nam_first { get; set; }
        public string nam_middle { get; set; }
        public string nam_last { get; set; }
    }

    #endregion

    public class clmNdcDtlModel
    {
        public string sak_short { get; set; }
        public string cde_prod_serv_id { get; set; }
        public string qty_units_svc { get; set; }
        public string cde_unit_measure { get; set; }
        public string amt_drug_unit_price { get; set; }
        public string qlf_prod_serv_id { get; set; }
        public string qlf_prescription_id { get; set; }
        public string num_prescription_id { get; set; }
    }
    #endregion
    public class clmEapgDtlModel
    {
        public string cde_eapg_sub { get; set; }
    }
    #endregion

    public class diag_xrefModel
    {
        public string qlf_code_list { get; set; }
        public string cde_diag_seq { get; set; }
        public string cde_diag { get; set; }
    }

    #endregion

    public class clmPayerEntityModel
    {
        public string qlf_entity_type { get; set; }
        public string qlf_type_org { get; set; }
        public clmPyrEntnmadrModel clmPyrEntnmadr { get; set; }
        public clmRefModel clmRef { get; set; }
    }

    public class clmPyrEntnmadrModel
    {
        public string ind_primary_id { get; set; }
        public string qlf_entity_type { get; set; }
        public string nam_last { get; set; }
        public partyIdentifierModel partyIdentifier { get; set; }
        public clmNameNm1Model clmNameNm1 { get; set; }
    }

    public class clmRefModel
    {
        public string qlf_reference_id { get; set; }
        public string cde_ref_id { get; set; }
        public string qlf_entity_type { get; set; }
    }






    public class RenderingProviderSearch
    {
        public string id_perf_prov { get; set; }
        public string cde_party_id { get; set; }
        public string nam_last { get; set; }
        public string nam_first { get; set; }
    }
    public class SupervisingProviderSearch
    {
        public string id_perf_prov { get; set; }
        public string cde_party_id { get; set; }
        public string nam_last { get; set; }
        public string nam_first { get; set; }
    }
    public class ReferringProviderSearch
    {
        public string id_perf_prov { get; set; }
        public string cde_party_id { get; set; }
        public string nam_last { get; set; }
        public string nam_first { get; set; }
    }
    public static class ClaimsController
    {
        public static void UpdatePanelsData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {

                    if (pair.Value == null)
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedprocname = "";
                switch (tableName.ToLower())
                {
                    case "claim_adjudication_summary_status_screen": storedprocname = "usp_update_claim_adjudication_summary_status_Screen"; break;
                    case "claims_provider_information": storedprocname = "update_Claims_Provider_Information"; break;
                    case "claims_recipent_information": storedprocname = "usp_Update_Claims_Recipent_Information"; break;
                    case "claims_outpatient_adjudication_information": storedprocname = "updateClaims_Outpatient_Adjudication_Informationcustom"; break;
                    case "claims_tooth_and_surface_information": storedprocname = "updateClaims_Tooth_and_Surface_InformationCustom"; break;
                    case "claims_accident_information": storedprocname = "updateClaims_Accident_InformationCustom"; break;
                    case "claim_prior_authorization_referral": storedprocname = "update_Claim_Prior_Authorization_Referral_custom"; break;
                    case "claims_service_details": storedprocname = "updateClaims_Service_Details"; break;
                    case "claims_delayed_submission_resubmission_information": storedprocname = "update_Claims_Delayed_Submission_Resubmission_Information"; break;
                    case "claims_diagnosis_information": storedprocname = "update_claims_diagnosis_information"; break;
                    case "claims_service_information": storedprocname = "updateClaims_Service_Information"; break;
                    case "claims_service_facility_information": storedprocname = "updateclaims_service_facility_information"; break;
                    case "claims_additional_provider_information_service": storedprocname = "update_Claims_Additional_Provider_Information_Service_Custom"; break;
                    case "claims_other_payer_adjudication_information_service_detail": storedprocname = "update_Claims_Other_Payer_Adjudication_Information_Service_Detail"; break;
                    case "claims_providers_note": storedprocname = "Usp_UpdateClaim_Provider_Notes"; break;
                    case "claims_other_payer_adjustment_service_detail": storedprocname = "update_Claims_Other_Payer_Adjustment_Service_Detail"; break;
                    case "claims_other_payer_information": storedprocname = "updateClaims_Other_Payer_InformationCustom"; break;
                    case "claims_ambulance_pick_up_drop_off_location": storedprocname = "Usp_Update_Claims_Ambulance_Pick_Up_Drop_Off_Location"; break;
                    case "claims_ambulance_information": storedprocname = "usp_update_Claims_Ambulance_Information"; break;
                    case "claims_service_information_professional": storedprocname = "Usp_update_Claims_Service_Information_Professional"; break;
                    case "claims_header_other_payer_adjustment_information": storedprocname = "updateClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "claims_service_information_institutional": storedprocname = "Usp_update_Claims_Service_Information_Institutional"; break;
                    case "claims_ndc_details": storedprocname = "Usp_update_Claims_NDC_Details"; break;

                    case "claims_occurrence_information": storedprocname = "updateClaims_Occurrence_Information"; break;

                    case "claims_inpatient_adjudication_information": storedprocname = "usp_Update_Claims_Inpatient_Adjudication_Information"; break;
                    case "claims_icd_procedurecode": storedprocname = "Usp_update_Claims_ICD_Procedure_Code_Sequence"; break;
                    case "claims_occurrence_code_span_information": storedprocname = "Usp_Update_Claims_Occurrence_Code_Span_Information"; break;
                    case "claims_condition_code_information": storedprocname = "Usp_Update_Claims_Condition_Code_Information"; break;
                    case "claims_value_code_information": storedprocname = "Usp_Update_Claims_Value_Code_Information"; break;


                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataAccess.ExecuteStoredProcedure(storedprocname, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }
        }
        public static int InsertPanelsData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedproc = string.Empty;
                switch (tableName.ToLower())
                {
                    case "claim_adjudication_summary_status_screen": storedproc = "insertClaim_Adjudication_Summary_Status_Screen"; break;
                    case "claims_provider_information": storedproc = "insertclaims_provider_information"; break;
                    case "claims_outpatient_adjudication_information": storedproc = "insertClaims_Outpatient_Adjudication_Information"; break;

                    case "claims_tooth_and_surface_information": storedproc = "insertclaims_tooth_and_surface_information"; break;
                    case "claims_accident_information": storedproc = "insertClaims_Accident_InformationCustom"; break;
                    case "claim_prior_authorization_referral": storedproc = "insertClaim_prior_authorization_referral"; break;
                    case "claims_other_payer_information": storedproc = "insertClaims_Other_Payer_Information"; break;
                    case "claims_service_details": storedproc = "insertClaims_Service_Details"; break;
                    case "claims_delayed_submission_resubmission_information": storedproc = "insertclaims_delayed_submission_resubmission_information"; break;
                    case "claims_diagnosis_information": storedproc = "insertclaims_diagnosis_information"; break;
                    case "claims_recipent_information": storedproc = "insertClaims_Recipent_Information"; break;
                    case "claims_service_information": storedproc = "insertClaims_Service_Information"; break;
                    case "claims_service_facility_information": storedproc = "insertclaims_service_facility_information"; break;
                    case "claims_additional_provider_information_service": storedproc = "insertclaims_additional_provider_information_service"; break;
                    case "claims_other_payer_adjudication_information_service_detail": storedproc = "insertClaims_Other_Payer_Adjudication_Information_Service_Detail"; break;
                    case "claims_providers_note": storedproc = "Usp_Insert_Claim_Provider_Notes"; break;
                    case "claims_other_payer_adjustment_service_detail": storedproc = "insertClaims_Other_Payer_Adjustment_Service_Detail"; break;
                    case "claims_header_other_payer_adjustment_information": storedproc = "insertClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "claims_ambulance_pick_up_drop_off_location": storedproc = "usp_Insert_Claims_Ambulance_Pick_Up_Drop_Off_Location"; break;
                    case "claims_ambulance_information": storedproc = "usp_Insert_Claims_Ambulance_Information"; break;
                    case "claims_service_information_professional": storedproc = "Usp_Insert_Claims_Service_Information_Professional"; break;
                    case "claims_service_information_institutional": storedproc = "Usp_Insert_Claims_Service_Information_Institutional"; break;
                    case "claims_ndc_details": storedproc = "InsertClaims_NDC_Details"; break;

                    case "claims_occurrence_information": storedproc = "insertClaims_Occurrence_Information"; break;

                    case "claims_inpatient_adjudication_information": storedproc = "insertClaims_Inpatient_Adjudication_Information"; break;
                    case "claims_icd_procedurecode": storedproc = "Usp_Insert_Claims_ICD_Procedure_Code_Sequence"; break;
                    case "claims_occurrence_code_span_information": storedproc = "insertClaims_Occurrence_Code_Span_Information"; break;
                    case "claims_condition_code_information": storedproc = "insertClaims_Condition_Code_Information"; break;
                    case "claims_value_code_information": storedproc = "insertClaims_Value_Code_Information"; break;
                    case "claims_attachment_screen": storedproc = "insertClaims_Attachment_Screen"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }

                int identityId = Convert.ToInt32(DataAccess.ExecuteScalar(storedproc, parameters));
                return identityId;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }

        }
        public static DataSet SelectPanelsData(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }
                string storedproc = string.Empty;
                switch (tableName.ToLower())
                {
                    case "claim_adjudication_summary_status_screen": storedproc = "usp_Selectclaim_Adjudication_Summary_Status_Screen"; break;
                    case "claims_recipent_information": storedproc = "usp_SelectClaims_Recipient_Information"; break;
                    case "claims_tooth_and_surface_information": storedproc = "usp_SelectClaims_Tooth_and_Surface_Information"; break;
                    case "claims_accident_information": storedproc = "usp_Selectclaims_accident_information"; break;
                    case "claims_other_payer_information": storedproc = "usp_SelectClaims_Other_Payer_Information"; break;
                    case "claim_prior_authorization_referral": storedproc = "usp_SelectClaim_Prior_Authorization_Referral"; break;
                    case "claims_service_details": storedproc = "usp_SelectClaims_Service_Details"; break;
                    case "claims_outpatient_adjudication_information": storedproc = "usp_SelectClaims_Outpatient_Adjudication_Information"; break;
                    case "claims_delayed_submission_resubmission_information": storedproc = "usp_SelectClaims_Delayed_Submission_Resubmission_Information"; break;
                    case "claims_service_information": storedproc = "usp_SelectClaims_Service_Details_GetServiceDate"; break;
                    case "claims_service_facility_information": storedproc = "usp_SelectClaims_Service_Facility_Information"; break;
                    case "claims_additional_provider_information_service": storedproc = "usp_Select_Claims_Additional_Provider_Information_Service"; break;
                    case "claims_diagnosis_information": storedproc = "usp_SelectClaimsDiagnosisInformation"; break;
                    case "claims_header_other_payer_adjustment_information":
                        storedproc = "usp_SelectClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "edit_claims_header_other_payer_adjustment_information":
                        storedproc = "usp_EditClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "claims_other_payer_adjudication_information_service_detail": storedproc = "usp_SelectClaimsOtherPayerAdjudicationInformation"; break;
                    case "claims_providers_note": storedproc = "Usp_SelectClaim_Provider_Notes"; break;
                    case "notereferencecode": storedproc = "Usp_Select_ProviderNote_ReferenceCode"; break;
                    case "claims_other_payer_adjustment_service_detail": storedproc = "usp_SelectClaims_Other_Payer_Adjustment_Service_Detail"; break;
                    case "claims_ambulance_pick_up_drop_off_location": storedproc = "usp_Select_Claims_Ambulance_Pick_Up_Drop_Off_Location"; break;
                    case "claims_ambulance_information": storedproc = "usp_Select_Claims_Ambulance_Information"; break;
                    case "claims_service_information_professional": storedproc = "Usp_Select_Claims_Service_Information_Professional"; break;
                    case "claims_service_information_institutional": storedproc = "Usp_Select_Claims_Service_Information_Institutional"; break;
                    case "claims_ndc_details": storedproc = "Usp_Select_Claims_NDC_Details"; break;

                    case "claims_occurrence_information": storedproc = "Usp_Select_Claims_Occurrence_Information"; break;

                    case "claims_inpatient_adjudication_information": storedproc = "usp_Select_Inpatient_Adjudication_Information"; break;
                    case "claims_icd_procedurecode": storedproc = "Usp_Select_Claims_ICD_Procedure_Code_Sequence"; break;
                    case "claims_occurrence_code_span_information": storedproc = "usp_Select_Claims_Occurrence_Code_Span_Information"; break;
                    case "claims_occurrence_code": storedproc = "Usp_Select_Claims_Occurrence_Code"; break;
                    case "claims_condition_code_information": storedproc = "usp_Select_Claims_Condition_Code_Information"; break;
                    case "claims_sequence_description": storedproc = "Usp_SelectClaimsDiagnosis_Sequence_Description"; break;
                    case "claims_value_code_information": storedproc = "Usp_Select_Claims_Value_Code_Information"; break;
                    case "claims_value_code": storedproc = "usp_Select_Claims_Value_Code"; break;
                    case "claims_attachment_screen": storedproc = "usp_Select_Claims_Attachments"; break;
                    case "claims_additional_provider_information_filter_serviceline": storedproc = "usp_Select_Claims_Additional_Provider_Information_Filter_ServiceLine"; break;
                    case "claims_other_payer_adjudication_servicedetail_with_serviceline": storedproc = "usp_Select_Claims_Other_Payer_Adjudication_ServiceDetail_With_ServiceLine"; break;

                    case "claims_search_details": storedproc = "usp_Search_ClaimDetails"; break;
                    case "claims_search_details_inst": storedproc = "usp_Search_ClaimDetails_Inst"; break;
                    case "claims_search_details_prof": storedproc = "usp_Search_ClaimDetails_Prof"; break;
                    case "destinationpayerid": storedproc = "usp_Select_DESTINATIONPAYERID"; break;
                    case "taxonomycodeforbillingprovider": storedproc = "usp_select_taxonomycodeforbillingprovider"; break;
                    case "claims_outbound_document_uploads": storedproc = "usp_Select_Claims_Attachments_Files"; break;
                    case "claim_attachment_type": storedproc = "usp_Select_Claims_Attachments_DocTypes"; break;
                    case "get_otherpayerpaidamount": storedproc = "usp_Get_OtherPayerPaidAmount"; break;
                    case "selectadjustmentgroupsforheaderotherpayeronedit": storedproc = "usp_SelectAdjustmentGroupsForHeaderOtherPayerOnEdit"; break;
                    case "selectadjustmentgroupsforotherpayeradjustmentonedit": storedproc = "usp_SelectAdjustmentGroupsForOtherPayerAdjustmentOnEdit"; break;
                    case "get_otherpayerpaidamount_onedit": storedproc = "usp_Get_OtherPayerPaidAmountOnEdit"; break;

                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet ds = DataAccess.ExecuteStoredProcedure(storedproc, parameters, tableName);
                return ds;

            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName + " - " +
                    ex.Message + " - " + ex.StackTrace));
            }

        }
        // This select block is only for all four common panels.(referring,rendering, supervising, assistant surgeon)
        public static DataSet SelectDentalClaimData(int claimID, string tableName, string claimPanelName = "", int IsPrimary = 0)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (claimID > 0)
                    parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimID, true));
                if (claimPanelName != null)
                    parameters.Add(SqlParms.CreateParameter("Claims_Panel_Name", DbType.String, claimPanelName, true));
                if (claimPanelName != null)
                    parameters.Add(SqlParms.CreateParameter("Is_Primary", DbType.Int32, IsPrimary, true));

                string storedProc = "";
                switch (tableName.ToLower())
                {
                    case "claims_provider_information": storedProc = "usp_SelectClaims_Provider_Information"; break;
                    case "claims_service_details": storedProc = "usp_SelectClaims_Service_Details"; break;


                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, tableName);
                ds.Tables[0].TableName = "Table";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet SelectDentalServiceDetailsClaimData(int claimID, string tableName, string claimPanelName = "")
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (claimID > 0)
                    parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimID, true));
                if (claimPanelName != null)
                    parameters.Add(SqlParms.CreateParameter("Claims_Panel_Name", DbType.String, claimPanelName, true));

                string storedProc = "";
                switch (tableName.ToLower())
                {
                    case "claims_provider_information": storedProc = "usp_SelectClaims_Provider_Information"; break;
                    case "claims_service_details": storedProc = "usp_SelectClaims_Service_Details"; break;


                    default: throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure(storedProc, parameters, tableName);
                ds.Tables[0].TableName = "Table";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static void DeletePanelsData(string tableName, string idColumnName, int id)
        {
            try
            {
                string storedproc = "";
                switch (tableName.ToLower())
                {
                    case "claims_tooth_and_surface_information": storedproc = "deleteClaims_Tooth_and_Surface_Information"; break;
                    case "claims_other_payer_information": storedproc = "deleteclaims_other_payer_information"; break;
                    case "claims_other_payer_information_by_claimid": storedproc = "deleteClaims_Other_Payer_Information_by_ClaimId"; break;
                    case "claims_other_payer_adjudication_information_service_detail": storedproc = "delete_Claims_Other_Payer_Adjudication_Information_Service_Detail"; break;
                    case "claims_other_payer_adjudication_information_service_detail_by_claimid": storedproc = "delete_Claims_Other_Payer_Adjudication_Information_Service_Detail_by_ClaimId"; break;
                    case "claims_providers_note": storedproc = "Usp_DeleteClaim_Provider_Notes"; break;
                    case "claims_additional_provider_information_service": storedproc = "delete_Claims_Additional_Provider_Information_ServiceCustom"; break;
                    case "claims_header_other_payer_adjustment_information": storedproc = "deleteClaims_Header_Other_Payer_Adjustment_Information"; break;
                    case "claims_header_other_payer_adjustment_information_by_claimid": storedproc = "deleteClaims_Header_Other_Payer_Adjustment_Information_by_ClaimId"; break;
                    case "claims_diagnosis_information": storedproc = "delete_Claims_Diagnosis_Information_custom"; break;
                    case "claims_other_payer_adjustment_service_detail": storedproc = "delete_Claims_Other_Payer_Adjustment_Service_Detail"; break;
                    case "claims_other_payer_adjustment_service_detail_by_claimid": storedproc = "delete_Claims_Other_Payer_Adjustment_Service_Detail_by_ClaimId"; break;
                    case "claims_ambulance_pick_up_drop_off_location": storedproc = "usp_Delete_Claims_Ambulance_Pick_Up_Drop_Off_Location"; break;
                    case "claims_ndc_details": storedproc = "usp_delete_Claims_NDC_Details"; break;

                    case "usp_delete_claims_occurrence_information": storedproc = "usp_delete_Claims_Occurrence_Information"; break;

                    case "claims_icd_procedurecode": storedproc = "Usp_delete_Claims_ICD_Procedure_Code_Sequence"; break;
                    case "claims_occurrence_code_span_information": storedproc = "Usp_delete_Claims_Occurrence_Code_Span_Information"; break;
                    case "claims_condition_code_information": storedproc = "Usp_Delete_Claims_Condition_Code_Information"; break;
                    case "claims_value_code_information": storedproc = "Usp_delete_Claims_Value_Code_Information"; break;
                    case "claims_attachment_screen": storedproc = "usp_delete_Claims_Attachments"; break;


                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter(idColumnName, DbType.Int32, id, true));
                DataAccess.ExecuteStoredProcedure(storedproc, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName +
                    ", Id Column Name: " + idColumnName + " - " + ex.Message + " - " + ex.StackTrace));
            }
        }

        public static void DeletePanelsDataWithParams(string tableName, Dictionary<string, string> parms)
        {
            try
            {
                string storedproc = "";
                switch (tableName.ToLower())
                {
                    case "claims_service_details":
                        storedproc = "usp_deleteClaims_ServiceLine_Details";
                        break;
                    case "claim_other_payer_details":
                        storedproc = "deleteClaim_Other_Payers_by_ClaimIdAndHealthPlanId";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Invalid value provided for: tableName");
                }

                List<SqlParameter> parameters = new List<SqlParameter>();

                foreach (KeyValuePair<string, string> pair in parms)
                {
                    if (string.IsNullOrEmpty(pair.Value))
                    {
                        parameters.Add(new SqlParameter(pair.Key, DBNull.Value));
                    }
                    else
                    {
                        parameters.Add(new SqlParameter(pair.Key, pair.Value));
                    }
                }

                DataAccess.ExecuteStoredProcedure(storedproc, parameters);
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception("Table Name: " + tableName +
                    " - " + ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetToothServiceLine(int claimId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CLAIM_ID", DbType.Int32, claimId, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("usp_Select_Service_Number", parameters, "Claims_Tooth_and_Surface_Information");
                ds.Tables[0].TableName = "Claims_Tooth_and_Surface_Information";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetFilteredOtherayerSequence()
        {
            try
            {
                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectFilteredPayerSequence");
                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetConditionCodeDescription(string ConditionCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CLAIMS_CONDITION_CODE", DbType.String, ConditionCode, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("Usp_Get_CLAIMS_CONDITION_CODE", parameters, "CLAIMS_CONDITION_CODE");
                ds.Tables[0].TableName = "CLAIMS_CONDITION_CODE";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }


        public static DataSet GetValueCodeDescription(string ValueCode)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("CLAIMS_VALUE_CODE", DbType.String, ValueCode, true));
                DataSet ds = new DataSet();
                ds = DataAccess.ExecuteStoredProcedure("Usp_Get_CLAIMS_VALUE_CODE", parameters, "CLAIMS_VALUE_CODE");
                ds.Tables[0].TableName = "CLAIMS_VALUE_CODE";
                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static bool IsValidProcedureModifier(string ModifierCode)
        {
            bool retVal = false;
            try
            {

                DataTable dt = new DataTable();
                dt = LookupTableController.GetProcedureModifierDetail(ModifierCode, "");
                if (ObjectControllerHelper.HasRows(dt))
                {
                    retVal = true;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static bool IsValidProcedureCode(string Code)
        {
            bool retVal = false;
            try
            {

                DataTable dt = new DataTable();
                dt = LookupTableController.GetProcedureCodeServiceDetail(Code, string.Empty);
                if (ObjectControllerHelper.HasRows(dt))
                {
                    retVal = true;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static bool IsValidRevenueCode(string Code)
        {
            bool retVal = false;
            try
            {

                DataTable dt = new DataTable();
                dt = LookupTableController.GetRevenueCode(Code, string.Empty);
                if (ObjectControllerHelper.HasRows(dt))
                {
                    retVal = true;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static bool IsValidPlaceOfservice(string pos)
        {
            bool retVal = false;
            try
            {

                DataTable dt = new DataTable();
                dt = LookupTableController.GetPlaceofServiceDetail(pos, string.Empty);
                if (ObjectControllerHelper.HasRows(dt))
                {
                    retVal = true;
                }
                return retVal;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }

        }
        public static DataSet GetClaimsDentalDataDetails(int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Dentals_DataDetails", parameters, "Claims_Service_Details");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetClaimsProfDataDetails(int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Professional_DataDetails", parameters, "Claims_Service_Details");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetClaimsInstDataDetails(int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectClaims_Institutional_DataDetails", parameters, "Claims_Service_Details");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet CheckClaimIsDelet(int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Selectclaim_Adjudication_Summary_Status_ScreenData", parameters, "claim_Adjudication_Summary_Status_Screen");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet CheckDiagnosisCodeUse(String diagnosisCode, int claimid)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claimid, true));
                parameters.Add(SqlParms.CreateParameter("Diagnosis_Code", DbType.Int32, diagnosisCode, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectServiceDetail_Diagnosis", parameters, "Claims_Service_Details");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static string GetClaimID(string Medicaid_ID, string Provider_NPI, string Claim_Type, string ICN)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Medicaid_ID", DbType.String, Medicaid_ID, true));
                parameters.Add(SqlParms.CreateParameter("Provider_NPI", DbType.String, Provider_NPI, true));
                parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.String, Claim_Type, true));
                parameters.Add(SqlParms.CreateParameter("ICN", DbType.String, ICN, true));

                string claimid = null;

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_CliamId_Claim_Adjudication_Summary_Status_Screen", parameters, "Claim_Adjudication_Summary_Status_Screen");
                if (ObjectControllerHelper.HasRows(lookup))
                {
                    claimid = lookup.Tables[0].Rows[0]["claim_id"].ToString();
                }
                return claimid;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetMaliciousDocs(string claimId, string claimType, string claimTypeCode, string medicaidId)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("DOCUMENT_TYPE", DbType.String, claimTypeCode, true));
                parameters.Add(SqlParms.CreateParameter("PAClaimType_Type", DbType.String, claimType, true));
                parameters.Add(SqlParms.CreateParameter("ICN", DbType.String, claimId, true));
                parameters.Add(SqlParms.CreateParameter("MedicaidId", DbType.String, medicaidId, true));

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_MaliciousDocs_ByPAClaimType", parameters, "MaliciousAttachments");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet GetIndAmbulanceDropOffCode(string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claims_Ambulance_Pick_Up_Drop_Off_Location_ID", DbType.Int32, Claims_Ambulance_Pick_Up_Drop_Off_Location_ID, true));
               
                DataSet lookup = DataAccess.ExecuteStoredProcedure("Usp_Select_IND_Claims_Ambulance_Pick_Up_Drop_Off_Location", parameters, "Claims_Ambulance_Pick_Up_Drop_Off_Location");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
     
             public static DataSet GetIndProviderNote(string Claim_ID, string providerNoteID, string claim_type)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, true));
                parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.Int32, claim_type, true));
                parameters.Add(SqlParms.CreateParameter("Claim_providernote_id", DbType.Int32, providerNoteID, true));


                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectIndProviderNote_Code", parameters, "Claims_Providers_Note");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetIndDiagCode(string claim_id, string claim_diag_info_id, string claim_type)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
                parameters.Add(SqlParms.CreateParameter("Claim_Type", DbType.Int32, claim_type, true));
                parameters.Add(SqlParms.CreateParameter("Claim_diag_info_id", DbType.Int32, claim_diag_info_id, true));
                

                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_SelectIndDiagnosis_Code", parameters, "Claims_Diagnosis_Information");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet GetOccurrenceSpanInfo(string Claims_Occurrence_Code_Span_Information_ID, string Claim_ID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_Occurrence_Code_Span_Information_ID", DbType.Int32, Claims_Occurrence_Code_Span_Information_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Edit_Claims_Occurrence_Code_Span_Information", sqlParms, "claims_occurrence_code_span_information");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetValueCodeData(string Claims_Value_Code_Information_ID, string Claim_ID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_Value_Code_Information_ID", DbType.Int32, Claims_Value_Code_Information_ID, false));
             
                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Get_Claims_Value_Code_Information", sqlParms, "Claims_Value_Code_Information_ID");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
        public static DataSet GetConditionCodeData(string Claims_Condition_Code_Information_ID, string Claim_ID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_Condition_Code_Information_ID", DbType.Int32, Claims_Condition_Code_Information_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Get_Claims_Condition_Code_Information", sqlParms, "Claims_Condition_Code_Information_ID");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetICDProcCode(string claim_id, string claim_icd_procdeure_code)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
                parameters.Add(SqlParms.CreateParameter("claim_icd_procdeure_code", DbType.Int32, claim_icd_procdeure_code, true));


                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Select_Claims_ICD_Proc_Code", parameters, "Claims_Diagnosis_Information");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }
        public static DataSet EditNDCCodeData(string Claims_NDC_Details_Screen_ID, string ClaimId)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_NDC_Details_Screen_ID", DbType.Int32, Claims_NDC_Details_Screen_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("Usp_Edit_Claims_NDC_Details", sqlParms, "Claims_NDC_Details");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet EditOccurrenceInfoData(string Claims_Occurrence_Information_ID, string ClaimId)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_Occurrence_Information_ID", DbType.Int32, Claims_Occurrence_Information_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("Usp_Edit_Claims_Occurrence_Information", sqlParms, "Claims_Occurrence_Information");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet Get_ToothQuadrantInfoData(string claim_id, string Claims_Tooth_and_Surface_Information_ID)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, claim_id, true));
                parameters.Add(SqlParms.CreateParameter("Claims_Tooth_and_ToothSurface_Information_ID", DbType.Int32, Claims_Tooth_and_Surface_Information_ID, true));


                DataSet lookup = DataAccess.ExecuteStoredProcedure("usp_Get_Claims_Tooth_and_Surface_Information", parameters, "Claims_Tooth_and_Surface_Information");

                return lookup;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(new Exception(ex.Message + " - " + ex.StackTrace));
            }
        }

        public static DataSet EditOtherPayerAdjustmentInfoServiceDetails(string Other_Payer_Adjustment_Service_Detail_ID, string Claim_ID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, false));
                sqlParms.Add(SqlParms.CreateParameter("Other_Payer_Adjustment_Service_Detail_ID", DbType.Int32, Other_Payer_Adjustment_Service_Detail_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_EditClaims_Other_Payer_Adjustment_Service_Detail", sqlParms, "Claims_Other_Payer_Adjustment_Service_Detail");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet GetOPPAServiceDetail(string ClaimId, string OPPAServiceDetailId)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, ClaimId, false));
                sqlParms.Add(SqlParms.CreateParameter("OPPAServiceDetailId", DbType.Int32, OPPAServiceDetailId, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_SelectClaimsOPPAserviceDetail", sqlParms, "Claims_Other_Payer_Adjustment_Service_Detail");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }

        public static DataSet EditAdditionalProviderInformation(string Claim_ID, string Claims_Additional_Provider_Information_Service_ID)
        {
            try
            {
                List<SqlParameter> sqlParms = new List<SqlParameter>();
                sqlParms.Add(SqlParms.CreateParameter("Claim_ID", DbType.Int32, Claim_ID, false));
                sqlParms.Add(SqlParms.CreateParameter("Claims_Additional_Provider_Information_Service_ID", DbType.Int32, Claims_Additional_Provider_Information_Service_ID, false));

                DataSet ds = DataAccess.ExecuteStoredProcedure("usp_Edit_Claims_Additional_Provider_Information_Service", sqlParms, "claims_additional_provider_information_service");

                return ds;
            }
            catch (Exception ex)
            {
                throw CoreException.ThrowException(ex);
            }
        }
    }
    #endregion
   

}


