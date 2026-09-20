using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAXIMUS.DataExchange.PDMS.AutomatedReports
{
    public class AutoReportsModel
    {
    }

    public class TransactionQueueDetails
    {
        public List<TransactionQueue> TransactionQueue { get; set; }
    }

    public class TransactionAnalysis
    {
        public List<CLAIMS_PA_SUMMARY> CLAIMS_PA_SUMMARY { get; set; }
        public List<CLAIMS_PA_ERROR_DETAILS> CLAIMS_PA_ERROR_DETAILS { get; set; }
        public List<ELIGIBILITY_SUMMARY> ELIGIBILITY_SUMMARY { get; set; }
        public List<ELIGIBILITY_ERROR_DETAILS> ELIGIBILITY_ERROR_DETAILS { get; set; }
        public List<HOSPICE_SUMMARY> HOSPICE_SUMMARY { get; set; }
        public List<HOSPICE_ERROR_DETAILS> HOSPICE_ERROR_DETAILS { get; set; }
        public List<PROV_FINC_SUMMARY> PROV_FINC_SUMMARY { get; set; }
        public List<PROV_FINC_ERROR_DETAILS> PROV_FINC_ERROR_DETAILS { get; set; }
        public List<EVV_SUMMARY> EVV_SUMMARY { get; set; }
        public List<PROV_MGMT_SUMMARY> PROV_MGMT_SUMMARY { get; set; }
        public List<PROV_MGMT_ERROR_DETAILS> PROV_MGMT_ERROR_DETAILS { get; set; }
        public List<ATTACHMENT_CLAIMS_SUMMARY> ATTACHMENT_CLAIMS_SUMMARY { get; set; }
        public List<ATTACHMENT_PA_SUMMARY> ATTACHMENT_PA_SUMMARY { get; set; }
    }

    public class CLAIMS_PA_SUMMARY
    {
        public string RequestType { get; set; }
        public int SuccessCount { get; set; }
        public int BusinessFailureCount { get; set; }
        public int TechnicalFailureCount { get; set; }
        public DateTime QueryRunDate { get; set; }
        public string Blank { get; set; }
    }

    public class CLAIMS_PA_ERROR_DETAILS
    {
        public string TransactionType { get; set; }
        public string SOAPBodyErrorCode { get; set; }
        public string SOAPBodyErrorMsg { get; set; }
        public string SOAPHdrRespCode { get; set; }
        public string SOAPHdrRespMsg { get; set; }
        public int FailureCount { get; set; }
        public DateTime QueryRunDate { get; set; }
        public string FailureCountConcat { get; set; }
    }

    public class ELIGIBILITY_SUMMARY
    {
        public string EligibilityRequestType { get; set; }
        public int EligibilitySuccessCount { get; set; }
        public int EligibilityBusinessFailureCount { get; set; }
        public int EligibilityTechnicalFailureCount { get; set; }
        public DateTime EligibilityQueryRunDate { get; set; }
        public string Blank { get; set; }
    }

    public class ELIGIBILITY_ERROR_DETAILS
    {
        public string EligibilityTransactionType { get; set; }
        public string EligibilitySOAPBodyErrorCode { get; set; }
        public string EligibilitySOAPBodyErrorMsg { get; set; }
        public string EligibilitySOAPHdrRespCode { get; set; }
        public string EligibilitySOAPHdrRespMsg { get; set; }
        public int EligibilityFailureCount { get; set; }
        public DateTime EligibilityQueryRunDate { get; set; }
        public string EligibilityFailureCountConcat { get; set; }
    }


    public class HOSPICE_SUMMARY
    {
        public string HospiceRequestType { get; set; }
        public int HospiceSuccessCount { get; set; }
        public int HospiceBusinessFailureCount { get; set; }
        public int HospiceTechnicalFailureCount { get; set; }
        public DateTime HospiceQueryRunDate { get; set; }
        public string Blank { get; set; }
    }

    public class HOSPICE_ERROR_DETAILS
    {
        public string HospiceTransactionType { get; set; }
        public string HospiceSOAPBodyErrorCode { get; set; }
        public string HospiceSOAPBodyErrorMsg { get; set; }
        public string HospiceSOAPHdrRespCode { get; set; }
        public string HospiceSOAPHdrRespMsg { get; set; }
        public int HospiceFailureCount { get; set; }
        public DateTime HospiceQueryRunDate { get; set; }
        public string HospiceFailureCountConcat { get; set; }
    }


    public class PROV_FINC_SUMMARY
    {
        public string PFRequestType { get; set; }
        public int PFSuccessCount { get; set; }
        public int PFBusinessFailureCount { get; set; }
        public int PFTechnicalFailureCount { get; set; }
        public DateTime PFQueryRunDate { get; set; }
        public string Blank { get; set; }
    }

    public class PROV_FINC_ERROR_DETAILS
    {
        public string PFTransactionType { get; set; }
        public string PFSOAPHdrRespCode { get; set; }
        public string PFSOAPHdrRespMsg { get; set; }
        public int PFFailureCount { get; set; }
        public DateTime PFQueryRunDate { get; set; }
        public string PFFailureCountConcat { get; set; }
    }

    public class EVV_SUMMARY
    {
        public string EVVResponseType { get; set; }
        public string EVVResponseMessage { get; set; }
        public int EVVSuccessCount { get; set; }
        public int EVVFailureCount { get; set; }
        public DateTime QueryRunDate { get; set; }
    }

    public class PROV_MGMT_SUMMARY
    {
        public string TransactionType { get; set; }
        public int SISuccessCount { get; set; }
        public int SIFailureCount { get; set; }
        public int MITSSuccessCount { get; set; }
        public int MITSFailureCount { get; set; }
        public int EVVSuccessCount { get; set; }
        public int EVVFailureCount { get; set; }
        public int SPBMSuccessCount { get; set; }
        public int SPBMFailureCount { get; set; }
        public int FISuccessCount { get; set; }
        public int FIFailureCount { get; set; }
        public DateTime QueryRunDate { get; set; }
        public string Blank { get; set; }
    }

    public class PROV_MGMT_ERROR_DETAILS
    {
        public string TransactionType { get; set; }
        public string SIResponseType { get; set; }
        public string SIResponseMessage { get; set; }
        public string MITSResponseType { get; set; }
        public string MITSResponseMessage { get; set; }
        public string EVVResponseType { get; set; }
        public string EVVResponseMessage { get; set; }
        public string SPBMResponseType { get; set; }
        public string SPBMResponseMessage { get; set; }
        public string FIResponseType { get; set; }
        public string FIResponseMessage { get; set; }
        public int SISuccessCount { get; set; }
        public int SIFailureCount { get; set; }
        public int FISuccessCount { get; set; }
        public int FIFailureCount { get; set; }
        public int EVVSuccessCount { get; set; }
        public int EVVFailureCount { get; set; }
        public int SPBMSuccessCount { get; set; }
        public int SPBMFailureCount { get; set; }
        public int MITSSuccessCount { get; set; }
        public int MITSFailureCount { get; set; }
        public DateTime PMQueryRunDate { get; set; }
    }


    public class ATTACHMENT_CLAIMS_SUMMARY
    {
        public int ClaimsAttachments { get; set; }
    }

    public class ATTACHMENT_PA_SUMMARY
    {
        public int PAAttachments { get; set; }
    }

    public class TransactionQueue
    {
        public int RegID { get; set; }
        public string TransactionType { get; set; }
        public string SITransactionKey { get; set; }
        public DateTime TransactionCreatedDate { get; set; }
        public string SIResponseCode { get; set; }
        public string SIResponseType { get; set; }
        public string SIResponseMessage { get; set; }
        public string SIAckResponseCode { get; set; }
        public string SIAckResponseType { get; set; }
        public string SIAckResponseMessage { get; set; }
        public string MITSAckResponseCode { get; set; }
        public string MITSAckResponseType { get; set; }
        public string MITSAckResponseMessage { get; set; }
        public string SPBMAckResponseCode { get; set; }
        public string SPBMAckResponseType { get; set; }
        public string SPBMAckResponseMessage { get; set; }
        public string FIAckResponseCode { get; set; }
        public string FIAckResponseType { get; set; }
        public string FIAckResponseMessage { get; set; }
        public string EVVAckResponseCode { get; set; }
        public string EVVAckResponseType { get; set; }
        public string EVVAckResponseMessage { get; set; }
    }

    public class UploadAttachment
    {
        public string DocumentName { get; set; }
        public string SI_TRANSACTION_KEY {  get; set; }
        public string TransactionType { get; set; }
        public string Billing_Service_Type { get; set; }
    }
    public class DailyUploadAttachment
    {
        public string Category { get; set; }
        public int SentCount { get; set; }
        public int NotSentCount { get; set; }
        public int MaliciousCount { get; set; }
        public int TotalAttachments { get; set; }
    }
    public class UploadAttachmentDetails
    {
        public List<UploadAttachment> UploadAttachment { get; set; }
    }
    public class DailyUploadAttachmentDetails
    {
        public List<DailyUploadAttachment> DailyUploadAttachment { get; set; }
    }
}
