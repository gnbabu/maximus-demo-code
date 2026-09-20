using System;
using System.Collections.Generic;

namespace Corp.Core.Libraries
{
    [Serializable]
    public class OtherPayerInformation
    {
        public OtherPayerInformation()
        {
            claimsAdjustments = new List<ClaimsAdjustment>();
        }
        public long OtherPayerSeq { get; set; }
        public string named_insured_group { get; set; }
        public string cde_insured_group_number { get; set; }
        public string nam_first { get; set; }
        public string nam_last { get; set; }
        public string employer_name { get; set; }
        public string cde_ind_relationship { get; set; }
        public string relationshipText { get; set; }
        public string cde_claim_filing_ind { get; set; }
        public string claimFilingText { get; set; }
        public string cde_payer_responib { get; set; }
        public string payerResponibText { get; set; }
        public string dte_clm_adjudication { get; set; }
        public string amt_monetary { get; set; }
        public List<ClaimsAdjustment> claimsAdjustments { get; set; }
    }


    [Serializable]
    public class AdditionalProvider
    {
        public string seqNo { get; set; }
        public string num_dtl { get; set; }
        public string cde_provider_type { get; set; }
        public string cde_party_id { get; set; }
        public string nam_last { get; set; }
        public string nam_first { get; set; }
        public string nam_middle { get; set; }

    }

    [Serializable]
    public class OtherPayerPaidAmount
    {
        public OtherPayerPaidAmount()
        {
            claimsAdjustments = new List<ClaimsAdjustment>();
        }
        public string OtherPayerPaidId { get; set; }
        public string num_dtl { get; set; }
        public string cde_proc { get; set; }
        public string cde_insured_group_number { get; set; }
        public string amt_paid { get; set; }
        public string paid_date { get; set; }
        public string amt_due { get; set; }
        public List<ClaimsAdjustment> claimsAdjustments { get; set; }
    }

    [Serializable]
    public class Attachment
    {
       
        public long LineNumber { get; set; }
        public string DocumentId { get; set; }
        public bool Mailed { get; set; }
        public string ControlNumber { get; set; }
        public string DocumentType { get; set; }
        public string Note { get; set; }
    }

    [Serializable]
    public class ProviderNote
    {

        public long num_dtl { get; set; }
        public string dsc_note { get; set; }
        public string Claims_Providers_Note_ID { get; set; }
    }
    [Serializable]
    public class OtherPayerAdjustmentInfo
    {
        public string cde_health_plan_id { get; set; }
        public string cde_adjustment_group { get; set; }
        public string cde_reason_code { get; set; }
        public string cde_amount { get; set; }
        public string cde_quantity { get; set; }
    }


}
