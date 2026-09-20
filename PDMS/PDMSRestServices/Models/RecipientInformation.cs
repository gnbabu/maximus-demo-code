using Corp.Core.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PDMSRestServices.Models
{
    public class MemberEligibilityPageRequest
    {
        public string? MEpid { get; set; }
        public string? MEmbn { get; set; }
        public string? MEssn { get; set; }
        public string? MEdob { get; set; }
        public DateTime? MEfdos { get; set; }
        public DateTime? MEtdos { get; set; }
        public string? MEpc { get; set; }
        public string? MErt { get; set; }
        public string? MEun { get; set; }
        public string? MEsdt { get; set; }
    }
    public class MemberEligibilityPageRequestPDF
    {
        public string? MEpid { get; set; }
        public string? MEmbn { get; set; }
        public string? MEssn { get; set; }
        public string? MEdob { get; set; }
        public string? MEfdos { get; set; }
        public string? MEtdos { get; set; }
        public string? MEpc { get; set; }
        public string? MErt { get; set; }
        public string? MEun { get; set; }
        public string MEsdt { get; set; }
    }
    public class RecipientInformationPDF
    {
        public string? MedicaidId { get; set; }
        public string? DateOfBirth { get; set; }
        public string? DateOfDeath { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? SSN { get; set; }
        public string? Gender { get; set; }
        public string? CountyOfResidence { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? City { get; set; }
        public string? StateCode { get; set; }
        public string? ZipCode5 { get; set; }
        public string? CountyOfEligibility { get; set; }
        public string? Errors { get; set; }
        public string? ErrorMsg { get; set; }
    }
    public class RecipientInformationV2
    {
        public string MedicaidId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public string CountyOfResidence { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode5 { get; set; }
        public string CountyOfEligibility { get; set; }
        public List<ErrorDetail> Errors { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class RecipientInformation
    {
        public string MedicaidId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDeath { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string SSN { get; set; }
        public string Gender { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string ZipCode5 { get; set; }
        public List<ErrorDetail> Errors { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class ErrorDetail
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class RecipientEligibilitySearchResponseV2
    {
        public ResponseHeader ResponseHeaderDetails { get; set; }
        public RecipientInformationV2 RecipientInfo { get; set; }
        public List<BenifitAssignmentPlan> BenifitAssignmentPlans { get; set; }
        public List<ManagedCarePlan> ManagedCarePlans { get; set; }
        public List<ThirdPartyLiabilityV2> ThirdPartyLiabilities { get; set; }
        public List<PatientLiability> PatientLiabilities { get; set; }
        public List<LTCFPlacement> LTCFPlacements { get; set; }
        public List<Lockin> Lockins { get; set; }
        public List<MedicareCoverageDetail> MedicareCoverageDetails { get; set; }
        public List<SNIFlOCDetail> SNIFlOCDetails { get; set; }
        public List<ServiceLimitation> ServiceLimitations { get; set; }
        public List<RestrictedCoverage> RestrictedCoverages { get; set; }
        public List<Under19FamilyMember> Under19FamilyMembers { get; set; }
        public List<ErrorDetail> ErrorDetails { get; set; }
    }

    public class MemberEligibilityPDFRequest
    {
        public MemberEligibilityPageRequestPDF RecipientSearchParameters { get; set; }
        public RecipientInformationPDF RecipientInfo { get; set; }
        public List<BenifitAssignmentPlanPDF> BenifitAssignmentPlans { get; set; }
        public List<ManagedCarePlanPDF> ManagedCarePlans { get; set; }
        public List<ThirdPartyLiabilityPDF> ThirdPartyLiabilities { get; set; }
        public List<PatientLiabilityPDF> PatientLiabilities { get; set; }
        public List<LTCFPlacementPDF> LTCFPlacements { get; set; }
        public List<LockinPDF> Lockins { get; set; }
        public List<MedicareCoverageDetailPDF> MedicareCoverageDetails { get; set; }
        public List<ServiceLimitationPDF> ServiceLimitations { get; set; }
        public List<RestrictedCoveragePDF> RestrictedCoverages { get; set; }
        public List<Under19FamilyMemberPDF> Under19FamilyMembers { get; set; }
    }

    public class RecipientEligibilitySearchResponse
    {
        public ResponseHeader ResponseHeaderDetails { get; set; }
        public RecipientInformation RecipientInfo { get; set; }
        public List<BenifitAssignmentPlan> BenifitAssignmentPlans { get; set; }
        public List<ManagedCarePlan> ManagedCarePlans { get; set; }
        public List<ThirdPartyLiability> ThirdPartyLiabilities { get; set; }
        public List<PatientLiability> PatientLiabilities { get; set; }
        public List<LTCFPlacement> LTCFPlacements { get; set; }
        public List<Lockin> Lockins { get; set; }
        public List<MedicareCoverageDetail> MedicareCoverageDetails { get; set; }
        public List<SNIFlOCDetail> SNIFlOCDetails { get; set; }
        public List<ServiceLimitation> ServiceLimitations { get; set; }
        public List<RestrictedCoverage> RestrictedCoverages { get; set; }
        public List<Under19FamilyMember> Under19FamilyMembers { get; set; }
        public List<ErrorDetail> ErrorDetails { get; set; }
    }

    public class ResponseHeader
    {
        public string SITransactionKey { get; set; }
        public string ModuleTransactionId { get; set; }
        public string AdditionalModuleTransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseType { get; set; }
        public string ResponseMessage { get; set; }
        public string ResponseDetails { get; set; }
    }

    public class BenifitAssignmentPlan
    {
        public string AssignmentPlan { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class BenifitAssignmentPlanPDF
    {
        public string AssignmentPlan { get; set; }

        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class ManagedCarePlan
    {
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string PlanDescription { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ManagedCareBenefits { get; set; }
    }

    public class ManagedCarePlanPDF
    {
        public string? PlanId { get; set; }
        public string? PlanName { get; set; }
        public string? PlanDescription { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
        public string? ManagedCareBenefits { get; set; }
    }
    public class ThirdPartyLiabilityV2
    {
        public string? CarrierName { get; set; }
        public string? CarrierNumber { get; set; }
        public string? NAIC { get; set; }
        public string? PolicyNumber { get; set; }
        public string? PolicyHolder { get; set; }
        public string? CoverageType { get; set; }
        public string? Coverage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? GroupNumber { get; set; }
    }
    public class ThirdPartyLiabilityPDF
    {
        public string? CarrierName { get; set; }
        public string? CarrierNumber { get; set; }
        public string? NAIC { get; set; }
        public string? PolicyNumber { get; set; }
        public string? PolicyHolder { get; set; }
        public string? CoverageType { get; set; }
        public string? Coverage { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
        public string? GroupNumber { get; set; }
    }

    public class ThirdPartyLiability
    {
        public string CarrierName { get; set; }
        public string CarrierNumber { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyHolder { get; set; }
        public string CoverageType { get; set; }
        public string? Coverage { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string GroupNumber { get; set; }
    }

    public class PatientLiability
    {
        public string FinancialPayer { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public string? Type { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class PatientLiabilityPDF
    {
        public string FinancialPayer { get; set; }
        public decimal? MonthlyAmount { get; set; }
        public string? Type { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class LTCFPlacement
    {
        public string FacilityType { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? EffectiveDateMedicaidCoverage { get; set; }
        public DateTime? EndDateMedicaidCoverage { get; set; }
    }

    public class LTCFPlacementPDF
    {
        public string FacilityType { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
        public string? EffectiveDateMedicaidCoverage { get; set; }
        public string? EndDateMedicaidCoverage { get; set; }
    }

    public class Lockin
    {
        public string LockinPlan { get; set; }
        public string LockinType { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderName { get; set; }
        public string? ProviderPhoneNumber { get; set; }
    }

    public class LockinPDF
    {
        public string LockinPlan { get; set; }
        public string LockinType { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
        public string ProviderNPI { get; set; }
        public string ProviderName { get; set; }
        public string? ProviderPhoneNumber { get; set; }
    }

    public class MedicareCoverageDetail
    {
        public string Coverage { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string MedicareId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class MedicareCoverageDetailPDF
    {
        public string Coverage { get; set; }
        public string PlanId { get; set; }
        public string PlanName { get; set; }
        public string MedicareId { get; set; }
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class SNIFlOCDetail
    {
        public string FacilityType { get; set; }
        public string Status { get; set; }
        public DateTime? DeterminationDate { get; set; }
        public string LOCDetermination { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ServiceLimitation
    {
        public string? ProcedureCode { get; set; }
        public string? ServiceLimitDescription { get; set; }
        public string? BenefitDescription { get; set; }
        public int? TotalLimits { get; set; }
        public int? UsedLimits { get; set; }
        public int? RemainingLimits { get; set; }
        public string? Timeframe { get; set; }
        public DateTime? DateOfNextService { get; set; }
    }

    public class ServiceLimitationPDF
    {
        public string? ProcedureCode { get; set; }
        public string? ServiceLimitDescription { get; set; }
        public string? BenefitDescription { get; set; }
        public int? TotalLimits { get; set; }
        public int? UsedLimits { get; set; }
        public int? RemainingLimits { get; set; }
        public string? Timeframe { get; set; }
        public string? DateOfNextService { get; set; }
    }

    public class RestrictedCoverage
    {
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class RestrictedCoveragePDF
    {
        public string? EffectiveDate { get; set; }
        public string? EndDate { get; set; }
    }

    public class Under19FamilyMember
    {
        public string? MedicaidId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
    }

    public class Under19FamilyMemberPDF
    {
        public string? MedicaidId { get; set; }
        public string? DateOfBirth { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string? Gender { get; set; }
    }

}