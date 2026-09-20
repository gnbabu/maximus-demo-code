using System.Collections.Generic;

namespace Models.Hospice
{
    public class BenefitPeriods
    {
        public List<BenefitPeriod> BenefitPeriodes { get; set; }
        public List<IDGPhysician> IDGPhysicianes { get; set; }
        public List<AttendingPhysician> AttendingPhysicianes { get; set; }
    }
}
