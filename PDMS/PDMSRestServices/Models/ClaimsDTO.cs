using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class valuecodedetails
    {
        public valuecodedetails(string value_code, string value_desc)
        {
            this.Value_Code = value_code;
            this.Value_Desc = value_desc;
        }
        private string _valuecode;
        private string _valuedesc;
        public string Value_Code
        {
            get { return _valuecode; }
            set { _valuecode = value; }
        }
        public string Value_Desc
        {
            get { return _valuedesc; }
            set { _valuedesc = value; }
        }
    }

    public class Revencode
    {
        public Revencode(string reven_code, string reven_desc)
        {
            this.Reven_Code = reven_code;
            this.Reven_Desc = reven_desc;
        }
        private string _revencode;
        private string _revendesc;
        public string Reven_Code
        {
            get { return _revencode; }
            set { _revencode = value; }
        }
        public string Reven_Desc
        {
            get { return _revendesc; }
            set { _revendesc = value; }
        }
    }

    public class RegSpecialtyType
    {
        public RegSpecialtyType(int regId, int regSpecialtyId, int specialtyTypeId, string mmisSpecialtyTypeId)
        {
            this.RegId = regId;
            this.RegSpecialtyId = regSpecialtyId;
            this.SpecialtyTypeId = specialtyTypeId;
            this.MmisSpecialtyTypeId = mmisSpecialtyTypeId;
        }
        private int _regId;
        private int _regSpecialtyId;
        private int _specialtyTypeId;
        private string _mmisSpecialtyTypeId;
        public int RegId
        {
            get { return _regId; }
            set { _regId = value; }
        }
        public int RegSpecialtyId
        {
            get { return _regSpecialtyId; }
            set { _regSpecialtyId = value; }
        }
        public int SpecialtyTypeId
        {
            get { return _specialtyTypeId; }
            set { _specialtyTypeId = value; }
        }
        public string MmisSpecialtyTypeId
        {
            get { return _mmisSpecialtyTypeId; }
            set { _mmisSpecialtyTypeId = value; }
        }
    }

    public class CodesetRule
    {
        public CodesetRule(string allowed, string errorMessage)
        {
            this.Allowed = allowed;
            this.ErrorMessage = errorMessage;
        }
        private string _allowed;
        private string _errorMessage;
        public string Allowed
        {
            get { return _allowed; }
            set { _allowed = value; }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
    }

    public class Proccode
    {
        public Proccode(string proc_code, string proc_desc)
        {
            this.Proc_Code = proc_code;
            this.Proc_Desc = proc_desc;
        }
        private string _proccode;
        private string _procdesc;
        public string Proc_Code
        {
            get { return _proccode; }
            set { _proccode = value; }
        }
        public string Proc_Desc
        {
            get { return _procdesc; }
            set { _procdesc = value; }
        }
    }

    public class TypeOfBill
    {
        public TypeOfBill(string typeOfBill_code, string typeOfBill_desc)
        {
            this.TypeOfBill_Code = typeOfBill_code;
            this.TypeOfBill_Desc = typeOfBill_desc;
        }
        private string _typeOfBillcode;
        private string _typeOfBilldesc;
        public string TypeOfBill_Code
        {
            get { return _typeOfBillcode; }
            set { _typeOfBillcode = value; }
        }
        public string TypeOfBill_Desc
        {
            get { return _typeOfBilldesc; }
            set { _typeOfBilldesc = value; }
        }
    }


    public class Reasoncode
    {
        public Reasoncode(string reason_code, string reason_desc)
        {
            this.Reason_Code = reason_code;
            this.Reason_Desc = reason_desc;
        }
        private string _reasoncode;
        private string _reasondesc;
        public string Reason_Code
        {
            get { return _reasoncode; }
            set { _reasoncode = value; }
        }
        public string Reason_Desc
        {
            get { return _reasondesc; }
            set { _reasondesc = value; }
        }
    }

    public class conditioncodedetails
    {
        public conditioncodedetails(string condition_code, string condition_desc)
        {
            this.condition_Code = condition_code;
            this.condition_Desc = condition_desc;
        }
        private string _conditioncode;
        private string _conditiondesc;
        public string condition_Code
        {
            get { return _conditioncode; }
            set { _conditioncode = value; }
        }
        public string condition_Desc
        {
            get { return _conditiondesc; }
            set { _conditiondesc = value; }
        }
    }

    public class occerencecodedetails
    {
        public occerencecodedetails(string occurence_code, string occurence_desc)
        {
            this.occurence_Code = occurence_code;
            this.occurence_Desc = occurence_desc;
        }
        private string _occurencecode;
        private string _occurencedesc;
        public string occurence_Code
        {
            get { return _occurencecode; }
            set { _occurencecode = value; }
        }
        public string occurence_Desc
        {
            get { return _occurencedesc; }
            set { _occurencedesc = value; }
        }
    }

    public class occerencedetails
    {
        public occerencedetails(string occur_code, string occur_desc)
        {
            this.occur_Code = occur_code;
            this.occur_Desc = occur_desc;
        }
        private string _occurcode;
        private string _occurdesc;
        public string occur_Code
        {
            get { return _occurcode; }
            set { _occurcode = value; }
        }
        public string occur_Desc
        {
            get { return _occurdesc; }
            set { _occurdesc = value; }
        }
    }

    public class IcdProccodedetails
    {
        public IcdProccodedetails(string ICD_code, string ICD_desc, string ICD_version)
        {
            this.ICD_Code = ICD_code;
            this.ICD_Desc = ICD_desc;
            this.ICD_Version = ICD_version;
        }
        private string ICD_code;
        private string ICD_desc;
        private string ICD_version;
        public string ICD_Code
        {
            get { return ICD_code; }
            set { ICD_code = value; }
        }
        public string ICD_Desc
        {
            get { return ICD_desc; }
            set { ICD_desc = value; }
        }

        public string ICD_Version
        {
            get { return ICD_version; }
            set { ICD_version = value; }
        }
    }

    public class NPIcodedetails
    {
        public NPIcodedetails(string npi, string Medicateid, string firstname, string lastname, string AddressLine1, string AddressLine2, string City, string State, string zip)
        {

            this.NPI_Npi = npi;
            this.NPI_Medicateid = Medicateid;
            this.NPI_Lastname = lastname;
            this.NPI_Firstname = firstname;
            this.NPI_AddressLine1 = AddressLine1;
            this.NPI_AddressLine2 = AddressLine2;
            this.NPI_City = City;
            this.NPI_State = State;
            this.NPI_Zip = zip;
        }
        private string NPI_npi;
        private string NPI_medicateid;
        private string NPI_lastname;
        private string NPI_firstname;
        private string NPI_addressLine1;
        private string NPI_addressLine2;
        private string NPI_city;
        private string NPI_state;
        private string NPI_zip;
        public string NPI_Npi
        {
            get { return NPI_npi; }
            set { NPI_npi = value; }
        }
        public string NPI_Medicateid
        {
            get { return NPI_medicateid; }
            set { NPI_medicateid = value; }
        }

        public string NPI_Lastname
        {
            get { return NPI_lastname; }
            set { NPI_lastname = value; }
        }
        public string NPI_Firstname
        {
            get { return NPI_firstname; }
            set { NPI_firstname = value; }
        }
        public string NPI_AddressLine1
        {
            get { return NPI_addressLine1; }
            set { NPI_addressLine1 = value; }
        }
        public string NPI_AddressLine2
        {
            get { return NPI_addressLine2; }
            set { NPI_addressLine2 = value; }
        }

        public string NPI_City
        {
            get { return NPI_city; }
            set { NPI_city = value; }
        }
        public string NPI_State
        {
            get { return NPI_state; }
            set { NPI_state = value; }
        }
        public string NPI_Zip
        {
            get { return NPI_zip; }
            set { NPI_zip = value; }
        }
    }
    public class Diagnosiscodedetails
    {
        public Diagnosiscodedetails(string Diag_code, string Diag_version, string Diag_desc)
        {
            this.Diag_Code = Diag_code;
            this.Diag_Desc = Diag_desc;
            this.Diag_Version = Diag_version;
        }
        private string Diag_code;
        private string Diag_desc;
        private string Diag_version;
        public string Diag_Code
        {
            get { return Diag_code; }
            set { Diag_code = value; }
        }
        public string Diag_Desc
        {
            get { return Diag_desc; }
            set { Diag_desc = value; }
        }

        public string Diag_Version
        {
            get { return Diag_version; }
            set { Diag_version = value; }
        }
    }
    [Serializable]
    public class OtherPayerAdjustmentInfo
    {
        public string cde_health_plan_id { get; set; }
        public string cde_adjustment_group { get; set; }
        public string cde_reason_code { get; set; }
        public string cde_amount { get; set; }
        public string cde_quantity { get; set; }
        public string Claim_ID { get; set; }
        public string Claims_Header_Other_Payer_Adjustment_Information_ID { get; set; }
        public string lblErrorMessageHeaderOtherAdjustment { get; set; }
    }
    public class DiagnosisPanel
    {
        public string sequence { get; set; }
        public string DiagnosisCode { get; set; }
        public string ICDVersion { get; set; }
        public string PreasentOnAdmission { get; set; }
        public string DianosisCodeDescription { get; set; }
        public string Claims_Diagnosis_ID { get; set; }
        public string Claim_ID { get; set; }
        public string lblErrorMessageDiagnosis { get; set; }

    }

    public class providerNotes
    {
        public string claimID { get; set; }
        public string providerNoteID { get; set; }
        public string notes { get; set; }
        public string noteRefCode { get; set; }

    }

    public class AmbulanceDropOffPanel
    {
        public string serviceLine { get; set; }
        public string pickUpAddressLine1 { get; set; }
        public string pickUpAddressLine2 { get; set; }
        public string pickUpCity { get; set; }
        public string pickUpState { get; set; }
        public string pickUpZip { get; set; }
        public string dropOffLocationName { get; set; }
        public string dropOffLocationAddressline1 { get; set; }
        public string dropOffLocationAddressline2 { get; set; }
        public string dropOffLocationCity { get; set; }
        public string dropOffLocationState { get; set; }
        public string dropOffLocationZip { get; set; }
        public string Claims_Ambulance_Pick_Up_Drop_Off_Location_ID { get; set; }
        public string Claim_ID { get; set; }
        public string Error_Message { get; set; }
    }

    public class DentalServiceDetail
    {
        public string total_amount_billed { get; set; }
        public string total_amount_paid { get; set; }
        public string total_charges { get; set; }
        public string paid_amount { get; set; }
        public string num_dtl_total { get; set; }
        public string revenue_Code { get; set; }
        public string procedure_type { get; set; }
        public string cde_proc { get; set; }
        public string status { get; set; }
        public string plc_service { get; set; }
        public string Claim_service_Id { get; set; }
        public string Claim_Id { get; set; }
        public string ServiceInfo_DOS { get; set; }
        public string qty_billed { get; set; }
        public string qty_allowed { get; set; }
        public string dte_first_svc { get; set; }
        //public string dte_last_svc { get; set; }
        public string final_EAPG { get; set; }
        public string payment_Action { get; set; }
        public string non_Covered_Charges { get; set; }
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
        public string unit { get; set; }
        public string unit_of_measurement { get; set; }

        public string service_Line { get; set; }
        public DateTime? ServiceDate
        {
            get; set;

        }
        public DateTime? fromDOS
        {
            get; set;

        }



        public DateTime? toDOS
        {
            get; set;

        }
    }


    public class OtherPayerDetail
    {
        public string claimid { get; set; }
        public string otherpayerInfoID { get; set; }
        public string otherPayerName { get; set; }
        public string healthPlanID { get; set; }
        public string claimFilingIndicator { get; set; }
        public string payerResponsibilitySequence { get; set; }
        public string subscriberNumber { get; set; }
        public string policyNumber { get; set; }
        public string groupName { get; set; }
        public string insuranceTypeCode { get; set; }
        public string patientRelationshipSuscriber { get; set; }
        public string subscriberFirstName { get; set; }
        public string subscriberLastName { get; set; }
        public string subscriberMiddleName { get; set; }
        public string subscriberAddressLine1 { get; set; }
        public string subscriberAddressLine2 { get; set; }
        public string subscriberCity { get; set; }
        //public string dte_last_svc { get; set; }
        public string subscriberState { get; set; }
        public string subscriberZip { get; set; }
        public string claimAdjudicationLevel { get; set; }
        public string claimNumber { get; set; }
        public string paidDate { get; set; }
        public string paidAmount { get; set; }
        public string nonCoveredAmoount { get; set; }

    }

    //[Serializable]
    public class ProfessionalServiceDetail
    {

        public string num_dtl_total { get; set; }
        public string service_line { get; set; }
        public string cde_proc { get; set; }
        public string plc_service { get; set; }
        public string Claim_service_Id { get; set; }
        public string Claim_Id { get; set; }

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
        public string pad_amnt { get; set; }
        public string bil_unt { get; set; }
        public string pad_unt { get; set; }
        public string unt_of_measurment { get; set; }
        public string dme_cert_type { get; set; }
        public string dme_duration { get; set; }
        public string prior_auth { get; set; }
        public string ref_num { get; set; }
        public string Line_Control_Number { get; set; }
        public DateTime? ServiceDate
        {
            get; set;

        }
        public string Referred_EPSDT_Service { get; set; }
        public string Family_Planning { get; set; }
        public string Emergency { get; set; }
        public string Final_EAPG { get; set; }
        public string Payment_Action { get; set; }
        public string Paid_Amount { get; set; }
        public string Total_Charges { get; set; }
        public string Cert_Revision { get; set; }

    }
    public class revenueDetails
    {
        public revenueDetails(string revenue_code, string revenue_desc)
        {
            this.RevenueCode = revenue_code;
            this.RevenueDesc = revenue_desc;
        }
        private string _revenuecode;
        private string _revenuedesc;
        public string RevenueCode
        {
            get { return _revenuecode; }
            set { _revenuecode = value; }
        }
        public string RevenueDesc
        {
            get { return _revenuedesc; }
            set { _revenuedesc = value; }
        }
    }

    public class SearchClaimDropdowns
    {
        public List<ClaimStaus> ClaimStaus { get; set; }

        public List<DestinationPayer> Payers { get; set; }

        public List<PageSize> PageSizes { get; set; }

        public List<ClaimType> ClaimTypes { get; set; }
    }

    public class ClaimStaus
    {
        public string StatusType { get; set; }

        public string StatusId { get; set; }
    }
    public class DestinationPayer
    {
        public string PayerDesc { get; set; }

        public string PayerId { get; set; }
    }

    public class PageSize
    {
        public string Value { get; set; }

        public string Size { get; set; }
    }

    public class ClaimType
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

    public class OPPAServiceDetail
    {
        public string ClaimId { get; set; }
        public string OPPAServiceDetailId { get; set; }
        public string Service_Line { get; set; }
        public string Procedure_Code { get; set; }
        public string Health_Plan_ID { get; set; }
        public string Amount_Paid { get; set; }
        public string Paid_Date { get; set; }
        public string Paid_unit_Count { get; set; }
        public string Revenue_Code { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class AdditionalProviderInformation
    {
        public string ServiceLine { get; set; }
        public string ProviderType { get; set; }
        public string ProviderNPI { get; set; }
        public string MedicaidID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string ClaimID { get; set; }
        public string Claims_Additional_Provider_Information_Service_ID { get; set; }
        public string Error_Msg { get; set; }

    }

    public class NDCDetails
    {
        public string ServiceLine { get; set; }
        public string NDCCode { get; set; }
        public string TotalUnit { get; set; }
        public string UnitMeasure { get; set; }
        public string PrescriptionNuber { get; set; }
        public string Claims_NDC_Details_Screen_ID { get; set; }
        public string Claim_ID { get; set; }
    }

    public class OtherPayerAdjustmentInfoServiceDetail
    {
        public string Service_Line { get; set; }
        public string Procedure_Code { get; set; }
        public string Health_Plan_ID { get; set; }
        public string Adjustment_Group { get; set; }
        public string Reason_Code { get; set; }
        public string Amount { get; set; }
        public string Quantity { get; set; }
        public string Claim_ID { get; set; }
        public string ClaimType { get; set; }
        public string Revenue_Code { get; set; }
        public string Other_Payer_Adjustment_Service_Detail_ID { get; set; }
        public string Error_Message { get; set; }

    }

    public class ValueCodeDetail
    {
        public string Line { get; set; }
        public string Value_Code { get; set; }
        public string Amount { get; set; }
        public string ValueCodeDesc { get; set; }
        public string ClaimID { get; set; }
        public string hdnValue_Code { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class OccurrenceSpanInfo
    {
        public string Line { get; set; }
        public string OccurrenceSpanCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string OccurrenceDesc { get; set; }
        public string Claims_Occurrence_Code_Span_Information_ID { get; set; }
        public string Claim_ID { get; set; }
        public string Error { get; set; }
    }

    public class ServiceLine
    {
        public string Service_Line { get; set; }

    }

    public class ICDProcedureCode
    {
        public string Sequence { get; set; }
        public string IcdProcedureCode { get; set; }
        public string ICDVersion { get; set; }
        public string Date { get; set; }
        public string IcdProcedureCodeDescription { get; set; }
        public string Perior_Auth_Claim_Sequence_Desc { get; set; }
        public string Claims_ICD_Procedure_Code_Sequence_ID { get; set; }
        public string Claim_ID { get; set; }
        public string SequeneCount_Principal { get; set; }
        public string SequeneCount_Other { get; set; }

    }

    public class ConditionCodeDetail
    {
        public string Line { get; set; }
        public string ConditionCode { get; set; }
        public string ConditionCodeDesc { get; set; }
        public string ClaimID { get; set; }
        public string hdnConditionCode { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class ToothQuadrantInfo
    {
        public string Claims_Tooth_and_Surface_Information_ID { get; set; }
        public string hdnToothPanelId { get; set; }
        public string ToothNumber { get; set; }
        public string hdnClaimId { get; set; }
        public string ToothServiceLine { get; set; }
        public string ToothSurface1 { get; set; }
        public string ToothSurface2 { get; set; }
        public string ToothSurface3 { get; set; }
        public string ToothSurface4 { get; set; }
        public string ToothSurface5 { get; set; }
        public int ToothQuadrantInfoCount { get; set; }


    }

    public class OccurenceInfo
    {
        public string Line { get; set; }
        public string OccurrenceInfoCode { get; set; }
        public string OccurrenceDate { get; set; }

        public string OccurrenceDesc { get; set; }
        public string ClaimsOccurrenceInformationID { get; set; }
        public string ClaimId { get; set; }
        public string Error { get; set; }
    }

    public class ToothSurfaceList
    {
        public string TOOTHSURFACE_ID { get; set; }
        public string TOOTHSURFACE { get; set; }
    }

    public class HealthPlanIDOPPA
    {
        public string Health_Plan_ID { get; set; }
        public string Claims_Other_Payer_Information_ID { get; set; }
    }
    public class ServiceLineOPPA
    {
        public string Service_Line { get; set; }
        public string Claims_Service_Details_ID { get; set; }
    }

    public class NDCServiceLine
    {
        public string Service_Line { get; set; }
        public string Claims_Service_Details_ID { get; set; }

    }
    public class ProviderTypes
    {
        public ProviderTypes(string ID, string Desc)
        {
            this.PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID = ID;
            this.PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC = Desc;
        }
        private string PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID;
        private string PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC;
        public string ID
        {
            get { return PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID; }
            set { PRIOR_AUTH_CLAIM_PROVIDERTYPE_ID = value; }
        }
        public string Desc
        {
            get { return PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC; }
            set { PRIOR_AUTH_CLAIM_PROVIDERTYPE_DESC = value; }
        }
    }

    public class ClaimFilingIndicator
    {
        public ClaimFilingIndicator(string ID, string Desc)
        {
            this.PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE = ID;
            this.PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC = Desc;
        }
        private string PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE;
        private string PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC;
        public string ID
        {
            get { return PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE; }
            set { PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_CODE = value; }
        }
        public string Desc
        {
            get { return PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC; }
            set { PRIOR_AUTH_SUBMIT_CLAIM_CLAIMFILINGINDICATOR_DESC = value; }
        }
    }

    public class NDCCode
    {
        public string CLAIMS_NDC_CODE { get; set; }
        public string LAY_DESC { get; set; }
    }

    public class NPIData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MedicaidId { get; set; }
        public string Errormessage { get; set; }
    }
}