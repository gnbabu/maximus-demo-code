using System;
using System.Collections.Generic;

namespace Models.Hospice
{
    public class HospiceResponse
    { 
        public string HospiceTrackNo { get; set; }
        public string RecipID { get; set; }
        public string ActionType { get; set; }
        public DateTime ConsBirthDate { get; set; }
        public string ConsFirstName { get; set; }
        public string ConsLastName { get; set; }
        public List<ServiceCountyState> ServiceCountyStates { get; set; }
        public List<ElectionDisenrollDates> ElectionDisenrollDatesList { get; set; }
        public List<LongTermCareFacility> LongTermCareFacilities { get; set; }
        public BenefitPeriods BenefitPeriods { get; set; }
        public List<DiagnosisCodes> DiagnosisCodesList { get; set; }
        public List<OtherPayerInfo> OtherPayerInfoList { get; set; }
        public List<ProvService> ProvServices { get; set; }
        public List<Attachments> AttachmentsList { get; set; }
        public List<EpisodeOfCare> EpisodeOfCares { get; set; }
    }
}
