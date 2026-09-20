using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Corp.Core.Libraries.HospiceService;
namespace Corp.Core.Libraries.ServiceAgentModels
{
    public class HospiceAddUpdateRequest
    {
        public int HospiceTrackNo { get; set; }
        public string RecipID { get; set; }
        public string ActionType { get; set; }
        public System.DateTime ConsBirthDate { get; set; }
        public string ConsFirstName { get; set; }
        public string ConsLastName { get; set; }
        public HospiceRequestResponseServiceCountyState[] ServiceCountyState { get; set; }
        public HospiceRequestResponseElectionDisenrollDates[] ElectionDisenrollDates { get; set; }
        public HospiceRequestResponseLongTermCareFacility[] LongTermCareFacility { get; set; }
        public HospiceRequestResponseBenefitPeriods[] BenefitPeriods { get; set; }
        public HospiceRequestResponseDiagnosisCodes[] DiagnosisCodes { get; set; }
        public HospiceRequestResponseOtherPayerInfo[] OtherPayerInfo { get; set; }
        public HospiceRequestResponseProvService[] ProvService { get; set; }
        public HospiceRequestResponseAttachments[] Attachments { get; set; }
        public AddUpdateResponseResponse[] AddUpdateResponses { get; set; }
    }
}
